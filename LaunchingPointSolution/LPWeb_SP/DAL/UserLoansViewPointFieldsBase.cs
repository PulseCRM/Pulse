using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:UserLoansViewPointFields
    /// </summary>
    public class UserLoansViewPointFieldsBase
    {
        public UserLoansViewPointFieldsBase()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UserId, int PointFieldId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from UserLoansViewPointFields");
            strSql.Append(" where UserId=@UserId and PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@PointFieldId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;
            parameters[1].Value = PointFieldId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.UserLoansViewPointFields model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UserLoansViewPointFields(");
            strSql.Append("UserId,PointFieldId)");
            strSql.Append(" values (");
            strSql.Append("@UserId,@PointFieldId)");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@PointFieldId", SqlDbType.Int,4)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.PointFieldId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.UserLoansViewPointFields model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserLoansViewPointFields set ");
#warning 系统发现缺少更新的字段，请手工确认如此更新是否正确！
            strSql.Append("UserId=@UserId,");
            strSql.Append("PointFieldId=@PointFieldId");
            strSql.Append(" where UserId=@UserId and PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@PointFieldId", SqlDbType.Int,4)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.PointFieldId;

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
        public bool Delete(int UserId, int PointFieldId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UserLoansViewPointFields ");
            strSql.Append(" where UserId=@UserId and PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@PointFieldId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;
            parameters[1].Value = PointFieldId;

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
        public LPWeb.Model.UserLoansViewPointFields GetModel(int UserId, int PointFieldId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserId,PointFieldId from UserLoansViewPointFields ");
            strSql.Append(" where UserId=@UserId and PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@PointFieldId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;
            parameters[1].Value = PointFieldId;

            LPWeb.Model.UserLoansViewPointFields model = new LPWeb.Model.UserLoansViewPointFields();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PointFieldId"].ToString() != "")
                {
                    model.PointFieldId = int.Parse(ds.Tables[0].Rows[0]["PointFieldId"].ToString());
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
            strSql.Append("select UserId,PointFieldId ");
            strSql.Append(" FROM UserLoansViewPointFields ");
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
            strSql.Append(" UserId,PointFieldId ");
            strSql.Append(" FROM UserLoansViewPointFields ");
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
            parameters[0].Value = "UserLoansViewPointFields";
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

