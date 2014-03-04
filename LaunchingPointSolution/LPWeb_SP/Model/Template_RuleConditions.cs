using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Template_RuleConditions 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Template_RuleConditions
	{
		public Template_RuleConditions()
		{}
		#region Model
		private int _rulecondid;
		private int _ruleid;
		private decimal _pointfieldid;
		private int _condition;
		private string _tolerance;
		private string _tolerancetype;
		/// <summary>
		/// 
		/// </summary>
		public int RuleCondId
		{
			set{ _rulecondid=value;}
			get{return _rulecondid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RuleId
		{
			set{ _ruleid=value;}
			get{return _ruleid;}
		}
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
		public int Condition
		{
			set{ _condition=value;}
			get{return _condition;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Tolerance
		{
			set{ _tolerance=value;}
			get{return _tolerance;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ToleranceType
		{
			set{ _tolerancetype=value;}
			get{return _tolerancetype;}
		}
		#endregion Model

	}
}

