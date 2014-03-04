using System;
namespace LPWeb.Model
{
    /// <summary>
    /// ProspectEmployment:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ProspectEmployment
    {
        public ProspectEmployment()
        { }
        #region Model
        private int _emplid;
        private int _contactid;
        private bool? _selfemployed;
        private string _position;
        private decimal? _startyear;
        private decimal? _startmonth;
        private decimal? _endyear;
        private decimal? _endmonth;
        private decimal? _yearsonwork;
        private string _phone;
        private int? _contactbranchid;
        private string _companyname;
        private string _address;
        private string _city;
        private string _state;
        private string _zip;
        private string _businessType;
        private bool? _verifyYourTaxes;
        /// <summary>
        /// 
        /// </summary>
        public int EmplId
        {
            set { _emplid = value; }
            get { return _emplid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ContactId
        {
            set { _contactid = value; }
            get { return _contactid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? SelfEmployed
        {
            set { _selfemployed = value; }
            get { return _selfemployed; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Position
        {
            set { _position = value; }
            get { return _position; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? StartYear
        {
            set { _startyear = value; }
            get { return _startyear; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? StartMonth
        {
            set { _startmonth = value; }
            get { return _startmonth; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? EndYear
        {
            set { _endyear = value; }
            get { return _endyear; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? EndMonth
        {
            set { _endmonth = value; }
            get { return _endmonth; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? YearsOnWork
        {
            set { _yearsonwork = value; }
            get { return _yearsonwork; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ContactBranchId
        {
            set { _contactbranchid = value; }
            get { return _contactbranchid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CompanyName
        {
            set { _companyname = value; }
            get { return _companyname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string City
        {
            set { _city = value; }
            get { return _city; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Zip
        {
            set { _zip = value; }
            get { return _zip; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BusinessType
        {
            set { _businessType = value; }
            get { return _businessType; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? VerifyYourTaxes
        {
            set { _verifyYourTaxes = value; }
            get { return _verifyYourTaxes; }
        }
        #endregion Model

    }
}

