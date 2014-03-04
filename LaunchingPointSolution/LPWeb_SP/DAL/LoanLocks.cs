using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    public class LoanLocks : LoanLocksBase
    {
        public LoanLocks()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <returns></returns>
        public DataTable GetLoanLocksInfo(int iFileId)
        {
            string sSql = "select * from LoanLocks where FileId=" + iFileId;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="sLockOption"></param>
        public void UpdateLockOption(int iFileId, string sLockOption) 
        {
            string sSql = "update LoanLocks set LockOption=@LockOption where FileId=" + iFileId;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@LockOption", SqlDbType.NVarChar, sLockOption);
            
            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }
    }
}
