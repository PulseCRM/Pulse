using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Settings
{
    /// <summary>
    /// Personalization - Preferences
    /// Author: Peter
    /// Date: 2011-05-25
    /// </summary>
    public partial class PersonalizationPreferences : BasePage
    {
        BLL.Users UsersManager = new BLL.Users();
        BLL.UserPipelineColumns UserPipelineColsManager = new BLL.UserPipelineColumns();
        BLL.UserProspectColumns UserProspectColsManager = new BLL.UserProspectColumns();
        BLL.UserHomePref UserHomePrefManager = new BLL.UserHomePref();
        const int ROLEID_EXECUTIVE = 1;     // Executive RoleId
        const int ROLEID_BRANCHMANAGER = 2; // Branch manager RoleId
        const int ROLEID_LO = 3;            // Loan Officer RoleId

        protected void Page_Load(object sender, EventArgs e)
        {
            Model.Users userInfo = UsersManager.GetModel(CurrUser.iUserID);
            if (!IsPostBack)
            {
                if (!CurrUser.userRole.CustomUserHome)
                {
                    this.ckbCompanyCalendar.Enabled = false;
                    this.ckbRatesSummary.Enabled = false;
                    this.ckbPipelineChart.Enabled = false;
                    this.ckbGoalsChart.Enabled = false;
                    this.ckbSalesBreakdownChart.Enabled = false;
                    this.ckbOverdueTasks.Enabled = false;
                    this.ckbOrgProductionChart.Enabled = false;
                    this.ckbAnnouncements.Enabled = false;
                    this.ckbOrgProductSaleBreakdownChart.Enabled = false;
                    this.ckbExchangeInbox.Enabled = false;
                    this.ckbExchangeCalendar.Enabled = false;
                    this.ckbQuickleadform.Enabled = false;
                }

                if (ROLEID_EXECUTIVE != CurrUser.iRoleID && ROLEID_BRANCHMANAGER != CurrUser.iRoleID)
                {
                    this.ckbSalesBreakdownChart.Enabled = false;
                    this.ckbOrgProductionChart.Enabled = false;
                    this.ckbOrgProductSaleBreakdownChart.Enabled = false;
                    ClientFun("isneedcheck", "needCheckAllChart = false;");
                }

                Model.UserPipelineColumns userPipelineCols = UserPipelineColsManager.GetModel(CurrUser.iUserID);
                Model.UserProspectColumns userProspectCols = UserProspectColsManager.GetModel(CurrUser.iUserID);
                Model.UserHomePref userHomePref = UserHomePrefManager.GetModel(CurrUser.iUserID);
                if (null == userInfo)
                {
                    LPLog.LogMessage(LogType.Logerror, string.Format("User Personalization: User with id {0} does not exist.", CurrUser.iUserID));
                    ClientFun("unknowerrormsg", "alert('User does not exists, unknow error.');");
                }
                
                ListItem item = this.ddlLoanPerPage.Items.FindByValue(userInfo.LoansPerPage.ToString());
                if (null != item)
                {
                    this.ddlLoanPerPage.ClearSelection();
                    item.Selected = true;
                }
                //gdc CR47
                this.cbxShowTasksInLSR.Checked = userInfo.ShowTasksInLSR;

                //gdc CR48
                this.cbxRemindTaskDue.Checked = userInfo.RemindTaskDue;

                //gdc CR48

                if (!string.IsNullOrEmpty(userInfo.TaskReminder.ToString()) && userInfo.TaskReminder.ToString()!="0")
                {
                    this.txtReminderTime.Text = userInfo.TaskReminder.ToString();
                    if (this.cbxRemindTaskDue.Checked == true)
                    {
                        txtReminderTime.Enabled = true;
                    }
                }
                else
                {
                    this.txtReminderTime.Text = "15";
                }

                if (!string.IsNullOrEmpty(userInfo.SortTaskPickList.ToString()))
                {
                    ListItem itemSortTaskPickList = this.ddlSortTaskPickList.Items.FindByValue(userInfo.SortTaskPickList.ToString());
                    if (null != itemSortTaskPickList)
                    {
                        this.ddlSortTaskPickList.ClearSelection();
                        itemSortTaskPickList.Selected = true;
                    }
                }


                if (null != userPipelineCols)
                    SetUserPipelineColumnsInfo(userPipelineCols);
                if (null != userProspectCols)
                    SetUserProspectColumnsInfo(userProspectCols);
                if (null != userHomePref)
                    SetUserHomePrefInfo(userHomePref);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Model.Users userInfo = UsersManager.GetModel(CurrUser.iUserID);
            Model.UserPipelineColumns userPipelineCols = UserPipelineColsManager.GetModel(CurrUser.iUserID);
            Model.UserProspectColumns userProspectCols = UserProspectColsManager.GetModel(CurrUser.iUserID);
            Model.UserHomePref userHomePref = UserHomePrefManager.GetModel(CurrUser.iUserID);

            try
            {
                if (null == userInfo)
                {
                    LPLog.LogMessage(LogType.Logerror, string.Format("User Personalization - Preferences: User with id {0} does not exist.", CurrUser.iUserID));
                    ClientFun("unknowerrmsg2", "alert('User does not exists, unknow error.');");
                }
                if (!GetUserInfo(ref userInfo))
                {
                    ClientFun("invalidinputmsg", "alert('Invalid input!');");
                    return;
                }
                UsersManager.Update(userInfo);

                if (null == userPipelineCols)
                {
                    userPipelineCols = new Model.UserPipelineColumns();
                    userPipelineCols.UserId = CurrUser.iUserID;
                    GetUserPipelineColumnsInfo(ref userPipelineCols);
                    UserPipelineColsManager.Add(userPipelineCols);
                }
                else
                {
                    GetUserPipelineColumnsInfo(ref userPipelineCols);
                    UserPipelineColsManager.Update(userPipelineCols);
                }

                if (null == userProspectCols)
                {
                    userProspectCols = new Model.UserProspectColumns();
                    userProspectCols.UserId = CurrUser.iUserID;
                    GetUserProspectColumnsInfo(ref userProspectCols);
                    UserProspectColsManager.Add(userProspectCols);
                }
                else
                {
                    GetUserProspectColumnsInfo(ref userProspectCols);
                    UserProspectColsManager.Update(userProspectCols);
                }

                if (null == userHomePref)
                {
                    userHomePref = new Model.UserHomePref();
                    userHomePref.UserId = CurrUser.iUserID;
                    GetUserHomePrefInfo(ref userHomePref);
                    UserHomePrefManager.Add(userHomePref);
                }
                else
                {
                    GetUserHomePrefInfo(ref userHomePref);
                    UserHomePrefManager.Update(userHomePref);
                }

                ClientFun("sucsmsg", "alert('Saved!');");
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to save user personalization preferences info, reason:" + ex.Message);
                LPLog.LogMessage(LogType.Logerror, "Failed to save user personalization preferences info: " + ex.Message);
                return;
            }
        }

        /// <summary>
        /// load Users Personalization Info
        /// </summary>
        /// <param name="user"></param>
        private bool GetUserInfo(ref Model.Users user)
        {
            int nLoanPerPage = 10;
            if (!int.TryParse(this.ddlLoanPerPage.SelectedValue, out nLoanPerPage))
                nLoanPerPage = 10;
            user.LoansPerPage = nLoanPerPage;
            user.ShowTasksInLSR = cbxShowTasksInLSR.Checked;

            user.RemindTaskDue = cbxRemindTaskDue.Checked;

            if (!string.IsNullOrEmpty(txtReminderTime.Text.Trim()))
            {
                user.TaskReminder = int.Parse(txtReminderTime.Text.Trim());
            }
            else
            {
                user.TaskReminder = 15;
            }

            user.SortTaskPickList = ddlSortTaskPickList.SelectedValue;
            
            return true;
        }

        /// <summary>
        /// load UserPipelineColumns Personalization Info
        /// </summary>
        /// <param name="pipelineCols"></param>
        private bool GetUserPipelineColumnsInfo(ref Model.UserPipelineColumns pipelineCols)
        {
            pipelineCols.PointFolder = this.ckbPointFolder.Checked;
            pipelineCols.Stage = this.ckbStage.Checked;
            pipelineCols.Branch = this.ckbBranch.Checked;
            pipelineCols.EstimatedClose = this.ckbEstimatedClose.Checked;
            pipelineCols.Alerts = this.ckbAlerts.Checked;
            pipelineCols.LoanOfficer = this.ckbLoanOfficer.Checked;
            pipelineCols.Amount = this.ckbAmount.Checked;
            pipelineCols.Lien = this.ckbLien.Checked;
            pipelineCols.Rate = this.ckbRate.Checked;
            pipelineCols.Lender = this.ckbLender.Checked;
            pipelineCols.LockExp = this.ckbLockExpirDate.Checked;
            pipelineCols.PercentCompl = this.ckbPercentComplete.Checked;
            pipelineCols.Processor = this.ckbProcessor.Checked;
            pipelineCols.TaskCount = this.ckbTaskCount.Checked;
            pipelineCols.PointFileName = this.ckbFilename.Checked;
            pipelineCols.LastCompletedStage = this.chkLastComplStage.Checked;
            pipelineCols.LastStageComplDate = this.chkLastStageComlDate.Checked;

            // CR49
            pipelineCols.Purpose = this.cbxPurpose.Checked;


            //gdc CR40
            pipelineCols.Assistant = this.ckbAssistant.Checked;
            pipelineCols.DocPrep = this.ckbDocPrep.Checked;
            pipelineCols.Shipper = this.ckbShipper.Checked;
            pipelineCols.Closer = this.ckbCloser.Checked;

            //gdc CR47
            pipelineCols.LoanProgram = this.cbxLoanProgram.Checked;

            //gdc CR48
            pipelineCols.QuickLeadForm = this.ckbQuickleadform.Checked;

            pipelineCols.JrProcessor = this.cbxJrProcessor.Checked;

            return true;
        }

        /// <summary>
        /// load UserProspectColumns Personalization Info
        /// </summary>
        /// <param name="pipelineCols"></param>
        private bool GetUserProspectColumnsInfo(ref Model.UserProspectColumns prospectCols)
        {
            prospectCols.Pv_Created = this.ckbpvCreate.Checked;
            prospectCols.Pv_Leadsource = this.ckbpvLeadSource.Checked;
            prospectCols.Pv_Branch = this.ckbpvBranch.Checked;
            prospectCols.Pv_Loanofficer = this.ckbpvLoanOfficer.Checked;
            prospectCols.Pv_Refcode = this.ckbpvRefCode.Checked;
            //    prospectCols.Pv_Progress = this.ckbpvProgress.Checked;

            prospectCols.Lv_Amount = this.ckblvAmount.Checked;
            prospectCols.Lv_Branch = this.ckblvBranch.Checked;
            prospectCols.Lv_Estclose = this.ckblvEstClose.Checked;
            prospectCols.Lv_Leadsource = this.ckblvLeadSource.Checked;
            prospectCols.Lv_Lien = this.ckblvLien.Checked;
            prospectCols.Lv_Loanofficer = this.ckblvLoanOfficer.Checked;
            prospectCols.Lv_Loanprogram = this.ckblvLoanProgram.Checked;
            prospectCols.Lv_Pointfilename = this.ckblvPointFilename.Checked;
            prospectCols.Lv_Progress = this.ckblvProgress.Checked;
            prospectCols.Lv_Ranking = this.ckblvRanking.Checked;
            prospectCols.Lv_Rate = this.ckblvRate.Checked;
            prospectCols.Lv_Refcode = this.ckblvRefCode.Checked;
            prospectCols.Pv_Referral = this.ckbpvReferral.Checked;
            prospectCols.Pv_Partner = this.ckbpvPartner.Checked;
            prospectCols.Lv_Referral = this.ckblvReferral.Checked;
            prospectCols.Lv_Partner = this.ckblvPartner.Checked;

            prospectCols.LastCompletedStage = this.chklvLastComplStage.Checked;
            prospectCols.LastStageComplDate = this.chklvLastStageComplDate.Checked;


            return true;
        }

        /// <summary>
        /// load UserHomePref Personalization Info
        /// </summary>
        /// <param name="homePref"></param>
        private bool GetUserHomePrefInfo(ref Model.UserHomePref homePref)
        {
            homePref.CompanyCalendar = this.ckbCompanyCalendar.Checked;
            homePref.PipelineChart = this.ckbPipelineChart.Checked;
            homePref.SalesBreakdownChart = this.ckbSalesBreakdownChart.Checked;
            homePref.OrgProductionChart = this.ckbOrgProductionChart.Checked;
            homePref.Org_N_Sales_Charts = this.ckbOrgProductSaleBreakdownChart.Checked;
            homePref.RateSummary = this.ckbRatesSummary.Checked;
            homePref.GoalsChart = this.ckbGoalsChart.Checked;
            homePref.OverDueTaskAlert = this.ckbOverdueTasks.Checked;
            homePref.Announcements = this.ckbAnnouncements.Checked;
            homePref.ExchangeInbox = this.ckbExchangeInbox.Checked;
            homePref.ExchangeCalendar = this.ckbExchangeCalendar.Checked;

            homePref.QuickLeadForm = this.ckbQuickleadform.Checked;

            homePref.AlertFilter = Convert.ToInt32(this.ddlAlertsFilter.SelectedValue.Trim());

            homePref.DashboardLastCompletedStages = Convert.ToInt32(this.ddlDashboardLastCompletedStages.SelectedValue);
            homePref.DefaultClientsPipelineViewId = Convert.ToInt32(this.ddlDefaultClientsPV.SelectedValue);
            homePref.DefaultLeadsPipelineViewId = Convert.ToInt32(this.ddlDefaultLeadsPV.SelectedValue);
            homePref.DefaultLoansPipelineViewId = Convert.ToInt32(this.ddlDefaultLoansPV.SelectedValue);

            return true;
        }

        /// <summary>
        /// bind UserHomePref Info
        /// </summary>
        /// <param name="homePref"></param>
        private bool SetUserHomePrefInfo(Model.UserHomePref homePref)
        {
            this.ckbCompanyCalendar.Checked = homePref.CompanyCalendar;
            this.ckbPipelineChart.Checked = homePref.PipelineChart;
            this.ckbSalesBreakdownChart.Checked = homePref.SalesBreakdownChart;
            this.ckbOrgProductionChart.Checked = homePref.OrgProductionChart;
            this.ckbOrgProductSaleBreakdownChart.Checked = homePref.Org_N_Sales_Charts;
            this.ckbRatesSummary.Checked = homePref.RateSummary;
            this.ckbGoalsChart.Checked = homePref.GoalsChart;
            this.ckbOverdueTasks.Checked = homePref.OverDueTaskAlert;
            this.ckbAnnouncements.Checked = homePref.Announcements;
            this.ckbExchangeInbox.Checked = homePref.ExchangeInbox;
            this.ckbExchangeCalendar.Checked = homePref.ExchangeCalendar;


            this.ckbQuickleadform.Checked = homePref.QuickLeadForm;


            this.ddlAlertsFilter.SelectedValue = homePref.AlertFilter == 0 ? "1" : homePref.AlertFilter.ToString();

            //gdc CR45

            BLL.UserPipelineViews userPipelineViewsBll = new BLL.UserPipelineViews();


            ddlDefaultClientsPV.DataSource = userPipelineViewsBll.GetList_ViewName("PipelineType = 'Clients' AND Enabled = 1 ", "ViewName asc");
            ddlDefaultClientsPV.DataBind();
            ddlDefaultClientsPV.Items.Insert(0, new ListItem("-- select --", "0"));
            ddlDefaultClientsPV.SelectedValue = homePref.DefaultClientsPipelineViewId.ToString();

            ddlDefaultLeadsPV.DataSource = userPipelineViewsBll.GetList_ViewName("PipelineType = 'Leads' AND Enabled = 1 ", "ViewName asc");
            ddlDefaultLeadsPV.DataBind();
            ddlDefaultLeadsPV.Items.Insert(0, new ListItem("-- select --", "0"));
            ddlDefaultLeadsPV.SelectedValue = homePref.DefaultLeadsPipelineViewId.ToString();

            ddlDefaultLoansPV.DataSource = userPipelineViewsBll.GetList_ViewName("PipelineType = 'Loans' AND Enabled = 1 ", "ViewName asc");
            ddlDefaultLoansPV.DataBind();
            ddlDefaultLoansPV.Items.Insert(0, new ListItem("-- select --", "0"));
            ddlDefaultLoansPV.SelectedValue = homePref.DefaultLoansPipelineViewId.ToString();

            ddlDashboardLastCompletedStages.SelectedValue = homePref.DashboardLastCompletedStages.ToString();

            return true;
        }

        /// <summary>
        /// bind UserPipelineColumns Info
        /// </summary>
        /// <param name="pipelineCols"></param>
        private bool SetUserPipelineColumnsInfo(Model.UserPipelineColumns pipelineCols)
        {
            this.ckbPointFolder.Checked = pipelineCols.PointFolder;
            this.ckbStage.Checked = pipelineCols.Stage;
            this.ckbBranch.Checked = pipelineCols.Branch;
            this.ckbEstimatedClose.Checked = pipelineCols.EstimatedClose;
            this.ckbAlerts.Checked = pipelineCols.Alerts;
            this.ckbLoanOfficer.Checked = pipelineCols.LoanOfficer;
            this.ckbAmount.Checked = pipelineCols.Amount;
            this.ckbLien.Checked = pipelineCols.Lien;
            this.ckbRate.Checked = pipelineCols.Rate;
            this.ckbLender.Checked = pipelineCols.Lender;
            this.ckbLockExpirDate.Checked = pipelineCols.LockExp;
            this.ckbPercentComplete.Checked = pipelineCols.PercentCompl;
            this.ckbProcessor.Checked = pipelineCols.Processor;
            this.ckbTaskCount.Checked = pipelineCols.TaskCount;
            this.ckbFilename.Checked = pipelineCols.PointFileName;

            this.chkLastComplStage.Checked = pipelineCols.LastCompletedStage;
            this.chkLastStageComlDate.Checked = pipelineCols.LastStageComplDate;

            //gdc CR40
            this.ckbCloser.Checked = pipelineCols.Closer;
            this.ckbShipper.Checked = pipelineCols.Shipper;
            this.ckbDocPrep.Checked = pipelineCols.DocPrep;
            this.ckbAssistant.Checked = pipelineCols.Assistant;

            //gdc CR47

            this.cbxLoanProgram.Checked = pipelineCols.LoanProgram;

            //CR49
            this.cbxPurpose.Checked = pipelineCols.Purpose;

            //gdc CR51
            this.cbxJrProcessor.Checked = pipelineCols.JrProcessor;

            return true;
        }

        /// <summary>
        /// bind UserProspectColumns Info
        /// </summary>
        /// <param name="pipelineCols"></param>
        private bool SetUserProspectColumnsInfo(Model.UserProspectColumns prospectCols)
        {
            this.ckbpvBranch.Checked = prospectCols.Pv_Branch;
            this.ckbpvCreate.Checked = prospectCols.Pv_Created;
            this.ckbpvLeadSource.Checked = prospectCols.Pv_Leadsource;
            this.ckbpvLoanOfficer.Checked = prospectCols.Pv_Loanofficer;
            //   this.ckbpvProgress.Checked = prospectCols.Pv_Progress;
            this.ckbpvRefCode.Checked = prospectCols.Pv_Refcode;
            this.ckbpvReferral.Checked = prospectCols.Pv_Referral;
            this.ckbpvPartner.Checked = prospectCols.Pv_Partner;

            this.ckblvAmount.Checked = prospectCols.Lv_Amount;
            this.ckblvBranch.Checked = prospectCols.Lv_Branch;
            this.ckblvEstClose.Checked = prospectCols.Lv_Estclose;
            this.ckblvLeadSource.Checked = prospectCols.Lv_Leadsource;
            this.ckblvLien.Checked = prospectCols.Lv_Lien;
            this.ckblvLoanOfficer.Checked = prospectCols.Lv_Loanofficer;
            this.ckblvLoanProgram.Checked = prospectCols.Lv_Loanprogram;
            this.ckblvPointFilename.Checked = prospectCols.Lv_Pointfilename;
            this.ckblvProgress.Checked = prospectCols.Lv_Progress;
            this.ckblvRanking.Checked = prospectCols.Lv_Ranking;
            this.ckblvRate.Checked = prospectCols.Lv_Rate;
            this.ckblvRefCode.Checked = prospectCols.Lv_Refcode;
            this.ckblvReferral.Checked = prospectCols.Lv_Referral;
            this.ckblvPartner.Checked = prospectCols.Lv_Partner;

            this.chklvLastComplStage.Checked = prospectCols.LastCompletedStage;
            this.chklvLastStageComplDate.Checked = prospectCols.LastStageComplDate;
            return true;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            int nPCSCheckedCount = 0;   // Pipeline Column Selections count
            if (this.ckbPointFolder.Checked) nPCSCheckedCount++;
            if (this.ckbAmount.Checked) nPCSCheckedCount++;
            if (this.ckbPercentComplete.Checked) nPCSCheckedCount++;
            if (this.ckbStage.Checked) nPCSCheckedCount++;
            if (this.ckbLien.Checked) nPCSCheckedCount++;
            if (this.ckbProcessor.Checked) nPCSCheckedCount++;
            if (this.ckbBranch.Checked) nPCSCheckedCount++;
            if (this.ckbRate.Checked) nPCSCheckedCount++;
            if (this.ckbTaskCount.Checked) nPCSCheckedCount++;
            if (this.ckbAlerts.Checked) nPCSCheckedCount++;
            if (this.ckbLender.Checked) nPCSCheckedCount++;
            if (this.ckbFilename.Checked) nPCSCheckedCount++;
            if (this.ckbLoanOfficer.Checked) nPCSCheckedCount++;
            if (this.ckbLockExpirDate.Checked) nPCSCheckedCount++;
            if (this.ckbEstimatedClose.Checked) nPCSCheckedCount++;
            if (this.chkLastComplStage.Checked) nPCSCheckedCount++;
            if (this.chkLastStageComlDate.Checked) nPCSCheckedCount++;

            if (this.cbxPurpose.Checked) nPCSCheckedCount++;
            if (this.ckbCloser.Checked) nPCSCheckedCount++;
            if (this.ckbShipper.Checked) nPCSCheckedCount++; 
            if (this.ckbDocPrep.Checked) nPCSCheckedCount++; 
            if (this.ckbAssistant.Checked) nPCSCheckedCount++; 
            if (this.cbxLoanProgram.Checked) nPCSCheckedCount++;
            if (this.cbxJrProcessor.Checked) nPCSCheckedCount++;


            ClientFun("pcscheckedcount", string.Format("nPCSChecked={0};", nPCSCheckedCount));

            int nPVPCSChecked = 0;  // Pipeline Loan Column Selections count
            if (this.ckbpvBranch.Checked) nPVPCSChecked++;
            if (this.ckbpvCreate.Checked) nPVPCSChecked++;
            if (this.ckbpvLeadSource.Checked) nPVPCSChecked++;
            if (this.ckbpvLoanOfficer.Checked) nPVPCSChecked++;
            //     if (this.ckbpvProgress.Checked) nPVPCSChecked++;
            if (this.ckbpvRefCode.Checked) nPVPCSChecked++;
            if (this.ckbpvReferral.Checked) nPVPCSChecked++;
            if (this.ckbpvPartner.Checked) nPVPCSChecked++;
            ClientFun("pvpcscheckedcount", string.Format("nPVPCSChecked={0};", nPVPCSChecked));

            int nPsCSCheckedCount = 0;   // Pipeline Loan Column Selections count
            if (this.ckblvAmount.Checked) nPsCSCheckedCount++;
            if (this.ckblvBranch.Checked) nPsCSCheckedCount++;
            if (this.ckblvEstClose.Checked) nPsCSCheckedCount++;
            if (this.ckblvLeadSource.Checked) nPsCSCheckedCount++;
            if (this.ckblvLien.Checked) nPsCSCheckedCount++;
            if (this.ckblvLoanOfficer.Checked) nPsCSCheckedCount++;
            if (this.ckblvLoanProgram.Checked) nPsCSCheckedCount++;
            if (this.ckblvPointFilename.Checked) nPsCSCheckedCount++;
            if (this.ckblvProgress.Checked) nPsCSCheckedCount++;
            if (this.ckblvRanking.Checked) nPsCSCheckedCount++;
            if (this.ckblvRate.Checked) nPsCSCheckedCount++;
            if (this.ckblvRefCode.Checked) nPsCSCheckedCount++;
            if (this.ckblvReferral.Checked) nPsCSCheckedCount++;
            if (this.ckblvPartner.Checked) nPsCSCheckedCount++;

            if (this.chklvLastComplStage.Checked) nPsCSCheckedCount++;
            if (this.chklvLastStageComplDate.Checked) nPsCSCheckedCount++;
            ClientFun("pscscheckedcount", string.Format("nPPsCSChecked={0};", nPsCSCheckedCount));

            int nHPCheckedCount = 0;    // Homepage Selections count 
            if (this.ckbCompanyCalendar.Checked) nHPCheckedCount++;
            if (this.ckbPipelineChart.Checked) nHPCheckedCount++;
            if (this.ckbSalesBreakdownChart.Checked) nHPCheckedCount++;
            if (this.ckbOrgProductionChart.Checked) nHPCheckedCount++;
            if (this.ckbOrgProductSaleBreakdownChart.Checked) nHPCheckedCount++;
            if (this.ckbRatesSummary.Checked) nHPCheckedCount++;
            if (this.ckbGoalsChart.Checked) nHPCheckedCount++;
            if (this.ckbOverdueTasks.Checked) nHPCheckedCount++;
            if (this.ckbAnnouncements.Checked) nHPCheckedCount++;
            if (this.ckbExchangeInbox.Checked) nHPCheckedCount++;
            if (this.ckbExchangeCalendar.Checked) nHPCheckedCount++;

            if (this.ckbQuickleadform.Checked) nHPCheckedCount++;

            ClientFun("hpcheckedcount", string.Format("nHPChecked={0};", nHPCheckedCount));
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
