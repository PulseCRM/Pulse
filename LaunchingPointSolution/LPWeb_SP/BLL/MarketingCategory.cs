using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// MarketingCategory
	/// </summary>
	public class MarketingCategory
	{
		private readonly LPWeb.DAL.MarketingCategory dal=new LPWeb.DAL.MarketingCategory();
		public MarketingCategory()
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
		public bool Exists(int CategoryId)
		{
			return dal.Exists(CategoryId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.MarketingCategory model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.MarketingCategory model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int CategoryId)
		{
			
			return dal.Delete(CategoryId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string CategoryIdlist )
		{
			return dal.DeleteList(CategoryIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.MarketingCategory GetModel(int CategoryId)
		{
			
			return dal.GetModel(CategoryId);
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
		public List<LPWeb.Model.MarketingCategory> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.MarketingCategory> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.MarketingCategory> modelList = new List<LPWeb.Model.MarketingCategory>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.MarketingCategory model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.MarketingCategory();
					if(dt.Rows[n]["CategoryId"].ToString()!="")
					{
						model.CategoryId=int.Parse(dt.Rows[n]["CategoryId"].ToString());
					}
					model.CategoryName=dt.Rows[n]["CategoryName"].ToString();
					model.GlobalId=dt.Rows[n]["GlobalId"].ToString();
					model.Description=dt.Rows[n]["Description"].ToString();
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

        /// <summary>
        /// get marketing categories in alphabetical order
        /// </summary>
        public DataSet GetListInAlphOrder(string strWhere)
        {
            return dal.GetListInAlphOrder(strWhere);
        }

		#endregion  Method
	}
}

