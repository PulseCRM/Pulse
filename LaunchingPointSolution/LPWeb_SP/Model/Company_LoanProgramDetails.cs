using System;
namespace LPWeb.Model
{
    /// <summary>
    /// Company_LoanProgramDetails:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Company_LoanProgramDetails
    {
        public Company_LoanProgramDetails()
        { }
        #region Model
        private int _loanprogramid;
        private string _investor;
        private string _program;
        private int _lendercompanyid;
        private string _indextype;
        private decimal? _margin;
        private decimal? _firstadj;
        private decimal? _subadj;
        private decimal? _lifetimecap;
        private int _investorid;
        private bool _enabled;
        private int? _term;
        private int? _due;
        /// <summary>
        /// 
        /// </summary>
        public int LoanProgramID
        {
            set { _loanprogramid = value; }
            get { return _loanprogramid; }
        }

        public string Investor
        {
            set { _investor = value; }
            get { return _investor; }
        }

        public string Program
        {
            set { _program = value; }
            get { return _program; }
        }


        /// <summary>
        /// 
        /// </summary>
        public int LenderCompanyId
        {
            set { _lendercompanyid = value; }
            get { return _lendercompanyid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IndexType
        {
            set { _indextype = value; }
            get { return _indextype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Margin
        {
            set { _margin = value; }
            get { return _margin; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? FirstAdj
        {
            set { _firstadj = value; }
            get { return _firstadj; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? SubAdj
        {
            set { _subadj = value; }
            get { return _subadj; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? LifetimeCap
        {
            set { _lifetimecap = value; }
            get { return _lifetimecap; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int InvestorID
        {
            set { _investorid = value; }
            get { return _investorid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Term
        {
            set { _term = value; }
            get { return _term; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Due
        {
            set { _due = value; }
            get { return _due; }
        }
        #endregion Model

    }
}

