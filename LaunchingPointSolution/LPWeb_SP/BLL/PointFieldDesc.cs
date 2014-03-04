using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// PointFieldDesc 的摘要说明。
	/// </summary>
	public class PointFieldDesc
	{
		private readonly LPWeb.DAL.PointFieldDesc dal=new LPWeb.DAL.PointFieldDesc();
		public PointFieldDesc()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.PointFieldDesc model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.PointFieldDesc model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(decimal PointFieldId)
		{
			
			dal.Delete(PointFieldId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.PointFieldDesc GetModel(decimal PointFieldId)
		{
			
			return dal.GetModel(PointFieldId);
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
		public List<LPWeb.Model.PointFieldDesc> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.PointFieldDesc> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.PointFieldDesc> modelList = new List<LPWeb.Model.PointFieldDesc>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.PointFieldDesc model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.PointFieldDesc();
					if(dt.Rows[n]["PointFieldId"].ToString()!="")
					{
						model.PointFieldId=decimal.Parse(dt.Rows[n]["PointFieldId"].ToString());
					}
					model.Label=dt.Rows[n]["Label"].ToString();
					if(dt.Rows[n]["DataType"].ToString()!="")
					{
						model.DataType=int.Parse(dt.Rows[n]["DataType"].ToString());
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

