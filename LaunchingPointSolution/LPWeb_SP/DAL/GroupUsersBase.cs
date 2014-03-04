using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类GroupUsers。
	/// </summary>
	public class GroupUsersBase
    {
        public GroupUsersBase()
		{}
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.GroupUsers model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into GroupUsers(");
            strSql.Append("GroupId,UserId)");
            strSql.Append(" values (");
            strSql.Append("@GroupId,@UserId)");
            SqlParameter[] parameters = {
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = model.GroupId;
            parameters[1].Value = model.UserId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.GroupUsers model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update GroupUsers set ");
            strSql.Append("GroupId=@GroupId,");
            strSql.Append("UserId=@UserId");
            strSql.Append(" where GroupId=@GroupId and UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = model.GroupId;
            parameters[1].Value = model.UserId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// delete group user info, if GroupId has value, delete single row, else delete all user's group info
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="UserId"></param>
        public void Delete(int? GroupId, int UserId)
        {
            StringBuilder strSql = new StringBuilder();
            if (GroupId.HasValue)
            {
                strSql.Append("DELETE FROM GroupUsers WHERE GroupId=@GroupId AND UserId=@UserId ");
                SqlParameter[] parameters = {
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
                parameters[0].Value = GroupId.Value;
                parameters[1].Value = UserId;
                DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            }
            else
            {
                strSql.Append("DELETE FROM GroupUsers WHERE UserId=@UserId ");
                SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4)};
                parameters[0].Value = UserId;
                DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.GroupUsers GetModel(int GroupId, int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 GroupId,UserId from GroupUsers ");
            strSql.Append(" where GroupId=@GroupId and UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = GroupId;
            parameters[1].Value = UserId;

            LPWeb.Model.GroupUsers model = new LPWeb.Model.GroupUsers();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["GroupId"].ToString() != "")
                {
                    model.GroupId = int.Parse(ds.Tables[0].Rows[0]["GroupId"].ToString());
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
            strSql.Append("select GroupId,UserId ");
            strSql.Append(" FROM GroupUsers ");
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
            strSql.Append(" GroupId,UserId ");
            strSql.Append(" FROM GroupUsers ");
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
            parameters[0].Value = "GroupUsers";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        /// <summary>
        /// 获取绑定到UserSetup页面的数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetGroupUsersForUserSetup(string strWhere)
        {
            return DbHelperSQL.Query(string.Format(@"SELECT gu.GroupID, gu.UserID, g.GroupName FROM dbo.GroupUsers gu
                INNER JOIN dbo.Groups g ON gu.GroupID=g.GroupId WHERE 1=1 {0}", strWhere.Trim().Length > 0 ? string.Format("AND {0}", strWhere) : ""));
        }

        /// <summary>
        /// 保存GroupUser关系
        /// </summary>
        /// <param name="nUserId"></param>
        /// <param name="strSelectedIds"></param>
        public void SaveGroupUser(int nUserId, string strSelectedIds)
        {
            DataTable dtCurrGroupUser = DbHelperSQL.ExecuteDataTable(string.Format("SELECT * FROM dbo.GroupUsers WHERE UserID={0}", nUserId));
            if (null != dtCurrGroupUser)
            {
                string[] arrIDs = strSelectedIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in arrIDs)
                {
                    DataRow dr = null;
                    DataRow[] drs = dtCurrGroupUser.Select(string.Format("GroupID={0} AND UserID={1}", str, nUserId));
                    if (drs.Length == 0)
                    {
                        dr = dtCurrGroupUser.NewRow();
                        dtCurrGroupUser.Rows.Add(dr);
                        dr["GroupID"] = str;
                        dr["UserID"] = nUserId;
                    }
                }
                foreach (DataRow row in dtCurrGroupUser.Rows)
                {
                    if (!IsExitsInArray(arrIDs, row["GroupID"].ToString()))
                        row.Delete();
                }
                SqlCommand cmdInsert = new SqlCommand("INSERT INTO dbo.GroupUsers(GroupID,UserID)VALUES(@GroupID, @UserID)");
                cmdInsert.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.Int, 4));
                cmdInsert.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4));
                cmdInsert.Parameters[0].SourceColumn = "GroupID";
                cmdInsert.Parameters[1].SourceColumn = "UserID";
                SqlCommand cmdDelete = new SqlCommand("DELETE dbo.GroupUsers WHERE GroupID=@GroupID AND UserID=@UserID");
                cmdDelete.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.Int, 4));
                cmdDelete.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4));
                cmdDelete.Parameters[0].SourceColumn = "GroupID";
                cmdDelete.Parameters[1].SourceColumn = "UserID";
                DbHelperSQL.UpdateDataTable(dtCurrGroupUser, cmdInsert, null, cmdDelete);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private bool IsExitsInArray(string[] arr, string strValue)
        {
            foreach (string str in arr)
            {
                if (str == strValue)
                    return true;
            }
            return false;
        }

        #endregion  成员方法
	}
}

