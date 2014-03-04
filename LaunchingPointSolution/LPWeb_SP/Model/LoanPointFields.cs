using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����LoanPointFields ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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

