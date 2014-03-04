using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Text;

namespace LPWeb.AnyChart
{
    public partial class SalesBreakdown : BasePage
    {
        //public string sChartQueryString = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Request.QueryString["xx"].ToString();
            //StringBuilder sbQueryString = new StringBuilder();
            //for (int i = 0; i < this.Request.QueryString.Count; i++)
            //{
            //    string sQueryStringName = this.Request.QueryString.GetKey(i);
            //    string sQueryStringValue = this.Request.QueryString.Get(i);

            //    if (sQueryStringName == "sid")
            //    {
            //        continue;
            //    }

            //    sbQueryString.Append("&" + sQueryStringName + "=" + sQueryStringValue);
            //}

            //this.sChartQueryString = sbQueryString.ToString();
        }
    }
}