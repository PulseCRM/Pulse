using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using Utilities;
using LPWeb.LP_Service;
using System.Web.UI;
using LPWeb.BLL;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    /// <summary>
    /// Prospect Pipeline Summary - Loan View
    /// Author: Peter
    /// Date: 2011-02-20
    /// </summary>
    public partial class ProspectPipelineSummaryLoan : BasePage
    {
        LoginUser _curLoginUser = new LoginUser();
        BLL.UserProspectColumns upcManager = new BLL.UserProspectColumns();
        BLL.Loans LoansManager = new BLL.Loans();
        private readonly BLL.Prospect _bllProspect = new BLL.Prospect();
        BLL.LoanAlerts loanAlertMngr = new BLL.LoanAlerts();
        BLL.Template_Stages TpltStages = new BLL.Template_Stages();
        BLL.LoanStages loanStages = new BLL.LoanStages();
        private UserRecentItems _bllUserRecentItems = new UserRecentItems();

        public string FromURL = string.Empty;
        private bool isReset = false;
        protected string sHasViewRight = "0";
        private string fromHomeFilter = string.Empty;
        private string StageFilter;
        private string sViewType = "";
        private bool FirstTimeFlag = false;
        int viewstateIdx = 0;
        int currentpageIdx = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Url != null)
            {
                FromURL = Request.Url.ToString();
            }
            _curLoginUser = new LoginUser();
            //是否有View权限
            sHasViewRight = _curLoginUser.userRole.Loans.ToString().IndexOf('D') > 0 ? "1" : "0";  //View

            sViewType = Request.QueryString["type"] == null ? "" : Request.QueryString["type"].ToString();

            FirstTimeFlag = false;

            if (!IsPostBack)
            {
                try
                {
                    FirstTimeFlag = true;
                    viewstateIdx = 0;
                    currentpageIdx = 0;

                    //权限验证 
                    if (_curLoginUser.userRole.Loans.ToString().Trim().Length > 0) // Has Rights
                    {
                        if (_curLoginUser.userRole.Loans.ToString().IndexOf('C') == -1)
                        {
                            lbtnRemove.Enabled = false;
                        }
                        if (_curLoginUser.userRole.Loans.ToString().IndexOf('D') == -1)
                        {
                            lbtnDetail.Enabled = false;
                        }
                        if (_curLoginUser.userRole.Loans.ToString().IndexOf('E') == -1)
                        {
                            lbtnSearch.Enabled = false;
                        }
                        if (_curLoginUser.userRole.Loans.ToString().IndexOf('F') == -1)
                        {
                            lbtnDispose.Enabled = false;
                        }
                        if (_curLoginUser.userRole.Loans.ToString().IndexOf('G') == -1)
                        {
                            lbtnSync.Enabled = false;
                        }

                        //gdc CR45
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

                    //gdc CR45

                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }



                //DataTable dtStages = TpltStages.GetStageTemplateList(" AND [Enabled]=1 AND WorkflowType='Prospect'");
                ////DataTable dtStages = loanStages.GetLoanStageAlias("Prospect");
                //this.ddlStages.DataSource = dtStages;
                //this.ddlStages.DataValueField = "TemplStageId";
                //this.ddlStages.DataTextField = "Alias";
                //this.ddlStages.DataBind();
                //this.ddlStages.Items.Insert(0, new ListItem("All Stages", "-1"));
                //this.ddlStages.Items.Add(new ListItem("Uncategorized", "-100"));

                BindFilter();

                this.DoInitData();



                fromHomeFilter = FilterFromHomePiplineSummary();

                if (sViewType != "")
                {
                    ddlViewType.SelectedValue = sViewType;
                }

                //Lead_FirstPage_BindGrid();
                BindGrid();

            }
        }

        /// <summary>
        /// load dropdown list data
        /// </summary>
        private void DoInitData()
        {
            try
            {
                DataSet dsFilter = PageCommon.GetOrgStructureDataSourceByLoginUser(null, null, true);
                this.ddlRegion.DataSource = dsFilter.Tables[0];
                this.ddlRegion.DataValueField = "RegionId";
                this.ddlRegion.DataTextField = "Name";
                this.ddlRegion.DataBind();

                this.ddlDivision.DataSource = dsFilter.Tables[1];
                this.ddlDivision.DataValueField = "DivisionId";
                this.ddlDivision.DataTextField = "Name";
                this.ddlDivision.DataBind();

                this.ddlBranch.DataSource = dsFilter.Tables[2];
                this.ddlBranch.DataValueField = "BranchId";
                this.ddlBranch.DataTextField = "Name";
                this.ddlBranch.DataBind();

                this.ddlRegion.SelectedIndex = 0;
                this.ddlDivision.SelectedIndex = 0;
                this.ddlBranch.SelectedIndex = 0;

                #region Context Menu

                //加载 ArchiveLeadStatus
                StringBuilder sbSubMenuItems = new StringBuilder();
                ArchiveLeadStatus _bArchiveLeadStatus = new ArchiveLeadStatus();
                DataSet dsAS = _bArchiveLeadStatus.GetList("isnull([Enabled],0)=1 order by LeadStatusName");

                sbSubMenuItems.AppendLine("<ul style='width:200px '>");
                foreach (DataRow dr in dsAS.Tables[0].Rows)
                {                                                                                                   //modify by gdc 20111213 解决'符号 会引起js错误问题
                    sbSubMenuItems.AppendLine("<li><a href=\"\" onclick=\"DisposeLoan('" + dr["LeadStatusName"].ToString().Replace("'", "\\'") + "'); return false;\">" + dr["LeadStatusName"].ToString() + "</a></li>");
                }
                sbSubMenuItems.AppendLine("</ul>");

                StringBuilder sbMenuItems = new StringBuilder();

                sbMenuItems.AppendLine("<li id='Archive' ><a href=\"\" onclick=\"return false;\">Archive</a>" + sbSubMenuItems + "</li>");
                sbMenuItems.AppendLine("<li id='Convert'><a href=\"\" onclick=\"DisposeLoan('Convert'); return false;\">Convert to active loan</a></li>");
                sbMenuItems.AppendLine("<li id='Resume'><a href=\"\" onclick=\"DisposeLoan('Resume***Active'); return false;\">Resume</a></li>");
                this.ltrMenuItems.Text = sbMenuItems.ToString();


                #endregion


                BindUserPiplineView();//gdc CR45

                BindStages(); //Rocky CR54

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindFilter()
        {
            ddlOrganization.Items.Clear();
            ddlOrganization.Items.Add(new ListItem("All organizations", "0"));
            ddlLeadSource.Items.Clear();
            ddlLeadSource.Items.Add(new ListItem("All", "0"));
        }

        /// <summary>
        /// Get division data
        /// </summary>
        /// <returns></returns>
        private DataTable GetDivisionData(string sRegionID)
        {
            try
            {
                int? nRegion = null;
                int nTemp = -1;
                if (!int.TryParse(sRegionID, out nTemp))
                    nTemp = -1;
                if (nTemp != -1)
                    nRegion = nTemp;
                DataTable dtDivision = PageCommon.GetOrgStructureDataSourceByLoginUser(nRegion, null, true).Tables[1];
                return dtDivision;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Branch data
        /// </summary>
        /// <returns></returns>
        private DataTable GetBranchData(string sRegionID, string sDivisionID)
        {
            try
            {
                int? nDivision = null;
                int nTemp = -1;
                if (!int.TryParse(sDivisionID, out nTemp))
                    nTemp = -1;
                if (nTemp != -1)
                    nDivision = nTemp;

                int? nRegionID = null;
                int nTemp2 = -1;
                if (!int.TryParse(sRegionID, out nTemp2))
                    nTemp2 = -1;
                if (nTemp2 != -1)
                    nRegionID = nTemp2;
                DataTable dtBranches = PageCommon.GetOrgStructureDataSourceByLoginUser(nRegionID, nDivision, true).Tables[2];
                return dtBranches;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 得到所有Loans 引用的Region信息
        /// </summary>
        /// <returns></returns>
        private DataTable GetRegionDataInUsed()
        {
            Regions _bRegions = new Regions();
            DataTable dtRegion = _bRegions.GetRegionList("RegionID in (select RegionID from dbo.lpvw_PipelineInfo)");
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("ID");
            dtRst.Columns.Add("Name");
            DataRow dr;
            foreach (DataRow drRegion in dtRegion.Rows)
            {
                dr = dtRst.NewRow();
                dr["ID"] = drRegion["RegionID"].ToString();
                dr["Name"] = drRegion["Name"].ToString();
                dtRst.Rows.Add(dr);
            }
            dr = dtRst.NewRow();
            dr["ID"] = "0";
            dr["Name"] = "All organizations";
            dtRst.Rows.InsertAt(dr, 0);
            dtRst.AcceptChanges();
            return dtRst;

        }

        /// <summary>
        /// 得到所有Loans 引用的Division信息
        /// </summary>
        /// <returns></returns>
        private DataTable GetDivisionDataInUsed()
        {
            Divisions _bDivision = new Divisions();
            DataTable dtDivision = _bDivision.GetDivisionList("DivisionId in (select DivisionId from dbo.lpvw_PipelineInfo)");
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("ID");
            dtRst.Columns.Add("Name");
            DataRow dr;
            foreach (DataRow drDivision in dtDivision.Rows)
            {
                dr = dtRst.NewRow();
                dr["ID"] = drDivision["DivisionId"].ToString();
                dr["Name"] = drDivision["Name"].ToString();
                dtRst.Rows.Add(dr);
            }
            dr = dtRst.NewRow();
            dr["ID"] = "0";
            dr["Name"] = "All organizations";
            dtRst.Rows.InsertAt(dr, 0);
            dtRst.AcceptChanges();
            return dtRst;

        }

        /// <summary>
        /// 得到所有Loans 引用的Branch信息
        /// </summary>
        /// <returns></returns>
        private DataTable GetBranchDataInUsed()
        {
            Branches _bBranches = new Branches();
            DataTable dtBranches = _bBranches.GetBranchList("BranchId in (select BranchId from dbo.lpvw_PipelineInfo)");
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("ID");
            dtRst.Columns.Add("Name");
            DataRow dr;
            foreach (DataRow drBranches in dtBranches.Rows)
            {
                dr = dtRst.NewRow();
                dr["ID"] = drBranches["BranchId"].ToString();
                dr["Name"] = drBranches["Name"].ToString();
                dtRst.Rows.Add(dr);
            }
            dr = dtRst.NewRow();
            dr["ID"] = "0";
            dr["Name"] = "All organizations";
            dtRst.Rows.InsertAt(dr, 0);
            dtRst.AcceptChanges();
            return dtRst;

        }

        /// <summary>
        /// 得到所有Loans 引用的Branch信息
        /// </summary>
        /// <returns></returns>
        private DataTable GetLoanOfficerDataInUsed()
        {
            Branches _bBranches = new Branches();
            DataTable dtBranches = _bBranches.GetBranchList("BranchId in (select BranchId from dbo.lpvw_PipelineInfo)");
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("ID");
            dtRst.Columns.Add("Name");
            DataRow dr;
            foreach (DataRow drBranches in dtBranches.Rows)
            {
                dr = dtRst.NewRow();
                dr["ID"] = drBranches["BranchId"].ToString();
                dr["Name"] = drBranches["Name"].ToString();
                dtRst.Rows.Add(dr);
            }
            dr = dtRst.NewRow();
            dr["ID"] = "0";
            dr["Name"] = "All organizations";
            dtRst.Rows.InsertAt(dr, 0);
            dtRst.AcceptChanges();
            return dtRst;

        }

        private void BindRegions(LoginUser curLoginUser)
        {
            Regions RegionManager = new Regions();
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

            Divisions DivisionManager = new Divisions();
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

            Branches BrancheManager = new Branches();
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

        private void BindProcessor()
        {
            DataTable OrganListData = new DataTable();
            OrganListData.Columns.Add("UserID", typeof(string));
            OrganListData.Columns.Add("ProcessorFullName", typeof(string));

            DataRow NewOrganRow = OrganListData.NewRow();
            NewOrganRow["UserID"] = "-1";
            NewOrganRow["ProcessorFullName"] = "All organizations";
            OrganListData.Rows.Add(NewOrganRow);

            DataTable ProcessorListData = LoansManager.GetProcessorList(CurrUser.iUserID);

            foreach (DataRow ProcessorRow in ProcessorListData.Rows)
            {
                string sProcessorID = ProcessorRow["UserID"].ToString();
                string sFirstName = ProcessorRow["FirstName"].ToString();
                string sLastName = ProcessorRow["LastName"].ToString();

                string sProcessorFullName = sLastName + ", " + sFirstName;

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["UserID"] = sProcessorID;
                NewOrganRow1["ProcessorFullName"] = sProcessorFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }
            ddlOrganization.DataTextField = "ProcessorFullName";
            ddlOrganization.DataValueField = "UserID";
            this.ddlOrganization.DataSource = OrganListData;
            this.ddlOrganization.DataBind();
        }

        private void BindLoanOfficer()
        {
            Loans LoanManager = new Loans();
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
            Company_Lead_Sources LeadSourcesManager = new Company_Lead_Sources();
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

        private void BindStages()
        {
            this.ddlStages.Items.Clear();
            DataTable dtStages = TpltStages.GetStageTemplateList(" AND [Enabled]=1 AND WorkflowType='Prospect'");
            DataTable dtCompletedStageData = this.GetCompletedStages();
            this.ddlStages.Items.Add(new ListItem("All Stages", "All Stages"));
            foreach (DataRow StageTempRow in dtStages.Rows)
            {
                string sStageName = StageTempRow["Alias"].ToString();
                this.ddlStages.Items.Add(new ListItem(sStageName, "All Stages-" + sStageName));
            }
            this.ddlStages.Items.Add(new ListItem("Uncategorized", "All Stages-Uncategorized"));

            this.ddlStages.Items.Add(new ListItem("All Completed Stages", "All Completed Stages"));
            foreach (DataRow compStageRow in dtCompletedStageData.Rows)
            {
                if (compStageRow["LastCompletedStage"].ToString().Trim() == "")
                {
                    continue;
                }
                this.ddlStages.Items.Add(new ListItem(compStageRow["LastCompletedStage"].ToString(), "All Completed Stages-" + compStageRow["LastCompletedStage"].ToString()));

            }
        }

        private void Lead_FirstPage_BindGrid()
        {
            int pageSize = AspNetPager1.PageSize;
            try
            {
                if (_curLoginUser != null)
                {
                    Users users = new Users();
                    int iLoansPerPage = 0;
                    //Model.Users u = users.GetModel(_curLoginUser.iUserID);
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
                }
            }
            catch (Exception exception)
            {
                pageSize = 20;
                AspNetPager1.PageSize = 20;
                LPLog.LogMessage(exception.Message);
            }
            int pageIndex = 1;

            if (FirstTimeFlag == true)
                pageIndex = AspNetPager1.CurrentPageIndex = 1;
            else
                pageIndex = AspNetPager1.CurrentPageIndex;

            string strWhare = GetSqlWhereClause();
            int recordCount = 0;
            int recordCount2 = 0;

            DataSet loansList = null;
            DataSet leadCount = null;
            try
            {
                loansList = LoansManager.Lead_FirstPage_GetProspectListNew_Fast100(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
                if (loansList == null || loansList.Tables[0].Rows.Count == 0)
                {
                    loansList = LoansManager.GetList(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
                }
                leadCount = LoansManager.Lead_Count(pageSize, pageIndex, strWhare, out recordCount2, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount2;

            GetRuleAlertloanLists(ref loansList);
            gridList.DataSource = loansList;
            gridList.DataBind();

            ShowColumn(gridList);
        }

        /// <summary>
        /// Bind prospect loan gridview
        /// </summary>
        private void BindGrid()
        {
            int pageSize = AspNetPager1.PageSize;
            try
            {
                if (_curLoginUser != null)
                {
                    Users users = new Users();
                    int iLoansPerPage = 0;
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
                }
            }
            catch (Exception exception)
            {
                pageSize = 20;
                AspNetPager1.PageSize = 20;
                LPLog.LogMessage(exception.Message);
            }
            int pageIndex = 1;

            if (FirstTimeFlag == true)
                pageIndex = AspNetPager1.CurrentPageIndex = 1;
            else
                pageIndex = AspNetPager1.CurrentPageIndex;

            string strWhare = GetSqlWhereClause();
            int recordCount = 0;

            DataSet loansList = null;
            try
            {
                loansList = LoansManager.GetProspectListNew_Fast(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
                if (loansList == null || loansList.Tables[0].Rows.Count == 0)
                {
                    loansList = LoansManager.GetList(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
                }
                LoansManager.GetLeadData(ref loansList);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            #region Base64 strWhare SQL To HiddenField For Export

            try
            {
                hidFilterQueryCondition.Value = Encrypter.Base64Encode(strWhare).Replace("+", "_99_");
                hidrecordTotal.Value = recordCount.ToString();
            }
            catch { }
            #endregion

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            GetRuleAlertloanLists(ref loansList);
            gridList.DataSource = loansList;
            gridList.DataBind();

            ShowColumn(gridList);
        }

        private void ShowColumn(GridView gridView)
        {
            bool defaultValue = false;
            Model.UserProspectColumns modUPC = null;
            try
            {
                if (CurrUser != null)
                {
                    modUPC = upcManager.GetModel(CurrUser.iUserID);
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            gridView.Columns[1].Visible = modUPC == null ? defaultValue : modUPC.Lv_Ranking;
            gridView.Columns[4].Visible = modUPC == null ? !defaultValue : modUPC.Lv_Progress;
            gridView.Columns[6].Visible = modUPC == null ? !defaultValue : modUPC.Lv_Amount;
            gridView.Columns[7].Visible = modUPC == null ? defaultValue : modUPC.Lv_Rate;
            gridView.Columns[8].Visible = modUPC == null ? !defaultValue : modUPC.Lv_Loanofficer;
            gridView.Columns[9].Visible = modUPC == null ? !defaultValue : modUPC.Lv_Lien;
            gridView.Columns[10].Visible = modUPC == null ? !defaultValue : modUPC.Lv_Branch;
            gridView.Columns[11].Visible = modUPC == null ? defaultValue : modUPC.Lv_Loanprogram;
            gridView.Columns[12].Visible = modUPC == null ? !defaultValue : modUPC.Lv_Leadsource;
            gridView.Columns[13].Visible = modUPC == null ? defaultValue : modUPC.Lv_Refcode;
            gridView.Columns[14].Visible = modUPC == null ? !defaultValue : modUPC.Lv_Estclose;
            gridView.Columns[15].Visible = modUPC == null ? defaultValue : modUPC.Lv_Pointfilename;

            gridView.Columns[16].Visible = modUPC == null ? defaultValue : modUPC.LastCompletedStage;
            gridView.Columns[17].Visible = modUPC == null ? defaultValue : modUPC.LastStageComplDate;

            gridView.Columns[18].Visible = modUPC == null ? defaultValue : modUPC.Lv_Partner;
            gridView.Columns[19].Visible = modUPC == null ? defaultValue : modUPC.Lv_Referral;
        }

        private void GetRuleAlertloanLists(ref DataSet LoadLists)
        {
            if (LoadLists == null)
            {
                return;
            }
            DataTable dt = LoadLists.Tables[0];
            if (!dt.Columns.Contains("AlertID"))
            {
                dt.Columns.Add("AlertID");

                foreach (DataRow dr in dt.Rows)
                {
                    dr["AlertID"] = loanAlertMngr.GetRuleAlertID(Convert.ToInt32(dr["FileId"]));
                }
            }
            dt.AcceptChanges();
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            if (FirstTimeFlag == true)
            {
                ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
                return;
            }
            if (ViewState["pageIndex"] != null)
            {
                viewstateIdx = (int)ViewState["pageIndex"];
                currentpageIdx = AspNetPager1.CurrentPageIndex;
                if (viewstateIdx != currentpageIdx)
                {
                    ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
                    if (FirstTimeFlag == false)
                    {
                        BindGrid();
                    }
                }
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
                    ddlOrganizationTypes.SelectedIndex = 1;
                    this.ddlOrganization.SelectedValue = KVPs["RegionID"];
                }

                if (KVPs.ContainsKey("DivisionID") && !string.IsNullOrEmpty(KVPs["DivisionID"]))
                {
                    //searchCondition += string.Format(searchTempllage, "DivisionID", KVPs["DivisionID"]);
                    ddlOrganizationTypes.SelectedIndex = 2;
                    this.ddlOrganization.SelectedValue = KVPs["DivisionID"];
                }

                if (KVPs.ContainsKey("BranchID") && !string.IsNullOrEmpty(KVPs["BranchID"]))
                {
                    //searchCondition += string.Format(searchTempllage, "BranchID", KVPs["BranchID"]);
                    ddlOrganizationTypes.SelectedIndex = 3;
                    this.ddlOrganization.SelectedValue = KVPs["BranchID"];
                }

                if (KVPs.ContainsKey("StageID") && !string.IsNullOrEmpty(KVPs["StageID"]))
                {
                    //searchCondition += string.Format(searchTempllage, "StageID", KVPs["StageID"]);
                    this.ddlStages.SelectedValue = KVPs["StageID"];
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


                //if (KVPs.ContainsKey("Status") && !string.IsNullOrEmpty(KVPs["Status"]))
                //{
                //    searchCondition += string.Format(searchTempllage, "[Status]", KVPs["Status"]);
                //}

                if (KVPs.ContainsKey("CurrentStage") && !string.IsNullOrEmpty(KVPs["CurrentStage"]))
                {
                    string strStageFilter = "CurrentStages";
                    if (KVPs.ContainsKey("StageFilter") && !string.IsNullOrEmpty(KVPs["StageFilter"]))
                    {
                        strStageFilter = KVPs["StageFilter"];
                    }

                    if (strStageFilter == "CurrentStages")
                    {
                        //searchCondition += string.Format(searchTempllage, "[Stage]", KVPs["CurrentStage"]);
                        if (KVPs["CurrentStage"].ToUpper() == "Uncategorized".ToUpper())
                        {
                            searchCondition += " AND dbo.lpfn_GetLoanStageCount(FileId) = 0 ";

                            //设置 ddlViewType 为 All Leads
                            ddlViewType.SelectedValue = "1";
                        }
                        else
                        {
                            searchCondition += string.Format(searchTempllage, "dbo.lpfn_GetLoanStage(FileId)", KVPs["CurrentStage"]);
                        }
                        StageFilter = "'" + KVPs["CurrentStage"] + "'";

                        var item = ddlStages.Items.FindByValue("All Stages-" + KVPs["CurrentStage"]);
                        if (item != null)
                        {
                            item.Selected = true;
                        }
                    }
                    else
                    {
                        searchCondition += " and [LastCompletedStage]='" + KVPs["CurrentStage"] + "'";
                        var item = this.ddlStages.Items.FindByValue("All Completed Stages-" + KVPs["CurrentStage"]);
                        if (item != null)
                        {
                            item.Selected = true;
                        }

                    }
                }
                else
                {
                    StageFilter = string.Empty;
                }
            }
            return searchCondition;
        }
        /// <summary>
        /// Get filter
        /// </summary>
        /// <returns></returns>
        private string GetSqlWhereClause()
        {
            string strWhere = " 1>0 ";
            string strInWhere = strWhere;
            string groupIdQueryPart = GenOrgQueryCondition();
            if (!string.IsNullOrEmpty(groupIdQueryPart))
            {
                strWhere += groupIdQueryPart;

                strInWhere += groupIdQueryPart.Replace("LeadSource", "dbo.lpfn_GetProspectLeadSource(FileId)").Replace("Partner", "dbo.lpfn_GetContactCompanyName(dbo.lpfn_GetProspectReferral(FileId))").Replace("Referral", "dbo.lpfn_GetContactName(dbo.lpfn_GetProspectReferral(FileId))");
                //strInWhere += groupIdQueryPart;
            }

            strInWhere += " AND FileId IN(SELECT FileId FROM Loans WHERE 1>0";
            if (ddlViewType.SelectedValue == "0")
            {
                strWhere += string.Format(" AND ProspectLoanStatus='Active'");
                strInWhere += string.Format(" AND ProspectLoanStatus='Active'");
            }
            else if (ddlViewType.SelectedValue == "1")
            {
                strWhere += string.Format(" AND Status='Prospect'");
                strInWhere += string.Format(" AND Status='Prospect'");
            }
            else if (ddlViewType.SelectedValue == "2")
            {
                strWhere += string.Format(" AND ProspectLoanStatus<>'Active'");
                strInWhere += string.Format(" AND ProspectLoanStatus<>'Active'");
            }

            #region Organization
            //type 0 all 1 Region 2 Division 3 Branch 4 LoanOfficer
            if (ddlOrganizationTypes.SelectedIndex > 0 && ddlOrganization.SelectedIndex > 0)
            {
                if (ddlOrganizationTypes.SelectedIndex == 1)//Region
                {
                    strWhere += string.Format(" AND RegionID = {0} ", ddlOrganization.SelectedValue);
                    strInWhere += string.Format(" AND RegionID = {0} ", ddlOrganization.SelectedValue);
                }
                else if (ddlOrganizationTypes.SelectedIndex == 2)//Division
                {
                    strWhere += string.Format(" AND DivisionID = {0} ", ddlOrganization.SelectedValue);
                    strInWhere += string.Format(" AND DivisionID = {0} ", ddlOrganization.SelectedValue);
                }
                else if (ddlOrganizationTypes.SelectedIndex == 3)//Branch
                {
                    strWhere += string.Format(" AND  BranchId={0} ", ddlOrganization.SelectedValue);
                    strInWhere += string.Format(" AND  BranchId={0} ", ddlOrganization.SelectedValue);
                }
                else if (ddlOrganizationTypes.SelectedIndex == 4)//Loan Officer
                {
                    // Loan Officer
                    strWhere += string.Format(@" AND  FileId in (select FileId from LoanTeam lt inner join Roles r
	                        on lt.RoleId=r.RoleId and r.Name='Loan Officer' where lt.UserId ={0} ) ", ddlOrganization.SelectedValue);
                    strInWhere += string.Format(@" AND  FileId in (select FileId from LoanTeam lt inner join Roles r
	                        on lt.RoleId=r.RoleId and r.Name='Loan Officer' where lt.UserId ={0} ) ", ddlOrganization.SelectedValue);
                }
                else if (ddlOrganizationTypes.SelectedIndex == 5)//Processor
                {
                    strWhere += string.Format(@" AND  dbo.lpfn_GetProcessorID(FileId)={0}", ddlOrganization.SelectedValue);
                    strInWhere += string.Format(@" AND  dbo.lpfn_GetProcessorID(FileId)={0}", ddlOrganization.SelectedValue);
                }
            }
            #endregion Organization

            if (!string.IsNullOrEmpty(fromHomeFilter))
            {
                strWhere += fromHomeFilter;
                strInWhere += fromHomeFilter.Replace("[Stage]", "dbo.lpfn_GetLoanStage(FileId)");
            }

            if (!string.IsNullOrEmpty(this.ddlAlphabet.SelectedValue))
            {
                strWhere += string.Format(" AND [Borrower] Like '{0}%'", ddlAlphabet.SelectedValue);
                strInWhere += string.Format(" AND dbo.lpfn_GetBorrower(FileId) Like '{0}%'", ddlAlphabet.SelectedValue);
            }

            #region Stage

            string sStage = this.ddlStages.SelectedValue.Trim();
            if (sStage == "All Stages")
            {
                strWhere += "";
                strInWhere += "";
            }
            else if (sStage == "All Completed Stages")
            {
                DataTable dtCompletedStageData = this.GetCompletedStages();
                if (dtCompletedStageData.Rows.Count > 0)
                {
                    string sWhereStage = " AND (";
                    foreach (DataRow compStageRow in dtCompletedStageData.Rows)
                    {
                        if (compStageRow["LastCompletedStage"].ToString().Trim() == "")
                        {
                            continue;
                        }
                        sWhereStage += (sWhereStage == " AND (") ? ("[LastCompletedStage]='" + compStageRow["LastCompletedStage"].ToString().Trim() + "'") : (" OR [LastCompletedStage]='" + compStageRow["LastCompletedStage"].ToString().Trim() + "'");
                    }
                    sWhereStage += ")";

                    strWhere += sWhereStage;
                    strInWhere += sWhereStage;
                }
            }
            else if (sStage.IndexOf("All Stages-") >= 0)
            {
                sStage = sStage.Replace("All Stages-", "");
                strWhere += string.Format(" AND [Stage] = '{0}' ", sStage.Trim());
                if (sStage.Equals("Uncategorized", StringComparison.CurrentCultureIgnoreCase))
                {
                    strInWhere += string.Format(" AND dbo.lpfn_GetLoanStageCount(FileId) = 0 ", sStage);//gdc 20111120
                }
                else
                {
                    strInWhere += string.Format(" AND dbo.lpfn_GetLoanStage(FileId) = '{0}' ", sStage);
                }
            }
            else if (sStage.IndexOf("All Completed Stages-") >= 0)
            {
                sStage = sStage.Replace("All Completed Stages-", "");
                strWhere += string.Format(" AND [LastCompletedStage] = '{0}' ", sStage.Trim());
                strInWhere += string.Format(" AND [LastCompletedStage] = '{0}' ", sStage.Trim());
            }

            #endregion

            if (ddlDataType.SelectedValue == "-1")
            {
                string sTmpWhere1 = " 1>0 ";
                string sTmpWhere2 = " 1>0 ";

                if (!string.IsNullOrEmpty(this.tbEstCloseDateStart.Text))
                {
                    sTmpWhere1 += string.Format(" AND [EstClose] >= '{0}' ", this.tbEstCloseDateStart.Text);
                    sTmpWhere2 += string.Format(" AND [Created] >= '{0}' ", this.tbEstCloseDateStart.Text);
                }

                if (!string.IsNullOrEmpty(this.tbEstCloseDateEnd.Text))
                {
                    sTmpWhere1 += string.Format(" AND [EstClose] <= '{0}' ", this.tbEstCloseDateEnd.Text);
                    sTmpWhere2 += string.Format(" AND [Created] <= '{0}' ", this.tbEstCloseDateEnd.Text);
                }

                strWhere += " and ((" + sTmpWhere1 + ") or (" + sTmpWhere2 + ")) ";
                strInWhere += " and ((" + sTmpWhere1 + ") or (" + sTmpWhere2 + ")) ";
            }
            else if (ddlDataType.SelectedValue == "0")
            {
                if (!string.IsNullOrEmpty(this.tbEstCloseDateStart.Text))
                {
                    strWhere += string.Format(" AND [EstClose] >= '{0}' ", this.tbEstCloseDateStart.Text);
                    strInWhere += string.Format(" AND [EstClose] >= '{0}' ", this.tbEstCloseDateStart.Text);
                }

                if (!string.IsNullOrEmpty(this.tbEstCloseDateEnd.Text))
                {
                    strWhere += string.Format(" AND [EstClose] <= '{0}' ", this.tbEstCloseDateEnd.Text);
                    strInWhere += string.Format(" AND [EstClose] <= '{0}' ", this.tbEstCloseDateEnd.Text);
                }
            }
            else if (ddlDataType.SelectedValue == "1")
            {
                if (!string.IsNullOrEmpty(this.tbEstCloseDateStart.Text))
                {
                    strWhere += string.Format(" AND [Created] >= '{0}' ", this.tbEstCloseDateStart.Text);
                    strInWhere += string.Format(" AND [Created] >= '{0}' ", this.tbEstCloseDateStart.Text);
                }

                if (!string.IsNullOrEmpty(this.tbEstCloseDateEnd.Text))
                {
                    strWhere += string.Format(" AND [Created] <= '{0}' ", this.tbEstCloseDateEnd.Text);
                    strInWhere += string.Format(" AND [Created] <= '{0}' ", this.tbEstCloseDateEnd.Text);
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
                                strWhere += string.Format(" AND Borrower LIKE '%{0}%'", strValue);
                                strInWhere += string.Format(" AND dbo.lpfn_GetBorrower(FileId) LIKE '%{0}%'", strValue);
                                break;
                            case "fname":
                                strWhere += string.Format(" AND Borrower LIKE '%{0}%'", strValue);
                                strInWhere += string.Format(" AND dbo.lpfn_GetBorrower(FileId) LIKE '%{0}%'", strValue);
                                break;
                            case "status":
                                strWhere += string.Format(" AND ProspectLoanStatus='{0}'", strValue);
                                strInWhere += string.Format(" AND ProspectLoanStatus='{0}'", strValue);
                                break;
                            case "addr":
                                strWhere += string.Format(" AND PropertyAddr LIKE '%{0}%'", strValue);
                                strInWhere += string.Format(" AND PropertyAddr LIKE '%{0}%'", strValue);
                                break;
                            case "city":
                                strWhere += string.Format(" AND PropertyCity LIKE '%{0}%'", strValue);
                                strInWhere += string.Format(" AND PropertyCity LIKE '%{0}%'", strValue);
                                break;
                            case "state":
                                strWhere += string.Format(" AND PropertyState='{0}'", strValue);
                                strInWhere += string.Format(" AND PropertyCity LIKE '%{0}%'", strValue);
                                break;
                            case "zip":
                                strWhere += string.Format(" AND PropertyZip LIKE '%{0}%'", strValue);
                                strInWhere += string.Format(" AND PropertyCity LIKE '%{0}%'", strValue);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            #region Handle Unassigned Leads privileges
            if (_curLoginUser.sRoleName == "Executive")
            {
                strWhere += string.Format(" AND ([FileId] IN (SELECT LoanID FROM dbo.[lpfn_GetUserLeads_Executive] ({0})) {1} )", CurrUser.iUserID
                                                        , CurrUser.userRole.AccessUnassignedLeads ? " OR [LoanOfficerID] IS NULL " : string.Empty);
                strInWhere += string.Format(" AND ([FileId] IN (SELECT LoanID FROM dbo.[lpfn_GetUserLeads_Executive] ({0})) {1} )", CurrUser.iUserID
                                                        , CurrUser.userRole.AccessUnassignedLeads ? " OR [LoanOfficerID] IS NULL " : string.Empty);
            }
            else
            {
                if (_curLoginUser.sRoleName == "Branch Manager")
                {
                    strWhere += string.Format(" AND ([FileId] IN (SELECT LoanID FROM dbo.[lpfn_GetUserLeads_Branch_Manager] ({0})) {1} )", CurrUser.iUserID
                                    , CurrUser.userRole.AccessUnassignedLeads ? " OR [LoanOfficerID] IS NULL " : string.Empty);
                    strInWhere += string.Format(" AND ([FileId] IN (SELECT LoanID FROM dbo.[lpfn_GetUserLeads_Branch_Manager] ({0})) {1})", CurrUser.iUserID
                                    , CurrUser.userRole.AccessUnassignedLeads ? " OR [LoanOfficerID] IS NULL " : string.Empty);
                }
                else
                {
                    strWhere += string.Format(" AND ([FileId] IN(SELECT LoanID FROM dbo.[lpfn_GetUserLeads] ('{0}', '{1}')) {2} )", CurrUser.iUserID, CurrUser.bAccessOtherLoans
                        , CurrUser.userRole.AccessUnassignedLeads ? " OR [LoanOfficerID] IS NULL " : string.Empty);
                    strInWhere += string.Format(" AND ([FileId] IN(SELECT LoanID FROM dbo.[lpfn_GetUserLeads] ('{0}', '{1}')) {2} )", CurrUser.iUserID, CurrUser.bAccessOtherLoans
                        , CurrUser.userRole.AccessUnassignedLeads ? " OR [LoanOfficerID] IS NULL  " : string.Empty);

                }
            }

            #endregion
            strInWhere += ")";
            //return strWhere;
            return strInWhere;
        }

        /// <summary>
        /// 根据region,division和branch选择获取所属的全部groupid
        /// </summary>
        /// <returns></returns>
        private string GenOrgQueryCondition()
        {
            string strWhere = " AND 1>0 ";


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
                    strWhere += " and Partner='" + ddlLeadSource.SelectedItem.Text + "' ";
                }
                else if (ddlLeadSourceType.SelectedIndex == 3)//Referral
                {
                    strWhere += " and Referral='" + ddlLeadSource.SelectedItem.Text + "' ";
                }
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

        StringBuilder sbAllIds = new StringBuilder();
        public string strCkAllID = "";
        StringBuilder sbLoanInfo = new StringBuilder();
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
                TextBox tbContactID = e.Row.FindControl("tbContactID") as TextBox;
                if (sbAllIds.Length > 0)
                    sbAllIds.Append(",");
                sbAllIds.AppendFormat("{0}", strID);

                CheckBox ckb = e.Row.FindControl("ckbSelect") as CheckBox;
                if (null != ckb)
                {
                    ckb.Attributes.Add("onclick", string.Format("CheckBoxClicked(this, '{0}', '{1}', '{2}', '{3}','{4}','{5}');",
                        strCkAllID, this.hiAllIds.ClientID, this.hiCheckedIds.ClientID, strID, this.hiCheckedContactIds.ClientID, tbContactID.Text));
                }

                // set ranking icon
                Literal litRanking = e.Row.FindControl("litRanking") as Literal;
                if (null != litRanking)
                {
                    string strRanking = string.Format("{0}", gv.DataKeys[e.Row.RowIndex]["Ranking"]);
                    if ("cold" == strRanking.ToLower() || "warm" == strRanking.ToLower() || "hot" == strRanking.ToLower())
                    {
                        litRanking.Text = string.Format("<img alt='{0}' src='../images/prospect/{0}.gif' />", strRanking);
                    }
                }

                // set loan status and branch info
                if (sbLoanInfo.Length > 0)
                    sbLoanInfo.Append(";");
                sbLoanInfo.AppendFormat("{0}:{1}:{2}", strID, gv.DataKeys[e.Row.RowIndex]["ProspectLoanStatus"], gv.DataKeys[e.Row.RowIndex]["BranchID"]);

                // get import error icon
                Image imgImptError = e.Row.FindControl("imgImptError") as Image;
                if (null != imgImptError)
                {
                    string strIcon = string.Format("{0}", gv.DataKeys[e.Row.RowIndex]["ImportErrorIcon"]);
                    if ("unknown.png" == strIcon.ToLower())
                        imgImptError.Visible = false;
                    else
                    {
                        imgImptError.ImageUrl = Page.ResolveClientUrl(string.Format("../images/loan/{0}", strIcon));
                        imgImptError.Attributes.Add("onclick", string.Format("showImportErrorWin('{0}', '{1}');",
                            gv.DataKeys[e.Row.RowIndex].Value, strIcon));
                    }
                }
                // get prospect task alerts icon 
                Image imgAlerts = e.Row.FindControl("imgAlerts") as Image;
                if (null != imgAlerts)
                {
                    string strIcon = string.Format("{0}", gv.DataKeys[e.Row.RowIndex]["AlertIcon"]);
                    if ("unknown.png" == strIcon.ToLower() || "taskgreen.png" == strIcon.ToLower())
                        imgAlerts.Visible = false;
                    else
                    {
                        imgAlerts.ImageUrl = Page.ResolveClientUrl(string.Format("../images/loan/{0}", strIcon));
                        imgAlerts.Attributes.Add("onclick", string.Format("showTaskAlertWin('', '{0}');", gv.DataKeys[e.Row.RowIndex].Value));
                    }
                }
                // get import error icon
                Image imgRuleAlert = e.Row.FindControl("imgRuleAlert") as Image;
                if (null != imgRuleAlert)
                {
                    string strIcon = string.Format("{0}", gv.DataKeys[e.Row.RowIndex]["RuleAlertIcon"]);
                    if ("unknown.png" == strIcon.ToLower())
                        imgRuleAlert.Visible = false;
                    else
                    {
                        imgRuleAlert.ImageUrl = Page.ResolveClientUrl(string.Format("../images/alert/{0}", strIcon));
                        imgRuleAlert.Attributes.Add("onclick", string.Format("showRuleAlertWin('{0}', '{1}');",
                            gv.DataKeys[e.Row.RowIndex].Value, gv.DataKeys[e.Row.RowIndex]["AlertID"]));
                    }
                }
            }
        }

        protected void gridList_PreRender(object sender, EventArgs e)
        {
            this.hiAllIds.Value = sbAllIds.ToString();
            this.hiCheckedIds.Value = "";
            this.hiLoanInfo.Value = sbLoanInfo.ToString();
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
            CheackIsChangeFiler(); //gdc CR45
            isReset = true;
            BindGrid();
        }

        /// <summary>
        /// Search prospect loan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSearchFilter_Click(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
            this.hiSearchFilter.Value = "";
        }

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRegion.SelectedIndex < 0)
            {
                return;
            }
            try
            {
                this.ddlDivision.AutoPostBack = false;
                if (ddlRegion.SelectedValue == "0" || ddlRegion.SelectedValue == "-1")
                {
                    this.ddlDivision.DataSource = this.GetDivisionData("-1");
                    this.ddlBranch.DataSource = this.GetBranchData("-1", "-1");
                    this.ddlDivision.DataBind();
                    this.ddlBranch.DataBind();
                }
                else
                {
                    this.ddlDivision.DataSource = this.GetDivisionData(this.ddlRegion.SelectedValue);
                    this.ddlBranch.DataSource = this.GetBranchData(this.ddlRegion.SelectedValue, "-1");
                    this.ddlDivision.DataBind();
                    this.ddlBranch.DataBind();
                }
                this.ddlDivision.AutoPostBack = true;
            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Bind Branch list when Division select change
            if (ddlDivision.SelectedIndex < 0)
            {
                return;
            }
            try
            {
                if (ddlDivision.SelectedValue == "0" || ddlDivision.SelectedValue == "-1")
                {
                    this.ddlBranch.DataSource = this.GetBranchData(this.ddlRegion.SelectedValue, "-1");
                    this.ddlBranch.DataBind();
                }
                else
                {
                    this.ddlBranch.DataSource = this.GetBranchData(this.ddlRegion.SelectedValue, this.ddlDivision.SelectedValue);
                    this.ddlBranch.DataBind();
                }
            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
            }
        }

        protected void ddlAlphabet_SelectedIndexChanged(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }

        protected void ddlViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            isReset = true;
            //BindGrid();
            Response.Redirect("ProspectPipelineSummaryLoan.aspx?type=" + ddlViewType.SelectedValue.ToString());
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
            else if (ddlOrganizationTypes.SelectedIndex == 5)
            {
                BindProcessor();
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
                    var selctedStr = this.hiCheckedIds.Value;
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
                        BindGrid();
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
            var selctedStr = this.hiCheckedIds.Value;
            if (!string.IsNullOrEmpty(selctedStr))
            {
                string[] selectedItems = selctedStr.Split(',');
                //delete the selected items
                DeleteLoans(selectedItems);
                //reload the grid data
                PageCommon.WriteJsEnd(this, "Loan has been removed successfully.", PageCommon.Js_RefreshSelf);
                BindGrid();
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
                        LoansManager.Delete(iItem);
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
            int nFileId = -1;
            string[] arrIds = this.hiCheckedIds.Value.Split(new char[] { ',' });
            if (arrIds.Length < 1)
            {
                // no record selected
                return;
            }
            else if (arrIds.Length > 1)
            {
                // only one record can selected
                return;
            }
            if (!int.TryParse(arrIds[0], out nFileId))
                nFileId = -1;
            if (nFileId == -1)
            {
                PageCommon.AlertMsg(this, "Invalid loan Id: " + arrIds[0]);
                LPLog.LogMessage(LogType.Logerror, "Invalid loan Id: " + arrIds[0]);
                return;
            }

            try
            {
                // get selected folder id
                int nFolderId = -1;
                if (!int.TryParse(this.hiSelectedFolderId.Value, out nFolderId))
                    nFolderId = -1;
                if (-1 == nFolderId)
                {
                    PageCommon.AlertMsg(this, "Invalid folder Id: " + nFolderId);
                    LPLog.LogMessage(LogType.Logerror, "Invalid folder Id: " + nFolderId);
                    return;
                }

                ChangeProspectLoanStatus(this.hiSelectedDisposal.Value.ToLower(), nFileId, nFolderId);
                BindGrid();
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to dispose the selected loan, please try again.");
                LPLog.LogMessage(LogType.Logerror, string.Format("Failed to dispose the loan({0}): {1}", nFileId, ex.Message));
            }
        }

        private bool MoveFile(int nFileId, int nFolderId, out string err)
        {
            string strResultInfo = string.Empty;
            err = string.Empty;

            string sProspectStatus = LoansManager.GetProspectStatusInfo(nFileId);
            string sFileName = LoansManager.GetProspectFileNameInfo(nFileId);  //bug 878
            if (sFileName == "")
            {
                err = "The selected lead does not have a Point file.";
                return false;
            }

            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient client = sm.StartServiceClient())
            {
                MoveFileRequest req = new MoveFileRequest();
                req.FileId = nFileId;
                req.NewFolderId = nFolderId;
                req.hdr = new ReqHdr();
                req.hdr.UserId = CurrUser.iUserID;

                MoveFileResponse response = client.MoveFile(req);
                if (response.hdr.Successful)
                    return true;

                LPLog.LogMessage(LogType.Logerror, string.Format("Failed to move file:{0}", response.hdr.StatusInfo));
                //PageCommon.AlertMsg(this, response.hdr.StatusInfo);
                err = response.hdr.StatusInfo;
                return false;
            }

        }
        private bool DisposeLead(int nFileId, int nFolderId, string newLeadStatus, out string err)
        {
            string strResultInfo = string.Empty;
            err = string.Empty;

            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient client = sm.StartServiceClient())
            {
                DisposeLeadRequest req = new DisposeLeadRequest();
                req.FileId = nFileId;
                req.NewFolderId = nFolderId;
                req.hdr = new ReqHdr();
                req.hdr.UserId = CurrUser.iUserID;
                req.LoanStatus = newLeadStatus;
                req.StatusDate = DateTime.Now;

                DisposeLeadResponse response = client.DisposeLead(req);
                if (response.hdr.Successful)
                    return true;

                LPLog.LogMessage(LogType.Logerror, string.Format("Failed to dispose file:{0}", response.hdr.StatusInfo));
                //PageCommon.AlertMsg(this, response.hdr.StatusInfo);
                err = response.hdr.StatusInfo;
                return false;
            }

        }
        private void ChangeProspectLoanStatus(string strStatus, int nFileId, int nFolderId)
        {
            // Invoke the PointManager method DisposeLoan, move loan to folder with id nFolderId
            string strTemp = "";
            try
            {
                string strResultInfo = "";
                bool bResult = false;

                switch (strStatus.ToLower())
                {
                    case "converted":
                        strTemp = "convert the lead";
                        if (!DisposeLead(nFileId, nFolderId, "Processing", out strResultInfo))
                            PageCommon.AlertMsg(this, string.Format("Failed to conver the lead, reason: {0}.", strResultInfo));

                        break;
                    case "move":
                        strTemp = "move the Point file";
                        if (!MoveFile(nFileId, nFolderId, out strResultInfo))
                            PageCommon.AlertMsg(this, string.Format("Failed to  move the Point file, reason: {0}.", strResultInfo));

                        break;
                    default:
                        strTemp = "archive the lead";
                        if (!DisposeLead(nFileId, nFolderId, strStatus, out strResultInfo))
                            PageCommon.AlertMsg(this, string.Format("Failed to dispose of the lead, reason: {0}.", strResultInfo));
                        break;
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ee)
            {
                LPLog.LogMessage(ee.Message);
                PageCommon.AlertMsg(this, string.Format("Failed to {0}, reason: Point Manager is not running.", strTemp));
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, string.Format("Failed to {0}, error message: {1} ", strTemp, ex.Message));
                LPLog.LogMessage(LogType.Logerror, string.Format("Failed to dispose/move the lead with id({0}), error message: {1}",
                    nFileId, ex.Message));
            }
        }

        private bool CallWorkflowManager(string loanStatus, int nFileId, int nFolderId, out string strStatusInfo)
        {
            // Invoke the PointManager method DisposeLoan, move loan to folder with id nFolderId
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                DisposeLeadRequest disposeReq = new DisposeLeadRequest();
                ReqHdr hdr = new ReqHdr();
                hdr.UserId = CurrUser.iUserID;
                disposeReq.hdr = hdr;
                disposeReq.FileId = nFileId;
                disposeReq.NewFolderId = nFolderId;
                disposeReq.LoanStatus = loanStatus;
                //disposeReq.LoanStatus = (LoanStatusEnum)Enum.Parse(typeof(LoanStatusEnum), loanStatus, true); ;
                disposeReq.StatusDate = DateTime.Now;

                DisposeLeadResponse disposeResponse;
                disposeResponse = service.DisposeLead(disposeReq);

                strStatusInfo = disposeResponse.hdr.StatusInfo;
                return disposeResponse.hdr.Successful;
            }
        }

        /// <summary>
        /// generate the workflow for the loan
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnGenWorkflow_Click(object sender, EventArgs e)
        {
            try
            {
                // workflow api
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient service = sm.StartServiceClient())
                {
                    GenerateWorkflowRequest req = new GenerateWorkflowRequest();
                    req.FileId = int.Parse(this.hiGenWfTpltFileId.Value);
                    req.WorkflowTemplId = int.Parse(this.hiWfTpltId.Value);
                    req.hdr = new ReqHdr();
                    req.hdr.UserId = CurrUser.iUserID;

                    GenerateWorkflowResponse respone = null;
                    respone = service.GenerateWorkflow(req);
                    if (respone.hdr.Successful)
                    {
                        PageCommon.AlertMsg(this, "Generate workflow successfully!");
                    }
                    else
                    {
                        PageCommon.AlertMsg(this, string.Format("Failed to generate workflow for the loan: {0}", respone.hdr.StatusInfo));
                    }
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ee)
            {
                PageCommon.AlertMsg(this, "Failed to generate workflow for the loan, reason: Workflow Manager is not running.");
                LPLog.LogMessage(LogType.Logerror, string.Format("Failed to generate workflow for the loan: {0}", ee.Message));
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to generate workflow for the loan.");
                LPLog.LogMessage(LogType.Logerror, string.Format("Failed to generate workflow for the loan: {0}", ex.Message));
            }
            BindGrid();
        }

        /// <summary>
        /// call client function
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strScript"></param>
        private void ClientFun(string strKey, string strScript)
        {
            ClientScriptManager csm = this.Page.ClientScript;
            if (!csm.IsStartupScriptRegistered(this.GetType(), strKey))
            {
                csm.RegisterStartupScript(this.GetType(), strKey, strScript, true);
            }
        }


        //gdc CR45


        /// <summary>
        /// 
        /// </summary>
        private void BindUserPiplineView()
        {
            BLL.UserPipelineViews bll = new UserPipelineViews();

            ddlUserPipelineView.DataSource = bll.GetList_ViewName("PipelineType='Leads' AND Enabled = 1 AND UserID = " + CurrUser.iUserID.ToString(), "ViewName ASC");
            ddlUserPipelineView.DataBind();

            ddlUserPipelineView.Items.Insert(0, new ListItem() { Selected = true, Text = "--select--", Value = "0" });

            if (!IsPostBack && string.IsNullOrEmpty(Request.QueryString["q"]))
            {
                BLL.UserHomePref bllUHP = new UserHomePref();

                var model = bllUHP.GetModel(CurrUser.iUserID);
                if (model != null && model.UserId == CurrUser.iUserID)
                {
                    ddlUserPipelineView.SelectedValue = model.DefaultLeadsPipelineViewId.ToString();
                    ddlUserPipelineView_SelectedIndexChanged(ddlUserPipelineView, new EventArgs());
                }
            }

        }

        //CR54
        private DataTable GetCompletedStages()
        {
            string sSql = string.Format("SELECT DISTINCT LastCompletedStage FROM lpvw_PipelineInfo WHERE ISNULL(LastCompletedStage,'') <> '' ORDER BY LastCompletedStage");

            return DAL.DbHelperSQL.ExecuteDataTable(sSql);
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
                if (FirstTimeFlag == true)
                {
                    BindDefaultFiler();
                }
                else
                {
                    SetUserPipelineView(Convert.ToInt32(ddlUserPipelineView.SelectedValue));
                }
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
                BLL.UserPipelineViews bll = new UserPipelineViews();

                var model = bll.GetModel(userPipelineViewID);


                if (model != null && model.UserId == CurrUser.iUserID)
                {
                    try
                    {
                        ddlViewType.SelectedValue = model.ViewFilter;
                        ddlViewType.Attributes["UPV"] = string.IsNullOrEmpty(model.ViewFilter) ? "" : model.ViewFilter;
                        //ddlViewType_SelectedIndexChanged(ddlViewType, new EventArgs());
                        sViewType = model.ViewFilter;

                        ddlOrganizationTypes.SelectedValue = model.OrgTypeFilter;
                        ddlOrganizationTypes.Attributes["UPV"] = string.IsNullOrEmpty(model.OrgTypeFilter) ? "" : model.OrgTypeFilter;
                        ddlOrganizationTypes_SelectedIndexChanged(ddlOrganizationTypes, new EventArgs());

                        ddlOrganization.SelectedValue = model.OrgFilter;
                        ddlOrganization.Attributes["UPV"] = string.IsNullOrEmpty(model.OrgFilter) ? "" : model.OrgFilter;

                        ddlStages.SelectedValue = model.StageFilter;
                        ddlStages.Attributes["UPV"] = string.IsNullOrEmpty(model.StageFilter) ? "" : model.StageFilter;

                        ddlLeadSourceType.SelectedValue = model.ContactTypeFilter;
                        ddlLeadSourceType.Attributes["UPV"] = string.IsNullOrEmpty(model.ContactTypeFilter) ? "" : model.ContactTypeFilter;
                        ddlLeadSourceType_SelectedIndexChanged(ddlLeadSourceType, new EventArgs());

                        ddlLeadSource.SelectedValue = model.ContactFilter;
                        ddlLeadSource.Attributes["UPV"] = string.IsNullOrEmpty(model.ContactFilter) ? "" : model.ContactFilter;


                        ddlDataType.SelectedValue = model.DateTypeFilter;
                        ddlDataType.Attributes["UPV"] = string.IsNullOrEmpty(model.DateTypeFilter) ? "" : model.DateTypeFilter;

                        #region DateFilter
                        if (!string.IsNullOrEmpty(model.DateFilter))
                        {
                            var item = model.DateFilter.Split(',');

                            tbEstCloseDateStart.Attributes["UPV"] = "";
                            tbEstCloseDateStart.Text = "";
                            if (item.Count() >= 1 && !string.IsNullOrEmpty(item.FirstOrDefault()))
                            {
                                tbEstCloseDateStart.Text = item.FirstOrDefault();
                                tbEstCloseDateStart.Attributes["UPV"] = item.FirstOrDefault();
                            }

                            tbEstCloseDateEnd.Attributes["UPV"] = "";
                            tbEstCloseDateEnd.Text = "";
                            if (item.Count() == 2 && !string.IsNullOrEmpty(item.LastOrDefault()))
                            {
                                tbEstCloseDateEnd.Text = item.LastOrDefault();
                                tbEstCloseDateEnd.Attributes["UPV"] = item.LastOrDefault();
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
            ddlViewType.SelectedIndex = 0;


            ddlOrganizationTypes.SelectedIndex = 0;
            ddlOrganizationTypes_SelectedIndexChanged(ddlOrganizationTypes, new EventArgs());

            ddlOrganization.SelectedIndex = 0;

            ddlStages.SelectedIndex = 0;

            ddlLeadSourceType.SelectedIndex = 0;

            ddlLeadSource.SelectedIndex = 0;

            ddlDataType.SelectedIndex = 0;

            tbEstCloseDateEnd.Text = "";
            tbEstCloseDateStart.Text = "";
        }


        private void CheackIsChangeFiler()
        {
            if (ddlViewType.Attributes["UPV"] != ddlViewType.SelectedValue
                || ddlOrganizationTypes.Attributes["UPV"] != ddlOrganizationTypes.SelectedValue
                || ddlOrganization.Attributes["UPV"] != ddlOrganization.SelectedValue
                || ddlStages.Attributes["UPV"] != ddlStages.SelectedValue
                || ddlLeadSourceType.Attributes["UPV"] != ddlLeadSourceType.SelectedValue
                || ddlLeadSource.Attributes["UPV"] != ddlLeadSource.SelectedValue
                || ddlDataType.Attributes["UPV"] != ddlDataType.SelectedValue
                || tbEstCloseDateStart.Attributes["UPV"] != tbEstCloseDateStart.Text
                || tbEstCloseDateEnd.Attributes["UPV"] != tbEstCloseDateEnd.Text
                )
            {
                ddlUserPipelineView.SelectedValue = "0";
            }

        }


        protected void btnSaveView_OnClick(object sender, EventArgs e)
        {
            var viewName = txtSaveViewName.Text.Trim().Replace("'", "");
            BLL.UserPipelineViews bll = new UserPipelineViews();
            Model.UserPipelineViews model = new Model.UserPipelineViews();
            int ID = 0;
            var ds = bll.GetList_ViewName("ViewName ='" + viewName + "'" + " AND PipelineType='Leads'  AND UserId =" + CurrUser.iUserID, "");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ID = ds.Tables[0].Rows[0]["UserPipelineViewID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["UserPipelineViewID"]) : 0;
            }

            model.ViewName = viewName;
            model.ContactFilter = ddlLeadSource.SelectedValue.Trim();
            model.ContactTypeFilter = ddlLeadSourceType.SelectedValue.Trim();
            model.DateFilter = tbEstCloseDateStart.Text.Trim() + "," + tbEstCloseDateEnd.Text.Trim();
            model.DateTypeFilter = ddlDataType.SelectedValue.Trim();

            model.Enabled = true;
            model.OrgFilter = ddlOrganization.SelectedValue.Trim();
            model.OrgTypeFilter = ddlOrganizationTypes.SelectedValue.Trim();
            model.PipelineType = "Leads";
            model.StageFilter = ddlStages.SelectedValue.Trim();
            model.UserId = CurrUser.iUserID;
            model.ViewFilter = ddlViewType.SelectedValue.Trim();

            model.UserPipelineViewID = ID;


            if (ID != 0)
            {
                bll.Update(model);
            }
            else
            {
                bll.Add(model);

                ds = bll.GetList_ViewName("ViewName ='" + viewName + "'" + " AND PipelineType='Leads'  AND UserId =" + CurrUser.iUserID, "");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ID = ds.Tables[0].Rows[0]["UserPipelineViewID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["UserPipelineViewID"]) : 0;
                }

                BindUserPiplineView();

                ddlUserPipelineView.SelectedValue = ID.ToString();

                SetUserPipelineView(ID);
            }




        }
    }
}


