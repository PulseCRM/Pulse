using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanTeam。
	/// </summary>
    public class LoanTeam : LoanTeamBase
	{
		public LoanTeam()
		{}

        public void Reassign(LPWeb.Model.LoanTeam oldModel, LPWeb.Model.LoanTeam model, int UserId)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@FileId", SqlDbType.Int),
                new SqlParameter("@NewUserId", SqlDbType.Int),
                new SqlParameter("@OldUserId", SqlDbType.Int),
                new SqlParameter("@RoleId", SqlDbType.Int),
                new SqlParameter("@Requester", SqlDbType.Int)
            };


            parameters[0].Value = oldModel.FileId;
            parameters[1].Value = model.UserId;
            parameters[2].Value = oldModel.UserId;
            parameters[3].Value = model.RoleId;
            parameters[4].Value = UserId;

            DbHelperSQL.RunProcedure("lpsp_ReassignLoan", parameters);
        }
        public string GetLoanOfficer(int FileId)
        {
            string LO = string.Empty;
            string sqlCmd = string.Format("Select dbo.lpfn_GetLoanOfficer({0})", FileId);
            object obj = DbHelperSQL.GetSingle(sqlCmd);
            if (obj == null)
                return LO;
            LO = Convert.ToString(obj);
            return LO;
        }

        public int GetLoanOfficerID(int FileId)
        {

            int LO = 0;
            string sqlCmd = string.Format("Select dbo.lpfn_GetLoanOfficerID({0})", FileId);
            object obj = DbHelperSQL.GetSingle(sqlCmd);
            if (obj == null)
                return LO;
            LO = Convert.ToInt32(obj);
            return LO;

        }

        public int GetBranchManage(int FileId)
        {
             int UId = 0;
            string sqlCmd = string.Format(@"select lt.UserId from LoanTeam lt inner join Users u
                                            on lt.UserId=u.UserId inner join Roles r
                                            on lt.RoleId=r.RoleId and r.Name='Branch Manager' and lt.FileId = {0}", FileId);
            object obj = DbHelperSQL.GetSingle(sqlCmd);
            if (obj == null)
                return UId;
            UId = Convert.ToInt32(obj);
            return UId;
        }

        public string GetProcessor(int FileId)
        {
            string Proc = string.Empty;
            string sqlCmd = string.Format("Select dbo.lpfn_GetProcessor({0})", FileId);
            object obj = DbHelperSQL.GetSingle(sqlCmd);
            if (obj == null)
                return Proc;
            Proc = Convert.ToString(obj);
            return Proc;
        }

        public DataSet GetUserLoan(int nUserId)
        {
            string strSql = string.Format("SELECT FileId,RoleId,UserId FROM LoanTeam where UserId={0}", nUserId);
            return DbHelperSQL.Query(strSql);
        }

        public int GetUserLoanCount(int nUserId)
        {
            string strSql = string.Format("SELECT dbo.lpfn_GetUserLoanCount({0})", nUserId);
            object objResult = DbHelperSQL.ExecuteScalar(strSql);
            int nResult = 0;
            if (!int.TryParse(string.Format("{0}", objResult), out nResult))
                nResult = 0;
            return nResult;
        }

        /// <summary>
        /// get loan officer user info
        /// neo 2012-12-29
        /// </summary>
        /// <param name="iFileId"></param>
        /// <returns></returns>
        public DataTable GetLoanOfficerInfo(int iFileId) 
        {
            string sSql = @"select u.* from LoanTeam lt inner join Users u
	on lt.UserId=u.UserId inner join Roles r
	on lt.RoleId=r.RoleId and r.Name='Loan Officer' and lt.FileId =" + iFileId;

            return DbHelperSQL.ExecuteDataTable(sSql);
        }
	}
}

