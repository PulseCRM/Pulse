using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ProspectIncome
	/// </summary>
	public partial class ProspectIncome
	{
		private readonly LPWeb.DAL.ProspectIncome dal=new LPWeb.DAL.ProspectIncome();
		public ProspectIncome()
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
		public bool Exists(int ProspectIncomeId)
		{
			return dal.Exists(ProspectIncomeId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.ProspectIncome model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.ProspectIncome model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int ProspectIncomeId)
		{
			
			return dal.Delete(ProspectIncomeId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string ProspectIncomeIdlist )
		{
			return dal.DeleteList(ProspectIncomeIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.ProspectIncome GetModel(int ProspectIncomeId)
		{
			
			return dal.GetModel(ProspectIncomeId);
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
		public List<LPWeb.Model.ProspectIncome> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.ProspectIncome> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.ProspectIncome> modelList = new List<LPWeb.Model.ProspectIncome>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.ProspectIncome model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.ProspectIncome();
					if(dt.Rows[n]["ProspectIncomeId"]!=null && dt.Rows[n]["ProspectIncomeId"].ToString()!="")
					{
						model.ProspectIncomeId=int.Parse(dt.Rows[n]["ProspectIncomeId"].ToString());
					}
					if(dt.Rows[n]["ContactId"]!=null && dt.Rows[n]["ContactId"].ToString()!="")
					{
						model.ContactId=int.Parse(dt.Rows[n]["ContactId"].ToString());
					}
					if(dt.Rows[n]["Salary"]!=null && dt.Rows[n]["Salary"].ToString()!="")
					{
						model.Salary=decimal.Parse(dt.Rows[n]["Salary"].ToString());
					}
					if(dt.Rows[n]["Overtime"]!=null && dt.Rows[n]["Overtime"].ToString()!="")
					{
						model.Overtime=decimal.Parse(dt.Rows[n]["Overtime"].ToString());
					}
					if(dt.Rows[n]["Bonuses"]!=null && dt.Rows[n]["Bonuses"].ToString()!="")
					{
						model.Bonuses=decimal.Parse(dt.Rows[n]["Bonuses"].ToString());
					}
					if(dt.Rows[n]["Commission"]!=null && dt.Rows[n]["Commission"].ToString()!="")
					{
						model.Commission=decimal.Parse(dt.Rows[n]["Commission"].ToString());
					}
					if(dt.Rows[n]["Div_Int"]!=null && dt.Rows[n]["Div_Int"].ToString()!="")
					{
						model.Div_Int=decimal.Parse(dt.Rows[n]["Div_Int"].ToString());
					}
					if(dt.Rows[n]["NetRent"]!=null && dt.Rows[n]["NetRent"].ToString()!="")
					{
						model.NetRent=decimal.Parse(dt.Rows[n]["NetRent"].ToString());
					}
					if(dt.Rows[n]["Other"]!=null && dt.Rows[n]["Other"].ToString()!="")
					{
						model.Other=decimal.Parse(dt.Rows[n]["Other"].ToString());
					}
					if(dt.Rows[n]["EmplId"]!=null && dt.Rows[n]["EmplId"].ToString()!="")
					{
						model.EmplId=int.Parse(dt.Rows[n]["EmplId"].ToString());
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

        public void UpdateIncome(int emplid, string salary)
        {
            if (!dal.ExistIncome(emplid))
                dal.InsertIncome(emplid, salary);
            else
                dal.UpdateIncome(emplid, salary);
        }

        public DataTable GetProspectIncome(int iContactID)
        {
            return dal.GetProspectIncome(iContactID);
        }
	}
}

