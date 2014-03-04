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

public partial class Contact_PartnerBranchSetup : BasePage
{
    private string sContactBranchID;
    private string sCompanyName;

    private ContactBranches _bContactBranches = new ContactBranches();

    protected string sHasCreate = "0";
    protected string sHasModify = "0";
    protected string sHasDelete = "0";
    protected string sHasAssign = "0";
    protected string sHasMerge = "0";

    protected void Page_Load(object sender, EventArgs e)
    {
        sContactBranchID = this.Request.QueryString["branchID"] != null ? this.Request.QueryString["branchID"].ToString() : "0";
        sCompanyName = this.Request.QueryString["CompanyName"] != null ? this.Request.QueryString["CompanyName"].ToString() : "0";


        sHasCreate = CurrUser.userRole.ContactMgmt.ToString().IndexOf('1') > -1 ? "1" : "0";
        sHasModify = CurrUser.userRole.ContactMgmt.ToString().IndexOf('2') > -1 ? "1" : "0";
        sHasDelete = CurrUser.userRole.ContactMgmt.ToString().IndexOf('3') > -1 ? "1" : "0";
        sHasAssign = CurrUser.userRole.ContactMgmt.ToString().IndexOf('4') > -1 ? "1" : "0";
        sHasMerge = CurrUser.userRole.ContactMgmt.ToString().IndexOf('6') > -1 ? "1" : "0";

        if (!IsPostBack)
        {
            InitControl();
        }
       
    }

    private void InitControl()
    {
        #region 加载Primary Contact
        Contacts ContactsManager = new Contacts();

        ContactCompanies ContactCompaniesManager = new ContactCompanies();

        LPWeb.Model.ContactCompanies mContactCompanies = (LPWeb.Model.ContactCompanies)ContactCompaniesManager.GetModelbyName(sCompanyName);
    
        int iContactCompanyId = 0;

        if (mContactCompanies != null)
        {
            iContactCompanyId = (int)mContactCompanies.ContactCompanyId;
        }

        int iContactBranchID = 0;

        DataTable ContactsList = ContactsManager.GetEnableCompanyContactInfo(iContactCompanyId, iContactBranchID);

        if (ContactsList == null)
        {
            ContactsList = ContactsManager.GetEnableContactInfo();
        }

        DataRow NewContactsRow = ContactsList.NewRow();
        NewContactsRow["ContactId"] = DBNull.Value;
        NewContactsRow["Contact"] = "—select—";
        ContactsList.Rows.InsertAt(NewContactsRow, 0);
        ContactsList.AcceptChanges();

        this.ddlContact.DataSource = ContactsList;
        this.ddlContact.DataBind();

        #endregion

        #region 加载Company
        //ContactCompanies ContactCompaniesManager = new ContactCompanies();
        DataSet ContactCompaniesList = ContactCompaniesManager.GetList("(Enabled='true')");
        //if (ContactCompaniesList != null && ContactCompaniesList.Tables[0].Rows.Count > 0)
        //{
        DataRow NewContactCompaniesRow = ContactCompaniesList.Tables[0].NewRow();
        NewContactCompaniesRow["ContactCompanyId"] = DBNull.Value;
        NewContactCompaniesRow["Name"] = "All Companies";
        ContactCompaniesList.Tables[0].Rows.InsertAt(NewContactCompaniesRow, 0);
        ContactCompaniesList.Tables[0].AcceptChanges();

        this.ddlCompany.DataSource = ContactCompaniesList.Tables[0];
        this.ddlCompany.DataBind();

        if (this.Request.QueryString["CompanyName"] != null)
        {
            sCompanyName = this.Request.QueryString["CompanyName"].ToString();
            this.ddlCompany.SelectedItem.Text = sCompanyName;
            string sSelCompanyID = "";
            if (ContactCompaniesList != null && ContactCompaniesList.Tables[0].Rows.Count > 0)
            {
                sSelCompanyID = ContactCompaniesList.Tables[0].Select("Name='" + sCompanyName + "'")[0]["ContactCompanyId"].ToString();
            }
            this.ddlCompany.SelectedValue = sSelCompanyID;
        }
        //}

        #endregion

        #region 加载States
        LPWeb.Layouts.LPWeb.Common.USStates.Init(this.ddlStates);
        #endregion

        #region 加载Company列表

        this.BranchSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

        string sWhere = " and ContactBranchId=" + sContactBranchID;



        int iRowCount = LPWeb.DAL.DbHelperSQL.Count(this.BranchSqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);

        // empty data text
        if (iRowCount == 0 )
        {
            this.gridContactList.EmptyDataText = "There is no branch contact.";
        }
        else
        {
            this.gridContactList.EmptyDataText = "There is no partner branch.";
        }

        this.AspNetPager1.RecordCount = iRowCount;

        this.BranchSqlDataSource.SelectParameters["Where"].DefaultValue = sWhere;
        this.gridContactList.DataBind();

        #endregion
    }

    /// <summary>
    /// Save
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //check  if there is a duplicate branch name in the CompanyBranches
        if (_bContactBranches.IsExist_CreateBase(tbBranch.Text.Trim()))
        {
            PageCommon.WriteJsEnd(this, "duplication of this branch name.", PageCommon.Js_RefreshSelf);
            return;
        }
        //Load
        LPWeb.Model.ContactBranches _mContactBranches = new LPWeb.Model.ContactBranches();
        if (ddlCompany.SelectedValue.ToString() != "")
        {
            _mContactBranches.ContactCompanyId = Convert.ToInt32(ddlCompany.SelectedValue);
        }
        _mContactBranches.Name = tbBranch.Text.Trim();
        _mContactBranches.Phone = tbPhone.Text.Trim();
        _mContactBranches.State = ddlStates.SelectedValue.ToString();
        _mContactBranches.Zip = tbZip.Text.Trim();
        _mContactBranches.Fax = tbFax.Text.Trim();
        _mContactBranches.Enabled = chkEnable.Checked;
        _mContactBranches.Address = tbAddress.Text.Trim();
        _mContactBranches.City = tbCity.Text.Trim();
        if (ddlContact.SelectedValue.ToString() != "")
        {
            _mContactBranches.PrimaryContact = Convert.ToInt32(ddlContact.SelectedValue);
        }
        //Save
        _bContactBranches.Add(_mContactBranches);

        PageCommon.WriteJsEnd(this, "Create partner branch successfully.", "window.parent.location.href='PartnerBranchList.aspx';");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    { 
    
    }
}

