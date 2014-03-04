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

public partial class LoanDetails_LoanDetailsProfitability : BasePage
{
    int iFileId = 0;
    bool SyncNow_Once = false;

    DataTable LoanInfo = null;
    DataTable LoanProfitInfo = null;
    DataTable LoanLocksInfo = null;
    LoanPointFields LoanPointFieldsMgr = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 检查权限

        SyncNow_Once = false;

        if (this.CurrUser.userRole.AccessProfitability == false)
        {
            this.Response.Redirect("../Unauthorize1.aspx");
            return;
        }

        // Rate Lock权限
        if (this.CurrUser.userRole.LockRate == false && this.CurrUser.userRole.ViewLockInfo == false)
        {
            this.hdnShowLockRatePopup.Value = "false";
        }
        else
        {
            this.hdnShowLockRatePopup.Value = "true";
        }

        // View Compensation权限
        if (this.CurrUser.userRole.ViewCompensation == true)
        {
            this.hdnShowCompensationDetailPopup.Value = "true";
        }
        else
        {
            this.hdnShowCompensationDetailPopup.Value = "false";
        }

        // Modify Loan Info权限
        if (this.CurrUser.userRole.Loans.Contains("B") == true)
        {
            this.hdnModifyLoan.Value = "true";
        }
        else
        {
            this.hdnModifyLoan.Value = "false";
        }

        #endregion

        #region 校验页面参数

        #region 校验FileId

        bool bValid = PageCommon.ValidateQueryString(this, "FileId", QueryStringType.ID);
        if (bValid == false)
        {
            PageCommon.WriteJsEnd(this, "Invalid file id.", "");
            return;
        }

        string sFileId = this.Request.QueryString["FileId"];
        this.iFileId = Convert.ToInt32(sFileId);

        #endregion

        #endregion

        #region 加载Loan Info

        Loans LoansMgr = new Loans();
        this.LoanInfo = LoansMgr.GetLoanInfo(this.iFileId);
        if (LoanInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid file id.", "");
            return;
        }

        #endregion

        #region 加载LoanProfit Info

        LoanProfit LoanProfitMgr = new LoanProfit();
        this.LoanProfitInfo = LoanProfitMgr.GetLoanProfitInfo(this.iFileId);

        #endregion

        #region 加载LoanLocks Info

        LoanLocks LoanLocksMgr = new LoanLocks();
        this.LoanLocksInfo = LoanLocksMgr.GetLoanLocksInfo(this.iFileId);

        #endregion

        if (this.IsPostBack == false)
        {
            #region Loan Amount
            decimal dPoints = decimal.Zero;
            decimal dLoanAmount = 0;

            LoanPointFieldsMgr = new LoanPointFields();
            LPWeb.Model.LoanPointFields PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 21017);
            if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
            {
                decimal.TryParse(PointFieldInfo.CurrentValue, out dLoanAmount);
            }
            if (dLoanAmount <= 0 && this.LoanInfo != null && this.LoanInfo.Rows.Count > 0)
            {
                dLoanAmount = this.LoanInfo.Rows[0]["LoanAmount"] == DBNull.Value ? 0 : (decimal)this.LoanInfo.Rows[0]["LoanAmount"];
            }
            this.txtLoanAmount.Text = dLoanAmount.ToString("n2");          
            this.txtLoanAmount.Enabled = false;

            #endregion

            #region Mandatory/Final Price

            decimal dMandatoryPrice = decimal.Zero;
            decimal dMandatoryPricePoint = decimal.Zero;
            PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 11604);
            if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
            {
                if (decimal.TryParse(PointFieldInfo.CurrentValue, out dMandatoryPricePoint))
                {
                    this.txtMandatory_Points.Text = dMandatoryPricePoint.ToString("n3");
                    dMandatoryPrice = dMandatoryPricePoint * dLoanAmount/100;
                    this.txtMandatory_Amount.Text = dMandatoryPrice.ToString("n2");
                }
            }

            #endregion

            #region Best Effort Price

            decimal dBestEffortPrice = decimal.Zero;
            decimal dBestEffortPricePoint = decimal.Zero;
            PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 12492);
            if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
            {
                if (decimal.TryParse(PointFieldInfo.CurrentValue, out dBestEffortPricePoint))
                {
                    this.txtBestEffortPrice_Points.Text = (dBestEffortPricePoint).ToString("n3");
                    dBestEffortPrice = dBestEffortPricePoint * dLoanAmount / 100;
                    this.txtBestEffortPrice_Amount.Text = dBestEffortPrice.ToString("n2");
                }
            }           

            #endregion

            #region Commission Total

             // CR67
            #region 获取Commission Total %

            LoanTeam LoanTeamMgr = new LoanTeam();
            DataTable LoanOfficerInfo = LoanTeamMgr.GetLoanOfficerInfo(this.iFileId);

            decimal dCommissionTotalPoint = decimal.Zero;
            if (LoanOfficerInfo.Rows.Count > 0)
            {
                DataRow LoanOfficerRow = LoanOfficerInfo.Rows[0];

                decimal dLOComp = LoanOfficerRow["LOComp"] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(LoanOfficerRow["LOComp"]);
                decimal dBranchMgrComp = LoanOfficerRow["BranchMgrComp"] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(LoanOfficerRow["BranchMgrComp"]);
                decimal dDivisionMgrComp = LoanOfficerRow["DivisionMgrComp"] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(LoanOfficerRow["DivisionMgrComp"]);
                decimal dRegionMgrComp = LoanOfficerRow["RegionMgrComp"] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(LoanOfficerRow["RegionMgrComp"]);

                dCommissionTotalPoint = dLOComp + dBranchMgrComp + dDivisionMgrComp + dRegionMgrComp;
                this.txtCommissionTotal_Points.Text = dCommissionTotalPoint.ToString("n3");
            }

            #endregion

            // 计算Commission Total $
            decimal dCommissionTotalAmount = dLoanAmount * dCommissionTotalPoint / 100;
            this.txtCommissionTotal_Amount.Text = dCommissionTotalAmount.ToString("n2");

            #endregion

            #region Lender Credit

            // CR67
            // 获取Lender Credit %
            decimal dLenderCreditAmount = decimal.Zero;
            decimal dLenderCreditPoint = decimal.Zero;
            // LW 08/30/2013 Changed Lender Credit 2284-->812
            PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 812);
            if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
            {
                if (decimal.TryParse(PointFieldInfo.CurrentValue, out dLenderCreditAmount) == true)
                {
                    // Lender Credit %
                    this.txtLenderCredit_Amount.Text = dLenderCreditAmount.ToString("n2");

                    // 计算Lender Credit $
                    if (dLoanAmount > 0)
                    {
                        dLenderCreditPoint = dLenderCreditAmount * 100 / dLoanAmount;
                    }
                    else
                    {
                        dLenderCreditPoint = 0;
                    }
                    this.txtLenderCredit_Points.Text = dLenderCreditPoint.ToString("n3");
                                      
                }
            }

            #endregion

            #region LPMI

            decimal dLPMI = decimal.Zero;
            decimal dLPMIPoint = decimal.Zero;
            PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 12973);
            if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
            {
                if (decimal.TryParse(PointFieldInfo.CurrentValue, out dLPMIPoint))
                {
                    this.txtLPMI_Points.Text = dLPMIPoint.ToString("n3");
                    dLPMI = dLPMIPoint * dLoanAmount / 100;
                    this.txtLPMI_Amount.Text = dLPMI.ToString("n2");
                }
            }

            #endregion

            #region Best Effort Price to LO

            #region 注释代码
            //decimal dBestEffortPriceToLO = decimal.Zero;
            //PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 12495);
            //if (PointFieldInfo.Rows.Count > 0)
            //{
            //    string sAmount = PointFieldInfo.Rows[0]["CurrentValue"] == DBNull.Value ? string.Empty : PointFieldInfo.Rows[0]["CurrentValue"].ToString();
            //    if (sAmount != string.Empty)
            //    {
            //        decimal.TryParse(sAmount, out dBestEffortPriceToLO);
            //    }
            //    else
            //    {
            //        dBestEffortPriceToLO = dCommissionTotalAmount + dLenderCreditAmount + dLPMI;
            //    }

            //    this.txtBestEffortPriceToLO_Amount.Text = dBestEffortPriceToLO.ToString("n2");

            //    // Calc. Points
            //    dPoints = this.CalcPoints(dBestEffortPriceToLO, dLoanAmount);
            //    this.txtBestEffortPriceToLO_Points.Text = (dPoints * 100).ToString("n3");
            //}
            #endregion

            // // 计算Best Effort Price to LO %
            decimal dBestEffortPriceToLOPoint = dCommissionTotalPoint + dLenderCreditPoint + dLPMIPoint;
            this.txtBestEffortPriceToLO_Points.Text = dBestEffortPriceToLOPoint.ToString("n3");
            //this.Response.Write("dCommissionTotalPoint: " + dCommissionTotalPoint + "; dLenderCreditPoint: " + dLenderCreditPoint + "; dLPMIPoint" + dLPMIPoint);

            // 计算Best Effort Price to LO $
            decimal dBestEffortPriceToLOAmount = dLoanAmount * dBestEffortPriceToLOPoint / 100;
            this.txtBestEffortPriceToLO_Amount.Text = dBestEffortPriceToLOAmount.ToString("n2");

            #endregion

            #region Hedge Cost
            decimal dHedgeCost = decimal.Zero;
            decimal dHedgeCostPoint = decimal.Zero;
            decimal dHedgeCost_Reverse = decimal.Zero;
            decimal dHedgeCostPoint_Reverse = decimal.Zero;
            PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 12974);
            if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
            {
                if (decimal.TryParse(PointFieldInfo.CurrentValue, out dHedgeCostPoint))
                {
                    //dHedgeCostPoint_Reverse = 0 - dHedgeCostPoint;
                    this.txtHedgeCost_Points.Text = dHedgeCostPoint.ToString("n3");
                    dHedgeCost = dHedgeCostPoint * dLoanAmount/100;
                    //dHedgeCost_Reverse = 0 - dHedgeCost;
                    this.txtHedgeCost_Amount.Text = dHedgeCost.ToString("n2");
                }
            }

            #endregion

            #region Cost on sale
            decimal dCostOnSale = decimal.Zero;
            decimal dCostOnSalePoint = decimal.Zero;
            PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 12975);
            if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
            {
                if (decimal.TryParse(PointFieldInfo.CurrentValue, out dCostOnSalePoint))
                {
                    this.txtCostOnSale_Points.Text = dCostOnSalePoint.ToString("n3");
                    dCostOnSale = dCostOnSalePoint * dLoanAmount/100;
                    this.txtCostOnSale_Amount.Text = dCostOnSale.ToString("n2");
                }
            }

            #endregion

            #region Origination Pts
            decimal dOriginationPts = decimal.Zero;
            decimal dOriginationPts_Amount = decimal.Zero;
            PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 11243);
            if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
            {
                if (decimal.TryParse(PointFieldInfo.CurrentValue, out dOriginationPts))
                {
                    this.txtOriginationPts_Points.Text = dOriginationPts.ToString("n3");
                    dOriginationPts_Amount = dOriginationPts * dLoanAmount / 100;
                    this.txtOriginationPts_Amount.Text = dOriginationPts_Amount.ToString("n2");
                }
            }

            #endregion

            #region Discount Pts
            decimal dDiscountPts = decimal.Zero;
            decimal dDiscountPts_Amount = decimal.Zero;
            PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 11241);
            if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
            {
                if (decimal.TryParse(PointFieldInfo.CurrentValue, out dDiscountPts))
                {
                    this.txtDiscountPts_Points.Text = dDiscountPts.ToString("n3");
                    dDiscountPts_Amount = dDiscountPts * dLoanAmount / 100;
                    this.txtDiscountPts_Amount.Text = dDiscountPts_Amount.ToString("n2");
                }
            }

            #endregion

            //dBestEffortPrice = dBestEffortPriceToLOAmount + dHedgeCost + dCostOnSale + dOriginationPts_Amount + dDiscountPts_Amount;
            //this.txtBestEffortPrice_Amount.Text = dBestEffortPrice.ToString("n2");
            //dBestEffortPricePoint = dBestEffortPrice * 100 / dLoanAmount;
            //this.txtBestEffortPrice_Points.Text = (dBestEffortPricePoint).ToString("n3");

            #region Best Effort Margin
            // LCW 10-05-2013 Per MSA Request remove HedgeCost in the formula
            //decimal dBestEffortMargin = dBestEffortPrice - dBestEffortPriceToLOAmount - dHedgeCost + dCostOnSale + (dOriginationPts * dLoanAmount) + (dDiscountPts * dLoanAmount);
            decimal dBestEffortMargin = dBestEffortPrice - dBestEffortPriceToLOAmount + dCostOnSale + (dOriginationPts * dLoanAmount) + (dDiscountPts * dLoanAmount);
            this.txtBestEffortMargin_Amount.Text = dBestEffortMargin.ToString("n2");

            // Calc. Points
            decimal dBestEffortMargin_Points = this.CalcPoints(dBestEffortMargin, dLoanAmount);
            this.txtBestEffortMargin_Points.Text = (dBestEffortMargin_Points * 100).ToString("n3");

            #endregion

            #region Extension1
            decimal dExtension1 = decimal.Zero;
            decimal dExtension1Point = decimal.Zero;
            PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 12976);
            if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
            {
                if (decimal.TryParse(PointFieldInfo.CurrentValue, out dExtension1Point))
                {
                    dExtension1 = dExtension1Point * dLoanAmount /100;
                    this.txtExtension1_Amount.Text = dExtension1.ToString("n2");
                    this.txtExtension1_Points.Text = dExtension1Point.ToString("n3");
                }
            }

            #endregion

            #region Extension2
            decimal dExtension2 = decimal.Zero;
            decimal dExtension2Point = decimal.Zero;
            PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 12977);
            if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
            {
                if (decimal.TryParse(PointFieldInfo.CurrentValue, out dExtension2Point))
                {
                    dExtension2 = dExtension2Point * dLoanAmount / 100;
                    this.txtExtension2_Amount.Text = dExtension2.ToString("n2");
                    this.txtExtension2_Points.Text = dExtension2Point.ToString("n3");
                }
            }

            #endregion

            #region Final Mandatory Margin

            decimal dFinalMandatoryMargin = dMandatoryPrice - dBestEffortPriceToLOAmount + dHedgeCost + dCostOnSale + (dOriginationPts * dLoanAmount) + (dDiscountPts * dLoanAmount);
            this.txtFinalMandatoryMargin_Amount.Text = dFinalMandatoryMargin.ToString("n2");

            // Calc. Points
            decimal dFinalMandatoryMargin_Points = this.CalcPoints(dFinalMandatoryMargin, dLoanAmount);
            this.txtFinalMandatoryMargin_Points.Text = (dFinalMandatoryMargin_Points * 100).ToString("n3");

            #endregion

            #region Final Best Effort Margin

            decimal dFinalBestEffortMargin = dBestEffortMargin + dExtension1 + dExtension2;
            this.txtFinalBestEffortMargin_Amount.Text = dFinalBestEffortMargin.ToString("n2");

            // Calc. Points
            decimal dFinalBestEffortMargin_Points = this.CalcPoints(dFinalBestEffortMargin, dLoanAmount);
            this.txtFinalBestEffortMargin_Points.Text = (dFinalBestEffortMargin_Points * 100).ToString("n3");

            #endregion

            #region Final Mandatory Pickup
         
            decimal dMandatoryPickup_Amount = dFinalMandatoryMargin -  dFinalBestEffortMargin;
            this.txtMandatoryPickup_Amount.Text = dMandatoryPickup_Amount.ToString("n2");

            // Calc. Points
            decimal dMandatoryPickup_Points = this.CalcPoints(dMandatoryPickup_Amount, dLoanAmount);
            this.txtMandatoryPickup_Points.Text = (dMandatoryPickup_Points * 100).ToString("n3");

            #endregion

            #region Market to Market Daily Price
            // Loan Amount
            this.txtMM_LoanAmount.Text = dLoanAmount.ToString("n2");
            if (this.LoanProfitInfo.Rows.Count > 0)
            {
                // Net Sell
                decimal dNetSell = decimal.Zero;
                string sAmount = LoanProfitInfo.Rows[0]["NetSell"] == DBNull.Value ? string.Empty : LoanProfitInfo.Rows[0]["NetSell"].ToString();
                if (sAmount != string.Empty)
                {
                    if (decimal.TryParse(sAmount, out dNetSell))
                    {
                        this.txtMM_NetSell.Text = dNetSell.ToString("n2");
                    }
                }

                string sCommitmentNumber = LoanProfitInfo.Rows[0]["CommitmentNumber"] == DBNull.Value ? string.Empty : LoanProfitInfo.Rows[0]["CommitmentNumber"].ToString();
                this.txtCommitmentNumber.Text = sCommitmentNumber;

                this.txtCommitmentDate.Text = string.Empty;
                string sCommitmentDate = LoanProfitInfo.Rows[0]["CommitmentDate"] == DBNull.Value ? string.Empty : LoanProfitInfo.Rows[0]["CommitmentDate"].ToString();
                DateTime dt_CommitmentDate = DateTime.MinValue;
                if (sCommitmentDate != string.Empty)
                {
                    if (DateTime.TryParse(sCommitmentDate, out dt_CommitmentDate))
                    {
                        this.txtCommitmentDate.Text = dt_CommitmentDate.ToString("MM/dd/yyyy");
                    }
                }

                int iCommitmentTerm = 0;
                string sCommitmentTerm = LoanProfitInfo.Rows[0]["CommitmentTerm"] == DBNull.Value ? string.Empty : LoanProfitInfo.Rows[0]["CommitmentTerm"].ToString();
                if (sCommitmentTerm != string.Empty)
                {
                    if (int.TryParse(sCommitmentTerm, out iCommitmentTerm))
                    {
                        this.txtCommitmentTerm.Text = iCommitmentTerm.ToString();
                    }
                }

                this.txtCommitmentExpirationDate.Text = string.Empty;
                string sCommitmentExpirationDate = LoanProfitInfo.Rows[0]["CommitmentExpDate"] == DBNull.Value ? string.Empty : LoanProfitInfo.Rows[0]["CommitmentExpDate"].ToString();
                DateTime dt_CommitmentExpirationDate = DateTime.MinValue;
                if (sCommitmentExpirationDate != string.Empty)
                {
                    if (DateTime.TryParse(sCommitmentExpirationDate, out dt_CommitmentExpirationDate))
                    {
                        this.txtCommitmentExpirationDate.Text = dt_CommitmentExpirationDate.ToString("MM/dd/yyyy");
                    }
                }

                // SRP
                decimal dSRP = decimal.Zero;
                sAmount = this.LoanProfitInfo.Rows[0]["SRP"] == DBNull.Value ? string.Empty : this.LoanProfitInfo.Rows[0]["SRP"].ToString();
                if (sAmount != string.Empty)
                {
                    if (decimal.TryParse(sAmount, out dSRP))
                    {
                        this.txtMM_SRP.Text = dSRP.ToString("n2");
                    }
                }

                // LLPA
                decimal dLLPA = decimal.Zero;
                sAmount = this.LoanProfitInfo.Rows[0]["LLPA"] == DBNull.Value ? string.Empty : this.LoanProfitInfo.Rows[0]["SRP"].ToString();
                if (sAmount != string.Empty)
                {
                    if (decimal.TryParse(sAmount, out dLLPA))
                    {
                        this.txtMM_LLPA.Text = dLLPA.ToString("n2");
                    }
                }

                // Sum Price
                //if (this.LoanProfitInfo.Rows[0]["NetSell"] != DBNull.Value || this.LoanProfitInfo.Rows[0]["SRP"] != DBNull.Value)
                //{
                decimal dSumPrice = dNetSell + dSRP + dLLPA;
                this.txtMM_SumPrice.Text = dSumPrice.ToString("n2");
                //}

                // Investor LCW 10/08/2013 use the LoanProfitInfo.Investor field instead
                if (this.LoanProfitInfo.Rows[0]["Investor"] != DBNull.Value)
                {
                    this.txtMM_Investor.Text = this.LoanProfitInfo.Rows[0]["Investor"].ToString();
                }
                
                // Investor
                //if (this.LoanLocksInfo.Rows[0]["InvestorID"] == DBNull.Value)
                //{
                //    this.txtMM_Investor.Text = this.LoanLocksInfo.Rows[0]["Investor"].ToString();
                //}
                //else
                //{
                //    int iInvestorID = Convert.ToInt32(this.LoanLocksInfo.Rows[0]["InvestorID"]);
                //    ContactCompanies ContactCompaniesMgr = new ContactCompanies();
                //    DataTable InvestorInfo = ContactCompaniesMgr.GetContactCompanyInfo(iInvestorID);
                //    this.txtMM_Investor.Text = InvestorInfo.Rows[0]["Name"].ToString();
                //}

            }

            #endregion

            #region Compensation Plan

            if (this.LoanProfitInfo.Rows.Count > 0)
            {
                this.ddlCompensationPlan.SelectedValue = this.LoanProfitInfo.Rows[0]["CompensationPlan"].ToString();
            }

            #endregion

            #region Price

            decimal LOCompDecimal = 0;
            DataTable LoanOfficerInfo1 = LoanTeamMgr.GetLoanOfficerInfo(this.iFileId);
                        
            if (LoanOfficerInfo1.Rows.Count > 0)
            {
                DataRow LoanOfficerRow1 = LoanOfficerInfo1.Rows[0];

                LOCompDecimal = LoanOfficerRow1["LOComp"] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(LoanOfficerRow1["LOComp"]);
           }
            
            decimal AmountDecimal = 0;
            decimal TempDecimal = 0;
            
            //if (LOCompDecimal > 0)
            //{
            //    if (decimal.TryParse(this.txtLoanAmount.Text, out AmountDecimal))
            //    {
            //        TempDecimal = AmountDecimal * LOCompDecimal /100;
            //        this.txtPrice.Text = TempDecimal.ToString("n0");
            //    }
            //    else
            //    {
            //        TempDecimal = 0;
            //        this.txtPrice.Text = TempDecimal.ToString("n3");
            //    }          
            //}
            //else
            //{
            //    TempDecimal = 0;
            //    this.txtPrice.Text = TempDecimal.ToString("n3");
            //}          

            if (this.LoanProfitInfo.Rows.Count > 0)
            {
                this.txtPrice.Text = this.LoanProfitInfo.Rows[0]["Price"] == DBNull.Value ? string.Empty : Convert.ToDecimal(this.LoanProfitInfo.Rows[0]["Price"]).ToString("n3");
            }

            #endregion

            #region Lock Option

            if (this.LoanLocksInfo.Rows.Count > 0)
            {
                this.ddlLockOption.SelectedItem.Text = this.LoanLocksInfo.Rows[0]["LockOption"].ToString();
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

            #region First time homebuyer

            if (this.LoanInfo.Rows[0]["FirstTimeBuyer"] != DBNull.Value)
            {
                if (Convert.ToBoolean(this.LoanInfo.Rows[0]["FirstTimeBuyer"]) == true)
                {
                    this.ddlFirsTimeHomebuyer.SelectedValue = "Yes";
                }
                else
                {
                    this.ddlFirsTimeHomebuyer.SelectedValue = "No";
                }
            }

            #endregion

            #region Escrow Taxes

            PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 4003);
            this.ddlEscrowTaxes.SelectedValue = "N";
            if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
            {
                string cValue = PointFieldInfo.CurrentValue.ToUpper();
                if ((cValue.Trim().ToUpper() == "X") || (cValue.Trim().ToUpper() == "Y") || (cValue.Trim().ToUpper() == "YES"))
                {
                    this.ddlEscrowTaxes.SelectedValue = "Y";
                }
            }

            #endregion

            #region Escrow Insurance
            this.ddlEscrowInsurance.SelectedValue = "N";
            PointFieldInfo = LoanPointFieldsMgr.GetModel(this.iFileId, 4004);
            if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
            {
                string cValue = PointFieldInfo.CurrentValue.ToUpper();
                if ((cValue.Trim().ToUpper() == "X") || (cValue.Trim().ToUpper() == "Y") || (cValue.Trim().ToUpper() == "YES"))
                {
                    this.ddlEscrowInsurance.SelectedValue = "Y";
                }
            }
            #endregion

        }
    }
    private decimal CalcPoints(decimal dAmount, decimal dLoanAmount)
    {
        if (dAmount == 0 || dLoanAmount == 0)
        {
            return decimal.Zero;
        }

        decimal dPoints = dAmount / dLoanAmount;

        return dPoints;
    }

    protected bool SyncNow(int iFileId)
    {
        SyncNow_Once = true;
        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            ImportLoansRequest req = new ImportLoansRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken";
            req.hdr.UserId = this.CurrUser.iUserID;
            req.FileIds = new int[1] { iFileId };
            ImportLoansResponse respone = null;
            try
            {
                respone = service.ImportLoans(req);

                if (respone.hdr.Successful)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region 获取用户输入

        string sMandatoryPricePoint = this.txtMandatory_Points.Text.Trim();
        string sBestEffortPricePoint = this.txtBestEffortPrice_Points.Text.Trim();

        //string sCommissionTotal = this.txtCommissionTotal_Amount.Text.Trim();
        //string sCommissionTotal_Points = this.txtCommissionTotal_Points.Text.Trim();
        string sCommissionTotal = Request.Form["txtCommissionTotal_Amount"].Trim();
        string sCommissionTotal_Points = Request.Form["txtCommissionTotal_Points"].Trim();

        string sLenderCredit = Request.Form["txtLenderCredit_Amount"].Trim();
        string sLenderCreditAmount = this.txtLenderCredit_Amount.Text.Trim();
        string sLenderCreditPoint = this.txtLenderCredit_Points.Text.Trim();

        string sLPMIPoint = Request.Form["txtLPMI_Points"].Trim();

        string sBestEffortPriceToLOPoint = Request.Form["txtBestEffortPriceToLO_Points"].Trim();

        string sHedgeCostPoint = Request.Form["txtHedgeCost_Points"].Trim();
        string sCostOnSalePoint = Request.Form["txtCostOnSale_Points"].Trim();
        string sOriginationPts = Request.Form["txtOriginationPts_Points"].Trim();
        string sDiscountPts = Request.Form["txtDiscountPts_Points"].Trim();
        string sExtension1Point = this.txtExtension1_Points.Text.Trim();
        string sExtension2Point = this.txtExtension2_Points.Text.Trim();

        string sCompensationPlan = this.ddlCompensationPlan.SelectedValue;
        string sLockOption = this.ddlLockOption.SelectedItem.Text;
        string sMIOption = this.ddlMIOption.SelectedItem.Text.Trim();
        string sFirsTimeHomebuyer = this.ddlFirsTimeHomebuyer.SelectedValue;
        string sEscrowTaxes = this.ddlEscrowTaxes.SelectedValue;
        string sEscrowInsurance = this.ddlEscrowInsurance.SelectedValue;
        string sPrice = this.txtPrice.Text.Trim();

        #endregion
        #region LoanProfit fields: Price, Lender Credit and Compensation Plan
        decimal tempDecimal = 0;
        LoanProfit LoanProfitMgr = new LoanProfit();
        LPWeb.Model.LoanProfit loanProfitInfo = LoanProfitMgr.GetModel(this.iFileId);
        bool insert = false;
        if (loanProfitInfo == null)
        {
            loanProfitInfo = new LPWeb.Model.LoanProfit();
            insert = true;
        }
        loanProfitInfo.FileId = this.iFileId;
        loanProfitInfo.CompensationPlan = sCompensationPlan;
        if (!string.IsNullOrEmpty(sLenderCredit))
            decimal.TryParse(sLenderCredit, out tempDecimal);
        loanProfitInfo.LenderCredit = tempDecimal;
        tempDecimal = 0;
        if (!string.IsNullOrEmpty(sPrice))
            decimal.TryParse(sPrice, out tempDecimal);
        loanProfitInfo.Price = tempDecimal;
        if (insert)
        {
            LoanProfitMgr.Add(loanProfitInfo);
        }
        else
            LoanProfitMgr.Update(loanProfitInfo);

        #endregion
        #region Mandatory/Final Price

        LoanPointFields LoanPointFieldsMgr = new LoanPointFields();
        DataTable PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 11604);
        if (PointFieldInfo.Rows.Count > 0)
        {
            #region update

            LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, 11604, sMandatoryPricePoint);

            #endregion
        }
        else
        {
            #region insert

            this.AddLoanPointField(this.iFileId, 11604, sMandatoryPricePoint);

            #endregion
        }

        #endregion

        #region Best Effort Price

        PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 12492);
        if (PointFieldInfo.Rows.Count > 0)
        {
            #region update

            LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, 12492, sBestEffortPricePoint);

            #endregion
        }
        else
        {
            #region insert

            this.AddLoanPointField(this.iFileId, 12492, sBestEffortPricePoint);

            #endregion
        }

        #endregion

        #region Commission Total

        // 保存Commission Total %，不再保存Commission Total $
        // LW 08/30/2013 Changed Commission Total % 14153 --> 6177
        PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 6177);
        if (PointFieldInfo.Rows.Count > 0)
        {
            #region update

            LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, 6177, sCommissionTotal_Points);

            #endregion
        }
        else
        {
            #region insert

            this.AddLoanPointField(this.iFileId, 6177, sCommissionTotal_Points);

            #endregion
        }

        #endregion

        #region Lender Credit
        // LW 08/30/2013 Changed Lender Credit 2284 --> 812
        DataTable LenderCreditInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 812);
        if (LenderCreditInfo.Rows.Count > 0)
        {
            #region update

            LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, 812, sLenderCreditAmount);

            #endregion
        }
        else
        {
            #region insert

            this.AddLoanPointField(this.iFileId, 812, sLenderCreditAmount);

            #endregion
        }

        #endregion

        #region LPMI

        PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 12973);
        if (PointFieldInfo.Rows.Count > 0)
        {
            #region update

            LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, 12973, sLPMIPoint);

            #endregion
        }
        else
        {
            #region insert

            this.AddLoanPointField(this.iFileId, 12973, sLPMIPoint);

            #endregion
        }

        #endregion

        #region Best Effort Price to LO

        // only update Best Effort Price to LO %, not Best Effort Price to LO $
        PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 12495);
        if (PointFieldInfo.Rows.Count > 0)
        {
            #region update

            LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, 12495, sBestEffortPriceToLOPoint);

            #endregion
        }
        else
        {
            #region insert

            this.AddLoanPointField(this.iFileId, 12495, sBestEffortPriceToLOPoint);

            #endregion
        }

        #endregion

        #region Hedge Cost

        PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 12974);
        if (PointFieldInfo.Rows.Count > 0)
        {
            #region update

            LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, 12974, sHedgeCostPoint);

            #endregion
        }
        else
        {
            #region insert

            this.AddLoanPointField(this.iFileId, 12974, sHedgeCostPoint);

            #endregion
        }

        #endregion

        #region Cost on sale

        PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 12975);
        if (PointFieldInfo.Rows.Count > 0)
        {
            #region update

            LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, 12975, sCostOnSalePoint);

            #endregion
        }
        else
        {
            #region insert

            this.AddLoanPointField(this.iFileId, 12975, sCostOnSalePoint);

            #endregion
        }

        #endregion

        #region Origination Pts

        PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 11243);
        if (PointFieldInfo.Rows.Count > 0)
        {
            #region update

            LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, 11243, sOriginationPts);

            #endregion
        }
        else
        {
            #region insert

            this.AddLoanPointField(this.iFileId, 11243, sOriginationPts);

            #endregion
        }

        #endregion

        #region Discount Pts

        PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 11241);
        if (PointFieldInfo.Rows.Count > 0)
        {
            #region update

            LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, 11241, sDiscountPts);

            #endregion
        }
        else
        {
            #region insert

            this.AddLoanPointField(this.iFileId, 11241, sDiscountPts);

            #endregion
        }

        #endregion

        #region Extension1

        PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 12976);
        if (PointFieldInfo.Rows.Count > 0)
        {
            #region update

            LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, 12976, sExtension1Point);

            #endregion
        }
        else
        {
            #region insert

            this.AddLoanPointField(this.iFileId, 12976, sExtension1Point);

            #endregion
        }

        #endregion

        #region Extension2

        PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 12977);
        if (PointFieldInfo.Rows.Count > 0)
        {
            #region update

            LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, 12977, sExtension2Point);

            #endregion
        }
        else
        {
            #region insert

            this.AddLoanPointField(this.iFileId, 12977, sExtension2Point);

            #endregion
        }

        #endregion

        #region Lock Option

        LoanLocks LoanLocksMgr = new LoanLocks();

        if (this.LoanLocksInfo.Rows.Count > 0)
        {
            #region update

            LoanLocksMgr.UpdateLockOption(this.iFileId, sLockOption);

            #endregion
        }
        else
        {
            #region insert

            #region Build Model

            LPWeb.Model.LoanLocks LoanLocksModel = new LPWeb.Model.LoanLocks();

            LoanLocksModel.FileId = this.iFileId;

            LoanLocksModel.LockOption = sLockOption;

            #endregion

            LoanLocksMgr.Add(LoanLocksModel);

            #endregion
        }
        // delete all the options first then add the one that's selected
        LoanPointFieldsMgr.DeletePointFields(this.iFileId, " PointFieldId in (6100, 6101, 11438, 12545)");
        int iPointFieldId = 0;
        int.TryParse(ddlLockOption.SelectedValue, out iPointFieldId);
        if (iPointFieldId > 0)
        {
            this.AddLoanPointField(this.iFileId, iPointFieldId, "X");
        }
        #endregion

        #region MI Option

        Loans LoansMgr = new Loans();
        LoansMgr.UpdateMIOption(this.iFileId, sMIOption);
        //LoanPointFieldsMgr.DeletePointFields(this.iFileId, " PointFieldId=4018");
        //this.AddLoanPointField(iFileId, 4018, sMIOption);
        LoanPointFieldsMgr.UpdatePointFieldValue(this.iFileId, 4018, sMIOption);
       
        #endregion

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

        #region Escrow Taxes

        PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(this.iFileId, 4003);
        LoanPointFieldsMgr.DeletePointFields(this.iFileId, " PointFieldId=4003");
        if (sEscrowTaxes.Trim().ToUpper() == "Y")
            this.AddLoanPointField(iFileId, 4003, "X");

        #endregion

        #region Escrow Insurance
        LoanPointFieldsMgr.DeletePointFields(this.iFileId, " PointFieldId=4004");
        if (sEscrowInsurance.Trim().ToUpper() == "Y")
            this.AddLoanPointField(iFileId, 4004, "X");
        #endregion

        #region Call WCF UpdateLockInfo

        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            UpdateLockInfoRequest req = new UpdateLockInfoRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = this.CurrUser.iUserID;
            req.FileId = this.iFileId;

            UpdateLockInfoResponse respone = null;
            try
            {
                respone = service.UpdateLockInfo(req);

                if (respone.hdr.Successful == false)
                {
                    PageCommon.WriteJsEnd(this, "Save successfully, but failed to invoke UpdateLockInfo wcf service, reason:" + respone.hdr.StatusInfo, PageCommon.Js_RefreshSelf);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                string sExMsg = string.Format("Save successfully, but exception happened when Sync Point File (FileID={0}): {1}", this.iFileId, "Point Manager is not running.");
                LPLog.LogMessage(LogType.Logerror, sExMsg);

                PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
            }
            catch (Exception ex)
            {
                string sExMsg = string.Format("Save successfully, but exception happened when Sync Point File (FileID={0}): {1}", this.iFileId, ex.Message);
                LPLog.LogMessage(LogType.Logerror, sExMsg);

                PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
            }
        }

        #endregion

        // success
        PageCommon.WriteJsEnd(this, "Save successfully.", PageCommon.Js_RefreshSelf);
    }

    private void AddLoanPointField(int iFileId, int iPointFieldId, string sCurrentValue)
    {
        LPWeb.Model.LoanPointFields LoanPointFieldsModel = new LPWeb.Model.LoanPointFields();
        LoanPointFieldsModel.FileId = iFileId;
        LoanPointFieldsModel.PointFieldId = iPointFieldId;
        LoanPointFieldsModel.CurrentValue = sCurrentValue;

        if (LoanPointFieldsMgr == null)
            LoanPointFieldsMgr = new LoanPointFields();
        LoanPointFieldsMgr.Add(LoanPointFieldsModel);
    }
}
