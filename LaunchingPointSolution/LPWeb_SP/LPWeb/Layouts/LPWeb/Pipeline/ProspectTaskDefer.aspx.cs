using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.DAL;
using LPWeb.Common;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    public partial class ProspectTaskDefer : LayoutsPageBase
    {
        string sTaskIDs = string.Empty;
        ProspectTasks ProspectTaskManager = new ProspectTasks();

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 检查必要参数

            string sErrorJs = "window.parent.CloseDialog_DeferTask();";
            string sError_Missing = "Missing required query string.";
            string sError_Invalid = "Invalid query string.";

            #region TaskIDs

            if (this.Request.QueryString["TaskIDs"] == null)
            {
                PageCommon.RegisterJsMsg(this, sError_Missing, sErrorJs);
                return;
            }

            string sTempTaskIDs = this.Request.QueryString["TaskIDs"].ToString();

            if (Regex.IsMatch(sTempTaskIDs, @"^([1-9]\d*)(,[1-9]\d*)*$") == false)
            {
                PageCommon.RegisterJsMsg(this, sError_Invalid, sErrorJs);
                return;
            }

            this.sTaskIDs = sTempTaskIDs;

            #endregion

            #endregion
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            LoginUser CurrentUser = new LoginUser();
            int iDeferDays = Convert.ToInt32(this.txtDeferDays.Text);

            // successful Task IDs
            List<int> SuccessTasksIDs = new List<int>();

            // workflow api
            string[] TaskIDArray = this.sTaskIDs.Split(',');
            foreach (string sTaskID in TaskIDArray)
            {
                int iTaskID = Convert.ToInt32(sTaskID);
                bool bIsSuccess = WorkflowManager.DeferProspectTask(iTaskID, CurrentUser.iUserID, iDeferDays);
                if (bIsSuccess == true)
                {
                    SuccessTasksIDs.Add(iTaskID);
                }
            }

            if (SuccessTasksIDs.Count == TaskIDArray.Length)
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Defer task(s) successfully.');window.parent.RefreshPage();", true);
            }
            else
            {
                string sErrorMsg = "There are " + SuccessTasksIDs.Count + " success and " + (TaskIDArray.Length - SuccessTasksIDs.Count) + " failture of " + TaskIDArray.Length + " Tasks.";
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('" + sErrorMsg + "');window.parent.RefreshPage();", true);
            }
        }
    }
}
