using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����LoanAlerts��
	/// </summary>
    public class LoanAlerts : LoanAlertsBase
	{
		public LoanAlerts()
		{}

        /// <summary>
        /// ��ҳ��ȡ�����б�
        /// </summary>
        /// <param name="PageSize">ÿҳ�ļ�¼��</param>
        /// <param name="PageIndex">ҳ��</param>
        /// <param name="orderField">�����ֶ�</param>
        /// <param name="sortDirection">������(false������true�ǽ���)</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="count">���ؼ�¼����</param>
        /// <returns></returns>
        public DataSet GetList(int PageSize, int PageIndex, string orderField, bool sortDirection, string strWhere, out int count)
        {
            string execSql = string.Format("(SELECT  distinct UserName,LoanAlertId,FileId,[Desc],DueDate,ClearedBy,[Status], " +
                     "Cleared,AcknowlegeReq,AcknowledgedBy,Acknowledged,LoanRuleId," +
                     "OwnerId,LoanTaskId,AlertType,DateCreated," +
                      "BorrowerName, DisplayIcon,LoanStatus  from ( SELECT * FROM dbo.Get_AlertView WHERE {0} ) as tmp) as tt1", strWhere);

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 4000),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000)
					};
            parameters[0].Value = execSql;
            parameters[1].Value = orderField;
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = false;//�Ƿ��ǲ�ѯ��¼��
            parameters[5].Value = sortDirection;
            parameters[6].Value = "";

            int dcnt;

            var dsData = DbHelperSQL.RunProcedure_AlertList("lpsp_GetRecordByPage_SQL", parameters, "ds", out dcnt);

            //��ѯ��¼��
            SqlParameter[] pas = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 4000),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000)
					};
            pas[0].Value = execSql;
            pas[1].Value = "Desc";
            pas[2].Value = PageSize;
            pas[3].Value = PageIndex;
            pas[4].Value = true;//�Ƿ��ǲ�ѯ��¼��
            pas[5].Value = true;
            pas[6].Value = "";
            var dsRecordCount = DbHelperSQL.RunProcedure("lpsp_GetRecordByPage_SQL", pas, "dsRecordCount");
            if(dsRecordCount==null || dsRecordCount.Tables.Count==0 ||dsRecordCount.Tables[0].Rows.Count==0)
            {
                count = 0;
            }
            else
            {
                count = int.Parse(dsRecordCount.Tables[0].Rows[0][0].ToString());
                count = count - dcnt;
            }
            
            return dsData;
        }

	    public DataTable GetBranches(string con)
	    {
	        string sql =
	            "SELECT     Branches.Name, GroupUsers.UserID, Groups.DivisionID, Groups.RegionID, Groups.BranchID " +
	            " FROM         GroupUsers INNER JOIN " +
	            "Groups ON GroupUsers.GroupID = Groups.GroupId INNER JOIN " +
	            "Branches ON Groups.BranchID = Branches.BranchId";

	        return DbHelperSQL.ExecuteDataTable(sql + con);
	    }

        /// <summary>
        /// �õ����е�OwnerName
        /// </summary>
        /// <returns></returns>
        public DataTable GetAlertOwner()
        {
            string sSql = "select distinct dbo.lpfn_GetUserName(la.OwnerId) as OwnerName,la.OwnerId from dbo.LoanAlerts la left join Users u on u.UserId=la.OwnerId where isnull(dbo.lpfn_GetUserName(la.OwnerId),'')!='' ";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// ��ȡ��Alert List for Dashboard Home
        /// neo 2010-10-12
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetSimpleAlertListBase(int iUserID) 
        {
            string sSqlu = "select distinct top(7) a.FileId, a.AlertType, a.LoanAlertId, a.LoanTaskId, a.[Desc], dbo.lpfn_GetBorrower(a.FileId) as FullName, "
                           + "CASE a.AlertType WHEN 'Rate Lock' THEN dbo.lpfn_GetRateLockIcon(a.FileId) "
                           + "WHEN 'Task Alert' THEN dbo.lpfn_GetTaskIcon(a.LoanTaskId) "
                           + "WHEN 'Rule Alert' THEN dbo.lpfn_RuleAlertIconByAlert(a.LoanAlertId) "
                           + "ELSE dbo.lpfn_GetAlertIconFileName(a.LoanAlertId) END AS DisplayIcon, a.DueDate "
                        + "from LoanAlerts as a "
                        + "inner join LoanTeam as t on t.FileId=a.FileId "
                        + "inner join LoanContacts as c on a.FileId = c.FileId "
                        + "inner join Contacts as d on c.ContactId = d.ContactId "
                        + "where (a.[Status] IS NULL OR a.Status='Acknowledged') AND "
                        + "((a.AlertType='Task Alert' and a.OwnerId=" + iUserID + " and a.DueDate<=GetDate()) OR "
                        + "(a.AlertType='Rule Alert' and t.UserId=" + iUserID + ")) order by a.DueDate asc ";

            return DbHelperSQL.ExecuteDataTable(sSqlu);
        }

        public DataTable GetSimpleAlertListBase(int iUserID, string sWhere_DueDate)
        {
            string sSqlu =   "select distinct top(7) a.FileId, a.AlertType, a.LoanAlertId, a.LoanTaskId, a.[Desc], dbo.lpfn_GetBorrower(a.FileId) as FullName, "
                           + "CASE a.AlertType WHEN 'Rate Lock' THEN dbo.lpfn_GetRateLockIcon(a.FileId) "
                           + "WHEN 'Task Alert' THEN dbo.lpfn_GetTaskIcon(a.LoanTaskId) "
                           + "WHEN 'Rule Alert' THEN dbo.lpfn_RuleAlertIconByAlert(a.LoanAlertId) "
                           + "ELSE dbo.lpfn_GetAlertIconFileName(a.LoanAlertId) END AS DisplayIcon, a.DueDate "
                           + "from LoanAlerts as a "
                           + "inner join LoanTeam as t on t.FileId=a.FileId "
                           + "inner join LoanContacts as c on a.FileId = c.FileId "
                           + "inner join Contacts as d on c.ContactId = d.ContactId "
                           + "where (a.[Status] IS NULL OR a.Status='Acknowledged') "
                           + "and (a.OwnerId=" + iUserID + ") "
                           + sWhere_DueDate + " "
                           + "order by a.DueDate asc ";

            return DbHelperSQL.ExecuteDataTable(sSqlu);
        }

        public DataTable Loan_GetSimpleAlertListBase(int iUserID, string sWhere_DueDate)
        {
            string sSqlu = "select distinct top(7) a.FileId, a.AlertType, a.LoanAlertId, a.LoanTaskId, a.[Desc], dbo.lpfn_GetBorrower(a.FileId) as FullName, "
                           + "CASE a.AlertType WHEN 'Rate Lock' THEN dbo.lpfn_GetRateLockIcon(a.FileId) "
                           + "WHEN 'Task Alert' THEN dbo.lpfn_GetTaskIcon(a.LoanTaskId) "
                           + "WHEN 'Rule Alert' THEN dbo.lpfn_RuleAlertIconByAlert(a.LoanAlertId) "
                           + "ELSE dbo.lpfn_GetAlertIconFileName(a.LoanAlertId) END AS DisplayIcon, a.DueDate "
                           + "from LoanAlerts as a "
                           + "inner join LoanTeam as t on t.FileId=a.FileId "
                           + "inner join LoanContacts as c on a.FileId = c.FileId "
                           + "inner join Contacts as d on c.ContactId = d.ContactId "
                           + "where (a.[Status] IS NULL OR a.Status='Acknowledged') "
                           + "and (a.FileId in (select Fileid from loans where loans.status='Processing')) "
                           + "and (a.OwnerId=" + iUserID + ") "
                           + sWhere_DueDate + " "
                           + "order by a.DueDate asc ";

            return DbHelperSQL.ExecuteDataTable(sSqlu);
        }

        /// <summary>
        /// �õ�Loan ��һ�� Rule Alert ��ID
        /// Alex 2011-01-24
        /// </summary>
        /// <param name="iLoadID"></param>
        /// <returns></returns>
        public int GetLoadRuleAlertID(int iLoadID)
        {
            int iLoanAlertID = 0;
            string sSql = "select top 1 LoanAlertId from LoanAlerts where FileId=" + iLoadID + " and AlertType='Rule Alert' AND ([Status] is null OR [Status] = 'Acknowledged') order by DueDate";
            DataTable dt= DbHelperSQL.ExecuteDataTable(sSql);
            if (dt.Rows.Count > 0)
            {
                iLoanAlertID = Convert.ToInt32(dt.Rows[0][0]);
            }
            return iLoanAlertID;
        }
	}
}

