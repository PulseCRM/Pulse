using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Workflow;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.DAL;
using Utilities;
using System.Text;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    /// <summary>
    /// Handle Point Errors list page
    /// Author: Peter
    /// Date: 2010-11-12
    /// </summary>
    public partial class HandlePointErrors : BasePage
    {
        BLL.PointImportHistory PIHManager = new BLL.PointImportHistory();
        private bool isReset = false;
        private string fromHomeFilter = string.Empty;
        string queryCon_2 = string.Empty;
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
                    ViewState["orderName"] = "Borrower";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BLL.Roles bllRoles = new BLL.Roles();

                var dsRoles = bllRoles.GetList(" RoleId=" + CurrUser.iRoleID);
                if (dsRoles != null && dsRoles.Tables.Count != 0 && dsRoles.Tables[0].Rows.Count > 0)
                {
                    if (dsRoles.Tables[0].Rows[0]["OtherLoanAccess"] == null || dsRoles.Tables[0].Rows[0]["OtherLoanAccess"].ToString().Trim().Length == 0)
                    {
                        Response.Redirect("../Unauthorize.aspx");
                        return;
                    }
                }
                BindFilter();
                fromHomeFilter = FilterFromHomePiplineSummary();
                BindGrid();
            }
        }

        /// <summary>
        /// Filters from home pipline summary.
        /// </summary>
        /// <returns></returns>
        private string FilterFromHomePiplineSummary()
        {
            string searchCondition = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["q"]))
            {
                string qString = Encrypter.Base64Decode(Request.QueryString["q"]);
                string[] args = qString.Split("&".ToCharArray());

                string searchTempllage = " AND {0} = '{1}'";

                Dictionary<string, string> KVPs = new Dictionary<string, string>();
                try
                {
                    KVPs = (from arg in args.Where(s => !string.IsNullOrEmpty(s)).ToList()
                            select new { key = arg.Split("=".ToCharArray())[0], value = arg.Split("=".ToCharArray())[1] }).
                        ToDictionary(p => p.key, p => p.value);
                }
                catch (Exception e)
                {
                    LPLog.LogMessage(e.Message);
                    return string.Empty;
                }

                if (KVPs.ContainsKey("RegionID") && !string.IsNullOrEmpty(KVPs["RegionID"]))
                {
                    //searchCondition += string.Format(searchTempllage, "RegionID", KVPs["RegionID"]);
                    this.ddlRegion.SelectedValue = KVPs["RegionID"];
                }

                if (KVPs.ContainsKey("DivisionID") && !string.IsNullOrEmpty(KVPs["DivisionID"]))
                {
                    //searchCondition += string.Format(searchTempllage, "DivisionID", KVPs["DivisionID"]);
                    this.ddlDivision.SelectedValue = KVPs["DivisionID"];
                }

                if (KVPs.ContainsKey("BranchID") && !string.IsNullOrEmpty(KVPs["BranchID"]))
                {
                    //searchCondition += string.Format(searchTempllage, "BranchID", KVPs["BranchID"]);
                    this.ddlBranch.SelectedValue = KVPs["BranchID"];
                }

                if (KVPs.ContainsKey("DateType") && !string.IsNullOrEmpty(KVPs["DateType"]))
                {
                    //string dateTemplage = "AND {0} >= '{1}' AND {0} <= '{2}'";
                    string dateType = string.Empty;
                    if (KVPs["DateType"].Equals("CloseDate", StringComparison.CurrentCultureIgnoreCase))
                    {
                        dateType = "DateClose";
                    }
                    else
                    {
                        dateType = "DateOpen";
                    }

                    if (KVPs.ContainsKey("FromDate") && !string.IsNullOrEmpty(KVPs["FromDate"]))
                    {
                        searchCondition += string.Format(" AND {0} >= '{1}'", dateType, KVPs["FromDate"]);
                    }

                    if (KVPs.ContainsKey("ToDate") && !string.IsNullOrEmpty(KVPs["ToDate"]))
                    {
                        searchCondition += string.Format(" AND {0} <= '{1}'", dateType, KVPs["ToDate"]);
                    }
                }

                if (KVPs.ContainsKey("Status") && !string.IsNullOrEmpty(KVPs["Status"]))
                {
                    searchCondition += string.Format(searchTempllage, "Status", KVPs["Status"]);
                }

                if (KVPs.ContainsKey("LastCompletedStage") && !string.IsNullOrEmpty(KVPs["LastCompletedStage"]))
                {
                    searchCondition += string.Format(searchTempllage, "LastCompletedStage", KVPs["LastCompletedStage"]);
                }
            }
            return searchCondition;
        }

        private void BindFilter()
        {
            BindRegions(CurrUser);

            BindDivisions(CurrUser, "0");

            BindBranches(CurrUser, "0", "0");
        }

        /// <summary>
        /// 根据用户界面选择生成过滤条件
        /// </summary>
        /// <returns></returns>
        private string GenerateQueryCondition()
        {
            //string queryCon = " Cleared IS NULL ";
            string queryCon = " 1=1 ";

            //alphabets 
            if (!string.IsNullOrEmpty(ddlAlphabet.SelectedValue))
            {
                queryCon_2 = string.Format(" AND [Borrower] Like '{0}%'", ddlAlphabet.SelectedValue);
            }
            else
            {
                queryCon_2 = string.Empty;
            }

            string groupIdQueryPart = GenOrgQueryCondition();
            if (!string.IsNullOrEmpty(groupIdQueryPart))
                queryCon += groupIdQueryPart;

            //来自lin邮件的描述: 如果用户没有"Access others loans"的权限, 则整个筛选必须包括LoanTeam.UserId和当前用户的匹配。

            if (!CurrUser.userRole.OtherLoanAccess)
            {
                //queryCon += " AND UserID = " + CurrUser.iUserID;
                queryCon += string.Format(" AND (UserID = {0} OR Owner = {0})", CurrUser.iUserID);
            }
            else
            {
                queryCon += string.Format(" OR (UserID = {0} OR Owner = {0})", CurrUser.iUserID);
            }
            
            return queryCon;
        }

        /// <summary>
        /// 根据region,division和branch选择获取所属的全部groupid
        /// </summary>
        /// <returns></returns>
        private string GenOrgQueryCondition()
        {
            /*
             筛选条件依据（来自Rocky的邮件）：
             * 目前在我们的数据库中有一个GroupFolder表；这个表有两个字段，分别是groupid和folderid，是用来记录Group与Folder关系的。
             * 当Region/Division/Branch保存时，系统会自动找到Region/Division/Branch所对应的BranchID，然后根据BranchID找到对应的FolderID，最后系统会将Region/Division/Branch对应的GroupID与找到的FolderID保存到GroupFolder表中。

                因此，我认为，你在找Alert对应的PointFolder时，可以根据Region/Division/Branch对应的GroupID，查找关联的Point Folder，再根据Point Folder查找对应的Loan File。
                例如：1. Region A  关联Group A,  Division A 关联Group B, Branch A关联Group C。当在Alert中选择的Region A时，查找Group A对应的所有Point Folder下的所有Loan alert； 如果选择Division A，选择Group B对应的所有Point Folder……
             */
            var bllGroup = new Groups();
            string queryCon = " 1=1 ";
            if (ddlBranch.SelectedValue != "-1")
            {
                queryCon += string.Format(" AND BranchID={0}", ddlBranch.SelectedValue);
            }
            else if (ddlDivision.SelectedValue != "-1")
            {
                queryCon += string.Format(" AND DivisionID={0}", ddlDivision.SelectedValue);
            }
            else if (ddlRegion.SelectedValue != "-1")
            {
                queryCon += string.Format(" AND RegionId={0}", ddlRegion.SelectedValue);
            }
            else//All,All,All的情况，这时是初始化用户还没有选择的情况，应该从region-->Division-->Branch检查(从上向下)，哪个dropdownlist有值，就加上这个限制查询条件
            {
                var condition = "";
                var template = " {2} {0} IN ({1})";
                bool hasCon = false;

                //region
                if (ddlRegion.Items.Count > 1)
                {
                    var aggregateIds = AggregateIds(ddlRegion.Items);
                    condition = string.Format(template, "RegionId", aggregateIds, "AND");
                    hasCon = true;
                }

                //division
                if (!hasCon)
                {
                    if (ddlDivision.Items.Count > 1)
                    {
                        var aggregateIds = AggregateIds(ddlDivision.Items);
                        condition += string.Format(template, "DivisionId", aggregateIds, "AND");
                        hasCon = true;
                    }
                }
                else
                {
                    if (ddlDivision.Items.Count > 1)
                    {
                        var aggregateIds = AggregateIds(ddlDivision.Items);
                        condition += string.Format(template, "DivisionId", aggregateIds, "OR");
                        hasCon = true;
                    }
                }

                //branch
                if (!hasCon)
                {
                    if (ddlBranch.Items.Count > 1)
                    {
                        var aggregateIds = AggregateIds(ddlBranch.Items);
                        condition += string.Format(template, "BranchId", aggregateIds, "AND");
                        hasCon = true;
                    }
                }
                else
                {
                    if (ddlBranch.Items.Count > 1)
                    {
                        var aggregateIds = AggregateIds(ddlBranch.Items);
                        condition += string.Format(template, "BranchId", aggregateIds, "OR");
                        hasCon = true;
                    }
                }

                if (hasCon)
                {
                    queryCon += condition;
                }
            }

            DataSet dsGroups = bllGroup.GetList(queryCon);
            if (dsGroups == null || dsGroups.Tables.Count == 0 || dsGroups.Tables[0].Rows.Count == 0)
                return " AND 1<0 ";//todo:如果没有筛选出group，那么整个查询则是无效的

            //根据组织结构查询出所属的所有groupid，并用‘，’拼接成字符串
            string groupIds =
                dsGroups.Tables[0].AsEnumerable().Select(groupFolder => groupFolder.Field<int>("GroupId").ToString()).
                    Aggregate((ids, next) => ids + "," + next);

            return string.Format(" AND GroupID IN ({0})", groupIds);
        }

        /// <summary>
        /// 用','拼接下拉选择项中所有项的值（除了"ALL ...."的选项）
        /// </summary>
        /// <param name="items">下拉框所有选项</param>
        /// <returns></returns>
        private string AggregateIds(ListItemCollection items)
        {
            var aggregateIds = string.Empty;
            foreach (ListItem item in items)
            {
                if (item.Value.Trim() == "-1")
                {
                    continue;
                }

                aggregateIds += string.Format("{0},", item.Value.Trim());
            }

            return aggregateIds.TrimEnd(',');
        }

        private void BindRegions(LoginUser curLoginUser)
        {
            BLL.Regions RegionManager = new BLL.Regions();
            DataTable RegionListData = null;

            if (curLoginUser.userRole.OtherLoanAccess == true)   // All Loans
            {
                RegionListData = RegionManager.GetRegionList_AllLoans(curLoginUser.iUserID);
            }
            else // Assigned Loans
            {
                RegionListData = RegionManager.GetRegionList_AssingedLoans(curLoginUser.iUserID);
            }

            // add "All Regions" row
            DataRow NewRegionRow = RegionListData.NewRow();
            NewRegionRow["RegionID"] = "-1";
            NewRegionRow["Name"] = "All Regions";
            RegionListData.Rows.InsertAt(NewRegionRow, 0);

            this.ddlRegion.DataSource = RegionListData;
            this.ddlRegion.DataBind();
        }

        private void BindDivisions(LoginUser curLoginUser, string regionId)
        {
            int iRegionID = int.Parse(regionId);

            BLL.Divisions DivisionManager = new BLL.Divisions();
            DataTable DivisionListData = null;

            if (curLoginUser.userRole.OtherLoanAccess == true)   // All Loans
            {
                if (iRegionID == 0)     // 没有region参数
                {
                    DivisionListData = DivisionManager.GetDivision_AllLoans(curLoginUser.iUserID);
                }
                else // 以region来筛选division
                {
                    DivisionListData = DivisionManager.GetDivision_AllLoans(curLoginUser.iUserID, iRegionID);
                }
            }
            else // Assigned Loans
            {
                if (iRegionID == 0)     // 没有region参数
                {
                    DivisionListData = DivisionManager.GetDivisionList_AssingedLoans(curLoginUser.iUserID);
                }
                else // 以region来筛选division
                {
                    DivisionListData = DivisionManager.GetDivisionList_AssingedLoans(curLoginUser.iUserID, iRegionID);
                }
            }

            DataRow NewDivisionRow = DivisionListData.NewRow();
            NewDivisionRow["DivisionID"] = "-1";
            NewDivisionRow["Name"] = "All Divisions";
            DivisionListData.Rows.InsertAt(NewDivisionRow, 0);

            this.ddlDivision.DataSource = DivisionListData;
            this.ddlDivision.DataBind();
        }

        private void BindBranches(LoginUser curLoginUser, string regionId, string divisionId)
        {
            int iRegionID = int.Parse(regionId);
            int iDivisionID = int.Parse(divisionId);

            iRegionID = iRegionID >= 0 ? iRegionID : 0;
            iDivisionID = iDivisionID >= 0 ? iDivisionID : 0;

            BLL.Branches BrancheManager = new BLL.Branches();
            DataTable BranchListData = null;

            if (curLoginUser.userRole.OtherLoanAccess == true)   // All Loans
            {
                BranchListData = BrancheManager.GetBranchList_AllLoans(curLoginUser.iUserID, iRegionID, iDivisionID);
            }
            else // Assigned Loans
            {
                BranchListData = BrancheManager.GetBranchList_AssingedLoans(curLoginUser.iUserID, iRegionID, iDivisionID);
            }

            DataRow NewBranchRow = BranchListData.NewRow();
            NewBranchRow["BranchID"] = "-1";
            NewBranchRow["Name"] = "All Branches";
            BranchListData.Rows.InsertAt(NewBranchRow, 0);

            this.ddlBranch.DataSource = BranchListData;
            this.ddlBranch.DataBind();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }
        
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
            BindGrid();
        }

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRegion.SelectedIndex == 0)
            {
                BindFilter();
                return;
            }
            BindDivisions(CurrUser, ddlRegion.SelectedValue);
            BindBranches(CurrUser, ddlRegion.SelectedValue, ddlDivision.SelectedValue);
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindBranches(CurrUser, ddlRegion.SelectedValue, ddlDivision.SelectedValue);
        }

        private void BindGrid()
        {
            int pageSize = AspNetPager1.PageSize;
            int pageIndex = 1;

            if (isReset == true)
                pageIndex = AspNetPager1.CurrentPageIndex = 1;
            else
                pageIndex = AspNetPager1.CurrentPageIndex;

            string queryCondition = GenerateQueryCondition();

            if (!string.IsNullOrEmpty(fromHomeFilter))
            {
                queryCondition += fromHomeFilter;
            }

            // see DAL.Loans.GetList
            string strWhere = string.Format(@"AND EXISTS (SELECT 1 FROM (SELECT pi.* FROM lpvw_PipelineInfo pi 
            INNER JOIN (SELECT DISTINCT FileId FROM lpvw_PipelineInfoGroup WHERE {0} ) tt ON pi.FileId=tt.FileId
            ) AS pinfo WHERE pinfo.FileId=lpvw_PointImportErrors.FileId)", queryCondition);

            if ("2" == ddlErrorType.SelectedValue)
            {
                strWhere += " AND lpvw_PointImportErrors.Severity='Error'";
            }
            else if ("3" == ddlErrorType.SelectedValue)
            {
                strWhere += " AND lpvw_PointImportErrors.Severity='Warning'";
            }

            if (queryCon_2 != string.Empty)
            {
                strWhere += queryCon_2;
            }

            int recordCount = 0;

            DataSet dataList = null;
            try
            {
                dataList = PIHManager.GetListForGridView(pageSize, pageIndex, strWhere, out recordCount, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gridErrors.DataSource = dataList;
            gridErrors.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the gvPipelineView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gridErrors_Sorting(object sender, GridViewSortEventArgs e)
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
            BindGrid();
        }

        StringBuilder sbAllErrorIds = new StringBuilder();
        /// <summary>
        /// Set selected row when click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridErrors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (DataControlRowType.DataRow == e.Row.RowType)
            {
                CheckBox ckbChecked = e.Row.FindControl("ckbSelected") as CheckBox;
                int nID = 0;
                if (null != gridErrors.DataKeys[e.Row.RowIndex])
                {
                    if (!int.TryParse(gridErrors.DataKeys[e.Row.RowIndex].Value.ToString(), out nID))
                        nID = 0;

                    if (0 != nID)
                    {
                        string strID = string.Format("{0}", gridErrors.DataKeys[e.Row.RowIndex].Value);
                        if (sbAllErrorIds.Length > 0)
                            sbAllErrorIds.Append(",");
                        sbAllErrorIds.AppendFormat("'{0}'", strID);

                        if (null != ckbChecked)
                        {
                            ckbChecked.Attributes.Add("onclick", string.Format("onRowChecked(this.checked, '{0}');", nID));
                        }
                    }
                }

                LinkButton lbtnError = e.Row.FindControl("lbtnError") as LinkButton;
                if (null != lbtnError)
                {
                    if (lbtnError.Text.Length > 25)
                    {
                        lbtnError.ToolTip = lbtnError.Text;
                        lbtnError.Text = lbtnError.Text.Substring(0, 25) + "...";
                    }
                }

                Image imgAlert = e.Row.FindControl("imgAlert") as Image;
                if (null != imgAlert)
                {
                    switch (imgAlert.ToolTip.ToLower())
                    {
                        case "error":
                            imgAlert.ImageUrl = "../images/loan/AlertError.png";
                            break;
                        case "warning":
                            imgAlert.ImageUrl = "../images/loan/AlertWarning.png";
                            break;
                        default:
                            imgAlert.Visible = false;
                            break;
                    }
                }
            }
        }

        protected void gridErrors_PreRender(object sender, EventArgs e)
        {
            ClientFun(this.updatePanel, "registerLOIds", string.Format("arrAllErrorIds = [{0}];arrSelectedID=[];", sbAllErrorIds.ToString()));
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            Dictionary<int, int> dicIDs = new Dictionary<int, int>();
            // Get userid of current selected row
            foreach (GridViewRow row in gridErrors.Rows)
            {
                if (DataControlRowType.DataRow == row.RowType)
                {
                    CheckBox ckbChecked = row.FindControl("ckbSelected") as CheckBox;
                    if (null != ckbChecked && ckbChecked.Checked)
                        dicIDs.Add(row.RowIndex, (int)gridErrors.DataKeys[row.RowIndex].Value);
                }
            }
            if (dicIDs.Count > 0)
            {
                try
                {
                    PIHManager.DeleteImportErrors(dicIDs.Select(i => i.Value).ToList());
                }
                catch (Exception ex)
                {
                    ClientFun(this.updatePanel, "failedtodeleteuserinad", "alert('Failed to delete the selected import error(s), please try it again.');");
                    LPLog.LogMessage(LogType.Logerror, "Failed to delete the selected import error record(s): " + ex.Message);
                    return;
                }
                BindGrid();
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
