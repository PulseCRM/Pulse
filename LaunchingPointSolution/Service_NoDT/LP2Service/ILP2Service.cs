using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LP2.Service.Common;
using FocusIT.Pulse;
namespace LP2Service
{
    // NOTE: If you change the interface name "ILP2Service" here, you must also update the reference to "ILP2Service" in App.config.
    [ServiceContract]
    public interface ILP2Service
    {
        [OperationContract]
        string GetVersion();
        #region User Manager Methods
        [OperationContract]
        StartUserImportResponse StartUserImportService(StartUserImportRequest req);

        [OperationContract]
        StopUserImportResponse StopUserImportService(StopUserImportRequest req);

        [OperationContract]
        ImportADUsersResponse ImportADUsers(ImportADUsersRequest req);

        [OperationContract]
        UpdateADUserResponse UpdateADUser(UpdateADUserRequest req);
        #endregion

        #region Point Manager Methods
        [OperationContract]
        GetPointMgrStatusResponse GetPointManagerStatus(GetPointMgrStatusRequest req);

        [OperationContract]
        StartPointImportResponse StartPointImportService(StartPointImportRequest req);

        [OperationContract]
        StopPointImportResponse StopPointImportService(StopPointImportRequest req);
        #region import methods
        [OperationContract]
        ImportAllLoansResponse ImportAllLoans(ImportAllLoansRequest req);

        [OperationContract]
        ImportLoansResponse ImportLoans(ImportLoansRequest req);            

        [OperationContract]
        ImportLoanRepNamesResponse ImportLoanRepNames(ImportLoanRepNamesRequest req);

        [OperationContract]
        ImportCardexResponse ImportCardex(ImportCardexRequest req);
        #endregion
        [OperationContract]
        GetPointFileResponse GetPointFile(GetPointFileRequest req);

        [OperationContract]
        GetPointFileInfoResp GetPointFileInfo(GetPointFileInfoReq req);


        /// <summary>
        /// Checks the point file status.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        [OperationContract]
        CheckPointFileStatusResp CheckPointFileStatus(CheckPointFileStatusReq req);

        /// <summary>
        /// Imports the lock info.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        [OperationContract]
        ImportLockInfoResponse ImportLockInfo(ImportLockInfoRequest req);

        #region update methods
        [OperationContract]
        MoveFileResponse MoveFile(MoveFileRequest req);
        [OperationContract]
        DisposeLoanResponse DisposeLoan(DisposeLoanRequest req);
        /// <summary>
        /// Updates the lock info.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        [OperationContract]
        UpdateLockInfoResponse UpdateLockInfo(UpdateLockInfoRequest req);
        /// <summary>
        /// Updates the Loan Profitability info.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        [OperationContract]
        UpdateLockInfoResponse UpdateLoanProfitability(UpdateLockInfoRequest req);
        [OperationContract]
        UpdateConditionsResponse UpdateConditions(UpdateConditionsRequest req);
        [OperationContract]
        ExtendRateLockResponse ExtendRateLock(ExtendRateLockRequest req);
        [OperationContract]
        UpdateEstCloseDateResponse UpdateEstCloseDate(UpdateEstCloseDateRequest req);
        [OperationContract]
        UpdateStageResponse UpdateStage(UpdateStageRequest req);
        [OperationContract]
        ReassignLoanResponse ReassignLoan(ReassignLoanRequest req);
        [OperationContract]
        ReassignContactResponse ReassignContact(ReassignContactRequest req);
        [OperationContract]
        AddNoteResponse AddNote(AddNoteRequest req);
        [OperationContract]
        UpdateBorrowerResponse UpdateBorrower(UpdateBorrowerRequest req);
        [OperationContract]
        UpdateLoanInfoResponse UpdateLoanInfo(UpdateLoanInfoRequest req);
        //[OperationContract]
        //CreateFileResponse CreateFile(CreateFileRequest req);
        [OperationContract]
        ConvertToLeadResponse ConvertToLead(ConvertToLeadRequest req);
        [OperationContract]
        DisposeLeadResponse DisposeLead(DisposeLeadRequest req);
        #endregion
        #endregion
        #region Workflow Manager Methods
        [OperationContract]
        GenerateWorkflowResponse GenerateWorkflow(GenerateWorkflowRequest req);
        [OperationContract]
        CalculateDueDatesResponse CalculateDueDates(CalculateDueDatesRequest req);
        #endregion

        #region Email Manager Methods
        [OperationContract]
        SendEmailResponse SendEmail(SendEmailRequest req);
        [OperationContract]
        EmailPreviewResponse PreviewEmail(EmailPreviewRequest req);
        [OperationContract]
        bool ProcessEmailQue();

        #endregion

        #region Rule Manager

        /// <summary>
        /// Processes the loan rules.
        /// </summary>
        [OperationContract]
        void ProcessLoanRules();

        /// <summary>
        /// Acknowledges the alert.
        /// </summary>
        /// <param name="currentLoanAlertId">The current loan alert id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        [OperationContract]
        bool AcknowledgeAlert(int currentLoanAlertId, int userId);

        #endregion

        #region EWS

        /// <summary>
        /// Gets the replies.
        /// </summary>
        [OperationContract]
        int GetReplies();

        [OperationContract]
        string ReplyToMessage(ReplyToEmailRequest replyToEmailRequest);
        #endregion

        #region Marketing Manager Methods

        [OperationContract]
        UpdateCompanyResponse UpdateCompany(UpdateCompanyRequest req);

        [OperationContract]
        UpdateRegionResponse UpdateRegion(UpdateRegionRequest req);

        [OperationContract]
        UpdateBranchResponse UpdateBranch(UpdateBranchRequest req);

        [OperationContract]
        UpdateUserResponse UpdateUser(UpdateUserRequest req);

        [OperationContract]
        UpdateProspectResponse UpdateProspect(UpdateProspectRequest req);

        [OperationContract]
        StartCampaignResponse StartCampaign(StartCampaignRequest req);

        [OperationContract]
        RemoveCampaignResponse RemoveCampaign(RemoveCampaignRequest req);

        [OperationContract]
        CompleteCampaignEventResponse CompleteCampaignEvent(CompleteCampaignEventRequest req);

        [OperationContract]
        GetUserAccountBalanceResponse GetUserAccountBalance(GetUserAccountBalanceRequest req);

        [OperationContract]
        UpdateCreditCardResponse UpdateCreditCard(UpdateCreditCardRequest req);

        [OperationContract]
        GetCreditCardResponse GetCreditCard(ref GetCreditCardRequest req);

        [OperationContract]
        AddToAccountResponse AddToAccount(AddToAccountRequest req);

        [OperationContract]
        ReassignProspectResponse ReassignProspect(ReassignProspectRequest req);

        [OperationContract]
        bool SyncMarketingData(ref string err);

        #endregion

        #region Report Manager Methods
        [OperationContract]
        GenerateReportResponse GenerateReport(GenerateReportRequest req);

        [OperationContract]
        SendLSRResponse SendLSR(SendLSRRequest req);

        #region added for CR64

        [OperationContract]
        PreviewLSRResponse PreviewLSR(PreviewLSRRequest req);

        #endregion


        #endregion

        #region MailChimpManagerService

        [OperationContract]
        MailChimp_Response MailChimp_SyncNow(int BranchId);

        [OperationContract(Name = "MailChimp_SubscribeBatch")]
        MailChimp_Response MailChimp_Subscribe(List<int> ContactIds, string MailChimpListId);

        [OperationContract]
        MailChimp_Response MailChimp_Subscribe(int ContactId, string MailChimpListId);

        [OperationContract(Name = "MailChimp_UnsubscribeBatch")]
        MailChimp_Response MailChimp_Unsubscribe(List<int> ContactIds, string MailChimpListId);

        [OperationContract]
        MailChimp_Response MailChimp_Unsubscribe(int ContactId, string MailChimpListId);

        [OperationContract]
        void ScheduleMailChimp();

        #endregion

        #region DataTrac Manger Interfaces

        [OperationContract]
        DT_ImportLoansResponse DTImportLoans(DT_ImportLoansRequest req);

        [OperationContract]
        DT_GetStatusDatesResponse DTGetStatusDates(DT_GetStatusDatesRequest req);

        [OperationContract]
        DT_SubmitLoanResponse DTSubmitLoan(DT_SubmitLoanRequest req);
        #endregion

        #region Lead Manager Methods
        [OperationContract]
        RespHdr PostLead(FocusIT.Pulse.PostLeadRequest req);

        /// <summary>
        /// Leads the routing_ get next loanofficer.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns>LR_GetNextLoanOfficerResp.</returns>
        [OperationContract]
        LR_GetNextLoanOfficerResp LeadRouting_GetNextLoanofficer(LR_GetNextLoanOfficerReq req);

        /// <summary>
        /// Leads the routing_ assign loan officer.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns>RespHdr.</returns>
        [OperationContract]
        RespHdr LeadRouting_AssignLoanOfficer(LR_AssignLoanOfficerReq req);
        #endregion
    }
}
