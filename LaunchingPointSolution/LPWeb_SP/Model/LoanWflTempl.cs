using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类LoanWflTempl 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class LoanWflTempl
	{
		public LoanWflTempl()
		{}
		#region Model
		private int _fileid;
		private int _wfltemplid;
		private DateTime _applydate;
		private int _applyby;
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
		public int WflTemplId
		{
			set{ _wfltemplid=value;}
			get{return _wfltemplid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ApplyDate
		{
			set{ _applydate=value;}
			get{return _applydate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ApplyBy
		{
			set{ _applyby=value;}
			get{return _applyby;}
		}
		#endregion Model

	}
}

