using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// Company_Alerts ��ժҪ˵����
	/// </summary>
	public class Company_Alerts
	{
		private readonly LPWeb.DAL.Company_Alerts dal=new LPWeb.DAL.Company_Alerts();
		public Company_Alerts()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.Company_Alerts model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.Company_Alerts model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete()
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			dal.Delete();
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.Company_Alerts GetModel()
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			return dal.GetModel();
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
		public List<LPWeb.Model.Company_Alerts> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
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

		#endregion  ��Ա����
	}
}

