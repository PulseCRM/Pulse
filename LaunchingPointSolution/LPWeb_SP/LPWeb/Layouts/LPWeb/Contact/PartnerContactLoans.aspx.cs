using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;

public partial class PartnerContactLoans : BasePage
{
    public int iContactID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sReturnUrl = "../Pipeline/ProspectPipelineSummary.aspx";
        string sErrorMsg = "Failed to load this page: missing required query string.";

        // ContactID
        bool bIsValid = PageCommon.ValidateQueryString(this, "ContactID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sErrorMsg, "window.parent.location.href='" + sReturnUrl + "'");
        }
        string sContactID = this.Request.QueryString["ContactID"];
        this.hfContactID.Value = sContactID;
        this.iContactID = Convert.ToInt32(sContactID);

        #endregion

    }
}

