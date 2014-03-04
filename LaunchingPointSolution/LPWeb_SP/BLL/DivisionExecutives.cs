using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// DivisionExecutives 的摘要说明。
	/// </summary>
	public class DivisionExecutives
	{
		private readonly LPWeb.DAL.DivisionExecutives dal=new LPWeb.DAL.DivisionExecutives();
		public DivisionExecutives()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.DivisionExecutives model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.DivisionExecutives model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int DivisionId,int ExecutiveId)
		{
			
			dal.Delete(DivisionId,ExecutiveId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.DivisionExecutives GetModel(int DivisionId,int ExecutiveId)
		{
			
			return dal.GetModel(DivisionId,ExecutiveId);
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
		public List<LPWeb.Model.DivisionExecutives> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.DivisionExecutives> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.DivisionExecutives> modelList = new List<LPWeb.Model.DivisionExecutives>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.DivisionExecutives model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.DivisionExecutives();
					if(dt.Rows[n]["DivisionId"].ToString()!="")
					{
						model.DivisionId=int.Parse(dt.Rows[n]["DivisionId"].ToString());
					}
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

        /// <summary>
        /// 根据UserId 获取查询其所属的Divisions
        /// </summary>
        /// <param name="executiveId">userid</param>
        /// <returns></returns>
	    public DataTable GetDivisonsByExecutiveId(int iUserId)
	    {
	        return dal.GetDivisonsByExecutiveId(iUserId);
	    }
	}
}

