using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using Utilities;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;


public partial class EmploymentTab : BasePage
{
    #region Fields
    string sFileID = "0";
    string sContactID = "0";
    string sPageFrom = "LeadDetail";
    string FromURL = string.Empty;

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
        set
        {
            ViewState["sortDirection"] = value;
        }

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
                ViewState["orderName"] = "CompanyName";
            return Convert.ToString(ViewState["orderName"]);
        }
        set
        {
            ViewState["orderName"] = value;
        }
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
        set
        {
            ViewState["orderType"] = value;
        }
    }
    #endregion

    #region Event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Url != null)
        {
            FromURL = Request.Url.ToString();
        }
        sFileID = Request.QueryString["FileID"] == null ? "0" : Request.QueryString["FileID"].ToString();

        sContactID = Request.QueryString["ContactID"] == null ? "0" : Request.QueryString["ContactID"].ToString();

        sPageFrom = Request.QueryString["PageFrom"] == null ? "LeadDetail" : Request.QueryString["PageFrom"].ToString();

        if (sFileID == "0" && sContactID=="0")
        {
            return;
        }

        if (!IsPostBack)
        {
            BindContact();
            BindGrid();
        }
    }

    /// <summary>
    /// Handles the Sorting event of the gridList control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
    protected void gridList_Sorting(object sender, GridViewSortEventArgs e)
    {
        OrderName = e.SortExpression;
        string sortExpression = e.SortExpression;
        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            OrderType = 0;
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            OrderType = 1;
        }
        BindGrid();
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void ddlContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    /// <summary>
    /// Handles the Click event of the btnRemove control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        string employeeIDs = hdnEmplIds.Value;
        string errMsg = string.Empty;
        try
        {
            ProspectEmployment bll = new ProspectEmployment();
            bll.DeleteList(employeeIDs);
        }
        catch(Exception ex)
        {
            errMsg = ex.Message;
        }
        if (errMsg == string.Empty)
        {
            PageCommon.WriteJsEnd(this, "Removed successfully.", PageCommon.Js_RefreshSelf);
        }
        else
        {
            PageCommon.WriteJsEnd(this, errMsg, PageCommon.Js_RefreshSelf);
        }
    }

    #endregion

    #region Function
    private void BindContact()
    {
        string strWhere = " 1>0";
        if (sPageFrom == "LeadDetail")
        {
            strWhere += " AND FileID = '" + sFileID + "' ";
            ddlContacts.Enabled = true;
        }
        else
        {
            strWhere += " AND ContactID = '" + sContactID + "' ";
            ddlContacts.Enabled = false;
        }
        try
        {
            ProspectEmployment bll = new ProspectEmployment();
            DataSet ds = bll.ProspectContacts(strWhere);
            ddlContacts.DataSource = ds;
            ddlContacts.DataBind();
        }
        catch
        { }
    }

    /// <summary>
    /// Bind Grid
    /// </summary>
    private void BindGrid()
    {
        int pageIndex = 1;
        int pageSize = AspNetPager1.PageSize;
        if (AspNetPager1.CurrentPageIndex > 0)
            pageIndex = AspNetPager1.CurrentPageIndex;

        string queryCondition = GenOrgQueryCondition();

        int recordCount = 0;

        DataSet ds = null;
        try
        {
            ProspectEmployment bll = new ProspectEmployment();

            ds = bll.GetProspectEmployment(pageSize, pageIndex, queryCondition, out recordCount, OrderName, OrderType);
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

        AspNetPager1.PageSize = pageSize;
        AspNetPager1.RecordCount = recordCount;

        gridList.DataSource = ds;
        gridList.DataBind();
    }

    /// <summary>
    /// Query Condition
    /// </summary>
    /// <returns></returns>
    private string GenOrgQueryCondition()
    {
        string strWhere = " 1>0 ";

        if (sFileID != "0")
        {
            strWhere += " AND FileID = '" + sFileID + "'";
        }

        if (this.ddlContacts.SelectedIndex > -1)
        {
            strWhere += " AND ContactID = '" + this.ddlContacts.SelectedValue + "'";
        }

        return strWhere;
    }

    #endregion

}
