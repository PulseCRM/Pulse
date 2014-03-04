using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;

public partial class Prospect_ProspectLoanDetails : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sReturnUrl = "../Pipeline/ProspectPipelineSummaryLoan.aspx";
        string sErrorMsg = "Failed to load this page: missing required query string.";
        string sBackJs = "window.location.href='" + sReturnUrl + "'";

        // FileID
        bool bIsValid = PageCommon.ValidateQueryString(this, "FileID", QueryStringType.ID);
        if(bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sErrorMsg, sBackJs);
        }
        string sFileID = this.Request.QueryString["FileID"];

        // FileIDs
        bIsValid = PageCommon.ValidateQueryString(this, "FileIDs", QueryStringType.IDs);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sErrorMsg, sBackJs);
        }
        string sFileIDs = this.Request.QueryString["FileIDs"];

        // FromPage
        bIsValid = PageCommon.ValidateQueryString(this, "FromPage", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sErrorMsg, sBackJs);
        }
        string sPageFrom = this.Request.QueryString["FromPage"];
        if (sPageFrom == string.Empty)
        {
            sPageFrom = sReturnUrl;
        }

        #endregion

        this.hfID.Value = sFileID;
        this.hfIDs.Value = sFileIDs;
        this.hfPageFrom.Value = sPageFrom;

        // insert the UserRecentItems 
        LPWeb.BLL.UserRecentItems _bUserRecentItems = new LPWeb.BLL.UserRecentItems();
        _bUserRecentItems.InsertUserRecentItems(Convert.ToInt32(sFileID), CurrUser.iUserID);

        #region MyRegion

        //与 ProspectLoanDetailsInfo.aspx 页采用相同方法
        string sSql4 = string.Format("Select dbo.lpfn_GetLoanOfficer({0})", sFileID);
        object obj = LPWeb.DAL.DbHelperSQL.GetSingle(sSql4);
        string LoanOfficerInfo = (obj == null || obj == DBNull.Value) ? string.Empty : (string)obj;

        if (string.IsNullOrEmpty(LoanOfficerInfo) && CurrUser.userRole.AccessUnassignedLeads == false
             //&& CurrUser.sRoleName != "Executive" && CurrUser.sRoleName != "Branch Manager"
            )
        {
            PageCommon.WriteJsEnd(this, "No permission to access", sBackJs);
        }

        
        #endregion
    }

}