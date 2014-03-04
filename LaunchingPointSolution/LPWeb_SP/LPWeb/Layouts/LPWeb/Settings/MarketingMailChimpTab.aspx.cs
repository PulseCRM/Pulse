using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;

public partial class MarketingMailChimpTab : BasePage
{
    LPWeb.BLL.Branches bllB = new LPWeb.BLL.Branches();
    int iBranchID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.QueryString["BranchID"] != null)
        {
            string sBranchID = this.Request.QueryString["BranchID"].ToString();
            if (PageCommon.IsID(sBranchID) == false)
            {
                return;
            }

            this.iBranchID = Convert.ToInt32(sBranchID);
        }
        else
        {
            return;
        }

        if (iBranchID == 0)
        {
            return;
        }

        if (!IsPostBack)
        {
            FillMailChiiBranchIDmp();
        }
    }

    private void FillMailChiiBranchIDmp()
    {
        LPWeb.Model.Branches model = new LPWeb.Model.Branches();
        model = bllB.GetModel(iBranchID);

        cbEnableMailChimp.Checked = model.EnableMailChimp;

        txbMCKey.Text = model.MailChimpAPIKey;

        if (cbEnableMailChimp.Checked)
        {
            txbMCKey.Enabled = true;

            if (!string.IsNullOrEmpty(model.MailChimpAPIKey))
            {
                btnSync.Disabled = false;
            }
            else
            {
                btnSync.Disabled = true;
            }
        }
        else
        {
            txbMCKey.Enabled = false;
            btnSync.Disabled = true;
        }
    }

    protected void btnSaveMailChimp_Click(object sender, EventArgs e)
    {
        try
        {
            if (iBranchID == 0)
            {
                return;
            }

            LPWeb.Model.Branches model = new LPWeb.Model.Branches();
            model.BranchId = iBranchID;
            model.EnableMailChimp = cbEnableMailChimp.Checked;
            model.MailChimpAPIKey = txbMCKey.Text.Trim();

            bllB.UpdateChimpAPIKey(model);
            FillMailChiiBranchIDmp();
            PageCommon.AlertMsg(this, "Save successfully.");
        }
        catch(Exception ex)
        {
            LPLog.LogMessage(ex.Message); 
            PageCommon.AlertMsg(this, "Failed to save the record ,Error:"+ex.Message);
        }
    }

}


