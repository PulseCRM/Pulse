using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:ProspectIncome
    /// </summary>
    public partial class ProspectIncome : ProspectIncomeBase
    {
        private static readonly string insertSql = "INSERT ProspectIncome(EmplId,Salary) VALUES({0},{1})";
        private static readonly string existSql = "SELECT COUNT(1) FROM ProspectIncome WHERE Emplid ={0}";
        private static readonly string updateSql = "UPDATE ProspectIncome SET salary={0} WHERE Emplid={1}";
        public void UpdateIncome(int emplid, string salary)
        {
            DbHelperSQL.ExecuteNonQuery(string.Format(updateSql, salary, emplid));
        }

        public void InsertIncome(int emplid, string salary)
        {
            DbHelperSQL.ExecuteNonQuery(string.Format(insertSql, emplid, salary));
        }

        public bool ExistIncome(int emplid)
        {
            DataTable dt = DbHelperSQL.ExecuteDataTable(string.Format(existSql, emplid));
            if (dt != null && dt.Rows.Count == 1)
            {
                return Convert.ToInt32(dt.Rows[0][0].ToString()) == 1;
            }
            return false;
        }

        public DataTable GetProspectIncome(int iContactID)
        {
            string sSql = "select * from ProspectIncome where ContactId=" + iContactID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
    }
}


