using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// MarketingCampaigns
	/// </summary>
	public class MarketingCampaigns
	{
		private readonly LPWeb.DAL.MarketingCampaigns dal=new LPWeb.DAL.MarketingCampaigns();
		public MarketingCampaigns()
		{}
		#region  Method

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.MarketingCampaigns model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.MarketingCampaigns model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int CampaignId)
		{
			
			return dal.Delete(CampaignId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string CampaignIdlist )
		{
			return dal.DeleteList(CampaignIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.MarketingCampaigns GetModel(int CampaignId)
		{
			
			return dal.GetModel(CampaignId);
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
		public List<LPWeb.Model.MarketingCampaigns> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.MarketingCampaigns> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.MarketingCampaigns> modelList = new List<LPWeb.Model.MarketingCampaigns>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.MarketingCampaigns model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.MarketingCampaigns();
					if(dt.Rows[n]["CampaignId"].ToString()!="")
					{
						model.CampaignId=int.Parse(dt.Rows[n]["CampaignId"].ToString());
					}
					model.GlobalId=dt.Rows[n]["GlobalId"].ToString();
                    model.CampaignName = dt.Rows[n]["CampaignName"].ToString();
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

        public DataSet GetCampaignsList(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetCampaignsList(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        public DataSet GetCampaignsListForPersonlizationAdd(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetCampaignsListForPersonlizationAdd(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        public DataSet GetListForPersonlization(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetListForPersonlization(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        public void AddAutoCampaigns(int[] campaignIds, string strLoanType, int nTemplStageId, int nUserId)
        {
            if (campaignIds.Length > 0)
                dal.AddAutoCampaigns(campaignIds, strLoanType, nTemplStageId, nUserId);
        }

        public void SaveAutoCampaigns(DataTable dtAc)
        {
            if (null != dtAc && dtAc.Rows.Count > 0)
                dal.SaveAutoCampaigns(dtAc);
        }

        public void RemoveAutoCampaigns(int[] campaignIds)
        {
            if (campaignIds.Length > 0)
                dal.RemoveAutoCampaigns(campaignIds);
        }

        public DataTable GetMarketingCategoryInfo()
        {
            return dal.GetMarketingCategoryInfo();
        }

        /// <summary>
        /// get marketing campaigns in alphabetical order
        /// </summary>
        public DataSet GetListInAlphOrder(string strWhere)
        {
            return dal.GetListInAlphOrder(strWhere);
        }
	}
}

