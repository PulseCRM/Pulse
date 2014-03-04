using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    public class UserLoansViewPointFields : UserLoansViewPointFieldsBase
    {
        public DataTable GetUserLoansViewPointFieldsInfo(int iUserID,int iFileID)
        {
            string sSQL = @"select cpf.Heading,lf.CurrentValue from UserLoansViewPointFields up 
                            left join LoanPointFields lf on lf.PointFieldId=up.PointFieldId and FileId="+ iFileID;
                    sSQL +=" left join CompanyLoanPointFields cpf on cpf.PointFieldId=up.PointFieldId where up.UserId=" + iUserID + "";
            return DbHelperSQL.ExecuteDataTable(sSQL);
        }

        public string GetUserLoansViewPointFieldsCurrentValue(int iUserID, int iFileID,int iPointFieldID)
        {
            string sCurrentValue = "";
            string sSQL = @"select lf.CurrentValue from UserLoansViewPointFields up 
                            left join LoanPointFields lf on lf.PointFieldId=up.PointFieldId and FileId=" + iFileID + " where up.UserId=" + iUserID + " and  up.PointFieldId=" + iPointFieldID;
            object obj= DbHelperSQL.GetSingle(sSQL);
            if (obj != null)
            {
                sCurrentValue = obj.ToString();
            }
            return sCurrentValue;
        }
        
        public int GetUserLoansViewPointFieldsCount(int iUserID)
        {
            string sSQL = @"select count(UserId) from UserLoansViewPointFields up where up.UserId=" + iUserID + "";
            return Convert.ToInt32(DbHelperSQL.GetSingle(sSQL));
        }



        public DataTable GetUserLoansViewPointFieldsHeadingInfo(int iUserID)
        {
            string sSQL = @"select cpf.Heading,up.PointFieldId from UserLoansViewPointFields up 
                          left join CompanyLoanPointFields cpf on cpf.PointFieldId=up.PointFieldId where up.UserId=" + iUserID + "";
            return DbHelperSQL.ExecuteDataTable(sSQL);
        }

        public DataTable GetUserLoansViewPointFieldsLabelHeadingInfo(int iUserID)
        {
            string sSQL = @"select cpf.Heading,up.PointFieldId,pfd.Label from UserLoansViewPointFields up 
                            left join CompanyLoanPointFields cpf on cpf.PointFieldId=up.PointFieldId 
                            left join PointFieldDesc pfd on pfd.PointFieldId = up.PointFieldId
                            where up.UserId =" + iUserID + "";
            return DbHelperSQL.ExecuteDataTable(sSQL);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetUserPointFieldInfo(int iUserID)
        {
            string sSql = @"select a.PointFieldId, a.UserId, b.Heading, c.Label, c.DataType, 
case when c.DataType=2 then 'Numeric' when c.DataType=3 then 'Yes/No' when c.DataType=4 then 'Date' else 'String' end as DataTypeName 
from UserLoansViewPointFields as a 
left join CompanyLoanPointFields as b on a.PointFieldId = b.PointFieldId 
left join PointFieldDesc as c on a.PointFieldId = c.PointFieldId 
where a.UserId="+iUserID;

            return DbHelperSQL.ExecuteDataTable(sSql);
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteAllByUser(int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UserLoansViewPointFields ");
            strSql.Append(" where UserId=@UserId");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
