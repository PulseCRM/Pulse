using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPWeb.BLL
{
    public class WorkflowManager
    {
        #region TaskAPI


        /// <summary>
        /// Gets the task due date.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        public static DateTime GetTaskDueDate(int loanTaskId)
        {
            return DAL.WorkflowManager.GetTaskDueDate(loanTaskId);
        }

        /// <summary>
        /// Gets the task icon.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        public static string GetTaskIcon(int loanTaskId)
        {
            return DAL.WorkflowManager.GetTaskIcon(loanTaskId);
        }
        /// <summary>
        /// Removes the task alerts.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        public static bool RemoveTaskAlerts(int loanTaskId)
        {
            return DAL.WorkflowManager.RemoveTaskAlerts(loanTaskId);
        }

        /// <summary>
        /// Creates the task alerts.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        public static bool CreateTaskAlerts(int loanTaskId)
        {
            return DAL.WorkflowManager.CreateTaskAlerts(loanTaskId);
        }
        /// <summary>
        /// Gets the red tasks.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        public static int GetRedTasks(int loanTaskId)
        {
            return DAL.WorkflowManager.GetRedTasks(loanTaskId);
        }
        /// <summary>
        /// Gets the yellow tasks.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        public static int GetYellowTasks(int loanTaskId)
        {
            return DAL.WorkflowManager.GetYellowTasks(loanTaskId);
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
            return DAL.WorkflowManager.CompleteTask(loanTaskId, userId, ref emailTemplate);
        }
        /// <summary>
        /// Uns the complete task.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public static bool UnCompleteTask(int loanTaskId, int userId)
        {
            return DAL.WorkflowManager.UnCompleteTask(loanTaskId, userId);
        }

        /// <summary>
        /// Adds the task.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <returns></returns>
        public static bool AddTask(int loanTaskId)
        {
            return DAL.WorkflowManager.AddTask(loanTaskId);
        }
        /// <summary>
        /// Adds the task.
        /// </summary>
        /// <param name="LoanTasks">The Model.LoanTasks object.</param>
        /// <returns>TaskId</returns>
        public static int AddTask(Model.LoanTasks loanTask)
        {
            return DAL.WorkflowManager.AddTask(loanTask);
        }

        public static int AddTask_Lead(Model.LoanTasks loanTask)
        {
            return DAL.WorkflowManager.AddTask_Lead(loanTask);
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
            return DAL.WorkflowManager.AddTask(loanTask, sDaysToEstClose, sDaysAfterCreation, sDaysDueAfterRrerequisite, DaysDueAfterPrevStage);
        }

        /// <summary>
        /// Deletes the task.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public static bool DeleteTask(int loanTaskId, int userId)
        {
            return DAL.WorkflowManager.DeleteTask(loanTaskId, userId);
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
            return DAL.WorkflowManager.DeferTask(loanTaskId, userId, DaysDeferred);
        }

        /// <summary>
        /// Assigns the task.
        /// </summary>
        /// <param name="loanTaskId">The loan task id.</param>
        /// <param name="newUserId">The new user id.</param>
        /// <returns></returns>
        public static bool AssignTask(int loanTaskId, int newUserId)
        {
            return DAL.WorkflowManager.AssignTask(loanTaskId, newUserId);
        }
        /// <summary>
        /// update a Loan Task
        /// neo 2010-11-28
        /// </summary>
        /// <param name=LoanTasks>The model.LoanTask object.</param>
        /// <returns>true if successful</returns>
        public static bool UpdateTask(LPWeb.Model.LoanTasks loanTask)
        {
            return DAL.WorkflowManager.UpdateTask(loanTask);
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
            return DAL.WorkflowManager.UpdateTask(loanTask, sDaysToEstClose, sDaysAfterCreation, sDaysDueAfterRrerequisite, DaysDueAfterPrevStage);
        }

        ///// <summary>
        ///// update task
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
        //    return DAL.WorkflowManager.UpdateTask(iTaskID, sTaskName, sDueDate,
        //        iLoginUserID, iOwerID, iNewLoanStageID, iPrerequisiteTaskID,
        //        iDaysToEstClose, iDaysAfterPrerequisite, iWarningEmailID,
        //        iOverdueEmailID, iCompletionEmailID, iOldLoanStageID);
        //}

        /// <summary>
        /// Gets the prospect task due date.
        /// </summary>
        /// <param name="prospectTaskId">The prospect task id.</param>
        /// <returns></returns>
        public static DateTime GetProspectTaskDueDate(int prospectTaskId)
        {
            return DAL.WorkflowManager.GetProspectTaskDueDate(prospectTaskId);
        }

        /// <summary>
        /// Creates the prospect task alerts.
        /// </summary>
        /// <param name="prospectTaskId">The prospect task id.</param>
        /// <returns></returns>
        public static bool CreateProspectTaskAlerts(int prospectTaskId)
        {
            return DAL.WorkflowManager.CreateProspectTaskAlerts(prospectTaskId);
        }

        /// <summary>
        /// Removes the prospect task alerts.
        /// </summary>
        /// <param name="prospectTaskId">The prospect task id.</param>
        /// <returns></returns>
        public static bool RemoveProspectTaskAlerts(int prospectTaskId)
        {
            return DAL.WorkflowManager.RemoveProspectTaskAlerts(prospectTaskId);
        }


        #endregion

        #region StageAPI

        /// <summary>
        /// Stages the completed.
        /// </summary>
        /// <param name="loanStageId">The loan stage id.</param>
        /// <returns></returns>
        public static bool StageCompleted(int loanStageId)
        {
            return DAL.WorkflowManager.StageCompleted(loanStageId);
        }

        /// <summary>
        /// Gets the current stage.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public static string GetCurrentStage(int fileId)
        {
            return DAL.WorkflowManager.GetCurrentStage(fileId);
        }

        /// <summary>
        /// Gets the last completed stage.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public static string GetLastCompletedStage(int fileId)
        {
            return DAL.WorkflowManager.GetLastCompletedStage(fileId);
        }
        /// <summary>
        /// Gets the stage icon.
        /// </summary>
        /// <param name="loanStageId">The loan stage id.</param>
        /// <returns></returns>
        public static string GetStageIcon(int loanStageId)
        {
            return DAL.WorkflowManager.GetStageIcon(loanStageId);
        }

        /// <summary>
        /// Completes the stage.
        /// </summary>
        /// <param name="loanStageId">The loan stage id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public static bool CompleteStage(int loanStageId, int userId)
        {
            return DAL.WorkflowManager.CompleteStage(loanStageId, userId);
        }

        /// <summary>
        /// Uns the complete stage.
        /// </summary>
        /// <param name="loanStageId">The loan stage id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public static bool UnCompleteStage(int loanStageId, int userId)
        {
            return DAL.WorkflowManager.UnCompleteStage(loanStageId, userId);
        }

        /// <summary>
        /// Gets the stage alias.
        /// </summary>
        /// <param name="loanStageId">The loan stage id.</param>
        /// <returns></returns>
        public static string GetStageAlias(int loanStageId)
        {
            return DAL.WorkflowManager.GetStageAlias(loanStageId);
        }

        /// <summary>
        /// Gets the stage graph.
        /// </summary>
        /// <param name="loanStageId">The loan stage id.</param>
        /// <returns></returns>
        public static string GetStageGraph(int loanStageId)
        {
            return DAL.WorkflowManager.GetStageGraph(loanStageId);
        }

        public static int GetCurrentLoanStageId(int FileId)
        {
            return DAL.WorkflowManager.GetCurrentLoanStageId(FileId);
        }
        public static int GetDefaultWorkflowTemplate(string WorkflowType)
        {
            return DAL.WorkflowManager.GetDefaultWorkflowTemplate(WorkflowType);
        }
        public static int GenerateDefaultLoanStages(int FileId, string WorkflowType)
        {
            if (string.IsNullOrEmpty(WorkflowType))
                WorkflowType = "Processing";
            return DAL.WorkflowManager.GenerateDefaultLoanStages(FileId, WorkflowType);
        }
        #endregion

        #region LoanAPI

        /// <summary>
        /// Updates the loan status.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public static bool UpdateLoanStatus(int fileId, string status, int requestByUserId)
        {
            return DAL.WorkflowManager.UpdateLoanStatus(fileId, status, requestByUserId);
        }

        /// <summary>
        /// Updates the prospect status.
        /// </summary>
        /// <param name="fileId">The prospect id.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public static bool UpdateProspectStatus(int prospectId, string status, int requestByUserId)
        {
            return DAL.WorkflowManager.UpdateProspectStatus(prospectId, status, requestByUserId);
        }
        /// <summary>
        /// Updates the loan status.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="operation">The operation: resume, convert, bad, cancel, suspend, deny.</param>
        /// <returns>error -- error message or exception</returns>
        public static string UpdateProspectLoanStatus(int fileId, string operation, int requestUserId)
        {
            return DAL.WorkflowManager.UpdateProspectLoanStatus(fileId, operation, requestUserId);
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
            return DAL.WorkflowManager.UpdateProspectAndLoanProspectLoanStatus(ContactId, operation, requestUserId);
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
            return DAL.WorkflowManager.ReassignLoan(fileId, newUserId, oldUserId, roleId, requester);
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
            return DAL.WorkflowManager.ReassignLoanContact(fileId, newContactId, oldContactId, contactRoleId);
        }

        /// <summary>
        /// Generates the workflow.
        /// </summary>
        /// <param name="workflowTemplId">The workflow templ id.</param>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public static bool GenerateWorkflow(int workflowTemplId, int fileId)
        {
            return DAL.WorkflowManager.GenerateWorkflow(workflowTemplId, fileId);
        }

        /// <summary>
        /// IsLoanClosed
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public static bool IsLoanClosed(int fileId)
        {
            return DAL.WorkflowManager.IsLoanClosed(fileId);
        }
        #endregion
    }
}
