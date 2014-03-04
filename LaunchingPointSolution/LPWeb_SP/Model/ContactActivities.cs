using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类ContactActivities 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class ContactActivities
	{
		public ContactActivities()
		{}
		#region Model
		private int _contactactivityid;
		private int _contactid;
		private int? _userid;
		private string _activityname;
		private DateTime _activitytime;
		/// <summary>
		/// 
		/// </summary>
		public int ContactActivityId
		{
			set{ _contactactivityid=value;}
			get{return _contactactivityid;}
		}
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
		public int? UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ActivityName
		{
			set{ _activityname=value;}
			get{return _activityname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ActivityTime
		{
			set{ _activitytime=value;}
			get{return _activitytime;}
		}
		#endregion Model

	}
}

