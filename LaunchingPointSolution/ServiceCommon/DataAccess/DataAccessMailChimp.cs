using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using Common;
using focusIT;
using LP2.Service.Common;

namespace DataAccess
{
    public partial class DataAccess
    {
        /// <summary>
        /// Gets the mail chimp API key.
        /// </summary>
        /// <param name="branchId">The branch id.</param>
        /// <returns></returns>
        public string GetMailChimpAPIKey(int branchId)
        {
            string err = "";
            var mailChimpApiKey = string.Empty;
            bool logErr = false;
            string sqlCmd = string.Format("SELECT MailChimpAPIKey FROM dbo.Branches WHERE BranchId={0} AND EnableMailChimp=1", branchId);
            try
            {
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                mailChimpApiKey = obj == null ? string.Empty : Convert.ToString(obj);
                return mailChimpApiKey;
            }
            catch (Exception ex)
            {
                err = "GetMailChimpAPIKey, Exception: " + ex.Message;
                Trace.TraceError(err);
                int Event_id = 2011;               
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return mailChimpApiKey;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 2011;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        /// <summary>
        /// Gets the mail chimp list.
        /// </summary>
        /// <param name="branchId">The branch id.</param>
        /// <returns></returns>
        public List<Table.MailChimpList> GetMailChimpList(string branchId)
        {
            var mailChimpLists = new List<Table.MailChimpList>();
            string SQLString = string.Format(@"SELECT LID,Name,BranchId FROM dbo.MailChimpLists WHERE BranchId='{0}'", branchId);
            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.MailChimpList chimpList = null;
                while (dataReader.Read())
                {
                    chimpList = new Table.MailChimpList();
                    chimpList.ListId = dataReader.IsDBNull(0) ? string.Empty : dataReader.GetString(0);
                    chimpList.Name = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                    string branchID = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    int bid = 0;
                    int.TryParse(branchId, out bid);
                    chimpList.BranchId = bid;
                    mailChimpLists.Add(chimpList);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return mailChimpLists;
        }

        /// <summary>
        /// Deletes the mail chimp list andpcampaigns.
        /// </summary>
        /// <param name="listId">The list id.</param>
        /// <param name="branchId">The branch id.</param>
        public void DeleteMailChimpListAndpcampaigns(string listId, int branchId)
        {
            string err = "";
            bool logErr = false;
            string sqlCmd = string.Format("[dbo].[lpsp_DeleteMailChimpListAndCampaigns]");
            SqlParameter[] parameters = {
                                            new SqlParameter("@LID", SqlDbType.NVarChar,255),
                                            new SqlParameter("@BranchId", SqlDbType.Int)
                                        };           
            if (string.IsNullOrEmpty(listId))
                parameters[0].Value = DBNull.Value;
            else
                parameters[0].Value = listId;
            parameters[1].Value = branchId;
            try
            {
                DbHelperSQL.ExecuteSql(sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                //err = "DeleteMailChimpListAndpcampaigns, Exception: " + ex.Message;
                //int Event_id = 2012;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (logErr)
                {
                    //Trace.TraceError(err);
                    //int Event_id = 2012;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        /// <summary>
        /// Gets the mail chimp campaigns.
        /// </summary>
        /// <param name="branchId">The branch id.</param>
        /// <returns></returns>
        public List<Table.MailChimpCampaign> GetMailChimpCampaigns(string branchId)
        {
            var mailChimpCampaigns = new List<Table.MailChimpCampaign>();
            string SQLString = string.Format(@"SELECT  [CID]
                                                      ,[Name]
                                                      ,[BranchId] FROM dbo.MailChimpCampaigns WHERE BranchId={0}", branchId);
            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.MailChimpCampaign chimpCampaign = null;
                while (dataReader.Read())
                {
                    chimpCampaign = new Table.MailChimpCampaign();
                    chimpCampaign.CId = dataReader.IsDBNull(0) ? string.Empty : dataReader.GetString(0);
                    chimpCampaign.Name = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                    chimpCampaign.BranchId = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt32(2);
                    mailChimpCampaigns.Add(chimpCampaign);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return mailChimpCampaigns;
        }

        /// <summary>
        /// Deletes the mail chimp campaigns and update contact mail campaigns.
        /// </summary>
        /// <param name="campaignId">The campaign id.</param>
        /// <param name="branchId">The branch id.</param>
        public void DeleteMailChimpCampaignsAndUpdateContactMailCampaigns(string campaignId, int branchId)
        {
            string err = "";
            bool logErr = false;
            string sqlCmd = string.Format("[dbo].[lpsp_DeleteMailChimpCampaignsAndUpdateContactMailCampaigns]");
            SqlParameter[] parameters = {
                                            new SqlParameter("@CID", SqlDbType.NVarChar,255),
                                            new SqlParameter("@BranchId", SqlDbType.Int)
                                        };
            parameters[0].Value = campaignId;
            parameters[1].Value = branchId;
            try
            {
                DbHelperSQL.ExecuteSql(sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                err = "DeleteMailChimpCampaignsAndUpdateContactMailCampaigns, Exception: " + ex.Message;
                int Event_id = 2014;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 2014;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }
        /// <summary>
        /// Gets the mail chimp contact.
        /// </summary>
        /// <param name="contactIds">The contact ids.</param>
        /// <returns></returns>
        public List<Table.MailChimpContact> GetMailChimpContact(List<int> contactIds)
        {
            var chimpContact = new List<Table.MailChimpContact>();

            if (contactIds == null || contactIds.Count == 0)
                return chimpContact;

            string contactId = string.Join(",", contactIds.Select(item => item.ToString()).ToArray());
            string SQLString = string.Format(@"SELECT ContactId,FirstName,LastName,Email FROM dbo.Contacts  WHERE ContactId IN ({0})", contactId);
            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.MailChimpContact contact = null;
                while (dataReader.Read())
                {
                    contact = new Table.MailChimpContact();
                    contact.ContactId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    contact.FirstName = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                    contact.LastName = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    contact.Email = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
                    chimpContact.Add(contact);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return chimpContact;
        }
        /// <summary>
        /// Gets the mail chimp contact.
        /// </summary>
        /// <param name="contactId">The contact id.</param>
        /// <returns></returns>
        public Table.MailChimpContact GetMailChimpContact(int contactId)
        {
            return GetMailChimpContact(new List<int> { contactId }).FirstOrDefault();
        }

        public string GetApiKey(string mailChimpListId)
        {
            string err = "";
            var mailChimpApiKey = string.Empty;
            bool logErr = false;
            string sqlCmd = string.Format("SELECT b.MailChimpAPIKey FROM dbo.Branches b INNER JOIN dbo.MailChimpLists mcl ON b.BranchId=mcl.BranchId WHERE mcl.LID='{0}'", mailChimpListId);
            try
            {
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                mailChimpApiKey = obj == null ? string.Empty : Convert.ToString(obj);
                return mailChimpApiKey;
            }
            catch (Exception ex)
            {
                err = "GetApiKey, Exception: " + ex.Message;
                int Event_id = 2015;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return mailChimpApiKey;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 2015;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        public void AddMailChimpLists(Table.MailChimpList mailChimpList)
        {
            string err = "";
            bool logErr = false;
            string sqlCmd = string.Format(@"INSERT INTO [dbo].[MailChimpLists]
                                                               ([LID]
                                                               ,[Name]
                                                               ,[BranchId]
                                                               ,[UserId])
                                                         VALUES
                                                               (@LID
                                                               ,@Name
                                                               ,@Branchid
                                                               ,@UserId)");
            SqlParameter[] parameters = {
                                            new SqlParameter("@LID", SqlDbType.NVarChar,255),
                                            new SqlParameter("@Name", SqlDbType.NVarChar,255),
                                            new SqlParameter("@BranchId", SqlDbType.Int),
                                            new SqlParameter("@UserId", SqlDbType.Int)
                                        };
            parameters[0].Value = mailChimpList.ListId;
            parameters[1].Value = mailChimpList.Name;
            parameters[2].Value = mailChimpList.BranchId;
            parameters[3].Value = mailChimpList.UserId;
            try
            {
                DbHelperSQL.ExecuteSql(sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                //err = "AddMailChimpLists, Exception: " + ex.Message;
                //int Event_id = 2016;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (logErr)
                {
                    //Trace.TraceError(err);
                    //int Event_id = 2016;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        public void AddMailChimpCampaigns(Table.MailChimpCampaign mailChimpCampaign)
        {
            string err = "";
            bool logErr = false;
            string sqlCmd = string.Format(@"INSERT INTO [dbo].[MailChimpCampaigns]
                                                               ([CID]
                                                               ,[Name]
                                                               ,[BranchId])
                                                         VALUES
                                                               (@CID
                                                               ,@Name
                                                               ,@Branchid)");
            SqlParameter[] parameters = {
                                            new SqlParameter("@CID", SqlDbType.NVarChar,255),
                                            new SqlParameter("@Name", SqlDbType.NVarChar,255),
                                            new SqlParameter("@BranchId", SqlDbType.Int)
                                        };
            parameters[0].Value = mailChimpCampaign.CId;
            parameters[1].Value = mailChimpCampaign.Name;
            parameters[2].Value = mailChimpCampaign.BranchId;
            try
            {
                DbHelperSQL.ExecuteSql(sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                //err = "AddMailChimpCampaigns, Exception: " + ex.Message;
                //int Event_id = 2017;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (logErr)
                {
                    //Trace.TraceError(err);
                    //int Event_id = 2017;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }


        /// <summary>
        /// Updates the contact mail campaign.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="cid">The cid.</param>
        /// <param name="status">The status.</param>
        public void UpdateContactMailCampaign(string email, string cid, string status)
        {
            string err = "";
            bool logErr = false;
            string sqlCmd = string.Format(@" UPDATE dbo.ContactMailCampaign 
		                                            SET Result='{1}' ,
                                                        CID='{2}'
		                                            WHERE ContactId IN (SELECT ContactId FROM [dbo].[lpfn_GetContactId]('{0}'))", email, status, cid);
            try
            {
                DbHelperSQL.ExecuteSql(sqlCmd);
            }
            catch (Exception ex)
            {
                err = "UpdateContactMailCampaign, Exception: " + ex.Message;
                int Event_id = 2018;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 2018;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        /// <summary>
        /// Gets the last run datetime.
        /// </summary>
        /// <param name="mailchimpmanager">The mailchimpmanager.</param>
        /// <returns></returns>
        public DateTime GetLastRunDatetime(string mailchimpmanager)
        {
            string err = "";
            var runDatetime = new DateTime();
            bool logErr = false;
            string sqlCmd = string.Format(" SELECT LastRun FROM dbo.ServiceStatus WHERE ServiceName='{0}'", mailchimpmanager);
            try
            {
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                runDatetime = obj == null ? new DateTime() : Convert.ToDateTime(obj);
                return runDatetime;
            }
            catch (Exception ex)
            {
                err = "GetLastRunDatetime, Exception: " + ex.Message;
                int Event_id = 2019;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return runDatetime;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 2019;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        /// <summary>
        /// Gets the branch list.
        /// </summary>
        /// <returns></returns>
        public List<Table.Branch> GetBranchList()
        {
            var branchList = new List<Table.Branch>();
            string SQLString = string.Format(@"SELECT BranchId,MailChimpAPIKey,EnableMailChimp FROM dbo.Branches");
            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.Branch branch = null;
                while (dataReader.Read())
                {
                    branch = new Table.Branch();
                    branch.BranchId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    branch.MailChimpAPIKey = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                    branch.EnableMailChimp = dataReader.IsDBNull(2) ? false : dataReader.GetBoolean(2);
                    branchList.Add(branch);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return branchList;
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <param name="defaultFromEmail">The default from email.</param>
        /// <returns></returns>
        public int GetUserId(string defaultFromEmail)
        {
            int userId = 0;
            string sqlCmd = "SELECT TOP 1 UserId FROM dbo.Users WHERE EmailAddress=@EmailAddress";
            if (string.IsNullOrEmpty(defaultFromEmail))
            {
                return userId;
            }
            DataSet ds = null;
            SqlCommand cmd = new SqlCommand(sqlCmd);
            cmd.Parameters.Add(new SqlParameter("@EmailAddress", SqlDbType.NVarChar));
            cmd.Parameters[0].Value = defaultFromEmail.Trim();
            object obj = DbHelperSQL.ExecuteScalar(cmd);
            userId = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
            return userId;
        }

        /// <summary>
        /// Updates the mail chimp lists.
        /// </summary>
        /// <param name="mailChimpList">The mail chimp list.</param>
        public void UpdateMailChimpLists(Table.MailChimpList mailChimpList)
        {
            string err = "";
            bool logErr = false;
            string sqlCmd = string.Format(@"UPDATE [dbo].[MailChimpLists]
                                                           SET [Name] = @Name
                                                              ,[UserId] = @UserId
                                                         WHERE [LID] = @LID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@LID", SqlDbType.NVarChar,255),
                                            new SqlParameter("@Name", SqlDbType.NVarChar,255),
                                            new SqlParameter("@UserId", SqlDbType.Int)
                                        };
            parameters[0].Value = mailChimpList.ListId;
            parameters[1].Value = mailChimpList.Name;
            parameters[2].Value = mailChimpList.UserId;
            try
            {
                DbHelperSQL.ExecuteSql(sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                err = "AddMailChimpLists, Exception: " + ex.Message;
                int Event_id = 2020;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 2020;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }
    }
}
