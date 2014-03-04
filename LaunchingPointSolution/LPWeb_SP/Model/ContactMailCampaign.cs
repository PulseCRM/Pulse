using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ContactMailCampaign: 
	/// </summary>
	[Serializable]
	public partial class ContactMailCampaign
	{
		public ContactMailCampaign()
		{}
		#region Model
		private int _contactmailcampaignid;
		private int? _contactid;
		private string _cid;
		private string _lid;
		private int? _branchid;
		private DateTime? _added;
		private int _addedby;
		private string _result;
		/// <summary>
		/// 
		/// </summary>
		public int ContactMailCampaignId
		{
			set{ _contactmailcampaignid=value;}
			get{return _contactmailcampaignid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ContactId
		{
			set{ _contactid=value;}
			get{return _contactid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CID
		{
			set{ _cid=value;}
			get{return _cid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LID
		{
			set{ _lid=value;}
			get{return _lid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? BranchId
		{
			set{ _branchid=value;}
			get{return _branchid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Added
		{
			set{ _added=value;}
			get{return _added;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int AddedBy
		{
			set{ _addedby=value;}
			get{return _addedby;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Result
		{
			set{ _result=value;}
			get{return _result;}
		}
		#endregion Model

	}
}

