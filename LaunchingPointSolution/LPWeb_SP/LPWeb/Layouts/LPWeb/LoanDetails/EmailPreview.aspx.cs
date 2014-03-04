using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.LP_Service;
using System.Text;

public partial class LoanDetails_EmailPreview : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sErrorJs = "alert('Missing required query string.');window.close();";

        #region UseEmailTemplate

        if (this.Request.QueryString["UseEmailTemplate"] == null)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing3", sErrorJs, true);
            return;
        }
        string sUseEmailTemplate = this.Request.QueryString["UseEmailTemplate"].ToString();
        if (sUseEmailTemplate != "0" && sUseEmailTemplate != "1") 
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Invalid3", "alert('Invalid query string.');window.close();", true);
            return;
        }
        bool bUseEmailTemplate = sUseEmailTemplate == "1" ? true : false;

        #endregion

        #region EmailTemplateID

        bool bIsValid = PageCommon.ValidateQueryString(this, "EmailTemplateID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing1", sErrorJs, true);
            return;
        }
        string sEmailTemplateID = this.Request.QueryString["EmailTemplateID"].ToString();

        #endregion

        #region text email body

        if (this.Request.QueryString["TextBody"] == null)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing6", sErrorJs, true);
            return;
        }
        string sTextBody = this.Request.QueryString["TextBody"].ToString();

        #endregion

        #region LoanID or ProspectID or ProspectAlertID

        string sLoanID = string.Empty;
        string sProspectID = string.Empty;
        string sProspectAlertID = string.Empty;

        if (this.Request.QueryString["LoanID"] == null
            && this.Request.QueryString["ProspectID"] == null
            && this.Request.QueryString["ProspectAlertID"] == null)
        {

            this.ClientScript.RegisterStartupScript(this.GetType(), "_Missing11", sErrorJs, true);
            return;
        }

        if (this.Request.QueryString["LoanID"] != null)
        {
            #region LoanID

            bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
            if (bIsValid == false)
            {

                this.ClientScript.RegisterStartupScript(this.GetType(), "_Missing12", sErrorJs, true);
                return;
            }
            sLoanID = this.Request.QueryString["LoanID"].ToString();

            #endregion
        }
        else if (this.Request.QueryString["ProspectID"] != null)
        {
            #region ProspectID

            bIsValid = PageCommon.ValidateQueryString(this, "ProspectID", QueryStringType.ID);
            if (bIsValid == false)
            {

                this.ClientScript.RegisterStartupScript(this.GetType(), "_Missing13", sErrorJs, true);
                return;
            }
            sProspectID = this.Request.QueryString["ProspectID"].ToString();

            #endregion
        }
        else if (this.Request.QueryString["ProspectAlertID"] != null)
        {
            #region ProspectAlertID

            bIsValid = PageCommon.ValidateQueryString(this, "ProspectAlertID", QueryStringType.ID);
            if (bIsValid == false)
            {

                this.ClientScript.RegisterStartupScript(this.GetType(), "_Missing14", sErrorJs, true);
                return;
            }
            sProspectAlertID = this.Request.QueryString["ProspectAlertID"].ToString();

            #endregion
        }

        #endregion

        #region Append My Picture and Signature

        if (this.Request.QueryString["AppendMyPic"] == null)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing4", sErrorJs, true);
            return;
        }
        string sAppendMyPic = this.Request.QueryString["AppendMyPic"].ToString();
        if (sAppendMyPic != "0" && sAppendMyPic != "1")
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Invalid4", "alert('Invalid query string.');window.close();", true);
            return;
        }
        bool bAppendMyPic = sAppendMyPic == "1" ? true : false;

        #endregion

        #endregion

        #region 生成email body

        // workflow api
        string sHtmlBody = string.Empty;

        try 
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                EmailPreviewRequest req = new EmailPreviewRequest();
                if (bUseEmailTemplate == true)
                {
                    req.EmailTemplId = Convert.ToInt32(sEmailTemplateID);
                    req.EmailBody = null;
                    req.AppendPictureSignature = false;
                }
                else
                {
                    req.EmailTemplId = 0;
                    string sTextBody_Decode = Encrypter.Base64Decode(sTextBody);
                    req.EmailBody = Encoding.UTF8.GetBytes(sTextBody_Decode);
                    req.AppendPictureSignature = bAppendMyPic;
                }

                if (this.Request.QueryString["LoanID"] != null)
                {
                    req.FileId = Convert.ToInt32(sLoanID);
                }
                else if (this.Request.QueryString["ProspectID"] != null)
                {
                    req.ProspectId = Convert.ToInt32(sProspectID);
                }
                else if (this.Request.QueryString["ProspectAlertID"] != null)
                {
                    req.PropsectTaskId = Convert.ToInt32(sProspectAlertID);
                }
         

                req.hdr = new ReqHdr();
                req.UserId = this.CurrUser.iUserID;
                
                EmailPreviewResponse response = service.PreviewEmail(req);

                if (response.resp.Successful == false)
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_FailedGetHtml", "alert('Failed to preview email: " + response.resp.StatusInfo + "');window.close();", true);
                    return;
                }

                sHtmlBody = Encoding.UTF8.GetString(response.EmailHtmlContent);
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            PageCommon.AlertMsg(this, "Failed to preview email,  Email Manager is not running, error: " + ee.ToString());
        }

        #endregion
        
        this.ltEmailBody.Text = sHtmlBody;
    }
}
