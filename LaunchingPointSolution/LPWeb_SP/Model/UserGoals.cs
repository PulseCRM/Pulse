using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����UserGoals ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class UserGoals
	{
		public UserGoals()
		{}
		#region Model
		private int _goalid;
		private int _userid;
		private decimal? _lowrange;
		private decimal? _mediumrange;
		private decimal? _highrange;
		private int? _month;
		/// <summary>
		/// 
		/// </summary>
		public int GoalId
		{
			set{ _goalid=value;}
			get{return _goalid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? LowRange
		{
			set{ _lowrange=value;}
			get{return _lowrange;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? MediumRange
		{
			set{ _mediumrange=value;}
			get{return _mediumrange;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? HighRange
		{
			set{ _highrange=value;}
			get{return _highrange;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Month
		{
			set{ _month=value;}
            get { return _month; }
		}
		#endregion Model

	}
}

