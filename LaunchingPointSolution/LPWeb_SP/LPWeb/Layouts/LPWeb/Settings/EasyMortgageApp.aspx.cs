using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.Model;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;

public partial class Settings_EasyMortgageApp : BasePage
{
    LPWeb.BLL.Company_General bllCompanyGeneral = new LPWeb.BLL.Company_General();
    LPWeb.BLL.Company_EasyMortgage bllCompanyEasyMortgage = new LPWeb.BLL.Company_EasyMortgage();
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

        if (!IsPostBack)
        {
            var compMode = bllCompanyGeneral.GetModel();

            if (compMode != null && !string.IsNullOrEmpty(compMode.GlobalId))
            {
                txtSecurityToken.Text = compMode.GlobalId.ToString();
            }


            var compapp = bllCompanyEasyMortgage.GetModel();

            if (compapp != null)
            {
                ckEnable.Checked = compapp.Enabled;
                txbSyncIntervalHours.Text = compapp.SyncIntervalHours == null ? "24" : compapp.SyncIntervalHours.ToString();
                txtURL.Text = compapp.URL.ToString();
                txtClientID.Text = compapp.ClientID.ToString();
            }

        }


    }


    protected void btnSave_Click(object sender, EventArgs e)
    {

        int SyncIntervalHours = 24;
        try
        {
            SyncIntervalHours = Convert.ToInt32(txbSyncIntervalHours.Text.Trim());
        }
        catch
        {
            PageCommon.AlertMsg(this, "Send data every must be number!");
            return;
        }
        LPWeb.Model.Company_EasyMortgage compapp = new  LPWeb.Model.Company_EasyMortgage();

        compapp.Enabled = ckEnable.Checked;
        compapp.SyncIntervalHours = SyncIntervalHours;
        compapp.URL = txtURL.Text.Trim();
        compapp.ClientID = txtClientID.Text.Trim();

        if (bllCompanyEasyMortgage.Exists())
        {
            bllCompanyEasyMortgage.Update(compapp);
        }
        else
        {
            bllCompanyEasyMortgage.Add(compapp);
        }

        PageCommon.AlertMsg(this, "Save Successful!");


    }

}

