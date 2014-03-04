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


public partial class LoanContactAdd : BasePage
{

    int iContactID = 0;
    Contacts contacts = new Contacts();
    LoginUser CurrentUser;

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
            if (CurrentUser.userRole.ContactMgmt.ToString().IndexOf('1')==-1)
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


        if (!IsPostBack)
        {
            USStates.Init(ddlState);
            BindServiceType();
        }
    }

    private void BindContactCompany()
    {
        try
        {
            ddlCompany.Items.Clear();
            ContactCompanies company = new ContactCompanies();
            DataSet ds = company.GetList(0, " Enabled=1 and ServiceTypeId="+ddlServiceTypes.SelectedValue.Trim(), " [Name] asc");
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
    private void BindContactBranch()
    {
        try
        {
            ddlBranch.Items.Clear();
            ContactBranches branch = new ContactBranches();
            DataSet ds = branch.GetList(0, " Enabled=1 and ContactCompanyID="+ddlCompany.SelectedValue.Trim(), " [Name] asc");
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

        model.ContactId = 0;
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
        if (hdnCompany.Value == "0")
        {
            model.ContactCompanyId = 0;
        }
        else
        {
            model.ContactCompanyId = int.Parse(ddlCompany.SelectedItem.Value);
        }
        if (hdnBranch.Value == "0")
        {
            model.ContactBranchId = 0;
        }
        else
        {
            model.ContactBranchId = int.Parse(ddlBranch.SelectedItem.Value);
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
            int ContactId = contacts.Add(FillModel());
            if (ContactId > 0 && (ddlCompany.SelectedIndex > 0 || ddlBranch.SelectedIndex > 0))
            {
                int cCompanyId = ddlBranch.SelectedIndex > 0 ? 0 : Convert.ToInt32(ddlCompany.SelectedValue);
                int cBranchId = ddlBranch.SelectedIndex > 0 ? Convert.ToInt32(ddlBranch.SelectedValue) : 0;
                string sqlCmd = string.Format("Update Contacts set ContactBranchId={0}, ContactCompanyId={1} where ContactId={2}",
                    cBranchId > 0 ? cBranchId.ToString() : "NULL", cBranchId > 0 ? "NULL" : cCompanyId.ToString(), ContactId);

                LPWeb.DAL.DbHelperSQL.ExecuteSql(sqlCmd);
            }
            //contacts.ADDContact(FillModel());
            PageCommon.WriteJs(this, "Added Loan Contact successfully.", "window.parent.location.href=window.parent.location.href; window.parent.DialogContactAddClose(); ");
        }
        catch (Exception ex)
        {
            PageCommon.WriteJs(this, "Failed to save the contact record.", "window.parent.location.href=window.parent.location.href; window.parent.DialogContactAddClose(); ");
            return;
        }
    }

}
