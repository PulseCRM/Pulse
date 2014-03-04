using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Data;

namespace Common
{
    public class Record
    {
        public class Conditions
        {
            public int LoanCondId;
            public int FileId;
            public string CondName;
            public string CondType;
            public string Status;
            public DateTime Created;
            public string CreatedBy;
            public DateTime Received;
            public string ReceivedBy;
            public DateTime Collected;
            public string CollectedBy;
            public DateTime Submitted;
            public string SubmittedBy;
            public DateTime Cleared;
            public string ClearedBy;
            public int Sequence;
            public DateTime Due;
            public bool ExternalViewing;
            public bool Delete;
        }
        public class Docs
        {
            public int BasicDocId;
            public int FileId;
            public string DocName;
            public string Status;
            public DateTime Ordered;
            public string OrderedBy;
            public DateTime ReOrdered;
            public string ReOrderedBy;
            public DateTime Received;
            public string ReceivedBy;
            public DateTime Submitted;
            public string SubmittedBy;
            public DateTime Cleared;
            public string ClearedBy;
            public DateTime Due;
            public bool Delete;
        }
        public class LoanTasks
        {
            public string LoanTaskId;
            public string FileId;
            public string WflTemplId;
            public string Name;
            public string DependentTaskId;
            public string Due;
            public string Completed;
            public string CompletedBy;
            public string LastModified;
            public string TemplStageId;
            public string TaskType;
        }

        public class Users
        {
            public string UserId;
            public string UserEnabled;
            public string Prefix;
            public string Username;
            public string EmailAddress;
            public string UserPictureFile;
            public string FirstName;
            public string LastName;
            public string RoleId;
            public string Password;
            public string LoansPerPage;
        }

        public class Employment
        {
            public int EmplId;
            public int ContactId;
            public bool SelfEmployed;
            public string Position;
            public decimal StartYear;
            public decimal StartMonth;
            public decimal EndYear;
            public decimal EndMonth;
            public decimal YearsOnWork;
            public string Phone;
            public int ContactBranchId;
            public string CompanyName;
            public string Address;
            public string City;
            public string State;
            public string Zip;
        }

        public class Income
        {
            public int ProspectIncomeId;
            public int ContactId;
            public decimal Salary;
            public decimal Overtime;
            public decimal Bonuses;
            public decimal Commission;
            public decimal Div_Int;
            public decimal NetRent;
            public decimal Other;
            public int EmplId;
        }

        public class OtherIncome
        {
            public int ProspectOtherIncomeId;
            public int ContactId;
            public string Type;
            public decimal MonthlyIncome;
        }

        public class Assets
        {
            public int ProspectAssetId;
            public int ContactId;
            public string Name;
            public string Account;
            public decimal Amount;
            public string Type;
        }

        public class Contacts
        {
            public int ContactId;
            public string FirstName;
            public string MiddleName;
            public string LastName;
            public string NickName;
            public string Title;
            public string GenerationCode;
            public string SSN;
            public string HomePhone;
            public string CellPhone;
            public string BusinessPhone;
            public string Fax;
            public string Email;
            public string DOB;
            public string Experian;
            public string TransUnion;
            public string Equifax;
            public string MailingAddr;
            public string MailingCity;
            public string MailingState;
            public string MailingZip;
            public string ContactCompanyId;
            public string ContactBranchId;
            public string WebAccountId;
            public string CompanyName;
            public List<Employment> employment;
            public Income income;
            public List<OtherIncome> otherincome;
            public List<Assets> assets;
        }

        public class Loans
        {
            public int FileId;
            public string BranchName;
            public int BranchId;
            public int UserID;
            public string AppraisedValue;
            public string CCScenario;
            public string CLTV;
            public string County;
            public string DateCreated;
            public string DateOpen;
            public string DateHMDA;
            public string DateProcessing;
            public string DateSubmit;
            public string DateApprove;
            public string DateReSubmit;
            public string DateClearToClose;
            public string DateDocs;
            public string DateDocsOut;
            public string DateFund;
            public string DateDocsReceived;
            public string DateRecord;
            public string DateClose;
            public string DateDenied;
            public string DateCanceled;
            public string DateSuspended;
            public string DownPay;
            public string EstCloseDate;
            public string Lender;
            public string LienPosition;
            public string LoanAmount;
            public string LoanNumber;
            public string LoanType;
            public string LTV;
            public string MonthlyPayment;
            public string LenderNotes;
            public string Occupancy;
            public string Program;
            public string PropertyAddr;
            public string PropertyCity;
            public string PropertyState;
            public string PropertyZip;
            public string Purpose;
            public string Rate;
            public string RateLockExpiration;
            public string SalesPrice;
            public string Term;
            public string Due;
            public string PointFilePath;
            public string RentAmount;
            public string HousingStatus;
            public string InterestOnly;
            public string IncludeEscrow;
            public string Joint;
            public string LeadRanking;
            public string PurchasedDate;
            public string PropertyType;
        }

        public class LoanRecord
        {
            public Loans loans;
            public Contacts borrower_contacts;
            public Contacts coborrower_contacts;
            public LoanTeam team;
            public LoanAgents agents;
            public List<Docs> docs = new List<Docs>();
            public List<Conditions> conditions = new List<Conditions>();
            public List<Table.LoanNotes> notes = new List<Table.LoanNotes>();
            public LoanLocks lockInfos;
            public LoanLocksPage lockInfosPage;
            public LoanProfit loanProfit;
            public LoanArmCaps armsCapInfo;
        }

        public class LoanTeam
        {
            public string lo;
            public string processor;
            public string underwriter;
            public string docPrep;
            public string closer;
            public string shipper;
        }

        public class Agent
        {
            public string Company;
            public Contacts Contact;
        }

        public class LoanAgents
        {
            public Agent lender_contact1;
            public Agent lender_contact2;
            public Agent lender_contact3;
            public Agent mortgage_broker;
            public Agent appraiser;
            public Agent builder;
            public Agent buyers_agent;
            public Agent buyers_attorney;
            public Agent closing_agent;
            public Agent flood_insurance;
            public Agent hazard_insurance;
            public Agent investor;
            public Agent listing_agent;
            public Agent mortgage_insurance;
            public Agent selling_agent;
            public Agent title_insurance;
        }

        public class LoanLocks
        {
            public int FileId { get; set; }
            public string LockOption { get; set; }
            public string LockedBy { get; set; }
            public string LockTime { get; set; }
            public string LockTerm { get; set; }
            public string ConfirmedBy { get; set; }
            public string ConfirmTime { get; set; }
            public string LockExpirationDate { get; set; }
            public string Ext1Term { get; set; }
            public string Ext1LockExpDate { get; set; }
            public string Ext1LockTime { get; set; }
            public string Ext1LockedBy { get; set; }
            public string Ext1ConfirmTime { get; set; }
            public string Ext2Term { get; set; }
            public string Ext2LockExpDate { get; set; }
            public string Ext2LockTime { get; set; }
            public string Ext2LockedBy { get; set; }
            public string Ext2ConfirmTime { get; set; }
            public string Ext3Term { get; set; }
            public string Ext3LockExpDate { get; set; }
            public string Ext3LockTime { get; set; }
            public string Ext3LockedBy { get; set; }
            public string Ext3ConfirmTime { get; set; }
        }

        public class LoanLocksPage
        {
            public int FileId { get; set; }
            public string FICO { get; set; }
            public string Price { get; set; }
            public string MIOption { get; set; }
            public string PropertyType { get; set; }
            public string Float { get; set; }
            public string Lock { get; set; }
            public string LockDate { get; set; }
            public string LockTerm { get; set; }
            public string LockExpDate { get; set; }
            public string Extension12976 { get; set; }
            public string Extension12977 { get; set; }
        }

        public class LoanProfit
        {
            public int FileId { get; set; }
            public string CompensationPlan { get; set; }
            public string NetSell { get; set; }
            public string SRP { get; set; }
            public string LLPA { get; set; }            
            public string LenderCredit { get; set; }
            public string Price { get; set; }
            public string HedgeCost { get; set; }
            public string MandatoryFinalPrice { get; set; }
            public string CommitmentNumber { get; set; }
            public string CommitmentTerm { get; set; }
            public string CommitmentDate { get; set; }
            public string Error { get; set; }
            public string CommitmentExpDate { get; set; }            
            public string Investor { get; set; }
        }

        public class LoanArmCaps
        {
            public int FileId { get; set; }
            public string AdjCap { get; set; }
            public string LifeCap { get; set; }
            public string Index { get; set; }
            public string Margin { get; set; }
            public string InitAdjCap { get; set; }
        }
    }
}
