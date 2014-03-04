using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类ContactRoles。
	/// </summary>
    public class ContactRoles : ContactRolesBase
	{
		public ContactRoles()
        { }

        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = "ContactRoles";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "1=1 " + strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        /// <summary>
        /// get contact role list
        /// neo 2010-12-09
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetContactRoleListBase(string sWhere)
        {
            string sSql = "select * from ContactRoles where 1=1 " + sWhere;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public bool IsContactRoleNameExsits(string strName)
        {
            string strSql = string.Format("SELECT COUNT(1) FROM ContactRoles WHERE Name='{0}'", strName);
            object obj = DbHelperSQL.ExecuteScalar(strSql);
            int n = 0;
            if (!int.TryParse(obj.ToString(), out n))
                return false;
            if (n > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listIds"></param>
        public void DeleteContactRole(List<int> listIds)
        {
            string strSql = "";
            string strWhere = GetIDsWhereClauseFromList(listIds);
            if (strWhere.Length > 0)
            {
                strSql = string.Format("DELETE LoanContacts WHERE ContactRoleId IN ({0});DELETE ContactRoles WHERE ContactRoleId IN ({0});",
                    strWhere);
            }

            if (strSql.Length > 0)
            {
                DbHelperSQL.ExecuteNonQuery(strSql);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listIds"></param>
        public void EnableContactRole(List<int> listIds)
        {
            string strSql = "";
            string strWhere = GetIDsWhereClauseFromList(listIds);
            if (strWhere.Length > 0)
            {
                strSql = string.Format("UPDATE ContactRoles SET Enabled=1 WHERE ContactRoleId IN ({0});",
                    strWhere);
            }

            if (strSql.Length > 0)
            {
                DbHelperSQL.ExecuteNonQuery(strSql);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listIds"></param>
        public void DisableContactRole(List<int> listIds)
        {
            string strSql = "";
            string strWhere = GetIDsWhereClauseFromList(listIds);
            if (strWhere.Length > 0)
            {
                strSql = string.Format("UPDATE ContactRoles SET Enabled=0 WHERE ContactRoleId IN ({0});",
                    strWhere);
            }

            if (strSql.Length > 0)
            {
                DbHelperSQL.ExecuteNonQuery(strSql);
            }
        }

        private string GetIDsWhereClauseFromList(List<int> list)
        {
            StringBuilder sbIds = new StringBuilder();
            foreach (int n in list)
            {
                if (sbIds.Length > 0)
                    sbIds.Append(",");
                sbIds.AppendFormat("'{0}'", n);
            }
            return sbIds.ToString();
        }
	}
}

