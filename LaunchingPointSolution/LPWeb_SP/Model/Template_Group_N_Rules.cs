using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����Template_Group_N_Rules ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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

