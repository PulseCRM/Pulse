using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Web.UI.WebControls;
using System.Text;
using Utilities;
using System.Data;
using System.Text.RegularExpressions;
using LPWeb.Common;

public partial class PartnerContactDetailReferralstab : BasePage
{
     
    private LoginUser _curLoginUser = new LoginUser();
    LPWeb.BLL.Prospect prospect = new LPWeb.BLL.Prospect();
    private string ContactID = ""; 
    protected void Page_Load(object sender, EventArgs e)
    {
        bool bIsValid = PageCommon.ValidateQueryString(this, "ContactID", QueryStringType.ID);
        if(bIsValid == false)
        {
            
        }
        
        ContactID = Request.QueryString["ContactID"]; 

        if (!IsPostBack)
        {
            BindGrid(1);
        }
    }

    /// <summary>
    /// Bind prospect loan gridview
    /// </summary>
    private void BindGrid(int pageIndex)
    {
        int pageSize = AspNetPager1.PageSize;
 
        string strWhare = GetSqlWhereClause();
        int recordCount = 0;
        int iContactID = Convert.ToInt32(ContactID);

        DataSet loansList = prospect.GetProspectRefLoansInfo(iContactID, this.CurrUser.iUserID, pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
        
        AspNetPager1.PageSize = pageSize;
        AspNetPager1.RecordCount = recordCount;

        gridList.DataSource = loansList;
        gridList.DataBind();
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        BindGrid(AspNetPager1.CurrentPageIndex); 
    }

    /// <summary>
    /// Get filter
    /// </summary>
    /// <returns></returns>
    private string GetSqlWhereClause()
    {
        string strWhere = " 1=1 ";

        if (!string.IsNullOrEmpty(ContactID))
            strWhere += string.Format(" AND Referral={0} ", ContactID);
        else
            strWhere += " AND 1=0 ";

        if (!string.IsNullOrEmpty(this.tbSentStart.Text.Trim()))
        {
            DateTime dt = new DateTime();
            DateTime dtNew = new DateTime();
            try
            {
                if (DateTime.TryParse(this.tbSentStart.Text.Trim(), out dt))
                {
                    dtNew = new DateTime(dt.Year, dt.Month, dt.Day);
                    strWhere += string.Format(" AND [Created] >= '{0}' ", dtNew.ToString());
                }
            }
            catch
            { }
        }

        if (!string.IsNullOrEmpty(this.tbSentEnd.Text.Trim()))
        {
            DateTime dt = new DateTime();
            DateTime dtNew = new DateTime();
            try
            {
                if (DateTime.TryParse(this.tbSentEnd.Text.Trim(), out dt))
                {
                    dtNew = dt.AddDays(1).AddMilliseconds(-1);
                    strWhere += string.Format(" AND [Created] <= '{0}' ", dtNew.ToString());
                }
            }
            catch
            { }
        }

        return strWhere;
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
                ViewState["sortDirection"] = SortDirection.Descending;
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
                ViewState["orderName"] = "Created";
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
        BindGrid(AspNetPager1.CurrentPageIndex);
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    { 
        BindGrid(1);
    }
}

