using System;
namespace LPWeb.Model
{
	/// <summary>
	/// User2LoanType:ʵ����(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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

