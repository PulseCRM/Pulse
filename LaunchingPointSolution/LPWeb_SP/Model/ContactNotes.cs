using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����ContactNotes ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class ContactNotes
	{
		public ContactNotes()
		{}
		#region Model
		private int _contactnoteid;
		private int _contactid;
		private DateTime _created;
		private int _createdby;
		private string _note;
		/// <summary>
		/// 
		/// </summary>
		public int ContactNoteId
		{
			set{ _contactnoteid=value;}
			get{return _contactnoteid;}
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
		public DateTime Created
		{
			set{ _created=value;}
			get{return _created;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int CreatedBy
		{
			set{ _createdby=value;}
			get{return _createdby;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Note
		{
			set{ _note=value;}
			get{return _note;}
		}
		#endregion Model

	}
}

