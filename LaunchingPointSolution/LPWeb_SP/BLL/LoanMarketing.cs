using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// LoanMarketing
	/// </summary>
	public class LoanMarketing
	{
		private readonly LPWeb.DAL.LoanMarketing dal=new LPWeb.DAL.LoanMarketing();
		public LoanMarketing()
		{}
		#region  Method

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
		public bool Exists(int LoanMarketingId)
		{
			return dal.Exists(LoanMarketingId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.LoanMarketing model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.LoanMarketing model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int LoanMarketingId)
		{
			
			return dal.Delete(LoanMarketingId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string LoanMarketingIdlist )
		{
			return dal.DeleteList(LoanMarketingIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.LoanMarketing GetModel(int LoanMarketingId)
		{
			
			return dal.GetModel(LoanMarketingId);
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
		public List<LPWeb.Model.LoanMarketing> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.LoanMarketing> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.LoanMarketing> modelList = new List<LPWeb.Model.LoanMarketing>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.LoanMarketing model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.LoanMarketing();
					if(dt.Rows[n]["LoanMarketingId"].ToString()!="")
					{
						model.LoanMarketingId=int.Parse(dt.Rows[n]["LoanMarketingId"].ToString());
					}
					if(dt.Rows[n]["Selected"].ToString()!="")
					{
						model.Selected=DateTime.Parse(dt.Rows[n]["Selected"].ToString());
					}
					model.Type=dt.Rows[n]["Type"].ToString();
					if(dt.Rows[n]["Started"].ToString()!="")
					{
						model.Started=DateTime.Parse(dt.Rows[n]["Started"].ToString());
					}
					if(dt.Rows[n]["StartedBy"].ToString()!="")
					{
						model.StartedBy=int.Parse(dt.Rows[n]["StartedBy"].ToString());
					}
					if(dt.Rows[n]["CampaignId"].ToString()!="")
					{
						model.CampaignId=int.Parse(dt.Rows[n]["CampaignId"].ToString());
					}
					model.Status=dt.Rows[n]["Status"].ToString();
					if(dt.Rows[n]["FileId"].ToString()!="")
					{
						model.FileId=int.Parse(dt.Rows[n]["FileId"].ToString());
					}
					if(dt.Rows[n]["SelectedBy"].ToString()!="")
					{
						model.SelectedBy=int.Parse(dt.Rows[n]["SelectedBy"].ToString());
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
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  Method

          /// <summary>
        /// 根据ContactID 得到对应的LoanMarketing信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="count"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <param name="iContactID"></param>
        /// <returns></returns>
        public DataSet GetLoanMarketingByContactID(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType, string iContactID)
        {
            return dal.GetLoanMarketingByContactID(PageSize, PageIndex, strWhere, out count, orderName, orderType, iContactID);
        }

        public DataTable GetDisStartByInfo()
        {
            return dal.GetDisStartByInfo();
        }

        public DataTable GetLoanTypeInfoForAdd(int iContactID)
        {
            return dal.GetLoanTypeInfoForAdd(iContactID);
        }

        /// <summary>
        /// 根据FileID 得到对应的LoanMarketing信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="count"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <param name="iFileID"></param>
        /// <returns></returns>
        public DataSet GetLoanMarketingByFileID(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType, string iFileID)
        {
            return dal.GetLoanMarketingByFileID(PageSize, PageIndex, strWhere, out count, orderName, orderType, iFileID);
        }
	}
}

