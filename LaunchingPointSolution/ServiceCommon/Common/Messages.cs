using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Collections.ObjectModel;

namespace LP2.Service.Common
{
    #region Common Request Header and Response Header
    [DataContract]
    public class ReqHdr
    {
        string securityToken;
        int userid;
        [DataMember]
        public string SecurityToken
        {
            get { return securityToken; }
            set { securityToken = value; }
        }
        [DataMember]
        public int UserId
        {
            get { return userid; }
            set { userid = value; }
        }
    }

    [DataContract]
    public class RespHdr
    {
        bool success;      // 1 = success, 0 = failure
        string status;       // status message or error information
        int reqId;              // Request Id returned so that the web page can get progress info in the RequestQueue table.
        [DataMember]
        public bool Successful
        {
            get { return success; }
            set { success = value; }
        }
        [DataMember]
        public string StatusInfo
        {
            get { return status; }
            set { status = value; }
        }
        [DataMember]
        public int RequestId
        {
            get { return reqId; }
            set { reqId = value; }
        }
    }
    #endregion

    #region User Manager Requests/Responses
    [DataContract]
    public class User
    {
        int m_UserId;
        string m_Username;
        string m_Firstname;
        string m_Lastname;
        string m_Password;
        string m_Email;
        string m_Phone;
        string m_Cell;
        string m_Fax;
        bool m_Enabled;
        public User()
        { }
        public User(string username, string firstname, string lastname, string email, string pwd, bool enabled)
        {
            m_Username = username;
            m_Firstname = firstname;
            m_Lastname = lastname;
            m_Email = email;
            m_Password = pwd;
            m_Enabled = enabled;
        }
        [DataMember]
        public int UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        [DataMember]
        public string Username
        {
            get { return m_Username; }
            set { m_Username = value; }
        }

        [DataMember]
        public string Firstname
        {
            get { return m_Firstname; }
            set { m_Firstname = value; }
        }
        [DataMember]
        public string Lastname
        {
            get { return m_Lastname; }
            set { m_Lastname = value; }
        }
        [DataMember]
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }
        [DataMember]
        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
        }
        [DataMember]
        public bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }
        [DataMember]
        public string Phone
        {
            get { return m_Phone; }
            set { m_Phone = value; }
        }
        [DataMember]
        public string Cell
        {
            get { return m_Cell; }
            set { m_Cell = value; }
        }
        [DataMember]
        public string Fax
        {
            get { return m_Fax; }
            set { m_Fax = value; }
        }

        public string ExchangePassword { get; set; }

    }

    [DataContract]
    public class StartUserImportRequest
    {
        [DataMember]
        public ReqHdr hdr;
    }

    [DataContract]
    public class StartUserImportResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class StopUserImportRequest
    {
        [DataMember]
        public ReqHdr hdr;
    }

    [DataContract]
    public class StopUserImportResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class ImportADUsersRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public string AD_OU_Filter;
    }

    [DataContract]
    public class ImportADUsersResponse
    {
        [DataMember]
        public RespHdr hdr;
        [DataMember]
        public int RequestId;
    }

    [DataContract]
    public class UpdateADUserRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public UserMgrCommandType Command;     // add, update, delete, disable, enable, hange Password
        [DataMember]
        public string AD_OU_Filter;
        [DataMember]
        public User AD_User;
    }

    [DataContract]
    public class UpdateADUserResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    #endregion

    #region Point Manager Requests/Responses
    #region Common Data Contracts
    [DataContract]
    public class Address
    {
        [DataMember]
        public string Street;
        [DataMember]
        public string City;
        [DataMember]
        public string State;
        [DataMember]
        public string Zip;
    }

    [DataContract]
    public class ContactInfo
    {
        [DataMember]
        public string HomePhone;
        [DataMember]
        public string CellPhone;
        [DataMember]
        public string BusinessPhone;
        [DataMember]
        public string Fax;
        [DataMember]
        public string Email;
    }

    [DataContract]
    public class LoanDetail
    {
        [DataMember]
        public string LoanOfficer;
        [DataMember]
        public string Ranking;
        [DataMember]
        public string Borrower;
        [DataMember]
        public string Coborrower;
        [DataMember]
        public string Amount;
        [DataMember]
        public string InterestRate;
        [DataMember]
        public string EstimatedCloseDate;
        [DataMember]
        public string LoanProgram;
        [DataMember]
        public string PropertyAddress;
        [DataMember]
        public string City;
        [DataMember]
        public string State;
        [DataMember]
        public string Zip;
        [DataMember]
        public string PointFolder;
        [DataMember]
        public string PointFilename;
        [DataMember]
        public string CreatedOn;
        [DataMember]
        public string CreatedBy;
        [DataMember]
        public string ModifiedOn;
        [DataMember]
        public string ModifiedBy;
    }

    [DataContract]
    public class Agent
    {
        [DataMember]
        public string FirstName;
        [DataMember]
        public string LastName;
        [DataMember]
        public string Company;
        [DataMember]
        public Address Addr;
        [DataMember]
        public ContactInfo Contact;
    }
    [DataContract]
    public class SubjectProperty
    {
        [DataMember]
        public Address Address;
        [DataMember]
        public string County;
    }

    [DataContract]
    public class Borrower
    {
        [DataMember]
        public string FirstName;
        [DataMember]
        public string MiddleName;
        [DataMember]
        public string LastName;
        [DataMember]
        public string NickName;
        [DataMember]
        public string Birthdate;
        [DataMember]
        public string Title;
        [DataMember]
        public string GenerationCode;
        [DataMember]
        public string SSN;
        [DataMember]
        public string Experian;
        [DataMember]
        public string Equifax;
        [DataMember]
        public string Empirco;
        [DataMember]
        public Address MailingAddress;
        [DataMember]
        public ContactInfo ContactInfo;
    }

    [DataContract]
    public class StageInfo
    {
        [DataMember]
        public string StageName;
        [DataMember(IsRequired = true)]
        public int LoanStageId;
        [DataMember]
        public DateTime Completed;
    }
    [DataContract]
    public class ReassignUserInfo
    {
        [DataMember]
        public int FileId;
        [DataMember]
        public int NewUserId;
        [DataMember]
        public int RoleId;
    }
    [DataContract]
    public class ReassignContactInfo
    {
        [DataMember]
        public int FileId;
        [DataMember]
        public int NewContactId;
        [DataMember]
        public int ContactRoleId;
    }
    #endregion
    #region Get Point Manager Status
    [DataContract]
    public class GetPointMgrStatusRequest
    {
        [DataMember]
        public ReqHdr hdr;
    }
    [DataContract]
    public class GetPointMgrStatusResponse
    {
        [DataMember]
        public RespHdr hdr;
        [DataMember]
        public bool Running;
    }
    #endregion
    #region Point Manager Messages
    #region Start/Stop Point Import
    [DataContract]
    public class StartPointImportRequest
    {
        [DataMember]
        public ReqHdr hdr;
    }
    public class StartPointImportResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class StopPointImportRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public string[] PointFolders;    // a list of Point folders 
    }
    [DataContract]
    public class StopPointImportResponse
    {
        [DataMember]
        public RespHdr hdr;
    }
    #endregion
    #region Import Loans
    [DataContract]
    public class ImportAllLoansRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public string[] PointFolders;   // a list of Point Folders 
    }
    [DataContract]
    public class ImportAllLoansResponse
    {
        [DataMember]
        public RespHdr hdr;
        [DataMember]
        public int RequestId;
    }
    [DataContract]
    public class ImportLoansRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int[] FileIds;           // Point File Ids of the selected Point files 
    }
    [DataContract]
    public class ImportLoansResponse
    {
        [DataMember]
        public RespHdr hdr;
        [DataMember]
        public int RequestId;
    }
    #endregion
    #region Get Point File
    [DataContract]
    public class GetPointFileRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public string PointFile;           // full path of the Point file
    }

    [DataContract]
    public class GetPointFileResponse
    {
        [DataMember]
        public RespHdr hdr;
        [DataMember]
        public byte[] FileContent;
    }
    #endregion
    #region Import Loan Rep Names
    [DataContract]
    public class ImportLoanRepNamesRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public string[] PointFolders;           // full path of the Point folders 
    }
    [DataContract]
    public class ImportLoanRepNamesResponse
    {
        [DataMember]
        public RespHdr hdr;
        [DataMember]
        public int RequestId;
    }
    #endregion
    #region Import Cardex
    [DataContract]
    public class ImportCardexRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public string CardexFile;           // full path of the Cardex file
    }

    [DataContract]
    public class ImportCardexResponse
    {
        [DataMember]
        public RespHdr hdr;
        [DataMember]
        public int RequestId;
    }
    #endregion

    #region GetPointFileInfo

    [DataContract]
    public class GetPointFileInfoReq
    {
        [DataMember]
        public ReqHdr hdr { get; set; }

        [DataMember]
        public int FileId { get; set; }
    }

    [DataContract]
    public class GetPointFileInfoResp
    {
        [DataMember]
        public RespHdr hdr { get; set; }

        [DataMember]
        public bool FileExists { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public byte[] FileImage { get; set; }
    }

    [DataContract]
    public class CheckPointFileStatusReq
    {
        [DataMember]
        public ReqHdr hdr { get; set; }
        [DataMember]
        public int FileId { get; set; }
    }
    [DataContract]
    public class CheckPointFileStatusResp
    {
        [DataMember]
        public RespHdr hdr { get; set; }
        [DataMember]
        public bool FileLocked { get; set; }
    }


    #endregion

    #region Lock information

    [DataContract]
    public class ImportLockInfoRequest
    {
        [DataMember]
        public ReqHdr hdr { get; set; }

        [DataMember]
        public int FileId { get; set; }
    }

    [DataContract]
    public class ImportLockInfoResponse
    {
        [DataMember]
        public RespHdr hdr { get; set; }
    }

    [DataContract]
    public class UpdateLockInfoRequest
    {
        [DataMember]
        public ReqHdr hdr { get; set; }

        [DataMember]
        public int FileId { get; set; }
    }

    [DataContract]
    public class UpdateLockInfoResponse
    {
        [DataMember]
        public RespHdr hdr { get; set; }

        [DataMember]
        public int FileId { get; set; }
    }

    #endregion
    #endregion
    #region Point Update Messages
    [DataContract]
    public class ExtendRateLockRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;
        [DataMember]
        public int DaysExtended;
        [DataMember]
        public DateTime NewDate;
    }

    [DataContract]
    public class ExtendRateLockResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class UpdateStageRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;				//Point file Id
        [DataMember]
        public List<StageInfo> StageList;
    }

    [DataContract]
    public class UpdateStageResponse
    {
        [DataMember]
        public RespHdr hdr;
    }
    [DataContract]
    public class ReassignLoanRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public List<ReassignUserInfo> reassignUsers;
    }

    [DataContract]
    public class ReassignLoanResponse
    {
        [DataMember]
        public RespHdr hdr;
    }
    [DataContract]
    public class ReassignContactRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public List<ReassignContactInfo> reassignContacts;
    }

    [DataContract]
    public class ReassignContactResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class MoveFileRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;
        [DataMember]
        public int NewFolderId;
    }

    [DataContract]
    public class MoveFileResponse
    {
        [DataMember]
        public RespHdr hdr;
    }
    [DataContract]
    public class DisposeLoanRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;
        [DataMember]
        public int NewFolderId;
        [DataMember]
        public string LoanStatus;
        [DataMember]
        public DateTime StatusDate;
    }
    [DataContract]
    public class DisposeLoanResponse
    {
        [DataMember]
        public RespHdr hdr;
    }
    [DataContract]
    public class UpdateBorrowerRequest
    {
        [DataMember]
        public ReqHdr hdr;

        [DataMember]
        public int ContactId;
    }
    [DataContract]
    public class UpdateBorrowerResponse
    {
        [DataMember]
        public RespHdr hdr;
    }
    [DataContract]
    public class UpdateLoanInfoRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;
        [DataMember]
        public bool CreateFile;
    }
    [DataContract]
    public class UpdateLoanInfoResponse
    {
        [DataMember]
        public RespHdr hdr;
    }
    [DataContract]
    public class CreateFileRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public LoanDetail LoanDetail;
    }
    [DataContract]
    public class CreateFileResponse
    {
        [DataMember]
        public RespHdr hdr;
    }
    [DataContract]
    public class UpdateEstCloseDateRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;
        [DataMember]
        public DateTime NewDate;
    }
    [DataContract]
    public class UpdateEstCloseDateResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class AddNoteRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;
        [DataMember]
        public DateTime Created;
        [DataMember]
        public string Sender;
        [DataMember]
        public string Note;
        [DataMember]
        public Nullable<DateTime> NoteTime;
    }

    [DataContract]
    public class AddNoteResponse
    {
        [DataMember]
        public RespHdr hdr;
    }
    [DataContract]
    public class ConvertToLeadRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;
        [DataMember]
        public int NewFolderId;
    }
    [DataContract]
    public class ConvertToLeadResponse
    {
        [DataMember]
        public RespHdr hdr;
    }
    [DataContract]
    public class DisposeLeadRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;
        [DataMember]
        public int NewFolderId;
        [DataMember]
        public string LoanStatus;
        [DataMember]
        public DateTime StatusDate;
    }
    [DataContract]
    public class DisposeLeadResponse
    {
        [DataMember]
        public RespHdr hdr;
    }
    #endregion
    #endregion

    #region Workflow Manager Requests/Responses
    [DataContract]
    public class GenerateWorkflowRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;
        [DataMember]
        public int WorkflowTemplId;
    }

    [DataContract]
    public class GenerateWorkflowResponse
    {
        [DataMember]
        public RespHdr hdr;
        [DataMember]
        public int ReqId;
    }
    [DataContract]
    public class CalculateDueDatesRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;
        [DataMember]
        public int UserId;
        [DataMember]
        public DateTime NewEstCloseDate;
    }
    [DataContract]
    public class CalculateDueDatesResponse
    {
        [DataMember]
        public RespHdr hdr;
    }
    #endregion

    #region Email Manager Requests/Responses

    [DataContract]
    public class SendEmailRequest : ICloneable
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int UserId;
        [DataMember]
        public int FileId;
        [DataMember]
        public int EmailTemplId;
        [DataMember]
        public int[] EmailTemplIds;
        [DataMember]
        public int[] ToUserIds;
        [DataMember]
        public int[] ToContactIds;
        [DataMember]
        public string[] ToEmails;
        [DataMember]
        public int[] CCUserIds;
        [DataMember]
        public int[] CCContactIds;
        [DataMember]
        public string[] CCEmails;
        [DataMember]
        public int LoanAlertId;
        [DataMember]
        public byte[] EmailBody;
        [DataMember]
        public int EmailQueId { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public bool AppendPictureSignature { get; set; }
        [DataMember]
        public int ProspectId { get; set; }
        [DataMember]
        public int PropsectTaskId { get; set; }

        public int PropsectAlertId { get; set; }
        [DataMember]
        public int TaskId { get; set; }

        [DataMember]
        public Dictionary<string, byte[]> Attachments;
        public object Clone()
        {
            SendEmailRequest request = new SendEmailRequest(this);
            return request;
        }

        public SendEmailRequest()
        {
        }

        public SendEmailRequest(SendEmailRequest req)
        {
            this.hdr = req.hdr;
            UserId = req.UserId;
            FileId = req.FileId;
            EmailTemplId = req.EmailTemplId;
            EmailTemplIds = req.EmailTemplIds;
            ToUserIds = req.ToUserIds;
            ToContactIds = req.ToContactIds;
            ToEmails = req.ToEmails;
            CCUserIds = req.CCUserIds;
            CCContactIds = req.CCContactIds;
            CCEmails = req.CCEmails;
            LoanAlertId = req.LoanAlertId;
            EmailBody = req.EmailBody;
            EmailQueId = req.EmailQueId;
            EmailSubject = req.EmailSubject;
            AppendPictureSignature = req.AppendPictureSignature;
            ProspectId = req.ProspectId;
            PropsectTaskId = req.PropsectTaskId;
            PropsectAlertId = req.PropsectAlertId;
            TaskId = req.TaskId;
            Attachments = req.Attachments;
        }
    }

    [DataContract]
    public class SendEmailResponse
    {
        [DataMember]
        public RespHdr resp;
        [DataMember]
        public int ReqId;
    }
    #endregion

    #region Email Preview Request/Response
    [DataContract]
    public class EmailPreviewRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int UserId;
        [DataMember]
        public int FileId;
        [DataMember]
        public int EmailTemplId;
        [DataMember]
        public int LoanAlertId;
        [DataMember]
        public byte[] EmailBody;
        [DataMember]
        public bool AppendPictureSignature { get; set; }
        [DataMember]
        public int ProspectId { get; set; }
        [DataMember]
        public int PropsectTaskId { get; set; }
        [DataMember]
        public int TaskId { get; set; }
    }
    [DataContract]
    public class EmailPreviewResponse
    {
        [DataMember]
        public RespHdr resp;
        [DataMember]
        public byte[] EmailHtmlContent;

    }

    #endregion

    #region Reply Email

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ReplyToEmailRequest
    {
        [DataMember]
        public ReqHdr hdr { get; set; }
        [DataMember]
        public int FromUser { get; set; }
        [DataMember]
        public int ReplyToEmailLogId { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public int[] ToUser { get; set; }
        [DataMember]
        public string ToEmail { get; set; }
        [DataMember]
        public int[] ToContact { get; set; }
        [DataMember]
        public int[] CCUser { get; set; }
        [DataMember]
        public string CCEmail { get; set; }
        [DataMember]
        public int[] CCContact { get; set; }
        [DataMember]
        public bool AppendPictureSignature { get; set; }
        [DataMember]
        public byte[] EmailBody { get; set; }
        [DataMember]
        public string EmailUniqueId { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ReplyToEmailResponse
    {
        [DataMember]
        public RespHdr resp { get; set; }
    }

    #endregion

    #region Marketing Manager Requests/Responses

    [DataContract]
    public class UpdateCompanyRequest
    {
        [DataMember]
        public ReqHdr hdr;
    }

    [DataContract]
    public class UpdateCompanyResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class UpdateRegionRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int RegionId;
    }

    [DataContract]
    public class UpdateRegionResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class UpdateBranchRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int BranchId;
    }

    [DataContract]
    public class UpdateBranchResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class UpdateUserRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int UserId;
    }

    [DataContract]
    public class UpdateUserResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class UpdateCreditCardRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public string Card_ID;
        [DataMember]
        public string Card_Exp_Month;
        [DataMember]
        public string Card_Exp_Year;
        [DataMember]
        public string Card_First_Name;
        [DataMember]
        public string Card_IsDefault;
        [DataMember]
        public string Card_Last_Name;
        [DataMember]
        public string Card_Number;
        [DataMember]
        public string Card_SIC;
        [DataMember]
        public CreditCardType Card_Type;
    }

    [DataContract]
    public class UpdateCreditCardResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class GetCreditCardRequest
    {
        [DataMember]
        public LP2.Service.Common.ReqHdr hdr;

        [DataMember]
        public string Card_ID;
        [DataMember]
        public string Card_Exp_Month;
        [DataMember]
        public string Card_Exp_Year;
        [DataMember]
        public string Card_First_Name;
        [DataMember]
        public string Card_IsDefault;
        [DataMember]
        public string Card_Last_Name;
        [DataMember]
        public string Card_Number;
        [DataMember]
        public string Card_SIC;
        [DataMember]
        public CreditCardType Card_Type;
    }

    [DataContract]
    public class GetCreditCardResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class AddToAccountRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public decimal Amount;
    }

    [DataContract]
    public class AddToAccountResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class ReassignProspectRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int[] UserId;
        [DataMember]
        public int[] ContactId;
        [DataMember]
        public int[] FileId;
        [DataMember]
        public int FromUser;
        [DataMember]
        public int ToUser;
    }

    [DataContract]
    public class ReassignProspectResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class UpdateProspectRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;
    }

    [DataContract]
    public class UpdateProspectResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class StartCampaignRequest
    {
        [DataMember(IsRequired = true)]
        public ReqHdr hdr;
        [DataMember(IsRequired = true)]
        public int CampaignId;
        [DataMember(IsRequired = true)]
        public int[] FileId;
        [DataMember(IsRequired = true)]
        public DateTime StartDate;
        [DataMember(IsRequired = true)]
        public Payer_Type payer;
        [DataMember(IsRequired = false)]
        public bool Auto;
    }

    [DataContract]
    public class StartCampaignResponse
    {
        [DataMember]
        public RespHdr hdr;
        [DataMember]
        public string ErrorCode;
    }

    [DataContract]
    public class RemoveCampaignRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int CampaignId;
        [DataMember]
        public int FileId;
    }

    [DataContract]
    public class RemoveCampaignResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class CompleteCampaignEventRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;
        [DataMember]
        public int CampaignId;
        [DataMember]
        public int EventId;
    }

    [DataContract]
    public class CompleteCampaignEventResponse
    {
        [DataMember]
        public RespHdr hdr;
        [DataMember]
        public bool Completed;
        [DataMember]
        public DateTime Execution_Date;
    }

    [DataContract]
    public class GetUserAccountBalanceRequest
    {
        [DataMember]
        public ReqHdr hdr;
    }

    [DataContract]
    public class GetUserAccountBalanceResponse
    {
        [DataMember]
        public RespHdr hdr;
        [DataMember]
        public decimal Balance;
    }

    #endregion

    #region Report Manager methods
    [DataContract]
    public class GenerateReportRequest
    {
        [DataMember(IsRequired = true)]
        public ReqHdr hdr;           //required
        [DataMember(IsRequired = true)]
        public int TemplReportId;    //required
        [DataMember]
        public int FileId;
        [DataMember]
        public bool External;
        [DataMember]
        public bool Preview;
        public int LoanAutoEmailId { get; set; }
    }

    [DataContract]
    public class SendLSRRequest
    {
        [DataMember(IsRequired = true)]
        public ReqHdr hdr;           //required
        [DataMember(IsRequired = true)]
        public int TemplReportId;    //required
        [DataMember]
        public int FileId;
        [DataMember]
        public bool External;
        [DataMember]
        public Dictionary<string, byte[]> Attachments;
        [DataMember]
        public int LoanAutoEmailid { get; set; }
        [DataMember]
        public int ToContactId { get; set; }
        [DataMember]
        public string ToContactUserName { get; set; }
        [DataMember]
        public string ToContactEmail { get; set; }
        [DataMember]
        public int ToUserId { get; set; }
        [DataMember]
        public string ToUserUserName { get; set; }
        [DataMember]
        public string ToUserEmail { get; set; }
        [DataMember]
        public string Borrower { get; set; }
    }
    [DataContract]
    public class SendLSRResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    [DataContract]
    public class GenerateReportResponse
    {
        [DataMember(IsRequired = true)]
        public RespHdr hdr;
        [DataMember]
        public byte[] ReportContent;
    }

    #region added for CR64

    [DataContract]
    public class PreviewLSRRequest
    {
        [DataMember(IsRequired = true)]
        public ReqHdr hdr; //required

        [DataMember(IsRequired = true)]
        public int FileId;

        [DataMember]
        public int ContactId;

        [DataMember]
        public int UserId;
    }

    [DataContract]
    public class PreviewLSRResponse
    {
        [DataMember(IsRequired = true)]
        public RespHdr hdr;

        [DataMember]
        public byte[] ReportContent;
    }

    #endregion



    #endregion

    #region DataTrac Manager Requests/Responses

    [DataContract]
    public class DT_ImportLoansRequest
    {
        [DataMember(IsRequired = true)]
        public ReqHdr hdr;           //required

        [DataMember(IsRequired = true)]
        public int[] FileIds;        //required
    }

    [DataContract]
    public class DT_ImportLoansResponse
    {
        [DataMember(IsRequired = true)]
        public RespHdr hdr;
    }

    [DataContract]
    public class DT_GetStatusDatesRequest
    {
        [DataMember(IsRequired = true)]
        public ReqHdr hdr;           //required

        [DataMember(IsRequired = true)]
        public int[] FileIds;        //required
    }
    [DataContract]
    public class DT_GetStatusDatesResponse
    {
        [DataMember(IsRequired = true)]
        public RespHdr hdr;
    }

    public class DT_SubmitLoanRequest
    {
        [DataMember(IsRequired = true)]
        public ReqHdr hdr;           //required

        [DataMember(IsRequired = true)]
        public int FileId;        //required

        [DataMember(IsRequired = true)]
        public string Loan_Program;   //required

        [DataMember(IsRequired = true)]
        public string Originator_Type;   //required
    }

    public class StatusDate
    {
        public string StatusName;
        public Nullable<DateTime> CompletionDate;
    }

    [DataContract]
    public class DT_SubmitLoanResponse
    {
        [DataMember(IsRequired = true)]
        public RespHdr hdr;
    }

    [DataContract]
    public class MailChimp_Response
    {
        [DataMember(IsRequired = true)]
        public RespHdr hdr;
    }
    #endregion

    #region Point Manager Conditions

    [DataContract]
    public class Conditions
    {
        [DataMember]
        public int ConditionId;
        [DataMember]
        public string Name;
        [DataMember]
        public string Status;
    }

    [DataContract]
    public class UpdateConditionsRequest
    {
        [DataMember]
        public ReqHdr hdr;
        [DataMember]
        public int FileId;
        [DataMember]
        public List<Conditions> ConditionList;
    }

    [DataContract]
    public class UpdateConditionsResponse
    {
        [DataMember]
        public RespHdr hdr;
    }

    #endregion


    #region Lead Routing Engine

    [DataContract]
    public class LR_GetNextLoanOfficerReq
    {
        [DataMember]
        public ReqHdr ReqHdr { get; set; }

        [DataMember]
        public string LeadSource { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string Purpose { get; set; }

        [DataMember]
        public string LoanType { get; set; }
    }

    [DataContract]
    public class LR_GetNextLoanOfficerResp
    {
        [DataMember]
        public RespHdr RespHdr { get; set; }

        [DataMember]
        public int NextLoanOfficerID { get; set; }
    }

    [DataContract]
    public class LR_AssignLoanOfficerReq
    {
        [DataMember]
        public ReqHdr ReqHdr { get; set; }

        [DataMember]
        public int LoanOfficerId { get; set; }

        [DataMember]
        public int BorrowerContactId { get; set; }

        [DataMember]
        public int CoBorrowerContactId { get; set; }

        [DataMember]
        public int LoanId { get; set; }
    }

    #endregion




}
