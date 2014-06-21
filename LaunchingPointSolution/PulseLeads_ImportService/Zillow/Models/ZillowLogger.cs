using System;
using System.Data;
using System.Data.SqlClient;
using PulseLeads.Zillow._Code;
using focusIT;

namespace PulseLeads.Zillow.Models
{
    public class ZillowLogger
    {
        public DateTime LogDate { get; private set; }
        public string Thread { get; private set; }
        public string Level { get; private set; }
        public string Logger { get; private set; }
        public string Exception { get; private set; }
        public string Message { get; private set; }

        private ZillowLogger(string logger)
        {
            LogDate = DateTime.Now;
            Thread = string.Empty;
            Level = string.Empty;
            Exception = string.Empty;
            Message = string.Empty;
            Logger = logger;
        }

        private void Save()
        {
            try
            {
                using (SqlConnection conn = DbHelperSQL.GetOpenConnection())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand(@"INSERT INTO ZillowLogger ([date_created],[thread],[level],[logger],[message],[exception]) VALUES (@a, @b, @c, @d, @e, @f)", conn);
                        DbHelperSQL.AddSqlParameter(cmd, "@a", SqlDbType.DateTime, LogDate);
                        DbHelperSQL.AddSqlParameter(cmd, "@b", SqlDbType.NVarChar, Helpers.FormatSqlStringValue(Thread, 255));
                        DbHelperSQL.AddSqlParameter(cmd, "@c", SqlDbType.NVarChar, Helpers.FormatSqlStringValue(Level, 50));
                        DbHelperSQL.AddSqlParameter(cmd, "@d", SqlDbType.NVarChar, Helpers.FormatSqlStringValue(Logger, 255));
                        DbHelperSQL.AddSqlParameter(cmd, "@e", SqlDbType.NVarChar, Helpers.FormatSqlStringValue(Message, 4000));
                        DbHelperSQL.AddSqlParameter(cmd, "@f", SqlDbType.NVarChar, Helpers.FormatSqlStringValue(Exception, 4000));
                        DbHelperSQL.ExecuteNonQuery(cmd);
                    }
                    catch (Exception ex)
                    {
                        // ignore
                    }
                }
            }
            catch (Exception ex)
            {
                //ignore
            }
        }

        public static ZillowLogger GetLogger(string logger)
        {
            return new ZillowLogger(logger);
        }

        public void Error(string message)
        {
            Level = "Error";
            Message = message;
            Save();
        }

        public void Error(string message, Exception ex)
        {
            Level = "Error";
            Message = message;

            Exception =
                    "Exception type " + ex.GetType() + Environment.NewLine +
                    "Exception message: " + ex.Message + Environment.NewLine +
                    "Stack trace: " + ex.StackTrace + Environment.NewLine;
            if (ex.InnerException != null)
            {
                Exception += "---BEGIN InnerException--- " + Environment.NewLine +
                           "Exception type " + ex.InnerException.GetType() + Environment.NewLine +
                           "Exception message: " + ex.InnerException.Message + Environment.NewLine +
                           "Stack trace: " + ex.InnerException.StackTrace + Environment.NewLine +
                           "---END Inner Exception";
            }

            Save();
        }

        public void Warn(string message)
        {
            Level = "Warn";
            Message = message;
            Exception = string.Empty;
            Save();
        }

        public void Debug(string message)
        {
            Level = "Debug";
            Message = message;
            Save();
        }

        public void Info(string message)
        {
            Level = "Info";
            Message = message;
            Save();
        }

    }
}