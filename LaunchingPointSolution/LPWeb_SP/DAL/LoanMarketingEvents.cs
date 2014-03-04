using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanMarketingEvents。
	/// </summary>
	public class LoanMarketingEvents : LoanMarketingEventsBase
	{
		public LoanMarketingEvents()
		{}

        public DataSet GetListForMarketingActivitiesEvents(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = @"(SELECT a.LoanMarketingEventId, a.Completed, c.CampaignName, a.[Action], a.ExecutionDate, 
a.EventContent, b.FileId, b.CampaignId, b.[Status], b.[Type], b.StartedBy, c.CategoryId,
'Week ' + CONVERT(nvarchar(50), ISNULL(a.WeekNo, 0)) + ' ' + ISNULL(a.[Action], '') AS [Event],
[dbo].[lpfn_GetBorrower] (b.FileId) AS BorrowerName, (SELECT [Status] FROM Loans WHERE FileId=b.FileId) AS LoanStatus,
(SELECT Name FROM PointFiles WHERE FileId=b.FileId) AS PointFileName FROM LoanMarketingEvents a 
LEFT JOIN LoanMarketing b on a.LoanMarketingId=b.LoanMarketingId
LEFT JOIN MarketingCampaigns c on b.CampaignId=c.CampaignId) t";
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

        public DataSet GetStartedByUserOfLoanMarketing(string strWhere)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT DISTINCT StartedBy, dbo.lpfn_GetUserName(StartedBy) AS StartedByUserName FROM LoanMarketing ORDER BY StartedByUserName");
            if (!string.IsNullOrEmpty(strWhere))
            {
                sbSql.Append(" where ");
                sbSql.Append(strWhere);
            }
            return DbHelperSQL.Query(sbSql.ToString());
        }

        public bool CompleteLoanMarketingEvent(int nId)
        {
            string strSql = string.Format("UPDATE LoanMarketingEvents SET Completed=1 WHERE LoanMarketingEventId='{0}'", nId);
            try
            {
                DbHelperSQL.ExecuteNonQuery(strSql);
                return true;
            }
            catch 
            {
                return false;
            }
        }
	}
}

