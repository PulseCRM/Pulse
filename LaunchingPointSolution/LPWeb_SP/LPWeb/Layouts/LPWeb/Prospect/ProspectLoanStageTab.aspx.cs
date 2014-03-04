using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;
using LPWeb.BLL;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;

public partial class ProspectLoanStageTab : BasePage
{
    private readonly LoanStages bllStages = new LoanStages();
    private const string sErrorMsg = "Failed to load current page: invalid FileID.";
    private const string sReturnPage = "ProspectLoanStageTab.aspx";

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                var loginUser = new LoginUser();
                //loginUser.ValidatePageVisitPermission("LoanSetup");
                //权限验证
                if (!loginUser.userRole.LoanSetup)
                {
                    Response.Redirect("../Unauthorize1.aspx");
                    return;
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            if (Request.QueryString["FileID"] != null)
            {
                string sFileID = Request.QueryString["FileID"];

                if (PageCommon.IsID(sFileID) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }

                CurrentFileId = Convert.ToInt32(sFileID);
            }
            if (CurrentFileId == 0)
            {
                return;
            }

            BindPage();
            ValidateButtonStatus();
        }
    }

    private void ValidateButtonStatus()
    {
        var loan = new Loans().GetModel(CurrentFileId);

        if (!string.IsNullOrEmpty(loan.ProspectLoanStatus) && loan.ProspectLoanStatus != "Active")
        {
            btnSave.Enabled = false;
        }
    }

    /// <summary>
    /// Binds the page.
    /// </summary>
    private void BindPage()
    {
        if (CurrentFileId < 1)
        { 
            return;
        }

        DataSet ds = bllStages.GetLoanStageSetupInfo(CurrentFileId);
        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        {
            gvWfGrid.DataSource = null;
            gvWfGrid.DataBind();
            return;
        }

        DateTime dtMin = DateTime.MinValue;
        if (DateTime.TryParse(ds.Tables[0].Rows[0]["EstCloseDate"].ToString(), out dtMin))
        {
            tbxTargetCloseDate.Text = dtMin.ToShortDateString();
        }
        LPWeb.BLL.Loans loanMgr = new Loans();
        LPWeb.Model.Loans loanModel = loanMgr.GetModel(CurrentFileId);
        if (loanModel.EstCloseDate != null)
        {
            tbxTargetCloseDate.Text = loanModel.EstCloseDate.Value.ToShortDateString();
        }
        if (loanModel.Created != null)
        {
            lbDateCreated.Text = loanModel.Created.Value.ToShortDateString();
        }

        gvWfGrid.DataSource = ds;
        gvWfGrid.DataBind();
    }

    /// <summary>
    /// Handles the Click event of the btnSave control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        /*Save: When clicked, if the Target Close Date is changed, an UpdateEstCloseDateRequest message will be sent to the Point Manager.
         * If the Point Manager returns an error, it will display the error and will not save the change; 
         * otherwise, it will send a CalculateDueDates message to the Workflow Manager.
         * If the Workflow Manager returns an error, it will display the error and abort the operation.
         */
        DateTime dtClose = DateTime.MinValue;
        DateTime.TryParse(tbxTargetCloseDate.Text, out dtClose);
        if (dtClose == DateTime.MinValue)
        {
            PageCommon.AlertMsg(this, "Please enter the Target Close Date");
            return;
        }
        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient client = sm.StartServiceClient())
            {
                var req = new UpdateEstCloseDateRequest();
                req.FileId = CurrentFileId;
                req.NewDate = dtClose;
                req.hdr = new ReqHdr();
                req.hdr.UserId = new LoginUser().iUserID;

                var response = client.UpdateEstCloseDate(req);
                if (!response.hdr.Successful)
                {
                    PageCommon.AlertMsg(this, response.hdr.StatusInfo);
                    return;
                }

                var req1 = new CalculateDueDatesRequest();
                req1.FileId = CurrentFileId;
                req1.NewEstCloseDate = dtClose;
                req1.hdr = new ReqHdr();
                req1.hdr.UserId = new LoginUser().iUserID;

                var response1 = client.CalculateDueDates(req1);
                if (!response1.hdr.Successful)
                {
                    PageCommon.AlertMsg(this, response1.hdr.StatusInfo);
                }
                else
                {
                    PageCommon.AlertMsg(this, "Save Successfuly!");
                    BindPage();
                }
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException ex)
        {
            PageCommon.AlertMsg(this, "Failed to save the Target Close Date, reason: Workflow Manager Service not running.");
        }
        catch (Exception ee)
        {
            PageCommon.AlertMsg(this, String.Format("Failed to save the Target Close Date, reason: {0}", ee.Message));
        }

    }

    #region Properties

    /// <summary>
    /// Gets or sets the current file id.
    /// </summary>
    /// <value>The current file id.</value>
    private int CurrentFileId
    {
        set { ViewState["fileId"] = value; }
        get
        {
            if (ViewState["fileId"] == null)
                return 0;
            int fileId = 0;
            int.TryParse(ViewState["fileId"].ToString(), out fileId);

            return fileId;
        }
    }

    #endregion
}
