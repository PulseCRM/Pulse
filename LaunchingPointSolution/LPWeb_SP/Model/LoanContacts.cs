using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����LoanContacts ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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

