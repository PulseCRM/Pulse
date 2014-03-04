using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// LoanContacts ��ժҪ˵����
	/// </summary>
	public class LoanContacts
	{
		private readonly LPWeb.DAL.LoanContacts dal=new LPWeb.DAL.LoanContacts();
		public LoanContacts()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.LoanContacts model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.LoanContacts model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int FileId,int ContactRoleId,int ContactId)
		{
			
			dal.Delete(FileId,ContactRoleId,ContactId);
		}

        public void Reassign(LPWeb.Model.LoanContacts oldModel, LPWeb.Model.LoanContacts model, int UserId)
        {
            dal.Reassign(oldModel,model, UserId);

        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.LoanContacts GetModel(int FileId,int ContactRoleId,int ContactId)
		{
			
			return dal.GetModel(FileId,ContactRoleId,ContactId);
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
		public List<LPWeb.Model.LoanContacts> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.LoanContacts> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.LoanContacts> modelList = new List<LPWeb.Model.LoanContacts>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.LoanContacts model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.LoanContacts();
					if(dt.Rows[n]["FileId"].ToString()!="")
					{
						model.FileId=int.Parse(dt.Rows[n]["FileId"].ToString());
					}
					if(dt.Rows[n]["ContactRoleId"].ToString()!="")
					{
						model.ContactRoleId=int.Parse(dt.Rows[n]["ContactRoleId"].ToString());
					}
					if(dt.Rows[n]["ContactId"].ToString()!="")
					{
						model.ContactId=int.Parse(dt.Rows[n]["ContactId"].ToString());
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
        public DataSet GetLoanContacts(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetLoanContacts(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }
        public DataSet GetLoanContactsReassign(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetLoanContactsReassign(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }
        public void DeleteLoanContacts(int FileID, string Contacts)
        {
            dal.DeleteLoanContacts(FileID, Contacts);
        }

        public DataSet GetLoanContactsForReassign(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetLoanContactsForReassign(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        public DataSet GetDistinctLoanContactsForReassign(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetDistinctLoanContactsForReassign(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        public DataSet GetProspectLoanContacts(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetProspectLoanContacts(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }


        public DataSet GetContactLoans(int ContactID)
        {
            return dal.GetContactLoans(ContactID);
        }

        public void UpdateContactId(int iFileId, int iContactRoleId, int iNewContactId)
        {
            this.dal.UpdateContactId(iFileId, iContactRoleId, iNewContactId);
        }
	}
}

