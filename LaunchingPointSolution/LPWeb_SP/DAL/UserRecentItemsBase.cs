using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:UserRecentItems
    /// </summary>
    public class UserRecentItemsBase
    {
        public UserRecentItemsBase()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UserId, DateTime LastAccessed)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from UserRecentItems");
            strSql.Append(" where UserId=@UserId and LastAccessed=@LastAccessed ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@LastAccessed", SqlDbType.DateTime)};
            parameters[0].Value = UserId;
            parameters[1].Value = LastAccessed;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.UserRecentItems model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UserRecentItems(");
            strSql.Append("UserId,LastAccessed,FileId)");
            strSql.Append(" values (");
            strSql.Append("@UserId,@LastAccessed,@FileId)");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@LastAccessed", SqlDbType.DateTime),
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.LastAccessed;
            parameters[2].Value = model.FileId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.UserRecentItems model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserRecentItems set ");
            strSql.Append("FileId=@FileId");
            strSql.Append(" where UserId=@UserId and LastAccessed=@LastAccessed ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@LastAccessed", SqlDbType.DateTime),
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.LastAccessed;
            parameters[2].Value = model.FileId;

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
        public bool Delete(int UserId, DateTime LastAccessed)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UserRecentItems ");
            strSql.Append(" where UserId=@UserId and LastAccessed=@LastAccessed ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@LastAccessed", SqlDbType.DateTime)};
            parameters[0].Value = UserId;
            parameters[1].Value = LastAccessed;

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
        public LPWeb.Model.UserRecentItems GetModel(int UserId, DateTime LastAccessed)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserId,LastAccessed,FileId from UserRecentItems ");
            strSql.Append(" where UserId=@UserId and LastAccessed=@LastAccessed ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@LastAccessed", SqlDbType.DateTime)};
            parameters[0].Value = UserId;
            parameters[1].Value = LastAccessed;

            LPWeb.Model.UserRecentItems model = new LPWeb.Model.UserRecentItems();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LastAccessed"].ToString() != "")
                {
                    model.LastAccessed = DateTime.Parse(ds.Tables[0].Rows[0]["LastAccessed"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
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
            strSql.Append("select UserId,LastAccessed,FileId ");
            strSql.Append(" FROM UserRecentItems ");
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
            strSql.Append(" UserId,LastAccessed,FileId ");
            strSql.Append(" FROM UserRecentItems ");
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
            parameters[0].Value = "UserRecentItems";
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

