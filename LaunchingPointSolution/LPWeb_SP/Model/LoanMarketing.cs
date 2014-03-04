using System;
namespace LPWeb.Model
{
	/// <summary>
	/// LoanMarketing:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class LoanMarketing
	{
		public LoanMarketing()
		{}
		#region Model
		private int _loanmarketingid;
		private DateTime? _selected;
		private string _type;
		private DateTime? _started;
		private int? _startedby;
		private int _campaignid;
		private string _status;
		private int _fileid;
		private int? _selectedby;
		/// <summary>
		/// 
		/// </summary>
		public int LoanMarketingId
		{
			set{ _loanmarketingid=value;}
			get{return _loanmarketingid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Selected
		{
			set{ _selected=value;}
			get{return _selected;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Type
		{
			set{ _type=value;}
			get{return _type;}
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
		public int? StartedBy
		{
			set{ _startedby=value;}
			get{return _startedby;}
		}
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
		public string Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int FileId
		{
			set{ _fileid=value;}
			get{return _fileid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SelectedBy
		{
			set{ _selectedby=value;}
			get{return _selectedby;}
		}
		#endregion Model

	}
}

