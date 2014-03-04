using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类LoanRules 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class LoanRules
	{
		public LoanRules()
		{}
		#region Model
		private int _loanruleid;
		private int _fileid;
		private int? _rulegroupid;
		private int? _ruleid;
		private DateTime? _applied;
		private int? _appliedby;
		/// <summary>
		/// 
		/// </summary>
		public int LoanRuleId
		{
			set{ _loanruleid=value;}
			get{return _loanruleid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Fileid
		{
			set{ _fileid=value;}
			get{return _fileid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RuleGroupId
		{
			set{ _rulegroupid=value;}
			get{return _rulegroupid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RuleId
		{
			set{ _ruleid=value;}
			get{return _ruleid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Applied
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
		#endregion Model

	}
}

