using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����ContactUsers ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class ContactUsers
	{
		public ContactUsers()
		{}
		#region Model
		private int _userid;
		private int _contactid;
		private bool _enabled;
		private DateTime? _created;
		/// <summary>
		/// 
		/// </summary>
		public int UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ContactId
		{
			set{ _contactid=value;}
			get{return _contactid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Enabled
		{
			set{ _enabled=value;}
			get{return _enabled;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Created
		{
			set{ _created=value;}
			get{return _created;}
		}
		#endregion Model

	}
}

