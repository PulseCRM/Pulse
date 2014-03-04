using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using Utilities;
using LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Settings
{
    /// <summary>
    /// Email template list
    /// author: peter pan
    /// date: 2010-12-10
    /// </summary>
    public partial class EmailTemplateList : BasePage
    {
        BLL.Template_Email EmailTempManager = new BLL.Template_Email();
        private bool isReset = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //权限验证
                var loginUser = new LoginUser();
                if (!loginUser.userRole.CompanySetup)
                {
                    Response.Redirect("../Unauthorize.aspx");
                    return;
                }
                BindGrid();

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
                listData = EmailTempManager.GetListForGridView(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
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

            // from EmailSkinList
            // EmailSkinID
            // get email templates refence EmailSkinID
            if (this.Request.QueryString["EmailSkinID"] != null)
            {
                string sEmailSkinID = this.Request.QueryString["EmailSkinID"];

                strWhere += " and TemplEmailId in (select TemplEmailId from Template_Email where EmailSkinId=" + sEmailSkinID + ")";
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
            List<int> listIDs = new List<int>();

            // Get email template of current selected row
            foreach (GridViewRow row in gridList.Rows)
            {
                if (DataControlRowType.DataRow == row.RowType)
                {
                    CheckBox ckbChecked = row.FindControl("ckbSelected") as CheckBox;
                    if (null != ckbChecked && ckbChecked.Checked)
                        listIDs.Add((int)gridList.DataKeys[row.RowIndex].Value);
                }
            }
            if (listIDs.Count > 0)
            {
                try
                {
                    EmailTempManager.SetEmailTemplateDisabled(listIDs);
                }
                catch (Exception ex)
                {
                    ClientFun(this.updatePanel, "failedtodisableet", "alert('Failed to disable the selected email template(s), please try it again.');");
                    LPLog.LogMessage(LogType.Logerror, "Failed to disable the selected email template(s): " + ex.Message);
                    return;
                }
                BindGrid();
            }
        }

        protected void lbtnEnable_Click(object sender, EventArgs e)
        {
            List<int> listIDs = new List<int>();

            // Get email template of current selected row
            foreach (GridViewRow row in gridList.Rows)
            {
                if (DataControlRowType.DataRow == row.RowType)
                {
                    CheckBox ckbChecked = row.FindControl("ckbSelected") as CheckBox;
                    if (null != ckbChecked && ckbChecked.Checked)
                        listIDs.Add((int)gridList.DataKeys[row.RowIndex].Value);
                }
            }
            if (listIDs.Count > 0)
            {
                try
                {
                    EmailTempManager.SetEmailTemplateEnabled(listIDs);
                }
                catch (Exception ex)
                {
                    ClientFun(this.updatePanel, "failedtoenableet", "alert('Failed to enable the selected email template(s), please try it again.');");
                    LPLog.LogMessage(LogType.Logerror, "Failed to enable the selected email template(s): " + ex.Message);
                    return;
                }
                BindGrid();
            }
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            List<int> listIDs = new List<int>();

            // Get userid of current selected row
            foreach (GridViewRow row in gridList.Rows)
            {
                if (DataControlRowType.DataRow == row.RowType)
                {
                    CheckBox ckbChecked = row.FindControl("ckbSelected") as CheckBox;
                    if (null != ckbChecked && ckbChecked.Checked)
                        listIDs.Add((int)gridList.DataKeys[row.RowIndex].Value);
                }
            }
            if (listIDs.Count > 0)
            {
                try
                {
                    EmailTempManager.DeleteEmailTemplates(listIDs);
                }
                catch (Exception ex)
                {
                    ClientFun(this.updatePanel, "failedtodeleterecord", "alert('Failed to delete the selected email template(s), please try it again.');");
                    LPLog.LogMessage(LogType.Logerror, "Failed to delete the selected user account(s): " + ex.Message);
                    return;
                }
                BindGrid();
            }
        }

        protected void lbtnEmpty_Click(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }

        protected void lbtnEmpty2_Click(object sender, EventArgs e)
        {
            isReset = false;
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
            if (DataControlRowType.DataRow == e.Row.RowType)
            {
                CheckBox ckbChecked = e.Row.FindControl("ckbSelected") as CheckBox;
                int nEmailTempID = 0;
                if (null != gridList.DataKeys[e.Row.RowIndex])
                {
                    if (!int.TryParse(gridList.DataKeys[e.Row.RowIndex].Value.ToString(), out nEmailTempID))
                        nEmailTempID = 0;

                    if (0 != nEmailTempID)
                    {
                        if (sbAllIds.Length > 0)
                            sbAllIds.Append(",");
                        sbAllIds.AppendFormat("{0}", nEmailTempID);

                        string strR = EmailTempManager.bIsRef(nEmailTempID) ? "1" : "0";
                        int nR = 0;
                        if (!int.TryParse(strR, out nR))
                            nR = 0;

                        if (sbReferenced.Length > 0)
                            sbReferenced.Append(";");
                        sbReferenced.AppendFormat("{0}:{1}", nEmailTempID, nR);

                        if (null != ckbChecked)
                        {
                            ckbChecked.Attributes.Add("onclick", string.Format("CheckBoxClicked(this, '{0}', '{1}', '{2}');",
                                this.hiAllIds.ClientID, this.hiCheckedIds.ClientID, nEmailTempID));
                        }
                    }
                }

                // reset description text
                Label lblDesc = e.Row.FindControl("lblDesc") as Label;
                if (null != lblDesc)
                {
                    if (lblDesc.Text.Length > 50)
                    {
                        lblDesc.ToolTip = lblDesc.Text;
                        lblDesc.Text = lblDesc.Text.Substring(0, 50) + "...";
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
