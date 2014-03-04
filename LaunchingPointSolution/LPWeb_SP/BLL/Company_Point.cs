using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// Company_Point ��ժҪ˵����
	/// </summary>
	public class Company_Point
	{
		private readonly LPWeb.DAL.Company_Point dal=new LPWeb.DAL.Company_Point();
		public Company_Point()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.Company_Point model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.Company_Point model)
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
		public LPWeb.Model.Company_Point GetModel()
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
		public List<LPWeb.Model.Company_Point> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.Company_Point> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Company_Point> modelList = new List<LPWeb.Model.Company_Point>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Company_Point model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Company_Point();
					model.WinpointIniPath=dt.Rows[n]["WinpointIniPath"].ToString();
					model.PointFieldIDMappingFile=dt.Rows[n]["PointFieldIDMappingFile"].ToString();
					model.CardexFile=dt.Rows[n]["CardexFile"].ToString();
					if(dt.Rows[n]["PointImportIntervalMinutes"].ToString()!="")
					{
						model.PointImportIntervalMinutes=int.Parse(dt.Rows[n]["PointImportIntervalMinutes"].ToString());
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

        public DataTable GetCompany_PointInfo()
        {
            return dal.GetCompany_PointInfo();
        }
	}
}

