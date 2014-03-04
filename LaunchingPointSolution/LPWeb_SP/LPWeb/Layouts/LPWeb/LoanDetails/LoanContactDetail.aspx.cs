using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.BLL;
using Utilities;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;

public partial class LoanContactDetail : BasePage
{

    int iContactID = 0;
    Contacts contacts = new Contacts();
    LoginUser CurrentUser;
    int iContactRoleID = 0;
    //int iContactBranchId = 0;
    //int iContactCompanyId = 0;

    /// <summary>
    /// Gets or sets the current file id.
    /// </summary>
    /// <value>The current file id.</value>
    protected int CurrentFileId
    {
        set
        {
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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CurrentUser = new LoginUser();
            //loginUser.ValidatePageVisitPermission("LoanSetup");
            //not have Contact Modify Power
            if (CurrentUser.userRole.ContactMgmt.ToString().IndexOf('2')==-1)
            {
                Response.Redirect("../Unauthorize.aspx");
                return;
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

        if (Request.QueryString["FileID"] != null) // 如果有FileID
        {
            string sFileID = Request.QueryString["FileID"];

            if (PageCommon.IsID(sFileID) == false)
            {
                //PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
            }

            CurrentFileId = Convert.ToInt32(sFileID);
        }

        if (Request.QueryString["ContactID"] != null) // 如果有ContactID
        {
            string sContactID = Request.QueryString["ContactID"];
            sContactID = sContactID.Replace("Contract", "");
            string[] tmpIds = sContactID.Split("_".ToCharArray());
            if (tmpIds.Length == 2)
            {
                iContactID = Convert.ToInt32(tmpIds[0]);
                iContactRoleID = Convert.ToInt32(tmpIds[1]);
            }
            else
            {
                iContactID = 0;
                iContactRoleID = 0;
            }
        }
        else
        {
            iContactID = 0;
        }
        if (iContactID == 0)
        {
            return;
        }
        hdnContactID.Value = iContactID.ToString();

        if (!IsPostBack)
        {
            USStates.Init(ddlState);
            BindServiceType();
            BindContact();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void BindContact()
    {
        try
        {
            LPWeb.Model.Contacts model = contacts.GetModel(iContactID);
            if (model == null)
            {
                return;
            }
            //this.lblContactId.Text = model.ContactId.ToString();
            this.txbFirstName.Text = model.FirstName;
            this.txbLastName.Text = model.LastName;
            ckEnable.Checked = model.ContactEnable;
            //ddlServiceTypes.SelectedValue = ///???
            this.txbFax.Text = model.Fax;
            this.txbBPhone.Text = model.BusinessPhone;
            this.txbCPhone.Text = model.CellPhone;
            this.txbEmail.Text = model.Email;
            ddlState.SelectedValue = model.MailingState;
            txbCity.Text = model.MailingCity;
            txbAddress.Text = model.MailingAddr;
            txbZip.Text = model.MailingZip;

            if (model.ContactCompanyId.HasValue)
            {
                BindContactCompany(model.ContactCompanyId.Value);
                this.ddlCompany.SelectedValue = model.ContactCompanyId.ToString();
                BindContactBranch();
            }
            else if (model.ContactBranchId.HasValue)
            {
                BindContactBranch(model.ContactBranchId.Value);
                this.ddlBranch.SelectedValue = model.ContactBranchId.ToString();
            }
            else
                BindContactCompany();
        }
        catch
        {
        }
    }

    private void BindContactCompany()
    {
        try
        {
            ContactCompanies company = new ContactCompanies();
            LPWeb.Model.ContactCompanies model = new LPWeb.Model.ContactCompanies();
            DataSet ds = company.GetList(0, "", " [Name] asc");
            //DataSet ds = company.GetAllList();
            if (ds == null)
            {
                return;
            }

            ddlCompany.DataValueField = "ContactCompanyID";
            ddlCompany.DataTextField = "Name";
            ddlCompany.Items.Add(new ListItem("--select--", "0"));
            ddlCompany.SelectedIndex = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ddlCompany.Items.Add(new ListItem(dr["Name"].ToString(), dr["ContactCompanyID"].ToString()));
            }

        }
        catch
        { }
    }

    private void BindContactCompany(int ContactCompanyID)
    {
        try
        {
            BindContactCompany();
            ContactCompanies company = new ContactCompanies();
            LPWeb.Model.ContactCompanies model = new LPWeb.Model.ContactCompanies();
            model = company.GetModel(ContactCompanyID);
            if (model == null)
            {
                return;
            }

            ddlCompany.SelectedValue = ContactCompanyID.ToString();
            ddlServiceTypes.SelectedItem.Text = model.ServiceTypes;
            
        }
        catch
        { }
    }
    private void BindContactBranch()
    {
        try
        {
            ddlBranch.Items.Clear();
            ContactBranches branch = new ContactBranches();
            string strWhere = " Enabled=1 ";
            if (ddlCompany.SelectedValue != "0")
               strWhere += string.Format(" AND ContactCompanyID={0} ", ddlCompany.SelectedValue.Trim());
            DataSet ds = branch.GetList(0, strWhere, " [Name] asc");
            if (ds == null)
            {
                return;
            }
            ddlBranch.DataValueField = "ContactBranchID";
            ddlBranch.DataTextField = "Name";
            ddlBranch.Items.Add(new ListItem("--select--", "0"));
            ddlBranch.SelectedIndex = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ddlBranch.Items.Add(new ListItem(dr["Name"].ToString(), dr["ContactBranchID"].ToString()));
            }
        }
        catch
        { }
    }
    private void BindContactBranch(int ContactBranchId)
    {
        try
        {
            BindContactBranch();
            ContactBranches branch = new ContactBranches();
            LPWeb.Model.ContactBranches model = new LPWeb.Model.ContactBranches();
            model = branch.GetModel(ContactBranchId);
            if (model == null)
            {
                return;
            }

            ddlBranch.SelectedValue = ContactBranchId.ToString();
            BindContactCompany(model.ContactCompanyId.Value);
            ddlCompany.SelectedValue = model.ContactCompanyId.ToString();
        }
        catch
        { }
    }
    private void BindServiceType()
    {
        try
        {
            ServiceTypes type = new ServiceTypes();
            DataSet ds = type.GetList("Enabled=1");
            ddlServiceTypes.DataTextField = "ServiceType";
            ddlServiceTypes.DataValueField = "ServiceTypeId";
            ddlServiceTypes.Items.Add(new ListItem("-- select --", "0"));
            ddlServiceTypes.SelectedIndex = 0;
            if (ds == null || ds.Tables[0].Rows.Count == 0)
                return;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ddlServiceTypes.Items.Add(new ListItem(dr["ServiceType"].ToString(), dr["ServiceTypeId"].ToString()));
            }
        }
        catch
        { }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private LPWeb.Model.Contacts FillModel()
    {
        LPWeb.Model.Contacts model = new LPWeb.Model.Contacts();

        model.ContactId = int.Parse(hdnContactID.Value);
        model.FirstName = this.txbFirstName.Text.Trim();
        model.LastName = this.txbLastName.Text.Trim();
        model.ContactEnable = this.ckEnable.Enabled;
        //this.ckEnable.Checked = model.
        //model.ContactCompanyId =  
        //ddlServiceTypes.SelectedValue = ///???
        model.Fax = this.txbFax.Text.Trim();
        model.BusinessPhone = this.txbBPhone.Text.Trim();
        model.CellPhone = this.txbCPhone.Text.Trim();
        model.Email = this.txbEmail.Text.Trim();
        model.MailingState = ddlState.SelectedValue;

        model.MailingZip = txbZip.Text.Trim();
        model.MailingAddr = txbAddress.Text.Trim();
        model.MailingCity = txbCity.Text.Trim();
        if (ddlCompany.SelectedIndex == 0)
        {
            model.ContactCompanyId = 0;
        }
        else
        {
            model.ContactCompanyId = int.Parse(ddlCompany.SelectedItem.Value);
        }
        model.Enabled = ckEnable.Checked;
        return model;
    }
    protected void ddlServiceTypes_onSelectedIndexChanged(object sender, EventArgs e)
    {

        BindContactCompany();
    }
    protected void ddlCompany_onSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCompany.SelectedIndex == 0)
        {
            PageCommon.AlertMsg(this, "Please select a partner company.");
            return;
        }
        int companyId = Convert.ToInt32(ddlCompany.SelectedValue.Trim());
        LPWeb.Model.ContactCompanies model = new LPWeb.Model.ContactCompanies();

        LPWeb.BLL.ContactCompanies cCompany = new ContactCompanies();
        model = cCompany.GetModel(companyId);

        ddlState.SelectedValue = model.State.Trim();
        txbZip.Text = model.Zip.Trim();
        txbAddress.Text = model.Address.Trim();
        txbCity.Text = model.City.Trim();
        BindContactBranch();
    }

    protected void ddlBranch_onSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBranch.SelectedIndex == 0)
        {
            return;
        }
        int branchId = Convert.ToInt32(ddlBranch.SelectedValue.Trim());
        LPWeb.Model.ContactBranches model = new LPWeb.Model.ContactBranches();

        LPWeb.BLL.ContactBranches cBranch = new ContactBranches();
        model = cBranch.GetModel(branchId);

        ddlState.SelectedValue = model.State.Trim();
        txbZip.Text = model.Zip.Trim();
        txbAddress.Text = model.Address.Trim();
        txbCity.Text = model.City.Trim();
        txbFax.Text = model.Fax.Trim();
        txbBPhone.Text = model.Phone.Trim();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool CheckInput()
    {
        if (ddlBranch.SelectedIndex == 0 && ddlCompany.SelectedIndex == 0)
        {
            PageCommon.AlertMsg(this, "Please select a partner company.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!CheckInput())
        {
            return;
        }
        try 
        {
            contacts.UpdateContact(FillModel());
            PageCommon.WriteJs(this, "Updated Loan Contact successfully.", "window.parent.DialogContactEditClose(); window.parent.location.href=window.parent.location.href;");
        }
        catch
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string Contacts = "Contract" + iContactID.ToString() + "_" + iContactRoleID.ToString();
            LoanContacts lc = new LoanContacts();
            lc.DeleteLoanContacts(CurrentFileId, Contacts);
            PageCommon.WriteJs(this, "Deleted Loan Contact successfully.", "  window.parent.location.href=window.parent.location.href;");    
        }
        catch
        {
        }
    }
}