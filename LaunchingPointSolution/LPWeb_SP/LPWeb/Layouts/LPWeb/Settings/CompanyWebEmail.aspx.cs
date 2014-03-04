using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.Model;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;

public partial class Settings_CompanyWebEmail : BasePage
{
    /// <summary>
    /// 
    /// </summary>
    private Company_Web modCompanyWeb = new Company_Web();
    LPWeb.BLL.Company_Web bllCompanyWeb = new LPWeb.BLL.Company_Web();
    private bool isNew = false;
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //权限验证
            var loginUser = new LoginUser();
            if (!loginUser.userRole.CompanySetup)
            {
                Response.Redirect("../Unauthorize.aspx");
                return;
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
        try
        {
            if (!Page.IsPostBack)
            {
                if (Request.UrlReferrer != null)
                {
                    this.btnCancel.PostBackUrl = Request.UrlReferrer.ToString();
                }

                GetInitData();
                if (modCompanyWeb != null)
                {
                    SetDataToUI();
                }
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

    }
    /// <summary>
    /// Gets the data from UI.
    /// </summary>
    private void GetDataFromUI()
    {
        //get existing record
        GetInitData();
        if (modCompanyWeb == null)
        {
            modCompanyWeb = new Company_Web();
            isNew = true;
        }

        modCompanyWeb.EmailAlertsEnabled = chkEmailAlertsEnabled.Checked;

        modCompanyWeb.SendEmailViaEWS = chkSendEmailEWS.Checked;

        string emailRelayServer = txtEmailRelayServer.Text.Trim();
        if (!string.IsNullOrEmpty(emailRelayServer))
        {
            modCompanyWeb.EmailRelayServer = emailRelayServer;
        }

        string defaultAlertEmail = txtDefaultAlertEmail.Text.Trim();
        if (!string.IsNullOrEmpty(defaultAlertEmail))
        {
            modCompanyWeb.DefaultAlertEmail = defaultAlertEmail;
        }

        string emailInterval = ddlEmailInterval.SelectedValue;
        if (!string.IsNullOrEmpty(emailInterval))
        {
            modCompanyWeb.EmailInterval = emailInterval.Parse<int>();
        }

        string websiteURL = txtWebsiteURL.Text.Trim();
        if (!string.IsNullOrEmpty(websiteURL))
        {
            modCompanyWeb.LPCompanyURL = websiteURL;
        }

        string partnerWebsiteURL = txtPartnerWebsiteURL.Text.Trim();
        if (!string.IsNullOrEmpty(partnerWebsiteURL))
        {
            modCompanyWeb.BorrowerURL = partnerWebsiteURL;
        }

        string partnerWebsiteGreeting = txtPartnerWebsiteGreeting.Text.Trim();
        if (!string.IsNullOrEmpty(partnerWebsiteGreeting))
        {
            modCompanyWeb.BorrowerGreeting = partnerWebsiteGreeting;
        }

        string homePageLogo = txtHomePageLogo.PostedFile.FileName.GetFileName();
        if (!string.IsNullOrEmpty(homePageLogo))
        {
            modCompanyWeb.HomePageLogo = homePageLogo;
            string ext = Path.GetExtension(homePageLogo);
            if (CheckFileExt(ext))
            {
                int intDocLen = txtHomePageLogo.PostedFile.ContentLength;
                byte[] Docbuffer = new byte[intDocLen];
                Stream objStream = txtHomePageLogo.PostedFile.InputStream;
                objStream.Read(Docbuffer, 0, intDocLen);
                modCompanyWeb.HomePageLogoData = Docbuffer;
            }
            else
            {
                PageCommon.WriteJsEnd(this, string.Format("File type {0} is not allowed", ext), PageCommon.Js_RefreshSelf);
            }
        }

        string logoforSubPages = txtLogoforSubPages.PostedFile.FileName.GetFileName();
        if (!string.IsNullOrEmpty(logoforSubPages))
        {
            modCompanyWeb.LogoForSubPages = logoforSubPages;
            string ext = Path.GetExtension(logoforSubPages);
            if (CheckFileExt(ext))
            {
                int intDocLen = txtLogoforSubPages.PostedFile.ContentLength;
                byte[] Docbuffer = new byte[intDocLen];
                Stream objStream = txtLogoforSubPages.PostedFile.InputStream;
                objStream.Read(Docbuffer, 0, intDocLen);
                modCompanyWeb.SubPageLogoData = Docbuffer;
            }
            else
            {
                PageCommon.WriteJsEnd(this, string.Format("File type {0} is not allowed", ext), PageCommon.Js_RefreshSelf);
            }
        }

        this.modCompanyWeb.EnableEmailAuditTrail = this.chkEnableEmailAuditTrail.Checked;


        //CR57

        int smtpport = 25;
        try
        {
            if (!string.IsNullOrEmpty(this.txtSMTPPortNo.Text))
            {
                smtpport = Convert.ToInt32(this.txtSMTPPortNo.Text);
            }
        }
        catch { }
        this.modCompanyWeb.SMTP_Port = smtpport;

        this.modCompanyWeb.AuthReq = chkRequriesAuthentication.Checked;
        if (this.modCompanyWeb.AuthReq)
        {
            this.modCompanyWeb.AuthEmailAccount = this.txtEmailAccount.Text.Trim();
            this.modCompanyWeb.AuthPassword = this.txtPassword.Text.Trim();
            this.modCompanyWeb.SMTP_EncryptMethod = this.ddlEncryptMethod.SelectedValue.Trim();
        }
        else
        {
            this.modCompanyWeb.AuthEmailAccount = "";
            this.modCompanyWeb.AuthPassword = "";
            this.modCompanyWeb.SMTP_EncryptMethod = "";
        }
        if (this.modCompanyWeb.SendEmailViaEWS)
        {
            this.modCompanyWeb.EWS_Version = this.ddlEWSVersion.SelectedValue.Trim();
            this.modCompanyWeb.EWS_Domain = this.txbEWSDomain.Text.Trim();
            this.modCompanyWeb.EwsUrl = txbEWSURL.Text.Trim();
        }
        else
        {
            this.modCompanyWeb.EWS_Version = "";
            this.modCompanyWeb.EWS_Domain = "";
            this.modCompanyWeb.EwsUrl = "";
        }

    }

    /// <summary>
    /// Gets the init data.
    /// </summary>
    private void GetInitData()
    {
        try
        {
            modCompanyWeb = bllCompanyWeb.GetModel();
        }
        catch (Exception exception)
        {
            modCompanyWeb = null;
            LPLog.LogMessage(exception.Message);
        }
    }

    /// <summary>
    /// Sets the data to UI.
    /// </summary>
    private void SetDataToUI()
    {
        if (modCompanyWeb == null)
        {
            return;
        }

        chkEmailAlertsEnabled.Checked = modCompanyWeb.EmailAlertsEnabled;
        txtEmailRelayServer.Text = modCompanyWeb.EmailRelayServer;
        txtDefaultAlertEmail.Text = modCompanyWeb.DefaultAlertEmail;
        ddlEmailInterval.SelectedValue = modCompanyWeb.EmailInterval.ToString();
        txtWebsiteURL.Text = modCompanyWeb.LPCompanyURL;
        txtPartnerWebsiteURL.Text = modCompanyWeb.BorrowerURL;
        txtPartnerWebsiteGreeting.Text = modCompanyWeb.BorrowerGreeting;

        chkSendEmailEWS.Checked = modCompanyWeb.SendEmailViaEWS;
        txbEWSURL.Text = modCompanyWeb.EwsUrl;

        #region if no logo, show default Your Logo Here image

        if ((modCompanyWeb.HomePageLogo == null) || (modCompanyWeb.HomePageLogo == string.Empty))
        {
            this.homeLogo.ImageUrl = "../images/YourLogoHere.gif";
        }
        if ((modCompanyWeb.LogoForSubPages == null) || (modCompanyWeb.LogoForSubPages == string.Empty))
        {
            this.subPageLogo.ImageUrl = "../images/YourLogoHere.gif";
        }

        #endregion

        //txtHomePageLogo.Text = modCompanyWeb.HomePageLogo;
        //txtLogoforSubPages.Text = modCompanyWeb.LogoForSubPages;
        this.chkEnableEmailAuditTrail.Checked = modCompanyWeb.EnableEmailAuditTrail;

        //CR57
        this.txtSMTPPortNo.Text = modCompanyWeb.SMTP_Port.ToString();
        this.chkRequriesAuthentication.Checked = modCompanyWeb.AuthReq;
        this.txtEmailAccount.Text = modCompanyWeb.AuthEmailAccount.ToString();
        this.txtPassword.Text = modCompanyWeb.AuthPassword.ToString();
        this.ddlEncryptMethod.SelectedValue = modCompanyWeb.SMTP_EncryptMethod.ToString();
        this.ddlEWSVersion.SelectedValue = modCompanyWeb.EWS_Version.ToString();
        this.txbEWSDomain.Text = modCompanyWeb.EWS_Domain;

        if (!modCompanyWeb.AuthReq)
        {
            this.txtEmailAccount.Enabled = false;
            this.txtPassword.Enabled = false;
            this.ddlEncryptMethod.Enabled = false;
        }

    }
    /// <summary>
    /// Saves this instance.
    /// </summary>
    /// <returns></returns>
    private bool Save()
    {
        bool status = false;
        try
        {
            if (isNew)
            {
                bllCompanyWeb.Add(modCompanyWeb);
            }
            else
            {
                bllCompanyWeb.Update(modCompanyWeb);
            }
            status = true;
        }
        catch (Exception exception)
        {
            status = false;
            LPLog.LogMessage(exception.Message);
        }
        return status;
    }

    /// <summary>
    /// Handles the Click event of the btnSave control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            return;
        }
        GetDataFromUI();
        if (Save() == true)
        {
            //display successful message
            PageCommon.WriteJsEnd(this, "Saved successfully.", PageCommon.Js_RefreshSelf);
        }
        else
        {
            //display faild message
            PageCommon.WriteJsEnd(this, "Failed to save the record.", PageCommon.Js_RefreshSelf);
        }
        SetDataToUI();
    }

    /// <summary>
    /// Handles the Click event of the btnCancel control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    protected void btnImportUsers_Click(object sender, EventArgs e)
    {
    }

    protected void btnSaveAs_Click(object sender, EventArgs e)
    {
    }

    protected void btnTestEmail_Click(object sender, EventArgs e)
    {
        string sSmtpHost = string.Empty;
        if (!string.IsNullOrEmpty(txtEmailRelayServer.Text.Trim()))
        {
            sSmtpHost = txtEmailRelayServer.Text.Trim();
        }

        string sSmtpUsername = string.Empty;
        sSmtpUsername = txtEmailAccount.Text.Trim();

        string sSmtpPwd = string.Empty;
        sSmtpPwd = txtPassword.Text.Trim();

        int iSmtpPort = 25;
        try
        {
            iSmtpPort = Convert.ToInt32(txtSMTPPortNo.Text.Trim());
        }
        catch { }

        bool reqAuth = false;
        if (chkRequriesAuthentication.Checked)
        {
            reqAuth = true;
        }

        string sToName = string.Empty;
        string sToEmail = string.Empty;
        string sFrom = string.Empty;
        if (!string.IsNullOrEmpty(txtDefaultAlertEmail.Text.Trim()))
        {
            sFrom = txtDefaultAlertEmail.Text.Trim();
            sToEmail = sFrom;
            sToName = sToEmail;
        }

        string sSubject = "Test Email from Pulse";
        string sBody = string.Format(@"This is a test email sent from Pulse with the following SMTP settings:
SMTP Relay Server: {0}
SMTP Port No: {1}
Requires Authentication: {2}
Email Account: {3}", sSmtpHost, iSmtpPort, reqAuth ? "Yes" : "No", sSmtpUsername); //"Test Email";
        string sMessage = string.Empty;

        try
        {
            sMessage = SendEmail(sSmtpHost, iSmtpPort, sSmtpUsername, sSmtpPwd, sFrom, sToName, sToEmail, sSubject, sBody);
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
            PageCommon.WriteJsEnd(this, "Failed to send test email, reason:" + exception.Message, "window.location.href=window.location.href");
        }

        if (string.IsNullOrEmpty(sMessage))
        {
            PageCommon.WriteJsEnd(this, "Sent test email successfully.", "window.location.href=window.location.href");
        }
        else
        {
            PageCommon.WriteJsEnd(this, "Failed to send test email.", "window.location.href=window.location.href");
        }
    }

    /// <summary>
    /// Sends the email.
    /// </summary>
    /// <param name="sSmtpHost">The s SMTP host.</param>
    /// <param name="iSmtpPort">The i SMTP port.</param>
    /// <param name="sSmtpUsername">The s SMTP username.</param>
    /// <param name="sSmtpPwd">The s SMTP PWD.</param>
    /// <param name="sFrom">The s from.</param>
    /// <param name="sToName">Name of the s to.</param>
    /// <param name="sToEmail">The s to email.</param>
    /// <param name="sSubject">The s subject.</param>
    /// <param name="sBody">The s body.</param>
    /// <returns></returns>
    private string SendEmail(string sSmtpHost, int iSmtpPort, string sSmtpUsername, string sSmtpPwd, string sFrom, string sToName, string sToEmail, string sSubject, string sBody)
    {
        MailMessage message = new MailMessage();
        message.From = new MailAddress(sFrom);
        message.To.Add(new MailAddress(sToEmail));

        message.Subject = sSubject;
        message.Body = sBody;
        SmtpClient client = new SmtpClient(sSmtpHost, iSmtpPort);
        if (chkRequriesAuthentication.Checked)
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(txtEmailAccount.Text.Trim(), txtPassword.Text.Trim());
            var encryptMethod = ddlEncryptMethod.SelectedValue;
            client.EnableSsl = encryptMethod.Contains("TLS") || encryptMethod.Contains("SSL");
        }
        else
        {
            client.UseDefaultCredentials = true;
        }

        try
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            client.Send(message);
            return string.Empty;
        }
        catch (Exception exception)
        {

            LPLog.LogMessage(exception.Message);
            return exception.Message;
        }
        return string.Empty;
    }

    private bool CheckFileExt(string ext)
    {
        switch (ext.ToLower())
        {
            case ".jpg":
            case ".bmp":
            case ".gif":
            case ".png":
                return true;
                break;
            default:
                return false;
                break;
        }
        return false;
    }
}