using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;

namespace LPWeb.AnyChart
{
    public partial class UserGoals_GetData : BasePage
    {
        // date range
        string sDateRange = "ThisMonth";

        // 当前用户
        LoginUser CurrentUser;
        int iCurrentUserID = 0;

        LPWeb.BLL.UserGoals UserGoalsManager = new LPWeb.BLL.UserGoals();

        public string sChartTitle = string.Empty;
        public string sLowRangeEnd = string.Empty;
        public string sMediumRangeEnd = string.Empty;
        public string sHighRangeEnd = string.Empty;
        public string sScaleInterval = string.Empty;
        public string sRealAmount = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 获取当前用户信息
            this.CurrentUser = new LoginUser();
            this.iCurrentUserID = CurrentUser.iUserID;

            #region chart title

            string sChartTitle = string.Empty; 
            
            if(this.Request.QueryString["DateRange"] != null)
            {
                this.sDateRange = this.Request.QueryString["DateRange"].ToString();

                if (this.sDateRange != "ThisMonth" && this.sDateRange != "NextMonth"
                     && this.sDateRange != "ThisQuarter" && this.sDateRange != "NextQuarter"
                        && this.sDateRange != "ThisYear" && this.sDateRange != "NextYear") 
                {
                    this.sDateRange = "ThisMonth";
                }
            }

            if (this.sDateRange.Contains("Month") == true)
            {
                sChartTitle = "Monthly Production Goals";
            }
            else if (this.sDateRange.Contains("Quarter") == true)
            {
                sChartTitle = "Quarterly Production Goals";
            }
            else if (this.sDateRange.Contains("Year") == true)
            {
                sChartTitle = "Yearly Production Goals";
            }

            #endregion

            #region low/medium/high range

            int iLowRangeEnd = 0;
            int iMediumRangeEnd = 0;
            int iHighRangeEnd = 0;

            DataTable UserGoalsInfo;

            // get month
            if(this.sDateRange.Contains("Month") == true)
            {
                int iMonth = 0;
                if(this.sDateRange == "ThisMonth")
                {
                    iMonth = DateTime.Now.Month;
                }
                else // NextMonth
                {
                    iMonth = DateTime.Now.AddMonths(1).Month;
                }

                // get user goals info
                UserGoalsInfo = UserGoalsManager.GetUserGoals(this.iCurrentUserID, iMonth);
            }
            else if (this.sDateRange.Contains("Quarter") == true) 
            {
                int iThisMonth = DateTime.Now.Month;
                string sMonths = string.Empty;

                // 计算This Quarter
                int iThisQuarter = this.GetQuarter(iThisMonth);

                if (this.sDateRange == "ThisQuarter")
                {
                    #region ThisQuarter

                    if (iThisQuarter == 1)
                    {
                        sMonths = "1,2,3";
                    }
                    else if (iThisQuarter == 2)
                    {
                        sMonths = "4,5,6";
                    }
                    if (iThisQuarter == 3)
                    {
                        sMonths = "7,8,9";
                    }
                    if (iThisQuarter == 4)
                    {
                        sMonths = "10,11,12";
                    }

                    #endregion
                }
                else if (this.sDateRange == "NextQuarter")
                {
                    #region NextQuarter

                    int iNextQuarter = iThisQuarter + 1;
                    if (iNextQuarter > 4)
                    {
                        iNextQuarter = 1;
                    }

                    if (iNextQuarter == 1)
                    {
                        sMonths = "1,2,3";
                    }
                    else if (iNextQuarter == 2)
                    {
                        sMonths = "4,5,6";
                    }
                    if (iNextQuarter == 3)
                    {
                        sMonths = "7,8,9";
                    }
                    if (iNextQuarter == 4)
                    {
                        sMonths = "10,11,12";
                    }

                    #endregion
                }

                // get user goals info
                UserGoalsInfo = UserGoalsManager.GetUserGoals(this.iCurrentUserID, sMonths);
            }
            else
            {
                // get user goals info
                UserGoalsInfo = UserGoalsManager.GetUserGoals(this.iCurrentUserID);
            }

            // if no user goals setup
            if (UserGoalsInfo.Rows.Count == 0)
            {
                this.Response.End();
            }

            if (UserGoalsInfo.Rows[0]["LowRange"] == DBNull.Value
                || UserGoalsInfo.Rows[0]["MediumRange"] == DBNull.Value
                || UserGoalsInfo.Rows[0]["HighRange"] == DBNull.Value)
            {
                this.Response.End();
            }

            iLowRangeEnd = Convert.ToInt32(UserGoalsInfo.Rows[0]["LowRange"]);
            iMediumRangeEnd = Convert.ToInt32(UserGoalsInfo.Rows[0]["MediumRange"]);
            iHighRangeEnd = Convert.ToInt32(UserGoalsInfo.Rows[0]["HighRange"]);

            #endregion

            #region scale interval

            int iScaleInterval = 6;

            decimal dScaleRange = iHighRangeEnd / iScaleInterval;

            iScaleInterval = Convert.ToInt32(dScaleRange/1000);

            #endregion

            #region calculate real amount

            int iRealAmount = 0;
            
            string sWhere = string.Empty;
            string sSqlx1 = string.Empty;

            object oLoanAmount = null;
            DataTable UserLoanList = null;

            string sFiledName = "b.LastStageComplDate";          
            string sStageAlias = "Closed";            

            if(this.sDateRange.Contains("Month") == true)
            {
                if (this.sDateRange == "ThisMonth")
                {
                    #region ThisMonth

                    // 获取本月天数
                    int iDaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

                    // 本月开始日期和结束日期
                    DateTime StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, iDaysInMonth);
                 
                    //// 获取Loan Amount
                    //decimal dLoanAmount = this.UserGoalsManager.GetUserLoanAmount_This(this.iCurrentUserID, StartDate, EndDate);

                    //// 转换成int
                    //iRealAmount = Convert.ToInt32(dLoanAmount);

                    sWhere += SqlTextBuilder.BuildDateSearchCondition(sFiledName, StartDate, EndDate);

                    sSqlx1 = "select b.Amount as Amount, b.Status as Status from lpfn_GetUserLoans(" + this.iCurrentUserID + ") as a inner join V_ProcessingPipelineInfo as b on a.LoanID = b.FileId "
                      + "where (1=1) and (b.Stage='Closed')  " + sWhere;
                    UserLoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx1);
               
                    oLoanAmount = UserLoanList.Compute("Sum(Amount)", "Status='Closed'");
                  
                    if ((oLoanAmount != null) && (oLoanAmount != DBNull.Value))
                    {
                        iRealAmount = Convert.ToInt32(oLoanAmount, null);
                    }

                    #endregion
                }
                else if (this.sDateRange == "NextMonth")
                {
                    #region NextMonth

                    // 获取下个月天数
                    int iDaysInNextMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month);

                    // 本月开始日期和结束日期
                    DateTime StartDate = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1);
                    DateTime EndDate = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, iDaysInNextMonth);

                    //// 获取Loan Amount
                    //decimal dLoanAmount = this.UserGoalsManager.GetUserLoanAmount_Next(this.iCurrentUserID, StartDate, EndDate);

                    //// 转换成int
                    //iRealAmount = Convert.ToInt32(dLoanAmount);

                    sWhere += SqlTextBuilder.BuildDateSearchCondition(sFiledName, StartDate, EndDate);

                    sSqlx1 = "select b.Amount as Amount, b.Status as Status from lpfn_GetUserLoans(" + this.iCurrentUserID + ") as a inner join V_ProcessingPipelineInfo as b on a.LoanID = b.FileId "
                      + "where (1=1) and (b.Stage='Closed')  " + sWhere;
                    UserLoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx1);

                    oLoanAmount = UserLoanList.Compute("Sum(Amount)", "Stage='" + sStageAlias + "'");

                    if ((oLoanAmount != null) && (oLoanAmount != DBNull.Value))
                    {
                        iRealAmount = Convert.ToInt32(oLoanAmount, null);
                    }
                    #endregion
                }
            }
            else if (this.sDateRange.Contains("Quarter") == true)
            {
                // 计算This Quarter
                int iThisQuarter = this.GetQuarter(DateTime.Now.Month);

                if (this.sDateRange == "ThisQuarter")
                {
                    #region ThisQuarter

                    DateTime StartDate = DateTime.Now;
                    DateTime EndDate = DateTime.Now;

                    if (iThisQuarter == 1)  // 一季度=1,2,3月
                    {
                        // 1月开始日期
                        StartDate = new DateTime(DateTime.Now.Year, 1, 1);

                        // 获取3月天数
                        int iDaysIn3 = DateTime.DaysInMonth(DateTime.Now.Year, 3);
                        // 结束日期
                        EndDate = new DateTime(DateTime.Now.Year, 3, iDaysIn3);
                    }
                    else if (iThisQuarter == 2)  // 二季度=4,5,6月
                    {
                        // 4月开始日期
                        StartDate = new DateTime(DateTime.Now.Year, 4, 1);

                        // 获取6月天数
                        int iDaysIn6 = DateTime.DaysInMonth(DateTime.Now.Year, 6);
                        // 结束日期
                        EndDate = new DateTime(DateTime.Now.Year, 6, iDaysIn6);
                    }
                    else if (iThisQuarter == 3)  // 三季度=7,8,9月
                    {
                        // 7月开始日期
                        StartDate = new DateTime(DateTime.Now.Year, 7, 1);

                        // 获取9月天数
                        int iDaysIn9 = DateTime.DaysInMonth(DateTime.Now.Year, 9);
                        // 结束日期
                        EndDate = new DateTime(DateTime.Now.Year, 9, iDaysIn9);
                    }
                    else if (iThisQuarter == 4)  // 四季度=10,11,12月
                    {
                        // 7月开始日期
                        StartDate = new DateTime(DateTime.Now.Year, 10, 1);

                        // 获取12月天数
                        int iDaysIn12 = DateTime.DaysInMonth(DateTime.Now.Year, 12);
                        // 结束日期
                        EndDate = new DateTime(DateTime.Now.Year, 12, iDaysIn12);
                    }

                    //// 获取Loan Amount
                    //decimal dLoanAmount = this.UserGoalsManager.GetUserLoanAmount_This(this.iCurrentUserID, StartDate, EndDate);

                    //// 转换成int
                    //iRealAmount = Convert.ToInt32(dLoanAmount);

                    sWhere += SqlTextBuilder.BuildDateSearchCondition(sFiledName, StartDate, EndDate);

                    sSqlx1 = "select b.Amount as Amount, b.Status as Status from lpfn_GetUserLoans(" + this.iCurrentUserID + ") as a inner join V_ProcessingPipelineInfo as b on a.LoanID = b.FileId "
                      + "where (1=1) and (b.Stage='Closed')  " + sWhere;
                    UserLoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx1);

                    oLoanAmount = UserLoanList.Compute("Sum(Amount)", "Stage='" + sStageAlias + "'");

                    if ((oLoanAmount != null) && (oLoanAmount != DBNull.Value))
                    {
                        iRealAmount = Convert.ToInt32(oLoanAmount, null);
                    }
                    #endregion
                }
                else if (this.sDateRange == "NextQuarter")
                {
                    #region NextQuarter

                    int iNextQuarter = iThisQuarter + 1;
                    int iNextYear = DateTime.Now.Year;
                    if (iNextQuarter > 4)
                    {
                        iNextQuarter = 1;
                        iNextYear = DateTime.Now.AddYears(1).Year;
                    }

                    DateTime StartDate = DateTime.Now;
                    DateTime EndDate = DateTime.Now;

                    if (iNextQuarter == 1)  // 一季度=1,2,3月
                    {
                        // 1月开始日期
                        StartDate = new DateTime(iNextYear, 1, 1);

                        // 获取3月天数
                        int iDaysIn3 = DateTime.DaysInMonth(iNextYear, 3);
                        // 结束日期
                        EndDate = new DateTime(iNextYear, 3, iDaysIn3);
                    }
                    else if (iNextQuarter == 2)  // 二季度=4,5,6月
                    {
                        // 4月开始日期
                        StartDate = new DateTime(iNextYear, 4, 1);

                        // 获取6月天数
                        int iDaysIn6 = DateTime.DaysInMonth(iNextYear, 6);
                        // 结束日期
                        EndDate = new DateTime(iNextYear, 6, iDaysIn6);
                    }
                    else if (iNextQuarter == 3)  // 三季度=7,8,9月
                    {
                        // 7月开始日期
                        StartDate = new DateTime(iNextYear, 7, 1);

                        // 获取9月天数
                        int iDaysIn9 = DateTime.DaysInMonth(iNextYear, 9);
                        // 结束日期
                        EndDate = new DateTime(iNextYear, 9, iDaysIn9);
                    }
                    else if (iNextQuarter == 4)  // 四季度=10,11,12月
                    {
                        // 7月开始日期
                        StartDate = new DateTime(iNextYear, 10, 1);

                        // 获取12月天数
                        int iDaysIn12 = DateTime.DaysInMonth(iNextYear, 12);
                        // 结束日期
                        EndDate = new DateTime(iNextYear, 12, iDaysIn12);
                    }

                    //// 获取Loan Amount
                    //decimal dLoanAmount = this.UserGoalsManager.GetUserLoanAmount_Next(this.iCurrentUserID, StartDate, EndDate);

                    //// 转换成int
                    //iRealAmount = Convert.ToInt32(dLoanAmount);

                    sWhere += SqlTextBuilder.BuildDateSearchCondition(sFiledName, StartDate, EndDate);

                    sSqlx1 = "select b.Amount as Amount, b.Status as Status from lpfn_GetUserLoans(" + this.iCurrentUserID + ") as a inner join V_ProcessingPipelineInfo as b on a.LoanID = b.FileId "
                      + "where (1=1) and (b.Stage='Closed')  " + sWhere;
                    UserLoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx1);

                    oLoanAmount = UserLoanList.Compute("Sum(Amount)", "Stage='" + sStageAlias + "'");

                    if ((oLoanAmount != null) && (oLoanAmount != DBNull.Value))
                    {
                        iRealAmount = Convert.ToInt32(oLoanAmount, null);
                    }

                    #endregion
                }
            }
            else if (this.sDateRange.Contains("Year") == true) 
            {
                if (this.sDateRange == "ThisYear")
                {
                    #region ThisYear

                    // 开始日期：1月1日
                    DateTime StartDate = new DateTime(DateTime.Now.Year, 1, 1);

                    // 获取12月天数
                    int iDaysIn12 = DateTime.DaysInMonth(DateTime.Now.Year, 12);
                    // 结束日期
                    DateTime EndDate = new DateTime(DateTime.Now.Year, 12, iDaysIn12);

                    //// 获取Loan Amount
                    //decimal dLoanAmount = this.UserGoalsManager.GetUserLoanAmount_This(this.iCurrentUserID, StartDate, EndDate);

                    //// 转换成int
                    //iRealAmount = Convert.ToInt32(dLoanAmount);

                    sWhere += SqlTextBuilder.BuildDateSearchCondition(sFiledName, StartDate, EndDate);

                    sSqlx1 = "select b.Amount as Amount, b.Status as Status from lpfn_GetUserLoans(" + this.iCurrentUserID + ") as a inner join V_ProcessingPipelineInfo as b on a.LoanID = b.FileId "
                      + "where (1=1) and (b.Stage='Closed')  " + sWhere;
                    UserLoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx1);

                    oLoanAmount = UserLoanList.Compute("Sum(Amount)", "Stage='" + sStageAlias + "'");

                    if ((oLoanAmount != null) && (oLoanAmount != DBNull.Value))
                    {
                        iRealAmount = Convert.ToInt32(oLoanAmount, null);
                    }

                    #endregion
                }
                else if (this.sDateRange == "NextYear")
                {
                    #region NextYear

                    // 开始日期：1月1日
                    DateTime StartDate = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);

                    // 获取12月天数
                    int iDaysIn12 = DateTime.DaysInMonth(DateTime.Now.AddYears(1).Year, 12);
                    // 结束日期
                    DateTime EndDate = new DateTime(DateTime.Now.AddYears(1).Year, 12, iDaysIn12);

                    //// 获取Loan Amount
                    //decimal dLoanAmount = this.UserGoalsManager.GetUserLoanAmount_Next(this.iCurrentUserID, StartDate, EndDate);

                    //// 转换成int
                    //iRealAmount = Convert.ToInt32(dLoanAmount);

                    sWhere += SqlTextBuilder.BuildDateSearchCondition(sFiledName, StartDate, EndDate);

                    sSqlx1 = "select b.Amount as Amount, b.Status as Status from lpfn_GetUserLoans(" + this.iCurrentUserID + ") as a inner join V_ProcessingPipelineInfo as b on a.LoanID = b.FileId "
                      + "where (1=1) and (b.Stage='Closed')  " + sWhere;
                    UserLoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx1);

                    oLoanAmount = UserLoanList.Compute("Sum(Amount)", "Stage='" + sStageAlias + "'");

                    if ((oLoanAmount != null) && (oLoanAmount != DBNull.Value))
                    {
                        iRealAmount = Convert.ToInt32(oLoanAmount, null);
                    }
                    #endregion
                }
            }

            #endregion

            sLowRangeEnd = (iLowRangeEnd / 1000).ToString();
            sMediumRangeEnd = (iMediumRangeEnd / 1000).ToString();
            sHighRangeEnd = (iHighRangeEnd / 1000).ToString();
            sScaleInterval = iScaleInterval.ToString();
            sRealAmount = (iRealAmount / 1000).ToString();
        }

        private int GetQuarter(int iMonth) 
        {
            #region 计算This Quarter

            int iThisQuarter = 0;

            if (iMonth >= 1 && iMonth <= 3)
            {
                iThisQuarter = 1;
            }
            else if (iMonth >= 4 && iMonth <= 6)
            {
                iThisQuarter = 2;
            }
            else if (iMonth >= 7 && iMonth <= 9)
            {
                iThisQuarter = 3;
            }
            else if (iMonth >= 10 && iMonth <= 12)
            {
                iThisQuarter = 4;
            }

            #endregion

            return iThisQuarter;
        }
    }
}