using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Common;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Text;
using System.ServiceModel.Description;
using System.Threading;
using System.Windows.Forms;
using Common;
using DataAccess;
using System.Reflection;
using System.ServiceModel;
using EmailManager;
using Framework;
using LP2.Service;
using MailChimpMgr;
using focusIT;

using LP2.Service.Common;
using RuleManager;
using System.Net;
//using Excel = Microsoft.Office.Interop.Excel;

namespace LP2Service
{
    internal class InfoHubService : ServiceBase
    {
        static ServiceHost sh = null;

        public InfoHubService()
        {
            if (!EventLog.SourceExists(InfoHubEventLog.LogSource))
                EventLog.CreateEventSource(InfoHubEventLog.LogSource, InfoHubEventLog.LogType);
            this.ServiceName = InfoHubEventLog.LogSource;

            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
        }

        private static void Main()
        {
            Process aProcess = Process.GetCurrentProcess();
            string aProcName = aProcess.ProcessName;
            InfoHubService infoHubService = new InfoHubService();
            if (Process.GetProcessesByName(aProcName).Length > 1)
            {
                Application.ExitThread();
            }
            else
            {
                try
                {
                    System.Diagnostics.Trace.Listeners.Add(new TextWriterTraceListener("InfoHubService.Log"));

                    LPService.ServiceThreadDelegate = new ThreadStart(LPService.LP2_ServiceStart);
                    LPService.SPHostThread = new Thread(LPService.ServiceThreadDelegate);
                    //LPService.SPHostThread.Start();
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, "Successfully started Service Thread.");

                    Thread.Sleep(20);

                    LPService.SAUIDelegate = new ThreadStart(LPService.Scheduled_AD_User_Imports);    // 11nn
                    LPService.SAUIThread = new Thread(LPService.SAUIDelegate);
                    //LPService.SAUIThread.Start();

                    Thread.Sleep(20);

                    LPService.SPIDelegate = new ThreadStart(LPService.Scheduled_Point_Imports);     //  90nn
                    LPService.SPIThread = new Thread(LPService.SPIDelegate);
                    LPService.SPIThread.Start();

                    Thread.Sleep(20);

                    LPService.SMLDelegate = new ThreadStart(LPService.Scheduled_MonitorLoans);        // 91nn
                    LPService.SMLThread = new Thread(LPService.SMLDelegate);
                    //LPService.SMLThread.Start();

                    Thread.Sleep(20);

                    //LPService.SPEDelegate = new ThreadStart(LPService.Scheduled_PointExports);        // 92nn
                    //LPService.SPEThread = new Thread(LPService.SPEDelegate);
                    //LPService.SPEThread.Start();

                    //Thread.Sleep(20);

                    LPService.SPRIDelegate = new ThreadStart(LPService.Scheduled_Prospect_Imports);    // 80nn
                    LPService.SPRIThread = new Thread(LPService.SPRIDelegate);
                    //LPService.SPRIThread.Start();

                    Thread.Sleep(20);

                    //LPService.SMPDelegate = new ThreadStart(LPService.Scheduled_MonitorProspects);     // 81nn
                    //LPService.SMPThread = new Thread(LPService.SMPDelegate);
                    //LPService.SMPThread.Start();

                    //Thread.Sleep(20);

                    LPService.DataTracDelegate = new ThreadStart(LPService.Scheduled_DataTrac);       //  70nn
                    LPService.DataTracThread = new Thread(LPService.DataTracDelegate);
                    //LPService.DataTracThread.Start();

                    Thread.Sleep(20);

                    LPService.SSDelegate = new ThreadStart(LPService.Scheduled_SyncMarketingData);   //  60nn
                    LPService.SSThread = new Thread(LPService.SSDelegate);
                    //LPService.SSThread.Start();

                    //Thread.Sleep(20);

                    //LPService.SMEDelegate = new ThreadStart(LPService.Scheduled_MarketingEvents);      //  61nn
                    //LPService.SMEThread = new Thread(LPService.SMEDelegate);
                    //LPService.SMEThread.Start();

                    Thread.Sleep(20);

                    LPService.SEQDelegate = new ThreadStart(LPService.Scheduled_Email_Que);         //  50nn
                    LPService.SEQThread = new Thread(LPService.SEQDelegate);
                    //LPService.SEQThread.Start();

                    Thread.Sleep(20);

                    LPService.SRADelegate = new ThreadStart(LPService.Scheduled_Rule_Alert);       //  40nn
                    LPService.SRAThread = new Thread(LPService.SRADelegate);
                    //LPService.SRAThread.Start();

                    Thread.Sleep(20);

                    LPService.SRMDelegate = new ThreadStart(LPService.Scheduled_LSR);              // 30nn
                    LPService.SRMThread = new Thread(LPService.SRMDelegate);
                    //LPService.SRMThread.Start();

                    Thread.Sleep(20);

                    LPService.SGRDelegate = new ThreadStart(LPService.Scheduled_Get_Replies);       // 20nn
                    LPService.SGRThread = new Thread(LPService.SGRDelegate);
                    //LPService.SGRThread.Start();

                    Thread.Sleep(20);

                    LPService.SMDelegate = new ThreadStart(LPService.Scheduled_MailChimp);       // 20nn
                    LPService.SMThread = new Thread(LPService.SMDelegate);
                    //LPService.SMThread.Start();

                    Thread.Sleep(20);
                    // for demo system only
                    DataAccess.DataAccess da = new DataAccess.DataAccess();
                    if (da.GetRunningEdition() == 1000)            // demo only
                    {
                        LPService.SDemoDelegate = new ThreadStart(LPService.Scheduled_DemoMoveDueDates);
                        LPService.SDemoThread = new Thread(LPService.SDemoDelegate);
                        //LPService.SDemoThread.Start();
                    }
                }
                catch (Exception ex)
                {
                    int Event_id = 9999;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.Message + ex.StackTrace, EventLogEntryType.Error, Event_id);
                    Trace.TraceError(System.Reflection.MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message +
                                     ex.StackTrace);
                }
            }

            //ServiceBase.Run(new InfoHubService());
        }

        #region OnActions

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            if (sh != null)
                sh.Close();

            base.OnStop();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnContinue()
        {
            base.OnContinue();
        }

        protected override void OnShutdown()
        {
            if (sh != null)
                sh.Close();
            base.OnShutdown();
        }

        protected override void OnCustomCommand(int command)
        {
            base.OnCustomCommand(command);
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
        }

        #endregion

    }

    internal class LPService
    {
        private static DataAccess.DataAccess da = new DataAccess.DataAccess();
        private static LP2.Service.UserManager um = UserManager.Instance;
        private static PointMgr m_PointMgr = PointMgr.Instance;
        private static EmailManager.EmailMgr m_EmailMgr = EmailMgr.Instance;
        private static RuleManager.RuleMgr m_RuleMgr = RuleMgr.Instance;

        public static ServiceHost serviceHost = null;
        public static Thread SPHostThread = null;
        public static ThreadStart ServiceThreadDelegate = null;

        public static Thread SAUIThread = null;
        public static ThreadStart SAUIDelegate = null;
        public static ThreadStart SPIDelegate = null;
        public static Thread SPIThread = null;
        public static ThreadStart SPRIDelegate = null;
        public static Thread SPRIThread = null;
        public static ThreadStart SMLDelegate = null;
        public static Thread SMLThread = null;
        public static ThreadStart SMPDelegate = null;
        public static Thread SMPThread = null;
        public static ThreadStart SPEDelegate = null;
        public static Thread SPEThread = null;
        public static ThreadStart SEQDelegate = null;
        public static Thread SEQThread = null;
        public static ThreadStart SRADelegate = null;
        public static Thread SRAThread = null;
        public static ThreadStart SGRDelegate = null;
        public static Thread SGRThread = null;
        public static ThreadStart SDemoDelegate = null;
        public static Thread SDemoThread = null;
        public static ThreadStart SRMDelegate = null;
        public static Thread SRMThread = null;
        public static ThreadStart SSDelegate = null;
        public static Thread SSThread = null;
        public static ThreadStart SNDelegate = null;
        public static Thread SNThread = null;
        public static ThreadStart SMDelegate = null;
        public static Thread SMThread = null;


        // DataTrac
        public static ThreadStart DataTracDelegate = null;
        public static Thread DataTracThread = null;

        #region Import AD Users
        public static bool AD_User_Imports_Process()
        {
            int reqId = 0;
            int userid = 0;
            string cmd = "ImportADUsers";
            string err = "";
            string AD_OU_Filter = "";
            bool status = false;

            da.AddRequestQueue(userid, cmd, ref reqId, ref err);
            status = um.ImportUsers(AD_OU_Filter, userid, reqId, ref err);
            return status;
        }

        [STAThread]
        public static void Scheduled_AD_User_Imports()
        {
            string err = "";
            bool status = true;
            int Event_id = 1199;

            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            int ImportUserInterval = 6;

            while (true)
            {
                ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                            DateTime.Now.Minute * 60 * 1000 +
                                                            DateTime.Now.Second * 1000 +
                                                            DateTime.Now.Millisecond;

                try
                {
                    status = da.GetADConfigData(ref ImportUserInterval, ref err);

                    if (status == false)
                    {
                        err = "Failed to get AD Configuration data, err:" + err;
                        Trace.TraceError(err);
                        EventLog.WriteEntry(InfoHubEventLog.LogSource,
                                            MethodBase.GetCurrentMethod() + ", " + err,
                                            EventLogEntryType.Error,
                                            Event_id);
                        ImportUserInterval = 6;
                    }
                    else
                    {
                        status = AD_User_Imports_Process();
                        //ImportUserInterval = 6;
                    }
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource,
                                        ex.Message + ex.StackTrace,
                                        EventLogEntryType.Error,
                                        Event_id);
                    Trace.TraceError(ex.Message);
                    ImportUserInterval = 6;
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                       DateTime.Now.Minute * 60 * 1000 +
                                                       DateTime.Now.Second * 1000 +
                                                       DateTime.Now.Millisecond;

                ProcessSleepingTime = ImportUserInterval * 60 * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;

                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;

                Thread.Sleep(ProcessSleepingTime);
            }
        }
        #endregion

        #region Demo code to move the due dates
        public static void Demo_MoveDates()
        {
            try
            {
                int edition;
                edition = da.GetRunningEdition();
                if (edition == 1000)                       // this is for demo only, if it's 1000 we assume it's the demo system
                {
                    DataSet ds = null;
                    string sqlCmd = string.Empty;
                    DataSet ds1 = null;

                    WorkflowManager wm = WorkflowManager.Instance;

                    sqlCmd = "Select FileId, Status, EstCloseDate from Loans where [Status]='Processing' OR [Status]='Prospect'";
                    ds = DbHelperSQL.Query(sqlCmd);
                    if (ds == null || ds.Tables[0].Rows.Count <= 0)
                        return;
                    DateTime EstCloseDate = DateTime.MinValue;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr[0] == DBNull.Value || dr[1] == DBNull.Value || dr[2] == DBNull.Value)
                            continue;
                        int FileId = (int)dr[0];
                        if (FileId <= 0)
                            continue;
                        string Status = dr[1].ToString();
                        if (string.IsNullOrEmpty(Status))
                            continue;
                        EstCloseDate = (DateTime)dr[2];
                        sqlCmd = "select Count(1) from LoanStages where dbo.lpfn_GetStageIcon(LoanStageId)='StageRed.png' AND FileId=" + FileId;
                        object tempCount = DbHelperSQL.GetSingle(sqlCmd);
                        if (tempCount == null || tempCount == DBNull.Value)
                            continue;

                        int RedStageCount = (int)tempCount;
                        if (RedStageCount <= 1)
                            continue;

                        SqlParameter[] parameters = {
                             new SqlParameter("@FileId", SqlDbType.Int),
                             new SqlParameter("@LoanStatus", SqlDbType.NVarChar, 255)
                             };
                        parameters[0].Value = FileId;
                        parameters[1].Value = Status;
                        DbHelperSQL.RunProcedure("dbo.[lpsp_DemoMoveDates]", parameters);
                    }
                    sqlCmd = "Update Loans set DateClose=GetDate() where [Status]='Closed'";
                    DbHelperSQL.ExecuteSql(sqlCmd);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(InfoHubEventLog.LogSource,
                                    ex.Message + ex.StackTrace,
                                    EventLogEntryType.Error);
                Trace.TraceError(ex.Message);
            }
        }
        [STAThread]
        public static void Scheduled_DemoMoveDueDates()
        {
            try
            {
                int edition;
                edition = da.GetRunningEdition();
                if (edition != 1000)
                    return;
            }
            catch (Exception ex)
            {
                return;
            }
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;

            DateTime stDt = DateTime.Now;
            DateTime edDt = stDt.AddDays(1);
            string endDate = edDt.ToString("d") + @" 02:00:00AM";
            edDt = Convert.ToDateTime(endDate);
            TimeSpan ts = edDt - stDt;
            int days = ts.Days;
            int hrs = ts.Hours;
            int min = ts.Minutes;
            int MonitorInterval = 24 * 5;                            // every 5 days
            WorkflowManager wm = WorkflowManager.Instance;
            PointMgr pm = PointMgr.Instance;
            ProcessSleepingTime = ts.Hours * 60 * 60 * 1000 +
                                                      ts.Minutes * 60 * 1000 +
                                                      ts.Seconds * 1000 +
                                                      ts.Milliseconds;
            while (true)
            {
                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;
                Thread.Sleep(ProcessSleepingTime);
                try
                {
                    ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                                DateTime.Now.Minute * 60 * 1000 +
                                                                DateTime.Now.Second * 1000 +
                                                                DateTime.Now.Millisecond;

                    Debug.WriteLine("starting Scheduled_DemoMoveDueDates...");
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, "starting Scheduled_DemoMoveDueDates...", EventLogEntryType.Information);
                    Demo_MoveDates();
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, "completed Scheduled_DemoMoveDueDates.", EventLogEntryType.Information);
                    Debug.WriteLine("completed Scheduled_DemoMoveDueDates.");
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource,
                                                         ex.Message + ex.StackTrace,
                                                         EventLogEntryType.Error);
                    Trace.TraceError(ex.Message);
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                       DateTime.Now.Minute * 60 * 1000 +
                                                       DateTime.Now.Second * 1000 +
                                                       DateTime.Now.Millisecond;

                ProcessSleepingTime = MonitorInterval * 60 * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;
            }
        }

        #endregion
        #region Monitor Loans' Rate Lock and Task Alerts
        [STAThread]
        public static void Scheduled_MonitorLoans()
        {
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            string err = string.Empty;
            DateTime stDt = DateTime.Now;
            DateTime edDt = stDt.AddDays(1);
            string endDate = edDt.ToString("d") + @" 01:00:00AM";
            edDt = Convert.ToDateTime(endDate);
            TimeSpan ts = edDt - stDt;
            int days = ts.Days;
            int hrs = ts.Hours;
            int min = ts.Minutes;
            int MonitorInterval = 24;                            // every 24 hours
            WorkflowManager wm = WorkflowManager.Instance;
            PointMgr pm = PointMgr.Instance;
            ProcessSleepingTime = ts.Hours * 60 * 60 * 1000 +
                                                      ts.Minutes * 60 * 1000 +
                                                      ts.Seconds * 1000 +
                                                      ts.Milliseconds;
            while (true)
            {
                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;
                Thread.Sleep(ProcessSleepingTime);
                try
                {
                    ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                                DateTime.Now.Minute * 60 * 1000 +
                                                                DateTime.Now.Second * 1000 +
                                                                DateTime.Now.Millisecond;
                    wm.MonitorLoans();
                    pm.MonitorLoanStages();
                    wm.MonitorProspectTasks();
                    wm.MonitorProspectLoans();
                    if (pm.ExportTaskHistory(ref err) == false)
                    {
                        Trace.TraceError(err);
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                    }
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource,
                                                         ex.Message + ex.StackTrace,
                                                         EventLogEntryType.Error);
                    Trace.TraceError(ex.Message);
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                       DateTime.Now.Minute * 60 * 1000 +
                                                       DateTime.Now.Second * 1000 +
                                                       DateTime.Now.Millisecond;

                ProcessSleepingTime = MonitorInterval * 60 * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;
            }
        }
        #endregion
        //#region Monitor Prospect Alerts
        //public static void Scheduled_MonitorProspects()
        //{
        //    int ProcessSleepingTime = 0;
        //    int ProcessBeginningTime = 0;
        //    int ProcessEndingTime = 0;

        //    DateTime stDt = DateTime.Now;
        //    DateTime edDt = stDt.AddDays(1);
        //    string endDate = edDt.ToString("d") + @" 01:00:00AM";
        //    edDt = Convert.ToDateTime(endDate);
        //    TimeSpan ts = edDt - stDt;
        //    int days = ts.Days;
        //    int hrs = ts.Hours;
        //    int min = ts.Minutes;
        //    int MonitorInterval = 24;                            // every 24 hours
        //    WorkflowManager wm = WorkflowManager.Instance;
        //    PointMgr pm = PointMgr.Instance;
        //    ProcessSleepingTime = ts.Hours * 60 * 60 * 1000 +
        //                                              ts.Minutes * 60 * 1000 +
        //                                              ts.Seconds * 1000 +
        //                                              ts.Milliseconds;
        //    while (true)
        //    {
        //        if (ProcessSleepingTime <= 2000)
        //            ProcessSleepingTime = 2000;
        //        Thread.Sleep(ProcessSleepingTime);
        //        try
        //        {
        //            ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
        //                                                        DateTime.Now.Minute * 60 * 1000 +
        //                                                        DateTime.Now.Second * 1000 +
        //                                                        DateTime.Now.Millisecond;
        //            wm.MonitorProspectTasks();
        //            wm.MonitorProspectLoans();
        //        }
        //        catch (Exception ex)
        //        {
        //            EventLog.WriteEntry(InfoHubEventLog.LogSource,
        //                                                 ex.Message + ex.StackTrace,
        //                                                 EventLogEntryType.Error);
        //            Trace.TraceError(ex.Message);
        //        }

        //        ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
        //                                               DateTime.Now.Minute * 60 * 1000 +
        //                                               DateTime.Now.Second * 1000 +
        //                                               DateTime.Now.Millisecond;

        //        ProcessSleepingTime = MonitorInterval * 60 * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;
        //    }
        //}
        //#endregion
        //#region Point Exports
        //[STAThread]
        //public static void Scheduled_PointExports()
        //{
        //    int ProcessSleepingTime = 0;
        //    int ProcessBeginningTime = 0;
        //    int ProcessEndingTime = 0;
        //    string err = "";
        //    DateTime stDt = DateTime.Now;
        //    DateTime edDt = stDt.AddDays(1);
        //    string endDate = edDt.ToString("d") + @" 23:00:00PM";
        //    edDt = Convert.ToDateTime(endDate);
        //    TimeSpan ts = edDt - stDt;
        //    int days = ts.Days;
        //    int hrs = ts.Hours;
        //    int min = ts.Minutes;
        //    int MonitorInterval = 24;                            // every 24 hours
        //    PointMgr pm = PointMgr.Instance;
        //    ProcessSleepingTime = ts.Hours * 60 * 60 * 1000 +
        //                                              ts.Minutes * 60 * 1000 +
        //                                              ts.Seconds * 1000 +
        //                                              ts.Milliseconds;
        //    while (true)
        //    {
        //        if (ProcessSleepingTime <= 2000)
        //            ProcessSleepingTime = 2000;
        //        Thread.Sleep(ProcessSleepingTime);
        //        try
        //        {
        //            ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
        //                                                        DateTime.Now.Minute * 60 * 1000 +
        //                                                        DateTime.Now.Second * 1000 +
        //                                                        DateTime.Now.Millisecond;
        //            if (m_PointMgr == null)
        //                m_PointMgr = PointMgr.Instance;
        //            if (m_PointMgr != null && pm.ExportTaskHistory(ref err) == false)
        //            {
        //                Trace.TraceError(err);
        //                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            EventLog.WriteEntry(InfoHubEventLog.LogSource,
        //                                                 ex.Message + ex.StackTrace,
        //                                                 EventLogEntryType.Error);
        //            Trace.TraceError(ex.Message);
        //        }

        //        ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
        //                                               DateTime.Now.Minute * 60 * 1000 +
        //                                               DateTime.Now.Second * 1000 +
        //                                               DateTime.Now.Millisecond;

        //        ProcessSleepingTime = MonitorInterval * 60 * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;
        //    }
        //}
        //#endregion
        #region Point Imports
        static bool GetPDSFolders(ref string err)
        {
            err = "";
            bool logErr = false;
            List<PointFolderInfo> folderList = null;
            try
            {
                folderList = da.GetPDSPointFolders(ref err);
                if ((folderList == null) || (folderList.Count <= 0))
                {
                    err = MethodBase.GetCurrentMethod() + ", no PDS Folders available, err " + err;
                    logErr = true;
                    return false;
                }
                foreach (PointFolderInfo pf in folderList)
                {
                    if (da.Add_PointFolders(pf.Name, pf.Path, ref err) <= 0)
                    {
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, MethodBase.GetCurrentMethod() + ", " + err, EventLogEntryType.Error);
                        Trace.TraceError(err);
                    }
                }
            }
            catch (Exception ex)
            {
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                logErr = true;
                return false;
            }
            finally
            {
                if (folderList != null)
                {
                    folderList.Clear();
                    folderList = null;
                }
                if (logErr)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
                    Trace.TraceError(err);
                }
            }
            return true;
        }

        public static bool Winpoint_Setup(ref int PointImportInterval)
        {
            bool logErr = false;
            int len = 0;
            int index = 0;
            int FolderId = 0;
            int exitflag = 0;
            bool usePDS = false;
            string err = "";
            string line = "";
            string name = "";
            string folderline = "";
            string folderpath = "";
            string folderfilename = "";
            string WinpointPath = "";

            PointConfig pc = da.GetPointConfigData(ref err);

            if (pc == null)
            {
                err = "(Winpoint_Setup)Failed to get Point Configuration data, err:" + err;
                logErr = true;
                return false;
            }

            WinpointPath = pc.WinPointIni.Trim();
            PointImportInterval = pc.ImportInterval;

            if ((WinpointPath == null) || (WinpointPath == String.Empty))
                usePDS = true;
            else
                if (!File.Exists(WinpointPath))
                    usePDS = true;

            if (usePDS)
            {
                GetPDSFolders(ref err);
                return true;
            }

            StreamReader file = null;
            StreamReader folderfile = null;
            try
            {
                file = new StreamReader(WinpointPath);
                while ((line = file.ReadLine()) != null)
                {
                    index = line.IndexOf("Folder");
                    if (index >= 0)
                    {
                        index = line.LastIndexOf("\\");
                        if (index >= 0)
                        {
                            index = line.LastIndexOf("=");
                            name = line.Substring(0, index);
                            name = name.Trim();
                            index = index + 1;
                            len = line.Length - index;

                            folderpath = line.Substring(index, len);
                            folderpath = folderpath.Trim();
                            folderfilename = folderpath + @"\FOLDER.INI";
                            if (File.Exists(folderfilename))
                            {
                                exitflag = 0;
                                folderfile = new StreamReader(folderfilename);
                                while (exitflag == 0)
                                {
                                    if ((folderline = folderfile.ReadLine()) != null)
                                    {
                                        index = folderline.IndexOf("Name");
                                        if (index >= 0)
                                        {
                                            index = folderline.LastIndexOf("=");
                                            index = index + 1;
                                            len = folderline.Length - index;

                                            name = folderline.Substring(index, len);
                                            name = name.Trim();
                                            exitflag = 1;
                                        }
                                    }
                                    else
                                    {
                                        exitflag = 1;
                                    }
                                }
                            }
                            FolderId = da.Add_PointFolders(name, folderpath, ref err);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                err = MethodBase.GetCurrentMethod() + ", exception:" + ex.Message;
                logErr = true;
                return false;
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, MethodBase.GetCurrentMethod() + ", " + err, EventLogEntryType.Error);
                }
            }

        }


        [STAThread]
        public static void Scheduled_Prospect_Imports()
        {
            string err = "";
            int PointImportInterval = 15;
            bool status = true;
            int Event_id = 8099;
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;

            int reqId = 0;
            List<DataAccess.PointFolderInfo> FolderList = null;
            while (true)
            {
                ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                       DateTime.Now.Minute * 60 * 1000 +
                                       DateTime.Now.Second * 1000 +
                                       DateTime.Now.Millisecond;

                try
                {
                    string sError = string.Empty;
                    PointConfig PointConfig1 = da.GetPointConfigData(ref sError);

                    if (PointConfig1 == null)
                    {
                        sError = "(Scheduled_Prospect_Imports)Failed to get Point Configuration data, err:" + sError;
                        Event_id = 8099;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, sError, EventLogEntryType.Error, Event_id);
                        Trace.TraceError(sError);
                    }
                    else
                    {
                        PointImportInterval = PointConfig1.ImportInterval;
                    }
                }
                catch (Exception ex)
                {
                    string sErrorMsg = "Exception happened when get Point Configuration data, err:" + ex.Message;

                    EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Error);
                    Trace.TraceError(sErrorMsg);
                }

                #region Import Prospects

                try
                {
                    if (FolderList != null)
                    {
                        FolderList.Clear();
                        FolderList = null;
                    }
                    da.AddRequestQueue(0, "Scheduled Point Prospect Import", ref reqId, ref err);
                    FolderList = da.GetPointFolders(6, true, ref err);
                    if (status == false)
                    {
                        PointImportInterval = 15;
                    }
                    else
                        if ((FolderList == null) || (FolderList.Count <= 0))
                        {
                            Event_id = 8011;
                            err = MethodBase.GetCurrentMethod() + ", No Prospect Point folders that are Enabled found.";
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);
                        }
                        else
                        {
                            string[] folderArr = { "" };
                            folderArr = new string[FolderList.Count];
                            int i = 0;

                            foreach (DataAccess.PointFolderInfo fi in FolderList)
                            {
                                folderArr[i] = fi.Path;
                                i++;
                            }
                            ImportAllLoansRequest req = new ImportAllLoansRequest();
                            req.hdr = new ReqHdr();
                            req.hdr.UserId = 0;
                            req.PointFolders = folderArr;
                            PointMgrEvent e = new PointMgrEvent(0, PointMgrCommandType.ImportAllLoans, 0, req);
                            if (m_PointMgr == null)
                                m_PointMgr = PointMgr.Instance;
                            if (m_PointMgr != null)
                            {
                                status = m_PointMgr.ImportAllLoans(e, false);
                            }
                        }
                }
                catch (Exception ex)
                {
                    Event_id = 8099;
                    err = MethodBase.GetCurrentMethod() + ", " + ex.Message;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id);
                    Trace.TraceError(err);
                    PointImportInterval = 15;
                }

                #endregion

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                    DateTime.Now.Minute * 60 * 1000 +
                                    DateTime.Now.Second * 1000 +
                                    DateTime.Now.Millisecond;

                ProcessSleepingTime = PointImportInterval * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;

                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;

                Thread.Sleep(ProcessSleepingTime);
            }
        }

        [STAThread]
        public static void Scheduled_Point_Imports()
        {
            string err = "";
            int PointImportInterval = 15;
            bool status = true;
            int Event_id = 9099;
            bool Point_Import_Flag = true;
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;

            int reqId = 0;
            List<DataAccess.PointFolderInfo> FolderList = null;
            while (true)
            {
                Point_Import_Flag = true;

                ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                       DateTime.Now.Minute * 60 * 1000 +
                                       DateTime.Now.Second * 1000 +
                                       DateTime.Now.Millisecond;

                status = Winpoint_Setup(ref PointImportInterval);

                #region Get Point Config

                try
                {
                    string sError = string.Empty;
                    PointConfig PointConfig1 = da.GetPointConfigData(ref sError);

                    if (PointConfig1 == null)
                    {
                        sError = "(Scheduled_Point_Imports)Failed to get Point Configuration data, err:" + sError;
                        Event_id = 9099;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, sError, EventLogEntryType.Error, Event_id);
                        Trace.TraceError(sError);
                    }
                    else
                    {
                        if (PointConfig1.MasterSource.Trim().ToLower() == "datatrac")
                            Point_Import_Flag = false;
                        PointImportInterval = PointConfig1.ImportInterval;
                    }
                    if (da == null)
                        da = new DataAccess.DataAccess();
                    if (da != null)
                    {
                        string errmsg = string.Empty;
                        int cnt = da.Sync_Loan_Table(ref errmsg);
                    }
                }
                catch (Exception ex)
                {
                    string sErrorMsg = "Exception happened when get Point Configuration data, err:" + ex.Message;

                    EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Error);
                    Trace.TraceError(sErrorMsg);
                }

                #endregion

                if (Point_Import_Flag == true)
                {
                    #region Import Point
                    try
                    {
                        if (FolderList != null)
                        {
                            FolderList.Clear();
                            FolderList = null;
                        }
                        da.AddRequestQueue(0, "Scheduled Point Import", ref reqId, ref err);
                        FolderList = da.GetPointFolders(1, true, ref err);
                        if (status == false)
                        {
                            PointImportInterval = 30;
                        }
                        else
                            if ((FolderList == null) || (FolderList.Count <= 0))
                            {
                                Event_id = 9011;
                                err = MethodBase.GetCurrentMethod() + ", No Processing Point folders that are Enabled found in PointFolders.";
                                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);
                            }
                            else
                            {
                                string[] folderArr = { "" };
                                folderArr = new string[FolderList.Count];
                                int i = 0;

                                foreach (DataAccess.PointFolderInfo fi in FolderList)
                                {
                                    folderArr[i] = fi.Path;
                                    i++;
                                }
                                ImportAllLoansRequest req = new ImportAllLoansRequest();
                                req.hdr = new ReqHdr();
                                req.hdr.UserId = 0;
                                req.PointFolders = folderArr;
                                PointMgrEvent e = new PointMgrEvent(0, PointMgrCommandType.ImportAllLoans, 0, req);
                                if (m_PointMgr == null)
                                    m_PointMgr = PointMgr.Instance;
                                if (m_PointMgr != null)
                                {
                                    status = m_PointMgr.ImportAllLoans(e, true);
                                }

                                if (da == null)
                                    da = new DataAccess.DataAccess();
                                if (da != null)
                                {
                                    Thread.Sleep(10000);
                                    string errmsg = string.Empty;
                                    int cnt = da.Sync_Loan_Table(ref errmsg);
                                }

                            }
                    }
                    catch (Exception ex)
                    {
                        err = MethodBase.GetCurrentMethod() + ", " + ex.Message;
                        Event_id = 9098;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id);
                        Trace.TraceError(err);
                        PointImportInterval = 15;
                    }

                    #endregion
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                    DateTime.Now.Minute * 60 * 1000 +
                                    DateTime.Now.Second * 1000 +
                                    DateTime.Now.Millisecond;

                ProcessSleepingTime = PointImportInterval * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;

                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;

                Thread.Sleep(ProcessSleepingTime);
            }
        }

        //public static void releaseObject(object obj)
        //{
        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
        //        obj = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        obj = null;
        //        MessageBox.Show("Unable to release the Object " + ex.ToString());
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //}

        //[STAThread]
        //public static void FieldID_Imports()
        //{
        //    Excel.Application app = new Excel.Application();
        //    Excel.Workbook workbook = app.Workbooks.Open(@"C:\WINDOWS\PointFieldIDMappingFile.xls",
        //        0,
        //        true,
        //        5,
        //        "",
        //        "",
        //        true,
        //        Microsoft.Office.Interop.Excel.XlPlatform.xlWindows,
        //        "\t",
        //        false,
        //        false,
        //        0,
        //        true,
        //        1,
        //        0); ;
        //    Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets.get_Item(1);
        //    Excel.Range range;

        //    int Id = 0;
        //    string err = "";
        //    string Label = "";

        //    int PointFieldId = 0;
        //    string str_PointFieldId = "";

        //    int DataType = 0;
        //    string str_DataType = "";

        //    int rCnt = 0;
        //    int cCnt = 0;

        //    range = worksheet.UsedRange;

        //    for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
        //    {
        //        for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
        //        {
        //            if (cCnt == 1)
        //            {
        //                str_PointFieldId = (string)(range.Cells[rCnt, cCnt] as Excel.Range).Text;
        //                PointFieldId = Convert.ToInt32(str_PointFieldId);
        //            }

        //            if (cCnt == 2)
        //            {
        //                Label = (string)(range.Cells[rCnt, cCnt] as Excel.Range).Value2;
        //            }

        //            if (cCnt == 3)
        //            {
        //                str_DataType = (string)(range.Cells[rCnt, cCnt] as Excel.Range).Text;
        //                DataType = Convert.ToInt32(str_DataType);
        //            }
        //        }

        //        Id = da.Save_PointFieldDesc(PointFieldId, Label, DataType, ref err);

        //    }

        //    workbook.Close(true, null, null);
        //    app.Quit();

        //    releaseObject(worksheet);
        //    releaseObject(workbook);
        //    releaseObject(app);
        //}
        #endregion
        #region WCF Service Host
        //[STAThread]
        public static void LP2_ServiceStart()
        {
            int Event_id = 1099;

            try
            {
                if (serviceHost != null)
                {
                    try
                    {
                        serviceHost.Close();
                    }
                    catch (Exception ee)
                    {
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, "LP2_ServiceStart, Failed to close Service Host, Exception: " + ee.Message);
                    }
                    serviceHost = null;
                }
                string strBaseAddressUri = ConfigurationManager.AppSettings["ServiceBaseAddress"];
                serviceHost = new ServiceHost(typeof(LP2Service), new Uri(strBaseAddressUri));

                ServiceMetadataBehavior MetadataBehavior = new ServiceMetadataBehavior();
                MetadataBehavior.HttpGetEnabled = true;
                serviceHost.Description.Behaviors.Add(MetadataBehavior);

                ServiceEndpoint MexEndpoint = serviceHost.AddServiceEndpoint(
                    ServiceMetadataBehavior.MexContractName,
                    MetadataExchangeBindings.CreateMexHttpBinding(),
                    "mex");

                BasicHttpBinding serviceHttpBinding = new BasicHttpBinding();
                int bufSize = serviceHttpBinding.MaxBufferSize;
                bufSize = 650000000;
                serviceHttpBinding.MaxBufferPoolSize = bufSize;
                serviceHttpBinding.MaxBufferSize = bufSize;
                serviceHttpBinding.MaxReceivedMessageSize = bufSize;
                serviceHttpBinding.ReaderQuotas.MaxArrayLength = bufSize;

                ServiceEndpoint ServiceEndpoint = serviceHost.AddServiceEndpoint(
                                        typeof(ILP2Service),
                                        serviceHttpBinding,
                                        "");

                ServiceEndpoint.Name = "InfoHubServiceEndPoint";

                serviceHost.Open();

                Event_id = 1001;

                EventLog.WriteEntry(InfoHubEventLog.LogSource, MethodBase.GetCurrentMethod() + ", successfully started Background WCF Service Host at " + strBaseAddressUri, EventLogEntryType.Information, Event_id);

                while (true)
                {
                    Thread.Sleep(86400000);
                }
            }
            catch (Exception ex)
            {
                Event_id = 1099;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, MethodBase.GetCurrentMethod() + ", " + ex.Message, EventLogEntryType.Error, Event_id);
                if (serviceHost != null)
                    serviceHost.Close();
            }

        }
        #endregion
        [STAThread]
        public static void Scheduled_Email_Que()
        {
            string err = "";
            bool status = true;

            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            int EmailInterval = 4 * 60; //default interval 4 hours


            while (true)
            {
                ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                            DateTime.Now.Minute * 60 * 1000 +
                                                            DateTime.Now.Second * 1000 +
                                                            DateTime.Now.Millisecond;

                try
                {
                    m_EmailMgr = EmailMgr.Instance;
                    if (m_EmailMgr.EmailServerSetting.EmailInterval > 0)
                    {
                        EmailInterval = m_EmailMgr.EmailServerSetting.EmailInterval;
                        /* testing */
                        EmailInterval = EmailInterval / 12;
                        //minutes
                    }

                    m_EmailMgr.ProcessEmailQue();
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource,
                                                         ex.Message + ex.StackTrace,
                                                         EventLogEntryType.Error);
                    Trace.TraceError(ex.Message);
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                       DateTime.Now.Minute * 60 * 1000 +
                                                       DateTime.Now.Second * 1000 +
                                                       DateTime.Now.Millisecond;

                ProcessSleepingTime = EmailInterval * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;

                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;

                Thread.Sleep(ProcessSleepingTime);
            }
        }

        #region Rule Alert

        [STAThread]
        public static void Scheduled_Rule_Alert()
        {
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            int RuleInterval = 4 * 60; //default interval 4 hours in minutes

            //RuleInterval = da.GetRuleMonitorInterval();
            //if (RuleInterval <= 0)
            //    RuleInterval = 4 * 60;

            Thread.Sleep(2 * 1000);

            while (true)
            {                
                ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                       DateTime.Now.Minute * 60 * 1000 +
                                       DateTime.Now.Second * 1000 +
                                       DateTime.Now.Millisecond;

                try
                {
                    m_RuleMgr.ProcessLoanRules();
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource,
                                        ex.Message + ex.StackTrace,
                                        EventLogEntryType.Error);
                    Trace.TraceError(ex.Message);
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                    DateTime.Now.Minute * 60 * 1000 +
                                    DateTime.Now.Second * 1000 +
                                    DateTime.Now.Millisecond;

                ProcessSleepingTime = RuleInterval * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;
                /* Testing */
                //ProcessSleepingTime = RuleInterval * 60 * 60 * 1000 / 12 + ProcessBeginningTime - ProcessEndingTime;

                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;

                Thread.Sleep(ProcessSleepingTime);
            }
        }

        #endregion


        #region Email Manager Get Replies

        [STAThread]
        public static void Scheduled_Get_Replies()
        {
            string err = "";
            bool status = true;

            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            int EmailInterval = 2 * 60; //default interval 2 hours


            while (true)
            {
                ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                       DateTime.Now.Minute * 60 * 1000 +
                                       DateTime.Now.Second * 1000 +
                                       DateTime.Now.Millisecond;

                try
                {
                    m_EmailMgr = EmailMgr.Instance;
                    //todo:check logic
                    if (m_EmailMgr.EmailServerSetting.SendEmailViaEWS == false)
                    {
                        return;
                    }
                    if (m_EmailMgr.EmailServerSetting.EmailInterval > 0)
                    {
                        EmailInterval = m_EmailMgr.EmailServerSetting.EmailInterval;
                    }

                    m_EmailMgr.GetReplies();
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource,
                                        ex.Message + ex.StackTrace,
                                        EventLogEntryType.Error);
                    Trace.TraceError(ex.Message);
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                    DateTime.Now.Minute * 60 * 1000 +
                                    DateTime.Now.Second * 1000 +
                                    DateTime.Now.Millisecond;

                ProcessSleepingTime = EmailInterval * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;

                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;

                Thread.Sleep(ProcessSleepingTime);
            }
        }

        #endregion

        #region LSR

        [STAThread]
        public static void Scheduled_LSR()
        {
            //int ProcessSleepingTime = 0;
            //int ProcessBeginningTime = 0;
            //int ProcessEndingTime = 0;
            //int LSRInterval = 24 * 60; //default interval 4 hours in minutes


            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;

            //DateTime stDt = DateTime.Now;
            //DateTime edDt = stDt.AddDays(1);
            //string endDate = edDt.ToString("d") + @" 02:00:00AM";
            //edDt = Convert.ToDateTime(endDate);
            //TimeSpan ts = edDt - stDt;
            //int days = ts.Days;
            //int hrs = ts.Hours;
            //int min = ts.Minutes;
            //int MonitorInterval = 24;                            // every 24 hours
            int MonitorInterval = 1;                            // every 1 hours

            //ProcessSleepingTime = ts.Hours * 60 * 60 * 1000 +
            //                                          ts.Minutes * 60 * 1000 +
            //                                          ts.Seconds * 1000 +
            //                                          ts.Milliseconds;
            while (true)
            {
                //if (ProcessSleepingTime <= 2000)
                //    ProcessSleepingTime = 2000;
                //Thread.Sleep(ProcessSleepingTime);
                try
                {
                    ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                                DateTime.Now.Minute * 60 * 1000 +
                                                                DateTime.Now.Second * 1000 +
                                                                DateTime.Now.Millisecond;

                    ReportManager rm = ReportManager.Instance;
                    rm.EmailLSR();
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource,
                                                         ex.Message + ex.StackTrace,
                                                         EventLogEntryType.Error);
                    Trace.TraceError(ex.Message);
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                       DateTime.Now.Minute * 60 * 1000 +
                                                       DateTime.Now.Second * 1000 +
                                                       DateTime.Now.Millisecond;

                ProcessSleepingTime = MonitorInterval * 60 * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;

                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;

                Thread.Sleep(ProcessSleepingTime);
            }
        }

        #endregion

        #region DataTrac

        [STAThread]
        public static void Scheduled_DataTrac()
        {
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            bool DataTrac_Flag = false;
            int Event_id = 6099;
            int ImportInterval = 15;
            bool Normal = true;

            while (true)
            {
                Normal = true;

                try
                {
                    DataTrac_Flag = false;

                    string sError = string.Empty;
                    PointConfig PointConfig1 = da.GetPointConfigData(ref sError);

                    if (PointConfig1 == null)
                    {
                        Normal = false;
                        Event_id = 6099;
                        sError = "(Scheduled_DataTrac)Failed to get Point Configuration data, err:" + sError;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, sError, EventLogEntryType.Error, Event_id);
                    }
                    else
                    {
                        if (PointConfig1.MasterSource.Trim().ToLower() == "datatrac")
                            DataTrac_Flag = true;
                    }
                }
                catch (Exception ex)
                {
                    Normal = false;
                    string sErrorMsg = "Exception happened when get Point Configuration data, err:" + ex.Message;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Error);
                }

                ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                       DateTime.Now.Minute * 60 * 1000 +
                                       DateTime.Now.Second * 1000 +
                                       DateTime.Now.Millisecond;

                if (Normal == true)
                {
                    if (DataTrac_Flag == true)
                    {
                        DT_ImportLoansRequest ImportLoansRequest = new DT_ImportLoansRequest();
                        DT_ImportLoansResponse ImportLoansResponse = null;
                        ImportLoansRequest.hdr = new ReqHdr();
                        ImportLoansRequest.hdr.UserId = 0;
                        int[] TargetFileIDs = new int[0];
                        ImportLoansRequest.FileIds = TargetFileIDs;

                        try
                        {
                            LP2Service LP2Service1 = new LP2Service();
                            ImportLoansResponse = LP2Service1.DTImportLoans(ImportLoansRequest);

                            if (ImportLoansResponse.hdr.Successful == false)
                            {
                                if (ImportLoansResponse.hdr.StatusInfo != "none")
                                {
                                    string sErrorMsg = "Failed to call DTImportLoans(), err:" + ImportLoansResponse.hdr.StatusInfo;
                                    EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Error);
                                }
                            }
                            else
                            {
                                DT_GetStatusDatesRequest GetStatusDatesRequest = new DT_GetStatusDatesRequest();
                                GetStatusDatesRequest.hdr = new ReqHdr();
                                GetStatusDatesRequest.hdr.UserId = 0;
                                GetStatusDatesRequest.FileIds = TargetFileIDs;

                                DT_GetStatusDatesResponse GetStatusDatesResponse = LP2Service1.DTGetStatusDates(GetStatusDatesRequest);

                                if (GetStatusDatesResponse.hdr.Successful == false)
                                {
                                    string sErrorMsg = "Failed to call DTGetStatusDates(), err:" + GetStatusDatesResponse.hdr.StatusInfo;
                                    EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string sErrorMsg = "Exception happened when call DTImportLoans() or DTGetStatusDates(), err:" + ex.Message;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Error);
                        }
                    }
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                    DateTime.Now.Minute * 60 * 1000 +
                                    DateTime.Now.Second * 1000 +
                                    DateTime.Now.Millisecond;

                ProcessSleepingTime = ImportInterval * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;

                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;

                Thread.Sleep(ProcessSleepingTime);
            }
        }

        #endregion

        #region Sync Marketing Data
        [STAThread]
        public static void Scheduled_SyncMarketingData()
        {
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            string err = "";
            DateTime stDt = DateTime.Now;
            DateTime edDt = stDt.AddDays(1);
            string endDate = edDt.ToString("d") + @" 08:00:00PM";
            edDt = Convert.ToDateTime(endDate);
            TimeSpan ts = edDt - stDt;
            int days = ts.Days;
            int hrs = ts.Hours;
            int min = ts.Minutes;
            int MonitorInterval = 24;                            // every 24 hours
            MarketingManager.MkgMgr mm = MarketingManager.MkgMgr.Instance;
            ProcessSleepingTime = ts.Hours * 60 * 60 * 1000 +
                                                      ts.Minutes * 60 * 1000 +
                                                      ts.Seconds * 1000 +
                                                      ts.Milliseconds;

            int rm = 0;
            Random r = new Random();

            while (true)
            {
                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;

                //Thread.Sleep(ProcessSleepingTime);

                try
                {
                    ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                                DateTime.Now.Minute * 60 * 1000 +
                                                                DateTime.Now.Second * 1000 +
                                                                DateTime.Now.Millisecond;

                    rm = r.Next(540);
                    rm = rm * 60 * 1000;
                    //Thread.Sleep(rm);

                    if (mm.Scheduled_SyncMarketingData(ref err) == false)
                    {
                        Trace.TraceError(err);
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                    }
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource,
                                                         ex.Message + ex.StackTrace,
                                                         EventLogEntryType.Error);
                    Trace.TraceError(ex.Message);
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                       DateTime.Now.Minute * 60 * 1000 +
                                                       DateTime.Now.Second * 1000 +
                                                       DateTime.Now.Millisecond;

                ProcessSleepingTime = MonitorInterval * 60 * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;
            }
        }

        #endregion

        #region MailChimp
        [STAThread]
        public static void Scheduled_MailChimp()
        {
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;

            DateTime stDt = DateTime.Now;
            DateTime edDt = stDt.AddDays(1);
            string endDate = edDt.ToString("d") + @" 03:00:00AM";
            edDt = Convert.ToDateTime(endDate);
            TimeSpan ts = edDt - stDt;
            int MonitorInterval = 24;                            // every 24 hours

            ProcessSleepingTime = ts.Hours * 60 * 60 * 1000 +
                                                      ts.Minutes * 60 * 1000 +
                                                      ts.Seconds * 1000 +
                                                      ts.Milliseconds;
            while (true)
            {
                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;
                Thread.Sleep(ProcessSleepingTime);
                try
                {
                    ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                                DateTime.Now.Minute * 60 * 1000 +
                                                                DateTime.Now.Second * 1000 +
                                                                DateTime.Now.Millisecond;

                    var mailChimpManager = new MailChimpManager();
                    mailChimpManager.ScheduleMailChimp();

                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource,
                                                         ex.Message + ex.StackTrace,
                                                         EventLogEntryType.Error);
                    Trace.TraceError(ex.Message);
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                       DateTime.Now.Minute * 60 * 1000 +
                                                       DateTime.Now.Second * 1000 +
                                                       DateTime.Now.Millisecond;

                ProcessSleepingTime = MonitorInterval * 60 * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;
            }
        }

        #endregion

        //#region Sync Marketing Data Now
        //public static void SyncMarketingData_Now()
        //{
        //    string err = "";
        //    bool status = true;            
        //    MarketingManager.MkgMgr mm = MarketingManager.MkgMgr.Instance;

        //    try
        //    {
        //        status = mm.SyncMarketingData(ref err);                  
        //        if (status == false)
        //            {
        //                Trace.TraceError(err);
        //                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.Message + ex.StackTrace, EventLogEntryType.Warning);
        //        Trace.TraceError(ex.Message);
        //    }

        //}

        //#endregion

        //#region Marketing Events
        //[STAThread]
        //public static void Scheduled_MarketingEvents()
        //{
        //    int ProcessSleepingTime = 0;
        //    int ProcessBeginningTime = 0;
        //    int ProcessEndingTime = 0;
        //    string err = "";
        //    DateTime stDt = DateTime.Now;
        //    DateTime edDt = stDt.AddDays(1);
        //    string endDate = edDt.ToString("d") + @" 02:00:00AM";
        //    edDt = Convert.ToDateTime(endDate);
        //    TimeSpan ts = edDt - stDt;
        //    int days = ts.Days;
        //    int hrs = ts.Hours;
        //    int min = ts.Minutes;
        //    int MonitorInterval = 10;                            // every 10 minutes
        //    MarketingManager.MkgMgr mm = MarketingManager.MkgMgr.Instance;
        //    //ProcessSleepingTime = ts.Hours * 60 * 60 * 1000 +
        //    //                                          ts.Minutes * 60 * 1000 +
        //    //                                          ts.Seconds * 1000 +
        //    //                                          ts.Milliseconds;
        //    while (true)
        //    {
        //        if (ProcessSleepingTime <= 2000)
        //            ProcessSleepingTime = 2000;

        //        try
        //        {
        //            ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
        //                                                        DateTime.Now.Minute * 60 * 1000 +
        //                                                        DateTime.Now.Second * 1000 +
        //                                                        DateTime.Now.Millisecond;
        //            if (mm.Scheduled_MarketingEvents(ref err) == false)
        //            {
        //                Trace.TraceError(err);
        //                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            EventLog.WriteEntry(InfoHubEventLog.LogSource,
        //                                                 ex.Message + ex.StackTrace,
        //                                                 EventLogEntryType.Error);
        //            Trace.TraceError(ex.Message);
        //        }

        //        ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
        //                                               DateTime.Now.Minute * 60 * 1000 +
        //                                               DateTime.Now.Second * 1000 +
        //                                               DateTime.Now.Millisecond;

        //        ProcessSleepingTime = MonitorInterval * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;
        //        Thread.Sleep(ProcessSleepingTime);
        //    }
        //}

        //#endregion
    }
}