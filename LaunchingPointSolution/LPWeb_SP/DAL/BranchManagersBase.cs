using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary> 
	/// </summary>
    public class BranchManagersBase
    {
        public BranchManagersBase()
		{
        }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.BranchManagers model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BranchManagers(");
            strSql.Append("BranchId,BranchMgrId)");
            strSql.Append(" values (");
            strSql.Append("@BranchId,@BranchMgrId)");
            SqlParameter[] parameters = {
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					new SqlParameter("@BranchMgrId", SqlDbType.Int,4)};
            parameters[0].Value = model.BranchId;
            parameters[1].Value = model.BranchMgrId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.BranchManagers model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BranchManagers set ");
            strSql.Append("BranchId=@BranchId,");
            strSql.Append("BranchMgrId=@BranchMgrId");
            strSql.Append(" where BranchId=@BranchId and BranchMgrId=@BranchMgrId ");
            SqlParameter[] parameters = {
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					new SqlParameter("@BranchMgrId", SqlDbType.Int,4)};
            parameters[0].Value = model.BranchId;
            parameters[1].Value = model.BranchMgrId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int BranchId, int BranchMgrId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from BranchManagers ");
            strSql.Append(" where BranchId=@BranchId and BranchMgrId=@BranchMgrId ");
            SqlParameter[] parameters = {
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					new SqlParameter("@BranchMgrId", SqlDbType.Int,4)};
            parameters[0].Value = BranchId;
            parameters[1].Value = BranchMgrId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.BranchManagers GetModel(int BranchId, int BranchMgrId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 BranchId,BranchMgrId from BranchManagers ");
            strSql.Append(" where BranchId=@BranchId and BranchMgrId=@BranchMgrId ");
            SqlParameter[] parameters = {
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					new SqlParameter("@BranchMgrId", SqlDbType.Int,4)};
            parameters[0].Value = BranchId;
            parameters[1].Value = BranchMgrId;

            LPWeb.Model.BranchManagers model = new LPWeb.Model.BranchManagers();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["BranchId"].ToString() != "")
                {
                    model.BranchId = int.Parse(ds.Tables[0].Rows[0]["BranchId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BranchMgrId"].ToString() != "")
                {
                    model.BranchMgrId = int.Parse(ds.Tables[0].Rows[0]["BranchMgrId"].ToString());
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
            strSql.Append("select BranchId,BranchMgrId ");
            strSql.Append(" FROM BranchManagers ");
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
            strSql.Append(" BranchId,BranchMgrId ");
            strSql.Append(" FROM BranchManagers ");
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
            parameters[0].Value = "BranchManagers";
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

