using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ҵ���߼���UserLoanRep ��ժҪ˵����
	/// </summary>
	public class UserLoanRep
	{
		private readonly LPWeb.DAL.UserLoanRep dal=new LPWeb.DAL.UserLoanRep();
		public UserLoanRep()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(LPWeb.Model.UserLoanRep model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.UserLoanRep model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int NameId)
		{
			
			dal.Delete(NameId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.UserLoanRep GetModel(int NameId)
		{
			
			return dal.GetModel(NameId);
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
		public List<LPWeb.Model.UserLoanRep> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.UserLoanRep> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.UserLoanRep> modelList = new List<LPWeb.Model.UserLoanRep>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.UserLoanRep model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.UserLoanRep();
					if(dt.Rows[n]["NameId"].ToString()!="")
					{
						model.NameId=int.Parse(dt.Rows[n]["NameId"].ToString());
					}
					if(dt.Rows[n]["BranchId"].ToString()!="")
					{
						model.BranchId=int.Parse(dt.Rows[n]["BranchId"].ToString());
					}
					model.Name=dt.Rows[n]["Name"].ToString();
					if(dt.Rows[n]["UserId"].ToString()!="")
					{
						model.UserId=int.Parse(dt.Rows[n]["UserId"].ToString());
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

        /// <summary>
        /// ����UserLoanRep��ϵ
        /// </summary>
        /// <param name="nUserID"></param>
        /// <param name="strLoanRepIds"></param>
        public void SaveUserLoanRep(int nUserID, string strLoanRepIds)
        {
            if (!string.IsNullOrEmpty(strLoanRepIds))
                dal.SaveUserLoanRep(nUserID, strLoanRepIds);
            else
                dal.DeleteLoanRepMapping(null, nUserID);
        }

        /// <summary>
        /// ɾ���û�Loan Rep Mapping
        /// </summary>
        public void DeleteLoanRepMapping(int nLoanRepID, int nUserID)
        {
            dal.DeleteLoanRepMapping(nLoanRepID, nUserID);
        }

		#endregion  ��Ա����
	}
}

