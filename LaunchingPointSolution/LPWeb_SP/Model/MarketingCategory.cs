using System;
namespace LPWeb.Model
{
	/// <summary>
	/// MarketingCategory:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class MarketingCategory
	{
		public MarketingCategory()
		{}
		#region Model
		private int _categoryid;
		private string _categoryname;
		private string _globalid;
		private string _description;
		/// <summary>
		/// 
		/// </summary>
		public int CategoryId
		{
			set{ _categoryid=value;}
			get{return _categoryid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CategoryName
		{
			set{ _categoryname=value;}
			get{return _categoryname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GlobalId
		{
			set{ _globalid=value;}
			get{return _globalid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		#endregion Model

	}
}

