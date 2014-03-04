using System;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using System.Text;
using LPWeb.Common;

public partial class GetLoanProgramDetails_Ajax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // {"ExecResult":"Success","FirstAdj":"","SubAdj":"","LifetimeCap":"","IndexType":""}
        // {"ExecResult":"Failed","ErrorMsg":"error message"}

        bool bIsValid = PageCommon.ValidateQueryString(this, "InvestorID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid Investor id\"}");
            this.Response.End();
        }
        string InvestorID = this.Request.QueryString["InvestorID"];

        bIsValid = PageCommon.ValidateQueryString(this, "LoanProgramID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid lender company id\"}");
            this.Response.End();
        }
        string sLoanProgramID = this.Request.QueryString["LoanProgramID"];

        string sSql = "select * from Company_LoanProgramDetails where InvestorID=" + InvestorID + " and LoanProgramID=" + sLoanProgramID;
        DataTable LoanProgramDetails = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        string sFirstAdj = string.Empty;
        string sSubAdj = string.Empty;
        string sLifetimeCap = string.Empty;
        string sIndexType = string.Empty;
        string sMargin = string.Empty;

        if (LoanProgramDetails.Rows.Count > 0)
        {
            sFirstAdj = LoanProgramDetails.Rows[0]["FirstAdj"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanProgramDetails.Rows[0]["FirstAdj"]).ToString("n3");
            sSubAdj = LoanProgramDetails.Rows[0]["SubAdj"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanProgramDetails.Rows[0]["SubAdj"]).ToString("n3");
            sLifetimeCap = LoanProgramDetails.Rows[0]["LifetimeCap"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanProgramDetails.Rows[0]["LifetimeCap"]).ToString("n3");
            sIndexType = LoanProgramDetails.Rows[0]["IndexType"].ToString();
            sMargin = LoanProgramDetails.Rows[0]["Margin"].ToString();
        }

        this.Response.Write("{\"ExecResult\":\"Success\",\"FirstAdj\":\"" + sFirstAdj + "\",\"SubAdj\":\"" + sSubAdj + "\",\"LifetimeCap\":\"" + sLifetimeCap + "\",\"IndexType\":\"" + sIndexType + "\",\"Margin\":\"" + sMargin + "\"}");
    }
}
