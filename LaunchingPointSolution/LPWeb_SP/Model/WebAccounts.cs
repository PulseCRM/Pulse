using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类WebAccounts 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class WebAccounts
	{
		public WebAccounts()
		{}
		#region Model
		private int _webaccountid;
		private bool _enabled;
		private DateTime? _lastlogin;
		private string _password;
		private string _passwordquestion;
		private string _passwordanswer;
		/// <summary>
		/// 
		/// </summary>
		public int WebAccountId
		{
			set{ _webaccountid=value;}
			get{return _webaccountid;}
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
		public DateTime? LastLogin
		{
			set{ _lastlogin=value;}
			get{return _lastlogin;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Password
		{
			set{ _password=value;}
			get{return _password;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PasswordQuestion
		{
			set{ _passwordquestion=value;}
			get{return _passwordquestion;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PasswordAnswer
		{
			set{ _passwordanswer=value;}
			get{return _passwordanswer;}
		}
		#endregion Model

	}
}

