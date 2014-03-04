using System;
namespace LPWeb.Model
{
    /// <summary>
    /// 实体类UserHomePref 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class UserHomePref
    {
        public UserHomePref()
        { }
        #region Model
        private int _userid;
        private bool _companycalendar;
        private bool _pipelinechart;
        private bool _salesbreakdownchart;
        private bool _orgproductionchart;
        private bool _org_n_sales_charts;
        private bool _ratesummary;
        private bool _goalschart;
        private bool _overduetaskalert;
        private bool _announcements;
        private bool _exchangeinbox;
        private bool _exchangecalendar;
        private int? _alertfilter;
        private int? _defaultclientspipelineviewid;
        private int? _defaultloanspipelineviewid;
        private int? _defaultleadspipelineviewid;
        private int? _dashboardlastcompletedstages;
        private bool _quickleadform;
        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
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
        public bool OverDueTaskAlert
        {
            set { _overduetaskalert = value; }
            get { return _overduetaskalert; }
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
        public int? AlertFilter
        {
            set { _alertfilter = value; }
            get { return _alertfilter; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DefaultClientsPipelineViewId
        {
            set { _defaultclientspipelineviewid = value; }
            get { return _defaultclientspipelineviewid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DefaultLoansPipelineViewId
        {
            set { _defaultloanspipelineviewid = value; }
            get { return _defaultloanspipelineviewid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DefaultLeadsPipelineViewId
        {
            set { _defaultleadspipelineviewid = value; }
            get { return _defaultleadspipelineviewid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DashboardLastCompletedStages
        {
            set { _dashboardlastcompletedstages = value; }
            get { return _dashboardlastcompletedstages; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool QuickLeadForm
        {
            set { _quickleadform = value; }
            get { return _quickleadform; }
        }
        #endregion Model

    }
}

