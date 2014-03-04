using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Utilities
{
    /// <summary>
    /// Log
    /// </summary>
    public class LPLog
    {
        private static LogWriter writer = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
        /// <summary>
        /// Logs the specified log types.
        /// </summary>
        /// <param name="logTypes">The log types.</param>
        /// <param name="message">The message.</param>
        private static void Log(LogType logTypes, string message)
        {
            LogEntry logEntry = new LogEntry();
            try
            {
                switch (logTypes)
                {
                    case LogType.Loginfo:
                        logEntry.Categories.Add("InfoLogCategory");
                        logEntry.Severity = TraceEventType.Information;
                        break;
                    case LogType.Logdebug:
                        logEntry.Categories.Add("DebugLogCategory");
                        logEntry.Severity = TraceEventType.Verbose;
                        break;
                    case LogType.Logwarn:
                        logEntry.Categories.Add("WarnLogCategory");
                        logEntry.Severity = TraceEventType.Warning;
                        break;
                    case LogType.Logerror:
                        logEntry.Categories.Add("ErrorLogCategory");
                        logEntry.Severity = TraceEventType.Error;
                        break;
                    case LogType.Logfatal:
                        logEntry.Categories.Add("FatalLogCategory");
                        logEntry.Severity = TraceEventType.Critical;
                        break;
                    default:
                        logEntry.Categories.Add("General");
                        logEntry.Severity = TraceEventType.Information;
                        break;
                }
                logEntry.Message = message;
                writer.Write(logEntry);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Logs the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        private static void Log(LogEntry entry)
        {
            writer.Write(entry);
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogMessage(string message)
        {
            Log(LogType.General, message);
        }
        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public static void LogMessage(LogEntry entry)
        {
            Log(entry);
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="logTypes">The log types.</param>
        /// <param name="message">The message.</param>
        public static void LogMessage(LogType logTypes, string message)
        {
            LogMessage(message);
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logType">Type of the log.</param>
        /// <param name="priority">The priority.</param>
        public static void LogMessage(string message, string logType, int priority)
        {
            LogMessage(message);
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="logType">Type of the log.</param>
        /// <param name="priority">The priority.</param>
        public static void LogMessage(Exception exception, string logType, int priority)
        {
            LogMessage((string) exception.Message);
        }
    }
}
