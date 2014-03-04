using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Regions 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Regions
	{
		public Regions()
		{}
		#region Model
		private int _regionid;
		private string _name;
		private string _desc;
		private bool _enabled;
	    private int? _groupid;
        /// <summary>
        /// 
        /// </summary>
        public int? GroupID
        {
            set { _groupid = value; }
            get { return _groupid; }
        }
		/// <summary>
		/// 
		/// </summary>
		public int RegionId
		{
			set{ _regionid=value;}
			get{return _regionid;}
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
		#endregion Model

	}
}

