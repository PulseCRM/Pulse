using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// LoanNotes 的摘要说明。
	/// </summary>
	public class LoanNotes
	{
		private readonly LPWeb.DAL.LoanNotes dal=new LPWeb.DAL.LoanNotes();
		public LoanNotes()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.LoanNotes model)
		{
			return dal.Add(model);
		}

        public int Add_LoanTaskId(LPWeb.Model.LoanNotes model)
        {
            return dal.Add_LoanTaskId(model);
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.LoanNotes model)
		{
			dal.Update(model);
        }

        public void Update_LoanTaskId(LPWeb.Model.LoanNotes model)
        {
            dal.Update_LoanTaskId(model);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int NoteId)
		{
			
			dal.Delete(NoteId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.LoanNotes GetModel(int NoteId)
		{
			
			return dal.GetModel(NoteId);
		}

        public LPWeb.Model.LoanNotes GetModel_LoanTaskId(int FileId, int LoanTaskId)
        {

            return dal.GetModel_LoanTaskId(FileId, LoanTaskId);
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
		public List<LPWeb.Model.LoanNotes> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.LoanNotes> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.LoanNotes> modelList = new List<LPWeb.Model.LoanNotes>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.LoanNotes model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.LoanNotes();
					if(dt.Rows[n]["NoteId"].ToString()!="")
					{
						model.NoteId=int.Parse(dt.Rows[n]["NoteId"].ToString());
					}
					if(dt.Rows[n]["FileId"].ToString()!="")
					{
						model.FileId=int.Parse(dt.Rows[n]["FileId"].ToString());
					}
					if(dt.Rows[n]["Created"].ToString()!="")
					{
						model.Created=DateTime.Parse(dt.Rows[n]["Created"].ToString());
					}
					model.Sender=dt.Rows[n]["Sender"].ToString();
					model.Note=dt.Rows[n]["Note"].ToString();
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


        /// <summary>
        /// Gets the loan notes.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="queryCondition">The query condition.</param>
        /// <param name="recordCount">The record count.</param>
        /// <param name="orderName">Name of the order.</param>
        /// <param name="orderType">Type of the order.</param>
        /// <returns></returns>
	    public DataSet GetLoanNotes(int pageSize, int pageIndex, string queryCondition, out int recordCount, string orderName, int orderType)
	    {
            return dal.GetLoanNotes(pageSize, pageIndex, queryCondition, out recordCount, orderName, orderType);
	    }

        public void UpdateNoteAndProspectIncomeAndProspectAssets(int FileId, string Note, string Other, string Amount)
        {
            dal.UpdateNoteAndProspectIncomeAndProspectAssets(FileId, Note, Other, Amount);
        }

        public void InterNoteAndProspectIncomeAndProspectAssets(int FileId, string Note, string Other, string Amount)
        {
            dal.InterNoteAndProspectIncomeAndProspectAssets(FileId, Note, Other, Amount);
        }

        public string GetLastNoteByFileID(int iFileID)
        {
            return dal.GetLastNoteByFileID(iFileID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileId"></param>
        /// <param name="LoanConditionId"></param>
        /// <param name="Note"></param>
        /// <param name="ExternalViewing"></param>
        /// <param name="Sender"></param>
        public void InsertConditionNote(int FileId, int LoanConditionId, string Note, bool ExternalViewing, string Sender)
        {
            this.dal.InsertConditionNote(FileId, LoanConditionId, Note, ExternalViewing, Sender);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="LoanConditionId"></param>
        /// <returns></returns>
        public DataTable GetConditionNoteList(int iFileId, int LoanConditionId)
        {
            return this.dal.GetConditionNoteList(iFileId, LoanConditionId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NoteIDs"></param>
        /// <param name="bEnabled"></param>
        public void EnableExternalViewing(string NoteIDs, bool bEnabled)
        {
            this.dal.EnableExternalViewing(NoteIDs, bEnabled);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iNoteId"></param>
        /// <returns></returns>
        public DataTable GetLoanNotesInfo(int iNoteId) 
        {
            return this.dal.GetLoanNotesInfo(iNoteId);
        }
	}
}

