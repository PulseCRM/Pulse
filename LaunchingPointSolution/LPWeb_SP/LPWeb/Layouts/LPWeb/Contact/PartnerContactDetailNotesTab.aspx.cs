using System;
using System.Data;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.LP_Service;
using Microsoft.SharePoint.WebControls;
using Utilities;
using LoanActivities = LPWeb.Model.LoanActivities;


public partial class Contact_PartnerContactDetailNotesTab : LayoutsPageBase
{
    private readonly ContactNotes _bllContactNotes = new ContactNotes();
    private string sErrorMsg = "Failed to load current page: invalid ContactID.";
    private string sReturnPage = "PartnerContactDetailView.aspx";

    protected void Page_Load(object sender, EventArgs e)
    {
        var loginUser = new LoginUser();
        //权限验证
        hdnCreateNotes.Value = loginUser.userRole.Prospect.Contains("M") == true ? "1" : "0";

        if (Request.QueryString["ContactID"] != null) // 如果有ContactID
        {
            string sContactID = Request.QueryString["ContactID"];

            if (PageCommon.IsID(sContactID) == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
            }

            CurrentContactId = Convert.ToInt32(sContactID);
        }
        if (CurrentContactId == 0)
        {
            return;
        }

        if (!IsPostBack)
        {
            BindPage(1);
        }
    }

    private void BindPage(int pageIndex)
    {
        if (CurrentContactId < 1)
        {
            PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
            return;
        }

        ValidateButtonPermission();

        int pageSize = 20;
        int recordCount = 0;
        DataSet ds = _bllContactNotes.GetContactNotes(pageSize, pageIndex,
                                                     "ContactId=" + CurrentContactId,
                                                     out recordCount, OrderName, OrderType);
        AspNetPager1.RecordCount = recordCount;
        AspNetPager1.CurrentPageIndex = pageIndex;
        AspNetPager1.PageSize = pageSize;

        gvNoteList.DataSource = ds;
        gvNoteList.DataBind();
    }

    private void ValidateButtonPermission()
    {
        var loginUser = new LoginUser();
        btnNew.Disabled = !loginUser.userRole.CreateNotes;
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        BindPage(AspNetPager1.CurrentPageIndex);
    }

    protected void gvNoteList_Sorting(object sender, GridViewSortEventArgs e)
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
        BindPage(AspNetPager1.CurrentPageIndex);
    }
    #region Properties

    /// <summary>
    /// Gets or sets the current contact id.
    /// </summary>
    /// <value>The current contact id.</value>
    protected int CurrentContactId
    {
        set
        {
            hfdContactId.Value = value.ToString();
            ViewState["contactId"] = value;
        }
        get
        {
            if (ViewState["contactId"] == null)
                return 0;
            int contactId = 0;
            int.TryParse(ViewState["contactId"].ToString(), out contactId);

            return contactId;
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
                ViewState["orderName"] = "Created";
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
                ViewState["orderType"] = 1;
            return Convert.ToInt32(ViewState["orderType"]);
        }
        set { ViewState["orderType"] = value; }
    }

    #endregion
}

