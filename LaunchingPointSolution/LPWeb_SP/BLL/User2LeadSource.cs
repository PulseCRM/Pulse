using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// User2LeadSource
	/// </summary>
	public class User2LeadSource
	{
		private readonly LPWeb.DAL.User2LeadSource dal=new LPWeb.DAL.User2LeadSource();
		public User2LeadSource()
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
		public bool Exists(int UserID,int LeadSourceID)
		{
			return dal.Exists(UserID,LeadSourceID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.User2LeadSource model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.User2LeadSource model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int UserID,int LeadSourceID)
		{
			
			return dal.Delete(UserID,LeadSourceID);
		}

	    public bool DeleteByUserID(int userID)
	    {
            return dal.DeleteByUserID(userID);
	    }

	    /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="leadSourceIds"></param>
        /// <returns></returns>
	    public void Delete(String leadSourceIds)
	    {
            string[] leadSources = leadSourceIds.Split(',');
            foreach (var leadSource in leadSources)
            {
                dal.Delete(int.Parse(leadSource));
            }
	    }

	    /// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.User2LeadSource GetModel(int UserID,int LeadSourceID)
		{
			
			return dal.GetModel(UserID,LeadSourceID);
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
		public List<LPWeb.Model.User2LeadSource> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.User2LeadSource> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.User2LeadSource> modelList = new List<LPWeb.Model.User2LeadSource>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.User2LeadSource model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.User2LeadSource();
					if(dt.Rows[n]["UserID"].ToString()!="")
					{
						model.UserID=int.Parse(dt.Rows[n]["UserID"].ToString());
					}
					if(dt.Rows[n]["LeadSourceID"].ToString()!="")
					{
						model.LeadSourceID=int.Parse(dt.Rows[n]["LeadSourceID"].ToString());
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

