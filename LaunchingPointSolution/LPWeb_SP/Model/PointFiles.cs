using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类PointFiles 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class PointFiles
	{
		public PointFiles()
		{}
		#region Model
		private int _fileid;
		private int _folderid;
		private string _name;
		private DateTime? _firstimported;
		private DateTime? _lastimported;
		private bool _success;
		private string _currentimage;
		private string _previousimage;
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
		public int FolderId
		{
			set{ _folderid=value;}
			get{return _folderid;}
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
		public DateTime? FirstImported
		{
			set{ _firstimported=value;}
			get{return _firstimported;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastImported
		{
			set{ _lastimported=value;}
			get{return _lastimported;}
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
		public string CurrentImage
		{
			set{ _currentimage=value;}
			get{return _currentimage;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PreviousImage
		{
			set{ _previousimage=value;}
			get{return _previousimage;}
		}
		#endregion Model

	}
}

