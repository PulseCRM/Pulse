using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LPWeb.DAL
{
    public class CompanyLoanPointFields : CompanyLoanPointFieldsBase
    {
        public DataTable GetCompanyLoanPointFieldsInfo()
        {
            string sSQL = @"select cp.*,pd.Label from dbo.CompanyLoanPointFields  cp 
            left join dbo.PointFieldDesc pd on pd.PointFieldId=cp.PointFieldId";
            return DbHelperSQL.ExecuteDataTable(sSQL);
        }
    }
}
