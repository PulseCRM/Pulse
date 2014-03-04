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

public partial class Contact_PartnerCompanyList : BasePage
{
    protected string sHasCreate = "0";
    protected string sHasModify = "0";
    protected string sHasDelete = "0";
    protected string sHasDisable = "0";
    protected string sHasAddBranch = "0";
    protected string sHasRemoveBranch = "0";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrUser.userRole.ContactCompany.ToString().IndexOf('1') == -1)
        {
            Response.Redirect("../Unauthorize.aspx");  // have not View Power
            return;
        }

        sHasCreate = CurrUser.userRole.ContactCompany.ToString().IndexOf('2') > -1 ? "1" : "0";
        sHasModify = CurrUser.userRole.ContactCompany.ToString().IndexOf('3') > -1 ? "1" : "0";
        sHasDelete = CurrUser.userRole.ContactCompany.ToString().IndexOf('4') > -1 ? "1" : "0";
        sHasDisable = CurrUser.userRole.ContactCompany.ToString().IndexOf('5') > -1 ? "1" : "0";
        sHasAddBranch = CurrUser.userRole.ContactCompany.ToString().IndexOf('6') > -1 ? "1" : "0";
        sHasRemoveBranch = CurrUser.userRole.ContactCompany.ToString().IndexOf('7') > -1 ? "1" : "0";

        if (sHasDelete == "0")
        {
            lnkDelete.Enabled = false;
        }
        if (sHasDisable=="0")
        {
            lnkDisable.Enabled=false;
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

        #region 加载Company列表

        this.CompanySqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

        string sWhere = string.Empty;

        bool bSetCondition = false;

        // Alphabet
        if (this.Request.QueryString["Alphabet"] != null)
        {
            string sAlphabet = this.Request.QueryString["Alphabet"].ToString();
            sWhere += " and (Name like '" + sAlphabet + "%')";

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

        int iRowCount = LPWeb.DAL.DbHelperSQL.Count(this.CompanySqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);

        // empty data text
        if (iRowCount == 0 && bSetCondition == true)
        {
            this.gridCompanyList.EmptyDataText = "There is no partner company by search criteria，please search again.";
        }
        else
        {
            this.gridCompanyList.EmptyDataText = "There is no partner company.";
        }

        this.AspNetPager1.RecordCount = iRowCount;

        this.CompanySqlDataSource.SelectParameters["Where"].DefaultValue = sWhere;
        this.gridCompanyList.DataBind();

        #endregion
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        string sSelectedCompanyID = this.hdnSelectedCompanyID.Value;
        int iSelectedCompanyID = Convert.ToInt32(sSelectedCompanyID);

        ContactCompanies ContactCompanyManager = new ContactCompanies();
        ContactCompanyManager.DeletePartnerCompany(iSelectedCompanyID);

        PageCommon.WriteJsEnd(this, "Deleted partner company successfully.", PageCommon.Js_RefreshSelf);
    }

    protected void lnkDisable_Click(object sender, EventArgs e)
    {
        string sSelectedCompanyID = this.hdnSelectedCompanyID.Value;
        int iSelectedCompanyID = Convert.ToInt32(sSelectedCompanyID);

        ContactCompanies ContactCompanyManager = new ContactCompanies();
        ContactCompanyManager.DisablePartnerCompany(iSelectedCompanyID);

        PageCommon.WriteJsEnd(this, "Disabled partner company successfully.", PageCommon.Js_RefreshSelf);
    }
}
