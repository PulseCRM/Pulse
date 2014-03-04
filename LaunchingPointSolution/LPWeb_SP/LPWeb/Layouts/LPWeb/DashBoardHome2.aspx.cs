using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using LPWeb.Common;
using System.Text.RegularExpressions;
using System.Text;
using LPWeb.DAL;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Portal.WebControls;
using System.Configuration;
using Microsoft.SharePoint.WebPartPages;
using System.Data.SqlClient;
using Utilities;

public partial class DashBoardHome2 : BasePage
{
    public string sChartQueryString = string.Empty;
    public string sWhereEncode = string.Empty;
    public DataTable LoanAnalysisData = null;
    public string sWorkflowType = string.Empty;

    protected string strComAnnListUrl = "#";
    protected string strComCalListUrl = "#";
    protected string strRatesListUrl = "#";

    // 当前用户
    LoginUser CurrentUser;
    int iCurrentUserID = 0;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        // 获取当前用户信息
        this.CurrentUser = new LoginUser();
        this.iCurrentUserID = CurrentUser.iUserID;

        #region 页面重定向

        if (this.CurrentUser.sRoleName != "Executive" && this.CurrentUser.sRoleName != "Branch Manager") 
        {
            try
            {
                this.Response.Redirect("DashBoardHome.aspx");
            }
            catch
            {

            }
        }

        #endregion

        #region Get User Home Profile

        LPWeb.BLL.UserHomePref UserHomePref1 = new LPWeb.BLL.UserHomePref();
        LPWeb.Model.UserHomePref UserHomePref2 = UserHomePref1.GetModel(this.iCurrentUserID);

        #endregion

        #region 角色权限-显示控制

        #region 设置是否显示Alert

        if (this.CurrentUser.userRole.OverdueTaskAlerts == true)
        {
            this.AlertWebPart1.Visible = true;
        }
        else
        {
            this.AlertWebPart1.Visible = false;
        }

        // User Home Profile
        if (UserHomePref2 != null)
        {
            if (UserHomePref2.OverDueTaskAlert == true)
            {
                this.AlertWebPart1.Visible = true;
            }
            else
            {
                this.AlertWebPart1.Visible = false;
            }
        }

        #endregion

        #region 设置是否显示Email Inbox

        if (this.CurrentUser.userRole.ExchangeInbox == true)
        {
            this.divEmailInbox.Visible = true;
        }
        else
        {
            this.divEmailInbox.Visible = false;
        }

        // User Home Profile
        if(UserHomePref2 != null)
        {
            if (UserHomePref2.ExchangeInbox == true)
            {
                this.divEmailInbox.Visible = true;
            }
            else
            {
                this.divEmailInbox.Visible = false;
            }
        }

        #endregion

        #region 设置是否显示Company Announcement

        //if (this.CurrentUser.userRole.Announcements == true)
        //{
        //    this.divCompanyAnn.Visible = true;
        //}
        //else
        //{
        //    this.divCompanyAnn.Visible = false;
        //}

        //// User Home Profile
        //if (UserHomePref2 != null)
        //{
        //    if (UserHomePref2.Announcements == true)
        //    {
        //        this.divCompanyAnn.Visible = true;
        //    }
        //    else
        //    {
        //        this.divCompanyAnn.Visible = false;
        //    }
        //}

        #endregion

        #region 设置是否显示Calendar

        if (this.CurrentUser.userRole.ExchangeCalendar == true)
        {
            this.divCalendar.Visible = true;
        }
        else
        {
            this.divCalendar.Visible = false;
        }

        // User Home Profile
        if (UserHomePref2 != null)
        {
            if (UserHomePref2.ExchangeCalendar == true)
            {
                this.divCalendar.Visible = true;
            }
            else
            {
                this.divCalendar.Visible = false;
            }
        }

        #endregion

        #region 设置是否显示Pipeline Summary/Sales Breakdown/Organizational Production

        #region Role Setup

        // initialize
        this.divPipelineAnalysis.Visible = false;
        this.divSalesBreakdown.Visible = false;
        this.divOrganProduction.Visible = false;

        // PipelineChart
        if (this.CurrentUser.userRole.PipelineChart == true) 
        {
            this.divPipelineAnalysis.Visible = true;
        }
        
        // SalesBreakdownChart
        if (this.CurrentUser.userRole.SalesBreakdownChart == true)
        {
            this.divPipelineAnalysis.Visible = true;
            this.divSalesBreakdown.Visible = true;
        }
        
        // OrgProductionChart
        if (this.CurrentUser.userRole.OrgProductionChart == true)
        {
            this.divPipelineAnalysis.Visible = true;
            this.divOrganProduction.Visible = true;
        }
        
        // Org_N_Sales_Charts
        if (this.CurrentUser.userRole.Org_N_Sales_Charts == true)
        {
            this.divPipelineAnalysis.Visible = true;
            this.divSalesBreakdown.Visible = true;
            this.divOrganProduction.Visible = true;
        }
        
        #endregion

        #region User Home Profile

        if (UserHomePref2 != null)
        {
            // initialize
            this.divPipelineAnalysis.Visible = false;
            this.divSalesBreakdown.Visible = false;
            this.divOrganProduction.Visible = false;

            // PipelineChart
            if (UserHomePref2.PipelineChart == true)
            {
                this.divPipelineAnalysis.Visible = true;
            }

            // SalesBreakdownChart
            if (UserHomePref2.SalesBreakdownChart == true)
            {
                this.divPipelineAnalysis.Visible = true;
                this.divSalesBreakdown.Visible = true;
            }

            // OrgProductionChart
            if (UserHomePref2.OrgProductionChart == true)
            {
                this.divPipelineAnalysis.Visible = true;
                this.divOrganProduction.Visible = true;
            }

            // Org_N_Sales_Charts
            if (UserHomePref2.Org_N_Sales_Charts == true)
            {
                this.divPipelineAnalysis.Visible = true;
                this.divSalesBreakdown.Visible = true;
                this.divOrganProduction.Visible = true;
            }
        }

        #endregion

        #endregion

        #region 设置是否显示filter

        if(this.divPipelineAnalysis.Visible == false
            && this.divSalesBreakdown.Visible == false
            && this.divOrganProduction.Visible == false) 
        {
            this.divFilters.Visible = false;
        }

        #endregion

        #region UserHomePref.DashboardLastCompletedStages

        // StageFilter
        string sStageFilter = string.Empty;
        this.ddlStageFilter.Value = "CurrentStages";
        sStageFilter = "CurrentStages";

        if (UserHomePref2 != null && UserHomePref2.DashboardLastCompletedStages != null && UserHomePref2.DashboardLastCompletedStages == 1)
        {
            this.ddlStageFilter.Value = "LastCompletedStages";
            sStageFilter = "LastCompletedStages";
        }

        #endregion

        #region Quick Lead Form

        if (this.CurrentUser.QuickLeadForm == true)
        {
            this.divQuickLead.Visible = true;
        }
        else
        {
            this.divQuickLead.Visible = false;
        }

        #endregion

        #endregion

        string sErrorMsg = "Invalid query string, be ignored.";

        #region 获取页面参数

        int iRegionID = 0;
        int iDivisionID = 0;
        string sRegionID = string.Empty;
        string sDivisionID = string.Empty;

        string sBranchID = string.Empty;
        string sDateType = string.Empty;
        string sFromDate = string.Empty;
        string sToDate = string.Empty;

        // LoanOfficerIDs
        string sLoanOfficerIDs = string.Empty;

        #region RegionID

        if (this.Request.QueryString["Region"] != null)
        {
            #region get region id

            string sRegionID_Encode = this.Request.QueryString["Region"].ToString();
            string sRegionID_Decode = Encrypter.Base64Decode(sRegionID_Encode);

            if (sRegionID_Decode == sRegionID_Encode)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
            }
            bool IsValid = Regex.IsMatch(sRegionID_Decode, @"^(-)?\d+$");
            if (IsValid == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
            }

            sRegionID = sRegionID_Decode;
            iRegionID = Convert.ToInt32(sRegionID_Decode);

            #endregion
        }

        #endregion

        #region DivisionID

        if (this.Request.QueryString["Division"] != null)
        {
            #region get division id

            string sDivisionID_Encode = this.Request.QueryString["Division"].ToString();
            string sDivisionID_Decode = Encrypter.Base64Decode(sDivisionID_Encode);

            if (sDivisionID_Decode == sDivisionID_Encode)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
            }
            bool IsValid = Regex.IsMatch(sDivisionID_Decode, @"^(-)?\d+$");
            if (IsValid == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
            }

            sDivisionID = sDivisionID_Decode;
            iDivisionID = Convert.ToInt32(sDivisionID_Decode);

            #endregion
        }

        #endregion

        #region BranchID

        if (this.Request.QueryString["Branch"] != null)
        {
            string sBranch_Encode = this.Request.QueryString["Branch"].ToString();
            sBranchID = Encrypter.Base64Decode(sBranch_Encode);
            if (sBranch_Encode == sBranchID)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
            }
            bool IsValid = Regex.IsMatch(sBranchID, @"^(-)?\d+$");
            if (IsValid == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
            }
        }

        #endregion

        #region Date Type

        if (this.Request.QueryString["DateType"] != null)
        {
            string sDateType_Encode = this.Request.QueryString["DateType"].ToString();
            sDateType = Encrypter.Base64Decode(sDateType_Encode);
            if (sDateType != "CloseDate" && sDateType != "OpenDate")
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
            }
        }
        else
        {
            sDateType = "CloseDate";
        }

        #endregion

        #region FromDate

        if (this.Request.QueryString["FromDate"] != null)
        {
            string sFromDate_Encode = this.Request.QueryString["FromDate"].ToString();
            sFromDate = Encrypter.Base64Decode(sFromDate_Encode);
            if (sFromDate_Encode == sFromDate)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
            }
        }

        #endregion

        #region ToDate

        if (this.Request.QueryString["ToDate"] != null)
        {
            string sToDate_Encode = this.Request.QueryString["ToDate"].ToString();
            sToDate = Encrypter.Base64Decode(sToDate_Encode);
            if (sToDate_Encode == sToDate)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
            }
        }

        #endregion

        #region Workflow Type

        if (this.Request.QueryString["WorkflowType"] != null)
        {
            string sWorkflowType_Encode = this.Request.QueryString["WorkflowType"].ToString();
            this.sWorkflowType = Encrypter.Base64Decode(sWorkflowType_Encode);
            if (this.sWorkflowType != "Processing" && this.sWorkflowType != "Prospect" && this.sWorkflowType != "Archived")
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
            }
        }
        else
        {
            this.sWorkflowType = "Processing";
        }

        #endregion

        #region LoanOfficerIDs

        if (this.Request.QueryString["LoanOfficerIDs"] != null) 
        {
            string sLoanOfficerIDs_Encode = this.Request.QueryString["LoanOfficerIDs"].ToString();
            string sLoanOfficerIDs_Decode = Encrypter.Base64Decode(sLoanOfficerIDs_Encode);

            if (sLoanOfficerIDs_Encode == sLoanOfficerIDs_Decode)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
            }

            bool IsValid = PageCommon.IsIDString(sLoanOfficerIDs_Decode);
            if (IsValid == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
            }

            sLoanOfficerIDs = sLoanOfficerIDs_Decode;
        }

        #endregion

        #region StageFilter

        if (this.Request.QueryString["StageFilter"] != null)
        {
            sStageFilter = this.Request.QueryString["StageFilter"].ToString();

            if (sStageFilter != "CurrentStages")
            {
                sStageFilter = "LastCompletedStages";
            }
        }

        #endregion

        #endregion

        #region load filters

        DataSet OrganFilters = PageCommon.GetOrganFilter(iRegionID, iDivisionID);

        // region filter
        DataTable RegionListData = OrganFilters.Tables["Regions"];
        this.ddlRegions.DataSource = RegionListData;
        this.ddlRegions.DataBind();

        // division filter
        DataTable DivisionListData = OrganFilters.Tables["Divisions"];
        this.ddlDivisions.DataSource = DivisionListData;
        this.ddlDivisions.DataBind();

        // branch filter
        DataTable BranchListData = OrganFilters.Tables["Branches"];
        this.ddlBranches.DataSource = BranchListData;
        this.ddlBranches.DataBind();

        #endregion

        #region Build Search Conditions

        string sWhere = string.Empty;

        #region sWorkflowType

        if (this.sWorkflowType == "Archived")
        {
            sWhere += " and (b.Status!='Processing') and (b.Status!='Prospect')";
            //CR063 : Remove the “Suspended” status in the list of the “Archived Loans” status
            sWhere += " AND (b.Status != 'Suspended')";
        }
        else
        {
            sWhere += " and (b.Status='" + this.sWorkflowType + "')";
        }

        if (this.sWorkflowType == "Prospect")
        {
            sWhere += " and (b.ProspectLoanStatus='Active')";
        }

        #endregion

        if (sRegionID != string.Empty)
        {
            sWhere += " and (a.RegionID = " + sRegionID + ")";
        }

        if (sDivisionID != string.Empty)
        {
            sWhere += " and (a.DivisionID = " + sDivisionID + ")";
        }

        if (sBranchID != string.Empty)
        {
            sWhere += " and (a.BranchID = " + sBranchID + ")";
        }

        if (sDateType != string.Empty)
        {
            #region FromDate and ToDate

            DateTime? FromDate = null;
            DateTime? ToDate = null;
            
            if (sFromDate != string.Empty)
            {
                DateTime FromDate1;
                bool IsDate1 = DateTime.TryParse(sFromDate, out FromDate1);
                if (IsDate1 == true)
                {
                    FromDate = FromDate1;
                }
            }

            if (sToDate != string.Empty)
            {
                DateTime ToDate1;
                bool IsDate2 = DateTime.TryParse(sToDate, out ToDate1);
                if (IsDate2 == true)
                {
                    ToDate = ToDate1;
                }
            }

            string sFiledName = string.Empty;
            if(sDateType == "CloseDate")
            {
                sFiledName = "b.LastStageComplDate";
            }
            else
            {
                sFiledName = "b.Created";
            }

            sWhere += SqlTextBuilder.BuildDateSearchCondition(sFiledName, FromDate, ToDate);

            #endregion
        }

        if(sLoanOfficerIDs != string.Empty)
        {
            sWhere += " and dbo.lpfn_GetLoanOfficerID(b.FileId) in (" + sLoanOfficerIDs + ")";
        }

        #endregion

        #region Pipeline Summary

        #region get stage template list

        // loan manager
        LPWeb.BLL.Loans LoanManager = new LPWeb.BLL.Loans();

        if (this.sWorkflowType == "Archived")
        {
            #region init LoanAnalysisData

            this.LoanAnalysisData = new DataTable();
            this.LoanAnalysisData.Columns.Add("StageName", typeof(string));
            this.LoanAnalysisData.Columns.Add("StageAlias", typeof(string));
            this.LoanAnalysisData.Columns.Add("Href", typeof(string));
            this.LoanAnalysisData.Columns.Add("Amount", typeof(decimal));
            this.LoanAnalysisData.Columns.Add("LoanCounts", typeof(int)); //gdc CR40

            DataRow ClosedRow = this.LoanAnalysisData.NewRow();
            ClosedRow["StageName"] = "Closed";
            ClosedRow["StageAlias"] = "Closed";
            ClosedRow["Href"] = string.Empty;
            ClosedRow["Amount"] = decimal.Zero;
            ClosedRow["LoanCounts"] = decimal.Zero;//gdc CR40
            this.LoanAnalysisData.Rows.Add(ClosedRow);

            DataRow CanceledRow = this.LoanAnalysisData.NewRow();
            CanceledRow["StageName"] = "Canceled";
            CanceledRow["StageAlias"] = "Canceled";
            CanceledRow["Href"] = string.Empty;
            CanceledRow["Amount"] = decimal.Zero;
            CanceledRow["LoanCounts"] = decimal.Zero;//gdc CR40
            this.LoanAnalysisData.Rows.Add(CanceledRow);

            DataRow DeniedRow = this.LoanAnalysisData.NewRow();
            DeniedRow["StageName"] = "Denied";
            DeniedRow["StageAlias"] = "Denied";
            DeniedRow["Href"] = string.Empty;
            DeniedRow["Amount"] = decimal.Zero;
            DeniedRow["LoanCounts"] = decimal.Zero;//gdc CR40
            this.LoanAnalysisData.Rows.Add(DeniedRow);

            //CR063 : Remove the “Suspended” status in the list of the “Archived Loans” status
            //DataRow SuspendedRow = this.LoanAnalysisData.NewRow();
            //SuspendedRow["StageName"] = "Suspended";
            //SuspendedRow["StageAlias"] = "Suspended";
            //SuspendedRow["Href"] = string.Empty;
            //SuspendedRow["Amount"] = decimal.Zero;
            //SuspendedRow["LoanCounts"] = decimal.Zero;//gdc CR40
            //this.LoanAnalysisData.Rows.Add(SuspendedRow);

            #endregion

            #region 添加Uncategorized分类

            DataRow UncategorizedRow = this.LoanAnalysisData.NewRow();
            UncategorizedRow["StageName"] = "Uncategorized";
            UncategorizedRow["StageAlias"] = "Uncategorized";
            UncategorizedRow["Href"] = string.Empty;
            UncategorizedRow["Amount"] = decimal.Zero;
            UncategorizedRow["LoanCounts"] = decimal.Zero;//gdc CR40

            this.LoanAnalysisData.Rows.Add(UncategorizedRow);

            #endregion
        }
        else
        {
            string sSql2 = "select Name as StageName, Alias as StageAlias, '' as Href, 0.00 as Amount,0 as LoanCounts from Template_Stages "  //gdc CR40  add ",0 as LoanCounts"
                         + "where WorkflowType=@WorkflowType and [Enabled]=1 order by SequenceNumber";
            SqlCommand SqlCmd2 = new SqlCommand(sSql2);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@WorkflowType", this.sWorkflowType);
            this.LoanAnalysisData = DbHelperSQL.ExecuteDataTable(SqlCmd2);

            #region 如果Lead，添加Uncategorized分类

            if (this.sWorkflowType == "Prospect") 
            {
                DataRow NewStageTempRow = this.LoanAnalysisData.NewRow();
                NewStageTempRow["StageName"] = "Uncategorized";
                NewStageTempRow["StageAlias"] = "Uncategorized";
                NewStageTempRow["Href"] = string.Empty;
                NewStageTempRow["Amount"] = decimal.Zero;
                NewStageTempRow["LoanCounts"] = decimal.Zero; //gdc CR40

                this.LoanAnalysisData.Rows.Add(NewStageTempRow);
            }

            #endregion
        }

        #endregion

        #region get user loan list

        DataTable UserLoanList = null;

        if (this.sWorkflowType == "Prospect")
        {
            #region Prospect

            string sStageField = string.Empty;

            if (sStageFilter == "CurrentStages")
            {
                sStageField = "b.Stage";
            }
            else
            {
                sStageField = "b.LastCompletedStage";
            }

            if (this.CurrentUser.sRoleName == "Branch Manager")
            {
                string sSqlm = "select " + sStageField + " as Stage, ISNULL(b.Amount,0) as Amount from lpfn_GetUserLeads_Branch_Manager(" + this.iCurrentUserID + ") as a inner join lpvw_PipelineInfo as b on a.LoanID = b.FileId "
                          + "where (1=1) " + sWhere;
                UserLoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlm);
            }
            else
            {
                if (this.CurrentUser.sRoleName == "Executive")
                {
                    string sSqlx1 = "select " + sStageField + " as Stage, ISNULL(b.Amount,0) as Amount from lpfn_GetUserLeads_Executive(" + this.iCurrentUserID + ") as a inner join lpvw_PipelineInfo as b on a.LoanID = b.FileId "
                              + "where (1=1) " + sWhere;
                    UserLoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx1);
                }
                else
                {
                    string sSqlx1 = "select " + sStageField + " as Stage, ISNULL(b.Amount,0) as Amount from lpfn_GetUserLeads(" + this.iCurrentUserID + ") as a inner join lpvw_PipelineInfo as b on a.LoanID = b.FileId "
                                              + "where (1=1) " + sWhere;
                    UserLoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx1);
                }
            }

            #endregion
        }
        else
        {
            string sStageField = string.Empty;

            if (sStageFilter == "CurrentStages")
            {
                sStageField = "b.Stage";
            }
            else
            {
                sStageField = "b.LastCompletedStage";
            }

            if (this.CurrentUser.sRoleName == "Branch Manager")
            {
                string sSqlm = "select " + sStageField + " as Stage, b.Amount as Amount from lpfn_GetUserLoans_Branch_Manager(" + this.iCurrentUserID + ") as a inner join V_ProcessingPipelineInfo as b on a.LoanID = b.FileId "
                             + "where (1=1) " + sWhere;

                UserLoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlm);
            }
            else
            {
                if (this.CurrentUser.sRoleName == "Executive")
                {
                    string sSqlx1 = "select " + sStageField + " as Stage, b.Amount as Amount from lpfn_GetUserLoans_Executive(" + this.iCurrentUserID + ") as a inner join V_ProcessingPipelineInfo as b on a.LoanID = b.FileId "
                                  + "where (1=1) " + sWhere;

                    UserLoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx1);
                }
                else
                {
                    string sSqlx1 = "select " + sStageField + " as Stage, b.Amount as Amount from lpfn_GetUserLoans(" + this.iCurrentUserID + ") as a inner join V_ProcessingPipelineInfo as b on a.LoanID = b.FileId "
                                  + "where (1=1) " + sWhere;

                    UserLoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx1);
                }
            }
        }

        #endregion

        #region set Amount and Href
        foreach (DataRow LoanAnaylysisRow in this.LoanAnalysisData.Rows)
        {
            string sStageName = LoanAnaylysisRow["StageName"].ToString();
            string sStageAlias = LoanAnaylysisRow["StageAlias"] == DBNull.Value ? LoanAnaylysisRow["StageName"].ToString() : LoanAnaylysisRow["StageAlias"].ToString();

            if (sStageName == "Uncategorized")
            {
                #region Amount

                string sSqlx10 = string.Empty;
                if (this.sWorkflowType == "Prospect")
                {
                    //sSqlx10 = "select isnull(SUM(b.Amount),0.00) as TotalAmount,count(1) as Total from lpfn_GetUserLoans(" + this.iCurrentUserID + ") as a inner join lpvw_PipelineInfo as b on a.LoanID=b.FileId "
                    //          + "where (select COUNT(1) from LoanStages where FileId=a.LoanID)=0 " + sWhere.Replace(" and (b.ProspectLoanStatus='Active')", "");
                    sSqlx10 = string.Format("select ISNULL(SUM(pi.Amount),0) as TotalAmount, count(1) as Total from lpvw_PipelineInfo pi inner join (select distinct FileId from lpvw_PipelineInfoGroupForPropectNew where  1>0  AND 1>0  AND FileId IN(SELECT FileId FROM Loans WHERE 1>0 AND Status='Prospect' AND dbo.lpfn_GetLoanStageCount(FileId) = 0  and (( 1>0 ) or ( 1>0 ))  AND ([FileId] IN (SELECT LoanID FROM dbo.[lpfn_GetUserLeads_Executive]({0}))))) tt on pi.FileId=tt.FileId", this.iCurrentUserID);
                 }
                else if (this.sWorkflowType == "Archived")
                {
                    //string sWhere10 = sWhere.Replace(" and (b.Status!='Processing') and (b.Status!='Prospect')", " and b.Status='Uncategorized Archive'");

                    //CR063 : Remove the “Suspended” status in the list of the “Archived Loans” status
                    string sWhere10 = sWhere.Replace(" and (b.Status!='Processing') and (b.Status!='Prospect') AND (b.Status != 'Suspended')", " and b.Status='Uncategorized Archive'");

                    sSqlx10 = "select isnull(SUM(b.Amount),0.00) as TotalAmount,count(1) as Total  from lpfn_GetUserLoans(" + this.iCurrentUserID + ") as a inner join lpvw_PipelineInfo as b on a.LoanID=b.FileId "
                              + "where 1=1 " + sWhere10;
                }

                //object oSum = LPWeb.DAL.DbHelperSQL.ExecuteScalar(sSqlx10);
                //decimal dSum = oSum == DBNull.Value ? decimal.Zero : Convert.ToDecimal(oSum);
                //LoanAnaylysisRow["Amount"] = dSum;

                decimal dSum = 0;
                int Total = 0;

                var dt = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx10);
                if (dt.Rows.Count > 0)
                {
                    dSum = dt.Rows[0]["TotalAmount"] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(dt.Rows[0]["TotalAmount"]);
                    Total = dt.Rows[0]["Total"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["Total"]);
                }

                if (sStageFilter == "LastCompletedStages")
                {
                    dSum = decimal.Zero;
                    Total = 0;
                }

                LoanAnaylysisRow["Amount"] = dSum;
                LoanAnaylysisRow["LoanCounts"] = Total;

                #endregion

                #region Href

                string sPipelineUrl = string.Empty;
                if (this.sWorkflowType == "Prospect")
                {
                    string sAliasString = this.BuildQueryString_PipelineSummary(sRegionID, sDivisionID, sBranchID, sDateType, sFromDate, sToDate, this.sWorkflowType, sStageAlias, sLoanOfficerIDs, sStageFilter);
                    sPipelineUrl = "Pipeline/ProspectPipelineSummaryLoan.aspx?q=" + Encrypter.Base64Encode(sAliasString);
                }
                else if (this.sWorkflowType == "Archived")
                {
                    string sAliasString = this.BuildQueryString_PipelineSummary(sRegionID, sDivisionID, sBranchID, sDateType, sFromDate, sToDate, sStageAlias, string.Empty, sLoanOfficerIDs, sStageFilter);
                    sPipelineUrl = "Pipeline/ProcessingPipelineSummary.aspx?q=" + Encrypter.Base64Encode(sAliasString);
                }

                LoanAnaylysisRow["Href"] = sPipelineUrl;

                #endregion
            }
            else
            {
                #region Amount

                object oLoanAmount = null;
                //oLoanAmount = UserLoanList.Compute("Sum(Amount)", "Stage='" + sStageName + "'");
                oLoanAmount = UserLoanList.Compute("Sum(Amount)", "Stage='" + sStageAlias + "'");

                //gdc CR40
                object oLoanTotal = null;
                oLoanTotal = UserLoanList.Compute("Count(Amount)", "Stage='" + sStageAlias + "'");
                
                decimal dLoanAmount = decimal.Zero;
                int iLoanTotal = 0;

                if ((oLoanAmount == null) || (oLoanAmount != DBNull.Value))
                {
                    dLoanAmount = Convert.ToDecimal(oLoanAmount, null);
                }

                //gdc CR40
                if ((oLoanTotal == null) || (oLoanTotal != DBNull.Value))
                {
                    iLoanTotal = Convert.ToInt32(oLoanTotal, null);
                }

                LoanAnaylysisRow["Amount"] = dLoanAmount;

                LoanAnaylysisRow["LoanCounts"] = iLoanTotal; //gdc CR40

                #endregion

                #region Href

                string sPipelineUrl = string.Empty;
                if (this.sWorkflowType == "Processing")
                {
                    string sAliasString = this.BuildQueryString_PipelineSummary(sRegionID, sDivisionID, sBranchID, sDateType, sFromDate, sToDate, this.sWorkflowType, sStageAlias, sLoanOfficerIDs, sStageFilter);
                    sPipelineUrl = "Pipeline/ProcessingPipelineSummary.aspx?q=" + Encrypter.Base64Encode(sAliasString);
                }
                else if (this.sWorkflowType == "Prospect")
                {
                    string sAliasString = this.BuildQueryString_PipelineSummary(sRegionID, sDivisionID, sBranchID, sDateType, sFromDate, sToDate, this.sWorkflowType, sStageAlias, sLoanOfficerIDs, sStageFilter);
                    sPipelineUrl = "Pipeline/ProspectPipelineSummaryLoan.aspx?q=" + Encrypter.Base64Encode(sAliasString);
                }
                else if (this.sWorkflowType == "Archived")
                {
                    string sAliasString = this.BuildQueryString_PipelineSummary(sRegionID, sDivisionID, sBranchID, sDateType, sFromDate, sToDate, sStageAlias, string.Empty, sLoanOfficerIDs, sStageFilter);
                    sPipelineUrl = "Pipeline/ProcessingPipelineSummary.aspx?q=" + Encrypter.Base64Encode(sAliasString);
                }

                string sHref = sPipelineUrl;
                LoanAnaylysisRow["Href"] = sHref;

                #endregion
            }
        }
        #endregion

        this.rptLoanAnalysis.DataSource = this.LoanAnalysisData;
        this.rptLoanAnalysis.DataBind();

        #endregion

        #region Build Query String for Sales Breakdown Chart

        Random x = new Random();
        int iRadomNum = x.Next(10000, 99999);

        StringBuilder sbChartQueryString = new StringBuilder("sid=" + iRadomNum);
        foreach (DataRow LoanAnaylysisRow in this.LoanAnalysisData.Rows)
        {
            string sStageAlias = LoanAnaylysisRow["StageAlias"].ToString();
            decimal dAmount = Convert.ToDecimal(LoanAnaylysisRow["Amount"]);

            if (dAmount > 0)
            {
                sbChartQueryString.Append("&" + Uri.EscapeUriString(sStageAlias) + "=" + dAmount);
            }
        }
        this.sChartQueryString = sbChartQueryString.ToString();

        #endregion

        #region where for organ. production chart

        string sWhereOrganPro = string.Empty;

        #region sWorkflowType

        if (this.sWorkflowType == "Archived")
        {
            sWhereOrganPro += " and (b.Status!='Processing') and (b.Status!='Prospect')";

            //CR063 : Remove the “Suspended” status in the list of the “Archived Loans” status
            sWhere += " AND (b.Status != 'Suspended')";
        }
        else
        {
            sWhereOrganPro += " and (b.Status='" + this.sWorkflowType + "')";
        }

        if (this.sWorkflowType == "Prospect")
        {
            sWhereOrganPro += " and (b.ProspectLoanStatus='Active')";
        }

        #endregion

        if (sRegionID != string.Empty)
        {
            sWhereOrganPro += " and (a.RegionID = " + sRegionID + ")";
        }

        if (sDivisionID != string.Empty)
        {
            sWhereOrganPro += " and (a.DivisionID = " + sDivisionID + ")";
        }

        if (sBranchID != string.Empty)
        {
            sWhereOrganPro += " and (a.BranchID = " + sBranchID + ")";
        }

        if (sDateType != string.Empty)
        {
            #region FromDate and ToDate

            DateTime? FromDate = null;
            DateTime? ToDate = null;

            if (sFromDate != string.Empty)
            {
                DateTime FromDate1;
                bool IsDate1 = DateTime.TryParse(sFromDate, out FromDate1);
                if (IsDate1 == true)
                {
                    FromDate = FromDate1;
                }
            }

            if (sToDate != string.Empty)
            {
                DateTime ToDate1;
                bool IsDate2 = DateTime.TryParse(sToDate, out ToDate1);
                if (IsDate2 == true)
                {
                    ToDate = ToDate1;
                }
            }

            string sFiledName = string.Empty;
            if (sDateType == "CloseDate")
            {
                sFiledName = "t.LastStageComplDate";
            }
            else
            {
                sFiledName = "t.Created";
            }

            sWhere += SqlTextBuilder.BuildDateSearchCondition(sFiledName, FromDate, ToDate);

            #endregion
        }

        if (sLoanOfficerIDs != string.Empty)
        {
            sWhere += " and dbo.lpfn_GetLoanOfficerID(t.FileId) in (" + sLoanOfficerIDs + ")";
        }

        this.sWhereEncode = Encrypter.Base64Encode(sWhereOrganPro);

        #endregion

        #region peter's codes

        string strRootUrl = "";
        if (null != ConfigurationManager.AppSettings["WPOWARootUrl"])
        {
            strRootUrl = ConfigurationManager.AppSettings["WPOWARootUrl"];
        }
        try
        {
            OWAInboxPart myInbox = new OWAInboxPart();
            myInbox.OWAServerAddressRoot = strRootUrl;
            myInbox.ViewName = ConfigurationManager.AppSettings["WPOWAInboxViewName"];
            this.phInbox.Controls.Add(myInbox);
        }
        catch (Exception ex)
        {
            Label lbl = new Label();
            lbl.Text = "Failed to load Webpart OWAInbox error: " + ex.Message;
            this.phInbox.Controls.Add(lbl);
            LPLog.LogMessage(LogType.Logerror, "Failed to load Webpart OWAInbox error: " + ex.Message);
        }
        try
        {
            OWACalendarPart myCalendar = new OWACalendarPart();
            myCalendar.OWAServerAddressRoot = strRootUrl;
            myCalendar.ViewName = ConfigurationManager.AppSettings["WPOWACalendarViewName"];
            this.phMyCalendar.Controls.Add(myCalendar);
        }
        catch (Exception ex)
        {
            Label lbl = new Label();
            lbl.Text = "Failed to load Webpart OWACalendar error: " + ex.Message;
            this.phMyCalendar.Controls.Add(lbl);
            LPLog.LogMessage(LogType.Logerror, "Failed to load Webpart OWACalendar error: " + ex.Message);
        }

        SPWeb web = this.Web;
        //SPList spList = web.Lists[System.Configuration.ConfigurationManager.AppSettings["WPComAnnName"]];
        //SPView spView = spList.Views[System.Configuration.ConfigurationManager.AppSettings["WPComAnnViewName"]];
        //this.xlvwpComAnn.ListId = spList.ID;
        //this.xlvwpComAnn.ListName = string.Format("{{{0}}}", spList.ID.ToString());
        //this.xlvwpComAnn.ViewGuid = spView.ID.ToString();

        #region =====show company announcement webpart=====

        if (this.CurrentUser.userRole.Announcements == true)
        {
            this.divCompanyAnn.Visible = true;
        }
        else
        {
            this.divCompanyAnn.Visible = false;
        }

        // User Home Profile
        if (UserHomePref2 != null)
        {
            if (UserHomePref2.Announcements == true)
            {
                this.divCompanyAnn.Visible = true;
            }
            else
            {
                this.divCompanyAnn.Visible = false;
            }
        }

        if (this.divCompanyAnn.Visible == true)
        {
            SPList spListComAnn = null;
            SPView spViewComAnn = null;
            try
            {
                spListComAnn = web.Lists[System.Configuration.ConfigurationManager.AppSettings["WPComAnnName"]];
            }
            catch
            {
                spListComAnn = null;
            }
            if (null != spListComAnn)
            {
                try
                {
                    spViewComAnn = spListComAnn.Views[System.Configuration.ConfigurationManager.AppSettings["WPComAnnViewName"]];
                }
                catch
                {
                    spViewComAnn = null;
                }
                if (null != spViewComAnn)
                {
                    strComAnnListUrl = spListComAnn.DefaultViewUrl;

                    XsltListViewWebPart xlvwpComAnn = new XsltListViewWebPart();
                    xlvwpComAnn.ListId = spListComAnn.ID;
                    xlvwpComAnn.ListName = string.Format("{{{0}}}", spListComAnn.ID.ToString());
                    xlvwpComAnn.ViewGuid = spViewComAnn.ID.ToString();
                    //xlvwpComAnn.Toolbar = "None";
                    xlvwpComAnn.XmlDefinition = @"
            <View Name=""{EC6E2014-F0A2-4273-B6BC-1A9F00110341}"" MobileView=""TRUE"" Type=""HTML"" Hidden=""TRUE"" DisplayName="""" Url=""/SitePages/Home.aspx"" Level=""1"" BaseViewID=""1"" ContentTypeID=""0x"" ImageUrl=""/_layouts/images/announce.png"">
              <Query>
                <OrderBy>
                  <FieldRef Name=""Modified"" Ascending=""FALSE""/>
                </OrderBy>
              </Query>
              <ViewFields>
                <FieldRef Name=""LinkTitle""/>
              </ViewFields>
              <RowLimit Paged=""TRUE"">5</RowLimit>
              <Toolbar Type=""None""/>
            </View>";
                    xlvwpComAnn.Xsl = @"
            <xsl:stylesheet xmlns:x=""http://www.w3.org/2001/XMLSchema"" xmlns:d=""http://schemas.microsoft.com/sharepoint/dsp"" version=""1.0"" exclude-result-prefixes=""xsl msxsl ddwrt"" xmlns:ddwrt=""http://schemas.microsoft.com/WebParts/v2/DataView/runtime"" xmlns:asp=""http://schemas.microsoft.com/ASPNET/20"" xmlns:__designer=""http://schemas.microsoft.com/WebParts/v2/DataView/designer"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:SharePoint=""Microsoft.SharePoint.WebControls"" xmlns:ddwrt2=""urn:frontpage:internal"" xmlns:o=""urn:schemas-microsoft-com:office:office"">
              <xsl:include href=""/_layouts/xsl/main.xsl""/>
              <xsl:include href=""/_layouts/xsl/internal.xsl""/>
              <xsl:param name=""AllRows"" select=""/dsQueryResponse/Rows/Row[$EntityName = '' or (position() &gt;= $FirstRow and position() &lt;= $LastRow)]""/>
              <xsl:param name=""dvt_apos"">'</xsl:param>
        
              <xsl:template name=""FieldRef_ValueOf.Modified"" ddwrt:dvt_mode=""body"" ddwrt:ghost="""" xmlns:ddwrt2=""urn:frontpage:internal"">
                <xsl:param name=""thisNode"" select="".""/>
                <span style=""color: #FF00FF"">
                  <xsl:value-of select=""$thisNode/@*[name()=current()/@Name]"" />
                </span>
              </xsl:template>
            </xsl:stylesheet>";
                    this.phComAnn.Controls.Add(xlvwpComAnn);
                }
                else
                {
                    Label lbl = new Label();
                    lbl.Text = string.Format("Can not find the specified webpart view with name \"{0}\".",
                        System.Configuration.ConfigurationManager.AppSettings["WPComAnnViewName"]);
                    this.phComAnn.Controls.Add(lbl);
                }
            }
            else
            {
                Label lbl = new Label();
                lbl.Text = string.Format("Can not find the specified webpart with name \"{0}\".",
                    System.Configuration.ConfigurationManager.AppSettings["WPComAnnName"]);
                this.phComAnn.Controls.Add(lbl);
            }
        }
        #endregion

        #region =====show company calendar webpart=====

        if (this.CurrentUser.userRole.CompanyCalendar == true)
        {
            this.divComCal.Visible = true;
        }
        else
        {
            this.divComCal.Visible = false;
        }

        // User Home Profile
        if (UserHomePref2 != null)
        {
            if (UserHomePref2.CompanyCalendar == true)
            {
                this.divComCal.Visible = true;
            }
            else
            {
                this.divComCal.Visible = false;
            }
        }

        if (this.divComCal.Visible == true)
        {
            SPList spListComCal = null;
            SPView spViewComCal = null;
            try
            {
                spListComCal = web.Lists[System.Configuration.ConfigurationManager.AppSettings["WPComCalName"]];
            }
            catch
            {
                spListComCal = null;
            }
            if (null != spListComCal)
            {
                try
                {
                    spViewComCal = spListComCal.Views[System.Configuration.ConfigurationManager.AppSettings["WPComCalViewName"]];
                }
                catch
                {
                    spViewComCal = null;
                }
                if (null != spViewComCal)
                {
                    strComCalListUrl = spListComCal.DefaultViewUrl;

                    XsltListViewWebPart xlvwpComCal = new XsltListViewWebPart();
                    xlvwpComCal.ListId = spListComCal.ID;
                    xlvwpComCal.ListName = string.Format("{{{0}}}", spListComCal.ID.ToString());
                    xlvwpComCal.ViewGuid = spViewComCal.ID.ToString();
                    //xlvwpComCal.Toolbar = "None";
                    xlvwpComCal.XmlDefinition = @"<View Name=""{3A64E1C5-8CDF-418C-A94F-5E66F61EB5CF}"" MobileView=""TRUE"" Type=""HTML"" Hidden=""TRUE"" DisplayName="""" Url=""/SitePages/Test webparts.aspx"" Level=""1"" BaseViewID=""2"" ContentTypeID=""0x"" MobileUrl=""_layouts/mobile/viewdaily.aspx"" ImageUrl=""/_layouts/images/events.png"">
        				<Query>
        					<Where>
        						<DateRangesOverlap>
        							<FieldRef Name=""EventDate""/>
        							<FieldRef Name=""EndDate""/>
        							<FieldRef Name=""RecurrenceID""/>
        							<Value Type=""DateTime"">
        								<Month/>
        							</Value>
        						</DateRangesOverlap>
        					</Where>
        				</Query>
        				<ViewFields>
        					<FieldRef Name=""Title""/>
        					<FieldRef Name=""EventDate""/>
        				</ViewFields>
        				<Toolbar Type=""None""/>
        			</View>";
                    xlvwpComCal.Xsl = @"<xsl:stylesheet xmlns:x=""http://www.w3.org/2001/XMLSchema"" xmlns:d=""http://schemas.microsoft.com/sharepoint/dsp"" version=""1.0"" exclude-result-prefixes=""xsl msxsl ddwrt"" xmlns:ddwrt=""http://schemas.microsoft.com/WebParts/v2/DataView/runtime"" xmlns:asp=""http://schemas.microsoft.com/ASPNET/20"" xmlns:__designer=""http://schemas.microsoft.com/WebParts/v2/DataView/designer"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:SharePoint=""Microsoft.SharePoint.WebControls"" xmlns:ddwrt2=""urn:frontpage:internal"" xmlns:o=""urn:schemas-microsoft-com:office:office""> 
          <xsl:include href=""/_layouts/xsl/main.xsl""/> 
          <xsl:include href=""/_layouts/xsl/internal.xsl""/> 
          			<xsl:param name=""AllRows"" select=""/dsQueryResponse/Rows/Row[$EntityName = '' or (position() &gt;= $FirstRow and position() &lt;= $LastRow)]""/>
          			<xsl:param name=""dvt_apos"">'</xsl:param>
        			<xsl:template name=""FieldRef_Recurrence_body.fRecurrence"" ddwrt:dvt_mode=""body"" match=""FieldRef[@Name='fRecurrence']"" mode=""Recurrence_body"" ddwrt:ghost="""" xmlns:ddwrt2=""urn:frontpage:internal"">
            			<xsl:param name=""thisNode"" select="".""/>
            			<xsl:variable name=""fRecurrence"" select=""$thisNode/@*[name()=current()/@Name]""/>
            			<xsl:variable name=""src"">/_layouts/images/
        					<xsl:choose>
                				<xsl:when test=""$fRecurrence='1'"">
                  					<xsl:choose>
                    					<xsl:when test=""$thisNode/@EventType='3'"">recurEx.gif</xsl:when>
                    					<xsl:when test=""$thisNode/@EventType='4'"">recurEx.gif</xsl:when>
                    					<xsl:otherwise>recur.gif</xsl:otherwise>
                  					</xsl:choose>
                </xsl:when>
                				<xsl:otherwise>blank.gif</xsl:otherwise>
              				</xsl:choose>
            </xsl:variable>
            			<xsl:variable name=""alt"">
              				<xsl:if test=""$fRecurrence='1'"">
                				<xsl:choose>
                  					<xsl:when test=""@EventType='3'"">
                    <xsl:value-of select=""'Exception to Recurring Event'""/>
                  </xsl:when>
                  					<xsl:when test=""@EventType='4'"">
                    <xsl:value-of select=""'Exception to Recurring Event'""/>
                  </xsl:when>
                  					<xsl:otherwise>
                    <xsl:value-of select=""'Recurring Event'""/>
                  </xsl:otherwise>
                				</xsl:choose>
              </xsl:if>
            </xsl:variable>
            <img border=""0"" width=""16"" height=""16"" src=""/_layouts/images/
        			blank.gif"" alt=""{$alt}"" title=""{$alt}""/>
          </xsl:template></xsl:stylesheet>";
                    this.phComCal.Controls.Add(xlvwpComCal);
                }
                else
                {
                    Label lbl = new Label();
                    lbl.Text = string.Format("Can not find the specified webpart view with name \"{0}\".",
                        System.Configuration.ConfigurationManager.AppSettings["WPComCalViewName"]);
                    this.phComCal.Controls.Add(lbl);
                }
            }
            else
            {
                Label lbl = new Label();
                lbl.Text = string.Format("Can not find the specified webpart with name \"{0}\".",
                    System.Configuration.ConfigurationManager.AppSettings["WPComCalName"]);
                this.phComCal.Controls.Add(lbl);
            }
        }
        #endregion

        #region =====show rates sheet webpart=====

        if (this.CurrentUser.userRole.RateSummary == true)
        {
            this.divRates.Visible = true;
        }
        else
        {
            this.divRates.Visible = false;
        }

        // User Home Profile
        if (UserHomePref2 != null)
        {
            if (UserHomePref2.RateSummary == true)
            {
                this.divRates.Visible = true;
            }
            else
            {
                this.divRates.Visible = false;
            }
        }

        if (this.divRates.Visible == true)
        {
            SPList spListRates = null;
            SPView spViewRates = null;
            try
            {
                spListRates = web.Lists[System.Configuration.ConfigurationManager.AppSettings["WPRatesName"]];
            }
            catch
            {
                spListRates = null;
            }
            if (null != spListRates)
            {
                try
                {
                    spViewRates = spListRates.Views[System.Configuration.ConfigurationManager.AppSettings["WPRatesViewName"]];
                }
                catch
                {
                    spViewRates = null;
                }
                if (null != spViewRates)
                {
                    strRatesListUrl = spListRates.DefaultViewUrl;

                    XsltListViewWebPart xlvwpRates = new XsltListViewWebPart();
                    xlvwpRates.ListId = spListRates.ID;
                    xlvwpRates.ListName = string.Format("{{{0}}}", spListRates.ID.ToString());
                    xlvwpRates.ViewGuid = spViewRates.ID.ToString();
                    //xlvwpRates.Toolbar = "None";
                    xlvwpRates.XmlDefinition = @"<View Name=""{FE66F61E-EBF7-49E8-BD37-F9955793DDCE}"" MobileView=""TRUE"" Type=""HTML"" Hidden=""TRUE"" DisplayName="""" Url=""/SitePages/Test webparts.aspx"" Level=""1"" BaseViewID=""1"" ContentTypeID=""0x"" ImageUrl=""/_layouts/images/dlicon.png"">
        				<Query>
        					<OrderBy>
        						<FieldRef Name=""FileLeafRef""/>
        					</OrderBy>
        				</Query>
        				<ViewFields>
        					<FieldRef Name=""LinkFilename""/>
        				</ViewFields>
        				<RowLimit Paged=""TRUE"">30</RowLimit>
        				<Toolbar Type=""None""/>
        			</View>";
                    xlvwpRates.Xsl = @"
            <xsl:stylesheet xmlns:x=""http://www.w3.org/2001/XMLSchema"" xmlns:d=""http://schemas.microsoft.com/sharepoint/dsp"" version=""1.0"" exclude-result-prefixes=""xsl msxsl ddwrt"" xmlns:ddwrt=""http://schemas.microsoft.com/WebParts/v2/DataView/runtime"" xmlns:asp=""http://schemas.microsoft.com/ASPNET/20"" xmlns:__designer=""http://schemas.microsoft.com/WebParts/v2/DataView/designer"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:SharePoint=""Microsoft.SharePoint.WebControls"" xmlns:ddwrt2=""urn:frontpage:internal"" xmlns:o=""urn:schemas-microsoft-com:office:office"">
              <xsl:include href=""/_layouts/xsl/main.xsl""/>
              <xsl:include href=""/_layouts/xsl/internal.xsl""/>
              <xsl:param name=""AllRows"" select=""/dsQueryResponse/Rows/Row[$EntityName = '' or (position() &gt;= $FirstRow and position() &lt;= $LastRow)]""/>
              <xsl:param name=""dvt_apos"">'</xsl:param>
        
              <xsl:template name=""FieldRef_ValueOf.Modified"" ddwrt:dvt_mode=""body"" ddwrt:ghost="""" xmlns:ddwrt2=""urn:frontpage:internal"">
                <xsl:param name=""thisNode"" select="".""/>
                <span style=""color: #FF00FF"">
                  <xsl:value-of select=""$thisNode/@*[name()=current()/@Name]"" />
                </span>
              </xsl:template>
            </xsl:stylesheet>";
                    this.phRates.Controls.Add(xlvwpRates);
                }
                else
                {
                    Label lbl = new Label();
                    lbl.Text = string.Format("Can not find the specified webpart view with name \"{0}\".",
                        System.Configuration.ConfigurationManager.AppSettings["WPComCalViewName"]);
                    this.phRates.Controls.Add(lbl);
                }
            }
            else
            {
                Label lbl = new Label();
                lbl.Text = string.Format("Can not find the specified webpart with name \"{0}\".",
                    System.Configuration.ConfigurationManager.AppSettings["WPRatesName"]);
                this.phRates.Controls.Add(lbl);
            }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// add "All Regions" item
    /// neo 2010-09-24
    /// </summary>
    private void AddAddRegionItem(DataTable RegionListData, int iPos) 
    {
        DataRow NewRegionRow = RegionListData.NewRow();
        NewRegionRow["RegionID"] = "0";
        NewRegionRow["Name"] = "All Regions";
        NewRegionRow["Enabled"] = true;
        RegionListData.Rows.InsertAt(NewRegionRow, iPos);
    }

    /// <summary>
    /// QueryString for Changke's pipeline summary
    /// </summary>
    /// <param name="sRegionID"></param>
    /// <param name="sDivisionID"></param>
    /// <param name="sBranchID"></param>
    /// <param name="sDateType"></param>
    /// <param name="sFromDate"></param>
    /// <param name="sToDate"></param>
    /// <param name="sStatus"></param>
    /// <param name="sCurrentStage"></param>
    /// <returns></returns>
    private string BuildQueryString_PipelineSummary(string sRegionID, string sDivisionID, string sBranchID, string sDateType, string sFromDate, string sToDate, string sStatus, string sCurrentStage, string sLoanOfficerIDs, string sStageFilter) 
    {
        StringBuilder sbQueryString = new StringBuilder("RegionID=<%=RegionID%>&DivisionID=<%=DivisionID%>&BranchID=<%=BranchID%>&DateType=<%=DateType%>&FromDate=<%=FromDate%>&ToDate=<%=ToDate%>&Status=<%=Status%>&LoanOfficerIDs=<%=LoanOfficerIDs%>&StageFilter=<%=StageFilter%>");

        #region RegionID
        if (sRegionID == string.Empty)
        {
            sbQueryString.Replace("<%=RegionID%>", "-1");
        }
        else
        {
            sbQueryString.Replace("<%=RegionID%>", sRegionID);
        }
        #endregion

        #region DivisionID
        if (sDivisionID == string.Empty)
        {
            sbQueryString.Replace("<%=DivisionID%>", "-1");
        }
        else
        {
            sbQueryString.Replace("<%=DivisionID%>", sDivisionID);
        }
        #endregion

        #region sBranchID
        if (sBranchID == string.Empty)
        {
            sbQueryString.Replace("<%=BranchID%>", "-1");
        }
        else
        {
            sbQueryString.Replace("<%=BranchID%>", sBranchID);
        }
        #endregion

        #region sDateType
        if (sDateType == string.Empty)
        {
            sbQueryString.Replace("<%=DateType%>", string.Empty);
        }
        else
        {
            sbQueryString.Replace("<%=DateType%>", sDateType);
        }
        #endregion

        #region sFromDate
        if (sFromDate == string.Empty)
        {
            sbQueryString.Replace("<%=FromDate%>", string.Empty);
        }
        else
        {
            sbQueryString.Replace("<%=FromDate%>", DateTime.Parse(sFromDate).ToString("yyyy-MM-dd"));
        }
        #endregion

        #region sToDate
        if (sToDate == string.Empty)
        {
            sbQueryString.Replace("<%=ToDate%>", string.Empty);
        }
        else
        {
            sbQueryString.Replace("<%=ToDate%>", DateTime.Parse(sToDate).ToString("yyyy-MM-dd"));
        }
        #endregion
        
        // Status
        sbQueryString.Replace("<%=Status%>", sStatus);

        // CurrentStage
        if(sCurrentStage != string.Empty)
        {
            sbQueryString.Append("&CurrentStage=" + sCurrentStage);
        }

        #region sLoanOfficerIDs
        if (sLoanOfficerIDs == string.Empty)
        {
            sbQueryString.Replace("<%=LoanOfficerIDs%>", string.Empty);
        }
        else
        {
            sbQueryString.Replace("<%=LoanOfficerIDs%>", sLoanOfficerIDs);
        }
        #endregion

        if (sStageFilter == string.Empty)
        {
            sbQueryString.Replace("<%=StageFilter%>", string.Empty);
        }
        else
        {
            sbQueryString.Replace("<%=StageFilter%>", sStageFilter);
        }
        
        return sbQueryString.ToString();
    }


    public string link()
    {
        string link = "0";
         LPWeb.BLL.LoanTasks lt = new LPWeb.BLL.LoanTasks();

         if (CurrentUser.RemindTaskDue == true)
         {
             string ltask = lt.GetLoanTasks(int.Parse(CurrentUser.TaskReminder.ToString()), iCurrentUserID);

             if (ltask != "0")
             {
                 link = ltask;
             }
         }

         return link;
    }
}