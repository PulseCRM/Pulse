using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类LoanMarketingEvents 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class LoanMarketingEvents
	{
		public LoanMarketingEvents()
		{}
		#region Model
		private int _loanmarketingeventid;
		private string _action;
		private DateTime? _executiondate;
		private int? _loanmarketingid;
		private string _leadstareventid;
		private bool _completed;
		private int? _weekno;
		private string _eventcontent;
		private string _eventurl;
		private int _fileid;
		/// <summary>
		/// 
		/// </summary>
		public int LoanMarketingEventId
		{
			set{ _loanmarketingeventid=value;}
			get{return _loanmarketingeventid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Action
		{
			set{ _action=value;}
			get{return _action;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? ExecutionDate
		{
			set{ _executiondate=value;}
			get{return _executiondate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LoanMarketingId
		{
			set{ _loanmarketingid=value;}
			get{return _loanmarketingid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LeadStarEventId
		{
			set{ _leadstareventid=value;}
			get{return _leadstareventid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Completed
		{
			set{ _completed=value;}
			get{return _completed;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? WeekNo
		{
			set{ _weekno=value;}
			get{return _weekno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EventContent
		{
			set{ _eventcontent=value;}
			get{return _eventcontent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EventURL
		{
			set{ _eventurl=value;}
			get{return _eventurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int FileId
		{
			set{ _fileid=value;}
			get{return _fileid;}
		}
		#endregion Model

	}
}

