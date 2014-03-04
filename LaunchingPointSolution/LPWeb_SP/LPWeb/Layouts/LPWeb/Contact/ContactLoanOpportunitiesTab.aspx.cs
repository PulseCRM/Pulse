using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.Common;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;

public partial class ContactLoanOpportunitiesTab : BasePage
{
    int iLoanID = 0;
    LoginUser CurrentUser;
    private readonly Loans loan = new Loans();
    protected string sHasCreateRight = "0";
    public string FromURL = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Url != null)
        {
            FromURL = Request.Url.ToString();
        }

        //是否有Create权限
        CurrentUser = new LoginUser();
        sHasCreateRight = CurrentUser.userRole.Loans.ToString().IndexOf('A') > -1 ? "1" : "0"; //Create 

        if (Request.QueryString["ContactID"] != null) // 如果有FileID
        {
            string sContactID = Request.QueryString["ContactID"];

            if (PageCommon.IsID(sContactID) == false)
            {
                //PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
            }
            try
            {
                CurrentContactID = Convert.ToInt32(sContactID);
            }
            catch
            {
                CurrentContactID = 0;
            }
        }

        if (CurrentContactID == 0)
        {
            return;
        }

        hfdContactID.Value = CurrentContactID.ToString();

        if (!IsPostBack)
        {

            BindGrid(1);
        }
    }

    #region Properties

    /// <summary>
    /// Gets or sets the current contact id.
    /// </summary>
    /// <value>The current contact id.</value>
    protected int CurrentContactID
    {
        set
        {
            hfdContactID.Value = value.ToString();
            ViewState["ContactID"] = value;
        }
        get
        {
            if (ViewState["ContactID"] == null)
                return 0;
            int CID = 0;
            int.TryParse(ViewState["ContactID"].ToString(), out CID);

            return CID;
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
                ViewState["orderName"] = "ClientName";
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

    private void BindGrid(int pageIndex)
    {
        try
        {
            int pageSize = AspNetPager1.PageSize;
            int recordCount = 0;
            DataSet ds = loan.GetPartnerContactLoans(pageSize, pageIndex, " Contactid=" + CurrentContactID.ToString(), out recordCount, OrderName, OrderType);

            AspNetPager1.RecordCount = recordCount;
            AspNetPager1.CurrentPageIndex = pageIndex;
            AspNetPager1.PageSize = pageSize;

            gvLoans.DataSource = ds;
            gvLoans.DataBind();
        }
        catch
        { }
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        BindGrid(AspNetPager1.CurrentPageIndex);
    }

    protected void gvLoans_Sorting(object sender, GridViewSortEventArgs e)
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
}
