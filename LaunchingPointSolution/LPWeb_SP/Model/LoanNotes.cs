using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类LoanNotes 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class LoanNotes
	{
		public LoanNotes()
		{}
		#region Model
		private int _noteid;
		private int _fileid;
		private DateTime _created;
		private string _sender;
		private string _note;
        private bool _exported;
        private bool _externalviewing;
        private int _loantaskid;
		/// <summary>
		/// 
		/// </summary>
		public int NoteId
		{
			set{ _noteid=value;}
			get{return _noteid;}
		}
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
		public DateTime Created
		{
			set{ _created=value;}
			get{return _created;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Sender
		{
			set{ _sender=value;}
			get{return _sender;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Note
		{
			set{ _note=value;}
			get{return _note;}
		}

	    public bool Exported
	    {
            set { _exported = value; }
            get { return _exported; }
	    }

        public bool ExternalViewing
        {
            set { _externalviewing = value; }
            get { return _externalviewing; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LoanTaskId
        {
            set { _loantaskid = value; }
            get { return _loantaskid; }
        }

	    #endregion Model

	}
}

