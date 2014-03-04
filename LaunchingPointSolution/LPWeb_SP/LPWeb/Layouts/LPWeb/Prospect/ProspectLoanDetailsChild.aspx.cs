using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;

public partial class Prospect_ProspectLoanDetailChild : BasePage
{
    public int iFileID = 0;
    protected string sHasEmailViewRight = "0";
    protected string sHasNoteViewRight = "0";
    protected string sHasMarketingRight = "0";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sReturnUrl = "../Pipeline/ProspectPipelineSummaryLoan.aspx";
        string sErrorMsg = "Failed to load this page: missing required query string.";

        // FileID
        bool bIsValid = PageCommon.ValidateQueryString(this, "FileID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sErrorMsg, "window.parent.location.href='" + sReturnUrl + "'");
        }
        string sFileID = this.Request.QueryString["FileID"];
        this.hfFileID.Value = sFileID;
        this.iFileID = Convert.ToInt32(sFileID);

        #endregion

        //是否有View权限
        sHasEmailViewRight = this.CurrUser.userRole.Loans.ToString().IndexOf('J') > -1 ? "1" : "0";  //View
        sHasNoteViewRight = this.CurrUser.userRole.Loans.ToString().IndexOf('H') > -1 ? "1" : "0";  //View

        sHasMarketingRight = this.CurrUser.userRole.Marketing.ToString().IndexOf('3') > -1 ? "1" : "0"; //View
    }
}