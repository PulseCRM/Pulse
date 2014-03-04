using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading;
using System.Text;
using EmailManager;
using MailChimpMgr;
using MarketingManager;
using LP2.Service.Common;
using LP2.Service;
using DataTracManager;
using System.Data;
using Common;

namespace LP2Service
{
    public class LP2Service : ILP2Service
    {
        Framework.PNTLib pntLib = null;
        DataAccess.DataAccess da = null;
        LP2.Service.UserManager um = null;
        PointMgr pm = null;
        EmailManager.EmailMgr em = null;
        MarketingManager.MkgMgr mm = null;
        private RuleManager.RuleMgr rm = null;
        WorkflowManager wm = null;
        MailChimpMgr.MailChimpManager mailChimpManager = new MailChimpManager();
        public LP2Service()
        {
            if (da == null)
                da = new DataAccess.DataAccess();
            if (um == null)
                um = UserManager.Instance;
            if (pm == null)
                pm = PointMgr.Instance;
            if (wm == null)
                wm = WorkflowManager.Instance;
            if (em == null)
                em = EmailManager.EmailMgr.Instance;
            if (mm == null)
                mm = MarketingManager.MkgMgr.Instance;
            if (rm == null)
                rm = RuleManager.RuleMgr.Instance;
            if (pntLib == null)
                pntLib = new Framework.PNTLib();
        }
        public string GetVersion()
        {
            Assembly a = Assembly.GetEntryAssembly();
            string versionInfo = string.Empty;
            string company = string.Empty;
            string product = string.Empty;
            if (a != null)
            {
                foreach (object attr in a.GetCustomAttributes(false))
                {
                    string typeName = attr.GetType().ToString();
                    if (typeName == "System.Reflection.AssemblyCompanyAttribute")
                        company = ((AssemblyCompanyAttribute)attr).Company.ToString();
                    if (typeName == "System.Reflection.AssemblyProductAttribute")
                        product = ((AssemblyProductAttribute)attr).Product.ToString();
                }
                //versionInfo = company + ", " + product + " Version " + a.GetName().Version.ToString();
                versionInfo = " Version  9/12/2012  5:46 PM";
            }
            return versionInfo;
        }

        #region User Manager Methods

        public StartUserImportResponse StartUserImportService(StartUserImportRequest req)
        {
            string err = "";
            bool status = false;
            StartUserImportResponse resp = new StartUserImportResponse();
            resp.hdr = new RespHdr();
            try
            {
                bool thread_status = false;
                thread_status = LPService.SAUIThread.IsAlive;

                if (thread_status)
                {
                    LPService.SAUIThread.Abort();
                }

                LPService.SAUIDelegate = new ThreadStart(LPService.Scheduled_AD_User_Imports);
                LPService.SAUIThread = new Thread(LPService.SAUIDelegate);
                LPService.SAUIThread.Start();
                status = true;
                err = "";
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Successfully started Scheduled User Import Service.", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                status = false;
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
            }
            resp.hdr.Successful = status;
            resp.hdr.StatusInfo = err;
            return resp;
        }

        public StopUserImportResponse StopUserImportService(StopUserImportRequest req)
        {
            StopUserImportResponse resp = new StopUserImportResponse();
            resp.hdr = new RespHdr();
            bool status = false;
            string err = "";
            try
            {
                bool thread_status = false;

                thread_status = LPService.SAUIThread.IsAlive;
                if (thread_status)
                {
                    LPService.SAUIThread.Abort();
                }
                status = true;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Successfully stopped Scheduled User Import Service.", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                status = false;
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
            }
            resp.hdr.Successful = status;
            resp.hdr.StatusInfo = err;
            return resp;
        }

        public ImportADUsersResponse ImportADUsers(ImportADUsersRequest req)
        {
            int reqId = 0;
            int userid = 0;
            string cmd = "ImportADUsers";
            string err = "";
            string AD_OU_Filter = "";
            bool status = false;

            ImportADUsersResponse resp = new ImportADUsersResponse();
            resp.hdr = new RespHdr();
            if ((req == null) || (req.hdr == null) || (req.AD_OU_Filter == null) || (req.AD_OU_Filter == string.Empty))
            {
                err = MethodBase.GetCurrentMethod() + ", request or part of the request is empty.";
                return resp;
            }
            userid = req.hdr.UserId;
            AD_OU_Filter = req.AD_OU_Filter;

            try
            {
                da.AddRequestQueue(userid, cmd, ref reqId, ref err);
                status = um.ImportUsers(AD_OU_Filter, userid, reqId, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
            }
            finally
            {
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }

        public UpdateADUserResponse UpdateADUser(UpdateADUserRequest req)
        {
            int reqId = 0;
            int userid = 0;
            string err = "";
            bool status = false;

            UpdateADUserResponse resp = new UpdateADUserResponse();
            resp.hdr = new RespHdr();

            if ((req == null) || (req.hdr == null) || (req.AD_OU_Filter == null) || (req.AD_OU_Filter == string.Empty) || (req.AD_User == null))
            {
                status = false;
                err = MethodBase.GetCurrentMethod() + ", request or part of the request is empty.";
            }
            else if (string.IsNullOrEmpty(req.AD_User.Username))
            {
                status = false;
                err = MethodBase.GetCurrentMethod() + ", request/part of the request is empty.";
            }
            else
            {
                User user = req.AD_User;
                userid = req.hdr.UserId;
                try
                {
                    da.AddRequestQueue(userid, req.Command.ToString(), ref reqId, ref err);
                    switch (req.Command)
                    {
                        case UserMgrCommandType.CreateUser:
                            if ((user.Firstname == string.Empty) || (user.Lastname == string.Empty) || (user.Password == string.Empty))
                            {
                                status = false;
                                err = "Invalid request, missing user's firstname, lastname or password.";
                            }
                            else
                            {
                                status = um.CreateUser(user, userid, reqId, ref err);
                            }
                            break;
                        case UserMgrCommandType.ChangePassword:
                            if (user.Password == string.Empty)
                            {
                                status = false;
                                err = "Invalid request, missing user's password.";
                            }
                            else
                            {
                                status = um.ChangeUserPassword(user.Username, user.Password, userid, reqId, ref err);
                            }
                            break;
                        case UserMgrCommandType.DeleteUser:
                            status = um.DeleteUser(user, userid, reqId, ref err);
                            break;
                        case UserMgrCommandType.DisableUser:
                            status = um.DisableUser(user, userid, reqId, ref err);
                            break;
                        case UserMgrCommandType.EnableUser:
                            status = um.EnableUser(user, userid, reqId, ref err);
                            break;
                        case UserMgrCommandType.UpdateUser:
                            status = um.UpdateUser(user, userid, reqId, ref err);
                            break;
                        default:
                            status = false;
                            err = MethodBase.GetCurrentMethod() + ", received invalid command=" + req.Command.ToString();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    status = false;
                    err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
                }
            }

            resp.hdr.Successful = status;
            resp.hdr.StatusInfo = err;
            resp.hdr.RequestId = reqId;
            return resp;
        }
        #endregion
        #region Point Manager Methods

        public GetPointMgrStatusResponse GetPointManagerStatus(GetPointMgrStatusRequest req)
        {
            GetPointMgrStatusResponse resp = new GetPointMgrStatusResponse();
            resp.hdr = new RespHdr();
            resp.Running = (LPService.SPIThread == null)?false : LPService.SPIThread.IsAlive;
            return resp;
        }

        public StartPointImportResponse StartPointImportService(StartPointImportRequest req)
        {
            StartPointImportResponse resp = new StartPointImportResponse();
            resp.hdr = new RespHdr();
            bool status = false;
            bool thread_status = false;
            string err = "";

            try
            {
                thread_status = LPService.SPIThread.IsAlive;
                if (thread_status)
                {
                    LPService.SPIThread.Abort();
                }

                LPService.SPIDelegate = new ThreadStart(LPService.Scheduled_Point_Imports);
                LPService.SPIThread = new Thread(LPService.SPIDelegate);
                LPService.SPIThread.Start();
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Successfully started Scheduled Point Import Service.", EventLogEntryType.Information);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
            }
            resp.hdr.Successful = status;
            resp.hdr.RequestId = 0;
            resp.hdr.StatusInfo = err;
            return resp;
        }

        public StopPointImportResponse StopPointImportService(StopPointImportRequest req)
        {
            StopPointImportResponse resp = new StopPointImportResponse();
            resp.hdr = new RespHdr();
            bool status = false;
            bool thread_status = false;
            string err = "";

            try
            {
                thread_status = LPService.SPIThread.IsAlive;
                if (thread_status)
                {
                    LPService.SPIThread.Abort();
                }
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Successfully stopped Scheduled Point Import Service.", EventLogEntryType.Information);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                err = MethodBase.GetCurrentMethod() + ", Exception: " + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
            }
            resp.hdr.Successful = status;
            resp.hdr.RequestId = 0;
            resp.hdr.StatusInfo = "";
            return resp;
        }
        #region Import Methods
        public ImportAllLoansResponse ImportAllLoans(ImportAllLoansRequest req)
        {
            ImportAllLoansResponse resp = new ImportAllLoansResponse();
            resp.hdr = new RespHdr();
            bool status = false;
            string err = "";
            int reqId = 0;

            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }

            if ((req.PointFolders == null) || (req.PointFolders.Length <= 0))
            {
                err = "No Point Folder specified in the request.";
                return resp;
            }

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "ImportLoans", ref reqId, ref err);
                PointMgrEvent e = new PointMgrEvent(reqId, PointMgrCommandType.ImportAllLoans, 0, req);
                status = pm.ProcessRequest(e, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to import all loans from folder " + req.PointFolders[0] + ", Exception:" + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
            }
            finally
            {
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.RequestId = reqId;
            }
            return resp;
        }

        public ImportLoansResponse ImportLoans(ImportLoansRequest req)
        {
            ImportLoansResponse resp = new ImportLoansResponse();
            resp.hdr = new RespHdr();
            bool status = false;
            string err = "";
            int reqId = 0;

            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }

            if ((req.FileIds == null) || (req.FileIds.Length <= 0))
            {
                err = "No Point File Ids specified in the request.";
                return resp;
            }
            //EventLog.WriteEntry(InfoHubEventLog.LogSource, MethodBase.GetCurrentMethod() + ", received Import Loans request from userId=" + req.hdr.UserId + ", FileId=" + req.FileIds[0], EventLogEntryType.Information);

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "ImportLoans", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.ImportLoans, 0, req);
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to import loans, Exception: " + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
            }
            finally
            {
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.RequestId = reqId;
            }
            return resp;
        }

        public ImportLoanRepNamesResponse ImportLoanRepNames(ImportLoanRepNamesRequest req)
        {
            ImportLoanRepNamesResponse resp = new ImportLoanRepNamesResponse();
            resp.hdr = new RespHdr();
            bool status = false;
            string err = "";
            int reqId = 0;

            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }

            if ((req.PointFolders == null) || (req.PointFolders.Length <= 0))
            {
                err = "No Point Folder specified in the request.";
                return resp;
            }
            try
            {
                da.AddRequestQueue(req.hdr.UserId, "ImportLoanRepNames", ref reqId, ref err);
                PointMgrEvent e = new PointMgrEvent(reqId, PointMgrCommandType.ImportLONames, 0, req);
                status = pm.ProcessRequest(e, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to import Loan Rep Names from folders: ";
                foreach (string str in req.PointFolders)
                {
                    resp.hdr.StatusInfo += str + ",";
                }
                err += ", Exception:" + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
            }
            finally
            {
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }

        public ImportCardexResponse ImportCardex(ImportCardexRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            ImportCardexResponse resp = new ImportCardexResponse();
            resp.hdr = new RespHdr();

            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }

            if ((req.CardexFile == null) || (req.CardexFile.Length <= 0))
            {
                err = "No Cardex File specified in the request.";
                return resp;
            }
            if (!req.CardexFile.ToLower().Trim().EndsWith(".mdb"))
            {
                err = "Invalid Cardex file name: " + req.CardexFile;
                return resp;
            }
            try
            {
                da.AddRequestQueue(req.hdr.UserId, "ImportCardex", ref reqId, ref err);
                string[] file = { req.CardexFile };

                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.ImportCardex, 0, req);
                pm.ProcessRequest(pe, ref err);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to import Cardex from file: " + req.CardexFile;
                err += "Exception:" + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
            }
            finally
            {
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }

            return resp;
        }
        #endregion
        #region Update Methods

        public GetPointFileInfoResp GetPointFileInfo(GetPointFileInfoReq req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            GetPointFileInfoResp resp = new GetPointFileInfoResp();
            if (req == null)
            {
                err = "Cannot get point file, Request is empty.";
                return resp;
            }
            if (req.FileId <= 0)
            {
                err = "Cannot get point file,  invalid File Id, " + req.FileId;
                return resp;
            }

            try
            {
                resp = pm.GetPointFileInfo(req);
                status = resp.hdr.Successful;
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to get point file,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }

            return resp;
        }

        public ExtendRateLockResponse ExtendRateLock(ExtendRateLockRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            ExtendRateLockResponse resp = new ExtendRateLockResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "Cannot extend Rate Lock, Request is empty.";
                return resp;
            }
            if (req.FileId <= 0)
            {
                err = "Cannot extend Rate Lock,  invalid File Id, " + req.FileId;
                return resp;
            }
            if (req.DaysExtended <= 0)
            {
                err = "Cannot extend Rate Lock, Invalid Days Extended " + req.DaysExtended + " specified";
                return resp;
            }

            if ((req.NewDate == null) || (req.NewDate == DateTime.MinValue))
            {
                err = "New Ratelock Expiration date cannot be empty.";
                return resp;
            }
            try
            {
                da.AddRequestQueue(req.hdr.UserId, "Extend RateLock", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.ExtendRateLock, req.FileId, req);
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to extend Ratelock,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }
        public UpdateEstCloseDateResponse UpdateEstCloseDate(UpdateEstCloseDateRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            UpdateEstCloseDateResponse resp = new UpdateEstCloseDateResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "Cannot update Est Close Date, Request is empty.";
                return resp;
            }
            if (req.FileId <= 0)
            {
                err = "Cannot update Est Close Date, invalid File Id, " + req.FileId;
                return resp;
            }

            if ((req.NewDate == null) || (req.NewDate == DateTime.MinValue))
            {
                err = "Cannot update Est Close Date, new Est Close Date cannot be empty.";
                return resp;
            }
            try
            {
                da.AddRequestQueue(req.hdr.UserId, "Update Est Close Date", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.UpdateEstCloseDate, req.FileId, req);
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to update Est Close Date,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }

        public UpdateStageResponse UpdateStage(UpdateStageRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            UpdateStageResponse resp = new UpdateStageResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "Cannot update stage, Request is empty.";
                return resp;
            }
            if (req.FileId <= 0)
            {
                err = "Cannot update stage, invalid File Id, " + req.FileId;
                return resp;
            }

            if ((req.StageList == null) || (req.StageList.Count <= 0))
            {
                err = "Cannot update stage, missing stage information.";
                return resp;
            }

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "Update Stage", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.UpdateStage, req.FileId, req);
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to update stage,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(MethodBase.GetCurrentMethod() + ", " + err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, MethodBase.GetCurrentMethod() + ", " + err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }
        public ReassignLoanResponse ReassignLoan(ReassignLoanRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            ReassignLoanResponse resp = new ReassignLoanResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "Cannot reassign loan, request is empty.";
                return resp;
            }
            if (req.reassignUsers == null || req.reassignUsers.Count <= 0)
            {
                err = "No reassignUsers information in the request.";
                return resp;
            }

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "Reassign Loan", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.ReassignLoan, req);
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to update Reassign Loan,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }

        public ReassignContactResponse ReassignContact(ReassignContactRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            ReassignContactResponse resp = new ReassignContactResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "Cannot reassign contact, request is empty.";
                return resp;
            }

            if (req.reassignContacts == null || req.reassignContacts.Count <= 0)
            {
                err = "No reassignContacts information available ";
                return resp;
            }

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "Reassign Contact", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.ReassignContact, req);
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Cannot reassign contact,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }
        public AddNoteResponse AddNote(AddNoteRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            AddNoteResponse resp = new AddNoteResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "Cannot add note, Request is empty.";
                return resp;
            }
            if (req.FileId <= 0)
            {
                err = "Cannot add note,  invalid File Id, " + req.FileId;
                return resp;
            }
            if (req.Sender == String.Empty)
            {
                err = "Cannot add note, Sender is not  specified.";
                return resp;
            }

            if ((req.Created == null) || (req.Created == DateTime.MinValue))
            {
                err = "Cannot add note, Creation Date Time is not specified.";
                return resp;
            }
            if (req.Note == String.Empty)
            {
                err = "Cannot add note, Note is missing.";
                return resp;
            }
            try
            {
                da.AddRequestQueue(req.hdr.UserId, "Add Note", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.AddNote, req.FileId, req);
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to Add Note,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }
        #endregion
        public GetPointFileResponse GetPointFile(GetPointFileRequest req)
        {
            throw new NotImplementedException();
            //GetPointFileResponse resp = new GetPointFileResponse();
            //resp.hdr = new RespHdr();
            //resp.hdr.Successful = true;
            //return resp;
        }
        public MoveFileResponse MoveFile(MoveFileRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            MoveFileResponse resp = new MoveFileResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }
            if (req.FileId <= 0)
            {
                err = "Invalid File Id, " + req.FileId;
                return resp;
            }

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "Move Point File", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.MovePointFile, req.FileId, req);
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to move Point file ,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }

        public DisposeLoanResponse DisposeLoan(DisposeLoanRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            DisposeLoanResponse resp = new DisposeLoanResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }
            if (req.FileId <= 0)
            {
                err = "Invalid File Id, " + req.FileId;
                return resp;
            }

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "Dispose Loan", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.DisposeLoan, req.FileId, req);
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to dispose loan, Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }
        public DisposeLeadResponse DisposeLead(DisposeLeadRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            DisposeLeadResponse resp = new DisposeLeadResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "DisposeLeadRequest is empty.";
                return resp;
            }
            if (req.FileId <= 0)
            {
                err = "DisposeLead, Invalid File Id, " + req.FileId;
                return resp;
            }

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "Dispose Lead", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.DisposeLead, req.FileId, req);
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to dispose of the lead, Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }
        public UpdateBorrowerResponse UpdateBorrower(UpdateBorrowerRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            UpdateBorrowerResponse resp = new UpdateBorrowerResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }
            if (req.ContactId <= 0)
            {
                err = "Invalid Contact Id, " + req.ContactId;
                return resp;
            }

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "Update Borrower", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.UpdateBorrower, req.ContactId, req);
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to Update Borrower, Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }

        public UpdateLoanInfoResponse UpdateLoanInfo(UpdateLoanInfoRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            UpdateLoanInfoResponse resp = new UpdateLoanInfoResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "Update LoanInfo", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.UpdateLoanInfo, req.FileId, req);
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to Update LoanInfo, Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }

        public ConvertToLeadResponse ConvertToLead(ConvertToLeadRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            ConvertToLeadResponse resp = new ConvertToLeadResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }
            if (req.FileId <= 0)
            {
                err = "Invalid File Id, " + req.FileId;
                return resp;
            }

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "ConvertToLead ", ref reqId, ref err);
                status = pm.ConvertToLead(req, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to convert to lead, Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }
        #endregion

        #region Workflow Manager Methods
        public GenerateWorkflowResponse GenerateWorkflow(GenerateWorkflowRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            GenerateWorkflowResponse resp = new GenerateWorkflowResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }
            if (req.FileId <= 0)
            {
                err = "Invalid File Id, " + req.FileId;
                return resp;
            }
            if (req.WorkflowTemplId <= 0)
            {
                err = "Invalid Workflow Template Id, " + req.WorkflowTemplId;
                return resp;
            }

            try
            {
                status = wm.GenerateWorkflow(req, ref err);
                string msg = string.Format("FileId = {0},UserId={1}, WorkflowTemplate={2}", req.FileId, req.hdr.UserId, req.WorkflowTemplId);
                da.AddRequestQueue(req.hdr.UserId, "GenerateWorkflow - Website", msg, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to generate workflow,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }
        public CalculateDueDatesResponse CalculateDueDates(CalculateDueDatesRequest req)
        {
            string err = "";
            int reqId = 0;
            bool status = false;

            CalculateDueDatesResponse resp = new CalculateDueDatesResponse();
            resp.hdr = new RespHdr();
            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }
            if (req.FileId <= 0)
            {
                err = "Invalid File Id, " + req.FileId;
                return resp;
            }
            if ((req.NewEstCloseDate == null) || (req.NewEstCloseDate == DateTime.MinValue))
            {
                err = "Invalid Est Close Date ";
                return resp;
            }

            try
            {
                da.AddRequestQueue(req.UserId, "Calculate Due Dates", ref reqId, ref err);
                status = wm.CalculateDueDates(req, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to calculate due dates,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }

        #endregion

        #region Email Manager Methods

        public SendEmailResponse SendEmail(SendEmailRequest req)
        {
            string err = "";
            bool status = false;
            var resp = new SendEmailResponse();
            resp.resp = new RespHdr();
            try
            {
                resp.resp = new RespHdr();
                if (req == null)
                {
                    err = "Request is empty.";
                    return resp;
                }

                if (req.hdr == null)
                {
                    err = "Request Hdr is null.";
                    return resp;
                }

                if (req.EmailTemplIds != null && req.EmailTemplIds.Length > 0)
                {
                    foreach (int emailTemplId in req.EmailTemplIds)
                    {
                        SendEmailRequest lRequest = req.Clone() as SendEmailRequest;
                        if (lRequest != null)
                        {
                            lRequest.EmailTemplId = emailTemplId;
                            req.EmailTemplId = emailTemplId;
                            WaitCallback callBack = em.SendEmail;
                            ThreadPool.QueueUserWorkItem(callBack, lRequest);
                        }
                    }
                }
                else
                {
                    status = em.SendEmail(req);
                }

                resp.resp.Successful = true;
                resp.resp.StatusInfo = err;
                resp.resp.RequestId = default(int);
                return resp;
            }
            catch (Exception ex)
            {
                err = "SendEmail, Exception:" + ex.Message;
                resp.resp.Successful = false;
                resp.resp.StatusInfo = err;
                resp.resp.RequestId = default(int);
                return resp;
            }

        }

        public EmailPreviewResponse PreviewEmail(EmailPreviewRequest req)
        {
            string err = "";
            bool logErr = false;
            bool fatal = false;
            EmailPreviewResponse resp = new EmailPreviewResponse();
            bool status = false;
            try
            {
                resp.resp = new RespHdr();
                if (req == null)
                {
                    err = "Request is empty.";
                    return resp;
                }
                //if (req.FileId <= 0)
                //{
                //    err = "Invalid File Id, " + req.FileId;
                //    return resp;
                //}
                //if (req.EmailTemplId <= 0)
                //{
                //    err = "Invalid Email Template Id, " + req.EmailTemplId;
                //    return resp;
                //}
                //if (req.hdr == null)
                //{
                //    err = "req hdr is empty.";
                //    return resp;
                //}
                resp.resp = null;  // to avoid memory leaks
                resp = null;       // to avoid memory leaks
                resp = em.PreviewEmail(req);
                if (resp.resp.Successful)
                    status = true;
                else
                    err = resp.resp.StatusInfo;
            }
            catch (Exception ex)
            {
                err = "PreviewEmail, Exception:" + ex.Message;
                fatal = true;
                logErr = true;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    if (fatal)
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
                    else
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.resp.Successful = status;
                resp.resp.StatusInfo = err;
            }
            return resp;
        }

        public bool ProcessEmailQue()
        {
            return em.ProcessEmailQue();
        }

        public string ReplyToMessage(ReplyToEmailRequest replyToEmailRequest)
        {
            SendStatus status = new SendStatus();
            status.Message = " ";

            try
            {
                status = em.ReplyToMessage(replyToEmailRequest);

                if (status != null)
                {
                    if (status.Status == true)
                        return string.Empty;

                    if ((status.Message != null) && (status.Message != " "))
                    {
                        return status.Message;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                status.Status = false;
                status.Message = "Failed to ReplyToMessage, Exception:" + ex.Message;
                return status.Message;
            }
        }

        #region RuleManager

        /// <summary>
        /// Processes the loan rules.
        /// </summary>
        public void ProcessLoanRules()
        {
            rm.ProcessLoanRules();
        }

        /// <summary>
        /// Acknowledges the alert.
        /// </summary>
        /// <param name="currentLoanAlertId">The current loan alert id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public bool AcknowledgeAlert(int currentLoanAlertId, int userId)
        {
            return rm.AcknowledgeAlert(currentLoanAlertId, userId);
        }

        #endregion

        #region Implementation of ILP2Service

        /// <summary>
        /// Gets the replies.
        /// </summary>
        public int GetReplies()
        {
            em.GetReplies();
            return 0;
        }

        #endregion



        #endregion

        #region Marketing Manager Methods

        public UpdateCompanyResponse UpdateCompany(UpdateCompanyRequest req)
        {
            string err = "";
            bool status = true;
            UpdateCompanyResponse resp = new UpdateCompanyResponse();
            resp.hdr = new RespHdr();
            try
            {
                status = mm.UpdateCompany(ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to update Company, Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;

        }

        public UpdateRegionResponse UpdateRegion(UpdateRegionRequest req)
        {
            string err = "";
            bool status = true;
            UpdateRegionResponse resp = new UpdateRegionResponse();
            resp.hdr = new RespHdr();
            try
            {
                //resp = mm.UpdateRegion(req, ref err);
                status = resp.hdr.Successful;
                err = resp.hdr.StatusInfo;
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to update Region, Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;

        }

        public UpdateBranchResponse UpdateBranch(UpdateBranchRequest req)
        {
            string err = "";
            bool status = true;
            UpdateBranchResponse resp = new UpdateBranchResponse();
            resp.hdr = new RespHdr();
            try
            {
                status = mm.UpdateBranch(req.BranchId, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to update Branch, Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;

        }

        public UpdateUserResponse UpdateUser(UpdateUserRequest req)
        {
            string err = "";
            bool status = true;
            UpdateUserResponse resp = new UpdateUserResponse();
            resp.hdr = new RespHdr();
            try
            {
                status = mm.UpdateUser(req.UserId, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to update User, Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;

        }

        public UpdateProspectResponse UpdateProspect(UpdateProspectRequest req)
        {
            string err = "";
            bool status = true;
            UpdateProspectResponse resp = new UpdateProspectResponse();
            resp.hdr = new RespHdr();
            try
            {
                status = mm.UpdateProspect(req.FileId, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to update Prospect, Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;

        }

        public StartCampaignResponse StartCampaign(StartCampaignRequest req)
        {
            string err = "";
            bool status = true;
            StartCampaignResponse resp = new StartCampaignResponse();
            resp.hdr = new RespHdr();
            try
            {
                resp = mm.StartCampaign(req, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to start Marketing Campaign,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;

        }

        public bool SyncMarketingData(ref string err)
        {
            err = "";
            bool status = true;
            StartCampaignResponse resp = new StartCampaignResponse();
            resp.hdr = new RespHdr();
            try
            {
                status = mm.ProcessSyncMarketingData(ref err);

                return status;
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to start Sync Marketing Data,  Exception:" + ex.Message;
                return status;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        public RemoveCampaignResponse RemoveCampaign(RemoveCampaignRequest req)
        {
            string err = "";
            bool status = true;
            RemoveCampaignResponse resp = new RemoveCampaignResponse();
            resp.hdr = new RespHdr();
            try
            {
                status = mm.RemoveCampaign(req, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to remove Marketing Campaign,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;

        }

        public CompleteCampaignEventResponse CompleteCampaignEvent(CompleteCampaignEventRequest req)
        {
            string err = "";
            bool status = true;
            CompleteCampaignEventResponse resp = new CompleteCampaignEventResponse();
            resp.hdr = new RespHdr();
            try
            {
                resp = mm.CompleteCampaignEvent(req, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to remove Marketing Campaign,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;

        }

        public GetUserAccountBalanceResponse GetUserAccountBalance(GetUserAccountBalanceRequest req)
        {
            string err = "";
            bool status = true;
            GetUserAccountBalanceResponse resp = new GetUserAccountBalanceResponse();
            resp.hdr = new RespHdr();
            try
            {
                resp = mm.GetUserAccountBalance(req, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to remove Marketing Campaign,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;

        }

        public UpdateCreditCardResponse UpdateCreditCard(UpdateCreditCardRequest req)
        {
            string err = "";
            bool status = true;
            UpdateCreditCardResponse resp = new UpdateCreditCardResponse();
            resp.hdr = new RespHdr();
            try
            {
                resp = mm.UpdateCreditCard(req, ref err);
                status = resp.hdr.Successful;
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to Update CreditCard,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;
        }

        public GetCreditCardResponse GetCreditCard(ref GetCreditCardRequest req)
        {
            string err = "";
            bool status = true;
            GetCreditCardResponse resp = new GetCreditCardResponse();
            resp.hdr = new RespHdr();
            try
            {
                resp = mm.GetCreditCard(req, ref err);
                status = resp.hdr.Successful;
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to Get CreditCard,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;
        }

        public AddToAccountResponse AddToAccount(AddToAccountRequest req)
        {
            string err = "";
            bool status = true;
            AddToAccountResponse resp = new AddToAccountResponse();
            resp.hdr = new RespHdr();
            try
            {
                resp = mm.AddToAccount(req, ref err);
                status = resp.hdr.Successful;
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to Add to Account,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;
        }

        public ReassignProspectResponse ReassignProspect(ReassignProspectRequest req)
        {
            string err = "";
            bool status = true;
            ReassignProspectResponse resp = new ReassignProspectResponse();
            resp.hdr = new RespHdr();

            List<int> FileId_List = new List<int>();

            try
            {
                if (req.UserId != null)
                {
                    FileId_List = da.GetFileId_FromUserId(req.UserId, ref err);
                    req.FileId = FileId_List.ToArray();
                    status = mm.ReassignProspect(req, ref err);
                }
                else
                {
                    if (req.ContactId != null)
                    {
                        FileId_List = da.GetFileId_FromContactId(req.ContactId, ref err);
                        req.FileId = FileId_List.ToArray();
                        status = mm.ReassignProspect(req, ref err);
                    }
                    else
                    {
                        if (req.FileId != null)
                        {
                            status = mm.ReassignProspect(req, ref err);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to Reassign Prospect,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;
        }
        #endregion
        #region Report Manager methods
        public GenerateReportResponse GenerateReport(GenerateReportRequest req)
        {
            GenerateReportResponse resp = null;
            string err = "";
            bool status = false;
            try
            {
                ReportManager rm = ReportManager.Instance;
                resp = rm.GenerateReport(req);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to generate report,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                if (resp == null)
                {
                    resp = new GenerateReportResponse();
                    resp.hdr = new RespHdr();
                    resp.hdr.Successful = status;
                    resp.hdr.StatusInfo = err;
                }
            }
            return resp;
        }
        public SendLSRResponse SendLSR(SendLSRRequest req)
        {
            SendLSRResponse resp = null;
            string err = "";
            bool status = false;
            try
            {
                ReportManager rm1 = ReportManager.Instance;
                resp = rm1.SendLSR(req);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to SendLSR,  Exception:" + ex.Message;
            }
            finally
            {
                if (status == false)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
                if (resp == null)
                {
                    resp = new SendLSRResponse();
                    resp.hdr = new RespHdr();
                    resp.hdr.Successful = status;
                    resp.hdr.StatusInfo = err;
                }
            }
            return resp;
        }
        #endregion

        #region DataTrac Manger Methods

        /// <summary>
        /// Import Loans from DataTrac to Pulse
        /// neo 2011-06-21
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>

        public DT_ImportLoansResponse DTImportLoans(DT_ImportLoansRequest req)
        {
            DT_ImportLoansResponse DT_ImportLoansResponse1 = new DT_ImportLoansResponse();
            DT_ImportLoansResponse1.hdr = new RespHdr();

            if ((req == null) || (req.hdr.UserId < 0) || (req.FileIds == null))
            {
                DT_ImportLoansResponse1.hdr.Successful = false;
                DT_ImportLoansResponse1.hdr.StatusInfo = "DTImportLoans: DT_ImportLoansRequest is not initialized.";
                return DT_ImportLoansResponse1;
            }

            DataTracMgr DataTracMgr1 = new DataTracMgr();

            string sError = string.Empty;
            List<Common.Table.Loans> LoanList = null;

            try
            {
                string err = "";
                DataAccess.PointConfig pointConfig;
                pointConfig = da.GetPointConfigData(ref err);
                if ((pointConfig == null) ||
                    (pointConfig.MasterSource.Trim().ToLower() != "datatrac"))
                {
                    DT_ImportLoansResponse1.hdr.Successful = false;
                    DT_ImportLoansResponse1.hdr.StatusInfo = "DTImportLoans Error: pointConfig.MasterSource != DataTrac";
                    return DT_ImportLoansResponse1;
                }

                if (req.FileIds.Length == 0)
                {
                    LoanList = DataTracMgr1.GetLoanInfoList("Processing", ref sError);
                }
                else
                {
                    LoanList = DataTracMgr1.GetLoanInfoList(req.FileIds, ref sError);
                }

                if (sError != string.Empty)
                {
                    string sErrorMsg = "Failed to get loan info list from DataTrac Manager, Error: " + sError;
                    DT_ImportLoansResponse1.hdr.Successful = false;
                    DT_ImportLoansResponse1.hdr.StatusInfo = sErrorMsg;
                    return DT_ImportLoansResponse1;
                }
                else
                {
                    if (LoanList == null)
                    {
                        DT_ImportLoansResponse1.hdr.Successful = false;
                        DT_ImportLoansResponse1.hdr.StatusInfo = "none";
                        return DT_ImportLoansResponse1;
                    }
                }
            }
            catch (Exception ex)
            {
                string sErrorMsg = "Failed to get loan info list from DataTrac: " + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Warning);
                DT_ImportLoansResponse1.hdr.Successful = false;
                DT_ImportLoansResponse1.hdr.StatusInfo = sErrorMsg;
                return DT_ImportLoansResponse1;
            }

            try
            {

                foreach (Common.Table.Loans LoanItem in LoanList)
                {
                    string sError_GetBorrowerInfo = string.Empty;
                    Common.Table.Contacts BorrowerInfo = DataTracMgr1.GetBorrowerInfo(LoanItem.FileId, ref sError_GetBorrowerInfo);

                    if (sError_GetBorrowerInfo != string.Empty)
                    {
                        string sErrorMsg = "Failed to get Borrower info from DataTrac: " + sError_GetBorrowerInfo;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Warning);
                        DT_ImportLoansResponse1.hdr.Successful = false;
                        DT_ImportLoansResponse1.hdr.StatusInfo = sErrorMsg;
                        return DT_ImportLoansResponse1;
                    }

                    string sError_GetCoBorrowerInfo = string.Empty;
                    Common.Table.Contacts CoBorrowerInfo = DataTracMgr1.GetCoBorrowerInfo(LoanItem.FileId, ref sError_GetCoBorrowerInfo);

                    if (sError_GetCoBorrowerInfo != string.Empty)
                    {
                        string sErrorMsg = "Failed to get CoBorrower info from DataTrac: " + sError_GetCoBorrowerInfo;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Error);
                        DT_ImportLoansResponse1.hdr.Successful = false;
                        DT_ImportLoansResponse1.hdr.StatusInfo = sErrorMsg;
                        return DT_ImportLoansResponse1;
                    }

                    da.UpdateLoanInfo(LoanItem, BorrowerInfo, CoBorrowerInfo);

                    #region Call UpdateLoanInfo API after update loan field

                    UpdateLoanInfoRequest loanInfoReq = new UpdateLoanInfoRequest();
                    string strErr = "";
                    loanInfoReq.FileId = LoanItem.FileId;
                    loanInfoReq.hdr = new ReqHdr();
                    loanInfoReq.hdr.UserId = req.hdr.UserId;
                    pm.UpdateLoanInfo(loanInfoReq, ref strErr);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                string sErrorMsg = "Failed to update DataTrac loan info to Pulse database: " + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Warning);
                DT_ImportLoansResponse1.hdr.Successful = false;
                DT_ImportLoansResponse1.hdr.StatusInfo = sErrorMsg;
                return DT_ImportLoansResponse1;
            }

            foreach (Common.Table.Loans LoanItem in LoanList)
            {
                string sError1 = string.Empty;
                bool bSuccess = da.Check_SaveRateLockAlert(LoanItem.FileId, ref sError1);

                if (bSuccess == false)
                {
                    string sErrorMsg = "Failed to exec sp CheckRateLockAlert: " + sError1;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Error);
                    DT_ImportLoansResponse1.hdr.Successful = false;
                    DT_ImportLoansResponse1.hdr.StatusInfo = sErrorMsg;
                    return DT_ImportLoansResponse1;
                }
            }

            DT_ImportLoansResponse1.hdr.Successful = true;
            DT_ImportLoansResponse1.hdr.StatusInfo = string.Empty;
            return DT_ImportLoansResponse1;
        }

        public DT_SubmitLoanResponse DTSubmitLoan(DT_SubmitLoanRequest req)
        {
            DT_SubmitLoanResponse response = new DT_SubmitLoanResponse();
            response.hdr = new RespHdr();

            if ((req == null) || (req.hdr.UserId < 1) || (req.FileId < 1))
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = "DTSubmitLoan: DT_SubmitLoanRequest is not initialized.";
                return response;
            }

            if (string.IsNullOrEmpty(req.Loan_Program))
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = "The Loan Program specified in the SubmitLoanRequest is invalid, FileId=< " + req.FileId.ToString() + " >, Loan Program=< >.";
                return response;
            }

            if (string.IsNullOrEmpty(req.Originator_Type))
            {
                response.hdr.Successful = false;
                response.hdr.StatusInfo = "The Originator Type specified in the SubmitLoanRequest is invalid, FileId=< " + req.FileId.ToString() + " >, Originator Type=< >.";
                return response;
            }

            string sError = string.Empty;
            bool isSuccessful = false;

            try
            {
                string err = "";
                DataAccess.PointConfig pointConfig;
                pointConfig = da.GetPointConfigData(ref err);
                if ((pointConfig == null) ||
                    (pointConfig.MasterSource.Trim().ToLower() != "datatrac"))
                {
                    response.hdr.Successful = false;
                    response.hdr.StatusInfo = "DTSubmitLoan Error: pointConfig.MasterSource != DataTrac";
                    return response;
                }

                DataTracMgr dtm = new DataTracMgr();
                isSuccessful = dtm.SubmitLoan(req.FileId, req.Originator_Type, req.Loan_Program, ref sError);

                if (isSuccessful)
                {
                    response.hdr.Successful = true;
                    response.hdr.StatusInfo = string.Empty;
                }
                else
                {
                    response.hdr.Successful = false;
                    response.hdr.StatusInfo = sError;
                }

                return response;

            }
            catch (Exception ex)
            {
                string sErrorMsg = "DTSubmitLoan Error: " + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Warning);
                response.hdr.Successful = false;
                response.hdr.StatusInfo = sErrorMsg;
                return response;
            }

            return response;
        }



        /// <summary>
        /// Get status dates from DataTrac and update Pulse database
        /// neo 2011-06-22
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>

        public DT_GetStatusDatesResponse DTGetStatusDates(DT_GetStatusDatesRequest req)
        {
            DT_GetStatusDatesResponse DT_GetStatusDatesResponse1 = new DT_GetStatusDatesResponse();
            DT_GetStatusDatesResponse1.hdr = new RespHdr();

            if ((req == null) || (req.hdr.UserId < 0) || (req.FileIds == null))
            {
                DT_GetStatusDatesResponse1.hdr.Successful = false;
                DT_GetStatusDatesResponse1.hdr.StatusInfo = "DTGetStatusDates: DT_GetStatusDatesRequest is not initialized.";
                return DT_GetStatusDatesResponse1;
            }

            DataTracMgr DataTracMgr1 = new DataTracMgr();
            List<int> TargetFileIDs = new List<int>();

            if (req.FileIds.Length == 0)
            {
                string sError = string.Empty;
                List<Common.Table.Loans> LoanList = null;

                try
                {
                    string err = "";
                    DataAccess.PointConfig pointConfig;
                    pointConfig = da.GetPointConfigData(ref err);
                    if ((pointConfig == null) ||
                        (pointConfig.MasterSource.Trim().ToLower() != "datatrac"))
                    {
                        DT_GetStatusDatesResponse1.hdr.Successful = false;
                        DT_GetStatusDatesResponse1.hdr.StatusInfo = "DTGetStatusDates Error: pointConfig.MasterSource != DataTrac";
                        return DT_GetStatusDatesResponse1;
                    }

                    if (req.FileIds.Length == 0)
                    {
                        LoanList = DataTracMgr1.GetLoanInfoList("Processing", ref sError);
                    }

                    if (sError != string.Empty)
                    {
                        string sErrorMsg = "Failed to get loan info list from DataTrac: " + sError;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Warning);
                        DT_GetStatusDatesResponse1.hdr.Successful = false;
                        DT_GetStatusDatesResponse1.hdr.StatusInfo = sErrorMsg;
                        return DT_GetStatusDatesResponse1;
                    }
                }
                catch (Exception ex)
                {
                    string sErrorMsg = "Exception happened when get loan info list from DataTrac: " + ex.Message;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Warning);
                    DT_GetStatusDatesResponse1.hdr.Successful = false;
                    DT_GetStatusDatesResponse1.hdr.StatusInfo = sErrorMsg;
                    return DT_GetStatusDatesResponse1;
                }

                foreach (Common.Table.Loans LoanItem in LoanList)
                {
                    TargetFileIDs.Add(LoanItem.FileId);
                }

            }
            else
            {
                foreach (int iFileID in req.FileIds)
                {
                    TargetFileIDs.Add(iFileID);
                }
            }
            foreach (int iTargetFileID in TargetFileIDs)
            {
                string sError = string.Empty;
                List<StatusDate> StatusDateList = null;

                try
                {
                    StatusDateList = DataTracMgr1.GetDates(iTargetFileID, ref sError);

                    if (sError != string.Empty)
                    {
                        string sErrorMsg = "Failed to get status dates from DataTrac: " + sError;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Error);
                        DT_GetStatusDatesResponse1.hdr.Successful = false;
                        DT_GetStatusDatesResponse1.hdr.StatusInfo = sErrorMsg;
                        return DT_GetStatusDatesResponse1;
                    }
                }
                catch (Exception ex)
                {
                    string sErrorMsg = "Exception happened when get status dates from DataTrac: " + ex.Message;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Error);
                    DT_GetStatusDatesResponse1.hdr.Successful = false;
                    DT_GetStatusDatesResponse1.hdr.StatusInfo = sErrorMsg;
                    return DT_GetStatusDatesResponse1;
                }

                try
                {

                    string err = string.Empty;

                    da.UpdateDataTracStatusDate_All(iTargetFileID, StatusDateList);

                    Record.Loans loans_record = null;
                    loans_record = da.GetLoanInfo(iTargetFileID, ref err);

                    List<Table.LoanStages> stageList = da.GetLoanStagesByFileId(iTargetFileID, ref err);

                    if (stageList == null)
                    {

                        stageList = new List<Table.LoanStages>();

                        List<Table.DefaultStage> defStages = null;
                        da.GetDefaultStageName(ref defStages, ref err);

                        string pointDate = string.Empty;
                        short pointDateField = 0;
                        short pointStageField = 0;
                        foreach (Table.DefaultStage s in defStages)
                        {
                            DateTime ComplDate = DateTime.MinValue;
                            pointDate = string.Empty;
                            switch (s.Name)
                            {
                                case "Open":
                                case PointStage.Application:
                                    pointDateField = (short)PointStageDateField.Application;
                                    break;
                                case PointStage.SentToProcessing:
                                case "Processing":
                                case "Sent To Processing":
                                    pointDateField = (short)PointStageDateField.SentToProcessing;
                                    break;
                                case PointStage.HMDACompleted:
                                case PointStage.HMDA:
                                case PointStage.HMDAComplete:
                                    pointDateField = (short)PointStageDateField.HDMA;
                                    break;
                                case PointStage.Submit:
                                    pointDateField = (short)PointStageDateField.Submitted;
                                    break;
                                case PointStage.Approve:
                                    pointDateField = (short)PointStageDateField.Approved;
                                    pointDate = loans_record.DateApprove;
                                    break;
                                case PointStage.Resubmit:
                                case PointStage.Re_submit:
                                case PointStage.Resubmitted:
                                    pointDateField = (short)PointStageDateField.Resubmitted;
                                    break;
                                case PointStage.CleartoClose:
                                case "Clear To Close":
                                    pointDateField = (short)PointStageDateField.ClearedToClose;
                                    pointDate = loans_record.DateClearToClose;
                                    break;
                                case PointStage.DocsDrawn:
                                    pointDateField = (short)PointStageDateField.DocsDrawn;
                                    break;
                                case PointStage.DocsOut:
                                case "Docs":
                                    pointDateField = (short)PointStageDateField.DocsOut;
                                    break;
                                case PointStage.DocsReceived:
                                    pointDateField = (short)PointStageDateField.DocsReceived;
                                    break;
                                case PointStage.Fund:
                                    pointDateField = (short)PointStageDateField.Funded;
                                    pointDate = loans_record.DateFund;
                                    break;
                                case PointStage.Record:
                                    pointDateField = (short)PointStageDateField.Recorded;
                                    break;
                                case PointStage.Close:
                                    pointDateField = (short)PointStageDateField.Closed;
                                    break;
                                default:
                                    pointDateField = s.StageDateFld;
                                    pointStageField = s.StageNameFld;
                                    break;
                            }

                            if (string.IsNullOrEmpty(pointDate) || pointDate.Contains("1/1/0001"))
                                ComplDate = DateTime.MinValue;
                            else
                                DateTime.TryParse(pointDate, out ComplDate);

                            stageList.Add(new Table.LoanStages()
                            {
                                //FileId = fileId,
                                StageName = s.Name.Trim(),
                                Completed = ComplDate,
                                WflStageId = s.WflStageId,
                                WflTemplId = s.WflTemplId,
                                SequenceNumber = s.SequenceNumber,
                                DaysFromCreation = s.DaysAfterCreation,
                                DaysFromEstClose = s.DaysFromEstClose,

                            });

                        }

                    }

                    Table.Loans loans_table = new Table.Loans();
                    PointData pd = new PointData(pntLib, da);
                    loans_record.FileId = iTargetFileID;
                    pd.Process_LoanStatus_Stages(loans_record, ref loans_table, ref stageList);

                    da.UpdateDataTracLoanStages(iTargetFileID, loans_table);

                    #region Call UpdateState API after update loan Stage date

                    string strErr = "";
                    UpdateStageRequest reqUpdateStage = new UpdateStageRequest();
                    List<StageInfo> listStageInfo = null;
                    List<Common.Table.LoanStages> listLoanStage = null;
                    listLoanStage = da.GetLoanStagesByFileId(iTargetFileID, ref strErr);
                    if ((listLoanStage != null) && (listLoanStage.Count > 0))
                    {
                        listStageInfo = new List<StageInfo>();
                        foreach (Common.Table.LoanStages stage in listLoanStage)
                        {
                            if ((stage.Completed != null) && (stage.Completed == DateTime.MinValue))
                            {
                                StageInfo si = new StageInfo();
                                si.Completed = stage.Completed;
                                si.StageName = stage.StageName;
                                listStageInfo.Add(si);
                            }
                        }
                        if (listStageInfo.Count > 0)
                        {
                            reqUpdateStage.hdr = new ReqHdr();
                            reqUpdateStage.hdr.UserId = req.hdr.UserId;
                            reqUpdateStage.FileId = iTargetFileID;
                            reqUpdateStage.StageList = listStageInfo;
                            pm.UpdateStage(reqUpdateStage, ref strErr);
                        }
                    }

                    #endregion

                }
                catch (Exception ex)
                {
                    int eventID = 7099;
                    string sErrorMsg = "(datatrac) FileID = " + iTargetFileID.ToString() + " Failed to update Pulse database: " + ex.Message;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Warning, eventID);
                    DT_GetStatusDatesResponse1.hdr.Successful = false;
                    DT_GetStatusDatesResponse1.hdr.StatusInfo = sErrorMsg;
                    return DT_GetStatusDatesResponse1;
                }

            }

            DT_GetStatusDatesResponse1.hdr.Successful = true;
            DT_GetStatusDatesResponse1.hdr.StatusInfo = string.Empty;
            return DT_GetStatusDatesResponse1;
        }

        #endregion
        public MailChimp_Response MailChimp_SyncNow(int BranchId)
        {
            return mailChimpManager.MailChimp_SyncNow(BranchId);
        }

        public MailChimp_Response MailChimp_Subscribe(List<int> ContactIds, string MailChimpListId)
        {
            return mailChimpManager.MailChimp_Subscribe(ContactIds, MailChimpListId);
        }

        public MailChimp_Response MailChimp_Subscribe(int ContactId, string MailChimpListId)
        {
            return mailChimpManager.MailChimp_Subscribe(ContactId, MailChimpListId);
        }
        public MailChimp_Response MailChimp_Unsubscribe(List<int> ContactIds, string MailChimpListId)
        {
            return mailChimpManager.MailChimp_Unsubscribe(ContactIds, MailChimpListId);
        }

        public MailChimp_Response MailChimp_Unsubscribe(int ContactId, string MailChimpListId)
        {
            return mailChimpManager.MailChimp_Unsubscribe(ContactId, MailChimpListId);
        }

        public void ScheduleMailChimp()
        {
            mailChimpManager.ScheduleMailChimp();
        }
    }
}
