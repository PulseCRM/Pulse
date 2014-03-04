using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;

namespace LPWeb.BLL
{
	/// <summary>
	/// LoanTasks 的摘要说明。
	/// </summary>
	public class LoanTasks
	{
		private readonly LPWeb.DAL.LoanTasks dal=new LPWeb.DAL.LoanTasks();
		public LoanTasks()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.LoanTasks model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.LoanTasks model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int LoanTaskId)
		{
			
			dal.Delete(LoanTaskId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.LoanTasks GetModel(int LoanTaskId)
		{
			
			return dal.GetModel(LoanTaskId);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.LoanTasks> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.LoanTasks> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.LoanTasks> modelList = new List<LPWeb.Model.LoanTasks>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.LoanTasks model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.LoanTasks();
					if(dt.Rows[n]["LoanTaskId"].ToString()!="")
					{
						model.LoanTaskId=int.Parse(dt.Rows[n]["LoanTaskId"].ToString());
					}
					if(dt.Rows[n]["FileId"].ToString()!="")
					{
						model.FileId=int.Parse(dt.Rows[n]["FileId"].ToString());
					}
					if(dt.Rows[n]["WflTemplId"].ToString()!="")
					{
						model.WflTemplId=int.Parse(dt.Rows[n]["WflTemplId"].ToString());
					}
					model.Name=dt.Rows[n]["Name"].ToString();
					
					if(dt.Rows[n]["Due"].ToString()!="")
					{
						model.Due=DateTime.Parse(dt.Rows[n]["Due"].ToString());
					}
					if(dt.Rows[n]["Completed"].ToString()!="")
					{
						model.Completed=DateTime.Parse(dt.Rows[n]["Completed"].ToString());
					}
					if(dt.Rows[n]["CompletedBy"].ToString()!="")
					{
						model.CompletedBy=int.Parse(dt.Rows[n]["CompletedBy"].ToString());
					}
					if(dt.Rows[n]["LastModified"].ToString()!="")
					{
						model.LastModified=DateTime.Parse(dt.Rows[n]["LastModified"].ToString());
					}
					modelList.Add(model);
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			return dal.GetList(PageSize,PageIndex,strWhere);
		}

		#endregion  成员方法

        #region neo

        /// <summary>
        /// get prerequisite list
        /// neo 2010-11-17
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetPrerequisiteList(string sWhere)
        {
            return dal.GetPrerequisiteListBase(sWhere);
        }

        /// <summary>
        /// get loan task list
        /// neo 2010-11-17
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetLoanTaskList(string sWhere)
        {
            return dal.GetLoanTaskListBase(sWhere);
        }

        public DataTable GetLoanTaskList(int iTop, string sWhere, string sOrderBy)
        {
            return dal.GetLoanTaskListBase(iTop, sWhere, sOrderBy);
        }

        /// <summary>
        /// get loan task owners
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetLoanTaskOwers(int iLoanID)
        {
            return dal.GetLoanTaskOwersBase(iLoanID);
        }

        /// <summary>
        /// get task owners for reassign
        /// neo 2010-11-23
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetLoanTaskOwers_Reassign(int iLoanID)
        {
            return dal.GetLoanTaskOwers_ReassignBase(iLoanID);
        }

        /// <summary>
        /// get task owner for Task Owner Filter
        /// neo 2010-11-19
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetTaskOwnerList(int iLoanID)
        {
            return dal.GetTaskOwnerListBase(iLoanID);
        }

        /// <summary>
        /// 检查某个loan下任务名称是否重复
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <param name="sTaskName"></param>
        /// <returns></returns>
        public bool IsLoanTaskExists_Insert(int iLoanID, string sTaskName)
        {
            return dal.IsLoanTaskExists_InsertBase(iLoanID, sTaskName);
        }

        /// <summary>
        /// 检查某个loan下任务名称是否重复
        /// neo 2010-11-18
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <param name="iTaskID"></param>
        /// <param name="sTaskName"></param>
        /// <returns></returns>
        public bool IsLoanTaskExists_Update(int iLoanID, int iTaskID, string sTaskName)
        {
            return dal.IsLoanTaskExists_UpdateBase(iLoanID, iTaskID, sTaskName);
        }

        /// <summary>
        /// get next seq number
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public int GetNextSequenceNum(int iLoanID)
        {
            return dal.GetNextSequenceNumBase(iLoanID);
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
        public void InsertLoanTask(int iLoanID, string sTaskName, string sDueDate, int iOwerID, int iLoanStageID, int iPreTaskID, int iSeq, int iDaysToEstClose, int iDaysAfterPre, int iWarningEmailID, int iOverdueEmailID, int iCompletionEmailID)
        {
            dal.InsertLoanTaskBase(iLoanID, sTaskName, sDueDate, iOwerID, iLoanStageID, iPreTaskID, iSeq, iDaysToEstClose, iDaysAfterPre, iWarningEmailID, iOverdueEmailID, iCompletionEmailID);
        }

        /// <summary>
        /// get loan task info
        /// neo 2010-11-18
        /// </summary>
        /// <param name="iTaskID"></param>
        /// <returns></returns>
        public DataTable GetLoanTaskInfo(int iTaskID)
        {
            return dal.GetLoanTaskInfoBase(iTaskID);
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
        /// <param name="iLoanStageID"></param>
        /// <param name="iPreTaskID"></param>
        /// <param name="iDaysToEstClose"></param>
        /// <param name="iDaysAfterPre"></param>
        /// <param name="iWarningEmailID"></param>
        /// <param name="iOverdueEmailID"></param>
        /// <param name="iCompletionEmailID"></param>
        /// <param name="iOldLoanStageID"></param>
        /// <returns></returns>
        //public bool UpdateLoanTask(int iTaskID, string sTaskName, string sDueDate, int iLoginUserID, int iOwerID, int iLoanStageID, int iPreTaskID, int iDaysToEstClose, int iDaysAfterPre, int iWarningEmailID, int iOverdueEmailID, int iCompletionEmailID, int iOldLoanStageID)
        //{
        //    return dal.UpdateLoanTaskBase(iTaskID, sTaskName, sDueDate, iLoginUserID, iOwerID, iLoanStageID, iPreTaskID, iDaysToEstClose, iDaysAfterPre, iWarningEmailID, iOverdueEmailID, iCompletionEmailID, iOldLoanStageID);
        //}

        /// <summary>
        /// delete loan task
        /// neo 2010-11-18
        /// </summary>
        /// <param name="iLoanTaskID"></param>
        public void DeleteLoanTask(int iLoanTaskID)
        {
            dal.DeleteLoanTaskBase(iLoanTaskID);
        }

        /// <summary>
        /// is a prerequisite task?
        /// neo 2010-11-22
        /// </summary>
        /// <param name="iLoanTaskID"></param>
        /// <returns></returns>
        public bool IsPrerequisite(int iLoanTaskID)
        {
            return dal.IsPrerequisiteBase(iLoanTaskID);
        }

        /// <summary>
        /// get user task list which due today
        /// neo 2012-09-12
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetUserTaskDueToday(int iUserID)
        {
            return this.dal.GetUserTaskDueToday(iUserID);
        }

        /// <summary>
        /// 
        /// duanlijun 2012-09-116
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public string CreateLoanTasks(int FileId, string TasksName, int CompletedBy, DateTime Due,string time, int Owner, DateTime Completed,bool cbInterestOnly)
        {
            return this.dal.CreateLoanTasks(FileId, TasksName, CompletedBy, Due,time, Owner, Completed, cbInterestOnly);
        }


        public string CreateLoanTasks(int FileId, string TasksName, int CompletedBy, DateTime Due, string time, int Owner, DateTime Completed, bool cbInterestOnly, string notes)
        {
            return this.dal.CreateLoanTasks(FileId, TasksName, CompletedBy, Due, time, Owner, Completed, cbInterestOnly, notes);
        }

        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetListForGridView(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        public void GetSnoozePopup(string minute, string LoanTaskId)
        {
             dal.GetSnoozePopup(minute, LoanTaskId);
        }

        public string GetLoanTasks(int minutes,int useId)
        {
            return dal.GetLoanTasks(minutes,useId);
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
            return this.dal.iCalendarToString(FileId, LoanTaskId, UserId);
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
            return this.dal.GetReminderTaskList(iUserId, iMinutes);
        }

        /// <summary>
        /// snooze reminder task
        /// neo 2012-11-25
        /// </summary>
        /// <param name="sTaskIDs"></param>
        /// <param name="iMinutes"></param>
        public void SnoozeTask(string sTaskIDs, int iMinutes)
        {
            this.dal.SnoozeTask(sTaskIDs, iMinutes);
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
            return this.dal.ShowTaskReminder(iUserId, iMinutes);
        }

        #endregion
    }
}

