using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类GroupFolder 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class GroupFolder
	{
		public GroupFolder()
		{}
		#region Model
		private int _groupid;
		private int _folderid;
		/// <summary>
		/// 
		/// </summary>
		public int GroupId
		{
			set{ _groupid=value;}
			get{return _groupid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int FolderId
		{
			set{ _folderid=value;}
			get{return _folderid;}
		}
		#endregion Model

	}
}

