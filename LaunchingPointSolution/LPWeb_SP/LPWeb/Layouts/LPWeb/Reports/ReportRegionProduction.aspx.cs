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
    public partial class ReportRegionProduction : BasePage
    {
        private bool isReset = false;
        Regions region = new Regions();
        int iRegionID = 0;
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
            //if (iRegionID == 0 || iRegionID == -1)
            //{
            //    gvRegionReport.DataSource = null;
            //    gvRegionReport.DataBind();
            //    return;
            //}
            BindReportGrid();
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
             
            int recordCount = 0;

            DataSet reports = null;
            try
            {
                reports = region.GetRegionGoalsReport(pageSize, pageIndex, iRegionID.ToString(), out recordCount, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gvRegionReport.DataSource = reports;
            gvRegionReport.DataBind(); 
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
                    ViewState["orderName"] = "RegionName";
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
            DataSet reports = region.GetRegionGoalsReport(65535, 0, iRegionID.ToString(), out recordCount, OrderName, OrderType);
            if (null != reports && reports.Tables.Count > 0)
                dtReports = reports.Tables[0];

            // Excel Columns
            NameValueCollection ExcelCollection = new NameValueCollection();
            ExcelCollection.Add("Region", "RegionName");
            ExcelCollection.Add("Progress", "Progress");
            ExcelCollection.Add("Running Total", "RunningTotal");
            ExcelCollection.Add("Low Range", "LowRange");
            ExcelCollection.Add("Medium Range", "MediumRange");
            ExcelCollection.Add("High Range", "HighRange");

            // sheet name
            string sSheetName = "Region Production Goals Report";

            // 显示给用户的Xls文件名
            DateTime dt = DateTime.Now;
            string sClientXlsFileName = "Region_Production_Goals_Report-" + dt.ToString("MM_dd_yy") + ".xls";

            // export and download
            XlsExporter.DownloadXls(this, dtReports, sClientXlsFileName, sSheetName);
        }
    }
}