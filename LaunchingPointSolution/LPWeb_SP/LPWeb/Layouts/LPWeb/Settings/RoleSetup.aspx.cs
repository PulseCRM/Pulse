using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;

public partial class Settings_RoleSetup : BasePage
{
    private readonly Roles _bllRoles = new Roles();
    private readonly List<string> _listCanSelectRoles = new List<string> { "Executive", "Branch Manager" };

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //权限验证
            var loginUser = new LoginUser();
            if (!loginUser.userRole.CompanySetup)
            {
                Response.Redirect("../Unauthorize.aspx");
                return;
            }
            BindRoleDropDownListValues();

            int roleId = int.Parse(ddlRoleName.SelectedValue);

            if (Request.QueryString["RoleId"] != null && !string.IsNullOrEmpty(Request.QueryString["RoleId"].ToString()))
            {
                int.TryParse(Request.QueryString["RoleId"].ToString().Trim(), out roleId);
            }
            SetPageValuesByRoleId(roleId);
        }
    }

    #region Method

    /// <summary>
    /// 设置页面默认显示值
    /// </summary>
    private void BindRoleDropDownListValues()
    {
        var dataSource = _bllRoles.GetAllList();
        if (dataSource == null || dataSource.Tables.Count == 0 || dataSource.Tables[0].Rows.Count == 0)
            return;
        ddlRoleName.DataSource = dataSource;
        ddlRoleName.DataTextField = "Name";
        ddlRoleName.DataValueField = "RoleId";
        ddlRoleName.DataBind();
    }

    /// <summary>
    /// 根据role name所选设置页面控件的值
    /// </summary>
    /// <param name="roleId">roleId</param>
    private void SetPageValuesByRoleId(int roleId)
    {
        if (roleId <= 0)
            return;

        LPWeb.Model.Roles currentModel = _bllRoles.GetModel(roleId);
        if (currentModel == null)
            return;

        ddlRoleName.SelectedValue = currentModel.RoleId.ToString();
        cbxAccCompany.Checked = currentModel.CompanySetup;
        cbxAccLoan.Checked = currentModel.LoanSetup;
        //OtherLoanAccess对应的是All Loans。如"OtherLoanAccess" 字段等於1, 则"All loans"选中, 否则"Assigned Loans"选中
        rblAccRights.SelectedValue = currentModel.OtherLoanAccess ? "All loans" : "Assigned loans";
        cbxCusUserHomePage.Checked = currentModel.CustomUserHome;

        //Contact Management Rights
        SetCheckedRights(cbxContactCreate, cbxContactModify, cbxContactDelete, currentModel.ContactMgmt);

        if (!currentModel.ContactMgmt.HasValue)
        {
            cbxContactReassign.Checked = false;
            cbxContactView.Checked = false;

            cbxContactMerge.Checked = false;
            cbxContactExport.Checked = false;
        }
        else
        {
            string strRights = currentModel.ContactMgmt.Value.ToString();

            cbxContactReassign.Checked = strRights.Contains("4");
            cbxContactView.Checked = strRights.Contains("5");
            cbxContactMerge.Checked = strRights.Contains("6");

            cbxContactExport.Checked = strRights.Contains("7");//gdc CR45
        }

        //Prospect Rights
        string strProspectRights = currentModel.Prospect.ToString();
        cbxProspectCreate.Checked = strProspectRights.Contains("A");
        cbxProspectModify.Checked = strProspectRights.Contains("B");
        cbxProspectDelete.Checked = strProspectRights.Contains("C");
        cbxProspectView.Checked = strProspectRights.Contains("D");
        cbxProspectSearch.Checked = strProspectRights.Contains("E");
        cbxProspectDispose.Checked = strProspectRights.Contains("F");
        cbxProspectImport.Checked = strProspectRights.Contains("G");
        cbxProspectUpdatePoint.Checked = strProspectRights.Contains("H");
        cbxProspectAssign.Checked = strProspectRights.Contains("I");
        cbxProspectMerge.Checked = strProspectRights.Contains("J");
        cbxProspectLinkLoans.Checked = strProspectRights.Contains("K");
        cbxProspectViewNotes.Checked = strProspectRights.Contains("L");
        cbxProspectAddNotes.Checked = strProspectRights.Contains("M");
        cbxProspectViewEmails.Checked = strProspectRights.Contains("N");
        cbxProspectSendEmails.Checked = strProspectRights.Contains("O");

        cbxProspectExport.Checked = strProspectRights.Contains("P"); //gdc CR45

        //Loan Rights
        string strProspectLoanRights = currentModel.Loans.ToString();
        cbxProspectLoanCreate.Checked = strProspectLoanRights.Contains("A");
        cbxProspectLoanModify.Checked = strProspectLoanRights.Contains("B");
        cbxProspectLoanDelete.Checked = strProspectLoanRights.Contains("C");
        cbxProspectLoanView.Checked = strProspectLoanRights.Contains("D");
        cbxProspectLoanSearch.Checked = strProspectLoanRights.Contains("E");
        cbxProspectLoanDispose.Checked = strProspectLoanRights.Contains("F");
        cbxProspectLoanSync.Checked = strProspectLoanRights.Contains("G");
        cbxProspectLoanViewNotes.Checked = strProspectLoanRights.Contains("H");
        cbxProspectLoanAddNotes.Checked = strProspectLoanRights.Contains("I");
        cbxProspectLoanViewEmails.Checked = strProspectLoanRights.Contains("J");
        cbxProspectLoanSendEmails.Checked = strProspectLoanRights.Contains("K");

        cbxExportPipelines.Checked = currentModel.ExportPipelines; //gdc CR45
        chkUpdateCondition.Checked = currentModel.UpdateCondition; //Rocky CR65

        //AccessAllContacts
        cbxAccessAllContacts.Checked = currentModel.AccessAllContacts;

        //ContactCompany
        string strContactCompany = currentModel.ContactCompany == null ? "" : currentModel.ContactCompany.Value.ToString();
        cbxCTCompanyView.Checked = strContactCompany.Contains("1");
        cbxCTCompanyCreate.Checked = strContactCompany.Contains("2");
        cbxCTCompanyModify.Checked = strContactCompany.Contains("3");
        cbxCTCompanyDelete.Checked = strContactCompany.Contains("4");
        cbxCTCompanyDisable.Checked = strContactCompany.Contains("5");
        cbxCTCompanyAddBranches.Checked = strContactCompany.Contains("6");
        cbxCTCompanyRemoveBranches.Checked = strContactCompany.Contains("7");

        //ContactBranch
        string strContactBranch = currentModel.ContactBranch == null ? "" : currentModel.ContactBranch.Value.ToString();
        cbxCTBranchView.Checked = strContactBranch.Contains("1");
        cbxCTBranchCreate.Checked = strContactBranch.Contains("2");
        cbxCTBranchModify.Checked = strContactBranch.Contains("3");
        cbxCTBranchDelete.Checked = strContactBranch.Contains("4");
        cbxCTBranchDisable.Checked = strContactBranch.Contains("5");
        cbxCTBranchAddContacts.Checked = strContactBranch.Contains("6");
        cbxCTBranchRemoveContacts.Checked = strContactBranch.Contains("7");

        //Service Type
        string strServiceType = currentModel.ServiceType == null ? "" : currentModel.ServiceType.Value.ToString();
        cbxCTServiceView.Checked = strServiceType.Contains("1");
        cbxCTServiceCreate.Checked = strServiceType.Contains("2");
        cbxCTServiceModify.Checked = strServiceType.Contains("3");
        cbxCTServiceDelete.Checked = strServiceType.Contains("4");
        cbxCTServiceDisable.Checked = strServiceType.Contains("5");

        //Contact Role:
        string strContactRole = currentModel.ContactRole == null ? "" : currentModel.ContactRole.Value.ToString();
        cbxCTRoleView.Checked = strContactRole.Contains("1");
        cbxCTRoleCreate.Checked = strContactRole.Contains("2");
        cbxCTRoleModify.Checked = strContactRole.Contains("3");
        cbxCTRoleDelete.Checked = strContactRole.Contains("4");
        cbxCTRoleDisable.Checked = strContactRole.Contains("5");

        //Marketing Role:
        string strMarketing = currentModel.Marketing == null ? "" : currentModel.Marketing.Value.ToString();
        //this.chkAddMarketing.Checked = strMarketing.Contains("1");
        //this.chkRemoveMarketing.Checked = strMarketing.Contains("2");
        //this.chkVewMarketing.Checked = strMarketing.Contains("3");

        //Task & Alert Rights
        SetCheckedRights(cbxWfCreate, cbxWfModify, cbxWfDel, currentModel.WorkflowTempl);
        SetCheckedRights(cbxLoanCreate, cbxLoanModify, cbxLoanDel, currentModel.CustomTask); //todo:confirm the field 
        SetCheckedRights(cbxAlertRulesCreate, cbxAlertRulesModify, cbxAlertRulesDel, currentModel.AlertRules);
        SetCheckedRights(cbxAlertRuleTempCreate, cbxAlertRuleTempModify, cbxAlertRuleTempDel,
                         currentModel.AlertRuleTempl);

        #region Conditions
        string cond = currentModel.ConditionRights;
        if (string.IsNullOrEmpty(cond))
        {
            cbxConditionsAssign.Checked = false;
            cbxConditionsClear.Checked = false;
            cbxConditionsCreate.Checked = false;
            cbxConditionsDel.Checked = false;
            cbxConditionsModify.Checked = false;
        }
        else
        {
            if (cond.Contains("1"))
            {
                cbxConditionsCreate.Checked = true;
            }
            if (cond.Contains("2"))
            {
                cbxConditionsModify.Checked = true;
            }
            if (cond.Contains("3"))
            {
                cbxConditionsDel.Checked = true;
            }
            if (cond.Contains("4"))
            {
                cbxConditionsAssign.Checked = true;
            }
            if (cond.Contains("5"))
            {
                cbxConditionsClear.Checked = true;
            }
            if (cond.Contains("6"))
            {
                cbxEnableLSR.Checked = true;
            }
        }
        #endregion


        bool markOtherTaskCompl = false;
        if (rbMarkTasks_All.Checked)
            markOtherTaskCompl = true;

        if (currentModel.MarkOtherTaskCompl)
        {
            rbMarkTasks_All.Checked = true;
            rbMarkTasks_Ass.Checked = false;
        }
        else
        {
            rbMarkTasks_All.Checked = false;
            rbMarkTasks_Ass.Checked = true;
        }
        //rblMarkTasks.SelectedValue = currentModel.MarkOtherTaskCompl ? "All tasks" : "Assigned tasks";

        cbxAssignTasks.Checked = currentModel.AssignTask;

        //Loan Management Rights

        cbxImportLoans.Checked = currentModel.ImportLoan;
        cbxRemoveLoans.Checked = currentModel.RemoveLoan;
        cbxLoanReassignment.Checked = currentModel.AssignLoan;
        cbxApplyWf.Checked = currentModel.ApplyWorkflow;
        cbxApplyAlertRule.Checked = currentModel.ApplyAlertRule;
        cbxSendMail.Checked = currentModel.SendEmail;
        cbxCreateNotes.Checked = currentModel.CreateNotes;
        cbxSendLSR.Checked =currentModel.SendLSR;
        cbxExtendRateLock.Checked = currentModel.ExtendRateLock;
        cbxViewLockInfo.Checked = currentModel.ViewLockInfo;
        cbxLockRate.Checked = currentModel.LockRate;
        cbxAccessProfitability.Checked = currentModel.AccessProfitability;
        cbxViewCompensation.Checked = currentModel.ViewCompensation;

        //Homepage Selections(pick up to six)

        cbxCompanyCalendar.Checked = currentModel.CompanyCalendar;
        cbxRatesSummary.Checked = currentModel.RateSummary;
        cbxPipelineChart.Checked = currentModel.PipelineChart;
        cbxGoalsChart.Checked = currentModel.GoalsChart;

        cbxOverdueTasks.Checked = currentModel.OverdueTaskAlerts;

        cbxCompanyAnn.Checked = currentModel.Announcements;


        if (_listCanSelectRoles.Contains(currentModel.Name.Trim()))
        {
            cbxPipelineSummaryWithSales.Enabled = true;
            cbxPipelineSummaryWithOrg.Enabled = true;
            cbxPipelineSummaryWithOrgAndSales.Enabled = true;

            cbxPipelineSummaryWithSales.Checked = currentModel.SalesBreakdownChart;
            cbxPipelineSummaryWithOrg.Checked = currentModel.OrgProductionChart;
            cbxPipelineSummaryWithOrgAndSales.Checked = currentModel.Org_N_Sales_Charts;
        }
        else
        {
            cbxPipelineSummaryWithSales.Checked = false;
            cbxPipelineSummaryWithOrg.Checked = false;
            cbxPipelineSummaryWithOrgAndSales.Checked = false;
            cbxPipelineSummaryWithSales.Enabled = false;
            cbxPipelineSummaryWithOrg.Enabled = false;
            cbxPipelineSummaryWithOrgAndSales.Enabled = false;
        }

        cbxExchangeInbox.Checked = currentModel.ExchangeInbox;
        cbxExchangeCalendar.Checked = currentModel.ExchangeCalendar;

        if (currentModel.SetUserGoals)
        {
            rblSetProductionGoals.SelectedValue = "0";
        }

        if (currentModel.SetOwnGoals)
        {
            rblSetProductionGoals.SelectedValue = "1";
        }

        cbxAccessReports.Checked = currentModel.Reports;

        //MailChimp 
        cbxAccessAllMailChimpList.Checked = currentModel.AccessAllMailChimpList;

        //gdc CR47
        cbxUnassignedLeads.Checked = currentModel.AccessUnassignedLeads;
    }

    /// <summary>
    /// Get rights base on create,modify and delete checkbox
    /// </summary>
    /// <param name="cbxCreate">Create right checkbox</param>
    /// <param name="cbxModify">Modify right checkbox</param>
    /// <param name="cbxDel">Delete right checkbox</param>
    /// <returns></returns>
    protected int GetCheckedRights(CheckBox cbxCreate, CheckBox cbxModify, CheckBox cbxDel)
    {
        string rights = string.Empty;
        if (cbxCreate.Checked) //Create:1
            rights = "1";

        if (cbxModify.Checked) //Modify:2
            rights += "2";

        if (cbxDel.Checked) //Delete:3
            rights += "3";

        if (string.IsNullOrEmpty(rights))
            return 0;

        return int.Parse(rights);
    }

    /// <summary>
    /// 根据权限值设置页面增，改，删的checbox是否选中
    /// </summary>
    /// <param name="cbxCreate">Create right checkbox</param>
    /// <param name="cbxModify">Modify right checkbox</param>
    /// <param name="cbxDel">Delete right checkbox</param>
    /// <param name="rights">rights</param>
    /// <returns></returns>
    protected void SetCheckedRights(CheckBox cbxCreate, CheckBox cbxModify, CheckBox cbxDel, int? rights)
    {
        if (!rights.HasValue)
        {
            cbxCreate.Checked = false;
            cbxModify.Checked = false;
            cbxDel.Checked = false;
            return;
        }

        string strRights = rights.Value.ToString();
        cbxCreate.Checked = strRights.Contains("1");//Create:1

        cbxModify.Checked = strRights.Contains("2"); //Modify:2

        cbxDel.Checked = strRights.Contains("3");//Delete:3
    }

    #endregion Method

    #region Events

    /// <summary>
    /// role选择改变执行事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRoleName_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetPageValuesByRoleId(int.Parse(ddlRoleName.SelectedValue));
    }

    /// <summary>
    /// Save Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        var model = new LPWeb.Model.Roles();

        model.RoleId = int.Parse(ddlRoleName.SelectedValue);

        model.Name = ddlRoleName.SelectedItem.Text.Trim();
        model.CompanySetup = cbxAccCompany.Checked;
        model.LoanSetup = cbxAccLoan.Checked;

        model.OtherLoanAccess = rblAccRights.SelectedValue == "All loans";
        //OtherLoanAccess对应的是All Loans。如"OtherLoanAccess" 字段等於1, 则"All loans"选中, 否则"Assigned Loans"选中
        model.CustomUserHome = cbxCusUserHomePage.Checked;

        //Contact Management Rights
        var cmd = GetCheckedRights(cbxContactCreate, cbxContactModify, cbxContactDelete);
        string strValue = "";
        if (cmd != 0)
            strValue = cmd.ToString();
        if (cbxContactReassign.Checked)//reassign
        {
            strValue += "4";
        }

        if (cbxContactView.Checked)//view
        {
            strValue += "5";
        }
        if (cbxContactMerge.Checked)//Merge
        {
            strValue += "6";
        }

        //gdc CR45
        if (cbxContactExport.Checked) //Export
        {
            strValue += "7";
        }


        if (strValue.Length > 0)
            model.ContactMgmt = int.Parse(strValue);

        //Prospect Rights
        string strProspectValue = "";
        if (cbxProspectCreate.Checked)
        {
            strProspectValue += "A";
        }
        if (cbxProspectModify.Checked)
        {
            strProspectValue += "B";
        }
        if (cbxProspectDelete.Checked)
        {
            strProspectValue += "C";
        }
        if (cbxProspectView.Checked)
        {
            strProspectValue += "D";
        }
        if (cbxProspectSearch.Checked)
        {
            strProspectValue += "E";
        }
        if (cbxProspectDispose.Checked)
        {
            strProspectValue += "F";
        }
        if (cbxProspectImport.Checked)
        {
            strProspectValue += "G";
        }
        if (cbxProspectUpdatePoint.Checked)
        {
            strProspectValue += "H";
        }
        if (cbxProspectAssign.Checked)
        {
            strProspectValue += "I";
        }
        if (cbxProspectMerge.Checked)
        {
            strProspectValue += "J";
        }
        if (cbxProspectLinkLoans.Checked)
        {
            strProspectValue += "K";
        }
        if (cbxProspectViewNotes.Checked)
        {
            strProspectValue += "L";
        }
        if (cbxProspectAddNotes.Checked)
        {
            strProspectValue += "M";
        }
        if (cbxProspectViewEmails.Checked)
        {
            strProspectValue += "N";
        }
        if (cbxProspectSendEmails.Checked)
        {
            strProspectValue += "O";
        }

        //gdc CR45
        if (cbxProspectExport.Checked)
        {
            strProspectValue += "P";
        }


        if (strProspectValue.Length > 0)
            model.Prospect = strProspectValue;

        //Loan Rights
        string strProspectLoanValue = "";
        if (cbxProspectLoanCreate.Checked)
        {
            strProspectLoanValue += "A";
        }
        if (cbxProspectLoanModify.Checked)
        {
            strProspectLoanValue += "B";
        }
        if (cbxProspectLoanDelete.Checked)
        {
            strProspectLoanValue += "C";
        }
        if (cbxProspectLoanView.Checked)
        {
            strProspectLoanValue += "D";
        }
        if (cbxProspectLoanSearch.Checked)
        {
            strProspectLoanValue += "E";
        }
        if (cbxProspectLoanDispose.Checked)
        {
            strProspectLoanValue += "F";
        }
        if (cbxProspectLoanSync.Checked)
        {
            strProspectLoanValue += "G";
        }
        if (cbxProspectLoanViewNotes.Checked)
        {
            strProspectLoanValue += "H";
        }
        if (cbxProspectLoanAddNotes.Checked)
        {
            strProspectLoanValue += "I";
        }
        if (cbxProspectLoanViewEmails.Checked)
        {
            strProspectLoanValue += "J";
        }
        if (cbxProspectLoanSendEmails.Checked)
        {
            strProspectLoanValue += "K";
        }

        model.ExportPipelines = cbxExportPipelines.Checked; //gdc CR45
        model.UpdateCondition = chkUpdateCondition.Checked; //Rocky CR65
         
        if (strProspectLoanValue.Length > 0)
            model.Loans = strProspectLoanValue;

        //AccessAllContacts
        model.AccessAllContacts = cbxAccessAllContacts.Checked;

        //ContactCompany
        string strContactCompany = "";
        if (cbxCTCompanyView.Checked)
        {
            strContactCompany += "1";
        }
        if (cbxCTCompanyCreate.Checked)
        {
            strContactCompany += "2";
        }
        if (cbxCTCompanyModify.Checked)
        {
            strContactCompany += "3";
        }
        if (cbxCTCompanyDelete.Checked)
        {
            strContactCompany += "4";
        }
        if (cbxCTCompanyDisable.Checked)
        {
            strContactCompany += "5";
        }
        if (cbxCTCompanyAddBranches.Checked)
        {
            strContactCompany += "6";
        }
        if (cbxCTCompanyRemoveBranches.Checked)
        {
            strContactCompany += "7";
        }
        if (strContactCompany.Length > 0)
            model.ContactCompany =  int.Parse(strContactCompany);

        //ContactBranch
        string strContactBranch = "";
        if (cbxCTBranchView.Checked)
        {
            strContactBranch += "1";
        }
        if (cbxCTBranchCreate.Checked)
        {
            strContactBranch += "2";
        }
        if (cbxCTBranchModify.Checked)
        {
            strContactBranch += "3";
        }
        if (cbxCTBranchDelete.Checked)
        {
            strContactBranch += "4";
        }
        if (cbxCTBranchDisable.Checked)
        {
            strContactBranch += "5";
        }
        if (cbxCTBranchAddContacts.Checked)
        {
            strContactBranch += "6";
        }
        if (cbxCTBranchRemoveContacts.Checked)
        {
            strContactBranch += "7";
        }
        if (strContactBranch.Length > 0)
            model.ContactBranch = int.Parse(strContactBranch);

        //ServiceType
        string sServiceType = "";
        if (cbxCTServiceView.Checked)
        {
            sServiceType += "1";
        }
        if (cbxCTServiceCreate.Checked)
        {
            sServiceType += "2";
        }
        if (cbxCTServiceModify.Checked)
        {
            sServiceType += "3";
        }
        if (cbxCTServiceDelete.Checked)
        {
            sServiceType += "4";
        }
        if (cbxCTServiceDisable.Checked)
        {
            sServiceType += "5";
        }
        if (sServiceType.Length > 0)
            model.ServiceType = int.Parse(sServiceType);

        //Contact Role
        string sContactRole = "";
        if (cbxCTRoleView.Checked)
        {
            sContactRole += "1";
        }
        if (cbxCTRoleCreate.Checked)
        {
            sContactRole += "2";
        }
        if (cbxCTRoleModify.Checked)
        {
            sContactRole += "3";
        }
        if (cbxCTRoleDelete.Checked)
        {
            sContactRole += "4";
        }
        if (cbxCTRoleDisable.Checked)
        {
            sContactRole += "5";
        }
        if (sContactRole.Length > 0)
            model.ContactRole = int.Parse(sContactRole);

        //Task & Alert Rights

        int wfRights = GetCheckedRights(cbxWfCreate, cbxWfModify, cbxWfDel);
        if (wfRights != 0)
            model.WorkflowTempl = wfRights;

        int loanTaskRights = GetCheckedRights(cbxLoanCreate, cbxLoanModify, cbxLoanDel);
        if (loanTaskRights != 0)
            model.CustomTask = loanTaskRights;

        int alertRuleRights = GetCheckedRights(cbxAlertRulesCreate, cbxAlertRulesModify, cbxAlertRulesDel);
        if (alertRuleRights != 0)
            model.AlertRules = alertRuleRights;

        int alertRuleTempRights = GetCheckedRights(cbxAlertRuleTempCreate, cbxAlertRuleTempModify, cbxAlertRuleTempDel);
        if (alertRuleTempRights != 0)
            model.AlertRuleTempl = alertRuleTempRights;


        #region Conditions

        string cond = "";

        if (cbxConditionsCreate.Checked)
        {
            cond += "1";
        }
        if (cbxConditionsModify.Checked)
        {
            cond += "2";
        }
        if (cbxConditionsDel.Checked)
        {
            cond += "3";
        }
        if (cbxConditionsAssign.Checked)
        {
            cond += "4";
        }
        if (cbxConditionsClear.Checked)
        {
            cond += "5";
        }
        if (cbxEnableLSR.Checked)
        {
            cond += "6";
        }
        model.ConditionRights = cond;
        #endregion

        bool markOtherTaskCompl = false;
        if (rbMarkTasks_All.Checked)
            markOtherTaskCompl = true;
        model.MarkOtherTaskCompl = markOtherTaskCompl;// rblMarkTasks.SelectedValue == "All tasks";

        model.AssignTask = cbxAssignTasks.Checked;

        //Loan Management Rights

        model.ImportLoan = cbxImportLoans.Checked;
        model.RemoveLoan = cbxRemoveLoans.Checked;
        model.AssignLoan = cbxLoanReassignment.Checked;
        model.ApplyWorkflow = cbxApplyWf.Checked;
        model.ApplyAlertRule = cbxApplyAlertRule.Checked;
        model.SendEmail = cbxSendMail.Checked;
        model.CreateNotes = cbxCreateNotes.Checked;
        model.SendLSR = cbxSendLSR.Checked;
        model.ExtendRateLock = cbxExtendRateLock.Checked;
        model.ViewLockInfo = cbxViewLockInfo.Checked;
        model.LockRate = cbxLockRate.Checked;
        model.AccessProfitability = cbxAccessProfitability.Checked;
        model.ViewCompensation = cbxViewCompensation.Checked;
        //Homepage Selections(pick up to six)

        var homePickUpCount = 0;

        model.CompanyCalendar = cbxCompanyCalendar.Checked;
        if (cbxCompanyCalendar.Checked)
            homePickUpCount++;

        model.RateSummary = cbxRatesSummary.Checked;
        if (cbxRatesSummary.Checked)
            homePickUpCount++;

        model.PipelineChart = cbxPipelineChart.Checked;
        if (cbxPipelineChart.Checked)
            homePickUpCount++;

        model.GoalsChart = cbxGoalsChart.Checked;
        if (cbxGoalsChart.Checked)
            homePickUpCount++;

        if (cbxPipelineSummaryWithSales.Enabled)
        {
            model.SalesBreakdownChart = cbxPipelineSummaryWithSales.Checked;
            if (cbxPipelineSummaryWithSales.Checked)
                homePickUpCount++;
        }

        model.OverdueTaskAlerts = cbxOverdueTasks.Checked;
        if (cbxOverdueTasks.Checked)
            homePickUpCount++;

        if (cbxPipelineSummaryWithOrg.Enabled)
        {
            model.OrgProductionChart = cbxPipelineSummaryWithOrg.Checked;
            if (cbxPipelineSummaryWithOrg.Checked)
                homePickUpCount++;
        }

        model.Announcements = cbxCompanyAnn.Checked;
        if (cbxCompanyAnn.Checked)
            homePickUpCount++;

        if (cbxPipelineSummaryWithOrgAndSales.Enabled)
        {
            model.Org_N_Sales_Charts = cbxPipelineSummaryWithOrgAndSales.Checked;
            if (cbxPipelineSummaryWithOrgAndSales.Checked)
                homePickUpCount++;
        }


        model.ExchangeInbox = cbxExchangeInbox.Checked;
        if (cbxExchangeInbox.Checked)
            homePickUpCount++;

        model.ExchangeCalendar = cbxExchangeCalendar.Checked;
        if (cbxExchangeCalendar.Checked)
            homePickUpCount++;

        //Set Production Goals
        model.SetUserGoals = rblSetProductionGoals.SelectedValue == "0" ? true : false;
        model.SetOwnGoals = rblSetProductionGoals.SelectedValue == "1" ? true : false;

        model.Reports = cbxAccessReports.Checked;
        // MailChimp 
        model.AccessAllMailChimpList = cbxAccessAllMailChimpList.Checked;

        //CR47
        model.AccessUnassignedLeads = cbxUnassignedLeads.Checked;


        //Marketing
        string strMarketing = "";
        //if (this.chkAddMarketing.Checked)
        //{
        //    strMarketing += "1";
        //}
        //if (this.chkRemoveMarketing.Checked)
        //{
        //    strMarketing += "2";
        //}
        //if (this.chkVewMarketing.Checked)
        //{
        //    strMarketing += "3";
        //}
        if (strMarketing.Length > 0)
            model.Marketing = int.Parse(strMarketing);

        if (homePickUpCount > 6)
        {
            PageCommon.AlertMsg(this, "Only up to six homepage webparts are allowed.");
            return;
        }

        try
        {
            _bllRoles.Update(model);
        }
        catch (Exception ex)
        {
            PageCommon.AlertMsg(this, "Failed to save role info.");
            LPLog.LogMessage(LogType.Logerror, "Failed to save role info, reason: " + ex.Message);
            return;
        }

        PageCommon.WriteJsEnd(this, "Role save successfully.", "try{window.location.href = 'RoleSetup.aspx?RoleId=" + ddlRoleName.SelectedValue + "';}catch(e){}");
    }

    /// <summary>
    /// 取消更改事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    #endregion Events
}