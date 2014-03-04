using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类EmailQue 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class EmailQue
	{
		public EmailQue()
		{}
        #region Model
        private int _emailid;
        private string _touser;
        private string _tocontact;
        private string _toborrower;
        private int? _emailtmplid;
        private int? _loanalertid;
        private int _fileid;
        private int? _alertemailtype;
        private byte[] _emailbody;
        /// <summary>
        /// 
        /// </summary>
        public int EmailId
        {
            set { _emailid = value; }
            get { return _emailid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ToUser
        {
            set { _touser = value; }
            get { return _touser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ToContact
        {
            set { _tocontact = value; }
            get { return _tocontact; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ToBorrower
        {
            set { _toborrower = value; }
            get { return _toborrower; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? EmailTmplId
        {
            set { _emailtmplid = value; }
            get { return _emailtmplid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LoanAlertId
        {
            set { _loanalertid = value; }
            get { return _loanalertid; }
        }
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
        public int? AlertEmailType
        {
            set { _alertemailtype = value; }
            get { return _alertemailtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] EmailBody
        {
            set { _emailbody = value; }
            get { return _emailbody; }
        }
        #endregion Model

	}
}

