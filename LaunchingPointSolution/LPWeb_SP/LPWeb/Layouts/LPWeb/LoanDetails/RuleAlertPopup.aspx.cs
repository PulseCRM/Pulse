using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using LPWeb.Common;
using System.Text;

public partial class LoanDetails_RuleAlertPopup : BasePage
{
    int iLoanID = 0;
    int iAlertID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing1", "$('#divContainer').hide();alert('Missing required query string1.');window.parent.CloseDialog_RuleAlert();", true);
            return;
        }
        this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

        bIsValid = PageCommon.ValidateQueryString(this, "AlertID", QueryStringType.ID);
        string sAlertID = "0";
        if (bIsValid == false)
        {
            //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing2", "$('#divContainer').hide();alert('Missing required query string.');window.parent.CloseDialog_RuleAlert();", true);
            //return;
        }
        else
        {
            sAlertID = this.Request.QueryString["AlertID"];
        }
        this.iAlertID = Convert.ToInt32(sAlertID);
        if (this.iLoanID > 0 && this.iAlertID == 0)
        {
            //Get first alert id
            LPWeb.BLL.LoanAlerts loanAlert = new LPWeb.BLL.LoanAlerts();
            DataSet dsAlert = loanAlert.GetList(1, " FileId=" + this.iLoanID + " AND AlertType='Rule' AND ([Status] IS NULL OR [Status]='Acknowledged')", "DueDate");
            if(dsAlert.Tables[0].Rows.Count > 0)
            {
                this.iAlertID = Convert.ToInt32(dsAlert.Tables[0].Rows[0]["LoanAlertId"].ToString());
            }
        }
        if (this.iAlertID == 0)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing2", "$('#divContainer').hide();alert('Missing required query string2.');window.parent.CloseDialog_RuleAlert();", true);
            return;
        }
        this.hdnCloseDialogCodes.Value = "";
        this.hdnCloseDialogCodes.Value = this.Request.QueryString["CloseDialogCodes"];
        #endregion

        LoginUser CurrentUser = new LoginUser();
        this.hdnLoginUserID.Value = CurrentUser.iUserID.ToString();
        this.hdnSendEmail.Value = CurrentUser.userRole.SendEmail == true ? "1" : "0";

        #region 获取Alert数据

        string sSql2 = "select dbo.lpfn_GetAlertIconFileName(LoanAlertId) as AlertIcon, * from LoanAlerts where LoanAlertId=" + this.iAlertID;
        DataTable AlertInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql2);

        #endregion

        if (AlertInfo.Rows.Count == 0)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Invalid1", "$('#divContainer').hide();alert('Invalid loan alert id.');window.parent.CloseDialog_RuleAlert();", true);
            return;
        }

        #region 加载Loan Info

        string sSql3 = "select * from Loans where FileId=" + this.iLoanID;
        DataTable LoanInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql3);

        #endregion

        #region 加载Loan Officer Info

        string sSql4 = string.Format("select [dbo].[lpfn_GetLoanOfficer]({0}) ", this.iLoanID);
        object LoanOfficerInfo = LPWeb.DAL.DbHelperSQL.GetSingle(sSql4);

        #endregion

        #region 加载Borrower Info

        string sSql5 = string.Format("select [dbo].[lpfn_GetBorrower]({0}) ", this.iLoanID);
        object BorrowerInfo = LPWeb.DAL.DbHelperSQL.GetSingle(sSql5);

        #endregion

        #region 加载CoBorrower Info

        string sSql6 = string.Format("select [dbo].[lpfn_GetCoborrower] ({0}) ", this.iLoanID);
        object CoBorrowerInfo = LPWeb.DAL.DbHelperSQL.GetSingle(sSql6);

        #endregion

        // alert status
        this.hdnAlertStatus.Value = AlertInfo.Rows[0]["Status"] == DBNull.Value ? String.Empty : AlertInfo.Rows[0]["Status"].ToString();

        if (this.IsPostBack == false)
        {
            #region 绑定Alert信息

            string sAlertIcon = AlertInfo.Rows[0]["AlertIcon"].ToString();
            if (sAlertIcon != string.Empty)
            {
                this.imgAlertIcon.Src = "../images/alert/" + AlertInfo.Rows[0]["AlertIcon"].ToString();
            }
            else
            {
                this.imgAlertIcon.Visible = false;
            }
            this.lbRuleName.Text = AlertInfo.Rows[0]["Desc"] == DBNull.Value ? string.Empty : AlertInfo.Rows[0]["Desc"].ToString();
            this.lbDetected.Text = Convert.ToDateTime(AlertInfo.Rows[0]["DateCreated"]).ToShortDateString();
            this.lbDueDate.Text = Convert.ToDateTime(AlertInfo.Rows[0]["DueDate"]).ToShortDateString();

            string sAlertEmail = AlertInfo.Rows[0]["AlertEmail"].ToString();
            this.ltEmailContent.Text = sAlertEmail;
            this.txtAlertEmail.Value = sAlertEmail;

            this.txtRecommEmail.Value = AlertInfo.Rows[0]["RecomEmail"].ToString();

            #endregion

            #region 绑定Borrower信息

            this.lbBorrower.Text = BorrowerInfo == null ? string.Empty : (string)BorrowerInfo;

            #endregion

            #region 绑定Loan Officer信息

            this.lbLoanOfficer.Text = LoanOfficerInfo == DBNull.Value ? string.Empty : (string)LoanOfficerInfo;

            #endregion

            #region 绑定CoBorrower信息

            this.lbCoborrower.Text = CoBorrowerInfo == DBNull.Value ? string.Empty : (string)CoBorrowerInfo;

            #endregion

            #region 绑定Loan信息
            if (LoanInfo == null || LoanInfo.Rows.Count <= 0)
                return;
            this.lbLoanAmount.Text = LoanInfo.Rows[0]["LoanAmount"]==DBNull.Value ? string.Empty : Convert.ToDecimal(LoanInfo.Rows[0]["LoanAmount"]).ToString("n0");

            string sPropertyAddr = LoanInfo.Rows[0]["PropertyAddr"]==DBNull.Value ? string.Empty : LoanInfo.Rows[0]["PropertyAddr"].ToString();
            string sPropertyCity = LoanInfo.Rows[0]["PropertyCity"] == DBNull.Value ? string.Empty : LoanInfo.Rows[0]["PropertyCity"].ToString();
            string sPropertyState = LoanInfo.Rows[0]["PropertyState"] == DBNull.Value ? string.Empty : LoanInfo.Rows[0]["PropertyState"].ToString();
            string sPropertyZip = LoanInfo.Rows[0]["PropertyZip"] == DBNull.Value ? string.Empty : LoanInfo.Rows[0]["PropertyZip"].ToString();
            string sProperty = sPropertyAddr + ", " + sPropertyCity + ", " + sPropertyState + " " + sPropertyZip;

            this.lbProperty.Text = sProperty;

            this.lbInterestRate.Text = LoanInfo.Rows[0]["Rate"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanInfo.Rows[0]["Rate"]).ToString("n3");

            #endregion

        }
    }
}
