using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.AnyChart
{
    public partial class OrganProduction : BasePage
    {
        public string sChartQueryString = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string sOrgan = this.Request.QueryString["Organ"].ToString();
            string sWhere = this.Request.QueryString["w"].ToString();

            this.sChartQueryString = "&Organ=" + sOrgan + "&w=" + sWhere;
        }
    }
}