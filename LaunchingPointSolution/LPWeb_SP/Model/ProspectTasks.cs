using System;
namespace LPWeb.Model
{
    /// <summary>
    /// ProspectTasks:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class ProspectTasks
    {
        public ProspectTasks()
        { }
        #region Model
        private int _prospecttaskid;
        private int _contactid;
        private string _taskname;
        private string _desc;
        private int? _ownerid;
        private DateTime? _due;
        private int? _warningemailtemplid;
        private int? _overdueemailtemplid;
        private int? _completionemailtemplid;
        private DateTime? _completed;
        private int? _completedby;
        private int? _daysFromCreation;
        private bool _enabled;
        /// <summary>
        /// 
        /// </summary>
        public int ProspectTaskId
        {
            set { _prospecttaskid = value; }
            get { return _prospecttaskid; }
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
        public string TaskName
        {
            set { _taskname = value; }
            get { return _taskname; }
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
        public int? OwnerId
        {
            set { _ownerid = value; }
            get { return _ownerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Due
        {
            set { _due = value; }
            get { return _due; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? WarningEmailTemplId
        {
            set { _warningemailtemplid = value; }
            get { return _warningemailtemplid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OverdueEmailTemplId
        {
            set { _overdueemailtemplid = value; }
            get { return _overdueemailtemplid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CompletionEmailTemplid
        {
            set { _completionemailtemplid = value; }
            get { return _completionemailtemplid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Completed
        {
            set { _completed = value; }
            get { return _completed; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CompletedBy
        {
            set { _completedby = value; }
            get { return _completedby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DaysFromCreation
        {
            set { _daysFromCreation = value; }
            get { return _daysFromCreation; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }
        #endregion Model

    }
}

