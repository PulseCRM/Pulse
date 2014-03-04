using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Template_RuleGroups 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Template_RuleGroups
	{
        public Template_RuleGroups()
        { }
        #region Model
        private int _rulegroupid;
        private string _name;
        private string _desc;
        private bool _enabled;
        private int? _rulescope;
        private int? _loantarget;
        /// <summary>
        /// 
        /// </summary>
        public int RuleGroupId
        {
            set { _rulegroupid = value; }
            get { return _rulegroupid; }
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
        public int? RuleScope
        {
            set { _rulescope = value; }
            get { return _rulescope; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LoanTarget
        {
            set { _loantarget = value; }
            get { return _loantarget; }
        }
        #endregion Model

	}
}

