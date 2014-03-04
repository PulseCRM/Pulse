using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类RegionExecutives 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class RegionExecutives
	{
		public RegionExecutives()
		{}
		#region Model
		private int _regionid;
		private int _executiveid;
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
		public int ExecutiveId
		{
			set{ _executiveid=value;}
			get{return _executiveid;}
		}
		#endregion Model

	}
}

