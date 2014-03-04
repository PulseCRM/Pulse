using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using Utilities;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;


public partial class LeadSetupTab : LayoutsPageBase
{
    public int iFileID = 0;

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
    }
}

