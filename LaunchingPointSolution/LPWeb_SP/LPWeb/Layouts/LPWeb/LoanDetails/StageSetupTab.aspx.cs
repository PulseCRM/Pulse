using System;
using System.Data;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.LP_Service;
using Microsoft.SharePoint.WebControls;
using Utilities;
using LoanActivities = LPWeb.Model.LoanActivities;
using LPWeb.Layouts.LPWeb.Common;



public partial class StageSetupTab : LayoutsPageBase
{
    private readonly LPWeb.BLL.LoanStages bllStages = new LPWeb.BLL.LoanStages();
    private const string sErrorMsg = "Failed to load current page: invalid FileID.";
    private const string sReturnPage = "StageSetupTab.aspx";

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
        var loan = new LPWeb.BLL.Loans().GetModel(CurrentFileId);
        if(!string.IsNullOrEmpty(loan.Status) && loan.Status!="Processing")
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
            PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
            return;
        }

        DataSet ds = bllStages.GetLoanStageSetupInfo(CurrentFileId);
        if(ds==null || ds.Tables.Count==0 || ds.Tables[0].Rows.Count==0)
        {
            //PageCommon.AlertMsg(this, "There is no data in database.");
            return;
        }

        foreach (DataRow LoanStageRow in ds.Tables[0].Rows)
        {
            int iLoanStageId = Convert.ToInt32(LoanStageRow["LoanStageId"]);

            LoanStages LoanStagesMrg = new LoanStages();
            DataTable LoanStageInfo = LoanStagesMrg.GetList(" LoanStageId=" + iLoanStageId).Tables[0];
            
            if (LoanStageInfo.Rows.Count > 0)
            {
                #region 获取Template_Stage.Alias

                string sWflStageName = LoanStageInfo.Rows[0]["StageName"].ToString();
                string sDiaplayAs = LoanStageInfo.Rows[0]["StageName"].ToString();

                if (LoanStageInfo.Rows[0]["WflStageId"] != DBNull.Value)
                {
                    string sWflStageId = LoanStageInfo.Rows[0]["WflStageId"].ToString();

                    Template_Wfl_Stages Template_Wfl_Stages1 = new Template_Wfl_Stages();
                    DataTable Template_Wfl_Stages_info = Template_Wfl_Stages1.GetList(" WflStageId=" + sWflStageId).Tables[0];
                    if (Template_Wfl_Stages_info.Rows.Count > 0)
                    {
                        sWflStageName = Template_Wfl_Stages_info.Rows[0]["Name"].ToString();
                        
                        string sTemplStageId = Template_Wfl_Stages_info.Rows[0]["TemplStageId"].ToString();
                        if (sTemplStageId != string.Empty)
                        {
                            Template_Stages Template_Stages1 = new Template_Stages();
                            DataTable Template_Stage_Info = Template_Stages1.GetStageTemplateList(" and TemplStageId=" + sTemplStageId);
                            if (Template_Stage_Info.Rows.Count > 0)
                            {
                                string sAlias = Template_Stage_Info.Rows[0]["Alias"].ToString();
                                if (sAlias != string.Empty)
                                {
                                    sDiaplayAs = sAlias;
                                }
                            }
                        }
                    }
                }

                LoanStageRow["StageName"] = sWflStageName;
                LoanStageRow["Alias"] = sDiaplayAs;

                #endregion
            }
        }

        

        DateTime dtMin = DateTime.MinValue;
        if(DateTime.TryParse(ds.Tables[0].Rows[0]["EstCloseDate"].ToString(), out dtMin))
        {
            tbxTargetCloseDate.Text = dtMin.ToShortDateString();
        }
        Loans loanMgr = new Loans();
        LPWeb.Model.Loans loanModel = loanMgr.GetModel(CurrentFileId);
        if (loanModel.EstCloseDate != null)
        {
            tbxTargetCloseDate.Text = loanModel.EstCloseDate.Value.ToShortDateString();
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
