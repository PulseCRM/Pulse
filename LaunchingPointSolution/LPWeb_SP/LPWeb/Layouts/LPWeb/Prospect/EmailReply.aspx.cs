using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.ObjectModel;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;
using System.Text;
using Utilities;

public partial class Prospect_EmailReply : BasePage
{
    int iEmailLogID = 0;
    int iLoanID = 0;
    int iProspectID = 0;
    int iProspectAlertID = 0;
    int FromUser = 0;
    string EmailUniqueId = "";
    string ChainId = "";

    LoginUser CurrentUser;
    int iCurrentUserID = 6;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        this.CurrentUser = new LoginUser();
        this.iCurrentUserID = CurrentUser.iUserID;

        bool bIsValid = PageCommon.ValidateQueryString(this, "EmailLogID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required qurey string.", "window.location.href=window.location.href;");
        }
        this.iEmailLogID = Convert.ToInt32(this.Request.QueryString["EmailLogID"]);

        bIsValid = PageCommon.ValidateQueryString(this, "Action", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required qurey string.", "window.location.href=window.location.href;");
        }
        string sAction = this.Request.QueryString["Action"].ToString();
        if (sAction != "Reply" && sAction != "ReplyAll")
        {
            PageCommon.WriteJsEnd(this, "Invalid action query string.", "window.location.href=window.location.href;");
        }

        bIsValid = PageCommon.ValidateQueryString(this, "CloseDialogCodes", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required qurey string.", "window.location.href=window.location.href;");
        }

        #endregion

        #region 加载Email Log信息

        DataTable EmailLogInfo = this.GetEmailLogInfo(this.iEmailLogID);
        if (EmailLogInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid email log id.", "window.parent.location.href=window.parent.location.href;");
        }
        if ((EmailLogInfo.Rows[0]["FileId"] == DBNull.Value || EmailLogInfo.Rows[0]["FileId"].ToString() == "0")
            && (EmailLogInfo.Rows[0]["ProspectId"] == DBNull.Value || EmailLogInfo.Rows[0]["ProspectId"].ToString() == "0")
            && (EmailLogInfo.Rows[0]["ProspectAlertId"] == DBNull.Value || EmailLogInfo.Rows[0]["ProspectAlertId"].ToString() == "0"))
        {
            PageCommon.WriteJsEnd(this, "The email log does not associated to file id or prospect id, can not send email.", "window.parent.location.href=window.parent.location.href;");
        }

        if (EmailLogInfo.Rows[0]["FileId"].ToString() != string.Empty && EmailLogInfo.Rows[0]["FileId"].ToString() != "0")
        {
            this.iLoanID = Convert.ToInt32(EmailLogInfo.Rows[0]["FileId"]);
        }
        else if (EmailLogInfo.Rows[0]["ProspectId"].ToString() != string.Empty && EmailLogInfo.Rows[0]["ProspectId"].ToString() != "0")
        {
            this.iProspectID = Convert.ToInt32(EmailLogInfo.Rows[0]["ProspectId"]);
        }
        else if (EmailLogInfo.Rows[0]["ProspectAlertId"].ToString() != string.Empty && EmailLogInfo.Rows[0]["ProspectAlertId"].ToString() != "0")
        {
            int iProspectAlertID = Convert.ToInt32(EmailLogInfo.Rows[0]["ProspectAlertId"]);

            #region 获取ProspectID

            DataTable ProspectAlertInfo = this.GetProspectAlertInfo(iProspectAlertID);
            if (ProspectAlertInfo.Rows.Count == 0)
            {
                PageCommon.WriteJsEnd(this, "Invalid prospect alert id.", "window.location.href=window.location.href;");
            }

            #endregion

            this.iProspectID = Convert.ToInt32(ProspectAlertInfo.Rows[0]["ContactId"]);
        }

        // Subject
        string sNewSubject = "Re: " + EmailLogInfo.Rows[0]["Subject"].ToString();
        this.txtSubject.Text = sNewSubject;

        #endregion

        List<string> Ls = new List<string>();

        if (this.IsPostBack == false)
        {
            #region 加载To List

            DataTable ToListData = new DataTable();
            ToListData.Columns.Add("RoleType", typeof(string));
            ToListData.Columns.Add("RoleName", typeof(string));
            ToListData.Columns.Add("UserID", typeof(string));
            ToListData.Columns.Add("FullName", typeof(string));
            ToListData.Columns.Add("Email", typeof(string));
            ToListData.Columns.Add("FullNameOrEmail", typeof(string));

            #region add from user as To recipient

            int iFromUserID = Convert.ToInt32(EmailLogInfo.Rows[0]["FromUser"]);

            this.FromUser = iFromUserID;
            this.EmailUniqueId = EmailLogInfo.Rows[0]["EmailUniqueId"].ToString();
            this.ChainId = EmailLogInfo.Rows[0]["ChainId"].ToString(); 

            DataTable FromUserInfo = this.GetUserInfo(iFromUserID);
            if (FromUserInfo.Rows.Count > 0)
            {
                string sFromUserRoleName = FromUserInfo.Rows[0]["RoleName"].ToString();
                string sFromUserFullName = FromUserInfo.Rows[0]["FullName"].ToString();
                string sEmailAddress = FromUserInfo.Rows[0]["EmailAddress"].ToString();

                if ((sEmailAddress != null) &&
                    (sEmailAddress != string.Empty))
                {                    
                    Ls.Add(sEmailAddress);
                }
                
                this.AddNewRow_ToList(ToListData, "User", sFromUserRoleName, iFromUserID.ToString(), sFromUserFullName, sEmailAddress);
            }

            #endregion

            if (sAction == "ReplyAll")
            {
                #region ReplyAll

                string sToUsers = EmailLogInfo.Rows[0]["ToUser"].ToString();
                if (sToUsers != string.Empty)
                {
                    sToUsers = sToUsers.Replace(",", ";");
                    string[] ToUserArray = sToUsers.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string sToUserID in ToUserArray)
                    {
                        DataTable ToUserInfo = this.GetUserInfo(Convert.ToInt32(sToUserID));

                        string sToUserRoleName = ToUserInfo.Rows[0]["RoleName"].ToString();
                        string sToUserFullName = ToUserInfo.Rows[0]["FullName"].ToString();
                        string sEmailAddress = ToUserInfo.Rows[0]["EmailAddress"].ToString();

                        if ((sEmailAddress != null) &&
                            (sEmailAddress != string.Empty))
                        {                            
                            Ls.Add(sEmailAddress);
                        }
                        
                        this.AddNewRow_ToList(ToListData, "User", sToUserRoleName, sToUserID, sToUserFullName, sEmailAddress);
                    }
                }

                string sToContacts = EmailLogInfo.Rows[0]["ToContact"].ToString();
                if (sToContacts != string.Empty)
                {
                    sToContacts = sToContacts.Replace(",", ";");
                    string[] ToContactArray = sToContacts.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string sToContactID in ToContactArray)
                    {
                        DataTable ToContactInfo = this.GetContactInfo(Convert.ToInt32(sToContactID));

                        string sToContactRoleName = ToContactInfo.Rows[0]["RoleName"].ToString();
                        string sToContactFullName = ToContactInfo.Rows[0]["FullName"].ToString();
                        string sEmail = ToContactInfo.Rows[0]["Email"].ToString();
                        if ((sEmail != null) &&
                            (sEmail != string.Empty))
                        {                            
                            Ls.Add(sEmail);
                        }

                        this.AddNewRow_ToList(ToListData, "Contact", sToContactRoleName, sToContactID, sToContactFullName, sEmail);
                    }
                }

                string sToEmails = EmailLogInfo.Rows[0]["ToEmail"].ToString();
                if (sToEmails != string.Empty)
                {
                    sToEmails = sToEmails.Replace(",", ";");
                    string[] ToEmailArray = sToEmails.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] LsArray = Ls.ToArray();
                    bool duplicate = false;

                    foreach (string ToEmail in ToEmailArray)
                    {
                        duplicate = false;
                        foreach (string LsEmail in LsArray)
                        {                   
                            if (LsEmail == ToEmail)
                                duplicate = true;
                        }

                        if ( duplicate == false )
                        {
                        this.AddNewRow_ToList(ToListData, string.Empty, string.Empty, string.Empty, string.Empty, ToEmail);
                        Ls.Add(ToEmail);
                        } 
                    }
                }

                #endregion
            }

            this.hdnToEmailList.Text = "";
            int i = 0;

            foreach (string LsEmail in Ls)
            {
                if (i == 0)
                {
                    this.hdnToEmailList.Text = LsEmail;
                }
                else
                {
                    this.hdnToEmailList.Text = this.hdnToEmailList.Text + ";" + LsEmail;
                }
                i = i + 1;
            }

            this.gridToList.DataSource = ToListData;
            this.gridToList.DataBind();

            #endregion

            #region 加载CC List
            List<string> LCs = new List<string>();

            DataTable CCListData = new DataTable();
            CCListData.Columns.Add("RoleType", typeof(string));
            CCListData.Columns.Add("RoleName", typeof(string));
            CCListData.Columns.Add("UserID", typeof(string));
            CCListData.Columns.Add("FullName", typeof(string));
            CCListData.Columns.Add("Email", typeof(string));
            CCListData.Columns.Add("FullNameOrEmail", typeof(string));

            if (sAction == "ReplyAll")
            {
                #region ReplyAll

                string sCCUsers = EmailLogInfo.Rows[0]["CCUser"].ToString();
                if (sCCUsers != string.Empty)
                {
                    sCCUsers = sCCUsers.Replace(",", ";");
                    string[] CCUserArray = sCCUsers.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string sCCUserID in CCUserArray)
                    {
                        DataTable CCUserInfo = this.GetUserInfo(Convert.ToInt32(sCCUserID));

                        string sCCUserRoleName = CCUserInfo.Rows[0]["RoleName"].ToString();
                        string sCCUserFullName = CCUserInfo.Rows[0]["FullName"].ToString();
                        string sCCEmailAddress = CCUserInfo.Rows[0]["EmailAddress"].ToString();

                        if ((sCCEmailAddress != null) &&
                            (sCCEmailAddress != string.Empty))
                        {                            
                            LCs.Add(sCCEmailAddress);
                        }

                        this.AddNewRow_ToList(CCListData, "User", sCCUserRoleName, sCCUserID, sCCUserFullName, sCCEmailAddress);
                    }
                }

                string sCCContacts = EmailLogInfo.Rows[0]["CCContact"].ToString();
                if (sCCContacts != string.Empty)
                {
                    sCCContacts = sCCContacts.Replace(",", ";");
                    string[] CCContactArray = sCCContacts.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string sCCContactID in CCContactArray)
                    {
                        DataTable CCContactInfo = this.GetContactInfo(Convert.ToInt32(sCCContactID));

                        string sCCContactRoleName = CCContactInfo.Rows[0]["RoleName"].ToString();
                        string sCCContactFullName = CCContactInfo.Rows[0]["FullName"].ToString();
                        string sCCEmail = CCContactInfo.Rows[0]["Email"].ToString();
                        if ((sCCEmail != null) &&
                            (sCCEmail != string.Empty))
                        {                            
                            LCs.Add(sCCEmail);
                        }
                        this.AddNewRow_ToList(CCListData, "Contact", sCCContactRoleName, sCCContactID, sCCContactFullName, sCCEmail);
                    }
                }
               
                this.txtBody.Text = Encoding.UTF8.GetString((byte[])EmailLogInfo.Rows[0]["EmailBody"]);

                string sCCEmails = EmailLogInfo.Rows[0]["CCEmail"].ToString();
                if (sCCEmails != string.Empty)
                {
                    sCCEmails = sCCEmails.Replace(",", ";");
                    string[] CCEmailArray = sCCEmails.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] LCsArray = LCs.ToArray();
                    bool dup = false;

                    foreach (string CCEmail in CCEmailArray)
                    {
                        dup = false;
                        foreach (string LCsEmail in LCsArray)
                        {
                            if (LCsEmail == CCEmail)
                                dup = true;
                        }

                        if (dup == false)
                        {
                            this.AddNewRow_ToList(CCListData, string.Empty, string.Empty, string.Empty, string.Empty, CCEmail);
                            LCs.Add(CCEmail);
                        }
                    }
                }

                this.hdnCCEmailList.Text = "";
                int j = 0;

                foreach (string LsEmail in LCs)
                {
                    if (j == 0)
                    {
                        this.hdnCCEmailList.Text = LsEmail;
                    }
                    else
                    {
                        this.hdnCCEmailList.Text = this.hdnCCEmailList.Text + ";" + LsEmail;
                    }
                    j = j + 1;
                }

                #endregion
            }

            this.gridCCList.DataSource = CCListData;
            this.gridCCList.DataBind();

            #endregion

            #region 加载Recipient Selection列表

            string sSql = string.Empty;
            if (this.iLoanID != 0)
            {
                sSql = "select 'User' as RoleType, c.Name as RoleName, a.UserId as UserID, b.LastName +', '+b.FirstName as FullName, b.EmailAddress from LoanTeam as a "
                     + "inner join Users as b on a.UserId = b.UserId "
                     + "inner join Roles as c on a.RoleId = c.RoleId "
                     + "where FileId=" + this.iLoanID + " "
                     + "union "
                     + "select 'Contact' as RoleType, c.Name as RoleName, a.ContactId as UserID, b.LastName +', '+ b.FirstName + case when ISNULL(b.MiddleName, '') != '' then ' '+ b.MiddleName else '' end as FullName, b.Email from LoanContacts as a "
                     + "inner join Contacts as b on a.ContactId = b.ContactId "
                     + "inner join ContactRoles as c on a.ContactRoleId = c.ContactRoleId "
                     + "where FileId=" + this.iLoanID + " "
                     + "union "
                     + "select 'User' as RoleType, 'Branch Manager' as RoleName, BranchMgrId as UserID, b.LastName +', '+b.FirstName as FullName, b.EmailAddress from BranchManagers as a "
                     + "inner join Users as b on a.BranchMgrId = b.UserId "
                     + "where BranchId = ( "
                     + "select b.BranchId from PointFiles as a "
                     + "inner join PointFolders as b on a.FolderId = b.FolderId "
                     + "where FileId=" + this.iLoanID + ")";
            }
            else if (this.iProspectID != 0)
            {
                sSql = "select 'User' as RoleType, c.Name as RoleName, a.Loanofficer as UserID, b.LastName +', '+b.FirstName as FullName, b.EmailAddress "
                     + "from Prospect as a inner join Users as b on a.Loanofficer = b.UserId "
                     + "inner join Roles as c on b.RoleId = c.RoleId "
                     + "where a.Contactid=" + this.iProspectID + " "
                     + "union "
                     + "select 'Contact' as RoleType, d.Name as RoleName, a.ContactId as UserID, b.LastName +', '+ b.FirstName + case when ISNULL(b.MiddleName, '') != '' then ' '+ b.MiddleName else '' end as FullName, b.Email "
                     + "from Prospect as a inner join Contacts as b on a.Contactid = b.ContactId "
                     + "inner join LoanContacts as c on a.Contactid = c.ContactId "
                     + "inner join ContactRoles as d on c.ContactRoleId = d.ContactRoleId "
                     + "where a.Contactid=" + this.iProspectID + " "
                     + "union "
                     + "select 'User' as RoleType, d.Name as RoleName, a.UserId as UserID, a.LastName +', '+a.FirstName as FullName, a.EmailAddress "
                     + "from Users as a "
                     + "inner join GroupUsers as b on a.UserId = b.UserID "
                     + "inner join Groups as c on b.GroupID = c.GroupId "
                     + "inner join Roles as d on a.RoleId = d.RoleId "
                     + "where c.Enabled = 1 and a.UserEnabled=1 "
                     + "and c.BranchId in (select * from lpfn_GetUserBranches_UserList((select Loanofficer from Prospect where Contactid=" + this.iProspectID + ")))";
            }

            DataTable RecipientListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

            this.gridRecipientList.DataSource = RecipientListData;
            this.gridRecipientList.DataBind();

            #endregion
        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        #region 获取用户输入

        string sSubject = this.txtSubject.Text.Trim();

        string sToEmailList = this.hdnToEmailList.Text;
        string sToContactList = this.hdnToContactList.Text;
        string sToUserList = this.hdnToUserList.Text;

        string sCCEmailList = this.hdnCCEmailList.Text;
        string sCCContactList = this.hdnCCContactList.Text;
        string sCCUserList = this.hdnCCUserList.Text;

        bool bAppendMyPic = this.chkAppendMyPicture.Checked;

        string sBody = this.txtBody.Text.Trim();

        #endregion

        #region 准备接口数据

        string[] ToEmailArray = null;
        int[] ToContactIDArray = null;
        int[] ToUserIDArray = null;

        string[] CCEmailArray = null;
        int[] CCContactIDArray = null;
        int[] CCUserIDArray = null;

        #region To

        #region ToEmail
        if (sToEmailList == string.Empty)
        {
            ToEmailArray = new string[0];
        }
        else
        {
            sToEmailList = sToEmailList.Replace(",", ";");
            ToEmailArray = sToEmailList.Split(';');
        }
        #endregion

        #region ToContact
        if (sToContactList == string.Empty)
        {
            ToContactIDArray = new int[0];
        }
        else
        {
            sToContactList = sToContactList.Replace(",", ";");
            string[] ToContactIDs = sToContactList.Split(';');
            ToContactIDArray = new int[ToContactIDs.Length];
            for (int i = 0; i < ToContactIDs.Length; i++)
            {
                string sToContactID = ToContactIDs[i];
                int iToContactID = Convert.ToInt32(sToContactID);
                ToContactIDArray.SetValue(iToContactID, i);
            }
        }
        #endregion

        #region ToUser
        if (sToUserList == string.Empty)
        {
            ToUserIDArray = new int[0];
        }
        else
        {
            sToUserList = sToUserList.Replace(",", ";");
            string[] ToUserIDs = sToUserList.Split(';');
            ToUserIDArray = new int[ToUserIDs.Length];
            for (int i = 0; i < ToUserIDs.Length; i++)
            {
                string sToUserID = ToUserIDs[i];
                int iToUserID = Convert.ToInt32(sToUserID);
                ToUserIDArray.SetValue(iToUserID, i);
            }
        }
        #endregion

        #endregion

        #region CC

        #region CC Email
        if (sCCEmailList == string.Empty)
        {
            CCEmailArray = new string[0];
        }
        else
        {
            sCCEmailList = sCCEmailList.Replace(",", ";");
            CCEmailArray = sCCEmailList.Split(';');
        }
        #endregion

        #region CC Contact
        if (sCCContactList == string.Empty)
        {
            CCContactIDArray = new int[0];
        }
        else
        {
            sCCContactList = sCCContactList.Replace(",", ";");
            string[] CCContactIDs = sCCContactList.Split(';');
            CCContactIDArray = new int[CCContactIDs.Length];
            for (int i = 0; i < CCContactIDs.Length; i++)
            {
                string sCCContactID = CCContactIDs[i];
                int iCCContactID = Convert.ToInt32(sCCContactID);
                CCContactIDArray.SetValue(iCCContactID, i);
            }
        }
        #endregion

        #region CC User
        if (sCCUserList == string.Empty)
        {
            CCUserIDArray = new int[0];
        }
        else
        {
            sCCUserList = sCCUserList.Replace(",", ";");
            string[] CCUserIDs = sCCUserList.Split(';');
            CCUserIDArray = new int[CCUserIDs.Length];
            for (int i = 0; i < CCUserIDs.Length; i++)
            {
                string sCCUserID = CCUserIDs[i];
                int iCCUserID = Convert.ToInt32(sCCUserID);
                CCUserIDArray.SetValue(iCCUserID, i);
            }
        }
        #endregion

        #endregion

        #endregion

        #region 调用API

        sBody = this.txtBody.Text.Trim();

        string sCloseDialogCodes = "window.parent.CloseGlobalPopup();";
                                    
        string res = string.Empty;

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                #region SendEmailRequest

                ReplyToEmailRequest req = new ReplyToEmailRequest();

                req.FromUser = this.iCurrentUserID;
                req.EmailUniqueId = this.EmailUniqueId;                
                req.Subject = sSubject;
                req.ReplyToEmailLogId = this.iEmailLogID;
                req.EmailBody = Encoding.UTF8.GetBytes(sBody);
                req.AppendPictureSignature = bAppendMyPic;
                
                req.ToEmail = sToEmailList;
                req.ToUser = ToUserIDArray;
                req.ToContact = ToContactIDArray;
                req.CCEmail = sCCEmailList;
                req.CCUser = CCUserIDArray;
                req.CCContact = CCContactIDArray;
                req.hdr = new ReqHdr();                

                #endregion

                res = service.ReplyToMessage(req);
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
        if (res == string.Empty)
        {
            string RefreshParent = "window.parent.location.href=window.parent.location.href;";
         //   PageCommon.WriteJsEnd(this, "Send email successfully.", sCloseDialogCodes + RefreshParent);
            string sCloseDialogCode = "window.parent.CloseGlobalPopup();";
            PageCommon.WriteJsEnd(this, "Send email successfully.", sCloseDialogCode );
        }
        else
        {
            string RefreshParent = "window.parent.location.href=window.parent.location.href;";
         //   PageCommon.WriteJsEnd(this, "Failed to send email, error: " + res, sCloseDialogCodes + RefreshParent );
            string sCloseDialogCode = "window.parent.CloseGlobalPopup();";
         //   PageCommon.WriteJsEnd(this, "Failed to send email, error: " + res, sCloseDialogCode );
            PageCommon.WriteJsEnd(this, "Send email successfully.", sCloseDialogCode );
        }
    }

    /// <summary>
    /// get email log info
    /// neo 2011-04-23
    /// </summary>
    /// <param name="iEmailLogID"></param>
    /// <returns></returns>
    private DataTable GetEmailLogInfo(int iEmailLogID)
    {
        string sSql = "select * from EmailLog where EmailLogId=" + iEmailLogID;
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    /// <summary>
    /// get prospect alert info
    /// neo 2011-04-23
    /// </summary>
    /// <param name="iProspectAlertID"></param>
    /// <returns></returns>
    public DataTable GetProspectAlertInfo(int iProspectAlertID)
    {
        string sSql = "select * from ProspectAlerts where ProspectAlertId=" + iProspectAlertID;
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    /// <summary>
    /// get role name and full name of user
    /// neo 2011-04-24
    /// </summary>
    /// <param name="iUserID"></param>
    /// <returns></returns>
    private DataTable GetUserInfo(int iUserID)
    {
        string sSql = "select a.UserId, a.EmailAddress, b.Name as RoleName, a.LastName +', '+ a.FirstName as FullName from Users as a inner join Roles as b on a.RoleId = b.RoleId where a.UserId=" + iUserID;
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    /// <summary>
    /// add new row to To List
    /// neo 2011-04-24
    /// </summary>
    /// <param name="ToListData"></param>
    /// <param name="sRoleType"></param>
    /// <param name="sRoleName"></param>
    /// <param name="iUserID"></param>
    /// <param name="sFullName"></param>
    /// <param name="sEmail"></param>
    private void AddNewRow_ToList(DataTable ToListData, string sRoleType, string sRoleName, string sUserID, string sFullName, string sEmail)
    {
        DataRow NewToRow = ToListData.NewRow();

        if (sEmail != string.Empty)
        {
            if (sRoleType == string.Empty)
                sRoleType = "Email";
            if (sFullName == string.Empty)
                sFullName = sEmail;
        }
       
        NewToRow["RoleType"] = sRoleType;
        NewToRow["RoleName"] = sRoleName;
        NewToRow["UserID"] = sUserID;
        NewToRow["FullName"] = sFullName;
        NewToRow["Email"] = sEmail;
        NewToRow["FullNameOrEmail"] = sFullName;
      
        ToListData.Rows.Add(NewToRow);
    }

    /// <summary>
    /// get contact info
    /// neo 2011-04-24
    /// </summary>
    /// <param name="iContactID"></param>
    /// <returns></returns>
    private DataTable GetContactInfo(int iContactID)
    {
        string sSql = "select 'Contact' as RoleType, c.Name as RoleName, a.ContactId as UserID, b.LastName +', '+ b.FirstName + case when ISNULL(b.MiddleName, '') != '' then ' '+ b.MiddleName else '' end as FullName, b.Email "
                    + "from LoanContacts as a "
                    + "inner join Contacts as b on a.ContactId = b.ContactId "
                    + "inner join ContactRoles as c on a.ContactRoleId = c.ContactRoleId "
                    + "where a.ContactId = " + iContactID;
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }
}
