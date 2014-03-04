using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// CompanyExecutives ��ժҪ˵����
	/// </summary>
	public class CompanyExecutives
	{
		private readonly LPWeb.DAL.CompanyExecutives dal=new LPWeb.DAL.CompanyExecutives();
		public CompanyExecutives()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.CompanyExecutives model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.CompanyExecutives model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ExecutiveId)
		{
			
			dal.Delete(ExecutiveId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.CompanyExecutives GetModel(int ExecutiveId)
		{
			
			return dal.GetModel(ExecutiveId);
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
		public List<LPWeb.Model.CompanyExecutives> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.CompanyExecutives> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.CompanyExecutives> modelList = new List<LPWeb.Model.CompanyExecutives>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.CompanyExecutives model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.CompanyExecutives();
					if(dt.Rows[n]["ExecutiveId"].ToString()!="")
					{
						model.ExecutiveId=int.Parse(dt.Rows[n]["ExecutiveId"].ToString());
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

