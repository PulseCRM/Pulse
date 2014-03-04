using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.DAL;
using System.Data;
using System.Collections.Specialized;
using System.IO;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using Utilities;

namespace LPWeb.Layouts.LPWeb.Reports
{
    public partial class ReportUserProduction : LayoutsPageBase
    {
        string sDbTable = string.Empty;
        string sWhere = string.Empty;
        DataTable ReportData = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoginUser loginUser = new LoginUser();
            try
            {

                if (loginUser.userRole.Reports == false)
                {
                    Response.Redirect("../Unauthorize.aspx");
                    return;
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
            this.ReportSqlDataSource.ConnectionString = DbHelperSQL.connectionString;

            this.sDbTable = "select a.BranchID, d.Name as BranchName, a.UserId, c.LastName +', '+ c.FirstName as FullName, c.LastName, "
                          + "case when a.HighRange = 0.00 then 0 else CAST(b.LoanAmount*1.0/a.HighRange * 100 as numeric(18, 2)) end as Progress, "
                          + "b.LoanAmount as RunningTotal, a.LowRange, a.MediumRange, a.HighRange, a.RegionID, a.DivisionID "
                          + "from (select x.UserId, "
                          + "ISNULL(y.LowRange, 0.00) as LowRange, "
                          + "ISNULL(y.MediumRange, 0.00) as MediumRange, "
                          + "ISNULL(y.HighRange, 0.00) as HighRange, "
                          + "x.RegionID, x.DivisionID, x.BranchID "
                          + "from lpfn_GetLoanOfficerBelongToLoginUser(" + loginUser.iUserID + ") as x "
                          + "left outer join ( "
	                      + "    select * from UserGoals where Month = MONTH(GETDATE()) "
                          + ") as y on x.UserId = y.UserId ) as a "
                          + "inner join ( "
	                      + "    select u.UserID, "
	                      + "    SUM(ISNULL(s.LoanAmount, 0.00)) as LoanAmount "
                          + "    from lpfn_GetLoanOfficerBelongToLoginUser(" + loginUser.iUserID + ") as u "
	                      + "    left outer join LoanTeam as t on u.UserId = t.UserId "
	                      + "    left outer join ( "
		                  + "        select * from Loans where Status='Closed' and DATEDIFF(MONTH, DateClose, GETDATE())=0 "
	                      + "    ) as s on t.FileId = s.FileId "
	                      + "    group by u.UserID "
                          + ") as b on a.UserId = b.UserId "
                          + "inner join Users as c on a.UserId = c.UserId "
                          + "left outer join Branches as d on a.BranchID = d.BranchId";

            #region sWhere

            // Region
            if (this.Request.QueryString["Region"] != null)
            {
                string sRegion = this.Request.QueryString["Region"].ToString();
                if(sRegion != "-1")
                {
                    this.sWhere += " and RegionID = " + sRegion;
                }
            }

            // Division
            if (this.Request.QueryString["Division"] != null)
            {
                string sDivision = this.Request.QueryString["Division"].ToString();
                if (sDivision != "-1")
                {
                    this.sWhere += " and DivisionID = " + sDivision;
                }
            }

            // Branch
            if (this.Request.QueryString["Branch"] != null)
            {
                string sBranch = this.Request.QueryString["Branch"].ToString();
                if (sBranch != "-1")
                {
                    this.sWhere += " and BranchId = " + sBranch;
                }
            }

            // Alphabet
            if (this.Request.QueryString["Alphabet"] != null)
            {
                string sAlphabet = this.Request.QueryString["Alphabet"].ToString();
                this.sWhere += " and UPPER(LastName) like '" + sAlphabet + "%'";
            }

            #endregion

            int iCount = DbHelperSQL.Count("(" + this.sDbTable + ") as m", this.sWhere);
            this.AspNetPager1.RecordCount = iCount;

            // 绑定数据
            this.ReportSqlDataSource.SelectParameters["DbTable"].DefaultValue = "(" + this.sDbTable + ") as m";
            this.ReportSqlDataSource.SelectParameters["Where"].DefaultValue = this.sWhere;
            this.gridReportGrid.DataBind();

            
        }

        protected void btnExport_Click(object sender, EventArgs e) 
        {
            string sSql2 = "select * from (" + this.sDbTable + ") as m where 1=1" + this.sWhere;
            this.ReportData = DbHelperSQL.ExecuteDataTable(sSql2);

            // Excel Columns
            NameValueCollection ExcelCollection = new NameValueCollection();
            ExcelCollection.Add("Branch", "BranchName");
            ExcelCollection.Add("Name", "FullName");
            ExcelCollection.Add("Progress", "Progress");
            ExcelCollection.Add("Running Total", "RunningTotal");
            ExcelCollection.Add("Low Total", "LowRange");
            ExcelCollection.Add("Medium Total", "MediumRange");
            ExcelCollection.Add("High Range", "HighRange");

            // sheet name
            string sSheetName = "User Production Goals";

            // 显示给用户的Xls文件名            
            DateTime dt = DateTime.Now;
            string sClientXlsFileName = "User_Production_Goals_Report-" + dt.ToString("MM_dd_yy") + ".xls";

            // export and download
            XlsExporter.DownloadXls(this, ReportData, sClientXlsFileName, sSheetName);
        }

        public int GetProgressBarWidth(string sProgress)
        {
            decimal dProgress = Convert.ToDecimal(sProgress);
            decimal dProgressWidth = 142 * dProgress / 100;
            int iWidth = Convert.ToInt32(dProgressWidth);
            if (iWidth > 142) 
            {
                iWidth = 142;
            }

            return iWidth;
        }

        public int GetPercentPosition(string sProgress) 
        {
            if(sProgress.Length == 4)   // 5.00
            {
                return 55;
            }
            else if (sProgress.Length == 5)   // 50.00
            {
                return 60;
            }
            else if (sProgress.Length == 6)   // 100.00
            {
                return 50;
            }
            else   // 1000.00
            {
                return 45;
            }
        }
    }
}
