using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanWflTempl。
	/// </summary>
    public class LoanWflTempl : LoanWflTemplBase
	{
		public LoanWflTempl()
		{}

        public void Apply(LPWeb.Model.LoanWflTempl model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int),
					new SqlParameter("@WfTemplId", SqlDbType.Int),
					new SqlParameter("@UserId", SqlDbType.Int)
					};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.WflTemplId;
            parameters[2].Value = model.ApplyBy;

            DbHelperSQL.RunProcedure("lpsp_ApplyLoanWflTempl", parameters);
        }
        /// <summary>
        /// Get LoanTask Infos
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="count"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public DataSet GetWorkflowSetupList(int pageSize, int pageIndex, string strWhere,out int count, string orderName, int orderType)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = "lpvw_GetLoanTasksInfo";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = pageSize;
            parameters[4].Value = pageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        /// <summary>
        /// get LoanWflTempl info
        /// neo 2011-03-21
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetLoanWorkflowTemplateInfoBase(int iLoanID) 
        {
            string sSql = "select * from LoanWflTempl where FileId=" + iLoanID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
	}
}

