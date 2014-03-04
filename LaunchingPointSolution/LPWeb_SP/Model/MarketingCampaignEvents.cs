using System;
namespace LPWeb.Model
{
    /// <summary>
    /// MarketingCampaignEvents:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class MarketingCampaignEvents
    {
        public MarketingCampaignEvents()
        { }
        #region Model
        private int _campaigneventid;
        private int? _weekno;
        private string _action;
        private string _eventcontent;
        private string _eventurl;
        private string _globalid;
        private int _campaignid;
        /// <summary>
        /// 
        /// </summary>
        public int CampaignEventId
        {
            set { _campaigneventid = value; }
            get { return _campaigneventid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? WeekNo
        {
            set { _weekno = value; }
            get { return _weekno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Action
        {
            set { _action = value; }
            get { return _action; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EventContent
        {
            set { _eventcontent = value; }
            get { return _eventcontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EventURL
        {
            set { _eventurl = value; }
            get { return _eventurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GlobalId
        {
            set { _globalid = value; }
            get { return _globalid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CampaignId
        {
            set { _campaignid = value; }
            get { return _campaignid; }
        }
        #endregion Model

    }
}

