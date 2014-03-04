using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LPWeb.BLL
{
    public class LoanConditions
    {
        private readonly LPWeb.DAL.LoanConditions dal = new LPWeb.DAL.LoanConditions();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iConditionID"></param>
        /// <returns></returns>
        public DataTable GetLoanConditionsInfo(int iConditionID)
        {
            return this.dal.GetLoanConditionsInfo(iConditionID);
        }

        /// <summary>
        /// GetConditionsList
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="strWhere"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataSet GetList(int PageIndex, int PageSize, string strWhere, out int count)
        {
            return dal.GetList(PageIndex, PageSize, strWhere, out count);
        }

        /// <summary>
        /// UpdateExternalViewing
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ExternalViewing"></param>
        /// <returns></returns>
        public bool UpdateExternalViewing(int ID, bool ExternalViewing)
        {
            return dal.UpdateExternalViewing(ID, ExternalViewing);
        }

        /// <summary>
        /// Update the Loan Condition Status to received
        /// </summary>
        /// <param name="ID">Condition</param>
        /// <param name="sEditor">Received By</param>
        /// <returns></returns>
        public bool UpdateConditionStatusToReceived(int ID, string sEditor)
        {
            return dal.UpdateConditionStatusToReceived(ID, sEditor);
        }
    }
}
