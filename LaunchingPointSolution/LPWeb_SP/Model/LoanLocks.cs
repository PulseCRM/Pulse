using System;
namespace LPWeb.Model
{
    /// <summary>
    /// LoanLocks:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class LoanLocks
    {
        public LoanLocks()
        { }
        #region Model
        private int _fileid;
        private string _lockoption;
        private string _lockedby;
        private DateTime? _locktime;
        private int? _lockterm;
        private string _confirmedby;
        private DateTime? _confirmtime;
        private DateTime? _lockexpirationdate;
        private int? _ext1term;
        private DateTime? _ext1lockexpdate;
        private DateTime? _ext1locktime;
        private string _ext1lockedby;
        private DateTime? _ext1confirmtime;
        private int? _ext2term;
        private DateTime? _ext2lockexpdate;
        private DateTime? _ext2locktime;
        private string _ext2lockedby;
        private DateTime? _ext2confirmtime;
        private int? _ext3term;
        private DateTime? _ext3lockexpdate;
        private DateTime? _ext3locktime;
        private string _ext3lockedby;
        private DateTime? _ext3confirmtime;
        //CR67
        private int? _investorid;
        private string _investor;
        private int? _programid;
        private string _program;
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
        public string LockOption
        {
            set { _lockoption = value; }
            get { return _lockoption; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LockedBy
        {
            set { _lockedby = value; }
            get { return _lockedby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LockTime
        {
            set { _locktime = value; }
            get { return _locktime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LockTerm
        {
            set { _lockterm = value; }
            get { return _lockterm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ConfirmedBy
        {
            set { _confirmedby = value; }
            get { return _confirmedby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ConfirmTime
        {
            set { _confirmtime = value; }
            get { return _confirmtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LockExpirationDate
        {
            set { _lockexpirationdate = value; }
            get { return _lockexpirationdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Ext1Term
        {
            set { _ext1term = value; }
            get { return _ext1term; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Ext1LockExpDate
        {
            set { _ext1lockexpdate = value; }
            get { return _ext1lockexpdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Ext1LockTime
        {
            set { _ext1locktime = value; }
            get { return _ext1locktime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Ext1LockedBy
        {
            set { _ext1lockedby = value; }
            get { return _ext1lockedby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Ext1ConfirmTime
        {
            set { _ext1confirmtime = value; }
            get { return _ext1confirmtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Ext2Term
        {
            set { _ext2term = value; }
            get { return _ext2term; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Ext2LockExpDate
        {
            set { _ext2lockexpdate = value; }
            get { return _ext2lockexpdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Ext2LockTime
        {
            set { _ext2locktime = value; }
            get { return _ext2locktime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Ext2LockedBy
        {
            set { _ext2lockedby = value; }
            get { return _ext2lockedby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Ext2ConfirmTime
        {
            set { _ext2confirmtime = value; }
            get { return _ext2confirmtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Ext3Term
        {
            set { _ext3term = value; }
            get { return _ext3term; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Ext3LockExpDate
        {
            set { _ext3lockexpdate = value; }
            get { return _ext3lockexpdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Ext3LockTime
        {
            set { _ext3locktime = value; }
            get { return _ext3locktime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Ext3LockedBy
        {
            set { _ext3lockedby = value; }
            get { return _ext3lockedby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Ext3ConfirmTime
        {
            set { _ext3confirmtime = value; }
            get { return _ext3confirmtime; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? InvestorID
        {
            set { _investorid = value; }
            get { return _investorid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Investor
        {
            set { _investor = value; }
            get { return _investor; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProgramID
        {
            set { _programid = value; }
            get { return _programid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Program
        {
            set { _program = value; }
            get { return _program; }
        }
        #endregion Model

    }
}

