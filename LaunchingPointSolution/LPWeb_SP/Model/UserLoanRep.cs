using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����UserLoanRep ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class UserLoanRep
	{
		public UserLoanRep()
		{}
		#region Model
		private int _nameid;
		private int? _branchid;
		private string _name;
		private int? _userid;
		/// <summary>
		/// 
		/// </summary>
		public int NameId
		{
			set{ _nameid=value;}
			get{return _nameid;}
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
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		#endregion Model

	}
}

