using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Roles。
	/// </summary>
    public class Roles : RolesBase
	{
		public Roles()
		{}

        /// <summary>
        /// 获取Role List
        /// neo 2010-12-07
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetRoleListBase(string sWhere)
        {
            string sSql = "select * from Roles where 1=1 " + sWhere;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// 获取recipient role list
        /// neo 2010-12-08
        /// </summary>
        /// <returns></returns>
        public DataTable GetRecipientRoleListBase()
        {
            string sSql = "select ContactRoleId as RoleID, 'Contact' as RecipientType, Name as RecipientRole from ContactRoles "
                        + "union "
                        + "select RoleID, 'User' as RecipientType, Name as RecipientRole from Roles where RoleId not in (1,2)";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }


        public DataTable GetRoleByUserID(string sUserID)
        {
            string sSql = "SELECT Roles.Name, Users.UserId FROM Users INNER JOIN Roles ON  Users.RoleId = Roles.RoleId where Users.UserId ='" + sUserID+"'";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public DataTable GetRoleByUserID(int iUserId)
        {
            string sSql = "SELECT Roles.AccessAllMailChimpList,Roles.Name, Users.UserId FROM Users INNER JOIN Roles ON  Users.RoleId = Roles.RoleId where Users.UserId ='" + iUserId.ToString(CultureInfo.InvariantCulture) + "'";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
    }
}

