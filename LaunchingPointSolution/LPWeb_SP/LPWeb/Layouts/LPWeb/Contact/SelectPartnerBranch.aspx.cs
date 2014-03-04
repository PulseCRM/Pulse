using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;

public partial class Contact_SelectPartnerBranch : BasePage
{
    string sCloseDialogCodes = string.Empty;
    string sAction = string.Empty;
    int iCompanyID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sMissingMsg = "Missing required qurey string.";

        bool bIsValid = PageCommon.ValidateQueryString(this, "CloseDialogCodes", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sMissingMsg, "window.parent.location.href=window.parent.location.href;");
        }
        this.sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";

        // Action
        bIsValid = PageCommon.ValidateQueryString(this, "Action", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sMissingMsg, sCloseDialogCodes);
        }
        this.sAction = this.Request.QueryString["Action"].ToString();
        if (this.sAction != "Add" && this.sAction != "Remove")
        {
            PageCommon.WriteJsEnd(this, "Invalid query string.", sCloseDialogCodes);
        }

        // CompanyID
        bIsValid = PageCommon.ValidateQueryString(this, "CompanyID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sMissingMsg, sCloseDialogCodes);
        }
        this.iCompanyID = Convert.ToInt32(this.Request.QueryString["CompanyID"]);

        #endregion

        #region 加载Point Folder List

        DataTable BranchListData = null;

        if (this.sAction == "Add")
        {
            this.btnSelect.Visible = true;
            this.btnRemove.Visible = false;
            this.gridPartnerBranchList.EmptyDataText = "There is no partner branch for selection.";
            BranchListData = this.GetEnabledPartnerBranchList();
        }
        else
        {
            this.btnSelect.Visible = false;
            this.btnRemove.Visible = true;
            this.gridPartnerBranchList.EmptyDataText = "There is no partner branch to remove.";
            BranchListData = this.GetAssociatedPartnerBranchList(this.iCompanyID);
        }

        this.gridPartnerBranchList.DataSource = BranchListData;
        this.gridPartnerBranchList.DataBind();

        #endregion
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        string sBranchIDs = this.hdnSelectedBranchIDs.Value;

        ContactCompanies ContactCompanyManager = new ContactCompanies();
        ContactCompanyManager.AddBranchToCompany(this.iCompanyID, sBranchIDs);

        PageCommon.WriteJsEnd(this, "Added parnter branch successfully.", "window.parent.location.href=window.parent.location.href;");
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        string sBranchIDs = this.hdnSelectedBranchIDs.Value;
        ContactCompanies ContactCompanyManager = new ContactCompanies();
        ContactCompanyManager.RemoveBranchFromCompany(sBranchIDs);

        PageCommon.WriteJsEnd(this, "Removed partner branch successfully.", "window.parent.location.href=window.parent.location.href;");
    }

    private DataTable GetEnabledPartnerBranchList()
    {
        string sSql = "select * from ContactBranches where Enabled=1 and ContactCompanyId is null";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private DataTable GetAssociatedPartnerBranchList(int iCompanyID)
    {
        string sSql = "select * from ContactBranches where ContactCompanyId=" + iCompanyID;
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }
}
