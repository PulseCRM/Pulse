﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Simulator.HILLH001 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="HILLH001.ILP2Service")]
    public interface ILP2Service {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/GetReplies", ReplyAction="http://tempuri.org/ILP2Service/GetRepliesResponse")]
        int GetReplies();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ReplyToMessage", ReplyAction="http://tempuri.org/ILP2Service/ReplyToMessageResponse")]
        string ReplyToMessage(LP2.Service.Common.ReplyToEmailRequest replyToEmailRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/UpdateCompany", ReplyAction="http://tempuri.org/ILP2Service/UpdateCompanyResponse")]
        LP2.Service.Common.UpdateCompanyResponse UpdateCompany(LP2.Service.Common.UpdateCompanyRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/UpdateRegion", ReplyAction="http://tempuri.org/ILP2Service/UpdateRegionResponse")]
        LP2.Service.Common.UpdateRegionResponse UpdateRegion(LP2.Service.Common.UpdateRegionRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/UpdateBranch", ReplyAction="http://tempuri.org/ILP2Service/UpdateBranchResponse")]
        LP2.Service.Common.UpdateBranchResponse UpdateBranch(LP2.Service.Common.UpdateBranchRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/UpdateUser", ReplyAction="http://tempuri.org/ILP2Service/UpdateUserResponse")]
        LP2.Service.Common.UpdateUserResponse UpdateUser(LP2.Service.Common.UpdateUserRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/UpdateProspect", ReplyAction="http://tempuri.org/ILP2Service/UpdateProspectResponse")]
        LP2.Service.Common.UpdateProspectResponse UpdateProspect(LP2.Service.Common.UpdateProspectRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/StartCampaign", ReplyAction="http://tempuri.org/ILP2Service/StartCampaignResponse")]
        LP2.Service.Common.StartCampaignResponse StartCampaign(LP2.Service.Common.StartCampaignRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/RemoveCampaign", ReplyAction="http://tempuri.org/ILP2Service/RemoveCampaignResponse")]
        LP2.Service.Common.RemoveCampaignResponse RemoveCampaign(LP2.Service.Common.RemoveCampaignRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/CompleteCampaignEvent", ReplyAction="http://tempuri.org/ILP2Service/CompleteCampaignEventResponse")]
        LP2.Service.Common.CompleteCampaignEventResponse CompleteCampaignEvent(LP2.Service.Common.CompleteCampaignEventRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/GetUserAccountBalance", ReplyAction="http://tempuri.org/ILP2Service/GetUserAccountBalanceResponse")]
        LP2.Service.Common.GetUserAccountBalanceResponse GetUserAccountBalance(LP2.Service.Common.GetUserAccountBalanceRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/UpdateCreditCard", ReplyAction="http://tempuri.org/ILP2Service/UpdateCreditCardResponse")]
        LP2.Service.Common.UpdateCreditCardResponse UpdateCreditCard(LP2.Service.Common.UpdateCreditCardRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/GetCreditCard", ReplyAction="http://tempuri.org/ILP2Service/GetCreditCardResponse")]
        LP2.Service.Common.GetCreditCardResponse GetCreditCard(ref LP2.Service.Common.GetCreditCardRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/AddToAccount", ReplyAction="http://tempuri.org/ILP2Service/AddToAccountResponse")]
        LP2.Service.Common.AddToAccountResponse AddToAccount(LP2.Service.Common.AddToAccountRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ReassignProspect", ReplyAction="http://tempuri.org/ILP2Service/ReassignProspectResponse")]
        LP2.Service.Common.ReassignProspectResponse ReassignProspect(LP2.Service.Common.ReassignProspectRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/SyncMarketingData", ReplyAction="http://tempuri.org/ILP2Service/SyncMarketingDataResponse")]
        bool SyncMarketingData(ref string err);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/GenerateReport", ReplyAction="http://tempuri.org/ILP2Service/GenerateReportResponse")]
        LP2.Service.Common.GenerateReportResponse GenerateReport(LP2.Service.Common.GenerateReportRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/SendLSR", ReplyAction="http://tempuri.org/ILP2Service/SendLSRResponse")]
        LP2.Service.Common.SendLSRResponse SendLSR(LP2.Service.Common.SendLSRRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/DTImportLoans", ReplyAction="http://tempuri.org/ILP2Service/DTImportLoansResponse")]
        LP2.Service.Common.DT_ImportLoansResponse DTImportLoans(LP2.Service.Common.DT_ImportLoansRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/DTGetStatusDates", ReplyAction="http://tempuri.org/ILP2Service/DTGetStatusDatesResponse")]
        LP2.Service.Common.DT_GetStatusDatesResponse DTGetStatusDates(LP2.Service.Common.DT_GetStatusDatesRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/DTSubmitLoan", ReplyAction="http://tempuri.org/ILP2Service/DTSubmitLoanResponse")]
        LP2.Service.Common.DT_SubmitLoanResponse DTSubmitLoan(LP2.Service.Common.DT_SubmitLoanRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/MailChimp_SyncNow", ReplyAction="http://tempuri.org/ILP2Service/MailChimp_SyncNowResponse")]
        LP2.Service.Common.MailChimp_Response MailChimp_SyncNow(int BranchId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/MailChimp_SubscribeBatch", ReplyAction="http://tempuri.org/ILP2Service/MailChimp_SubscribeBatchResponse")]
        LP2.Service.Common.MailChimp_Response MailChimp_SubscribeBatch(int[] ContactIds, string MailChimpListId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/MailChimp_Subscribe", ReplyAction="http://tempuri.org/ILP2Service/MailChimp_SubscribeResponse")]
        LP2.Service.Common.MailChimp_Response MailChimp_Subscribe(int ContactId, string MailChimpListId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/MailChimp_UnsubscribeBatch", ReplyAction="http://tempuri.org/ILP2Service/MailChimp_UnsubscribeBatchResponse")]
        LP2.Service.Common.MailChimp_Response MailChimp_UnsubscribeBatch(int[] ContactIds, string MailChimpListId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/MailChimp_Unsubscribe", ReplyAction="http://tempuri.org/ILP2Service/MailChimp_UnsubscribeResponse")]
        LP2.Service.Common.MailChimp_Response MailChimp_Unsubscribe(int ContactId, string MailChimpListId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ScheduleMailChimp", ReplyAction="http://tempuri.org/ILP2Service/ScheduleMailChimpResponse")]
        void ScheduleMailChimp();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/GetVersion", ReplyAction="http://tempuri.org/ILP2Service/GetVersionResponse")]
        string GetVersion();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/StartUserImportService", ReplyAction="http://tempuri.org/ILP2Service/StartUserImportServiceResponse")]
        LP2.Service.Common.StartUserImportResponse StartUserImportService(LP2.Service.Common.StartUserImportRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/StopUserImportService", ReplyAction="http://tempuri.org/ILP2Service/StopUserImportServiceResponse")]
        LP2.Service.Common.StopUserImportResponse StopUserImportService(LP2.Service.Common.StopUserImportRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ImportADUsers", ReplyAction="http://tempuri.org/ILP2Service/ImportADUsersResponse")]
        LP2.Service.Common.ImportADUsersResponse ImportADUsers(LP2.Service.Common.ImportADUsersRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/UpdateADUser", ReplyAction="http://tempuri.org/ILP2Service/UpdateADUserResponse")]
        LP2.Service.Common.UpdateADUserResponse UpdateADUser(LP2.Service.Common.UpdateADUserRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/GetPointManagerStatus", ReplyAction="http://tempuri.org/ILP2Service/GetPointManagerStatusResponse")]
        LP2.Service.Common.GetPointMgrStatusResponse GetPointManagerStatus(LP2.Service.Common.GetPointMgrStatusRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/StartPointImportService", ReplyAction="http://tempuri.org/ILP2Service/StartPointImportServiceResponse")]
        LP2.Service.Common.StartPointImportResponse StartPointImportService(LP2.Service.Common.StartPointImportRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/StopPointImportService", ReplyAction="http://tempuri.org/ILP2Service/StopPointImportServiceResponse")]
        LP2.Service.Common.StopPointImportResponse StopPointImportService(LP2.Service.Common.StopPointImportRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ImportAllLoans", ReplyAction="http://tempuri.org/ILP2Service/ImportAllLoansResponse")]
        LP2.Service.Common.ImportAllLoansResponse ImportAllLoans(LP2.Service.Common.ImportAllLoansRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ImportLoans", ReplyAction="http://tempuri.org/ILP2Service/ImportLoansResponse")]
        LP2.Service.Common.ImportLoansResponse ImportLoans(LP2.Service.Common.ImportLoansRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ImportLoanRepNames", ReplyAction="http://tempuri.org/ILP2Service/ImportLoanRepNamesResponse")]
        LP2.Service.Common.ImportLoanRepNamesResponse ImportLoanRepNames(LP2.Service.Common.ImportLoanRepNamesRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ImportCardex", ReplyAction="http://tempuri.org/ILP2Service/ImportCardexResponse")]
        LP2.Service.Common.ImportCardexResponse ImportCardex(LP2.Service.Common.ImportCardexRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/GetPointFile", ReplyAction="http://tempuri.org/ILP2Service/GetPointFileResponse")]
        LP2.Service.Common.GetPointFileResponse GetPointFile(LP2.Service.Common.GetPointFileRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/MoveFile", ReplyAction="http://tempuri.org/ILP2Service/MoveFileResponse")]
        LP2.Service.Common.MoveFileResponse MoveFile(LP2.Service.Common.MoveFileRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/DisposeLoan", ReplyAction="http://tempuri.org/ILP2Service/DisposeLoanResponse")]
        LP2.Service.Common.DisposeLoanResponse DisposeLoan(LP2.Service.Common.DisposeLoanRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ExtendRateLock", ReplyAction="http://tempuri.org/ILP2Service/ExtendRateLockResponse")]
        LP2.Service.Common.ExtendRateLockResponse ExtendRateLock(LP2.Service.Common.ExtendRateLockRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/UpdateEstCloseDate", ReplyAction="http://tempuri.org/ILP2Service/UpdateEstCloseDateResponse")]
        LP2.Service.Common.UpdateEstCloseDateResponse UpdateEstCloseDate(LP2.Service.Common.UpdateEstCloseDateRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/UpdateStage", ReplyAction="http://tempuri.org/ILP2Service/UpdateStageResponse")]
        LP2.Service.Common.UpdateStageResponse UpdateStage(LP2.Service.Common.UpdateStageRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ReassignLoan", ReplyAction="http://tempuri.org/ILP2Service/ReassignLoanResponse")]
        LP2.Service.Common.ReassignLoanResponse ReassignLoan(LP2.Service.Common.ReassignLoanRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ReassignContact", ReplyAction="http://tempuri.org/ILP2Service/ReassignContactResponse")]
        LP2.Service.Common.ReassignContactResponse ReassignContact(LP2.Service.Common.ReassignContactRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/AddNote", ReplyAction="http://tempuri.org/ILP2Service/AddNoteResponse")]
        LP2.Service.Common.AddNoteResponse AddNote(LP2.Service.Common.AddNoteRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/UpdateBorrower", ReplyAction="http://tempuri.org/ILP2Service/UpdateBorrowerResponse")]
        LP2.Service.Common.UpdateBorrowerResponse UpdateBorrower(LP2.Service.Common.UpdateBorrowerRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/UpdateLoanInfo", ReplyAction="http://tempuri.org/ILP2Service/UpdateLoanInfoResponse")]
        LP2.Service.Common.UpdateLoanInfoResponse UpdateLoanInfo(LP2.Service.Common.UpdateLoanInfoRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ConvertToLead", ReplyAction="http://tempuri.org/ILP2Service/ConvertToLeadResponse")]
        LP2.Service.Common.ConvertToLeadResponse ConvertToLead(LP2.Service.Common.ConvertToLeadRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/DisposeLead", ReplyAction="http://tempuri.org/ILP2Service/DisposeLeadResponse")]
        LP2.Service.Common.DisposeLeadResponse DisposeLead(LP2.Service.Common.DisposeLeadRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/GenerateWorkflow", ReplyAction="http://tempuri.org/ILP2Service/GenerateWorkflowResponse")]
        LP2.Service.Common.GenerateWorkflowResponse GenerateWorkflow(LP2.Service.Common.GenerateWorkflowRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/CalculateDueDates", ReplyAction="http://tempuri.org/ILP2Service/CalculateDueDatesResponse")]
        LP2.Service.Common.CalculateDueDatesResponse CalculateDueDates(LP2.Service.Common.CalculateDueDatesRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/SendEmail", ReplyAction="http://tempuri.org/ILP2Service/SendEmailResponse")]
        LP2.Service.Common.SendEmailResponse SendEmail(LP2.Service.Common.SendEmailRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/PreviewEmail", ReplyAction="http://tempuri.org/ILP2Service/PreviewEmailResponse")]
        LP2.Service.Common.EmailPreviewResponse PreviewEmail(LP2.Service.Common.EmailPreviewRequest req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ProcessEmailQue", ReplyAction="http://tempuri.org/ILP2Service/ProcessEmailQueResponse")]
        bool ProcessEmailQue();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/ProcessLoanRules", ReplyAction="http://tempuri.org/ILP2Service/ProcessLoanRulesResponse")]
        void ProcessLoanRules();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILP2Service/AcknowledgeAlert", ReplyAction="http://tempuri.org/ILP2Service/AcknowledgeAlertResponse")]
        bool AcknowledgeAlert(int currentLoanAlertId, int userId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILP2ServiceChannel : Simulator.HILLH001.ILP2Service, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LP2ServiceClient : System.ServiceModel.ClientBase<Simulator.HILLH001.ILP2Service>, Simulator.HILLH001.ILP2Service {
        
        public LP2ServiceClient() {
        }
        
        public LP2ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LP2ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LP2ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LP2ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int GetReplies() {
            return base.Channel.GetReplies();
        }
        
        public string ReplyToMessage(LP2.Service.Common.ReplyToEmailRequest replyToEmailRequest) {
            return base.Channel.ReplyToMessage(replyToEmailRequest);
        }
        
        public LP2.Service.Common.UpdateCompanyResponse UpdateCompany(LP2.Service.Common.UpdateCompanyRequest req) {
            return base.Channel.UpdateCompany(req);
        }
        
        public LP2.Service.Common.UpdateRegionResponse UpdateRegion(LP2.Service.Common.UpdateRegionRequest req) {
            return base.Channel.UpdateRegion(req);
        }
        
        public LP2.Service.Common.UpdateBranchResponse UpdateBranch(LP2.Service.Common.UpdateBranchRequest req) {
            return base.Channel.UpdateBranch(req);
        }
        
        public LP2.Service.Common.UpdateUserResponse UpdateUser(LP2.Service.Common.UpdateUserRequest req) {
            return base.Channel.UpdateUser(req);
        }
        
        public LP2.Service.Common.UpdateProspectResponse UpdateProspect(LP2.Service.Common.UpdateProspectRequest req) {
            return base.Channel.UpdateProspect(req);
        }
        
        public LP2.Service.Common.StartCampaignResponse StartCampaign(LP2.Service.Common.StartCampaignRequest req) {
            return base.Channel.StartCampaign(req);
        }
        
        public LP2.Service.Common.RemoveCampaignResponse RemoveCampaign(LP2.Service.Common.RemoveCampaignRequest req) {
            return base.Channel.RemoveCampaign(req);
        }
        
        public LP2.Service.Common.CompleteCampaignEventResponse CompleteCampaignEvent(LP2.Service.Common.CompleteCampaignEventRequest req) {
            return base.Channel.CompleteCampaignEvent(req);
        }
        
        public LP2.Service.Common.GetUserAccountBalanceResponse GetUserAccountBalance(LP2.Service.Common.GetUserAccountBalanceRequest req) {
            return base.Channel.GetUserAccountBalance(req);
        }
        
        public LP2.Service.Common.UpdateCreditCardResponse UpdateCreditCard(LP2.Service.Common.UpdateCreditCardRequest req) {
            return base.Channel.UpdateCreditCard(req);
        }
        
        public LP2.Service.Common.GetCreditCardResponse GetCreditCard(ref LP2.Service.Common.GetCreditCardRequest req) {
            return base.Channel.GetCreditCard(ref req);
        }
        
        public LP2.Service.Common.AddToAccountResponse AddToAccount(LP2.Service.Common.AddToAccountRequest req) {
            return base.Channel.AddToAccount(req);
        }
        
        public LP2.Service.Common.ReassignProspectResponse ReassignProspect(LP2.Service.Common.ReassignProspectRequest req) {
            return base.Channel.ReassignProspect(req);
        }
        
        public bool SyncMarketingData(ref string err) {
            return base.Channel.SyncMarketingData(ref err);
        }
        
        public LP2.Service.Common.GenerateReportResponse GenerateReport(LP2.Service.Common.GenerateReportRequest req) {
            return base.Channel.GenerateReport(req);
        }
        
        public LP2.Service.Common.SendLSRResponse SendLSR(LP2.Service.Common.SendLSRRequest req) {
            return base.Channel.SendLSR(req);
        }
        
        public LP2.Service.Common.DT_ImportLoansResponse DTImportLoans(LP2.Service.Common.DT_ImportLoansRequest req) {
            return base.Channel.DTImportLoans(req);
        }
        
        public LP2.Service.Common.DT_GetStatusDatesResponse DTGetStatusDates(LP2.Service.Common.DT_GetStatusDatesRequest req) {
            return base.Channel.DTGetStatusDates(req);
        }
        
        public LP2.Service.Common.DT_SubmitLoanResponse DTSubmitLoan(LP2.Service.Common.DT_SubmitLoanRequest req) {
            return base.Channel.DTSubmitLoan(req);
        }
        
        public LP2.Service.Common.MailChimp_Response MailChimp_SyncNow(int BranchId) {
            return base.Channel.MailChimp_SyncNow(BranchId);
        }
        
        public LP2.Service.Common.MailChimp_Response MailChimp_SubscribeBatch(int[] ContactIds, string MailChimpListId) {
            return base.Channel.MailChimp_SubscribeBatch(ContactIds, MailChimpListId);
        }
        
        public LP2.Service.Common.MailChimp_Response MailChimp_Subscribe(int ContactId, string MailChimpListId) {
            return base.Channel.MailChimp_Subscribe(ContactId, MailChimpListId);
        }
        
        public LP2.Service.Common.MailChimp_Response MailChimp_UnsubscribeBatch(int[] ContactIds, string MailChimpListId) {
            return base.Channel.MailChimp_UnsubscribeBatch(ContactIds, MailChimpListId);
        }
        
        public LP2.Service.Common.MailChimp_Response MailChimp_Unsubscribe(int ContactId, string MailChimpListId) {
            return base.Channel.MailChimp_Unsubscribe(ContactId, MailChimpListId);
        }
        
        public void ScheduleMailChimp() {
            base.Channel.ScheduleMailChimp();
        }
        
        public string GetVersion() {
            return base.Channel.GetVersion();
        }
        
        public LP2.Service.Common.StartUserImportResponse StartUserImportService(LP2.Service.Common.StartUserImportRequest req) {
            return base.Channel.StartUserImportService(req);
        }
        
        public LP2.Service.Common.StopUserImportResponse StopUserImportService(LP2.Service.Common.StopUserImportRequest req) {
            return base.Channel.StopUserImportService(req);
        }
        
        public LP2.Service.Common.ImportADUsersResponse ImportADUsers(LP2.Service.Common.ImportADUsersRequest req) {
            return base.Channel.ImportADUsers(req);
        }
        
        public LP2.Service.Common.UpdateADUserResponse UpdateADUser(LP2.Service.Common.UpdateADUserRequest req) {
            return base.Channel.UpdateADUser(req);
        }
        
        public LP2.Service.Common.GetPointMgrStatusResponse GetPointManagerStatus(LP2.Service.Common.GetPointMgrStatusRequest req) {
            return base.Channel.GetPointManagerStatus(req);
        }
        
        public LP2.Service.Common.StartPointImportResponse StartPointImportService(LP2.Service.Common.StartPointImportRequest req) {
            return base.Channel.StartPointImportService(req);
        }
        
        public LP2.Service.Common.StopPointImportResponse StopPointImportService(LP2.Service.Common.StopPointImportRequest req) {
            return base.Channel.StopPointImportService(req);
        }
        
        public LP2.Service.Common.ImportAllLoansResponse ImportAllLoans(LP2.Service.Common.ImportAllLoansRequest req) {
            return base.Channel.ImportAllLoans(req);
        }
        
        public LP2.Service.Common.ImportLoansResponse ImportLoans(LP2.Service.Common.ImportLoansRequest req) {
            return base.Channel.ImportLoans(req);
        }
        
        public LP2.Service.Common.ImportLoanRepNamesResponse ImportLoanRepNames(LP2.Service.Common.ImportLoanRepNamesRequest req) {
            return base.Channel.ImportLoanRepNames(req);
        }
        
        public LP2.Service.Common.ImportCardexResponse ImportCardex(LP2.Service.Common.ImportCardexRequest req) {
            return base.Channel.ImportCardex(req);
        }
        
        public LP2.Service.Common.GetPointFileResponse GetPointFile(LP2.Service.Common.GetPointFileRequest req) {
            return base.Channel.GetPointFile(req);
        }
        
        public LP2.Service.Common.MoveFileResponse MoveFile(LP2.Service.Common.MoveFileRequest req) {
            return base.Channel.MoveFile(req);
        }
        
        public LP2.Service.Common.DisposeLoanResponse DisposeLoan(LP2.Service.Common.DisposeLoanRequest req) {
            return base.Channel.DisposeLoan(req);
        }
        
        public LP2.Service.Common.ExtendRateLockResponse ExtendRateLock(LP2.Service.Common.ExtendRateLockRequest req) {
            return base.Channel.ExtendRateLock(req);
        }
        
        public LP2.Service.Common.UpdateEstCloseDateResponse UpdateEstCloseDate(LP2.Service.Common.UpdateEstCloseDateRequest req) {
            return base.Channel.UpdateEstCloseDate(req);
        }
        
        public LP2.Service.Common.UpdateStageResponse UpdateStage(LP2.Service.Common.UpdateStageRequest req) {
            return base.Channel.UpdateStage(req);
        }
        
        public LP2.Service.Common.ReassignLoanResponse ReassignLoan(LP2.Service.Common.ReassignLoanRequest req) {
            return base.Channel.ReassignLoan(req);
        }
        
        public LP2.Service.Common.ReassignContactResponse ReassignContact(LP2.Service.Common.ReassignContactRequest req) {
            return base.Channel.ReassignContact(req);
        }
        
        public LP2.Service.Common.AddNoteResponse AddNote(LP2.Service.Common.AddNoteRequest req) {
            return base.Channel.AddNote(req);
        }
        
        public LP2.Service.Common.UpdateBorrowerResponse UpdateBorrower(LP2.Service.Common.UpdateBorrowerRequest req) {
            return base.Channel.UpdateBorrower(req);
        }
        
        public LP2.Service.Common.UpdateLoanInfoResponse UpdateLoanInfo(LP2.Service.Common.UpdateLoanInfoRequest req) {
            return base.Channel.UpdateLoanInfo(req);
        }
        
        public LP2.Service.Common.ConvertToLeadResponse ConvertToLead(LP2.Service.Common.ConvertToLeadRequest req) {
            return base.Channel.ConvertToLead(req);
        }
        
        public LP2.Service.Common.DisposeLeadResponse DisposeLead(LP2.Service.Common.DisposeLeadRequest req) {
            return base.Channel.DisposeLead(req);
        }
        
        public LP2.Service.Common.GenerateWorkflowResponse GenerateWorkflow(LP2.Service.Common.GenerateWorkflowRequest req) {
            return base.Channel.GenerateWorkflow(req);
        }
        
        public LP2.Service.Common.CalculateDueDatesResponse CalculateDueDates(LP2.Service.Common.CalculateDueDatesRequest req) {
            return base.Channel.CalculateDueDates(req);
        }
        
        public LP2.Service.Common.SendEmailResponse SendEmail(LP2.Service.Common.SendEmailRequest req) {
            return base.Channel.SendEmail(req);
        }
        
        public LP2.Service.Common.EmailPreviewResponse PreviewEmail(LP2.Service.Common.EmailPreviewRequest req) {
            return base.Channel.PreviewEmail(req);
        }
        
        public bool ProcessEmailQue() {
            return base.Channel.ProcessEmailQue();
        }
        
        public void ProcessLoanRules() {
            base.Channel.ProcessLoanRules();
        }
        
        public bool AcknowledgeAlert(int currentLoanAlertId, int userId) {
            return base.Channel.AcknowledgeAlert(currentLoanAlertId, userId);
        }
    }
}