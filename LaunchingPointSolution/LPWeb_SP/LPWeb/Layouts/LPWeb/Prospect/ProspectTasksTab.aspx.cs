using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using Utilities;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using System.Text;


public partial class ProspectTasksTab : BasePage
{
    int iLoanID = 0;
    LoginUser CurrentUser;
    private readonly ProspectTasks bProspectTasks = new ProspectTasks();
    private string sErrorMsg = "Failed to load current page: invalid FileID.";
    private string sReturnPage = "LoanTasksTab.aspx";
    private bool isReset = false;
    protected string sContactID = "";
    protected string sHasUpdateRight = "0";

    #region Properties

    /// <summary>
    /// Gets or sets the current Contact id.
    /// </summary>
    /// <value>The current file id.</value>
    protected int CurrentContactId
    {
        set
        {
            hfdFileId.Value = value.ToString();
            ViewState["fileId"] = value;
        }
        get
        {
            if (ViewState["fileId"] == null)
                return 0;
            int fileId = 0;
            int.TryParse(ViewState["fileId"].ToString(), out fileId);

            return fileId;
        }
    }

    /// <summary>
    /// Gets or sets the grid view sort direction.
    /// </summary>
    /// <value>The grid view sort direction.</value>
    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;
            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    /// <summary>
    /// Gets or sets the name of the order.
    /// </summary>
    /// <value>The name of the order.</value>
    public string OrderName
    {
        get
        {
            if (ViewState["orderName"] == null)
                ViewState["orderName"] = "TaskId";
            return Convert.ToString(ViewState["orderName"]);
        }
        set { ViewState["orderName"] = value; }
    }

    /// <summary>
    /// Gets or sets the type of the order.
    /// </summary>
    /// <value>The type of the order.</value>
    public int OrderType
    {
        get
        {
            if (ViewState["orderType"] == null)
                ViewState["orderType"] = 0;
            return Convert.ToInt32(ViewState["orderType"]);
        }
        set { ViewState["orderType"] = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CurrentUser = new LoginUser();
            //权限验证
            if (CurrentUser.userRole.ContactMgmt.ToString().IndexOf('5') > -1)
            {
                if (CurrentUser.userRole.ContactMgmt.ToString().IndexOf('1') == -1)
                {
                    this.btnNewTask.Disabled = true;
                }
                if (CurrentUser.userRole.ContactMgmt.ToString().IndexOf('2') == -1)
                {
                    btnUpdate.Enabled = false;
                }
                else
                {
                    sHasUpdateRight = "1";
                }
                if (CurrentUser.userRole.ContactMgmt.ToString().IndexOf('3') == -1)
                {
                    btnRemove.Enabled = false;
                }
              
            }
            else
            {
                Response.Redirect("../Unauthorize1.aspx");  // have not View Power
                return;
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }


        if (Request.QueryString["ContactID"] != null) // 如果有FileID
        {
            sContactID = Request.QueryString["ContactID"];

            if (PageCommon.IsID(sContactID) == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
            }

            CurrentContactId = Convert.ToInt32(sContactID);
        }

        //if (CurrentContactId == 0)
        //{
        //    return;
        //}

        hfdFileId.Value = CurrentContactId.ToString();


        string sSql = "select distinct a.FileId as TaskTypeID, case when a.Status='Prospect' then 'Lead' when a.Status='Processing' then 'Active Loan' else a.Status+' Loan' end+'-'+ISNULL(a.LienPosition,'')+'-'+ISNULL(a.PropertyAddr,'')+' Tasks' as TaskType "
                    + "from Loans as a inner join LoanContacts as b on a.FileId= b.FileId "
                    + "where b.ContactId=" + this.CurrentContactId + " and (b.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or b.ContactRoleId=dbo.lpfn_GetCoborrowerRoleId())";

        DataTable TaskTypeList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        #region Build Context Menu Items

        StringBuilder sbMenuItems = new StringBuilder();
        foreach (DataRow TaskTypeRow in TaskTypeList.Rows)
        {
            string sLoanID = TaskTypeRow["TaskTypeID"].ToString();
            string sLoanName = TaskTypeRow["TaskType"].ToString();

            sbMenuItems.AppendLine("<li><a href=\"#" + sLoanID + "\">" + sLoanName + "</a></li>");
        }
        this.ltrContextMenuItems.Text = sbMenuItems.ToString();

        #endregion

        if (!IsPostBack)
        {
            #region 加载Task Type Filter

            DataRow NewTaskTypeRow = TaskTypeList.NewRow();
            NewTaskTypeRow["TaskTypeID"] = DBNull.Value;
            NewTaskTypeRow["TaskType"] = "All Tasks";
            TaskTypeList.Rows.InsertAt(NewTaskTypeRow, 0);

            NewTaskTypeRow = TaskTypeList.NewRow();
            NewTaskTypeRow["TaskTypeID"] = 0;
            NewTaskTypeRow["TaskType"] = "Client Tasks";
            TaskTypeList.Rows.InsertAt(NewTaskTypeRow, 1);

            this.ddlTaskTypeFilter.DataSource = TaskTypeList;
            this.ddlTaskTypeFilter.DataBind();

            #endregion

            BindControlSource();
            BindGrid(1);
        }
    }


    private void BindControlSource()
    {
        string sSql = "select isnull(b.LastName+', '+b.FirstName,'None') as TaskOwner, isnull(OwnerId,0) as OwnerId "
                    + "from ProspectTasks as a left outer join Users as b on a.OwnerId=b.UserId "
                    + "where ContactId=" + this.CurrentContactId + " "
                    + "union "
                    + "select isnull(e.LastName+', '+e.FirstName,'None') as TaskOwner, isnull(a.Owner,0) as OwnerId "
                    + "from LoanTasks as a inner join Loans as b on a.FileId=b.FileId "
                    + "inner join LoanContacts as c on b.FileId=c.FileId "
                    + "left outer join Users as e on a.Owner=e.UserId "
                    + "where c.ContactId=" + this.CurrentContactId + " and (c.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or c.ContactRoleId=dbo.lpfn_GetCoborrowerRoleId())";

        DataTable TaskOwnerList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        DataRow NewTaskOwnerRow = TaskOwnerList.NewRow();
        NewTaskOwnerRow["TaskOwner"] = "All";
        NewTaskOwnerRow["OwnerId"] = DBNull.Value;
        TaskOwnerList.Rows.InsertAt(NewTaskOwnerRow, 0);

        ddlOwner.DataSource = TaskOwnerList;
        ddlOwner.DataBind();
    }

    private void BindGrid(int pageIndex)
    {
        int pageSize = AspNetPager1.PageSize;

        string sDbTable = "(select ProspectTaskId as TaskId, dbo.lpfn_GetProspectTaskIcon(ProspectTaskId) as Status, -1 as PrerequisiteTaskId, "
                        + "'Client Task' as TaskType, '' as LoanFile, TaskName, b.LastName+', '+b.FirstName as TaskOwner, Due, Completed, OwnerId, null as LoanID "
                        + "from ProspectTasks as a left outer join Users as b on a.OwnerId=b.UserId "
                        + "where ContactId=" + this.CurrentContactId + " "
                        + "union "
                        + "select distinct a.LoanTaskId as TaskId, dbo.lpfn_GetTaskIcon(a.LoanTaskId) as Status, a.PrerequisiteTaskId, "
                        + "case when b.Status='Prospect' then 'Lead Task' when b.Status='Processing' then 'Active Loan Task' else b.Status+' Loan Task' end as TaskType, "
                        + "d.Name as LoanFile, a.Name as TaskName, e.LastName+', '+e.FirstName as TaskOwner, a.Due, a.Completed, a.Owner as OwnerId, a.FileId as LoanID "
                        + "from LoanTasks as a inner join Loans as b on a.FileId=b.FileId "
                        + "inner join LoanContacts as c on b.FileId=c.FileId "
                        + "inner join PointFiles as d on c.FileId=d.FileId "
                        + "left outer join Users as e on a.Owner=e.UserId "
                        + "where c.ContactId=" + this.CurrentContactId + " and (c.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or c.ContactRoleId=dbo.lpfn_GetCoborrowerRoleId())) as t";

        string sWhere = GenerateQueryCondition();

        int iRowCount = LPWeb.DAL.DbHelperSQL.Count(sDbTable, sWhere);

        int iStartIndex = PageCommon.CalcStartIndex(pageIndex, pageSize);
        int iEndIndex = PageCommon.CalcEndIndex(iStartIndex, pageSize, iRowCount);

        DataTable ProspectTaskList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sDbTable, iStartIndex, iEndIndex, sWhere, OrderName, OrderType);

        AspNetPager1.RecordCount = iRowCount;
        AspNetPager1.CurrentPageIndex = pageIndex;
        AspNetPager1.PageSize = pageSize;

        AspNetPager1.PageSize = pageSize;
        AspNetPager1.RecordCount = iRowCount;

        gvTasks.DataSource = ProspectTaskList;
        gvTasks.DataBind();
    }

    /// <summary>
    /// 根据用户界面选择生成过滤条件
    /// </summary>
    /// <returns></returns>
    private string GenerateQueryCondition()
    {
        string strWhere = string.Empty;

        string sTaskTypeFilter = this.ddlTaskTypeFilter.SelectedValue;
        if (sTaskTypeFilter == string.Empty)    // All Tasks
        {

        }
        else if (sTaskTypeFilter == "0")    // Prospect Tasks
        {
            strWhere += " and TaskType='Client Task'";
        }
        else
        {
            strWhere += " and LoanID=" + sTaskTypeFilter;
        }

        if (this.ddlOwner.SelectedValue != "-1" && this.ddlOwner.SelectedValue != "")
        {
            if(this.ddlOwner.SelectedValue == "0")
            {
                strWhere += " AND OwnerID is null";
            }
            else
            {
                strWhere += " AND OwnerID = " + this.ddlOwner.SelectedValue;
            }
        }

        if (this.ddlStatus.SelectedValue != "-1" && this.ddlStatus.SelectedValue != "0")
        {
            if (this.ddlStatus.SelectedValue == "Uncompleted")
                strWhere += " AND isnull(Completed,'')='' ";
            else if(this.ddlStatus.SelectedValue == "Completed")
                strWhere += " AND isnull(Completed,'')!='' ";
        }

        if (this.ddlDue.SelectedValue != "-1" && this.ddlDue.SelectedValue != "0")
        {
            if (this.ddlDue.SelectedValue == "1")
                strWhere += " AND Due <= '" + DateTime.Now.AddDays(30).ToShortDateString() + "' AND isnull(Completed,'')=''";
            else if (this.ddlDue.SelectedValue == "2")
                strWhere += " AND Due <= '" + DateTime.Now.AddDays(14).ToShortDateString() + "' AND isnull(Completed,'')=''";
            else if (this.ddlDue.SelectedValue == "3")
                strWhere += " AND Due <= '" + DateTime.Now.AddDays(7).ToShortDateString() + "' AND isnull(Completed,'')=''";
            else if (this.ddlDue.SelectedValue == "4")
                strWhere += " AND Due = '" + DateTime.Now.AddDays(1).ToShortDateString() + "' AND isnull(Completed,'')=''";
            else if (this.ddlDue.SelectedValue == "5")
                strWhere += " AND Due = '" + DateTime.Now.ToShortDateString() + "' AND isnull(Completed,'')=''";
            else if (this.ddlDue.SelectedValue == "6")
                strWhere += " AND Due < '" + DateTime.Now.ToShortDateString() + "' AND isnull(Completed,'')=''";
        }



        return strWhere;
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        BindGrid(AspNetPager1.CurrentPageIndex);
    }

    protected void gvTasks_Sorting(object sender, GridViewSortEventArgs e)
    {
        OrderName = e.SortExpression;
        string sortExpression = e.SortExpression;
        if (GridViewSortDirection == SortDirection.Ascending) //设置排序方向
        {
            GridViewSortDirection = SortDirection.Descending;
            OrderType = 0;
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            OrderType = 1;
        }
        BindGrid(AspNetPager1.CurrentPageIndex);
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        string sTaskType = this.hfSelectedTaskType.Value;
        string ProspectTaskIds = this.hfContactIDs.Value;
        if(sTaskType == "Client Task")
        {
            ProspectTasks bProspectTasks = new ProspectTasks();
            bProspectTasks.DeleteProspectTasks(ProspectTaskIds, CurrentUser.iUserID);
        }
        else
        {
            #region 检查PrerequisiteTaskId

            string sSql = "select count(1) from LoanTasks where PrerequisiteTaskId=" + ProspectTaskIds;
            int iChildCount = Convert.ToInt32(LPWeb.DAL.DbHelperSQL.ExecuteScalar(sSql));
            if(iChildCount > 0)
            {
                PageCommon.WriteJsEnd(this, "Cannot delete a prerequisite task.", "window.location.href=window.location.href;");
            }

            #endregion

            int iTaskID = Convert.ToInt32(ProspectTaskIds);
            bool bIsSuccess = WorkflowManager.DeleteTask(iTaskID, this.CurrentUser.iUserID);
        }

        BindGrid(1);
    }

    //protected void btnComplate_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string ProspectTaskId = this.hfContactIDs.Value;
    //        ProspectTasks bProspectTasks = new ProspectTasks();
    //        bProspectTasks.ComplateSelProspectTask(Convert.ToInt32(ProspectTaskId), CurrentUser.iUserID);
            
    //        BindGrid(1);
    //    }
    //    catch
    //    { }
    //}

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        isReset = true;
        BindGrid(1);
    }
}

