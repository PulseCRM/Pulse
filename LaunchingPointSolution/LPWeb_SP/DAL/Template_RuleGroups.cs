using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Collections.Generic;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Template_RuleGroups。
    /// </summary>
    public class Template_RuleGroups : Template_RuleGroupsBase
    {
        public Template_RuleGroups()
        { }

        /// <summary>
        /// get rulegroup list for gridview
        /// </summary>
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
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
            parameters[0].Value = "(SELECT a.*, (SELECT COUNT(*) FROM LoanRules WHERE RuleGroupId=a.RuleGroupId) AS Referenced, CASE RuleScope WHEN 0 THEN 'Loan' WHEN 1 THEN 'Company' WHEN 2 THEN 'Region' WHEN 3 THEN 'Division' WHEN 4 THEN 'Branch' ELSE '' END AS ScopeName, CASE LoanTarget WHEN 0 THEN 'Processing' WHEN 1 THEN 'Prospect' WHEN 2 THEN 'Processing and Prospect' ELSE '' END AS TargetName FROM Template_RuleGroups a) t";
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
        /// get rule list of rule group for gridview
        /// </summary>
        public DataSet GetRuleListOfRuleGroup(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string strTableName = "(select a.RuleGroupId, b.*, c.Name as AlertEmailTpltName from Template_Group_N_Rules a inner join Template_Rules b on a.RuleId=b.RuleId left join Template_Email c on b.AlertEmailTemplId=c.TemplEmailId) t";
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

        public bool IsReferencedByLoan(int nId)
        {
            string sSql = string.Format("SELECT LoanRuleId FROM LoanRules WHERE RuleGroupId='{0}'", nId);
            return DbHelperSQL.Exists(sSql);
        }

        public bool IsRuleGroupNameExists(int nId, string strName)
        {
            strName = strName.Replace('\'', '\"');
            string sSql = "";
            if (nId > 0)
                sSql = string.Format("SELECT RuleGroupId FROM Template_RuleGroups WHERE RuleGroupId<>'{0}' AND Name='{1}'", nId, strName);
            else
                sSql = string.Format("SELECT RuleGroupId FROM Template_RuleGroups WHERE Name='{0}'", strName);
            return DbHelperSQL.Exists(sSql);
        }

        /// <summary>
        /// Add rule group info, clone a rule group when the parameter nSourceID is not null
        /// </summary>
        /// <param name="ruleGroup"></param>
        /// <param name="strRuleIds"></param>
        /// <param name="nSourceID"></param>
        public int AddRuleGroupInfo(Model.Template_RuleGroups ruleGroup, string strRuleIds, int? nSourceID)
        {
            // 
            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

            int nRuleGroupId = -1;
            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                // 1. save rule group info
                StringBuilder sbSql = new StringBuilder();
                SqlCommand cmdAddNew = new SqlCommand();
                sbSql.Append("INSERT INTO Template_RuleGroups(Name, [Desc], [Enabled],RuleScope,LoanTarget)VALUES(@Name, @Desc, @Enabled,@RuleScope,@LoanTarget);");
                sbSql.Append("SELECT @@IDENTITY");
                SqlParameter[] cmdParms = {
                    new SqlParameter("@Name", SqlDbType.NVarChar),
                    new SqlParameter("@Desc", SqlDbType.NVarChar),
                    new SqlParameter("@Enabled", SqlDbType.Bit),
					new SqlParameter("@RuleScope", SqlDbType.SmallInt,2),
					new SqlParameter("@LoanTarget", SqlDbType.SmallInt,2)
                                          };
                cmdParms[0].Value = ruleGroup.Name;
                cmdParms[1].Value = ruleGroup.Desc;
                cmdParms[2].Value = ruleGroup.Enabled;
                cmdParms[3].Value = ruleGroup.RuleScope;
                cmdParms[4].Value = ruleGroup.LoanTarget;

                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmdAddNew.Parameters.Add(parameter);
                }

                cmdAddNew.CommandText = sbSql.ToString();
                cmdAddNew.Transaction = SqlTrans;
                cmdAddNew.Connection = SqlConn;
                object obj = cmdAddNew.ExecuteScalar();
                cmdAddNew.Parameters.Clear();
                if (!(Object.Equals(obj, null)) && !(Object.Equals(obj, System.DBNull.Value)))
                {
                    if (!int.TryParse(obj.ToString(), out nRuleGroupId))
                        nRuleGroupId = -1;
                }

                // 2. save rulegroup's rule
                if (!string.IsNullOrEmpty(strRuleIds))
                {
                    SqlCommand cmdGroupRule = new SqlCommand();
                    StringBuilder sbGroupRule = new StringBuilder();
                    string[] arrGroupIDs = strRuleIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in arrGroupIDs)
                    {
                        sbGroupRule.Append(string.Format("INSERT INTO Template_Group_N_Rules(RuleGroupId, RuleId) VALUES('{0}', '{1}');", nRuleGroupId, str));
                    }
                    cmdGroupRule.CommandText = sbGroupRule.ToString();
                    SqlCmdList.Add(cmdGroupRule);
                }

                foreach (SqlCommand cmd in SqlCmdList)
                {
                    DbHelperSQL.ExecuteNonQuery(cmd, SqlTrans);
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

            return nRuleGroupId;
        }

        /// <summary>
        /// Save rule group info from rule group setup page
        /// </summary>
        /// <param name="ruleGroup"></param>
        /// <param name="strRuleIds"></param>
        public void UpdateRuleGroupInfo(Model.Template_RuleGroups ruleGroup, string strRuleIds)
        {
            //
            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;
            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                // 1.Update rule group info
                SqlCommand cmdItemUpdate = new SqlCommand();
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("UPDATE Template_RuleGroups SET Name=@Name, [Desc]=@Desc, [Enabled]=@Enabled, RuleScope=@RuleScope, LoanTarget=@LoanTarget WHERE RuleGroupId=@RuleGroupId");
                SqlParameter[] parameters = {
					new SqlParameter("@RuleGroupId", SqlDbType.Int),
					new SqlParameter("@Name", SqlDbType.NVarChar),
					new SqlParameter("@Desc", SqlDbType.NVarChar),
					new SqlParameter("@Enabled", SqlDbType.Bit),
					new SqlParameter("@RuleScope", SqlDbType.SmallInt,2),
					new SqlParameter("@LoanTarget", SqlDbType.SmallInt,2)};
                parameters[0].Value = ruleGroup.RuleGroupId;
                parameters[1].Value = ruleGroup.Name;
                parameters[2].Value = ruleGroup.Desc;
                parameters[3].Value = ruleGroup.Enabled;
                parameters[4].Value = ruleGroup.RuleScope;
                parameters[5].Value = ruleGroup.LoanTarget;

                cmdItemUpdate.CommandText = sbSql.ToString();
                foreach (SqlParameter parameter in parameters)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmdItemUpdate.Parameters.Add(parameter);
                }
                SqlCmdList.Add(cmdItemUpdate);

                // 2.update rulegroup's rule
                SqlCommand cmdGroupRules = new SqlCommand();
                StringBuilder sbGroupRules = new StringBuilder();
                sbGroupRules.Append(string.Format("DELETE Template_Group_N_Rules WHERE RuleGroupId='{0}';", ruleGroup.RuleGroupId));
                if (!string.IsNullOrEmpty(strRuleIds))
                {
                    string[] arrGroupRules = strRuleIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in arrGroupRules)
                    {
                        sbGroupRules.Append(string.Format("INSERT INTO Template_Group_N_Rules(RuleGroupId, RuleId) VALUES('{0}', '{1}');", ruleGroup.RuleGroupId, str));
                    }
                }
                cmdGroupRules.CommandText = sbGroupRules.ToString();
                SqlCmdList.Add(cmdGroupRules);

                foreach (SqlCommand cmd in SqlCmdList)
                {
                    DbHelperSQL.ExecuteNonQuery(cmd, SqlTrans);
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
        }

        public void AddRuleToRuleGroup(int nRuleGroupId, List<int> listRuleId)
        {
            StringBuilder sbAddRules = new StringBuilder();
            foreach (int n in listRuleId)
            {
                sbAddRules.AppendFormat("INSERT INTO Template_Group_N_Rules(RuleGroupId, RuleId) VALUES('{0}', '{1}');", nRuleGroupId, n);
            }
            SqlCommand cmdAddRuleRules = new SqlCommand(sbAddRules.ToString());
            DbHelperSQL.ExecuteNonQuery(cmdAddRuleRules);
        }

        /// <summary>
        /// remove rule from rule group
        /// </summary>
        /// <param name="nRuleGroupId"></param>
        /// <param name="strRuleIds"></param>
        public void RemoveRuleFromRuleGroup(int nRuleGroupId, string strRuleIds)
        {
            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;
            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                // delete rule group
                if (!string.IsNullOrEmpty(strRuleIds))
                {
                    SqlCommand cmdRemoveGroupRules = GetDeleteGroupRuleCmd(nRuleGroupId, strRuleIds);
                    SqlCmdList.Add(cmdRemoveGroupRules);
                }

                foreach (SqlCommand cmd in SqlCmdList)
                {
                    DbHelperSQL.ExecuteNonQuery(cmd, SqlTrans);
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
        }

        /// <summary>
        /// disable rule of rule group
        /// </summary>
        /// <param name="nRuleGroupId"></param>
        /// <param name="strRuleIds"></param>
        public void DisableRuleOfRuleGroup(int nRuleGroupId, string strRuleIds)
        {
            if (string.IsNullOrEmpty(strRuleIds))
                return;

            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;
            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                // 1. delete groupRule
                //SqlCommand cmdRemoveGroupRules = GetDeleteGroupRuleCmd(nRuleGroupId, strRuleIds);
                //SqlCmdList.Add(cmdRemoveGroupRules);

                // 2. disable rule template
                SqlCommand cmdDisableRules = new SqlCommand();
                StringBuilder sbDisableRules = new StringBuilder();
                string[] arrRuleIds = strRuleIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in arrRuleIds)
                {
                    sbDisableRules.AppendFormat("UPDATE Template_Rules SET Enabled=0 WHERE RuleId='{0}';", str);
                }
                cmdDisableRules.CommandText = sbDisableRules.ToString();
                SqlCmdList.Add(cmdDisableRules);

                foreach (SqlCommand cmd in SqlCmdList)
                {
                    DbHelperSQL.ExecuteNonQuery(cmd, SqlTrans);
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
        }

        /// <summary>
        /// delete rule of rule group
        /// </summary>
        /// <param name="nRuleGroupId"></param>
        /// <param name="strRuleIds"></param>
        public void DeleteRuleOfRuleGroup(int nRuleGroupId, string strRuleIds)
        {
            if (string.IsNullOrEmpty(strRuleIds))
                return;

            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();
            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;
            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                // 1. delete groupRule 
                SqlCommand cmdRemoveGroupRules = GetDeleteGroupRuleCmd(nRuleGroupId, strRuleIds);
                SqlCmdList.Add(cmdRemoveGroupRules);

                // 2. delete rule template
                SqlCommand cmdDeleteRules = new SqlCommand();
                cmdDeleteRules.CommandType = CommandType.StoredProcedure;
                cmdDeleteRules.CommandText = "lpsp_DeleteRuleTemplate";
                SqlParameter[] parameters = {
					new SqlParameter("@RuleIds", SqlDbType.NVarChar,2000)
                                        };
                strRuleIds = "'" + strRuleIds.Replace(",", "','") + "'";
                parameters[0].Value = strRuleIds;
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                            (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        cmdDeleteRules.Parameters.Add(parameter);
                    }
                }
                SqlCmdList.Add(cmdDeleteRules);

                foreach (SqlCommand cmd in SqlCmdList)
                {
                    DbHelperSQL.ExecuteNonQuery(cmd, SqlTrans);
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
        }

        /// <summary>
        /// get delete query of group rules
        /// </summary>
        /// <param name="nRuleGroupId"></param>
        /// <param name="strRuleIds"></param>
        /// <returns></returns>
        private SqlCommand GetDeleteGroupRuleCmd(int nRuleGroupId, string strRuleIds)
        {
            SqlCommand cmd = new SqlCommand();
            StringBuilder sbGroupRules = new StringBuilder();
            string[] arrGroupRules = strRuleIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in arrGroupRules)
            {
                sbGroupRules.Append(string.Format("DELETE Template_Group_N_Rules WHERE RuleGroupId='{0}' and RuleId='{1}';", nRuleGroupId, str));
            }
            cmd.CommandText = sbGroupRules.ToString();
            return cmd;
        }

        /// <summary>
        /// disable rule group
        /// </summary>
        /// <param name="listIds"></param>
        public void DisableRuleGroupInfo(List<int> listIds)
        {
            if (listIds.Count > 0)
            {
                StringBuilder sbTemp = new StringBuilder();
                foreach (int n in listIds)
                {
                    if (sbTemp.Length > 0)
                        sbTemp.Append(",");
                    sbTemp.AppendFormat("'{0}'", n);
                }
                string strSql = string.Format("UPDATE Template_RuleGroups SET [Enabled]=0 WHERE RuleGroupId IN ({0})", sbTemp.ToString());
                DbHelperSQL.ExecuteNonQuery(strSql);
            }
        }

        public void EnableRuleGroupInfo(List<int> listIds)
        {
            if (listIds.Count > 0)
            {
                StringBuilder sbTemp = new StringBuilder();
                foreach (int n in listIds)
                {
                    if (sbTemp.Length > 0)
                        sbTemp.Append(",");
                    sbTemp.AppendFormat("'{0}'", n);
                }
                string strSql = string.Format("UPDATE Template_RuleGroups SET [Enabled]=1 WHERE RuleGroupId IN ({0})", sbTemp.ToString());
                DbHelperSQL.ExecuteNonQuery(strSql);
            }
        }

        /// <summary>
        /// delete rule group
        /// </summary>
        /// <param name="listIds"></param>
        public bool DeleteRuleGroupInfo(List<int> listIds)
        {
            if (listIds.Count > 0)
            {
                StringBuilder sbIds = new StringBuilder();
                foreach (int n in listIds)
                {
                    if (sbIds.Length > 0)
                        sbIds.Append(",");
                    sbIds.AppendFormat("'{0}'", n);
                }
                Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

                SqlCommand cmdDeleteEmailQue = new SqlCommand(string.Format(@"DELETE EmailQue WHERE LoanAlertId IN 
                    (SELECT LoanAlertId FROM LoanAlerts WHERE LoanRuleId IN 
                        (SELECT LoanRuleId FROM LoanRules WHERE RuleGroupId IN ({0})))", sbIds));
                SqlCmdList.Add(cmdDeleteEmailQue);

                SqlCommand cmdDeleteLoanAlert = new SqlCommand(string.Format(@"DELETE LoanAlerts WHERE LoanRuleId IN 
                    (SELECT LoanRuleId FROM LoanRules WHERE RuleGroupId IN ({0}))", sbIds));
                SqlCmdList.Add(cmdDeleteLoanAlert);

                SqlCommand cmdDeleteLoanRule = new SqlCommand(string.Format("DELETE LoanRules WHERE RuleGroupId IN ({0})", sbIds));
                SqlCmdList.Add(cmdDeleteLoanRule);

                SqlCommand cmdDeleteGroupNRule = new SqlCommand(string.Format("DELETE Template_Group_N_Rules WHERE RuleGroupId IN ({0})", sbIds));
                SqlCmdList.Add(cmdDeleteGroupNRule);

                // delete UserHomePref table
                SqlCommand cmdDeleteRuleGroup = new SqlCommand(string.Format("DELETE Template_RuleGroups WHERE RuleGroupId IN ({0})", sbIds));
                SqlCmdList.Add(cmdDeleteRuleGroup);

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
                    return true;
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
            }
            else
                return true;
        }

        #region neo (Company Global Rules)

        /// <summary>
        /// get non-global(RuleScope=0) rule group
        /// neo 2011-03-19
        /// </summary>
        /// <returns></returns>
        public DataTable GetNonGlobalRuleGroupListBase() 
        {
            string sSql = "select * from Template_RuleGroups where [Enabled]=1 and (RuleScope is null or RuleScope=0)";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// add company global rule group
        /// neo 2011-03-19
        /// </summary>
        /// <param name="sRuleGroupIDs"></param>
        public void AddGlobalRuleGroupsBase(string sRuleGroupIDs) 
        {
            string sSql = "update Template_RuleGroups set RuleScope=1 where RuleGroupId in (" + sRuleGroupIDs + ");";
            
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
        }

        #endregion
    }
}