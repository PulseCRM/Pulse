using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    public class LoanProfit : LoanProfitBase
    {
        public LoanProfit()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <returns></returns>
        public DataTable GetLoanProfitInfo(int iFileId) 
        {
            string sSql = "select * from LoanProfit where FileId=" + iFileId;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="sCompensationPlan"></param>
        /// <param name="sLenderCredit"></param>
        public void UpdateLoanProfit(int iFileId, string sCompensationPlan, string sLenderCredit) 
        {
            string sSql = "update dbo.LoanProfit set CompensationPlan=@CompensationPlan, LenderCredit=@LenderCredit where FileId=" + iFileId;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@CompensationPlan", SqlDbType.NVarChar, sCompensationPlan);

            if (sLenderCredit == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@LenderCredit", SqlDbType.Decimal, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@LenderCredit", SqlDbType.Decimal, Convert.ToDecimal(sLenderCredit));
            }

            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }
    }
}
