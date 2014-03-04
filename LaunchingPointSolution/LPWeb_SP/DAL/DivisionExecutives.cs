using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类DivisionExecutives。
	/// </summary>
    public class DivisionExecutives : DivisionExecutivesBase
	{
		public DivisionExecutives()
		{}

        /// <summary>
        /// 根据UserId 获取查询其所属的Divisons
        /// </summary>
        /// <param name="executiveId">userid</param>
        /// <returns></returns>
        public DataTable GetDivisonsByExecutiveId(int executiveId)
	    {
            if (executiveId <= 0)
                return null;

            string sql = "SELECT     Divisions.Name, DivisionExecutives.ExecutiveId, DivisionExecutives.DivisionId " +
                         "FROM         DivisionExecutives INNER JOIN " +
                         "  Divisions ON DivisionExecutives.DivisionId = Divisions.DivisionId WHERE ExecutiveId IN ({0})";

            return DbHelperSQL.ExecuteDataTable(string.Format(sql, executiveId));
	    }
	}
}

