using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类BranchManagers。
    /// </summary>
    public class BranchManagers : BranchManagersBase
    {
        public BranchManagers()
        { }

        /// <summary>
        /// 根据UserId 获取查询其所属的Branch
        /// </summary>
        /// <param name="mgrId">userid</param>
        /// <returns></returns>
        public DataTable GetBranchesByBranchMgrId(int mgrId)
        {
            if (mgrId <= 0)
                return null;

            string sql = "SELECT Branches.Name, BranchManagers.BranchId, BranchManagers.BranchMgrId " +
"FROM         Branches INNER JOIN " +
                      "BranchManagers ON Branches.BranchId = BranchManagers.BranchId WHERE BranchMgrId IN ({0})";

            return DbHelperSQL.ExecuteDataTable(string.Format(sql, mgrId));
        }
    }
}

