using System;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using HtmlAgilityPack;
using log4net;
using log4net.Config;

namespace EmailAndSkinTemplateCleanUp
{
    class Program
    {
        private static readonly List<ReportParameters> _ReportParametersMailChimpAttributeCleanup;
        private static readonly List<ReportParameters> _ReportParametersMalformedEmailTemplate;
        private static readonly ILog _Logger;
        private static readonly Regex _RegexMailChimpAttribute = new Regex("\\s*mc:edit\\s*=\\s*\"std_content00\"\\s*", RegexOptions.IgnoreCase);
        private static readonly SmtpClient _SmtpClient;
        private const string _sTemplateTypeEmailTemplate = "Email Template";
        private const string _sTemplateTypeEmailSkinTemplate = "Email Skin";

        static Program()
        {
            _ReportParametersMailChimpAttributeCleanup = new List<ReportParameters>();
            _ReportParametersMalformedEmailTemplate = new List<ReportParameters>();
            _Logger = LogManager.GetLogger(typeof(Program));
            XmlConfigurator.Configure();

            _SmtpClient = new SmtpClient();
            _SmtpClient.Host = ConfigurationManager.AppSettings["Smtp_Host"];
            _SmtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Smtp_Port"]);
            _SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _SmtpClient.UseDefaultCredentials = false;
            _SmtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["Smtp_EnableSsl"]);
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["Smtp_EnableAuthentication"]))
            {
                _SmtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Smtp_UserName"], ConfigurationManager.AppSettings["Smtp_Password"]);
            }
            _SmtpClient.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["Smtp_Timeout_Seconds"]) * 1000;
        }

        public static void Main(string[] args)
        {
            ProcessDatabaseInstances();
            MailReports();
        }

        private static void ProcessDatabaseInstances()
        {
            foreach (ConnectionStringSettings connectionStringSettings in ConfigurationManager.ConnectionStrings)
            {
                using (SqlConnection sqlConnectionDatabaseInstance = new SqlConnection(connectionStringSettings.ConnectionString))
                {
                    _Logger.Debug(string.Format("Processing '{0}' database instance for Pulse databases.", connectionStringSettings.Name));

                    try
                    {
                        sqlConnectionDatabaseInstance.Open();

                        try
                        {
                            _Logger.Debug(string.Format("Fetching list of databases of '{0}' database instance.", connectionStringSettings.Name));

                            using (DataTable dataTable = sqlConnectionDatabaseInstance.GetSchema("Databases"))
                            {
                                _Logger.Debug(string.Format("Finished fetching list of databases of '{0}' database instance.", connectionStringSettings.Name));

                                foreach (DataRow dataRow in dataTable.Rows)
                                {
                                    string sDatabaseName = dataRow["database_name"].ToString();

                                    if (!string.IsNullOrEmpty(sDatabaseName))
                                    {
                                        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionStringSettings.ConnectionString);

                                        sqlConnectionStringBuilder.InitialCatalog = sDatabaseName;

                                        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
                                        {
                                            _Logger.Debug(string.Format("Processing '{0}' database of '{1}' database instance.", sDatabaseName, connectionStringSettings.Name));

                                            try
                                            {
                                                sqlConnection.Open();

                                                ProcessDatabase(connectionStringSettings.Name, sqlConnection);
                                            }
                                            catch (Exception exception)
                                            {
                                                _Logger.Error(string.Format("Unable to connect to '{0}' database of '{1}' database instance.", sDatabaseName, connectionStringSettings.Name), exception);
                                                _Logger.Info(string.Format("Failed to process '{0}' database of '{1}' database instance.", sDatabaseName, connectionStringSettings.Name));

                                                continue;
                                            }

                                            _Logger.Debug(string.Format("Finished processing '{0}' database of '{1}' database instance.", sDatabaseName, connectionStringSettings.Name));
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            _Logger.Error(string.Format("Unable to fetch list of databases of '{0}' database instance.", connectionStringSettings.Name), exception);
                            _Logger.Info(string.Format("Failed to process '{0}' database instance for Pulse databases.\r\n\r\n", connectionStringSettings.Name));

                            continue;
                        }
                    }
                    catch (Exception exception)
                    {
                        _Logger.Error(string.Format("Unable to connect to '{0}' database instance.", connectionStringSettings.Name), exception);
                        _Logger.Info(string.Format("Failed to process '{0}' database instance for Pulse databases.\r\n\r\n", connectionStringSettings.Name));

                        continue;
                    }

                    _Logger.Debug(string.Format("Finished processing '{0}' database instance for Pulse databases.\r\n\r\n", connectionStringSettings.Name));
                }
            }
        }

        private static void ProcessDatabase(string sDatabaseInstanceName, SqlConnection sqlConnection)
        {
            try
            {
                _Logger.Debug(string.Format("Checking if '{0}' database of '{1}' database instance is a Pulse database.", sqlConnection.Database, sDatabaseInstanceName));

                using (DataTable dataTableTables = sqlConnection.GetSchema("Tables"))
                {
                    dataTableTables.DefaultView.RowFilter = "[TABLE_NAME]='Template_Email' OR [TABLE_NAME]='Template_EmailSkins'";

                    if (dataTableTables.DefaultView.Count == 2)
                    {
                        using (DataTable dataTableColumns_EmailTemplate = sqlConnection.GetSchema("Columns", new[] { sqlConnection.Database, null, "Template_Email" }))
                        {
                            dataTableColumns_EmailTemplate.DefaultView.RowFilter = "[COLUMN_NAME]='Content' OR [COLUMN_NAME]='Name' OR [COLUMN_NAME]='TemplEmailId'";

                            if (dataTableColumns_EmailTemplate.DefaultView.Count == 3)
                            {
                                using (DataTable dataTableColumns_EmailSkinsTemplate = sqlConnection.GetSchema("Columns", new[] { sqlConnection.Database, null, "Template_EmailSkins" }))
                                {
                                    dataTableColumns_EmailSkinsTemplate.DefaultView.RowFilter = "[COLUMN_NAME]='HTMLBody' OR [COLUMN_NAME]='Name' OR [COLUMN_NAME]='EmailSkinId'";

                                    if (dataTableColumns_EmailSkinsTemplate.DefaultView.Count == 3)
                                    {
                                        _Logger.Debug(string.Format("'{0}' database of '{1}' database instance is a Pulse database.", sqlConnection.Database, sDatabaseInstanceName));
                                        _Logger.Debug(string.Format("Processing '{0}' database of '{1}' database instance.", sqlConnection.Database, sDatabaseInstanceName));

                                        ProcessTemplates(sDatabaseInstanceName, sqlConnection, _sTemplateTypeEmailTemplate, "Template_Email", "Content", "Name", "TemplEmailId");
                                        ProcessTemplates(sDatabaseInstanceName, sqlConnection, _sTemplateTypeEmailSkinTemplate, "Template_EmailSkins", "HTMLBody", "Name", "EmailSkinId");

                                        _Logger.Debug(string.Format("Finished processing '{0}' database of '{1}' database instance.", sqlConnection.Database, sDatabaseInstanceName));
                                    }
                                    else
                                    {
                                        _Logger.Debug(string.Format("'{0}' database of '{1}' database instance is not a Pulse database.", sqlConnection.Database, sDatabaseInstanceName));
                                    }
                                }
                            }
                            else
                            {
                                _Logger.Debug(string.Format("'{0}' database of '{1}' database instance is not a Pulse database.", sqlConnection.Database, sDatabaseInstanceName));
                            }
                        }
                    }
                    else
                    {
                        _Logger.Debug(string.Format("'{0}' database of '{1}' database instance is not a Pulse database.", sqlConnection.Database, sDatabaseInstanceName));
                    }
                }
            }
            catch (Exception exception)
            {
                _Logger.Error(string.Format("Unable to check if '{0}' database of '{1}' database instance is a Pulse database.", sqlConnection.Database, sDatabaseInstanceName), exception);
                _Logger.Info(string.Format("Failed to check if '{0}' database of '{1}' database instance is a Pulse database.", sqlConnection.Database, sDatabaseInstanceName));
                _Logger.Info(string.Format("Failed to process '{0}' database of '{1}' database instance.", sqlConnection.Database, sDatabaseInstanceName));
            }
        }

        private static void ProcessTemplates(string sDatabaseInstanceName, SqlConnection sqlConnection, string sTemplateType, string sTableName, string sColumnName, string sNameColumnName, string sIdColumnName)
        {
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(string.Format("SELECT [{0}], [{1}], [{2}] FROM [{3}]", sIdColumnName, sNameColumnName, sColumnName, sTableName), sqlConnection))
            {
                try
                {
                    using (DataTable dataTable = new DataTable())
                    {
                        bool isAnyTemplateModified = false;

                        _Logger.Debug(string.Format("Processing {0}s.", sTemplateType));

                        sqlDataAdapter.Fill(dataTable);

                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            if (!dataRow.IsNull(sColumnName))
                            {
                                string sContent = dataRow[sColumnName].ToString().Trim();

                                if (!string.IsNullOrEmpty(sContent))
                                {
                                    if (ProcessTemplate(ref sContent, sTemplateType, dataRow[sNameColumnName].ToString().Trim(), dataRow[sIdColumnName].ToString().Trim(), sDatabaseInstanceName, sqlConnection))
                                    {
                                        dataRow[sColumnName] = sContent;

                                        isAnyTemplateModified = true;
                                    }
                                }
                            }
                        }

                        if (isAnyTemplateModified)
                        {
                            try
                            {
                                _Logger.Debug(string.Format("Updating modified {0}s to '{1}' database of '{2} database instance'.", sTemplateType, sqlConnection.Database, sDatabaseInstanceName));


                                sqlDataAdapter.UpdateCommand = new SqlCommandBuilder(sqlDataAdapter).GetUpdateCommand();

                                sqlDataAdapter.Update(dataTable);

                                _Logger.Debug(string.Format("Finished updating modified {0}s to '{1}' database of '{2} database instance'.", sTemplateType, sqlConnection.Database, sDatabaseInstanceName));
                            }
                            catch (Exception exception)
                            {
                                _ReportParametersMailChimpAttributeCleanup.RemoveAll(p => p.TemplateType == sTemplateType && p.DatabaseInstanceName == sDatabaseInstanceName && p.DatabaseName == sqlConnection.Database);
                                _ReportParametersMalformedEmailTemplate.RemoveAll(p => p.TemplateType == sTemplateType && p.DatabaseInstanceName == sDatabaseInstanceName && p.DatabaseName == sqlConnection.Database);

                                _Logger.Error(string.Format("Something unexpected occurred while updating modified {0}s to '{1}' database of '{2} database instance'.", sTemplateType, sqlConnection.Database, sDatabaseInstanceName), exception);
                                _Logger.Info(string.Format("Failed to update modified {0}s for '{1}' database of '{2}' database instance.", sTemplateType, sqlConnection.Database, sDatabaseInstanceName));
                                _Logger.Info(string.Format("Failed to process {0}s.", sTemplateType));

                                return;
                            }
                        }

                        _Logger.Debug(string.Format("Finished processing {0}s.", sTemplateType));
                    }
                }
                catch (Exception exception)
                {
                    _Logger.Error(string.Format("Something unexpected occurred while processing {0}s for '{1}' database of '{2}' database instance.", sTemplateType, sqlConnection.Database, sDatabaseInstanceName), exception);
                    _Logger.Info(string.Format("Failed to process {0}s for '{1}' database of '{2}' database instance.", sTemplateType, sqlConnection.Database, sDatabaseInstanceName));
                }
            }
        }

        private static bool ProcessTemplate(ref string sContent, string sTemplateType, string sTemplateName, string sTemplateId, string sDatabaseInstanceName, SqlConnection sqlConnection)
        {
            string sNewContent = sContent;
            bool bReturnValue = false;
            HtmlDocument htmlDocument = new HtmlDocument();

            try
            {
                _Logger.Debug(string.Format("Processing {0} with name '{1}' & id '{2}'.", sTemplateType, sTemplateName, sTemplateId));

                if (_RegexMailChimpAttribute.IsMatch(sNewContent))
                {
                    sNewContent = _RegexMailChimpAttribute.Replace(sNewContent, string.Empty);
                    _Logger.Debug(string.Format("String 'mc:edit=\"std_content00\"' removed from {0} with name '{1}' & id '{2}' for '{3}' database of '{4}' database instance.", sTemplateType, sTemplateName, sTemplateId, sqlConnection.Database, sDatabaseInstanceName));
                    bReturnValue = true;
                }

                if (sNewContent.Contains("&lt;div"))
                {
                    sNewContent = sNewContent.Replace("&lt;div", "<div");
                    _Logger.Debug(string.Format("String '&lt;div' replaced with '<div' from {0} with name '{1}' & id '{2}' for '{3}' database of '{4}' database instance.", sTemplateType, sTemplateName, sTemplateId, sqlConnection.Database, sDatabaseInstanceName));
                    bReturnValue = true;
                }

                if (sNewContent.Contains("/&gt;"))
                {
                    sNewContent = sNewContent.Replace("/&gt;", "/>");
                    _Logger.Debug(string.Format("String '/&gt;' replaced with '/>' from {0} with name '{1}' & id '{2}' for '{3}' database of '{4}' database instance.", sTemplateType, sTemplateName, sTemplateId, sqlConnection.Database, sDatabaseInstanceName));
                    bReturnValue = true;
                }

                if (bReturnValue)
                {
                    _Logger.Debug(string.Format("{0} with name '{1}' & id '{2}' for '{3}' database of '{4}' database instance modified from following ====>>\r\n{5}\r\n<<==== to ====>>\r\n{6}\r\n<<====.", sTemplateType, sTemplateName, sTemplateId, sqlConnection.Database, sDatabaseInstanceName, sContent, sNewContent));
                    sContent = sNewContent;

                    _ReportParametersMailChimpAttributeCleanup.Add(new ReportParameters(sTemplateType, sDatabaseInstanceName, sqlConnection.Database, sTemplateId, sTemplateName));
                }

                htmlDocument.LoadHtml(sContent);
                if (htmlDocument.ParseErrors != null)
                {
                    int errorCounter = 1;
                    List<string> htmlParsingErrors = new List<string>();

                    foreach (HtmlParseError htmlParseError in htmlDocument.ParseErrors)
                    {
                        if (errorCounter > 1)
                        {
                            htmlParsingErrors.Add("\r\n");
                        }
                        htmlParsingErrors.Add(string.Format("ISSUE # {0}: {1} - '{2}' on line # {3} & position # {4} for following text:\r\n\t\t{5}", errorCounter, htmlParseError.Code, htmlParseError.Reason, htmlParseError.Line, htmlParseError.LinePosition, htmlParseError.SourceText));

                        errorCounter++;
                    }

                    if (errorCounter > 1)
                    {
                        _Logger.Error(string.Format("{0} with name '{1}' & id '{2}' for '{3}' database of '{4}' database instance has following HTML parsing error(s):\r\n{5}", sTemplateType, sTemplateName, sTemplateId, sqlConnection.Database, sDatabaseInstanceName, string.Concat(htmlParsingErrors.ToArray())));

                        ReportParameters reportParameters = new ReportParameters(sTemplateType, sDatabaseInstanceName, sqlConnection.Database, sTemplateId, sTemplateName);
                        reportParameters.Errors = htmlParsingErrors;

                        _ReportParametersMalformedEmailTemplate.Add(reportParameters);
                    }
                }

                _Logger.Debug(string.Format("Finished processing {0} with name '{1}' & id '{2}'.", sTemplateType, sTemplateName, sTemplateId));
            }
            catch (Exception exception)
            {
                _Logger.Error(string.Format("Something unexpected occurred while processing {0} with name '{1}' & id '{2}'.", sTemplateType, sTemplateName, sTemplateId), exception);
                _Logger.Info(string.Format("Failed to process {0} with name '{1}' & id '{2}' for '{3}' database of '{4}' database instance.", sTemplateType, sTemplateName, sTemplateId, sqlConnection.Database, sDatabaseInstanceName));

                return false;
            }

            return bReturnValue;
        }

        private static void MailReports()
        {
            if (_ReportParametersMailChimpAttributeCleanup.Count > 0)
            {
                MailReport(
                    ConfigurationManager.AppSettings["MailChimpAttributeCleanupReportSenderEmail"],
                    ConfigurationManager.AppSettings["MailChimpAttributeCleanupReportSenderDisplayName"],
                    ConfigurationManager.AppSettings["MailChimpAttributeCleanupReportRecipients"],
                    ConfigurationManager.AppSettings["MailChimpAttributeCleanupReportSubject"],
                    ConfigurationManager.AppSettings["MailChimpAttributeCleanupReportReplyToEmail"],
                    ConfigurationManager.AppSettings["MailChimpAttributeCleanupReportReplyToDisplayName"],
                    "<table border='1' width='100%'><tr><th>Database Instance</th><th>Database</th><th>Template Type</th><!--<th>Template Id</th>--><th>Template Name</th></tr>" + string.Concat(_ReportParametersMailChimpAttributeCleanup.Select(p => string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><!--<td>{3}</td>--><td>{4}</td></tr>", p.DatabaseInstanceName, p.DatabaseName, p.TemplateType, p.TemplateId, p.TemplateName)).ToArray()) + "</table>"
                );
            }

            if (_ReportParametersMalformedEmailTemplate.Count > 0)
            {
                MailReport(
                    ConfigurationManager.AppSettings["MalformedEmailTemplateReportSenderEmail"],
                    ConfigurationManager.AppSettings["MalformedEmailTemplateReportSenderDisplayName"],
                    ConfigurationManager.AppSettings["MalformedEmailTemplateReportRecipients"],
                    ConfigurationManager.AppSettings["MalformedEmailTemplateReportSubject"],
                    ConfigurationManager.AppSettings["MalformedEmailTemplateReportReplyToEmail"],
                    ConfigurationManager.AppSettings["MalformedEmailTemplateReportReplyToDisplayName"],
                    "<table border='1' width='100%'><tr><th>Database Instance</th><th>Database</th><th>Template Type</th><!--<th>Template Id</th>--><th>Template Name</th><th>Error(s)</th></tr>" + string.Concat(_ReportParametersMalformedEmailTemplate.Select(p => string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><!--<td>{3}</td>--><td>{4}</td><td>{5}</td></tr>", p.DatabaseInstanceName, p.DatabaseName, p.TemplateType, p.TemplateId, p.TemplateName, HttpUtility.HtmlEncode(string.Concat(p.Errors.ToArray())).Replace("\r\n", "<br/>").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;"))).ToArray()) + "</table>"
                );
            }
        }

        private static void MailReport(string sSenderEmail, string sSenderDisplayName, string sRecipientEmails, string sSubject, string sReplyToEmail, string sReplyToDisplayName, string sBody)
        {
            try
            {
                _Logger.Debug(string.Format("Sending {0}.", sSubject));

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(sSenderEmail, sSenderDisplayName, System.Text.UTF8Encoding.UTF8);
                    mailMessage.To.Add(sRecipientEmails);
                    mailMessage.SubjectEncoding = System.Text.UTF8Encoding.UTF8;
                    mailMessage.Subject = sSubject;
                    mailMessage.BodyEncoding = System.Text.UTF8Encoding.UTF8;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = sBody;
                    mailMessage.ReplyTo = new MailAddress(sReplyToEmail, sReplyToDisplayName, System.Text.UTF8Encoding.UTF8);
                    mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    mailMessage.Priority = MailPriority.High;

                    _SmtpClient.Send(mailMessage);
                }

                _Logger.Debug(string.Format("Finished sending {0}.", sSubject));
            }
            catch (Exception exception)
            {
                _Logger.Error(string.Format("Something unexpected occurred while sending {0}.", sSubject), exception);
                _Logger.Info(string.Format("Failed to send {0}.", sSubject));
            }
        }
    }

    internal class ReportParameters
    {
        public string TemplateType { get; set; }
        public string DatabaseInstanceName { get; set; }
        public string DatabaseName { get; set; }
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }
        public List<string> Errors { get; set; }

        public ReportParameters(string sTemplateType, string sDatabaseInstanceName, string sDatabaseName, string sTemplateId, string sTemplateName)
        {
            this.TemplateType = sTemplateType;
            this.DatabaseInstanceName = sDatabaseInstanceName;
            this.DatabaseName = sDatabaseName;
            this.TemplateId = sTemplateId;
            this.TemplateName = sTemplateName;
            this.Errors = new List<string>();
        }
    }
}
