using System;
namespace LPWeb.Model
{
    /// <summary>
    /// 实体类Template_Email_Attachments 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Template_Email_Attachments
    {
        public Template_Email_Attachments()
        { }
        #region Model
        private int _templattachid;
        private int _templemailid;
        private bool _enabled;
        private string _name;
        private string _filetype;
        private byte[] _fileimage;
        /// <summary>
        /// 
        /// </summary>
        public int TemplAttachId
        {
            set { _templattachid = value; }
            get { return _templattachid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TemplEmailId
        {
            set { _templemailid = value; }
            get { return _templemailid; }
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
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FileType
        {
            set { _filetype = value; }
            get { return _filetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] FileImage
        {
            set { _fileimage = value; }
            get { return _fileimage; }
        }
        #endregion Model

    }
}

