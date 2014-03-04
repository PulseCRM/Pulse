using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.Layouts.LPWeb;
using System.Linq;
using Utilities;
using System.Collections.ObjectModel;

public partial class LoanDetails_LoanTaskAdd : BasePage
{
    int iLoanID = 0;
    int iCurrentLoanStageId = 0;
    LoanTasks LoanTaskManager = new LoanTasks();
    Loans LoanManager = new Loans();

    protected void Page_Load(object sender, EventArgs e)
    {
        string sErrorJs = "window.parent.CloseDialog_AddTask();";

        #region 检查必要参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.RegisterJsMsg(this, "Missing required query string.", sErrorJs);
            return;
        }

        this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

        #endregion

        this.hdnNow.Value = DateTime.Now.ToString("MM/dd/yyyy");

        #region 获取Borrower和Property信息

        #region Property

        DataTable LoanInfo = this.LoanManager.GetLoanInfo(this.iLoanID);
        if (LoanInfo.Rows.Count == 0)
        {
            PageCommon.RegisterJsMsg(this, "Invalid required query string.", sErrorJs);
            return;
        }
        string sPropertyAddress = LoanInfo.Rows[0]["PropertyAddr"].ToString();
        string sPropertyCity = LoanInfo.Rows[0]["PropertyCity"].ToString();
        string sPropertyState = LoanInfo.Rows[0]["PropertyState"].ToString();
        string sPropertyZip = LoanInfo.Rows[0]["PropertyZip"].ToString();

        string sProperty = sPropertyAddress + ", " + sPropertyCity + ", " + sPropertyState + " " + sPropertyZip;

        // 存储Loan.EstCloseDate
        if (LoanInfo.Rows[0]["EstCloseDate"] != DBNull.Value)
        {
            this.hdnEstCloseDate.Value = Convert.ToDateTime(LoanInfo.Rows[0]["EstCloseDate"]).ToString("MM/dd/yyyy");
        }

        #endregion

        #region Borrower

        DataTable BorrowerInfo = this.LoanManager.GetBorrowerInfo(this.iLoanID);
        if (BorrowerInfo.Rows.Count == 0)
        {
            PageCommon.RegisterJsMsg(this, "There is no Borrower in this loan.", sErrorJs);
            return;
        }
        string sFirstName = BorrowerInfo.Rows[0]["FirstName"].ToString();
        string sMiddleName = BorrowerInfo.Rows[0]["MiddleName"].ToString();
        string sLastName = BorrowerInfo.Rows[0]["LastName"].ToString();

        string sBorrower = sLastName + ",  " + sFirstName;
        if (sMiddleName != string.Empty)
        {
            sBorrower += " " + sMiddleName;
        }

        this.lbProperty.Text = sProperty;
        this.lbBorrower.Text = sBorrower;

        #endregion

        #endregion

        if (this.IsPostBack == false)
        {
            #region 加载Stage

            DataTable LoanStages = this.LoanManager.GetLoanStages(this.iLoanID);
            if (LoanStages == null || LoanStages.Rows.Count <= 0)
            {
                  this.iCurrentLoanStageId = GetTaskStageID(this.iLoanID);
                  if ((this.iCurrentLoanStageId == null) ||
                      (this.iCurrentLoanStageId <= 0))
                  {
                      this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed0", "$('#divContainer').hide();alert('The loan does not have any stage to add the task to.');window.parent.RefreshPage();", true);
                      return;
                  }
            }
            this.ddlStage.DataSource = LoanStages;
            this.ddlStage.DataBind();

            this.ddlStage2.DataSource = LoanStages;
            this.ddlStage2.DataBind();

            #endregion

            #region 加载Owner

            DataTable OwnerList = this.LoanTaskManager.GetLoanTaskOwers(this.iLoanID);

            DataRow EmptyOwnerRow = OwnerList.NewRow();
            EmptyOwnerRow["UserID"] = 0;
            EmptyOwnerRow["FullName"] = "-- select a task owner--";
            OwnerList.Rows.InsertAt(EmptyOwnerRow, 0);

            this.ddlOwner.DataSource = OwnerList;
            this.ddlOwner.DataBind();

            #endregion

            #region 加载Prerequisite

            string sSelectedStageID = string.Empty;
            if (this.Request.QueryString["Stage"] == null)
            {
                sSelectedStageID = this.ddlStage.SelectedItem.Value;
            }
            else
            {
                sSelectedStageID = this.Request.QueryString["Stage"].ToString();
            }

            DataTable PrerequisiteList = this.LoanTaskManager.GetPrerequisiteList(" and FileID=" + this.iLoanID + " and LoanStageId = " + sSelectedStageID + " and PrerequisiteTaskId is null");
            DataRow NonePrerequisiteRow = PrerequisiteList.NewRow();
            NonePrerequisiteRow["LoanTaskId"] = 0;
            NonePrerequisiteRow["Name"] = "None";
            PrerequisiteList.Rows.InsertAt(NonePrerequisiteRow, 0);

            this.ddlPrerequisite.DataSource = PrerequisiteList;
            this.ddlPrerequisite.DataBind();

            this.ddlPrerequisite2.DataSource = PrerequisiteList;
            this.ddlPrerequisite2.DataBind();

            #endregion

            #region 加载email template

            Template_Email EmailTempManager = new Template_Email();
            DataTable EmailTemplates = EmailTempManager.GetEmailTemplate(" and Enabled = 1");

            DataRow NoneEmailTemplateRow = EmailTemplates.NewRow();
            NoneEmailTemplateRow["TemplEmailId"] = 0;
            NoneEmailTemplateRow["Name"] = "None";
            EmailTemplates.Rows.InsertAt(NoneEmailTemplateRow, 0);

            this.ddlCompletionEmail.DataSource = EmailTemplates;
            this.ddlCompletionEmail.DataBind();

            this.ddlWarningEmail.DataSource = EmailTemplates;
            this.ddlWarningEmail.DataBind();

            this.ddlOverdueEmail.DataSource = EmailTemplates;
            this.ddlOverdueEmail.DataBind();

            this.ddlEmailTemplate.DataSource = EmailTemplates;
            this.ddlEmailTemplate.DataBind();
            #endregion

            gridCompletetionEmails.DataSource = null;
            gridCompletetionEmails.DataBind();
        }
    }

    protected int GetTaskStageID(int iFileId)
    {
        //LPWeb.BLL.WorkflowManager wm = new LPWeb.BLL.WorkflowManager();
        int iCurrentLoanStageId = LPWeb.BLL.WorkflowManager.GetCurrentLoanStageId(iFileId);
        if (iCurrentLoanStageId > 0)
            return iCurrentLoanStageId;
        int iDefaultWorkflowTempl = LPWeb.BLL.WorkflowManager.GetDefaultWorkflowTemplate("Prospect");
        if (iDefaultWorkflowTempl > 0)
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                GenerateWorkflowRequest req = new GenerateWorkflowRequest();
                req.hdr = new ReqHdr();
                req.hdr.SecurityToken = string.Empty;
                req.hdr.UserId = CurrUser.iUserID;
                req.FileId = iFileId;
                req.WorkflowTemplId = iDefaultWorkflowTempl;
                GenerateWorkflowResponse resp = service.GenerateWorkflow(req);
                if ((resp == null) || (!resp.hdr.Successful))
                {
                    return iCurrentLoanStageId;
                }
                iCurrentLoanStageId = LPWeb.BLL.WorkflowManager.GetCurrentLoanStageId(iFileId);
                return iCurrentLoanStageId;
            }
            return iCurrentLoanStageId;
        }
        iCurrentLoanStageId = LPWeb.BLL.WorkflowManager.GenerateDefaultLoanStages(iFileId, "Prospect");
        return iCurrentLoanStageId;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region 获取数据

        DataTable LoanStages = this.LoanManager.GetLoanStages(this.iLoanID);
        if (LoanStages == null || LoanStages.Rows.Count <= 0)
        {
              this.iCurrentLoanStageId = GetTaskStageID(this.iLoanID);
              if ((this.iCurrentLoanStageId == null) ||
                  (this.iCurrentLoanStageId <= 0))
              {
                  this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed0", "$('#divContainer').hide();alert('The loan does not have any stage to add the task to.');window.parent.RefreshPage();", true);
                  return;
              }
        }      

        //int iStageID = Convert.ToInt32(this.ddlStage.SelectedItem.Value);
        int iStageID = string.IsNullOrEmpty(ddlStage.SelectedValue) ? this.iCurrentLoanStageId : Convert.ToInt32(ddlStage.SelectedValue);  //CR54 this.iCurrentLoanStageId;
        string sTaskName = this.txtTaskName.Text.Trim();

        int iOwnerID = Convert.ToInt32(this.ddlOwner.SelectedItem.Value);
        string sDueDate = this.txtDueDate.Text.Trim();

        int iDaysToEstClose = 0;
        if (this.txtDaysToEst.Text != string.Empty)
        {
            iDaysToEstClose = Convert.ToInt32(this.txtDaysToEst.Text);
        }
        int iDaysAfterCreation = 0;
        if (this.txtDaysAfterCreation.Text != string.Empty)
        {
            iDaysAfterCreation = Convert.ToInt32(this.txtDaysAfterCreation.Text);
        }

        //int iDaysDueAfterPrevStage = 0;
        //if (this.txtDaysDueAfterPrevStage.Text != string.Empty)
        //{
        //    iDaysDueAfterPrevStage = Convert.ToInt32(this.txtDaysDueAfterPrevStage.Text.Trim());
        //}


        int iPrerequisiteID = Convert.ToInt32(this.ddlPrerequisite.SelectedItem.Value);

        int iDaysDueAfterPre = 0;
        if (this.txtDaysDueAfter.Text != string.Empty)
        {
            iDaysDueAfterPre = Convert.ToInt32(this.txtDaysDueAfter.Text);
        }

        int iCompletionEmailID = Convert.ToInt32(this.ddlCompletionEmail.SelectedItem.Value);
        int iWarningEmailID = Convert.ToInt32(this.ddlWarningEmail.SelectedItem.Value);
        int iOverdueEmailID = Convert.ToInt32(this.ddlOverdueEmail.SelectedItem.Value);

        bool bExternalViewing = chbExternalViewing.Checked;

        #endregion

        #region 检查任务名称重复
        if (string.IsNullOrEmpty(sTaskName) || sTaskName.Trim() == string.Empty)
        {
            PageCommon.AlertMsg(this, "The task name cannot be blank.");
            return;
        }
        bool bIsExist = this.LoanTaskManager.IsLoanTaskExists_Insert(this.iLoanID, sTaskName);
        if (bIsExist == true)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Duplicate", "$('#divContainer').hide();alert('The task name is already taken.');$('#divContainer').show();", true);
            return;
        }

        #endregion
        #region Invoke Workflow Manager checking and task API
 
        bool StageCompleted = LPWeb.BLL.WorkflowManager.StageCompleted(iStageID);

        // get next seq num
        int iNextSeq = LoanTaskManager.GetNextSequenceNum(this.iLoanID);

        // add task
        LPWeb.Model.LoanTasks taskModel = new LPWeb.Model.LoanTasks();
        taskModel.LoanTaskId = 0;
        taskModel.FileId = iLoanID;
        taskModel.Name = sTaskName.Trim();
        taskModel.LoanStageId = iStageID;
        taskModel.OldLoanStageId = 0;

        taskModel.Owner = iOwnerID;
        taskModel.ModifiedBy = CurrUser.iUserID;
        taskModel.LastModified = DateTime.Now;
        taskModel.FileId = iLoanID;
        
        if (string.IsNullOrEmpty(sDueDate))
            taskModel.Due = DateTime.MinValue;
        else
            taskModel.Due = DateTime.Parse(sDueDate);
        taskModel.DaysDueFromEstClose = (short)iDaysToEstClose;
        taskModel.DaysFromCreation = (short)iDaysAfterCreation;

       


        taskModel.PrerequisiteTaskId = iPrerequisiteID;
        taskModel.DaysDueAfterPrerequisite = (short)iDaysDueAfterPre;

        taskModel.CompletionEmailId = iCompletionEmailID;
        taskModel.WarningEmailId = iWarningEmailID;
        taskModel.OverdueEmailId = iOverdueEmailID;
        taskModel.SequenceNumber = (short)iNextSeq;
        taskModel.ExternalViewing = bExternalViewing;

        #endregion

        // update
        //bool bIsSuccess1 = this.LoanTaskManager.UpdateLoanTask(this.iTaskID, sTaskName, sDueDate, iCurrentUserID, iOwnerID, iStageID, iPrerequisiteID, iDaysToEstClose, iDaysDueAfterPre, iWarningEmailID, iOverdueEmailID, iCompletionEmailID, iOldStageID);
        // we need to invoke Workflow Manager UpdateTask in order to set up everything correctly!
        //bool bIsSuccess1 = LPWeb.BLL.WorkflowManager.UpdateTask(this.iTaskID, sTaskName, sDueDate, iCurrentUserID, iOwnerID, iStageID, iPrerequisiteID, iDaysToEstClose, iDaysDueAfterPre, iWarningEmailID, iOverdueEmailID, iCompletionEmailID, iOldStageID);
        int iLoanTaskId = LPWeb.BLL.WorkflowManager.AddTask(taskModel, this.txtDaysToEst.Text.Trim(), this.txtDaysAfterCreation.Text.Trim(), this.txtDaysDueAfter.Text.Trim(), txtDaysDueAfterPrevStage.Text.Trim());
        
        if (iLoanTaskId > 0)
        {
            SaveCompletetionEmails(iLoanTaskId);

            //gdc CR40
            try
            {
                LPWeb.BLL.Company_Alerts companyAlertBLL = new Company_Alerts();

                LPWeb.Model.Company_Alerts modelAlert = companyAlertBLL.GetModel();
                if (modelAlert != null && modelAlert.SendEmailCustomTasks == true
                    && modelAlert.CustomTaskEmailTemplId > 0)
                {
                    SaveOKAndSendEmail(modelAlert, iLoanTaskId);
                        
                }
            }
            catch
            {

            }
            //gdc CR40 END

            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Saved successfully.');window.parent.RefreshPage();", true);
        }
        else
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed", "$('#divContainer').hide();alert('Failed to save the record.');window.parent.RefreshPage();", true);
        }
        //LoanTaskManager.InsertLoanTask(this.iLoanID, sTaskName, sDueDate, iOwnerID, iStageID, iPrerequisiteID, iNextSeq, iDaysToEstClose, iDaysDueAfterPre, iWarningEmailID, iOverdueEmailID, iCompletionEmailID);

        #region update point file stage
        if (StageCompleted)
        {
            string sError = LoanTaskCommon.UpdatePointFileStage(this.iLoanID, this.CurrUser.iUserID, iStageID);

            // if failed
            if (sError != string.Empty)
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed0", "$('#divContainer').hide();alert('Added task successfully but failed to update stage date in Point.');$('#divContainer').show();", true);
                return;
            }
        }

        #endregion

        // success
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Added task successfully.');window.parent.RefreshPage();", true);
    }

    /// <summary>
    /// 保存任务完成邮件模板关系
    /// </summary>
    /// <param name="iLoanTaskId"></param>
    public void SaveCompletetionEmails(int iLoanTaskId)
    {
        var tmp = hdnCompletionEmail.Value;
        if (string.IsNullOrEmpty(tmp))
        {
            return;
        }
        var tmpList = tmp.Split('|').ToList();

        LPWeb.BLL.LoanTask_CompletionEmails blltaskMail = new LoanTask_CompletionEmails();
        var tmpidList = new List<int> { };
        foreach (var item in tmpList)
        {
            var list = item.Split(',').ToList();
            if (list.Count == 3 && !string.IsNullOrEmpty(list[1]) && !string.IsNullOrEmpty(list[2])
                && Convert.ToInt32(list[1]) != 0)
            {
                int templEmailId =Convert.ToInt32(list[1]);
                if (tmpidList.Contains(templEmailId))
                {
                    continue;
                }
                LPWeb.Model.LoanTask_CompletionEmails modMail = new LPWeb.Model.LoanTask_CompletionEmails();


                modMail.LoanTaskid = iLoanTaskId;
                modMail.TemplEmailId = Convert.ToInt32(list[1]);
                modMail.Enabled = list[2] == "1" ? true : false;

                blltaskMail.Add(modMail);
                tmpidList.Add(templEmailId);
            }
        }
    }



    protected void gridCompletetionEmails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }


    /// <summary>
    /// 完成 自定义task后 发送邮件 //gdc CR40
    /// </summary>
    /// <param name="modelcmpAlerts"></param>
    /// <param name="taskID"></param>
    public void SaveOKAndSendEmail(LPWeb.Model.Company_Alerts modelcmpAlerts, int taskID)
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
        DataTable RecipientList = EmailTemplateManager.GetRecipientList(modelcmpAlerts.CustomTaskEmailTemplId);

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
                    string sSql_LoanTasks = "select Owner from LoanTasks where LoanTaskId=" + taskID;
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
                req.EmailTemplId = modelcmpAlerts.CustomTaskEmailTemplId;
                req.FileId = this.iLoanID;
                req.UserId = CurrentUser.iUserID;
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
            //PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseDialog_SendCompletionEmail();");
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Failed to send email, error: {0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            //PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseDialog_SendCompletionEmail();");
        }

        #endregion
    }
}
