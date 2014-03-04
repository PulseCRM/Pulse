using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LPWeb.DAL
{
    public class UserLicenses
    {
        public UserLicenses()
        {
        }


        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.UserLicenses model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UserLicenses(");
            strSql.Append("[UserId],[LicenseNumber])");
            strSql.Append(" values (");
            strSql.Append("@UserId,@LicenseNumber)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,1),
					new SqlParameter("@LicenseNumber", SqlDbType.NVarChar,1)};

            parameters[0].Value = model.UserId;
            parameters[1].Value = model.LicenseNumber;

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
        public void Update(LPWeb.Model.UserLicenses model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserLicenses set ");
            strSql.Append("UserId=@UserId,");
            strSql.Append("LicenseNumber=@LicenseNumber");
            strSql.Append(" where UserLicenseId=@UserLicenseId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@LicenseNumber", SqlDbType.NVarChar,255)  };

            parameters[0].Value = model.UserId;
            parameters[1].Value = model.LicenseNumber;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int UserLicenseId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UserLicenses ");
            strSql.Append(" where UserLicenseId=@UserLicenseId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserLicenseId", SqlDbType.Int,4)};
            parameters[0].Value = UserLicenseId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.UserLicenses GetModel(int UserLicenseId)
        {

            StringBuilder strSql = new StringBuilder();

            strSql.Append("select  top 1 UserLicenseId,UserId,LicenseNumber from UserLicenses ");
            strSql.Append(" where UserLicenseId=@UserLicenseId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserLicenseId", SqlDbType.Int,4)};
            parameters[0].Value = UserLicenseId;

            LPWeb.Model.UserLicenses model = new LPWeb.Model.UserLicenses();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }

                model.LicenseNumber = ds.Tables[0].Rows[0]["LicenseNumber"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["LicenseNumber"].ToString();

                model.UserLicenseId = ds.Tables[0].Rows[0]["UserLicenseId"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["UserLicenseId"]);

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

            strSql.Append("select UserLicenseId,UserId,LicenseNumber ");
            strSql.Append(" FROM UserLicenses ");
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

            strSql.Append(" UserLicenseId,UserId,LicenseNumber ");
            strSql.Append(" FROM UserLicenses ");
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
            parameters[0].Value = "UserLicenses";
            parameters[1].Value = "UserLicenseId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        #endregion  成员方法


        public void UpdatebatchByUserID(List<Model.UserLicenses> userLin)
        {
            #region 生产批量执行语句
            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

            string sSql2 = "DELETE FROM [UserLicenses]  WHERE UserId = " + userLin.FirstOrDefault().UserId;
            SqlCommand SqlCmd2 = new SqlCommand(sSql2);
            SqlCmdList.Add(SqlCmd2);

            foreach (var item in userLin)
            {
                string sSql3 = "INSERT INTO [UserLicenses] ([UserId],[LicenseNumber])VALUES(@UserId,@LicenseNumber)";
                SqlCommand SqlCmd3 = new SqlCommand(sSql3);
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd3, "@UserId", SqlDbType.Int, item.UserId);
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd3, "@LicenseNumber", SqlDbType.VarChar, item.LicenseNumber);
                SqlCmdList.Add(SqlCmd3);
            }

            #endregion

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = LPWeb.DAL.DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                foreach (SqlCommand SqlCmdItem in SqlCmdList)
                {
                    LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmdItem, SqlTrans);
                }

                SqlTrans.Commit();
            }
            catch (Exception ex)
            {
                SqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                if (SqlConn != null)
                {
                    SqlConn.Close();
                }
            }

            #endregion

        }
    }
}
