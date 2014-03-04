using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using Common;
using focusIT;
using EmailManager;
using LP2.Service.Common;
using System.Threading;

namespace RuleManager
{
    public class RuleMgr : IRuleMgr
    {
        short Category = 30;
        protected static DataAccess.DataAccess m_da = null;
        /// <summary>
        /// Instances this instance.
        /// </summary>
        /// <returns></returns>
        public static RuleMgr Instance
        {
            get
            {
                return new RuleMgr();
            }
        }

        private RuleMgr()
        {
            m_da = new DataAccess.DataAccess();
        }

        public void ProcessLoanRulesOld()
        {
            string err = "";
            bool logErr = false;
            DataSet ds = null;
            try
            {
                EmailManager.EmailMgr em = EmailManager.EmailMgr.Instance;
                ds = m_da.ProcessLoanRules();
                if ((ds == null) || (ds.Tables.Count <= 0) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = MethodBase.GetCurrentMethod() + " no rules to process.";
                    int Event_id = 4003;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id, Category);
                    return;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int EmailId = Convert.ToInt32(ds.Tables[0].Rows[i]["EmailId"]);
                    int FileId = Convert.ToInt32(ds.Tables[0].Rows[i]["FileId"]);
                    int EmailTemplId = Convert.ToInt32(ds.Tables[0].Rows[i]["EmailTmplId"]);
                    int LoanAlertId = Convert.ToInt32(ds.Tables[0].Rows[i]["LoanAlertId"]);
                    int AlertEmailType = Convert.ToInt32(ds.Tables[0].Rows[i]["AlertEmailType"]);
                    var ereq = new EmailPreviewRequest
                                    {
                                        FileId = FileId,
                                        EmailTemplId = EmailTemplId,
                                        LoanAlertId = LoanAlertId
                                    };
                    try
                    {
                        EmailPreviewResponse epr = em.PreviewEmail(ereq);
                        if (epr.resp.Successful)
                        {
                            string emailBody = string.Empty;
                            if (epr.EmailHtmlContent != null && epr.EmailHtmlContent.Length > 0)
                            {
                                emailBody = Encoding.UTF8.GetString(epr.EmailHtmlContent);
                            }
                            m_da.UpdateEmailBody(EmailId, LoanAlertId, AlertEmailType, emailBody, epr.EmailHtmlContent);
                        }
                    }
                    catch (Exception exception)
                    {
                        err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.Message + "\r\n\r\nStackTrace: " + exception.StackTrace;
                        int Event_id = 4005;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                }
            }
            catch (Exception e)
            {
                err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + e.Message + "\r\n\r\nStackTrace: " + e.StackTrace;
                int Event_id = 4007;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 4008;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public void ProcessLoanRules()
        {
            string err = "";
            bool logErr = false;
            DataSet ds = null;
            List<Table.PendingRuleInfo> pendingRuelInfos = null;
            List<Table.TemplateRules> templateRuleses = null;
            EmailManager.EmailMgr em = EmailManager.EmailMgr.Instance;
            try
            {
                int loop_count = 0;
                pendingRuelInfos = m_da.GetPendgingRuleInfo();
                if (pendingRuelInfos == null || pendingRuelInfos.Count <= 0)
                {
                    err = MethodBase.GetCurrentMethod() + " no rules to process.";
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information);
                    return;
                }

/*
 * Moved the logic to the database store procedure
 * 
 * for (int i = 0; i < pendingRuelInfos.Count; i++)
                {
                    try
                    {
                        pendingRuelInfos[i].RuleCondValue = m_da.GetConditionValue(pendingRuelInfos[i].FileId,
                                                                                   pendingRuelInfos[i].RuleCondId);
                        loop_count = loop_count + 1;
                        if (loop_count > 500)
                        {
                            loop_count = 0;
                            Thread.Sleep(50);
                        }
                    }
                    catch (Exception exception)
                    {
                        err = MethodBase.GetCurrentMethod() + string.Format(" GetConditionValue error for FileId<{0}>,RuleCondId<{1}>", pendingRuelInfos[i].FileId, pendingRuelInfos[i].RuleCondId);
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                    }
                }
 */

                //var invalidRuleInfo = from ri in pendingRuelInfos
                //                      where ri.RuleCondValue == -1
                //                      select ri;
                var invalidRuleInfo = pendingRuelInfos
                                        .Select((item, index) => new { Info = item, Index = index })
                                        .Where(x => x.Info.RuleCondValue == -1);

                foreach (var ruleInfo in invalidRuleInfo)
                {
                    var pointFieldId = "";
                    var fieldValue = "";
                    var prevfieldValue = "";
                    DataSet ds1 = new DataSet();
                    try
                    {
                        ds1 = m_da.GetPointFieldIdAndValue(ruleInfo.Info.FileId, ruleInfo.Info.RuleCondId);
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            if (!ds1.Tables[0].Rows[0].IsNull("PointFieldId"))
                                pointFieldId = ds1.Tables[0].Rows[0]["PointFieldId"].ToString();

                            if (!ds1.Tables[0].Rows[0].IsNull("CurrentValue"))
                                fieldValue = ds1.Tables[0].Rows[0]["CurrentValue"].ToString();

                            if (!ds1.Tables[0].Rows[0].IsNull("PrevValue"))
                                prevfieldValue = ds1.Tables[0].Rows[0]["PrevValue"].ToString();
                        }
                    }
                    catch (Exception exception)
                    {
                        err = MethodBase.GetCurrentMethod() + " GetPointFieldIdAndValue.";
                        int Event_id = 4009;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                        
                    }
                    //err = string.Format("FileId={0},PointFieldId={1},Value=[Prev-{5}:Curr-{2}],RuleId={3},RuleConditionId={4}", ruleInfo.Info.FileId, pointFieldId, fieldValue, ruleInfo.Info.RuleId, ruleInfo.Info.RuleCondId, prevfieldValue);
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                    pendingRuelInfos[ruleInfo.Index].RuleCondValue = 0;
                }

                templateRuleses = m_da.GetPendgingTemplateRuleInfo();
                if (templateRuleses == null || templateRuleses.Count <= 0)
                {
                    //err = MethodBase.GetCurrentMethod() + " can't get template rule information.";
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                    return;
                }

                loop_count = 0;

                var query =
                    from rule in pendingRuelInfos
                    group rule by new { rule.RuleId, rule.FileId }
                        into g
                        select new { GroupKey = g.Key, Conditions = g };

                foreach (var groupItem in query)
                {
                    try
                    {
                        //get template info
                        var templateRuelQuery = from tr in templateRuleses where tr.RuleId == groupItem.GroupKey.RuleId select tr;
                        Table.TemplateRules templateRule = templateRuelQuery.SingleOrDefault();
                        string AdvFormula = string.Empty;

                        if (templateRule == null)
                        {
                            err = MethodBase.GetCurrentMethod() + " can't get template rule information for ruleID " + groupItem.GroupKey.RuleId;
                            int Event_id = 4011;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                        
                            continue;
                        }

                        bool ruleValue = GetAdvFormulaValue(templateRule.AdvFormula, groupItem.Conditions.ToList());
                        if (ruleValue)
                        {
                            int alertEmailType = 1;
                            int emailTemplId = 0;

                            int fileId = groupItem.GroupKey.FileId;
                            int loanAlertID = 0;

                            loanAlertID = CreateRuleAlert(fileId, groupItem.GroupKey.RuleId, alertEmailType, templateRule.Desc, templateRule.AckReq, "Rule Alert", "");
                            //create rule alert
                            if (templateRule.AlertEmailTemplId != 0 && templateRule.AckReq == false)
                            {
                                alertEmailType = 1;
                                emailTemplId = templateRule.AlertEmailTemplId;

                                if (loanAlertID != 0)
                                {
                                    string emailBody = GetEmailBody(fileId, emailTemplId, loanAlertID, em);
                                    UpdateRuleAlertEmailBody(loanAlertID, alertEmailType, emailBody);
                                    CreateEmailQue(emailTemplId, loanAlertID, fileId, alertEmailType, emailBody);
                                }

                            }

                            //create rule recom email
                            if (templateRule.RecomEmailTemplid != 0)
                            {
                                alertEmailType = 2;
                                emailTemplId = templateRule.RecomEmailTemplid;

                                if (loanAlertID != 0)
                                {
                                    string emailBody = GetEmailBody(fileId, emailTemplId, loanAlertID, em);
                                    UpdateRuleAlertEmailBody(loanAlertID, alertEmailType, emailBody);
                                    CreateEmailQue(emailTemplId, loanAlertID, fileId, alertEmailType, emailBody);
                                }
                            }
                        }

                        loop_count = loop_count + 1;
                        if (loop_count > 500)
                        {
                            loop_count = 0;
                            Thread.Sleep(50);
                        }
                       
                        UpdateLoanRuleLastCheck(groupItem.GroupKey.RuleId);
                        
                    }
                    catch (Exception exception)
                    {
                        string errInfo = MethodBase.GetCurrentMethod() +
                                         string.Format("  Exception:  fileID={0},  ruleID={1},  Message:   {2}",
                                                groupItem.GroupKey.FileId,  groupItem.GroupKey.RuleId,  exception.Message);
                        int Event_id = 4015;                     
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, errInfo, EventLogEntryType.Warning, Event_id, Category);
                    }
                }

                UpdateRuleManagerServiceStatus();
            }
            catch (Exception e)
            {
                err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + e.Message + "\r\n\r\nStackTrace: " + e.StackTrace;
                int Event_id = 4017;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 4019;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                    
                }
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        /// <summary>
        /// Updates the rule alert email body.
        /// </summary>
        /// <param name="loanAlertID">The loan alert ID.</param>
        /// <param name="alertEmailType">Type of the alert email.</param>
        /// <param name="emailBody">The email body.</param>
        private void UpdateRuleAlertEmailBody(int loanAlertID, int alertEmailType, string emailBody)
        {
            try
            {
                m_da.UpdateRuleAlertEmailBody(loanAlertID, alertEmailType, emailBody);
            }
            catch (Exception exception)
            {
                string err = MethodBase.GetCurrentMethod() + string.Format("Update rule alert email body for loanAlertId={0} alertEmailType={1}. \nException: {2}", loanAlertID, alertEmailType, exception);
                int Event_id = 4021;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                   
             }
        }


        /// <summary>
        /// Creates the email que.
        /// </summary>
        /// <param name="emailTemplId">The email templ id.</param>
        /// <param name="loanAlertID">The loan alert ID.</param>
        /// <param name="fileId">The file id.</param>
        /// <param name="alertEmailType">Type of the alert email.</param>
        /// <param name="emailBody">The email body.</param>
        private void CreateEmailQue(int emailTemplId, int loanAlertID, int fileId, int alertEmailType, string emailBody)
        {
            try
            {
                m_da.CreateEmailQue(emailTemplId, loanAlertID, fileId, alertEmailType, emailBody);
            }
            catch (Exception exception)
            {
                string err = MethodBase.GetCurrentMethod() + string.Format("Create rule email queue for fildID={0} loanAlertId={1} emailTemplId={2}. Exception: {3}", fileId, loanAlertID, emailTemplId, exception);
                int Event_id = 4023;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category); 
            }
        }


        /// <summary>
        /// Gets the email body.
        /// </summary>
        /// <param name="FileId">The file id.</param>
        /// <param name="EmailTemplId">The email templ id.</param>
        /// <param name="LoanAlertId">The loan alert id.</param>
        /// <param name="em">The em.</param>
        /// <returns></returns>
        private string GetEmailBody(int FileId, int EmailTemplId, int LoanAlertId, EmailManager.EmailMgr em)
        {
            var ereq = new EmailPreviewRequest
                           {
                               FileId = FileId,
                               EmailTemplId = EmailTemplId,
                               LoanAlertId = LoanAlertId
                           };
            try
            {
                EmailPreviewResponse epr = em.PreviewEmail(ereq);
                if (epr.resp.Successful)
                {
                    string emailBody = string.Empty;
                    if (epr.EmailHtmlContent != null && epr.EmailHtmlContent.Length > 0)
                    {
                        emailBody = Encoding.UTF8.GetString(epr.EmailHtmlContent);
                    }
                    return emailBody;
                }
            }
            catch (Exception exception)
            {
                string err = MethodBase.GetCurrentMethod() + string.Format("Generate email body faild for FileId={0} EmailTemplId={1}. Exception: {2}", FileId, EmailTemplId, exception);
                int Event_id = 4025;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                
            }
            return string.Empty;
        }

        /// <summary>
        /// Creates the rule alert.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="ruleId">The rule id.</param>
        /// <param name="alertEmailType">Type of the alert email.</param>
        /// <param name="desc">The desc.</param>
        /// <param name="ackReq">if set to <c>true</c> [ack req].</param>
        /// <param name="ruleAlert">The rule alert.</param>
        /// <param name="emailBody">The email body.</param>
        /// <returns></returns>
        private int CreateRuleAlert(int fileId, int ruleId, int alertEmailType, string desc, bool ackReq, string ruleAlert, string emailBody)
        {
            string sqlCmd = string.Format("select top 1 LoanAlertId, DateCreated  from dbo.LoanAlerts where [FileId]={0} AND [LoanRuleId]={1}", fileId, ruleId);
            int LoanAlertId = 0;
            DateTime DateCreated = DateTime.MinValue;
            DateTime DTN = DateTime.Now;
            DataSet ds = null;
            DataSet ds1 = null;
            DataSet ds2 = null;
            DataSet ds3 = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr[0] != DBNull.Value)
                        LoanAlertId = Convert.ToInt32(dr[0]);
                    if (dr[1] != DBNull.Value)
                    {
                        DateCreated = (DateTime)(dr[1]);
                        if (DateCreated.Date != DTN.Date)
                        {
                            int PointFieldId = 0;
                            string sqlCmd2 = string.Format("select top 1 PointFieldId from dbo.Template_RuleConditions where [RuleId]={0}", ruleId);
                            ds1 = DbHelperSQL.Query(sqlCmd2);
                            if (ds1.Tables[0].Rows.Count > 0)
                            {                               
                                DataRow dr1 = ds1.Tables[0].Rows[0];
                                if (dr1[0] != DBNull.Value)
                                    PointFieldId = Convert.ToInt32(dr1[0]);
                            }
                        
                            DateTime ChangeTime = DateTime.MinValue;
                            string sqlCmd3 = string.Format("select top 1 ChangeTime from dbo.LoanPointFields where [FileId]={0} and PointFieldId={1}", fileId, PointFieldId);
                            ds2 = DbHelperSQL.Query(sqlCmd3);
                            if (ds2.Tables[0].Rows.Count > 0)
                                {
                                    DataRow dr2 = ds2.Tables[0].Rows[0];
                                    if (dr2[0] != DBNull.Value)
                                        ChangeTime = (DateTime)(dr2[0]);
                                }

                            DateTime DateTimeReceived = DateTime.MinValue;
                            string sqlCmd4 = string.Format("select top 1 DateTimeReceived from dbo.EmailLog where [FileId]={0} and LoanAlertId={1}", fileId, LoanAlertId);
                            ds3 = DbHelperSQL.Query(sqlCmd4);
                            if (ds3.Tables[0].Rows.Count > 0)
                                {
                                    DataRow dr2 = ds3.Tables[0].Rows[0];
                                    if (dr2[0] != DBNull.Value)
                                        DateTimeReceived = (DateTime)(dr2[0]);
                                }

                            if (ChangeTime > DateCreated)
                                {
                                    if (DateTimeReceived.Date != DTN.Date)
                                    {
                                        string sqlCmd5 = string.Format("Update dbo.LoanAlerts SET DateCreated='{0}' where [FileId]={1} AND [LoanRuleId]={2}", DTN.ToString(), fileId, ruleId);
                                        DbHelperSQL.ExecuteSql(sqlCmd5);
                                        return LoanAlertId;
                                    }
                                }                          
                        }
                        else
                        {
                            return 0;
                        }
                    } 
                }               

                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
            catch (Exception ex)
            { }

            try
            {
                return m_da.CreateRuleAlert(fileId, ruleId, alertEmailType, desc, ackReq, ruleAlert, emailBody);
            }
            catch (Exception exception)
            {
                string err = MethodBase.GetCurrentMethod() + string.Format("Create rule alert faild for fildID={0} ruleId={1}. Exception: {2}", fileId, ruleId, exception);
                int Event_id = 4027;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            return 0;
        }

        /// <summary>
        /// Updates the rule manager service status.
        /// </summary>
        private void UpdateRuleManagerServiceStatus()
        {
            try
            {
                m_da.UpdateRuleManagerServiceStatus();
            }
            catch (Exception exception)
            {
                string err = MethodBase.GetCurrentMethod() + string.Format(" update rule manager status faild. Exception: {0}", exception);
                int Event_id = 4029;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
        }

        /// <summary>
        /// Updates the loan rule last check.
        /// </summary>
        /// <param name="ruleId">The rule id.</param>
        private void UpdateLoanRuleLastCheck(int ruleId)
        {
            try
            {
                m_da.UpdateLoanRuleLastCheck(ruleId);
            }
            catch (Exception exception)
            {
                string err = MethodBase.GetCurrentMethod() + string.Format(" update rule last check faild for ruleid {0}. Exception: {1}", ruleId, exception);
                int Event_id = 4031;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
        }

        /// <summary>
        /// Gets the adv formula value.
        /// repalce 1 and 2 and 3 with condition value
        /// ==> 1=(condition 1's value) and 1=(condition 2's value) ...
        /// pass to sql server eval the combined condition 's value.
        /// </summary>
        /// <param name="advFormula">The adv formula.</param>
        /// <param name="conditions">The conditions.</param>
        /// <returns></returns>
        private bool GetAdvFormulaValue(string advFormula, List<Table.PendingRuleInfo> conditions)
        {
            if (conditions == null || conditions.Count <= 0)
                throw new ArgumentNullException("conditions");

            string formula = advFormula;
            Debug.WriteLine(string.Format("original formula:{0}", formula));

            if (string.IsNullOrEmpty(formula))
            {
                int[] aryConditions = new int[conditions.Count];
                for (int i = 0; i < conditions.Count; i++)
                {
                    aryConditions[i] = i + 1;
                }
                //build condition for AdvFormula is empty==>  e.g. 1 and 2 and 3
                formula = string.Join(" AND ", aryConditions.Select(item => item.ToString()).ToArray());
            }

            formula = Regex.Replace(formula, @"\b(\d)\b", "@$1@", RegexOptions.IgnoreCase);

            for (int i = 1; i <= conditions.Count; i++)
            {
                formula = Regex.Replace(formula, string.Format("@{0}@", i), string.Format("1={0}", conditions[i - 1].RuleCondValue), RegexOptions.IgnoreCase);
            }

            bool formulaValue = m_da.GetAdvFormulaValue(formula);
            Debug.WriteLine(string.Format("formula:{0}  value:{1}", formula, formulaValue));
            return formulaValue;
        }

        /// <summary>
        /// Acknowledges the alert.
        /// </summary>
        /// <param name="currentLoanAlertId">The current loan alert id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public bool AcknowledgeAlert(int currentLoanAlertId, int userId)
        {
            string err = "";
            bool logErr = false;
            DataSet ds = null;
            try
            {
                EmailManager.EmailMgr em = EmailManager.EmailMgr.Instance;
                bool status = false;
                ds = m_da.AcknowledgeAlert(currentLoanAlertId, userId, out status);
                if (status == false)
                    return false;
                //if ((ds == null) || (ds.Tables.Count <= 0) || (ds.Tables[0].Rows.Count <= 0))
                //{
                //    err = MethodBase.GetCurrentMethod() + " failed to Acknowledge Rule Alert, AlertId=" + currentLoanAlertId;
                //    logErr = true;
                //    return false;
                //}
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        int FileId = Convert.ToInt32(ds.Tables[0].Rows[i]["FileId"]);
                        int EmailTemplId = Convert.ToInt32(ds.Tables[0].Rows[i]["AlertEmailTemplId"]);
                        int LoanAlertId = Convert.ToInt32(ds.Tables[0].Rows[i]["LoanAlertId"]);
                        var ereq = new EmailPreviewRequest
                                       {
                                           FileId = FileId,
                                           EmailTemplId = EmailTemplId,
                                           LoanAlertId = LoanAlertId
                                       };
                        try
                        {
                            EmailPreviewResponse epr = em.PreviewEmail(ereq);
                            if (!epr.resp.Successful)
                            {
                                err = MethodBase.GetCurrentMethod() +
                                      ", failed to get email body from Email Manager. \r\nreason:" + epr.resp.StatusInfo;
                                int Event_id = 4032;
                                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                                return false;
                            }
                            string emailBody = string.Empty;
                            if (epr.EmailHtmlContent != null && epr.EmailHtmlContent.Length > 0)
                            {
                                emailBody = Encoding.UTF8.GetString(epr.EmailHtmlContent);
                            }
                            m_da.AcknowledgeAlertEmailQue(currentLoanAlertId, userId, emailBody, epr.EmailHtmlContent);
                            return true;
                        }
                        catch (Exception exception)
                        {
                            err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.Message +
                                  "\r\n\r\nStackTrace: " + exception.StackTrace;                            
                            int Event_id = 4033;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + e.Message + "\r\n\r\nStackTrace: " + e.StackTrace;              
                int Event_id = 4035;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 4037;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }
    }
}
