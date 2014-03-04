using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Data;

namespace Common
{
    public class Table
    {
        public class LoanStages
        {
            public int LoanStageId;
            public DateTime Completed;
            public int SequenceNumber;
            public int FileId;
            public int WflTemplId;
            public int WflStageId;
            public int DaysFromEstClose;
            public int DaysFromCreation;
            public string StageName;
            public short PointDateField;
            public short PointNameField;
            public int TemplStageId;
            public int TaskCount;
            public short CalculationMethod;
        }

        public class WorkflowStage
        {
            public int WorkflowTemplId;
            public int WorkflowStageId;
            public string Name;
            public int SequenceNumber;
            public int DaysFromEstClose;
            public int TemplStageId;
            public bool Delete;
            public short CalculationMethod;
        }

        public class WorkflowTask
        {
            public int WorkflowStageId;
            public int WorkflowTaskId;
            public string Name;
            public int SequenceNumber;
            public int DaysFromEstClose;
            public int PrerequisiteTaskId;
            public int DaysAfterPrereq;
            public int OwnerRoleId;
            public int WarningEmailId;
            public int OverdueEmailId;
            public int CompletionEmailId;
            public int DaysDueFromPrevStage;
        }

        public class LoanTasks
        {
            public int LoanTaskId;
            public int FileId;
            public string Name;
            public int Owner;
            public int PrerequisiteTaskId;
            public int DaysAfterPrerequisiteTask;
            public int DaysFromEstClose;
            public int DaysFromCreation;
            public DateTime Due;
            public DateTime Completed;
            public int CompletedBy;
            public DateTime LastModified;
            public int LoanStageId;
            public DateTime Created;
            public int SequenceNumber;
            public int TemplTaskId;
            public int WflTemplId;
            public int WarmingEmailId;
            public int OverdueEmailId;
            public int CompletionEmailId;
            public int DaysDueFromPrevStage;
        }

        public class Users
        {
            public int UserId;
            public Boolean UserEnabled;
            public string Prefix;
            public string Username;
            public string Cell;
            public string EmailAddress;
            public string Fax;
            public string Phone;
            public string UserPictureFile;
            public string FirstName;
            public string LastName;
            public int RoleId;
            public string Password;
            public Int16 LoansPerPage;
            public string RoleName;
            public string NMLS;
        }
        public class UserLicenses
        {
            public int UserLicenseId;
            public int UserId;
            public string LicenseNumber;
        }

        public class LoanAutoEmails
        {
            public int LoanAutoEmailId;
            public int FileId;
            public int ToContactId;
            public int TouserId;
            public bool Enabled;
            public bool External;
            public int TempReportId;
            public DateTime Applied;
            public int AppliedBy;
            public DateTime LastRun;
            public int ScheduleType;
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
            public string RoleType;
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
            public DateTime DOB;
            public int Experian;
            public int TransUnion;
            public int Equifax;
            public string MailingAddr;
            public string MailingCity;
            public string MailingState;
            public string MailingZip;
            public int ContactCompanyId;
            public int ContactBranchId;
            public int WebAccountId;
            public int ContactRoleId;
            public string ContactRoleName;
            public string CompanyName;
            public List<Employment> employment;
            public Income income;
            public List<OtherIncome> otherincome;
            public List<Assets> assets;
        }

        public class Loans
        {
            public int FileId;
            public double AppraisedValue;
            public string CCScenario;
            public double CLTV;
            public string County;
            public DateTime DateOpen;
            public DateTime DateHMDA;
            public DateTime DateProcessing;
            public DateTime DateSubmit;
            public DateTime DateApprove;
            public DateTime DateReSubmit;
            public DateTime DateClearToClose;
            public DateTime DateNote;
            public DateTime DateDocs;
            public DateTime DateDocsOut;
            public DateTime DateFund;
            public DateTime DateDocsReceived;
            public DateTime DateRecord;
            public DateTime DateClose;
            public DateTime DateDenied;
            public DateTime DateCanceled;
            public DateTime DateSuspended;
            public DateTime Disposed;
            public double DownPay;
            public DateTime EstCloseDate;
            public int Lender;
            public string LienPosition;
            public double LoanAmount;
            public string LoanNumber;
            public string LoanType;
            public double LTV;
            public double MonthlyPayment;
            public string LenderNotes;
            public string Occupancy;
            public string Program;
            public string PropertyAddr;
            public string PropertyCity;
            public string PropertyState;
            public string PropertyZip;
            public string Purpose;
            public double Rate;
            public DateTime RateLockExpiration;
            public double SalesPrice;
            public double Term;
            public double Due;
            public string CurrentStage;
            public string ProspectLoanStatus;
            public string LastCompletedStage;
            public string LoanStatus;
            public string LeadRanking;
            public DateTime? PurchasedDate;
            public string PropertyType;
        }

        public class LoanAlert
        {
            public int FileId;
            public string AlertType;
            public string Desc;
            public DateTime Due;
            public int Owner;
            public int TaskId;
            public int RuleId;
            public int ClearedBy;
            public DateTime Cleared;
            public DateTime Created;
        }

        public class PointFileInfo
        {
            public int FileId;
            public int FolderId;
            public string Path;
            public int BranchId;
            public DateTime LastImported;
            public byte[] CurrentImage;
            public byte[] PreviousImage;
            public int PDSFileId { get; set; }
            public int PDSFolderId { get; set; }
        }

        public class LoanStatus
        {
            public int FileId;
            public string Status;
        }

        public class LoanNotes
        {
            public int NoteId;
            public DateTime Created;
            public string Sender;
            public string Note;
            public bool Exported;
            public bool ExternalViewing;

        }

        public class PointFieldDesc
        {
            public short FieldId;
            public LP2.Service.Common.PointFieldDataType DataType;
        }

        public class LoanPointField
        {
            public short FieldId;
            public string PrevValue;
            public string CurrValue;
        }

        public class DefaultStage
        {
            public int WflStageId;
            public int WflTemplId;
            public string Name;
            public short SequenceNumber;
            public short StageDateFld;
            public short StageNameFld;
            public short DaysFromEstClose;
            public short DaysAfterCreation;
            public short CalculateMethod;
        }

        public class TaskHistory
        {
            public int TaskHistoryId;
            public DateTime ActivityTime;
            public string ActivityName;
            public string User;
            public bool Exported;
        }

        public class EmailLog
        {
            public int EmailLogId { get; set; }
            public string Sender { get; set; }
            public string FromEmail { get; set; }
            public string Subject { get; set; }
            public string EmailBody { get; set; }
            public int FileId { get; set; }
            public DateTime LastSent { get; set; }
            public string ToUser { get; set; }
            public string ToContact { get; set; }
            public string ToEmail { get; set; }
            public bool Exported { get; set; }
        }

        public class UpdateContact
        {
            public int FileId { get; set; }
            public int ContactRoleId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }
        }

        public class TemplateEmail
        {
            public int TemplEmailId { get; set; }
            public bool Enabled { get; set; }
            public string Name { get; set; }
            public string Desc { get; set; }
            public int FromUserRoles { get; set; }
            public string FromEmailAddress { get; set; }
            public string FromUserName { get; set; }
            public string Content { get; set; }
            public string Subject { get; set; }
            public string Target { get; set; }
            public bool Custom { get; set; }
            public string SendTrigger { get; set; }
            public string SenderName { get; set; }
        }

        public class CompanyWeb
        {
            #region Model
            private bool _emailalertsenabled;
            private string _emailrelayserver;
            private string _defaultalertemail;
            private int? _emailinterval;
            private string _lpcompanyurl;
            private string _borrowerurl;
            private string _borrowergreeting;
            private string _homepagelogo;
            private string _logoforsubpages;
            private byte[] _homepagelogodata;
            private byte[] _subpagelogodata;
            private string _BackgroundLoanAlertPage;
            private bool _EnableEmailAuditTrail;
            private string _BackgroundWCFURL;
            private bool _SendEmailViaEWS;
            private string _EwsUrl;

            private int _SMTP_Port;
            private bool _AuthReq;
            private string _AuthEmailAccount;
            private string _AuthPassword;
            private string _SMTP_EncryptMethod;
            private string _EWS_Version;



            /// <summary>
            /// 
            /// </summary>
            public bool EmailAlertsEnabled
            {
                set { _emailalertsenabled = value; }
                get { return _emailalertsenabled; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string EmailRelayServer
            {
                set { _emailrelayserver = value; }
                get { return _emailrelayserver; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string DefaultAlertEmail
            {
                set { _defaultalertemail = value; }
                get { return _defaultalertemail; }
            }
            /// <summary>
            /// 
            /// </summary>
            public int? EmailInterval
            {
                set { _emailinterval = value; }
                get { return _emailinterval; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string LPCompanyURL
            {
                set { _lpcompanyurl = value; }
                get { return _lpcompanyurl; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string BorrowerURL
            {
                set { _borrowerurl = value; }
                get { return _borrowerurl; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string BorrowerGreeting
            {
                set { _borrowergreeting = value; }
                get { return _borrowergreeting; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string HomePageLogo
            {
                set { _homepagelogo = value; }
                get { return _homepagelogo; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string LogoForSubPages
            {
                set { _logoforsubpages = value; }
                get { return _logoforsubpages; }
            }
            /// <summary>
            /// 
            /// </summary>
            public byte[] HomePageLogoData
            {
                set { _homepagelogodata = value; }
                get { return _homepagelogodata; }
            }
            /// <summary>
            /// 
            /// </summary>
            public byte[] SubPageLogoData
            {
                set { _subpagelogodata = value; }
                get { return _subpagelogodata; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string BackgroundLoanAlertPage
            {
                set { _BackgroundLoanAlertPage = value; }
                get { return _BackgroundLoanAlertPage; }
            }
            /// <summary>
            /// 
            /// </summary>
            public bool EnableEmailAuditTrail
            {
                set { _EnableEmailAuditTrail = value; }
                get { return _EnableEmailAuditTrail; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string BackgroundWCFURL
            {
                set { _BackgroundWCFURL = value; }
                get { return _BackgroundWCFURL; }
            }
            /// <summary>
            /// 
            /// </summary>
            public bool SendEmailViaEWS
            {
                set { _SendEmailViaEWS = value; }
                get { return _SendEmailViaEWS; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string EwsUrl
            {
                set { _EwsUrl = value; }
                get { return _EwsUrl; }
            }

            public int SMTP_Port
            {
                get { return _SMTP_Port; }
                set { _SMTP_Port = value; }
            }

            public bool AuthReq
            {
                get { return _AuthReq; }
                set { _AuthReq = value; }
            }
            public string AuthEmailAccount
            {
                get { return _AuthEmailAccount; }
                set { _AuthEmailAccount = value; }
            }
            public string AuthPassword
            {
                get { return _AuthPassword; }
                set { _AuthPassword = value; }
            }
            public string SMTP_EncryptMethod
            {
                get { return _SMTP_EncryptMethod; }
                set { _SMTP_EncryptMethod = value; }
            }
            public string EWS_Version
            {
                get { return _EWS_Version; }
                set { _EWS_Version = value; }
            }



            #endregion Model

            private string _domain;


            public string EWS_Domain
            {
                get { return _domain; }
                set { _domain = value; }
            }
        }

        public class TemplateEmailRecipient
        {
            public int TemplRecipientId { get; set; }
            public int TemplEmailId { get; set; }
            public string EmailAddr { get; set; }
            public string UserRoles { get; set; }
            public string ContactRoles { get; set; }
            public string RecipientType { get; set; }
            public string UserName { get; set; }
            public string[] ToEmails { get; set; }
            public int[] ToUserIds { get; set; }
            public int[] ToContactIds { get; set; }
            public string[] CCEmails { get; set; }
            public int[] CCUserIds { get; set; }
            public int[] CCContactIds { get; set; }
            public int UserId { get; set; }
            public string RoleType { get; set; }
            public bool TaskOwner { get; set; }
        }

        public class PendingRuleInfo
        {
            public int RuleCondId { get; set; }
            public int RuleId { get; set; }
            public int RuleGroupId { get; set; }
            public int FileId { get; set; }
            public int RuleCondValue { get; set; }
            public string RuleType { get; set; }
            public override string ToString()
            {
                return "RuleCondId:" + RuleCondId;
            }
        }

        public class TemplateRules
        {
            public int RuleId { get; set; }
            public string Name { get; set; }
            public string Desc { get; set; }
            public bool Enabled { get; set; }
            public int AlertEmailTemplId { get; set; }
            public bool AckReq { get; set; }
            public int RecomEmailTemplid { get; set; }
            public string AdvFormula { get; set; }
            public int RuleScope { get; set; }
            public int LoanTarget { get; set; }
            public int AutoCampaignId { get; set; }
        }

        public class AutoCampaigns
        {
            public int CampaignId { get; set; }
            public int PaidBy { get; set; }
            public bool Enabled { get; set; }
            public int SelectedBy { get; set; }
            public DateTime Started { get; set; }
            public string LoanType { get; set; }
            public int TemplStageId { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class PendingEmailLog
        {
            public int EmailLogId { get; set; }
            public string ToUser { get; set; }
            public string ToContact { get; set; }
            public int EmailTmplId { get; set; }
            public bool Success { get; set; }
            public string Error { get; set; }
            public DateTime LastSent { get; set; }
            public int LoanAlertId { get; set; }
            public Int16 Retries { get; set; }
            public int FileId { get; set; }
            public string FromEmail { get; set; }
            public int FromUser { get; set; }
            public DateTime Created { get; set; }
            public Int16 AlertEmailType { get; set; }
            public byte[] EmailBody { get; set; }
            public string Subject { get; set; }
            public bool Exported { get; set; }
            public string ToEmail { get; set; }
            public int ProspectId { get; set; }
            public int ProspectAlertId { get; set; }
            public string CCUser { get; set; }
            public string CCContact { get; set; }
            public string ChainId { get; set; }
            public int SequenceNumber { get; set; }
            public bool EwsImported { get; set; }
            public string CCEmail { get; set; }
            public DateTime DateTimeReceived { get; set; }
            public string EmailUniqueId { get; set; }
        }

        public class Prospect
        {
            public int ContactId;
            public string LeadSource;
            public string ReferenceCode;
            public int Referral;
            public DateTime Created;
            public int CreatedBy;
            public DateTime Modifed;
            public int ModifiedBy;
            public int LoanOfficer;
            public string Status;
            public string CreditRanking;
            public string PreferredContact;
            public bool Dependents;
        }

        public class LoanTeam
        {
            public string Region;
            public string Division;
            public string Branch;
            public Users LoanOfficer;
            public Users Processor;
            public Users Closer;
            public Users DocPrep;
            public Users Shipper;
            public Users Underwriter;
        }

        public class ReportBranchInfo
        {
            public string Name;
            public string BranchAddress;
            public string Phone;
            public string WebURL;
            public string Email;
            public string CityBranchStateZip;
            public byte[] WebsiteLogo;
        }

        public class ReportLoanDetailInfo
        {
            public int FileId;
            public string PropertyAddr;
            public string PropertyCity;
            public string PropertyState;
            public string PropertyZip;
            public string SalesPrice;
            public string LoanAmount;
            public string Rate;
            public string Program;
            public string Purpose;
            public DateTime EstCloseDate;
            public string Progress;
            public string BorrowerName;
            public string CoBorrowerName;
            public string MailingAddr;
            public string MailingCity;
            public string MailingState;
            public string MailingZip;
            public string BusinessPhone;
            public string Fax;
            public string Email;
        }

        public class ReportLoanOfficerInfo
        {
            public string Name;
            public string Phone;
            public string Fax;
            public string EmailAddress;
            public byte[] UserPictureFile;
            public string NMLS;
        }

        public class ReportTaskInfo
        {
            public int LoanTaskId;
            public string Name;
            public string ICON;
            public DateTime Due;

        }
        public class ReportLoanContactInfo
        {
            public int ContactId;
            public string RolesName;
            public string Name;
            public string CompanyName;
            public string Email;
            public string BusinessPhone;
            public string Fax;
            public string Website;
            public byte[] Picture;

        }

        public class LoanStatusReport
        {
            public ReportBranchInfo BranchInfo;
            public ReportLoanDetailInfo LoanDetailInfo;
            public ReportLoanOfficerInfo LoanOfficerInfo;
            public ReportTaskInfo TaskInfo;
            public ReportLoanContactInfo LoanContactInfo;
        }

        public class LSRInfo
        {
            public int TemplReportId { get; set; }
            public int ToContactId { get; set; }
            public string ToContactUserName { get; set; }
            public string ToContactEmail { get; set; }
            public int ToUserId { get; set; }
            public string ToUserUserName { get; set; }
            public string ToUserEmail { get; set; }
            public int XTD { get; set; }
            public string Borrower { get; set; }
            public int ScheduleType { get; set; }
            public int FileId { get; set; }
            public int LoanAutoEmailid { get; set; }
            public DateTime LastRun { get; set; }
        }

        public class Template_Workflow
        {
            public int WflTemplId { get; set; }
            public string Name { get; set; }
            public bool Enabled { get; set; }
            public string Desc { get; set; }
            public string WorkflowType { get; set; }
            public bool Custom { get; set; }
            public bool Default { get; set; }
            public int CalculationMethod { get; set; }
        }
        public class AutoApplyWorkflowInfo
        {
            public int FileID { get; set; }
            public int WflTemplId { get; set; }

            public string Status { get; set; }
        }

        public class MailChimpList
        {
            public string ListId { get; set; }
            public string Name { get; set; }
            public int BranchId { get; set; }
            public int UserId { get; set; }
        }
        public class MailChimpCampaign
        {
            public string CId { get; set; }
            public string Name { get; set; }
            public int BranchId { get; set; }
        }
        public class MailChimpContact
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public int ContactId { get; set; }
        }

        public class Branch
        {
            public int BranchId { get; set; }
            public bool EnableMailChimp { get; set; }
            public string MailChimpAPIKey { get; set; }
        }

        public class CompanyReport
        {
            public int DOW { get; set; }
            public int TOD { get; set; }
            public int SenderRoleId { get; set; }
            public string SenderEmail { get; set; }
            public string SenderName { get; set; }
        }

        public class Company_MCT
        {
            public string ClientID { get; set; }
            public string PostURL { get; set; }
            public bool? PostDataEnabled { get; set; }
            public int ActiveLoanInterval { get; set; }
            public int ArchivedLoanInterval { get; set; }
            public int ArchivedLoanDisposeMonth { get; set; }
            public string ArchivedLoanStatuses { get; set; }
        }
    }
}
