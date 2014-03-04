using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using Utilities;

namespace LPWeb.Layouts.LPWeb.LoanDetails
{
    public partial class Activity : BasePage
    {
        BLL.LoanActivities LA = new BLL.LoanActivities();
        private bool isReset = false;
        private string strFileId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            strFileId = Request.QueryString["FileID"];
            if (!IsPostBack)
            {
                string strProWhere = "";
                if (!string.IsNullOrEmpty(strFileId))
                    strProWhere = string.Format("AND FileId='{0}'", strFileId);
                this.ddlPerformedBy.DataTextField = "PerformedBy";
                this.ddlPerformedBy.DataValueField = "UserId";
                this.ddlPerformedBy.DataSource = LA.GetProformedBy(strProWhere);
                this.ddlPerformedBy.DataBind();
                this.ddlPerformedBy.Items.Insert(0, new ListItem("All", ""));
                this.ddlPerformedBy.Items.Add(new ListItem("System", "0"));
                BindGrid();
            }
        }
        protected void gridList_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (DataControlRowType.DataRow == e.Row.RowType)
            {
                LinkButton lbtnActivity = e.Row.FindControl("lbtnActivity") as LinkButton;
                if (null != lbtnActivity)
                {
                    string str = "";
                    str = Server.HtmlDecode(lbtnActivity.Text);
                    lbtnActivity.ToolTip = str;
                    if (lbtnActivity.Text.Length > 80)
                    {
                        lbtnActivity.Text = lbtnActivity.Text.Substring(0, 80) + "...";
                    }
                }
            }
        }

        /// <summary>
        /// Bind email template gridview
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

            DataSet listData = null;
            try
            {
                listData = LA.GetListForGridView(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gridList.DataSource = listData;
            gridList.DataBind();
        }

        /// <summary>
        /// Get filter
        /// </summary>
        /// <returns></returns>
        private string GetSqlWhereClause()
        {
            string strWhere = "";
            if (!string.IsNullOrEmpty(strFileId))
                strWhere = string.Format(" AND FileID='{0}'", strFileId);

            if (this.ddlPerformedBy.SelectedIndex > 0)
            {
                if (strWhere.Length > 0)
                {
                    strWhere = string.Format(" {0} AND ISNULL(UserId2, '0')='{1}'", strWhere, this.ddlPerformedBy.SelectedValue);
                }
                else
                {
                    strWhere = string.Format(" AND ISNULL(UserId2, '0')='{0}'", ddlPerformedBy.SelectedValue);
                }
            }

            if (!string.IsNullOrEmpty(this.tbStartDate.Text))
            {
                DateTime dt = new DateTime();
                DateTime dtNew = new DateTime();
                try
                {
                    if (DateTime.TryParse(this.tbStartDate.Text, out dt))
                    {
                        dtNew = new DateTime(dt.Year, dt.Month, dt.Day);
                        if (strWhere.Length > 0)
                            strWhere = string.Format(" {0} AND ActivityTime>='{1}'", strWhere, dtNew.ToString());
                        else
                            strWhere = string.Format(" AND ActivityTime>='{0}'", dtNew.ToString());
                    }
                }
                catch
                {

                }
            }

            if (!string.IsNullOrEmpty(this.tbEndDate.Text))
            {
                DateTime dt = new DateTime();
                DateTime dtNew = new DateTime();
                try
                {
                    if (DateTime.TryParse(this.tbEndDate.Text, out dt))
                    {
                        dtNew = new DateTime(dt.Year, dt.Month, dt.Day + 1).AddMilliseconds(-1);
                        if (strWhere.Length > 0)
                            strWhere = string.Format("{0} AND ActivityTime<='{1}'", strWhere, dtNew.ToString());
                        else
                            strWhere = string.Format(" AND ActivityTime<='{0}'", dtNew.ToString());
                    }
                }
                catch
                { 

                }
            }

            return strWhere;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }

        protected void ddlPerformedBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
            BindGrid();
        }

        /// <summary>
        /// Handles the Sorting event of the gridUserList control.
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
                    ViewState["orderName"] = "ActivityTime";
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
    }
}