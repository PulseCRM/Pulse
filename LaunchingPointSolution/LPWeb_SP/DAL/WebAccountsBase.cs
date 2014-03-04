using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类WebAccounts。
	/// </summary>
	public class WebAccountsBase
    {
        public WebAccountsBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.WebAccounts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WebAccounts(");
            strSql.Append("Enabled,LastLogin,Password,PasswordQuestion,PasswordAnswer)");
            strSql.Append(" values (");
            strSql.Append("@Enabled,@LastLogin,@Password,@PasswordQuestion,@PasswordAnswer)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@LastLogin", SqlDbType.DateTime),
					new SqlParameter("@Password", SqlDbType.NVarChar,50),
					new SqlParameter("@PasswordQuestion", SqlDbType.NVarChar,250),
					new SqlParameter("@PasswordAnswer", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.Enabled;
            parameters[1].Value = model.LastLogin;
            parameters[2].Value = model.Password;
            parameters[3].Value = model.PasswordQuestion;
            parameters[4].Value = model.PasswordAnswer;

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
        public void Update(LPWeb.Model.WebAccounts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WebAccounts set ");
            strSql.Append("WebAccountId=@WebAccountId,");
            strSql.Append("Enabled=@Enabled,");
            strSql.Append("LastLogin=@LastLogin,");
            strSql.Append("Password=@Password,");
            strSql.Append("PasswordQuestion=@PasswordQuestion,");
            strSql.Append("PasswordAnswer=@PasswordAnswer");
            strSql.Append(" where WebAccountId=@WebAccountId ");
            SqlParameter[] parameters = {
					new SqlParameter("@WebAccountId", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@LastLogin", SqlDbType.DateTime),
					new SqlParameter("@Password", SqlDbType.NVarChar,50),
					new SqlParameter("@PasswordQuestion", SqlDbType.NVarChar,250),
					new SqlParameter("@PasswordAnswer", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.WebAccountId;
            parameters[1].Value = model.Enabled;
            parameters[2].Value = model.LastLogin;
            parameters[3].Value = model.Password;
            parameters[4].Value = model.PasswordQuestion;
            parameters[5].Value = model.PasswordAnswer;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int WebAccountId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WebAccounts ");
            strSql.Append(" where WebAccountId=@WebAccountId ");
            SqlParameter[] parameters = {
					new SqlParameter("@WebAccountId", SqlDbType.Int,4)};
            parameters[0].Value = WebAccountId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.WebAccounts GetModel(int WebAccountId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WebAccountId,Enabled,LastLogin,Password,PasswordQuestion,PasswordAnswer from WebAccounts ");
            strSql.Append(" where WebAccountId=@WebAccountId ");
            SqlParameter[] parameters = {
					new SqlParameter("@WebAccountId", SqlDbType.Int,4)};
            parameters[0].Value = WebAccountId;

            LPWeb.Model.WebAccounts model = new LPWeb.Model.WebAccounts();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["WebAccountId"].ToString() != "")
                {
                    model.WebAccountId = int.Parse(ds.Tables[0].Rows[0]["WebAccountId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Enabled"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Enabled"].ToString() == "1") || (ds.Tables[0].Rows[0]["Enabled"].ToString().ToLower() == "true"))
                    {
                        model.Enabled = true;
                    }
                    else
                    {
                        model.Enabled = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["LastLogin"].ToString() != "")
                {
                    model.LastLogin = DateTime.Parse(ds.Tables[0].Rows[0]["LastLogin"].ToString());
                }
                model.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                model.PasswordQuestion = ds.Tables[0].Rows[0]["PasswordQuestion"].ToString();
                model.PasswordAnswer = ds.Tables[0].Rows[0]["PasswordAnswer"].ToString();
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
            strSql.Append("select WebAccountId,Enabled,LastLogin,Password,PasswordQuestion,PasswordAnswer ");
            strSql.Append(" FROM WebAccounts ");
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
            strSql.Append(" WebAccountId,Enabled,LastLogin,Password,PasswordQuestion,PasswordAnswer ");
            strSql.Append(" FROM WebAccounts ");
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
            parameters[0].Value = "WebAccounts";
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

