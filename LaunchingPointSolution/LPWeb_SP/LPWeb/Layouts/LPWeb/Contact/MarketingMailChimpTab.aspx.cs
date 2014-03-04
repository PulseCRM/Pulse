using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication3.forms
{
    public partial class MarketingMailChimpTab : System.Web.UI.Page
    {
        LPWeb.BLL.Branches bllB = new LPWeb.BLL.Branches();
        int BranchID = 1;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void FillControl()
        {
            LPWeb.Model.Branches model = new LPWeb.Model.Branches();
            model = bllB.GetModel(BranchID);

            cbEnableMailChimp.Checked = model.EnableMailChimp;

            txbMCKey.Text = model.MailChimpAPIKey;

            if (cbEnableMailChimp.Checked)
            {
                txbMCKey.Enabled = true;
            }
            else
            {
                txbMCKey.Enabled = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LPWeb.Model.Branches model = new LPWeb.Model.Branches();
                model.BranchId = BranchID;
                model.EnableMailChimp = cbEnableMailChimp.Checked;
                model.MailChimpAPIKey = txbMCKey.Text.Trim();

                bllB.UpdateChimpAPIKey(model);
            }
            catch
            {
 
            }
        }

        protected void btnSync_Click(object sender, EventArgs e)
        {

        }
    }
}
