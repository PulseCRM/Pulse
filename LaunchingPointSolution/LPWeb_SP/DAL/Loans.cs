using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据暶问类Loans。
    /// </summary>
    public class Loans : LoansBase
    {
        public Loans()
        { }
        public bool IsActiveLoan(int FileiD)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [dbo].[lpfn_IsActiveLoan](@FileId)");

            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = FileiD;
            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(obj);
            }
        }

        public DataSet Lead_FirstPage_GetProspectListNew(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = string.Format(
                            "(select pi.*,dbo.lpfn_GetBorrowerContactId(pi.FileId) AS ContactId from lpvw_PipelineInfo pi inner join (select distinct FileId from lpvw_PipelineInfoGroupForPropectNew where {0} ) tt on pi.FileId=tt.FileId) as pinfo", strWhere);

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 4000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "";
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet Lead_FirstPage_GetProspectListNew_Fast100(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = string.Format(
                            "(select top {0} pi.FileId, pi.Borrower, pi.Stage, pi.ImportErrorIcon, pi.AlertIcon, pi.RuleAlertIcon, pi.EstClose, pi.[Loan Officer] , pi.Amount,  pi.Lien,  pi.Rate, pi.Branch, pi.Progress, pi.Filename, pi.BranchID, pi.LeadSource, pi.RefCode, pi.Program, pi.ProspectLoanStatus, pi.Ranking, pi.LastCompletedStage,pi.LastStageComplDate, dbo.lpfn_GetBorrowerContactId(pi.FileId) AS ContactId from lpvw_PipelineInfo pi inner join (select distinct FileId from lpvw_PipelineInfoGroupForPropectNew where {1} ) tt on pi.FileId=tt.FileId) as pinfo", PageSize, strWhere);

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 4000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "";
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet Lead_Count(int PageSize, int PageIndex, string strWhere, out int leadcount, string orderName, int orderType)
        {
            string tempTable = string.Format(
                            "(select pi.FileId, pi.Borrower, dbo.lpfn_GetBorrowerContactId(pi.FileId) AS ContactId from lpvw_PipelineInfo pi inner join (select distinct FileId from lpvw_PipelineInfoGroupForPropectNew where {0} ) tt on pi.FileId=tt.FileId) as pinfo", strWhere);

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 4000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "";
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            leadcount = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet GetProspectListNew(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = string.Format(
                            "(select pi.*,dbo.lpfn_GetBorrowerContactId(pi.FileId) AS ContactId from lpvw_PipelineInfo pi inner join (select distinct FileId from lpvw_PipelineInfoGroupForPropectNew where {0} ) tt on pi.FileId=tt.FileId) as pinfo", strWhere);

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 4000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "";
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet GetProspectListNew_Fast(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = string.Format(
                            "(select pi.FileId, pi.Borrower, pi.RefCode as Stage, pi.RefCode as ImportErrorIcon,  pi.RefCode as AlertIcon,  pi.RefCode as RuleAlertIcon, pi.EstClose,  pi.Created, pi.[Loan Officer] , pi.Amount,  pi.Lien,  pi.Rate, pi.Branch, pi.RefCode as Progress, pi.Filename, pi.RefCode as BranchID, pi.LeadSource, pi.RefCode, pi.Program, pi.ProspectLoanStatus, pi.Ranking, pi.RefCode as LastCompletedStage, pi.RefCode as LastStageComplDate, dbo.lpfn_GetBorrowerContactId(pi.FileId) AS ContactId, pi.Partner, pi.Referral from lpvw_PipelineInfoGroupForPropectNew pi where {0} ) as pinfo", strWhere);

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 4000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            if (orderName == "Loan Officer")
            {
                orderName = "[Loan Officer]";
            }
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "";
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public void GetLeadData(ref DataSet loansList)
        {
            int fileid = 0;
            DataTable dt = null;

            string sSql = string.Format(
                           "select Stage, ImportErrorIcon, AlertIcon, RuleAlertIcon, Progress, BranchID, LastCompletedStage, LastStageComplDate, Partner, Referral from lpvw_PipelineInfo where FileId={0}", fileid);

            foreach (DataRow dr in loansList.Tables[0].Rows)
            {
                dt = null;
                fileid = (int)dr["FileId"];
                sSql = string.Format("select Stage, ImportErrorIcon, AlertIcon, RuleAlertIcon, Progress, BranchID, LastCompletedStage, LastStageComplDate, Partner, Referral from lpvw_PipelineInfo where FileId={0}", fileid);
                dt = DbHelperSQL.ExecuteDataTable(sSql);
                if (dt.Rows.Count > 0)
                {
                    dr["Stage"] = dt.Rows[0]["Stage"].ToString();
                    dr["ImportErrorIcon"] = dt.Rows[0]["ImportErrorIcon"].ToString();
                    dr["AlertIcon"] = dt.Rows[0]["AlertIcon"].ToString();
                    dr["RuleAlertIcon"] = dt.Rows[0]["RuleAlertIcon"].ToString();
                    dr["Progress"] = dt.Rows[0]["Progress"].ToString();
                    dr["BranchID"] = dt.Rows[0]["BranchID"].ToString();
                    dr["LastCompletedStage"] = dt.Rows[0]["LastCompletedStage"].ToString();
                    dr["LastStageComplDate"] = dt.Rows[0]["LastStageComplDate"].ToString();
                }
            }

            return;
        }

        public DataSet GetProspectList(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable =
                string.Format(
                    "(select pi.*,dbo.lpfn_GetBorrowerContactId(pi.FileId) AS ContactId from lpvw_PipelineInfo pi inner join (select distinct FileId from lpvw_PipelineInfoGroupForPropect where {0} ) tt on pi.FileId=tt.FileId) as pinfo", strWhere);
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 4000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "";
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet GetList(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable =
                string.Format(
                    "(select pi.*,dbo.lpfn_GetBorrowerContactId(pi.FileId) AS ContactId from lpvw_PipelineInfo pi inner join (select distinct FileId from lpvw_PipelineInfoGroup where {0} ) tt on pi.FileId=tt.FileId) as pinfo", strWhere);
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 4000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "";
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }
        /// <summary>
        /// Deletes the specified file ID.
        /// </summary>
        /// <param name="fileID">The file ID.</param>
        public void Delete(int fileID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@fileID", SqlDbType.Int)
					};
            parameters[0].Value = fileID;
            int rowsAffected;
            DbHelperSQL.RunProcedure("lpsp_RemoveLoan", parameters, out rowsAffected);
        }

        /// <summary>
        /// get sum of loan amount for Processing
        /// neo 2010-11-22
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="sWhere"></param>
        /// <param name="sStatus"></param>
        /// <param name="sCurrentStage)"></param>
        /// <returns></returns>
        public decimal GetLoanAmountSumBase_Processing(int iUserID, string sWhere, string sStatus, string sCurrentStage)
        {
            string sWhere2 = string.Empty;

            if (sStatus != string.Empty)
            {
                sWhere2 += " and (b.Status='" + sStatus + "')";
            }

            if (sCurrentStage != string.Empty)
            {
                sWhere2 += " and dbo.lpfn_GetCurrentStage(b.FileId)='" + sCurrentStage + "' ";
                //sWhere2 += " and (b.CurrentStage='" + sCurrentStage + "')";
            }

            string sSql = "select ISNULL(SUM(b.LoanAmount), 0.00) from lpfn_GetUserLoans(" + iUserID + ") as a inner join Loans as b on a.LoanID = b.FileId "
                        + "where (1=1)" + sWhere + sWhere2;

            decimal dSum = Convert.ToDecimal(DbHelperSQL.ExecuteScalar(sSql));
            return dSum;
        }

        /// <summary>
        /// get sum of loan amount for Prospect
        /// neo 2010-11-22
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="sWhere"></param>
        /// <param name="sStatus"></param>
        /// <param name="sCurrentStage)"></param>
        /// <returns></returns>
        public decimal GetLoanAmountSumBase_Prospect(int iUserID, string sWhere, string sStatus, string sCurrentStage)
        {
            string sWhere2 = string.Empty;

            if (sStatus != string.Empty)
            {
                sWhere2 += " and (b.Status='" + sStatus + "')";
            }

            if (sCurrentStage != string.Empty)
            {
                sWhere2 += " and dbo.lpfn_GetCurrentStage(b.FileId)='" + sCurrentStage + "' ";
                //sWhere2 += " and (b.CurrentStage='" + sCurrentStage + "')";
            }

            sWhere2 += "and (b.ProspectLoanStatus='Active')";

            string sSql = "select ISNULL(SUM(b.LoanAmount), 0.00) from lpfn_GetUserLoans(" + iUserID + ") as a inner join Loans as b on a.LoanID = b.FileId "
                        + "where (1=1)" + sWhere + sWhere2;

            decimal dSum = Convert.ToDecimal(DbHelperSQL.ExecuteScalar(sSql));
            return dSum;
        }

        /// <summary>
        /// get sum of loan amount for Achived
        /// neo 2010-11-22
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="sWhere"></param>
        /// <param name="sStatus"></param>
        /// <param name="sCurrentStage)"></param>
        /// <returns></returns>
        public decimal GetLoanAmountSumBase_Achived(int iUserID, string sWhere, string sStatus)
        {
            string sWhere2 = string.Empty;

            if (sStatus != string.Empty)
            {
                sWhere2 += " and (b.Status='" + sStatus + "')";
            }

            string sSql = "select ISNULL(SUM(b.LoanAmount), 0.00) from lpfn_GetUserLoans(" + iUserID + ") as a inner join Loans as b on a.LoanID = b.FileId "
                        + "where (1=1)" + sWhere + sWhere2;

            decimal dSum = Convert.ToDecimal(DbHelperSQL.ExecuteScalar(sSql));
            return dSum;
        }

        /// <summary>
        /// get data for organization production by regional
        /// neo 2010-10-25
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetOrganProductionData_RegionalBase(int iUserID, string sWhere)
        {
            string sSql = "select a.RegionID, b.Name, SUM(c.Amount) as Amount from lpfn_GetUserLoans(" + iUserID + ") as a "
                        + "inner join Regions as b on a.RegionID = b.RegionId "
                        + "inner join V_ProcessingPipelineInfo as c on a.LoanID = c.FileId "
                        + "where (1=1) " + sWhere + " "
                        + "group by a.RegionID, b.Name";

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get data for organization production by Division
        /// neo 2010-10-25
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetOrganProductionData_DivisionBase(int iUserID, string sWhere)
        {
            string sSql = "select a.DivisionID, b.Name, SUM(c.Amount) as Amount from lpfn_GetUserLoans(" + iUserID + ") as a "
                        + "inner join Divisions as b on a.DivisionID = b.DivisionID "
                        + "inner join V_ProcessingPipelineInfo as c on a.LoanID = c.FileId "
                        + "where (1=1) " + sWhere + " "
                        + "group by a.DivisionID, b.Name";

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get data for organization production by Branch
        /// neo 2010-10-25
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetOrganProductionData_BranchBase(int iUserID, string sWhere)
        {
            string sSql = "select a.BranchID, b.Name, SUM(c.Amount) as Amount from lpfn_GetUserLoans(" + iUserID + ") as a "
                        + "inner join Branches as b on a.BranchID = b.BranchID "
                        + "inner join V_ProcessingPipelineInfo as c on a.LoanID = c.FileId "
                        + "where (1=1) " + sWhere + " "
                        + "group by a.BranchID, b.Name";


            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// Gets the lender.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public string GetLender(int fileId)
        {
            return GetMappingValueByParameter("lpfn_GetLender", fileId);
        }

        /// <summary>
        /// get loan State by file id
        /// </summary>
        /// <param name="fileId">file id</param>
        /// <returns></returns>
        public string GetLoanStage(int fileId)
        {
            return GetMappingValueByParameter("lpfn_GetLoanStage", fileId);
        }

        private string GetMappingValueByParameter(string fnName, int fileId)
        {
            string sql = "SELECT dbo.{0}({1}) as val";

            sql = string.Format(sql, fnName, fileId);

            var dsData = DbHelperSQL.ExecuteDataTable(sql);

            if (dsData != null && dsData.Rows.Count > 0)
                return dsData.Rows[0][0].ToString();

            return string.Empty;
        }

        /// <summary>
        /// Gets the task alert detail.
        /// </summary>
        /// <param name="fileID">The file ID.</param>
        /// <returns></returns>
        public DataTable GetTaskAlertDetail(int fileID)
        {
            string sSql =
                string.Format("select top 1 * from lpvw_GetTastAlertDetail where (FileID={0} AND DueDate IS NOT NULL AND (AlertIcon like '%Red%' OR AlertIcon like '%Yellow%')) order by DueDate asc, LoanStageId ",
                              fileID);
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetTaskAlertDetail_loantaskID(int fileID, int loantaskID)
        {
            string sSql =
                string.Format("select * from lpvw_GetTastAlertDetail where FileID={0} and LoanTaskId={1} ",
                              fileID, loantaskID);
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataSet GetLoanDetailByLinkLoans(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable =
                string.Format(
                "(SELECT l.*,dbo.lpfn_GetLoanStage(l.FileId) as Stage,pf.Name AS fileName FROM Loans l LEFT OUTER JOIN PointFiles pf ON l.FileId=pf.FileId WHERE {0}) AS pinfo", strWhere);
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 4000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "";
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }


        #region neo

        /// <summary>
        /// get loan info
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetLoanInfoBase(int iLoanID)
        {
            string sSql = "select * from Loans where FileId = " + iLoanID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get borrower info
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetBorrowerInfoBase(int iLoanID)
        {
            string sSql = "select * from [dbo].[lpvw_GetLoanContactInfowRoles] where FileId = " + iLoanID + " and RoleName='Borrower'";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
        /// <summary>
        /// get coborrower info
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetCoBorrowerInfoBase(int iLoanID)
        {
            string sSql = "select * from [dbo].[lpvw_GetLoanContactInfowRoles] where FileId = " + iLoanID + " and RoleName='CoBorrower'";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
        /// <summary>
        /// get loan stages
        /// neo 2010-11-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetLoanStageBase(int iLoanID)
        {
            string sSql = "select * from LoanStages where FileId = " + iLoanID + " order by SequenceNumber";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get loan stages
        /// neo 2010-11-20
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetLoanStageBase(string sWhere)
        {
            string sSql = "select * from LoanStages where 1=1 " + sWhere + " order by SequenceNumber";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// Prospect Loan Details→Delete
        /// neo 2011-02-28
        /// </summary>
        /// <param name="iLoanID"></param>
        public void DeleteLoanBase(int iLoanID)
        {
            Delete(iLoanID);
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
        public DataSet GetLoanPipelineListBase(int iLoginUserID, int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            //string tempTable = string.Format("(select a.* from V_ProcessingPipelineInfo as a where 1=1 {0}) as t", strWhere);
            //string tempTable = string.Format("(select a.FileId, a.Status, a.BranchID, a.Borrower, a.Stage, a.EstClose, a.RateLockicon, " +
            //    "a.ImportErrorIcon, a.AlertIcon, a.RuleAlertIcon, a.[Loan Officer], a.Amount, a.Lien, a.Rate, a.Lender,dbo.lpfn_GetBorrowerContactId(a.FileId) AS ContactId, " +
            //    "a.[Lock Expiration Date], a.Branch, a.Progress, a.Processor, a.[Task Count], a.[Point Folder], a.Filename,LastCompletedStage,LastStageComplDate " +
            //    " ,a.[Closer],a.[Shipper],a.[Assistant],a.[DocPrep],a.[LoanProgram],a.[Purpose],a.[JrProcessor],'' as LastNote " +
            //    " from V_ProcessingPipelineInfo as a where 1=1 {0}) as t", strWhere);

            string tempTable = string.Format("(select a.FileId, a.Status, a.BranchID, a.Borrower, a.Stage, a.EstClose, a.RateLockicon, " +
                "a.ImportErrorIcon, a.AlertIcon, a.RuleAlertIcon, a.[Loan Officer], a.Amount, a.Lien, a.Rate, a.Lender,a.FileId AS ContactId, " +
                "a.[Lock Expiration Date], a.Branch, a.Progress, a.Processor, a.[Task Count], a.[Point Folder], a.Filename,LastCompletedStage,LastStageComplDate " +
                " ,a.[Closer],a.[Shipper],a.[Assistant],a.[DocPrep],a.[LoanProgram],a.[Purpose],a.[JrProcessor],'' as LastNote " +
                " from V_ProcessingPipelineInfo as a where 1=1 {0}) as t", strWhere);

            //string tempTable = string.Format("(select a.* from lpvw_PipelineInfo as a inner join dbo.lpfn_GetUserLoans2(" + iLoginUserID + ", ) as b on a.FileId=b.LoanID where 1=1 {0}) as t", strWhere);
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 4000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "";
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataTable GetLoanPipelineInfo(int iFileID)
        {
            string sSql = "select * from V_ProcessingPipelineInfo where FileId=" + iFileID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// Get Sum(Loans.Amount),  COUNT(Loans.FileId)
        /// WangXiao 2011-08-27
        /// </summary>
        /// <param name="iLoginUserID"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetTotalInfo(int iLoginUserID, string strWhere)
        {
            string sqlstring = string.Format("(SELECT SUM(Amount) AS TotalAmount, COUNT(FileId) AS TotalFileId FROM (select Amount, Fileid from V_ProcessingPipelineInfo as a where 1=1 {0}) as t)", strWhere);

            return DbHelperSQL.ExecuteDataTable(sqlstring);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="sPurpose"></param>
        /// <param name="sLoanType"></param>
        /// <param name="sProgram"></param>
        /// <param name="sLoanAmount"></param>
        /// <param name="sRate"></param>
        public void UpldateLoanInfo(int iFileId, string sPurpose, string sLoanType, string sProgram, string sLoanAmount, string sRate)
        {
            string sSql = "update Loans set Purpose=@Purpose, LoanType=@LoanType, Program=@Program, LoanAmount=@LoanAmount, Rate=@Rate where FileId = " + iFileId;
            SqlCommand SqlCmd = new SqlCommand(sSql);

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Purpose", SqlDbType.NVarChar, sPurpose);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LoanType", SqlDbType.NVarChar, sLoanType);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Program", SqlDbType.NVarChar, sProgram);
            if (sLoanAmount == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LoanAmount", SqlDbType.Money, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LoanAmount", SqlDbType.Money, Convert.ToDecimal(sLoanAmount));
            }
            if (sRate == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Rate", SqlDbType.SmallMoney, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Rate", SqlDbType.SmallMoney, Convert.ToDecimal(sRate));
            }


            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        public void UpldateLoanInfo(int iFileId, string sHousingStatus, string sRentAmount,
            string sPropertyStreetAddress1, string sPropertyStreetAddress2, string sPropertyCity, string sPropertyState,
            string sPropertyZip, string sPropertyValue, string sPurpose, string sLoanType, string sProgram, string sAmount,
            string sRate, string sPMI, string sPMITax, string sTerm, string sStartDate, bool b2nd, string s2ndAmount, string sRanking)
        {

            string sSql = "update Loans set HousingStatus=@HousingStatus, RentAmount=@RentAmount, PropertyAddr=@PropertyAddr, "
                        + "PropertyCity=@PropertyCity, PropertyState=@PropertyState, PropertyZip=@PropertyZip, "
                        + "SalesPrice=@SalesPrice, Purpose=@Purpose, LoanType=@LoanType, Program=@Program, LoanAmount=@LoanAmount, "
                        + "Rate=@Rate, MonthlyPMI=@MonthlyPMI, MonthlyPMITax=@MonthlyPMITax, Term=@Term, EstCloseDate=@EstCloseDate, TD_2=@TD_2, TD_2Amount=@TD_2Amount,Ranking=@Ranking "
                        + "where FileId=" + iFileId + ";"
                        + "update V_ProcessingPipelineInfo set EstClose=@EstCloseDate, Amount=@LoanAmount, Program=@Program, PropertyAddr=@PropertyAddr, "
                        + "PropertyCity=@PropertyCity, PropertyState=@PropertyState, PropertyZip=@PropertyZip, Rate=@Rate "
                        + "where FileId=" + iFileId;

            SqlCommand SqlCmd = new SqlCommand(sSql);

            #region Add Parameters

            DbHelperSQL.AddSqlParameter(SqlCmd, "@HousingStatus", SqlDbType.NVarChar, sHousingStatus);
            if (sRentAmount == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@RentAmount", SqlDbType.Decimal, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@RentAmount", SqlDbType.Decimal, Convert.ToDecimal(sRentAmount));
            }

            string sPropertyAddr = (sPropertyStreetAddress1 + " " + sPropertyStreetAddress2).Trim();
            DbHelperSQL.AddSqlParameter(SqlCmd, "@PropertyAddr", SqlDbType.NVarChar, sPropertyAddr);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@PropertyCity", SqlDbType.NVarChar, sPropertyCity);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@PropertyState", SqlDbType.NVarChar, sPropertyState);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@PropertyZip", SqlDbType.NVarChar, sPropertyZip);
            if (sPropertyValue == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@SalesPrice", SqlDbType.Money, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@SalesPrice", SqlDbType.Money, Convert.ToDecimal(sPropertyValue));
            }
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Purpose", SqlDbType.NVarChar, sPurpose);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@LoanType", SqlDbType.NVarChar, sLoanType);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Program", SqlDbType.NVarChar, sProgram);
            if (sAmount == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@LoanAmount", SqlDbType.Money, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@LoanAmount", SqlDbType.Money, Convert.ToDecimal(sAmount));
            }
            if (sRate == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@Rate", SqlDbType.SmallMoney, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@Rate", SqlDbType.SmallMoney, Convert.ToDecimal(sRate));
            }
            if (sPMI == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@MonthlyPMI", SqlDbType.Decimal, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@MonthlyPMI", SqlDbType.Decimal, Convert.ToDecimal(sPMI));
            }
            if (sPMITax == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@MonthlyPMITax", SqlDbType.Decimal, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@MonthlyPMITax", SqlDbType.Decimal, Convert.ToDecimal(sPMITax));
            }
            if (sTerm == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@Term", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@Term", SqlDbType.SmallInt, Convert.ToInt16(sTerm));
            }
            if (sStartDate == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@EstCloseDate", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@EstCloseDate", SqlDbType.DateTime, Convert.ToDateTime(sStartDate));
            }
            DbHelperSQL.AddSqlParameter(SqlCmd, "@TD_2", SqlDbType.Bit, b2nd);
            if (s2ndAmount == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@TD_2Amount", SqlDbType.Decimal, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@TD_2Amount", SqlDbType.Decimal, Convert.ToDecimal(s2ndAmount));
            }

            DbHelperSQL.AddSqlParameter(SqlCmd, "@Ranking", SqlDbType.NVarChar, sRanking);

            #endregion

            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="sCloseDate"></param>
        public void MakeProspectLoanClosed(int iFileId, string sCloseDate, int iUserID)
        {
            string sSql = "update Loans set Status='Closed', DateClose=@DateClose, Modifed=getdate(), ModifiedBy=" + iUserID + " where FileId = " + iFileId;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DateClose", SqlDbType.DateTime, Convert.ToDateTime(sCloseDate));

            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        /// <summary>
        /// get borrower contact id
        /// neo
        /// </summary>
        /// <param name="iFileId"></param>
        /// <returns></returns>
        public int? GetBorrowerID(int iFileId)
        {
            string sSql = "select dbo.lpfn_GetBorrowerContactId(" + iFileId + ")";
            object oID = DbHelperSQL.ExecuteScalar(sSql);
            if (oID == DBNull.Value)
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(oID);
            }
        }

        /// <summary>
        /// get co-borrower contact id
        /// neo
        /// </summary>
        /// <param name="iFileId"></param>
        /// <returns></returns>
        public int? GetCoBorrowerID(int iFileId)
        {
            string sSql = "select dbo.lpfn_GetCoBorrowerContactId(" + iFileId + ")";
            object oID = DbHelperSQL.ExecuteScalar(sSql);
            if (oID == DBNull.Value)
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(oID);
            }
        }

        /// <summary>
        /// get lender id and name by file id
        /// neo 2012-12-17
        /// </summary>
        /// <param name="iFileID"></param>
        /// <returns></returns>
        public DataTable GetLenderNameAndID(int iFileID)
        {
            string sSql = "select dbo.lpfn_GetLender(" + iFileID + ") as LenderName,dbo.lpfn_GetLenderID(" + iFileID + ") as LenderID";
            DataTable LenderData = DbHelperSQL.ExecuteDataTable(sSql);
            return LenderData;
        }

        /// <summary>
        /// update loan program
        /// neo 2012-12-21
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="sLoanProgram"></param>
        public void UpdateLoanProgram(int iFileId, string sLoanProgram)
        {
            string sSql = "update Loans set Program=@Program where FileId=" + iFileId;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Program", SqlDbType.NVarChar, sLoanProgram);

            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        /// <summary>
        /// update FirstTimeHomeBuyer
        /// neo 2012-12-21
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="bYes"></param>
        public void UpdateFirstTimeHomeBuyer(int iFileId, bool bYes)
        {
            string sSql = "update Loans set FirstTimeBuyer=@FirstTimeBuyer where FileId=" + iFileId;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@FirstTimeBuyer", SqlDbType.Bit, bYes);

            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="sMIOption"></param>
        public void UpdateMIOption(int iFileId, string sMIOption)
        {
            string sSql = "update Loans set MIOption=@MIOption where FileId=" + iFileId;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@MIOption", SqlDbType.NVarChar, sMIOption);

            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        #endregion

        #region ===== Prospect Loan =====

        /// <summary>
        /// mark loan as bad
        /// </summary>
        /// <param name="nFileId"></param>
        /// <param name="nUserId"></param>
        /// <returns></returns>
        public bool ProspectMarkAsBad(int nFileId, int nUserId)
        {
            SqlConnection sqlConn = null;
            SqlTransaction trans = null;
            try
            {
                sqlConn = DbHelperSQL.GetOpenConnection();
                trans = sqlConn.BeginTransaction();

                // 1. change Loan.ProspectLoanStatus='Bad'
                ChangeProspectLoanStatus(nFileId, "Bad", trans);

                // 2. delete LoanAlerts, EmailQue
                DeleteInfoWhenInactive(nFileId, trans);

                // 3. update Loan.Disposed as current datetime and add the LoanActivities record: "The loan status has been changed from xxx to yyy."
                SaveDisposedTimeAndLoanActivity(nFileId, nUserId, "Active", "Bad", trans);

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }
        }

        /// <summary>
        /// convert loan to processing
        /// </summary>
        /// <param name="nFileId"></param>
        /// <param name="nUserId"></param>
        /// <returns></returns>
        public bool ProspectConvert(int nFileId, int nUserId)
        {
            SqlConnection sqlConn = null;
            SqlTransaction trans = null;
            try
            {
                sqlConn = DbHelperSQL.GetOpenConnection();
                trans = sqlConn.BeginTransaction();

                // 1. change Loan.Status='Processing'
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = string.Format("UPDATE Loans SET [Status]='Processing' WHERE FileId='{0}'", nFileId);
                DbHelperSQL.ExecuteNonQuery(cmd, trans);

                // 2. change Loan.ProspectLoanStatus='Converted'
                ChangeProspectLoanStatus(nFileId, "Converted", trans);

                // 3. remove LoanStages, LoanTasks, DeletedLoanTasks, LoanAlerts, EmailQue
                SqlCommand cmdDeleteTasks = new SqlCommand();
                cmdDeleteTasks.CommandText = string.Format("DELETE LoanStages WHERE FileId='{0}';" +
                                                "DELETE LoanTasks WHERE FileId='{0}';" +
                                                "DELETE DeletedLoanTasks WHERE FileId='{0}';", nFileId);
                DbHelperSQL.ExecuteNonQuery(cmdDeleteTasks, trans);

                // 4. delete LoanAlerts, EmailQue
                DeleteInfoWhenInactive(nFileId, trans);

                // 5. update Loan.Disposed as current datetime and add the LoanActivities record: "The loan status has been changed from xxx to yyy."
                SaveDisposedTimeAndLoanActivity(nFileId, nUserId, "Active", "Converted", trans);

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }
        }

        /// <summary>
        /// cancel prospect loan
        /// </summary>
        /// <param name="nFileId"></param>
        /// <param name="nUserId"></param>
        /// <returns></returns>
        public bool ProspectCancel(int nFileId, int nUserId)
        {
            SqlConnection sqlConn = null;
            SqlTransaction trans = null;
            try
            {
                sqlConn = DbHelperSQL.GetOpenConnection();
                trans = sqlConn.BeginTransaction();

                // 1. change Loan.ProspectLoanStatus='Canceled'
                ChangeProspectLoanStatus(nFileId, "Canceled", trans);

                // 2. delete LoanAlerts, EmailQue
                DeleteInfoWhenInactive(nFileId, trans);

                // 3. update Loan.Disposed as current datetime and add the LoanActivities record: "The loan status has been changed from xxx to yyy."
                SaveDisposedTimeAndLoanActivity(nFileId, nUserId, "Active", "Canceled", trans);

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }
        }

        /// <summary>
        /// suspend prospect loan
        /// </summary>
        /// <param name="nFileId"></param>
        /// <param name="nUserId"></param>
        /// <returns></returns>
        public bool ProspectSuspend(int nFileId, int nUserId)
        {
            SqlConnection sqlConn = null;
            SqlTransaction trans = null;
            try
            {
                sqlConn = DbHelperSQL.GetOpenConnection();
                trans = sqlConn.BeginTransaction();

                // 1. change Loan.ProspectLoanStatus=''
                ChangeProspectLoanStatus(nFileId, "Suspended", trans);

                // 2. delete LoanAlerts, EmailQue
                DeleteInfoWhenInactive(nFileId, trans);

                // 3. update Loan.Disposed as current datetime and add the LoanActivities record: "The loan status has been changed from xxx to yyy."
                SaveDisposedTimeAndLoanActivity(nFileId, nUserId, "Active", "Suspended", trans);

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }
        }

        /// <summary>
        /// activate prospect loan
        /// </summary>
        /// <param name="nFileId"></param>
        /// <param name="nUserId"></param>
        /// <returns></returns>
        public bool ProspectActive(int nFileId, int nUserId)
        {
            SqlConnection sqlConn = null;
            SqlTransaction trans = null;
            try
            {
                sqlConn = DbHelperSQL.GetOpenConnection();
                trans = sqlConn.BeginTransaction();

                // get curren Loan.ProspectLoanStatus
                string strCurrStatus = "";
                LPWeb.Model.Loans loan = this.GetModel(nFileId);
                strCurrStatus = loan.ProspectLoanStatus;

                // 1. change Loan.ProspectLoanStatus='Active'
                ChangeProspectLoanStatus(nFileId, "Active", trans);

                // 2. update Loan.Disposed as current datetime and add the LoanActivities record: "The loan status has been changed from xxx to yyy."
                SaveDisposedTimeAndLoanActivity(nFileId, nUserId, strCurrStatus, "Active", trans);

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }
        }

        /// <summary>
        /// change Loan.ProspectLoanStatus
        /// </summary>
        /// <param name="nFileId">loan file to change</param>
        /// <param name="strStatus">change to status</param>
        /// <param name="sqlTrans"></param>
        private void ChangeProspectLoanStatus(int nFileId, string strStatus, SqlTransaction sqlTrans)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = string.Format("UPDATE Loans SET ProspectLoanStatus='{0}' WHERE FileId='{1}'", strStatus, nFileId);
            DbHelperSQL.ExecuteNonQuery(cmd, sqlTrans);
        }

        /// <summary>
        /// delete loan info when inactive 
        /// </summary>
        /// <param name="nFileId">loan file to delete</param>
        /// <param name="sqlTrans"></param>
        private void DeleteInfoWhenInactive(int nFileId, SqlTransaction sqlTrans)
        {
            // delete LoanAlerts, EmailQue
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = string.Format("DELETE LoanAlerts WHERE FileId='{0}'; DELETE EmailQue WHERE FileId='{0}';", nFileId);
            DbHelperSQL.ExecuteNonQuery(cmd, sqlTrans);
        }

        /// <summary>
        /// after dispose loan file actions
        /// </summary>
        /// <param name="nFileId">loan file which have been disposed</param>
        /// <param name="nUserId">userid</param>
        /// <param name="fromStatus">old ProspectLoanStatus</param>
        /// <param name="toStatus">new ProspectLoanStatus</param>
        /// <param name="sqlTrans"></param>
        private void SaveDisposedTimeAndLoanActivity(int nFileId, int nUserId, string fromStatus, string toStatus, SqlTransaction sqlTrans)
        {
            // update the Loans.Disposed field with the current date and time, and then add one LoanActivities record
            string strActivity = string.Format("The loan status has been changed from {0} to {1}.", fromStatus, toStatus);
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("UPDATE Loans SET Disposed=GETDATE() WHERE FileId='{0}';", nFileId);
            sbSql.AppendFormat("INSERT INTO LoanActivities(FileId, UserId, ActivityName, ActivityTime)VALUES({0}, {1}, '{2}', GETDATE());",
                nFileId, nUserId, strActivity);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sbSql.ToString();
            DbHelperSQL.ExecuteNonQuery(cmd, sqlTrans);
        }


        public DataSet GetProspectLoans(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
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
            parameters[0].Value = "lpvw_GetProspectLoans";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet GetProspectActiveLoans(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
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
            parameters[0].Value = "lpvw_GetProspectActiveLoans";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet GetProspectArchivedLoans(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
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
            parameters[0].Value = "lpvw_GetProspectArchivedLoans";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet GetProspectLoanDetail(int ContactID)
        {
            string strSQL = "SELECT [Name],[LienPosition],[LoanAmount],[LoanType],[Program],[PropertyAddr],[Purpose],[Rate],[FileId],[Contactid] FROM [lpvw_GetProspectLoans]";
            strSQL += "WHERE Contactid=@ContactID";
            SqlParameter[] parameters = {
					new SqlParameter("@ContactID", SqlDbType.Int,4)};
            parameters[0].Value = ContactID;

            return DbHelperSQL.Query(strSQL, parameters);
        }

        public DataSet GetProspectCopyFromLoans(int ContactID)
        {
            string strSQL = "SELECT [FileId], 'Active Loan -'+ [LienPosition] + '-'+[PropertyAddr] AS Loan FROM [dbo].[lpvw_GetProspectActiveLoans] WHERE Contactid=@ContactID UNION "
                          + "SELECT [FileId], [Status]+ ' Loan -'+ [LienPosition] + '-'+[PropertyAddr] AS Loan FROM [dbo].[lpvw_GetProspectArchivedLoans] WHERE Contactid=@ContactID UNION "
                          + "SELECT [FileId], 'Opportunity -'+ [LienPosition] + '-'+[PropertyAddr] AS Loan FROM [dbo].[lpvw_GetProspectLoans] WHERE Contactid=@ContactID ";
            SqlParameter[] parameters = {
					new SqlParameter("@ContactID", SqlDbType.Int,4)};
            parameters[0].Value = ContactID;
            return DbHelperSQL.Query(strSQL, parameters);
        }

        public DataSet GetProspectBrowwers(int ContactID)
        {
            string strSQL = "SELECT Contacts.ContactId,Contacts.LastName+ ','+Contacts.FirstName +' '+ Contacts.MiddleName"
                    + "FROM [Prospect] P1 INNER JOIN Prospect P2 	ON P1.Loanofficer = P2.Loanofficer"
                    + "INNER JOIN Contacts ON P2.Contactid = Contacts.ContactId  WHERE Contacts.Contactid=@ContactID";
            SqlParameter[] parameters = {
					new SqlParameter("@ContactID", SqlDbType.Int,4)};
            parameters[0].Value = ContactID;
            return DbHelperSQL.Query(strSQL, parameters);
        }

        public DataSet GetProspectPointFolders(int LOID)
        {
            string strSQL = " SELECT PointFolders.FolderId,PointFolders.Name FROM GroupUsers INNER JOIN Groups 	ON GroupUsers.GroupID = Groups.GroupId AND Groups.OrganizationType = 'Branch'"
                          + "INNER JOIN PointFolders	ON Groups.BranchID = PointFolders.BranchId AND PointFolders.LoanStatus=6 and PointFolders.Enabled=1 "
                          + "WHERE GroupUsers.UserID = @LOID";
            SqlParameter[] parameters = {
					new SqlParameter("@LOID", SqlDbType.Int,4)};
            parameters[0].Value = LOID;
            return DbHelperSQL.Query(strSQL, parameters);
        }

        public void LoanDetailSave(LPWeb.Model.LoanDetails model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@FolderId", SqlDbType.Int,4),
					new SqlParameter("@FileName", SqlDbType.NVarChar,255),
					new SqlParameter("@EstCloseDate", SqlDbType.DateTime),
					new SqlParameter("@LoanAmount", SqlDbType.Money,8),
					new SqlParameter("@Program", SqlDbType.NVarChar,255),
					new SqlParameter("@PropertyAddr", SqlDbType.NVarChar,50),
					new SqlParameter("@PropertyCity", SqlDbType.NVarChar,50),
					new SqlParameter("@PropertyState", SqlDbType.Char,2),
					new SqlParameter("@PropertyZip", SqlDbType.NVarChar,10),
					new SqlParameter("@Rate", SqlDbType.SmallMoney,4),
					new SqlParameter("@Status", SqlDbType.NVarChar,50),
					new SqlParameter("@ProspectLoanStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@Ranking", SqlDbType.NVarChar,20),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@CreatedBy", SqlDbType.Int,4),
					new SqlParameter("@Modifed", SqlDbType.DateTime),
					new SqlParameter("@ModifiedBy", SqlDbType.Int,4),
					new SqlParameter("@BoID", SqlDbType.Int,4),
					new SqlParameter("@CoBoID", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@Lien", SqlDbType.NVarChar, 50),
                    new SqlParameter("@Purpose", SqlDbType.NVarChar, 50),
                    new SqlParameter("@LoanOfficerId", SqlDbType.Int, 4),
                    new SqlParameter("@BranchId", SqlDbType.Int, 4),
                    new SqlParameter("@PropertyType", SqlDbType.NVarChar,255),
                    new SqlParameter("@HousingStatus", SqlDbType.NVarChar,255),
                    new SqlParameter("@IncludeEscrows", SqlDbType.Bit),
                    new SqlParameter("@InterestOnly", SqlDbType.Bit),
                    new SqlParameter("@CoBrwType", SqlDbType.NVarChar,255),
                    new SqlParameter("@RentAmount", SqlDbType.Decimal)

                                        };

            parameters[0].Direction = ParameterDirection.InputOutput; //gdc crm33
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.FolderId;
            parameters[2].Value = model.FileName;
            parameters[3].Value = model.EstCloseDate;
            parameters[4].Value = model.LoanAmount;
            parameters[5].Value = model.Program;
            parameters[6].Value = model.PropertyAddr;
            parameters[7].Value = model.PropertyCity;
            parameters[8].Value = model.PropertyState;
            parameters[9].Value = model.PropertyZip;
            parameters[10].Value = model.Rate;
            parameters[11].Value = model.Status;
            parameters[12].Value = model.ProspectLoanStatus;
            parameters[13].Value = model.Ranking;
            parameters[14].Value = model.Created;
            parameters[15].Value = model.CreatedBy;
            parameters[16].Value = model.Modifed;
            parameters[17].Value = model.ModifiedBy;
            parameters[18].Value = model.BoID;
            parameters[19].Value = model.CoBoID;
            parameters[20].Value = model.UserId;
            parameters[21].Value = model.Lien;
            parameters[22].Value = model.Purpose;
            parameters[23].Value = model.LoanOfficerId;
            parameters[24].Value = model.BranchId;
            parameters[25].Value = model.PropertyType;
            parameters[26].Value = model.HousingStatus;
            parameters[27].Value = model.IncludeEscrows;
            parameters[28].Value = model.InterestOnly;
            parameters[29].Value = model.CoborrowerType;
            parameters[30].Value = model.RentAmount;

            DbHelperSQL.RunProcedure("lpsp_LoanDetailSave", parameters, out rowsAffected);
            model.FileId = Convert.ToInt32(parameters[0].Value);
        }

        public int LoanDetailSaveFileId(LPWeb.Model.LoanDetails model)
        {
            int rowsAffected;
            int FileId = 0;

            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@FolderId", SqlDbType.Int,4),
					new SqlParameter("@FileName", SqlDbType.NVarChar,255),
					new SqlParameter("@EstCloseDate", SqlDbType.DateTime),
					new SqlParameter("@LoanAmount", SqlDbType.Money,8),
					new SqlParameter("@Program", SqlDbType.NVarChar,255),
					new SqlParameter("@PropertyAddr", SqlDbType.NVarChar,50),
					new SqlParameter("@PropertyCity", SqlDbType.NVarChar,50),
					new SqlParameter("@PropertyState", SqlDbType.Char,2),
					new SqlParameter("@PropertyZip", SqlDbType.NVarChar,10),
					new SqlParameter("@Rate", SqlDbType.SmallMoney,4),
					new SqlParameter("@Status", SqlDbType.NVarChar,50),
					new SqlParameter("@ProspectLoanStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@Ranking", SqlDbType.NVarChar,20),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@CreatedBy", SqlDbType.Int,4),
					new SqlParameter("@Modifed", SqlDbType.DateTime),
					new SqlParameter("@ModifiedBy", SqlDbType.Int,4),
					new SqlParameter("@BoID", SqlDbType.Int,4),
					new SqlParameter("@CoBoID", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@Lien", SqlDbType.NVarChar, 50),
                    new SqlParameter("@Purpose", SqlDbType.NVarChar, 50),
                    new SqlParameter("@LoanOfficerId", SqlDbType.Int, 4),
                    new SqlParameter("@BranchId", SqlDbType.Int, 4),
                    new SqlParameter("@PropertyType", SqlDbType.NVarChar,255),
                    new SqlParameter("@HousingStatus", SqlDbType.NVarChar,255),
                    new SqlParameter("@IncludeEscrows", SqlDbType.Bit),
                    new SqlParameter("@InterestOnly", SqlDbType.Bit),
                    new SqlParameter("@CoBrwType", SqlDbType.NVarChar,255),
                    new SqlParameter("@RentAmount", SqlDbType.Decimal),

                    new SqlParameter("@SalesPrice", SqlDbType.Money),
                    new SqlParameter("@Term", SqlDbType.SmallInt),
                    new SqlParameter("@TD_2", SqlDbType.Bit),
                    new SqlParameter("@TD_2Amount", SqlDbType.Decimal),
                    new SqlParameter("@Subordinate", SqlDbType.Bit),
                    new SqlParameter("@MonthlyPMI", SqlDbType.Decimal),
                    new SqlParameter("@MonthlyPMITax", SqlDbType.Decimal),
                    new SqlParameter("@LoanType", SqlDbType.NVarChar, 50)

                                        };
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.FolderId;
            parameters[2].Value = model.FileName;
            parameters[3].Value = model.EstCloseDate;
            parameters[4].Value = model.LoanAmount;
            parameters[5].Value = model.Program;
            parameters[6].Value = model.PropertyAddr;
            parameters[7].Value = model.PropertyCity;
            parameters[8].Value = model.PropertyState;
            parameters[9].Value = model.PropertyZip;
            parameters[10].Value = model.Rate;
            parameters[11].Value = model.Status;
            parameters[12].Value = model.ProspectLoanStatus;
            parameters[13].Value = model.Ranking;
            parameters[14].Value = model.Created;
            parameters[15].Value = model.CreatedBy;
            parameters[16].Value = model.Modifed;
            parameters[17].Value = model.ModifiedBy;
            parameters[18].Value = model.BoID;
            parameters[19].Value = model.CoBoID;
            parameters[20].Value = model.UserId;
            parameters[21].Value = model.Lien;
            parameters[22].Value = model.Purpose;
            parameters[23].Value = model.LoanOfficerId;
            parameters[24].Value = model.BranchId;
            parameters[25].Value = model.PropertyType;
            parameters[26].Value = model.HousingStatus;
            parameters[27].Value = model.IncludeEscrows;
            parameters[28].Value = model.InterestOnly;
            parameters[29].Value = model.CoborrowerType;
            parameters[30].Value = model.RentAmount;

            parameters[31].Value = model.SalesPrice;
            parameters[32].Value = model.Term;
            parameters[33].Value = model.TD_2;
            parameters[34].Value = model.TD_2Amount;
            parameters[35].Value = model.Subordinate;
            parameters[36].Value = model.MonthlyPMI;
            parameters[37].Value = model.MonthlyPMITax;
            parameters[38].Value = model.LoanType;

            FileId = DbHelperSQL.RunProcedure("lpsp_LoanDetailSaveFileId", parameters, out rowsAffected);
            return FileId;
        }

        public void UpdatePoint(int FileID, int UpdatePoint)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update PointFiles set ");
            strSql.Append("UpdatePoint=@UpdatePoint");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4), 
					new SqlParameter("@UpdatePoint", SqlDbType.Bit,1) };
            parameters[0].Value = FileID;
            parameters[1].Value = UpdatePoint;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        #endregion

        #region  ===== Contact Loan =====

        public DataSet GetPartnerContactLoans(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
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
            parameters[0].Value = "lpvw_GetPartnerContactLoans";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet GetPartnerContactActiveLoans(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
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
            parameters[0].Value = "lpvw_GetPartnerContactActiveLoans";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet GetPartnerContactArchivedLoans(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
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
            parameters[0].Value = "lpvw_GetPartnerContactArchivedLoans";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        #endregion

        /// <summary>
        /// 暤回Loan对应的ProspectLoanStatus
        /// Coder:Alex 2011-05-11
        /// </summary>
        /// <param name="iFileID"></param>
        /// <returns></returns>
        public string GetProspectStatusInfo(int iFileID)
        {
            string sRst = "";
            string sSql = "select isnull(Status,'') from lpvw_PipelineInfo where FileId=" + iFileID;
            object obj = DbHelperSQL.GetSingle(sSql);
            if (obj != null)
            {
                sRst = Convert.ToString(obj);
            }
            return sRst;
        }

        /// <summary>
        /// 暤回Loan对应的FileName
        /// Coder:Alex 2011-05-11
        /// </summary>
        /// <param name="iFileID"></param>
        /// <returns></returns>
        public string GetProspectFileNameInfo(int iFileID)
        {
            string sRst = "";
            string sSql = "select isnull(Filename,'') from lpvw_PipelineInfo where FileId=" + iFileID;
            object obj = DbHelperSQL.GetSingle(sSql);
            if (obj != null)
            {
                sRst = Convert.ToString(obj);
            }
            return sRst;
        }

        public int CheckProspectFileFolderId(int iFileID)
        {
            int FolderId = 0;
            string sSql = "select isnull(FolderId,0) from PointFiles where FileId=" + iFileID;
            object obj = DbHelperSQL.GetSingle(sSql);
            if (obj != null)
            {
                FolderId = (int)(obj);
            }

            if (FolderId > 0)
            {
                return FolderId;
            }

            int PointFolders_FolderId = 0;
            string sSql2 = "select top 1 isnull(FolderId,0) from PointFolders where (LoanStatus=6) and (Enabled=1)";
            object obj2 = DbHelperSQL.GetSingle(sSql2);
            if (obj2 != null)
            {
                PointFolders_FolderId = (int)(obj2);
            }

            if (PointFolders_FolderId == 0)
            {
                return 0;
            }

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update PointFiles set ");
            strSql.Append("FolderId=@FolderId");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4), 
					new SqlParameter("@FolderId", SqlDbType.Int,4) };
            parameters[0].Value = iFileID;
            parameters[1].Value = PointFolders_FolderId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

            return PointFolders_FolderId;
        }

        public string CheckProspectFileName(int iFileID)
        {
            string Name = "";
            string sSql = "select isnull(Name,'') from PointFiles where FileId=" + iFileID;
            object obj = DbHelperSQL.GetSingle(sSql);
            if (obj != null)
            {
                Name = Convert.ToString(obj);
            }

            if (Name != "")
            {
                return Name;
            }

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update PointFiles set ");
            strSql.Append("Name=@Name");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4), 
					new SqlParameter("@Name", SqlDbType.NVarChar,255) };
            parameters[0].Value = iFileID;
            parameters[1].Value = Name;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

            return Name;
        }

        /// <summary>
        /// 暤回所有的Loan对应的LoanOfficer 信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllLoanOfficerInfo()
        {
            string sSql = @"select distinct ISNULL(u.Lastname,'')+', '+ISNULL(u.Firstname,'') as LoanOfficer,u.UserId from LoanTeam lt inner join Users u
	                        on lt.UserId=u.UserId inner join Roles r
	                        on lt.RoleId=r.RoleId and r.Name='Loan Officer' order by LoanOfficer";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 暤回Processoer信息
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetProcessorList(int iUserID)
        {
            //string sSql = "select * from dbo.lpfn_GetAllProcessor(" + iUserID + ") where [Enabled]=1 order by LastName";

            string sSql = "select * from dbo.lpfn_GetAllProcessor(" + iUserID + ") "
                + " WHERE UserId IN(SELECT userID FROM loanteam WHERE RoleId =(SELECT RoleId FROM roles WHERE Name='Processor')) order by LastName";

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 所有的Loan对应的LoanOfficer 信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllLoanProcessor()
        {
            string sSql = @"select distinct u.Lastname,u.Firstname,u.UserId from LoanTeam lt inner join Users u
	                        on lt.UserId=u.UserId inner join Roles r
	                        on lt.RoleId=r.RoleId and r.Name='Processor' order by u.Lastname,u.Firstname";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }


        /// <summary>
        /// 得到Loan的Borrower name
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public string GetLoanBorrowerName(int iLoanID)
        {
            string sName = "";
            string sSql = "select dbo.lpfn_GetBorrower(" + iLoanID + ")";
            DataTable dt = DbHelperSQL.ExecuteDataTable(sSql);
            if (dt.Rows.Count > 0)
            {
                sName = dt.Rows[0][0].ToString();
            }
            return sName;
        }
    }
}



