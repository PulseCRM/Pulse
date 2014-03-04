using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类UserGoals。
    /// </summary>
    public class UserGoals : UserGoalsBase
    {
        public UserGoals()
        { }

        /// <summary>
        /// get user goals of 12 months
        /// neo 2010-09-15
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetUserGoalsBase(int iUserID)
        {
            string sSql = "select SUM(LowRange) as LowRange,SUM(MediumRange) as MediumRange,SUM(HighRange) as HighRange from UserGoals where UserId=" + iUserID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get user goals
        /// neo 2010-09-15
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="iMonth"></param>
        /// <returns></returns>
        public DataTable GetUserGoalsBase(int iUserID, int iMonth)
        {
            string sSql = "select * from UserGoals where UserId=" + iUserID + " and Month=" + iMonth;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get user goals for multi-months
        /// neo 2010-09-15
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="sMonths"></param>
        /// <returns></returns>
        public DataTable GetUserGoalsBase(int iUserID, string sMonths)
        {
            string sSql = "select SUM(LowRange) as LowRange,SUM(MediumRange) as MediumRange,SUM(HighRange) as HighRange from UserGoals where UserId=" + iUserID + " and Month in (" + sMonths + ")";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get loan amount of this month/quarter/year
        /// neo 2010-09-15
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public decimal GetUserLoanAmountBase_This(int iUserID, DateTime StartDate, DateTime EndDate)
        {
            string sSql = "select ISNULL(SUM(Loans.LoanAmount), 0.00) from Loans where FileID in ( "
                         + "    select distinct a.FileID from LoanTeam as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
                         + "    inner join Branches as d on c.BranchId = d.BranchId "
                         + "    inner join Loans as e on b.FileID = e.FileID "
                         + "    where a.UserId=" + iUserID + " "
                         + "    union "
                         + "    select distinct a.FileID from LoanTasks as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
                         + "    inner join Branches as d on c.BranchId = d.BranchId "
                         + "    inner join Loans as e on b.FileID = e.FileID "
                         + "    where Owner=" + iUserID + " "
                         + ")" + " and (DateClose between '" + StartDate.ToShortDateString() + "' and '" + EndDate.ToShortDateString() + "')";
            
            return Convert.ToDecimal(DbHelperSQL.ExecuteScalar(sSql));
        }

        /// <summary>
        /// get loan amount of next month/quarter/year
        /// neo 2010-09-15
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public decimal GetUserLoanAmountBase_Next(int iUserID, DateTime StartDate, DateTime EndDate)
        {
            string sSql = "select ISNULL(SUM(Loans.LoanAmount), 0.00) from Loans where FileID in ( "
                         + "    select distinct a.FileID from LoanTeam as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
                         + "    inner join Branches as d on c.BranchId = d.BranchId "
                         + "    inner join Loans as e on b.FileID = e.FileID "
                         + "    where a.UserId=" + iUserID + " "
                         + "    union "
                         + "    select distinct a.FileID from LoanTasks as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
                         + "    inner join Branches as d on c.BranchId = d.BranchId "
                         + "    inner join Loans as e on b.FileID = e.FileID "
                         + "    where Owner=" + iUserID + " "
                         + ")" + " and (EstCloseDate between '" + StartDate.ToShortDateString() + "' and '" + EndDate.ToShortDateString() + "')";
            
            return Convert.ToDecimal(DbHelperSQL.ExecuteScalar(sSql));
        }

        /// <summary>
        /// get user and his name
        /// </summary>
        /// <param name="strUserIDs"></param>
        /// <returns></returns>
        public DataSet GetUserForGoalsGrid(string strUserIDs)
        {
            return DbHelperSQL.Query(string.Format("SELECT UserId, FirstName + ' ' + LastName AS Name FROM Users WHERE UserId IN ({0})", strUserIDs));
        }

        /// <summary>
        /// get user goals, multi-user and multi months
        /// </summary>
        /// <param name="strUserID"></param>
        /// <param name="strMonth"></param>
        /// <returns></returns>
        public DataSet GetUserGoals(string strUserID, string strMonth)
        {
            string strSql = string.Format("SELECT * FROM UserGoals WHERE UserId IN ({0}) AND Month IN ({1})", strUserID, strMonth);
            return DbHelperSQL.Query(strSql);
        }

        /// <summary>
        /// save user goals info
        /// </summary>
        /// <param name="strUserIDs"></param>
        /// <param name="strMonths"></param>
        /// <param name="dsUserGoals"></param>
        public void SaveUserGoals(string strUserIDs, string strMonths, DataSet dsUserGoals)
        {
            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;
            try
            {
                DataSet dsUserGoalsCurr = this.GetUserGoals(strUserIDs, strMonths);
                if (null != dsUserGoals && dsUserGoals.Tables.Count > 0 && null != dsUserGoalsCurr && dsUserGoalsCurr.Tables.Count > 0)
                {
                    SqlConn = DbHelperSQL.GetOpenConnection();
                    SqlTrans = SqlConn.BeginTransaction();

                    SqlCommand cmdUserGoals = new SqlCommand();
                    StringBuilder sbUserGoals = new StringBuilder();
                    string strUID2Save = "";
                    string strMonth2Save = "";
                    foreach (DataRow dr in dsUserGoals.Tables[0].Rows)
                    {
                        strUID2Save = null == dr["userid"] ? "-1" : dr["userid"].ToString();
                        strMonth2Save = null == dr["Month"] ? "-1" : dr["Month"].ToString();
                        // if current user goals exists, then update else create new
                        DataRow[] drsTest = dsUserGoalsCurr.Tables[0].Select(string.Format("UserId='{0}' AND Month='{1}'", strUID2Save, strMonth2Save));
                        if (null == drsTest || drsTest.Length < 1)
                        {
                            sbUserGoals.Append(string.Format("INSERT INTO UserGoals(UserId,LowRange,MediumRange,HighRange,Month)VALUES({0},{1},{2},{3},{4});",
                                dr["UserId"], dr["LowRange"], dr["MediumRange"], dr["HighRange"], dr["Month"]));
                        }
                        else if (1 == drsTest.Length)
                        {
                            sbUserGoals.Append(string.Format("UPDATE UserGoals SET LowRange = {0}, MediumRange = {1}, HighRange = {2} WHERE UserId='{3}' AND Month='{4}';",
                                dr["LowRange"], dr["MediumRange"], dr["HighRange"], dr["UserId"], dr["Month"]));
                        }
                        else
                        {
                            // data error, one month of one user should have only one data record
                        }
                    }
                    cmdUserGoals.CommandText = sbUserGoals.ToString();

                    DbHelperSQL.ExecuteNonQuery(cmdUserGoals, SqlTrans);

                    SqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                SqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                if (SqlConn != null)
                {
                    SqlConn.Close();
                }
            }
        }
    }
}

