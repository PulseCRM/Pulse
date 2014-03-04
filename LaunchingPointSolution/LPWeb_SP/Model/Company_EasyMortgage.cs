using System;
namespace LPWeb.Model
{
    /// <summary>
    /// Company_EasyMortgage:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Company_EasyMortgage
    {
        public Company_EasyMortgage()
        { }
        #region Model
        private string _clientid;
        private string _url;
        private int? _syncintervalhours;
        private bool _enabled;
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
        public string URL
        {
            set { _url = value; }
            get { return _url; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SyncIntervalHours
        {
            set { _syncintervalhours = value; }
            get { return _syncintervalhours; }
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

