using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:Template_Wfl_CompletionEmails
    /// </summary>
    public partial class Template_Wfl_CompletionEmailsBase
    {
        public Template_Wfl_CompletionEmailsBase()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CompletionEmailId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Template_Wfl_CompletionEmails");
            strSql.Append(" where CompletionEmailId=@CompletionEmailId");
            SqlParameter[] parameters = {
					new SqlParameter("@CompletionEmailId", SqlDbType.Int,4)
            };
            parameters[0].Value = CompletionEmailId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_Wfl_CompletionEmails model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Template_Wfl_CompletionEmails(");
            strSql.Append("TemplTaskid,TemplEmailId,Enabled)");
            strSql.Append(" values (");
            strSql.Append("@TemplTaskid,@TemplEmailId,@Enabled)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplTaskid", SqlDbType.Int,4),
					new SqlParameter("@TemplEmailId", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.TemplTaskid;
            parameters[1].Value = model.TemplEmailId;
            parameters[2].Value = model.Enabled;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
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
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Template_Wfl_CompletionEmails model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Wfl_CompletionEmails set ");
            strSql.Append("TemplTaskid=@TemplTaskid,");
            strSql.Append("TemplEmailId=@TemplEmailId,");
            strSql.Append("Enabled=@Enabled");
            strSql.Append(" where CompletionEmailId=@CompletionEmailId");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplTaskid", SqlDbType.Int,4),
					new SqlParameter("@TemplEmailId", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@CompletionEmailId", SqlDbType.Int,4)};
            parameters[0].Value = model.TemplTaskid;
            parameters[1].Value = model.TemplEmailId;
            parameters[2].Value = model.Enabled;
            parameters[3].Value = model.CompletionEmailId;

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
        public bool Delete(int CompletionEmailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_Wfl_CompletionEmails ");
            strSql.Append(" where CompletionEmailId=@CompletionEmailId");
            SqlParameter[] parameters = {
					new SqlParameter("@CompletionEmailId", SqlDbType.Int,4)
};
            parameters[0].Value = CompletionEmailId;

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
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string CompletionEmailIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_Wfl_CompletionEmails ");
            strSql.Append(" where CompletionEmailId in (" + CompletionEmailIdlist + ")  ");
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
        public LPWeb.Model.Template_Wfl_CompletionEmails GetModel(int CompletionEmailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 CompletionEmailId,TemplTaskid,TemplEmailId,Enabled from Template_Wfl_CompletionEmails ");
            strSql.Append(" where CompletionEmailId=@CompletionEmailId");
            SqlParameter[] parameters = {
					new SqlParameter("@CompletionEmailId", SqlDbType.Int,4)
};
            parameters[0].Value = CompletionEmailId;

            LPWeb.Model.Template_Wfl_CompletionEmails model = new LPWeb.Model.Template_Wfl_CompletionEmails();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CompletionEmailId"] != null && ds.Tables[0].Rows[0]["CompletionEmailId"].ToString() != "")
                {
                    model.CompletionEmailId = int.Parse(ds.Tables[0].Rows[0]["CompletionEmailId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TemplTaskid"] != null && ds.Tables[0].Rows[0]["TemplTaskid"].ToString() != "")
                {
                    model.TemplTaskid = int.Parse(ds.Tables[0].Rows[0]["TemplTaskid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TemplEmailId"] != null && ds.Tables[0].Rows[0]["TemplEmailId"].ToString() != "")
                {
                    model.TemplEmailId = int.Parse(ds.Tables[0].Rows[0]["TemplEmailId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Enabled"] != null && ds.Tables[0].Rows[0]["Enabled"].ToString() != "")
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
            strSql.Append("select CompletionEmailId,TemplTaskid,TemplEmailId,Enabled ");
            strSql.Append(" FROM Template_Wfl_CompletionEmails ");
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
            strSql.Append(" CompletionEmailId,TemplTaskid,TemplEmailId,Enabled ");
            strSql.Append(" FROM Template_Wfl_CompletionEmails ");
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
            parameters[0].Value = "Template_Wfl_CompletionEmails";
            parameters[1].Value = "CompletionEmailId";
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

