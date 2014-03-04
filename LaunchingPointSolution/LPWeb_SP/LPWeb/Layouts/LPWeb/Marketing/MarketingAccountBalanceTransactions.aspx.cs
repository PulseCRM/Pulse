using System;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using Utilities;
using LPWeb.BLL;
using LPWeb.LP_Service;
using LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Marketing
{
    public partial class MarketingAccountBalanceTransactions : BasePage
    {
        UserMarketingTrans umtMngr = new UserMarketingTrans();
        private bool isReset = false;
        protected string FromURL = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Url != null)
            {
                FromURL = Request.Url.ToString();
            }

            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        /// <summary>
        /// Bind UserMarketingTrans gridview
        /// </summary>
        private void BindGrid()
        {
            int pageSize = AspNetPager1.PageSize;
            int pageIndex = 1;

            if (isReset == true)
                pageIndex = AspNetPager1.CurrentPageIndex = 1;
            else
                pageIndex = AspNetPager1.CurrentPageIndex;

            string strWhare = GetSqlWhereClause();
            int recordCount = 0;

            DataSet umTrans = null;
            try
            {
                umTrans = umtMngr.GetListForGridView(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gridList.DataSource = umTrans;
            gridList.DataBind();
        }

        /// <summary>
        /// Get filter
        /// </summary>
        /// <returns></returns>
        private string GetSqlWhereClause()
        {
            StringBuilder sbWhere = new StringBuilder();

            if (this.ddlAction.SelectedIndex > 0)
            {
                sbWhere.AppendFormat(" AND Action='{0}'", this.ddlAction.SelectedValue);
            }

            if (!string.IsNullOrEmpty(this.tbStartDate.Text))
            {
                sbWhere.AppendFormat(" AND TransTime>='{0}'", this.tbStartDate.Text);
            }

            if (!string.IsNullOrEmpty(this.tbEndDate.Text))
            {
                sbWhere.AppendFormat(" AND TransTime<='{0}'", this.tbEndDate.Text);
            }

            return sbWhere.ToString();
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
            BindGrid();
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

        /// <summary>
        /// Gets or sets the name of the order.
        /// </summary>
        /// <value>The name of the order.</value>
        public string OrderName
        {
            get
            {
                if (ViewState["orderName"] == null)
                    ViewState["orderName"] = "TransTime";
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
        /// Handles the Sorting event of the gridList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gridList_Sorting(object sender, GridViewSortEventArgs e)
        {
            OrderName = e.SortExpression;
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                OrderType = 0;
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                OrderType = 1;
            }
            BindGrid();
        }

        protected void gridList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (DataControlRowType.DataRow == e.Row.RowType)
            {
                LinkButton lbtnDesc = e.Row.FindControl("lbtnDesc") as LinkButton;
                if (null != lbtnDesc)
                {
                    string strFileId = string.Format("{0}", gridList.DataKeys[e.Row.RowIndex]["FileId"]);
                    string strLoanStatus = string.Format("{0}", gridList.DataKeys[e.Row.RowIndex]["LoanStatus"]);
                    string strDesc = string.Format("{0}", gridList.DataKeys[e.Row.RowIndex]["Description"]);
                    lbtnDesc.OnClientClick = string.Format("showMarketingInfo('{0}','{1}'); return false;", strFileId, strLoanStatus);
                    if (strDesc.Length > 50)
                    {
                        lbtnDesc.ToolTip = strDesc;
                        lbtnDesc.Text = strDesc.Substring(0, 50) + "...";
                    }
                    else
                        lbtnDesc.Text = strDesc;
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }
    }
}
