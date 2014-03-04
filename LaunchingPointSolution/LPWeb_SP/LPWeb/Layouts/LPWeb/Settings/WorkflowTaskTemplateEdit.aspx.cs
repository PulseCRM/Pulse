using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using System.Data.SqlClient;
using System.Text;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;

public partial class Settings_WorkflowTaskTemplateEdit : BasePage
{
    int iTemplateID = 0;
    int iStageID = 0;
    int iTaskID = 0;
    DataTable CmpltEmailTemplates = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sJsCloseDialog = "$('#divContainer').hide();window.parent.CloseGlobalDialog();";

        bool bIsValid = PageCommon.ValidateQueryString(this, "TemplateID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing", "alert('Missing required query string.');" + sJsCloseDialog, true);
            return;
        }
        this.iTemplateID = Convert.ToInt32(this.Request.QueryString["TemplateID"]);


        if (this.Request.QueryString["StageID"] != null)
        {
            bIsValid = PageCommon.ValidateQueryString(this, "StageID", QueryStringType.ID);
            if (bIsValid == false)
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing", "alert('Missing required query string.');" + sJsCloseDialog, true);
                return;
            }
            this.iStageID = Convert.ToInt32(this.Request.QueryString["StageID"]);
        }

        bIsValid = PageCommon.ValidateQueryString(this, "TaskID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing", "alert('Missing required query string.');" + sJsCloseDialog, true);
            return;
        }
        this.iTaskID = Convert.ToInt32(this.Request.QueryString["TaskID"]);

        #endregion

        #region 获取Workflow Template Name

        string sSql = "select * from dbo.Template_Workflow where WflTemplId=" + this.iTemplateID;
        DataTable WorkflowTemplateInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        if (WorkflowTemplateInfo.Rows.Count == 0)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_m1", "alert('Invalid workflow template id.');" + sJsCloseDialog, true);
            return;
        }
        string sWorkflowTemplateName = WorkflowTemplateInfo.Rows[0]["Name"].ToString();
        this.tbxTemplateName.Text = sWorkflowTemplateName;

        #endregion

        #region 加载Workflow Task信息

        string sSql4 = "select * from Template_Wfl_Tasks where TemplTaskId=" + this.iTaskID;
        DataTable TaskInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql4);
        if (TaskInfo.Rows.Count == 0)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_m2", "alert('Invalid workflow task id.');" + sJsCloseDialog, true);
            return;
        }

        #endregion

        //Check Referenced
        LoanTasks loanTaskMgr = new LoanTasks();
        if (loanTaskMgr.GetLoanTaskList(" AND a.TemplTaskId=" + this.iTaskID.ToString()).Rows.Count > 0)
        {
            this.hdnIsReferenced.Value = "true";
        }

        // __doPostBack
        this.GetPostBackEventReference(this.btnClone1);

        if (this.IsPostBack == false)
        {
            #region 获取Workflow Stage List

            string sSql2 = "select * from Template_Wfl_Stages where WflTemplId=" + this.iTemplateID;
            DataTable StageList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql2);

            DataRow NoneStageRow = StageList.NewRow();
            NoneStageRow["WflStageId"] = 0;
            NoneStageRow["Name"] = "-- select an stage --";
            StageList.Rows.InsertAt(NoneStageRow, 0);

            this.ddlStage.DataSource = StageList;
            this.ddlStage.DataBind();

            this.ddlStage.SelectedValue = this.iStageID.ToString();

            #endregion

            #region 获取Default Owner

            string sSql3 = "select * from Roles where Name<>'Executive' AND Name <>'Branch Manager'";
            DataTable RoleList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql3);

            DataView RoleView = new DataView(RoleList);
            RoleView.Sort = "Name";
            this.ddlOwner.DataSource = RoleView;
            this.ddlOwner.DataBind();

            #endregion

            #region 获取Prerequisite

            string sSql6 = "select * from Template_Wfl_Tasks where (PrerequisiteTaskId IS NULL OR PrerequisiteTaskId=0) AND WflStageId=" + this.iStageID;
            DataTable WflTaskList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql6);

            DataRow NoneWflTaskRow = WflTaskList.NewRow();
            NoneWflTaskRow["TemplTaskId"] = 0;
            NoneWflTaskRow["Name"] = "";
            WflTaskList.Rows.InsertAt(NoneWflTaskRow, 0);

            this.ddlPrerequisiteTask.DataSource = WflTaskList;
            this.ddlPrerequisiteTask.DataBind();

            #endregion

            #region 加载email template

            LoanTasks LoanTaskManager = new LoanTasks();
            Template_Email EmailTempManager = new Template_Email();
            DataTable EmailTemplates = EmailTempManager.GetEmailTemplate(" and Enabled = 1");
            this.CmpltEmailTemplates = EmailTemplates.Copy();

            DataRow NoneEmailTemplateRow = EmailTemplates.NewRow();
            NoneEmailTemplateRow["TemplEmailId"] = 0;
            NoneEmailTemplateRow["Name"] = "-- select an Email Template --";
            EmailTemplates.Rows.InsertAt(NoneEmailTemplateRow, 0);

            this.ddlWarningEmail.DataSource = EmailTemplates;
            this.ddlWarningEmail.DataBind();

            this.ddlOverdueEmail.DataSource = EmailTemplates;
            this.ddlOverdueEmail.DataBind();

            this.CmpltEmailTmpltText.DataSource = new DataView(this.CmpltEmailTemplates, "", "Name", DataViewRowState.CurrentRows) ;
            this.CmpltEmailTmpltText.DataBind();

            #endregion

            #region 绑定数据

            this.tbxTaskName.Text = TaskInfo.Rows[0]["Name"].ToString();
            this.chkEnable.Checked = Convert.ToBoolean(TaskInfo.Rows[0]["Enabled"]);
            this.chkExternalViewing.Checked = TaskInfo.Rows[0]["ExternalViewing"] == DBNull.Value ? false : Convert.ToBoolean(TaskInfo.Rows[0]["ExternalViewing"]);

            this.txtSequence.Text = TaskInfo.Rows[0]["SequenceNumber"].ToString();
            this.tbxDescription.Text = TaskInfo.Rows[0]["Description"].ToString();


            this.ddlOwner.SelectedValue = TaskInfo.Rows[0]["OwnerRoleId"].ToString();
            this.tbxDueDaysByDate.Text = TaskInfo.Rows[0]["DaysDueFromCoe"].ToString();
            this.tbxDaysDueAfterCreationDate.Text = TaskInfo.Rows[0]["DaysFromCreation"].ToString();


            this.txtDaysDueAfterPrevStage.Text = TaskInfo.Rows[0]["DaysDueAfterPrevStage"].ToString();

            this.ddlPrerequisiteTask.SelectedValue = TaskInfo.Rows[0]["PrerequisiteTaskId"].ToString() == string.Empty ? "0" : TaskInfo.Rows[0]["PrerequisiteTaskId"].ToString();

            this.tbxDueDaysByTask.Text = TaskInfo.Rows[0]["DaysDueAfterPrerequisite"].ToString();

            this.ddlWarningEmail.SelectedValue = TaskInfo.Rows[0]["WarningEmailId"].ToString() == string.Empty ? "0" : TaskInfo.Rows[0]["WarningEmailId"].ToString();
            this.ddlOverdueEmail.SelectedValue = TaskInfo.Rows[0]["OverdueEmailId"].ToString() == string.Empty ? "0" : TaskInfo.Rows[0]["OverdueEmailId"].ToString();

            #endregion

            #region 加载Completion Email List

            string sSql5 = "select ROW_NUMBER() OVER (ORDER BY CompletionEmailId) AS RowIndex, * from Template_Wfl_CompletionEmails as a inner join Template_Email as b on a.TemplEmailId=b.TemplEmailId where TemplTaskid=" + this.iTaskID;
            DataTable CompletionEmailList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql5);

            this.gridCompletionEmailList.DataSource = CompletionEmailList;
            this.gridCompletionEmailList.DataBind();

            #endregion

            // set counter
            this.hdnCounter.Value = CompletionEmailList.Rows.Count.ToString();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region 获取用户输入

        string sStageID = this.ddlStage.SelectedValue;
        string sTaskName = this.tbxTaskName.Text.Trim();
        bool bEnabled = this.chkEnable.Checked;
        bool bExtView = this.chkExternalViewing.Checked;

        string sSequence = this.txtSequence.Text.Trim();
        Int16 iSequence = 1;
        if (sSequence != string.Empty)
        {
            iSequence = Convert.ToInt16(sSequence);
        }

        string _DaysDueAfterPrevStage = this.txtDaysDueAfterPrevStage.Text.Trim();
        //Int16 iDaysDueAfterPrevStage = 1;
        //if (_DaysDueAfterPrevStage != string.Empty)
        //{
        //    iDaysDueAfterPrevStage = Convert.ToInt16(_DaysDueAfterPrevStage);
        //}

        string sDesc = this.tbxDescription.Text.Trim();
        string sDefaultOwnerID = this.ddlOwner.SelectedValue;
        string sPrereqTaskID = this.ddlPrerequisiteTask.SelectedValue;
        string sDaysDueFrom = this.tbxDueDaysByDate.Text;
        string sDaysDueAfter = this.tbxDaysDueAfterCreationDate.Text;
        string sDaysDueAfterPrereq = this.tbxDueDaysByTask.Text;
        string sWarningEmailID = this.ddlWarningEmail.SelectedValue;
        string sOverdueEmailID = this.ddlOverdueEmail.SelectedValue;

        // completion email list data
        string sCompetionEmailIDs = this.hdnEmailTemplateIDs.Value;
        string sEnabledList = this.hdnEnabledList.Value;

        #endregion

        #region Build Completion Email List

        string sSql0 = "select * from Template_Wfl_CompletionEmails where 1=0";
        DataTable CompletionEmailList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);

        if (sCompetionEmailIDs != string.Empty)
        {
            string[] CompetionEmailIDArray = sCompetionEmailIDs.Split(',');
            string[] EnabledArray = sEnabledList.Split(',');

            for (int i = 0; i < CompetionEmailIDArray.Length; i++)
            {
                string sCompetionEmailID = CompetionEmailIDArray[i];
                string sEnabled = EnabledArray[i];

                #region add rows

                DataRow CompletionEmailRow = CompletionEmailList.NewRow();
                CompletionEmailRow["CompletionEmailId"] = 0;
                CompletionEmailRow["TemplTaskid"] = this.iTaskID;
                CompletionEmailRow["TemplEmailId"] = Convert.ToInt32(sCompetionEmailID);
                CompletionEmailRow["Enabled"] = Convert.ToBoolean(sEnabled);

                CompletionEmailList.Rows.Add(CompletionEmailRow);

                #endregion
            }
        }

        #endregion

        #region build sql command - insert Template_Wfl_Tasks

        string sSql = "UPDATE Template_Wfl_Tasks SET WflStageId = @WflStageId,Name = @Name,Enabled = @Enabled,DaysDueFromCoe = @DaysDueFromCoe,PrerequisiteTaskId = @PrerequisiteTaskId,DaysDueAfterPrerequisite = @DaysDueAfterPrerequisite,OwnerRoleId = @OwnerRoleId,WarningEmailId = @WarningEmailId,OverdueEmailId = @OverdueEmailId,Description = @Description,DaysFromCreation = @DaysFromCreation,ExternalViewing = @ExternalViewing,SequenceNumber=@SequenceNumber,DaysDueAfterPrevStage=@DaysDueAfterPrevStage where TemplTaskId=@TemplTaskId";

        SqlCommand SqlCmd = new SqlCommand(sSql);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@WflStageId", SqlDbType.Int, Convert.ToInt32(sStageID));
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sTaskName);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);
        //LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Type", SqlDbType.SmallInt, 1);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@OwnerRoleId", SqlDbType.Int, sDefaultOwnerID);
        if (sWarningEmailID == "0")
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@WarningEmailId", SqlDbType.Int, DBNull.Value);
        }
        else
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@WarningEmailId", SqlDbType.Int, sWarningEmailID);
        }

        if (sOverdueEmailID == "0")
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@OverdueEmailId", SqlDbType.Int, DBNull.Value);
        }
        else
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@OverdueEmailId", SqlDbType.Int, sOverdueEmailID);
        }

        //LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@CompletionEmailId", SqlDbType.Int, DBNull.Value);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@SequenceNumber", SqlDbType.SmallInt, iSequence);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Description", SqlDbType.NVarChar, sDesc);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@ExternalViewing", SqlDbType.Bit, bExtView);

        if (!string.IsNullOrEmpty(_DaysDueAfterPrevStage))
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueAfterPrevStage", SqlDbType.SmallInt, Convert.ToInt16(_DaysDueAfterPrevStage));
        }
        else
        {

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueAfterPrevStage", SqlDbType.SmallInt, DBNull.Value);
        }

        if (sPrereqTaskID != "0")
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@PrerequisiteTaskId", SqlDbType.Int, Convert.ToInt32(sPrereqTaskID));
            if (sDaysDueAfterPrereq == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueAfterPrerequisite", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueAfterPrerequisite", SqlDbType.SmallInt, Convert.ToInt16(sDaysDueAfterPrereq));
            }
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueFromCoe", SqlDbType.SmallInt, DBNull.Value);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysFromCreation", SqlDbType.SmallInt, DBNull.Value);
        }
        else
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@PrerequisiteTaskId", SqlDbType.Int, DBNull.Value);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueAfterPrerequisite", SqlDbType.SmallInt, DBNull.Value);

            if (sDaysDueFrom == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueFromCoe", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueFromCoe", SqlDbType.SmallInt, Convert.ToInt16(sDaysDueFrom));
            }

            if (sDaysDueAfter == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysFromCreation", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysFromCreation", SqlDbType.SmallInt, Convert.ToInt16(sDaysDueAfter));
            }
        }

        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@TemplTaskId", SqlDbType.Int, this.iTaskID);

        #endregion

        #region build sql command - Template_Wfl_CompletionEmails

        string sSql4 = "delete from Template_Wfl_CompletionEmails where TemplTaskid=" + this.iTaskID;
        SqlCommand SqlCmd4 = new SqlCommand(sSql4);

        string sSql3 = "insert into Template_Wfl_CompletionEmails (TemplTaskid,TemplEmailId,Enabled) values (@TemplTaskid,@TemplEmailId,@Enabled)";
        SqlCommand SqlCmd3 = new SqlCommand(sSql3);

        LPWeb.DAL.DbHelperSQL.AddSqlParameter1(SqlCmd3, "@TemplTaskid", SqlDbType.Int, "TemplTaskid");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter1(SqlCmd3, "@TemplEmailId", SqlDbType.Int, "TemplEmailId");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter1(SqlCmd3, "@Enabled", SqlDbType.Bit, "Enabled");

        #endregion

        #region 批量执行SQL语句

        SqlConnection SqlConn = null;
        SqlTransaction SqlTrans = null;

        try
        {
            SqlConn = LPWeb.DAL.DbHelperSQL.GetOpenConnection();
            SqlTrans = SqlConn.BeginTransaction();

            // update
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd, SqlTrans);

            // delete Template_Wfl_CompletionEmails
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd4, SqlTrans);

            #region update new TemplTaskid to Template_Wfl_CompletionEmails

            if (CompletionEmailList.Rows.Count > 0)
            {
                // update data table
                LPWeb.DAL.DbHelperSQL.UpdateDataTable(CompletionEmailList, SqlCmd3, null, null, SqlTrans);
            }

            #endregion

            SqlTrans.Commit();

        }
        catch (Exception ex)
        {
            SqlTrans.Rollback();
            throw ex;
        }
        finally
        {
            if (SqlConn != null)
            {
                SqlConn.Close();
            }
        }

        #endregion

        // success
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Update workflow task successfully.');window.parent.location.href=window.parent.location.href;", true);
    }

    public string GetOptions_ddlCmpltEmailTmplt(string sSelectedEmailTemplateID)
    {
        StringBuilder sbOptions = new StringBuilder();

        foreach (DataRow EmailTemplateRow in this.CmpltEmailTemplates.Select("", "Name"))
        {
            string sEmailTemplateID = EmailTemplateRow["TemplEmailId"].ToString();
            string sEmailTemplateName = EmailTemplateRow["Name"].ToString();
            if (sEmailTemplateID == sSelectedEmailTemplateID)
            {
                sbOptions.AppendLine("<option value='" + sEmailTemplateID + "' selected>" + sEmailTemplateName + "</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='" + sEmailTemplateID + "'>" + sEmailTemplateName + "</option>");
            }
        }

        return sbOptions.ToString();
    }

    protected void btnClone1_Click(object sender, EventArgs e)
    {
        #region 获取用户输入

        string sStageID = this.ddlStage.SelectedValue;
        string sTaskName = this.txbTaskName1.Text.Trim();
        bool bEnabled = this.chkEnable.Checked;
        bool bExtView = this.chkExternalViewing.Checked;

        string sSequence = this.txtSequence.Text.Trim();
        Int16 iSequence = 1;
        if (sSequence != string.Empty)
        {
            iSequence = Convert.ToInt16(sSequence);
        }

        string sDesc = this.tbxDescription.Text.Trim();
        string sDefaultOwnerID = this.ddlOwner.SelectedValue;
        string sPrereqTaskID = this.ddlPrerequisiteTask.SelectedValue;
        string sDaysDueFrom = this.tbxDueDaysByDate.Text;
        string sDaysDueAfter = this.tbxDaysDueAfterCreationDate.Text;
        string sDaysDueAfterPrereq = this.tbxDueDaysByTask.Text;
        string sWarningEmailID = this.ddlWarningEmail.SelectedValue;
        string sOverdueEmailID = this.ddlOverdueEmail.SelectedValue;

        // completion email list data
        string sCompetionEmailIDs = this.hdnEmailTemplateIDs.Value;
        string sEnabledList = this.hdnEnabledList.Value;

        #endregion

        #region Build Completion Email List

        string sSql0 = "select * from Template_Wfl_CompletionEmails where 1=0";
        DataTable CompletionEmailList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);

        if (sCompetionEmailIDs != string.Empty)
        {
            string[] CompetionEmailIDArray = sCompetionEmailIDs.Split(',');
            string[] EnabledArray = sEnabledList.Split(',');

            for (int i = 0; i < CompetionEmailIDArray.Length; i++)
            {
                string sCompetionEmailID = CompetionEmailIDArray[i];
                string sEnabled = EnabledArray[i];

                #region add rows

                DataRow CompletionEmailRow = CompletionEmailList.NewRow();
                CompletionEmailRow["CompletionEmailId"] = 0;
                CompletionEmailRow["TemplTaskid"] = 0;
                CompletionEmailRow["TemplEmailId"] = Convert.ToInt32(sCompetionEmailID);
                CompletionEmailRow["Enabled"] = Convert.ToBoolean(sEnabled);

                CompletionEmailList.Rows.Add(CompletionEmailRow);

                #endregion
            }
        }

        #endregion

        #region build sql command - insert Template_Wfl_Tasks

        string sSql = "insert into Template_Wfl_Tasks (WflStageId,Name,Enabled,Type,DaysDueFromCoe,PrerequisiteTaskId,DaysDueAfterPrerequisite,OwnerRoleId,WarningEmailId,OverdueEmailId,CompletionEmailId,SequenceNumber,Description,DaysFromCreation,ExternalViewing) values (@WflStageId,@Name,@Enabled,@Type,@DaysDueFromCoe,@PrerequisiteTaskId,@DaysDueAfterPrerequisite,@OwnerRoleId,@WarningEmailId,@OverdueEmailId,@CompletionEmailId,@SequenceNumber,@Description,@DaysFromCreation,@ExternalViewing);"
                    + "select SCOPE_IDENTITY();";
        SqlCommand SqlCmd = new SqlCommand(sSql);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@WflStageId", SqlDbType.Int, Convert.ToInt32(sStageID));
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sTaskName);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Type", SqlDbType.SmallInt, 1);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@OwnerRoleId", SqlDbType.Int, sDefaultOwnerID);
        if (sWarningEmailID == "0")
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@WarningEmailId", SqlDbType.Int, DBNull.Value);
        }
        else
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@WarningEmailId", SqlDbType.Int, sWarningEmailID);
        }

        if (sOverdueEmailID == "0")
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@OverdueEmailId", SqlDbType.Int, DBNull.Value);
        }
        else
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@OverdueEmailId", SqlDbType.Int, sOverdueEmailID);
        }

        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@CompletionEmailId", SqlDbType.Int, DBNull.Value);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@SequenceNumber", SqlDbType.SmallInt, iSequence);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Description", SqlDbType.NVarChar, sDesc);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@ExternalViewing", SqlDbType.Bit, bExtView);

        if (sPrereqTaskID != "0")
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@PrerequisiteTaskId", SqlDbType.Int, Convert.ToInt32(sPrereqTaskID));
            if (sDaysDueAfterPrereq == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueAfterPrerequisite", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueAfterPrerequisite", SqlDbType.SmallInt, Convert.ToInt16(sDaysDueAfterPrereq));
            }
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueFromCoe", SqlDbType.SmallInt, DBNull.Value);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysFromCreation", SqlDbType.SmallInt, DBNull.Value);
        }
        else
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@PrerequisiteTaskId", SqlDbType.Int, DBNull.Value);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueAfterPrerequisite", SqlDbType.SmallInt, DBNull.Value);

            if (sDaysDueFrom == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueFromCoe", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueFromCoe", SqlDbType.SmallInt, Convert.ToInt16(sDaysDueFrom));
            }

            if (sDaysDueAfter == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysFromCreation", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysFromCreation", SqlDbType.SmallInt, Convert.ToInt16(sDaysDueAfter));
            }
        }

        #endregion

        #region build sql command - Template_Wfl_CompletionEmails

        string sSql3 = "insert into Template_Wfl_CompletionEmails (TemplTaskid,TemplEmailId,Enabled) values (@TemplTaskid,@TemplEmailId,@Enabled)";
        SqlCommand SqlCmd3 = new SqlCommand(sSql3);

        LPWeb.DAL.DbHelperSQL.AddSqlParameter1(SqlCmd3, "@TemplTaskid", SqlDbType.Int, "TemplTaskid");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter1(SqlCmd3, "@TemplEmailId", SqlDbType.Int, "TemplEmailId");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter1(SqlCmd3, "@Enabled", SqlDbType.Bit, "Enabled");

        #endregion

        #region 批量执行SQL语句

        SqlConnection SqlConn = null;
        SqlTransaction SqlTrans = null;

        try
        {
            SqlConn = LPWeb.DAL.DbHelperSQL.GetOpenConnection();
            SqlTrans = SqlConn.BeginTransaction();

            int iNewID = Convert.ToInt32(LPWeb.DAL.DbHelperSQL.ExecuteScalar(SqlCmd, SqlTrans));

            #region update new TemplTaskid to Template_Wfl_CompletionEmails

            if (CompletionEmailList.Rows.Count > 0)
            {
                foreach (DataRow CompletionEmailRow in CompletionEmailList.Rows)
                {
                    CompletionEmailRow["TemplTaskid"] = iNewID;
                }

                // update data table
                LPWeb.DAL.DbHelperSQL.UpdateDataTable(CompletionEmailList, SqlCmd3, null, null, SqlTrans);
            }

            #endregion

            SqlTrans.Commit();

        }
        catch (Exception ex)
        {
            SqlTrans.Rollback();
            throw ex;
        }
        finally
        {
            if (SqlConn != null)
            {
                SqlConn.Close();
            }
        }

        #endregion

        // success
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Clone workflow task successfully.');window.parent.location.href=window.parent.location.href;", true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string sSql0 = "delete from Template_Wfl_CompletionEmails where TemplTaskid=" + this.iTaskID;
        SqlCommand SqlCmd0 = new SqlCommand(sSql0);

        string sSql2 = "delete from Template_Wfl_Tasks where TemplTaskid=" + this.iTaskID;
        SqlCommand SqlCmd2 = new SqlCommand(sSql2);

        #region 批量执行SQL语句

        SqlConnection SqlConn = null;
        SqlTransaction SqlTrans = null;

        try
        {
            SqlConn = LPWeb.DAL.DbHelperSQL.GetOpenConnection();
            SqlTrans = SqlConn.BeginTransaction();

            // delete Template_Wfl_CompletionEmails
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd0, SqlTrans);

            // delete Template_Wfl_Tasks
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd2, SqlTrans);

            SqlTrans.Commit();

        }
        catch (Exception ex)
        {
            SqlTrans.Rollback();
            throw ex;
        }
        finally
        {
            if (SqlConn != null)
            {
                SqlConn.Close();
            }
        }

        #endregion

        // success
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Delete workflow task successfully.');window.parent.location.href=window.parent.location.href;", true);
    }
}
