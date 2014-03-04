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

public partial class LoanDetails_TaskCreate : BasePage
{
    int iLoanID = 0;
    int iCurrentLoanStageId = 0;
    LoanTasks LoanTaskManager = new LoanTasks();
    Loans LoanManager = new Loans();

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 检查必要参数

        string sErrorJs = string.Empty;
        if (this.Request.QueryString["CloseDialogCodes"] == null)
        {
            sErrorJs = "window.parent.RefreshPage();";
        }
        else
        {
            sErrorJs = this.Request.QueryString["CloseDialogCodes"] + ";";
        }

        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.RegisterJsMsg(this, "Missing required query string.", sErrorJs);
            return;
        }

        this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

        #endregion

        #region 校验LoanId

        DataTable LoanInfo = this.LoanManager.GetLoanInfo(this.iLoanID);
        if (LoanInfo.Rows.Count == 0)
        {
            PageCommon.RegisterJsMsg(this, "Invalid required query string.", sErrorJs);
            return;
        }

        // 存储Loan.EstCloseDate
        if (LoanInfo.Rows[0]["EstCloseDate"] != DBNull.Value)
        {
            this.hdnEstCloseDate.Value = Convert.ToDateTime(LoanInfo.Rows[0]["EstCloseDate"]).ToString("MM/dd/yyyy");
        }

        this.iCurrentLoanStageId = WorkflowManager.GetCurrentLoanStageId(this.iLoanID);

        bIsValid = PageCommon.ValidateQueryString(this, "Stage", QueryStringType.ID);
        if (bIsValid == true)
        {
            int iout = 0;
            string sStage = this.Request.QueryString["Stage"];
            if (Int32.TryParse(sStage, out iout))
            {
                this.iCurrentLoanStageId = iout;
            }           
        }

        #endregion

        if (this.IsPostBack == false)
        {
            #region 加载Owner

            DataTable OwnerList = this.LoanTaskManager.GetLoanTaskOwers(this.iLoanID);

            DataRow EmptyOwnerRow = OwnerList.NewRow();
            EmptyOwnerRow["UserID"] = 0;
            EmptyOwnerRow["FullName"] = "-- select --";
            OwnerList.Rows.InsertAt(EmptyOwnerRow, 0);

            this.ddlOwner.DataSource = OwnerList;
            this.ddlOwner.DataBind();

            // 绑定Owner
            this.ddlOwner.SelectedValue = this.CurrUser.iUserID.ToString();

            #endregion

            #region 加载ddlTaskList for TaskNaem

            LeadTaskList LeadTaskListMgr = new LeadTaskList();

            string sOrderBy = string.Empty;
            if (this.CurrUser.SortTaskPickList == "S")
            {
                sOrderBy = "SequenceNumber";
            }
            else
            {
                sOrderBy = "TaskName";
            }

            DataTable LeadTaskList1 = LeadTaskListMgr.GetLeadTaskList(" and Enabled=1", sOrderBy);

            DataRow EmptyTaskRow = LeadTaskList1.NewRow();
            EmptyTaskRow["TaskName"] = "-- select --";
            LeadTaskList1.Rows.InsertAt(EmptyTaskRow, 0);

            this.ddlTaskList.DataSource = LeadTaskList1;
            this.ddlTaskList.DataBind();

            #endregion

            // set default value
            this.txtDueDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            //this.txtDueTime.Text = DateTime.Now.AddMinutes(15).ToShortTimeString();
            this.ddlDueTime_hour.SelectedValue = DateTime.Now.Hour.ToString("00");
            this.ddlDueTime_min.SelectedValue = (DateTime.Now.AddMinutes(15).Minute / 5 * 5).ToString();

            #region 加载Prerequisite

            DataTable PrerequisiteList = this.LoanTaskManager.GetPrerequisiteList(" and FileID=" + this.iLoanID + " and LoanStageId = " + iCurrentLoanStageId + " and PrerequisiteTaskId is null");
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

            this.ddlWarningEmail.DataSource = EmailTemplates;
            this.ddlWarningEmail.DataBind();

            this.ddlOverdueEmail.DataSource = EmailTemplates;
            this.ddlOverdueEmail.DataBind();

            this.ddlEmailTemplate.DataSource = EmailTemplates;
            this.ddlEmailTemplate.DataBind();

            #endregion

            #region completion email list

            gridCompletetionEmails.DataSource = null;
            gridCompletetionEmails.DataBind();

            #endregion


            #region Stage

            //Template_Stages stage = new Template_Stages();
            //var dtStage = stage.GetStageTemplateList(" And [Enabled] = 1 order by  SequenceNumber ");

            LoanStages ls = new LoanStages();
            var dtStage = ls.GetLoanStageSetupInfo(iLoanID);

            ddlStage.DataSource = dtStage;
            ddlStage.DataBind();

            ddlStage.SelectedValue = this.iCurrentLoanStageId.ToString();
            #endregion
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
        int Valid_LoanStageId = 0;
        string sErrorJs = string.Empty;
        if (this.Request.QueryString["CloseDialogCodes"] == null)
        {
            sErrorJs = "window.parent.RefreshPage();";
        }
        else
        {
            sErrorJs = this.Request.QueryString["CloseDialogCodes"] + ";";
        }

        #region 获取数据

        DataTable LoanStages = this.LoanManager.GetLoanStages(this.iLoanID);
        if (LoanStages == null || LoanStages.Rows.Count <= 0)
        {
            this.iCurrentLoanStageId = GetTaskStageID(this.iLoanID);
            if ((this.iCurrentLoanStageId == null) ||
                (this.iCurrentLoanStageId <= 0))
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed0", "$('#divContainer').hide();alert('The loan does not have any stage to add the task to.');" + sErrorJs, true);
                return;
            }
        }

        int iStageID = string.IsNullOrEmpty(ddlStage.SelectedValue) ? this.iCurrentLoanStageId : Convert.ToInt32(ddlStage.SelectedValue);  //CR54 this.iCurrentLoanStageId;
        string sTaskName = string.Empty;
        if (radTaskList.Checked == true)
        {
            sTaskName = this.ddlTaskList.SelectedValue;
            if (sTaskName == "-- select --")
            {
                sTaskName = string.Empty;
            }
        }
        else
        {
            sTaskName = this.txtTaskName.Text.Trim();
        }
        string sDesc = this.txtDescription.Text.Trim();

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

        int iCompletionEmailID = 0;
        int iWarningEmailID = Convert.ToInt32(this.ddlWarningEmail.SelectedItem.Value);
        int iOverdueEmailID = Convert.ToInt32(this.ddlOverdueEmail.SelectedItem.Value);

        bool bExternalViewing = false;

        #endregion

        #region 检查任务名称重复
        if (string.IsNullOrEmpty(sTaskName) || sTaskName.Trim() == string.Empty)
        {
            PageCommon.AlertMsg(this, "The task name cannot be blank.");
            return;
        }

        var loanInfo = this.LoanManager.GetModel(this.iLoanID);

        if (loanInfo == null || loanInfo.Status != "Prospect")  //CR54 当为Prospect时检查重复
        {
            bool bIsExist = this.LoanTaskManager.IsLoanTaskExists_Insert(this.iLoanID, sTaskName);
            if (bIsExist == true)
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Duplicate", "$('#divContainer').hide();alert('The task name is already taken.');$('#divContainer').show();", true);
                return;
            }
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
        taskModel.Desc = sDesc.Trim();
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

        string sDueTime = ddlDueTime_hour.Text + ":" + ddlDueTime_min.Text; //this.txtDueTime.Text.Trim();  CR54
        sDueTime = sDueTime.Replace("am", "").Replace("pm", "");

        DateTime DTN = DateTime.Now;
        string sDueTime_Span = null;
        TimeSpan DueTime = new TimeSpan();

        if (sDueTime == string.Empty)
        {
            taskModel.DueTime = null;
        }
        else
        {
            taskModel.DueTime = null;
            if (DateTime.TryParse(sDueTime, out DTN) == true)
            {
                sDueTime_Span = DTN.ToString("HH:mm");
                if (TimeSpan.TryParse(sDueTime_Span, out DueTime) == true)
                {
                    taskModel.DueTime = DueTime;
                }
            }
        }

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

        // create task
        int iLoanTaskId = LPWeb.BLL.WorkflowManager.AddTask(taskModel, this.txtDaysToEst.Text.Trim(), this.txtDaysAfterCreation.Text.Trim(), this.txtDaysDueAfter.Text.Trim(), txtDaysDueAfterPrevStage.Text.Trim());

        if (iLoanTaskId > 0)
        {
            SaveCompletetionEmails(iLoanTaskId);
            string Notes = txtNotes.Text;
            this.CreateLoanNotes(taskModel.FileId, iLoanTaskId, Notes);

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
            
        }
        else
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed", "$('#divContainer').hide();alert('Failed to save the record.');" + sErrorJs, true);
        }
        
        #region update point file stage
        if (StageCompleted)
        {
            string sError = LoanTaskCommon.UpdatePointFileStage(this.iLoanID, this.CurrUser.iUserID, iStageID);

            // if failed
            //if (sError != string.Empty)
            //{
            //    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed0", "$('#divContainer').hide();alert('Added task successfully but failed to update stage date in Point.');$('#divContainer').show();", true);
            //    return;
            //}
        }

        #endregion

        #region completed

        if (chkCompleted.Checked == true)
        {
            string sResult = this.CompleteTask(iLoanTaskId);
            if (sResult != string.Empty)
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed", "$('#divContainer').hide();alert('Failed to complete task.');" + sErrorJs, true);
            }
        }

        #endregion

        // success
        if (((Button)sender).Text == "Save and Create Another")
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Redirect", "$('#divContainer').hide();alert('Added task successfully.');window.location.href=window.location.href;", true);
        }
        else
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Added task successfully.');" + sErrorJs, true);
        }
    }

    private void CreateLoanNotes(int iFileId, int iLoanTaskId, string sComments)
    {
        #region create LoanNotes

        LPWeb.Model.LoanNotes LoanNotesModel = new LPWeb.Model.LoanNotes();

        LoanNotesModel.FileId = iFileId;
        LoanNotesModel.Created = DateTime.Now;
        LoanNotesModel.Sender = this.CurrUser.sFirstName + " " + this.CurrUser.sLastName;
        LoanNotesModel.Note = sComments;
        LoanNotesModel.LoanTaskId = iLoanTaskId;

        LoanNotes LoanNotesMgr = new LoanNotes();
        LoanNotesMgr.Add_LoanTaskId(LoanNotesModel);

        #endregion
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
                int templEmailId = Convert.ToInt32(list[1]);
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

    private string CompleteTask(int iLoanTaskId) 
    {
        #region complete task

        string sErrorMsg = string.Empty;
        int iEmailTemplateId = 0;
        bool bIsSuccess = LPWeb.DAL.WorkflowManager.CompleteTask(iLoanTaskId, this.CurrUser.iUserID, ref iEmailTemplateId);

        if (bIsSuccess == false)
        {
            sErrorMsg = "Failed to invoke WorkflowManager.CompleteTask.";
            return sErrorMsg;
        }

        #endregion

        #region update point file stage

        int iLoanStageID = 0;

        #region get loan task info

        LoanTasks LoanTaskManager = new LoanTasks();
        DataTable LoanTaskInfo = LoanTaskManager.GetLoanTaskInfo(iLoanTaskId);
        if (LoanTaskInfo.Rows.Count == 0)
        {
            sErrorMsg = "Invalid task id.";
            return sErrorMsg;
        }
        string sLoanStageID = LoanTaskInfo.Rows[0]["LoanStageId"].ToString();
        if (sLoanStageID == string.Empty)
        {
            sErrorMsg = "Invalid loan stage id.";
            return sErrorMsg;
        }
        iLoanStageID = Convert.ToInt32(sLoanStageID);

        #endregion
        if (WorkflowManager.StageCompleted(iLoanStageID) == true)
        {
            #region invoke PointManager.UpdateStage()

            //add by  gdc 20111212  Bug #1306 
            LPWeb.BLL.PointFiles pfile = new PointFiles();
            var model = pfile.GetModel(iLoanID);
            if (model != null && !string.IsNullOrEmpty(model.Name.Trim()))
            {
                #region UPdatePointFileStage  WCF
                
                string sError = LoanTaskCommon.UpdatePointFileStage(iLoanID, this.CurrUser.iUserID, iLoanStageID);

                #endregion
            }

            #endregion
        }

        #endregion

        return sErrorMsg;
    }
}