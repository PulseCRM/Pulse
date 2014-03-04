using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.BLL;
using System.Data;
using LPWeb.Layouts.LPWeb;
using LPWeb.Common;

public partial class LPWeb_TaskReminderPopup : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsPostBack == false)
        {
            if (this.CurrUser.RemindTaskDue == true && this.CurrUser.TaskReminder != null)
            {
                LoanTasks LoanTaskManager = new LoanTasks();
                DataTable ReminderTaskList = LoanTaskManager.GetReminderTaskList(this.CurrUser.iUserID, (int)this.CurrUser.TaskReminder);
                this.gridTaskList.DataSource = ReminderTaskList;
                this.gridTaskList.DataBind();
            }
        }
    }

    protected void lnkComplete_Click(object sender, EventArgs e)
    {
        string sSelTaskIDs = this.hdnSelTaskIDs.Value;
        string[] TaskIDArray = sSelTaskIDs.Split(',');

        foreach (string sTaskID in TaskIDArray)
        {
            int iLoanTaskId = Convert.ToInt32(sTaskID);
            string sResult = this.CompleteTask(iLoanTaskId);
            if (sResult != string.Empty)
            {
                PageCommon.WriteJsEnd(this, "Failed to complete task(s).", PageCommon.Js_RefreshSelf);
            }
        }

        // success
        PageCommon.WriteJsEnd(this, "Complete task(s) successfully.", PageCommon.Js_RefreshSelf);
    }

    private string CompleteTask(int iLoanTaskId)
    {
        #region complete task

        string sErrorMsg = string.Empty;
        int iEmailTemplateId = 0;
        bool bIsSuccess = LPWeb.DAL.WorkflowManager.CompleteTask(iLoanTaskId, this.CurrUser.iUserID, ref iEmailTemplateId);

        if (bIsSuccess == false)
        {
            sErrorMsg = "Failed to invoke WorkflowManager.CompleteTask.";
            return sErrorMsg;
        }

        #endregion

        #region update point file stage

        int iLoanStageID = 0;

        #region get loan task info

        LoanTasks LoanTaskManager = new LoanTasks();
        DataTable LoanTaskInfo = LoanTaskManager.GetLoanTaskInfo(iLoanTaskId);
        if (LoanTaskInfo.Rows.Count == 0)
        {
            sErrorMsg = "Invalid task id.";
            return sErrorMsg;
        }
        string sLoanStageID = LoanTaskInfo.Rows[0]["LoanStageId"].ToString();
        if (sLoanStageID == string.Empty)
        {
            sErrorMsg = "Invalid loan stage id.";
            return sErrorMsg;
        }
        iLoanStageID = Convert.ToInt32(sLoanStageID);
        int iLoanID = Convert.ToInt32(LoanTaskInfo.Rows[0]["FileId"]);

        #endregion
        if (WorkflowManager.StageCompleted(iLoanStageID) == true)
        {
            #region invoke PointManager.UpdateStage()

            //add by  gdc 20111212  Bug #1306 
            LPWeb.BLL.PointFiles pfile = new PointFiles();
            var model = pfile.GetModel(iLoanID);
            if (model != null && !string.IsNullOrEmpty(model.Name.Trim()))
            {
                #region UPdatePointFileStage  WCF

                string sError = LoanTaskCommon.UpdatePointFileStage(iLoanID, this.CurrUser.iUserID, iLoanStageID);

                #endregion
            }

            #endregion
        }

        #endregion

        return sErrorMsg;
    }
}
