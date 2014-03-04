using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Common;
using DataAccess;
using focusIT;
using LP2.Service.Common;
using Microsoft.Exchange.WebServices.Data;
using Attachment = System.Net.Mail.Attachment;

namespace EmailManager
{
    /// <summary>
    /// EmailMgr
    /// </summary>
    public class EmailMgr : IEmailMgr
    {
        short Category = 60;
        protected static DataAccess.DataAccess m_da = null;
        private static string BackgroundLoanAlertPage = string.Empty;
        public static Common.Table.CompanyWeb _emailServerSetting = null;
        public static SendEmailRequest req_se = new SendEmailRequest();
        public static ThreadStart seDelegate = null;
        public static Thread seThread = null;
        public const string CC = "CC";
        public const string BCC = "BCC";
        public const string TO = "TO";
        public const string PROPERTYSETID = "3F27C5B5-EE44-4A2C-84E0-0EDBF9994146";
        /// <summary>
        /// Instances this instance.
        /// </summary>
        /// <returns></returns>
        public static EmailMgr Instance
        {
            get
            {
                return new EmailMgr();
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMgr"/> class.
        /// </summary>
        private EmailMgr()
        {
            m_da = new DataAccess.DataAccess();
            string err = string.Empty;
            try
            {
                _emailServerSetting = m_da.GetEmailServerSetting(out err);
                if (!string.IsNullOrEmpty(_emailServerSetting.LPCompanyURL))
                {
                    BackgroundLoanAlertPage = _emailServerSetting.LPCompanyURL.TrimEnd(new char[] { '/' });
                }
                else
                {
                    BackgroundLoanAlertPage = string.Empty;
                }
                if (!string.IsNullOrEmpty(_emailServerSetting.BackgroundLoanAlertPage))
                {
                    BackgroundLoanAlertPage = BackgroundLoanAlertPage + "/" + _emailServerSetting.BackgroundLoanAlertPage.TrimStart(new char[] { '/' });
                }

            }
            catch (Exception e)
            {
                int Event_id = 5099;
                err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + e.Message + "\r\n\r\nStackTrace: " + e.StackTrace;
                Trace.TraceError(e.Message);
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
            }
        }


        public Common.Table.CompanyWeb EmailServerSetting
        {
            get { return _emailServerSetting; }
            set { _emailServerSetting = value; }
        }

        /// <summary>
        /// Toes the email list.
        /// </summary>
        /// <param name="toContractIds">To contract ids.</param>
        /// <param name="toUserIds">To user ids.</param>
        /// <param name="toEmails">To emails.</param>
        /// <returns></returns>
        private static List<MailAddress> ToEmailList(int[] toContractIds, int[] toUserIds, string[] toEmails)
        {
            var toList = new List<string>();
            if (toEmails != null)
            {
                toList.AddRange(toEmails.Select(email => string.Format("{0};{1}", "", email)));
            }
            string err;
            toList.AddRange(m_da.GetEmailList(toContractIds, toUserIds, out err));
            var listAddress = new List<MailAddress>();
            toList.ForEach(item =>
            {
                var items = item.Split(Convert.ToChar(";"));
                var newAddress = new MailAddress(items[1], items[0]);
                if (!ChkExists(listAddress, newAddress))
                {
                    listAddress.Add(newAddress);
                }
            });
            return listAddress;
        }
        /// <summary>
        /// Ccs the email list.
        /// </summary>
        /// <param name="ccContractIds">The cc contract ids.</param>
        /// <param name="ccUserIds">The cc user ids.</param>
        /// <param name="ccEmails">The cc emails.</param>
        /// <returns></returns>
        private static List<MailAddress> CcEmailList(int[] ccContractIds, int[] ccUserIds, string[] ccEmails)
        {
            var toList = new List<string>();
            if (ccEmails != null)
                toList.AddRange(ccEmails.Select(email => string.Format("{0};{1}", "", email)));

            string err;
            toList.AddRange(m_da.GetEmailList(ccContractIds, ccUserIds, out err));
            var listAddress = new List<MailAddress>();
            toList.ForEach(item =>
            {
                var items = item.Split(Convert.ToChar(";"));
                var newAddress = new MailAddress(items[1], items[0]);
                if (!ChkExists(listAddress, newAddress))
                {
                    listAddress.Add(newAddress);
                }
            });
            return listAddress;
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="from">From.</param>
        /// <param name="smtpHost">The SMTP host.</param>
        /// <param name="port">The port.</param>
        /// <param name="tos">The tos.</param>
        /// <param name="ccs">The CCS.</param>
        /// <param name="bcc">The BCC.</param>
        /// <returns></returns>
        public static bool SendEmail(string subject, string body, MailAddress from, string smtpHost, int port, List<MailAddress> tos, List<MailAddress> ccs, List<MailAddress> bcc, Dictionary<string, Stream> attachments)
        {
            short Category = 60;
            string err = "";
            if (from == null)
                throw new ArgumentNullException("from");
            if (string.IsNullOrEmpty(body))
                throw new ArgumentNullException("body");
            if (tos.Count < 1)
                throw new ArgumentNullException("tos");
            int Event_id = 5007;

            #region Email Address check
            if (tos.Count == 1 && !IsEmail(tos.FirstOrDefault().Address))
            {
                Event_id = 5005;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Email Address is invalid. " + tos.FirstOrDefault().Address, EventLogEntryType.Warning, Event_id, Category);
                throw new ArgumentException("Email Address is invalid.");
            }
            try
            {

                foreach (var toMail in tos)
                {
                    //toMail.Address
                    if (string.IsNullOrEmpty(toMail.Address) || !IsEmail(toMail.Address))
                    {
                        Event_id = 5005;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, "Email Address is invalid. " + toMail.Address, EventLogEntryType.Warning, Event_id, Category);
                    }
                }
            }
            catch { }
            #endregion

            if (string.IsNullOrEmpty(smtpHost))
                throw new ArgumentNullException("smtpHost");

            if (port < 0)
            {
                port = 25;
            }

            MailMessage message = new MailMessage();
            message.From = from;
            message.IsBodyHtml = true;
            message.Subject = subject;
            if (tos != null)
                tos.ForEach(item => message.To.Add(item));
            if (ccs != null)
                ccs.ForEach(cc => message.CC.Add(cc));
            if (bcc != null)
                bcc.ForEach(item => message.Bcc.Add(item));

            try
            {
                SmtpClient client = new SmtpClient(_emailServerSetting.EmailRelayServer, _emailServerSetting.SMTP_Port);
                if (client == null)
                {
                    err = string.Format("SendEmail, Unable to make a connection to SMTP: {0}, Port: {1}, ReqAuth: {2}, AuthEmailAccount: {3}, AuthPwd: {4}", _emailServerSetting.EmailRelayServer, _emailServerSetting.SMTP_Port, _emailServerSetting.AuthReq, _emailServerSetting.AuthPassword);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                    return false;
                }

                if (_emailServerSetting.AuthReq)
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_emailServerSetting.AuthEmailAccount, _emailServerSetting.AuthPassword);

                    var encryptMethod = _emailServerSetting.SMTP_EncryptMethod.ToUpper();
                    client.EnableSsl = encryptMethod.Contains("TLS") || encryptMethod.Contains("SSL");
                }
                else
                {
                    client.UseDefaultCredentials = true;
                }

                if (attachments != null && attachments.Count > 0)
                {
                    foreach (KeyValuePair<string, Stream> attachment in attachments)
                    {
                        Stream content = attachment.Value;
                        if (content.Length > 0)
                        {
                            message.Attachments.Add(new Attachment(content, attachment.Key));
                        }
                    }
                }
                //process image data start
                List<LinkedResource> resources = new List<LinkedResource>();
                try
                {
                    body = Regex.Replace(body, @"<img src=""cid:(?<cid>\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-Z0-9]{12}\b)""\s*?data=""(?<data>.*?)"" />", match => ImageReplacement(match, resources, false), RegexOptions.IgnoreCase);
                }
                catch (Exception)
                {
                    body = Regex.Replace(body, @"<img src=""cid:(?<cid>\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-Z0-9]{12}\b)""\s*?data=""(?<data>.*?)""[\s]*?style=""(?<style>.*?)""[\s]*?/>", match => ImageReplacement(match, resources, false, true), RegexOptions.IgnoreCase);
                }
                AlternateView av = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                resources.ForEach(item => av.LinkedResources.Add(item));
                message.AlternateViews.Add(av);
                //process image data end
                client.Send(message);
                return true;
            }
            catch (Exception exception)
            {
                err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.ToString();
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="from">From.</param>
        /// <param name="smtpHost">The SMTP host.</param>
        /// <param name="port">The port.</param>
        /// <param name="tos">The tos.</param>
        /// <param name="ccs">The CCS.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="bImgStyle">The Mark of img style.</param>
        /// <returns></returns>
        public static bool SendEmail(string subject, string body, MailAddress from, string smtpHost, int port, List<MailAddress> tos, List<MailAddress> ccs, List<MailAddress> bcc, bool bImgStyle, Dictionary<string, Stream> attachments)
        {
            short Category = 60;
            if (from == null)
                throw new ArgumentNullException("from");
            if (string.IsNullOrEmpty(body))
                throw new ArgumentNullException("body");
            if (tos.Count < 1)
                throw new ArgumentNullException("tos");


            #region Email Address check
            if (tos.Count == 1 && !IsEmail(tos.FirstOrDefault().Address))
            {
                int Event_id = 5009;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Email Address is invalid. " + tos.FirstOrDefault().Address, EventLogEntryType.Warning, Event_id, Category);
                throw new ArgumentException("Email Address is invalid.");
            }
            try
            {

                foreach (var toMail in tos)
                {
                    //toMail.Address
                    if (string.IsNullOrEmpty(toMail.Address) || !IsEmail(toMail.Address))
                    {
                        int Event_id = 5009;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, "Email Address is invalid. " + toMail.Address, EventLogEntryType.Warning, Event_id, Category);
                    }
                }
            }
            catch { }
            #endregion

            if (string.IsNullOrEmpty(smtpHost))
                throw new ArgumentNullException("smtpHost");

            if (port < 0)
            {
                port = 25;
            }

            MailMessage message = new MailMessage();
            message.From = from;
            message.IsBodyHtml = true;
            message.Subject = subject;
            if (tos != null)
                tos.ForEach(item => message.To.Add(item));
            if (ccs != null)
                ccs.ForEach(cc => message.CC.Add(cc));
            if (bcc != null)
                bcc.ForEach(item => message.Bcc.Add(item));

            SmtpClient client = new SmtpClient(_emailServerSetting.EmailRelayServer, _emailServerSetting.SMTP_Port);

            if (_emailServerSetting.AuthReq)
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_emailServerSetting.AuthEmailAccount, _emailServerSetting.AuthPassword);
                var encryptMethod = _emailServerSetting.SMTP_EncryptMethod.ToUpper();
                client.EnableSsl = encryptMethod.Contains("TLS") || encryptMethod.Contains("SSL");
            }
            else
            {
                client.UseDefaultCredentials = true;
            }


            try
            {
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (KeyValuePair<string, Stream> attachment in attachments)
                    {
                        Stream content = attachment.Value;
                        if (content.Length > 0)
                        {
                            message.Attachments.Add(new Attachment(content, attachment.Key));
                        }
                    }
                }
                //process image data start
                List<LinkedResource> resources = new List<LinkedResource>();
                try
                {
                    body = Regex.Replace(body, @"<img src=""cid:(?<cid>\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-Z0-9]{12}\b)""\s*?data=""(?<data>.*?)"" />", match => ImageReplacement(match, resources, false), RegexOptions.IgnoreCase);
                }
                catch (Exception)
                {
                    body = Regex.Replace(body, @"<img src=""cid:(?<cid>\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-Z0-9]{12}\b)""\s*?data=""(?<data>.*?)""[\s]*?style=""(?<style>.*?)""[\s]*?/>", match => ImageReplacement(match, resources, false, true), RegexOptions.IgnoreCase);
                }
                AlternateView av = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                resources.ForEach(item => av.LinkedResources.Add(item));
                message.AlternateViews.Add(av);
                //process image data end
                client.Send(message);
                return true;
            }
            catch (Exception exception)
            {
                string err = "";
                int Event_id = 5011;
                err = MethodBase.GetCurrentMethod() + exception.ToString();
                Trace.TraceError(exception.Message);
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
        }

        private static ExchangeVersion GetExchangeVersion(string configVersion, ref bool isKnownVersion)
        {
            switch (configVersion.ToUpper())
            {
                case "2007":
                    isKnownVersion = true;
                    return ExchangeVersion.Exchange2007_SP1;
                    break;
                case "2007_SP1":
                    isKnownVersion = true;
                    return ExchangeVersion.Exchange2007_SP1;
                    break;
                case "2010":
                    isKnownVersion = true;
                    return ExchangeVersion.Exchange2010;
                    break;
                case "2010_SP1":
                    isKnownVersion = true;
                    return ExchangeVersion.Exchange2010_SP1;
                    break;
                case "2010_SP2":
                    isKnownVersion = true;
                    return ExchangeVersion.Exchange2010_SP1;
                    break;
                case "2013":
                    isKnownVersion = true;
                    return ExchangeVersion.Exchange2010_SP1;
                    break;
                default:
                    isKnownVersion = false;
                    return ExchangeVersion.Exchange2007_SP1;
                    break;
            }
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="from">From.</param>
        /// <param name="smtpHost">The SMTP host.</param>
        /// <param name="port">The port.</param>
        /// <param name="tos">The tos.</param>
        /// <param name="ccs">The CCS.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="userInfo">The user info.</param>
        /// <returns></returns>
        private static bool SendEmail(string subject, string body, MailAddress from, string smtpHost, int port, List<MailAddress> tos, List<MailAddress> ccs, List<MailAddress> bcc, User userInfo, Dictionary<string, Stream> attachments, out string emailId)
        {
            short Category = 60;
            if (from == null)
                throw new ArgumentNullException("from");
            if (string.IsNullOrEmpty(body))
            {
                body = "   ";
            }
            //   throw new ArgumentNullException("body");
            if (tos.Count < 1)
                throw new ArgumentNullException("tos");


            #region Email Address check
            if (tos.Count == 1 && !IsEmail(tos.FirstOrDefault().Address))
            {
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Email Address is invalid. " + tos.FirstOrDefault().Address, EventLogEntryType.Warning);
                throw new ArgumentException("Email Address is invalid.");
            }
            try
            {

                foreach (var toMail in tos)
                {
                    //toMail.Address
                    if (string.IsNullOrEmpty(toMail.Address) || !IsEmail(toMail.Address))
                    {
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, "Email Address is invalid. " + toMail.Address, EventLogEntryType.Warning);
                    }
                }
            }
            catch { }
            #endregion

            if (string.IsNullOrEmpty(smtpHost))
                throw new ArgumentNullException("smtpHost");
            if (string.IsNullOrEmpty(_emailServerSetting.EwsUrl))
                throw new ArgumentNullException("Invalid Ews URL");
            if (userInfo == null)
                throw new ArgumentNullException("userInfo", "User Info can not be null.");
            if (string.IsNullOrEmpty(userInfo.Username))
                throw new ArgumentNullException("UserName", "The domain user name can't be empty.");
            if (string.IsNullOrEmpty(userInfo.Password))
                throw new ArgumentNullException("Password", "The domain user password can't be empty.");


            emailId = string.Empty;
            bool isKnownVersion = false;
            var ExchgVer = GetExchangeVersion(_emailServerSetting.EWS_Version, ref isKnownVersion);
            ExchangeService service = new ExchangeService();

            if (isKnownVersion)
            {
                service = new ExchangeService(ExchgVer);
            }

            if (!string.IsNullOrEmpty(_emailServerSetting.EWS_Domain))
            {
                service.Credentials = new NetworkCredential(userInfo.Username, userInfo.Password, _emailServerSetting.EWS_Domain);
            }
            else
            {
                service.Credentials = new NetworkCredential(userInfo.Email, userInfo.ExchangePassword);
            }
            //service.Credentials = new NetworkCredential("SPSTEST_CEO", "12345", "FOCUS");

            service.Url = new Uri(_emailServerSetting.EwsUrl);
            //service.Url = new Uri("https://mail2.launchingpoint.com/EWS/exchange.asmx");

            // Create an e-mail message and identify the Exchange service.
            EmailMessage message = new EmailMessage(service);

            message.Body = body;
            message.Body.BodyType = BodyType.HTML;

            message.Subject = subject;

            tos.ForEach(item => message.ToRecipients.Add(new EmailAddress(item.DisplayName, item.Address)));
            ccs.ForEach(item => message.CcRecipients.Add(new EmailAddress(item.DisplayName, item.Address)));
            bcc.ForEach(item => message.BccRecipients.Add(new EmailAddress(item.DisplayName, item.Address)));

            try
            {
                //add customize prop
                Guid guid = Guid.NewGuid();

                //process image data start
                List<LinkedResource> resources = new List<LinkedResource>();
                try
                {
                    body = Regex.Replace(body, @"<img src=""cid:(?<cid>\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-Z0-9]{12}\b)""\s*?data=""(?<data>.*?)"" />", match => ImageReplacement(match, resources, false), RegexOptions.IgnoreCase);
                }
                catch (Exception)
                {
                    body = Regex.Replace(body, @"<img src=""cid:(?<cid>\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-Z0-9]{12}\b)""\s*?data=""(?<data>.*?)""[\s]*?style=""(?<style>.*?)""[\s]*?/>", match => ImageReplacement(match, resources, false, true), RegexOptions.IgnoreCase);
                }
                message.Body = body;//over write email body without image data.
                message.Body += string.Format("<font style=\"color:white;display:none;text-indent:-9999px;font-size:0px;\">GUID={0}</font>", guid.ToString());
                List<FileAttachment> faResource = new List<FileAttachment>();

                foreach (LinkedResource linkedResource in resources)
                {
                    FileAttachment attachment = message.Attachments.AddFileAttachment(string.Format("{0}.png", linkedResource.ContentId), linkedResource.ContentStream);
                    attachment.ContentId = linkedResource.ContentId;
                }

                if (attachments != null)
                {
                    foreach (KeyValuePair<string, Stream> att in attachments)
                    {
                        message.Attachments.AddFileAttachment(att.Key, att.Value);
                    }
                }

                // Define the extended property itself.
                ExtendedPropertyDefinition extendedPropertyDefinition = GetExtendedPropertyDefinition();

                // Stamp the extended property on a message.
                message.SetExtendedProperty(extendedPropertyDefinition, guid.ToString());

                // Send the e-mail message and save a copy.
                message.SendAndSaveCopy();

                //Get email property Id
                emailId = guid.ToString();

                return true;
            }
            catch (Exception exception)
            {
                string err = "";
                int Event_id = 5015;
                err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.ToString();
                Trace.TraceError(exception.Message);
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
        }
        /// <summary>
        /// Gets the extended property definition.
        /// </summary>
        /// <returns></returns>
        private static ExtendedPropertyDefinition GetExtendedPropertyDefinition()
        {
            Guid guid = new Guid(PROPERTYSETID);
            return GetExtendedPropertyDefinition(guid);
        }

        /// <summary>
        /// Gets the extended property definition.
        /// </summary>
        /// <param name="yourPropertySetId">Your property set id.</param>
        /// <returns></returns>
        private static ExtendedPropertyDefinition GetExtendedPropertyDefinition(Guid yourPropertySetId)
        {
            return new ExtendedPropertyDefinition(yourPropertySetId, yourPropertySetId.ToString(), MapiPropertyType.String);
        }
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        public bool SendEmail(SendEmailRequest req)
        {
            string err = string.Empty;
            SendStatus st = null;

            try
            {
                st = SendEmailPopup(ref req, ref err);
                return true;
            }
            catch (Exception exception)
            {
                int Event_id = 5017;
                err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.ToString();
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

        }
        public void SendEmail(object req)
        {
            SendEmailRequest emailRequest = req as SendEmailRequest;
            if (emailRequest != null)
            {
                SendEmail(emailRequest);
            }

        }

        public SendStatus SendEmail(SendEmailRequest req, out string err)
        {
            StringBuilder sbErrorMessage = new StringBuilder();
            SendStatus SendSatus = new SendStatus();
            bool status = false;
            string emailBody = string.Empty;
            err = string.Empty;
            bool ScheduledEmailRequest = false;
            Common.Table.TemplateEmail templateEmail = new Common.Table.TemplateEmail();
            List<Common.Table.TemplateEmailRecipient> emailRecipients = new List<Common.Table.TemplateEmailRecipient>();

            //get prospect alert id 
            if (req.PropsectTaskId > 0)
            {
                try
                {
                    req.PropsectAlertId = m_da.GetProspectAlertId(req.PropsectTaskId);
                }
                catch (Exception exception)
                {
                    int Event_id = 5019;
                    string errorMessage = "SendEmail, Exception: " + exception.ToString();
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, errorMessage, EventLogEntryType.Warning, Event_id, Category);
                }
            }

            SendSatus.req = req;

            #region neo 2011-01-30

            string sEmailBodyTemplate = string.Empty;
            SendSatus.SendFailedAndRemoveEmailQue = true;
            if (req.EmailTemplId == 0)   // not use email template, use plain text email
            {
                if (req.EmailBody != null && req.EmailBody.Length > 0)
                {
                    sEmailBodyTemplate = Encoding.UTF8.GetString(req.EmailBody);
                }

                if (req.AppendPictureSignature == true)
                {
                    sEmailBodyTemplate += "<br /><p>&lt;@DB-Sender Picture@&gt;</p><p>&lt;@DB-Sender Signature@&gt;</p>";
                }
            }
            else // use email template
            {
                ScheduledEmailRequest = true;
                try
                {
                    templateEmail = m_da.GetEmailTemplateByTemplateId(req.EmailTemplId, out err);
                    if (templateEmail.FromUserRoles > 0)
                    {
                        int userId = 0;
                        userId = Getuserid(req.FileId, templateEmail.FromUserRoles, ref err);
                        if (userId > 0)
                            req.UserId = userId;
                    }

                    SendSatus.FromUser = req.UserId;
                }
                catch (Exception exception)
                {
                    int Event_id = 5021;
                    err = "Exception:" + exception.ToString();
                    SendSatus.Message = err;
                    err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + err;
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    sbErrorMessage.AppendLine(err);
                    status = false;

                    //return SendSatus;
                }

                sEmailBodyTemplate = templateEmail.Content;
            }

            emailBody = GetEmailContent(sEmailBodyTemplate, req.FileId, req.LoanAlertId, req.UserId, req.ProspectId, req.PropsectTaskId, req.PropsectAlertId);
            string emailLogBody = Regex.Replace(emailBody, @"<img src=""cid:(?<cid>\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-Z0-9]{12}\b)""\s*?data=""(?<data>.*?)"" />", match => ImageReplacement(match, null, true), RegexOptions.IgnoreCase);
            SendSatus.EmailBody = emailLogBody;

            #endregion
            bool no_tos = false;
            List<MailAddress> tos_test = ToEmailList(req.ToContactIds, req.ToUserIds, req.ToEmails);

            try
            {
                //emailRecipients = m_da.GetTemplateEmailRecipient(req.EmailTemplId, out err);
                //emailRecipients = m_da.GetEmailRecipients(req.EmailTemplId, req.FileId, req.EmailQueId);                
                emailRecipients = m_da.GetEmailRecipients_cs(req.EmailTemplId, req.FileId, req.EmailQueId, ref no_tos, out err);
                //check for task owner
                CheckOwnerEmail(req, emailRecipients, tos_test);
                SendSatus.SendFailedAndRemoveEmailQue = true;
                if ((no_tos == true) && (tos_test.Count < 1))
                {
                    SendSatus.EmailId = req.EmailQueId;
                    SendSatus.Message = err.ToString();
                    SendSatus.LastSent = DateTime.Now;
                    return SendSatus;
                }
                else
                {
                    foreach (Common.Table.TemplateEmailRecipient ter in emailRecipients)
                    {
                        if (ter.RecipientType.ToString().Trim().ToUpper() == "TO")
                        {
                            SendSatus.ToEmails = ter.ToEmails;
                            SendSatus.ToUserIds = ter.ToUserIds;
                            SendSatus.ToContactIds = ter.ToContactIds;
                        }
                        if (ter.RecipientType.ToString().Trim().ToUpper() == "CC")
                        {
                            SendSatus.CCEmails = ter.CCEmails;
                            SendSatus.CCUserIds = ter.CCUserIds;
                            SendSatus.CCContactIds = ter.CCContactIds;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                err = string.Empty;
                err = "Exception:" + exception.ToString();
                SendSatus.Message = err;
                err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + err;
                Trace.TraceError(err);
                int Event_id = 5021;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                sbErrorMessage.AppendLine(err);
                SendSatus.SendFailedAndRemoveEmailQue = true;
                SendSatus.Message = err;
                SendSatus.LastSent = DateTime.Now;
                return SendSatus;
            }

            if (_emailServerSetting != null)
            {
                if (_emailServerSetting.EmailAlertsEnabled == false && req.LoanAlertId > 0)
                {
                    SendSatus.Message = "Email Alerts is not enabled. Alert Email has not been sent.";
                    SendSatus.Status = false;
                    status = false;
                    SendSatus.LastSent = DateTime.Now;
                    int Event_id = 5023;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, SendSatus.Message, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(SendSatus.Message);
                    sbErrorMessage.AppendLine(SendSatus.Message);
                    //return SendSatus;
                }
                var hasQuery = from recp in emailRecipients
                               where recp.TaskOwner == true
                               select recp;

                var ownerEmail = from recipient in hasQuery
                                 where recipient.RoleType == "Owner"
                                 select recipient;
                if (hasQuery.Count() > 0)
                {
                    if (req.LoanAlertId < 1)
                    {
                        SendSatus.Message = "The Loan Alert Id is not specified in the EmailQue record with TaskOwner=1";
                        SendSatus.Status = false;
                        status = false;
                        SendSatus.LastSent = DateTime.Now;
                        int Event_id = 5025;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, SendSatus.Message, EventLogEntryType.Warning, Event_id, Category);
                        Trace.TraceError(SendSatus.Message);
                        sbErrorMessage.AppendLine(SendSatus.Message);
                        //return SendSatus;
                    }
                    if (ownerEmail.Count() < 1)
                    {
                        SendSatus.Message = "The OwnerId is not specified in the LoanAlerts record with EmailQue.TaskOwner=1";
                        SendSatus.Status = false;
                        status = false;
                        int Event_id = 5027;
                        SendSatus.LastSent = DateTime.Now;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, SendSatus.Message, EventLogEntryType.Warning, Event_id, Category);
                        Trace.TraceError(SendSatus.Message);
                        sbErrorMessage.AppendLine(SendSatus.Message);
                        //return SendSatus;
                    }
                }

                MailAddress fromAddress = null;
                string senderName = string.Empty;
                string strFromAddress = string.Empty;
                string userAddress = m_da.GetUserEmailAddress(req.UserId, out err);

                if (req.UserId > 0 && !string.IsNullOrEmpty(userAddress))
                {
                    strFromAddress = userAddress;
                    var userInfo = m_da.Get_UserInfo(req.UserId, ref err);
                    if (userInfo != null)
                    {
                        senderName = string.Format("{0} {1}", userInfo.Firstname, userInfo.Lastname);
                    }
                }
                else if (!string.IsNullOrEmpty(templateEmail.FromEmailAddress))
                {
                    strFromAddress = templateEmail.FromEmailAddress;
                    senderName = templateEmail.SenderName;
                }
                else
                {
                    strFromAddress = _emailServerSetting.DefaultAlertEmail;
                }

                if (string.IsNullOrEmpty(strFromAddress))
                {
                    int Event_id = 5029;
                    SendSatus.Message = "The sender is not specified.";
                    SendSatus.Status = false;
                    status = false;
                    SendSatus.LastSent = DateTime.Now;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, SendSatus.Message, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(SendSatus.Message);
                    sbErrorMessage.AppendLine(SendSatus.Message);
                    //return SendSatus;
                }

                fromAddress = new MailAddress(strFromAddress, senderName);
                SendSatus.FromEmail = fromAddress.ToString();

                int port = 25;
                List<MailAddress> tos = ToEmailList(req.ToContactIds, req.ToUserIds, req.ToEmails);
                List<MailAddress> ccs = CcEmailList(req.CCContactIds, req.CCUserIds, req.CCEmails);
                List<MailAddress> bcc = new List<MailAddress>();

                if (tos.Count < 1)
                {
                    var query = from recp in emailRecipients
                                where recp.RecipientType.ToUpper() == TO && !string.IsNullOrEmpty(recp.EmailAddr)
                                select recp;
                    query.ToList().ForEach(recp =>
                    {
                        var emails = recp.EmailAddr.Split(Convert.ToChar(","));
                        tos.AddRange(emails.Select(email => new MailAddress(email)));
                    });

                }
                else
                {
                    var query = from recp in emailRecipients
                                where
                                    recp.RecipientType.ToUpper() == TO && !string.IsNullOrEmpty(recp.EmailAddr) &&
                                    recp.TaskOwner == true && recp.RoleType == "Owner"
                                select recp;
                    query.ToList().ForEach(recp =>
                    {
                        var emails = recp.EmailAddr.Split(Convert.ToChar(","));
                        tos.AddRange(emails.Select(email => new MailAddress(email)));
                    });
                }
                if (ccs.Count < 1)
                {
                    var query = from recp in emailRecipients
                                where recp.RecipientType.ToUpper() == CC && !string.IsNullOrEmpty(recp.EmailAddr)
                                select recp;
                    query.ToList().ForEach(recp =>
                    {
                        var emails = recp.EmailAddr.Split(Convert.ToChar(","));
                        ccs.AddRange(emails.Select(email => new MailAddress(email)));
                    });
                }
                else
                {
                    var query = from recp in emailRecipients
                                where
                                    recp.RecipientType.ToUpper() == CC && !string.IsNullOrEmpty(recp.EmailAddr) &&
                                    recp.TaskOwner == true && recp.RoleType == "Owner"
                                select recp;
                    query.ToList().ForEach(recp =>
                    {
                        var emails = recp.EmailAddr.Split(Convert.ToChar(","));
                        ccs.AddRange(emails.Select(email => new MailAddress(email)));
                    });
                }
                var queryBcc = from recp in emailRecipients
                               where recp.RecipientType.ToUpper() == BCC && !string.IsNullOrEmpty(recp.EmailAddr)
                               select recp;
                queryBcc.ToList().ForEach(recp =>
                {
                    var emails = recp.EmailAddr.Split(Convert.ToChar(","));
                    bcc.AddRange(emails.Select(email => new MailAddress(email)));
                });

                try
                {
                    #region neo 2011-01-30

                    string subject = string.Empty;
                    if (req.EmailTemplId == 0) // use plain text subject
                    {
                        subject = req.EmailSubject;
                    }
                    else // use email template
                    {
                        subject = templateEmail.Subject;
                    }

                    #endregion
                    subject = subject.Replace("<@", "&lt;@").Replace("@>", "@&gt;");
                    subject = GetEmailContent(subject, req.FileId, req.LoanAlertId, req.UserId, req.ProspectId, req.PropsectTaskId, req.PropsectAlertId);
                    SendSatus.Subject = subject;
                    if (SendSatus.ToContactIds == null)
                    {
                        SendSatus.ToContactIds = req.ToContactIds;
                    }
                    if (SendSatus.ToUserIds == null)
                    {
                        SendSatus.ToUserIds = req.ToUserIds;
                    }
                    if (SendSatus.ToEmails == null)
                    {
                        SendSatus.ToEmails = req.ToEmails;
                    }
                    var attachments = new Dictionary<string, Stream>();
                    if (req.Attachments != null)
                    {
                        SendSatus.Attachments = req.Attachments;
                        foreach (var attachment in req.Attachments)
                        {
                            var data = attachment.Value;
                            MemoryStream memoryStream = new MemoryStream(data.Length);
                            memoryStream.Write(data, 0, data.Length);
                            memoryStream.Position = 0;//very important flag.
                            attachments.Add(attachment.Key, memoryStream);
                        }
                    }

                    bool retryViaSmtp = false;

                    if (_emailServerSetting.SendEmailViaEWS)
                    {
                        if (ScheduledEmailRequest == false)
                        {
                            User userInfo = new User();
                            userInfo = m_da.Get_UserInfo(req.UserId, ref err);

                            if (userInfo == null || string.IsNullOrEmpty(userInfo.Password))
                            {
                                retryViaSmtp = true;
                                string userName = string.Empty;
                                if (userInfo != null)
                                    userName = userInfo.Username;
                                err = string.Format("Unable to send email via EWS because the sender <{0}> does not have a password in the database. Sent the email via SMTP.", userName);
                                Trace.TraceError(err);
                                int Event_id = 5031;
                                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                            }
                            else
                            {
                                string emailId = string.Empty;
                                //send email via ews
                                try
                                {
                                    status = SendEmail(subject, emailBody, fromAddress, _emailServerSetting.EmailRelayServer, port,
                                                       tos, ccs, bcc, userInfo, attachments, out emailId);
                                }
                                catch (Exception exception)
                                {
                                    retryViaSmtp = true;
                                    err = "Exchange Service is not avaliable." + " Exception: " + exception.ToString();
                                    int Event_id = 5033;
                                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                                }

                                if (status)
                                {
                                    SendSatus.ChainId = emailId;
                                    SendSatus.SequenceNumber = 0;
                                    SendSatus.EwsImported = false;
                                }
                                else
                                {
                                    retryViaSmtp = true;
                                }

                            }
                        }
                    }


                    int acnt = 0;
                    //check _emailServerSetting.SendEmailViaEWS == false or retryViaSmtp
                    if (_emailServerSetting.SendEmailViaEWS == false || retryViaSmtp)
                    {
                        //send email via smtp
                        status = SendEmail(subject, emailBody, fromAddress, _emailServerSetting.EmailRelayServer, port, tos, ccs, bcc, attachments);
                    }
                    else
                    {
                        if (ScheduledEmailRequest == true)
                        {
                            status = SendEmail(subject, emailBody, fromAddress, _emailServerSetting.EmailRelayServer, port, tos, ccs, bcc, attachments);
                        }
                    }

                }
                catch (ArgumentNullException exception)
                {
                    //SendEmailRequest ScheduledEmailRequest
                    string message = "Email Manager cannot send this email due to the lack of TO email recipients while processing <{0}>. Details: FileId=<{1}>, ProspectId=<{2}>, EmailTemplId=<{3}>, TaskAlertId=<{4}>, ProspectAlertId=<{5}>, RequestUserId=<{6}>";

                    string ptype = "SendEmailRequest";
                    if (req.EmailQueId > 0)
                    {
                        ptype = "ScheduledEmailRequest";
                    }

                    message = string.Format(message, ptype, req.FileId, req.ProspectId, req.EmailTemplId,
                                            req.LoanAlertId, req.PropsectAlertId, req.UserId);
                    err = message;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, message, EventLogEntryType.Warning);
                }
                catch (Exception exception)
                {
                    err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.ToString() + "\r\n\r\nStackTrace: " + exception.StackTrace;
                    Trace.TraceError(exception.Message);
                    int Event_id = 5035;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
            else
            {
                err = string.Format("Email Server Settings are required");
                SendSatus.Message = err;
                SendSatus.Status = false;
                status = false;
                int Event_id = 5037;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                //return SendSatus;
            }

            SendSatus.Status = status;
            SendSatus.Message = sbErrorMessage.ToString();
            SendSatus.LastSent = DateTime.Now;
            return SendSatus;
        }

        private void CheckOwnerEmail(SendEmailRequest req, List<Table.TemplateEmailRecipient> emailRecipients, List<MailAddress> tos_test)
        {
            if (req.LoanAlertId > 0)
            {
                Table.TemplateEmailRecipient ownerRecipient = m_da.GetLoanAlertOwnerEmail(req.LoanAlertId);
                if (ownerRecipient != null)
                {
                    //for condition check
                    tos_test.Add(new MailAddress(ownerRecipient.ToEmails[0]));
                    if (emailRecipients != null && emailRecipients.Count == 0)
                    {
                        emailRecipients.Add(ownerRecipient);
                        return;
                    }

                    //emailRecipients.Add(ownerRecipient);
                    var query = from recipient in emailRecipients
                                where recipient.RecipientType == TO
                                select recipient;
                    var tcRecipient = query.SingleOrDefault();
                    if (tcRecipient != null)
                    {
                        List<string> toEmail = new List<string>();
                        if (tcRecipient.ToEmails != null)
                        {
                            toEmail.AddRange(tcRecipient.ToEmails);
                        }
                        toEmail.Add(ownerRecipient.ToEmails[0]);
                        tcRecipient.ToEmails = toEmail.ToArray();
                    }

                }
            }
        }
        private MailAddress GetSenderEmail(SendEmailRequest req, EmailTemplateRecord EmailTemplateR, out string err)
        {
            MailAddress fromEmailAddr = null;
            string strFromAddress = string.Empty;
            string senderName = string.Empty;
            err = string.Empty;
            string userAddress = string.Empty;
            User userInfo = null;
            try
            {
                if (req != null && req.UserId > 0)
                    userInfo = m_da.Get_UserInfo(req.UserId, ref err);

                if (userInfo != null && !string.IsNullOrEmpty(userInfo.Email))
                {
                    strFromAddress = userInfo.Email;
                    senderName = string.Format("{0} {1}", userInfo.Firstname, userInfo.Lastname);
                }
                else if (EmailTemplateR != null && !string.IsNullOrEmpty(EmailTemplateR.FromEmailAddress))
                {
                    strFromAddress = EmailTemplateR.FromEmailAddress;
                    senderName = EmailTemplateR.SenderName;
                }
                else
                {
                    strFromAddress = _emailServerSetting.DefaultAlertEmail;
                }
                if (string.IsNullOrEmpty(strFromAddress))
                    return fromEmailAddr;
                if (string.IsNullOrEmpty(senderName))
                    senderName = strFromAddress;
                fromEmailAddr = new MailAddress(strFromAddress, senderName);
                return fromEmailAddr;
            }
            catch (Exception ex)
            {
                err = string.Format("GetSenderEmail, Exception: {0}", ex.ToString());
                int Event_id = 5039;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return fromEmailAddr;
            }
        }
        public SendStatus SendEmailQue(ref SendEmailRequest req, ref string err)
        {
            StringBuilder sbErrorMessage = new StringBuilder();
            SendStatus SendSatus = new SendStatus();
            bool status = false;
            int userid = 0;
            string emailBody = string.Empty;
            err = string.Empty;
            int Event_id = 5055;

            EmailTemplateRecord EmailTemplateR = new EmailTemplateRecord();
            Common.Table.TemplateEmail templateEmail = new Common.Table.TemplateEmail();
            List<Common.Table.TemplateEmailRecipient> emailRecipients = null;
            EmailQueRecord EmailQueR = new EmailQueRecord();

            if (_emailServerSetting == null)
            {
                SendSatus.Message = "No Email Server Settings found.";
                SendSatus.SendFailedAndRemoveEmailQue = true;
                SendSatus.EmailId = req.EmailQueId;
                SendSatus.LastSent = DateTime.Now;
                err = SendSatus.Message;
                SendSatus.Status = false;
                SendSatus.LastSent = DateTime.Now;
                return SendSatus;
            }

            if (_emailServerSetting.EmailAlertsEnabled == false && req.LoanAlertId > 0)
            {
                SendSatus.Message = "Email Alerts is not enabled. Alert Email has not been sent.";
                SendSatus.SendFailedAndRemoveEmailQue = true;
                SendSatus.EmailId = req.EmailQueId;
                SendSatus.LastSent = DateTime.Now;
                err = SendSatus.Message;
                SendSatus.Status = false;
                return SendSatus;
            }

            if (req.EmailQueId > 0)
            {
                try
                {
                    EmailQueR = GetEmailQueRecord(req.EmailQueId, ref err);
                    req.FileId = EmailQueR.FileId;
                    req.PropsectAlertId = EmailQueR.ProspectAlertId;
                    req.LoanAlertId = EmailQueR.LoanAlertId;
                    req.EmailTemplId = EmailQueR.EmailTmplId;
                }
                catch (Exception exception)
                {
                    string errorMessage = string.Format("Can not get Email Que for EmailQueId <0>", req.EmailQueId);
                    SendSatus.SendFailedAndRemoveEmailQue = true;
                    SendSatus.EmailId = req.EmailQueId;
                    SendSatus.LastSent = DateTime.Now;
                    SendSatus.Message = errorMessage + exception.Message;
                    err = SendSatus.Message;
                    SendSatus.Status = false;
                    Event_id = 5041;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return SendSatus;
                }
            }

            SendSatus.FromUser = req.UserId;
            SendSatus.req = req;

            string sEmailBodyTemplate = string.Empty;
            SendSatus.SendFailedAndRemoveEmailQue = true;

            if (req.EmailTemplId == 0)
            {
                if (EmailQueR.EmailBody != null && EmailQueR.EmailBody.Length > 0)
                {
                    sEmailBodyTemplate = EmailQueR.EmailBody;
                }

                if (req.AppendPictureSignature == true)
                {
                    sEmailBodyTemplate += "<br /><p>&lt;@DB-Sender Picture@&gt;</p><p>&lt;@DB-Sender Signature@&gt;</p>";
                }
            }
            else
            {
                try
                {
                    EmailTemplateR = GetEmailTemplateRecord(req.EmailTemplId, ref err);
                    templateEmail = m_da.GetEmailTemplateByTemplateId(req.EmailTemplId, out err);
                    EmailTemplateR.Content = templateEmail.Content;
                    emailRecipients = m_da.GetEmailRecipients(req.EmailTemplId, req.FileId, req.EmailQueId);
                }
                catch (Exception exception)
                {
                    string errorMessage = string.Format("Can not get Email Template Record for EmailTemplId: {0}", req.EmailTemplId.ToString());
                    SendSatus.SendFailedAndRemoveEmailQue = true;
                    SendSatus.EmailId = req.EmailQueId;
                    SendSatus.LastSent = DateTime.Now;
                    SendSatus.Message = errorMessage + exception.Message;
                    err = SendSatus.Message;
                    SendSatus.Status = false;
                    Event_id = 5043;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return SendSatus;
                }

                sEmailBodyTemplate = EmailTemplateR.Content;

                if (EmailQueR.EmailBody != null && EmailQueR.EmailBody.Length > 0)
                {
                    sEmailBodyTemplate = EmailQueR.EmailBody;
                }

                if (EmailTemplateR.FromUserRoles > 0)
                {
                    userid = Getuserid(req.FileId, EmailTemplateR.FromUserRoles, ref err);
                }

                if (userid > 0)
                {
                    req.UserId = userid;
                }

                SendSatus.FromUser = req.UserId;
                SendSatus.req = req;
            }
            MailAddress fromAddress = GetSenderEmail(req, EmailTemplateR, out err);
            if (fromAddress == null)
            {
                err = string.Format("Unable to find the sender email for FileId {0}, EmailQueId {1}, EmailTemplId {2}", req.FileId, req.EmailQueId, req.EmailTemplId);
                SendSatus.Message = err;
                SendSatus.SendFailedAndRemoveEmailQue = true;
                SendSatus.EmailId = req.EmailQueId;
                SendSatus.LastSent = DateTime.Now;
                SendSatus.Status = false;
                return SendSatus;
            }
            SendSatus.FromEmail = fromAddress.Address.ToString();
            emailBody = GetEmailContent(sEmailBodyTemplate, req.FileId, req.LoanAlertId, req.UserId, req.ProspectId, req.PropsectTaskId, req.PropsectAlertId);
            string emailLogBody = Regex.Replace(emailBody, @"<img src=""cid:(?<cid>\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-Z0-9]{12}\b)""\s*?data=""(?<data>.*?)"" />", match => ImageReplacement(match, null, true), RegexOptions.IgnoreCase);
            SendSatus.EmailBody = emailLogBody;

            bool no_tos = false;
            List<MailAddress> tos_test = ToEmailList(req.ToContactIds, req.ToUserIds, req.ToEmails);
            List<string> toEmails = req.ToEmails == null ? (new List<string>()) : req.ToEmails.ToList();
            List<int> toUserIds = req.ToUserIds == null ? (new List<int>()) : req.ToUserIds.ToList();
            List<int> ccUserIds = req.CCUserIds == null ? (new List<int>()) : req.CCUserIds.ToList();
            List<int> toContactIds = req.ToContactIds == null ? (new List<int>()) : req.ToContactIds.ToList();
            List<int> ccContactIds = req.CCContactIds == null ? (new List<int>()) : req.CCContactIds.ToList();
            List<string> ccEmails = req.CCEmails == null ? (new List<string>()) : req.CCEmails.ToList();
            try
            {
                //emailRecipients = m_da.GetEmailQueRecipients_cs(req.EmailTemplId, req.LoanAlertId, req.FileId, req.EmailQueId, ref no_tos, out err);
                foreach (Table.TemplateEmailRecipient tRecipient in emailRecipients)
                {
                    if (tRecipient == null || string.IsNullOrEmpty(tRecipient.RecipientType))
                        continue;
                    if (tRecipient.RecipientType.ToUpper() == "TO" &&
                        (tRecipient.ToEmails != null && tRecipient.ToEmails.Length > 0))
                    {
                        no_tos = false;
                        break;
                    }
                }
                no_tos = (tos_test != null && tos_test.Count > 0) ? false : no_tos;
                SendSatus.SendFailedAndRemoveEmailQue = true;
                if (no_tos == true)
                {
                    SendSatus.EmailId = req.EmailQueId;
                    SendSatus.Message = "lack of TO email recipients";
                    SendSatus.LastSent = DateTime.Now;
                    err = SendSatus.Message;
                    SendSatus.Status = false;
                    return SendSatus;
                }

                //SendSatus.SendFailedAndRemoveEmailQue = false;

                foreach (Common.Table.TemplateEmailRecipient ter in emailRecipients)
                {
                    if (ter.RecipientType.ToString().Trim().ToUpper() == "TO")
                    {
                        if (ter.ToEmails != null && ter.ToEmails.Length > 0 && !string.IsNullOrEmpty(ter.ToEmails[0]))
                            toEmails.Add(ter.ToEmails[0]);
                        if (ter.ToUserIds != null && ter.ToUserIds.Length > 0 && ter.ToUserIds[0] > 0)
                            toUserIds.Add(ter.ToUserIds[0]);
                        if (ter.ToContactIds != null && ter.ToContactIds.Length > 0 && ter.ToContactIds[0] > 0)
                            toContactIds.Add(ter.ToContactIds[0]);
                    }
                    if (ter.RecipientType.ToString().Trim().ToUpper() == "CC")
                    {
                        if (ter.ToEmails != null && ter.ToEmails.Length > 0 && !string.IsNullOrEmpty(ter.ToEmails[0]))
                            ccEmails.Add(ter.ToEmails[0]);
                        if (ter.ToUserIds != null && ter.ToUserIds.Length > 0 && ter.ToUserIds[0] > 0)
                            ccUserIds.Add(ter.ToUserIds[0]);
                        if (ter.ToContactIds != null && ter.ToContactIds.Length > 0 && ter.ToContactIds[0] > 0)
                            ccContactIds.Add(ter.ToContactIds[0]);
                    }
                }

                SendSatus.ToUserIds = toUserIds.ToArray();
                SendSatus.ToContactIds = toContactIds.ToArray();
                SendSatus.ToEmails = toEmails.ToArray();
                SendSatus.CCUserIds = ccUserIds.ToArray();
                SendSatus.CCContactIds = ccContactIds.ToArray();
                SendSatus.CCEmails = ccEmails.ToArray();
            }
            catch (Exception exception)
            {
                string errorMessage = string.Format("Can not get Email Recipients Record for EmailTemplId: {0} ", req.EmailTemplId.ToString());
                SendSatus.SendFailedAndRemoveEmailQue = true;
                SendSatus.EmailId = req.EmailQueId;
                sbErrorMessage.Append(errorMessage + exception.ToString());
                SendSatus.Status = false;
                SendSatus.LastSent = DateTime.Now;
                SendSatus.Message = errorMessage;
                err = SendSatus.Message;
                //int Event_id = 5045;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, errorMessage, EventLogEntryType.Warning, Event_id, Category);
                return SendSatus;
            }
            int port = 25;
            List<MailAddress> tos = ToEmailList(toContactIds.ToArray(), toUserIds.ToArray(), toEmails.ToArray());
            List<MailAddress> ccs = CcEmailList(ccContactIds.ToArray(), ccUserIds.ToArray(), ccEmails.ToArray());
            List<MailAddress> bcc = new List<MailAddress>();

            try
            {
                string subject = string.Empty;

                if (req.EmailTemplId == 0)
                {
                    subject = req.EmailSubject;
                }
                else
                {
                    subject = EmailTemplateR.Subject;
                }
                subject = subject.Replace("<@", "&lt;@").Replace("@>", "@&gt;");
                subject = GetEmailContent(subject, req.FileId, req.LoanAlertId, req.UserId, req.ProspectId, req.PropsectTaskId, req.PropsectAlertId);
                SendSatus.Subject = subject;

                if (SendSatus.ToContactIds == null)
                {
                    SendSatus.ToContactIds = req.ToContactIds;
                }
                if (SendSatus.ToUserIds == null)
                {
                    SendSatus.ToUserIds = req.ToUserIds;
                }
                if (SendSatus.ToEmails == null)
                {
                    SendSatus.ToEmails = req.ToEmails;
                }
                if (SendSatus.CCContactIds == null)
                {
                    SendSatus.CCContactIds = req.CCContactIds;
                }
                if (SendSatus.CCUserIds == null)
                {
                    SendSatus.CCUserIds = req.CCUserIds;
                }
                if (SendSatus.CCEmails == null)
                {
                    SendSatus.CCEmails = req.CCEmails;
                }

                var attachments = new Dictionary<string, Stream>();
                if (req.Attachments != null)
                {
                    SendSatus.Attachments = req.Attachments;
                    foreach (var attachment in req.Attachments)
                    {
                        var data = attachment.Value;
                        MemoryStream memoryStream = new MemoryStream(data.Length);
                        memoryStream.Write(data, 0, data.Length);
                        memoryStream.Position = 0;//very important flag.
                        attachments.Add(attachment.Key, memoryStream);
                    }
                }

                bool retryViaSmtp = false;
                if (req.UserId <= 0)
                    retryViaSmtp = true;
                if (_emailServerSetting.SendEmailViaEWS && req.UserId > 0)
                {
                    User userInfo = new User();
                    userInfo = m_da.Get_UserInfo(req.UserId, ref err);

                    if (userInfo == null || string.IsNullOrEmpty(userInfo.Password))
                    {
                        retryViaSmtp = true;
                    }
                    else
                    {
                        string emailId = string.Empty;         //send email via ews

                        try
                        {
                            status = SendEmail(subject, emailBody, fromAddress, _emailServerSetting.EmailRelayServer, port,
                                               tos, ccs, bcc, userInfo, attachments, out emailId);
                        }
                        catch (Exception exception)
                        {
                            sbErrorMessage.Append(exception.ToString());
                            retryViaSmtp = true;
                        }

                        if (status)
                        {
                            SendSatus.ChainId = emailId;
                            SendSatus.EmailUniqueId = emailId;
                            SendSatus.SequenceNumber = 0;
                            SendSatus.EwsImported = false;
                            sbErrorMessage.Remove(0, sbErrorMessage.Length);
                            SendSatus.LastSent = DateTime.Now;
                            return SendSatus;
                        }
                        else
                        {
                            retryViaSmtp = true;
                        }

                    }

                }

                if (_emailServerSetting.SendEmailViaEWS == false || retryViaSmtp)
                {
                    status = SendEmail(subject, emailBody, fromAddress, _emailServerSetting.EmailRelayServer, port, tos, ccs, bcc, attachments);
                }

            }
            catch (ArgumentNullException ex)
            {
                sbErrorMessage.Append("lack of TO email recipients");
                status = false;
                SendSatus.LastSent = DateTime.Now;
                return SendSatus;
            }
            catch (Exception exception)
            {
                sbErrorMessage.Append(exception.Message);
                status = false;
                SendSatus.LastSent = DateTime.Now;
                return SendSatus;
            }
            finally
            {
                SendSatus.EmailId = req.EmailQueId;
                SendSatus.SendFailedAndRemoveEmailQue = true;
                SendSatus.Status = status;
                SendSatus.Message = sbErrorMessage.ToString();
                err = SendSatus.Message;
                SendSatus.LastSent = DateTime.Now;
                if (status == true)
                {
                    //string errMsg = string.Format("SendEmailQueue, processed FileId={0}, EmailQueId={1}, Status={2} ", req.FileId, req.EmailQueId, SendSatus.Status.ToString());
                    //Event_id = 9038;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Information, Event_id, Category);
                }
                else
                {
                    string errMsg = string.Format("SendEmailQueue, processed FileId={0}, EmailQueId={1}, Status={2}, error: {3}  ", req.FileId, req.EmailQueId, SendSatus.Status.ToString(), err);
                    Event_id = 9038;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                }
            }
            return SendSatus;
        }

        public SendStatus SendEmailPopup(ref SendEmailRequest req, ref string err)
        {
            err = "";
            StringBuilder sbErrorMessage = new StringBuilder();
            SendStatus SendSatus = new SendStatus();
            bool status = false;
            string emailBody = string.Empty;
            err = string.Empty;
            bool ScheduledEmailRequest = false;
            Common.Table.TemplateEmail templateEmail = new Common.Table.TemplateEmail();
            List<Common.Table.TemplateEmailRecipient> emailRecipients = new List<Common.Table.TemplateEmailRecipient>();

            //get prospect alert id 
            if (req.PropsectTaskId > 0)
            {
                try
                {
                    req.PropsectAlertId = m_da.GetProspectAlertId(req.PropsectTaskId);
                }
                catch (Exception exception)
                {
                    string errorMessage = string.Format("Can not get prospect alert Id for prospect task Id <0>", req.PropsectTaskId);
                    int Event_id = 5049;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, errorMessage, EventLogEntryType.Warning, Event_id, Category);
                }
            }
            SendSatus.FromUser = req.UserId;
            SendSatus.req = req;

            #region neo 2011-01-30

            string sEmailBodyTemplate = string.Empty;
            SendSatus.SendFailedAndRemoveEmailQue = true;
            if (req.EmailTemplId == 0)   // not use email template, use plain text email
            {
                if (req.EmailBody != null && req.EmailBody.Length > 0)
                {
                    sEmailBodyTemplate = Encoding.UTF8.GetString(req.EmailBody);
                }

                if (req.AppendPictureSignature == true)
                {
                    sEmailBodyTemplate += "<br /><p>&lt;@DB-Sender Picture@&gt;</p><p>&lt;@DB-Sender Signature@&gt;</p>";
                }
            }
            else // use email template
            {
                ScheduledEmailRequest = true;
                try
                {
                    templateEmail = m_da.GetEmailTemplateByTemplateId(req.EmailTemplId, out err);
                    if (templateEmail.FromUserRoles > 0)
                    {
                        int userId = 0;
                        userId = Getuserid(req.FileId, templateEmail.FromUserRoles, ref err);
                        if (userId > 0)
                            req.UserId = userId;
                    }

                    SendSatus.FromUser = req.UserId;
                }
                catch (Exception exception)
                {
                    err = "Exception:" + exception.ToString();
                    SendSatus.Message = err;
                    err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.ToString() + "\r\n\r\nStackTrace: " + exception.StackTrace;
                    Trace.TraceError(exception.Message);
                    int Event_id = 5051;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    sbErrorMessage.AppendLine(err);
                    status = false;

                    //return SendSatus;
                }

                sEmailBodyTemplate = templateEmail.Content;
            }

            emailBody = GetEmailContent(sEmailBodyTemplate, req.FileId, req.LoanAlertId, req.UserId, req.ProspectId, req.PropsectTaskId, req.PropsectAlertId);
            string emailLogBody = Regex.Replace(emailBody, @"<img src=""cid:(?<cid>\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-Z0-9]{12}\b)""\s*?data=""(?<data>.*?)"" />", match => ImageReplacement(match, null, true), RegexOptions.IgnoreCase);
            SendSatus.EmailBody = emailLogBody;

            #endregion
            bool no_tos = false;
            List<MailAddress> tos_test = ToEmailList(req.ToContactIds, req.ToUserIds, req.ToEmails);
            List<MailAddress> tos_extra = new List<MailAddress>();
            List<MailAddress> ccs_extra = new List<MailAddress>();

            if (req.EmailTemplId > 0)
            {
                try
                {
                    emailRecipients = m_da.GetEmailRecipients_cs(req.EmailTemplId, req.FileId, req.EmailQueId, ref no_tos, out err);
                    //check for task owner
                    CheckOwnerEmail(req, emailRecipients, tos_test);
                    SendSatus.SendFailedAndRemoveEmailQue = true;
                    if ((no_tos == true) && (tos_test.Count < 1))
                    {

                        SendSatus.EmailId = req.EmailQueId;
                        SendSatus.Message = err.ToString();
                        SendSatus.LastSent = DateTime.Now;
                        return SendSatus;
                    }
                    else
                    {
                        //SendSatus.SendFailedAndRemoveEmailQue = false;
                        foreach (Common.Table.TemplateEmailRecipient ter in emailRecipients)
                        {
                            if (ter.RecipientType.ToString().Trim().ToUpper() == "TO")
                            {
                                SendSatus.ToEmails = ter.ToEmails;
                                SendSatus.ToUserIds = ter.ToUserIds;
                                SendSatus.ToContactIds = ter.ToContactIds;
                                tos_extra = ToEmailList(ter.ToContactIds, ter.ToUserIds, ter.ToEmails);
                            }
                            if (ter.RecipientType.ToString().Trim().ToUpper() == "CC")
                            {
                                SendSatus.CCEmails = ter.CCEmails;
                                SendSatus.CCUserIds = ter.CCUserIds;
                                SendSatus.CCContactIds = ter.CCContactIds;
                                ccs_extra = CcEmailList(ter.CCContactIds, ter.CCUserIds, ter.CCEmails);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    err = string.Empty;
                    err = "Exception:" + exception.ToString();
                    SendSatus.Message = err;
                    err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.ToString() + "\r\n\r\nStackTrace: " + exception.StackTrace;
                    Trace.TraceError(exception.Message);
                    int Event_id = 5053;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    sbErrorMessage.AppendLine(err);
                    SendSatus.SendFailedAndRemoveEmailQue = true;
                    SendSatus.Message = err.ToString();
                    SendSatus.LastSent = DateTime.Now;
                    return SendSatus;
                }
            }

            if (_emailServerSetting != null)
            {
                MailAddress fromAddress = null;
                string strFromAddress = string.Empty;
                string senderName = string.Empty;
                //string userAddress = m_da.GetUserEmailAddress(req.UserId, out err);

                //if (req.UserId > 0 && !string.IsNullOrEmpty(userAddress))
                if (req.UserId > 0)
                {
                    strFromAddress = m_da.GetUserEmailAddress(req.UserId, out err);
                    var userInfo = m_da.Get_UserInfo(req.UserId, ref err);
                    if (userInfo != null)
                    {
                        senderName = string.Format("{0} {1}", userInfo.Firstname, userInfo.Lastname);
                    }
                }
                else if (!string.IsNullOrEmpty(templateEmail.FromEmailAddress))
                {
                    strFromAddress = templateEmail.FromEmailAddress;
                    senderName = templateEmail.SenderName;
                }

                if (string.IsNullOrEmpty(strFromAddress))
                {
                    strFromAddress = _emailServerSetting.DefaultAlertEmail;
                }

                if (string.IsNullOrEmpty(strFromAddress))
                {
                    SendSatus.Message = "The sender is not specified.";
                    SendSatus.Status = false;
                    status = false;
                    SendSatus.LastSent = DateTime.Now;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, SendSatus.Message, EventLogEntryType.Warning);
                    Trace.TraceError(SendSatus.Message);
                    sbErrorMessage.AppendLine(SendSatus.Message);
                }

                fromAddress = new MailAddress(strFromAddress, senderName);
                SendSatus.FromEmail = fromAddress.ToString();

                int port = 25;
                List<MailAddress> tos = ToEmailList(req.ToContactIds, req.ToUserIds, req.ToEmails);
                List<MailAddress> ccs = CcEmailList(req.CCContactIds, req.CCUserIds, req.CCEmails);
                List<MailAddress> bcc = new List<MailAddress>();

                if (req.EmailTemplId > 0)
                {
                    if (tos.Count < 1)
                    {
                        tos = tos_extra;
                    }
                    else
                    {
                        if (tos_extra.Count() > 0)
                        {
                            tos.AddRange(tos_extra);
                        }
                    }

                    if (ccs.Count < 1)
                    {
                        ccs = ccs_extra;
                    }
                    else
                    {
                        if (ccs_extra.Count() > 0)
                        {
                            ccs.AddRange(ccs_extra);
                        }
                    }
                }

                try
                {
                    string subject = string.Empty;
                    if (req.EmailTemplId == 0)
                    {
                        subject = req.EmailSubject;
                    }
                    else
                    {
                        subject = templateEmail.Subject;
                    }
                    subject = subject.Replace("<@", "&lt;@").Replace("@>", "@&gt;");
                    subject = GetEmailContent(subject, req.FileId, req.LoanAlertId, req.UserId, req.ProspectId, req.PropsectTaskId, req.PropsectAlertId);
                    SendSatus.Subject = subject;
                    if (SendSatus.ToContactIds == null)
                    {
                        SendSatus.ToContactIds = req.ToContactIds;
                    }
                    if (SendSatus.ToUserIds == null)
                    {
                        SendSatus.ToUserIds = req.ToUserIds;
                    }
                    if (SendSatus.ToEmails == null)
                    {
                        SendSatus.ToEmails = req.ToEmails;
                    }
                    if (SendSatus.CCUserIds == null)
                    {
                        SendSatus.CCUserIds = req.CCUserIds;
                    }
                    if (SendSatus.CCContactIds == null)
                    {
                        SendSatus.CCContactIds = req.CCContactIds;
                    }
                    if (SendSatus.CCEmails == null)
                    {
                        SendSatus.CCEmails = req.CCEmails;
                    }
                    var attachments = new Dictionary<string, Stream>();
                    if (req.Attachments != null)
                    {
                        SendSatus.Attachments = req.Attachments;
                        foreach (var attachment in req.Attachments)
                        {
                            var data = attachment.Value;
                            MemoryStream memoryStream = new MemoryStream(data.Length);
                            memoryStream.Write(data, 0, data.Length);
                            memoryStream.Position = 0;//very important flag.
                            attachments.Add(attachment.Key, memoryStream);
                        }
                    }

                    bool retryViaSmtp = false;

                    if (_emailServerSetting.SendEmailViaEWS)
                    {
                        User userInfo = new User();
                        userInfo = m_da.Get_UserInfo(req.UserId, ref err);

                        if (userInfo == null || string.IsNullOrEmpty(userInfo.Password))
                        {
                            retryViaSmtp = true;
                            string userName = string.Empty;
                            if (userInfo != null)
                                userName = userInfo.Username;
                            err = string.Format("Unable to send email via EWS because the sender <{0}> does not have a password in the database. Sent the email via SMTP.", userName);
                            Trace.TraceError(err);
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, 5091);
                        }
                        else
                        {
                            string emailId = string.Empty;
                            //send email via ews
                            try
                            {
                                status = SendEmail(subject, emailBody, fromAddress, _emailServerSetting.EmailRelayServer, port,
                                                   tos, ccs, bcc, userInfo, attachments, out emailId);
                            }
                            catch (Exception exception)
                            {
                                retryViaSmtp = true;
                                err = "Exchange Service is not avaliable." + " Exception: " + exception.ToString();
                            }

                            if (status)
                            {
                                SendSatus.ChainId = emailId;
                                SendSatus.EmailUniqueId = emailId;
                                SendSatus.SequenceNumber = 0;
                                SendSatus.EwsImported = false;
                            }
                            else
                            {
                                retryViaSmtp = true;
                            }
                        }
                    }

                    int acnt = 0;

                    MailAddress[] MA = tos.ToArray();

                    acnt = MA.Length;

                    String[] STR = new String[acnt];

                    for (int i = 0; i < acnt; i++)
                    {
                        STR[i] = MA[i].ToString();
                    }

                    SendSatus.ToEmails = STR;

                    if (_emailServerSetting.SendEmailViaEWS == false || retryViaSmtp)
                    {
                        status = SendEmail(subject, emailBody, fromAddress, _emailServerSetting.EmailRelayServer, port, tos, ccs, bcc, attachments);
                    }

                }
                catch (ArgumentNullException exception)
                {
                    string message = "Email Manager cannot send this email due to the lack of TO email recipients while processing <{0}>. Details: FileId=<{1}>, ProspectId=<{2}>, EmailTemplId=<{3}>, TaskAlertId=<{4}>, ProspectAlertId=<{5}>, RequestUserId=<{6}>";
                    string ptype = "SendEmailPopup";
                    message = string.Format(message, ptype, req.FileId, req.ProspectId, req.EmailTemplId,
                                            req.LoanAlertId, req.PropsectAlertId, req.UserId);
                    err = message;
                    int Event_id = 5055;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
                catch (Exception exception)
                {
                    err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.ToString() + "\r\n\r\nStackTrace: " + exception.StackTrace;
                    int Event_id = 5057;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
            else
            {
                err = string.Format("Email Server Settings are required");
                SendSatus.Message = err;
                SendSatus.Status = false;
                status = false;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
            }

            SendStatus st = SendSatus;
            st.LastSent = DateTime.Now;
            string toUser = string.Empty;
            if (req.ToUserIds != null && req.ToUserIds.Length > 0)
            {
                toUser = req.ToUserIds.Select(a => a.ToString()).Aggregate((a, b) => a + ";" + b);
            }

            string toContact = string.Empty;
            if (req.ToContactIds != null && req.ToContactIds.Length > 0)
            {
                toContact = req.ToContactIds.Select(a => a.ToString()).Aggregate((a, b) => a + ";" + b);
            }

            string ccUser = string.Empty;
            if (req.CCUserIds != null && req.CCUserIds.Length > 0)
            {
                ccUser = req.CCUserIds.Select(a => a.ToString()).Aggregate((a, b) => a + ";" + b);
            }

            string ccContact = string.Empty;
            if (req.CCContactIds != null && req.CCContactIds.Length > 0)
            {
                ccContact = req.CCContactIds.Select(a => a.ToString()).Aggregate((a, b) => a + ";" + b);
            }

            try
            {
                string toEmails = string.Empty;
                if (st.ToEmails != null && st.ToEmails.Length > 0)
                {
                    toEmails = st.ToEmails.Aggregate((a, b) => a + ";" + b);
                }

                string ccEmails = string.Empty;
                if (st.CCEmails != null && st.CCEmails.Length > 0)
                {
                    ccEmails = st.CCEmails.Aggregate((a, b) => a + ";" + b);
                }

                int prospectId = 0;
                int prospectAlertId = 0;
                if (st.req != null)
                {
                    prospectId = st.req.ProspectId;
                    prospectAlertId = st.req.PropsectAlertId;
                }
                int emailLogId = 0;
                if (m_da.EmailLog(toUser, toContact, req.EmailTemplId, status, err, req.LoanAlertId, default(int), req.FileId, st.FromEmail, st.FromUser, default(int), st.EmailBody, st.Subject, default(bool), toEmails, prospectId, prospectAlertId, st.ChainId, st.SequenceNumber, st.EwsImported, ccEmails, ccUser, ccContact, st.EmailUniqueId, ref err, ref emailLogId) == false)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }

                if (emailLogId > 0 && st.Attachments != null)
                {
                    foreach (var attachment in st.Attachments)
                    {
                        var nt = attachment.Key.Split(new[] { '.' });
                        var name = nt[0];
                        var ext = nt[1];
                        m_da.SaveEmailLogAttachment(emailLogId, req.FileId, name, ext, attachment.Value);
                    }
                }

            }
            catch (Exception exception)
            {
                err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.ToString() + "\r\n\r\nStackTrace: " + exception.StackTrace;
                int Event_id = 5059;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }

            SendSatus.Status = status;
            SendSatus.Message = sbErrorMessage.ToString();
            SendSatus.LastSent = DateTime.Now;
            return SendSatus;
        }

        /// <summary>
        /// Previews the email.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>

        public EmailPreviewResponse PreviewEmail(EmailPreviewRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            EmailPreviewResponse previewEmail = new EmailPreviewResponse();
            previewEmail.resp = new RespHdr();

            string emailContent = string.Empty;
            try
            {
                #region neo 2011-02-02

                string sEmailBodyTemplate = string.Empty;

                if (req.EmailTemplId == 0)   // not use email template, use plain text email
                {
                    if (req.EmailBody != null && req.EmailBody.Length > 0)
                    {
                        sEmailBodyTemplate = Encoding.UTF8.GetString(req.EmailBody);
                    }

                    if (req.AppendPictureSignature == true)
                    {
                        sEmailBodyTemplate += "<br /><p>&lt;@DB-Sender Picture@&gt;</p><p>&lt;@DB-Sender Signature@&gt;</p>";
                    }
                }
                else // use email template
                {
                    Common.Table.TemplateEmail templateEmail = m_da.GetEmailTemplateByTemplateId(req.EmailTemplId, out err);
                    sEmailBodyTemplate = templateEmail.Content;
                }

                emailContent = GetEmailContent(sEmailBodyTemplate, req.FileId, req.LoanAlertId, req.UserId,
                                               req.ProspectId, req.PropsectTaskId, 0);
                status = true;
                emailContent = Regex.Replace(emailContent, @"<img src=""cid:(?<cid>\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-Z0-9]{12}\b)""\s*?data=""(?<data>.*?)"" />", match => ImageReplacement(match, null, true), RegexOptions.IgnoreCase);
                byte[] byt = System.Text.Encoding.UTF8.GetBytes(emailContent);
                previewEmail.EmailHtmlContent = byt;

                #endregion
            }
            catch (ArgumentException ex)
            {
                err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + ex.Message + "\r\n\r\nStackTrace: " + ex.StackTrace;
                Trace.TraceError(ex.Message);
                int Event_id = 5061;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return previewEmail;
            }

            previewEmail.resp.Successful = status;
            previewEmail.resp.StatusInfo = MethodBase.GetCurrentMethod() + ", " + err;
            previewEmail.resp.RequestId = reqId;
            return previewEmail;

        }

        /// <summary>
        /// Gets the content of the email.
        /// </summary>
        /// <param name="emailTemplate">The email template.</param>
        /// <param name="fileId">The file id.</param>
        /// <param name="loanAlertId">The loan alert id.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        private static string GetEmailContent(string emailTemplate, int fileId, int loanAlertId, int userID, int prospectId, int propsectTaskId, int propsectAlertId)
        {
            short Category = 60;
            string err = string.Empty;
            string emailContent = string.Empty;
            string template;

            try
            {
                if (string.IsNullOrEmpty(emailTemplate))
                {
                    return emailContent;
                }
                template = emailTemplate;
                //if (appendPictureSignature == true)
                //{
                //    if (!Regex.IsMatch(template, @"db\s*?-\s*?(Sender Picture|Sender Signature)", RegexOptions.IgnoreCase))
                //    {
                //        template = template + "<br /><p>&lt;@DB-Sender Picture@&gt;<br />&lt;@DB-Sender Signature@&gt;<br /></p>";
                //    }
                //}
                //emailContent = Regex.Replace(template, @"&lt;\s*?@\s*?(?:(?<type>Previous|DB)-)?(?:(?<act>.*?)\s*?)(?:\((?<field>\d+)\))?\s*?@\s*?&gt;", match => ComputeReplacement(match, fileId, loanAlertId, userID), RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                //template = Regex.Replace(template, @"\r\n", "", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
                emailContent = Regex.Replace(template, @"&lt;\s*?@\s*?(?:(?<type>Previous|DB)-)?(?:(?<act>.*?)\s*?)(?:\((?<field>\d+)\))?\s*?@\s*?&gt;", match => ComputeReplacement(match, fileId, loanAlertId, userID, prospectId, propsectTaskId, propsectTaskId), RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
                return emailContent;
            }
            catch (Exception exception)
            {
                err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.ToString() + "\r\n\r\nStackTrace: " + exception.StackTrace;
                Trace.TraceError(exception.Message);
                int Event_id = 5063;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            return emailContent;
        }

        /// <summary>
        /// Processes the email que.
        /// </summary>
        /// <returns></returns>
        public bool ProcessEmailQue()
        {
            string err = string.Empty;
            string ptype = "SendEmailQue";
            List<EmailQue> lstEmailQue = m_da.GetEmalQueList(ref err);
            Dictionary<EmailQue, SendEmailRequest> sendEmailRequests = new Dictionary<EmailQue, SendEmailRequest>();
            Dictionary<EmailQue, SendStatus> sendStatus = new Dictionary<EmailQue, SendStatus>();
            SendStatus SendEmailStatus = new SendStatus();
            SendEmailRequest SendEmailRequestRecord = new SendEmailRequest();
            bool returnstatus = true;
            SendEmailRequest SER = new SendEmailRequest();

            if (lstEmailQue != null && lstEmailQue.Count > 0)
            {
                foreach (EmailQue emailQue in lstEmailQue)
                {
                    SendEmailRequestRecord = EmailQueToSendReq(emailQue);
                    sendEmailRequests.Add(emailQue, SendEmailRequestRecord);
                }
                foreach (KeyValuePair<EmailQue, SendEmailRequest> request in sendEmailRequests)
                {
                    try
                    {
                        SER = request.Value;
                        SendEmailStatus = SendEmailQue(ref SER, ref err);
                        if (SendEmailStatus.Status == false)
                        {
                            string message = "Email Manager cannot send this email while processing <{0}>. Details: FileId=<{1}>, ProspectId=<{2}>, EmailTemplId=<{3}>, TaskAlertId=<{4}>, ProspectAlertId=<{5}>, RequestUserId=<{6}>";
                            message = string.Format(message, ptype, SER.FileId, SER.ProspectId, SER.EmailTemplId, SER.LoanAlertId, SER.PropsectAlertId, SER.UserId);
                            message = message + " Exception: " + err;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, message, EventLogEntryType.Warning);
                            //returnstatus = m_da.DeleteEmailQue(SendEmailStatus.EmailId, ref err);
                            //if (returnstatus == false)
                            //{
                            //    EventLog.WriteEntry(InfoHubEventLog.LogSource, string.Format("Failed to delete Email Queue, EmailQue.EmailId={0}, err:{1}", SendEmailStatus.EmailId, err), EventLogEntryType.Warning);
                            //}
                        }

                        sendStatus.Add(request.Key, SendEmailStatus);
                        //returnstatus = m_da.DeleteEmailQue(SendEmailStatus.EmailId, ref err);
                        //if (returnstatus == false)
                        //{
                        //    EventLog.WriteEntry(InfoHubEventLog.LogSource, string.Format("Failed to delete Email Queue, EmailQue.EmailId={0}, err:{1}", SendEmailStatus.EmailId, err), EventLogEntryType.Warning);
                        //}
                        //else
                        //{
                        //    sendStatus.Add(request.Key, SendEmailStatus);
                        //}
                    }
                    catch (Exception exception)
                    {
                        string message = "Email Manager cannot send this email while processing <{0}>. Details: FileId=<{1}>, ProspectId=<{2}>, EmailTemplId=<{3}>, TaskAlertId=<{4}>, ProspectAlertId=<{5}>, RequestUserId=<{6}>";
                        message = string.Format(message, ptype, SER.FileId, SER.ProspectId, SER.EmailTemplId, SER.LoanAlertId, SER.PropsectAlertId, SER.UserId);
                        message = message + " Exception: " + exception.Message;
                        int Event_id = 5065;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, message, EventLogEntryType.Warning, Event_id, Category);
                        returnstatus = m_da.DeleteEmailQue(SendEmailStatus.EmailId, ref err);
                    }
                }
            }

            int prospectId = 0;
            int prospectAlertId = 0;
            foreach (KeyValuePair<EmailQue, SendStatus> status in sendStatus)
            {
                EmailQue emailQue = status.Key;
                SendStatus value = status.Value;
                try
                {
                    prospectId = 0;
                    prospectAlertId = 0;

                    var emailLogId = 0;
                    var fileId = 0;
                    if (value.req != null)
                    {
                        prospectId = value.req.ProspectId;
                        prospectAlertId = value.req.PropsectAlertId;
                    }
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, string.Format("ProcessEmailQue, before invoking HandleEmailQueLog, emailQue.EmailId={0}, Status={1}, Message={2}", emailQue.EmailId, value.Status, value.Message), EventLogEntryType.Information, 9102);
                    bool delStatus = m_da.HandleEmailQueLog(emailQue.EmailId, value.Status, value.Message, value.LastSent, value.Retries, value.FromEmail, value.FromUser, value.SendFailedAndRemoveEmailQue, value.EmailBody,
                                           value.ToEmails, value.ToUserIds, value.ToContactIds, value.Subject, prospectId, prospectAlertId, value.ChainId, value.SequenceNumber, value.EwsImported, value.ToEmails, value.ToUserIds, value.ToContactIds, out err, ref emailLogId, ref fileId);

                    foreach (var attachment in value.Attachments)
                    {
                        var nt = attachment.Key.Split(new[] { '.' });
                        var name = nt[0];
                        var ext = nt[1];
                        m_da.SaveEmailLogAttachment(emailLogId, fileId, name, ext, attachment.Value);
                    }

                    if (delStatus == false)
                    {
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                        continue;
                    }
                }
                catch (Exception exception)
                {
                    string message = "Email Manager cannot log this email while processing <{0}>. Details: FileId=<{1}>, ProspectId=<{2}>, EmailTemplId=<{3}>, TaskAlertId=<{4}>, ProspectAlertId=<{5}>, RequestUserId=<{6}>";
                    message = string.Format(message, ptype, emailQue.FileId, prospectId, emailQue.EmailTmplId, emailQue.LoanAlertId, prospectAlertId, value.req.UserId);
                    message = message + " Exception: " + exception;
                    int Event_id = 5067;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, message, EventLogEntryType.Warning, Event_id, Category);
                    returnstatus = m_da.DeleteEmailQue(emailQue.EmailId, ref err);
                }
            }

            return true;
        }

        /// <summary>
        /// Emails the que to send req.
        /// </summary>
        /// <param name="emailQue">The email que.</param>
        /// <returns></returns>
        private SendEmailRequest EmailQueToSendReq(EmailQue emailQue)
        {
            SendEmailRequest request = new SendEmailRequest();
            if (emailQue != null)
            {
                request.FileId = emailQue.FileId;
                request.EmailTemplId = emailQue.EmailTmplId;
                request.LoanAlertId = emailQue.LoanAlertId;
                request.EmailQueId = emailQue.EmailId;

                if (!string.IsNullOrEmpty(emailQue.ToContact))
                {
                    request.ToContactIds =
                        emailQue.ToContact.Split(new char[] { ';' }).Select(sId => int.Parse(sId)).ToArray();
                }
                if (!string.IsNullOrEmpty(emailQue.ToUser))
                {
                    request.ToUserIds =
                        emailQue.ToUser.Split(new char[] { ';' }).Select(sId => int.Parse(sId)).ToArray();
                }
                if (!string.IsNullOrEmpty(emailQue.ToBorrower))
                {
                    //Borrower from Contact table
                    if (request.ToContactIds != null)
                    {
                        request.ToContactIds = request.ToContactIds.Union(emailQue.ToBorrower.Split(new char[] { ';' }).Select(sId => int.Parse(sId)).ToArray()).ToArray();
                    }
                    else
                    {
                        request.ToContactIds =
                            emailQue.ToBorrower.Split(new char[] { ';' }).Select(sId => int.Parse(sId)).ToArray();
                    }
                }

                if (request.EmailTemplId > 0)
                {
                    string err = string.Empty;
                    try
                    {
                        request.Attachments = m_da.GetEmailAttachments(request.EmailTemplId);
                    }
                    catch (Exception e)
                    {
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, e.Message, EventLogEntryType.Warning);
                    }
                }

            }
            return request;
        }

        /// <summary>
        /// Computes the replacement.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        private static string ComputeReplacement(Match m, int fileId, int loanAlertId, int userId, int prospectId, int propsectTaskId, int propsectAlertId)
        {
            short Category = 60;
            string replaceValue = string.Empty;
            string DataType = string.Empty;
            string pointFieldId = string.Empty;
            string actValue = string.Empty;
            string err;
            if (m.Success)
            {
                if (m.Groups["type"] != null && !string.IsNullOrEmpty(m.Groups["type"].Value))
                {
                    DataType = m.Groups["type"].Value;
                }
                if (m.Groups["act"] != null && !string.IsNullOrEmpty(m.Groups["act"].Value))
                {
                    actValue = m.Groups["act"].Value;
                }
                if (m.Groups["field"] != null && !string.IsNullOrEmpty(m.Groups["field"].Value))
                {
                    pointFieldId = m.Groups["field"].Value;
                }
            }
            FieldType fieldType = FieldType.Default;
            if (!string.IsNullOrEmpty(DataType) && Enum.IsDefined(typeof(FieldType), DataType))
            {
                fieldType = (FieldType)Enum.Parse(typeof(FieldType), DataType, true);
            }

            try
            {
                if (!string.IsNullOrEmpty(DataType))
                {
                    if (loanAlertId > 0 && actValue.Equals("Decline Alert Button", StringComparison.CurrentCultureIgnoreCase))
                    {
                        replaceValue = string.Format("{0}?AlertId={1}&Action=Decline", BackgroundLoanAlertPage, loanAlertId);
                        return string.Format("<a href='{0}'>Decline</a>", replaceValue);
                    }
                    else if (loanAlertId > 0 && actValue.Equals("Accept Alert Button", StringComparison.CurrentCultureIgnoreCase))
                    {
                        replaceValue = string.Format("{0}?AlertId={1}&Action=Accept", BackgroundLoanAlertPage, loanAlertId);
                        return string.Format("<a href='{0}'>Accept</a>", replaceValue);
                    }
                    else if (actValue.Equals("Accept Alert Button", StringComparison.CurrentCultureIgnoreCase) || actValue.Equals("Decline Alert Button", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return string.Empty;
                    }
                }
                replaceValue = m_da.GetLoanPointFieldValue(pointFieldId, fieldType, fileId, actValue, userId, prospectId, propsectTaskId, loanAlertId, propsectAlertId, out err);
                return replaceValue;
            }
            catch (Exception exception)
            {
                err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.ToString() + "\r\n\r\nStackTrace: " + exception.StackTrace;
                Trace.TraceError(exception.Message);
                int Event_id = 5069;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return actValue;
            }
        }

        /// <summary>
        /// Images the replacement.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <param name="res">The res.</param>
        /// <param name="isPreview">if set to <c>true</c> [is preview].</param>
        /// <returns></returns>
        private static string ImageReplacement(Match m, List<LinkedResource> res, bool isPreview)
        {
            string returnTemplate = "<img src=\"cid:{0}\" />";
            string previewTemplate = "<img src=\"data:image/gif;base64,{0}\" />";
            string styleTemplate = "<img style=\"{0}\" />";
            string cid = string.Empty;
            string imageData = string.Empty;
            string sStyle = string.Empty;
            if (m.Success)
            {
                if (m.Groups["cid"] != null && !string.IsNullOrEmpty(m.Groups["cid"].Value))
                {
                    cid = m.Groups["cid"].Value;
                }
                if (m.Groups["data"] != null && !string.IsNullOrEmpty(m.Groups["data"].Value))
                {
                    imageData = m.Groups["data"].Value;

                    if (!string.IsNullOrEmpty(imageData))
                    {
                        if (isPreview == false)
                        {
                            byte[] data = Convert.FromBase64String(imageData);
                            MemoryStream memoryStream = new MemoryStream(data.Length);
                            memoryStream.Write(data, 0, data.Length);
                            memoryStream.Position = 0;//very important flag.
                            LinkedResource linkedResource = new LinkedResource(memoryStream, MediaTypeNames.Image.Jpeg);
                            linkedResource.ContentId = cid;
                            if (res != null)
                                res.Add(linkedResource);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(imageData))
                                return string.Empty;

                            return string.Format(previewTemplate, imageData);
                        }
                    }
                }
            }

            return string.Format(returnTemplate, cid);
        }


        /// <summary>
        /// Images the replacement.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <param name="res">The res.</param>
        /// <param name="isPreview">if set to <c>true</c> [is preview].</param>
        /// <param name="bStyle">mark for Style</param>
        /// <returns></returns>
        private static string ImageReplacement(Match m, List<LinkedResource> res, bool isPreview, bool bStyle)
        {
            string returnTemplate = "<img src=\"cid:{0}\" style=\"{1}\"/>";
            string previewTemplate = "<img src=\"data:image/gif;base64,{0}\" style=\"{1}\"/>";
            string cid = string.Empty;
            string imageData = string.Empty;
            string sStyle = string.Empty;
            if (m.Success)
            {
                if (m.Groups["cid"] != null && !string.IsNullOrEmpty(m.Groups["cid"].Value))
                {
                    cid = m.Groups["cid"].Value;
                }

                if (m.Groups["style"] != null && !string.IsNullOrEmpty(m.Groups["style"].Value))
                {
                    sStyle = m.Groups["style"].Value;
                }

                if (m.Groups["data"] != null && !string.IsNullOrEmpty(m.Groups["data"].Value))
                {
                    imageData = m.Groups["data"].Value;

                    if (!string.IsNullOrEmpty(imageData))
                    {
                        if (isPreview == false)
                        {
                            byte[] data = Convert.FromBase64String(imageData);
                            MemoryStream memoryStream = new MemoryStream(data.Length);
                            memoryStream.Write(data, 0, data.Length);
                            memoryStream.Position = 0;//very important flag.
                            LinkedResource linkedResource = new LinkedResource(memoryStream, MediaTypeNames.Image.Gif);
                            linkedResource.ContentId = cid;
                            if (res != null)
                                res.Add(linkedResource);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(imageData))
                                return string.Empty;

                            return string.Format(previewTemplate, imageData, sStyle);
                        }
                    }
                }
            }
            return string.Format(returnTemplate, cid, sStyle);

        }

        /// <summary>
        /// Gets the replies.
        /// </summary>
        public void GetReplies()
        {
            string err = string.Empty;
            ExchangeService service = GetService();

            var emailLogs = m_da.GetPendgingEmailLog();


            foreach (var emailLog in emailLogs)
            {
                try
                {
                    User userInfo = new User();
                    userInfo = m_da.Get_UserInfo(emailLog.FromUser, ref err);

                    if (userInfo != null && !string.IsNullOrEmpty(userInfo.Password))
                    {
                        if (!string.IsNullOrEmpty(_emailServerSetting.EWS_Domain))
                        {
                            service.Credentials = new NetworkCredential(userInfo.Username, userInfo.Password, _emailServerSetting.EWS_Domain);
                        }
                        else
                        {
                            service.Credentials = new NetworkCredential(userInfo.Email, userInfo.ExchangePassword);
                        }
                        //service.Credentials = new NetworkCredential(userInfo.Username, userInfo.Password, _emailServerSetting.EWS_Domain);
                    }
                    else
                    {
                        continue;
                    }


                    //get original email
                    ExtendedPropertyDefinition extendedPropertyDefinition = GetExtendedPropertyDefinition();
                    ItemView originalView = new ItemView(1);
                    SearchFilter searchFilter = new SearchFilter.IsEqualTo(extendedPropertyDefinition, emailLog.ChainId);
                    originalView.PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.DisplayTo, extendedPropertyDefinition);
                    FindItemsResults<Item> originalFindResults = service.FindItems(WellKnownFolderName.SentItems, searchFilter, originalView);
                    EmailMessage sendMessage = null;
                    if (originalFindResults.Count() > 0)
                    {
                        Item sendItem = originalFindResults.ToArray()[0];
                        sendMessage = sendItem as EmailMessage;
                        if (sendMessage != null)
                        {
                            sendMessage.Load();
                        }
                        else
                        {
                            continue;
                        }
                    }
                    ////end

                    ItemView view = new ItemView(int.MaxValue);
                    view.PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.Id);

                    FindItemsResults<Item> findInboxResults = service.FindItems(
                        WellKnownFolderName.Inbox,
                        new SearchFilter.SearchFilterCollection(
                            LogicalOperator.And
                            , new SearchFilter.ContainsSubstring(ItemSchema.Subject, string.Format("Re: {0}", emailLog.Subject))
                            , new SearchFilter.ContainsSubstring(ItemSchema.Body, emailLog.ChainId)
                        //, new SearchFilter.IsLessThanOrEqualTo(ItemSchema.DateTimeCreated, emailLog.LastSent.AddMinutes(_emailServerSetting.EmailInterval))
                        //, new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeCreated, emailLog.LastSent.AddMinutes(-_emailServerSetting.EmailInterval))
                        //, new SearchFilter.IsLessThanOrEqualTo(ItemSchema.DateTimeCreated, DateTime.Now.AddHours(3))
                        //, new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeCreated, DateTime.Now.AddHours(-3))
                            )
                        , view);
                    FindItemsResults<Item> findSendItemResults = service.FindItems(
                        WellKnownFolderName.SentItems,
                        new SearchFilter.SearchFilterCollection(
                            LogicalOperator.And
                            , new SearchFilter.ContainsSubstring(ItemSchema.Subject, string.Format("Re: {0}", emailLog.Subject))
                            , new SearchFilter.ContainsSubstring(ItemSchema.Body, emailLog.ChainId)
                        //, new SearchFilter.IsLessThanOrEqualTo(ItemSchema.DateTimeCreated, emailLog.LastSent.AddMinutes(_emailServerSetting.EmailInterval))
                        //, new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeCreated, emailLog.LastSent.AddMinutes(-_emailServerSetting.EmailInterval))
                        //, new SearchFilter.IsLessThanOrEqualTo(ItemSchema.DateTimeCreated, DateTime.Now.AddHours(3))
                        //, new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeCreated, DateTime.Now.AddHours(-3))

                            )
                        , view);
                    List<EmailMessage> searchedMessages = new List<EmailMessage>();
                    EmailMessage message = null;
                    foreach (Item inboxResult in findInboxResults)
                    {
                        message = inboxResult as EmailMessage;

                        if (message != null)
                        {
                            message.Load();
                            searchedMessages.Add(message);
                        }

                    }

                    //var query = from msg in searchedMessages
                    //            where
                    //                (EmailAddressContains(msg.ToRecipients, sendMessage.Sender) && (EmailAddressContains(sendMessage.ToRecipients, msg.Sender) || EmailAddressContains(sendMessage.CcRecipients, msg.Sender))) //inbox
                    //                || (EmailAddressContains(msg.ToRecipients, sendMessage.Sender) && (EmailAddressContains(sendMessage.ToRecipients, msg.Sender) || EmailAddressContains(sendMessage.CcRecipients, msg.Sender))) //inbox
                    //            select msg;
                    var query = from msg in searchedMessages
                                where
                                    (EmailAddressContains(msg.ToRecipients, sendMessage.Sender) && (EmailAddressContains(sendMessage.ToRecipients, msg.Sender) || EmailAddressContains(sendMessage.CcRecipients, msg.Sender))) //inbox
                                select msg;

                    List<EmailMessage> emailLogItems = query.ToList();

                    //senditems with re subject
                    foreach (Item sendItemResult in findSendItemResults)
                    {
                        message = sendItemResult as EmailMessage;

                        if (message != null)
                        {
                            message.Load();
                            emailLogItems.Add(message);
                        }

                    }
                    SaveEmailReplyToDb(emailLogItems, emailLog);
                    int cnt = emailLogItems.Count();
                }
                catch (Exception exception)
                {
                    //string errMsg = "Can not get reply email for EmailLogId: " + emailLog.EmailLogId + "\r\n" + exception.ToString();
                    //Trace.TraceError(exception.Message);
                    //int Event_id = 5071;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                }
            }

        }

        private ExchangeService GetService()
        {

            bool isKnownVersion = false;
            var ExchgVer = GetExchangeVersion(_emailServerSetting.EWS_Version, ref isKnownVersion);
            ExchangeService service = new ExchangeService();

            if (isKnownVersion)
            {
                service = new ExchangeService(ExchgVer);
            }

            service.Url = new Uri(_emailServerSetting.EwsUrl);
            return service;
        }

        private void SaveEmailReplyToDb(List<EmailMessage> emailMessages, Table.PendingEmailLog emailLog)
        {
            foreach (EmailMessage emailMessage in emailMessages)
            {
                try
                {

                    string toEmail = string.Empty;
                    emailMessage.ToRecipients.ToList().ForEach(item => toEmail += item.Address + ";");

                    string ccEmail = string.Empty;
                    emailMessage.CcRecipients.ToList().ForEach(item => ccEmail += item.Address + ";");

                    byte[] emailBody = null;
                    if (!string.IsNullOrEmpty(emailMessage.Body.Text))
                    {
                        emailBody = Encoding.Default.GetBytes(emailMessage.Body.Text);
                    }

                    m_da.SaveEmailReplyToDb(emailLog.EmailLogId, emailMessage.Sender.Address, emailBody, emailMessage.Subject, toEmail, ccEmail, emailMessage.DateTimeReceived, emailMessage.Id.UniqueId);
                }
                catch (Exception exception)
                {
                    string errMsg = "Can not save email reply for emailID: " + emailMessage.Id.UniqueId + " Exception: " + exception.ToString();
                    int Event_id = 5073;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }


        private bool EmailAddressContains(IEnumerable<EmailAddress> source, EmailAddress value)
        {
            if (value == null || source == null || source.Count() <= 0)
                return false;

            foreach (EmailAddress emailAddress in source)
            {
                if (emailAddress.Address.Equals(value.Address, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Replies to message.
        /// </summary>
        /// <param name="replyToEmailRequest">The reply to email request.</param>
        /// <returns></returns>
        public SendStatus ReplyToMessage(ReplyToEmailRequest replyToEmailRequest)
        {
            SendStatus status = new SendStatus();
            status.Message = " ";

            int LogId = 0;
            int FromUser = 0;
            bool Update_EmailLog = false;

            string ChainId = string.Empty;
            string EmailUniqueId = string.Empty;

            string sToUser = string.Empty;
            string sToContact = string.Empty;

            string sCcUser = string.Empty;
            string sCcContacts = string.Empty;

            string fromEmail = string.Empty;
            string newEmailUniqueId = string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            Table.PendingEmailLog emailLogInfo = null;
            DateTime dateTimeReceived = DateTime.Now;
            ExchangeService service = GetService();

            LogId = replyToEmailRequest.ReplyToEmailLogId;
            FromUser = replyToEmailRequest.FromUser;
            EmailUniqueId = replyToEmailRequest.EmailUniqueId;

            if (replyToEmailRequest.ToUser != null && replyToEmailRequest.ToUser.Length > 0)
            {
                var query = from item in replyToEmailRequest.ToUser select item.ToString();
                sToUser = string.Join(";", query.ToArray());
            }

            if (replyToEmailRequest.ToContact != null && replyToEmailRequest.ToContact.Length > 0)
            {
                var query = from item in replyToEmailRequest.ToContact select item.ToString();
                sToContact = string.Join(";", query.ToArray());
            }

            if (replyToEmailRequest.CCUser != null && replyToEmailRequest.CCUser.Length > 0)
            {
                var query = from item in replyToEmailRequest.CCUser select item.ToString();
                sCcUser = string.Join(";", query.ToArray());
            }

            if (replyToEmailRequest.CCContact != null && replyToEmailRequest.CCContact.Length > 0)
            {
                var query = from item in replyToEmailRequest.CCContact select item.ToString();
                sCcContacts = string.Join(";", query.ToArray());
            }

            if (replyToEmailRequest.ReplyToEmailLogId > 0)
            {
                emailLogInfo = m_da.GetEmailLogInfo(LogId);
                if (emailLogInfo != null)
                {
                    ChainId = emailLogInfo.ChainId;
                    //if (string.IsNullOrEmpty(ChainId))
                    //{
                    //    Update_EmailLog = true;
                    //    ChainId = Guid.NewGuid().ToString();
                    //    
                    //}
                    if (string.IsNullOrEmpty(EmailUniqueId))
                    {
                        EmailUniqueId = emailLogInfo.EmailUniqueId;
                        //if (string.IsNullOrEmpty(EmailUniqueId))
                        //{
                        //    Update_EmailLog = true;
                        //    EmailUniqueId = Guid.NewGuid().ToString();
                        //}
                    }
                    if (replyToEmailRequest.EmailBody == null)
                    {
                        replyToEmailRequest.EmailBody = emailLogInfo.EmailBody;
                    }
                }
            }

            string smtpReplyBody = string.Empty;
            string smtpSubject = string.Empty;
            MailAddress smtpFromEmail = null;
            List<MailAddress> tos = new List<MailAddress>();
            List<MailAddress> ccs = new List<MailAddress>();
            List<MailAddress> bccs = new List<MailAddress>();

            try
            {
                User userInfo = new User();
                string err = string.Empty;

                userInfo = m_da.Get_UserInfo(replyToEmailRequest.FromUser, ref err);

                fromEmail = m_da.GetUserEmailAddress(replyToEmailRequest.FromUser);

                if (userInfo != null && !string.IsNullOrEmpty(userInfo.Password))
                {
                    if (!string.IsNullOrEmpty(_emailServerSetting.EWS_Domain))
                    {
                        service.Credentials = new NetworkCredential(userInfo.Username, userInfo.Password, _emailServerSetting.EWS_Domain);
                    }
                    else
                    {
                        service.Credentials = new NetworkCredential(userInfo.Email, userInfo.ExchangePassword);
                    }
                    //service.Credentials = new NetworkCredential(userInfo.Username, userInfo.Password, _emailServerSetting.EWS_Domain);
                }
                else
                {
                    //   stringBuilder.AppendLine(string.Format("Can't not get user info from ReplyToEmailRequest.FromUser={0}", replyToEmailRequest.FromUser));
                }
                ExtendedPropertyDefinition extendedPropertyDefinition = GetExtendedPropertyDefinition();
                //add customize prop
                Guid guid = Guid.NewGuid();

                EmailMessage message = null;

                // Bind to an existing message using its unique identifier.

                if (!string.IsNullOrEmpty(EmailUniqueId))
                {
                    try
                    {
                        if (Regex.IsMatch(EmailUniqueId, @"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-Z0-9]{12}\b", RegexOptions.IgnoreCase))
                        {
                            //get original email
                            ItemView originalView = new ItemView(1);
                            SearchFilter searchFilter = new SearchFilter.IsEqualTo(extendedPropertyDefinition, EmailUniqueId);
                            originalView.PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.DisplayTo, extendedPropertyDefinition);
                            FindItemsResults<Item> originalFindResults = service.FindItems(WellKnownFolderName.SentItems, searchFilter, originalView);
                            if (originalFindResults.Count() > 0)
                            {
                                Item sendItem = originalFindResults.ToArray()[0];
                                message = sendItem as EmailMessage;
                                if (message != null)
                                {
                                    message.Load();
                                }
                            }
                            ////end
                        }
                        else
                        {
                            message = EmailMessage.Bind(service, new ItemId(EmailUniqueId));
                        }
                    }
                    catch (Exception ex)
                    {
                        message = null;
                        Trace.TraceError(ex.Message);
                        int Event_id = 5077;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, string.Format("Can't not load email for EWS item ID<0>,Pulse will try reply email via SMTP.", EmailUniqueId), EventLogEntryType.Warning, Event_id, Category);
                    }
                }



                #region Reply via EWS

                if (message != null)
                {
                    ResponseMessage responseMessage = message.CreateReply(false);

                    //Subject )
                    if (!string.IsNullOrEmpty(replyToEmailRequest.Subject))
                    {
                        responseMessage.Subject = replyToEmailRequest.Subject;
                    }

                    //email body
                    string replyBody = string.Empty;
                    if (replyToEmailRequest.EmailBody != null && replyToEmailRequest.EmailBody.Length > 0)
                    {
                        replyBody = Encoding.UTF8.GetString(replyToEmailRequest.EmailBody);
                    }

                    int propsectTaskId = 0;
                    try
                    {
                        propsectTaskId = m_da.GetProspectTaskId(emailLogInfo.ProspectAlertId);
                    }
                    catch (Exception exception)
                    {
                        //   stringBuilder.AppendLine(string.Format("Can't not get propsectTaskId via ProspectAlertId={0}", emailLogInfo.ProspectAlertId));
                    }

                    replyBody = GetEmailContent(replyBody, emailLogInfo.FileId, emailLogInfo.LoanAlertId, emailLogInfo.FromUser, emailLogInfo.ProspectId, propsectTaskId, emailLogInfo.ProspectAlertId);

                    //process email tag

                    //add signature tag

                    List<LinkedResource> resources = null;
                    if (replyToEmailRequest.AppendPictureSignature == true)
                    {
                        replyBody += "<br /><p>&lt;@DB-Sender Picture@&gt;</p><p>&lt;@DB-Sender Signature@&gt;</p>";
                        replyBody = GetPictureSignature(replyBody, replyToEmailRequest.FromUser);
                    }
                    smtpReplyBody = replyBody;
                    resources = new List<LinkedResource>();
                    replyBody = Regex.Replace(replyBody, @"<img src=""cid:(?<cid>\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-Z0-9]{12}\b)""\s*?data=""(?<data>.*?)"" />", match => ImageReplacement(match, resources, false), RegexOptions.IgnoreCase);
                    replyBody += string.Format("<font style=\"color:white;display:none;text-indent:-9999px;font-size:0px;\">GUID={0},CHAINID={1}</font>", guid.ToString(), ChainId);

                    string emailLogBody = Regex.Replace(replyBody, @"<img src=""cid:(?<cid>\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-Z0-9]{12}\b)""\s*?data=""(?<data>.*?)"" />", match => ImageReplacement(match, null, true), RegexOptions.IgnoreCase);
                    status.EmailBody = emailLogBody;

                    responseMessage.BodyPrefix = replyBody;

                    #region process recipient

                    //process recipient
                    //to user start

                    if (replyToEmailRequest.ToUser != null && replyToEmailRequest.ToUser.Length > 0)
                    {
                        var query = from item in replyToEmailRequest.ToUser select item.ToString();
                        sToUser = string.Join(";", query.ToArray());

                        foreach (int uid in replyToEmailRequest.ToUser)
                        {
                            try
                            {
                                string userEmail = string.Empty;
                                EmailAddress eaUserEmail = null;
                                userEmail = m_da.GetUserEmailAddress(uid);
                                if (!string.IsNullOrEmpty(userEmail))
                                {
                                    eaUserEmail = new EmailAddress(userEmail);
                                    if (!EmailAddressContains(responseMessage.ToRecipients, eaUserEmail))
                                    {
                                        responseMessage.ToRecipients.Add(eaUserEmail);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                stringBuilder.AppendLine(string.Format("Can't not get user email for user id = {0}", uid));
                            }
                        }
                    }

                    //to user end

                    //to contact start
                    if (replyToEmailRequest.ToContact != null && replyToEmailRequest.ToContact.Length > 0)
                    {
                        var query = from item in replyToEmailRequest.ToContact select item.ToString();
                        sToContact = string.Join(";", query.ToArray());

                        foreach (int uid in replyToEmailRequest.ToContact)
                        {
                            try
                            {
                                string userEmail = string.Empty;
                                EmailAddress eaUserEmail = null;
                                userEmail = m_da.GetContactEmailAddress(uid);
                                if (!string.IsNullOrEmpty(userEmail))
                                {
                                    eaUserEmail = new EmailAddress(userEmail);
                                    if (!EmailAddressContains(responseMessage.ToRecipients, eaUserEmail))
                                    {
                                        responseMessage.ToRecipients.Add(eaUserEmail);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                stringBuilder.AppendLine(string.Format("Can't not get user user email for contact id = {0}", uid));
                            }
                        }
                    }
                    //to contact end

                    //cc user start
                    if (replyToEmailRequest.CCUser != null && replyToEmailRequest.CCUser.Length > 0)
                    {
                        var query = from item in replyToEmailRequest.CCUser select item.ToString();
                        sCcUser = string.Join(";", query.ToArray());
                        foreach (int uid in replyToEmailRequest.CCUser)
                        {
                            try
                            {
                                string userEmail = string.Empty;
                                EmailAddress eaUserEmail = null;
                                userEmail = m_da.GetUserEmailAddress(uid);
                                if (!string.IsNullOrEmpty(userEmail))
                                {
                                    eaUserEmail = new EmailAddress(userEmail);
                                    if (!EmailAddressContains(responseMessage.CcRecipients, eaUserEmail))
                                    {
                                        responseMessage.CcRecipients.Add(eaUserEmail);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                stringBuilder.AppendLine(string.Format("Can't not get user user email for user id = {0}", uid));
                            }
                        }
                    }
                    //cc user end

                    //cc contact start
                    if (replyToEmailRequest.CCContact != null && replyToEmailRequest.CCContact.Length > 0)
                    {
                        var query = from item in replyToEmailRequest.CCContact select item.ToString();
                        sCcContacts = string.Join(";", query.ToArray());
                        foreach (int uid in replyToEmailRequest.CCContact)
                        {
                            try
                            {
                                string userEmail = string.Empty;
                                EmailAddress eaUserEmail = null;
                                userEmail = m_da.GetContactEmailAddress(uid);
                                if (!string.IsNullOrEmpty(userEmail))
                                {
                                    eaUserEmail = new EmailAddress(userEmail);
                                    if (!EmailAddressContains(responseMessage.CcRecipients, eaUserEmail))
                                    {
                                        responseMessage.CcRecipients.Add(eaUserEmail);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                stringBuilder.AppendLine(string.Format("Can't not get user user email contact id = {0}", uid));
                            }
                        }
                    }
                    //cc contact end

                    //toemail start 
                    if (!string.IsNullOrEmpty(replyToEmailRequest.ToEmail))
                    {
                        EmailAddress[] EMA =
                            replyToEmailRequest.ToEmail.Split(new char[] { ';' }).Select(sId => new EmailAddress(sId)).ToArray();
                        foreach (EmailAddress eaUserEmail in EMA)
                        {
                            if (!EmailAddressContains(responseMessage.ToRecipients, eaUserEmail))
                            {
                                responseMessage.ToRecipients.Add(eaUserEmail);
                            }
                        }
                    }

                    //toemail end

                    //ccemail start 
                    if (!string.IsNullOrEmpty(replyToEmailRequest.CCEmail))
                    {
                        EmailAddress[] EMA =
                            replyToEmailRequest.CCEmail.Split(new char[] { ';' }).Select(sId => new EmailAddress(sId)).ToArray();
                        foreach (EmailAddress eaUserEmail in EMA)
                        {
                            if (!EmailAddressContains(responseMessage.CcRecipients, eaUserEmail))
                            {
                                responseMessage.CcRecipients.Add(eaUserEmail);
                            }
                        }
                    }
                    //ccemail end
                    //end recipient

                    #endregion

                    EmailMessage reply = responseMessage.Save();
                    //todo:check
                    reply.Load();
                    smtpSubject = reply.Subject;
                    smtpFromEmail = new MailAddress(reply.Sender.Address);

                    foreach (EmailAddress emailAddress in reply.ToRecipients)
                    {
                        tos.Add(new MailAddress(emailAddress.Address));
                    }
                    foreach (EmailAddress emailAddress in reply.CcRecipients)
                    {
                        ccs.Add(new MailAddress(emailAddress.Address));
                    }
                    foreach (EmailAddress emailAddress in reply.BccRecipients)
                    {
                        bccs.Add(new MailAddress(emailAddress.Address));
                    }
                    dateTimeReceived = reply.DateTimeReceived;
                    if (resources != null && resources.Count > 0)
                    {
                        foreach (LinkedResource linkedResource in resources)
                        {
                            FileAttachment attachment = reply.Attachments.AddFileAttachment(linkedResource.ContentId, linkedResource.ContentStream);
                            attachment.ContentId = linkedResource.ContentId;
                        }
                    }

                    reply.Update(ConflictResolutionMode.AutoResolve);

                    // Define the extended property itself.
                    //ExtendedPropertyDefinition extendedPropertyDefinition = GetExtendedPropertyDefinition();

                    //Stamp the extended property on a message.
                    message.SetExtendedProperty(extendedPropertyDefinition, guid.ToString());
                    //message.SetExtendedProperty(extendedPropertyDefinition, EmailUniqueId);
                    newEmailUniqueId = guid.ToString();
                    // newEmailUniqueId = Guid.NewGuid().ToString();
                    reply.SendAndSaveCopy();
                    status.Status = true;
                    status.EmailUniqueId = newEmailUniqueId;
                }
                else
                {
                    stringBuilder.AppendLine(string.Format("Can't not get reply email message from ews for item ID={0} and user name={1}", EmailUniqueId, userInfo.Username));
                }
            }
            catch (Exception exception)
            {
                status.Status = false;
                stringBuilder.AppendLine(exception.Message);
                Trace.TraceError(exception.Message);
                int Event_id = 5079;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, exception.Message, EventLogEntryType.Warning, Event_id, Category);
            }

                #endregion

            smtpReplyBody = string.Empty;

            string rBody = string.Empty;
            if (replyToEmailRequest.EmailBody != null && replyToEmailRequest.EmailBody.Length > 0)
            {
                rBody = Encoding.UTF8.GetString(replyToEmailRequest.EmailBody);
            }

            int pTaskId = 0;
            try
            {
                pTaskId = m_da.GetProspectTaskId(emailLogInfo.ProspectAlertId);
            }
            catch (Exception exception)
            {
                //   stringBuilder.AppendLine(string.Format("Can't not get propsectTaskId via ProspectAlertId={0}", emailLogInfo.ProspectAlertId));
            }

            rBody = GetEmailContent(rBody, emailLogInfo.FileId, emailLogInfo.LoanAlertId, emailLogInfo.FromUser, emailLogInfo.ProspectId, pTaskId, emailLogInfo.ProspectAlertId);

            if (replyToEmailRequest.AppendPictureSignature == true)
            {
                rBody += "<br /><p>&lt;@DB-Sender Picture@&gt;</p><p>&lt;@DB-Sender Signature@&gt;</p>";
                rBody = GetPictureSignature(rBody, replyToEmailRequest.FromUser);
            }

            smtpReplyBody = rBody;

            smtpSubject = replyToEmailRequest.Subject;

            string uEmail = string.Empty;

            uEmail = m_da.GetUserEmailAddress(replyToEmailRequest.FromUser);
            smtpFromEmail = new MailAddress(uEmail);

            tos = new List<MailAddress>();

            if (replyToEmailRequest.ToUser != null && replyToEmailRequest.ToUser.Length > 0)
            {
                var query = from item in replyToEmailRequest.ToUser select item.ToString();
                sToUser = string.Join(";", query.ToArray());

                foreach (int uid in replyToEmailRequest.ToUser)
                {
                    try
                    {
                        string userEmail = string.Empty;
                        userEmail = m_da.GetUserEmailAddress(uid);
                        if (!string.IsNullOrEmpty(userEmail))
                        {
                            MailAddress address = new MailAddress(userEmail);
                            if (!ChkExists(tos, address))
                                tos.Add(address);
                        }
                    }
                    catch (Exception)
                    {
                        //    stringBuilder.AppendLine(string.Format("Can't not get user user email for user id = {0}", uid));
                    }
                }
            }

            if (replyToEmailRequest.ToContact != null && replyToEmailRequest.ToContact.Length > 0)
            {
                var query = from item in replyToEmailRequest.ToContact select item.ToString();
                sToContact = string.Join(";", query.ToArray());

                foreach (int uid in replyToEmailRequest.ToContact)
                {
                    try
                    {
                        string userEmail = string.Empty;
                        userEmail = m_da.GetContactEmailAddress(uid);
                        if (!string.IsNullOrEmpty(userEmail))
                        {
                            MailAddress address = new MailAddress(userEmail);
                            if (!ChkExists(tos, address))
                                tos.Add(address);
                        }
                    }
                    catch (Exception)
                    {
                        //   stringBuilder.AppendLine(string.Format("Can't not get user user email for contact id = {0}", uid));
                    }
                }
            }

            bool dup = false;

            if (!string.IsNullOrEmpty(replyToEmailRequest.ToEmail))
            {
                MailAddress[] MA =
                replyToEmailRequest.ToEmail.Split(new char[] { ';' }).Select(sId => new MailAddress(sId)).ToArray();
                foreach (MailAddress m in MA)
                {
                    dup = false;
                    foreach (MailAddress t in tos)
                    {

                        if (m.Address.Equals(t.Address, StringComparison.CurrentCultureIgnoreCase))
                        {
                            dup = true;
                        }
                    }
                    if (dup == false)
                    {
                        tos.Add(m);
                    }
                }

            }

            ccs = new List<MailAddress>();

            if (replyToEmailRequest.CCUser != null && replyToEmailRequest.CCUser.Length > 0)
            {
                var query = from item in replyToEmailRequest.CCUser select item.ToString();
                sCcUser = string.Join(";", query.ToArray());
                foreach (int uid in replyToEmailRequest.CCUser)
                {
                    try
                    {
                        string userEmail = string.Empty;
                        userEmail = m_da.GetUserEmailAddress(uid);
                        if (!string.IsNullOrEmpty(userEmail))
                        {
                            MailAddress address = new MailAddress(userEmail);
                            if (!ChkExists(ccs, address))
                                ccs.Add(address);
                        }
                    }
                    catch (Exception)
                    {
                        //stringBuilder.AppendLine(string.Format("Can't not get user user email for user id = {0}", uid));
                    }
                }
            }

            if (replyToEmailRequest.CCContact != null && replyToEmailRequest.CCContact.Length > 0)
            {
                var query = from item in replyToEmailRequest.CCContact select item.ToString();
                sCcContacts = string.Join(";", query.ToArray());
                foreach (int uid in replyToEmailRequest.CCContact)
                {
                    try
                    {
                        string userEmail = string.Empty;
                        userEmail = m_da.GetContactEmailAddress(uid);
                        if (!string.IsNullOrEmpty(userEmail))
                        {
                            MailAddress address = new MailAddress(userEmail);
                            if (!ChkExists(ccs, address))
                                ccs.Add(address);
                        }
                    }
                    catch (Exception)
                    {
                        //    stringBuilder.AppendLine(string.Format("Can't not get user user email contact id = {0}", uid));
                    }
                }
            }

            if (!string.IsNullOrEmpty(replyToEmailRequest.CCEmail))
            {
                MailAddress[] MA =
                replyToEmailRequest.CCEmail.Split(new char[] { ';' }).Select(sId => new MailAddress(sId)).ToArray();
                foreach (MailAddress m in MA)
                {
                    dup = false;
                    foreach (MailAddress t in ccs)
                    {
                        if (m == t)
                        {
                            dup = true;
                        }
                    }
                    if (dup == false)
                    {
                        ccs.Add(m);
                    }
                }

            }

            bccs = new List<MailAddress>();

            if (status.Status == false)
            {
                try
                {
                    status.Status = SendEmail(smtpSubject, smtpReplyBody, smtpFromEmail, _emailServerSetting.EmailRelayServer, 25, tos, ccs, bccs, null);
                }
                catch (Exception exception)
                {
                    stringBuilder.AppendLine(string.Format("Reply email message failed via SMTP"));
                }
            }

            status.Message = stringBuilder.ToString();
            string sErr = string.Empty;

            try
            {
                //m_da.EmailLog(sToUser, sToContact, emailLogInfo.EmailTmplId, status.Status, status.Message, emailLogInfo.LoanAlertId, default(int), emailLogInfo.FileId, fromEmail, replyToEmailRequest.FromUser, default(int), status.EmailBody, replyToEmailRequest.Subject, default(bool), replyToEmailRequest.ToEmail, emailLogInfo.ProspectId, emailLogInfo.ProspectAlertId, emailLogInfo.ChainId, default(int), false, newEmailUniqueId, ref sErr);
                emailLogInfo.ToUser = sToUser;
                emailLogInfo.ToContact = sToContact;
                emailLogInfo.Success = status.Status;
                emailLogInfo.Error = status.Message;
                emailLogInfo.LastSent = DateTime.Now;
                emailLogInfo.Retries = 0;
                emailLogInfo.FromEmail = fromEmail;
                emailLogInfo.FromUser = replyToEmailRequest.FromUser;
                emailLogInfo.Created = DateTime.Now;
                emailLogInfo.EmailBody = replyToEmailRequest.EmailBody;
                emailLogInfo.Subject = replyToEmailRequest.Subject;
                emailLogInfo.Exported = false;
                emailLogInfo.ToEmail = replyToEmailRequest.ToEmail;
                emailLogInfo.CCUser = sCcUser;
                emailLogInfo.CCContact = sCcContacts;
                emailLogInfo.ChainId = ChainId;
                emailLogInfo.SequenceNumber = 0;
                emailLogInfo.EwsImported = false;
                emailLogInfo.CCEmail = replyToEmailRequest.CCEmail;
                emailLogInfo.DateTimeReceived = dateTimeReceived;
                emailLogInfo.EmailUniqueId = status.EmailUniqueId;

                m_da.SaveEmailLog(emailLogInfo);
                if (Update_EmailLog == true)
                {
                    int st = 0;
                    //如果通过ＳＭＴＰ发送邮件就不能更新EmailUniqueId这一列数据，这列的值表示EWS里存在和这个ＩＤ相关的邮件。
                    //string elSql = "update dbo.emaillog set ChainId='" + ChainId + "', EmailUniqueId='" + EmailUniqueId + "' where EmailLogId=" + LogId;
                    string elSql = "update dbo.emaillog set ChainId='" + ChainId + "' where EmailLogId=" + LogId;
                    st = DbHelperSQL.ExecuteSql(elSql);
                }
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.Message);
                int Event_id = 5081;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, sErr + exception.Message, EventLogEntryType.Warning, Event_id, Category);
            }
            return status;
        }

        /// <summary>
        /// Gets the picture signature.
        /// </summary>
        /// <param name="replyBody">The reply body.</param>
        /// <param name="fromUser">From user.</param>
        /// <returns></returns>
        private string GetPictureSignature(string replyBody, int fromUser)
        {
            return Regex.Replace(replyBody, @"&lt;\s*?@\s*?(?:(?<type>Previous|DB)-)?(?:(?<act>.*?)\s*?)(?:\((?<field>\d+)\))?\s*?@\s*?&gt;", match => PictureSignature(match, fromUser), RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        }

        private string PictureSignature(Match m, int fromUser)
        {
            string replaceValue = string.Empty;
            string DataType = string.Empty;
            string pointFieldId = string.Empty;
            string actValue = string.Empty;
            string err;
            if (m.Success)
            {
                if (m.Groups["type"] != null && !string.IsNullOrEmpty(m.Groups["type"].Value))
                {
                    DataType = m.Groups["type"].Value;
                }
                if (m.Groups["act"] != null && !string.IsNullOrEmpty(m.Groups["act"].Value))
                {
                    actValue = m.Groups["act"].Value;
                }
                if (m.Groups["field"] != null && !string.IsNullOrEmpty(m.Groups["field"].Value))
                {
                    pointFieldId = m.Groups["field"].Value;
                }
            }
            FieldType fieldType = FieldType.Default;
            if (!string.IsNullOrEmpty(DataType) && Enum.IsDefined(typeof(FieldType), DataType))
            {
                fieldType = (FieldType)Enum.Parse(typeof(FieldType), DataType, true);
            }

            try
            {
                replaceValue = m_da.PictureSignature(fieldType, actValue, fromUser, out err);
                return replaceValue;
            }
            catch (Exception exception)
            {
                err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.ToString() + "\r\n\r\nStackTrace: " + exception.StackTrace;
                Trace.TraceError(exception.Message);
                int Event_id = 5083;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return actValue;
            }
        }

        public EmailQueRecord GetEmailQueRecord(int EmailId, ref string err)
        {
            var EmailQueR = new EmailQueRecord();
            Byte[] ByteA = null;
            System.Data.SqlTypes.SqlBytes SB = new System.Data.SqlTypes.SqlBytes();
            string SQLString = "SELECT [EmailId],[ToUser],[ToContact],[ToBorrower],[EmailTmplId],[LoanAlertId],[FileId],[AlertEmailType],[EmailBody],[ProspectId],[ProspectAlertId] FROM [dbo].[EmailQue] where EmailId=" + EmailId;
            err = "";

            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                while (dataReader.Read())
                {
                    EmailQueR.EmailId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    EmailQueR.ToUser = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                    EmailQueR.ToContact = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    EmailQueR.ToBorrower = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
                    EmailQueR.EmailTmplId = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt32(4);
                    EmailQueR.LoanAlertId = dataReader.IsDBNull(5) ? 0 : dataReader.GetInt32(5);
                    EmailQueR.FileId = dataReader.IsDBNull(6) ? 0 : dataReader.GetInt32(6);
                    EmailQueR.AlertEmailType = dataReader.IsDBNull(7) ? 0 : dataReader.GetInt16(7);
                    if (dataReader.IsDBNull(8))
                    {
                        EmailQueR.EmailBody = string.Empty;
                    }
                    else
                    {
                        SB = dataReader.GetSqlBytes(8);
                        ByteA = SB.Value;
                        EmailQueR.EmailBody = Encoding.UTF8.GetString(ByteA);
                    }
                    EmailQueR.ProspectId = dataReader.IsDBNull(9) ? 0 : dataReader.GetInt32(9);
                    EmailQueR.ProspectAlertId = dataReader.IsDBNull(10) ? 0 : dataReader.GetInt32(10);
                };

            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in EmailQue." + ex.Message;
                int Event_id = 5085;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                Trace.TraceError(err);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }

            return EmailQueR;
        }

        public EmailTemplateRecord GetEmailTemplateRecord(int TemplEmailId, ref string err)
        {
            var EmailTemplateR = new EmailTemplateRecord();
            string SQLString = "SELECT [TemplEmailId],[Enabled],[Name],[Desc],[FromUserRoles],[FromEmailAddress],[Content],[Subject],[Target],[Custom],[SenderName] FROM [dbo].[Template_Email] where TemplEmailId=" + TemplEmailId;
            err = "";

            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                while (dataReader.Read())
                {
                    EmailTemplateR.TemplEmailId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    EmailTemplateR.Enabled = dataReader.IsDBNull(1) ? false : dataReader.GetBoolean(1);
                    EmailTemplateR.Name = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    EmailTemplateR.Desc = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
                    EmailTemplateR.FromUserRoles = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt32(4);
                    EmailTemplateR.FromEmailAddress = dataReader.IsDBNull(5) ? string.Empty : dataReader.GetString(5);
                    EmailTemplateR.Content = dataReader.IsDBNull(6) ? string.Empty : dataReader.GetString(6);
                    EmailTemplateR.Subject = dataReader.IsDBNull(7) ? string.Empty : dataReader.GetString(7);
                    EmailTemplateR.Target = dataReader.IsDBNull(8) ? string.Empty : dataReader.GetString(8);
                    EmailTemplateR.Custom = dataReader.IsDBNull(9) ? false : dataReader.GetBoolean(9);
                    EmailTemplateR.SenderName = dataReader.IsDBNull(10) ? string.Empty : dataReader.GetString(10);
                };

            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Template_Email." + ex.Message;
                int Event_id = 5087;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                Trace.TraceError(err);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }

            return EmailTemplateR;
        }

        public int Getuserid(int fileid, int roleid, ref string err)
        {
            int userid = 0;
            string SQLString = "SELECT top 1 [UserId] FROM [dbo].[LoanTeam] where FileId=" + fileid + " AND RoleId=" + roleid;
            err = "";

            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                while (dataReader.Read())
                {
                    userid = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                };

            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in LoanTeam." + ex.Message;
                int Event_id = 5089;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }

            return userid;
        }
        private static bool ChkExists(List<MailAddress> source, MailAddress item)
        {
            if (source == null)
                return false;
            if (source.Count < 1)
                return false;
            if (item == null)
                return false;
            foreach (var address in source)
            {
                if (address.Address.Equals(item.Address, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }


        private static bool IsEmail(string str_Email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_Email, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
    }

    public class SendStatus
    {
        public int EmailId { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public DateTime LastSent { get; set; }
        public int Retries { get; set; }
        public string FromEmail { get; set; }
        public int FromUser { get; set; }
        public bool SendFailedAndRemoveEmailQue { get; set; }
        public string EmailBody { get; set; }
        public string[] ToEmails { get; set; }
        public int[] ToUserIds { get; set; }
        public int[] ToContactIds { get; set; }
        public string[] CCEmails { get; set; }
        public int[] CCUserIds { get; set; }
        public int[] CCContactIds { get; set; }
        public string Subject { get; set; }
        public SendEmailRequest req { get; set; }
        public string ChainId { get; set; }
        public string EmailUniqueId { get; set; }
        public int SequenceNumber { get; set; }
        public bool EwsImported { get; set; }
        public Dictionary<string, byte[]> Attachments { get; set; }
    }

    public class EmailQueRecord
    {
        public int EmailId { get; set; }
        public string ToUser { get; set; }
        public string ToContact { get; set; }
        public string ToBorrower { get; set; }
        public int EmailTmplId { get; set; }
        public int LoanAlertId { get; set; }
        public int FileId { get; set; }
        public int AlertEmailType { get; set; }
        public string EmailBody { get; set; }
        public int ProspectId { get; set; }
        public int ProspectAlertId { get; set; }
    }

    public class EmailTemplateRecord
    {
        public int TemplEmailId { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int FromUserRoles { get; set; }
        public string FromEmailAddress { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public string Target { get; set; }
        public bool Custom { get; set; }
        public string SenderName { get; set; }
    }

}


