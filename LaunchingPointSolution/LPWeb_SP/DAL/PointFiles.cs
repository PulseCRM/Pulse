using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类PointFiles。
	/// </summary>
    public class PointFiles : PointFilesBase
	{
		public PointFiles()
		{}
        public int GetPointFileBrancId(string strWhere)
        {
            int BranchId = 0;
            string sqlCmd = "select BranchId from lpvw_GetPointFileInfo";
            if (strWhere.Length > 0)
                sqlCmd += " where " + strWhere;
            DataSet ds = DbHelperSQL.Query(sqlCmd);
            if (ds == null || ds.Tables[0].Rows.Count <= 0)
                return BranchId;

            BranchId = (ds.Tables[0].Rows[0][0] == DBNull.Value) ? 0 : (int)(ds.Tables[0].Rows[0][0]);
            return BranchId;
        }
	}
}

