using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����LoanActivities ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class LoanActivities
	{
		public LoanActivities()
		{}
		#region Model
		private int _activityid;
		private int _fileid;
		private int? _userid;
		private string _activityname;
		private DateTime _activitytime;
		/// <summary>
		/// 
		/// </summary>
		public int ActivityId
		{
			set{ _activityid=value;}
			get{return _activityid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int FileId
		{
			set{ _fileid=value;}
			get{return _fileid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ActivityName
		{
			set{ _activityname=value;}
			get{return _activityname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ActivityTime
		{
			set{ _activitytime=value;}
			get{return _activitytime;}
		}
		#endregion Model

	}
}

