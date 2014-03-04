using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    public class Company_MCTBase
    {
        public Company_MCTBase()
        { }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(LPWeb.Model.Company_MCT model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_MCT(");
            strSql.Append("ClientID,PostURL,PostDataEnabled,ActiveLoanInterval,ArchivedLoanInterval,ArchivedLoanDisposeMonth,ArchivedLoanStatuses)");
            strSql.Append(" values (");
            strSql.Append("@ClientID,@PostURL,@PostDataEnabled,@ActiveLoanInterval,@ArchivedLoanInterval,@ArchivedLoanDisposeMonth,@ArchivedLoanStatuses)");
            SqlParameter[] parameters = {
					new SqlParameter("@ClientID", SqlDbType.NVarChar,255),
					new SqlParameter("@PostURL", SqlDbType.NVarChar,255),
					new SqlParameter("@PostDataEnabled", SqlDbType.Bit,1),
					new SqlParameter("@ActiveLoanInterval", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanInterval", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanDisposeMonth", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanStatuses", SqlDbType.NVarChar,255)};
            parameters[0].Value = model.ClientID;
            parameters[1].Value = model.PostURL;
            parameters[2].Value = model.PostDataEnabled;
            parameters[3].Value = model.ActiveLoanInterval;
            parameters[4].Value = model.ArchivedLoanInterval;
            parameters[5].Value = model.ArchivedLoanDisposeMonth;
            parameters[6].Value = model.ArchivedLoanStatuses;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Company_MCT model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Company_MCT set ");
            strSql.Append("ClientID=@ClientID,");
            strSql.Append("PostURL=@PostURL,");
            strSql.Append("PostDataEnabled=@PostDataEnabled,");
            strSql.Append("ActiveLoanInterval=@ActiveLoanInterval,");
            strSql.Append("ArchivedLoanInterval=@ArchivedLoanInterval,");
            strSql.Append("ArchivedLoanDisposeMonth=@ArchivedLoanDisposeMonth,");
            strSql.Append("ArchivedLoanStatuses=@ArchivedLoanStatuses");
            strSql.Append(" where ClientID=@ClientID and PostURL=@PostURL and PostDataEnabled=@PostDataEnabled and ActiveLoanInterval=@ActiveLoanInterval and ArchivedLoanInterval=@ArchivedLoanInterval and ArchivedLoanDisposeMonth=@ArchivedLoanDisposeMonth and ArchivedLoanStatuses=@ArchivedLoanStatuses ");
            SqlParameter[] parameters = {
					new SqlParameter("@ClientID", SqlDbType.NVarChar,255),
					new SqlParameter("@PostURL", SqlDbType.NVarChar,255),
					new SqlParameter("@PostDataEnabled", SqlDbType.Bit,1),
					new SqlParameter("@ActiveLoanInterval", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanInterval", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanDisposeMonth", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanStatuses", SqlDbType.NVarChar,255)};
            parameters[0].Value = model.ClientID;
            parameters[1].Value = model.PostURL;
            parameters[2].Value = model.PostDataEnabled;
            parameters[3].Value = model.ActiveLoanInterval;
            parameters[4].Value = model.ArchivedLoanInterval;
            parameters[5].Value = model.ArchivedLoanDisposeMonth;
            parameters[6].Value = model.ArchivedLoanStatuses;

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
        public bool Delete(string ClientID, string PostURL, bool PostDataEnabled, int ActiveLoanInterval, int ArchivedLoanInterval, int ArchivedLoanDisposeMonth, string ArchivedLoanStatuses)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Company_MCT ");
            strSql.Append(" where ClientID=@ClientID and PostURL=@PostURL and PostDataEnabled=@PostDataEnabled and ActiveLoanInterval=@ActiveLoanInterval and ArchivedLoanInterval=@ArchivedLoanInterval and ArchivedLoanDisposeMonth=@ArchivedLoanDisposeMonth and ArchivedLoanStatuses=@ArchivedLoanStatuses ");
            SqlParameter[] parameters = {
					new SqlParameter("@ClientID", SqlDbType.NVarChar,255),
					new SqlParameter("@PostURL", SqlDbType.NVarChar,255),
					new SqlParameter("@PostDataEnabled", SqlDbType.Bit,1),
					new SqlParameter("@ActiveLoanInterval", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanInterval", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanDisposeMonth", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanStatuses", SqlDbType.NVarChar,255)			};
            parameters[0].Value = ClientID;
            parameters[1].Value = PostURL;
            parameters[2].Value = PostDataEnabled;
            parameters[3].Value = ActiveLoanInterval;
            parameters[4].Value = ArchivedLoanInterval;
            parameters[5].Value = ArchivedLoanDisposeMonth;
            parameters[6].Value = ArchivedLoanStatuses;

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
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_MCT GetModel(string ClientID, string PostURL, bool PostDataEnabled, int ActiveLoanInterval, int ArchivedLoanInterval, int ArchivedLoanDisposeMonth, string ArchivedLoanStatuses)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ClientID,PostURL,PostDataEnabled,ActiveLoanInterval,ArchivedLoanInterval,ArchivedLoanDisposeMonth,ArchivedLoanStatuses from Company_MCT ");
            strSql.Append(" where ClientID=@ClientID and PostURL=@PostURL and PostDataEnabled=@PostDataEnabled and ActiveLoanInterval=@ActiveLoanInterval and ArchivedLoanInterval=@ArchivedLoanInterval and ArchivedLoanDisposeMonth=@ArchivedLoanDisposeMonth and ArchivedLoanStatuses=@ArchivedLoanStatuses ");
            SqlParameter[] parameters = {
					new SqlParameter("@ClientID", SqlDbType.NVarChar,255),
					new SqlParameter("@PostURL", SqlDbType.NVarChar,255),
					new SqlParameter("@PostDataEnabled", SqlDbType.Bit,1),
					new SqlParameter("@ActiveLoanInterval", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanInterval", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanDisposeMonth", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanStatuses", SqlDbType.NVarChar,255)			};
            parameters[0].Value = ClientID;
            parameters[1].Value = PostURL;
            parameters[2].Value = PostDataEnabled;
            parameters[3].Value = ActiveLoanInterval;
            parameters[4].Value = ArchivedLoanInterval;
            parameters[5].Value = ArchivedLoanDisposeMonth;
            parameters[6].Value = ArchivedLoanStatuses;

            LPWeb.Model.Company_MCT model = new LPWeb.Model.Company_MCT();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ClientID"] != null && ds.Tables[0].Rows[0]["ClientID"].ToString() != "")
                {
                    model.ClientID = ds.Tables[0].Rows[0]["ClientID"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PostURL"] != null && ds.Tables[0].Rows[0]["PostURL"].ToString() != "")
                {
                    model.PostURL = ds.Tables[0].Rows[0]["PostURL"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PostDataEnabled"] != null && ds.Tables[0].Rows[0]["PostDataEnabled"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["PostDataEnabled"].ToString() == "1") || (ds.Tables[0].Rows[0]["PostDataEnabled"].ToString().ToLower() == "true"))
                    {
                        model.PostDataEnabled = true;
                    }
                    else
                    {
                        model.PostDataEnabled = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ActiveLoanInterval"] != null && ds.Tables[0].Rows[0]["ActiveLoanInterval"].ToString() != "")
                {
                    model.ActiveLoanInterval = int.Parse(ds.Tables[0].Rows[0]["ActiveLoanInterval"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ArchivedLoanInterval"] != null && ds.Tables[0].Rows[0]["ArchivedLoanInterval"].ToString() != "")
                {
                    model.ArchivedLoanInterval = int.Parse(ds.Tables[0].Rows[0]["ArchivedLoanInterval"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ArchivedLoanDisposeMonth"] != null && ds.Tables[0].Rows[0]["ArchivedLoanDisposeMonth"].ToString() != "")
                {
                    model.ArchivedLoanDisposeMonth = int.Parse(ds.Tables[0].Rows[0]["ArchivedLoanDisposeMonth"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ArchivedLoanStatuses"] != null && ds.Tables[0].Rows[0]["ArchivedLoanStatuses"].ToString() != "")
                {
                    model.ArchivedLoanStatuses = ds.Tables[0].Rows[0]["ArchivedLoanStatuses"].ToString();
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
            strSql.Append("select ClientID,PostURL,PostDataEnabled,ActiveLoanInterval,ArchivedLoanInterval,ArchivedLoanDisposeMonth,ArchivedLoanStatuses ");
            strSql.Append(" FROM Company_MCT ");
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
            strSql.Append(" ClientID,PostURL,PostDataEnabled,ActiveLoanInterval,ArchivedLoanInterval,ArchivedLoanDisposeMonth,ArchivedLoanStatuses ");
            strSql.Append(" FROM Company_MCT ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Company_MCT ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.ArchivedLoanStatuses desc");
            }
            strSql.Append(")AS Row, T.*  from Company_MCT T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }
    }
}
