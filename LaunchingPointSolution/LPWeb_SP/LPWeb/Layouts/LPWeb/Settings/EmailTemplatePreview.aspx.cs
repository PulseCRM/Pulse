using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.BLL;
using LPWeb.Common;
using System.Data;

public partial class Settings_EmailTemplatePreview : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.QueryString["html"] == null && this.Request.QueryString["id"] == null) 
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing1", "alert('Missing required query string.');window.close();", true);
            return;
        }
        
        string sHtmlBody = "";

        if (this.Request.QueryString["html"] != null)
        {
            string sHtmlBody_Encode = this.Request.QueryString["html"].ToString();
            sHtmlBody = Encrypter.Base64Decode(sHtmlBody_Encode);
        }
        else if (this.Request.QueryString["id"] != null)
        {
            int iEmailTemplateID = Convert.ToInt32(this.Request.QueryString["id"]);
            LPWeb.BLL.Template_Email emailTempMgr = new Template_Email();
            LPWeb.Model.Template_Email emailTempModel = emailTempMgr.GetModel(iEmailTemplateID);
            

            if (emailTempModel.TemplEmailId > 0)
            {
                sHtmlBody = GetEmailTemplateContentWithEmailSkin(emailTempModel.EmailSkinId, emailTempModel.Content);
            }
            else
            {
                sHtmlBody = emailTempModel.Content;
            }
        }

        this.ltEmailBody.Text = sHtmlBody;
    }

    //gdc CR32 20120804
    public string GetEmailTemplateContentWithEmailSkin(int emailSkinId, string emailTemplateContent)
    {
        LPWeb.BLL.Template_EmailSkins bll = new Template_EmailSkins();
        string content = "";

        #region Skin Read To content
        if (emailSkinId <= 0)
        {
            DataSet dsDefaultSkin = null;
            string sqlCmdDefaultSkin = string.Format(" [Default] =1 AND [Enabled] = 1 ");
            dsDefaultSkin = bll.GetList(sqlCmdDefaultSkin);
            if ((dsDefaultSkin == null) || (dsDefaultSkin.Tables[0].Rows.Count <= 0) || dsDefaultSkin.Tables[0].Rows[0]["HTMLBody"] == DBNull.Value)
            {
                content = "";
            }
            else
            {
                content = dsDefaultSkin.Tables[0].Rows[0]["HTMLBody"].ToString();
            }
        }
        else
        {
            DataSet dsSkin = null;
            string sqlCmdSkin = string.Format(" EmailSkinId ={0} AND [Enabled] = 1 ", emailSkinId);
            dsSkin = bll.GetList(sqlCmdSkin);
            if ((dsSkin == null) || (dsSkin.Tables[0].Rows.Count <= 0) || dsSkin.Tables[0].Rows[0]["HTMLBody"] == DBNull.Value)
            {
                content = "";
            }
            else
            {
                content = dsSkin.Tables[0].Rows[0]["HTMLBody"].ToString();
            }
        }
        #endregion

        if (!string.IsNullOrEmpty(content) && System.Text.RegularExpressions.Regex.IsMatch(content, "&lt;@EmailBody@&gt;"))  //<@EmailBody@> == &lt;@EmailBody@&gt;   (Encoded)
        {

            content = System.Text.RegularExpressions.Regex.Replace(content, "&lt;@EmailBody@&gt;", emailTemplateContent);

        }
        else
        {
            content = emailTemplateContent;
        }

        return content;
    }

}
