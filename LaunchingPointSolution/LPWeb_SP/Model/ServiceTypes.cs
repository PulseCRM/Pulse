using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ʵ����ServiceTypes ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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

