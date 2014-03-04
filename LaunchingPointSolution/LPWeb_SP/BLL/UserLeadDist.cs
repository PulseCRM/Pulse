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
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int UserID)
		{
			return dal.Exists(UserID);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.UserLeadDist model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(LPWeb.Model.UserLeadDist model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int UserID)
		{
			
			return dal.Delete(UserID);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool DeleteList(string UserIDlist )
		{
			return dal.DeleteList(UserIDlist );
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.UserLeadDist GetModel(int UserID)
		{
			
			return dal.GetModel(UserID);
		}

		

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// ���ǰ��������
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.UserLeadDist> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
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
		/// ��������б�
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// ��ҳ��ȡ�����б�
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  Method
	}
}

