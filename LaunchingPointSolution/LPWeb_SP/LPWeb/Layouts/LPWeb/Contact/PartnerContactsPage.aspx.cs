using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.BLL;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;

public partial class PartnerContactsPage : BasePage
{
    ContactBranches bllContactBranchs = new ContactBranches();
    Contacts bllContact = new Contacts();
    public string FromURL = string.Empty;
    int PageIndex = 1;


    protected string sHasCreate = "0";
    protected string sHasModify = "0";
    protected string sHasDelete = "0";
    protected string sHasAssign = "0";
    protected string sHasView = "0";
    protected string sHasMerge = "0";
    protected string sHasAccessAllContacts = "0";


    protected void Page_Load(object sender, EventArgs e)
    {
        sHasCreate = CurrUser.userRole.ContactMgmt.ToString().IndexOf('1') > -1 ? "1" : "0";
        sHasModify = CurrUser.userRole.ContactMgmt.ToString().IndexOf('2') > -1 ? "1" : "0";
        sHasDelete = CurrUser.userRole.ContactMgmt.ToString().IndexOf('3') > -1 ? "1" : "0";
        sHasAssign = CurrUser.userRole.ContactMgmt.ToString().IndexOf('4') > -1 ? "1" : "0";
        sHasView = CurrUser.userRole.ContactMgmt.ToString().IndexOf('5') > -1 ? "1" : "0";
        sHasMerge = CurrUser.userRole.ContactMgmt.ToString().IndexOf('6') > -1 ? "1" : "0";
        sHasAccessAllContacts = CurrUser.userRole.AccessAllContacts == false ? "0" : "1";

        if (sHasDelete == "0")
        {
            //btnRemove.Enabled = false;
        }

        if (Request.Url != null)
        {
            FromURL = Request.Url.ToString();
        }
        if (!IsPostBack)
        {
            var bllUser = new LPWeb.BLL.Users();


            AssignContactUser();

            BindPageDefaultValues();

            BindContactsGrid(GenerateQueryCondition());
        }
    }

    /// <summary>
    /// Bind Page default values
    /// </summary>
    private void BindPageDefaultValues()
    {
        BindServiceTypes();
        BindBranches();
        BindCompanys();
    }

    /// <summary>
    /// 根据用户界面选择生成过滤条件
    /// </summary>
    /// <returns></returns>
    private string GenerateQueryCondition()
    {

        string sWhere = " 0=0 ";

        // Alphabet
        if (!string.IsNullOrEmpty(this.Request.QueryString["Alphabet"]))
        {
            string sAlphabet = this.Request.QueryString["Alphabet"].ToString();
            sWhere += " and (ContactName like '" + sAlphabet + "%')";

        }

        // Alphabet
        if (!string.IsNullOrEmpty(this.Request.QueryString["PageIndex"]))
        {
            string sPageIndex = this.Request.QueryString["PageIndex"].ToString();
            if (PageCommon.IsID(sPageIndex) == true && sPageIndex != "1")
            {
                try
                {
                    PageIndex = int.Parse(sPageIndex);
                }
                catch
                {
                    PageIndex = 1;
                }
            }
            else
            {
                PageIndex = 1;
            }
        }
        else
        {
            PageIndex = 1;
        }

        // Service Type
        if (!string.IsNullOrEmpty(this.Request.QueryString["ServiceType"]))
        {
            string sServiceType = this.Request.QueryString["ServiceType"].ToString();
            if (PageCommon.IsID(sServiceType) == true && sServiceType != "0")
            {
                sWhere += " and (ServiceTypeId =" + SqlTextBuilder.ConvertQueryValue(sServiceType) + ")";
            }
        }

        // CompanyID
        if (!string.IsNullOrEmpty(this.Request.QueryString["CompanyID"]))
        {
            string sCompanyID = this.Request.QueryString["CompanyID"].ToString();
            if (PageCommon.IsID(sCompanyID) == true && sCompanyID != "0")
            {
                sWhere += string.Format(" AND ContactCompanyId={0}", sCompanyID);
            }
        }

        if (!string.IsNullOrEmpty(this.Request.QueryString["BranchID"]))
        {
            string sBranchID = this.Request.QueryString["BranchID"].ToString();
            if (PageCommon.IsID(sBranchID) == true && sBranchID != "0")
            {
                sWhere += string.Format(" AND ContactBranchId={0}", sBranchID);
            }
        }

        if (this.Request.QueryString["Referral"] != null)
        {
            sWhere  += " and ContactId in (select top(10) ContactId from "
                    + "(select dbo.lpfn_GetTotalReferral(ContactId, " + this.CurrUser.iUserID + ") as TotalReferral, ContactId from lpvw_PartnerContacts) as t "
                    + "order by TotalReferral desc)";
        }

        if (!string.IsNullOrEmpty(this.Request.QueryString["ContactIds"]))
        {
            string sSearchConditions = this.Request.QueryString["ContactIds"].ToString();

            sWhere += LPWeb.Common.Encrypter.Base64Decode(sSearchConditions);
        }

        if (sHasAccessAllContacts == "0")
        {
            //sWhere += " AND ContactId in (select ContactId from ContactUsers where UserID="+ CurrUser.iUserID +") ";
            //数据权限 gdc Bug #1670
            sWhere += string.Format(@"AND t.ContactId IN(SELECT     dbo.Contacts.ContactId
                            FROM          dbo.LoanContacts INNER JOIN
                                                   dbo.Contacts ON dbo.LoanContacts.ContactId = dbo.Contacts.ContactId INNER JOIN
                                                   dbo.ContactRoles ON dbo.LoanContacts.ContactRoleId = dbo.ContactRoles.ContactRoleId AND dbo.ContactRoles.Name <> 'Borrower' AND 
                                                   dbo.ContactRoles.Name <> 'CoBorrower' 
                                                    AND dbo.LoanContacts.FileId in (SELECT LoanID FROM dbo.[lpfn_GetUserLoans2] ('{0}', '{1}')))", CurrUser.iUserID, CurrUser.bAccessOtherLoans);
        }

        return sWhere;
    }


    /// <summary>
    /// Assign Contact
    /// </summary>
    private void AssignContactUser()
    { 
        string sSelUserIDs = string.Empty;
        string selctedStr = string.Empty;
        if (!string.IsNullOrEmpty(this.Request.QueryString["AssignUserIDs"]))
        {
            sSelUserIDs = this.Request.QueryString["AssignUserIDs"];
        }
        else
        {
            return;
        }

        if (!string.IsNullOrEmpty(this.Request.QueryString["SelContactIDs"]))
        {
            selctedStr = this.Request.QueryString["SelContactIDs"];
        }
        else
        {
            return;
        }

        if (sSelUserIDs != "" && selctedStr != "")
        {
            ContactBranches _bContactBranches = new ContactBranches();
            foreach (string ContactID in selctedStr.Split(','))
            {
                foreach (string sUserID in sSelUserIDs.Split(','))
                {
                    try
                    {
                        _bContactBranches.AssignUser2Contact(Convert.ToInt32(sUserID), Convert.ToInt32(ContactID));
                    }
                    catch
                    { }
                }
            }
        }
    }

    private void BindServiceTypes()
    {
        try
        {
            LPWeb.BLL.ServiceTypes st = new ServiceTypes();
            ddlServiceType.DataSource = st.GetList(" Enabled=1 order by Name");
            ddlServiceType.DataBind();

            var item = new ListItem("All Service Types", "0") { Selected = true };
            ddlServiceType.Items.Insert(0, item);
        }
        catch
        { }
    }

    private void BindBranches()
    {
        try
        {
            LPWeb.BLL.ContactBranches bllBranch = new ContactBranches();
            ddlBranchs.DataSource = bllBranch.GetList(" Enabled=1 order by Name");
            ddlBranchs.DataBind();

            var item = new ListItem("All Branches", "0") { Selected = true };
            ddlBranchs.Items.Insert(0, item);

        }
        catch
        { }
    }

    private void BindCompanys()
    {
        try
        {
            LPWeb.BLL.ContactCompanies bllCC = new ContactCompanies();
            ddlCompanies.DataSource = bllCC.GetList(" Enabled=1 order by Name"); ;
            ddlCompanies.DataBind();

            var item = new ListItem("All Companies", "0") { Selected = true };
            ddlCompanies.Items.Insert(0, item);

        }
        catch
        { }
    }

    /// <summary>
    /// Bind Grid
    /// </summary>
    private void BindContactsGrid(string queryCondition)
    {
        int pageSize = 20;
        int pageIndex = 1;

        pageIndex = PageIndex;

        int recordCount = 0;

        if(this.Request.QueryString["Referral"] != null)
        {
            this.OrderName = "TotalReferral";
            this.OrderType = 1;
        }

        DataSet ds = null;
        try
        {
            ds = bllContactBranchs.GetPartnerContacts(this.CurrUser.iUserID, pageSize, pageIndex, queryCondition, out recordCount, OrderName, OrderType);            
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

        AspNetPager1.PageSize = pageSize;
        AspNetPager1.RecordCount = recordCount;

        gvPartnerContacts.DataSource = ds;
        gvPartnerContacts.DataBind();
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
                ViewState["orderName"] = "ContactName";
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

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        //BindContactsGrid();
    }

    protected void ddlAlphabets_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindContactsGrid();
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        //ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
        //BindContactsGrid();
    }

    /// <summary>
    /// Handles the Sorting event of the gvPartnerContacts control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
    protected void gvPartnerContacts_Sorting(object sender, GridViewSortEventArgs e)
    {
        OrderName = e.SortExpression;
        string sortExpression = e.SortExpression;
        if (GridViewSortDirection == SortDirection.Ascending)                      //设置排序方向
        {
            GridViewSortDirection = SortDirection.Descending;
            OrderType = 0;
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            OrderType = 1;
        }
        BindContactsGrid(GenerateQueryCondition());
    }

}
