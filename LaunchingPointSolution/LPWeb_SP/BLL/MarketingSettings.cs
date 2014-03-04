using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// MarketingSettings
	/// </summary>
	public class MarketingSettings
	{
		private readonly LPWeb.DAL.MarketingSettings dal=new LPWeb.DAL.MarketingSettings();
		public MarketingSettings()
		{}
		#region  Method

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.MarketingSettings model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(LPWeb.Model.MarketingSettings model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete()
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			return dal.Delete();
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.MarketingSettings GetModel()
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
		public List<LPWeb.Model.MarketingSettings> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.MarketingSettings> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.MarketingSettings> modelList = new List<LPWeb.Model.MarketingSettings>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.MarketingSettings model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.MarketingSettings();
					model.WebServiceURL=dt.Rows[n]["WebServiceURL"].ToString();
					model.CampaignDetailURL=dt.Rows[n]["CampaignDetailURL"].ToString();
					if(dt.Rows[n]["ReconcileInterval"].ToString()!="")
					{
						model.ReconcileInterval=int.Parse(dt.Rows[n]["ReconcileInterval"].ToString());
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

