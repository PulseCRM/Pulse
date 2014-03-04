using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    public class LeadTaskListBase
    {
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.LeadTaskList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LeadTaskList(");
            strSql.Append("TaskName,SequenceNumber,Enabled)");
            strSql.Append(" values (");
            strSql.Append("@TaskName,@SequenceNumber,@Enabled)");
            SqlParameter[] parameters = {
					new SqlParameter("@TaskName", SqlDbType.NVarChar,255),
					new SqlParameter("@SequenceNumber", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.TaskName;
            parameters[1].Value = model.SequenceNumber;
            parameters[2].Value = model.Enabled;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.LeadTaskList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LeadTaskList set ");
            strSql.Append("SequenceNumber=@SequenceNumber,");
            strSql.Append("Enabled=@Enabled");
            strSql.Append(" where TaskName=@TaskName ");
            SqlParameter[] parameters = {
					new SqlParameter("@TaskName", SqlDbType.NVarChar,255),
					new SqlParameter("@SequenceNumber", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.TaskName;
            parameters[1].Value = model.SequenceNumber;
            parameters[2].Value = model.Enabled;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string TaskName)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LeadTaskList ");
            strSql.Append(" where TaskName=@TaskName ");
            SqlParameter[] parameters = {
					new SqlParameter("@TaskName", SqlDbType.NVarChar,50)};
            parameters[0].Value = TaskName;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LeadTaskList GetModel(string TaskName)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 TaskName,SequenceNumber,Enabled from LeadTaskList ");
            strSql.Append(" where TaskName=@TaskName ");
            SqlParameter[] parameters = {
					new SqlParameter("@TaskName", SqlDbType.NVarChar,50)};
            parameters[0].Value = TaskName;

            LPWeb.Model.LeadTaskList model = new LPWeb.Model.LeadTaskList();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                model.TaskName = ds.Tables[0].Rows[0]["TaskName"].ToString();
                if (ds.Tables[0].Rows[0]["SequenceNumber"].ToString() != "")
                {
                    model.SequenceNumber = int.Parse(ds.Tables[0].Rows[0]["SequenceNumber"].ToString());
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
            strSql.Append("select TaskName,SequenceNumber,Enabled ");
            strSql.Append(" FROM LeadTaskList ");
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
            strSql.Append(" TaskName,SequenceNumber,Enabled ");
            strSql.Append(" FROM LeadTaskList ");
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
