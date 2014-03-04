using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanContacts。
	/// </summary>
	public class LoanContactsBase
    {
        public LoanContactsBase()
		{}
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.LoanContacts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanContacts(");
            strSql.Append("FileId,ContactRoleId,ContactId)");
            strSql.Append(" values (");
            strSql.Append("@FileId,@ContactRoleId,@ContactId)");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@ContactRoleId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.ContactRoleId;
            parameters[2].Value = model.ContactId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.LoanContacts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LoanContacts set ");
            strSql.Append("FileId=@FileId,");
            strSql.Append("ContactRoleId=@ContactRoleId,");
            strSql.Append("ContactId=@ContactId");
            strSql.Append(" where FileId=@FileId and ContactRoleId=@ContactRoleId and ContactId=@ContactId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@ContactRoleId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.ContactRoleId;
            parameters[2].Value = model.ContactId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int FileId, int ContactRoleId, int ContactId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanContacts ");
            strSql.Append(" where FileId=@FileId and ContactRoleId=@ContactRoleId and ContactId=@ContactId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@ContactRoleId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;
            parameters[1].Value = ContactRoleId;
            parameters[2].Value = ContactId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        public void Delete(LPWeb.Model.LoanContacts model)
        {
            if ((model.FileId <= 0) || (model.ContactRoleId <= 0))
                return;
            string strSql = string.Format("delete from LoanContacts  where FileId={0} and ContactRoleId={1} ", model.FileId, model.ContactRoleId);
            if (model.ContactId >= 0)
                strSql += " and ContactId="+model.ContactId;

            DbHelperSQL.ExecuteSql(strSql);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanContacts GetModel(int FileId, int ContactRoleId, int ContactId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 FileId,ContactRoleId,ContactId from LoanContacts ");
            strSql.Append(" where FileId=@FileId and ContactRoleId=@ContactRoleId and ContactId=@ContactId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@ContactRoleId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;
            parameters[1].Value = ContactRoleId;
            parameters[2].Value = ContactId;

            LPWeb.Model.LoanContacts model = new LPWeb.Model.LoanContacts();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactRoleId"].ToString() != "")
                {
                    model.ContactRoleId = int.Parse(ds.Tables[0].Rows[0]["ContactRoleId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactId"].ToString() != "")
                {
                    model.ContactId = int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
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
            strSql.Append("select FileId,ContactRoleId,ContactId ");
            strSql.Append(" FROM LoanContacts ");
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
            strSql.Append(" FileId,ContactRoleId,ContactId ");
            strSql.Append(" FROM LoanContacts ");
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
            parameters[0].Value = "LoanContacts";
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

