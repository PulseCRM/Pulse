using System;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using System.Text;

public partial class GetUserData_Ajax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sTerm = this.Request.QueryString["term"].ToString();

        string sWhere = " and UserEnabled=1 and lower(LastName + ', ' +  FirstName) like lower('%" + sTerm + "%')";
        string sOrderBy = " order by LastName + ', ' +  FirstName";

        string sSql = "select * from Users where 1=1 " + sWhere + sOrderBy;
        DataTable UserList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        // json: [{"label":"Name1","value":"2"},{"label":"Name2","value":"2"},{"label":"Name3","value":"3"}]
        StringBuilder sbUsers = new StringBuilder();
        foreach (DataRow UserRow in UserList.Rows)
        {
            string sUserID = UserRow["UserID"].ToString();
            string sFirstName = UserRow["FirstName"].ToString();
            string sLastName = UserRow["LastName"].ToString();

            string sFullName = sLastName + ", " + sFirstName;

            if (sbUsers.Length == 0)
            {
                sbUsers.Append("{\"label\":\"" + sFullName + "\",\"value\":\"" + sFullName + "\",\"id\":\"" + sUserID + "\"}");
            }
            else
            {
                sbUsers.Append(",{\"label\":\"" + sFullName + "\",\"value\":\"" + sFullName + "\",\"id\":\"" + sUserID + "\"}");
            }
        }

        this.Response.Write("[" + sbUsers.ToString() + "]");
    }
}
