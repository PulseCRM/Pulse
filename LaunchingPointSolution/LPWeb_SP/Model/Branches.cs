using System;
namespace LPWeb.Model
{/// <summary>
    /// 实体类Branches 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Branches
    {
        public Branches()
        { }
        #region Model
        private int _branchid;
        private string _name;
        private string _desc;
        private bool _enabled;
        private int? _regionid;
        private int? _divisionid;
        private int? _groupid;
        private string _branchaddress;
        private string _city;
        private string _branchstate;
        private string _zip;
        private byte[] _websitelogo;
        private string _globalid;
        private string _license1;
        private string _license2;
        private string _license3;
        private string _license4;
        private string _license5;
        private string _disclaimer;
        private string _phone;
        private string _fax;
        private string _email;
        private string _weburl;
        private bool _homebranch;
        /// <summary>
        /// 
        /// </summary>
        public int BranchId
        {
            set { _branchid = value; }
            get { return _branchid; }
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
        public string Desc
        {
            set { _desc = value; }
            get { return _desc; }
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
        public int? GroupID
        {
            set { _groupid = value; }
            get { return _groupid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BranchAddress
        {
            set { _branchaddress = value; }
            get { return _branchaddress; }
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
        public string BranchState
        {
            set { _branchstate = value; }
            get { return _branchstate; }
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
        public byte[] WebsiteLogo
        {
            set { _websitelogo = value; }
            get { return _websitelogo; }
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
        public string License1
        {
            set { _license1 = value; }
            get { return _license1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string License2
        {
            set { _license2 = value; }
            get { return _license2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string License3
        {
            set { _license3 = value; }
            get { return _license3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string License4
        {
            set { _license4 = value; }
            get { return _license4; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string License5
        {
            set { _license5 = value; }
            get { return _license5; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Disclaimer
        {
            set { _disclaimer = value; }
            get { return _disclaimer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string WebURL
        {
            get { return _weburl; }
            set { _weburl = value; }
        }

        public bool HomeBranch
        {
            get { return _homebranch; }
            set { _homebranch = value; }
        }
        #endregion Model

        private string _leadstar_username;
        private string _leadstar_id;
        private string _leadstar_userid;

        /// <summary>
        /// 
        /// </summary>
        public string Leadstar_Username
        {
            set { _leadstar_username = value; }
            get { return _leadstar_username; }
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
        public string Leadstar_Userid
        {
            set { _leadstar_userid = value; }
            get { return _leadstar_userid; }
        }

        private bool _enablemailchimp;
        private string _mailchimpapikey;

        /// <summary>
        /// 
        /// </summary>
        public bool EnableMailChimp
        {
            set { _enablemailchimp = value; }
            get { return _enablemailchimp; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MailChimpAPIKey
        {
            set { _mailchimpapikey = value; }
            get { return _mailchimpapikey; }
        }
    }
}

