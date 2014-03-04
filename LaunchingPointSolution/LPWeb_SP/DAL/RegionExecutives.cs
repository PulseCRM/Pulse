using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类RegionExecutives。
    /// </summary>
    public class RegionExecutives : RegionExecutivesBase
    {
        public RegionExecutives()
        { }

        /// <summary>
        /// 根据UserId 获取查询其所属的regions
        /// </summary>
        /// <param name="executiveId">userid</param>
        /// <returns></returns>
        public DataTable GetRegionsByExecutiveId(int executiveId)
        {
            if (executiveId<=0)
                return null;

            string sql = "SELECT     Regions.Name, RegionExecutives.ExecutiveId, RegionExecutives.RegionId " +
                         "FROM         RegionExecutives INNER JOIN " +
                         "  Regions ON RegionExecutives.RegionId = Regions.RegionId WHERE ExecutiveId IN ({0})";

            return DbHelperSQL.ExecuteDataTable(string.Format(sql, executiveId));
        }
    }
}

