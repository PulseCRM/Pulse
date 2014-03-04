using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    public class MarketingCampaigns : MarketingCampaignsBase
    {
        public DataSet GetCampaignsList(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string strTable = string.Format("(select CampaignId,mc.GlobalId,CampaignName,mcat.CategoryName,WebServiceURL,CampaignDetailURL,mc.CategoryId ");
            strTable += string.Format(" from dbo.MarketingCampaigns mc left join dbo.MarketingSettings ms on mc.CampaignId =ms.ReconcileInterval ");
            strTable += string.Format(" LEFT JOIN dbo.MarketingCategory mcat ON mcat.CategoryId = mc.CategoryId) as tblNotes", strWhere);

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

        public DataSet GetCampaignsListForPersonlizationAdd(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            //string tempTable = "MarketingCampaigns";
            //gdc 20110612 Modify
            string tempTable = "(SELECT mc.*,mcat.CategoryName FROM MarketingCampaigns mc LEFT JOIN dbo.MarketingCategory mcat ON mcat.CategoryId = mc.CategoryId) T";
            return GetListByPage(tempTable, PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        public DataTable GetMarketingCategoryInfo()
        {
            string sSql = "select * from dbo.MarketingCategory";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataSet GetListForPersonlization(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = "(SELECT a.*, b.CampaignName, c.Name AS TemplStageName FROM AutoCampaigns a LEFT JOIN MarketingCampaigns b ON a.CampaignId=b.CampaignId LEFT JOIN Template_Stages c on a.TemplStageId=c.TemplStageId) t";
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

        public void AddAutoCampaigns(int[] campaignIds, string strLoanType, int nTemplStageId, int nUserId)
        {
            StringBuilder sbSql = new StringBuilder();

            foreach (int nId in campaignIds)
            {
                sbSql.AppendFormat("IF NOT EXISTS(SELECT 1 FROM AutoCampaigns WHERE CampaignId='{0}' AND SelectedBy='{1}') INSERT INTO AutoCampaigns(CampaignId, PaidBy, Enabled, SelectedBy, LoanType, TemplStageId) VALUES({0}, 2, 1, {1}, '{2}', {3});",
                    nId, nUserId, strLoanType, nTemplStageId);
            }

            SqlCommand cmdAddAutoCampaigns = new SqlCommand();
            cmdAddAutoCampaigns.CommandType = CommandType.Text;
            cmdAddAutoCampaigns.CommandText = sbSql.ToString();
            DbHelperSQL.ExecuteNonQuery(cmdAddAutoCampaigns);
        }

        public void SaveAutoCampaigns(DataTable dtAc)
        {
            if (null != dtAc && dtAc.Rows.Count > 0)
            {
                StringBuilder sbSql = new StringBuilder();
                foreach (DataRow dr in dtAc.Rows)
                {
                    sbSql.AppendFormat("UPDATE AutoCampaigns SET LoanType='{0}', TemplStageId=(SELECT TemplStageId FROM Template_Stages WHERE TemplStageId='{1}'), Enabled={2} WHERE CampaignId={3};",
                        dr["LoanType"], dr["TemplStageId"], dr["Enabled"], dr["ID"]);
                }
                SqlCommand cmdUpdateAutoCampaigns = new SqlCommand();
                cmdUpdateAutoCampaigns.CommandType = CommandType.Text;
                cmdUpdateAutoCampaigns.CommandText = sbSql.ToString();
                DbHelperSQL.ExecuteNonQuery(cmdUpdateAutoCampaigns);
            }
        }

        public void RemoveAutoCampaigns(int[] campaignIds)
        {
            if (campaignIds.Length > 0)
            {
                string strSql = string.Format("DELETE AutoCampaigns WHERE CampaignId IN ({0});", string.Join(",", campaignIds.Select(i => i.ToString()).ToArray()));
                SqlCommand cmdAddAutoCampaigns = new SqlCommand();
                cmdAddAutoCampaigns.CommandType = CommandType.Text;
                DbHelperSQL.ExecuteNonQuery(cmdAddAutoCampaigns);
            }
        }

        /// <summary>
        /// get marketing campaigns in alphabetical order
        /// </summary>
        public DataSet GetListInAlphOrder(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CampaignId,GlobalId,CampaignName,CategoryId,Description,Price,ImageUrl,Status ");
            strSql.Append(" FROM MarketingCampaigns ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" ORDER BY CampaignName");
            return DbHelperSQL.Query(strSql.ToString());
        }
    }
}
