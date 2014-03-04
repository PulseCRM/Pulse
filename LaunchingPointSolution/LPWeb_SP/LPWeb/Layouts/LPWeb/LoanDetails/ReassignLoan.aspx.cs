using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.BLL;
using Utilities;
using System.Data;
using LPWeb.LP_Service;


public partial class ReassignLoan : BasePage
{
    int iLoanID = 0;
    LoginUser CurrentUser;
    private string sErrorMsg = "Failed to load current page: invalid FileID.";
    private string sReturnPage = "LoanReassignContactTab.aspx";
    int oldUserID = 0;
    int oldRoleID = 0;
    //int iUserID = 0;
    /// <summary>
    /// Gets or sets the current file id.
    /// </summary>
    /// <value>The current file id.</value>
    protected int CurrentFileId
    {
        set
        {
            ViewState["fileId"] = value;
        }
        get
        {
            if (ViewState["fileId"] == null)
                return 0;
            int fileId = 0;
            int.TryParse(ViewState["fileId"].ToString(), out fileId);

            return fileId;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            CurrentUser = new LoginUser();
            //loginUser.ValidatePageVisitPermission("LoanSetup");
            //权限验证
            if (!CurrentUser.userRole.AssignLoan)
            {
                Response.Redirect("../Unauthorize.aspx");
                return;
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

        if (Request.QueryString["FileID"] != null) // 如果有FileID
        {
            string sFileID = Request.QueryString["FileID"];

            if (PageCommon.IsID(sFileID) == false)
            {
                //PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
            }

            CurrentFileId = Convert.ToInt32(sFileID);
        }
        if (Request.QueryString["uandr"] != null) // 如果有userid and roleid
        {
            string sUandRID = Request.QueryString["uandr"].Replace("User", "");
            try
            {
                oldUserID = Convert.ToInt32(sUandRID.Split("_".ToCharArray())[0]);
                oldRoleID = Convert.ToInt32(sUandRID.Split("_".ToCharArray())[1]);
            }
            catch
            { }
        }

        if (!IsPostBack)
        {
            BindControls();
            BindRoles();
            this.hdnCloseDialogCodes.Value = "";
            this.hdnCloseDialogCodes.Value = this.Request.QueryString["CloseDialogCodes"];
        }
    }

    void BindControls()
    {
        try
        {
            Contacts contact = new Contacts();
            lbBorrower.Text = contact.GetBorrower(CurrentFileId);

            LPWeb.Model.Loans model = new LPWeb.Model.Loans();
            Loans loans = new Loans();
            model = loans.GetModel(CurrentFileId);
            if (model == null)
            { return; }

            //lbPointFile.Text
            lbProperty.Text = model.PropertyAddr + " " + model.PropertyCity + " " + model.PropertyState + " " + model.PropertyZip;
        }
        catch
        { }
    }
    void BindRoles()
    {
        try
        {
            Roles role = new Roles();
            //DataSet ds = role.GetList(" Name <> 'Executive' ");
            DataSet ds = role.GetList(" (1=1) order by [Name] asc");
            ddlRole.DataSource = ds;
            ddlRole.DataBind();
        }
        catch
        { }
    }

    private bool AllowReassignProspect()
    {
        bool EnableMarketing = false;
        if (ddlRole.SelectedItem.Text != "Loan Officer")
        {
            return EnableMarketing;
        }

        try
        {
            LPWeb.BLL.Company_General cg = new LPWeb.BLL.Company_General();
            EnableMarketing = cg.CheckMarketingEnabled();
        }
        catch
        { }

        return EnableMarketing;
    }

    private void ReassignProspect(LP2ServiceClient service)
    {
        if (!AllowReassignProspect())
        {
            return;
        }
        try
        {
            ReassignProspectRequest rpq = new ReassignProspectRequest();
            rpq.hdr = new ReqHdr();
            rpq.FileId = new int[1] { this.CurrentFileId };
            rpq.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            rpq.hdr.UserId = CurrentUser.iUserID;
            rpq.FromUser = oldUserID;
            rpq.ToUser = int.Parse(ddlUsers.SelectedValue);
            rpq.ContactId = null;
            rpq.UserId = null;
            ReassignProspectResponse rpp = null;
            rpp = service.ReassignProspect(rpq);

            if (!rpp.hdr.Successful)
            {
                try
                {
                    PageCommon.AlertMsg(this, rpp.hdr.StatusInfo);
                }
                catch
                { }
            }
        }
        catch
        { }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.CurrentFileId == 0)
        {
            return;
        }
        ServiceManager sm = new ServiceManager();
        bool bSuccessful = false;
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            ReassignLoanRequest req = new ReassignLoanRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = CurrentUser.iUserID;
            ReassignUserInfo uInfo = new ReassignUserInfo();

            uInfo.FileId = this.CurrentFileId;
            uInfo.RoleId = int.Parse(ddlRole.SelectedValue);
            if (ddlUsers.SelectedValue != "")
            {
                uInfo.NewUserId = int.Parse(ddlUsers.SelectedValue);
            }
            else
            {
                uInfo.NewUserId = 0;
            }
            List<ReassignUserInfo> uList = new List<ReassignUserInfo>();
            uList.Add(uInfo);
            req.reassignUsers = uList.ToArray();
            ReassignLoanResponse respone = null;
            try
            {
                ReassignProspect(service);

                respone = service.ReassignLoan(req);

                if (respone.hdr.Successful)
                {
                    LPWeb.Model.LoanTeam lcModel = new LPWeb.Model.LoanTeam();
                    lcModel.FileId = CurrentFileId;
                    lcModel.RoleId = uInfo.RoleId;
                    lcModel.UserId = uInfo.NewUserId;

                    LPWeb.Model.LoanTeam oldlcModel = new LPWeb.Model.LoanTeam();
                    oldlcModel.FileId = CurrentFileId;
                    oldlcModel.RoleId = oldRoleID;
                    oldlcModel.UserId = oldUserID;

                    LPWeb.BLL.LoanTeam lc = new LoanTeam();

                    //lc.Reassign(oldlcModel, lcModel, req.hdr.UserId, ddlRole.SelectedItem.Text, ddlUsers.SelectedItem.Text);
                    lc.Reassign(oldlcModel, lcModel, req.hdr.UserId);
                    
                    bSuccessful = true;

                }
                else
                {
                    bSuccessful = false;
                 }
                if (bSuccessful)
                {
                    PageCommon.WriteJsEnd(this, "Reassigned loan successfully", "window.parent.RefreshPage();" + this.hdnCloseDialogCodes.Value);
                }
                else
                {
                    PageCommon.WriteJsEnd(this, string.Format("Failed to reassign loan, reason:{0}.", respone.hdr.StatusInfo), PageCommon.Js_RefreshSelf);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ex)
            {
                LPLog.LogMessage(ex.Message);
                PageCommon.WriteJsEnd(this, "Failed to reassign loan, reason: Point Manager is not running.", PageCommon.Js_RefreshSelf);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
                PageCommon.WriteJsEnd(this, string.Format("Failed to reassign loan, reason:{0}.", exception.Message), PageCommon.Js_RefreshSelf);
            }
        }
    }

    protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            int RoleId = int.Parse(ddlRole.SelectedValue);
            Users user = new Users();
            DataSet ds = user.GetAllBranchUser(CurrentFileId, RoleId);
            if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
            {
                ds.Dispose();
                ds = user.GetAllCompanyUserByRoleId(RoleId);
            }
            DataView dvUser = new DataView(ds.Tables[0], "", "Username", DataViewRowState.CurrentRows);
            ddlUsers.DataSource = dvUser;
            ddlUsers.DataBind();
        }
        catch
        { }
    }
}
