using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类ServiceTypes 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class ServiceTypes
	{
		public ServiceTypes()
		{}

        #region Model
        private int _servicetypeid;
        private string _name;
        private string _servicetype;
        private bool _enabled;

        /// <summary>
        /// 
        /// </summary>
        public int ServiceTypeId
        {
            set { _servicetypeid = value; }
            get { return _servicetypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ServiceType
        {
            set { _servicetype = value; }
            get { return _servicetype; }
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

