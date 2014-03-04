using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����PointImportHistory ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class PointImportHistory
	{
		public PointImportHistory()
		{}
		#region Model
		private int _historyid;
		private int _fileid;
		private DateTime _importtime;
		private bool _success;
		private string _error;
		/// <summary>
		/// 
		/// </summary>
		public int HistoryId
		{
			set{ _historyid=value;}
			get{return _historyid;}
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
		public DateTime ImportTime
		{
			set{ _importtime=value;}
			get{return _importtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Success
		{
			set{ _success=value;}
			get{return _success;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Error
		{
			set{ _error=value;}
			get{return _error;}
		}
		#endregion Model

	}
}

