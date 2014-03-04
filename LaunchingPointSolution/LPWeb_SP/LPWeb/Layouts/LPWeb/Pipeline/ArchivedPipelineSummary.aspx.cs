using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Text;

public partial class Pipeline_ArchivedPipelineSummary : BasePage
{
    private readonly Loans _bllLoans = new Loans();
    private LoginUser _curLoginUser = new LoginUser();
    private LoanAlerts _loanAlerts = new LoanAlerts();
    private UserPipelineColumns _bllUPC = new UserPipelineColumns();
    public string FromURL = string.Empty;
    private bool isReset = false;
    private string fromHomeFilter = string.Empty;
    private string StageFilter;
    private UserRecentItems _bllUserRecentItems = new UserRecentItems();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Url != null)
        {
            FromURL = Request.Url.ToString();
        }
        if (!IsPostBack)
        {
            _curLoginUser = new LoginUser();
            ////not have LoanSetup can view this page 
            //if (!_curLoginUser.userRole.LoanSetup)
            //{
            //    Response.Redirect("../Unauthorize.aspx");
            //    lbtnSetup.Enabled = false;
            //    return;
            //}
            if (!_curLoginUser.userRole.ImportLoan)
            {
                btnSync.Enabled = false;
            }
            if (!_curLoginUser.userRole.RemoveLoan)
            {
                btnRemove.Enabled = false;
            }

            BindPageDefaultValues();
            fromHomeFilter = FilterFromHomePiplineSummary();
            BindLoanGrid();
        }
    }
    /// <summary>
    /// Filters from home pipline summary.
    /// </summary>
    /// <returns></returns>
    private string FilterFromHomePiplineSummary()
    {
        string searchCondition = string.Empty;
        StageFilter = string.Empty;
        if (!string.IsNullOrEmpty(Request.QueryString["q"]))
        {
            string qString = Encrypter.Base64Decode(Request.QueryString["q"]);
            //string qString = Request.QueryString["q"];
            //string qString = "RegionID=-1&DivisionID=8&BranchID=-1&DateType=CloseDate&FromDate=&ToDate=2010-11-08&Status=Processing&LastCompletedStage=Opened";
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
                this.ddlDivison.SelectedValue = KVPs["DivisionID"];
            }

            if (KVPs.ContainsKey("BranchID") && !string.IsNullOrEmpty(KVPs["BranchID"]))
            {
                //searchCondition += string.Format(searchTempllage, "BranchID", KVPs["BranchID"]);
                this.ddlBranch.SelectedValue = KVPs["BranchID"];
            }

            if (KVPs.ContainsKey("StageID") && !string.IsNullOrEmpty(KVPs["StageID"]))
            {
                //searchCondition += string.Format(searchTempllage, "StageID", KVPs["StageID"]);
                this.ddlStatus.SelectedValue = KVPs["StageID"];
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
                    searchCondition += string.Format(" AND [{0}] >= '{1}'", dateType, KVPs["FromDate"]);
                }

                if (KVPs.ContainsKey("ToDate") && !string.IsNullOrEmpty(KVPs["ToDate"]))
                {
                    searchCondition += string.Format(" AND [{0}] <= '{1}'", dateType, KVPs["ToDate"]);
                }
            }


            if (KVPs.ContainsKey("Status") && !string.IsNullOrEmpty(KVPs["Status"]))
            {
                searchCondition += string.Format(searchTempllage, "[Status]", KVPs["Status"]);
            }

            if (KVPs.ContainsKey("LastCompletedStage") && !string.IsNullOrEmpty(KVPs["LastCompletedStage"]))
            {
                searchCondition += string.Format(searchTempllage, "[LastCompletedStage]", KVPs["LastCompletedStage"]);
                StageFilter = "'" + KVPs["LastCompletedStage"] + "'";
            }
            else
            {
                StageFilter = string.Empty;
            }
        }
        return searchCondition;
    }
    /// <summary>
    /// Bind Page default values
    /// </summary>
    private void BindPageDefaultValues()
    {
        //Bind Alphabet
        const string alphabets = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        foreach (string alphabet in alphabets.Split(','))
        {
            ddlAlphabets.Items.Add(new ListItem(alphabet, alphabet));
        }

        // 获取当前用户信息
        _curLoginUser = new LoginUser();

        //Bind region,division,branch dropdownlist
        BindFilter();
    }

    private void BindFilter()
    {
        BindRegions(_curLoginUser);

        BindDivisions(_curLoginUser, "0");

        BindBranches(_curLoginUser, "0", "0");

        BindStages();
    }

    /// <summary>
    /// 根据用户界面选择生成过滤条件
    /// </summary>
    /// <returns></returns>
    private string GenerateQueryCondition()
    {
        //string queryCon = " Cleared IS NULL ";
        string queryCon = " 1=1 AND ([Status] <> 'Processing' AND [Status] <> 'Prospect') ";

        //alphabets 
        if (!string.IsNullOrEmpty(ddlAlphabets.SelectedValue))
            queryCon += string.Format(" AND [Borrower] Like '{0}%'", ddlAlphabets.SelectedValue);

        string groupIdQueryPart = GenOrgQueryCondition();
        if (!string.IsNullOrEmpty(groupIdQueryPart))
            queryCon += groupIdQueryPart;

        if (!string.IsNullOrEmpty(fromHomeFilter))
        {
            queryCon += fromHomeFilter;
        }

        if (!string.IsNullOrEmpty(StageFilter))
        {
            queryCon += string.Format(" AND ([Status] <> 'Processing' and [Status] <> 'Prospect') AND [LastCompletedStage] = {0} ", StageFilter);
        }
        else
        {
            if (ddlStatus.SelectedValue != "-1")
            {
                queryCon += string.Format(" AND [Status] = '{0}' ", this.ddlStatus.SelectedItem.Text);
            }

            if (!string.IsNullOrEmpty(this.EstStartDate.Text))
            {
                queryCon += string.Format(" AND [EstClose] >= '{0}' ", this.EstStartDate.Text);
            }

            if (!string.IsNullOrEmpty(this.EstEndDate.Text))
            {
                queryCon += string.Format(" AND [EstClose] <= '{0}' ", this.EstEndDate.Text);
            }
        }

        // filter from search loan popup
        if (!string.IsNullOrEmpty(this.hiSearchFilter.Value))
        {
            string strReturn = Server.HtmlDecode(this.hiSearchFilter.Value);
            string[] arrSearchPair = strReturn.Split('\u0001');
            foreach (string str in arrSearchPair)
            {
                string[] arrTemp = str.Split('\u0002');
                if (arrTemp.Length == 2)
                {
                    string strValue = string.Format("{0}", arrTemp[1]);
                    switch (arrTemp[0].ToLower())
                    {
                        case "lname":
                            queryCon += string.Format(" AND Borrower LIKE '%{0}%'", strValue);
                            break;
                        case "fname":
                            queryCon += string.Format(" AND Borrower LIKE '%{0}%'", strValue);
                            break;
                        case "status":
                            queryCon += string.Format(" AND Status='{0}'", strValue);
                            break;
                        case "addr":
                            queryCon += string.Format(" AND PropertyAddr LIKE '%{0}%'", strValue);
                            break;
                        case "city":
                            queryCon += string.Format(" AND PropertyCity LIKE '%{0}%'", strValue);
                            break;
                        case "state":
                            queryCon += string.Format(" AND PropertyState='{0}'", strValue);
                            break;
                        case "zip":
                            queryCon += string.Format(" AND PropertyZip LIKE '%{0}%'", strValue);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //来自lin邮件的描述: 如果用户没有"Access others loans"的权限, 则整个筛选必须包括LoanTeam.UserId和当前用户的匹配。

        if (!_curLoginUser.userRole.OtherLoanAccess)
        {
            //queryCon += " AND UserID = " + _curLoginUser.iUserID;
            queryCon += string.Format(" AND ([UserID] = {0} OR [Owner] = {0})", _curLoginUser.iUserID);
        }
        else
        {
            queryCon += string.Format(" OR ([UserID] = {0} OR [Owner] = {0})", _curLoginUser.iUserID);
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
        else if (ddlDivison.SelectedValue != "-1")
        {
            queryCon += string.Format(" AND DivisionID={0}", ddlDivison.SelectedValue);
        }
        else if (ddlRegion.SelectedValue != "-1")
        {
            queryCon += string.Format(" AND RegionId={0}", ddlRegion.SelectedValue);
        }
        else if(!_curLoginUser.bIsCompanyExecutive)//如果当前用户是company executive, 可以查所有group对应的loans, group查询条件为空 
        {
            //All,All,All的情况，这时是初始化用户还没有选择的情况，应该从region-->Division-->Branch检查(从上向下)，哪个dropdownlist有值，就加上这个限制查询条件
            var condition = "";
            var template = " {2} {0} IN ({1})";
            bool hasCon = false;

            //region
            if (ddlRegion.Items.Count > 1)
            {
                var aggregateIds = AggregateIds(ddlRegion.Items);
                condition = string.Format(template, "[RegionId]", aggregateIds, "AND");
                hasCon = true;
            }

            //division
            if (!hasCon)
            {
                if (ddlDivison.Items.Count > 1)
                {
                    var aggregateIds = AggregateIds(ddlDivison.Items);
                    condition += string.Format(template, "[DivisionId]", aggregateIds, "AND");
                    hasCon = true;
                }
            }
            else
            {
                if (ddlDivison.Items.Count > 1)
                {
                    var aggregateIds = AggregateIds(ddlDivison.Items);
                    condition += string.Format(template, "[DivisionId]", aggregateIds, "OR");
                    hasCon = true;
                }
            }

            //branch
            if (!hasCon)
            {
                if (ddlBranch.Items.Count > 1)
                {
                    var aggregateIds = AggregateIds(ddlBranch.Items);
                    condition += string.Format(template, "[BranchId]", aggregateIds, "AND");
                    hasCon = true;
                }
            }
            else
            {
                if (ddlBranch.Items.Count > 1)
                {
                    var aggregateIds = AggregateIds(ddlBranch.Items);
                    condition += string.Format(template, "[BranchId]", aggregateIds, "OR");
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
        LPWeb.BLL.Regions RegionManager = new LPWeb.BLL.Regions();
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

        LPWeb.BLL.Divisions DivisionManager = new LPWeb.BLL.Divisions();
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

        this.ddlDivison.DataSource = DivisionListData;
        this.ddlDivison.DataBind();
    }

    private void BindBranches(LoginUser curLoginUser, string regionId, string divisionId)
    {
        int iRegionID = int.Parse(regionId);
        int iDivisionID = int.Parse(divisionId);

        iRegionID = iRegionID >= 0 ? iRegionID : 0;
        iDivisionID = iDivisionID >= 0 ? iDivisionID : 0;

        LPWeb.BLL.Branches BrancheManager = new LPWeb.BLL.Branches();
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

    private void BindStages()
    {
        DataTable statusData = new DataTable();

        statusData.Columns.Add("StageID", typeof(string));
        statusData.Columns.Add("Name", typeof(string));

        statusData.Rows.Add("-1", "All Statuses");
        statusData.Rows.Add("Canceled", "Canceled");
        statusData.Rows.Add("Closed", "Closed");
        statusData.Rows.Add("Denied", "Denied");
        statusData.Rows.Add("Suspended", "Suspended");

        this.ddlStatus.DataSource = statusData;
        this.ddlStatus.DataBind();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        StageFilter = string.Empty;
        isReset = true;
        BindLoanGrid();
    }

    protected void ddlAlphabets_SelectedIndexChanged(object sender, EventArgs e)
    {
        isReset = true;
        BindLoanGrid();
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
        BindLoanGrid();
    }

    protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        isReset = true;
        _curLoginUser = new LoginUser();
        if (ddlRegion.SelectedIndex == 0)
        {
            BindFilter();
            return;
        }
        BindDivisions(_curLoginUser, ddlRegion.SelectedValue);
        BindBranches(_curLoginUser, ddlRegion.SelectedValue, ddlDivison.SelectedValue);
    }

    protected void ddlDivison_SelectedIndexChanged(object sender, EventArgs e)
    {
        isReset = true;
        _curLoginUser = new LoginUser();
        BindBranches(_curLoginUser, ddlRegion.SelectedValue, ddlDivison.SelectedValue);
    }

    /// <summary>
    /// Bind Grid
    /// </summary>
    private void BindLoanGrid()
    {
        int pageSize = AspNetPager1.PageSize;
        try
        {
            if (_curLoginUser != null)
            {
                Users users = new Users();               
                int iLoansPerPage = 0;
                //LPWeb.Model.Users u = users.GetModel(_curLoginUser.iUserID);
                iLoansPerPage = users.GetLoansPerPage(_curLoginUser.iUserID);
                if (iLoansPerPage != 0)
                {
                    pageSize = iLoansPerPage;
                    AspNetPager1.PageSize = iLoansPerPage;
                }
                else
                {
                    pageSize = 20;
                    AspNetPager1.PageSize = 20;
                }
                //_curLoginUser.
            }
        }
        catch (Exception exception)
        {
            pageSize = 20;
            AspNetPager1.PageSize = 20;
            LPLog.LogMessage(exception.Message);
        }

        int pageIndex = 1;

        if (AspNetPager1.CurrentPageIndex > 0 && isReset == false)
            pageIndex = AspNetPager1.CurrentPageIndex;

        string queryCondition = GenerateQueryCondition();

        //if(!string.IsNullOrEmpty(fromHomeFilter))
        //{
        //    queryCondition += fromHomeFilter;
        //}
        int recordCount = 0;

        DataSet loanLists = null;
        try
        {
            loanLists = _bllLoans.GetList(pageSize, pageIndex, queryCondition, out recordCount, OrderName, OrderType);
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

        AspNetPager1.PageSize = pageSize;
        AspNetPager1.RecordCount = recordCount;
        //得到RuleAlertID
        GetRuleAlertloanLists(ref loanLists);
        gvPipelineView.DataSource = loanLists;
        gvPipelineView.DataBind();

        UserColumn(gvPipelineView);
    }

    private void UserColumn(GridView gridView)
    {
        bool defaultValue = false;
        LPWeb.Model.UserPipelineColumns modUPC = null;
        try
        {
            if (_curLoginUser != null)
            {
                modUPC = _bllUPC.GetModel(_curLoginUser.iUserID);
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

        //2 Status 
        //3 alert icon
        gridView.Columns[4].Visible = modUPC == null ? !defaultValue : modUPC.PercentCompl;
        gridView.Columns[5].Visible = modUPC == null ? !defaultValue : modUPC.EstimatedClose;
        
        gridView.Columns[6].Visible = modUPC == null ? !defaultValue : modUPC.LoanOfficer;

        gridView.Columns[7].Visible = modUPC == null ? !defaultValue : modUPC.Amount;
        gridView.Columns[8].Visible = modUPC == null ? !defaultValue : modUPC.Lien;
        gridView.Columns[9].Visible = modUPC == null ? !defaultValue : modUPC.Rate;
        gridView.Columns[10].Visible = modUPC == null ? defaultValue : modUPC.Lender;
        gridView.Columns[11].Visible = modUPC == null ? defaultValue : modUPC.LockExp;
        gridView.Columns[12].Visible = modUPC == null ? defaultValue : modUPC.Branch;

        gridView.Columns[13].Visible = modUPC == null ? defaultValue : modUPC.Processor;
        gridView.Columns[14].Visible = modUPC == null ? defaultValue : modUPC.TaskCount;
        gridView.Columns[15].Visible = modUPC == null ? defaultValue : modUPC.PointFolder;
        gridView.Columns[16].Visible = modUPC == null ? defaultValue : modUPC.PointFileName;
    }

    private void GetRuleAlertloanLists(ref DataSet LoadLists)
    {
        DataTable dt = LoadLists.Tables[0];
        if (!dt.Columns.Contains("AlertID"))
        {
            dt.Columns.Add("AlertID");

            foreach (DataRow dr in dt.Rows)
            {
                dr["AlertID"] = _loanAlerts.GetRuleAlertID(Convert.ToInt32(dr["FileId"]));
            }
        }
        dt.AcceptChanges();
    }

    /// <summary>
    /// Handles the Sorting event of the gvPipelineView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
    protected void gvPipelineView_Sorting(object sender, GridViewSortEventArgs e)
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
        BindLoanGrid();
    }

    StringBuilder sbAllLIdStatus = new StringBuilder();
    /// <summary>
    /// Set selected row when click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvPipelineView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (DataControlRowType.DataRow == e.Row.RowType)
        {
            int nFileId = 0;
            string strStatus = string.Format("{0}", gvPipelineView.DataKeys[e.Row.RowIndex]["Status"]);
            string strBranchId = string.Format("{0}", gvPipelineView.DataKeys[e.Row.RowIndex]["BranchID"]);
            if (null != gvPipelineView.DataKeys[e.Row.RowIndex])
            {
                if (!int.TryParse(gvPipelineView.DataKeys[e.Row.RowIndex].Value.ToString(), out nFileId))
                    nFileId = 0;

                if (0 != nFileId)
                {
                    sbAllLIdStatus.AppendFormat("allLoan.push(new SelectedLoan('{0}', '{1}', '{2}'));", nFileId, strStatus, strBranchId);
                }
            }
        }
    }

    protected void gvPipelineView_PreRender(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "registerLOIds", sbAllLIdStatus.ToString(), true);
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

    /// <summary>
    /// Handles the Click event of the btnSync control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnSync_Click(object sender, EventArgs e)
    {
        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            ImportLoansResponse respone;
            try
            {
                var selctedStr = this.hfDeleteItems.Value;
                string[] selectedItems = selctedStr.Split(',');
                ImportLoansRequest req = new ImportLoansRequest();
                //req.PointFiles = selectedItems;//todo:check DataContract change
                req.FileIds = Array.ConvertAll(selectedItems, item => int.Parse(item));
                req.hdr = new ReqHdr();
                req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                req.hdr.UserId = 5;//todo:check dummy data

                respone = service.ImportLoans(req);

                if (respone.hdr.Successful)
                {
                    PageCommon.WriteJsEnd(this, "Sync loan(s) Successfully", PageCommon.Js_RefreshSelf);
                }
                else
                {
                    PageCommon.WriteJsEnd(this, "Failed to sync loan(s).", PageCommon.Js_RefreshSelf);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ee)
            {
                LPLog.LogMessage(ee.Message);
                PageCommon.AlertMsg(this, "Failed to sync loan(s), reason: Point Manager is not running.");
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
                PageCommon.WriteJsEnd(this, "Failed to sync loan(s).", PageCommon.Js_RefreshSelf);
            }
        }
    }

    /// <summary>
    /// Handles the Click event of the btnRemove control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        var selctedStr = this.hfDeleteItems.Value;
        if (!string.IsNullOrEmpty(selctedStr))
        {
            string[] selectedItems = selctedStr.Split(',');
            //delete the selected items
            DeleteLoans(selectedItems);
            //reload the grid data
            PageCommon.WriteJsEnd(this, "Loan has been removed successfully.", PageCommon.Js_RefreshSelf);
        }
        this.hfDeleteItems.Value = "";
    }

    /// <summary>
    /// Deletes the loan programs.
    /// </summary>
    /// <param name="items">The items.</param>
    private void DeleteLoans(string[] items)
    {
        int iItem = 0;
        foreach (var item in items)
        {
            if (int.TryParse(item, out iItem))
            {
                try
                {
                    _bllLoans.Delete(iItem);
                    _bllUserRecentItems.DeleteItemsByFileID(iItem); // delete UserRecentItems records
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDispose_Click(object sender, EventArgs e)
    {
        bool bmove = false;
        int nFileId = -1;
        if (!int.TryParse(this.hiSelectedLoan.Value, out nFileId))
            nFileId = -1;
        if (nFileId == -1)
        {
            LPLog.LogMessage(LogType.Logerror, "Invalid file Id: " + this.hiSelectedLoan.Value);
            return;
        }

        int nFolderId = -1;
        if (!int.TryParse(this.hiSelectedFolderId.Value, out nFolderId))
            nFolderId = -1;
        if (nFolderId == -1)
        {
            LPLog.LogMessage(LogType.Logerror, "Invalid folder Id: " + this.hiSelectedFolderId.Value);
            return;
        }


        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient client = sm.StartServiceClient())
            {
                if (!bmove)
                {
                    DisposeLoanRequest req = new DisposeLoanRequest();
                    req.FileId = nFileId;
                    req.LoanStatus = hiSelectedDisposal.Value;
                    req.NewFolderId = nFolderId;
                    req.hdr = new ReqHdr();
                    req.hdr.UserId = CurrUser.iUserID;
                    req.StatusDate = DateTime.Now;

                    DisposeLoanResponse response = client.DisposeLoan(req);
                    if (response.hdr.Successful)
                    {

                        if (WorkflowManager.UpdateLoanStatus(nFileId, hiSelectedDisposal.Value, CurrUser.iUserID))
                        {
                            BindLoanGrid();
                            LPLog.LogMessage(LogType.Loginfo, string.Format("Successfully update loan status, LoanId:{0}, to Status:{1}. ",
                                nFileId, this.hiSelectedDisposal.Value));
                        }
                        else
                        {
                            PageCommon.AlertMsg(this, "Failed to update loan status.");
                            LPLog.LogMessage(LogType.Logerror, string.Format("Failed to update loan status, LoanId:{0}, to Status:{1}.",
                                nFileId, this.hiSelectedDisposal.Value));
                        }

                    }
                    else
                    {
                        LPLog.LogMessage(LogType.Logerror, string.Format("Failed to move file:{0}", response.hdr.StatusInfo));
                        PageCommon.AlertMsg(this, response.hdr.StatusInfo);
                    }
                }
                else
                {
                    MoveFileRequest req = new MoveFileRequest();
                    req.FileId = nFileId;
                    req.NewFolderId = nFolderId;
                    req.hdr = new ReqHdr();
                    req.hdr.UserId = CurrUser.iUserID;

                    MoveFileResponse response = client.MoveFile(req);
                    if (response.hdr.Successful)
                    {

                    }
                    else
                    {
                        LPLog.LogMessage(LogType.Logerror, string.Format("Failed to move file:{0}", response.hdr.StatusInfo));
                        PageCommon.AlertMsg(this, response.hdr.StatusInfo);
                    }
                }
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ee.Message));
            PageCommon.AlertMsg(this, "Failed to move the Point file, reason: Point Manager is not running.");
        }
        catch (Exception ex)
        {
            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ex.Message));
            PageCommon.AlertMsg(this, ex.Message);
        }
    }

    protected void btnResume_Click(object sender, EventArgs e)
    {
        int nFileId = -1;
        if (!int.TryParse(this.hiSelectedLoan.Value, out nFileId))
            nFileId = -1;
        if (nFileId == -1)
        {
            LPLog.LogMessage(LogType.Logerror, "Invalid file Id: " + this.hiSelectedLoan.Value);
            return;
        }

        int nFolderId = -1;
        if (!int.TryParse(this.hiSelectedFolderId.Value, out nFolderId))
            nFolderId = -1;
        if (nFolderId == -1)
        {
            LPLog.LogMessage(LogType.Logerror, "Invalid folder Id: " + this.hiSelectedFolderId.Value);
            return;
        }

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient client = sm.StartServiceClient())
            {
                DisposeLoanRequest req = new DisposeLoanRequest();
                req.FileId = nFileId;
                req.LoanStatus = this.hiSelectedDisposal.Value;
                req.NewFolderId = nFolderId;
                req.hdr = new ReqHdr();
                req.hdr.UserId = CurrUser.iUserID;
                req.StatusDate = DateTime.Now;

                DisposeLoanResponse response = client.DisposeLoan(req);
                if (response.hdr.Successful)
                {
                    if (WorkflowManager.UpdateLoanStatus(nFileId, hiSelectedDisposal.Value, CurrUser.iUserID))
                    {
                        BindLoanGrid();
                        LPLog.LogMessage(LogType.Loginfo, string.Format("Successfully update loan status, LoanId:{0}, to Status:{1}. ",
                            nFileId, this.hiSelectedDisposal.Value));
                    }
                    else
                    {
                        PageCommon.AlertMsg(this, "Failed to update loan status.");
                        LPLog.LogMessage(LogType.Logerror, string.Format("Failed to update loan status, LoanId:{0}, to Status:{1}.",
                            nFileId, this.hiSelectedDisposal.Value));
                    }
                }
                else
                {
                    LPLog.LogMessage(LogType.Logerror, string.Format("Failed to move file:{0}", response.hdr.StatusInfo));
                    PageCommon.AlertMsg(this, response.hdr.StatusInfo);
                }
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ee.Message));
            PageCommon.AlertMsg(this, "Failed to move the Point file, reason: Point Manager is not running.");
        }
        catch (Exception ex)
        {
            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ex.Message));
            PageCommon.AlertMsg(this, ex.Message);
        }

    }
}
