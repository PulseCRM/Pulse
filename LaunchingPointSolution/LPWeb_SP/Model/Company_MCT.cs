using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPWeb.Model
{
    /// <summary>
    /// 实体类Loans 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Company_MCT
    {

        public Company_MCT()
		{}

        #region Model
        private string _clientid;
        private string _posturl;
        private bool _postdataenabled;
        private int _activeloaninterval;
        private int _archivedloaninterval;
        private int _archivedloandisposemonth;
        private string _archivedloanstatuses;
        /// <summary>
        /// 
        /// </summary>
        public string ClientID
        {
            set { _clientid = value; }
            get { return _clientid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PostURL
        {
            set { _posturl = value; }
            get { return _posturl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool PostDataEnabled
        {
            set { _postdataenabled = value; }
            get { return _postdataenabled; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ActiveLoanInterval
        {
            set { _activeloaninterval = value; }
            get { return _activeloaninterval; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ArchivedLoanInterval
        {
            set { _archivedloaninterval = value; }
            get { return _archivedloaninterval; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ArchivedLoanDisposeMonth
        {
            set { _archivedloandisposemonth = value; }
            get { return _archivedloandisposemonth; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ArchivedLoanStatuses
        {
            set { _archivedloanstatuses = value; }
            get { return _archivedloanstatuses; }
        }
        #endregion
    }
}
