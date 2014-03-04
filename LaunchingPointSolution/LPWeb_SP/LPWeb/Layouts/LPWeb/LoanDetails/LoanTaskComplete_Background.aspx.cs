using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using System.Text;
using LPWeb.LP_Service;
using System.Data;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb;

public partial class LoanDetails_LoanTaskComplete_Background : BasePage
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

        // LoanID
        bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            return;
        }
        string sLoanID = this.Request.QueryString["LoanID"].ToString();
        int iLoanID = Convert.ToInt32(sLoanID);

        #endregion

        // json示例
        // {"ExecResult":"Success","ErrorMsg":"","EmailTemplateID":"1", "LoanClosed":"Yes"}
        // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}

        string sErrorMsg = string.Empty;
        string sEmailTemplateID = string.Empty;
        bool bIsSuccess = false;
        string LoanClosed = "No";
        var result = "";
        try
        {
            #region complete task

            int iEmailTemplateId = 0;
            bIsSuccess = LPWeb.DAL.WorkflowManager.CompleteTask(iTaskID, this.CurrUser.iUserID, ref iEmailTemplateId);

            if (bIsSuccess == false)
            {
                sErrorMsg = "Failed to invoke WorkflowManager.CompleteTask.";
                return;
            }

            if (iEmailTemplateId != 0)
            {
                sEmailTemplateID = iEmailTemplateId.ToString();
            }

            #endregion

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
                return;
            }

            #region invoke PointManager.UpdateStage()

            //add by  gdc 20111212  Bug #1306 
            LPWeb.BLL.PointFiles pfile = new PointFiles();
            var model = pfile.GetModel(iLoanID);
            if (model != null && !string.IsNullOrEmpty(model.Name.Trim()))
            {
                #region check Point File Status first
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient service = sm.StartServiceClient())
                {
                    CheckPointFileStatusReq checkFileReq = new CheckPointFileStatusReq();
                    checkFileReq.hdr = new ReqHdr();
                    checkFileReq.hdr.UserId = CurrUser.iUserID;
                    checkFileReq.hdr.SecurityToken = "SecurityToken";
                    checkFileReq.FileId = iLoanID;
                    CheckPointFileStatusResp checkFileResp = service.CheckPointFileStatus(checkFileReq);
                    if (checkFileResp == null || checkFileResp.hdr == null || !checkFileResp.hdr.Successful)
                    {
                        sErrorMsg = "Unable to get Point file status from Point Manager.";
                        WorkflowManager.UnCompleteTask(iTaskID, CurrUser.iUserID);
                        bIsSuccess = false;
                        return;
                    }
                    if (checkFileResp.FileLocked)
                    {
                        sErrorMsg = checkFileResp.hdr.StatusInfo;
                        WorkflowManager.UnCompleteTask(iTaskID, CurrUser.iUserID);
                        bIsSuccess = false;
                        return;
                    }
                }
                #endregion
                #region UPdatePointFileStage  WCF
                string sError = LoanTaskCommon.UpdatePointFileStage(iLoanID, this.CurrUser.iUserID, iLoanStageID);

                // the last one, sleep 1 second
                System.Threading.Thread.Sleep(1000);

                if (sError == string.Empty) // success
                {
                    sErrorMsg = "Completed task successfully.";
                }
                else
                {
                    sErrorMsg = "Completed task successfully but failed to update stage date in Point.";
                    //sErrorMsg = "Failed to update point file stage: " + sError.Replace("\"", "\\\"");
                } 
                #endregion
            }
            if (WorkflowManager.IsLoanClosed(iLoanID))
                LoanClosed = "Yes";
            return;
            #endregion
        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            sErrorMsg = "Completed task successfully but failed to update stage date in Point.";
            return;
        }
        catch (Exception ex)
        {
            if (bIsSuccess)
                sErrorMsg = "Completed task successfully but encountered an error:" + ex.Message;
            else
                sErrorMsg = "Failed to complete task, reason:" + ex.Message;
            //sErrorMsg = "Exception happened when invoke WorkflowManager.CompleteTask: " + ex.ToString().Replace("\"", "\\\"");
            bIsSuccess = false;
            return;
        }
        finally
        {
            if (bIsSuccess)
                result = "{\"ExecResult\":\"Success\",\"ErrorMsg\":\"" + sErrorMsg + "\",\"EmailTemplateID\":\"" + sEmailTemplateID + "\",\"TaskID\":\"" + sTaskID + "\",\"LoanClosed\":\"" + LoanClosed + "\"}";
            //result = "{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\",\"EmailTemplateID\":\"" + sEmailTemplateID + "\",\"LoanClosed\":\"" + LoanClosed + "\"}";
            else
                result = "{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}";
            this.Response.Write(result);
        }

        #endregion
    }
}