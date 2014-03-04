using System;
namespace LPWeb.Model
{
	/// <summary>
	/// MailChimpLists 
	/// </summary>
	[Serializable]
	public partial class MailChimpLists
	{
		public MailChimpLists()
		{}
		#region Model
		private string _lid;
		private string _name;
		private string _branchid;
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
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BranchId
		{
			set{ _branchid=value;}
			get{return _branchid;}
		}
		#endregion Model

	}
}

