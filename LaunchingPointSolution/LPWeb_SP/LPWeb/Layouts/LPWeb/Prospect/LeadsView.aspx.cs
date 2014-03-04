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

namespace LPWeb.Layouts.LPWeb.Prospect
{
    /// <summary>
    /// Leads View
    /// Author: Peter
    /// Date: 2011-05-22
    /// </summary>
    public partial class LeadsView : BasePage
    {
        LoginUser _curLoginUser = new LoginUser();
        BLL.UserProspectColumns upcManager = new BLL.UserProspectColumns();
        BLL.Loans LoansManager = new BLL.Loans();
        private readonly BLL.Prospect _bllProspect = new BLL.Prospect();
        BLL.LoanAlerts loanAlertMngr = new BLL.LoanAlerts();
        BLL.Template_Stages TpltStages = new BLL.Template_Stages();
        public string FromURL = string.Empty;
        private bool isReset = false;
        protected string sHasViewRight = "0";
        private string fromHomeFilter = string.Empty;
        private string StageFilter;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Url != null)
            {
                FromURL = Request.Url.ToString();
            }
            _curLoginUser = new LoginUser();
            //是否有View权限
            sHasViewRight = _curLoginUser.userRole.Loans.ToString().IndexOf('D') > 0 ? "1" : "0";  //View
            if (!IsPostBack)
            {
                try
                {
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

                this.DoInitData();

                BindFilter();

                DataTable dtStages = TpltStages.GetStageTemplateList(" AND [Enabled]=1 AND WorkflowType='Prospect'");
                this.ddlStages.DataSource = dtStages;
                this.ddlStages.DataValueField = "TemplStageId";
                this.ddlStages.DataTextField = "Name";
                this.ddlStages.DataBind();
                this.ddlStages.Items.Insert(0, new ListItem("All Stages", "-1"));
                fromHomeFilter = FilterFromHomePiplineSummary();
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

            dsSource = LeadSourcesManager.GetAllList();
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
                loansList = LoansManager.GetProspectList(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
                if (loansList == null || loansList.Tables[0].Rows.Count == 0)
                {
                    loansList = LoansManager.GetList(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

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
            ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
            BindGrid();
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
                    searchCondition += string.Format(searchTempllage, "[Stage]", KVPs["CurrentStage"]);
                    StageFilter = "'" + KVPs["CurrentStage"] + "'";
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

            if (ddlViewType.SelectedValue == "0")
            {
                strWhere += string.Format(" AND Status='Prospect' AND ProspectLoanStatus='Active'");
            }
            else if (ddlViewType.SelectedValue == "1")
            {
                strWhere += string.Format(" AND Status='Prospect'");
            }
            else if (ddlViewType.SelectedValue == "2")
            {
                strWhere += string.Format(" AND Status='Prospect' AND ProspectLoanStatus<>'Active'");
            }

            if (!string.IsNullOrEmpty(fromHomeFilter))
            {
                strWhere += fromHomeFilter;
            }
            string groupIdQueryPart = GenOrgQueryCondition();
            if (!string.IsNullOrEmpty(groupIdQueryPart))
                strWhere += groupIdQueryPart;

            if (!string.IsNullOrEmpty(this.ddlAlphabet.SelectedValue))
                strWhere += string.Format(" AND [Borrower] Like '{0}%'", ddlAlphabet.SelectedValue);

            if (ddlStages.SelectedValue != "-1")
            {
                strWhere += string.Format(" AND [Stage] = '{0}' ", this.ddlStages.SelectedItem.Text);
            }

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
            }
            else if (ddlDataType.SelectedValue == "0")
            {
                if (!string.IsNullOrEmpty(this.tbEstCloseDateStart.Text))
                {
                    strWhere += string.Format(" AND [EstClose] >= '{0}' ", this.tbEstCloseDateStart.Text);
                }

                if (!string.IsNullOrEmpty(this.tbEstCloseDateEnd.Text))
                {
                    strWhere += string.Format(" AND [EstClose] <= '{0}' ", this.tbEstCloseDateEnd.Text);
                }
            }
            else if (ddlDataType.SelectedValue == "1")
            {
                if (!string.IsNullOrEmpty(this.tbEstCloseDateStart.Text))
                {
                    strWhere += string.Format(" AND [Created] >= '{0}' ", this.tbEstCloseDateStart.Text);
                }

                if (!string.IsNullOrEmpty(this.tbEstCloseDateEnd.Text))
                {
                    strWhere += string.Format(" AND [Created] <= '{0}' ", this.tbEstCloseDateEnd.Text);
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
                                break;
                            case "fname":
                                strWhere += string.Format(" AND Borrower LIKE '%{0}%'", strValue);
                                break;
                            case "status":
                                strWhere += string.Format(" AND ProspectLoanStatus='{0}'", strValue);
                                break;
                            case "addr":
                                strWhere += string.Format(" AND PropertyAddr LIKE '%{0}%'", strValue);
                                break;
                            case "city":
                                strWhere += string.Format(" AND PropertyCity LIKE '%{0}%'", strValue);
                                break;
                            case "state":
                                strWhere += string.Format(" AND PropertyState='{0}'", strValue);
                                break;
                            case "zip":
                                strWhere += string.Format(" AND PropertyZip LIKE '%{0}%'", strValue);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            //来自lin邮件的描述: 如果用户没有"Access others loans"的权限, 则整个筛选必须包括LoanTeam.UserId和当前用户的匹配。

            if (!CurrUser.userRole.OtherLoanAccess)
            {
                //queryCon += " AND UserID = " + _curLoginUser.iUserID;
                strWhere += string.Format(" AND ([UserID] = {0} OR [Owner] = {0})", CurrUser.iUserID);
            }
            else
            {
                strWhere += string.Format(" OR ([UserID] = {0} OR [Owner] = {0})", CurrUser.iUserID);
            }
            return strWhere;
        }

        /// <summary>
        /// 根据region,division和branch选择获取所属的全部groupid
        /// </summary>
        /// <returns></returns>
        private string GenOrgQueryCondition()
        {
            string strWhere = " AND 1>0 ";
            #region Organization
            //type 0 all 1 Region 2 Division 3 Branch 4 LoanOfficer
            if (ddlOrganizationTypes.SelectedIndex > 0 && ddlOrganization.SelectedIndex > 0)
            {
                if (ddlOrganizationTypes.SelectedIndex == 1)//Region
                {
                    strWhere += string.Format(" AND RegionID = {0} ", ddlOrganization.SelectedValue);
                }
                else if (ddlOrganizationTypes.SelectedIndex == 2)//Division
                {
                    strWhere += string.Format(" AND DivisionID = {0} ", ddlOrganization.SelectedValue);
                }
                else if (ddlOrganizationTypes.SelectedIndex == 3)//Branch
                {
                    strWhere = string.Format(" AND  BranchId={0} ", ddlOrganization.SelectedValue);
                }
                else if (ddlOrganizationTypes.SelectedIndex == 4)//Loan Officer
                {
                    // Loan Officer
                    strWhere = string.Format(@" AND  FileId in (select FileId from LoanTeam lt inner join Roles r
	                        on lt.RoleId=r.RoleId and r.Name='Loan Officer' where lt.UserId ={0} ) ", ddlOrganization.SelectedValue);
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
        string strCkAllID = "";
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

                if (sbAllIds.Length > 0)
                    sbAllIds.Append(",");
                sbAllIds.AppendFormat("{0}", strID);

                CheckBox ckb = e.Row.FindControl("ckbSelect") as CheckBox;
                if (null != ckb)
                {
                    ckb.Attributes.Add("onclick", string.Format("CheckBoxClicked(this, '{0}', '{1}', '{2}', '{3}');",
                        strCkAllID, this.hiAllIds.ClientID, this.hiCheckedIds.ClientID, strID));
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
            BindGrid();
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

        private void ChangeProspectLoanStatus(string strStatus, int nFileId, int nFolderId)
        {
            // Invoke the PointManager method DisposeLoan, move loan to folder with id nFolderId
            try
            {
                string strResultInfo = "";
                bool bResult = false;
                string strTemp = "";
                switch (strStatus.ToLower())
                {
                    case "active":
                        if (CallWorkflowManager("Prospect", nFileId, nFolderId, out strResultInfo))
                        {
                            bResult = true;
                            // active loan 
                            LoansManager.ProspectActive(nFileId, CurrUser.iUserID);
                        }
                        else
                        {
                            bResult = false;
                            strTemp = "Prospect";
                        }
                        break;
                    case "bad":
                        if (CallWorkflowManager(strStatus, nFileId, nFolderId, out strResultInfo))
                        {
                            bResult = true;
                            // mark as bad
                            LoansManager.ProspectMarkAsBad(nFileId, CurrUser.iUserID);
                        }
                        else
                        {
                            bResult = false;
                            strTemp = "Bad";
                        }
                        break;
                    case "canceled":
                        if (CallWorkflowManager(strStatus, nFileId, nFolderId, out strResultInfo))
                        {
                            bResult = true;
                            // cancel loan 
                            LoansManager.ProspectCancel(nFileId, CurrUser.iUserID);
                        }
                        else
                        {
                            bResult = false;
                            strTemp = "Canceled";
                        }
                        break;
                    case "converted":
                        if (CallWorkflowManager("Processing", nFileId, nFolderId, out strResultInfo))
                        {
                            bResult = true;
                            // convert loan to Processing
                            LoansManager.ProspectConvert(nFileId, CurrUser.iUserID);

                            // call client function to show WorkflowTemplate selection window
                            this.hiGenWfTpltFileId.Value = nFileId.ToString();
                            ClientFun("showwftpltwin", "$(document).ready(function(){showWkTpltWin();});");
                        }
                        else
                        {
                            bResult = false;
                            strTemp = "Processing";
                        }
                        break;
                    case "suspended":
                        if (CallWorkflowManager(strStatus, nFileId, nFolderId, out strResultInfo))
                        {
                            bResult = true;
                            // suspend loan 
                            LoansManager.ProspectSuspend(nFileId, CurrUser.iUserID);
                        }
                        else
                        {
                            bResult = false;
                            strTemp = "Suspended";
                        }
                        break;
                    case "move":
                        //Move File

                        try
                        {
                            string sProspectStatus = LoansManager.GetProspectStatusInfo(nFileId);
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
                                {
                                    bResult = true;
                                }
                                else
                                {
                                    LPLog.LogMessage(LogType.Logerror, string.Format("Failed to move file:{0}", response.hdr.StatusInfo));
                                    //PageCommon.AlertMsg(this, response.hdr.StatusInfo);
                                    strResultInfo = response.hdr.StatusInfo;
                                    strTemp = "Move File";
                                }
                            }
                        }
                        catch (System.ServiceModel.EndpointNotFoundException ee)
                        {
                            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ee.Message));
                            //PageCommon.AlertMsg(this, "Failed to move the Point file, reason: Point Manager is not running.");
                            strResultInfo = "Point Manager is not running.";
                            strTemp = "Move File";
                        }
                        catch (Exception ex)
                        {
                            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ex.Message));
                            //PageCommon.AlertMsg(this, ex.Message);
                            strResultInfo = ex.Message;
                            strTemp = "Move File";
                        }

                        break;
                    default:
                        PageCommon.AlertMsg(this, "Unknown status!");
                        LPLog.LogMessage(LogType.Logerror, string.Format("Failed to change the loan status to {0}, this is an unknown status, FileId: {1}",
                            strTemp, nFileId));
                        return;
                }

                if (!bResult)
                {
                    if (strTemp == "Move File")
                    {
                        PageCommon.AlertMsg(this, string.Format("Failed to move file, error message: {0}", strResultInfo));
                    }
                    else
                    {
                        PageCommon.AlertMsg(this, string.Format("Failed to change the loan status as {0}, error message: {1}", strTemp, strResultInfo));
                    }
                    LPLog.LogMessage(LogType.Logerror, string.Format("Failed to change the loan status as {0}, FileId: {1}, error message: {2}",
                        strTemp, nFileId, strResultInfo));
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ee)
            {
                LPLog.LogMessage(ee.Message);
                PageCommon.AlertMsg(this, "Failed to convert loan, reason: Point Manager is not running.");
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to convert the loan, error message: " + ex.Message);
                LPLog.LogMessage(LogType.Logerror, string.Format("Failed to convert the loan with id({0}), error message: {1}",
                    nFileId, ex.Message));
            }
        }

        private bool CallWorkflowManager(string loanStatus, int nFileId, int nFolderId, out string strStatusInfo)
        {
            // Invoke the PointManager method DisposeLoan, move loan to folder with id nFolderId
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                DisposeLoanRequest disposeReq = new DisposeLoanRequest();
                ReqHdr hdr = new ReqHdr();
                hdr.UserId = CurrUser.iUserID;
                disposeReq.hdr = hdr;
                disposeReq.FileId = nFileId;
                disposeReq.NewFolderId = nFolderId;
                disposeReq.LoanStatus = loanStatus;
                //disposeReq.LoanStatus = (LoanStatusEnum)Enum.Parse(typeof(LoanStatusEnum), loanStatus, true); ;
                disposeReq.StatusDate = DateTime.Now;

                DisposeLoanResponse disposeResponse;
                disposeResponse = service.DisposeLoan(disposeReq);

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
    }
}
