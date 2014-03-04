using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// Company_Web 的摘要说明。
	/// </summary>
	public class Company_Web
	{
		private readonly LPWeb.DAL.Company_Web dal=new LPWeb.DAL.Company_Web();
		public Company_Web()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.Company_Web model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.Company_Web model)
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
		public LPWeb.Model.Company_Web GetModel()
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
		public List<LPWeb.Model.Company_Web> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.Company_Web> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Company_Web> modelList = new List<LPWeb.Model.Company_Web>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Company_Web model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Company_Web();
					if(dt.Rows[n]["EmailAlertsEnabled"].ToString()!="")
					{
						if((dt.Rows[n]["EmailAlertsEnabled"].ToString()=="1")||(dt.Rows[n]["EmailAlertsEnabled"].ToString().ToLower()=="true"))
						{
						model.EmailAlertsEnabled=true;
						}
						else
						{
							model.EmailAlertsEnabled=false;
						}
					}
					model.EmailRelayServer=dt.Rows[n]["EmailRelayServer"].ToString();
					model.DefaultAlertEmail=dt.Rows[n]["DefaultAlertEmail"].ToString();
					if(dt.Rows[n]["EmailInterval"].ToString()!="")
					{
						model.EmailInterval=int.Parse(dt.Rows[n]["EmailInterval"].ToString());
					}
					model.LPCompanyURL=dt.Rows[n]["LPCompanyURL"].ToString();
					model.BorrowerURL=dt.Rows[n]["BorrowerURL"].ToString();
					model.BorrowerGreeting=dt.Rows[n]["BorrowerGreeting"].ToString();
					model.HomePageLogo=dt.Rows[n]["HomePageLogo"].ToString();
					model.LogoForSubPages=dt.Rows[n]["LogoForSubPages"].ToString();
					if(dt.Rows[n]["HomePageLogoData"].ToString()!="")
					{
						model.HomePageLogoData=(byte[])dt.Rows[n]["HomePageLogoData"];
					}
					if(dt.Rows[n]["SubPageLogoData"].ToString()!="")
					{
						model.SubPageLogoData=(byte[])dt.Rows[n]["SubPageLogoData"];
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
        public string GetWcfUrl()
        {
            return dal.GetWcfUrl();
        }

	    #endregion  成员方法
	}
}

