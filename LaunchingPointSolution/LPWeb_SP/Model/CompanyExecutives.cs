using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����CompanyExecutives ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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

