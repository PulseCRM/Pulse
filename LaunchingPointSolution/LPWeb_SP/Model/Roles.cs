using System;
namespace LPWeb.Model
{
    /// <summary>
    /// Roles:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Roles
    {
        public Roles()
        { }
        #region Model
        private int _roleid;
        private string _name;
        private bool _companysetup;
        private bool _loansetup;
        private bool _otherloanaccess;
        private bool _customuserhome;
        private int? _workflowtempl;
        private int? _customtask;
        private int? _alertrules;
        private int? _alertruletempl;
        private bool _markothertaskcompl;
        private bool _assigntask;
        private bool _importloan;
        private bool _removeloan;
        private bool _assignloan;
        private bool _applyworkflow;
        private bool _applyalertrule;
        private bool _sendemail;
        private bool _createnotes;
        private bool _companycalendar;
        private bool _pipelinechart;
        private bool _salesbreakdownchart;
        private bool _orgproductionchart;
        private bool _org_n_sales_charts;
        private bool _ratesummary;
        private bool _goalschart;
        private bool _overduetaskalerts;
        private bool _announcements;
        private bool _exchangeinbox;
        private bool _exchangecalendar;
        private bool _setowngoals;
        private bool _setusergoals;
        private bool _reports;
        private int? _contactmgmt;
        private string _prospect;
        private string _loans;
        private bool _accessallcontacts;
        private int? _contactcompany;
        private int? _contactbranch;
        private int? _servicetype;
        private int? _contactrole;
        private int? _marketing;
        private bool _extendratelock;
        private string _conditionrights;
        private bool _sendlsr;
        private bool _accessUnassignedLeads; //gdc CR47

        //gdc CR45
        private bool _exportpipelines;

        //CR38
        private bool _viewlockinfo;
        private bool _lockrate;
        //CR52
        private bool _accessprofitability;
        private bool _viewcompensation;
        //CR65
        private bool _updatecondition;

        /// <summary>
        /// 
        /// </summary>
        public int RoleId
        {
            set { _roleid = value; }
            get { return _roleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool CompanySetup
        {
            set { _companysetup = value; }
            get { return _companysetup; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool LoanSetup
        {
            set { _loansetup = value; }
            get { return _loansetup; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool OtherLoanAccess
        {
            set { _otherloanaccess = value; }
            get { return _otherloanaccess; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool CustomUserHome
        {
            set { _customuserhome = value; }
            get { return _customuserhome; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? WorkflowTempl
        {
            set { _workflowtempl = value; }
            get { return _workflowtempl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CustomTask
        {
            set { _customtask = value; }
            get { return _customtask; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AlertRules
        {
            set { _alertrules = value; }
            get { return _alertrules; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AlertRuleTempl
        {
            set { _alertruletempl = value; }
            get { return _alertruletempl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool MarkOtherTaskCompl
        {
            set { _markothertaskcompl = value; }
            get { return _markothertaskcompl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool AssignTask
        {
            set { _assigntask = value; }
            get { return _assigntask; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ImportLoan
        {
            set { _importloan = value; }
            get { return _importloan; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool RemoveLoan
        {
            set { _removeloan = value; }
            get { return _removeloan; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool AssignLoan
        {
            set { _assignloan = value; }
            get { return _assignloan; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ApplyWorkflow
        {
            set { _applyworkflow = value; }
            get { return _applyworkflow; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ApplyAlertRule
        {
            set { _applyalertrule = value; }
            get { return _applyalertrule; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool SendEmail
        {
            set { _sendemail = value; }
            get { return _sendemail; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool CreateNotes
        {
            set { _createnotes = value; }
            get { return _createnotes; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool CompanyCalendar
        {
            set { _companycalendar = value; }
            get { return _companycalendar; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool PipelineChart
        {
            set { _pipelinechart = value; }
            get { return _pipelinechart; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool SalesBreakdownChart
        {
            set { _salesbreakdownchart = value; }
            get { return _salesbreakdownchart; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool OrgProductionChart
        {
            set { _orgproductionchart = value; }
            get { return _orgproductionchart; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Org_N_Sales_Charts
        {
            set { _org_n_sales_charts = value; }
            get { return _org_n_sales_charts; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool RateSummary
        {
            set { _ratesummary = value; }
            get { return _ratesummary; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool GoalsChart
        {
            set { _goalschart = value; }
            get { return _goalschart; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool OverdueTaskAlerts
        {
            set { _overduetaskalerts = value; }
            get { return _overduetaskalerts; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Announcements
        {
            set { _announcements = value; }
            get { return _announcements; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ExchangeInbox
        {
            set { _exchangeinbox = value; }
            get { return _exchangeinbox; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ExchangeCalendar
        {
            set { _exchangecalendar = value; }
            get { return _exchangecalendar; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool SetOwnGoals
        {
            set { _setowngoals = value; }
            get { return _setowngoals; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool SetUserGoals
        {
            set { _setusergoals = value; }
            get { return _setusergoals; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Reports
        {
            set { _reports = value; }
            get { return _reports; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ContactMgmt
        {
            set { _contactmgmt = value; }
            get { return _contactmgmt; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Prospect
        {
            set { _prospect = value; }
            get { return _prospect; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Loans
        {
            set { _loans = value; }
            get { return _loans; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool AccessAllContacts
        {
            set { _accessallcontacts = value; }
            get { return _accessallcontacts; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ContactCompany
        {
            set { _contactcompany = value; }
            get { return _contactcompany; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ContactBranch
        {
            set { _contactbranch = value; }
            get { return _contactbranch; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ServiceType
        {
            set { _servicetype = value; }
            get { return _servicetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ContactRole
        {
            set { _contactrole = value; }
            get { return _contactrole; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Marketing
        {
            set { _marketing = value; }
            get { return _marketing; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ExtendRateLock
        {
            set { _extendratelock = value; }
            get { return _extendratelock; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ConditionRights
        {
            set { _conditionrights = value; }
            get { return _conditionrights; }
        }

        public bool SendLSR
        {
            set { _sendlsr = value; }
            get { return _sendlsr; }
        }

        public bool AccessAllMailChimpList { get; set; }

        //gdc CR45

        public bool ExportClients
        {
            get
            {
                return Prospect.ToUpper().Contains("P");
            }
        }

        public bool ExportContacts
        {
            get { return ContactMgmt.ToString().Contains("7"); }
        }

        public bool ExportPipelines
        {
            get { return _exportpipelines; }
            set { _exportpipelines = value; }
        }

        //gdc CR45 end


        //gdc CR47
        public bool AccessUnassignedLeads
        {
            get { return _accessUnassignedLeads; }
            set { _accessUnassignedLeads = value; }
        }

        //CR38 Rocky
        public bool ViewLockInfo
        {
            get { return _viewlockinfo; }
            set { _viewlockinfo = value; }
        }
        public bool LockRate
        {
            get { return _lockrate; }
            set { _lockrate = value; }
        }

        //CR52 Rocky
        public bool AccessProfitability
        {
            get { return _accessprofitability; }
            set { _accessprofitability = value; }
        }
        public bool ViewCompensation
        {
            get { return _viewcompensation; }
            set { _viewcompensation = value; }
        }
        //CR65
        public bool UpdateCondition
        {
            get { return _updatecondition; }
            set { _updatecondition = value; }
        }
        #endregion Model

    }
}

