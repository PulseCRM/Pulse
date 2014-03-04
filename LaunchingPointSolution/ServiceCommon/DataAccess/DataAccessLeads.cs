using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using Common;
using focusIT;
using LP2.Service.Common;

namespace DataAccess
{
    public partial class DataAccess
    {
        short Category = 20;
        /// <summary>
        /// Gets the mail chimp API key.
        /// </summary>
        /// <returns></returns>
        public string GetMCT_ClientID()
        {
            string err = "";
            var integrationID = string.Empty;
            int Event_id = 8111;
            bool logErr = false;
            string sqlCmd =
                string.Format("SELECT TOP 1 ClientID FROM dbo.Company_MCT");
            try
            {
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                integrationID = obj == null ? string.Empty : Convert.ToString(obj);
                return integrationID;
            }
            catch (Exception ex)
            {
                err = "GetMCT_ClientID, Exception: " + ex.ToString();
                logErr = true;
                return integrationID;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }
        public string GetFileIdByLoanNumber(string loanNumber)
        {
            string err = "";
            var fileId = string.Empty;
            bool logErr = false;
            string sqlCmd =
                string.Format("SELECT FileId FROM loans WHERE LoanNumber='{0}'", loanNumber);
            try
            {
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                fileId = obj == null ? string.Empty : Convert.ToString(obj);
                return fileId;
            }
            catch (Exception ex)
            {
                err = "GetFileIdByLoanNumber, Exception: " + ex.Message;
                int Event_id = 8112;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return fileId;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 8112;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }
        #region obsolete SaveLoanProfit code
        //public void SaveLoanProfit(string fileId, string netSell, string SRP, string LLPA, string investor)
        //{
        //    string err = "";
        //    bool logErr = false;

        //    SqlParameter[] parameters = {
        //            new SqlParameter("@FileId", SqlDbType.Int),
        //            new SqlParameter("@NetSell", SqlDbType.Decimal),
        //            new SqlParameter("@SRP", SqlDbType.Decimal),
        //            new SqlParameter("@LLPA", SqlDbType.Decimal),
        //            new SqlParameter("@Investor", SqlDbType.NVarChar,500)
        //            };
        //    parameters[0].Value = fileId;
        //    parameters[1].Value = netSell;
        //    parameters[2].Value = SRP;
        //    parameters[3].Value = LLPA;
        //    parameters[4].Value = investor;
        //    string sqlCmd = "[lpsp_SaveLoanProfit]";
        //    try
        //    {
        //        DbHelperSQL.RunProcedure(sqlCmd, parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        err = "SaveLoanProfit, Exception: " + ex.Message;
        //        int Event_id = 8114;
        //        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
        //    }
        //    finally
        //    {
        //        if (logErr)
        //        {
        //            Trace.TraceError(err);
        //            int Event_id = 8114;
        //            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
        //        }
        //    }
        //}
        #endregion
        public bool SaveLoanProfit(Record.LoanProfit rec, out string err)
        {
            err = "";
            bool success = false;
            bool logErr = false;
            if (rec == null)
            {
                err = "SaveLoanProfit, the Record.LoanProfit is null.";
                return success;
            }
            try
            {
                SqlParameter[] parameters = {
					    new SqlParameter("@FileId", SqlDbType.Int),  // 0
 					    new SqlParameter("@NetSell", SqlDbType.Decimal),    // 1
					    new SqlParameter("@SRP", SqlDbType.Decimal),          // 2
					    new SqlParameter("@LLPA", SqlDbType.Decimal),   // 3
					    new SqlParameter("@Investor", SqlDbType.NVarChar,500),   // 4
					    new SqlParameter("@HedgeCost", SqlDbType.Decimal),   // 5
					    new SqlParameter("@MandatoryFinalPrice", SqlDbType.Decimal),   // 6
  					    new SqlParameter("@CommitmentNumber", SqlDbType.NVarChar, 255),    // 7
					    new SqlParameter("@CommitmentTerm", SqlDbType.SmallInt),           // 8         
					    new SqlParameter("@CommitmentDate", SqlDbType.Date),         // 9
                        new SqlParameter("@CommitmentExpDate", SqlDbType.Date),    // 10
                        new SqlParameter("@Error", SqlDbType.NVarChar,500)         // 11
					    };
                parameters[0].Value = rec.FileId;

                //NetSell
                decimal dNetSell = 0;
                parameters[1].Value = DBNull.Value;
                if (rec.NetSell != string.Empty && decimal.TryParse(rec.NetSell, out dNetSell))
                {
                      parameters[1].Value = dNetSell;
                }
                //SRP
                decimal dSRP = 0;
                 parameters[2].Value = DBNull.Value;
                if (rec.SRP != string.Empty && decimal.TryParse(rec.SRP, out dSRP))
                {
                      parameters[2].Value = dSRP;
                }
                //LLPA
                decimal dLLPA = 0;
                parameters[3].Value = DBNull.Value;
                if (rec.LLPA != string.Empty && decimal.TryParse(rec.LLPA, out dLLPA))
                {
                  parameters[3].Value = dLLPA;
                }
                //Investor
                parameters[4].Value = rec.Investor;
                // Hedge Cost
                decimal dHedgeCost = 0;
                parameters[5].Value = DBNull.Value;
                if (rec.HedgeCost != string.Empty && decimal.TryParse(rec.HedgeCost, out dHedgeCost))
                {
                        parameters[5].Value = dHedgeCost;
                }

                decimal dMandatoryFinalPrice = 0;
                parameters[6].Value = DBNull.Value;
                if (rec.MandatoryFinalPrice != string.Empty && decimal.TryParse(rec.MandatoryFinalPrice, out dMandatoryFinalPrice))
                {
                        parameters[6].Value = dMandatoryFinalPrice;
                }
                parameters[7].Value = rec.CommitmentNumber;

                int iCommitmentTerm = 0;
                parameters[8].Value = DBNull.Value;
                if (rec.CommitmentTerm != string.Empty && int.TryParse(rec.CommitmentTerm, out iCommitmentTerm))
                {
                    parameters[8].Value = iCommitmentTerm; 
                }

                DateTime  dtCommitmentDate = DateTime.MinValue;
                parameters[9].Value = DBNull.Value;
                if (rec.CommitmentDate != string.Empty && DateTime.TryParse(rec.CommitmentDate, out dtCommitmentDate))
                {
                        parameters[9].Value = dtCommitmentDate;
                }

                DateTime dtCommitmentExpDate = DateTime.MinValue;
                parameters[10].Value = DBNull.Value;
                if (rec.CommitmentExpDate != string.Empty && DateTime.TryParse(rec.CommitmentExpDate, out dtCommitmentExpDate))
                {
                       parameters[10].Value = dtCommitmentExpDate;
                }

                parameters[11].Value = rec.Error;

                string sqlCmd = "[lpsp_SaveLoanProfit]";

                DbHelperSQL.RunProcedure(sqlCmd, parameters);
                success = true;
            }
            catch (Exception ex)
            {
                err = string.Format("SaveLoanProfit, FileID: {0} \r\nException: {1} ", rec.FileId, ex.ToString());
                //logErr = true;
            }
            //finally
            //{

            //if (logErr)
            //{
            //    Trace.TraceError(err);
            //    int Event_id = 8114;
            //    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            //}
            //}
            return success;
        }
        public Record.LoanProfit GetLoanProfit(int fileId, ref string err)
        {
            err = string.Empty;
            Record.LoanProfit loanProfit = null;
            bool logErr = false;
            if (fileId <= 0)
            {
                err = string.Format("GetLoanProfit, FileId {0} is invalid.", fileId);
                return loanProfit;
            }

            string sqlCmd = string.Format("select top 1 * from dbo.LoanProfit where FileId={0}", fileId);
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                    return loanProfit;
                DataRow dr = ds.Tables[0].Rows[0];
                loanProfit = new Record.LoanProfit()
                {
                    FileId = dr["FileId"] == DBNull.Value || dr["FileId"] == null ? 0 : (int)dr["FileId"],
                    CompensationPlan = dr["CompensationPlan"] == DBNull.Value || dr["CompensationPlan"] == null ? string.Empty : dr["CompensationPlan"].ToString(),
                    NetSell = dr["NetSell"] == DBNull.Value || dr["NetSell"] == null ? string.Empty : dr["NetSell"].ToString(),
                    SRP = dr["SRP"] == DBNull.Value || dr["SRP"] == null ? string.Empty : dr["SRP"].ToString(),
                    LLPA = dr["LLPA"] == DBNull.Value || dr["LLPA"] == null ? string.Empty : dr["LLPA"].ToString(),
                    Investor = dr["Investor"] == DBNull.Value || dr["Investor"] == null ? string.Empty : dr["Investor"].ToString(),
                    LenderCredit = dr["LenderCredit"] == DBNull.Value || dr["LenderCredit"] == null ? string.Empty : dr["LenderCredit"].ToString(),

                    HedgeCost = dr["HedgeCost"] == DBNull.Value ? string.Empty : dr["HedgeCost"].ToString(),
                    MandatoryFinalPrice = dr["MandatoryFinalPrice"] == DBNull.Value ? string.Empty : dr["MandatoryFinalPrice"].ToString(),
                    CommitmentNumber = dr["CommitmentNumber"] == DBNull.Value ? string.Empty : dr["CommitmentNumber"].ToString(),
                    CommitmentTerm = dr["CommitmentTerm"] == DBNull.Value ? string.Empty : dr["CommitmentTerm"].ToString(),
                    CommitmentDate = dr["CommitmentDate"] == DBNull.Value ? string.Empty : dr["CommitmentDate"].ToString(),
                    CommitmentExpDate = dr["CommitmentExpDate"] == DBNull.Value ? string.Empty : dr["CommitmentExpDate"].ToString(),
                    Error = dr["Error"] == DBNull.Value ? string.Empty : (string)dr["Error"]
                };

                return loanProfit;
            }
            catch (Exception ex)
            {
                err = "GetLoanProfit, Exception: " + ex.Message;
                logErr = true;
                return loanProfit;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

    }
}
