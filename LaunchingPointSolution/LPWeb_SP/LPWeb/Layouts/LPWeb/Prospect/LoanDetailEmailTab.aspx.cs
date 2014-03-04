using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Web.UI.WebControls;
using System.Text;
using Utilities;
using System.Data;
using System.Text.RegularExpressions;
using LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Prospect
{
    /// <summary>
    /// Prospect Detail View - Emails Tab
    /// Author: Peter
    /// Date: 2011-02-24
    /// </summary>
    public partial class LoanDetailEmailTab : BasePage
    {

        protected string sHasSendRight = "0";
        private LoginUser _curLoginUser = new LoginUser();

        BLL.EmailLog emailLogManager = new BLL.EmailLog();
        private bool isReset = false;
        protected string strFrom = "";
        private string strItemId = "";
        protected string strLoanId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            strFrom = Request.QueryString["from"];
            strItemId = Request.QueryString["itemId"];
            if ("0" == strFrom || "2" == strFrom)
            {
                strLoanId = strItemId;
            }

            if ("2" == strFrom)
            {
                //是否有Send权限
                sHasSendRight = _curLoginUser.userRole.Loans.ToString().IndexOf('K') > 0 ? "1" : "0";  //Send 
            }
            else if ("1" == strFrom)
            {
                //是否有Send权限
                sHasSendRight = _curLoginUser.userRole.Prospect.ToString().IndexOf('O') > 0 ? "1" : "0";  //Send  
            }
            else if ("3" == strFrom)
            {
                //是否有Send权限
                sHasSendRight = _curLoginUser.userRole.Prospect.ToString().IndexOf('O') > 0 ? "1" : "0";  //Send  
            }
            else
            {
                //From=0 表示Processing，根据SendEmail 操作权限 控制
                sHasSendRight = _curLoginUser.userRole.SendEmail == true ? "1" : "0";
            }


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
            int pageSize = AspNetPager1.PageSize;
            int pageIndex = 1;

            if (isReset == true)
                pageIndex = AspNetPager1.CurrentPageIndex = 1;
            else
                pageIndex = AspNetPager1.CurrentPageIndex;

            string strWhare = GetSqlWhereClause();
            int recordCount = 0;

            DataSet loansList = null;
            try
            {
                loansList = emailLogManager.GetListForGridView(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gridList.DataSource = loansList;
            gridList.DataBind();
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
            BindGrid();
        }

        /// <summary>
        /// Get filter
        /// </summary>
        /// <returns></returns>
        private string GetSqlWhereClause()
        {
            string strWhere = "";

            switch (strFrom)
            {
                case "0":   // processing loan detail
                    goto case "2";
                case "1":   // prospect detail
                    if (!string.IsNullOrEmpty(strItemId))
                        strWhere = string.Format(" AND ProspectId='{0}' ", strItemId);
                    else
                        goto default;
                    break;
                case "2":   // prospect loan detail
                    if (!string.IsNullOrEmpty(strItemId))
                        strWhere = string.Format(" AND FileId='{0}' ", strItemId);
                    else
                        goto default;
                    break;
                case "3":   // prospect loan detail
                    if (!string.IsNullOrEmpty(strItemId))
                        strWhere = string.Format(" AND ToContact='{0}' ", strItemId);
                    else
                        goto default;
                    break;
                default:
                    strWhere = " AND 1=0 ";
                    break;
            }

            if (!string.IsNullOrEmpty(this.tbSentStart.Text))
            {
                DateTime dt = new DateTime();
                DateTime dtNew = new DateTime();
                try
                {
                    if (DateTime.TryParse(this.tbSentStart.Text, out dt))
                    {
                        dtNew = new DateTime(dt.Year, dt.Month, dt.Day);
                        strWhere += string.Format(" AND [LastSent] >= '{0}' ", dtNew.ToString());
                    }
                }
                catch
                { }
            }

            if (!string.IsNullOrEmpty(this.tbSentEnd.Text))
            {
                DateTime dt = new DateTime();
                DateTime dtNew = new DateTime();
                try
                {
                    if (DateTime.TryParse(this.tbSentEnd.Text, out dt))
                    {
                        dtNew = dt.AddDays(1).AddMilliseconds(-1);
                        strWhere += string.Format(" AND [LastSent] <= '{0}' ", dtNew.ToString());
                    }
                }
                catch
                { }
            }

            return strWhere;
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
                    ViewState["orderName"] = "LastSent";
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
                    ViewState["orderType"] = 1;
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
            }

            // set ranking icon
            Literal litBody = e.Row.FindControl("litBody") as Literal;
            if (null != litBody)
            {
                if (gv.DataKeys[e.Row.RowIndex]["EmailBody"].ToString() == string.Empty)
                {
                    litBody.Text = string.Empty;
                }
                else
                {
                    byte[] bytes = null;
                    try
                    {
                        bytes = (byte[])gv.DataKeys[e.Row.RowIndex]["EmailBody"];
                    }
                    catch
                    { }
                    string strTemp = new String(Encoding.UTF8.GetChars(bytes));
                    var reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
                    strTemp = reg.Replace(strTemp, "");
                    if (strTemp.Length > 50)
                        litBody.Text = strTemp.Substring(0, 50) + "...";
                    else
                        litBody.Text = strTemp;
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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }
    }
}
