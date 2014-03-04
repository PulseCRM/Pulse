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

public partial class LoanDetails_EmailSendPopup : BasePage
{
    int iLoanID = 0;
    int iProspectID = 0;
    int iProspectAlertID = 0;
    int iContactID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sCloseDialogCodes = this.GetCloseDialogJs();

        if (this.Request.QueryString["LoanID"] == null 
            && this.Request.QueryString["ProspectID"] == null
            && this.Request.QueryString["ContactID"] == null 
            && this.Request.QueryString["ProspectAlertID"] == null) 
        {
            
            this.ClientScript.RegisterStartupScript(this.GetType(), "_InvalidQuery1", "$('#divContainer').hide();alert('Missing required query string.');" + sCloseDialogCodes, true);
            return;
        }

        if (this.Request.QueryString["LoanID"] != null)
        {
            #region LoanID

            bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
            if (bIsValid == false)
            {

                this.ClientScript.RegisterStartupScript(this.GetType(), "_InvalidQuery2", "$('#divContainer').hide();alert('Missing required query string.');" + sCloseDialogCodes, true);
                return;
            }
            this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

            #endregion
        }
        else if (this.Request.QueryString["ProspectID"] != null)
        {
            #region ProspectID

            bool bIsValid = PageCommon.ValidateQueryString(this, "ProspectID", QueryStringType.ID);
            if (bIsValid == false)
            {

                this.ClientScript.RegisterStartupScript(this.GetType(), "_InvalidQuery2", "$('#divContainer').hide();alert('Missing required query string.');" + sCloseDialogCodes, true);
                return;
            }
            this.iProspectID = Convert.ToInt32(this.Request.QueryString["ProspectID"]);

            #endregion
        }
        else if (this.Request.QueryString["ContactID"] != null)
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
        else if (this.Request.QueryString["ProspectAlertID"] != null)
        {
            #region ProspectAlertID

            bool bIsValid = PageCommon.ValidateQueryString(this, "ProspectAlertID", QueryStringType.ID);
            if (bIsValid == false)
            {

                this.ClientScript.RegisterStartupScript(this.GetType(), "_InvalidQuery2", "$('#divContainer').hide();alert('Missing required query string.');" + sCloseDialogCodes, true);
                return;
            }
            this.iProspectAlertID = Convert.ToInt32(this.Request.QueryString["ProspectAlertID"]);

            #endregion

            #region 获取ProspectID

            ProspectAlerts ProspectAlertManager = new ProspectAlerts();
            DataTable ProspectAlertInfo = ProspectAlertManager.GetProspectAlertInfo(this.iProspectAlertID);
            if (ProspectAlertInfo.Rows.Count == 0)
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "_InvalidQuery3", "$('#divContainer').hide();alert('Invalid prospect alert id.');" + sCloseDialogCodes, true);
                return;
            }

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
            DataTable EmailTemplates = EmailTempManager.GetEmailTemplate(" and Enabled = 1 "); //gdc 20130528 取消 order by [Name] asc 有人在方法里加入了排序

            this.ddlEmailTemplateList.DataSource = EmailTemplates;
            this.ddlEmailTemplateList.DataBind();

            #endregion

            #region 加载To(Dropdown List)

            if (this.Request.QueryString["LoanID"] != null)
            {
                #region LoanID

                string sSql = "select 'User'+convert(varchar, a.UserId) as UserID, c.Name +' - '+ b.LastName +', '+b.FirstName as RoleAndFullName from LoanTeam as a "
                            + "inner join Users as b on a.UserId = b.UserId "
                            + "inner join Roles as c on a.RoleId = c.RoleId "
                            + "where FileId=" + this.iLoanID + " "
                            + "union "
                            + "select 'Contact'+convert(varchar, a.ContactId) as UserID, c.Name +' - '+ b.LastName +', '+ b.FirstName + case when ISNULL(b.MiddleName, '') != '' then ' '+ b.MiddleName else '' end as RoleAndFullName from LoanContacts as a "
                            + "inner join Contacts as b on a.ContactId = b.ContactId "
                            + "inner join ContactRoles as c on a.ContactRoleId = c.ContactRoleId "
                            + "where FileId=" + this.iLoanID + " "
                            + "union "
                            + "select 'User'+convert(varchar, BranchMgrId) as UserID, 'Branch Manager - ' + b.LastName +', '+b.FirstName as RoleAndFullName from BranchManagers as a "
                            + "inner join Users as b on a.BranchMgrId = b.UserId "
                            + "where BranchId = ( "
                            + "select b.BranchId from PointFiles as a "
                            + "inner join PointFolders as b on a.FolderId = b.FolderId "
                            + "where FileId=" + this.iLoanID + ")";

                SqlCommand SqlCmd = new SqlCommand(sSql);
                DataTable ToListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd);

                this.ddlToList.DataSource = ToListData;
                this.ddlToList.DataBind();

                string sSql2 = "select 'User'+convert(varchar, a.UserId) as UserID, b.LastName +', '+b.FirstName as FullName from LoanTeam as a "
                            + "inner join Users as b on a.UserId = b.UserId "
                            + "inner join Roles as c on a.RoleId = c.RoleId "
                            + "where FileId=" + this.iLoanID + " "
                            + "union "
                            + "select 'Contact'+convert(varchar, a.ContactId) as UserID, b.LastName +', '+ b.FirstName + case when ISNULL(b.MiddleName, '') != '' then ' '+ b.MiddleName else '' end as FullName from LoanContacts as a "
                            + "inner join Contacts as b on a.ContactId = b.ContactId "
                            + "inner join ContactRoles as c on a.ContactRoleId = c.ContactRoleId "
                            + "where FileId=" + this.iLoanID + " "
                            + "union "
                            + "select 'User'+convert(varchar, BranchMgrId) as UserID, b.LastName +', '+b.FirstName as FullName from BranchManagers as a "
                            + "inner join Users as b on a.BranchMgrId = b.UserId "
                            + "where BranchId = ( "
                            + "select b.BranchId from PointFiles as a "
                            + "inner join PointFolders as b on a.FolderId = b.FolderId "
                            + "where FileId=" + this.iLoanID + ")";

                SqlCommand SqlCmd2 = new SqlCommand(sSql2);
                DataTable ToList_FullName_Data = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd2);

                this.ddlToList_FullName.DataSource = ToList_FullName_Data;
                this.ddlToList_FullName.DataBind();

                string sSql3 = "select 'User'+convert(varchar, a.UserId) as UserID, isnull(b.EmailAddress,'') as Email from LoanTeam as a "
                            + "inner join Users as b on a.UserId = b.UserId "
                            + "inner join Roles as c on a.RoleId = c.RoleId "
                            + "where FileId=" + this.iLoanID + " "
                            + "union "
                            + "select 'Contact'+convert(varchar, a.ContactId) as UserID, isnull(b.Email,'') as Email from LoanContacts as a "
                            + "inner join Contacts as b on a.ContactId = b.ContactId "
                            + "inner join ContactRoles as c on a.ContactRoleId = c.ContactRoleId "
                            + "where FileId=" + this.iLoanID + " "
                            + "union "
                            + "select 'User'+convert(varchar, BranchMgrId) as UserID, isnull(b.EmailAddress,'') as Email from BranchManagers as a "
                            + "inner join Users as b on a.BranchMgrId = b.UserId "
                            + "where BranchId = ( "
                            + "select b.BranchId from PointFiles as a "
                            + "inner join PointFolders as b on a.FolderId = b.FolderId "
                            + "where FileId=" + this.iLoanID + ")";

                SqlCommand SqlCmd3 = new SqlCommand(sSql3);
                DataTable ToList_Email_Data = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd3);

                this.ddlToList_Email.DataSource = ToList_Email_Data;
                this.ddlToList_Email.DataBind();

                #endregion
            }
            else if (this.Request.QueryString["ContactID"] != null)
            {
                #region ContactID

                string sSql = "select 'Contact'+convert(varchar, a.ContactId) as UserID, a.LastName +', '+ a.FirstName + case when ISNULL(a.MiddleName, '') != '' then ' '+ a.MiddleName else '' end as RoleAndFullName from Contacts as a "     
                            + "where ContactId=" + this.iContactID;
                         

                SqlCommand SqlCmd = new SqlCommand(sSql);
                DataTable ToListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd);

                this.ddlToList.DataSource = ToListData;
                this.ddlToList.DataBind();

                string sSql2 = "select 'Contact'+convert(varchar, a.ContactId) as UserID, a.LastName +', '+ a.FirstName + case when ISNULL(a.MiddleName, '') != '' then ' '+ a.MiddleName else '' end as FullName from Contacts as a "
                            + "where ContactId=" + this.iContactID;

                SqlCommand SqlCmd2 = new SqlCommand(sSql2);
                DataTable ToList_FullName_Data = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd2);

                this.ddlToList_FullName.DataSource = ToList_FullName_Data;
                this.ddlToList_FullName.DataBind();

                string sSql3 = "select 'Contact'+convert(varchar, a.ContactId) as UserID, isnull(a.Email,'') as Email from Contacts as a "
                           + "where ContactId=" + this.iContactID ;

                SqlCommand SqlCmd3 = new SqlCommand(sSql3);
                DataTable ToList_Email_Data = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd3);

                this.ddlToList_Email.DataSource = ToList_Email_Data;
                this.ddlToList_Email.DataBind();

                #endregion
            }
            else
            {
                #region ProspectID or ProspectAlertID

                string sSql = "select 'User'+convert(varchar, a.Loanofficer) as UserID, c.Name +' - '+ b.LastName +', '+b.FirstName as RoleAndFullName "
                            + "from Prospect as a inner join Users as b on a.Loanofficer = b.UserId "
                            + "inner join Roles as c on b.RoleId = c.RoleId "
                            + "where a.Contactid=" + this.iProspectID + " "
                            + "union "
                            + "select 'Contact'+convert(varchar, a.ContactId) as UserID, d.Name +' - '+ b.LastName +', '+ b.FirstName + case when ISNULL(b.MiddleName, '') != '' then ' '+ b.MiddleName else '' end as RoleAndFullName "
                            + "from Prospect as a inner join Contacts as b on a.Contactid = b.ContactId "
                            + "inner join LoanContacts as c on a.Contactid = c.ContactId "
                            + "inner join ContactRoles as d on c.ContactRoleId = d.ContactRoleId "
                            + "where a.Contactid=" + this.iProspectID + " "
                            + "union "
                            + "select 'User'+convert(varchar, a.UserId) as UserID, d.Name +' - '+ a.LastName +', '+a.FirstName as RoleAndFullName "
                            + "from Users as a "
                            + "inner join GroupUsers as b on a.UserId = b.UserID "
                            + "inner join Groups as c on b.GroupID = c.GroupId "
                            + "inner join Roles as d on a.RoleId = d.RoleId "
                            + "where c.Enabled = 1 and a.UserEnabled=1 "
                            + "and c.BranchId in (select * from lpfn_GetUserBranches_UserList((select Loanofficer from Prospect where Contactid=" + this.iProspectID + ")))";

                SqlCommand SqlCmd = new SqlCommand(sSql);
                DataTable ToListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd);

                this.ddlToList.DataSource = ToListData;
                this.ddlToList.DataBind();

                string sSql2 = "select 'User'+convert(varchar, a.Loanofficer) as UserID, b.LastName +', '+b.FirstName as FullName "
                             + "from Prospect as a inner join Users as b on a.Loanofficer = b.UserId "
                             + "where a.Contactid=" + this.iProspectID + " "
                             + "union "
                             + "select 'Contact'+convert(varchar, a.ContactId) as UserID, b.LastName +', '+ b.FirstName + case when ISNULL(b.MiddleName, '') != '' then ' '+ b.MiddleName else '' end as FullName "
                             + "from Prospect as a inner join Contacts as b on a.Contactid = b.ContactId "
                             + "where a.Contactid=" + this.iProspectID + " "
                             + "union "
                             + "select 'User'+convert(varchar, a.UserId) as UserID, a.LastName +', '+a.FirstName as FullName "
                             + "from Users as a "
                             + "inner join GroupUsers as b on a.UserId = b.UserID "
                             + "inner join Groups as c on b.GroupID = c.GroupId "
                             + "where c.Enabled = 1 and a.UserEnabled=1 "
                             + "and c.BranchId in (select * from lpfn_GetUserBranches_UserList((select Loanofficer from Prospect where Contactid=" + this.iProspectID + ")))";

                SqlCommand SqlCmd2 = new SqlCommand(sSql2);
                DataTable ToList_FullName_Data = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd2);

                this.ddlToList_FullName.DataSource = ToList_FullName_Data;
                this.ddlToList_FullName.DataBind();

                string sSql3 = "select 'User'+convert(varchar, a.Loanofficer) as UserID, isnull(b.EmailAddress,'') as Email "
                             + "from Prospect as a inner join Users as b on a.Loanofficer = b.UserId "
                             + "where a.Contactid=" + this.iProspectID + " "
                             + "union "
                             + "select 'Contact'+convert(varchar, a.ContactId) as UserID, isnull(b.Email,'') as Email "
                             + "from Prospect as a inner join Contacts as b on a.Contactid = b.ContactId "
                             + "where a.Contactid=" + this.iProspectID + " "
                             + "union "
                             + "select 'User'+convert(varchar, a.UserId) as UserID, isnull(a.EmailAddress,'') as Email "
                             + "from Users as a "
                             + "inner join GroupUsers as b on a.UserId = b.UserID "
                             + "inner join Groups as c on b.GroupID = c.GroupId "
                             + "where c.Enabled = 1 and a.UserEnabled=1 "
                             + "and c.BranchId in (select * from lpfn_GetUserBranches_UserList((select Loanofficer from Prospect where Contactid=" + this.iProspectID + ")))";

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
        string Token = hidToken.Value;
        Dictionary<string, byte[]> Attachments = new Dictionary<string, byte[]>();

        #region Attachments

        //LPWeb.BLL.Template_Email_Attachments bllTempEmailattach = new Template_Email_Attachments();
        //var ds = bllTempEmailattach.GetList(" [Enabled] =1 AND TemplEmailId = " + sEmailTemplateID);

        LPWeb.BLL.Email_AttachmentsTemp bllEmailAttachTemp = new Email_AttachmentsTemp();

        var ds = bllEmailAttachTemp.GetListWithFileImage(Convert.ToInt32(sEmailTemplateID), Token);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                try
                {
                    Attachments.Add(item["Name"].ToString() + "." + item["FileType"].ToString(), (byte[])item["FileImage"]);
                }
                catch { }
            }

        }


        #endregion

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
                
                if(this.Request.QueryString["LoanID"] != null)
                {
                    req.FileId = this.iLoanID;
                }
                else if (this.Request.QueryString["ProspectID"] != null)
                {
                    req.ProspectId = this.iProspectID;
                }
                else if (this.Request.QueryString["ProspectAlertID"] != null)
                {
                    req.PropsectTaskId = this.iProspectAlertID;
                }

                if ( (this.Request.QueryString["LoanID"] == null) &&
                     (this.Request.QueryString["ProspectID"] != null) )
                {
                    string sSql = "select * from LoanContacts where ContactId=" + req.ProspectId + " and (ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId())";

                    DataTable LoanList = null;
                    try
                    {
                        LoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
                        foreach (DataRow LoanListRow in LoanList.Rows)
                        {
                            req.FileId = (int)LoanListRow["FileId"];                           
                        }
                    }
                    catch
                    {

                    }

                    if (LoanList== null || LoanList.Rows.Count == 0)
                    {

                    }
                }

                req.UserId = CurrentUser.iUserID;
                req.ToEmails = ToEmailAddrArray;
                req.ToUserIds = ToUserIDArray;
                req.ToContactIds = ToContactIDArray;
                req.CCEmails = CCEmailAddrArray;
                req.CCUserIds = CCUserIDArray;
                req.CCContactIds = CCContactIDArray;
                req.hdr = new ReqHdr();

                #region add Attachments

                req.Attachments = Attachments;


                #endregion


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
            try
            {
                bllEmailAttachTemp.DeleteByToken(Token);
            }
            catch { }
            
            string RefreshParent = "window.parent.location.href=window.parent.location.href;";
            PageCommon.WriteJsEnd(this, "Sent email successfully.", RefreshParent + sCloseDialogCodes );            
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
