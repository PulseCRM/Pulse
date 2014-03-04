using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;


public partial class PartnerContactDetailView : BasePage
{
    public string sContactID = "0";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.Request.QueryString["ContactID"]))
        {
            sContactID = this.Request.QueryString["ContactID"];
        }
        hfContactID.Value = sContactID;
    }
}
