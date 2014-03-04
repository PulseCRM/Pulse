using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using System.Data;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;
using System.Text;
using System.Text.RegularExpressions;



public partial class TaskReminderSnoozePopup : BasePage
{
    string sTaskIDs = string.Empty;
    string sError_Missing = "Missing required query string.";
    string sError_Invalid = "Invalid query string.";

    string sErrorJs = "";

    LoanTasks loanTasks = new LoanTasks();

    protected void Page_Load(object sender, EventArgs e)
    {
        
        //if (!IsPostBack)
        //{
            string sTempTaskIDs = this.Request.QueryString["TaskIDs"].ToString();

            if (Regex.IsMatch(sTempTaskIDs, @"^([1-9]\d*)(,[1-9]\d*)*$") == false)
            {
                PageCommon.RegisterJsMsg(this, sError_Invalid, sErrorJs);
                return;
            }

            this.sTaskIDs = sTempTaskIDs;
        //}
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try{
        string flag = Request.QueryString["flag"].ToString();

        string[] TaskIDArray = this.sTaskIDs.Split(',');
  

        if (flag == "0")
        {
            //说明没有全部选择 指刷新Task Reminder
            foreach (string sTaskID in TaskIDArray)
            {
                loanTasks.GetSnoozePopup(txtSnooze.Text.Trim(),sTaskID);
            }

               //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "alert('Snooze task(s) successfully.'); window.parent.location.href = window.parent.location.href;", true);

            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "alert('Snooze task(s) successfully.'); window.opener=null;window.close();", true);

         
        }
        else {

            //说明全部选择,关闭Task Reminder

             foreach (string sTaskID in TaskIDArray)
            {
                loanTasks.GetSnoozePopup(txtSnooze.Text.Trim(),sTaskID);
            }

            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "alert('Snooze task(s) successfully.');window.parent.close();", true);
        
        }

      
        }
        catch
        {
           string sErrorMsg = "The loanTasks update error!";
           this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_error", "alert('" + sErrorMsg + "');window.parent.location.href = window.parent.location.href;", true);
        }

        
   
    }

   
   

   
    
}

