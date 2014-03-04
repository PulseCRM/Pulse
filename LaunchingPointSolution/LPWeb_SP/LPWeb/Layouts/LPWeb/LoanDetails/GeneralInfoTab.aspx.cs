using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;
using System.Data.SqlClient;
using System.Data;



    /// <summary>
    /// Email template list
    /// author: duanlijun
    /// date: 2012-09-25
    /// </summary>
public partial class GeneralInfoTab : BasePage
    {
        CompanyTaskPickList bllTaskPickList = new CompanyTaskPickList();
        int iProspectID = 0;
        int iLoanOfficerRoleID = 3;
        int iBranchID = 0;
        Prospect pro = new Prospect();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //权限验证
                var loginUser = new LoginUser();
                if (!loginUser.userRole.CompanySetup)
                {
                    Response.Redirect("../Unauthorize.aspx");
                    return;
                }

              
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            if (!Page.IsPostBack)
            {
                //string sqlLeadSource = "SELECT [LeadSourceID],[LeadSource] FROM [dbo].[Company_Lead_Sources] order by LeadSource ";

                //var dtLeadSource = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sqlLeadSource);

                //ddlLeadSource.DataSource = dtLeadSource;
                //ddlLeadSource.DataTextField = dtLeadSource.Columns["LeadSource"].ColumnName;
                //ddlLeadSource.DataValueField = dtLeadSource.Columns["LeadSourceID"].ColumnName;
                //ddlLeadSource.DataBind();
                //ddlLeadSource.Items.Insert(0, new ListItem() { Text = "--Select--", Value = "" });

                //if (Request.QueryString["ProspectID"] != null)
                //{
                //    this.iProspectID = Convert.ToInt32(this.Request.QueryString["ProspectID"]);

                //    string sql = "Select Top 1, PropertyAddr, PropertyCity, PropertyState, PropertyZip, SalesPrice, Ranking, ProspectLoanStatus from Loans where Status=’Prospect' and lpfn_GetBorrowerContactId (Loans.FileId)='" + iProspectID + "' order by Created DESC";

                //    DataTable dt = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sql);

                //    GetCurrentLoan(dt);
                //}
                //else
                //{
                //    lblStatus.Text = "Active";
                //}
            }
        }



     //private void GetCurrentLoan(DataTable dt)
     //   {
     //       if (!string.IsNullOrEmpty(dt.Rows[0]["PropertyAddr"].ToString()))
     //       {
     //           txtStreetAddress1.Text= dt.Rows[0]["PropertyAddr"].ToString();
     //       }

     //       if (!string.IsNullOrEmpty(dt.Rows[0]["LoanType"].ToString()))
     //       {
                
     //       }

     //       if (!string.IsNullOrEmpty(dt.Rows[0]["ProspectLoanStatus"].ToString()))
     //       {
     //           lblStatus.Text = dt.Rows[0]["ProspectLoanStatus"].ToString();
     //       }

          
     //}





        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                #region Create Lead – Other Income/Comments tab

               

                try
                {

                    LoanNotes ln = new LoanNotes();
                    ln.UpdateNoteAndProspectIncomeAndProspectAssets(1, txtComments.Text.Trim(), txtOtherMonthlyIncome.Text.Trim(), txtLiquidAsset.Text.Trim());
                   
                }
                catch (Exception ex)
                {
                    PageCommon.AlertMsg(this, "Failed to save contact and prospect for borrower.");
                    return;
                }

                #endregion
            }
            catch
            {
                PageCommon.WriteJsEnd(this, "Please enter the borrower information!", "window.parent.location.href=window.parent.location.href");

            }
        }

      

      
      

       
    }

