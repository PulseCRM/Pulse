using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Company_General 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Company_General
	{
		public Company_General()
		{}
		#region Model
		private string _name;
		private string _ad_ou_filter;
		private string _desc;
		private string _address;
		private string _city;
		private string _state;
		private string _zip;
		private int? _importuserinterval;
        private string _prefix;
        private string _releaseversion;
        private int _edition;
        private int? _rulemonitorinterval;
        private string _globalid;
        private bool _enablemarketing;
        private string _phone;
        private string _fax;
        private string _weburl;
        private string _integrationid;
        private string _apikey;
        private string _email;
        private string _leadstar_id;
        private string _leadstar_username;
        private string _leadstar_userid;
        private bool _noactiveloanworkflow;
        private bool _startmarketingsync;
        private string _MyEmailInboxURL;
        private string _MyCalendarURL;
        private string _RatesURL;
        
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AD_OU_Filter
		{
			set{ _ad_ou_filter=value;}
			get{return _ad_ou_filter;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Desc
		{
			set{ _desc=value;}
			get{return _desc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string City
		{
			set{ _city=value;}
			get{return _city;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Zip
		{
			set{ _zip=value;}
			get{return _zip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ImportUserInterval
		{
			set{ _importuserinterval=value;}
			get{return _importuserinterval;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Prefix
		{
			set{ _prefix=value;}
			get{return _prefix;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReleaseVersion
		{
			set{ _releaseversion=value;}
			get{return _releaseversion;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Edition
		{
			set{ _edition=value;}
			get{return _edition;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RuleMonitorInterval
		{
			set{ _rulemonitorinterval=value;}
			get{return _rulemonitorinterval;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GlobalId
		{
			set{ _globalid=value;}
			get{return _globalid;}
		}
        /// <summary>
        /// 
        /// </summary>
        public bool EnableMarketing
        {
            set { _enablemarketing = value; }
            get { return _enablemarketing; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WebURL
        {
            get { return _weburl; }
            set { _weburl = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IntegrationID
        {
            get { return _integrationid; }
            set { _integrationid = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string APIKey
        {
            get { return _apikey; }
            set { _apikey = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LeadStar_ID
        {
            get { return _leadstar_id; }
            set { _leadstar_id = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LeadStar_username
        {
            get { return _leadstar_username; }
            set { _leadstar_username = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LeadStar_userid
        {
            get { return _leadstar_userid; }
            set { _leadstar_userid = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ActiveLoanWorkflow 
        {
            get { return _noactiveloanworkflow; }
            set { _noactiveloanworkflow = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool StartMarketingSync
        {
            get { return _startmarketingsync; }
            set { _startmarketingsync = value; }
        }

        public string MyEmailInboxURL
        {
            get { return _MyEmailInboxURL; }
            set { _MyEmailInboxURL = value; }
        }

        public string MyCalendarURL
        {
            get { return _MyCalendarURL; }
            set { _MyCalendarURL = value; }
        }

        public string RatesURL
        {
            get { return _RatesURL; }
            set { _RatesURL = value; }
        }

		#endregion Model

	}
}

