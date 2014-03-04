using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Template_RuleCollection 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Template_RuleCollection
	{
		public Template_RuleCollection()
		{}
		#region Model
		private int _rulecollectionlid;
		private string _name;
		private bool _enabled;
		/// <summary>
		/// 
		/// </summary>
		public int RuleCollectionlId
		{
			set{ _rulecollectionlid=value;}
			get{return _rulecollectionlid;}
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
		public bool Enabled
		{
			set{ _enabled=value;}
			get{return _enabled;}
		}
		#endregion Model

	}
}

