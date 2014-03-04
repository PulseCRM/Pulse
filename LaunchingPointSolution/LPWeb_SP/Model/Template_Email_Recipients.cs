using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Template_Email_Recipients 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Template_Email_Recipients
	{
		public Template_Email_Recipients()
		{}
		#region Model
		private int _templrecipientid;
		private int _templemailid;
		private string _emailaddr;
		private string _userroles;
		private string _contactroles;
		private string _recipienttype;
		private bool _taskowner;
		/// <summary>
		/// 
		/// </summary>
		public int TemplRecipientId
		{
			set{ _templrecipientid=value;}
			get{return _templrecipientid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int TemplEmailId
		{
			set{ _templemailid=value;}
			get{return _templemailid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EmailAddr
		{
			set{ _emailaddr=value;}
			get{return _emailaddr;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserRoles
		{
			set{ _userroles=value;}
			get{return _userroles;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ContactRoles
		{
			set{ _contactroles=value;}
			get{return _contactroles;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RecipientType
		{
			set{ _recipienttype=value;}
			get{return _recipienttype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool TaskOwner
		{
			set{ _taskowner=value;}
			get{return _taskowner;}
		}
		#endregion Model

	}
}

