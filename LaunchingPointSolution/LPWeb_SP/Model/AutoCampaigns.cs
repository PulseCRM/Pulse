using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类AutoCampaigns 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class AutoCampaigns
	{
		public AutoCampaigns()
		{}
		#region Model
		private int _campaignid;
		private int? _paidby;
		private bool _enabled;
		private int _selectedby;
		private DateTime? _started;
		private string _loantype;
		private int? _templstageid;
		/// <summary>
		/// 
		/// </summary>
		public int CampaignId
		{
			set{ _campaignid=value;}
			get{return _campaignid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? PaidBy
		{
			set{ _paidby=value;}
			get{return _paidby;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Enabled
		{
			set{ _enabled=value;}
			get{return _enabled;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int SelectedBy
		{
			set{ _selectedby=value;}
			get{return _selectedby;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Started
		{
			set{ _started=value;}
			get{return _started;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LoanType
		{
			set{ _loantype=value;}
			get{return _loantype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TemplStageId
		{
			set{ _templstageid=value;}
			get{return _templstageid;}
		}
		#endregion Model

	}
}

