using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using LPWeb.Common;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;

public partial class PartnerContactsSetupPopup : BasePage
{
    #region params
    private bool isReset = false;
    const string alphabets = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
    public string FromURL = string.Empty;
    private Contacts contactMgr = new Contacts();
    private string Alphabet = string.Empty;
    private int PageIndex = 1;
    private int ContactId = 0;
    string sCloseDialogCodes = string.Empty;
    protected string sHasAccessAllContacts = "0";
    string sRefreshCodes = string.Empty;
    string sRefreshTabCodes = string.Empty;
    #endregion

    #region Event
    /// <summary>
    /// Page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //权限验证
            var loginUser = new LoginUser();
            if (loginUser.userRole.ContactMgmt.ToString() == "")
            {
                Response.Redirect("../Unauthorize.aspx");
                return;
            }
            sHasAccessAllContacts = loginUser.userRole.AccessAllContacts == false ? "0" : "1";
            // CloseDialogCodes
            bool bIsValid = PageCommon.ValidateQueryString(this, "CloseDialogCodes", QueryStringType.String);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, "Missing required query string.", "window.parent.location.href=window.parent.location.href");
            }
            this.sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";

            // RefreshCodes
            bIsValid = PageCommon.ValidateQueryString(this, "RefreshCodes", QueryStringType.String);
            if (bIsValid == true)
            {
                this.sRefreshCodes = this.Request.QueryString["RefreshCodes"].ToString() + ";";
            }
            if (PageCommon.ValidateQueryString(this, "RefreshCodes", QueryStringType.String))
            {
                this.sRefreshTabCodes = this.Request.QueryString["RefreshTab"].ToString().Trim() == "" ? "" : (this.Request.QueryString["RefreshTab"].ToString().Trim() + ";");
            }
            PageLoad();
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
    }

    /// <summary>
    /// Save button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.FileUpload1.HasFile)
            {
                string strMsg = "";
                bool bIsValid = PageCommon.ValidateUpload(this, this.FileUpload1, 1024 * 1024 * 15, out strMsg, ".jpg", ".bmp", ".png", ".gif");
                if (!bIsValid)
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed", string.Format("alert('{0}');", strMsg), true);
                    return;
                }
            }
            //Save contact info
            this.SaveContact();

            if (this.ContactId != 0)
            {
                this.SaveContactUser();
            }
        }
        catch (Exception ex)
        {
            LPLog.LogMessage(ex.Message);

            // Faild
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed", "window.parent.location.href=window.parent.location.href;$('#divContainer').hide();alert('Save contact Failed.');" + this.sCloseDialogCodes, true);
            return;
        }


        // success
        if (this.sRefreshCodes != string.Empty && this.sRefreshCodes != "")
        {
            PageCommon.WriteJsEnd(this, "Saved contact successfully.", this.sRefreshCodes + this.sRefreshTabCodes + this.sCloseDialogCodes);
        }
        else
        {
            PageCommon.WriteJsEnd(this, "Saved contact successfully.", "window.parent.location.href=window.parent.location.href;" + this.sCloseDialogCodes);
        }
        //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "window.parent.location.href=window.parent.location.href;$('#divContainer').hide();alert('Save contact successfully.');" + this.sCloseDialogCodes, true);
    }

    /// <summary>
    /// Alphabets selected changed event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAlphabets_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.Alphabet = this.ddlAlphabets.SelectedValue;
            this.BindUserGrid();
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
    }
    #endregion

    #region Function
    /// <summary>
    /// Page load
    /// </summary>
    private void PageLoad()
    {
        try
        {
            if (Request.Url != null)
            {
                FromURL = Request.Url.ToString();
            }

            ContactId = 0;
            if (this.Request.QueryString["ContactID"] != null) // PageIndex
            {

                ContactId = int.Parse(this.Request.QueryString["ContactID"].ToString());
            }
            else
            {
                if (hdnContactID.Value.Length > 0)
                {
                    ContactId = int.Parse(hdnContactID.Value);
                }
            }

            if (!IsPostBack)
            {
                this.BindPageDefaultValues();

                this.BindUserGrid();

                this.BindControls();
            }

            hdnContactID.Value = ContactId.ToString();

            if (this.Request.QueryString["Alphabet"] != null) // 如果有Alphabet
            {
                Alphabet = this.Request.QueryString["Alphabet"].ToString();
            }
            else
            {
                if (!string.IsNullOrEmpty(ddlAlphabets.SelectedValue))
                    Alphabet = ddlAlphabets.SelectedValue.Trim();
            }
            ddlAlphabets.SelectedValue = Alphabet;
            PageIndex = 1;
            if (this.Request.QueryString["PageIndex"] != null) // PageIndex
            {
                PageIndex = int.Parse(this.Request.QueryString["PageIndex"].ToString());
            }
            else
            {
                PageIndex = AspNetPager1.CurrentPageIndex;
            }

            if (this.Request.QueryString["FromPage"] != null) // FromPage
            {
                hdnPageFrom.Value = this.Request.QueryString["FromPage"];
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Load contact details info
    /// </summary>
    private void BindControls()
    {
        if (ContactId == 0)
        {
            this.imgPhoto.ImageUrl = string.Format("~/_layouts/LPWeb/Settings/GetContactPicture.aspx?t={0}", DateTime.Now.Ticks);
            return;
        }
        try
        {
            LPWeb.Model.Contacts model = new LPWeb.Model.Contacts();
            model = this.contactMgr.GetModel(ContactId);
            if (model == null)
            {
                return;
            }

            this.ddlBranch.SelectedValue = model.ContactBranchId.ToString();
            this.txtFirstName.Text = model.FirstName.Trim();
            this.txtLastName.Text = model.LastName.Trim();
            this.chkEnabled.Checked = model.Enabled;
            this.txtAddress.Text = model.MailingAddr.Trim();
            this.ddlStates.SelectedValue = model.MailingState;
            this.txtCity.Text = model.MailingCity.Trim();
            this.txtZip.Text = model.MailingZip.Trim();
            this.txtBizPhone.Text = model.BusinessPhone.Trim();
            this.txtCellPhone.Text = model.CellPhone.Trim();
            this.txtEmail.Text = model.Email.Trim();
            this.txtFax.Text = model.Fax.Trim();
            this.txtSignature.Text = model.Signature;
            if (model.Picture == null)
            {
                this.imgPhoto.ImageUrl = string.Format("~/_layouts/LPWeb/Settings/GetContactPicture.aspx?t={0}", DateTime.Now.Ticks);
            }
            else
            {
                this.imgPhoto.ImageUrl = string.Format("~/_layouts/LPWeb/Settings/GetContactPicture.aspx?cid={0}&t={1}", ContactId, DateTime.Now.Ticks);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    /// <summary>
    /// Bind Page default values
    /// </summary>
    private void BindPageDefaultValues()
    {
        try
        {
            //Bind Alphabet
            foreach (string alphabet in alphabets.Split(','))
            {
                ddlAlphabets.Items.Add(new ListItem(alphabet, alphabet));
            }

            //Bind State
            LPWeb.Layouts.LPWeb.Common.USStates.Init(this.ddlStates);

            //Bind Branch
            ContactBranches contactBranch = new ContactBranches();
            DataSet dsCBranch = contactBranch.GetList(" Enabled='true' Order by Name asc");
            DataRow drNew = dsCBranch.Tables[0].NewRow();
            drNew["ContactBranchId"] = 0;
            drNew["Name"] = "-- Select Branch --";
            dsCBranch.Tables[0].Rows.InsertAt(drNew, 0);
            this.ddlBranch.DataSource = dsCBranch.Tables[0];
            this.ddlBranch.DataValueField = "ContactBranchId";
            this.ddlBranch.DataTextField = "Name";
            this.ddlBranch.DataBind();

            //this.ddlBranch.SelectedValue = CurrUser.

            // autocomplete address
            this.ddlBranchAddress.DataSource = dsCBranch.Tables[0];
            this.ddlBranchAddress.DataBind();
            List<string> s = new List<string>();
            

            if (this.ContactId == 0)
            {
                this.btnDelete.Disabled = true;
            }
            else
            {
                this.btnDelete.Disabled = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// set filter condition
    /// </summary>
    /// <returns></returns>
    private string GenerateQueryCondition()
    {
        string queryCon = " AND ContactId = " + ContactId.ToString();

        if (!string.IsNullOrEmpty(Alphabet))
            queryCon += string.Format(" AND FullName Like '{0}%' ", Alphabet);

        return queryCon;
    }

    /// <summary>
    /// Bind Grid
    /// </summary>
    private void BindUserGrid()
    {
        int pageSize = AspNetPager1.PageSize;
        int pageIndex = PageIndex;
        string queryCondition = GenerateQueryCondition();

        int recordCount = 0;

        DataSet dsUserLists = null;
        try
        {
            LPWeb.BLL.Users userMgr = new LPWeb.BLL.Users();
            dsUserLists = userMgr.GetContactUserByContactID(pageSize, pageIndex, queryCondition, out recordCount, "FullName", 0);

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            this.gvUserList.DataSource = null;
            gvUserList.DataBind();
            gvUserList.DataSource = dsUserLists;
            gvUserList.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    /// <summary>
    /// Fill contact model
    /// </summary>
    /// <returns></returns>
    public LPWeb.Model.Contacts FillModel()
    {
        LPWeb.Model.Contacts model = new LPWeb.Model.Contacts();
        model.ContactId = this.ContactId;
        if (this.ddlBranch.SelectedIndex > 0)
        {
            model.ContactBranchId = int.Parse(this.ddlBranch.SelectedValue);
            LPWeb.BLL.ContactBranches contactBranch = new ContactBranches();
            LPWeb.Model.ContactBranches branchModel = contactBranch.GetModel(model.ContactBranchId.Value);
            if (branchModel != null)
            {
                model.ContactCompanyId = branchModel.ContactCompanyId;
            }
        }
        model.FirstName = this.txtFirstName.Text.Trim();
        model.LastName = this.txtLastName.Text.Trim();
        model.Enabled = this.chkEnabled.Checked;
        model.ContactEnable = this.chkEnabled.Enabled;
        model.MailingAddr = this.txtAddress.Text.Trim();
        model.MailingCity = this.txtCity.Text.Trim();
        if (this.ddlStates.SelectedIndex >= 1)
        {
            model.MailingState = this.ddlStates.SelectedValue;
        }
        model.MailingZip = this.txtZip.Text.Trim();
        model.CellPhone = this.txtCellPhone.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "");
        string CellPhone = model.CellPhone;
        if ((CellPhone != null) && (CellPhone != string.Empty))
        {
            CellPhone = System.Text.RegularExpressions.Regex.Replace(CellPhone, @"[-() ]", String.Empty);

            if ((CellPhone.Length != 10) && (CellPhone.Length > 0))
            {
                PageCommon.WriteJsEnd(this, "Cell phone number must be 10 digits.", PageCommon.Js_RefreshSelf);
                return null;
            }
        }
        model.BusinessPhone = this.txtBizPhone.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "");
        string BusinessPhone = model.BusinessPhone;
        if ((BusinessPhone != null) && (BusinessPhone != string.Empty))
        {
            BusinessPhone = System.Text.RegularExpressions.Regex.Replace(BusinessPhone, @"[-() ]", String.Empty);

            if ((BusinessPhone.Length != 10) && (BusinessPhone.Length > 0))
            {
                PageCommon.WriteJsEnd(this, "Business phone number must be 10 digits.", PageCommon.Js_RefreshSelf);
                return null;
            }
        }
        model.Email = this.txtEmail.Text.Trim();
        model.Fax = this.txtFax.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "");
        string Fax = model.Fax;
        if ((Fax != null) && (Fax != string.Empty))
        {
            Fax = System.Text.RegularExpressions.Regex.Replace(Fax, @"[-() ]", String.Empty);

            if ((Fax.Length != 10) && (Fax.Length > 0))
            {
                PageCommon.WriteJsEnd(this, "Fax number must be 10 digits.", PageCommon.Js_RefreshSelf);
                return null;
            }
        }
        if (this.FileUpload1.PostedFile.ContentLength > 0)
        {
            byte[] ImageData = new byte[this.FileUpload1.PostedFile.ContentLength];
            this.FileUpload1.PostedFile.InputStream.Read(ImageData, 0, this.FileUpload1.PostedFile.ContentLength);
            model.Picture = ImageData;
        }
        string sMySignature = this.txtSignature.Text.Trim();
        model.Signature = sMySignature;

        return model;
    }

    /// <summary>
    /// Save contact
    /// </summary>
    private void SaveContact()
    {
        try
        {
            LPWeb.Model.Contacts contactModel = this.FillModel();
            if (contactModel == null)
            {
                this.ContactId = 0;
                return;
            }

            if (this.ContactId == 0)
            {
                DataSet dsContact = this.contactMgr.GetList(" LastName='" + contactModel.LastName.Replace("'", "''") + "' AND FirstName='" + contactModel.FirstName.Replace("'", "''") + "' AND Email = '" + contactModel.Email.Replace("'", "''") + "' AND ContactBranchId = " + contactModel.ContactBranchId.Value.ToString());
                if (dsContact.Tables.Count > 0 && dsContact.Tables[0].Rows.Count > 0)
                {
                    this.ContactId = int.Parse(dsContact.Tables[0].Rows[0]["ContactId"].ToString());
                }
            }

            if (this.ContactId == 0)
            {
                contactModel.Created = DateTime.Now;
                contactModel.CreatedBy = CurrUser.iUserID;
                this.ContactId = this.contactMgr.Add(contactModel);
            }
            else
            {
                this.contactMgr.Update(contactModel);
            }
            this.hdnContactID.Value = this.ContactId.ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Save contact users
    /// </summary>
    private void SaveContactUser()
    {
        if(this.ContactId == 0)
        {
            return;
        }
        try
        {
            LPWeb.BLL.ContactUsers contactUsers = new LPWeb.BLL.ContactUsers();

            //Remove contact user
            string sRemoveUserIDs = this.hdnRemoveUserID.Value;
            string[] removeUserIDArray = sRemoveUserIDs.Split(',');

            foreach (string sUserID in removeUserIDArray)
            {
                int iUserID = 0;
                if(!Int32.TryParse(sUserID,out iUserID))
                {
                    continue;
                }
                contactUsers.Delete(iUserID, this.ContactId);
            }

            //Add contact user

            string sAddUserIDs = this.hdnContactUserID.Value;
            string[] AddUserIDArray = sAddUserIDs.Split(',');
            foreach (string sUserID in AddUserIDArray)
            {
                int iUserID = 0;
                if (!Int32.TryParse(sUserID, out iUserID))
                {
                    continue;
                }

                LPWeb.Model.ContactUsers userModel = contactUsers.GetModel(iUserID, this.ContactId);
                if (userModel != null)
                {
                    contactUsers.Update(userModel);
                }
                else
                {
                    userModel = new LPWeb.Model.ContactUsers();
                    userModel.UserId = iUserID;
                    userModel.ContactId = this.ContactId;
                    userModel.Enabled = true;
                    userModel.Created = DateTime.Now;
                    contactUsers.Add(userModel);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
}

