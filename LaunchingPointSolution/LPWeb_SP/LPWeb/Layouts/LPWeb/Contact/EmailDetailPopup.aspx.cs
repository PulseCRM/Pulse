using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using System.Text;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;


public partial class Contact_EmailDetailPopup : BasePage
{
    int iEmailLogID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";

        bool bIsValid = PageCommon.ValidateQueryString(this, "EmailLogID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", sCloseDialogCodes);
        }
        this.iEmailLogID = Convert.ToInt32(this.Request.QueryString["EmailLogID"]);

        #endregion

        #region 加载EmailLog

        DataTable EmailLogList = this.GetEmailLogInfo(this.iEmailLogID);
        if (EmailLogList.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid Email Log ID.", sCloseDialogCodes);
        }

        string sChainId = EmailLogList.Rows[0]["ChainId"].ToString();
        if (sChainId != string.Empty)
        {
            EmailLogList = this.GetEmailLogList(this.iEmailLogID);
        }

        this.rptEmailLogList.DataSource = EmailLogList;
        this.rptEmailLogList.DataBind();

        #endregion
    }

    public string GetFromUserFullName(string sUserID)
    {
        if (sUserID == string.Empty || sUserID == "0")
        {
            return string.Empty;
        }

        return this.GetUserFullName(Convert.ToInt32(sUserID));
    }

    public string GetEmailStatus(string sSuccess)
    {
        if (sSuccess == "True")
        {
            return "Success";
        }
        else
        {
            return "Failure";
        }
    }

    public string GetToListRows(string sToUserIDs, string sToContactIDs, string sToEmails)
    {
        StringBuilder sbToList = new StringBuilder();
        bool apply_flag = true;
        string First_Email = "";
        List<string> Exist = new List<string>();

        if (sToUserIDs == string.Empty && sToContactIDs == string.Empty && sToEmails == string.Empty)
        {
            sbToList.AppendLine("<tr class=\"EmptyDataRow\" align=\"center\"><td colspan=\"2\">There is no recipient.</td></tr>");
        }
        else
        {
            sbToList.AppendLine("<tr><th scope=\"col\">Role/Type</th><th scope=\"col\">Name/Email</th></tr>");
        }

        if (sToEmails != string.Empty)
        {
            string[] ToEmailArray = sToEmails.Split(';');
            foreach (string sEmail in ToEmailArray)
            {
                First_Email = sEmail;
                break;
            }
        }

        if (sToContactIDs != string.Empty)
        {
            StringBuilder sbToContactIDs = new StringBuilder(sToContactIDs);
            sbToContactIDs.Replace(";", ",");

            DataTable ToContactListData = this.GetToContactList(sbToContactIDs.ToString());

            foreach (DataRow ContactRow in ToContactListData.Rows)
            {
                string sRoleName = ContactRow["RoleName"].ToString();
                string sEmail = ContactRow["Email"].ToString();
                if (sEmail == First_Email)
                {
                    apply_flag = false;
                }
                Exist.Add(sEmail);
                string sFirstName = ContactRow["FirstName"].ToString();
                string sLastName = ContactRow["LastName"].ToString();
                string sMiddleName = ContactRow["MiddleName"].ToString();

                string sFullName = sLastName + ", " + sFirstName;
                if (sMiddleName != string.Empty)
                {
                    sFullName += " " + sMiddleName;
                }
                if (apply_flag == false)
                {
                    sbToList.AppendLine("<tr><td style=\"width: 90px;\">" + sRoleName + "</td><td>" + sFullName + "</td></tr>");
                }
            }
        }

        if (sToUserIDs != string.Empty)
        {
            StringBuilder sbToUserIDs = new StringBuilder(sToUserIDs);
            sbToUserIDs.Replace(";", ",");

            DataTable ToUserListData = this.GetToUserList(sbToUserIDs.ToString());

            foreach (DataRow UserRow in ToUserListData.Rows)
            {
                string sRoleName = UserRow["RoleName"].ToString();
                string sFullName = UserRow["FullName"].ToString();
                string sEmailAddress = UserRow["EmailAddress"].ToString();
                Exist.Add(sEmailAddress);
                sbToList.AppendLine("<tr><td style=\"width: 90px;\">" + sRoleName + "</td><td>" + sFullName + "</td></tr>");
            }
        }

        if (apply_flag == true)
        {
            if (sToContactIDs != string.Empty)
            {
                StringBuilder sbToContactIDs = new StringBuilder(sToContactIDs);
                sbToContactIDs.Replace(";", ",");

                DataTable ToContactListData = this.GetToContactList(sbToContactIDs.ToString());

                foreach (DataRow ContactRow in ToContactListData.Rows)
                {
                    string sRoleName = ContactRow["RoleName"].ToString();

                    string sFirstName = ContactRow["FirstName"].ToString();
                    string sLastName = ContactRow["LastName"].ToString();
                    string sMiddleName = ContactRow["MiddleName"].ToString();

                    string sFullName = sLastName + ", " + sFirstName;
                    if (sMiddleName != string.Empty)
                    {
                        sFullName += " " + sMiddleName;
                    }

                    sbToList.AppendLine("<tr><td style=\"width: 90px;\">" + sRoleName + "</td><td>" + sFullName + "</td></tr>");
                }
            }
        }

        if (sToEmails != string.Empty)
        {
            bool notExist = true;
            string[] ExistStr = Exist.ToArray();
            string[] ToEmailArray = sToEmails.Split(';');
            foreach (string sEmail in ToEmailArray)
            {
                notExist = true;
                foreach (string ex in ExistStr)
                {
                    if (sEmail == ex)
                        notExist = false;
                }

                if (notExist == true)
                {
                    sbToList.AppendLine("<tr><td style=\"width: 90px;\">&nbsp;</td><td>" + sEmail + "</td></tr>");
                }
            }
        }

        return sbToList.ToString();
    }

    public string GetUserFullName(int iUserID)
    {
        string sSql = "select LastName + ', ' + FirstName as FullName from Users where UserID=" + iUserID;
        DataTable FullNameInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        if (FullNameInfo.Rows.Count == 0)
        {
            return string.Empty;
        }
        return FullNameInfo.Rows[0]["FullName"].ToString();
    }

    private DataTable GetEmailLogInfo(int iEmailLogID)
    {
        string sSql = "select * from EmailLog where EmailLogId=" + iEmailLogID;
        DataTable EmailLogInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        return EmailLogInfo;
    }

    private DataTable GetEmailLogList(int iEmailLogID)
    {
        string sSql = "select * from EmailLog where ChainId = (select ChainId from EmailLog where EmailLogId=" + iEmailLogID + ") order by SequenceNumber";
        DataTable EmailLogList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        return EmailLogList;
    }

    private DataTable GetToUserList(string sUserIDs)
    {
        string sSql = "select top 1 a.EmailAddress, a.UserId, LastName + ', ' + FirstName as FullName, b.Name as RoleName from Users as a inner join Roles as b on a.RoleId = b.RoleId where UserId in (" + sUserIDs + ")";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private DataTable GetToContactList(string sContactIDs)
    {
        string sSql = "select top 1 a.ContactId, b.Name as RoleName, c.FirstName, c.LastName, c.MiddleName, c.Email "
                    + "from LoanContacts as a inner join ContactRoles as b on a.ContactRoleId = b.ContactRoleId "
                    + "inner join Contacts as c on a.ContactId = c.ContactId "
                    + "where a.ContactId in (" + sContactIDs + ")";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }
}
