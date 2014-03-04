using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanTasks。
	/// </summary>
    public class LoanTasks : LoanTasksBase
	{
		public LoanTasks()
		{}

        #region neo

        /// <summary>
        /// get prerequisite task list
        /// neo 2010-11-17
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetPrerequisiteListBase(string sWhere)
        {
            string sSql = "select * from LoanTasks where 1=1 " + sWhere + " order by SequenceNumber";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get loan task list
        /// neo 2010-11-17
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetLoanTaskListBase(string sWhere)
        {
            string sSql = "select *, b.LastName +', '+ b.FirstName as OwnerName,dbo.lpfn_GetUserName(a.CompletedBy) as CompletedByName from LoanTasks as a left outer join Users as b on a.Owner = b.UserId where 1=1 " + sWhere + " order by SequenceNumber";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetLoanTaskListBase(int iTop, string sWhere, string sOrderBy)
        {
            string sSql = "select top(" + iTop + ") * from LoanTasks where 1=1 " + sWhere + " " + sOrderBy;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get loan task owners
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetLoanTaskOwersBase(int iLoanID)
        {
            //string sSql = "select b.UserID, b.LastName +', '+ b.FirstName as FullName from LoanTeam as a inner join Users as b on a.UserId = b.UserId "
            //            + "where a.FileId = " + iLoanID + " and b.UserEnabled = 1 "
            //            + "union "
            //            + "select e.UserID, e.LastName +', '+ e.FirstName as FullName from PointFiles as a inner join PointFolders as b on a.FolderId = b.FolderId "
            //            + "inner join Branches as c on b.BranchId = c.BranchId "
            //            + "inner join GroupUsers as d on c.GroupID = d.GroupId "
            //            + "inner join Users as e on d.UserID = e.UserId "
            //            + "where FileId = " + iLoanID + " and e.UserEnabled = 1 "
            //            + "union "
            //            + "select e.UserID, e.LastName +', '+ e.FirstName as FullName from PointFiles as a inner join PointFolders as b on a.FolderId = b.FolderId "
            //            + "inner join Branches as c on b.BranchId = c.BranchId "
            //            + "inner join BranchManagers as d on c.BranchId = d.BranchId "
            //            + "inner join Users as e on d.BranchMgrId = e.UserId "
            //            + "where FileId = " + iLoanID + " and e.UserEnabled = 1";
            string sSql = string.Format("select BranchID from Loans where FileId={0}", iLoanID);
            object obj = DbHelperSQL.GetSingle(sSql);
            int branchId = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
            
            if (branchId > 0)
                sSql = string.Format("select a.Userid, a.Username as FullName from dbo.lpvw_GetUserGroups a inner join Groups b on a.GroupID=b.GroupID where a.UserEnabled=1 and (b.OrganizationType='Company' OR a.BranchID={0})", branchId);
            else
                sSql = "select a.Userid, a.Username as FullName from dbo.lpvw_GetUserGroups a where a.UserEnabled=1 ";
            sSql += " GROUP BY a.Userid, a.Username";
            sSql += " Order by FullName ASC";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get task owners for reassign
        /// neo 2010-11-23
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetLoanTaskOwers_ReassignBase(int iLoanID)
        {
            //string sSql = "select b.UserID, b.LastName +', '+ b.FirstName as FullName from LoanTeam as a inner join Users as b on a.UserId = b.UserId "
            //            + "where a.FileId = " + iLoanID + " and b.UserEnabled = 1 "
            //            + "union "
            //            + "select e.UserID, e.LastName +', '+ e.FirstName as FullName from PointFiles as a inner join PointFolders as b on a.FolderId = b.FolderId "
            //            + "inner join Branches as c on b.BranchId = c.BranchId "
            //            + "inner join GroupUsers as d on c.GroupID = d.GroupId "
            //            + "inner join Users as e on d.UserID = e.UserId "
            //            + "where FileId = " + iLoanID + " and e.UserEnabled = 1 "
            //            + "union "
            //            + "select e.UserID, e.LastName +', '+ e.FirstName as FullName from PointFiles as a inner join PointFolders as b on a.FolderId = b.FolderId "
            //            + "inner join Branches as c on b.BranchId = c.BranchId "
            //            + "inner join BranchManagers as d on c.BranchId = d.BranchId "
            //            + "inner join Users as e on d.BranchMgrId = e.UserId "
            //            + "where FileId = " + iLoanID + " and e.UserEnabled = 1";

            string sSql = string.Format("select BranchID from Loans where FileId={0}", iLoanID);
            object obj = DbHelperSQL.GetSingle(sSql);
            int branchId = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;

            if (branchId > 0)
                sSql = string.Format("select a.Userid, a.Username as FullName from dbo.lpvw_GetUserGroups a inner join Groups b on a.GroupID=b.GroupID where a.UserEnabled=1 and (b.OrganizationType='Company' OR a.BranchID={0})", branchId);
            else
                sSql = "select a.Userid, a.Username as FullName from dbo.lpvw_GetUserGroups a where a.UserEnabled=1 ";
            sSql += " GROUP BY a.Userid, a.Username";
            sSql += " Order by FullName ASC";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get task owner for Task Owner Filter
        /// neo 2010-11-19
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetTaskOwnerListBase(int iLoanID)
        {
            string sSql = "select b.UserID, b.LastName +', '+ b.FirstName as FullName from LoanTeam as a inner join Users as b on a.UserId = b.UserId where a.FileId = " + iLoanID + " and b.UserEnabled = 1";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 检查某个loan下任务名称是否重复
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <param name="sTaskName"></param>
        /// <returns></returns>
        public bool IsLoanTaskExists_InsertBase(int iLoanID, string sTaskName)
        {
            string sSql = "select count(1) from LoanTasks where FileID=" + iLoanID + " and Name=@Name";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sTaskName);
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
        /// 检查某个loan下任务名称是否重复
        /// neo 2010-11-18
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <param name="iTaskID"></param>
        /// <param name="sTaskName"></param>
        /// <returns></returns>
        public bool IsLoanTaskExists_UpdateBase(int iLoanID, int iTaskID, string sTaskName)
        {
            string sSql = "select count(1) from LoanTasks where FileID=" + iLoanID + " and LoanTaskId != " + iTaskID + " and Name=@Name";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sTaskName);
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
        /// get next seq number
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public int GetNextSequenceNumBase(int iLoanID)
        {
            string sSql = "select ISNULL(MAX(SequenceNumber)+1, 1) from LoanTasks where FileId=" + iLoanID;
            return Convert.ToInt32(DbHelperSQL.ExecuteScalar(sSql));
        }

        /// <summary>
        /// insert task loan
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <param name="sTaskName"></param>
        /// <param name="sDueDate"></param>
        /// <param name="iOwerID"></param>
        /// <param name="iLoanStageID"></param>
        /// <param name="iPreTaskID"></param>
        /// <param name="iSeq"></param>
        /// <param name="iDaysToEstClose"></param>
        /// <param name="iDaysAfterPre"></param>
        /// <param name="iWarningEmailID"></param>
        /// <param name="iOverdueEmailID"></param>
        /// <param name="iCompletionEmailID"></param>
        public void InsertLoanTaskBase(int iLoanID, string sTaskName, string sDueDate, int iOwerID, int iLoanStageID, int iPreTaskID, int iSeq, int iDaysToEstClose, int iDaysAfterPre, int iWarningEmailID, int iOverdueEmailID, int iCompletionEmailID)
        {
            #region build sql - inser

            string sSql = "insert into LoanTasks (FileId,Name,Due,Completed,CompletedBy,LastModified,Created,Owner,LoanStageId,PrerequisiteTaskId,SequenceNumber,DaysDueFromEstClose,TemplTaskId,WflTemplId,DaysDueAfterPrerequisite,WarningEmailId,OverdueEmailId,CompletionEmailId) values (@FileId,@Name,@Due,@Completed,@CompletedBy,getdate(),getdate(),@Owner,@LoanStageId,@PrerequisiteTaskId,@SequenceNumber,@DaysDueFromEstClose,@TemplTaskId,@WflTemplId,@DaysDueAfterPrerequisite,@WarningEmailId,@OverdueEmailId,@CompletionEmailId)"
                        + ";select SCOPE_IDENTITY();";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@FileId", SqlDbType.Int, iLoanID);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sTaskName);
            if (sDueDate == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@Due", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@Due", SqlDbType.DateTime, DateTime.Parse(sDueDate));
            }

            DbHelperSQL.AddSqlParameter(SqlCmd, "@Completed", SqlDbType.DateTime, DBNull.Value);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@CompletedBy", SqlDbType.Int, DBNull.Value);
            //DbHelperSQL.AddSqlParameter(SqlCmd, "@LastModified", SqlDbType.DateTime, iLoginUser);
            //DbHelperSQL.AddSqlParameter(SqlCmd, "@Created", SqlDbType.DateTime, Created);
            if (iOwerID == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@Owner", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@Owner", SqlDbType.Int, iOwerID);
            }
            DbHelperSQL.AddSqlParameter(SqlCmd, "@LoanStageId", SqlDbType.Int, iLoanStageID);
            if (iPreTaskID == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@PrerequisiteTaskId", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@PrerequisiteTaskId", SqlDbType.Int, iPreTaskID);
            }
            DbHelperSQL.AddSqlParameter(SqlCmd, "@SequenceNumber", SqlDbType.SmallInt, iSeq);
            if (iDaysToEstClose == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueFromEstClose", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueFromEstClose", SqlDbType.SmallInt, iDaysToEstClose);
            }

            DbHelperSQL.AddSqlParameter(SqlCmd, "@TemplTaskId", SqlDbType.Int, DBNull.Value);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@WflTemplId", SqlDbType.Int, DBNull.Value);

            if (iDaysAfterPre == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueAfterPrerequisite", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueAfterPrerequisite", SqlDbType.SmallInt, iDaysAfterPre);
            }
            if (iWarningEmailID == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@WarningEmailId", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@WarningEmailId", SqlDbType.Int, iWarningEmailID);
            }
            if (iOverdueEmailID == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@OverdueEmailId", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@OverdueEmailId", SqlDbType.Int, iOverdueEmailID);
            }
            if (iCompletionEmailID == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@CompletionEmailId", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@CompletionEmailId", SqlDbType.Int, iCompletionEmailID);
            }

            #endregion

            #region build sql - CreateTaskAlert

            SqlCommand SqlCmd2 = new SqlCommand("lpsp_CreateTaskAlerts");
            SqlCmd2.CommandType = CommandType.StoredProcedure;

            #endregion

            #region build sql - update loan stage

            string sSql3 = "UPDATE LoanStages SET Completed=NULL WHERE LoanStageId=@LoanStageId";
            SqlCommand SqlCmd3 = new SqlCommand(sSql3);
            DbHelperSQL.AddSqlParameter(SqlCmd3, "@LoanStageId", SqlDbType.Int, iLoanStageID);

            #endregion

            #region build sql - update loan

            string sSql4 = "UPDATE Loans SET CurrentStage=dbo.lpfn_GetCurrentStage(@FileId)	WHERE FileId=@FileId";
            SqlCommand SqlCmd4 = new SqlCommand(sSql4);
            DbHelperSQL.AddSqlParameter(SqlCmd4, "@FileId", SqlDbType.Int, iLoanID);

            #endregion

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                int iNewTaskID = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd, SqlTrans));

                DbHelperSQL.AddSqlParameter(SqlCmd2, "@LoanTaskId", iNewTaskID);
                DbHelperSQL.ExecuteNonQuery(SqlCmd2, SqlTrans);

                DbHelperSQL.ExecuteNonQuery(SqlCmd3, SqlTrans);
                DbHelperSQL.ExecuteNonQuery(SqlCmd4, SqlTrans);

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
        /// get loan task info
        /// neo 2010-11-18
        /// </summary>
        /// <param name="iTaskID"></param>
        /// <returns></returns>
        public DataTable GetLoanTaskInfoBase(int iTaskID)
        {
            string sSql = "select *, b.LastName +', '+ b.FirstName as OwnerName from LoanTasks as a left outer join Users as b on a.Owner = b.UserId where a.LoanTaskID = " + iTaskID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// update loan task info
        /// neo 2010-11-08
        /// </summary>
        /// <param name="iTaskID"></param>
        /// <param name="sTaskName"></param>
        /// <param name="sDueDate"></param>
        /// <param name="iLoginUserID"></param>
        /// <param name="iOwerID"></param>
        /// <param name="iNewLoanStageID"></param>
        /// <param name="iPrerequisiteTaskID"></param>
        /// <param name="iDaysToEstClose"></param>
        /// <param name="iDaysAfterPrerequisite"></param>
        /// <param name="iWarningEmailID"></param>
        /// <param name="iOverdueEmailID"></param>
        /// <param name="iCompletionEmailID"></param>
        /// <param name="iOldLoanStageID"></param>
        /// <returns></returns>
        //public bool UpdateLoanTaskBase(int iTaskID, string sTaskName, string sDueDate, int iLoginUserID, int iOwerID, int iNewLoanStageID, int iPrerequisiteTaskID, int iDaysToEstClose, int iDaysAfterPrerequisite, int iWarningEmailID, int iOverdueEmailID, int iCompletionEmailID, int iOldLoanStageID)
        //{
        //    bool bIsSuccess = DAL.WorkflowManager.UpdateTask(iTaskID, sTaskName, sDueDate, iLoginUserID, iOwerID, iNewLoanStageID, iPrerequisiteTaskID, iDaysToEstClose, iDaysAfterPrerequisite, iWarningEmailID, iOverdueEmailID, iCompletionEmailID, iOldLoanStageID);

        //    return bIsSuccess;
        //}

        /// <summary>
        /// delete loan task
        /// neo 2010-11-18
        /// </summary>
        /// <param name="iLoanTaskID"></param>
        public void DeleteLoanTaskBase(int iLoanTaskID)
        {
            string sSql = "delete from LoanTasks where LoanTaskId=" + iLoanTaskID;
            DbHelperSQL.ExecuteNonQuery(sSql);
        }

        /// <summary>
        /// is a prerequisite task?
        /// neo 2010-11-22
        /// </summary>
        /// <param name="iLoanTaskID"></param>
        /// <returns></returns>
        public bool IsPrerequisiteBase(int iLoanTaskID)
        {
            string sSql = "select COUNT(1) from LoanTasks where PrerequisiteTaskId=" + iLoanTaskID;
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
        /// get user task list which due today
        /// neo 2012-09-12
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetUserTaskDueToday(int iUserID) 
        {
            string sSql = "select *,dbo.lpfn_GetBorrower(FileId) as Borrower from LoanTasks where Owner=" + iUserID + " and Due is not null and DATEDIFF(DAY,Due,GETDATE())<=0";

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// CR48
        /// </summary>
        /// <param name="FileId"></param>
        /// <param name="LoanTaskId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string iCalendarToString(int FileId, int LoanTaskId, int UserId)
        {
            #region get data

            // Users
            LPWeb.DAL.Users UserMgr = new Users();
            LPWeb.Model.Users UserModel = UserMgr.GetModel(UserId);
            if (UserModel == null)
            {
                return string.Empty;
            }

            // LoanTasks
            LoanTasks LoanTaskMgr = new LoanTasks();
            DataTable LoanTaskInfo = LoanTaskMgr.GetLoanTaskInfoBase(LoanTaskId);
            if (LoanTaskInfo.Rows.Count == 0)
            {
                return string.Empty;
            }

            string sDue = string.Empty;
            if (LoanTaskInfo.Rows[0]["Due"] != DBNull.Value)
            {
                sDue = Convert.ToDateTime(LoanTaskInfo.Rows[0]["Due"]).ToString("MM/dd/yyyy");
            }

            string sTaskName = LoanTaskInfo.Rows[0]["Name"].ToString();

            string sPriority = string.Empty;

            // Borrower
            Contacts ContactMgr = new Contacts();
            string sBorrowerName = ContactMgr.GetBorrower(FileId);
            
            // Loans
            
            Loans LoanManager = new Loans();
            DataTable LoanInfo = LoanManager.GetLoanInfoBase(FileId);
            if (LoanInfo.Rows.Count == 0)
            {
                return string.Empty;
            }
            string sPropertyAddress = LoanInfo.Rows[0]["PropertyAddr"].ToString();
            string sPurpose = LoanInfo.Rows[0]["Purpose"].ToString();
            string sLoanAmount = LoanInfo.Rows[0]["LoanAmount"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanInfo.Rows[0]["LoanAmount"]).ToString("n0");

            string sDESCRIPTION = sPropertyAddress + ", " + sPurpose + ", $" + sLoanAmount;

            // Others
            string sUID = DateTime.Now.ToUniversalTime().ToString("MM/dd/yyyy") + "@pulsedashboard.net";
            string sDTSTAMP = DateTime.Now.ToUniversalTime().ToString("MM/dd/yyyy");

            #endregion

            #region template

            string sTemp = @"BEGIN:VCALENDAR
VERSION:2.0
METHOD:PUBLISH
BEGIN:VEVENT
ORGANIZER:MAILTO:[UserEmail]
DTSTART:[LoanTasks.Due]
DTEND:[LoanTasks.Due]
LOCATION:[BorrowerName] 
UID:[UID]
DTSTAMP:[DTSTAMP]
SUMMARY:[LoanTasks.Name]
DESCRIPTION:[DESCRIPTION]
PRIORITY:[Priority]
CLASS:PRIVATE";

            string sTemp2 = Environment.NewLine + @"BEGIN:VALARM
TRIGGER:-PT[Users.TaskReminder]M
ACTION:DISPLAY
DESCRIPTION:Reminder
END:VALARM";

            StringBuilder sbTemp = new StringBuilder(sTemp);

            if (UserModel.RemindTaskDue == true)
            {
                sbTemp.AppendLine(sTemp2);
            }

            sbTemp.AppendLine("END:VEVENT");
            sbTemp.AppendLine("END:VCALENDAR");

            #endregion

            #region replace

            // [UserEmail]
            sbTemp.Replace("[UserEmail]", UserModel.EmailAddress);

            // [LoanTasks.Due]
            sbTemp.Replace("[LoanTasks.Due]", sDue);

            // [BorrowerName]
            sbTemp.Replace("[BorrowerName]", sBorrowerName);

            // [UID]
            sbTemp.Replace("[UID]", sUID);

            // [DTSTAMP]
            sbTemp.Replace("[DTSTAMP]", sDTSTAMP);

            // [LoanTasks.Name]
            sbTemp.Replace("[LoanTasks.Name]", sTaskName);

            // [DESCRIPTION]
            sbTemp.Replace("[DESCRIPTION]", sDESCRIPTION);

            // [Priority]
            sbTemp.Replace("[Priority]", sPriority);

            // [Users.TaskReminder]
            sbTemp.Replace("[Users.TaskReminder]", UserModel.TaskReminder.ToString());

            #endregion


            return sbTemp.ToString();
        }

        /// <summary>
        /// CR48 
        /// </summary>
        public string CreateLoanTasks(int FileId, string TasksName, int CompletedBy, DateTime Due, string time, int Owner, DateTime Completed, bool taskCompleted)
        {

            return CreateLoanTasks(FileId, TasksName, CompletedBy, Due, time, Owner, Completed, taskCompleted, "");
        }


        /// <summary>
        /// CF54 add notes
        /// </summary>
        /// <returns></returns>
        public string CreateLoanTasks(int FileId, string TasksName, int CompletedBy, DateTime Due, string time, int Owner
            , DateTime Completed, bool taskCompleted, string notes)
         {
             SqlConnection SqlConn = null;
             SqlTransaction SqlTrans = null;

             string msg = "error";
             try
             {
                 SqlConn = LPWeb.DAL.DbHelperSQL.GetOpenConnection();
                 SqlTrans = SqlConn.BeginTransaction();
                 string sql = " Select Top 1 LoanStageId from [LoanStages] where Completed IS NULL and FileId=" + FileId + " order by SequenceNumber asc";

                 DataSet ds  = DbHelperSQL.Query(sql);
                 int sqlc = 0;
                 if (ds.Tables[0].Rows.Count > 0)
                 {
                     int LoanSId = int.Parse(ds.Tables[0].Rows[0]["LoanStageId"].ToString());

                     string sqltaskName = "";
                     if (taskCompleted == false)
                     {
                         string dueStr = (Due == DateTime.MinValue ? "NULL" : ("'" + Due + "'"));

                         sqltaskName = "insert into [LoanTasks]([FileId],[LoanStageId],[Name],[Owner],[Created],[Due],[DueTime])VALUES(" + FileId + "," + LoanSId + ",'" + TasksName + "'," + Owner + ",'" + DateTime.Now + "'," + dueStr + ",CONVERT(VARCHAR(10),'" + time + "',108)) SELECT  @@IDENTITY  as  newIDValue ";
                     }
                     else
                     {
                         sqltaskName = "insert into [LoanTasks]([FileId],[LoanStageId],[Name],[Owner],[Created],[Completed],[CompletedBy],[Due],[DueTime])VALUES(" + FileId + "," + LoanSId + ",'" + TasksName + "'," + Owner + ",'" + DateTime.Now + "','" + Completed + "'," + CompletedBy + ",'" + Completed + "','" + time + "') SELECT  @@IDENTITY  as  newIDValue ";
                     }
                    

                     int myDr = int.Parse(DbHelperSQL.ExecuteScalar(sqltaskName).ToString());

                     if (myDr>0)
                     {
                         sqlc = myDr;

                     }
                 }
                 //CR54
                 if (sqlc > 0 && !string.IsNullOrEmpty(notes))  // sqlc :  LoanTasksId  
                 {
                     string sqlNote = string.Format(@"INSERT INTO [LP].[dbo].[LoanNotes] ([FileId],[Created],[Sender],[Note],[Exported],[ExternalViewing],[LoanTaskId])VALUES({0},'{1}','{2}','{3}',0,0,{4})"
                         , FileId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "", notes, sqlc);

                     var noteResult = DbHelperSQL.ExecuteNonQuery(sqlNote);

                 }

                 if (taskCompleted == true)
                 {
                     #region
                     int emailTemplate = 0;
                     StringBuilder strSql = new StringBuilder();
                     strSql.Append("lpsp_CompleteTask");

                     SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4),
					new SqlParameter("@userId", SqlDbType.Int,4),
					new SqlParameter("@emailTemplate", SqlDbType.Int,4)
                                        };
                     parameters[0].Value = sqlc;
                     parameters[1].Value = Owner;
                     parameters[2].Direction = ParameterDirection.Output;

                     int rowsAffected = 0;

                     int returnValue = -1;
                     returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

                     if (parameters[2].Value != DBNull.Value)
                     {
                         emailTemplate = Convert.ToInt32(parameters[2].Value);
                     }
                     #endregion

                 }
                 else
                 {

                     #region
                     StringBuilder strSql = new StringBuilder();
                     strSql.Append("lpsp_AddTask");

                     SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4),
					
                                        };
                     parameters[0].Value = sqlc;

                     int rowsAffected = 0;

                     int returnValue = -1;
                     returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);
                     #endregion
                 }

                 msg = "Success";
                 SqlTrans.Commit();
             
             }
             catch
             {
                 SqlTrans.Rollback();
                 msg = "error";
             }
             finally
             {
                 if (SqlConn != null)
                 {
                     SqlConn.Close();
                 }
             }
             return msg;
         }

        /// <summary>
        /// get data for loan activities list
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="count"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string strTableName = "LoanTasks";
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
        /// 
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public void GetSnoozePopup(string minute,string LoanTaskId)
        {
            string due="convert(char(8),Dateadd(minute,"+minute+",convert(datetime,(convert(varchar(11),Due,120)+' '+convert(varchar(10),DueTime,120)))),108)";
            string sSql = "update LoanTasks  set DueTime=" + due + " where 1=1 and LoanTaskId=" + LoanTaskId;

             int i = DbHelperSQL.ExecuteNonQuery(sSql);
        }


        public string GetLoanTasks(int minutes,int useId)
        {
            int i = 0;
            //string sSql = "select LoanTaskId from LoanTasks  where 1=1 and due=convert(varchar(10),getdate(),120) and DueTime= convert(varchar(5),dateadd(mi,-" + minutes + ",'2012-09-27 10:19'),108) and Owner=" + useId;

            string sSql = "select LoanTaskId from LoanTasks  where 1=1 and due=convert(varchar(10),getdate(),120) and DueTime= convert(varchar(5),dateadd(mi,-" + minutes + ",getdate()),108)";
         
            DataTable dt = DbHelperSQL.ExecuteDataTable(sSql);

            if (dt.Rows.Count>0)
            {
                i = dt.Rows.Count;
            }
            return i.ToString();
        }

        /// <summary>
        /// get task reminder
        /// neo 2012-11-24
        /// </summary>
        /// <param name="iUserId"></param>
        /// <param name="iMinutes"></param>
        /// <returns></returns>
        public DataTable GetReminderTaskList(int iUserId, int iMinutes) 
        {
            string sSql = @"select dbo.lpfn_GetBorrower(a.FileId) as BorrowerName, 
a.Name as TaskName, dbo.lpfn_GetUserName(a.Owner) as OwnerName, 
CONVERT(varchar, a.Due, 101) + ' ' + CONVERT(varchar(5), a.DueTime, 108) as DueDateTime, * 
from LoanTasks as a 
where a.Owner = {0} and a.Completed is null and a.DueTime is not null and a.Due is not null
and DATEDIFF(minute, GETDATE(), (a.Due + a.DueTime))<={1}
order by a.SequenceNumber ";

            sSql = string.Format(sSql, iUserId.ToString(), iMinutes.ToString());

            return DbHelperSQL.ExecuteDataTable(sSql);

        }

        /// <summary>
        /// whether or not show task reminder
        /// neo 2012-11-24
        /// </summary>
        /// <param name="iUserId"></param>
        /// <param name="iMinutes"></param>
        /// <returns></returns>
        public bool ShowTaskReminder(int iUserId, int iMinutes)
        {
            string sSql = @"select count(1) 
from LoanTasks as a 
where a.Owner = {0} and a.Completed is null and a.DueTime is not null and a.Due is not null
and DATEDIFF(minute, GETDATE(), (a.Due + a.DueTime))<={1}";

            sSql = string.Format(sSql, iUserId.ToString(), iMinutes.ToString());

            int iCount = Convert.ToInt32(DbHelperSQL.ExecuteScalar(sSql));

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
        /// snooze reminder task
        /// neo 2012-11-25
        /// </summary>
        /// <param name="sTaskIDs"></param>
        /// <param name="iMinutes"></param>
        public void SnoozeTask(string sTaskIDs, int iMinutes) 
        {
            string sSql = "update LoanTasks set DueTime=DATEADD(minute, " + iMinutes + ", DueTime),LastModified=getdate() where LoanTaskId in (" + sTaskIDs + ")";
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
        }


        #endregion

    }
}

