using System;
namespace LPWeb.Model
{
    /// <summary>
    /// Template_EmailSkins:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Template_EmailSkins
    {
        public Template_EmailSkins()
        { }
        #region Model
        private int _emailskinid;
        private string _name;
        private string _desc;
        private string _htmlbody;
        private bool _enabled;
        private bool _default;
        /// <summary>
        /// 
        /// </summary>
        public int EmailSkinId
        {
            set { _emailskinid = value; }
            get { return _emailskinid; }
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
        public string Desc
        {
            set { _desc = value; }
            get { return _desc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HTMLBody
        {
            set { _htmlbody = value; }
            get { return _htmlbody; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Default
        {
            set { _default = value; }
            get { return _default; }
        }
        #endregion Model

    }
}

