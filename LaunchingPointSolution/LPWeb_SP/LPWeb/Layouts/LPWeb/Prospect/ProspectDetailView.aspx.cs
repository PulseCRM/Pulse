using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Text.RegularExpressions;

public partial class ProspectDetailView : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sReturnUrl = "../Pipeline/ProspectPipelineSummary.aspx";
        string sErrorMsg = "Failed to load this page: missing required query string.";
        string sBackJs = "window.location.href='" + sReturnUrl + "'";

        // e
        bool bIsValid = PageCommon.ValidateQueryString(this, "e", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sErrorMsg, sBackJs);
        }
        string sEncodeQueryString = this.Request.QueryString["e"];
        string sDecodeQueryString = Encrypter.Base64Decode(sEncodeQueryString);

        bIsValid = Regex.IsMatch(sDecodeQueryString, @"^ContactID=([1-9]\d*)&ContactIDs=([1-9]\d*)(,[1-9]\d*)*$");
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sErrorMsg, sBackJs);
        }

        string[] QueryStringArray = sDecodeQueryString.Split('&');

        string sContactIDKeyValue = QueryStringArray[0];
        string sContactIDsKeyValue = QueryStringArray[1];

        string[] ContactIDKeyValueArray = sContactIDKeyValue.Split('=');
        string[] ContactIDsKeyValueArray = sContactIDsKeyValue.Split('=');

        // ContactID
        string sContactID = ContactIDKeyValueArray[1];

        // ContactIDs
        string sContactIDs = ContactIDsKeyValueArray[1];

        // PageFrom
        bIsValid = PageCommon.ValidateQueryString(this, "PageFrom", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sErrorMsg, sBackJs);
        }
        string sPageFrom = this.Request.QueryString["PageFrom"];
        if (sPageFrom == string.Empty)
        {
            sPageFrom = sReturnUrl;
        }

        #endregion
        
        hfID.Value = sContactID;
        hfIDs.Value = sContactIDs;
        hfPageFrom.Value = sPageFrom;

        #region AccessUnassignedLeads check

        LPWeb.BLL.Prospect prospect = new LPWeb.BLL.Prospect();
        
        LPWeb.Model.Prospect pModel = prospect.GetModel(Convert.ToInt32(sContactID));

        //当 有loanOfficer 指定  或者
        if (pModel != null && (pModel.Loanofficer == null || pModel.Loanofficer == 0 || pModel.Loanofficer == -1))
        {
            if (!CurrUser.bAccessOtherLoans && !CurrUser.userRole.AccessUnassignedLeads)
            {
                PageCommon.WriteJsEnd(this, "No permission to access", sBackJs);
            }
        }

        #endregion
    }
}
