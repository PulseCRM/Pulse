using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    public class ProspectNotes : ProspectNotesBase
    {
        public ProspectNotes()
		{}

        /// <summary>
        /// Gets the prospect notes.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="queryCondition">The query condition.</param>
        /// <param name="recordCount">The record count.</param>
        /// <param name="orderName">Name of the order.</param>
        /// <param name="orderType">Type of the order.</param>
        /// <returns></returns>
        public DataSet GetProspectNotes(int pageSize, int pageIndex, string queryCondition, out int recordCount, string orderName, int orderType, int iContactID)
        {
            ////string strTable = string.Format("(SELECT PropsectNoteId, ContactId, Created, Sender AS SenderName, Note, Exported FROM dbo.ProspectNotes WHERE {0}) AS ProspectNotes", queryCondition);

            //string strTable = string.Format("(select NoteId,Created,Sender AS SenderName,Note,Exported,FileId, '0' as form,[Status] as StatusName, (select top 1 case l.[Status] when 'Prospect' then 'Prospect Note' when 'suspend' then 'Archived Loan Note' when 'denied' then 'Archived Loan Note' when 'cancel' then 'Archived Loan Note' when 'closed' then 'Archived Loan Note' else ' Active Loan Note' end  from dbo.[lpvw_GetLoanContactwRoles] lc inner join Loans l on lc.FileId=l.FileId where ContactId=" + iContactID + " and (RoleName='Borrower' or RoleName='CoBorrower')) as LoanType, ");
            //strTable += string.Format("(select top 1 Name from PointFiles where FileId in (select lc.FileId from dbo.[lpvw_GetLoanContactwRoles] lc inner join Loans l on lc.FileId=l.FileId where ContactId=" + iContactID + " and (RoleName='Borrower' or RoleName='CoBorrower')))  as LoanFile ");
            //strTable += string.Format(" from dbo.LoanNotes where FileId in (select lc.FileId from dbo.[lpvw_GetLoanContactwRoles] lc inner join Loans l on lc.FileId=l.FileId where ContactId=" + iContactID + " and (RoleName='Borrower' or RoleName='CoBorrower')) ");
            //strTable += string.Format(" Union SELECT PropsectNoteId as NoteId, Created, Sender AS SenderName, Note, Exported,-1 as FileId, '1' as form,  'Prospect Note' as LoanType, '' as LoanFile FROM dbo.ProspectNotes where ContactId =" + iContactID + " ) as tblNotes", queryCondition);

            string strTable = string.Format("(select ln.NoteId,ln.Created,ln.Sender as SenderName, ln.Note,ln.Exported,ln.FileId,'0' as form,");
            strTable += string.Format("l.[Status] as StatusName,(case l.[Status] when 'Prospect' then 'Opportunity Note' when 'suspend' then 'Archived Loan Note' when 'denied' then 'Archived Loan Note' when 'cancel' then 'Archived Loan Note' when 'closed' then 'Archived Loan Note' else ' Active Loan Note' end) as LoanType, ");
            strTable += string.Format("pf.Name  as LoanFile ");
            strTable += string.Format(" from dbo.LoanNotes ln left outer join  Loans l on ln.FileId =l.FileId ");
            strTable += string.Format(" left outer join PointFiles pf on pf.FileId=ln.FileId ");
            strTable += string.Format(" where ln.FileId in (select lc.FileId from dbo.[lpvw_GetLoanContactwRoles] lc inner join Loans l on lc.FileId=l.FileId where ContactId=" + iContactID + " and (RoleName='Borrower' or RoleName='CoBorrower'))");
            strTable += string.Format(" Union SELECT PropsectNoteId as NoteId, Created, Sender AS SenderName, Note, Exported,-1 as FileId, '1' as form, '' as StatusName, 'Prospect Note' as LoanType, '' as LoanFile FROM dbo.ProspectNotes where ContactId =" + iContactID + " ) as tblNotes", queryCondition);


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
            parameters[3].Value = pageSize;
            parameters[4].Value = pageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = queryCondition;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            recordCount = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public DataTable GetLoanNoteTypeInfo(int iContactID)
        {
            string sSql = @"select (case [Status] when 'Prospect' then 'Opportunity' when 'Processing' then 'Active Loan' else [Status] + ' Loan' end + ' - ' + LienPosition + ' - ' + PropertyAddr + ' Notes') as NoteTypeName,[Status] as StatusName,FileId as ContactId  from dbo.Loans where FileId in (                                     
                        select lc.FileId from dbo.[lpvw_GetLoanContactwRoles] lc inner join Loans l on lc.FileId=l.FileId where ContactId=" + iContactID +" and (RoleName='Borrower' or RoleName='CoBorrower'))";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetLoanNoteTypeInfoForAdd(int iContactID)
        {
            string sSql = @"select (case [Status] when 'Prospect' then 'Opportunity' when 'Processing' then 'Active Loan' else [Status] end) as StatusName, (case [Status] when 'Prospect' then 'Opportunity' when 'Processing' then 'Active Loan' else [Status] + ' Loan' end + ' - ' + LienPosition + ' - ' + PropertyAddr + ' Note') as NoteTypeName,FileId as ContactId  from dbo.Loans where FileId in (                                     
                        select lc.FileId from dbo.[lpvw_GetLoanContactwRoles] lc inner join Loans l on lc.FileId=l.FileId where ContactId=" + iContactID + " and (RoleName='Borrower' or RoleName='CoBorrower')) and ([Status]='Prospect' or [Status]='Processing')";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
    }
}
