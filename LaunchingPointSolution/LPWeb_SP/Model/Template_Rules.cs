using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Template_Rules 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Template_Rules
	{
		public Template_Rules()
		{}
		#region Model
		private int _ruleid;
		private string _name;
		private string _desc;
		private bool _enabled;
		private int _alertemailtemplid;
		private bool _ackreq;
		private int? _recomemailtemplid;
		private string _advformula;
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
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string desc
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
		public int AlertEmailTemplId
		{
			set{ _alertemailtemplid=value;}
			get{return _alertemailtemplid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool AckReq
		{
			set{ _ackreq=value;}
			get{return _ackreq;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RecomEmailTemplid
		{
			set{ _recomemailtemplid=value;}
			get{return _recomemailtemplid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AdvFormula
		{
			set{ _advformula=value;}
			get{return _advformula;}
		}
		#endregion Model

	}
}

