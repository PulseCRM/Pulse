using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类UserLoanRep。
	/// </summary>
	public class UserLoanRepBase
    {
        public UserLoanRepBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.UserLoanRep model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UserLoanRep(");
            strSql.Append("BranchId,Name,UserId)");
            strSql.Append(" values (");
            strSql.Append("@BranchId,@Name,@UserId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = model.BranchId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.UserId;

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
        public void Update(LPWeb.Model.UserLoanRep model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserLoanRep set ");
            strSql.Append("NameId=@NameId,");
            strSql.Append("BranchId=@BranchId,");
            strSql.Append("Name=@Name,");
            strSql.Append("UserId=@UserId");
            strSql.Append(" where NameId=@NameId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NameId", SqlDbType.Int,4),
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = model.NameId;
            parameters[1].Value = model.BranchId;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.UserId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int NameId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UserLoanRep ");
            strSql.Append(" where NameId=@NameId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NameId", SqlDbType.Int,4)};
            parameters[0].Value = NameId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.UserLoanRep GetModel(int NameId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 NameId,BranchId,Name,UserId from UserLoanRep ");
            strSql.Append(" where NameId=@NameId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NameId", SqlDbType.Int,4)};
            parameters[0].Value = NameId;

            LPWeb.Model.UserLoanRep model = new LPWeb.Model.UserLoanRep();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["NameId"].ToString() != "")
                {
                    model.NameId = int.Parse(ds.Tables[0].Rows[0]["NameId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BranchId"].ToString() != "")
                {
                    model.BranchId = int.Parse(ds.Tables[0].Rows[0]["BranchId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
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
            strSql.Append("select NameId,BranchId,Name,UserId ");
            strSql.Append(" FROM UserLoanRep ");
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
            strSql.Append(" NameId,BranchId,Name,UserId ");
            strSql.Append(" FROM UserLoanRep ");
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
            parameters[0].Value = "UserLoanRep";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        /// <summary>
        /// 保存UserLoanRep关系
        /// </summary>
        /// <param name="nUserID"></param>
        /// <param name="strLoanRepIds"></param>
        public void SaveUserLoanRep(int nUserID, string strLoanRepIds)
        {
            DeleteLoanRepMapping(null, nUserID);
            string strSql = string.Format("UPDATE dbo.UserLoanRep SET UserId={0} WHERE NameId IN ({1})", nUserID, strLoanRepIds);
            DbHelperSQL.ExecuteNonQuery(strSql);
        }

        /// <summary>
        /// 删除用户Loan Rep Mapping
        /// </summary>
        public void DeleteLoanRepMapping(int? nLoanRepID, int nUserID)
        {
            string strSql = "";
            if (nLoanRepID.HasValue)
                strSql = string.Format("UPDATE dbo.UserLoanRep SET UserId=NULL WHERE NameId={0}", nLoanRepID);
            else
                strSql = string.Format("UPDATE dbo.UserLoanRep SET UserId=NULL WHERE UserId={0}", nUserID);
            DbHelperSQL.ExecuteNonQuery(strSql);
        }
        #endregion  成员方法
	}
}

