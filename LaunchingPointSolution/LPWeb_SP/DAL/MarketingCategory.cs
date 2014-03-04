using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类:MarketingCategory
	/// </summary>
    public class MarketingCategory : MarketingCategoryBase
	{
		public MarketingCategory()
        { }

        /// <summary>
        /// get marketing categories in alphabetical order
        /// </summary>
        public DataSet GetListInAlphOrder(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CategoryId,CategoryName,GlobalId,Description ");
            strSql.Append(" FROM MarketingCategory ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" ORDER BY CategoryName");
            return DbHelperSQL.Query(strSql.ToString());
        }
	}
}

