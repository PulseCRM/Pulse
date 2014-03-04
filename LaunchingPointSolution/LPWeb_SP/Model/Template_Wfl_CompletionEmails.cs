using System;
namespace LPWeb.Model
{
    /// <summary>
    /// Template_Wfl_CompletionEmails:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Template_Wfl_CompletionEmails
    {
        public Template_Wfl_CompletionEmails()
        { }
        #region Model
        private int _completionemailid;
        private int _templtaskid;
        private int _templemailid;
        private bool? _enabled;
        /// <summary>
        /// 
        /// </summary>
        public int CompletionEmailId
        {
            set { _completionemailid = value; }
            get { return _completionemailid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TemplTaskid
        {
            set { _templtaskid = value; }
            get { return _templtaskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TemplEmailId
        {
            set { _templemailid = value; }
            get { return _templemailid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? Enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }
        #endregion Model

    }
}

