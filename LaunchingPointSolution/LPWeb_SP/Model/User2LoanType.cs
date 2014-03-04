using System;
namespace LPWeb.Model
{
	/// <summary>
	/// User2LoanType:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class User2LoanType
	{
		public User2LoanType()
		{}
		#region Model
		private int _userid;
		private string _loantype;
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
		public string LoanType
		{
			set{ _loantype=value;}
			get{return _loantype;}
		}
		#endregion Model

	}
}

