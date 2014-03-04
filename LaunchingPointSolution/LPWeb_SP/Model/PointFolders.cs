using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类PointFolders 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class PointFolders
	{
		public PointFolders()
		{}
		#region Model
		private int _folderid;
		private string _name;
		private int? _branchid;
		private string _path;
		private bool _enabled;
		private int? _importcount;
		private DateTime? _lastimport;
		private int _loanstatus;
        private string _branchname;

        private bool _autonaming;
        private string _prefix;

        private bool _randomFileNaming;
        private int? _filenameLength;

		/// <summary>
		/// 
		/// </summary>
		public int FolderId
		{
			set{ _folderid=value;}
			get{return _folderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? BranchId
		{
			set{ _branchid=value;}
			get{return _branchid;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string BranchName
        {
            set { _branchname = value; }
            get { return _branchname; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string Path
		{
			set{ _path=value;}
			get{return _path;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Enabled
		{
			set{ _enabled=value;}
			get{return _enabled;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ImportCount
		{
			set{ _importcount=value;}
			get{return _importcount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastImport
		{
			set{ _lastimport=value;}
			get{return _lastimport;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int LoanStatus
		{
			set{ _loanstatus=value;}
			get{return _loanstatus;}
		}

        /// <summary>
        /// Enable Auto File-naming
        /// </summary>
        public bool AutoNaming
        {
            set { _autonaming = value; }
            get { return _autonaming; }
        }

        /// <summary>
        /// PreFix
        /// </summary>
        public string PreFix
        {
            set { _prefix = value; }
            get { return _prefix; }
        }
        public bool RandomFileNaming
        {
            set { _randomFileNaming = value; }
            get { return _randomFileNaming; }
        }

        public int? FilenameLength
        {
            set { _filenameLength = value; }
            get { return _filenameLength; }
        }

		#endregion Model

	}
}

