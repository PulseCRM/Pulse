using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.AnyChart
{
    public partial class SalesBreakdown_GetData : BasePage
    {
        public string sPiePointText = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sbPiePoints = new StringBuilder();

            for (int i = 0; i < this.Request.QueryString.Count; i++)
            {
                string sQueryStringName = this.Request.QueryString.GetKey(i);
                string sQueryStringValue = this.Request.QueryString.Get(i);

                if (sQueryStringName == "sid")
                {
                    continue;
                }

                if (sQueryStringName == "XMLCallDate")
                {
                    continue;
                }

                decimal dAmount = decimal.Parse(sQueryStringValue) / 1000;
                string sPointText = "<point name=\"" + sQueryStringName + "\" y=\"" + dAmount + "\"";
                //if (ConfigurationManager.AppSettings["PieColor_Opened"] != null && ConfigurationManager.AppSettings["PieColor_Opened"] != string.Empty)
                //{
                //    string sColor = ConfigurationManager.AppSettings["PieColor_Opened"];
                //    sPointText += " color=\"" + sColor + "\"";
                //}
                sPointText += " />";
                sbPiePoints.AppendLine(sPointText);
            }
            
            this.sPiePointText = sbPiePoints.ToString();
        }
    }
}