using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.BLL;
using System.Data;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;
using System.Linq;
using System.Collections.Generic;

public partial class LoanDetails_LockRatePopup : BasePage
{
    static int G_LoanProgramID = 0;
    int iFileId = 0;
    DataTable LoanInfo = null;
    DataTable LoanLocksInfo = null;
    LoanPointFields LoanPointFieldsMgr = null;
    LPWeb.Model.LoanProfit LoanProfitInfo = new LPWeb.Model.LoanProfit();

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 检查权限

        G_LoanProgramID = 0;

        CheckUserPrivileges();

        //this.txtMargin.Text = string.Empty;
        //this.txtInitialAdjCap.Text = string.Empty;
        //this.txtAdjCap.Text = string.Empty;
        //this.txtLifeCap.Text = string.Empty;
        //this.txtIndex.Text = string.Empty;

        #endregion

        #region 校验页面参数

        #region 校验FileId

        bool bValid = PageCommon.ValidateQueryString(this, "FileId", QueryStringType.ID);
        if (bValid == false)
        {
            PageCommon.WriteJsEnd(this, "Invalid file id.", "window.parent.CloseGlobalPopup();");
            return;
        }

        string sFileId = this.Request.QueryString["FileId"];
        this.iFileId = Convert.ToInt32(sFileId);

        string slocked = this.Request.QueryString["locked"];
        if (slocked == "1")
        {
            this.hdnViewLockRate.Value = "true";
        }

        #endregion

        #endregion

        #region 加载Loan Info

        Loans LoansMgr = new Loans();
        this.LoanInfo = LoansMgr.GetLoanInfo(this.iFileId);
        if (LoanInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid file id.", "window.parent.CloseGlobalPopup();");
            return;
        }

        #endregion
        decimal tempDecimal = 0;
        #region 加载LoanProfit Info
        LoanProfit LoanProfitMgr = new LoanProfit();

        #endregion

        LoanLocks LoanLocksMgr = new LoanLocks();
        this.LoanLocksInfo = LoanLocksMgr.GetLoanLocksInfo(this.iFileId);

        if (!Page.IsPostBack)
        {
            LPWeb.BLL.Company_LoanProgramDetails bllcompanyLPD = new Company_LoanProgramDetails();
            LPWeb.BLL.Company_Loan_Programs bllcompanyLP = new Company_Loan_Programs();

            BindInvestors(this.LoanLocksInfo);
            BindLoanProgram(this.LoanLocksInfo);

            #region 绑定Loan Info

            LoanPointFields LoanPointFieldsMgr = new LoanPointFields();
            DataTable LoanAmountInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 21017);
            if (LoanAmountInfo.Rows.Count > 0)
            {
                string sLoanAmount = LoanAmountInfo.Rows[0]["CurrentValue"] == DBNull.Value ? string.Empty : LoanAmountInfo.Rows[0]["CurrentValue"].ToString();
                if (sLoanAmount != string.Empty)
                {
                    if (decimal.TryParse(sLoanAmount, out tempDecimal))
                        this.txtLoanAmount.Text = tempDecimal.ToString("n0");
                        //this.lbLoanAmount.Text = tempDecimal.ToString("n0");
                }
            }
            else
            {
                decimal.TryParse(LoanInfo.Rows[0]["LoanAmount"].ToString(), out tempDecimal);
                this.txtLoanAmount.Text = tempDecimal.ToString("n0");
                //this.lbLoanAmount.Text = tempDecimal.ToString("n0");
            }
            //2014/1/19 CR072 
            tempDecimal = 0;
            DataTable dtAppraisedValue = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 801);
            if (dtAppraisedValue.Rows.Count > 0)
            {
                string sAppraisedValue = dtAppraisedValue.Rows[0]["CurrentValue"] == DBNull.Value ? string.Empty : dtAppraisedValue.Rows[0]["CurrentValue"].ToString();
                if (sAppraisedValue != string.Empty)
                {
                    if (decimal.TryParse(sAppraisedValue, out tempDecimal))
                        this.txtApprValue.Text = tempDecimal.ToString("n0");
                }
            }
            //2014/1/19 CR072 
            tempDecimal = 0;
            DataTable dtSalesPrice = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 800);
            if (dtSalesPrice.Rows.Count > 0)
            {
                string sSalesPrice = dtSalesPrice.Rows[0]["CurrentValue"] == DBNull.Value ? string.Empty : dtSalesPrice.Rows[0]["CurrentValue"].ToString();
                if (sSalesPrice != string.Empty)
                {
                    if (decimal.TryParse(sSalesPrice, out tempDecimal))
                        this.txtSalesPrice.Text = tempDecimal.ToString("n0");
                }
            }
            //this.txtLoanAmount.Enabled = false;

            // Loan Changed
            //this.chkLoanChanged.Checked = this.LoanInfo.Rows[0]["LoanChanged"] == DBNull.Value ? false : Convert.ToBoolean(LoanInfo.Rows[0]["LoanChanged"]);

            try
            {
                this.LoanProfitInfo = LoanProfitMgr.GetModel(this.iFileId);
                if (this.LoanProfitInfo != null)
                {
                    if (this.LoanProfitInfo.Price != null)
                    {
                        decimal.TryParse(this.LoanProfitInfo.Price.ToString(), out tempDecimal);
                        txtPrice.Text = tempDecimal.ToString("n3");
                    }
                    else txtPrice.Text = string.Empty;
                    if (this.LoanProfitInfo.LenderCredit != null)
                    {
                        decimal.TryParse(this.LoanProfitInfo.LenderCredit.ToString(), out tempDecimal);
                        txtLenderCredit.Text = tempDecimal.ToString("n2");
                    }
                    else
                        txtLenderCredit.Text = string.Empty;
                    string CompPlan = string.IsNullOrEmpty(this.LoanProfitInfo.CompensationPlan) ? string.Empty : this.LoanProfitInfo.CompensationPlan.Trim();
                    if (string.IsNullOrEmpty(CompPlan))
                        ddlCompensationPlan.SelectedIndex = 0;
                    else
                        ddlCompensationPlan.SelectedValue = CompPlan;
                }

                if (this.LoanLocksInfo != null && this.LoanLocksInfo.Rows.Count > 0)
                {
                    var loanLocksInfoDr = this.LoanLocksInfo.Rows[0];
                    if (
                        loanLocksInfoDr["Ext1Term"] != DBNull.Value || loanLocksInfoDr["Ext1LockExpDate"] != DBNull.Value || loanLocksInfoDr["Ext1LockTime"] != DBNull.Value
                        || loanLocksInfoDr["Ext1LockedBy"] != DBNull.Value
                        || loanLocksInfoDr["Ext2Term"] != DBNull.Value || loanLocksInfoDr["Ext2LockExpDate"] != DBNull.Value || loanLocksInfoDr["Ext2LockTime"] != DBNull.Value
                        || loanLocksInfoDr["Ext2LockedBy"] != DBNull.Value
                        || loanLocksInfoDr["Ext3Term"] != DBNull.Value || loanLocksInfoDr["Ext3LockExpDate"] != DBNull.Value || loanLocksInfoDr["Ext3LockTime"] != DBNull.Value
                        || loanLocksInfoDr["Ext3LockedBy"] != DBNull.Value
                        )
                    {
                        //Ext1Term, Ext1LockExpDate, Ext1LockTime, Ext1LockedBy, Ext2Term, Ext2LockExpDate, Ext2LockTime, Ext2LockedBy, and Ext3Term, Ext3LockExpDate, Ext3LockTime, Ext3LockedBy
                        this.chkLoanChanged.Checked = true;
                    }
                    //2014/1/16 CR072
                    string sLockTime = loanLocksInfoDr["LockTime"] == DBNull.Value ? string.Empty : loanLocksInfoDr["LockTime"].ToString();
                    DateTime tempLockTime = DateTime.MinValue;
                    if (!string.IsNullOrEmpty(sLockTime))
                    {
                        if (DateTime.TryParse(sLockTime, out tempLockTime))
                        {
                            this.txtOrigLockDate.Text = tempLockTime.ToString("MM/dd/yyyy");
                        }
                    }
                    //string sLockExpirationDate = loanLocksInfoDr["LockExpirationDate"] == DBNull.Value ? string.Empty : loanLocksInfoDr["LockExpirationDate"].ToString();
                    //tempLockTime = DateTime.MinValue;
                    //if (!string.IsNullOrEmpty(sLockExpirationDate))
                    //{
                    //    if (DateTime.TryParse(sLockExpirationDate, out tempLockTime))
                    //    {
                    //        this.txtOrigExpirationDate.Text = tempLockTime.ToString("MM/dd/yyyy");
                    //    }
                    //}
                    string sLockTerm = loanLocksInfoDr["LockTerm"] == DBNull.Value ? string.Empty : loanLocksInfoDr["LockTerm"].ToString();
                    int tempLockTerm = 0;
                    if (sLockTerm != string.Empty)
                    {
                        if (int.TryParse(sLockTerm, out tempLockTerm))
                            this.txtOrigLockDateTerm.Text = tempLockTerm.ToString("n0");
                    }

                    //string sLockExpirationDate = loanLocksInfoDr["LockExpirationDate"] == DBNull.Value ? string.Empty : loanLocksInfoDr["LockExpirationDate"].ToString();
                    //tempLockTime = DateTime.MinValue;
                    //if (!string.IsNullOrEmpty(sLockExpirationDate))
                    //{
                    //    if (DateTime.TryParse(sLockExpirationDate, out tempLockTime))
                    //    {
                    //        this.txtOrigExpirationDate.Text = tempLockTime.ToString("MM/dd/yyyy");
                    //    }
                    //}

                    DateTime tempDate1 = DateTime.MinValue;
                    int tempInt1 = 0;
                    this.txtOrigExpirationDate.Text = string.Empty;

                    if (!string.IsNullOrEmpty(this.txtOrigLockDate.Text) && !string.IsNullOrEmpty(this.txtOrigLockDateTerm.Text))
                    {
                        tempDate1 = DateTime.MinValue;
                        tempInt1 = 0;
                        int.TryParse(this.txtOrigLockDateTerm.Text, out tempInt1);
                        if (DateTime.TryParse(this.txtOrigLockDate.Text, out tempDate1))
                        {
                            DateTime answer = tempDate1.AddDays(tempInt1);
                            this.txtOrigExpirationDate.Text = answer.ToShortDateString();
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }

            // Rate
            tempDecimal = 0;
            DataTable NoteRateInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 12);
            if (NoteRateInfo.Rows.Count > 0)
            {
                string sRate = NoteRateInfo.Rows[0]["CurrentValue"] == DBNull.Value ? string.Empty : NoteRateInfo.Rows[0]["CurrentValue"].ToString();
                if (sRate != string.Empty)
                {
                    if (decimal.TryParse(sRate, out tempDecimal))
                        this.txtRate.Text = tempDecimal.ToString("n3");
                }
            }

            // Term
            string sTerm = this.LoanInfo.Rows[0]["Term"] == DBNull.Value ? string.Empty : this.LoanInfo.Rows[0]["Term"].ToString();
            int tempInt = 0;
            if (sTerm != string.Empty)
            {
                if (int.TryParse(sTerm, out tempInt))
                    this.txtTerm.Text = tempInt.ToString("n0");
            }
            // Due
            string sDue = this.LoanInfo.Rows[0]["Due"] == DBNull.Value ? string.Empty : this.LoanInfo.Rows[0]["Due"].ToString();
            tempInt = 0;
            if (sDue != string.Empty)
            {
                if (int.TryParse(sDue, out tempInt))
                    this.txtDue.Text = tempInt.ToString("n0");
            }

            #region LTV/CLTV
            //2014/1/19 CR072
            tempDecimal = 0;
            string sLTV = this.LoanInfo.Rows[0]["LTV"] == DBNull.Value ? string.Empty : this.LoanInfo.Rows[0]["LTV"].ToString();
            if (decimal.TryParse(sLTV, out tempDecimal))
                sLTV = tempDecimal.ToString("n3");
            DataTable dtLTV = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 540);
            if (dtLTV.Rows.Count > 0)
            {
                sLTV = dtLTV.Rows[0]["CurrentValue"] == DBNull.Value ? string.Empty : dtLTV.Rows[0]["CurrentValue"].ToString();
                if (sLTV != string.Empty)
                {
                    if (decimal.TryParse(sLTV, out tempDecimal))
                        sLTV = tempDecimal.ToString("n3");
                }
            }

            string sCLTV = this.LoanInfo.Rows[0]["CLTV"] == DBNull.Value ? string.Empty : this.LoanInfo.Rows[0]["CLTV"].ToString();
            if (decimal.TryParse(sCLTV, out tempDecimal))
                sCLTV = tempDecimal.ToString("n3");
            DataTable dtCLTV = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 541);
            if (dtCLTV.Rows.Count > 0)
            {
                sCLTV = dtCLTV.Rows[0]["CurrentValue"] == DBNull.Value ? string.Empty : dtCLTV.Rows[0]["CurrentValue"].ToString();
                if (sCLTV != string.Empty)
                {
                    if (decimal.TryParse(sCLTV, out tempDecimal))
                        sCLTV = tempDecimal.ToString("n3");
                }
            }

            this.txtLTV.Text = sLTV; //+ " / " + sCLTV;
            this.txtCLTV.Text = sCLTV; //CR67
            #endregion

            // Purpose
            //this.txtPurpose.Text = this.LoanInfo.Rows[0]["Purpose"] == DBNull.Value ? string.Empty : this.LoanInfo.Rows[0]["Purpose"].ToString();
            //CR67
            var spurpose = (this.LoanInfo.Rows[0]["Purpose"] == DBNull.Value || this.LoanInfo.Rows[0]["Purpose"].ToString() == "") ? string.Empty : this.LoanInfo.Rows[0]["Purpose"].ToString();

            if (!string.IsNullOrEmpty(spurpose))
            {
                ListItem itemSpurpose = this.ddlPurpose.Items.FindByText(spurpose);
                if (itemSpurpose == null)
                {
                    this.ddlPurpose.SelectedValue = "0";
                }
                else
                {
                    this.ddlPurpose.SelectedValue = itemSpurpose.Value;
                }

            }

            // Loan Program
            this.ddlLoanProgram.SelectedValue = this.LoanInfo.Rows[0]["Program"] == DBNull.Value ? string.Empty : this.LoanInfo.Rows[0]["Program"].ToString();
            #region Occupancy
            // Occupancy
            string sOccupancy = this.LoanInfo.Rows[0]["Occupancy"] == DBNull.Value ? string.Empty : this.LoanInfo.Rows[0]["Occupancy"].ToString();
            if (!string.IsNullOrEmpty(sOccupancy))
            {
                ListItem item = this.ddlOccupancy.Items.FindByText(sOccupancy);
                if (item == null)
                {
                    ddlOccupancy.SelectedValue = "0";
                }
                else
                    ddlOccupancy.SelectedValue = item.Value;
            }
            #endregion
            // Property Type
            DataTable PropertyTypeInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 2729);
            if (PropertyTypeInfo.Rows.Count > 0)
            {
                string cValue = PropertyTypeInfo.Rows[0]["CurrentValue"].ToString();
                string stream = cValue.Trim().ToLower();

                this.ddlPropertyType.SelectedValue = stream;
            }

            //this.txtPropertyType.Text = this.LoanInfo.Rows[0]["PropertyType"] == DBNull.Value ? string.Empty : this.LoanInfo.Rows[0]["PropertyType"].ToString();

            #region Borrower FICO

            LPWeb.Model.LoanPointFields BwrFICOInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 2836);
            if (BwrFICOInfo != null && !string.IsNullOrEmpty(BwrFICOInfo.CurrentValue))
            {
                this.txtBorrowerFICO.Text = BwrFICOInfo.CurrentValue;
            }

            #endregion

            #region First time homebuyer
            if (this.LoanInfo.Rows[0]["FirstTimeBuyer"] != DBNull.Value)
            {
                this.ddlFirsTimeHomebuyer.SelectedValue = Convert.ToBoolean(this.LoanInfo.Rows[0]["FirstTimeBuyer"]) ? "Yes" : "No";
            }

            #endregion

            #region LenderCredit
            tempDecimal = 0;
            // LW 08/30/2013 Changed Lender Credit 2284 --> 812
            LPWeb.Model.LoanPointFields lenderCredit = LoanPointFieldsMgr.GetModel(this.iFileId, 812);
            if (lenderCredit != null)
            {
                decimal.TryParse(lenderCredit.CurrentValue, out tempDecimal);
                if (tempDecimal > 0)
                {
                    this.txtLenderCredit.Text = tempDecimal.ToString("n3");
                }
            }

            #endregion

            #region Price

            // Price
            //if ( LoanProfitInfo != null )
            //{
            //    if (LoanProfitInfo.Price != null)
            //    {
            //        this.txtPrice.Text = LoanProfitInfo.Price.Value.ToString("n3");
            //    }
            //    else
            //    {
            //        this.txtPrice.Text = string.Empty;
            //    }
            //}
            //else
            //{
            //    this.txtPrice.Text = string.Empty;
            //}

            #endregion

            #region LPMI Factor
            tempDecimal = 0;
            LPWeb.Model.LoanPointFields LPMIFactorInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 12973);
            if (LPMIFactorInfo != null)
            {
                decimal.TryParse(LPMIFactorInfo.CurrentValue, out tempDecimal);
                this.txtLPMIFactor.Text = tempDecimal.ToString("n3");
            }

            #endregion

            //#region Compensation Plan
            //if (LoanProfitInfo != null)
            //{
            //    this.ddlCompensationPlan.SelectedValue = string.IsNullOrEmpty(LoanProfitInfo.CompensationPlan) ? "-- select --" : LoanProfitInfo.CompensationPlan;
            //}

            //#endregion

            #region Lock Option
            List<LPWeb.Model.LoanPointFields> lockOptions = LoanPointFieldsMgr.GetModelList(string.Format(" FileId={0} and PointFieldId in (6100, 6101)", iFileId));
            ddlLockOption.SelectedValue = "0";
            if (lockOptions != null && lockOptions.Count > 0)
            {
                int pointFieldId = 0;
                foreach (LPWeb.Model.LoanPointFields lpf in lockOptions)
                {
                    if (lpf == null || string.IsNullOrEmpty(lpf.CurrentValue) || lpf.CurrentValue.ToUpper() == "NO" || lpf.CurrentValue.ToUpper() == "N")
                        continue;
                    pointFieldId = lpf.PointFieldId;
                    break;
                }

                if (pointFieldId > 0)
                {
                    ddlLockOption.SelectedValue = pointFieldId.ToString();
                }
            }
            #endregion

            #region Escrow Taxes

            DataTable EscrowTaxesInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 4003);
            if (EscrowTaxesInfo.Rows.Count > 0)
            {
                string cValue = EscrowTaxesInfo.Rows[0]["CurrentValue"].ToString();
                if ((cValue.Trim().ToUpper() == "Y") || (cValue.Trim().ToUpper() == "X") || (cValue.Trim().ToUpper() == "YES"))
                {
                    this.ddlEscrowTaxes.SelectedValue = "Y";
                }
                else
                {
                    this.ddlEscrowTaxes.SelectedValue = "N";
                }
            }

            #endregion

            #region Escrow Insurance

            DataTable EscrowInsuranceInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 4004);
            if (EscrowInsuranceInfo.Rows.Count > 0)
            {
                string cValue = EscrowInsuranceInfo.Rows[0]["CurrentValue"].ToString();
                if ((cValue.Trim().ToUpper() == "Y") || (cValue.Trim().ToUpper() == "X") || (cValue.Trim().ToUpper() == "YES"))
                {
                    this.ddlEscrowInsurance.SelectedValue = "Y";
                }
                else
                {
                    this.ddlEscrowInsurance.SelectedValue = "N";
                }
            }

            #endregion

            string N_string = "No MI Required";
            string B_string = "Borrower Paid MI";
            string L_string = "Lender Paid MI";
            string O_string = "No MI Option";

            // MI Option
            DataTable MIOptionInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 4018);
            if (MIOptionInfo.Rows.Count > 0)
            {
                string cValue = MIOptionInfo.Rows[0]["CurrentValue"].ToString();
                if (cValue.Trim().ToLower() == N_string.Trim().ToLower())
                {
                    this.ddlMIOption.SelectedValue = "N";
                }

                if (cValue.Trim().ToLower() == B_string.Trim().ToLower())
                {
                    this.ddlMIOption.SelectedValue = "B";
                }

                if (cValue.Trim().ToLower() == L_string.Trim().ToLower())
                {
                    this.ddlMIOption.SelectedValue = "L";
                }

                if (cValue.Trim().ToLower() == O_string.Trim().ToLower())
                {
                    this.ddlMIOption.SelectedValue = "O";
                }

            }

            #region Lock Date
            DateTime tempDate = DateTime.MinValue;
            //if (this.LoanLocksInfo.Rows.Count > 0)
            //{
            //    string sLockTime = this.LoanLocksInfo.Rows[0]["LockTime"] == DBNull.Value ? string.Empty : this.LoanLocksInfo.Rows[0]["LockTime"].ToString();
            //    if (!string.IsNullOrEmpty(sLockTime))
            //    {
            //        if (DateTime.TryParse(sLockTime, out tempDate))
            //            this.txtLockDate.Text = tempDate.ToShortDateString();

            //        this.txtLockDate.Enabled = false;
            //    }
            //}

            DataTable dt_LockDate = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 6061);
            if (dt_LockDate.Rows.Count > 0)
            {
                string cValue = dt_LockDate.Rows[0]["CurrentValue"].ToString();
                string stream = cValue.Trim().ToLower();
                if (!string.IsNullOrEmpty(stream))
                {
                    if (DateTime.TryParse(stream, out tempDate))
                        this.txtLockDate.Text = tempDate.ToShortDateString();

                    this.txtLockDate.Enabled = false;
                }

            }

            #endregion

            #region Lock Term

            //if (this.LoanLocksInfo.Rows.Count > 0)
            //{
            //    string sLockTerm = this.LoanLocksInfo.Rows[0]["LockTerm"] == DBNull.Value ? string.Empty : this.LoanLocksInfo.Rows[0]["LockTerm"].ToString();
            //    this.ddlLockTerm.SelectedValue = sLockTerm.ToString();

            //    if (!string.IsNullOrEmpty(sLockTerm))
            //    {
            //        this.ddlLockTerm.Enabled = false;
            //    }
            //}

            DataTable dt_LockTerm = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 6062);
            if (dt_LockTerm.Rows.Count > 0)
            {
                string cValue = dt_LockTerm.Rows[0]["CurrentValue"].ToString();
                string stream = cValue.Trim().ToLower();
                if (!string.IsNullOrEmpty(stream))
                {
                    this.ddlLockTerm.SelectedValue = stream;
                    if (!string.IsNullOrEmpty(stream))
                    {
                        this.ddlLockTerm.Enabled = false;
                    }
                }

            }

            this.txtExpirationDate.Text = string.Empty;

            DataTable dt_ExpirationDate = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 6063);
            if (dt_ExpirationDate.Rows.Count > 0)
            {
                string cValue = dt_ExpirationDate.Rows[0]["CurrentValue"].ToString();
                string stream = cValue.Trim().ToLower();
                if (!string.IsNullOrEmpty(stream))
                {
                    if (DateTime.TryParse(stream, out tempDate))
                        this.txtExpirationDate.Text = tempDate.ToShortDateString();

                    this.txtExpirationDate.Enabled = false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.txtLockDate.Text) && !string.IsNullOrEmpty(this.ddlLockTerm.SelectedValue))
                    {
                        tempDate = DateTime.MinValue;
                        tempInt = 0;
                        int.TryParse(this.ddlLockTerm.SelectedValue, out tempInt);
                        if (DateTime.TryParse(this.txtLockDate.Text, out tempDate))
                        {
                            DateTime answer = tempDate.AddDays(tempInt);
                            this.txtExpirationDate.Text = answer.ToShortDateString();
                        }
                    }
                }

            #endregion

                #region Expiration Date

                this.txtExt1ExpireDate.Text = string.Empty;
                this.txtExt2ExpireDate.Text = string.Empty;
                this.txtExt3ExpireDate.Text = string.Empty;
                //if (this.LoanLocksInfo.Rows.Count > 0)
                //{
                //    string sExt1LockExpDate = this.LoanLocksInfo.Rows[0]["Ext1LockExpDate"] == DBNull.Value ? string.Empty : this.LoanLocksInfo.Rows[0]["Ext1LockExpDate"].ToString();
                //    string sExt2LockExpDate = this.LoanLocksInfo.Rows[0]["Ext2LockExpDate"] == DBNull.Value ? string.Empty : this.LoanLocksInfo.Rows[0]["Ext2LockExpDate"].ToString();
                //    string sExt3LockExpDate = this.LoanLocksInfo.Rows[0]["Ext3LockExpDate"] == DBNull.Value ? string.Empty : this.LoanLocksInfo.Rows[0]["Ext3LockExpDate"].ToString();

                //    if (!string.IsNullOrEmpty(this.txtLockDate.Text) && !string.IsNullOrEmpty(this.ddlLockTerm.SelectedValue))
                //    {
                //        tempDate = DateTime.MinValue;
                //        tempInt = 0;
                //        int.TryParse(this.ddlLockTerm.SelectedValue, out tempInt);
                //        if (DateTime.TryParse(this.txtLockDate.Text, out tempDate))
                //        {
                //            DateTime answer = tempDate.AddDays(tempInt);
                //            this.txtExpirationDate.Text = answer.ToShortDateString();
                //        }
                //    }
                //    if (!string.IsNullOrEmpty(sExt1LockExpDate))
                //        txtExpirationDate.Text = sExt1LockExpDate;
                //    if (!string.IsNullOrEmpty(sExt2LockExpDate))
                //        txtExpirationDate.Text = sExt2LockExpDate;
                //    if (!string.IsNullOrEmpty(sExt3LockExpDate))
                //        txtExpirationDate.Text = sExt3LockExpDate;
                //    this.txtExpirationDate.Enabled = false;

                //if (!string.IsNullOrEmpty(this.txtExt1LockDate.Text) && !string.IsNullOrEmpty(this.ddlExt1Term.SelectedValue))
                //{
                //    tempDate = DateTime.MinValue;
                //    tempInt = 0;
                //    int.TryParse(this.ddlExt1Term.SelectedValue, out tempInt);
                //    if (DateTime.TryParse(this.txtExt1LockDate.Text, out tempDate))
                //    {
                //        DateTime answer = tempDate.AddDays(tempInt);
                //        this.txtExt1ExpireDate.Text = answer.ToShortDateString();
                //    }
                //}
                //if (!string.IsNullOrEmpty(this.txtExt1ExpireDate.Text))
                //    this.txtExpirationDate.Text = this.txtExt1ExpireDate.Text;

                //this.txtExt1ExpireDate.Enabled = false;

                //if (!string.IsNullOrEmpty(this.txtExt2LockDate.Text) && !string.IsNullOrEmpty(this.ddlExt2Term.SelectedValue))
                //{
                //    tempDate = DateTime.MinValue;
                //    tempInt = 0;
                //    int.TryParse(this.ddlExt2Term.SelectedValue, out tempInt);
                //    if (DateTime.TryParse(this.txtExt2LockDate.Text, out tempDate))
                //    {
                //        DateTime answer = tempDate.AddDays(tempInt);
                //        this.txtExt2ExpireDate.Text = answer.ToShortDateString();
                //    }
                //}
                //if (!string.IsNullOrEmpty(this.txtExt2ExpireDate.Text))
                //    this.txtExpirationDate.Text = this.txtExt2ExpireDate.Text;

                //this.txtExt2ExpireDate.Enabled = false;

                //if (!string.IsNullOrEmpty(this.txtExt3LockDate.Text) && !string.IsNullOrEmpty(this.ddlExt3Term.SelectedValue))
                //{
                //    tempDate = DateTime.MinValue;
                //    tempInt = 0;
                //    int.TryParse(this.ddlExt3Term.SelectedValue, out tempInt);
                //    if (DateTime.TryParse(this.txtExt3LockDate.Text, out tempDate))
                //    {
                //        DateTime answer = tempDate.AddDays(tempInt);
                //        this.txtExt3ExpireDate.Text = answer.ToShortDateString();
                //    }
                //}
                //if (!string.IsNullOrEmpty(this.txtExt3ExpireDate.Text))
                //    this.txtExpirationDate.Text = this.txtExt3ExpireDate.Text;
                //this.txtExt3ExpireDate.Enabled = false;

            }

                #endregion

            #endregion

            #region ARM Caps

            #region Adj. Cap
            tempDecimal = 0;
            LPWeb.Model.LoanPointFields AdjCapInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 2324);
            if (AdjCapInfo != null && !string.IsNullOrEmpty(AdjCapInfo.CurrentValue))
            {
                if (decimal.TryParse(AdjCapInfo.CurrentValue, out tempDecimal))
                {
                    this.txtAdjCap.Text = tempDecimal.ToString("n3");
                }
            }

            #endregion

            #region Life Cap
            tempDecimal = 0;
            LPWeb.Model.LoanPointFields LifeCapInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 2325);
            if (LifeCapInfo != null && !string.IsNullOrEmpty(LifeCapInfo.CurrentValue))
            {
                if (decimal.TryParse(LifeCapInfo.CurrentValue, out tempDecimal))
                {
                    this.txtLifeCap.Text = tempDecimal.ToString("n3");
                }
            }

            #endregion

            #region Initial Adj Cap
            tempDecimal = 0;
            LPWeb.Model.LoanPointFields InitialAdjCapInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 2338);
            if (InitialAdjCapInfo != null && !string.IsNullOrEmpty(InitialAdjCapInfo.CurrentValue))
            {
                if (decimal.TryParse(InitialAdjCapInfo.CurrentValue, out tempDecimal))
                {
                    this.txtInitialAdjCap.Text = tempDecimal.ToString("n3");
                }
            }

            #endregion

            #region Margin
            tempDecimal = 0;
            LPWeb.Model.LoanPointFields MarginInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 2322);
            if (MarginInfo != null && !string.IsNullOrEmpty(MarginInfo.CurrentValue))
            {
                if (decimal.TryParse(MarginInfo.CurrentValue, out tempDecimal))
                {
                    this.txtMargin.Text = tempDecimal.ToString("n3");
                }
            }

            #endregion

            #region Index

            LPWeb.Model.LoanPointFields IndexInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 3896);
            if (IndexInfo != null && !string.IsNullOrEmpty(IndexInfo.CurrentValue))
            {
                this.txtIndex.Text = IndexInfo.CurrentValue;
            }

            #endregion

            #endregion

            #region 绑定Lock Info

            #region Locked By and Confirmed By

            if (this.LoanLocksInfo.Rows.Count > 0)
            {
                string sLockTime = this.LoanLocksInfo.Rows[0]["LockTime"] == DBNull.Value ? string.Empty : this.LoanLocksInfo.Rows[0]["LockTime"].ToString();
                if (DateTime.TryParse(sLockTime, out tempDate))
                    sLockTime = tempDate.ToString("MM/dd/yyyy hh:mm:ss");
                this.lbLockedBy.Text = this.LoanLocksInfo.Rows[0]["LockedBy"].ToString() + " on " + sLockTime;

                string sConfirmTime = this.LoanLocksInfo.Rows[0]["ConfirmTime"] == DBNull.Value ? string.Empty : this.LoanLocksInfo.Rows[0]["ConfirmTime"].ToString();
                if (DateTime.TryParse(sConfirmTime, out tempDate))
                    sConfirmTime = tempDate.ToString("MM/dd/yyyy hh:mm:ss");
                this.lbConfirmedBy.Text = this.LoanLocksInfo.Rows[0]["ConfirmedBy"].ToString() + " on " + sConfirmTime;
            }

            #endregion

            if (this.CurrUser.userRole.ExtendRateLock == false)
            {
                this.txtExt1LockDate.Enabled = false;
                this.ddlExt1Term.Enabled = false;
                this.txtExt1ExpireDate.Enabled = false;
                this.txtExt2LockDate.Enabled = false;
                this.ddlExt2Term.Enabled = false;
                this.txtExt2ExpireDate.Enabled = false;
                this.txtExt3LockDate.Enabled = false;
                this.ddlExt3Term.Enabled = false;
                this.txtExt3ExpireDate.Enabled = false;
            }

            if (this.LoanLocksInfo.Rows.Count > 0)
            {
                #region Extension I

                this.txtExt1LockDate.Text = this.LoanLocksInfo.Rows[0]["Ext1LockTime"] == DBNull.Value ? string.Empty : Convert.ToDateTime(this.LoanLocksInfo.Rows[0]["Ext1LockTime"]).ToString("MM/dd/yyyy");
                if (this.txtExt1LockDate.Text != string.Empty)
                {
                    this.txtExt1LockDate.Enabled = false;
                }

                string sExt1Term = this.LoanLocksInfo.Rows[0]["Ext1Term"].ToString();
                this.ddlExt1Term.SelectedValue = sExt1Term;
                if (sExt1Term != string.Empty)
                {
                    this.ddlExt1Term.Enabled = false;
                }

                if (this.txtExt1LockDate.Text == string.Empty)
                {
                    this.txtExt1ExpireDate.Text = string.Empty;
                }
                else
                {
                    this.txtExt1ExpireDate.Text = this.LoanLocksInfo.Rows[0]["Ext1LockExpDate"] == DBNull.Value ? string.Empty : Convert.ToDateTime(this.LoanLocksInfo.Rows[0]["Ext1LockExpDate"]).ToString("MM/dd/yyyy");
                    if (this.txtExt1ExpireDate.Text != string.Empty)
                    {
                        this.txtExt1ExpireDate.Enabled = false;
                    }
                }



                #endregion

                #region Extension II

                this.txtExt2LockDate.Text = this.LoanLocksInfo.Rows[0]["Ext2LockTime"] == DBNull.Value ? string.Empty : Convert.ToDateTime(this.LoanLocksInfo.Rows[0]["Ext2LockTime"]).ToString("MM/dd/yyyy");
                if (this.txtExt2LockDate.Text != string.Empty)
                {
                    this.txtExt2LockDate.Enabled = false;
                }

                string sExt2Term = this.LoanLocksInfo.Rows[0]["Ext2Term"].ToString();
                this.ddlExt2Term.SelectedValue = sExt2Term;
                if (sExt2Term != string.Empty)
                {
                    this.ddlExt2Term.Enabled = false;
                }

                if (this.txtExt2LockDate.Text == string.Empty)
                {
                    this.txtExt2ExpireDate.Text = string.Empty;
                }
                else
                {
                    this.txtExt2ExpireDate.Text = this.LoanLocksInfo.Rows[0]["Ext2LockExpDate"] == DBNull.Value ? string.Empty : Convert.ToDateTime(this.LoanLocksInfo.Rows[0]["Ext2LockExpDate"]).ToString("MM/dd/yyyy");
                    if (this.txtExt2ExpireDate.Text != string.Empty)
                    {
                        this.txtExt2ExpireDate.Enabled = false;
                    }
                }

                #endregion

                #region Extension III

                this.txtExt3LockDate.Text = this.LoanLocksInfo.Rows[0]["Ext3LockTime"] == DBNull.Value ? string.Empty : Convert.ToDateTime(this.LoanLocksInfo.Rows[0]["Ext3LockTime"]).ToString("MM/dd/yyyy");
                if (this.txtExt3LockDate.Text != string.Empty)
                {
                    this.txtExt3LockDate.Enabled = false;
                }

                string sExt3Term = this.LoanLocksInfo.Rows[0]["Ext3Term"].ToString();
                this.ddlExt3Term.SelectedValue = sExt3Term;
                if (sExt3Term != string.Empty)
                {
                    this.ddlExt3Term.Enabled = false;
                }

                if (this.txtExt3LockDate.Text == string.Empty)
                {
                    this.txtExt3ExpireDate.Text = string.Empty;
                }
                else
                {
                    this.txtExt3ExpireDate.Text = this.LoanLocksInfo.Rows[0]["Ext3LockExpDate"] == DBNull.Value ? string.Empty : Convert.ToDateTime(this.LoanLocksInfo.Rows[0]["Ext3LockExpDate"]).ToString("MM/dd/yyyy");
                    if (this.txtExt3ExpireDate.Text != string.Empty)
                    {
                        this.txtExt3ExpireDate.Enabled = false;
                    }
                }

                #endregion

            }

            #endregion
        }
    }
    private void BindInvestors(DataTable LoanLockInfo)
    {
        string strSql = @"Select DISTINCT cc.Name as Investor, cc.ContactCompanyId as InvestorId from ContactCompanies cc left join ServiceTypes st on cc.ServiceTypeId=st.ServiceTypeId where st.[Name]='Investor' and cc.Enabled=1 and cc.Name is not null order by cc.[Name] asc";
        try
        {
            DataTable dt = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(strSql);

            DataRow NewdtRow = dt.NewRow();
            NewdtRow["InvestorID"] = "0";
            NewdtRow["Investor"] = "—select—";
            dt.Rows.InsertAt(NewdtRow, 0);
            dt.AcceptChanges();

            ddlInvestor.DataSource = dt;
            ddlInvestor.DataBind();


            if (this.LoanLocksInfo != null && this.LoanLocksInfo.Rows.Count > 0)
            {
                string InvestorID = this.LoanLocksInfo.Rows[0]["InvestorID"] == DBNull.Value ? string.Empty : this.LoanLocksInfo.Rows[0]["InvestorID"].ToString();
                string Investor = this.LoanLocksInfo.Rows[0]["Investor"] == DBNull.Value ? string.Empty : this.LoanLocksInfo.Rows[0]["Investor"].ToString();
                ListItem item = null;
                if (!string.IsNullOrEmpty(InvestorID))
                {
                    item = this.ddlInvestor.Items.FindByValue(InvestorID);
                    if (item == null)
                    {
                        ddlInvestor.Items.Add(new ListItem(this.LoanLocksInfo.Rows[0]["Investor"].ToString(), this.LoanLocksInfo.Rows[0]["InvestorID"].ToString()));
                    }
                    ddlInvestor.SelectedValue = InvestorID;
                    return;
                }
                else if (!string.IsNullOrEmpty(Investor))
                {
                    item = this.ddlInvestor.Items.FindByText(Investor);
                    if (item == null)
                    {
                        item = new ListItem(Investor, string.Empty);
                        ddlInvestor.Items.Add(item);
                    }
                    item.Selected = true; //.SelectedItem.Text = Investor;
                }
                else
                {
                    ddlInvestor.SelectedValue = "0";
                    //var it = new ListItem(string.Empty, string.Empty);
                    //it.Selected = true;
                    //ddlInvestor.Items.Add(it);
                }
            }
            else
            {
                ddlInvestor.SelectedValue = "0";
                //var it = new ListItem(string.Empty, string.Empty);
                //it.Selected = true;
                //ddlInvestor.Items.Add(it);
            }

        }
        catch (Exception ex)
        { }
    }

    private DataTable BindLoanProgramsWithInvestorId(int InvestorID)
    {
        DataTable ProgramList = null;
        string sSql = string.Empty;
        if (InvestorID <= 0)
        {
            //return ProgramList;
            sSql = string.Format("select distinct clp.LoanProgramID,clp.LoanProgram,clp.IsARM from Company_Loan_Programs clp left join Company_LoanProgramDetails clpd on clp.LoanProgramID=clpd.LoanProgramID  where clpd.InvestorID IS NULL order by clp.LoanProgram asc");
        }
        else
        {//select distinct clp.*, clpd.Enabled, clpd.InvestorID, clpd.FirstAdj, clpd.IndexType, clpd.LifetimeCap, clpd.Margin, clpd.SubAdj from
            sSql = string.Format("select distinct clp.LoanProgramID,clp.LoanProgram,clp.IsARM  from Company_Loan_Programs clp left join Company_LoanProgramDetails clpd on clp.LoanProgramID=clpd.LoanProgramID  where clpd.InvestorId={0} or clpd.InvestorID IS NULL order by clp.LoanProgram asc", InvestorID);
        }
        ProgramList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        return ProgramList;
    }

    private string GetLoanProgramIDWithLoanProgram(string LoanProgram)
    {
        string LoanProgramID = "";
        string sSql = string.Empty;

        sSql = string.Format("Select [LoanProgramID] from Company_Loan_Programs where LoanProgram='{0}'", LoanProgram);
        object obj = LPWeb.DAL.DbHelperSQL.GetSingle(sSql);
        LoanProgramID = obj == null ? string.Empty : obj.ToString();

        return LoanProgramID;
    }

    private void BindLoanProgram(DataTable LoanLockInfo)
    {
        Company_Loan_Programs ProgramMgr = new Company_Loan_Programs();
        DataTable ProgramList = null;
        ListItem item = null;
        string currentProgramID = string.Empty;
        string currentProgram = string.Empty;
        int InvestorID = 0;
        this.ddlLoanProgram.Items.Add(new ListItem("-- select --", ""));
        if (LoanLockInfo.Rows.Count > 0 && LoanLockInfo.Rows[0] != null && LoanLockInfo.Rows[0]["InvestorID"] != DBNull.Value)
        {
            InvestorID = LoanLockInfo.Rows[0]["InvestorID"] == DBNull.Value ? 0 : (int)LoanLockInfo.Rows[0]["InvestorID"];
        }
        //if (InvestorID > 0)
        //    ProgramList = BindLoanProgramsWithInvestorId(InvestorID);
        //else 
        //    ProgramList = ProgramMgr.GetList("1=1 order by LoanProgram").Tables[0];

        ProgramList = BindLoanProgramsWithInvestorId(InvestorID);
        //foreach (DataRow ProgramRow in ProgramList.Rows)
        //{
        //    currentProgramID = ProgramRow["LoanProgramID"].ToString();
        //    currentProgram = ProgramRow["LoanProgram"].ToString().Trim();
        //}
        DataRow NewProgramRow = ProgramList.NewRow();
        NewProgramRow["LoanProgramID"] = DBNull.Value;
        NewProgramRow["LoanProgram"] = "—select—";
        ProgramList.Rows.InsertAt(NewProgramRow, 0);
        ProgramList.AcceptChanges();

        ddlLoanProgram.DataSource = ProgramList;
        ddlLoanProgram.DataBind();


        if (LoanLockInfo.Rows.Count > 0 && LoanLockInfo.Rows[0] != null && LoanLockInfo.Rows[0]["ProgramID"] != DBNull.Value)
        {
            currentProgramID = LoanLockInfo.Rows[0]["ProgramID"] == DBNull.Value ? string.Empty : LoanLockInfo.Rows[0]["ProgramID"].ToString();
            currentProgram = LoanLockInfo.Rows[0]["Program"] == DBNull.Value ? string.Empty : LoanLockInfo.Rows[0]["Program"].ToString();
            if (!string.IsNullOrEmpty(currentProgramID))
                item = ddlLoanProgram.Items.FindByValue(currentProgramID);
            else if (!string.IsNullOrEmpty(currentProgram))
                item = ddlLoanProgram.Items.FindByText(currentProgram);
        }
        if (item == null)
        {
            item = new ListItem(currentProgram, currentProgramID);
            ddlLoanProgram.Items.Add(item);
        }

        if (currentProgram == "")
        {
            currentProgram = "—select—";
            currentProgramID = "0";
        }

        ddlLoanProgram.SelectedItem.Text = currentProgram;

        string sLoanProgramId = currentProgramID;
        int LoanProgramId = 0;
        int.TryParse(sLoanProgramId, out LoanProgramId);
        BindLoanProgramWithLoanProgramId(LoanProgramId);

        foreach (DataRow ProgramRow in ProgramList.Rows)
        {
            string sLoanProgramID = ProgramRow["LoanProgramID"].ToString();
            string sLoanProgram = ProgramRow["LoanProgram"].ToString().Trim();
            string sIsARM = ProgramRow["IsARM"] == DBNull.Value ? "false" : ProgramRow["IsARM"].ToString().ToLower();
            //string sInvestorID = ProgramRow["InvestorID"] != DBNull.Value ? ProgramRow["InvestorID"].ToString() : "";

            //var listitem = new ListItem();
            ////listitem.Attributes.Add("InvestorID", sInvestorID);
            ////listitem.Attributes.Add("LoanProgramID", sLoanProgramID);
            ////listitem.Attributes.Add("sIsARM", sLoanProgram);
            //listitem.Text = sLoanProgram;
            //listitem.Value = sLoanProgramID;
            //this.ddlLoanProgram.Items.Add(listitem);


            //this.ddlLoanProgram.Items.Add(sLoanProgram);

            this.ddlLoanProgramARM.Items.Add(new ListItem(sIsARM, sLoanProgramID));

            this.ddlLoanProgramID.Items.Add(new ListItem(sLoanProgramID, sLoanProgram));
        }
    }

    private void BindLoanProgramWithLoanProgramId(int LoanProgramId)
    {
        Company_Loan_Programs ProgramMgr = new Company_Loan_Programs();
        DataTable ProgramList = null;
        ListItem item = null;
        int currentLoanProgramId = LoanProgramId;
        string currentProgramID = string.Empty;
        string currentProgram = string.Empty;
        int iInvestorID = 0;
        int iTemp = 0;

        if (LoanProgramId == 0)
        {
            this.txtMargin.Text = string.Empty;
            this.txtInitialAdjCap.Text = string.Empty;
            this.txtAdjCap.Text = string.Empty;
            this.txtLifeCap.Text = string.Empty;
            this.txtIndex.Text = string.Empty;

            return;
        }

        string sIndexType = string.Empty;
        LoanLocks LoanLocksMgr = new LoanLocks();
        DataTable LoanLockInfo = LoanLocksMgr.GetLoanLocksInfo(this.iFileId);
        string sInvestorID = this.ddlInvestor.SelectedValue.Trim();
        if (string.IsNullOrEmpty(sInvestorID))
        {
            PageCommon.AlertMsg(this, "Cannot find the Investor in the database.");
            return;
        }
        if (!int.TryParse(sInvestorID, out iInvestorID))
        {
            PageCommon.AlertMsg(this, "The selected InvestorID is invalid.");
            return;
        }

        //LoanPointFields LoanPointFieldsMgr = new LoanPointFields();
        //LPWeb.Model.LoanPointFields IndexInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 3896);
        //if (IndexInfo != null && !string.IsNullOrEmpty(IndexInfo.CurrentValue))
        //{
        //    sIndexType = IndexInfo.CurrentValue;
        //}

        decimal tempDecimal = 0;
        LPWeb.BLL.Company_LoanProgramDetails _bllLoanProgramDetail = new Company_LoanProgramDetails();
        LPWeb.Model.Company_LoanProgramDetails LoanProgramDetailsInfo = _bllLoanProgramDetail.GetModel(LoanProgramId, iInvestorID);
        if (LoanProgramDetailsInfo == null)
        {
            this.txtMargin.Text = string.Empty;
            this.txtInitialAdjCap.Text = string.Empty;
            this.txtAdjCap.Text = string.Empty;
            this.txtLifeCap.Text = string.Empty;
            this.txtIndex.Text = string.Empty;

            return;
        }

        tempDecimal = LoanProgramDetailsInfo.Margin == null ? 0 : (decimal)LoanProgramDetailsInfo.Margin;
        this.txtMargin.Text = tempDecimal.ToString("n3");
        tempDecimal = LoanProgramDetailsInfo.FirstAdj == null ? 0 : (decimal)LoanProgramDetailsInfo.FirstAdj;
        this.txtInitialAdjCap.Text = tempDecimal.ToString("n3");
        tempDecimal = LoanProgramDetailsInfo.SubAdj == null ? 0 : (decimal)LoanProgramDetailsInfo.SubAdj;
        this.txtAdjCap.Text = tempDecimal.ToString("n3");
        tempDecimal = LoanProgramDetailsInfo.LifetimeCap == null ? 0 : (decimal)LoanProgramDetailsInfo.LifetimeCap;
        this.txtLifeCap.Text = tempDecimal.ToString("n3");
        this.txtIndex.Text = LoanProgramDetailsInfo.IndexType;
        this.txtTerm.Text = LoanProgramDetailsInfo.Term == null ? string.Empty : LoanProgramDetailsInfo.Term.ToString();
        this.txtDue.Text = LoanProgramDetailsInfo.Due == null ? string.Empty : LoanProgramDetailsInfo.Due.ToString();
    }

    protected void ddlInvestor_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sInvestorID = ddlInvestor.SelectedValue;
        int InvestorID = 0;
        int.TryParse(sInvestorID, out InvestorID);
        DataTable programList = BindLoanProgramsWithInvestorId(InvestorID);

        DataRow NewProgramRow = programList.NewRow();
        NewProgramRow["LoanProgramID"] = DBNull.Value;
        NewProgramRow["LoanProgram"] = "—select—";
        programList.Rows.InsertAt(NewProgramRow, 0);
        programList.AcceptChanges();
        ddlLoanProgram.DataSource = programList;
        ddlLoanProgram.DataBind();
        ddlLoanProgram.SelectedItem.Text = "—select—";

        string sLoanProgramId = ddlLoanProgram.SelectedValue;
        int LoanProgramId = 0;
        int.TryParse(sLoanProgramId, out LoanProgramId);
        BindLoanProgramWithLoanProgramId(LoanProgramId);
    }

    protected void ddlLoanProgram_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sLoanProgramId = ddlLoanProgram.SelectedValue;
        int LoanProgramId = 0;
        int.TryParse(sLoanProgramId, out LoanProgramId);
        BindLoanProgramWithLoanProgramId(LoanProgramId);
    }

    private void CheckUserPrivileges()
    {
        if (this.CurrUser.userRole.ViewLockInfo == true)
        {
            if (this.CurrUser.userRole.LockRate == false && this.CurrUser.userRole.ExtendRateLock == false) // only view
            {
                this.hdnViewLockRate.Value = "true";
            }
            else
            {
                this.hdnViewLockRate.Value = "false";
            }

            if (this.CurrUser.userRole.LockRate == true)
            {
                this.btnSave.Enabled = true;
                //this.btnSyncNow.Enabled = true;
                this.txtLender.Enabled = true;
                //this.txtLoanAmount.Enabled = true;
                //this.chkLoanChanged.Enabled = true;
                this.txtRate.Enabled = true;
                this.txtTerm.Enabled = true;
                this.txtDue.Enabled = true;
                //this.txtLTV.Enabled = true;
                this.txtPurpose.Enabled = true;
                this.ddlLoanProgram.Enabled = true;
                this.ddlLoanProgramARM.Enabled = true;
                this.ddlLoanProgramID.Enabled = true;
                this.txtOccupancy.Enabled = true;
                this.ddlPropertyType.Enabled = true;
                this.txtBorrowerFICO.Enabled = true;
                this.ddlFirsTimeHomebuyer.Enabled = true;
                this.txtLenderCredit.Enabled = true;
                this.txtPrice.Enabled = true;
                this.txtLPMIFactor.Enabled = true;
                this.ddlCompensationPlan.Enabled = true;
                this.ddlLockOption.Enabled = true;
                this.ddlEscrowTaxes.Enabled = true;
                this.ddlEscrowInsurance.Enabled = true;
                this.ddlMIOption.Enabled = true;
                this.txtLockDate.Enabled = true;
                this.ddlLockTerm.Enabled = true;
                this.txtExpirationDate.Enabled = false;
                this.txtAdjCap.Enabled = true;
                this.txtLifeCap.Enabled = true;
                this.txtInitialAdjCap.Enabled = true;
                this.txtMargin.Enabled = true;
                this.txtIndex.Enabled = true;
                this.lbLockedBy.Enabled = true;
                this.lbConfirmedBy.Enabled = true;
            }
            else
            {
                this.btnSave.Enabled = false;
                //this.btnSyncNow.Enabled = false;
                this.txtLender.Enabled = false;
                //this.txtLoanAmount.Enabled = false;
                //this.chkLoanChanged.Enabled = false;
                this.txtRate.Enabled = false;
                this.txtTerm.Enabled = false;
                this.txtDue.Enabled = false;
                //this.txtLTV.Enabled = false;
                this.txtPurpose.Enabled = false;
                this.ddlLoanProgram.Enabled = false;
                this.ddlLoanProgramARM.Enabled = false;
                this.ddlLoanProgramID.Enabled = false;
                this.txtOccupancy.Enabled = false;
                this.ddlPropertyType.Enabled = false;
                this.txtBorrowerFICO.Enabled = false;
                this.ddlFirsTimeHomebuyer.Enabled = false;
                this.txtLenderCredit.Enabled = false;
                this.txtPrice.Enabled = false;
                this.txtLPMIFactor.Enabled = false;
                this.ddlCompensationPlan.Enabled = false;
                this.ddlLockOption.Enabled = false;
                this.ddlEscrowTaxes.Enabled = false;
                this.ddlEscrowInsurance.Enabled = false;
                this.ddlMIOption.Enabled = false;
                this.txtLockDate.Enabled = false;
                this.ddlLockTerm.Enabled = false;
                this.txtExpirationDate.Enabled = false;
                this.txtAdjCap.Enabled = false;
                this.txtLifeCap.Enabled = false;
                this.txtInitialAdjCap.Enabled = false;
                this.txtMargin.Enabled = false;
                this.txtIndex.Enabled = false;
                this.lbLockedBy.Enabled = false;
                this.lbConfirmedBy.Enabled = false;
            }

            if (this.CurrUser.userRole.ExtendRateLock == true)
            {
                this.txtExt1LockDate.Enabled = true;
                this.ddlExt1Term.Enabled = true;
                this.txtExt1ExpireDate.Enabled = false;
                this.txtExt2LockDate.Enabled = true;
                this.ddlExt2Term.Enabled = true;
                this.txtExt2ExpireDate.Enabled = false;
                this.txtExt3LockDate.Enabled = true;
                this.ddlExt3Term.Enabled = true;
                this.txtExt3ExpireDate.Enabled = false;
            }
            else
            {
                this.txtExt1LockDate.Enabled = false;
                this.ddlExt1Term.Enabled = false;
                this.txtExt1ExpireDate.Enabled = false;
                this.txtExt2LockDate.Enabled = false;
                this.ddlExt2Term.Enabled = false;
                this.txtExt2ExpireDate.Enabled = false;
                this.txtExt3LockDate.Enabled = false;
                this.ddlExt3Term.Enabled = false;
                this.txtExt3ExpireDate.Enabled = false;
            }
        }
        else
        {
            if (this.CurrUser.userRole.LockRate == false && this.CurrUser.userRole.ExtendRateLock == false)
            {
                PageCommon.WriteJsEnd(this, "You do not have the privilege to view the lock information or lock the rate.", "window.parent.CloseGlobalPopup();");
                return;
            }

            if (this.CurrUser.userRole.LockRate == true)
            {
                this.btnSave.Enabled = true;
                //this.btnSyncNow.Enabled = true;
                this.txtLender.Enabled = true;
                //this.txtLoanAmount.Enabled = true;
                //this.chkLoanChanged.Enabled = true;
                this.txtRate.Enabled = true;
                this.txtTerm.Enabled = true;
                this.txtDue.Enabled = true;
                //this.txtLTV.Enabled = true;
                this.txtPurpose.Enabled = true;
                this.ddlLoanProgram.Enabled = true;
                this.ddlLoanProgramARM.Enabled = true;
                this.ddlLoanProgramID.Enabled = true;
                this.txtOccupancy.Enabled = true;
                this.ddlPropertyType.Enabled = true;
                this.txtBorrowerFICO.Enabled = true;
                this.ddlFirsTimeHomebuyer.Enabled = true;
                this.txtLenderCredit.Enabled = true;
                this.txtPrice.Enabled = true;
                this.txtLPMIFactor.Enabled = true;
                this.ddlCompensationPlan.Enabled = true;
                this.ddlLockOption.Enabled = true;
                this.ddlEscrowTaxes.Enabled = true;
                this.ddlEscrowInsurance.Enabled = true;
                this.ddlMIOption.Enabled = true;
                this.txtLockDate.Enabled = true;
                this.ddlLockTerm.Enabled = true;
                this.txtExpirationDate.Enabled = false;
                this.txtAdjCap.Enabled = true;
                this.txtLifeCap.Enabled = true;
                this.txtInitialAdjCap.Enabled = true;
                this.txtMargin.Enabled = true;
                this.txtIndex.Enabled = true;
                this.lbLockedBy.Enabled = true;
                this.lbConfirmedBy.Enabled = true;
            }
            else
            {
                this.btnSave.Enabled = false;
                //this.btnSyncNow.Enabled = false;
                this.txtLender.Enabled = false;
                //this.txtLoanAmount.Enabled = false;
                //this.chkLoanChanged.Enabled = false;
                this.txtRate.Enabled = false;
                this.txtTerm.Enabled = false;
                this.txtDue.Enabled = false;
                //this.txtLTV.Enabled = false;
                this.txtPurpose.Enabled = false;
                this.ddlLoanProgram.Enabled = false;
                this.ddlLoanProgramARM.Enabled = false;
                this.ddlLoanProgramID.Enabled = false;
                this.txtOccupancy.Enabled = false;
                this.ddlPropertyType.Enabled = false;
                this.txtBorrowerFICO.Enabled = false;
                this.ddlFirsTimeHomebuyer.Enabled = false;
                this.txtLenderCredit.Enabled = false;
                this.txtPrice.Enabled = false;
                this.txtLPMIFactor.Enabled = false;
                this.ddlCompensationPlan.Enabled = false;
                this.ddlLockOption.Enabled = false;
                this.ddlEscrowTaxes.Enabled = false;
                this.ddlEscrowInsurance.Enabled = false;
                this.ddlMIOption.Enabled = false;
                this.txtLockDate.Enabled = false;
                this.ddlLockTerm.Enabled = false;
                this.txtExpirationDate.Enabled = false;
                this.txtAdjCap.Enabled = false;
                this.txtLifeCap.Enabled = false;
                this.txtInitialAdjCap.Enabled = false;
                this.txtMargin.Enabled = false;
                this.txtIndex.Enabled = false;
                this.lbLockedBy.Enabled = false;
                this.lbConfirmedBy.Enabled = false;
            }

            if (this.CurrUser.userRole.ExtendRateLock == true)
            {
                this.txtExt1LockDate.Enabled = true;
                this.ddlExt1Term.Enabled = true;
                this.txtExt1ExpireDate.Enabled = false;
                this.txtExt2LockDate.Enabled = true;
                this.ddlExt2Term.Enabled = true;
                this.txtExt2ExpireDate.Enabled = false;
                this.txtExt3LockDate.Enabled = true;
                this.ddlExt3Term.Enabled = true;
                this.txtExt3ExpireDate.Enabled = false;
            }
            else
            {
                this.txtExt1LockDate.Enabled = false;
                this.ddlExt1Term.Enabled = false;
                this.txtExt1ExpireDate.Enabled = false;
                this.txtExt2LockDate.Enabled = false;
                this.ddlExt2Term.Enabled = false;
                this.txtExt2ExpireDate.Enabled = false;
                this.txtExt3LockDate.Enabled = false;
                this.ddlExt3Term.Enabled = false;
                this.txtExt3ExpireDate.Enabled = false;
            }

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region 获取用户输入

        string sInvestorID = this.ddlInvestor.SelectedValue.Trim();
        string sddlPropertyType = this.ddlPropertyType.SelectedValue.Trim();
        string sInvestorName = this.ddlInvestor.SelectedItem.Text.Trim();
        string sLoanProgram = this.ddlLoanProgram.SelectedItem.Text.Trim();
        string sLoanProgramID = this.ddlLoanProgram.SelectedValue.Trim();
        if (sLoanProgram.ToUpper().Contains("-- SELECT"))
        {
            sLoanProgramID = "0";
            sLoanProgram = string.Empty;
        }
        string sFirsTimeHomebuyer = this.ddlFirsTimeHomebuyer.SelectedValue;
        string sLenderCredit = this.txtLenderCredit.Text.Trim();
        string sPrice = this.txtPrice.Text.Trim();

        string sLPMIFactor = this.txtLPMIFactor.Text.Trim();
        string sCompensationPlan = this.ddlCompensationPlan.SelectedValue;
        string sLockOption = this.ddlLockOption.SelectedValue.Trim() == "0" ? string.Empty : this.ddlLockOption.SelectedItem.Text.Trim();

        string sEscrowTaxes = this.ddlEscrowTaxes.SelectedValue;
        string sEscrowInsurance = this.ddlEscrowInsurance.SelectedValue;
        string sMIOption = this.ddlMIOption.SelectedItem.Text.Trim();
        string sPurpose = this.ddlPurpose.SelectedValue == "0" ? "" : this.ddlPurpose.SelectedItem.Text.Trim();
        // Lock Info
        string sLockDate = this.txtLockDate.Text.Trim();
        string sLockTerm = this.ddlLockTerm.SelectedValue;

        int tempInt = 0;
        DateTime tempDate = DateTime.MinValue;

        if (!string.IsNullOrEmpty(this.txtLockDate.Text) && !string.IsNullOrEmpty(this.ddlLockTerm.SelectedValue))
        {
            tempDate = DateTime.MinValue;
            tempInt = 0;
            int.TryParse(this.ddlLockTerm.SelectedValue, out tempInt);
            if (DateTime.TryParse(this.txtLockDate.Text, out tempDate))
            {
                DateTime answer = tempDate.AddDays(tempInt);
                //this.txtExpirationDate.Text = answer.ToShortDateString();
            }
        }

        string sExpirationDate = this.txtExpirationDate.Text.Trim();

        string sAdjCap = this.txtAdjCap.Text.Trim();
        string sLifeCap = this.txtLifeCap.Text.Trim();
        string sInitialAdjCap = this.txtInitialAdjCap.Text.Trim();
        string sMargin = this.txtMargin.Text.Trim();
        string sIndex = this.txtIndex.Text.Trim();

        string sExt1LockDate = this.txtExt1LockDate.Text.Trim();
        string sExt1Term = this.ddlExt1Term.SelectedValue;

        string sExt2LockDate = this.txtExt2LockDate.Text.Trim();
        string sExt2Term = this.ddlExt2Term.SelectedValue;

        string sExt3LockDate = this.txtExt3LockDate.Text.Trim();
        string sExt3Term = this.ddlExt3Term.SelectedValue;

        string sBorrowerFICO = this.txtBorrowerFICO.Text.Trim();

        //Fixed BUG254
        decimal dRate = 0;      //Rate
        int iTerm = 0;          //Term
        int iDue = 0;           //Due

        if ((this.txtExt1LockDate.Text != string.Empty) && (this.ddlExt1Term.SelectedValue != string.Empty))
        {
            try
            {
                DateTime dt = Convert.ToDateTime(this.txtExt1LockDate.Text);
                double db = Convert.ToDouble(this.ddlExt1Term.SelectedValue);
                DateTime answer = dt.AddDays(db);
                this.txtExt1ExpireDate.Text = answer.ToShortDateString();
            }
            catch
            {
                this.txtExt1ExpireDate.Text = string.Empty;
            }
        }
        else
        {
            this.txtExt1ExpireDate.Text = string.Empty;
        }

        this.txtExt1ExpireDate.Enabled = false;

        if ((this.txtExt2LockDate.Text != string.Empty) && (this.ddlExt2Term.SelectedValue != string.Empty))
        {
            try
            {
                DateTime dt = Convert.ToDateTime(this.txtExt2LockDate.Text);
                double db = Convert.ToDouble(this.ddlExt2Term.SelectedValue);
                DateTime answer = dt.AddDays(db);
                this.txtExt2ExpireDate.Text = answer.ToShortDateString();
            }
            catch
            {
                this.txtExt2ExpireDate.Text = string.Empty;
            }
        }
        else
        {
            this.txtExt2ExpireDate.Text = string.Empty;
        }

        this.txtExt2ExpireDate.Enabled = false;

        if ((this.txtExt3LockDate.Text != string.Empty) && (this.ddlExt3Term.SelectedValue != string.Empty))
        {
            try
            {
                DateTime dt = Convert.ToDateTime(this.txtExt3LockDate.Text);
                double db = Convert.ToDouble(this.ddlExt3Term.SelectedValue);
                DateTime answer = dt.AddDays(db);
                this.txtExt3ExpireDate.Text = answer.ToShortDateString();
            }
            catch
            {
                this.txtExt3ExpireDate.Text = string.Empty;
            }
        }
        else
        {
            this.txtExt3ExpireDate.Text = string.Empty;
        }

        this.txtExt3ExpireDate.Enabled = false;

        string sExt1ExpireDate = this.txtExt1ExpireDate.Text.Trim();
        string sExt2ExpireDate = this.txtExt2ExpireDate.Text.Trim();
        string sExt3ExpireDate = this.txtExt3ExpireDate.Text.Trim();
        string sOccupancy = this.ddlOccupancy.SelectedItem.Text.Trim();
        if (this.ddlOccupancy.SelectedValue == "0")
        {
            sOccupancy = "";
        }

        #endregion

        #region Investor

        if (!string.IsNullOrEmpty(sInvestorID))
        {
            // 获取Lender下的Contact ID
            Contacts ContactsMrg = new Contacts();
            DataTable FirstContactInfo = ContactsMrg.GetList(1, " ContactCompanyId=" + sInvestorID + " and Enabled=1 ", "ContactId").Tables[0];
            if (FirstContactInfo.Rows.Count > 0)
            {
                int iNewContactId = Convert.ToInt32(FirstContactInfo.Rows[0]["ContactId"]);

                // 检查LoanContacts是否存在
                LoanContacts LoanContactsMgr = new LoanContacts();
                DataTable LoanContactList = LoanContactsMgr.GetList(" FileId=" + this.iFileId + " and ContactRoleId = (select ContactRoleId from ContactRoles where Name='Investor')").Tables[0];

                if (LoanContactList.Rows.Count > 0)     // 如果存在->Update
                {
                    int iContactRoleId = Convert.ToInt32(LoanContactList.Rows[0]["ContactRoleId"]);

                    #region update

                    LoanContactsMgr.UpdateContactId(this.iFileId, iContactRoleId, iNewContactId);

                    #endregion
                }
                else // 如果不存在->Insert
                {
                    // 获取Lender->ContactRoleId
                    ContactRoles ContactRolesMgr = new ContactRoles();
                    DataTable InvestorRoleId = ContactRolesMgr.GetContactRoleList(" and Name='Investor'");

                    if (InvestorRoleId.Rows.Count > 0)
                    {
                        int iContactRoleId = Convert.ToInt32(InvestorRoleId.Rows[0]["ContactRoleId"]);

                        #region insert

                        LPWeb.Model.LoanContacts LoanContactsModel = new LPWeb.Model.LoanContacts();
                        LoanContactsModel.FileId = this.iFileId;
                        LoanContactsModel.ContactRoleId = iContactRoleId;
                        LoanContactsModel.ContactId = iNewContactId;

                        LoanContactsMgr.Add(LoanContactsModel);

                        #endregion
                    }
                }
            }
        }

        #endregion

        #region Loans table update, Purpose, Loan Program, Occupancy, MIOption, FirstTimeHomeBuyer

        Loans LoansMgr = new Loans();
        LPWeb.Model.Loans loan = LoansMgr.GetModel(this.iFileId);
        if (loan != null)
        {

            loan.Program = sLoanProgram;
            loan.Purpose = sPurpose;
            loan.Occupancy = sOccupancy;
            loan.MIOption = sMIOption;
            loan.FirstTimeHomeBuyer = sFirsTimeHomebuyer.Contains("Y") ? true : false;

            //Fixed BUG254
            if (this.txtRate.Enabled == true && this.txtRate.Text.Trim() != "" && Decimal.TryParse(this.txtRate.Text.Trim(), out dRate) == true)
            {
                loan.Rate = dRate;
            }
            if (this.txtTerm.Enabled == true && this.txtTerm.Text.Trim() != "" && Int32.TryParse(this.txtTerm.Text.Trim(), out iTerm) == true)
            {
                loan.Term = iTerm;
            }
            if (this.txtDue.Enabled == true && this.txtDue.Text.Trim() != "" && Int32.TryParse(this.txtDue.Text.Trim(), out iDue) == true)
            {
                loan.Due = iDue;
            }

            if (!string.IsNullOrEmpty(txtExt1ExpireDate.Text))
                sExpirationDate = txtExt1ExpireDate.Text;
            if (!string.IsNullOrEmpty(txtExt2ExpireDate.Text))
                sExpirationDate = txtExt2ExpireDate.Text;
            if (!string.IsNullOrEmpty(txtExt3ExpireDate.Text))
                sExpirationDate = txtExt3ExpireDate.Text;

            if (!string.IsNullOrEmpty(sExpirationDate))
            {
                DateTime dt3 = DateTime.MinValue;
                if (DateTime.TryParse(sExpirationDate, out dt3))
                {
                    loan.RateLockExpiration = dt3;
                }

            }

            if (loan.Program.ToUpper().Contains("SELECT"))
            {
                loan.Program = string.Empty;
            }

            LoansMgr.Update(loan);
        }
        #endregion

        decimal tempDecimal = 0;
        #region LenderCredit and Compensation Plan
        bool insert = false;
        LoanProfit LoanProfitMgr = new LoanProfit();
        this.LoanProfitInfo = LoanProfitMgr.GetModel(this.iFileId);
        if (this.LoanProfitInfo == null)
        {
            this.LoanProfitInfo = new LPWeb.Model.LoanProfit();
            insert = true;
        }
        LoanProfitInfo.FileId = this.iFileId;
        LoanProfitInfo.CompensationPlan = sCompensationPlan;
        decimal.TryParse(sLenderCredit, out tempDecimal);
        LoanProfitInfo.LenderCredit = tempDecimal;
        tempDecimal = 0;
        decimal.TryParse(sPrice, out tempDecimal);
        LoanProfitInfo.Price = tempDecimal;
        if (!insert) // update
        {
            LoanProfitMgr.Update(LoanProfitInfo);
        }
        else
        {
            LoanProfitMgr.Add(LoanProfitInfo);
        }

        #endregion

        #region Escrow Taxes/Insurance, ARM Info & Caps in Point Fields

        if (LoanPointFieldsMgr == null)
            LoanPointFieldsMgr = new LoanPointFields();
        string cValue = null;
        if (this.ddlEscrowTaxes.SelectedValue.Trim().ToUpper() == "Y")
            cValue = "Yes";
        UpdateLoanPointField(iFileId, 4003, cValue);

        cValue = null;
        if (this.ddlEscrowInsurance.SelectedValue.Trim().ToUpper() == "Y")
            cValue = "Yes";
        UpdateLoanPointField(this.iFileId, 4004, cValue);
        // ARM Caps & Margin info
        UpdateLoanPointField(this.iFileId, 2322, this.txtMargin.Text);
        UpdateLoanPointField(this.iFileId, 2324, this.txtAdjCap.Text);
        UpdateLoanPointField(this.iFileId, 2325, this.txtLifeCap.Text);
        UpdateLoanPointField(this.iFileId, 3896, this.txtIndex.Text);
        UpdateLoanPointField(this.iFileId, 2338, this.txtInitialAdjCap.Text);
        #endregion

        #region Update LoanLocks Table in Pulse Database

        LoanLocks LoanLocksMgr = new LoanLocks();
        LPWeb.Model.LoanLocks LoanLocksModel = new LPWeb.Model.LoanLocks();
        LoanLocksModel.FileId = this.iFileId;
        tempDate = DateTime.MinValue;
        LoanLocksModel.LockOption = sLockOption;

        LoanLocksModel.LockTime = null;

        if (sLockDate != string.Empty)
        {
            if (DateTime.TryParse(sLockDate, out tempDate))
            {
                LoanLocksModel.LockTime = tempDate;
            }
        }

        tempDate = DateTime.MinValue;
        tempInt = 0;
        LoanLocksModel.LockTerm = null;

        if (!string.IsNullOrEmpty(sLockTerm))
        {
            if (int.TryParse(sLockTerm, out tempInt))
            {
                LoanLocksModel.LockTerm = tempInt;
            }
        }

        if (!string.IsNullOrEmpty(txtExt1ExpireDate.Text))
        {
            sExpirationDate = txtExt1ExpireDate.Text;
        }

        if (!string.IsNullOrEmpty(txtExt2ExpireDate.Text))
        {
            sExpirationDate = txtExt2ExpireDate.Text;
        }

        if (!string.IsNullOrEmpty(txtExt3ExpireDate.Text))
        {
            sExpirationDate = txtExt3ExpireDate.Text;
        }

        tempDate = DateTime.MinValue;
        tempInt = 0;
        LoanLocksModel.LockExpirationDate = null;
        if (sExpirationDate != string.Empty)
        {
            if (DateTime.TryParse(sExpirationDate, out tempDate))
            {
                LoanLocksModel.LockExpirationDate = tempDate;
            }
        }

        sExt1LockDate = this.txtExt1LockDate.Text.Trim();
        tempDate = DateTime.MinValue;
        LoanLocksModel.Ext1LockTime = null;
        if (sExt1LockDate != string.Empty)
        {
            if (DateTime.TryParse(sExt1LockDate, out tempDate))
            {
                LoanLocksModel.Ext1LockTime = tempDate;
            }
        }

        sExt1Term = this.ddlExt1Term.SelectedValue;
        tempInt = 0;
        LoanLocksModel.Ext1Term = null;
        if (sExt1Term != string.Empty)
        {
            if (int.TryParse(sExt1Term, out tempInt))
            {
                LoanLocksModel.Ext1Term = tempInt;
            }
        }

        sExt1ExpireDate = this.txtExt1ExpireDate.Text.Trim();
        tempDate = DateTime.MinValue;
        tempInt = 0;
        LoanLocksModel.Ext1LockExpDate = null;
        if (sExt1ExpireDate != string.Empty)
        {
            if (DateTime.TryParse(sExt1ExpireDate, out tempDate))
            {
                LoanLocksModel.Ext1LockExpDate = tempDate;
            }
        }

        sExt2LockDate = this.txtExt2LockDate.Text.Trim();
        tempDate = DateTime.MinValue;
        LoanLocksModel.Ext2LockTime = null;
        if (sExt2LockDate != string.Empty)
        {
            if (DateTime.TryParse(sExt2LockDate, out tempDate))
            {
                LoanLocksModel.Ext2LockTime = tempDate;
            }
        }

        sExt2Term = this.ddlExt2Term.SelectedValue;
        tempInt = 0;
        LoanLocksModel.Ext2Term = null;
        if (sExt2Term != string.Empty)
        {
            if (int.TryParse(sExt2Term, out tempInt))
            {
                LoanLocksModel.Ext2Term = tempInt;
            }
        }

        sExt2ExpireDate = this.txtExt2ExpireDate.Text.Trim();
        tempDate = DateTime.MinValue;
        tempInt = 0;
        LoanLocksModel.Ext2LockExpDate = null;
        if (sExt2ExpireDate != string.Empty)
        {
            if (DateTime.TryParse(sExt2ExpireDate, out tempDate))
            {
                LoanLocksModel.Ext2LockExpDate = tempDate;
            }
        }

        sExt3LockDate = this.txtExt3LockDate.Text.Trim();
        tempDate = DateTime.MinValue;
        tempInt = 0;
        LoanLocksModel.Ext3LockTime = null;
        if (sExt3LockDate != string.Empty)
        {
            if (DateTime.TryParse(sExt3LockDate, out tempDate))
            {
                LoanLocksModel.Ext3LockTime = tempDate;
            }
        }

        sExt3Term = this.ddlExt3Term.SelectedValue;
        tempDate = DateTime.MinValue;
        tempInt = 0;
        LoanLocksModel.Ext3Term = null;
        if (sExt3Term != string.Empty)
        {
            if (int.TryParse(sExt3Term, out tempInt))
            {
                LoanLocksModel.Ext3Term = tempInt;
            }
        }

        sExt3ExpireDate = this.txtExt3ExpireDate.Text.Trim();
        tempDate = DateTime.MinValue;
        LoanLocksModel.Ext3LockExpDate = null;
        if (sExt3ExpireDate != string.Empty)
        {
            if (DateTime.TryParse(sExt3ExpireDate, out tempDate))
            {
                LoanLocksModel.Ext3LockExpDate = tempDate;
            }
        }

        LoanLocksModel.Program = sLoanProgram;
        sLoanProgramID = ddlLoanProgram.SelectedValue;
        if (sLoanProgram != "")
        {
            if (sLoanProgramID == "")
            {
                sLoanProgramID = GetLoanProgramIDWithLoanProgram(sLoanProgram);
            }
        }

        LoanLocksModel.ProgramID = string.IsNullOrEmpty(sLoanProgramID) ? 0 : Convert.ToInt32(sLoanProgramID);

        LoanLocksModel.InvestorID = string.IsNullOrEmpty(sInvestorID) ? 0 : Convert.ToInt32(sInvestorID);
        LoanLocksModel.Investor = sInvestorName;
        if (LoanLocksModel.Investor.ToUpper().Contains("SELECT"))
        {
            LoanLocksModel.Investor = string.Empty;
        }

        if (this.LoanLocksInfo.Rows.Count > 0)
        {
            LoanLocksMgr.Update(LoanLocksModel);
        }
        else
        {
            LoanLocksMgr.Add(LoanLocksModel);
        }

        #endregion

        #region update point fields for loan Info
        //UpdateLoanPointField(this.iFileId, 6001, sLenderName);
        UpdateLoanPointField(this.iFileId, 7403, sLoanProgram);
        UpdateLoanPointField(this.iFileId, 12973, sLPMIFactor);
        UpdateLoanPointField(this.iFileId, 2729, sddlPropertyType);

        //Purpose
        int iPointFieldId = 0;
        int.TryParse(ddlPurpose.SelectedValue, out iPointFieldId);
        LoanPointFieldsMgr.DeletePointFields(iFileId, "PointFieldId in (1190, 1191, 1192, 1193, 1194, 1198) ");
        if (iPointFieldId > 0)
        {
            UpdateLoanPointField(this.iFileId, iPointFieldId, "Yes");
        }
        //Occupancy
        iPointFieldId = 0;
        int.TryParse(ddlOccupancy.SelectedValue, out iPointFieldId);
        LoanPointFieldsMgr.DeletePointFields(iFileId, "PointFieldId in (921, 923, 924) ");
        if (iPointFieldId > 0)
        {
            UpdateLoanPointField(this.iFileId, iPointFieldId, "Yes");
        }
        //Fixed BUG254
        if (this.txtRate.Enabled == true)
        {
            UpdateLoanPointField(this.iFileId, 12, dRate.ToString());
        }
        if (this.txtTerm.Enabled == true)
        {
            UpdateLoanPointField(this.iFileId, 13, iTerm.ToString());
        }
        if (this.txtDue.Enabled == true)
        {
            UpdateLoanPointField(this.iFileId, 3190, iDue.ToString());
        }

        #region update Point Fields for Lock Info
        // Lock Option
        int.TryParse(ddlLockOption.SelectedValue, out iPointFieldId);
        LoanPointFieldsMgr.DeletePointFields(iFileId, " PointFieldId in (6100, 6101)");
        if (iPointFieldId > 0)
            UpdateLoanPointField(this.iFileId, iPointFieldId, "Yes");
        // Lock date, term, expiration date
        //UpdateLoanPointField(this.iFileId, 6061, sLockDate);
        //UpdateLoanPointField(this.iFileId, 6062, sLockTerm);
        if (!string.IsNullOrEmpty(sExt1ExpireDate))
        {
            sLockDate = sExt1LockDate;
            sLockTerm = sExt1Term;
            sExpirationDate = sExt1ExpireDate;
            this.txtLockDate.Text = this.txtExt1LockDate.Text;
            this.ddlLockTerm.SelectedValue = this.ddlExt1Term.SelectedValue;
            this.txtExpirationDate.Text = this.txtExt1ExpireDate.Text;
        }
        if (!string.IsNullOrEmpty(sExt2ExpireDate))
        {
            sLockDate = sExt2LockDate;
            sLockTerm = sExt2Term;
            sExpirationDate = sExt2ExpireDate;
            this.txtLockDate.Text = this.txtExt2LockDate.Text;
            this.ddlLockTerm.SelectedValue = this.ddlExt2Term.SelectedValue;
            this.txtExpirationDate.Text = this.txtExt2ExpireDate.Text;
        }
        if (!string.IsNullOrEmpty(sExt3ExpireDate))
        {
            sLockDate = sExt3LockDate;
            sLockTerm = sExt3Term;
            sExpirationDate = sExt3ExpireDate;
            this.txtLockDate.Text = this.txtExt3LockDate.Text;
            this.ddlLockTerm.SelectedValue = this.ddlExt3Term.SelectedValue;
            this.txtExpirationDate.Text = this.txtExt3ExpireDate.Text;
        }

        if (string.IsNullOrEmpty(sExpirationDate))
        {
            if (!string.IsNullOrEmpty(this.txtLockDate.Text) && !string.IsNullOrEmpty(this.ddlLockTerm.SelectedValue))
            {
                tempDate = DateTime.MinValue;
                tempInt = 0;
                int.TryParse(this.ddlLockTerm.SelectedValue, out tempInt);
                if (DateTime.TryParse(this.txtLockDate.Text, out tempDate))
                {
                    DateTime answer = tempDate.AddDays(tempInt);
                    this.txtExpirationDate.Text = answer.ToShortDateString();
                    sExpirationDate = this.txtExpirationDate.Text;
                }
            }
        }

        UpdateLoanPointField(this.iFileId, 6061, sLockDate);
        UpdateLoanPointField(this.iFileId, 6062, sLockTerm);

        UpdateLoanPointField(this.iFileId, 6063, sExpirationDate);
        if (!string.IsNullOrEmpty(sExpirationDate))
        {
            DateTime dt3 = DateTime.MinValue;
            if (DateTime.TryParse(sExpirationDate, out dt3))
            {
                loan.RateLockExpiration = dt3;
            }

        }

        LoansMgr.Update(loan);
        // Extension 1
        UpdateLoanPointField(this.iFileId, 7021, sExt1LockDate);
        //UpdateLoanPointField(this.iFileId, 11439, sExt1Term);
        //UpdateLoanPointField(this.iFileId, 6063, sExt1ExpireDate);
        // Extension 2
        UpdateLoanPointField(this.iFileId, 7025, sExt2LockDate);

        string ext_term = string.Empty;

        if (!string.IsNullOrEmpty(sExt1Term))
        {
            ext_term = sExt1Term;
        }

        if (!string.IsNullOrEmpty(sExt2Term))
        {
            ext_term = sExt2Term;
        }

        if (!string.IsNullOrEmpty(sExt3Term))
        {
            ext_term = sExt3Term;
        }

        UpdateLoanPointField(this.iFileId, 1143, ext_term);

        //UpdateLoanPointField(this.iFileId, 11439, sExt2Term);
        //UpdateLoanPointField(this.iFileId, 6063, sExt2ExpireDate);
        // Extension 3
        //UpdateLoanPointField(this.iFileId, 7025, sExt3LockDate);
        //UpdateLoanPointField(this.iFileId, 11439, sExt3Term);
        //UpdateLoanPointField(this.iFileId, 6063, sExt3ExpireDate);

        UpdateLoanPointField(this.iFileId, 2836, sBorrowerFICO);
        UpdateLoanPointField(this.iFileId, 4018, sMIOption);

        #region Firs Time Homebuyer

        if (!string.IsNullOrEmpty(sFirsTimeHomebuyer))
        {
            if (sFirsTimeHomebuyer.ToUpper().Contains("Y"))
                LoansMgr.UpdateFirstTimeHomeBuyer(this.iFileId, true);
        }
        else
        {
            LoansMgr.UpdateFirstTimeHomeBuyer(this.iFileId, false);
        }
        LoanPointFieldsMgr.DeletePointFields(this.iFileId, " PointFieldId=7505");
        string s7505_FirstTimeHomeBuyer = "X";
        this.AddLoanPointField(iFileId, 7505, s7505_FirstTimeHomeBuyer);
        #endregion


        // 08/30/2013 LW changed Lender Credit Point Field ID 2284-->812 instead
        UpdateLoanPointField(this.iFileId, 812, sLenderCredit);

        if (!string.IsNullOrEmpty(sPrice))
        {
            var tempsPrice = 0M;
            decimal.TryParse(sPrice, out tempsPrice);
        }
        #endregion
        #endregion

        #region Call WCF UpdateLockInfo & Call WCF ImportLockInfo

        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            UpdateLockInfoRequest req = new UpdateLockInfoRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = this.CurrUser.iUserID;
            req.FileId = this.iFileId;


            ImportLockInfoRequest req1 = new ImportLockInfoRequest();
            req1.hdr = new ReqHdr();
            req1.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req1.hdr.UserId = this.CurrUser.iUserID;
            req1.FileId = this.iFileId;

            UpdateLockInfoResponse respone = null;
            ImportLockInfoResponse respone1 = null;
            try
            {
                respone = service.UpdateLockInfo(req);

                if (respone.hdr.Successful == false)
                {
                    //PageCommon.WriteJsEnd(this, "Saved successfully but Point Manager failed to update Point file, reason:" + respone.hdr.StatusInfo, PageCommon.Js_RefreshSelf);                    
                    string current_URL = Request.Url.ToString();
                    PageCommon.WriteJsEnd(this, "Saved successfully but Point Manager failed to update Point file, reason:" + respone.hdr.StatusInfo, "window.parent.CloseGlobalPopup();");
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                string sExMsg = string.Format("Saved successfully but failed to update Point File, reason: (FileID={0}): {1}", this.iFileId, "Point Manager is not running.");
                LPLog.LogMessage(LogType.Logerror, sExMsg);

                //PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
                PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseGlobalPopup();");
            }
            catch (Exception ex)
            {
                string sExMsg = string.Format("Saved successfully but failed to update Point file, error: (FileID={0}): {1}", this.iFileId, ex.Message);
                LPLog.LogMessage(LogType.Logerror, sExMsg);

                //PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
                PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseGlobalPopup();");
            }


            //try
            //{
            //    respone1 = service.ImportLockInfo(req1);

            //    if (respone1.hdr.Successful == false)
            //    {
            //        PageCommon.WriteJsEnd(this, "Failed to invoke ImportLockInfo wcf service, reason:" + respone1.hdr.StatusInfo, "window.parent.CloseGlobalPopup();");
            //    }
            //}
            //catch (System.ServiceModel.EndpointNotFoundException)
            //{
            //    string sExMsg = string.Format("Exception occurred when Sync'ing Point File (FileID={0}): {1}", this.iFileId, "Point Manager is not running.");
            //    LPLog.LogMessage(LogType.Logerror, sExMsg);

            //    PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseGlobalPopup();");
            //}
            //catch (Exception ex)
            //{
            //    string sExMsg = string.Format("Exception occurred when Sync'ing Point File (FileID={0}): {1}", this.iFileId, ex.Message);
            //    LPLog.LogMessage(LogType.Logerror, sExMsg);

            //    //PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
            //    PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseGlobalPopup();");
            //}

            string CURRENT_URL = Request.Url.ToString();
            //var iFrameSrc = "LinkLoanDetails.aspx?sid=" + RadomStr + "&ProspectID=" + ProspectID + "&CloseDialogCodes=window.parent.CloseGlobalPopupPipeline()&RefreshCodes=window.parent.RefreshLoanDetailInfo()";
            // success
            //PageCommon.WriteJsEnd(this, "Save Point File and Sync'ed Point File successfully.", "window.parent.CloseGlobalPopup();RefreshLoanDetailInfo();");
            System.Threading.Thread.Sleep(1000);
            PageCommon.WriteJsEnd(this, "Save Point File and Sync'ed Point File successfully.", "window.parent.RefreshLoanDetailInfo();window.parent.CloseGlobalPopup();");

        }

        #endregion


        #region Call WCF ImportLockInfo

        //ServiceManager sm1 = new ServiceManager();
        //using (LP2ServiceClient service = sm1.StartServiceClient())
        //{
        //    ImportLockInfoRequest req = new ImportLockInfoRequest();
        //    req.hdr = new ReqHdr();
        //    req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
        //    req.hdr.UserId = this.CurrUser.iUserID;
        //    req.FileId = this.iFileId;

        //    ImportLockInfoResponse respone = null;
        //    try
        //    {
        //        respone = service.ImportLockInfo(req);

        //        if (respone.hdr.Successful == false)
        //        {
        //            PageCommon.WriteJsEnd(this, "Failed to invoke ImportLockInfo wcf service, reason:" + respone.hdr.StatusInfo, "window.parent.CloseGlobalPopup();");
        //        }
        //    }
        //    catch (System.ServiceModel.EndpointNotFoundException)
        //    {
        //        string sExMsg = string.Format("Exception occurred when Sync'ing Point File (FileID={0}): {1}", this.iFileId, "Point Manager is not running.");
        //        LPLog.LogMessage(LogType.Logerror, sExMsg);

        //        PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseGlobalPopup();");
        //    }
        //    catch (Exception ex)
        //    {
        //        string sExMsg = string.Format("Exception occurred when Sync'ing Point File (FileID={0}): {1}", this.iFileId, ex.Message);
        //        LPLog.LogMessage(LogType.Logerror, sExMsg);

        //        PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseGlobalPopup();");
        //    }

        //    // success
        //    PageCommon.WriteJsEnd(this, "Sync'ed Point File successfully.", "window.parent.CloseGlobalPopup();");
        //}

        #endregion

        // success
        PageCommon.WriteJsEnd(this, "Saved successfully.", "window.parent.CloseGlobalPopup();");
    }

    protected void btnSyncNow_Click(object sender, EventArgs e)
    {
        #region Call WCF ImportLockInfo

        //ServiceManager sm = new ServiceManager();
        //using (LP2ServiceClient service = sm.StartServiceClient())
        //{
        //    ImportLockInfoRequest req = new ImportLockInfoRequest();
        //    req.hdr = new ReqHdr();
        //    req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
        //    req.hdr.UserId = this.CurrUser.iUserID;
        //    req.FileId = this.iFileId;

        //    ImportLockInfoResponse respone = null;
        //    try
        //    {
        //        respone = service.ImportLockInfo(req);

        //        if (respone.hdr.Successful == false)
        //        {
        //            PageCommon.WriteJsEnd(this, "Failed to invoke ImportLockInfo wcf service, reason:" + respone.hdr.StatusInfo, "window.parent.CloseGlobalPopup();");
        //        }
        //    }
        //    catch (System.ServiceModel.EndpointNotFoundException)
        //    {
        //        string sExMsg = string.Format("Exception occurred when Sync'ing Point File (FileID={0}): {1}", this.iFileId, "Point Manager is not running.");
        //        LPLog.LogMessage(LogType.Logerror, sExMsg);

        //        PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseGlobalPopup();");
        //    }
        //    catch (Exception ex)
        //    {
        //        string sExMsg = string.Format("Exception occurred when Sync'ing Point File (FileID={0}): {1}", this.iFileId, ex.Message);
        //        LPLog.LogMessage(LogType.Logerror, sExMsg);

        //        PageCommon.WriteJsEnd(this, sExMsg, "window.parent.CloseGlobalPopup();");
        //    }

        //    // success
        //    PageCommon.WriteJsEnd(this, "Sync'ed Point File successfully.", "window.parent.CloseGlobalPopup();");
        //}

        #endregion
    }
    private void UpdateLoanPointField(int iFileId, int iPointFieldId, string sCurrentValue)
    {
        LPWeb.Model.LoanPointFields PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, iPointFieldId);
        if (PointFieldInfo != null)
        {
            LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, iPointFieldId, sCurrentValue);
        }
        else
        {
            this.AddLoanPointField(this.iFileId, iPointFieldId, sCurrentValue);
        }
    }

    private void AddLoanPointField(int iFileId, int iPointFieldId, string sCurrentValue)
    {
        LPWeb.Model.LoanPointFields LoanPointFieldsModel = new LPWeb.Model.LoanPointFields();
        LoanPointFieldsModel.FileId = iFileId;
        LoanPointFieldsModel.PointFieldId = iPointFieldId;
        LoanPointFieldsModel.CurrentValue = sCurrentValue;

        LoanPointFields LoanPointFieldsMgr = new LoanPointFields();
        LoanPointFieldsMgr.Add(LoanPointFieldsModel);
    }
}


