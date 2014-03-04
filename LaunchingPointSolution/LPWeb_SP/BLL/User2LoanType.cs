using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// User2LoanType
	/// </summary>
	public class User2LoanType
	{
		private readonly LPWeb.DAL.User2LoanType dal=new LPWeb.DAL.User2LoanType();
		public User2LoanType()
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
		public bool Exists(int UserID,string LoanType)
		{
			return dal.Exists(UserID,LoanType);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.User2LoanType model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(LPWeb.Model.User2LoanType model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int UserID,string LoanType)
		{
			
			return dal.Delete(UserID,LoanType);
		}

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public bool DeleteByUser(int UserID)
        {

            return dal.DeleteByUser(UserID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.User2LoanType GetModel(int UserID,string LoanType)
		{
			
			return dal.GetModel(UserID,LoanType);
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
		public List<LPWeb.Model.User2LoanType> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.User2LoanType> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.User2LoanType> modelList = new List<LPWeb.Model.User2LoanType>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.User2LoanType model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.User2LoanType();
					if(dt.Rows[n]["UserID"].ToString()!="")
					{
						model.UserID=int.Parse(dt.Rows[n]["UserID"].ToString());
					}
					model.LoanType=dt.Rows[n]["LoanType"].ToString();
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

