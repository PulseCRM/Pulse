using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����Company_Loan_Programs ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Company_Loan_Programs
	{
		public Company_Loan_Programs()
		{}
		#region Model
		private int _loanprogramid;
		private string _loanprogram;
        private bool _IsARM;
		/// <summary>
		/// 
		/// </summary>
		public int LoanProgramID
		{
			set{ _loanprogramid=value;}
			get{return _loanprogramid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LoanProgram
		{
			set{ _loanprogram=value;}
			get{return _loanprogram;}
		}

        /// <summary>
        /// 
        /// </summary>
        public bool IsARM
        {
            set { _IsARM = value; }
            get { return _IsARM; }
        }

		#endregion Model

	}
}

