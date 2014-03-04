using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ProspectOtherIncome:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ProspectOtherIncome
	{
		public ProspectOtherIncome()
		{}
		#region Model
		private int _prospectotherincomeid;
		private int _contactid;
		private string _type;
		private decimal? _monthlyincome;
		/// <summary>
		/// 
		/// </summary>
		public int ProspectOtherIncomeId
		{
			set{ _prospectotherincomeid=value;}
			get{return _prospectotherincomeid;}
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
		public string Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? MonthlyIncome
		{
			set{ _monthlyincome=value;}
			get{return _monthlyincome;}
		}
		#endregion Model

	}
}

