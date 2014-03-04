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


public partial class Contact_PartnerBranchEdit : BasePage
{
    protected string sContactBranchID;

    private ContactBranches _bContactBranches = new ContactBranches();
    private LoginUser _curLoginUser = null;
    Contacts bllContact = new Contacts();
    protected string sHasCreate = "0";
    protected string sHasModify = "0";
    protected string sHasDelete = "0";
    protected string sHasAssign = "0";
    protected string sHasView = "0";
    protected string sHasMerge = "0";
    protected string sHasAccessAllContacts = "0";

    protected void Page_Load(object sender, EventArgs e)
    {
        sContactBranchID = this.Request.QueryString["ContactBranchId"] != null ? this.Request.QueryString["ContactBranchId"].ToString() : "0";

        sHasCreate = CurrUser.userRole.ContactMgmt.ToString().IndexOf('1') > -1 ? "1" : "0";
        sHasModify = CurrUser.userRole.ContactMgmt.ToString().IndexOf('2') > -1 ? "1" : "0";
        sHasDelete = CurrUser.userRole.ContactMgmt.ToString().IndexOf('3') > -1 ? "1" : "0";
        sHasAssign = CurrUser.userRole.ContactMgmt.ToString().IndexOf('4') > -1 ? "1" : "0";
        sHasView = CurrUser.userRole.ContactMgmt.ToString().IndexOf('5') > -1 ? "1" : "0";
        sHasMerge = CurrUser.userRole.ContactMgmt.ToString().IndexOf('6') > -1 ? "1" : "0";
        sHasAccessAllContacts = CurrUser.userRole.AccessAllContacts == false ? "0" : "1";

        if (sHasDelete == "0")
        {
            btnDeleteContact.Enabled = false;
        }

        _curLoginUser = new LoginUser();

        if (!IsPostBack)
        {
            _curLoginUser = new LoginUser();
            InitControl();

            AssignContactUser();

            BindBranchInfo();

            BindContactInfo();
        }

    }

    /// <summary>
    /// Assign Contact
    /// </summary>
    private void AssignContactUser()
    {
        _bContactBranches = new ContactBranches();
        string sSelUserIDs = this.Request.QueryString["AssignUserIDs"] != null ? this.Request.QueryString["AssignUserIDs"].ToString() : "";
        var selctedStr = this.Request.QueryString["SelContactIDs"] != null ? this.Request.QueryString["SelContactIDs"].ToString() : "";
        if (sSelUserIDs != "" && selctedStr != "")
        {
            foreach (string ContactID in selctedStr.Split(','))
            {
                foreach (string sUserID in sSelUserIDs.Split(','))
                {
                    _bContactBranches.AssignUser2Contact(Convert.ToInt32(sUserID), Convert.ToInt32(ContactID));
                }
            }
        }
    }

    private void BindBranchInfo()
    {
        _bContactBranches = new ContactBranches();
        LPWeb.Model.ContactBranches mContactBranches = _bContactBranches.GetModel(Convert.ToInt32(sContactBranchID));

        this.ddlCompany.SelectedValue = Convert.ToString(mContactBranches.ContactCompanyId);
        this.tbBranch.Text = mContactBranches.Name;
        this.tbAddress.Text = mContactBranches.Address;
        this.tbCity.Text = mContactBranches.City;
        this.tbFax.Text = mContactBranches.Fax;
        this.tbPhone.Text = mContactBranches.Phone;
        this.tbZip.Text = mContactBranches.Zip;
        this.ddlContact.SelectedValue = Convert.ToString(mContactBranches.PrimaryContact);
        this.ddlStates.SelectedValue = mContactBranches.State;
        this.chkEnable.Checked = mContactBranches.Enabled;

    }

    private void InitControl()
    {
        #region 加载Primary Contact
        Contacts ContactsManager = new Contacts();

        _bContactBranches = new ContactBranches();
        int iContactBranchID = 0;
        int result = 0;
        if ( Int32.TryParse(sContactBranchID, out result))       
        {
            iContactBranchID = result;
        }
        LPWeb.Model.ContactBranches mContactBranches = _bContactBranches.GetModel(iContactBranchID);

        int iContactCompanyId = 0;

        if (mContactBranches != null && mContactBranches.ContactCompanyId != null) //bug #201
        {
            iContactCompanyId = (int)mContactBranches.ContactCompanyId;
        }

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
        ContactCompanies ContactCompaniesManager = new ContactCompanies();
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
        //}

        #endregion

        #region 加载States
        LPWeb.Layouts.LPWeb.Common.USStates.Init(this.ddlStates);
        #endregion

    }

    private void BindContactInfo()
    {
        #region 加载Contact列表

        Contacts ContactsManager = new Contacts();
        int st = ContactsManager.FormatCellPhone(sContactBranchID);

        this.BranchSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

        string sWhere = " and ContactBranchId=" + sContactBranchID;
        if (sHasAccessAllContacts == "0")
        {
            sWhere += " AND ContactId in (select ContactId from ContactUsers where UserID=" + CurrUser.iUserID + ") ";
        }


        int iRowCount = LPWeb.DAL.DbHelperSQL.Count(this.BranchSqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);

        // empty data text
        if (iRowCount == 0)
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
        _bContactBranches = new ContactBranches();
        //check  if there is a duplicate branch name in the CompanyBranches
        if (_bContactBranches.IsExist_EditBase(Convert.ToInt32(sContactBranchID),tbBranch.Text.Trim()))
        {
            PageCommon.WriteJsEnd(this, "duplication of this branch name.", PageCommon.Js_RefreshSelf);
            return;
        }
        //Load
        LPWeb.Model.ContactBranches _mContactBranches = new LPWeb.Model.ContactBranches();
        _mContactBranches.ContactBranchId = Convert.ToInt32(sContactBranchID);
        if (ddlCompany.SelectedValue.ToString() != "")
        {
            _mContactBranches.ContactCompanyId = Convert.ToInt32(ddlCompany.SelectedValue);
        }
        _mContactBranches.Name = tbBranch.Text.Trim();
        _mContactBranches.Phone = tbPhone.Text.Trim();

        string Phone = _mContactBranches.Phone;
        if ((Phone != null) && (Phone != string.Empty))
        {
            Phone = System.Text.RegularExpressions.Regex.Replace(Phone, @"[-() ]", String.Empty);

            if ((Phone.Length != 10) && (Phone.Length > 0))
            {
                PageCommon.WriteJsEnd(this, "Phone number must be 10 digits.", PageCommon.Js_RefreshSelf);
                return; 
            }
        }

        _mContactBranches.State = ddlStates.SelectedValue.ToString();
        _mContactBranches.Zip = tbZip.Text.Trim();
        _mContactBranches.Fax = tbFax.Text.Trim();
        string Fax = _mContactBranches.Fax;
        if ((Fax != null) && (Fax != string.Empty))
        {
            Fax = System.Text.RegularExpressions.Regex.Replace(Fax, @"[-() ]", String.Empty);

            if ((Fax.Length != 10) && (Fax.Length > 0))
            {
                PageCommon.WriteJsEnd(this, "Fax number must be 10 digits.", PageCommon.Js_RefreshSelf);
                return;
            }
        }
        _mContactBranches.Enabled = chkEnable.Checked;
        _mContactBranches.Address = tbAddress.Text.Trim();
        _mContactBranches.City = tbCity.Text.Trim();
        if (ddlContact.SelectedValue.ToString() != "")
        {
            _mContactBranches.PrimaryContact = Convert.ToInt32(ddlContact.SelectedValue);
        }
        //Save
        _bContactBranches.Update(_mContactBranches);

        PageCommon.WriteJsEnd(this, "Updated partner branch successfully.", "window.parent.location.href='PartnerBranchList.aspx';");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        _bContactBranches.RomoveBranch(Convert.ToInt32(sContactBranchID));
        PageCommon.WriteJsEnd(this, "Deleted partner branch successfully.", "window.parent.location.href='PartnerBranchList.aspx';");
    }

    /// <summary>
    /// Handles the Click event of the btnRemove control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnDeleteContact_Click(object sender, EventArgs e)
    {
        var selctedStr = this.hiSelectedContact.Value;
        if (!string.IsNullOrEmpty(selctedStr))
        {
            string[] selectedItems = selctedStr.Split(',');
            int UserType = 0;
            if (_curLoginUser.bIsCompanyExecutive)
            {
                UserType = 0;
            }
            else if (_curLoginUser.bIsBranchManager)
            {
                UserType = 1;
            }
            else
            {
                UserType = 2;
            }
            //delete the selected items
            DeleteContacts(selectedItems, UserType);
            //reload the grid data
            PageCommon.WriteJsEnd(this, "Contacts has been removed successfully.", PageCommon.Js_RefreshSelf);
        }
        this.hiSelectedContact.Value = "";
    }

    /// <summary>
    /// Deletes the loan programs.
    /// </summary>
    /// <param name="items">The items.</param>
    private void DeleteContacts(string[] items, int UserType)
    {
        int iItem = 0;
        foreach (var item in items)
        {
            if (int.TryParse(item, out iItem))
            {
                try
                {
                    bllContact.PartnerContactsDelete(iItem,  UserType, _curLoginUser.iUserID);
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }
            }
        }
    }

    /// <summary>
    /// Disable
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDisable_Click(object sender, EventArgs e)
    {
        var selctedStr = this.hiSelectedContact.Value;
        if (!string.IsNullOrEmpty(selctedStr))
        {
            string[] selectedItems = selctedStr.Split(',');
            //delete the selected items
            DisableContact(selectedItems);
            //reload the grid data
            PageCommon.WriteJsEnd(this, "Contact has been disabled successfully.", PageCommon.Js_RefreshSelf);
        }
        this.hiSelectedContact.Value = "";
    }

    private void DisableContact(string[] items)
    {
        int iItem = 0;
        foreach (var item in items)
        {
            if (int.TryParse(item, out iItem))
            {
                try
                {
                    _bContactBranches.DisableContact(iItem);
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }
            }
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e) 
    {
        string sContactIDs = this.hiSelectedContact.Value;
        _bContactBranches.RemoveContactSetBranchNull(sContactIDs);
        PageCommon.WriteJsEnd(this, "Removed parnter contact successfully.", "window.location.href=window.location.href;");
    }
}
