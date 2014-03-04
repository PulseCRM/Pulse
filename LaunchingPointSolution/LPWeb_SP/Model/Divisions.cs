using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Divisions 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Divisions
	{
		public Divisions()
		{}
		#region Model
		private int _divisionid;
		private string _name;
		private string _desc;
		private bool _enabled;
		private int? _regionid;
		private int? _groupid;
		/// <summary>
		/// 
		/// </summary>
		public int DivisionId
		{
			set{ _divisionid=value;}
			get{return _divisionid;}
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
		public bool Enabled
		{
			set{ _enabled=value;}
			get{return _enabled;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RegionID
		{
			set{ _regionid=value;}
			get{return _regionid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? GroupID
		{
			set{ _groupid=value;}
			get{return _groupid;}
		}
		#endregion Model

	}
}

