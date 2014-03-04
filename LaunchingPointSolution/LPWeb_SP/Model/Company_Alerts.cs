using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Company_Alerts 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Company_Alerts
	{
		public Company_Alerts()
		{}
		#region Model
		private int? _alertyellowdays;
		private int? _alertreddays;
		private int? _taskyellowdays;
		private int? _taskreddays;
		private int? _ratelockyellowdays;
		private int? _ratelockreddays;
        private bool _sendEmailCustomTasks = false;
        private int _customTaskEmailTemplId = 0;

		/// <summary>
		/// 
		/// </summary>
		public int? AlertYellowDays
		{
			set{ _alertyellowdays=value;}
			get{return _alertyellowdays;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? AlertRedDays
		{
			set{ _alertreddays=value;}
			get{return _alertreddays;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TaskYellowDays
		{
			set{ _taskyellowdays=value;}
			get{return _taskyellowdays;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TaskRedDays
		{
			set{ _taskreddays=value;}
			get{return _taskreddays;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RateLockYellowDays
		{
			set{ _ratelockyellowdays=value;}
			get{return _ratelockyellowdays;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RateLockRedDays
		{
			set{ _ratelockreddays=value;}
			get{return _ratelockreddays;}
		}

        /// <summary>
        /// 
        /// </summary>
        public int CustomTaskEmailTemplId
        {
            set { _customTaskEmailTemplId = value; }
            get { return _customTaskEmailTemplId; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SendEmailCustomTasks
        {
            set { _sendEmailCustomTasks = value; }
            get { return _sendEmailCustomTasks; }
        }

		#endregion Model

	}
}

