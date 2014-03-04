using System;
namespace LPWeb.Model
{
    /// <summary>
    /// LoanTask_CompletionEmails:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class LoanTask_CompletionEmails
    {
        public LoanTask_CompletionEmails()
        { }
        #region Model
        private int _taskcompletionemailid;
        private int _loantaskid;
        private int _templemailid;
        private bool _enabled;
        /// <summary>
        /// 
        /// </summary>
        public int TaskCompletionEmailId
        {
            set { _taskcompletionemailid = value; }
            get { return _taskcompletionemailid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int LoanTaskid
        {
            set { _loantaskid = value; }
            get { return _loantaskid; }
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
        public bool Enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }
        #endregion Model

    }
}

