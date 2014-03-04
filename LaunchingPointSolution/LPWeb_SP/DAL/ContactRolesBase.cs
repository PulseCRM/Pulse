using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类ContactRoles。
	/// </summary>
    public class ContactRolesBase
    {
        public ContactRolesBase()
		{}
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ContactRoles model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ContactRoles(");
            strSql.Append("Name,Enabled)");
            strSql.Append(" values (");
            strSql.Append("@Name,@Enabled)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.Name;

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
        public void Update(LPWeb.Model.ContactRoles model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ContactRoles set ");
            strSql.Append("ContactRoleId=@ContactRoleId,");
            strSql.Append("Name=@Name");
            strSql.Append("Enabled=@Enabled");
            strSql.Append(" where ContactRoleId=@ContactRoleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactRoleId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.ContactRoleId;
            parameters[1].Value = model.Name;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ContactRoleId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ContactRoles ");
            strSql.Append(" where ContactRoleId=@ContactRoleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactRoleId", SqlDbType.Int,4)};
            parameters[0].Value = ContactRoleId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.ContactRoles GetModel(int ContactRoleId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ContactRoleId,Name from ContactRoles ");
            strSql.Append(" where ContactRoleId=@ContactRoleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactRoleId", SqlDbType.Int,4)};
            parameters[0].Value = ContactRoleId;

            LPWeb.Model.ContactRoles model = new LPWeb.Model.ContactRoles();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ContactRoleId"].ToString() != "")
                {
                    model.ContactRoleId = int.Parse(ds.Tables[0].Rows[0]["ContactRoleId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
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
            strSql.Append("select ContactRoleId,Name,Enabled ");
            strSql.Append(" FROM ContactRoles ");
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
            strSql.Append(" ContactRoleId,Name,Enabled ");
            strSql.Append(" FROM ContactRoles ");
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
            parameters[0].Value = "ContactRoles";
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

