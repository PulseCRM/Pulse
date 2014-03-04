using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.Common;
using System.Data;

public partial class Settings_CloneWorkflowTaskAjax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // {"ExecResult":"Success"}
        // {"ExecResult":"Failed","ErrorMsg":"unknown errors."}

        #region 接收参数

        // WflTaskID
        bool bIsValid = PageCommon.ValidateQueryString(this, "WflTaskID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Lose required query string.\"}");
            this.Response.End();
        }
        string sWflTaskID = this.Request.QueryString["WflTaskID"];
        int iWflTaskID = Convert.ToInt32(sWflTaskID);
        
        #endregion

        #region insert workflow task

        try
        {
            // Template_Wfl_Tasks
            string sSql = "insert into Template_Wfl_Tasks (WflStageId,Name,Enabled,Type,DaysDueFromCoe,PrerequisiteTaskId,DaysDueAfterPrerequisite,OwnerRoleId,WarningEmailId,OverdueEmailId,CompletionEmailId,SequenceNumber,Description,DaysFromCreation,ExternalViewing) "
                        + "select WflStageId,Name+' Copy' as Name,Enabled,Type,DaysDueFromCoe,PrerequisiteTaskId,DaysDueAfterPrerequisite,OwnerRoleId,WarningEmailId,OverdueEmailId,CompletionEmailId,SequenceNumber,Description,DaysFromCreation,ExternalViewing from dbo.Template_Wfl_Tasks where TemplTaskId=" + sWflTaskID + " "
                        + "select SCOPE_IDENTITY()";

            int iNewTaskID = Convert.ToInt32(LPWeb.DAL.DbHelperSQL.ExecuteScalar(sSql));

            // Template_Wfl_CompletionEmails
            string sSql2 = "insert into Template_Wfl_CompletionEmails (TemplTaskid,TemplEmailId,Enabled) "
                         + "select " + iNewTaskID + ", TemplEmailId,Enabled from Template_Wfl_CompletionEmails where TemplTaskid=" + iWflTaskID;
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql2);

        }
        catch (Exception)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Failed to clone task.\"}");
            this.Response.End();
        }
        
        #endregion

        this.Response.Write("{\"ExecResult\":\"Success\"}");
        this.Response.End();
    }
}

