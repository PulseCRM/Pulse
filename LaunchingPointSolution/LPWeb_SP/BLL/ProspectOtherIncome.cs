using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ProspectOtherIncome
	/// </summary>
	public partial class ProspectOtherIncome
	{
		private readonly LPWeb.DAL.ProspectOtherIncome dal=new LPWeb.DAL.ProspectOtherIncome();
		public ProspectOtherIncome()
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
		public bool Exists(int ProspectOtherIncomeId)
		{
			return dal.Exists(ProspectOtherIncomeId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.ProspectOtherIncome model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.ProspectOtherIncome model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int ProspectOtherIncomeId)
		{
			
			return dal.Delete(ProspectOtherIncomeId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string ProspectOtherIncomeIdlist )
		{
			return dal.DeleteList(ProspectOtherIncomeIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.ProspectOtherIncome GetModel(int ProspectOtherIncomeId)
		{
			
			return dal.GetModel(ProspectOtherIncomeId);
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
		public List<LPWeb.Model.ProspectOtherIncome> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.ProspectOtherIncome> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.ProspectOtherIncome> modelList = new List<LPWeb.Model.ProspectOtherIncome>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.ProspectOtherIncome model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.ProspectOtherIncome();
					if(dt.Rows[n]["ProspectOtherIncomeId"]!=null && dt.Rows[n]["ProspectOtherIncomeId"].ToString()!="")
					{
						model.ProspectOtherIncomeId=int.Parse(dt.Rows[n]["ProspectOtherIncomeId"].ToString());
					}
					if(dt.Rows[n]["ContactId"]!=null && dt.Rows[n]["ContactId"].ToString()!="")
					{
						model.ContactId=int.Parse(dt.Rows[n]["ContactId"].ToString());
					}
					if(dt.Rows[n]["Type"]!=null && dt.Rows[n]["Type"].ToString()!="")
					{
					model.Type=dt.Rows[n]["Type"].ToString();
					}
					if(dt.Rows[n]["MonthlyIncome"]!=null && dt.Rows[n]["MonthlyIncome"].ToString()!="")
					{
						model.MonthlyIncome=decimal.Parse(dt.Rows[n]["MonthlyIncome"].ToString());
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
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  Method
	}
}

