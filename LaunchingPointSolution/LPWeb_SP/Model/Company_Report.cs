using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPWeb.Model
{
    /// <summary>
    /// Company_Report:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Company_Report
    {
        public Company_Report()
        { }
        #region Model
        private int? _dow;
        private int _tod;
        private int? _senderroleid;
        private string _senderemail;
        private string _sendername;
        /// <summary>
        /// 
        /// </summary>
        public int? DOW
        {
            set { _dow = value; }
            get { return _dow; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TOD
        {
            set { _tod = value; }
            get { return _tod; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SenderRoleId
        {
            set { _senderroleid = value; }
            get { return _senderroleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SenderEmail
        {
            set { _senderemail = value; }
            get { return _senderemail; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SenderName
        {
            set { _sendername = value; }
            get { return _sendername; }
        }
        #endregion Model

    }
}
