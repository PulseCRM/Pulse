using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类LoanPointFields 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class LoanPointFields
	{
		public LoanPointFields()
		{}
		#region Model
		private int _fileid;
		private int _pointfieldid;
		private string _prevvalue;
		private string _currentvalue;
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
		public int PointFieldId
		{
			set{ _pointfieldid=value;}
			get{return _pointfieldid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PrevValue
		{
			set{ _prevvalue=value;}
			get{return _prevvalue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CurrentValue
		{
			set{ _currentvalue=value;}
			get{return _currentvalue;}
		}
		#endregion Model

	}
}

