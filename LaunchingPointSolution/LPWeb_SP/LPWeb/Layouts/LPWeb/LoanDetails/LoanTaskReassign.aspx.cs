using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using System.Text.RegularExpressions;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;


public partial class LoanDetails_LoanTaskReassign : BasePage
{
    int iLoanID = 0;
    string sTaskIDs = string.Empty;
    DataTable LoanTaskList;
    LoanTasks LoanTaskManager = new LoanTasks();

    protected void Page_Load(object sender, EventArgs e)
    {
        string sErrorJs = "window.parent.CloseDialog_ReassignTask();";
        string sError_Missing = "Missing required query string.";
        string sError_Invalid = "Invalid query string.";

        #region 检查必要参数

        #region LoanID

        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.RegisterJsMsg(this, sError_Missing, sErrorJs);
            return;
        }

        this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

        #endregion

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

        #region 加载Loan Task信息

        this.LoanTaskList = this.LoanTaskManager.GetLoanTaskList(" and a.LoanTaskID in (" + this.sTaskIDs + ")");
        if (this.LoanTaskList.Rows.Count == 0)
        {
            PageCommon.RegisterJsMsg(this, sError_Invalid, sErrorJs);
            return;
        }

        this.rptTaskNameList.DataSource = this.LoanTaskList;
        this.rptTaskNameList.DataBind();

        #endregion

        Loans LoanManager = new Loans();

        #region 获取Borrower和Property信息

        #region Property

        DataTable LoanInfo = LoanManager.GetLoanInfo(this.iLoanID);
        if (LoanInfo.Rows.Count == 0)
        {
            PageCommon.RegisterJsMsg(this, "Invalid required query string.", sErrorJs);
            return;
        }
        string sPropertyAddress = LoanInfo.Rows[0]["PropertyAddr"].ToString();
        string sPropertyCity = LoanInfo.Rows[0]["PropertyCity"].ToString();
        string sPropertyState = LoanInfo.Rows[0]["PropertyState"].ToString();
        string sPropertyZip = LoanInfo.Rows[0]["PropertyZip"].ToString();

        string sProperty = sPropertyAddress + ", " + sPropertyCity + ", " + sPropertyState + " " + sPropertyZip;

        #endregion

        #region Borrower

        DataTable BorrowerInfo = LoanManager.GetBorrowerInfo(this.iLoanID);
        if (BorrowerInfo.Rows.Count == 0)
        {
            PageCommon.RegisterJsMsg(this, "There is no Borrower in this loan.", sErrorJs);
            return;
        }
        string sFirstName = BorrowerInfo.Rows[0]["FirstName"].ToString();
        string sMiddleName = BorrowerInfo.Rows[0]["MiddleName"].ToString();
        string sLastName = BorrowerInfo.Rows[0]["LastName"].ToString();

        string sBorrower = sLastName + ",  " + sFirstName;
        if (sMiddleName != string.Empty)
        {
            sBorrower += " " + sMiddleName;
        }

        this.lbProperty.Text = sProperty;
        this.lbBorrower.Text = sBorrower;

        #endregion

        #endregion

        if (this.IsPostBack == false)
        {
            #region 加载Owner

            DataTable OwnerList = this.LoanTaskManager.GetLoanTaskOwers_Reassign(this.iLoanID);

            DataRow EmptyOwnerRow = OwnerList.NewRow();
            EmptyOwnerRow["UserID"] = 0;
            EmptyOwnerRow["FullName"] = "-- select a new owner--";
            OwnerList.Rows.InsertAt(EmptyOwnerRow, 0);
            DataView dvUser = new DataView(OwnerList, "", "FullName", DataViewRowState.CurrentRows);
            this.ddlOwner.DataSource = dvUser;
            this.ddlOwner.DataBind();

            #endregion
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        LoginUser CurrentUser = new LoginUser();
        int iNewOwnerID = Convert.ToInt32(this.ddlOwner.SelectedItem.Value);
        string sNewOwnerName = this.ddlOwner.SelectedItem.Text;

        // successful Task IDs
        List<int> SuccessTasksIDs = new List<int>();

        // workflow api
        string[] TaskIDArray = this.sTaskIDs.Split(',');
        foreach (string sTaskID in TaskIDArray)
        {
            int iTaskID = Convert.ToInt32(sTaskID);
            
            bool bIsSuccess = WorkflowManager.AssignTask(iTaskID, iNewOwnerID);
            
            if (bIsSuccess == true)
            {
                SuccessTasksIDs.Add(iTaskID);
            }
        }

        if (SuccessTasksIDs.Count == TaskIDArray.Length)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success1", "$('#divContainer').hide();alert('Reasigned task successfully.');window.parent.RefreshPage();", true);
        }
        else
        {
            string sErrorMsg = "There are " + SuccessTasksIDs.Count + " successes and " + (TaskIDArray.Length - SuccessTasksIDs.Count) + " failtures of " + TaskIDArray.Length + " tasks.";
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed1", "$('#divContainer').hide();alert('" + sErrorMsg + "');window.parent.RefreshPage();", true);
        }
    }
}

