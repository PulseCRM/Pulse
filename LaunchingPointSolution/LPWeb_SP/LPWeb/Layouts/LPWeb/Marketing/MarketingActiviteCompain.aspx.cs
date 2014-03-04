using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Text;
using Utilities;

public partial class MarketingActiviteCompain : BasePage
{
    int iCampaignID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.Request.QueryString["CampaignId"] != null)
            {
                if (!int.TryParse(this.Request.QueryString["CampaignId"].ToString(), out this.iCampaignID))
                {
 
                }
            }
            BindDll();
            BindGrid();
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
                ViewState["orderName"] = "CampaignName";
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


    private void BindDll()
    {
        BindCatagorys();
        BindCampaigns(0);
        BindStartedBy();
    }

    private void BindCatagorys()
    {
        try
        {
            LPWeb.BLL.MarketingCategory mc = new LPWeb.BLL.MarketingCategory();
            DataSet ds = mc.GetList(0, "", " CategoryName ");

            ddlCategories.DataSource = ds;
            ddlCategories.DataBind();

            ddlCategories.Items.Insert(0, new ListItem("All Categories", "0"));
        }
        catch
        { }
    }

    private void BindCampaigns(int CategoryID)
    {
        try
        {
            LPWeb.BLL.MarketingCampaigns mc = new LPWeb.BLL.MarketingCampaigns();
            string strWhere = string.Empty;
            if (CategoryID > 0)
            {
                strWhere = " CategoryId = '" + CategoryID.ToString() + "' ";
            }

            DataSet ds = mc.GetList(0, strWhere, " CampaignName ");


            ddlCampaigns.DataSource = ds;
            ddlCampaigns.DataBind();

            ddlCampaigns.Items.Insert(0, new ListItem("All Campaigns", "0"));


            foreach (ListItem item in ddlCampaigns.Items)
            {
                item.Attributes.Add("title", item.Text);
            }


        }
        catch
        { }
    }

    private void BindStartedBy()
    {
        #region 加载Started By Filter

        try
        {
            string sSql = "select distinct isnull(a.StartedBy,0) as StartedByID, isnull(b.LastName +', '+b.FirstName,'None') as StartedByName "
                        + "from LoanMarketing as a "
                        + "left outer join Users as b on a.StartedBy=b.UserId "
                        + " WHERE 1>0";
            if (this.iCampaignID != 0)
            {
                sSql += " AND a.CampaignId=" + this.iCampaignID;
            }
            DataTable StartedByList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

            DataRow NewStartedByRow = StartedByList.NewRow();
            NewStartedByRow["StartedByID"] = DBNull.Value;
            NewStartedByRow["StartedByName"] = "Started By";
            StartedByList.Rows.InsertAt(NewStartedByRow, 0);

            this.ddlStartedBy.DataSource = StartedByList;
            this.ddlStartedBy.DataBind();
        }
        catch
        { }
        #endregion
    }

    private void BindGrid()
    {
        //int pageSize = AspNetPager1.PageSize;
        try
        {
            if (CurrUser != null)
            {
                LPWeb.BLL.Users users = new LPWeb.BLL.Users();
                LPWeb.Model.Users u = users.GetModel(CurrUser.iUserID);
                if (u != null)
                {
                    //pageSize = u.LoansPerPage;
                    AspNetPager1.PageSize = u.LoansPerPage;
                }
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

        string sWhere = GetCondition();
        this.MarketingActivitySqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;
        string sDbTable = "(select a.*,i.CategoryId, b.CampaignName, f.LastName +', '+ f.FirstName + case when ISNULL(f.MiddleName, '') != '' then ' '+ f.MiddleName else '' end as ClientName, "
                        + " c.Status+'-'+Substring(d.Name,Charindex('\',d.Name,2)+1,len(d.Name)-Charindex('\',d.Name,2)) as LoanName, "
                        + " g.LastName +', '+g.FirstName as StartedByName, h.Success, h.Error "
                        + " from LoanMarketing as a inner join MarketingCampaigns as b on a.CampaignId=b.CampaignId "
                        + " inner join MarketingCategory as i on b.CategoryId=i.CategoryId "
                        + " inner join Loans as c on a.FileId=c.FileId "
                        + " inner join PointFiles as d on c.FileId=d.FileId "
                        + " inner join LoanContacts as e on d.FileId=e.FileId "
                        + " inner join Contacts as f on e.ContactId=f.ContactId "
                        + " left outer join Users as g on a.StartedBy=g.UserId "
                        + " left outer join MarketingLog h on a.LoanMarketingId=h.LoanMarketingId) as t ";

        this.MarketingActivitySqlDataSource.SelectParameters["DbTable"].DefaultValue = sDbTable;
        int iRowCount1 = LPWeb.DAL.DbHelperSQL.Count(this.MarketingActivitySqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);

        this.AspNetPager1.RecordCount = iRowCount1;

        this.MarketingActivitySqlDataSource.SelectParameters["Where"].DefaultValue = sWhere;
        this.gridMarketingActivityList.DataBind();
    }

    private string GetCondition()
    {
        StringBuilder sWhere = new StringBuilder();
        //ddlCategories
        if (ddlCategories.SelectedIndex > 0)
        {
            string CategoryId = ddlCategories.SelectedValue;
            sWhere.Append(" and CategoryId = '" + CategoryId + "'");
        }

        //Campaigns
        if (ddlCampaigns.SelectedIndex > 0)
        {
            string CampainID = ddlCampaigns.SelectedValue;
            sWhere.Append(" and CampaignId = '" + CampainID + "'");
        }

        // Alphabet
        if (ddlAlphabet.SelectedIndex > 0)
        {
            string sAlphabet = ddlAlphabet.SelectedValue.ToLower();
            sWhere.Append(" and lower(ClientName) like '" + sAlphabet + "%'");
        }

        // Status
        if (ddlStatus.SelectedIndex > 0)
        {
            string sStatus = ddlStatus.SelectedItem.Text;
            sWhere.Append("  and Status='" + sStatus + "'");
        }

        // Type
        if (ddlType.SelectedIndex > 0)
        {
            string sType = ddlType.SelectedItem.Text;
            sWhere.Append(" and Type='" + sType + "'");
        }

        // Started By
        if (ddlStartedBy.SelectedIndex > 0)
        {
            string sStartedBy = ddlStartedBy.SelectedValue;

            sWhere.Append(" and StartedBy='" + sStartedBy + "'");

        }

        #region FromDate and ToDate

        string sFromDate = string.Empty;
        string sToDate = string.Empty;

        sFromDate = txbFromDate.Text.Trim();
        sToDate = txbToDate.Text.Trim();

        DateTime? FromDate = null;
        DateTime? ToDate = null;

        if (sFromDate != string.Empty)
        {
            DateTime FromDate1;
            bool IsDate1 = DateTime.TryParse(sFromDate, out FromDate1);
            if (IsDate1 == true)
            {
                FromDate = FromDate1;
            }
        }

        if (sToDate != string.Empty)
        {
            DateTime ToDate1;
            bool IsDate2 = DateTime.TryParse(sToDate, out ToDate1);
            if (IsDate2 == true)
            {
                ToDate = ToDate1;
            }
        }

        sWhere.Append(SqlTextBuilder.BuildDateSearchCondition("Started", FromDate, ToDate));

        #endregion

        return sWhere.ToString();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void ddlAlphabet_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
    {
        int CategoryID = 0;

        int CategoryID1;
        bool IsCategoryID = int.TryParse(ddlCategories.SelectedValue, out CategoryID1);
        if (IsCategoryID == true)
        {
            CategoryID = CategoryID1;
        }
        BindCampaigns(CategoryID);
    }
}

