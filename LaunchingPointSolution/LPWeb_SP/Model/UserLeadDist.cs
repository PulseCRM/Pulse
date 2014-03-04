using System;
namespace LPWeb.Model
{
	/// <summary>
	/// UserLeadDist:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class UserLeadDist
	{
		public UserLeadDist()
		{}
		#region Model
		private int _userid;
		private bool _enableleadrouting;
		private int? _maxdailyleads;
		private int? _leadsassignedtoday;
		private int? _lastleadassigned;
		private DateTime? _lastassigned;
		/// <summary>
		/// 
		/// </summary>
		public int UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool EnableLeadRouting
		{
			set{ _enableleadrouting=value;}
			get{return _enableleadrouting;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? MaxDailyLeads
		{
			set{ _maxdailyleads=value;}
			get{return _maxdailyleads;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LeadsAssignedToday
		{
			set{ _leadsassignedtoday=value;}
			get{return _leadsassignedtoday;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LastLeadAssigned
		{
			set{ _lastleadassigned=value;}
			get{return _lastleadassigned;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastAssigned
		{
			set{ _lastassigned=value;}
			get{return _lastassigned;}
		}
		#endregion Model

	}
}

