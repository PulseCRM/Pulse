using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Template_Wfl_Stages 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
    [Serializable]
    public class Template_Wfl_Stages
    {
        public Template_Wfl_Stages()
        { }
        #region Model
        private int _wflstageid;
        private int _wfltemplid;
        private int _sequencenumber;
        private bool _enabled;
        private int? _daysfromestclose;
        private string _name;
        private int? _daysfromcreation;
        private int _templstageid;
        private int? _calculationmethod;

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
        public int WflTemplId
        {
            set { _wfltemplid = value; }
            get { return _wfltemplid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SequenceNumber
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
        public string Name
        {
            set { _name = value; }
            get { return _name; }
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
        public int TemplStageId
        {
            set { _templstageid = value; }
            get { return _templstageid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CalculationMethod
        {
            set { _calculationmethod = value; }
            get { return _calculationmethod; }
        }
        #endregion Model

    }
}

