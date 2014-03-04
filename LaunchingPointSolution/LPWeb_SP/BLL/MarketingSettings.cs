using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// MarketingSettings
	/// </summary>
	public class MarketingSettings
	{
		private readonly LPWeb.DAL.MarketingSettings dal=new LPWeb.DAL.MarketingSettings();
		public MarketingSettings()
		{}
		#region  Method

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.MarketingSettings model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.MarketingSettings model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.Delete();
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.MarketingSettings GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.GetModel();
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
		public List<LPWeb.Model.MarketingSettings> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.MarketingSettings> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.MarketingSettings> modelList = new List<LPWeb.Model.MarketingSettings>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.MarketingSettings model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.MarketingSettings();
					model.WebServiceURL=dt.Rows[n]["WebServiceURL"].ToString();
					model.CampaignDetailURL=dt.Rows[n]["CampaignDetailURL"].ToString();
					if(dt.Rows[n]["ReconcileInterval"].ToString()!="")
					{
						model.ReconcileInterval=int.Parse(dt.Rows[n]["ReconcileInterval"].ToString());
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
	}
}

