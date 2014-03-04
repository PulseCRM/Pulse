using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPWeb.Model
{
    /// <summary>
    /// 实体类Company_Lead_Sources 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Company_Lead_Sources
    {
        public Company_Lead_Sources()
		{}
		#region Model
        private int _leadsourceid;
        private string _leadsource;
		/// <summary>
		/// 
		/// </summary>
        public int LeadSourceID
		{
            set { _leadsourceid = value; }
            get { return _leadsourceid; }
		}
		/// <summary>
		/// 
		/// </summary>
        public string LeadSource
		{
            set { _leadsource = value; }
            get { return _leadsource; }
		}
        /// <summary>
        /// get or set the default user id
        /// </summary>
        public int DefaultUserId { get; set; }
        /// <summary>
        /// Get or set the default user name
        /// </summary>
        public string DefaultUser { get; set; }
        /// <summary>
        /// get or set the default
        /// </summary>
        public bool Default { get; set; }
        /// <summary>
        /// get or set the defaut string(1=Yes, 0=No)
        /// </summary>
        public string DefaultString { get; set; }
        
        #endregion Model
    }

}
