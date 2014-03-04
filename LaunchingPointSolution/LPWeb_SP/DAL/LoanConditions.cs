using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    public class LoanConditions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iConditionID"></param>
        /// <returns></returns>
        public DataTable GetLoanConditionsInfo(int iConditionID) 
        {
            string sSql = "select * from LoanConditions where LoanCondId = " + iConditionID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// GetConditionsList
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="strWhere"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataSet GetList(int PageIndex, int PageSize, string strWhere, out int count)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 4000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = "LoanConditions as a";
            parameters[1].Value = "*, (select top(1) Note from LoanNotes as b where b.FileId=a.FileId and b.LoanConditionId=a.LoanCondId order by Created desc) as Note";
            parameters[2].Value = "LoanCondId";
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        
        }


        public bool UpdateExternalViewing(int ID, bool ExternalViewing)
        {

            string sql = "UPDATE [LoanConditions] SET [ExternalViewing] = " + (ExternalViewing ? "1" : "0") + " WHERE [LoanCondId] = " + ID;

            int ret = DbHelperSQL.ExecuteSql(sql);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Update the Loan Condition Status to received
        /// </summary>
        /// <param name="ID">Condition</param>
        /// <param name="sEditor">Received By</param>
        /// <returns></returns>
        public bool UpdateConditionStatusToReceived(int ID, string sEditor)
        {

            string sql = "UPDATE [LoanConditions] SET [Status] = 'Received', [Received] = getdate(), [ReceivedBy] = '" + sEditor.Replace("'", "''") + "' WHERE [LoanCondId] = " + ID.ToString();

            int ret = DbHelperSQL.ExecuteSql(sql);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }
    }
}
