using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace LPWeb.DAL
{
    public class LoanMarketing : LoanMarketingBase
    {
        /// <summary>
        /// 根据ContactID 得到对应的LoanMarketing信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="count"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <param name="iContactID"></param>
        /// <returns></returns>
        public DataSet GetLoanMarketingByContactID(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType,string iContactID)
        {
            string tempTable =
               string.Format(
                   @"(
SELECT LoanMarketing.LoanMarketingId, LoanMarketing.Selected, LoanMarketing.Type, LoanMarketing.Started, LoanMarketing.StartedBy, 
LoanMarketing.CampaignId, LoanMarketing.Status, LoanMarketing.FileId, LoanMarketing.SelectedBy, MarketingCampaigns.CampaignName, h.Success, h.Error,(case lpvw_PipelineInfo.Status when 'Prospect' then lpvw_PipelineInfo.ProspectLoanStatus when 'Processing' then 'Active' else lpvw_PipelineInfo.Status end) as LoanStatus,
(case lpvw_PipelineInfo.Status when 'Prospect' then lpvw_PipelineInfo.ProspectLoanStatus when 'Processing' then 'Active Loan' else lpvw_PipelineInfo.Status + ' Loan' end)  + ' ' + lpvw_PipelineInfo.Filename AS Loan, dbo.lpfn_GetUserName(Users.UserId) AS StartedByUser
,lme.ExecutionDate,lme.Completed,lme.Action,lme.LoanMarketingEventId,lme.WeekNo	
FROM LoanMarketing 
LEFT OUTER JOIN Users ON LoanMarketing.StartedBy = Users.UserId 
LEFT OUTER JOIN lpvw_PipelineInfo ON LoanMarketing.FileId = lpvw_PipelineInfo.FileId 
LEFT OUTER JOIN MarketingCampaigns ON LoanMarketing.CampaignId = MarketingCampaigns.CampaignId
LEFT OUTER JOIN MarketingLog h on LoanMarketing.LoanMarketingId=h.LoanMarketingId 
LEFT OUTER JOIN LoanMarketingEvents lme on LoanMarketing.LoanMarketingId = lme.LoanMarketingId
where dbo.LoanMarketing.FileId in(Select FileId from dbo.LoanContacts where dbo.LoanContacts.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() OR dbo.LoanContacts.ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId() and ContactId=" + iContactID + ")) as t", strWhere);
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

        public DataTable GetDisStartByInfo()
        {
            DataTable dt;
            string sSql = @"SELECT  Distinct   dbo.Users.UserId, dbo.lpfn_GetUserName(dbo.Users.UserId) AS StartedByUser
                        FROM         dbo.LoanMarketing LEFT OUTER JOIN
                        dbo.Users ON dbo.LoanMarketing.StartedBy = dbo.Users.UserId order by StartedByUser";
            dt = DbHelperSQL.ExecuteDataTable(sSql);
            return dt;
        }

        public DataTable GetLoanTypeInfoForAdd(int iContactID)
        {
            string sSql = @"select (case [Status] when 'Prospect' then 'Leads' when 'Processing' then 'Active Loan' else [Status] end) as StatusName, (case [Status] when 'Prospect' then 'Leads' when 'Processing' then 'Active Loan' else [Status] + ' Loan' end + ' - ' + LienPosition + ' - ' + PropertyAddr + ' Note') as NoteTypeName,FileId  from dbo.Loans where FileId in (                                     
                        select lc.FileId from dbo.[lpvw_GetLoanContactwRoles] lc inner join Loans l on lc.FileId=l.FileId where ContactId=" + iContactID + " and (RoleName='Borrower' or RoleName='CoBorrower'))";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 根据LoanID 得到对应的LoanMarketing信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="count"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <param name="iFileID"></param>
        /// <returns></returns>
        public DataSet GetLoanMarketingByFileID(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType, string iFileID)
        {
            string tempTable =
               string.Format(
                   @"(SELECT     dbo.LoanMarketing.LoanMarketingId, dbo.LoanMarketing.Selected, dbo.LoanMarketing.Type, dbo.LoanMarketing.Started, dbo.LoanMarketing.StartedBy, 
                      dbo.LoanMarketing.CampaignId,mcat.CategoryName, dbo.LoanMarketing.Status, dbo.LoanMarketing.FileId, dbo.LoanMarketing.SelectedBy, dbo.MarketingCampaigns.CampaignName, h.Success, h.Error,
                      (case dbo.lpvw_PipelineInfo.Status when 'Prospect' then dbo.lpvw_PipelineInfo.ProspectLoanStatus when 'Processing' then 'Active' else dbo.lpvw_PipelineInfo.Status end) as LoanStatus,
                    (case dbo.lpvw_PipelineInfo.Status when 'Prospect' then dbo.lpvw_PipelineInfo.ProspectLoanStatus when 'Processing' then 'Active Loan' else dbo.lpvw_PipelineInfo.Status + ' Loan' end)  + ' - ' + 
	 REVERSE(SUBSTRING(REVERSE(dbo.lpvw_PipelineInfo.Filename), 0, CHARINDEX('\', REVERSE(dbo.lpvw_PipelineInfo.Filename)))) AS Loan, 
	 dbo.lpfn_GetUserName(LoanMarketing.StartedBy) AS StartedByUser,
	 lmt.ExecutionDate,lmt.Completed,lmt.Action,lmt.LoanMarketingEventId,lmt.WeekNo	 
FROM         dbo.LoanMarketing LEFT OUTER JOIN
                      dbo.lpvw_PipelineInfo ON dbo.LoanMarketing.FileId = dbo.lpvw_PipelineInfo.FileId LEFT OUTER JOIN
                      dbo.MarketingCampaigns ON dbo.LoanMarketing.CampaignId = dbo.MarketingCampaigns.CampaignId
                    left outer join MarketingLog h on dbo.LoanMarketing.LoanMarketingId=h.LoanMarketingId 
                    left outer join MarketingCategory mcat on mcat.CategoryId = dbo.MarketingCampaigns.CategoryId
                    left outer join dbo.LoanMarketingEvents lmt on lmt.LoanMarketingId =dbo.LoanMarketing.LoanMarketingId
where dbo.LoanMarketing.FileId =" + iFileID + ") as t", strWhere);
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

        /// <summary>
        /// Delete the loan marketing.
        /// Delete LoanMarketing, LoanMarketingEvents, MarketingLog, 和MarketingQue,
        /// </summary>
        /// <param name="LoanMarketingId">Loan Marketing ID.</param>
        public bool Delete(int LoanMarketingId)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@LoanMarketingId", SqlDbType.Int)
					};
            parameters[0].Value = LoanMarketingId;
            int rowsAffected;
            int rows = DbHelperSQL.RunProcedure("lpsp_RemoveLoanMarketing", parameters, out rowsAffected);

            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
