using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;

namespace LPWeb.Layouts.LPWeb.WPPage
{
    public partial class MyCalendar : WebPartPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            this.MasterPageFile = "/_catalogs/masterpage/LPWeb/Super.master";
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}
