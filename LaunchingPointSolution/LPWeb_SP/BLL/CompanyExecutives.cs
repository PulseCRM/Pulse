using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// CompanyExecutives 的摘要说明。
	/// </summary>
	public class CompanyExecutives
	{
		private readonly LPWeb.DAL.CompanyExecutives dal=new LPWeb.DAL.CompanyExecutives();
		public CompanyExecutives()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.CompanyExecutives model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.CompanyExecutives model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ExecutiveId)
		{
			
			dal.Delete(ExecutiveId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.CompanyExecutives GetModel(int ExecutiveId)
		{
			
			return dal.GetModel(ExecutiveId);
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
		public List<LPWeb.Model.CompanyExecutives> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.CompanyExecutives> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.CompanyExecutives> modelList = new List<LPWeb.Model.CompanyExecutives>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.CompanyExecutives model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.CompanyExecutives();
					if(dt.Rows[n]["ExecutiveId"].ToString()!="")
					{
						model.ExecutiveId=int.Parse(dt.Rows[n]["ExecutiveId"].ToString());
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
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			return dal.GetList(PageSize,PageIndex,strWhere);
		}

		#endregion  成员方法
	}
}

