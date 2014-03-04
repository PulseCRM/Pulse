using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Groups。
    /// </summary>
    public class Groups : GroupsBase
    {
        public Groups()
        { }

        #region 刘洋添加的方法

        /// <summary>
        /// 获取Group列表
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetGroupListBase(string sWhere)
        {
            string sSql = "select * from Groups where 1=1 " + sWhere + " order by GroupName"; ;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 获取Group信息
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <returns></returns>
        public DataTable GetGroupInfoBase(int iGroupID)
        {
            string sSql = "select * from Groups where GroupID=" + iGroupID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 获取Group Member列表
        /// neo 2010-09-04
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <returns></returns>
        public DataTable GetGroupMemberListBase(int iGroupID)
        {
            string sSql = "select * from Users inner join GroupUsers on Users.UserID = GroupUsers.UserID where GroupID = " + iGroupID + " and UserEnabled = 1 order by Users.LastName";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 获取Group Member选择列表
        /// neo 2010-09-04
        /// </summary>
        /// <returns></returns>
        public DataTable GetGroupMemberSelectionListBase()
        {
            string sSql = "select distinct Users.* from Users left outer join GroupUsers on Users.UserID = GroupUsers.UserID order by Users.LastName";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 保存Group和Members信息
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <param name="bEnabled"></param>
        /// <param name="sGroupDesc"></param>
        /// <param name="sOldGroupMemberIDs"></param>
        /// <param name="sGroupMemberIDs"></param>
        public void SaveGroupAndMembersBase(int iGroupID, bool bEnabled, string sGroupDesc, string sOldGroupMemberIDs, string sGroupMemberIDs)
        {
            Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

            #region SQL for 更新Group信息

            string sSql = "update Groups set Enabled=@Enabled, GroupDesc=@GroupDesc where GroupID=" + iGroupID;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@GroupDesc", SqlDbType.NVarChar, sGroupDesc);
            SqlCmdList.Add(SqlCmd);

            #endregion

            if (sOldGroupMemberIDs != string.Empty)
            {
                // 解除原有User和Group之间的关系
                string sSql5 = "delete from GroupUsers where GroupID=" + iGroupID + " and UserID in (" + sOldGroupMemberIDs + ")";
                SqlCommand SqlCmd5 = new SqlCommand(sSql5);
                SqlCmdList.Add(SqlCmd5);
            }

            if (sGroupMemberIDs != string.Empty)
            {
                // 添加User和Group的关系
                string[] GroupMemberIDArray = sGroupMemberIDs.Split(',');
                foreach (string sMemberID in GroupMemberIDArray)
                {
                    string sSql5x = "insert into GroupUsers (GroupID, UserID) values (" + iGroupID + "," + sMemberID + ")";
                    SqlCommand SqlCmd5x = new SqlCommand(sSql5x);
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
        /// 检查Group Name是否存在
        /// </summary>
        /// <param name="sGroupName"></param>
        /// <returns></returns>
        public bool IsExist_CreateBase(string sGroupName)
        {
            string sSql = "select count(1) from Groups where upper(GroupName)=@GroupName";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@GroupName", SqlDbType.NVarChar, sGroupName.ToUpper());
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
        /// 创建Group
        /// </summary>
        /// <param name="sGroupName"></param>
        /// <returns></returns>
        public int CreateGroupBase(string sGroupName)
        {
            string sSql1 = "insert into Groups (GroupName, Enabled) values (@GroupName, 1);select SCOPE_IDENTITY();";
            SqlCommand SqlCmd1 = new SqlCommand(sSql1);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@GroupName", SqlDbType.NVarChar, sGroupName);
            object oNewID = DbHelperSQL.ExecuteScalar(SqlCmd1);
            int iNewID = Convert.ToInt32(oNewID);
            return iNewID;
        }

        #endregion

        public DataSet GetGroupsByDivisionID(int Divisionid)
        {
            string strSql = " SELECT [GroupId],[GroupName]  FROM [Groups] where ([DivisionID] = @Divisionid and [CompanyID] IS NULL AND isnull([BranchID],0) = 0) or ( isnull(RegionID,0) = 0  and isnull([DivisionID],0) = 0 and isnull([BranchID],0) = 0  and isnull(CompanyID,0) = 0  )  ORDER BY [GroupName] ";

            SqlParameter[] parameters = {
					new SqlParameter("@Divisionid", SqlDbType.Int,4)};
            parameters[0].Value = Divisionid;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            return ds;

        }

        public DataSet GetGroupsByBranchID(int BranchID)
        {
            string strSql = " SELECT [GroupId],[GroupName]  FROM [Groups] where ([BranchID] = @BranchID) or ( isnull(RegionID,0) = 0  and isnull([DivisionID],0) = 0 and isnull([BranchID],0) = 0  and isnull(CompanyID,0) = 0  )  ";

            SqlParameter[] parameters = {
					new SqlParameter("@BranchID", SqlDbType.Int,4)};
            parameters[0].Value = BranchID;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            return ds;

        }
        /// <summary>
        /// Query Groups by region id
        /// </summary>
        /// <param name="regionId">region id</param>
        /// <returns></returns>
        public DataSet GetGroupsByRegionID(int regionId)
        {
            string strSql = "SELECT [GroupId],[GroupName]  FROM [Groups] where ([RegionID] = @RegionID and [CompanyID] IS NULL and [DivisionID] IS null and [BranchID] IS null) or ([CompanyID] IS NULL AND RegionID IS null and [DivisionID] IS null and [BranchID] IS null) ORDER BY GroupName ASC";

            SqlParameter[] parameters = {
					new SqlParameter("@RegionID", SqlDbType.Int,4)};
            parameters[0].Value = regionId;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            return ds;

        }

        /// <summary>
        /// Gets the company rel groups.
        /// </summary>
        /// <returns></returns>
        public DataSet GetCompanyRelGroups()
        {
            string strCommand =
                @"SELECT [GroupId]
                          ,[GroupName]
                          ,[Enabled]
                          ,[CompanyID]
                          ,[RegionID]
                          ,[DivisionID]
                          ,[BranchID]
                          ,[OrganizationType]
                          ,[GroupDesc]
                      FROM [dbo].[Groups] where ([CompanyID] is null and [RegionID] is null and [DivisionID] is null and [BranchID] is null) or ([CompanyID] is not null and  OrganizationType='Company') ORDER BY GroupName ASC
                    ";
            DataSet ds = DbHelperSQL.Query(strCommand);
            return ds;
        }

        /// <summary>
        /// Updates the group access.
        /// </summary>
        /// <param name="prevId">The prev id.</param>
        /// <param name="newId">The new id.</param>
        /// <param name="companyId">The company id.</param>
        /// <param name="orgType">Type of the org.</param>
        /// <returns></returns>
        public bool UpdateGroupAccess(int prevId, int newId, int companyId, string orgType)
        {
            string strCommand = "lpsp_SetCompanyGroupAccess";
            int retVal = 0;
            SqlParameter[] parameters = {
					new SqlParameter("@prevId", SqlDbType.Int,4),
					new SqlParameter("@newId", SqlDbType.Int,4),
					new SqlParameter("@companyId", SqlDbType.Int,4),
					new SqlParameter("@orgType", SqlDbType.NVarChar,20)};
            parameters[0].Value = prevId;
            parameters[1].Value = newId;
            parameters[2].Value = companyId;
            parameters[3].Value = orgType;

            DbHelperSQL.RunProcedure(strCommand, parameters, out retVal);
            if (retVal > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// get group object of Company
        /// </summary>
        /// <returns></returns>
        public Model.Groups GetCompanyGroup()
        {
            string strSql = "SELECT * FROM dbo.Groups WHERE CompanyID IS NOT NULL AND OrganizationType='Company'";
            DataTable dtCompany = DbHelperSQL.ExecuteDataTable(strSql);
            LPWeb.Model.Groups groupCompany = null;
            if (null != dtCompany && dtCompany.Rows.Count > 0)
            {
                groupCompany = new Model.Groups();
                if (dtCompany.Rows[0]["GroupId"].ToString() != "")
                    groupCompany.GroupId = int.Parse(dtCompany.Rows[0]["GroupId"].ToString());
                groupCompany.GroupName = dtCompany.Rows[0]["GroupId"].ToString();
                if (dtCompany.Rows[0]["Enabled"].ToString() != "")
                {
                    if ((dtCompany.Rows[0]["Enabled"].ToString() == "1") || (dtCompany.Rows[0]["Enabled"].ToString().ToLower() == "true"))
                    {
                        groupCompany.Enabled = true;
                    }
                    else
                    {
                        groupCompany.Enabled = false;
                    }
                }
                groupCompany.OrganizationType = dtCompany.Rows[0]["OrganizationType"].ToString();
            }
            return groupCompany;
        }

    }
}

