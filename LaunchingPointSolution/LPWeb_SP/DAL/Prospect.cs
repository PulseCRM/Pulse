using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using LPWeb.Model;
namespace LPWeb.DAL
{
    public class Prospect : ProspectBase
    {
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string tempTable =
                string.Format(
                    @"( select * from dbo.lpvw_GetClients ) as t", strWhere);
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
        /// Deletes the specified Contact ID.
        /// </summary>
        /// <param name="fileID">The file ID.</param>
        public void Delete(int ContactID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@Contactid", SqlDbType.Int)
					};
            parameters[0].Value = ContactID;
            int rowsAffected;
            DbHelperSQL.RunProcedure("[lpsp_RemoveProspect]", parameters, out rowsAffected);
        }

        public void AssignProspect(int ContactID, int UserID,int iOldUserID)
        {
            string sSql = "update Prospect set Loanofficer='" + UserID + "' where ContactID=" + ContactID + "; ";
            sSql += "update ProspectTasks set OwnerId='" + UserID + "' where ContactId=" + ContactID + " and OwnerID=" + iOldUserID + " and isnull(Completed,'')=''";
            DbHelperSQL.ExecuteNonQuery(sSql);
        }
 

        /// <summary>
        /// Gets the task alert detail.
        /// </summary>
        /// <param name="contactID">The contactID.</param>
        /// <returns></returns>
        public DataTable GetTaskAlertDetail(int contactID)
        {
            string sSql =
                string.Format(@"SELECT     TOP 1 ISNULL(dbo.Users.FirstName, '') + ',' + ISNULL(dbo.Users.LastName, '') AS Owner, ISNULL(Users_1.FirstName, '') + ',' + ISNULL(Users_1.LastName, '') 
                      AS LoanOfficer, dbo.ProspectTasks.TaskName, dbo.ProspectTasks.Due, dbo.ProspectAlerts.ContactId, dbo.ProspectAlerts.AlertType,
ISNULL(dbo.Contacts.Lastname,'')+', '+ISNULL(dbo.Contacts.Firstname,'') + ' ' + ISNULL(dbo.Contacts.Middlename,'') AS Client,
dbo.ProspectAlerts.ProspectTaskId,dbo.lpfn_ProspectPipelineTaskAlertIcon(dbo.Prospect.Contactid) AS AlertIcon,dbo.ProspectAlerts.DueDate
FROM         dbo.ProspectTasks RIGHT OUTER JOIN
                      dbo.ProspectAlerts ON dbo.ProspectTasks.ProspectTaskId = dbo.ProspectAlerts.ProspectTaskId LEFT OUTER JOIN
                      dbo.Users ON dbo.ProspectTasks.OwnerId = dbo.Users.UserId LEFT OUTER JOIN
                      dbo.Prospect ON dbo.ProspectTasks.ContactId = dbo.Prospect.Contactid LEFT OUTER JOIN
                      dbo.Users AS Users_1 ON dbo.Prospect.Loanofficer = Users_1.UserId LEFT OUTER JOIN
                      dbo.Contacts ON dbo.ProspectAlerts.ContactId = dbo.Contacts.ContactId
WHERE     (dbo.ProspectAlerts.AlertType = N'Task Alert') AND (dbo.ProspectAlerts.ContactId ={0})
ORDER BY dbo.ProspectAlerts.DueDate",
                              contactID);
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        #region neo

        /// <summary>
        /// get prospect info
        /// neo 2011-03-13
        /// </summary>
        /// <param name="iContactID"></param>
        /// <returns></returns>
        public DataTable GetProspectInfoBase(int iContactID)
        {
            string sSql = "select * from Prospect where ContactID=" + iContactID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// update prospect detail
        /// neo 2011-03-13
        /// </summary>
        /// <param name="iContactID"></param>
        /// <param name="sLeadSource"></param>
        /// <param name="sReferenceCode"></param>
        /// <param name="iModifiedBy"></param>
        /// <param name="iLoanOfficerID"></param>
        /// <param name="sStatus"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sMiddleName"></param>
        /// <param name="sLastName"></param>
        /// <param name="sTitle"></param>
        /// <param name="sGenerationCode"></param>
        /// <param name="sSSN"></param>
        /// <param name="sHomePhone"></param>
        /// <param name="sCellPhone"></param>
        /// <param name="sBusinessPhone"></param>
        /// <param name="sFax"></param>
        /// <param name="sEmail"></param>
        /// <param name="sDOB"></param>
        /// <param name="iExperian"></param>
        /// <param name="iTransUnion"></param>
        /// <param name="iEquifax"></param>
        /// <param name="sAddress"></param>
        /// <param name="sCity"></param>
        /// <param name="sState"></param>
        /// <param name="sZip"></param>
        public void UpdateProspectDetailBase(int iContactID, string sLeadSource, string sReferenceCode, int iModifiedBy, int iLoanOfficerID, string sStatus, string strCreditRanking, string strPreferredContact,
            string sFirstName, string sMiddleName, string sLastName, string sTitle, string sGenerationCode, string sSSN, string sHomePhone,
            string sCellPhone, string sBusinessPhone, string sFax, string sEmail, string sDOB, Int16 iExperian, Int16 iTransUnion, Int16 iEquifax,
            string sAddress, string sCity, string sState, string sZip, int iReferralID)
        {
            #region build sql command - Contacts

            string sSql1 = "UPDATE Contacts SET FirstName = @FirstName,MiddleName = @MiddleName,LastName = @LastName,Title = @Title,GenerationCode = @GenerationCode,SSN = @SSN,HomePhone = @HomePhone,CellPhone = @CellPhone,BusinessPhone = @BusinessPhone,Fax = @Fax,Email = @Email,DOB = @DOB,Experian = @Experian,TransUnion = @TransUnion,Equifax = @Equifax,MailingAddr = @MailingAddr,MailingCity = @MailingCity,MailingState = @MailingState,MailingZip = @MailingZip WHERE ContactID=@ContactID";
            SqlCommand SqlCmd1 = new SqlCommand(sSql1);

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@FirstName", SqlDbType.NVarChar, sFirstName);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@MiddleName", SqlDbType.NVarChar, sMiddleName);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@LastName", SqlDbType.NVarChar, sLastName);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Title", SqlDbType.NVarChar, sTitle);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@GenerationCode", SqlDbType.NVarChar, sGenerationCode);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@SSN", SqlDbType.NVarChar, sSSN);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@HomePhone", SqlDbType.NVarChar, sHomePhone);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@CellPhone", SqlDbType.NVarChar, sCellPhone);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@BusinessPhone", SqlDbType.NVarChar, sBusinessPhone);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Fax", SqlDbType.NVarChar, sFax);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Email", SqlDbType.NVarChar, sEmail);

            if (sDOB == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@DOB", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DateTime DOB = Convert.ToDateTime(sDOB);
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@DOB", SqlDbType.DateTime, DOB);
            }

            if(iExperian == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@Experian", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@Experian", SqlDbType.SmallInt, iExperian);
            }

            if (iTransUnion == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@TransUnion", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@TransUnion", SqlDbType.SmallInt, iTransUnion);
            }

            if (iEquifax == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@Equifax", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@Equifax", SqlDbType.SmallInt, iEquifax);
            }
            
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@MailingAddr", SqlDbType.NVarChar, sAddress);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@MailingCity", SqlDbType.NVarChar, sCity);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@MailingState", SqlDbType.NChar, sState);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@MailingZip", SqlDbType.NVarChar, sZip);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@ContactID", SqlDbType.Int, iContactID);

            #endregion

            #region build sql command - Prospect

            string sSql2 = "UPDATE Prospect SET LeadSource = @LeadSource,ReferenceCode = @ReferenceCode,Modifed = getdate(),ModifiedBy = @ModifiedBy,Loanofficer = @Loanofficer,Status = @Status, Referral=@Referral, CreditRanking=@CreditRanking, PreferredContact=@PreferredContact WHERE ContactID=@ContactID";
            SqlCommand SqlCmd2 = new SqlCommand(sSql2);

            DbHelperSQL.AddSqlParameter(SqlCmd2, "@LeadSource", SqlDbType.NVarChar, sLeadSource);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@ReferenceCode", SqlDbType.NVarChar, sReferenceCode);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@ModifiedBy", SqlDbType.Int, iModifiedBy);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@Loanofficer", SqlDbType.Int, iLoanOfficerID);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@Status", SqlDbType.NVarChar, sStatus);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@ContactID", SqlDbType.Int, iContactID);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@Referral", SqlDbType.Int, iReferralID);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@CreditRanking", SqlDbType.NVarChar, strCreditRanking);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@PreferredContact", SqlDbType.NVarChar, strPreferredContact);

            #endregion

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                DbHelperSQL.ExecuteNonQuery(SqlCmd1, SqlTrans);

                DbHelperSQL.ExecuteNonQuery(SqlCmd2, SqlTrans);

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

            #endregion
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactInfo"></param>
        /// <param name="prospectInfo"></param>
        /// <returns></returns>
        public int CreateContactAndProspect(LPWeb.Model.Contacts contactInfo, LPWeb.Model.Prospect prospectInfo)
        {
            int iContactId = -1;
            if (contactInfo == null || (prospectInfo == null))
                throw new Exception("CreateProspect, ContactInfo and ProspectInfo cannot be empty.");

            if (string.IsNullOrEmpty(contactInfo.LastName) || string.IsNullOrEmpty(contactInfo.FirstName))
                throw new Exception("CreateProspect, First Name and Last Name cannot be blank.");
            if (string.IsNullOrEmpty(contactInfo.Email) && string.IsNullOrEmpty(contactInfo.HomePhone) && string.IsNullOrEmpty(contactInfo.CellPhone)
                && string.IsNullOrEmpty(contactInfo.BusinessPhone))
                throw new Exception("CreateProspect, must at least specified Email, Home Phone, Cell Phone or Business Phone.");

            Contacts contactMgr = new Contacts();
            iContactId = contactMgr.AddClient(contactInfo);
            if (iContactId > 0)
            {
                Prospect prospectMgr = new Prospect();
                prospectInfo.Contactid = iContactId;
                prospectMgr.Add(prospectInfo);
            }
            return iContactId;

        }

        /// <summary>
        /// create contact and prospect without checking duplicated
        /// neo 2012-10-24
        /// </summary>
        /// <param name="contactInfo"></param>
        /// <param name="prospectInfo"></param>
        /// <returns></returns>
        public int CreateContactAndProspectNoCheck(LPWeb.Model.Contacts contactInfo, LPWeb.Model.Prospect prospectInfo)
        {
            int iContactId = -1;
            if (contactInfo == null || (prospectInfo == null))
                throw new Exception("CreateProspect, ContactInfo and ProspectInfo cannot be empty.");

            if (string.IsNullOrEmpty(contactInfo.LastName) || string.IsNullOrEmpty(contactInfo.FirstName))
                throw new Exception("CreateProspect, First Name and Last Name cannot be blank.");
            if (string.IsNullOrEmpty(contactInfo.Email) && string.IsNullOrEmpty(contactInfo.HomePhone) && string.IsNullOrEmpty(contactInfo.CellPhone)
                && string.IsNullOrEmpty(contactInfo.BusinessPhone))
                throw new Exception("CreateProspect, must at least specified Email, Home Phone, Cell Phone or Business Phone.");

            Contacts contactMgr = new Contacts();
            iContactId = contactMgr.AddClientNoCheck(contactInfo);
            if (iContactId > 0)
            {
                Prospect prospectMgr = new Prospect();
                prospectInfo.Contactid = iContactId;
                prospectMgr.Add(prospectInfo);
            }
            return iContactId;

        }

        /// <summary>
        /// insert prospect detail
        /// neo 2011-03-16
        /// </summary>
        /// <param name="sLeadSource"></param>
        /// <param name="sReferenceCode"></param>
        /// <param name="iCreatedBy"></param>
        /// <param name="iLoanOfficerID"></param>
        /// <param name="sStatus"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sMiddleName"></param>
        /// <param name="sLastName"></param>
        /// <param name="sTitle"></param>
        /// <param name="sGenerationCode"></param>
        /// <param name="sSSN"></param>
        /// <param name="sHomePhone"></param>
        /// <param name="sCellPhone"></param>
        /// <param name="sBusinessPhone"></param>
        /// <param name="sFax"></param>
        /// <param name="sEmail"></param>
        /// <param name="sDOB"></param>
        /// <param name="iExperian"></param>
        /// <param name="iTransUnion"></param>
        /// <param name="iEquifax"></param>
        /// <param name="sAddress"></param>
        /// <param name="sCity"></param>
        /// <param name="sState"></param>
        /// <param name="sZip"></param>
        public void InsertProspectDetailBase(string sLeadSource, string sReferenceCode, int iCreatedBy, int iLoanOfficerID, string sStatus,
            string sFirstName, string sMiddleName, string sLastName, string sTitle, string sGenerationCode, string sSSN, string sHomePhone,
            string sCellPhone, string sBusinessPhone, string sFax, string sEmail, string sDOB, Int16 iExperian, Int16 iTransUnion, Int16 iEquifax,
            string sAddress, string sCity, string sState, string sZip)
        {
            #region build sql command - Contacts

            string sSql1 = "insert into Contacts (FirstName,MiddleName,LastName,Title,GenerationCode,SSN,HomePhone,CellPhone,BusinessPhone,Fax,Email,DOB,Experian,TransUnion,Equifax,MailingAddr,MailingCity,MailingState,MailingZip,ContactEnable,CreatedBy,Created) values (@FirstName,@MiddleName,@LastName,@Title,@GenerationCode,@SSN,@HomePhone,@CellPhone,@BusinessPhone,@Fax,@Email,@DOB,@Experian,@TransUnion,@Equifax,@MailingAddr,@MailingCity,@MailingState,@MailingZip,@ContactEnable,@CreatedBy,getdate());"
                         + "select SCOPE_IDENTITY();";
            SqlCommand SqlCmd1 = new SqlCommand(sSql1);

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@FirstName", SqlDbType.NVarChar, sFirstName);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@MiddleName", SqlDbType.NVarChar, sMiddleName);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@LastName", SqlDbType.NVarChar, sLastName);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Title", SqlDbType.NVarChar, sTitle);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@GenerationCode", SqlDbType.NVarChar, sGenerationCode);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@SSN", SqlDbType.NVarChar, sSSN);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@HomePhone", SqlDbType.NVarChar, sHomePhone);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@CellPhone", SqlDbType.NVarChar, sCellPhone);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@BusinessPhone", SqlDbType.NVarChar, sBusinessPhone);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Fax", SqlDbType.NVarChar, sFax);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@Email", SqlDbType.NVarChar, sEmail);

            if (sDOB == string.Empty)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@DOB", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DateTime DOB = Convert.ToDateTime(sDOB);
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@DOB", SqlDbType.DateTime, DOB);
            }

            if (iExperian == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@Experian", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@Experian", SqlDbType.SmallInt, iExperian);
            }

            if (iTransUnion == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@TransUnion", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@TransUnion", SqlDbType.SmallInt, iTransUnion);
            }

            if (iEquifax == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@Equifax", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@Equifax", SqlDbType.SmallInt, iEquifax);
            }

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@MailingAddr", SqlDbType.NVarChar, sAddress);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@MailingCity", SqlDbType.NVarChar, sCity);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@MailingState", SqlDbType.NChar, sState);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@MailingZip", SqlDbType.NVarChar, sZip);

            DbHelperSQL.AddSqlParameter(SqlCmd1, "@ContactEnable", SqlDbType.Bit, true);
            DbHelperSQL.AddSqlParameter(SqlCmd1, "@CreatedBy", SqlDbType.Int, iCreatedBy);

            #endregion

            #region build sql command - Prospect

            string sSql2 = "insert into Prospect (Contactid,LeadSource,ReferenceCode,Created,CreatedBy,Loanofficer,Status) values (@Contactid,@LeadSource,@ReferenceCode,getdate(),@CreatedBy,@Loanofficer,@Status)";
            SqlCommand SqlCmd2 = new SqlCommand(sSql2);

            DbHelperSQL.AddSqlParameter(SqlCmd2, "@LeadSource", SqlDbType.NVarChar, sLeadSource);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@ReferenceCode", SqlDbType.NVarChar, sReferenceCode);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@CreatedBy", SqlDbType.Int, iCreatedBy);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@Loanofficer", SqlDbType.Int, iLoanOfficerID);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@Status", SqlDbType.NVarChar, sStatus);

            #endregion

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                int iNewContactID = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd1, SqlTrans));

                DbHelperSQL.AddSqlParameter(SqlCmd2, "@ContactID", SqlDbType.Int, iNewContactID);

                DbHelperSQL.ExecuteNonQuery(SqlCmd2, SqlTrans);

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

            #endregion
        }
        
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Prospect model)
        {
            int rowsAffected = 0;

            SqlParameter[] parameters = {
               new SqlParameter("@ContactId", SqlDbType.Int),
               new SqlParameter("@LeadSource", SqlDbType.NVarChar, 255),
               new SqlParameter("@ReferenceCode", SqlDbType.NVarChar, 255),
               new SqlParameter("@Referral", SqlDbType.NVarChar, 255),
               new SqlParameter("@Created", SqlDbType.DateTime),
               new SqlParameter("@CreatedBy", SqlDbType.Int),
               new SqlParameter("@Modifed", SqlDbType.DateTime),
               new SqlParameter("@ModifiedBy", SqlDbType.Int),
               new SqlParameter("@Loanofficer", SqlDbType.Int),
               new SqlParameter("@Status", SqlDbType.NVarChar, 50),
               new SqlParameter("@CreditRanking", SqlDbType.NVarChar, 50),
               new SqlParameter("@PreferredContact", SqlDbType.NVarChar, 50),
               new SqlParameter("@Dependents", SqlDbType.Bit)
                                         };
            parameters[0].Value = model.Contactid;
            parameters[1].Value = model.LeadSource;
            parameters[2].Value = model.ReferenceCode;
            parameters[3].Value = model.Referral;
            parameters[4].Value = model.Created;
            parameters[5].Value = model.CreatedBy;
            parameters[6].Value = null; // Modified
            parameters[7].Value = null; // ModifiedBy
            parameters[8].Value = model.Loanofficer;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.CreditRanking;
            parameters[11].Value = model.PreferredContact;
            parameters[12].Value = model.Dependents;

            int ContactId = DbHelperSQL.RunProcedure("dbo.Prospect_Save", parameters, out rowsAffected);
            return ContactId;

        }
        
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Prospect model)
        {
            int rowsAffected = 0;

            SqlParameter[] parameters = {
               new SqlParameter("@ContactId", SqlDbType.Int),
               new SqlParameter("@LeadSource", SqlDbType.NVarChar, 255),
               new SqlParameter("@ReferenceCode", SqlDbType.NVarChar, 255),
               new SqlParameter("@Referral", SqlDbType.NVarChar, 255),
               new SqlParameter("@Created", SqlDbType.DateTime),
               new SqlParameter("@CreatedBy", SqlDbType.Int),
               new SqlParameter("@Modifed", SqlDbType.DateTime),
               new SqlParameter("@ModifiedBy", SqlDbType.Int),
               new SqlParameter("@Loanofficer", SqlDbType.Int),
               new SqlParameter("@Status", SqlDbType.NVarChar, 50),
               new SqlParameter("@CreditRanking", SqlDbType.NVarChar, 50),
               new SqlParameter("@PreferredContact", SqlDbType.NVarChar, 50)
                                         };
            parameters[0].Value = model.Contactid;
            parameters[1].Value = model.LeadSource;
            parameters[2].Value = model.ReferenceCode;
            parameters[3].Value = model.Referral;
            parameters[4].Value = model.Created;
            parameters[5].Value = model.CreatedBy;
            parameters[6].Value = model.Modifed; // Modified
            parameters[7].Value = model.ModifiedBy; // ModifiedBy
            parameters[8].Value = model.Loanofficer;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.CreditRanking;
            parameters[11].Value = model.PreferredContact;

            DbHelperSQL.RunProcedure("dbo.Prospect_Save", parameters, out rowsAffected);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iContactId"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sLastName"></param>
        /// <param name="sEmail"></param>
        /// <param name="sCellPhone"></param>
        /// <param name="sHomePhone"></param>
        /// <param name="sWorkPhone"></param>
        /// <param name="sDOB"></param>
        /// <param name="sSSN"></param>
        /// <param name="sDependants"></param>
        /// <param name="sCreditRanking"></param>
        /// <param name="sFICO"></param>
        /// <param name="sMailingStreetAddress1"></param>
        /// <param name="sMailingStreetAddress2"></param>
        /// <param name="sMailingCity"></param>
        /// <param name="sMailingState"></param>
        /// <param name="sMailingZip"></param>
        /// <param name="sLeadSource"></param>
        /// <param name="sReferralID"></param>
        public void UpdateBorrower(int iContactId, string sFirstName, string sLastName, string sEmail, string sCellPhone, string sHomePhone,
            string sWorkPhone, string sDOB, string sSSN,
            string sDependants, string sCreditRanking, string sFICO)
        {
            string sSql = "update Contacts set FirstName=@FirstName, LastName=@LastName, Email=@Email, CellPhone=@CellPhone, HomePhone=@HomePhone, "
                        + "BusinessPhone=@BusinessPhone, DOB=@DOB, SSN=@SSN, Experian=@Experian "
                        + "where ContactId=" + iContactId + ";"
                        + "update Prospect set Dependents=@Dependents, CreditRanking=@CreditRanking where ContactId=" + iContactId;
            SqlCommand SqlCmd = new SqlCommand(sSql);

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@FirstName", SqlDbType.NVarChar, sFirstName);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LastName", SqlDbType.NVarChar, sLastName);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Email", SqlDbType.NVarChar, sEmail);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@CellPhone", SqlDbType.NVarChar, sCellPhone);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@HomePhone", SqlDbType.NVarChar, sHomePhone);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@BusinessPhone", SqlDbType.NVarChar, sWorkPhone);

            if (sDOB == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DOB", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DOB", SqlDbType.DateTime, Convert.ToDateTime(sDOB));
            }
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@SSN", SqlDbType.NVarChar, sSSN);
            if (sFICO == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Experian", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Experian", SqlDbType.SmallInt, Convert.ToInt16(sFICO));
            }
            
            #region prospect

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@CreditRanking", SqlDbType.NVarChar, sCreditRanking);
            if (sDependants == "Yes")
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Dependents", SqlDbType.Bit, true);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Dependents", SqlDbType.Bit, false);
            }

            #endregion

            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iContactId"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sLastName"></param>
        /// <param name="sEmail"></param>
        /// <param name="sCellPhone"></param>
        /// <param name="sHomePhone"></param>
        /// <param name="sWorkPhone"></param>
        /// <param name="sDOB"></param>
        /// <param name="sSSN"></param>
        /// <param name="sDependants"></param>
        /// <param name="sCreditRanking"></param>
        /// <param name="sFICO"></param>
        /// <param name="sMailingStreetAddress1"></param>
        /// <param name="sMailingStreetAddress2"></param>
        /// <param name="sMailingCity"></param>
        /// <param name="sMailingState"></param>
        /// <param name="sMailingZip"></param>
        /// <param name="sLeadSource"></param>
        /// <param name="sReferralID"></param>
        public void UpdateBorrower(int iContactId, string sFirstName, string sLastName, string sEmail, string sCellPhone, string sHomePhone,
            string sWorkPhone, string sDOB, string sSSN,
            string sDependants, string sCreditRanking, string sFICO,
            string sMailingStreetAddress1, string sMailingStreetAddress2, string sMailingCity, string sMailingState, string sMailingZip,
            string sLeadSource, string sReferralID)
        {
            string sSql = "update Contacts set FirstName=@FirstName, LastName=@LastName, Email=@Email, CellPhone=@CellPhone, HomePhone=@HomePhone, "
                        + "BusinessPhone=@BusinessPhone, DOB=@DOB, SSN=@SSN, Experian=@Experian, "
                        + "MailingAddr=@MailingAddr, MailingCity=@MailingCity, MailingState=@MailingState, MailingZip=@MailingZip where ContactId=" + iContactId + ";"
                        + "update Prospect set LeadSource=@LeadSource, Referral=@Referral, Dependents=@Dependents, CreditRanking=@CreditRanking where ContactId=" + iContactId;
            SqlCommand SqlCmd = new SqlCommand(sSql);

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@FirstName", SqlDbType.NVarChar, sFirstName);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LastName", SqlDbType.NVarChar, sLastName);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Email", SqlDbType.NVarChar, sEmail);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@CellPhone", SqlDbType.NVarChar, sCellPhone);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@HomePhone", SqlDbType.NVarChar, sHomePhone);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@BusinessPhone", SqlDbType.NVarChar, sWorkPhone);

            if (sDOB == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DOB", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DOB", SqlDbType.DateTime, Convert.ToDateTime(sDOB));
            }
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@SSN", SqlDbType.NVarChar, sSSN);
            if (sFICO == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Experian", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Experian", SqlDbType.SmallInt, Convert.ToInt16(sFICO));
            }
            string sMailingAddr = (sMailingStreetAddress1 + " " + sMailingStreetAddress2).Trim();
            DbHelperSQL.AddSqlParameter(SqlCmd, "@MailingAddr", SqlDbType.NVarChar, sMailingAddr);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@MailingCity", SqlDbType.NVarChar, sMailingCity);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@MailingState", SqlDbType.NVarChar, sMailingState);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@MailingZip", SqlDbType.NVarChar, sMailingZip);

            #region prospect

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LeadSource", SqlDbType.NVarChar, sLeadSource);
            if (sReferralID == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Referral", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Referral", SqlDbType.Int, Convert.ToInt32(sReferralID));
            }
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@CreditRanking", SqlDbType.NVarChar, sCreditRanking);
            if (sDependants == "Yes")
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Dependents", SqlDbType.Bit, true);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Dependents", SqlDbType.Bit, false);
            }
            
            #endregion

            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iContactId"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sLastName"></param>
        /// <param name="sEmail"></param>
        /// <param name="sCellPhone"></param>
        /// <param name="sHomePhone"></param>
        /// <param name="sWorkPhone"></param>
        /// <param name="sDOB"></param>
        /// <param name="sSSN"></param>
        /// <param name="sFICO"></param>
        /// <param name="sMailingStreetAddress1"></param>
        /// <param name="sMailingStreetAddress2"></param>
        /// <param name="sMailingCity"></param>
        /// <param name="sMailingState"></param>
        /// <param name="sMailingZip"></param>
        /// <param name="sLeadSource"></param>
        /// <param name="sReferralID"></param>
        public void UpdateCoBorrower(int iContactId, string sFirstName, string sLastName, string sEmail, string sCellPhone, string sHomePhone,
            string sWorkPhone, string sDOB, string sSSN, string sFICO)
        {
            string sSql = "update Contacts set FirstName=@FirstName, LastName=@LastName, Email=@Email, CellPhone=@CellPhone, HomePhone=@HomePhone, "
                        + "BusinessPhone=@BusinessPhone, DOB=@DOB, SSN=@SSN, Experian=@Experian "
                        + "where ContactId=" + iContactId;
            SqlCommand SqlCmd = new SqlCommand(sSql);

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@FirstName", SqlDbType.NVarChar, sFirstName);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LastName", SqlDbType.NVarChar, sLastName);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Email", SqlDbType.NVarChar, sEmail);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@CellPhone", SqlDbType.NVarChar, sCellPhone);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@HomePhone", SqlDbType.NVarChar, sHomePhone);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@BusinessPhone", SqlDbType.NVarChar, sWorkPhone);

            if (sDOB == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DOB", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DOB", SqlDbType.DateTime, Convert.ToDateTime(sDOB));
            }
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@SSN", SqlDbType.NVarChar, sSSN);
            if (sFICO == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Experian", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Experian", SqlDbType.SmallInt, Convert.ToInt16(sFICO));
            }
            
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iContactId"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sLastName"></param>
        /// <param name="sEmail"></param>
        /// <param name="sCellPhone"></param>
        /// <param name="sHomePhone"></param>
        /// <param name="sWorkPhone"></param>
        /// <param name="sDOB"></param>
        /// <param name="sSSN"></param>
        /// <param name="sFICO"></param>
        /// <param name="sMailingStreetAddress1"></param>
        /// <param name="sMailingStreetAddress2"></param>
        /// <param name="sMailingCity"></param>
        /// <param name="sMailingState"></param>
        /// <param name="sMailingZip"></param>
        /// <param name="sLeadSource"></param>
        /// <param name="sReferralID"></param>
        public void UpdateCoBorrower(int iContactId, string sFirstName, string sLastName, string sEmail, string sCellPhone, string sHomePhone,
            string sWorkPhone, string sDOB, string sSSN, string sFICO,
            string sMailingStreetAddress1, string sMailingStreetAddress2, string sMailingCity, string sMailingState, string sMailingZip,
            string sLeadSource, string sReferralID)
        {
            string sSql = "update Contacts set FirstName=@FirstName, LastName=@LastName, Email=@Email, CellPhone=@CellPhone, HomePhone=@HomePhone, "
                        + "BusinessPhone=@BusinessPhone, DOB=@DOB, SSN=@SSN, Experian=@Experian, "
                        + "MailingAddr=@MailingAddr, MailingCity=@MailingCity, MailingState=@MailingState, MailingZip=@MailingZip where ContactId=" + iContactId + ";"
                        + "update Prospect set LeadSource=@LeadSource, Referral=@Referral where ContactId=" + iContactId;
            SqlCommand SqlCmd = new SqlCommand(sSql);

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@FirstName", SqlDbType.NVarChar, sFirstName);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LastName", SqlDbType.NVarChar, sLastName);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Email", SqlDbType.NVarChar, sEmail);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@CellPhone", SqlDbType.NVarChar, sCellPhone);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@HomePhone", SqlDbType.NVarChar, sHomePhone);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@BusinessPhone", SqlDbType.NVarChar, sWorkPhone);

            if (sDOB == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DOB", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DOB", SqlDbType.DateTime, Convert.ToDateTime(sDOB));
            }
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@SSN", SqlDbType.NVarChar, sSSN);
            if (sFICO == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Experian", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Experian", SqlDbType.SmallInt, Convert.ToInt16(sFICO));
            }
            string sMailingAddr = (sMailingStreetAddress1 + " " + sMailingStreetAddress2).Trim();
            DbHelperSQL.AddSqlParameter(SqlCmd, "@MailingAddr", SqlDbType.NVarChar, sMailingAddr);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@MailingCity", SqlDbType.NVarChar, sMailingCity);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@MailingState", SqlDbType.NVarChar, sMailingState);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@MailingZip", SqlDbType.NVarChar, sMailingZip);

            #region prospect

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LeadSource", SqlDbType.NVarChar, sLeadSource);
            if (sReferralID == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Referral", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Referral", SqlDbType.Int, Convert.ToInt32(sReferralID));
            }
            
            #endregion

            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }
        
        #endregion

        /// <summary>
        /// 得到所有的Partner 数据源
        /// Alex 2011-04-07
        /// </summary>
        /// <returns></returns>
        public DataTable GetReferralCompanies()
        {
            DataTable dt = new DataTable();
            string sSql = @"SELECT  distinct   dbo.Prospect.Referral as ContactCompanyId, dbo.ContactCompanies.Name as Partner
                        FROM         dbo.ContactCompanies RIGHT OUTER JOIN
                      dbo.Contacts ON dbo.ContactCompanies.ContactCompanyId = dbo.Contacts.ContactCompanyId RIGHT OUTER JOIN
                      dbo.Prospect ON dbo.Contacts.ContactId = dbo.Prospect.Referral
                        WHERE     (ISNULL(dbo.Contacts.ContactCompanyId, 0) <> 0) order by ContactCompanies.Name ";
                        //WHERE     (ISNULL(dbo.Prospect.Referral, '') <> '')";
            dt = DbHelperSQL.ExecuteDataTable(sSql);

            return dt;
        }

        /// <summary>
        /// 得到所有的Referra; 数据源
        /// Alex 2011-04-07
        /// </summary>
        /// <returns></returns>
        public DataTable GetReferralContactInfo()
        {
            DataTable dt = new DataTable();
            string sSql = @"SELECT  c1.ContactId, ISNULL(c1.Lastname,'')+ (CASE ISNULL(c1.Lastname,'') when '' then ' ' else ', ' end) +ISNULL(c1.Firstname,'') + ' ' + ISNULL(c1.Middlename,'') as Referral
                        FROM   dbo.Prospect left outer join dbo.Contacts c1 on c1.ContactId = dbo.Prospect.Referral
                        WHERE     (ISNULL(dbo.Prospect.Referral, '') <> '') order by Referral ";
            dt = DbHelperSQL.ExecuteDataTable(sSql);

            return dt;
        }


        public DataSet GetProspectRefLoansInfo(int ReferralContactID, int iLoginUserID, int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
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
            parameters[0].Value = "(select dbo.lpfn_GetContactName(a.Contactid) as ContactFullName, "
                                + "dbo.lpfn_GetTotalReferral_OnClientContactID(" + ReferralContactID + ", " + iLoginUserID + ",a.Contactid) as TotalReferral, "
                                + "dbo.lpfn_GetTotalReferralFunded_OnClientContactID(" + ReferralContactID + ", " + iLoginUserID + ",a.Contactid) as TotalReferralFunded, "
                                + "dbo.lpfn_GetTotalReferralCount_OnClientContactID(" + ReferralContactID + ", " + iLoginUserID + ",a.Contactid) as LoanCount, "
                                + "dbo.lpfn_GetTotalReferral_FileIDs_OnClientContactID(" + ReferralContactID + ", " + iLoginUserID + ",a.Contactid) as TotalReferralFileIDs, "
                                + "dbo.lpfn_GetTotalReferralFunded_FileIDs_OnClientContactID(" + ReferralContactID + ", " + iLoginUserID + ",a.Contactid) as TotalReferralFundedFileIDs, "
                                + "a.* from Prospect as a inner join Contacts as b on a.Contactid=b.ContactId ) as t";

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

    }
}
