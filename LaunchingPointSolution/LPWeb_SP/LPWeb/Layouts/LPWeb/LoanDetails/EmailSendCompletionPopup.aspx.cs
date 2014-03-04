using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using LPWeb.BLL;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;

public partial class LoanDetails_EmailSendCompletionPopup : BasePage
{
    int iLoanID = 0;
    int iTaskID = 0;
    public int iEmailTemplateID = 0;
    DataTable EmailTemplates = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sErrorJs = "$('#divContainer').hide();alert('Missing required query string.');window.parent.CloseDialog_SendCompletionEmail();";
        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "_InvalidQuery1", sErrorJs, true);
            return;
        }
        this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

        bIsValid = PageCommon.ValidateQueryString(this, "EmailTemplateID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "_InvalidQuery2", sErrorJs, true);
            return;
        }
        this.iEmailTemplateID = Convert.ToInt32(this.Request.QueryString["EmailTemplateID"]);
        string sTaskID = "";
        sTaskID = this.Request.QueryString["TaskID"];
        if ((sTaskID != null) && (sTaskID != ""))
        {
            this.iTaskID = Convert.ToInt32(sTaskID);
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

        #region 旧代码

        //#region 加载email template info

        //Template_Email EmailTempManager = new Template_Email();
        //DataTable EmailTemplateInfo = EmailTempManager.GetEmailTemplateInfo(this.iEmailTemplateID);

        //if (EmailTemplateInfo.Rows.Count == 0)
        //{
        //    this.ClientScript.RegisterStartupScript(this.GetType(), "_InvalidQuery3", "$('#divContainer').hide();alert('The email template does not exist.');window.parent.CloseDialog_SendCompletionEmail();", true);
        //    return;
        //}

        //string sEmailTemplate = EmailTemplateInfo.Rows[0]["Name"].ToString();
        //this.lbEmailTemplate.Text = sEmailTemplate;

        //string sFromUserRoleID = EmailTemplateInfo.Rows[0]["FromUserRoles"].ToString();
        //if (sFromUserRoleID != string.Empty)
        //{
        //    #region 获取Loan Team和Users信息

        //    string sSql = "select * from LoanTeam as a inner join Users as b on a.UserId = b.UserId where a.FileId=" + this.iLoanID + " and a.RoleId=" + sFromUserRoleID;
        //    DataTable SenderInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        //    if (SenderInfo.Rows.Count == 0)
        //    {
        //        this.lbSender.Text = "There is no sender, so can't send email.";
        //        this.lbSender.ForeColor = System.Drawing.Color.Red;
        //    }
        //    else
        //    {
        //        string sFirstName = SenderInfo.Rows[0]["FirstName"].ToString();
        //        string sLastName = SenderInfo.Rows[0]["LastName"].ToString();
        //        string sFullName = sLastName + ", " + sFirstName;
        //        this.lbSender.Text = sFullName;
        //    }

        //    #endregion
        //}
        //else
        //{
        //    string sFromEmailAddress = EmailTemplateInfo.Rows[0]["FromEmailAddress"].ToString();
        //    if (sFromEmailAddress != string.Empty)
        //    {
        //        this.lbSender.Text = sFromEmailAddress;
        //    }
        //    else
        //    {
        //        this.lbSender.Text = "There is no sender.";
        //        this.lbSender.ForeColor = System.Drawing.Color.Red;
        //    }
        //}

        //#endregion

        //if (this.IsPostBack == false)
        //{
        //    #region 加载Recipient列表

        //    #region 获取Email Template的Recipient(s)

        //    Template_Email EmailTemplateManager = new Template_Email();
        //    DataTable RecipientList = EmailTemplateManager.GetRecipientList(this.iEmailTemplateID);

        //    #endregion

        //    #region 获取Loan Team

        //    string sSql = "select * from LoanTeam as a inner join Users as b on a.UserId = b.UserId where a.FileId=" + this.iLoanID;
        //    DataTable LoanTeamList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        //    #endregion

        //    #region 获取Contacts

        //    string sSql2 = "select * from LoanContacts as a inner join Contacts as b on a.ContactId = b.ContactId where a.FileId=" + this.iLoanID;
        //    DataTable ContactList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql2);

        //    #endregion

        //    DataTable RecipientListData = this.BuildRecipientDataTable();

        //    #region To

        //    DataRow[] ToRecipient = RecipientList.Select("RecipientType='To'");
        //    if (ToRecipient.Length > 0)
        //    {
        //        string sEmailList_To = ToRecipient[0]["EmailAddr"].ToString();
        //        string sContactList_To = ToRecipient[0]["ContactRoles"].ToString();
        //        string sUserRoleList_To = ToRecipient[0]["UserRoles"].ToString();
        //        string sTaskOwner = ToRecipient[0]["TaskOwner"].ToString();

        //        #region Emails

        //        if (sEmailList_To != string.Empty)
        //        {
        //            string[] EmailArray_To = sEmailList_To.Split(';');
        //            foreach (string sEmailTo in EmailArray_To)
        //            {
        //                this.AddNewRecipientRow(RecipientListData, "To", string.Empty, sEmailTo);
        //            }
        //        }

        //        #endregion

        //        #region User IDs

        //        if (sUserRoleList_To != string.Empty)
        //        {
        //            string[] UserRoleArray_To = sUserRoleList_To.Split(';');
        //            foreach (string sUserRoleIDTo in UserRoleArray_To)
        //            {
        //                int iUserRoleIDTo = Convert.ToInt32(sUserRoleIDTo);

        //                DataRow[] LoanTeamRows = LoanTeamList.Select("RoleId=" + iUserRoleIDTo);
        //                foreach (DataRow LoanTeamRow in LoanTeamRows)
        //                {
        //                    string sFullName = LoanTeamRow["LastName"].ToString() + ", " + LoanTeamRow["FirstName"].ToString();
        //                    string sEmailAddress = LoanTeamRow["EmailAddress"].ToString();
        //                    if (sEmailAddress == string.Empty)
        //                    {
        //                        this.AddNewRecipientRow(RecipientListData, "To", sFullName, "There is no emaill address in the user account.");
        //                    }
        //                    else
        //                    {
        //                        this.AddNewRecipientRow(RecipientListData, "To", sFullName, sEmailAddress);
        //                    }
        //                }
        //            }
        //        }

        //        #endregion

        //        #region Contact IDs

        //        if (sContactList_To != string.Empty)
        //        {
        //            string[] ContactArray_To = sContactList_To.Split(';');
        //            foreach (string sContactIDTo in ContactArray_To)
        //            {
        //                int iContactRoleIDTo = Convert.ToInt32(sContactIDTo);

        //                DataRow[] ContactRows = ContactList.Select("ContactRoleId=" + iContactRoleIDTo);
        //                foreach (DataRow ContactRow in ContactRows)
        //                {
        //                    #region Build FullName

        //                    string sFirstName = ContactRow["FirstName"].ToString();
        //                    string sLastName = ContactRow["LastName"].ToString();
        //                    string sMiddleName = ContactRow["MiddleName"].ToString();

        //                    string sFullName = string.Empty;
        //                    if (sMiddleName != string.Empty)
        //                    {
        //                        sFullName = sLastName + ", " + sFirstName + " " + sMiddleName;
        //                    }
        //                    else
        //                    {
        //                        sFullName = sLastName + ", " + sFirstName;
        //                    }

        //                    #endregion

        //                    string sEmailAddress = ContactRow["Email"].ToString();

        //                    if (sEmailAddress == string.Empty)
        //                    {
        //                        this.AddNewRecipientRow(RecipientListData, "To", sFullName, "There is no emaill address in the contact account.");
        //                    }
        //                    else
        //                    {
        //                        this.AddNewRecipientRow(RecipientListData, "To", sFullName, sEmailAddress);
        //                    }
        //                }
        //            }
        //        }

        //        #endregion

        //        #region TaskOwner

        //        if ((sTaskOwner != string.Empty) && (sTaskOwner != null))
        //        {
        //            if (sTaskOwner == "True")
        //            {
        //                string sSql_LoanTasks = "select Owner from LoanTasks where LoanTaskId=" + this.iTaskID;
        //                DataTable LoanTasks_List = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql_LoanTasks);

        //                string sOwnerId = LoanTasks_List.Rows[0]["Owner"].ToString();

        //                if ((sTaskOwner != string.Empty) && (sTaskOwner != null))
        //                {
        //                    int iOwnerId = Convert.ToInt32(sOwnerId);
        //                    string sSql_Users = "select EmailAddress, LastName, FirstName from Users where UserId=" + iOwnerId;
        //                    DataTable Users_List = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql_Users);
        //                    string Owner_EmailAddress = Users_List.Rows[0]["EmailAddress"].ToString();
        //                    if ((Owner_EmailAddress != string.Empty) && (Owner_EmailAddress != null))
        //                    {
        //                        string sOwnerFullName = Users_List.Rows[0]["LastName"].ToString() + ", " + Users_List.Rows[0]["FirstName"].ToString();
        //                        this.AddNewRecipientRow(RecipientListData, "To", sOwnerFullName, Owner_EmailAddress);
        //                    }
        //                }
        //            }
        //        }

        //        #endregion
        //    }

        //    #endregion

        //    #region CC

        //    DataRow[] CCRecipient = RecipientList.Select("RecipientType='CC'");
        //    if (CCRecipient.Length > 0)
        //    {
        //        string sEmailList_CC = CCRecipient[0]["EmailAddr"].ToString();
        //        string sContactList_CC = CCRecipient[0]["ContactRoles"].ToString();
        //        string sUserRoleList_CC = CCRecipient[0]["UserRoles"].ToString();

        //        #region Emails

        //        if (sEmailList_CC != string.Empty)
        //        {
        //            string[] EmailArray_CC = sEmailList_CC.Split(';');
        //            foreach (string sEmailCC in EmailArray_CC)
        //            {
        //                this.AddNewRecipientRow(RecipientListData, "CC", string.Empty, sEmailCC);
        //            }
        //        }

        //        #endregion

        //        #region User IDs

        //        if (sUserRoleList_CC != string.Empty)
        //        {
        //            string[] UserRoleArray_CC = sUserRoleList_CC.Split(';');
        //            foreach (string sUserRoleIDCC in UserRoleArray_CC)
        //            {
        //                int iUserRoleIDCC = Convert.ToInt32(sUserRoleIDCC);

        //                DataRow[] LoanTeamRows = LoanTeamList.Select("RoleId=" + iUserRoleIDCC);
        //                foreach (DataRow LoanTeamRow in LoanTeamRows)
        //                {
        //                    string sFullName = LoanTeamRow["LastName"].ToString() + ", " + LoanTeamRow["FirstName"].ToString();
        //                    string sEmailAddress = LoanTeamRow["EmailAddress"].ToString();

        //                    if (sEmailAddress == string.Empty)
        //                    {
        //                        this.AddNewRecipientRow(RecipientListData, "CC", sFullName, "There is no emaill address in the user account.");
        //                    }
        //                    else
        //                    {
        //                        this.AddNewRecipientRow(RecipientListData, "CC", sFullName, sEmailAddress);
        //                    }
        //                }
        //            }
        //        }

        //        #endregion

        //        #region Contact IDs

        //        if (sContactList_CC != string.Empty)
        //        {
        //            string[] ContactArray_CC = sContactList_CC.Split(';');
        //            foreach (string sContactIDCC in ContactArray_CC)
        //            {
        //                int iContactRoleIDCC = Convert.ToInt32(sContactIDCC);

        //                DataRow[] ContactRows = ContactList.Select("ContactRoleId=" + iContactRoleIDCC);
        //                foreach (DataRow ContactRow in ContactRows)
        //                {
        //                    #region Build FullName

        //                    string sFirstName = ContactRow["FirstName"].ToString();
        //                    string sLastName = ContactRow["LastName"].ToString();
        //                    string sMiddleName = ContactRow["MiddleName"].ToString();

        //                    string sFullName = string.Empty;
        //                    if (sMiddleName != string.Empty)
        //                    {
        //                        sFullName = sLastName + ", " + sFirstName + " " + sMiddleName;
        //                    }
        //                    else
        //                    {
        //                        sFullName = sLastName + ", " + sFirstName;
        //                    }

        //                    #endregion

        //                    string sEmailAddress = ContactRow["Email"].ToString();

        //                    if (sEmailAddress == string.Empty)
        //                    {
        //                        this.AddNewRecipientRow(RecipientListData, "CC", sFullName, "There is no emaill address in the contact account.");
        //                    }
        //                    else
        //                    {
        //                        this.AddNewRecipientRow(RecipientListData, "CC", sFullName, sEmailAddress);
        //                    }
        //                }
        //            }
        //        }

        //        #endregion
        //    }

        //    #endregion

        //    this.gridRecipientList.DataSource = RecipientListData;
        //    this.gridRecipientList.DataBind();

        //    #endregion
        //}

        #endregion
        #region 新页面信息绑定 Borrower/Task/EmailTemplate

        #region 加载Borrower Info

        string sSql5 = "select * from LoanContacts as b1 inner join Contacts as b2 on b1.ContactId = b2.ContactId where b1.FileId=" + this.iLoanID + "  and b1.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() ";
        DataTable BorrowerInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql5);

        #endregion

        #region 绑定Borrower信息
        this.lbBorrower.Text = "";
        if (BorrowerInfo.Rows.Count > 0)
        {
            string sBorrowerLastName = BorrowerInfo.Rows[0]["LastName"].ToString();
            string sBorrowerFristName = BorrowerInfo.Rows[0]["FirstName"].ToString();
            string sBorrowerMiddleName = BorrowerInfo.Rows[0]["MiddleName"].ToString();

            StringBuilder sbBorrowerName = new StringBuilder();
            sbBorrowerName.Append(sBorrowerLastName);
            if (sBorrowerFristName != string.Empty)
            {
                sbBorrowerName.Append(", " + sBorrowerFristName);
            }
            if (sBorrowerMiddleName != string.Empty)
            {
                sbBorrowerName.Append(" " + sBorrowerMiddleName);
            }

            this.lbBorrower.Text = sbBorrowerName.ToString();
        }
        #endregion

        #region 加载Task Info

        string sSql6 = "SELECT [Name] FROM [LoanTasks] WHERE LoanTaskId=" + this.iTaskID;
        DataTable TaskInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql6);

        #endregion

        #region 绑定Task Info信息
        this.lbTask.Text = "";
        if (TaskInfo.Rows.Count > 0)
        {
            string taskName = TaskInfo.Rows[0]["Name"].ToString();

            this.lbTask.Text = taskName;
        }
        #endregion


        #region 加载email template

        Template_Email EmailTempManager = new Template_Email();
        EmailTemplates = EmailTempManager.GetEmailTemplate(" and Enabled = 1");

        DataRow NoneEmailTemplateRow = EmailTemplates.NewRow();
        NoneEmailTemplateRow["TemplEmailId"] = 0;
        NoneEmailTemplateRow["Name"] = "None";
        EmailTemplates.Rows.InsertAt(NoneEmailTemplateRow, 0);
        #endregion

        #region 加载 任务邮件模板 并绑定


        LPWeb.BLL.LoanTask_CompletionEmails bllLT_com_Emails = new LoanTask_CompletionEmails();

        var ltceDS = bllLT_com_Emails.GetList("[Enabled]=1 AND LoanTaskid=" + this.iTaskID);

        gridCompletetionEmails.DataSource = ltceDS;//new List<string>(){"t1","t2"};
        gridCompletetionEmails.DataBind();

        #endregion


        #endregion
    }

    /// <summary>
    /// 绑定模板列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridCompletetionEmails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DropDownList ddlEmailTemplate = (DropDownList)e.Row.Cells[0].FindControl("ddlEmailTemplate");
        if (ddlEmailTemplate != null)
        {
            ddlEmailTemplate.DataSource = EmailTemplates;
            ddlEmailTemplate.DataBind();

            var item = (DataRowView)e.Row.DataItem;

            //设置选定模板值

            ddlEmailTemplate.SelectedValue = item["TemplEmailId"].ToString();


            ddlEmailTemplate.Attributes.Add("cid", "ddlEmailTemplate_" + item["TaskCompletionEmailId"].ToString());

            ddlEmailTemplate.Enabled = false;
        }
        

        


    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        ///modify to  EmailSendConmpletion_Background.aspx

        LoginUser CurrentUser = new LoginUser();

        int[] ToUserIDArray = null;
        int[] ToContactIDArray = null;
        string[] ToEmailAddrArray = null;

        int[] CCUserIDArray = null;
        int[] CCContactIDArray = null;
        string[] CCEmailAddrArray = null;

        string Token = hidToken.Value;
        Dictionary<string, byte[]> Attachments = new Dictionary<string, byte[]>();

        #region Attachments

        LPWeb.BLL.Email_AttachmentsTemp bllEmailAttachTemp = new Email_AttachmentsTemp();

        var ds = bllEmailAttachTemp.GetListWithFileImage(iEmailTemplateID, Token);

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

            try
            {
                bllEmailAttachTemp.DeleteByToken(Token);
            }
            catch { }

        }


        #endregion


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

            #region TaskOwner

            if ((sTaskOwner != string.Empty) && (sTaskOwner != null))
            {
                if (sTaskOwner == "True")
                {
                    string sSql_LoanTasks = "select Owner from LoanTasks where LoanTaskId=" + this.iTaskID;
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

        SendEmailResponse response = null;

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                SendEmailRequest req = new SendEmailRequest();
                req.EmailTemplId = this.iEmailTemplateID;
                req.FileId = this.iLoanID;
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

                response = service.SendEmail(req);
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("Failed to send the email, reason: Email Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseDialog_SendCompletionEmail();");
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Failed to send email, error: {0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseDialog_SendCompletionEmail();");
        }

        #endregion

        if (response.resp.Successful == true)
        {
            PageCommon.WriteJsEnd(this, "Sent completion email successfully.", "window.parent.CloseDialog_SendCompletionEmail();");
        }
        else
        {
            PageCommon.WriteJsEnd(this, "Failed to send completion email: " + response.resp.StatusInfo.Replace("'", "\'"), "window.parent.CloseDialog_SendCompletionEmail();");
        }
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
