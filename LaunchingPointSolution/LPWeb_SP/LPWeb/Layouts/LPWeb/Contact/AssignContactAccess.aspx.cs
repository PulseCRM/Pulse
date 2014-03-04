using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using Utilities;
using System.Web.UI.WebControls;
using System.Text;
using LPWeb.Common;
using System.Web.UI;

namespace LPWeb.Layouts.LPWeb.Contact
{
    public partial class AssignContactAccess : LayoutsPageBase
    {
        private bool isReset = false;
        BLL.Users userMngr = new BLL.Users();
        DataTable dtUserBranch = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString["FromPage"] != null) // FromPage
            {
                this.hiFromPage.Value = this.Request.QueryString["FromPage"];
            }
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        /// <summary>
        /// Bind contact role gridview
        /// </summary>
        private void BindGrid()
        {
            // Get user branch info
            dtUserBranch = userMngr.GetUserBranchInfo();

            int pageSize = AspNetPager1.PageSize;
            int pageIndex = 1;

            if (isReset == true)
                pageIndex = AspNetPager1.CurrentPageIndex = 1;
            else
                pageIndex = AspNetPager1.CurrentPageIndex;

            string strWhare = " AND UserEnabled=1";
            int recordCount = 0;

            DataSet contactRoles = null;
            try
            {
                contactRoles = userMngr.GetListForGridView(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gridList.DataSource = contactRoles;
            gridList.DataBind();
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
                    ViewState["orderName"] = "FullName";
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

        StringBuilder sbAllIds = new StringBuilder();
        string strCkAllID = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView gv = sender as GridView;
            if (null == gv)
                return;

            if (DataControlRowType.Header == e.Row.RowType)
            {
                CheckBox ckbAll = e.Row.FindControl("ckbAll") as CheckBox;
                if (null != ckbAll)
                {
                    ckbAll.Attributes.Add("onclick", string.Format("CheckAllClicked(this, '{0}', '{1}', '{2}');",
                        gv.ClientID, this.hiAllIds.ClientID, this.hiCheckedIds.ClientID));
                    strCkAllID = ckbAll.ClientID;
                }
            }
            else if (DataControlRowType.DataRow == e.Row.RowType)
            {
                string strID = gv.DataKeys[e.Row.RowIndex].Value.ToString();

                if (sbAllIds.Length > 0)
                    sbAllIds.Append(",");
                sbAllIds.AppendFormat("{0}", strID);

                CheckBox ckb = e.Row.FindControl("ckbSelect") as CheckBox;
                if (null != ckb)
                {
                    ckb.Attributes.Add("onclick", string.Format("CheckBoxClicked(this, '{0}', '{1}', '{2}', '{3}');",
                        strCkAllID, this.hiAllIds.ClientID, this.hiCheckedIds.ClientID, strID));
                }

                TextBox lblBranch = e.Row.FindControl("lblBranch") as TextBox;
                int nUserID = 0;
                if (null != gridList.DataKeys[e.Row.RowIndex])
                {
                    if (!int.TryParse(gridList.DataKeys[e.Row.RowIndex].Value.ToString(), out nUserID))
                        nUserID = 0;

                    if (0 != nUserID)
                    {
                        if (null != lblBranch && null != dtUserBranch)
                        {
                            // concatenates all user branch names, using "," between each name
                            StringBuilder sbBName = new StringBuilder();
                            DataRow[] drs = dtUserBranch.Select(string.Format("UserId={0}", nUserID));
                            if (null != drs)
                            {
                                foreach (DataRow row in drs)
                                {
                                    if (sbBName.Length > 0)
                                        sbBName.Append(", ");
                                    sbBName.Append(row["Name"]);
                                }
                            }
                            int nDisLen = 30;
                            if (sbBName.Length > nDisLen)
                            {
                                lblBranch.ToolTip = sbBName.ToString();
                                lblBranch.Text = sbBName.ToString().Substring(0, nDisLen) + "...";
                            }
                            else
                                lblBranch.Text = sbBName.ToString();
                        }
                    }
                }
            }
        }

        protected void gridList_PreRender(object sender, EventArgs e)
        {
            this.hiAllIds.Value = sbAllIds.ToString();
            this.hiCheckedIds.Value = "";
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

        //protected void btnSelect_Click(object sender, EventArgs e)
        //{
        //    string sSelUserIDs = this.hiCheckedIds.Value;
        //    Response.Write("<script> window.parent.AssignContactAccessPopupSelected('" + sSelUserIDs + "');window.opener=null; window.close();</script>");

        //}
    }
}
