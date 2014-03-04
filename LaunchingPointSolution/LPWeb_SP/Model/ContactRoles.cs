using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类ContactRoles 。(属性说明自动提取数据库字段的描述信息)
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

