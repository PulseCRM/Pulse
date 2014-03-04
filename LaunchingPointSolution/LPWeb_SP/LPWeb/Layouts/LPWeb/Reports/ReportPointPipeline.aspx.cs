using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using LPWeb.BLL;
using LPWeb.Common;
using Utilities;
using System.Text;
using System.Collections.Specialized;

namespace LPWeb.Reports
{
    public partial class ReportPointPipeline : BasePage
    {
        private bool isReset = false;
        Regions region = new Regions();
        int iRegionID = -1;
        int iDivision = -1;
        int iBranch = -1;

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                LoginUser loginUser = new LoginUser();

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
            string sErrorMsg = "Failed to load current page: invalid FileID.";
            string sReturnPage = "ReportRegionProduction.aspx";

            if (this.Request.QueryString["Region"] != null) // 如果有GrouRegionIDpID
            {
                string sRegionID = this.Request.QueryString["Region"].ToString();

                if (PageCommon.IsID(sRegionID) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }

                this.iRegionID = Convert.ToInt32(sRegionID);
            }

            if (this.Request.QueryString["Division"] != null) 
            {
                string sDivision = this.Request.QueryString["Division"].ToString();

                if (PageCommon.IsID(sDivision) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }

                this.iDivision = Convert.ToInt32(sDivision);
            }
            //Branch
            if (this.Request.QueryString["Branch"] != null)
            {
                string sBranch = this.Request.QueryString["Branch"].ToString();

                if (PageCommon.IsID(sBranch) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }

                this.iBranch = Convert.ToInt32(sBranch);
            }

            if (!IsPostBack)
            {
                BindReportGrid();
            }
        }
        /// <summary>
        /// Bind Grid
        /// </summary>
        private void BindReportGrid()
        {
            int pageSize = AspNetPager1.PageSize;
            int pageIndex = 1;

            if (AspNetPager1.CurrentPageIndex > 0 && isReset == false)
                pageIndex = AspNetPager1.CurrentPageIndex;
            //CurrUser.bAccessOtherLoans
            int recordCount = 0;
            BLL.LoanPointFields bllLPF = new LoanPointFields();
            DataSet reports = null;


            #region Where
            string whereStr = " 1=1 ";

            if (iRegionID != -1)
            {
                whereStr += " AND RegionID= " + iRegionID.ToString();
            }
            if (iDivision != -1)
            {
                whereStr += " AND DivisionID= " + iDivision.ToString();
            }
            if (iBranch != -1)
            {
                whereStr += " AND BranchId = " + iBranch.ToString();
            }
            whereStr += " ";
            #endregion

            try
            {
                reports = bllLPF.GetProcessingList(pageSize, pageIndex, whereStr, out recordCount, OrderName, OrderType, CurrUser.iUserID, CurrUser.bAccessOtherLoans);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gvPointPipelineReport.DataSource = reports;
            gvPointPipelineReport.DataBind();
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
            BindReportGrid();
        }
        /// <summary>
        /// Gets or sets the name of the order.
        /// </summary>
        /// <value>The name of the order.</value>
        public string OrderName
        {
            get
            {
                if (ViewState["orderName"] == null)
                    ViewState["orderName"] = "LastName";
                return Convert.ToString(ViewState["orderName"]);
            }
            set
            {
                ViewState["orderName"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the order.
        /// </summary>
        /// <value>The type of the order.</value>
        public int OrderType
        {
            get
            {
                if (ViewState["orderType"] == null)
                    ViewState["orderType"] = 0;
                return Convert.ToInt32(ViewState["orderType"]);
            }
            set
            {
                ViewState["orderType"] = value;
            }
        }


        /// <summary>
        /// Gets or sets the grid view sort direction.
        /// </summary>
        /// <value>The grid view sort direction.</value>
        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;
                return (SortDirection)ViewState["sortDirection"];
            }
            set
            {
                ViewState["sortDirection"] = value;
            }

        }

        protected void gvPipelineView_Sorting(object sender, GridViewSortEventArgs e)
        {
            OrderName = e.SortExpression;

            if (GridViewSortDirection == SortDirection.Ascending)                      //设置排序方向
            {
                GridViewSortDirection = SortDirection.Descending;
                OrderType = 0;
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                OrderType = 1;
            }
            BindReportGrid();
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            int recordCount = 0;
            DataTable dtReports = null;
            BLL.LoanPointFields bllLPF = new LoanPointFields();

            #region Where
            string whereStr = " 1=1 ";

            if (iRegionID != -1)
            {
                whereStr += " AND RegionID= " + iRegionID.ToString();
            }
            if (iDivision != -1)
            {
                whereStr += " AND DivisionID= " + iDivision.ToString();
            }
            if (iBranch != -1)
            {
                whereStr += " AND BranchId = " + iBranch.ToString();
            }
            whereStr += " "; 
            #endregion

            DataSet reports = bllLPF.GetProcessingList(65535, 0, whereStr, out recordCount, OrderName, OrderType, CurrUser.iUserID, CurrUser.bAccessOtherLoans);
            if (null != reports && reports.Tables.Count > 0)
                dtReports = reports.Tables[0];

            // Excel Columns
            NameValueCollection ExcelCollection = new NameValueCollection();
            ExcelCollection.Add("Branch", "Branch");
            ExcelCollection.Add("Borrower Last Name", "LastName");
            ExcelCollection.Add("Borrower First Name", "FirstName");
            ExcelCollection.Add("Lender", "Lender");
            ExcelCollection.Add("Loan Originator", "LoanOriginator");
            ExcelCollection.Add("Loan Amount", "LoanAmount");
            ExcelCollection.Add("Note Rate", "NoteRate");
            ExcelCollection.Add("Status", "Status");
            ExcelCollection.Add("Status Date", "StatusDate");
            ExcelCollection.Add("Loan Processor", "LoanProcessor");
            ExcelCollection.Add("Loan Program", "LoanProgram");
            ExcelCollection.Add("Loan Origination Fee", "LoanOriginationFee");
            ExcelCollection.Add("GFE Date", "GFEDate");
            ExcelCollection.Add("LTV Ratio", "LTVRatio");
            ExcelCollection.Add("Net Adjusted Price", "NetAdjustedPrice");
            ExcelCollection.Add("Lock Date", "LockDate");

            // sheet name
            string sSheetName = "Point Pipeline Report";

            // 显示给用户的Xls文件名
            string sClientXlsFileName = "Point_Pipeline_Report.xls";

            // export and download
            XlsExporter.DownloadXls(this, dtReports, sClientXlsFileName, sSheetName);
        }

    }
}
