using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// PointFieldDesc ��ժҪ˵����
	/// </summary>
	public class PointFieldDesc
	{
		private readonly LPWeb.DAL.PointFieldDesc dal=new LPWeb.DAL.PointFieldDesc();
		public PointFieldDesc()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.PointFieldDesc model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.PointFieldDesc model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(decimal PointFieldId)
		{
			
			dal.Delete(PointFieldId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.PointFieldDesc GetModel(decimal PointFieldId)
		{
			
			return dal.GetModel(PointFieldId);
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
		public List<LPWeb.Model.PointFieldDesc> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.PointFieldDesc> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.PointFieldDesc> modelList = new List<LPWeb.Model.PointFieldDesc>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.PointFieldDesc model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.PointFieldDesc();
					if(dt.Rows[n]["PointFieldId"].ToString()!="")
					{
						model.PointFieldId=decimal.Parse(dt.Rows[n]["PointFieldId"].ToString());
					}
					model.Label=dt.Rows[n]["Label"].ToString();
					if(dt.Rows[n]["DataType"].ToString()!="")
					{
						model.DataType=int.Parse(dt.Rows[n]["DataType"].ToString());
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

