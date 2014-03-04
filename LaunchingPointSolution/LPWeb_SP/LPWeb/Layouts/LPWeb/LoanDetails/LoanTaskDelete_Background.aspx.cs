using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.BLL;
using System.Data;
using LPWeb.LP_Service;
using LPWeb.Layouts.LPWeb;

public partial class LoanDetails_LoanTaskDelete_Background : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 接收参数

        if (this.Request.QueryString["TaskIDs"] == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            return;
        }
        string sTaskIDs = this.Request.QueryString["TaskIDs"].ToString();
        if (Regex.IsMatch(sTaskIDs, @"^([1-9]\d*)(,[1-9]\d*)*$") == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid query string[" + sTaskIDs + "].\"}");
            return;
        }

        // LoanID
        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            return;
        }
        string sLoanID = this.Request.QueryString["LoanID"].ToString();
        int iLoanID = Convert.ToInt32(sLoanID);

        #endregion

        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}

        string sExecResult = string.Empty;
        string sErrorMsg = string.Empty;
        string sLoanClosed = "No";
        string result = "";
        bool bIsSuccess = false;
        try
        {
            // workflow api          
            string[] TaskIDArray = sTaskIDs.Split(',');
            foreach (string sTaskID in TaskIDArray)
            {
                int iTaskID = Convert.ToInt32(sTaskID);

                #region get loan task info

                LoanTasks LoanTaskManager = new LoanTasks();
                DataTable LoanTaskInfo = LoanTaskManager.GetLoanTaskInfo(iTaskID);
                if (LoanTaskInfo.Rows.Count == 0)
                {
                    sErrorMsg = "Failed to delete the tasks(s) invalid task id.";
                    return;
                }
                string sLoanStageID = LoanTaskInfo.Rows[0]["LoanStageId"].ToString();
                if (sLoanStageID == string.Empty)
                {
                    sErrorMsg = "Failed to delete the task(s) invalid Stage Id";
                    return;
                }
                int iLoanStageID = Convert.ToInt32(sLoanStageID);

                #endregion

                #region delete task

                bIsSuccess = LPWeb.DAL.WorkflowManager.DeleteTask(iTaskID, this.CurrUser.iUserID);

                if (bIsSuccess == false)
                {
                    sErrorMsg = "Failed to delete the task(s) due to a Workflow Manager problem.";
                    return;
                }
                bool StageCompletedAfter = LPWeb.BLL.WorkflowManager.StageCompleted(iLoanStageID);

                #endregion
                if (StageCompletedAfter)
                {
                    #region update point file stage

                    #region invoke PointManager.UpdateStage()
                    
                    string sError = LoanTaskCommon.UpdatePointFileStage(iLoanID, this.CurrUser.iUserID, iLoanStageID);

                    // the last one, sleep 1 second
                    System.Threading.Thread.Sleep(1000);

                    if (sError == string.Empty) // success
                    {
                        bIsSuccess = true;
                        sErrorMsg = "Deleted the task(s) successfully.";
                    }
                    else
                    {
                        sErrorMsg = "Deleted the task(s) but failed to update stage date in Point.";
                        return;
                    }

                    if (WorkflowManager.IsLoanClosed(iLoanID))
                        sLoanClosed = "Yes";

                    #endregion

                    #endregion

                }
            }
            return;
        }
        catch (Exception ex)
        {
            sErrorMsg = "Failed to delete selected task(s): " + ex.ToString().Replace("\"", "\\\"");
            return;
        }
        finally
        {
            string sEmailTemplateID = "";
            if (bIsSuccess)
                result = "{\"ExecResult\":\"Success\",\"ErrorMsg\":\"" + sErrorMsg + "\",\"EmailTemplateID\":\"" + sEmailTemplateID + "\",\"LoanClosed\":\"" + sLoanClosed + "\"}";
            else
                result = "{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}";
            Response.Write(result);

        }
    }
}

