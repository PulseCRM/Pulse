using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类ContactBranches 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class ContactBranches
	{
		public ContactBranches()
		{}
		#region Model
		private int _contactbranchid;
		private int? _contactcompanyid;
		private string _name;
		private bool _enabled;
		private string _address;
		private string _city;
		private string _state;
		private string _zip;
		private string _phone;
		private string _fax;
		private int? _primarycontact;
		private DateTime? _modified;
		private int? _modifiedby;
		/// <summary>
		/// 
		/// </summary>
		public int ContactBranchId
		{
			set{ _contactbranchid=value;}
			get{return _contactbranchid;}
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
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Enabled
		{
			set{ _enabled=value;}
			get{return _enabled;}
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
		public string Phone
		{
			set{ _phone=value;}
			get{return _phone;}
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
		public int? PrimaryContact
		{
			set{ _primarycontact=value;}
			get{return _primarycontact;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Modified
		{
			set{ _modified=value;}
			get{return _modified;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ModifiedBy
		{
			set{ _modifiedby=value;}
			get{return _modifiedby;}
		}
		#endregion Model

	}
}

