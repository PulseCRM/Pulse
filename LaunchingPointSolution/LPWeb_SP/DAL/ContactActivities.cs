using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类ContactActivities。
	/// </summary>
    public class ContactActivities : ContactActivitiesBase
	{
		public ContactActivities()
		{}


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
            string strTableName = "(SELECT PA.*, CASE(ISNULL(PA.UserId, '')) WHEN '' THEN 'System' ELSE ISNULL(U.FirstName, '') + ' ' + ISNULL(U.LastName, '') END AS PerformedBy FROM ContactActivities PA LEFT JOIN Users U ON PA.UserId=U.UserId) t";
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

        public DataTable GetProformedBy(string strWhere)
        {
            string strSql = "SELECT DISTINCT PA.ContactId, PA.UserId, U.FirstName, U.LastName, ISNULL(U.FirstName, '') + ' ' + ISNULL(U.LastName, '') AS PerformedBy FROM ContactActivities PA INNER JOIN Users U ON PA.UserId=U.UserId WHERE 1=1 {0} ORDER BY U.FirstName";
            strSql = string.Format(strSql, strWhere);
            return DbHelperSQL.ExecuteDataTable(strSql);
        }

	}
}

