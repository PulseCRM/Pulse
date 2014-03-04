using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ProspectAssets
	/// </summary>
	public partial class ProspectAssets
	{
		private readonly LPWeb.DAL.ProspectAssets dal=new LPWeb.DAL.ProspectAssets();
		public ProspectAssets()
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
		public bool Exists(int ProspectAssetId)
		{
			return dal.Exists(ProspectAssetId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.ProspectAssets model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.ProspectAssets model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int ProspectAssetId)
		{
			
			return dal.Delete(ProspectAssetId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string ProspectAssetIdlist )
		{
			return dal.DeleteList(ProspectAssetIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.ProspectAssets GetModel(int ProspectAssetId)
		{
			
			return dal.GetModel(ProspectAssetId);
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
		public List<LPWeb.Model.ProspectAssets> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.ProspectAssets> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.ProspectAssets> modelList = new List<LPWeb.Model.ProspectAssets>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.ProspectAssets model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.ProspectAssets();
					if(dt.Rows[n]["ProspectAssetId"]!=null && dt.Rows[n]["ProspectAssetId"].ToString()!="")
					{
						model.ProspectAssetId=int.Parse(dt.Rows[n]["ProspectAssetId"].ToString());
					}
					if(dt.Rows[n]["ContactId"]!=null && dt.Rows[n]["ContactId"].ToString()!="")
					{
						model.ContactId=int.Parse(dt.Rows[n]["ContactId"].ToString());
					}
					if(dt.Rows[n]["Name"]!=null && dt.Rows[n]["Name"].ToString()!="")
					{
					model.Name=dt.Rows[n]["Name"].ToString();
					}
					if(dt.Rows[n]["Account"]!=null && dt.Rows[n]["Account"].ToString()!="")
					{
					model.Account=dt.Rows[n]["Account"].ToString();
					}
					if(dt.Rows[n]["Amount"]!=null && dt.Rows[n]["Amount"].ToString()!="")
					{
						model.Amount=decimal.Parse(dt.Rows[n]["Amount"].ToString());
					}
					if(dt.Rows[n]["Type"]!=null && dt.Rows[n]["Type"].ToString()!="")
					{
					model.Type=dt.Rows[n]["Type"].ToString();
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

