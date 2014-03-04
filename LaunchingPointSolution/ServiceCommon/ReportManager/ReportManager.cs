using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using Common;
using EmailManager;
using LP2.Service.Common;
using ReportManager;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;
using focusIT;

namespace LP2Service
{
    public class ReportManager
    {
        short Category = 31;
        static DataAccess.DataAccess m_da = null;
        static ReportManager m_Instance = null;
        static int m_refCount = 0;
        public static EmailMgr m_EmailMgr = null;

        public static ReportManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new ReportManager();
                }
                lock (m_Instance)
                {
                    m_refCount++;
                }
                return m_Instance;
            }
        }

        public ReportManager()
        {
            if (m_da == null)
                m_da = new DataAccess.DataAccess();
            if (m_EmailMgr == null)
                m_EmailMgr = EmailMgr.Instance;

            if (m_Instance != null)
                return;
        }

        public PreviewLSRResponse PreviewLSR(PreviewLSRRequest req)
        {

            GenerateReportRequest genRptReq = new GenerateReportRequest();
            PreviewLSRResponse resp = new PreviewLSRResponse();
            bool status = false;
            string err = string.Empty;
            genRptReq.hdr = req.hdr;
            genRptReq.FileId = req.FileId;
            int TemplReportId = 0;

            try
            {
                string sSql = "select top 1 TemplReportId from Template_Reports where [Name] ='LSR'";
                object obj = DbHelperSQL.GetSingle(sSql);
                TemplReportId = obj == null ? 0 : (int)obj;
                if (TemplReportId <= 0)
                {
                    err = "Failed to get the Report Template for LSR.";
                    return resp;
                }
                genRptReq.TemplReportId = TemplReportId;
                genRptReq.External = req.ContactId > 0;
                genRptReq.Preview = true;
                var genRptResp = GenerateReport(genRptReq);
                if (genRptResp == null)
                {
                    err = "Failed to generate the LSR.";
                    return resp;
                }
                resp.hdr = genRptResp.hdr;
                resp.ReportContent = genRptResp.ReportContent;

                status = true;
                return resp;
            }
            catch (Exception exception)
            {
                err = "Failed to generate report, Exception:" + exception.Message;
                return resp;
            }
            finally
            {
                if (resp.hdr == null)
                    resp.hdr = new RespHdr();
                if (status == false)
                    resp.hdr.StatusInfo = err;
                resp.hdr.Successful = status;
            }

        }
        public GenerateReportResponse GenerateReport(GenerateReportRequest req)
        {
            GenerateReportResponse resp = new GenerateReportResponse();
            bool status = false;
            string err = string.Empty;
            try
            {
                string sSql = "select HtmlTemplContent from Template_Reports where TemplReportId=" + req.TemplReportId;
                DataTable dt = DbHelperSQL.ExecuteDataTable(sSql);
                //int Event_id = 3001;
                //short Category = 30;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, string.Format("GenerateReport, FileId={0}, External={1},LoanAutoEmaiLId={2}", req.FileId, req.External.ToString(), req.LoanAutoEmailId), EventLogEntryType.Information, Event_id, Category);
                string sHtmlBody = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    sHtmlBody = dt.Rows[0][0].ToString();
                    //Replace emailbody with Loan Info. 
                    GetTemplateReportContent(req.FileId, ref sHtmlBody, out err, req.External, req.LoanAutoEmailId, req.hdr.UserId, req.Preview);

                    resp.ReportContent = Encoding.UTF8.GetBytes(sHtmlBody);
                }

                status = true;
                return resp;
            }
            catch (Exception ex)
            {
                err = "Failed to generate report, Exception:" + ex.Message;
                return resp;
            }
            finally
            {
                resp.hdr = new RespHdr();
                if (status == false)
                    resp.hdr.StatusInfo = err;
                resp.hdr.Successful = status;
            }
        }

        public void EmailLSR()
        {
            int Event_id = 3001;
            string err = string.Empty;
            string err1 = string.Empty;
            string err2 = string.Empty;

            List<Table.LSRInfo> lstLsrInfo = m_da.GetLSRInfo();
            //filte out 
            var query = from item in lstLsrInfo
                        //where ((item.XTD >= 7 && item.ScheduleType == 2) || (item.XTD >= 1 && item.ScheduleType == 1) || (item.XTD==0))
                        where ((item.ScheduleType == 2) || (item.XTD >= 1 && item.ScheduleType == 1) || (item.XTD == 0 && item.ScheduleType != 0))
                        && item.TemplReportId != 0
                        select item;

            GenerateReportRequest req = null;
            GenerateReportResponse generateReportResponse = null;
            bool isExternal = false;
            string subject = string.Empty;
            string emailBody = string.Empty;

            err1 = string.Format("ReportManager::EmailLSR [Message detail]: " +
                                             " BackgroundLoanAlertPage = '{0}'," +
                                             " BackgroundWCFURL = '{1}'," +
                                             " BorrowerGreeting = '{2}'," +
                                             " BorrowerURL = '{3}'," +
                                             " DefaultAlertEmail = '{4}'," +
                                             " Domain = '{5}'," +
                                             " EmailAlertsEnabled = '{6}'," +
                                             " EmailInterval = '{7}'," +
                                             " EmailRelayServer = '{8}'," +
                                             " EwsUrl = '{9}'," +
                                             " ExchangeVersion = '{10}'," +
                                             " HomePageLogo = '{11}'," +
                                             " HomePageLogoData = '{12}'," +
                                             " LogoForSubPages = '{13}'," +
                                             " LPCompanyURL = '{14}'," +
                                             " SendEmailViaEWS = '{15}'," +
                                             " SubPageLogoData = '{16}'",
                                             EmailMgr._emailServerSetting.BackgroundLoanAlertPage,
                                             EmailMgr._emailServerSetting.BackgroundWCFURL,
                                             EmailMgr._emailServerSetting.BorrowerGreeting,
                                             EmailMgr._emailServerSetting.BorrowerURL,
                                             EmailMgr._emailServerSetting.DefaultAlertEmail,
                                             EmailMgr._emailServerSetting.EWS_Domain,
                                             EmailMgr._emailServerSetting.EmailAlertsEnabled,
                                             EmailMgr._emailServerSetting.EmailInterval,
                                             EmailMgr._emailServerSetting.EmailRelayServer,
                                             EmailMgr._emailServerSetting.EwsUrl,
                                             EmailMgr._emailServerSetting.EWS_Version,
                                             EmailMgr._emailServerSetting.HomePageLogo,
                                             EmailMgr._emailServerSetting.HomePageLogoData,
                                             EmailMgr._emailServerSetting.LogoForSubPages,
                                             EmailMgr._emailServerSetting.LPCompanyURL,
                                             EmailMgr._emailServerSetting.SendEmailViaEWS,
                                             EmailMgr._emailServerSetting.SubPageLogoData);


            string smtpHost = EmailMgr._emailServerSetting.EmailRelayServer;
            List<MailAddress> tos = null;
            List<MailAddress> ccs = null;
            List<MailAddress> bcc = null;
            bool sentStatus = false;
            err = string.Empty;
            foreach (var info in query)
            {
                err2 = string.Empty;

                err2 = string.Format(" [Message detail]: " +
                                             " Borrower = '{0}'," +
                                             " FileId = '{1}'," +
                                             " LoanAutoEmailid = '{2}'," +
                                             " ScheduleType = '{3}'," +
                                             " TemplReportId = '{4}'," +
                                             " ToContactEmail = '{5}'," +
                                             " ToContactId = '{6}'," +
                                             " ToContactUserName = '{7}'," +
                                             " ToUserEmail = '{8}'," +
                                             " ToUserId = '{9}'," +
                                             " ToUserUserName = '{10}'," +
                                             " XTD = '{11}'",
                                             info.Borrower,
                                             info.FileId.ToString(),
                                             info.LoanAutoEmailid.ToString(),
                                             info.ScheduleType.ToString(),
                                             info.TemplReportId.ToString(),
                                             info.ToContactEmail,
                                             info.ToContactId.ToString(),
                                             info.ToContactUserName,
                                             info.ToUserEmail,
                                             info.ToUserId.ToString(),
                                             info.ToUserUserName,
                                             info.XTD.ToString());

                #region Get Default Sender Email
                string sDefaultSenderEmail = "";
                string sDefaultSenderName = "";
                List<Table.CompanyReport> lstCompanyReportInfo = m_da.GetCompanyReportInfo();
                if (lstCompanyReportInfo.Count > 0)
                {
                    Table.CompanyReport companyreport = lstCompanyReportInfo[0];
                    if (companyreport.SenderRoleId > 0)
                    {
                        sDefaultSenderEmail = m_da.GetSendRoleUserEmail(companyreport.SenderRoleId, info.FileId);
                        sDefaultSenderName = m_da.GetSendRoleUserName(companyreport.SenderRoleId, info.FileId);
                    }
                    else if (companyreport.SenderEmail != "")
                    {
                        sDefaultSenderEmail = companyreport.SenderEmail;
                        sDefaultSenderName = companyreport.SenderName;
                    }
                }

                if (sDefaultSenderEmail == "")
                {
                    if (string.IsNullOrEmpty(EmailMgr._emailServerSetting.DefaultAlertEmail))
                    {
                        err = string.Format("Warning Message:DefaultAlertEmail will be used as the From Email Address can not be empty. ");
                        err = err + err1;
                        Event_id = 3011;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return;
                    }
                    sDefaultSenderEmail = EmailMgr._emailServerSetting.DefaultAlertEmail;
                }
                MailAddress mailFrom;
                if (sDefaultSenderName == "")
                {
                    mailFrom = new MailAddress(sDefaultSenderEmail);
                }
                else
                {
                    mailFrom = new MailAddress(sDefaultSenderEmail, sDefaultSenderName);
                }
                #endregion


                #region Check Send Condition
                if (info.LastRun.Year.ToString() != "1")
                {
                    if (lstCompanyReportInfo.Count == 0)
                    {
                        return;
                    }
                    Table.CompanyReport companyreport = lstCompanyReportInfo[0];
                    //TimeSpan ts = DateTime.Now - info.LastRun;
                    if (info.ScheduleType == 1)  //everyday
                    {
                        if (companyreport.TOD != DateTime.Now.Hour)
                        {
                            continue;
                        }
                        if (info.LastRun.AddHours(24) > DateTime.Now)
                        {
                            continue;
                        }
                    }
                    else if (info.ScheduleType == 2) //everyweek
                    {
                        if (companyreport.TOD != DateTime.Now.Hour)
                        {
                            continue;
                        }
                        if (companyreport.DOW != GetDayOfWeek(DateTime.Now.DayOfWeek.ToString()))
                        {
                            continue;
                        }
                        if (info.LastRun.AddHours(24) > DateTime.Now)
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    if (lstCompanyReportInfo.Count == 0)
                    {
                        return;
                    }
                    Table.CompanyReport companyreport = lstCompanyReportInfo[0];
                    if (info.ScheduleType == 2) //usually, lastRuntime is null must be a new weekly record in this condition.
                    {
                        if (companyreport.TOD != DateTime.Now.Hour)
                        {
                            continue;
                        }
                        if (companyreport.DOW != GetDayOfWeek(DateTime.Now.DayOfWeek.ToString()))
                        {
                            continue;
                        }
                    }
                }

                #endregion

                if (info.ToContactId > 0)
                    isExternal = true;

                req = new GenerateReportRequest();
                req.TemplReportId = info.TemplReportId;
                req.FileId = info.FileId;
                req.External = isExternal;
                req.LoanAutoEmailId = info.LoanAutoEmailid;
                req.hdr = new ReqHdr();
                string message = string.Empty;
                //subject
                subject = "Your Loan Status Update";
                string logErr = string.Empty;
                //subject = string.Format("Loan Status Report for {0}'s loan", info.Borrower.Trim());

                //email body
                try
                {
                    logErr = string.Empty;
                    generateReportResponse = GenerateReport(req);
                    if (generateReportResponse.hdr != null && generateReportResponse.hdr.Successful)
                    {
                        if (generateReportResponse.ReportContent != null && generateReportResponse.ReportContent.Length > 0)
                        {
                            //todo:check image data
                            emailBody = Encoding.UTF8.GetString(generateReportResponse.ReportContent);

                        }
                    }
                    else if (generateReportResponse.hdr != null)
                    {
                        Event_id = 3021;
                        err = string.Format("ReportManager::EmailLSR  Generate report faild for LoanAutoEmailId={0}, FileId={1},TemplReportId={2},error:{3}", info.LoanAutoEmailid, info.FileId, info.TemplReportId, generateReportResponse.hdr.StatusInfo);
                        err = err + err2;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        Trace.TraceError(err);
                        continue;
                    }
                    else
                    {
                        Event_id = 3031;
                        err = string.Format("ReportManager::EmailLSR  Generate report faild for LoanAutoEmailId={0}, FileId={1},TemplReportId={2}", info.LoanAutoEmailid, info.FileId, info.TemplReportId);
                        err = err + err2;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        Trace.TraceError(err);
                        continue;
                    }


                    //tos
                    tos = new List<MailAddress>();
                    if (info.ToUserId > 0)
                    {
                        MailAddress toUser = null;
                        if (!string.IsNullOrEmpty(info.ToUserEmail))
                        {
                            if (!string.IsNullOrEmpty(info.ToUserUserName))
                            {
                                toUser = new MailAddress(info.ToUserEmail, info.ToUserUserName);
                            }
                            else
                            {
                                toUser = new MailAddress(info.ToUserEmail);
                            }
                            tos.Add(toUser);
                        }
                        else
                        {
                            Event_id = 3051;
                            err = string.Format("ReportManager::EmailLSR   ToUserEmail Is Empty, ToUserId={0}", info.ToUserId);
                            err = err + err2;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                            Trace.TraceError(err);
                            continue;
                        }

                    }

                    if (info.ToContactId > 0)
                    {
                        MailAddress toContact = null;
                        if (!string.IsNullOrEmpty(info.ToContactEmail))
                        {
                            if (!string.IsNullOrEmpty(info.ToContactUserName))
                            {
                                toContact = new MailAddress(info.ToContactEmail, info.ToContactUserName);
                            }
                            else
                            {
                                toContact = new MailAddress(info.ToContactEmail);
                            }
                            tos.Add(toContact);
                        }
                        else
                        {
                            Event_id = 3061;
                            err = string.Format("ReportManager::EmailLSR   ToContactEmail Is Empty, ToContactId={0}", info.ToUserId);
                            err = err + err2;
                            short Category = 30;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                            Trace.TraceError(err);
                            continue;
                        }
                    }

                    sentStatus = EmailMgr.SendEmail(subject, emailBody, mailFrom, smtpHost, 25, tos, ccs, bcc, true, null);

                }
                catch (Exception exception)
                {
                    Event_id = 3071;
                    message = "ReportManager::EmailLSR " + err2 + "\r\n\r\nException: " + exception.Message + "\r\n\r\nStackTrace: " + exception.StackTrace;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, message, EventLogEntryType.Error, Event_id, Category);
                    Trace.TraceError(message);
                }


                try
                {
                    logErr = message;
                    if (!string.IsNullOrEmpty(logErr))
                        Trace.TraceError(logErr);
                    int emailLogId = 0;
                    if (m_da.EmailLog(info.ToUserId.ToString(), info.ToContactId.ToString(), 0, sentStatus, message, 0, 0, info.FileId, mailFrom.Address, 0, 0, emailBody, subject,
                                  false, "", 0, 0, "", 0, false, "", "", "", "", ref logErr, ref emailLogId) == false)
                    {
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, logErr, EventLogEntryType.Warning, Event_id, Category);
                    }
                    m_da.UpdateLoanAutoEmailStatus(info.LoanAutoEmailid, ref logErr);
                }
                catch (Exception exception)
                {
                    Event_id = 3081;
                    logErr = "ReportManager::EmailLSR, " + err + "Exception:" + exception.Message;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, logErr, EventLogEntryType.Error, Event_id, Category);
                    Trace.TraceError(logErr);
                }
            }
        }
        public void GetTemplateReportContent(int iFileID, ref string sReportContent, out string err, bool bExternal, int iLoanAutoEmailid, int iUserID, bool bPreview = false)
        {
            try
            {
                #region replace fields

                DataAccess.DataAccess dDateAccess = new DataAccess.DataAccess();
                Table.LoanAutoEmails loanAutoEmails = dDateAccess.GetLoanAutoEmailsByFileId(iFileID, iLoanAutoEmailid, out err);
                //bExternal = false;

                if (loanAutoEmails != null)
                    bExternal = loanAutoEmails.ToContactId > 0 ? true : false;

                #region 获取User.ShowTaskInLSR

                DataTable UserInfo = m_da.GetUserInfo(iUserID);
                bool bShowTaskInLSR = bExternal;

                if (iUserID > 0 && UserInfo.Rows.Count > 0 && UserInfo.Rows[0]["ShowTasksInLSR"] != DBNull.Value)
                {
                    bShowTaskInLSR = Convert.ToBoolean(UserInfo.Rows[0]["ShowTasksInLSR"]);
                }

                #endregion
                Table.ReportBranchInfo reportBranchInfo = dDateAccess.GetReportBranchInfoByFileId(iFileID, out err);
                Table.ReportLoanDetailInfo reportLoanDetailInfo = dDateAccess.GetReportLoanDetailInfoByFileId(iFileID, out err);
                Table.ReportLoanOfficerInfo reportLoanOfficerInfo = dDateAccess.GetReportLoanOfficerInfoByFileId(iFileID, out err);
                List<Table.ReportTaskInfo> ltreportTaskInfo = dDateAccess.GetReportLoanTaskInfoByFileId(iFileID, out err, bExternal, bShowTaskInLSR);
                List<Table.ReportLoanContactInfo> ltreportLoanContactInfo = dDateAccess.GetReportLoanContactInfoByFileId(iFileID, out err);
                var reportLoanOfficerUserLicenesNumber = dDateAccess.GetReportLoanOfficerUserLicenesNumber(iFileID, out err);
                DataSet ltreportImportMessages = null;
                if (bPreview)
                    ltreportImportMessages = dDateAccess.GetImportantMessagesPreview(iFileID, bExternal, out err);
                else
                    ltreportImportMessages = dDateAccess.GetImportantMessages(iLoanAutoEmailid, out err);

                string sCompanyName = dDateAccess.GetCompanyName();
                sReportContent = sReportContent.Replace("<@BranchName@>", sCompanyName);
                //sReportContent = sReportContent.Replace("<@BranchName@>", reportBranchInfo.Name);

                sReportContent = sReportContent.Replace("<@BranchAddress@>", reportBranchInfo.BranchAddress);
                sReportContent = sReportContent.Replace("<@BranchCity, BranchState BranchZip@>", reportBranchInfo.CityBranchStateZip);
                sReportContent = sReportContent.Replace("<@BranchPhone@>", reportBranchInfo.Phone);
                sReportContent = sReportContent.Replace("<@BranchEmail@>", reportBranchInfo.Email);
                sReportContent = sReportContent.Replace("<@BranchWebsiteUrl@>", reportBranchInfo.WebURL);
                sReportContent = sReportContent.Replace("<@BranchLogoPath@>", GetImageFormByte(reportBranchInfo.WebsiteLogo, 330, 90));

                sReportContent = sReportContent.Replace("<@ReportDate@>", DateTime.Now.ToString("MM/dd/yyyy"));

                sReportContent = sReportContent.Replace("<@StageProgressBarImagePath@>", GetImageFormByte(Bitmap2Bytes(GenerateStageProgressBarImage(reportLoanDetailInfo.FileId)), 6000, 32));
                sReportContent = sReportContent.Replace("<@PropertyAddress@>", reportLoanDetailInfo.PropertyAddr);
                sReportContent = sReportContent.Replace("<@Property City, State Zip@>", reportLoanDetailInfo.PropertyCity + " " + reportLoanDetailInfo.PropertyState + " " + reportLoanDetailInfo.PropertyZip);
                sReportContent = sReportContent.Replace("<@SalesPrice@>", Convert.ToDecimal(reportLoanDetailInfo.SalesPrice == null ? "0" : reportLoanDetailInfo.SalesPrice).ToString("n0"));
                sReportContent = sReportContent.Replace("<@LoanAmount@>", Convert.ToDecimal(reportLoanDetailInfo.LoanAmount == null ? "0" : reportLoanDetailInfo.LoanAmount).ToString("n0"));
                sReportContent = sReportContent.Replace("<@InterestRate@>", Convert.ToDecimal(reportLoanDetailInfo.Rate == null ? "0.0000" : reportLoanDetailInfo.Rate).ToString("n4"));
                sReportContent = sReportContent.Replace("<@LoanProgram@>", reportLoanDetailInfo.Program);

                var estClose = string.Empty;
                estClose = reportLoanDetailInfo.EstCloseDate == DateTime.MinValue
                               ? string.Empty
                               : reportLoanDetailInfo.EstCloseDate.ToString("MM/dd/yyyy");
                sReportContent = sReportContent.Replace("<@EstClose@>", estClose);

                sReportContent = sReportContent.Replace("<@Purpose@>", reportLoanDetailInfo.Purpose);

                // add Lender Name
                //string sLender = dDateAccess.GetLenderName(iFileID);
                sReportContent = sReportContent.Replace("<@Lender@>", "");
                sReportContent = sReportContent.Replace("Lender:", "");

                sReportContent = sReportContent.Replace("<@Borrower@>", reportLoanDetailInfo.BorrowerName);
                sReportContent = sReportContent.Replace("<@Coborrower@>", reportLoanDetailInfo.CoBorrowerName);
                sReportContent = sReportContent.Replace("<@MailingAddress@>", reportLoanDetailInfo.MailingAddr);
                sReportContent = sReportContent.Replace("<@Mailing City, Stage Zip@>", reportLoanDetailInfo.MailingCity + " " + reportLoanDetailInfo.MailingState + " " + reportLoanDetailInfo.MailingZip);
                sReportContent = sReportContent.Replace("<@MailingPhone@>", reportLoanDetailInfo.BusinessPhone);
                sReportContent = sReportContent.Replace("<@MailingFax@>", reportLoanDetailInfo.Fax);
                sReportContent = sReportContent.Replace("<@ProgressBarImagePath@>", GetImageFormByte(Bitmap2Bytes(DrawProgressBar(Convert.ToInt32(reportLoanDetailInfo.Progress))), 96, 14));

                sReportContent = sReportContent.Replace("<@LoanOfficerPicturePath@>", GetImageFormByte(reportLoanOfficerInfo.UserPictureFile, 73, 77));
                sReportContent = sReportContent.Replace("<@LoanOfficerName@>", reportLoanOfficerInfo.Name);
                sReportContent = sReportContent.Replace("<@LoanOfficerEmail@>", reportLoanOfficerInfo.EmailAddress);
                sReportContent = sReportContent.Replace("<@LoanOfficerPhone@>", reportLoanOfficerInfo.Phone);
                sReportContent = sReportContent.Replace("<@LoanOfficerFax@>", reportLoanOfficerInfo.Fax);
                sReportContent = sReportContent.Replace("<@LoanOfficerWebsite@>", reportBranchInfo.WebURL);

                #endregion

                #region Add LoanOffice NMLS And Licenses
                sReportContent = sReportContent.Replace("<@LoanOfficerNmls@>", reportLoanOfficerInfo.NMLS);
                sReportContent = sReportContent.Replace("<@LoanOfficerLicenses@>", reportLoanOfficerUserLicenesNumber);
                #endregion

                #region add Milestone List

                string sHtml_MilestoneList = this.GetHtml_MilestoneList(iFileID);
                if (sHtml_MilestoneList == string.Empty)
                {
                    sReportContent = sReportContent.Replace("<@ShowMilestone@>", "none");
                }
                else
                {
                    sReportContent = sReportContent.Replace("<@MilestoneList@>", sHtml_MilestoneList);
                    sReportContent = sReportContent.Replace("<@ShowMilestone@>", "block");
                }

                #endregion

                #region add Document List

                string sHtml_DocumentList = this.GetHtml_DocumentList(iFileID);
                sReportContent = sReportContent.Replace("<@DocumentList@>", sHtml_DocumentList);

                #endregion

                #region Add Important Message
                string sLoanImportMessages = GetLoanImportMessages(ltreportImportMessages);
                if (string.IsNullOrEmpty(sLoanImportMessages))
                    sReportContent = sReportContent.Replace("<@ImportMessages@>", string.Empty);
                else
                    sReportContent = sReportContent.Replace("<@ImportMessages@>", sLoanImportMessages);
                #endregion

                #region add Your Items
                if (ltreportTaskInfo != null && ltreportTaskInfo.Count > 0)
                    sReportContent = sReportContent.Replace("<@YourItems@>", GetLoanTaskInfo(ltreportTaskInfo));
                else
                    sReportContent = sReportContent.Replace("<@YourItems@>", "");
                #endregion

                sReportContent = sReportContent.Replace("<@LoanContactInfo@>", GetLoanContactInfo(ltreportLoanContactInfo));

                //sReportContent = sReportContent.Replace("<@PulseLogoPath@>", GetImageFormByte(Bitmap2Bytes(global::ReportManager.Properties.Resources.pulse_logo_long),100,50));
            }
            catch (Exception ex)
            {
                err = ex.Message;
                int Event_id = 3010;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
        }
        /// <summary>
        /// GetLoanImportMessages
        /// Changke 2012-04-28
        /// </summary>
        /// <returns></returns>
        public string GetLoanImportMessages(DataSet ltreportImportMessages)
        {
            //var sbHtml = new StringBuilder();
            //if (ltreportImportMessages.Tables.Count == 0)
            //{
            //    sbHtml.AppendLine("<tr>");
            //    sbHtml.AppendLine("<td height='10'>&nbsp;</td>");
            //    sbHtml.AppendLine("<td>&nbsp;</td>");
            //    sbHtml.AppendLine("<td>&nbsp;</td>");
            //    sbHtml.AppendLine("<tr>");
            //}
            //if (ltreportImportMessages.Tables.Count > 0 && ltreportImportMessages.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow item in ltreportImportMessages.Tables[0].Rows)
            //    {
            //        sbHtml.AppendLine(@"<tr>");
            //        sbHtml.AppendLine(@"<td colspan=""3"">");
            //        sbHtml.AppendLine(@"&nbsp;" + ConvertToHtmlEntities(item.Field<string>("note")));
            //        sbHtml.AppendLine(@"</td>");
            //        sbHtml.AppendLine(@"</tr>");
            //    }
            //}
            //return sbHtml.ToString();

            StringBuilder sbHtml = new StringBuilder();
            int i = 0;
            foreach (DataRow item in ltreportImportMessages.Tables[0].Rows)
            {
                sbHtml.AppendLine("<tr>");

                sbHtml.AppendLine("<td><p style=\"margin:0;padding:0;font-size:9pt;font-family:Arial,Helvetica,sans-serif;\">" + ConvertToHtmlEntities(item.Field<string>("note")) + "</p></td>");

                sbHtml.AppendLine("</tr>");

                i++;
            }
            return sbHtml.ToString();

        }

        public string ConvertToHtmlEntities(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            var sb = new StringBuilder(plainText.Length * 6);
            foreach (char c in plainText)
            {
                sb.Append("&#").Append((ushort)c).Append(';');
            }
            return sb.ToString();
        }
        /// <summary>
        /// Replace Loan Task Info
        /// Alex 2011-06-11
        /// </summary>
        /// <param name="ltreportTaskInfo"></param>
        /// <returns></returns>
        public string GetLoanTaskInfo(List<Table.ReportTaskInfo> ltreportTaskInfo)
        {
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.AppendLine("<tr>");
            sbHtml.AppendLine("    <td height=\"10\"></td>");
            sbHtml.AppendLine("</tr>");
            sbHtml.AppendLine("<tr>");
            sbHtml.AppendLine("    <td style=\"background-image: url(http://www.focusitinc.com/Email/Alert/images/bg-attention.gif); height:35px; background-repeat: no-repeat; padding-left: 15px;\" height=\"35\">");
            sbHtml.AppendLine("        <p style=\"color:#51890c;font-size:13pt;margin:0;padding:0;font-family:Arial,Helvetica,sans-serif;\">");
            sbHtml.AppendLine("            <strong>Items that require your attention</strong>");
            sbHtml.AppendLine("        </p>");
            sbHtml.AppendLine("    </td>");
            sbHtml.AppendLine("</tr>");

            sbHtml.AppendLine("<tr>");
            sbHtml.AppendLine("    <td valign=\"top\" style=\"background-image: url(http://www.focusitinc.com/Email/Alert/images/bg-grey.gif);background-repeat:repeat-x;border:solid 1px #a3a3a3;\">");
            sbHtml.AppendLine("        <table width=\"585\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">");
            sbHtml.AppendLine("            <tr>");
            sbHtml.AppendLine("                <td width=\"35\" height=\"10\"></td>");
            sbHtml.AppendLine("                <td width=\"360\" height=\"10\"></td>");
            sbHtml.AppendLine("                <td height=\"10\"></td>");
            sbHtml.AppendLine("            </tr>");

            int i = 0;
            foreach (Table.ReportTaskInfo rtInfo in ltreportTaskInfo)
            {
                if (i > 0)
                {
                    sbHtml.AppendLine("<tr>");
                    sbHtml.AppendLine("<td width=\"35\" height=\"10\"></td>");
                    sbHtml.AppendLine("<td width=\"360\" height=\"10\"></td>");
                    sbHtml.AppendLine("<td height=\"10\"></td>");
                    sbHtml.AppendLine("<tr>");
                }

                sbHtml.AppendLine("<tr>");
                if (rtInfo.ICON != string.Empty)
                {
                    sbHtml.AppendLine("<td>" + GetImageFormByte(Bitmap2Bytes(GetTaskIcon(rtInfo.ICON)), 16, 16) + "</td>");
                }
                else
                {
                    sbHtml.AppendLine("<td height=\"10\"></td>");
                }

                sbHtml.AppendLine("<td><p style=\"margin:0;padding:0;font-size:9pt;font-family:Arial,Helvetica,sans-serif;\">" + rtInfo.Name + "</p></td>");

                string sDueDate = rtInfo.Due.ToString("MM/dd/yyyy");
                if (sDueDate == "01/01/1900")
                {
                    sDueDate = string.Empty;
                }
                sbHtml.AppendLine("<td><p style=\"margin:0;padding:0;font-size:9pt;font-family:Arial,Helvetica,sans-serif;\">" + sDueDate + "</p></td>");
                sbHtml.AppendLine("</tr>");

                i++;
            }

            sbHtml.AppendLine("            <tr>");
            sbHtml.AppendLine("                <td width=\"35\" height=\"10\"></td>");
            sbHtml.AppendLine("                <td width=\"360\" height=\"10\"></td>");
            sbHtml.AppendLine("                <td height=\"10\"></td>");
            sbHtml.AppendLine("            </tr>");
            sbHtml.AppendLine("        </table>");
            sbHtml.AppendLine("    </td>");
            sbHtml.AppendLine("</tr>");


            return sbHtml.ToString();
        }

        /// <summary>
        /// Replace Loan Contact Info
        /// Alex 2011-06-11
        /// </summary>
        /// <param name="ltreportTaskInfo"></param>
        /// <returns></returns>
        public string GetLoanContactInfo(List<Table.ReportLoanContactInfo> ltreportLoanContactInfo)
        {
            if (ltreportLoanContactInfo.Count == 0)
            {
                return string.Empty;
            }

            // clone non-lender contact
            List<Table.ReportLoanContactInfo> NewContactList = new List<Table.ReportLoanContactInfo>();
            foreach (Table.ReportLoanContactInfo ContactItem in ltreportLoanContactInfo)
            {
                if (!ContactItem.RolesName.ToUpper().Contains("LEND") && ContactItem.RolesName != "Mortgage Broker")
                {
                    Table.ReportLoanContactInfo NewnContact = new Table.ReportLoanContactInfo();
                    NewnContact.BusinessPhone = ContactItem.BusinessPhone;
                    NewnContact.CompanyName = ContactItem.CompanyName;
                    NewnContact.ContactId = ContactItem.ContactId;
                    NewnContact.Email = ContactItem.Email;
                    NewnContact.Fax = ContactItem.Fax;
                    NewnContact.Name = ContactItem.Name;
                    NewnContact.Picture = ContactItem.Picture;
                    NewnContact.RolesName = ContactItem.RolesName;
                    NewnContact.Website = ContactItem.Website;

                    NewContactList.Add(NewnContact);
                }
            }

            StringBuilder sbHtml = new StringBuilder();

            if (NewContactList.Count > 0)
            {
                sbHtml.AppendLine("<tr>");
                sbHtml.AppendLine("    <td height=\"10\"></td>");
                sbHtml.AppendLine("</tr>");
                sbHtml.AppendLine("<tr>");
                sbHtml.AppendLine("    <td valign=\"top\" style=\"background-image: url('http://www.focusitinc.com/Email/Alert/images/bg-loan-contacts.gif'); padding-top:7px; padding-left:15px;\" height=\"35\">");
                sbHtml.AppendLine("        <p style=\"color:#fff;font-size:13pt;margin:0;padding:0;font-family:Arial,Helvetica,sans-serif;\"><strong>Your Loan Contacts</strong></p>");
                sbHtml.AppendLine("    </td>");
                sbHtml.AppendLine("</tr>");
                sbHtml.AppendLine("<tr>");
                sbHtml.AppendLine("    <td height=\"10\"></td>");
                sbHtml.AppendLine("</tr>");
                sbHtml.AppendLine("<tr>");
                sbHtml.AppendLine("    <td>");
                sbHtml.AppendLine("        <table width=\"585\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">");

                #region <@LoanContactInfo>

                int iPageCount = 0;
                int iRowCount = NewContactList.Count;
                int iPageSize = 2;
                int iYuShu = iRowCount % iPageSize;
                if (iYuShu == 0)
                {
                    iPageCount = iRowCount / iPageSize;
                }
                else
                {
                    iPageCount = (iRowCount - iYuShu) / iPageSize + 1;
                }

                for (int i = 1; i <= iPageCount; i++)
                {
                    int iPageIndex = i;
                    int iStartIndex = (iPageIndex - 1) * iPageSize + 1;
                    int iEndIndex = iStartIndex + iPageSize - 1;
                    if (iEndIndex > iRowCount)
                    {
                        iEndIndex = iRowCount;
                    }

                    sbHtml.AppendLine("<tr>");

                    Table.ReportLoanContactInfo rtInfo1 = NewContactList[iStartIndex - 1];
                    sbHtml.AppendLine("<td width='281' valign='top'>");
                    sbHtml.AppendLine("    <table width='100%' border='0' cellspacing='0' cellpadding='0'>");
                    sbHtml.AppendLine("        <tr>");
                    sbHtml.AppendLine("            <td height='30' colspan='2' valign='top'><p  style=\"color:#000;font-size:13pt;margin:0;padding:0;font-family:Arial,Helvetica,sans-serif;\"><strong>" + rtInfo1.RolesName + "</strong></p></td>");
                    sbHtml.AppendLine("        </tr>");
                    sbHtml.AppendLine("        <tr>");
                    sbHtml.AppendLine("            <td width='32%' valign='top'>" + GetImageFormByte(rtInfo1.Picture, 81, 85) + "</td>");
                    sbHtml.AppendLine("            <td width='68%' valign='top'>");
                    sbHtml.AppendLine("                <p style=\"line-height:18px;font-size:12px;margin:0;padding:0;word-break:break-all;font-family:Arial,Helvetica,sans-serif;\">");
                    sbHtml.AppendLine("                    " + rtInfo1.Name + "<br />");
                    sbHtml.AppendLine("                    " + rtInfo1.CompanyName + "<br />");
                    sbHtml.AppendLine("                    Email: <a href='mailto:" + rtInfo1.Email + "' style=\"color: #000; text-decoration: underline;font-family:Arial,Helvetica,sans-serif;\">" + rtInfo1.Email + "</a><br />");
                    sbHtml.AppendLine("                    Phone: " + rtInfo1.BusinessPhone + "<br />");
                    sbHtml.AppendLine("                    Fax: " + rtInfo1.Fax + "<br />");
                    sbHtml.AppendLine("                    <a href='http://" + rtInfo1.Website + "' style=\"color: #000; text-decoration: underline;font-family:Arial,Helvetica,sans-serif;\">" + rtInfo1.Website + "</a></p>");
                    sbHtml.AppendLine("             </td>");
                    sbHtml.AppendLine("        </tr>");
                    sbHtml.AppendLine("    </table>");
                    sbHtml.AppendLine("</td>");

                    if (iEndIndex > iStartIndex)
                    {
                        Table.ReportLoanContactInfo rtInfo2 = NewContactList[iEndIndex - 1];
                        sbHtml.AppendLine("<td width='43' align='center'>");
                        sbHtml.AppendLine("    <img src='http://www.focusitinc.com/Email/Alert/images/dotted-seprator.gif' width='6' height='128' alt='' />");
                        sbHtml.AppendLine("</td>");
                        sbHtml.AppendLine("<td width='261' valign='top'>");
                        sbHtml.AppendLine("    <table width='261' border='0' cellspacing='0' cellpadding='0'>");
                        sbHtml.AppendLine("        <tr>");
                        sbHtml.AppendLine("            <td height='30' colspan='2' valign='top'><p style=\"color:#000;font-size:13pt;margin:0;padding:0;font-family:Arial,Helvetica,sans-serif;\"><strong>" + rtInfo2.RolesName + "</strong></p></td>");
                        sbHtml.AppendLine("        </tr>");
                        sbHtml.AppendLine("        <tr>");
                        sbHtml.AppendLine("            <td width='90' valign='top'>" + GetImageFormByte(rtInfo2.Picture, 81, 85) + "</td>");
                        sbHtml.AppendLine("            <td width='171' valign='top'>");
                        sbHtml.AppendLine("                <p style=\"line-height:18px;font-size:12px;margin:0;padding:0;word-break:break-all;font-family:Arial,Helvetica,sans-serif;\">");
                        sbHtml.AppendLine("                    " + rtInfo2.Name + "<br />");
                        sbHtml.AppendLine("                    " + rtInfo2.CompanyName + "<br />");
                        sbHtml.AppendLine("                    Email: <a href='mailto:" + rtInfo2.Email + "' style=\"color: #000; text-decoration: underline;font-family:Arial,Helvetica,sans-serif;\">" + rtInfo2.Email + "</a><br />");
                        sbHtml.AppendLine("                    Phone: " + rtInfo2.BusinessPhone + "<br />");
                        sbHtml.AppendLine("                    Fax: " + rtInfo2.Fax + "<br />");
                        sbHtml.AppendLine("                    <a href='mailto:" + rtInfo2.Website + "' style=\"color: #000; text-decoration: underline;font-family:Arial,Helvetica,sans-serif;\">" + rtInfo2.Website + "</a></p>");
                        sbHtml.AppendLine("             </td>");
                        sbHtml.AppendLine("        </tr>");
                        sbHtml.AppendLine("    </table>");
                        sbHtml.AppendLine("</td>");
                    }

                    sbHtml.AppendLine("</tr>");
                }



                #endregion

                sbHtml.AppendLine("            <tr>");
                sbHtml.AppendLine("                <td height=\"10\"></td>");
                sbHtml.AppendLine("                <td height=\"10\"></td>");
                sbHtml.AppendLine("                <td height=\"10\"></td>");
                sbHtml.AppendLine("            </tr>");
                sbHtml.AppendLine("        </table>");
                sbHtml.AppendLine("    </td>");
                sbHtml.AppendLine("</tr>");
            }

            return sbHtml.ToString();
        }

        #region Drawing Stage Progress Bar Image by Neo
        public Bitmap GenerateStageProgressBarImage(int iLoanID)
        {
            // get loan stage data
            DataTable StageProgressDataTable = this.GetStageProgressDataTable(iLoanID);

            // set width and height of stage progress bar
            int iStageProgressBarWidth = (StageProgressDataTable.Rows.Count * 109 - 1) - ((StageProgressDataTable.Rows.Count - 1) * 15);
            int iStageProgressBarHeight = 32;

            Bitmap TargetBitmp = new Bitmap(iStageProgressBarWidth, iStageProgressBarHeight);
            Graphics g = Graphics.FromImage(TargetBitmp);

            int i = 0;
            foreach (DataRow StageProgressRow in StageProgressDataTable.Rows)
            {
                string sStageName = StageProgressRow["StageName"].ToString();
                string sCompletedDate = StageProgressRow["CompletedDate"].ToString() == string.Empty ? string.Empty : Convert.ToDateTime(StageProgressRow["CompletedDate"]).ToShortDateString();
                string sStageImageFileName = StageProgressRow["StageImageFileName"].ToString();

                int x = 0;
                if (i > 0)
                {
                    x = i * 94;
                }

                this.DrawStageProgressItemImage(g, sStageImageFileName, sStageName, sCompletedDate, x);
                i++;
            }

            return TargetBitmp;
        }

        public void DrawStageProgressItemImage(Graphics g, string sBackgroundImageFileName, string sStageName, string sCompletedDate, int x)
        {
            #region Write fonts to picutre

            Brush WhiteBrush = Brushes.White;
            Font ArialFont = new System.Drawing.Font("Arial", 11, GraphicsUnit.Pixel);

            #region Get BackgroundImage

            Bitmap BackgroundImage = null;
            if (sBackgroundImageFileName == "Grass_Left.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Grass_Left;
            }
            else if (sBackgroundImageFileName == "Grass_Mid.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Grass_Mid;
            }
            else if (sBackgroundImageFileName == "Grass_Right.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Grass_Right;
            }
            else if (sBackgroundImageFileName == "Gray_.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Gray_;
            }
            else if (sBackgroundImageFileName == "Gray_Left.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Gray_Left;
            }
            else if (sBackgroundImageFileName == "Gray_Left_Complete.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Gray_Left_Complete;
            }
            else if (sBackgroundImageFileName == "Gray_Mid.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Gray_Mid;
            }
            else if (sBackgroundImageFileName == "Gray_Mid_Complete.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Gray_Mid_Complete;
            }
            else if (sBackgroundImageFileName == "Gray_Right_Complete.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Gray_Right_Complete;
            }
            else if (sBackgroundImageFileName == "Green_Left.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Green_Left;
            }
            else if (sBackgroundImageFileName == "Green_Mid.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Green_Mid;
            }
            else if (sBackgroundImageFileName == "Green_Right.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Green_Right;
            }
            else if (sBackgroundImageFileName == "Orange_Left.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Orange_Left;
            }
            else if (sBackgroundImageFileName == "Orange_Mid.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Orange_Mid;
            }
            else if (sBackgroundImageFileName == "Orange_Right.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Orange_Right;
            }
            else if (sBackgroundImageFileName == "Pink_Left.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Pink_Left;
            }
            else if (sBackgroundImageFileName == "Pink_Mid.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Pink_Mid;
            }
            else if (sBackgroundImageFileName == "Pink_Right.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Pink_Right;
            }
            else if (sBackgroundImageFileName == "Red_Left.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Red_Left;
            }
            else if (sBackgroundImageFileName == "Red_Mid.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Red_Mid;
            }
            else if (sBackgroundImageFileName == "Red_Right.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Red_Right;
            }
            else if (sBackgroundImageFileName == "Silver_Left.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Silver_Left;
            }
            else if (sBackgroundImageFileName == "Silver_Left_Complete.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Silver_Left_Complete;
            }
            else if (sBackgroundImageFileName == "Silver_Mid.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Silver_Mid;
            }
            else if (sBackgroundImageFileName == "Silver_Mid_Complete.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Silver_Mid_Complete;
            }
            else if (sBackgroundImageFileName == "Silver_Right.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Silver_Right;
            }
            else if (sBackgroundImageFileName == "Silver_Right_Complete.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Silver_Right_Complete;
            }
            else if (sBackgroundImageFileName == "Yellow_Left.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Yellow_Left;
            }
            else if (sBackgroundImageFileName == "Yellow_Mid.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Yellow_Mid;
            }
            else if (sBackgroundImageFileName == "Yellow_Right.gif")
            {
                BackgroundImage = global::ReportManager.Properties.Resources.Yellow_Right;
            }

            #endregion

            g.DrawImage(BackgroundImage, x, 0);

            StringFormat StringFormat1 = StringFormat.GenericTypographic;
            StringFormat1.Alignment = StringAlignment.Center;
            StringFormat1.LineAlignment = StringAlignment.Center;

            Rectangle FontContainer = new Rectangle(x, 0, 98, 30);

            string sDrawString = sStageName;

            #region Add Space Chars

            if (sCompletedDate != string.Empty)
            {
                sDrawString = sStageName + Environment.NewLine + sCompletedDate;
            }

            #endregion

            g.DrawString(sDrawString, ArialFont, WhiteBrush, FontContainer, StringFormat1);

            #endregion
        }

        public DataTable GetStageProgressDataTable(int iLoanID)
        {
            DataTable StageProgressDataTable = new DataTable();
            string sConn = ConfigurationManager.ConnectionStrings["focusITConnectionString"].ConnectionString;
            using (SqlConnection SqlConn = new SqlConnection(sConn))
            {
                string sSql = "select dbo.lpfn_GetStageProgressImageFileName(LoanStageId) as StageImageFileName, dbo.lpfn_GetStageAlias(LoanStageId) as StageName, Completed as CompletedDate from LoanStages where FileId=" + iLoanID + " order by SequenceNumber";
                SqlCommand SqlCmd = new SqlCommand(sSql);
                SqlCmd.Connection = SqlConn;

                SqlDataAdapter SqlAdapter = new SqlDataAdapter(SqlCmd);
                SqlAdapter.Fill(StageProgressDataTable);
            }

            return StageProgressDataTable;
        }

        public Bitmap DrawProgressBar(int iPercent)
        {
            Bitmap TargetBitmp = new Bitmap(142, 20);
            Graphics g = Graphics.FromImage(TargetBitmp);

            // draw backgound
            g.DrawImage(global::ReportManager.Properties.Resources.ProgressBar_bg, 0, 0);

            // draw blue progress bar
            int iBlueBarWidth = iPercent * 140 / 100;
            for (int i = 0; i < iBlueBarWidth; i++)
            {
                g.DrawImage(global::ReportManager.Properties.Resources.ProgressBar_blue, i, 1);
            }

            // add percent text
            Color z = Color.FromArgb(129, 136, 146);
            Brush GrayBrush = new SolidBrush(z);
            Font ArialFont = new System.Drawing.Font("Arial", 11, GraphicsUnit.Pixel);

            StringFormat StringFormat1 = StringFormat.GenericTypographic;
            StringFormat1.Alignment = StringAlignment.Center;
            StringFormat1.LineAlignment = StringAlignment.Center;

            Rectangle FontContainer = new Rectangle(0, 0, 142, 19);

            g.DrawString(iPercent + "%", ArialFont, GrayBrush, FontContainer, StringFormat1);

            return TargetBitmp;
        }
        #endregion

        public byte[] Bitmap2Bytes(Bitmap bm)
        {

            MemoryStream ms = new MemoryStream();
            bm.Save(ms, ImageFormat.Png);
            ms.Flush();
            byte[] bData = ms.GetBuffer();
            ms.Close();
            return bData;

        }

        public string GetImageFormByte(byte[] bImages)
        {
            if (bImages == null)
            {
                return "";
            }
            string fieldValue = "";
            fieldValue = Convert.ToBase64String(bImages);
            return string.Format("<img src=\"cid:{0}\" data=\"{1}\" />", Guid.NewGuid().ToString(), fieldValue);
        }

        public string GetImageFormByte(byte[] bImages, int iWidth, int iHeight)
        {
            try
            {
                if (bImages == null)
                {
                    return "";
                }
                //MemoryStream ms1 = new MemoryStream(bImages);
                //Bitmap bm = (Bitmap)Image.FromStream(ms1);

                ImageConverter imc = new ImageConverter();
                Image _img = imc.ConvertFrom(null, null, bImages) as Image;


                //得到原始图片的大小
                int iOrgWidth = Convert.ToInt32(_img.Width);
                int iOrgHeight = Convert.ToInt32(_img.Height);
                //得到图片可显示的尺寸大小
                ResizeImg(iOrgWidth, iOrgHeight, ref iWidth, ref iHeight);
                //调整图片数据流的尺寸
                byte[] newbImages = Bitmap2Bytes(ImageSizeChange(bImages, iWidth, iHeight));

                string fieldValue = "";
                fieldValue = Convert.ToBase64String(newbImages);
                return string.Format("<img src=\"cid:{0}\" data=\"{1}\" style=\"width:{2}px; height:{3}px\" />", Guid.NewGuid().ToString(), fieldValue, iWidth, iHeight);
            }
            catch (Exception ex)
            {
                return "";
            }

            //try
            //{
            //    if (bImages == null)
            //    {
            //        return "";
            //    }
            //    //MemoryStream ms1 = new MemoryStream(bImages);
            //    //Bitmap bm = (Bitmap)Image.FromStream(ms1);

            //    ImageConverter imc = new ImageConverter();
            //    Image _img = imc.ConvertFrom(null, null, bImages) as Image;


            //    //得到原始图片的大小
            //    int iOrgWidth = Convert.ToInt32(_img.Width);
            //    int iOrgHeight = Convert.ToInt32(_img.Height);

            //    //得到图片可显示的尺寸大小
            //    //ResizeImg(iOrgWidth, iOrgHeight, ref iMaxWidth, ref iMaxHeight);

            //    int iNewWidth;
            //    int iNewHeight;
            //    this.GetImageScaleSize(iMaxHeight, iMaxHeight, iOrgWidth, iOrgHeight, out iNewWidth, out iNewHeight);

            //    //调整图片数据流的尺寸
            //    byte[] newbImages = Bitmap2Bytes(ImageSizeChange(bImages, iNewWidth, iNewHeight));

            //    string fieldValue = "";
            //    fieldValue = Convert.ToBase64String(newbImages);
            //    return string.Format("<img src=\"cid:{0}\" data=\"{1}\" style=\"width:{2}px; height:{3}px\" />", Guid.NewGuid().ToString(), fieldValue, iMaxWidth, iMaxHeight);
            //}
            //catch (Exception ex)
            //{
            //    return "";
            //}
        }

        /// <summary>
        ///  Reset Img Size for View
        /// </summary>
        /// <param name="iOrgWidth">图片原始宽度</param>
        /// <param name="iOrgHeight">图片原始高度</param>
        /// <param name="iWidth">页面规定图片显示最大宽度</param>
        /// <param name="iHeight">页面规定图片显示最大高度</param>
        public void ResizeImg(int iOrgWidth, int iOrgHeight, ref int iWidth, ref int iHeight)
        {
            int NewWidth = 0;
            int NewHeight = 0;

            if (iOrgWidth > iWidth && iOrgHeight < iHeight)
            {

                NewWidth = iWidth;
                string sHegiht = Convert.ToDecimal(iOrgHeight * iWidth / iOrgWidth).ToString();
                NewHeight = Convert.ToInt32(sHegiht);
            }
            else if (iOrgWidth < iWidth && iOrgHeight > iHeight)
            {
                string sWidth = Convert.ToDecimal(iOrgWidth * iHeight / iOrgHeight).ToString();
                NewWidth = Convert.ToInt32(sWidth);

                NewHeight = iHeight;
            }
            else if (iOrgWidth > iWidth && iOrgHeight > iHeight)
            {

                if (iWidth > iHeight)
                {

                    NewWidth = iWidth;
                    string sHegiht = Convert.ToDecimal(iOrgHeight * iWidth / iOrgWidth).ToString();
                    NewHeight = Convert.ToInt32(sHegiht);
                }
                else
                {
                    string sWidth = Convert.ToDecimal(iOrgWidth * iHeight / iOrgHeight).ToString();
                    NewWidth = Convert.ToInt32(sWidth);

                    NewHeight = iHeight;
                }
            }
            else
            {

                // if less than or equal to iWidth & iHeight, do not scale
                NewWidth = iOrgWidth;
                //alert("NewWidth5: " + NewWidth);

                NewHeight = iOrgHeight;
                //alert("NewHeight5: " + NewHeight);
            }

            iWidth = NewWidth;
            iHeight = NewHeight;
        }

        /// <summary>
        /// 调整图片流的尺寸
        /// </summary>
        /// <param name="_ImageByte"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <returns></returns>
        public System.Drawing.Bitmap ImageSizeChange(byte[] _ImageByte, int iWidth, int iHeight)
        {
            Bitmap _Bitmap = new Bitmap(iWidth, iHeight);
            Graphics _Graphcis = Graphics.FromImage(_Bitmap);
            _Graphcis.DrawImage(Image.FromStream(new MemoryStream(_ImageByte)), 0, 0, iWidth, iHeight);
            _Graphcis.Dispose();
            return _Bitmap;

        }

        #region 图片等比例缩放

        public void GetImageScaleSize(int MaxWidth, int MaxHeight, int ImageWidth, int ImageHeight, out int NewWidth, out int NewHeight)
        {
            #region get scaled size

            NewWidth = 0;
            NewHeight = 0;

            if (ImageWidth <= MaxWidth && ImageHeight <= MaxHeight)
            {

                NewWidth = ImageWidth;
                NewHeight = ImageHeight;
            }
            else
            {

                // scale by MaxWidth
                var Percent = MaxWidth / ImageWidth;

                NewWidth = MaxWidth;

                NewHeight = ImageHeight * Percent;

                if (NewHeight > MaxHeight)
                {

                    // scale by MaxHeight
                    Percent = MaxHeight / NewHeight;

                    NewWidth = NewWidth * Percent;

                    NewHeight = MaxHeight;
                }
            }

            #endregion
        }

        #endregion

        public Bitmap GetTaskIcon(string sIconName)
        {
            Bitmap TaskImage = null;
            switch (sIconName)
            {
                case "TaskGray.png":
                    TaskImage = global::ReportManager.Properties.Resources.TaskGray;
                    break;
                case "TaskGreen.png":
                    TaskImage = global::ReportManager.Properties.Resources.TaskGreen;
                    break;
                case "TaskRed.png":
                    TaskImage = global::ReportManager.Properties.Resources.TaskRed;
                    break;
                case "TaskYellow.png":
                    TaskImage = global::ReportManager.Properties.Resources.TaskYellow;
                    break;
                default:
                    TaskImage = global::ReportManager.Properties.Resources.TaskGray;
                    break;
            }
            return TaskImage;
        }

        public string GetHtml_MilestoneList(int iFileID)
        {
            DataAccess.DataAccess dDateAccess = new DataAccess.DataAccess();
            DataTable MilestoneList = dDateAccess.GetMilestoneList(iFileID);
            StringBuilder sbHtml = new StringBuilder();
            int i = 0;
            foreach (DataRow MilestoneRow in MilestoneList.Rows)
            {
                //string sStageName = MilestoneRow["StageName"].ToString();
                //string sAlias = MilestoneRow["Alias"].ToString();
                if (MilestoneRow["StageAlias"] == DBNull.Value)
                    continue;
                string sStage = MilestoneRow["StageAlias"].ToString();
                string sCompletedDate = MilestoneRow["Completed"] == DBNull.Value ? "Pending" : Convert.ToDateTime(MilestoneRow["Completed"]).ToShortDateString();
                if (sCompletedDate == "01/01/0001" || sCompletedDate == "01/01/1900")
                {
                    sCompletedDate = "Pending";
                }

                if (i == 0)
                {
                    sbHtml.AppendLine("<tr>");
                    sbHtml.AppendLine("<td style=\"word-break: break-all; padding-top: 8px;font-size:12px;font-family:Arial,Helvetica,sans-serif;\">" + sStage + "</td>");
                    sbHtml.AppendLine("<td style=\"word-break: break-all; padding-top: 8px;font-size:12px;font-family:Arial,Helvetica,sans-serif;\">" + sCompletedDate + "</td>");
                    sbHtml.AppendLine("</tr>");
                }
                else
                {
                    sbHtml.AppendLine("<tr>");
                    sbHtml.AppendLine("<td colspan=\"2\" style=\"border-bottom: dashed 1px gray;font-size:8px;color:#fff;font-family:Arial,Helvetica,sans-serif;\">x</td>");
                    sbHtml.AppendLine("</tr>");

                    sbHtml.AppendLine("<tr>");
                    sbHtml.AppendLine("<td style=\"word-break: break-all; padding-top: 8px;font-size:12px;font-family:Arial,Helvetica,sans-serif;\">" + sStage + "</td>");
                    sbHtml.AppendLine("<td style=\"word-break: break-all; padding-top: 8px;font-size:12px;font-family:Arial,Helvetica,sans-serif;\">" + sCompletedDate + "</td>");
                    sbHtml.AppendLine("</tr>");
                }

                i++;
            }

            return sbHtml.ToString();
        }

        public string GetHtml_DocumentList(int iFileID)
        {
            DataAccess.DataAccess dDateAccess = new DataAccess.DataAccess();
            DataTable LoanBasicDocList = dDateAccess.GetLoanBasicDocumentList(" and FileId=" + iFileID + " and  (not Ordered is null or not Received is null)");
            StringBuilder sbHtml = new StringBuilder();

            if (LoanBasicDocList.Rows.Count > 0)
            {
                sbHtml.AppendLine("<tr>");
                sbHtml.AppendLine("     <td style=\"background-image:url(http://www.focusitinc.com/Email/Alert/images/bg-dotted.png);height:1px\" height=\"1\"></td>");
                sbHtml.AppendLine("</tr>");

                sbHtml.AppendLine("<tr>");
                sbHtml.AppendLine("     <td height=\"10\"></td>");
                sbHtml.AppendLine("</tr>");

                sbHtml.AppendLine("<tr>");
                sbHtml.AppendLine("     <td height=\"23\" valign=\"top\"><p style=\"color:#51890c;font-size:13pt;margin:0;padding:0;font-family:Arial,Helvetica,sans-serif;\"><strong>Basic Loan Documents</strong></p></td>");
                sbHtml.AppendLine("</tr>");

                sbHtml.AppendLine("<tr>");
                sbHtml.AppendLine("    <td>");
                sbHtml.AppendLine("        <table cellpadding=\"0\" cellspacing=\"0\" style=\"font-family: Arial, Helvetica, sans-serif; font-size: 9pt;\">");
                sbHtml.AppendLine("            <tr>");
                sbHtml.AppendLine("                <td height=\"10\"></td>");
                sbHtml.AppendLine("                <td height=\"10\"></td>");
                sbHtml.AppendLine("            </tr>");
                sbHtml.AppendLine("            <tr>");
                sbHtml.AppendLine("                <th style=\"width: 300px;white-space: nowrap;cursor: pointer;padding: 3px 0px 3px 0px;text-align: left;text-decoration: underline;font-family:Arial,Helvetica,sans-serif;\">Document</th>");
                sbHtml.AppendLine("                <th style=\"width: 100px;white-space: nowrap;cursor: pointer;padding: 3px 0px 3px 0px;text-align: left;text-decoration: underline;font-family:Arial,Helvetica,sans-serif;\">Ordered</th>");
                sbHtml.AppendLine("                <th style=\"white-space: nowrap;cursor: pointer;padding: 3px 0px 3px 0px;text-align: left;text-decoration: underline;font-family:Arial,Helvetica,sans-serif;\">Received</th>");
                sbHtml.AppendLine("            </tr>");

                int j = 0;
                foreach (DataRow DocRow in LoanBasicDocList.Rows)
                {
                    string sDocName = DocRow["DocName"].ToString();
                    string sOrderedDate = DocRow["Ordered"].ToString() == string.Empty ? string.Empty : Convert.ToDateTime(DocRow["Ordered"]).ToShortDateString();
                    string sReceivedDate = DocRow["Received"].ToString() == string.Empty ? string.Empty : Convert.ToDateTime(DocRow["Received"]).ToShortDateString();

                    if (j == 0)
                    {
                        sbHtml.AppendLine("<tr>");
                        sbHtml.AppendLine("<td style=\"word-break: break-all; padding-top: 8px;font-size:12px;font-family:Arial,Helvetica,sans-serif;\">" + sDocName + "</td>");
                        sbHtml.AppendLine("<td style=\"word-break: break-all; padding-top: 8px;font-size:12px;font-family:Arial,Helvetica,sans-serif;\">" + sOrderedDate + "</td>");
                        sbHtml.AppendLine("<td style=\"word-break: break-all; padding-top: 8px;font-size:12px;font-family:Arial,Helvetica,sans-serif;\">" + sReceivedDate + "</td>");
                        sbHtml.AppendLine("</tr>");
                    }
                    else
                    {
                        sbHtml.AppendLine("<tr>");
                        sbHtml.AppendLine("<td colspan=\"3\" style=\"border-bottom: dashed 1px gray;font-size:8px;color:#fff;font-family:Arial,Helvetica,sans-serif;\">x</td>");
                        sbHtml.AppendLine("</tr>");

                        sbHtml.AppendLine("<tr>");
                        sbHtml.AppendLine("<td style=\"word-break: break-all; padding-top: 8px;font-size:12px;font-family:Arial,Helvetica,sans-serif;\">" + sDocName + "</td>");
                        sbHtml.AppendLine("<td style=\"word-break: break-all; padding-top: 8px;font-size:12px;font-family:Arial,Helvetica,sans-serif;\">" + sOrderedDate + "</td>");
                        sbHtml.AppendLine("<td style=\"word-break: break-all; padding-top: 8px;font-size:12px;font-family:Arial,Helvetica,sans-serif;\">" + sReceivedDate + "</td>");
                        sbHtml.AppendLine("</tr>");
                    }

                    j++;
                }

                sbHtml.AppendLine("        </table>");
                sbHtml.AppendLine("    </td>");
                sbHtml.AppendLine("</tr>");
            }

            return sbHtml.ToString();
        }

        public int GetDayOfWeek(string sDoW)
        {
            int iDoW = 1;
            switch (sDoW)
            {
                case "Monday":
                    iDoW = 1;
                    break;
                case "Tuesday":
                    iDoW = 2;
                    break;
                case "Wednesday":
                    iDoW = 3;
                    break;
                case "Thursday":
                    iDoW = 4;
                    break;
                case "Friday":
                    iDoW = 5;
                    break;
                case "Saturday":
                    iDoW = 6;
                    break;
                case "Sunday":
                    iDoW = 7;
                    break;
            }
            return iDoW;
        }

        public SendLSRResponse SendLSR(SendLSRRequest req1)
        {
            int Event_id = 3001;

            SendLSRResponse response = new SendLSRResponse();
            response.hdr = new RespHdr();
            response.hdr.Successful = false;

            string err = string.Empty;
            string err1 = string.Empty;
            string err2 = string.Empty;

            GenerateReportRequest req = null;
            GenerateReportResponse generateReportResponse = null;
            string subject = string.Empty;
            string emailBody = string.Empty;

            err1 = string.Format("ReportManager::EmailLSR [Message detail]: " +
                                             " BackgroundLoanAlertPage = '{0}'," +
                                             " BackgroundWCFURL = '{1}'," +
                                             " BorrowerGreeting = '{2}'," +
                                             " BorrowerURL = '{3}'," +
                                             " DefaultAlertEmail = '{4}'," +
                                             " Domain = '{5}'," +
                                             " EmailAlertsEnabled = '{6}'," +
                                             " EmailInterval = '{7}'," +
                                             " EmailRelayServer = '{8}'," +
                                             " EwsUrl = '{9}'," +
                                             " ExchangeVersion = '{10}'," +
                                             " HomePageLogo = '{11}'," +
                                             " HomePageLogoData = '{12}'," +
                                             " LogoForSubPages = '{13}'," +
                                             " LPCompanyURL = '{14}'," +
                                             " SendEmailViaEWS = '{15}'," +
                                             " SubPageLogoData = '{16}'",
                                             EmailMgr._emailServerSetting.BackgroundLoanAlertPage,
                                             EmailMgr._emailServerSetting.BackgroundWCFURL,
                                             EmailMgr._emailServerSetting.BorrowerGreeting,
                                             EmailMgr._emailServerSetting.BorrowerURL,
                                             EmailMgr._emailServerSetting.DefaultAlertEmail,
                                             EmailMgr._emailServerSetting.EWS_Domain,
                                             EmailMgr._emailServerSetting.EmailAlertsEnabled.ToString(),
                                             EmailMgr._emailServerSetting.EmailInterval.ToString(),
                                             EmailMgr._emailServerSetting.EmailRelayServer,
                                             EmailMgr._emailServerSetting.EwsUrl,
                                             EmailMgr._emailServerSetting.EWS_Version,
                                             EmailMgr._emailServerSetting.HomePageLogo,
                                             EmailMgr._emailServerSetting.HomePageLogoData,
                                             EmailMgr._emailServerSetting.LogoForSubPages,
                                             EmailMgr._emailServerSetting.LPCompanyURL,
                                             EmailMgr._emailServerSetting.SendEmailViaEWS.ToString(),
                                             EmailMgr._emailServerSetting.SubPageLogoData);


            string smtpHost = EmailMgr._emailServerSetting.EmailRelayServer;
            List<MailAddress> tos = null;
            List<MailAddress> ccs = null;
            List<MailAddress> bcc = null;
            bool sentStatus = false;

            err2 = string.Empty;


            #region Get Default Sender Email
            string sDefaultSenderEmail = "";
            string sDefaultSenderName = "";
            List<Table.CompanyReport> lstCompanyReportInfo = m_da.GetCompanyReportInfo();
            if (lstCompanyReportInfo.Count > 0)
            {
                Table.CompanyReport companyreport = lstCompanyReportInfo[0];
                if (companyreport.SenderRoleId > 0)
                {
                    sDefaultSenderEmail = m_da.GetSendRoleUserEmail(companyreport.SenderRoleId, req1.FileId);
                    sDefaultSenderName = m_da.GetSendRoleUserName(companyreport.SenderRoleId, req1.FileId);
                }
                else if (companyreport.SenderEmail != "")
                {
                    sDefaultSenderEmail = companyreport.SenderEmail;
                    sDefaultSenderName = companyreport.SenderName;
                }
            }

            if (sDefaultSenderEmail == "")
            {
                if (string.IsNullOrEmpty(EmailMgr._emailServerSetting.DefaultAlertEmail))
                {
                    err = string.Format("Warning Message:DefaultAlertEmail will be used as the From Email Address can not be empty. ");
                    err = err + err1;
                    Event_id = 3011;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    response.hdr.StatusInfo = err;
                    return response;
                }
                sDefaultSenderEmail = EmailMgr._emailServerSetting.DefaultAlertEmail;
            }
            MailAddress mailFrom;
            if (sDefaultSenderName == "")
            {
                mailFrom = new MailAddress(sDefaultSenderEmail);
            }
            else
            {
                mailFrom = new MailAddress(sDefaultSenderEmail, sDefaultSenderName);
            }
            #endregion

            req = new GenerateReportRequest();
            req.TemplReportId = req1.TemplReportId;
            req.FileId = req1.FileId;
            //req.External = req1.ToContactId > 0 ? true : false;
            req.LoanAutoEmailId = req1.LoanAutoEmailid;
            req.hdr = new ReqHdr();
            req.hdr.UserId = req1.hdr.UserId;

            //subject
            subject = "Your Loan Status Update";
            //subject = string.Format("Loan Status Report for {0}'s loan", info.Borrower.Trim());

            //email body
            try
            {
                generateReportResponse = GenerateReport(req);
                if (generateReportResponse.hdr != null && generateReportResponse.hdr.Successful)
                {
                    if (generateReportResponse.ReportContent != null && generateReportResponse.ReportContent.Length > 0)
                    {
                        //todo:check image data
                        emailBody = Encoding.UTF8.GetString(generateReportResponse.ReportContent);

                    }
                }
                else if (generateReportResponse.hdr != null)
                {
                    Event_id = 3021;
                    err = string.Format("ReportManager::EmailLSR  Generate report faild for FileId={0},TemplReportId={1},error:{2}", req1.FileId, req1.TemplReportId, generateReportResponse.hdr.StatusInfo);
                    err = err + err1;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    response.hdr.StatusInfo = err;
                    return response;
                }
                else
                {
                    Event_id = 3031;
                    err = string.Format("ReportManager::EmailLSR  Generate report faild for FileId={0},TemplReportId={1}", req1.FileId, req1.TemplReportId);
                    err = err + err1;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    response.hdr.StatusInfo = err;
                    return response;
                }
            }
            catch (Exception exception)
            {
                Event_id = 3041;
                err = string.Format("ReportManager::EmailLSR   Generate report faild for FileId={0},TemplReportId={1}", req1.FileId, req1.TemplReportId);
                err += MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.Message + "\r\n\r\nStackTrace: " + exception.StackTrace;
                err = err + err1;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                response.hdr.StatusInfo = err;
                return response;
            }

            Dictionary<string, Stream> attatchments = null;
            if (req1.Attachments != null)
            {
                attatchments = req1.Attachments.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => (new MemoryStream(keyValuePair.Value) as Stream));
            }

            //tos
            tos = new List<MailAddress>();
            if (req1.ToUserId > 0)
            {
                MailAddress toUser = null;
                if (!string.IsNullOrEmpty(req1.ToUserEmail))
                {
                    if (!string.IsNullOrEmpty(req1.ToUserUserName))
                    {
                        toUser = new MailAddress(req1.ToUserEmail, req1.ToUserUserName);
                    }
                    else
                    {
                        toUser = new MailAddress(req1.ToUserEmail);
                    }
                    tos.Add(toUser);
                }
                else
                {
                    Event_id = 3051;
                    err = string.Format("ReportManager::EmailLSR   ToUserEmail Is Empty, ToUserId={0}", req1.ToUserId);
                    err = err + err1;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    response.hdr.StatusInfo = err;
                    return response;
                }

            }

            if (req1.ToContactId > 0)
            {
                MailAddress toContact = null;
                if (!string.IsNullOrEmpty(req1.ToContactEmail))
                {
                    if (!string.IsNullOrEmpty(req1.ToContactUserName))
                    {
                        toContact = new MailAddress(req1.ToContactEmail, req1.ToContactUserName);
                    }
                    else
                    {
                        toContact = new MailAddress(req1.ToContactEmail);
                    }
                    tos.Add(toContact);
                }
                else
                {
                    Event_id = 3061;
                    err = string.Format("ReportManager::EmailLSR   ToContactEmail Is Empty, ToContactId={0}", req1.ToUserId);
                    err = err + err1;
                    short Category = 30;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    response.hdr.StatusInfo = err;
                    return response;
                }
            }

            string message = string.Empty;
            try
            {
                sentStatus = EmailMgr.SendEmail(subject, emailBody, mailFrom, smtpHost, 25, tos, ccs, bcc, true, attatchments);

            }
            catch (Exception exception)
            {
                Event_id = 3071;
                message = "ReportManager::EmailLSR " + MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.Message + "\r\n\r\nStackTrace: " + exception.StackTrace;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, message, EventLogEntryType.Error, Event_id, Category);
                response.hdr.StatusInfo = message;
                return response;
            }

            string logErr = string.Empty;
            try
            {
                int emailLogId = 0;
                if (m_da.EmailLog(req1.ToUserId.ToString(), req1.ToContactId.ToString(), 0, sentStatus, message, 0, 0, req1.FileId, mailFrom.Address, 0, 0, emailBody, subject,
                              false, "", 0, 0, "", 0, false, "", "", "", "", ref logErr, ref emailLogId) == false)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, logErr, EventLogEntryType.Warning);
                }
                m_da.UpdateLoanAutoEmailStatus(req1.LoanAutoEmailid, ref logErr);
            }
            catch (Exception exception)
            {
                Event_id = 3081;
                logErr = "ReportManager::EmailLSR Exception:" + exception.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, logErr, EventLogEntryType.Error, Event_id, Category);
                response.hdr.StatusInfo = logErr;
                return response;
            }
            response.hdr.Successful = true;
            return response;
        }


    }
}
