using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类WebAccounts 的摘要说明。
	/// </summary>
	public class WebAccounts
	{
		private readonly LPWeb.DAL.WebAccounts dal=new LPWeb.DAL.WebAccounts();
		public WebAccounts()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.WebAccounts model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.WebAccounts model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int WebAccountId)
		{
			
			dal.Delete(WebAccountId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.WebAccounts GetModel(int WebAccountId)
		{
			
			return dal.GetModel(WebAccountId);
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
		public List<LPWeb.Model.WebAccounts> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.WebAccounts> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.WebAccounts> modelList = new List<LPWeb.Model.WebAccounts>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.WebAccounts model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.WebAccounts();
					if(dt.Rows[n]["WebAccountId"].ToString()!="")
					{
						model.WebAccountId=int.Parse(dt.Rows[n]["WebAccountId"].ToString());
					}
					if(dt.Rows[n]["Enabled"].ToString()!="")
					{
						if((dt.Rows[n]["Enabled"].ToString()=="1")||(dt.Rows[n]["Enabled"].ToString().ToLower()=="true"))
						{
						model.Enabled=true;
						}
						else
						{
							model.Enabled=false;
						}
					}
					if(dt.Rows[n]["LastLogin"].ToString()!="")
					{
						model.LastLogin=DateTime.Parse(dt.Rows[n]["LastLogin"].ToString());
					}
					model.Password=dt.Rows[n]["Password"].ToString();
					model.PasswordQuestion=dt.Rows[n]["PasswordQuestion"].ToString();
					model.PasswordAnswer=dt.Rows[n]["PasswordAnswer"].ToString();
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

