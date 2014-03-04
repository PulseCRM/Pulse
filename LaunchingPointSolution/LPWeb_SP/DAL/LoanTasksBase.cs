using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanTasks。
	/// </summary>
	public class LoanTasksBase
	{
        public LoanTasksBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.LoanTasks model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanTasks(");
            strSql.Append("FileId,WflTemplId,Name,Due,Completed,CompletedBy,LastModified)");
            strSql.Append(" values (");
            strSql.Append("@FileId,@WflTemplId,@Name,@Due,@Completed,@CompletedBy,@LastModified)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@WflTemplId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Due", SqlDbType.DateTime),
					new SqlParameter("@Completed", SqlDbType.DateTime),
					new SqlParameter("@CompletedBy", SqlDbType.Int,4),
					new SqlParameter("@LastModified", SqlDbType.DateTime)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.WflTemplId;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.Due;
            parameters[4].Value = model.Completed;
            parameters[5].Value = model.CompletedBy;
            parameters[6].Value = model.LastModified;

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
        public void Update(LPWeb.Model.LoanTasks model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LoanTasks set ");
            strSql.Append("LoanTaskId=@LoanTaskId,");
            strSql.Append("FileId=@FileId,");
            strSql.Append("WflTemplId=@WflTemplId,");
            strSql.Append("Name=@Name,");
            strSql.Append("Due=@Due,");
            strSql.Append("Completed=@Completed,");
            strSql.Append("CompletedBy=@CompletedBy,");
            strSql.Append("LastModified=@LastModified");
            strSql.Append(" where LoanTaskId=@LoanTaskId ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@WflTemplId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Due", SqlDbType.DateTime),
					new SqlParameter("@Completed", SqlDbType.DateTime),
					new SqlParameter("@CompletedBy", SqlDbType.Int,4),
					new SqlParameter("@LastModified", SqlDbType.DateTime)};
            parameters[0].Value = model.LoanTaskId;
            parameters[1].Value = model.FileId;
            parameters[2].Value = model.WflTemplId;
            parameters[3].Value = model.Name;
            parameters[5].Value = model.Due;
            parameters[6].Value = model.Completed;
            parameters[7].Value = model.CompletedBy;
            parameters[8].Value = model.LastModified;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int LoanTaskId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanTasks ");
            strSql.Append(" where LoanTaskId=@LoanTaskId ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4)};
            parameters[0].Value = LoanTaskId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanTasks GetModel(int LoanTaskId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 LoanTaskId,FileId,WflTemplId,Name,Due,Completed,CompletedBy,LastModified from LoanTasks ");
            strSql.Append(" where LoanTaskId=@LoanTaskId ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4)};
            parameters[0].Value = LoanTaskId;

            LPWeb.Model.LoanTasks model = new LPWeb.Model.LoanTasks();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["LoanTaskId"].ToString() != "")
                {
                    model.LoanTaskId = int.Parse(ds.Tables[0].Rows[0]["LoanTaskId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["WflTemplId"].ToString() != "")
                {
                    model.WflTemplId = int.Parse(ds.Tables[0].Rows[0]["WflTemplId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                
                if (ds.Tables[0].Rows[0]["Due"].ToString() != "")
                {
                    model.Due = DateTime.Parse(ds.Tables[0].Rows[0]["Due"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Completed"].ToString() != "")
                {
                    model.Completed = DateTime.Parse(ds.Tables[0].Rows[0]["Completed"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CompletedBy"].ToString() != "")
                {
                    model.CompletedBy = int.Parse(ds.Tables[0].Rows[0]["CompletedBy"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LastModified"].ToString() != "")
                {
                    model.LastModified = DateTime.Parse(ds.Tables[0].Rows[0]["LastModified"].ToString());
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
            strSql.Append("select LoanTaskId,FileId,WflTemplId,Name,Due,Completed,CompletedBy,LastModified ");
            strSql.Append(" FROM LoanTasks ");
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
            strSql.Append(" LoanTaskId,FileId,WflTemplId,Name,Due,Completed,CompletedBy,LastModified ");
            strSql.Append(" FROM LoanTasks ");
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
            parameters[0].Value = "LoanTasks";
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

