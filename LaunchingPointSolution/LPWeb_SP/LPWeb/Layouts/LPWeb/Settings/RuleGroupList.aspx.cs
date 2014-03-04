using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using LPWeb.Layouts.LPWeb.Common;
using Utilities;
using LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Settings
{
    /// <summary>
    /// Rule Group List page
    /// Author: Peter
    /// Date: 2011-01-08
    /// </summary>
    public partial class RuleGroupList : BasePage
    {
        BLL.Template_RuleGroups ruleGroupManager = new BLL.Template_RuleGroups();
        private bool isReset = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //权限验证
                var loginUser = new LoginUser();
                if (loginUser.userRole.AlertRuleTempl.ToString() == "")
                {
                    Response.Redirect("../Unauthorize.aspx");
                    return;
                }
                else
                {
                    if (loginUser.userRole.AlertRuleTempl.ToString().IndexOf('1') == -1)
                    {
                        lbtnCreate.Enabled = false;
                    }
                    if (loginUser.userRole.AlertRuleTempl.ToString().IndexOf('2') == -1)
                    {
                        lbtnDisable.Enabled = false;
                        lbtnUpdate.Enabled = false;
                    }
                    if (loginUser.userRole.AlertRuleTempl.ToString().IndexOf('3') == -1)
                    {
                        lbtnDelete.Enabled = false;
                    }
                }
                BindGrid();
            }
        }

        /// <summary>
        /// Bind rule group gridview
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
                listData = ruleGroupManager.GetListForGridView(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
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
            if (this.ddlAlphabet.SelectedIndex > 0)
            {
                if (strWhere.Length > 0)
                {
                    strWhere = string.Format("{0} AND Name LIKE '{1}%'", strWhere, this.ddlAlphabet.SelectedValue);
                }
                else
                {
                    strWhere = string.Format("AND Name LIKE '{0}%'", ddlAlphabet.SelectedValue);
                }
            }
            return strWhere;
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
            BindGrid();
        }

        protected void lbtnDisable_Click(object sender, EventArgs e)
        {
            if (this.hiCheckedIds.Value.Length > 0)
            {
                List<int> listIds = new List<int>();
                Array arr = this.hiCheckedIds.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in arr)
                {
                    int nTemp = -1;
                    if (!int.TryParse(str, out nTemp))
                        nTemp = -1;
                    if (nTemp != -1)
                        listIds.Add(nTemp);
                }
                if (listIds.Count > 0)
                {
                    try
                    {
                        ruleGroupManager.DisableRuleGroupInfo(listIds);
                        BindGrid();
                    }
                    catch (Exception ex)
                    {
                        ClientFun(this.updatePanel, "failedtodisableet", "alert('Failed to disable the selected email template(s), please try it again.');");
                        LPLog.LogMessage(LogType.Logerror, "Failed to disable the selected email template(s): " + ex.Message);
                        return;
                    }
                }
            }
        }

        protected void lbtnEnable_Click(object sender, EventArgs e)
        {
            if (this.hiCheckedIds.Value.Length > 0)
            {
                List<int> listIds = new List<int>();
                Array arr = this.hiCheckedIds.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in arr)
                {
                    int nTemp = -1;
                    if (!int.TryParse(str, out nTemp))
                        nTemp = -1;
                    if (nTemp != -1)
                        listIds.Add(nTemp);
                }
                if (listIds.Count > 0)
                {
                    try
                    {
                        ruleGroupManager.EnableRuleGroupInfo(listIds);
                        BindGrid();
                    }
                    catch (Exception ex)
                    {
                        ClientFun(this.updatePanel, "failedtoenableet", "alert('Failed to enable the selected email template(s), please try it again.');");
                        LPLog.LogMessage(LogType.Logerror, "Failed to enable the selected email template(s): " + ex.Message);
                        return;
                    }
                }
            }
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            if (this.hiCheckedIds.Value.Length > 0)
            {
                List<int> listIds = new List<int>();
                Array arr = this.hiCheckedIds.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in arr)
                {
                    int nTemp = -1;
                    if (!int.TryParse(str, out nTemp))
                        nTemp = -1;
                    if (nTemp != -1)
                        listIds.Add(nTemp);
                }
                if (listIds.Count > 0)
                {
                    try
                    {
                        ruleGroupManager.DeleteRuleGroupInfo(listIds);
                        BindGrid();
                    }
                    catch (Exception ex)
                    {
                        ClientFun(this.updatePanel, "failedtodeleterecord", "alert('Failed to delete the selected email template(s), please try it again.');");
                        LPLog.LogMessage(LogType.Logerror, "Failed to delete the selected user account(s): " + ex.Message);
                        return;
                    }
                }
            }
        }

        protected void lbtnEmpty_Click(object sender, EventArgs e)
        {
            isReset = false;
            BindGrid();
        }

        protected void lbtnEmptyReset_Click(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }

        StringBuilder sbAllIds = new StringBuilder();
        StringBuilder sbReferenced = new StringBuilder();
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
                string strR = gv.DataKeys[e.Row.RowIndex]["Referenced"].ToString();
                int nR = 0;
                if (!int.TryParse(strR, out nR))
                    nR = 0;

                if (sbAllIds.Length > 0)
                    sbAllIds.Append(",");
                sbAllIds.AppendFormat("{0}", strID);

                if (sbReferenced.Length > 0)
                    sbReferenced.Append(";");
                sbReferenced.AppendFormat("{0}:{1}", strID, nR);

                CheckBox ckb = e.Row.FindControl("ckbSelect") as CheckBox;
                if (null != ckb)
                {
                    ckb.Attributes.Add("onclick", string.Format("CheckBoxClicked(this, '{0}', '{1}', '{2}', '{3}');",
                        strCkAllID, this.hiAllIds.ClientID, this.hiCheckedIds.ClientID, strID));
                }

                // reset description text
                Label lblDesc = e.Row.FindControl("lblDesc") as Label;
                if (null != lblDesc)
                {
                    if (lblDesc.Text.Trim().Length > 50)
                    {
                        lblDesc.ToolTip = lblDesc.Text.Trim();
                        lblDesc.Text = lblDesc.Text.Trim().Substring(0, 50) + "...";
                    }
                }
            }
        }

        protected void gridList_PreRender(object sender, EventArgs e)
        {
            this.hiAllIds.Value = sbAllIds.ToString();
            this.hiCheckedIds.Value = "";
            this.hiReferenced.Value = sbReferenced.ToString();
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
                    ViewState["orderName"] = "Name";
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

        protected void ddlAlphabet_SelectedIndexChanged(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }

        /// <summary>
        /// call client function
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="strKey"></param>
        /// <param name="strScript"></param>
        private void ClientFun(Control ctl, string strKey, string strScript)
        {
            ScriptManager.RegisterStartupScript(ctl, this.GetType(), strKey, strScript, true);
        }
    }
}
