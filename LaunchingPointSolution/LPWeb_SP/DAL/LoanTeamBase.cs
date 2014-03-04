using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanTeam。
	/// </summary>
	public class LoanTeamBase
    {
        public LoanTeamBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.LoanTeam model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanTeam(");
            strSql.Append("FileId,RoleId,UserId)");
            strSql.Append(" values (");
            strSql.Append("@FileId,@RoleId,@UserId)");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@RoleId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.RoleId;
            parameters[2].Value = model.UserId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.LoanTeam model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LoanTeam set ");
            strSql.Append("FileId=@FileId,");
            strSql.Append("RoleId=@RoleId,");
            strSql.Append("UserId=@UserId");
            strSql.Append(" where FileId=@FileId and RoleId=@RoleId and UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@RoleId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.RoleId;
            parameters[2].Value = model.UserId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int FileId, int RoleId, int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanTeam ");
            strSql.Append(" where FileId=@FileId and RoleId=@RoleId and UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@RoleId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;
            parameters[1].Value = RoleId;
            parameters[2].Value = UserId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanTeam GetModel(int FileId, int RoleId, int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 FileId,RoleId,UserId from LoanTeam ");
            strSql.Append(" where FileId=@FileId and RoleId=@RoleId and UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@RoleId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;
            parameters[1].Value = RoleId;
            parameters[2].Value = UserId;

            LPWeb.Model.LoanTeam model = new LPWeb.Model.LoanTeam();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RoleId"].ToString() != "")
                {
                    model.RoleId = int.Parse(ds.Tables[0].Rows[0]["RoleId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
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
            strSql.Append("select FileId,RoleId,UserId ");
            strSql.Append(" FROM LoanTeam ");
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
            strSql.Append(" FileId,RoleId,UserId ");
            strSql.Append(" FROM LoanTeam ");
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
            parameters[0].Value = "LoanTeam";
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

