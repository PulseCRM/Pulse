using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:ProspectEmployment
    /// </summary>
    public class ProspectEmployment : ProspectEmploymentBase
    {
        public DataSet GetProspectEmployment(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            string TableName = string.Empty;
            if (strWhere.IndexOf("FileID") > -1)
            {
                TableName = "lpvw_ProspectEmployment";
            }
            else
            {
                TableName = " ( SELECT distinct [ContactsName] ,[ContactId],[EmplId],[SelfEmployed],[Position],[StartYear],[StartMonth],[EndYear],[EndMonth],[YearsOnWork],[Phone],[ContactBranchId],[CurrentMY],[CompanyName] FROM [lpvw_ProspectEmployment] ) tmtb ";
            }
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
            parameters[0].Value = TableName;
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

        public DataSet ProspectContacts(string strWhere)
        {
            string strSQL = @"SELECT distinct [ContactsName],[ContactId] FROM (
	SELECT 
		Contacts.LastName + ', ' + Contacts.FirstName as ContactsName, 
		Contacts.ContactId, 
		dbo.LoanContacts.FileId
	FROM LoanContacts 
	INNER JOIN ContactRoles 
		ON LoanContacts.ContactRoleId = ContactRoles.ContactRoleId AND (ContactRoles.Name='Borrower' or ContactRoles.Name='CoBorrower')
	INNER JOIN Contacts 
		ON LoanContacts.ContactId = Contacts.ContactId 
    INNER JOIN Prospect 
	ON Contacts.ContactId = Prospect.[ContactId]
) tmtb where " + strWhere;

            SqlParameter[] parameters = {  
					};

            var ds = DbHelperSQL.Query(strSQL, parameters);

            return ds;
        }


        public DataTable GetEmployment(int emplid)
        {
            string querySql = "SELECT T1.CompanyName AS EmploymentName,"
                                                                           + " T1.SelfEmployed,"
                                                                           + " T1.Position,"
                                                                            + " T1.StartMonth,"
                                                                            + " T1.StartYear,"
                                                                            + " T1.EndMonth,"
                                                                            + " T1.EndYear,"
                                                                           + " T2.Salary,"
                                                                           + " T1.Phone,"
                                                                           + " T1.YearsOnWork,"
                                                                           + " T1.[Address] AS [Address],"
                                                                           + " T1.City AS City,"
                                                                           + " T1.[State] AS [State],"
                                                                           + " T1.Zip AS Zip,"
                                                                           + " T1.ContactBranchId,"
                                                                           + " T1.BusinessType,"
                                                                           + " T1.VerifyYourTaxes"
                                                                           + " FROM ProspectEmployment AS T1"
                                                                           + " LEFT JOIN ProspectIncome T2 ON T1.EmplId = T2.EmplId"
                                                                           + " WHERE T1.Emplid = {0}";
            return DbHelperSQL.ExecuteDataTable(string.Format(querySql, emplid));
        }

        public void UpdateProspectEmploymentAndProspectIncome(int FileId, string Employer, string StartMonth, string Dependants, string StartYear, string TP, string EndDate, string MonthlySalary, string EndYear, string Profession, string YearsInField)
        {
            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                bool ms = true;
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                if (Dependants == "Yes")
                {
                    ms = true;
                }
                else
                {
                    ms = false;
                }

                string sSqlNote = "update ProspectEmployment set SelfEmployed=" + ms + ",Position='" + TP + "',StartYear='" + StartYear + "',StartMonth='" + StartMonth + "',EndYear='" + EndYear + "',EndMonth='" + EndDate + "',YearsOnWork='" + YearsInField + "',CompanyName='" + Employer + "',BusinessType='" + Profession + "' where ContactId=" + FileId + "";
                int LoanNotes = LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSqlNote);

                string sSqlProspectIncome = "update ProspectIncome set Salary='" + MonthlySalary + "' where ContactId=" + FileId + "";
                int ProspectIncome = LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSqlProspectIncome);

             

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

        public void InsertProspectEmploymentAndProspectIncome(int FileId, string Employer, string StartMonth, string Dependants, string StartYear, string TP, string EndDate, string MonthlySalary, string EndYear, string Profession, string YearsInField)
        {
            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                bool ms = true;
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                if (Dependants == "Yes")
                {
                    ms = true;
                }
                else
                {
                    ms = false;
                }

                string sSqlNote = "insert into ProspectEmployment(ContactId,SelfEmployed,Position,StartYear,StartMonth,EndYear,EndMonth,YearsOnWork,CompanyName,BusinessType) values(" + FileId + "," + ms + ",'" + TP + "','" + StartYear + "','" + StartMonth + "','" + EndYear + "','" + EndDate + "','" + YearsInField + "','" + Employer + "','" + Profession + "')";
                int LoanNotes = LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSqlNote);

                string sSqlProspectIncome = "insert into ProspectIncome(ContactId,Salary) values(" + FileId + ",'" + MonthlySalary + "')";
                int ProspectIncome = LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSqlProspectIncome);



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

        public DataTable GetProspectEmployment(int iContactID)
        {
            string sSql = "select * from ProspectEmployment where ContactId=" + iContactID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
    }
}

