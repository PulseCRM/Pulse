using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ProspectIncome:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ProspectIncome
	{
		public ProspectIncome()
		{}
		#region Model
		private int _prospectincomeid;
		private int? _contactid;
		private decimal? _salary;
		private decimal? _overtime;
		private decimal? _bonuses;
		private decimal? _commission;
		private decimal? _div_int;
		private decimal? _netrent;
		private decimal? _other;
		private int? _emplid;
		/// <summary>
		/// 
		/// </summary>
		public int ProspectIncomeId
		{
			set{ _prospectincomeid=value;}
			get{return _prospectincomeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ContactId
		{
			set{ _contactid=value;}
			get{return _contactid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Salary
		{
			set{ _salary=value;}
			get{return _salary;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Overtime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Bonuses
		{
			set{ _bonuses=value;}
			get{return _bonuses;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Commission
		{
			set{ _commission=value;}
			get{return _commission;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Div_Int
		{
			set{ _div_int=value;}
			get{return _div_int;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? NetRent
		{
			set{ _netrent=value;}
			get{return _netrent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Other
		{
			set{ _other=value;}
			get{return _other;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? EmplId
		{
			set{ _emplid=value;}
			get{return _emplid;}
		}
		#endregion Model

	}
}

