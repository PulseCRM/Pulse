using System;
namespace LPWeb.Model
{
	/// <summary>
	/// MailChimpCampaigns 
	/// </summary>
	[Serializable]
	public partial class MailChimpCampaigns
	{
		public MailChimpCampaigns()
		{}
		#region Model
		private string _cid;
		private string _name;
		private int? _branchid;
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
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? BranchId
		{
			set{ _branchid=value;}
			get{return _branchid;}
		}
		#endregion Model

	}
}

