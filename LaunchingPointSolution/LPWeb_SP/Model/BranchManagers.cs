using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����BranchManagers ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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

