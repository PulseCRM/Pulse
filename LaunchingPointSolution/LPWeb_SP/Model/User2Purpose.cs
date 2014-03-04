using System;
namespace LPWeb.Model
{
	/// <summary>
	/// User2Purpose:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class User2Purpose
	{
		public User2Purpose()
		{}
		#region Model
		private int _userid;
		private string _purpose;
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
		public string Purpose
		{
			set{ _purpose=value;}
			get{return _purpose;}
		}
		#endregion Model

	}
}

