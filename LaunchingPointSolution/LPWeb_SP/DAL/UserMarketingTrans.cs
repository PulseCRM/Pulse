using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类UserMarketingTrans。
    /// </summary>
    public class UserMarketingTrans : UserMarketingTransBase
    {
        public UserMarketingTrans()
        { }

        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = @"(SELECT a.TransId,a.UserId,a.TransTime,a.[Action],a.Amount,a.Balance,a.LoanMarketingId,
 a.[Description],b.FileId,c.[Status] AS LoanStatus FROM UserMarketingTrans a 
LEFT JOIN LoanMarketing b on a.LoanMarketingId=b.LoanMarketingId LEFT JOIN Loans c on b.FileId=c.FileId) as t";
            return GetListByPage(tempTable, PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        private DataSet GetListByPage(string strTableName, int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,2000),
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
    }
}
