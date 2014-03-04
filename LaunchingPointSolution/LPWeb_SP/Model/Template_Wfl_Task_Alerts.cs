using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Template_Wfl_Task_Alerts 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Template_Wfl_Task_Alerts
	{
		public Template_Wfl_Task_Alerts()
		{}
		#region Model
		private int _templalertid;
		private int _templtaskid;
		private int? _successtemplateid;
		private int? _warningtemplateid;
		private int? _overduetemplateid;
		private int? _tocontacttype;
		/// <summary>
		/// 
		/// </summary>
		public int TemplAlertId
		{
			set{ _templalertid=value;}
			get{return _templalertid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int TemplTaskId
		{
			set{ _templtaskid=value;}
			get{return _templtaskid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SuccessTemplateId
		{
			set{ _successtemplateid=value;}
			get{return _successtemplateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? WarningTemplateId
		{
			set{ _warningtemplateid=value;}
			get{return _warningtemplateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OverdueTemplateId
		{
			set{ _overduetemplateid=value;}
			get{return _overduetemplateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ToContactType
		{
			set{ _tocontacttype=value;}
			get{return _tocontacttype;}
		}
		#endregion Model

	}
}

