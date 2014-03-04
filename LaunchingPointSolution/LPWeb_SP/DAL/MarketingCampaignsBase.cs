using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:MarketingCampaigns
    /// </summary>
    public class MarketingCampaignsBase
    {
        public MarketingCampaignsBase()
        { }
        #region  Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.MarketingCampaigns model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into MarketingCampaigns(");
            strSql.Append("GlobalId,CampaignName,CategoryId,Description,Price,ImageUrl,Status)");
            strSql.Append(" values (");
            strSql.Append("@GlobalId,@CampaignName,@CategoryId,@Description,@Price,@ImageUrl,@Status)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@GlobalId", SqlDbType.NVarChar,255),
					new SqlParameter("@CampaignName", SqlDbType.NVarChar,255),
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar),
					new SqlParameter("@Price", SqlDbType.Money,8),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.GlobalId;
            parameters[1].Value = model.CampaignName;
            parameters[2].Value = model.CategoryId;
            parameters[3].Value = model.Description;
            parameters[4].Value = model.Price;
            parameters[5].Value = model.ImageUrl;
            parameters[6].Value = model.Status;

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
        public bool Update(LPWeb.Model.MarketingCampaigns model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update MarketingCampaigns set ");
            strSql.Append("GlobalId=@GlobalId,");
            strSql.Append("CampaignName=@CampaignName,");
            strSql.Append("CategoryId=@CategoryId,");
            strSql.Append("Description=@Description,");
            strSql.Append("Price=@Price,");
            strSql.Append("ImageUrl=@ImageUrl,");
            strSql.Append("Status=@Status");
            strSql.Append(" where CampaignId=@CampaignId");
            SqlParameter[] parameters = {
					new SqlParameter("@CampaignId", SqlDbType.Int,4),
					new SqlParameter("@GlobalId", SqlDbType.NVarChar,255),
					new SqlParameter("@CampaignName", SqlDbType.NVarChar,255),
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar),
					new SqlParameter("@Price", SqlDbType.Money,8),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.CampaignId;
            parameters[1].Value = model.GlobalId;
            parameters[2].Value = model.CampaignName;
            parameters[3].Value = model.CategoryId;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.Price;
            parameters[6].Value = model.ImageUrl;
            parameters[7].Value = model.Status;

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
        public bool Delete(int CampaignId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from MarketingCampaigns ");
            strSql.Append(" where CampaignId=@CampaignId");
            SqlParameter[] parameters = {
					new SqlParameter("@CampaignId", SqlDbType.Int,4)
};
            parameters[0].Value = CampaignId;

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
        public bool DeleteList(string CampaignIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from MarketingCampaigns ");
            strSql.Append(" where CampaignId in (" + CampaignIdlist + ")  ");
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
        public LPWeb.Model.MarketingCampaigns GetModel(int CampaignId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 CampaignId,GlobalId,CampaignName,CategoryId,Description,Price,ImageUrl,Status from MarketingCampaigns ");
            strSql.Append(" where CampaignId=@CampaignId");
            SqlParameter[] parameters = {
					new SqlParameter("@CampaignId", SqlDbType.Int,4)
};
            parameters[0].Value = CampaignId;

            LPWeb.Model.MarketingCampaigns model = new LPWeb.Model.MarketingCampaigns();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CampaignId"].ToString() != "")
                {
                    model.CampaignId = int.Parse(ds.Tables[0].Rows[0]["CampaignId"].ToString());
                }
                model.GlobalId = ds.Tables[0].Rows[0]["GlobalId"].ToString();
                model.CampaignName = ds.Tables[0].Rows[0]["CampaignName"].ToString();
                if (ds.Tables[0].Rows[0]["CategoryId"].ToString() != "")
                {
                    model.CategoryId = int.Parse(ds.Tables[0].Rows[0]["CategoryId"].ToString());
                }
                model.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                if (ds.Tables[0].Rows[0]["Price"].ToString() != "")
                {
                    model.Price = decimal.Parse(ds.Tables[0].Rows[0]["Price"].ToString());
                }
                model.ImageUrl = ds.Tables[0].Rows[0]["ImageUrl"].ToString();
                model.Status = ds.Tables[0].Rows[0]["Status"].ToString();
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
            strSql.Append("select CampaignId,GlobalId,CampaignName,CategoryId,Description,Price,ImageUrl,Status ");
            strSql.Append(" FROM MarketingCampaigns ");
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
            strSql.Append(" CampaignId,GlobalId,CampaignName,CategoryId,Description,Price,ImageUrl,Status ");
            strSql.Append(" FROM MarketingCampaigns ");
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
            parameters[0].Value = "MarketingCampaigns";
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

