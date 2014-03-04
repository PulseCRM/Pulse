using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Collections;
using Framework;
using DataAccess;
using focusIT;
using Ionic.Zip;
using LP2.Service;
using Common;
using LP2.Service.Common;
using Utilities;
using System.Data;
using System.Text.RegularExpressions;
using System.Configuration;

namespace LP2Service
{
    public class PointMgr : IPointMgr
    {
        short Category = 90;
        SingleThreadedContext m_ThreadContext = null;
        static PointMgr m_Instance = null;
        static WorkflowManager wm_Instance = null;
        static DataAccess.DataAccess da = null;
        static Framework.PNTLib pntLib = null;
        static List<Table.PointFieldDesc> m_PointFieldList = null;
        static int m_RefCount = 0;
        private static Random random = new Random((int)DateTime.Now.Ticks);
        public static string PointZipFolder = @"C:\PulseBackup\";
        private PointMgr()
        {
            string err = "";
            bool logErr = false;
            int Event_id = 9099;
            try
            {
                string tempOU = ConfigurationManager.AppSettings["OU"];
                tempOU = tempOU.Replace(',', '_');
                PointZipFolder += tempOU;

                if (m_ThreadContext == null)
                {
                    m_ThreadContext = new SingleThreadedContext();
                    m_ThreadContext.ExceptionEvent += new ExceptionEventHandler(ThreadExceptionHandler);
                    m_ThreadContext.Init("Point Manager");
                }

                if (da == null)
                    da = new DataAccess.DataAccess();
                if (pntLib == null)
                    pntLib = new Framework.PNTLib();
                if (m_PointFieldList == null)
                {
                    m_PointFieldList = da.GetPointFieldDesc(ref err);
                    if ((m_PointFieldList == null) || (m_PointFieldList.Count <= 0))
                    {
                        err = "Point Manager Database Error: Point Field Desc List is null";
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                    }
                }
                if (wm_Instance == null)
                    wm_Instance = WorkflowManager.Instance;
            }
            catch (Exception ex)
            {
                err = "Point Manager, failed to initialize, Exception: " + ex.Message;
                Event_id = 9097;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    Event_id = 9095;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        public static PointMgr Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PointMgr();
                m_RefCount++;
                return m_Instance;
            }
        }

        public void Dispose()
        {
            m_RefCount--;
            if (m_RefCount == 0)
                m_Instance = null;
            if (m_ThreadContext != null)
            {
                m_ThreadContext.ClearQueue();
                m_ThreadContext.Exit();
            }

            if (m_PointFieldList != null)
            {
                m_PointFieldList.Clear();
                m_PointFieldList = null;
            }
        }

        public List<Table.PointFieldDesc> GetPointFieldDescList()
        {
            return m_PointFieldList;
        }

        public void ThreadExceptionHandler(object sender, ExceptionEventArgs args)
        {
            Trace.TraceError(args.Exception.Message);
            string err = args.Exception.Message;
            int Event_id = 9093;
            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
        }

        #region ImportLoans

        private bool ImportPointData(PointData pd, string filepath, ProspectFlagEnum Prospect_flag, ref string err)
        {
            string folder = "";
            short loanStatus = 0;
            int branchId = 0, folderId = 0, PDSFolderId = 0;
            bool changed = false;
            err = "";

            if (Framework.FileHelper.IsFilenameValid(filepath, ref folder, ref err) == false)
            {
                err = "ImportLoans, " + err;
                return false;
            }

            DateTime lastImport = DateTime.MinValue;

            if (da.GetPointFolderId(folder, ref folderId, ref branchId, ref loanStatus, ref lastImport, ref PDSFolderId, ref err) == false)
            {
                return false;
            }

            DateTime lastModified = File.GetLastWriteTime(filepath);

            if ((lastImport != DateTime.MinValue) && (lastModified > lastImport))
                changed = true;

            int FileId = pd.ProcessPointData(filepath, folder, folderId, branchId, changed, Prospect_flag, PDSFolderId, ref err);

            if (FileId <= 0)
            {
                return false;
            }

            string LoanStatus = (Prospect_flag == ProspectFlagEnum.RealtimeProspectFlag || Prospect_flag == ProspectFlagEnum.ScheduledProspectFlag) ? "Prospect" : "Processing";
            AutoApplyWorkflow(FileId, LoanStatus, ref err);
            return true;
        }
     
        private ProspectFlagEnum GetProspectFlag(LoanStatusEnum ls)
        {
            ProspectFlagEnum Prospect_flag = ProspectFlagEnum.Unknown;

            switch (ls)
            {
                case LoanStatusEnum.Archive:
                case LoanStatusEnum.Canceled:
                case LoanStatusEnum.Closed:
                case LoanStatusEnum.Denied:
                case LoanStatusEnum.Suspended:
                    Prospect_flag = ProspectFlagEnum.ArchivedLoansFlag;
                    break;
                case LoanStatusEnum.Prospect:
                case LoanStatusEnum.ProspectArchive:
                    Prospect_flag = ProspectFlagEnum.RealtimeProspectFlag;
                    break;
                case LoanStatusEnum.Processing:
                    Prospect_flag = ProspectFlagEnum.RealtimePointFlag;
                    break;
            }
            return Prospect_flag;
        }

        private bool ImportLoans(ImportLoansRequest req, ref string err)
        {
            bool logErr = false;
            ProspectFlagEnum Prospect_flag = ProspectFlagEnum.RealtimePointFlag;
            err = "";
            if ((req == null) || (req.hdr == null))
            {
                err = "Import Loans:: request is empty.";
                logErr = true;
                return false;
            }

            if (req.hdr.UserId < 0)
            {
                err = "Import Loans:: invalid User Id, " + req.hdr.UserId;
                logErr = true;
                return false;
            }

            if ((req.FileIds == null) || (req.FileIds.Length <= 0))
            {
                err = "Import Loans:: no File Ids specified.";
                logErr = true;
                return false;
            }

            List<string> paths = da.GetPointFilePaths(req.FileIds, ref err);
            PointData pd = null;

            try
            {
                int i = 0; int fileId = 0;
                string path = "";
                pd = new PointData(pntLib, da);
                LoanStatusEnum eLoanStatus = LoanStatusEnum.Processing;
                string folderStatus = "";
                int loanStatus;
                for (i = 0; i < req.FileIds.Length; i++)
                {
                    fileId = req.FileIds[i];
                    if (fileId <= 0)
                    {
                        err = "Import Loans:: Invalid FileId specified:" + fileId;
                        logErr = true;
                        return false;
                    }
                    path = da.GetPointFilePath(fileId, ref folderStatus, ref err);
                    if (path == string.Empty || string.IsNullOrEmpty(Path.GetFileName(path)))
                    {
                        err = String.Format("Cannot sync with Point, invalid filename specified for FileId={0}", fileId);
                        logErr = true;
                        return false;
                    }
                    if (string.IsNullOrEmpty(folderStatus))
                    {
                        err = String.Format("Cannot sync with Point, invalid LoanStatus {0} specified for ={0}", fileId);
                        logErr = true;
                        return false;
                    }
                    loanStatus = Convert.ToInt16(folderStatus);
                    eLoanStatus = (LoanStatusEnum)loanStatus;
                    DataAccess.PointConfig PointConfig1 = null;
                    PointConfig1 = da.GetPointConfigData(ref err);

                    if (PointConfig1 == null)
                    {
                        err = "Failed to get Point Configuration data, err:" + err;
                        int Event_id = 9099;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                        return false;
                    }
                    if (PointConfig1.MasterSource.ToUpper() == "DATATRAC" && eLoanStatus == LoanStatusEnum.Processing)
                    {
                        err = "Master Source is DataTrac, cannot import active loan file from Point.";
                        int Event_id = 9091;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                    Prospect_flag = GetProspectFlag(eLoanStatus);
                    if (ImportPointData(pd, path, Prospect_flag, ref err) == false)
                    {
                        string errMsg = string.Format("ImportPointData return false,  path: {0} ,  err: {1} ", path, err);
                        int Event_id = 9090;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Import Loans, Exception: " + ex.Message;
                string errMsg = err + " StackTrace:    " + ex.StackTrace;
                int Event_id = 9089;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (pd != null)
                {
                    pd.Dispose();
                    pd = null;
                }
                if (paths != null)
                {
                    paths.Clear();
                    paths = null;
                }
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9088;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }   

        #endregion
        #region Import Cardex
        private bool ImportCardex(PointMgrEvent e, ref string err)
        {
            err = "";
            bool logErr = false;
            if ((e == null) || (e.Request == null))
            {
                err = "Unable to import Cardex, invalid request.";
                int Event_id = 9087;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            int reqId = e.ReqId;
            ImportCardexRequest req = e.Request as ImportCardexRequest;
            int userId = 0;
            if ((req == null) || (req.CardexFile == null) || (req.hdr == null) || (req.CardexFile == string.Empty))
            {
                err = "Unable to import Cardex, invalid request.";
                int Event_id = 9086;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            userId = req.hdr.UserId;
            CardexHelper cardex = null;
            try
            {
                cardex = new CardexHelper();
                cardex.ImportCardex(req.hdr.UserId, reqId, req.CardexFile, ref err);
                return true;
            }
            catch (Exception ex)
            {
                err = "Import Cardex, Exception; " + ex.Message;
                int Event_id = 9085;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                string errMsg = "";
                da.UpdateRequestQueue(userId, "Import Cardex", ref reqId, !logErr, err, ref errMsg);
                if (cardex != null)
                {
                    cardex.Close();
                    cardex = null;
                }
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9084;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }
        #endregion
        #region Import Loan Reps
        private bool ImportLoanReps(PointMgrEvent e, ref string err)
        {
            err = "";
            bool logErr = false;
            if ((e == null) || (e.Request == null))
            {
                err = "Import Loan Reps:: request is empty.";
                int Event_id = 9083;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            ImportLoanRepNamesRequest req = e.Request as ImportLoanRepNamesRequest;
            if ((req == null) || (req.PointFolders == null) || (req.PointFolders.Length <= 0))
            {
                err = "Import Loan Reps: no file path specified.";
                Trace.TraceError(err);
                int Event_id = 9082;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            string[] filepatharray = req.PointFolders;
            int total = 0;
            int proc = 0;
            int failed = 0;
            int branchId = 0;
            int reqId = e.ReqId;
            total = filepatharray.Length;
            List<string> FieldArray = new List<string>();
            ArrayList FieldSeq = new ArrayList();
            try
            {
                foreach (string filepath in filepatharray)
                {
                    try
                    {
                        if (da.UpdateReqProgress(req.hdr.UserId, "Import Loan Reps", ref reqId, total, proc, failed, ref err) == false)
                            Trace.TraceError(err);
                        proc++;
                        if (pntLib.ReadPointData(ref FieldArray, ref FieldSeq, filepath, ref err) == false)
                        {
                            failed++;
                            Trace.TraceError(err);
                            string errMsg = string.Format("ReadPoint Data return false,  filepath: {0}, err: {1} ", filepath, err);
                            int Event_id = 9081;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                            continue;
                        }
                        string temp = pntLib.getPointField(FieldArray, FieldSeq, 19);
                        if ((temp == null) || (temp == String.Empty))
                        {
                            failed++;
                            err = "ImportLoanReps, missing Loan Rep name in " + filepath + ".";
                            Trace.TraceError(err);
                            int Event_id = 9080;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                            continue;
                        }
                        if (da.GetBranchIdByFolderName(filepath, ref branchId, ref err) == false)
                        {
                            failed++;
                            Trace.TraceError(err);
                            string errMsg = string.Format("GetBranchIdByFolderName return false, filepath: {0}, err: {1} ", filepath, err);
                            int Event_id = 9079;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                            continue;
                        }
                        if (da.Save_UserLoanRep(temp, branchId, ref err) == false)
                        {
                            failed++;
                            Trace.TraceError(err);
                            string errMsg = string.Format("Save_UserLoanRep return false, err: {0} ", err);
                            int Event_id = 9078;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        failed++;
                        err = "Import Loan Rep failed on " + filepath + ", Exception:" + ex.Message;
                        int Event_id = 9077;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        Trace.TraceError(err);
                    }
                }
                return true;
            }
            catch (Exception ee)
            {
                err = "Import Loan Rep failed, Exception: " + ee.Message;
                int Event_id = 9076;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                da.UpdateRequestQueue(req.hdr.UserId, "Import Loan Reps", ref reqId, !logErr, err, ref err);
                if (FieldArray != null)
                {
                    FieldArray.Clear();
                    FieldArray = null;
                }
                if (FieldSeq != null)
                {
                    FieldSeq.Clear();
                    FieldSeq = null;
                }

                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9075;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }

        }

        #endregion

        #region Import All Loans
        public bool ImportAllLoans(PointMgrEvent e, bool force)
        {
            string err = "";
            int Event_id = 9008;
            bool logErr = false;
            ProspectFlagEnum Prospect_flag = ProspectFlagEnum.ScheduledPointFlag;
            if ((e == null) || (e.Request == null))
            {
                err = "ImportAllLoans: request/path is empty.";
                Event_id = 9074;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            ImportAllLoansRequest req = e.Request as ImportAllLoansRequest;
            if ((req == null) || (req.hdr == null) || (req.PointFolders.Length <= 0))
            {
                err = "ImportAllLoans: request/path is empty.";
                Event_id = 9073;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            int total = 0;
            int proc = 0;
            int failed = 0;
            int folderTotal = 0;
            int folderProc = 0;
            int folderId = 0;
            int branchId = 0;
            int PDSFolderId = 0;

            PointData pd = new PointData(pntLib, da);

            int reqId = e.ReqId;

            List<string> FileList = new List<string>();
            List<Table.PointFileInfo> PointFilesInDB = null;
            DataAccess.PointConfig PointConfig1 = null;

            short loanStatus = 0;
            LoanStatusEnum eLoanStatus;
            DateTime LastImport = DateTime.MinValue;
            try
            {
                PointConfig1 = da.GetPointConfigData(ref err);

                if (PointConfig1 == null)
                {
                    err = "Failed to get Point Configuration data, err:" + err;
                    Event_id = 9099;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                    Trace.TraceError(err);
                    return false;
                }
                int taskCount = 0;
                foreach (string PointFolderPath in req.PointFolders)
                {

                    if (da.GetPointFolderId(PointFolderPath, ref folderId, ref branchId, ref loanStatus, ref LastImport, ref PDSFolderId, ref err) == false)
                    {
                        string errMsg = string.Format("GetPointFolderId return false, PointFolderPath: {0}, err: {1} ", PointFolderPath, err);
                        Event_id = 9072;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                        continue;
                    }
                    eLoanStatus = (LoanStatusEnum)loanStatus;
                    Prospect_flag = GetProspectFlag(eLoanStatus);
                    if (PointConfig1.MasterSource.ToUpper() == "DATATRAC" && eLoanStatus == LoanStatusEnum.Processing)
                    {
                        Event_id = 9071;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, "Master Source is DataTrac, cannot import  " + PointFolderPath, EventLogEntryType.Warning, Event_id, Category);
                        continue;
                    }

                    if (PointFilesInDB != null)
                        PointFilesInDB.Clear();
                    PointFilesInDB = da.GetPointFileInfoByFolderId(folderId, ref err);

                    if ((PointFilesInDB != null) && (PointFilesInDB.Count > 0))
                    {
                        string errMsg = "";
                        foreach (Table.PointFileInfo pInfo in PointFilesInDB)
                        {
                            taskCount = 0;
                            if (File.Exists(pInfo.Path)
                                || Prospect_flag == ProspectFlagEnum.RealtimeProspectFlag ||
                                Prospect_flag == ProspectFlagEnum.ScheduledProspectFlag)
                                continue;
                            taskCount = da.GetNumberLoanTasks(pInfo.FileId, ref err);
                            if (taskCount > 0)
                            {
                                if (pInfo.FileId > 0)
                                {
                                    da.Save_PointImportHistory(pInfo.FileId, "Error", "Point file does not exists. It may have been moved to a different folder.\r\n", ref err);
                                }
                                continue;
                            }
                            da.Remove_Loan(pInfo.FileId, ref errMsg);
                        }
                    }

                    FileList.Clear();
                    string[] fileExt = null;
                    if (Prospect_flag == ProspectFlagEnum.ScheduledProspectFlag ||
                        Prospect_flag == ProspectFlagEnum.RealtimeProspectFlag)
                        fileExt = new string[] { "*.BRW", "*.PRS" };
                    else
                        fileExt = new string[] { "*.BRW" };
                    FileHelper.GetFileList(PointFolderPath, fileExt, ref FileList, true);
                    total = total + FileList.Count;
                    folderTotal = FileList.Count;
                    folderProc = 0;

                    da.UpdateReqProgress(req.hdr.UserId, "Import All Loans", ref reqId, total, proc, failed, ref err);
                    if (FileList.Count == 0)
                    {
                        err = "Import All Loans" + ", No Point file found in the PointFolder " + PointFolderPath + ".";
                        Trace.TraceError(err);
                        //Event_id = 9018;
                        //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id, Category);
                        if (da.UpdateRequestQueue(req.hdr.UserId, "Import All Loans", ref reqId, false, err, ref err) == false)
                            Trace.TraceError(err);
                        continue;
                    }

                    foreach (string filepath in FileList)
                    {
                        err = "";
                        bool changed = false;
                        try
                        {
                            LastImport = DateTime.MinValue;
                            da.UpdateReqProgress(req.hdr.UserId, "Import All Loans", ref reqId, total, proc, failed, ref err);
                            folderProc++;
                            proc++;
                            if ((PointFilesInDB != null) && (PointFilesInDB.Count > 0))
                            {
                                foreach (Table.PointFileInfo pInfo in PointFilesInDB)
                                {
                                    if (filepath.Trim().ToUpper() == pInfo.Path.Trim().ToUpper())
                                    {
                                        LastImport = pInfo.LastImported;
                                        break;
                                    }
                                }
                            }
                            DateTime fileModTime = File.GetLastWriteTime(filepath);
                            if ((LastImport != DateTime.MinValue) && (fileModTime < LastImport))
                            {
                                if (!force)
                                    continue;
                            }
                            else changed = true;
                            if (ImportPointData(pd, filepath, Prospect_flag, ref err) == false)
                            {
                                failed++;
                                string errMsg = string.Format("ImportPointData return false, filepath: {0}, err: {1} ", filepath, err);
                                Event_id = 9070;
                                //EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                                //Trace.TraceError(err);
                                continue;
                            }

                            da.Save_PointFiles(filepath, PointFolderPath, ref folderId, ref branchId, null, changed, ref err);//todo check
                        }
                        catch (Exception ex)
                        {
                            failed++;
                            string errMsg = string.Format("Failed to import loan, filepath: {0}, err: {1} ", filepath, ex.Message);
                            Event_id = 9069;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                            Trace.TraceError(errMsg);
                        }
                    }
                    if (da.Save_PointFolderImportStats(folderId, folderProc, ref err) == false)
                    {
                        string errMsg = string.Format("Save_PointFolderImportStats, err: {0} ", err);
                        Event_id = 9068;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                        Trace.TraceError(err);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                err = "ImportAllLoans, Exception:" + ex.Message;
                Event_id = 9067;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                string errMsg = "";
                da.UpdateRequestQueue(req.hdr.UserId, "Import All Loans", ref reqId, !logErr, err, ref errMsg);
                if (logErr)
                {
                    Event_id = 9066;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
                if (pd != null)
                {
                    pd.Dispose();
                    pd = null;
                }
                if (PointFilesInDB != null)
                {
                    PointFilesInDB.Clear();
                    PointFilesInDB = null;
                }
                if (FileList != null)
                {
                    FileList.Clear();
                    FileList = null;
                }
            }
        }

        #endregion

        public void AddUpdatedFields(ref List<FieldMap> fieldMap, short fieldId, string value)
        {
            if (fieldMap == null)
                fieldMap = new List<FieldMap>();

            if (fieldId <= 0)
                return;

            FieldMap fm = new FieldMap();
            fm.FieldId = fieldId;
            fm.Value = value.Trim();
            fieldMap.Add(fm);
        }

        private bool Get_CheckPointFile(int fileId, ref string filePath, bool create, FileMode fileMode, ref string err)
        {
            err = "";
            FileStream fs = null;
            filePath = da.GetPointFilePath(fileId, ref err);
            if (filePath == String.Empty)
            {
                if (string.IsNullOrEmpty(err))
                    err = string.Format(" Missing Point filename or invalid Point filename for the FileId={0}.", fileId);
                int Event_id = 9065;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            string folder = "";
            if (create)
            {
                if (!string.IsNullOrEmpty(Path.GetFileName(filePath)) && !FileHelper.IsFilenameValid(filePath, ref folder, ref err))
                {
                    err = string.Format(" Invalid Point Filename {0} specified for FileId {1}.", filePath, fileId);
                    int Event_id = 9064;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                return true;
            }

            if (FileHelper.IsFilenameValid(filePath, ref folder, ref err) == false)
            {
                return false;
            }
            if (!File.Exists(filePath))
            {
                err = "Specified Point File " + filePath + " does not exist.";
                int Event_id = 9063;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if (FileHelper.ReadOnly(filePath) == true)
            {
                err = string.Format("Cannot update the specified Point file {0} is readonly. ", filePath);
                int Event_id = 9062;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            FileMode fm = fileMode;

            try
            {
                fs = new FileStream(filePath, fm, FileAccess.ReadWrite);
            }
            catch (Exception ex)
            {
                err = string.Format("FileStream error, filePath: {0},  Exception: {1}  ", filePath, ex.Message);
                int Event_id = 9061;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
            }
            return true;
        }

        public void AutoApplyWorkflow(int FileId, string LoanStatus, ref string err)
        {
            PointConfig pointConfig;
            bool LogErr = false;
            int WflTemplId = 0;
            try
            {
                if (FileId <= 0 || string.IsNullOrEmpty(LoanStatus))
                {
                    err = string.Format("AutoApplyWorkflow, FileId {0} or LoanStatus {1} not correct.", FileId, LoanStatus);
                    int Event_id = 9060;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }
                pointConfig = da.GetPointConfigData(ref err);
                if (pointConfig == null ||
                    (pointConfig.AutoApplyProcessingWorkflow == false &&
                    pointConfig.AutoApplyProspectWorkflow == false))
                    return;

                switch (LoanStatus.ToUpper().Trim())
                {
                    case "PROCESSING":
                        if (pointConfig.AutoApplyProcessingWorkflow == false)
                            return;
                        break;
                    case "PROSPECT":
                        if (pointConfig.AutoApplyProspectWorkflow == false)
                            return;
                        break;
                    default:
                        //err = string.Format("AutoApplyWorkflow, Invalid Loan Status {0} specified", LoanStatus);
                        //LogErr = true;
                        return;
                }
                WflTemplId = da.GetDefaultWorkflowTemplate(FileId);
                if (WflTemplId <= 0)
                {
                    err = string.Format("No default workflow template found for FileId: {0}.", FileId);
                    //int Event_id = 9059;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                           
                    return;
                }
                WorkflowManager wm = WorkflowManager.Instance;
                GenerateWorkflowRequest req = new GenerateWorkflowRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 0;
                req.FileId = FileId;
                req.WorkflowTemplId = WflTemplId;
                wm.GenerateWorkflow(req, ref err);
                string msg = string.Format("FileId={0}, Workflow Templ Id={1}", req.FileId, req.WorkflowTemplId);
                da.AddRequestQueue(req.hdr.UserId, "Generate Workflow - Auto Apply", msg, ref err);
                //da.Save_ApplyWorkflowTempl(req.FileId, req.WorkflowTemplId, req.hdr.UserId, ref err);
                if (!string.IsNullOrEmpty(err))
                    LogErr = true;
            }
            catch (Exception exception)
            {
                err = string.Format("AutoApplyWorkflow, FileId {0} LoanStatus {1}, Exception: {2}", FileId, LoanStatus, exception.Message + "\n" + exception.StackTrace);
                int Event_id = 9058;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            finally
            {
                if (LogErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9057;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }
        #region Update Methods
        private bool ExtendRateLock(ExtendRateLockRequest req, ref string err)
        {
            err = "";
            bool logErr = false;
            bool fatal = false;
            #region check Rate Lock parameter
            if ((req == null) || (req.hdr == null))
            {
                err = "Invalid ExtendRateLock Request, request is null. ";
                int Event_id = 9056;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if ((req.FileId <= 0) || (req.hdr.UserId <= 0))
            {
                err = "Missing FileId or User Id in the Extend Rate Lock request ";
                int Event_id = 9055;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if ((req.DaysExtended <= 0) || (req.NewDate == DateTime.MinValue))
            {
                err = "No Extended RateLock Expiration date specified for Request ";
                int Event_id = 9054;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            int DaysExtended = req.DaysExtended;
            DateTime NewDate = req.NewDate;
            string filePath = "";
            if (Get_CheckPointFile(req.FileId, ref filePath, false, FileMode.Open, ref err) == false)
            {
                err = string.Format("ExtendRateLock Get_CheckPointFile return false, fileid: {0}, err: {1} ", req.FileId, err);
                int Event_id = 9053;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if (filePath == string.Empty)
            {
                err = "ExtendRateLock Point filepath is empty.";
                int Event_id = 9052;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            #endregion

            List<Framework.FieldMap> UpdatedFieldds = new List<Framework.FieldMap>();
            try
            {
                UpdatedFieldds.Clear();
                // Days to extend
                AddUpdatedFields(ref UpdatedFieldds, 11439, DaysExtended.ToString());
                // Rate Lock Expiration Date
                AddUpdatedFields(ref UpdatedFieldds, 6063, NewDate.ToString("d"));
                if (pntLib.WritePointData(UpdatedFieldds, filePath, ref  err) == false)
                {
                    err = string.Format("pntLib.WritePointData return false, filePath: {0}, err: {1} ", filePath, err);
                    int Event_id = 9051;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if (da.ExtendRateLock(req.FileId, (short)req.DaysExtended, req.hdr.UserId, ref err) == false)
                {
                    err = string.Format("da.ExtendRateLock return false, fileId: {0}, err: {1} ", req.FileId, err);
                    int Event_id = 9050;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Cannot update the specified Point file " + filePath + ", Exception:" + ex.Message;
                int Event_id = 9049;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (UpdatedFieldds != null)
                {
                    UpdatedFieldds.Clear();
                    UpdatedFieldds = null;
                }
                if (logErr)
                {
                    int Event_id = 9048;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
                else
                {
                    err = "Rate lock has been extended to " + NewDate.ToString() + " for Point file " + filePath;
                    int Event_id = 9002;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id, Category);
                    Trace.TraceInformation(err);
                }
            }
        }

        private bool UpdateEstCloseDate(UpdateEstCloseDateRequest req, ref string err)
        {
            err = "";
            bool logErr = false;
            bool fatal = false;
            #region check Est Close parameter
            if ((req == null) || (req.hdr == null))
            {
                err = "Invalid UpdateEstCloseDate Request, request is null. ";
                int Event_id = 9047;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if ((req.FileId <= 0) || (req.hdr.UserId <= 0))
            {
                err = "Missing FileId or User Id in the UpdateEstCloseDateRequest.";
                int Event_id = 9046;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if ((req.NewDate == null) || (req.NewDate == DateTime.MinValue))
            {
                err = "No Est Close Date specified in the UpdateEstCloseDateRequest.";
                int Event_id = 9045;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            string filePath = "";
            if (Get_CheckPointFile(req.FileId, ref filePath, false, FileMode.Open, ref err) == false)
            {
                err = "UpdateEstCloseDate " + err;
                int Event_id = 9044;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if (filePath == string.Empty)
            {
                err = "UpdateEstCloseDate Point filepath is empty.";
                int Event_id = 9043;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            #endregion

            List<Framework.FieldMap> UpdatedFieldds = new List<Framework.FieldMap>();
            try
            {
                UpdatedFieldds.Clear();
                // Estimated Close Date
                AddUpdatedFields(ref UpdatedFieldds, 6075, req.NewDate.ToString("d"));
                if (pntLib.WritePointData(UpdatedFieldds, filePath, ref  err) == false)
                {
                    err = string.Format("pntLib.WritePointData return false, filePath: {0}, err: {1} ", filePath, err);
                    int Event_id = 9041;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                WorkflowManager wm = WorkflowManager.Instance;
                WorkflowEvent we = new WorkflowEvent(WorkflowCmd.UpdateEstCloseDate, req);
                wm.ProcessRequest(we);
                return true;
            }
            catch (Exception ex)
            {
                err = "Cannot update the specified Point file " + filePath + ", Exception:" + ex.Message;
                int Event_id = 9040;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (UpdatedFieldds != null)
                {
                    UpdatedFieldds.Clear();
                    UpdatedFieldds = null;
                }
                if (logErr)
                {
                    int Event_id = 9039;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
                else
                {
                    err = "Est Close Date has been changed to " + req.NewDate.ToString("d") + " for Point file " + filePath;
                    int Event_id = 9004;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id, Category);
                    Trace.TraceInformation(err);
                }
            }
        }
        public bool UpdatePointStatusDate(int fileId, string filePath, List<Table.LoanStages> stageList)
        {
            List<Framework.FieldMap> UpdatedFieldds = new List<Framework.FieldMap>();
            string err = string.Empty;
            bool logErr = false;
            try
            {
                #region check if the Point file is being opened/locked
                CheckPointFileStatusReq reqCheckPointFile = new CheckPointFileStatusReq();
                reqCheckPointFile.FileId = fileId;
                reqCheckPointFile.hdr = new ReqHdr();
                reqCheckPointFile.hdr.SecurityToken = "SecurityToken";
                reqCheckPointFile.hdr.UserId = 1;
                CheckPointFileStatusResp respCheckPointFile = CheckPointFileStatus(reqCheckPointFile);
                if (respCheckPointFile != null && (respCheckPointFile.FileLocked || !respCheckPointFile.hdr.Successful))
                {
                    err = string.Format("UpdateStage, FileId {0},  Point File {1} is locked or there is an error ({2}). ", fileId, filePath, respCheckPointFile.hdr.StatusInfo);
                    int Event_id = 9038;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                #endregion

                UpdatedFieldds.Clear();
                foreach (Table.LoanStages s in stageList)
                {
                    #region check for nulls
                    if (s == null)
                    {
                        err = string.Format("UpdatePointStatusDate StageList contains empty StageInfo, FileId {0}. ", fileId);
                        Trace.TraceError(err);
                        int Event_id = 9037;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        continue;
                    }
                    if (s.LoanStageId <= 0)
                    {
                        err = string.Format("UpdatePointStatusDate LoanStageId <= 0 for stage name{1}, FileId {0}.", fileId, s.StageName);
                        Trace.TraceError(err);
                        int Event_id = 9036;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        continue;
                    }
                    string CompletionDate = "";
                    List<string> FieldArray = new List<string>();
                    ArrayList FieldSeq = new ArrayList();
                    if (pntLib.ReadPointData(ref FieldArray, ref FieldSeq, filePath, ref err) == false)
                    {
                        Trace.TraceError(err);
                        int Event_id = 9035;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        continue;
                    }
                    if ((s.Completed != null) && (s.Completed != DateTime.MinValue))
                        CompletionDate = s.Completed.ToString("d");
                    short pointDateField = 0;
                    short pointStageField = 0;
                    string StageName = string.IsNullOrEmpty(s.StageName) ? string.Empty : s.StageName.Trim();

                    if (string.IsNullOrEmpty(StageName))
                    {
                        err = string.Format("UpdatePointStatusDate, cannot get Loan Stage Name, FileId {0}, LoanStageId {1}, StageName {2}." + fileId, s.LoanStageId, s.StageName);
                        Trace.TraceError(err);
                        int Event_id = 9034;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        continue;
                    }
                    #endregion
                    pointStageField = s.PointNameField;
                    pointDateField = s.PointDateField;
                    if (pointDateField <= 0)
                    {
                        #region no Point Date Field specified, check the standard names
                        switch (StageName)
                        {
                            case "Open":
                            case PointStage.Application:
                                pointDateField = (short)PointStageDateField.Application;
                                break;
                            case "Processing":
                            case "Sent To Processing":
                            case PointStage.SentToProcessing:
                                pointDateField = (short)PointStageDateField.SentToProcessing;
                                break;
                            case "HMDA":
                            case PointStage.HMDACompleted:
                            case PointStage.HMDAComplete:
                                pointDateField = (short)PointStageDateField.HDMA;
                                break;
                            case PointStage.Submit:
                            case PointStage.Submitted:
                                pointDateField = (short)PointStageDateField.Submitted;
                                break;
                            case PointStage.Approve:
                            case PointStage.Approved:
                                pointDateField = (short)PointStageDateField.Approved;
                                break;
                            case PointStage.Resubmit:
                            case PointStage.Re_submit:
                            case PointStage.Resubmitted:
                                pointDateField = (short)PointStageDateField.Resubmitted;
                                break;
                            case PointStage.CleartoClose:
                            case PointStage.ClearToClose:
                                pointDateField = (short)PointStageDateField.ClearedToClose;
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
                            case PointStage.Funded:
                                pointDateField = (short)PointStageDateField.Funded;
                                break;
                            case PointStage.Record:
                            case PointStage.Recorded:
                                pointDateField = (short)PointStageDateField.Recorded;
                                break;
                            case PointStage.Close:
                            case LoanStatus.LoanStatus_Closed:
                                pointDateField = (short)PointStageDateField.Closed;
                                break;
                            case LoanStatus.LoanStatus_Canceled:
                                pointDateField = (short)PointStageDateField.Canceled;
                                break;
                            case LoanStatus.LoanStatus_Denied:
                                pointDateField = (short)PointStageDateField.Denied;
                                break;
                            case LoanStatus.LoanStatus_Suspended:
                            case PointStage.Suspend:
                                pointDateField = (short)PointStageDateField.Suspended;
                                break;
                            default:
                                pointDateField = -1;
                                break;
                        }
                        #endregion
                    }
                    if (pointDateField > 0)
                    {
                        string pointStatusDate = pntLib.getPointField(FieldArray, FieldSeq, pointDateField);
                        if (string.IsNullOrEmpty(pointStatusDate))
                            pointStatusDate = string.Empty;
                        if (string.IsNullOrEmpty(CompletionDate))
                            CompletionDate = string.Empty;
                        if (pointStatusDate != CompletionDate)
                            AddUpdatedFields(ref UpdatedFieldds, pointDateField, CompletionDate);
                    }
                    if (pointStageField > 0 && !string.IsNullOrEmpty(StageName))
                    {
                        string pointStatusName = pntLib.getPointField(FieldArray, FieldSeq, pointDateField);
                        if (string.IsNullOrEmpty(pointStatusName))
                            pointStatusName = string.Empty;
                        if (pointStatusName != StageName)
                            AddUpdatedFields(ref UpdatedFieldds, pointStageField, StageName);
                    }
                }

            }
            catch (Exception ex)
            {
                err = string.Format("UpdatePointStatusDate, Filepath={0}, Exception: {1} ", filePath, ex.ToString());
                int Event_id = 9033;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9032;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
            if (UpdatedFieldds.Count <= 0)
                return true;
            return pntLib.WritePointData(UpdatedFieldds, filePath, ref  err);
        }
        public bool UpdateStage(UpdateStageRequest req, ref string err)
        {
            err = "";
            bool logErr = false;
            bool fatal = false;
            #region check header info
            if ((req == null) || (req.hdr == null))
            {
                err = "Invalid UpdateStage Request, request is null. ";
                int Event_id = 9031;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if ((req.FileId <= 0) || (req.hdr.UserId < 0))
            {
                err = "Missing FileId or User Id in the UpdateStageRequest.";
                int Event_id = 9030;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if ((req.StageList == null) || (req.StageList.Count <= 0))
            {
                err = string.Format("Missing StageList or the StageList is empty in the UpdateStageRequest, FileId {0}.", req.FileId);
                int Event_id = 9029;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            #endregion
            #region get Point file info
            Table.PointFileInfo fileInfo = da.GetPointFileInfo(req.FileId, ref err);
            if (fileInfo == null || fileInfo.FolderId <= 0)
            {
                err = string.Format("UpdateStage Error {0} , FileId {1}. ", err, req.FileId);
                //int Event_id = 9028;           
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);              
                return false;
            }

            if (fileInfo.FolderId <= 0 || string.IsNullOrEmpty(fileInfo.Path) || string.IsNullOrEmpty(Path.GetFileName(fileInfo.Path)))
            {
                err = string.Format("UpdateStage FolderId is invalid or Point filepath is empty , FileId {0}, FolderId {1}, Path {2}. ", req.FileId, fileInfo.FolderId, fileInfo.Path);
                int Event_id = 9027;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            DataAccess.PointFolderInfo pf = da.GetPointFolderInfo(fileInfo.FolderId, ref err);
            if (pf == null)
            {
                err = string.Format("UpdateStage Error {0} , FileId {1}, FolderId {2}. ", err, req.FileId, fileInfo.FolderId);
                int Event_id = 9026;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if ((LoanStatusEnum)pf.LoanStatus == LoanStatusEnum.Prospect || (LoanStatusEnum)pf.LoanStatus == LoanStatusEnum.ProspectArchive)
                return true;
            #endregion
            string filePath = fileInfo.Path;

            try
            {
                List<Table.LoanStages> listLoanStages = da.GetLoanStagesByFileId(req.FileId, ref err);
                if (listLoanStages == null)
                {
                    err = string.Format("UpdateStage, Cannot update the Point file, fileId {0}, Path {1}, LoanStage List is null.", req.FileId, filePath);
                    int Event_id = 9025;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                return (UpdatePointStatusDate(req.FileId, filePath, listLoanStages));
            }
            catch (Exception ex)
            {
                err = string.Format("UpdateStage, Cannot update the Point file, fileId {0}, Path {1}, Exception: {2}", req.FileId, filePath, ex.Message);
                int Event_id = 9024;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9023;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
            }
        }

        private void UpdatePointContactFields_Assets(Table.Contacts contactInfo, ref List<Framework.FieldMap> UpdatedFieldds, string RoleNmae)
        {
            int idx = 0;

            Table.Assets assets_temp = new Table.Assets();

            if (RoleNmae == ContactRoles.ContactRole_Borrower)
            {
                idx = contactInfo.assets.Count;
                if (idx <= 0)
                    return;
                AddUpdatedFields(ref UpdatedFieldds, 1280, idx.ToString());
                short nmFld = 1294, noFld = 1298, vFld = 1299, tFld = 8880;

                for (int i = 0; i < idx; i++)
                {

                    if (i >= 11)
                    {
                        nmFld = 3854;
                        noFld = 3858;
                        vFld = 3859;
                    }

                    assets_temp = null;
                    assets_temp = new Table.Assets();
                    assets_temp = contactInfo.assets[i];

                    AddUpdatedFields(ref UpdatedFieldds, (short)(nmFld), assets_temp.Name);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(noFld), assets_temp.Account);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(vFld), assets_temp.Amount.ToString());
                    AddUpdatedFields(ref UpdatedFieldds, (short)(tFld), assets_temp.Type);

                    nmFld = (short)(nmFld + 6);
                    noFld = (short)(noFld + 6);
                    vFld = (short)(vFld + 6);
                    tFld = (short)(tFld + 1);
                }
            }

            if (RoleNmae == ContactRoles.ContactRole_Coborrower)
            {


            }
        }

        private void UpdatePointContactFields_Employment(Table.Contacts contactInfo, ref List<Framework.FieldMap> UpdatedFieldds, string RoleNmae)
        {
            int idx = 0;

            Table.Employment employment_temp = new Table.Employment();

            if (RoleNmae == ContactRoles.ContactRole_Borrower)
            {
                idx = contactInfo.employment.Count;

                if (idx == 0)
                    return;

                employment_temp = null;
                employment_temp = new Table.Employment();
                employment_temp = contactInfo.employment[0];

                AddUpdatedFields(ref UpdatedFieldds, (short)(148), employment_temp.CompanyName);
                AddUpdatedFields(ref UpdatedFieldds, (short)(149), employment_temp.Address);
                AddUpdatedFields(ref UpdatedFieldds, (short)(140), employment_temp.City);

                AddUpdatedFields(ref UpdatedFieldds, (short)(142), employment_temp.State);
                AddUpdatedFields(ref UpdatedFieldds, (short)(143), employment_temp.Zip);

                if (employment_temp.SelfEmployed == true)
                {
                    AddUpdatedFields(ref UpdatedFieldds, (short)(141), "X");
                }

                AddUpdatedFields(ref UpdatedFieldds, (short)(135), employment_temp.Position);
                AddUpdatedFields(ref UpdatedFieldds, (short)(136), employment_temp.Phone);
                AddUpdatedFields(ref UpdatedFieldds, (short)(138), employment_temp.YearsOnWork.ToString());

                short fld = 5220;
                short nfld = 5546;

                string startdate_temp = "";
                string enddate_temp = "";

                int j = 0;

                for (int i = 1; i < idx; i++)
                {
                    employment_temp = null;
                    employment_temp = new Table.Employment();
                    employment_temp = contactInfo.employment[i];

                    AddUpdatedFields(ref UpdatedFieldds, (short)(fld), employment_temp.CompanyName);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(fld + 1), employment_temp.Address);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(fld + 2), employment_temp.City);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(fld + 3), employment_temp.State);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(fld + 4), employment_temp.Zip);

                    fld = (short)(fld + 5);

                    j = i - 1;

                    switch (j)
                    {
                        case 0: nfld = 5546; break;
                        case 1: nfld = 5556; break;
                        case 2: nfld = 5586; break;
                        case 3: nfld = 5606; break;
                        case 4: nfld = 5616; break;
                        case 5: nfld = 5626; break;
                        case 6: nfld = 5636; break;
                        case 7: nfld = 5646; break;
                        default: break;
                    }

                    startdate_temp = employment_temp.StartMonth.ToString() + "\\" + employment_temp.StartYear.ToString();

                    AddUpdatedFields(ref UpdatedFieldds, (short)(nfld), startdate_temp);

                    enddate_temp = employment_temp.EndMonth.ToString() + "\\" + employment_temp.EndYear.ToString();

                    AddUpdatedFields(ref UpdatedFieldds, (short)(nfld + 1), enddate_temp);

                    AddUpdatedFields(ref UpdatedFieldds, (short)(nfld - 2), employment_temp.Position);

                    AddUpdatedFields(ref UpdatedFieldds, (short)(nfld - 1), employment_temp.Phone);

                    if (employment_temp.SelfEmployed == true)
                    {
                        AddUpdatedFields(ref UpdatedFieldds, (short)(nfld - 6), "X");
                    }
                }
            }

            if (RoleNmae == ContactRoles.ContactRole_Coborrower)
            {
                idx = contactInfo.employment.Count;

                if (idx == 0)
                    return;

                employment_temp = null;
                employment_temp = new Table.Employment();
                employment_temp = contactInfo.employment[0];

                AddUpdatedFields(ref UpdatedFieldds, (short)(198), employment_temp.CompanyName);
                AddUpdatedFields(ref UpdatedFieldds, (short)(199), employment_temp.Address);
                AddUpdatedFields(ref UpdatedFieldds, (short)(190), employment_temp.City);

                AddUpdatedFields(ref UpdatedFieldds, (short)(192), employment_temp.State);
                AddUpdatedFields(ref UpdatedFieldds, (short)(193), employment_temp.Zip);

                if (employment_temp.SelfEmployed == true)
                {
                    AddUpdatedFields(ref UpdatedFieldds, (short)(191), "X");
                }

                AddUpdatedFields(ref UpdatedFieldds, (short)(185), employment_temp.Position);
                AddUpdatedFields(ref UpdatedFieldds, (short)(186), employment_temp.Phone);
                AddUpdatedFields(ref UpdatedFieldds, (short)(188), employment_temp.YearsOnWork.ToString());

                short fld = 5260;
                short nfld = 5566;

                string startdate_temp = "";
                string enddate_temp = "";

                int j = 0;

                for (int i = 1; i < idx; i++)
                {
                    employment_temp = null;
                    employment_temp = new Table.Employment();
                    employment_temp = contactInfo.employment[i];

                    j = i - 1;

                    fld = (short)(5260 + (j * 5));

                    if (j == 6)
                        fld = 5010;

                    if (j == 7)
                        fld = 5015;

                    AddUpdatedFields(ref UpdatedFieldds, (short)(fld), employment_temp.CompanyName);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(fld + 1), employment_temp.Address);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(fld + 2), employment_temp.City);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(fld + 3), employment_temp.State);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(fld + 4), employment_temp.Zip);

                    switch (j)
                    {
                        case 0: fld = 5566; break;
                        case 1: fld = 5576; break;
                        case 2: fld = 5596; break;
                        case 3: fld = 5656; break;
                        case 4: fld = 5666; break;
                        case 5: fld = 5676; break;
                        case 6: fld = 5686; break;
                        case 7: fld = 5696; break;
                        default: break;
                    }

                    startdate_temp = employment_temp.StartMonth.ToString() + "\\" + employment_temp.StartYear.ToString();

                    AddUpdatedFields(ref UpdatedFieldds, (short)(nfld), startdate_temp);

                    enddate_temp = employment_temp.EndMonth.ToString() + "\\" + employment_temp.EndYear.ToString();

                    AddUpdatedFields(ref UpdatedFieldds, (short)(nfld + 1), enddate_temp);

                    AddUpdatedFields(ref UpdatedFieldds, (short)(nfld - 2), employment_temp.Position);

                    AddUpdatedFields(ref UpdatedFieldds, (short)(nfld - 1), employment_temp.Phone);

                    if (employment_temp.SelfEmployed == true)
                    {
                        AddUpdatedFields(ref UpdatedFieldds, (short)(nfld - 6), "X");
                    }
                }
            }
        }

        private void UpdatePointContactFields_Income(Table.Contacts contactInfo, ref List<Framework.FieldMap> UpdatedFieldds, string RoleNmae)
        {

            if (RoleNmae == ContactRoles.ContactRole_Borrower)
            {
                AddUpdatedFields(ref UpdatedFieldds, (short)600, contactInfo.income.Salary.ToString());
                AddUpdatedFields(ref UpdatedFieldds, (short)601, contactInfo.income.Overtime.ToString());
                AddUpdatedFields(ref UpdatedFieldds, (short)602, contactInfo.income.Bonuses.ToString());
                AddUpdatedFields(ref UpdatedFieldds, (short)603, contactInfo.income.Commission.ToString());
                AddUpdatedFields(ref UpdatedFieldds, (short)604, contactInfo.income.Div_Int.ToString());
                AddUpdatedFields(ref UpdatedFieldds, (short)605, contactInfo.income.NetRent.ToString());
                AddUpdatedFields(ref UpdatedFieldds, (short)606, contactInfo.income.Other.ToString());
            }

            if (RoleNmae == ContactRoles.ContactRole_Coborrower)
            {
                AddUpdatedFields(ref UpdatedFieldds, (short)650, contactInfo.income.Salary.ToString());
                AddUpdatedFields(ref UpdatedFieldds, (short)651, contactInfo.income.Overtime.ToString());
                AddUpdatedFields(ref UpdatedFieldds, (short)652, contactInfo.income.Bonuses.ToString());
                AddUpdatedFields(ref UpdatedFieldds, (short)653, contactInfo.income.Commission.ToString());
                AddUpdatedFields(ref UpdatedFieldds, (short)654, contactInfo.income.Div_Int.ToString());
                AddUpdatedFields(ref UpdatedFieldds, (short)655, contactInfo.income.NetRent.ToString());
                AddUpdatedFields(ref UpdatedFieldds, (short)656, contactInfo.income.Other.ToString());
            }

        }

        private void UpdatePointContactFields_OtherIncome(Table.Contacts contactInfo, ref List<Framework.FieldMap> UpdatedFieldds, string RoleNmae)
        {
            int idx = 0;

            Table.OtherIncome otherincome_temp = new Table.OtherIncome();

            if (RoleNmae == ContactRoles.ContactRole_Borrower)
            {
                short fieldId = 1230;

                idx = contactInfo.otherincome.Count;

                for (int i = 0; i < idx; i++)
                {
                    otherincome_temp = null;
                    otherincome_temp = new Table.OtherIncome();
                    otherincome_temp = contactInfo.otherincome[i];

                    AddUpdatedFields(ref UpdatedFieldds, (short)(fieldId), otherincome_temp.RoleType);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(fieldId + 1), otherincome_temp.Type);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(fieldId + 2), otherincome_temp.MonthlyIncome.ToString());
                    fieldId = (short)(fieldId + 3);
                }
            }

            if (RoleNmae == ContactRoles.ContactRole_Coborrower)
            {
                short fieldId = 1230;

                idx = contactInfo.otherincome.Count;

                for (int i = 0; i < idx; i++)
                {
                    otherincome_temp = null;
                    otherincome_temp = new Table.OtherIncome();
                    otherincome_temp = contactInfo.otherincome[i];

                    AddUpdatedFields(ref UpdatedFieldds, (short)(fieldId), otherincome_temp.RoleType);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(fieldId + 1), otherincome_temp.Type);
                    AddUpdatedFields(ref UpdatedFieldds, (short)(fieldId + 2), otherincome_temp.MonthlyIncome.ToString());
                    fieldId = (short)(fieldId + 3);
                }
            }
        }

        private void UpdatePointContactFields(Table.Contacts contactInfo, ref object updateFields, string RoleNmae)
        {
            #region initialize  & error checking
            string err = "";
            short fullName = 0;
            short firstName = 0;
            short middleName = 0;
            short nickName = 0;
            short lastName = 0;
            short SSN = 0;
            short genCode = 0;
            short DOB = 0;
            short homePhone = 0;
            short cellPhone = 0;
            short businessPhone = 0;
            short fax = 0;
            short email = 0;
            short addr = 0;
            short city = 0;
            short state = 0;
            short zip = 0;
            short company = 0;
            bool logErr = false;

            if (updateFields == null)
            {
                err = "UpdatePointContactFields:: object is null for List<Framework.FieldMap>.";
                int Event_id = 9022;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            List<Framework.FieldMap> UpdatedFieldds = updateFields as List<Framework.FieldMap>;
            if (UpdatedFieldds == null)
            {
                err = "UpdatePointContactFields:: UpdatedFieldds is null for List<Framework.FieldMap>.";
                int Event_id = 9021;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            #endregion
            #region setting the Point Field Ids for the different Roles
            List<Framework.FieldMap> fieldMap = updateFields as List<Framework.FieldMap>;
            switch (RoleNmae)
            {
                case ContactRoles.ContactRole_Borrower:
                    if (fieldMap != null)
                    {
                        AddBorrowerFields(ref fieldMap, contactInfo, false);
                        return;
                    }
                    firstName = 100;
                    middleName = 117;
                    nickName = 11234;
                    lastName = 101;
                    genCode = 119;
                    SSN = 108;
                    homePhone = 106;
                    cellPhone = 139;
                    businessPhone = 136;
                    fax = 107;
                    email = 112;
                    DOB = 118;
                    addr = 102;
                    city = 103;
                    state = 104;
                    zip = 105;
                    break;
                case ContactRoles.ContactRole_Coborrower:
                    if (fieldMap != null)
                    {
                        AddBorrowerFields(ref fieldMap, contactInfo, true);
                        return;
                    }
                    firstName = 150;
                    middleName = 167;
                    lastName = 151;
                    genCode = 169;
                    nickName = 11236;
                    SSN = 158;
                    DOB = 168;
                    email = 162;
                    homePhone = 156;
                    businessPhone = 186;
                    cellPhone = 189;
                    fax = 157;
                    break;
                case ContactRoles.ContactRole_Appraiser:
                    fullName = 330;
                    company = 331;
                    addr = 333;
                    city = 334;
                    businessPhone = 332;
                    cellPhone = 12367;
                    fax = 335;
                    email = 12368;
                    break;
                case ContactRoles.ContactRole_Builder:
                    fullName = 360;
                    company = 361;
                    addr = 363;
                    city = 364;
                    fax = 368;
                    businessPhone = 362;
                    cellPhone = 12381;
                    email = 12382;
                    break;
                case ContactRoles.ContactRole_BuyersAgent:
                    fullName = 6191;
                    company = 6196;
                    addr = 6197;
                    city = 6198;
                    businessPhone = 6192;
                    cellPhone = 6194;
                    fax = 6193;
                    email = 6195;
                    break;
                case ContactRoles.ContactRole_Closing:
                    fullName = 6110;
                    company = 6111;
                    addr = 6113;
                    city = 6114;
                    businessPhone = 6112;
                    cellPhone = 12369;
                    fax = 6115;
                    email = 6119;
                    break;
                case ContactRoles.ContactRole_FloodInsurance:
                    fullName = 12786;
                    company = 12787;
                    addr = 12762;
                    city = 12763;
                    state = 12764;
                    zip = 12765;
                    businessPhone = 13000;
                    cellPhone = 13001;
                    fax = 13002;
                    email = 13003;
                    break;
                case ContactRoles.ContactRole_HazardInsurance:
                    fullName = 360;
                    company = 361;
                    addr = 363;
                    city = 364;
                    fax = 368;
                    businessPhone = 362;
                    cellPhone = 12381;
                    email = 12382;
                    break;
                case ContactRoles.ContactRole_Investor:
                    fullName = 0;
                    company = 7341;
                    addr = 7343;
                    city = 7344;
                    state = 7345;
                    zip = 7346;
                    fax = 7348;
                    businessPhone = 7342;
                    cellPhone = 12363;
                    email = 12364;
                    break;
                case ContactRoles.ContactRole_Lender:
                    fullName = 6000;
                    company = 6001;
                    addr = 6003;
                    city = 6004;
                    fax = 6005;
                    businessPhone = 6002;
                    cellPhone = 12357;
                    email = 12358;
                    break;
                case ContactRoles.ContactRole_ListingAgent:
                    fullName = 6130;
                    company = 6131;
                    addr = 6133;
                    city = 6134;
                    fax = 6135;
                    businessPhone = 6132;
                    cellPhone = 12377;
                    email = 12378;
                    break;
                case ContactRoles.ContactRole_MortgageBroker:
                    fullName = 6371;
                    company = 6370;
                    addr = 6372;
                    city = 6373;
                    fax = 6375;
                    businessPhone = 6374;
                    cellPhone = 12355;
                    email = 12356;
                    break;
                case ContactRoles.ContactRole_MortgageInsurance:
                    fullName = 460;
                    company = 463;
                    addr = 464;
                    city = 465;
                    fax = 462;
                    businessPhone = 461;
                    cellPhone = 12387;
                    email = 12388;
                    break;
                case ContactRoles.ContactRole_PropertyTax:
                    fullName = 12968;
                    company = 13029;
                    addr = 12964;
                    city = 12965;
                    state = 12966;
                    zip = 12967;
                    fax = 13006;
                    businessPhone = 13004;
                    cellPhone = 13005;
                    email = 13007;
                    break;
                case ContactRoles.ContactRole_SellersAttorney:
                    fullName = 440;
                    company = 441;
                    addr = 443;
                    city = 444;
                    fax = 445;
                    businessPhone = 442;
                    cellPhone = 12375;
                    email = 12376;
                    break;
                case ContactRoles.ContactRole_SellingAgent:
                    fullName = 6140;
                    company = 6141;
                    addr = 6143;
                    city = 6144;
                    fax = 6145;
                    businessPhone = 6142;
                    cellPhone = 12379;
                    email = 12380;
                    break;
                case ContactRoles.ContactRole_Surveyor:
                    fullName = 470;
                    company = 473;
                    addr = 474;
                    city = 475;
                    fax = 472;
                    businessPhone = 471;
                    cellPhone = 12389;
                    email = 12390;
                    break;
                case ContactRoles.ContactRole_Title:
                    fullName = 6120;
                    company = 6121;
                    addr = 6123;
                    city = 6124;
                    fax = 6125;
                    businessPhone = 6122;
                    cellPhone = 12371;
                    email = 12372;
                    break;


                default:
                    return;
            }
            #endregion
            try
            {
                UpdatedFieldds.Clear();
                if ((RoleNmae != ContactRoles.ContactRole_Borrower) && (RoleNmae != ContactRoles.ContactRole_Coborrower))
                {
                    AddUpdatedFields(ref UpdatedFieldds, fullName, contactInfo.FirstName + " " + contactInfo.MiddleName + " " + contactInfo.LastName);
                    AddUpdatedFields(ref UpdatedFieldds, addr, contactInfo.MailingAddr);
                    if (state == 0 && zip == 0 & city > 0)
                        AddUpdatedFields(ref UpdatedFieldds, city, contactInfo.MailingCity + " ," + contactInfo.MailingState + " " + contactInfo.MailingZip);
                    if (state > 0)
                        AddUpdatedFields(ref UpdatedFieldds, state, contactInfo.MailingState);
                    if (zip > 0)
                        AddUpdatedFields(ref UpdatedFieldds, zip, contactInfo.MailingZip);
                }
                else
                {
                    AddUpdatedFields(ref UpdatedFieldds, firstName, contactInfo.FirstName);
                    AddUpdatedFields(ref UpdatedFieldds, lastName, contactInfo.LastName);

                    if (RoleNmae != ContactRoles.ContactRole_Coborrower)
                    {
                        AddUpdatedFields(ref UpdatedFieldds, addr, contactInfo.MailingAddr);
                        AddUpdatedFields(ref UpdatedFieldds, city, contactInfo.MailingCity);
                        AddUpdatedFields(ref UpdatedFieldds, state, contactInfo.MailingState);
                        AddUpdatedFields(ref UpdatedFieldds, zip, contactInfo.MailingZip);
                    }
                    UpdatePointContactFields_Assets(contactInfo, ref UpdatedFieldds, RoleNmae);
                    UpdatePointContactFields_Employment(contactInfo, ref UpdatedFieldds, RoleNmae);
                    UpdatePointContactFields_Income(contactInfo, ref UpdatedFieldds, RoleNmae);
                    UpdatePointContactFields_OtherIncome(contactInfo, ref UpdatedFieldds, RoleNmae);
                }
                AddUpdatedFields(ref UpdatedFieldds, businessPhone, contactInfo.BusinessPhone);
                AddUpdatedFields(ref UpdatedFieldds, cellPhone, contactInfo.CellPhone);
                AddUpdatedFields(ref UpdatedFieldds, fax, contactInfo.Fax);
                AddUpdatedFields(ref UpdatedFieldds, email, contactInfo.Email);
                if (company > 0)
                    AddUpdatedFields(ref UpdatedFieldds, company, contactInfo.CompanyName);

                if (RoleNmae != ContactRoles.ContactRole_Borrower && RoleNmae != ContactRoles.ContactRole_Coborrower)
                    return;

                // all optional fields that are specific to borrower and co-borrower
                AddUpdatedFields(ref UpdatedFieldds, middleName, contactInfo.MiddleName);
                AddUpdatedFields(ref UpdatedFieldds, nickName, contactInfo.NickName);
                AddUpdatedFields(ref UpdatedFieldds, homePhone, contactInfo.HomePhone);
                if (contactInfo.DOB != DateTime.MinValue)
                    AddUpdatedFields(ref UpdatedFieldds, DOB, contactInfo.DOB.Date.ToString());
                else AddUpdatedFields(ref UpdatedFieldds, DOB, string.Empty);
                if (SSN > 0)
                    AddUpdatedFields(ref UpdatedFieldds, SSN, contactInfo.SSN);
                if (genCode > 0)
                    AddUpdatedFields(ref UpdatedFieldds, genCode, contactInfo.GenerationCode);
            }
            catch (Exception ex)
            {
                err = "UpdatePointContactFields got an exception: " + ex.Message;
                int Event_id = 9020;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9019;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }
        private bool UpdatePointContact(int ContactId, ref string err)
        {
            string filePath = "";
            Table.Contacts contactInfo = da.GetContactDetail(ContactId, ref err);
            if (contactInfo == null)
                return false;

            List<string> UpdateContactList = null;
            List<Framework.FieldMap> BorrowerFieldIds = new List<Framework.FieldMap>();
            List<Framework.FieldMap> CoborrowerFieldIds = new List<Framework.FieldMap>();
            List<Framework.FieldMap> ContactFieldIds = new List<Framework.FieldMap>();
            try
            {
                #region Get the Contact Roles and FileIds in the LoanContacts table for this contact
                bool hasPartnerContact = false;
                UpdateContactList = da.GetUpdateContactList(ContactId, ref hasPartnerContact, ref err);
                if (UpdateContactList == null)
                    return true;
                Table.Contacts partnerContactInfo = new Table.Contacts();
                if (hasPartnerContact)
                {
                    partnerContactInfo.ContactId = ContactId;
                    partnerContactInfo.FirstName = contactInfo.FirstName;
                    partnerContactInfo.LastName = contactInfo.LastName;
                    partnerContactInfo.MiddleName = contactInfo.MiddleName;
                    partnerContactInfo.BusinessPhone = contactInfo.BusinessPhone;
                    partnerContactInfo.CellPhone = contactInfo.CellPhone;
                    partnerContactInfo.Email = contactInfo.Email;
                    partnerContactInfo.Fax = contactInfo.Fax;
                    partnerContactInfo.ContactBranchId = contactInfo.ContactBranchId;
                    partnerContactInfo.ContactCompanyId = contactInfo.ContactCompanyId;
                    partnerContactInfo.MailingAddr = contactInfo.MailingAddr;
                    partnerContactInfo.MailingCity = contactInfo.MailingCity;
                    partnerContactInfo.MailingState = contactInfo.MailingState;
                    partnerContactInfo.MailingZip = contactInfo.MailingZip;
                }
                #endregion

                foreach (string str in UpdateContactList)
                {
                    if (string.IsNullOrEmpty(str))
                        continue;
                    string[] temp = str.Split(';');
                    if (temp.Length < 1)
                        continue;
                    if (string.IsNullOrEmpty(temp[0].Trim()) || string.IsNullOrEmpty(temp[1].Trim()))
                        continue;
                    string RoleName = temp[0].Trim();
                    string sFileId = temp[1].Trim();
                    if (string.IsNullOrEmpty(RoleName) || string.IsNullOrEmpty(sFileId))
                        continue;

                    filePath = "";
                    int FileId = Convert.ToInt32(sFileId);
                    if (FileId <= 0)
                        continue;
                    if (Get_CheckPointFile(FileId, ref filePath, false, FileMode.Open, ref err) == false)
                    {
                        err = "UpdatePointContact " + err;
                        int Event_id = 9017;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        Trace.TraceError(err);
                        continue;
                    }
                    if (filePath == string.Empty)
                        continue;

                    object obj = null;

                    if (RoleName == ContactRoles.ContactRole_Borrower ||
                        RoleName == ContactRoles.ContactRole_Coborrower)
                    {
                        Table.Contacts Borrower = null;
                        Table.Contacts CoBorrower = null;
                        GetBorrowerCoBorrowerInfo(FileId, ref Borrower, ref CoBorrower, ref err);
                        BorrowerFieldIds.Clear();
                        AddBorrowerFields(ref BorrowerFieldIds, Borrower, false);
                        AddBorrowerFields(ref BorrowerFieldIds, CoBorrower, true);
                        AddUpdatedFields(ref BorrowerFieldIds, 1, Path.GetFileNameWithoutExtension(filePath));
                        if (pntLib.WritePointData(BorrowerFieldIds, filePath, ref  err) == false)
                        {
                            int Event_id = 9016;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                            Trace.TraceError(err);
                        }
                        continue;
                    }

                    ContactFieldIds.Clear();
                    obj = ContactFieldIds;
                    UpdatePointContactFields(contactInfo, ref obj, RoleName);

                    if (pntLib.WritePointData(BorrowerFieldIds, filePath, ref  err) == false)
                    {
                        int Event_id = 9015;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        Trace.TraceError(err);
                    }
                    continue;
                }

                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                int Event_id = 9014;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
        }

        public bool UpdateBorrower(UpdateBorrowerRequest req, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                #region Check Paramters and errors
                if ((req == null) || (req.hdr == null))
                {
                    err = "Invalid UpdateBorrower Request, request is null. ";
                    int Event_id = 9012;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                if ((req.ContactId <= 0) || (req.hdr.UserId <= 0))
                {
                    err = "Missing Contact Id or User Id in the UpdateBorrower Request.";
                    int Event_id = 9011;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                #endregion
                if (UpdatePointContact(req.ContactId, ref err) == false)
                {
                    int Event_id = 9010;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "UpdateBorrower, exception:" + ex.Message;
                int Event_id = 9009;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                    Trace.TraceError(err);
                }
            }
        }

        private void AddLoanFields(ref List<FieldMap> FieldMap, Record.Loans rec)
        {
            if (FieldMap == null)
                FieldMap = new List<FieldMap>();
            AddUpdatedFields(ref FieldMap, 1, rec.LoanNumber);                   //     19 
            AddUpdatedFields(ref FieldMap, 5, rec.DateCreated);
            AddUpdatedFields(ref FieldMap, 11, rec.LoanAmount);                   //     19
            AddUpdatedFields(ref FieldMap, 12, rec.Rate);                                   //      32
            AddUpdatedFields(ref FieldMap, 13, rec.Term);                                  //       35
            AddUpdatedFields(ref FieldMap, 31, rec.PropertyAddr);                 //     27
            AddUpdatedFields(ref FieldMap, 32, rec.PropertyCity);                      //     28            
            AddUpdatedFields(ref FieldMap, 33, rec.PropertyState);                    //     29                     
            AddUpdatedFields(ref FieldMap, 34, rec.PropertyZip);                       //      30
            AddUpdatedFields(ref FieldMap, 35, rec.County);                       //   4
            AddUpdatedFields(ref FieldMap, 41, DateTime.Today.Date.ToShortDateString());    // date opened     

            AddUpdatedFields(ref FieldMap, 524, rec.DownPay);                     //    15           
            AddUpdatedFields(ref FieldMap, 527, rec.MonthlyPayment);
            AddUpdatedFields(ref FieldMap, 540, rec.LTV);
            AddUpdatedFields(ref FieldMap, 541, rec.CLTV);                        //   3
            if (rec.InterestOnly == "1")
                AddUpdatedFields(ref FieldMap, 550, "X");                         //     22
            else AddUpdatedFields(ref FieldMap, 550, string.Empty);

            AddUpdatedFields(ref FieldMap, 700, rec.RentAmount);
            AddUpdatedFields(ref FieldMap, 800, rec.SalesPrice);                      //       34
            AddUpdatedFields(ref FieldMap, 801, rec.AppraisedValue);              //  1

            if (rec.IncludeEscrow == "1")
                AddUpdatedFields(ref FieldMap, 1176, "X");
            else AddUpdatedFields(ref FieldMap, 1176, string.Empty);

            AddUpdatedFields(ref FieldMap, 1183, string.Empty);
            AddUpdatedFields(ref FieldMap, 1184, string.Empty);
            if (rec.Joint == "1")
                AddUpdatedFields(ref FieldMap, 1183, "X");
            else
                AddUpdatedFields(ref FieldMap, 1184, "X");

            AddUpdatedFields(ref FieldMap, 3190, rec.Due);                                //       36            
            AddUpdatedFields(ref FieldMap, 6063, rec.RateLockExpiration);      //      33
            AddUpdatedFields(ref FieldMap, 6075, rec.EstCloseDate);               //     16
            AddUpdatedFields(ref FieldMap, 6380, rec.LenderNotes);                //     24
            AddUpdatedFields(ref FieldMap, 7403, rec.Program);                    //     26
            AddUpdatedFields(ref FieldMap, 7404, rec.CCScenario);                 //   2
            AddUpdatedFields(ref FieldMap, 6209, (rec.LeadRanking == "Cold" ? "Cool" : rec.LeadRanking));        // Hot, Warm, Cold-->Cool
            short FldId = 0;
            #region Lien Position

            // Lien Position
            AddUpdatedFields(ref FieldMap, 915, string.Empty);
            AddUpdatedFields(ref FieldMap, 916, string.Empty);
            AddUpdatedFields(ref FieldMap, 917, string.Empty);
            if (!string.IsNullOrEmpty(rec.LienPosition))
            {
                switch (rec.LienPosition.Trim().ToUpper())
                {
                    case "FIRST": FldId = 915;
                        break;
                    case "SECOND": FldId = 916;
                        break;
                    case "OTHER": FldId = 917;
                        break;
                    default: FldId = 915;
                        break;
                }
                AddUpdatedFields(ref FieldMap, FldId, "X");
            }
            #endregion
            #region Loan Type

            //clear the Loan type fields first
            AddUpdatedFields(ref FieldMap, 26, string.Empty);
            AddUpdatedFields(ref FieldMap, 27, string.Empty);
            AddUpdatedFields(ref FieldMap, 28, string.Empty);
            AddUpdatedFields(ref FieldMap, 29, string.Empty);
            AddUpdatedFields(ref FieldMap, 1196, string.Empty);
            FldId = 0;
            switch (rec.LoanType.Trim().ToUpper())
            {
                case "CONVENTIONAL": FldId = 26;
                    break;
                case "VA": FldId = 27;
                    break;
                case "FHA": FldId = 28;
                    break;
                case "USDA/RH": FldId = 29;
                    break;
                default: FldId = 1196;
                    break;
            }

            AddUpdatedFields(ref FieldMap, FldId, "X");
            #endregion
            #region Occupancy
            // clear the fields first
            FldId = 0;
            AddUpdatedFields(ref FieldMap, 921, string.Empty);
            AddUpdatedFields(ref FieldMap, 923, string.Empty);
            AddUpdatedFields(ref FieldMap, 924, string.Empty);
            FldId = 921;             // default to Primary Residence
            if (rec.Occupancy.Trim().ToUpper().Contains("PRIMARY"))
                FldId = 921;
            if (rec.Occupancy.Trim().ToUpper().Contains("SECONDARY"))
                FldId = 923;
            if (rec.Occupancy.Trim().ToUpper().Contains("INVESTMENT"))
                FldId = 924;
            AddUpdatedFields(ref FieldMap, FldId, "X");
            #endregion
            #region Loan Purpose
            // clear the fields first
            AddUpdatedFields(ref FieldMap, 1190, string.Empty);
            AddUpdatedFields(ref FieldMap, 1191, string.Empty);
            AddUpdatedFields(ref FieldMap, 1192, string.Empty);
            AddUpdatedFields(ref FieldMap, 1193, string.Empty);
            AddUpdatedFields(ref FieldMap, 1194, string.Empty);
            AddUpdatedFields(ref FieldMap, 1198, string.Empty);
            switch (rec.Purpose.Trim().ToUpper())
            {
                case "PURCHASE": FldId = 1190;
                    break;
                case "CONSTRUCTION-PERMANENT": FldId = 1191;
                    break;
                case "CONSTRUCTION": FldId = 1192;
                    break;
                case "CASH-OUT REFINANCE": FldId = 1193;
                    break;
                case "OTHER": FldId = 1194;
                    break;
                case "NO CASH-OUT REFINANCE": FldId = 1198;
                    break;
                default: FldId = 1190;               // default to purchase
                    break;
            }

            AddUpdatedFields(ref FieldMap, FldId, "X");

            #endregion

        }

        private void AddBorrowerFields(ref List<Framework.FieldMap> FieldMap, Table.Contacts rec, bool Coborrower)
        {
            if (FieldMap == null)
                FieldMap = new List<FieldMap>();
            if (rec == null)
                return;
            if (!Coborrower)   // Borrower
            {
                AddUpdatedFields(ref FieldMap, 2, rec.LastName);
                AddUpdatedFields(ref FieldMap, 3, rec.FirstName);
                AddUpdatedFields(ref FieldMap, 100, rec.FirstName);
                AddUpdatedFields(ref FieldMap, 101, rec.LastName);
                AddUpdatedFields(ref FieldMap, 117, rec.MiddleName);
                AddUpdatedFields(ref FieldMap, 11234, rec.NickName);

                AddUpdatedFields(ref FieldMap, 11234, rec.NickName);          //   4 
                AddUpdatedFields(ref FieldMap, 119, rec.GenerationCode);   //    6
                AddUpdatedFields(ref FieldMap, 108, rec.SSN);                          //   7       

                AddUpdatedFields(ref FieldMap, 106, rec.HomePhone);             //   8      
                AddUpdatedFields(ref FieldMap, 139, rec.CellPhone);                 //   9 
                AddUpdatedFields(ref FieldMap, 136, rec.BusinessPhone);        //   10
                AddUpdatedFields(ref FieldMap, 107, rec.Fax);                            //    11
                AddUpdatedFields(ref FieldMap, 112, rec.Email);                         //    12
                if (rec.DOB != DateTime.MinValue)
                    AddUpdatedFields(ref FieldMap, 118, rec.DOB.ToShortDateString());       //    13
                else
                    AddUpdatedFields(ref FieldMap, 118, string.Empty);
                if (rec.Experian > 0)
                    AddUpdatedFields(ref FieldMap, 5032, rec.Experian.ToString());                 //    14 
                else
                    AddUpdatedFields(ref FieldMap, 5032, string.Empty);
                if (rec.TransUnion > 0)
                    AddUpdatedFields(ref FieldMap, 5034, rec.TransUnion.ToString());             //    15
                else
                    AddUpdatedFields(ref FieldMap, 5034, string.Empty);
                if (rec.Equifax > 0)
                    AddUpdatedFields(ref FieldMap, 5036, rec.Equifax.ToString());                    //    16    
                else
                    AddUpdatedFields(ref FieldMap, 5036, string.Empty);
                AddUpdatedFields(ref FieldMap, 102, rec.MailingAddr);               //    17         
                AddUpdatedFields(ref FieldMap, 103, rec.MailingCity);                 //    18           
                AddUpdatedFields(ref FieldMap, 104, rec.MailingState);              //     19             
                AddUpdatedFields(ref FieldMap, 105, rec.MailingZip);                  //     20  
                if (rec.employment != null && rec.employment.Count > 0)
                {
                    UpdatePointContactFields_Employment(rec, ref FieldMap, ContactRoles.ContactRole_Borrower);
                }
                if (rec.assets != null && rec.assets.Count > 0)
                {
                    UpdatePointContactFields_Assets(rec, ref FieldMap, ContactRoles.ContactRole_Borrower);
                }
                if (rec.income != null)
                {
                    UpdatePointContactFields_Income(rec, ref FieldMap, ContactRoles.ContactRole_Borrower);
                }
                if (rec.otherincome != null && rec.otherincome.Count > 0)
                {
                    UpdatePointContactFields_OtherIncome(rec, ref FieldMap, ContactRoles.ContactRole_Borrower);
                }
                return;
            }
            AddUpdatedFields(ref FieldMap, 150, rec.FirstName);               //   1
            AddUpdatedFields(ref FieldMap, 167, rec.MiddleName);           //   2 
            AddUpdatedFields(ref FieldMap, 151, rec.LastName);               //   3
            AddUpdatedFields(ref FieldMap, 11236, rec.NickName);           //   4 
            //         Title                                                                                5
            AddUpdatedFields(ref FieldMap, 169, rec.GenerationCode);      //  6
            AddUpdatedFields(ref FieldMap, 158, rec.SSN);                          //   7       

            AddUpdatedFields(ref FieldMap, 156, rec.HomePhone);             //   8      
            AddUpdatedFields(ref FieldMap, 189, rec.CellPhone);                 //   9 
            AddUpdatedFields(ref FieldMap, 186, rec.BusinessPhone);        //   10
            AddUpdatedFields(ref FieldMap, 157, rec.Fax);                            //    11
            AddUpdatedFields(ref FieldMap, 162, rec.Email);                         //    12
            if (rec.DOB != DateTime.MinValue)
                AddUpdatedFields(ref FieldMap, 168, rec.DOB.ToShortDateString());                          //    13
            else
                AddUpdatedFields(ref FieldMap, 168, string.Empty);
            if (rec.Experian > 0)
                AddUpdatedFields(ref FieldMap, 5033, rec.Experian.ToString());                 //    14 
            else
                AddUpdatedFields(ref FieldMap, 5033, string.Empty);
            if (rec.TransUnion > 0)
                AddUpdatedFields(ref FieldMap, 5035, rec.TransUnion.ToString());             //    15
            else
                AddUpdatedFields(ref FieldMap, 5035, string.Empty);
            if (rec.Equifax > 0)
                AddUpdatedFields(ref FieldMap, 5037, rec.Equifax.ToString());                    //    16   
            else
                AddUpdatedFields(ref FieldMap, 5037, string.Empty);
            if (rec.employment != null && rec.employment.Count > 0)
            {
                UpdatePointContactFields_Employment(rec, ref FieldMap, ContactRoles.ContactRole_Coborrower);
            }
            if (rec.assets != null && rec.assets.Count > 0)
            {
                UpdatePointContactFields_Assets(rec, ref FieldMap, ContactRoles.ContactRole_Coborrower);
            }
            if (rec.income != null)
            {
                UpdatePointContactFields_Income(rec, ref FieldMap, ContactRoles.ContactRole_Coborrower);
            }
            if (rec.otherincome != null && rec.otherincome.Count > 0)
            {
                UpdatePointContactFields_OtherIncome(rec, ref FieldMap, ContactRoles.ContactRole_Coborrower);
            }
        }
        private void GetBorrowerCoBorrowerInfo(int FileId, ref Table.Contacts Borrower, ref Table.Contacts CoBorrower, ref string err)
        {
            if (FileId <= 0)
            {
                err = "GetBorrowerCoBorrowerInfo, invalid FileId=" + FileId;
                int Event_id = 9109;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }

            string sqlCmd = string.Format("Select dbo.lpfn_GetBorrowerContactId({0})", FileId);
            object obj = DbHelperSQL.GetSingle(sqlCmd);
            if (obj == null)
            {
                err = "GetBorrowerCoBorrowerInfo,lpfn_GetBorrowerContactId return null, FileId=" + FileId;
                int Event_id = 9110;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            if (Borrower == null)
                Borrower = new Table.Contacts();
            Borrower = da.GetContactDetail((int)obj, ref err);
            // borrower is required
            if (Borrower == null)
            {
                Trace.TraceError(err);
                return;
            }

            da.GetContactDetail_Employment((int)obj, ref Borrower, ref err);
            da.GetContactDetail_Income((int)obj, ref Borrower, ref err);
            //coborrower is optional, so it's ok if it's null
            sqlCmd = string.Format("Select TOP 1 a.ContactId from LoanContacts a where a.ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId() and FileId={0}", FileId);
            object obj1 = DbHelperSQL.GetSingle(sqlCmd);
            if (obj1 != null && obj1 != DBNull.Value)
            {
                CoBorrower = da.GetContactDetail((int)obj1, ref err);
                da.GetContactDetail_Employment((int)obj1, ref CoBorrower, ref err);
                da.GetContactDetail_Income((int)obj1, ref CoBorrower, ref err);
            }
            da.GetContactDetail_OtherIncome((int)obj, ref Borrower, ref err);
            da.GetContactDetail_Assets((int)obj, ref Borrower, ref err);
        }

        private void AddTeamFields(ref List<Framework.FieldMap> UpdatedFieldds, Table.LoanTeam team)
        {
            if (!string.IsNullOrEmpty(team.Branch))
                AddUpdatedFields(ref UpdatedFieldds, 20, team.Branch);
            if (!string.IsNullOrEmpty(team.Division))
                AddUpdatedFields(ref UpdatedFieldds, 6351, team.Division);
            if (!string.IsNullOrEmpty(team.Region))
                AddUpdatedFields(ref UpdatedFieldds, 6350, team.Region);
            if (team.LoanOfficer != null)
            {
                UpdatePointTeamFields(team.LoanOfficer, ref UpdatedFieldds, UserRoles.UserRole_LoanOfficer);
                AddUpdatedFields(ref UpdatedFieldds, 4, team.LoanOfficer.FirstName + " " + team.LoanOfficer.LastName);
            }
            if (team.Processor != null)
                UpdatePointTeamFields(team.Processor, ref UpdatedFieldds, UserRoles.UserRole_Processor);
            if (team.Closer != null)
                UpdatePointTeamFields(team.Closer, ref UpdatedFieldds, UserRoles.UserRole_Closer);
            if (team.DocPrep != null)
                UpdatePointTeamFields(team.DocPrep, ref UpdatedFieldds, UserRoles.UserRole_DocPrep);
            if (team.Shipper != null)
                UpdatePointTeamFields(team.Shipper, ref UpdatedFieldds, UserRoles.UserRole_Shipper);
            if (team.Underwriter != null)
                UpdatePointTeamFields(team.Underwriter, ref UpdatedFieldds, UserRoles.UserRole_Underwriter);
        }

        private bool CreatePointFile(int fileid, string filePath, int folderId, Record.Loans loan, Table.Contacts Borrower, Table.Contacts CoBorrower, Table.LoanTeam team, ref string err)
        {
            List<Framework.FieldMap> UpdatedFieldds = new List<Framework.FieldMap>();
            err = string.Empty;
            try
            {
                UpdatedFieldds.Clear();
                AddBorrowerFields(ref UpdatedFieldds, Borrower, false);

                AddBorrowerFields(ref UpdatedFieldds, CoBorrower, true);
                if (loan != null)
                {
                    AddLoanFields(ref UpdatedFieldds, loan);
                    AddUpdatedFields(ref UpdatedFieldds, 7, filePath);
                }

                string Loan_Representative = "";
                bool st = true;

                if (team != null)
                {
                    if (team.LoanOfficer != null)
                    {
                        if ((!string.IsNullOrEmpty(team.LoanOfficer.FirstName)) &&
                             (!string.IsNullOrEmpty(team.LoanOfficer.LastName)))
                        {
                            Loan_Representative = team.LoanOfficer.FirstName + " " + team.LoanOfficer.LastName;
                            AddUpdatedFields(ref UpdatedFieldds, 4, Loan_Representative);
                            AddUpdatedFields(ref UpdatedFieldds, 19, Loan_Representative);
                            st = da.Update_Loans_LOS_LoanOfficer(fileid, Loan_Representative, ref err);
                        }
                    }
                }

                FileMode fm;
                FileStream fs;
                BinaryWriter bw;

                byte[] bytea = { 1, 0, 2, 51, 48, 255, 255 };

                if (File.Exists(filePath) == false)
                {
                    fm = FileMode.OpenOrCreate;
                    using (fs = new FileStream(filePath, fm, FileAccess.ReadWrite))
                    {
                        using (bw = new BinaryWriter(fs))
                        { bw.Write(bytea); }
                    }
                }

                if (pntLib.WritePointData(UpdatedFieldds, filePath, ref  err) == false)
                {
                    Trace.TraceError(err);
                    return false;
                }

                return da.Update_PointFiles_Name(fileid, filePath, folderId, ref err);
            }
            catch (Exception ex)
            {
                err = string.Format("CreatePointFile, Filepath: {0}, error:{1}", filePath, ex.Message);
                Trace.TraceError(err);
                return false;
            }
        }

        public bool UpdateLoanInfo(UpdateLoanInfoRequest req, ref string err)
        {
            err = "";

            bool logErr = false;
            bool fatal = false;
            bool status = false;
            List<string> FieldArray = new List<string>();
            DataAccess.PointFolderInfo pf = null;
            ArrayList FieldSeq = new ArrayList();
            if (req == null || req.FileId <= 0)
            {
                err = string.Format("Invalid UpdateLoanInfo Request, request is null or invlaid FIleid={0}. ", req.FileId);
                int Event_id = 9111;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            string filePath = "";
            string pointfiles_Name = null;
            List<Framework.FieldMap> updateFields = new List<FieldMap>();
            try
            {
                Table.PointFileInfo fileInfo = da.GetPointFileInfo(req.FileId, ref err);
                if (fileInfo == null)
                {
                    err = string.Format("UpdateLoanInfo, Error {0}, FileId {1}.", err, req.FileId);
                    int Event_id = 9112;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                filePath = fileInfo.Path;
                if (fileInfo.FolderId <= 0)
                {
                    err = string.Format("UpdateLoanInfo, Invalid FolderId {0} for FileId={1}", fileInfo.FolderId, req.FileId);
                    int Event_id = 9114;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                pf = da.GetPointFolderInfo(fileInfo.FolderId, ref err);
                if (pf == null)
                {
                    err = string.Format("UpdateLoanInfo, unable to get Point Folder Info for FolderId={0}, FileId={1}", fileInfo.FolderId, req.FileId);
                    int Event_id = 9115;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                LoanStatusEnum folderLoanStatus = (LoanStatusEnum)pf.LoanStatus;

                if (string.IsNullOrEmpty(filePath))
                {
                    err = string.Format("UpdateLoanInfo, filePath is NULL, FileId {1}, error: {0}", err, req.FileId);
                    int Event_id = 9116;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                pointfiles_Name = da.Get_PointFiles_Name(req.FileId, ref err);

                if (string.IsNullOrEmpty(pointfiles_Name))
                    filePath = Get_AutoPointFilename(req.FileId, filePath, pf);

                if (string.IsNullOrEmpty(filePath))
                {
                    err = string.Format("UpdateLoanInfo, Get_AutoPointFilename doesn't generate the filename , FileId {1}, FolderId {2}, Error: {0}", err, req.FileId, fileInfo.FolderId);
                    int Event_id = 9117;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                Table.Contacts borrower = null;
                Table.Contacts cBorrower = null;
                GetBorrowerCoBorrowerInfo(req.FileId, ref borrower, ref cBorrower, ref err);
                Record.Loans loan = da.GetLoanInfo(req.FileId, ref err);
                loan.PointFilePath = filePath;
                loan.LoanNumber = Path.GetFileNameWithoutExtension(filePath);
                Table.LoanTeam team = da.GetLoanTeamInfo(req.FileId, ref err);

                if (req.CreateFile && !File.Exists(filePath))
                {
                    status = CreatePointFile(req.FileId, filePath, fileInfo.FolderId, loan, borrower, cBorrower, team, ref err);
                    if (status == false)
                        return status;
                }

                if (borrower == null)
                {
                    err = string.Format("UpdateLoanInfo, failed to get borrower record, FileId={0}.", req.FileId);
                    int Event_id = 9118;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                AddBorrowerFields(ref updateFields, borrower, false);

                if (cBorrower != null)
                    AddBorrowerFields(ref updateFields, cBorrower, true);

                if (loan == null)
                    return false;
                AddLoanFields(ref updateFields, loan);

                if (team != null)
                    AddTeamFields(ref updateFields, team);
                status = true;
                return status;
            }
            catch (Exception ex)
            {
                err = string.Format("UpdateLoanInfo: Cannot update the specified Point file {0}, FileId {1}. Error: {2}", filePath, req.FileId, ex.Message);
                int Event_id = 9119;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (status)
                    status = da.Update_PointFiles_Name(req.FileId, filePath, pf.FolderId, ref err);

                if (status && updateFields.Count > 0)
                {
                    pntLib.WritePointData(updateFields, filePath, ref err);
                }
                if (logErr)
                {
                    int Event_id = 9120;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
            }
        }

        private bool CreateFile(CreateFileRequest req, ref string err)
        {
            err = "";
            string Name = "";
            string Pointfilename = "";
            string Borrower_FirstName = "";
            string Borrower_LastName = "";
            string Coborrower_FirstName = "";
            string Coborrower_LastName = "";
            string Loan_Number = "";
            bool Coborrower_Name_Valid = true;
            bool logErr = false;
            bool fatal = false;

            if (req == null || req.LoanDetail == null || string.IsNullOrEmpty(req.LoanDetail.PointFilename)
                || string.IsNullOrEmpty(req.LoanDetail.PointFolder))
            {
                err = "Missing req, LoanDetail, Point Folder, or Point Filename in the CreateFile Request.";
                int Event_id = 9121;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            int ContactId = -1;
            if (!int.TryParse(req.LoanDetail.Borrower, out ContactId))
            {
                err = string.Format("Invalid req.LoanDetail.Borrower {0} specified in the CreateFile Request.", req.LoanDetail.Borrower);
                int Event_id = 9122;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            string filePath = "";
            string pfilename = "";

            int FolderId = -1;
            Name = req.LoanDetail.PointFolder;

            if (int.TryParse(Name, out FolderId))
            {
                filePath = da.GetPointFolderPath(FolderId, ref err);
            }
            else
            {
                filePath = da.GetPointFolderPathByName(Name, ref err);
            }

            if (string.IsNullOrEmpty(filePath))
                filePath = Name;

            Pointfilename = req.LoanDetail.PointFilename.Trim();
            pfilename = req.LoanDetail.PointFilename.Trim().ToLower();

            if (pfilename.EndsWith(".prs"))
                filePath = filePath + @"\PROSPECT\";
            else
                filePath = filePath + @"\BORROWER\";

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            Pointfilename = Path.GetFileName(Pointfilename);
            filePath = filePath + Pointfilename;
            Loan_Number = Path.GetFileNameWithoutExtension(filePath);
            //len = Pointfilename.Trim().Length - 4;
            //Loan_Number = Pointfilename.Trim().Substring(0, len);

            int index = 0;
            string val = "";
            bool ContactId_found = false;

            val = req.LoanDetail.Borrower;

            if (ContactId > 0)
            {
                ContactId_found = da.Get_Contacts(ContactId, ref Borrower_FirstName, ref Borrower_LastName, ref err);
            }

            if (ContactId_found == false)
            {
                index = val.IndexOf(" ");
                if (index < 0)
                {
                    err = "Invalid UpdateLoanInfo Borrower Name.";
                    int Event_id = 9123;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                if (val.Substring(index - 1, 1) == ",")
                {
                    Borrower_LastName = val.Substring(0, index - 1);
                    Borrower_FirstName = val.Substring(index + 1, val.Length - index - 1);
                }
                else
                {
                    Borrower_FirstName = val.Substring(0, index);
                    Borrower_LastName = val.Substring(index + 1, val.Length - index - 1);
                }
            }

            ContactId_found = false;
            val = req.LoanDetail.Coborrower;
            if (!int.TryParse(val, out ContactId))
            {
                ContactId = -2;
            }

            if (ContactId > 0)
            {
                ContactId_found = da.Get_Contacts(ContactId, ref Coborrower_FirstName, ref Coborrower_LastName, ref err);
            }

            if (ContactId_found == false)
            {
                val = req.LoanDetail.Coborrower;

                index = val.IndexOf(" ");
                if (index < 0)
                {
                    Coborrower_Name_Valid = false;
                }
                else
                {
                    if (val.Substring(index - 1, 1) == ",")
                    {
                        Coborrower_LastName = val.Substring(0, index - 1);
                        Coborrower_FirstName = val.Substring(index + 1, val.Length - index - 1);
                    }
                    else
                    {
                        Coborrower_FirstName = val.Substring(0, index);
                        Coborrower_LastName = val.Substring(index + 1, val.Length - index - 1);
                    }
                }
            }

            List<Framework.FieldMap> UpdatedFieldds = new List<Framework.FieldMap>();
            try
            {
                UpdatedFieldds.Clear();

                AddUpdatedFields(ref UpdatedFieldds, 1, Loan_Number);
                AddUpdatedFields(ref UpdatedFieldds, 2, Borrower_LastName);
                AddUpdatedFields(ref UpdatedFieldds, 3, Borrower_FirstName);
                AddUpdatedFields(ref UpdatedFieldds, 4, req.LoanDetail.LoanOfficer);
                AddUpdatedFields(ref UpdatedFieldds, 5, DateTime.Today.Date.ToShortDateString());
                AddUpdatedFields(ref UpdatedFieldds, 7, filePath);
                AddUpdatedFields(ref UpdatedFieldds, 100, Borrower_FirstName);
                AddUpdatedFields(ref UpdatedFieldds, 101, Borrower_LastName);

                if (Coborrower_Name_Valid)
                {
                    AddUpdatedFields(ref UpdatedFieldds, 150, Coborrower_FirstName);
                    AddUpdatedFields(ref UpdatedFieldds, 151, Coborrower_LastName);
                }

                AddUpdatedFields(ref UpdatedFieldds, 19, req.LoanDetail.LoanOfficer);

                AddUpdatedFields(ref UpdatedFieldds, 11, req.LoanDetail.Amount);
                AddUpdatedFields(ref UpdatedFieldds, 12, req.LoanDetail.InterestRate);
                AddUpdatedFields(ref UpdatedFieldds, 6075, req.LoanDetail.EstimatedCloseDate);
                AddUpdatedFields(ref UpdatedFieldds, 7403, req.LoanDetail.LoanProgram);
                AddUpdatedFields(ref UpdatedFieldds, 31, req.LoanDetail.PropertyAddress);
                AddUpdatedFields(ref UpdatedFieldds, 32, req.LoanDetail.City);
                AddUpdatedFields(ref UpdatedFieldds, 33, req.LoanDetail.State);
                AddUpdatedFields(ref UpdatedFieldds, 34, req.LoanDetail.Zip);

                AddUpdatedFields(ref UpdatedFieldds, 7, filePath);
                AddUpdatedFields(ref UpdatedFieldds, 17, req.LoanDetail.ModifiedOn);
                AddUpdatedFields(ref UpdatedFieldds, 5, req.LoanDetail.CreatedOn);

                if ((req.LoanDetail.CreatedBy != "") ||
                     (req.LoanDetail.CreatedBy != null) ||
                     (req.LoanDetail.CreatedBy != string.Empty))
                    AddUpdatedFields(ref UpdatedFieldds, 25, req.LoanDetail.CreatedBy);

                FileMode fm;
                FileStream fs;
                BinaryWriter bw;

                byte[] bytea = { 1, 0, 2, 51, 48, 255, 255 };

                if (File.Exists(filePath) == false)
                {
                    fm = FileMode.OpenOrCreate;
                    fs = new FileStream(filePath, fm, FileAccess.ReadWrite);
                    {
                        bw = new BinaryWriter(fs);
                        bw.Write(bytea);
                        bw.Close();
                    }
                    fs.Close();
                }

                if (pntLib.WritePointData(UpdatedFieldds, filePath, ref  err) == false)
                {
                    int Event_id = 9124;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                err = "Cannot create the specified Point file " + filePath + ", Exception:" + ex.Message;
                int Event_id = 9125;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (UpdatedFieldds != null)
                {
                    UpdatedFieldds.Clear();
                    UpdatedFieldds = null;
                }
                if (logErr)
                {
                    int Event_id = 9126;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
                else
                {
                    err = "Point File has been created for Point file " + filePath;
                    int Event_id = 9005;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id, Category);
                    Trace.TraceInformation(err);
                }
            }
        }

        private void UpdatePointUserFields(User userInfo, ref List<Framework.FieldMap> pointFieldList, string roleName)
        {

            if (userInfo == null || string.IsNullOrEmpty(roleName))
            {
                string err = "UpdatePointUserFields, UserInfo or RoleName is not specified.";
                Trace.TraceError(err);
                int Event_id = 9127;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            if (pointFieldList == null)
                pointFieldList = new List<Framework.FieldMap>();

            Table.Users uInfo = new Table.Users();
            uInfo.Cell = userInfo.Cell;
            uInfo.EmailAddress = userInfo.Email;
            uInfo.Fax = userInfo.Fax;
            uInfo.FirstName = userInfo.Firstname;
            uInfo.LastName = userInfo.Lastname;
            uInfo.Phone = userInfo.Phone;

            UpdatePointTeamFields(uInfo, ref pointFieldList, roleName);
        }
        private void UpdatePointTeamFields(Table.Users userInfo, ref List<Framework.FieldMap> pointFieldList, string roleName)
        {
            short name = 0;
            short phone = 0;
            short fax = 0;
            short cell = 0;
            short email = 0;

            switch (roleName)
            {
                case UserRoles.UserRole_LoanOfficer:
                    name = 19;
                    phone = 6355;
                    fax = 6356;
                    cell = 6382;
                    email = 6383;
                    AddUpdatedFields(ref pointFieldList, 4, userInfo.FirstName + " " + userInfo.LastName);
                    break;
                case UserRoles.UserRole_Processor:
                    name = 18;
                    phone = 6359;
                    fax = 6360;
                    cell = 6384;
                    email = 6385;
                    break;
                case UserRoles.UserRole_Underwriter:
                    name = 942;
                    phone = 6362;
                    fax = 6364;
                    cell = 6386;
                    email = 6387;
                    break;
                case UserRoles.UserRole_Closer:
                    name = 11556;
                    phone = 11558;
                    fax = 11559;
                    cell = 11560;
                    email = 11561;
                    break;
                case UserRoles.UserRole_DocPrep:
                    name = 11550;
                    phone = 11552;
                    fax = 11553;
                    cell = 11554;
                    email = 11555;
                    break;
                case UserRoles.UserRole_Shipper:
                    name = 11562;
                    phone = 11564;
                    fax = 11565;
                    cell = 11566;
                    email = 11567;
                    break;
                default:
                    return;
            }
            AddUpdatedFields(ref pointFieldList, name, userInfo.FirstName + " " + userInfo.LastName);
            AddUpdatedFields(ref pointFieldList, phone, userInfo.Phone);
            AddUpdatedFields(ref pointFieldList, fax, userInfo.Fax);
            AddUpdatedFields(ref pointFieldList, email, userInfo.EmailAddress);
            AddUpdatedFields(ref pointFieldList, cell, userInfo.Cell);
        }

        private bool ReassignLoan(ReassignUserInfo req, int requester, ref string err)
        {
            err = "";
            bool logErr = false;
            bool fatal = false;
            #region check Reassign Loan parameters
            if (req == null || req.FileId <= 0)
            {
                err = "No ReassignUserInfo or the FileId is not valid.";
                int Event_id = 9128;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            if (req.NewUserId <= 0)
            {
                err = "Invalid Assignee's UserId " + req.NewUserId + " in the ReassignLoanRequest.";
                int Event_id = 9129;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if (req.RoleId <= 0)
            {
                err = "Invalid RoleId " + req.RoleId + " in the ReassignLoanRequest.";
                int Event_id = 9130;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            #endregion
            string filePath = "";
            try
            {
                string roleName = da.Get_RoleName(req.RoleId, ref err);
                if (roleName == String.Empty)
                {
                    err = string.Format("ReassignLoan, Error: {0}, FileId {1}, RoleName {2}, Assign to UserId {3}. ", err, req.FileId, roleName, req.NewUserId);
                    int Event_id = 9131;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                if (Get_CheckPointFile(req.FileId, ref filePath, false, FileMode.Open, ref err) == false)
                {
                    err = string.Format("ReassignLoan, Error: {0}, FileId {1}, RoleName {2}, Assign to UserId {3}. ", err, req.FileId, roleName, req.NewUserId);
                    int Event_id = 9132;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if (filePath == string.Empty)
                {
                    err = string.Format("ReassignLoan, Error: {0}, FileId {1}, RoleName {2}, Assign to UserId {3}. ", "FilePath is empty ", req.FileId, roleName, req.NewUserId);
                    int Event_id = 9133;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if (da.IsClosedLoan(req.FileId))
                {
                    err = string.Format("ReassignLoan cannot update the Point File of a closed loan, FileId={0}, {1}.", req.FileId, filePath);
                    int Event_id = 9134;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                User uInfo = da.Get_UserInfo(req.NewUserId, ref err);
                if ((uInfo == null) || (uInfo.Firstname == String.Empty) || (uInfo.Lastname == String.Empty))
                {
                    err = string.Format("ReassignLoan, Error: {0}, FileId {1}, RoleName {2}, Assign to UserId {3}. ", err, req.FileId, roleName, req.NewUserId);
                    int Event_id = 9135;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                List<FieldMap> UpdatedFieldds = new List<FieldMap>();
                UpdatePointUserFields(uInfo, ref UpdatedFieldds, roleName);
                if (UpdatedFieldds.Count > 0)
                {
                    if (pntLib.WritePointData(UpdatedFieldds, filePath, ref  err) == false)
                    {
                        err = string.Format("ReassignLoan, Error: {0}, FileId {1}, RoleName {2}, Assign to UserId {3}. ", err, req.FileId, roleName, req.NewUserId);
                        int Event_id = 9136;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                }
                if (da.ReassignLoan(req.FileId, req.RoleId, req.NewUserId, requester, ref err) == false)
                {
                    err = string.Format("ReassignLoan, Error: {0}, FileId {1}, RoleName {2}, Assign to UserId {3}. ", err, req.FileId, roleName, req.NewUserId);
                    int Event_id = 9137;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("ReassignLoan, Error: {0}, FileId {1}, RoleId {2}, Assign to UserId {3}. ", ex.Message, req.FileId, req.RoleId, req.NewUserId);
                int Event_id = 9138;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9139;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
            }
        }

        public bool ReassignLoan(ReassignLoanRequest req, ref string err)
        {
            bool status = true;
            if (req == null || req.hdr == null || req.hdr.UserId <= 0 || req.reassignUsers == null || req.reassignUsers.Count <= 0)
            {
                err = "Missing hdr, hdr.UserId or reassignUsers information in the ReassignLoanRequest.";
                int Event_id = 9140;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            foreach (ReassignUserInfo u in req.reassignUsers)
            {
                if (ReassignLoan(u, req.hdr.UserId, ref err) == false)
                {
                    status = false;
                }
            }
            return status;
        }
        private bool ReassignContact(ReassignContactInfo req, ref string err)
        {
            err = "";
            bool logErr = false;
            bool fatal = false;
            #region check Reassign Contact parameters
            if (req == null || req.FileId <= 0 || req.NewContactId <= 0 || req.ContactRoleId <= 0)
            {
                err = string.Format("Invalid ReassignContactInfo, either missing FileId={0}, NewContactId={1}, or ContactRoleId={2}.", req.FileId, req.NewContactId, req.ContactRoleId);
                int Event_id = 9141;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            string roleName = da.Get_ContactRoleName(req.ContactRoleId, ref err);
            if (roleName == String.Empty)
            {
                int Event_id = 9142;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            string filePath = "";
            if (Get_CheckPointFile(req.FileId, ref filePath, false, FileMode.Open, ref err) == false)
            {
                err = "ReassignContact " + err;
                int Event_id = 9143;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if (filePath == string.Empty)
            {
                err = "ReassignContact, unable to find Point File path for FileId " + req.FileId;
                int Event_id = 9144;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if (da.IsClosedLoan(req.FileId))
            {
                err = string.Format("ReassignContact cannot update the Point File of a closed loan, FileId={0}, {1}.", req.FileId, filePath);
                int Event_id = 9145;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            #endregion
            try
            {
                Table.Contacts contactInfo = null;
                contactInfo = da.GetContactDetail(req.NewContactId, ref err);
                if (contactInfo == null)
                    return false;

                //da.GetPartnerAddress(req.FileId, ref contactInfo, ref err);
                da.GetContactCompanyInfo(ref contactInfo, ref err);
                List<Framework.FieldMap> UpdatedFieldIds = new List<Framework.FieldMap>();
                object obj = UpdatedFieldIds;
                UpdatePointContactFields(contactInfo, ref obj, roleName);
                if (UpdatedFieldIds.Count > 0)
                {
                    if (pntLib.WritePointData(UpdatedFieldIds, filePath, ref  err) == false)
                    {
                        int Event_id = 9146;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                err = string.Format("ReassignContact, cannot update the specified Point file {0}, FileId={1}, Exception:{2}", filePath, req.FileId, ex.Message);
                int Event_id = 9147;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9148;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
            }
        }
        public bool ReassignContact(ReassignContactRequest req, ref string err)
        {
            bool status = true;
            if (req == null)
            {
                err = "Cannot reassign contact, request is empty.";
                int Event_id = 9149;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            if (req.reassignContacts == null || req.reassignContacts.Count <= 0)
            {
                err = "No reassignContacts information available ";
                int Event_id = 9150;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            foreach (ReassignContactInfo r in req.reassignContacts)
            {
                if (ReassignContact(r, ref err) == false)
                {
                    status = false;
                    continue;
                }
            }
            return status;
        }

        private string SortLoanNotes(ref string[] rawNotes, int fileId, ref string err)
        {
            //List<string> noteList = rawNotes.ToList();
            //if (noteList != null && noteList.Count > 1)
            //    noteList.Sort();
            //rawNotes = noteList.ToArray();
            string outputStr = string.Empty;
            bool note_found = false;
            DataTable workTable = new DataTable();
            workTable.Columns.Add("Created", typeof(DateTime));
            workTable.Columns.Add("Created_String", typeof(string));
            workTable.Columns.Add("Note", typeof(string));
            workTable.Columns.Add("Sender", typeof(string));
            DataTable workTable_arr = new DataTable();
            workTable_arr.Columns.Add("Created", typeof(DateTime));
            workTable_arr.Columns.Add("Created_String", typeof(string));
            workTable_arr.Columns.Add("Note", typeof(string));
            workTable_arr.Columns.Add("Sender", typeof(string));
            foreach (string note in rawNotes)
            {
                if (string.IsNullOrEmpty(note))
                    continue;
                string[] items = note.Split('|');
                if (items.Length <= 2)
                    continue;

                DateTime created = DateTime.MinValue;
                DateTime.TryParse(items[0], out created);
                if (created == DateTime.MinValue || string.IsNullOrEmpty(items[1]))
                    continue;
                DataRow dr = workTable.NewRow();

                string point_datetime_format = string.Empty;
                String format = "MM/dd/yyyy h:mmtt";
                point_datetime_format = created.ToString(format);
                dr["Created"] = created;
                dr["Created_String"] = point_datetime_format;
                dr["Note"] = items[1].Trim();
                dr["Sender"] = items[2].Trim();
                workTable.Rows.Add(dr);
                DataRow dr_arr = workTable_arr.NewRow();
                dr_arr["Created"] = created;
                dr_arr["Created_String"] = point_datetime_format;
                dr_arr["Note"] = items[1].Trim();
                dr_arr["Sender"] = items[2].Trim();
                workTable_arr.Rows.Add(dr_arr);
            }

            List<Table.LoanNotes> dbNoteList = null;

            try
            {
                dbNoteList = da.Get_LoanNotes(fileId, ref err);

                bool st = true;
                bool duplicate = false;
                DateTime dt = DateTime.MinValue;

                List<Table.LoanNotes> clean_notes = new List<Table.LoanNotes>();
                clean_notes.Clear();

                if ((dbNoteList != null) && (dbNoteList.Count > 0))
                {
                    DateTime noteTime = DateTime.MinValue;
                    foreach (Table.LoanNotes dbNote in dbNoteList)
                    {
                        duplicate = false;
                        Table.LoanNotes clean_note = dbNote;
                        if (clean_notes.Count > 0)
                        {
                            foreach (Table.LoanNotes exist_clean_note in clean_notes)
                            {
                                if ((clean_note.Created.Date == exist_clean_note.Created.Date) &&
                                    (clean_note.Created.Hour == exist_clean_note.Created.Hour) &&
                                    (clean_note.Created.Minute == exist_clean_note.Created.Minute) &&
                                    (clean_note.Note.Trim() == exist_clean_note.Note.Trim()) &&
                                    (clean_note.Sender.Trim() == exist_clean_note.Sender.Trim()))
                                {
                                    duplicate = true;
                                    st = da.DeleteLoanNotes(clean_note.NoteId, ref err);
                                    continue;
                                }
                            }
                        }
                        if (duplicate == false)
                        {
                            clean_notes.Add(clean_note);
                        }
                    }
                }

                dbNoteList = da.Get_LoanNotes(fileId, ref err);
                if ((dbNoteList != null) && (dbNoteList.Count > 0))
                {
                    DateTime noteTime = DateTime.MinValue;
                    string sender = "";
                    string note = "";
                    int noteId;
                    foreach (Table.LoanNotes dbNote in dbNoteList)
                    {
                        noteTime = dbNote.Created;
                        noteId = dbNote.NoteId;
                        sender = dbNote.Sender;
                        note = dbNote.Note;
                        note_found = false;

                        foreach (DataRow wt_dr in workTable_arr.Rows)
                        {
                            string dr_note = "";
                            dr_note = (string)wt_dr["Note"];

                            if (dr_note == note)
                            {
                                note_found = true;
                            }
                        }

                        if (note_found == false)
                        {
                            DataRow dr1 = workTable.NewRow();
                            string point_datetime_format = string.Empty;
                            String format = "MM/dd/yyyy h:mmtt";
                            point_datetime_format = noteTime.ToString(format);
                            dr1["Created"] = noteTime;
                            dr1["Created_String"] = point_datetime_format;
                            dr1["Note"] = note;
                            dr1["Sender"] = sender;
                            workTable.Rows.Add(dr1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string err_msg = ex.Message;
                int Event_id = 9151;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err_msg, EventLogEntryType.Warning, Event_id, Category);
            }

            DataRow[] arrayDataRows = workTable.Select(null, "Created DESC");
            foreach (DataRow dr1 in arrayDataRows)
            {
                outputStr += dr1["Created_String"].ToString() + "|" + dr1["Note"] + "|" + dr1["Sender"] + "^";
            }
            return outputStr;
        }

        public bool AddNote(AddNoteRequest req, ref string err)
        {
            err = "";
            bool logErr = false;
            bool fatal = false;
            #region check Add Note parameters
            if ((req == null) || (req.hdr == null))
            {
                err = "Invalid AddNote Request, request is null. ";
                int Event_id = 9152;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if ((req.FileId <= 0) || (req.hdr.UserId <= 0))
            {
                err = "Missing FileId or User Id in the AddNoteRequest.";
                int Event_id = 9153;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if ((req.Created == null) || (req.Created == DateTime.MinValue))
            {
                err = "Missing Creation Datetime  in the AddNoteRequest.";
                int Event_id = 9154;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if (req.Note == String.Empty)
            {
                err = "Missing Note content in the AddNoteRequest.";
                int Event_id = 9155;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            string correctString = req.Note.Trim().Replace("|", " ");
            req.Note = correctString.Replace("^", " ");

            if (req.Sender == String.Empty)
            {
                err = "Missing Sender in the AddNoteRequest.";
                int Event_id = 9156;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            string filePath = "";
            if (Get_CheckPointFile(req.FileId, ref filePath, false, FileMode.Open, ref err) == false)
            {
                err = "AddNote " + err;
                int Event_id = 9157;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if (filePath == string.Empty)
            {
                err = "AddNoteRequest Point filepath is empty.";
                int Event_id = 9158;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            #endregion

            List<Framework.FieldMap> UpdatedFieldds = new List<Framework.FieldMap>();
            List<string> FieldArray = new List<string>();
            ArrayList FieldSeq = new ArrayList();
            try
            {
                UpdatedFieldds.Clear();
                if (pntLib.ReadPointData(ref FieldArray, ref FieldSeq, filePath, ref err) == false)
                {
                    int Event_id = 9159;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                string tempNote = pntLib.getPointField(FieldArray, FieldSeq, 15);
                if (tempNote == null)
                {
                    tempNote = "";
                }
                else
                {
                    DateTime Point_Time = DateTime.MinValue;
                    DateTime Request_Time = DateTime.MinValue;

                    int point_len = 0;
                    int request_len = 0;

                    string[] PointNotes = tempNote.Split('^');
                    if (PointNotes.Length > 0)
                    {
                        int i;

                        for (i = 0; i < PointNotes.Length; i++)
                        {
                            string One_PointNote = PointNotes[i];
                            string[] One_PointNote_fields = One_PointNote.Split('|');
                            if (One_PointNote_fields.Length < 3)
                                continue;

                            if (One_PointNote_fields[2].Trim() == req.Sender.Trim())
                            {
                                Point_Time = DateTime.MinValue;
                                DateTime.TryParse(One_PointNote_fields[0], out Point_Time);
                                Request_Time = DateTime.MinValue;
                                DateTime.TryParse(req.NoteTime.ToString(), out Request_Time);

                                if ((Point_Time.Date == Request_Time.Date) &&
                                     (Point_Time.Hour == Request_Time.Hour) &&
                                     (Point_Time.Minute == Request_Time.Minute))
                                {
                                    if (One_PointNote_fields[1].Trim() == req.Note.Trim())
                                    {
                                        return false;
                                    }

                                    point_len = One_PointNote_fields[1].Trim().Length;
                                    request_len = req.Note.Trim().Length;

                                    if (point_len == request_len)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                if (req.NoteTime == null)
                {
                    req.NoteTime = DateTime.Now;
                }



                if (tempNote == "")
                {
                    tempNote += req.Created.ToString() + "|" + req.Note.Trim() + "|" + req.Sender.Trim();
                }
                else
                {
                    tempNote = req.NoteTime.ToString() + "|" + req.Note.Trim() + "|" + req.Sender.Trim() + "^" + tempNote;
                }

                string[] arrTempNote = tempNote.Split('^');
                string noteStr = string.Empty;
                noteStr = SortLoanNotes(ref arrTempNote, req.FileId, ref err);

                int nlen = noteStr.Length - 1;
                if (nlen > 0)
                {
                    if (noteStr[nlen] == '^')
                    {
                        noteStr = noteStr.Substring(0, nlen);
                    }
                }

                string[] tNotes = noteStr.Split('^');
                if (tNotes.Length > 0)
                {
                    int i;
                    bool duplicate = false;
                    DateTime dt = DateTime.MinValue;

                    List<string> clean_notes = new List<string>();
                    clean_notes.Clear();

                    for (i = 0; i < tNotes.Length; i++)
                    {
                        duplicate = false;
                        string clean_note = tNotes[i];
                        if (clean_notes.Count > 0)
                        {
                            foreach (string exist_clean_note in clean_notes)
                            {
                                string[] clean_note_fields = clean_note.Split('|');
                                if (clean_note_fields.Length < 3)
                                    continue;

                                string[] exist_clean_note_fields = exist_clean_note.Split('|');
                                if (exist_clean_note_fields.Length < 3)
                                    continue;

                                if (clean_note_fields[1].Trim() == "")
                                {
                                    duplicate = true;
                                    continue;
                                }

                                if ((clean_note_fields[0].Trim() == exist_clean_note_fields[0].Trim()) &&
                                    (clean_note_fields[1].Trim() == exist_clean_note_fields[1].Trim()) &&
                                    (clean_note_fields[2].Trim() == exist_clean_note_fields[2].Trim()))
                                {
                                    duplicate = true;
                                    continue;
                                }
                            }
                        }
                        if (duplicate == false)
                        {
                            clean_notes.Add(clean_note);
                        }
                    }

                    if (clean_notes.Count > 0)
                    {
                        noteStr = "";
                        int counter = 0;
                        foreach (string cn in clean_notes)
                        {
                            if (counter == 0)
                            {
                                noteStr = noteStr + cn;
                            }
                            else
                            {
                                noteStr = noteStr + "^" + cn;
                            }
                            counter = counter + 1;
                        }
                    }
                }

                AddUpdatedFields(ref UpdatedFieldds, 15, noteStr);
                if (pntLib.WritePointData(UpdatedFieldds, filePath, ref  err) == false)
                {
                    logErr = true;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "AddNoteRequest, cannot update the specified Point file " + filePath + ", Exception:" + ex.Message;
                int Event_id = 9160;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (UpdatedFieldds != null)
                {
                    UpdatedFieldds.Clear();
                    UpdatedFieldds = null;
                }
                if (logErr)
                {
                    int Event_id = 9161;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
            }
        }

        public bool ExportEmailLog(int FileId, ref List<FieldMap> UpdatedFields, string currentConvLog, ref string err)
        {
            err = "";
            bool logErr = false;

            if (UpdatedFields == null)
                UpdatedFields = new List<FieldMap>();
            List<Table.EmailLog> emailLogList = null;
            if (currentConvLog == null)
                currentConvLog = string.Empty;
            currentConvLog = currentConvLog.Trim();
            string tempBody = string.Empty;
            string sender = string.Empty;
            string emailLogIds = string.Empty;
            try
            {
                emailLogList = da.GetEmailLogList(FileId, " (Exported=0 OR Exported IS NULL) ", ref err);
                if (emailLogList == null || emailLogList.Count <= 0)
                    return true;
                foreach (Table.EmailLog el in emailLogList)
                {
                    if (el == null || el.Exported || el.EmailBody == null || el.EmailBody == string.Empty || el.LastSent == DateTime.MinValue)
                        continue;

                    if ((!el.Exported) && (el.EmailBody != string.Empty))
                    {
                        sender = err = tempBody = string.Empty;
                        tempBody = Common.HtmlUtility.StripHTML(el.EmailBody, ref err);
                        if ((tempBody == null) || (tempBody == string.Empty))
                        {
                            if (err != string.Empty)
                            {
                                Trace.TraceError(err);
                                int Event_id = 9162;
                                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                                continue;
                            }
                        }
                        if (el.Sender == string.Empty || el.Sender == null)
                            sender = el.FromEmail;
                        else
                            sender = el.Sender;
                    }
                    if (currentConvLog.Length > 0)
                        currentConvLog += "^";
                    currentConvLog += el.LastSent.Date.ToString() + "|" + "Subject: " + el.Subject + " Body:" + tempBody + "|" + sender;
                    if (emailLogIds == string.Empty || emailLogIds == null)
                        emailLogIds = el.EmailLogId.ToString();
                    else
                        emailLogIds += "," + el.EmailLogId.ToString();
                }

                FieldMap fm = new FieldMap();
                fm.FieldId = 15;
                fm.Value = currentConvLog.Trim();
                UpdatedFields.Add(fm);
                da.Update_EmailLogExported(emailLogIds, ref err);
                return true;
            }
            catch (Exception ex)
            {
                err = "ExportEmailLog, Exception:" + ex.Message;
                int Event_id = 9163;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (emailLogList != null)
                {
                    emailLogList.Clear();
                    emailLogList = null;
                }

                if (logErr)
                {
                    int Event_id = 9164;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
            }
        }
        // this is the background thread to export LoanTaskHistory from the DB --> Calyx Conversation Log
        public bool ExportTaskHistory(ref string err)
        {
            err = "";
            bool logErr = false;
            bool fatal = false;

            List<Table.LoanStatus> loanList = null;
            List<string> FieldArray = new List<string>();
            ArrayList FieldSeq = new ArrayList();
            List<FieldMap> UpdatedFields = new List<FieldMap>();
            List<Table.TaskHistory> TaskHistoryList = null;
            try
            {
                string filePath = "";
                loanList = da.GetActiveLoans(ref err);
                if (loanList == null || loanList.Count <= 0)
                {
                    err = "ExportTaskHistory, no active loans to process.";
                    //int Event_id = 9165;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                foreach (Table.LoanStatus ls in loanList)
                {
                    UpdatedFields.Clear();
                    if (TaskHistoryList != null)
                    {
                        TaskHistoryList.Clear();
                        TaskHistoryList = null;
                    }
                    TaskHistoryList = da.Get_TaskHistoryList(ls.FileId, false, ref err);
                    if ((TaskHistoryList == null) || (TaskHistoryList.Count <= 0))
                    {
                        err = "ExportTaskHistory, no task history records for FileId=" + ls.FileId;
                        //int Event_id = 9166;
                        //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        Trace.TraceError(err);
                        continue;
                    }

                    filePath = "";
                    if (Get_CheckPointFile(ls.FileId, ref filePath, false, FileMode.Open, ref err) == false)
                    {
                        err = "ExportTaskHistory " + err;
                        int Event_id = 9167;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                    if (filePath == string.Empty)
                    {
                        err = "ExportTaskHistory, unable to get Point Filepath for FileId=" + ls.FileId;
                        //int Event_id = 9168;
                        //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        Trace.TraceError(err);
                        continue;
                    }

                    if (pntLib.ReadPointData(ref FieldArray, ref FieldSeq, filePath, ref err) == false)
                    {
                        err = "ExportTaskHistory, unable to get Point Filepath for FileId=" + ls.FileId;
                        //int Event_id = 9169;
                        //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        Trace.TraceError(err);
                        continue;
                    }
                    string tempNotes = "";
                    tempNotes = pntLib.getPointField(FieldArray, FieldSeq, 15);
                    if (tempNotes == null)
                        tempNotes = "";
                    foreach (Table.TaskHistory th in TaskHistoryList)
                    {
                        if (tempNotes.Length > 0)
                            tempNotes += "^";
                        if (th != null)
                            tempNotes += th.ActivityTime.ToString() + "|" + th.ActivityName.Trim() + "|" + th.User.Trim();
                    }

                    FieldMap fm = new FieldMap();
                    fm.FieldId = 15;
                    fm.Value = tempNotes.Trim();
                    UpdatedFields.Add(fm);
                    if (pntLib.WritePointData(UpdatedFields, filePath, ref  err) == false)
                    {
                        int Event_id = 9170;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                    string tempIds = "";
                    foreach (Table.TaskHistory th in TaskHistoryList)
                    {
                        if (th != null)
                        {
                            if (tempIds.Length > 0)
                                tempIds += ";";
                            if (th.TaskHistoryId > 0)
                                tempIds += th.TaskHistoryId.ToString();
                        }
                    }
                    if (da.Update_TaskHistoryStatus(ls.FileId, tempIds, ref err) == false)
                    {
                        int Event_id = 9171;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "ExportTaskHistory, Exception:" + ex.Message;
                int Event_id = 9172;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (UpdatedFields != null)
                {
                    UpdatedFields.Clear();
                    UpdatedFields = null;
                }
                if (loanList != null)
                {
                    loanList.Clear();
                    loanList = null;
                }
                if (TaskHistoryList != null)
                {
                    TaskHistoryList.Clear();
                    TaskHistoryList = null;
                }
                if (FieldArray != null)
                {
                    FieldArray.Clear();
                    FieldArray = null;
                }
                if (FieldSeq != null)
                {
                    FieldSeq.Clear();
                    FieldSeq = null;
                }
                if (logErr)
                {
                    int Event_id = 9173;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
                else
                {

                }
            }
        }

        public bool MonitorLoanStages()
        {
            string err = "";
            bool logErr = false;
            List<Table.LoanStatus> activeLoans = null;
            List<Table.LoanStages> stageList = null;
            List<StageInfo> si = null;
            UpdateStageRequest req = new UpdateStageRequest();
            req.hdr = new ReqHdr();
            try
            {
                activeLoans = da.GetActiveLoans(ref err);
                if (activeLoans == null)
                {
                    if (err != String.Empty)
                    {
                        int Event_id = 9174;
                        err = "da.GetActiveLoans equal to null, error: " + err;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                    return true;
                }
                si = new List<StageInfo>();
                string filePath = string.Empty;
                foreach (Table.LoanStatus loan in activeLoans)
                {
                    stageList = da.GetLoanStagesByFileId(loan.FileId, ref err);
                    if ((stageList == null) || (stageList.Count <= 0))
                        continue;
                    bool hasTask = false;
                    foreach (Table.LoanStages stage in stageList)
                    {
                        if (stage == null || stage.TaskCount < 1)
                            continue;
                        hasTask = true;
                        break;
                    }
                    if (!hasTask)
                        continue;
                    Table.PointFileInfo pf = da.GetPointFileInfo(loan.FileId, ref err);
                    if (pf == null || string.IsNullOrEmpty(pf.Path))
                    {
                        err = string.Format("Got Null from da.GetPointFileInfo({0}) or path is null, err: {1}", loan.FileId, err);
                        int Event_id = 9175;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        continue;
                    }
                    filePath = pf.Path;
                    if (File.Exists(filePath))
                    {
                        DateTime accessTime = File.GetLastAccessTime(filePath);
                        if (accessTime.Date == DateTime.Now.Date)
                        {
                            UpdatePointStatusDate(loan.FileId, filePath, stageList);
                            if (stageList != null)
                            {
                                stageList.Clear();
                                stageList = null;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "PointManager::MonitorLoanStages, Exception:" + ex.Message;
                int Event_id = 9176;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (activeLoans != null)
                {
                    activeLoans.Clear();
                    activeLoans = null;
                }
                if (stageList != null)
                {
                    stageList.Clear();
                    stageList = null;
                }
                if (si != null)
                {
                    si.Clear();
                    si = null;
                }
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9177;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        private bool ZipAllFiles(string oldFilePath, string fileName, string newFilepath, ref string err)
        {
            List<string> AllFileList = null;
            int index = -1;
            string ZipFileName = "";
            err = string.Empty;
            string file_path = Path.GetDirectoryName(oldFilePath);

            // LCW, Per focusIT, we'll create a zip file in the folder C:\PulseBackup\<ClientID>
            string backup_path = PointZipFolder;
            try
            {
                if (!Directory.Exists(backup_path))
                    Directory.CreateDirectory(backup_path);

                string DT = DateTime.Now.ToString();
                DT = DT.Replace(@"/", "_");
                DT = DT.Replace(":", "-");

                if (fileName.Length > 6)
                    ZipFileName = backup_path + "\\" + fileName.Substring(0, fileName.Length - 4) + "_" + DT + ".zip";

                index = fileName.ToUpper().LastIndexOf(".BRW");

                if (index < 0)
                    index = fileName.ToUpper().LastIndexOf(".PRS");

                if (index > 0)
                    fileName = fileName.Remove(index);

                string filter_string = fileName + ".*";

                string[] filters = { filter_string };

                AllFileList = new List<string>();

                FileHelper.GetFileList(file_path, filters, ref AllFileList, false);

                if ((AllFileList == null) || (AllFileList.Count <= 0))
                    return true;

                string newFileName = string.Empty;
                string new_file_path = Path.GetDirectoryName(newFilepath);

                using (ZipFile zip = new ZipFile())
                {
                    foreach (string AllFile in AllFileList)
                    {
                        if (AllFile.Contains(fileName))
                        {
                            try
                            {
                                zip.AddFile(AllFile, "");
                            }
                            catch
                            {
                            }
                        }
                    }

                    zip.Save(ZipFileName);
                }
            }
            catch (Exception ex)
            {
                err = string.Format("ZipAllFiles, failed to zip up the files for {0} \r\n Exception: {1}", oldFilePath + @"\" + fileName, ex.ToString());
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, 9002);
            }
            return true;
        }

        private bool MoveAllFiles(string oldFilePath, string fileName, string newFilepath, ref string err)
        {
            List<string> AllFileList = null;
            int index = -1;
            string ZipFileName = "";

            if (oldFilePath.Length > 10)
                ZipFileName = oldFilePath.Substring(0, oldFilePath.Length - 4) + ".zip";

            index = fileName.ToUpper().LastIndexOf(".BRW");
            if (index < 0)
                index = fileName.ToUpper().LastIndexOf(".PRS");

            if (index > 0)
                fileName = fileName.Remove(index);

            string filter_string = fileName + ".*";

            string[] filters = { filter_string };

            AllFileList = new List<string>();
            string file_path = Path.GetDirectoryName(oldFilePath);
            FileHelper.GetFileList(file_path, filters, ref AllFileList, false);
            if ((AllFileList == null) || (AllFileList.Count <= 0))
                return true;
            string newFileName = string.Empty;
            string new_file_path = Path.GetDirectoryName(newFilepath);
            string tempFileName = string.Empty;
            fileName = fileName.ToLower();
            foreach (string AllFile in AllFileList)
            {
                if (AllFile == ZipFileName)
                {
                    continue;
                }
                tempFileName = AllFile.ToLower();

                //if (AllFile.Contains(fileName))
                if (tempFileName.Contains(fileName))
                {
                    try
                    {
                        newFileName = new_file_path + "\\" + Path.GetFileName(AllFile);
                        File.Move(AllFile, newFileName);
                    }
                    catch
                    {
                    }
                }
            }
            return true;
        }

        public bool MoveFile(MoveFileRequest req, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if ((req == null) || (req.hdr == null))
                {
                    err = "Invalid MoveFileRequest, request is null. ";
                    int Event_id = 9178;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((req.FileId <= 0) || (req.hdr.UserId <= 0) || (req.NewFolderId <= 0))
                {
                    err = string.Format("Invalid FileId {0}, UserId {1}, or NewFolderId {2} in the MoveFileRequest.", req.FileId, req.hdr.UserId, req.NewFolderId);
                    int Event_id = 9179;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                return MoveFile(req.FileId, req.NewFolderId, req.hdr.UserId, ref err);
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to move Point file, FileId={0}, err:{1}", req.FileId, err);
                int Event_id = 9180;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9181;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        private bool MoveFile(int FileId, int newFolderId, int UserId, ref string err)
        {
            err = "";
            bool logErr = false;
            bool fatal = false;
            bool status = true;
            string newPath = "";
            string filePath = string.Empty;
            try
            {
                if (Get_CheckPointFile(FileId, ref filePath, false, FileMode.Open, ref err) == false)
                {
                    err = "MoveFile " + err;
                    int Event_id = 9182;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if (filePath == string.Empty)
                {
                    err = string.Format("Failed to move the Point file, reason: cannot get the filepath for FileId={0}.", FileId);
                    int Event_id = 9183;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                PointFolderInfo pf = da.GetPointFolderInfo(newFolderId, ref err);
                if (pf == null)
                {
                    err = " MoveFile" + err;
                    int Event_id = 9184;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if (pf.LoanStatus <= 0)
                {
                    err = string.Format("Cannot Move the Point File {0}, invalid Point Folder Status={1}, FolderId={2}, FileId={3}.", filePath, pf.LoanStatus, newFolderId, FileId);
                    int Event_id = 9185;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                LoanStatusEnum loanStatus = (LoanStatusEnum)pf.LoanStatus;

                newPath = pf.Path;
                string tempFilename = Path.GetFileName(filePath);
                if (loanStatus == LoanStatusEnum.Processing)
                {
                    tempFilename = tempFilename.Replace(".PRS", ".BRW");
                    tempFilename = tempFilename.Replace(".prs", ".BRW");
                }
                if (tempFilename.ToUpper().EndsWith(".BRW"))
                    newPath = newPath + @"\BORROWER\";
                else
                    if (tempFilename.ToUpper().EndsWith(".PRS"))
                        newPath = newPath + @"\PROSPECT\";

                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);
                string newFileName = newPath + tempFilename;

                if (!File.Exists(filePath))
                {
                    err = string.Format("The Point File {0} does not exist.", filePath);
                    int Event_id = 9186;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                if (filePath == newFileName)
                {
                    //err = string.Format("Cannot Move the Point File {0}, the target and the source file cannot be the same file.", filePath);
                    //logErr = true;
                    return true;
                }

                status = ZipAllFiles(filePath, tempFilename, newFileName, ref err);

                File.Move(filePath, newFileName);

                if (MoveAllFiles(filePath, tempFilename, newFileName, ref err) == false)
                {
                    err = string.Format("Unable to move the Point file, FileId={0}, Point File, {1}, Error:{2}", FileId, filePath, err);
                    int Event_id = 9187;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

                if (da.MovePointFile(FileId, newFolderId, newFileName, UserId, ref err) == false)
                {
                    int Event_id = 9188;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("Unable to move the Point file, FileId={0}, Point File, {1}, Exception:{2}", FileId, filePath, ex.Message);
                int Event_id = 9189;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9190;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
                else
                {
                    string msg = "Point file  " + filePath + " has been moved to " + newPath;
                    err = msg;
                    int Event_id = 9003;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id, Category);
                    Trace.TraceInformation(err);
                }
            }
        }

        private void UpdatePointStatus(LoanStatusEnum newFolderStatus, string newStatus, string StatusDate,
                                        List<FieldMap> UpdatedFieldds, List<string> fieldArray, ArrayList fieldSeq)
        {
            short FldId = 0;

            if (newFolderStatus == LoanStatusEnum.Prospect)
            {
                StatusDate = "";
                AddUpdatedFields(ref UpdatedFieldds, (short)PointStageDateField.Canceled, StatusDate);         // cancelled date
                AddUpdatedFields(ref UpdatedFieldds, (short)PointStageDateField.Denied, StatusDate);         // denied date
                AddUpdatedFields(ref UpdatedFieldds, (short)PointStageDateField.Suspended, StatusDate);         // suspended date
                AddUpdatedFields(ref UpdatedFieldds, (short)PointStageDateField.Closed, StatusDate);         // closed date
                AddUpdatedFields(ref UpdatedFieldds, (short)PointStageDateField.Prospect, DateTime.Now.Date.ToString());   // prospect status date
                return;
            }

            switch (newStatus.ToUpper())
            {
                case "CLOSED":
                    FldId = (short)PointStageDateField.Closed;
                    break;
                case "CANCELED":
                    FldId = (short)PointStageDateField.Canceled;
                    break;
                case "DENIED":
                    FldId = (short)PointStageDateField.Denied;
                    break;
                case "PROCESSING":
                    StatusDate = "";
                    AddUpdatedFields(ref UpdatedFieldds, (short)PointStageDateField.Canceled, StatusDate);         // cancelled date
                    AddUpdatedFields(ref UpdatedFieldds, (short)PointStageDateField.Denied, StatusDate);         // denied date
                    AddUpdatedFields(ref UpdatedFieldds, (short)PointStageDateField.Suspended, StatusDate);         // suspended date
                    AddUpdatedFields(ref UpdatedFieldds, (short)PointStageDateField.Closed, StatusDate);         // closed date
                    break;
                case "SUSPENDED":
                    FldId = (short)PointStageDateField.Suspended;
                    break;
            }

            if (FldId > 0)
                AddUpdatedFields(ref UpdatedFieldds, FldId, StatusDate);

        }

        private bool UpdateLoanStatus(int FileId, int UserId, string newStatus, DateTime newStatusDate,
                               string filePath, LoanStatusEnum newFolderStatus, ref string err)
        {
            err = "";
            bool status = true;
            List<FieldMap> UpdatedFieldds = new List<FieldMap>();
            List<string> fieldArray = null;
            ArrayList fieldSeq = null;

            string dateNewStatus = string.Empty;
            try
            {
                if (!File.Exists(filePath))
                {
                    return true;
                }
                UpdatedFieldds.Clear();
                dateNewStatus = newStatusDate == DateTime.MinValue ? DateTime.Today.Date.ToString() : newStatusDate.Date.ToString();
                UpdatePointStatus(newFolderStatus, newStatus, dateNewStatus, UpdatedFieldds, fieldArray, fieldSeq);
                if (UpdatedFieldds != null && UpdatedFieldds.Count > 0)
                {
                    status = pntLib.WritePointData(UpdatedFieldds, filePath, ref  err);
                    if (status == false)
                    {
                        err = string.Format("UpdateLoanStatus, Error: {0} for FileId {1}, Status {2}", err, FileId, newStatus);
                        int Event_id = 9191;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return status;
                    }
                }
                status = da.UpdateLoanStatus(FileId, newStatus, UserId, newFolderStatus, ref err);
                if (status == false)
                {
                    err = string.Format("UpdateLoanStatus, Error: {0} for FileId {1}, Status {2}", err, FileId, newStatus);
                    int Event_id = 9192;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
            catch (Exception ex)
            {
                err = string.Format("UpdateLoanStatus, Exception: {0} for FileId {1}, Status {2}", ex.Message, FileId, newStatus);
                int Event_id = 9193;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                status = false;
            }

            return status;
        }
        private string RandomString(int size)
        {
            //StringBuilder builder = new StringBuilder(); 
            //char ch; 
            //for (int i = 0; i < size; i++)
            //{ 
            //ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))); 
            //    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble())));
            //    builder.Append(ch); 
            //} 

            //return builder.ToString(); 
            //string temp = random.NextDouble().ToString();

            string temp = random.Next().ToString();
            temp += (3664637658 * random.NextDouble()).ToString();
            temp = temp.Replace(".", "");
            return temp.Substring(0, size);
        }
        private string Get_AutoPointFilename(int fileid, string folderPath, DataAccess.PointFolderInfo pf)
        {
            string filename = string.Empty;
            string name = string.Empty;
            string err = string.Empty;

            if (string.IsNullOrEmpty(folderPath) && (pf == null || string.IsNullOrEmpty(pf.Path)))
                return filename;

            if (string.IsNullOrEmpty(folderPath))
                folderPath = pf.Path;

            filename = string.Empty;

            string tempName = string.Empty;
            if (pf.FilenameLength <= 0)
                pf.FilenameLength = 14;
            if (string.IsNullOrEmpty(pf.FilenamePrefix))
                pf.FilenamePrefix = string.Empty;

            if (pf.RandomNumbering)
            {
                tempName = RandomString(pf.FilenameLength);
            }
            else
                tempName = DateTime.Now.ToString("o");
            //tempName = DateTime.Now.Date.Year.ToString() + DateTime.Now.Date.Month.ToString("MM") +
            //              DateTime.Now.Date.Day.ToString("dd") + DateTime.Now.Hour.ToString("HH") + DateTime.Now.Minute.ToString("mm") + DateTime.Now.Second.ToString("ss");
            //tempName = tempName.Replace("-", "");
            //tempName = tempName.Replace(".", "");
            //tempName = tempName.Replace(":", "");

            Regex r = new Regex("(?:[^0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            tempName = r.Replace(tempName, String.Empty);
            tempName = pf.FilenamePrefix + tempName;
            int length = pf.FilenameLength + pf.FilenamePrefix.Trim().Length;
            filename = tempName.Substring(0, length) + ".BRW";
            name = @"\BORROWER\" + filename;
            folderPath = folderPath + @"\BORROWER\";
            filename = folderPath + filename;
            return filename;
        }

        private bool CreateLoanRecord_PointFile(int FileId, string newPath, int folderId, ref string err)
        {
            string fileName = Path.GetFileName(newPath);
            if (string.IsNullOrEmpty(fileName))
            {
                err = string.Format("CreateLoanRecord_PointFile, Cannot get the file path from the path {0}, fileId {1}", newPath, FileId);
                int Event_id = 9194;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            try
            {
                Table.Contacts borrower = null;
                Table.Contacts cBorrower = null;
                GetBorrowerCoBorrowerInfo(FileId, ref borrower, ref cBorrower, ref err);
                Record.Loans loan = da.GetLoanInfo(FileId, ref err);
                if (loan == null)
                {
                    err = string.Format("CreateLoanRecord_PointFile, No loan record available to create the Point file with.  FileId {1}, Path {0}", newPath, FileId);
                    int Event_id = 9195;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                loan.PointFilePath = newPath;
                loan.LoanNumber = Path.GetFileNameWithoutExtension(newPath);
                Table.LoanTeam team = da.GetLoanTeamInfo(FileId, ref err);

                return (CreatePointFile(FileId, newPath, folderId, loan, borrower, cBorrower, team, ref err));
            }
            catch (Exception ex)
            {
                err = "CreateLoanRecord_PointFile, Exception: " + ex.Message;
                int Event_id = 9196;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
        }

        public bool ConvertToLead(ConvertToLeadRequest req, ref string err)
        {
            err = "";
            bool status = false;
            bool logErr = false;
            bool fatal = false;

            #region check Convert to Lead parameters

            if ((req == null) || (req.hdr == null))
            {
                err = "Invalid ConvertToLeadRequest, request is null. ";
                int Event_id = 9197;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            if ((req.FileId <= 0) || (req.hdr.UserId < 0) || (req.NewFolderId <= 0))
            {
                err = string.Format("Invalid FileId {0}, UserId {1}, or NewFolderId {2} in the ConvertToLeadRequest.", req.FileId, req.hdr.UserId, req.NewFolderId);
                int Event_id = 9198;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            #endregion

            string currentPath = string.Empty;
            string newPath = string.Empty;

            try
            {
                if (Get_CheckPointFile(req.FileId, ref currentPath, true, FileMode.OpenOrCreate, ref err) == false)
                {
                    err = "ConvertToLead " + err;
                    int Event_id = 9210;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return status;
                }
                string folderPath = Path.GetDirectoryName(currentPath);
                if (string.IsNullOrEmpty(folderPath))
                {
                    err = string.Format("ConvertToLead, Cannot get the Directory path from the path {0}, fileId {1}", folderPath, req.FileId);
                    int Event_id = 9211;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return status;
                }
                if (!File.Exists(currentPath))
                {
                    err = string.Format("ConvertToLead, cannot move the file {0}  which doesn't exist, fileId {1}.", currentPath, req.FileId);
                    int Event_id = 9212;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return status;
                }

                status = da.ConvertToLead(req.FileId, req.hdr.UserId, ref err);
                if (status == false)
                {
                    err = string.Format("ConvertToLead,  error {0}, FolderId {1}, FileId {2}", err, req.NewFolderId, req.FileId);
                    int Event_id = 9214;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return status;
                }
                status = MoveFile(req.FileId, req.NewFolderId, req.hdr.UserId, ref err);
                if (status == false)
                {
                    err = string.Format("ConvertToLead,  error {0}, FolderId {1}, FileId {2}", err, req.NewFolderId, req.FileId);
                    int Event_id = 9215;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return status;
                }


                return status;
            }
            catch (Exception ex)
            {
                err = string.Format("ConvertToLead failed, FileId={0}, Point File, {1}, Exception:{2}", req.FileId, currentPath, ex.Message);
                int Event_id = 9216;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return status;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9217;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
                if (status)
                    AutoApplyWorkflow(req.FileId, "Prospect", ref err);
            }
        }

        private bool DisposeLoan(DisposeLoanRequest req, ref string err)
        {
            err = "";
            bool logErr = false;
            bool fatal = false;

            #region check Dispose Loan parameters

            if ((req == null) || (req.hdr == null))
            {
                err = "Invalid DisposeLoanRequest, request is null. ";
                int Event_id = 9218;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            if ((req.FileId <= 0) || (req.hdr.UserId < 0) || (req.NewFolderId <= 0))
            {
                err = string.Format("Invalid FileId {0}, UserId {1}, or NewFolderId {2} in the DisposeLoanRequest.", req.FileId, req.hdr.UserId, req.NewFolderId);
                int Event_id = 9219;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            if (req.LoanStatus == null || string.IsNullOrEmpty(req.LoanStatus))
            {
                err = "No LoanStatus specified in the DisposeLoanRequest.";
                int Event_id = 9220;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            if ((req.StatusDate == null) || (req.StatusDate == DateTime.MinValue))
            {
                err = "No StatusDate specified in the DisposeLoanRequest.";
                int Event_id = 9221;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            #endregion

            string filePath = "";
            string newPath = "";

            try
            {
                DataAccess.PointFolderInfo pf = da.GetPointFolderInfo(req.NewFolderId, ref err);
                if (pf == null)
                {
                    err = string.Format("DisposeLoan, Error: {0}, FileId {1}, LoanStatus {2}, FolderId {3}", err, req.FileId, req.LoanStatus, req.NewFolderId);
                    int Event_id = 9222;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                LoanStatusEnum folderLoanStatus = (LoanStatusEnum)pf.LoanStatus;
                Table.PointFileInfo fileInfo = da.GetPointFileInfo(req.FileId, ref err);
                if (fileInfo == null)
                {
                    err = string.Format("DisposeLoan, Error: {0}, FileId {1}, LoanStatus {2}, FolderId {3}", err, req.FileId, req.LoanStatus, req.NewFolderId);
                    int Event_id = 9223;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                filePath = fileInfo.Path;
                if (folderLoanStatus == LoanStatusEnum.Prospect || folderLoanStatus == LoanStatusEnum.ProspectArchive)
                {
                    string fileName = Path.GetFileName(fileInfo.Path);
                    if (string.IsNullOrEmpty(fileName) || !File.Exists(fileInfo.Path))
                    {
                        bool status = da.UpdateLoanStatus(req.FileId, req.LoanStatus, req.hdr.UserId, folderLoanStatus, ref err);
                        if (status == false)
                        {
                            err = string.Format("DisposeLoan, Error: {0}, FileId {1}, LoanStatus {2}, FolderId {3}", err, req.FileId, req.LoanStatus, req.NewFolderId);
                            int Event_id = 9224;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                            return status;
                        }
                        status = da.MovePointFile(req.FileId, req.NewFolderId, fileName, req.hdr.UserId, ref err);
                        if (status == false)
                        {
                            err = string.Format("DisposeLoan, Error: {0}, FileId {1}, LoanStatus {2}, FolderId {3}", err, req.FileId, req.LoanStatus, req.NewFolderId);
                            int Event_id = 9225;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        }
                        return status;
                    }
                }

                string pointfiles_Name = "";

                if (folderLoanStatus != LoanStatusEnum.Prospect && folderLoanStatus != LoanStatusEnum.ProspectArchive)
                {
                    pointfiles_Name = da.Get_PointFiles_Name(req.FileId, ref err);

                    if (string.IsNullOrEmpty(pointfiles_Name))
                        filePath = Get_AutoPointFilename(req.FileId, filePath, pf);

                    if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(Path.GetFileName(filePath)))
                    {
                        err = string.Format("DisposeLoan, Get_AutoPointFilename doesn't generate the filename , FileId {1}, LoanStatus {2}, FolderId {3}", err, req.FileId, req.LoanStatus, req.NewFolderId);
                        int Event_id = 9226;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                }

                if (UpdateLoanStatus(req.FileId, req.hdr.UserId, req.LoanStatus, req.StatusDate, filePath, (LoanStatusEnum)pf.LoanStatus, ref err) == false)
                {
                    err = string.Format("DisposeLoan, Error: {0}, FileId {1}, LoanStatus {2}, FolderId {3}", err, req.FileId, req.LoanStatus, req.NewFolderId);
                    int Event_id = 9227;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                if (MoveFile(req.FileId, req.NewFolderId, req.hdr.UserId, ref err) == false)
                {
                    int Event_id = 9228;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("Unable to dispose the loan, FileId={0}, Point File, {1}, Exception:{2}", req.FileId, filePath, ex.Message);
                int Event_id = 9229;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9230;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
                else
                {
                    string msg = "Point file  " + filePath + " has been moved to " + newPath;
                    err = msg;
                    int Event_id = 9003;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id, Category);
                    Trace.TraceInformation(err);
                }
            }

        }
        private bool ConvertToLoan(int FileId, int newFolderId, ref string filePath, DataAccess.PointFolderInfo pf, int UserId, ref string err)
        {
            bool status = false;
            string pointfiles_Name = "";
            string newFilePath = "";
            try
            {
                pointfiles_Name = da.Get_PointFiles_Name(FileId, ref err);
                // if the Point filename is NULL, generate one
                if (string.IsNullOrEmpty(pointfiles_Name))
                {
                    newFilePath = Get_AutoPointFilename(FileId, pf.Path, pf);
                    filePath = newFilePath;
                }
                else
                {
                    // if the file already exists, don't do anything
                    if (File.Exists(filePath))
                    {
                        return true;
                    }
                    newFilePath = pf.Path + pointfiles_Name;
                    if (newFilePath.Contains(@"PROSPECT\"))
                        newFilePath = newFilePath.Replace(@"PROSPECT\", @"BORROWER\");
                }
                // Unable to generate a filename
                if (string.IsNullOrEmpty(newFilePath))
                {
                    err = string.Format("ConvertToLoan: unable to generate a filename, FileId {1}, LoanStatus {2}, FolderId {3}", err, FileId, "Processing", newFolderId);
                    int Event_id = 9231;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return status;
                }
                // Create the Point file
                status = CreateLoanRecord_PointFile(FileId, newFilePath, newFolderId, ref err);
                if (status == false)
                {
                    err = string.Format("ConvertToLoan:  FileId {1}, LoanStatus {2}, FolderId {3}, Err {0}", err, FileId, "Processing", newFolderId);
                    int Event_id = 9232;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return status;
                }
                filePath = newFilePath;
                return status;
            }
            catch (Exception ex)
            {
                err = string.Format("ConvertToLoan:  FileId {1}, LoanStatus {2}, FolderId {3}, Err {0}", ex.Message, FileId, "Processing", newFolderId);
                int Event_id = 9233;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                status = false;
            }
            return status;
        }
        private bool DisposeLead(DisposeLeadRequest req, ref string err)
        {
            err = "";
            bool logErr = false;
            bool fatal = false;
            bool status = false;
            #region check Dispose Lead parameters

            if ((req == null) || (req.hdr == null))
            {
                err = "Invalid DisposeLeadRequest, request is null. ";
                int Event_id = 9234;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            if ((req.FileId <= 0) || (req.hdr.UserId < 0) || (req.NewFolderId <= 0))
            {
                err = string.Format("Invalid FileId {0}, UserId {1}, or NewFolderId {2} in the DisposeLeadRequest.", req.FileId, req.hdr.UserId, req.NewFolderId);
                int Event_id = 9235;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            if (req.LoanStatus == null || string.IsNullOrEmpty(req.LoanStatus))
            {
                err = "No LoanStatus specified in the DisposeLeadRequest.";
                int Event_id = 9236;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            if ((req.StatusDate == null) || (req.StatusDate == DateTime.MinValue))
            {
                err = "No StatusDate specified in the DisposeLeadRequest.";
                int Event_id = 9237;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            #endregion

            string filePath = "";
            //string fileName = "";
            DataAccess.PointFolderInfo new_pf = null;
            DataAccess.PointFolderInfo oldpf = null;
            Table.PointFileInfo fileInfo = null;
            LoanStatusEnum folderLoanStatus = LoanStatusEnum.Prospect;
            try
            {
                new_pf = da.GetPointFolderInfo(req.NewFolderId, ref err);
                if (new_pf == null)
                {
                    err = string.Format("DisposeLead, Error: {0}, FileId {1}, LoanStatus {2}, FolderId {3}", err, req.FileId, req.LoanStatus, req.NewFolderId);
                    int Event_id = 9238;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                folderLoanStatus = (LoanStatusEnum)new_pf.LoanStatus;
                fileInfo = da.GetPointFileInfo(req.FileId, ref err);
                if (fileInfo == null)
                {
                    err = string.Format("DisposeLead, Error: {0}, FileId {1}, LoanStatus {2}, FolderId {3}", err, req.FileId, req.LoanStatus, req.NewFolderId);
                    int Event_id = 9239;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    //return false;
                }
                else
                {
                    if (string.IsNullOrEmpty(fileInfo.Path))
                    {
                        oldpf = da.GetPointFolderInfo(fileInfo.FolderId, ref err);
                        if (oldpf != null)
                        {
                            filePath = oldpf.Path;
                            //fileName = string.Empty;
                        }
                    }
                    else
                    {
                        filePath = fileInfo.Path;
                        //fileName = Path.GetFileName(filePath);
                    }
                }

                if (req.LoanStatus.ToUpper() == "PROCESSING")
                {
                    status = ConvertToLoan(req.FileId, req.NewFolderId, ref filePath, new_pf, req.hdr.UserId, ref err);
                    if (status == false)
                    {
                        err = string.Format("DisposeLead, Error: {0}, FileId {1}, LoanStatus {2}, FolderId {3}", err, req.FileId, req.LoanStatus, req.NewFolderId);
                        int Event_id = 9240;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                    if (!File.Exists(filePath))
                    {
                        err = string.Format("DisposeLead, Error: Cannot convert to loan, Point file does not exist for Point file Id={0}.", req.FileId);
                        int Event_id = 9241;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        status = false;
                        return status;
                    }
                    if (fileInfo != null && string.IsNullOrEmpty(fileInfo.Path))
                        return status;
                    // file exists and the path is the new path, don't move it.
                    if (File.Exists(filePath) && filePath.StartsWith(new_pf.Path))
                        return status;
                }

                // if the file exists, move it and return
                if (File.Exists(filePath))
                {
                    status = MoveFile(req.FileId, req.NewFolderId, req.hdr.UserId, ref err);
                    if (status == false)
                    {
                        err = string.Format("DisposeLead, Error: {0}, FileId {1}, LoanStatus {2}, FolderId {3}", err, req.FileId, req.LoanStatus, req.NewFolderId);
                        int Event_id = 9242;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                err = string.Format("Unable to dispose of the lead, FileId={0}, Point File, {1}, Exception:{2}", req.FileId, filePath, ex.Message);
                int Event_id = 9243;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (status)
                {
                    status = da.DisposeLead(req.FileId, req.LoanStatus, req.hdr.UserId, filePath, new_pf, ref err);
                    if (status == false)
                    {
                        err = string.Format("DisposeLead, Error: {0}, FileId {1}, LoanStatus {2}, FolderId {3}", err, req.FileId, req.LoanStatus, req.NewFolderId);
                        int Event_id = 9244;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    filePath = da.GetPointFileInfo(req.FileId, ref err).Path;
                    if (File.Exists(filePath))
                    {
                        PointData pd = new PointData(pntLib, da);
                        ProspectFlagEnum prospectFlag = GetProspectFlag(folderLoanStatus);
                        UpdateLoanStatus(req.FileId, req.hdr.UserId, req.LoanStatus, DateTime.MinValue, filePath, folderLoanStatus, ref err);
                        if (ImportPointData(pd, filePath, prospectFlag, ref err) == false)
                        {
                            int Event_id = 9245;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        }
                    }
                }
                if (logErr)
                {
                    int Event_id = 9246;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
                else
                {
                    string msg = string.Format("The lead FileId={0} has been disposed to Status={1}, OldFolderId={2}, NewFolderId={3}.", req.FileId, req.LoanStatus, fileInfo.FolderId, req.NewFolderId);
                    int Event_id = 9003;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, msg, EventLogEntryType.Information, Event_id, Category);
                    Trace.TraceInformation(err);
                }
            }

        }
        private void QueueRequest(object o)
        {
            string err = "";
            if (o == null)
            {
                err = "QueueRequest, Request is empty.";
                Trace.TraceError(err);
                int Event_id = 9247;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            PointMgrEvent e = o as PointMgrEvent;
            if (e == null)
            {
                err = "QueueRequest, Request is empty.";
                Trace.TraceError(err);
                int Event_id = 9248;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            bool status = false;
            switch (e.RequestType)
            {
                case PointMgrCommandType.ImportLoans:
                    ImportLoansRequest il_req = e.Request as ImportLoansRequest;
                    status = ImportLoans(il_req, ref err);
                    break;                
                case PointMgrCommandType.ImportAllLoans:
                    status = ImportAllLoans(e, true);
                    break;
                case PointMgrCommandType.ImportLONames:
                    status = ImportLoanReps(e, ref err);
                    break;
                case PointMgrCommandType.ImportCardex:
                    status = ImportCardex(e, ref err);
                    break;
                case PointMgrCommandType.AddNote:
                    AddNoteRequest an_req = e.Request as AddNoteRequest;
                    status = AddNote(an_req, ref err);
                    break;
                case PointMgrCommandType.ExtendRateLock:
                    ExtendRateLockRequest el_req = e.Request as ExtendRateLockRequest;
                    status = ExtendRateLock(el_req, ref err);
                    break;
                case PointMgrCommandType.GetPointFile:
                    break;
                case PointMgrCommandType.MovePointFile:
                    MoveFileRequest mf_req = e.Request as MoveFileRequest;
                    status = MoveFile(mf_req, ref err);
                    break;
                case PointMgrCommandType.DisposeLoan:
                    DisposeLoanRequest dl_req = e.Request as DisposeLoanRequest;
                    status = DisposeLoan(dl_req, ref err);
                    break;
                case PointMgrCommandType.UpdateBorrower:
                    UpdateBorrowerRequest ub_req = e.Request as UpdateBorrowerRequest;
                    status = UpdateBorrower(ub_req, ref err);
                    break;
                case PointMgrCommandType.UpdateLoanInfo:
                    UpdateLoanInfoRequest ul_req = e.Request as UpdateLoanInfoRequest;
                    status = UpdateLoanInfo(ul_req, ref err);
                    break;
                case PointMgrCommandType.ReassignContact:
                    ReassignContactRequest rc_req = e.Request as ReassignContactRequest;
                    status = ReassignContact(rc_req, ref err);
                    break;
                case PointMgrCommandType.ReassignLoan:
                    ReassignLoanRequest rl_req = e.Request as ReassignLoanRequest;
                    status = ReassignLoan(rl_req, ref err);
                    break;
                case PointMgrCommandType.UpdateEstCloseDate:
                    UpdateEstCloseDateRequest ue_req = e.Request as UpdateEstCloseDateRequest;
                    status = UpdateEstCloseDate(ue_req, ref err);
                    break;
                case PointMgrCommandType.UpdateStage:
                    UpdateStageRequest us_req = e.Request as UpdateStageRequest;
                    status = UpdateStage(us_req, ref err);
                    break;
                case PointMgrCommandType.ImportLockInfo:
                    var lockInfoRequest = e.Request as ImportLockInfoRequest;
                    ImportLoansRequest ili_req = new ImportLoansRequest();
                    ili_req.hdr = lockInfoRequest.hdr;
                    ili_req.FileIds = new int[] { lockInfoRequest.FileId };
                    status = ImportLoans(ili_req, ref err);
                    break;
                case PointMgrCommandType.UpdateLockInfo:
                    var updateLockInfoRequest = e.Request as UpdateLockInfoRequest;
                    status = UpdateLockInfo(updateLockInfoRequest, ref err);
                    break;
                case PointMgrCommandType.UpdateLoanProfitability:
                    updateLockInfoRequest = e.Request as UpdateLockInfoRequest;
                    status = UpdateLoanProfitability(updateLockInfoRequest, ref err);
                    break;
                default:
                    err = "Point Manager, invalid Request Type=" + e.RequestType.ToString();
                    int Event_id = 9249;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    break;
            }
            if (status == false)
            {
                Trace.TraceError(err);
                int Event_id = 9250;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            o = null;
        }

        private bool ProcessRealTimeRequest(PointMgrEvent e, ref string err)
        {
            err = "";
            if (e == null)
            {
                err = "Point Manager, empty request.";
                int Event_id = 9251;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            string filePath = "";

            if (e.RequestType == PointMgrCommandType.ImportLoans)
            {
                ImportLoansRequest req = e.Request as ImportLoansRequest;
                if (req == null)
                {
                    err = PointMgrCommandType.ImportLoans.ToString() + "received an empty request.";
                    return false;
                }

                if ((req.FileIds == null) || (req.FileIds.Length <= 0))
                {
                    err = "Point Manager, miss FileIds, requestType=" + PointMgrCommandType.ImportLoans.ToString();
                    int Event_id = 9252;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((req.FileIds.Length == 1) &&
                    (Get_CheckPointFile(req.FileIds[0], ref filePath, false, FileMode.Open, ref err) == false))
                {
                    err = "Unable to " + e.RequestType.ToString() + ", " + err;
                    int Event_id = 9253;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                ThreadPool.QueueUserWorkItem(new WaitCallback(QueueRequest), (object)e);
                return true;
            }          

            if ((e.RequestType == PointMgrCommandType.AddNote) ||
                (e.RequestType == PointMgrCommandType.ExtendRateLock) ||
                (e.RequestType == PointMgrCommandType.MovePointFile) ||
                (e.RequestType == PointMgrCommandType.DisposeLoan) ||
                (e.RequestType == PointMgrCommandType.DisposeLead) ||
                (e.RequestType == PointMgrCommandType.CreateFile) ||
                (e.RequestType == PointMgrCommandType.ConvertToLead) ||
                (e.RequestType == PointMgrCommandType.ReassignContact) ||
                (e.RequestType == PointMgrCommandType.ReassignLoan) ||
                (e.RequestType == PointMgrCommandType.UpdateEstCloseDate) ||
                (e.RequestType == PointMgrCommandType.UpdateStage) ||
                (e.RequestType == PointMgrCommandType.UpdateLoanInfo) ||
                (e.RequestType == PointMgrCommandType.UpdateBorrower) ||
                (e.RequestType == PointMgrCommandType.ImportLockInfo) ||
                (e.RequestType == PointMgrCommandType.UpdateLockInfo) ||
                (e.RequestType == PointMgrCommandType.UpdateLoanProfitability)
                )
            {
                if (e.RequestType != PointMgrCommandType.ReassignContact
                    && e.RequestType != PointMgrCommandType.ReassignLoan
                    && e.RequestType != PointMgrCommandType.UpdateBorrower
                    && e.FileId <= 0)
                {
                    err = string.Format("Failed to {0}, invalid FileId={1}.", e.RequestType.ToString(), e.FileId);
                    int Event_id = 9254;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                if (e.RequestType != PointMgrCommandType.ReassignContact
                    && e.RequestType != PointMgrCommandType.ReassignLoan
                    && e.RequestType != PointMgrCommandType.UpdateBorrower
                    && e.RequestType != PointMgrCommandType.UpdateLoanInfo
                    && e.RequestType != PointMgrCommandType.DisposeLead)
                {
                    if (Get_CheckPointFile(e.FileId, ref filePath, false, FileMode.Open, ref err) == false)
                    {
                        err = string.Format("Failed to {0}, FileId={1}, Error:{2}.", e.RequestType.ToString(), e.FileId, err);
                        int Event_id = 9255;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                    if ((filePath == null) || (filePath == string.Empty))
                    {
                        err = "Unable to get the filepath for Point file Id=" + e.FileId;
                        int Event_id = 9256;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                }

                if (e.RequestType == PointMgrCommandType.DisposeLoan)
                    return DisposeLoan((DisposeLoanRequest)e.Request, ref err);

                if (e.RequestType == PointMgrCommandType.DisposeLead)
                    return DisposeLead((DisposeLeadRequest)e.Request, ref err);

                if (e.RequestType == PointMgrCommandType.ConvertToLead)
                    return ConvertToLead((ConvertToLeadRequest)e.Request, ref err);

                ThreadPool.QueueUserWorkItem(new WaitCallback(QueueRequest), (object)e);
            }
            return true;
        }

        public bool ProcessRequest(PointMgrEvent e, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (e == null)
                {
                    err = "Point Manager, unable to process request, empty request.";
                    int Event_id = 9257;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                if ((e.RequestType == PointMgrCommandType.ImportAllLoans) || (e.RequestType == PointMgrCommandType.ImportCardex) ||
                    (e.RequestType == PointMgrCommandType.ImportLONames))
                {
                    m_ThreadContext.Post(new SendOrPostCallback(QueueRequest), e);
                    return true;
                }
                return (ProcessRealTimeRequest(e, ref err));
            }
            catch (Exception ex)
            {
                err = "Point Manager: Process Request, Exception: " + ex.Message;
                int Event_id = 9258;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9259;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        public GetPointFileInfoResp GetPointFileInfo(GetPointFileInfoReq req)
        {
            string err = "";
            bool logErr = false;
            GetPointFileInfoResp resp = new GetPointFileInfoResp();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = false;
            Table.PointFileInfo pf_Info = null;
            try
            {
                pf_Info = da.GetPointFileInfo(req.FileId, ref err);
                if (pf_Info == null)
                {
                    err = string.Format("unable to get PointFiles record for FileiD={0}. ", req.FileId);
                    resp.hdr.StatusInfo = err;
                    return resp;
                }
                if (string.IsNullOrEmpty(pf_Info.Path) || !File.Exists(pf_Info.Path))
                {
                    resp.FileExists = false;
                    resp.hdr.StatusInfo = string.Format("The file does not exist. ");
                    return resp;
                }

                resp.FileImage = File.ReadAllBytes(pf_Info.Path);
                resp.FilePath = pf_Info.Path;
                resp.FileExists = true;
                resp.hdr.Successful = true;

                return resp;
            }
            catch (Exception e)
            {
                err = string.Format("GetPointFileInfo, FileId {0},  Exception: {1}", req.FileId, e.Message);
                int Event_id = 9261;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                resp.FileImage = null;
                resp.FilePath = pf_Info == null ? string.Empty : pf_Info.Path;
                resp.FileExists = string.IsNullOrEmpty(resp.FilePath) ? false : true;
                resp.hdr.StatusInfo = e.Message;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9262;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
            return resp;
        }

        /// <summary>
        /// Determines whether [is file locked] [the specified file].
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        ///   <c>true</c> if [is file locked] [the specified file]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }

        public CheckPointFileStatusResp CheckPointFileStatus(CheckPointFileStatusReq req)
        {
            string err = string.Empty;
            const string info = "OK";
            bool logErr = false;
            var resp = new CheckPointFileStatusResp();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = false;

            try
            {
                var configData = da.GetPointConfigData(ref err);
                if (configData == null)
                {
                    err = "Failed to get Point Configuration data, err:" + err;
                    int Event_id = 9263;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);

                    resp.hdr.StatusInfo = err;
                    return resp;
                }
                var loanInfo = da.GetPointFileInfo(req.FileId, ref err);
                if (!string.IsNullOrEmpty(configData.WinPointIni))//Traditional Point Steps
                {

                    if (loanInfo == null || string.IsNullOrEmpty(loanInfo.Path) || !File.Exists(loanInfo.Path))
                    {
                        resp.hdr.Successful = true;
                        resp.hdr.StatusInfo = info;
                        return resp;
                    }

                    if (!IsFileLocked(new FileInfo(loanInfo.Path)))
                    {
                        resp.hdr.Successful = true;
                        resp.hdr.StatusInfo = info;
                    }
                    else
                    {
                        resp.hdr.Successful = true;
                        resp.FileLocked = true;
                        resp.hdr.StatusInfo = "The Point file is opened or locked.";
                    }
                    return resp;
                }
                else//PDS Steps
                {
                    var userName = string.Empty;
                    if (loanInfo == null)
                    {
                        resp.hdr.Successful = true;
                        return resp;
                    }

                    if ((loanInfo.PDSFileId == null) || (loanInfo.PDSFileId <= 0))
                    {
                        loanInfo.PDSFileId = da.GetPDSFileId(req.FileId, loanInfo.PDSFolderId, ref err);
                        if ((loanInfo.PDSFileId == null) || (loanInfo.PDSFileId <= 0))
                        {
                            resp.hdr.Successful = true;
                            return resp;
                        }
                    }

                    int pdsFileId = da.GetPDSPointFileUserNameById(loanInfo.PDSFileId, ref userName, ref err);

                    if (pdsFileId == 0 || string.IsNullOrEmpty(userName))
                    {
                        resp.hdr.Successful = true;
                        return resp;
                    }

                    if (pdsFileId != 0 && !string.IsNullOrEmpty(userName))
                    {
                        resp.hdr.Successful = true;
                        resp.FileLocked = true;
                        resp.hdr.StatusInfo = string.Format("The Point file is opened and locked by {0}.", userName);
                        return resp;
                    }
                }
            }
            catch (Exception exception)
            {
                err = "Point Manager Exception: " + exception.Message;
                int Event_id = 9264;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                resp.hdr.Successful = false;
                resp.hdr.StatusInfo = err;
                return resp;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9265;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }

            return resp;
        }

        private bool UpdateLockInfo(UpdateLockInfoRequest updateLockInfoRequest, ref string err)
        {
            err = "";
            bool logErr = false;
            Record.LoanLocksPage loanLocksPage = new Record.LoanLocksPage();

            #region check Rate Lock parameter
            if ((updateLockInfoRequest == null) || (updateLockInfoRequest.hdr == null))
            {
                err = "Invalid Update LockI nfo Request, request is null. ";
                int Event_id = 9266;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            if ((updateLockInfoRequest.FileId <= 0) || (updateLockInfoRequest.hdr.UserId < 0))
            {
                err = "Missing FileId or User Id in the Update Lock Info request ";
                int Event_id = 9267;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            string filePath = "";
            if (Get_CheckPointFile(updateLockInfoRequest.FileId, ref filePath, false, FileMode.Open, ref err) == false)
            {
                err = "Update Lock Info, " + err;
                int Event_id = 9268;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if (filePath == string.Empty)
            {
                err = "Update Lock Info, failed to get Point filepath.";
                int Event_id = 9269;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            #endregion

            List<Framework.FieldMap> UpdatedFieldds = new List<Framework.FieldMap>();
            try
            {
                string tempValue = string.Empty;
                UpdatedFieldds.Clear();
                #region Loan Fields
                Record.Loans loanInfo = da.GetLoanInfo(updateLockInfoRequest.FileId, ref err);
                if (loanInfo == null)
                {
                    err = "Update Lock Info, failed to get Loan Lock information from Pulse.";
                    int Event_id = 9270;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                //Loan Program
                AddUpdatedFields(ref UpdatedFieldds, 2000, loanInfo.Program);
                AddUpdatedFields(ref UpdatedFieldds, 7403, loanInfo.Program);

                if (!string.IsNullOrEmpty(loanInfo.Due))
                {
                    AddUpdatedFields(ref UpdatedFieldds, 3190, loanInfo.Due);
                }

                #endregion
                #region Loan Lock Section
                string sqlCmd5 = "Select CurrentValue FROM LoanPointFields Where FileId=" + updateLockInfoRequest.FileId + " and PointFieldId=2836";
                try
                {
                    object ds5 = DbHelperSQL.GetSingle(sqlCmd5);
                    if ((ds5 != null) && (ds5 != DBNull.Value))
                    {
                        loanLocksPage.FICO = (string)ds5;
                        AddUpdatedFields(ref UpdatedFieldds, 2836, loanLocksPage.FICO);
                    }
                }
                catch (Exception e)
                {
                }

                Record.LoanLocks loanLock = da.GetLoanLock(updateLockInfoRequest.FileId, ref err);
                if (loanLock == null)
                {
                    err = "Update Lock Info, failed to get Loan Lock information from Pulse.";
                    int Event_id = 9271;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                //Locked By
                AddUpdatedFields(ref UpdatedFieldds, 7338, loanLock.LockedBy);
                AddUpdatedFields(ref UpdatedFieldds, 6061, loanLock.LockTime);
                AddUpdatedFields(ref UpdatedFieldds, 6062, loanLock.LockTerm);
                AddUpdatedFields(ref UpdatedFieldds, 6063, loanLock.LockExpirationDate);
                // Extension 1
                //if (!string.IsNullOrEmpty(loanLock.Ext1LockExpDate) && !loanLock.Ext1LockExpDate.Contains("01/01/1900"))
                //    AddUpdatedFields(ref UpdatedFieldds, 6063, loanLock.Ext1LockExpDate);
                if (!string.IsNullOrEmpty(loanLock.Ext1LockTime) && !loanLock.Ext1LockTime.Contains("01/01/1900"))
                {
                    AddUpdatedFields(ref UpdatedFieldds, 7021, loanLock.Ext1LockTime);
                }
                else
                {
                    AddUpdatedFields(ref UpdatedFieldds, 7021, string.Empty);
                }

                if (!string.IsNullOrEmpty(loanLock.Ext1Term))
                    AddUpdatedFields(ref UpdatedFieldds, 1143, loanLock.Ext1Term);
                // Extension 2
                //if (!string.IsNullOrEmpty(loanLock.Ext2LockExpDate) && !loanLock.Ext2LockExpDate.Contains("01/01/1900"))
                //    AddUpdatedFields(ref UpdatedFieldds, 6063, loanLock.Ext2LockExpDate);
                if (!string.IsNullOrEmpty(loanLock.Ext2LockTime) && !loanLock.Ext2LockTime.Contains("01/01/1900"))
                {
                    AddUpdatedFields(ref UpdatedFieldds, 7025, loanLock.Ext2LockTime);
                }
                else
                {
                    AddUpdatedFields(ref UpdatedFieldds, 7025, string.Empty);
                }
                if (!string.IsNullOrEmpty(loanLock.Ext2Term))
                    AddUpdatedFields(ref UpdatedFieldds, 1143, loanLock.Ext2Term);
                // Extension 3
                if (!string.IsNullOrEmpty(loanLock.Ext3Term))
                    AddUpdatedFields(ref UpdatedFieldds, 1143, loanLock.Ext3Term);
                //if (!string.IsNullOrEmpty(loanLock.Ext3LockExpDate) && !loanLock.Ext3LockExpDate.Contains("01/01/1900"))
                //    AddUpdatedFields(ref UpdatedFieldds, 6063, loanLock.Ext3LockExpDate);
                #endregion
                #region Loan Profit
                Record.LoanProfit loanProfit = da.GetLoanProfit(updateLockInfoRequest.FileId, ref err);
                if (loanProfit != null)
                {
                    // LenderCredit changed 2284-->812 LW 10/18/2013
                    AddUpdatedFields(ref UpdatedFieldds, 812, loanProfit.LenderCredit);    // 
                }
                #endregion

                #region Lender Info
                //Lender Info 
                Record.Contacts lender = da.GetLoanContactInfo(updateLockInfoRequest.FileId, "Lender", ref err);
                if (lender != null)
                {
                    tempValue = da.GetLoanFieldValue(updateLockInfoRequest.FileId, 6001);
                    tempValue = tempValue == null ? "" : tempValue.Trim();
                    if (lender.CompanyName.Trim().ToUpper() != tempValue.ToUpper())
                    {
                        AddUpdatedFields(ref UpdatedFieldds, 6001, lender.CompanyName.Trim());
                        if (!string.IsNullOrEmpty(lender.FirstName) && !string.IsNullOrEmpty(lender.LastName))
                            AddUpdatedFields(ref UpdatedFieldds, 6000, lender.FirstName + " " + lender.LastName);
                        else
                            AddUpdatedFields(ref UpdatedFieldds, 6000, string.Empty);
                        AddUpdatedFields(ref UpdatedFieldds, 6002, lender.BusinessPhone);
                        AddUpdatedFields(ref UpdatedFieldds, 6003, lender.MailingAddr);
                        if (!string.IsNullOrEmpty(lender.MailingCity) && !string.IsNullOrEmpty(lender.MailingState) && !string.IsNullOrEmpty(lender.MailingZip))
                            AddUpdatedFields(ref UpdatedFieldds, 6004, lender.MailingCity + ", " + lender.MailingState + " " + lender.MailingZip);
                        else
                            AddUpdatedFields(ref UpdatedFieldds, 6004, string.Empty);
                        AddUpdatedFields(ref UpdatedFieldds, 12358, lender.Email);
                        AddUpdatedFields(ref UpdatedFieldds, 12357, lender.CellPhone);
                    }
                }
                #endregion
                #region Loan Point Fields
                var fieldIds = new short[] { 2322, 2324, 2325, 2329, 2338, 3896, 4003, 4004, 12973 }; //ARM Caps Margin, ARM Adj Caps, ARM Life Cap, ARM Init Adj Cap, Escrow Taxes, Escrow Insurance, LPMI
                var profitIds = new short[] { 812, 4018, 11241, 11243, 11604, 12492, 12495, 12973, 12974, 12975, 12976, 12977, 6177 }; // Mandatory Final Price, Best Effort Price, Comp Total, Best Effort to LO, Hedge Cost, Cost on Sale,Orig Points, Discount Point, Ext Cost 1, Ext Cost 2
                var lockIds = new short[] { 12, 13, 1190, 1191, 1192, 1193, 1194, 1198, 921, 923, 924, 2729, 2836, 7505, 812, 11438, 6100, 6101, 12545, 4018, 6061, 6062, 6063, 7403 };
                tempValue = string.Empty;
                #region ARM Caps fields
                // ARMS Cap fields
                foreach (short fieldId in fieldIds)     // ARM Caps
                {
                    tempValue = da.GetLoanFieldValue(updateLockInfoRequest.FileId, fieldId);
                    // Escrow Insurance=4004, Escrow Taxes=4003
                    if (fieldId == 4003 || fieldId == 4004)
                    {
                        if ((tempValue.Trim().ToUpper() == "YES") ||
                            (tempValue.Trim().ToUpper() == "Y") ||
                            (tempValue.Trim().ToUpper() == "X"))
                        {
                            tempValue = "X";
                        }
                        else
                        {
                            tempValue = string.Empty;
                        }
                    }
                    AddUpdatedFields(ref UpdatedFieldds, fieldId, tempValue);
                }
                #endregion
                #region Loan Profitability Fields
                // Loan Profitability Fields
                foreach (short profitField in profitIds)
                {
                    tempValue = da.GetLoanFieldValue(updateLockInfoRequest.FileId, profitField);

                    AddUpdatedFields(ref UpdatedFieldds, profitField, tempValue);
                }
                #endregion
                #region Loan Lock Fields
                // Loan Lock fields
                foreach (short lockField in lockIds)
                {
                    tempValue = da.GetLoanFieldValue(updateLockInfoRequest.FileId, lockField);
                    // Purpose
                    if (lockField == 1190 || lockField == 1191 || lockField == 1192 || lockField == 1193 || lockField == 1194 || lockField == 1198)  // purpose
                        if (tempValue.Trim().ToUpper() == "YES" || tempValue.Trim().ToUpper() == "Y" || tempValue.Trim().ToUpper() == "X")
                        {
                            tempValue = "X";
                        }
                        else tempValue = string.Empty;
                    // Occupancy
                    if (lockField == 921 || lockField == 923 || lockField == 924)  // occupancy
                        if (tempValue.Trim().ToUpper() == "YES" || tempValue.Trim().ToUpper() == "Y" || tempValue.Trim().ToUpper() == "X")
                        {
                            tempValue = "X";
                        }
                        else tempValue = string.Empty;

                    // Lock Options
                    if (lockField == 6100 || lockField == 6101)  // Lock Options
                        if (tempValue.Trim().ToUpper() == "YES" || tempValue.Trim().ToUpper() == "Y" || tempValue.Trim().ToUpper() == "X")
                        {
                            tempValue = "X";
                        }
                        else tempValue = string.Empty;
                    AddUpdatedFields(ref UpdatedFieldds, lockField, tempValue);
                }
                #endregion
                if (UpdatedFieldds.Count <= 0)
                {
                    err = string.Format("UpdateLockInfo, # Fields needs to be updated={0}, FileId={1}.", UpdatedFieldds.Count, updateLockInfoRequest.FileId);
                    int Event_id = 9272;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return true;
                }
                // Update Point file
                if (pntLib.WritePointData(UpdatedFieldds, filePath, ref  err) == false)
                {
                    int Event_id = 9273;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                da.Sync_Loan_Table(updateLockInfoRequest.FileId, ref err);
                return true;
            }
            catch (Exception ex)
            {
                err = "Cannot update the specified Point file " + filePath + ", Exception:" + ex.Message;
                int Event_id = 9274;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (UpdatedFieldds != null)
                {
                    UpdatedFieldds.Clear();
                    UpdatedFieldds = null;
                }
                if (logErr)
                {
                    int Event_id = 9275;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
                else
                {
                    err = "Lock Info has been updated for Point file " + filePath;
                    int Event_id = 9003;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id, Category);
                    Trace.TraceInformation(err);
                }
            }
        }
        private bool UpdateLoanProfitability(UpdateLockInfoRequest req, ref string err)
        {
            err = "";
            bool logErr = false;
            int Event_id = 9267;

            #region check  parameters
            if ((req == null) || (req.hdr == null))
            {
                err = "UpdateLoanProfitability, Invalid  Request, request is null. ";
                Event_id = 9266;
                logErr = true;
                return false;
            }

            if (req.FileId <= 0)
            {
                err = "UpdateLoanProfitability, Missing FileId in the UpdateLockInfoRequest  request ";
                Event_id = 9267;
                logErr = true;
                return false;
            }
            Record.LoanProfit loanProfit = da.GetLoanProfit(req.FileId, ref err);
            if (loanProfit == null)
            {
                err = string.Format("UpdateLoanProfitability, No LoanProfit record found, FileID={0}. ", req.FileId);
                Event_id = 9267;
                logErr = true;
                return false;
            }
            string filePath = "";
            if (Get_CheckPointFile(req.FileId, ref filePath, false, FileMode.Open, ref err) == false)
            {
                err = string.Format("UpdateLoanProfitability, FileID={0], Error: {1}", req.FileId, err);
                Event_id = 9268;
                logErr = true;
                return false;
            }
            if (filePath == string.Empty)
            {
                err = string.Format("UpdateLoanProfitability, FileID={0},  filepath returned from Get_CheckPointFile is empty.", req.FileId);
                Event_id = 9269;
                logErr = true;
                return false;
            }
            #endregion

            List<Framework.FieldMap> UpdatedFieldds = new List<Framework.FieldMap>();
            try
            {
                string tempValue = string.Empty;
                UpdatedFieldds.Clear();

                #region Adding Loan Profit fields in the Point field list
                AddUpdatedFields(ref UpdatedFieldds, 812, loanProfit.LenderCredit);
                AddUpdatedFields(ref UpdatedFieldds, 7341, loanProfit.Investor);
                AddUpdatedFields(ref UpdatedFieldds, 12974, loanProfit.HedgeCost);
                AddUpdatedFields(ref UpdatedFieldds, 11604, loanProfit.MandatoryFinalPrice);
                AddUpdatedFields(ref UpdatedFieldds, 11601, loanProfit.CommitmentDate);
                AddUpdatedFields(ref UpdatedFieldds, 7339, loanProfit.CommitmentNumber);
                AddUpdatedFields(ref UpdatedFieldds, 11845, loanProfit.CommitmentTerm);
                AddUpdatedFields(ref UpdatedFieldds, 11602, loanProfit.CommitmentExpDate);
                #endregion

                // Update Point file
                if (pntLib.WritePointData(UpdatedFieldds, filePath, ref  err) == false)
                {
                    Event_id = 9273;
                    logErr = true;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("UpdateLoanProfitability, FileID={0}, Cannot update the specified Point file {1}. \r\nException: {2} ", req.FileId, filePath, ex.ToString());
                logErr = true;
                return false;
            }
            finally
            {
                if (UpdatedFieldds != null)
                {
                    UpdatedFieldds.Clear();
                    UpdatedFieldds = null;
                }
                if (logErr)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
                else
                {
                    err = string.Format("UpdateLoanProfitability, FileID={0}, LoanProfit fields has been updated in Point file, {1}. ", req.FileId, filePath);
                    Event_id = 9003;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id, Category);
                    Trace.TraceInformation(err);
                }
            }
        }
        public UpdateConditionsResponse UpdateConditions(UpdateConditionsRequest req)
        {
            UpdateConditionsResponse resp = new UpdateConditionsResponse();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = false;
            string err = string.Empty;

            try
            {
                var pd = new PointData(pntLib, da);
                pd.UpdateConditions(req.FileId, req.hdr.UserId, req.ConditionList, ref err);
                resp.hdr.Successful = true;
            }
            catch (Exception exception)
            {
                err = exception.ToString();
                resp.hdr.Successful = false;
                resp.hdr.StatusInfo = err;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, string.Format("UpdateConditions, FileId={0}, Exception:{1}", req.FileId, err), EventLogEntryType.Error);
            }
            return resp;
        }
    }
}
                #endregion
        #endregion