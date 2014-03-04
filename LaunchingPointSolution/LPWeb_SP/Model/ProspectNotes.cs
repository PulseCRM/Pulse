using System;

namespace LPWeb.Model
{
    /// <summary>
    /// 实体类ProspectNotes 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class ProspectNotes
    {
        public ProspectNotes()
        { }
        #region Model
        private int _propsectnoteid;
        private int _contactid;
        private DateTime _created;
        private string _sender;
        private string _note;
        private bool _exported;
        /// <summary>
        /// 
        /// </summary>
        public int PropsectNoteId
        {
            set { _propsectnoteid = value; }
            get { return _propsectnoteid; }
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
        public DateTime Created
        {
            set { _created = value; }
            get { return _created; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Sender
        {
            set { _sender = value; }
            get { return _sender; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Note
        {
            set { _note = value; }
            get { return _note; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Exported
        {
            set { _exported = value; }
            get { return _exported; }
        }
        #endregion Model

    }
}
