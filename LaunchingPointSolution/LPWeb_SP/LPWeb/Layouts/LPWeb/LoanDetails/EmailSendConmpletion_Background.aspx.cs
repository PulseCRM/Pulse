using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.BLL;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;



public partial class EmailSendConmpletion_Background : Page
{
    int iLoanID = 0;
    int iTaskID = 0;
    List<int> iEmailTemplateIDList = new List<int>();

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 参数校验并获取
        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            return;
        }
        this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

        bIsValid = PageCommon.ValidateQueryString(this, "EmailTemplateID", QueryStringType.String);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            return;
        }

        this.iEmailTemplateIDList = this.Request.QueryString["EmailTemplateID"].Trim().Split(',').Select(c => Convert.ToInt32(c)).ToList();

        string sTaskID = "";
        sTaskID = this.Request.QueryString["TaskID"];
        if ((sTaskID != null) && (sTaskID != ""))
        {
            this.iTaskID = Convert.ToInt32(sTaskID);
        }
        #endregion

        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"

        SendEmail();
    }

    /// <summary>
    /// 发送邮件(旧发送方式)
    /// </summary>
    public void SendEmailOld()
    {
        LoginUser CurrentUser = new LoginUser();
        string Token = Request.QueryString["Token"] != null ? Request.QueryString["Token"].ToString() : "";
        LPWeb.BLL.Email_AttachmentsTemp bllEmailAttachTemp = new Email_AttachmentsTemp();
        var errMsg = string.Empty;
        //遍历所有模板 发送邮件
        foreach (var iEmailTemplateID in iEmailTemplateIDList)
        {
            string sEmailTemplateName =string.Empty;
            int[] ToUserIDArray = null;
            int[] ToContactIDArray = null;
            string[] ToEmailAddrArray = null;

            int[] CCUserIDArray = null;
            int[] CCContactIDArray = null;
            string[] CCEmailAddrArray = null;

            
            Dictionary<string, byte[]> Attachments = new Dictionary<string, byte[]>();

            #region Attachments

            

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



            }


            #endregion


            #region use To and CC of email template

            #region 获取Email Template的Recipient(s)

            Template_Email EmailTemplateManager = new Template_Email();
            DataTable RecipientList = EmailTemplateManager.GetRecipientList(iEmailTemplateID);
            var emailTemplatemodel = EmailTemplateManager.GetModel(iEmailTemplateID);
            if (emailTemplatemodel != null)
            {
                sEmailTemplateName = emailTemplatemodel.Name;
            }
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

                        if ((sOwnerId != string.Empty) && (sOwnerId != null))
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


            if (ToUserIDs.Count == 0 && ToEmailList.Count == 0 && ToContactIDs.Count == 0
                && CCUserIDs.Count == 0 && CCContactIDs.Count == 0 && CCEmailList.Count == 0)
            {
                errMsg = "No email recipient !   email template: " + sEmailTemplateName;
                break;
            }

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
                    req.EmailTemplId = iEmailTemplateID;
                    req.FileId = this.iLoanID;
                    //req.UserId = CurrentUser.iUserID;
                    req.UserId = 0;
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
                //PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseDialog_SendCompletionEmail();");
                response = new SendEmailResponse() { resp = new RespHdr() { Successful = false, StatusInfo = sExMsg } };
            }
            catch (Exception ex)
            {
                string sExMsg = string.Format("Failed to send email, error: {0}", ex.Message);
                LPLog.LogMessage(LogType.Logerror, sExMsg);
                //PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseDialog_SendCompletionEmail();");
                response = new SendEmailResponse() { resp = new RespHdr() { Successful = false, StatusInfo = sExMsg } };
            }

            #endregion

            if (response.resp.Successful != true)
            {
                //PageCommon.WriteJsEnd(this, "Failed to send completion email: " + response.resp.StatusInfo.Replace("'", "\'"), "window.parent.CloseDialog_SendCompletionEmail();");
                errMsg = response.resp.StatusInfo.Replace("'", "\'");
                break;
            }
            //有一个邮件发送错误都报发送错误并退出发送

        }

        if (string.IsNullOrEmpty(errMsg))
        {
            try
            {
                bllEmailAttachTemp.DeleteByToken(Token);
            }
            catch { }
            this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
            return;
        }
        else
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + errMsg + "\"}");
            return;
        }

    }
    /// <summary>
    /// 发送邮件
    /// </summary>
    public void SendEmail()
    {
        SendEmailOld();
        return;
 /*       int[] EmailTemplIds = new int[iEmailTemplateIDList.Count];

        this.iEmailTemplateIDList.CopyTo(EmailTemplIds);

        LoginUser CurrentUser = new LoginUser();
        var errMsg = string.Empty;


        #region 调用API

        SendEmailResponse response = null;

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                SendEmailRequest req = new SendEmailRequest();
                //req.EmailTemplId = iEmailTemplateID;
                req.EmailTemplIds = EmailTemplIds;
                req.FileId = this.iLoanID;
                req.UserId = CurrentUser.iUserID;
                req.hdr = new ReqHdr();

                response = service.SendEmail(req);
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("Failed to send the email, reason: Email Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            //PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseDialog_SendCompletionEmail();");
            response = new SendEmailResponse() { resp = new RespHdr() { Successful = false, StatusInfo = sExMsg } };
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Failed to send email, error: {0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            //PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseDialog_SendCompletionEmail();");
            response = new SendEmailResponse() { resp = new RespHdr() { Successful = false, StatusInfo = sExMsg } };
        }

        #endregion

        if (response.resp.Successful != true)
        {
            //PageCommon.WriteJsEnd(this, "Failed to send completion email: " + response.resp.StatusInfo.Replace("'", "\'"), "window.parent.CloseDialog_SendCompletionEmail();");
            errMsg = response.resp.StatusInfo.Replace("'", "\'");
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + errMsg + "\"}");
            return;
        }
        else
        {
            this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
            return;
        } */

    }
}

