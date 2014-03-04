using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类LoanContacts 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class LoanContacts
	{
		public LoanContacts()
		{}
		#region Model
		private int _fileid;
		private int _contactroleid;
		private int _contactid;
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
		public int ContactRoleId
		{
			set{ _contactroleid=value;}
			get{return _contactroleid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ContactId
		{
			set{ _contactid=value;}
			get{return _contactid;}
		}
		#endregion Model

	}
}

