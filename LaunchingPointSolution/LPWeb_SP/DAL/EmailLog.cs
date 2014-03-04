using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类EmailLog。
    /// </summary>
    public class EmailLog : EmailLogBase
    {
        public EmailLog()
        { }
        
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = @"(SELECT dbo.lpfn_GetEmailLogFromUser(EmailLogId) as FromUserName, 
                dbo.lpfn_GetEmailLogFirstToUser(EmailLogId) as ToUserName, *,dbo.lpfn_GetEmailLog_AttachmentsByEmailLogId(EmailLogId) as Attachments FROM EmailLog) as t";

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,2000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "1=1 " + strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataSet GetListForGridView_Client(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable = @"(SELECT dbo.lpfn_GetEmailLogFromUser(EmailLogId) as FromUserName, 'Prospect Email' as EmailType, '' as LoanFile,
                dbo.lpfn_GetEmailLogFirstToUser(EmailLogId) as ToUserName, *,dbo.lpfn_GetEmailLog_AttachmentsByEmailLogId(EmailLogId) as Attachments FROM EmailLog) as t";

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,2000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "1=1 " + strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        /// <summary>
        /// get email log list for prospect
        /// neo 2011-04-28
        /// </summary>
        /// <param name="sDbTable"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public DataTable GetProspectEmailLogListBase(string sDbTable, int iStartIndex, int iEndIndex, string strWhere, string orderName, int orderType)
        {
            string sAscOrDesc = string.Empty;
            if(orderType == 0)
            {
                sAscOrDesc = "asc";
            }
            else
            {
                sAscOrDesc = "desc";
            }

            SqlCommand SqlCmd = new SqlCommand("lpsp_ExecSqlByPager");
            SqlCmd.CommandType = CommandType.StoredProcedure;
            DbHelperSQL.AddSqlParameter(SqlCmd, "@OrderByField", SqlDbType.NVarChar, orderName);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@AscOrDesc", SqlDbType.VarChar, sAscOrDesc);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Fields", SqlDbType.NVarChar, "*");
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DbTable", SqlDbType.NVarChar, sDbTable);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Where", SqlDbType.NVarChar, strWhere);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@StartIndex", SqlDbType.Int, iStartIndex);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@EndIndex", SqlDbType.Int, iEndIndex);

            return DbHelperSQL.ExecuteDataTable(SqlCmd);
        }


        public DataTable GetEmailLogAttachments(int iEmailLogID)
        {

            string sql = string.Format("SELECT [EmailLogId],[FileId],[Name],[FileType],[FileImage],datalength([FileImage]) as FileSize FROM EmailLog_Attachments WHERE [EmailLogId] = {0}", iEmailLogID);

            return DbHelperSQL.ExecuteDataTable(sql);
        }
    }
}

