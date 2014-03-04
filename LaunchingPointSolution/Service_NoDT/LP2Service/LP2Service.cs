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
using System.Linq;
//using MarketingManager;
using LP2.Service.Common;
using LP2.Service;
//using DataTracManager;
using System.Data;
using Common;
using FocusIT.Pulse;

namespace LP2Service
{
    public class LP2Service : ILP2Service
    {
        short Category = 10;
        Framework.PNTLib pntLib = null;
        DataAccess.DataAccess da = null;
        LP2.Service.UserManager um = null;
        PointMgr pm = null;
        EmailManager.EmailMgr em = null;
        //MarketingManager.MkgMgr mm = null;
        private RuleManager.RuleMgr rm = null;
        WorkflowManager wm = null;
        MailChimpMgr.MailChimpManager mailChimpManager = new MailChimpManager();
        public LP2Service()
        {
            if (da == null)
                da = new DataAccess.DataAccess();
            if (um == null)
                um = UserManager.Instance;
            if (em == null)
                em = EmailManager.EmailMgr.Instance;
            if (wm == null)
                wm = WorkflowManager.Instance;

            if (pm == null)
                pm = PointMgr.Instance;
            //if (mm == null)
            //    mm = MarketingManager.MkgMgr.Instance;
            if (rm == null)
                rm = RuleManager.RuleMgr.Instance;
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
                versionInfo = "v3.9.0 released at 1/23 8:10 AM";
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
                int Event_id = 1001;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Successfully started Scheduled User Import Service.", EventLogEntryType.Information, Event_id, Category);
            }
            catch (Exception ex)
            {
                status = false;
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                int Event_id = 1090;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                int Event_id = 1002;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Successfully stopped Scheduled User Import Service.", EventLogEntryType.Information, Event_id, Category);
            }
            catch (Exception ex)
            {
                status = false;
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                int Event_id = 1091;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                int Event_id = 1092;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1093;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
            resp.Running = (LPService.SPIThread == null) ? false : LPService.SPIThread.IsAlive;
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
                int Event_id = 1001;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Successfully started Scheduled Point Import Service.", EventLogEntryType.Information, Event_id, Category);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                int Event_id = 1098;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                int Event_id = 1002;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Successfully stopped Scheduled Point Import Service.", EventLogEntryType.Information, Event_id, Category);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                err = MethodBase.GetCurrentMethod() + ", Exception: " + ex.Message;
                int Event_id = 1098;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            resp.hdr.Successful = status;
            resp.hdr.RequestId = 0;
            resp.hdr.StatusInfo = "";
            return resp;
        }

        public UpdateConditionsResponse UpdateConditions(UpdateConditionsRequest req)
        {
            UpdateConditionsResponse resp = new UpdateConditionsResponse();
            resp.hdr = new RespHdr();

            if (req == null || req.hdr == null || req.hdr.UserId <= 0 || req.FileId <= 0 || req.ConditionList == null ||
                !req.ConditionList.Any())
            {
                resp.hdr.StatusInfo = "Required parameters, Userid, FileId and ConditionList must not be missing.";
                resp.hdr.Successful = false;
                return resp;
            }
            var query = req.ConditionList.Where(
                item => item.ConditionId <= 0 || string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.Status));
            if (query.Any())
            {
                resp.hdr.StatusInfo = "Required condition parametrs Name and Status must not be missing.";
                resp.hdr.Successful = false;
                return resp;
            }

            resp = pm.UpdateConditions(req);

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
                if (pm == null)
                    pm = PointMgr.Instance;
                status = pm.ProcessRequest(e, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to import all loans from folder " + req.PointFolders[0] + ", Exception:" + ex.Message;
                int Event_id = 1097;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
            //EventLog.WriteEntry(InfoHubEventLog.LogSource, MethodBase.GetCurrentMethod() + ", received Import Loans request from userId=" + req.hdr.UserId + ", FileId=" + req.FileIds[0], EventLogEntryType.Information, Category);

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "ImportLoans", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.ImportLoans, 0, req);
                if (pm == null)
                    pm = PointMgr.Instance;
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to import loans, Exception: " + ex.Message;
                int Event_id = 1095;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                int Event_id = 1094;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                int Event_id = 1093;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1091;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }

            return resp;
        }

        public CheckPointFileStatusResp CheckPointFileStatus(CheckPointFileStatusReq req)
        {
            string err = "";
            bool status = false;

            CheckPointFileStatusResp resp = new CheckPointFileStatusResp();
            if (req == null)
            {
                err = "Can not check point file status, Request is empty.";
                resp.hdr.Successful = false;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = req.FileId;
                return resp;
            }
            if (req.FileId <= 0)
            {
                err = "Can not check point file status,  invalid File Id, " + req.FileId;
                resp.hdr.Successful = false;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = req.FileId;
                return resp;
            }

            try
            {
                resp = pm.CheckPointFileStatus(req);
                status = resp.hdr.Successful;
                resp.hdr.RequestId = req.FileId;
                return resp;
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to check point file status,  Exception:" + ex.Message;
                resp.hdr.Successful = false;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = req.FileId;
                return resp;
            }
            finally
            {
                if (status == false)
                {
                    int Event_id = 1090;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
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
                    int Event_id = 1089;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1088;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1087;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1086;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1085;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1084;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1083;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1082;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1081;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1080;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1079;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1078;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
                resp.hdr.RequestId = reqId;
            }
            return resp;
        }

        /// <summary>
        /// Imports the lock info.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        public ImportLockInfoResponse ImportLockInfo(ImportLockInfoRequest req)
        {
            var resp = new ImportLockInfoResponse();
            resp.hdr = new RespHdr();
            bool status = false;
            string err = "";
            int reqId = 0;

            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }

            if (req.FileId <= 0)
            {
                err = "No Point File Id specified in the request.";
                return resp;
            }

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "ImportLockInfo", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.ImportLockInfo, req.FileId, req);
                if (pm == null)
                    pm = PointMgr.Instance;
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to import LockInfo, Exception: " + ex.Message;
                int Event_id = 1077;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;
        }
        /// <summary>
        /// Updates the lock info.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        public UpdateLockInfoResponse UpdateLockInfo(UpdateLockInfoRequest req)
        {
            var resp = new UpdateLockInfoResponse();
            resp.hdr = new RespHdr();
            bool status = false;
            string err = "";
            int reqId = 0;

            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }

            if (req.FileId <= 0)
            {
                err = "No Point File Id specified in the request.";
                return resp;
            }

            try
            {
                da.AddRequestQueue(req.hdr.UserId, "UpdateLockInfo", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.UpdateLockInfo, req.FileId, req);
                if (pm == null)
                    pm = PointMgr.Instance;
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to update LockInfo, Exception: " + ex.Message;
                int Event_id = 1075;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
            }
            return resp;
        }
        /// <summary>
        /// Updates the Loan Profitability info.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        public UpdateLockInfoResponse UpdateLoanProfitability(UpdateLockInfoRequest req)
        {
            var resp = new UpdateLockInfoResponse();
            resp.hdr = new RespHdr();
            bool status = false;
            string err = "";
            int reqId = 0;
            #region check parameters
            if (req == null)
            {
                err = "Request is empty.";
                return resp;
            }

            if (req.FileId <= 0)
            {
                err = "No Point File Id specified in the request.";
                return resp;
            }
            #endregion
            try
            {
                da.AddRequestQueue(req.hdr.UserId, "UpdateLoanProfitability from MCT", ref reqId, ref err);
                PointMgrEvent pe = new PointMgrEvent(reqId, PointMgrCommandType.UpdateLoanProfitability, req.FileId, req);
                if (pm == null)
                    pm = PointMgr.Instance;
                status = pm.ProcessRequest(pe, ref err);
            }
            catch (Exception ex)
            {
                status = false;
                err = string.Format("Failed to update Loan Profitability from MCT in the Point file, FileID={0}, \r\nException:{1} ", req.FileId, ex.ToString());
                int Event_id = 1075;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                resp.hdr.Successful = status;
                resp.hdr.StatusInfo = err;
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
                    int Event_id = 1074;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1073;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                int Event_id = 1072;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 1071;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                //status = mm.UpdateCompany(ref err);
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
                    int Event_id = 1070;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1069;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                //status = mm.UpdateBranch(req.BranchId, ref err);
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
                    int Event_id = 1068;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                //status = mm.UpdateUser(req.UserId, ref err);
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
                    int Event_id = 1067;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                //status = mm.UpdateProspect(req.FileId, ref err);
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
                    int Event_id = 1066;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                //resp = mm.StartCampaign(req, ref err);
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
                    int Event_id = 1065;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                //status = mm.ProcessSyncMarketingData(ref err);

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
                    int Event_id = 1064;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                //status = mm.RemoveCampaign(req, ref err);
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
                    int Event_id = 1063;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                //resp = mm.CompleteCampaignEvent(req, ref err);
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
                    int Event_id = 1062;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                //resp = mm.GetUserAccountBalance(req, ref err);
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
                    int Event_id = 1061;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                //resp = mm.UpdateCreditCard(req, ref err);
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
                    int Event_id = 1060;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                //resp = mm.GetCreditCard(req, ref err);
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
                    int Event_id = 1059;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                //resp = mm.AddToAccount(req, ref err);
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
                    int Event_id = 1058;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    //status = mm.ReassignProspect(req, ref err);
                }
                else
                {
                    if (req.ContactId != null)
                    {
                        FileId_List = da.GetFileId_FromContactId(req.ContactId, ref err);
                        req.FileId = FileId_List.ToArray();
                        //status = mm.ReassignProspect(req, ref err);
                    }
                    else
                    {
                        if (req.FileId != null)
                        {
                            //status = mm.ReassignProspect(req, ref err);
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
                    int Event_id = 1057;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1056;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1055;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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

        #region added for CR64

        public PreviewLSRResponse PreviewLSR(PreviewLSRRequest req)
        {
            PreviewLSRResponse resp = new PreviewLSRResponse();
            resp.hdr = new RespHdr();
            string err = "";
            bool status = false;

            if (req == null)
            {
                resp.hdr.Successful = false;
                resp.hdr.StatusInfo = "request object can't be NULL";
                return resp;
            }

            if (req.FileId <= 0)
            {
                resp.hdr.Successful = false;
                resp.hdr.StatusInfo = "The FileID is invalid";
                return resp;
            }

            if (req.ContactId <= 0 && req.UserId <= 0)
            {
                resp.hdr.Successful = false;
                resp.hdr.StatusInfo = "Must have at least a ContactId or UserId";
                return resp;
            }

            if (req.ContactId > 0 && req.UserId > 0)
            {
                resp.hdr.Successful = false;
                resp.hdr.StatusInfo = "Can have only a UserId or ContactId";
                return resp;
            }

            try
            {
                int banner_idx = 0;
                int cid_idx = 0;

                int first = 0;
                string b_str = "";
                int middle = 0;
                string m_str = "";
                string e_str = "";

                string raw_str = "";
                string processed_str = "";

                ReportManager rm1 = ReportManager.Instance;
                resp = rm1.PreviewLSR(req);

                raw_str = Encoding.UTF8.GetString(resp.ReportContent);

                banner_idx = raw_str.IndexOf("main-banner.jpg");
                cid_idx = raw_str.IndexOf("cid:");

                if ((cid_idx > 0) && (cid_idx < banner_idx))
                {
                    first = raw_str.IndexOf("style=\"width");
                    b_str = raw_str.Substring(0, first + 40);
                    middle = b_str.LastIndexOf("/>");
                    m_str = "\"width:330px; height:64px\"";
                    b_str = raw_str.Substring(0, first + 6);
                    e_str = raw_str.Substring(middle, raw_str.Length - middle);

                    processed_str = b_str + m_str + e_str;

                    resp.ReportContent = Encoding.UTF8.GetBytes(processed_str);
                }

                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                err = "Failed to PreviewLSR,  Exception:" + ex.Message;

                if (resp != null && resp.hdr != null)
                {
                    resp.hdr.StatusInfo =
                        string.Format("Failed to generate the report content. Please contact tech support. Error:{0}",
                                      resp.hdr.StatusInfo);
                    return resp;
                }
            }
            finally
            {
                if (status == false)
                {
                    int Event_id = 1055;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
                if (resp == null)
                {
                    resp = new PreviewLSRResponse();
                    resp.hdr = new RespHdr();
                    resp.hdr.Successful = status;
                    resp.hdr.StatusInfo = err;
                }
            }
            return resp;
        }

        #endregion


        #endregion

        #region MailChimp
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
        #endregion
        #region DataTrac Stuff

        public DT_GetStatusDatesResponse DTGetStatusDates(DT_GetStatusDatesRequest req)
        {
            throw new NotImplementedException();
        }

        public DT_ImportLoansResponse DTImportLoans(DT_ImportLoansRequest req)
        {
            throw new NotImplementedException();
        }
        public DT_SubmitLoanResponse DTSubmitLoan(DT_SubmitLoanRequest req)
        {
            throw new NotImplementedException();
        }
        #endregion DataTrac Stuff
        #region Lead Manager Methods
        public RespHdr PostLead(FocusIT.Pulse.PostLeadRequest req)
        {
            bool status = false;
            string err = string.Empty;
            RespHdr resp = new RespHdr();
            try
            {
                if (req == null || req.RequestHeader == null || string.IsNullOrEmpty(req.RequestHeader.SecurityToken))
                {
                    err = "Request, Request Header or Security Token is empty.";
                    return resp;
                }
                LeadMgr.LeadMgr lm = new LeadMgr.LeadMgr();
                status = lm.PostLead(req, out err);
                return resp;
            }
            catch (Exception ex)
            {
                err = ex.ToString();
                return resp;
            }
            finally
            {
                if (!status)
                {
                    int Event_id = 1054;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    resp.StatusInfo = err;
                }
                resp.Successful = status;
            }
        }
        #endregion

        #region Lead Manager Service

        /// <summary>
        /// Leads the routing_ get next loanofficer.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns>LR_GetNextLoanOfficerResp.</returns>
        public LR_GetNextLoanOfficerResp LeadRouting_GetNextLoanofficer(LR_GetNextLoanOfficerReq req)
        {
            bool status = false;
            string err = string.Empty;
            var resp = new LR_GetNextLoanOfficerResp();
            resp.RespHdr = new RespHdr();

            try
            {
                if (req == null || req.ReqHdr == null || string.IsNullOrEmpty(req.ReqHdr.SecurityToken))
                {
                    err = "Request, Request Header or Security Token is empty.";
                    return resp;
                }

                LeadMgr.LeadMgr lm = new LeadMgr.LeadMgr();
                var userId = lm.GetNextLoanofficer(req.LeadSource, req.State, req.Purpose, req.LoanType, ref err);
                resp.NextLoanOfficerID = userId;
                if (userId != 0)
                    status = true;

                return resp;
            }
            catch (Exception ex)
            {
                err = ex.ToString();
                return resp;
            }
            finally
            {
                if (!status)
                {
                    int Event_id = 1054;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    resp.RespHdr.StatusInfo = err;
                }
                resp.RespHdr.Successful = status;
            }
        }

        /// <summary>
        /// Leads the routing_ assign loan officer.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns>RespHdr.</returns>
        public RespHdr LeadRouting_AssignLoanOfficer(LR_AssignLoanOfficerReq req)
        {
            bool status = false;
            string err = string.Empty;
            RespHdr resp = new RespHdr();
            try
            {
                if (req == null || req.ReqHdr == null || string.IsNullOrEmpty(req.ReqHdr.SecurityToken))
                {
                    err = "Request, Request Header or Security Token is empty.";
                    return resp;
                }
                if (req.LoanId <= 0)
                {
                    err = "Request, LoanId is empty.";
                    return resp;
                }
                if (req.LoanOfficerId <= 0)
                {
                    err = "Request, LoanOfficerId is empty.";
                    return resp;
                }

                LeadMgr.LeadMgr lm = new LeadMgr.LeadMgr();
                status = lm.AssignLoanOfficer(req, out err);
                return resp;
            }
            catch (Exception ex)
            {
                err = ex.ToString();
                return resp;
            }
            finally
            {
                if (!status)
                {
                    int Event_id = 1054;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    resp.StatusInfo = err;
                }
                resp.Successful = status;
            }
        }

        #endregion

    }
}
