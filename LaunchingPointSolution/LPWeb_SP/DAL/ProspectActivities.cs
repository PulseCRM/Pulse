using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    public class ProspectActivities : ProspectActivitiesBase
    {
        public ProspectActivities()
        { }

        /// <summary>
        /// get data for loan activities list
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="count"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string strTableName = "(SELECT PA.*, CASE(ISNULL(PA.UserId, '')) WHEN '' THEN 'System' ELSE ISNULL(U.FirstName, '') + ' ' + ISNULL(U.LastName, '') END AS PerformedBy FROM ProspectActivities PA LEFT JOIN Users U ON PA.UserId=U.UserId) t";
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = strTableName;
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
        /// get data for loan activities list
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="count"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType, int iContactID)
        {
            ////string strTableName = "(SELECT PA.*, CASE(ISNULL(PA.UserId, '')) WHEN '' THEN 'System' ELSE ISNULL(U.FirstName, '') + ' ' + ISNULL(U.LastName, '') END AS PerformedBy FROM ProspectActivities PA LEFT JOIN Users U ON PA.UserId=U.UserId) t";
            //string strTable = string.Format("(select ActivityId,UserId,'' as PerformedBy,ActivityName,ActivityTime,FileId, '0' as form, (select top 1 case l.[Status] when 'Prospect' then 'Prospect Activity' when 'suspend' then 'Archived Loan Activity' when 'denied' then 'Archived Loan Activity' when 'cancel' then 'Archived Loan Activity' when 'closed' then 'Archived Loan Activity' else ' Active Loan Note' end  from dbo.[lpvw_GetLoanContactwRoles] lc inner join Loans l on lc.FileId=l.FileId where ContactId=" + iContactID + " and (RoleName='Borrower' or RoleName='CoBorrower')) as ActivityType, ");
            //strTable += string.Format("(select top 1 Name from PointFiles where FileId in (select lc.FileId from dbo.[lpvw_GetLoanContactwRoles] lc inner join Loans l on lc.FileId=l.FileId where ContactId=" + iContactID + " and (RoleName='Borrower' or RoleName='CoBorrower')))  as ActivityFile  from dbo.LoanActivities where FileId in (select lc.FileId from dbo.[lpvw_GetLoanContactwRoles] lc inner join Loans l on lc.FileId=l.FileId where ContactId=" + iContactID + " and (RoleName='Borrower' or RoleName='CoBorrower')) ");
            //strTable += string.Format(" Union SELECT ProspectActivityId as ActivityId, UserId, '' as PerformedBy,ActivityName,ActivityTime,-1 as FileId, '1' as form,  'Prospect Activity' as ActivityType, '' as ActivityFile FROM dbo.ProspectActivities  where ContactId =" + iContactID + " ) as tblNotes", strWhere);

           string strTable = string.Format("(select la.ActivityId,la.UserId,'' as PerformedBy,la.ActivityName,la.ActivityTime,la.FileId, '0' as form, ");
                strTable += string.Format("(case LOWER(l.[Status]) when 'prospect' then 'Opportunity Activity' when 'suspend' then 'Archived Loan Activity' when 'denied' then 'Archived Loan Activity' when 'canceled' then 'Archived Loan Activity' when 'closed' then 'Archived Loan Activity' else 'Active Loan Activity' end) as ActivityType,");
                strTable += string.Format("pf.Name  as ActivityFile ");
                strTable += string.Format(" from dbo.LoanActivities la left outer join Loans l on la.FileId =l.FileId ");
                strTable += string.Format("left outer join PointFiles pf on pf.FileId=la.FileId");
                strTable += string.Format(" where la.FileId in (select lc.FileId from dbo.[lpvw_GetLoanContactwRoles] lc inner join Loans l on lc.FileId=l.FileId where ContactId=" + iContactID + " and (RoleName='Borrower' or RoleName='CoBorrower')) ");
                strTable += string.Format(" Union SELECT ProspectActivityId as ActivityId, UserId, '' as PerformedBy,ActivityName,ActivityTime,-1 as FileId, '1' as form,  'Prospect Activity' as ActivityType, '' as ActivityFile FROM dbo.ProspectActivities  where ContactId =" + iContactID + " ) as tblNotes", strWhere);

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 2000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = strTable;
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
        /// Get proformedby user of loan activity
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetProformedBy(string strWhere)
        {
            string strSql = "SELECT DISTINCT PA.ContactId, PA.UserId, U.FirstName, U.LastName, ISNULL(U.FirstName, '') + ' ' + ISNULL(U.LastName, '') AS PerformedBy FROM ProspectActivities PA INNER JOIN Users U ON PA.UserId=U.UserId WHERE 1=1 {0} ORDER BY U.FirstName";
            strSql = string.Format(strSql, strWhere);
            return DbHelperSQL.ExecuteDataTable(strSql);
        }

        public DataTable GetActivityTypeInfo(int iContactID)
        {
            string sSql = @"select (case [Status] when 'Prospect' then 'Opportunity' when 'Processing' then 'Active Loan' else [Status] + ' Loan' end + ' - ' + LienPosition + ' - ' + PropertyAddr + ' Activities') as ActivityTypeName,FileId as ContactId  from dbo.Loans where FileId in (                                     
                        select lc.FileId from dbo.[lpvw_GetLoanContactwRoles] lc inner join Loans l on lc.FileId=l.FileId where ContactId=" + iContactID + " and (RoleName='Borrower' or RoleName='CoBorrower'))";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
    }
}
