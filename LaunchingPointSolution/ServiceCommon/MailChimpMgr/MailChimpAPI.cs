using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Common;
using LP2.Service.Common;
using PerceptiveMCAPI;
using PerceptiveMCAPI.Methods;
using PerceptiveMCAPI.Types;
namespace MailChimpMgr
{
    public class MailChimpAPI
    {
        short Category = 50;
        /// <summary>
        /// Gets the mail chimp lists.
        /// </summary>
        /// <param name="apikey">The apikey.</param>
        /// <returns></returns>
        public List<listsResults.DataItem> GetMailChimpLists(string apikey)
        {
            //todo:pass
            listsInput input = new listsInput(apikey);
            lists cmd = new lists(input);
            listsOutput output = cmd.Execute();
            if (output.api_ErrorMessages.Count > 0)
            {
                StringBuilder sbErrDetail = new StringBuilder();
                foreach (var apiErrorMessage in output.api_ErrorMessages)
                {
                    sbErrDetail.AppendLine(string.Format("error code:{0},error msg:{1}", apiErrorMessage.code,
                                                         apiErrorMessage.error));
                }
                string errMsg = string.Format("apikey:{4}\r\n{0}{1}{2}{3}", output.api_Request, output.api_Response,     // raw data  
                sbErrDetail.ToString(), output.api_ValidatorMessages, apikey);
                //int Event_id = 5011;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                return new List<listsResults.DataItem>();
            }
            return output.result.data;
        }

        /// <summary>
        /// Gets the mail chimp campaigns.
        /// </summary>
        /// <param name="apikey">The apikey.</param>
        /// <returns></returns>
        public List<campaignsResults.DataItem> GetMailChimpCampaigns(string apikey)
        {
            //todo:pass
            return GetMailChimpCampaigns(apikey, string.Empty);
        }

        /// <summary>
        /// Gets the mail chimp campaign members.
        /// </summary>
        /// <param name="apikey">The apikey.</param>
        /// <param name="cid">The cid.</param>
        /// <returns></returns>
        public List<campaignMembersResults.DataItem> GetMailChimpCampaignMembers(string apikey, string cid)
        {
            //todo:pass
            campaignMembersInput input = new campaignMembersInput();
            campaignMembersParms parms = new campaignMembersParms();
            parms.apikey = apikey;
            parms.cid = cid;
            parms.status = EnumValues.campaignMembers_status.sent;
            input.parms = parms;
            campaignMembers cmd = new campaignMembers(input);
            campaignMembersOutput output = cmd.Execute();
            if (output.api_ErrorMessages.Count > 0)
            {
                StringBuilder sbErrDetail = new StringBuilder();
                foreach (var apiErrorMessage in output.api_ErrorMessages)
                {
                    sbErrDetail.AppendLine(string.Format("error code:{0},error msg:{1}", apiErrorMessage.code,
                                                         apiErrorMessage.error));
                }
                string errMsg = string.Format("apikey:{4}cid:{5}\r\n{0}{1}{2}{3}", output.api_Request, output.api_Response,     // raw data  
                           sbErrDetail.ToString(), output.api_ValidatorMessages, apikey, cid);
                //int Event_id = 5012;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                return new List<campaignMembersResults.DataItem>();
            }
            return output.result.data;
        }

        /// <summary>
        /// Gets the campaign bounce messages.
        /// </summary>
        /// <param name="apikey">The apikey.</param>
        /// <param name="cid">The cid.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public List<campaignBounceMessagesResults.DataItem> GetCampaignBounceMessages(string apikey, string cid, DateTime since)
        {
            campaignBounceMessagesInput input = new campaignBounceMessagesInput();

            campaignBounceMessagesParms parms = new campaignBounceMessagesParms();
            parms.apikey = apikey;
            parms.cid = cid;
            parms.since = since;
            input.parms = parms;

            var cmd = new campaignBounceMessages(input);

            campaignBounceMessagesOutput output = cmd.Execute();
            if (output.api_ErrorMessages.Count > 0)
            {
                StringBuilder sbErrDetail = new StringBuilder();
                foreach (var apiErrorMessage in output.api_ErrorMessages)
                {
                    sbErrDetail.AppendLine(string.Format("error code:{0},error msg:{1}", apiErrorMessage.code,
                                                         apiErrorMessage.error));
                }
                string errMsg = string.Format("apikey:{4}cid:{5}\r\n{0}{1}{2}{3}", output.api_Request, output.api_Response,     // raw data  
                                           sbErrDetail.ToString(), output.api_ValidatorMessages, apikey, cid);
                //int Event_id = 5014;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                return new List<campaignBounceMessagesResults.DataItem>();
            }
            return output.result.data;
        }

        /// <summary>
        /// Gets the mail chimp campaigns.
        /// </summary>
        /// <param name="apikey">The apikey.</param>
        /// <param name="mailChimpListId">The mail chimp list id.</param>
        /// <returns></returns>
        public List<campaignsResults.DataItem> GetMailChimpCampaigns(string apikey, string mailChimpListId)
        {
            //todo:pass
            var parms = new campaignsParms();
            parms.apikey = apikey;
            parms.filters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(mailChimpListId))
                parms.filters.Add("list_id", mailChimpListId);
            var input = new campaignsInput(parms);
            var cmd = new campaigns(input);
            var output = cmd.Execute();
            if (output.api_ErrorMessages.Count > 0)
            {
                StringBuilder sbErrDetail = new StringBuilder();
                foreach (var apiErrorMessage in output.api_ErrorMessages)
                {
                    sbErrDetail.AppendLine(string.Format("error code:{0},error msg:{1}", apiErrorMessage.code,
                                                         apiErrorMessage.error));
                }
                string errMsg = string.Format("apikey:{4}mailChimpListId:{5}\r\n{0}{1}{2}{3}", output.api_Request, output.api_Response,     // raw data  
                                           sbErrDetail, output.api_ValidatorMessages, apikey, mailChimpListId);
                //int Event_id = 5015;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
            }
            return output.result.data;
        }

        /// <summary>
        /// Lists the batch subscribe.
        /// </summary>
        /// <param name="apikey">The apikey.</param>
        /// <param name="mailChimpListId">The mail chimp list id.</param>
        /// <param name="subscriberList">The subscriber list.</param>
        /// <returns></returns>
        public MailChimp_Response ListBatchSubscribe(string apikey, string mailChimpListId, List<Table.MailChimpContact> subscriberList)
        {
            //todo:pass
            MailChimp_Response response = new MailChimp_Response();
            response.hdr = new RespHdr();
            string StatusInfo = "";

            var input = GetListBatchSubscribeInput(apikey, mailChimpListId);
            // method parameters 
            var batch = new List<Dictionary<string, object>>();
            foreach (Table.MailChimpContact sub_rec in subscriberList)
            {
                Dictionary<string, object> entry = new Dictionary<string, object>();
                entry.Add("EMAIL", sub_rec.Email);
                entry.Add("EMAIL_TYPE", EnumValues.emailType.NotSpecified);
                entry.Add("FNAME", sub_rec.FirstName);
                entry.Add("LNAME", sub_rec.LastName);
                batch.Add(entry);
            }
            input.parms.batch = batch;

            // execution 
            var cmd = new listBatchSubscribe(input);
            var output = cmd.Execute();

            // output, format with user control 
            if (output.api_ErrorMessages.Count > 0)
            {
                StringBuilder sbErrDetail = new StringBuilder();
                foreach (var apiErrorMessage in output.api_ErrorMessages)
                {
                    sbErrDetail.AppendLine(string.Format("error code:{0},error msg:{1}", apiErrorMessage.code,
                                                         apiErrorMessage.error));
                    StatusInfo = apiErrorMessage.error;
                }
                string errMsg = string.Format("apikey:{4}mailChimpListId:{5}\r\n{0}{1}{2}{3}", output.api_Request, output.api_Response,     // raw data  
                                           sbErrDetail, output.api_ValidatorMessages, apikey, mailChimpListId);
                int Event_id = 5016;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                response.hdr.Successful = false;
                response.hdr.StatusInfo = "MailChimpMgr: " + StatusInfo;
                return response;
            }
            else
            {
                //Console.Write(output);
                response.hdr.Successful = true;
                response.hdr.StatusInfo = "";
                return response;
            }

        }
        /// <summary>
        /// Lists the batch unsubscribe.
        /// </summary>
        /// <param name="apikey">The apikey.</param>
        /// <param name="mailChimpListId">The mail chimp list id.</param>
        /// <param name="subscriberList">The subscriber list.</param>
        /// <returns></returns>
        public MailChimp_Response ListBatchUnsubscribe(string apikey, string mailChimpListId, List<Table.MailChimpContact> subscriberList)
        {
            //todo:pass
            MailChimp_Response response = new MailChimp_Response();
            response.hdr = new RespHdr();
            string StatusInfo = "";

            var input = GetListBatchUnsubscribeInput(apikey, mailChimpListId);
            var deleteEmails = (from subscriber in subscriberList select subscriber.Email).ToList();
            input.parms.emails = deleteEmails;

            // execution 
            var cmd = new listBatchUnsubscribe(input);
            var output = cmd.Execute();

            if (output.api_ErrorMessages.Count > 0)
            {
                StringBuilder sbErrDetail = new StringBuilder();
                foreach (var apiErrorMessage in output.api_ErrorMessages)
                {
                    sbErrDetail.AppendLine(string.Format("error code:{0},error msg:{1}", apiErrorMessage.code,
                                                         apiErrorMessage.error));
                    StatusInfo = apiErrorMessage.error;
                }
                string errMsg = string.Format("apikey:{4}mailChimpListId:{5}\r\n{0}{1}{2}{3}", output.api_Request, output.api_Response,     // raw data  
                                           sbErrDetail, output.api_ValidatorMessages, apikey, mailChimpListId);
                int Event_id = 5017;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                response.hdr.Successful = false;
                response.hdr.StatusInfo = "MailChimpMgr: " + StatusInfo;
                return response;
            }
            else
            {
                //Console.Write(output);
                response.hdr.Successful = true;
                response.hdr.StatusInfo = "";
                return response;
            }

        }
        /// <summary>
        /// Lists the subscribe.
        /// </summary>
        /// <param name="apikey">The apikey.</param>
        /// <param name="mailChimpListId">The mail chimp list id.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <returns></returns>
        public MailChimp_Response ListSubscribe(string apikey, string mailChimpListId, Table.MailChimpContact subscriber)
        {
            //todo:pass
            MailChimp_Response response = new MailChimp_Response();
            response.hdr = new RespHdr();
            string StatusInfo = "";

            var input = GetListSubscribeInput(apikey, mailChimpListId);

            var entry = new Dictionary<string, object>();
            entry.Add("EMAIL", subscriber.Email);
            entry.Add("FNAME", subscriber.FirstName);
            entry.Add("LNAME", subscriber.LastName);
            input.parms.merge_vars = entry;
            input.parms.email_type = EnumValues.emailType.NotSpecified;
            input.parms.email_address = subscriber.Email;

            var cmd = new listSubscribe(input);
            var output = cmd.Execute();

            if (output.api_ErrorMessages.Count > 0)
            {
                StringBuilder sbErrDetail = new StringBuilder();
                foreach (var apiErrorMessage in output.api_ErrorMessages)
                {
                    sbErrDetail.AppendLine(string.Format("error code:{0},error msg:{1}", apiErrorMessage.code,
                                                         apiErrorMessage.error));
                    StatusInfo = apiErrorMessage.error;
                }
                string errMsg = string.Format("apikey:{4}mailChimpListId:{5}\r\n{0}{1}{2}{3}", output.api_Request, output.api_Response,     // raw data  
                                           sbErrDetail, output.api_ValidatorMessages, apikey, mailChimpListId);
                int Event_id = 5018;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                response.hdr.Successful = false;
                response.hdr.StatusInfo = "MailChimpMgr: " + StatusInfo;
                return response;
            }
            else
            {
                //Console.Write(output);
                response.hdr.Successful = true;
                response.hdr.StatusInfo = "";
                return response;
            }
        }
        /// <summary>
        /// Lists the unsubscribe.
        /// </summary>
        /// <param name="apikey">The apikey.</param>
        /// <param name="mailChimpListId">The mail chimp list id.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <returns></returns>
        public MailChimp_Response ListUnsubscribe(string apikey, string mailChimpListId, Table.MailChimpContact subscriber)
        {
            //todo:pass
            MailChimp_Response response = new MailChimp_Response();
            response.hdr = new RespHdr();
            string StatusInfo = "";

            var input = GetListUnsubscribeInput(apikey, mailChimpListId);
            input.parms.email_address = subscriber.Email;

            // execution 
            var cmd = new listUnsubscribe(input);
            var output = cmd.Execute();

            if (output.api_ErrorMessages.Count > 0)
            {
                StringBuilder sbErrDetail = new StringBuilder();
                foreach (var apiErrorMessage in output.api_ErrorMessages)
                {
                    sbErrDetail.AppendLine(string.Format("error code:{0},error msg:{1}", apiErrorMessage.code,
                                                         apiErrorMessage.error));
                    StatusInfo = apiErrorMessage.error;
                }
                string errMsg = string.Format("apikey:{4}mailChimpListId:{5}\r\n{0}{1}{2}{3}", output.api_Request, output.api_Response,     // raw data  
                                           sbErrDetail, output.api_ValidatorMessages, apikey, mailChimpListId);
                int Event_id = 5019;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                response.hdr.Successful = false;
                response.hdr.StatusInfo = "MailChimpMgr: " + StatusInfo;
                return response;
            }
            else
            {
                //Console.Write(output);
                response.hdr.Successful = true;
                response.hdr.StatusInfo = "";
                return response;
            }
        }

        /// <summary>
        /// Gets the list batch subscribe input.
        /// </summary>
        /// <param name="apikey">The apikey.</param>
        /// <param name="mailChimpListId">The mail chimp list id.</param>
        /// <returns></returns>
        private static listBatchSubscribeInput GetListBatchSubscribeInput(string apikey, string mailChimpListId)
        {
            var listBatchSubscribeInput = new listBatchSubscribeInput
            {
                api_Validate = true,
                api_AccessType = EnumValues.AccessType.Serial,
                api_OutputType = EnumValues.OutputType.JSON,
                parms =
                {
                    apikey = apikey,
                    id = mailChimpListId,
                    double_optin = false,
                    replace_interests = true,
                    update_existing = true
                }
            };
            return listBatchSubscribeInput;
        }
        /// <summary>
        /// Gets the list batch unsubscribe input.
        /// </summary>
        /// <param name="apikey">The apikey.</param>
        /// <param name="mailChimpListId">The mail chimp list id.</param>
        /// <returns></returns>
        private static listBatchUnsubscribeInput GetListBatchUnsubscribeInput(string apikey, string mailChimpListId)
        {
            var listBatchUnsubscribeInput = new listBatchUnsubscribeInput
            {
                api_Validate = true,
                api_AccessType = EnumValues.AccessType.Serial,
                api_OutputType = EnumValues.OutputType.JSON,
                parms =
                {
                    apikey = apikey,
                    id = mailChimpListId,
                    //todo:check the requirement
                    delete_member = true,
                    send_goodbye = true,
                    send_notify = false
                }
            };
            return listBatchUnsubscribeInput;
        }

        /// <summary>
        /// Gets the list subscribe input.
        /// </summary>
        /// <param name="apikey">The apikey.</param>
        /// <param name="mailChimpListId">The mail chimp list id.</param>
        /// <returns></returns>
        private static listSubscribeInput GetListSubscribeInput(string apikey, string mailChimpListId)
        {
            var listSubscribeInput = new listSubscribeInput
            {
                parms =
                {
                    apikey = apikey,
                    id = mailChimpListId,
                    double_optin = false,
                    replace_interests = true,
                    update_existing = true,
                    send_welcome = false
                }
            };
            return listSubscribeInput;
        }
        /// <summary>
        /// Gets the list unsubscribe input.
        /// </summary>
        /// <param name="apikey">The apikey.</param>
        /// <param name="mailChimpListId">The mail chimp list id.</param>
        /// <returns></returns>
        private static listUnsubscribeInput GetListUnsubscribeInput(string apikey, string mailChimpListId)
        {
            var listUnsubscribeInput = new listUnsubscribeInput
            {
                parms =
                {
                    apikey = apikey,
                    id = mailChimpListId,
                    //todo:check the requirement
                    delete_member = true,
                    send_goodbye = true,
                    send_notify = false
                }
            };
            return listUnsubscribeInput;
        }
    }
}
