using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;

public partial class Home : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LPLog.LogMessage("test");
    }
}