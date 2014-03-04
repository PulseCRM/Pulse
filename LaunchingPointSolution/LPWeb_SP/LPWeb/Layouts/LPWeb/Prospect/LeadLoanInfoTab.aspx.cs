using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.BLL;
using System.Data;
using System.Web.UI.WebControls;
using LPWeb.Common;

public partial class LeadLoanInfoTab : BasePage
{
    int iLoanId = 0;    // 0: Create Lead, >0: Edit Lead
    int iContactId = 0;

    /// <summary>
    /// Edit_HasLoanId_NoContactId
    /// Edit_HasContactId_HasLoan
    /// </summary>
    string sActionMode = string.Empty;

    DataTable BorrowerInfo_Contact = null;
    DataTable BorrowerInfo_Prospect = null;

    DataTable CoBorrowerInfo_Contact = null;
    DataTable CoBorrowerInfo_Prospect = null;

    DataTable CurrentLoanInfo = null;

    DataTable BorrowerInfo_Employment = null;
    DataTable BorrowerInfo_Income = null;
    DataTable BorrowerInfo_Assets = null;

    Loans LoansMgr = new Loans();
    Prospect ProspectMgr = new Prospect();
    Contacts ContactMgr = new Contacts();

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        #region LoanId

        if (this.Request.QueryString["FileID"] != null)     // edit lead
        {
            #region 校验LoanId

            string sLoanId = this.Request.QueryString["FileID"];
            bool bValid = PageCommon.IsID(sLoanId);
            if (bValid == false)
            {
                PageCommon.WriteJsEnd(this, "Invalid loan id.", "window.location.href='LeadCreate.aspx'");
                return;
            }

            #endregion

            #region LoanId -> 加载CurrentLoanInfo

            // 加载Prospect Loan Info
            this.CurrentLoanInfo = this.GetProspectLoanInfo(sLoanId);
            if (this.CurrentLoanInfo.Rows.Count == 0)
            {
                PageCommon.WriteJsEnd(this, "Invalid loan id.", "window.location.href='LeadCreate.aspx'");
                return;
            }

            // LoanId
            this.iLoanId = Convert.ToInt32(sLoanId);

            #endregion

            // 加载Borrower/Co-Boorwer Info
            this.InitBorrowerCoBorrowerInfo(this.iLoanId);

            this.sActionMode = "Edit_HasLoanId_NoContactId";     // 纯edit
        }
        else
        {
            PageCommon.WriteJsEnd(this, "Invalid query string arrange.", "window.location.href='LeadCreate.aspx'");
            return;
        }

        #endregion

        #endregion

        if (this.IsPostBack == false)
        {

            #region 加载Program列表

            Company_Loan_Programs ProgramMgr = new Company_Loan_Programs();
            DataTable ProgramList = ProgramMgr.GetList("1=1 order by LoanProgram").Tables[0];

            this.ddlProgram.Items.Add(new ListItem("-- select --", ""));
            this.ddlProgramNewLoan.Items.Add(new ListItem("-- select --", ""));

            foreach (DataRow ProgramRow in ProgramList.Rows)
            {
                string sLoanProgram = ProgramRow["LoanProgram"].ToString();
                this.ddlProgram.Items.Add(sLoanProgram);
                this.ddlProgramNewLoan.Items.Add(sLoanProgram);
            }

            #endregion

            #region 加载Start Date→Year

            this.ddlStartYear.Items.Add(new ListItem("year", ""));
            this.ddlStartYearNewLoan.Items.Add(new ListItem("year", ""));

            DateTime saveNow = DateTime.Now;

            int iBeginYear = saveNow.Year;
            for (int i = 0; i < 41; i++)
            {
                int iNextYear = iBeginYear - i;
                this.ddlStartYear.Items.Add(iNextYear.ToString());
                this.ddlStartYearNewLoan.Items.Add(iNextYear.ToString());
            }

            #endregion

            #region Loan Info

            this.BindingData_LoanInfo(this.CurrentLoanInfo);

            #endregion

        }
    }

    #region create months

    protected int CreateContactAndProspect(string sFirstName, string sLastName, string sEmail, string sCellPhone, string sHomePhone,
        string sWorkPhone, string sDOB, string sSSN,
        string sDependants, string sCreditRanking, string sFICO,
        string sMailingStreetAddress1, string sMailingStreetAddress2, string sMailingCity, string sMailingState, string sMailingZip,
        string sLeadSource, string sReferralID)
    {
        #region create new contact

        LPWeb.Model.Contacts ContactsModel = new LPWeb.Model.Contacts();
        ContactsModel.ContactId = 0;
        ContactsModel.FirstName = sFirstName;
        ContactsModel.LastName = sLastName;
        ContactsModel.Email = sEmail;

        ContactsModel.CellPhone = sCellPhone;
        ContactsModel.HomePhone = sHomePhone;
        ContactsModel.BusinessPhone = sWorkPhone;

        if (sDOB == string.Empty)
        {
            ContactsModel.DOB = null;
        }
        else
        {
            ContactsModel.DOB = Convert.ToDateTime(sDOB);
        }

        ContactsModel.SSN = sSSN;

        if (sFICO == string.Empty)
        {
            ContactsModel.Experian = null;
        }
        else
        {
            ContactsModel.Experian = Convert.ToInt32(sFICO);
        }

        ContactsModel.MailingAddr = (sMailingStreetAddress1 + " " + sMailingStreetAddress2).Trim();
        ContactsModel.MailingCity = sMailingCity;
        ContactsModel.MailingState = sMailingState;
        ContactsModel.MailingZip = sMailingZip;

        ContactsModel.Created = DateTime.Now;
        ContactsModel.CreatedBy = this.CurrUser.iUserID;

        ContactsModel.MiddleName = string.Empty;
        ContactsModel.NickName = sFirstName;
        ContactsModel.Title = string.Empty;
        ContactsModel.GenerationCode = string.Empty;
        ContactsModel.Fax = string.Empty;


        #endregion

        #region create new prospect

        LPWeb.Model.Prospect ProspectModel = new LPWeb.Model.Prospect();

        ProspectModel.Contactid = 0;
        ProspectModel.LeadSource = sLeadSource;
        if (sReferralID == string.Empty)
        {
            ProspectModel.Referral = null;
        }
        else
        {
            ProspectModel.Referral = Convert.ToInt32(sReferralID);
        }
        ProspectModel.CreditRanking = sCreditRanking;
        ProspectModel.Created = DateTime.Now;
        ProspectModel.CreatedBy = this.CurrUser.iUserID;
        ProspectModel.Status = "Active";
        if (sDependants == "Yes")
        {
            ProspectModel.Dependents = true;
        }
        else
        {
            ProspectModel.Dependents = false;
        }

        ProspectModel.Modifed = null;
        ProspectModel.ModifiedBy = null;
        ProspectModel.Loanofficer = null;
        ProspectModel.ReferenceCode = null;
        ProspectModel.PreferredContact = null;

        #endregion

        int iContactId = this.ProspectMgr.CreateContactAndProspect(ContactsModel, ProspectModel);

        return iContactId;
    }

    private DateTime? GetStartDate(string sStartYear, string sStartMonth)
    {
        if (sStartYear != string.Empty && sStartMonth != string.Empty)
        {
            int iStartYear = Convert.ToInt32(sStartYear);
            int iStartMonth = Convert.ToInt32(sStartMonth);

            return new DateTime(iStartYear, iStartMonth, 1);
        }
        else
        {
            return null;
        }
    }

    private string GetStartDateStr(string sStartYear, string sStartMonth)
    {
        if (sStartYear != string.Empty && sStartMonth != string.Empty)
        {
            int iStartYear = Convert.ToInt32(sStartYear);
            int iStartMonth = Convert.ToInt32(sStartMonth);

            DateTime StartDate = new DateTime(iStartYear, iStartMonth, 1);

            return StartDate.ToShortDateString();
        }
        else
        {
            return string.Empty;
        }
    }

    protected int CreateLoan(int iBorrowerID, int iCoBorrowerID, string sHousingStatus, string sRentAmount,
        string sPropertyStreetAddress1, string sPropertyStreetAddress2, string sPropertyCity, string sPropertyState,
        string sPropertyZip, string sPropertyValue, string sPurpose, string sLoanType, string sProgram, string sAmount,
        string sRate, string sPMI, string sPMITax, string sTerm, string sStartYear, string sStartMonth, bool bSubordinate, bool b2nd, string s2ndAmount, string sRanking)
    {
        int iFolderId = 0;

        LPWeb.Model.LoanDetails LoanDetailsModel = new LPWeb.Model.LoanDetails();

        LoanDetailsModel.FileId = 0;
        LoanDetailsModel.FolderId = iFolderId;
        LoanDetailsModel.Status = "Prospect";
        LoanDetailsModel.ProspectLoanStatus = "Active";
        LoanDetailsModel.Ranking = sRanking; //"Hot";
        LoanDetailsModel.BoID = iBorrowerID;
        LoanDetailsModel.CoBoID = iCoBorrowerID;
        LoanDetailsModel.HousingStatus = sHousingStatus;
        if (sRentAmount == string.Empty)
        {
            LoanDetailsModel.RentAmount = 0;
        }
        else
        {
            LoanDetailsModel.RentAmount = Convert.ToDecimal(sRentAmount);
        }
        LoanDetailsModel.PropertyAddr = (sPropertyStreetAddress1 + " " + sPropertyStreetAddress2).Trim();
        LoanDetailsModel.PropertyCity = sPropertyCity;
        LoanDetailsModel.PropertyState = sPropertyState;
        LoanDetailsModel.PropertyZip = sPropertyZip;

        if (sPropertyValue == string.Empty)
        {
            LoanDetailsModel.SalesPrice = null;
        }
        else
        {
            LoanDetailsModel.SalesPrice = Convert.ToDecimal(sPropertyValue);
        }

        LoanDetailsModel.Purpose = sPurpose;
        LoanDetailsModel.LoanType = sLoanType;
        LoanDetailsModel.Program = sProgram;
        if (sAmount == "")
        {
            LoanDetailsModel.LoanAmount = null;
        }
        else
        {
            LoanDetailsModel.LoanAmount = Convert.ToDecimal(sAmount);
        }

        if (sRate == string.Empty)
        {
            LoanDetailsModel.Rate = null;
        }
        else
        {
            LoanDetailsModel.Rate = Convert.ToDecimal(sRate);
        }

        if (sPMI == string.Empty)
        {
            LoanDetailsModel.MonthlyPMI = null;
        }
        else
        {
            LoanDetailsModel.MonthlyPMI = Convert.ToDecimal(sPMI);
        }

        if (sPMITax == string.Empty)
        {
            LoanDetailsModel.MonthlyPMITax = null;
        }
        else
        {
            LoanDetailsModel.MonthlyPMITax = Convert.ToDecimal(sPMITax);
        }

        if (sTerm == string.Empty)
        {
            LoanDetailsModel.Term = null;
        }
        else
        {
            LoanDetailsModel.Term = Convert.ToInt16(sTerm);
        }

        // StartDate=MM/yyyy/01
        LoanDetailsModel.EstCloseDate = this.GetStartDate(sStartYear, sStartMonth);

        LoanDetailsModel.TD_2 = b2nd;
        Decimal it = 0;
        if (LoanDetailsModel.TD_2 == true)
        {
            if (Decimal.TryParse(s2ndAmount, out it))
            {
                LoanDetailsModel.TD_2Amount = it;
            }
            else
            {
                LoanDetailsModel.TD_2Amount = null;
            }
        }
        else
        {
            LoanDetailsModel.TD_2Amount = null;
        }

        LoanDetailsModel.Subordinate = bSubordinate;

        LoanDetailsModel.Created = DateTime.Now;
        LoanDetailsModel.CreatedBy = this.CurrUser.iUserID;
        LoanDetailsModel.Modifed = null;
        LoanDetailsModel.ModifiedBy = null;
        LoanDetailsModel.UserId = this.CurrUser.iUserID;
        LoanDetailsModel.LoanOfficerId = this.CurrUser.iUserID;

        LoanDetailsModel.Lien = string.Empty;
        LoanDetailsModel.FileName = string.Empty;
        LoanDetailsModel.IncludeEscrows = false;
        LoanDetailsModel.InterestOnly = false;
        LoanDetailsModel.CoborrowerType = string.Empty;

        int iFileID = this.LoansMgr.LoanDetailSaveFileId(LoanDetailsModel);

        return iFileID;

    }

    protected void CloseNewLoan(int iFileId, string sCloseDate, int iUserID)
    {

        this.LoansMgr.MakeProspectLoanClosed(iFileId, sCloseDate, this.CurrUser.iUserID);
    }

    #endregion

    #region edit methods

    private void InitBorrowerCoBorrowerInfo(int iFileId)
    {
        #region 获取BorrowerID and CoBorrowerID

        int? iBorrowerID = this.LoansMgr.GetBorrowerID(iFileId);
        int? iCoBorrowerID = this.LoansMgr.GetCoBorrowerID(iFileId);

        #endregion

        #region 加载BorrowerInfo

        if (iBorrowerID != null)
        {
            // 加载Contact Info
            this.BorrowerInfo_Contact = this.ContactMgr.GetContactInfo((int)iBorrowerID);

            // 加载Prospect Info
            this.BorrowerInfo_Prospect = this.ProspectMgr.GetProspectInfo((int)iBorrowerID);
        }

        #endregion

        #region 加载CoBorrowerInfo

        if (iCoBorrowerID != null)
        {
            // 加载Contact Info
            this.CoBorrowerInfo_Contact = this.ContactMgr.GetContactInfo((int)iCoBorrowerID);

            // 加载Prospect Info
            this.CoBorrowerInfo_Prospect = this.ProspectMgr.GetProspectInfo((int)iCoBorrowerID);
        }

        #endregion

        #region 加载Income and Employment信息

        if (iBorrowerID != null)
        {
            this.InitIncomeAndEmploymentInfo((int)iBorrowerID);
        }

        #endregion
    }

    private void InitIncomeAndEmploymentInfo(int iBorrowerID)
    {
        #region 加载 Income and Employment

        #region 加载Employment信息

        ProspectEmployment ProspectEmploymentMgr = new ProspectEmployment();
        DataTable EmploymentInfo = ProspectEmploymentMgr.GetList("1=1 and ContactId=" + iBorrowerID + " order by EmplId desc").Tables[0];
        if (EmploymentInfo.Rows.Count > 0)
        {
            this.BorrowerInfo_Employment = EmploymentInfo.Copy();
        }

        #endregion

        #region 加载Income信息

        ProspectIncome ProspectIncomeMgr = new ProspectIncome();
        DataTable IncomeInfo = ProspectIncomeMgr.GetList("1=1 and ContactId=" + iBorrowerID + " order by ProspectIncomeId desc").Tables[0];
        if (IncomeInfo.Rows.Count > 0)
        {
            this.BorrowerInfo_Income = IncomeInfo.Copy();
        }

        #endregion

        #region 加载Assets信息

        ProspectAssets ProspectAssetsMgr = new ProspectAssets();
        DataTable AssetsInfo = ProspectAssetsMgr.GetList("1=1 and ContactId=" + iBorrowerID + " order by ProspectAssetId desc").Tables[0];
        if (AssetsInfo.Rows.Count > 0)
        {
            this.BorrowerInfo_Assets = AssetsInfo.Copy();
        }

        #endregion

        #endregion
    }

    private DataTable GetProspectLoanInfo(string sLoanId)
    {
        string sSql = "select * from Loans where Status='Prospect' and FileId=" + sLoanId;
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private DataTable GetContactLoanInfo(int iContactId)
    {
        string sSql = "select top 1 * from Loans l inner join LoanContacts lc on l.FileId=lc.FileId "
                    + "where (dbo.lpfn_GetBorrowerContactId(l.FileId)=" + iContactId + " or dbo.lpfn_GetCoBorrowerContactId(l.FileId)=" + iContactId + ") and l.Status='Prospect' "
                    + "order by Created desc";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private DataTable GetBorrowerClosedLoanInfo(int iContactId)
    {
        string sSql = "select top 1 * from Loans l inner join LoanContacts lc on l.FileId=lc.FileId "
                    + "where (dbo.lpfn_GetBorrowerContactId(l.FileId)=" + iContactId + " or dbo.lpfn_GetCoBorrowerContactId(l.FileId)=" + iContactId + ") and l.Status='Closed' "
                    + "order by Created desc";

        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }


    private void BindingData_LoanInfo(DataTable LoanInfoData)
    {
        #region Loan Info

        // Archive Loan
        //this.ddlArchiveLoan.SelectedValue = LoanInfoData.Rows[0]["Status"].ToString();

        this.ddlPurpose.SelectedValue = LoanInfoData.Rows[0]["Purpose"].ToString();
        this.ddlProgram.SelectedValue = LoanInfoData.Rows[0]["Program"].ToString();
        this.ddlType.SelectedValue = LoanInfoData.Rows[0]["LoanType"].ToString();
        this.txtAmount.Text = LoanInfoData.Rows[0]["LoanAmount"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanInfoData.Rows[0]["LoanAmount"]).ToString("n0");
        this.txtRate.Text = LoanInfoData.Rows[0]["Rate"].ToString();
        this.txtPMI.Text = LoanInfoData.Rows[0]["MonthlyPMI"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanInfoData.Rows[0]["MonthlyPMI"]).ToString("n0");
        this.txtPMITax.Text = LoanInfoData.Rows[0]["MonthlyPMITax"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanInfoData.Rows[0]["MonthlyPMITax"]).ToString("n0");
        this.txtTerm.Text = LoanInfoData.Rows[0]["Term"].ToString();

        decimal dRate = decimal.Zero;
        bool t_status = false;

        if (LoanInfoData.Rows[0]["Rate"] != DBNull.Value)
        {
            t_status = decimal.TryParse(this.txtRate.Text, out dRate);

            if (t_status == true)
            {
                if (dRate < 10)
                {
                    this.txtRate.Text = '0' + this.txtRate.Text;
                }
            }
        }

        if (LoanInfoData.Rows[0]["EstCloseDate"] != DBNull.Value)
        {
            DateTime StartDate = Convert.ToDateTime(LoanInfoData.Rows[0]["EstCloseDate"]);

            this.ddlStartYear.SelectedValue = StartDate.Year.ToString();
            this.ddlStartMonth.SelectedValue = StartDate.Month.ToString();
        }

        this.chk2nd.Checked = LoanInfoData.Rows[0]["TD_2"] == DBNull.Value ? false : Convert.ToBoolean(LoanInfoData.Rows[0]["TD_2"]);
        this.txt2ndAmount.Text = LoanInfoData.Rows[0]["TD_2Amount"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanInfoData.Rows[0]["TD_2Amount"]).ToString("n0");

        #endregion
    }



    #region create income and employment


    /// <summary>
    /// 
    /// </summary>
    /// <param name="iFileId"></param>
    /// <param name="iContactId"></param>
    /// <param name="sCompanyName"></param>
    /// <param name="bSelfEmployed"></param>
    /// <param name="sPosition"></param>
    /// <param name="sMonthlySalar"></param>
    /// <param name="sProfession"></param>
    /// <param name="sYearsInField"></param>
    /// <param name="sStartYear"></param>
    /// <param name="sStartMonth"></param>
    /// <param name="sEndYear"></param>
    /// <param name="sEndMonth"></param>
    /// <param name="sOtherMonthlyIncome"></param>
    /// <param name="sLiguidAssets"></param>
    /// <param name="sComments"></param>
    private void CreateIncomeAndEmployment(int iFileId, int iContactId, string sCompanyName, bool bSelfEmployed, string sPosition, string sMonthlySalar, string sProfession,
        string sYearsInField, string sStartYear, string sStartMonth, string sEndYear, string sEndMonth,
        string sOtherMonthlyIncome, string sLiquidAssets, string sComments)
    {
        // create ProspectEmployment
        this.CreateProspectEmployment(iContactId, sCompanyName, bSelfEmployed, sPosition, sProfession,
            sYearsInField, sStartYear, sStartMonth, sEndYear, sEndMonth);

        // create ProspectIncome
        this.CreateProspectIncome(iContactId, sMonthlySalar, sOtherMonthlyIncome);

        // create ProspectAssets
        this.CreateProspectAssets(iContactId, sLiquidAssets);

        // create LoanNotes
        this.CreateLoanNotes(iFileId, sComments);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iContactId"></param>
    /// <param name="sCompanyName"></param>
    /// <param name="bSelfEmployed"></param>
    /// <param name="sPosition"></param>
    /// <param name="sProfession"></param>
    /// <param name="sYearsInField"></param>
    /// <param name="sStartYear"></param>
    /// <param name="sStartMonth"></param>
    /// <param name="sEndYear"></param>
    /// <param name="sEndMonth"></param>
    private void CreateProspectEmployment(int iContactId, string sCompanyName, bool bSelfEmployed, string sPosition, string sProfession,
        string sYearsInField, string sStartYear, string sStartMonth, string sEndYear, string sEndMonth)
    {
        #region create ProspectEmployment

        LPWeb.Model.ProspectEmployment ProspectEmploymentModel = new LPWeb.Model.ProspectEmployment();

        ProspectEmploymentModel.ContactId = iContactId;
        ProspectEmploymentModel.CompanyName = sCompanyName;
        ProspectEmploymentModel.SelfEmployed = bSelfEmployed;
        ProspectEmploymentModel.Position = sPosition;
        ProspectEmploymentModel.BusinessType = sProfession;

        if (sYearsInField == string.Empty)
        {
            ProspectEmploymentModel.YearsOnWork = null;
        }
        else
        {
            ProspectEmploymentModel.YearsOnWork = Convert.ToDecimal(sYearsInField);
        }

        if (sStartYear == string.Empty)
        {
            ProspectEmploymentModel.StartYear = null;
        }
        else
        {
            ProspectEmploymentModel.StartYear = Convert.ToDecimal(sStartYear);
        }

        if (sStartMonth == string.Empty)
        {
            ProspectEmploymentModel.StartMonth = null;
        }
        else
        {
            ProspectEmploymentModel.StartMonth = Convert.ToDecimal(sStartMonth);
        }

        if (sEndYear == string.Empty)
        {
            ProspectEmploymentModel.EndYear = null;
        }
        else
        {
            ProspectEmploymentModel.EndYear = Convert.ToDecimal(sEndYear);
        }

        if (sEndMonth == string.Empty)
        {
            ProspectEmploymentModel.EndMonth = null;
        }
        else
        {
            ProspectEmploymentModel.EndMonth = Convert.ToDecimal(sEndMonth);
        }

        ProspectEmployment ProspectEmploymentMgr = new ProspectEmployment();
        ProspectEmploymentMgr.Add(ProspectEmploymentModel);

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iContactId"></param>
    /// <param name="sMonthlySalar"></param>
    /// <param name="sOtherMonthlyIncome"></param>
    private void CreateProspectIncome(int iContactId, string sMonthlySalar, string sOtherMonthlyIncome)
    {
        #region create ProspectIncome

        LPWeb.Model.ProspectIncome ProspectIncomeModel = new LPWeb.Model.ProspectIncome();

        ProspectIncomeModel.ContactId = iContactId;
        if (sMonthlySalar == string.Empty)
        {
            ProspectIncomeModel.Salary = null;
        }
        else
        {
            ProspectIncomeModel.Salary = Convert.ToDecimal(sMonthlySalar);
        }

        if (sOtherMonthlyIncome == string.Empty)
        {
            ProspectIncomeModel.Other = null;
        }
        else
        {
            ProspectIncomeModel.Other = Convert.ToDecimal(sOtherMonthlyIncome);
        }

        ProspectIncome ProspectIncomeMgr = new ProspectIncome();
        ProspectIncomeMgr.Add(ProspectIncomeModel);


        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iContactId"></param>
    /// <param name="sLiquidAssets"></param>
    private void CreateProspectAssets(int iContactId, string sLiquidAssets)
    {
        #region create ProspectAssets

        LPWeb.Model.ProspectAssets ProspectAssetsModel = new LPWeb.Model.ProspectAssets();

        ProspectAssetsModel.ContactId = iContactId;
        if (sLiquidAssets == string.Empty)
        {
            ProspectAssetsModel.Amount = null;
        }
        else
        {
            ProspectAssetsModel.Amount = Convert.ToDecimal(sLiquidAssets);
        }
        ProspectAssetsModel.Type = "Other";
        ProspectAssets ProspectAssetsMgr = new ProspectAssets();
        ProspectAssetsMgr.Add(ProspectAssetsModel);

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iFileId"></param>
    /// <param name="sComments"></param>
    private void CreateLoanNotes(int iFileId, string sComments)
    {
        #region create LoanNotes

        LPWeb.Model.LoanNotes LoanNotesModel = new LPWeb.Model.LoanNotes();

        LoanNotesModel.FileId = iFileId;
        LoanNotesModel.Created = DateTime.Now;
        LoanNotesModel.Sender = this.CurrUser.sFirstName + " " + this.CurrUser.sLastName;
        LoanNotesModel.Note = sComments;

        LoanNotes LoanNotesMgr = new LoanNotes();
        LoanNotesMgr.Add(LoanNotesModel);

        #endregion
    }

    #endregion

    #region update income and employment

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iContactId"></param>
    /// <param name="sCompanyName"></param>
    /// <param name="bSelfEmployed"></param>
    /// <param name="sPosition"></param>
    /// <param name="sProfession"></param>
    /// <param name="sYearsInField"></param>
    /// <param name="sStartYear"></param>
    /// <param name="sStartMonth"></param>
    /// <param name="sEndYear"></param>
    /// <param name="sEndMonth"></param>
    private void UpdateProspectEmployment(int iEmploymentID, string sCompanyName, bool bSelfEmployed, string sPosition, string sProfession,
        string sYearsInField, string sStartYear, string sStartMonth, string sEndYear, string sEndMonth)
    {
        #region update ProspectEmployment

        ProspectEmployment ProspectEmploymentMgr = new ProspectEmployment();
        LPWeb.Model.ProspectEmployment ProspectEmploymentModel = ProspectEmploymentMgr.GetModel(iEmploymentID);

        ProspectEmploymentModel.CompanyName = sCompanyName;
        ProspectEmploymentModel.SelfEmployed = bSelfEmployed;
        ProspectEmploymentModel.Position = sPosition;
        ProspectEmploymentModel.BusinessType = sProfession;

        if (sYearsInField == string.Empty)
        {
            ProspectEmploymentModel.YearsOnWork = null;
        }
        else
        {
            ProspectEmploymentModel.YearsOnWork = Convert.ToDecimal(sYearsInField);
        }

        if (sStartYear == string.Empty)
        {
            ProspectEmploymentModel.StartYear = null;
        }
        else
        {
            ProspectEmploymentModel.StartYear = Convert.ToDecimal(sStartYear);
        }

        if (sStartMonth == string.Empty)
        {
            ProspectEmploymentModel.StartMonth = null;
        }
        else
        {
            ProspectEmploymentModel.StartMonth = Convert.ToDecimal(sStartMonth);
        }

        if (sEndYear == string.Empty)
        {
            ProspectEmploymentModel.EndYear = null;
        }
        else
        {
            ProspectEmploymentModel.EndYear = Convert.ToDecimal(sEndYear);
        }

        if (sEndMonth == string.Empty)
        {
            ProspectEmploymentModel.EndMonth = null;
        }
        else
        {
            ProspectEmploymentModel.EndMonth = Convert.ToDecimal(sEndMonth);
        }

        ProspectEmploymentMgr.Update(ProspectEmploymentModel);

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iContactId"></param>
    /// <param name="sMonthlySalar"></param>
    /// <param name="sOtherMonthlyIncome"></param>
    private void UpdateProspectIncome(int iProspectIncomeID, string sMonthlySalar, string sOtherMonthlyIncome)
    {
        #region update ProspectIncome

        ProspectIncome ProspectIncomeMgr = new ProspectIncome();
        LPWeb.Model.ProspectIncome ProspectIncomeModel = ProspectIncomeMgr.GetModel(iProspectIncomeID);

        if (sMonthlySalar == string.Empty)
        {
            ProspectIncomeModel.Salary = null;
        }
        else
        {
            ProspectIncomeModel.Salary = Convert.ToDecimal(sMonthlySalar);
        }

        if (sOtherMonthlyIncome == string.Empty)
        {
            ProspectIncomeModel.Other = null;
        }
        else
        {
            ProspectIncomeModel.Other = Convert.ToDecimal(sOtherMonthlyIncome);
        }


        ProspectIncomeMgr.Update(ProspectIncomeModel);


        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iContactId"></param>
    /// <param name="sLiquidAssets"></param>
    private void UpdateProspectAssets(int iProspectAssetsID, string sLiquidAssets)
    {
        #region update ProspectAssets

        ProspectAssets ProspectAssetsMgr = new ProspectAssets();
        LPWeb.Model.ProspectAssets ProspectAssetsModel = ProspectAssetsMgr.GetModel(iProspectAssetsID);

        if (sLiquidAssets == string.Empty)
        {
            ProspectAssetsModel.Amount = null;
        }
        else
        {
            ProspectAssetsModel.Amount = Convert.ToDecimal(sLiquidAssets);
        }

        ProspectAssetsMgr.Update(ProspectAssetsModel);

        #endregion
    }

    #endregion

    #endregion


    private bool SaveLead(out int iNewFileId)
    {
        iNewFileId = 0;

        #region get user input

        #region Borrower Info

        string sFirstName = this.BorrowerInfo_Contact.Rows[0]["FirstName"].ToString();
        string sLastName = this.BorrowerInfo_Contact.Rows[0]["LastName"].ToString();
        string sEmail = this.BorrowerInfo_Contact.Rows[0]["Email"].ToString();
        string sCellPhone = this.BorrowerInfo_Contact.Rows[0]["CellPhone"].ToString();
        string sHomePhone = this.BorrowerInfo_Contact.Rows[0]["HomePhone"].ToString();
        string sWorkPhone = this.BorrowerInfo_Contact.Rows[0]["BusinessPhone"].ToString();
        string sDOB = this.BorrowerInfo_Contact.Rows[0]["DOB"] == DBNull.Value ? string.Empty : Convert.ToDateTime(this.BorrowerInfo_Contact.Rows[0]["DOB"]).ToString("MM/dd/yyyy");
        DateTime DOB;
        DateTime DT = DateTime.Today;
        DT = DateTime.Today.AddYears(-18);

        if (sDOB != string.Empty)
        {
            if (DateTime.TryParse(sDOB, out DOB) == false)
            {
                PageCommon.AlertMsg(this, "Date of Birth is not a valid date format.");
                return false;
            }
            if (DOB > DT)
            {
                PageCommon.AlertMsg(this, "Date of Birth should be 18 years old.");
                return false;
            }

        }
        string sSSN = this.BorrowerInfo_Contact.Rows[0]["SSN"].ToString();
        string sDependants = this.BorrowerInfo_Prospect.Rows[0]["Dependents"].ToString();
        string sCreditRanking = this.BorrowerInfo_Prospect.Rows[0]["CreditRanking"].ToString();
        string sHousingStatus = this.CurrentLoanInfo.Rows[0]["HousingStatus"].ToString();
        string sRentAmount = this.CurrentLoanInfo.Rows[0]["RentAmount"] == DBNull.Value ? string.Empty : Convert.ToDecimal(this.CurrentLoanInfo.Rows[0]["RentAmount"]).ToString("n0");

        string sFICOScore = this.BorrowerInfo_Contact.Rows[0]["Experian"].ToString();

        #endregion

        #region Co-Borrower Info

        string sFirstNameCoBorrower = this.CoBorrowerInfo_Contact == null ? "" : this.CoBorrowerInfo_Contact.Rows[0]["FirstName"].ToString();
        string sLastNameCoBorrower = this.CoBorrowerInfo_Contact == null ? "" : this.CoBorrowerInfo_Contact.Rows[0]["LastName"].ToString();
        string sEmailCoBorrower = this.CoBorrowerInfo_Contact == null ? "" : this.CoBorrowerInfo_Contact.Rows[0]["Email"].ToString();
        string sCellPhoneCoBorrower = this.CoBorrowerInfo_Contact == null ? "" : this.CoBorrowerInfo_Contact.Rows[0]["CellPhone"].ToString();
        string sHomePhoneCoBorrower = this.CoBorrowerInfo_Contact == null ? "" : this.CoBorrowerInfo_Contact.Rows[0]["HomePhone"].ToString();
        string sWorkPhoneCoBorrower = this.CoBorrowerInfo_Contact == null ? "" : this.CoBorrowerInfo_Contact.Rows[0]["BusinessPhone"].ToString();
        string sDOBCoBorrower = (this.CoBorrowerInfo_Contact == null || this.CoBorrowerInfo_Contact.Rows[0]["DOB"] == DBNull.Value) ? string.Empty : Convert.ToDateTime(this.CoBorrowerInfo_Contact.Rows[0]["DOB"]).ToString("MM/dd/yyyy");
        DateTime DOBCoBorrower;
        if (sDOBCoBorrower != string.Empty)
        {
            if (DateTime.TryParse(sDOBCoBorrower, out DOBCoBorrower) == false)
            {
                PageCommon.AlertMsg(this, "Date of Birth is not a valid date format.");
                return false;
            }
            if (DOBCoBorrower > DT)
            {
                PageCommon.AlertMsg(this, "Date of Birth should be 18 years old.");
                return false;
            }

        }
        string sSSNCoBorrower = this.CoBorrowerInfo_Contact == null ? "" : this.CoBorrowerInfo_Contact.Rows[0]["SSN"].ToString();
        string sFICOScoreCoBorrower = this.CoBorrowerInfo_Contact == null ? "" : this.CoBorrowerInfo_Contact.Rows[0]["Experian"].ToString();

        #endregion

        #region Mailing Address

        string sMailingAddress = "Both";
        string sMailingStreetAddress1 = this.BorrowerInfo_Contact.Rows[0]["MailingAddr"].ToString();
        string sMailingStreetAddress2 = "";
        string sMailingCity = this.BorrowerInfo_Contact.Rows[0]["MailingCity"].ToString();
        string sMailingState = this.BorrowerInfo_Contact.Rows[0]["MailingState"].ToString();
        string sMailingZip = this.BorrowerInfo_Contact.Rows[0]["MailingZip"].ToString();

        string sLeadSource = this.BorrowerInfo_Prospect.Rows[0]["LeadSource"].ToString();
        string sReferralID = this.BorrowerInfo_Prospect.Rows[0]["Referral"].ToString();

        #endregion

        #region Property Address

        string sPropertyStreetAddress1 = this.CurrentLoanInfo.Rows[0]["PropertyAddr"].ToString();
        string sPropertyStreetAddress2 = "";
        string sPropertyCity = this.CurrentLoanInfo.Rows[0]["PropertyCity"].ToString();
        string sPropertyState = this.CurrentLoanInfo.Rows[0]["PropertyState"].ToString();
        string sPropertyZip = this.CurrentLoanInfo.Rows[0]["PropertyZip"].ToString();

        string sPropertyValue = this.CurrentLoanInfo.Rows[0]["SalesPrice"] == DBNull.Value ? string.Empty : Convert.ToDecimal(this.CurrentLoanInfo.Rows[0]["SalesPrice"]).ToString("n0");

        #endregion

        #region Loan Info

        #region Current Loan

        string sArchiveLoan = this.ddlArchiveLoan.SelectedValue;

        string sPurpose = this.ddlPurpose.SelectedValue;
        string sLoanType = this.ddlType.SelectedValue;
        string sProgram = this.ddlProgram.SelectedValue;

        string sAmount = this.txtAmount.Text.Trim();
        string sRate = this.txtRate.Text.Trim();
        string sPMI = this.txtPMI.Text.Trim();
        string sPMITax = this.txtPMITax.Text.Trim();

        string sTerm = this.txtTerm.Text.Trim();

        string sStartYear = this.ddlStartYear.SelectedValue;
        string sStartMonth = this.ddlStartMonth.SelectedValue;
        string sStartDate = this.GetStartDateStr(sStartYear, sStartMonth);

        bool b2nd = this.chk2nd.Checked;
        string s2ndAmount = this.txt2ndAmount.Text.Trim();

        string sRanking = this.CurrentLoanInfo.Rows[0]["Ranking"] == DBNull.Value ? "Hot" : this.CurrentLoanInfo.Rows[0]["Ranking"].ToString();
        #endregion

        #region New Loan

        string sPurposeNewLoan = this.ddlPurposeNewLoan.SelectedValue;
        string sLoanTypeNewLoan = this.ddlTypeNewLoan.SelectedValue;
        string sProgramNewLoan = this.ddlProgramNewLoan.SelectedValue;

        string sAmountNewLoan = this.txtAmountNewLoan.Text.Trim();
        string sRateNewLoan = this.txtRateNewLoan.Text.Trim();
        string sPMINewLoan = this.txtPMINewLoan.Text.Trim();
        string sPMITaxNewLoan = this.txtPMITaxNewLoan.Text.Trim();

        string sTermNewLoan = this.txtTermNewLoan.Text.Trim();
        string sStartYearNewLoan = this.ddlStartYearNewLoan.SelectedValue;
        string sStartMonthNewLoan = this.ddlStartMonthNewLoan.SelectedValue;

        bool bSubordinate = this.chkSubordinate.Checked;


        #endregion

        #endregion

        #endregion

        if (this.sActionMode == "Edit_HasLoanId_NoContactId" || this.sActionMode == "Edit_HasContactId_HasLoan")
        {
            #region update borrower

            try
            {
                int iBorrowerID = Convert.ToInt32(this.BorrowerInfo_Contact.Rows[0]["ContactId"]);

                if (sMailingAddress == "Both" || sMailingAddress == "Borrower")
                {
                    this.ProspectMgr.UpdateBorrower(iBorrowerID, sFirstName, sLastName, sEmail,
                        sCellPhone, sHomePhone, sWorkPhone, sDOB, sSSN, sDependants, sCreditRanking, sFICOScore,
                        sMailingStreetAddress1, sMailingStreetAddress2, sMailingCity, sMailingState, sMailingZip,
                        sLeadSource, sReferralID);
                }
                else
                {
                    this.ProspectMgr.UpdateBorrower(iBorrowerID, sFirstName, sLastName, sEmail,
                        sCellPhone, sHomePhone, sWorkPhone, sDOB, sSSN, sDependants, sCreditRanking, sFICOScore);
                }
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to save loan info.");
                return false;
            }

            #endregion

            #region update co-borrower

            if (this.CoBorrowerInfo_Contact == null)    // create co-borrower
            {

            }
            else
            {
                #region update co-borrower

                int iCoBorrowerID = Convert.ToInt32(this.CoBorrowerInfo_Contact.Rows[0]["ContactId"]);

                if (sMailingAddress == "Both" || sMailingAddress == "Co-Borrower")
                {
                    this.ProspectMgr.UpdateCoBorrower(iCoBorrowerID, sFirstName, sLastName, sEmail,
                        sCellPhone, sHomePhone, sWorkPhone, sDOB, sSSN, sFICOScore,
                        sMailingStreetAddress1, sMailingStreetAddress2, sMailingCity, sMailingState, sMailingZip,
                        sLeadSource, sReferralID);
                }
                else
                {
                    this.ProspectMgr.UpdateCoBorrower(iCoBorrowerID, sFirstName, sLastName, sEmail,
                        sCellPhone, sHomePhone, sWorkPhone, sDOB, sSSN, sFICOScore);
                }

                #endregion
            }

            #endregion

            #region update loan

            int iCurrentLoanId = Convert.ToInt32(this.CurrentLoanInfo.Rows[0]["FileId"]);

            try
            {

                this.LoansMgr.UpldateLoanInfo(iCurrentLoanId, sHousingStatus, sRentAmount, sPropertyStreetAddress1, sPropertyStreetAddress2,
                    sPropertyCity, sPropertyState, sPropertyZip, sPropertyValue,
                    sPurpose, sLoanType, sProgram, sAmount, sRate, sPMI, sPMITax, sTerm, sStartDate, b2nd, s2ndAmount, sRanking);
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to save loan info.");
                return false;
            }

            #endregion

            #region 如果输入New Loan→Create

            bool bCreateNewLoan = false;
            if (sPurposeNewLoan != "" || sLoanTypeNewLoan != "" || sProgramNewLoan != "" || sAmountNewLoan != ""
                || sRateNewLoan != "" || sPMINewLoan != "" || sPMITaxNewLoan != "" || sTermNewLoan != ""
                || sStartYearNewLoan != "" || sStartMonthNewLoan != "")
            {
                bCreateNewLoan = true;
            }

            if (bCreateNewLoan == true)
            {

                #region create contact/prospect

                #region create Borrower

                int iNewBorrowerID = 0;

                try
                {
                    if (sMailingAddress == "Both" || sMailingAddress == "Borrower")
                    {
                        iNewBorrowerID = this.CreateContactAndProspect(sFirstName, sLastName, sEmail, sCellPhone, sHomePhone, sWorkPhone, sDOB, sSSN, sDependants, sCreditRanking, sFICOScore, sMailingStreetAddress1, sMailingStreetAddress2, sMailingCity, sMailingState, sMailingZip, sLeadSource, sReferralID);
                    }
                    else
                    {
                        iNewBorrowerID = this.CreateContactAndProspect(sFirstName, sLastName, sEmail, sCellPhone, sHomePhone, sWorkPhone, sDOB, sSSN, sDependants, sCreditRanking, sFICOScore, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, sLeadSource, sReferralID);
                    }
                }
                catch (Exception ex)
                {
                    PageCommon.AlertMsg(this, "Failed to save contact and prospect for borrower.");
                    return false;
                }

                #endregion

                #region create Co-Borrower

                int iNewCoBorrowerID = 0;

                if (iNewBorrowerID > 0 && sFirstNameCoBorrower != string.Empty)
                {
                    try
                    {
                        if (sMailingAddress == "Both" || sMailingAddress == "Co-Borrower")
                        {
                            iNewCoBorrowerID = this.CreateContactAndProspect(sFirstNameCoBorrower, sLastNameCoBorrower, sEmailCoBorrower, sCellPhoneCoBorrower, sHomePhoneCoBorrower, sWorkPhoneCoBorrower, sDOBCoBorrower, sSSNCoBorrower, string.Empty, string.Empty, sFICOScoreCoBorrower, sMailingStreetAddress1, sMailingStreetAddress2, sMailingCity, sMailingState, sMailingZip, sLeadSource, sReferralID);
                        }
                        else
                        {
                            iNewCoBorrowerID = this.CreateContactAndProspect(sFirstNameCoBorrower, sLastNameCoBorrower, sEmailCoBorrower, sCellPhoneCoBorrower, sHomePhoneCoBorrower, sWorkPhoneCoBorrower, sDOBCoBorrower, sSSNCoBorrower, string.Empty, string.Empty, sFICOScoreCoBorrower, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, sLeadSource, sReferralID);
                        }
                    }
                    catch (Exception ex)
                    {
                        PageCommon.AlertMsg(this, "Failed to save contact and prospect for co-borrower.");
                        return false;
                    }
                }

                #endregion

                #endregion

                #region create loan

                int iFileID = 0;
                try
                {
                    iFileID = this.CreateLoan(iNewBorrowerID, iNewCoBorrowerID, sHousingStatus, sRentAmount, sPropertyStreetAddress1, sPropertyStreetAddress2, sPropertyCity, sPropertyState, sPropertyZip, sPropertyValue,
                        sPurposeNewLoan, sLoanTypeNewLoan, sProgramNewLoan, sAmountNewLoan, sRateNewLoan, sPMINewLoan, sPMITaxNewLoan, sTermNewLoan, sStartYearNewLoan, sStartMonthNewLoan, bSubordinate, b2nd, s2ndAmount, sRanking);
                }
                catch (Exception ex)
                {
                    PageCommon.AlertMsg(this, "Failed to save loan info.");
                    return false;
                }

                #endregion

            }

            #endregion

            iNewFileId = iCurrentLoanId;
        }

        return true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int iFileId = 0;
        bool bSuccess = this.SaveLead(out iFileId);

        // success
        if (bSuccess == true)
        {
            PageCommon.WriteJsEnd(this, "Save successfully.", "window.location.href='LeadLoanInfoTab.aspx?FromPage=&FileID=" + iFileId + "&FileIDs=" + iFileId + "'");
        }
    }
}

