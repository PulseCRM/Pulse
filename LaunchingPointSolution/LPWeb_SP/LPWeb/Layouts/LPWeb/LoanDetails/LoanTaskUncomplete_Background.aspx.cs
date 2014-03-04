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

public partial class LoanDetails_LoanTaskUncomplete_Background : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 接收参数

        // TaskID
        bool bIsValid = PageCommon.ValidateQueryString(this, "TaskID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string sTaskID = this.Request.QueryString["TaskID"].ToString();
        int iTaskID = Convert.ToInt32(sTaskID);

        // LoanID
        bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string sLoanID = this.Request.QueryString["LoanID"].ToString();
        int iLoanID = Convert.ToInt32(sLoanID);

        #endregion

        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}

        string sErrorMsg = string.Empty;
        int iLoanStageID;
        bool StageCompleted = false;

        try
        {
            #region get loan task info

            LoanTasks LoanTaskManager = new LoanTasks();
            DataTable LoanTaskInfo = LoanTaskManager.GetLoanTaskInfo(iTaskID);
            if (LoanTaskInfo.Rows.Count == 0)
            {
                this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid task id.\"}");
                return;
            }
            string sLoanStageID = LoanTaskInfo.Rows[0]["LoanStageId"].ToString();
            if (sLoanStageID == string.Empty)
            {
                this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid loan stage id.\"}");
                return;
            }
            iLoanStageID = Convert.ToInt32(sLoanStageID);

            #endregion
            #region uncomplete task

            StageCompleted = WorkflowManager.StageCompleted(iLoanStageID);

            bool bIsSuccess = LPWeb.DAL.WorkflowManager.UnCompleteTask(iTaskID, this.CurrUser.iUserID);

            if (bIsSuccess == false)
            {
                this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Failed to invoke WorkflowManager.UnCompleteTask api.\"}");
                return;
            }

            #endregion
        }
        catch (Exception ex)
        {
            sErrorMsg = "Failed to invoke Workflow Manager, reason:"+ex.Message;
            //sErrorMsg = "Exception happened when invoke WorkflowManager.UnCompleteTask: " + ex.ToString().Replace("\"", "\\\"");

            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
            return;
        }

        if (!StageCompleted)  // if the stage was not completed prior to the un-complete 
        {
            this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
            return;
        }
        try
        {
            //add by  gdc 20111212  Bug #1306 
            LPWeb.BLL.PointFiles pfile = new PointFiles();
            var model = pfile.GetModel(iLoanID);
            if (model != null && !string.IsNullOrEmpty(model.Name.Trim()))
            {
                #region check Point File Status first
                //ServiceManager sm = new ServiceManager();
                //using (LP2ServiceClient service = sm.StartServiceClient())
                //{
                //    CheckPointFileStatusReq checkFileReq = new CheckPointFileStatusReq();
                //    checkFileReq.hdr = new ReqHdr();
                //    checkFileReq.hdr.UserId = CurrUser.iUserID;
                //    checkFileReq.hdr.SecurityToken = "SecurityToken";
                //    checkFileReq.FileId = iLoanID;
                //    int emailTemplateId = 0;
                //    CheckPointFileStatusResp checkFileResp = service.CheckPointFileStatus(checkFileReq);
                //    if (checkFileResp == null || checkFileResp.hdr == null || !checkFileResp.hdr.Successful)
                //    {
                //        sErrorMsg = "Unable to get Point file status from Point Manager.";
                //        WorkflowManager.CompleteTask(iTaskID, CurrUser.iUserID, ref emailTemplateId);
                //        this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
                //        return;
                //    }
                //    if (checkFileResp.FileLocked)
                //    {
                //        sErrorMsg = checkFileResp.hdr.StatusInfo;
                //        WorkflowManager.CompleteTask(iTaskID, CurrUser.iUserID, ref emailTemplateId);
                //        this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
                //        return;
                //    }
                //}
                #endregion
                #region update point file stage

                string sError = LoanTaskCommon.UpdatePointFileStage(iLoanID, this.CurrUser.iUserID, iLoanStageID);

                // the last one, sleep 1 second
                System.Threading.Thread.Sleep(1000);

                if (sError == string.Empty) // success
                {
                    this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
                }
                else
                {
                    sErrorMsg = "Un-completed task successfully but failed to update stage date in Point.";
                    //sErrorMsg = "Uncomplete task successfully, but failed to update point file stage: " + sError.Replace("\"", "\\\"");

                    this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
                }

                #endregion
            }
            else
            {
                this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
            }
        }
        catch (Exception ex)
        {
            sErrorMsg = "Un-completed task successfully but failed to update stage date in Point, reason:"+ex.Message;
            //sErrorMsg = "Uncomplete task successfully, but exception happened when update point stage: " + ex.ToString().Replace("\"", "\\\"");

            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
        }
    }
}
