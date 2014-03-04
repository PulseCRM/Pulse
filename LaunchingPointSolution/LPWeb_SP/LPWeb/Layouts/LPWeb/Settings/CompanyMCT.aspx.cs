using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.Model;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;

public partial class Settings_CompanyMCT : BasePage
{
    LPWeb.Model.Company_MCT mctMode = new LPWeb.Model.Company_MCT();
    LPWeb.BLL.Company_MCT mctMgr = new LPWeb.BLL.Company_MCT();

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            //权限验证
            var loginUser = new LoginUser();
            if (!loginUser.userRole.CompanySetup)
            {
                Response.Redirect("../Unauthorize.aspx");
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
                if (Request.UrlReferrer != null)
                {
                    this.btnCancel.PostBackUrl = Request.UrlReferrer.ToString();
                }

                GetInitData();
                if (mctMode != null)
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
    /// Save button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        GetDataFromUI();
        if (Save() == true)
        {
            //display successful message
            PageCommon.WriteJsEnd(this, "Saved successfully.", PageCommon.Js_RefreshSelf);

        }
        else
        {
            //display faild message 
            PageCommon.WriteJsEnd(this, "Failed to save the record.", PageCommon.Js_RefreshSelf);
        }
        SetDataToUI();
    }
    #endregion


    #region Functions

    /// <summary>
    /// Gets the init data.
    /// </summary>
    private void GetInitData()
    {
        try
        {
            mctMode = this.mctMgr.GetModel();
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
    }

    /// <summary>
    /// Sets the data to UI.
    /// </summary>
    private void SetDataToUI()
    {
        if (mctMode == null)
        {
            return;
        }

        tbClientID.Text = mctMode.ClientID;
        if (mctMode.PostDataEnabled)
        {
            this.chkEnablePostData.Checked = true;
        }
        else
        {
            this.chkEnablePostData.Checked = false;
        }
        tbMCTPostURL.Text = mctMode.PostURL;
        if (mctMode.ActiveLoanInterval > 0)
        {
            ddlActiveLoanInterval.SelectedValue = mctMode.ActiveLoanInterval.ToString();
        }
        else
        {
            ddlActiveLoanInterval.SelectedValue = "30";
        }
        if (mctMode.ArchivedLoanDisposeMonth > 0)
        {
            ddlArchivedLoanDisposeMonth.SelectedValue = mctMode.ArchivedLoanDisposeMonth.ToString();
        }
        else
        {
            ddlArchivedLoanDisposeMonth.SelectedValue = "3";
        }
        if (mctMode.ArchivedLoanInterval > 0)
        {
            ddlArchivedLoanInterval.SelectedValue = mctMode.ArchivedLoanInterval.ToString();
        }
        else
        {
            ddlArchivedLoanInterval.SelectedValue = "24";
        }

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
            mctMgr.Save(mctMode);

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
    /// Gets the data from UI.
    /// </summary>
    private void GetDataFromUI()
    {
        //get existing record
        GetInitData();
        if (mctMode == null)
        {
            mctMode = new Company_MCT();
        }
        mctMode.ClientID = tbClientID.Text.Trim();
        if (chkEnablePostData.Checked)
        {
            mctMode.PostDataEnabled = true;
        }
        else
        {
            mctMode.PostDataEnabled = false;
        }
        if (mctMode.PostDataEnabled == true)
        {
            mctMode.PostURL = tbMCTPostURL.Text.Trim();
            if (this.ddlActiveLoanInterval.SelectedIndex >= 0)
            {
                mctMode.ActiveLoanInterval = Convert.ToInt32(this.ddlActiveLoanInterval.SelectedValue);
            }
            else
            {
                mctMode.ActiveLoanInterval = 0;
            }
            if (this.ddlArchivedLoanDisposeMonth.SelectedIndex >= 0)
            {
                mctMode.ArchivedLoanDisposeMonth = Convert.ToInt32(this.ddlArchivedLoanDisposeMonth.SelectedValue);
            }
            else
            {
                mctMode.ArchivedLoanDisposeMonth = 0;
            }
            if (this.ddlArchivedLoanInterval.SelectedIndex >= 0)
            {
                mctMode.ArchivedLoanInterval = Convert.ToInt32(this.ddlArchivedLoanInterval.SelectedValue);
            }
            else
            {
                mctMode.ArchivedLoanInterval = 0;
            }
        }
        else
        {
            mctMode.PostURL = "";
            mctMode.ActiveLoanInterval = 0;
            mctMode.ArchivedLoanDisposeMonth = 0;
            mctMode.ArchivedLoanInterval = 0;
        }
        
    }
    #endregion
}
