using System;
namespace LPWeb.Model
{
    /// <summary>
    /// LoanProfit:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class LoanProfit
    {
        public LoanProfit()
        { }
        #region Model
        private int _fileid;
        private string _compensationplan;
        private decimal _netsell;
        private decimal _srp;
        private decimal _llpa;
        //private string _investor;
        private decimal? _lendercredit;
        private decimal? _price;
        private decimal? _hedgecost;
        private decimal? _mandatoryfinalprice;
        private string _commitmentnumber;
        /// <summary>
        /// 
        /// </summary>
        public int FileId
        {
            set { _fileid = value; }
            get { return _fileid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CompensationPlan
        {
            set { _compensationplan = value; }
            get { return _compensationplan; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal NetSell
        {
            set { _netsell = value; }
            get { return _netsell; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal SRP
        {
            set { _srp = value; }
            get { return _srp; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal LLPA
        {
            set { _llpa = value; }
            get { return _llpa; }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //public string Investor
        //{
        //    set { _investor = value; }
        //    get { return _investor; }
        //}
        public decimal? LenderCredit
        {
            set { _lendercredit = value; }
            get { return _lendercredit; }
        }
        public decimal? Price
        {
            set { _price = value; }
            get { return _price; }
        }

        public decimal? HedgeCost
        {
            set { _hedgecost = value; }
            get { return _hedgecost; }
        }

        public decimal? MandatoryFinalPrice
        {
            set { _mandatoryfinalprice = value; }
            get { return _mandatoryfinalprice; }
        }

        public string CommitmentNumber
        {
            set { _commitmentnumber = value; }
            get { return _commitmentnumber; }
        }
        #endregion Model

    }
}

