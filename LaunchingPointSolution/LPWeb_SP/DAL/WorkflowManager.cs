using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using LPWeb.Model;

namespace LPWeb.DAL
{
    /// <summary>
    /// The Workflow Manager
    /// </summary>
    public partial class WorkflowManager
    {
        #region TaskAPI

        /// <summary>
        /// Gets the task due date.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        public static DateTime GetTaskDueDate(int loanTaskId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [dbo].[lpfn_GetTaskDueDate](@LoanTaskId)");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4)};
            parameters[0].Value = loanTaskId;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                return Convert.ToDateTime(obj);
            }
        }

        /// <summary>
        /// Gets the task icon.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        public static string GetTaskIcon(int loanTaskId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [dbo].[lpfn_GetTaskIcon](@LoanTaskId)");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4)};
            parameters[0].Value = loanTaskId;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return Convert.ToString(obj);
            }
        }
        /// <summary>
        /// Removes the task alerts.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        public static bool RemoveTaskAlerts(int loanTaskId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_RemoveTaskAlerts");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4)};
            parameters[0].Value = loanTaskId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }

        /// <summary>
        /// Creates the task alerts.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        public static bool CreateTaskAlerts(int loanTaskId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_CreateTaskAlerts");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4)};
            parameters[0].Value = loanTaskId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }
        /// <summary>
        /// Gets the red tasks.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        public static int GetRedTasks(int loanStageId)
        {
            return GetStatusTasks(loanStageId, "TaskRed.png");
        }
        /// <summary>
        /// Gets the red tasks.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        private static int GetStatusTasks(int loanStageId, string taskIconName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select COUNT(1) from dbo.LoanTasks where LoanStageId=@LoanStageId and [dbo].[lpfn_GetTaskIcon](LoanTaskId)=@taskIconName");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanStageId", SqlDbType.Int,4),
					new SqlParameter("@LoanStageId", SqlDbType.NVarChar,20)};
            parameters[0].Value = loanStageId;
            parameters[1].Value = taskIconName;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// Gets the yellow tasks.
        /// </summary>
        /// <param name="loanStageId">The loan stage id.</param>
        /// <returns></returns>
        public static int GetYellowTasks(int loanStageId)
        {
            return GetStatusTasks(loanStageId, "TaskYellow.png");
        }

        /// <summary>
        /// Completes the task.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="emailTemplate">The email template.</param>
        /// <returns></returns>
        public static bool CompleteTask(int loanTaskId, int userId, ref int emailTemplate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_CompleteTask");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4),
					new SqlParameter("@userId", SqlDbType.Int,4),
					new SqlParameter("@emailTemplate", SqlDbType.Int,4)
                                        };
            parameters[0].Value = loanTaskId;
            parameters[1].Value = userId;
            parameters[2].Direction = ParameterDirection.Output;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            if (parameters[2].Value != DBNull.Value)
            {
                emailTemplate = Convert.ToInt32(parameters[2].Value);
            }
            return returnValue == 0;
        }
        /// <summary>
        /// Uns the complete task.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public static bool UnCompleteTask(int loanTaskId, int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_UnCompleteTask");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4),
					new SqlParameter("@userId", SqlDbType.Int,4)
                                        };
            parameters[0].Value = loanTaskId;
            parameters[1].Value = userId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }

        /// <summary>
        /// Adds the task.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        public static bool AddTask(int loanTaskId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_AddTask");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4)
                                        };
            parameters[0].Value = loanTaskId;
            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }
        /// <summary>
        /// Adds the task.
        /// </summary>
        /// <param name="loanTask">The LPWeb.Model.LoanTasks object.</param>
        /// <returns></returns>
        public static int AddTask(LPWeb.Model.LoanTasks loanTask)
        {
            if (loanTask == null || loanTask.FileId <= 0 || loanTask.Name.Trim() == string.Empty || loanTask.LoanStageId <= 0)
                throw new Exception("Invalid LoanTask parameter, FileId, LoanStageId or Task Name is not specified.");
            #region build sql

            SqlParameter[] parameters = {
                new SqlParameter("@LoanTaskId", SqlDbType.Int), //0
                new SqlParameter("@FileId", SqlDbType.Int), //1
                new SqlParameter("@Name", SqlDbType.NVarChar, 255), //2
                new SqlParameter("@Due", SqlDbType.DateTime), //3
                new SqlParameter("@Completed", SqlDbType.DateTime), //4
                new SqlParameter("@CompletedBy", SqlDbType.Int), //5
                new SqlParameter("@LastModified", SqlDbType.DateTime), //6
                new SqlParameter("@Created", SqlDbType.DateTime), //7
                new SqlParameter("@Owner", SqlDbType.Int), //8                
                new SqlParameter("@LoanStageId", SqlDbType.Int), //9
                new SqlParameter("@PrerequisiteTaskId", SqlDbType.Int), //10
                new SqlParameter("@DaysDueFromEstClose", SqlDbType.SmallInt), //11
                new SqlParameter("@TemplTaskId", SqlDbType.Int), //12
                new SqlParameter("@WflTemplId", SqlDbType.Int), //13
                new SqlParameter("@DaysDueAfterPrerequisite", SqlDbType.SmallInt), //14
                new SqlParameter("@WarningEmailId", SqlDbType.Int), //15
                new SqlParameter("@OverdueEmailId", SqlDbType.Int), //16
                new SqlParameter("@CompletionEmailId", SqlDbType.Int), //17
                new SqlParameter("@SequenceNumber", SqlDbType.SmallInt), //18
                new SqlParameter("@DaysFromCreation", SqlDbType.SmallInt), //19
                new SqlParameter("@ExternalViewing",SqlDbType.Bit),  //20
                new SqlParameter("@DaysDueAfterPrevStage", SqlDbType.SmallInt),  //21
                new SqlParameter("@DueTime", SqlDbType.Time), //22
                new SqlParameter("@Desc", SqlDbType.NVarChar)    //23
                                        };

            parameters[0].Value = loanTask.LoanTaskId <= 0 ? null : loanTask.LoanTaskId.ToString();
            parameters[1].Value = loanTask.FileId.ToString();
            parameters[2].Value = loanTask.Name.Trim();
            parameters[3].Value = loanTask.Due == DateTime.MinValue ? null : loanTask.Due;
            parameters[4].Value = null;    // completed
            parameters[5].Value = null;   // completedBy
            parameters[6].Value = loanTask.LastModified == DateTime.MinValue ? null : loanTask.LastModified;
            parameters[7].Value = null;     // created
            parameters[8].Value = loanTask.Owner <= 0 ? null : loanTask.Owner.ToString();
            parameters[9].Value = loanTask.LoanStageId <= 0 ? null : loanTask.LoanStageId.ToString();
            parameters[10].Value = loanTask.PrerequisiteTaskId <= 0 ? null : loanTask.PrerequisiteTaskId.ToString();
            parameters[11].Value = loanTask.DaysDueFromEstClose < -365 ? null : loanTask.DaysDueFromEstClose.ToString();
            parameters[12].Value = null;     //TemplTaskId
            parameters[13].Value = null;     //WflTemplId            
            parameters[14].Value = loanTask.DaysDueAfterPrerequisite <= 0 ? null : loanTask.DaysDueAfterPrerequisite.ToString();
            parameters[15].Value = loanTask.WarningEmailId <= 0 ? null : loanTask.WarningEmailId.ToString();
            parameters[16].Value = loanTask.OverdueEmailId <= 0 ? null : loanTask.OverdueEmailId.ToString();
            parameters[17].Value = loanTask.CompletionEmailId <= 0 ? null : loanTask.CompletionEmailId.ToString();
            parameters[18].Value = loanTask.SequenceNumber.ToString();            
            parameters[19].Value = loanTask.DaysFromCreation < -365 ? null : loanTask.DaysFromCreation.ToString();
            parameters[20].Value = loanTask.ExternalViewing.ToString();
            parameters[21].Value = loanTask.DaysDueAfterPrevStage < -365 ? null : loanTask.DaysDueAfterPrevStage.ToString();
            parameters[22].Value = loanTask.DueTime == null ? null : loanTask.DueTime;  // DueTime
            parameters[23].Value = loanTask.Desc.Trim();  // Desc

            #endregion
            int loanTaskId = -1, rowsAffected = 0;
            loanTaskId = DbHelperSQL.RunProcedure("dbo.[LoanTasks_Save]", parameters, out rowsAffected);
            if (loanTaskId > 0)
            {
                SqlParameter[] param2 = {
                    new SqlParameter("@LoanTaskId", SqlDbType.Int)
                                     };
                param2[0].Value = loanTaskId;
                DbHelperSQL.RunProcedure("dbo.lpsp_AddTask", param2, out rowsAffected);
            }
            return loanTaskId;
        }

        public static int AddTask_Lead(LPWeb.Model.LoanTasks loanTask)
        {           
            #region build sql

            SqlParameter[] parameters = {
                new SqlParameter("@LoanTaskId", SqlDbType.Int), //0
                new SqlParameter("@FileId", SqlDbType.Int), //1
                new SqlParameter("@Name", SqlDbType.NVarChar, 255), //2
                new SqlParameter("@Due", SqlDbType.DateTime), //3
                new SqlParameter("@Completed", SqlDbType.DateTime), //4
                new SqlParameter("@CompletedBy", SqlDbType.Int), //5
                new SqlParameter("@LastModified", SqlDbType.DateTime), //6
                new SqlParameter("@Created", SqlDbType.DateTime), //7
                new SqlParameter("@Owner", SqlDbType.Int), //8                
                new SqlParameter("@LoanStageId", SqlDbType.Int), //9
                new SqlParameter("@PrerequisiteTaskId", SqlDbType.Int), //10
                new SqlParameter("@DaysDueFromEstClose", SqlDbType.SmallInt), //11
                new SqlParameter("@TemplTaskId", SqlDbType.Int), //12
                new SqlParameter("@WflTemplId", SqlDbType.Int), //13
                new SqlParameter("@DaysDueAfterPrerequisite", SqlDbType.SmallInt), //14
                new SqlParameter("@WarningEmailId", SqlDbType.Int), //15
                new SqlParameter("@OverdueEmailId", SqlDbType.Int), //16
                new SqlParameter("@CompletionEmailId", SqlDbType.Int), //17
                new SqlParameter("@SequenceNumber", SqlDbType.SmallInt), //18
                new SqlParameter("@DaysFromCreation", SqlDbType.SmallInt), //19
                new SqlParameter("@ExternalViewing",SqlDbType.Bit),  //20
                new SqlParameter("@DaysDueAfterPrevStage", SqlDbType.SmallInt),  //21
                new SqlParameter("@DueTime", SqlDbType.Time), //22
                new SqlParameter("@Desc", SqlDbType.NVarChar)    //23
                                        };

            parameters[0].Value = loanTask.LoanTaskId <= 0 ? null : loanTask.LoanTaskId.ToString();
            parameters[1].Value = loanTask.FileId.ToString();
            parameters[2].Value = loanTask.Name.Trim();
            parameters[3].Value = loanTask.Due == DateTime.MinValue ? null : loanTask.Due;
            parameters[4].Value = null;    // completed
            parameters[5].Value = null;   // completedBy
            parameters[6].Value = loanTask.LastModified == DateTime.MinValue ? null : loanTask.LastModified;
            parameters[7].Value = null;     // created
            parameters[8].Value = loanTask.Owner <= 0 ? null : loanTask.Owner.ToString();
            parameters[9].Value = loanTask.LoanStageId <= 0 ? null : loanTask.LoanStageId.ToString();
            parameters[10].Value = loanTask.PrerequisiteTaskId <= 0 ? null : loanTask.PrerequisiteTaskId.ToString();
            parameters[11].Value = loanTask.DaysDueFromEstClose < -365 ? null : loanTask.DaysDueFromEstClose.ToString();
            parameters[12].Value = null;     //TemplTaskId
            parameters[13].Value = null;     //WflTemplId            
            parameters[14].Value = loanTask.DaysDueAfterPrerequisite <= 0 ? null : loanTask.DaysDueAfterPrerequisite.ToString();
            parameters[15].Value = loanTask.WarningEmailId <= 0 ? null : loanTask.WarningEmailId.ToString();
            parameters[16].Value = loanTask.OverdueEmailId <= 0 ? null : loanTask.OverdueEmailId.ToString();
            parameters[17].Value = loanTask.CompletionEmailId <= 0 ? null : loanTask.CompletionEmailId.ToString();
            parameters[18].Value = loanTask.SequenceNumber.ToString();
            parameters[19].Value = loanTask.DaysFromCreation < -365 ? null : loanTask.DaysFromCreation.ToString();
            parameters[20].Value = loanTask.ExternalViewing.ToString();
            parameters[21].Value = loanTask.DaysDueAfterPrevStage < -365 ? null : loanTask.DaysDueAfterPrevStage.ToString();
            parameters[22].Value = loanTask.DueTime == null ? null : loanTask.DueTime;  // DueTime
            parameters[23].Value = loanTask.Desc.Trim();  // Desc

            #endregion
            int loanTaskId = -1, rowsAffected = 0;
            loanTaskId = DbHelperSQL.RunProcedure("dbo.[LoanTasks_Save]", parameters, out rowsAffected);
            if (loanTaskId > 0)
            {
                SqlParameter[] param2 = {
                    new SqlParameter("@LoanTaskId", SqlDbType.Int)
                                     };
                param2[0].Value = loanTaskId;
                DbHelperSQL.RunProcedure("dbo.lpsp_AddTask", param2, out rowsAffected);
            }
            return loanTaskId;
        }

        /// <summary>
        /// Add task
        /// neo 2011-04-11
        /// </summary>
        /// <param name="loanTask"></param>
        /// <param name="sDaysToEstClose"></param>
        /// <param name="sDaysAfterCreation"></param>
        /// <param name="sDaysDueAfterRrerequisite"></param>
        /// <returns></returns>
        public static int AddTask(LPWeb.Model.LoanTasks loanTask, string sDaysToEstClose, string sDaysAfterCreation, string sDaysDueAfterRrerequisite, string DaysDueAfterPrevStage)
        {
            if (loanTask == null || loanTask.FileId <= 0 || loanTask.Name.Trim() == string.Empty || loanTask.LoanStageId <= 0)
                throw new Exception("Invalid LoanTask parameter, FileId, LoanStageId or Task Name is not specified.");
            #region build sql

            SqlParameter[] parameters = {
                new SqlParameter("@LoanTaskId", SqlDbType.Int), //0
                new SqlParameter("@FileId", SqlDbType.Int), //1
                new SqlParameter("@Name", SqlDbType.NVarChar, 255),  //2
                new SqlParameter("@Due", SqlDbType.DateTime),  //3            
                new SqlParameter("@Completed", SqlDbType.DateTime),  //4
                new SqlParameter("@CompletedBy", SqlDbType.Int),  //5
                new SqlParameter("@LastModified", SqlDbType.DateTime),  //6
                new SqlParameter("@Created", SqlDbType.DateTime),  //7
                new SqlParameter("@Owner", SqlDbType.Int),  //8
                new SqlParameter("@LoanStageId", SqlDbType.Int),  //9
                new SqlParameter("@PrerequisiteTaskId", SqlDbType.Int),  //10
                new SqlParameter("@DaysDueFromEstClose", SqlDbType.SmallInt),  //11
                new SqlParameter("@TemplTaskId", SqlDbType.Int),  //12
                new SqlParameter("@WflTemplId", SqlDbType.Int),  //13
                new SqlParameter("@DaysDueAfterPrerequisite", SqlDbType.SmallInt),  //14
                new SqlParameter("@WarningEmailId", SqlDbType.Int),  //15
                new SqlParameter("@OverdueEmailId", SqlDbType.Int),  //16
                new SqlParameter("@CompletionEmailId", SqlDbType.Int),  //17
                new SqlParameter("@SequenceNumber", SqlDbType.SmallInt),  //18              
                new SqlParameter("@DaysFromCreation", SqlDbType.SmallInt),  //19
                new SqlParameter("@ExternalViewing",SqlDbType.Bit),  //20
                new SqlParameter("@DaysDueAfterPrevStage", SqlDbType.SmallInt),  //21
                new SqlParameter("@DueTime", SqlDbType.Time), //22
                new SqlParameter("@Desc", SqlDbType.NVarChar)    //23
                                        };

            parameters[0].Value = loanTask.LoanTaskId <= 0 ? null : loanTask.LoanTaskId.ToString();
            parameters[1].Value = loanTask.FileId.ToString();
            parameters[2].Value = loanTask.Name.Trim();
            parameters[3].Value = loanTask.Due == DateTime.MinValue ? null : loanTask.Due;
            parameters[4].Value = loanTask.Completed == DateTime.MinValue ? null : loanTask.Completed;
            parameters[5].Value = loanTask.CompletedBy <= 0 ? null : loanTask.CompletedBy;
            parameters[6].Value = loanTask.LastModified == DateTime.MinValue ? null : loanTask.LastModified;
            parameters[7].Value = loanTask.Created == DateTime.MinValue ? null : loanTask.Created;
            parameters[8].Value = loanTask.Owner <= 0 ? null : loanTask.Owner.ToString();
            parameters[9].Value = loanTask.LoanStageId <= 0 ? null : loanTask.LoanStageId.ToString();
            parameters[10].Value = loanTask.PrerequisiteTaskId <= 0 ? null : loanTask.PrerequisiteTaskId.ToString();

            if (sDaysToEstClose == string.Empty)
            {
                parameters[11].Value = null;
            }
            else
            {
                parameters[11].Value = Convert.ToInt32(sDaysToEstClose);
            }
            parameters[12].Value = null;     //TemplTaskId
            parameters[13].Value = null;     //WflTemplId         
            if (sDaysDueAfterRrerequisite == string.Empty)
            {
                parameters[14].Value = null;
            }
            else
            {
                parameters[14].Value = Convert.ToInt32(sDaysDueAfterRrerequisite);
            }
            parameters[15].Value = loanTask.WarningEmailId <= 0 ? null : loanTask.WarningEmailId.ToString();
            parameters[16].Value = loanTask.OverdueEmailId <= 0 ? null : loanTask.OverdueEmailId.ToString();
            parameters[17].Value = loanTask.CompletionEmailId <= 0 ? null : loanTask.CompletionEmailId.ToString();
            parameters[18].Value = loanTask.SequenceNumber.ToString();          
            if (sDaysAfterCreation == string.Empty)
            {
                parameters[19].Value = null;
            }
            else
            {
                parameters[19].Value = Convert.ToInt32(sDaysAfterCreation);
            }
            parameters[20].Value = loanTask.ExternalViewing;

            if (DaysDueAfterPrevStage == string.Empty)
            {
                parameters[21].Value = null;
            }
            else
            {
                parameters[21].Value = Convert.ToInt32(DaysDueAfterPrevStage);
            }
            parameters[22].Value = loanTask.Due == DateTime.MinValue ? null : loanTask.DueTime;

            parameters[23].Value = loanTask.Desc.Trim();
            #endregion
            int loanTaskId = -1, rowsAffected = 0;
            loanTaskId = DbHelperSQL.RunProcedure("dbo.[LoanTasks_Save]", parameters, out rowsAffected);
            if (loanTaskId > 0)
            {
                SqlParameter[] param2 = {
                    new SqlParameter("@LoanTaskId", SqlDbType.Int)
                                     };
                param2[0].Value = loanTaskId;
                DbHelperSQL.RunProcedure("dbo.lpsp_AddTask", param2, out rowsAffected);
            }
            return loanTaskId;
        }

        /// <summary>
        /// Deletes the task.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public static bool DeleteTask(int loanTaskId, int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_DeleteTask");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4),
					new SqlParameter("@userId", SqlDbType.Int,4)
                                        };
            parameters[0].Value = loanTaskId;
            parameters[1].Value = userId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }

        /// <summary>
        /// Defers the task.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="DaysDeferred">The days deferred.</param>
        /// <returns></returns>
        public static bool DeferTask(int loanTaskId, int userId, int DaysDeferred)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_DeferTask");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4),
					new SqlParameter("@userId", SqlDbType.Int,4),
					new SqlParameter("@DaysDeferred", SqlDbType.Int,4)
                                        };
            parameters[0].Value = loanTaskId;
            parameters[1].Value = userId;
            parameters[2].Value = DaysDeferred;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }

        /// <summary>
        /// Defers  the prospect task.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="DaysDeferred">The days deferred.</param>
        /// <returns></returns>
        public static bool DeferProspectTask(int ProspectTaskId, int userId, int DaysDeferred)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_DeferProspectTask");

            SqlParameter[] parameters = {
					new SqlParameter("@ProspectTaskId", SqlDbType.Int,4),
					new SqlParameter("@userId", SqlDbType.Int,4),
					new SqlParameter("@DaysDeferred", SqlDbType.Int,4)
                                        };
            parameters[0].Value = ProspectTaskId;
            parameters[1].Value = userId;
            parameters[2].Value = DaysDeferred;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }

        /// <summary>
        /// Assigns the task.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <param name="newUserId">The new user id.</param>
        /// <returns></returns>
        public static bool AssignTask(int loanTaskId, int newUserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_AssignTask");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4),
					new SqlParameter("@newUserId", SqlDbType.Int,4)
                                        };
            parameters[0].Value = loanTaskId;
            parameters[1].Value = newUserId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }
        /// <summary>
        /// update a Loan Task
        /// neo 2010-11-28
        /// </summary>
        /// <param name=LoanTasks>The model.LoanTask object.</param>
        /// <returns>true if successful</returns>
        public static bool UpdateTask(LPWeb.Model.LoanTasks loanTask)
        {
            if (loanTask == null || loanTask.LoanTaskId <= 0 || loanTask.Name.Trim() == string.Empty || loanTask.LoanStageId <= 0)
                throw new Exception("Invalid LoanTask parameter, LoanTaskId, LoanStageId or Task Name is not specified.");
            #region build sql

            SqlParameter[] parameters = {
                new SqlParameter("@LoanTaskID", SqlDbType.Int),
                new SqlParameter("@TaskName", SqlDbType.NVarChar, 255),
                new SqlParameter("@Due", SqlDbType.DateTime),
                new SqlParameter("@Owner", SqlDbType.Int),
                new SqlParameter("@NewLoanStageId", SqlDbType.Int),
                new SqlParameter("@PrerequisiteTaskId", SqlDbType.Int),
                new SqlParameter("@DaysDueFromEstClose", SqlDbType.SmallInt),
                new SqlParameter("@DaysDueAfterPrerequisite", SqlDbType.SmallInt),
                new SqlParameter("@WarningEmailId", SqlDbType.Int),
                new SqlParameter("@OverdueEmailId", SqlDbType.Int),
                new SqlParameter("@CompletionEmailId", SqlDbType.Int),
                new SqlParameter("@OldLoanStageId", SqlDbType.Int),
                new SqlParameter("@UserID", SqlDbType.Int),
                new SqlParameter("@DaysAfterCreated", SqlDbType.SmallInt),
                new SqlParameter("@Desc", SqlDbType.NVarChar)
                                        };

            parameters[0].Value = loanTask.LoanTaskId.ToString();
            parameters[1].Value = loanTask.Name.Trim();
            parameters[2].Value = loanTask.Due == DateTime.MinValue ? null : loanTask.Due;
            parameters[3].Value = loanTask.Owner <= 0 ? null : loanTask.Owner.ToString();
            parameters[4].Value = loanTask.LoanStageId <= 0 ? null : loanTask.LoanStageId.ToString();
            parameters[5].Value = loanTask.PrerequisiteTaskId <= 0 ? null : loanTask.PrerequisiteTaskId.ToString();
            parameters[6].Value = loanTask.DaysDueFromEstClose < -365 ? null : loanTask.DaysDueFromEstClose.ToString();
            parameters[7].Value = loanTask.DaysDueAfterPrerequisite <= 0 ? null : loanTask.DaysDueAfterPrerequisite.ToString();
            parameters[8].Value = loanTask.WarningEmailId <= 0 ? null : loanTask.WarningEmailId.ToString();
            parameters[9].Value = loanTask.OverdueEmailId <= 0 ? null : loanTask.OverdueEmailId.ToString();
            parameters[10].Value = loanTask.CompletionEmailId <= 0 ? null : loanTask.CompletionEmailId.ToString();
            parameters[11].Value = loanTask.OldLoanStageId <= 0 ? null : loanTask.OldLoanStageId.ToString();
            parameters[12].Value = loanTask.ModifiedBy <= 0 ? null : loanTask.ModifiedBy.ToString();
            parameters[13].Value = loanTask.DaysFromCreation < -365 ? null : loanTask.DaysFromCreation.ToString();
            parameters[14].Value = loanTask.Desc.Trim();

            #endregion

            DbHelperSQL.RunProcedure("dbo.[lpsp_UpdateLoanTask]", parameters);
            return true;
        }

        /// <summary>
        /// update a Loan Task
        /// neo 2011-04-11
        /// </summary>
        /// <param name="loanTask"></param>
        /// <param name="sDaysToEstClose"></param>
        /// <param name="sDaysAfterCreation"></param>
        /// <param name="sDaysDueAfterRrerequisite"></param>
        /// <returns></returns>
        public static bool UpdateTask(LPWeb.Model.LoanTasks loanTask, string sDaysToEstClose, string sDaysAfterCreation, string sDaysDueAfterRrerequisite, string DaysDueAfterPrevStage)
        {
            if (loanTask == null || loanTask.LoanTaskId <= 0 || loanTask.Name.Trim() == string.Empty || loanTask.LoanStageId <= 0)
                throw new Exception("Invalid LoanTask parameter, LoanTaskId, LoanStageId or Task Name is not specified.");
            #region build sql

            SqlParameter[] parameters = {
                new SqlParameter("@LoanTaskID", SqlDbType.Int),
                new SqlParameter("@TaskName", SqlDbType.NVarChar, 255),
                new SqlParameter("@Due", SqlDbType.DateTime),
                new SqlParameter("@Owner", SqlDbType.Int),
                new SqlParameter("@NewLoanStageId", SqlDbType.Int),
                new SqlParameter("@PrerequisiteTaskId", SqlDbType.Int),
                new SqlParameter("@DaysDueFromEstClose", SqlDbType.SmallInt),
                new SqlParameter("@DaysDueAfterPrerequisite", SqlDbType.SmallInt),
                new SqlParameter("@WarningEmailId", SqlDbType.Int),
                new SqlParameter("@OverdueEmailId", SqlDbType.Int),
                new SqlParameter("@CompletionEmailId", SqlDbType.Int),
                new SqlParameter("@OldLoanStageId", SqlDbType.Int),
                new SqlParameter("@UserID", SqlDbType.Int),
                new SqlParameter("@DaysAfterCreated", SqlDbType.SmallInt),               
                new SqlParameter("@ExternalViewing",SqlDbType.Bit),
                new SqlParameter("@DaysDueAfterPrevStage", SqlDbType.SmallInt),
                new SqlParameter("@DueTime", SqlDbType.Time),
                new SqlParameter("@Desc", SqlDbType.NVarChar)
                                        };

            parameters[0].Value = loanTask.LoanTaskId.ToString();
            parameters[1].Value = loanTask.Name.Trim();
            parameters[2].Value = loanTask.Due == DateTime.MinValue ? null : loanTask.Due;
            parameters[3].Value = loanTask.Owner <= 0 ? null : loanTask.Owner.ToString();
            parameters[4].Value = loanTask.LoanStageId <= 0 ? null : loanTask.LoanStageId.ToString();
            parameters[5].Value = loanTask.PrerequisiteTaskId <= 0 ? null : loanTask.PrerequisiteTaskId.ToString();

            // days from est close
            if (sDaysToEstClose == string.Empty)
            {
                parameters[6].Value = null;
            }
            else
            {
                parameters[6].Value = Convert.ToInt32(sDaysToEstClose);
            }

            // days after creation
            if (sDaysAfterCreation == string.Empty)
            {
                parameters[13].Value = null;
            }
            else
            {
                parameters[13].Value = Convert.ToInt32(sDaysAfterCreation);
            }

            // days after prerequisite
            if (sDaysDueAfterRrerequisite == string.Empty)
            {
                parameters[7].Value = null;
            }
            else
            {
                parameters[7].Value = Convert.ToInt32(sDaysDueAfterRrerequisite);
            }

            //parameters[6].Value = loanTask.DaysFromEstClose < -365 ? null : loanTask.DaysFromEstClose.ToString();
            //parameters[7].Value = loanTask.DaysAfterPrerequisiteCompleted <= 0 ? null : loanTask.DaysAfterPrerequisiteCompleted.ToString();
            parameters[8].Value = loanTask.WarningEmailId <= 0 ? null : loanTask.WarningEmailId.ToString();
            parameters[9].Value = loanTask.OverdueEmailId <= 0 ? null : loanTask.OverdueEmailId.ToString();
            parameters[10].Value = loanTask.CompletionEmailId <= 0 ? null : loanTask.CompletionEmailId.ToString();
            parameters[11].Value = loanTask.OldLoanStageId <= 0 ? null : loanTask.OldLoanStageId.ToString();
            parameters[12].Value = loanTask.ModifiedBy <= 0 ? null : loanTask.ModifiedBy.ToString();
            //parameters[13].Value = loanTask.DaysAfterCreated < -365 ? null : loanTask.DaysAfterCreated.ToString();

            parameters[14].Value = loanTask.ExternalViewing;//13在上边呢

            if (DaysDueAfterPrevStage == string.Empty)
            {
                parameters[15].Value = null;
            }
            else
            {
                parameters[15].Value = Convert.ToInt32(DaysDueAfterPrevStage);
            }

            // DueTime
            parameters[16].Value = loanTask.DueTime;
            // Desc
            parameters[17].Value = loanTask.Desc;

          
            #endregion

            DbHelperSQL.RunProcedure("dbo.[lpsp_UpdateLoanTask]", parameters);
            return true;
        }

        ///// neo 2010-11-28
        ///// </summary>
        ///// <param name="iTaskID"></param>
        ///// <param name="sTaskName"></param>
        ///// <param name="sDueDate"></param>
        ///// <param name="iLoginUserID"></param>
        ///// <param name="iOwerID"></param>
        ///// <param name="iNewLoanStageID"></param>
        ///// <param name="iPreTaskID"></param>
        ///// <param name="iDaysToEstClose"></param>
        ///// <param name="iDaysAfterPre"></param>
        ///// <param name="iWarningEmailID"></param>
        ///// <param name="iOverdueEmailID"></param>
        ///// <param name="iCompletionEmailID"></param>
        ///// <param name="iOldLoanStageID"></param>
        ///// <returns></returns>
        //public static bool UpdateTask(int iTaskID, string sTaskName, string sDueDate,
        //    int iLoginUserID, int iOwerID, int iNewLoanStageID, int iPrerequisiteTaskID,
        //    int iDaysToEstClose, int iDaysAfterPrerequisite, int iWarningEmailID,
        //    int iOverdueEmailID, int iCompletionEmailID, int iOldLoanStageID)
        //{
        //    #region build sql

        //    SqlCommand SqlCmd = new SqlCommand("lpsp_UpdateLoanTask");
        //    SqlCmd.CommandType = CommandType.StoredProcedure;

        //    DbHelperSQL.AddSqlParameter(SqlCmd, "@LoanTaskID", SqlDbType.Int, iTaskID);
        //    DbHelperSQL.AddSqlParameter(SqlCmd, "@TaskName", SqlDbType.NVarChar, sTaskName);
        //    if (sDueDate == string.Empty)
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@Due", SqlDbType.DateTime, DBNull.Value);
        //    }
        //    else
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@Due", SqlDbType.DateTime, DateTime.Parse(sDueDate));
        //    }

        //    if (iOwerID == 0)
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@Owner", SqlDbType.Int, DBNull.Value);
        //    }
        //    else
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@Owner", SqlDbType.Int, iOwerID);
        //    }

        //    DbHelperSQL.AddSqlParameter(SqlCmd, "@NewLoanStageId", SqlDbType.Int, iNewLoanStageID);

        //    if (iPrerequisiteTaskID == 0)
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@PrerequisiteTaskId", SqlDbType.Int, DBNull.Value);
        //    }
        //    else
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@PrerequisiteTaskId", SqlDbType.Int, iPrerequisiteTaskID);
        //    }

        //    if (iDaysToEstClose == 0)
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueFromEstClose", SqlDbType.SmallInt, DBNull.Value);
        //    }
        //    else
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueFromEstClose", SqlDbType.SmallInt, iDaysToEstClose);
        //    }

        //    if (iDaysAfterPrerequisite == 0)
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueAfterPrerequisite", SqlDbType.SmallInt, DBNull.Value);
        //    }
        //    else
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@DaysDueAfterPrerequisite", SqlDbType.SmallInt, iDaysAfterPrerequisite);
        //    }

        //    if (iWarningEmailID == 0)
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@WarningEmailId", SqlDbType.Int, DBNull.Value);
        //    }
        //    else
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@WarningEmailId", SqlDbType.Int, iWarningEmailID);
        //    }

        //    if (iOverdueEmailID == 0)
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@OverdueEmailId", SqlDbType.Int, DBNull.Value);
        //    }
        //    else
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@OverdueEmailId", SqlDbType.Int, iOverdueEmailID);
        //    }

        //    if (iCompletionEmailID == 0)
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@CompletionEmailId", SqlDbType.Int, DBNull.Value);
        //    }
        //    else
        //    {
        //        DbHelperSQL.AddSqlParameter(SqlCmd, "@CompletionEmailId", SqlDbType.Int, iCompletionEmailID);
        //    }
        //    DbHelperSQL.AddSqlParameter(SqlCmd, "@OldLoanStageId", SqlDbType.Int, iOldLoanStageID);
        //    DbHelperSQL.AddSqlParameter(SqlCmd, "@UserID", SqlDbType.Int, iLoginUserID);

        //    #endregion

        //    int returnValue = -1;
        //    returnValue = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd));

        //    return returnValue == 0;
        //}

        /// <summary>
        /// Gets the prospect task due date.
        /// </summary>
        /// <param name="prospectTaskId">The prospect task id.</param>
        /// <returns></returns>
        public static DateTime GetProspectTaskDueDate(int prospectTaskId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [dbo].[lpfn_GetProspectTaskDueDate](@ProspectTaskId)");

            SqlParameter[] parameters = {
					new SqlParameter("@ProspectTaskId", SqlDbType.Int,4)};
            parameters[0].Value = prospectTaskId;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                return Convert.ToDateTime(obj);
            }
        }

        /// <summary>
        /// Creates the prospect task alerts.
        /// </summary>
        /// <param name="prospectTaskId">The prospect task id.</param>
        /// <returns></returns>
        public static bool CreateProspectTaskAlerts(int prospectTaskId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_CreateProspectTaskAlerts");

            SqlParameter[] parameters = {
					new SqlParameter("@ProspectTaskId", SqlDbType.Int,4)
                                        };
            parameters[0].Value = prospectTaskId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }

        /// <summary>
        /// Removes the prospect task alerts.
        /// </summary>
        /// <param name="prospectTaskId">The prospect task id.</param>
        /// <returns></returns>
        public static bool RemoveProspectTaskAlerts(int prospectTaskId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_RemoveProspectTaskAlerts");

            SqlParameter[] parameters = {
					new SqlParameter("@ProspectTaskId", SqlDbType.Int,4)
                                        };
            parameters[0].Value = prospectTaskId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }

        #endregion

        #region StageAPI
        /// <summary>
        /// Get StageId using a taskId
        /// </summary>
        /// <param name="loanStageId">The loan task id.</param>
        /// <returns></returns>
        public static int GetStageIdByTaskId(int loanTaskId)
        {
            int stageId = -1;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [dbo].[lpfn_GetStageIdByTaskId(@LoanTaskId)");
            SqlParameter[] parameters = {
                 new SqlParameter("@LoanTaskId", SqlDbType.Int, 4)
                                        };
            parameters[0].Value = loanTaskId;
            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj != null)
                stageId = Convert.ToInt32(obj);
            return stageId;
        }
        /// <summary>
        /// Stages the completed.
        /// </summary>
        /// <param name="loanStageId">The loan stage id.</param>
        /// <returns></returns>
        public static bool StageCompleted(int loanStageId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [dbo].[lpfn_StageCompleted](@loanStageId)");

            SqlParameter[] parameters = {
					new SqlParameter("@loanStageId", SqlDbType.Int,4)};
            parameters[0].Value = loanStageId;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(obj);
            }
        }

        /// <summary>
        /// Gets the current stage.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public static string GetCurrentStage(int fileId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [dbo].[lpfn_GetCurrentStage](@fileId)");

            SqlParameter[] parameters = {
					new SqlParameter("@fileId", SqlDbType.Int,4)};
            parameters[0].Value = fileId;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return Convert.ToString(obj);
            }
        }

        /// <summary>
        /// Gets the last completed stage.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public static string GetLastCompletedStage(int fileId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [dbo].[lpfn_GetLastCompletedStage](@fileId)");

            SqlParameter[] parameters = {
					new SqlParameter("@fileId", SqlDbType.Int,4)};
            parameters[0].Value = fileId;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return Convert.ToString(obj);
            }
        }
        /// <summary>
        /// Gets the stage icon.
        /// </summary>
        /// <param name="loanStageId">The loan stage id.</param>
        /// <returns></returns>
        public static string GetStageIcon(int loanStageId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [dbo].[lpfn_GetStageIcon](@loanStageId)");

            SqlParameter[] parameters = {
					new SqlParameter("@loanStageId", SqlDbType.Int,4)};
            parameters[0].Value = loanStageId;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return Convert.ToString(obj);
            }
        }

        /// <summary>
        /// Completes the stage.
        /// </summary>
        /// <param name="loanStageId">The loan stage id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public static bool CompleteStage(int loanStageId, int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_CompleteStage");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanStageId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = loanStageId;
            parameters[1].Value = userId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }

        /// <summary>
        /// Uns the complete stage.
        /// </summary>
        /// <param name="loanStageId">The loan stage id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public static bool UnCompleteStage(int loanStageId, int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_UnCompleteStage");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanStageId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = loanStageId;
            parameters[1].Value = userId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }


        /// <summary>
        /// Gets the stage alias.
        /// </summary>
        /// <param name="loanStageId">The loan stage id.</param>
        /// <returns></returns>
        public static string GetStageAlias(int loanStageId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [dbo].[lpfn_GetStageAlias](@LoanStageID)");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanStageID", SqlDbType.Int,4)};
            parameters[0].Value = loanStageId;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return Convert.ToString(obj);
            }
        }

        /// <summary>
        /// Gets the stage graph.
        /// </summary>
        /// <param name="loanStageId">The loan stage id.</param>
        /// <returns></returns>
        public static string GetStageGraph(int loanStageId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [dbo].[lpfn_GetStageProgressImageFileName](@LoanStageID)");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanStageID", SqlDbType.Int,4)};
            parameters[0].Value = loanStageId;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return Convert.ToString(obj);
            }
        }
        public static int GetCurrentLoanStageId(int FileId)
        {
            int loanStageId = -1;
            string sqlCmd = string.Format("Select top 1 LoanStageId from LoanStages where FileId={0} and COMPLETED  IS NULL order by SequenceNumber desc", FileId);
            object obj = DbHelperSQL.GetSingle(sqlCmd);
            loanStageId = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
            return loanStageId;
        }
        public static int GetDefaultWorkflowTemplate(string WorkflowType)
        {
            int defWflTemplId = 0;
            string sqlCmd = string.Empty;
            sqlCmd = string.Format("Select WflTemplId from Template_Workflow where [Enabled]=1 and [Default]=1 and WorkflowType='{0}'", WorkflowType);
            object obj = DbHelperSQL.GetSingle(sqlCmd);
            defWflTemplId = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
            return defWflTemplId;
        }

        public static int GenerateDefaultLoanStages(int FileId, string WorkflowType)
        {
            int noLoanStages = 0;
            StringBuilder strSql = new StringBuilder();

            string sqlCmd = string.Format("Select Count(1) from LoanStages where FileId={0}", FileId);
            object obj = DbHelperSQL.GetSingle(sqlCmd);
            noLoanStages = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
            if (noLoanStages > 0)
            {
                return GetCurrentLoanStageId(FileId);
            }
            
            strSql.Append("dbo.GenerateDefaultLoanStages");
            SqlParameter[] parameters = {
				new SqlParameter("@FileId", SqlDbType.Int,4),
				new SqlParameter("@WorkflowType", SqlDbType.NVarChar,20)};
            parameters[0].Value = FileId;
            parameters[1].Value = WorkflowType;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);
            return GetCurrentLoanStageId(FileId);
        }

        #endregion

        #region LoanAPI
        /// <summary>
        /// Updates the loan status.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public static bool UpdateLoanStatus(int fileId, string status, int requestUserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_UpdateLoanStatus");

            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.NVarChar, 50),
                    new SqlParameter("@RequestBy", SqlDbType.Int, 4)};
            parameters[0].Value = fileId;
            parameters[1].Value = status;
            if (requestUserId <= 0)
                parameters[2].Value = DBNull.Value;
            else
                parameters[2].Value = requestUserId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }

        /// <summary>
        /// Updates the Prospect status.
        /// </summary>
        /// <param name="ContactID">The ContactID id.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public static bool UpdateProspectStatus(int ContactID, string status, int requestUserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_UpdateProspectStatus");

            SqlParameter[] parameters = {
					new SqlParameter("@ContactID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.NVarChar, 50),
                    new SqlParameter("@RequestBy", SqlDbType.Int, 4)};
            parameters[0].Value = ContactID;
            parameters[1].Value = status;
            if (requestUserId <= 0)
                parameters[2].Value = DBNull.Value;
            else
                parameters[2].Value = requestUserId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }
        /// <summary>
        /// Updates the Prospect loan status.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="operation">The operation: resume, convert, bad, cancel, suspend, deny.</param>
        /// <returns>error -- error message or exception</returns>
        public static string UpdateProspectLoanStatus(int fileId, string operation, int requestUserId)
        {
            string newLoanStatus = string.Empty;
            string errorMsg = string.Empty;
            switch (operation.ToLower().Trim())
            {
                case "bad": newLoanStatus = "Bad";
                    break;
                case "cancel": newLoanStatus = "Canceled";
                    break;
                case "convert": newLoanStatus = "Converted";
                    break;
                case "deny": newLoanStatus = "Denied";
                    break;
                case "resume": newLoanStatus = "Active";
                    break;
                case "suspend": newLoanStatus = "Suspend";
                    break;
                default: newLoanStatus = operation;
                    break;
                //default: errorMsg = "Unknown operation: " + operation;
                //    return errorMsg;

            }

            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_UpdateProspectLoanStatus");

            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.NVarChar, 50),
                    new SqlParameter("@RequestBy", SqlDbType.Int, 4)};
            parameters[0].Value = fileId;
            parameters[1].Value = newLoanStatus;
            if (requestUserId <= 0)
                parameters[2].Value = DBNull.Value;
            else
                parameters[2].Value = requestUserId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return errorMsg;
        }

        /// <summary>
        /// UpdateProspectAndLoanProspectLoanStatus
        /// Alex 2011-06-21
        /// </summary>
        /// <param name="ContactId"></param>
        /// <param name="operation"></param>
        /// <param name="requestUserId"></param>
        /// <returns></returns>
        public static string UpdateProspectAndLoanProspectLoanStatus(int ContactId, string operation, int requestUserId)
        {
            string errorMsg = string.Empty;
             StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_UpdateProspectAndLoanProspectLoanStatus");

            SqlParameter[] parameters = {
					new SqlParameter("ContactId", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.NVarChar, 50),
                    new SqlParameter("@RequestBy", SqlDbType.Int, 4)};
            parameters[0].Value = ContactId;
            parameters[1].Value = operation;
            if (requestUserId <= 0)
                parameters[2].Value = DBNull.Value;
            else
                parameters[2].Value = requestUserId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return errorMsg;

        }

        /// <summary>
        /// Reassigns the loan.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="newUserId">The new user id.</param>
        /// <param name="oldUserId">The old user id.</param>
        /// <param name="roleId">The role id.</param>
        /// <returns></returns>
        public static bool ReassignLoan(int fileId, int newUserId, int oldUserId, int roleId, int requester)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_ReassignLoan");

            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@NewUserId", SqlDbType.Int,4),
					new SqlParameter("@OldUserId", SqlDbType.Int,4),
					new SqlParameter("@RoleId", SqlDbType.Int,4),
                    new SqlParameter("@Requester", SqlDbType.Int, 4)
                                        };
            parameters[0].Value = fileId;
            parameters[1].Value = newUserId;
            if (oldUserId <= 0)
                parameters[2].Value = DBNull.Value;
            else
                parameters[2].Value = oldUserId;
            parameters[3].Value = roleId;
            if (requester <= 0)
                parameters[4].Value = DBNull.Value;
            else
                parameters[4].Value = requester;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }

        /// <summary>
        /// Reassigns the loan contact.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="newContactId">The new contact id.</param>
        /// <param name="oldContactId">The old contact id.</param>
        /// <param name="contactRoleId">The contact role id.</param>
        /// <returns></returns>
        public static bool ReassignLoanContact(int fileId, int newContactId, int oldContactId, int contactRoleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_ReassignLoanContact");

            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@NewContactId", SqlDbType.Int,4),
					new SqlParameter("@OldContactId", SqlDbType.Int,4),
					new SqlParameter("@ContactRoleId", SqlDbType.Int,4)
                                        };
            parameters[0].Value = fileId;
            parameters[1].Value = newContactId;
            parameters[2].Value = oldContactId;
            parameters[3].Value = contactRoleId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }

        /// <summary>
        /// Generates the workflow.
        /// </summary>
        /// <param name="workflowTemplId">The workflow templ id.</param>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public static bool GenerateWorkflow(int workflowTemplId, int fileId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_GenerateWorkflow");

            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@WorkflowTemplId", SqlDbType.Int,4)
                                        };
            parameters[0].Value = fileId;
            parameters[1].Value = workflowTemplId;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }
        /// <summary>
        /// IsLoanClosed
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public static bool IsLoanClosed(int fileId)
        {
            string sqlCmd = "Select [Status] from Loans where FileId=" + fileId;
            DataSet ds = DbHelperSQL.Query(sqlCmd);

            if (ds == null || ds.Tables[0].Rows.Count == 0)
                return false;

            string loanStatus = ds.Tables[0].Rows[0][0] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0][0].ToString();

            return (loanStatus == "Closed");
        }
        #endregion


    }
}
