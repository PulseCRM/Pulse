using System;
namespace LPWeb.Model
{
    /// <summary>
    /// UserLoansViewPointFields:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class UserLoansViewPointFields
    {
        public UserLoansViewPointFields()
        { }
        #region Model
        private int _userid;
        private int _pointfieldid;
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
        public int PointFieldId
        {
            set { _pointfieldid = value; }
            get { return _pointfieldid; }
        }
        #endregion Model

    }
}

