using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类LoanTasks 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class LoanTasks
	{
		public LoanTasks()
		{}
		#region Model
		private int _loantaskid; //0
		private int _fileid; //1
        private string _name; //2
        private DateTime? _due; //3
        private DateTime? _completed; //4
        private int? _completedby; //5
        private DateTime? _lastmodified; //6
        private DateTime? _created; //7
        private int _owner; //8
        private int _loanstageid; //9
        private int _prerequisitetaskid; //10
        private short _daysduefromestclose; //11
        private int? _templtaskid; //12
        private int? _wfltemplid; //13
        private short _daysdueafterprerequisite; //14
        private int _warningemailid; //15
        private int _overdueemailid; //16
        private int _completionemailid; //17
        private short _sequencenumber; //18
        private short _daysfromcreation; //19
        private bool _externalviewing; //20
        private short _daysDueAfterPrevStage; //21
        private TimeSpan? _duetime; //22
        private int _oldloanstageid;   //23
        private int _modifiedby;   //24   
        private string _desc;   //25

		/// <summary>
		/// 
		/// </summary>
		public int LoanTaskId //0
		{
			set{ _loantaskid=value;}
			get{return _loantaskid;}
		}

		/// <summary>
		/// 
		/// </summary>
		public int FileId //1
		{
			set{ _fileid=value;}
			get{return _fileid;}
		}

        /// <summary>
        /// 
        /// </summary>
        public string Name //2
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Due //3
        {
            set { _due = value; }
            get { return _due; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Completed //4
        {
            set { _completed = value; }
            get { return _completed; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? CompletedBy //5
        {
            set { _completedby = value; }
            get { return _completedby; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastModified //6
        {
            set { _lastmodified = value; }
            get { return _lastmodified; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Created //7
        {
            set { _created = value; }
            get { return _created; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Owner //8
        {
            set { _owner = value; }
            get { return _owner; }
        }

        public int LoanStageId //9
        {
            set { _loanstageid = value; }
            get { return _loanstageid; }
        }

        public int PrerequisiteTaskId //10
        {
            set { _prerequisitetaskid = value; }
            get { return _prerequisitetaskid; }
        }		

        public short DaysDueFromEstClose //11
        {
            set { _daysduefromestclose = value; }
            get { return _daysduefromestclose; }
        }

        public int? TemplTaskId //12
        {
            set { _templtaskid = value; }
            get { return _templtaskid; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? WflTemplId //13
        {
            set { _wfltemplid = value; }
            get { return _wfltemplid; }
        }

        public short DaysDueAfterPrerequisite //14
        {
            set { _daysdueafterprerequisite = value; }
            get { return _daysdueafterprerequisite; }
        }

        public int WarningEmailId //15
        {
            set { _warningemailid = value; }
            get { return _warningemailid; }
        }

        public int OverdueEmailId //16
        {
            set { _overdueemailid = value; }
            get { return _overdueemailid; }
        }

        public int CompletionEmailId //17
        {
            set { _completionemailid = value; }
            get { return _completionemailid; }
        }

        public short SequenceNumber //18
        {
            set { _sequencenumber = value; }
            get { return _sequencenumber; }
        }

        public short DaysFromCreation //19
        {
            set { _daysfromcreation = value; }
            get { return _daysfromcreation; }
        }

        public bool ExternalViewing //20
        {
            get { return _externalviewing; }
            set { _externalviewing = value; }
        }

        public short DaysDueAfterPrevStage //21
        {
            set { _daysDueAfterPrevStage = value; }
            get { return _daysDueAfterPrevStage; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan? DueTime //22
        {
            set { _duetime = value; }
            get { return _duetime; }
        }

        public int OldLoanStageId
        {
            set { _oldloanstageid = value; }
            get { return _oldloanstageid; }
        }

        public int ModifiedBy
        {
            set { _modifiedby = value; }
            get { return _modifiedby; }
        }

        public string Desc
        {
            set { _desc = value; }
            get { return _desc; }
        }
		#endregion Model

	}
}

