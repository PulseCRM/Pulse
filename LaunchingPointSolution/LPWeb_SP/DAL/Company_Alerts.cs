using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Company_Alerts。
    /// </summary>
    public class Company_Alerts : Company_AlertsBase
    {
        public Company_Alerts()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.Company_Alerts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_Alerts(");
            strSql.Append("AlertYellowDays,AlertRedDays,TaskYellowDays,TaskRedDays,RateLockYellowDays,RateLockRedDays,SendEmailCustomTasks,CustomTaskEmailTemplId)");//gdc CR40
            strSql.Append(" values (");
            strSql.Append("@AlertYellowDays,@AlertRedDays,@TaskYellowDays,@TaskRedDays,@RateLockYellowDays,@RateLockRedDays,@SendEmailCustomTasks,@CustomTaskEmailTemplId)");//gdc CR40
            SqlParameter[] parameters = {
					new SqlParameter("@AlertYellowDays", SqlDbType.SmallInt,2),
					new SqlParameter("@AlertRedDays", SqlDbType.SmallInt,2),
					new SqlParameter("@TaskYellowDays", SqlDbType.SmallInt,2),
					new SqlParameter("@TaskRedDays", SqlDbType.SmallInt,2),
					new SqlParameter("@RateLockYellowDays", SqlDbType.SmallInt,2),
					new SqlParameter("@RateLockRedDays", SqlDbType.SmallInt,2),
                    new SqlParameter("@SendEmailCustomTasks", SqlDbType.Bit,2),//gdc CR40
                    new SqlParameter("@CustomTaskEmailTemplId", SqlDbType.Int)};//gdc CR40
            parameters[0].Value = model.AlertYellowDays;
            parameters[1].Value = model.AlertRedDays;
            parameters[2].Value = model.TaskYellowDays;
            parameters[3].Value = model.TaskRedDays;
            parameters[4].Value = model.RateLockYellowDays;
            parameters[5].Value = model.RateLockRedDays;
            parameters[6].Value = model.SendEmailCustomTasks;//gdc CR40
            parameters[7].Value = model.CustomTaskEmailTemplId;//gdc CR40

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
            strSql.Append("RateLockRedDays=@RateLockRedDays,");
            strSql.Append("SendEmailCustomTasks=@SendEmailCustomTasks,");//gdc CR40
            strSql.Append("CustomTaskEmailTemplId=@CustomTaskEmailTemplId");//gdc CR40
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
					new SqlParameter("@AlertYellowDays", SqlDbType.SmallInt,2),
					new SqlParameter("@AlertRedDays", SqlDbType.SmallInt,2),
					new SqlParameter("@TaskYellowDays", SqlDbType.SmallInt,2),
					new SqlParameter("@TaskRedDays", SqlDbType.SmallInt,2),
					new SqlParameter("@RateLockYellowDays", SqlDbType.SmallInt,2),
					new SqlParameter("@RateLockRedDays", SqlDbType.SmallInt,2),
                    new SqlParameter("@SendEmailCustomTasks", SqlDbType.Bit,2),//gdc CR40
                    new SqlParameter("@CustomTaskEmailTemplId", SqlDbType.Int)};//gdc CR40
            parameters[0].Value = model.AlertYellowDays;
            parameters[1].Value = model.AlertRedDays;
            parameters[2].Value = model.TaskYellowDays;
            parameters[3].Value = model.TaskRedDays;
            parameters[4].Value = model.RateLockYellowDays;
            parameters[5].Value = model.RateLockRedDays;
            parameters[6].Value = model.SendEmailCustomTasks;//gdc CR40
            parameters[7].Value = model.CustomTaskEmailTemplId;//gdc CR40

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }



        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_Alerts GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 AlertYellowDays,AlertRedDays,TaskYellowDays,TaskRedDays,RateLockYellowDays,RateLockRedDays,CustomTaskEmailTemplId,SendEmailCustomTasks from Company_Alerts ");
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

                //gdc CR40
                if (ds.Tables[0].Rows[0]["CustomTaskEmailTemplId"] != DBNull.Value && ds.Tables[0].Rows[0]["CustomTaskEmailTemplId"].ToString() != "")
                {
                    model.CustomTaskEmailTemplId = int.Parse(ds.Tables[0].Rows[0]["CustomTaskEmailTemplId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SendEmailCustomTasks"] != DBNull.Value && ds.Tables[0].Rows[0]["SendEmailCustomTasks"].ToString() != "")
                {
                    model.SendEmailCustomTasks = Convert.ToBoolean(ds.Tables[0].Rows[0]["SendEmailCustomTasks"].ToString());
                }

                return model;
            }
            else
            {
                return null;
            }
        }


        #endregion  成员方法
    }
}

