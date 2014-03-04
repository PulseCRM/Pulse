using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类LoanContacts。
    /// </summary>
    public class LoanContacts : LoanContactsBase
    {
        public LoanContacts()
        { }
        public DataSet GetLoanContacts(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = "lpvw_GetLoanContacts";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet GetLoanContactsReassign(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = "lpvw_GetReassignLoanContacts";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public void Reassign(LPWeb.Model.LoanContacts oldModel, LPWeb.Model.LoanContacts model, int UserId)
        {
            SqlParameter[] parameters = {
                                new SqlParameter("@FileId", SqlDbType.Int, 4),
                                new SqlParameter("@NewContactId", SqlDbType.Int, 4),
                                new SqlParameter("@OldContactId", SqlDbType.Int, 4),
                                new SqlParameter("@ContactRoleId",SqlDbType.Int, 4),
                                new SqlParameter("@Requester",SqlDbType.Int, 4)
                                        };
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.ContactId;
            parameters[2].Value = oldModel.ContactId;
            parameters[3].Value = model.ContactRoleId;
            parameters[4].Value = UserId;
            DbHelperSQL.RunProcedure("dbo.lpsp_ReassignLoanContact", parameters);
        }

        public void DeleteLoanContacts(int FileID, string Contacts)
        {
            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();
            string[] Ids = Contacts.Split(",".ToCharArray());
            foreach (string cids in Ids)
            {
                if (cids.Split("_".ToCharArray()).Length == 2)
                {
                    string ContractID = cids.Split("_".ToCharArray())[0];
                    int ContractRoleID;
                    if (int.TryParse(cids.Split("_".ToCharArray())[1], out ContractRoleID))
                    {
                        SqlCommand SqlCmd = new SqlCommand("lpsp_RemoveLoanContacts");
                        SqlCmd.CommandType = CommandType.StoredProcedure;
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@FileId", SqlDbType.Int, FileID);
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@ContactId", SqlDbType.NVarChar, ContractID);
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@ContactRoleId", SqlDbType.Int, ContractRoleID);
                        SqlCmdList.Add(SqlCmd);
                    }
                }
            }

            #region 批量执行SQL

            if (SqlCmdList.Count < 1)
            {
                return;
            }
            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;
            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                foreach (SqlCommand xSqlCmd in SqlCmdList)
                {
                    DbHelperSQL.ExecuteNonQuery(xSqlCmd, SqlTrans);
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

        public DataSet GetLoanContactsForReassign(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = "lpvw_GetALLContacts";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet GetDistinctLoanContactsForReassign(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = "lpvw_GetDistinctALLContacts";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet GetProspectLoanContacts(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = "lpvw_GetProspectLoanContacts";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetContactLoans(int ContactID)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append(" SELECT Loans.FileId FROM LoanContacts INNER JOIN ContactRoles ");
            strSql.Append(" ON LoanContacts.ContactRoleId = ContactRoles.ContactRoleId AND ContactRoles.Name='Borrower' ");
            strSql.Append(" INNER JOIN Loans ON LoanContacts.FileId = Loans.FileId WHERE LoanContacts.ContactId = @ContactID ");

            SqlParameter[] parameters = {
					new SqlParameter("@ContactID", SqlDbType.Int)
					};
            parameters[0].Value = ContactID;

            return DbHelperSQL.Query(strSql.ToString(),parameters);
        }

        public void UpdateContactId(int iFileId, int iContactRoleId, int iNewContactId) 
        {
            string sSql = "update LoanContacts set ContactId=" + iNewContactId + " where FileId=" + iFileId + " and ContactRoleId=" + iContactRoleId;
            DbHelperSQL.ExecuteNonQuery(sSql);
        }
    }
}

