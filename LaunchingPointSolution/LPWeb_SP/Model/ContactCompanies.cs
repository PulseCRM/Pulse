using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类ContactCompanies 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class ContactCompanies
	{
		public ContactCompanies()
		{}
		#region Model
		private int _contactcompanyid;
		private string _name;
		private string _address;
		private string _city;
		private string _state;
		private string _zip;
		private string _servicetypes;
        private int _servicetypeid;
        private bool _enabled;
		/// <summary>
		/// 
		/// </summary>
		public int ContactCompanyId
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
		public string ServiceTypes
		{
			set{ _servicetypes=value;}
			get{return _servicetypes;}
		}

        public int ServiceTypeId
        {
            set { _servicetypeid = value; }
            get { return _servicetypeid; }
        }


        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

		#endregion Model

	}
}

