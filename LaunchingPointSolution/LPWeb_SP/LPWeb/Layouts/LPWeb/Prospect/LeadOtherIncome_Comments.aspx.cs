using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.BLL;
using System.Data;
using System.Web.UI.WebControls;
using LPWeb.Common;

//注意 本页 代码 是在LeadEdit.aspx基础上修改而来
public partial class LeadOtherIncome_Comments : BasePage
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

        if (this.Request.QueryString["LoanId"] != null && this.Request.QueryString["ContactId"] == null)     // edit lead
        {
            #region 校验LoanId

            string sLoanId = this.Request.QueryString["LoanId"];
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
        else if (this.Request.QueryString["ContactId"] != null && this.Request.QueryString["LoanId"] == null)
        {
            #region 校验ContactId

            string sContactId = this.Request.QueryString["ContactId"];
            bool bValid = PageCommon.IsID(sContactId);
            if (bValid == false)
            {
                PageCommon.WriteJsEnd(this, "Invalid contact id.", "window.location.href='LeadCreate.aspx'");
                return;
            }
            this.iContactId = Convert.ToInt32(sContactId);

            #endregion

            #region 加载ContactInfo

            // 加载ContactInfo
            DataTable ContactInfo = this.ContactMgr.GetContactInfo(this.iContactId);
            if (ContactInfo.Rows.Count == 0)
            {
                PageCommon.WriteJsEnd(this, "Invalid contact id.", "window.location.href='LeadCreate.aspx'");
                return;
            }

            #endregion

            // ContactId -> 加载Current Loan Info
            DataTable ContactLoanInfo = this.GetContactLoanInfo(this.iContactId);

            if (ContactLoanInfo.Rows.Count > 0)
            {
                // Current Loan
                this.CurrentLoanInfo = ContactLoanInfo.Copy();

                // 加载Borrower/Co-Boorwer Info
                int iFileId = Convert.ToInt32(this.CurrentLoanInfo.Rows[0]["FileId"]);
                this.InitBorrowerCoBorrowerInfo(iFileId);

                this.sActionMode = "Edit_HasContactId_HasLoan";

            }
            else    // find no prospect loan -> create lead
            {
                // go to create lead
                this.Response.Redirect("LeadCreate.aspx?ContactId=" + this.iContactId);
            }
        }
        else
        {
            PageCommon.WriteJsEnd(this, "Invalid query string arrange.", "window.location.href='LeadCreate.aspx'");
            return;
        }

        #endregion

        if (this.IsPostBack == false)
        {
            #region 加载State列表

            USStates.Init(this.ddlMailingState);
            USStates.Init(this.ddlPropertyState);

            #endregion

            #region 加载Lead Source列表

            Company_Lead_Sources LeadSourceManager = new Company_Lead_Sources();
            DataTable LeadSourceList = LeadSourceManager.GetList("1=1 order by LeadSource").Tables[0];

            this.ddlLeadSource.Items.Add(new ListItem("-- select --", ""));

            foreach (DataRow LeadSourceRow in LeadSourceList.Rows)
            {
                this.ddlLeadSource.Items.Add(LeadSourceRow["LeadSource"].ToString());
            }

            #endregion

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

            #region 加载Work Start Date→Year

            this.ddlWorkStartYear.Items.Add(new ListItem("year", ""));
            this.ddlWorkEndYear.Items.Add(new ListItem("year", ""));

            for (int i = DateTime.Now.Year; i > 1950; i--)
            {
                int iNextYear = i;

                this.ddlWorkStartYear.Items.Add(iNextYear.ToString());
                this.ddlWorkEndYear.Items.Add(iNextYear.ToString());
            }

            #endregion

            if (this.sActionMode == "Edit_HasLoanId_NoContactId" || this.sActionMode == "Edit_HasContactId_HasLoan")
            {
                #region 绑定信息

                #region Borrower

                this.BindingData_Borrower(this.CurrentLoanInfo);

                #endregion

                #region CoBorrower

                if (this.CoBorrowerInfo_Contact != null && this.CoBorrowerInfo_Contact.Rows.Count > 0)
                {
                    this.txtFirstNameCoBorrower.Text = this.CoBorrowerInfo_Contact.Rows[0]["FirstName"].ToString();
                    this.txtLastNameCoBorrower.Text = this.CoBorrowerInfo_Contact.Rows[0]["LastName"].ToString();
                    this.txtEmailCoBorrower.Text = this.CoBorrowerInfo_Contact.Rows[0]["Email"].ToString();
                    this.txtCellPhoneCoBorrower.Text = this.CoBorrowerInfo_Contact.Rows[0]["CellPhone"].ToString();
                    this.txtHomePhoneCoBorrower.Text = this.CoBorrowerInfo_Contact.Rows[0]["HomePhone"].ToString();
                    this.txtWorkPhoneCoBorrower.Text = this.CoBorrowerInfo_Contact.Rows[0]["BusinessPhone"].ToString();
                    this.txtBirthdayCoBorrower.Text = this.CoBorrowerInfo_Contact.Rows[0]["DOB"] == DBNull.Value ? string.Empty : Convert.ToDateTime(this.CoBorrowerInfo_Contact.Rows[0]["DOB"]).ToString("MM/dd/yyyy");
                    this.txtSSNCoBorrower.Text = this.CoBorrowerInfo_Contact.Rows[0]["SSN"].ToString();

                    this.txtFICOScoreCoBorrower.Text = this.CoBorrowerInfo_Contact.Rows[0]["Experian"].ToString();
                }

                #endregion

                #region General Info

                if (this.BorrowerInfo_Contact != null && this.BorrowerInfo_Contact.Rows.Count > 0)
                {
                    #region

                    this.txtMailingStreetAddress1.Text = this.BorrowerInfo_Contact.Rows[0]["MailingAddr"].ToString();
                    //this.txtMailingStreetAddress2.Text = this.BorrowerInfo_Contact.Rows[0]["MailingAddr"].ToString();
                    this.txtMailingCity.Text = this.BorrowerInfo_Contact.Rows[0]["MailingCity"].ToString();
                    this.ddlMailingState.SelectedValue = this.BorrowerInfo_Contact.Rows[0]["MailingState"].ToString();
                    this.txtMailingZip.Text = this.BorrowerInfo_Contact.Rows[0]["MailingZip"].ToString();

                    #endregion
                }

                if (this.BorrowerInfo_Prospect != null && this.BorrowerInfo_Prospect.Rows.Count > 0)
                {
                    #region

                    this.ddlLeadSource.SelectedValue = this.BorrowerInfo_Prospect.Rows[0]["LeadSource"].ToString();

                    string sReferralID = this.BorrowerInfo_Prospect.Rows[0]["Referral"].ToString();
                    if (sReferralID != string.Empty)
                    {
                        DataTable ReferralInfo = this.ContactMgr.GetContactInfo(Convert.ToInt32(sReferralID));
                        if (ReferralInfo.Rows.Count > 0)
                        {
                            string sReferralFirstName = ReferralInfo.Rows[0]["FirstName"].ToString();
                            string sReferralLastName = ReferralInfo.Rows[0]["LastName"].ToString();

                            this.txtReferralSource.Text = sReferralLastName + ", " + sReferralFirstName;
                            this.hdnReferralID.Value = sReferralID;
                        }
                    }

                    #endregion
                }

                #region Property Address

                this.txtPropertyStreetAddress1.Text = this.CurrentLoanInfo.Rows[0]["PropertyAddr"].ToString();
                //this.txtPropertyStreetAddress2.Text = this.CurrentLoanInfo.Rows[0]["PropertyAddr"].ToString();
                this.txtPropertyCity.Text = this.CurrentLoanInfo.Rows[0]["PropertyCity"].ToString();
                this.ddlPropertyState.SelectedValue = this.CurrentLoanInfo.Rows[0]["PropertyState"].ToString();
                this.txtPropertyZip.Text = this.CurrentLoanInfo.Rows[0]["PropertyZip"].ToString();
                this.txtPropertyValue.Text = this.CurrentLoanInfo.Rows[0]["SalesPrice"] == DBNull.Value ? string.Empty : Convert.ToDecimal(this.CurrentLoanInfo.Rows[0]["SalesPrice"]).ToString("n0");

                #endregion

                this.lbRanking.Text = this.CurrentLoanInfo.Rows[0]["Ranking"].ToString();
                this.imgRanking.ImageUrl = String.Format("../images/prospect/{0}.gif", this.lbRanking.Text);
                this.lbStatus.Text = this.CurrentLoanInfo.Rows[0]["ProspectLoanStatus"].ToString();
                this.ddlRanking.SelectedValue = this.CurrentLoanInfo.Rows[0]["Ranking"].ToString();
                #endregion

                #region Loan Info

                this.BindingData_LoanInfo(this.CurrentLoanInfo);

                #endregion

                #region Income and Employment

                this.BindingData_IncomeAndEmployment();

                #endregion

                #endregion
            }
        }
    }

    #region create months

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sFirstName"></param>
    /// <param name="sLastName"></param>
    /// <param name="sEmail"></param>
    /// <param name="sCellPhone"></param>
    /// <param name="sHomePhone"></param>
    /// <param name="sWorkPhone"></param>
    /// <param name="sDOB"></param>
    /// <param name="sSSN"></param>
    /// <param name="sDependants"></param>
    /// <param name="sCreditRanking"></param>
    /// <param name="sFICO"></param>
    /// <param name="sMailingStreetAddress1"></param>
    /// <param name="sMailingStreetAddress2"></param>
    /// <param name="sMailingCity"></param>
    /// <param name="sMailingState"></param>
    /// <param name="sMailingZip"></param>
    /// <param name="sLeadSource"></param>
    /// <param name="sReferralID"></param>
    /// <returns></returns>
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
        ContactsModel.NickName = txtFirstName.Text.Trim();
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sStartYear"></param>
    /// <param name="sStartMonth"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sStartYear"></param>
    /// <param name="sStartMonth"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iBorrowerID"></param>
    /// <param name="iCoBorrowerID"></param>
    /// <param name="sHousingStatus"></param>
    /// <param name="sRentAmount"></param>
    /// <param name="sPropertyStreetAddress1"></param>
    /// <param name="sPropertyStreetAddress2"></param>
    /// <param name="sPropertyCity"></param>
    /// <param name="sPropertyState"></param>
    /// <param name="sPropertyZip"></param>
    /// <param name="sPropertyValue"></param>
    /// <param name="sPurpose"></param>
    /// <param name="sLoanType"></param>
    /// <param name="sProgram"></param>
    /// <param name="sAmount"></param>
    /// <param name="sRate"></param>
    /// <param name="sPMI"></param>
    /// <param name="sPMITax"></param>
    /// <param name="sTerm"></param>
    /// <param name="sStartYear"></param>
    /// <param name="sStartMonth"></param>
    /// <param name="bSubordinate"></param>
    /// <returns></returns>
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
        LoanDetailsModel.Ranking = sRanking;//"Hot";
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iFileId"></param>
    /// <param name="sCloseDate"></param>
    /// <param name="iUserID"></param>
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

    private void BindingData_Borrower(DataTable LoanInfoData)
    {
        #region Borrower

        if (this.BorrowerInfo_Contact != null && this.BorrowerInfo_Contact.Rows.Count > 0)
        {
            #region

            this.txtFirstName.Text = this.BorrowerInfo_Contact.Rows[0]["FirstName"].ToString();
            this.txtLastName.Text = this.BorrowerInfo_Contact.Rows[0]["LastName"].ToString();
            this.txtEmail.Text = this.BorrowerInfo_Contact.Rows[0]["Email"].ToString();
            this.txtCellPhone.Text = this.BorrowerInfo_Contact.Rows[0]["CellPhone"].ToString();
            this.txtHomePhone.Text = this.BorrowerInfo_Contact.Rows[0]["HomePhone"].ToString();
            this.txtWorkPhone.Text = this.BorrowerInfo_Contact.Rows[0]["BusinessPhone"].ToString();
            this.txtBirthday.Text = this.BorrowerInfo_Contact.Rows[0]["DOB"] == DBNull.Value ? string.Empty : Convert.ToDateTime(this.BorrowerInfo_Contact.Rows[0]["DOB"]).ToString("MM/dd/yyyy");
            this.txtSSN.Text = this.BorrowerInfo_Contact.Rows[0]["SSN"].ToString();

            if (LoanInfoData != null)
            {
                this.ddlHousingStatus.SelectedValue = LoanInfoData.Rows[0]["HousingStatus"].ToString();
                this.txtRentAmount.Text = LoanInfoData.Rows[0]["RentAmount"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanInfoData.Rows[0]["RentAmount"]).ToString("n0");
            }

            this.txtFICOScore.Text = this.BorrowerInfo_Contact.Rows[0]["Experian"].ToString();

            #endregion
        }

        if (this.BorrowerInfo_Prospect != null && this.BorrowerInfo_Prospect.Rows.Count > 0)
        {
            #region

            #region Dependants

            string sDependants = this.BorrowerInfo_Prospect.Rows[0]["Dependents"].ToString();

            if (sDependants == "True")
            {
                this.ddlDependants.SelectedValue = "Yes";
            }
            else if (sDependants == "False")
            {
                this.ddlDependants.SelectedValue = "No";
            }

            #endregion

            this.ddlCreditRanking.SelectedValue = this.BorrowerInfo_Prospect.Rows[0]["CreditRanking"].ToString();

            #endregion
        }

        #endregion
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

    private void BindingData_IncomeAndEmployment()
    {
        if (this.BorrowerInfo_Employment != null)
        {
            this.txtCompanyName.Text = this.BorrowerInfo_Employment.Rows[0]["CompanyName"].ToString();

            #region Self Employed

            string sSelfEmployed = this.BorrowerInfo_Employment.Rows[0]["SelfEmployed"].ToString();

            if (sSelfEmployed == "True")
            {
                this.ddlSelfEmployed.SelectedValue = "Yes";
            }
            else if (sSelfEmployed == "False")
            {
                this.ddlSelfEmployed.SelectedValue = "No";
            }

            #endregion

            this.txtPosition.Text = this.BorrowerInfo_Employment.Rows[0]["Position"].ToString();
            this.txtProfession.Text = this.BorrowerInfo_Employment.Rows[0]["BusinessType"].ToString();

            this.txtYearsinFiled.Text = this.BorrowerInfo_Employment.Rows[0]["YearsOnWork"].ToString();

            this.ddlWorkStartMonth.SelectedValue = this.BorrowerInfo_Employment.Rows[0]["StartMonth"].ToString();
            this.ddlWorkStartYear.SelectedValue = this.BorrowerInfo_Employment.Rows[0]["StartYear"].ToString();
            this.ddlWorkEndMonth.SelectedValue = this.BorrowerInfo_Employment.Rows[0]["EndMonth"].ToString();
            this.ddlWorkEndYear.SelectedValue = this.BorrowerInfo_Employment.Rows[0]["EndYear"].ToString();
        }

        if (this.BorrowerInfo_Income != null)
        {
            this.txtMonthlySalary.Text = BorrowerInfo_Income.Rows[0]["Salary"] == DBNull.Value ? string.Empty : Convert.ToDecimal(BorrowerInfo_Income.Rows[0]["Salary"]).ToString("n0");
            this.txtOtherMonthlyIncome.Text = BorrowerInfo_Income.Rows[0]["Other"] == DBNull.Value ? string.Empty : Convert.ToDecimal(BorrowerInfo_Income.Rows[0]["Other"]).ToString("n0");
        }

        if (this.BorrowerInfo_Assets != null)
        {
            this.txtLiquidAssets.Text = BorrowerInfo_Assets.Rows[0]["Amount"] == DBNull.Value ? string.Empty : Convert.ToDecimal(BorrowerInfo_Assets.Rows[0]["Amount"]).ToString("n0");
        }
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

    private bool SaveLead(out int iReturnFileId)
    {
        iReturnFileId = 0;

        #region get user input

        #region Borrower Info

        string sFirstName = this.txtFirstName.Text.Trim();
        string sLastName = this.txtLastName.Text.Trim();
        string sEmail = this.txtEmail.Text.Trim();
        string sCellPhone = this.txtCellPhone.Text.Trim();
        string sHomePhone = this.txtHomePhone.Text.Trim();
        string sWorkPhone = this.txtWorkPhone.Text.Trim();
        string sDOB = this.txtBirthday.Text.Trim();
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
        string sSSN = this.txtSSN.Text.Trim();
        string sDependants = this.ddlDependants.SelectedValue;
        string sCreditRanking = this.ddlCreditRanking.SelectedValue;
        string sHousingStatus = this.ddlHousingStatus.SelectedValue;
        string sRentAmount = this.txtRentAmount.Text.Trim();
        string sFICOScore = this.txtFICOScore.Text.Trim();

        #endregion

        #region Co-Borrower Info

        string sFirstNameCoBorrower = this.txtFirstNameCoBorrower.Text.Trim();
        string sLastNameCoBorrower = this.txtLastNameCoBorrower.Text.Trim();
        string sEmailCoBorrower = this.txtEmailCoBorrower.Text.Trim();
        string sCellPhoneCoBorrower = this.txtCellPhoneCoBorrower.Text.Trim();
        string sHomePhoneCoBorrower = this.txtHomePhoneCoBorrower.Text.Trim();
        string sWorkPhoneCoBorrower = this.txtWorkPhoneCoBorrower.Text.Trim();
        string sDOBCoBorrower = this.txtBirthdayCoBorrower.Text.Trim();
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
        string sSSNCoBorrower = this.txtSSNCoBorrower.Text.Trim();
        string sFICOScoreCoBorrower = this.txtFICOScoreCoBorrower.Text.Trim();

        #region 如果输入Co-Borrower，校验必输字段

        if (sFirstNameCoBorrower != string.Empty || sLastNameCoBorrower != string.Empty)
        {
            if (sFirstNameCoBorrower == string.Empty)
            {
                PageCommon.AlertMsg(this, "Please enter First Name, Last Name and Email of Co-Borrower.");
                return false;
            }

            if (sLastNameCoBorrower == string.Empty)
            {
                PageCommon.AlertMsg(this, "Please enter First Name, Last Name and Email of Co-Borrower.");
                return false;
            }

            if (sEmailCoBorrower == string.Empty && sCellPhoneCoBorrower == string.Empty
                 && sHomePhoneCoBorrower == string.Empty && sWorkPhoneCoBorrower == string.Empty)
            {
                PageCommon.AlertMsg(this, "Please enter one of Email, Cell Phone, Home Phone or Work Phone of Co-Borrower.");
                return false;
            }
        }

        #endregion

        #endregion

        #region General Info

        #region Mailing Address

        string sMailingAddress = this.ddlMailingAddress.SelectedValue;
        string sMailingStreetAddress1 = this.txtMailingStreetAddress1.Text.Trim();
        string sMailingStreetAddress2 = this.txtMailingStreetAddress2.Text.Trim();
        string sMailingCity = this.txtMailingCity.Text.Trim();
        string sMailingState = this.ddlMailingState.SelectedValue;
        string sMailingZip = this.txtMailingZip.Text.Trim();

        string sLeadSource = this.ddlLeadSource.SelectedValue;
        string sReferralID = this.hdnReferralID.Value;

        #endregion

        #region Property Address

        string sPropertyStreetAddress1 = this.txtPropertyStreetAddress1.Text.Trim();
        string sPropertyStreetAddress2 = this.txtPropertyStreetAddress2.Text.Trim();
        string sPropertyCity = this.txtPropertyCity.Text.Trim();
        string sPropertyState = this.ddlPropertyState.SelectedValue;
        string sPropertyZip = this.txtPropertyZip.Text.Trim();

        string sPropertyValue = this.txtPropertyValue.Text.Trim();

        #endregion

        string sRanking = ddlRanking.SelectedValue; //"Hot";
        //string sStatus = "Active";

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

        bool bCreateNewLoan = false;
        if (sPurposeNewLoan != "" || sLoanTypeNewLoan != "" || sProgramNewLoan != "" || sAmountNewLoan != ""
            || sRateNewLoan != "" || sPMINewLoan != "" || sPMITaxNewLoan != "" || sTermNewLoan != ""
            || sStartYearNewLoan != "" || sStartMonthNewLoan != "")
        {
            bCreateNewLoan = true;
        }

        #endregion

        #region Income / Employment

        string sCompanyName = this.txtCompanyName.Text.Trim();
        string sSelfEmployed = this.ddlSelfEmployed.SelectedValue;
        bool bSelfEmployed = false;
        if (sSelfEmployed == "Yes")
        {
            bSelfEmployed = true;
        }
        string sPosition = this.txtPosition.Text.Trim();
        string sMonthlySalary = this.txtMonthlySalary.Text.Trim();
        string sProfession = this.txtProfession.Text.Trim();
        string sYearsInField = this.txtYearsinFiled.Text.Trim();

        string sWorkStartYear = this.ddlWorkStartYear.SelectedValue;
        string sWorkStartMonth = this.ddlWorkStartMonth.SelectedValue;
        string sWorkEndYear = this.ddlWorkEndYear.SelectedValue;
        string sWorkEndMonth = this.ddlWorkEndMonth.SelectedValue;

        #endregion

        #region Other Incoe and payment

        string sOtherMonthlyIncome = this.txtOtherMonthlyIncome.Text.Trim();
        string sLiquidAssets = this.txtLiquidAssets.Text.Trim();
        string sComments = this.txtComments.Text.Trim();

        #endregion

        #endregion

        #region Check numeric

        if (!string.IsNullOrEmpty(sMonthlySalary))
        {
            try
            {
                var num = Convert.ToDecimal(sMonthlySalary);

                if (num > 999999999)
                {
                    PageCommon.AlertMsg(this, "Failed to save, MonthlySalary Can not be greater than 999,999,999!");
                    return false;
                }
            }
            catch
            {
                PageCommon.AlertMsg(this, "Failed to save, MonthlySalary");
                return false;
            }
        }

        if (!string.IsNullOrEmpty(sOtherMonthlyIncome))
        {
            try
            {
                var num = Convert.ToDecimal(sOtherMonthlyIncome);

                if (num > 999999999)
                {
                    PageCommon.AlertMsg(this, "Failed to save, Other Monthly Income Can not be greater than 999,999,999!");
                    return false;
                }
            }
            catch
            {
                PageCommon.AlertMsg(this, "Failed to save, Other Monthly Income");
                return false;
            }
        }

        if (!string.IsNullOrEmpty(sLiquidAssets))
        {
            try
            {
                var num = Convert.ToDecimal(sLiquidAssets);

                if (num > 999999999)
                {
                    PageCommon.AlertMsg(this, "Failed to save, Liquid Assets Can not be greater than 999,999,999!");
                    return false;
                }
            }
            catch
            {
                PageCommon.AlertMsg(this, "Failed to save, Liquid Assets");
                return false;
            }
        }



        #endregion

        if (this.sActionMode == "Edit_HasLoanId_NoContactId" || this.sActionMode == "Edit_HasContactId_HasLoan")
        {
            #region update

            #region update borrower

            int iBorrowerID = Convert.ToInt32(this.BorrowerInfo_Contact.Rows[0]["ContactId"]);
            try
            {
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
                    this.ProspectMgr.UpdateCoBorrower(iCoBorrowerID, sFirstNameCoBorrower, sLastNameCoBorrower, sEmailCoBorrower,
                        sCellPhoneCoBorrower, sHomePhoneCoBorrower, sWorkPhoneCoBorrower, sDOBCoBorrower, sSSNCoBorrower, sFICOScoreCoBorrower,
                        sMailingStreetAddress1, sMailingStreetAddress2, sMailingCity, sMailingState, sMailingZip,
                        sLeadSource, sReferralID);
                }
                else
                {
                    this.ProspectMgr.UpdateCoBorrower(iCoBorrowerID, sFirstNameCoBorrower, sLastNameCoBorrower, sEmailCoBorrower,
                        sCellPhoneCoBorrower, sHomePhoneCoBorrower, sWorkPhoneCoBorrower, sDOBCoBorrower, sSSNCoBorrower, sFICOScoreCoBorrower);
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

            #region create/update Income and Employment

            if (this.BorrowerInfo_Employment == null)
            {
                // create
                this.CreateProspectEmployment(iBorrowerID, sCompanyName, bSelfEmployed, sPosition, sProfession,
                    sYearsInField, sWorkStartYear, sWorkStartMonth, sWorkEndYear, sWorkEndMonth);
            }
            else
            {
                // update
                int iEmploymentID = Convert.ToInt32(this.BorrowerInfo_Employment.Rows[0]["EmplId"]);
                this.UpdateProspectEmployment(iEmploymentID, sCompanyName, bSelfEmployed, sPosition, sProfession,
                    sYearsInField, sWorkStartYear, sWorkStartMonth, sWorkEndYear, sWorkEndMonth);
            }

            if (this.BorrowerInfo_Income == null)
            {
                // create
                this.CreateProspectIncome(iBorrowerID, sMonthlySalary, sOtherMonthlyIncome);
            }
            else
            {
                // update
                int iProspectIncomeId = Convert.ToInt32(this.BorrowerInfo_Income.Rows[0]["ProspectIncomeId"]);
                this.UpdateProspectIncome(iProspectIncomeId, sMonthlySalary, sOtherMonthlyIncome);
            }

            if (this.BorrowerInfo_Assets == null)
            {
                // create
                this.CreateProspectAssets(iBorrowerID, sLiquidAssets);
            }
            else
            {
                // update
                int iProspectAssetId = Convert.ToInt32(this.BorrowerInfo_Assets.Rows[0]["ProspectAssetId"]);
                this.UpdateProspectAssets(iProspectAssetId, sLiquidAssets);
            }

            // create LoanNotes
            this.CreateLoanNotes(iCurrentLoanId, sComments);

            #endregion

            #endregion

            #region 如果输入New Loan→Create new loan

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

                if (sFirstNameCoBorrower != string.Empty)
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

                int iNewFileID = 0;
                try
                {
                    iNewFileID = this.CreateLoan(iNewBorrowerID, iNewCoBorrowerID, sHousingStatus, sRentAmount, sPropertyStreetAddress1, sPropertyStreetAddress2, sPropertyCity, sPropertyState, sPropertyZip, sPropertyValue,
                        sPurposeNewLoan, sLoanTypeNewLoan, sProgramNewLoan, sAmountNewLoan, sRateNewLoan, sPMINewLoan, sPMITaxNewLoan, sTermNewLoan, sStartYearNewLoan, sStartMonthNewLoan, bSubordinate, b2nd, s2ndAmount, sRanking);
                }
                catch (Exception ex)
                {
                    PageCommon.AlertMsg(this, "Failed to save loan info.");
                    return false;
                }

                #endregion

                #region create income and employment

                try
                {
                    this.CreateIncomeAndEmployment(iNewFileID, iNewBorrowerID, sCompanyName, bSelfEmployed, sPosition, sMonthlySalary, sProfession,
                        sYearsInField, sWorkStartYear, sWorkStartMonth, sWorkEndYear, sWorkEndMonth,
                        sOtherMonthlyIncome, sLiquidAssets, sComments);
                }
                catch (Exception ex)
                {
                    PageCommon.AlertMsg(this, "Failed to save income and employment.");
                    return false;
                }

                #endregion
            }

            #endregion

            iReturnFileId = iCurrentLoanId;
        }

        return true;
    }

    protected void btnSaveGoToDetail_Click(object sender, EventArgs e)
    {
        int iFileId = 0;
        bool bSuccess = this.SaveLead(out iFileId);

        // success
        if (bSuccess == true)
        {
            PageCommon.WriteJsEnd(this, "Save successfully.", "window.location.href=window.location.href");
        }
    }

    protected void btnCreateAnother_Click(object sender, EventArgs e)
    {
        int iFileId = 0;
        bool bSuccess = this.SaveLead(out iFileId);

        // success
        if (bSuccess == true)
        {
            PageCommon.WriteJsEnd(this, "Save successfully.", PageCommon.Js_RefreshSelf);
        }
    }

    public int iReturnFileId { get; set; }
}
