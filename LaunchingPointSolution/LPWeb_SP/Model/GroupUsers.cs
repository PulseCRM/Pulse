using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����GroupUsers ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class GroupUsers
	{
		public GroupUsers()
		{}
		#region Model
		private int _groupid;
		private int _userid;
		/// <summary>
		/// 
		/// </summary>
		public int GroupId
		{
			set{ _groupid=value;}
			get{return _groupid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		#endregion Model

	}
}

