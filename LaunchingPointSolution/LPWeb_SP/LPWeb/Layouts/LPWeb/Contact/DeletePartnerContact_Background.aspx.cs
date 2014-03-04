using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.BLL;
using LPWeb.Common;

public partial class Contact_DeletePartnerContact_Background : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // {"ExecResult":"Success"}
        // {"ExecResult":"Failed","ErrorMsg":"Failed to check contact reference."}

        #region 接收参数

        // DelContactID
        bool bIsValid = PageCommon.ValidateQueryString(this, "DelContactID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            return;
        }

        string sDelContactID = this.Request.QueryString["DelContactID"].ToString();
        int iDelContactID = Convert.ToInt32(sDelContactID);
        
        #endregion

        #region delete contact

        Contacts bllContact = new Contacts();
        int iUserType = 0;
        if (this.CurrUser.bIsCompanyExecutive)
        {
            iUserType = 0;
        }
        else if (this.CurrUser.bIsBranchManager)
        {
            iUserType = 1;
        }
        else
        {
            iUserType = 2;
        }
        try
        {
            bllContact.PartnerContactsDelete(iDelContactID, iUserType, CurrUser.iUserID);
        }
        catch
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Failed to delete partner contact.\"}");
            return;
        }

        #endregion

        this.Response.Write("{\"ExecResult\":\"Success\"}");
    }
}
