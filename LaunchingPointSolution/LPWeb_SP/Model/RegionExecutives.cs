using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����RegionExecutives ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class RegionExecutives
	{
		public RegionExecutives()
		{}
		#region Model
		private int _regionid;
		private int _executiveid;
		/// <summary>
		/// 
		/// </summary>
		public int RegionId
		{
			set{ _regionid=value;}
			get{return _regionid;}
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

