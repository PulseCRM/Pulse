using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类EmailLog 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class EmailLog
	{
		public EmailLog()
		{}
		#region Model
		private int _emaillogid;
		private string _touser;
		private string _tocontact;
		private int? _emailtmplid;
		private bool _success;
		private string _error;
		private DateTime? _lastsent;
		private int? _loanalertid;
		private int? _retries;
		private int? _fileid;
		private string _fromemail;
		private int? _fromuser;
		private DateTime? _created;
		private int? _alertemailtype;
		private byte[] _emailbody;
		/// <summary>
		/// 
		/// </summary>
		public int EmailLogId
		{
			set{ _emaillogid=value;}
			get{return _emaillogid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ToUser
		{
			set{ _touser=value;}
			get{return _touser;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ToContact
		{
			set{ _tocontact=value;}
			get{return _tocontact;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? EmailTmplId
		{
			set{ _emailtmplid=value;}
			get{return _emailtmplid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Success
		{
			set{ _success=value;}
			get{return _success;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Error
		{
			set{ _error=value;}
			get{return _error;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastSent
		{
			set{ _lastsent=value;}
			get{return _lastsent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LoanAlertId
		{
			set{ _loanalertid=value;}
			get{return _loanalertid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Retries
		{
			set{ _retries=value;}
			get{return _retries;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? FileId
		{
			set{ _fileid=value;}
			get{return _fileid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FromEmail
		{
			set{ _fromemail=value;}
			get{return _fromemail;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? FromUser
		{
			set{ _fromuser=value;}
			get{return _fromuser;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Created
		{
			set{ _created=value;}
			get{return _created;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? AlertEmailType
		{
			set{ _alertemailtype=value;}
			get{return _alertemailtype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public byte[] EmailBody
		{
			set{ _emailbody=value;}
			get{return _emailbody;}
		}
		#endregion Model

	}
}

