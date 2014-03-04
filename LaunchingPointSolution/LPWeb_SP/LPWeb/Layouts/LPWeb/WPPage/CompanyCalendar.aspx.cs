using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.WPPage
{
    public partial class CompanyCalendar : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SPWeb web = this.Web;
            SPList spList = web.Lists[System.Configuration.ConfigurationManager.AppSettings["WPComCalName"]];
            this.lvwpComCal.ListId = spList.ID;
            this.lvwpComCal.ListName = string.Format("{{{0}}}", spList.ID.ToString());
        }
    }
}
