using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LPWeb.DAL
{
    public class UserRecentItems : UserRecentItemsBase
    {
        public UserRecentItems()
		{}

        public DataSet GetUserRecentItems(int iUserID)
        {
            string sSql = "select top 10 * from UserRecentItems where UserId=" + iUserID + " order by LastAccessed desc";
            return DbHelperSQL.Query(sSql);
        }

        public string GetUserRecentItemsBorrowerInfo(int iFileID)
        {
            string sSql = "select dbo.lpfn_GetBorrower(" + iFileID + ")";
            object obj= DbHelperSQL.GetSingle(sSql);
            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }

        public string GetLoanStatusbyFileID(int iFileID)
        {
            string sSql = "select Status from Loans where FileId=" + iFileID ;
            object obj = DbHelperSQL.GetSingle(sSql);
            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }

        public void InsertUserRecentItems(int iFileID, int iUserID)
        {
            string sSql = "select UserId from UserRecentItems where FileId=" + iFileID + " and UserId=" + iUserID;
            object obj = DbHelperSQL.GetSingle(sSql);
            if (obj != null)
            {
                sSql = "update UserRecentItems set LastAccessed=getdate() where FileId=" + iFileID + " and UserId=" + iUserID;
                DbHelperSQL.ExecuteSql(sSql);
            }
            else
            {
                sSql = "Insert into UserRecentItems (FileId,UserId,LastAccessed) values(" + iFileID + "," + iUserID + ",getdate())";
                DbHelperSQL.ExecuteSql(sSql);
            }
        }

        public void DeleteItemsByFileID(int iFileID)
        {
            string sSQL = "delete from UserRecentItems where FileId=" + iFileID;
            DbHelperSQL.ExecuteSql(sSQL);
        }
    }
}
