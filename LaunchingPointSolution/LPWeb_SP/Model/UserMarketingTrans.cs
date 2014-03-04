using System;

namespace LPWeb.Model
{
    /// <summary>
    /// 实体类UserMarketingTrans 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class UserMarketingTrans
	{
        public UserMarketingTrans()
		{}
		#region Model
        private int _transid;
        private int _userid;
        private DateTime _transtime;
        private string _action;
        private decimal? _amount;
        private decimal? _balance;
        private int? _loanmarketingid;
        private string _description;
		/// <summary>
		/// 
		/// </summary>
        public int TransId
		{
            set { _transid = value; }
            get { return _transid; }
		}
		/// <summary>
		/// 
		/// </summary>
        public int UserId
		{
            set { _userid = value; }
            get { return _userid; }
		}
		/// <summary>
		/// 
		/// </summary>
        public DateTime TransTime
		{
            set { _transtime = value; }
            get { return _transtime; }
		}
		/// <summary>
		/// 
		/// </summary>
        public string Action
		{
            set { _action = value; }
            get { return _action; }
		}
		/// <summary>
		/// 
		/// </summary>
        public decimal? Amount
		{
            set { _amount = value; }
            get { return _amount; }
		}
		/// <summary>
		/// 
		/// </summary>
        public decimal? Balance
		{
            set { _balance = value; }
            get { return _balance; }
		}
        /// <summary>
        /// 
        /// </summary>
        public int? LoanMarketingId
        {
            set { _loanmarketingid = value; }
            get { return _loanmarketingid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
		#endregion Model

	}
}
