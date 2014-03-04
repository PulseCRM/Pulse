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



public partial class LogLeadTaskPopup : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        this.hdnNowDate.Value = DateTime.Now.ToString("MM/dd/yyyy");
        this.hdnNowTime.Value = DateTime.Now.ToString("hh:mm");

        

        if (!IsPostBack)
        {
            this.ddlDueTime_hour.SelectedValue = DateTime.Now.Hour.ToString("00");
            this.ddlDueTime_min.SelectedValue = (DateTime.Now.Minute / 5 * 5).ToString("00");

            if (cbInterestOnly.Checked == true)
            {
                //if (string.IsNullOrEmpty(txtDate.Text.Trim()))
                //{
                //    txtDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                //}

                //if (string.IsNullOrEmpty(txtTime.Text.Trim()))
                //{
                //    txtTime.Text = System.DateTime.Now.ToString("HH:mm");
                //}

                txtDate.Enabled = false;
                txtTime.Enabled = false;



            }


            #region 加载ddlTaskList for TaskNaem

            LeadTaskList LeadTaskListMgr = new LeadTaskList();

            string sOrderBy = string.Empty;
            if (this.CurrUser.SortTaskPickList == "S")
            {
                sOrderBy = "SequenceNumber";
            }
            else
            {
                sOrderBy = "TaskName";
            }

            DataTable LeadTaskList1 = LeadTaskListMgr.GetLeadTaskList(" and Enabled=1", sOrderBy);

            DataRow EmptyTaskRow = LeadTaskList1.NewRow();
            EmptyTaskRow["TaskName"] = "-- select --";
            LeadTaskList1.Rows.InsertAt(EmptyTaskRow, 0);

            this.ddlTaskList.DataSource = LeadTaskList1;
            this.ddlTaskList.DataBind();

            #endregion

        }


    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Loans LoanManager = new Loans();
        CompanyTaskPickList bllTaskPickList = new CompanyTaskPickList();
        LoginUser CurrentUser = new LoginUser(); ;

        int field = 0;
        if (Request.QueryString["FileId"] != null)
        {
            field = int.Parse(Request.QueryString["FileId"].ToString());
        }

        var loanInfo = LoanManager.GetModel(field);

        string taskName = rbtaskNameInput.Checked ? txtTaskname.Text.Trim() : ddlTaskList.SelectedValue.Trim();


        if (string.IsNullOrEmpty(txtTaskname.Text.Trim()) && string.IsNullOrEmpty(ddlTaskList.SelectedValue))
        {
            //PageCommon.WriteJs(this, "Task Name is Null!", "");
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>alert('Task Name is Null!')</script>");
        }
        else
        {

            if (loanInfo == null || loanInfo.Status != "Prospect")  //CR54 当为Prospect时检查重复
            {

                DataSet ds = bllTaskPickList.GetList("TaskName like '%" + taskName + "%'");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //说明存在相同的TaskName
                    // PageCommon.WriteJs(this, "Task Name Repeat!", "");
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>alert('Task Name Repeat!')</script>");
                    txtTaskname.Text = "";

                    return;
                }
            }

            LoanTasks lt = new LoanTasks();


            
            int Owner = CurrentUser.iUserID;

            string CompletedBy = "0";

            if (cbInterestOnly.Checked == true)
            {
                CompletedBy = CurrentUser.iUserID.ToString();
            }

            DateTime Completed = new DateTime();
            if (cbInterestOnly.Checked == true)
            {
                Completed = DateTime.Now;
            }

            DateTime Due = new DateTime();

            //if (cbInterestOnly.Checked == true)
            //{

            //string strdate = "";
            //if (!string.IsNullOrEmpty(txtDate.Text.Trim()))
            //{
            //    string[] date = txtDate.Text.Split('/');   //将字符串转换成数组arr1   

            //    strdate = date[2] + "-" + date[0] + "-" + date[1];
            //}

            //if (!string.IsNullOrEmpty(txtTime.Text.Trim()))
            //{
            //    Due = DateTime.Parse(strdate + " " + txtTime.Text.Trim());
            //}

            //}


            string strdate = "";
            string strTime = "";
            if (!string.IsNullOrEmpty(txtDate.Text.Trim()))
            {
                string[] date = txtDate.Text.Split('/');   //将字符串转换成数组arr1   

                strdate = date[2] + "-" + date[0] + "-" + date[1];

                Due = DateTime.Parse(strdate);
            }


            //if (!string.IsNullOrEmpty(txtTime.Text.Trim()))
            //{
            //    strTime = txtTime.Text.Trim();
            //}
            strTime = ddlDueTime_hour.SelectedValue + ":" + ddlDueTime_min.SelectedValue;


            string note = txbNotes.Text.Trim();

            string msg = lt.CreateLoanTasks(field, taskName, int.Parse(CompletedBy), Due, strTime, Owner, Completed, cbInterestOnly.Checked, note);

            if (msg == "Success")
            {
                PageCommon.WriteJsEnd(this, "Task saved successfully.", "window.parent.location.href=window.parent.location.href");
            }
            else
            {
                PageCommon.WriteJsEnd(this, "Failed to save the task.", "window.parent.location.href=window.parent.location.href");
            }

        }

    }


    #region#检查

    [System.Web.Services.WebMethod()]
    public static string ShowTaskName(string str)   //方法是静态的       
    {
        CompanyTaskPickList bllTaskPickList = new CompanyTaskPickList();
        string msg = "success";

        if (!string.IsNullOrEmpty(str))
        {
            DataSet ds = bllTaskPickList.GetList("TaskName like '%" + str.Trim() + "%'");

            if (ds.Tables[0].Rows.Count > 0)
            {
                //说明存在相同的TaskName
                msg = "Task Name Repeat!";

            }
        }
        else
        {
            msg = "Task Name is Null!";
        }

        return msg;
    }



    #endregion




}

