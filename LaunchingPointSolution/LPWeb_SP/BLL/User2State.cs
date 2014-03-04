using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// User2State
	/// </summary>
	public class User2State
	{
		private readonly LPWeb.DAL.User2State dal=new LPWeb.DAL.User2State();
		public User2State()
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
		public bool Exists(int UserID,string State)
		{
			return dal.Exists(UserID,State);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.User2State model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(LPWeb.Model.User2State model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int UserID,string State)
		{
			return dal.Delete(UserID,State);
		}
        /// <summary>
        /// ����UserIDɾ������
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
	    public bool DeleteByUserID(int UserID)
	    {
	        return dal.DeleteByUserID(UserID);
	    }

	    /// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.User2State GetModel(int UserID,string State)
		{
			
			return dal.GetModel(UserID,State);
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
		public List<LPWeb.Model.User2State> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.User2State> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.User2State> modelList = new List<LPWeb.Model.User2State>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.User2State model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.User2State();
					if(dt.Rows[n]["UserID"].ToString()!="")
					{
						model.UserID=int.Parse(dt.Rows[n]["UserID"].ToString());
					}
					model.State=dt.Rows[n]["State"].ToString();
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

