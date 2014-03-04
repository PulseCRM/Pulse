using System;
namespace LPWeb.Model
{
    /// <summary>
    /// Email_AttachmentsTemp:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Email_AttachmentsTemp
    {
        public Email_AttachmentsTemp()
        { }
        #region Model
        private int _id;
        private string _token;
        private int? _templattachid;
        private string _name;
        private string _filetype;
        private byte[] _fileimage;
        private DateTime _createdatetime = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Token
        {
            set { _token = value; }
            get { return _token; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TemplAttachId
        {
            set { _templattachid = value; }
            get { return _templattachid; }
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
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDateTime
        {
            set { _createdatetime = value; }
            get { return _createdatetime; }
        }
        #endregion Model

    }
}

