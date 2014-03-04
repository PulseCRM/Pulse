using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;

public partial class Settings_EmailTemplateAttachmentEnableAjax : BasePage
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

        // Enabled
        bIsValid = PageCommon.ValidateQueryString(this, "Enabled", QueryStringType.String);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string Enabled = this.Request.QueryString["Enabled"];
        if (Enabled.ToLower() != "true" && Enabled.ToLower() != "false")
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid query string.\"}");
            this.Response.End();
        }
        
        #endregion

        #region Enable/Disable Template_Email_Attachments

        try
        {
            string sSql = string.Empty;
            if (Enabled == "true")
            {
                sSql = "update Template_Email_Attachments set Enabled=1 where TemplAttachId in (" + TemplAttachIDs + ")";
            }
            else
            {
                sSql = "update Template_Email_Attachments set Enabled=0 where TemplAttachId in (" + TemplAttachIDs + ")";
            }
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
        }
        catch (Exception)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Enable/Disable attachment(s) failed.\"}");
            this.Response.End();
        }

        #endregion
        
        this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
        this.Response.End();
    }
}
