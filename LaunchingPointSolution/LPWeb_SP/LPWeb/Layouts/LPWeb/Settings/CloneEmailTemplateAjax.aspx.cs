using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.Common;
using System.Data;

public partial class Settings_CloneEmailTemplateAjax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // {"ExecResult":"Success"}
        // {"ExecResult":"Failed","ErrorMsg":"unknown errors."}

        #region 接收参数

        // EmailTemplateID
        bool bIsValid = PageCommon.ValidateQueryString(this, "EmailTemplateID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Lose required query string.\"}");
            this.Response.End();
        }
        string sEmailTemplateID = this.Request.QueryString["EmailTemplateID"];
        int iEmailTemplateID = Convert.ToInt32(sEmailTemplateID);
        
        #endregion

        #region insert email template

        try
        {
            // Template_Email
            string sSql = "INSERT INTO Template_Email (Enabled,Name,[Desc],FromUserRoles,FromEmailAddress,Content,Subject,Target,Custom,SendTrigger,SenderName,EmailSkinId) "
                        + "select Enabled,Name + ' Copy' as Name,[Desc],FromUserRoles,FromEmailAddress,Content,Subject,Target,Custom,SendTrigger,SenderName,EmailSkinId from Template_Email where TemplEmailId=" + sEmailTemplateID + " "
                        + "select SCOPE_IDENTITY()";

            int iNewID = Convert.ToInt32(LPWeb.DAL.DbHelperSQL.ExecuteScalar(sSql));

            // Template_Email_Recipients
            string sSql2 = "INSERT INTO Template_Email_Recipients (TemplEmailId,EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner) "
                         + "select " + iNewID + ", EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner from Template_Email_Recipients where TemplEmailId=" + iEmailTemplateID;
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql2);

        }
        catch (Exception)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Failed to clone task.\"}");
            this.Response.End();
        }
        
        #endregion

        this.Response.Write("{\"ExecResult\":\"Success\"}");
        this.Response.End();
    }
}

