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
using System.Data;

public partial class LoanDetails_LoanDetails : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sReturnUrl = "../Pipeline/ProcessingPipelineSummary.aspx";
        string sErrorMsg = "Failed to load this page: missing required query string.";
        string sBackJs = "window.location.href='" + sReturnUrl + "'";
        string sFileID = string.Empty;
        string sFileIDs = string.Empty;
        string sPageFrom = string.Empty;

        // contactid
        bool bIsValid = PageCommon.ValidateQueryString(this, "ContactID", QueryStringType.ID);
        if (bIsValid)
        {
            try
            {
                int ContactID = int.Parse(this.Request.QueryString["ContactID"]);
                LPWeb.BLL.LoanContacts blllc = new LPWeb.BLL.LoanContacts();
                DataSet ds = blllc.GetContactLoans(ContactID);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, sBackJs);
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sFileID = ds.Tables[0].Rows[0][0].ToString();
                        }
                        sFileIDs = ds.Tables[0].Rows[0][0].ToString() + ",";
                    }

                    sFileIDs = sFileIDs.TrimEnd(",".ToCharArray());
                }
            }
            catch
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, sBackJs);
            }
        }
        else
        {
            // fieldid
            bIsValid = PageCommon.ValidateQueryString(this, "fieldid", QueryStringType.ID);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, sBackJs);
            }
            sFileID = this.Request.QueryString["fieldid"];

            // fieldids
            bIsValid = PageCommon.ValidateQueryString(this, "fieldids", QueryStringType.IDs);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, sBackJs);
            }
            sFileIDs = this.Request.QueryString["fieldids"];
        }

        // FromPage
        bIsValid = PageCommon.ValidateQueryString(this, "FromPage", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sErrorMsg, sBackJs);
        }
        sPageFrom = this.Request.QueryString["FromPage"];
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
    }

}