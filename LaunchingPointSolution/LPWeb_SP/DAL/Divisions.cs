using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Divisions。
    /// </summary>
    public class Divisions : DivisionsBase
    {
        public Divisions()
        { }

        /// <summary>
        /// 保存Division和Members信息
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <param name="bEnabled"></param>
        /// <param name="sGroupDesc"></param>
        /// <param name="sOldGroupMemberIDs"></param>
        /// <param name="sGroupMemberIDs"></param>
        public void SaveDivisionAndMembersBase(int iDivisionID, bool bEnabled, string sDesc, int iGroupId, string sBranchMemberIDs, string sExectives)
        {
            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

            #region SQL for 更新Division信息
            string sSql = "UPDATE Divisions set Enabled=@Enabled, [Desc]=@Desc, [GroupID]=@GroupID where DivisionID=" + iDivisionID;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Desc", SqlDbType.NVarChar, sDesc);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@GroupID", SqlDbType.Int, iGroupId);
            SqlCmdList.Add(SqlCmd);

            #endregion

            string sSql5 = "update Branches set RegionID=null,DivisionID=null where DivisionID = " + iDivisionID.ToString();
            SqlCommand SqlCmd5 = new SqlCommand(sSql5);
            SqlCmdList.Add(SqlCmd5);

            LPWeb.Model.Divisions model = GetModel(iDivisionID);
            if (sBranchMemberIDs != string.Empty)
            {
                // 清空新添加members的ID信息
                string sSql5x = "update Branches set RegionID=null where BranchID in (" + sBranchMemberIDs + ")";
                SqlCommand SqlCmd5x = new SqlCommand(sSql5x);
                SqlCmdList.Add(SqlCmd5x);

                #region
                // 获取Group信息 
                string sRegionID = null;

                string sSql6 = string.Empty;
                if (model.RegionID != null)
                {
                    sRegionID = model.RegionID.ToString();

                    sSql6 = "update Branches set RegionID=" + sRegionID + ",DivisionID= " + iDivisionID.ToString() + " where BranchID in (" + sBranchMemberIDs + ")";
                }
                else
                {
                    sSql6 = "update Branches set RegionID=null,DivisionID= " + iDivisionID.ToString() + " where BranchID in (" + sBranchMemberIDs + ")";
                }

                SqlCommand SqlCmd6 = new SqlCommand(sSql6);
                SqlCmdList.Add(SqlCmd6);


                #endregion
            }

            #region 更新 group 表
            
            string sSql9 = "EXEC GroupRelationInfoUpdate 'Division'," + iDivisionID.ToString() + "," + iGroupId.ToString();
            SqlCommand sqlcmd9 = new SqlCommand(sSql9);
            SqlCmdList.Add(sqlcmd9);
            ////Clear all group data
            //string sSql9 = "UPDATE Groups SET RegionID=NULL,OrganizationType=NULL,DivisionID=NULL,BranchID=NULL WHERE DivisionID=" + iDivisionID.ToString();
            //SqlCommand sqlcmd9 = new SqlCommand(sSql9);
            //SqlCmdList.Add(sqlcmd9);


            ////设置新的group属于当前region DivisionID
            //string sSql10 = string.Empty;
            //string sSql11 = string.Empty;
            //if (model.GroupID.HasValue)
            //{
            //    if (model.RegionID.HasValue)
            //    {
            //        sSql10 = "UPDATE Groups SET RegionID=" + model.RegionID.ToString() + ",OrganizationType='Division',DivisionID=" + iDivisionID.ToString() + " WHERE GroupID=" + model.GroupID.ToString();
                    
            //        //Update all branch and group relation data
            //        sSql11 = "UPDATE Groups SET RegionID=" + model.RegionID.ToString() + ",OrganizationType='Branch', DivisionID=" + iDivisionID.ToString() + ", BranchID=";
            //    }
            //    else
            //    {
            //        sSql10 = "UPDATE Groups SET OrganizationType='Division',DivisionID=" + iDivisionID.ToString() + "  WHERE GroupID=" + model.GroupID.ToString();
            //    }
            //    SqlCommand sqlcmd10 = new SqlCommand(sSql10);
            //    SqlCmdList.Add(sqlcmd10);
            //}
            #endregion

            string sSql4 = "DELETE DivisionExecutives WHERE [DivisionId] = @DivisionId ";
            SqlCommand SqlCmd4 = new SqlCommand(sSql4);
            DbHelperSQL.AddSqlParameter(SqlCmd4, "@DivisionId", SqlDbType.Int, iDivisionID);
            SqlCmdList.Add(SqlCmd4);

            if (sExectives != string.Empty)
            {
                string[] managers = sExectives.Split(",".ToCharArray());
                foreach (string manager in managers)
                {
                    string sSql5x = "INSERT INTO DivisionExecutives(DivisionId,ExecutiveId) values ( @DivisionId,@ExecutiveId)";
                    SqlCommand SqlCmd5x = new SqlCommand(sSql5x);
                    DbHelperSQL.AddSqlParameter(SqlCmd5x, "@DivisionId", SqlDbType.Int, iDivisionID);
                    DbHelperSQL.AddSqlParameter(SqlCmd5x, "@ExecutiveId", SqlDbType.Int, int.Parse(manager));
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
        /// 检查Division Name是否存在
        /// </summary>
        /// <param name="sGroupName"></param>
        /// <returns></returns>
        public bool IsExist_CreateBase(string sDivName)
        {
            string sSql = "select count(1) from Divisions where upper(Name)=@DivName";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DivName", SqlDbType.NVarChar, sDivName.ToUpper());
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
        public int CreateDivision(string sDivName)
        {
            string sSql1 = "insert into Divisions (Name, Enabled) values (@DivName, 1);select SCOPE_IDENTITY();";
            SqlCommand SqlCmd1 = new SqlCommand(sSql1);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@DivName", SqlDbType.NVarChar, sDivName);
            object oNewID = DbHelperSQL.ExecuteScalar(SqlCmd1);
            int iNewID = Convert.ToInt32(oNewID);
            return iNewID;
        }

        /// <summary>
        /// 获取Division列表
        /// neo 2010-09-24
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetDivisionListBase(string sWhere)
        {
            string sSql = "select * from Divisions where 1=1 " + sWhere + " order by Name";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of division filter for "All Loans" in dashboard home
        /// neo 2010-10-19
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetDivision_AllLoansBase(int iExecutiveID)
        {
            string sSql = " SELECT [Name] ,[DivisionId] FROM( "
                        + " select Name, DivisionId  from Divisions where Enabled=1 and exists( "
                        + "     select * from Groups as a inner join GroupUsers as b on a.GroupId = b.GroupID where OrganizationType='Company' "
                        + "     and b.UserID = " + iExecutiveID + ") "
                        + "union "
                        + "select b.Name, b.DivisionId from DivisionExecutives as a left outer join Divisions as b on a.DivisionId = b.DivisionId "
                        + "where b.Enabled=1 and ExecutiveId=" + iExecutiveID + " "
                        + "union "
                        + "select b.Name, b.DivisionId from GroupUsers as a left outer join Divisions as b on a.GroupID = b.GroupID "
                        + "where b.Enabled=1 and a.UserID=" + iExecutiveID + " "
                        + "union "
                        + "select Name, DivisionId from Divisions where RegionID in ( "
                        + "     select b.RegionId from RegionExecutives as a left outer join Regions as b on a.RegionId = b.RegionId "
                        + "     where b.Enabled=1 and ExecutiveId=" + iExecutiveID + " "
                        + "     union "
                        + "     select b.RegionId from GroupUsers as a left outer join Regions as b on a.GroupID = b.GroupID "
                        + "     where b.Enabled=1 and a.UserID=" + iExecutiveID + " "
                        + ")"
                        + "   ) tmptb order by Name ";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of division filter for "All Loans" in dashboard home
        /// neo 2010-10-19
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetDivision_AllLoansBase(int iExecutiveID, int iRegionID)
        {
            string sSql = " SELECT [Name] ,[DivisionId] FROM( "
                        + " select Name, DivisionId from Divisions where Enabled=1 and exists( "
                        + "     select * from Groups as a inner join GroupUsers as b on a.GroupId = b.GroupID where OrganizationType='Company' "
                        + "     and b.UserID = " + iExecutiveID + ") and RegionID = " + iRegionID
                        + "union "
                        + "select b.Name, b.DivisionId from DivisionExecutives as a left outer join Divisions as b on a.DivisionId = b.DivisionId "
                        + "where b.Enabled=1 and ExecutiveId=" + iExecutiveID + " and b.RegionID = " + iRegionID + " "
                        + "union "
                        + "select b.Name, b.DivisionId from GroupUsers as a left outer join Divisions as b on a.GroupID = b.GroupID "
                        + "where b.Enabled=1 and a.UserID=" + iExecutiveID + " and b.RegionID = " + iRegionID + " "
                        + "union "
                        + "select Name, DivisionId from Divisions where RegionID in ( "
                        + "     select b.RegionId from RegionExecutives as a left outer join Regions as b on a.RegionId = b.RegionId "
                        + "     where b.Enabled=1 and ExecutiveId=" + iExecutiveID + " "
                        + "     union "
                        + "     select b.RegionId from GroupUsers as a left outer join Regions as b on a.GroupID = b.GroupID "
                        + "     where b.Enabled=1 and a.UserID=" + iExecutiveID + " "
                        + ") and RegionID = " + iRegionID
                        + "   ) tmptb order by Name ";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of division filter for "Assigned Loans" in dashboard home
        /// neo 2010-10-24
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetDivisionList_AssingedLoansBase(int iUserID)
        {
            //string sSql = " SELECT [Name] ,[DivisionId] FROM( "
            //            + " select DivisionId, Name from Divisions where DivisionId in ( "
            //            + "	    select distinct DivisionId from LoanTeam as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
            //            + "     inner join Branches as d on c.BranchId = d.BranchId "
            //            + "     where UserId=" + iUserID + " "
            //            + "     union "
            //            + "     select distinct DivisionId from LoanTasks as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
            //            + "     inner join Branches as d on c.BranchId = d.BranchId where Owner=" + iUserID + " "
            //            + ")"
            //            + "   ) tmptb order by Name ";
            string sSql = string.Format("SELECT [Name], [DivisionId] from dbo.[lpfn_GetUserDivisions]({0})", iUserID);
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of division filter for "Assigned Loans" in dashboard home
        /// neo 2010-10-24
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetDivisionList_AssingedLoansBase(int iUserID, int iRegionID)
        {
            string sSql = " SELECT [Name] ,[DivisionId] FROM( "
                        + " select DivisionId, Name from Divisions where DivisionId in ( "
                        + "	    select distinct DivisionId from LoanTeam as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
                        + "     inner join Branches as d on c.BranchId = d.BranchId "
                        + "     where UserId=" + iUserID + "  and RegionID = " + iRegionID + " "
                        + "     union "
                        + "     select distinct DivisionId from LoanTasks as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
                        + "     inner join Branches as d on c.BranchId = d.BranchId where Owner=" + iUserID + "  and RegionID=" + iRegionID + " "
                        + ")"
                        + "   ) tmptb order by Name ";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of division filter
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetDivisionFilterBase(int iUserID, int iRegionID)
        {
            string sWhere = string.Empty;
            if (iRegionID != 0)
            {
                sWhere = " and b.RegionID = " + iRegionID;
            }

            string sSql = "select a.DivisionID, b.Name from dbo.lpfn_GetUserDivisions(" + iUserID + ") as a inner join Divisions as b on a.DivisionID = b.DivisionId "
                        + "where (1=1)" + sWhere + " order by b.Name";

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetDivisionFilterBase_Branch_Manager(int iUserID, int iRegionID)
        {
            string sWhere = string.Empty;
            if (iRegionID != 0)
            {
                sWhere = " and b.RegionID = " + iRegionID;
            }

            string sSql = "select a.DivisionID, b.Name from dbo.lpfn_GetUserDivisions_Branch_Manager(" + iUserID + ") as a inner join Divisions as b on a.DivisionID = b.DivisionId "
                        + "where (1=1)" + sWhere + " order by b.Name";

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetDivisionFilterBase_Executive(int iUserID, int iRegionID)
        {
            string sWhere = string.Empty;
            if (iRegionID != 0)
            {
                sWhere = " and b.RegionID = " + iRegionID;
            }

            string sSql = "select a.DivisionID, b.Name from dbo.lpfn_GetUserDivisions_Executive(" + iUserID + ") as a inner join Divisions as b on a.DivisionID = b.DivisionId "
                        + "where (1=1)" + sWhere + " order by b.Name";

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetDivisionFilterBase(int iUserID, string sRegionIDs)
        {
            string sWhere = string.Empty;
            if (sRegionIDs != "0")
            {
                sWhere = " and b.RegionID in (" + sRegionIDs + ") ";
            }

            string sSql = "select a.DivisionID, b.Name from dbo.lpfn_GetUserDivisions(" + iUserID + ") as a inner join Divisions as b on a.DivisionID = b.DivisionId "
                        + "where (1=1)" + sWhere + " order by b.Name";

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetDivisionFilterBase_Branch_Manager(int iUserID, string sRegionIDs)
        {
            string sWhere = string.Empty;
            if (sRegionIDs != "0")
            {
                sWhere = " and b.RegionID in (" + sRegionIDs + ") ";
            }

            string sSql = "select a.DivisionID, b.Name from dbo.lpfn_GetUserDivisions_Branch_Manager(" + iUserID + ") as a inner join Divisions as b on a.DivisionID = b.DivisionId "
                        + "where (1=1)" + sWhere + " order by b.Name";

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetDivisionFilterBase_Executive(int iUserID, string sRegionIDs)
        {
            string sWhere = string.Empty;
            if (sRegionIDs != "0")
            {
                sWhere = " and b.RegionID in (" + sRegionIDs + ") ";
            }

            string sSql = "select a.DivisionID, b.Name from dbo.lpfn_GetUserDivisions_Executive(" + iUserID + ") as a inner join Divisions as b on a.DivisionID = b.DivisionId "
                        + "where (1=1)" + sWhere + " order by b.Name";

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of division filter for user list view
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetDivisionFilterBase_UserList(int iUserID, int iRegionID)
        {
            string sWhere = string.Empty;
            if (iRegionID != 0)
            {
                sWhere = " and b.RegionID = " + iRegionID;
            }

            string sSql = "select a.DivisionID, b.Name from dbo.lpfn_GetUserDivisions_UserList(" + iUserID + ") as a inner join Divisions as b on a.DivisionID = b.DivisionId "
                        + "where (1=1)" + sWhere;

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataSet GetDivisionGoalsReport(int PageSize, int PageIndex, string strRegion, string strDivision, out int count, string orderName, int orderType)
        {
            string tempTable =
                string.Format(
                    " (SELECT * FROM [dbo].[lpfn_GetDivisionGoalsReport]({0}, {1})) as DivisionGoalsReport ", strRegion, strDivision);
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

        public DataTable GetUserDivisions(int iUserID)
        {
            string sSql = "select * from dbo.lpfn_GetUserDivisions(" + iUserID + ")";

            return DbHelperSQL.ExecuteDataTable(sSql);
        }
    }
}

