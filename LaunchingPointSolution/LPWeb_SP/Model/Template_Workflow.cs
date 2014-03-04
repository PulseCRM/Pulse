using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Template_Workflow 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Template_Workflow
	{
		public Template_Workflow()
		{}
        #region Model
		private int _wfltemplid;
		private string _name;
		private bool _enabled;
        private string _desc;
        private int _daysfromestclose;
        private int _stageid;
        private bool _default;
        private string _workflowtype;
        private bool _custom;
        private int _calculationmethod;

		/// <summary>
		/// 
		/// </summary>
		public int WflTemplId
		{
			set{ _wfltemplid=value;}
			get{return _wfltemplid;}
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
		public bool Enabled
		{
			set{ _enabled=value;}
			get{return _enabled;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Desc
		{
			set{ _desc=value;}
			get{return _desc;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int DaysFromEstClose
        {
            set { _daysfromestclose = value; }
            get { return _daysfromestclose; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int StageId
        {
            set { _stageid = value; }
            get { return _stageid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CalculationMethod
        {
            set { _calculationmethod = value; }
            get { return _calculationmethod; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Default
        {
            set { _default = value; }
            get { return _default; }
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
        public string WorkflowType
        {
            set { _workflowtype = value; }
            get { return _workflowtype; }
        }
		#endregion Model

	}
}

