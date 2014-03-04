using System;
namespace LPWeb.Model
{
	/// <summary>
	/// User2State:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class User2State
	{
		public User2State()
		{}
		#region Model
		private int _userid;
		private string _state;
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
		public string State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

