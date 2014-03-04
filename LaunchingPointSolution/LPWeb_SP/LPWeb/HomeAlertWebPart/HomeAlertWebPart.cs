using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace LPWeb.HomeAlertWebPart
{
    [ToolboxItemAttribute(false)]
    public class HomeAlertWebPart : WebPart
    {
        // 当更改可视 Web 部件项目项时，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/LPWeb/HomeAlertWebPart/HomeAlertWebPartUserControl.ascx";

        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(_ascxPath);
            Controls.Add(control);
        }
    }
}
