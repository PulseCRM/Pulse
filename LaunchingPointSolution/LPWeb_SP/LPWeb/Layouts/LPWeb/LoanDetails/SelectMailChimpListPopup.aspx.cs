using System;
using System.Globalization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using Utilities;

public partial class SelectMailChimpListPopup : BasePage
{
    private LoginUser _curLoginUser = new LoginUser();
    LPWeb.BLL.MailChimpLists bllml = new LPWeb.BLL.MailChimpLists();
    private Contacts _bContacts = new Contacts();
    int iUserID = 0;
    private bool _isAccessAllMailChimpList = false;
    private string sContactIDs = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        iUserID = _curLoginUser.iUserID;
        sContactIDs = this.Request.QueryString["ContactIDs"].ToString();
        if (sContactIDs == "")
        {
            PageCommon.WriteJsEnd(this, " Cannot subscribe to a MailChimp list. The selected contact does not exist.", "window.parent.CloseGlobalPopup();");
        }
        string sNullEmailInfo = _bContacts.GetContactsEmailInfo(sContactIDs);
        if (sNullEmailInfo != "")
        {
            string sAlerInfo = "";
            if (sNullEmailInfo.IndexOf(";") > -1)
            {
                sAlerInfo = sNullEmailInfo + " do not have email address. Please de-select them and try again.";
            }
            else
            {
                sAlerInfo = sNullEmailInfo + " does not have an email address. Please de-select him and try again.";
            }
            PageCommon.WriteJsEnd(this, sAlerInfo, "window.parent.CloseGlobalPopup();");
        }
        if (!IsPostBack)
        {
            BindDDLBranch();
            BindDDLUsers();
            CheckRolePermistion(iUserID);
            BindGrid(1);
        }
    }

    /// <summary>
    /// Checks the role permistion.
    /// </summary>
    /// <param name="iUserId">The i user id.</param>
    private void CheckRolePermistion(int iUserId)
    {
        try
        {
            var bllRole = new Roles();
            var role = bllRole.GetRoleByUserID(iUserId);
            if (role != null && role.Rows.Count > 0)
            {
                if (role.Rows[0]["AccessAllMailChimpList"] != DBNull.Value)
                {
                    if ((role.Rows[0]["AccessAllMailChimpList"].ToString() == "1") || (role.Rows[0]["AccessAllMailChimpList"].ToString().ToLower() == "true"))
                    {
                        _isAccessAllMailChimpList = true;
                    }
                }
                this.ddlBranchs.Enabled = this.ddlUsers.Enabled = _isAccessAllMailChimpList;
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
    }

    #region Properties

    /// <summary>
    /// Gets or sets the current file id.
    /// </summary>
    /// <value>The current file id.</value>
    protected int CurrentFileId
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
                ViewState["orderName"] = "List";
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

    private void BindDDLBranch()
    {
        DataTable dt = this.GetUserBranches(this.CurrUser.iUserID, this.CurrUser.sRoleName);
        ddlBranchs.DataValueField = "BranchId";
        ddlBranchs.DataTextField = "Name";
        ddlBranchs.DataSource = dt;
        ddlBranchs.DataBind();

        ListItem item = new ListItem();
        item.Value = "-1";
        item.Text = " All Branches ";

        ddlBranchs.Items.Insert(0, item);
    }

    private DataTable GetUserBranches(int iUserID, string sRoleName) 
    {
        string sSql = string.Empty;
        if (sRoleName == "Executive")
        {
            sSql = "select a.BranchId,a.Name,a.Enabled from Branches as a inner join dbo.lpfn_GetUserBranches_Executive(" + iUserID + ") as b on a.BranchId=b.BranchID order by a.Name";
        }
        else if (sRoleName == "Branch Manager")
        {
            sSql = "select a.BranchId,a.Name,a.Enabled from Branches as a inner join dbo.lpfn_GetUserBranches_Branch_Manager(" + iUserID + ") as b on a.BranchId=b.BranchID order by a.Name";
        }
        else
        {
            sSql = "select a.BranchId,a.Name,a.Enabled from Branches as a inner join dbo.lpfn_GetUserBranches(" + iUserID + ") as b on a.BranchId=b.BranchID order by a.Name";
        }

        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private void BindDDLUsers()
    {
        var bllUsers = new Users();

        DataTable dt = bllUsers.GetAllUsers(int.Parse(ddlBranchs.SelectedValue));

        ddlUsers.DataValueField = "UserId";
        ddlUsers.DataTextField = "UserName";
        ddlUsers.DataSource = dt;
        ddlUsers.DataBind();

        var item = new ListItem();
        item.Value = "-1";
        item.Text = " All Users ";

        ddlUsers.Items.Insert(0, item);
    }
    private void BindGrid(int pageIndex)
    {
        CheckRolePermistion(iUserID);
        //if (CurrentFileId < 1)
        //{
        //    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
        //    return;
        //}

        int pageSize = AspNetPager1.PageSize;
        int recordCount = 0;

        string strWhere = " 1>0 ";

        if (ddlBranchs.SelectedIndex > 0)
        {
            strWhere += " AND BranchID = '" + ddlBranchs.SelectedValue + "'";
        }
        if (ddlUsers.SelectedIndex > 0)
        {
            strWhere += " AND UserId = '" + ddlUsers.SelectedValue + "'";
        }
        if (!_isAccessAllMailChimpList)
        {
            strWhere += " AND UserId = '" + iUserID.ToString(CultureInfo.InvariantCulture) + "'";
        }

        DataSet ds = bllml.GetMailChimpLists(pageSize, pageIndex, iUserID, strWhere, out recordCount, OrderName, OrderType);

        AspNetPager1.RecordCount = recordCount;
        AspNetPager1.CurrentPageIndex = pageIndex;
        AspNetPager1.PageSize = pageSize;


        gvMailChimpLists.DataSource = ds;
        gvMailChimpLists.DataBind();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindGrid(1);
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        BindGrid(AspNetPager1.CurrentPageIndex);
    }

    protected void gvMailChimpLists_Sorting(object sender, GridViewSortEventArgs e)
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

    protected void btnSelect_Click(object sender, EventArgs e)
    {

    }

    protected void ddlBranchs_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindDDLUsers();
        CheckRolePermistion(iUserID);
    }
}
