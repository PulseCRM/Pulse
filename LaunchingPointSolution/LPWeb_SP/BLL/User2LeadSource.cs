using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// User2LeadSource
	/// </summary>
	public class User2LeadSource
	{
		private readonly LPWeb.DAL.User2LeadSource dal=new LPWeb.DAL.User2LeadSource();
		public User2LeadSource()
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
		public bool Exists(int UserID,int LeadSourceID)
		{
			return dal.Exists(UserID,LeadSourceID);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.User2LeadSource model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(LPWeb.Model.User2LeadSource model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int UserID,int LeadSourceID)
		{
			
			return dal.Delete(UserID,LeadSourceID);
		}

	    public bool DeleteByUserID(int userID)
	    {
            return dal.DeleteByUserID(userID);
	    }

	    /// <summary>
        /// ɾ����������
        /// </summary>
        /// <param name="leadSourceIds"></param>
        /// <returns></returns>
	    public void Delete(String leadSourceIds)
	    {
            string[] leadSources = leadSourceIds.Split(',');
            foreach (var leadSource in leadSources)
            {
                dal.Delete(int.Parse(leadSource));
            }
	    }

	    /// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.User2LeadSource GetModel(int UserID,int LeadSourceID)
		{
			
			return dal.GetModel(UserID,LeadSourceID);
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
		public List<LPWeb.Model.User2LeadSource> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.User2LeadSource> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.User2LeadSource> modelList = new List<LPWeb.Model.User2LeadSource>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.User2LeadSource model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.User2LeadSource();
					if(dt.Rows[n]["UserID"].ToString()!="")
					{
						model.UserID=int.Parse(dt.Rows[n]["UserID"].ToString());
					}
					if(dt.Rows[n]["LeadSourceID"].ToString()!="")
					{
						model.LeadSourceID=int.Parse(dt.Rows[n]["LeadSourceID"].ToString());
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

