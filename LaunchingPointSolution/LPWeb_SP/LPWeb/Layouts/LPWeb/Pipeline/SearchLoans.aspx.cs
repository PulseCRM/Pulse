using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    /// <summary>
    /// Search Loans
    /// Author: Peter
    /// Date: 2011-02-20
    /// </summary>
    public partial class SearchLoans : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sType = Request.QueryString["type"];
            if (sType == "active")
            {
                this.trStatus.Attributes.Add("style", "display:none");
            }
            else if (sType == "archived")
            {
                lbStatus.Text = "Archived";

                ddlLoanStatus.Items.Add(new ListItem("All", "-1"));
                ddlLoanStatus.Items.Add(new ListItem("Canceled", "Canceled"));
                ddlLoanStatus.Items.Add(new ListItem("Closed", "Closed"));
                ddlLoanStatus.Items.Add(new ListItem("Denied", "Denied"));
                ddlLoanStatus.Items.Add(new ListItem("Suspended", "Suspended"));
            }
            else
            {
                ddlLoanStatus.Items.Add(new ListItem("All", "-1"));
                ddlLoanStatus.Items.Add(new ListItem("Active", "Active"));
                ddlLoanStatus.Items.Add(new ListItem("Bad", "Bad"));
                ddlLoanStatus.Items.Add(new ListItem("Canceled", "Canceled"));
                ddlLoanStatus.Items.Add(new ListItem("Converted", "Converted"));
                ddlLoanStatus.Items.Add(new ListItem("Lost", "Lost"));
                ddlLoanStatus.Items.Add(new ListItem("Suspended", "Suspended"));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder sbReturn = new StringBuilder();
            if (!string.IsNullOrEmpty(this.tbLastName.Text))
            {
                if (sbReturn.Length > 0)
                    sbReturn.Append('\u0001');
                sbReturn.AppendFormat("lname{0}{1}", '\u0002', this.tbLastName.Text);
            }
            if (!string.IsNullOrEmpty(this.tbFirstName.Text))
            {
                if (sbReturn.Length > 0)
                    sbReturn.Append('\u0001');
                sbReturn.AppendFormat("fname{0}{1}", '\u0002', this.tbFirstName.Text);
            }
            if (this.ddlLoanStatus.SelectedIndex > 0)
            {
                if (sbReturn.Length > 0)
                    sbReturn.Append('\u0001');
                sbReturn.AppendFormat("status{0}{1}", '\u0002', this.ddlLoanStatus.SelectedValue);
            }
            if (!string.IsNullOrEmpty(this.tbProAddr.Text))
            {
                if (sbReturn.Length > 0)
                    sbReturn.Append('\u0001');
                sbReturn.AppendFormat("addr{0}{1}", '\u0002', this.tbProAddr.Text);
            }
            if (!string.IsNullOrEmpty(this.tbCity.Text))
            {
                if (sbReturn.Length > 0)
                    sbReturn.Append('\u0001');
                sbReturn.AppendFormat("city{0}{1}", '\u0002', this.tbCity.Text);
            }
            if (this.ddlState.SelectedIndex > 0)
            {
                if (sbReturn.Length > 0)
                    sbReturn.Append('\u0001');
                sbReturn.AppendFormat("state{0}{1}", '\u0002', this.ddlState.SelectedValue);
            }
            if (!string.IsNullOrEmpty(this.tbZip.Text))
            {
                if (sbReturn.Length > 0)
                    sbReturn.Append('\u0001');
                sbReturn.AppendFormat("zip{0}{1}", '\u0002', this.tbZip.Text);
            }
            ClientFun("callback", string.Format("callBack('{0}');", Server.HtmlEncode(sbReturn.ToString())));
        }

        /// <summary>
        /// Call client function
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strScript"></param>
        private void ClientFun(string strKey, string strScript)
        {
            ClientScriptManager csm = this.Page.ClientScript;
            if (!csm.IsStartupScriptRegistered(this.GetType(), strKey))
            {
                csm.RegisterStartupScript(this.GetType(), strKey, strScript, true);
            }
        }
    }
}
