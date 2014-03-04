using System;
namespace LPWeb.Model
{
    /// <summary>
    /// 实体类Template_Stages 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Template_Stages
    {
        public Template_Stages()
        { }
        #region Model
        private int _templstageid;
        private string _name;
        private bool _enabled;
        private int? _sequencenumber;
        private string _workflowtype;
        private bool _custom;
        private int? _pointstagenamefield;
        private int? _pointstagedatefield;
        private string _alias;
        private int? _daysfromestclose;
        private int? _daysfromcreation;
        /// <summary>
        /// 
        /// </summary>
        public int TemplStageId
        {
            set { _templstageid = value; }
            get { return _templstageid; }
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
        public int? SequenceNumber
        {
            set { _sequencenumber = value; }
            get { return _sequencenumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WorkflowType
        {
            set { _workflowtype = value; }
            get { return _workflowtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Custom
        {
            set { _custom = value; }
            get { return _custom; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? PointStageNameField
        {
            set { _pointstagenamefield = value; }
            get { return _pointstagenamefield; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? PointStageDateField
        {
            set { _pointstagedatefield = value; }
            get { return _pointstagedatefield; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Alias
        {
            set { _alias = value; }
            get { return _alias; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DaysFromEstClose
        {
            set { _daysfromestclose = value; }
            get { return _daysfromestclose; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DaysFromCreation
        {
            set { _daysfromcreation = value; }
            get { return _daysfromcreation; }
        }
        #endregion Model

    }
}

