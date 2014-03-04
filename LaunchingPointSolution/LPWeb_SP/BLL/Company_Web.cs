using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// Company_Web ��ժҪ˵����
	/// </summary>
	public class Company_Web
	{
		private readonly LPWeb.DAL.Company_Web dal=new LPWeb.DAL.Company_Web();
		public Company_Web()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.Company_Web model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.Company_Web model)
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
		public LPWeb.Model.Company_Web GetModel()
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
		public List<LPWeb.Model.Company_Web> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
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
        public string GetWcfUrl()
        {
            return dal.GetWcfUrl();
        }

	    #endregion  ��Ա����
	}
}

