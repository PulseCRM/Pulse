using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类ContactBranches。
	/// </summary>
    public class ContactBranches : ContactBranchesBase
	{
		public ContactBranches()
		{}

        public DataSet GetPartnerContacts(int iLoginUserID, int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
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
            parameters[0].Value = "(select dbo.lpfn_GetTotalReferral(ContactId, " + iLoginUserID + ") as TotalReferral, dbo.lpfn_GetTotalReferralFunded(ContactId, " + iLoginUserID + ") as TotalReferralFunded, isnull(dbo.lpfn_GetTotalReferralFunded(ContactId, " + iLoginUserID + "),0)/dbo.lpfn_GetTotalReferral(ContactId, " + iLoginUserID + ") as WinRatio, dbo.lpfn_GetTotalReferral_FileIDs(ContactId, " + iLoginUserID + ") as TotalReferralFileIDs, dbo.lpfn_GetTotalReferralFunded_FileIDs(ContactId, " + iLoginUserID + ") as TotalReferralFundedFileIDs, * from lpvw_PartnerContacts) as t";
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

        public DataTable GePartnerContactsForSel(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable =
                string.Format(@"(SELECT     dbo.Contacts.ContactId, dbo.Contacts.FirstName, dbo.Contacts.LastName, dbo.Contacts.LastName + ', ' + dbo.Contacts.FirstName AS ContactName, 
                      dbo.ContactBranches.ContactBranchId, dbo.ContactBranches.Name AS BranchName, dbo.ContactCompanies.ContactCompanyId, 
                      dbo.ContactCompanies.Name AS CompanyName, dbo.ServiceTypes.ServiceTypeId, dbo.ServiceTypes.Name AS ServiceType,'' as ContactType
                        ,dbo.Contacts.Enabled, dbo.Contacts.MailingAddr, dbo.Contacts.MailingCity, dbo.Contacts.MailingState 
FROM         dbo.ServiceTypes INNER JOIN
                      dbo.ContactCompanies ON dbo.ServiceTypes.ServiceTypeId = dbo.ContactCompanies.ServiceTypeId RIGHT OUTER JOIN
                      dbo.Contacts ON dbo.ContactCompanies.ContactCompanyId = dbo.Contacts.ContactCompanyId LEFT OUTER JOIN
                      dbo.ContactBranches ON dbo.Contacts.ContactBranchId = dbo.ContactBranches.ContactBranchId ) as t", strWhere);

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

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                dr["ContactType"] = GetContactTypeByID(Convert.ToInt32(dr["ContactId"]));
            }
            dt.AcceptChanges();

            return dt;
        }

        public bool DisableBranch(int iContactBranchID)
        {
            int rowsAffected = 0;

            SqlParameter[] parameters = {
               new SqlParameter("@ContactBranchId", SqlDbType.Int)};
            parameters[0].Value = iContactBranchID;


            DbHelperSQL.RunProcedure("dbo.lpsp_DisableContactBranch", parameters, out rowsAffected);
            return true;
        }

        public bool DisableContact(int iContactID)
        {
            int rowsAffected = 0;

            SqlParameter[] parameters = {
               new SqlParameter("@ContactId", SqlDbType.Int)};
            parameters[0].Value = iContactID;


            DbHelperSQL.RunProcedure("dbo.lpsp_DisablePartnerContact", parameters, out rowsAffected);
            return true;
        }

        public bool RomoveBranch(int iContactBranchID)
        {
            int rowsAffected = 0;

            SqlParameter[] parameters = {
               new SqlParameter("@ContactBranchId", SqlDbType.Int)};
            parameters[0].Value = iContactBranchID;


            DbHelperSQL.RunProcedure("dbo.lpsp_RemoveContactBranch", parameters, out rowsAffected);
            return true;
        }

        /// <summary>
        /// Insert ContactUser
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="iContactID"></param>
        /// <returns></returns>
        public bool AssignUser2Contact(int iUserID, int iContactID)
        {
            string sSql = "select count(*) from ContactUsers where UserID=" + iUserID + " and ContactId=" + iContactID + "";
            DataTable dt = DbHelperSQL.ExecuteDataTable(sSql);
            int iCount = Convert.ToInt32(dt.Rows[0][0]);
            if (iCount == 0)
            {
                sSql = "insert into ContactUsers(UserID,ContactId,[Enabled],Created) values ";
                sSql += "(" + iUserID + "," + iContactID + ",1,getdate())";
                DbHelperSQL.ExecuteSql(sSql);
            }
            return true;
        }

        /// <summary>
        /// 检查branch name是否存在
        /// Alex 2011-04-14
        /// </summary>
        /// <param name="sRuleName"></param>
        /// <returns></returns>
        public bool IsExist_CreateBase(string sBranchName)
        {
            string sSql = "select count(1) from ContactBranches where Name = @Name";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sBranchName);
            int iCount = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd));
            if (iCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查branch name是否存在
        /// Alex 2011-04-14
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <param name="sRuleName"></param>
        /// <returns></returns>
        public bool IsExist_EditBase(int iBranchID, string sBranchName)
        {
            string sSql = "select count(1) from ContactBranches where Name = @Name and ContactBranchId != " + iBranchID;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sBranchName);
            int iCount = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd));
            if (iCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Update Contacts with BranchID
        /// </summary>
        /// <param name="iBranchID"></param>
        /// <param name="sContactIDs"></param>
        public void AddContactToBranch(int iBranchID, string sContactIDs)
        {
            string sSql = "";
            foreach (string sContactID in sContactIDs.Split(','))
            {
                sSql = "Update Contacts set ContactCompanyId=(select ContactCompanyId from ContactBranches where ContactBranchId=" + iBranchID + "), ContactBranchId=" + iBranchID + " where ContactId=" + sContactID;
                DbHelperSQL.ExecuteSql(sSql);
            }
        }

        /// <summary>
        /// Remove Contacts set BranchID=null
        /// </summary>
        /// <param name="iBranchID"></param>
        /// <param name="sContactIDs"></param>
        public void RemoveContactSetBranchNull(string sContactIDs)
        {
            string sSql = "";
            foreach (string sContactID in sContactIDs.Split(','))
            {
                sSql = "Update Contacts set ContactBranchId=NULL,ContactCompanyId=NULL where ContactId=" + sContactID;
                DbHelperSQL.ExecuteSql(sSql);
            }
        }

        private string GetContactTypeByID(int iContactID)
        {
            string sType = "";
            string sSql = @"Select Contacts.ContactId,'Client' as ContactType ,'1' as tyfrom from 
            Contacts left join LoanContacts on Contacts.ContactId=LoanContacts.ContactId inner join Prospect on Contacts.ContactId=Prospect.ContactId
            where LoanContacts.ContactRoleId=(dbo.lpfn_GetBorrowerRoleId()) 
            OR LoanContacts.ContactRoleid=(dbo.lpfn_GetCoBorrowerRoleId()) 
            Union 
            Select Contacts.ContactId,'Partner' as ContactType,'2' as tyfrom from 
            Contacts left join LoanContacts on Contacts.ContactId=LoanContacts.ContactId 
            where LoanContacts.ContactRoleId<>(dbo.lpfn_GetBorrowerRoleId() ) 
            and LoanContacts.ContactRoleid<>(dbo.lpfn_GetCoBorrowerRoleId())
            OR ISNULL(Contacts.ContactBranchId,0)!=0";
            DataTable dt = DbHelperSQL.ExecuteDataTable(sSql);
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["ContactId"]) == iContactID)
                {
                    sType += dr["ContactType"].ToString() + ",";
                }
            }
            return sType.TrimEnd(',');
        }
       
	}
}

