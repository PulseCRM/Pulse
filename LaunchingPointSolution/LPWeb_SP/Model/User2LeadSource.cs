using System;
namespace LPWeb.Model
{
	/// <summary>
	/// User2LeadSource:ʵ����(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class User2LeadSource
	{
		public User2LeadSource()
		{}
		#region Model
		private int _userid;
		private int _leadsourceid;
		/// <summary>
		/// 
		/// </summary>
		public int UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int LeadSourceID
		{
			set{ _leadsourceid=value;}
			get{return _leadsourceid;}
		}
		#endregion Model

	}
}

