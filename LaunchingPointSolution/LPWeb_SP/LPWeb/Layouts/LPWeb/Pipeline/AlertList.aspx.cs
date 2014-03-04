using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using System.Collections.Specialized;

public partial class Pipeline_AlertList : BasePage
{
    private readonly LoanAlerts _bllLoanAlerts = new LoanAlerts();

    // 当前用户
    public string FromURL = string.Empty;
    private LoginUser _curLoginUser;
    private bool isReset = false;
    private DataSet dsAlertList;
    private bool isGrid = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Url != null)
        {
            FromURL = Request.Url.ToString();
        }

        isGrid = false;

        if (!IsPostBack)
        {
            _curLoginUser = new LoginUser();
            ////权限验证 
            //if (!_curLoginUser.userRole.OtherLoanAccess) // OtherLoanAccess 应该是控制数据访问权限，而不是进入页面的权限，此处去掉权限判断
            //{
            //    Response.Redirect("../Unauthorize.aspx");
            //    return;
            //}
            BindPageDefaultValues();

            BindAlertGrid();
        }
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
        BindRegions(_curLoginUser);

        BindDivisions(_curLoginUser,"0");

        BindBranches(_curLoginUser,"0", "0");

        BindAlertsOwner();
    }

    private void BindAlertsOwner()
    {
        DataTable dtOwner = _bllLoanAlerts.GetAlertOwner();
        foreach (DataRow dr in dtOwner.Rows)
        {
            ddlAlerts.Items.Add(new ListItem(dr["OwnerName"].ToString(), dr["OwnerId"].ToString()));
        }
    }

    /// <summary>
    /// Bind Grid
    /// </summary>
    private void BindAlertGrid()
    {
        if (isGrid == true)
        {
            return;
        }
      
        isGrid = true;

        int pageSize = AspNetPager1.PageSize;
        int pageIndex = 1;

        if (isReset == true)
            pageIndex = AspNetPager1.CurrentPageIndex = 1;
        else
            pageIndex = AspNetPager1.CurrentPageIndex;

        string queryCondition = GenerateQueryCondition();
        int recordCount = 0;
        string sortField = "Desc";
        bool sortDirection = false;
        if(ViewState["SortFD"]!=null)
        {
            var arg = ViewState["SortFD"].ToString().Split(',');
            sortField = arg[0];

            if(arg[1].Equals("DESC"))
                sortDirection = true;
        }
        dsAlertList = _bllLoanAlerts.GetList(pageSize, pageIndex,sortField,sortDirection, queryCondition, out recordCount);

        AspNetPager1.PageSize = pageSize;
        AspNetPager1.RecordCount = recordCount;

        rptGrid.DataSource = dsAlertList;
        rptGrid.DataBind();

        if (ViewState["SortFD"] != null)//情况排序条件
        {
            ViewState["SortFD"] = null;   
        }
    }

    /// <summary>
    /// 根据用户界面选择生成过滤条件
    /// </summary>
    /// <returns></returns>
    private string GenerateQueryCondition()
    {
        //string queryCon = " Cleared IS NULL ";
        string queryCon = " 1=1 ";

        queryCon += " AND ([Status] IS NULL OR [Status]='Acknowledged') AND ([DisplayIcon] LIKE '%Red%' OR [DisplayIcon] LIKE '%Yellow%') ";
        //alphabets 
        if (!string.IsNullOrEmpty(ddlAlphabets.SelectedValue))
            queryCon += string.Format(" AND [Desc] Like '{0}%'", ddlAlphabets.SelectedValue);

        //Tasks
        if (ddlTasks.SelectedValue.ToLower().Equals("1"))
            queryCon += string.Format(" AND DueDate < '{0}'", DateTime.Now.ToShortDateString());
        else if (ddlTasks.SelectedValue.ToLower().Equals("2"))
            queryCon += string.Format(" AND DueDate = '{0}'", DateTime.Now.ToShortDateString());
        else if (ddlTasks.SelectedValue.ToLower().Equals("3"))
            queryCon += string.Format(" AND DueDate = '{0}'", DateTime.Now.AddDays(1).ToShortDateString());
        else if (ddlTasks.SelectedValue.ToLower().Equals("4"))
            queryCon += string.Format(" AND DueDate > '{0}' and DueDate <= '{1}' ", DateTime.Now.ToShortDateString(), DateTime.Now.AddDays(7).ToShortDateString());
        else if (ddlTasks.SelectedValue.ToLower().Equals("5"))
            queryCon += string.Format(" AND DueDate <= '{0}'", DateTime.Now.ToShortDateString());

        _curLoginUser = new LoginUser();
        //My Alerts
        if (ddlAlerts.SelectedValue == "0")
            queryCon += string.Format(" AND OwnerId={0}", _curLoginUser.iUserID);
        else if(ddlAlerts.SelectedValue != "")
            queryCon += string.Format(" AND OwnerId={0}", ddlAlerts.SelectedValue.ToString());

        //Alert Type
        if(ddlAlertType.SelectedValue !="")
            queryCon += string.Format(" AND AlertType='{0}'", ddlAlertType.SelectedValue.ToString());

        string groupIdQueryPart = GenOrgQueryCondition();
        if (!string.IsNullOrEmpty(groupIdQueryPart))
            queryCon += groupIdQueryPart;

        //来自lin邮件的描述: 如果用户没有"Access others loans"的权限, 则整个筛选必须包括LoanTeam.UserId和当前用户的匹配。

        //if (!_curLoginUser.userRole.OtherLoanAccess)
        //{
        //    //queryCon += " AND UserID = " + _curLoginUser.iUserID;
        //    queryCon += string.Format(" AND (UserID = {0} OR Owner = {0})", _curLoginUser.iUserID);
        //}
        //else
        //{
        //    queryCon += string.Format(" OR (UserID = {0} OR Owner = {0})", _curLoginUser.iUserID);
        //}
        if (CurrUser.bIsCompanyExecutive || CurrUser.bIsRegionExecutive || CurrUser.bIsDivisionExecutive)
        {
            queryCon += string.Format(" AND FileId in (select LoanID from dbo.lpfn_GetUserLoans_Executive({0})) ", CurrUser.iUserID);
        }
        else
            if (CurrUser.bIsBranchManager)
                queryCon += string.Format(" AND FileId in (select LoanID from dbo.lpfn_GetUserLoans_Branch_Manager({0})) ", CurrUser.iUserID);
        else
                queryCon += string.Format(" AND FileId in (select LoanID from dbo.lpfn_GetUserLoans({0})) ", CurrUser.iUserID);
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
        string queryCon = string.Empty;
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
                if (ddlDivison.Items.Count > 1)
                {
                    var aggregateIds = AggregateIds(ddlDivison.Items);
                    condition += string.Format(template, "DivisionId", aggregateIds, "AND");
                    hasCon = true;
                }
            }
            else
            {
                if (ddlDivison.Items.Count > 1)
                {
                    var aggregateIds = AggregateIds(ddlDivison.Items);
                    condition += string.Format(template, "DivisionId", aggregateIds, "AND");
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
                    condition += string.Format(template, "BranchId", aggregateIds, "AND");
                    hasCon = true;
                }
            }

            if (hasCon)
            {
                queryCon += condition;
            }
        }
        return queryCon;
        //DataSet dsGroups = bllGroup.GetList(queryCon);
        //if (dsGroups == null || dsGroups.Tables.Count == 0 || dsGroups.Tables[0].Rows.Count == 0)
        //    return " AND 1<0 ";//todo:如果没有筛选出group，那么整个查询则是无效的

        ////根据组织结构查询出所属的所有groupid，并用‘，’拼接成字符串
        //string groupIds =
        //    dsGroups.Tables[0].AsEnumerable().Select(groupFolder => groupFolder.Field<int>("GroupId").ToString()).
        //        Aggregate((ids, next) => ids + "," + next);

        //return string.Format(" AND GroupID IN ({0})", groupIds);
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
        #region Obsolete Code
        //if (curLoginUser.userRole.OtherLoanAccess == true)   // All Loans
        //{
        //    RegionListData = RegionManager.GetRegionList_AllLoans(curLoginUser.iUserID);
        //}
        //else // Assigned Loans
        //{
        //    RegionListData = RegionManager.GetRegionList_AssingedLoans(curLoginUser.iUserID);
        //}
        #endregion
        if (CurrUser.bIsCompanyExecutive || CurrUser.bIsRegionExecutive || CurrUser.bIsDivisionUser)
            RegionListData = RegionManager.GetRegionFilter_Executive(CurrUser.iUserID);
        else
            if (CurrUser.bIsBranchManager)
                RegionListData = RegionManager.GetRegionFilter_Branch_Manager(CurrUser.iUserID);
            else
                RegionListData = RegionManager.GetRegionFilter(CurrUser.iUserID);

        // add "All Regions" row
        DataRow NewRegionRow = RegionListData.NewRow();
        NewRegionRow["RegionID"] = "-1";
        NewRegionRow["Name"] = "All Regions";
        RegionListData.Rows.InsertAt(NewRegionRow, 0);

        this.ddlRegion.DataSource = RegionListData;
        this.ddlRegion.DataBind();
    }

    private void BindDivisions(LoginUser curLoginUser,string regionId)
    {
        int iRegionID = int.Parse(regionId);

        LPWeb.BLL.Divisions DivisionManager = new LPWeb.BLL.Divisions();
        DataTable DivisionListData = null;

        if (CurrUser.bIsCompanyExecutive || CurrUser.bIsRegionExecutive || CurrUser.bIsDivisionUser)
            DivisionListData = DivisionManager.GetDivisionFilter_Executive(CurrUser.iUserID, iRegionID);
        else
            if (CurrUser.bIsBranchManager)
                DivisionListData = DivisionManager.GetDivisionFilter_Branch_Manager(CurrUser.iUserID, iRegionID);
            else
                DivisionListData = DivisionManager.GetDivisionFilter(CurrUser.iUserID, iRegionID);
        #region obsolete code
        //if (curLoginUser.userRole.OtherLoanAccess == true)   // All Loans
        //{
        //    if (iRegionID == 0)     // 没有region参数
        //    {
        //        DivisionListData = DivisionManager.GetDivision_AllLoans(curLoginUser.iUserID);
        //    }
        //    else // 以region来筛选division
        //    {
        //        DivisionListData = DivisionManager.GetDivision_AllLoans(curLoginUser.iUserID, iRegionID);
        //    }
        //}
        //else // Assigned Loans
        //{
        //    if (iRegionID == 0)     // 没有region参数
        //    {
        //        DivisionListData = DivisionManager.GetDivisionList_AssingedLoans(curLoginUser.iUserID);
        //    }
        //    else // 以region来筛选division
        //    {
        //        DivisionListData = DivisionManager.GetDivisionList_AssingedLoans(curLoginUser.iUserID, iRegionID);
        //    }
        //}
        #endregion
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
        if (CurrUser.bIsCompanyExecutive || CurrUser.bIsRegionExecutive || CurrUser.bIsDivisionUser)
            BranchListData = BrancheManager.GetBranchFilter_Executive(CurrUser.iUserID, iRegionID, iDivisionID);
        else
            if (CurrUser.bIsBranchManager)
                BranchListData = BrancheManager.GetBranchFilter_Branch_Manager(CurrUser.iUserID, iRegionID, iDivisionID);
            else
                BranchListData = BrancheManager.GetBranchFilter(CurrUser.iUserID, iRegionID, iDivisionID);
        #region Obsolete Code
        //if (curLoginUser.userRole.OtherLoanAccess == true)   // All Loans
        //{
        //    BranchListData = BrancheManager.GetBranchList_AllLoans(curLoginUser.iUserID, iRegionID, iDivisionID);
        //}
        //else // Assigned Loans
        //{
        //    BranchListData = BrancheManager.GetBranchList_AssingedLoans(curLoginUser.iUserID, iRegionID, iDivisionID);
        //}
        #endregion
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
        BindAlertGrid();
    }

    protected void lbtnEmpty_Click(object sender, EventArgs e)
    {
        isReset = false;
        BindAlertGrid();
    }

    protected void ddlAlphabets_SelectedIndexChanged(object sender, EventArgs e)
    {
        isReset = true;
        BindAlertGrid();
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
        BindAlertGrid();
    }

    protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        _curLoginUser = new LoginUser();
        if (ddlRegion.SelectedIndex == 0)
        {
            BindRegions(_curLoginUser);

            BindDivisions(_curLoginUser, "0");

            BindBranches(_curLoginUser, "0", "0");
            return;
        }
        BindDivisions(_curLoginUser, ddlRegion.SelectedValue);
        BindBranches(_curLoginUser, ddlRegion.SelectedValue, "0");
    }

    protected void ddlDivison_SelectedIndexChanged(object sender, EventArgs e)
    {
        _curLoginUser = new LoginUser();
        BindBranches(_curLoginUser,ddlRegion.SelectedValue,ddlDivison.SelectedValue);
    }

    protected void btnSort_Click(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(hfdSortField.Value) || string.IsNullOrEmpty(hfdDirection.Value))
            return;

        string sortField = hfdSortField.Value;
        string sortDirection = hfdDirection.Value;

        ViewState["SortFD"] = string.Format("{0},{1}", sortField, sortDirection);
        BindAlertGrid();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        BindAlertGrid();

        DataTable dtExport = new DataTable();
        dtExport.Columns.Add("Alert Description");
        dtExport.Columns.Add("Assign User");
        dtExport.Columns.Add("Type");
        dtExport.Columns.Add("Due Date");
        dtExport.Columns.Add("Borrower");
        DataRow drExport;
        string sSelAlertID = hfSelAlertId.Value.TrimEnd(',');
        DataRow[] drs = dsAlertList.Tables[0].Select("LoanAlertId in (" + sSelAlertID + ")");
        foreach (DataRow dr in drs)
        {
            drExport = dtExport.NewRow();
            drExport["Alert Description"] = dr["Desc"].ToString();
            drExport["Assign User"] = dr["Username"].ToString();
            drExport["Type"] = dr["AlertType"].ToString();
            drExport["Due Date"] = dr["DueDate"].ToString() != "" ? Convert.ToDateTime(dr["DueDate"]).ToShortDateString() : dr["DueDate"].ToString();
            drExport["Borrower"] = dr["BorrowerName"].ToString();
            dtExport.Rows.Add(drExport);
        }
        //// Excel Columns
        //NameValueCollection ExcelCollection = new NameValueCollection();
        //ExcelCollection.Add("Alert Description", "Desc");
        //ExcelCollection.Add("Assign User", "Username");
        //ExcelCollection.Add("Type", "AlertType");
        //ExcelCollection.Add("Due Date", "DueDate");
        //ExcelCollection.Add("Borrower", "BorrowerName");

        // sheet name
        string sSheetName = "Alerts";

        // 显示给用户的Xls文件名            
        DateTime dt = DateTime.Now;
        string sClientXlsFileName = "Alerts-" + dt.ToString("MM_dd_yy") + ".xls";

        // export and download
        XlsExporter.DownloadXls(this, dtExport, sClientXlsFileName, sSheetName);
    }
}