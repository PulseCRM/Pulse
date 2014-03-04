using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类UserMarketingTrans。
    /// </summary>
    public class UserMarketingTransBase
    {
        public UserMarketingTransBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.UserMarketingTrans model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UserMarketingTrans(");
            strSql.Append("UserId, TransTime, Action, Amount, Balance, LoanMarketingId, Description)");
            strSql.Append(" values (");
            strSql.Append("@UserId,@TransTime,@Action,@Amount,@Balance,@LoanMarketingId,@Description)");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@TransTime", SqlDbType.DateTime),
					new SqlParameter("@Action", SqlDbType.NVarChar,30),
					new SqlParameter("@Amount", SqlDbType.Money,8),
					new SqlParameter("@Balance", SqlDbType.Money,8),
					new SqlParameter("@LoanMarketingId", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.VarBinary)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.TransTime;
            parameters[2].Value = model.Action;
            parameters[3].Value = model.Amount;
            parameters[4].Value = model.Balance;
            parameters[5].Value = model.LoanMarketingId;
            parameters[6].Value = model.Description;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.UserMarketingTrans model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserMarketingTrans set ");
            strSql.Append("UserId=@UserId,");
            strSql.Append("TransTime=@TransTime,");
            strSql.Append("Action=@Action,");
            strSql.Append("Amount=@Amount,");
            strSql.Append("Balance=@Balance,");
            strSql.Append("LoanMarketingId=@LoanMarketingId,");
            strSql.Append("Description=@Description");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@TransTime", SqlDbType.DateTime),
					new SqlParameter("@Action", SqlDbType.NVarChar,30),
					new SqlParameter("@Amount", SqlDbType.Money,8),
					new SqlParameter("@Balance", SqlDbType.Money,8),
					new SqlParameter("@LoanMarketingId", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.VarBinary)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.TransTime;
            parameters[2].Value = model.Action;
            parameters[3].Value = model.Amount;
            parameters[4].Value = model.Balance;
            parameters[5].Value = model.LoanMarketingId;
            parameters[6].Value = model.Description;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int TransId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UserMarketingTrans ");
            strSql.Append(" where TransId=@TransId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TransId", SqlDbType.Int,4)};
            parameters[0].Value = TransId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.UserMarketingTrans GetModel(int TransId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 TransId, UserId, TransTime, Action, Amount, Balance, LoanMarketingId, Description from UserMarketingTrans ");
            strSql.Append(" where TransId=@TransId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TransId", SqlDbType.Int,4)};
            parameters[0].Value = TransId;

            LPWeb.Model.UserMarketingTrans model = new LPWeb.Model.UserMarketingTrans();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["TransId"].ToString() != "")
                {
                    model.TransId = int.Parse(ds.Tables[0].Rows[0]["TransId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TransTime"].ToString() != "")
                {
                    model.TransTime = DateTime.Parse(ds.Tables[0].Rows[0]["TransTime"].ToString());
                }
                model.Action = ds.Tables[0].Rows[0]["Action"].ToString();
                if (ds.Tables[0].Rows[0]["Amount"].ToString() != "")
                {
                    model.Amount = decimal.Parse(ds.Tables[0].Rows[0]["Amount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Balance"].ToString() != "")
                {
                    model.Balance = decimal.Parse(ds.Tables[0].Rows[0]["Balance"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LoanMarketingId"].ToString() != "")
                {
                    model.LoanMarketingId = int.Parse(ds.Tables[0].Rows[0]["LoanMarketingId"].ToString());
                }
                model.Description = ds.Tables[0].Rows[0]["Description"].ToString();
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
            strSql.Append("select UserId, TransTime, Action, Amount, Balance, LoanMarketingId, Description ");
            strSql.Append(" FROM UserMarketingTrans ");
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
            strSql.Append(" UserId, TransTime, Action, Amount, Balance, LoanMarketingId, Description ");
            strSql.Append(" FROM UserMarketingTrans ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        #endregion  成员方法
    }
}
