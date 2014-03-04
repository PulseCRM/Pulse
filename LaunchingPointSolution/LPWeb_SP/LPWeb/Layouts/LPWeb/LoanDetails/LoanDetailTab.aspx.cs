using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;
using System.Data;


public partial class LoanDetailTab : BasePage
{
    int iFileID = 0;
    Loans loans = new Loans();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            LoginUser loginUser = new LoginUser();
            ////权限验证
            //if (!loginUser.userRole.LoanSetup)
            //{
            //    Response.Redirect("../Unauthorize.aspx");
            //    return;
            //}

        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
        string sErrorMsg = "Failed to load current page: invalid FileID.";
        string sReturnPage = "LoanDetailTab.aspx";

        if (this.Request.QueryString["FileID"] != null) // 如果有GroupID
        {
            string sFileID = this.Request.QueryString["FileID"].ToString();

            if (PageCommon.IsID(sFileID) == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
            }

            this.iFileID = Convert.ToInt32(sFileID);
        }
        if (iFileID == 0)
        {
            return;
        }
        BindPage();
    }
    private void BindPage()
    {
        try
        {
            BindItems();
        }
        catch
        { }
        try
        {
            BindBorrower();
        }
        catch
        { }
        try
        {
            BindCoBorrower();
        }
        catch
        { }
    }

    private void BindItems()
    {
        LPWeb.Model.Loans model = new LPWeb.Model.Loans();
        model = loans.GetModel(iFileID);
        if (model.LoanAmount.HasValue)
        {
            lbLoanAmount.Text = "$" + string.Format("{0:N0}", model.LoanAmount.Value);
        }
        if (model.SalesPrice.HasValue)
        {
            lbSalesPrice.Text = "$" + string.Format("{0:N0}", model.SalesPrice.Value);
        }
        if (model.AppraisedValue.HasValue)
        {
            lbAppraisedValue.Text = "$" + string.Format("{0:N0}", model.AppraisedValue.Value);
        }
        if (model.Rate.HasValue)
        {
            lbRate.Text = model.Rate.Value.ToString("#.####") + "%";
        }
        lbLoanType.Text = model.LoanType;

        if (model.DownPay.HasValue)
        {
            lbDownPayment.Text = model.DownPay.Value.ToString("#.##") + "%";
        }

        if (model.MonthlyPayment.HasValue)
        {
            lbMonthlyPayment.Text = "$" + string.Format("{0:N0}", model.MonthlyPayment.Value);
        }
        if (model.LTV.HasValue)
        {
            lbLTV.Text = model.LTV.Value.ToString("#.##") + "%";
        }
        if (model.CLTV.HasValue)
        {
            lbCLTV.Text = model.CLTV.Value.ToString("#.##") + "%";
        }
        string term = string.Empty;
        if (model.Term.HasValue)
        {
            term = model.Term.Value.ToString();
        }
        string due = string.Empty;
        if (model.Due.HasValue)
        {
            due = model.Due.Value.ToString();
        }
        lbTermDue.Text = term + "/" + due;
        lbPurpose.Text = model.Purpose;
        lbLienPosition.Text = model.LienPosition;
        lbOccupancy.Text = model.Occupancy;
        lbProgram.Text = model.Program;
        lbCCScenario.Text = model.CCScenario;
        lbCounty.Text = model.County;
         
        lbLender.Text = loans.GetLender(iFileID);

        //if (model.Lender.HasValue)
        //{
        //    lbLender.Text = string.Empty;
        //    ContactCompanies cc = new ContactCompanies();
        //    LPWeb.Model.ContactCompanies ccmodel= cc.GetModel(model.Lender.Value);
        //    if (ccmodel != null)
        //    {
        //        lbLender.Text = ccmodel.Name;
        //    }
        //}
    }

    private void BindBorrower()
    {
        Contacts contact = new Contacts();
        DataRow row = contact.GetBorrowerDetails(iFileID, "Borrower");
        if (row == null)
        {
            return;
        }
        if ((row["DOB"] != null) && (row["DOB"] != DBNull.Value))    
        {
            lbDateBirth.Text = DateTime.Parse(row["DOB"].ToString()).ToShortDateString();
        }
        if ((row["TransUnion"] != null) && (row["TransUnion"] != DBNull.Value))       
        {
            lbTransUnion.Text = int.Parse(row["TransUnion"].ToString()).ToString();
        }
        if ((row["Experian"] != null) && (row["Experian"] != DBNull.Value))  
        {
            lbExperianFico.Text = int.Parse(row["Experian"].ToString()).ToString();
        }
        if ((row["Equifax"] != null) && (row["Equifax"] != DBNull.Value))   
        {
            lbEquifax.Text = int.Parse(row["Equifax"].ToString()).ToString();
        }
        if ((row["SSN"] != null) && (row["SSN"] != DBNull.Value))    
        {
            lbSecurityNumber.Text = row["SSN"].ToString();
        }
    }

    private void BindCoBorrower()
    {
        Contacts contact = new Contacts();
        DataRow row = contact.GetBorrowerDetails(iFileID, "CoBorrower");
        if (row == null)
        {
            return;
        }
        if ((row["DOB"] != null) && (row["DOB"] != DBNull.Value))
        {
            lbCDateBirth.Text = DateTime.Parse(row["DOB"].ToString()).ToShortDateString();
        }
        if ((row["TransUnion"] != null) && (row["TransUnion"] != DBNull.Value))
        {
            lbCTransUnion.Text = int.Parse(row["TransUnion"].ToString()).ToString();
        }
        if ((row["Experian"] != null) && (row["Experian"] != DBNull.Value))
        {
            lbCExperianFico.Text = int.Parse(row["Experian"].ToString()).ToString();
        }
        if ((row["Equifax"] != null) && (row["Equifax"] != DBNull.Value))
        {
            lbCEquifax.Text = int.Parse(row["Equifax"].ToString()).ToString();
        }
        if ((row["SSN"] != null) && (row["SSN"] != DBNull.Value))
        {
            lbCSecurityNumber.Text = row["SSN"].ToString();
        }
    }
}

