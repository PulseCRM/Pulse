using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类LoanAutoEmails 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class LoanAutoEmails
	{
		public LoanAutoEmails()
		{}
		#region Model
		private int _loanautoemailid;
		private int _fileid;
		private int? _tocontactid;
		private int? _touserid;
		private bool _enabled;
		private bool _external;
		private int? _templreportid;
		private DateTime _applied;
		private int? _appliedby;
		private DateTime? _lastrun;
		private int _scheduletype;
		/// <summary>
		/// 
		/// </summary>
		public int LoanAutoEmailid
		{
			set{ _loanautoemailid=value;}
			get{return _loanautoemailid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int FileId
		{
			set{ _fileid=value;}
			get{return _fileid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ToContactId
		{
			set{ _tocontactid=value;}
			get{return _tocontactid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ToUserId
		{
			set{ _touserid=value;}
			get{return _touserid;}
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
		public bool External
		{
			set{ _external=value;}
			get{return _external;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TemplReportId
		{
			set{ _templreportid=value;}
			get{return _templreportid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime Applied
		{
			set{ _applied=value;}
			get{return _applied;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? AppliedBy
		{
			set{ _appliedby=value;}
			get{return _appliedby;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastRun
		{
			set{ _lastrun=value;}
			get{return _lastrun;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ScheduleType
		{
			set{ _scheduletype=value;}
			get{return _scheduletype;}
		}
		#endregion Model

	}
}

