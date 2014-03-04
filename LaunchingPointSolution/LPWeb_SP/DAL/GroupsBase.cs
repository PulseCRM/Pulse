using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Groups。
    /// </summary>
    public class GroupsBase
    {
        public GroupsBase()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Groups model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Groups(");
            strSql.Append("GroupName,OrganizationType,Enabled)");
            strSql.Append(" values (");
            strSql.Append("@GroupName,@OrganizationType,@Enabled)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@GroupName", SqlDbType.NVarChar,50),
					new SqlParameter("@OrganizationType", SqlDbType.NVarChar,50),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.GroupName;
            parameters[1].Value = model.OrganizationType;
            parameters[2].Value = model.Enabled;

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
        public void Update(LPWeb.Model.Groups model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Groups set ");
            strSql.Append("GroupId=@GroupId,");
            strSql.Append("GroupName=@GroupName,");
            strSql.Append("OrganizationType=@OrganizationType,");
            strSql.Append("Enabled=@Enabled");
            strSql.Append(" where GroupId=@GroupId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@GroupName", SqlDbType.NVarChar,50),
					new SqlParameter("@OrganizationType", SqlDbType.NVarChar,50),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.GroupId;
            parameters[1].Value = model.GroupName;
            parameters[2].Value = model.OrganizationType;
            parameters[3].Value = model.Enabled;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int GroupId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Groups ");
            strSql.Append(" where GroupId=@GroupId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GroupId", SqlDbType.Int,4)};
            parameters[0].Value = GroupId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Groups GetModel(int GroupId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 GroupId,GroupName,OrganizationType,Enabled from Groups ");
            strSql.Append(" where GroupId=@GroupId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GroupId", SqlDbType.Int,4)};
            parameters[0].Value = GroupId;

            LPWeb.Model.Groups model = new LPWeb.Model.Groups();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["GroupId"].ToString() != "")
                {
                    model.GroupId = int.Parse(ds.Tables[0].Rows[0]["GroupId"].ToString());
                }
                model.GroupName = ds.Tables[0].Rows[0]["GroupName"].ToString();
                model.OrganizationType = ds.Tables[0].Rows[0]["OrganizationType"].ToString();
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
            strSql.Append("select GroupId,GroupName,OrganizationType,Enabled ");
            strSql.Append(" FROM Groups ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where Enabled=1 and " + strWhere);
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
            strSql.Append(" GroupId,GroupName,OrganizationType,Enabled ");
            strSql.Append(" FROM Groups ");
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
            parameters[0].Value = "Groups";
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

