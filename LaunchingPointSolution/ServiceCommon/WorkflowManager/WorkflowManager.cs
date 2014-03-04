using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Common;
using System.Diagnostics;
using LP2.Service.Common;
using Utilities;
using System.Threading;
using System.Data;

namespace LP2Service
{
    public class WorkflowManager : IWorkflowManager
    {
        short Category = 70;
        static DataAccess.DataAccess m_da = null;
        static WorkflowManager m_Instance;
        SingleThreadedContext m_ThreadContext = null;
        static int m_refCount;
        private WorkflowManager()
        {
            if (m_da == null)
                m_da = new DataAccess.DataAccess();
            if (m_ThreadContext == null)
            {
                m_ThreadContext = new SingleThreadedContext();
                m_ThreadContext.ExceptionEvent += new ExceptionEventHandler(ThreadExceptionHandler);
                m_ThreadContext.Init("Workflow Manager");
            }

            if (m_Instance != null)
                return;
        }

        public static WorkflowManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new WorkflowManager();
                }
                lock (m_Instance)
                {
                    m_refCount++;
                }
                return m_Instance;
            }
        }

        public void Dispose()
        {
            lock (m_Instance)
            {
                m_refCount--;
            }
            if (m_refCount <= 0)
            {
                m_ThreadContext.Exit();
                m_da = null;
                m_Instance = null;
            }
        }

        public void ThreadExceptionHandler(object sender, ExceptionEventArgs args)
        {
            int Event_id = 7010;
            EventLog.WriteEntry(InfoHubEventLog.LogSource, args.Exception.Message, EventLogEntryType.Warning, Event_id, Category);             
        }

        #region monitor loan tasks/alerts
        private bool MonitorTasks(int fileId, ref string err)
        {
            List<Table.LoanTasks> taskList = null;
            List<Table.LoanTasks> allTaskList = null;
            try
            {
                taskList = m_da.GetLoanTasks(fileId, false, false, ref err);
                if ((taskList != null) && (taskList.Count > 0))
                {
                    foreach (Table.LoanTasks task in taskList)
                    {
                        if (task.PrerequisiteTaskId > 0)
                        {
                            if (m_da.UpdateTaskDueDate(fileId, task, ref err) == false)
                            {
                                Trace.TraceError(err);
                                int Event_id = 7011;
                                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                            }
                        }
                    }
                }

                allTaskList = m_da.GetLoanTasks(fileId, true, false, ref err);
                if ((allTaskList == null) || (allTaskList.Count <= 0))
                    return true;
                foreach (Table.LoanTasks task in allTaskList)
                {
                    if (m_da.Check_SaveTaskAlert(fileId, task.LoanTaskId, ref err) == false)
                    {
                        int Event_id = 7012;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                err = "MonitorTasks, Exception: " + ex.Message;
                int Event_id = 7014;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                return false;
            }
            finally
            {
                if (taskList != null)
                {
                    taskList.Clear();
                    taskList = null;
                }
                if (allTaskList != null)
                {
                    allTaskList.Clear();
                    allTaskList = null;
                }
            }
        }

        private bool MonitorRateLock(int fileId, ref string err)
        {
            bool status = m_da.Check_SaveRateLockAlert(fileId, ref err);
            if (status == false)
            {
                Trace.TraceError(err);
                int Event_id = 7015;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
            }
            return status;
        }

        public bool MonitorLoan(int fileId, bool activeLoan, ref string err)
        {
            err = "";
            if (fileId <= 0)
            {
                err = "MonitorLoan:: Invalid FileId = " + fileId;
                int Event_id = 7016;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                return false;
            }
            if (m_da == null)
                m_da = new DataAccess.DataAccess();

            m_da.CheckLoanStages(fileId, ref err);

            if (activeLoan)
            {
                MonitorRateLock(fileId, ref err);
                return MonitorTasks(fileId, ref err);
            }
            return m_da.DeleteLoanAlerts(fileId, ref err);
        }
        public bool MonitorProspectTasks()
        {
            bool logErr = false;
            string err = "";
            List<int> ProspectList = null;
            //int Event_id = 7001;
            //EventLog.WriteEntry(InfoHubEventLog.LogSource, "Starting MonitorProspectTasks now....", EventLogEntryType.Information, Event_id, Category);             
            try
            {
                if (m_da == null)
                    m_da = new DataAccess.DataAccess();
                ProspectList = m_da.GetActiveProspects(ref err);
                if (ProspectList == null || ProspectList.Count == 0)
                {
                    int Event_id = 7002;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, "No Active Prospects to monitor.", EventLogEntryType.Information, Event_id, Category);             
                }
                else
                {
                    foreach (int ProspectId in ProspectList)
                    {
                        if (m_da.MonitorProspectTasks(ProspectId, ref err) == false)
                        {
                            int Event_id = 7017;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                        }
                    }
                }
                if (ProspectList != null)
                    ProspectList.Clear();
                ProspectList = m_da.GetInactiveProspects(ref err);
                if (ProspectList == null || ProspectList.Count == 0)
                {
                    return true;
                }
                foreach (int ProspectId in ProspectList)
                {
                    if (m_da.RemoveProspectAlerts(ProspectId, ref err) == false)
                    {
                        int Event_id = 7018;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                    }
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                int Event_id = 7019;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                return false;
            }
            return true;
        }

        public bool MonitorProspectLoans()
        {
            bool logErr = false;
            string err = "";
            List<Table.LoanStatus> activeLoans = null;
            List<Table.LoanStatus> inactiveLoans = null;
            //int Event_id = 7001;
            //EventLog.WriteEntry(InfoHubEventLog.LogSource, "Starting MonitorProspectLoans now....", EventLogEntryType.Information, Event_id, Category);
            try
            {
                if (m_da == null)
                    m_da = new DataAccess.DataAccess();

                activeLoans = m_da.GetActiveProspectLoans(ref err);
                if (activeLoans == null)
                {
                    if (err != String.Empty)
                    {
                        int Event_id = 7020;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                        return false;
                    }
                }
                else
                {
                    foreach (Table.LoanStatus l in activeLoans)
                    {
                        if (MonitorLoan(l.FileId, true, ref err) == false)
                        {
                            int Event_id = 7021;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                            return false;
                        }
                    }
                }

                inactiveLoans = m_da.GetInactiveProspectLoans(ref err);
                if (inactiveLoans == null || inactiveLoans.Count <= 0)
                {
                    if (err != String.Empty)
                    {
                        int Event_id = 7022;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                        return false;
                    }
                    return true;
                }
                foreach (Table.LoanStatus il in inactiveLoans)
                {
                    if (MonitorLoan(il.FileId, false, ref err) == false)
                    {
                        int Event_id = 7023;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                err = "MonitorLoans, Exception:" + ex.Message;
                int Event_id = 7024;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                return false;
            }
            finally
            {
                if (activeLoans != null)
                    activeLoans.Clear();
                if (inactiveLoans != null)
                    inactiveLoans.Clear();
                activeLoans = null;
                inactiveLoans = null;
                if (logErr)
                {
                    int Event_id = 7025;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                }
                else
                {
                    //int Event_id = 7002;                   
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, "Completing MonitorLoans...", EventLogEntryType.Information, Event_id, Category);
                }

            }

        }

        public bool MonitorLoans()
        {
            bool logErr = false;
            string err = "";
            List<Table.LoanStatus> activeLoans = null;
            List<Table.LoanStatus> inactiveLoans = null;
            //int Event_id = 4001;
            //EventLog.WriteEntry(InfoHubEventLog.LogSource, "Starting MonitorLoans now....", EventLogEntryType.Information, Event_id, Category);
            try
            {
                if (m_da == null)
                    m_da = new DataAccess.DataAccess();
                // process active loans first
                activeLoans = m_da.GetActiveLoans(ref err);
                if (activeLoans == null)
                {
                    if (err != String.Empty)
                    {
                        int Event_id = 7026;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                        return false;
                    }
                }
                else
                {
                    foreach (Table.LoanStatus l in activeLoans)
                    {
                        if (MonitorLoan(l.FileId, true, ref err) == false)
                        {
                            int Event_id = 7027;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                            return false;
                        }
                    }
                }
                // delete all the obsolete loan alerts
                if (m_da.DeleteObsoleteLoanAlerts(ref err) == false)
                {
                    int Event_id = 7028;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                }
                // process all the inactive loans
                inactiveLoans = m_da.GetInactiveLoans(ref err);
                if (inactiveLoans == null)
                {
                    if (err != String.Empty)
                    {
                        int Event_id = 7029;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                        return false;
                    }
                    return true;
                }
                foreach (Table.LoanStatus il in inactiveLoans)
                {
                    if (MonitorLoan(il.FileId, false, ref err) == false)
                    {
                        int Event_id = 7030;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                err = "MonitorLoans, Exception:" + ex.Message;
                int Event_id = 7031;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                return false;
            }
            finally
            {
                if (activeLoans != null)
                    activeLoans.Clear();
                if (inactiveLoans != null)
                    inactiveLoans.Clear();
                activeLoans = null;
                inactiveLoans = null;
                if (logErr)
                {
                    int Event_id = 7032;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                }
                else
                {
                    //int Event_id = 7002;                   
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, "Completing MonitorLoans...", EventLogEntryType.Information, Event_id, Category);
                }

            }
        }
        #endregion

        private bool GenerateNewStagesTasks(int FileId, int WorkflowTemplId, ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            try
            {
                List<Table.WorkflowStage> WflStageList = null;
                WflStageList = m_da.GetWorkflowStagesByWorkflowTemplate(WorkflowTemplId, ref err);
                //CompareGenerateLoanStagesTasks(FileId, WorkflowTemplId, WflStageList, ref err);
                //return true;

                if ((WflStageList == null) || (WflStageList.Count <= 0))
                {
                    err = string.Format("GenerateNewStagesTasks, Workflow Stages not found for Workflow Tempalte Id={0}, FileId={1}.", WorkflowTemplId, FileId);
                    int Event_id = 7033;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                if (m_da.GenerateLoanStages(FileId, WorkflowTemplId, ref err) == false)
                {
                    err = string.Format("GenerateNewStagesTasks, FileId={0}, WorkflowTemplateId={1}, Error: {2}", FileId, WorkflowTemplId, err);
                    int Event_id = 7034;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                foreach (Table.WorkflowStage s in WflStageList)
                {
                    if (s.WorkflowStageId <= 0)
                        continue;
                    if (s.WorkflowTemplId <= 0)
                        continue;
                    if (m_da.GenerateLoanTaskByStage(FileId, WorkflowTemplId, s.WorkflowStageId, ref err) == false)
                    {
                        err = string.Format("GenerateNewStagesTasks, FileId={0}, WorkflowTemplateId={1}, Error: {2}", FileId, WorkflowTemplId, err);
                        int Event_id = 7035;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("GenerateNewStagesTasks, FileId={0}, WflTemplId={1}, Exception:{2}", FileId, WorkflowTemplId, ex.Message);
                int Event_id = 7036;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 7037;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }


        private void CompareGenerateLoanStagesTasks(int FileId, int WorkflowTemplId, List<Table.WorkflowStage> NewWorkflowStageList, ref string err)
        {
            List<Table.LoanStages> LoanStageList = null;
            err = string.Empty;
            // get the existing Loan Stages
            m_da.InitObsoleteLoanStagesTasks(FileId, WorkflowTemplId, ref err);
            LoanStageList = m_da.GetLoanStagesByFileId(FileId, ref err);
            Dictionary<string, int> loanStages = new Dictionary<string, int>();
            foreach (Table.LoanStages ls in LoanStageList)
            {
                try
                {
                    if (ls.SequenceNumber >= 0)
                        loanStages.Add(ls.StageName, ls.LoanStageId);
                }
                catch (Exception ex)
                {
                    m_da.DeleteLoanStage(FileId, ls.LoanStageId, ref err);
                }
            }
            foreach (Table.LoanStages ls in LoanStageList)
            {
                try
                {
                    foreach (KeyValuePair<string, int> kv in loanStages)
                    {
                        if (kv.Key.ToUpper() != ls.StageName.ToUpper())
                        {
                            continue;
                        }
                        if (kv.Value == ls.LoanStageId)
                        {
                            break;
                        }
                        m_da.DeleteLoanStage(FileId, ls.LoanStageId, ref err);
                        break;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            LoanStageList.Clear();
            LoanStageList = m_da.GetLoanStagesByFileId(FileId, ref err);
            Table.LoanStages loanStage = null;
            foreach (Table.WorkflowStage ws in NewWorkflowStageList)
            {
                bool completed = false;
                bool found = false;
                if (LoanStageList != null && LoanStageList.Count > 0)
                {
                    foreach (Table.LoanStages ls in LoanStageList)
                    {
                        loanStage = ls;
                        if (ls == null)
                            continue;
                        if (ls.WflStageId == ws.WorkflowStageId || ls.StageName.Trim().ToUpper() == ws.Name.Trim().ToUpper())
                        {
                            if (ws.Delete)
                                m_da.DeleteLoanStage(FileId, ls.LoanStageId, ref err);
                            else
                                if (ls.Completed != DateTime.MinValue)
                                    completed = true;
                            if (ls.TaskCount <= 0)
                                break;
                            found = true;
                            break;
                        }
                    }
                }

                if (ws.Delete)
                    continue;
                if (!found)
                {
                    m_da.GenerateLoanTaskByStage(FileId, WorkflowTemplId, ws.WorkflowStageId, ref err);
                    continue;
                }
                // update the LoanStages with the new WflStageid and WflTemplId
                bool stageCompleted = false;
                m_da.ReplaceLoanStageTasks(FileId, loanStage, WorkflowTemplId, ws.WorkflowStageId, out stageCompleted, ref err);
                if (completed)
                {
                    // need to update Point Stage date
                }
            }

            // after comparing clean up
            m_da.CleanupObsoleteLoanStagesTasks(FileId, WorkflowTemplId, ref err);
        }

        private List<Table.WorkflowStage> CompareWorkflowStages(int WorkflowTemplId, int OldWorkflowTemplId, ref string err)
        {
            List<Table.WorkflowStage> NewWorkflowStageList = null;
            List<Table.WorkflowStage> OldWorkflowStageList = null;
            err = string.Empty;
            // get the new and old Workflow Stages
            NewWorkflowStageList = m_da.GetWorkflowStagesByWorkflowTemplate(WorkflowTemplId, ref err);
            OldWorkflowStageList = m_da.GetWorkflowStagesByWorkflowTemplate(OldWorkflowTemplId, ref err);
            // if the old Workflow Stages not found, return the new ones
            if (OldWorkflowStageList == null || OldWorkflowStageList.Count <= 0)
                return NewWorkflowStageList;
            // if the new Workflow Stages not found, return the null
            if (NewWorkflowStageList == null || NewWorkflowStageList.Count <= 0)
                return NewWorkflowStageList;

            foreach (Table.WorkflowStage oldStage in OldWorkflowStageList)
            {
                bool found = false;
                foreach (Table.WorkflowStage newStage in NewWorkflowStageList)
                {
                    if (newStage == null)
                        continue;
                    if (newStage.WorkflowStageId == oldStage.WorkflowStageId ||
                        (newStage.TemplStageId > 0 && newStage.TemplStageId == oldStage.TemplStageId))
                    {
                        found = true;
                        break;
                    }
                    if (newStage.Name.Trim().ToUpper() == oldStage.Name.Trim().ToUpper())
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    oldStage.Delete = true;
                    NewWorkflowStageList.Add(oldStage);
                }
            }

            return NewWorkflowStageList;
        }

        private bool ReplaceExistingStagesTasks(int FileId, int WorkflowTemplId, int oldWorkflowTemplId, ref string err)
        {
            bool logErr = false;
            err = string.Empty;

            List<Table.WorkflowStage> NewWorkflowStageList = null;
            try
            {
                if (oldWorkflowTemplId != WorkflowTemplId)
                {
                    NewWorkflowStageList = CompareWorkflowStages(WorkflowTemplId, oldWorkflowTemplId, ref err);
                }
                else
                {
                    NewWorkflowStageList = m_da.GetWorkflowStagesByWorkflowTemplate(WorkflowTemplId, ref err);
                }

                if (NewWorkflowStageList == null || NewWorkflowStageList.Count <= 0)
                {
                    err = string.Format("ReplaceExistingStagesTasks, No Workflow Stages found within new Workflow TemplateId {0}, FileId={1}", WorkflowTemplId, oldWorkflowTemplId);
                    int Event_id = 7038;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                CompareGenerateLoanStagesTasks(FileId, WorkflowTemplId, NewWorkflowStageList, ref err);
                if (m_da.DeleteObsoleteLoanAlerts(ref err) == false)
                {
                    logErr = true;
                }
            }
            catch (Exception ex)
            {
                err = string.Format("ReplaceExistingStagesTasks, FileId={0}, WflTemplId={1}, Exception:{2}", FileId, WorkflowTemplId, ex.Message);
                int Event_id = 7039;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 7040;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
            return true;
        }

        private void GenWorkflow_Internal(GenerateWorkflowRequest req)
        {
            bool logErr = false;
            bool fatal = false;
            bool generateNewWorkflow = true;
            string err = "";

            try
            {
                if ((req == null) || (req.hdr == null) || (req.hdr.UserId == null))
                {
                    err = "GenerateWorkflowRequest or its request header is null";
                    int Event_id = 7041;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }
                if (req.hdr.UserId < 0)
                {
                    err = "User Id in the GenerateWorkflowRequest is negative, userid=" + req.hdr.UserId;
                    int Event_id = 7042;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }
                if (req.FileId <= 0)
                {
                    err = "FileId in GenerateWorkflowRequest is invalid, FileId=" + req.FileId;
                    int Event_id = 7043;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }
                if (req.WorkflowTemplId <= 0)
                {
                    err = "Workflow Template Id in GenerateWorkflowRequest is invalid, Workflow Tempalte Id=" + req.WorkflowTemplId;
                    int Event_id = 7044;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }
                int oldWorkflowTemplateId;
                generateNewWorkflow = m_da.NeedToRegenerateWorkflow(req.FileId, req.WorkflowTemplId, out oldWorkflowTemplateId);
                if (generateNewWorkflow)
                {
                    if (GenerateNewStagesTasks(req.FileId, req.WorkflowTemplId, ref err) == false)
                        return;
                }
                else
                {
                    if (ReplaceExistingStagesTasks(req.FileId, req.WorkflowTemplId, oldWorkflowTemplateId, ref err) == false)
                        return;
                }
                if (m_da.GenerateMultipleCompletionEmails(req.FileId, ref err) == false)
                {
                    logErr = true;
                    return;
                }
                if (req.hdr.UserId <= 0)
                    m_da.SaveLoanWflTempl(req.FileId, req.WorkflowTemplId, req.hdr.UserId, ref err);
            }
            catch (Exception ex)
            {
                err = string.Format("GenerateWorkflow, FileId={0}, WorkflowTemplId={1}, Exception:{2}", req.FileId, req.WorkflowTemplId, ex.Message);
                int Event_id = 7045;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                fatal = true;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 7046;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        private void CalcuateDueDates_Internal(CalculateDueDatesRequest req)
        {
            bool logErr = false;
            bool fatal = false;
            string err = "";
            try
            {
                if ((req == null) || (req.hdr == null) || (req.UserId == null))
                {
                    err = "CalculateDueDatesRequest or its request header is null";
                    int Event_id = 7047;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }

                if (req.UserId < 0)
                {
                    err = "User Id in the CalculateDueDatesRequest is negative, userid=" + req.UserId;
                    int Event_id = 7048;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }

                if (req.FileId <= 0)
                {
                    err = "FileId in CalculateDueDatesRequest is invalid, FileId=" + req.FileId;
                    int Event_id = 7049;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }

                if (m_da.Save_EstCloseDate(req.FileId, req.NewEstCloseDate, req.UserId, ref err) == false)
                {
                    int Event_id = 7050;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }

                if (m_da.CalculateDueDates(req.FileId, req.NewEstCloseDate, ref err) == false)
                {
                    int Event_id = 7051;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }
            }
            catch (Exception ex)
            {
                err = "CalculateDueDates, Exception:" + ex.Message;
                int Event_id = 7052;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                fatal = true;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 7053;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        private void UpdateEstCloseDate(UpdateEstCloseDateRequest req)
        {
            string err = "";
            bool logErr = false;
            if ((req == null) || (req.FileId <= 0) || (req.NewDate == null))
            {
                err = "WorkflowMgr::UpdateEstCloseDate, ";
                if (req == null)
                    err += "request is empty.";
                else if (req.FileId <= 0)
                    err += String.Format("Fileid {0} is invalid.", req.FileId);
                else if (req.NewDate == null)
                    err += "missing new Est Close Date.";
                else if (req.NewDate == DateTime.MinValue)
                    err += String.Format("invalid Est Close Date {0}.", req.NewDate.Date.ToString());
                int Event_id = 7054;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            try
            {
                if (m_da.UpdateEstCloseDate(req.FileId, req.hdr.UserId, req.NewDate, ref err) == false)
                {
                    int Event_id = 7055;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }
            }
            catch (Exception ex)
            {
                err = "WorkflowMgr::UpdateEstCloseDate, Exception:" + ex.Message;
                int Event_id = 7056;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 7057;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }

        }

        public void ProcessRequest(object o)
        {
            string err = "";
            bool logErr = false;
            bool fatal = false;
            try
            {
                if (o == null)
                {
                    err = "Process Request, Workflow Event is null.";
                    int Event_id = 7058;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
                WorkflowEvent e = o as WorkflowEvent;
                if (e == null)
                {
                    err = "Process Request, Workflow Event is null.";
                    int Event_id = 7059;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }
                if (e.RequestMsg == null)
                {
                    err = "Process Request, Workflow Request Message is null.";
                    int Event_id = 7060;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }
                switch (e.RequestType)
                {
                    case WorkflowCmd.GenerateWorkflow:
                        GenerateWorkflowRequest req = e.RequestMsg as GenerateWorkflowRequest;
                        GenWorkflow_Internal(req);
                        break;
                    case WorkflowCmd.MonitorLoans:
                        MonitorLoans();
                        break;
                    case WorkflowCmd.CalculateDueDates:
                        CalculateDueDatesRequest req1 = e.RequestMsg as CalculateDueDatesRequest;
                        CalcuateDueDates_Internal(req1);
                        break;
                    case WorkflowCmd.UpdateEstCloseDate:
                        UpdateEstCloseDateRequest req2 = e.RequestMsg as UpdateEstCloseDateRequest;
                        UpdateEstCloseDate(req2);
                        break;
                }
            }
            catch (Exception ex)
            {
                err = "ProcessRequest, Exception:" + ex.Message;
                int Event_id = 7061;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 7062;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
            return;
        }

        public bool GenerateWorkflow(GenerateWorkflowRequest req, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                WorkflowEvent e = new WorkflowEvent(WorkflowCmd.GenerateWorkflow, req);
                m_ThreadContext.Post(new SendOrPostCallback(ProcessRequest), e);

                err = "Your request is being processed. It'll take a moment to finish.";
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("GenerateWorkflow, FileId={0}, Workflow Templ Id={1}, Exception: {2}", req.FileId, req.WorkflowTemplId, ex.Message);
                int Event_id = 7063;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 7064;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }

        }

        public bool CalculateDueDates(CalculateDueDatesRequest req, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                WorkflowEvent e = new WorkflowEvent(WorkflowCmd.CalculateDueDates, req);
                m_ThreadContext.Post(new SendOrPostCallback(ProcessRequest), e);
                err = "Your request is being processed. It'll take a moment to finish.";
                return true;
            }
            catch (Exception ex)
            {
                err = "CalculateDueDates, Exception:" + ex.Message;
                int Event_id = 7065;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 7066;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }

        }
    }
}
