using System;
using LPWeb.Common;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    public partial class PopupAlertDetail : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var CurrentUser = new LoginUser();
                if (!CurrentUser.userRole.SendEmail)
                {
                    BtnSendEmail.Enabled = false;
                }

                if (!CurrentUser.userRole.ExtendRateLock)
                {
                    //BtnExtendRateLock.Disabled = true;
                    this.hdnShowLockRatePopup.Value = "false";
                }
                else
                {
                    this.hdnShowLockRatePopup.Value = "true";
                }

                var loanId = 0;
                if (Request.QueryString["fileId"] != null)
                    int.TryParse(Request.QueryString["fileId"], out loanId);

                var imgSrc = string.Empty;
                if (Request.QueryString["icon"] != null)
                    imgSrc = Request.QueryString["icon"].ToString();

                if (loanId == 0 || string.IsNullOrEmpty(imgSrc))
                    return;

                BindPage(loanId);
                imgIcon.Src = "../images/loan/" + imgSrc;
            }
        }

        private void BindPage(int fileId)
        {
            BLL.Loans bllLoans = new BLL.Loans();

            Model.Loans modelLoan = new Model.Loans();
            BLL.Contacts bllContact = new BLL.Contacts();
            BLL.Users bllUser = new BLL.Users();

            modelLoan = bllLoans.GetModel(fileId);
            if (modelLoan != null && modelLoan.RateLockExpiration != null)
            {
                lblRateLockExp.Text = modelLoan.RateLockExpiration.Value.ToShortDateString();
                hfdExpDate.Value = modelLoan.RateLockExpiration.Value.ToOADate().ToString();
            }

            lblCurrentState.Text = bllLoans.GetLoanStage(fileId);

            lblBorrower.Text = bllContact.GetBorrower(fileId);

            if (modelLoan != null)
                lblEstCloseDate.Text = modelLoan.EstCloseDate != null ? modelLoan.EstCloseDate.Value.ToShortDateString() : string.Empty;

            lblCoborrower.Text = bllContact.GetCoBorrower(fileId);

            lblLoanOfficer.Text = bllUser.GetLoanOfficer(fileId);

            if (modelLoan != null)
                lblPropertyAddress.Text = modelLoan.PropertyAddr + " " + modelLoan.PropertyCity + " " + modelLoan.PropertyState + " " + modelLoan.PropertyZip;

            if (modelLoan != null && modelLoan.LoanAmount.HasValue)
                lblLoanAmount.Text = "$" + string.Format("{0:N0}", modelLoan.LoanAmount.Value);

            if (modelLoan != null && modelLoan.Rate.HasValue)
                lblInterestRate.Text = modelLoan.Rate.Value.ToString("#.####") + "%";

            lblLender.Text = bllLoans.GetLender(fileId);

            hfdFileId.Value = fileId.ToString();
        }

        protected void BtnSendEmail_Click(object sender, EventArgs e)
        {
            PageCommon.AlertMsg(this, "Not Implemented!");
        }

        protected void BtnColse_Click(object sender, EventArgs e)
        {
            PageCommon.AlertMsg(this, "Not Implemented!");
        }
    }
}

