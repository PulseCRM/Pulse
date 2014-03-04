using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:ProspectTasks
    /// </summary>
    public class ProspectTasksBase
    {
        public ProspectTasksBase()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ProspectTaskId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ProspectTasks");
            strSql.Append(" where ProspectTaskId=@ProspectTaskId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ProspectTaskId", SqlDbType.Int,4)};
            parameters[0].Value = ProspectTaskId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ProspectTasks model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ProspectTasks(");
            strSql.Append("ContactId,TaskName,[Desc],OwnerId,[Due],WarningEmailTemplId,OverdueEmailTemplId,CompletionEmailTemplid,Completed,CompletedBy,DaysFromCreation,Enabled)");
            strSql.Append(" values (");
            strSql.Append("@ContactId,@TaskName,@Desc,@OwnerId,@Due,@WarningEmailTemplId,@OverdueEmailTemplId,@CompletionEmailTemplid,@Completed,@CompletedBy,@DaysFromCreation,@Enabled)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@TaskName", SqlDbType.NVarChar,255),
					new SqlParameter("@Desc", SqlDbType.NVarChar,255),
					new SqlParameter("@OwnerId", SqlDbType.Int,4),
					new SqlParameter("@Due", SqlDbType.DateTime),
					new SqlParameter("@WarningEmailTemplId", SqlDbType.Int,4),
					new SqlParameter("@OverdueEmailTemplId", SqlDbType.Int,4),
					new SqlParameter("@CompletionEmailTemplid", SqlDbType.Int,4),
					new SqlParameter("@Completed", SqlDbType.DateTime),
					new SqlParameter("@CompletedBy", SqlDbType.Int,4),
					new SqlParameter("@DaysFromCreation", SqlDbType.SmallInt,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.ContactId;
            parameters[1].Value = model.TaskName;
            parameters[2].Value = model.Desc;
            parameters[3].Value = model.OwnerId;
            parameters[4].Value = model.Due;
            parameters[5].Value = model.WarningEmailTemplId;
            parameters[6].Value = model.OverdueEmailTemplId;
            parameters[7].Value = model.CompletionEmailTemplid;
            parameters[8].Value = model.Completed;
            parameters[9].Value = model.CompletedBy;
            parameters[10].Value = model.DaysFromCreation;
            parameters[11].Value = model.Enabled;

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
        public bool Update(LPWeb.Model.ProspectTasks model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ProspectTasks set ");
            strSql.Append("ContactId=@ContactId,");
            strSql.Append("TaskName=@TaskName,");
            strSql.Append("[Desc]=@Desc,");
            strSql.Append("OwnerId=@OwnerId,");
            strSql.Append("[Due]=@Due,");
            strSql.Append("WarningEmailTemplId=@WarningEmailTemplId,");
            strSql.Append("OverdueEmailTemplId=@OverdueEmailTemplId,");
            strSql.Append("CompletionEmailTemplid=@CompletionEmailTemplid,");
            strSql.Append("Completed=@Completed,");
            strSql.Append("CompletedBy=@CompletedBy,");
            strSql.Append("DaysFromCreation=@DaysFromCreation,");
            strSql.Append("Enabled=@Enabled");
            strSql.Append(" where ProspectTaskId=@ProspectTaskId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProspectTaskId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@TaskName", SqlDbType.NVarChar,255),
					new SqlParameter("@Desc", SqlDbType.NVarChar,255),
					new SqlParameter("@OwnerId", SqlDbType.Int,4),
					new SqlParameter("@Due", SqlDbType.DateTime),
					new SqlParameter("@WarningEmailTemplId", SqlDbType.Int,4),
					new SqlParameter("@OverdueEmailTemplId", SqlDbType.Int,4),
					new SqlParameter("@CompletionEmailTemplid", SqlDbType.Int,4),
					new SqlParameter("@Completed", SqlDbType.DateTime),
					new SqlParameter("@CompletedBy", SqlDbType.Int,4),
					new SqlParameter("@DaysFromCreation", SqlDbType.SmallInt,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.ProspectTaskId;
            parameters[1].Value = model.ContactId;
            parameters[2].Value = model.TaskName;
            parameters[3].Value = model.Desc;
            parameters[4].Value = model.OwnerId;
            parameters[5].Value = model.Due;
            parameters[6].Value = model.WarningEmailTemplId;
            parameters[7].Value = model.OverdueEmailTemplId;
            parameters[8].Value = model.CompletionEmailTemplid;
            parameters[9].Value = model.Completed;
            parameters[10].Value = model.CompletedBy;
            parameters[11].Value = model.DaysFromCreation;
            parameters[12].Value = model.Enabled;

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
        public bool Delete(int ProspectTaskId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProspectTasks ");
            strSql.Append(" where ProspectTaskId=@ProspectTaskId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProspectTaskId", SqlDbType.Int,4)
};
            parameters[0].Value = ProspectTaskId;

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
        public bool DeleteList(string ProspectTaskIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProspectTasks ");
            strSql.Append(" where ProspectTaskId in (" + ProspectTaskIdlist + ")  ");
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
        public LPWeb.Model.ProspectTasks GetModel(int ProspectTaskId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ProspectTaskId,ContactId,TaskName,[Desc],OwnerId,Due,WarningEmailTemplId,OverdueEmailTemplId,CompletionEmailTemplid,Completed,CompletedBy,DaysFromCreation,Enabled from ProspectTasks ");
            strSql.Append(" where ProspectTaskId=@ProspectTaskId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProspectTaskId", SqlDbType.Int,4)
};
            parameters[0].Value = ProspectTaskId;

            LPWeb.Model.ProspectTasks model = new LPWeb.Model.ProspectTasks();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ProspectTaskId"].ToString() != "")
                {
                    model.ProspectTaskId = int.Parse(ds.Tables[0].Rows[0]["ProspectTaskId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactId"].ToString() != "")
                {
                    model.ContactId = int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
                }
                model.TaskName = ds.Tables[0].Rows[0]["TaskName"].ToString();
                model.Desc = ds.Tables[0].Rows[0]["Desc"].ToString();
                if (ds.Tables[0].Rows[0]["OwnerId"].ToString() != "")
                {
                    model.OwnerId = int.Parse(ds.Tables[0].Rows[0]["OwnerId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Due"].ToString() != "")
                {
                    model.Due = DateTime.Parse(ds.Tables[0].Rows[0]["Due"].ToString());
                }
                if (ds.Tables[0].Rows[0]["WarningEmailTemplId"].ToString() != "")
                {
                    model.WarningEmailTemplId = int.Parse(ds.Tables[0].Rows[0]["WarningEmailTemplId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["OverdueEmailTemplId"].ToString() != "")
                {
                    model.OverdueEmailTemplId = int.Parse(ds.Tables[0].Rows[0]["OverdueEmailTemplId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CompletionEmailTemplid"].ToString() != "")
                {
                    model.CompletionEmailTemplid = int.Parse(ds.Tables[0].Rows[0]["CompletionEmailTemplid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Completed"].ToString() != "")
                {
                    model.Completed = DateTime.Parse(ds.Tables[0].Rows[0]["Completed"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CompletedBy"].ToString() != "")
                {
                    model.CompletedBy = int.Parse(ds.Tables[0].Rows[0]["CompletedBy"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DaysFromCreation"].ToString() != "")
                {
                    model.DaysFromCreation = int.Parse(ds.Tables[0].Rows[0]["DaysFromCreation"].ToString());
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
            strSql.Append("select ProspectTaskId,ContactId,TaskName,[Desc],OwnerId,Due,WarningEmailTemplId,OverdueEmailTemplId,CompletionEmailTemplid,Completed,CompletedBy,DaysFromCreation,Enabled ");
            strSql.Append(" FROM ProspectTasks ");
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
            strSql.Append(" ProspectTaskId,ContactId,TaskName,[Desc],OwnerId,Due,WarningEmailTemplId,OverdueEmailTemplId,CompletionEmailTemplid,Completed,CompletedBy,DaysFromCreation,Enabled ");
            strSql.Append(" FROM ProspectTasks ");
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
            parameters[0].Value = "ProspectTasks";
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

