using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// User2Purpose
	/// </summary>
	public class User2Purpose
	{
		private readonly LPWeb.DAL.User2Purpose dal=new LPWeb.DAL.User2Purpose();
		public User2Purpose()
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
		public bool Exists(int UserID,string Purpose)
		{
			return dal.Exists(UserID,Purpose);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.User2Purpose model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.User2Purpose model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int UserID,string Purpose)
		{
			
			return dal.Delete(UserID,Purpose);
		}

        /// <summary>
        /// 根据UserID删除一条数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
	    public bool DeleteByUserID(int userId)
	    {
	        return dal.DeleteByUserID(userId);
	    }

	    /// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.User2Purpose GetModel(int UserID,string Purpose)
		{
			
			return dal.GetModel(UserID,Purpose);
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
		public List<LPWeb.Model.User2Purpose> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.User2Purpose> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.User2Purpose> modelList = new List<LPWeb.Model.User2Purpose>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.User2Purpose model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.User2Purpose();
					if(dt.Rows[n]["UserID"].ToString()!="")
					{
						model.UserID=int.Parse(dt.Rows[n]["UserID"].ToString());
					}
					model.Purpose=dt.Rows[n]["Purpose"].ToString();
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

