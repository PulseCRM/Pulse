using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Template_Group_N_Rules 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Template_Group_N_Rules
	{
		public Template_Group_N_Rules()
		{}
		#region Model
		private int _rulegroupid;
		private int _ruleid;
		/// <summary>
		/// 
		/// </summary>
		public int RuleGroupId
		{
			set{ _rulegroupid=value;}
			get{return _rulegroupid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RuleId
		{
			set{ _ruleid=value;}
			get{return _ruleid;}
		}
		#endregion Model

	}
}

