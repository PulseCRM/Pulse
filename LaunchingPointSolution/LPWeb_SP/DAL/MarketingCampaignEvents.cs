using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:MarketingCampaignEvents
    /// </summary>
    public class MarketingCampaignEvents
    {
        public MarketingCampaignEvents()
        { }
        #region  Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.MarketingCampaignEvents model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into MarketingCampaignEvents(");
            strSql.Append("WeekNo,Action,EventContent,EventURL,GlobalId,CampaignId)");
            strSql.Append(" values (");
            strSql.Append("@WeekNo,@Action,@EventContent,@EventURL,@GlobalId,@CampaignId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@WeekNo", SqlDbType.Int,4),
					new SqlParameter("@Action", SqlDbType.NVarChar,50),
					new SqlParameter("@EventContent", SqlDbType.NVarChar),
					new SqlParameter("@EventURL", SqlDbType.NVarChar,255),
					new SqlParameter("@GlobalId", SqlDbType.NVarChar,255),
					new SqlParameter("@CampaignId", SqlDbType.Int,4)};
            parameters[0].Value = model.WeekNo;
            parameters[1].Value = model.Action;
            parameters[2].Value = model.EventContent;
            parameters[3].Value = model.EventURL;
            parameters[4].Value = model.GlobalId;
            parameters[5].Value = model.CampaignId;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.MarketingCampaignEvents model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update MarketingCampaignEvents set ");
            strSql.Append("WeekNo=@WeekNo,");
            strSql.Append("Action=@Action,");
            strSql.Append("EventContent=@EventContent,");
            strSql.Append("EventURL=@EventURL,");
            strSql.Append("GlobalId=@GlobalId,");
            strSql.Append("CampaignId=@CampaignId");
            strSql.Append(" where CampaignEventId=@CampaignEventId");
            SqlParameter[] parameters = {
					new SqlParameter("@CampaignEventId", SqlDbType.Int,4),
					new SqlParameter("@WeekNo", SqlDbType.Int,4),
					new SqlParameter("@Action", SqlDbType.NVarChar,50),
					new SqlParameter("@EventContent", SqlDbType.NVarChar),
					new SqlParameter("@EventURL", SqlDbType.NVarChar,255),
					new SqlParameter("@GlobalId", SqlDbType.NVarChar,255),
					new SqlParameter("@CampaignId", SqlDbType.Int,4)};
            parameters[0].Value = model.CampaignEventId;
            parameters[1].Value = model.WeekNo;
            parameters[2].Value = model.Action;
            parameters[3].Value = model.EventContent;
            parameters[4].Value = model.EventURL;
            parameters[5].Value = model.GlobalId;
            parameters[6].Value = model.CampaignId;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int CampaignEventId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from MarketingCampaignEvents ");
            strSql.Append(" where CampaignEventId=@CampaignEventId");
            SqlParameter[] parameters = {
					new SqlParameter("@CampaignEventId", SqlDbType.Int,4)
};
            parameters[0].Value = CampaignEventId;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string CampaignEventIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from MarketingCampaignEvents ");
            strSql.Append(" where CampaignEventId in (" + CampaignEventIdlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.MarketingCampaignEvents GetModel(int CampaignEventId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 CampaignEventId,WeekNo,Action,EventContent,EventURL,GlobalId,CampaignId from MarketingCampaignEvents ");
            strSql.Append(" where CampaignEventId=@CampaignEventId");
            SqlParameter[] parameters = {
					new SqlParameter("@CampaignEventId", SqlDbType.Int,4)
};
            parameters[0].Value = CampaignEventId;

            LPWeb.Model.MarketingCampaignEvents model = new LPWeb.Model.MarketingCampaignEvents();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CampaignEventId"].ToString() != "")
                {
                    model.CampaignEventId = int.Parse(ds.Tables[0].Rows[0]["CampaignEventId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["WeekNo"].ToString() != "")
                {
                    model.WeekNo = int.Parse(ds.Tables[0].Rows[0]["WeekNo"].ToString());
                }
                model.Action = ds.Tables[0].Rows[0]["Action"].ToString();
                model.EventContent = ds.Tables[0].Rows[0]["EventContent"].ToString();
                model.EventURL = ds.Tables[0].Rows[0]["EventURL"].ToString();
                model.GlobalId = ds.Tables[0].Rows[0]["GlobalId"].ToString();
                if (ds.Tables[0].Rows[0]["CampaignId"].ToString() != "")
                {
                    model.CampaignId = int.Parse(ds.Tables[0].Rows[0]["CampaignId"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CampaignEventId,WeekNo,Action,EventContent,EventURL,GlobalId,CampaignId ");
            strSql.Append(" FROM MarketingCampaignEvents ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" CampaignEventId,WeekNo,Action,EventContent,EventURL,GlobalId,CampaignId ");
            strSql.Append(" FROM MarketingCampaignEvents ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
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
            parameters[0].Value = "MarketingCampaignEvents";
            parameters[1].Value = "";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  Method
    }
}

