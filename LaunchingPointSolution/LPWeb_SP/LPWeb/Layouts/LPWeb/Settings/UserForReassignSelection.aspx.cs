using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using Utilities;
using System.Text;
using System.Web.UI.WebControls;
using LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Settings
{
    public partial class UserForReassignSelection : BasePage
    {
        BLL.Users UsersManager = new BLL.Users();
        DataTable dtUserBranch = new DataTable();
        private bool isReset = false;
        int nUserId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";
            bool bIsValid = PageCommon.ValidateQueryString(this, "uid", QueryStringType.ID);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, "Missing required query string.", sCloseDialogCodes);
            }
            this.nUserId = Convert.ToInt32(this.Request.QueryString["uid"]);

            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        /// <summary>
        /// Bind prospect loan gridview
        /// </summary>
        private void BindGrid()
        {
            // Get user branch info
            dtUserBranch = UsersManager.GetUserBranchInfo();

            int pageSize = AspNetPager1.PageSize;
            int pageIndex = 1;

            if (isReset == true)
                pageIndex = AspNetPager1.CurrentPageIndex = 1;
            else
                pageIndex = AspNetPager1.CurrentPageIndex;

            string strWhare = GetSqlWhereClause();
            int recordCount = 0;

            DataSet dsList = null;
            try
            {
                dsList = UsersManager.GetListForUserReassign(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gridList.DataSource = dsList;
            gridList.DataBind();
        }

        /// <summary>
        /// Get filter
        /// </summary>
        /// <returns></returns>
        private string GetSqlWhereClause()
        {
            string strWhere = string.Format(" AND EXISTS (SELECT 1 FROM Users WHERE RoleId=u.RoleId and UserId='{0}') AND UserEnabled=1 AND UserId<>{0}", this.nUserId);

            if (UsersManager.IsCompanyUser(this.nUserId))
            {

            }
            else if (UsersManager.IsRegionUser(this.nUserId))
            {
                strWhere += string.Format(@" AND (EXISTS (SELECT 1 FROM (SELECT UserId FROM GroupUsers gu WHERE EXISTS (SELECT 1 FROM Groups WHERE RegionID in 
(SELECT RegionID from Groups as a INNER JOIN GroupUsers as b on a.GroupId = b.GroupID 
    INNER JOIN Users as c on b.UserID = c.UserId where OrganizationType='Region' and b.UserID='{0}'
) AND GroupId=gu.GroupID)) t1 WHERE u.UserId=t1.UserID))", this.nUserId);
            }
            else if (UsersManager.IsDivisionUser(this.nUserId))
            {
                strWhere += string.Format(@" AND (EXISTS (SELECT 1 FROM 
(SELECT UserId FROM GroupUsers gu WHERE EXISTS (SELECT 1 FROM Groups WHERE DivisionID in 
(
    SELECT DivisionID FROM Groups AS a INNER JOIN GroupUsers AS b ON a.GroupId = b.GroupID 
    INNER JOIN Users AS c ON b.UserID = c.UserId WHERE OrganizationType='Division' AND b.UserID = '{0}'
) AND GroupId=gu.GroupID)) t1 WHERE u.UserId=t1.UserID))", this.nUserId);
            }
            else if (UsersManager.IsBranchUser(this.nUserId))
            {
                strWhere += string.Format(@" AND (EXISTS (SELECT 1 FROM 
(SELECT UserId FROM GroupUsers gu WHERE EXISTS (SELECT 1 FROM Groups WHERE BranchID in
(
    SELECT BranchID FROM Groups AS a INNER JOIN GroupUsers AS b ON a.GroupId = b.GroupID 
    INNER JOIN Users AS c ON b.UserID = c.UserId WHERE OrganizationType='Branch' AND b.UserID = '{0}'
) AND GroupId=gu.GroupID)) t1 WHERE u.UserId=t1.UserID))", this.nUserId);
            }
            else
            {
                strWhere += " AND 1=2";
            }
            return strWhere;
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

        StringBuilder sbAllIds = new StringBuilder();
        string strCkAllID = "";
        /// <summary>
        /// Set selected row when click
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

                Label lblBranch = e.Row.FindControl("lblBranch") as Label;
                int nUserID = 0;
                if (!int.TryParse(gv.DataKeys[e.Row.RowIndex].Value.ToString(), out nUserID))
                    nUserID = 0;
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
    }
}
