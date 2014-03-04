using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
//using Maticsoft.DBUtility;//Please add references
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:Company_EasyMortgageBase
    /// </summary>
    public class Company_EasyMortgageBase
    {
        public Company_EasyMortgageBase()
        { }
        #region  Method

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Company_EasyMortgage");
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
};

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.Company_EasyMortgage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_EasyMortgage(");
            strSql.Append("ClientID,URL,SyncIntervalHours,Enabled)");
            strSql.Append(" values (");
            strSql.Append("@ClientID,@URL,@SyncIntervalHours,@Enabled)");
            SqlParameter[] parameters = {
					new SqlParameter("@ClientID", SqlDbType.NVarChar,255),
					new SqlParameter("@URL", SqlDbType.NVarChar,255),
					new SqlParameter("@SyncIntervalHours", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.ClientID;
            parameters[1].Value = model.URL;
            parameters[2].Value = model.SyncIntervalHours;
            parameters[3].Value = model.Enabled;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Company_EasyMortgage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Company_EasyMortgage set ");
            strSql.Append("ClientID=@ClientID,");
            strSql.Append("URL=@URL,");
            strSql.Append("SyncIntervalHours=@SyncIntervalHours,");
            strSql.Append("Enabled=@Enabled");
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
					new SqlParameter("@ClientID", SqlDbType.NVarChar,255),
					new SqlParameter("@URL", SqlDbType.NVarChar,255),
					new SqlParameter("@SyncIntervalHours", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.ClientID;
            parameters[1].Value = model.URL;
            parameters[2].Value = model.SyncIntervalHours;
            parameters[3].Value = model.Enabled;

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
        public bool Delete()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Company_EasyMortgage ");
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
};

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
        public LPWeb.Model.Company_EasyMortgage GetModel()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ClientID,URL,SyncIntervalHours,Enabled from Company_EasyMortgage ");
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
};

            LPWeb.Model.Company_EasyMortgage model = new LPWeb.Model.Company_EasyMortgage();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                model.ClientID = ds.Tables[0].Rows[0]["ClientID"].ToString();
                model.URL = ds.Tables[0].Rows[0]["URL"].ToString();
                if (ds.Tables[0].Rows[0]["SyncIntervalHours"].ToString() != "")
                {
                    model.SyncIntervalHours = int.Parse(ds.Tables[0].Rows[0]["SyncIntervalHours"].ToString());
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
            strSql.Append("select ClientID,URL,SyncIntervalHours,Enabled ");
            strSql.Append(" FROM Company_EasyMortgage ");
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
            strSql.Append(" ClientID,URL,SyncIntervalHours,Enabled ");
            strSql.Append(" FROM Company_EasyMortgage ");
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
            parameters[0].Value = "Company_EasyMortgage";
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

