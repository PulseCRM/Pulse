using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Groups 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Groups
	{
		public Groups()
		{}
		#region Model
		private int _groupid;
		private string _groupname;
		private string _organizationtype;
		private int? _organizationid;
		private bool _enabled;
		/// <summary>
		/// 
		/// </summary>
		public int GroupId
		{
			set{ _groupid=value;}
			get{return _groupid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GroupName
		{
			set{ _groupname=value;}
			get{return _groupname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrganizationType
		{
			set{ _organizationtype=value;}
			get{return _organizationtype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OrganizationId
		{
			set{ _organizationid=value;}
			get{return _organizationid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Enabled
		{
			set{ _enabled=value;}
			get{return _enabled;}
		}
		#endregion Model

	}
}

