using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ArchiveLeadStatus:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class ArchiveLeadStatus
	{
		public ArchiveLeadStatus()
		{}
		#region Model
		private int _leadstatusid;
		private string _leadstatusname;
		private bool _enabled;
		/// <summary>
		/// 
		/// </summary>
		public int LeadStatusId
		{
			set{ _leadstatusid=value;}
			get{return _leadstatusid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LeadStatusName
		{
			set{ _leadstatusname=value;}
			get{return _leadstatusname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Enabled
		{
			set{ _enabled=value;}
			get{return _enabled;}
		}
		#endregion Model

	}
}

