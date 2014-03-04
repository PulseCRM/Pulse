using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using LP2.Service.Common;
using PerceptiveMCAPI.Export;
using PerceptiveMCAPI.Methods;
using PerceptiveMCAPI.Export.Types;
using PerceptiveMCAPI.Types;

namespace MailChimpMgr
{
    public class MailChimpManager : IMailChimpManagerService
    {
        short Category = 50;
        private static readonly MailChimpAPI _mailChimpApi = new MailChimpAPI();
        private static readonly DataAccess.DataAccess _dataAccess = new DataAccess.DataAccess();
        /// <summary>
        /// Gets the mail chimp API key.
        /// </summary>
        /// <param name="branchId">The branch id.</param>
        /// <returns></returns>
        private string GetMailChimpAPIKey(int branchId)
        {
            return _dataAccess.GetMailChimpAPIKey(branchId);
        }

        #region Implementation of IMailChimpManagerService
        /// <summary>
        /// Mails the chimp_ sync now.
        /// </summary>
        /// <param name="BranchId">The branch id.</param>
        /// <returns></returns>
        public MailChimp_Response MailChimp_SyncNow(int BranchId)
        {
            MailChimp_Response response = new MailChimp_Response();
            response.hdr = new RespHdr();

            string mailChimpAPIKey = GetMailChimpAPIKey(BranchId);
            if (string.IsNullOrEmpty(mailChimpAPIKey))
            {
                string err = string.Format("MailChimpAPIKey is empty for branchId:{0}", BranchId);
                int Event_id = 2010;               
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: MailChimpAPIKey is empty for branchId:{0}", BranchId);
                return response;
            }

            try
            {
                MailChimpListSync(BranchId, mailChimpAPIKey);
                MailChimpCampaignSync(BranchId, mailChimpAPIKey);
            }
            catch (Exception exception)
            {
                string err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.Message + "\r\n\r\nStackTrace: " + exception.StackTrace;
                Trace.TraceError(exception.Message);
                int Event_id = 2011;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category); 
                response.hdr.Successful = false;
                response.hdr.StatusInfo = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.Message;
                return response;
            }

            response.hdr.Successful = true;
            response.hdr.StatusInfo = "";
            return response;
        }
        /// <summary>
        /// Mails the chimp_ subscribe.
        /// Will go through each ContactId in the ContactIdslist, retrieve the Contacts.FirstName, Contacts.LastName, Contacts.Email.
        /// Invoke the MailChimp API listBatchSubscribe
        /// </summary>
        /// <param name="ContactIds">The contact ids.</param>
        /// <param name="MailChimpListId">The mail chimp list id.</param>
        /// <returns></returns>
        public MailChimp_Response MailChimp_Subscribe(List<int> ContactIds, string MailChimpListId)
        {
            MailChimp_Response response = new MailChimp_Response();
            response.hdr = new RespHdr();

            if (ContactIds == null || ContactIds.Count == 0)
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: ContactIds is null.");
                return response;
            }

            if (string.IsNullOrEmpty(MailChimpListId))
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: MailChimpListId is null.");
                return response;
            }

            string apiKey = string.Empty;
            apiKey = GetApiKey(MailChimpListId);
            if (string.IsNullOrEmpty(apiKey))
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: MailChimpAPIKey is null.");
                return response;
            }

            var contacts = _dataAccess.GetMailChimpContact(ContactIds);
            return _mailChimpApi.ListBatchSubscribe(apikey: apiKey, mailChimpListId: MailChimpListId, subscriberList: contacts);

        }

        /// <summary>
        /// Mails the chimp_ subscribe.
        ///  Will retrieve the Contacts.FirstName, Contacts.LastName, Contacts.Email using the ContactId.
        /// Invoke the MailChimp API listSubscribe
        /// </summary>
        /// <param name="ContactId">The contact id.</param>
        /// <param name="MailChimpListId">The mail chimp list id.</param>
        /// <returns></returns>
        public MailChimp_Response MailChimp_Subscribe(int ContactId, string MailChimpListId)
        {
            MailChimp_Response response = new MailChimp_Response();
            response.hdr = new RespHdr();

            if (ContactId <= 0)
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: ContactId is less than 0.");
                return response;
            }

            if (string.IsNullOrEmpty(MailChimpListId))
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: MailChimpListId is null.");
                return response;
            }

            string apiKey = string.Empty;
            apiKey = GetApiKey(MailChimpListId);
            if (string.IsNullOrEmpty(apiKey))
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: MailChimpAPIKey is null.");
                return response;
            }

            var contact = _dataAccess.GetMailChimpContact(ContactId);
            return _mailChimpApi.ListSubscribe(apikey: apiKey, mailChimpListId: MailChimpListId, subscriber: contact);
        }

        /// <summary>
        /// Mails the chimp_ unsubscribe.
        ///  Will go through each ContactId in the ContactIdslist, retrieve the Contacts.FirstName, Contacts.LastName, Contacts.Email.
        /// Invoke the MailChimp API listBatchUnsubscribe
        /// </summary>
        /// <param name="ContactIds">The contact ids.</param>
        /// <param name="MailChimpListId">The mail chimp list id.</param>
        /// <returns></returns>
        public MailChimp_Response MailChimp_Unsubscribe(List<int> ContactIds, string MailChimpListId)
        {
            MailChimp_Response response = new MailChimp_Response();
            response.hdr = new RespHdr();

            if (ContactIds == null || ContactIds.Count == 0)
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: ContactIds is null.");
                return response;
            }

            if (string.IsNullOrEmpty(MailChimpListId))
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: MailChimpListId is null.");
                return response;
            }

            string apiKey = string.Empty;
            apiKey = GetApiKey(MailChimpListId);
            if (string.IsNullOrEmpty(apiKey))
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: MailChimpAPIKey is null.");
                return response;
            }

            var contacts = _dataAccess.GetMailChimpContact(ContactIds);
            return _mailChimpApi.ListBatchUnsubscribe(apikey: apiKey, mailChimpListId: MailChimpListId, subscriberList: contacts);
        }

        /// <summary>
        /// Mails the chimp_ unsubscribe.
        /// Will retrieve the Contacts.FirstName, Contacts.LastName, Contacts.Email using the ContactId.
        /// Invoke the MailChimp API listUnsubscribe
        /// </summary>
        /// <param name="ContactId">The contact id.</param>
        /// <param name="MailChimpListId">The mail chimp list id.</param>
        /// <returns></returns>
        public MailChimp_Response MailChimp_Unsubscribe(int ContactId, string MailChimpListId)
        {
            MailChimp_Response response = new MailChimp_Response();
            response.hdr = new RespHdr();

            if (ContactId <= 0)
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: ContactId is less than 0.");
                return response;
            }

            if (string.IsNullOrEmpty(MailChimpListId))
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: MailChimpListId is null.");
                return response;
            }

            string apiKey = string.Empty;
            apiKey = GetApiKey(MailChimpListId);
            if (string.IsNullOrEmpty(apiKey))
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: MailChimpAPIKey is null.");
                return response;
            }

            var contact = _dataAccess.GetMailChimpContact(ContactId);
            return _mailChimpApi.ListUnsubscribe(apikey: apiKey, mailChimpListId: MailChimpListId, subscriber: contact);
        }

        #endregion

        /// <summary>
        /// Schedules the mail chimp.
        /// </summary>
        public void ScheduleMailChimp()
        {
            var branches = _dataAccess.GetBranchList();
            var query = from branch in branches
                        where branch.EnableMailChimp == true && !string.IsNullOrEmpty(branch.MailChimpAPIKey)
                        select branch;
            foreach (var branch in query)
            {
                try
                {
                    MailChimpListSync(branch.BranchId, branch.MailChimpAPIKey, true);
                    MailChimpCampaignSync(branch.BranchId, branch.MailChimpAPIKey, true);
                }
                catch (Exception exception)
                {
                    string err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.Message + "\r\n\r\nStackTrace: " + exception.StackTrace;
                    Trace.TraceError(exception.Message);
                    int Event_id = 2012;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category); 
                }
            }
            string errMsg = string.Empty;
            _dataAccess.Save_ServiceStatus("MailChimpManager", true, ref errMsg);
        }

        private string GetApiKey(string mailChimpListId)
        {
            return _dataAccess.GetApiKey(mailChimpListId);
        }

        /// <summary>
        /// Mails the chimp campaign sync.
        /// </summary>
        /// <param name="BranchId">The branch id.</param>
        /// <param name="mailChimpAPIKey">The mail chimp API key.</param>
        private void MailChimpCampaignSync(int BranchId, string mailChimpAPIKey)
        {
            MailChimpCampaignSync(BranchId, mailChimpAPIKey, true);
        }

        /// <summary>
        /// Mails the chimp campaign sync.
        /// </summary>
        /// <param name="BranchId">The branch id.</param>
        /// <param name="mailChimpAPIKey">The mail chimp API key.</param>
        /// <param name="dualSync"></param>
        private void MailChimpCampaignSync(int BranchId, string mailChimpAPIKey, bool dualSync)
        {
            var mailChimpCampaigns = _mailChimpApi.GetMailChimpCampaigns(mailChimpAPIKey);
            var mailChimpCampaignsTable = _dataAccess.GetMailChimpCampaigns(BranchId.ToString());

            var mcclistIds = (from item in mailChimpCampaigns select item.id).ToList();
            var mcclistTableIds = (from item in mailChimpCampaignsTable select item.CId).ToList();

            /*mc-server   local-db
             * 0            1
             */
            var mclistIsNotInMcSvrIds = mcclistTableIds.Except(mcclistIds);
            foreach (string campaignId in mclistIsNotInMcSvrIds)
            {
                DeleteMailChimpCampaignsAndUpdateContactMailCampaigns(campaignId, BranchId);
            }

            /*mc-server   local-db
             * 0            1
             */

            if (dualSync)
            {
                var newCampaignIdsFromMc = mcclistIds.Except(mcclistTableIds);
                var newCampaignsFromMc = from campaign in mailChimpCampaigns where newCampaignIdsFromMc.Contains(campaign.id) select campaign;
                var mailChimpCampaign = new Common.Table.MailChimpCampaign();
                foreach (var campaign in newCampaignsFromMc)
                {
                    mailChimpCampaign = new Common.Table.MailChimpCampaign();
                    mailChimpCampaign.CId = campaign.id;
                    mailChimpCampaign.Name = campaign.title;
                    mailChimpCampaign.BranchId = BranchId;
                    _dataAccess.AddMailChimpCampaigns(mailChimpCampaign);
                }
                var campaignMembers = new Dictionary<string, List<campaignMembersResults.DataItem>>();
                foreach (var chimpCampaign in mailChimpCampaigns)
                {
                    List<campaignMembersResults.DataItem> members = null;
                    try
                    {
                        members = _mailChimpApi.GetMailChimpCampaignMembers(mailChimpAPIKey, chimpCampaign.id);
                    }
                    catch (Exception exception)
                    {
                        string err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.Message + "\r\n\r\nStackTrace: " + exception.StackTrace;
                        Trace.TraceError(exception.Message);
                        int Event_id = 2014;
                        //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category); 
                    }
                    if (members != null && members.Count > 0)
                    {
                        campaignMembers.Add(chimpCampaign.id, members);
                    }
                }

                //update ContactMailCampaign
                foreach (var campaignMemberInfo in campaignMembers)
                {
                    foreach (var campaignMember in campaignMemberInfo.Value)
                    {
                        try
                        {
                            _dataAccess.UpdateContactMailCampaign(campaignMember.email, campaignMemberInfo.Key, "Successful");
                        }
                        catch (Exception exception)
                        {
                            string err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.Message + "\r\n\r\nStackTrace: " + exception.StackTrace;
                            Trace.TraceError(exception.Message);
                            int Event_id = 2015;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category); 
                        }
                    }

                }

                //update ContactMailCampaign
                var lastRun = _dataAccess.GetLastRunDatetime("MailChimpManager");
                var bounceMessages = new Dictionary<string, List<campaignBounceMessagesResults.DataItem>>();
                foreach (var chimpCampaign in mailChimpCampaigns)
                {
                    var messages = new List<campaignBounceMessagesResults.DataItem>();
                    messages = _mailChimpApi.GetCampaignBounceMessages(mailChimpAPIKey, chimpCampaign.id, lastRun);
                    if (messages != null && messages.Count > 0)
                    {
                        bounceMessages.Add(chimpCampaign.id, messages);
                    }
                }
                foreach (var bounceMessageInfo in bounceMessages)
                {
                    foreach (var bounceMessage in bounceMessageInfo.Value)
                    {
                        if (string.IsNullOrEmpty(bounceMessage.email))
                            continue;

                        try
                        {
                            _dataAccess.UpdateContactMailCampaign(bounceMessage.email, bounceMessageInfo.Key, "Bounced");
                        }
                        catch (Exception exception)
                        {
                            string err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.Message + "\r\n\r\nStackTrace: " + exception.StackTrace;
                            Trace.TraceError(exception.Message);
                            int Event_id = 2016;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category); 
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Deletes the mail chimp campaigns and update contact mail campaigns.
        /// </summary>
        /// <param name="campaignId">The campaign id.</param>
        /// <param name="branchId">The branch id.</param>
        private void DeleteMailChimpCampaignsAndUpdateContactMailCampaigns(string campaignId, int branchId)
        {
            _dataAccess.DeleteMailChimpCampaignsAndUpdateContactMailCampaigns(campaignId, branchId);
        }

        /// <summary>
        /// Mails the chimp list sync.
        /// </summary>
        /// <param name="BranchId">The branch id.</param>
        /// <param name="mailChimpAPIKey">The mail chimp API key.</param>
        private void MailChimpListSync(int BranchId, string mailChimpAPIKey)
        {
            MailChimpListSync(BranchId, mailChimpAPIKey, true);
        }

        /// <summary>
        /// Mails the chimp list sync.
        /// </summary>
        /// <param name="BranchId">The branch id.</param>
        /// <param name="mailChimpAPIKey">The mail chimp API key.</param>
        /// <param name="dualSync">if set to <c>true</c> [dual sync].</param>
        private void MailChimpListSync(int BranchId, string mailChimpAPIKey, bool dualSync)
        {
            var mailChimpLists = _mailChimpApi.GetMailChimpLists(mailChimpAPIKey);
            var mailChimpListTable = _dataAccess.GetMailChimpList(BranchId.ToString());

            var mclistIds = (from item in mailChimpLists select item.id).ToList();
            var mclistTableIds = (from item in mailChimpListTable select item.ListId).ToList();

            /*mc-server   local-db
             * 0            1
             */

            var mclistIsNotInMcSvrIds = mclistTableIds.Except(mclistIds);
            foreach (string listId in mclistIsNotInMcSvrIds)
            {
                DeleteMailChimpListAndpcampaigns(listId, BranchId);
            }

            /*mc-server   local-db
            * 1           0
            */

            if (dualSync)
            {
                var newListIdsFromMc = mclistIds.Except(mclistTableIds);
                var newListsFromMc = from list in mailChimpLists where newListIdsFromMc.Contains(list.id) select list;
                var mailChimpList = new Common.Table.MailChimpList();
                foreach (var list in newListsFromMc)
                {
                    mailChimpList = new Common.Table.MailChimpList();
                    mailChimpList.ListId = list.id;
                    mailChimpList.Name = list.name;
                    mailChimpList.BranchId = BranchId;
                    mailChimpList.UserId = GetUserId(list.default_from_email);
                    _dataAccess.AddMailChimpLists(mailChimpList);
                }

                var updateListIdsFromMc = mclistIds.Intersect(mclistTableIds);
                var updateListsFromMc = from list in mailChimpLists where updateListIdsFromMc.Contains(list.id) select list;
                foreach (var list in updateListsFromMc)
                {
                    mailChimpList = new Common.Table.MailChimpList();
                    mailChimpList.ListId = list.id;
                    mailChimpList.Name = list.name;
                    //mailChimpList.BranchId = BranchId;
                    mailChimpList.UserId = GetUserId(list.default_from_email);
                    _dataAccess.UpdateMailChimpLists(mailChimpList);
                }
            }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <param name="defaultFromEmail">The default from email.</param>
        /// <returns></returns>
        private int GetUserId(string defaultFromEmail)
        {
            if (string.IsNullOrEmpty(defaultFromEmail))
                return 0;
            try
            {
                return _dataAccess.GetUserId(defaultFromEmail);
            }
            catch (Exception exception)
            {
                string err = MethodBase.GetCurrentMethod() + "\r\n\r\nException: " + exception.Message + "\r\n\r\nStackTrace: " + exception.StackTrace;
                Trace.TraceError(exception.Message);
                int Event_id = 2017;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category); 
                return 0;
            }
        }

        /// <summary>
        /// Deletes the mail chimp list andpcampaigns.
        /// </summary>
        /// <param name="listId">The list id.</param>
        /// <param name="branchId">The branch id.</param>
        private void DeleteMailChimpListAndpcampaigns(string listId, int branchId)
        {
            _dataAccess.DeleteMailChimpListAndpcampaigns(listId, branchId);
        }
    }
}
