using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using LPWeb.BLL;

namespace LPWeb.Layouts.LPWeb.Prospect
{
    public partial class SubmitLoanPopup : LayoutsPageBase
    {
        protected int iLoanID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString["loanID"] != null)
            {
                iLoanID = Convert.ToInt32(this.Request.QueryString["loanID"]);
            }
            if (!IsPostBack)
            {
                BindDDL();
            }
        }

        private void BindDDL()
        {
            Company_Loan_Programs _bCompany_Loan_Programs=new Company_Loan_Programs();
            DataSet ds = _bCompany_Loan_Programs.GetList("1>0");

            ddlProgram.DataValueField = "LoanProgramID";
            ddlProgram.DataTextField = "LoanProgram";
            ddlProgram.DataSource = ds.Tables[0];
            ddlProgram.DataBind();

        }

        protected void btnYes_Click(object sender, EventArgs e)
        { 
            
        }
    }
}
