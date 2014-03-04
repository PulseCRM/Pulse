using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// Contacts 的摘要说明。
    /// </summary>
    public class Contacts
    {
        private readonly LPWeb.DAL.Contacts dal = new LPWeb.DAL.Contacts();
        public Contacts()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Contacts model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Contacts model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ContactId)
        {

            dal.Delete(ContactId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Contacts GetModel(int ContactId)
        {

            return dal.GetModel(ContactId);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Contacts> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Contacts> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Contacts> modelList = new List<LPWeb.Model.Contacts>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Contacts model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Contacts();
                    if (dt.Rows[n]["ContactId"].ToString() != "")
                    {
                        model.ContactId = int.Parse(dt.Rows[n]["ContactId"].ToString());
                    }
                    model.FirstName = dt.Rows[n]["FirstName"].ToString();
                    model.MiddleName = dt.Rows[n]["MiddleName"].ToString();
                    model.LastName = dt.Rows[n]["LastName"].ToString();
                    model.NickName = dt.Rows[n]["NickName"].ToString();
                    model.Title = dt.Rows[n]["Title"].ToString();
                    model.GenerationCode = dt.Rows[n]["GenerationCode"].ToString();
                    model.SSN = dt.Rows[n]["SSN"].ToString();
                    model.HomePhone = dt.Rows[n]["HomePhone"].ToString();
                    model.CellPhone = dt.Rows[n]["CellPhone"].ToString();
                    model.BusinessPhone = dt.Rows[n]["BusinessPhone"].ToString();
                    model.Fax = dt.Rows[n]["Fax"].ToString();
                    model.Email = dt.Rows[n]["Email"].ToString();
                    if (dt.Rows[n]["DOB"].ToString() != "")
                    {
                        model.DOB = DateTime.Parse(dt.Rows[n]["DOB"].ToString());
                    }
                    if (dt.Rows[n]["Experian"].ToString() != "")
                    {
                        model.Experian = int.Parse(dt.Rows[n]["Experian"].ToString());
                    }
                    if (dt.Rows[n]["TransUnion"].ToString() != "")
                    {
                        model.TransUnion = int.Parse(dt.Rows[n]["TransUnion"].ToString());
                    }
                    if (dt.Rows[n]["Equifax"].ToString() != "")
                    {
                        model.Equifax = int.Parse(dt.Rows[n]["Equifax"].ToString());
                    }
                    model.MailingAddr = dt.Rows[n]["MailingAddr"].ToString();
                    model.MailingCity = dt.Rows[n]["MailingCity"].ToString();
                    model.MailingState = dt.Rows[n]["MailingState"].ToString();
                    model.MailingZip = dt.Rows[n]["MailingZip"].ToString();
                    if (dt.Rows[n]["ContactCompanyId"].ToString() != "")
                    {
                        model.ContactCompanyId = int.Parse(dt.Rows[n]["ContactCompanyId"].ToString());
                    }
                    if (dt.Rows[n]["WebAccountId"].ToString() != "")
                    {
                        model.WebAccountId = int.Parse(dt.Rows[n]["WebAccountId"].ToString());
                    }
                    if (dt.Rows[n]["ContactEnable"].ToString() != "")
                    {
                        if ((dt.Rows[n]["ContactEnable"].ToString() == "1") || (dt.Rows[n]["ContactEnable"].ToString().ToLower() == "true"))
                        {
                            model.ContactEnable = true;
                        }
                        else
                        {
                            model.ContactEnable = false;
                        }
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            return dal.GetList(PageSize, PageIndex, strWhere);
        }

        #endregion  成员方法
        /// <summary>
        /// get contact name
        /// 
        /// </summary>
        /// <param name="iContactID"></param>
        /// <returns></returns>
        public string GetContactName(int iContactID)
        {
            return dal.GetContactName(iContactID);
        }

        /// <summary>
        /// Get Borrower
        /// </summary>
        public string GetBorrower(int FileID)
        {
            return dal.GetBorrower(FileID);
        }

        /// <summary>
        /// Get CoBorrower
        /// </summary>
        public string GetCoBorrower(int FileID)
        {
            return dal.GetCoBorrower(FileID);
        }

        /// <summary>
        /// Get Borrower
        /// </summary>
        public string GetBorrowerLastName(int FileID)
        {
            string Borrower = string.Empty;
            try
            {
                DataSet ds = dal.GetLoanContacts(FileID, "Borrower");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    Borrower = row["LastName"].ToString();
                }
            }
            catch
            {
            }
            return Borrower;
        }

        /// <summary>
        /// Get Borrower
        /// </summary>
        public DataRow GetBorrowerDetails(int FileID, string Borrower)
        {
            //string Borrower = "Borrower";Borrower CoBorrower
            DataRow row = null;
            try
            {
                DataSet ds = dal.GetLoanContacts(FileID, Borrower);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    row = ds.Tables[0].Rows[0];
                }
            }
            catch
            {
            }
            return row;
        }

        public DataSet GetReassignContract(string ServiceTypes)
        {
            return dal.GetReassignContract(ServiceTypes);
        }

        public void UpdateContact(LPWeb.Model.Contacts model)
        {
            dal.UpdateContact(model);
        }
        public int AddClient(LPWeb.Model.Contacts model)
        {
            return dal.AddClient(model);
        }
        /// <summary>
        /// create contact without check duplicated
        /// neo 2012-10-24
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddClientNoCheck(LPWeb.Model.Contacts model)
        {
            return this.AddClientNoCheck(model);
        }
         /// <summary>
        /// line co-borrower to loan
        /// neo 2012-10-24
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="iCoBorrowerID"></param>
        public void LinkCoBorrowerToLoan(int iFileId, int iCoBorrowerID)
        {
            this.dal.LinkCoBorrowerToLoan(iFileId, iCoBorrowerID);
        }
        public void ADDContact(LPWeb.Model.Contacts model)
        {
            dal.ADDContact(model);
        }

        /// <summary>
        /// Gets the prospects by file ids.
        /// </summary>
        /// <param name="fileIds">The file ids.</param>
        /// <returns></returns>
        public DataSet GetProspectsByFileIds(string fileIds)
        {
            return dal.GetProspectsByFileIds(fileIds);
        }

        /// <summary>
        /// Merges the prospects.
        /// </summary>
        /// <param name="froms">The froms.</param>
        /// <param name="to">To.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public bool MergeProspects(List<int> froms, int to, int userId)
        {
            return dal.MergeProspects(froms, to, userId);
        }

        #region neo

        /// <summary>
        /// get contact info
        /// neo 2011-03-13
        /// </summary>
        /// <param name="iContactID"></param>
        /// <returns></returns>
        public DataTable GetContactInfo(int iContactID)
        {
            return dal.GetContactInfoBase(iContactID);
        }

        /// <summary>
        /// update Contacts.UpdatePoint= 0 or 1
        /// neo 2011-03-13
        /// </summary>
        /// <param name="iContactID"></param>
        /// <param name="bUpdatePoint"></param>
        public void UpdatePoint(int iContactID, bool bUpdatePoint)
        {
            dal.UpdatePointBase(iContactID, bUpdatePoint);
        }

        /// <summary>
        /// get related contact count
        /// neo 2011-03-15
        /// </summary>
        /// <param name="ContactId"></param>
        /// <returns></returns>
        public int GetRelatedContactCount(int ContactId)
        {
            return dal.GetRelatedContactCountBase(ContactId);
        }

        /// <summary>
        /// get related contacts
        /// neo 2011-03-15
        /// </summary>
        /// <param name="ContactId"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <returns></returns>
        public DataTable GetRelatedContacts(int ContactId, int iStartIndex, int iEndIndex)
        {
            return dal.GetRelatedContacts(ContactId, iStartIndex, iEndIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iContactId"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sLastName"></param>
        /// <param name="sEmail"></param>
        /// <param name="sCellPhone"></param>
        /// <param name="sHomePhone"></param>
        /// <param name="sWorkPhone"></param>
        /// <param name="sDOB"></param>
        /// <param name="sLeadSource"></param>
        /// <param name="sReferralID"></param>
        public void UpdateContactProspectInfo(int iContactId, string sFirstName, string sLastName, string sEmail, string sCellPhone, string sHomePhone, string sWorkPhone, string sDOB, string sLeadSource, string sReferralID)
        {
            this.dal.UpdateContactProspectInfo(iContactId, sFirstName, sLastName, sEmail, sCellPhone, sHomePhone, sWorkPhone, sDOB, sLeadSource, sReferralID);
        }

        #endregion

        /// <summary>
        /// get related contacts 
        /// </summary>
        /// <param name="ContactId"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <returns></returns>
        public DataTable GetRelatedContacts(int ContactId)
        {
            return dal.GetRelatedContacts(ContactId);
        }

        public DataSet GetRelationship(int ContactId1, int ContactId2)
        {
            return dal.GetRelationship(ContactId1, ContactId2);
        }

        public DataTable GetContactInfoByBranchID(string sContactBranchID)
        {
            return dal.GetContactInfoByBranchID(sContactBranchID);
        }

        public DataTable GetEnableContactInfo()
        {
            return dal.GetEnableContactInfo();
        }

        public int FormatCellPhone(string sContactBranchID)
        {
            return dal.FormatCellPhone(sContactBranchID);
        }        

        public DataTable GetEnableCompanyContactInfo(int companyid, int branchid)
        {
            return dal.GetEnableCompanyContactInfo(companyid, branchid);
        }

        public DataSet GetContactsByFileIds(string sContacts)
        {
            return dal.GetContactsByIds(sContacts);
        }

        public bool MergeContacts(List<int> iFroms, int iTo, int iUserId)
        {
            return dal.MergeContacts(iFroms, iTo, iUserId);
        }

        public void PartnerContactsDelete(int ContactId, int UserType, int UserID)
        { 
            dal.PartnerContactsDelete(ContactId, UserType, UserID);
        }

        /// <summary>
        ///  
        /// </summary>
        public DataSet SearchContacts(string strWhere)
        {
            return dal.SearchContacts(strWhere );
        }

          /// <summary>
        /// If Contacts Exsit Email Info
        /// </summary>
        /// <param name="sContactIDs"></param>
        /// <returns></returns>
        public string GetContactsEmailInfo(string sContactIDs)
        {
            return dal.GetContactsEmailInfo(sContactIDs);
        }

        public string vCardToString(int ContactId, bool Client)
        {
            return dal.vCardToString(ContactId, Client);
        }
    }
}

