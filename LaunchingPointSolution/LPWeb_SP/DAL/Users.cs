using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Users。
    /// </summary>
    public class Users : UsersBase
    {
        public Users()
        { }

        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = "(SELECT u.*, dbo.lpfn_GetUserName(u.UserId) AS FullName, r.Name AS RoleName, dbo.lpfn_GetUserLoanCount(u.UserId) as UserLoanCount, dbo.lpfn_GetUserContactCount(u.UserId) as UserContactCount FROM Users u LEFT JOIN Roles r ON u.RoleId=r.RoleId) t";
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,2000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
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

        public DataSet GetListForUserReassign(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = "(SELECT *, dbo.lpfn_GetUserName(UserId) AS FullName FROM Users) u";
            return GetListByPage(tempTable, PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        public DataSet GetUserList(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = "(SELECT u.UserId, u.LastName, u.Firstname, u.UserName, u.UserEnabled,u.EmailAddress, dbo.lpfn_GetUserName(u.UserId) AS FullName, r.RoleId, r.Name AS RoleName, dbo.lpfn_GetUserLoanCount(u.UserId) as UserLoanCount, dbo.lpfn_GetUserContactCount(u.UserId) as UserContactCount FROM Users u LEFT JOIN Roles r ON u.RoleId=r.RoleId) t";
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,2000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
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

        private DataSet GetListByPage(string strTableName, int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,2000),
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
        /// 获取User列表
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetBranchManagerSeletion()
        {
            string sSql = @"SELECT Users.UserId, Users.Username,Users.LastName+', '+ Users.FirstName as FullName FROM 
 Users WHERE RoleID='1' order by LastName";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 用户名是否已经存在
        /// </summary>
        /// <param name="nID"></param>
        /// <param name="strUserName"></param>
        /// <returns></returns>
        public bool IsUserNameExists(int nID, string strUserName)
        {
            strUserName = strUserName.Replace('\'', '\"');
            string sSql = "";
            if (nID > 0)
                sSql = string.Format("SELECT * FROM Users WHERE UserId<>'{0}' AND Username='{1}'", nID, strUserName);
            else
                sSql = string.Format("SELECT * FROM Users WHERE Username='{0}'", strUserName);
            return DbHelperSQL.Exists(sSql);
        }

        /// <summary>
        /// 设置指定的User为Disable
        /// </summary>
        /// <param name="strWhere"></param>
        public void SetUsersDisable(string strWhere)
        {
            if (!string.IsNullOrEmpty(strWhere))
            {
                string strSql = string.Format("UPDATE Users SET UserEnabled=0 WHERE {0}", strWhere);
                DbHelperSQL.ExecuteNonQuery(strSql);
            }
        }

        /// <summary>
        /// 删除指定用户
        /// </summary>
        /// <param name="strWhere"></param>
        public void DeleteUsers(string strWhere)
        {
            if (!string.IsNullOrEmpty(strWhere))
            {
                string strSql = string.Format("DELETE Users WHERE {0}", strWhere);
                DbHelperSQL.ExecuteNonQuery(strSql);
            }
        }

        /// <summary>
        /// 用户邮件地址是否已经存在
        /// </summary>
        /// <param name="nID"></param>
        /// <param name="strUserName"></param>
        /// <returns></returns>
        public bool IsUserEmailExists(int nID, string strUserEmail)
        {
            string sSql = "";
            if (nID > 0)
                sSql = string.Format("SELECT * FROM Users WHERE UserId<>'{0}' AND ISNULL(EmailAddress, '')<>'' AND EmailAddress='{1}'", nID, strUserEmail);
            else
                sSql = string.Format("SELECT * FROM Users WHERE ISNULL(EmailAddress, '')<>'' AND EmailAddress='{0}'", strUserEmail);
            return DbHelperSQL.Exists(sSql);
        }

        /// <summary>
        /// whether user have privilege to set own goals
        /// </summary>
        /// <param name="nUserId"></param>
        /// <returns></returns>
        public bool IsHaveSetOwnGoalsPrivilege(int nUserId)
        {
            return false;
        }

        /// <summary>
        /// delete all user related info
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="nLoanRepID"></param>
        /// <param name="GroupId"></param>
        /// <param name="nCurrUserId"></param>
        /// <param name="nReassignUserId"></param>
        /// <returns></returns>
        public bool DeleteUserInfo(int UserId, int? nLoanRepID, int? GroupId, int nCurrUserId, int nReassignUserId)
        {
            Model.Users theUser = this.GetModel(UserId);
            Model.Users reassignUser = this.GetModel(nReassignUserId);
            string strTheUserName = string.Format("{0}, {1}", theUser.LastName, theUser.FirstName);
            string strReassignUserName = "";
            if (null != reassignUser)
                strReassignUserName = string.Format("{0}, {1}", reassignUser.LastName, reassignUser.FirstName);

            // delete user table
            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();
            SqlCommand cmdDeleteUser = new SqlCommand("DELETE FROM Users WHERE UserId=@UserId");
            DbHelperSQL.AddSqlParameter(cmdDeleteUser, "@UserId", SqlDbType.Int, UserId);
            SqlCmdList.Add(cmdDeleteUser);

            // delete UserHomePref table
            SqlCommand cmdDeleteUserHomeInfo = new SqlCommand("DELETE FROM UserHomePref WHERE UserId=@UserId");
            DbHelperSQL.AddSqlParameter(cmdDeleteUserHomeInfo, "@UserId", SqlDbType.Int, UserId);
            SqlCmdList.Add(cmdDeleteUserHomeInfo);

            // delete UserPipelineColumns table
            SqlCommand cmdDeletePipelineCols = new SqlCommand("DELETE FROM UserPipelineColumns WHERE UserId=@UserId");
            DbHelperSQL.AddSqlParameter(cmdDeletePipelineCols, "@UserId", SqlDbType.Int, UserId);
            SqlCmdList.Add(cmdDeletePipelineCols);

            // delete UserLoanRep table
            string strSqlULR = "";
            if (nLoanRepID.HasValue)
                strSqlULR = string.Format("UPDATE dbo.UserLoanRep SET UserId=NULL WHERE NameId={0}", nLoanRepID.Value);
            else
                strSqlULR = string.Format("UPDATE dbo.UserLoanRep SET UserId=NULL WHERE UserId={0}", UserId);
            SqlCommand cmdDeleteUlr = new SqlCommand(strSqlULR);
            SqlCmdList.Add(cmdDeleteUlr);

            // delete GroupUsers table
            string strSqlGroup = "";
            SqlCommand cmdDeleteGroupUsers = null;
            if (GroupId.HasValue)
            {
                strSqlGroup = "DELETE FROM GroupUsers WHERE GroupId=@GroupId AND UserId=@UserId ";
                cmdDeleteGroupUsers = new SqlCommand(strSqlGroup);
                DbHelperSQL.AddSqlParameter(cmdDeleteGroupUsers, "@GroupId", SqlDbType.Int, GroupId.Value);
                DbHelperSQL.AddSqlParameter(cmdDeleteGroupUsers, "@UserId", SqlDbType.Int, UserId);
            }
            else
            {
                strSqlGroup = "DELETE FROM GroupUsers WHERE UserId=@UserId ";
                cmdDeleteGroupUsers = new SqlCommand(strSqlGroup);
                DbHelperSQL.AddSqlParameter(cmdDeleteGroupUsers, "@UserId", SqlDbType.Int, UserId);
            }
            SqlCmdList.Add(cmdDeleteGroupUsers);

            // delete CompanyExecutive
            SqlCommand cmdDeleteComExe = new SqlCommand("DELETE dbo.CompanyExecutives WHERE ExecutiveId=@UserId");
            DbHelperSQL.AddSqlParameter(cmdDeleteComExe, "@UserId", SqlDbType.Int, UserId);
            SqlCmdList.Add(cmdDeleteComExe);

            //Delete UserLeadDist
            SqlCommand cmdDeleteUserLeadDist = new SqlCommand("DELETE dbo.UserLeadDist WHERE UserID=@UserId");
            DbHelperSQL.AddSqlParameter(cmdDeleteUserLeadDist, "@UserId", SqlDbType.Int, UserId);
            SqlCmdList.Add(cmdDeleteUserLeadDist);
            //Delete User2LeadSource
            SqlCommand cmdDeleteUser2LeadSource = new SqlCommand("DELETE dbo.User2LeadSource WHERE UserID=@UserId");
            DbHelperSQL.AddSqlParameter(cmdDeleteUser2LeadSource, "@UserId", SqlDbType.Int, UserId);
            SqlCmdList.Add(cmdDeleteUser2LeadSource);
            //Delete User2State
            SqlCommand cmdDeleteUser2State = new SqlCommand("DELETE dbo.User2State WHERE UserID=@UserId");
            DbHelperSQL.AddSqlParameter(cmdDeleteUser2State, "@UserId", SqlDbType.Int, UserId);
            SqlCmdList.Add(cmdDeleteUser2State);
            //Delete User2LoanType
            SqlCommand cmdUser2LoanType = new SqlCommand("DELETE dbo.User2LoanType WHERE UserID=@UserId");
            DbHelperSQL.AddSqlParameter(cmdUser2LoanType, "@UserId", SqlDbType.Int, UserId);
            SqlCmdList.Add(cmdUser2LoanType);
            //Delete User2Purpose
            SqlCommand cmdUser2Purpose = new SqlCommand("DELETE dbo.User2Purpose WHERE UserID=@UserId");
            DbHelperSQL.AddSqlParameter(cmdUser2Purpose, "@UserId", SqlDbType.Int, UserId);
            SqlCmdList.Add(cmdUser2Purpose);


            // reassign contact user
            SqlCommand cmdReassignContact = new SqlCommand();
            string strSqlQueryContactUsers = string.Format("SELECT * FROM ContactUsers WHERE UserId='{0}'", UserId);
            DataTable dtContactUsers = DbHelperSQL.ExecuteDataTable(strSqlQueryContactUsers);
            StringBuilder sbReassignContact = new StringBuilder();
            if (null != dtContactUsers)
            {
                foreach (DataRow drCu in dtContactUsers.Rows)
                {
                    sbReassignContact.AppendFormat("DELETE ContactUsers WHERE UserId='{0}' AND ContactId='{1}';", nReassignUserId, drCu["ContactId"]);
                }
            }
            sbReassignContact.AppendFormat("UPDATE ContactUsers SET UserId='{0}' WHERE UserId='{1}';", nReassignUserId, UserId);
            if (null != dtContactUsers)
            {
                foreach (DataRow drCu in dtContactUsers.Rows)
                {
                    sbReassignContact.AppendFormat("INSERT INTO ContactActivities (ContactId, UserId, ActivityName, ActivityTime) VALUES ({0}, {1}, 'User access to this contact has been replaced from {2} with {3}.', GetDate());",
                        drCu["ContactId"], nCurrUserId, strTheUserName, strReassignUserName);
                }
            }
            cmdReassignContact.CommandText = sbReassignContact.ToString();
            SqlCmdList.Add(cmdReassignContact);

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

        /// <summary>
        /// Add User Info, clone a user when the parameter nSourceID is not null
        /// by Peter
        /// </summary>
        /// <param name="user"></param>
        /// <param name="strLoanRepIds"></param>
        /// <param name="strSelectedIds"></param>
        /// <param name="nSourceID"></param>
        /// <param name="nComGroupId">Group Id of Company</param>
        /// <param name="nRoleIdExecutive">Executive Role Id</param>
        public int AddUserInfo(Model.Users user, string strLoanRepIds, string strGroupIds, int? nSourceID, int? nComGroupId, int? nRoleIdExecutive)
        {
            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

            int nUserID = -1;
            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;
            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                // 1.保存传入的User信息，要克隆的用户的基本信息（Users）仍然从页面上获取，其他信息从被克隆的用户获取
                StringBuilder sbSql = new StringBuilder();
                SqlCommand cmdAddUser = new SqlCommand();
                sbSql.Append("INSERT INTO Users(");
                sbSql.Append("UserEnabled,Username,EmailAddress,UserPictureFile,FirstName,LastName,RoleId,Password,LoansPerPage,MarketingAcctEnabled,Phone,Cell,Fax,[LOComp],[BranchMgrComp],[DivisionMgrComp],[RegionMgrComp],ExchangePassword)");
                sbSql.Append(" VALUES (");
                sbSql.Append("@UserEnabled,@Username,@EmailAddress,@UserPictureFile,@FirstName,@LastName,@RoleId,@Password,@LoansPerPage,@MarketingAcctEnabled,@Phone,@Cell,@Fax,@LOComp,@BranchMgrComp,@DivisionMgrComp,@RegionMgrComp,@ExchangePassword)");
                sbSql.Append(";SELECT @@IDENTITY");
                SqlParameter[] cmdParms = {
                    new SqlParameter("@UserEnabled", SqlDbType.Bit),
                    new SqlParameter("@Username", SqlDbType.NVarChar),
                    new SqlParameter("@EmailAddress", SqlDbType.NVarChar),
                    new SqlParameter("@UserPictureFile", SqlDbType.VarBinary),
                    new SqlParameter("@FirstName", SqlDbType.NVarChar),
                    new SqlParameter("@LastName", SqlDbType.NVarChar),
                    new SqlParameter("@RoleId", SqlDbType.Int),
                    new SqlParameter("@Password", SqlDbType.NVarChar),
                    new SqlParameter("@LoansPerPage", SqlDbType.SmallInt),
                    new SqlParameter("@MarketingAcctEnabled", SqlDbType.Bit),
                    new SqlParameter("@Phone", SqlDbType.NVarChar),
                    new SqlParameter("@Cell", SqlDbType.NVarChar),
                    new SqlParameter("@Fax", SqlDbType.NVarChar),
                    new SqlParameter("@LOComp", SqlDbType.Decimal),
                    new SqlParameter("@BranchMgrComp", SqlDbType.Decimal),
                    new SqlParameter("@DivisionMgrComp", SqlDbType.Decimal),
                    new SqlParameter("@RegionMgrComp", SqlDbType.Decimal),
                    new SqlParameter("@ExchangePassword", SqlDbType.NVarChar)
                };
                cmdParms[0].Value = user.UserEnabled;
                cmdParms[1].Value = user.Username;
                cmdParms[2].Value = user.EmailAddress;
                cmdParms[3].Value = user.UserPictureFile;
                cmdParms[4].Value = user.FirstName;
                cmdParms[5].Value = user.LastName;
                cmdParms[6].Value = user.RoleId;
                cmdParms[7].Value = nSourceID.HasValue ? "" : user.Password;
                cmdParms[8].Value = user.LoansPerPage;
                cmdParms[9].Value = user.MarketingAcctEnabled;
                cmdParms[10].Value = user.Phone;
                cmdParms[11].Value = user.Cell;
                cmdParms[12].Value = user.Fax;
                cmdParms[13].Value = user.LOComp;
                cmdParms[14].Value = user.BranchMgrComp;
                cmdParms[15].Value = user.DivisionMgrComp;
                cmdParms[16].Value = user.RegionMgrComp;
                cmdParms[17].Value = user.ExchangePassword;

                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmdAddUser.Parameters.Add(parameter);
                }

                cmdAddUser.CommandText = sbSql.ToString();
                cmdAddUser.Transaction = SqlTrans;
                cmdAddUser.Connection = SqlConn;
                object obj = cmdAddUser.ExecuteScalar();
                cmdAddUser.Parameters.Clear();
                if (!(Object.Equals(obj, null)) && !(Object.Equals(obj, System.DBNull.Value)))
                {
                    if (!int.TryParse(obj.ToString(), out nUserID))
                        nUserID = -1;
                }
                // Lin added the BranchUser, DivisionUser and RegionUser for this user + group
                if (nUserID > 0 && !string.IsNullOrEmpty(strGroupIds))
                {
                      string[] GroupIds = strGroupIds.Trim().Split(',');
                      foreach (string groupId in GroupIds)
                      {
                          AddUserOrgs(nUserID, groupId);
                      }
                }
                // 2.保存传入的UserLoanRep信息
                if (!string.IsNullOrEmpty(strLoanRepIds))
                {
                    SqlCommand cmdUserLoanRep = new SqlCommand(string.Format("UPDATE dbo.UserLoanRep SET UserId={0} WHERE NameId IN ({1})", nUserID, strLoanRepIds));
                    SqlCmdList.Add(cmdUserLoanRep);
                }

                // 3.保存传入的GroupUsers信息
                if (!string.IsNullOrEmpty(strGroupIds))
                {
                    SqlCommand cmdGroup = new SqlCommand();
                    StringBuilder sbGroup = new StringBuilder();
                    string[] arrGroupIDs = strGroupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in arrGroupIDs)
                    {
                        sbGroup.Append(string.Format("INSERT INTO dbo.GroupUsers(GroupID,UserID)VALUES({0}, {1});", str, nUserID));
                    }
                    cmdGroup.CommandText = sbGroup.ToString();
                    SqlCmdList.Add(cmdGroup);

                    // 3.1 save companyexecutive
                    if (nRoleIdExecutive.HasValue && user.RoleId == nRoleIdExecutive.Value && nComGroupId.HasValue)
                    {
                        int nComIndex = Array.IndexOf(arrGroupIDs, nComGroupId.Value.ToString());
                        if (nComIndex >= 0)
                        {
                            SqlCommand cmdGroupExe = new SqlCommand();
                            cmdGroupExe.CommandText = string.Format("INSERT INTO dbo.CompanyExecutives(ExecutiveId) VALUES({0})", nUserID);
                            SqlCmdList.Add(cmdGroupExe);
                        }
                    }
                }

                if (nSourceID.HasValue)
                {
                    // 4.复制UserPipelineColumns信息
                    DAL.UserPipelineColumns dal = new UserPipelineColumns();
                    Model.UserPipelineColumns userPipelineCols = dal.GetModel(nSourceID.Value);
                    if (null != userPipelineCols)
                    {
                        SqlCommand cmdPipeline = new SqlCommand();
                        StringBuilder sbSqlUPC = new StringBuilder();
                        sbSqlUPC.Append("insert into UserPipelineColumns(");
                        sbSqlUPC.Append("UserId,PointFolder,Stage,Branch,EstimatedClose,Alerts,LoanOfficer,Amount,Lien,Rate,Lender,LockExp,PercentCompl,Processor,TaskCount,PointFileName)");
                        sbSqlUPC.Append(" values (");
                        sbSqlUPC.Append("@UserId,@PointFolder,@Stage,@Branch,@EstimatedClose,@Alerts,@LoanOfficer,@Amount,@Lien,@Rate,@Lender,@LockExp,@PercentCompl,@Processor,@TaskCount,@PointFileName)");
                        SqlParameter[] parameters = {
					        new SqlParameter("@UserId", SqlDbType.Int,4),
					        new SqlParameter("@PointFolder", SqlDbType.Bit,1),
					        new SqlParameter("@Stage", SqlDbType.Bit,1),
					        new SqlParameter("@Branch", SqlDbType.Bit,1),
					        new SqlParameter("@EstimatedClose", SqlDbType.Bit,1),
					        new SqlParameter("@Alerts", SqlDbType.Bit,1),
					        new SqlParameter("@LoanOfficer", SqlDbType.Bit,1),
					        new SqlParameter("@Amount", SqlDbType.Bit,1),
					        new SqlParameter("@Lien", SqlDbType.Bit,1),
					        new SqlParameter("@Rate", SqlDbType.Bit,1),
					        new SqlParameter("@Lender", SqlDbType.Bit,1),
					        new SqlParameter("@LockExp", SqlDbType.Bit,1),
					        new SqlParameter("@PercentCompl", SqlDbType.Bit,1),
					        new SqlParameter("@Processor", SqlDbType.Bit,1),
					        new SqlParameter("@TaskCount", SqlDbType.Bit,1),
					        new SqlParameter("@PointFileName", SqlDbType.Bit,1)};
                        parameters[0].Value = nUserID;
                        parameters[1].Value = userPipelineCols.PointFolder;
                        parameters[2].Value = userPipelineCols.Stage;
                        parameters[3].Value = userPipelineCols.Branch;
                        parameters[4].Value = userPipelineCols.EstimatedClose;
                        parameters[5].Value = userPipelineCols.Alerts;
                        parameters[6].Value = userPipelineCols.LoanOfficer;
                        parameters[7].Value = userPipelineCols.Amount;
                        parameters[8].Value = userPipelineCols.Lien;
                        parameters[9].Value = userPipelineCols.Rate;
                        parameters[10].Value = userPipelineCols.Lender;
                        parameters[11].Value = userPipelineCols.LockExp;
                        parameters[12].Value = userPipelineCols.PercentCompl;
                        parameters[13].Value = userPipelineCols.Processor;
                        parameters[14].Value = userPipelineCols.TaskCount;
                        parameters[15].Value = userPipelineCols.PointFileName;

                        cmdPipeline.CommandText = sbSqlUPC.ToString();
                        foreach (SqlParameter parameter in parameters)
                        {
                            if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                                (parameter.Value == null))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            cmdPipeline.Parameters.Add(parameter);
                        }
                        SqlCmdList.Add(cmdPipeline);
                    }
                    else
                    {
                        // save UserPipelineColumns default value
                        SqlCommand cmdUserPipelineColsDefault = new SqlCommand();
                        cmdUserPipelineColsDefault.CommandText = string.Format("INSERT INTO UserPipelineColumns(UserId, Stage, EstimatedClose, Alerts, LoanOfficer, Amount, PercentCompl) VALUES('{0}', 1, 1, 1, 1, 1, 1)", nUserID);
                        SqlCmdList.Add(cmdUserPipelineColsDefault);
                    }

                    // 5.复制UserHomePref信息
                    DAL.UserHomePref dalHomePref = new UserHomePref();
                    Model.UserHomePref userHomePref = dalHomePref.GetModel(nSourceID.Value);
                    if (null != userHomePref)
                    {
                        SqlCommand cmdHomePref = new SqlCommand();
                        StringBuilder sbHomePref = new StringBuilder();
                        sbHomePref.Append("insert into UserHomePref(");
                        sbHomePref.Append("UserId,CompanyCalendar,PipelineChart,SalesBreakdownChart,OrgProductionChart,Org_N_Sales_Charts,RateSummary,GoalsChart,OverDueTaskAlert,Announcements,ExchangeInbox,ExchangeCalendar)");
                        sbHomePref.Append(" values (");
                        sbHomePref.Append("@UserId,@CompanyCalendar,@PipelineChart,@SalesBreakdownChart,@OrgProductionChart,@Org_N_Sales_Charts,@RateSummary,@GoalsChart,@OverDueTaskAlert,@Announcements,@ExchangeInbox,@ExchangeCalendar)");
                        SqlParameter[] paramsHomePref = {
					        new SqlParameter("@UserId", SqlDbType.Int,4),
					        new SqlParameter("@CompanyCalendar", SqlDbType.Bit,1),
					        new SqlParameter("@PipelineChart", SqlDbType.Bit,1),
					        new SqlParameter("@SalesBreakdownChart", SqlDbType.Bit,1),
					        new SqlParameter("@OrgProductionChart", SqlDbType.Bit,1),
					        new SqlParameter("@Org_N_Sales_Charts", SqlDbType.Bit,1),
					        new SqlParameter("@RateSummary", SqlDbType.Bit,1),
					        new SqlParameter("@GoalsChart", SqlDbType.Bit,1),
					        new SqlParameter("@OverDueTaskAlert", SqlDbType.Bit,1),
					        new SqlParameter("@Announcements", SqlDbType.Bit,1),
					        new SqlParameter("@ExchangeInbox", SqlDbType.Bit,1),
					        new SqlParameter("@ExchangeCalendar", SqlDbType.Bit,1)};
                        paramsHomePref[0].Value = nUserID;
                        paramsHomePref[1].Value = userHomePref.CompanyCalendar;
                        paramsHomePref[2].Value = userHomePref.PipelineChart;
                        paramsHomePref[3].Value = userHomePref.SalesBreakdownChart;
                        paramsHomePref[4].Value = userHomePref.OrgProductionChart;
                        paramsHomePref[5].Value = userHomePref.Org_N_Sales_Charts;
                        paramsHomePref[6].Value = userHomePref.RateSummary;
                        paramsHomePref[7].Value = userHomePref.GoalsChart;
                        paramsHomePref[8].Value = userHomePref.OverDueTaskAlert;
                        paramsHomePref[9].Value = userHomePref.Announcements;
                        paramsHomePref[10].Value = userHomePref.ExchangeInbox;
                        paramsHomePref[11].Value = userHomePref.ExchangeCalendar;

                        cmdHomePref.CommandText = sbHomePref.ToString();
                        foreach (SqlParameter parameter in paramsHomePref)
                        {
                            if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                                (parameter.Value == null))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            cmdHomePref.Parameters.Add(parameter);
                        }
                        SqlCmdList.Add(cmdHomePref);
                    }

                    DAL.UserProspectColumns calProspectPref = new UserProspectColumns();
                    Model.UserProspectColumns userProspectPref = calProspectPref.GetModel(nSourceID.Value);
                    if (null != userProspectPref)
                    {
                        SqlCommand cmdProspectPref = new SqlCommand();
                        StringBuilder sbProspectPref = new StringBuilder();
                        sbProspectPref.Append("insert into UserProspectColumns(");
                        sbProspectPref.Append("UserId, PV_Created, PV_LeadSource, PV_RefCode, PV_LoanOfficer, PV_Branch, PV_Progress, LV_Ranking, LV_Amount, LV_Rate, LV_LoanOfficer, LV_Lien, LV_Progress, LV_Branch, LV_LoanProgram, LV_LeadSource, LV_RefCode, LV_EstClose, LV_PointFilename,Pv_Referral,Pv_Partner,Lv_Referral,Lv_Partner)");
                        sbProspectPref.Append(" values (");
                        sbProspectPref.Append("@UserId,@PV_Created,@PV_LeadSource,@PV_RefCode,@PV_LoanOfficer,@PV_Branch,@PV_Progress,@LV_Ranking,@LV_Amount,@LV_Rate,@LV_LoanOfficer,@LV_Lien,@LV_Progress,@LV_Branch,@LV_LoanProgram,@LV_LeadSource,@LV_RefCode,@LV_EstClose,@LV_PointFilename,@Pv_Referral,@Pv_Partner,@Lv_Referral,@Lv_Partner)");
                        SqlParameter[] paramsProspectPref = {
					        new SqlParameter("@UserId", SqlDbType.Int,4),
					        new SqlParameter("@PV_Created", SqlDbType.Bit,1),
					        new SqlParameter("@PV_LeadSource", SqlDbType.Bit,1),
					        new SqlParameter("@PV_RefCode", SqlDbType.Bit,1),
					        new SqlParameter("@PV_LoanOfficer", SqlDbType.Bit,1),
					        new SqlParameter("@PV_Branch", SqlDbType.Bit,1),
					        new SqlParameter("@PV_Progress", SqlDbType.Bit,1),
					        new SqlParameter("@LV_Ranking", SqlDbType.Bit,1),
					        new SqlParameter("@LV_Amount", SqlDbType.Bit,1),
					        new SqlParameter("@LV_Rate", SqlDbType.Bit,1),
					        new SqlParameter("@LV_LoanOfficer", SqlDbType.Bit,1),
					        new SqlParameter("@LV_Lien", SqlDbType.Bit,1),
					        new SqlParameter("@LV_Progress", SqlDbType.Bit,1),
					        new SqlParameter("@LV_Branch", SqlDbType.Bit,1),
					        new SqlParameter("@LV_LoanProgram", SqlDbType.Bit,1),
					        new SqlParameter("@LV_LeadSource", SqlDbType.Bit,1),
					        new SqlParameter("@LV_RefCode", SqlDbType.Bit,1),
					        new SqlParameter("@LV_EstClose", SqlDbType.Bit,1),
					        new SqlParameter("@Lv_PointFilename", SqlDbType.Bit,1),
					        new SqlParameter("@Pv_Referral", SqlDbType.Bit,1),
					        new SqlParameter("@Pv_Partner", SqlDbType.Bit,1),
					        new SqlParameter("@Lv_Referral", SqlDbType.Bit,1),
					        new SqlParameter("@Lv_Partner", SqlDbType.Bit,1)};
                        paramsProspectPref[0].Value = nUserID;
                        paramsProspectPref[1].Value = userProspectPref.Pv_Created;
                        paramsProspectPref[2].Value = userProspectPref.Pv_Leadsource;
                        paramsProspectPref[3].Value = userProspectPref.Pv_Refcode;
                        paramsProspectPref[4].Value = userProspectPref.Pv_Loanofficer;
                        paramsProspectPref[5].Value = userProspectPref.Pv_Branch;
                        paramsProspectPref[6].Value = userProspectPref.Pv_Progress;
                        paramsProspectPref[7].Value = userProspectPref.Lv_Ranking;
                        paramsProspectPref[8].Value = userProspectPref.Lv_Amount;
                        paramsProspectPref[9].Value = userProspectPref.Lv_Rate;
                        paramsProspectPref[10].Value = userProspectPref.Lv_Loanofficer;
                        paramsProspectPref[11].Value = userProspectPref.Lv_Lien;
                        paramsProspectPref[12].Value = userProspectPref.Lv_Progress;
                        paramsProspectPref[13].Value = userProspectPref.Lv_Branch;
                        paramsProspectPref[14].Value = userProspectPref.Lv_Loanprogram;
                        paramsProspectPref[15].Value = userProspectPref.Lv_Leadsource;
                        paramsProspectPref[16].Value = userProspectPref.Lv_Refcode;
                        paramsProspectPref[17].Value = userProspectPref.Lv_Estclose;
                        paramsProspectPref[18].Value = userProspectPref.Lv_Pointfilename;
                        paramsProspectPref[19].Value = userProspectPref.Pv_Referral;
                        paramsProspectPref[20].Value = userProspectPref.Pv_Partner;
                        paramsProspectPref[21].Value = userProspectPref.Lv_Referral;
                        paramsProspectPref[22].Value = userProspectPref.Lv_Partner;

                        cmdProspectPref.CommandText = sbProspectPref.ToString();
                        foreach (SqlParameter parameter in paramsProspectPref)
                        {
                            if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                                (parameter.Value == null))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            cmdProspectPref.Parameters.Add(parameter);
                        }
                        SqlCmdList.Add(cmdProspectPref);
                    }
                    else
                    {
                        // save UserProspectColumns default value
                        SqlCommand cmdUserProspectColsDefault = new SqlCommand();
                        cmdUserProspectColsDefault.CommandText = string.Format("INSERT INTO UserProspectColumns(UserId, PV_Created, PV_LeadSource, PV_RefCode, PV_LoanOfficer, PV_Progress, LV_Ranking, LV_Amount, LV_LoanOfficer, LV_Progress, LV_LeadSource) VALUES('{0}', 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)", nUserID);
                        SqlCmdList.Add(cmdUserProspectColsDefault);
                    }
                }
                else
                {
                    // save UserPipelineColumns and UserProspectColumns default value
                    SqlCommand cmdUserPipelineColsDefault = new SqlCommand();
                    cmdUserPipelineColsDefault.CommandText = string.Format("INSERT INTO UserPipelineColumns(UserId, Stage, EstimatedClose, Alerts, LoanOfficer, Amount, PercentCompl) VALUES('{0}', 1, 1, 1, 1, 1, 1)", nUserID);
                    SqlCmdList.Add(cmdUserPipelineColsDefault);

                    SqlCommand cmdUserProspectColsDefault = new SqlCommand();
                    cmdUserProspectColsDefault.CommandText = string.Format("INSERT INTO UserProspectColumns(UserId, PV_Created, PV_LeadSource, PV_RefCode, PV_LoanOfficer, PV_Progress, LV_Ranking, LV_Amount, LV_LoanOfficer, LV_Progress, LV_LeadSource) VALUES('{0}', 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)", nUserID);
                    SqlCmdList.Add(cmdUserProspectColsDefault);
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
            return nUserID;
        }

        /// <summary>
        /// Save user info from UserSetup page
        /// </summary>
        /// <param name="user"></param>
        /// <param name="strLoanRepIds"></param>
        /// <param name="strGroupIds"></param>
        /// <param name="nComGroupId">Group Id of Company</param>
        /// <param name="nRoleIdExecutive">Executive Role Id</param>
        public void UpdateUserInfo(Model.Users user, string strLoanRepIds, string strGroupIds, int? nComGroupId, int? nRoleIdExecutive)
        {
            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;
            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                // 1.Update user info
                SqlCommand cmdUser = new SqlCommand();
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("update Users set ");
                sbSql.Append("UserEnabled=@UserEnabled,");
                sbSql.Append("Username=@Username,");
                sbSql.Append("EmailAddress=@EmailAddress,");
                sbSql.Append("UserPictureFile=@UserPictureFile,");
                sbSql.Append("FirstName=@FirstName,");
                sbSql.Append("LastName=@LastName,");
                sbSql.Append("RoleId=@RoleId,");
                sbSql.Append("Password=@Password,");
                sbSql.Append("LoansPerPage=@LoansPerPage,");
                sbSql.Append("MarketingAcctEnabled=@MarketingAcctEnabled,");
                sbSql.Append("Phone=@Phone,");
                sbSql.Append("Cell=@Cell,");
                sbSql.Append("Fax=@Fax,");
                sbSql.Append("LOComp=@LOComp,");
                sbSql.Append("BranchMgrComp=@BranchMgrComp,");
                sbSql.Append("DivisionMgrComp=@DivisionMgrComp,");
                sbSql.Append("RegionMgrComp=@RegionMgrComp,");
                sbSql.Append("ExchangePassword=@ExchangePassword");
                sbSql.Append(" where UserId=@UserId ");
                SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserEnabled", SqlDbType.Bit,1),
					new SqlParameter("@Username", SqlDbType.NVarChar,50),
					new SqlParameter("@EmailAddress", SqlDbType.NVarChar,255),
					new SqlParameter("@UserPictureFile", SqlDbType.VarBinary,-1),
					new SqlParameter("@FirstName", SqlDbType.NVarChar,50),
					new SqlParameter("@LastName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoleId", SqlDbType.Int,4),
					new SqlParameter("@Password", SqlDbType.NVarChar,50),
					new SqlParameter("@LoansPerPage", SqlDbType.SmallInt,2),
					new SqlParameter("@MarketingAcctEnabled", SqlDbType.Bit,1),
                    new SqlParameter("@Phone",SqlDbType.NVarChar,20),
                    new SqlParameter("@Cell",SqlDbType.NVarChar,20),
                    new SqlParameter("@Fax",SqlDbType.NVarChar,20),
                    new SqlParameter("@LOComp",SqlDbType.Decimal),
                    new SqlParameter("@BranchMgrComp",SqlDbType.Decimal),
                    new SqlParameter("@DivisionMgrComp",SqlDbType.Decimal),
                    new SqlParameter("@RegionMgrComp",SqlDbType.Decimal),
                    new SqlParameter("@ExchangePassword",SqlDbType.NVarChar)
                                            };
                parameters[0].Value = user.UserId;
                parameters[1].Value = user.UserEnabled;
                parameters[2].Value = user.Username;
                parameters[3].Value = user.EmailAddress;
                parameters[4].Value = user.UserPictureFile;
                parameters[5].Value = user.FirstName;
                parameters[6].Value = user.LastName;
                parameters[7].Value = user.RoleId;
                parameters[8].Value = user.Password;
                parameters[9].Value = user.LoansPerPage;
                parameters[10].Value = user.MarketingAcctEnabled;

                parameters[11].Value = user.Phone;
                parameters[12].Value = user.Cell;
                parameters[13].Value = user.Fax;

                parameters[14].Value = user.LOComp;
                parameters[15].Value = user.BranchMgrComp;
                parameters[16].Value = user.DivisionMgrComp;
                parameters[17].Value = user.RegionMgrComp;
                parameters[18].Value = user.ExchangePassword;

                cmdUser.CommandText = sbSql.ToString();
                foreach (SqlParameter parameter in parameters)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmdUser.Parameters.Add(parameter);
                }
                SqlCmdList.Add(cmdUser);

                // 2.Update UserLoanRepMapping info
                SqlCommand cmdLoanRep = new SqlCommand();
                StringBuilder sbLoanRep = new StringBuilder();
                sbLoanRep.Append(string.Format("UPDATE dbo.UserLoanRep SET UserId=NULL WHERE UserId={0};", user.UserId));
                if (!string.IsNullOrEmpty(strLoanRepIds))
                {
                    sbLoanRep.Append(string.Format("UPDATE dbo.UserLoanRep SET UserId={0} WHERE NameId IN ({1})", user.UserId, strLoanRepIds));
                }
                cmdLoanRep.CommandText = sbLoanRep.ToString();
                SqlCmdList.Add(cmdLoanRep);

                // 3.Update GroupUsers info
                int nComExeCount = 0;   // whether current user is company executive
                string strQueryComExe = string.Format("SELECT COUNT(*) FROM dbo.CompanyExecutives WHERE ExecutiveId={0}", user.UserId);
                object objGE = DbHelperSQL.ExecuteScalar(strQueryComExe);
                if (!(Object.Equals(objGE, null)) && !(Object.Equals(objGE, System.DBNull.Value)))
                {
                    if (!int.TryParse(objGE.ToString(), out nComExeCount))
                        nComExeCount = 0;
                }
                SqlCommand cmdGroupExe = new SqlCommand();

                SqlCommand cmdGroupUsers = new SqlCommand();
                StringBuilder sbGroupUsers = new StringBuilder();
                sbGroupUsers.Append(string.Format("DELETE dbo.GroupUsers WHERE UserID={0}; exec dbo.lpsp_DeleteUserOrgs {0}; ", user.UserId));
                if (!string.IsNullOrEmpty(strGroupIds))
                {
                    string[] arrGroups = strGroupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in arrGroups)
                    {
                        sbGroupUsers.Append(string.Format("INSERT INTO dbo.GroupUsers(GroupID,UserID)VALUES({0}, {1});", str, user.UserId));
                        sbGroupUsers.Append(string.Format("exec dbo.lpsp_UpdateUserOrgs {0}, {1}; ", user.UserId, str));
                    }

                    // 3.1 save companyexecutive
                    if (nRoleIdExecutive.HasValue && user.RoleId == nRoleIdExecutive.Value && nComGroupId.HasValue)
                    {
                        // get companyexecutive
                        int nComIndex = Array.IndexOf(arrGroups, nComGroupId.Value.ToString());   // whether company group selected
                        if (nComIndex >= 0)
                        {
                            if (nComExeCount <= 0)
                            {
                                cmdGroupExe.CommandText = string.Format("INSERT INTO dbo.CompanyExecutives(ExecutiveId) VALUES({0})", user.UserId);
                                SqlCmdList.Add(cmdGroupExe);
                            }
                        }
                        else
                        {
                            if (nComExeCount > 0)
                            {
                                cmdGroupExe.CommandText = string.Format("DELETE dbo.CompanyExecutives WHERE ExecutiveId={0}", user.UserId);
                                SqlCmdList.Add(cmdGroupExe);
                            }
                        }
                    }
                }
                else
                {
                    if (nComExeCount > 0)
                    {
                        cmdGroupExe.CommandText = string.Format("DELETE dbo.CompanyExecutives WHERE ExecutiveId={0}", user.UserId);
                        SqlCmdList.Add(cmdGroupExe);
                    }
                }
                cmdGroupUsers.CommandText = sbGroupUsers.ToString();
                SqlCmdList.Add(cmdGroupUsers);

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
        /// Get user and his branch group info
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserBranchInfo()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT u.UserId, b.Name FROM Users u INNER JOIN GroupUsers gu ON u.UserId=gu.UserID ");
            sbSql.Append("INNER JOIN Groups g ON gu.GroupID=g.GroupId INNER JOIN Branches b ON g.BranchID=b.BranchId ");
            sbSql.Append("WHERE g.OrganizationType='Branch'");
            return DbHelperSQL.ExecuteDataTable(sbSql.ToString());
        }

        public DataTable GetUserBranchInfo(string sUserID)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT u.UserId, b.Name,g.BranchID FROM Users u INNER JOIN GroupUsers gu ON u.UserId=gu.UserID ");
            sbSql.Append("INNER JOIN Groups g ON gu.GroupID=g.GroupId INNER JOIN Branches b ON g.BranchID=b.BranchId ");
            sbSql.Append("WHERE g.OrganizationType='Branch' and u.UserId=" + sUserID);
            return DbHelperSQL.ExecuteDataTable(sbSql.ToString());
        }

        /// <summary>
        /// 获取Group Member列表
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <returns></returns>
        public DataTable GetGroupMemberListBase(int iGroupID)
        {
            string sSql = "select * from Users where GroupID = " + iGroupID + " order by LastName";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 获取User列表
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetUserListBase(string sWhere)
        {
            string sSql = "select * from Users where 1=1 " + sWhere + " order by LastName";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetUserList(string sWhere)
        {
            string sSql = "select * from dbo.lpvw_GetUserGroups_ByRoleName where 1=1 AND UserEnabled=1 " + sWhere + " order by UserName";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
        /// <summary>
        /// 获取User列表
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetBranchManager(string BranchID)
        {
            string sSql = @"SELECT Users.UserId, Users.Username,Users.LastName+', '+ Users.FirstName as FullName FROM dbo.BranchManagers INNER JOIN
 dbo.Users ON BranchManagers.BranchMgrId =  Users.UserId  WHERE BranchManagers.BranchID='" + BranchID + "' order by LastName";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 获取Region选择Executive的列表
        /// </summary>
        /// <param name="regionId">region id</param>
        /// <param name="groupId">group id</param>
        /// <param name="executiveRoleId">executive role id</param>
        /// <returns></returns>
        public DataTable GetRegionExecutivesSelectionList(int regionId, int groupId, int executiveRoleId)
        {
            //region选择Executives的规则：
            //1.该用户是当前Region关联的Group下的
            //2.该用户是"Executive"角色
            //3.该用户当前是不属于任何Division的Executive

            //这里筛选GroupId时要按照新的表结构来查询，新的表结构改动：1.删除了users表中的regionid,divisionid,groupid等字段 2.添加了GroupUser表

            //string sSql = string.Format("select * from Users where UserID IN (SELECT UserID FROM GroupUsers) AND RoleID={1} AND UserId NOT IN(SELECT ExecutiveId FROM DivisionExecutives) order by LastName", groupId, executiveRoleId);

            string sSql = string.Format("select * from Users where UserID not IN (SELECT UserID FROM GroupUsers) AND RoleID={1} order by LastName", groupId, executiveRoleId);
            //todo:暂时修改
            //string sSql = string.Format("select * from Users where RoleID={1} AND UserId NOT IN(SELECT ExecutiveId FROM RegionExecutives) order by LastName", groupId, executiveRoleId);

            return DbHelperSQL.ExecuteDataTable(sSql);
        }


        /// <summary>
        /// 获取User列表
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetDivisionExecutive(string DivisionId)
        {
            string sSql = @"SELECT Users.UserId, Users.Username,Users.LastName+', '+ Users.FirstName as FullName FROM DivisionExecutives INNER JOIN
 Users ON DivisionExecutives.ExecutiveId =  Users.UserId  WHERE DivisionExecutives.DivisionId='" + DivisionId + "' order by LastName";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }


        /// <summary>
        /// 获取具有division executive 权限的 User列表
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetDivisionExecutiveSeletion()
        {
            string sSql = @"SELECT Users.UserId, Users.Username,Users.LastName+', '+ Users.FirstName as FullName FROM 
 Users WHERE RoleID='1' order by LastName";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataSet GetLoanOfficer(int FileId, string RoleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT Roles.Name, Users.FirstName, Users.LastName, LoanTeam.FileId, LoanTeam.RoleId, LoanTeam.UserId, Users.Username ");
            strSql.Append(" FROM LoanTeam INNER JOIN Users ON LoanTeam.UserId = Users.UserId INNER JOIN Roles ON LoanTeam.RoleId = Roles.RoleId ");
            strSql.Append(" WHERE LoanTeam.FileId = @FileId AND Roles.Name = @RoleName  ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int),
					new SqlParameter("@RoleName", SqlDbType.NVarChar)};
            parameters[0].Value = FileId;
            parameters[1].Value = RoleName;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            return ds;
        }

        /// <summary>
        /// adjust whether or not the user is company executive
        /// neo 2010-11-14
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsCompanyExecutiveBase(int iUserID)
        {
            string sSql = "select * from Groups as a inner join GroupUsers as b on a.GroupId = b.GroupID inner join Users as c on b.UserID = c.UserId "
                        + "where OrganizationType='Company' and c.RoleId = 1 and b.UserID = " + iUserID;
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
        /// adjust whether or not the user is region executive
        /// neo 2010-11-14
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsRegionExecutiveBase(int iUserID)
        {
            string sSql = "select count(1) from Users as a inner join RegionExecutives as b on a.UserId = b.ExecutiveId where a.RoleId = 1 and a.UserId = " + iUserID;
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
        /// adjust whether or not the user is division executive
        /// neo 2010-11-14
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsDivisionExecutiveBase(int iUserID)
        {
            string sSql = "select count(1) from Users as a inner join DivisionExecutives as b on a.UserId = b.ExecutiveId where a.RoleId = 1 and a.UserId = " + iUserID;
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

        private void AddUserOrgs(int UserId, string GroupId)
        {
            int iGroupId = 0;
            try
            {
                int.TryParse(GroupId, out iGroupId);
                if (UserId <= 0 || iGroupId <= 0)
                    return;
                string sqlCmd = string.Format("exec dbo.[lpsp_UpdateUserOrgs]({0}, {1})", UserId, iGroupId);
                DbHelperSQL.ExecuteNonQuery(sqlCmd);
            }
            catch (Exception ex)
            {

            }
        }

        public DataSet GetAllBranchUser(int FileId, int RoleId)
        {
            DataSet ds = null;
            //SqlParameter[] parameters = {
            //        new SqlParameter("@FileId", SqlDbType.Int),
            //        new SqlParameter("@RoleId", SqlDbType.Int)};
            //parameters[0].Value = FileId;
            //parameters[1].Value = RoleId;
            //ds = DbHelperSQL.RunProcedure("lpsp_ReassignLoanUser", parameters, "ds");
            string sqlCmd = "select BranchId from lpvw_GetPointFileInfo Where FileId=" + FileId;
            ds = DbHelperSQL.Query(sqlCmd);
            if (ds == null)
                return ds;
            if (ds.Tables[0].Rows.Count <= 0)
            {
                ds.Clear();
                ds.Dispose();
                ds = null;
                return ds;
            }
            int BranchId = ds.Tables[0].Rows[0][0] == DBNull.Value ? 0 : (int)ds.Tables[0].Rows[0][0];
            ds.Clear();
            ds.Dispose();
            sqlCmd = string.Format("select UserId, Username from lpvw_GetUserGroups where BranchId={0} AND RoleId={1}", BranchId, RoleId);
            sqlCmd += "GROUP BY UserId, Username";
            ds = DbHelperSQL.Query(sqlCmd);
            return ds;
        }

        /// <summary>
        /// adjust whether or not the user is Branch Manager
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public bool IsBranchManagerBase(int iUserID)
        {
            string sSql = "select count(1) from Users as a inner join BranchManagers as b on a.UserId = b.BranchMgrId where a.RoleId = 2 and a.UserId = " + iUserID;
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
        /// Gets the user name from user role.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public string GetUserNameFromUserRole(int roleId, int fileId)
        {
            string sSql = "SELECT ISNULL(Lastname,'')+', '+ISNULL(Firstname,'') FROM dbo.LoanTeam lt inner join Users u ON lt.UserId=u.UserId WHERE lt.RoleId={0} and lt.FileId={1}";
            sSql = string.Format(sSql, roleId, fileId);

            object oReturn = DbHelperSQL.ExecuteScalar(sSql);

            if (oReturn != null)
            {
                return oReturn.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public DataSet GetAllCompanyUserByRoleId(int RoleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT Users.UserId,Users.LastName +', '+ Users.FirstName AS Username FROM Users WHERE UserEnabled = 'true'");
            strSql.Append(" AND Users.UserId IN(SELECT UserID FROM GroupUsers WHERE GroupID IN(SELECT TOP 1 GroupId FROM Groups WHERE OrganizationType='Company'))");
            strSql.Append(" AND Users.RoleId = @RoleId");
            DataSet ds = new DataSet();
            SqlParameter[] parameters = {
					new SqlParameter("@RoleId", SqlDbType.Int)};
            parameters[0].Value = RoleId;

            ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            return ds;

        }

        /// <summary>
        /// --根据UserID，得到该User 所在Branch的其他LoanOfficer User 信息
        /// =====================================================================
        /// Modify by Rocky(2011/11/8)
        /// Get all loan officer base on current login user
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GetUserBranchOthersLoanOfficerUserInfo(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            //Modify by Rocky(2011/11/8)
            //strSql.Append(" Select * from (SELECT u.UserId,  u.LastName+','+u.FirstName AS UserName,b.BranchId,u.RoleId FROM Users u INNER JOIN GroupUsers gu ON u.UserId=gu.UserID");
            //  strSql.Append(" INNER JOIN Groups g ON gu.GroupID=g.GroupId INNER JOIN Branches b ON g.BranchID=b.BranchId ");
            //  strSql.Append(" WHERE g.OrganizationType='Branch' ) ub where ub.BranchId =(");

            //  strSql.Append(" SELECT b1.BranchId FROM Users u1 INNER JOIN GroupUsers gu1 ON u1.UserId=gu1.UserID ");
            //  strSql.Append(" INNER JOIN Groups g1 ON gu1.GroupID=g1.GroupId INNER JOIN Branches b1 ON g1.BranchID=b1.BranchId ");
            //  strSql.Append(" WHERE g1.OrganizationType='Branch' and u1.UserId =@UserId) and UserId !=@UserId and RoleId=3");
            strSql.Append("select *, LastName+', '+FirstName as FullName from dbo.lpfn_GetAllLoanOfficer(@UserId)");
            DataSet ds = new DataSet();
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int)};
            parameters[0].Value = UserID;

            ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            return ds;
        }

        /// <summary>
        /// adjust whether or not the user is company User
        /// Alex 2011-02-21
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsCompanyUserBase(int iUserID)
        {
            string sSql = "select * from Groups as a inner join GroupUsers as b on a.GroupId = b.GroupID inner join Users as c on b.UserID = c.UserId "
                        + "where OrganizationType='Company'  and b.UserID = " + iUserID;
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
        /// adjust whether or not the user is Region User
        /// Alex 2011-02-21
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsRegionUserBase(int iUserID)
        {
            string sSql = "select * from Groups as a inner join GroupUsers as b on a.GroupId = b.GroupID inner join Users as c on b.UserID = c.UserId "
                        + "where OrganizationType='Region'  and b.UserID = " + iUserID;
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
        /// adjust whether or not the user is Division User
        /// Alex 2011-02-21
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsDivisionUserBase(int iUserID)
        {
            string sSql = "select * from Groups as a inner join GroupUsers as b on a.GroupId = b.GroupID inner join Users as c on b.UserID = c.UserId "
                        + "where OrganizationType='Division'  and b.UserID = " + iUserID;
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
        /// adjust whether or not the user is Branch User
        /// Alex 2011-02-21
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsBranchUserBase(int iUserID)
        {
            string sSql = "select * from Groups as a inner join GroupUsers as b on a.GroupId = b.GroupID inner join Users as c on b.UserID = c.UserId "
                        + "where OrganizationType='Branch'  and b.UserID = " + iUserID;
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
        /// all the enable lo in the prospect lo's branch
        /// </summary>
        /// <param name="ContactId"></param>
        /// <returns></returns>
        public DataSet GetProspectLoanOfficers(int ContactId)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int) };
            parameters[0].Value = ContactId;

            DataSet ds = DbHelperSQL.RunProcedure("lpsp_GetProspectLoanOfficers", parameters, "ds");
            return ds;
        }

        /// <summary>
        /// 根据条件，得到LoanOfficer信息
        /// </summary>
        /// <param name="ContactId"></param>
        /// <returns></returns>
        public DataSet GetConditionLoanOfficers(string sConditionString)
        {
            string sSql = "select UserId ,LastName + ', '+ t.FirstName AS FULLNAME from Users t where t.UserEnabled=1 and t.RoleID=(select RoleId from Roles where Name='Loan Officer') ";
            if (sConditionString != "")
            {
                sSql += sConditionString;
            }

            DataSet ds = DbHelperSQL.Query(sSql);
            return ds;
        }


        #region neo

        /// <summary>
        /// get user info
        /// neo 2011-03-13
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetUserInfoBase(int iUserID)
        {
            string sSql = "SELECT  * from Users where UserId=" + iUserID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }


        public DataSet GetContactUserByContactID(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = "(SELECT U.Username, dbo.lpfn_GetUserName(U.UserId) AS FullName, R.Name AS RoleName, B.Name AS BranchName, CU.*, CASE CU.Enabled WHEN 'False' THEN 'No' ELSE 'Yes' END AS EnabledName"
                        + " FROM ContactUsers CU LEFT OUTER JOIN Users U ON CU.UserId = U.UserId"
                        + " LEFT OUTER JOIN GroupUsers GU ON U.UserId = GU.UserID"
                        + " LEFT OUTER JOIN Groups G ON GU.GroupID = G.GroupId"
                        + " LEFT OUTER JOIN Branches B ON G.BranchID = B.BranchId"
                        + " LEFT OUTER JOIN Roles R ON U.RoleId = R.RoleId) t";

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,2000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
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

        #endregion

        public DataTable GetAllUsers(int branchId)
        {
            StringBuilder sb = new StringBuilder(630);
            sb.AppendLine(@"SELECT  UserId ,");
            sb.AppendLine(@"        dbo.lpfn_GetUserName(UserId) AS UserName");
            sb.AppendLine(@"FROM    dbo.Users");
            sb.AppendLine(@"WHERE   UserEnabled = 1");
            sb.AppendLine(@"        AND UserId IN ( SELECT  UserId");
            sb.AppendLine(@"                        FROM    GroupUsers");
            sb.AppendLine(@"                        WHERE   GroupID IN ( SELECT GroupId");
            sb.AppendLine(@"                                             FROM   Groups");
            sb.AppendLine(@"                                             WHERE  BranchID IN ( SELECT");
            sb.AppendLine(@"                                                              BranchId");
            sb.AppendLine(@"                                                              FROM");

            if (branchId == -1)
            {
                sb.AppendLine(@"                                                              Branches ) ) )");
            }
            else
            {
                sb.AppendFormat(@"                                                              Branches WHERE BranchId={0} ) ) )", branchId);
            }
            sb.AppendLine(@"ORDER BY UserName ASC");
            return DbHelperSQL.Query(sb.ToString()).Tables[0];
        }

        public DataTable GetUserListByBranches_Executive(int iUserID)
        {

            StringBuilder sb = new StringBuilder(630);
            sb.AppendLine(@"SELECT  UserId ,");
            sb.AppendLine(@"        dbo.lpfn_GetUserName(UserId) AS UserName");
            sb.AppendLine(@"FROM    dbo.Users");
            sb.AppendLine(@"WHERE   UserEnabled = 1");
            sb.AppendLine(@"        AND UserId IN ( SELECT  UserId");
            sb.AppendLine(@"                        FROM    GroupUsers");
            sb.AppendLine(@"                        WHERE   GroupID IN ( SELECT GroupId");
            sb.AppendLine(@"                                             FROM   Groups");
            sb.AppendLine(@"                                             WHERE  BranchID IN ( SELECT");
            sb.AppendLine(@"                                                              BranchId");
            sb.AppendLine(@"                                                              FROM");
            sb.AppendFormat(@"                                                              dbo.lpfn_GetUserBranches_Executive({0}) ) ) )", iUserID);

            sb.AppendLine(@"ORDER BY UserName ASC");
            return DbHelperSQL.Query(sb.ToString()).Tables[0];
        }

        public DataTable GetUserListByUserBranches(int iUserID)
        {

            StringBuilder sb = new StringBuilder(630);
            sb.AppendLine(@"SELECT  UserId ,");
            sb.AppendLine(@"        dbo.lpfn_GetUserName(UserId) AS UserName");
            sb.AppendLine(@"FROM    dbo.Users");
            sb.AppendLine(@"WHERE   UserEnabled = 1");
            sb.AppendLine(@"        AND UserId IN ( SELECT  UserId");
            sb.AppendLine(@"                        FROM    GroupUsers");
            sb.AppendLine(@"                        WHERE   GroupID IN ( SELECT GroupId");
            sb.AppendLine(@"                                             FROM   Groups");
            sb.AppendLine(@"                                             WHERE  BranchID IN ( SELECT");
            sb.AppendLine(@"                                                              BranchId");
            sb.AppendLine(@"                                                              FROM");
            sb.AppendFormat(@"                                                              dbo.lpfn_GetUserBranches_UserList({0}) ) ) )", iUserID);

            sb.AppendLine(@"ORDER BY UserName ASC");
            return DbHelperSQL.Query(sb.ToString()).Tables[0];
        }

        public DataTable GetUserListByRole(string roleCondition)
        {
            StringBuilder sb = new StringBuilder(630);
            sb.AppendLine(@"SELECT  UserId ,");
            sb.AppendLine(@"        dbo.lpfn_GetUserName(UserId) AS UserName");
            sb.AppendLine(@"FROM    dbo.Users");
            sb.AppendLine(@"WHERE   UserEnabled = 1");
            sb.AppendLine(@"        AND RoleId IN ( SELECT  RoleId");
            sb.AppendLine(@"                        FROM    dbo.Roles");
            sb.AppendFormat(@"                        WHERE   Name IN ({0}))", roleCondition);
            sb.AppendLine(@"ORDER BY UserName ASC");

            return DbHelperSQL.Query(sb.ToString()).Tables[0];
        }
    }


}

