using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.BLL;
using System.Data;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.LP_Service;

public partial class Prospect_LeadCreate : BasePage
{
    int iContactId = 0;

    /// <summary>
    /// Create_NoContactId_NoLoanId
    /// Create_HasContactId_NoLoan
    /// </summary>
    string sActionMode = string.Empty;

    DataTable BorrowerInfo_Contact = null;
    DataTable BorrowerInfo_Prospect = null;

    DataTable BorrowerClosedLoanInfo = null;

    DataTable BorrowerInfo_Employment = null;
    DataTable BorrowerInfo_Income = null;
    DataTable BorrowerInfo_Assets = null;

    Loans LoansMgr = new Loans();
    Prospect ProspectMgr = new Prospect();
    Contacts ContactMgr = new Contacts();
    int iLoanOfficerID;
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        // if current user doesn't have Prospect Create or Modify privileges, don't allow it
        if ((CurrUser.userRole == null) || string.IsNullOrEmpty(CurrUser.userRole.Prospect) ||
            !(CurrUser.userRole.Prospect.ToUpper().Contains("A") || CurrUser.userRole.Prospect.ToUpper().Contains("B")))
        {
            Response.Redirect("../Unauthorize1.aspx");
            return;
        }

        if (this.Request.QueryString["LoanId"] == null && this.Request.QueryString["ContactId"] == null)     // create lead
        {

            if (!CurrUser.userRole.Prospect.ToUpper().Contains("A")) // add privilege
            {
                Response.Redirect("../Unauthorize1.aspx");
                return;
            }
            this.sActionMode = "Create_NoContactId_NoLoanId";    // 纯create
        }
        else if (this.Request.QueryString["ContactId"] != null && this.Request.QueryString["LoanId"] == null)
        {
            if (!CurrUser.userRole.Prospect.ToUpper().Contains("B")) // update privilege
            {
                Response.Redirect("../Unauthorize1.aspx");
                return;
            }
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

            if (ContactLoanInfo.Rows.Count == 0)    // find no prospect loan -> create lead
            {
                #region 加载Borrower Info

                // Borrower Contact Info
                this.BorrowerInfo_Contact = ContactInfo.Copy();
                
                // Borrower Prospect Info
                Prospect ProspectMgr = new Prospect();
                DataTable ProspectInfo = ProspectMgr.GetProspectInfo(this.iContactId);
                if (ProspectInfo.Rows.Count > 0)
                {
                    this.BorrowerInfo_Prospect = ProspectInfo.Copy();
                }

                #endregion

                #region 加载Borrower's Closed Loan Info

                DataTable ClosedLoanInfo = this.GetBorrowerClosedLoanInfo(this.iContactId);
                if (ClosedLoanInfo.Rows.Count > 0)
                {
                    this.BorrowerClosedLoanInfo = ClosedLoanInfo.Copy();
                }

                #endregion

                #region 加载 Income and Employment

                #region 加载Employment信息

                ProspectEmployment ProspectEmploymentMgr = new ProspectEmployment();
                DataTable EmploymentInfo = ProspectEmploymentMgr.GetList("1=1 and ContactId=" + this.iContactId + " order by EmplId desc").Tables[0];
                if (EmploymentInfo.Rows.Count > 0)
                {
                    this.BorrowerInfo_Employment = EmploymentInfo.Copy();
                }

                #endregion

                #region 加载Income信息

                ProspectIncome ProspectIncomeMgr = new ProspectIncome();
                DataTable IncomeInfo = ProspectIncomeMgr.GetList("1=1 and ContactId=" + this.iContactId + " order by ProspectIncomeId desc").Tables[0];
                if (IncomeInfo.Rows.Count > 0)
                {
                    this.BorrowerInfo_Income = IncomeInfo.Copy();
                }

                #endregion

                #region 加载Assets信息

                ProspectAssets ProspectAssetsMgr = new ProspectAssets();
                DataTable AssetsInfo = ProspectAssetsMgr.GetList("1=1 and ContactId=" + this.iContactId + " order by ProspectAssetId desc").Tables[0];
                if (AssetsInfo.Rows.Count > 0)
                {
                    this.BorrowerInfo_Assets = AssetsInfo.Copy();
                }

                #endregion

                #endregion

                this.sActionMode = "Create_HasContactId_NoLoan";
            }
            else // edit lead
            {
                // go to edit lead
                this.Response.Redirect("LeadEdit.aspx?ContactId=" + this.iContactId);
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

            this.ddlLeadSource.Items.Add(new ListItem("-- select --", "0"));

            foreach (DataRow LeadSourceRow in LeadSourceList.Rows)
            {
                this.ddlLeadSource.Items.Add(new ListItem(LeadSourceRow["LeadSource"].ToString(), LeadSourceRow["LeadSourceID"].ToString()));
            }
            // set default selected
            DataRow[] DefaultRowArray = LeadSourceList.Select("Default=1");
            if (DefaultRowArray.Length > 0)
            {
                string sLeadSource = DefaultRowArray[0]["LeadSourceID"].ToString();
                this.ddlLeadSource.SelectedValue = sLeadSource;
            }
            #endregion

            #region 加载 ddlLoanOfficer

            DataTable dtLoadOfficer = this.GetLoanOfficerList(CurrUser.iUserID);

            DataRow drNew = dtLoadOfficer.NewRow();
            //2014/1/16 CR072 Add the current user in the Loan Officer dropdown list
            if (dtLoadOfficer.Select("ID=" + CurrUser.iUserID.ToString()).Length < 1)
            {
                drNew["ID"] = CurrUser.iUserID;
                drNew["Name"] = CurrUser.sFullName;
                drNew["LastName"] = CurrUser.sLastName;
                drNew["FirstName"] = CurrUser.sFirstName;
                dtLoadOfficer.Rows.InsertAt(drNew, 0);
            }

            drNew = dtLoadOfficer.NewRow();
            drNew["ID"] = 0;
            drNew["Name"] = "Lead Routing Engine";
            dtLoadOfficer.Rows.InsertAt(drNew, 0);

            drNew = dtLoadOfficer.NewRow();
            drNew["ID"] = -1;
            drNew["Name"] = "Unassigned";
            dtLoadOfficer.Rows.InsertAt(drNew, 0);

            drNew = dtLoadOfficer.NewRow();
            drNew["ID"] = -2;
            drNew["Name"] = "- select -";
            dtLoadOfficer.Rows.InsertAt(drNew, 0); 

            ddlLoanOfficer.DataSource = dtLoadOfficer;
            ddlLoanOfficer.DataTextField = "Name";
            ddlLoanOfficer.DataValueField = "ID";
            //if (dtLoadOfficer.Select("ID=" + CurrUser.iUserID.ToString()).Length > 0)
            //{
            //    ddlLoanOfficer.SelectedValue = CurrUser.iUserID.ToString();
            //}
            //else
            //{
                ddlLoanOfficer.SelectedValue = "0";
            //}
            ddlLoanOfficer.DataBind();
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

            if (this.sActionMode == "Create_NoContactId_NoLoanId")
            {
                // load nothing
            }
            else if (this.sActionMode == "Create_HasContactId_NoLoan")
            {
                // 绑定Borrower信息
                this.BindingData_Borrower(this.BorrowerClosedLoanInfo);

                // 绑定closed loan info
                if (this.BorrowerClosedLoanInfo != null)
                {
                    this.BindingData_LoanInfo(this.BorrowerClosedLoanInfo);
                }

                // 绑定Income and Employment信息
                this.BindingData_IncomeAndEmployment();
            }
        }
    }

    public DataTable GetLoanOfficerList()
    {
        string sSql0 = string.Empty;
        if (this.CurrUser.sRoleName == "Executive")
        {
            sSql0 = @"select distinct LoanOfficerId as ID, [Loan Officer] as Name from V_ProcessingPipelineInfo 
where isnull(LoanOfficerId,'')<>'' 
and BranchId in (select BranchId from dbo.lpfn_GetUserBranches_Executive(" + this.CurrUser.iUserID + @")) 
order by [Loan Officer]";
        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            sSql0 = @"select distinct LoanOfficerId as ID, [Loan Officer] as Name from V_ProcessingPipelineInfo 
where isnull(LoanOfficerId,'')<>'' 
and BranchId in (select BranchId from dbo.lpfn_GetUserBranches_Branch_Manager(" + this.CurrUser.iUserID + @")) 
order by [Loan Officer]";
        }
        else
        {
            sSql0 = @"select distinct LoanOfficerId as ID, [Loan Officer] as Name from V_ProcessingPipelineInfo 
where isnull(LoanOfficerId,'')<>'' 
and BranchId in (select BranchId from dbo.lpfn_GetUserBranches(" + this.CurrUser.iUserID + @")) 
order by [Loan Officer]";
        }

        DataTable LoanOfficerList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);

        return LoanOfficerList;
    }

    /// <summary>
    /// get loan officer list
    /// neo 2011-04-26
    /// </summary>
    /// <param name="iLoginUserID"></param>
    /// <returns></returns>
    private DataTable GetLoanOfficerList(int iLoginUserID)
    {
        string sSql = "select distinct LastName, FirstName, LastName +', '+FirstName as Name,UserId as ID from dbo.lpfn_GetAllLoanOfficer(" + iLoginUserID + ") order by  LastName, FirstName";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
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
        string sLeadSource, string sReferralID, string sPurpose, string sLoanType, string sPropertyState)
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

        string sLoanOfficerID = ddlLoanOfficer.SelectedValue;
        iLoanOfficerID = 0;
        if (sLoanOfficerID == "-1" || sLoanOfficerID == "-2")
        {
            //Nobody
        }
        else if (sLoanOfficerID == "0")
        {
            //Lead Routing Engine
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    //invoke the WCF API GetNextLoanOfficer
                    LR_GetNextLoanOfficerReq req = new LR_GetNextLoanOfficerReq();
                    req.LeadSource = ddlLeadSource.SelectedItem.Text;
                    req.Purpose = sPurpose;
                    req.LoanType = sLoanType;
                    req.State = sPropertyState;
                    req.ReqHdr = new ReqHdr();
                    req.ReqHdr.UserId = CurrUser.iUserID;
                    req.ReqHdr.SecurityToken = "SecurityToken";
                    LR_GetNextLoanOfficerResp response = client.LeadRouting_GetNextLoanofficer(req);
                    if (response.RespHdr.Successful)
                    {
                        iLoanOfficerID = response.NextLoanOfficerID;
                    }
                    else
                    {
                        Company_Lead_Sources LeadSourceMgr = new Company_Lead_Sources();
                        DataTable SelLeadSourceList = LeadSourceMgr.GetList("LeadSourceID='" + ddlLeadSource.SelectedValue + "'").Tables[0];
                        if (SelLeadSourceList != null && SelLeadSourceList.Rows.Count > 0)
                        {
                            iLoanOfficerID = SelLeadSourceList.Rows[0]["DefaultUserId"] == DBNull.Value ? 0 : Convert.ToInt32(SelLeadSourceList.Rows[0]["DefaultUserId"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Company_Lead_Sources LeadSourceMgr = new Company_Lead_Sources();
                DataTable SelLeadSourceList = LeadSourceMgr.GetList("LeadSourceID='" + ddlLeadSource.SelectedValue + "'").Tables[0];
                if (SelLeadSourceList != null && SelLeadSourceList.Rows.Count > 0)
                {
                    iLoanOfficerID = SelLeadSourceList.Rows[0]["DefaultUserId"] == DBNull.Value ? 0 : Convert.ToInt32(SelLeadSourceList.Rows[0]["DefaultUserId"]);
                }
            }
        }
        else
        {
            iLoanOfficerID = Convert.ToInt32(sLoanOfficerID);
        }

        ProspectModel.Loanofficer = iLoanOfficerID;
        ProspectModel.ReferenceCode = null;
        ProspectModel.PreferredContact = null;
     

        #endregion

        int iContactId = this.ProspectMgr.CreateContactAndProspectNoCheck(ContactsModel, ProspectModel);

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
        string sRate, string sPMI, string sPMITax, string sTerm, string sStartYear, string sStartMonth, bool bSubordinate, bool b2nd, string s2ndAmount
        ,string sRanking)
    {
        int iFolderId = 0;

        LPWeb.Model.LoanDetails LoanDetailsModel = new LPWeb.Model.LoanDetails();

        LoanDetailsModel.FileId = 0;
        LoanDetailsModel.FolderId = iFolderId;
        LoanDetailsModel.Status = "Prospect";
        LoanDetailsModel.ProspectLoanStatus = "Active";
        LoanDetailsModel.Ranking = sRanking; //"Hot"; //bug 200
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

        string sLoanOfficerID = ddlLoanOfficer.SelectedValue;
        iLoanOfficerID = 0;
        if (sLoanOfficerID == "-1" || sLoanOfficerID == "-2")
        {
            //Nobody
        }
        else if (sLoanOfficerID == "0")
        {
            //Lead Routing Engine
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    //invoke the WCF API GetNextLoanOfficer
                    LR_GetNextLoanOfficerReq req = new LR_GetNextLoanOfficerReq();
                    req.LeadSource = ddlLeadSource.SelectedItem.Text;
                    req.Purpose = sPurpose;
                    req.LoanType = sLoanType;
                    req.State = sPropertyState;
                    req.ReqHdr = new ReqHdr();
                    req.ReqHdr.UserId = CurrUser.iUserID;
                    req.ReqHdr.SecurityToken = "SecurityToken";
                    LR_GetNextLoanOfficerResp response = client.LeadRouting_GetNextLoanofficer(req);
                    if (response.RespHdr.Successful)
                    {
                        iLoanOfficerID = response.NextLoanOfficerID;
                    }
                    else
                    {
                        Company_Lead_Sources LeadSourceMgr = new Company_Lead_Sources();
                        DataTable SelLeadSourceList = LeadSourceMgr.GetList("LeadSourceID='" + ddlLeadSource.SelectedValue + "'").Tables[0];
                        if (SelLeadSourceList != null && SelLeadSourceList.Rows.Count > 0)
                        {
                            iLoanOfficerID = SelLeadSourceList.Rows[0]["DefaultUserId"] == DBNull.Value ? 0 : Convert.ToInt32(SelLeadSourceList.Rows[0]["DefaultUserId"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Company_Lead_Sources LeadSourceMgr = new Company_Lead_Sources();
                DataTable SelLeadSourceList = LeadSourceMgr.GetList("LeadSourceID='" + ddlLeadSource.SelectedValue + "'").Tables[0];
                if (SelLeadSourceList != null && SelLeadSourceList.Rows.Count > 0)
                {
                    iLoanOfficerID = SelLeadSourceList.Rows[0]["DefaultUserId"] == DBNull.Value ? 0 : Convert.ToInt32(SelLeadSourceList.Rows[0]["DefaultUserId"]);
                }
            }
        }
        else
        {
            iLoanOfficerID = Convert.ToInt32(sLoanOfficerID);
        }

        LoanDetailsModel.LoanOfficerId = iLoanOfficerID;

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iContactId"></param>
    /// <returns></returns>
    private DataTable GetContactLoanInfo(int iContactId) 
    {
        string sSql = "select top 1 * from Loans l inner join LoanContacts lc on l.FileId=lc.FileId "
                    + "where (dbo.lpfn_GetBorrowerContactId(l.FileId)=" + iContactId + " or dbo.lpfn_GetCoBorrowerContactId(l.FileId)=" + iContactId + ") and l.Status='Prospect' "
                    + "order by Created desc";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iContactId"></param>
    /// <returns></returns>
    private DataTable GetBorrowerClosedLoanInfo(int iContactId) 
    {
        string sSql = "select top 1 * from Loans l inner join LoanContacts lc on l.FileId=lc.FileId "
                    + "where (dbo.lpfn_GetBorrowerContactId(l.FileId)=" + iContactId + " or dbo.lpfn_GetCoBorrowerContactId(l.FileId)=" + iContactId + ") and l.Status='Closed' "
                    + "order by Created desc";

        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    #region Binding Data

    /// <summary>
    /// 
    /// </summary>
    /// <param name="LoanInfoData"></param>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="LoanInfoData"></param>
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

    /// <summary>
    /// 
    /// </summary>
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

    #endregion

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
        int iNewBorrowerID = 0;
        int iNewCoBorrowerID = 0;
        int iFileID = 0;

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

        string sLeadSource = ddlLeadSource.SelectedValue.ToString() == "0" ? "" : this.ddlLeadSource.SelectedItem.Text;
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

        string sRanking = ddlRanking.SelectedValue; //"Hot";  //bug 200
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

        decimal dRate = decimal.Zero;

        if (!string.IsNullOrEmpty(sRate))
        {
            try
            {
                if (decimal.TryParse(sRate, out dRate) == false)
                {
                    PageCommon.AlertMsg(this, "Failed to save, invalid value for field 'Rate(%)'");
                    return false;
                }
            }
            catch
            {
                PageCommon.AlertMsg(this, "Failed to save, invalid value for field 'Rate(%)'");
                return false;
            }
        }

        if (!string.IsNullOrEmpty(sRateNewLoan))
        {
            try
            {
                if (decimal.TryParse(sRateNewLoan, out dRate) == false)
                {
                    PageCommon.AlertMsg(this, "Failed to save, invalid value for field 'Rate(%)'");
                    return false;
                }
            }
            catch
            {
                PageCommon.AlertMsg(this, "Failed to save, invalid value for field 'Rate(%)'");
                return false;
            }
        }

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

        if (this.sActionMode == "Create_NoContactId_NoLoanId")
        {
            #region 如果输入CurrentLoan→require Start Date

            bool bEditCurrentLoan = false;
            if (sPurpose != "" || sLoanType != "" || sProgram != "" || sAmount != ""
                || sRate != "" || sPMI != "" || sPMITax != "" || sTerm != ""
                || sStartYear != "" || sStartMonth != "" || s2ndAmount != "")
            {
                bEditCurrentLoan = true;
            }

            if (bEditCurrentLoan == true)
            {
                // require sStartYear and sStartMonth
                if (sStartYear == "" || sStartMonth == "")
                {
                    PageCommon.AlertMsg(this, "Start Date of Current Loan is required.");
                    return false;
                }
            }

            #endregion

            #region create contact/prospect

            #region create borrower

           

            try
            {
                if (sMailingAddress == "Both" || sMailingAddress == "Borrower")
                {
                    iNewBorrowerID = this.CreateContactAndProspect(sFirstName, sLastName, sEmail, sCellPhone, sHomePhone, sWorkPhone, sDOB, sSSN, sDependants, sCreditRanking, sFICOScore, sMailingStreetAddress1, sMailingStreetAddress2, sMailingCity, sMailingState, sMailingZip, sLeadSource, sReferralID, sPurposeNewLoan, sLoanTypeNewLoan, sPropertyState);
                }
                else
                {
                    iNewBorrowerID = this.CreateContactAndProspect(sFirstName, sLastName, sEmail, sCellPhone, sHomePhone, sWorkPhone, sDOB, sSSN, sDependants, sCreditRanking, sFICOScore, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, sLeadSource, sReferralID, sPurposeNewLoan, sLoanTypeNewLoan, sPropertyState);
                }
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to save contact and prospect for borrower.");
                return false;
            }

            #endregion

            #region create co-borrower

          

            if (sFirstNameCoBorrower != string.Empty)
            {
                try
                {
                    if (sMailingAddress == "Both" || sMailingAddress == "Co-Borrower")
                    {
                        iNewCoBorrowerID = this.CreateContactAndProspect(sFirstNameCoBorrower, sLastNameCoBorrower, sEmailCoBorrower, sCellPhoneCoBorrower, sHomePhoneCoBorrower, sWorkPhoneCoBorrower, sDOBCoBorrower, sSSNCoBorrower, string.Empty, string.Empty, sFICOScoreCoBorrower, sMailingStreetAddress1, sMailingStreetAddress2, sMailingCity, sMailingState, sMailingZip, sLeadSource, sReferralID, sPurposeNewLoan, sLoanTypeNewLoan, sPropertyState);
                    }
                    else
                    {
                        iNewCoBorrowerID = this.CreateContactAndProspect(sFirstNameCoBorrower, sLastNameCoBorrower, sEmailCoBorrower, sCellPhoneCoBorrower, sHomePhoneCoBorrower, sWorkPhoneCoBorrower, sDOBCoBorrower, sSSNCoBorrower, string.Empty, string.Empty, sFICOScoreCoBorrower, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, sLeadSource, sReferralID, sPurposeNewLoan, sLoanTypeNewLoan, sPropertyState);
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

          
            try
            {
                iFileID = this.CreateLoan(iNewBorrowerID, iNewCoBorrowerID, sHousingStatus, sRentAmount, sPropertyStreetAddress1, sPropertyStreetAddress2, sPropertyCity, sPropertyState, sPropertyZip, sPropertyValue,
                    sPurposeNewLoan, sLoanTypeNewLoan, sProgramNewLoan, sAmountNewLoan, sRateNewLoan, sPMINewLoan, sPMITaxNewLoan, sTermNewLoan, sStartYearNewLoan, sStartMonthNewLoan, bSubordinate, b2nd, s2ndAmount,sRanking);
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to save new loan info.");
                return false;
            }

            #endregion

            #region create income and employment

            try
            {
                this.CreateIncomeAndEmployment(iFileID, iNewBorrowerID, sCompanyName, bSelfEmployed, sPosition, sMonthlySalary, sProfession,
                    sYearsInField, sWorkStartYear, sWorkStartMonth, sWorkEndYear, sWorkEndMonth,
                    sOtherMonthlyIncome, sLiquidAssets, sComments);
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to save income and employment.");
                return false;
            }

            #endregion

            // 如果输入CurrentLoan→create a new loan→close it
            if (bEditCurrentLoan == true)
            {
                #region create loan

                int iFileID2 = 0;
                try
                {
                    iFileID2 = this.CreateLoan(iNewBorrowerID, iNewCoBorrowerID, sHousingStatus, sRentAmount, sPropertyStreetAddress1, sPropertyStreetAddress2, sPropertyCity, sPropertyState, sPropertyZip, sPropertyValue,
                        sPurpose, sLoanType, sProgram, sAmount, sRate, sPMI, sPMITax, sTerm, sStartYear, sStartMonth, bSubordinate, b2nd, s2ndAmount,sRanking);
                }
                catch (Exception ex)
                {
                    PageCommon.AlertMsg(this, "Failed to save current loan info.");
                    return false;
                }

                #endregion

                #region close the new loan

                DateTime? CloseDate = this.GetStartDate(sStartYear, sStartMonth);
                string sCloseDate = ((DateTime)CloseDate).ToShortDateString();

                try
                {
                    this.CloseNewLoan(iFileID2, sCloseDate, this.CurrUser.iUserID);
                }
                catch (Exception)
                {
                    PageCommon.AlertMsg(this, "Failed to close the new loan.");
                    return false;
                }
                
                #endregion
            }

            iNewFileId = iFileID;
        }
        else if (this.sActionMode == "Create_HasContactId_NoLoan")
        {
            #region create contact/prospect

            #region update borrower

            iNewBorrowerID = this.iContactId;
            try
            {
                if (sMailingAddress == "Both" || sMailingAddress == "Borrower")
                {
                    this.ProspectMgr.UpdateBorrower(iNewBorrowerID, sFirstName, sLastName, sEmail,
                        sCellPhone, sHomePhone, sWorkPhone, sDOB, sSSN, sDependants, sCreditRanking, sFICOScore,
                        sMailingStreetAddress1, sMailingStreetAddress2, sMailingCity, sMailingState, sMailingZip,
                        sLeadSource, sReferralID);
                }
                else
                {
                    this.ProspectMgr.UpdateBorrower(iNewBorrowerID, sFirstName, sLastName, sEmail,
                        sCellPhone, sHomePhone, sWorkPhone, sDOB, sSSN, sDependants, sCreditRanking, sFICOScore);
                }
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to save loan info.");
                return false;
            }

            #endregion

            #region create co-Borrower


            if (sFirstNameCoBorrower != string.Empty)
            {
                try
                {
                    if (sMailingAddress == "Both" || sMailingAddress == "Co-Borrower")
                    {
                        iNewCoBorrowerID = this.CreateContactAndProspect(sFirstNameCoBorrower, sLastNameCoBorrower, sEmailCoBorrower, sCellPhoneCoBorrower, sHomePhoneCoBorrower, sWorkPhoneCoBorrower, sDOBCoBorrower, sSSNCoBorrower, string.Empty, string.Empty, sFICOScoreCoBorrower, sMailingStreetAddress1, sMailingStreetAddress2, sMailingCity, sMailingState, sMailingZip, sLeadSource, sReferralID, sPurposeNewLoan, sLoanTypeNewLoan, sPropertyState);
                    }
                    else
                    {
                        iNewCoBorrowerID = this.CreateContactAndProspect(sFirstNameCoBorrower, sLastNameCoBorrower, sEmailCoBorrower, sCellPhoneCoBorrower, sHomePhoneCoBorrower, sWorkPhoneCoBorrower, sDOBCoBorrower, sSSNCoBorrower, string.Empty, string.Empty, sFICOScoreCoBorrower, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, sLeadSource, sReferralID, sPurposeNewLoan, sLoanTypeNewLoan, sPropertyState);
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

            try
            {
                iFileID = this.CreateLoan(iNewBorrowerID, iNewCoBorrowerID, sHousingStatus, sRentAmount, sPropertyStreetAddress1, sPropertyStreetAddress2, sPropertyCity, sPropertyState, sPropertyZip, sPropertyValue,
                    sPurposeNewLoan, sLoanTypeNewLoan, sProgramNewLoan, sAmountNewLoan, sRateNewLoan, sPMINewLoan, sPMITaxNewLoan, sTermNewLoan, sStartYearNewLoan, sStartMonthNewLoan, bSubordinate, b2nd, s2ndAmount,sRanking);
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
                this.CreateProspectEmployment(iNewBorrowerID, sCompanyName, bSelfEmployed, sPosition, sProfession,
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
                this.CreateProspectIncome(iNewBorrowerID, sMonthlySalary, sOtherMonthlyIncome);
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
                this.CreateProspectAssets(iNewBorrowerID, sLiquidAssets);
            }
            else
            {
                // update
                int iProspectAssetId = Convert.ToInt32(this.BorrowerInfo_Assets.Rows[0]["ProspectAssetId"]);
                this.UpdateProspectAssets(iProspectAssetId, sLiquidAssets);
            }

            // create LoanNotes
            this.CreateLoanNotes(iFileID, sComments);

            #endregion

            // update close info
            if (this.BorrowerClosedLoanInfo == null)
            {
                #region update loan

                int iClosedLoanId = Convert.ToInt32(this.BorrowerClosedLoanInfo.Rows[0]["FileId"]);

                try
                {
                    this.LoansMgr.UpldateLoanInfo(iClosedLoanId, sHousingStatus, sRentAmount, sPropertyStreetAddress1, sPropertyStreetAddress2,
                        sPropertyCity, sPropertyState, sPropertyZip, sPropertyValue,
                        sPurpose, sLoanType, sProgram, sAmount, sRate, sPMI, sPMITax, sTerm, sStartDate, b2nd, s2ndAmount,sRanking);
                }
                catch (Exception ex)
                {
                    PageCommon.AlertMsg(this, "Failed to save borrower closed loan info.");
                    return false;
                }

                #endregion
            }

            iNewFileId = iFileID;
        }

        if (ddlLoanOfficer.SelectedValue == "0")  //Lead Routing Engine
        {
            #region  invoke the WCF API  LeadRouting_AssignLoanOfficer
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    LR_AssignLoanOfficerReq req = new LR_AssignLoanOfficerReq();
                    req.LoanId = iNewFileId;
                    req.LoanOfficerId = iLoanOfficerID;
                    req.BorrowerContactId = iNewBorrowerID;
                    req.CoBorrowerContactId = iNewCoBorrowerID;
                    req.ReqHdr = new ReqHdr();
                    req.ReqHdr.UserId = CurrUser.iUserID;
                    req.ReqHdr.SecurityToken = "SecurityToken";
                    RespHdr resp = client.LeadRouting_AssignLoanOfficer(req);
                }
            }
            catch (Exception ex)
            {

                PageCommon.WriteJsEnd(this, "Pulse Lead Manager is not running. Please select a Loan Officer from the list and save the lead.", PageCommon.Js_RefreshSelf);
                return false;
            }
            #endregion
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
            PageCommon.WriteJsEnd(this, "Save successfully.", "window.location.href='ProspectLoanDetails.aspx?FromPage=&FileID=" + iFileId + "&FileIDs=" + iFileId + "'");
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
}
