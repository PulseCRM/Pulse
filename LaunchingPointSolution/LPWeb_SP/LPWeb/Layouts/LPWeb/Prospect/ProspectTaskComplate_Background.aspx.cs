using System;
using System.Collections.ObjectModel;
using System.Data;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;
using LPWEBDAL = LPWeb.DAL;

namespace LPWeb.Layouts.LPWeb.Prospect
{
    public partial class ProspectTaskComplate_Background : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 接收参数

            // TaskID
            bool bIsValid = PageCommon.ValidateQueryString(this, "TaskID", QueryStringType.ID);
            if (bIsValid == false)
            {
                this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
                return;
            }
            string sTaskID = this.Request.QueryString["TaskID"].ToString();
            int iTaskID = Convert.ToInt32(sTaskID);
            string sLoanID = this.Request.QueryString["LoanID"].ToString();
            int iLoanID = Convert.ToInt32(sLoanID);

            #endregion

            // json示例
            // {"ExecResult":"Success","ErrorMsg":"","EmailTemplateID":"1"}
            // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}

            string sErrorMsg = string.Empty;
            string sEmailTemplateID = string.Empty;
            string LoanClosed = "No";

            ProspectTasks bpTasks = new ProspectTasks();
            int iEmailTemplateId = 0;
            bool bIsSuccess = false;
            string result = string.Empty;

            try
            {
                if (iLoanID == -1)
                {
                    bIsSuccess = bpTasks.ComplateSelProspectTask(iTaskID, this.CurrUser.iUserID, ref iEmailTemplateId);
                    if (bIsSuccess == false)
                    {
                        sErrorMsg = "Failed to CompleteTask.";
                        return;
                    }
                    else
                    {
                        sErrorMsg = "Completed task successfully.";
                    }
                }
                else
                {
                    bIsSuccess = LPWEBDAL.WorkflowManager.CompleteTask(iTaskID, this.CurrUser.iUserID, ref iEmailTemplateId);
                    if (bIsSuccess == false)
                    {
                        sErrorMsg = "Failed to invoke WorkflowManager.CompleteTask.";
                        return;
                    }

                    #region update point file stage

                    int iLoanStageID = 0;

                    #region get loan task info

                    LoanTasks LoanTaskManager = new LoanTasks();
                    DataTable LoanTaskInfo = LoanTaskManager.GetLoanTaskInfo(iTaskID);
                    if (LoanTaskInfo.Rows.Count == 0)
                    {
                        bIsSuccess = false;
                        sErrorMsg = "Invalid task id.";
                        return;
                    }
                    string sLoanStageID = LoanTaskInfo.Rows[0]["LoanStageId"].ToString();
                    if (sLoanStageID == string.Empty)
                    {
                        bIsSuccess = false;
                        sErrorMsg = "Invalid loan stage id.";
                        return;
                    }
                    iLoanStageID = Convert.ToInt32(sLoanStageID);

                    #endregion

                    bIsSuccess = true;
                    if (!WorkflowManager.StageCompleted(iLoanStageID))
                    {
                        sErrorMsg = "Completed task successfully.";
                    }

                    #region invoke PointManager.UpdateStage()

                    string sError = LoanTaskCommon.UpdatePointFileStage(iLoanID, this.CurrUser.iUserID, iLoanStageID);

                    if (sError == string.Empty) // success
                    {
                        sErrorMsg = "Completed task successfully.";
                    }
                    else
                    {
                        sErrorMsg = "Completed task successfully but failed to update stage date in Point.";
                    }
                    #endregion

                    if (iEmailTemplateId != 0)
                    {
                        //根据Lin 2011-02-28邮件，暂不增加发送邮件功能。
                        sEmailTemplateID = iEmailTemplateId.ToString();
                    }
                }
                    #endregion

                if (string.Equals(new Loans().GetLoanInfo(iLoanID).Rows[0]["Status"], "Processing"))
                    LoanClosed = "Yes";
            }
            catch (Exception ex)
            {
                if (bIsSuccess)
                    sErrorMsg = "Completed task successfully but encountered an error:" + ex.Message;
                else
                    sErrorMsg = "Failed to complete task, reason:" + ex.Message;
                bIsSuccess = false;
            }
            finally
            {
                if (bIsSuccess)
                {
                    this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\",\"EmailTemplateID\":\"" + sEmailTemplateID + "\",\"LoanID\":\"" + sLoanID + "\",\"LoanClosed\":\"" + LoanClosed + "\"}");
                }
                else
                {
                    this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
                }
                this.Response.End();
            } 
        }
    }
}