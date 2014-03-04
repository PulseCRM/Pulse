using System;
namespace LPWeb.Model
{
    /// <summary>
    /// ProspectAlerts:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class ProspectAlerts
    {
        public ProspectAlerts()
        { }
        #region Model
        private int _prospectalertid;
        private int _contactid;
        private string _desc;
        private DateTime _duedate;
        private int? _ownerid;
        private int? _prospecttaskid;
        private string _alerttype;
        private DateTime? _created;
        /// <summary>
        /// 
        /// </summary>
        public int ProspectAlertId
        {
            set { _prospectalertid = value; }
            get { return _prospectalertid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ContactId
        {
            set { _contactid = value; }
            get { return _contactid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Desc
        {
            set { _desc = value; }
            get { return _desc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime DueDate
        {
            set { _duedate = value; }
            get { return _duedate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OwnerId
        {
            set { _ownerid = value; }
            get { return _ownerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProspectTaskId
        {
            set { _prospecttaskid = value; }
            get { return _prospecttaskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AlertType
        {
            set { _alerttype = value; }
            get { return _alerttype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Created
        {
            set { _created = value; }
            get { return _created; }
        }
        #endregion Model

    }
}

