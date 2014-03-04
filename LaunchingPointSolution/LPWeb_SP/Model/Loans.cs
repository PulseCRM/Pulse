using System;
namespace LPWeb.Model
{
    /// <summary>
    /// 实体类Loans 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Loans
    {
        public Loans()
        { }
        #region Model
        private int _fileid;
        private decimal? _appraisedvalue;
        private string _ccscenario;
        private decimal? _cltv;
        private string _county;
        private DateTime? _dateopen;
        private DateTime? _datesubmit;
        private DateTime? _dateapprove;
        private DateTime? _datecleartoclose;
        private DateTime? _datedocs;
        private DateTime? _datefund;
        private DateTime? _daterecord;
        private DateTime? _dateclose;
        private DateTime? _datedenied;
        private DateTime? _datecanceled;
        private decimal? _downpay;
        private DateTime? _estclosedate;
        private int? _lender;
        private string _lienposition;
        private decimal? _loanamount;
        private string _loannumber;
        private string _loantype;
        private decimal? _ltv;
        private decimal? _monthlypayment;
        private string _lendernotes;
        private string _occupancy;
        private string _program;
        private string _propertyaddr;
        private string _propertycity;
        private string _propertystate;
        private string _propertyzip;
        private string _purpose;
        private decimal? _rate;
        private DateTime? _ratelockexpiration;
        private decimal? _salesprice;
        private int? _term;
        private int? _due;
        private DateTime? _datesuspended;
        private int? _regionid;
        private int? _divisionid;
        private int? _branchid;
        private int? _groupid;
        private int? _userid;
        private string _status;
        private string _lastcompletedstage;
        private string _currentstage;
        private string _prospectloanstatus;
        private DateTime? _disposed;
        private string _ranking;
        private DateTime? _created;
        private int? _createdby;
        private DateTime? _modifed;
        private int? _modifiedby;
        private string _globalid;
        private DateTime? _datehmda;
        private DateTime? _dateprocessing;
        private DateTime? _dateresubmit;
        private DateTime? _datedocsout;
        private DateTime? _datedocsreceived;
        private string _los_loanofficer;
        private DateTime? _datenote;
        private string _leadstar_username;
        private string _leadstar_userid;
        private bool _joint;
        private string _cobrwtype;
        private string _propertytype;
        private string _housingstatus;
        private decimal? _rentamount;
        private bool _interestonly;
        private bool _includeescrows;
        private string _dt_fileid;
        private bool _td_2;
        private decimal? _td_2amount;
        private bool _subordinate;
        private decimal? _monthlypmi;
        private decimal? _monthlypmitax;
        private DateTime? _purchaseddate;
        private string _MIOption;
        private bool _firstTimeHomeBuyer;
        private bool _loanchanged;
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
        public decimal? AppraisedValue
        {
            set { _appraisedvalue = value; }
            get { return _appraisedvalue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CCScenario
        {
            set { _ccscenario = value; }
            get { return _ccscenario; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? CLTV
        {
            set { _cltv = value; }
            get { return _cltv; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string County
        {
            set { _county = value; }
            get { return _county; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateOpen
        {
            set { _dateopen = value; }
            get { return _dateopen; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateSubmit
        {
            set { _datesubmit = value; }
            get { return _datesubmit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateApprove
        {
            set { _dateapprove = value; }
            get { return _dateapprove; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateClearToClose
        {
            set { _datecleartoclose = value; }
            get { return _datecleartoclose; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateDocs
        {
            set { _datedocs = value; }
            get { return _datedocs; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateFund
        {
            set { _datefund = value; }
            get { return _datefund; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateRecord
        {
            set { _daterecord = value; }
            get { return _daterecord; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateClose
        {
            set { _dateclose = value; }
            get { return _dateclose; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateDenied
        {
            set { _datedenied = value; }
            get { return _datedenied; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateCanceled
        {
            set { _datecanceled = value; }
            get { return _datecanceled; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? DownPay
        {
            set { _downpay = value; }
            get { return _downpay; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EstCloseDate
        {
            set { _estclosedate = value; }
            get { return _estclosedate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Lender
        {
            set { _lender = value; }
            get { return _lender; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LienPosition
        {
            set { _lienposition = value; }
            get { return _lienposition; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? LoanAmount
        {
            set { _loanamount = value; }
            get { return _loanamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoanNumber
        {
            set { _loannumber = value; }
            get { return _loannumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoanType
        {
            set { _loantype = value; }
            get { return _loantype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? LTV
        {
            set { _ltv = value; }
            get { return _ltv; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? MonthlyPayment
        {
            set { _monthlypayment = value; }
            get { return _monthlypayment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LenderNotes
        {
            set { _lendernotes = value; }
            get { return _lendernotes; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Occupancy
        {
            set { _occupancy = value; }
            get { return _occupancy; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Program
        {
            set { _program = value; }
            get { return _program; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PropertyAddr
        {
            set { _propertyaddr = value; }
            get { return _propertyaddr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PropertyCity
        {
            set { _propertycity = value; }
            get { return _propertycity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PropertyState
        {
            set { _propertystate = value; }
            get { return _propertystate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PropertyZip
        {
            set { _propertyzip = value; }
            get { return _propertyzip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Purpose
        {
            set { _purpose = value; }
            get { return _purpose; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Rate
        {
            set { _rate = value; }
            get { return _rate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? RateLockExpiration
        {
            set { _ratelockexpiration = value; }
            get { return _ratelockexpiration; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? SalesPrice
        {
            set { _salesprice = value; }
            get { return _salesprice; }
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
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateSuspended
        {
            set { _datesuspended = value; }
            get { return _datesuspended; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? RegionID
        {
            set { _regionid = value; }
            get { return _regionid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DivisionID
        {
            set { _divisionid = value; }
            get { return _divisionid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BranchID
        {
            set { _branchid = value; }
            get { return _branchid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? GroupID
        {
            set { _groupid = value; }
            get { return _groupid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// Prospect,Processing,Closed,Canceled,Denied,Suspended
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// Opened, Submitted, Approved, Cleared to Close, Doc'ed, Funded, Recorded, Closed
        /// </summary>
        public string LastCompletedStage
        {
            set { _lastcompletedstage = value; }
            get { return _lastcompletedstage; }
        }
        /// <summary>
        /// Open, Submit, Approve, Clear to Close, Docs, Fund, Record, Close
        /// </summary>
        public string CurrentStage
        {
            set { _currentstage = value; }
            get { return _currentstage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProspectLoanStatus
        {
            set { _prospectloanstatus = value; }
            get { return _prospectloanstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Disposed
        {
            set { _disposed = value; }
            get { return _disposed; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Ranking
        {
            set { _ranking = value; }
            get { return _ranking; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Created
        {
            set { _created = value; }
            get { return _created; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CreatedBy
        {
            set { _createdby = value; }
            get { return _createdby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modifed
        {
            set { _modifed = value; }
            get { return _modifed; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ModifiedBy
        {
            set { _modifiedby = value; }
            get { return _modifiedby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GlobalId
        {
            set { _globalid = value; }
            get { return _globalid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateHMDA
        {
            set { _datehmda = value; }
            get { return _datehmda; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateProcessing
        {
            set { _dateprocessing = value; }
            get { return _dateprocessing; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateReSubmit
        {
            set { _dateresubmit = value; }
            get { return _dateresubmit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateDocsOut
        {
            set { _datedocsout = value; }
            get { return _datedocsout; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateDocsReceived
        {
            set { _datedocsreceived = value; }
            get { return _datedocsreceived; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LOS_LoanOfficer
        {
            set { _los_loanofficer = value; }
            get { return _los_loanofficer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateNote
        {
            set { _datenote = value; }
            get { return _datenote; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LeadStar_username
        {
            set { _leadstar_username = value; }
            get { return _leadstar_username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LeadStar_userid
        {
            set { _leadstar_userid = value; }
            get { return _leadstar_userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Joint
        {
            set { _joint = value; }
            get { return _joint; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CoBrwType
        {
            set { _cobrwtype = value; }
            get { return _cobrwtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PropertyType
        {
            set { _propertytype = value; }
            get { return _propertytype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HousingStatus
        {
            set { _housingstatus = value; }
            get { return _housingstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? RentAmount
        {
            set { _rentamount = value; }
            get { return _rentamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool InterestOnly
        {
            set { _interestonly = value; }
            get { return _interestonly; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IncludeEscrows
        {
            set { _includeescrows = value; }
            get { return _includeescrows; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DT_FileID
        {
            set { _dt_fileid = value; }
            get { return _dt_fileid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool TD_2
        {
            set { _td_2 = value; }
            get { return _td_2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? TD_2Amount
        {
            set { _td_2amount = value; }
            get { return _td_2amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Subordinate
        {
            set { _subordinate = value; }
            get { return _subordinate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? MonthlyPMI
        {
            set { _monthlypmi = value; }
            get { return _monthlypmi; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? MonthlyPMITax
        {
            set { _monthlypmitax = value; }
            get { return _monthlypmitax; }
        }
        
        public DateTime? PurchasedDate
        {
            set { _purchaseddate = value; }
            get { return _purchaseddate; }
        }
        public string MIOption
        {
            set { _MIOption = value; }
            get { return _MIOption; }
        }

        public bool FirstTimeHomeBuyer
        {
            set { _firstTimeHomeBuyer = value; }
            get { return _firstTimeHomeBuyer; }
        }

        public bool LoanChanged
        {
            set { _loanchanged = value; }
            get { return _loanchanged; }
        }

        #endregion Model

    }
}

