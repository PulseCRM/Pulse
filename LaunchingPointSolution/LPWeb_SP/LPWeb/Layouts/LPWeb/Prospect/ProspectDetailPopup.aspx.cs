using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;
using System.Text;

public partial class Prospect_ProspectDetailPopup : BasePage
{
    int iProspectID = 0;
    string sCloseDialogCodes = string.Empty;
    string sRefreshCodes = string.Empty;
    string sRefreshTabCodes = string.Empty;

    private LoginUser _curLoginUser = new LoginUser();

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 检查页面参数

        // CloseDialogCodes
        bool bIsValid = PageCommon.ValidateQueryString(this, "CloseDialogCodes", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.parent.location.href=window.parent.location.href");
        }
        this.sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";

        // RefreshCodes
        bIsValid = PageCommon.ValidateQueryString(this, "RefreshCodes", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.parent.location.href=window.parent.location.href");
        }
        this.sRefreshCodes = this.Request.QueryString["RefreshCodes"].ToString() + ";";
        if (PageCommon.ValidateQueryString(this, "RefreshCodes", QueryStringType.String))
        {
            this.sRefreshTabCodes = this.Request.QueryString["RefreshTab"].ToString().Trim() == "" ? "" : (this.Request.QueryString["RefreshTab"].ToString().Trim() + ";");
        }

        // ProspectID
        bIsValid = PageCommon.ValidateQueryString(this, "ProspectID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", this.sCloseDialogCodes);
        }
        this.iProspectID = Convert.ToInt32(this.Request.QueryString["ProspectID"]);

        #endregion

        //权限验证 
        if (_curLoginUser.userRole.Prospect.ToString().IndexOf('H') == -1) // UpdatePoint
        {
            btnUpdatePoint.Enabled = false;
        }

        #region 加载Contact信息

        Contacts ContactManager = new Contacts();
        DataTable ContactInfo = null;
        try
        {
            ContactInfo = ContactManager.GetContactInfo(this.iProspectID);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Exception happened when get contact info (ContactID={0}): {1}", this.iProspectID, ex.Message);
            LPLog.LogMessage(LogType.Logerror, ex.Message);
            PageCommon.WriteJsEnd(this, "Exception happened when get contact info.", this.sCloseDialogCodes);
        }

        if (ContactInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid contact id.", this.sCloseDialogCodes);
        }

        #endregion

        #region 加载Prospect信息

        Prospect ProspectManager = new Prospect();
        DataTable ProspectInfo = null;
        try
        {
            ProspectInfo = ProspectManager.GetProspectInfo(this.iProspectID);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Exception happened when get prospect info (ContactID={0}): {1}", this.iProspectID, ex.Message);
            LPLog.LogMessage(LogType.Logerror, ex.Message);
            PageCommon.WriteJsEnd(this, "Exception happened when get prospect info.", this.sCloseDialogCodes);
        }

        if (ProspectInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid prospect id.", this.sCloseDialogCodes);
        }



        #region 加载Create By

        Users UserManager = new Users();
        string sCreatedBy = ProspectInfo.Rows[0]["CreatedBy"].ToString();
        int iCreatedBy = 0;  //Convert.ToInt32(sCreatedBy);

        if (sCreatedBy != string.Empty)
        {
            iCreatedBy =Convert.ToInt32(sCreatedBy);
            DataTable CreatedByInfo = UserManager.GetUserInfo(iCreatedBy);
            if (CreatedByInfo.Rows.Count == 0)
            {
                this.lbCreatedBy.Text = string.Empty;
            }
            else
            {
                this.lbCreatedBy.Text = CreatedByInfo.Rows[0]["LastName"].ToString() + ", " + CreatedByInfo.Rows[0]["FirstName"].ToString();
            }
        }
        else
        {
            this.lbCreatedBy.Text = string.Empty;
        }

        #endregion

        #region 加载Create By

        string sModifiedBy = ProspectInfo.Rows[0]["ModifiedBy"].ToString();

        if (sModifiedBy != string.Empty)
        {
            DataTable ModifiedByInfo = UserManager.GetUserInfo(Convert.ToInt32(sModifiedBy));
            if (ModifiedByInfo.Rows.Count == 0)
            {
                this.lbModifiedBy.Text = string.Empty;
            }
            else
            {
                this.lbModifiedBy.Text = ModifiedByInfo.Rows[0]["LastName"].ToString() + ", " + ModifiedByInfo.Rows[0]["FirstName"].ToString();
            }
        }
        else
        {
            this.lbModifiedBy.Text = string.Empty;
        }

        #endregion

        #endregion

        // Old Status
        this.hdnOldStatus.Value = ProspectInfo.Rows[0]["Status"].ToString();

        if (this.IsPostBack == false)
        {
            USStates.Init(ddlState);
            #region 加载Loan Officer列表

            DataTable LoanOfficerList = this.GetLoanOfficerList(iCreatedBy);
            this.ddlLoanOfficer.DataSource = LoanOfficerList;
            this.ddlLoanOfficer.DataBind();

            #endregion

            #region 加载Lead Source列表

            Company_Lead_Sources LeadSourceManager = new Company_Lead_Sources();
            DataTable LeadSourceList = LeadSourceManager.GetList("1=1 order by LeadSource").Tables[0];

            DataRow NewLeadSourceRow = LeadSourceList.NewRow();
            NewLeadSourceRow["LeadSourceID"] = DBNull.Value;
            NewLeadSourceRow["LeadSource"] = "-- select --";

            LeadSourceList.Rows.InsertAt(NewLeadSourceRow, 0);

            this.ddlLeadSource.DataSource = LeadSourceList;
            this.ddlLeadSource.DataBind();

            #endregion

            #region 绑定Prospect数据

            // Loan Officer
            this.ddlLoanOfficer.SelectedValue = ProspectInfo.Rows[0]["LoanOfficer"].ToString();

            hfLoanOfficer.Value = ProspectInfo.Rows[0]["LoanOfficer"].ToString();
            this.txtRefCode.Text = ProspectInfo.Rows[0]["ReferenceCode"].ToString();
            this.ddlStatus.SelectedValue = ProspectInfo.Rows[0]["Status"].ToString();
            ListItem item = this.ddlCreditRanking.Items.FindByValue(string.Format("{0}", ProspectInfo.Rows[0]["CreditRanking"]));
            if (null != item)
            {
                this.ddlCreditRanking.ClearSelection();
                item.Selected = true;
            }
            item = this.ddlPreferredContact.Items.FindByValue(string.Format("{0}", ProspectInfo.Rows[0]["PreferredContact"]));
            if (null != item)
            {
                this.ddlPreferredContact.ClearSelection();
                item.Selected = true;
            }
            this.ddlLeadSource.SelectedValue = ProspectInfo.Rows[0]["LeadSource"].ToString();

            this.lbCreatedOn.Text = ProspectInfo.Rows[0]["Created"].ToString();

            this.lbModifiedOn.Text = ProspectInfo.Rows[0]["Modifed"].ToString();

            //Get referral contact name
            int iReferralId = 0;
            if (ProspectInfo.Rows[0]["Referral"].ToString() != "" && int.TryParse(ProspectInfo.Rows[0]["Referral"].ToString(),out iReferralId))
            {
                this.txbReferral.Text = ContactManager.GetContactName(iReferralId);
                this.hdnReferralID.Value = iReferralId.ToString();
            }

            #endregion

            #region 绑定Contact数据

            this.txtFirstName.Text = ContactInfo.Rows[0]["FirstName"].ToString();
            this.txtMiddleName.Text = ContactInfo.Rows[0]["MiddleName"].ToString();
            this.txtLastName.Text = ContactInfo.Rows[0]["LastName"].ToString();
            this.ddlTitle.SelectedValue = ContactInfo.Rows[0]["Title"].ToString();
            this.txtGenerationCode.Text = ContactInfo.Rows[0]["GenerationCode"].ToString();
            this.txtSSN.Text = ContactInfo.Rows[0]["SSN"].ToString();
            this.txtHomePhone.Text = ContactInfo.Rows[0]["HomePhone"].ToString();
            this.txtCellPhone.Text = ContactInfo.Rows[0]["CellPhone"].ToString();
            this.txtBizPhone.Text = ContactInfo.Rows[0]["BusinessPhone"].ToString();
            this.txtFax.Text = ContactInfo.Rows[0]["Fax"].ToString();
            this.txtAddress.Text = ContactInfo.Rows[0]["MailingAddr"].ToString();
            this.txtCity.Text = ContactInfo.Rows[0]["MailingCity"].ToString();
            this.ddlState.SelectedValue = ContactInfo.Rows[0]["MailingState"].ToString();
            this.txtZip.Text = ContactInfo.Rows[0]["MailingZip"].ToString();
            this.txtEmail.Text = ContactInfo.Rows[0]["Email"].ToString();
            if (ContactInfo.Rows[0]["DOB"] == DBNull.Value)
            {
                this.txtDOB.Text = string.Empty;
            }
            else 
            {
                this.txtDOB.Text = Convert.ToDateTime(ContactInfo.Rows[0]["DOB"]).ToShortDateString();
            }
            this.txtExperianScore.Text = ContactInfo.Rows[0]["Experian"].ToString();
            this.txtTransUnitScore.Text = ContactInfo.Rows[0]["TransUnion"].ToString();
            this.txtEquifaxScore.Text = ContactInfo.Rows[0]["Equifax"].ToString();



            #endregion
        }
    }

    private bool AllowReassignProspect()
    {
        bool EnableMarketing = false;

        if (hfLoanOfficer.Value.Length == 0 || ddlLoanOfficer.SelectedItem.Text == hfLoanOfficer.Value)
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
            rpq.FileId = null;
            rpq.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            rpq.hdr.UserId = _curLoginUser.iUserID;
            rpq.FromUser = int.Parse(hfLoanOfficer.Value);
            rpq.ToUser = int.Parse(ddlLoanOfficer.SelectedValue);
            rpq.ContactId = new int[1] { iProspectID };
            rpq.UserId = null;

            ReassignProspectResponse rpp = null;
            rpp = service.ReassignProspect(rpq);

            if (!rpp.hdr.Successful)
            {
                PageCommon.AlertMsg(this, rpp.hdr.StatusInfo);
            }
        }
        catch(Exception ex)
        {
            LPLog.LogMessage(LogType.Logerror, ex.Message);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region 获取用户输入

        string sLoanOfficerID = this.ddlLoanOfficer.SelectedValue;
        int iLoanOfficerID = Convert.ToInt32(sLoanOfficerID);

        string sReferenceCode = this.txtRefCode.Text.Trim();
        string sStatus = this.ddlStatus.SelectedValue;
        string strCreditRanking = this.ddlCreditRanking.SelectedValue;
        string strPreferredContact = this.ddlPreferredContact.SelectedValue;
        
        string sLeadSource = this.ddlLeadSource.SelectedValue;
        if (sLeadSource == "-- select --")
        {
            sLeadSource = string.Empty;
        }

        string sFirstName = this.txtFirstName.Text.Trim();
        string sMiddleName = this.txtMiddleName.Text.Trim();
        string sLastName = this.txtLastName.Text.Trim();
        string sTitle = this.ddlTitle.SelectedValue;
        string sGenerationCode = this.txtGenerationCode.Text.Trim();
        string sSSN = this.txtSSN.Text.Trim();
        string sHomePhone = this.txtHomePhone.Text.Trim();
        string sCellPhone = this.txtCellPhone.Text.Trim();
        string sBizPhone = this.txtBizPhone.Text.Trim();
        string sFax = this.txtFax.Text.Trim();
        string sAddress = this.txtAddress.Text.Trim();
        string sCity = this.txtCity.Text.Trim();
        string sState = this.ddlState.SelectedValue;
        string sZip = this.txtZip.Text.Trim();
        string sEmail = this.txtEmail.Text.Trim();
        string sDOB = this.txtDOB.Text.Trim();
        int iReferralID = 0;
        if (! int.TryParse(this.hdnReferralID.Value, out iReferralID))
        {
            iReferralID = 0;
        }

        string sExperianScore = this.txtExperianScore.Text.Trim();
        if (sExperianScore == string.Empty)
        {
            sExperianScore = "0";
        }
        Int16 iExperianScore = Convert.ToInt16(sExperianScore);

        string sTransUnitScore = this.txtTransUnitScore.Text.Trim();
        if (sTransUnitScore == string.Empty)
        {
            sTransUnitScore = "0";
        }
        Int16 iTransUnitScore = Convert.ToInt16(sTransUnitScore);

        string sEquifaxScore = this.txtEquifaxScore.Text.Trim();
        if (sEquifaxScore == string.Empty)
        {
            sEquifaxScore = "0";
        }
        Int16 iEquifaxScore = Convert.ToInt16(sEquifaxScore);

        #endregion

        #region 检查LoanContacts

        string sSql = "select * from LoanContacts where ContactID=" + this.iProspectID;
        DataTable RefLoanIDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        if (RefLoanIDs.Rows.Count > 0) 
        {
            #region UpdateBorrowerRequest

            UpdateBorrowerResponse response = null;

            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient service = sm.StartServiceClient())
                {
                    UpdateBorrowerRequest req = new UpdateBorrowerRequest();
                    ReqHdr hdr = new ReqHdr();
                    hdr.UserId = this.CurrUser.iUserID;
                    req.hdr = hdr;
                    req.ContactId = this.iProspectID;

                    response = service.UpdateBorrower(req);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                string sExMsg = string.Format("Exception happened when send UpdateBorrowerRequest to Point Manager (ContactID={0}): {1}", this.iProspectID, "Point Manager is not running.");
                LPLog.LogMessage(LogType.Logerror, sExMsg);

                // update Contacts.UpdatePoint=1
                this.UpdatePoint(this.iProspectID, true);

                PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
            }
            catch (Exception ex)
            {
                string sExMsg = string.Format("Exception happened when send UpdateBorrowerRequest to Point Manager (ContactID={0}): {1}", this.iProspectID, ex.Message);
                LPLog.LogMessage(LogType.Logerror, sExMsg);

                // update Contacts.UpdatePoint=1
                this.UpdatePoint(this.iProspectID, true);

                PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
            }

            if (response.hdr.Successful == false)
            {
                string sFailedMsg = "Failed to send UpdateBorrowerRequest to Point Manager: " + response.hdr.StatusInfo;
                LPLog.LogMessage(LogType.Logerror, sFailedMsg);

                // update Contacts.UpdatePoint=1
                this.UpdatePoint(this.iProspectID, true);

                PageCommon.WriteJsEnd(this, sFailedMsg, PageCommon.Js_RefreshSelf);
            }
            else
            {
                // update Contacts.UpdatePoint=0
                this.UpdatePoint(this.iProspectID, false);
            }

            #endregion
        }

        #endregion

        LoginUser CurrentUser = new LoginUser();
        Prospect ProspectManager = new Prospect();

        // Update prospect detail
        ProspectManager.UpdateProspectDetail(this.iProspectID, sLeadSource, sReferenceCode, CurrentUser.iUserID, iLoanOfficerID, sStatus, strCreditRanking, strPreferredContact, sFirstName, sMiddleName, sLastName, sTitle, sGenerationCode, sSSN, sHomePhone, sCellPhone, sBizPhone, sFax, sEmail, sDOB, iExperianScore, iTransUnitScore, iEquifaxScore, sAddress, sCity, sState, sZip, iReferralID);

        // build js
        StringBuilder sbJavaScript = new StringBuilder();
        sbJavaScript.AppendLine("$('#divContainer').hide();");
        sbJavaScript.AppendLine("alert('Save client detail successfully.');");

        #region Has associated Prospect Loan

        bool bHasProspectLoan = this.HasProspectLoan(this.iProspectID);
        string sOldStatus = this.hdnOldStatus.Value;
        string sNewStatus = this.ddlStatus.SelectedValue;
        if (sNewStatus == "Suspended")
        {
            sNewStatus = "Suspend";
        }

        #endregion

        if (bHasProspectLoan == true && sOldStatus == "Active"
            && (sStatus == "Bad" || sStatus == "Lost" || sStatus == "Suspended"))  // has prospect loan
        {
            sbJavaScript.AppendLine("var result = confirm('Do you want to also change the status of the associated leads to <" + sStatus + ">?')");

            sbJavaScript.AppendLine("if(result == false){" + this.sCloseDialogCodes + this.sRefreshCodes + "}else{ShowDialog_PointFolderSelection('" + this.iProspectID + "', '" + sNewStatus + "');}");
        }
        else {

            sbJavaScript.AppendLine(this.sCloseDialogCodes + this.sRefreshCodes);
        }
        btnUpdatePoint_Click(this, e);

        //Fixed BUG111, Refresh contact tab
        if (sRefreshTabCodes != string.Empty && sRefreshTabCodes != "")
        {
            sbJavaScript.AppendLine(sRefreshTabCodes);
        }

        // success
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", sbJavaScript.ToString(), true);

    }

    protected void btnUpdatePoint_Click(object sender, EventArgs e)
    {
        #region send UpdateBorrowerRequest to point manager

        UpdateBorrowerResponse response = null;

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                ReassignProspect(service);
                UpdateBorrowerRequest req = new UpdateBorrowerRequest();
                ReqHdr hdr = new ReqHdr();
                hdr.UserId = this.CurrUser.iUserID;
                req.hdr = hdr;
                req.ContactId = this.iProspectID;

                response = service.UpdateBorrower(req);
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("Exception occurred while trying to send UpdateBorrowerRequest to Point Manager (ContactID={0}): {1}", this.iProspectID, "Point Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);

            // update Contacts.UpdatePoint=1
            this.UpdatePoint(this.iProspectID, true);

            PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Exception occured while trying to send UpdateBorrowerRequest to Point Manager (ContactID={0}): {1}", this.iProspectID, ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);

            // update Contacts.UpdatePoint=1
            this.UpdatePoint(this.iProspectID, true);

            PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
        }

        #endregion

        if (response.hdr.Successful == true)
        {
            // update Contacts.UpdatePoint=0
            this.UpdatePoint(this.iProspectID, false);
            PageCommon.WriteJsEnd(this, "Updated Point successfully.", this.sRefreshCodes + this.sRefreshTabCodes + this.sCloseDialogCodes);
        }
        else
        {
            string sFailedMsg = response.hdr.StatusInfo;
            LPLog.LogMessage(LogType.Logerror, sFailedMsg);

            // update Contacts.UpdatePoint=1
            this.UpdatePoint(this.iProspectID, true);

            PageCommon.WriteJsEnd(this, sFailedMsg, PageCommon.Js_RefreshSelf);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        // build js
        StringBuilder sbJavaScript = new StringBuilder();
        try
        {
            Prospect ProspectManager = new Prospect();
            ProspectManager.Delete(this.iProspectID);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Exception occurred while trying to delete the client record (ContactID={0}): {1}", this.iProspectID, ex.Message);
            LPLog.LogMessage(LogType.Logerror, ex.Message);
            PageCommon.WriteJsEnd(this, "Exception occurred while trying to delete the client record.", PageCommon.Js_RefreshSelf);
        }

        PageCommon.WriteJsEnd(this, "Deleted the client record successfully.", this.sCloseDialogCodes + "window.parent.parent.location.href='../Pipeline/ProspectPipelineSummary.aspx';");
    }

    /// <summary>
    /// get loan officer list
    /// neo 2011-03-13
    /// </summary>
    /// <param name="iUserID"></param>
    /// <returns></returns>
    private DataTable GetLoanOfficerList(int iUserID)
    {
        // get branch count
        //string sSql0 = "select count(1) from lpfn_GetUserBranches_UserList(" + iUserID + ")";
        //int iCount = Convert.ToInt32(LPWeb.DAL.DbHelperSQL.ExecuteScalar(sSql0));

        // all loan officer
        //string sSql = "select a.UserID, a.FirstName +', '+ a.LastName as FullName "
        //            + "from Users as a "
        //            + "inner join GroupUsers as b on a.UserId = b.UserID "
        //            + "inner join Groups as c on b.GroupID = c.GroupId "
        //            + "where c.Enabled = 1 and a.UserEnabled=1 and (a.RoleId = 3 or a.RoleId = 1)";

        // if belong to branch, get loan officer under same branch
        //if (iCount > 0)
        //{
        //    sSql += " and (c.BranchId in (select * from lpfn_GetUserBranches_UserList(" + iUserID + ")))";
        //}

        string sSql = "select UserID, FirstName +' '+ LastName as FullName from Users where UserEnabled=1 and (RoleId = 3 or RoleId = 1) order by FirstName asc";

        DataTable LoanOfficerList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        DataRow NewLORow = LoanOfficerList.NewRow();
        NewLORow["UserID"] = DBNull.Value;
        NewLORow["FullName"] = "-- select --";
        LoanOfficerList.Rows.InsertAt(NewLORow, 0);

        return LoanOfficerList;
    }

    /// <summary>
    /// check wether or not current contact has prospect loan
    /// neo 2011-03-13
    /// </summary>
    /// <param name="iContactID"></param>
    /// <returns></returns>
    private bool HasProspectLoan(int iContactID)
    {
        string sSql = "select count(1) from LoanContacts as a inner join Loans as b on a.FileId = b.FileId "
                    + "where a.ContactId=" + iContactID + " and (a.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or a.ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId()) and b.Status='Prospect'";
        int iCount = Convert.ToInt32(LPWeb.DAL.DbHelperSQL.ExecuteScalar(sSql));

        if (iCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// update Contacts.UpdatePoint= 0 or 1
    /// neo 2011-03-13
    /// </summary>
    /// <param name="iContactID"></param>
    /// <param name="bUpdatePoint"></param>
    private void UpdatePoint(int iContactID, bool bUpdatePoint) 
    {
        Contacts ContactManager = new Contacts();

        try
        {
            ContactManager.UpdatePoint(this.iProspectID, false);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Failed to update the client record (ContactID={0}): {1}", this.iProspectID, ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
        }
    }
}