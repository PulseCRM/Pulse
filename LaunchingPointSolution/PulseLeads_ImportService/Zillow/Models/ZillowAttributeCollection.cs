using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using PulseLeads.Zillow.Controllers;
using PulseLeads.Zillow._Code;
using focusIT;

namespace PulseLeads.Zillow.Models
{
    public class ZillowAttributeCollection
    {
        public Guid ImportTransId { get; set; }
        public string PulseSecurityToken { get; set; }
        public string XmlVersion { get; set; }
        public string ReturnStatus { get; set; }
        public string ZillowXml { get; set; }
        public int BorrowerId { get; set; }
        public int CoborrowerId { get; set; }
        public int LoanId { get; set; }
        public string CreditScore { get; set; }
        public List<ZillowAttribute> ZillowAttributes { get; set; }

        public ZillowAttributeCollection()
        {
            ImportTransId = Guid.NewGuid();
            ZillowAttributes = new List<ZillowAttribute>();
        }

        public void Save()
        {
            try
            {
                using (SqlConnection conn = DbHelperSQL.GetOpenConnection())
                {
                    using (SqlTransaction tran = conn.BeginTransaction(ImportTransId.ToString().Substring(0,8)))
                    {
                        try
                        {
                            SaveImport(conn, tran);
                            SaveAttributes(conn, tran);
                            tran.Commit();
                            NLogger.Info(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, "ZillowImport (" + ImportTransId.ToString() + ") completed.");
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback(ImportTransId.ToString());
                            NLogger.Error(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ex);
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NLogger.Error(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        private void SaveImport(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = new SqlCommand(@"insert into ZillowImports (ImportDate, ReturnStatus, ZillowXml, ImportTransId) values (@a, @b, @c, @d)", conn);
            DbHelperSQL.AddSqlParameter(cmd, "@a", SqlDbType.DateTime, DateTime.Now);
            DbHelperSQL.AddSqlParameter(cmd, "@b", SqlDbType.NVarChar, Helpers.FormatSqlStringValue(ReturnStatus, 50));
            DbHelperSQL.AddSqlParameter(cmd, "@c", SqlDbType.NText, ZillowXml);
            DbHelperSQL.AddSqlParameter(cmd, "@d", SqlDbType.UniqueIdentifier, ImportTransId);
            DbHelperSQL.ExecuteNonQuery(cmd, tran);
        }
        
        private void SaveAttributes(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                foreach (ZillowAttribute attribute in ZillowAttributes)
                {
                    SqlCommand cmd = new SqlCommand(@"insert into ZillowAttributes (ImportTransId, Version, GroupName, AttributeName, AttributeValue, ImportedFlag, BorrowerId, CoborrowerId, LoanId) values (@a, @b, @c, @d, @e, @f, @g, @h, @i)", conn);
                    DbHelperSQL.AddSqlParameter(cmd, "@a", SqlDbType.UniqueIdentifier, ImportTransId);
                    DbHelperSQL.AddSqlParameter(cmd, "@b", SqlDbType.NVarChar, Helpers.FormatSqlStringValue(XmlVersion, 10));
                    DbHelperSQL.AddSqlParameter(cmd, "@c", SqlDbType.NVarChar, Helpers.FormatSqlStringValue(attribute.GroupName, 255));
                    DbHelperSQL.AddSqlParameter(cmd, "@d", SqlDbType.NVarChar, Helpers.FormatSqlStringValue(attribute.AttributeName, 255));
                    DbHelperSQL.AddSqlParameter(cmd, "@e", SqlDbType.NVarChar, Helpers.FormatSqlStringValue(attribute.AttributeValue, 2000));
                    DbHelperSQL.AddSqlParameter(cmd, "@f", SqlDbType.Bit, attribute.ImportedFlag);
                    DbHelperSQL.AddSqlParameter(cmd, "@g", SqlDbType.Int, BorrowerId);
                    DbHelperSQL.AddSqlParameter(cmd, "@h", SqlDbType.Int, CoborrowerId);
                    DbHelperSQL.AddSqlParameter(cmd, "@i", SqlDbType.Int, LoanId);
                    DbHelperSQL.ExecuteNonQuery(cmd, tran);
                }
            }
            catch (Exception ex)
            {
                NLogger.Error(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

    }
}