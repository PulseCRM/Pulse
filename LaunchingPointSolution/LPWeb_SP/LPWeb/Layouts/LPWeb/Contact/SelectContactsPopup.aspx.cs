using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using Utilities;
using LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.BLL;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;

public partial class Contact_SelectContactsPopup : LayoutsPageBase
{
    private ContactBranches bllContactBranchs = new ContactBranches();
    private string sBranchID = "0";
    private string sWhere = "";
    private string sType = "add";
    protected string sHasAccessAllContacts = "0";
    private LoginUser _curLoginUser = new LoginUser();

    protected void Page_Load(object sender, EventArgs e)
    {
        sBranchID = this.Request.QueryString["ContactBranchId"] != null ? this.Request.QueryString["ContactBranchId"].ToString() : "0";
        sType = this.Request.QueryString["type"] != null ? this.Request.QueryString["type"].ToString() : "add";
        sWhere = this.Request.QueryString["sCon"] != null ? this.Request.QueryString["sCon"].ToString() : "";
        sHasAccessAllContacts = _curLoginUser.userRole.AccessAllContacts == false ? "0" : "1";
        if (!IsPostBack)
        {
            if (sType == "add" || sType.ToLower().IndexOf("prospectdetail") >= 0 || sType == "ProspectLoanDetailInfo" || sType == "TabPage")
            {
                this.btnSelect.Text = "Select";
            }
            else
            {
                this.btnSelect.Text = "Unselect";
            }
            BindContactsGrid();
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
    /// <summary>
    /// Bind Grid
    /// </summary>
    private void BindContactsGrid()
    {
        int pageSize = this.Request.QueryString["pagesize"] != null ? Convert.ToInt32(this.Request.QueryString["pagesize"]) : 20; //gdc crm33 增加可自定义大小参数 减少页面高度
        int pageIndex = 1;

        if (AspNetPager1.CurrentPageIndex > 0)
            pageIndex = AspNetPager1.CurrentPageIndex;

        string queryCondition = GenerateQueryCondition();
        int recordCount = 0;

        DataTable dt = null;
        try
        {
            dt = bllContactBranchs.GePartnerContactsForSel(pageSize, pageIndex, queryCondition, out recordCount, OrderName, OrderType);
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

        AspNetPager1.PageSize = pageSize;
        AspNetPager1.RecordCount = recordCount;

        gridPartnerContactList.DataSource = dt;
        gridPartnerContactList.DataBind();
    }

    /// <summary>
    /// 根据用户界面选择生成过滤条件
    /// </summary>
    /// <returns></returns>
    private string GenerateQueryCondition()
    {
        string queryCon = " 1=1 ";
        if (sType == "add")
        {
            queryCon += " and isnull(ContactCompanyId,0)=0 AND isnull(ContactBranchId,0)=0 ";
        }
        else if (sType.ToLower().IndexOf("prospectdetail") >= 0 && this.sWhere != "")
        {
            queryCon += LPWeb.Common.Encrypter.Base64Decode(this.sWhere);
        }
        else if (sType == "ProspectLoanDetailInfo" && this.sWhere != "")
        {
            queryCon += LPWeb.Common.Encrypter.Base64Decode(this.sWhere);
        }
        else if (sType == "TabPage" && this.sWhere != "")
        {
            queryCon += LPWeb.Common.Encrypter.Base64Decode(this.sWhere);
        }
        else
        {
            queryCon += " and isnull(ContactBranchId,0)=" + sBranchID;
        }
        if (sHasAccessAllContacts == "0")
        {
            //queryCon += " AND ContactId in (select ContactId from ContactUsers where UserID=" + _curLoginUser.iUserID + ") ";
        }

        if (sBranchID != "0")  //若是 Branch模块调用，则不应包括Borrower, CoBorrower, 和Prospect
        {
            queryCon += @" AND ContactId not in (
                        select ContactId from dbo.[lpvw_GetLoanContactwRoles] where (RoleName='Borrower' or RoleName='CoBorrower')) and 
                        ContactId not in ( select ContactID from Prospect) ";
        }
        if (ddlFilter.SelectedValue != "")
        {
            queryCon += " and ContactName like '" + ddlFilter.SelectedValue + "%'";
        }
        return queryCon;
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
        BindContactsGrid();
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        string sContactIDs = this.hdnSelectedContactIDs.Value;
        string sContactName = this.hdnSelectedContactName.Value;

        if (sType == "add")
        {
            bllContactBranchs.AddContactToBranch(Convert.ToInt32(sBranchID), sContactIDs);
            PageCommon.WriteJsEnd(this, "Selected parnter contact successfully.", "window.parent.location.href=window.parent.location.href;");
        }
        else if (sType.ToLower().IndexOf("prospectdetail") >= 0)
        {
            PageCommon.WriteJsEnd(this, string.Empty, "window.parent.ShowReferral('" + sContactIDs + "','" + sContactName + "');");
        }
        else if (sType == "ProspectLoanDetailInfo")
        {
            string sArg = sContactIDs + ":" + sContactName;
            PageCommon.WriteJsEnd(this, string.Empty, "window.parent.InvokeFn('ShowReferral', '" + sArg + "');");
        }
        else if (sType == "TabPage")
        {
            string sArg = sContactIDs + ":" + sContactName;
            PageCommon.WriteJsEnd(this, string.Empty, "window.parent.InvokeFn('ShowReferral2', '" + sArg + "');");
        }
        else
        {
            bllContactBranchs.RemoveContactSetBranchNull(sContactIDs);
            PageCommon.WriteJsEnd(this, "Removed parnter contact successfully.", "window.parent.location.href=window.parent.location.href;");
        }

    }

    protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindContactsGrid();
    }


}

