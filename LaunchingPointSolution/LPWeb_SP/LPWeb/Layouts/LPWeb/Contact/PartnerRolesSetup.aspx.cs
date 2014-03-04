using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Web.UI.WebControls;
using System.Data;
using Utilities;
using System.Text;
using LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Contact
{
    /// <summary>
    /// contact roles Setup Page
    /// Peter
    /// 2011-04-11
    /// </summary>
    public partial class PartnerRolesSetup : BasePage
    {
        private bool isReset = false;
        BLL.ContactRoles contactRolesMngr = new BLL.ContactRoles();
        protected string sHasCreate = "0";
        protected string sHasModify = "0";
        protected string sHasDelete = "0";
        protected string sHasDisable = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrUser.userRole.ContactRole.ToString().IndexOf('1') == -1)
            {
                Response.Redirect("../Unauthorize.aspx");  // have not View Power
                return;
            }

            sHasCreate = CurrUser.userRole.ContactRole.ToString().IndexOf('2') > -1 ? "1" : "0";
            sHasModify = CurrUser.userRole.ContactRole.ToString().IndexOf('3') > -1 ? "1" : "0";
            sHasDelete = CurrUser.userRole.ContactRole.ToString().IndexOf('4') > -1 ? "1" : "0";
            sHasDisable = CurrUser.userRole.ContactRole.ToString().IndexOf('5') > -1 ? "1" : "0";

            if (sHasCreate == "0")
            {
                btnAdd.Enabled = false;
            }
            if (sHasModify == "0")
            {
                lbtnEnable.Enabled = false;
            }
            if (sHasDelete == "0")
            {
                lbtnRemove.Enabled = false;
            }
            if (sHasDisable == "0")
            {
                lbtnDisable.Enabled = false;
            }

            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        /// <summary>
        /// Get filter
        /// </summary>
        /// <returns></returns>
        private string GetSqlWhereClause()
        {
            string strWhere = " AND Name<>'Borrower' AND Name<>'CoBorrower'";
            if (this.ddlAlphabet.SelectedIndex > 0)
            {
                strWhere = string.Format("{0} AND Name LIKE '{1}%'", strWhere, this.ddlAlphabet.SelectedValue);
            }
            return strWhere;
        }

        /// <summary>
        /// Bind contact role gridview
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

            DataSet contactRoles = null;
            try
            {
                contactRoles = contactRolesMngr.GetListForGridView(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            isReset = true;
            try
            {
                if (string.IsNullOrEmpty(this.tbRoleName.Text.Trim()))
                {
                    PageCommon.AlertMsg(this, "The contact role cannot be blank.");
                    return;
                }
                if (!contactRolesMngr.IsContactRoleNameExsits(this.tbRoleName.Text.Trim()))
                {
                    Model.ContactRoles cRole = new Model.ContactRoles();
                    cRole.Name = this.tbRoleName.Text.Trim();
                    cRole.Enabled = true;
                    contactRolesMngr.Add(cRole);
                    this.tbRoleName.Text = "";

                    BindGrid();
                    PageCommon.AlertMsg(this, "Add contact role successfully.");
                }
                else
                {
                    PageCommon.AlertMsg(this, "The contact role already exists.");
                }
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to add the contact role, error:"+ex.Message);
                LPLog.LogMessage(LogType.Logerror, "Failed to add contact role, exception: " + ex.Message);
            }
        }

        protected void ddlAlphabet_SelectedIndexChanged(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }

        protected void lbtnRemove_Click(object sender, EventArgs e)
        {
            isReset = true;
            try
            {
                contactRolesMngr.DeleteContactRole(this.hiCheckedIds.Value);
            }
            catch(Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to remove the selected contact role(s).");
                LPLog.LogMessage(LogType.Logerror, "Failed to remove the selected contact role(s), exception: " + ex.Message);
            }
            BindGrid();
        }

        protected void lbtnEnable_Click(object sender, EventArgs e)
        {
            isReset = false;
            try
            {
                contactRolesMngr.EnableContactRole(this.hiCheckedIds.Value);
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to enable the selected contact role(s)");
                LPLog.LogMessage(LogType.Logerror, "Failed to enable the selected contact role(s), exception: " + ex.Message);
            }
            BindGrid();
        }

        protected void lbtnDisable_Click(object sender, EventArgs e)
        {
            isReset = false;
            try
            {
                contactRolesMngr.DisableContactRole(this.hiCheckedIds.Value);
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to disable the selected contact role(s).");
                LPLog.LogMessage(LogType.Logerror, "Failed to disable the selected contact role(s), exception: " + ex.Message);
            }
            BindGrid();
        }
    }
}
