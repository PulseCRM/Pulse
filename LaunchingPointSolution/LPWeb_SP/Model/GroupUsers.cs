using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类GroupUsers 。(属性说明自动提取数据库字段的描述信息)
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

