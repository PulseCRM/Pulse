using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����PointFieldDesc ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class PointFieldDesc
	{
		public PointFieldDesc()
		{}
		#region Model
		private decimal _pointfieldid;
		private string _label;
		private int _datatype;
		/// <summary>
		/// 
		/// </summary>
		public decimal PointFieldId
		{
			set{ _pointfieldid=value;}
			get{return _pointfieldid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Label
		{
			set{ _label=value;}
			get{return _label;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int DataType
		{
			set{ _datatype=value;}
			get{return _datatype;}
		}
		#endregion Model

	}
}

