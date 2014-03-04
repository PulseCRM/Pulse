using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LP2.Service.Common
{
    public class FileMonitorInfo
    {      
        FileSystemWatcher m_Watcher = null;
        string m_Path = string.Empty;
        FileSystemEventHandler m_CallBack = null;
        public string Path
        {
            get { return m_Path; }
        }
        public FileMonitorInfo(string path, string filter, FileSystemEventHandler CallBack)
        {
            m_Watcher = new FileSystemWatcher();
            m_Watcher.Path = path;
            m_Watcher.NotifyFilter = NotifyFilters.FileName;
            m_Watcher.Filter = filter;
            m_Watcher.Created += new FileSystemEventHandler(CallBack);
        }
        public void Dispose()
        {
            if (m_Watcher != null)
                m_Watcher.EnableRaisingEvents = false;
            m_Watcher.Dispose();
            m_Watcher = null;
            m_Path = string.Empty;
            m_CallBack = null;
        }
    }

    public class FileMonitor
    {
        #region properties and constructor/destructor
        short Category = 11;
        static FileMonitor m_Instance = null;
        static List<FileMonitorInfo> m_MonitorInfo = null;
        static int m_RefCount = 0;
        private FileMonitor()
        {
            string err = "";
            bool logErr = false;
            try
            {
                if (m_MonitorInfo == null)
                    m_MonitorInfo = new List<FileMonitorInfo>();
            }
            catch (Exception ex)
            {
                err = "FileMonitor, failed to initialize, Exception: " + ex.Message;
                logErr = true;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 7901;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                }
            }
        }

        public static FileMonitor Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new FileMonitor();
                m_RefCount++;
                return m_Instance;
            }
        }
        public void Dispose()
        {
            m_RefCount--;
            if (m_RefCount == 0)
            {
                m_Instance = null;
                if (m_MonitorInfo != null)
                {
                    foreach (FileMonitorInfo mi in m_MonitorInfo)
                    {
                        mi.Dispose();
                        m_MonitorInfo.Remove(mi);
                    }
                    m_MonitorInfo.Clear();
                }
                m_MonitorInfo = null;
            }
        }
        #endregion

        public bool AddFolder(string path, string filter, FileSystemEventHandler callBack)
        {
            if (m_MonitorInfo == null)
                m_MonitorInfo = new List<FileMonitorInfo>();
            FileMonitorInfo fmi = new FileMonitorInfo(path, filter, callBack);
            m_MonitorInfo.Add(fmi);
            return true;
        }

        public bool RemoveFolder(string path)
        {
            if (m_MonitorInfo == null)
                return true;

            if (string.IsNullOrEmpty(path))
                return true;

            int i;
            FileMonitorInfo fmi = null;
            for (i = 0; i < m_MonitorInfo.Count; i++)
            {
                fmi = m_MonitorInfo[i];
                if (fmi.Path.ToUpper().Trim() != path.ToUpper().Trim())
                    continue;
                fmi.Dispose();
                m_MonitorInfo.Remove(fmi);
                break;
            }
            return true;
        }
    }
}
