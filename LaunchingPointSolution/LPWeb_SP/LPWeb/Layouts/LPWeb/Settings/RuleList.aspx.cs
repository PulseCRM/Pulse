using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Web.UI.WebControls;
using System.Data;
using Utilities;
using LPWeb.Common;
using LPWeb.BLL;
using LPWeb.Model;

namespace LPWeb.Settings
{
    public partial class Settings_RuleList : BasePage
    {
        #region Parameters


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

        private BLL.Template_Rules template = new BLL.Template_Rules();
        public string FromURL = string.Empty;
        private string sDbTable = "(SELECT Template_Rules.*, Template_Email.Name AS EmailTemplateName, CASE RuleScope WHEN 0 THEN 'Loan' WHEN 1 THEN 'Company' WHEN 2 THEN 'Region' WHEN 3 THEN 'Division' WHEN 4 THEN 'Branch' ELSE '' END AS ScopeName, (SELECT STUFF((SELECT DISTINCT ', ' + LoanTargets.LT FROM (SELECT CASE WHEN Template_Rules.LoanTarget=" + Template_Rules_LoanTarget._constOldProcessing + " OR Template_Rules.LoanTarget=" + Template_Rules_LoanTarget._constOldProcessingAndProspect + " THEN 'Active Loans' END AS LT UNION ALL SELECT CASE WHEN Template_Rules.LoanTarget=" + Template_Rules_LoanTarget._constOldProspect + " OR Template_Rules.LoanTarget=" + Template_Rules_LoanTarget._constOldProcessingAndProspect + " THEN 'Active Leads' END AS LT UNION ALL SELECT CASE WHEN Template_Rules.LoanTarget & " + Template_Rules_LoanTarget._constActiveLoans + " = " + Template_Rules_LoanTarget._constActiveLoans + " THEN 'Active Loans' END AS LT UNION ALL SELECT CASE WHEN Template_Rules.LoanTarget & " + Template_Rules_LoanTarget._constActiveLeads + " = " + Template_Rules_LoanTarget._constActiveLeads + " THEN 'Active Leads' END AS LT UNION ALL SELECT CASE WHEN Template_Rules.LoanTarget & " + Template_Rules_LoanTarget._constArchivedLoans + " = " + Template_Rules_LoanTarget._constArchivedLoans + " THEN 'Archived Loans' END AS LT UNION ALL SELECT CASE WHEN Template_Rules.LoanTarget & " + Template_Rules_LoanTarget._constArchivedLeads + " = " + Template_Rules_LoanTarget._constArchivedLeads + " THEN 'Archived Leads' END AS LT) AS LoanTargets FOR XML PATH('')), 1, 2, '')) AS TargetName FROM Template_Rules LEFT OUTER JOIN Template_Email ON Template_Rules.AlertEmailTemplId=Template_Email.TemplEmailId) t";
        #endregion 


        #region Event
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Url != null)
                {
                    FromURL = Request.Url.ToString();
                }

                if (this.IsPostBack == false)
                {
                    this.RuleSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;
                    this.RuleSqlDataSource.SelectParameters["DbTable"].DefaultValue = this.sDbTable;

                    //LoginUser loginUser = new LoginUser();
                    //loginUser.ValidatePageVisitPermission("RuleList");//页面访问权限验证
                    //权限验证
                    var loginUser = new LoginUser();
                    if (loginUser.userRole.AlertRules.ToString() == "")
                    {
                        Response.Redirect("../Unauthorize.aspx");
                        return;
                    }
                    else
                    {
                        hdnAlertRuleTempl.Value = loginUser.userRole.AlertRules.ToString();

                        if (loginUser.userRole.AlertRules.ToString().IndexOf('2') == -1)
                        {
                            btnDisable.Enabled = false;
                        }
                        if (loginUser.userRole.AlertRules.ToString().IndexOf('3') == -1)
                        {
                            btnDelete.Enabled = false;
                        }
                    }

                    this.ddlRuleTarget.Items.Add(new ListItem("Active Loans", Template_Rules_LoanTarget._constActiveLoans.ToString()));
                    this.ddlRuleTarget.Items.Add(new ListItem("Active Leads", Template_Rules_LoanTarget._constActiveLeads.ToString()));
                    this.ddlRuleTarget.Items.Add(new ListItem("Archived Loans", Template_Rules_LoanTarget._constArchivedLoans.ToString()));
                    this.ddlRuleTarget.Items.Add(new ListItem("Archived Leads", Template_Rules_LoanTarget._constArchivedLeads.ToString()));

                    this.BindingGrid();
                }
            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDisable_Click(object sender, EventArgs e)
        {
            string sRuleIDs = this.hdnRuleIDs.Value;
            if (sRuleIDs.Length > 0)
            {

                try
                {
                    this.template.DisableRuleTemplates(sRuleIDs);
                    this.BindingGrid();
                }
                catch (Exception ex)
                {
                    LPLog.LogMessage(LogType.Logdebug, ex.ToString());
                }
            }
            this.hdnRuleIDs.Value = string.Empty;
        }

        protected void btnEnable_Click(object sender, EventArgs e)
        {
            string sRuleIDs = this.hdnRuleIDs.Value;
            if (sRuleIDs.Length > 0)
            {

                try
                {
                    this.template.EnableRuleTemplates(sRuleIDs);
                    this.BindingGrid();
                }
                catch (Exception ex)
                {
                    LPLog.LogMessage(LogType.Logdebug, ex.ToString());
                }
            }
            this.hdnRuleIDs.Value = string.Empty;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string sRuleIDs = this.hdnRuleIDs.Value;
            if (sRuleIDs.Length > 0)
            {
                try
                {
                    sRuleIDs = "'" + sRuleIDs.Replace(",", "','") + "'";
                    template.DeleteRuleTemplate(sRuleIDs);
                    BindingGrid();
                }
                catch (Exception ex)
                {
                    LPLog.LogMessage(LogType.Logdebug, ex.ToString());
                }
            }
            this.hdnRuleIDs.Value = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRuleList_Sorting(object sender, GridViewSortEventArgs e)
        {
            OrderName = e.SortExpression;
            string sortExpression = e.SortExpression;
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
            BindingGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindingGrid();
        }
        #endregion

        #region function

        /// <summary>
        /// 
        /// </summary>
        private void BindingGrid()
        {
            try
            {
                this.RuleSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;
                this.RuleSqlDataSource.SelectParameters["DbTable"].DefaultValue = this.sDbTable;

                string sWhere = "";
                bool bSetWhere = false;

                //Alphabet
                if (this.Request.QueryString["Alphabet"] != null && this.Request.QueryString["Alphabet"].ToString() != "")
                {
                    sWhere += " AND (Name LIKE '" + this.Request.QueryString["Alphabet"].ToString() + "%')";
                    bSetWhere = true;
                }

                if (this.ddlRuleScope.SelectedValue.ToString() != "")
                {
                    sWhere += " AND (RuleScope=" + this.ddlRuleScope.SelectedValue.ToString() + ")";
                    bSetWhere = true;
                }

                if (this.ddlRuleTarget.SelectedValue.ToString() != "")
                {
                    if (this.ddlRuleTarget.SelectedValue == Template_Rules_LoanTarget._constActiveLoans.ToString())
                    {
                        sWhere += " AND (LoanTarget=" + Template_Rules_LoanTarget._constOldProcessing + " OR LoanTarget=" + Template_Rules_LoanTarget._constOldProcessingAndProspect + " OR LoanTarget & " + Template_Rules_LoanTarget._constActiveLoans + "=" + Template_Rules_LoanTarget._constActiveLoans + ")";
                        bSetWhere = true;
                    }
                    else if (this.ddlRuleTarget.SelectedValue == Template_Rules_LoanTarget._constActiveLeads.ToString())
                    {
                        sWhere += " AND (LoanTarget=" + Template_Rules_LoanTarget._constOldProspect + " OR LoanTarget=" + Template_Rules_LoanTarget._constOldProcessingAndProspect + " OR LoanTarget & " + Template_Rules_LoanTarget._constActiveLeads + "=" + Template_Rules_LoanTarget._constActiveLeads + ")";
                        bSetWhere = true;
                    }
                    else if (this.ddlRuleTarget.SelectedValue == Template_Rules_LoanTarget._constArchivedLoans.ToString())
                    {
                        sWhere += " AND (LoanTarget & " + Template_Rules_LoanTarget._constArchivedLoans + "=" + Template_Rules_LoanTarget._constArchivedLoans + ")";
                        bSetWhere = true;
                    }
                    else if (this.ddlRuleTarget.SelectedValue == Template_Rules_LoanTarget._constArchivedLeads.ToString())
                    {
                        sWhere += " AND (LoanTarget & " + Template_Rules_LoanTarget._constArchivedLeads + "=" + Template_Rules_LoanTarget._constArchivedLeads + ")";
                        bSetWhere = true;
                    }
                }

                // Get data number
                int iCount = DAL.DbHelperSQL.Count(this.RuleSqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);
                this.AspNetPager1.RecordCount = iCount;

                // No data
                if (bSetWhere == true && iCount == 0)
                {
                    //set message
                    this.gridRuleList.EmptyDataText = "There is no rule template by your conditions.";
                }
                else
                {
                    //data bing
                    this.RuleSqlDataSource.SelectParameters["Where"].DefaultValue = sWhere;
                    this.gridRuleList.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
