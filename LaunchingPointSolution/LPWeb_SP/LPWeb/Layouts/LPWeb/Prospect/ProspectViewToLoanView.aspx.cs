using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;
using System.Text;

public partial class Prospect_ProspectViewToLoanView : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // FromPage
        if (this.Request.QueryString["FromPage"] == null)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.location.href='../Pipeline/ProspectPipelineSummary.aspx'");
        }
        string sFromPage = this.Request.QueryString["FromPage"].ToString();
        string sFromPage_Decode = Encrypter.Base64Decode(sFromPage);

        // ContactID
        bool bIsValid = PageCommon.ValidateQueryString(this, "ContactID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.location.href='" + sFromPage_Decode + "'");
        }
        string sContactID = this.Request.QueryString["ContactID"].ToString();

        #region get loan ids by contact id

        string sSql = "select a.FileId from LoanContacts as a inner join Loans as b on a.FileId=b.FileId "
                         + "where a.ContactId=" + sContactID + " "
                        + "and (a.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or a.ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId()) "
                        + "and b.Status<>'Prospect' ";
        
        DataTable LoanList = null;
        try
        {
            LoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        }
        catch
        {
            PageCommon.WriteJsEnd(this, "Failed to get loan records for the prospect.", "window.location.href='" + sFromPage_Decode + "'");
        }

        if (LoanList.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "No loans found for the prospect.", "window.location.href='" + sFromPage_Decode + "'");
        }
        
        #endregion

        #region build url

        string sFirstLoanID = string.Empty;
        StringBuilder sbLoanIDs = new StringBuilder();
        foreach (DataRow LoanRow in LoanList.Rows)
        {
            string sLoanID = LoanRow["FileID"].ToString();

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

        this.Response.Redirect("../LoanDetails/LoanDetails.aspx?FromPage=&fieldid=" + sFirstLoanID + "&fieldids=" + sbLoanIDs.ToString());
    }
}
