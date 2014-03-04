using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Template_Email。
    /// </summary>
    public class Template_Email : Template_EmailBase
    {
        public Template_Email()
        { }

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
            parameters[0].Value = "(select a.*,b.Name as EmailSkinName from Template_Email as a left outer join Template_EmailSkins as b on a.EmailSkinId=b.EmailSkinId) as t";
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
        /// update email template by where
        /// </summary>
        /// <param name="strWhere"></param>
        public void SetEmailTemplateDisabled(string strWhere)
        {
            if (!string.IsNullOrEmpty(strWhere))
            {
                string strSql = string.Format("UPDATE Template_Email SET Enabled=0 WHERE {0}", strWhere);
                DbHelperSQL.ExecuteNonQuery(strSql);
            }
        }

        public void SetEmailTemplateEnabled(string strWhere)
        {
            if (!string.IsNullOrEmpty(strWhere))
            {
                string strSql = string.Format("UPDATE Template_Email SET Enabled=1 WHERE {0}", strWhere);
                DbHelperSQL.ExecuteNonQuery(strSql);
            }
        }

        /// <summary>
        /// delete all user related info
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool DeleteEmailTemplateInfo(int nId)
        {
            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

            // delete email template referenced info from table Template_Email_Recipients, EmailQue, Template_Wfl_Tasks, LoanTasks
            SqlCommand cmdReferenced = new SqlCommand(@"DELETE Template_Email_Recipients WHERE TemplEmailId=@TemplEmailId;
                    DELETE Template_Wfl_CompletionEmails WHERE TemplEmailId=@TemplEmailId;
                    UPDATE EmailQue SET EmailTmplId=NULL WHERE EmailTmplId=@TemplEmailId;
                    UPDATE Template_Wfl_Tasks SET WarningEmailId=NULL WHERE WarningEmailId=@TemplEmailId;
                    UPDATE Template_Wfl_Tasks SET OverdueEmailId=NULL WHERE OverdueEmailId=@TemplEmailId;
                    UPDATE Template_Wfl_Tasks SET CompletionEmailId=NULL WHERE CompletionEmailId=@TemplEmailId;
                    UPDATE LoanTasks SET WarningEmailId=NULL WHERE WarningEmailId=@TemplEmailId;
                    UPDATE LoanTasks SET OverdueEmailId=NULL WHERE OverdueEmailId=@TemplEmailId;
                    UPDATE LoanTasks SET CompletionEmailId=NULL WHERE CompletionEmailId=@TemplEmailId;");
            DbHelperSQL.AddSqlParameter(cmdReferenced, "@TemplEmailId", SqlDbType.Int, nId);
            SqlCmdList.Add(cmdReferenced);

            // delete email template table
            SqlCommand cmdDeleteRecord = new SqlCommand("DELETE FROM Template_Email WHERE TemplEmailId=@TemplEmailId");
            DbHelperSQL.AddSqlParameter(cmdDeleteRecord, "@TemplEmailId", SqlDbType.Int, nId);
            SqlCmdList.Add(cmdDeleteRecord);

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

        #region neo

        /// <summary>
        /// get email template
        /// neo 2010-11-17
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetEmailTemplateBase(string sWhere)
        {
            string sSql = "select * from Template_Email where 1=1 " + sWhere + " order by [Name]";
            //sSql += " ORDER BY Name";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get email template info
        /// neo 2010-12-09
        /// </summary>
        /// <param name="iEmailTemplateID"></param>
        /// <returns></returns>
        public DataTable GetEmailTemplateInfoBase(int iEmailTemplateID)
        {
            string sSql = "select * from Template_Email where TemplEmailId = " + iEmailTemplateID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 检查email template是否存在
        /// neo 2010-12-07
        /// </summary>
        /// <param name="sEmailTemplateName"></param>
        /// <returns></returns>
        public bool IsExist_CreateBase(string sEmailTemplateName)
        {
            string sSql = "select count(1) from Template_Email where Name = @Name";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sEmailTemplateName);
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
        /// 检查email template是否存在
        /// neo 2010-12-09
        /// </summary>
        /// <param name="iEmailTemplateID"></param>
        /// <param name="sEmailTemplateName"></param>
        /// <returns></returns>
        public bool IsExist_EditBase(int iEmailTemplateID, string sEmailTemplateName)
        {
            string sSql = "select count(1) from Template_Email where Name = @Name and TemplEmailId != " + iEmailTemplateID;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sEmailTemplateName);
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
        /// insert email template
        /// neo 2010-11-17
        /// </summary>
        /// <param name="sEmailTemplateName"></param>
        /// <param name="sDesc"></param>
        /// <param name="iFromUserRoles"></param>
        /// <param name="sFromEmailAddress"></param>
        /// <param name="sContent"></param>
        /// <param name="sSubject"></param>
        /// <param name="sEmailList_To"></param>
        /// <param name="sUserRoleIDs_To"></param>
        /// <param name="sContactRoleIDs_To"></param>
        /// <param name="sEmailList_CC"></param>
        /// <param name="sUserRoleIDs_CC"></param>
        /// <param name="sContactRoleIDs_CC"></param>
        /// <param name="sTaskOwnerChecked_To"></param>
        /// <param name="sTaskOwnerChecked_CC"></param>
        public void InsertEmailTemplateBase(string sEmailTemplateName, string sDesc, int iFromUserRoles, string sFromEmailAddress, string sContent, string sSubject, string sEmailList_To, string sUserRoleIDs_To, string sContactRoleIDs_To, string sEmailList_CC, string sUserRoleIDs_CC, string sContactRoleIDs_CC, string sTaskOwnerChecked_To, string sTaskOwnerChecked_CC, bool chkLeadCreated, string sSenderName, int iEmailSkinID, bool Enabled)
        {
            #region build sql command - Template_Email

            string sSql = "INSERT INTO Template_Email (Enabled,Name,[Desc],FromUserRoles,FromEmailAddress,Content,Subject,SendTrigger,SenderName,EmailSkinId) VALUES (@Enabled,@Name,@Desc,@FromUserRoles,@FromEmailAddress,@Content,@Subject,@SendTrigger,@SenderName,@EmailSkinId);"
                             + "select SCOPE_IDENTITY();";

            SqlCommand SqlCmd = new SqlCommand(sSql);

            #region add parameters

            DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, Enabled);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sEmailTemplateName);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Desc", SqlDbType.NVarChar, sDesc);

            if (iFromUserRoles == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@FromUserRoles", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@FromUserRoles", SqlDbType.Int, iFromUserRoles);
            }

            if (sFromEmailAddress == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@FromEmailAddress", SqlDbType.NVarChar, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@FromEmailAddress", SqlDbType.NVarChar, sFromEmailAddress);
            }
            if (chkLeadCreated)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@SendTrigger", SqlDbType.NVarChar, "LeadCreated");
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@SendTrigger", SqlDbType.NVarChar, DBNull.Value);
            }

            DbHelperSQL.AddSqlParameter(SqlCmd, "@Content", SqlDbType.NVarChar, sContent);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Subject", SqlDbType.NVarChar, sSubject);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@SenderName", SqlDbType.NVarChar, sSenderName);

            if (iEmailSkinID == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@EmailSkinId", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@EmailSkinId", SqlDbType.Int, iEmailSkinID);
            }

            #endregion

            #endregion

            #region build sql command - Template_Email_Recipients (To)

            SqlCommand SqlCmd2 = null;
            if (sEmailList_To != string.Empty || sUserRoleIDs_To != string.Empty || sContactRoleIDs_To != string.Empty)
            {
                string sSql2 = "INSERT INTO Template_Email_Recipients (TemplEmailId,EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner) VALUES (@TemplEmailId,@EmailAddr,@UserRoles,@ContactRoles,@RecipientType,@TaskOwner)";
                SqlCmd2 = new SqlCommand(sSql2);

                if (sEmailList_To == string.Empty)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@EmailAddr", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@EmailAddr", SqlDbType.NVarChar, sEmailList_To);
                }

                if (sUserRoleIDs_To == string.Empty)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@UserRoles", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@UserRoles", SqlDbType.NVarChar, sUserRoleIDs_To);
                }

                if (sContactRoleIDs_To == string.Empty)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@ContactRoles", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@ContactRoles", SqlDbType.NVarChar, sContactRoleIDs_To);
                }

                DbHelperSQL.AddSqlParameter(SqlCmd2, "@RecipientType", SqlDbType.NVarChar, "To");
                DbHelperSQL.AddSqlParameter(SqlCmd2, "@TaskOwner", SqlDbType.Bit, false);
            }

            #region TaskOwner=true

            SqlCommand SqlCmd2x = null;
            if (sTaskOwnerChecked_To == "True")
            {
                string sSql2x = "INSERT INTO Template_Email_Recipients (TemplEmailId,EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner) VALUES (@TemplEmailId,null,null,null,'To',1)";
                SqlCmd2x = new SqlCommand(sSql2x);
            }

            #endregion

            #endregion

            #region build sql command - Template_Email_Recipients (CC)

            SqlCommand SqlCmd3 = null;
            if (sEmailList_CC != string.Empty || sUserRoleIDs_CC != string.Empty || sContactRoleIDs_CC != string.Empty)
            {
                string sSql3 = "INSERT INTO Template_Email_Recipients (TemplEmailId,EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner) VALUES (@TemplEmailId,@EmailAddr,@UserRoles,@ContactRoles,@RecipientType,@TaskOwner)";
                SqlCmd3 = new SqlCommand(sSql3);

                if (sEmailList_CC == string.Empty)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3, "@EmailAddr", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3, "@EmailAddr", SqlDbType.NVarChar, sEmailList_CC);
                }

                if (sUserRoleIDs_CC == string.Empty)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3, "@UserRoles", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3, "@UserRoles", SqlDbType.NVarChar, sUserRoleIDs_CC);
                }

                if (sContactRoleIDs_CC == string.Empty)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3, "@ContactRoles", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3, "@ContactRoles", SqlDbType.NVarChar, sContactRoleIDs_CC);
                }

                DbHelperSQL.AddSqlParameter(SqlCmd3, "@RecipientType", SqlDbType.NVarChar, "CC");
                DbHelperSQL.AddSqlParameter(SqlCmd3, "@TaskOwner", SqlDbType.Bit, false);
            }

            #region TaskOwner=true

            SqlCommand SqlCmd3x = null;
            if (sTaskOwnerChecked_CC == "True")
            {
                string sSql3x = "INSERT INTO Template_Email_Recipients (TemplEmailId,EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner) VALUES (@TemplEmailId,null,null,null,'CC',1)";
                SqlCmd3x = new SqlCommand(sSql3x);
            }

            #endregion

            #endregion

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                int iNewID = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd, SqlTrans));

                if (SqlCmd2 != null)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@TemplEmailId", SqlDbType.Int, iNewID);
                    DbHelperSQL.ExecuteNonQuery(SqlCmd2, SqlTrans);
                }

                if (SqlCmd2x != null)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2x, "@TemplEmailId", SqlDbType.Int, iNewID);
                    DbHelperSQL.ExecuteNonQuery(SqlCmd2x, SqlTrans);
                }

                if (SqlCmd3 != null)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3, "@TemplEmailId", SqlDbType.Int, iNewID);
                    DbHelperSQL.ExecuteNonQuery(SqlCmd3, SqlTrans);
                }

                if (SqlCmd3x != null)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3x, "@TemplEmailId", SqlDbType.Int, iNewID);
                    DbHelperSQL.ExecuteNonQuery(SqlCmd3x, SqlTrans);
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
        /// 获取recipient list
        /// neo 2010-12-09
        /// </summary>
        /// <param name="iEmailTemplateID"></param>
        /// <returns></returns>
        public DataTable GetRecipientListBase(int iEmailTemplateID)
        {
            string sSql = "select * from Template_Email_Recipients where TemplEmailId = " + iEmailTemplateID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// update email template
        /// neo 2010-12-09
        /// </summary>
        /// <param name="iEmailTemplateID"></param>
        /// <param name="sEmailTemplateName"></param>
        /// <param name="bEnalbed"></param>
        /// <param name="sDesc"></param>
        /// <param name="iFromUserRoles"></param>
        /// <param name="sFromEmailAddress"></param>
        /// <param name="sContent"></param>
        /// <param name="sSubject"></param>
        /// <param name="sEmailList_To"></param>
        /// <param name="sUserRoleIDs_To"></param>
        /// <param name="sContactRoleIDs_To"></param>
        /// <param name="sEmailList_CC"></param>
        /// <param name="sUserRoleIDs_CC"></param>
        /// <param name="sContactRoleIDs_CC"></param>
        /// <param name="sTaskOwnerChecked_To"></param>
        /// <param name="sTaskOwnerChecked_CC"></param>
        public void UpdateEmailTemplateBase(int iEmailTemplateID, string sEmailTemplateName, bool bEnalbed, string sDesc, int iFromUserRoles, string sFromEmailAddress, string sContent, string sSubject, string sEmailList_To, string sUserRoleIDs_To, string sContactRoleIDs_To, string sEmailList_CC, string sUserRoleIDs_CC, string sContactRoleIDs_CC, string sTaskOwnerChecked_To, string sTaskOwnerChecked_CC, bool chkLeadCreated, string sSenderName, int iEmailSkinID)
        {
            #region build sql command - Template_Email

            string sSql = "UPDATE Template_Email SET Enabled = @Enabled,Name = @Name,[Desc] = @Desc,FromUserRoles = @FromUserRoles,FromEmailAddress = @FromEmailAddress,Content = @Content,Subject = @Subject,SendTrigger=@SendTrigger,SenderName=@SenderName,EmailSkinId=@EmailSkinId WHERE TemplEmailId=@TemplEmailId";
            SqlCommand SqlCmd = new SqlCommand(sSql);

            #region add parameters

            DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnalbed);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sEmailTemplateName);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Desc", SqlDbType.NVarChar, sDesc);

            if (iFromUserRoles == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@FromUserRoles", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@FromUserRoles", SqlDbType.Int, iFromUserRoles);
            }

            if (sFromEmailAddress == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@FromEmailAddress", SqlDbType.NVarChar, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@FromEmailAddress", SqlDbType.NVarChar, sFromEmailAddress);
            }
            if (chkLeadCreated)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@SendTrigger", SqlDbType.NVarChar, "LeadCreated");
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@SendTrigger", SqlDbType.NVarChar, DBNull.Value);
            }

            DbHelperSQL.AddSqlParameter(SqlCmd, "@Content", SqlDbType.NVarChar, sContent);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Subject", SqlDbType.NVarChar, sSubject);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@TemplEmailId", SqlDbType.Int, iEmailTemplateID);

            DbHelperSQL.AddSqlParameter(SqlCmd, "@SenderName", SqlDbType.NVarChar, sSenderName);

            if (iEmailSkinID == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@EmailSkinId", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@EmailSkinId", SqlDbType.Int, iEmailSkinID);
            }

            #endregion

            #endregion

            // 先删除
            string sSql4 = "delete from Template_Email_Recipients where TemplEmailId=@TemplEmailId";
            SqlCommand SqlCmd4 = new SqlCommand(sSql4);
            DbHelperSQL.AddSqlParameter(SqlCmd4, "@TemplEmailId", SqlDbType.Int, iEmailTemplateID);

            #region build sql command - Template_Email_Recipients (To)

            SqlCommand SqlCmd2 = null;
            if (sEmailList_To != string.Empty || sUserRoleIDs_To != string.Empty || sContactRoleIDs_To != string.Empty)
            {
                string sSql2 = "INSERT INTO Template_Email_Recipients (TemplEmailId,EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner) VALUES (@TemplEmailId,@EmailAddr,@UserRoles,@ContactRoles,@RecipientType,@TaskOwner)";
                SqlCmd2 = new SqlCommand(sSql2);

                DbHelperSQL.AddSqlParameter(SqlCmd2, "@TemplEmailId", SqlDbType.Int, iEmailTemplateID);
                if (sEmailList_To == string.Empty)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@EmailAddr", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@EmailAddr", SqlDbType.NVarChar, sEmailList_To);
                }

                if (sUserRoleIDs_To == string.Empty)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@UserRoles", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@UserRoles", SqlDbType.NVarChar, sUserRoleIDs_To);
                }

                if (sContactRoleIDs_To == string.Empty)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@ContactRoles", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@ContactRoles", SqlDbType.NVarChar, sContactRoleIDs_To);
                }

                DbHelperSQL.AddSqlParameter(SqlCmd2, "@RecipientType", SqlDbType.NVarChar, "To");
                DbHelperSQL.AddSqlParameter(SqlCmd2, "@TaskOwner", SqlDbType.Bit, false);
            }

            #region TaskOwner=true

            SqlCommand SqlCmd2x = null;
            if (sTaskOwnerChecked_To == "True")
            {
                string sSql2x = "INSERT INTO Template_Email_Recipients (TemplEmailId,EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner) VALUES (" + iEmailTemplateID + ",null,null,null,'To',1)";
                SqlCmd2x = new SqlCommand(sSql2x);
            }

            #endregion

            #endregion

            #region build sql command - Template_Email_Recipients (CC)

            SqlCommand SqlCmd3 = null;
            if (sEmailList_CC != string.Empty || sUserRoleIDs_CC != string.Empty || sContactRoleIDs_CC != string.Empty)
            {
                string sSql3 = "INSERT INTO Template_Email_Recipients (TemplEmailId,EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner) VALUES (@TemplEmailId,@EmailAddr,@UserRoles,@ContactRoles,@RecipientType,@TaskOwner)";
                SqlCmd3 = new SqlCommand(sSql3);

                DbHelperSQL.AddSqlParameter(SqlCmd3, "@TemplEmailId", SqlDbType.Int, iEmailTemplateID);

                if (sEmailList_CC == string.Empty)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3, "@EmailAddr", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3, "@EmailAddr", SqlDbType.NVarChar, sEmailList_CC);
                }

                if (sUserRoleIDs_CC == string.Empty)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3, "@UserRoles", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3, "@UserRoles", SqlDbType.NVarChar, sUserRoleIDs_CC);
                }

                if (sContactRoleIDs_CC == string.Empty)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3, "@ContactRoles", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd3, "@ContactRoles", SqlDbType.NVarChar, sContactRoleIDs_CC);
                }

                DbHelperSQL.AddSqlParameter(SqlCmd3, "@RecipientType", SqlDbType.NVarChar, "CC");
                DbHelperSQL.AddSqlParameter(SqlCmd3, "@TaskOwner", SqlDbType.Bit, false);
            }

            #region TaskOwner=true

            SqlCommand SqlCmd3x = null;
            if (sTaskOwnerChecked_CC == "True")
            {
                string sSql3x = "INSERT INTO Template_Email_Recipients (TemplEmailId,EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner) VALUES (" + iEmailTemplateID + ",null,null,null,'CC',1)";
                SqlCmd3x = new SqlCommand(sSql3x);
            }

            #endregion

            #endregion

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                DbHelperSQL.ExecuteNonQuery(SqlCmd, SqlTrans);
                DbHelperSQL.ExecuteNonQuery(SqlCmd4, SqlTrans);

                if (SqlCmd2 != null)
                {
                    DbHelperSQL.ExecuteNonQuery(SqlCmd2, SqlTrans);
                }

                if (SqlCmd2x != null)
                {
                    DbHelperSQL.ExecuteNonQuery(SqlCmd2x, SqlTrans);
                }

                if (SqlCmd3 != null)
                {
                    DbHelperSQL.ExecuteNonQuery(SqlCmd3, SqlTrans);
                }

                if (SqlCmd3x != null)
                {
                    DbHelperSQL.ExecuteNonQuery(SqlCmd3x, SqlTrans);
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
        /// 检查是否被引用
        /// neo 2010-12-11
        /// </summary>
        /// <param name="iEmailTemplateID"></param>
        /// <returns></returns>
        public bool bIsRefBase(int iEmailTemplateID)
        {
            string sSql = "select ISNULL(SUM(RefCount),0) from ( "
                        + "select COUNT(1) as RefCount from LoanTasks where WarningEmailId=" + iEmailTemplateID + " or OverdueEmailId=" + iEmailTemplateID + " or CompletionEmailId=" + iEmailTemplateID + " "
                        + "union all "
                        + "select COUNT(1) as RefCount from EmailQue where EmailTmplId=" + iEmailTemplateID + " "
                        + "union all "
                        + "select COUNT(1) as RefCount from Template_Wfl_Tasks where WarningEmailId=" + iEmailTemplateID + " or OverdueEmailId=" + iEmailTemplateID + " or CompletionEmailId=" + iEmailTemplateID + " "
                        + "union all "
                        + "select COUNT(1) as RefCount from Template_Rules where AlertEmailTemplId=" + iEmailTemplateID + " or RecomEmailTemplid=" + iEmailTemplateID + " "
                        + ") as AllRefCount";
            int iCount = Convert.ToInt32(DbHelperSQL.ExecuteScalar(sSql));
            if (iCount == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// delete email template
        /// neo 2010-12-11
        /// </summary>
        /// <param name="iEmailTemplateID"></param>
        //public void DeleteEmailTemplateBase(int iEmailTemplateID)
        //{
        //    string sSql = "update LoanTasks set WarningEmailId=null,OverdueEmailId=null,CompletionEmailId=null where WarningEmailId=" + iEmailTemplateID + " or OverdueEmailId=" + iEmailTemplateID + " or CompletionEmailId=" + iEmailTemplateID + ";"
        //                + "update Template_Wfl_Tasks set WarningEmailId=null,OverdueEmailId=null,CompletionEmailId=null where WarningEmailId=" + iEmailTemplateID + " or OverdueEmailId=" + iEmailTemplateID + " or CompletionEmailId=" + iEmailTemplateID + ";"
        //                + "update Template_Rules set PartnerAlertEmaillId=null,PartnerRecomEmaillId=null,LOAlertTemplId=null,LORecomTemplId=null,ProcessorAlertEmaillId=null,ProcessorRecomEmaillId=null,MgrAlertEmaillId=null,MgrRecomEmaillId=null where PartnerAlertEmaillId=" + iEmailTemplateID + " or PartnerRecomEmaillId=" + iEmailTemplateID + " or LOAlertTemplId=" + iEmailTemplateID + " or LORecomTemplId=" + iEmailTemplateID + " or ProcessorAlertEmaillId=" + iEmailTemplateID + " or ProcessorRecomEmaillId=" + iEmailTemplateID + " or MgrAlertEmaillId=" + iEmailTemplateID + " or MgrRecomEmaillId=" + iEmailTemplateID + ";"
        //                + "delete from EmailQue where EmailTmplId=" + iEmailTemplateID + ";"
        //                + "delete from Template_Email where TemplEmailId=" + iEmailTemplateID;

        //    DbHelperSQL.ExecuteNonQuery(sSql);
        //}

        /// <summary>
        /// get point field list
        /// neo 2010-12-17
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetPointFieldListBase(string sWhere)
        {
            string sSql = "select * from PointFieldDesc where 1=1 " + sWhere;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        #endregion

        /// <summary>
        /// Gets the email recipients.
        /// </summary>
        /// <param name="templEmailId">The templ email id.</param>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public DataSet GetEmailRecipients(int templEmailId, int fileId)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TemplEmailId", SqlDbType.Int),
					new SqlParameter("@FileId", SqlDbType.Int)
					};
            parameters[0].Value = templEmailId;
            parameters[1].Value = fileId;

            return DbHelperSQL.RunProcedure("lpsp_GetEmailRecipients", parameters, "ds");
        }
    }
}

