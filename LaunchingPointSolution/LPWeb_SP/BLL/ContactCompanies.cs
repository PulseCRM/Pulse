using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;

namespace LPWeb.BLL
{
	/// <summary>
	/// ContactCompanies ��ժҪ˵����
	/// </summary>
	public class ContactCompanies
	{
		private readonly LPWeb.DAL.ContactCompanies dal=new LPWeb.DAL.ContactCompanies();
		public ContactCompanies()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(LPWeb.Model.ContactCompanies model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.ContactCompanies model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ContactCompanyId)
		{
			
			dal.Delete(ContactCompanyId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.ContactCompanies GetModel(int ContactCompanyId)
		{			
			return dal.GetModel(ContactCompanyId);
		}

        public LPWeb.Model.ContactCompanies GetModelbyName(string Name)
        {
            return dal.GetModelbyName(Name);
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
		public List<LPWeb.Model.ContactCompanies> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.ContactCompanies> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.ContactCompanies> modelList = new List<LPWeb.Model.ContactCompanies>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.ContactCompanies model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.ContactCompanies();
					if(dt.Rows[n]["ContactCompanyId"].ToString()!="")
					{
						model.ContactCompanyId=int.Parse(dt.Rows[n]["ContactCompanyId"].ToString());
					}
					model.Name=dt.Rows[n]["Name"].ToString();
					model.Address=dt.Rows[n]["Address"].ToString();
					model.City=dt.Rows[n]["City"].ToString();
					model.State=dt.Rows[n]["State"].ToString();
					model.Zip=dt.Rows[n]["Zip"].ToString();
					model.ServiceTypes=dt.Rows[n]["ServiceTypes"].ToString();
                    model.Enabled = dt.Rows[n]["Enabled"] == DBNull.Value ? false : Convert.ToBoolean(dt.Rows[n]["Enabled"]);
                    model.ServiceTypeId = dt.Rows[n]["ServiceTypeId"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[n]["ServiceTypeId"]);

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

        #region neo

        /// <summary>
        /// insert contact company
        /// neo 2011-04-08
        /// </summary>
        /// <param name="sCompanyName"></param>
        /// <param name="iServiceTypeID"></param>
        /// <param name="sServiceType"></param>
        /// <param name="sAddress"></param>
        /// <param name="sCity"></param>
        /// <param name="sState"></param>
        /// <param name="sZip"></param>
        /// <param name="strWebsite"></param>
        public void InsertContactCompany(string sCompanyName, int iServiceTypeID, string sServiceType, string sAddress, string sCity, string sState, string sZip, string strWebsite)
        {
            this.dal.InsertContactCompanyBase(sCompanyName, iServiceTypeID, sServiceType, sAddress, sCity, sState, sZip, strWebsite);
        }

        /// <summary>
        /// get contact company info
        /// neo 2011-04-08
        /// </summary>
        /// <param name="iContactCompanyID"></param>
        /// <returns></returns>
        public DataTable GetContactCompanyInfo(int iContactCompanyID)
        {
            return dal.GetContactCompanyInfoBase(iContactCompanyID);
        }

        /// <summary>
        /// update contact company info
        /// neo 2011-04-08
        /// </summary>
        /// <param name="iContactCompanyID"></param>
        /// <param name="sCompanyName"></param>
        /// <param name="iServiceTypeID"></param>
        /// <param name="sServiceType"></param>
        /// <param name="sAddress"></param>
        /// <param name="sCity"></param>
        /// <param name="sState"></param>
        /// <param name="sZip"></param>
        /// <param name="bEnabled"></param>
        public void UpdateContactCompany(int iContactCompanyID, string sCompanyName, int iServiceTypeID, string sServiceType, string sAddress, string sCity, string sState, string sZip, bool bEnabled, string strWebSite)
        {
            dal.UpdateContactCompanyBase(iContactCompanyID, sCompanyName, iServiceTypeID, sServiceType, sAddress, sCity, sState, sZip, bEnabled, strWebSite);
        }

        /// <summary>
        /// add partner branch to partner company
        /// neo 2011-04-12
        /// </summary>
        /// <param name="iCompanyID"></param>
        /// <param name="sBranchIDs"></param>
        public void AddBranchToCompany(int iCompanyID, string sBranchIDs)
        {
            dal.AddBranchToCompanyBase(iCompanyID, sBranchIDs);
        }

        /// <summary>
        /// remove partner branch from partner company
        /// neo 2011-04-12
        /// </summary>
        /// <param name="sBranchIDs"></param>
        public void RemoveBranchFromCompany(string sBranchIDs)
        {
            dal.RemoveBranchFromCompanyBase(sBranchIDs);
        }

        /// <summary>
        /// delete partner company
        /// neo 2011-04-14
        /// </summary>
        /// <param name="iCompanyID"></param>
        public void DeletePartnerCompany(int iCompanyID)
        {
            dal.DeletePartnerCompanyBase(iCompanyID);
        }

        /// <summary>
        /// disable partner company
        /// neo 2011-04-17
        /// </summary>
        /// <param name="iCompanyID"></param>
        public void DisablePartnerCompany(int iCompanyID)
        {
            dal.DisablePartnerCompanyBase(iCompanyID);
        }

        #endregion

        /// <summary>
        /// ���ҹ�˾
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable Search(string strWhere)
        {
            return dal.Search(strWhere);
        }

        /// <summary>
        /// Add by wangxiao for getting branch info in Employment Detail popup
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable SearchSingleContact(string strWhere)
        {
            return dal.SearchSingleContact(strWhere);
        }
    }
}

