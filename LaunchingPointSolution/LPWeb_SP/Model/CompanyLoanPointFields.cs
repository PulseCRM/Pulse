using System;
namespace LPWeb.Model
{
    /// <summary>
    /// CompanyLoanPointFields:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class CompanyLoanPointFields
    {
        public CompanyLoanPointFields()
        { }
        #region Model
        private int _pointfieldid;
        private string _heading;
        /// <summary>
        /// 
        /// </summary>
        public int PointFieldId
        {
            set { _pointfieldid = value; }
            get { return _pointfieldid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Heading
        {
            set { _heading = value; }
            get { return _heading; }
        }
        #endregion Model

    }
}

