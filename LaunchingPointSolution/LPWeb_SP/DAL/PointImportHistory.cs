using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类PointImportHistory。
	/// </summary>
    public class PointImportHistory : PointImportHistoryBase
	{
		public PointImportHistory()
        { }

        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = "lpvw_PointImportErrors";
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
            parameters[0].Value = tempTable;
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
        /// delete point import histories
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public bool DeleteImportHistory(List<int> listIDs)
        {
            if (listIDs.Count > 0)
            {
                try
                {
                    string strIDs = string.Join("", listIDs.Select(i => i.ToString()).ToList().ToArray());
                    string strSql = string.Format("DELETE FROM PointImportHistory WHERE HistoryId IN ({0})", strIDs);
                    DbHelperSQL.ExecuteSql(strSql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
	}
}

