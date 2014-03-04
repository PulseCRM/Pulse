using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Company_Web 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Company_Web
	{
		public Company_Web()
		{}
		#region Model
		private bool _emailalertsenabled;
		private string _emailrelayserver;
		private string _defaultalertemail;
		private int? _emailinterval;
		private string _lpcompanyurl;
		private string _borrowerurl;
		private string _borrowergreeting;
		private string _homepagelogo;
		private string _logoforsubpages;
		private byte[] _homepagelogodata;
		private byte[] _subpagelogodata;
        private string _BackgroundLoanAlertPage;
        private bool _EnableEmailAuditTrail;
        private string _BackgroundWCFURL;
        private bool _SendEmailViaEWS;
        private string _EwsUrl;

        private int _SMTP_Port;
        private bool _AuthReq;
        private string _AuthEmailAccount;
        private string _AuthPassword;
        private string _SMTP_EncryptMethod;
        private string _EWS_Version;
        private string _EWS_Domain;

        

		/// <summary>
		/// 
		/// </summary>
		public bool EmailAlertsEnabled
		{
			set{ _emailalertsenabled=value;}
			get{return _emailalertsenabled;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EmailRelayServer
		{
			set{ _emailrelayserver=value;}
			get{return _emailrelayserver;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DefaultAlertEmail
		{
			set{ _defaultalertemail=value;}
			get{return _defaultalertemail;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? EmailInterval
		{
			set{ _emailinterval=value;}
			get{return _emailinterval;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LPCompanyURL
		{
			set{ _lpcompanyurl=value;}
			get{return _lpcompanyurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BorrowerURL
		{
			set{ _borrowerurl=value;}
			get{return _borrowerurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BorrowerGreeting
		{
			set{ _borrowergreeting=value;}
			get{return _borrowergreeting;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HomePageLogo
		{
			set{ _homepagelogo=value;}
			get{return _homepagelogo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LogoForSubPages
		{
			set{ _logoforsubpages=value;}
			get{return _logoforsubpages;}
		}
		/// <summary>
		/// 
		/// </summary>
		public byte[] HomePageLogoData
		{
			set{ _homepagelogodata=value;}
			get{return _homepagelogodata;}
		}
		/// <summary>
		/// 
		/// </summary>
		public byte[] SubPageLogoData
		{
			set{ _subpagelogodata=value;}
			get{return _subpagelogodata;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string BackgroundLoanAlertPage
        {
            set { _BackgroundLoanAlertPage = value; }
            get { return _BackgroundLoanAlertPage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool EnableEmailAuditTrail
        {
            set { _EnableEmailAuditTrail = value; }
            get { return _EnableEmailAuditTrail; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BackgroundWCFURL
        {
            set { _BackgroundWCFURL = value; }
            get { return _BackgroundWCFURL; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool SendEmailViaEWS
        {
            set { _SendEmailViaEWS = value; }
            get { return _SendEmailViaEWS; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EwsUrl
        {
            set { _EwsUrl = value; }
            get { return _EwsUrl; }
        }

        public int SMTP_Port
        {
            get { return _SMTP_Port; }
            set { _SMTP_Port = value; }
        }

        public bool AuthReq
        {
            get { return _AuthReq; }
            set { _AuthReq = value; }
        }
        public string AuthEmailAccount
        {
            get { return _AuthEmailAccount; }
            set { _AuthEmailAccount = value; }
        }
        public string AuthPassword
        {
            get { return _AuthPassword; }
            set { _AuthPassword = value; }
        }
        public string SMTP_EncryptMethod
        {
            get { return _SMTP_EncryptMethod; }
            set { _SMTP_EncryptMethod = value; }
        }
        public string EWS_Version
        {
            get { return _EWS_Version; }
            set { _EWS_Version = value; }
        }
        public string EWS_Domain
        {
            get { return _EWS_Domain; }
            set { _EWS_Domain = value; }
        }




		#endregion Model

	}
}

