using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类UserGoals。
	/// </summary>
	public class UserGoalsBase
    {
        public UserGoalsBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.UserGoals model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UserGoals(");
            strSql.Append("UserId,LowRange,MediumRange,HighRange,Month)");
            strSql.Append(" values (");
            strSql.Append("@UserId,@LowRange,@MediumRange,@HighRange,@Month)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@LowRange", SqlDbType.Money,8),
					new SqlParameter("@MediumRange", SqlDbType.Money,8),
					new SqlParameter("@HighRange", SqlDbType.Money,8),
					new SqlParameter("@Month", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.LowRange;
            parameters[2].Value = model.MediumRange;
            parameters[3].Value = model.HighRange;
            parameters[4].Value = model.Month;

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
        public void Update(LPWeb.Model.UserGoals model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserGoals set ");
            strSql.Append("GoalId=@GoalId,");
            strSql.Append("UserId=@UserId,");
            strSql.Append("LowRange=@LowRange,");
            strSql.Append("MediumRange=@MediumRange,");
            strSql.Append("HighRange=@HighRange,");
            strSql.Append("Month=@Month");
            strSql.Append(" where GoalId=@GoalId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoalId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@LowRange", SqlDbType.Money,8),
					new SqlParameter("@MediumRange", SqlDbType.Money,8),
					new SqlParameter("@HighRange", SqlDbType.Money,8),
					new SqlParameter("@Month", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.GoalId;
            parameters[1].Value = model.UserId;
            parameters[2].Value = model.LowRange;
            parameters[3].Value = model.MediumRange;
            parameters[4].Value = model.HighRange;
            parameters[5].Value = model.Month;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int GoalId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UserGoals ");
            strSql.Append(" where GoalId=@GoalId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoalId", SqlDbType.Int,4)};
            parameters[0].Value = GoalId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.UserGoals GetModel(int GoalId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 GoalId,UserId,LowRange,MediumRange,HighRange,Month from UserGoals ");
            strSql.Append(" where GoalId=@GoalId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoalId", SqlDbType.Int,4)};
            parameters[0].Value = GoalId;

            LPWeb.Model.UserGoals model = new LPWeb.Model.UserGoals();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["GoalId"].ToString() != "")
                {
                    model.GoalId = int.Parse(ds.Tables[0].Rows[0]["GoalId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LowRange"].ToString() != "")
                {
                    model.LowRange = decimal.Parse(ds.Tables[0].Rows[0]["LowRange"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MediumRange"].ToString() != "")
                {
                    model.MediumRange = decimal.Parse(ds.Tables[0].Rows[0]["MediumRange"].ToString());
                }
                if (ds.Tables[0].Rows[0]["HighRange"].ToString() != "")
                {
                    model.HighRange = decimal.Parse(ds.Tables[0].Rows[0]["HighRange"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Month"].ToString() != "")
                {
                    model.Month = int.Parse(ds.Tables[0].Rows[0]["Month"].ToString());
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
            strSql.Append("select GoalId,UserId,LowRange,MediumRange,HighRange,Month ");
            strSql.Append(" FROM UserGoals ");
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
            strSql.Append(" GoalId,UserId,LowRange,MediumRange,HighRange,Month ");
            strSql.Append(" FROM UserGoals ");
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
            parameters[0].Value = "UserGoals";
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

