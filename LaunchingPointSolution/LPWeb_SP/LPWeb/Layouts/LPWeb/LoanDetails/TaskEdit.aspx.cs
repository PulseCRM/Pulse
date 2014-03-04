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
using System.IO;

public partial class LoanDetails_TaskEdit : BasePage
{
    int iLoanID = 0;
    int iTaskID = 0;
    DataTable LoanTaskInfo;
    DataTable EmailTemplates;
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

        bIsValid = PageCommon.ValidateQueryString(this, "TaskID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.RegisterJsMsg(this, "Missing required query string.", sErrorJs);
            return;
        }

        this.iTaskID = Convert.ToInt32(this.Request.QueryString["TaskID"]);

        #endregion

        #region 加载Loan Task信息

        this.LoanTaskInfo = this.LoanTaskManager.GetLoanTaskInfo(this.iTaskID);
        if (this.LoanTaskInfo.Rows.Count == 0)
        {
            PageCommon.RegisterJsMsg(this, "Invalid required query string.", sErrorJs);
            return;
        }

        #endregion

        #region 检查是否是Prerequisite（is a father?）

        bool bIsPrerequisite = this.LoanTaskManager.IsPrerequisite(this.iTaskID);
        this.hndIsPrerequisite.Value = bIsPrerequisite.ToString();

        #endregion

        #region 加载LoanInfo信息

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

            #region 加载Prerequisite

            int iCurrentLoanStageId = Convert.ToInt32(this.LoanTaskInfo.Rows[0]["LoanStageId"]);

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
            this.EmailTemplates = EmailTempManager.GetEmailTemplate(" and Enabled = 1");

            DataRow NoneEmailTemplateRow = this.EmailTemplates.NewRow();
            NoneEmailTemplateRow["TemplEmailId"] = 0;
            NoneEmailTemplateRow["Name"] = "None";
            this.EmailTemplates.Rows.InsertAt(NoneEmailTemplateRow, 0);

            this.ddlWarningEmail.DataSource = this.EmailTemplates;
            this.ddlWarningEmail.DataBind();

            this.ddlOverdueEmail.DataSource = this.EmailTemplates;
            this.ddlOverdueEmail.DataBind();

            this.ddlEmailTemplate.DataSource = this.EmailTemplates;
            this.ddlEmailTemplate.DataBind();

            #endregion

            #region completion email list

            LPWeb.BLL.LoanTask_CompletionEmails bllTaskMail = new LoanTask_CompletionEmails();

            gridCompletetionEmails.DataSource = bllTaskMail.GetList("LoanTaskid=" + iTaskID);
            gridCompletetionEmails.DataBind();

            #endregion

            #region Stage

            //Template_Stages stage = new Template_Stages();
            //var dtStage = stage.GetStageTemplateList(" And [Enabled] = 1 order by  SequenceNumber ");

            LoanStages ls = new LoanStages();
            var dtStage = ls.GetLoanStageSetupInfo(iLoanID);

            ddlStage.DataSource = dtStage;
            ddlStage.DataBind();

            
            #endregion

            #region Bind Data

            ddlStage.SelectedValue = this.LoanTaskInfo.Rows[0]["LoanStageId"].ToString(); 

            this.radTaskName.Checked = true;
            this.txtTaskName.Text = this.LoanTaskInfo.Rows[0]["Name"].ToString();
            this.txtDescription.Text = this.LoanTaskInfo.Rows[0]["Desc"].ToString();
            
            #region Owner

            string sOwnerID = this.LoanTaskInfo.Rows[0]["Owner"].ToString();
            if (sOwnerID == string.Empty)
            {
                this.ddlOwner.SelectedIndex = 0;
            }
            else
            {
                this.ddlOwner.SelectedValue = sOwnerID;
            }

            #endregion

            #region Due Date

            string sDueDate = this.LoanTaskInfo.Rows[0]["Due"].ToString();
            if (sDueDate != string.Empty)
            {
                this.txtDueDate.Text = Convert.ToDateTime(this.LoanTaskInfo.Rows[0]["Due"]).ToString("MM/dd/yyyy");
            }

            string sDueTime = this.LoanTaskInfo.Rows[0]["DueTime"].ToString();
            if (sDueTime != string.Empty)
            {
                TimeSpan DueTime = TimeSpan.Parse(this.LoanTaskInfo.Rows[0]["DueTime"].ToString());
                this.txtDueTime.Text = DueTime.ToString().Substring(0, 5);

                ddlDueTime_hour.SelectedValue = DueTime.Hours.ToString();
                ddlDueTime_min.SelectedValue = ((DueTime.Minutes / 5) * 5).ToString();
            }

            #endregion

            //#region Completed Date

            string sCompletedDate = this.LoanTaskInfo.Rows[0]["Completed"].ToString();
            if (sCompletedDate != string.Empty)
            {
                this.chkCompleted.Checked = true;
                this.txtCompletedDate.Text = Convert.ToDateTime(this.LoanTaskInfo.Rows[0]["Completed"]).ToString("MM/dd/yyyy");
                this.hdnCompleted.Value = Convert.ToDateTime(this.LoanTaskInfo.Rows[0]["Completed"]).ToString("MM/dd/yyyy");
            }

            //#endregion

            this.txtDaysToEst.Text = this.LoanTaskInfo.Rows[0]["DaysDueFromEstClose"] == DBNull.Value ? string.Empty : this.LoanTaskInfo.Rows[0]["DaysDueFromEstClose"].ToString();
            this.txtDaysAfterCreation.Text = this.LoanTaskInfo.Rows[0]["DaysFromCreation"] == DBNull.Value ? string.Empty : this.LoanTaskInfo.Rows[0]["DaysFromCreation"].ToString();

            this.txtDaysDueAfterPrevStage.Text = this.LoanTaskInfo.Rows[0]["DaysDueAfterPrevStage"] == DBNull.Value ? string.Empty : this.LoanTaskInfo.Rows[0]["DaysDueAfterPrevStage"].ToString();

            #region Prerequisite Task Id

            string sPrerequisiteTaskId = this.LoanTaskInfo.Rows[0]["PrerequisiteTaskId"] == DBNull.Value ? string.Empty : this.LoanTaskInfo.Rows[0]["PrerequisiteTaskId"].ToString();
            if (sPrerequisiteTaskId == string.Empty)
            {
                this.ddlPrerequisite.SelectedIndex = 0;
            }
            else
            {
                this.ddlPrerequisite.SelectedValue = sPrerequisiteTaskId;
            }

            #endregion

            this.txtDaysDueAfter.Text = LoanTaskInfo.Rows[0]["DaysDueAfterPrerequisite"] == DBNull.Value ? string.Empty : this.LoanTaskInfo.Rows[0]["DaysDueAfterPrerequisite"].ToString();

            

            #region Warning Email

            string sWarningEmailId = this.LoanTaskInfo.Rows[0]["WarningEmailId"].ToString();
            if (sWarningEmailId == string.Empty)
            {
                this.ddlWarningEmail.SelectedIndex = 0;
            }
            else
            {
                this.ddlWarningEmail.SelectedValue = sWarningEmailId;
            }

            #endregion

            #region Overdue Email

            string sOverdueEmailId = this.LoanTaskInfo.Rows[0]["OverdueEmailId"].ToString();
            if (sOverdueEmailId == string.Empty)
            {
                this.ddlOverdueEmail.SelectedIndex = 0;
            }
            else
            {
                this.ddlOverdueEmail.SelectedValue = sOverdueEmailId;
            }

            #endregion

            #endregion
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sErrorJs = string.Empty;
        if (this.Request.QueryString["CloseDialogCodes"] == null)
        {
            sErrorJs = "window.parent.RefreshPage();";
        }
        else
        {
            sErrorJs = this.Request.QueryString["CloseDialogCodes"] + ";";
        }

        int iStageID = Convert.ToInt32(this.LoanTaskInfo.Rows[0]["LoanStageId"]);
        
        int iCurrentUserID = this.CurrUser.iUserID;

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

        int iPrerequisiteID = Convert.ToInt32(this.ddlPrerequisite.SelectedItem.Value);

        //Get task template Calculation Method
        int iCalculationMethod = 0;
        if (this.LoanTaskInfo.Rows[0]["TemplTaskId"].ToString() != "" && this.LoanTaskInfo.Rows[0]["TemplTaskId"].ToString() != "0")
        {
            LPWeb.BLL.Template_Wfl_Tasks taskTempMgr = new Template_Wfl_Tasks();
            LPWeb.Model.Template_Wfl_Tasks taskTempModel = taskTempMgr.GetModel(int.Parse(this.LoanTaskInfo.Rows[0]["TemplTaskId"].ToString()));

            if (taskTempModel != null)
            {
                LPWeb.BLL.Template_Wfl_Stages stageWflMgr = new Template_Wfl_Stages();
                LPWeb.Model.Template_Wfl_Stages stageModel = stageWflMgr.GetModel(taskTempModel.WflStageId);
                if (stageModel != null)
                {
                    LPWeb.BLL.Template_Workflow templateMgr = new Template_Workflow();
                    LPWeb.Model.Template_Workflow templateModel = templateMgr.GetModel(stageModel.WflTemplId);
                    if (templateModel != null)
                    {
                        iCalculationMethod = templateModel.CalculationMethod;
                    }

                    if (stageModel.CalculationMethod != null && stageModel.CalculationMethod.Value != 0)
                    {
                        iCalculationMethod = stageModel.CalculationMethod.Value;
                    }
                }
            }
        }

        int iDaysToEstClose = 0;
        if (this.txtDaysToEst.Text != string.Empty)
        {
            if (iCalculationMethod == 2 && this.txtDaysAfterCreation.Text != string.Empty)
            {
                iDaysToEstClose = 0;
                this.txtDaysToEst.Text = "";
            }
            else
            {
                iDaysToEstClose = Convert.ToInt32(this.txtDaysToEst.Text);
            }
        }

        int iDaysAfterCreation = 0;
        if (this.txtDaysAfterCreation.Text != string.Empty)
        {
            if (iCalculationMethod == 1 && this.txtDaysToEst.Text != string.Empty)
            {
                iDaysAfterCreation = -1;
                this.txtDaysAfterCreation.Text = "";
            }
            else
            {
                iDaysAfterCreation = Convert.ToInt32(this.txtDaysAfterCreation.Text);
            }
        }

        int iDaysDueAfterPre = 0;
        if (this.txtDaysDueAfter.Text != string.Empty)
        {
            iDaysDueAfterPre = Convert.ToInt32(this.txtDaysDueAfter.Text);
        }

        int iWarningEmailID = Convert.ToInt32(this.ddlWarningEmail.SelectedItem.Value);
        int iOverdueEmailID = Convert.ToInt32(this.ddlOverdueEmail.SelectedItem.Value);

        // 原Loan Stage ID
        int iOldStageID = string.IsNullOrEmpty(ddlStage.SelectedValue) ? Convert.ToInt32(this.LoanTaskInfo.Rows[0]["LoanStageId"]) : Convert.ToInt32(ddlStage.SelectedValue);  //CR54 this.iCurrentLoanStageId; //Convert.ToInt32(this.LoanTaskInfo.Rows[0]["LoanStageId"]);

        #region 检查任务名称重复
        if (string.IsNullOrEmpty(sTaskName) || sTaskName.Trim() == string.Empty)
        {
            PageCommon.AlertMsg(this, "The task name cannot be blank.");
            return;
        }

        var loanInfo = this.LoanManager.GetModel(this.iLoanID);
        if (loanInfo == null || loanInfo.Status != "Prospect")  //CR54 当为Prospect时检查重复
        {
            bool bIsExist = this.LoanTaskManager.IsLoanTaskExists_Update(this.iLoanID, this.iTaskID, sTaskName);
            if (bIsExist == true)
            {
                PageCommon.AlertMsg(this, "The task name is already taken.");
                return;
            }
        }

        LPWeb.Model.LoanTasks taskModel = new LPWeb.Model.LoanTasks();
        taskModel.LoanTaskId = iTaskID;
        taskModel.Name = sTaskName.Trim();
        taskModel.Desc = sDesc.Trim();
        taskModel.LoanStageId = iStageID;
        if (iStageID == iOldStageID || iOldStageID <= 0)
            taskModel.OldLoanStageId = 0;
        else
            taskModel.OldLoanStageId = iOldStageID;

        taskModel.Owner = iOwnerID;
        taskModel.ModifiedBy = iCurrentUserID;
        taskModel.LastModified = DateTime.Now;
        taskModel.FileId = iLoanID;

        if (string.IsNullOrEmpty(sDueDate))
            taskModel.Due = DateTime.MinValue;
        else
            taskModel.Due = DateTime.Parse(sDueDate);

        string sDueTime = this.ddlDueTime_hour.Text + ":" + this.ddlDueTime_min.Text;
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

        taskModel.WarningEmailId = iWarningEmailID;
        taskModel.OverdueEmailId = iOverdueEmailID;

        taskModel.ExternalViewing = false;

        if (this.chkCompleted.Checked == true)
        {
            taskModel.Completed = DateTime.Now;
            taskModel.CompletedBy = iCurrentUserID;
        }       

        #endregion

        // update
        bool bIsSuccess1 = LPWeb.BLL.WorkflowManager.UpdateTask(taskModel, this.txtDaysToEst.Text.Trim(), this.txtDaysAfterCreation.Text.Trim(), this.txtDaysDueAfter.Text.Trim(), this.txtDaysDueAfterPrevStage.Text.Trim());
        if (bIsSuccess1 == true)
        {
            SaveCompletetionEmails(iTaskID);

            #region completed

            if (chkCompleted.Checked == true)
            {
                string sResult = this.CompleteTask(this.iTaskID);
                if (sResult != string.Empty)
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed", "$('#divContainer').hide();alert('Failed to complete task.');" + sErrorJs, true);
                }
            }

            #endregion

            // save and create
            if (((Button)sender).Text == "Save and Create Another")
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Redirect", "$('#divContainer').hide();alert('Saved successfully.');window.parent.CloseDialog_EditTask();window.parent.ShowDialog_AddTask();", true);
            }
            else
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Saved successfully.');" + sErrorJs, true);
            }
        }
        else
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed", "$('#divContainer').hide();alert('Failed to save the record.');" + sErrorJs, true);
        }

    }


    /// <summary>
    /// 保存任务完成邮件模板关系
    /// </summary>
    /// <param name="iLoanTaskId"></param>
    public void SaveCompletetionEmails(int iLoanTaskId)
    {
        var tmp = hdnCompletionEmail.Value;


        LPWeb.BLL.LoanTask_CompletionEmails blltaskMail = new LoanTask_CompletionEmails();

        var ds = blltaskMail.GetList("LoanTaskid=" + iTaskID); //数据库中原有数据
        List<int> IdList = ds.Tables[0].AsEnumerable().Select(c => c.Field<int>("TaskCompletionEmailId")).ToList();
        List<int> tmpIdList = ds.Tables[0].AsEnumerable().Select(c => c.Field<int>("TemplEmailId")).ToList();
        var oldDataList = ds.Tables[0].AsEnumerable().Select(c => new { Id = c.Field<int>("TaskCompletionEmailId"), tmpId = c.Field<int>("TemplEmailId") }).ToList();

        if (string.IsNullOrEmpty(tmp) && IdList.Count == 0)
        {
            return;
        }


        if (!string.IsNullOrEmpty(tmp))
        {
            var tmpList = tmp.Split('|').ToList();
            foreach (var item in tmpList)
            {
                var list = item.Split(',').ToList();

                if (list.Count == 3 && !string.IsNullOrEmpty(list[1]) && !string.IsNullOrEmpty(list[2])
                    && Convert.ToInt32(list[1]) != 0)
                {
                    int Id = Convert.ToInt32(list[0]);
                    int templEmailId = Convert.ToInt32(list[1]);
                    bool enabled = list[2] == "1" ? true : false;

                    var oldData = oldDataList.Where(c => c.tmpId == templEmailId).FirstOrDefault();
                    if (tmpIdList.Contains(templEmailId) && oldData != null & oldData.Id != Id)
                    {
                        continue;
                    }
                    else if (Id == 0)
                    {
                        LPWeb.Model.LoanTask_CompletionEmails modMail = new LPWeb.Model.LoanTask_CompletionEmails();

                        modMail.LoanTaskid = iLoanTaskId;
                        modMail.TemplEmailId = templEmailId;
                        modMail.Enabled = enabled;

                        blltaskMail.Add(modMail);
                        tmpIdList.Add(templEmailId);
                    }
                    else if (IdList.Where(c => c == Id).Count() > 0)
                    {
                        LPWeb.Model.LoanTask_CompletionEmails modMail = new LPWeb.Model.LoanTask_CompletionEmails();

                        modMail.TaskCompletionEmailId = Id;
                        modMail.LoanTaskid = iLoanTaskId;
                        modMail.TemplEmailId = templEmailId;
                        modMail.Enabled = enabled;

                        blltaskMail.Update(modMail);

                        IdList.Remove(Id);
                    }

                }
            }
        }

        //删除
        if (IdList.Count > 0)
        {
            var allDelId = string.Empty;
            foreach (var Id in IdList)
            {
                allDelId += "," + Id;
            }
            if (!string.IsNullOrEmpty(allDelId))
            {
                allDelId = allDelId.Remove(0, 1);
            }
            blltaskMail.DeleteList(allDelId);
        }

    }

    protected void gridCompletetionEmails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DropDownList ddlEmailTemplate = (DropDownList)e.Row.FindControl("ddlEmailTemplate");
        if (ddlEmailTemplate != null)
        {
            ddlEmailTemplate.DataSource = this.EmailTemplates;
            ddlEmailTemplate.DataBind();

            var item = (DataRowView)e.Row.DataItem;

            //设置选定模板值

            ddlEmailTemplate.SelectedValue = item["TemplEmailId"].ToString();


            ddlEmailTemplate.Attributes.Add("cid", "EmailTemplate");
        }
    }

    protected void btniCalendarExport_Click(object sender, EventArgs e)
    {
        int iNextTaskID = this.iTaskID;


        string sCurrentPagePath = this.Server.MapPath("~/");
        string sFileName = Guid.NewGuid().ToString();
        string sFilePath = Path.Combine(Path.GetDirectoryName(sCurrentPagePath), sFileName);

        #region #region Call iCalendarToString() API

        LPWeb.BLL.LoanTasks x = new LPWeb.BLL.LoanTasks();
        string s = x.iCalendarToString(this.iLoanID, iNextTaskID, this.CurrUser.iUserID);

        #endregion

        // save file
        if (File.Exists(sFilePath) == false)
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(sFilePath))
            {
                sw.Write(s);
            }
        }

        FileInfo FileInfo1 = new FileInfo(sFilePath);
        this.Response.Clear();
        this.Response.ClearHeaders();
        this.Response.Buffer = false;
        this.Response.ContentType = "application/octet-stream";
        this.Response.AppendHeader("Content-Disposition", "attachment;filename=Lock.ics");
        this.Response.AppendHeader("Content-Length", FileInfo1.Length.ToString());
        this.Response.WriteFile(sFilePath);
        this.Response.Flush();

        // 删除临时文件
        File.Delete(sFilePath);

        this.Response.End();
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