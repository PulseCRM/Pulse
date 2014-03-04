using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    public  class ProspectTasks : ProspectTasksBase
    {
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable =
                string.Format(
                    @"(SELECT dbo.ProspectTasks.ProspectTaskId,dbo.lpfn_GetProspectTaskIcon(dbo.ProspectTasks.ProspectTaskId) as [Status], dbo.ProspectTasks.ContactId, dbo.ProspectTasks.TaskName, dbo.ProspectTasks.[Desc], dbo.ProspectTasks.OwnerId, 
                      Convert(char(10),dbo.ProspectTasks.Due,120) as DueDesc,dbo.ProspectTasks.Due, Convert(char(10),dbo.ProspectTasks.Completed,120) as Completed, dbo.ProspectTasks.CompletionEmailTemplid, dbo.ProspectTasks.OverdueEmailTemplId, 
                      dbo.ProspectTasks.WarningEmailTemplId, ISNULL(dbo.Users.LastName, '') + ', ' + ISNULL(dbo.Users.FirstName, '') AS OwnerName
FROM         dbo.ProspectTasks LEFT OUTER JOIN
                      dbo.Users ON dbo.ProspectTasks.OwnerId = dbo.Users.UserId) as t", strWhere);
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 4000),
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
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataTable GetOwnerInfo()
        {
            string sSql =
                string.Format(
                    @"SELECT dbo.ProspectTasks.OwnerId, 
                       ISNULL(dbo.Users.LastName, '') + ', ' + ISNULL(dbo.Users.FirstName, '') AS OwnerName
FROM         dbo.ProspectTasks LEFT OUTER JOIN
                      dbo.Users ON dbo.ProspectTasks.OwnerId = dbo.Users.UserId");
          
            var dt = DbHelperSQL.ExecuteDataTable(sSql);


            return dt;
        }

        /// <summary>
        /// Update Complate Task
        /// Alex 2011-02-25
        /// </summary>
        /// <param name="iLoanTaskID"></param>
        public bool ComplateSelProspectTask(int iTaskID, int UserID, ref int iCompletionEmailTemplid)
        {
            try
            {
                string sSql = "Update ProspectTasks set Completed=getdate(),CompletedBy='" + UserID + "' where ProspectTaskId=" + iTaskID;
                DbHelperSQL.ExecuteNonQuery(sSql);

                sSql = "select CompletionEmailTemplid from ProspectTasks  where ProspectTaskId=" + iTaskID;
                DataSet ds = DbHelperSQL.Query(sSql);
                if (ds != null)
                {
                    iCompletionEmailTemplid = ds.Tables[0].Rows[0][0] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
                else
                {
                    iCompletionEmailTemplid = 0;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Deletes the Prospect Tasks.
        /// </summary>
        /// <param name="iTaskIDs"></param>
        public void DeleteProspectTasks(string iTaskIDs,int UserID)
        {
            foreach (string iTaskID in iTaskIDs.Split(','))
            {
                SqlParameter[] parameters = {
					new SqlParameter("@ProspectTaskid", SqlDbType.Int),
                    new SqlParameter("@UserID", SqlDbType.Int)
					};
                parameters[0].Value = Convert.ToInt32(iTaskID);
                parameters[1].Value = UserID;
                int rowsAffected;
                DbHelperSQL.RunProcedure("[lpsp_RemoveProspectTasks]", parameters, out rowsAffected);
            }
           
        }

        public bool IsProspectTaskNameExists(int nId, string strName)
        {
            strName = strName.Replace('\'', '\"');
            string sSql = "";
            if (nId > 0)
                sSql = string.Format("SELECT ProspectTaskId FROM ProspectTasks WHERE ProspectTaskId<>'{0}' AND TaskName='{1}'", nId, strName);
            else
                sSql = string.Format("SELECT ProspectTaskId FROM ProspectTasks WHERE TaskName='{0}'", strName);
            return DbHelperSQL.Exists(sSql);
        }

        public void CheckProspectTaskAlert(int iProspectTaskID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("CheckProspectTaskAlert");

            SqlParameter[] parameters = {
					new SqlParameter("@ProspectTaskId", SqlDbType.Int,4)
                                        };
            parameters[0].Value = iProspectTaskID;

            DbHelperSQL.RunProcedure(strSql.ToString(), parameters);
        }
    }
}
