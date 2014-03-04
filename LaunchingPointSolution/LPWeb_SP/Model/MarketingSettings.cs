using System;
namespace LPWeb.Model
{
	/// <summary>
	/// MarketingSettings:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class MarketingSettings
	{
		public MarketingSettings()
		{}
		#region Model
		private string _webserviceurl;
		private string _campaigndetailurl;
		private int _reconcileinterval;
		/// <summary>
		/// 
		/// </summary>
		public string WebServiceURL
		{
			set{ _webserviceurl=value;}
			get{return _webserviceurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CampaignDetailURL
		{
			set{ _campaigndetailurl=value;}
			get{return _campaigndetailurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ReconcileInterval
		{
			set{ _reconcileinterval=value;}
			get{return _reconcileinterval;}
		}
		#endregion Model

	}
}

