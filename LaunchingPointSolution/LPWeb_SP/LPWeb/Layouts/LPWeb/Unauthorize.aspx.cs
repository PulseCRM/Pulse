using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace LPWeb.Layouts.LPWeb
{
    public partial class Unauthorize : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sMsg = "You have no privilege to access this page.";

            if(this.Request.QueryString["Msg"] != null)
            {
                sMsg = this.Request.QueryString["Msg"].ToString();
            }

            this.lbMsg.InnerText = sMsg;

            string sGoBackUrl = this.ResolveClientUrl("~/_layouts/LPWeb/DashBoardHome.aspx");

            if (this.Request.QueryString["GoBackUrl"] != null)
            {
                sGoBackUrl = this.ResolveClientUrl(this.Request.QueryString["GoBackUrl"].ToString());
            }

            this.lnkBack.HRef = sGoBackUrl;
        }

        protected override void OnPreInit(EventArgs e)
        {
            if (null != this.MasterPageFile)
            {
                this.MasterPageFile = "/_catalogs/masterpage/LPWeb/Home.master";
                base.OnPreInit(e);
            }
        }
    }
}
