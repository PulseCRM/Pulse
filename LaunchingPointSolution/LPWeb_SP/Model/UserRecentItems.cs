using System;
namespace LPWeb.Model
{
    /// <summary>
    /// UserRecentItems:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class UserRecentItems
    {
        public UserRecentItems()
        { }
        #region Model
        private int _userid;
        private DateTime _lastaccessed;
        private int _fileid;
        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastAccessed
        {
            set { _lastaccessed = value; }
            get { return _lastaccessed; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int FileId
        {
            set { _fileid = value; }
            get { return _fileid; }
        }
        #endregion Model

    }
}

