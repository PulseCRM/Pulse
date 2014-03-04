using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using System.Text;

public partial class GetLoanOfficerIDs_Background : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sTerm = this.Request.QueryString["term"].ToString();

        string[] LoanOfficerNames = sTerm.Split(';');

        string sLastOne = LoanOfficerNames[LoanOfficerNames.Length - 1];

        #region 获取Loan Officer列表
        string sSql = "select * from "
                    + "("
                    + "select UserId, LastName+', '+FirstName as FullName from dbo.lpfn_GetAllLoanOfficer(62) "
                    + "union "
                    + "select b.UserId, b.LastName+', '+b.FirstName as FullName from LoanTeam as a inner join Users as b on a.UserId=b.UserId where a.RoleId=(select RoleId from Roles where Name='Loan Officer') "
                    + ") as c "
                    + "where c.FullName like '%" + LPWeb.Common.SqlTextBuilder.ConvertQueryValue(sLastOne) + "%'";
        DataTable LoanOfficerList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        #endregion

        // json: [{"label":"LoanOfficerName1","value":"LoanOfficerName1","id":"1"},{"label":"LoanOfficerName2","value":"LoanOfficerName2","id":"2"}]
        StringBuilder sbLoanOfficer = new StringBuilder();
        foreach (DataRow PointFieldRow in LoanOfficerList.Rows)
        {
            string sUserId = PointFieldRow["UserId"].ToString();
            string sFullName = PointFieldRow["FullName"].ToString();

            //sFullName = sFullName.Replace(",", @"\\,");

            if (sbLoanOfficer.Length == 0)
            {
                sbLoanOfficer.Append("{\"label\":\"" + sFullName + "\",\"value\":\"" + sFullName + "\",\"id\":\"" + sUserId + "\"}");
            }
            else
            {
                sbLoanOfficer.Append(",{\"label\":\"" + sFullName + "\",\"value\":\"" + sFullName + "\",\"id\":\"" + sUserId + "\"}");
            }
        }

        this.Response.Write("[" + sbLoanOfficer.ToString() + "]");
    }
}
