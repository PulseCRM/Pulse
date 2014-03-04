using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using System.Linq;

public partial class LoanDetails_LoanTaskEdit : BasePage
{
    int iLoanID = 0;
    int iTaskID = 0;
    DataTable LoanTaskInfo;
    DataTable EmailTemplates;
    LoanTasks LoanTaskManager = new LoanTasks();
    Loans LoanManager = new Loans();

    protected void Page_Load(object sender, EventArgs e)
    {
        string sErrorJs = "window.parent.CloseDialog_EditTask();";

        #region 检查必要参数

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

        // server now
        this.hdnNow.Value = DateTime.Now.ToString("MM/dd/yyyy");

        // task icon
        this.imgTaskIcon.ImageUrl = "../images/task/" + WorkflowManager.GetTaskIcon(this.iTaskID);

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

            // 先绑定Stage
            this.ddlStage.SelectedValue = this.LoanTaskInfo.Rows[0]["LoanStageId"].ToString();

            string sSelectedStageID = string.Empty;
            if (this.Request.QueryString["Stage"] == null)
            {
                sSelectedStageID = this.ddlStage.SelectedItem.Value;
            }
            else
            {
                sSelectedStageID = this.Request.QueryString["Stage"].ToString();
            }

            DataTable PrerequisiteList = this.LoanTaskManager.GetPrerequisiteList(" and FileID=" + this.iLoanID + " and LoanStageId = " + sSelectedStageID + " and PrerequisiteTaskId is null and LoanTaskId !=" + iTaskID);
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
            EmailTemplates = EmailTempManager.GetEmailTemplate(" and Enabled = 1");

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

            #region 绑定对应模板列表

            LPWeb.BLL.LoanTask_CompletionEmails bllTaskMail = new LoanTask_CompletionEmails();

            gridCompletetionEmails.DataSource = bllTaskMail.GetList("LoanTaskid=" + iTaskID);
            gridCompletetionEmails.DataBind();
            
            #endregion

            #region Bind Data

            this.txtTaskName.Text = this.LoanTaskInfo.Rows[0]["Name"].ToString();


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

            #region ExternalViewing
            
            if (this.LoanTaskInfo.Rows[0]["ExternalViewing"] != null && this.LoanTaskInfo.Rows[0]["ExternalViewing"].ToString() != "")
            {
                bool ExternalViewing = Convert.ToBoolean(this.LoanTaskInfo.Rows[0]["ExternalViewing"]);

                this.chbExternalViewing.Checked = ExternalViewing;
            }

            #endregion


            #region Due Date

            string sDueDate = this.LoanTaskInfo.Rows[0]["Due"].ToString();
            if (sDueDate != string.Empty)
            {
                this.txtDueDate.Text = Convert.ToDateTime(this.LoanTaskInfo.Rows[0]["Due"]).ToString("MM/dd/yyyy");
            }

            #endregion

            //#region Completed Date

            string sCompletedDate = this.LoanTaskInfo.Rows[0]["Completed"].ToString();
            if (sCompletedDate != string.Empty)
            {
                this.hdnCompleted.Value = Convert.ToDateTime(this.LoanTaskInfo.Rows[0]["Completed"]).ToString("MM/dd/yyyy");
            }

            //#endregion

            this.txtDaysToEst.Text = this.LoanTaskInfo.Rows[0]["DaysDueFromEstClose"] == DBNull.Value ? string.Empty : this.LoanTaskInfo.Rows[0]["DaysDueFromEstClose"].ToString();
            this.txtDaysAfterCreation.Text = this.LoanTaskInfo.Rows[0]["DaysFromCreation"] == DBNull.Value ? string.Empty : this.LoanTaskInfo.Rows[0]["DaysFromCreation"].ToString();

            this.txtDaysDueAfterPrevStage.Text = this.LoanTaskInfo.Rows[0]["DaysDueAfterPrevStage"] == DBNull.Value ? string.Empty : this.LoanTaskInfo.Rows[0]["DaysDueAfterPrevStage"].ToString();


            //Get task template Calculation Method
            if (this.LoanTaskInfo.Rows[0]["TemplTaskId"].ToString() != "" && this.LoanTaskInfo.Rows[0]["TemplTaskId"].ToString() != "0")
            {
                LPWeb.BLL.Template_Wfl_Tasks taskTempMgr = new Template_Wfl_Tasks();
                LPWeb.Model.Template_Wfl_Tasks taskTempModel = taskTempMgr.GetModel(int.Parse(this.LoanTaskInfo.Rows[0]["TemplTaskId"].ToString()));
                int iCalculationMethod = 0;
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

                if (iCalculationMethod == 1 && this.txtDaysAfterCreation.Text == "")
                {
                    this.txtDaysToEst.Enabled = true;
                    this.txtDaysAfterCreation.Enabled = false;
                }
                else if (iCalculationMethod == 2 && this.txtDaysToEst.Text == "")
                {
                    this.txtDaysToEst.Enabled = false;
                    this.txtDaysAfterCreation.Enabled = true;
                }
            }
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

            #region Completion Email

            string sCompletionEmailID = this.LoanTaskInfo.Rows[0]["CompletionEmailId"].ToString();
            if (sCompletionEmailID == string.Empty)
            {
                this.ddlCompletionEmail.SelectedIndex = 0;
            }
            else
            {
                this.ddlCompletionEmail.SelectedValue = sCompletionEmailID;
            }

            #endregion

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
        int iStageID = Convert.ToInt32(this.ddlStage.SelectedItem.Value);

        #region Update Loan Stage of Point File

        DataTable SelectedStageInfo = this.LoanManager.GetLoanStage(" and LoanStageId=" + iStageID);
        string sStageCompletedDate = SelectedStageInfo.Rows[0]["Completed"].ToString();
        if (sStageCompletedDate != string.Empty)
        {
            bool bIsSuccess = true;

            // invoke PointManager.UpdateLoanStage()

            if (bIsSuccess == false)
            {
                PageCommon.AlertMsg(this, "Failed to update status date in Point.");
                return;
            }
        }

        #endregion

        LoginUser CurrentUser = new LoginUser();
        int iCurrentUserID = CurrentUser.iUserID;

        string sTaskName = this.txtTaskName.Text.Trim();

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

        //int iDaysDueAfterPrevStage = 0;
        //if (this.txtDaysDueAfterPrevStage.Text != string.Empty)
        //{
        //    iDaysDueAfterPrevStage = Convert.ToInt32(this.txtDaysDueAfterPrevStage.Text.Trim());
        //}

     
        int iCompletionEmailID = Convert.ToInt32(this.ddlCompletionEmail.SelectedItem.Value);
        int iWarningEmailID = Convert.ToInt32(this.ddlWarningEmail.SelectedItem.Value);
        int iOverdueEmailID = Convert.ToInt32(this.ddlOverdueEmail.SelectedItem.Value);

        // 原Loan Stage ID
        int iOldStageID = Convert.ToInt32(this.LoanTaskInfo.Rows[0]["LoanStageId"]);

        #region 检查任务名称重复
        if (string.IsNullOrEmpty(sTaskName) || sTaskName.Trim() == string.Empty)
        {
            PageCommon.AlertMsg(this, "The task name cannot be blank.");
            return;
        }
        bool bIsExist = this.LoanTaskManager.IsLoanTaskExists_Update(this.iLoanID, this.iTaskID, sTaskName);
        if (bIsExist == true)
        {
            PageCommon.AlertMsg(this, "The task name is already taken.");
            return;
        }
        LPWeb.Model.LoanTasks taskModel = new LPWeb.Model.LoanTasks();
        taskModel.LoanTaskId = iTaskID;
        taskModel.Name = sTaskName.Trim();
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

        taskModel.DaysDueFromEstClose = (short)iDaysToEstClose;
        taskModel.DaysFromCreation = (short)iDaysAfterCreation;
        taskModel.PrerequisiteTaskId = iPrerequisiteID;
        taskModel.DaysDueAfterPrerequisite = (short)iDaysDueAfterPre;

        taskModel.CompletionEmailId = iCompletionEmailID;
        taskModel.WarningEmailId = iWarningEmailID;
        taskModel.OverdueEmailId = iOverdueEmailID;

        taskModel.ExternalViewing = chbExternalViewing.Checked;

        //taskModel.DaysDueAfterPrevStage = (short)iDaysDueAfterPrevStage;

        #endregion

        // update
        //bool bIsSuccess1 = this.LoanTaskManager.UpdateLoanTask(this.iTaskID, sTaskName, sDueDate, iCurrentUserID, iOwnerID, iStageID, iPrerequisiteID, iDaysToEstClose, iDaysDueAfterPre, iWarningEmailID, iOverdueEmailID, iCompletionEmailID, iOldStageID);
        // we need to invoke Workflow Manager UpdateTask in order to set up everything correctly!
        //bool bIsSuccess1 = LPWeb.BLL.WorkflowManager.UpdateTask(this.iTaskID, sTaskName, sDueDate, iCurrentUserID, iOwnerID, iStageID, iPrerequisiteID, iDaysToEstClose, iDaysDueAfterPre, iWarningEmailID, iOverdueEmailID, iCompletionEmailID, iOldStageID);
        bool bIsSuccess1 = LPWeb.BLL.WorkflowManager.UpdateTask(taskModel, this.txtDaysToEst.Text.Trim(), this.txtDaysAfterCreation.Text.Trim(), this.txtDaysDueAfter.Text.Trim(), this.txtDaysDueAfterPrevStage.Text.Trim());
        if (bIsSuccess1 == true)
        {
            SaveCompletetionEmails(iTaskID);
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Saved successfully.');window.parent.RefreshPage();", true);
        }
        else
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed", "$('#divContainer').hide();alert('Failed to save the record.');window.parent.RefreshPage();", true);
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


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LoginUser CurrentUser = new LoginUser();
        bool bIsSuccess = WorkflowManager.DeleteTask(this.iTaskID, CurrentUser.iUserID);

        //this.LoanTaskManager.DeleteLoanTask(this.iTaskID);

        if (bIsSuccess == true)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success1", "$('#divContainer').hide();alert('Deleted the task successfully.');window.parent.RefreshPage();", true);
        }
        else
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed1", "$('#divContainer').hide();alert('Failed to delete the task.');window.parent.RefreshPage();", true);
        }

    }

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


            ddlEmailTemplate.Attributes.Add("cid", "EmailTemplate");
        }
    }
}

