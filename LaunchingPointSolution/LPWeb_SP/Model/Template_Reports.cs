using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����Template_Reports ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Template_Reports
	{
		public Template_Reports()
		{}
		#region Model
		private int _templreportid;
		private string _name;
		private bool _standard;
		private string _htmltemplcontent;
		/// <summary>
		/// 
		/// </summary>
		public int TemplReportId
		{
			set{ _templreportid=value;}
			get{return _templreportid;}
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
		public bool Standard
		{
			set{ _standard=value;}
			get{return _standard;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HtmlTemplContent
		{
			set{ _htmltemplcontent=value;}
			get{return _htmltemplcontent;}
		}
		#endregion Model

	}
}

