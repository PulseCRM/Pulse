using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Regions。
    /// </summary>
    public class Regions : RegionsBase
    {
        public Regions()
        {

        }

        /// <summary>
        /// 检查Region Name是否存在
        /// </summary>
        /// <param name="sRegionName"></param>
        /// <returns></returns>
        public bool IsExist_CreateBase(string sRegionName)
        {
            string sSql = "select count(1) from Regions where upper(Name)=@DivName";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DivName", SqlDbType.NVarChar, sRegionName.ToUpper());
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
        /// 创建Region
        /// </summary>
        /// <param name="sRegionName"></param>
        /// <returns></returns>
        public int CreateRegion(string sRegionName)
        {
            string sSql1 = "insert into Regions (Name, Enabled) values (@DivName, 1);select SCOPE_IDENTITY();";
            SqlCommand SqlCmd1 = new SqlCommand(sSql1);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@DivName", SqlDbType.NVarChar, sRegionName);
            object oNewID = DbHelperSQL.ExecuteScalar(SqlCmd1);
            int iNewID = Convert.ToInt32(oNewID);
            return iNewID;
        }

        public void SaveRegionAndMembersBase(int iRegionId, bool bEnabled, string sDesc, int groupId, string sDivisionMemberIDs, string sExectives)
        {
            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

            #region SQL for 更新Regions信息
            string sSql = "UPDATE Regions set Enabled=@Enabled, [Desc]=@Desc,GroupID=@groupId where RegionId=" + iRegionId;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Desc", SqlDbType.NVarChar, sDesc);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@groupId", SqlDbType.Int, groupId);
            SqlCmdList.Add(SqlCmd);

            #endregion

            string sSql5 = "update Divisions set RegionID=null where RegionID = " + iRegionId.ToString();
            SqlCommand SqlCmd5 = new SqlCommand(sSql5);
            SqlCmdList.Add(SqlCmd5);

            if (sDivisionMemberIDs != string.Empty)
            {
                // 清空新添加members的ID信息
                string sSql5x = "update Divisions set RegionID=null where RegionID in (" + iRegionId + ")";
                SqlCommand SqlCmd5x = new SqlCommand(sSql5x);
                SqlCmdList.Add(SqlCmd5x);

                #region
                LPWeb.Model.Regions model = GetModel(iRegionId);
                // 获取Group信息 
                string sRegionID = null;

                string sSql6 = string.Empty;
                sSql6 = "update Divisions set RegionID= " + iRegionId.ToString() + " where DivisionID in (" + sDivisionMemberIDs + ")";


                SqlCommand SqlCmd6 = new SqlCommand(sSql6);
                SqlCmdList.Add(SqlCmd6);

                //更新该region下所有Divisions 下的Branch下的RegionID字段
                string sSql12 = "update Branches set RegionID=NULL where RegionID in (" + iRegionId + ")";
                SqlCommand SqlCmd12 = new SqlCommand(sSql12);
                SqlCmdList.Add(SqlCmd12);

                //更新该region下所有Divisions 下的Branch下的RegionID字段
                string sSql11 = "update Branches set RegionID=" + iRegionId + " where DivisionID in (" + sDivisionMemberIDs + ")";
                SqlCommand SqlCmd11 = new SqlCommand(sSql11);
                SqlCmdList.Add(SqlCmd11);

                #endregion
            }

            //更新RegionExecutives 表

            //1.首先删除RegionExecutives表中region所有的Executive关系记录
            string sSql7 = "Delete RegionExecutives where RegionID = " + iRegionId.ToString();
            SqlCommand SqlCmd7 = new SqlCommand(sSql7);
            SqlCmdList.Add(SqlCmd7);

            //2.将sExectives增加到RegionExecutives表
            string sSqlInsertModel = "INSERT INTO RegionExecutives VALUES({0},{1}) ";
            string sSql8 = "";
            var executives = sExectives.Split(',');

            int e;
            int flag = 0;
            foreach (var executive in executives)//循环executives将所有的executive记录到表中
            {
                if (!int.TryParse(executive, out e))//检查该userid是否合法
                    continue;

                flag++;
                sSql8 += string.Format(sSqlInsertModel, iRegionId, executive);
            }

            if (flag > 0)
            {
                SqlCommand SqlCmd8 = new SqlCommand(sSql8);
                SqlCmdList.Add(SqlCmd8);
            }

            #region 更新 group 表

            string sSql9 = "EXEC GroupRelationInfoUpdate 'Region'," + iRegionId.ToString() + "," + groupId.ToString();
            SqlCommand sqlcmd9 = new SqlCommand(sSql9);
            SqlCmdList.Add(sqlcmd9);
            ////更新原来的group为空
            //string sSql9 = "UPDATE Groups SET RegionID=NULL,OrganizationType=NULL,DivisionID=NULL,BranchID=NULL WHERE RegionID=" + iRegionId;
            //SqlCommand sqlcmd9 = new SqlCommand(sSql9);
            //SqlCmdList.Add(sqlcmd9);
            //sSql9 = "UPDATE Groups SET";

            ////设置新的group属于当前region
            //string sSql10 = "UPDATE Groups SET RegionID=" + iRegionId + ",OrganizationType='Region',DivisionID=NULL,BranchID=NULL WHERE GroupID=" + groupId;
            //SqlCommand sqlcmd10 = new SqlCommand(sSql10);
            //SqlCmdList.Add(sqlcmd10);
            #endregion

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
        /// 获取Region列表
        /// neo 2010-09-24
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetRegionListBase(string sWhere)
        {
            string sSql = "select * from Regions where 1=1 " + sWhere + " order by Name";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of region filter for "All Loans" in dashboard home
        /// neo 2010-10-14
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetRegionList_AllLoansBase(int iExecutiveID)
        {
            string sSql = "select RegionId, Name from Regions where Enabled=1 and exists( "
                        + "     select * from Groups as a inner join GroupUsers as b on a.GroupId = b.GroupID where OrganizationType='Company' "
                        + "     and b.UserID = " + iExecutiveID.ToString() + ") "
                        + "union "
                        + "select b.RegionId, b.Name from RegionExecutives as a left outer join Regions as b on a.RegionId = b.RegionId "
                        + "where b.Enabled=1 and ExecutiveId=" + iExecutiveID.ToString() + " "
                        + "union "
                        + "select b.RegionId, b.Name from GroupUsers as a left outer join Regions as b on a.GroupID = b.GroupID "
                        + "where b.Enabled=1 and a.UserID=" + iExecutiveID.ToString()
                        + " Order by Name ";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of region filter for "Assigned Loans" in dashboard home
        /// neo 2010-10-24
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetRegionList_AssingedLoansBase(int iUserID)
        {
            //string sSql = "select RegionId, Name from Regions where RegionId in ( "
            //            + "	    select distinct RegionID from LoanTeam as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
            //            + "     inner join Branches as d on c.BranchId = d.BranchId "
            //            + "     where UserId=" + iUserID + " "
            //            + "     union "
            //            + "     select distinct RegionID from LoanTasks as a inner join PointFiles as b on a.FileId = b.FileId inner join PointFolders as c on b.FolderId = c.FolderId "
            //            + "     inner join Branches as d on c.BranchId = d.BranchId where Owner=" + iUserID + " "
            //            + ")";
            string sSql = string.Format("Select RegionId, Name from [lpfn_GetUserRegions] ({0})", iUserID);
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of region filter
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetRegionFilterBase(int iUserID)
        {
            string sSql = "select RegionID, Name from dbo.lpfn_GetUserRegions(" + iUserID + ") order by Name";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetRegionFilterBase_Branch_Manager(int iUserID)
        {
            string sSql = "select a.RegionID, b.Name from dbo.lpfn_GetUserRegions_Branch_Manager(" + iUserID + ") as a inner join Regions as b on a.RegionID = b.RegionId order by b.Name";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetRegionFilterBase_Executive(int iUserID)
        {
            string sSql = "select a.RegionID, b.Name from dbo.lpfn_GetUserRegions_Executive(" + iUserID + ") as a inner join Regions as b on a.RegionID = b.RegionId order by b.Name";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get items of region filter for user list view
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetRegionFilterBase_UserList(int iUserID)
        {
            string sSql = "select a.RegionID, b.Name from dbo.lpfn_GetUserRegions_UserList(" + iUserID + ") as a inner join Regions as b on a.RegionID = b.RegionId order by b.Name";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataSet GetRegionGoalsReport(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable =
                string.Format(
                    " (SELECT * FROM [dbo].[lpfn_GetRegionGoalsReport]({0})) as RegionGoalsReport ", strWhere);
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


        public DataTable GetUserRegions(int iUserID)
        {
            string sSql = "select * from  lpfn_GetUserRegions(" + iUserID + ")";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

    }
}

