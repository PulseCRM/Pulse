using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Template_Email 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Template_Email
	{
		public Template_Email()
		{}
		#region Model
		private int _templemailid;
		private bool _enabled;
		private string _name;
		private string _desc;
		private int? _fromuserroles;
		private string _fromemailaddress;
		private string _content;
        private string _subject;
        private int _EmailSkinId;
		/// <summary>
		/// 
		/// </summary>
		public int TemplEmailId
		{
			set{ _templemailid=value;}
			get{return _templemailid;}
		}

        public int EmailSkinId
        {
            set { _EmailSkinId = value; }
            get { return _EmailSkinId; }
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
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Desc
		{
			set{ _desc=value;}
			get{return _desc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? FromUserRoles
		{
			set{ _fromuserroles=value;}
			get{return _fromuserroles;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FromEmailAddress
		{
			set{ _fromemailaddress=value;}
			get{return _fromemailaddress;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string Subject
        {
            set { _subject = value; }
            get { return _subject; }
        }
		#endregion Model

	}
}

