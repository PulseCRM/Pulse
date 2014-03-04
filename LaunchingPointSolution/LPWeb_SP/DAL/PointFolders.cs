using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类PointFolders。
	/// </summary>
    public class PointFolders : PointFoldersBase
	{
		public PointFolders()
		{}
        public DataSet GetListByLoanId (int LoanId, string strWhere)
        {
            string sqlCmd = "Select PF.BranchId as F_Branchid, L.BranchId as L_BranchId from PointFolders PF inner join PointFiles F on PF.FolderId=F.FolderId inner join Loans L on F.FileId=L.FileId where F.FileId=" + LoanId;
            DataSet ds = DbHelperSQL.Query(sqlCmd);
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                sqlCmd = "Select BranchId from Loans where FileId=" + LoanId;
                ds = DbHelperSQL.Query(sqlCmd);
            }
            if (ds == null || ds.Tables[0].Rows.Count == 0)
                return GetList(strWhere);
            int BranchId = -1;
            if (ds.Tables[0].Rows[0]["F_Branchid"] != DBNull.Value)
            {
                BranchId = (int)ds.Tables[0].Rows[0]["F_Branchid"];
            }
            else if (ds.Tables[0].Rows[0]["L_BranchId"] != DBNull.Value)
            {
                BranchId = (int)ds.Tables[0].Rows[0]["L_BranchId"];
            }
            if (BranchId > 0)
                strWhere += " AND BranchId=" + BranchId;
            return GetList(strWhere);
        }

        /// <summary>
        ///  查找Loan 对应的LoanOfficerBranchID
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public string GetLoanOfficerBranchID(int iLoanID, string sType)
        {
            string sSql = "";
            DataSet ds;

            if (sType == "lead")
            {
                //查找 Prospect.LoanOfficer 是否存在，
                sSql = "select top 1 BranchID from Groups G where GroupId in (select   GroupId from GroupUsers GU where GU.UserID in (select   Loanofficer from Prospect pt where pt.Contactid in (select ContactId from LoanContacts lc where lc.FileId=" + iLoanID + " and (LC.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() OR LC.ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId())) )) ";
                ds = DbHelperSQL.Query(sSql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                return "0";
            }
            else
            {
                //Loan 查找Loan 对应的LoanOfficer
                sSql = "select BranchID from Groups G where GroupId =(select top 1 GroupId from GroupUsers GU where GU.UserID=(select top 1 UserID from LoanTeam LT where LT.RoleId=(select top 1 RoleID from Roles where Roles.Name='Loan Officer') and LT.FileId=" + iLoanID + ")) ";
                ds = DbHelperSQL.Query(sSql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                return "0";
            }
        }



        public DataTable GetPointFolder_Executive(int iUserID, string sRegionIDs, string sDivisionIDs, string sBranchIDs)
        {
            string sWhere = string.Empty;
            if (sRegionIDs != "0")
            {
                sWhere += " and b.RegionID in (" + sRegionIDs + ")";
            }

            if (sDivisionIDs != "0")
            {
                sWhere += " and b.DivisionID in (" + sDivisionIDs + ")";
            }

            if (sBranchIDs != "0")
            {
                sWhere += " and b.BranchId in (" + sBranchIDs + ") ";
            }

            string sSql = "select * from PointFolders where BranchId in (select a.BranchID from dbo.lpfn_GetUserBranches_Executive(" + iUserID + ") as a inner join Branches as b on a.BranchID = b.BranchID where 1=1 " + sWhere + ") order by Name";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetPointFolder_BranchManager(int iUserID, string sRegionIDs, string sDivisionIDs, string sBranchIDs)
        {
            string sWhere = string.Empty;
            if (sRegionIDs != "0")
            {
                sWhere += " and b.RegionID in (" + sRegionIDs + ")";
            }

            if (sDivisionIDs != "0")
            {
                sWhere += " and b.DivisionID in (" + sDivisionIDs + ")";
            }

            if (sBranchIDs != "0")
            {
                sWhere += " and b.BranchId in (" + sBranchIDs + ") ";
            }

            string sSql = "select * from PointFolders where BranchId in (select a.BranchID from dbo.lpfn_GetUserBranches_Branch_Manager(" + iUserID + ") as a inner join Branches as b on a.BranchID = b.BranchID where 1=1 " + sWhere + ") order by Name";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetPointFolder_User(int iUserID, string sRegionIDs, string sDivisionIDs, string sBranchIDs)
        {
            string sWhere = string.Empty;
            if (sRegionIDs != "0")
            {
                sWhere += " and b.RegionID in (" + sRegionIDs + ")";
            }

            if (sDivisionIDs != "0")
            {
                sWhere += " and b.DivisionID in (" + sDivisionIDs + ")";
            }

            if (sBranchIDs != "0")
            {
                sWhere += " and b.BranchId in (" + sBranchIDs + ") ";
            }

            string sSql = "select * from PointFolders where BranchId in (select a.BranchID from dbo.lpfn_GetUserBranches(" + iUserID + ") as a inner join Branches as b on a.BranchID = b.BranchID where 1=1 " + sWhere + ") order by Name";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
          

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.PointFolders GePonitFolderModel(int FolderId)
        {
             
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 PointFolders.FolderId,PointFolders.Name,PointFolders.BranchId,PointFolders.Path,PointFolders.Enabled,PointFolders.ImportCount,PointFolders.LastImport,PointFolders.LoanStatus, Branches.Name AS BranchName,PointFolders.AutoNaming,PointFolders.FilenamePrefix,RandomFileNaming,FilenameLength from PointFolders LEFT OUTER JOIN Branches ON PointFolders.BranchId = Branches.BranchId ");
            strSql.Append(" where PointFolders.FolderId=@FolderId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FolderId", SqlDbType.Int,4)};
            parameters[0].Value = FolderId;

            LPWeb.Model.PointFolders model = new LPWeb.Model.PointFolders();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FolderId"].ToString() != "")
                {
                    model.FolderId = int.Parse(ds.Tables[0].Rows[0]["FolderId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                if (ds.Tables[0].Rows[0]["BranchId"].ToString() != "")
                {
                    model.BranchId = int.Parse(ds.Tables[0].Rows[0]["BranchId"].ToString());
                }
                model.BranchName = ds.Tables[0].Rows[0]["BranchName"].ToString();
                model.Path = ds.Tables[0].Rows[0]["Path"].ToString();
                if (ds.Tables[0].Rows[0]["Enabled"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Enabled"].ToString() == "1") || (ds.Tables[0].Rows[0]["Enabled"].ToString().ToLower() == "true"))
                    {
                        model.Enabled = true;
                    }
                    else
                    {
                        model.Enabled = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ImportCount"].ToString() != "")
                {
                    model.ImportCount = int.Parse(ds.Tables[0].Rows[0]["ImportCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LastImport"].ToString() != "")
                {
                    model.LastImport = DateTime.Parse(ds.Tables[0].Rows[0]["LastImport"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LoanStatus"].ToString() != "")
                {
                    model.LoanStatus = int.Parse(ds.Tables[0].Rows[0]["LoanStatus"].ToString());
                }
                model.AutoNaming = false;
                if (ds.Tables[0].Rows[0]["AutoNaming"] != DBNull.Value && ds.Tables[0].Rows[0]["AutoNaming"].ToString() != "")
                {
                    model.AutoNaming = Convert.ToBoolean(ds.Tables[0].Rows[0]["AutoNaming"]);
                }

                if (ds.Tables[0].Rows[0]["FilenamePrefix"] != DBNull.Value && ds.Tables[0].Rows[0]["FilenamePrefix"].ToString() != "")
                {
                    model.PreFix = ds.Tables[0].Rows[0]["FilenamePrefix"].ToString();
                }

                model.RandomFileNaming = false;
                if (ds.Tables[0].Rows[0]["RandomFileNaming"] != DBNull.Value && ds.Tables[0].Rows[0]["RandomFileNaming"].ToString() != "")
                {
                    model.RandomFileNaming = Convert.ToBoolean(ds.Tables[0].Rows[0]["RandomFileNaming"]);
                }

                if (ds.Tables[0].Rows[0]["FilenameLength"] != DBNull.Value && ds.Tables[0].Rows[0]["FilenameLength"].ToString() != "")
                {
                    model.FilenameLength = int.Parse(ds.Tables[0].Rows[0]["FilenameLength"].ToString());
                }

                return model;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 更新Point Folder状态
        /// </summary>
        public void UpdatePointFolderEnabled(string sFolderIDs, bool bEnabled)
        {
            if (sFolderIDs == "")
            {
                return;
            }
            string strSql = "UPDATE PointFolders SET Enabled='" + bEnabled.ToString() + "' WHERE FolderId IN(" + sFolderIDs + ")"; 

            DbHelperSQL.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 更新Point Folder 是否自动命名
        /// </summary>
        /// <param name="FolderID"></param>
        /// <param name="AutoNaming"></param>
        /// <param name="Prefix"></param>
        public void UpdatePointFolderAutoNaming(int FolderID, bool AutoNaming, string Prefix,bool RandomFileNaming, string FilenameLength)
        {
            if (FolderID == 0)
            {
                return;
            }
             string strSql ="";
             if (string.IsNullOrEmpty(FilenameLength))
             {
                 strSql = string.Format("UPDATE PointFolders SET AutoNaming='{0}',FilenamePrefix='{1}',RandomFileNaming='{3}',FilenameLength={4} WHERE FolderId ={2}", AutoNaming.ToString(), Prefix, FolderID, false,"Null");

             }
             else
             {
                  strSql = string.Format("UPDATE PointFolders SET AutoNaming='{0}',FilenamePrefix='{1}',RandomFileNaming='{3}',FilenameLength='{4}' WHERE FolderId ={2}", AutoNaming.ToString(), Prefix, FolderID, RandomFileNaming, FilenameLength);
             }

            DbHelperSQL.ExecuteSql(strSql);
        }

        /// <summary>
        /// 设置默认Point
        /// </summary>
        /// <param name="sFolderIDs"></param>
        /// <param name="BranchId"></param>
        public void SetDefaultPoint(string sFolderID, int BranchId, bool IsCancel)
        {
            if (sFolderID == "" || BranchId==0)
            {
                return;
            }
            string strSql = string.Empty;
            if (IsCancel)
            {
                strSql = string.Format(" UPDATE PointFolders SET [Default] = 0 Where FolderId = {0} AND BranchId ={1} ", sFolderID, BranchId.ToString());
            }
            else
            {
                strSql = string.Format(" UPDATE PointFolders SET [Default] = 0 Where BranchId ={0} AND [Default] = 1 ", BranchId.ToString());

                strSql += string.Format(" UPDATE PointFolders SET [Default] = 1 Where FolderId = {0} AND BranchId ={1} ", sFolderID, BranchId.ToString());
            }
            DbHelperSQL.ExecuteSql(strSql.ToString());
        }
	}
}

