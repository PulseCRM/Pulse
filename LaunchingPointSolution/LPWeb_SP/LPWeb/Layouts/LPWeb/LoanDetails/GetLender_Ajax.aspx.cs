using System;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using System.Text;

public partial class GetLender_Ajax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sTerm = this.Request.QueryString["term"].ToString();

        string sWhere = " and Enabled=1 and lower(Name) like lower('%" + sTerm + "%')";
        string sOrderBy = " order by Name";

        string sSql = "select * from ContactCompanies where 1=1 " + sWhere + sOrderBy;
        DataTable LenderList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        // json: [{"label":"Name1","value":"2"},{"label":"Name2","value":"2"},{"label":"Name3","value":"3"}]
        StringBuilder sbLenders = new StringBuilder();
        foreach (DataRow LenderRow in LenderList.Rows)
        {
            string sCompanyId = LenderRow["ContactCompanyId"].ToString();
            string sCompanyName = LenderRow["Name"].ToString();
            
            if (sbLenders.Length == 0)
            {
                sbLenders.Append("{\"label\":\"" + sCompanyName + "\",\"value\":\"" + sCompanyName + "\",\"id\":\"" + sCompanyId + "\"}");
            }
            else
            {
                sbLenders.Append(",{\"label\":\"" + sCompanyName + "\",\"value\":\"" + sCompanyName + "\",\"id\":\"" + sCompanyId + "\"}");
            }
        }

        this.Response.Write("[" + sbLenders.ToString() + "]");
    }
}
