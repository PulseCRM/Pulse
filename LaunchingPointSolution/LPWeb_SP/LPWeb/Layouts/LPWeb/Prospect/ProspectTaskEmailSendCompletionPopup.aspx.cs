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
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using LPWeb.Common;
using LPWeb.LP_Service;

public partial class Prospect_ProspectTaskEmailSendCompletionPopup : BasePage
{
    int iLoanID = 0;
    int iOwnerID = 0;
    int iProspectID = 0;
    int iEmailTemplateID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sErrorJs = "$('#divContainer').hide();alert('Missing required query string.');window.parent.CloseDialog_SendCompletionEmail();";
        bool bIsValid = PageCommon.ValidateQueryString(this, "prospectID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "_InvalidQuery1", sErrorJs, true);
            return;
        }
        this.iProspectID = Convert.ToInt32(this.Request.QueryString["prospectID"]);

        bIsValid = PageCommon.ValidateQueryString(this, "EmailTemplateID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "_InvalidQuery2", sErrorJs, true);
            return;
        }
        this.iEmailTemplateID = Convert.ToInt32(this.Request.QueryString["EmailTemplateID"]);

        #endregion

        #region 加载email template info

        Template_Email EmailTempManager = new Template_Email();
        DataTable EmailTemplateInfo = EmailTempManager.GetEmailTemplateInfo(this.iEmailTemplateID);

        if (EmailTemplateInfo.Rows.Count == 0)
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "_InvalidQuery3", "$('#divContainer').hide();alert('The email template does not exist.');window.parent.CloseDialog_SendCompletionEmail();", true);
            return;
        }

        string sEmailTemplate = EmailTemplateInfo.Rows[0]["Name"].ToString();
        this.lbEmailTemplate.Text = sEmailTemplate;

        string sFromUserRoleID = EmailTemplateInfo.Rows[0]["FromUserRoles"].ToString();
        if (sFromUserRoleID != string.Empty)
        {
            #region 获取Loan Team和Users信息

            string sSql = "select * from LoanTasks as a inner join Users as b on a.CompletedBy = b.UserId where a.LoanTaskId=" + this.iProspectID;
            DataTable SenderInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

            if (SenderInfo.Rows.Count == 0)
            {
                this.lbSender.Text = "There is no sender, so can't send email.";
                this.lbSender.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                string sFirstName = SenderInfo.Rows[0]["FirstName"].ToString();
                string sLastName = SenderInfo.Rows[0]["LastName"].ToString();
                string sFullName = sLastName + ", " + sFirstName;
                this.lbSender.Text = sFullName;
                this.iLoanID = Convert.ToInt32(SenderInfo.Rows[0]["FileId"].ToString());

                string sSql_n = "select * from LoanTeam as a inner join Users as b on a.UserId = b.UserId where a.FileId=" + this.iLoanID + " and a.RoleId=" + sFromUserRoleID;
                DataTable SenderInfo_n = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql_n);

                if (SenderInfo_n.Rows.Count > 0)
                {
                    string sFirstName_n = SenderInfo_n.Rows[0]["FirstName"].ToString();
                    string sLastName_n = SenderInfo_n.Rows[0]["LastName"].ToString();
                    string sFullName_n = sLastName_n + ", " + sFirstName_n;
                    this.lbSender.Text = sFullName_n;
                }
            }

            #endregion
        }
        else
        {
            string sFromEmailAddress = EmailTemplateInfo.Rows[0]["FromEmailAddress"].ToString();
            if (sFromEmailAddress != string.Empty)
            {
                this.lbSender.Text = sFromEmailAddress;
            }
            else
            {
                this.lbSender.Text = "There is no sender.";
                this.lbSender.ForeColor = System.Drawing.Color.Red;
            }
        }

        #endregion

        if (this.IsPostBack == false)
        {
            #region 加载Recipient列表

            #region 获取Email Template的Recipient(s)

            Template_Email EmailTemplateManager = new Template_Email();
            DataTable RecipientList = EmailTemplateManager.GetRecipientList(this.iEmailTemplateID);

            #endregion

            #region 获取Loan Team

            string sSql = "select * from LoanTeam as a inner join Users as b on a.UserId = b.UserId where a.FileId=" + this.iLoanID;
            DataTable LoanTeamList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

            #endregion

            #region 获取Contacts

            string sSql2 = "select * from LoanContacts as a inner join Contacts as b on a.ContactId = b.ContactId where a.FileId=" + this.iLoanID;
            DataTable ContactList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql2);

            #endregion

            #region 获取Prospect Loan Officer 

            string sSql3 = "SELECT dbo.Users.*  FROM dbo.Prospect INNER JOIN  dbo.ProspectTasks ON dbo.Prospect.Contactid = dbo.ProspectTasks.ContactId INNER JOIN dbo.Users ON dbo.Prospect.Loanofficer = dbo.Users.UserId WHERE  dbo.ProspectTasks.ProspectTaskId =" + this.iProspectID;
            DataTable LoanOfficerList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql3);

            #endregion

            DataTable RecipientListData = this.BuildRecipientDataTable();

            #region To

            DataRow[] ToRecipient = RecipientList.Select("RecipientType='To'");
            if (ToRecipient.Length > 0)
            {
                string sEmailList_To = ToRecipient[0]["EmailAddr"].ToString();
                string sContactList_To = ToRecipient[0]["ContactRoles"].ToString();
                string sUserRoleList_To = ToRecipient[0]["UserRoles"].ToString();
                string sTaskOwner = ToRecipient[0]["TaskOwner"].ToString();

                #region Emails

                if (sEmailList_To != string.Empty)
                {
                    string[] EmailArray_To = sEmailList_To.Split(';');
                    foreach (string sEmailTo in EmailArray_To)
                    {
                        this.AddNewRecipientRow(RecipientListData, "To", string.Empty, sEmailTo);
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

                        DataRow[] LoanTeamRows = LoanOfficerList.Select("RoleId=" + iUserRoleIDTo);
                        foreach (DataRow LoanTeamRow in LoanTeamRows)
                        {
                            string sFullName = LoanTeamRow["LastName"].ToString() + ", " + LoanTeamRow["FirstName"].ToString();
                            string sEmailAddress = LoanTeamRow["EmailAddress"].ToString();
                            if (sEmailAddress == string.Empty)
                            {
                                this.AddNewRecipientRow(RecipientListData, "To", sFullName, "There is no emaill address in the user account.");
                            }
                            else
                            {
                                this.AddNewRecipientRow(RecipientListData, "To", sFullName, sEmailAddress);
                            }
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
                            #region Build FullName

                            string sFirstName = ContactRow["FirstName"].ToString();
                            string sLastName = ContactRow["LastName"].ToString();
                            string sMiddleName = ContactRow["MiddleName"].ToString();

                            string sFullName = string.Empty;
                            if (sMiddleName != string.Empty)
                            {
                                sFullName = sLastName + ", " + sFirstName + " " + sMiddleName;
                            }
                            else
                            {
                                sFullName = sLastName + ", " + sFirstName;
                            }

                            #endregion

                            string sEmailAddress = ContactRow["Email"].ToString();

                            if (sEmailAddress == string.Empty)
                            {
                                this.AddNewRecipientRow(RecipientListData, "To", sFullName, "There is no emaill address in the contact account.");
                            }
                            else
                            {
                                this.AddNewRecipientRow(RecipientListData, "To", sFullName, sEmailAddress);
                            }
                        }
                    }
                }

                #endregion

                #region Loan Officer User IDs

                if (sUserRoleList_To != string.Empty)
                {
                    foreach (DataRow LoanOfficerRow in LoanOfficerList.Rows)
                        {
                            string sFullName = LoanOfficerRow["LastName"].ToString() + ", " + LoanOfficerRow["FirstName"].ToString();
                            string sEmailAddress = LoanOfficerRow["EmailAddress"].ToString();
                            if (sEmailAddress == string.Empty)
                            {
                                this.AddNewRecipientRow(RecipientListData, "To", sFullName, "There is no emaill address in the user account.");
                            }
                            else
                            {
                                this.AddNewRecipientRow(RecipientListData, "To", sFullName, sEmailAddress);
                            }
                        }
                }

                #endregion
                #region TaskOwner

                if ((sTaskOwner != string.Empty) && (sTaskOwner != null))
                {
                    if (sTaskOwner == "True")
                    {
                        string sSql_LoanTasks = "select Owner from LoanTasks where LoanTaskId=" + this.iProspectID;
                        DataTable LoanTasks_List = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql_LoanTasks);

                        string sOwnerId = LoanTasks_List.Rows[0]["Owner"].ToString();

                        if ((sTaskOwner != string.Empty) && (sTaskOwner != null))
                        {
                            int iOwnerId = Convert.ToInt32(sOwnerId);
                            string sSql_Users = "select EmailAddress, LastName, FirstName from Users where UserId=" + iOwnerId;
                            DataTable Users_List = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql_Users);
                            string Owner_EmailAddress = Users_List.Rows[0]["EmailAddress"].ToString();
                            if ((Owner_EmailAddress != string.Empty) && (Owner_EmailAddress != null))
                            {
                                string sOwnerFullName = Users_List.Rows[0]["LastName"].ToString() + ", " + Users_List.Rows[0]["FirstName"].ToString();
                                this.AddNewRecipientRow(RecipientListData, "To", sOwnerFullName, Owner_EmailAddress);
                            }
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
                        this.AddNewRecipientRow(RecipientListData, "CC", string.Empty, sEmailCC);
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
                            string sFullName = LoanTeamRow["LastName"].ToString() + ", " + LoanTeamRow["FirstName"].ToString();
                            string sEmailAddress = LoanTeamRow["EmailAddress"].ToString();

                            if (sEmailAddress == string.Empty)
                            {
                                this.AddNewRecipientRow(RecipientListData, "CC", sFullName, "There is no emaill address in the user account.");
                            }
                            else
                            {
                                this.AddNewRecipientRow(RecipientListData, "CC", sFullName, sEmailAddress);
                            }
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
                            #region Build FullName

                            string sFirstName = ContactRow["FirstName"].ToString();
                            string sLastName = ContactRow["LastName"].ToString();
                            string sMiddleName = ContactRow["MiddleName"].ToString();

                            string sFullName = string.Empty;
                            if (sMiddleName != string.Empty)
                            {
                                sFullName = sLastName + ", " + sFirstName + " " + sMiddleName;
                            }
                            else
                            {
                                sFullName = sLastName + ", " + sFirstName;
                            }

                            #endregion

                            string sEmailAddress = ContactRow["Email"].ToString();

                            if (sEmailAddress == string.Empty)
                            {
                                this.AddNewRecipientRow(RecipientListData, "CC", sFullName, "There is no emaill address in the contact account.");
                            }
                            else
                            {
                                this.AddNewRecipientRow(RecipientListData, "CC", sFullName, sEmailAddress);
                            }
                        }
                    }
                }

                #endregion
            }

            #endregion

            this.gridRecipientList.DataSource = RecipientListData;
            this.gridRecipientList.DataBind();

            #endregion
        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        LoginUser CurrentUser = new LoginUser();

        int[] ToUserIDArray = null;
        int[] ToContactIDArray = null;
        string[] ToEmailAddrArray = null;

        int[] CCUserIDArray = null;
        int[] CCContactIDArray = null;
        string[] CCEmailAddrArray = null;

        #region use To and CC of email template

        #region 获取Email Template的Recipient(s)

        Template_Email EmailTemplateManager = new Template_Email();
        DataTable RecipientList = EmailTemplateManager.GetRecipientList(this.iEmailTemplateID);

        #endregion

        #region 获取Loan Team

        string sSql = "select * from LoanTeam where FileId = " + this.iLoanID;
        DataTable LoanTeamList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        #endregion

        #region 获取Contacts

        string sSql2 = "select * from LoanContacts where FileId = " + this.iLoanID;
        DataTable ContactList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql2);

        #endregion

        #region 获取Prospect Loan Officer

        string sSql3 = "SELECT dbo.Users.*  FROM dbo.Prospect INNER JOIN  dbo.ProspectTasks ON dbo.Prospect.Contactid = dbo.ProspectTasks.ContactId INNER JOIN dbo.Users ON dbo.Prospect.Loanofficer = dbo.Users.UserId WHERE  dbo.ProspectTasks.ProspectTaskId =" + this.iProspectID;
        DataTable LoanOfficerList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql3);

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
            string sTaskOwner = ToRecipient[0]["TaskOwner"].ToString();

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

            #region Loan Officer User IDs

            if (sUserRoleList_To != string.Empty)
            {
                foreach (DataRow LoanOfficerRow in LoanOfficerList.Rows)
                {
                    int iLOUserID = Convert.ToInt32(LoanOfficerRow["UserId"]);
                    ToUserIDs.Add(iLOUserID);
                }
            }

            #endregion
            #region TaskOwner

            if ((sTaskOwner != string.Empty) && (sTaskOwner != null))
            {
                if (sTaskOwner == "True")
                {
                    string sSql_LoanTasks = "select Owner from LoanTasks where LoanTaskId=" + this.iProspectID;
                    DataTable LoanTasks_List = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql_LoanTasks);

                    string sOwnerId = LoanTasks_List.Rows[0]["Owner"].ToString();

                    if ((sTaskOwner != string.Empty) && (sTaskOwner != null))
                    {
                        int iOwnerId = Convert.ToInt32(sOwnerId);
                        string sSql_Users = "select EmailAddress, LastName, FirstName from Users where UserId=" + iOwnerId;
                        DataTable Users_List = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql_Users);
                        string Owner_EmailAddress = Users_List.Rows[0]["EmailAddress"].ToString();
                        if ((Owner_EmailAddress != string.Empty) && (Owner_EmailAddress != null))
                        {
                            ToUserIDs.Add(iOwnerId);
                            ToEmailList.Add(Owner_EmailAddress);
                        }
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

        #region 调用API

        bool successful_status = true;

        try
        {

            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                SendEmailRequest req = new SendEmailRequest();
                req.EmailTemplId = this.iEmailTemplateID;
                req.FileId = this.iProspectID;
                req.UserId = CurrentUser.iUserID;
                req.ToEmails = ToEmailAddrArray;
                req.ToUserIds = ToUserIDArray;
                req.ToContactIds = ToContactIDArray;
                req.CCEmails = CCEmailAddrArray;
                req.CCUserIds = CCUserIDArray;
                req.CCContactIds = CCContactIDArray;
                req.hdr = new ReqHdr();

                SendEmailResponse response = service.SendEmail(req);

                if (response.resp.Successful == false)
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_FailedSend5", "alert('Failed to send completion email.');window.parent.CloseDialog_SendCompletionEmail()();", true);
                    return;
                }
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            successful_status = false;
            PageCommon.AlertMsg(this, "Failed to send completion email, reason: Email Manager is not running.");
        }

        #endregion
        if (successful_status == true)
        PageCommon.WriteJs(this, "Send completion email successfully.", "window.parent.CloseDialog_SendCompletionEmail();");
    }

    private DataTable BuildRecipientDataTable()
    {
        DataTable RecipientList = new DataTable();
        RecipientList.Columns.Add("RecipientType", typeof(string));
        RecipientList.Columns.Add("FullName", typeof(string));
        RecipientList.Columns.Add("Email", typeof(string));

        return RecipientList;
    }

    private void AddNewRecipientRow(DataTable RecipientList, string sRecipientType, string sFullName, string sEmail)
    {
        DataRow NewRecipientRow = RecipientList.NewRow();
        NewRecipientRow["RecipientType"] = sRecipientType;
        NewRecipientRow["FullName"] = sFullName;
        NewRecipientRow["Email"] = sEmail;

        RecipientList.Rows.Add(NewRecipientRow);
    }
}
