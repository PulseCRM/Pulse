using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LP2.Service.Common;

namespace MailChimpMgr
{
    public class MailChimpManagerService : IMailChimpManagerService
    {
        private readonly static MailChimpManager _mailChimpManager = new MailChimpManager();
        /// <summary>
        /// Mails the chimp_ sync now.
        /// Retrieve the Branches.MailChimpAPIKey and invoke the MailChimp API lists to get all the MailChimp lists for the branch and perform the steps below:
        ///     Update the MailChimpLists table with the records obtained from MailChimp.
        ///     Compare the list of MailChimp lists against the records from the MailChimpLists table. If the database record within the MailChimpLists table is not found in the list from MailChimp, delete it from the MailChimpLists table and the associated ContactMailCampaigns record referencing the same LID.
        /// Retrieve the Branches.MailChimpAPIKey and invoke the MailChimp API campaigns to get all the MailChimpcampaigns for the branch and perform the steps below:
        ///     Update the MailChimpCampaigns table with the records obtained from MailChimp.
        ///     Compare the list of MailChimp campaigns against the records from the MailChimpCampaignstable. If the database record within the MailChimpCampaignstable is not found in the list from MailChimp, delete it from the MailChimpCampaignstable and update the associatedContactMailCampaigns records withContactMailCampaigns.CID=NULL referencing the same CID.
        /// </summary>
        /// <param name="BranchId">The branch id.</param>
        /// <returns></returns>
        public MailChimp_Response MailChimp_SyncNow(int BranchId)
        {
            MailChimp_Response response = new MailChimp_Response();
            response.hdr = new RespHdr();

            if (BranchId <= 0)
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = string.Format("MailChimpMgr: BranchId is null.");
                return response;
            }

            return _mailChimpManager.MailChimp_SyncNow(BranchId);
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

            response.hdr.Successful = true;
            response.hdr.StatusInfo = "";
            return response;
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

            response.hdr.Successful = true;
            response.hdr.StatusInfo = "";
            return response;
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

            response.hdr.Successful = true;
            response.hdr.StatusInfo = "";
            return response;
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

            response.hdr.Successful = true;
            response.hdr.StatusInfo = "";
            return response;
        }
    }
}
