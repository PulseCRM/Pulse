using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Contacts。
    /// </summary>
    public class Contacts : ContactsBase
    {
        public Contacts()
        { }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DataSet GetLoanContacts(int FileId, string RoleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT Contacts.FirstName,Contacts.MiddleName, Contacts.LastName, LoanContacts.FileId, LoanContacts.ContactRoleId, ContactRoles.Name, LoanContacts.ContactId , Contacts.DOB, Contacts.TransUnion, Contacts.Experian, Contacts.SSN, Contacts.Equifax ");
            strSql.Append(" FROM LoanContacts INNER JOIN Contacts ON LoanContacts.ContactId = Contacts.ContactId INNER JOIN ContactRoles ON LoanContacts.ContactRoleId = ContactRoles.ContactRoleId ");
            strSql.Append(" WHERE LoanContacts.FileId = @FileId AND ContactRoles.Name = @RoleName  ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int),
					new SqlParameter("@RoleName", SqlDbType.NVarChar)};
            parameters[0].Value = FileId;
            parameters[1].Value = RoleName;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            return ds;
        }
        /// </summary>
        /// <returns></returns>
        public DataSet GetReassignContract(string ServiceTypes)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  ContactCompanies.Name,ContactCompanies.ServiceTypes,Contacts.FirstName, Contacts.LastName, ");
            strSql.Append(" Contacts.MailingCity, 	Contacts.MailingZip FROM ContactCompanies INNER JOIN Contacts ON ContactCompanies.ContactCompanyId = Contacts.ContactCompanyId ");
            strSql.Append(" WHERE ContactCompanies.ServiceTypes = @ServiceTypes ");
            SqlParameter[] parameters = { 
					new SqlParameter("@ServiceTypes", SqlDbType.NVarChar)};
            parameters[0].Value = ServiceTypes;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            return ds;
        }

        public void UpdateContact(LPWeb.Model.Contacts model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@FirstName", SqlDbType.NVarChar,50),
					new SqlParameter("@LastName", SqlDbType.NVarChar,50), 
					new SqlParameter("@CellPhone", SqlDbType.NVarChar,20),
					new SqlParameter("@BusinessPhone", SqlDbType.NVarChar,20),
					new SqlParameter("@Fax", SqlDbType.NVarChar,20),
					new SqlParameter("@Email", SqlDbType.NVarChar,255), 
					new SqlParameter("@MailingAddr", SqlDbType.NVarChar,50),
					new SqlParameter("@MailingCity", SqlDbType.NVarChar,50),
					new SqlParameter("@MailingState", SqlDbType.NChar,2),
					new SqlParameter("@MailingZip", SqlDbType.NVarChar,12),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@ServiceTypes", SqlDbType.NVarChar,50),
					new SqlParameter("@ContactEnable", SqlDbType.Bit),
					new SqlParameter("@ContactCompanyId", SqlDbType.Int,4) };
            parameters[0].Value = model.ContactId;
            parameters[1].Value = model.FirstName;
            parameters[2].Value = model.LastName;
            parameters[3].Value = model.CellPhone;
            parameters[4].Value = model.BusinessPhone;
            parameters[5].Value = model.Fax;
            parameters[6].Value = model.Email;
            parameters[7].Value = model.MailingAddr;
            parameters[8].Value = model.MailingCity;
            parameters[9].Value = model.MailingState;
            parameters[10].Value = model.MailingZip;
            parameters[11].Value = model.MiddleName;
            parameters[12].Value = model.NickName;
            parameters[13].Value = model.ContactEnable;
            parameters[14].Value = model.ContactCompanyId;

            DbHelperSQL.RunProcedure("lpsp_ContactsUpdate", parameters, out rowsAffected);
        }

        public int AddClient(LPWeb.Model.Contacts model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@FirstName", SqlDbType.NVarChar,50),
                    new SqlParameter("@MiddleName", SqlDbType.NVarChar, 50),
					new SqlParameter("@LastName", SqlDbType.NVarChar,50), 
					new SqlParameter("@NickName", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@GenerationCode", SqlDbType.NVarChar,10),
					new SqlParameter("@SSN", SqlDbType.NVarChar,20),
                    new SqlParameter("@HomePhone", SqlDbType.NVarChar, 20),
                    new SqlParameter("@CellPhone", SqlDbType.NVarChar, 20),
                    new SqlParameter("@BusinessPhone", SqlDbType.NVarChar, 20),
                    new SqlParameter("@Fax", SqlDbType.NVarChar, 20),
                    new SqlParameter("@Email", SqlDbType.NVarChar, 255),
                    new SqlParameter("@Dob", SqlDbType.DateTime),
                    new SqlParameter("@Experian", SqlDbType.NVarChar, 20),
                    new SqlParameter("@TransUnion", SqlDbType.NVarChar, 20),
                    new SqlParameter("@Equifax", SqlDbType.NVarChar, 20),
					new SqlParameter("@MailingAddr", SqlDbType.NVarChar,50),
					new SqlParameter("@MailingCity", SqlDbType.NVarChar,50),
					new SqlParameter("@MailingState", SqlDbType.NChar,2),
					new SqlParameter("@MailingZip", SqlDbType.NVarChar,12),
					new SqlParameter("@ContactCompanyId", SqlDbType.Int),
					new SqlParameter("@WebAccountId", SqlDbType.Int)};
            parameters[0].Value = null;  // ContactId
            parameters[1].Value = model.FirstName;
            parameters[2].Value = model.MiddleName;
            parameters[3].Value = model.LastName;
            if (string.IsNullOrEmpty(model.NickName))
                parameters[4].Value = model.FirstName;
            else
                parameters[4].Value = model.NickName;
            parameters[5].Value = model.Title;
            parameters[6].Value = model.GenerationCode;
            parameters[7].Value = model.SSN;   // SSN
            parameters[8].Value = model.HomePhone;
            parameters[9].Value = model.CellPhone;
            parameters[10].Value = model.BusinessPhone;
            parameters[11].Value = model.Fax;
            parameters[12].Value = model.Email;
            parameters[13].Value = model.DOB;
            parameters[14].Value = model.Experian;  // Experian
            parameters[15].Value = model.TransUnion;  // TransUnion
            parameters[16].Value = model.Equifax;  // Equifax

            parameters[17].Value = model.MailingAddr;
            parameters[18].Value = model.MailingCity;
            parameters[19].Value = model.MailingState;
            parameters[20].Value = model.MailingZip;
            parameters[21].Value = null;    // ContactCompanyId
            parameters[22].Value = null;    // WebAccountId
            int iContactId = -1;
            iContactId = DbHelperSQL.RunProcedure("dbo.Contacts_Save", parameters, out rowsAffected);
            return iContactId;
        }

        /// <summary>
        /// create contact without check duplicated
        /// neo 2012-10-24
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddClientNoCheck(LPWeb.Model.Contacts model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@FirstName", SqlDbType.NVarChar,50),
                    new SqlParameter("@MiddleName", SqlDbType.NVarChar, 50),
					new SqlParameter("@LastName", SqlDbType.NVarChar,50), 
					new SqlParameter("@NickName", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@GenerationCode", SqlDbType.NVarChar,10),
					new SqlParameter("@SSN", SqlDbType.NVarChar,20),
                    new SqlParameter("@HomePhone", SqlDbType.NVarChar, 20),
                    new SqlParameter("@CellPhone", SqlDbType.NVarChar, 20),
                    new SqlParameter("@BusinessPhone", SqlDbType.NVarChar, 20),
                    new SqlParameter("@Fax", SqlDbType.NVarChar, 20),
                    new SqlParameter("@Email", SqlDbType.NVarChar, 255),
                    new SqlParameter("@Dob", SqlDbType.DateTime),
                    new SqlParameter("@Experian", SqlDbType.NVarChar, 20),
                    new SqlParameter("@TransUnion", SqlDbType.NVarChar, 20),
                    new SqlParameter("@Equifax", SqlDbType.NVarChar, 20),
					new SqlParameter("@MailingAddr", SqlDbType.NVarChar,50),
					new SqlParameter("@MailingCity", SqlDbType.NVarChar,50),
					new SqlParameter("@MailingState", SqlDbType.NChar,2),
					new SqlParameter("@MailingZip", SqlDbType.NVarChar,12),
					new SqlParameter("@ContactCompanyId", SqlDbType.Int),
					new SqlParameter("@WebAccountId", SqlDbType.Int)};
            parameters[0].Value = null;  // ContactId
            parameters[1].Value = model.FirstName;
            parameters[2].Value = model.MiddleName;
            parameters[3].Value = model.LastName;
            if (string.IsNullOrEmpty(model.NickName))
                parameters[4].Value = model.FirstName;
            else
                parameters[4].Value = model.NickName;
            parameters[5].Value = model.Title;
            parameters[6].Value = model.GenerationCode;
            parameters[7].Value = model.SSN;   // SSN
            parameters[8].Value = model.HomePhone;
            parameters[9].Value = model.CellPhone;
            parameters[10].Value = model.BusinessPhone;
            parameters[11].Value = model.Fax;
            parameters[12].Value = model.Email;
            parameters[13].Value = model.DOB;
            parameters[14].Value = model.Experian;  // Experian
            parameters[15].Value = model.TransUnion;  // TransUnion
            parameters[16].Value = model.Equifax;  // Equifax

            parameters[17].Value = model.MailingAddr;
            parameters[18].Value = model.MailingCity;
            parameters[19].Value = model.MailingState;
            parameters[20].Value = model.MailingZip;
            parameters[21].Value = null;    // ContactCompanyId
            parameters[22].Value = null;    // WebAccountId
            int iContactId = -1;
            iContactId = DbHelperSQL.RunProcedure("dbo.Contacts_SaveNoCheck", parameters, out rowsAffected);
            return iContactId;
        }

        /// <summary>
        /// line co-borrower to loan
        /// neo 2012-10-24
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="iCoBorrowerID"></param>
        public void LinkCoBorrowerToLoan(int iFileId, int iCoBorrowerID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int),
					new SqlParameter("@ContactId", SqlDbType.Int)};
            parameters[0].Value = iFileId;  // ContactId
            parameters[1].Value = iCoBorrowerID;

            DbHelperSQL.RunProcedure("dbo.CoBorrower_Save", parameters, out rowsAffected);
        } 

        public void ADDContact(LPWeb.Model.Contacts model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@FirstName", SqlDbType.NVarChar,50),
					new SqlParameter("@LastName", SqlDbType.NVarChar,50), 
					new SqlParameter("@CellPhone", SqlDbType.NVarChar,20),
					new SqlParameter("@BusinessPhone", SqlDbType.NVarChar,20),
					new SqlParameter("@Fax", SqlDbType.NVarChar,20),
					new SqlParameter("@Email", SqlDbType.NVarChar,255), 
					new SqlParameter("@MailingAddr", SqlDbType.NVarChar,50),
					new SqlParameter("@MailingCity", SqlDbType.NVarChar,50),
					new SqlParameter("@MailingState", SqlDbType.NChar,2),
					new SqlParameter("@MailingZip", SqlDbType.NVarChar,12),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@ServiceTypes", SqlDbType.NVarChar,50),
					new SqlParameter("@ContactEnable", SqlDbType.Bit),
                    new SqlParameter("@ContactCompanyId", SqlDbType.Int,4),
                    new SqlParameter("@ContactBranchId", SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.ContactId;
            parameters[1].Value = model.FirstName;
            parameters[2].Value = model.LastName;
            parameters[3].Value = model.CellPhone;
            parameters[4].Value = model.BusinessPhone;
            parameters[5].Value = model.Fax;
            parameters[6].Value = model.Email;
            parameters[7].Value = model.MailingAddr;
            parameters[8].Value = model.MailingCity;
            parameters[9].Value = model.MailingState;
            parameters[10].Value = model.MailingZip;
            parameters[11].Value = model.MiddleName;
            parameters[12].Value = model.NickName;
            parameters[13].Value = model.ContactEnable;
            parameters[14].Value = model.ContactCompanyId;
            parameters[15].Value = model.ContactBranchId;

            DbHelperSQL.RunProcedure("lpsp_ContactsAdd", parameters, out rowsAffected);
        }

        /// <summary>
        /// Gets the prospects by file ids.
        /// </summary>
        /// <param name="contactIDs">The contact I ds.</param>
        /// <returns></returns>
        public DataSet GetProspectsByFileIds(string contactIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT		C.ContactId,
                                        ISNULL(C.Lastname+', ','')+ISNULL(C.Firstname,'') Client,  
                                        ISNULL(C.MailingAddr+',','')+ISNULL(C.MailingCity+',','')+ISNULL(C.MailingCity+',','')+ISNULL(C.MailingState+',','')+ISNULL(C.MailingZip,'') [Address],  
                                        C.SSN 
                                        FROM dbo.Contacts C  
                                        WHERE charindex(','+cast(C.ContactId as nvarchar)+',',','+@contactIDs+',')>0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@contactIDs", SqlDbType.NVarChar),
					 };
            parameters[0].Value = contactIDs;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            return ds;
        }

        /// <summary>
        /// Merges the prospects.
        /// </summary>
        /// <param name="froms">The froms.</param>
        /// <param name="to">To.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public bool MergeProspects(List<int> froms, int to, int userId)
        {

            using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //var query = from it in froms select string.Format("{0}", it);
                        //string ps = string.Join(",", query.ToArray());
                        //int ips = 0;
                        ////get contact name base on prospect id
                        //if (ps != "" && Int32.TryParse(ps, out ips))
                        //{
                        //    ps = this.GetContactName(ips);
                        //}
                        string ps = "";
                        SqlCommand command = new SqlCommand("lpsp_MergeProspect", connection, transaction);
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (int item in froms)
                        {
                            //get contact name base on prospect id
                            string contactName = this.GetContactName(item);
                            ps += (ps == "") ? contactName : ("; " + contactName);

                            command.Parameters.Clear();
                            command.Parameters.AddRange(new SqlParameter[]
                                                            {
                                                                new SqlParameter("@fromId",SqlDbType.Int,4), 
                                                                new SqlParameter("@toId",SqlDbType.Int,4)
                                                            
                                                            });
                            command.Parameters[0].Value = item;
                            command.Parameters[1].Value = to;
                            command.ExecuteNonQuery();
                        }

                        command = new SqlCommand("insert into ProspectActivities(ContactId,UserId,ActivityName,ActivityTime) values(@ContactId,@UserId,@ActivityName,getdate())", connection, transaction);
                        command.CommandType = CommandType.Text;
                        command.Parameters.Clear();
                        command.Parameters.AddRange(new SqlParameter[]
                                                        {
                                                            new SqlParameter("@ContactId",SqlDbType.Int,4), 
                                                            new SqlParameter("@UserId",SqlDbType.Int,4),
                                                            new SqlParameter("@ActivityName",SqlDbType.NVarChar)
                                                            
                                                        });

                        command.Parameters[0].Value = to;
                        command.Parameters[1].Value = userId;
                        command.Parameters[2].Value = string.Format("The prospect has been merged with <{0}>.", ps);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    transaction.Commit();
                    return true;
                }

            }

        }

        #region neo

        /// <summary>
        /// get contact info
        /// neo 2011-03-13
        /// </summary>
        /// <param name="iContactID"></param>
        /// <returns></returns>
        public DataTable GetContactInfoBase(int iContactID)
        {
            string sSql = "select * from Contacts where ContactID=" + iContactID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
        /// <summary>
        /// get borrower name
        /// 
        /// </summary>
        /// <param name="iContactID"></param>
        /// <returns></returns>
        public string GetBorrower(int iFileID)
        {
            string sSql = string.Format("select [dbo].[lpfn_GetBorrower]({0})", iFileID);
            SqlDataReader dataReader = null;
            string borrowerName = string.Empty;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(sSql);
                if (dataReader == null)
                    return borrowerName;

                while (dataReader.Read())
                {
                    borrowerName = dataReader.IsDBNull(0) ? string.Empty : dataReader.GetString(0);
                }
                return borrowerName;
            }
            catch (Exception ex)
            {
                return borrowerName;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }

        }
        /// <summary>
        /// get coborrower name
        /// 
        /// </summary>
        /// <param name="iContactID"></param>
        /// <returns></returns>
        public string GetCoBorrower(int iFileID)
        {
            string sSql = string.Format("select [dbo].[lpfn_GetCoborrower]({0})", iFileID);
            SqlDataReader dataReader = null;
            string coborrowerName = string.Empty;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(sSql);
                if (dataReader == null)
                    return coborrowerName;

                while (dataReader.Read())
                {
                    coborrowerName = dataReader.IsDBNull(0) ? string.Empty : dataReader.GetString(0);
                }
                return coborrowerName;
            }
            catch (Exception ex)
            {
                return coborrowerName;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
        }
        /// <summary>
        /// get contact name
        /// 
        /// </summary>
        /// <param name="iContactID"></param>
        /// <returns></returns>
        public string GetContactName(int iContactID)
        {
            string sSql = string.Format("select [dbo].[lpfn_GetContactName]({0})", iContactID);
            SqlDataReader dataReader = null;
            string contactName = string.Empty;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(sSql);
                if (dataReader == null)
                    return contactName;

                while (dataReader.Read())
                {
                    contactName = dataReader.IsDBNull(0) ? string.Empty : dataReader.GetString(0);
                }
                return contactName;
            }
            catch (Exception ex)
            {
                return contactName;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }

        }

        /// <summary>
        /// update Contacts.UpdatePoint= 0 or 1
        /// neo 2011-03-13
        /// </summary>
        /// <param name="iContactID"></param>
        /// <param name="bUpdatePoint"></param>
        public void UpdatePointBase(int iContactID, bool bUpdatePoint)
        {
            string sSql = "update Contacts set UpdatePoint=@UpdatePoint where ContactID=" + iContactID;
            SqlCommand SqlCmd = new SqlCommand(sSql);

            DbHelperSQL.AddSqlParameter(SqlCmd, "@UpdatePoint", SqlDbType.Bit, bUpdatePoint);
            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        /// <summary>
        /// get related contact count
        /// neo 2011-03-15
        /// </summary>
        /// <param name="ContactId"></param>
        /// <returns></returns>
        public int GetRelatedContactCountBase(int ContactId)
        {
            string sSql = "select 'To' as Direction, a.ToContactId as RelContactID, a.RelTypeId, c.RelToName as Relationship, b.LastName +', '+ b.FirstName + case when b.MiddleName is null then '' when b.MiddleName='' then '' else ' '+ b.MiddleName end as ContactName "
                        + "from Contact_Relationship as a "
                        + "inner join Contacts as b on a.ToContactId = b.ContactId "
                        + "inner join RelationshipRoles as c on a.RelTypeId = c.RelTypeId "
                        + "where a.FromContactId = " + ContactId + " "
                        + "union "
                        + "select 'From' as Direction, a.FromContactId as RelContactID, a.RelTypeId, c.RelFromName as Relationship, b.LastName +', '+ b.FirstName + case when b.MiddleName is null then '' when b.MiddleName='' then '' else ' '+ b.MiddleName end as ContactName "
                        + "from Contact_Relationship as a "
                        + "inner join Contacts as b on a.FromContactId = b.ContactId "
                        + "inner join RelationshipRoles as c on a.RelTypeId = c.RelTypeId "
                        + "where a.ToContactId = " + ContactId;

            // row count
            int iRowCount = DbHelperSQL.Count("(" + sSql + ") as t", "");

            return iRowCount;
        }

        /// <summary>
        /// get related contacts
        /// neo 2011-03-15
        /// </summary>
        /// <param name="ContactId"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <returns></returns>
        public DataTable GetRelatedContacts(int ContactId, int iStartIndex, int iEndIndex)
        {
            string sSql = "select 'To' as Direction, a.ToContactId as RelContactID, a.RelTypeId, c.RelToName as Relationship, b.LastName +', '+ b.FirstName + case when b.MiddleName is null then '' when b.MiddleName='' then '' else ' '+ b.MiddleName end as ContactName "
                        + "from Contact_Relationship as a "
                        + "inner join Contacts as b on a.ToContactId = b.ContactId "
                        + "inner join RelationshipRoles as c on a.RelTypeId = c.RelTypeId "
                        + "where a.FromContactId = " + ContactId + " "
                        + "union "
                        + "select 'From' as Direction, a.FromContactId as RelContactID, a.RelTypeId, c.RelFromName as Relationship, b.LastName +', '+ b.FirstName + case when b.MiddleName is null then '' when b.MiddleName='' then '' else ' '+ b.MiddleName end as ContactName "
                        + "from Contact_Relationship as a "
                        + "inner join Contacts as b on a.FromContactId = b.ContactId "
                        + "inner join RelationshipRoles as c on a.RelTypeId = c.RelTypeId "
                        + "where a.ToContactId = " + ContactId;

            SqlCommand SqlCmd = new SqlCommand("lpsp_ExecSqlByPager");
            SqlCmd.CommandType = CommandType.StoredProcedure;

            DbHelperSQL.AddSqlParameter(SqlCmd, "@OrderByField", SqlDbType.NVarChar, "Direction, RelContactID");
            DbHelperSQL.AddSqlParameter(SqlCmd, "@AscOrDesc", SqlDbType.NVarChar, "asc");
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Fields", SqlDbType.NVarChar, "*");
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DbTable", SqlDbType.NVarChar, "(" + sSql + ") as t");
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Where", SqlDbType.NVarChar, "");
            DbHelperSQL.AddSqlParameter(SqlCmd, "@StartIndex", SqlDbType.Int, iStartIndex);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@EndIndex", SqlDbType.Int, iEndIndex);

            return DbHelperSQL.ExecuteDataTable(SqlCmd);
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
        /// <param name="sLeadSource"></param>
        /// <param name="sReferralID"></param>
        public void UpdateContactProspectInfo(int iContactId, string sFirstName, string sLastName, string sEmail, string sCellPhone, string sHomePhone, string sWorkPhone, string sDOB, string sLeadSource, string sReferralID)
        {
            string sSql = "update Contacts set FirstName=@FirstName, LastName=@LastName, Email=@Email, CellPhone=@CellPhone, HomePhone=@HomePhone, BusinessPhone=@BusinessPhone, DOB=@DOB where ContactId=@ContactId;"
                        + "update Prospect set LeadSource=@LeadSource, Referral=@Referral where ContactId=@ContactId";
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


            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LeadSource", SqlDbType.NVarChar, sLeadSource);
            if (sReferralID == string.Empty)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Referral", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Referral", SqlDbType.Int, Convert.ToInt32(sReferralID));
            }
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@ContactId", SqlDbType.Int, iContactId);

            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        #endregion

        /// <summary>
        /// get relationship name
        /// shawn 2011-03-13
        /// </summary>
        public DataSet GetRelationship(int ContactId1, int ContactId2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT rsr.RelFromName+','+rsr.RelToName FROM [Contact_Relationship] rs INNER JOIN RelationshipRoles rsr ON rs.RelTypeId = rsr.RelTypeId");
            strSql.Append(" WHERE (rs.[FromContactId]=@ContactId1 OR rs.[FromContactId]=@ContactId2) AND (rs.[ToContactId]=@ContactId1 OR rs.[ToContactId]=@ContactId2) ");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId1", SqlDbType.Int),
					new SqlParameter("@ContactId2", SqlDbType.Int)};
            parameters[0].Value = ContactId1;
            parameters[1].Value = ContactId2;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            return ds;
        }

        /// <summary>
        /// get related contacts 
        /// </summary>
        /// <param name="ContactId"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <returns></returns>
        public DataTable GetRelatedContacts(int ContactId)
        {
            string sSql = "select 'To' as Direction, a.ToContactId as RelContactID, a.RelTypeId, c.RelToName as Relationship, b.LastName +', '+ b.FirstName + case when b.MiddleName is null then '' when b.MiddleName='' then '' else ' '+ b.MiddleName end as ContactName "
                        + "from Contact_Relationship as a "
                        + "inner join Contacts as b on a.ToContactId = b.ContactId "
                        + "inner join RelationshipRoles as c on a.RelTypeId = c.RelTypeId "
                        + "where a.FromContactId = " + ContactId + " "
                        + "union "
                        + "select 'From' as Direction, a.FromContactId as RelContactID, a.RelTypeId, c.RelFromName as Relationship, b.LastName +', '+ b.FirstName + case when b.MiddleName is null then '' when b.MiddleName='' then '' else ' '+ b.MiddleName end as ContactName "
                        + "from Contact_Relationship as a "
                        + "inner join Contacts as b on a.FromContactId = b.ContactId "
                        + "inner join RelationshipRoles as c on a.RelTypeId = c.RelTypeId "
                        + "where a.ToContactId = " + ContactId;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetContactInfoByBranchID(string sContactBranchID)
        {
            string sSql = "select ContactId,ISNULL(Lastname,'')+ (CASE ISNULL(Lastname,'') when '' then ' ' else ', ' end) +ISNULL(Firstname,'')  as Contact from Contacts where ContactBranchId=" + sContactBranchID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetEnableContactInfo()
        {
            string sSql = "select ContactId,ISNULL(Lastname,'')+ (CASE ISNULL(Lastname,'') when '' then ' ' else ', ' end) +ISNULL(Firstname,'')  as Contact from Contacts where isnull([Enabled],0)=1";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public int FormatCellPhone(string sContactBranchID)
        {
            string sSql = string.Format("select ContactId, CellPhone from contacts where contactbranchid={0}", sContactBranchID);

            try
            {
                DataTable dt = DbHelperSQL.ExecuteDataTable(sSql);
                if ((dt == null) || (dt.Rows.Count == 0))
                {
                    return -1;
                }

                int iContactID = 0;
                string CellPhone = "";
                string raw_data = "";
                string processed_data = " ";

                foreach (DataRow dtRow in dt.Rows)
                {
                    iContactID = (int)dtRow["ContactId"];
                    CellPhone = dtRow["CellPhone"].ToString();

                    if (CellPhone != null)
                    {
                        raw_data = CellPhone.Trim();

                        raw_data = System.Text.RegularExpressions.Regex.Replace(raw_data, @"[-() ]", String.Empty);

                        if (raw_data.Length > 10)
                        {
                            raw_data = raw_data.Substring(0, 10);
                        }

                        if (raw_data.Length == 10)
                        {
                            processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 4);
                        }
                        else if (raw_data.Length == 9)
                        {
                            processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 3);
                        }
                        else if (raw_data.Length == 8)
                        {
                            processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 2);
                        }
                        else if (raw_data.Length == 7)
                        {
                            processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 1);
                        }
                        else if (raw_data.Length == 6)
                        {
                            processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-";
                        }
                        else if (raw_data.Length == 5)
                        {
                            processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 2) + " -";
                        }
                        else if (raw_data.Length == 4)
                        {
                            processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 1) + "  -";
                        }
                        else if (raw_data.Length == 3)
                        {
                            processed_data = "(" + raw_data.Substring(0, 3) + ") " +  "   -";
                        }
                        else if (raw_data.Length == 2)
                        {
                            processed_data = "(" + raw_data.Substring(0, 2) + " ) " + "   -";
                        }
                        else if (raw_data.Length == 1)
                        {
                            processed_data = "(" + raw_data.Substring(0, 1) + "  ) " + "   -";
                        }
                        else
                        {
                            processed_data = "(   )    -    ";
                        }
                    }
                    else
                    {
                        processed_data = "(   )    -    ";
                    }

                    CellPhone = processed_data;

                    string sSqla = "update Contacts set CellPhone=@CellPhone where ContactID=" + iContactID;
                    SqlCommand SqlCmd = new SqlCommand(sSqla);

                    DbHelperSQL.AddSqlParameter(SqlCmd, "@CellPhone", SqlDbType.NVarChar, CellPhone);
                    DbHelperSQL.ExecuteNonQuery(SqlCmd);

                }
            }
            catch (Exception ex)
            {
                return -1;
            }

            return 0;
        }

        public DataTable GetEnableCompanyContactInfo(int companyid, int branchid)
        {
            string sSql = string.Format("select ContactId,ISNULL(Lastname,'')+ (CASE ISNULL(Lastname,'') when '' then ' ' else ', ' end) +ISNULL(Firstname,'')  as Contact from Contacts where (isnull([Enabled],0)=1) and (ContactCompanyId={0} or ContactBranchId={1})", companyid, branchid);
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataSet GetContactsByIds(string sContacts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT		C.ContactId,
                                        ISNULL(C.Lastname+', ','')+ISNULL(C.Firstname,'') Contact,  
                                        [dbo].[lpfn_GetContactServiceType](C.ContactId) [Service Type],
                                        [dbo].[lpfn_GetContactBranchName](C.ContactId) [Branch],
                                        [dbo].[lpfn_GetContactCompanyName](C.ContactId) [Company]
                                        FROM dbo.Contacts C  
                                        WHERE charindex(','+cast(C.ContactId as nvarchar)+',',','+@contactIDs+',')>0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@contactIDs", SqlDbType.NVarChar),
					 };
            parameters[0].Value = sContacts;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            return ds;
        }

        public bool MergeContacts(List<int> iFroms, int iTo, int iUserId)
        {
            using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string sContactNames = "";
                        SqlCommand command = new SqlCommand("lpsp_MergeContact", connection, transaction);
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (int item in iFroms)
                        {
                            //get contact name base on contact id
                            string contactName = this.GetContactName(item);
                            sContactNames += (sContactNames == "") ? contactName : ("; " + contactName);

                            command.Parameters.Clear();
                            command.Parameters.AddRange(new SqlParameter[]
                                                            {
                                                                new SqlParameter("@fromId",SqlDbType.Int,4), 
                                                                new SqlParameter("@toId",SqlDbType.Int,4)
                                                            
                                                            });
                            command.Parameters[0].Value = item;
                            command.Parameters[1].Value = iTo;
                            command.ExecuteNonQuery();
                        }

                        command = new SqlCommand("insert into ContactActivities(ContactId,UserId,ActivityName,ActivityTime) values(@ContactId,@UserId,@ActivityName,getdate())", connection, transaction);
                        command.CommandType = CommandType.Text;
                        command.Parameters.Clear();
                        command.Parameters.AddRange(new SqlParameter[]
                                                        {
                                                            new SqlParameter("@ContactId",SqlDbType.Int,4), 
                                                            new SqlParameter("@UserId",SqlDbType.Int,4),
                                                            new SqlParameter("@ActivityName",SqlDbType.NVarChar)
                                                            
                                                        });

                        command.Parameters[0].Value = iTo;
                        command.Parameters[1].Value = iUserId;
                        command.Parameters[2].Value = string.Format("The contact has been merged with <{0}>.", sContactNames);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    transaction.Commit();
                    return true;
                }

            }
        }

        ///  
        /// </summary>
        public void PartnerContactsDelete(int ContactId,int UserType ,int UserID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
					new SqlParameter("@ContactID", SqlDbType.Int,4),
					new SqlParameter("@UserType", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = ContactId;
            parameters[1].Value = UserType;
            parameters[2].Value = UserID;

            DbHelperSQL.RunProcedure("lpsp_PartnerContactDelete", parameters, out rowsAffected);
        }
         
        /// <summary>
        ///  
        /// </summary>
        public DataSet SearchContacts(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ContactId FROM lpvw_PartnerContactsSearch ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// If Contacts Exsit Email Info
        /// </summary>
        /// <param name="sContactIDs"></param>
        /// <returns></returns>
        public string GetContactsEmailInfo(string sContactIDs)
        {
            string sEmailNullInfo = "";
            string sSql = "select (ISNULL(Lastname,'')+', '+ISNULL(Firstname,'')+' '+ISNULL(Middlename,'')) as BorrowerName, Email from Contacts where ContactId in(" + sContactIDs + ")";
            DataTable dt= DbHelperSQL.ExecuteDataTable(sSql);
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[1] == null || dr[1].ToString() == "")
                {
                    sEmailNullInfo += " " + dr[0].ToString() + " ;"; //" does not have an email address. ";
                }
            }
            if (sEmailNullInfo != "")
            {
                sEmailNullInfo = sEmailNullInfo.TrimEnd(';');
            }
            return sEmailNullInfo;
        }

        public string vCardToString(int ContactId, bool Client)
        {
            string vcardTMP = @"BEGIN:VCARD
VERSION:3.0
N;CHARSET=UTF-8:{0};{1};;;
FN;CHARSET=UTF-8:{2}
TEL;TYPE=CELL:{3}
TEL;TYPE=HOME:{4}
TEL;TYPE=WORK:{5}
TEL;TYPE=FAX:{6}
EMAIL;TYPE=HOME:{7}
ADR;TYPE=HOME;CHARSET=UTF-8:;;{8};{9};;{10};{11};
ORG;CHARSET=UTF-8:{12};{13}
TITLE;CHARSET=UTF-8:{14}
NOTE;CHARSET=UTF-8:{15}
X-WDJ-STARRED:0
BDAY:{16}
END:VCARD";

            var model = GetModel(ContactId);

            if (model == null)
            {
                return "";
            }
            var companyName = "";
            var companyServiceType = "";
            var note = "";
            if (!Client)
            {
                DAL.ContactCompanies dal = new ContactCompanies();

                if (model.ContactCompanyId != null)
                {
                    var companyModel = dal.GetModel(model.ContactCompanyId.Value);

                    companyName = companyModel.Name;

                    companyServiceType = companyModel.ServiceTypes;
                }

                //DAL.Prospect prospectDal = new Prospect();
                //var prospectModel = prospectDal.GetModel(ContactId);
                //prospectModel.f

                //PartnerNotes 

                DAL.ContactNotes cnDal = new ContactNotes();
                int rcount = 0;
                var dsCNote = cnDal.GetContactNotes(1, 1, " 1=1", out rcount, "ContactNoteId", 1);
                if (dsCNote != null && dsCNote.Tables[0].Rows.Count > 0 && dsCNote.Tables[0].Rows[0]["Note"] != DBNull.Value)
                {
                    note = dsCNote.Tables[0].Rows[0]["Note"].ToString();
                }
            }
            else
            {
                DAL.ProspectNotes pnDal = new ProspectNotes();
                int count =0;
                var dsNote = pnDal.GetProspectNotes(1, 1, "", out count, "NoteId", 1, ContactId);
                if (dsNote != null && dsNote.Tables[0].Rows.Count > 0 && dsNote.Tables[0].Rows[0]["Note"] != DBNull.Value)
                {
                    note = dsNote.Tables[0].Rows[0]["Note"].ToString();
                }

            }

            string sDOB = string.Empty;
            if (model.DOB != null)
            {
                sDOB = Convert.ToDateTime(model.DOB).ToString("yyyyMMdd");
            }

            var str = string.Format(vcardTMP
                   , model.FirstName
                   , model.LastName
                   , model.LastName + "," + model.FirstName
                   , model.CellPhone
                   , model.HomePhone
                   , model.BusinessPhone
                   , model.Fax
                   , model.Email
                   , model.MailingAddr //{8}
                   , model.MailingCity //{9}
                   , model.MailingZip
                   , "United States" //国家 
                   , companyName
                   , companyServiceType
                   , model.Title
                   , note
                   , sDOB
                   );

            return str;
        }

    }
}

