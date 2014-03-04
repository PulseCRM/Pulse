using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;

public partial class Settings_EmailTemplateAttachmentDeleteAjax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"error message"}

        #region 校验页面参数

        // TemplAttachIDs
        bool bIsValid = PageCommon.ValidateQueryString(this, "TemplAttachIDs", QueryStringType.IDs);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string TemplAttachIDs = this.Request.QueryString["TemplAttachIDs"];
        
        #endregion

        #region Delete Template_Email_Attachments

        try
        {
            string sSql = "delete from Template_Email_Attachments where TemplAttachId in (" + TemplAttachIDs + ")";
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
        }
        catch (Exception)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Delete attachment(s) failed.\"}");
            this.Response.End();
        }

        #endregion
        
        this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
        this.Response.End();
    }
}
