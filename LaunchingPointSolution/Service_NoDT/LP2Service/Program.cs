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
using Utilities;
using LP2.Service.Common;
using RuleManager;
using System.Net;
//using Excel = Microsoft.Office.Interop.Excel;

namespace LP2Service
{
    #region InfoHub Service base
    internal class InfoHubService : ServiceBase
    {
        short Category = 10;
        static ServiceHost sh = null;
        static string myServiceName = ConfigurationManager.AppSettings.Get("ServiceName");
        public InfoHubService()
        {
            try
            {
                if (!string.IsNullOrEmpty(myServiceName))
                    InfoHubEventLog.LogSource = myServiceName;
                if (!EventLog.SourceExists(InfoHubEventLog.LogSource))
                    EventLog.CreateEventSource(InfoHubEventLog.LogSource, "Application");
                this.ServiceName = string.IsNullOrEmpty(myServiceName) ? InfoHubEventLog.LogSource : myServiceName.Trim();
                myServiceName = this.ServiceName;
                this.CanHandlePowerEvent = true;
                this.CanHandleSessionChangeEvent = true;
                this.CanPauseAndContinue = true;
                this.CanShutdown = true;
                this.CanStop = true;
            }
            catch (Exception ex)
            {
                if (!EventLog.SourceExists(InfoHubEventLog.LogSource))
                    EventLog.CreateEventSource(InfoHubEventLog.LogSource, "Application");
                string err = InfoHubEventLog.LogSource + " failed to instantiate, error: " + ex.ToString();
                int Event_id = 1091;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                LPLog.LogMessage(LogType.Logfatal, ex.ToString());
            }

        }
        #region Main - Creating threads
        private static void Main()
        {
            short Category = 10;
            Process aProcess = Process.GetCurrentProcess();
            string aProcName = aProcess.ProcessName;
            InfoHubService infoHubService = new InfoHubService();
            //if (Process.GetProcessesByName(aProcName).Length > 1)
            //{
            //    Application.ExitThread();
            //}
            //else
            //{
            try
            {
                System.Diagnostics.Trace.Listeners.Add(new TextWriterTraceListener(InfoHubService.myServiceName + ".Log"));

                LPService.ServiceThreadDelegate = new ThreadStart(LPService.LP2_ServiceStart);
                LPService.SPHostThread = new Thread(LPService.ServiceThreadDelegate);
                LPService.SPHostThread.Start();
                //int Event_id = 1000;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, "Successfully started Service Thread.", EventLogEntryType.Information,  Event_id, Category);

                Thread.Sleep(20);

                LPService.SAUIDelegate = new ThreadStart(LPService.Scheduled_AD_User_Imports);    // 11nn
                LPService.SAUIThread = new Thread(LPService.SAUIDelegate);
                LPService.SAUIThread.Start();

                Thread.Sleep(20);

                LPService.SPIDelegate = new ThreadStart(LPService.Scheduled_Point_Imports);     //  90nn
                LPService.SPIThread = new Thread(LPService.SPIDelegate);
                LPService.SPIThread.Start();

                Thread.Sleep(20);

                LPService.SMLDelegate = new ThreadStart(LPService.Scheduled_MonitorLoans);        // 91nn
                LPService.SMLThread = new Thread(LPService.SMLDelegate);
                LPService.SMLThread.Start();

                Thread.Sleep(20);

                LPService.SPRIDelegate = new ThreadStart(LPService.Scheduled_Prospect_Imports);    // 80nn
                LPService.SPRIThread = new Thread(LPService.SPRIDelegate);
                LPService.SPRIThread.Start();

                Thread.Sleep(20);

                LPService.SEQDelegate = new ThreadStart(LPService.Scheduled_Email_Que);         //  50nn
                LPService.SEQThread = new Thread(LPService.SEQDelegate);
                LPService.SEQThread.Start();

                Thread.Sleep(20);

                LPService.SRADelegate = new ThreadStart(LPService.Scheduled_Rule_Alert);       //  40nn
                LPService.SRAThread = new Thread(LPService.SRADelegate);
                LPService.SRAThread.Start();

                LPService.SRMDelegate = new ThreadStart(LPService.Scheduled_LSR);              // 30nn
                LPService.SRMThread = new Thread(LPService.SRMDelegate);
                LPService.SRMThread.Start();

                Thread.Sleep(20);

                LPService.SGRDelegate = new ThreadStart(LPService.Scheduled_Get_Replies);       // 20nn
                LPService.SGRThread = new Thread(LPService.SGRDelegate);
                LPService.SGRThread.Start();

                Thread.Sleep(20);

                LPService.SMDelegate = new ThreadStart(LPService.Scheduled_MailChimp);       // 20nn
                LPService.SMThread = new Thread(LPService.SMDelegate);
                LPService.SMThread.Start();

                Thread.Sleep(20);
                LPService.SyncPointStageDelegate = new ThreadStart(LPService.Scheduled_SyncPointStages);  // 20nn
                LPService.SyncPointStageThread = new Thread(LPService.SyncPointStageDelegate);
                LPService.SyncPointStageThread.Start();

                Thread.Sleep(20);
                LPService.PostActiveLoanDelegate = new ThreadStart(LPService.Scheduled_PostActiveLoan);  // 24 hrs
                LPService.PostActiveLoanThread = new Thread(LPService.PostActiveLoanDelegate);
                LPService.PostActiveLoanThread.Start();

                Thread.Sleep(20);
                LPService.PostArchivedLoanDelegate = new ThreadStart(LPService.Scheduled_PostArchivedLoan);  // 24 hrs
                LPService.PostArchivedLoanThread = new Thread(LPService.PostArchivedLoanDelegate);
                LPService.PostArchivedLoanThread.Start();

                Thread.Sleep(20);
                LPService.UpdateUserLeadsDistDelegate = new ThreadStart(LPService.Scheduled_UpdateUserLeadsDist);  // 24 hrs
                LPService.UpdateUserLeadsDistThread = new Thread(LPService.UpdateUserLeadsDistDelegate);
                LPService.UpdateUserLeadsDistThread.Start();

            }
            catch (Exception ex)
            {
                int Event_id = 1092;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.ToString(), EventLogEntryType.Error, Event_id, Category);
                if (ex.InnerException != null)
                {
                    Event_id = 1093;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.InnerException.Message + "\r\n" + ex.InnerException.StackTrace, EventLogEntryType.Error, Event_id, Category);
                }
                Trace.TraceError(System.Reflection.MethodBase.GetCurrentMethod() + ", Exception:" + ex.ToString());
            }
            //}

            ServiceBase.Run(new InfoHubService());
        }
        #endregion
        #region OnActions

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                base.OnStart(args);
            }
            catch (Exception ex)
            {
                if (!EventLog.SourceExists(InfoHubEventLog.LogSource))
                    EventLog.CreateEventSource(InfoHubEventLog.LogSource, "Application");
                int Event_id = 1094;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "ERROR Service OnStart failed to start " + ex.Message + "\r\n" + ex.StackTrace, EventLogEntryType.Error, Event_id, Category);
                LPLog.LogMessage(LogType.Logfatal, ex.Message + "\r\n" + ex.StackTrace);
            }
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
    #endregion

    internal class LPService
    {
        short Category = 10;
        #region LPService base initialization and declaring threads
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
        //public static ThreadStart SSDelegate = null;
        //public static Thread SSThread = null;
        public static ThreadStart SNDelegate = null;
        public static Thread SNThread = null;
        public static ThreadStart SMDelegate = null;
        public static Thread SMThread = null;

        public static ThreadStart SyncPointStageDelegate = null;
        public static Thread SyncPointStageThread = null;
        // DataTrac
        //public static ThreadStart DataTracDelegate = null;
        //public static Thread DataTracThread = null;

        //
        public static ThreadStart PostActiveLoanDelegate = null;
        public static Thread PostActiveLoanThread = null;

        public static ThreadStart PostArchivedLoanDelegate = null;
        public static Thread PostArchivedLoanThread = null;

        public static ThreadStart UpdateUserLeadsDistDelegate { get; set; }

        public static Thread UpdateUserLeadsDistThread { get; set; }

        #endregion
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
            short Category = 10;
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
                        Event_id = 1095;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, MethodBase.GetCurrentMethod() + ", " + err, EventLogEntryType.Error, Event_id, Category);
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
                    Event_id = 1096;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.Message + ex.StackTrace, EventLogEntryType.Error, Event_id, Category);
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
            short Category = 10;
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
                int Event_id = 1097;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.Message + ex.StackTrace, EventLogEntryType.Error, Event_id, Category);
                Trace.TraceError(ex.Message);
            }
        }

        [STAThread]
        public static void Scheduled_DemoMoveDueDates()
        {
            short Category = 10;
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
                    int Event_id = 1097;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.Message + ex.StackTrace, EventLogEntryType.Error, Event_id, Category);
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
            short Category = 10;
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
                        //int Event_id = 1001;
                        //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning,  Event_id, Category);
                    }
                }
                catch (Exception ex)
                {
                    int Event_id = 1098;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.Message + ex.StackTrace, EventLogEntryType.Error, Event_id, Category);
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
        #region Monitor Prospect Alerts - obsolete
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
        #endregion
        #region Sync Point Stages
        [STAThread]
        public static void Scheduled_SyncPointStages()
        {
            short Category = 10;
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            string err = "";
            DateTime stDt = DateTime.Now;
            DateTime edDt = stDt.AddHours(1);
            //string endDate = edDt.ToString("d") + @" 23:00:00PM";
            //edDt = Convert.ToDateTime(endDate);
            TimeSpan ts = edDt - stDt;
            int days = ts.Days;
            int hrs = ts.Hours;
            int min = ts.Minutes;
            int MonitorInterval = 1;                            // every 1 hours
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
                    if (pm == null)
                        pm = PointMgr.Instance;
                    if (pm == null)
                    {
                        err = string.Format("Scheduled_SyncPointStages, unable to get a Point Manager instance.");
                        Trace.TraceError(err);
                        int Event_id = 1002;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        continue;
                    }
                    if (pm.MonitorLoanStages() == false)
                    {
                        err = string.Format("Scheduled_SyncPointStages, failed to update Point Stages for all the active loans, error: pm.MonitorLoanStages return false");
                        Trace.TraceError(err);
                        int Event_id = 1003;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    else
                    {
                        err = string.Format("Scheduled_SyncPointStages, successfully update Point Stages for all the active loans.");
                        Trace.TraceError(err);
                        //int Event_id = 1004;
                        //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information,  Event_id, Category);
                    }
                }
                catch (Exception ex)
                {
                    int Event_id = 1099;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.Message + ex.StackTrace, EventLogEntryType.Error, Event_id, Category);
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
        #region Point Imports
        static bool GetPDSFolders(ref string err)
        {
            short Category = 10;
            err = "";
            bool logErr = false;
            List<PointFolderInfo> folderList = null;
            try
            {
                folderList = da.GetPDSPointFolders(ref err);
                if ((folderList == null) || (folderList.Count <= 0))
                {
                    err = MethodBase.GetCurrentMethod() + ", no PDS Folders available, err " + err;
                    int Event_id = 1005;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                foreach (PointFolderInfo pf in folderList)
                {
                    if (da.Add_PointFolders(pf.Name, pf.Path, pf.FolderId, ref err) <= 0)
                    {
                        int Event_id = 1089;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, MethodBase.GetCurrentMethod() + ", " + err, EventLogEntryType.Error, Event_id, Category);
                        Trace.TraceError(err);
                    }
                }
            }
            catch (Exception ex)
            {
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                int Event_id = 1006;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1007;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
            }
            return true;
        }

        public static bool Winpoint_Setup(ref int PointImportInterval)
        {
            short Category = 10;
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
                err = "Failed to get Point Configuration data, da.GetPointConfigData error: " + err;
                int Event_id = 1008;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                            FolderId = da.Add_PointFolders(name, folderpath, 0, ref err);//todo:check 
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                err = MethodBase.GetCurrentMethod() + ", exception:" + ex.Message;
                int Event_id = 1009;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    int Event_id = 1010;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, MethodBase.GetCurrentMethod() + ", " + err, EventLogEntryType.Warning, Event_id, Category);
                }
            }

        }


        [STAThread]
        public static void Scheduled_Prospect_Imports()
        {
            short Category = 10;
            string err = "";
            int PointImportInterval = 15;
            bool status = true;
            int Event_id = 1099;
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
                        Event_id = 1099;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, sError, EventLogEntryType.Error, Event_id, Category);
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

                    Event_id = 1098;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Error, Event_id, Category);
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
                            Event_id = 1011;
                            err = MethodBase.GetCurrentMethod() + ", No Prospect Point folders that are Enabled found.";
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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
                    Event_id = 1099;
                    err = MethodBase.GetCurrentMethod() + ", " + ex.Message;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
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
            short Category = 10;
            string err = "";
            int PointImportInterval = 15;
            bool status = true;
            int Event_id = 1099;
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
                        Event_id = 1099;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, sError, EventLogEntryType.Error, Event_id, Category);
                        Trace.TraceError(sError);
                    }
                    else
                    {
                        PointImportInterval = PointConfig1.ImportInterval;
                    }
                    if (da == null)
                        da = new DataAccess.DataAccess();
                    if (da != null)
                    {
                        string errmsg = string.Empty;
                        //int cnt = da.Sync_Loan_Table(ref errmsg);
                    }
                }
                catch (Exception ex)
                {
                    string sErrorMsg = "Exception happened when get Point Configuration data, err:" + ex.Message;
                    Event_id = 1088;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Error, Event_id, Category);
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
                                Event_id = 1011;
                                err = MethodBase.GetCurrentMethod() + ", No Processing Point folders that are Enabled found in PointFolders.";
                                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
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

                                if (da == null)
                                    da = new DataAccess.DataAccess();
                                if (da != null)
                                {
                                    Thread.Sleep(2000);
                                    string errmsg = string.Empty;

                                    int SyncBeginningTime = 0;

                                    DateTime stDt = DateTime.Now;
                                    DateTime edDt = DateTime.Now;
                                    string endDate = edDt.ToString("d") + @" 03:00:00AM";
                                    edDt = Convert.ToDateTime(endDate);
                                    if (edDt > stDt)
                                    {
                                        TimeSpan ts = edDt - stDt;
                                        SyncBeginningTime = ts.Hours * 60 * 60 * 1000 +
                                                            ts.Minutes * 60 * 1000 +
                                                            ts.Seconds * 1000 +
                                                            ts.Milliseconds;

                                        if (SyncBeginningTime <= 7200000)
                                        {
                                            int cnt = da.Sync_Loan_Table(ref errmsg);
                                        }
                                    }
                                }

                            }
                    }
                    catch (Exception ex)
                    {
                        err = MethodBase.GetCurrentMethod() + ", " + ex.Message;
                        Event_id = 1098;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
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

        #endregion
        #region WCF Service Host
        //[STAThread]
        public static void LP2_ServiceStart()
        {
            short Category = 10;
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
                        Event_id = 1010;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, "LP2_ServiceStart, Failed to close Service Host, Exception: " + ee.Message, EventLogEntryType.Warning, Event_id, Category);
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

                EventLog.WriteEntry(InfoHubEventLog.LogSource, MethodBase.GetCurrentMethod() + ", successfully started Background WCF Service Host at " + strBaseAddressUri, EventLogEntryType.Information, Event_id, Category);

                while (true)
                {
                    Thread.Sleep(86400000);
                }
            }
            catch (Exception ex)
            {
                Event_id = 1099;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, MethodBase.GetCurrentMethod() + ", " + ex.Message, EventLogEntryType.Error, Event_id, Category);
                if (serviceHost != null)
                    serviceHost.Close();
            }

        }
        #endregion
        #region Scheduled_Email_Queue thread
        [STAThread]
        public static void Scheduled_Email_Que()
        {
            short Category = 10;
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            int EmailInterval = 60;

            while (true)
            {
                ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                            DateTime.Now.Minute * 60 * 1000 +
                                                            DateTime.Now.Second * 1000 +
                                                            DateTime.Now.Millisecond;

                try
                {
                    m_EmailMgr = EmailMgr.Instance;
                    if (m_EmailMgr.EmailServerSetting.EmailInterval.HasValue)
                    {
                        EmailInterval = m_EmailMgr.EmailServerSetting.EmailInterval.Value;
                    }

                    m_EmailMgr.ProcessEmailQue();
                }
                catch (Exception ex)
                {
                    int Event_id = 1010;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.Message + ex.StackTrace, EventLogEntryType.Error, Event_id, Category);
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
        #region Rule Alert

        [STAThread]
        public static void Scheduled_Rule_Alert()
        {
            short Category = 10;
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            int RuleInterval = 30;

            Thread.Sleep(120 * 1000);

            while (true)
            {
                //RuleInterval = da.GetRuleMonitorInterval();
                m_EmailMgr = EmailMgr.Instance;

                if (m_EmailMgr.EmailServerSetting.EmailInterval.HasValue)
                {
                    RuleInterval = m_EmailMgr.EmailServerSetting.EmailInterval.Value;
                }
                if (RuleInterval <= 60)
                    RuleInterval = 60;

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
                    int Event_id = 1011;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.Message + ex.StackTrace, EventLogEntryType.Error, Event_id, Category);
                    Trace.TraceError(ex.Message);
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                    DateTime.Now.Minute * 60 * 1000 +
                                    DateTime.Now.Second * 1000 +
                                    DateTime.Now.Millisecond;

                ProcessSleepingTime = RuleInterval * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;

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
            short Category = 10;
            string err = "";
            bool status = true;

            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            int EmailInterval = 30;

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
                    if (m_EmailMgr.EmailServerSetting.EmailInterval.HasValue)
                    {
                        EmailInterval = m_EmailMgr.EmailServerSetting.EmailInterval.Value;
                    }

                    m_EmailMgr.GetReplies();
                }
                catch (Exception ex)
                {
                    int Event_id = 1012;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.Message + ex.StackTrace, EventLogEntryType.Error, Event_id, Category);
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

            short Category = 10;
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

            int iMin = 0;
            int iSeds = 0;
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
                    int Event_id = 1014;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.Message + ex.StackTrace, EventLogEntryType.Error, Event_id, Category);
                    Trace.TraceError(ex.Message);
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                       DateTime.Now.Minute * 60 * 1000 +
                                                       DateTime.Now.Second * 1000 +
                                                       DateTime.Now.Millisecond;

                TimeSpan ts = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + DateTime.Now.AddHours(1).Hour + ":00:00") - DateTime.Now;
                iMin = ts.Minutes;
                iSeds = ts.Seconds;

                //ProcessSleepingTime = MonitorInterval * 60 * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;
                ProcessSleepingTime = MonitorInterval * iMin * 60 * 1000 + iSeds * 1000 + ts.Milliseconds;

                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;

                Thread.Sleep(ProcessSleepingTime);
            }
        }

        #endregion

        #region MailChimp
        [STAThread]
        public static void Scheduled_MailChimp()
        {
            short Category = 10;
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
                    int Event_id = 1015;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.Message + ex.StackTrace, EventLogEntryType.Error, Event_id, Category);
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

        #region Post Loan

        [STAThread]
        public static void Scheduled_PostActiveLoan()
        {
            short Category = 10;
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            DataAccess.DataAccess da = new DataAccess.DataAccess();
            int interval = 60 * 24;
            int numProcessedLoans = 0;
            Table.Company_MCT mct = null;
            LoanPost loanPost = new LoanPost();

            while (true)
            {
                ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                       DateTime.Now.Minute * 60 * 1000 +
                                       DateTime.Now.Second * 1000 +
                                       DateTime.Now.Millisecond;
                try
                {
                    numProcessedLoans = 0;
                    da.GetCompanyMCT(ref mct);
                    interval = 60 * 24;
                    if (mct != null && (bool)mct.PostDataEnabled && mct.ActiveLoanInterval > 0)
                    {
                        interval = mct.ActiveLoanInterval;    // this interval is by minutes
                        if (string.IsNullOrEmpty(mct.PostURL))
                        {
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, "Unable to post active loans to MCT, reason: the Post URL is empty.", EventLogEntryType.Warning, 9202);
                        }
                        else
                        {
                            numProcessedLoans = loanPost.PostActiveLoan();
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, string.Format("Successfully posted {0} active loans to MCT at {1}.", numProcessedLoans, mct.PostURL), EventLogEntryType.Information, 9201);
                        }
                    }
                }
                catch (Exception ex)
                {
                    int Event_id = 1016;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, "Scheduled_PostActiveLoan \r\n" + ex.ToString(), EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(ex.Message);
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                    DateTime.Now.Minute * 60 * 1000 +
                                    DateTime.Now.Second * 1000 +
                                    DateTime.Now.Millisecond;

                ProcessSleepingTime = interval * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;

                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;

                Thread.Sleep(ProcessSleepingTime);
            }
        }
        private static void CleanupFiles(string folder, string filter)
        {
            if (string.IsNullOrEmpty(filter) || filter.ToUpper().Contains("BRW") || filter.ToUpper().Contains("PRS"))
                return;
            string[] files = Directory.GetFiles(folder, filter);
            foreach (string file in files)
            {
                DateTime ModTime = File.GetLastWriteTime(file);
                if (ModTime.Year <= DateTime.Now.Year && ModTime.DayOfYear <= DateTime.Now.DayOfYear - 30)
                {
                    File.Delete(file);
                }
            }
        }
        [STAThread]
        public static void Scheduled_PostArchivedLoan()
        {
            short Category = 10;
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            DataAccess.DataAccess da = new DataAccess.DataAccess();
            int interval = 60 * 24;
            Table.Company_MCT mct = null;
            LoanPost loanPost = new LoanPost();
            int numLoansProcessed = 0;

            while (true)
            {
                ProcessBeginningTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                       DateTime.Now.Minute * 60 * 1000 +
                                       DateTime.Now.Second * 1000 +
                                       DateTime.Now.Millisecond;

                try
                {
                    interval = 60 * 24;
                    numLoansProcessed = 0;
                    try
                    {
                        if (!string.IsNullOrEmpty(PointMgr.PointZipFolder) && Directory.Exists(PointMgr.PointZipFolder))
                        {
                            CleanupFiles(PointMgr.PointZipFolder, "*.zip");
                        }
                    }
                    catch (Exception ee)
                    { }
                    da.GetCompanyMCT(ref mct);
                    if (mct != null && (bool)mct.PostDataEnabled && mct.ArchivedLoanInterval > 0)
                    {
                        interval = 60 * mct.ArchivedLoanInterval;         // this interval is by hours
                        if (string.IsNullOrEmpty(mct.PostURL))
                        {
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, "Unable to post archived loans to MCT, reason: the Post URL is empty.", EventLogEntryType.Error, 9202);
                        }
                        else
                        {
                            numLoansProcessed = loanPost.PostArchivedLoan();
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, string.Format("Successfully posted {0} archived loans to MCT at {1}.", numLoansProcessed, mct.PostURL), EventLogEntryType.Information, 9201);
                        }
                    }
                }
                catch (Exception ex)
                {
                    int Event_id = 1017;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, "Scheduled_PostArchivedLoan \r\n" + ex.ToString(), EventLogEntryType.Error, Event_id, Category);
                    Trace.TraceError(ex.Message);
                }
                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                    DateTime.Now.Minute * 60 * 1000 +
                                    DateTime.Now.Second * 1000 +
                                    DateTime.Now.Millisecond;

                ProcessSleepingTime = interval * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;

                if (ProcessSleepingTime <= 2000)
                    ProcessSleepingTime = 2000;

                Thread.Sleep(ProcessSleepingTime);
            }
        }

        #endregion
        [STAThread]
        public static void Scheduled_UpdateUserLeadsDist()
        {
            short Category = 10;
            int ProcessSleepingTime = 0;
            int ProcessBeginningTime = 0;
            int ProcessEndingTime = 0;
            string err = string.Empty;
            DateTime stDt = DateTime.Now;
            DateTime edDt = stDt.AddDays(1);
            string endDate = edDt.ToString("d") + @" 12:00:00AM";
            edDt = Convert.ToDateTime(endDate);
            TimeSpan ts = edDt - stDt;
            int days = ts.Days;
            int hrs = ts.Hours;
            int min = ts.Minutes;
            int MonitorInterval = 24;                            // every 24 hours
            LeadMgr.LeadMgr pm = new LeadMgr.LeadMgr();
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
                    if (pm.UpdateUserLeadsDist(ref err) == false)
                    {
                        Trace.TraceError(err);
                    }
                }
                catch (Exception ex)
                {
                    int Event_id = 1098;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.Message + ex.StackTrace, EventLogEntryType.Error, Event_id, Category);
                    Trace.TraceError(ex.Message);
                }

                ProcessEndingTime = DateTime.Now.Hour * 60 * 60 * 1000 +
                                                       DateTime.Now.Minute * 60 * 1000 +
                                                       DateTime.Now.Second * 1000 +
                                                       DateTime.Now.Millisecond;

                ProcessSleepingTime = MonitorInterval * 60 * 60 * 1000 + ProcessBeginningTime - ProcessEndingTime;
            }
        }
    }
}