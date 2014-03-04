using System;
namespace LPWeb.Model
{
    /// <summary>
    /// 实体类Users 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Users
    {
        public Users()
        { }
        #region Model
        private int _userid;
        private bool _userenabled;
        private string _username;
        private string _emailaddress;
        private string _firstname;
        private string _lastname;
        private int _roleid;
        private string _password;
        private int _loansperpage;
        private byte[] _userpicturefile;
        private string _phone;
        private string _fax;
        private string _cell;
        private string _signature;
        private string _globalid;
        private bool _marketingacctenabled;
        private string _licensenumber;
        private string _leadstar_id;
        private string _nmls;
        private bool _showtasksinlsr;
        private bool _remindtaskdue;
        private int? _taskreminder;
        private string _sorttaskpicklist;
        private decimal _locomp;
        private decimal _branchmgrcomp;
        private decimal _divisionmgrcomp;
        private decimal _regionmgrcomp;
        private string _exchangepassword;
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
        public bool UserEnabled
        {
            set { _userenabled = value; }
            get { return _userenabled; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Username
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmailAddress
        {
            set { _emailaddress = value; }
            get { return _emailaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FirstName
        {
            set { _firstname = value; }
            get { return _firstname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LastName
        {
            set { _lastname = value; }
            get { return _lastname; }
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
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int LoansPerPage
        {
            set { _loansperpage = value; }
            get { return _loansperpage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] UserPictureFile
        {
            set { _userpicturefile = value; }
            get { return _userpicturefile; }
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
        public string Fax
        {
            set { _fax = value; }
            get { return _fax; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Cell
        {
            set { _cell = value; }
            get { return _cell; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Signature
        {
            set { _signature = value; }
            get { return _signature; }
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
        public bool MarketingAcctEnabled
        {
            set { _marketingacctenabled = value; }
            get { return _marketingacctenabled; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LicenseNumber
        {
            set { _licensenumber = value; }
            get { return _licensenumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Leadstar_ID
        {
            set { _leadstar_id = value; }
            get { return _leadstar_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NMLS
        {
            set { _nmls = value; }
            get { return _nmls; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ShowTasksInLSR
        {
            set { _showtasksinlsr = value; }
            get { return _showtasksinlsr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool RemindTaskDue
        {
            set { _remindtaskdue = value; }
            get { return _remindtaskdue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TaskReminder
        {
            set { _taskreminder = value; }
            get { return _taskreminder; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SortTaskPickList
        {
            set { _sorttaskpicklist = value; }
            get { return _sorttaskpicklist; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal LOComp
        {
            set { _locomp = value; }
            get { return _locomp; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal BranchMgrComp
        {
            set { _branchmgrcomp = value; }
            get { return _branchmgrcomp; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal DivisionMgrComp
        {
            set { _divisionmgrcomp = value; }
            get { return _divisionmgrcomp; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal RegionMgrComp
        {
            set { _regionmgrcomp = value; }
            get { return _regionmgrcomp; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExchangePassword
        {
            set { _exchangepassword = value; }
            get { return _exchangepassword; }
        }
        #endregion Model

    }
}

