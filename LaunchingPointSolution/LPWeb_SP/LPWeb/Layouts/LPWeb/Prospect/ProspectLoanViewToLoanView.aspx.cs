using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;
using System.Text;

public partial class Prospect_ProspectLoanViewToLoanView : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        // FromPage
        if (this.Request.QueryString["FromPage"] == null)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.location.href='../Pipeline/ProcessingPipelineSummary.aspx'");
        }
        string sFromPage = this.Request.QueryString["FromPage"].ToString();
        string sFromPage_Decode = Encrypter.Base64Decode(sFromPage);

        // FileID
        bool bIsValid = PageCommon.ValidateQueryString(this, "FileID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.location.href='" + sFromPage_Decode + "'");
        }
        string sFileID = this.Request.QueryString["FileID"].ToString();

        #endregion

        #region get the current lead's Borrower's or Co-borrower's loan records

        string sSql = "select a.FileId from Loans as a inner join LoanContacts as b on a.FileId=b.FileId "
                         + "where a.Status<>'Prospect' "
                         + "and (b.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or b.ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId()) "
                         + "and b.ContactId in (select ContactId from LoanContacts where FileId=" + sFileID + " and (ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId()))";

        DataTable LoanList = null;
        try
        {
            LoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        }
        catch
        {
            PageCommon.WriteJsEnd(this, "Failed to get the loan records for the lead.", "window.location.href='" + sFromPage_Decode + "'");
        }

        if (LoanList.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Cannot find the loan record for the current lead.", "window.location.href='" + sFromPage_Decode + "'");
        }

        #endregion

        #region build url

        string sFirstLoanID = string.Empty;
        StringBuilder sbLoanIDs = new StringBuilder();
        foreach (DataRow LoanRow in LoanList.Rows)
        {
            string sLoanID = LoanRow["FileId"].ToString();
            if (sbLoanIDs.Length == 0)
            {
                sbLoanIDs.Append(sLoanID);
                sFirstLoanID = sLoanID;
            }
            else
            {
                sbLoanIDs.Append("," + sLoanID);
            }
        }

        #endregion

        // 重定向到 ProspectLoanDetails.aspx
        this.Response.Redirect("../LoanDetails/LoanDetails.aspx?FromPage=&fieldid=" + sFirstLoanID + "&fieldids=" + sbLoanIDs.ToString());
    }
}
