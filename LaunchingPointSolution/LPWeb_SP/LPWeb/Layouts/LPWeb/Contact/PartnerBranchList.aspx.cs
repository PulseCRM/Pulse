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
using Utilities;


public partial class Contact_PartnerBranchList : BasePage
{
    private ContactBranches _bContactBranches = new ContactBranches();

    protected string sHasCreate = "0";
    protected string sHasModify = "0";
    protected string sHasDelete = "0";
    protected string sHasDisable = "0";
    protected string sHasAddBranch = "0";
    protected string sHasRemoveBranch = "0";
    protected string sHasModifyCompany = "0";
    protected string sHasModifyServiceType = "0";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrUser.userRole.ContactBranch.ToString().IndexOf('1') == -1)
        {
            Response.Redirect("../Unauthorize.aspx");  // have not View Power
            return;
        }

        sHasCreate = CurrUser.userRole.ContactBranch.ToString().IndexOf('2') > -1 ? "1" : "0";
        sHasModify = CurrUser.userRole.ContactBranch.ToString().IndexOf('3') > -1 ? "1" : "0";
        sHasDelete = CurrUser.userRole.ContactBranch.ToString().IndexOf('4') > -1 ? "1" : "0";
        sHasDisable = CurrUser.userRole.ContactBranch.ToString().IndexOf('5') > -1 ? "1" : "0";
        sHasAddBranch = CurrUser.userRole.ContactBranch.ToString().IndexOf('6') > -1 ? "1" : "0";
        sHasRemoveBranch = CurrUser.userRole.ContactBranch.ToString().IndexOf('7') > -1 ? "1" : "0";
        sHasModifyCompany = CurrUser.userRole.ContactCompany.ToString().IndexOf('3') > -1 ? "1" : "0"; //Company Modify
        sHasModifyServiceType = CurrUser.userRole.ServiceType.ToString().IndexOf('1') > -1 ? "1" : "0"; //Service Type View
        if (sHasDelete == "0")
        {
            btnDelete.Enabled = false;
        }
        if (sHasDisable == "0")
        {
            btnDisable.Enabled = false;
        }

        #region 加载Service Type Filter
        ServiceTypes ServiceTypeManager = new ServiceTypes();
        DataTable ServiceTypeList = ServiceTypeManager.GetServiceTypeList(" and (Enabled=1)");
        DataRow NewServiceTypeRow = ServiceTypeList.NewRow();
        NewServiceTypeRow["ServiceTypeId"] = "0";
        NewServiceTypeRow["Name"] = "All Service Types";
        ServiceTypeList.Rows.InsertAt(NewServiceTypeRow, 0);
        ServiceTypeList.AcceptChanges();

        this.ddlServiceType.DataSource = ServiceTypeList;
        this.ddlServiceType.DataBind();

        #endregion

        #region 加载Company Filter
        ContactCompanies ContactCompaniesManager = new ContactCompanies();
        DataSet ContactCompaniesList = ContactCompaniesManager.GetList("(Enabled=1) order by Name");
        //if (ContactCompaniesList != null && ContactCompaniesList.Tables[0].Rows.Count > 0)
        //{
        DataRow NewContactCompaniesRow = ContactCompaniesList.Tables[0].NewRow();
        NewContactCompaniesRow["ContactCompanyId"] = "0";
        NewContactCompaniesRow["Name"] = "All Companies";
        ContactCompaniesList.Tables[0].Rows.InsertAt(NewContactCompaniesRow, 0);
        ContactCompaniesList.Tables[0].AcceptChanges();

        this.ddlCompany.DataSource = ContactCompaniesList.Tables[0];
        this.ddlCompany.DataBind();
        //}

        #endregion

        #region 加载States Filter
        LPWeb.Layouts.LPWeb.Common.USStates.Init(this.ddlStates);
        #endregion

        #region 加载Company列表

        this.BranchSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

        string sWhere = string.Empty;

        bool bSetCondition = false;

        // Alphabet
        if (this.Request.QueryString["Alphabet"] != null)
        {
            string sAlphabet = this.Request.QueryString["Alphabet"].ToString();
            sWhere += " and (Branch like '" + sAlphabet + "%')";

            bSetCondition = true;
        }

        // Service Type
        if (this.Request.QueryString["ServiceType"] != null)
        {
            string sServiceType = this.Request.QueryString["ServiceType"].ToString();
            if (PageCommon.IsID(sServiceType) == true)
            {
                sWhere += " and (ServiceTypeId =" + SqlTextBuilder.ConvertQueryValue(sServiceType) + ")";
            }
            else
            {
                sWhere += " and (ServiceTypeId =0)";
            }

            bSetCondition = true;
        }

        // Company
        if (this.Request.QueryString["Company"] != null)
        {
            string sCompany = this.Request.QueryString["Company"].ToString();
            if (PageCommon.IsID(sCompany) == true)
            {
                sWhere += " and (ContactCompanyId =" + SqlTextBuilder.ConvertQueryValue(sCompany) + ")";
            }
            else
            {
                sWhere += " and (ContactCompanyId =0)";
            }

            bSetCondition = true;
        }

        // State
        if (this.Request.QueryString["State"] != null)
        {
            string sState = this.Request.QueryString["State"].ToString();
            if (sState != "")
            {
                sWhere += " and ([State] ='" + SqlTextBuilder.ConvertQueryValue(sState) + "')";

                bSetCondition = true;
            }

        }

        int iRowCount = LPWeb.DAL.DbHelperSQL.Count(this.BranchSqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);

        // empty data text
        if (iRowCount == 0 && bSetCondition == true)
        {
            this.gridBranchList.EmptyDataText = "There is no partner branch by search criteria，please search again.";
        }
        else
        {
            this.gridBranchList.EmptyDataText = "There is no partner branch.";
        }

        this.AspNetPager1.RecordCount = iRowCount;

        this.BranchSqlDataSource.SelectParameters["Where"].DefaultValue = sWhere;
        this.gridBranchList.DataBind();

        #endregion
    }
    /// <summary>
    /// Disable
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDisable_Click(object sender, EventArgs e)
    {
        var selctedStr = this.hiSelectedBranch.Value;
        if (!string.IsNullOrEmpty(selctedStr))
        {
            string[] selectedItems = selctedStr.Split(',');
            //delete the selected items
            DisableBranch(selectedItems);
            //reload the grid data
            PageCommon.WriteJsEnd(this, "Branch has been disabled successfully.", PageCommon.Js_RefreshSelf);
        }
        this.hiSelectedBranch.Value = "";
    }

    private void DisableBranch(string[] items)
    {
        int iItem = 0;
        foreach (var item in items)
        {
            if (int.TryParse(item, out iItem))
            {
                try
                {
                    _bContactBranches.DisableBranch(iItem);
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }
            }
        }
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        var selctedStr = this.hiSelectedBranch.Value;
        if (!string.IsNullOrEmpty(selctedStr))
        {
            string[] selectedItems = selctedStr.Split(',');
            //delete the selected items
            RemoveBranch(selectedItems);
            //reload the grid data
            PageCommon.WriteJsEnd(this, "Branch has been deleted successfully.", PageCommon.Js_RefreshSelf);
        }
        this.hiSelectedBranch.Value = "";
    }

    private void RemoveBranch(string[] items)
    {
        int iItem = 0;
        foreach (var item in items)
        {
            if (int.TryParse(item, out iItem))
            {
                try
                {
                    _bContactBranches.RomoveBranch(iItem);
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }
            }
        }
    }
}

