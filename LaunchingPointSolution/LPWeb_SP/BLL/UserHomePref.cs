using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ҵ���߼���UserHomePref ��ժҪ˵����
	/// </summary>
	public class UserHomePref
	{
		private readonly LPWeb.DAL.UserHomePref dal=new LPWeb.DAL.UserHomePref();
		public UserHomePref()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.UserHomePref model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.UserHomePref model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int UserId)
		{
			
			dal.Delete(UserId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.UserHomePref GetModel(int UserId)
		{
			
			return dal.GetModel(UserId);
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
		public List<LPWeb.Model.UserHomePref> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.UserHomePref> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.UserHomePref> modelList = new List<LPWeb.Model.UserHomePref>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.UserHomePref model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.UserHomePref();
					if(dt.Rows[n]["UserId"].ToString()!="")
					{
						model.UserId=int.Parse(dt.Rows[n]["UserId"].ToString());
					}
					if(dt.Rows[n]["CompanyCalendar"].ToString()!="")
					{
						if((dt.Rows[n]["CompanyCalendar"].ToString()=="1")||(dt.Rows[n]["CompanyCalendar"].ToString().ToLower()=="true"))
						{
						model.CompanyCalendar=true;
						}
						else
						{
							model.CompanyCalendar=false;
						}
					}
					if(dt.Rows[n]["PipelineChart"].ToString()!="")
					{
						if((dt.Rows[n]["PipelineChart"].ToString()=="1")||(dt.Rows[n]["PipelineChart"].ToString().ToLower()=="true"))
						{
						model.PipelineChart=true;
						}
						else
						{
							model.PipelineChart=false;
						}
					}
					if(dt.Rows[n]["SalesBreakdownChart"].ToString()!="")
					{
						if((dt.Rows[n]["SalesBreakdownChart"].ToString()=="1")||(dt.Rows[n]["SalesBreakdownChart"].ToString().ToLower()=="true"))
						{
						model.SalesBreakdownChart=true;
						}
						else
						{
							model.SalesBreakdownChart=false;
						}
					}
					if(dt.Rows[n]["OrgProductionChart"].ToString()!="")
					{
						if((dt.Rows[n]["OrgProductionChart"].ToString()=="1")||(dt.Rows[n]["OrgProductionChart"].ToString().ToLower()=="true"))
						{
						model.OrgProductionChart=true;
						}
						else
						{
							model.OrgProductionChart=false;
						}
					}
					if(dt.Rows[n]["Org_N_Sales_Charts"].ToString()!="")
					{
						if((dt.Rows[n]["Org_N_Sales_Charts"].ToString()=="1")||(dt.Rows[n]["Org_N_Sales_Charts"].ToString().ToLower()=="true"))
						{
						model.Org_N_Sales_Charts=true;
						}
						else
						{
							model.Org_N_Sales_Charts=false;
						}
					}
					if(dt.Rows[n]["RateSummary"].ToString()!="")
					{
						if((dt.Rows[n]["RateSummary"].ToString()=="1")||(dt.Rows[n]["RateSummary"].ToString().ToLower()=="true"))
						{
						model.RateSummary=true;
						}
						else
						{
							model.RateSummary=false;
						}
					}
					if(dt.Rows[n]["GoalsChart"].ToString()!="")
					{
						if((dt.Rows[n]["GoalsChart"].ToString()=="1")||(dt.Rows[n]["GoalsChart"].ToString().ToLower()=="true"))
						{
						model.GoalsChart=true;
						}
						else
						{
							model.GoalsChart=false;
						}
					}
					if(dt.Rows[n]["OverDueTaskAlert"].ToString()!="")
					{
						if((dt.Rows[n]["OverDueTaskAlert"].ToString()=="1")||(dt.Rows[n]["OverDueTaskAlert"].ToString().ToLower()=="true"))
						{
						model.OverDueTaskAlert=true;
						}
						else
						{
							model.OverDueTaskAlert=false;
						}
					}
					if(dt.Rows[n]["Announcements"].ToString()!="")
					{
						if((dt.Rows[n]["Announcements"].ToString()=="1")||(dt.Rows[n]["Announcements"].ToString().ToLower()=="true"))
						{
						model.Announcements=true;
						}
						else
						{
							model.Announcements=false;
						}
					}
					if(dt.Rows[n]["ExchangeInbox"].ToString()!="")
					{
						if((dt.Rows[n]["ExchangeInbox"].ToString()=="1")||(dt.Rows[n]["ExchangeInbox"].ToString().ToLower()=="true"))
						{
						model.ExchangeInbox=true;
						}
						else
						{
							model.ExchangeInbox=false;
						}
					}
					if(dt.Rows[n]["ExchangeCalendar"].ToString()!="")
					{
						if((dt.Rows[n]["ExchangeCalendar"].ToString()=="1")||(dt.Rows[n]["ExchangeCalendar"].ToString().ToLower()=="true"))
						{
						model.ExchangeCalendar=true;
						}
						else
						{
							model.ExchangeCalendar=false;
						}
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
		/// ��������б�
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			return dal.GetList(PageSize,PageIndex,strWhere);
		}

        /// <summary>
        /// copy user Home perf info
        /// </summary>
        /// <param name="nSourceUserID"></param>
        /// <param name="nDistUserID"></param>
        public void CopyUserHomePrefInfo(int nSourceUserID, int nDistUserID)
        {
            Model.UserHomePref userHomePref = dal.GetModel(nSourceUserID);
            if (null != userHomePref)
            {
                userHomePref.UserId = nDistUserID;
                dal.Add(userHomePref);
            }
        }
		#endregion  ��Ա����
	}
}

