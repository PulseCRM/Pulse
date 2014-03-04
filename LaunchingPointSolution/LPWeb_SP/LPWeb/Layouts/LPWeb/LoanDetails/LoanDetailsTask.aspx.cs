using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using System.Text;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;

public partial class LoanDetails_LoanDetailsTask : BasePage
{
    int iLoanID = 0;
    LoanTasks LoanTaskManager = new LoanTasks();
    LoginUser CurrentUser;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 权限验证
        
        this.CurrentUser = new LoginUser();
        if (this.CurrentUser.userRole.CustomTask.ToString() == "")
        {
            Response.Redirect("../Unauthorize1.aspx");
            return;
        }
        else
        {
            this.hdnCustomTask.Value = this.CurrentUser.userRole.CustomTask.ToString();
            this.hdnAssignTask.Value = this.CurrentUser.userRole.AssignTask.ToString();
            this.hdnCompleteOtherTask.Value = this.CurrentUser.userRole.MarkOtherTaskCompl == true ? "True" : "False";
        }

        #endregion

        #region 检查必要参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            //this.Response.Write("Missing required query string.");
            try
            {
                this.Response.End();
            }
            catch
            {

            }
        }

        this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

        #endregion

        // login user id
        this.hdnLoginUserID.Value = this.CurrentUser.iUserID.ToString();

        #region 加载Loan

        Loans LoanManager = new Loans();
        DataTable LoanInfo = LoanManager.GetLoanInfo(this.iLoanID);
        if (LoanInfo.Rows.Count == 0)
        {
            this.Response.Write("Invalid query string.");
            try
            {
                this.Response.End();
            }
            catch
            {

            }
        }

        #endregion

        #region Active Loan or Not

        string sLoanStatus = LoanInfo.Rows[0]["Status"].ToString();
        this.hdnLoanStatus.Value = sLoanStatus;
        
        #endregion

        #region Is Post-Close Complated

        string sSqlx = "select * from LoanStages where FileId=" + this.iLoanID + " and StageName='Post-Close'";
        DataTable PostCloseStageInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx);
        if (PostCloseStageInfo.Rows.Count == 0) 
        {
            this.hdnIsPostCloseUncompleted.Value = "false";
        }
        else
        {
            string CompletedDate = PostCloseStageInfo.Rows[0]["Completed"].ToString();
            if (CompletedDate == string.Empty) 
            {
                this.hdnIsPostCloseUncompleted.Value = "true";
            }
            else
            {
                this.hdnIsPostCloseUncompleted.Value = "false";
            }
        }

        #endregion

        #region hidden fields for Regenerate

        #region 获取Loan Workflow Template ID

        LoanWflTempl LoanWflTemplManager = new LoanWflTempl();
        DataTable LoanWflTempInfo = LoanWflTemplManager.GetLoanWorkflowTemplateInfo(this.iLoanID);
        if (LoanWflTempInfo.Rows.Count > 0) 
        {
            this.hdnLoanWflTempID.Value = LoanWflTempInfo.Rows[0]["WflTemplId"].ToString();
        }

        #endregion

        #region 获取Default Workflow Template ID and Name

        Template_Workflow WorkflowTempManager = new Template_Workflow();
        DataTable DefaultWorkflowTempInfo = WorkflowTempManager.GetDefaultWorkflowTemplateInfo(sLoanStatus);
        if (DefaultWorkflowTempInfo.Rows.Count > 0)
        {
            this.hdnDefaultWflTempID.Value = DefaultWorkflowTempInfo.Rows[0]["WflTemplId"].ToString();
            this.hdnDefaultWflTempName.Value = DefaultWorkflowTempInfo.Rows[0]["Name"].ToString();
        }

        #endregion

        #endregion

        #region 加载Filter

        #region 加载Task Owner

        DataTable TaskOwners = this.LoanTaskManager.GetTaskOwnerList(this.iLoanID);

        DataRow AllTaskOwnerRow = TaskOwners.NewRow();
        AllTaskOwnerRow["UserID"] = 0;
        AllTaskOwnerRow["FullName"] = "All";
        TaskOwners.Rows.InsertAt(AllTaskOwnerRow, 0);

        this.ddlTaskOwner.DataSource = TaskOwners;
        this.ddlTaskOwner.DataBind();

        #endregion

        #region 加载Stage

        DataTable LoanStages = LoanManager.GetLoanStages(this.iLoanID);

        DataRow AllStageRow = LoanStages.NewRow();
        AllStageRow["LoanStageID"] = 0;
        AllStageRow["StageName"] = "All";
        LoanStages.Rows.InsertAt(AllStageRow, 0);

        this.ddlStage.DataSource = LoanStages;
        this.ddlStage.DataBind();

        #endregion

        #endregion

        #region 生成Stage & Task树

        #region 加载筛选后的Loan Stage

        #region sWhere_Stage

        string sWhere_Stage = string.Empty;
        if (this.Request.QueryString["StageFilter"] != null)
        {
            string sStageFilter = this.Request.QueryString["StageFilter"].ToString();
            sWhere_Stage += " and LoanStageId = " + sStageFilter;
        }
        else
        {
            sWhere_Stage += " and FileId = " + this.iLoanID;
        }

        #endregion

        DataTable LoanStageNodes = LoanManager.GetLoanStage(sWhere_Stage);

        #endregion

        StringBuilder sbStageTemplate = new StringBuilder();
        foreach (DataRow StageRow in LoanStageNodes.Rows)
        {
            string sStageName = StageRow["StageName"].ToString();

            #region 获取Template_Stage.Alias

            if (StageRow["WflStageId"] != DBNull.Value)
            {
                string sWflStageId = StageRow["WflStageId"].ToString();

                Template_Wfl_Stages Template_Wfl_Stages1 = new Template_Wfl_Stages();
                DataTable Template_Wfl_Stages_info = Template_Wfl_Stages1.GetList(" WflStageId=" + sWflStageId).Tables[0];
                if (Template_Wfl_Stages_info.Rows.Count > 0)
                {
                    string sTemplStageId = Template_Wfl_Stages_info.Rows[0]["TemplStageId"].ToString();

                    if (sTemplStageId != string.Empty)
                    {
                        Template_Stages Template_Stages1 = new Template_Stages();
                        DataTable Template_Stage_Info = Template_Stages1.GetStageTemplateList(" and TemplStageId=" + sTemplStageId);
                        if (Template_Stage_Info.Rows.Count > 0)
                        {
                            string sAlias = Template_Stage_Info.Rows[0]["Alias"].ToString();
                            if (sAlias != string.Empty)
                            {
                                sStageName = sAlias;
                            }
                        }
                    }
                }
            }

            #endregion

            string sLoanStageID = StageRow["LoanStageId"].ToString();
            int iLoanStageID = Convert.ToInt32(StageRow["LoanStageId"]);
            string sCompletedDate = StageRow["Completed"].ToString() == string.Empty ? string.Empty : " (" + Convert.ToDateTime(StageRow["Completed"]).ToString("MM/dd/yyyy") + ")";
            string sStageIconFileName = this.GetStageIconFileName(iLoanStageID);
            string sLi = "<li><img class='StageIcon' src='../images/stage/" + sStageIconFileName + "' /><a onclick='return Stage_onclick(this);' href='LoanDetailsTask.aspx?LoanID=" + this.iLoanID + "&Stage=" + sLoanStageID + "'><span>" + sStageName + sCompletedDate + "</span></a>";
            sbStageTemplate.AppendLine(sLi);

            #region 加载Stage下的Task

            StringBuilder sbChildTaskList = new StringBuilder();

            #region sWhere_Task

            string sWhere_Task = string.Empty;

            // TaskStatus
            sWhere_Task += this.BuildWhere_TaskStatus();

            // TaskOwner
            sWhere_Task += BuildWhere_TaskOwner();

            // Due
            sWhere_Task += BuildWhere_Due();

            #endregion

            if (sLoanStageID != string.Empty)
            {
                DataTable ChildTasks = this.LoanTaskManager.GetLoanTaskList(" and  FileId = " + this.iLoanID + " and LoanStageId = " + sLoanStageID + " and PrerequisiteTaskId is null" + sWhere_Task);
                if (ChildTasks.Rows.Count > 0)
                {
                    sbChildTaskList.AppendLine("<ul>");

                    foreach (DataRow ChildTaskRow in ChildTasks.Rows)
                    {
                        string sTaskID = ChildTaskRow["LoanTaskId"].ToString();
                        string sTaskName = ChildTaskRow["Name"].ToString();
                        string sCmptDate = ChildTaskRow["Completed"].ToString() == string.Empty ? string.Empty : " " + Convert.ToDateTime(ChildTaskRow["Completed"]).ToString("MM/dd/yyyy");
                        string sLoanTaskIconFileName = this.GetLoanTaskIconFileName(Convert.ToInt32(sTaskID));
                        string sChecked = ChildTaskRow["Completed"].ToString() == string.Empty ? string.Empty : "checked";
                        string sChildDisabled = string.Empty;

                        #region 加载Prerequisite Tasks
                        StringBuilder sbPrerequisiteTaskList = new StringBuilder();
                        DataTable PreTasks = this.LoanTaskManager.GetLoanTaskList(" and  FileId = " + this.iLoanID + " and PrerequisiteTaskId = " + sTaskID);
                        if (PreTasks.Rows.Count > 0)
                        {
                            sbPrerequisiteTaskList.AppendLine("<ul>");

                            foreach (DataRow PreTask in PreTasks.Rows)
                            {
                                string sPreTaskID = PreTask["LoanTaskId"].ToString();
                                string sPreTaskName = PreTask["Name"].ToString();
                                string sPreCmptDate = PreTask["Completed"].ToString() == string.Empty ? string.Empty : " " + Convert.ToDateTime(PreTask["Completed"]).ToString("MM/dd/yyyy");
                                string sPreLoanTaskIconFileName = this.GetLoanTaskIconFileName(Convert.ToInt32(sPreTaskID));
                                string sPreChecked = PreTask["Completed"].ToString() == string.Empty ? string.Empty : "checked";

                                // 如果prerequisite task未完成，其子task不允许完成
                                string sPreDisabled = string.Empty;
                                if (ChildTaskRow["Completed"].ToString() == string.Empty)
                                {
                                    sPreDisabled = "disabled title='Please complete prerequisite task at first.'";
                                }
                                else
                                {
                                    // 如果如果prerequisite task完成了，如果其下有任一child task也完成了，那么不允许uncomplete prerequisite task
                                    if (sPreCmptDate != string.Empty)
                                    {
                                        sChildDisabled = "disabled title='You cannot un-complete a task that has completed dependent task(s). Please un-complete the dependent task(s) first.'";
                                    }
                                }

                                string sPreTaskTemplate = "<li><input myTaskID='" + sPreTaskID + "' myStageID='" + sLoanStageID + "' type='checkbox' class='TaskCheckbox' " + sPreChecked + " " + sPreDisabled + " /><img class='TaskIcon' src='../images/task/" + sPreLoanTaskIconFileName + "' /><a onclick='return Task_onclick(this);' href='LoanDetailsTask.aspx?LoanID=" + this.iLoanID + "&Stage=" + sLoanStageID + "&TaskID=" + sPreTaskID + "'><span title='" + sPreTaskName + sPreCmptDate + "'>" + sPreTaskName + sPreCmptDate + "</span></a></li>";
                                sbPrerequisiteTaskList.AppendLine(sPreTaskTemplate);
                            }

                            sbPrerequisiteTaskList.AppendLine("</ul>");
                        }

                        #endregion

                        string sTaskTemplate = "<li><input myTaskID='" + sTaskID + "' myStageID='" + sLoanStageID + "' type='checkbox' class='TaskCheckbox' " + sChecked + " " + sChildDisabled + " />"
                                             + "<img class='TaskIcon' src='../images/task/" + sLoanTaskIconFileName + "' />"
                                             + "<a onclick='return Task_onclick(this);' href='LoanDetailsTask.aspx?LoanID=" + this.iLoanID + "&Stage=" + sLoanStageID + "&TaskID=" + sTaskID + "'><span title='" + sTaskName + sCmptDate + "'>" + sTaskName + sCmptDate + "</span></a>";
                        sbChildTaskList.AppendLine(sTaskTemplate);
                        sbChildTaskList.AppendLine(sbPrerequisiteTaskList.ToString());

                        sbChildTaskList.AppendLine("</li>");
                    }

                    sbChildTaskList.AppendLine("</ul>");
                }
            }

            #endregion

            sbStageTemplate.AppendLine(sbChildTaskList.ToString());


            sbStageTemplate.AppendLine("</li>");
        }

        this.ltrStageTaskNodes.Text = sbStageTemplate.ToString();

        #endregion

        #region 加载Task列表

        // default
        string sWhere = " and a.FileID=" + this.iLoanID;

        // TaskStatus
        sWhere += this.BuildWhere_TaskStatus();

        // TaskOwner
        sWhere += BuildWhere_TaskOwner();

        // StageFilter
        sWhere += BuildWhere_StageFilter();

        // Due
        sWhere += BuildWhere_Due();

        // TaskID
        if (this.Request.QueryString["TaskID"] != null)
        {
            string sTaskID = this.Request.QueryString["TaskID"].ToString();
            sWhere += " and a.LoanTaskId = " + sTaskID;
        }

        DataTable LoanTaskData = this.LoanTaskManager.GetLoanTaskList(sWhere);
        this.gridTaskList.DataSource = LoanTaskData;
        this.gridTaskList.DataBind();

        #endregion

        // Add thead and tbody
        PageCommon.MakeGridViewAccessible(this.gridTaskList);
    }

    /// <summary>
    /// formate due date
    /// neo 2010-11-25
    /// </summary>
    /// <param name="oDueDate"></param>
    /// <returns></returns>
    public string FormatDueDate(object oDueDate)
    {
        if (oDueDate == DBNull.Value)
        {
            return string.Empty;
        }
        else
        {
            return Convert.ToDateTime(Eval("Due")).ToString("MM/dd/yyyy");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iLoanID"></param>
    /// <param name="iLoanStageID"></param>
    /// <param name="sLoanStage"></param>
    /// <returns></returns>
    private string GetStageIconFileName(int iLoanStageID)
    {
        return LPWeb.DAL.WorkflowManager.GetStageIcon(iLoanStageID);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iLoanTaskID"></param>
    /// <param name="sCompletedDate"></param>
    /// <returns></returns>
    public string GetLoanTaskIconFileName(int iLoanTaskID)
    {
        return LPWeb.DAL.WorkflowManager.GetTaskIcon(iLoanTaskID);
    }

    #region Build Where

    private string BuildWhere_TaskStatus()
    {
        string sWhere_TaskStatus = string.Empty;

        // TaskStatus
        if (this.Request.QueryString["TaskStatus"] != null)
        {
            string sTaskStatus = this.Request.QueryString["TaskStatus"].ToString();
            if (sTaskStatus == "Complete")
            {
                sWhere_TaskStatus = " and a.Completed is not null";
            }
            else // Incomplete
            {
                sWhere_TaskStatus = " and a.Completed is null";
            }
        }

        return sWhere_TaskStatus;
    }

    private string BuildWhere_TaskOwner()
    {
        string sWhere_TaskOwner = string.Empty;

        // TaskOwner
        if (this.Request.QueryString["TaskOwner"] != null)
        {
            string sTaskOwner = this.Request.QueryString["TaskOwner"].ToString();
            sWhere_TaskOwner = " and a.Owner = " + sTaskOwner;
        }

        return sWhere_TaskOwner;
    }

    private string BuildWhere_StageFilter()
    {
        string sWhere_StageFilter = string.Empty;

        // StageFilter
        if (this.Request.QueryString["Stage"] != null || this.Request.QueryString["StageFilter"] != null)
        {
            string sStageFilter = string.Empty;
            if (this.Request.QueryString["Stage"] != null)
            {
                sStageFilter = this.Request.QueryString["Stage"].ToString();
            }
            else if (this.Request.QueryString["StageFilter"] != null)
            {
                sStageFilter = this.Request.QueryString["StageFilter"].ToString();
            }

            if (sStageFilter != string.Empty && sStageFilter != "undefined")
            {
                sWhere_StageFilter = " and a.LoanStageId = " + sStageFilter;
            }
        }

        return sWhere_StageFilter;
    }

    private string BuildWhere_Due()
    {
        string sWhere_Due = string.Empty;

        // Due
        if (this.Request.QueryString["Due"] != null)
        {
            string sDue = this.Request.QueryString["Due"].ToString();
            if (sDue == "In30")
            {
                sWhere_Due = " and datediff(day, getdate(), Due) <= 30 and datediff(day, getdate(), Due) >=0";
            }
            else if (sDue == "In14")
            {
                sWhere_Due = " and datediff(day, getdate(), Due) <= 14 and datediff(day, getdate(), Due) >=0";
            }
            else if (sDue == "In7")
            {
                sWhere_Due = " and datediff(day, getdate(), Due) <= 7 and datediff(day, getdate(), Due) >=0";
            }
            else if (sDue == "In1")
            {
                sWhere_Due = " and datediff(day, getdate(), Due) = 1";
            }
            else if (sDue == "In0")
            {
                sWhere_Due = " and datediff(day, getdate(), Due) = 0";
            }
            else if (sDue == "Overdue")
            {
                sWhere_Due = " and datediff(day, getdate(), Due) < 0";
            }
        }

        return sWhere_Due;
    }

    #endregion
}