using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using Utilities;
using LPWeb.BLL;

namespace LPWeb.Prospect
{
    public partial class ProspectActivityTab : BasePage
    {
        BLL.ProspectActivities prospectActivities = new BLL.ProspectActivities();
        private bool isReset = false;
        private string strContactId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            strContactId = Request.QueryString["ContactID"];
            if (!IsPostBack)
            {
                string strProWhere = "";
                if (!string.IsNullOrEmpty(strContactId))
                    strProWhere = string.Format("AND ContactId='{0}'", strContactId);
                this.ddlPerformedBy.DataTextField = "PerformedBy";
                this.ddlPerformedBy.DataValueField = "UserId";
                this.ddlPerformedBy.DataSource = prospectActivities.GetProformedBy(strProWhere);
                this.ddlPerformedBy.DataBind();
                this.ddlPerformedBy.Items.Insert(0, new ListItem("All", ""));
                this.ddlPerformedBy.Items.Add(new ListItem("System", "-1"));

                BindActivityType();
                BindGrid();
            }
        }

        private void BindActivityType()
        {
            DataTable dtSource = prospectActivities.GetActivityTypeInfo(Convert.ToInt32(Request.QueryString["ContactID"]));
            DataRow dr = dtSource.NewRow();
            dr["ActivityTypeName"] = "Prospect Activities";
            dr["ContactId"] = 0;
            dtSource.Rows.InsertAt(dr, 0);
            dr = dtSource.NewRow();
            dr["ActivityTypeName"] = "All Activities";
            dr["ContactId"] = -1;
            dtSource.Rows.InsertAt(dr, 0);

            ddlActivityType.DataSource = dtSource;
            ddlActivityType.DataBind();

           
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
                listData = prospectActivities.GetListForGridView(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType, Convert.ToInt32(Request.QueryString["ContactID"]));
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;
            LoanActivities bLoanActivities=new LoanActivities();

            if (listData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in listData.Tables[0].Rows)
                {
                    if (dr["ActivityType"].ToString() == "Prospect Activity")
                    {
                        dr["ActivityFile"] = "";
                    }
                    string sProspectPerformaBy = prospectActivities.GetProformedBy(" and ProspectActivityId=" + dr["ActivityId"].ToString()).Rows.Count > 0 ? prospectActivities.GetProformedBy(" and ProspectActivityId=" + dr["ActivityId"].ToString()).Rows[0]["PerformedBy"].ToString() : "System";
                    string sLoanPerformaBy = bLoanActivities.GetProformedBy(" and ActivityId=" + dr["ActivityId"].ToString()).Rows.Count > 0 ? bLoanActivities.GetProformedBy(" and ActivityId=" + dr["ActivityId"].ToString()).Rows[0]["PerformedBy"].ToString() : "System";

                    dr["PerformedBy"] = (dr["form"].ToString() == "1") ? sProspectPerformaBy : sLoanPerformaBy;
                }
                listData.Tables[0].AcceptChanges();
            }
            listData.AcceptChanges();

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
            //if (!string.IsNullOrEmpty(strContactId))
            //    strWhere = string.Format(" AND ContactId='{0}'", strContactId);

            if (ddlActivityType.SelectedValue.ToString() == "-1")
            {
            }
            else if (ddlActivityType.SelectedValue.ToString() == "0")
            {
                strWhere += " and form='1' ";
            }
            else if (Convert.ToInt32(ddlActivityType.SelectedValue.ToString()) > 0)
            {
                strWhere += " and FileId=" + ddlActivityType.SelectedValue.ToString();
            }

            if (this.ddlPerformedBy.SelectedIndex > 0)
            {
                if (strWhere.Length > 0)
                {
                    strWhere = string.Format(" {0} AND ISNULL(UserId, '-1')='{1}'", strWhere, this.ddlPerformedBy.SelectedValue);
                }
                else
                {
                    strWhere = string.Format(" AND ISNULL(UserId, '-1')='{0}'", ddlPerformedBy.SelectedValue);
                }
            }

            if (!string.IsNullOrEmpty(this.tbStartDate.Text))
            {
                if (strWhere.Length > 0)
                    strWhere = string.Format(" {0} AND ActivityTime>='{1}'", strWhere, this.tbStartDate.Text);
                else
                    strWhere = string.Format(" AND ActivityTime>='{0}'", this.tbStartDate.Text);
            }

            if (!string.IsNullOrEmpty(this.tbEndDate.Text))
            {
                if (strWhere.Length > 0)
                    strWhere = string.Format("{0} AND ActivityTime<='{1}'", strWhere, this.tbEndDate.Text);
                else
                    strWhere = string.Format(" AND ActivityTime<='{0}'", this.tbEndDate.Text);
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