using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;
using System.Text;

public partial class Prospect_LoanViewToProspectView : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // FromPage
        if (this.Request.QueryString["FromPage"] == null)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.location.href='../Pipeline/ProspectPipelineSummaryLoan.aspx'");
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

        #region get contact ids by loan id

        string sSql = "select * from LoanContacts where FileId=" + sFileID + " and (ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId())";
        DataTable ContactList = null;
        try
        {
            ContactList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        }
        catch
        {
            PageCommon.WriteJsEnd(this, "Failed to get the client record for the lead.", "window.location.href='" + sFromPage_Decode + "'");
        }
         
        if (ContactList.Rows.Count == 0) 
        {
            PageCommon.WriteJsEnd(this, "Cannot find the client record for the current lead.", "window.location.href='" + sFromPage_Decode + "'");
        }

        #endregion

        #region build url

        string sFirstContactID = string.Empty;
        StringBuilder sbContactIDs = new StringBuilder();
        foreach (DataRow ContactRow in ContactList.Rows)
        {
            string sContactID = ContactRow["ContactID"].ToString();
            if (sbContactIDs.Length == 0) 
            {
                sbContactIDs.Append(sContactID);
                sFirstContactID = sContactID;
            }
            else
            {
                sbContactIDs.Append("," + sContactID);
            }
        }

        #endregion

        string c = "ContactID=" + sFirstContactID + "&ContactIDs=" + sbContactIDs.ToString();
        string en = Encrypter.Base64Encode(c);

        this.Response.Redirect("ProspectDetailView.aspx?PageFrom=&e=" + en);
    }
}
