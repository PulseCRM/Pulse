using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类PointFieldDesc 。(属性说明自动提取数据库字段的描述信息)
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

