using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Template_Rules。
    /// </summary>
    public class Template_Rules : Template_RulesBase
    {
        public Template_Rules()
        { }

        /// <summary>
        /// get rule list for rule selection list
        /// </summary>
        public DataSet GetListForRuleSelection(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string strTableName = "(SELECT a.*, b.Name as AlertEmailTpltName FROM Template_Rules a LEFT JOIN Template_Email b on a.AlertEmailTemplId=b.TemplEmailId) t";
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = strTableName;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "1=1 " + strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        /// <summary>
        /// get rule current selected list for rule selection page
        /// </summary>
        public DataSet GetListOfCurrSelectedRule(List<string> listCurrIds)
        {
            string strTableName = "(SELECT a.*, b.Name as AlertEmailTpltName FROM Template_Rules a LEFT JOIN Template_Email b on a.AlertEmailTemplId=b.TemplEmailId) t";
            StringBuilder sbWhere = new StringBuilder();
            foreach (string str in listCurrIds)
            {
                if (sbWhere.Length > 0)
                    sbWhere.Append(",");
                sbWhere.Append(str);
            }
            string strSql = "";
            if (sbWhere.Length > 0)
            {
                strSql = string.Format("SELECT * FROM {0} WHERE RuleId IN ({1})", strTableName, sbWhere.ToString());
                return DbHelperSQL.Query(strSql.ToString());
            }
            else
                return null;
        }

        /// <summary>
        /// Disable Rule template
        /// </summary>
        /// <param name="RuleIDs"></param>
        public void DisableRuleTemplates(string RuleIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Rules set ");
            strSql.Append("Enabled=0 ");
            strSql.Append(" where RuleId in (");
            strSql.Append(RuleIDs);
            strSql.Append(")");
            DbHelperSQL.ExecuteSql(strSql.ToString());
        }

        public void EnableRuleTemplates(string RuleIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Rules set ");
            strSql.Append("Enabled=1 ");
            strSql.Append(" where RuleId in (");
            strSql.Append(RuleIDs);
            strSql.Append(")");
            DbHelperSQL.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// Delete rule template
        /// </summary>
        /// <param name="RuleIDs"></param>
        public void DeleteRuleTemplate(string RuleIDs)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
					new SqlParameter("@RuleIds", SqlDbType.NVarChar,2000)
                                        };
            parameters[0].Value = RuleIDs;

            DbHelperSQL.RunProcedure("lpsp_DeleteRuleTemplate", parameters, out rowsAffected);
        }

        /// <summary>
        /// get rule with alert email template info
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <returns></returns>
        public DataTable GetRuleWithAlertEmailTpltInfo(int iRuleID)
        {
            string sSql = @"SELECT a.*, b.Name AS AlertEmailTpltName, b.Enabled AS EmailTpltEnabled, 
                b.[Desc] AS EmailTpltDesc, b.FromUserRoles, b.FromEmailAddress, b.Content, b.Subject
                FROM Template_Rules a LEFT JOIN Template_Email b ON a.AlertEmailTemplId=b.TemplEmailId
                WHERE a.RuleId=" + iRuleID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        #region neo

        /// <summary>
        /// insert rule and conditions
        /// neo 2011-01-12
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sDesc"></param>
        /// <param name="iAlertEmailTemplId"></param>
        /// <param name="bAckReq"></param>
        /// <param name="iRecomEmailTemplid"></param>
        /// <param name="sAdvFormula"></param>
        /// <param name="ConditionList"></param>
        /// <returns></returns>
        public int InsertRuleBase(string sName, string sDesc, int iAlertEmailTemplId, bool bAckReq, int iRecomEmailTemplid, string sAdvFormula, Int16 iRuleScope, Int16 iLoanTarget, DataTable ConditionList)
        {
            #region build sql command - Template_Rules

            string sSql1 = "INSERT INTO Template_Rules (Name,[Desc],Enabled,AlertEmailTemplId,AckReq,RecomEmailTemplid,AdvFormula,RuleScope,LoanTarget) VALUES (@Name,@Desc,@Enabled,@AlertEmailTemplId,@AckReq,@RecomEmailTemplid,@AdvFormula,@RuleScope,@LoanTarget);"
                         + "select SCOPE_IDENTITY();";
            SqlCommand SqlCmd1 = new SqlCommand(sSql1);

            #region add parameters

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Name", SqlDbType.NVarChar, sName);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Desc", SqlDbType.NVarChar, sDesc);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Enabled", SqlDbType.Bit, true);
            if (iAlertEmailTemplId == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@AlertEmailTemplId", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@AlertEmailTemplId", SqlDbType.Int, iAlertEmailTemplId);
            }

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@AckReq", SqlDbType.Bit, bAckReq);
            if (iRecomEmailTemplid == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@RecomEmailTemplid", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@RecomEmailTemplid", SqlDbType.Int, iRecomEmailTemplid);
            }

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@AdvFormula", SqlDbType.NVarChar, sAdvFormula);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@RuleScope", SqlDbType.SmallInt, iRuleScope);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@LoanTarget", SqlDbType.SmallInt, iLoanTarget);

            #endregion

            #endregion

            #region build sql command - Template_RuleConditions

            string sSql2 = "INSERT INTO Template_RuleConditions (RuleId,PointFieldId,Condition,Tolerance,ToleranceType,Sequence) VALUES (@RuleId,@PointFieldId,@Condition,@Tolerance,@ToleranceType,@Sequence)";
            SqlCommand SqlCmd2 = new SqlCommand(sSql2);

            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@RuleId", SqlDbType.Int, "RuleId");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@PointFieldId", SqlDbType.Decimal, "PointFieldId");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@Condition", SqlDbType.SmallInt, "Condition");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@Tolerance", SqlDbType.NVarChar, "Tolerance");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@ToleranceType", SqlDbType.NVarChar, "ToleranceType");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@Sequence", SqlDbType.SmallInt, "Sequence");

            #endregion

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                int iNewID = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd1, SqlTrans));

                // update new rule id to ConditionList
                foreach (DataRow ConditionRow in ConditionList.Rows)
                {
                    ConditionRow["RuleId"] = iNewID;
                }
                // update data table
                DbHelperSQL.UpdateDataTable(ConditionList, SqlCmd2, null, null, SqlTrans);

                SqlTrans.Commit();
                return iNewID;
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
        /// insert rule and conditions  ,Add: AutoCampaignId 
        /// neo 2011-01-12
        /// Modify: Alex 2011-08-01
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sDesc"></param>
        /// <param name="iAlertEmailTemplId"></param>
        /// <param name="bAckReq"></param>
        /// <param name="iRecomEmailTemplid"></param>
        /// <param name="sAdvFormula"></param>
        /// <param name="ConditionList"></param>
        /// <returns></returns>
        public int InsertRuleBase(string sName, string sDesc, int iAlertEmailTemplId, bool bAckReq, int iRecomEmailTemplid, string sAdvFormula, Int16 iRuleScope, Int16 iLoanTarget, DataTable ConditionList, int AutoCampaignId)
        {
            #region build sql command - Template_Rules

            string sSql1 = "INSERT INTO Template_Rules (Name,[Desc],Enabled,AlertEmailTemplId,AckReq,RecomEmailTemplid,AdvFormula,RuleScope,LoanTarget,AutoCampaignId) VALUES (@Name,@Desc,@Enabled,@AlertEmailTemplId,@AckReq,@RecomEmailTemplid,@AdvFormula,@RuleScope,@LoanTarget,@AutoCampaignId);"
                         + "select SCOPE_IDENTITY();";
            SqlCommand SqlCmd1 = new SqlCommand(sSql1);

            #region add parameters

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Name", SqlDbType.NVarChar, sName);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Desc", SqlDbType.NVarChar, sDesc);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Enabled", SqlDbType.Bit, true);
            if (iAlertEmailTemplId == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@AlertEmailTemplId", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@AlertEmailTemplId", SqlDbType.Int, iAlertEmailTemplId);
            }

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@AckReq", SqlDbType.Bit, bAckReq);
            if (iRecomEmailTemplid == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@RecomEmailTemplid", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@RecomEmailTemplid", SqlDbType.Int, iRecomEmailTemplid);
            }

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@AdvFormula", SqlDbType.NVarChar, sAdvFormula);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@RuleScope", SqlDbType.SmallInt, iRuleScope);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@LoanTarget", SqlDbType.SmallInt, iLoanTarget);
            if (AutoCampaignId == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@AutoCampaignId", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@AutoCampaignId", SqlDbType.Int, AutoCampaignId);
            }
            #endregion

            #endregion

            #region build sql command - Template_RuleConditions

            string sSql2 = "INSERT INTO Template_RuleConditions (RuleId,PointFieldId,Condition,Tolerance,ToleranceType,Sequence) VALUES (@RuleId,@PointFieldId,@Condition,@Tolerance,@ToleranceType,@Sequence)";
            SqlCommand SqlCmd2 = new SqlCommand(sSql2);

            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@RuleId", SqlDbType.Int, "RuleId");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@PointFieldId", SqlDbType.Decimal, "PointFieldId");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@Condition", SqlDbType.SmallInt, "Condition");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@Tolerance", SqlDbType.NVarChar, "Tolerance");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@ToleranceType", SqlDbType.NVarChar, "ToleranceType");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@Sequence", SqlDbType.SmallInt, "Sequence");

            #endregion

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                int iNewID = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd1, SqlTrans));

                // update new rule id to ConditionList
                foreach (DataRow ConditionRow in ConditionList.Rows)
                {
                    ConditionRow["RuleId"] = iNewID;
                }
                // update data table
                DbHelperSQL.UpdateDataTable(ConditionList, SqlCmd2, null, null, SqlTrans);

                SqlTrans.Commit();
                return iNewID;
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
        /// update rule and conditions
        /// neo 2011-01-12
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <param name="sName"></param>
        /// <param name="sDesc"></param>
        /// <param name="bEnabled"></param>
        /// <param name="iAlertEmailTemplId"></param>
        /// <param name="bAckReq"></param>
        /// <param name="iRecomEmailTemplid"></param>
        /// <param name="sAdvFormula"></param>
        /// <param name="ConditionList"></param>
        public void UpdateRuleBase(int iRuleID, string sName, string sDesc, bool bEnabled, int iAlertEmailTemplId, bool bAckReq, int iRecomEmailTemplid, string sAdvFormula, Int16 iRuleScope, Int16 iLoanTarget, DataTable ConditionList)
        {
            #region build sql command - Template_Rules

            string sSql1 = "UPDATE Template_Rules SET Name = @Name,[Desc] = @Desc,Enabled = @Enabled,AlertEmailTemplId = @AlertEmailTemplId,AckReq = @AckReq,RecomEmailTemplid = @RecomEmailTemplid,AdvFormula = @AdvFormula,RuleScope=@RuleScope,LoanTarget=@LoanTarget WHERE RuleId=@RuleId";
            SqlCommand SqlCmd1 = new SqlCommand(sSql1);

            #region add parameters

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Name", SqlDbType.NVarChar, sName);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Desc", SqlDbType.NVarChar, sDesc);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Enabled", SqlDbType.Bit, bEnabled);
            if (iAlertEmailTemplId == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@AlertEmailTemplId", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@AlertEmailTemplId", SqlDbType.Int, iAlertEmailTemplId);
            }

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@AckReq", SqlDbType.Bit, bAckReq);
            if (iRecomEmailTemplid == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@RecomEmailTemplid", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@RecomEmailTemplid", SqlDbType.Int, iRecomEmailTemplid);
            }

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@AdvFormula", SqlDbType.NVarChar, sAdvFormula);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@RuleScope", SqlDbType.SmallInt, iRuleScope);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@LoanTarget", SqlDbType.SmallInt, iLoanTarget);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@RuleId", SqlDbType.Int, iRuleID);

            #endregion

            #endregion

            // 先删除
            string sSql3 = "delete from Template_RuleConditions where RuleId=@RuleId";
            SqlCommand SqlCmd3 = new SqlCommand(sSql3);
            DbHelperSQL.AddSqlParameter(SqlCmd3, "@RuleId", SqlDbType.Int, iRuleID);

            #region build sql command - Template_RuleConditions

            string sSql2 = "INSERT INTO Template_RuleConditions (RuleId,PointFieldId,Condition,Tolerance,ToleranceType,Sequence) VALUES (@RuleId,@PointFieldId,@Condition,@Tolerance,@ToleranceType,@Sequence)";
            SqlCommand SqlCmd2 = new SqlCommand(sSql2);

            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@RuleId", SqlDbType.Int, "RuleId");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@PointFieldId", SqlDbType.Decimal, "PointFieldId");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@Condition", SqlDbType.SmallInt, "Condition");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@Tolerance", SqlDbType.NVarChar, "Tolerance");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@ToleranceType", SqlDbType.NVarChar, "ToleranceType");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@Sequence", SqlDbType.SmallInt, "Sequence");

            #endregion

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                DbHelperSQL.ExecuteScalar(SqlCmd1, SqlTrans);
                DbHelperSQL.ExecuteScalar(SqlCmd3, SqlTrans);

                // update data table
                DbHelperSQL.UpdateDataTable(ConditionList, SqlCmd2, null, null, SqlTrans);

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
        /// update rule and conditions, Add :AutoCampaignId
        /// neo 2011-01-12
        /// Modify: Alex 2011-08-01
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <param name="sName"></param>
        /// <param name="sDesc"></param>
        /// <param name="bEnabled"></param>
        /// <param name="iAlertEmailTemplId"></param>
        /// <param name="bAckReq"></param>
        /// <param name="iRecomEmailTemplid"></param>
        /// <param name="sAdvFormula"></param>
        /// <param name="ConditionList"></param>
        public void UpdateRuleBase(int iRuleID, string sName, string sDesc, bool bEnabled, int iAlertEmailTemplId, bool bAckReq, int iRecomEmailTemplid, string sAdvFormula, Int16 iRuleScope, Int16 iLoanTarget, DataTable ConditionList, int AutoCampaignId)
        {
            #region build sql command - Template_Rules

            string sSql1 = "UPDATE Template_Rules SET Name = @Name,[Desc] = @Desc,Enabled = @Enabled,AlertEmailTemplId = @AlertEmailTemplId,AckReq = @AckReq,RecomEmailTemplid = @RecomEmailTemplid,AdvFormula = @AdvFormula,RuleScope=@RuleScope,LoanTarget=@LoanTarget,AutoCampaignId=@AutoCampaignId WHERE RuleId=@RuleId";
            SqlCommand SqlCmd1 = new SqlCommand(sSql1);

            #region add parameters

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Name", SqlDbType.NVarChar, sName);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Desc", SqlDbType.NVarChar, sDesc);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Enabled", SqlDbType.Bit, bEnabled);
            if (iAlertEmailTemplId == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@AlertEmailTemplId", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@AlertEmailTemplId", SqlDbType.Int, iAlertEmailTemplId);
            }

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@AckReq", SqlDbType.Bit, bAckReq);
            if (iRecomEmailTemplid == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@RecomEmailTemplid", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@RecomEmailTemplid", SqlDbType.Int, iRecomEmailTemplid);
            }

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@AdvFormula", SqlDbType.NVarChar, sAdvFormula);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@RuleScope", SqlDbType.SmallInt, iRuleScope);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@LoanTarget", SqlDbType.SmallInt, iLoanTarget);

            if (AutoCampaignId == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@AutoCampaignId", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@AutoCampaignId", SqlDbType.Int, AutoCampaignId);
            }

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@RuleId", SqlDbType.Int, iRuleID);

            #endregion

            #endregion

            // 先删除
            string sSql3 = "delete from Template_RuleConditions where RuleId=@RuleId";
            SqlCommand SqlCmd3 = new SqlCommand(sSql3);
            DbHelperSQL.AddSqlParameter(SqlCmd3, "@RuleId", SqlDbType.Int, iRuleID);

            #region build sql command - Template_RuleConditions

            string sSql2 = "INSERT INTO Template_RuleConditions (RuleId,PointFieldId,Condition,Tolerance,ToleranceType,Sequence) VALUES (@RuleId,@PointFieldId,@Condition,@Tolerance,@ToleranceType,@Sequence)";
            SqlCommand SqlCmd2 = new SqlCommand(sSql2);

            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@RuleId", SqlDbType.Int, "RuleId");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@PointFieldId", SqlDbType.Decimal, "PointFieldId");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@Condition", SqlDbType.SmallInt, "Condition");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@Tolerance", SqlDbType.NVarChar, "Tolerance");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@ToleranceType", SqlDbType.NVarChar, "ToleranceType");
            DbHelperSQL.AddSqlParameter1(SqlCmd2, "@Sequence", SqlDbType.SmallInt, "Sequence");

            #endregion

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                DbHelperSQL.ExecuteScalar(SqlCmd1, SqlTrans);
                DbHelperSQL.ExecuteScalar(SqlCmd3, SqlTrans);

                // update data table
                DbHelperSQL.UpdateDataTable(ConditionList, SqlCmd2, null, null, SqlTrans);

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
        /// get condition list
        /// neo 2011-01-12
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetConditionListBase(string sWhere)
        {
            string sSql = "select * from Template_RuleConditions where 1=1 " + sWhere + " order by Sequence";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get condition list with PointFieldDesc info
        /// neo 2011-01-12
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <returns></returns>
        public DataTable GetConditionListBase(int iRuleID)
        {
            string sSql = "select * from Template_RuleConditions as a inner join PointFieldDesc as b on a.PointFieldId = b.PointFieldId where a.RuleId=" + iRuleID + " order by a.Sequence";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 检查rule name是否存在
        /// neo 2011-01-12
        /// </summary>
        /// <param name="sRuleName"></param>
        /// <returns></returns>
        public bool IsExist_CreateBase(string sRuleName)
        {
            string sSql = "select count(1) from Template_Rules where Name = @Name";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sRuleName);
            int iCount = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd));
            if (iCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查rule name是否存在
        /// neo 2011-01-12
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <param name="sRuleName"></param>
        /// <returns></returns>
        public bool IsExist_EditBase(int iRuleID, string sRuleName)
        {
            string sSql = "select count(1) from Template_Rules where Name = @Name and RuleId != " + iRuleID;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sRuleName);
            int iCount = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd));
            if (iCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// get rule info
        /// neo 2011-01-12
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <returns></returns>
        public DataTable GetRuleInfoBase(int iRuleID)
        {
            string sSql = "select * from Template_Rules where RuleId=" + iRuleID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 检查是否被引用
        /// neo 2011-01-12
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <returns></returns>
        public bool bIsRefBase(int iRuleID)
        {
            string sSql = "select count(1) from Template_Group_N_Rules where RuleId = " + iRuleID;
            int iCount = Convert.ToInt32(DbHelperSQL.ExecuteScalar(sSql));
            if (iCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// delete rule
        /// neo 2011-01-12
        /// </summary>
        /// <param name="iRuleID"></param>
        public void DeleteRuleBase(int iRuleID)
        {
            string sSql = "delete from LoanRules where RuleId=" + iRuleID + " "
                        + "delete from Template_Group_N_Rules where RuleId=" + iRuleID + " "
                        + "delete from Template_RuleConditions where RuleId=" + iRuleID + " "
                        + "delete from Template_Rules where RuleId=" + iRuleID;
            DbHelperSQL.ExecuteNonQuery(sSql);
        }

        #region Company Global Rules

        /// <summary>
        /// get non-global(RuleScope=0) rule
        /// neo 2011-03-19
        /// </summary>
        /// <returns></returns>
        public DataTable GetNonGlobalRuleListBase()
        {
            string sSql = "select * from Template_Rules where [Enabled]=1 and (RuleScope is null or RuleScope=0) order by Name";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// add company global rule
        /// neo 2011-03-19
        /// </summary>
        /// <param name="sRuleIDs"></param>
        public void AddGlobalRulesBase(string sRuleIDs)
        {
            string sSql = "update Template_Rules set RuleScope=1 where RuleId in (" + sRuleIDs + ");";

            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
        }

        #endregion

        #endregion


        /// <summary>
        /// Get Company Rules
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <returns></returns>
        public DataTable GetCompanyRules()
        {
            string sSql = " SELECT [RuleGroupId] AS [RuleId] ,'Rule Group' AS RuleType,[Name], '' AS [AlertEmailTempl],[Enabled]  "
                        + "  FROM [Template_RuleGroups] WHERE [RuleScope] = 1"
                        + " UNION"
                        + " SELECT [RuleId],'Rule',[Template_Rules].[Name],Template_Email.Name as [AlertEmailTempl],[Template_Rules].[Enabled] "
                        + "  FROM [Template_Rules] "
                        + "  INNER JOIN Template_Email"
                        + "  ON [Template_Rules].AlertEmailTemplId = Template_Email.TemplEmailId"
                        + "  WHERE [RuleScope] = 1 ";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }


        public DataSet GetCompanyRules(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
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
            parameters[0].Value = "lpvw_GetTemplateRules";
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

    }
}

