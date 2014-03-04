using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// Prospect
    /// </summary>
    public class Prospect
    {
        private readonly LPWeb.DAL.Prospect dal = new LPWeb.DAL.Prospect();
        public Prospect()
        { }
        #region  Method
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Prospect model)
        {
           return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Prospect model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Contactid)
        {

            dal.Delete(Contactid);
        }
        ///// <summary>
        ///// 删除一条数据
        ///// </summary>
        //public bool DeleteList(string Contactidlist)
        //{
        //    return dal.DeleteList(Contactidlist);
        //}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Prospect GetModel(int Contactid)
        {

            return dal.GetModel(Contactid);
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
        public List<LPWeb.Model.Prospect> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Prospect> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Prospect> modelList = new List<LPWeb.Model.Prospect>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Prospect model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Prospect();
                    if (dt.Rows[n]["Contactid"].ToString() != "")
                    {
                        model.Contactid = int.Parse(dt.Rows[n]["Contactid"].ToString());
                    }
                    model.LeadSource = dt.Rows[n]["LeadSource"].ToString();
                    model.ReferenceCode = dt.Rows[n]["ReferenceCode"].ToString();
                    if (dt.Rows[n]["Referral"] != DBNull.Value)
                    {
                        model.Referral = Convert.ToInt32(dt.Rows[n]["Referral"]);
                    }
                    else
                    {
                        model.Referral = null;
                    }
                    if (dt.Rows[n]["Created"].ToString() != "")
                    {
                        model.Created = DateTime.Parse(dt.Rows[n]["Created"].ToString());
                    }
                    if (dt.Rows[n]["CreatedBy"].ToString() != "")
                    {
                        model.CreatedBy = int.Parse(dt.Rows[n]["CreatedBy"].ToString());
                    }
                    if (dt.Rows[n]["Modifed"].ToString() != "")
                    {
                        model.Modifed = DateTime.Parse(dt.Rows[n]["Modifed"].ToString());
                    }
                    if (dt.Rows[n]["ModifiedBy"].ToString() != "")
                    {
                        model.ModifiedBy = int.Parse(dt.Rows[n]["ModifiedBy"].ToString());
                    }
                    if (dt.Rows[n]["Loanofficer"].ToString() != "")
                    {
                        model.Loanofficer = int.Parse(dt.Rows[n]["Loanofficer"].ToString());
                    }
                    model.Status = dt.Rows[n]["Status"].ToString();
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
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  Method

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetList(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }
        /// <summary>
        /// 重新指定LoanOfficer
        /// </summary>
        /// <param name="ContactID"></param>
        /// <param name="UserID"></param>
        public void AssignProspect(int ContactID, int UserID, int iOldUserID)
        {
            dal.AssignProspect(ContactID, UserID, iOldUserID);
        }


        #region neo

        /// <summary>
        /// get prospect info
        /// neo 2011-03-13
        /// </summary>
        /// <param name="iContactID"></param>
        /// <returns></returns>
        public DataTable GetProspectInfo(int iContactID)
        {
            return dal.GetProspectInfoBase(iContactID);
        }
        /// <summary>
        /// Create Prospect and Contact records
        /// 
        /// </summary>
        /// <param name="Model.Contacts">contactInfo</param>
        /// <param name="Model.Prospect">prospectInfo</param>
        /// <returns>int ContactId</returns>
        public int CreateContactAndProspect(LPWeb.Model.Contacts contactInfo, LPWeb.Model.Prospect prospectInfo)
        {
            return dal.CreateContactAndProspect(contactInfo, prospectInfo);
        }
        /// <summary>
        /// create contact and prospect without checking duplicated
        /// neo 2012-10-24
        /// </summary>
        /// <param name="contactInfo"></param>
        /// <param name="prospectInfo"></param>
        /// <returns></returns>
        public int CreateContactAndProspectNoCheck(LPWeb.Model.Contacts contactInfo, LPWeb.Model.Prospect prospectInfo)
        {
            return dal.CreateContactAndProspectNoCheck(contactInfo, prospectInfo);
        }
        /// <summary>
        /// update prospect detail
        /// neo 2011-03-13
        /// </summary>
        /// <param name="iContactID"></param>
        /// <param name="sLeadSource"></param>
        /// <param name="sReferenceCode"></param>
        /// <param name="iModifiedBy"></param>
        /// <param name="iLoanOfficerID"></param>
        /// <param name="sStatus"></param>
        /// <param name="strCreditRanking"></param>
        /// <param name="strPreferredContact"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sMiddleName"></param>
        /// <param name="sLastName"></param>
        /// <param name="sTitle"></param>
        /// <param name="sGenerationCode"></param>
        /// <param name="sSSN"></param>
        /// <param name="sHomePhone"></param>
        /// <param name="sCellPhone"></param>
        /// <param name="sBusinessPhone"></param>
        /// <param name="sFax"></param>
        /// <param name="sEmail"></param>
        /// <param name="sDOB"></param>
        /// <param name="iExperian"></param>
        /// <param name="iTransUnion"></param>
        /// <param name="iEquifax"></param>
        /// <param name="sAddress"></param>
        /// <param name="sCity"></param>
        /// <param name="sState"></param>
        /// <param name="sZip"></param>
        public void UpdateProspectDetail(int iContactID, string sLeadSource, string sReferenceCode, int iModifiedBy, int iLoanOfficerID,
            string sStatus, string strCreditRanking, string strPreferredContact, string sFirstName, string sMiddleName, string sLastName, string sTitle, string sGenerationCode, string sSSN, string sHomePhone,
            string sCellPhone, string sBusinessPhone, string sFax, string sEmail, string sDOB, Int16 iExperian, Int16 iTransUnion, Int16 iEquifax,
            string sAddress, string sCity, string sState, string sZip, int iReferralID)
        {
            dal.UpdateProspectDetailBase(iContactID, sLeadSource, sReferenceCode, iModifiedBy, iLoanOfficerID, sStatus, strCreditRanking, strPreferredContact, sFirstName, sMiddleName, sLastName, sTitle, sGenerationCode, sSSN, sHomePhone, sCellPhone, sBusinessPhone, sFax, sEmail, sDOB, iExperian, iTransUnion, iEquifax, sAddress, sCity, sState, sZip, iReferralID);
        }

        /// <summary>
        /// insert prospect detail
        /// neo 2011-03-16
        /// </summary>
        /// <param name="sLeadSource"></param>
        /// <param name="sReferenceCode"></param>
        /// <param name="iCreatedBy"></param>
        /// <param name="iLoanOfficerID"></param>
        /// <param name="sStatus"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sMiddleName"></param>
        /// <param name="sLastName"></param>
        /// <param name="sTitle"></param>
        /// <param name="sGenerationCode"></param>
        /// <param name="sSSN"></param>
        /// <param name="sHomePhone"></param>
        /// <param name="sCellPhone"></param>
        /// <param name="sBusinessPhone"></param>
        /// <param name="sFax"></param>
        /// <param name="sEmail"></param>
        /// <param name="sDOB"></param>
        /// <param name="iExperian"></param>
        /// <param name="iTransUnion"></param>
        /// <param name="iEquifax"></param>
        /// <param name="sAddress"></param>
        /// <param name="sCity"></param>
        /// <param name="sState"></param>
        /// <param name="sZip"></param>
        public void InsertProspectDetail(string sLeadSource, string sReferenceCode, int iCreatedBy, int iLoanOfficerID, string sStatus,
            string sFirstName, string sMiddleName, string sLastName, string sTitle, string sGenerationCode, string sSSN, string sHomePhone,
            string sCellPhone, string sBusinessPhone, string sFax, string sEmail, string sDOB, Int16 iExperian, Int16 iTransUnion, Int16 iEquifax,
            string sAddress, string sCity, string sState, string sZip)
        {
            dal.InsertProspectDetailBase(sLeadSource, sReferenceCode, iCreatedBy, iLoanOfficerID, sStatus, sFirstName, sMiddleName, sLastName, sTitle, sGenerationCode, sSSN, sHomePhone, sCellPhone, sBusinessPhone, sFax, sEmail, sDOB, iExperian, iTransUnion, iEquifax, sAddress, sCity, sState, sZip);
        }


        /// <summary>
        /// Gets the task alert detail.
        /// </summary>
        /// <param name="fileID">The file ID.</param>
        /// <returns></returns>
        public DataTable GetTaskAlertDetail(int fileID)
        {
            return dal.GetTaskAlertDetail(fileID);
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
        /// <param name="sSSN"></param>
        /// <param name="sDependants"></param>
        /// <param name="sCreditRanking"></param>
        /// <param name="sFICO"></param>
        public void UpdateBorrower(int iContactId, string sFirstName, string sLastName, string sEmail, string sCellPhone, string sHomePhone,
           string sWorkPhone, string sDOB, string sSSN,
           string sDependants, string sCreditRanking, string sFICO) 
        {
            this.dal.UpdateBorrower(iContactId, sFirstName, sLastName, sEmail, sCellPhone, sHomePhone,
                sWorkPhone, sDOB, sSSN,
                sDependants, sCreditRanking, sFICO);
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
        /// <param name="sSSN"></param>
        /// <param name="sDependants"></param>
        /// <param name="sCreditRanking"></param>
        /// <param name="sFICO"></param>
        /// <param name="sMailingStreetAddress1"></param>
        /// <param name="sMailingStreetAddress2"></param>
        /// <param name="sMailingCity"></param>
        /// <param name="sMailingState"></param>
        /// <param name="sMailingZip"></param>
        /// <param name="sLeadSource"></param>
        /// <param name="sReferralID"></param>
        public void UpdateBorrower(int iContactId, string sFirstName, string sLastName, string sEmail, string sCellPhone, string sHomePhone,
            string sWorkPhone, string sDOB, string sSSN,
            string sDependants, string sCreditRanking, string sFICO,
            string sMailingStreetAddress1, string sMailingStreetAddress2, string sMailingCity, string sMailingState, string sMailingZip,
            string sLeadSource, string sReferralID) 
        {
            this.dal.UpdateBorrower(iContactId, sFirstName, sLastName, sEmail, sCellPhone, sHomePhone,
                sWorkPhone, sDOB, sSSN,
                sDependants, sCreditRanking, sFICO,
                sMailingStreetAddress1, sMailingStreetAddress2, sMailingCity, sMailingState, sMailingZip,
                sLeadSource, sReferralID);
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
        /// <param name="sSSN"></param>
        /// <param name="sFICO"></param>
        public void UpdateCoBorrower(int iContactId, string sFirstName, string sLastName, string sEmail, string sCellPhone, string sHomePhone,
            string sWorkPhone, string sDOB, string sSSN, string sFICO) 
        {
            this.dal.UpdateCoBorrower(iContactId, sFirstName, sLastName, sEmail, sCellPhone, sHomePhone,
                sWorkPhone, sDOB, sSSN, sFICO);
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
        /// <param name="sSSN"></param>
        /// <param name="sFICO"></param>
        /// <param name="sMailingStreetAddress1"></param>
        /// <param name="sMailingStreetAddress2"></param>
        /// <param name="sMailingCity"></param>
        /// <param name="sMailingState"></param>
        /// <param name="sMailingZip"></param>
        /// <param name="sLeadSource"></param>
        /// <param name="sReferralID"></param>
        public void UpdateCoBorrower(int iContactId, string sFirstName, string sLastName, string sEmail, string sCellPhone, string sHomePhone,
            string sWorkPhone, string sDOB, string sSSN, string sFICO,
            string sMailingStreetAddress1, string sMailingStreetAddress2, string sMailingCity, string sMailingState, string sMailingZip,
            string sLeadSource, string sReferralID)
        {
            this.dal.UpdateCoBorrower(iContactId, sFirstName, sLastName, sEmail, sCellPhone, sHomePhone,
                sWorkPhone, sDOB, sSSN, sFICO,
                sMailingStreetAddress1, sMailingStreetAddress2, sMailingCity, sMailingState, sMailingZip,
                sLeadSource, sReferralID);
        }

        #endregion

         /// <summary>
        /// 得到所有的Partner 数据源
        /// Alex 2011-04-07
        /// </summary>
        /// <returns></returns>
        public DataTable GetReferralCompanies()
        {
            return dal.GetReferralCompanies();
        }

         /// <summary>
        /// 得到所有的Referra; 数据源
        /// Alex 2011-04-07
        /// </summary>
        /// <returns></returns>
        public DataTable GetReferralContactInfo()
        {
            return dal.GetReferralContactInfo();
        }


        public DataSet GetProspectRefLoansInfo(int ReferralContactID, int iLoginUserID, int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetProspectRefLoansInfo(ReferralContactID, iLoginUserID, PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

    }
}

