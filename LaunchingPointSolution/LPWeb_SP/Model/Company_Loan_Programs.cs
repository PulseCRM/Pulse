using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Company_Loan_Programs 。(属性说明自动提取数据库字段的描述信息)
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

