using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类ContactCompanies。
	/// </summary>
    public class ContactCompanies : ContactCompaniesBase
	{
		public ContactCompanies()
		{ }

        #region neo

        /// <summary>
        /// insert contact company
        /// neo 2011-04-08
        /// </summary>
        /// <param name="sCompanyName"></param>
        /// <param name="iServiceTypeID"></param>
        /// <param name="sServiceType"></param>
        /// <param name="sAddress"></param>
        /// <param name="sCity"></param>
        /// <param name="sState"></param>
        /// <param name="sZip"></param>
        /// <param name="strUrl"></param>
        public void InsertContactCompanyBase(string sCompanyName, int iServiceTypeID, string sServiceType, string sAddress, string sCity, string sState, string sZip, string strUrl)
        {
            string sSql = "INSERT INTO ContactCompanies (Name,ServiceTypes,Address,City,State,Zip,Website,ServiceTypeId,Enabled) VALUES (@Name,@ServiceTypes,@Address,@City,@State,@Zip,@Website,@ServiceTypeId,@Enabled)";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sCompanyName);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@ServiceTypes", SqlDbType.NVarChar, sServiceType);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Address", SqlDbType.NVarChar, sAddress);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@City", SqlDbType.NVarChar, sCity);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@State", SqlDbType.NVarChar, sState);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Zip", SqlDbType.NVarChar, sZip);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Website", SqlDbType.NVarChar, strUrl);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@ServiceTypeId", SqlDbType.Int, iServiceTypeID);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, true);

            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        /// <summary>
        /// get contact company info
        /// neo 2011-04-08
        /// </summary>
        /// <param name="iContactCompanyID"></param>
        /// <returns></returns>
        public DataTable GetContactCompanyInfoBase(int iContactCompanyID)
        {
            string sSql = "select * from dbo.ContactCompanies where ContactCompanyId=" + iContactCompanyID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// update contact company info
        /// neo 2011-04-08
        /// </summary>
        /// <param name="iContactCompanyID"></param>
        /// <param name="sCompanyName"></param>
        /// <param name="iServiceTypeID"></param>
        /// <param name="sServiceType"></param>
        /// <param name="sAddress"></param>
        /// <param name="sCity"></param>
        /// <param name="sState"></param>
        /// <param name="sZip"></param>
        /// <param name="bEnabled"></param>
        public void UpdateContactCompanyBase(int iContactCompanyID, string sCompanyName, int iServiceTypeID, string sServiceType, string sAddress, string sCity, string sState, string sZip, bool bEnabled, string strWebSite)
        {
            string sSql = "UPDATE ContactCompanies SET Name = @Name,Address = @Address,City = @City,State = @State,Zip = @Zip,Website = @Website,ServiceTypeId = @ServiceTypeId,Enabled = @Enabled,ServiceTypes = @ServiceTypes WHERE ContactCompanyId=@ContactCompanyId";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sCompanyName);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@ServiceTypes", SqlDbType.NVarChar, sServiceType);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Address", SqlDbType.NVarChar, sAddress);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@City", SqlDbType.NVarChar, sCity);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@State", SqlDbType.NVarChar, sState);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Zip", SqlDbType.NVarChar, sZip);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Website", SqlDbType.NVarChar, strWebSite);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@ServiceTypeId", SqlDbType.Int, iServiceTypeID);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@ContactCompanyId", SqlDbType.Int, iContactCompanyID);

            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        /// <summary>
        /// add partner branch to partner company
        /// neo 2011-04-12
        /// </summary>
        /// <param name="iCompanyID"></param>
        /// <param name="sBranchIDs"></param>
        public void AddBranchToCompanyBase(int iCompanyID, string sBranchIDs)
        {
            string sSql = "update ContactBranches set ContactCompanyId=" + iCompanyID + " where (ContactBranchId in (" + sBranchIDs + "))";
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
        }

        /// <summary>
        /// remove partner branch from partner company
        /// neo 2011-04-12
        /// </summary>
        /// <param name="sBranchIDs"></param>
        public void RemoveBranchFromCompanyBase(string sBranchIDs)
        {
            string sSql = "update ContactBranches set ContactCompanyId=null where (ContactBranchId in (" + sBranchIDs + "))";
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
        }

        /// <summary>
        /// delete partner company
        /// neo 2011-04-14
        /// </summary>
        /// <param name="iCompanyID"></param>
        public void DeletePartnerCompanyBase(int iCompanyID)
        {
            string sSql = "delete from ContactNotes where ContactId in (select ContactId from Contacts where ContactBranchId in (select ContactBranchId from ContactBranches where ContactCompanyId=@ContactCompanyId));"
                        + "delete from ContactActivities where ContactId in (select ContactId from Contacts where ContactBranchId in (select ContactBranchId from ContactBranches where ContactCompanyId=@ContactCompanyId));"
                        + "delete from ContactUsers where ContactId in (select ContactId from Contacts where ContactBranchId in (select ContactBranchId from ContactBranches where ContactCompanyId=@ContactCompanyId));"
                        + "delete from LoanContacts where ContactId in (select ContactId from Contacts where ContactBranchId in (select ContactBranchId from ContactBranches where ContactCompanyId=@ContactCompanyId));"
                        + "delete from Contacts where ContactBranchId in (select ContactBranchId from ContactBranches where ContactCompanyId=@ContactCompanyId);"
                        + "update Prospect set Referral=null where Referral in (select ContactId from Contacts where ContactBranchId in (select ContactBranchId from ContactBranches where ContactCompanyId=@ContactCompanyId));"
                        + "delete from Prospect where ContactId in (select ContactId from Contacts where ContactBranchId in (select ContactBranchId from ContactBranches where ContactCompanyId=@ContactCompanyId));"
                        + "delete from ContactBranches where ContactCompanyId=@ContactCompanyId;"
                        + "delete from ContactCompanies where ContactCompanyId=@ContactCompanyId";

            SqlCommand SqlCmd = new SqlCommand(sSql);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@ContactCompanyId", SqlDbType.Int, iCompanyID);

            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        /// <summary>
        /// disable partner company
        /// neo 2011-04-17
        /// </summary>
        /// <param name="iCompanyID"></param>
        public void DisablePartnerCompanyBase(int iCompanyID) 
        {
            string sSql = "update ContactCompanies set Enabled=0 where ContactCompanyId=" + iCompanyID + ";"
                        + "update ContactBranches set Enabled=0 where ContactCompanyId=" + iCompanyID + ";"
                        + "update Contacts set Enabled=0 where ContactCompanyId=" + iCompanyID + ";"
                        + "update ContactUsers set Enabled=0 where ContactId in (select ContactId from Contacts where ContactCompanyId=" + iCompanyID + ")";
            DbHelperSQL.ExecuteNonQuery(sSql);
        }

        #endregion

        public DataTable Search(string strWhere)
        {
            //string sSql = " select distinct CompanyName "
            //       + " from (select cc.Name as CompanyName,cb.Name  as BranchName,cc.ServiceTypeid ,cc.City,cc.State   from dbo.ContactCompanies cc "
            //       + " left join  dbo.ContactBranches cb on cb.ContactCompanyId =cc.ContactCompanyId   where  cc.Enabled=1) company ";

            string sSql = "select * from ( select cc.Name as CompanyName,cb.ContactBranchId ,cb.Name  as BranchName,cc.ServiceTypeid ,cb.[Address],cb.City,cb.State,cb.Phone,cb.Zip,cc.ContactCompanyId "
                           + " from dbo.ContactBranches cb left join dbo.ContactCompanies cc  on cb.ContactCompanyId =cc.ContactCompanyId "
                           + " where  cc.Enabled=1 ) t ";

            if (!string.IsNullOrEmpty(strWhere))
            {
                sSql += " where  " + strWhere;
            }
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        #region Add by wangxiao for getting branch info in Employment Detail popup
        public DataTable SearchSingleContact(string strWhere)
        {
            string sSql = "SELECT * FROM ContactBranches";

            if (!string.IsNullOrEmpty(strWhere))
            {
                sSql += " WHERE  " + strWhere;
            }
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
        #endregion
    }
}

