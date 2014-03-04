using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanActivities。
	/// </summary>
    public class LoanActivities : LoanActivitiesBase
	{
		public LoanActivities()
        { }

        /// <summary>
        /// get data for loan activities list
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="count"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string strTableName = "(SELECT LA.*, ISNULL(U.UserId, '0') AS UserId2, CASE(ISNULL(U.UserId, '')) WHEN '' THEN 'System' ELSE ISNULL(U.FirstName, '') + ' ' + ISNULL(U.LastName, '') END AS PerformedBy FROM LoanActivities LA LEFT JOIN Users U ON LA.UserId=U.UserId) t";
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = strTableName;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "1=1 " + strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        /// <summary>
        /// Get proformedby user of loan activity
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetProformedBy(string strWhere)
        {
            string strSql = "SELECT DISTINCT LA.FileId, LA.UserId, U.FirstName, U.LastName, ISNULL(U.FirstName, '') + ' ' + ISNULL(U.LastName, '') AS PerformedBy FROM LoanActivities LA INNER JOIN Users U ON LA.UserId=U.UserId WHERE 1=1 {0} ORDER BY U.FirstName";
            strSql = string.Format(strSql, strWhere);
            return DbHelperSQL.ExecuteDataTable(strSql);
        }

        public void LoanReassignAcivities(int FileID,int UserID,int RoleID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int ),
					new SqlParameter("@UserId", SqlDbType.Int ),
					new SqlParameter("@RoleId", SqlDbType.Int )
					};
            parameters[0].Value = FileID;
            parameters[1].Value = UserID;
            parameters[2].Value = RoleID;
            DbHelperSQL.RunProcedure("lpsp_SaveLoanActivities", parameters, out rowsAffected);
        }

	}
}

