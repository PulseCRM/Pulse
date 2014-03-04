using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using LPWeb.Common;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Settings
{
    public partial class WorkflowTaskTemplateSetup : BasePage
    {
        private int iTaskID = 0;
        private int iStageID = 0;
        private int iTemplateID = 0;
        private LPWeb.DAL.Template_Wfl_Tasks taskTmpMgr = new LPWeb.DAL.Template_Wfl_Tasks();
        private int iCalculationMethod = 0;

        private string FromPage = string.Empty;

        #region functions
        private void SetControlsReadyonly()
        {
            this.tbxTemplateName.ReadOnly = true;

            this.tbxTaskName.ReadOnly = false;
            this.tbxDescription.ReadOnly = false;
            this.tbxDueDaysByDate.ReadOnly = false;
            this.tbxDueDaysByTask.ReadOnly = false;
            this.tbxDaysDueAfterCreationDate.ReadOnly = false;
            this.ddlStage.Enabled = true;
            this.ddlOwner.Enabled = true;
            this.ddlPrerequisiteTask.Enabled = true;
            //this.ddlCompletionEmail.Enabled = true;
            this.ddlOverdueEmail.Enabled = true;
            this.ddlWarningEmail.Enabled = true;
            this.chkEnable.Enabled = true;
            this.chkExternalViewing.Enabled = true;

            if (this.iTaskID == 0)
            {
                this.btnDelete.Enabled = false;
            }
            else
            {
                this.btnDelete.Enabled = true;
            }
            this.btnDelete.Visible = true;
            this.btnSave.Visible = true;

            Template_Workflow templateMgr = new Template_Workflow();
            LPWeb.Model.Template_Workflow tempModel = new Model.Template_Workflow();
            tempModel = templateMgr.GetModel(this.iTemplateID);
            this.hdnCustomTemplate.Value = tempModel.Custom.ToString();
            if (tempModel.Custom == false)
            {
                this.tbxTemplateName.Enabled = false;

                this.tbxTaskName.Enabled  = false;
                this.tbxDescription.Enabled = false;
                this.tbxDueDaysByDate.Enabled = false;
                this.tbxDueDaysByTask.Enabled = false;
                this.tbxDaysDueAfterCreationDate.Enabled = false;
                this.ddlStage.Enabled = false;
                this.ddlOwner.Enabled = false;
                this.ddlPrerequisiteTask.Enabled = false;
                //this.ddlCompletionEmail.Enabled = false;
                this.ddlOverdueEmail.Enabled = false;
                this.ddlWarningEmail.Enabled = false;
                this.chkEnable.Enabled = false;
                this.chkExternalViewing.Enabled = false;

                this.btnSave.Enabled = false;
                this.btnDelete.Enabled = false;
            }

        }

        private void DoInitData()
        {
            try
            {
                //Stage
                LPWeb.DAL.Template_Wfl_Stages stageMgr = new DAL.Template_Wfl_Stages();
                this.ddlStage.DataValueField = "WflStageId";
                this.ddlStage.DataTextField = "Name";
                this.ddlStage.DataSource = stageMgr.GetList(" WflTemplId=" + this.iTemplateID.ToString()).Tables[0];
                this.ddlStage.DataBind();

                //Owner
                LPWeb.DAL.Roles roleMgr = new DAL.Roles();
                this.ddlOwner.DataValueField = "RoleId";
                this.ddlOwner.DataTextField = "Name";
                this.ddlOwner.DataSource = roleMgr.GetList(" Name<>'Executive' AND Name <>'Branch Manager'").Tables[0];
                this.ddlOwner.DataBind();

                //Prerequisite Task
                this.ddlPrerequisiteTask.DataValueField = "TemplTaskId";
                this.ddlPrerequisiteTask.DataTextField = "Name";
                this.ddlPrerequisiteTask.DataSource = this.taskTmpMgr.GetList(" (PrerequisiteTaskId IS NULL OR PrerequisiteTaskId=0) AND WflStageId=" + this.iStageID.ToString() + " AND TemplTaskId <>" + this.iTaskID.ToString()).Tables[0];

                this.ddlPrerequisiteTask.DataBind();
                this.ddlPrerequisiteTask.Items.Insert(0, new ListItem("", "0"));

                //Email template
                LPWeb.DAL.Template_Email emlTmpMgr = new DAL.Template_Email();
                DataSet dsEmailTemp = emlTmpMgr.GetList(" Enabled=1");
                if (dsEmailTemp.Tables.Count > 0)
                {
                    DataRow drEmailTmp = dsEmailTemp.Tables[0].NewRow();
                    drEmailTmp["TemplEmailId"] = 0;
                    drEmailTmp["Enabled"] = true;
                    drEmailTmp["Name"] = "-- select a Email Template --";
                    drEmailTmp["Content"] = "";
                    dsEmailTemp.Tables[0].Rows.InsertAt(drEmailTmp, 0);
                }
                DataView dvEmailTemp = new DataView(dsEmailTemp.Tables[0], "", "Name", DataViewRowState.CurrentRows);
                this.ddlWarningEmail.DataValueField = "TemplEmailId";
                this.ddlWarningEmail.DataTextField = "Name";
                this.ddlWarningEmail.DataSource = dvEmailTemp;
                this.ddlWarningEmail.DataBind();
                //this.ddlCompletionEmail.DataValueField = "TemplEmailId";
                //this.ddlCompletionEmail.DataTextField = "Name";
                //this.ddlCompletionEmail.DataSource = dvEmailTemp;
                //this.ddlCompletionEmail.DataBind();
                this.ddlOverdueEmail.DataValueField = "TemplEmailId";
                this.ddlOverdueEmail.DataTextField = "Name";
                this.ddlOverdueEmail.DataSource = dvEmailTemp;
                this.ddlOverdueEmail.DataBind();

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void LoadTaskData()
        {
            this.tbxTemplateName.Text = "";

            this.tbxTaskName.Text = "";
            this.tbxDescription.Text = "";
            this.ddlOwner.SelectedIndex = -1;
            this.tbxDueDaysByDate.Text = "";
            this.tbxDueDaysByTask.Text = "";
            this.tbxDaysDueAfterCreationDate.Text = "";
            this.ddlPrerequisiteTask.SelectedIndex = -1;
            this.ddlWarningEmail.SelectedIndex = -1;
            //this.ddlCompletionEmail.SelectedIndex = -1;
            this.ddlOverdueEmail.SelectedIndex = -1;
            this.ddlStage.SelectedIndex = -1;

            LPWeb.Model.Template_Wfl_Tasks model = null;
            try
            {
                Template_Workflow templateMgr = new Template_Workflow();
                LPWeb.Model.Template_Workflow tempModel = new Model.Template_Workflow();
                tempModel = templateMgr.GetModel(this.iTemplateID);

                Template_Wfl_Stages wflStageMgr = new Template_Wfl_Stages();
                LPWeb.Model.Template_Wfl_Stages stageModel = new Model.Template_Wfl_Stages();

                this.iCalculationMethod = tempModel.CalculationMethod;
                if (this.iStageID != 0)
                {
                    stageModel = wflStageMgr.GetModel(this.iStageID);
                    if (stageModel.CalculationMethod.ToString() != "" && stageModel.CalculationMethod.ToString() != "0")
                    {
                        iCalculationMethod = int.Parse(stageModel.CalculationMethod.ToString());
                    }
                }

                this.tbxTemplateName.Text = tempModel.Name;


                model = this.taskTmpMgr.GetModel(this.iTaskID);
                if (this.iTaskID == 0 || model == null)
                {
                    if (this.iTemplateID != 0)
                    {
                        if (this.iStageID != 0)
                        {
                            this.ddlStage.SelectedValue = this.iStageID.ToString();
                        }
                    }
                    this.chkEnable.Checked = true;
                    return;
                }
                //Get Template Name by taskid
                this.ddlStage.SelectedValue = model.WflStageId.ToString();
                this.tbxTaskName.Text = model.Name;
                this.tbxDescription.Text = model.Description;
                this.ddlOwner.SelectedValue = model.OwnerRoleId.ToString();
                this.tbxDueDaysByDate.Text = model.DaysDueFromCoe.ToString();
                this.tbxDueDaysByTask.Text = model.DaysDueAfterPrerequisite.ToString();
                this.tbxDaysDueAfterCreationDate.Text = model.DaysFromCreation.ToString();
                this.ddlPrerequisiteTask.SelectedValue = model.PrerequisiteTaskId.ToString();
                if (this.ddlPrerequisiteTask.SelectedIndex > 0)
                {
                    this.ddlStage.Enabled = false;
                }
                if (model.Enabled)
                {
                    this.chkEnable.Checked = true;
                }
                else
                {
                    this.chkEnable.Checked = false;
                }

                if (model.ExternalViewing)
                {
                    this.chkExternalViewing.Checked = true;
                }
                else
                {
                    this.chkExternalViewing.Checked = false;
                }

                //
                if (iCalculationMethod == 1)
                {
                    this.tbxDueDaysByDate.Enabled = true;
                    if (model.DaysFromCreation.ToString() == "" || model.DaysFromCreation.ToString() == "0")
                    {
                        this.tbxDaysDueAfterCreationDate.Enabled = false;
                    }
                    //this.tbxDaysDueAfterCreationDate.Text = "";
                }
                else if (iCalculationMethod == 2)
                {

                    if (model.DaysDueFromCoe.ToString() == "" || model.DaysDueFromCoe.ToString() == "0")
                    {
                        this.tbxDueDaysByDate.Enabled = false;
                    }
                    //this.tbxDueDaysByDate.Text = "";
                    this.tbxDaysDueAfterCreationDate.Enabled = true;
                }

                this.ddlWarningEmail.SelectedValue = model.WarningEmailId.ToString();
                //this.ddlCompletionEmail.SelectedValue = model.CompletionEmailId.ToString();
                this.ddlOverdueEmail.SelectedValue = model.OverdueEmailId.ToString();

                //Set prerequisitetask status base on taskid
                DataTable dtTask = this.taskTmpMgr.GetList(" PrerequisiteTaskId=" + this.iTaskID.ToString()).Tables[0];
                if (dtTask.Rows.Count > 0)
                {
                    this.ddlPrerequisiteTask.SelectedIndex = -1;
                    this.tbxDueDaysByTask.Text = "";
                    this.tbxDueDaysByTask.ReadOnly = true;
                    this.tbxDaysDueAfterCreationDate.Text = "";
                    this.tbxDaysDueAfterCreationDate.ReadOnly = true;
                    this.ddlPrerequisiteTask.Enabled = false;
                    this.ddlStage.Enabled = false;
                    this.hdnIsDependTask.Value = "true";
                }
                //Check Referenced
                LoanTasks loanTaskMgr = new LoanTasks();
                if (loanTaskMgr.GetLoanTaskList(" AND a.TemplTaskId=" + this.iTaskID.ToString()).Rows.Count > 0)
                {
                    this.hdnIsReferenced.Value = "true";
                }
                this.hdnTaskID.Value = this.iTaskID.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlStage_SelectedIndexChanged(object sender, EventArgs e)
        { 
            string sStageID = this.ddlStage.SelectedValue;
            this.iStageID = Convert.ToInt32(sStageID);
            
            this.ddlPrerequisiteTask.DataValueField = "TemplTaskId";
            this.ddlPrerequisiteTask.DataTextField = "Name";
            this.ddlPrerequisiteTask.DataSource = this.taskTmpMgr.GetList(" (PrerequisiteTaskId IS NULL OR PrerequisiteTaskId=0) AND WflStageId=" + this.iStageID.ToString() + " AND TemplTaskId <>" + this.iTaskID.ToString()).Tables[0];

            this.ddlPrerequisiteTask.DataBind();
            this.ddlPrerequisiteTask.Items.Insert(0, new ListItem("", "0"));
            //BindTemplatesGrid();
        }

        private LPWeb.Model.Template_Wfl_Tasks SetTaskModel()
        {
            Template_Workflow templateMgr = new Template_Workflow();
            LPWeb.Model.Template_Workflow tempModel = new Model.Template_Workflow();
            tempModel = templateMgr.GetModel(this.iTemplateID);

            Template_Wfl_Stages wflStageMgr = new Template_Wfl_Stages();
            LPWeb.Model.Template_Wfl_Stages stageModel = new Model.Template_Wfl_Stages();

            this.iCalculationMethod = tempModel.CalculationMethod;
            if (this.iStageID != 0)
            {
                stageModel = wflStageMgr.GetModel(this.iStageID);
                if (stageModel.CalculationMethod.ToString() != "" && stageModel.CalculationMethod.ToString() != "0")
                {
                    iCalculationMethod = int.Parse(stageModel.CalculationMethod.ToString());
                }
            }

            LPWeb.Model.Template_Wfl_Tasks reModel = new Model.Template_Wfl_Tasks();
            reModel.TemplTaskId = 0;
            reModel.WflStageId = 0;
            reModel.Type = 1;
            if (this.iTaskID != 0)
            {
                reModel = this.taskTmpMgr.GetModel(this.iTaskID);
                //reModel.TemplTaskId = this.iTaskID;
            }
            else
            {
                reModel.Enabled = true;
            }
            reModel.TemplTaskId = Convert.ToInt32(this.hdnTaskID.Value);
            reModel.Name = this.tbxTaskName.Text.Trim();
            reModel.Description = this.tbxDescription.Text.Trim();
            if (chkEnable.Checked)
            {
                reModel.Enabled = true;
            }
            else
            {
                reModel.Enabled = false;
            }

            if (chkExternalViewing.Checked)
            {
                reModel.ExternalViewing = true;
            }
            else
            {
                reModel.ExternalViewing = false;
            }

            if (this.ddlStage.SelectedIndex >= 0)
            {
                reModel.WflStageId = Convert.ToInt32(this.ddlStage.SelectedValue);
            }
            else
            {
                reModel.WflStageId = 0;
            }
            if (this.ddlOwner.SelectedIndex >= 0)
            {
                reModel.OwnerRoleId = Convert.ToInt32(this.ddlOwner.SelectedValue);
            }
            else
            {
                reModel.OwnerRoleId = 0;
            }
            int iDays = 0;
            if (this.ddlPrerequisiteTask.SelectedIndex > 0)
            {
                reModel.DaysDueFromCoe = null;
                reModel.DaysFromCreation = null;
                reModel.PrerequisiteTaskId = Convert.ToInt32(this.ddlPrerequisiteTask.SelectedValue);
                if (this.tbxDueDaysByTask.Text.Trim() != "" && Int32.TryParse(this.tbxDueDaysByTask.Text, out iDays))
                {
                    reModel.DaysDueAfterPrerequisite = iDays;
                }
                else
                {
                    reModel.DaysDueAfterPrerequisite = 0;
                }
            }
            else
            {
                reModel.PrerequisiteTaskId = 0;
                reModel.DaysDueAfterPrerequisite = null;
                if (this.iCalculationMethod == 1)
                {
                    if (this.tbxDueDaysByDate.Text.Trim() != "" && Int32.TryParse(this.tbxDueDaysByDate.Text, out iDays))
                    {
                        reModel.DaysDueFromCoe = iDays;
                        reModel.DaysFromCreation = null;
                    }
                    else
                    {
                        reModel.DaysDueFromCoe = null;
                    }

                    if (this.tbxDaysDueAfterCreationDate.Text.Trim() != "" && Int32.TryParse(this.tbxDaysDueAfterCreationDate.Text, out iDays) && reModel.DaysDueFromCoe == null)
                    {
                        reModel.DaysFromCreation = iDays;
                    }
                    else
                    {
                        reModel.DaysFromCreation = null;
                    }
                }
                if (this.iCalculationMethod == 2)
                {

                    if (this.tbxDaysDueAfterCreationDate.Text.Trim() != "" && Int32.TryParse(this.tbxDaysDueAfterCreationDate.Text, out iDays))
                    {
                        reModel.DaysFromCreation = iDays;
                        reModel.DaysDueFromCoe = null;
                    }
                    else
                    {
                        reModel.DaysFromCreation = null;
                    }
                    if (this.tbxDueDaysByDate.Text.Trim() != "" && Int32.TryParse(this.tbxDueDaysByDate.Text, out iDays) && reModel.DaysFromCreation == null)
                    {
                        reModel.DaysDueFromCoe = iDays;
                    }
                    else
                    {
                        reModel.DaysDueFromCoe = null;
                    }
                }
            }
            //if (this.ddlCompletionEmail.SelectedIndex >= 0)
            //{
            //    reModel.CompletionEmailId = Convert.ToInt32(this.ddlCompletionEmail.SelectedValue);
            //}
            //else
            //{
            //    reModel.CompletionEmailId = null;
            //}
            if (this.ddlOverdueEmail.SelectedIndex >= 0)
            {
                reModel.OverdueEmailId = Convert.ToInt32(this.ddlOverdueEmail.SelectedValue);
            }
            else
            {
                reModel.OverdueEmailId = null;
            }
            if (this.ddlWarningEmail.SelectedIndex >= 0)
            {
                reModel.WarningEmailId = Convert.ToInt32(this.ddlWarningEmail.SelectedValue);
            }
            else
            {
                reModel.WarningEmailId = null;
            }

            return reModel;
        }

        /// <summary>
        /// add or update task data
        /// </summary>
        /// <param name="taskModel"></param>
        private void SaveWflTask(LPWeb.Model.Template_Wfl_Tasks taskModel)
        {
            try
            {
                if (taskModel.TemplTaskId == 0)
                {
                    this.iTaskID = this.taskTmpMgr.Add(taskModel);
                    this.hdnTaskID.Value = this.iTaskID.ToString();
                }
                else
                {
                    this.taskTmpMgr.Update(taskModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Check input
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
            if (this.hdnTaskID.Value == "")
            {
                this.hdnTaskID.Value = "0";
            }
            if (tbxTaskName.Text.Trim().Length < 1)
            {
                PageCommon.AlertMsg(this, "Please enter the task name.");
                return false;
            }
            else
            {
                try
                {
                    DataSet ds = taskTmpMgr.GetList(" [name] = '" + tbxTaskName.Text.Trim().Replace("'","''") + "' AND [TemplTaskId]<>" + this.hdnTaskID.Value + " AND [WflStageId] IN(SELECT WflStageId FROM Template_Wfl_Stages WHERE WflTemplId=" + this.iTemplateID + ")");
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    { }
                    else
                    {
                        PageCommon.AlertMsg(this, "The task template name already exists.");
                        return false;
                    }
                }
                catch { }
            }

            if (ddlStage.Text.Trim().Length < 1)
            {
                PageCommon.AlertMsg(this, "Please select a stage");
                return false;
            }
            return true;
        }
        #endregion

        #region Event
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //权限验证
                var loginUser = new LoginUser();
                if (loginUser.userRole.WorkflowTempl.ToString() == "")
                {
                    Response.Redirect("../Unauthorize.aspx");
                    return;
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            string sErrorMsg = "Failed to load current page: invalid task template ID.";
            string sReturnPage = "WorkflowTaskTemplateSetup.aspx";

            if (this.Request.QueryString["FromPage"] != null) // no task id
            {
                FromPage = this.Request.QueryString["FromPage"];

            }
            if (this.Request.QueryString["taskid"] != null) // no task id
            {
                string sTaskID = this.Request.QueryString["taskid"].ToString();
                if (PageCommon.IsID(sTaskID) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }

                this.iTaskID = Convert.ToInt32(sTaskID);

            }
            if (this.Request.QueryString["templateid"] != null) // no task id
            {
                string sTempID = this.Request.QueryString["templateid"].ToString();
                if (PageCommon.IsID(sTempID) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }

                this.iTemplateID = Convert.ToInt32(sTempID);

            }
            if (this.Request.QueryString["stageid"] != null) // no task id
            {
                string sStageID = this.Request.QueryString["stageid"].ToString();
                if (PageCommon.IsID(sStageID) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }

                this.iStageID = Convert.ToInt32(sStageID);

            }
            if (!IsPostBack)
            {
                try
                {
                    DoInitData();
                    SetControlsReadyonly();
                    LoadTaskData();
                }
                catch (Exception ex)
                {
                    LPLog.LogMessage(LogType.Logdebug, ex.ToString());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //CheckInput
                /*同一Template下的Task Name不可重复
                /Name, Stage必输
                */
                if (!CheckInput())
                {
                    return;
                }
                this.SaveWflTask(this.SetTaskModel());                
                if (FromPage.Length > 0)
                    Response.Write("<script>window.parent.location.href=window.parent.location.href </script>");
                Response.Write("<script>window.parent.ClosePopupTask(); </script>");
            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //check loan task
                /*Delete: delete this Workflow Task. If this Workflow Task has been referenced by LoanTasks table, it will display the following message:
The Workflow Task has been referenced by loan tasks. Deleting this Workflow Task will remove the references and is not reversible. Are you sure you want to continue?
[Yes] [No]
                */

                this.taskTmpMgr.Delete(this.iTaskID);

                //Close windows and refresh parent windows                
                if (FromPage.Length > 0)
                    Response.Write("<script>window.parent.location.href=window.parent.location.href </script>");
                Response.Write("<script>window.parent.ClosePopupTask(); </script>");

            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                this.iTaskID = 0;
                this.SaveWflTask(this.SetTaskModel());
                if (FromPage.Length > 0)
                    Response.Write("<script>window.parent.location.href=window.parent.location.href </script>");
                Response.Write("<script>window.parent.ClosePopupTask(); </script>");
            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
            }
        }
        #endregion

    }
}
