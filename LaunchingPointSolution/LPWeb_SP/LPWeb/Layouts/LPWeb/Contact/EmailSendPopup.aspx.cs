using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using System.Data;
using System.Data.SqlClient;
using LPWeb.Common;
using System.Collections.ObjectModel;
using LPWeb.LP_Service;
using System.Text;
using Utilities;


public partial class Contact_EmailSendPopup : BasePage
{
    int iLoanID = 0;
    int iProspectID = 0;
    int iProspectAlertID = 0;
    int iContactID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sCloseDialogCodes = this.GetCloseDialogJs();

        if ( this.Request.QueryString["ContactID"] == null)
        {

            this.ClientScript.RegisterStartupScript(this.GetType(), "_InvalidQuery1", "$('#divContainer').hide();alert('Missing required query string.');" + sCloseDialogCodes, true);
            return;
        }

        if (this.Request.QueryString["ContactID"] != null)
        {
            #region ContactID

            bool bIsValid = PageCommon.ValidateQueryString(this, "ContactID", QueryStringType.ID);
            if (bIsValid == false)
            {

                this.ClientScript.RegisterStartupScript(this.GetType(), "_InvalidQuery2", "$('#divContainer').hide();alert('Missing required query string.');" + sCloseDialogCodes, true);
                return;
            }
            this.iContactID = Convert.ToInt32(this.Request.QueryString["ContactID"]);

            #endregion
        }
      

        #endregion

        #region EWS

        Company_Web CompanyWebManager = new Company_Web();
        LPWeb.Model.Company_Web Company_Web_Model = CompanyWebManager.GetModel();

        Users UserManager = new Users();
        LPWeb.Model.Users UserModel = UserManager.GetModel(this.CurrUser.iUserID);

        if (Company_Web_Model.SendEmailViaEWS && UserModel.Password == string.Empty)
        {
            this.hdnUseEWS.Value = "True";
        }
        else
        {
            this.hdnUseEWS.Value = "False";
        }

        #endregion

        if (this.IsPostBack == false)
        {
            #region 加载email template

            Template_Email EmailTempManager = new Template_Email();
            DataTable EmailTemplates = EmailTempManager.GetEmailTemplate(" and Enabled = 1 ");

            this.ddlEmailTemplateList.DataSource = EmailTemplates;
            this.ddlEmailTemplateList.DataBind();

            #endregion

            #region 加载To(Dropdown List)

          if (this.Request.QueryString["ContactID"] != null)
            {
                #region ContactID

                string sSql = "select 'Contact'+convert(varchar, ContactId) as UserID, LastName +', '+ FirstName + case when ISNULL(MiddleName, '') != '' then ' '+ MiddleName else '' end as RoleAndFullName from Contacts "
                    + "where ContactId=" + this.iContactID;
                if (CurrUser.bIsCompanyUser)
                {
                    // All Contact 
                    sSql += "union select 'Contact'+convert(varchar, a.ContactId) as UserID, a.LastName +', '+ a.FirstName + case when ISNULL(a.MiddleName, '') != '' then ' '+ a.MiddleName else '' end as RoleAndFullName from Contacts as a "
                    + "where a.ContactId<>" + this.iContactID;
                }
                else if (CurrUser.bIsRegionUser)
                {
                    sSql += "union select 'Contact'+convert(varchar, a.ContactId) as UserID, a.LastName +', '+ a.FirstName + case when ISNULL(a.MiddleName, '') != '' then ' '+ a.MiddleName else '' end as RoleAndFullName from Contacts as a "
                   + "where a.ContactBranchId in (SELECT BranchId FROM Branches WHERE RegionID IN (Select RegionID from Regions where GroupID in (select GroupID from GroupUsers  where UserID=" + CurrUser.iUserID + "))) and a.[Enabled]=1 and a.ContactId<>" + this.iContactID;
                }
                else if (CurrUser.bIsDivisionUser)
                {
                    sSql += "union select 'Contact'+convert(varchar, a.ContactId) as UserID, a.LastName +', '+ a.FirstName + case when ISNULL(a.MiddleName, '') != '' then ' '+ a.MiddleName else '' end as RoleAndFullName from Contacts as a "
                   + " where a.ContactBranchId in (SELECT BranchId FROM Branches WHERE DivisionID IN (Select DivisionID from Divisions where GroupID in (select GroupID from GroupUsers  where UserID=" + CurrUser.iUserID + "))) and a.[Enabled]=1 and a.ContactId<>" + this.iContactID;
                }
                else
                {
                    sSql += "union select 'Contact'+convert(varchar, a.ContactId) as UserID, a.LastName +', '+ a.FirstName + case when ISNULL(a.MiddleName, '') != '' then ' '+ a.MiddleName else '' end as RoleAndFullName from Contacts as a "
                    + "where a.ContactBranchId in (Select BranchId from Branches where GroupID in (select GroupID from GroupUsers  where UserID=" + CurrUser.iUserID + ")) and a.[Enabled]=1 and a.ContactId<>" + this.iContactID;
                }

                string sNewSql = "select * from (" + sSql + ") as t order by RoleAndFullName";
                SqlCommand SqlCmd = new SqlCommand(sNewSql);
                DataTable ToListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd);

                this.ddlToList.DataSource = ToListData;
                this.ddlToList.DataBind();
                this.ddlToList.SelectedValue = "Contact" + this.iContactID.ToString();

                string sSql2 = "select 'Contact'+convert(varchar, ContactId) as UserID, LastName +', '+ FirstName + case when ISNULL(MiddleName, '') != '' then ' '+ MiddleName else '' end as FullName from Contacts "
                            + "where ContactId=" + this.iContactID;
                if (CurrUser.bIsCompanyUser)
                {
                    // All Contact 
                    sSql2 += "Union select 'Contact'+convert(varchar, a.ContactId) as UserID, a.LastName +', '+ a.FirstName + case when ISNULL(a.MiddleName, '') != '' then ' '+ a.MiddleName else '' end as FullName from Contacts as a "
                    + " where a.ContactId<>" + this.iContactID;
                }
                else if (CurrUser.bIsRegionUser)
                {
                    sSql2 += "Union select 'Contact'+convert(varchar, a.ContactId) as UserID, a.LastName +', '+ a.FirstName + case when ISNULL(a.MiddleName, '') != '' then ' '+ a.MiddleName else '' end as FullName from Contacts as a "
                   + " where a.ContactBranchId in (SELECT BranchId FROM Branches WHERE RegionID IN (Select RegionID from Regions where GroupID in (select GroupID from GroupUsers  where UserID=" + CurrUser.iUserID + "))) and a.[Enabled]=1 and a.ContactId<>" + this.iContactID;
                }
                else if (CurrUser.bIsDivisionUser)
                {
                    sSql2 += "Union select 'Contact'+convert(varchar, a.ContactId) as UserID, a.LastName +', '+ a.FirstName + case when ISNULL(a.MiddleName, '') != '' then ' '+ a.MiddleName else '' end as FullName from Contacts as a "
                   + " where a.ContactBranchId in (SELECT BranchId FROM Branches WHERE DivisionID IN (Select DivisionID from Divisions where GroupID in (select GroupID from GroupUsers  where UserID=" + CurrUser.iUserID + "))) and a.[Enabled]=1 and a.ContactId<>" + this.iContactID;
                }
                else
                {
                    sSql2 += "Union select 'Contact'+convert(varchar, a.ContactId) as UserID, a.LastName +', '+ a.FirstName + case when ISNULL(a.MiddleName, '') != '' then ' '+ a.MiddleName else '' end as FullName from Contacts as a "
                    + " where a.ContactBranchId in (Select BranchId from Branches where GroupID in (select GroupID from GroupUsers  where UserID=" + CurrUser.iUserID + ")) and a.[Enabled]=1 and a.ContactId<>" + this.iContactID;
                }

                SqlCommand SqlCmd2 = new SqlCommand(sSql2);
                DataTable ToList_FullName_Data = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd2);

                this.ddlToList_FullName.DataSource = ToList_FullName_Data;
                this.ddlToList_FullName.DataBind();

                string sSql3 = "select 'Contact'+convert(varchar, ContactId) as UserID, isnull(Email,'') as Email from Contacts "
                           + "where ContactId=" + this.iContactID;

                if (CurrUser.bIsCompanyUser)
                {
                    // All Contact 
                    sSql3 += "Union select 'Contact'+convert(varchar, a.ContactId) as UserID, isnull(a.Email,'') as Email from Contacts as a "
                    + " where a.ContactId<>" + this.iContactID;
                }
                else if (CurrUser.bIsRegionUser)
                {
                    sSql3 += "Union select 'Contact'+convert(varchar, a.ContactId) as UserID, isnull(a.Email,'') as Email from Contacts as a "
                   + " where a.ContactBranchId in (SELECT BranchId FROM Branches WHERE RegionID IN (Select RegionID from Regions where GroupID in (select GroupID from GroupUsers  where UserID=" + CurrUser.iUserID + "))) and a.[Enabled]=1 and a.ContactId<>" + this.iContactID;
                }
                else if (CurrUser.bIsDivisionUser)
                {
                    sSql3 += "Union select 'Contact'+convert(varchar, a.ContactId) as UserID, isnull(a.Email,'') as Email from Contacts as a "
                   + " where a.ContactBranchId in (SELECT BranchId FROM Branches WHERE DivisionID IN (Select DivisionID from Divisions where GroupID in (select GroupID from GroupUsers  where UserID=" + CurrUser.iUserID + "))) and a.[Enabled]=1 and a.ContactId<>" + this.iContactID;
                }
                else
                {
                    sSql3 += "Union select 'Contact'+convert(varchar, a.ContactId) as UserID, isnull(a.Email,'') as Email from Contacts as a "
                    + " where a.ContactBranchId in (Select BranchId from Branches where GroupID in (select GroupID from GroupUsers  where UserID=" + CurrUser.iUserID + ")) and a.[Enabled]=1 and a.ContactId<>" + this.iContactID;
                }

                SqlCommand SqlCmd3 = new SqlCommand(sSql3);
                DataTable ToList_Email_Data = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd3);

                this.ddlToList_Email.DataSource = ToList_Email_Data;
                this.ddlToList_Email.DataBind();

                #endregion
            }
            #endregion
        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        #region 准备接口数据

        string sEmailTemplateID = this.ddlEmailTemplateList.SelectedValue;
        string sToIDs = this.hdnToIDs.Value;
        LoginUser CurrentUser = this.CurrUser;

        int[] ToUserIDArray = null;
        int[] ToContactIDArray = null;
        string[] ToEmailAddrArray = null;

        int EmailIndex = 0;

        int[] CCUserIDArray = null;
        int[] CCContactIDArray = null;
        string[] CCEmailAddrArray = null;

        if (sToIDs == string.Empty)  // 如果未添加收件人，则以email template的To和CC为准发邮件
        {
            #region use To and CC of email template

            #region 获取Email Template的Recipient(s)

            Template_Email EmailTemplateManager = new Template_Email();
            DataTable RecipientList = EmailTemplateManager.GetRecipientList(Convert.ToInt32(sEmailTemplateID));

            #endregion

            #region 获取Loan Team

            string sSql = "select * from LoanTeam where FileId = " + this.iLoanID;
            DataTable LoanTeamList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

            #endregion

            #region 获取Contacts

            string sSql2 = "select * from LoanContacts where FileId = " + this.iLoanID;
            DataTable ContactList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql2);

            #endregion

            Collection<Int32> ToUserIDs = new Collection<int>();
            Collection<Int32> ToContactIDs = new Collection<int>();
            Collection<String> ToEmailList = new Collection<String>();

            Collection<Int32> CCUserIDs = new Collection<int>();
            Collection<Int32> CCContactIDs = new Collection<int>();
            Collection<String> CCEmailList = new Collection<String>();

            #region To

            DataRow[] ToRecipient = RecipientList.Select("RecipientType='To'");
            if (ToRecipient.Length > 0)
            {
                string sEmailList_To = ToRecipient[0]["EmailAddr"].ToString();
                string sContactList_To = ToRecipient[0]["ContactRoles"].ToString();
                string sUserRoleList_To = ToRecipient[0]["UserRoles"].ToString();

                #region Emails

                if (sEmailList_To != string.Empty)
                {
                    string[] EmailArray_To = sEmailList_To.Split(';');
                    foreach (string sEmailTo in EmailArray_To)
                    {
                        ToEmailList.Add(sEmailTo);
                    }
                }

                #endregion

                #region User IDs

                if (sUserRoleList_To != string.Empty)
                {
                    string[] UserRoleArray_To = sUserRoleList_To.Split(';');
                    foreach (string sUserRoleIDTo in UserRoleArray_To)
                    {
                        int iUserRoleIDTo = Convert.ToInt32(sUserRoleIDTo);

                        DataRow[] LoanTeamRows = LoanTeamList.Select("RoleId=" + iUserRoleIDTo);
                        foreach (DataRow LoanTeamRow in LoanTeamRows)
                        {
                            int iUserID = Convert.ToInt32(LoanTeamRow["UserId"]);
                            ToUserIDs.Add(iUserID);
                        }
                    }
                }

                #endregion

                #region Contact IDs

                if (sContactList_To != string.Empty)
                {
                    string[] ContactArray_To = sContactList_To.Split(';');
                    foreach (string sContactIDTo in ContactArray_To)
                    {
                        int iContactRoleIDTo = Convert.ToInt32(sContactIDTo);

                        DataRow[] ContactRows = ContactList.Select("ContactRoleId=" + iContactRoleIDTo);
                        foreach (DataRow ContactRow in ContactRows)
                        {
                            int iContactID = Convert.ToInt32(ContactRow["ContactId"]);
                            ToContactIDs.Add(iContactID);
                        }
                    }
                }

                #endregion
            }

            #endregion

            #region CC

            DataRow[] CCRecipient = RecipientList.Select("RecipientType='CC'");
            if (CCRecipient.Length > 0)
            {
                string sEmailList_CC = CCRecipient[0]["EmailAddr"].ToString();
                string sContactList_CC = CCRecipient[0]["ContactRoles"].ToString();
                string sUserRoleList_CC = CCRecipient[0]["UserRoles"].ToString();

                #region Emails

                if (sEmailList_CC != string.Empty)
                {
                    string[] EmailArray_CC = sEmailList_CC.Split(';');
                    foreach (string sEmailCC in EmailArray_CC)
                    {
                        CCEmailList.Add(sEmailCC);
                    }
                }

                #endregion

                #region User IDs

                if (sUserRoleList_CC != string.Empty)
                {
                    string[] UserRoleArray_CC = sUserRoleList_CC.Split(';');
                    foreach (string sUserRoleIDCC in UserRoleArray_CC)
                    {
                        int iUserRoleIDCC = Convert.ToInt32(sUserRoleIDCC);

                        DataRow[] LoanTeamRows = LoanTeamList.Select("RoleId=" + iUserRoleIDCC);
                        foreach (DataRow LoanTeamRow in LoanTeamRows)
                        {
                            int iUserID = Convert.ToInt32(LoanTeamRow["UserId"]);
                            CCUserIDs.Add(iUserID);
                        }
                    }
                }

                #endregion

                #region Contact IDs

                if (sContactList_CC != string.Empty)
                {
                    string[] ContactArray_CC = sContactList_CC.Split(';');
                    foreach (string sContactIDCC in ContactArray_CC)
                    {
                        int iContactRoleIDCC = Convert.ToInt32(sContactIDCC);

                        DataRow[] ContactRows = ContactList.Select("ContactRoleId=" + iContactRoleIDCC);
                        foreach (DataRow ContactRow in ContactRows)
                        {
                            int iContactID = Convert.ToInt32(ContactRow["ContactId"]);
                            CCContactIDs.Add(iContactID);
                        }
                    }
                }

                #endregion
            }

            // if check me, add user's email to CC list
            if (this.chkCCMe.Checked == true && CurrentUser.sEmail != string.Empty)
            {
                CCEmailList.Add(CurrentUser.sEmail);
            }

            #endregion

            ToUserIDArray = new int[ToUserIDs.Count];
            ToContactIDArray = new int[ToContactIDs.Count];
            ToEmailAddrArray = new string[ToEmailList.Count];

            CCUserIDArray = new int[CCUserIDs.Count];
            CCContactIDArray = new int[CCContactIDs.Count];
            CCEmailAddrArray = new string[CCEmailList.Count];

            ToUserIDs.CopyTo(ToUserIDArray, 0);
            ToContactIDs.CopyTo(ToContactIDArray, 0);
            //ToContactIDArray[0] =this.iContactID;
            ToEmailList.CopyTo(ToEmailAddrArray, 0);

            CCUserIDs.CopyTo(CCUserIDArray, 0);
            CCContactIDs.CopyTo(CCContactIDArray, 0);
            CCEmailList.CopyTo(CCEmailAddrArray, 0);

            #endregion
        }
        else // 如果添加收件人，则覆盖email template的To和CC
        {
            #region build ToUserIDArray and ToContactIDArray

            Collection<Int32> ToUserIDs = new Collection<int>();
            Collection<Int32> ToContactIDs = new Collection<int>();

            string EmailAddress = "";
            string UserSql = "";
            string ContactSql = "";

            DataTable UserTable = null;
            DataTable ContactTable = null;

            string[] ToIDArray = sToIDs.Split('$');
            ToEmailAddrArray = new string[ToIDArray.Length];
            foreach (string ToID in ToIDArray)
            {
                if (ToID.Contains("User") == true)
                {
                    int iToUserID = Convert.ToInt32(ToID.Replace("User", ""));
                    ToUserIDs.Add(iToUserID);
                    UserSql = "select EmailAddress from Users where UserId = " + iToUserID;
                    UserTable = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(UserSql);
                    foreach (DataRow dr in UserTable.Rows)
                    {
                        if (dr["EmailAddress"] == DBNull.Value)
                            EmailAddress = "";
                        else
                            EmailAddress = dr["EmailAddress"].ToString().Trim();
                    }
                    ToEmailAddrArray[EmailIndex] = EmailAddress;
                    EmailIndex = EmailIndex + 1;
                }
                else
                {
                    int iToContactID = Convert.ToInt32(ToID.Replace("Contact", ""));
                    ToContactIDs.Add(iToContactID);
                    ContactSql = "select Email from Contacts where ContactId = " + iToContactID;
                    ContactTable = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(ContactSql);

                    foreach (DataRow dr in ContactTable.Rows)
                    {
                        if (dr["Email"] == DBNull.Value)
                            EmailAddress = "";
                        else
                            EmailAddress = dr["Email"].ToString().Trim();
                    }
                    ToEmailAddrArray[EmailIndex] = EmailAddress;
                    EmailIndex = EmailIndex + 1;
                }
            }

            ToUserIDArray = new int[ToUserIDs.Count];
            ToContactIDArray = new int[ToContactIDs.Count];

            if (ToUserIDs.Count > 0)
            {
                ToUserIDs.CopyTo(ToUserIDArray, 0);
            }

            if (ToContactIDs.Count > 0)
            {
                ToContactIDs.CopyTo(ToContactIDArray, 0);
            }
            // ToContactIDArray[0] = this.iContactID;

            #endregion

            CCUserIDArray = new int[0];
            CCContactIDArray = new int[0];
            if (this.chkCCMe.Checked == true && CurrentUser.sEmail != string.Empty)
            {
                CCEmailAddrArray = new string[1];
                CCEmailAddrArray.SetValue(CurrentUser.sEmail, 0);
            }
            else
            {
                CCEmailAddrArray = new string[0];
            }
        }

        #endregion

        #region 调用API

        string sCloseDialogCodes = this.GetCloseDialogJs();

        SendEmailResponse response = null;

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                #region SendEmailRequest

                SendEmailRequest req = new SendEmailRequest();
                if (this.chkUserEmailTemplate.Checked == true)
                {
                    req.EmailTemplId = Convert.ToInt32(sEmailTemplateID);
                    req.EmailSubject = string.Empty;
                    req.EmailBody = null;
                    req.AppendPictureSignature = false;
                }
                else
                {
                    req.EmailTemplId = 0;
                    req.EmailSubject = this.txtSubject.Text.Trim();
                    req.EmailBody = Encoding.UTF8.GetBytes(this.txtBody.Text.Trim());
                    req.AppendPictureSignature = this.chkAppendMyPic.Checked;
                }

                if (this.Request.QueryString["ContactID"] != null)
                {
                    int[] iContactIDs = new int[] { this.iContactID };
                    req.ToContactIds = iContactIDs;
                }


                req.UserId = CurrentUser.iUserID;
                req.ToEmails = ToEmailAddrArray;
                req.ToUserIds = ToUserIDArray;
                req.ToContactIds = ToContactIDArray;
                req.CCEmails = CCEmailAddrArray;
                req.CCUserIds = CCUserIDArray;
                req.CCContactIds = CCContactIDArray;
                req.hdr = new ReqHdr();

                #endregion

                response = service.SendEmail(req);
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("Failed to send email, reason: Email Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, sCloseDialogCodes);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Failed to send email, error: {0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, sCloseDialogCodes);
        }

        #endregion

        // 提示调用结果
        if (response.resp.Successful == true)
        {
            string RefreshParent = "window.parent.location.href=window.parent.location.href;";
            PageCommon.WriteJsEnd(this, "Sent email successfully.", RefreshParent + sCloseDialogCodes);
        }
        else
        {
            PageCommon.WriteJsEnd(this, "Failed to send email: " + response.resp.StatusInfo.Replace("'", "\'"), sCloseDialogCodes);
        }
    }

    private string GetCloseDialogJs()
    {
        if (this.Request.QueryString["CloseDialogCodes"] != null)
        {
            string sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString();
            return sCloseDialogCodes + ";";
        }
        else
        {
            return "window.parent.CloseDialog_SendEmail();";
        }
    }
}

