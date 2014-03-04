using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类ContactNotes 的摘要说明。
	/// </summary>
	public class ContactNotes
	{
		private readonly LPWeb.DAL.ContactNotes dal=new LPWeb.DAL.ContactNotes();
		public ContactNotes()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.ContactNotes model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.ContactNotes model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ContactNoteId)
		{
			
			dal.Delete(ContactNoteId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.ContactNotes GetModel(int ContactNoteId)
		{
			
			return dal.GetModel(ContactNoteId);
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
		public List<LPWeb.Model.ContactNotes> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.ContactNotes> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.ContactNotes> modelList = new List<LPWeb.Model.ContactNotes>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.ContactNotes model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.ContactNotes();
					if(dt.Rows[n]["ContactNoteId"].ToString()!="")
					{
						model.ContactNoteId=int.Parse(dt.Rows[n]["ContactNoteId"].ToString());
					}
					if(dt.Rows[n]["ContactId"].ToString()!="")
					{
						model.ContactId=int.Parse(dt.Rows[n]["ContactId"].ToString());
					}
					if(dt.Rows[n]["Created"].ToString()!="")
					{
						model.Created=DateTime.Parse(dt.Rows[n]["Created"].ToString());
					}
					if(dt.Rows[n]["CreatedBy"].ToString()!="")
					{
						model.CreatedBy=int.Parse(dt.Rows[n]["CreatedBy"].ToString());
					}
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
        /// Gets the prospect notes.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="queryCondition">The query condition.</param>
        /// <param name="recordCount">The record count.</param>
        /// <param name="orderName">Name of the order.</param>
        /// <param name="orderType">Type of the order.</param>
        /// <returns></returns>
        public DataSet GetContactNotes(int pageSize, int pageIndex, string queryCondition, out int recordCount, string orderName, int orderType)
        {
            return dal.GetContactNotes(pageSize, pageIndex, queryCondition, out recordCount, orderName, orderType);
        }
	}
}

