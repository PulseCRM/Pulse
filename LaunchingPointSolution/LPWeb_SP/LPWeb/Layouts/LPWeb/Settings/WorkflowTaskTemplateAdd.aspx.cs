using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.BLL;

public partial class Settings_WorkflowTaskTemplateAdd : BasePage
{
    int iTemplateID = 0;
    int iStageID = 0;

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

            string sSql4 = "select * from Template_Wfl_Tasks where (PrerequisiteTaskId IS NULL OR PrerequisiteTaskId=0) AND WflStageId=" + this.iStageID;
            DataTable WflTaskList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql4);

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
            DataTable CmpltEmailTemplates = EmailTemplates.Copy();

            DataRow NoneEmailTemplateRow = EmailTemplates.NewRow();
            NoneEmailTemplateRow["TemplEmailId"] = 0;
            NoneEmailTemplateRow["Name"] = "-- select an Email Template --";
            EmailTemplates.Rows.InsertAt(NoneEmailTemplateRow, 0);

            this.ddlWarningEmail.DataSource = EmailTemplates;
            this.ddlWarningEmail.DataBind();

            this.ddlOverdueEmail.DataSource = EmailTemplates;
            this.ddlOverdueEmail.DataBind();

            this.CmpltEmailTmpltText.DataSource = new DataView(CmpltEmailTemplates, "", "Name", DataViewRowState.CurrentRows);
            this.CmpltEmailTmpltText.DataBind();

            #endregion
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

        //string iDaysDueAfterPrevStage = "";
        //if (_DaysDueAfterPrevStage != string.Empty)
        //{
        //    iDaysDueAfterPrevStage = _DaysDueAfterPrevStage;
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
                CompletionEmailRow["TemplTaskid"] = 0;
                CompletionEmailRow["TemplEmailId"] = Convert.ToInt32(sCompetionEmailID);
                CompletionEmailRow["Enabled"] = Convert.ToBoolean(sEnabled);

                CompletionEmailList.Rows.Add(CompletionEmailRow);

                #endregion
            }
        }

        #endregion

        #region build sql command - insert Template_Wfl_Tasks

        string sSql = "insert into Template_Wfl_Tasks (WflStageId,Name,Enabled,Type,DaysDueFromCoe,PrerequisiteTaskId,DaysDueAfterPrerequisite,OwnerRoleId,WarningEmailId,OverdueEmailId,CompletionEmailId,SequenceNumber,Description,DaysFromCreation,ExternalViewing,DaysDueAfterPrevStage) values (@WflStageId,@Name,@Enabled,@Type,@DaysDueFromCoe,@PrerequisiteTaskId,@DaysDueAfterPrerequisite,@OwnerRoleId,@WarningEmailId,@OverdueEmailId,@CompletionEmailId,@SequenceNumber,@Description,@DaysFromCreation,@ExternalViewing,@DaysDueAfterPrevStage);"
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
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Create workflow task successfully.');window.parent.location.href=window.parent.location.href;", true);
    }
}
