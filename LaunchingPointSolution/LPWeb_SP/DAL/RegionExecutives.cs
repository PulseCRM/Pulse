using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// ���ݷ�����RegionExecutives��
    /// </summary>
    public class RegionExecutives : RegionExecutivesBase
    {
        public RegionExecutives()
        { }

        /// <summary>
        /// ����UserId ��ȡ��ѯ��������regions
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

