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

public partial class Contact_PartnerCompanyEdit : BasePage
{
    int iCompanyID = 0;
    protected string sHasCreate = "0";
    protected string sHasModify = "0";
    protected string sHasDelete = "0";
    protected string sHasDisable = "0";
    protected string sHasAddBranch = "0";
    protected string sHasRemoveBranch = "0";

    protected void Page_Load(object sender, EventArgs e)
    {
        string sReturnJs = "window.location.href='PartnerCompanyList.aspx';";

        #region 权限控制
        sHasCreate = CurrUser.userRole.ContactBranch.ToString().IndexOf('2') > -1 ? "1" : "0";
        sHasModify = CurrUser.userRole.ContactBranch.ToString().IndexOf('3') > -1 ? "1" : "0";
        sHasDelete = CurrUser.userRole.ContactBranch.ToString().IndexOf('4') > -1 ? "1" : "0";
        sHasDisable = CurrUser.userRole.ContactBranch.ToString().IndexOf('5') > -1 ? "1" : "0";
        sHasAddBranch = CurrUser.userRole.ContactBranch.ToString().IndexOf('6') > -1 ? "1" : "0";
        sHasRemoveBranch = CurrUser.userRole.ContactBranch.ToString().IndexOf('7') > -1 ? "1" : "0";

        if (sHasDisable == "0")
        {
            lnkDisableBranch.Enabled = false;
        }
        if (sHasDelete == "0")
        {
            lnkDeleteBranch.Enabled = false;
        } 
        #endregion

        #region 检查页面参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "CompanyID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", sReturnJs);
        }
        this.iCompanyID = Convert.ToInt32(this.Request.QueryString["CompanyID"]);

        #endregion

        #region 加载Company信息

        ContactCompanies ContactCompanyManager = new ContactCompanies();
        DataTable CompanyInfo = ContactCompanyManager.GetContactCompanyInfo(this.iCompanyID);
        if (CompanyInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid query string.", sReturnJs);
        }

        #endregion

        #region 加载Branch列表

        this.BranchSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

        string sWhere = " and (ContactCompanyId=" + this.iCompanyID + ")";

        bool bSetCondition = false;

        // Alphabet
        if (this.Request.QueryString["Alphabet"] != null)
        {
            string sAlphabet = this.Request.QueryString["Alphabet"].ToString();
            sWhere += " and (Name like '" + sAlphabet + "%')";

            bSetCondition = true;
        }

        int iRowCount = LPWeb.DAL.DbHelperSQL.Count(this.BranchSqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);

        // empty data text
        if (iRowCount == 0 && bSetCondition == true)
        {
            this.gridBranchList.EmptyDataText = "There is no Partner Branch by search criteria，please search again.";
        }
        else
        {
            this.gridBranchList.EmptyDataText = "There is no Partner Branch.";
        }

        this.AspNetPager1.RecordCount = iRowCount;

        this.BranchSqlDataSource.SelectParameters["Where"].DefaultValue = sWhere;
        this.gridBranchList.DataBind();

        #endregion

        if (this.IsPostBack == false)
        {
            #region 加载Service Type Filter

            ServiceTypes ServiceTypeManager = new ServiceTypes();
            DataTable ServiceTypeList = ServiceTypeManager.GetServiceTypeList(" and (Enabled=1)");
            DataRow NewServiceTypeRow = ServiceTypeList.NewRow();
            NewServiceTypeRow["ServiceTypeId"] = DBNull.Value;
            NewServiceTypeRow["Name"] = "-- select --";
            ServiceTypeList.Rows.InsertAt(NewServiceTypeRow, 0);
            ServiceTypeList.AcceptChanges();

            this.ddlServiceType.DataSource = ServiceTypeList;
            this.ddlServiceType.DataBind();

            #endregion

            #region 绑定Company数据

            this.txtCompanyName.Text = CompanyInfo.Rows[0]["Name"].ToString();
            this.ddlServiceType.SelectedValue = CompanyInfo.Rows[0]["ServiceTypeId"].ToString();


            if (CompanyInfo.Rows[0]["Enabled"] == DBNull.Value)
            {
                this.chkEnabled.Checked = true;
            }
            else
            {
                this.chkEnabled.Checked = Convert.ToBoolean(CompanyInfo.Rows[0]["Enabled"]);
            }

            this.txtAddress.Text = CompanyInfo.Rows[0]["Address"].ToString();
            this.txtCity.Text = CompanyInfo.Rows[0]["City"].ToString();
            this.ddlState.SelectedValue = CompanyInfo.Rows[0]["State"].ToString();
            this.txtZip.Text = CompanyInfo.Rows[0]["Zip"].ToString();
            this.tbWebsite.Text = string.Format("{0}", CompanyInfo.Rows[0]["Website"].ToString());

            #endregion
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sCompanyName = this.txtCompanyName.Text.Trim();
        string sServiceTypeID = this.ddlServiceType.SelectedValue;
        int iServiceTypeID = Convert.ToInt32(sServiceTypeID);
        string sServiceType = this.ddlServiceType.SelectedItem.Text;
        string sAddress = this.txtAddress.Text.Trim();
        string sCity = this.txtCity.Text.Trim();
        string sState = this.ddlState.SelectedValue;
        string sZip = this.txtZip.Text.Trim();
        string sWebsite = this.tbWebsite.Text.Trim();

        ContactCompanies ContactCompanyManager = new ContactCompanies();
        ContactCompanyManager.UpdateContactCompany(this.iCompanyID, sCompanyName, iServiceTypeID, sServiceType, sAddress, sCity, sState, sZip, this.chkEnabled.Checked, sWebsite);

        PageCommon.WriteJsEnd(this, "Updated partner company successfully.", "window.parent.location.href='PartnerCompanyList.aspx';");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ContactCompanies ContactCompanyManager = new ContactCompanies();
        ContactCompanyManager.DeletePartnerCompany(this.iCompanyID);

        PageCommon.WriteJsEnd(this, "Deleted partner company successfully.", "window.location.href='PartnerCompanyList.aspx';");
    }

    protected void lnkDisableBranch_Click(object sender, EventArgs e)
    {
        string sSelBranchID = this.hdnSelectedBranchID.Value;
        int iSelBranchID = Convert.ToInt32(sSelBranchID);

        ContactBranches ContactBranchManager = new ContactBranches();
        ContactBranchManager.DisableBranch(iSelBranchID);

        PageCommon.WriteJsEnd(this, "Disabled partner branch successfully.", PageCommon.Js_RefreshSelf);
    }

    protected void lnkDeleteBranch_Click(object sender, EventArgs e)
    {
        string sSelBranchID = this.hdnSelectedBranchID.Value;
        int iSelBranchID = Convert.ToInt32(sSelBranchID);

        ContactBranches ContactBranchManager = new ContactBranches();
        ContactBranchManager.RomoveBranch(iSelBranchID);

        PageCommon.WriteJsEnd(this, "Deleted partner branch successfully.", PageCommon.Js_RefreshSelf);
    }
}