using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.WPPage
{
    public partial class Rates : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SPWeb web = this.Web;
            SPList spList = web.Lists[System.Configuration.ConfigurationManager.AppSettings["WPRatesName"]];
            SPView spView = spList.Views[System.Configuration.ConfigurationManager.AppSettings["WPRatesViewName"]];
            this.xlvwpRates.ListId = spList.ID;
            this.xlvwpRates.ListName = string.Format("{{{0}}}", spList.ID.ToString());
            this.xlvwpRates.ViewGuid = spView.ID.ToString();
        }
    }
}
