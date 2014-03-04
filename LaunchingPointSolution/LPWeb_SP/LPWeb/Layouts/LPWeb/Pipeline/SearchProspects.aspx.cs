using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    public partial class SearchProspects : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDDLSource();

                this.ddlLoanOfficer.SelectedValue = this.Request.QueryString["loanOfficer"].ToString() == "" ? "0" : this.Request.QueryString["loanOfficer"].ToString();
                this.ddlState.SelectedValue = this.Request.QueryString["state"].ToString();
                this.ddlStatus.SelectedValue = this.Request.QueryString["status"].ToString();
                this.tbAddress.Text = this.Request.QueryString["address"].ToString();
                this.tbCity.Text = this.Request.QueryString["city"].ToString();
                this.tbLastName.Text = this.Request.QueryString["lastName"].ToString();
                this.tbLeadSource.Text = this.Request.QueryString["leadSource"].ToString();
                this.tbRefCode.Text = this.Request.QueryString["refCode"].ToString();
                this.tbZip.Text = this.Request.QueryString["zip"].ToString();
            }
        }

        /// <summary>
        /// Bind DDL Data Source
        /// </summary>
        private void BindDDLSource()
        { 
            BLL.Users bllUser = new BLL.Users();          
            string strWhere = "AND RoleName='Loan Officer'";
            if (CurrUser.bIsCompanyUser)
            {
                strWhere += " AND (UserId IN (SELECT UserId FROM GroupUsers WHERE GroupID IN(select GroupID from Groups where  CompanyID in (SELECT CompanyID FROM Groups where GroupID in (select GroupID from GroupUsers WHERE UserID = " + CurrUser.iUserID.ToString() + ")))))";
            }
            else if (CurrUser.bIsRegionUser)
            {
                strWhere += " AND (UserId IN (SELECT UserId FROM GroupUsers WHERE GroupID IN(select GroupID from Groups where  RegionID in (SELECT RegionID FROM Groups where GroupID in (select GroupID from GroupUsers WHERE UserID = " + CurrUser.iUserID.ToString() + ")))))";

            }
            else if (CurrUser.bIsDivisionUser)
            {
                strWhere += " AND (UserId IN (SELECT UserId FROM GroupUsers WHERE GroupID IN(select GroupID from Groups where  DivisionID in (SELECT DivisionID FROM Groups where GroupID in (select GroupID from GroupUsers WHERE UserID = " + CurrUser.iUserID.ToString() + ")))))";

            }
            else if (CurrUser.bIsBranchUser)
            {
                strWhere += " AND (UserId IN (SELECT UserId FROM GroupUsers WHERE GroupID IN(select GroupID from Groups where  BranchID in (SELECT BranchID FROM Groups where GroupID in (select GroupID from GroupUsers WHERE UserID = " + CurrUser.iUserID.ToString() + ")))))";
            }
           
            DataTable dtLoadOfficer = bllUser.GetUserList(strWhere);
            if(!dtLoadOfficer.Columns.Contains("LoanOfficer"))
            {
                dtLoadOfficer.Columns.Add("LoanOfficer");
            }
            foreach (DataRow dr in dtLoadOfficer.Rows)
            {               
                dr["LoanOfficer"] = dr["Name"].ToString();
            }
            DataRow drNew= dtLoadOfficer.NewRow();
            drNew["UserID"] = 0;
            drNew["LoanOfficer"] = "All";
            dtLoadOfficer.Rows.Add(drNew);

            DataView dv = dtLoadOfficer.DefaultView;
            dv.Sort = "UserID ";
            DataTable dtLoadOfficerORder = dv.ToTable();

            ddlLoanOfficer.DataSource = dtLoadOfficerORder;
            ddlLoanOfficer.DataTextField = "LoanOfficer";
            ddlLoanOfficer.DataValueField = "UserID";
            ddlLoanOfficer.SelectedValue = "0";
            ddlLoanOfficer.DataBind();
        }

        /// <summary>
        /// Search info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            
        }
    }
}
