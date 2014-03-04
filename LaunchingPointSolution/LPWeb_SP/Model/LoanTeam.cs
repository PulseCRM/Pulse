using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类LoanTeam 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class LoanTeam
	{
		public LoanTeam()
		{}
		#region Model
		private int _fileid;
		private int _roleid;
		private int _userid;
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
		public int RoleId
		{
			set{ _roleid=value;}
			get{return _roleid;}
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

