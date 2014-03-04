using System;
namespace LPWeb.Model
{
    /// <summary>
    /// 实体类Prospect 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Prospect
    {
        public Prospect()
        { }
        #region Model
        private int _contactid;
        private string _leadsource;
        private string _referencecode;
        private int? _referral;
        private DateTime? _created;
        private int? _createdby;
        private DateTime? _modifed;
        private int? _modifiedby;
        private int? _loanofficer;
        private string _status;
        private string _creditranking;
        private string _preferredcontact;
        private bool _dependents;
        /// <summary>
        /// 
        /// </summary>
        public int Contactid
        {
            set { _contactid = value; }
            get { return _contactid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LeadSource
        {
            set { _leadsource = value; }
            get { return _leadsource; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReferenceCode
        {
            set { _referencecode = value; }
            get { return _referencecode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Referral
        {
            set { _referral = value; }
            get { return _referral; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Created
        {
            set { _created = value; }
            get { return _created; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CreatedBy
        {
            set { _createdby = value; }
            get { return _createdby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modifed
        {
            set { _modifed = value; }
            get { return _modifed; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ModifiedBy
        {
            set { _modifiedby = value; }
            get { return _modifiedby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Loanofficer
        {
            set { _loanofficer = value; }
            get { return _loanofficer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreditRanking
        {
            set { _creditranking = value; }
            get { return _creditranking; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PreferredContact
        {
            set { _preferredcontact = value; }
            get { return _preferredcontact; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Dependents
        {
            set { _dependents = value; }
            get { return _dependents; }
        }
        #endregion Model

    }
}

