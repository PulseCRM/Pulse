using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类ProspectActivitiesBase。
    /// </summary>
    public class ProspectActivitiesBase
    {
        public ProspectActivitiesBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ProspectActivities model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ProspectActivities(");
            strSql.Append("ContactId,UserId,ActivityName,ActivityTime)");
            strSql.Append(" values (");
            strSql.Append("@ContactId,@UserId,@ActivityName,@ActivityTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ActivityName", SqlDbType.NVarChar,500),
					new SqlParameter("@ActivityTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ContactId;
            parameters[1].Value = model.UserId;
            parameters[2].Value = model.ActivityName;
            parameters[3].Value = model.ActivityTime;

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
        public void Update(LPWeb.Model.ProspectActivities model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ProspectActivities set ");
            strSql.Append("ProspectActivityId=@ProspectActivityId,");
            strSql.Append("ContactId=@ContactId,");
            strSql.Append("UserId=@UserId,");
            strSql.Append("ActivityName=@ActivityName,");
            strSql.Append("ActivityTime=@ActivityTime");
            strSql.Append(" where ProspectActivityId=@ProspectActivityId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ProspectActivityId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ActivityName", SqlDbType.NVarChar,500),
					new SqlParameter("@ActivityTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ProspectActivityId;
            parameters[1].Value = model.ContactId;
            parameters[2].Value = model.UserId;
            parameters[3].Value = model.ActivityName;
            parameters[4].Value = model.ActivityTime;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ProspectActivityId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProspectActivities ");
            strSql.Append(" where ProspectActivityId=@ProspectActivityId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ProspectActivityId", SqlDbType.Int,4)};
            parameters[0].Value = ProspectActivityId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.ProspectActivities GetModel(int ProspectActivityId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ProspectActivityId,ContactId,UserId,ActivityName,ActivityTime from ProspectActivities ");
            strSql.Append(" where ProspectActivityId=@ProspectActivityId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ProspectActivityId", SqlDbType.Int,4)};
            parameters[0].Value = ProspectActivityId;

            LPWeb.Model.ProspectActivities model = new LPWeb.Model.ProspectActivities();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ProspectActivityId"].ToString() != "")
                {
                    model.ProspectActivityId = int.Parse(ds.Tables[0].Rows[0]["ProspectActivityId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactId"].ToString() != "")
                {
                    model.ContactId = int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                model.ActivityName = ds.Tables[0].Rows[0]["ActivityName"].ToString();
                if (ds.Tables[0].Rows[0]["ActivityTime"].ToString() != "")
                {
                    model.ActivityTime = DateTime.Parse(ds.Tables[0].Rows[0]["ActivityTime"].ToString());
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
            strSql.Append("select ProspectActivityId,ContactId,UserId,ActivityName,ActivityTime ");
            strSql.Append(" FROM ProspectActivities ");
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
            strSql.Append(" ProspectActivityId,ContactId,UserId,ActivityName,ActivityTime ");
            strSql.Append(" FROM ProspectActivities ");
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
            parameters[0].Value = "ProspectActivities";
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

