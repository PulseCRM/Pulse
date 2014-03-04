using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// UserLeadDist
	/// </summary>
	public class UserLeadDist
	{
		private readonly LPWeb.DAL.UserLeadDist dal=new LPWeb.DAL.UserLeadDist();
		public UserLeadDist()
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
		public bool Exists(int UserID)
		{
			return dal.Exists(UserID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.UserLeadDist model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.UserLeadDist model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int UserID)
		{
			
			return dal.Delete(UserID);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string UserIDlist )
		{
			return dal.DeleteList(UserIDlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.UserLeadDist GetModel(int UserID)
		{
			
			return dal.GetModel(UserID);
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
		public List<LPWeb.Model.UserLeadDist> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.UserLeadDist> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.UserLeadDist> modelList = new List<LPWeb.Model.UserLeadDist>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.UserLeadDist model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.UserLeadDist();
					if(dt.Rows[n]["UserID"].ToString()!="")
					{
						model.UserID=int.Parse(dt.Rows[n]["UserID"].ToString());
					}
					if(dt.Rows[n]["EnableLeadRouting"].ToString()!="")
					{
						if((dt.Rows[n]["EnableLeadRouting"].ToString()=="1")||(dt.Rows[n]["EnableLeadRouting"].ToString().ToLower()=="true"))
						{
						model.EnableLeadRouting=true;
						}
						else
						{
							model.EnableLeadRouting=false;
						}
					}
					if(dt.Rows[n]["MaxDailyLeads"].ToString()!="")
					{
						model.MaxDailyLeads=int.Parse(dt.Rows[n]["MaxDailyLeads"].ToString());
					}
					if(dt.Rows[n]["LeadsAssignedToday"].ToString()!="")
					{
						model.LeadsAssignedToday=int.Parse(dt.Rows[n]["LeadsAssignedToday"].ToString());
					}
					if(dt.Rows[n]["LastLeadAssigned"].ToString()!="")
					{
						model.LastLeadAssigned=int.Parse(dt.Rows[n]["LastLeadAssigned"].ToString());
					}
					if(dt.Rows[n]["LastAssigned"].ToString()!="")
					{
						model.LastAssigned=DateTime.Parse(dt.Rows[n]["LastAssigned"].ToString());
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

