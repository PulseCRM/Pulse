using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Company_General。
    /// </summary>
    public class Company_General : Company_GeneralBase
    {
        public Company_General()
        { }

        public DataSet GetUserForCompanyOverView(int UserID)
        {
            DataSet ds = new DataSet();
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int) };
            parameters[0].Value = UserID;
            ds = DbHelperSQL.RunProcedure("[lpsp_GetUserForCompanyOverview]", parameters, "ds");

            return ds;
        }

        public DataTable GetCompanyOverviewByUser(int Level, int typeID, string type, string strWhere)
        {
            string sColumns = string.Empty;
            if (Level == 0)
            {
                sColumns = "[CompanyName] ";
            }
            else if (Level == 1)
            {
                sColumns = "[CompanyName],[RegionName] ";
            }
            else if (Level == 2)
            {
                sColumns = "[CompanyName],[RegionName] ,[DivisionName] ";
            }
            else if (Level == 3)
            {
                sColumns = " [CompanyName],[RegionName] ,[DivisionName],[BranchName] ";
            }
            if (strWhere.Length > 0)
            {
                strWhere = " WHERE " + strWhere;
            }
            string TableName = string.Empty;
            if (type == "User")
            {
                TableName = "lpfn_GetCompanyOverviewByUser";
            }
            else if (type == "Group")
            {
                TableName = "lpfn_GetCompanyOverviewByGroup";
            }
            else if (type == "Folder")
            {
                TableName = "lpfn_GetCompanyOverviewByPointFolder";
            }
            else if (type == "File")
            {
                TableName = "lpfn_GetCompanyOverviewByPointFile";
            }
            string sSql = sSql = string.Format(@"SELECT {0} ,MAX([OrganizationType]) AS OrganizationType FROM [{1}]({2}) {3} GROUP BY {4}", sColumns, TableName, typeID.ToString(), strWhere, sColumns);

            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public bool CheckMarketingEnabled()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT TOP 1 EnableMarketing FROM dbo.Company_General");

            object obj = DbHelperSQL.ExecuteScalar(strSql.ToString());
            bool MarketingEnabled = obj == DBNull.Value ? false : (bool)obj;
            return MarketingEnabled;
        }
        public void UpdateMarketingEnabled(bool status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("UPDATE dbo.Company_General SET EnableMarketing={0}", status ? 1 : 0);

            DbHelperSQL.ExecuteSql(strSql.ToString());
        }
    }
}

