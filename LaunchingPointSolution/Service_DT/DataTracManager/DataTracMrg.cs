using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dtrac.interop;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Common;
using LP2.Service.Common;
using focusIT;
using DataAccess;
using LP2Service;

namespace DataTracManager
{
    public class DataTracMgr : IDataTracMrg
    {
        private DataAccess.DataAccess dataAccess = null;
        private TracTools tracTools = null;
        private string strServerPath = ConfigurationManager.AppSettings["DataTracServerPath"];
        private string strLoginName = ConfigurationManager.AppSettings["DataTracLoginName"];
        private string strLoginPwd = ConfigurationManager.AppSettings["DataTracLoginPwd"];

        public DataTracMgr()
        {
            if (null == dataAccess)
                dataAccess = new DataAccess.DataAccess();
            if (null == tracTools)
                tracTools = new TracTools();

            string strErrMsg = "";
            tracTools.PATHTODATATRAC = strServerPath;
            Login(strLoginName, strLoginPwd, ref strErrMsg);
        }

        /// <summary>
        /// login DataTrac
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public bool Login(string Username, string Password)
        {
            string strErrorMsg = "";
            return Login(Username, Password, ref strErrorMsg);
        }

        /// <summary>
        /// get loans with specific status
        /// </summary>
        /// <param name="LoanStatus"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public List<Common.Table.Loans> GetLoanInfoList(string LoanStatus, ref string err)
        {

            err = string.Empty;
            List<Common.Table.Loans> loanList = null;
            try
            {
                string strStatus = string.Format("{0}", LoanStatus).Trim();
                DataTable dtLoanNumInfo = dataAccess.GetLoanNumberInfo(strStatus);
                loanList = GetLoanInfoByNumber(dtLoanNumInfo, ref err);
            }
            catch (Exception ex)
            {
                err = "GetLoanInfoList, Exception: " + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
            }
            return loanList;
        }

        /// <summary>
        /// get loans which file in the specific array
        /// </summary>
        /// <param name="FileIds"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public List<Common.Table.Loans> GetLoanInfoList(int[] FileIds, ref string err)
        {
            DataTable dtLoanNumInfo = dataAccess.GetLoanNumberInfo(FileIds);
            return GetLoanInfoByNumber(dtLoanNumInfo, ref err);
        }

        /// <summary>
        /// get single loan with specific fileId
        /// </summary>
        /// <param name="FileId"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public Common.Table.Loans GetLoanInfo(int FileId, ref string err)
        {
            if (FileId <= 0)
                return null;

            string strLoanNumber = dataAccess.GetLoanNumber(FileId);

            //Common.Table.Loans loan = GetLoanInfoByNumber(FileId, strLoanNumber, ref err);
            //return loan;
            tracTools.PATHTODATATRAC = strServerPath;
            
            DataSet dsLoan = GetLoanInfoUseSql(string.Format("'{0}'", strLoanNumber), ref err);
            Common.Table.Loans loan = null;
            if (null != dsLoan && dsLoan.Tables.Count > 0 && dsLoan.Tables[0].Rows.Count > 0)
            {
                loan = GetLoanFromDataRow(dsLoan.Tables[0].Rows[0]);
            }
            if (null != loan)
            {
                loan.FileId = FileId;
                loan.LoanNumber = strLoanNumber;
            }
            return loan;
        }

        /// <summary>
        /// get stage date of loan
        /// </summary>
        /// <param name="FileId"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public List<StatusDate> GetDates(int FileId, ref string err)
        {
            if (FileId <= 0)
                return null;

            string strLoanNumber = dataAccess.GetLoanNumber(FileId);
            object objRt = tracTools.LoadLoanData(strLoanNumber, "loan_num", "");
            if ("1" != string.Format("{0}", objRt))
            {
                err = string.Format("{0}", tracTools.ErrorMsg);
                return null;
            }

            List<StatusDate> listStatusDate = new List<StatusDate>();
            StatusDate statusDate = new StatusDate();

            string[] arrFields = new string[] { "doc.note_date", "und.resub", "und.approved", "und.Clear_to_close", "fun.funded", "und.Suspended", "und.denied", "und.Cancelled" };
            foreach (string str in arrFields)
            {
                objRt = tracTools.GetFieldValue(str);
                statusDate = null;
                statusDate = new StatusDate();
                DateTime dtTemp = DateTime.MinValue;
                if (System.DBNull.Value == objRt)
                {
                    //  err = string.Format("{0}", tracTools.ErrorMsg);
                    //  return null;
                    statusDate.CompletionDate = null;
                }
                else
                {
                    try
                    {
                        dtTemp = Convert.ToDateTime(objRt);
                    }
                    catch
                    { }

                    statusDate.CompletionDate = dtTemp;
                }

                switch (str.ToLower())
                {                    
                    case "doc.note_date":
                        statusDate.StatusName = "Note";
                        break;
                    case "und.resub":
                        statusDate.StatusName = PointStage.Re_submit;
                        break;
                    case "und.approved":
                        statusDate.StatusName = PointStage.Approve;
                        break;
                    case "und.clear_to_close":
                        statusDate.StatusName = PointStage.CleartoClose;
                        break;
                    case "fun.funded":
                        statusDate.StatusName = PointStage.Fund;
                        break;
                    case "und.suspended":
                        statusDate.StatusName = LoanStatus.LoanStatus_Suspended;
                        break;
                    case "und.denied":
                        statusDate.StatusName = LoanStatus.LoanStatus_Denied;
                        break;
                    case "und.cancelled":
                        statusDate.StatusName = LoanStatus.LoanStatus_Canceled;
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(statusDate.StatusName))
                    listStatusDate.Add(statusDate);
            }
            statusDate = null;
            return listStatusDate;
        }

        /// <summary>
        /// get rate lock info of the loan
        /// </summary>
        /// <param name="FileId"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public RateLock GetRateLock(int FileId, ref string err)
        {
            if (FileId <= 0)
                return null;

            string strLoanNumber = dataAccess.GetLoanNumber(FileId);
            object objRt = tracTools.LoadLoanData(strLoanNumber, "loan_num", "");
            if ("1" != string.Format("{0}", objRt))
            {
                err = string.Format("{0}", tracTools.ErrorMsg);
                return null;
            }

            RateLock rateLock = new RateLock();

            // get RateLock.LockDate
            DateTime dtTemp = DateTime.MinValue;
            objRt = tracTools.GetFieldValue("gen.lock_date");
            if (System.DBNull.Value != objRt)
            {
                try
                {
                    dtTemp = Convert.ToDateTime(objRt);
                    rateLock.LockDate = dtTemp;
                }
                catch
                { }
            }
            else
            {
                err = string.Format("{0}", tracTools.ErrorMsg);
                return null;
            }

            // get RateLock.RelockDate
            objRt = tracTools.GetFieldValue("relocks.relock_dt");
            if (System.DBNull.Value != objRt)
            {
                try
                {
                    dtTemp = Convert.ToDateTime(objRt);
                    rateLock.RelockDate = dtTemp;
                }
                catch
                { }
            }
            else
            {
                err = string.Format("{0}", tracTools.ErrorMsg);
                return null;
            }

            // get RateLock.LockTerm
            objRt = tracTools.GetFieldValue("relocks.lock_trm");
            if (System.DBNull.Value == objRt)
            {
                err = string.Format("{0}", tracTools.ErrorMsg);
                return null;
            }
            int nTemp = 0;
            try
            {
                nTemp = Convert.ToInt32(objRt);
                rateLock.LockTerm = nTemp;
            }
            catch
            { }

            if (0 == nTemp)
            {
                objRt = tracTools.GetFieldValue("gen.lock_term");
                if (System.DBNull.Value == objRt)
                {
                    err = string.Format("{0}", tracTools.ErrorMsg);
                    return null;
                }

                try
                {
                    nTemp = Convert.ToInt32(objRt);
                    rateLock.LockTerm = nTemp;
                }
                catch
                { }
            }

            // get RateLock.Expiration
            objRt = tracTools.GetFieldValue("relocks.lock_exp");
            if (System.DBNull.Value == objRt)
            {
                err = string.Format("{0}", tracTools.ErrorMsg);
                return null;
            }
            try
            {
                dtTemp = Convert.ToDateTime(objRt);
                rateLock.Expiration = dtTemp;
            }
            catch
            { }
            if (DateTime.MinValue == dtTemp)
            {
                objRt = tracTools.GetFieldValue("gen.lock_expire");
                if (System.DBNull.Value == objRt)
                {
                    err = string.Format("{0}", tracTools.ErrorMsg);
                    return null;
                }
                try
                {
                    dtTemp = Convert.ToDateTime(objRt);
                    rateLock.Expiration = dtTemp;
                }
                catch
                { }
            }

            // get RateLock.LockBy
            objRt = tracTools.GetFieldValue("relocks.lock_by");
            if (System.DBNull.Value != objRt)
            {
                rateLock.LockBy = string.Format("{0}", objRt);
            }
            else
            {
                err = string.Format("{0}", tracTools.ErrorMsg);
                return null;
            }

            return rateLock;
        }

        /// <summary>
        /// get borrower info from FileId
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public Common.Table.Contacts GetBorrowerInfo(int iFileId, ref string err)
        {
            string strLoanNum = dataAccess.GetLoanNumber(iFileId);
            if (string.IsNullOrEmpty(strLoanNum))
                return null;

            string strSql = string.Format(@"SELECT gen.file_id, gen.loan_num, coborrow.cborrow_fn, coborrow.cborrow_ln, coborrow.cborrow_bday, coborrow.cborrow_ssn, 
(SELECT TOP 1 score FROM cscore a INNER JOIN bureaus b ON a.bureaus_id=b.bureaus_id WHERE cborr_id=coborrow.cborr_id AND b.name='TransUnion' ORDER BY a.credit_date DESC) AS TransUnion, 
(SELECT TOP 1 score FROM cscore a INNER JOIN bureaus b ON a.bureaus_id=b.bureaus_id WHERE cborr_id=coborrow.cborr_id AND b.name='Experian' ORDER BY a.credit_date DESC) AS Experian, 
(SELECT TOP 1 score FROM cscore a INNER JOIN bureaus b ON a.bureaus_id=b.bureaus_id WHERE cborr_id=coborrow.cborr_id AND b.name='Equifax' ORDER BY a.credit_date DESC) AS Equifax FROM gen 
LEFT JOIN coborrow ON gen.file_id=coborrow.file_id AND gen.borrow_fn=coborrow.cborrow_fn AND gen.borrow_ln=coborrow.cborrow_ln
WHERE gen.loan_num='{0}'", strLoanNum);
            object objQueryReturn = tracTools.ExecuteSQL(strSql, "recordset", null);
            if (System.DBNull.Value == objQueryReturn)
            {
                err = string.Format("{0}", tracTools.ErrorMsg);
                return null;
            }

            OleDbDataAdapter adapter = new OleDbDataAdapter();
            DataSet dsBorrower = new DataSet();
            adapter.Fill(dsBorrower, objQueryReturn, "Borrower");

            Common.Table.Contacts borrower = null;
            if (null != dsBorrower && dsBorrower.Tables.Count > 0 && dsBorrower.Tables[0].Rows.Count > 0)
            {
                borrower = GetContactFromDataRow(dsBorrower.Tables[0].Rows[0]);
            }
            return borrower;
        }

        /// <summary>
        /// get co-borrower info from FileId
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public Common.Table.Contacts GetCoBorrowerInfo(int iFileId, ref string err)
        {
            string strLoanNum = dataAccess.GetLoanNumber(iFileId);
            if (string.IsNullOrEmpty(strLoanNum))
                return null;

            string strSql = string.Format(@"SELECT TOP 1 gen.file_id, gen.loan_num, coborrow.cborr_id, coborrow.cborrow_fn, coborrow.cborrow_ln, coborrow.cborrow_bday, coborrow.cborrow_ssn, 
(SELECT TOP 1 score FROM cscore a INNER JOIN bureaus b ON a.bureaus_id=b.bureaus_id WHERE cborr_id=coborrow.cborr_id AND b.name='TransUnion' ORDER BY a.credit_date DESC) AS TransUnion, 
(SELECT TOP 1 score FROM cscore a INNER JOIN bureaus b ON a.bureaus_id=b.bureaus_id WHERE cborr_id=coborrow.cborr_id AND b.name='Experian' ORDER BY a.credit_date DESC) AS Experian, 
(SELECT TOP 1 score FROM cscore a INNER JOIN bureaus b ON a.bureaus_id=b.bureaus_id WHERE cborr_id=coborrow.cborr_id AND b.name='Equifax' ORDER BY a.credit_date DESC) AS Equifax 
FROM gen LEFT JOIN coborrow ON gen.file_id=coborrow.file_id AND coborrow.cborr_id<>(SELECT cb.cborr_id FROM coborrow cb WHERE cb.cborrow_fn=gen.borrow_fn AND cb.cborrow_ln=gen.borrow_ln AND cb.file_id=gen.file_id)
WHERE gen.loan_num='{0}'", strLoanNum);
            object objQueryReturn = tracTools.ExecuteSQL(strSql, "recordset", null);
            if (System.DBNull.Value == objQueryReturn)
            {
                err = string.Format("{0}", tracTools.ErrorMsg);
                return null;
            }

            OleDbDataAdapter adapter = new OleDbDataAdapter();
            DataSet dsBorrower = new DataSet();
            adapter.Fill(dsBorrower, objQueryReturn, "CoBorrower");

            Common.Table.Contacts borrower = null;
            if (null != dsBorrower && dsBorrower.Tables.Count > 0 && dsBorrower.Tables[0].Rows.Count > 0)
            {
                borrower = GetContactFromDataRow(dsBorrower.Tables[0].Rows[0]);
            }
            return borrower;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="err"></param>
        /// <returns></returns>
        public List<DT_LoanProgram> GetLoanPrograms(ref string err)
        {
            List<DT_LoanProgram> listPrograms = new List<DT_LoanProgram>();
            try
            {
                string strSql = "SELECT programs_id, prog_name FROM programs";
                object objQueryReturn = tracTools.ExecuteSQL(strSql, "recordset", null);
                if (System.DBNull.Value == objQueryReturn)
                {
                    err = string.Format("{0}", tracTools.ErrorMsg);
                    return null;
                }

                OleDbDataAdapter adapter = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                adapter.Fill(ds, objQueryReturn, "programs");
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                { 
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, "No Loan Program found in DataTrac.", EventLogEntryType.Warning);
                    return listPrograms;
                }

                DT_LoanProgram prog = null;
                foreach (DataRow dataRow in ds.Tables[0].Rows)
                {
                    prog = new DT_LoanProgram();
                    object obj = null;
                    obj = dataRow["programs_id"];
                    prog.programs_id = string.Format("{0}", obj).Trim();
                    obj = dataRow["prog_name"];
                    prog.prog_name = string.Format("{0}", obj).Trim();
                    listPrograms.Add(prog);
                }
   
            }
            catch (Exception ex)
            {
                err = "GetLoanPrograms, exception: " + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
            }
            return listPrograms;
        }

        public bool Schedule_ImportLoanPrograms(ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            try 
            {
                DataAccess.PointConfig pointConfig;
                pointConfig = dataAccess.GetPointConfigData(ref err);
                if ((pointConfig == null) ||
                    (pointConfig.MasterSource.Trim().ToLower() != "datatrac"))
                {
                    return true;
                }

                List<DT_LoanProgram> programList = GetLoanPrograms(ref err);
                if (programList == null || programList.Count <= 0)
                {
                    err = string.Format("Schedule_ImportLoanPrograms, DataTrac did not return any loan program, err: {0}", err);
                    logErr = true;
                    return false;
                }
                string sqlCmd = string.Empty;
                foreach (DT_LoanProgram prog in programList)
                {
                    if (prog == null || string.IsNullOrEmpty(prog.prog_name))
                        continue;
                    sqlCmd = string.Format("Select DTProgramID from [Company_Loan_Programs] where DTProgramID='{0}' OR LoanProgram='{1}'",
                        prog.programs_id, prog.prog_name);
                    object obj = DbHelperSQL.GetSingle(sqlCmd);
                    if (obj == null || obj == DBNull.Value)
                    {
                        sqlCmd = string.Format("INSERT INTO [Company_Loan_Programs] (DTProgramID,LoanProgram) VALUES ('{0}', '{1}')", prog.programs_id.Trim(), prog.prog_name.Trim());
                        DbHelperSQL.ExecuteSql(sqlCmd);
                        continue;
                    }
                    sqlCmd = string.Format("Update [Company_Loan_Programs] set LoanProgram='{0}' where DTProgramID='{1}'", prog.prog_name, prog.programs_id);
                    DbHelperSQL.ExecuteSql(sqlCmd);
                }
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("Schedule_ImportLoanPrograms,  Exception:{1} ", ex.Message);
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        private bool UpdatePulseStatusDates(int iTargetFileID, List<StatusDate> StatusDateList, ref string err)
        {
            bool logErr = false;
            err = string.Empty;
            try
            {
                dataAccess.UpdateDataTracStatusDate_All(iTargetFileID, StatusDateList);
                
                Record.Loans loans_record = null;
                loans_record = dataAccess.GetLoanInfo(iTargetFileID, ref err);

                List<Table.LoanStages> stageList = dataAccess.GetLoanStagesByFileId(iTargetFileID, ref err);

                if (stageList == null || stageList.Count <= 0)
                    return false;
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                UpdateStageRequest req = new UpdateStageRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 0;
                req.hdr.SecurityToken = "Security Token";
                req.StageList = new List<StageInfo>();
                req.FileId = iTargetFileID;
                foreach (Table.LoanStages s in stageList)
                {
                    if (s == null || string.IsNullOrEmpty(s.StageName))
                        continue;
                    StageInfo si = new StageInfo();
                    si.StageName = s.StageName;
                    si.Completed = s.Completed;
                    req.StageList.Add(si);
                }
                if (pm.UpdateStage(req, ref err) == false)
                {
                    logErr = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("DataTracMgr:UpdatePulseStatusDates,  Exception:{0}, FileId={1} ", ex.Message, iTargetFileID);
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        public bool Schedule_ImportStatusDates(ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            try
            {
                DataAccess.PointConfig pointConfig;
                pointConfig = dataAccess.GetPointConfigData(ref err);
                if ((pointConfig == null) ||
                    (pointConfig.MasterSource.Trim().ToLower() != "datatrac"))
                {
                    return true;
                }

                List<Common.Table.Loans> loanList = GetLoanInfoList("Processing", ref err);
                if (loanList == null || loanList.Count <= 0)
                {
                    return true;
                }
                string sqlCmd = string.Empty;
                List<int> TargetFileIDs = new List<int>();
                foreach (Common.Table.Loans loanItem in loanList)
                {
                    if (loanItem == null || loanItem.FileId <= 0)
                        continue;
                    TargetFileIDs.Add(loanItem.FileId);
                }
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;

 
                foreach (int iTargetFileID in TargetFileIDs)
                {
                    err = string.Empty;
                    List<StatusDate> StatusDateList = null;

                    try
                    {
                        StatusDateList = GetDates(iTargetFileID, ref err);
                        
                        if (err != string.Empty)
                        {
                            string sErrorMsg = string.Format("Schedule_ImportStatusDates Failed to get status dates from DataTrac:{0}, FileId: {1} ", err, iTargetFileID);
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, sErrorMsg, EventLogEntryType.Warning);
                            continue;
                        }
                        if (StatusDateList == null || StatusDateList.Count <= 0)
                            continue;
                        UpdateStageRequest req = new UpdateStageRequest();
                        req.hdr = new ReqHdr();
                        req.hdr.UserId = 0;
                        req.hdr.SecurityToken = "Security Token";
                        req.FileId = iTargetFileID;
                        req.StageList = new List<StageInfo>();
                        foreach (StatusDate sd in StatusDateList)
                        {
                            if (sd == null || string.IsNullOrEmpty(sd.StatusName))
                                continue;
                            StageInfo si = new StageInfo();
                            si.StageName = sd.StatusName;
                            si.Completed = (DateTime)sd.CompletionDate;
                            req.StageList.Add(si);
                        }
                        if (req.StageList.Count <= 0)
                            continue;
                        if (pm.UpdateStage(req, ref err) == false)
                        {
                            err = string.Format("DataTracMgr:Schedule_ImportStatusDates,  FileId={0}, Error:{1} ", iTargetFileID, err);
                        }
                    }
                    catch (Exception ex1)
                    {
                        err = string.Format("DataTracMgr:Schedule_ImportStatusDates,  FileId={0}, Exception:{1} ", iTargetFileID, ex1.Message+ex1.StackTrace);
                        logErr = true;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error);
                    }
 
                }
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("DataTracMgr:Schedule_ImportStatusDates,  Exception:{0} ", ex.Message);
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }

        }
        public bool Mapping_Borrower(string strLoanNumber, int FileId, string orig_type, string loan_program, ref string err)
        {
            string strErr = "Mapping_Borrower ";
            bool logErr = false;
            int ContactId = 0;
            int ContactRoleId = 1;

            try
            {
            string sqlCmd1 = "";
            sqlCmd1 = "select Top 1 ContactId from LoanContacts where FileId=" + FileId + " AND ContactRoleId=" + ContactRoleId;
            object obj = DbHelperSQL.GetSingle(sqlCmd1);
            ContactId = obj == null ? 0 : (int)obj;
            if (ContactId <= 0)
                {
                    return true;
                }

            Table.Contacts Borrower = new Table.Contacts();            

            Borrower = dataAccess.GetContactDetail(ContactId, ref err);
           
            if (Borrower == null)
            {
                err = strErr + string.Format("Failed to read Contacts table, ContactId={0}, Err: {1}", ContactId, err);
                logErr = true;
                return false;               
            }
              
            object objRt = tracTools.LoadLoanData(strLoanNumber, "loan_num", "");
            if (string.Format("{0}", objRt) != "1")
            {
                err = strErr+string.Format("{0}", tracTools.ErrorMsg);
                logErr = true;
                return false;
            }

            objRt = tracTools.SetFieldValue("gen.borrow_mn", Borrower.MiddleName);
            if (objRt == DBNull.Value || objRt == null)
            {
                err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                logErr = true;
                return false;
            }

            objRt = tracTools.SetFieldValue("gen.mail_street", Borrower.MailingAddr);
            if (objRt == DBNull.Value || objRt == null)
            {
                err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                logErr = true;
                return false;
            }

            objRt = tracTools.SetFieldValue("gen.mail_city", Borrower.MailingCity);
            if (objRt == DBNull.Value || objRt == null)
            {
                err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                logErr = true;
                return false;
            }

            objRt = tracTools.SetFieldValue("gen.mail_state", Borrower.MailingState);
            if (objRt == DBNull.Value || objRt == null)
            {
                err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                logErr = true;
                return false;
            }

            objRt = tracTools.SetFieldValue("gen.mail_zip", Borrower.MailingZip);
            if (objRt == DBNull.Value || objRt == null)
            {
                err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                logErr = true;
                return false;
            }

            objRt = tracTools.SetFieldValue("gen.borrow_fn", Borrower.FirstName);
            if (objRt == DBNull.Value || objRt == null)
            {
                err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                logErr = true;
                return false;
            }

            objRt = tracTools.SetFieldValue("gen.borrow_ln", Borrower.LastName);
            if (objRt == DBNull.Value || objRt == null)
            {
                err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                logErr = true;
                return false;
            }
  
            objRt = tracTools.SetFieldValue("gen.surname", Borrower.GenerationCode);
            if (objRt == DBNull.Value || objRt == null)
            {
                err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                logErr = true;
                return false;
            }

            objRt = tracTools.SetFieldValue("coborrow.cborrow_email", 
                    Borrower.Email);
            if (objRt == DBNull.Value || objRt == null)
            {
                err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                logErr = true;
                return false;
            }
          
            objRt = tracTools.Save();
            if ("1" != string.Format("{0}", objRt))
            {
                err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                logErr = true;
                return false;
            }

            return true;

            }
            catch (Exception ex)
            {
                err = "DataTracMgr.Mapping_Borrower, Exception:" + ex.Message + "\n" + ex.StackTrace;
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
            }
        }

        public bool Mapping_DataTrac_Fields(string strLoanNumber, int FileId, string orig_type, string loan_program, ref string err)
        {
            string strErr = "Mapping_DataTrac_Fields ";
            bool logErr = false;

            try
            {
                string strOrigType = "";

                if (orig_type.ToLower() == "branch")
                    strOrigType = "R";
                else
                    if (orig_type.ToLower() == "lender(b)")
                        strOrigType = "K";

                object objRt = tracTools.LoadLoanData(strLoanNumber, "loan_num", "");
                if (string.Format("{0}", objRt) != "1")
                {
                    err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                    logErr = true;
                    return false;
                }
               
                objRt = tracTools.SetFieldValue("Gen.Branch_Type", strOrigType);
                if (objRt == DBNull.Value || objRt == null)
                {
                    err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                    logErr = true;
                    return false;
                }

                string strLoanProgId = "";
                string strSqlQueryProgram = string.Format("SELECT TOP 1 programs_id FROM programs WHERE prog_name='{0}'", loan_program);
                DataSet dsProg = GetDataFromDataTracDB(strSqlQueryProgram);
                if ((dsProg != null) && (dsProg.Tables.Count > 0) && (dsProg.Tables[0].Rows.Count > 0))
                {
                    strLoanProgId = string.Format("{0}", dsProg.Tables[0].Rows[0][0]);
                }

                if (!string.IsNullOrEmpty(strLoanProgId))
                {
                    objRt = tracTools.SetFieldValue("Gen.programs_id", strLoanProgId);
                    if (DBNull.Value == objRt || null == objRt)
                    {
                        err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                        logErr = true;
                        return false;
                    }
                }

                objRt = tracTools.Save();
                if ("1" != string.Format("{0}", objRt))
                {
                    err = strErr + string.Format("{0}", tracTools.ErrorMsg);
                    logErr = true;
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                err = "DataTracMgr.Mapping_DataTrac_Fields, Exception:" + ex.Message + "\n" + ex.StackTrace;
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
            }
        }

        public bool Pulse_DataTrac_Mapping(string strLoanNumber, int FileId, string orig_type, string loan_program, ref string err)
        {
            bool logErr = false;
            bool st = true;

            try
            {
                st = Mapping_DataTrac_Fields(strLoanNumber, FileId, orig_type, loan_program, ref err);
                if (st == false)
                {
                    logErr = true;
                    return false;
                }

                st = Mapping_Borrower(strLoanNumber, FileId, orig_type, loan_program, ref err);
                if (st == false)
                {
                    logErr = true;
                    return false;
                }
         
                return true;

            }
            catch (Exception ex)
            {
                err = "DataTracMgr.SubmitLoan, Exception:" + ex.Message + "\n" + ex.StackTrace;
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
            }
        }
        public bool Scheduled_ImportLoans(ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            int currentFileId = 0;
            List<Common.Table.Loans> LoanList = null;
            try
            {
                DataAccess.PointConfig pointConfig;
                pointConfig = dataAccess.GetPointConfigData(ref err);
                if ((pointConfig == null) ||
                    (pointConfig.MasterSource.Trim().ToLower() != "datatrac"))
                {
                    return false;
                }
 
                LoanList = GetLoanInfoList("Processing", ref err);
                if (LoanList == null || LoanList.Count <= 0)
                {
                    err = "Scheduled_ImportLoans, no loans to be imported from DataTrac.";
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, 7001);
                    return true;
                }
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                foreach (Common.Table.Loans LoanItem in LoanList)
                {
                    string sError_GetBorrowerInfo = string.Empty;
                    if (LoanItem == null || LoanItem.FileId <= 0)
                        continue;
                    
                    currentFileId = LoanItem.FileId;
                    
                    Common.Table.Contacts BorrowerInfo = GetBorrowerInfo(currentFileId, ref sError_GetBorrowerInfo);
                    if (sError_GetBorrowerInfo != string.Empty)
                    {
                        err = string.Format("Scheduled_ImportLoans, Failed to get Borrower info from DataTrac, FileId={0}, error:{1} ", currentFileId, sError_GetBorrowerInfo);
                        Trace.TraceError(err);
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                        continue;
                    }

                    string sError_GetCoBorrowerInfo = string.Empty;
                    Common.Table.Contacts CoBorrowerInfo = GetCoBorrowerInfo(LoanItem.FileId, ref sError_GetCoBorrowerInfo);
                    if (sError_GetCoBorrowerInfo != string.Empty)
                    {
                        err = string.Format("Scheduled_ImportLoans, Failed to get Borrower info from DataTrac, FileId={0}, error:{1} ", currentFileId, sError_GetCoBorrowerInfo);
                        Trace.TraceError(err);
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, 7005);
                        //continue;
                    }

                    dataAccess.UpdateLoanInfo(LoanItem, BorrowerInfo, CoBorrowerInfo);

                    #region Call UpdateLoanInfo API after update loan field

                    UpdateLoanInfoRequest loanInfoReq = new UpdateLoanInfoRequest();
                    string strErr = "";
                    loanInfoReq.FileId = LoanItem.FileId;
                    loanInfoReq.hdr = new ReqHdr();
                    loanInfoReq.hdr.UserId = 0;
                    pm.UpdateLoanInfo(loanInfoReq, ref strErr);
                }
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("Scheduled_ImportLoans,  FileId={0}, Exception:{1} ", currentFileId, ex.Message);
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        public bool SubmitLoan(int FileId, string orig_type, string loan_program, ref string err)
        {
            string strErr = "";
            string err1 = string.Empty;
            int Event_id = 7001;
            bool logErr = false;
            bool st = true;
            string strFilePaths = string.Empty;
           
            try
            {
                DataAccess.PointConfig pointConfig;
                pointConfig = dataAccess.GetPointConfigData(ref err);
                if ((pointConfig == null) ||
                    (pointConfig.MasterSource.Trim().ToLower() != "datatrac"))
                {
                    Event_id = 7011;
                    err = "DataTrac SubmitLoan Error: pointConfig.MasterSource != DataTrac";
                    logErr = true;
                    return false;
                }

                strFilePaths = dataAccess.GetPointFilePath(FileId, ref strErr);
                if (!string.IsNullOrEmpty(strErr))
                {
                    err = "DataTrac SubmitLoan Error: " + strErr;
                    Event_id = 7012;
                    logErr = true;
                    return false;
                }

                if (string.IsNullOrEmpty(strFilePaths))
                {
                    err = "DataTrac SubmitLoan Error: No Point file path found in the database for file Id: " + FileId;
                    Event_id = 7013;
                    logErr = true; 
                    return false;
                }

                if (!File.Exists(strFilePaths))
                {
                    err = string.Format("DataTrac SubmitLoan Error: Point file, {0} does not exist, fileId={1}.", strFilePaths, FileId);
                    Event_id = 7014;
                    logErr = true;
                    return false;
                }

                string strOrigType = "";
                if ("branch" == orig_type.ToLower())
                    strOrigType = "R";
                else if ("lender(b)" == orig_type.ToLower())
                    strOrigType = "K";
                
                // add loan 
                string strLoanNumber = dataAccess.GetLoanNumber(FileId);
                
                tracTools.PATHTODATATRAC = strServerPath;
                object objLogin;
                //objLogin = tracTools.Login(strLoginName, strLoginPwd);
                objLogin = tracTools.ActorLogin(strLoginName, strLoginPwd);
                if ("0" == string.Format("{0}", objLogin))
                {
                    Event_id = 7015;
                    err = string.Format("DataTrac ActorLogin failed: {0}", tracTools.ErrorMsg);
                    err += string.Format(", Point File {0}, FileId={1}, DataTracPath={2}", strFilePaths, FileId, strServerPath);
                    logErr = true;
                    return false;
                }
                object objRt = tracTools.LOSFileImport4("POINT", strFilePaths, null, strLoanNumber, "FORCEADD", strOrigType, null, null);   // Empty String: Failure; FileId: Success
                //object objRt = tracTools.LOSFileImport("POINT", strFilePaths, null, strLoanNumber, objLogin, null, null);

                if ("" == string.Format("{0}", objRt) || objRt == null)
                {
                    Event_id = 7015;
                    err = string.Format("DataTrac SubmitLoan Error: {0}", tracTools.ErrorMsg);
                    err += string.Format(", Point File {0}, FileId={1}", strFilePaths, FileId);
                    logErr = true;
                    return false;
                }

                string dt_FileId = (string)objRt;
                if (!string.IsNullOrEmpty(dt_FileId))
                {
                    string SqlCmd = string.Format("Update Loans set DT_FileId='{0}' where FileId='{1}'", dt_FileId, FileId);
                    try
                    {
                        DbHelperSQL.ExecuteSql(SqlCmd);
                    }
                    catch (Exception ee)
                    {
                        err = "DataTrac SubmitLoan Error: " + ee.Message + "\n"+ ee.StackTrace;
                        err += string.Format(", Point File {0}, FileId={1}", strFilePaths, FileId);
                        Event_id = 7016;
                        logErr = true;
                        return false;
                    }
                }

                st = Pulse_DataTrac_Mapping( strLoanNumber, FileId, orig_type, loan_program, ref err);
                if ( st == false )
                {
                    err = "DataTrac SubmitLoan, Pulse_DataTrac_Mapping Error: " + err;
                    Event_id = 7017;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);                

                    logErr = true;
                }

                if (!UpdateHMDAFields(FileId, ref strErr))
                {
                    err = "DataTrac SubmitLoan, UpdateHMDAFields Error: " + strErr;
                    Event_id = 7018;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);                

                    logErr = true;
                }

                if (!UpdateLoanContact(FileId, "Listing Agent", ref strErr))
                {
                    err = "DataTrac SubmitLoan, Listing Agent UpdateLoanContact Error: " + strErr;
                    Event_id = 7019;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);                

                    logErr = true;
                }

                if (!UpdateLoanContact(FileId, "Buyer's Agent", ref strErr))
                {
                    err = "DataTrac SubmitLoan, Buyer's Agent, UpdateLoanContact Error: " + strErr;
                    Event_id = 7020;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);                

                    logErr = true;
                }

                tracTools.ActorLogout();

                err1 = string.Format("DataTrac Submit Loan Successfully: Loan Number='{0}', fileId={1}.", strLoanNumber, FileId);
                Event_id = 7001;
                logErr = false;

                return true;

            }
            catch (Exception ex)
            {
                Event_id = 7099;
                err = "DataTracMgr.SubmitLoan, Exception:" + ex.Message + "\n" + ex.StackTrace;
                err += string.Format(", Point File {0}, FileId={1}", strFilePaths, FileId);
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    err = err + err1;
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);                
                }
                else
                {
                    err = err1;                    
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id);
                }
            }
        }

        public bool UpdateHMDAFields(int FileId, ref string err)
        {
            string strLoanNumber = dataAccess.GetLoanNumber(FileId);
            object objRt = tracTools.LoadLoanData(strLoanNumber, "loan_num", "");
            if ("1" != string.Format("{0}", objRt))
            {
                err = string.Format("{0}", tracTools.ErrorMsg);
                return false;
            }

            string strValue = "";
            int nPointFieldId = 0;
            string strDTField = "";
            List<string[]> listFieldName = new List<string[]> { 
                new string[]{"MSA/MD Code", "6210", "und.msacode"}, 
                new string[]{"County Code", "6211", "und.countycode"}, 
                new string[]{"State Code", "5001", "und.statecode"}, 
                new string[]{"Census Tract", "6212", "und.cenus_trac"}, 
                //new string[]{"Lien Status", "6283", "gen.lien"},    // ??
                //new string[]{"Property Type", "6284", "gen.prog_type"}, // ??
                new string[]{"Purchaser Type", "6285", "und.purch_type"}
            };

            bool bIsAnySave = false;    // set to true when call TracTools.SetFieldValue
            foreach (string[] arr in listFieldName)
            {
                nPointFieldId = int.Parse(arr[1]);
                strDTField = arr[2];

                // set field value if not empty or null
                strValue = dataAccess.GetLoanFieldValue(FileId, nPointFieldId);
                if (!string.IsNullOrEmpty(strValue))
                {
                    objRt = tracTools.SetFieldValue(strDTField, strValue);
                    bIsAnySave = true;
                    if (DBNull.Value == objRt || null == objRt)
                    {
                        err = string.Format("{0}", tracTools.ErrorMsg);
                        return false;
                    }
                }
                else
                    continue;
            }

            if (bIsAnySave)
            {
                objRt = tracTools.Save();
                if ("1" != string.Format("{0}", objRt))
                {
                    err = string.Format("{0}", tracTools.ErrorMsg);
                    return false;
                }
            }

            return true;
        }

        public bool UpdateLoanContact(int FileId, string Role, ref string err)
        {
            string strLoanNumber = dataAccess.GetLoanNumber(FileId);
            object objRt = tracTools.LoadLoanData(strLoanNumber, "loan_num", "");
            if ("1" != string.Format("{0}", objRt))
            {
                err = string.Format("{0}", tracTools.ErrorMsg);
                return false;
            }

            int nNameFieldId = 0;
            int nEmailFieldId = 0;
            string strDTNameField = "";
            string strDTEmailFieId = "";
            string strNameValue = "";
            string strEmailValue = "";
            if ("listing agent" == Role.ToLower())
            {
                nNameFieldId = 6130;
                nEmailFieldId = 12378;
                strDTNameField = "fun.l_agt_name";
                strDTEmailFieId = "doc.l_agt_email";
            }
            else if ("buyer's agent" == Role.ToLower())
            {
                nNameFieldId = 6191;
                nEmailFieldId = 6195;
                strDTNameField = "fun.s_agt_name";
                strDTEmailFieId = "doc.s_agt_email";
            }
            else
            {
                err = "Not supported.";
                return false;
            }
            strNameValue = dataAccess.GetLoanFieldValue(FileId, nNameFieldId);
            if (!string.IsNullOrEmpty(strNameValue))
            {
                strEmailValue = dataAccess.GetLoanFieldValue(FileId, nEmailFieldId);

                objRt = tracTools.SetFieldValue(strDTNameField, strNameValue);
                if (DBNull.Value == objRt || null == objRt)
                {
                    err = string.Format("{0}", tracTools.ErrorMsg);
                    return false;
                }
                objRt = tracTools.SetFieldValue(strDTEmailFieId, strEmailValue);
                if (DBNull.Value == objRt || null == objRt)
                {
                    err = string.Format("{0}", tracTools.ErrorMsg);
                    return false;
                }

                objRt = tracTools.Save();
                if ("1" != string.Format("{0}", objRt))
                {
                    err = string.Format("{0}", tracTools.ErrorMsg);
                    return false;
                }
            }

            return true;
        }

        #region Private method for Datatrac API

        /// <summary>
        /// Login DataTrac Server
        /// </summary>
        /// <param name="strUN"></param>
        /// <param name="strPWD"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        private bool Login(string strUN, string strPWD, ref string strErr)
        {
            tracTools.PATHTODATATRAC = strServerPath;
            object objRt = null;
            try
            {
                objRt = tracTools.Login(strUN, strPWD);
                string strRt = string.Format("{0}", objRt);
                if ("1" == strRt)
                    return true;
                else
                {
                    strErr = string.Format("{0}", tracTools.ErrorMsg);
                    return false;
                }
            }
            catch (Exception ex)
            {
                strErr = string.Format("There is an error when login to DataTrac server: {0}", ex.Message);
                return false;
            }
        }

        private List<Common.Table.Loans> GetLoanInfoByNumber(DataTable dtLoanNums, ref string err)
        {
            if (null == dtLoanNums || dtLoanNums.Rows.Count <= 0)
                return null;

            StringBuilder sbNums = new StringBuilder();
            foreach (DataRow dr in dtLoanNums.Rows)
            {
                string strTemp = string.Format("{0}", dr["LoanNumber"]);
                              
                if (strTemp.Trim().Length > 0)
                {
                    if (sbNums.Length > 0)
                        sbNums.Append(",");
                    sbNums.AppendFormat("'{0}'", strTemp);
                }
            }

            if (sbNums.Length <= 0)
                return null;

            DataSet dsLoan = GetLoanInfoUseSql(sbNums.ToString(), ref err);
            if (null != dsLoan && dsLoan.Tables.Count > 0 && dsLoan.Tables[0].Rows.Count > 0)
            {
                List<Common.Table.Loans> listLoans = new List<Common.Table.Loans>();
                Common.Table.Loans loan = null;
                foreach (DataRow dataRow in dsLoan.Tables[0].Rows)
                {
                    loan = GetLoanFromDataRow(dataRow);
                    DataRow[] arrRows = dtLoanNums.Select(string.Format("LoanNumber='{0}'", dataRow["loan_num"]));
                    if (arrRows.Length == 1)
                    {
                        int nTemp = 0;
                        if (!int.TryParse(string.Format("{0}", arrRows[0]["FileId"]), out nTemp))
                            nTemp = 0;
                        if (nTemp <= 0)
                            continue;
                        loan.FileId = nTemp;
                        loan.LoanNumber = string.Format("{0}", arrRows[0]["LoanNumber"]);
                        listLoans.Add(loan);
                    }
                }
                
                return listLoans;
            }
            else
                return null;
        }

        private DataSet GetLoanInfoUseSql(string strLoanNum, ref string err)
        {
            if (strLoanNum.Length <= 0)
                return null;
            strLoanNum = strLoanNum.Trim(new char[] { '\'' });
            strLoanNum = string.Format("'{0}'", strLoanNum);

            string strSql = string.Format(@"SELECT gen.file_id, gen.loan_num,
gen.appras_val,gen.cltv,gen.app_date,gen.lien,gen.bloan_amt,gen.ltv,gen.programs_id,gen.prop_no,
gen.prop_city,gen.prop_state,gen.prop_zip,gen.int_rate,gen.loan_term,gen.stage,
CASE gen.owner_occu WHEN 'YES' THEN 'Primary Residence' WHEN '2ND' THEN 'Secondary Home' WHEN 'NO' THEN 'Investment Property' ELSE '' END AS Occupancy,
(SELECT TOP 1 ta.county FROM counties ta INNER JOIN und ON ta.counties_id=und.counties_id AND und.file_id=file_id) AS County,
(SELECT TOP 1 und.hmda_info_audited FROM und WHERE und.file_id=file_id) AS DateHMDA,
(SELECT TOP 1 und.approved FROM und WHERE und.file_id=file_id) AS DateApprove,
(SELECT TOP 1 und.resub FROM und WHERE und.file_id=file_id) AS DateReSubmit,
(SELECT TOP 1 und.clear_to_close FROM und WHERE und.file_id=file_id) AS DateClearToClose,
(SELECT TOP 1 doc.note_date FROM doc WHERE doc.file_id=file_id) as DateNote,
(SELECT TOP 1 und.foreclose_date FROM und WHERE und.file_id=file_id) AS DateClose,
(SELECT TOP 1 und.denied FROM und WHERE und.file_id=file_id) AS DateDenied,
(SELECT TOP 1 und.cancelled FROM und WHERE und.file_id=file_id) AS DateCanceled,
(SELECT TOP 1 und.Suspended FROM und WHERE und.file_id=file_id) AS DateSuspended,
(SELECT TOP 1 doc.drawn FROM doc WHERE doc.file_id=file_id) AS DateDocs,
(SELECT TOP 1 doc.sent FROM doc WHERE doc.file_id=file_id) AS DateDocsOut,
(SELECT TOP 1 fun.funded FROM fun WHERE fun.file_id=file_id) AS DateFund,
(SELECT TOP 1 doc.returned FROM doc WHERE doc.file_id=file_id) AS DateDocsReceived,
(SELECT TOP 1 mkt.est_closing FROM mkt WHERE mkt.file_id=file_id) AS EstCloseDate,
(SELECT TOP 1 mkt.sell_price FROM mkt WHERE mkt.file_id=file_id) AS SalesPrice,
(SELECT TOP 1 CASE ISNULL(programs.prog_type, '') WHEN '' THEN g.prog_type_other ELSE programs.prog_type END FROM gen g LEFT JOIN programs ON g.programs_id=programs.programs_id where g.file_id=gen.file_id) as Program,
CASE gen.purpose WHEN 'C' THEN 'Construction' WHEN 'D' THEN 'Debt Consolidation' WHEN 'E' THEN 'Equity Line' WHEN 'H' THEN 'Home Improvement' WHEN 'L' THEN 'Lease Option' WHEN 'O' THEN gen.purpose_other WHEN 'P' THEN 'Purchase' WHEN 'R' THEN 'Refinance' WHEN 'T' THEN 'Title' ELSE '' END AS Purpose,
(SELECT TOP 1 CASE ISNULL(relocks.lock_exp, CAST(-53690 AS DATETIME)) WHEN '1753-01-01' THEN g.lock_expire ELSE relocks.lock_exp END FROM gen g LEFT JOIN relocks ON g.file_id=relocks.file_id WHERE g.file_id=gen.file_id ORDER BY relocks.locked DESC) AS RateLockExpiration
FROM gen WHERE loan_num IN ({0})", strLoanNum);
            tracTools.PATHTODATATRAC = strServerPath;
            object objLogin = tracTools.ActorLogin(strLoginName, strLoginPwd);
            object objQueryReturn = tracTools.ExecuteSQL(strSql, "recordset", null);
            if (System.DBNull.Value == objQueryReturn)
            {
                err = string.Format("{0}", tracTools.ErrorMsg);
                return null;
            }

            OleDbDataAdapter adapter = new OleDbDataAdapter();
            DataSet dsLoan = new DataSet();
            adapter.Fill(dsLoan, objQueryReturn, "Loans");
            return dsLoan;
        }

        private Common.Table.Loans GetLoanFromDataRow(DataRow dr)
        {
            Common.Table.Loans loan = new Common.Table.Loans();
            object obj = null;
            obj = dr["appras_val"];
            loan.AppraisedValue = ParseDouble(obj);
            //obj = dr["scenarios_id"];
            //loan.CCScenario = string.Format("{0}", obj).Trim();
            obj = dr["cltv"];
            loan.CLTV = ParseDouble(obj);
            obj = dr["County"];
            loan.County = string.Format("{0}", obj).Trim();
            obj = dr["DateHMDA"];
            loan.DateHMDA = ParseDateTime(obj);
            obj = dr["app_date"];
            loan.DateSubmit = ParseDateTime(obj);
            obj = dr["DateApprove"];
            loan.DateApprove = ParseDateTime(obj);
            obj = dr["DateReSubmit"];
            loan.DateReSubmit = ParseDateTime(obj);
            obj = dr["DateClearToClose"];
            loan.DateClearToClose = ParseDateTime(obj);
            obj = dr["DateNote"];
            loan.DateNote = ParseDateTime(obj);
            obj = dr["DateDocs"];
            loan.DateDocs = ParseDateTime(obj);
            obj = dr["DateDocsOut"];
            loan.DateDocsOut = ParseDateTime(obj);
            obj = dr["DateFund"];
            loan.DateFund = ParseDateTime(obj);
            obj = dr["DateDocsReceived"];
            loan.DateDocsReceived = ParseDateTime(obj);
            obj = dr["DateClose"];
            loan.DateClose = ParseDateTime(obj);
            obj = dr["DateDenied"];
            loan.DateDenied = ParseDateTime(obj);
            obj = dr["DateCanceled"];
            loan.DateCanceled = ParseDateTime(obj);
            obj = dr["DateSuspended"];
            loan.DateSuspended = ParseDateTime(obj);
            obj = dr["EstCloseDate"];
            loan.EstCloseDate = ParseDateTime(obj);

            obj = dr["Occupancy"];
            string strTemp = string.Format("{0}", obj).Trim();
            if (!string.IsNullOrEmpty(strTemp))
            {
                if (strTemp.ToUpper().Contains("PRIMARY"))
                    strTemp = "Primary";
                else if (strTemp.ToUpper().Contains("SECONDARY"))
                    strTemp = "Secondary";
                else if (strTemp.ToUpper().Contains("INVESTMENT"))
                    strTemp = "Investment";
            }
            loan.Occupancy = strTemp;

            obj = dr["lien"];
            strTemp = string.Format("{0}", obj).Trim();
            if (!string.IsNullOrEmpty(strTemp))
            {
                switch (strTemp.ToUpper())
                {
                    case "F":
                        strTemp = "First";
                        break;
                    case "S":
                        strTemp = "Second";
                        break;
                    default:
                        break;
                }
            }
            loan.LienPosition = strTemp;
            obj = dr["bloan_amt"];
            loan.LoanAmount = ParseDouble(obj);
            obj = dr["ltv"];
            loan.LTV = ParseDouble(obj);
            obj = dr["Program"];
            loan.Program = string.Format("{0}", obj).Trim();
            obj = dr["prop_no"];
            loan.PropertyAddr = string.Format("{0}", obj).Trim();
            obj = dr["prop_city"];
            loan.PropertyCity = string.Format("{0}", obj).Trim();
            obj = dr["prop_state"];
            loan.PropertyState = string.Format("{0}", obj).Trim();
            obj = dr["prop_zip"];
            loan.PropertyZip = string.Format("{0}", obj).Trim();
            obj = dr["Purpose"];
            loan.Purpose = string.Format("{0}", obj).Trim();
            obj = dr["int_rate"];
            loan.Rate = ParseDouble(obj);
            obj = dr["SalesPrice"];
            loan.SalesPrice = ParseDouble(obj);
            obj = dr["loan_term"];
            loan.Term = ParseDouble(obj);
            obj = dr["RateLockExpiration"];
            loan.RateLockExpiration = ParseDateTime(obj);
            obj = dr["stage"];
            loan.CurrentStage = string.Format("{0}", obj).Trim();

            return loan;
        }

        private Common.Table.Contacts GetContactFromDataRow(DataRow dr)
        {
            Common.Table.Contacts borrower = new Common.Table.Contacts();
            object obj = null;
            obj = dr["cborrow_bday"];
            borrower.DOB = ParseDateTime(obj);
            obj = dr["cborrow_ssn"];
            borrower.SSN = string.Format("{0}", obj).Trim();
            obj = dr["TransUnion"];
            borrower.TransUnion = ParseInt(obj);
            obj = dr["Experian"];
            borrower.Experian = ParseInt(obj);
            obj = dr["Equifax"];
            borrower.Equifax = ParseInt(obj);
            return borrower;
        }

        private DateTime ParseDateTime(object obj)
        {
            DateTime dtTemp = DateTime.MinValue;
            if (null != obj && System.DBNull.Value != obj)
            {
                try
                {
                    dtTemp = Convert.ToDateTime(obj);
                }
                catch
                { }
            }
            return dtTemp;
        }

        private Double ParseDouble(object obj)
        {
            Double dTemp = 0d;
            if (null != obj && System.DBNull.Value != obj)
            {
                try
                {
                    dTemp = Convert.ToDouble(obj);
                }
                catch
                { }
            }
            return dTemp;
        }

        private int ParseInt(object obj)
        {
            int nTemp = 0;
            if (null != obj && System.DBNull.Value != obj)
            {
                try
                {
                    nTemp = Convert.ToInt32(obj);
                }
                catch
                { }
            }
            return nTemp;
        }

        private DataSet GetDataFromDataTracDB(string strSql)
        {
            object objQueryReturn = tracTools.ExecuteSQL(strSql, "recordset", null);
            if (System.DBNull.Value == objQueryReturn)
            {
                return null;
            }

            OleDbDataAdapter adapter = new OleDbDataAdapter();
            DataSet ds = new DataSet();
            adapter.Fill(ds, objQueryReturn, "table");
            return ds;
        }
        #endregion
    }
    #endregion
}
