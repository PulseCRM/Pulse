using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类BranchManagers 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BranchManagers
	{
		public BranchManagers()
		{}
		#region Model
		private int _branchid;
		private int _branchmgrid;
		/// <summary>
		/// 
		/// </summary>
		public int BranchId
		{
			set{ _branchid=value;}
			get{return _branchid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int BranchMgrId
		{
			set{ _branchmgrid=value;}
			get{return _branchmgrid;}
		}
		#endregion Model

	}
}

