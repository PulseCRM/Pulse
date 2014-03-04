using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Company_Alerts。
	/// </summary>
	public class Company_AlertsBase
    {
        public Company_AlertsBase()
		{}
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.Company_Alerts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_Alerts(");
            strSql.Append("AlertYellowDays,AlertRedDays,TaskYellowDays,TaskRedDays,RateLockYellowDays,RateLockRedDays)");
            strSql.Append(" values (");
            strSql.Append("@AlertYellowDays,@AlertRedDays,@TaskYellowDays,@TaskRedDays,@RateLockYellowDays,@RateLockRedDays)");
            SqlParameter[] parameters = {
					new SqlParameter("@AlertYellowDays", SqlDbType.SmallInt,2),
					new SqlParameter("@AlertRedDays", SqlDbType.SmallInt,2),
					new SqlParameter("@TaskYellowDays", SqlDbType.SmallInt,2),
					new SqlParameter("@TaskRedDays", SqlDbType.SmallInt,2),
					new SqlParameter("@RateLockYellowDays", SqlDbType.SmallInt,2),
					new SqlParameter("@RateLockRedDays", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.AlertYellowDays;
            parameters[1].Value = model.AlertRedDays;
            parameters[2].Value = model.TaskYellowDays;
            parameters[3].Value = model.TaskRedDays;
            parameters[4].Value = model.RateLockYellowDays;
            parameters[5].Value = model.RateLockRedDays;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Company_Alerts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Company_Alerts set ");
            strSql.Append("AlertYellowDays=@AlertYellowDays,");
            strSql.Append("AlertRedDays=@AlertRedDays,");
            strSql.Append("TaskYellowDays=@TaskYellowDays,");
            strSql.Append("TaskRedDays=@TaskRedDays,");
            strSql.Append("RateLockYellowDays=@RateLockYellowDays,");
            strSql.Append("RateLockRedDays=@RateLockRedDays");
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
					new SqlParameter("@AlertYellowDays", SqlDbType.SmallInt,2),
					new SqlParameter("@AlertRedDays", SqlDbType.SmallInt,2),
					new SqlParameter("@TaskYellowDays", SqlDbType.SmallInt,2),
					new SqlParameter("@TaskRedDays", SqlDbType.SmallInt,2),
					new SqlParameter("@RateLockYellowDays", SqlDbType.SmallInt,2),
					new SqlParameter("@RateLockRedDays", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.AlertYellowDays;
            parameters[1].Value = model.AlertRedDays;
            parameters[2].Value = model.TaskYellowDays;
            parameters[3].Value = model.TaskRedDays;
            parameters[4].Value = model.RateLockYellowDays;
            parameters[5].Value = model.RateLockRedDays;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Company_Alerts ");
            strSql.Append(" where ");
            SqlParameter[] parameters = {
};

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_Alerts GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 AlertYellowDays,AlertRedDays,TaskYellowDays,TaskRedDays,RateLockYellowDays,RateLockRedDays from Company_Alerts ");
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
};

            LPWeb.Model.Company_Alerts model = new LPWeb.Model.Company_Alerts();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["AlertYellowDays"].ToString() != "")
                {
                    model.AlertYellowDays = int.Parse(ds.Tables[0].Rows[0]["AlertYellowDays"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AlertRedDays"].ToString() != "")
                {
                    model.AlertRedDays = int.Parse(ds.Tables[0].Rows[0]["AlertRedDays"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TaskYellowDays"].ToString() != "")
                {
                    model.TaskYellowDays = int.Parse(ds.Tables[0].Rows[0]["TaskYellowDays"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TaskRedDays"].ToString() != "")
                {
                    model.TaskRedDays = int.Parse(ds.Tables[0].Rows[0]["TaskRedDays"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RateLockYellowDays"].ToString() != "")
                {
                    model.RateLockYellowDays = int.Parse(ds.Tables[0].Rows[0]["RateLockYellowDays"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RateLockRedDays"].ToString() != "")
                {
                    model.RateLockRedDays = int.Parse(ds.Tables[0].Rows[0]["RateLockRedDays"].ToString());
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
            strSql.Append("select AlertYellowDays,AlertRedDays,TaskYellowDays,TaskRedDays,RateLockYellowDays,RateLockRedDays ");
            strSql.Append(" FROM Company_Alerts ");
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
            strSql.Append(" AlertYellowDays,AlertRedDays,TaskYellowDays,TaskRedDays,RateLockYellowDays,RateLockYellowDays ");
            strSql.Append(" FROM Company_Alerts ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
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
            parameters[0].Value = "Company_Alerts";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        #endregion  成员方法
	}
}

