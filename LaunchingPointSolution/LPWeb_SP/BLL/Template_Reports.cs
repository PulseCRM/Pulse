using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类Template_Reports 的摘要说明。
	/// </summary>
	public class Template_Reports
	{
		private readonly LPWeb.DAL.Template_Reports dal=new LPWeb.DAL.Template_Reports();
		public Template_Reports()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.Template_Reports model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.Template_Reports model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int TemplReportId)
		{
			
			dal.Delete(TemplReportId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.Template_Reports GetModel(int TemplReportId)
		{
			
			return dal.GetModel(TemplReportId);
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
		public List<LPWeb.Model.Template_Reports> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.Template_Reports> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Template_Reports> modelList = new List<LPWeb.Model.Template_Reports>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Template_Reports model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Template_Reports();
					if(dt.Rows[n]["TemplReportId"].ToString()!="")
					{
						model.TemplReportId=int.Parse(dt.Rows[n]["TemplReportId"].ToString());
					}
					model.Name=dt.Rows[n]["Name"].ToString();
					if(dt.Rows[n]["Standard"].ToString()!="")
					{
						if((dt.Rows[n]["Standard"].ToString()=="1")||(dt.Rows[n]["Standard"].ToString().ToLower()=="true"))
						{
						model.Standard=true;
						}
						else
						{
							model.Standard=false;
						}
					}
					model.HtmlTemplContent=dt.Rows[n]["HtmlTemplContent"].ToString();
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

        ///// <summary>
        ///// 获得数据列表
        ///// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //    return dal.GetList(PageSize,PageIndex,strWhere);
        //}

		#endregion  成员方法
	}
}

