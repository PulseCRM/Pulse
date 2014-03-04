using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.Model;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;

public partial class Settings_CompanyGeneral : BasePage
{
    /// <summary>
    /// 
    /// </summary>
    private Company_General modCompanyGeneral = new Company_General();
    LPWeb.BLL.Company_General bllCompanyGeneral = new LPWeb.BLL.Company_General();
    private bool isNew = false;
    private int iOldGroupID = 0;
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            //权限验证
            var loginUser = new LoginUser();
            if (!loginUser.userRole.CompanySetup)
            {
                //若当前没有权限，则跳转到个人设定页面
                Response.Redirect("PersonalizationSettings.aspx");
                return;
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
        try
        {

            if (!Page.IsPostBack)
            {
                if (ddlState.Items.Count <= 0)
                    USStates.Init(ddlState);
                if (Request.UrlReferrer != null)
                {
                    this.btnCancel.PostBackUrl = Request.UrlReferrer.ToString();
                }

                GetInitData();
                InitGroupAccess();
                if (modCompanyGeneral != null)
                {
                    SetDataToUI();
                }
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

    }
    /// <summary>
    /// Gets the data from UI.
    /// </summary>
    private void GetDataFromUI()
    {
        //get existing record
        GetInitData();
        if (modCompanyGeneral == null)
        {
            modCompanyGeneral = new Company_General();
            isNew = true;
        }

        string ADOUFilterName = txtADOUFilterName.Text.Trim();
        if (!string.IsNullOrEmpty(ADOUFilterName))
        {
            modCompanyGeneral.AD_OU_Filter = ADOUFilterName;
        }

        string companyName = txtCompanyName.Text.Trim();
        if (!string.IsNullOrEmpty(companyName))
        {
            modCompanyGeneral.Name = companyName;
        }
        else
        {
            //todo:display message
            PageCommon.WriteJsEnd(this, "Company Name can not be empty.", PageCommon.Js_RefreshSelf);
            return;
        }

        string description = txtDescription.Text.Trim();
        if (!string.IsNullOrEmpty(description))
        {
            modCompanyGeneral.Desc = description;
        }

        string address = txtAddress.Text.Trim();
        if (!string.IsNullOrEmpty(address))
        {
            modCompanyGeneral.Address = address;
        }

        string city = txtCity.Text.Trim();
        if (!string.IsNullOrEmpty(city))
        {
            modCompanyGeneral.City = city;
        }

        string state = ddlState.SelectedValue;
        if (!string.IsNullOrEmpty(state))
        {
            modCompanyGeneral.State = state;
        }

        string zip = txtZip.Text.Trim();
        if (!string.IsNullOrEmpty(zip))
        {
            modCompanyGeneral.Zip = zip;
        }
        string interval = ddlInterval.SelectedValue;
        if (!string.IsNullOrEmpty(interval))
        {
            modCompanyGeneral.ImportUserInterval = interval.Parse<int>();
        }

        string phone = txtPhone.Text.Trim();
        if (!string.IsNullOrEmpty(phone))
        {
            modCompanyGeneral.Phone = phone;
        }

        string fax = txtFax.Text.Trim();
        if(!string.IsNullOrEmpty(fax))
        {
            modCompanyGeneral.Fax = fax;
        }

        string email = txtEmail.Text.Trim();
        if (!string.IsNullOrEmpty(email))
        {
            modCompanyGeneral.Email = email;
        }

        string weburl = txtWebURL.Text.Trim();
        if (!string.IsNullOrEmpty(weburl))
        {
            modCompanyGeneral.WebURL = weburl;
        }

        string strSecToken = this.txtSecToken.Text.Trim();
        if (string.IsNullOrEmpty(strSecToken))
            strSecToken = Guid.NewGuid().ToString();
        modCompanyGeneral.GlobalId = strSecToken;

        string sMyEmailInboxURL = this.txtMyEmailInboxUrl.Text.Trim();
        modCompanyGeneral.MyEmailInboxURL = sMyEmailInboxURL;

        string sMyCalendarURL = this.txtMyCalendarURL.Text.Trim();
        modCompanyGeneral.MyCalendarURL = sMyCalendarURL;

        string sRatesURL = this.txtRatesURL.Text.Trim();
        modCompanyGeneral.RatesURL = sRatesURL;
    }

    /// <summary>
    /// Gets the init data.
    /// </summary>
    private void GetInitData()
    {
        try
        {
            modCompanyGeneral = bllCompanyGeneral.GetModel();
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

    }
    /// <summary>
    /// Inits the group access.
    /// </summary>
    private void InitGroupAccess()
    {
        var bllGroup = new LPWeb.BLL.Groups();
        DataSet dsGroups = null;
        try
        {
            //get data from database
            dsGroups = bllGroup.GetCompanyRelGroups();
        }
        catch (Exception exception)
        {
            //log the error
            LPLog.LogMessage(exception.Message);
            return;
        }

        if (dsGroups != null && dsGroups.Tables.Count > 0 && dsGroups.Tables[0].Rows.Count > 0)
        {
            ddlGroupAccess.DataSource = dsGroups;
            ddlGroupAccess.DataTextField = "GroupName";
            ddlGroupAccess.DataValueField = "GroupId";
            ddlGroupAccess.DataBind();

            //set selected item
            foreach (DataRow dataRow in dsGroups.Tables[0].Select("CompanyID is not null and  OrganizationType='Company'"))
            {
                if (dataRow.IsNull("GroupId") == false)
                {
                    ViewState["previousSelectedIem"] = dataRow["GroupId"];
                    ddlGroupAccess.SelectedValue = dataRow["GroupId"].ToString();
                }
            }
        }

    }

    /// <summary>
    /// Sets the data to UI.
    /// </summary>
    private void SetDataToUI()
    {
        if (modCompanyGeneral == null)
        {
            return;
        }
        txtADOUFilterName.Text = modCompanyGeneral.AD_OU_Filter;
        txtCompanyName.Text = modCompanyGeneral.Name;
        txtDescription.Text = modCompanyGeneral.Desc;
        txtAddress.Text = modCompanyGeneral.Address;
        txtCity.Text = modCompanyGeneral.City;
        ddlState.SelectedValue = modCompanyGeneral.State;
        txtZip.Text = modCompanyGeneral.Zip;
        ddlInterval.SelectedValue = modCompanyGeneral.ImportUserInterval.ToString();

        txtFax.Text = modCompanyGeneral.Fax;
        txtPhone.Text = modCompanyGeneral.Phone;
        txtEmail.Text = modCompanyGeneral.Email;
        txtWebURL.Text = modCompanyGeneral.WebURL;
        this.txtSecToken.Text = modCompanyGeneral.GlobalId;

        this.txtMyEmailInboxUrl.Text = modCompanyGeneral.MyEmailInboxURL;
        this.txtMyCalendarURL.Text = modCompanyGeneral.MyCalendarURL;
        this.txtRatesURL.Text = modCompanyGeneral.RatesURL;
    }
    /// <summary>
    /// Saves this instance.
    /// </summary>
    /// <returns></returns>
    private bool Save()
    {
        bool status = false;
        try
        {
            if (isNew)
            {
                bllCompanyGeneral.Add(modCompanyGeneral);
            }
            else
            {
                //Get old group id (Rocky - 2011-1-30)
                LPWeb.BLL.Groups groupMgr = new LPWeb.BLL.Groups();
                LPWeb.Model.Groups model = groupMgr.GetCompanyGroup();
                this.iOldGroupID = model.GroupId;
                bllCompanyGeneral.Update(modCompanyGeneral);
            }


            status = true;
        }
        catch (Exception exception)
        {
            status = false;
            LPLog.LogMessage(exception.Message);
        }
        return status;
    }

    /// <summary>
    /// Handles the Click event of the btnSave control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            return;
        }
        GetDataFromUI();
        if (Save() == true)
        {
            SaveGroupAccess();
            string msg = "Saved successfully.";
            //msg = UpdateCompany();
            //todo:display successful message
            PageCommon.WriteJsEnd(this, msg, PageCommon.Js_RefreshSelf);
        }
        else
        {
            //todo:display faild message
            PageCommon.WriteJsEnd(this, "Failed to save the record.", PageCommon.Js_RefreshSelf);
        }

        SetDataToUI();
    }

    private string UpdateCompany()
    {
        string msg = "";
        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            UpdateCompanyRequest req = new UpdateCompanyRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            if (CurrUser != null)
            {
                req.hdr.UserId = this.CurrUser.iUserID;
            }

            UpdateCompanyResponse respone = null;
            try
            {
                respone = service.UpdateCompany(req);

                if (respone.hdr.Successful)
                {
                    msg = "Saved successfully.";
                    return msg;
                }
                else
                {
                    if (respone.hdr.StatusInfo == "")
                        respone.hdr.StatusInfo = " Marketing Manager is not running.";
                    msg = "Failed to update company, reason:" + respone.hdr.StatusInfo;
                    return msg;
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ee)
            {
                LPLog.LogMessage(ee.Message);
                msg = "Failed to update copmany , reason: Marketing Manager is not running.";
                return msg;
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
                msg = "Failed to update company, reason:" + exception.Message;
                return msg;
            }
        }

    }

    /// <summary>
    /// Saves the group access.
    /// </summary>
    /// <returns></returns>
    private bool SaveGroupAccess()
    {
        int prevId = 0;

        try
        {
            prevId = Convert.ToInt32(ViewState["previousSelectedIem"]);
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }


        LPWeb.BLL.Groups bllGroup = new LPWeb.BLL.Groups();
        int newId = 0;
        string strNewID = ddlGroupAccess.SelectedValue;
        if (int.TryParse(strNewID, out  newId))
        {
            int companyId = 5;//todo:check hard code
            string orgType = "Company";//todo:check hard code
            if (prevId == 0)
            {
                prevId = newId;
            }
            try
            {
                bllGroup.UpdateGroupAccess(prevId, newId, companyId, orgType);

                //Save group folder info
                LPWeb.BLL.GroupFolder groupFolder = new LPWeb.BLL.GroupFolder();
                if (newId != 0)
                {
                    groupFolder.DoSaveGroupFolder(newId, 5, "company", this.iOldGroupID);
                }

                return true;
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
        }
        return false;
    }

    /// <summary>
    /// Handles the Click event of the btnCancel control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    protected void btnImportUsers_Click(object sender, EventArgs e)
    {
        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            ImportADUsersRequest req = new ImportADUsersRequest();
            req.AD_OU_Filter = this.txtADOUFilterName.Text.Trim();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = 5;//todo:check dummy data

            ImportADUsersResponse respone = null;
            try
            {
                respone = service.ImportADUsers(req);

                if (respone.hdr.Successful)
                {
                    PageCommon.WriteJsEnd(this, "Imported AD Users successfully.", PageCommon.Js_RefreshSelf);
                }
                else
                {
                    PageCommon.WriteJsEnd(this, "Failed to import AD users, reason:" + respone.hdr.StatusInfo, PageCommon.Js_RefreshSelf);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ee)
            {
                LPLog.LogMessage(ee.Message);
                PageCommon.AlertMsg(this, "Failed to import AD Users, reason: User Manager is not running.");
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
                PageCommon.WriteJsEnd(this, "Failed to import AD users, reason:" + exception.Message, PageCommon.Js_RefreshSelf);
            }
        }
    }

    protected void btnSaveAs_Click(object sender, EventArgs e)
    {
    }
}