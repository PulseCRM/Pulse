using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类Template_Wfl_Task_Alerts 的摘要说明。
	/// </summary>
	public class Template_Wfl_Task_Alerts
	{
		private readonly LPWeb.DAL.Template_Wfl_Task_Alerts dal=new LPWeb.DAL.Template_Wfl_Task_Alerts();
		public Template_Wfl_Task_Alerts()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.Template_Wfl_Task_Alerts model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.Template_Wfl_Task_Alerts model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int TemplAlertId)
		{
			
			dal.Delete(TemplAlertId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.Template_Wfl_Task_Alerts GetModel(int TemplAlertId)
		{
			
			return dal.GetModel(TemplAlertId);
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
		public List<LPWeb.Model.Template_Wfl_Task_Alerts> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.Template_Wfl_Task_Alerts> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Template_Wfl_Task_Alerts> modelList = new List<LPWeb.Model.Template_Wfl_Task_Alerts>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Template_Wfl_Task_Alerts model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Template_Wfl_Task_Alerts();
					if(dt.Rows[n]["TemplAlertId"].ToString()!="")
					{
						model.TemplAlertId=int.Parse(dt.Rows[n]["TemplAlertId"].ToString());
					}
					if(dt.Rows[n]["TemplTaskId"].ToString()!="")
					{
						model.TemplTaskId=int.Parse(dt.Rows[n]["TemplTaskId"].ToString());
					}
					if(dt.Rows[n]["SuccessTemplateId"].ToString()!="")
					{
						model.SuccessTemplateId=int.Parse(dt.Rows[n]["SuccessTemplateId"].ToString());
					}
					if(dt.Rows[n]["WarningTemplateId"].ToString()!="")
					{
						model.WarningTemplateId=int.Parse(dt.Rows[n]["WarningTemplateId"].ToString());
					}
					if(dt.Rows[n]["OverdueTemplateId"].ToString()!="")
					{
						model.OverdueTemplateId=int.Parse(dt.Rows[n]["OverdueTemplateId"].ToString());
					}
					if(dt.Rows[n]["ToContactType"].ToString()!="")
					{
						model.ToContactType=int.Parse(dt.Rows[n]["ToContactType"].ToString());
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

