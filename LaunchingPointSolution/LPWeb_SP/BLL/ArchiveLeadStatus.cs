using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ArchiveLeadStatus
	/// </summary>
	public class ArchiveLeadStatus
	{
		private readonly LPWeb.DAL.ArchiveLeadStatus dal=new LPWeb.DAL.ArchiveLeadStatus();
		public ArchiveLeadStatus()
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
		public bool Exists(int LeadStatusId)
		{
			return dal.Exists(LeadStatusId);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(LPWeb.Model.ArchiveLeadStatus model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(LPWeb.Model.ArchiveLeadStatus model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int LeadStatusId)
		{
			
			return dal.Delete(LeadStatusId);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool DeleteList(string LeadStatusIdlist )
		{
			return dal.DeleteList(LeadStatusIdlist );
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.ArchiveLeadStatus GetModel(int LeadStatusId)
		{
			
			return dal.GetModel(LeadStatusId);
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
		public List<LPWeb.Model.ArchiveLeadStatus> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.ArchiveLeadStatus> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.ArchiveLeadStatus> modelList = new List<LPWeb.Model.ArchiveLeadStatus>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.ArchiveLeadStatus model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.ArchiveLeadStatus();
					if(dt.Rows[n]["LeadStatusId"].ToString()!="")
					{
						model.LeadStatusId=int.Parse(dt.Rows[n]["LeadStatusId"].ToString());
					}
					model.LeadStatusName=dt.Rows[n]["LeadStatusName"].ToString();
					if(dt.Rows[n]["Enabled"].ToString()!="")
					{
						if((dt.Rows[n]["Enabled"].ToString()=="1")||(dt.Rows[n]["Enabled"].ToString().ToLower()=="true"))
						{
						model.Enabled=true;
						}
						else
						{
							model.Enabled=false;
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
		/// ��ҳ��ȡ�����б�
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  Method
	}
}

