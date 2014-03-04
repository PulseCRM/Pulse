using System;
namespace LPWeb.Model
{
    /// <summary>
    /// MarketingCampaigns:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class MarketingCampaigns
    {
        public MarketingCampaigns()
        { }
        #region Model
        private int _campaignid;
        private string _globalid;
        private string _campaignname;
        private int? _categoryid;
        private string _description;
        private decimal? _price;
        private string _imageurl;
        private string _status;
        /// <summary>
        /// 
        /// </summary>
        public int CampaignId
        {
            set { _campaignid = value; }
            get { return _campaignid; }
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
        public string CampaignName
        {
            set { _campaignname = value; }
            get { return _campaignname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CategoryId
        {
            set { _categoryid = value; }
            get { return _categoryid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImageUrl
        {
            set { _imageurl = value; }
            get { return _imageurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        #endregion Model

    }
}

