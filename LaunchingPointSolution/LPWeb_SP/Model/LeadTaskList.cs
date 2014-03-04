using System;
namespace LPWeb.Model
{
    /// <summary>
    /// 实体类LeadTaskList 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class LeadTaskList
    {
        public LeadTaskList()
        { }
        #region Model
        private string _taskname;
        private int? _sequencenumber;
        private bool _enabled;
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
        public int? SequenceNumber
        {
            set { _sequencenumber = value; }
            get { return _sequencenumber; }
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

