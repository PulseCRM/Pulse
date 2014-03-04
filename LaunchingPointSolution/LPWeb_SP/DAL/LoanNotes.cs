using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanNotes。
	/// </summary>
    public class LoanNotes : LoanNotesBase
	{
		public LoanNotes()
		{}

        /// <summary>
        /// Gets the loan notes.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="queryCondition">The query condition.</param>
        /// <param name="recordCount">The record count.</param>
        /// <param name="orderName">Name of the order.</param>
        /// <param name="orderType">Type of the order.</param>
        /// <returns></returns>
	    public DataSet GetLoanNotes(int pageSize, int pageIndex, string queryCondition, out int recordCount, string orderName, int orderType)
	    {
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
            parameters[0].Value = "lpvw_GetLoanNotes";
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
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.LoanNotes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanNotes(");
            strSql.Append("FileId,Created,Sender,Note,Exported,ExternalViewing,LoanTaskId)");
            strSql.Append(" values (");
//            strSql.Append("@FileId,getdate(),@Sender,@Note,@Exported,@ExternalViewing)");
            strSql.Append("@FileId,@LocalTime,@Sender,@Note,@Exported,@ExternalViewing,@LoanTaskId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
                    new SqlParameter("@LocalTime", SqlDbType.DateTime),
					new SqlParameter("@Sender", SqlDbType.NVarChar,255),
					new SqlParameter("@Note", SqlDbType.NVarChar),
					new SqlParameter("@Exported", SqlDbType.Int),
                    new SqlParameter("@ExternalViewing",SqlDbType.Bit),
                    new SqlParameter("@LoanTaskId", SqlDbType.Int,4) };
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.Created;
            parameters[2].Value = model.Sender;
            parameters[3].Value = model.Note;
            parameters[4].Value = model.Exported;
            parameters[5].Value = model.ExternalViewing;
            parameters[6].Value = DBNull.Value;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        public int Add_LoanTaskId(LPWeb.Model.LoanNotes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanNotes(");
            strSql.Append("FileId,Created,Sender,Note,Exported,ExternalViewing,LoanTaskId)");
            strSql.Append(" values (");
            //            strSql.Append("@FileId,getdate(),@Sender,@Note,@Exported,@ExternalViewing)");
            strSql.Append("@FileId,@LocalTime,@Sender,@Note,@Exported,@ExternalViewing,@LoanTaskId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
                    new SqlParameter("@LocalTime", SqlDbType.DateTime),
					new SqlParameter("@Sender", SqlDbType.NVarChar,255),
					new SqlParameter("@Note", SqlDbType.NVarChar,500),
					new SqlParameter("@Exported", SqlDbType.Int),
                    new SqlParameter("@ExternalViewing",SqlDbType.Bit),
                    new SqlParameter("@LoanTaskId", SqlDbType.Int,4) };
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.Created;
            parameters[2].Value = model.Sender;
            parameters[3].Value = model.Note;
            parameters[4].Value = model.Exported;
            parameters[5].Value = model.ExternalViewing;
            if (model.LoanTaskId == null)
            {
                parameters[6].Value = DBNull.Value;
            }
            else
            {
                parameters[6].Value = model.LoanTaskId;
            }

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        public void UpdateNoteAndProspectIncomeAndProspectAssets(int FileId, string Note, string Other, string Amount)
        {
            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                string sSqlNote = "update LoanNotes set Note='" + Note + "' where FileId=" + FileId + "";
                int LoanNotes = LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSqlNote);

                string sSqlProspectIncome = "update ProspectIncome set Other='" + Other + "' where ContactId=" + FileId + "";
                int ProspectIncome = LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSqlProspectIncome);

                string sSqlProspectAssets = "update ProspectAssets set Type='Other',Amount='" + Note + "' where ContactId=" + FileId + "";
                int ProspectAssets = LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSqlProspectAssets);
           
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
           
        }

        public void InterNoteAndProspectIncomeAndProspectAssets(int FileId, string Note, string Other, string Amount)
        {
            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                string sSqlNote = "insert into LoanNotes(FileId,Created,Sender,Note) values(" + FileId + "," + DateTime.Now + ",'" + Note + "','" + Note + "'";
                int LoanNotes = LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSqlNote);

                string sSqlProspectIncome = "insert into ProspectIncome(ContactId,Other) values(" + FileId + ",'" + Other + "'";
                int ProspectIncome = LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSqlProspectIncome);

                string sSqlProspectAssets = "insert into ProspectAssets(ContactId,Type,Amount) values(" + FileId + ",'Other','" + Note + "'";
                int ProspectAssets = LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSqlProspectAssets);

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

        }

        public string GetLastNoteByFileID(int iFileID)
        {
            string sNote = "";
            string sSQL = "Select top 1 Note from LoanNotes where FileId=" + iFileID + " order by Created desc";
            object obj=DbHelperSQL.GetSingle(sSQL);
            if (obj != null)
            {
                sNote = obj.ToString();
            }
            return sNote;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileId"></param>
        /// <param name="LoanConditionId"></param>
        /// <param name="Note"></param>
        /// <param name="ExternalViewing"></param>
        /// <param name="Sender"></param>
        public void InsertConditionNote(int FileId, int LoanConditionId, string Note, bool ExternalViewing, string Sender) 
        {
            string sSql =
    @"INSERT INTO LoanNotes
           (FileId
           ,Created
           ,Sender
           ,Note
           ,Exported
           ,ExternalViewing
           ,LoanTaskId
           ,LoanConditionId)
     VALUES
           (" + FileId + @"
           ,getdate()
           ,@Sender
           ,@Note
           ,null
           ,@ExternalViewing
           ,null
           ," + LoanConditionId + @")";

            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Note", SqlDbType.NVarChar, Note);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@ExternalViewing", SqlDbType.Bit, ExternalViewing);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Sender", SqlDbType.NVarChar, Sender);

            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="LoanConditionId"></param>
        /// <returns></returns>
        public DataTable GetConditionNoteList(int iFileId, int LoanConditionId) 
        {
            string sSql = "select * from dbo.LoanNotes where FileId= " + iFileId + " and LoanConditionId=" + LoanConditionId + " order by Created desc";

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NoteIDs"></param>
        /// <param name="bEnabled"></param>
        public void EnableExternalViewing(string NoteIDs, bool bEnabled) 
        {
            string sSql = "update LoanNotes set ExternalViewing=@ExternalViewing where NoteId in (" + NoteIDs + ")";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@ExternalViewing", SqlDbType.Bit, bEnabled);
            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iNoteId"></param>
        /// <returns></returns>
        public DataTable GetLoanNotesInfo(int iNoteId) 
        {
            string sSql = "select * from LoanNotes where NoteId=" + iNoteId;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
	}
}

