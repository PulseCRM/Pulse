using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Common;
using Utilities;
using System.Data;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;


    public partial class AssignProspect : BasePage
    {
        int iProspectID = 0;
        LoginUser CurrentUser;
        private string sErrorMsg = "Failed to prospect current page: invalid ProspectID.";
        int oldUserID = 0;
        int oldRoleID = 0;
        //int iUserID = 0;
        /// <summary>
        /// Gets or sets the current file id.
        /// </summary>
        /// <value>The current file id.</value>
        protected int CurrentProspectId
        {
            set
            {
                ViewState["prospectId"] = value;
            }
            get
            {
                if (ViewState["prospectId"] == null)
                    return 0;
                int prospectId = 0;
                int.TryParse(ViewState["prospectId"].ToString(), out prospectId);

                return prospectId;
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

            if (Request.QueryString["prospectID"] != null) // 如果有prospectID
            {
                string sProspectID = Request.QueryString["prospectID"];
            
                CurrentProspectId = Convert.ToInt32(sProspectID);
            }

            if (Request.QueryString["UserID"] != null) // 如果有UserID
            {
                oldUserID = Convert.ToInt32(Request.QueryString["UserID"]);
              
            }

            if (!IsPostBack)
            {
                Contacts contact = new Contacts();
                LPWeb.Model.Contacts cs = (LPWeb.Model.Contacts)contact.GetModel(CurrentProspectId);
                lbClient.Text = cs.LastName + "," + cs.FirstName + " " + cs.MiddleName;
                BindUsers();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.CurrentProspectId == 0)
            {
                return;
            }
            try
            {
                ReassignProspect();
                LPWeb.BLL.Prospect bProspect = new Prospect();

                bProspect.AssignProspect(CurrentProspectId, Convert.ToInt32(ddlUsers.SelectedItem.Value), oldUserID);

                //PageCommon.WriteJsEnd(this, "Assign Prospect Successfully", PageCommon.Js_RefreshParent);
                PageCommon.RegisterJsMsg(this, "Assign prospect successfully!", "parent.DialogAssignClose();");

            }
            catch (System.ServiceModel.EndpointNotFoundException ex)
            {
                LPLog.LogMessage(ex.Message);
                PageCommon.WriteJsEnd(this, "Failed to assign prospect, reason: Point Manager is not running.", PageCommon.Js_RefreshSelf);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
                PageCommon.WriteJsEnd(this, string.Format("Failed to assign prospect, reason:{0}.", exception.Message), PageCommon.Js_RefreshSelf);
            }

        }
        /// <summary>
        /// Bind UserDataSource
        /// </summary>
        private void BindUsers()
        { 
            Users user = new Users();
            //Modify by Rocky (2011/11/8)
            //Get Loan Officer base on Current user id
            //DataSet DsUser = user.GetUserBranchOthersLoanOfficerUserInfo(oldUserID);
            DataSet DsUser = user.GetUserBranchOthersLoanOfficerUserInfo(CurrentUser.iUserID);
            if ((DsUser != null) && (DsUser.Tables[0].Rows.Count > 0))
            {
                DataView dvUser = new DataView(DsUser.Tables[0], "", "FullName", DataViewRowState.CurrentRows);
                ddlUsers.DataSource = dvUser;
                ddlUsers.DataBind();
            }
            
        }


        private bool AllowReassignProspect()
        {
            bool EnableMarketing = false;

            if (oldUserID == 0 || Convert.ToInt32(ddlUsers.SelectedItem.Value) == oldUserID)
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

        private void ReassignProspect()
        {
            if (!AllowReassignProspect())
            {
                return;
            }
            ServiceManager sm = new ServiceManager();

            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                ReassignProspectRequest rpq = new ReassignProspectRequest();
                rpq.hdr = new ReqHdr();
                rpq.FileId = null;// new int[1] { 0 };//????
                rpq.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                rpq.hdr.UserId = CurrentUser.iUserID;
                rpq.FromUser = oldUserID;
                rpq.ToUser = Convert.ToInt32(ddlUsers.SelectedItem.Value);
                rpq.ContactId = new int[1] {CurrentProspectId};
                rpq.UserId = null;
                ReassignProspectResponse rpp = null;
                rpp = service.ReassignProspect(rpq);

                if (!rpp.hdr.Successful)
                {
                    PageCommon.AlertMsg(this, rpp.hdr.StatusInfo);
                }
            }
        }
    }

