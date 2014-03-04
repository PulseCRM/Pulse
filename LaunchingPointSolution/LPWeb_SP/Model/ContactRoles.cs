using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����ContactRoles ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class ContactRoles
	{
		public ContactRoles()
		{}
		#region Model
		private int _contactroleid;
        private string _name;
        private bool _enabled;
		/// <summary>
		/// 
		/// </summary>
		public int ContactRoleId
		{
			set{ _contactroleid=value;}
			get{return _contactroleid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }
		#endregion Model

	}
}

