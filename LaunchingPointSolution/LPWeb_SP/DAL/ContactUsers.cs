using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类ContactUsers。
	/// </summary>
    public class ContactUsers : ContactUsersBase
	{
		public ContactUsers()
		{}

        public int GetUserContactCount(int nUserId)
        {
            string strSql = string.Format("SELECT dbo.lpfn_GetUserContactCount({0})", nUserId);
            object objResult = DbHelperSQL.ExecuteScalar(strSql);
            int nResult = 0;
            if (!int.TryParse(string.Format("{0}", objResult), out nResult))
                nResult = 0;
            return nResult;
        }
	}
}

