using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Contacts 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Contacts
	{
		public Contacts()
		{}
        #region Model
		private int _contactid;
		private string _firstname;
		private string _middlename;
		private string _lastname;
		private string _nickname;
		private string _title;
		private string _generationcode;
		private string _ssn;
		private string _homephone;
		private string _cellphone;
		private string _businessphone;
		private string _fax;
		private string _email;
		private DateTime? _dob;
		private int? _experian;
		private int? _transunion;
		private int? _equifax;
		private string _mailingaddr;
		private string _mailingcity;
		private string _mailingstate;
		private string _mailingzip;
		private int? _contactcompanyid;
		private int? _webaccountid;
		private bool _contactenable;
        private bool _updatepoint;
        private int? _createdby;
        private DateTime? _created;
        private int? _contactbranchid;
        private bool _enabled;
        private byte[] _picture;
        private string _signature;
		/// <summary>
		/// 
		/// </summary>
		public int ContactId
		{
			set{ _contactid=value;}
			get{return _contactid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FirstName
		{
			set{ _firstname=value;}
			get{return _firstname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MiddleName
		{
			set{ _middlename=value;}
			get{return _middlename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LastName
		{
			set{ _lastname=value;}
			get{return _lastname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string NickName
		{
			set{ _nickname=value;}
			get{return _nickname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GenerationCode
		{
			set{ _generationcode=value;}
			get{return _generationcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SSN
		{
			set{ _ssn=value;}
			get{return _ssn;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HomePhone
		{
			set{ _homephone=value;}
			get{return _homephone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CellPhone
		{
			set{ _cellphone=value;}
			get{return _cellphone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BusinessPhone
		{
			set{ _businessphone=value;}
			get{return _businessphone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Fax
		{
			set{ _fax=value;}
			get{return _fax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? DOB
		{
			set{ _dob=value;}
			get{return _dob;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Experian
		{
			set{ _experian=value;}
			get{return _experian;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TransUnion
		{
			set{ _transunion=value;}
			get{return _transunion;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Equifax
		{
			set{ _equifax=value;}
			get{return _equifax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MailingAddr
		{
			set{ _mailingaddr=value;}
			get{return _mailingaddr;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MailingCity
		{
			set{ _mailingcity=value;}
			get{return _mailingcity;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MailingState
		{
			set{ _mailingstate=value;}
			get{return _mailingstate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MailingZip
		{
			set{ _mailingzip=value;}
			get{return _mailingzip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ContactCompanyId
		{
			set{ _contactcompanyid=value;}
			get{return _contactcompanyid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? WebAccountId
		{
			set{ _webaccountid=value;}
			get{return _webaccountid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool ContactEnable
		{
			set{ _contactenable=value;}
			get{return _contactenable;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool UpdatePoint
		{
			set{ _updatepoint=value;}
			get{return _updatepoint;}
		}

        /// <summary>
        /// 
        /// </summary>
        public int? CreatedBy
        {
            set { _createdby = value; }
            get { return _createdby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Created
        {
            set { _created = value; }
            get { return _created; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ContactBranchId
        {
            set { _contactbranchid = value; }
            get { return _contactbranchid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Picture
        {
            set { _picture = value; }
            get { return _picture; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Signature
        {
            set { _signature = value; }
            get { return _signature; }
        }
		#endregion Model

	}
}

