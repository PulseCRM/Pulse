using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// Company_Alerts 的摘要说明。
	/// </summary>
	public class Company_Alerts
	{
		private readonly LPWeb.DAL.Company_Alerts dal=new LPWeb.DAL.Company_Alerts();
		public Company_Alerts()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.Company_Alerts model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.Company_Alerts model)
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
		public LPWeb.Model.Company_Alerts GetModel()
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
		public List<LPWeb.Model.Company_Alerts> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.Company_Alerts> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Company_Alerts> modelList = new List<LPWeb.Model.Company_Alerts>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Company_Alerts model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Company_Alerts();
					if(dt.Rows[n]["AlertYellowDays"].ToString()!="")
					{
						model.AlertYellowDays=int.Parse(dt.Rows[n]["AlertYellowDays"].ToString());
					}
					if(dt.Rows[n]["AlertRedDays"].ToString()!="")
					{
						model.AlertRedDays=int.Parse(dt.Rows[n]["AlertRedDays"].ToString());
					}
					if(dt.Rows[n]["TaskYellowDays"].ToString()!="")
					{
						model.TaskYellowDays=int.Parse(dt.Rows[n]["TaskYellowDays"].ToString());
					}
					if(dt.Rows[n]["TaskRedDays"].ToString()!="")
					{
						model.TaskRedDays=int.Parse(dt.Rows[n]["TaskRedDays"].ToString());
					}
					if(dt.Rows[n]["RateLockYellowDays"].ToString()!="")
					{
						model.RateLockYellowDays=int.Parse(dt.Rows[n]["RateLockYellowDays"].ToString());
					}
				    if(dt.Rows[n]["RateLockRedDays"].ToString()!="")
					{
						model.RateLockRedDays=int.Parse(dt.Rows[n]["RateLockRedDays"].ToString());
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

