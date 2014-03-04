using LP2.Service.Common;
using MarketingMgr.LeadStar;
using System;

namespace MarketingManager
{
    public enum MarketingPaymentType
    {
        PaidByCompany,
        PaidByBranch,
        PaidByUser
    }
    public class MarketingCampaignType
    {
        public const string Auto = "Auto";
        public const string Manual = "Manual";
    }

    public class MarketingLoanType
    {
        public const string Leads = "Leads";
        public const string Opportunities = "Opportunities";
        public const string ArchivedLoans = "Archived";
        public const string ActiveLoans = "Active";
    }

    public class AutoCampaigns
    {
        public int CampaignId;
        public MarketingPaymentType PaymentType;
        public bool Enabled;
        public int SelectedBy;
        public string LoanType;
        public int TemplStageId;
        public string TemplStageName;
        public int[] FileIds;
    }

    public class LoanMarketing
    {
        public int LoanMarketingId;
        public DateTime Selected;
        public string Type;
        public DateTime Started;
        public int StartedBy;
        public int CampaignId;
        public string Status;
        public int FileId;
        public int SelectedBy;
        public string LeadStarId;
    }

    public class MarketingQue
    {
        public int MarketingQueId;
        public DateTime QueTime;
        public string Type;
        public int LoanMarketingId;
    }

    public class MarketingLog
    {
        public int MarketingLogId;
        public int LoanMarketingId;
        public DateTime EventTime;
        public bool Success;
        public string Error;
    }

    public interface IMkgMgr
    {
        bool StartAutoCampaigns(StartCampaignRequest req, ref string err);

        bool UpdateCompany(ref string err);

        //UpdateRegionResponse UpdateRegion(UpdateRegionRequest req, ref string err);

        bool UpdateBranch(int BranchId, ref string err);

        bool UpdateUser(int UserId, ref string err);

        bool UpdateProspect(int FileId, ref string err);

        StartCampaignResponse StartCampaign(StartCampaignRequest req, ref string err);

        bool RemoveCampaign(RemoveCampaignRequest req, ref string err);

        CompleteCampaignEventResponse CompleteCampaignEvent(CompleteCampaignEventRequest req, ref string err);

        GetUserAccountBalanceResponse GetUserAccountBalance(GetUserAccountBalanceRequest req, ref string err);

        AddToAccountResponse AddToAccount(AddToAccountRequest req, ref string err);

        bool ReassignProspect(ReassignProspectRequest req, ref string err);

        GetCreditCardResponse GetCreditCard(GetCreditCardRequest req, ref string err);

        UpdateCreditCardResponse UpdateCreditCard(UpdateCreditCardRequest req, ref string err);

        bool Scheduled_MarketingEvents(ref string err);

        bool ProcessSyncMarketingData(ref string err);

        bool Scheduled_SyncMarketingData(ref string err);

    }
}