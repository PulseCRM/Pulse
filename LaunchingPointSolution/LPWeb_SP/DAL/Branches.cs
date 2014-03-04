using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Branches。
    /// </summary>
    public class Branches : BranchesBase
    {
        public Branches()
        { }
        /// <summary>
        /// 获取具有branc manager 权限的 User列表
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetBranchManagerSeletion()
        {
            string sSql = @"SELECT Users.UserId, Users.Username,Users.LastName+', '+ Users.FirstName as FullName FROM 
 Users WHERE RoleID='2' order by LastName";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 检查Division Name是否存在
        /// </summary>
        /// <param name="sGroupName"></param>
        /// <returns></returns>
        public bool IsExist_CreateBase(string sBranchName)
        {
            string sSql = "select count(1) from Branches where upper(Name)=@sBranchName";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@sBranchName", SqlDbType.NVarChar, sBranchName.ToUpper());
            int iCount = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd));
            if (iCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Create Division
        /// </summary>
        /// <param name="sGroupName"></param>
        /// <returns></returns>
        public int CreateBranch(string sBranchName)
        {
            string sSql1 = "insert into Branches (Name, Enabled) values (@BranchName, 1);select SCOPE_IDENTITY();";
            SqlCommand SqlCmd1 = new SqlCommand(sSql1);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@BranchName", SqlDbType.NVarChar, sBranchName);
            object oNewID = DbHelperSQL.ExecuteScalar(SqlCmd1);
            int iNewID = Convert.ToInt32(oNewID);
            return iNewID;
        }

        /// <summary>
        /// 保存Branch和Members信息
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <param name="bEnabled"></param>
        /// <param name="sGroupDesc"></param>
        /// <param name="sOldGroupMemberIDs"></param>
        /// <param name="sGroupMemberIDs"></param>
        public void SaveBranchAndMembersBase(LPWeb.Model.Branches model, string sFolderIDs, string sManagers)
        {
            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

            #region SQL for 更新Branches信息
            string sSql = string.Empty;
            if (model.WebsiteLogo != null)
            {
                sSql = " UPDATE [Branches] SET [Name] = @Name,[Desc] = @Desc,[WebsiteLogo] = @WebsiteLogo,[Enabled] = @Enabled, [GroupID] = @GroupID, [BranchAddress] = @BranchAddress,[City] = @City,[BranchState] = @BranchState,[Zip] = @Zip,License1=@License1,License2=@License2,License3=@License3,License4=@License4,License5=@License5,Disclaimer=@Disclaimer,Phone=@Phone,Fax=@Fax,Email=@Email,WebURL=@WebURL,HomeBranch=@HomeBranch WHERE BranchId=@BranchId ";
            }
            else
            {
                sSql = " UPDATE [Branches] SET [Name] = @Name,[Desc] = @Desc,[Enabled] = @Enabled , [GroupID] = @GroupID, [BranchAddress] = @BranchAddress,[City] = @City,[BranchState] = @BranchState,[Zip] = @Zip,License1=@License1,License2=@License2,License3=@License3,License4=@License4,License5=@License5,Disclaimer=@Disclaimer,Phone=@Phone,Fax=@Fax,Email=@Email,WebURL=@WebURL,HomeBranch=@HomeBranch WHERE BranchId=@BranchId ";
            }

            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, model.Name);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Desc", SqlDbType.NVarChar, model.Desc);
            if (model.WebsiteLogo != null)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@WebsiteLogo", SqlDbType.VarBinary, model.WebsiteLogo);
            }
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, model.Enabled);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@GroupID", SqlDbType.Int, model.GroupID);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@BranchAddress", SqlDbType.NVarChar, model.BranchAddress);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@City", SqlDbType.NVarChar, model.City);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@BranchState", SqlDbType.NVarChar, model.BranchState);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Zip", SqlDbType.NVarChar, model.Zip);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@BranchId", SqlDbType.Int, model.BranchId);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@License1", SqlDbType.NVarChar, model.License1);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@License2", SqlDbType.NVarChar, model.License2);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@License3", SqlDbType.NVarChar, model.License3);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@License4", SqlDbType.NVarChar, model.License4);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@License5", SqlDbType.NVarChar, model.License5);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Disclaimer", SqlDbType.NVarChar, model.Disclaimer);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Phone", SqlDbType.NVarChar, model.Phone);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Fax", SqlDbType.NVarChar, model.Fax);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Email", SqlDbType.NVarChar, model.Email);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@WebURL", SqlDbType.NVarChar, model.WebURL);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@HomeBranch", SqlDbType.Bit, model.HomeBranch);
            SqlCmdList.Add(SqlCmd);

            #endregion

            string sSql5 = " UPDATE [PointFolders] SET [BranchId] = null WHERE [BranchId]=@BranchId ";
            SqlCommand SqlCmd5 = new SqlCommand(sSql5);
            DbHelperSQL.AddSqlParameter(SqlCmd5, "@BranchId", SqlDbType.Int, model.BranchId);
            SqlCmdList.Add(SqlCmd5);

            string sSql2 = " UPDATE [PointFolders] SET [BranchId] = null WHERE [BranchId]=@BranchId ";
            SqlCommand SqlCmd2 = new SqlCommand(sSql2);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@BranchId", SqlDbType.Int, model.BranchId);
            SqlCmdList.Add(SqlCmd2);

            if (sFolderIDs != string.Empty)
            {
                string[] folders = sFolderIDs.Split("|".ToCharArray());
                foreach (string folder in folders)
                {
                    string[] folderstatus = folder.Split(",".ToCharArray());
                    if (folderstatus.Length == 2)
                    {
                        string sSql3 = " UPDATE [PointFolders] SET [BranchId] = @BranchId,LoanStatus=@LoanStatus WHERE FolderId = @FolderId";
                        SqlCommand SqlCmd3 = new SqlCommand(sSql3);
                        DbHelperSQL.AddSqlParameter(SqlCmd3, "@BranchId", SqlDbType.Int, model.BranchId);
                        DbHelperSQL.AddSqlParameter(SqlCmd3, "@LoanStatus", SqlDbType.Int, int.Parse(folderstatus[1]));
                        DbHelperSQL.AddSqlParameter(SqlCmd3, "@FolderId", SqlDbType.Int, int.Parse(folderstatus[0]));
                        SqlCmdList.Add(SqlCmd3);
                    }
                }

            }

            #region 更新 group 表
            string sSql9 = "EXEC GroupRelationInfoUpdate 'Branch'," + model.BranchId.ToString() + "," + model.GroupID.ToString();
            SqlCommand sqlcmd9 = new SqlCommand(sSql9);
            SqlCmdList.Add(sqlcmd9);
            //LPWeb.Model.Branches model1 = GetModel(model.BranchId);
            //string sSql9 = "UPDATE Groups SET RegionID=NULL,OrganizationType=NULL,DivisionID=NULL,BranchID=NULL WHERE BranchID=" + model.BranchId.ToString();
            //SqlCommand sqlcmd9 = new SqlCommand(sSql9);
            //SqlCmdList.Add(sqlcmd9);

            ////设置新的group属于当前region Division branch
            //string sSql10 = string.Empty;
            //if (model.GroupID.HasValue)
            //{
            //    if (model1.RegionID.HasValue && model1.DivisionID.HasValue)
            //    {
            //        sSql10 = "UPDATE Groups SET RegionID=" + model1.RegionID.ToString() + ",OrganizationType='Branch',DivisionID=" + model1.DivisionID.ToString() + ",BranchID=" + model.BranchId.ToString() + " WHERE GroupID=" + model.GroupID.ToString();
            //    }
            //    else if (model1.RegionID.HasValue && !model1.DivisionID.HasValue)
            //    {
            //        sSql10 = "UPDATE Groups SET RegionID=" + model1.RegionID.ToString() + ",OrganizationType='Branch', BranchID=" + model.BranchId.ToString() + " WHERE GroupID=" + model.GroupID.ToString();
            //    }
            //    else if (!model1.RegionID.HasValue && model1.DivisionID.HasValue)
            //    {
            //        sSql10 = "UPDATE Groups SET OrganizationType='Branch',DivisionID=" + model1.DivisionID.ToString() + ",BranchID=" + model.BranchId.ToString() + " WHERE GroupID=" + model.GroupID.ToString();
            //    }
            //    else
            //    {
            //        sSql10 = "UPDATE Groups SET OrganizationType='Branch' ,BranchID=" + model.BranchId.ToString() + " WHERE GroupID=" + model.GroupID.ToString();
            //    }
            //    SqlCommand sqlcmd10 = new SqlCommand(sSql10);
            //    SqlCmdList.Add(sqlcmd10);
            //}
            #endregion

            string sSql4 = "DELETE BranchManagers WHERE [BranchId] = @BranchId ";
            SqlCommand SqlCmd4 = new SqlCommand(sSql4);
            DbHelperSQL.AddSqlParameter(SqlCmd4, "@BranchId", SqlDbType.Int, model.BranchId);
            SqlCmdList.Add(SqlCmd4);

            if (sManagers != string.Empty)
            {
                string[] managers = sManagers.Split(",".ToCharArray());
                foreach (string manager in managers)
                {
                    string sSql5x = "INSERT INTO [BranchManagers]([BranchId],[BranchMgrId])VALUES(@BranchId,@BranchMgrId )";
                    SqlCommand SqlCmd5x = new SqlCommand(sSql5x);
                    DbHelperSQL.AddSqlParameter(SqlCmd5x, "@BranchId", SqlDbType.Int, model.BranchId);
                    DbHelperSQL.AddSqlParameter(SqlCmd5x, "@BranchMgrId", SqlDbType.Int, int.Parse(manager));
                    SqlCmdList.Add(SqlCmd5x);
                }

            }

            #region 批量执行SQL

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;
            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                foreach (SqlCommand xSqlCmd in SqlCmdList)
                {
                    DbHelperSQL.ExecuteNonQuery(xSqlCmd, SqlTrans);
                }

                SqlTrans.Commit();
            }
            catch (Exception ex)
            {
                SqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                if (SqlConn != null)
                {
                    SqlConn.Close();
                }
            }

            #endregion

        }

        /// <summary>
        /// 获取Branch列表
        /// neo 2010-09-24
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetBranchListBase(string sWhere)
        {
            string sSql = "select * from Branches where 1=1 " + sWhere + " order by Name";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of branch filter for "All Loans" in dashboard home
        /// neo 2010-10-24
        /// </summary>
        /// <param name="iManagerID"></param>
        /// <param name="iRegionID"></param>
        /// <param name="iDivisionID"></param>
        /// <returns></returns>
        public DataTable GetBranchList_AllLoansBase(int iManagerID, int iRegionID, int iDivisionID)
        {
            string sWhere1 = string.Empty;
            string sWhere2 = string.Empty;
            if (iRegionID != 0)
            {
                sWhere1 += " and b.RegionID=" + iRegionID + " ";
                sWhere2 += " and RegionID=" + iRegionID + " ";
            }

            if (iDivisionID != 0)
            {
                sWhere1 += " and b.DivisionID=" + iDivisionID + " ";
                sWhere2 += " and DivisionID=" + iDivisionID + " ";
            }

            string sSql = " SELECT [Name] ,[BranchId] FROM( "
                        + " select Name,  BranchId from Branches where Enabled=1 and exists( "
                        + "     select * from Groups as a inner join GroupUsers as b on a.GroupId = b.GroupID where OrganizationType='Company' "
                        + "     and b.UserID = " + iManagerID + ")" + sWhere2
                        + "union "
                        + "select b.Name, b.BranchId from BranchManagers as a left outer join Branches as b on a.BranchId = b.BranchId "
                        + "where b.Enabled=1 and BranchMgrId=" + iManagerID + sWhere1
                        + "union "
                        + "select  b.Name,b.BranchId from GroupUsers as a left outer join Branches as b on a.GroupID = b.GroupID "
                        + "where b.Enabled=1 and a.UserID=" + iManagerID + sWhere1
                        + "union "
                        + "select Name, BranchId from Branches where ((RegionID in ( "
                        + "    select b.RegionId from RegionExecutives as a left outer join Regions as b on a.RegionId = b.RegionId "
                        + "    where b.Enabled=1 and ExecutiveId=" + iManagerID + " "
                        + "    union "
                        + "    select b.RegionId from GroupUsers as a left outer join Regions as b on a.GroupID = b.GroupID "
                        + "    where b.Enabled=1 and a.UserID=" + iManagerID + " "
                        + ")) "
                        + "or (DivisionID in ( "
                        + "    select b.DivisionId from DivisionExecutives as a left outer join Divisions as b on a.DivisionId = b.DivisionId "
                        + "    where b.Enabled=1 and ExecutiveId=" + iManagerID + " "
                        + "    union "
                        + "    select b.DivisionId from GroupUsers as a left outer join Divisions as b on a.GroupID = b.GroupID "
                        + "    where b.Enabled=1 and a.UserID=" + iManagerID + " "
                        + "    union "
                        + "    select DivisionId from Divisions where RegionID in ( "
                        + "        select b.RegionId from RegionExecutives as a left outer join Regions as b on a.RegionId = b.RegionId "
                        + "        where b.Enabled=1 and ExecutiveId=" + iManagerID + " "
                        + "        union "
                        + "        select b.RegionId from GroupUsers as a left outer join Regions as b on a.GroupID = b.GroupID "
                        + "        where b.Enabled=1 and a.UserID=" + iManagerID + " "
                        + "    ) "
                        + ")))" + sWhere2
                        + "   ) tmptb order by Name ";

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of branch filter for "Assigned Loans" in dashboard home
        /// neo 2010-10-24
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetBranchList_AssingedLoansBase(int iUserID)
        {
            //string sSql = " SELECT [Name] ,[BranchId] FROM( "
            //            + " select distinct d.Name, d.BranchId from LoanTeam as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
            //            + "inner join Branches as d on c.BranchId = d.BranchId "
            //            + "where UserId=" + iUserID + " "
            //            + "union "
            //            + "select distinct d.Name, d.BranchId from LoanTasks as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
            //            + "inner join Branches as d on c.BranchId = d.BranchId where Owner=" + iUserID
            //            + "   ) tmptb order by Name ";
            string sSql = string.Format("Select [Name], [BranchId] from Branches where BranchId in (select BranchId from dbo.[lpfn_GetUserBranches]({0})) order by [Name]", iUserID);
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of branch filter for "Assigned Loans" in dashboard home
        /// neo 2010-10-24
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="iRegionID"></param>
        /// <param name="iDivisionID"></param>
        /// <returns></returns>
        public DataTable GetBranchList_AssingedLoansBase(int iUserID, int iRegionID, int iDivisionID)
        {
            string sWhere1 = string.Empty;
            if (iRegionID != 0)
            {
                sWhere1 += " and d.RegionID=" + iRegionID;
            }

            if (iDivisionID != 0)
            {
                sWhere1 += " and d.DivisionID = " + iDivisionID;
            }
            string sSql = " SELECT [Name] ,[BranchId] FROM( "
                        + " select distinct d.Name, d.BranchId from LoanTeam as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
                        + "inner join Branches as d on c.BranchId = d.BranchId "
                        + "where UserId=" + iUserID + sWhere1 + " "
                        + "union "
                        + "select distinct d.Name , d.BranchId from LoanTasks as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
                        + "inner join Branches as d on c.BranchId = d.BranchId where Owner=" + iUserID + sWhere1
                        + "   ) tmptb order by Name ";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of branch filter
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="iRegionID"></param>
        /// <param name="iDivisionID"></param>
        /// <returns></returns>
        public DataTable GetBranchFilterBase(int iUserID, int iRegionID, int iDivisionID)
        {
            string sWhere = string.Empty;
            if (iRegionID != 0)
            {
                sWhere += " and a.RegionID = " + iRegionID;
            }

            if (iDivisionID != 0)
            {
                sWhere += " and a.DivisionID = " + iDivisionID;
            }

            //string sSql = "select a.BranchID, a.[Name] from dbo.lpfn_GetUserBranches(" + iUserID + ") as a inner join Branches as b on a.BranchID = b.BranchID "
            string sSql = string.Format("select a.BranchID, a.[Name] from Branches a where a.BranchId in (select BranchId from dbo.[lpfn_GetUserBranches]({0})) ", iUserID)
            + " AND (1=1)" + sWhere + " Order By a.[Name] ";  //gdc 20120506 Bug #1664
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetBranchFilterBase_Branch_Manager(int iUserID, int iRegionID, int iDivisionID)
        {
            string sWhere = string.Empty;
            if (iRegionID != 0)
            {
                sWhere += " and b.RegionID = " + iRegionID;
            }

            if (iDivisionID != 0)
            {
                sWhere += " and b.DivisionID = " + iDivisionID;
            }

            string sSql = "select a.BranchID, b.Name from dbo.lpfn_GetUserBranches_Branch_Manager(" + iUserID + ") as a inner join Branches as b on a.BranchID = b.BranchID "
                        + "where (1=1)" + sWhere + " Order By b.Name ";  //gdc 20120506 Bug #1664
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetBranchFilterBase_Executive(int iUserID, int iRegionID, int iDivisionID)
        {
            string sWhere = string.Empty;
            if (iRegionID != 0)
            {
                sWhere += " and b.RegionID = " + iRegionID;
            }

            if (iDivisionID != 0)
            {
                sWhere += " and b.DivisionID = " + iDivisionID;
            }

            string sSql = "select a.BranchID, b.Name from dbo.lpfn_GetUserBranches_Executive(" + iUserID + ") as a inner join Branches as b on a.BranchID = b.BranchID "
                        + "where (1=1)" + sWhere + " Order By b.Name ";  //gdc 20120506 Bug #1664
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetBranchFilterBase(int iUserID, string sRegionIDs, string sDivisionIDs)
        {
            string sWhere = string.Empty;
            if (sRegionIDs != "0")
            {
                sWhere += " and a.RegionID in (" + sRegionIDs + ")";
            }

            if (sDivisionIDs != "0")
            {
                sWhere += " and a.DivisionID in (" + sDivisionIDs + ")";
            }

            //string sSql = "select a.BranchID, a.[Name] from dbo.lpfn_GetUserBranches(" + iUserID + ") as a inner join Branches as b on a.BranchID = b.BranchID "
            string sSql = string.Format("select a.BranchID, a.[Name] from Branches a where a.BranchId in (select BranchId from dbo.[lpfn_GetUserBranches]({0})) ", iUserID)
                        + " AND (1=1)" + sWhere + " Order By a.[Name] ";  //gdc 20120506 Bug #1664
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetBranchFilterBase_Branch_Manager(int iUserID, string sRegionIDs, string sDivisionIDs)
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

            string sSql = "select a.BranchID, b.Name from dbo.lpfn_GetUserBranches_Branch_Manager(" + iUserID + ") as a inner join Branches as b on a.BranchID = b.BranchID "
                        + "where (1=1)" + sWhere + " Order By b.Name ";  //gdc 20120506 Bug #1664
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetBranchFilterBase_Executive(int iUserID, string sRegionIDs, string sDivisionIDs)
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

            string sSql = "select a.BranchID, b.Name from dbo.lpfn_GetUserBranches_Executive(" + iUserID + ") as a inner join Branches as b on a.BranchID = b.BranchID "
                        + "where (1=1)" + sWhere + " Order By b.Name ";  //gdc 20120506 Bug #1664
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of branch filter for user list view
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="iRegionID"></param>
        /// <param name="iDivisionID"></param>
        /// <returns></returns>
        public DataTable GetBranchFilterBase_UserList(int iUserID, int iRegionID, int iDivisionID)
        {
            string sWhere = string.Empty;
            if (iRegionID != 0)
            {
                sWhere = " and b.RegionID = " + iRegionID;
            }

            if (iDivisionID != 0)
            {
                sWhere = " and b.DivisionID = " + iDivisionID;
            }

            string sSql = "select a.BranchID, b.Name from dbo.lpfn_GetUserBranches_UserList(" + iUserID + ") as a inner join Branches as b on a.BranchID = b.BranchID "
                        + "where (1=1)" + sWhere;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataSet GetBranchGoalsReport(int PageSize, int PageIndex, string strRegion, string strDivision, string strBranch,
            out int count, string orderName, int orderType)
        {
            string tempTable =
                string.Format(
                    " (SELECT * FROM [dbo].[lpfn_GetBranchGoalsReport]({0}, {1}, {2})) as BranchGoalsReport ", strRegion, strDivision, strBranch);
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "";
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }


        /// <summary>
        /// UpdateChimpAPIKey
        /// </summary>
        public void UpdateChimpAPIKey(LPWeb.Model.Branches model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update Branches set ");
            strSql.Append(" EnableMailChimp=@EnableMailChimp,");
            strSql.Append(" MailChimpAPIKey=@MailChimpAPIKey ");
            strSql.Append(" where BranchId=@BranchId ");
            SqlParameter[] parameters = {
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					new SqlParameter("@EnableMailChimp", SqlDbType.Bit),
					new SqlParameter("@MailChimpAPIKey", SqlDbType.NVarChar,255) };

            parameters[0].Value = model.BranchId;
            parameters[1].Value = model.EnableMailChimp;
            parameters[2].Value = model.MailChimpAPIKey;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        public DataTable GetUserBranches(int iUserID)
        {

            string sql = "select ub.BranchID,b.Name from  lpfn_GetUserBranches(" + iUserID + ") ub left join Branches b on b.BranchID = ub.BranchID";

            return DbHelperSQL.ExecuteDataTable(sql);
        }

    }
}

