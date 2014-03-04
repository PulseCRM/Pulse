using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;

public partial class ProspectLoans : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        string sContactID = this.Request.QueryString["ContactID"];
        hfContactID.Value = sContactID;
    }
}
