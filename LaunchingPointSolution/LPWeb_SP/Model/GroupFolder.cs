using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����GroupFolder ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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

