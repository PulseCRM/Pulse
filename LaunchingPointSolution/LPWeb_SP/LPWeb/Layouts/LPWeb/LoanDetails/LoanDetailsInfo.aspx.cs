using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.BLL;
using Utilities;
using LPWeb.LP_Service;

public partial class LoanDetails_LoanDetailsInfo : BasePage
{
    protected int iLoanID = 0;
    protected string  sBranchID="0";
    protected string sStatus = "0";
    protected string sForProspect = "1";
    protected string sEnableModify = "1";
    protected string sEnableDispose = "1";
    protected string sEnableSendEmail = "1";

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 检查必要参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("<h2>Missing required query string.</h2>");
            this.Response.End();
        }
        this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

        #endregion

        #region 权限验证
        try
        {
            CheckLoanStatus();
            if (this.CurrUser.userRole.Loans.ToString().IndexOf('G') == -1)
            {
                btnSyncNow.Enabled = false;
            }
            if (this.hdnActiveLoan.Value.ToUpper() == "FALSE")
            {
                btnSyncNow.Enabled = false;
            }
            //CR 38
            if (CurrUser.userRole.ViewLockInfo == true)
            {
                hdnShowLockRatePopup.Value = "true";
            }
            else
            {
                if (CurrUser.userRole.LockRate == false && CurrUser.userRole.ExtendRateLock == false)
                {
                    hdnShowLockRatePopup.Value = "false";
                }
                else
                {
                    hdnShowLockRatePopup.Value = "true";
                }
            }


        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
        #endregion

        #region Codes for Stage Progress Bar

        DataTable StageProgressData = this.GetStageProgressData(this.iLoanID);
        this.rptStageProgressItems.DataSource = StageProgressData;
        this.rptStageProgressItems.DataBind();

        #endregion

        #region Loan Details

        #region 加载Loan Info

        string sSql3 = "select * from Loans where FileId=" + this.iLoanID;
        DataTable LoanInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql3);

        #endregion

        #region Loan Pipleline Info

        Loans LoansMgr = new Loans();
        DataTable LoanPipelineInfo = LoansMgr.GetLoanPipelineInfo(this.iLoanID);

        #endregion

        #region 加载Loan Officer Info

        string sSql4 = "select * from LoanTeam as a1 inner join Users as a2 on a1.UserId = a2.UserId where a1.FileId=" + this.iLoanID + " and a1.RoleId=3 ";
        DataTable LoanOfficerInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql4);

        #endregion

        #region 加载Borrower Info

        string sSql5 = "select * from LoanContacts as b1 inner join Contacts as b2 on b1.ContactId = b2.ContactId where b1.FileId=" + this.iLoanID + "  and b1.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() ";
        DataTable BorrowerInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql5);

        #endregion

        #region 加载CoBorrower Info

        string sSql6 = "select * from LoanContacts as b1 inner join Contacts as b2 on b1.ContactId = b2.ContactId where b1.FileId=" + this.iLoanID + "  and b1.ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId() ";
        DataTable CoBorrowerInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql6);

        #endregion

        #region 加载Point File Info

        string sSql7 = "select * from PointFiles where FileId=" + this.iLoanID;
        DataTable PointFileInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql7);

        #endregion

        #region 绑定Borrower信息
        this.hProspectName.InnerText = "";
        this.lbBorrower.Text = "";
        if (BorrowerInfo.Rows.Count > 0)
        {
            string sBorrowerLastName = BorrowerInfo.Rows[0]["LastName"].ToString();
            string sBorrowerFristName = BorrowerInfo.Rows[0]["FirstName"].ToString();
            string sBorrowerMiddleName = BorrowerInfo.Rows[0]["MiddleName"].ToString();

            StringBuilder sbBorrowerName = new StringBuilder();
            sbBorrowerName.Append(sBorrowerLastName);
            if (sBorrowerFristName != string.Empty)
            {
                sbBorrowerName.Append(", " + sBorrowerFristName);
            }
            if (sBorrowerMiddleName != string.Empty)
            {
                sbBorrowerName.Append(" " + sBorrowerMiddleName);
            }

            this.hProspectName.InnerText = sbBorrowerName.ToString();
            this.lbBorrower.Text = sbBorrowerName.ToString();
        }
        #endregion

        #region 绑定CoBorrower信息

        string sCoBorrowerName = string.Empty;
        if (CoBorrowerInfo.Rows.Count > 0)
        {
            string sCoBorrowerLastName = CoBorrowerInfo.Rows[0]["LastName"].ToString();
            string sCoBorrowerFristName = CoBorrowerInfo.Rows[0]["FirstName"].ToString();
            string sCoBorrowerMiddleName = CoBorrowerInfo.Rows[0]["MiddleName"].ToString();

            StringBuilder sbCoBorrowerName = new StringBuilder();
            sbCoBorrowerName.Append(sCoBorrowerLastName);
            if (sCoBorrowerFristName != string.Empty)
            {
                sbCoBorrowerName.Append(", " + sCoBorrowerFristName);
            }
            if (sCoBorrowerMiddleName != string.Empty)
            {
                sbCoBorrowerName.Append(" " + sCoBorrowerMiddleName);
            }

            sCoBorrowerName = sbCoBorrowerName.ToString();
        }

        this.lbCoborrower.Text = sCoBorrowerName;

        #endregion

        #region 绑定Loan Officer信息
        this.lbLoanOfficer.Text = "";
        if (LoanOfficerInfo.Rows.Count > 0)
        {
            string sLOLastName = LoanOfficerInfo.Rows[0]["LastName"].ToString();
            string sLOFirstName = LoanOfficerInfo.Rows[0]["FirstName"].ToString();
            string sLOName = sLOLastName + ", " + sLOFirstName;
            this.lbLoanOfficer.Text = sLOName;
        }

        #endregion

        #region 绑定Loan信息
        if (LoanInfo.Rows.Count > 0)
        {
            string sStatus = LoanInfo.Rows[0]["Status"].ToString();
            this.lbStatus.Text = sStatus;

            this.sBranchID = LoanInfo.Rows[0]["BranchID"].ToString();
            this.sStatus = sStatus;

            this.lbLoanAmount.Text = LoanInfo.Rows[0]["LoanAmount"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanInfo.Rows[0]["LoanAmount"]).ToString("n0");

            this.lbLienPosition.Text = LoanInfo.Rows[0]["LienPosition"].ToString();

            string sEstCloseDate = LoanInfo.Rows[0]["EstCloseDate"].ToString();
            this.lbEstCloseDate.Text = sEstCloseDate == string.Empty ? string.Empty : Convert.ToDateTime(sEstCloseDate).ToShortDateString();

            this.lbDownPayment.Text = LoanInfo.Rows[0]["DownPay"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanInfo.Rows[0]["DownPay"]).ToString("n0");

            this.lbLoanProgram.Text = LoanInfo.Rows[0]["Program"].ToString();
            this.lbPurpose.Text = LoanInfo.Rows[0]["Purpose"].ToString();
            this.lbCountry.Text = LoanInfo.Rows[0]["County"].ToString();
            this.lbPointFile.Text = PointFileInfo.Rows[0]["Name"].ToString();

            // property
            string sPropertyAddr = LoanInfo.Rows[0]["PropertyAddr"].ToString();
            string sPropertyCity = LoanInfo.Rows[0]["PropertyCity"].ToString();
            string sPropertyState = LoanInfo.Rows[0]["PropertyState"].ToString();
            string sPropertyZip = LoanInfo.Rows[0]["PropertyZip"].ToString();
            string sProperty = sPropertyAddr + ", " + sPropertyCity + ", " + sPropertyState + " " + sPropertyZip;
            this.lbProperty.Text = sProperty;

            this.lbInterestRate.Text = LoanInfo.Rows[0]["Rate"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanInfo.Rows[0]["Rate"]).ToString("n3");

            #region Rate Lock Expiration

            string sRateLockExpiration = LoanInfo.Rows[0]["RateLockExpiration"].ToString();
            if (sRateLockExpiration != string.Empty)
            {
                DateTime RateLockExpiration = Convert.ToDateTime(sRateLockExpiration);
                this.lbRateLock.Text = RateLockExpiration.ToShortDateString();
                System.Threading.Thread.Sleep(1000);
            }
            else
            {
                //灰开锁
                lbRateLock.Text = string.Empty;
            }

            #endregion

            #region Rate Lock Icon

            string sIconFileName = LoanPipelineInfo.Rows[0]["RateLockicon"].ToString();
            if (sIconFileName == "Unknown.png" || sIconFileName == "TaskGreen.png")
            {
                this.imgRateLock.Visible = false;
            }
            else
            {
                imgRateLock.ImageUrl = "../images/loan/" + sIconFileName;
            }

            #endregion

            // LOS Loan Officer
            this.lbLOSLoanOfficer.Text = LoanInfo.Rows[0]["LOS_LoanOfficer"].ToString();

            this.lbFundingDate.Text = LoanInfo.Rows[0]["DateFund"].ToString() == string.Empty ? string.Empty : Convert.ToDateTime(LoanInfo.Rows[0]["DateFund"]).ToShortDateString();
            this.lbNotDate.Text = LoanInfo.Rows[0]["DateNote"].ToString() == string.Empty ? string.Empty : Convert.ToDateTime(LoanInfo.Rows[0]["DateNote"]).ToShortDateString();
        }
        #endregion

        #endregion
    }
    private void CheckLoanStatus()
    {
        LPWeb.BLL.Loans loanMgr = new LPWeb.BLL.Loans();

        this.hdnActiveLoan.Value = loanMgr.IsActiveLoan(iLoanID) == true ? "True" : "False";
    }
    protected void btnSyncNow_Click(object sender, EventArgs e)
    {
        if (lbPointFile.Text.Trim().Length <= 0 || System.IO.Path.GetFileName(lbPointFile.Text.Trim()).Length <= 0)
        {
            PageCommon.WriteJsEnd(this, "Cannot sync with Point, missing Point filename.", PageCommon.Js_RefreshSelf);
            return;
        }

        LPWeb.BLL.Company_Point bllCompanyPoint = new Company_Point();
        var modCompayPoint =bllCompanyPoint.GetModel();
        string MasterSource = modCompayPoint != null ? modCompayPoint.MasterSource : "Point";

        if (MasterSource.ToUpper() == "DataTrac".ToUpper() && sStatus.ToUpper() == "Processing".ToUpper())
        {
            PageCommon.WriteJsEnd(this, "Cannot sync an Active Loan with Point while the master data source is DataTrac.", PageCommon.Js_RefreshSelf);
            return;
        }

        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            ImportLoansRequest req = new ImportLoansRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = this.CurrUser.iUserID;
            req.FileIds = new int[1] { this.iLoanID };
            ImportLoansResponse respone = null;
            try
            {   
                respone = service.ImportLoans(req);

                if (respone.hdr.Successful)
                {
                    PageCommon.WriteJsEnd(this, "Synched point file successfully.", PageCommon.Js_RefreshSelf);
                }
                else
                {
                    PageCommon.WriteJsEnd(this, "Failed to sync Point file, reason:" + respone.hdr.StatusInfo, PageCommon.Js_RefreshSelf);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                string sExMsg = string.Format("Exception happened when Sync Point File (FileID={0}): {1}", this.iLoanID, "Point Manager is not running.");
                LPLog.LogMessage(LogType.Logerror, sExMsg);
                PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
            }
            catch (Exception ex)
            {
                string sExMsg = string.Format("Exception happened when Sync Point File (FileID={0}): {1}", this.iLoanID, ex.Message);
                LPLog.LogMessage(LogType.Logerror, sExMsg);
                PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
            }
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    { 
        
    }

    #region Methods for Stage Progress Bar

    /// <summary>
    /// get stage progress data
    /// neo 2011-02-26
    /// </summary>
    /// <param name="iLoanID"></param>
    /// <returns></returns>
    private DataTable GetStageProgressData(int iLoanID)
    {
        string sSql = "select dbo.lpfn_GetStageProgressImageFileName(LoanStageId) as StageImage, dbo.lpfn_GetStageAlias(LoanStageId) as StageAlias, * from LoanStages where FileId=" + iLoanID + " order by SequenceNumber";
        DataTable StageProgressData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        return StageProgressData;
    }

    /// <summary>
    /// get stage tool tip
    /// neo 2011-02-26
    /// </summary>
    /// <param name="sStageAlias"></param>
    /// <param name="sCompleteDate"></param>
    /// <returns></returns>
    public string GetStageToolTip(string sStageAlias, string sCompleteDate)
    {
        if (sCompleteDate == string.Empty)
        {
            return sStageAlias;
        }
        else
        {
            return sStageAlias + " " + sCompleteDate;
        }
    }

    /// <summary>
    /// get span html for stage alias and complete date
    /// neo 2011-02-26
    /// </summary>
    /// <param name="sStageAlias"></param>
    /// <param name="sStageImage"></param>
    /// <param name="sCompleteDate"></param>
    /// <returns></returns>
    public string GetSpan_StageAliasCompleteDate(string sStageAlias, string sStageImage, string sCompleteDate)
    {
        string sSpanText = string.Empty;

        if (sStageImage.Contains("Grass") || sStageImage.Contains("Pink") || sStageImage.Contains("Silver") || sStageImage.Contains("Yellow"))
        {
            sSpanText += "<span style=\"color: #000;\">" + sStageAlias + "</span>";
            if (sCompleteDate != string.Empty)
            {
                sSpanText += "<span style=\"color: #000; padding-top: 2px;\">" + Convert.ToDateTime(sCompleteDate).ToShortDateString() + "</span>";
            }
        }
        else
        {
            sSpanText += "<span>" + sStageAlias + "</span>";
            if (sCompleteDate != string.Empty)
            {
                sSpanText += "<span style=\"padding-top: 2px;\">" + Convert.ToDateTime(sCompleteDate).ToShortDateString() + "</span>";
            }
        }

        return sSpanText;
    }

    #endregion

    private int GetDateSpan(DateTime lockDate)
    {
        DateTime dt1 = lockDate;
        DateTime dt2 = DateTime.Today;//获取今天日期        
        TimeSpan ts1 = dt1.Subtract(dt2);//TimeSpan得到dt1和dt2的时间间隔        
        int countday = ts1.Days;//获取两个日期间的总天数        
        int weekday = 0;//工作日        //循环用来扣除总天数中的双休日        
        for (int i = 0; i < countday; i++)
        {
            DateTime tempdt = dt1.Date.AddDays(i);
            if (tempdt.DayOfWeek != System.DayOfWeek.Saturday &&
                tempdt.DayOfWeek != System.DayOfWeek.Sunday)
            {
                weekday++;
            }
        }

        return weekday;
    }
}
