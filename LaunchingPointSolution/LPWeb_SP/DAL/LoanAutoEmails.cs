using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace LPWeb.DAL
{
    public class LoanAutoEmails : LoanAutoEmailsBase
    { 
		public void UpdateEmailSettings(LPWeb.Model.LoanAutoEmails model)
		{
			int rowsAffected;
			SqlParameter[] parameters = { 
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@ToContactId", SqlDbType.Int,4),
					new SqlParameter("@ToUserId", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1), 
					new SqlParameter("@Applied", SqlDbType.DateTime),
					new SqlParameter("@AppliedBy", SqlDbType.Int,4), 
					new SqlParameter("@ScheduleType", SqlDbType.SmallInt,2),
					new SqlParameter("@External", SqlDbType.Bit,1),
					new SqlParameter("@TemplReportId", SqlDbType.Int,4)}; 
			parameters[0].Value = model.FileId;
			parameters[1].Value = model.ToContactId;
			parameters[2].Value = model.ToUserId;
			parameters[3].Value = model.Enabled; 
			parameters[4].Value = model.Applied;
			parameters[5].Value = model.AppliedBy;
            parameters[6].Value = model.ScheduleType;
            parameters[7].Value = model.External;
            parameters[8].Value = model.TemplReportId;

            DbHelperSQL.RunProcedure("lpsp_LoanAutoEmails_Save", parameters, out rowsAffected); 

		}

        public int GetLoanAutoEmailIdByContactUserId(int FileId, int UserId, int ContactId)
        {
            int LoanAutoEmailId = 0;
            if (FileId <= 0)
                return LoanAutoEmailId;

            if (UserId <= 0 && ContactId <= 0)
                return LoanAutoEmailId;

            object obj = null;
            string sqlCmd = string.Format("Select LoanAutoEmailId from LoanAutoEmails where FileId={0} ", FileId);
            if (UserId > 0)
            {
                sqlCmd += "AND ToUserId=" + UserId;
            }
            else
            if (ContactId > 0)
                sqlCmd += "AND ToContactId=" + ContactId;

            obj = DbHelperSQL.GetSingle(sqlCmd);

            if (obj == null || obj == DBNull.Value)
                return LoanAutoEmailId;
            LoanAutoEmailId = (int)obj;
            return LoanAutoEmailId;
        }
    }
}
