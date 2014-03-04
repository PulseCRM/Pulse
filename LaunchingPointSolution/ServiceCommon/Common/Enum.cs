using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LP2.Service.Common
{
    public class InfoHubEventLog
    {
        public static string LogSource = "Pulse Service";
    }
    public class InfoHubEvent
    {
        public const int PointMgrEvent = 1000;
        public const int DatabaseEvent = 2000;
        public const int UserMgrEvent = 3000;
        public const int WorkflowMgrEvent = 4000;
        public const int EmailMgrEvent = 5000;
        public const int LeadMgrEvent = 6000;
    }
    public class ContactServiceTypes
    {
        public const string ContactService_Appraisal = "Appraisal";
        public const string ContactService_Builder = "Builder";
        public const string ContactService_Closing = "Closing Service";
        public const string ContactService_FloodInsurance = "Flood Insurance";
        public const string ContactService_HazardInsurance = "Hazard Insurance";
        public const string ContactService_Investor = "Investor";
        public const string ContactService_Legal = "Legal Service";
        public const string ContactService_Lending = "Lending";
        public const string ContactService_Mortgage = "Mortgage";
        public const string ContactService_MortgageInsurance = "Mortgage Insurance";
        public const string ContactService_RealEstate = "Real Estate";
        public const string ContactService_Title = "Title Insurance";
    }
    public class ContactRoles
    {
        public const string ContactRole_Borrower = "Borrower";
        public const string ContactRole_Coborrower = "CoBorrower";
        public const string ContactRole_Appraiser = "Appraiser";
        public const string ContactRole_Builder = "Builder";
        public const string ContactRole_BuyersAgent = "Buyer's Agent";
        public const string ContactRole_BuyersAttorney = "Buyer's Attorney";
        public const string ContactRole_Closing = "Closing Agent";
        public const string ContactRole_FloodInsurance = "Flood Insurance";
        public const string ContactRole_HazardInsurance = "Hazard Insurance";
        public const string ContactRole_Investor = "Investor";
        public const string ContactRole_Lender = "Lender";
        public const string ContactRole_ListingAgent = "Listing Agent";
        public const string ContactRole_MortgageBroker = "Mortgage Broker";
        public const string ContactRole_MortgageInsurance = "Mortgage Insurance";
        public const string ContactRole_PropertyTax = "Property Tax";
        public const string ContactRole_SellersAttorney = "Seller's Attorney";
        public const string ContactRole_SellingAgent = "Selling Agent";
        public const string ContactRole_Surveyor = "Surveyor";
        public const string ContactRole_Title = "Title Insurance";
    }

    public class UserRoles
    {
        public const string UserRole_Executive = "Executive";
        public const string UserRole_BranchMgr = "Branch Manager";
        public const string UserRole_LoanOfficer = "Loan Officer";
        public const string UserRole_LoanOfficerAssistant = "Loan Officer Assistant";
        public const string UserRole_Processor = "Processor";
        public const string UserRole_Underwriter = "Underwriter";
        public const string UserRole_DocPrep = "Doc Prep";
        public const string UserRole_Closer = "Closer";
        public const string UserRole_Shipper = "Shipper";
    }

    public abstract class PointStage
    {
        public const string Application = "Application";
        public const string SentToProcessing = "Sent to Processing";
        public const string HMDAComplete = "HMDA Complete";
        public const string HMDA = "HMDA";
        public const string HMDACompleted = "HMDA Completed";
        public const string Submit = "Submit";
        public const string Submitted = "Submitted";
        public const string Approve = "Approve";
        public const string Approved = "Approved";
        public const string Re_submit = "Re-Submit";
        public const string Resubmitted = "ReSubmitted";
        public const string Resubmit = "Resubmit";
        public const string CleartoClose = "Clear to Close";
        public const string ClearToClose = "Clear To Close";
        public const string DocsDrawn = "Docs Drawn";
        public const string DocsOut = "Docs Out";
        public const string DocsReceived = "Docs Received";
        public const string Fund = "Fund";
        public const string Funded = "Funded";
        public const string Record = "Record";
        public const string Recorded = "Recorded";
        public const string Suspend = "Suspend";
        public const string Suspended = "Suspended";
        public const string Close = "Close";
        public const string Closed = "Closed";
    }

    public enum PointStageDateField
    {
        Prospect = 4993,
        Open = 6020,
        Application = 6020,
        SentToProcessing = 4995,
        HDMA = 4997,
        Submitted = 6025,
        Approved = 6030,
        Resubmitted = 4999,
        ClearedToClose = 6027,
        DocsDrawn = 6035,
        DocsOut = 11440,
        DocsReceived = 11442,
        Funded = 6040,
        Recorded = 6070,
        Closed = 6055,
        Suspended = 6065,
        Denied = 6045,
        Canceled = 6050
    }

    public class LoanStatus
    {
        public const string LoanStatus_Processing = "Processing";
        public const string LoanStatus_Closed = "Closed";
        public const string LoanStatus_Canceled = "Canceled";
        public const string LoanStatus_Denied = "Denied";
        public const string LoanStatus_Suspended = "Suspended";
        public const string LoanStatus_Prospect = "Prospect";
        public const string LoanStatus_Prospect_Active = "Active";
        public const string LoanStatus_PostClose = "Post-Close";
    }

    public enum LoanStatusEnum
    {
        Processing = 1,
        Closed = 2,
        Canceled = 3,
        Denied = 4,
        Suspended = 5,
        Prospect = 6,
        Archive = 7,
        ProspectArchive = 8
    }

    public enum ProspectFlagEnum
    {
        Unknown = 0,
        ScheduledPointFlag = 1,
        ScheduledProspectFlag = 2,
        RealtimePointFlag = 3,
        RealtimeProspectFlag = 4,
        ArchivedLoansFlag = 5
    }

    public enum ContactRoleIdEnum
    {
        Borrower = 1,
        CoBorrower = 2,
        Lender = 3,
        Partner = 4,
        Builder = 5,
        BuyersAttorney = 6,
        BuyersAgent = 7,
        ClosingAgent = 8,
        FloodInsurance = 9,
        HazardInsurance = 10,
        Investor = 11,
        MortgageBroker = 12,
        MortgageInsurance = 13,
        SellingAgent = 14,
        TitleInsurance = 15,
        ListingAgent = 16,
        Appraiser = 17
    }

    public enum TaskType
    {
        StandardCondition = 1,
        BankerPTD = 2,
        BankerPTF = 3,
        BankerTrailing = 4,
        BankerInvestor = 5
    }

    public enum UserMgrCommandType
    {
        Unknown = 0,
        CreateUser = 1,
        UpdateUser = 2,
        ChangePassword = 3,
        DisableUser = 4,
        EnableUser = 5,
        DeleteUser = 6,
        ImportUsers = 7,
        StopImport = 8,
        StartImport = 9
    }

    public enum PointMgrCommandType
    {
        Unknown = 0,
        ImportAllLoans = 1,
        ImportLoans = 2,
        ImportLONames = 3,
        GetPointFile = 4,
        MovePointFile = 5,
        ExtendRateLock = 6,
        ImportCardex = 7,
        StopImport = 8,
        StartImport = 9,
        UpdateEstCloseDate = 10,
        UpdateStage = 11,
        ReassignLoan = 12,
        ReassignContact = 13,
        AddNote = 14,
        DisposeLoan = 15,
        UpdateBorrower = 16,
        UpdateLoanInfo = 17,
        CreateFile = 18,
        ConvertToLead = 19,
        DisposeLead = 20,
        ImportLockInfo = 21,
        UpdateLockInfo = 22,
        UpdateLoanProfitability = 23        
    }

    public enum WorkflowCmd
    {
        Unknown = 0,
        GenerateWorkflow = 1,
        MonitorLoans = 2,
        CompleteStage = 3,
        UnCompleteStage = 4,
        CalculateDueDates = 5,
        UpdateEstCloseDate = 6,
    }

    public enum PointFieldDataType
    {
        Unknown = 0,
        StringType = 1,
        NumericType = 2,
        BooleanType = 3,
        DateType = 4
    }

    public enum FieldType
    {
        Unknown = 0,
        Previous = 1,
        DB = 2,
        Default = 3
    }

    public enum Payer_Type
    {
        Company = 0,
        Branch = 1,
        User = 2
    }

    public enum CreditCardType : int
    {
        VISA = 0,
        MasterCard = 1,
        Amex = 2,
        Discover = 3
    }

    public enum WorkflowCalculationMethod : short
    {
        ToEstClose = 1,
        FromCreation = 2,
        AfterPrevStageCompl = 3
    }
}
