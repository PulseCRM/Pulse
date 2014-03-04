using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// Company_Point 的摘要说明。
	/// </summary>
	public class Company_Point
	{
		private readonly LPWeb.DAL.Company_Point dal=new LPWeb.DAL.Company_Point();
		public Company_Point()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.Company_Point model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.Company_Point model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			dal.Delete();
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.Company_Point GetModel()
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
		public List<LPWeb.Model.Company_Point> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.Company_Point> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Company_Point> modelList = new List<LPWeb.Model.Company_Point>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Company_Point model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Company_Point();
					model.WinpointIniPath=dt.Rows[n]["WinpointIniPath"].ToString();
					model.PointFieldIDMappingFile=dt.Rows[n]["PointFieldIDMappingFile"].ToString();
					model.CardexFile=dt.Rows[n]["CardexFile"].ToString();
					if(dt.Rows[n]["PointImportIntervalMinutes"].ToString()!="")
					{
						model.PointImportIntervalMinutes=int.Parse(dt.Rows[n]["PointImportIntervalMinutes"].ToString());
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

        public DataTable GetCompany_PointInfo()
        {
            return dal.GetCompany_PointInfo();
        }
	}
}

