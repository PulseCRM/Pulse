using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using Utilities;
using LPWeb.LP_Service;

public partial class ProspectDetailViewTopPane : BasePage
{
    public int iContactID = 0;
    protected string sHasEmailViewRight = "0";
    protected string sHasNoteViewRight = "0";
    protected string sHasMarketingRight = "0";

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

        //是否有View权限
        sHasEmailViewRight = this.CurrUser.userRole.Prospect.ToString().IndexOf('N') > -1 ? "1" : "0";  //View
        sHasNoteViewRight = this.CurrUser.userRole.Prospect.ToString().IndexOf('L') > -1 ? "1" : "0";  //View
        sHasMarketingRight = this.CurrUser.userRole.Marketing.ToString().IndexOf('3') > -1 ? "1" : "0"; //View
    }
}
