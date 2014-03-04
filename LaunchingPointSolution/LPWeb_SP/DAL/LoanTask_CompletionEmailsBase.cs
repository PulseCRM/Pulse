using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:LoanTask_CompletionEmails
    /// </summary>
    public partial class LoanTask_CompletionEmailsBase
    {
        public LoanTask_CompletionEmailsBase()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int TaskCompletionEmailId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from LoanTask_CompletionEmails");
            strSql.Append(" where TaskCompletionEmailId=@TaskCompletionEmailId");
            SqlParameter[] parameters = {
					new SqlParameter("@TaskCompletionEmailId", SqlDbType.Int,4)
};
            parameters[0].Value = TaskCompletionEmailId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.LoanTask_CompletionEmails model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanTask_CompletionEmails(");
            strSql.Append("LoanTaskid,TemplEmailId,[Enabled])");
            strSql.Append(" values (");
            strSql.Append("@LoanTaskid,@TemplEmailId,@Enabled)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskid", SqlDbType.Int,4),
					new SqlParameter("@TemplEmailId", SqlDbType.Int,4),
                    new SqlParameter("@Enabled",SqlDbType.Bit)};
            parameters[0].Value = model.LoanTaskid;
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
        public bool Update(LPWeb.Model.LoanTask_CompletionEmails model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LoanTask_CompletionEmails set ");
            strSql.Append("LoanTaskid=@LoanTaskid,");
            strSql.Append("TemplEmailId=@TemplEmailId,");
            strSql.Append("[Enabled]=@Enabled");
            strSql.Append(" where TaskCompletionEmailId=@TaskCompletionEmailId");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskid", SqlDbType.Int,4),
					new SqlParameter("@TemplEmailId", SqlDbType.Int,4),
					new SqlParameter("@TaskCompletionEmailId", SqlDbType.Int,4),
                    new SqlParameter("@Enabled",SqlDbType.Bit)};
            parameters[0].Value = model.LoanTaskid;
            parameters[1].Value = model.TemplEmailId;
            parameters[2].Value = model.TaskCompletionEmailId;
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
        public bool Delete(int TaskCompletionEmailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanTask_CompletionEmails ");
            strSql.Append(" where TaskCompletionEmailId=@TaskCompletionEmailId");
            SqlParameter[] parameters = {
					new SqlParameter("@TaskCompletionEmailId", SqlDbType.Int,4)
};
            parameters[0].Value = TaskCompletionEmailId;

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
        public bool DeleteList(string TaskCompletionEmailIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanTask_CompletionEmails ");
            strSql.Append(" where TaskCompletionEmailId in (" + TaskCompletionEmailIdlist + ")  ");
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
        public LPWeb.Model.LoanTask_CompletionEmails GetModel(int TaskCompletionEmailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 TaskCompletionEmailId,LoanTaskid,TemplEmailId,[Enabled] from LoanTask_CompletionEmails ");
            strSql.Append(" where TaskCompletionEmailId=@TaskCompletionEmailId");
            SqlParameter[] parameters = {
					new SqlParameter("@TaskCompletionEmailId", SqlDbType.Int,4)
};
            parameters[0].Value = TaskCompletionEmailId;

            LPWeb.Model.LoanTask_CompletionEmails model = new LPWeb.Model.LoanTask_CompletionEmails();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["TaskCompletionEmailId"] != null && ds.Tables[0].Rows[0]["TaskCompletionEmailId"].ToString() != "")
                {
                    model.TaskCompletionEmailId = int.Parse(ds.Tables[0].Rows[0]["TaskCompletionEmailId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LoanTaskid"] != null && ds.Tables[0].Rows[0]["LoanTaskid"].ToString() != "")
                {
                    model.LoanTaskid = int.Parse(ds.Tables[0].Rows[0]["LoanTaskid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TemplEmailId"] != null && ds.Tables[0].Rows[0]["TemplEmailId"].ToString() != "")
                {
                    model.TemplEmailId = int.Parse(ds.Tables[0].Rows[0]["TemplEmailId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Enabled"] != null)
                {
                    model.Enabled = Convert.ToBoolean(ds.Tables[0].Rows[0]["Enabled"]);
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
            strSql.Append("select TaskCompletionEmailId,LoanTaskid,TemplEmailId,Enabled ");
            strSql.Append(" FROM LoanTask_CompletionEmails ");
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
            strSql.Append(" TaskCompletionEmailId,LoanTaskid,TemplEmailId,Enabled ");
            strSql.Append(" FROM LoanTask_CompletionEmails ");
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
            parameters[0].Value = "LoanTask_CompletionEmails";
            parameters[1].Value = "TaskCompletionEmailId";
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

