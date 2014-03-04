using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.WPPage
{
    public partial class SharedDocuments : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SPWeb web = this.Web;
            SPList spList = web.Lists[System.Configuration.ConfigurationManager.AppSettings["WPSharedDocsName"]];
            SPView spView = spList.Views[System.Configuration.ConfigurationManager.AppSettings["WPSharedDocsViewName"]];
            this.xlvwpSharedDocs.ListId = spList.ID;
            this.xlvwpSharedDocs.ListName = string.Format("{{{0}}}", spList.ID.ToString());
            this.xlvwpSharedDocs.ViewGuid = spView.ID.ToString();
        }
    }
}
