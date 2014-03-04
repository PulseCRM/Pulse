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

public partial class Pipeline_ProspectPipelineSummary : BasePage
{
    #region Parameters
    private readonly Prospect _bllProspect = new Prospect();
    private LoginUser _curLoginUser = new LoginUser();
    private ProspectAlerts _prospectAlerts = new ProspectAlerts();
    private UserProspectColumns _bllUPC = new UserProspectColumns();
    public string FromURL = string.Empty;
    private bool isReset = false;
    private string fromHomeFilter = string.Empty;
    LPWeb.BLL.Users UsersManager = new Users();
    DataTable dtUserBranch = new DataTable();
    private string StageFilter;
    protected string sHasViewRight = "0";
    protected string sHasCreateRight = "0";
    StringBuilder sbAllLIdStatus = new StringBuilder();
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Url != null)
        {
            FromURL = Request.Url.ToString();
        }

        _curLoginUser = new LoginUser();
        //是否有View权限
        sHasViewRight = _curLoginUser.userRole.Prospect.ToString().IndexOf('D') > -1 ? "1" : "0";  //View

        if (!IsPostBack)
        {
            try
            {
                //权限验证 
                if (_curLoginUser.userRole.Prospect.ToString().Trim().Length > 0) // Has Rights
                {
                    if (_curLoginUser.userRole.Prospect.ToString().IndexOf('I') == -1)
                    {
                        btnAssign.Enabled = false;
                    }
                    if (_curLoginUser.userRole.Prospect.ToString().IndexOf('C') == -1)
                    {
                        btnRemove.Enabled = false;
                    }
                    if (_curLoginUser.userRole.Prospect.ToString().IndexOf('E') == -1)
                    {
                        btnSearch.Enabled = false;
                    }
                    if (_curLoginUser.userRole.Prospect.ToString().IndexOf('F') == -1)
                    {
                        btnDispose.Enabled = false;
                    }
                    if (_curLoginUser.userRole.Prospect.ToString().IndexOf('J') == -1)
                    {
                        btnMerge.Enabled = false;
                    }
                    if (_curLoginUser.userRole.Prospect.ToString().IndexOf('A') == -1)
                    {
                        btnImportLeads.Enabled = false;
                        //btnCreate.Enabled = false;
                    }
                    else
                    {
                        sHasCreateRight = "1";
                    }
                    if (_curLoginUser.userRole.ExportPipelines)
                    {
                        aExport.Enabled = true;
                    }
                    else
                    {
                        aExport.Enabled = false;
                    }


                }
                else
                {
                    Response.Redirect("../Unauthorize1.aspx");  // have not View Power
                    return;
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
            BindPageDefaultValues();
            BindUserPiplineView();//gdc CR45
            fromHomeFilter = FilterFromHomePiplineSummary();
            BindLoanGrid();
        }
    }
    #endregion

    #region Function
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
                //this.ddlRegion.SelectedValue = KVPs["RegionID"];
                ddlOrganizationTypes.SelectedIndex = 1;
                this.ddlOrganization.SelectedValue = KVPs["RegionID"];
            }

            if (KVPs.ContainsKey("DivisionID") && !string.IsNullOrEmpty(KVPs["DivisionID"]))
            {
                //searchCondition += string.Format(searchTempllage, "DivisionID", KVPs["DivisionID"]);
                //this.ddlDivison.SelectedValue = KVPs["DivisionID"];
                ddlOrganizationTypes.SelectedIndex = 2;
                this.ddlOrganization.SelectedValue = KVPs["DivisionID"];
            }

            if (KVPs.ContainsKey("BranchID") && !string.IsNullOrEmpty(KVPs["BranchID"]))
            {
                //searchCondition += string.Format(searchTempllage, "BranchID", KVPs["BranchID"]);
                //this.ddlBranch.SelectedValue = KVPs["BranchID"];
                ddlOrganizationTypes.SelectedIndex = 3;
                this.ddlOrganization.SelectedValue = KVPs["BranchID"];
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
                ddlStatus.SelectedValue = KVPs["Status"];
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

        BindContextMenu();
    }

    private void BindFilter()
    {
        ddlOrganization.Items.Clear();
        ddlOrganization.Items.Add(new ListItem("All organizations", "0"));
        ddlLeadSource.Items.Clear();
        ddlLeadSource.Items.Add(new ListItem("All", "0"));
    }

    private void BindContextMenu()
    {
        #region Context Menu


        //加载 ArchiveLeadStatus
        StringBuilder sbSubMenuItems = new StringBuilder();
        ArchiveLeadStatus _bArchiveLeadStatus = new ArchiveLeadStatus();
        DataSet dsAS = _bArchiveLeadStatus.GetList("isnull([Enabled],0)=1");

        sbSubMenuItems.AppendLine("<ul>");
        foreach (DataRow dr in dsAS.Tables[0].Rows)
        {
            sbSubMenuItems.AppendLine("<li><a href=\"\" onclick=\"DisposeLoan('" + dr["LeadStatusName"].ToString() + "'); return false;\">" + dr["LeadStatusName"].ToString() + "</a></li>");
        }
        sbSubMenuItems.AppendLine("</ul>");


        this.ltrMenuItems.Text = sbSubMenuItems.ToString();


        #endregion
    }

    /// <summary>
    /// 根据用户界面选择生成过滤条件
    /// </summary>
    /// <returns></returns>
    private string GenerateQueryCondition()
    {
        //string queryCon = " Cleared IS NULL ";
        string queryCon = " 1=1 ";

        string groupIdQueryPart = GenOrgQueryCondition();
        if (!string.IsNullOrEmpty(groupIdQueryPart))
            queryCon += groupIdQueryPart;

        if (!string.IsNullOrEmpty(fromHomeFilter))
        {
            queryCon += fromHomeFilter;
        }

        //此处不做控制 因为prospect和loan team没关联 （经合Rocky 讨论 暂不加控制 2011-02-18 Alex）
        ////来自lin邮件的描述: 如果用户没有"Access others loans"的权限, 则整个筛选必须包括LoanTeam.UserId和当前用户的匹配。
        //if (!_curLoginUser.userRole.OtherLoanAccess)
        //{
        //    //queryCon += " AND UserID = " + _curLoginUser.iUserID;
        //    queryCon += string.Format(" AND ([UserID] = {0})", _curLoginUser.iUserID);
        //}
        //else
        //{
        //    queryCon += string.Format(" OR ([UserID] = {0})", _curLoginUser.iUserID);
        //}

        return queryCon;
    }

    /// <summary>
    /// Query Condition
    /// </summary>
    /// <returns></returns>
    private string GenOrgQueryCondition()
    {
        string strWhere = "";

        #region Organization
        //type 0 all 1 Region 2 Division 3 Branch 4 LoanOfficer
        if (ddlOrganizationTypes.SelectedIndex > 0 && ddlOrganization.SelectedIndex > 0)
        {
            if (ddlOrganizationTypes.SelectedIndex == 1)//Region
            {
                strWhere = string.Format(@"AND (EXISTS (SELECT 1 FROM 
(SELECT UserId FROM GroupUsers gu WHERE EXISTS (SELECT 1 FROM Groups WHERE RegionID={0} AND GroupId=gu.GroupID)) t1
WHERE t.UserId=t1.UserID)", ddlOrganization.SelectedValue);

                // 如果当前用户是所选Region的Excutive，则要显示Region的所有Executive
                //if (ROLEID_EXECUTIVE == CurrUser.iRoleID)   // current user is a executive
                //{
                //    strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM RegionExecutives WHERE RegionId " 
                //        + "IN(SELECT RegionId FROM RegionExecutives WHERE ExecutiveId='{0}'))", CurrUser.iUserID);
                //}
                strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM RegionExecutives WHERE RegionId = {0})", ddlOrganization.SelectedValue);
                strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM DivisionExecutives WHERE DivisionId IN(SELECT DivisionId FROM Divisions WHERE RegionID = {0}))", ddlOrganization.SelectedValue);
                strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId IN(SELECT BranchId FROM Branches WHERE RegionID = {0}))", ddlOrganization.SelectedValue);
                strWhere += ")";
            }
            else if (ddlOrganizationTypes.SelectedIndex == 2)//Division
            {
                strWhere = string.Format(@"AND (EXISTS (SELECT 1 FROM 
(SELECT UserId FROM GroupUsers gu WHERE EXISTS (SELECT 1 FROM Groups WHERE DivisionID={0} AND GroupId=gu.GroupID)) t1
WHERE t.UserId=t1.UserID)", ddlOrganization.SelectedValue);

                // 如果当前用户是所选Region的Excutive，则要显示Region的所有Executive
                //if (ROLEID_EXECUTIVE == CurrUser.iRoleID)   // current user is a executive
                //{
                //    strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM DivisionExecutives WHERE DivisionId "
                //        + "IN(SELECT DivisionId FROM DivisionExecutives WHERE ExecutiveId='{0}'))", CurrUser.iUserID);
                //}
                strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM DivisionExecutives WHERE DivisionId = {0})", ddlOrganization.SelectedValue);
                strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId IN(SELECT BranchId FROM Branches WHERE DivisionID = {0}))", ddlOrganization.SelectedValue);
                strWhere += ")";
            }
            else if (ddlOrganizationTypes.SelectedIndex == 3)//Branch
            {
                strWhere = string.Format(@" AND (EXISTS (SELECT 1 FROM 
(SELECT UserId FROM GroupUsers gu WHERE EXISTS (SELECT 1 FROM Groups WHERE BranchID={0} AND GroupId=gu.GroupID)) t1
WHERE t.UserId=t1.UserID)", ddlOrganization.SelectedValue);
                //if (ROLEID_BRANCHMANAGER == CurrUser.iRoleID)   // current user is a branchmanager
                //{
                //    strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId "
                //        + "IN(SELECT BranchId FROM BranchManagers WHERE BranchMgrId='{0}') AND BranchId)", CurrUser.iUserID);
                //}

                strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId={0})", ddlOrganization.SelectedValue);
                strWhere += ")";
            }
            else if (ddlOrganizationTypes.SelectedIndex == 4)//Loan Officer
            {
                strWhere += " AND LoanofficerID = '" + ddlOrganization.SelectedValue + "'";
            }
        }
        #endregion Organization

        #region Lead Source
        //0 All 1 lead source 2 partner 3 referral
        if (ddlLeadSourceType.SelectedIndex > 0 && ddlLeadSource.SelectedIndex > 0)
        {
            if (ddlLeadSourceType.SelectedIndex == 1)//LeadSource
            {
                strWhere += " AND LeadSource like '%" + this.ddlLeadSource.SelectedItem.Text + "%'";
            }
            else if (ddlLeadSourceType.SelectedIndex == 2)//Partner
            {
                strWhere += " and t.Partner='" + ddlLeadSource.SelectedItem.Text + "' ";
            }
            else if (ddlLeadSourceType.SelectedIndex == 3)//Referral
            {
                strWhere += " and t.Referral='" + ddlLeadSource.SelectedItem.Text + "' ";
            }
        }
        #endregion

        //if (CurrUser.bIsCompanyExecutive)
        //{
        //    strWhere += "";
        //}
        //else if (CurrUser.bIsRegionExecutive)
        //{
        //    strWhere += " AND (t.UserId IN(SELECT UserId FROM GroupUsers WHERE GroupID IN(SELECT GroupId FROM Groups WHERE RegionID IN(SELECT RegionId FROM RegionExecutives WHERE ExecutiveId = " + CurrUser.iUserID.ToString() + ")))";
        //    strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM RegionExecutives WHERE RegionId IN (SELECT RegionId FROM RegionExecutives WHERE ExecutiveId = {0}))", CurrUser.iUserID.ToString());
        //    strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM DivisionExecutives WHERE DivisionId IN(SELECT DivisionId FROM Divisions WHERE RegionID IN (SELECT RegionId FROM RegionExecutives WHERE ExecutiveId = {0})))", CurrUser.iUserID.ToString());
        //    strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId IN(SELECT BranchId FROM Branches WHERE RegionID IN (SELECT RegionId FROM RegionExecutives WHERE ExecutiveId = {0})))", CurrUser.iUserID.ToString());
        //    strWhere += ")";
        //}
        //else if (CurrUser.bIsDivisionExecutive)
        //{
        //    strWhere += " AND (t.UserId IN(SELECT UserId FROM GroupUsers WHERE GroupID IN(SELECT GroupId FROM Groups WHERE DivisionID IN(SELECT DivisionId FROM DivisionExecutives WHERE ExecutiveId = " + CurrUser.iUserID.ToString() + ")))";
        //    strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM DivisionExecutives WHERE DivisionId IN (SELECT DivisionId FROM DivisionExecutives WHERE ExecutiveId ={0}))", CurrUser.iUserID.ToString());
        //    strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId IN(SELECT BranchId FROM Branches WHERE DivisionID IN(SELECT DivisionId FROM DivisionExecutives WHERE ExecutiveId ={0})))", CurrUser.iUserID.ToString());

        //    strWhere += ")";
        //}
        //else if (CurrUser.bIsBranchManager)
        //{
        //    strWhere += " AND (t.UserId IN(SELECT UserId FROM GroupUsers WHERE GroupID IN(SELECT GroupId FROM Groups WHERE BranchID IN(SELECT BranchId FROM BranchManagers WHERE BranchMgrId = " + CurrUser.iUserID.ToString() + ")))";
        //    strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId IN (SELECT BranchId FROM BranchManagers WHERE BranchMgrId ={0}))", CurrUser.iUserID.ToString());
        //    strWhere += ")";
        //}
        //else
        //{
        //    strWhere += " AND (t.UserId =" + CurrUser.iUserID.ToString() + ")";
        //}

        //Add By Alex 20111023 
        strWhere += string.Format(" AND t.ContactId in (select ContactId from LoanContacts where (ContactRoleId=dbo.lpfn_GetBorrowerRoleId()  OR ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId()) AND FileId in (SELECT LoanID FROM dbo.[lpfn_GetUserLoans2] ('{0}', '{1}')))", CurrUser.iUserID, CurrUser.bAccessOtherLoans);
        
        //gdc CR47
        //strWhere += string.Format(" AND t.ContactId in (select ContactId from LoanContacts where (ContactRoleId=dbo.lpfn_GetBorrowerRoleId()  OR ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId()) AND FileId in (SELECT LoanID FROM dbo.[lpfn_GetUserLeads] ('{0}', '{1}')))", CurrUser.iUserID, CurrUser.bAccessOtherLoans);
        if (!CurrUser.userRole.AccessUnassignedLeads)
        {
            strWhere += " AND LoanofficerId <> 0 AND LoanofficerId <> -1 ";
        }



        if (this.ddlAlphabets.SelectedIndex > 0)
        {
            if (strWhere.Length > 0)
            {
                strWhere = string.Format("{0} AND LastName LIKE '{1}%'", strWhere, this.ddlAlphabets.SelectedValue);
            }
            else
            {
                strWhere = string.Format("AND LastName LIKE '{0}%'", ddlAlphabets.SelectedValue);
            }
        }

        if (this.ddlProspectStatus.SelectedIndex == 0)
        {
            strWhere += " AND Status1 = 'Active'";
        }
        else if (this.ddlProspectStatus.SelectedIndex == 1)
        {
        }
        else if (this.ddlProspectStatus.SelectedIndex == 2)
        {
            strWhere += " AND Status1 = 'Archived'";
        }

        if (this.ddlStatus.SelectedValue != "-1" && this.ddlStatus.SelectedValue != "0")
        {
            strWhere += " AND Status like '%" + this.ddlStatus.SelectedValue + "%'";
        }
        if (txbStartDate.Text.Trim().Length > 0)
        {
            try
            {
                DateTime.Parse(txbStartDate.Text.Trim()).ToShortDateString();
                strWhere += " AND Created >= '" + DateTime.Parse(txbStartDate.Text.Trim()).ToShortDateString() + "'";
            }
            catch
            { }
        }

        if (txbEndDate.Text.Trim().Length > 0)
        {
            try
            {
                DateTime.Parse(txbEndDate.Text.Trim()).ToShortDateString();
                strWhere += " AND Created <= '" + DateTime.Parse(txbEndDate.Text.Trim()).AddDays(1).ToShortDateString() + "'";
            }
            catch
            { }
        }
        #region Search return Condition
        if (this.hfloanOfficer.Value != "0" && this.hfloanOfficer.Value != "")
        {
            strWhere += " AND (UserId =" + this.hfloanOfficer.Value + ")";
        }
        if (this.hfrefCode.Value.Trim() != "")
        {
            strWhere += " AND (ReferenceCode like '%" + this.hfrefCode.Value.Replace("'", "''") + "%')";
        }
        if (this.hfstatus.Value != "0" && this.hfstatus.Value != "")
        {
            strWhere += " AND (Status like '%" + this.hfstatus.Value + "%')";
        }

        if (this.hfleadSource.Value.Trim() != "")
        {
            strWhere += " AND (LeadSource like '%" + this.hfleadSource.Value.Replace("'", "''") + "%')";
        }
        if (this.hflastName.Value.Trim() != "")
        {
            strWhere += " AND (Lastname like '%" + this.hflastName.Value.Replace("'", "''") + "%')";
        }
        if (this.hfaddress.Value.Trim() != "")
        {
            strWhere += " AND (MailingAddr like '%" + this.hfaddress.Value + "%')";
        }
        if (this.hfcity.Value.Trim() != "")
        {
            strWhere += " AND (MailingCity like '%" + this.hfcity.Value + "%')";
        }
        if (this.hfstate.Value != "0" && this.hfstatus.Value != "")
        {
            strWhere += " AND (MailingState like '%" + this.hfstate.Value + "%')";
        }
        if (this.hfzip.Value.Trim() != "")
        {
            strWhere += " AND (MailingZip like '%" + this.hfzip.Value + "%')";
        }
        #endregion

        return strWhere;
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
        NewRegionRow["Name"] = "All organizations";
        RegionListData.Rows.InsertAt(NewRegionRow, 0);

        ddlOrganization.DataTextField = "Name";
        ddlOrganization.DataValueField = "RegionID";
        this.ddlOrganization.DataSource = RegionListData;
        this.ddlOrganization.DataBind();
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
        NewDivisionRow["Name"] = "All organizations";
        DivisionListData.Rows.InsertAt(NewDivisionRow, 0);


        ddlOrganization.DataTextField = "Name";
        ddlOrganization.DataValueField = "DivisionID";
        ddlOrganization.DataSource = DivisionListData;
        ddlOrganization.DataBind();
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
        NewBranchRow["Name"] = "All organizations";
        BranchListData.Rows.InsertAt(NewBranchRow, 0);

        ddlOrganization.DataTextField = "Name";
        ddlOrganization.DataValueField = "BranchID";
        this.ddlOrganization.DataSource = BranchListData;
        this.ddlOrganization.DataBind();
    }

    private void BindLoanOfficer()
    {
        LPWeb.BLL.Loans LoanManager = new LPWeb.BLL.Loans();
        DataTable dt = null;

        dt = LoanManager.GetAllLoanOfficerInfo();

        DataRow NewBranchRow = dt.NewRow();
        NewBranchRow["UserId"] = "-1";
        NewBranchRow["LoanOfficer"] = "All organizations";
        dt.Rows.InsertAt(NewBranchRow, 0);

        ddlOrganization.DataTextField = "LoanOfficer";
        ddlOrganization.DataValueField = "UserId";
        this.ddlOrganization.DataSource = dt;
        this.ddlOrganization.DataBind();
    }

    private void BindLeadSources()
    {
        LPWeb.BLL.Company_Lead_Sources LeadSourcesManager = new Company_Lead_Sources();
        DataTable SourceListData = null;
        DataSet dsSource = null;

        dsSource = LeadSourcesManager.GetList(" 1=1 order by LeadSource");
        SourceListData = dsSource.Tables[0];

        // add "All Lead Sources" row
        DataRow NewSourceRow = SourceListData.NewRow();
        NewSourceRow["LeadSourceId"] = "-1";
        NewSourceRow["LeadSource"] = "All";
        SourceListData.Rows.InsertAt(NewSourceRow, 0);

        ddlLeadSource.DataTextField = "LeadSource";
        ddlLeadSource.DataValueField = "LeadSourceId";

        ddlLeadSource.DataSource = SourceListData;
        ddlLeadSource.DataBind();
    }

    private void BindPartnerInfo()
    {
        DataTable dt = _bllProspect.GetReferralCompanies();
        DataRow dr = dt.NewRow();
        dr["Partner"] = "All";
        dr["ContactCompanyId"] = "-1";
        dt.Rows.InsertAt(dr, 0);

        ddlLeadSource.DataTextField = "Partner";
        ddlLeadSource.DataValueField = "ContactCompanyId";

        ddlLeadSource.DataSource = dt;
        ddlLeadSource.DataBind();
    }

    private void BindReferralInfo()
    {
        DataTable dt = _bllProspect.GetReferralContactInfo();
        DataRow dr = dt.NewRow();
        dr["Referral"] = "All";
        dr["ContactId"] = "-1";
        dt.Rows.InsertAt(dr, 0);

        ddlLeadSource.DataTextField = "Referral";
        ddlLeadSource.DataValueField = "ContactId";

        ddlLeadSource.DataSource = dt;
        ddlLeadSource.DataBind();
    }

    /// <summary>
    /// Bind Grid
    /// </summary>
    private void BindLoanGrid()
    {
        // Get user branch info
        dtUserBranch = UsersManager.GetUserBranchInfo();

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

        DataSet ProspectLists = null;
        try
        {
            ProspectLists = _bllProspect.GetList(pageSize, pageIndex, queryCondition, out recordCount, OrderName, OrderType);
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

        #region Base64 QueryCondition SQL To HiddenField For Export

        try
        {
            hidFilterQueryCondition.Value = Encrypter.Base64Encode(queryCondition).Replace("+", "_99_");
            hidrecordTotal.Value = recordCount.ToString();
        }
        catch { }
        #endregion

        AspNetPager1.PageSize = pageSize;
        AspNetPager1.RecordCount = recordCount;
        AspNetPager1.CurrentPageIndex = pageIndex;
        //得到TaskAlertID
        GetTaskAlertprospectLists(ref ProspectLists);
        //得到Branch
        GetBranchInfo(ref ProspectLists);
        gvPropectView.DataSource = ProspectLists;
        gvPropectView.DataBind();

        UserColumn(gvPropectView);
    }

    private void UserColumn(GridView gridView)
    {
        bool defaultValue = false;
        LPWeb.Model.UserProspectColumns modUPC = null;
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

        gridView.Columns[1].Visible = modUPC == null ? !defaultValue : modUPC.Pv_Created;
        //2 Client
        //3 Alert
        gridView.Columns[4].Visible = false;           // progress bar should be hidden, by customer request
        //gridView.Columns[4].Visible = modUPC == null ? !defaultValue : modUPC.Pv_Progress;
        // 5 Status
        gridView.Columns[6].Visible = modUPC == null ? !defaultValue : modUPC.Pv_Leadsource;
        gridView.Columns[7].Visible = modUPC == null ? !defaultValue : modUPC.Pv_Refcode;
        gridView.Columns[8].Visible = modUPC == null ? !defaultValue : modUPC.Pv_Loanofficer;
        gridView.Columns[9].Visible = modUPC == null ? !defaultValue : modUPC.Pv_Branch;
        gridView.Columns[10].Visible = modUPC == null ? !defaultValue : modUPC.Pv_Partner;
        gridView.Columns[11].Visible = modUPC == null ? !defaultValue : modUPC.Pv_Referral;

    }

    private void GetTaskAlertprospectLists(ref DataSet ProspectLists)
    {
        if (ProspectLists == null)
        {
            return;
        }
        DataTable dt = ProspectLists.Tables[0];
        if (!dt.Columns.Contains("AlertID"))
        {
            dt.Columns.Add("AlertID");

            foreach (DataRow dr in dt.Rows)
            {
                dr["AlertID"] = _prospectAlerts.GetTaskAlertID(Convert.ToInt32(dr["Contactid"]));
            }
        }
        dt.AcceptChanges();
    }

    private void GetBranchInfo(ref DataSet ProspectLists)
    {
        if (ProspectLists == null)
        {
            return;
        }
        DataTable dt = ProspectLists.Tables[0];

        foreach (DataRow dr in dt.Rows)
        {
            StringBuilder sbBName = new StringBuilder();
            if (dr["UserID"] != DBNull.Value && dr["UserID"].ToString() != "")
            {
                DataRow[] drs = dtUserBranch.Select(string.Format("UserId={0}", dr["UserID"].ToString()));
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
                    dr["Branch"] = sbBName.ToString().Substring(0, nDisLen) + "...";
                }
                else
                    dr["Branch"] = sbBName.ToString();
            }
        }

        dt.AcceptChanges();
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
                ViewState["orderName"] = "Client";
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
                    _bllProspect.Delete(iItem);
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }
            }
        }
    }
    #endregion

    #region Event
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        CheackIsChangeFiler(); //gdc CR45
        StageFilter = string.Empty;
        isReset = true;
        BindLoanGrid();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        StageFilter = string.Empty;
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

    /// <summary>
    /// Handles the Sorting event of the gvPropectView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
    protected void gvPropectView_Sorting(object sender, GridViewSortEventArgs e)
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

    /// <summary>
    /// Set selected row when click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvPropectView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (DataControlRowType.DataRow == e.Row.RowType)
        //{
        //    int nFileId = 0;
        //    string strStatus = string.Format("{0}", gvPropectView.DataKeys[e.Row.RowIndex]["Status"]);

        //    if (null != gvPropectView.DataKeys[e.Row.RowIndex])
        //    {
        //        if (!int.TryParse(gvPropectView.DataKeys[e.Row.RowIndex].Value.ToString(), out nFileId))
        //            nFileId = 0;

        //        if (0 != nFileId)
        //        {
        //           // sbAllLIdStatus.AppendFormat("allLoan.push(new SelectedLoan('{0}', '{1}', '{2}'));", nFileId, strStatus, strBranchId);
        //        }
        //    }
        //}

        //if (DataControlRowType.DataRow == e.Row.RowType)
        //{
        //    Label lblBranch = e.Row.FindControl("lblBranch") as Label;
        //    CheckBox ckbChecked = e.Row.FindControl("ckbSelected") as CheckBox;
        //    string sUserID = "";
        //    if (null != gvPropectView.DataKeys[e.Row.RowIndex])
        //    {
        //        sUserID = string.Format("{0}", gvPropectView.DataKeys[e.Row.RowIndex]["UserId"]);

        //        if ("" != sUserID)
        //        {

        //            string UserId = string.Format("{0}", gvPropectView.DataKeys[e.Row.RowIndex]["UserId"]);
        //            if (null != lblBranch && null != dtUserBranch)
        //            {
        //                // concatenates all user branch names, using "," between each name
        //                StringBuilder sbBName = new StringBuilder();
        //                DataRow[] drs = dtUserBranch.Select(string.Format("UserId={0}", sUserID));
        //                if (null != drs)
        //                {
        //                    foreach (DataRow row in drs)
        //                    {
        //                        if (sbBName.Length > 0)
        //                            sbBName.Append(", ");
        //                        sbBName.Append(row["Name"]);
        //                    }
        //                }
        //                int nDisLen = 30;
        //                if (sbBName.Length > nDisLen)
        //                {
        //                    lblBranch.ToolTip = sbBName.ToString();
        //                    lblBranch.Text = sbBName.ToString().Substring(0, nDisLen) + "...";
        //                }
        //                else
        //                    lblBranch.Text = sbBName.ToString();
        //            }
        //        }
        //    }
        //}
    }

    protected void gvPropectView_PreRender(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "registerLOIds", sbAllLIdStatus.ToString(), true);
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
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDispose_Click(object sender, EventArgs e)
    {
        int nFileId = -1;
        if (!int.TryParse(this.hiSelectedLoan.Value, out nFileId))
            nFileId = -1;
        if (nFileId == -1)
        {
            LPLog.LogMessage(LogType.Logerror, "Invalid file Id: " + this.hiSelectedLoan.Value);
            return;
        }

        try
        {
            if (this.hiSelectedDisposal.Value.ToString() == "")
            {
                return;
            }
            string sError_UpdateProspectLoanStatus = string.Empty;
            sError_UpdateProspectLoanStatus = WorkflowManager.UpdateProspectAndLoanProspectLoanStatus(nFileId, this.hiSelectedDisposal.Value.ToString(), CurrUser.iUserID);
            //if (WorkflowManager.UpdateProspectLoanStatus(nFileId, this.hiSelectedDisposal.Value.ToString(), CurrUser.iUserID))
            //{
            //    BindLoanGrid();
            //    LPLog.LogMessage(LogType.Loginfo, string.Format("Successfully update prospect status, LoanId:{0}, to Status:{1}. ",
            //        nFileId, this.hiSelectedDisposal.Value));
            //}
            if (sError_UpdateProspectLoanStatus != string.Empty)
            {
                PageCommon.AlertMsg(this, "Failed to update prospect status.");
                LPLog.LogMessage(LogType.Logerror, string.Format("Failed to update prospect status, LoanId:{0}, to Status:{1}.",
                    nFileId, this.hiSelectedDisposal.Value));
            }

        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ee.Message));
            PageCommon.AlertMsg(this, "Failed to update prospect status.");
        }
        catch (Exception ex)
        {
            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ex.Message));
            PageCommon.AlertMsg(this, ex.Message);
        }
    }

    protected void ddlOrganizationTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        isReset = true;
        _curLoginUser = new LoginUser();

        if (ddlOrganizationTypes.SelectedIndex == 0)
        {
            ddlOrganization.Items.Clear();
            ddlOrganization.Items.Add(new ListItem("All organizations", "0"));
        }
        else if (ddlOrganizationTypes.SelectedIndex == 1)
        {
            BindRegions(_curLoginUser);
        }
        else if (ddlOrganizationTypes.SelectedIndex == 2)
        {
            BindDivisions(_curLoginUser, "0");
        }
        else if (ddlOrganizationTypes.SelectedIndex == 3)
        {
            BindBranches(_curLoginUser, "0", "0");
        }
        else if (ddlOrganizationTypes.SelectedIndex == 4)
        {
            BindLoanOfficer();
        }
    }

    protected void ddlLeadSourceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        isReset = true;
        _curLoginUser = new LoginUser();

        if (ddlLeadSourceType.SelectedIndex == 0)
        {
            ddlLeadSource.Items.Clear();
            ddlLeadSource.Items.Add(new ListItem("All", "0"));
        }
        else if (ddlLeadSourceType.SelectedIndex == 1)
        {
            BindLeadSources();
        }
        else if (ddlLeadSourceType.SelectedIndex == 2)
        {
            BindPartnerInfo();
        }
        else //if (ddlLeadSourceType.SelectedIndex == 3)
        {
            BindReferralInfo();
        }
    }

    protected void ddlProspectStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        isReset = true;
        BindLoanGrid();
    }
    #endregion

    #region CR45 

    //gdc CR45


    /// <summary>
    /// 
    /// </summary>
    private void BindUserPiplineView()
    {
        LPWeb.BLL.UserPipelineViews bll = new UserPipelineViews();

        ddlUserPipelineView.DataSource = bll.GetList_ViewName("PipelineType='Clients' AND Enabled = 1 AND UserID = " + CurrUser.iUserID.ToString(), "ViewName ASC");
        ddlUserPipelineView.DataBind();

        ddlUserPipelineView.Items.Insert(0, new ListItem() { Selected = true, Text = "--select--", Value = "0" });

        LPWeb.BLL.UserHomePref bllUHP = new UserHomePref();

        if (!IsPostBack && string.IsNullOrEmpty(Request.QueryString["q"]))
        {
            var model = bllUHP.GetModel(CurrUser.iUserID);
            if (model != null && model.UserId == CurrUser.iUserID)
            {
                ddlUserPipelineView.SelectedValue = model.DefaultClientsPipelineViewId.ToString();
                ddlUserPipelineView_SelectedIndexChanged(ddlUserPipelineView, new EventArgs());
            }
        }

    }


    /// <summary>
    /// UserPipelineView  Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlUserPipelineView_SelectedIndexChanged(object sender, EventArgs e)
    {

        // Load View Filer

        if (!string.IsNullOrEmpty(ddlUserPipelineView.SelectedValue))
        {
            SetUserPipelineView(Convert.ToInt32(ddlUserPipelineView.SelectedValue));
        }
        else
        {
            BindDefaultFiler();
        }

    }

    /// <summary>
    /// Bind Default Value
    /// </summary>
    /// <param name="userPipelineViewID"></param>
    private void SetUserPipelineView(int userPipelineViewID)
    {
        if (userPipelineViewID <= 0)
        {
            BindDefaultFiler();

        }
        else
        {
            #region LoadUPV
            LPWeb.BLL.UserPipelineViews bll = new UserPipelineViews();

            var model = bll.GetModel(userPipelineViewID);


            if (model != null && model.UserId == CurrUser.iUserID)
            {
                try
                {
                    ddlProspectStatus.SelectedValue = string.IsNullOrEmpty(model.ViewFilter) ? "" : model.ViewFilter;
                    ddlProspectStatus.Attributes["UPV"] = string.IsNullOrEmpty(model.ViewFilter) ? "" : model.ViewFilter;
                    ddlProspectStatus_SelectedIndexChanged(ddlProspectStatus, new EventArgs());
                    //sViewType = model.ViewFilter;

                    ddlOrganizationTypes.SelectedValue = model.OrgTypeFilter;
                    ddlOrganizationTypes.Attributes["UPV"] = string.IsNullOrEmpty(model.OrgTypeFilter) ? "" : model.OrgTypeFilter;
                    ddlOrganizationTypes_SelectedIndexChanged(ddlOrganizationTypes, new EventArgs());

                    ddlOrganization.SelectedValue = model.OrgFilter;
                    ddlOrganization.Attributes["UPV"] = string.IsNullOrEmpty(model.OrgFilter) ? "" : model.OrgFilter;

                    ddlStatus.SelectedValue = model.StageFilter;
                    ddlStatus.Attributes["UPV"] = string.IsNullOrEmpty(model.StageFilter) ? "" : model.StageFilter;

                    ddlLeadSourceType.SelectedValue = model.ContactTypeFilter;
                    ddlLeadSourceType.Attributes["UPV"] = string.IsNullOrEmpty(model.ContactTypeFilter) ? "" : model.ContactTypeFilter;
                    ddlLeadSourceType_SelectedIndexChanged(ddlLeadSourceType, new EventArgs());

                    ddlLeadSource.SelectedValue = model.ContactFilter;
                    ddlLeadSource.Attributes["UPV"] = string.IsNullOrEmpty(model.ContactFilter) ? "" : model.ContactFilter;

                    //ddlDataType.SelectedValue = model.DateTypeFilter;
                    //ddlDataType.Attributes["UPV"] = model.DateTypeFilter;

                    #region DateFilter
                    if (!string.IsNullOrEmpty(model.DateFilter))
                    {
                        var item = model.DateFilter.Split(',');
                        txbStartDate.Attributes["UPV"] = "";
                        txbStartDate.Text = "";
                        if (item.Count() >= 1 && !string.IsNullOrEmpty(item.FirstOrDefault()))
                        {
                            txbStartDate.Text = item.FirstOrDefault();
                            txbStartDate.Attributes["UPV"] = item.FirstOrDefault();
                        }
                        txbEndDate.Attributes["UPV"] = "";
                        txbEndDate.Text = "";
                        if (item.Count() == 2 && !string.IsNullOrEmpty(item.LastOrDefault()))
                        {
                            txbEndDate.Text = item.LastOrDefault();
                            txbEndDate.Attributes["UPV"] = item.LastOrDefault();
                        }

                    }
                    #endregion
                }
                catch { }

            }
            #endregion
        }

        this.btnFilter_Click(this.btnFilter, new EventArgs());

    }

    private void BindDefaultFiler()
    {
        ddlProspectStatus.SelectedIndex = 0;


        ddlOrganizationTypes.SelectedIndex = 0;
        ddlOrganizationTypes_SelectedIndexChanged(ddlOrganizationTypes, new EventArgs());

        ddlOrganization.SelectedIndex = 0;

        ddlStatus.SelectedIndex = 0;

        ddlLeadSourceType.SelectedIndex = 0;

        ddlLeadSource.SelectedIndex = 0;

        //ddlDataType.SelectedIndex = 0;

        txbEndDate.Text = "";
        txbStartDate.Text = "";
    }


    private void CheackIsChangeFiler()
    {
        if (ddlProspectStatus.Attributes["UPV"] != ddlProspectStatus.SelectedValue
            || ddlOrganizationTypes.Attributes["UPV"] != ddlOrganizationTypes.SelectedValue
            || ddlOrganization.Attributes["UPV"] != ddlOrganization.SelectedValue
            || ddlStatus.Attributes["UPV"] != ddlStatus.SelectedValue
            || ddlLeadSourceType.Attributes["UPV"] != ddlLeadSourceType.SelectedValue
            || ddlLeadSource.Attributes["UPV"] != ddlLeadSource.SelectedValue
            //|| ddlDataType.Attributes["UPV"] != ddlDataType.SelectedValue
            || txbStartDate.Attributes["UPV"] != txbStartDate.Text
            || txbEndDate.Attributes["UPV"] != txbEndDate.Text
            )
        {
            ddlUserPipelineView.SelectedValue = "0";
        }

    }


    protected void btnSaveView_OnClick(object sender, EventArgs e)
    {
        var viewName = txtSaveViewName.Text.Trim().Replace("'", "");
        LPWeb.BLL.UserPipelineViews bll = new UserPipelineViews();
        LPWeb.Model.UserPipelineViews model = new LPWeb.Model.UserPipelineViews();
        int ID = 0;
        var ds = bll.GetList_ViewName("ViewName ='" + viewName + "'" + " AND PipelineType='Clients'  AND UserId =" + CurrUser.iUserID, "");

        if (ds.Tables[0].Rows.Count > 0)
        {
            ID = ds.Tables[0].Rows[0]["UserPipelineViewID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["UserPipelineViewID"]) : 0;
        }

        model.ViewName = viewName;
        model.ContactFilter = ddlLeadSource.SelectedValue.Trim();
        model.ContactTypeFilter = ddlLeadSourceType.SelectedValue.Trim();
        model.DateFilter = txbStartDate.Text.Trim() + "," + txbEndDate.Text.Trim();
       // model.DateTypeFilter = ddlDataType.SelectedValue.Trim();

        model.Enabled = true;
        model.OrgFilter = ddlOrganization.SelectedValue.Trim();
        model.OrgTypeFilter = ddlOrganizationTypes.SelectedValue.Trim();
        model.PipelineType = "Clients";
        model.StageFilter = ddlStatus.SelectedValue.Trim();
        model.UserId = CurrUser.iUserID;
        model.ViewFilter = ddlProspectStatus.SelectedValue.Trim();

        model.UserPipelineViewID = ID;


        if (ID != 0)
        {
            bll.Update(model);
        }
        else
        {
            bll.Add(model);

            ds = bll.GetList_ViewName("ViewName ='" + viewName + "'" + " AND PipelineType='Clients'  AND UserId =" + CurrUser.iUserID, "");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ID = ds.Tables[0].Rows[0]["UserPipelineViewID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["UserPipelineViewID"]) : 0;
            }

            BindUserPiplineView();

            ddlUserPipelineView.SelectedValue = ID.ToString();

            SetUserPipelineView(ID);
        }




    }

    #endregion

}
