using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类GroupFolder。
    /// </summary>
    public class GroupFolderBase
    {
        public GroupFolderBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.GroupFolder model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into GroupFolder(");
            strSql.Append("GroupId,FolderId)");
            strSql.Append(" values (");
            strSql.Append("@GroupId,@FolderId)");
            SqlParameter[] parameters = {
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@FolderId", SqlDbType.Int,4)};
            parameters[0].Value = model.GroupId;
            parameters[1].Value = model.FolderId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.GroupFolder model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update GroupFolder set ");
            strSql.Append("GroupId=@GroupId,");
            strSql.Append("FolderId=@FolderId");
            strSql.Append(" where GroupId=@GroupId and FolderId=@FolderId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@FolderId", SqlDbType.Int,4)};
            parameters[0].Value = model.GroupId;
            parameters[1].Value = model.FolderId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int GroupId, int FolderId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from GroupFolder ");
            strSql.Append(" where GroupId=@GroupId and FolderId=@FolderId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@FolderId", SqlDbType.Int,4)};
            parameters[0].Value = GroupId;
            parameters[1].Value = FolderId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.GroupFolder GetModel(int GroupId, int FolderId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 GroupId,FolderId from GroupFolder ");
            strSql.Append(" where GroupId=@GroupId and FolderId=@FolderId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@FolderId", SqlDbType.Int,4)};
            parameters[0].Value = GroupId;
            parameters[1].Value = FolderId;

            LPWeb.Model.GroupFolder model = new LPWeb.Model.GroupFolder();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["GroupId"].ToString() != "")
                {
                    model.GroupId = int.Parse(ds.Tables[0].Rows[0]["GroupId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FolderId"].ToString() != "")
                {
                    model.FolderId = int.Parse(ds.Tables[0].Rows[0]["FolderId"].ToString());
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
            strSql.Append("select GroupId,FolderId ");
            strSql.Append(" FROM GroupFolder ");
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
            strSql.Append(" GroupId,FolderId ");
            strSql.Append(" FROM GroupFolder ");
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
            parameters[0].Value = "GroupFolder";
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

