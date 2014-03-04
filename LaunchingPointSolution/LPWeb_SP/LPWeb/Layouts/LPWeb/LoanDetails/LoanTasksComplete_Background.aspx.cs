using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using System.Text;
using LPWeb.LP_Service;
using System.Data;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb;
using Utilities;
using System.Collections.ObjectModel;

public partial class LoanTasksComplete_Background : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 接收参数

        // TaskID
        bool bIsValid = false;
        //bool bIsValid = PageCommon.ValidateQueryString(this, "TaskID", QueryStringType.ID);
        //if (bIsValid == false)
        //{
        //    this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
        //    return;
        //}
        string sTaskIDs = this.Request.QueryString["TaskIDs"].ToString();

        //string sTaskID = this.Request.QueryString["TaskID"].ToString();
        //int iTaskID = Convert.ToInt32(sTaskID);

        string sSendEmail = this.Request.QueryString["SendEmail"].ToString();
        // LoanID
        bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            return;
        }
        string sLoanID = this.Request.QueryString["LoanID"].ToString();
        int iLoanID = Convert.ToInt32(sLoanID);

        #endregion

        // json示例
        // {"ExecResult":"Success","ErrorMsg":"","EmailTemplateID":"1", "LoanClosed":"Yes"}
        // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}

        string sErrorMsg = string.Empty;
        string sEmailTemplateID = string.Empty;
        bool bIsSuccess = false;
        string LoanClosed = "No";
        var result = "";
        int iLoanStageID = 0;
        try
        {
            string[] TaskIDs = sTaskIDs.Split(",".ToCharArray());

            for (int i = 0; i < TaskIDs.Length; i++)
            {
                #region complete task
                int iTaskID = Convert.ToInt32(TaskIDs[i]);
                int iEmailTemplateId = 0;
                bIsSuccess = LPWeb.DAL.WorkflowManager.CompleteTask(iTaskID, this.CurrUser.iUserID, ref iEmailTemplateId);

                if (bIsSuccess == false)
                {
                    sErrorMsg = "Failed to complete the task(s) due to technical problem. Please contact focusIT Technical Support.";
                    return;
                }
                if (iEmailTemplateId != 0 && sSendEmail=="OK")
                {
                    sEmailTemplateID = iEmailTemplateId.ToString();
                    sErrorMsg = SendEmail(iLoanID, iTaskID, iEmailTemplateId);
                }
                #endregion

                #region update point file stage


                #region get loan task info

                LoanTasks LoanTaskManager = new LoanTasks();
                DataTable LoanTaskInfo = LoanTaskManager.GetLoanTaskInfo(iTaskID);
                if (LoanTaskInfo.Rows.Count == 0)
                {
                    bIsSuccess = false;
                    sErrorMsg = "Invalid task id.";
                    return;
                }
                string sLoanStageID = LoanTaskInfo.Rows[0]["LoanStageId"].ToString();
                if (sLoanStageID == string.Empty)
                {
                    bIsSuccess = false;
                    sErrorMsg = "Invalid loan stage id.";
                    return;
                }
                iLoanStageID = Convert.ToInt32(sLoanStageID);

                #endregion
                bIsSuccess = true;
                if (!WorkflowManager.StageCompleted(iLoanStageID))
                {
                    sErrorMsg = "Completed task successfully.";
                    continue;
                }

                #region invoke PointManager.UpdateStage()
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient service = sm.StartServiceClient())
                {
                    CheckPointFileStatusReq checkFileReq = new CheckPointFileStatusReq();
                    checkFileReq.hdr = new ReqHdr();
                    checkFileReq.hdr.UserId = CurrUser.iUserID;
                    checkFileReq.hdr.SecurityToken = "SecurityToken";
                    checkFileReq.FileId = iLoanID;
                    CheckPointFileStatusResp checkFileResp = service.CheckPointFileStatus(checkFileReq);
                    if (checkFileResp == null || checkFileResp.hdr == null || !checkFileResp.hdr.Successful)
                    {
                        sErrorMsg = "Unable to get Point file status from Point Manager.";
                        WorkflowManager.UnCompleteTask(iTaskID, CurrUser.iUserID);
                        bIsSuccess = false;
                        return;
                    }
                    if (checkFileResp.FileLocked)
                    {
                        sErrorMsg = checkFileResp.hdr.StatusInfo;
                        WorkflowManager.UnCompleteTask(iTaskID, CurrUser.iUserID);
                        bIsSuccess = false;
                        return;
                    }
                }

                string sError = LoanTaskCommon.UpdatePointFileStage(iLoanID, this.CurrUser.iUserID, iLoanStageID);

                // the last one, sleep 1 second
                System.Threading.Thread.Sleep(1000);

                if (sError == string.Empty) // success
                {
                    sErrorMsg = "Completed task successfully.";
                }
                else
                {
                    sErrorMsg = "Completed task successfully but failed to update stage date in Point.";
                    //sErrorMsg = "Failed to update point file stage: " + sError.Replace("\"", "\\\"");
                }
                if (WorkflowManager.IsLoanClosed(iLoanID))
                    LoanClosed = "Yes";
                #endregion
                #endregion

            }
            return;

        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            sErrorMsg = "Completed task successfully but failed to update stage date in Point.";
            return;
        }
        catch (Exception ex)
        {
            if (bIsSuccess)
                sErrorMsg = "Completed task successfully but encountered an error:" + ex.Message;
            else
                sErrorMsg = "Failed to complete task, reason:" + ex.Message;
            //sErrorMsg = "Exception happened when invoke WorkflowManager.CompleteTask: " + ex.ToString().Replace("\"", "\\\"");
            bIsSuccess = false;
            return;
        }
        finally
        {
            if (bIsSuccess)
                result = "{\"ExecResult\":\"Success\",\"ErrorMsg\":\"" + sErrorMsg + "\",\"TaskID\":\"" + sTaskIDs + "\",\"LoanClosed\":\"" + LoanClosed + "\"}";
            //result = "{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\",\"EmailTemplateID\":\"" + sEmailTemplateID + "\",\"LoanClosed\":\"" + LoanClosed + "\"}";
            else
                result = "{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}";
            this.Response.Write(result);
        }

    }


    protected string SendEmail(int iLoanID, int iTaskID, int iEmailTemplateID)
    {
        string ErorMsg = string.Empty;
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
        DataTable RecipientList = EmailTemplateManager.GetRecipientList(iEmailTemplateID);

        #endregion

        #region 获取Loan Team

        string sSql = "select * from LoanTeam where FileId = " + iLoanID;
        DataTable LoanTeamList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        #endregion

        #region 获取Contacts

        string sSql2 = "select * from LoanContacts where FileId = " + iLoanID;
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
                    string sSql_LoanTasks = "select Owner from LoanTasks where LoanTaskId=" + iTaskID;
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
                req.EmailTemplId = iEmailTemplateID;
                req.FileId = iLoanID;
                //req.UserId = CurrentUser.iUserID;
                req.ToEmails = ToEmailAddrArray;
                req.ToUserIds = ToUserIDArray;
                req.ToContactIds = ToContactIDArray;
                req.CCEmails = CCEmailAddrArray;
                req.CCUserIds = CCUserIDArray;
                req.CCContactIds = CCContactIDArray;
                req.hdr = new ReqHdr();

                response = service.SendEmail(req);
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("Failed to send the email, reason: Email Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Failed to send email, error: {0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
        }
        finally
        {

            if (response.resp.Successful == true)
            {
                ErorMsg = "Sent completion email successfully.";
            }
            else
            {
                ErorMsg = "Failed to send completion email: " + response.resp.StatusInfo.Replace("'", "\'");
            }
        }
        #endregion
        return ErorMsg;
    }

}