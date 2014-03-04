using System;
namespace LPWeb.Model
{
    /// <summary>
    /// 实体类LoanDetails  
    /// </summary>
    [Serializable]
    public class LoanDetails
    {
        public LoanDetails()
        { }

        #region Model
        private int _fileid;
        private int? _folderid;
        private string _filename;
        private DateTime? _estclosedate;
        private decimal? _loanamount;
        private string _program;
        private string _propertyaddr;
        private string _propertycity;
        private string _propertystate;
        private string _propertyzip;
        private decimal? _rate;
        private string _status;
        private string _prospectloanstatus;
        private string _ranking;
        private DateTime? _created;
        private int? _createdby;
        private DateTime? _modifed;
        private int? _modifiedby;
        private int _boid;
        private int _coboid;
        private int _loid;
        private int _userid;
        private int _coboroleid;
        private int _roleid;
        private string _name;
        private string _purpose;
        private string _lien;
        private int _boroleid;
        private int _branchid;

        private string _propertytype;
        private string _housingstatus;
        private bool _interestonly;
        private bool _includeescrows;

        private string _coborrowertype;
        private decimal _rentamount;

        
        private decimal? _salesprice;
        private int? _term;
        private bool _subordinate;
        private bool _td_2;
        private decimal? _td_2amount;
        private decimal? _monthlypmi;
        private decimal? _monthlypmitax;
        private string _loantype;

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
        public int? FolderId
        {
            set { _folderid = value; }
            get { return _folderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FileName
        {
            set { _filename = value; }
            get { return _filename; }
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
        public decimal? LoanAmount
        {
            set { _loanamount = value; }
            get { return _loanamount; }
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
        public decimal? Rate
        {
            set { _rate = value; }
            get { return _rate; }
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
        public string ProspectLoanStatus
        {
            set { _prospectloanstatus = value; }
            get { return _prospectloanstatus; }
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
        public int BoID
        {
            set { _boid = value; }
            get { return _boid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CoBoID
        {
            set { _coboid = value; }
            get { return _coboid; }
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
        public int CoBoRoleID
        {
            set { _coboroleid = value; }
            get { return _coboroleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RoleId
        {
            set { _roleid = value; }
            get { return _roleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BoRoleID
        {
            set { _boroleid = value; }
            get { return _boroleid; }
        }

        public string Purpose
        {
            set { _purpose = value; }
            get { return _purpose; }
        }

        public string Lien
        {
            set { _lien = value; }
            get { return _lien; }
        }

        public int LoanOfficerId
        {
            set { _loid = value; }
            get { return _loid; }
        }

        public int BranchId
        {
            set { _branchid = value; }
            get { return _branchid; }
        }


        public string PropertyType
        {
            set { _propertytype = value; }
            get { return _propertytype; }
        }

        public string HousingStatus
        {
            set { _housingstatus = value; }
            get { return _housingstatus; }
        }

        public bool InterestOnly
        {
            set { _interestonly = value; }
            get { return _interestonly; }
        }

        public bool IncludeEscrows
        {
            set { _includeescrows = value; }
            get { return _includeescrows; }

        }

        public string CoborrowerType
        {
            set { _coborrowertype = value; }
            get { return _coborrowertype; }
        }

        public decimal RentAmount
        {
            set { _rentamount = value; }
            get { return _rentamount; }
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
        /// <summary>
        /// 
        /// </summary>
        public string LoanType
        {
            set { _loantype = value; }
            get { return _loantype; }
        }

        #endregion Model

    }
}

