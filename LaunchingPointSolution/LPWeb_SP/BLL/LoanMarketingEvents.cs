using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类LoanMarketingEvents 的摘要说明。
	/// </summary>
	public class LoanMarketingEvents
	{
		private readonly LPWeb.DAL.LoanMarketingEvents dal=new LPWeb.DAL.LoanMarketingEvents();
		public LoanMarketingEvents()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int LoanMarketingEventId)
		{
			return dal.Exists(LoanMarketingEventId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.LoanMarketingEvents model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.LoanMarketingEvents model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int LoanMarketingEventId)
		{
			
			dal.Delete(LoanMarketingEventId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.LoanMarketingEvents GetModel(int LoanMarketingEventId)
		{
			
			return dal.GetModel(LoanMarketingEventId);
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
		public List<LPWeb.Model.LoanMarketingEvents> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.LoanMarketingEvents> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.LoanMarketingEvents> modelList = new List<LPWeb.Model.LoanMarketingEvents>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.LoanMarketingEvents model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.LoanMarketingEvents();
					if(dt.Rows[n]["LoanMarketingEventId"].ToString()!="")
					{
						model.LoanMarketingEventId=int.Parse(dt.Rows[n]["LoanMarketingEventId"].ToString());
					}
					model.Action=dt.Rows[n]["Action"].ToString();
					if(dt.Rows[n]["ExecutionDate"].ToString()!="")
					{
						model.ExecutionDate=DateTime.Parse(dt.Rows[n]["ExecutionDate"].ToString());
					}
					if(dt.Rows[n]["LoanMarketingId"].ToString()!="")
					{
						model.LoanMarketingId=int.Parse(dt.Rows[n]["LoanMarketingId"].ToString());
					}
					model.LeadStarEventId=dt.Rows[n]["LeadStarEventId"].ToString();
					if(dt.Rows[n]["Completed"].ToString()!="")
					{
						if((dt.Rows[n]["Completed"].ToString()=="1")||(dt.Rows[n]["Completed"].ToString().ToLower()=="true"))
						{
						model.Completed=true;
						}
						else
						{
							model.Completed=false;
						}
					}
					if(dt.Rows[n]["WeekNo"].ToString()!="")
					{
						model.WeekNo=int.Parse(dt.Rows[n]["WeekNo"].ToString());
					}
					model.EventContent=dt.Rows[n]["EventContent"].ToString();
					model.EventURL=dt.Rows[n]["EventURL"].ToString();
					if(dt.Rows[n]["FileId"].ToString()!="")
					{
						model.FileId=int.Parse(dt.Rows[n]["FileId"].ToString());
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
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  成员方法
        
        public DataSet GetListForMarketingActivitiesEvents(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetListForMarketingActivitiesEvents(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        public DataSet GetStartedByUserOfLoanMarketing(string strWhere)
        {
            return dal.GetStartedByUserOfLoanMarketing(strWhere);
        }

        public bool CompleteLoanMarketingEvent(int nId)
        {
            return dal.CompleteLoanMarketingEvent(nId);
        }
	}
}

