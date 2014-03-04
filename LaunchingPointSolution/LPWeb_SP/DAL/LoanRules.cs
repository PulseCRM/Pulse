using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanRules。
	/// </summary>
    public class LoanRules : LoanRulesBase
	{
		public LoanRules()
		{}

        #region neo

        /// <summary>
        /// add rule to loan
        /// neo 2011-01-14
        /// </summary>
        /// <param name="RuleIDs"></param>
        /// <param name="iLoanID"></param>
        /// <param name="iAppliedByID"></param>
        public void AddRuleToLoanBase(string[] RuleIDs, int iLoanID, int iAppliedByID)
        {
            SqlCommand[] SqlCmds = new SqlCommand[RuleIDs.Length];

            string sSql = "insert into LoanRules (Fileid, RuleGroupId, RuleId, Applied, AppliedBy, Enabled) values (@Fileid, null, @RuleId, getdate(), @AppliedBy, 1)";

            int i = 0;
            foreach (string sRuleID in RuleIDs)
            {
                int iRuleID = Convert.ToInt32(sRuleID);

                SqlCommand SqlCmd = new SqlCommand(sSql);
                DbHelperSQL.AddSqlParameter(SqlCmd, "@Fileid", SqlDbType.Int, iLoanID);
                DbHelperSQL.AddSqlParameter(SqlCmd, "@RuleId", SqlDbType.Int, iRuleID);
                DbHelperSQL.AddSqlParameter(SqlCmd, "@AppliedBy", SqlDbType.Int, iAppliedByID);

                SqlCmds.SetValue(SqlCmd, i);

                i++;
            }

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                foreach (SqlCommand SqlCmd1 in SqlCmds)
                {
                    DbHelperSQL.ExecuteNonQuery(SqlCmd1, SqlTrans);
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

        /// <summary>
        /// add rule group to loan
        /// neo 2011-01-15
        /// </summary>
        /// <param name="RuleGroupIDs"></param>
        /// <param name="iLoanID"></param>
        /// <param name="iAppliedByID"></param>
        public void AddRuleGroupToLoanBase(string[] RuleGroupIDs, int iLoanID, int iAppliedByID)
        {
            SqlCommand[] SqlCmds = new SqlCommand[RuleGroupIDs.Length];

            string sSql = "insert into LoanRules (Fileid, RuleGroupId, RuleId, Applied, AppliedBy, Enabled) values (@Fileid, @RuleGroupId, null, getdate(), @AppliedBy, 1)";

            int i = 0;
            foreach (string sRuleGroupID in RuleGroupIDs)
            {
                int iRuleGroupID = Convert.ToInt32(sRuleGroupID);

                SqlCommand SqlCmd = new SqlCommand(sSql);
                DbHelperSQL.AddSqlParameter(SqlCmd, "@Fileid", SqlDbType.Int, iLoanID);
                DbHelperSQL.AddSqlParameter(SqlCmd, "@RuleGroupId", SqlDbType.Int, iRuleGroupID);
                DbHelperSQL.AddSqlParameter(SqlCmd, "@AppliedBy", SqlDbType.Int, iAppliedByID);

                SqlCmds.SetValue(SqlCmd, i);

                i++;
            }

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                foreach (SqlCommand SqlCmd1 in SqlCmds)
                {
                    DbHelperSQL.ExecuteNonQuery(SqlCmd1, SqlTrans);
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

        /// <summary>
        /// 从Loan中移除Rule
        /// neo 2011-01-14
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <param name="sLoanRuleIDs"></param>
        public void RemoveRuleFromLoanBase(int iLoanID, string sLoanRuleIDs)
        {
            string sSql = "delete from LoanRules where FileId=" + iLoanID + " and LoanRuleId in (" + sLoanRuleIDs + ")"
                        + "update LoanAlerts set LoanRuleId = null where LoanRuleId in (" + sLoanRuleIDs + ")";
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
        }

        /// <summary>
        /// enable/disable rule in loan
        /// neo 2011-01-15
        /// </summary>
        /// <param name="sLoanRuleIDs"></param>
        /// <param name="bEnabled"></param>
        public void EnableBase(string sLoanRuleIDs, bool bEnabled)
        {
            string sSql = "update LoanRules set Enabled=@Enabled where LoanRuleId in (" + sLoanRuleIDs + ")";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);

            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        #endregion
	}
}

