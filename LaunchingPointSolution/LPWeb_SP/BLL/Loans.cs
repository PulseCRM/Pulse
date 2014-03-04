using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// Loans 的摘要说明。
    /// </summary>
    public class Loans
    {
        private readonly LPWeb.DAL.Loans dal = new LPWeb.DAL.Loans();

        public Loans()
        { }
        #region  成员暯暔
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Loans model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Loans model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int FileId)
        {

            dal.Delete(FileId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Loans GetModel(int FileId)
        {

            return dal.GetModel(FileId);
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
        public List<LPWeb.Model.Loans> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public bool IsActiveLoan(int FileId)
        {
            return dal.IsActiveLoan(FileId);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Loans> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Loans> modelList = new List<LPWeb.Model.Loans>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Loans model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Loans();
                    if (dt.Rows[n]["FileId"].ToString() != "")
                    {
                        model.FileId = int.Parse(dt.Rows[n]["FileId"].ToString());
                    }
                    if (dt.Rows[n]["AppraisedValue"].ToString() != "")
                    {
                        model.AppraisedValue = decimal.Parse(dt.Rows[n]["AppraisedValue"].ToString());
                    }
                    model.CCScenario = dt.Rows[n]["CCScenario"].ToString();
                    if (dt.Rows[n]["CLTV"].ToString() != "")
                    {
                        model.CLTV = decimal.Parse(dt.Rows[n]["CLTV"].ToString());
                    }
                    model.County = dt.Rows[n]["County"].ToString();
                    if (dt.Rows[n]["DateOpen"].ToString() != "")
                    {
                        model.DateOpen = DateTime.Parse(dt.Rows[n]["DateOpen"].ToString());
                    }
                    if (dt.Rows[n]["DateSubmit"].ToString() != "")
                    {
                        model.DateSubmit = DateTime.Parse(dt.Rows[n]["DateSubmit"].ToString());
                    }
                    if (dt.Rows[n]["DateApprove"].ToString() != "")
                    {
                        model.DateApprove = DateTime.Parse(dt.Rows[n]["DateApprove"].ToString());
                    }
                    if (dt.Rows[n]["DateClearToClose"].ToString() != "")
                    {
                        model.DateClearToClose = DateTime.Parse(dt.Rows[n]["DateClearToClose"].ToString());
                    }
                    if (dt.Rows[n]["DateDocs"].ToString() != "")
                    {
                        model.DateDocs = DateTime.Parse(dt.Rows[n]["DateDocs"].ToString());
                    }
                    if (dt.Rows[n]["DateFund"].ToString() != "")
                    {
                        model.DateFund = DateTime.Parse(dt.Rows[n]["DateFund"].ToString());
                    }
                    if (dt.Rows[n]["DateRecord"].ToString() != "")
                    {
                        model.DateRecord = DateTime.Parse(dt.Rows[n]["DateRecord"].ToString());
                    }
                    if (dt.Rows[n]["DateClose"].ToString() != "")
                    {
                        model.DateClose = DateTime.Parse(dt.Rows[n]["DateClose"].ToString());
                    }
                    if (dt.Rows[n]["DateDenied"].ToString() != "")
                    {
                        model.DateDenied = DateTime.Parse(dt.Rows[n]["DateDenied"].ToString());
                    }
                    if (dt.Rows[n]["DateCanceled"].ToString() != "")
                    {
                        model.DateCanceled = DateTime.Parse(dt.Rows[n]["DateCanceled"].ToString());
                    }
                    if (dt.Rows[n]["DownPay"].ToString() != "")
                    {
                        model.DownPay = decimal.Parse(dt.Rows[n]["DownPay"].ToString());
                    }
                    if (dt.Rows[n]["EstCloseDate"].ToString() != "")
                    {
                        model.EstCloseDate = DateTime.Parse(dt.Rows[n]["EstCloseDate"].ToString());
                    }
                    if (dt.Rows[n]["Lender"].ToString() != "")
                    {
                        model.Lender = int.Parse(dt.Rows[n]["Lender"].ToString());
                    }
                    model.LienPosition = dt.Rows[n]["LienPosition"].ToString();
                    if (dt.Rows[n]["LoanAmount"].ToString() != "")
                    {
                        model.LoanAmount = decimal.Parse(dt.Rows[n]["LoanAmount"].ToString());
                    }
                    model.LoanNumber = dt.Rows[n]["LoanNumber"].ToString();
                    model.LoanType = dt.Rows[n]["LoanType"].ToString();
                    if (dt.Rows[n]["LTV"].ToString() != "")
                    {
                        model.LTV = decimal.Parse(dt.Rows[n]["LTV"].ToString());
                    }
                    if (dt.Rows[n]["MonthlyPayment"].ToString() != "")
                    {
                        model.MonthlyPayment = decimal.Parse(dt.Rows[n]["MonthlyPayment"].ToString());
                    }
                    model.LenderNotes = dt.Rows[n]["LenderNotes"].ToString();
                    model.Occupancy = dt.Rows[n]["Occupancy"].ToString();
                    model.Program = dt.Rows[n]["Program"].ToString();
                    model.PropertyAddr = dt.Rows[n]["PropertyAddr"].ToString();
                    model.PropertyCity = dt.Rows[n]["PropertyCity"].ToString();
                    model.PropertyState = dt.Rows[n]["PropertyState"].ToString();
                    model.PropertyZip = dt.Rows[n]["PropertyZip"].ToString();
                    model.Purpose = dt.Rows[n]["Purpose"].ToString();
                    if (dt.Rows[n]["Rate"].ToString() != "")
                    {
                        model.Rate = decimal.Parse(dt.Rows[n]["Rate"].ToString());
                    }
                    if (dt.Rows[n]["RateLockExpiration"].ToString() != "")
                    {
                        model.RateLockExpiration = DateTime.Parse(dt.Rows[n]["RateLockExpiration"].ToString());
                    }
                    if (dt.Rows[n]["SalesPrice"].ToString() != "")
                    {
                        model.SalesPrice = decimal.Parse(dt.Rows[n]["SalesPrice"].ToString());
                    }
                    if (dt.Rows[n]["Term"].ToString() != "")
                    {
                        model.Term = int.Parse(dt.Rows[n]["Term"].ToString());
                    }
                    if (dt.Rows[n]["Due"].ToString() != "")
                    {
                        model.Due = int.Parse(dt.Rows[n]["Due"].ToString());
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

        #endregion  成员暯暔
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetProspectListNew(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetProspectListNew(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        public DataSet GetProspectListNew_Fast(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetProspectListNew_Fast(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        public void GetLeadData(ref DataSet loansList)
        {
            dal.GetLeadData(ref loansList);
        }

        public DataSet Lead_FirstPage_GetProspectListNew(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.Lead_FirstPage_GetProspectListNew(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        public DataSet Lead_FirstPage_GetProspectListNew_Fast100(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.Lead_FirstPage_GetProspectListNew_Fast100(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        public DataSet Lead_Count(int PageSize, int PageIndex, string strWhere, out int leadCount, string orderName, int orderType)
        {
            return dal.Lead_Count(PageSize, PageIndex, strWhere, out leadCount, orderName, orderType);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetProspectList(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetProspectList(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetList(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        /// <summary>
        /// get sum of loan amount for Processing
        /// neo 2010-10-25
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="bSetRegion"></param>
        /// <param name="bAllLoans"></param>
        /// <param name="sWhere"></param>
        /// <param name="sStatus"></param>
        /// <param name="CurrentStage"></param>
        /// <returns></returns>
        public decimal GetLoanAmountSum_Processing(int iUserID, string sWhere, string sStatus, string CurrentStage)
        {
            return dal.GetLoanAmountSumBase_Processing(iUserID, sWhere, sStatus, CurrentStage);
        }

        /// <summary>
        /// get sum of loan amount for Prospect
        /// neo 2010-10-25
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="bSetRegion"></param>
        /// <param name="bAllLoans"></param>
        /// <param name="sWhere"></param>
        /// <param name="sStatus"></param>
        /// <param name="CurrentStage"></param>
        /// <returns></returns>
        public decimal GetLoanAmountSum_Prospect(int iUserID, string sWhere, string sStatus, string CurrentStage)
        {
            return dal.GetLoanAmountSumBase_Prospect(iUserID, sWhere, sStatus, CurrentStage);
        }

        /// <summary>
        /// get sum of loan amount for Achived
        /// neo 2010-10-25
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="bSetRegion"></param>
        /// <param name="bAllLoans"></param>
        /// <param name="sWhere"></param>
        /// <param name="sStatus"></param>
        /// <returns></returns>
        public decimal GetLoanAmountSum_Achived(int iUserID, string sWhere, string sStatus)
        {
            return dal.GetLoanAmountSumBase_Achived(iUserID, sWhere, sStatus);
        }

        /// <summary>
        /// get data for organization production by regional
        /// neo 2010-10-25
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetOrganProductionData_Regional(int iUserID, string sWhere)
        {
            return dal.GetOrganProductionData_RegionalBase(iUserID, sWhere);
        }

        /// <summary>
        /// get data for organization production by division
        /// neo 2010-10-25
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetOrganProductionData_Division(int iUserID, string sWhere)
        {
            return dal.GetOrganProductionData_DivisionBase(iUserID, sWhere);
        }

        /// <summary>
        /// get data for organization production by branch
        /// neo 2010-10-25
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetOrganProductionData_Branch(int iUserID, string sWhere)
        {
            return dal.GetOrganProductionData_BranchBase(iUserID, sWhere);
        }

        /// <summary>
        /// Gets the lender.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public string GetLender(int fileId)
        {
            return dal.GetLender(fileId);
        }

        /// <summary>
        /// get loan State by file id
        /// </summary>
        /// <param name="fileId">file id</param>
        /// <returns></returns>
        public string GetLoanStage(int fileId)
        {
            return dal.GetLoanStage(fileId);
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

        public DataTable GetTaskAlertDetail_loantaskID(int fileID, int loantaskID)
        {
            return dal.GetTaskAlertDetail_loantaskID(fileID, loantaskID);
        }

        public DataSet GetLoanDetailByLinkLoans(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {

            return dal.GetLoanDetailByLinkLoans(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        #region neo

        /// <summary>
        /// get loan info
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetLoanInfo(int iLoanID)
        {
            return dal.GetLoanInfoBase(iLoanID);
        }

        /// <summary>
        /// get borrower info
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetBorrowerInfo(int iLoanID)
        {
            return dal.GetBorrowerInfoBase(iLoanID);
        }
        /// <summary>
        /// get coborrower info
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetCoBorrowerInfo(int iLoanID)
        {
            return dal.GetCoBorrowerInfoBase(iLoanID);
        }
        /// <summary>
        /// get loan stages
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetLoanStages(int iLoanID)
        {
            return dal.GetLoanStageBase(iLoanID);
        }

        /// <summary>
        /// get loan stages
        /// neo 2010-11-20
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetLoanStage(string sWhere)
        {
            return dal.GetLoanStageBase(sWhere);
        }

        /// <summary>
        /// Prospect Loan Details→Delete
        /// neo 2011-02-28
        /// </summary>
        /// <param name="iLoanID"></param>
        public void DeleteLoan(int iLoanID)
        {
            dal.DeleteLoanBase(iLoanID);
        }

        /// <summary>
        /// get data for Loan Pipeline View
        /// neo 2011-05-25
        /// </summary>
        /// <param name="iLoginUserID"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="count"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public DataSet GetLoanPipelineList(int iLoginUserID, int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetLoanPipelineListBase(iLoginUserID, PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        public DataTable GetLoanPipelineInfo(int iFileID)
        {
            return this.dal.GetLoanPipelineInfo(iFileID);
        }

        /// <summary>
        ///  Get TotalInfo
        ///  WangXiao 2011-08-28
        /// </summary>
        /// <param name="iLoginUserID"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetTotalInfo(int iLoginUserID, string strWhere)
        {
            return dal.GetTotalInfo(iLoginUserID, strWhere);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="sPurpose"></param>
        /// <param name="sLoanType"></param>
        /// <param name="sProgram"></param>
        /// <param name="dLoanAmount"></param>
        /// <param name="dRate"></param>
        public void UpldateLoanInfo(int iFileId, string sPurpose, string sLoanType, string sProgram, string sLoanAmount, string sRate)
        {
            this.dal.UpldateLoanInfo(iFileId, sPurpose, sLoanType, sProgram, sLoanAmount, sRate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="sHousingStatus"></param>
        /// <param name="sRentAmount"></param>
        /// <param name="sPropertyStreetAddress1"></param>
        /// <param name="sPropertyStreetAddress2"></param>
        /// <param name="sPropertyCity"></param>
        /// <param name="sPropertyState"></param>
        /// <param name="sPropertyZip"></param>
        /// <param name="sPropertyValue"></param>
        /// <param name="sPurpose"></param>
        /// <param name="sLoanType"></param>
        /// <param name="sProgram"></param>
        /// <param name="sAmount"></param>
        /// <param name="sRate"></param>
        /// <param name="sPMI"></param>
        /// <param name="sPMITax"></param>
        /// <param name="sTerm"></param>
        /// <param name="sStartDate"></param>
        /// <param name="b2nd"></param>
        /// <param name="s2ndAmount"></param>
        public void UpldateLoanInfo(int iFileId, string sHousingStatus, string sRentAmount,
            string sPropertyStreetAddress1, string sPropertyStreetAddress2, string sPropertyCity, string sPropertyState,
            string sPropertyZip, string sPropertyValue, string sPurpose, string sLoanType, string sProgram, string sAmount,
            string sRate, string sPMI, string sPMITax, string sTerm, string sStartDate, bool b2nd, string s2ndAmount, string sRanking)
        {
            this.dal.UpldateLoanInfo(iFileId, sHousingStatus, sRentAmount,
            sPropertyStreetAddress1, sPropertyStreetAddress2, sPropertyCity, sPropertyState,
            sPropertyZip, sPropertyValue, sPurpose, sLoanType, sProgram, sAmount,
            sRate, sPMI, sPMITax, sTerm, sStartDate, b2nd, s2ndAmount, sRanking);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="sCloseDate"></param>
        public void MakeProspectLoanClosed(int iFileId, string sCloseDate, int iUserID)
        {
            this.dal.MakeProspectLoanClosed(iFileId, sCloseDate, iUserID);
        }

        /// <summary>
        /// get borrower contact id
        /// neo
        /// </summary>
        /// <param name="iFileId"></param>
        /// <returns></returns>
        public int? GetBorrowerID(int iFileId)
        {
            return this.dal.GetBorrowerID(iFileId);
        }

        /// <summary>
        /// get co-borrower contact id
        /// neo
        /// </summary>
        /// <param name="iFileId"></param>
        /// <returns></returns>
        public int? GetCoBorrowerID(int iFileId)
        {
            return this.dal.GetCoBorrowerID(iFileId);
        }

        /// <summary>
        /// get lender id and name by file id
        /// neo 2012-12-17
        /// </summary>
        /// <param name="iFileID"></param>
        /// <returns></returns>
        public DataTable GetLenderNameAndID(int iFileID)
        {
            return this.dal.GetLenderNameAndID(iFileID);
        }

        /// <summary>
        /// update loan program
        /// neo 2012-12-21
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="sLoanProgram"></param>
        public void UpdateLoanProgram(int iFileId, string sLoanProgram)
        {
            this.dal.UpdateLoanProgram(iFileId, sLoanProgram);
        }

        /// <summary>
        /// update FirstTimeHomeBuyer
        /// neo 2012-12-21
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="bYes"></param>
        public void UpdateFirstTimeHomeBuyer(int iFileId, bool bYes)
        {
            this.dal.UpdateFirstTimeHomeBuyer(iFileId, bYes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="sMIOption"></param>
        public void UpdateMIOption(int iFileId, string sMIOption)
        {
            this.dal.UpdateMIOption(iFileId, sMIOption);
        }

        #endregion

        #region ===== Prospect Loan =====

        /// <summary>
        /// mark loan as bad
        /// </summary>
        /// <param name="nFileId"></param>
        /// <returns></returns>
        public bool ProspectMarkAsBad(int nFileId, int nUserId)
        {
            return dal.ProspectMarkAsBad(nFileId, nUserId);
        }

        /// <summary>
        /// convert loan to processing
        /// </summary>
        /// <param name="nFileId"></param>
        /// <returns></returns>
        public bool ProspectConvert(int nFileId, int nUserId)
        {
            return dal.ProspectConvert(nFileId, nUserId);
        }

        /// <summary>
        /// cancel prospect loan
        /// </summary>
        /// <param name="nFileId"></param>
        /// <returns></returns>
        public bool ProspectCancel(int nFileId, int nUserId)
        {
            return dal.ProspectCancel(nFileId, nUserId);
        }

        /// <summary>
        /// suspend prospect loan
        /// </summary>
        /// <param name="nFileId"></param>
        /// <returns></returns>
        public bool ProspectSuspend(int nFileId, int nUserId)
        {
            return dal.ProspectSuspend(nFileId, nUserId);
        }

        /// <summary>
        /// activate prospect loan
        /// </summary>
        /// <param name="nFileId"></param>
        /// <returns></returns>
        public bool ProspectActive(int nFileId, int nUserId)
        {
            return dal.ProspectActive(nFileId, nUserId);
        }


        public DataSet GetProspectLoans(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetProspectLoans(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }
        public DataSet GetProspectActiveLoans(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetProspectActiveLoans(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }
        public DataSet GetProspectArchivedLoans(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetProspectArchivedLoans(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }


        public DataSet GetProspectLoanDetail(int ContactID)
        {
            return dal.GetProspectLoanDetail(ContactID);
        }

        public DataSet GetProspectCopyFromLoans(int ContactID)
        {
            return dal.GetProspectCopyFromLoans(ContactID);
        }

        public DataSet GetProspectBrowwers(int ContactID)
        {
            return dal.GetProspectBrowwers(ContactID);
        }

        public DataSet GetProspectPointFolders(int LOID)
        {
            return dal.GetProspectPointFolders(LOID);
        }

        public void LoanDetailSave(LPWeb.Model.LoanDetails model)
        {
            dal.LoanDetailSave(model);
        }

        public int LoanDetailSaveFileId(LPWeb.Model.LoanDetails model)
        {
            int FileId = 0;
            FileId = dal.LoanDetailSaveFileId(model);
            return FileId;
        }

        public void UpdatePoint(int FileID, int UpdatePoint)
        {
            dal.UpdatePoint(FileID, UpdatePoint);
        }
        #endregion


        public DataSet GetPartnerContactLoans(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetPartnerContactLoans(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }
        public DataSet GetPartnerContactActiveLoans(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetPartnerContactActiveLoans(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }
        public DataSet GetPartnerContactArchivedLoans(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetPartnerContactArchivedLoans(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        /// <summary>
        /// 暤回Loan对应的ProspectLoanStatus
        /// Coder:Alex 2011-05-11
        /// </summary>
        /// <param name="iFileID"></param>
        /// <returns></returns>
        public string GetProspectStatusInfo(int iFileID)
        {
            return dal.GetProspectStatusInfo(iFileID);
        }

        /// <summary>
        /// 暤回Loan对应的FileName
        /// Coder:Alex 2011-05-11
        /// </summary>
        /// <param name="iFileID"></param>
        /// <returns></returns>
        public string GetProspectFileNameInfo(int iFileID)
        {
            return dal.GetProspectFileNameInfo(iFileID);
        }

        public int CheckProspectFileFolderId(int iFileID)
        {
            return dal.CheckProspectFileFolderId(iFileID);
        }

        public string CheckProspectFileName(int iFileID)
        {
            return dal.CheckProspectFileName(iFileID);
        }

        /// <summary>
        /// 暤回所有的Loan对应的LoanOfficer 信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllLoanOfficerInfo()
        {
            return dal.GetAllLoanOfficerInfo();
        }
        /// <summary>
        /// 暤回Processor信息
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetProcessorList(int iUserID)
        {
            return dal.GetProcessorList(iUserID);
        }

        /// <summary>
        /// 得到Loan的Borrower name
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public string GetLoanBorrowerName(int iLoanID)
        {
            return dal.GetLoanBorrowerName(iLoanID);
        }
    }
}



