using System;
namespace LPWeb.Model
{
    /// <summary>
    /// 实体类LoanAlerts 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class LoanAlerts
    {
        public LoanAlerts()
        { }
        #region Model
        private int _loanalertid;
        private int _fileid;
        private string _desc;
        private DateTime _duedate;
        private int? _clearedby;
        private DateTime? _cleared;
        private bool _acknowlegereq;
        private string _acknowledgedby;
        private DateTime? _acknowledged;
        private int? _loanruleid;
        private int? _ownerid;
        private int? _loantaskid;
        private string _alerttype;
        private DateTime? _datecreated;
        private string _status;
        private DateTime? _accepted;
        private DateTime? _declined;
        private DateTime? _dismissed;
        private string _acceptedby;
        private string _declinedby;
        private string _dismissedby;
        private byte[] _alertemail;
        private byte[] _recomemail;
        /// <summary>
        /// 
        /// </summary>
        public int LoanAlertId
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
        public string Desc
        {
            set { _desc = value; }
            get { return _desc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime DueDate
        {
            set { _duedate = value; }
            get { return _duedate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ClearedBy
        {
            set { _clearedby = value; }
            get { return _clearedby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Cleared
        {
            set { _cleared = value; }
            get { return _cleared; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool AcknowlegeReq
        {
            set { _acknowlegereq = value; }
            get { return _acknowlegereq; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AcknowledgedBy
        {
            set { _acknowledgedby = value; }
            get { return _acknowledgedby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Acknowledged
        {
            set { _acknowledged = value; }
            get { return _acknowledged; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LoanRuleId
        {
            set { _loanruleid = value; }
            get { return _loanruleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OwnerId
        {
            set { _ownerid = value; }
            get { return _ownerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LoanTaskId
        {
            set { _loantaskid = value; }
            get { return _loantaskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AlertType
        {
            set { _alerttype = value; }
            get { return _alerttype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateCreated
        {
            set { _datecreated = value; }
            get { return _datecreated; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Accepted
        {
            set { _accepted = value; }
            get { return _accepted; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Declined
        {
            set { _declined = value; }
            get { return _declined; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Dismissed
        {
            set { _dismissed = value; }
            get { return _dismissed; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AcceptedBy
        {
            set { _acceptedby = value; }
            get { return _acceptedby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DeclinedBy
        {
            set { _declinedby = value; }
            get { return _declinedby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DismissedBy
        {
            set { _dismissedby = value; }
            get { return _dismissedby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] AlertEmail
        {
            set { _alertemail = value; }
            get { return _alertemail; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] RecomEmail
        {
            set { _recomemail = value; }
            get { return _recomemail; }
        }
        #endregion Model

    }
}

