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
using System.Data;

public partial class LoanDetails_LoanDetail : BasePage
{
    public int iFileID = 0;
    protected string sHasMarketingRight = "1";          // enable marketing tab always for now

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sReturnUrl = "../Pipeline/ProcessingPipelineSummary.aspx";
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

        //sHasMarketingRight = this.CurrUser.userRole.Marketing.ToString().IndexOf('3') > -1 ? "1" : "0"; //View
        //sHasMarketingRight = this.CurrUser.userRole.AccessAllMailChimpList ? "1" : "0";
        #region 加载Loan

        Loans LoanManager = new Loans();
        DataTable LoanInfo = LoanManager.GetLoanInfo(this.iFileID);
        if (LoanInfo.Rows.Count == 0)
        {
            this.Response.Write("Invalid query string.");
            try
            {
                this.Response.End();
            }
            catch
            {

            }
        }

        #endregion

        #region Active Loan or Not

        string sLoanStatus = LoanInfo.Rows[0]["Status"].ToString();
        this.hdnLoanStatus.Value = sLoanStatus;

        #endregion
    }
}