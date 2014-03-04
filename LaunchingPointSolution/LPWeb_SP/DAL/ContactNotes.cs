using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类ContactNotes。
	/// </summary>
    public class ContactNotes : ContactNotesBase
	{
		public ContactNotes()
		{}
        /// <summary>
        /// Gets the Contact notes.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="queryCondition">The query condition.</param>
        /// <param name="recordCount">The record count.</param>
        /// <param name="orderName">Name of the order.</param>
        /// <param name="orderType">Type of the order.</param>
        /// <returns></returns>
        public DataSet GetContactNotes(int pageSize, int pageIndex, string queryCondition, out int recordCount, string orderName, int orderType)
        {
            string strTable = string.Format("(SELECT ContactNoteId, ContactId, Created, (FirstName +' ' + LastName) AS CreaterName, Note  FROM dbo.ContactNotes left Outer join dbo.Users on dbo.Users.UserId=dbo.ContactNotes.CreatedBy WHERE {0}) AS ProspectNotes", queryCondition);
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
            parameters[0].Value = strTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = pageSize;
            parameters[4].Value = pageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "";
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            recordCount = int.Parse(parameters[7].Value.ToString());
            return ds;
        }
	}
}

