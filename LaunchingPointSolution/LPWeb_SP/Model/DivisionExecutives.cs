using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����DivisionExecutives ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class DivisionExecutives
	{
		public DivisionExecutives()
		{}
		#region Model
		private int _divisionid;
		private int _executiveid;
		/// <summary>
		/// 
		/// </summary>
		public int DivisionId
		{
			set{ _divisionid=value;}
			get{return _divisionid;}
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

