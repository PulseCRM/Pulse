using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Template_Wfl_Tasks 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Template_Wfl_Tasks
	{
		public Template_Wfl_Tasks()
		{}

        #region Model
        private int _templtaskid;
        private int _wflstageid;
        private string _name;
        private bool _enabled;
        private int _type;
        private int? _daysduefromcoe;
        private int? _prerequisitetaskid;
        private int? _daysdueafterprerequisite;
        private int? _ownerroleid;
        private int? _warningemailid;
        private int? _overdueemailid;
        private int? _completionemailid;
        private int? _sequencenumber;
        private string _description;
        private int? _daysfromcreation;
        private bool _externalviewing;

        /// <summary>
        /// 
        /// </summary>
        public int TemplTaskId
        {
            set { _templtaskid = value; }
            get { return _templtaskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int WflStageId
        {
            set { _wflstageid = value; }
            get { return _wflstageid; }
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
        public bool Enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DaysDueFromCoe
        {
            set { _daysduefromcoe = value; }
            get { return _daysduefromcoe; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? PrerequisiteTaskId
        {
            set { _prerequisitetaskid = value; }
            get { return _prerequisitetaskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DaysDueAfterPrerequisite
        {
            set { _daysdueafterprerequisite = value; }
            get { return _daysdueafterprerequisite; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OwnerRoleId
        {
            set { _ownerroleid = value; }
            get { return _ownerroleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? WarningEmailId
        {
            set { _warningemailid = value; }
            get { return _warningemailid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OverdueEmailId
        {
            set { _overdueemailid = value; }
            get { return _overdueemailid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CompletionEmailId
        {
            set { _completionemailid = value; }
            get { return _completionemailid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SequenceNumber
        {
            set { _sequencenumber = value; }
            get { return _sequencenumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DaysFromCreation
        {
            set { _daysfromcreation = value; }
            get { return _daysfromcreation; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ExternalViewing 
        {
            get { return _externalviewing; }
            set { _externalviewing = value; }
        }
        #endregion Model

    }
}

