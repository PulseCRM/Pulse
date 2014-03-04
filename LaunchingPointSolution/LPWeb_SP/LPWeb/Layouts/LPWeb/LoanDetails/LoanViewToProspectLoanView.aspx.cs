using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;
using System.Text;

public partial class LoanDetails_LoanViewToProspectLoanView : BasePage
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

        #region get the current loan's Borrower's or Co-borrower's lead records

        string sSql = "select a.FileId from Loans as a inner join LoanContacts as b on a.FileId=b.FileId "
                         + "where a.Status='Prospect' "
                         + "and (b.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or b.ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId()) "
                         + "and b.ContactId in (select ContactId from LoanContacts where FileId=" + sFileID + " and (ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId()))";

        DataTable ProspectLoanList = null;
        try
        {
            ProspectLoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        }
        catch
        {
            PageCommon.WriteJsEnd(this, "Failed to get the lead record for the loan.", "window.location.href='" + sFromPage_Decode + "'");
        }

        if (ProspectLoanList.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Cannot find the lead record for the current loan.", "window.location.href='" + sFromPage_Decode + "'");
        }

        #endregion

        #region build url

        string sFirstProspectLoanID = string.Empty;
        StringBuilder sbProspectLoanIDs = new StringBuilder();
        foreach (DataRow ProspectLoanRow in ProspectLoanList.Rows)
        {
            string sProspectLoanID = ProspectLoanRow["FileId"].ToString();
            if (sbProspectLoanIDs.Length == 0)
            {
                sbProspectLoanIDs.Append(sProspectLoanID);
                sFirstProspectLoanID = sProspectLoanID;
            }
            else
            {
                sbProspectLoanIDs.Append("," + sProspectLoanID);
            }
        }

        #endregion

        // 重定向到 ProspectLoanDetails.aspx
        this.Response.Redirect("../Prospect/ProspectLoanDetails.aspx?FromPage=&FileID=" + sFirstProspectLoanID + "&FileIDs=" + sbProspectLoanIDs.ToString());
    }
}
