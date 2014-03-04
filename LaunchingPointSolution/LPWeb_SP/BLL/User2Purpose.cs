using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// User2Purpose
	/// </summary>
	public class User2Purpose
	{
		private readonly LPWeb.DAL.User2Purpose dal=new LPWeb.DAL.User2Purpose();
		public User2Purpose()
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
		public bool Exists(int UserID,string Purpose)
		{
			return dal.Exists(UserID,Purpose);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.User2Purpose model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(LPWeb.Model.User2Purpose model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int UserID,string Purpose)
		{
			
			return dal.Delete(UserID,Purpose);
		}

        /// <summary>
        /// ����UserIDɾ��һ������
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
	    public bool DeleteByUserID(int userId)
	    {
	        return dal.DeleteByUserID(userId);
	    }

	    /// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.User2Purpose GetModel(int UserID,string Purpose)
		{
			
			return dal.GetModel(UserID,Purpose);
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
		public List<LPWeb.Model.User2Purpose> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.User2Purpose> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.User2Purpose> modelList = new List<LPWeb.Model.User2Purpose>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.User2Purpose model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.User2Purpose();
					if(dt.Rows[n]["UserID"].ToString()!="")
					{
						model.UserID=int.Parse(dt.Rows[n]["UserID"].ToString());
					}
					model.Purpose=dt.Rows[n]["Purpose"].ToString();
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

