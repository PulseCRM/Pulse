using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类CompanyExecutives 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class CompanyExecutives
	{
		public CompanyExecutives()
		{}
		#region Model
		private int _executiveid;
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

