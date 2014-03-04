using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.BLL;

public partial class LPWeb_TaskReminderSnooze : BasePage
{
    string sTaskIDs = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        bool bValid = PageCommon.ValidateQueryString(this, "TaskIDs", QueryStringType.IDs);
        if (bValid == false)
        {
            PageCommon.WriteJsEnd(this, "Invalid task id.", PageCommon.Js_RefreshParent);
        }
        this.sTaskIDs = this.Request.QueryString["TaskIDs"];

        #endregion
    }

    protected void btnSnooze_Click(object sender, EventArgs e) 
    {
        string sMinutes = this.txtMinutes.Text.Trim();
        int iMinutes = Convert.ToInt32(sMinutes);

        LoanTasks LoanTaskManager = new LoanTasks();
        LoanTaskManager.SnoozeTask(this.sTaskIDs, iMinutes);

        // success
        if (this.Request.QueryString["All"] == null)
        {
            PageCommon.WriteJsEnd(this, "Snooze successfully.", PageCommon.Js_RefreshParent);
        }
        else
        {
            PageCommon.WriteJsEnd(this, "Snooze successfully.", "window.parent.parent.CloseGlobalPopup();");
        }
    }
}
