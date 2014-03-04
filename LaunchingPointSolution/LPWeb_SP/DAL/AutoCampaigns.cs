using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类AutoCampaigns。
    /// </summary>
    public class AutoCampaigns : AutoCampaignsBase
    {
        public DataSet GetMarketingList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT MC.CampaignId,MC.CampaignName AS [Marketing Campaign],AC.LoanType,AC.TemplStageId,AC.Enabled FROM AutoCampaigns AC INNER JOIN MarketingCampaigns MC
         ON AC.CampaignId=MC.CampaignId
         WHERE AC.PaidBy=0");

            return DbHelperSQL.Query(strSql.ToString());
        }

        public int GetMarketingCount(string sWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select count(*) from (SELECT MC.CampaignId,MC.CampaignName AS [Marketing Campaign],AC.LoanType,AC.TemplStageId,AC.Enabled FROM AutoCampaigns AC INNER JOIN MarketingCampaigns MC
         ON AC.CampaignId=MC.CampaignId
         WHERE AC.PaidBy=0) t ");
            strSql.Append("where 1=1 ");
            strSql.Append(sWhere);

            object obj = DbHelperSQL.ExecuteScalar(strSql.ToString());
            if (obj != null)
                return Convert.ToInt32(obj);

            return default(int);
        }
        public DataSet GetMarketingList(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string strTable = @"(SELECT MC.CampaignId,MC.CampaignName AS [Marketing Campaign],mcat.CategoryName,AC.LoanType,AC.TemplStageId,AC.Enabled FROM AutoCampaigns AC INNER JOIN MarketingCampaigns MC
         ON AC.CampaignId=MC.CampaignId LEFT JOIN  MarketingCategory mcat ON mcat.CategoryId = mc.CategoryId
         WHERE AC.PaidBy=0) T ";
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 2000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = strTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }
        public DataSet GetMarketingList(int PageSize, int PageIndex, string strWhere)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
            parameters[0].Value = @"SELECT * FROM (SELECT MC.CampaignId,MC.CampaignName AS [Marketing Campaign],AC.LoanType,AC.TemplStageId,AC.Enabled FROM AutoCampaigns AC INNER JOIN MarketingCampaigns MC
         ON AC.CampaignId=MC.CampaignId
         WHERE AC.PaidBy=0) T ";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }



        /// <summary>
        /// Clear all company marketing data
        /// Rocky
        /// </summary>
        public void DeleteAll(string sType)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from AutoCampaigns");
            strSql.Append(" where PaidBy=@sType ");
            SqlParameter[] parameters = {
					new SqlParameter("@sType", SqlDbType.VarChar,1)};
            parameters[0].Value = sType;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

    }
}

