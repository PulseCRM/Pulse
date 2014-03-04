using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.BLL;
using System.Data;

public partial class LoanDetails_AdvancedLoanFilters : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsPostBack == false)
        {
            #region Organizations

            #region Region

            Regions RegionMgr = new Regions();
            DataTable RegionList = null;

            if (this.CurrUser.sRoleName == "Executive")
            {
                RegionList = RegionMgr.GetRegionFilter_Executive(this.CurrUser.iUserID);
            }
            else if (this.CurrUser.sRoleName == "Branch Manager")
            {
                RegionList = RegionMgr.GetRegionFilter_Branch_Manager(this.CurrUser.iUserID);
            }
            else
            {
                RegionList = RegionMgr.GetUserRegions(this.CurrUser.iUserID);
            }

            if (RegionList.Rows.Count > 0)
            {
                DataRow NewRegionRow = RegionList.NewRow();
                NewRegionRow["RegionID"] = 0;
                NewRegionRow["Name"] = "ALL";
                RegionList.Rows.InsertAt(NewRegionRow, 0);
            }

            this.ddlRegion.DataSource = RegionList;
            this.ddlRegion.DataBind();

            #endregion

            #region Division

            Divisions DivisionMgr = new Divisions();
            DataTable DivisionList = null;

            if (this.CurrUser.sRoleName == "Executive")
            {
                DivisionList = DivisionMgr.GetDivisionFilter_Executive(this.CurrUser.iUserID, 0);
            }
            else if (this.CurrUser.sRoleName == "Branch Manager")
            {
                DivisionList = DivisionMgr.GetDivisionFilter_Branch_Manager(this.CurrUser.iUserID, 0);
            }
            else
            {
                DivisionList = DivisionMgr.GetDivisionFilter(this.CurrUser.iUserID, 0);
            }

            if (DivisionList.Rows.Count > 0)
            {
                DataRow NewDivisionRow = DivisionList.NewRow();
                NewDivisionRow["DivisionID"] = 0;
                NewDivisionRow["Name"] = "ALL";
                DivisionList.Rows.InsertAt(NewDivisionRow, 0);
            }

            this.ddlDivision.DataSource = DivisionList;
            this.ddlDivision.DataBind();

            #endregion

            #region Branch

            Branches BranchMgr = new Branches();
            DataTable BranchList = null;

            if (this.CurrUser.sRoleName == "Executive")
            {
                BranchList = BranchMgr.GetBranchFilter_Executive(this.CurrUser.iUserID, 0, 0);
            }
            else if (this.CurrUser.sRoleName == "Branch Manager")
            {
                BranchList = BranchMgr.GetBranchFilter_Branch_Manager(this.CurrUser.iUserID, 0, 0);
            }
            else
            {
                BranchList = BranchMgr.GetBranchFilter(this.CurrUser.iUserID, 0, 0);
            }

            if (BranchList.Rows.Count > 0)
            {
                DataRow NewBranchRow = BranchList.NewRow();
                NewBranchRow["BranchID"] = 0;
                NewBranchRow["Name"] = "ALL";
                BranchList.Rows.InsertAt(NewBranchRow, 0);
            }

            this.ddlBranch.DataSource = BranchList;
            this.ddlBranch.DataBind();

            #endregion

            #region PointFolder

            PointFolders PointFolderMgr = new PointFolders();
            DataTable PointFolderList = null;

            if (this.CurrUser.sRoleName == "Executive")
            {
                PointFolderList = PointFolderMgr.GetPointFolder_Executive(this.CurrUser.iUserID, "0", "0", "0");
            }
            else if (this.CurrUser.sRoleName == "Branch Manager")
            {
                PointFolderList = PointFolderMgr.GetPointFolder_BranchManager(this.CurrUser.iUserID, "0", "0", "0");
            }
            else
            {
                PointFolderList = PointFolderMgr.GetPointFolder_User(this.CurrUser.iUserID, "0", "0", "0");
            }

            if (PointFolderList.Rows.Count > 0)
            {
                DataRow NewPointFolderRow = PointFolderList.NewRow();
                NewPointFolderRow["FolderID"] = 0;
                NewPointFolderRow["Name"] = "ALL";
                PointFolderList.Rows.InsertAt(NewPointFolderRow, 0);
            }

            this.ddlFolder.DataSource = PointFolderList;
            this.ddlFolder.DataBind();

            #endregion

            #endregion

            #region Users and Borrower

            #region Loan Officer

            DataTable LoanRolesList = this.GetLoanOfficerList(CurrUser.iUserID);

            DataRow NewLoanRoleRow = null;
            if (LoanRolesList.Rows.Count > 0)
            {
                NewLoanRoleRow = LoanRolesList.NewRow();
                NewLoanRoleRow["ID"] = 0;
                NewLoanRoleRow["Name"] = "ALL";
                LoanRolesList.Rows.InsertAt(NewLoanRoleRow, 0);
            }

            this.ddlLoanOfficer.DataSource = LoanRolesList;
            this.ddlLoanOfficer.DataBind();

            #endregion

            #region Loan Officer Assistant

            LoanRolesList = this.GetLoanRolesList("Assistant");

            if (LoanRolesList.Rows.Count > 0)
            {
                NewLoanRoleRow = LoanRolesList.NewRow();
                NewLoanRoleRow["Name"] = "ALL";
                LoanRolesList.Rows.InsertAt(NewLoanRoleRow, 0);
            }

            this.ddlLoanOfficerAssistant.DataSource = LoanRolesList;
            this.ddlLoanOfficerAssistant.DataBind();

            #endregion

            #region Processor

            LoanRolesList = this.GetProcessorList();

            if (LoanRolesList.Rows.Count > 0)
            {
                NewLoanRoleRow = LoanRolesList.NewRow();
                NewLoanRoleRow["ID"] = 0;
                NewLoanRoleRow["Name"] = "ALL";
                LoanRolesList.Rows.InsertAt(NewLoanRoleRow, 0);
            }

            this.ddlProcessor.DataSource = LoanRolesList;
            this.ddlProcessor.DataBind();

            #endregion

            #region Jr Processor

            LoanRolesList = this.GetLoanRolesList("JrProcessor");

            if (LoanRolesList.Rows.Count > 0)
            {
                NewLoanRoleRow = LoanRolesList.NewRow();
                NewLoanRoleRow["Name"] = "ALL";
                LoanRolesList.Rows.InsertAt(NewLoanRoleRow, 0);
            }

            this.ddlJrProcessor.DataSource = LoanRolesList;
            this.ddlJrProcessor.DataBind();

            #endregion

            #region Doc Prep

            LoanRolesList = this.GetLoanRolesList("DocPrep");

            if (LoanRolesList.Rows.Count > 0)
            {
                NewLoanRoleRow = LoanRolesList.NewRow();
                NewLoanRoleRow["Name"] = "ALL";
                LoanRolesList.Rows.InsertAt(NewLoanRoleRow, 0);
            }

            this.ddlDocPrep.DataSource = LoanRolesList;
            this.ddlDocPrep.DataBind();

            #endregion

            #region Shipper

            LoanRolesList = this.GetLoanRolesList("Shipper");

            if (LoanRolesList.Rows.Count > 0)
            {
                NewLoanRoleRow = LoanRolesList.NewRow();
                NewLoanRoleRow["Name"] = "ALL";
                LoanRolesList.Rows.InsertAt(NewLoanRoleRow, 0);
            }

            this.ddlShipper.DataSource = LoanRolesList;
            this.ddlShipper.DataBind();

            #endregion

            #region Closer

            LoanRolesList = this.GetLoanRolesList("Closer");

            if (LoanRolesList.Rows.Count > 0)
            {
                NewLoanRoleRow = LoanRolesList.NewRow();
                NewLoanRoleRow["Name"] = "ALL";
                LoanRolesList.Rows.InsertAt(NewLoanRoleRow, 0);
            }

            this.ddlCloser.DataSource = LoanRolesList;
            this.ddlCloser.DataBind();

            #endregion

            #region Underwriter

            LoanRolesList = this.GetUnderwriterList();

            if (LoanRolesList.Rows.Count > 0)
            {
                NewLoanRoleRow = LoanRolesList.NewRow();
                NewLoanRoleRow["ID"] = 0;
                NewLoanRoleRow["Name"] = "ALL";
                LoanRolesList.Rows.InsertAt(NewLoanRoleRow, 0);
            }

            this.ddlUnderwriter.DataSource = LoanRolesList;
            this.ddlUnderwriter.DataBind();

            #endregion

            #endregion

            #region Lender, Referral and Loan Info

            #region Lender

            DataTable LenderList = this.GetLenderList();

            if (LenderList.Rows.Count > 0)
            {
                DataRow NewLenderRow = LenderList.NewRow();
                NewLenderRow["LenderId"] = 0;
                NewLenderRow["Lender"] = "ALL";
                LenderList.Rows.InsertAt(NewLenderRow, 0);
            }

            this.ddlLender.DataSource = LenderList;
            this.ddlLender.DataBind();

            #endregion

            #region Program

            DataTable ProgramList = this.GetProgramList();

            if (ProgramList.Rows.Count > 0)
            {
                DataRow NewProgramRow = ProgramList.NewRow();
                NewProgramRow["Program"] = "ALL";
                ProgramList.Rows.InsertAt(NewProgramRow, 0);
            }

            this.ddlProgram.DataSource = ProgramList;
            this.ddlProgram.DataBind();

            #endregion

            #region Partner

            DataTable PartnerList = this.GetPartnerList();

            if (PartnerList.Rows.Count > 0)
            {
                DataRow NewPartnerRow = PartnerList.NewRow();
                NewPartnerRow["PartnerId"] = 0;
                NewPartnerRow["Partner"] = "ALL";
                PartnerList.Rows.InsertAt(NewPartnerRow, 0);
            }

            this.ddlPartner.DataSource = PartnerList;
            this.ddlPartner.DataBind();

            #endregion

            #region LeadSource

            DataTable LeadSourceList = this.GetLeadSourceList();

            if (LeadSourceList.Rows.Count > 0)
            {
                DataRow NewLeadSourceRow = LeadSourceList.NewRow();
                NewLeadSourceRow["LeadSource"] = "ALL";
                LeadSourceList.Rows.InsertAt(NewLeadSourceRow, 0);
            }

            this.ddlLeadSource.DataSource = LeadSourceList;
            this.ddlLeadSource.DataBind();

            #endregion

            #region Referral

            DataTable ReferralList = this.GetReferralList();

            if (ReferralList.Rows.Count > 0)
            {
                DataRow NewReferralRow = ReferralList.NewRow();
                NewReferralRow["ReferralId"] = 0;
                NewReferralRow["Referral"] = "ALL";
                ReferralList.Rows.InsertAt(NewReferralRow, 0);
            }

            this.ddlReferral.DataSource = ReferralList;
            this.ddlReferral.DataBind();

            #endregion

            #region Purpose

            DataTable PurposeList = this.GetPurposeList();

            if (PurposeList.Rows.Count > 0)
            {
                DataRow NewPurposeRow = PurposeList.NewRow();
                NewPurposeRow["Purpose"] = "ALL";
                PurposeList.Rows.InsertAt(NewPurposeRow, 0);
            }

            this.ddlPurpose.DataSource = PurposeList;
            this.ddlPurpose.DataBind();

            #endregion

            #endregion

            #region Current Stage

            Template_Stages Template_Stages_Mgr = new Template_Stages();
            DataTable Template_Stages_List = Template_Stages_Mgr.GetStageTemplateList(" and Enabled=1 order by Alias");

            if (Template_Stages_List.Rows.Count > 0)
            {
                DataRow NewStageTmplRow = Template_Stages_List.NewRow();
                NewStageTmplRow["TemplStageId"] = "0";
                NewStageTmplRow["Alias"] = "ALL";
                Template_Stages_List.Rows.InsertAt(NewStageTmplRow, 0);
            }

            this.ddlCurrentStage.DataSource = Template_Stages_List;
            this.ddlCurrentStage.DataBind();

            #endregion

            #region Point Fields

            UserLoansViewPointFields UserLoansViewPointFields1 = new UserLoansViewPointFields();
            DataTable HeadingList = UserLoansViewPointFields1.GetUserPointFieldInfo(this.CurrUser.iUserID);

            this.rptHeading.DataSource = HeadingList;
            this.rptHeading.DataBind();

            #endregion

            //DataTable dtTaskName = new DataTable();
            //dtTaskName.Columns.Add("TaskID");
            //dtTaskName.Columns.Add("TaskName");
            //dtTaskName.Columns.Add("MatchType");
            //dtTaskName.Columns.Add("DueDateStart");
            //dtTaskName.Columns.Add("DueDateEnd");
            //dtTaskName.Columns.Add("CompDateStart");
            //dtTaskName.Columns.Add("CompDateEnd");

            //gridLicensesList.DataSource = dtTaskName;
            //gridLicensesList.DataBind();
        }
    }

    public DataTable GetLoanRolesList(string sRole) 
    {
        string sSql0 = string.Empty;
        if (this.CurrUser.sRoleName == "Executive")
        {
            sSql0 = "select distinct [" + sRole + "] as Name from V_ProcessingPipelineInfo where isnull([" + sRole + "],'')<>'' "
                  + "and BranchId in (select BranchId from dbo.lpfn_GetUserBranches_Executive(" + this.CurrUser.iUserID + ")) "
                  + "order by [" + sRole + "]";
        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            sSql0 = "select distinct [" + sRole + "] as Name from V_ProcessingPipelineInfo where isnull([" + sRole + "],'')<>'' "
                  + "and BranchId in (select BranchId from dbo.lpfn_GetUserBranches_Branch_Manager(" + this.CurrUser.iUserID + ")) "
                  + "order by [" + sRole + "]";

        }
        else
        {
            sSql0 = "select distinct [" + sRole + "] as Name from V_ProcessingPipelineInfo where isnull([" + sRole + "],'')<>'' "
                  + "and BranchId in (select BranchId from dbo.lpfn_GetUserBranches(" + this.CurrUser.iUserID + ")) "
                  + "order by [" + sRole + "]";

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

    public DataTable GetProcessorList()
    {
        string sSql0 = string.Empty;
        if (this.CurrUser.sRoleName == "Executive")
        {
            sSql0 = @"select distinct ProcessorId as ID, Processor as Name from V_ProcessingPipelineInfo 
where isnull(ProcessorId,'')<>'' 
and BranchId in (select BranchId from dbo.lpfn_GetUserBranches_Executive(" + this.CurrUser.iUserID + @")) 
order by Processor";
        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            sSql0 = @"select distinct ProcessorId as ID, Processor as Name from V_ProcessingPipelineInfo 
where isnull(ProcessorId,'')<>'' 
and BranchId in (select BranchId from dbo.lpfn_GetUserBranches_Branch_Manager(" + this.CurrUser.iUserID + @")) 
order by Processor";
        }
        else
        {
            sSql0 = @"select distinct ProcessorId as ID, Processor as Name from V_ProcessingPipelineInfo 
where isnull(ProcessorId,'')<>'' 
and BranchId in (select BranchId from dbo.lpfn_GetUserBranches(" + this.CurrUser.iUserID + @")) 
order by Processor";
        }

        DataTable ProcessorList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);

        return ProcessorList;
    }

    public DataTable GetUnderwriterList()
    {
        string sSql0 = string.Empty;
        if (this.CurrUser.sRoleName == "Executive")
        {
            sSql0 = 
    @"select UserId as ID, LastName + ', ' + FirstName as Name from Users where UserId in (
	    select distinct UnderwriterId from V_ProcessingPipelineInfo where isnull(UnderwriterId,'')<>'' 
	    and BranchId in (select BranchId from dbo.lpfn_GetUserBranches_Executive(" + this.CurrUser.iUserID + @"))
    ) order by LastName + ', ' + FirstName";

        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            sSql0 = 
    @"select UserId as ID, LastName + ', ' + FirstName as Name from Users where UserId in (
	    select distinct UnderwriterId from V_ProcessingPipelineInfo where isnull(UnderwriterId,'')<>'' 
		and BranchId in (select BranchId from dbo.lpfn_GetUserBranches_Branch_Manager(" + this.CurrUser.iUserID + @"))
    ) order by LastName + ', ' + FirstName";
        }
        else
        {
            sSql0 = 
    @"select UserId as ID, LastName + ', ' + FirstName as Name from Users where UserId in (
	    select distinct UnderwriterId from V_ProcessingPipelineInfo where isnull(UnderwriterId,'')<>'' 
		and BranchId in (select BranchId from dbo.lpfn_GetUserBranches(" + this.CurrUser.iUserID + @"))
    ) order by LastName + ', ' + FirstName";
        }

        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
    }

    private DataTable GetLenderList() 
    {
        string sSql = string.Empty;
        if (this.CurrUser.sRoleName == "Executive")
        {
            sSql = "select distinct LenderId, Lender from V_ProcessingPipelineInfo where isnull(LenderId,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans_Executive(" + this.CurrUser.iUserID + ")) order by Lender";
        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            sSql = "select distinct LenderId, Lender from V_ProcessingPipelineInfo where isnull(LenderId,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans_Branch_Manager(" + this.CurrUser.iUserID + ")) order by Lender";
        }
        else
        {
            sSql = "select distinct LenderId, Lender from V_ProcessingPipelineInfo where isnull(LenderId,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans2(" + this.CurrUser.iUserID + "," + Convert.ToInt32(this.CurrUser.bAccessOtherLoans) + ")) order by Lender";
        }

        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private DataTable GetProgramList()
    {
        string sSql = string.Empty;
        if (this.CurrUser.sRoleName == "Executive")
        {
            sSql = "select distinct Program from V_ProcessingPipelineInfo where isnull(Program,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans_Executive(" + this.CurrUser.iUserID + ")) order by Program";
        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            sSql = "select distinct Program from V_ProcessingPipelineInfo where isnull(Program,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans_Branch_Manager(" + this.CurrUser.iUserID + ")) order by Program";
        }
        else
        {
            sSql = "select distinct Program from V_ProcessingPipelineInfo where isnull(Program,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans2(" + this.CurrUser.iUserID + "," + Convert.ToInt32(this.CurrUser.bAccessOtherLoans) + ")) order by Program";
        }

        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private DataTable GetPartnerList()
    {
        string sSql = string.Empty;
        if (this.CurrUser.sRoleName == "Executive")
        {
            sSql = "select distinct PartnerId, Partner from V_ProcessingPipelineInfo where isnull(PartnerId,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans_Executive(" + this.CurrUser.iUserID + ")) order by Partner";
        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            sSql = "select distinct PartnerId, Partner from V_ProcessingPipelineInfo where isnull(PartnerId,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans_Branch_Manager(" + this.CurrUser.iUserID + ")) order by Partner";
        }
        else
        {
            sSql = "select distinct PartnerId, Partner from V_ProcessingPipelineInfo where isnull(PartnerId,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans2(" + this.CurrUser.iUserID + "," + Convert.ToInt32(this.CurrUser.bAccessOtherLoans) + ")) order by Partner";
        }

        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private DataTable GetLeadSourceList()
    {
        string sSql = string.Empty;
        if (this.CurrUser.sRoleName == "Executive")
        {
            sSql = "select distinct LeadSource from V_ProcessingPipelineInfo where isnull(LeadSource,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans_Executive(" + this.CurrUser.iUserID + ")) order by LeadSource";
        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            sSql = "select distinct LeadSource from V_ProcessingPipelineInfo where isnull(LeadSource,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans_Branch_Manager(" + this.CurrUser.iUserID + ")) order by LeadSource";
        }
        else
        {
            sSql = "select distinct LeadSource from V_ProcessingPipelineInfo where isnull(LeadSource,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans2(" + this.CurrUser.iUserID + "," + Convert.ToInt32(this.CurrUser.bAccessOtherLoans) + ")) order by LeadSource";
        }

        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private DataTable GetReferralList()
    {
        string sSql = string.Empty;
        if (this.CurrUser.sRoleName == "Executive")
        {
            sSql = "select distinct ReferralId, Referral from V_ProcessingPipelineInfo where isnull(ReferralId,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans_Executive(" + this.CurrUser.iUserID + ")) order by Referral";
        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            sSql = "select distinct ReferralId, Referral from V_ProcessingPipelineInfo where isnull(ReferralId,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans_Branch_Manager(" + this.CurrUser.iUserID + ")) order by Referral";
        }
        else
        {
            sSql = "select distinct ReferralId, Referral from V_ProcessingPipelineInfo where isnull(ReferralId,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans2(" + this.CurrUser.iUserID + "," + Convert.ToInt32(this.CurrUser.bAccessOtherLoans) + ")) order by Referral";
        }

        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private DataTable GetPurposeList()
    {
        string sSql = string.Empty;
        if (this.CurrUser.sRoleName == "Executive")
        {
            sSql = "select distinct Purpose from V_ProcessingPipelineInfo where isnull(Purpose,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans_Executive(" + this.CurrUser.iUserID + ")) order by Purpose";
        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            sSql = "select distinct Purpose from V_ProcessingPipelineInfo where isnull(Purpose,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans_Branch_Manager(" + this.CurrUser.iUserID + ")) order by Purpose";
        }
        else
        {
            sSql = "select distinct Purpose from V_ProcessingPipelineInfo where isnull(Purpose,'')<>'' and FileId in (select LoanID from dbo.lpfn_GetUserLoans2(" + this.CurrUser.iUserID + "," + Convert.ToInt32(this.CurrUser.bAccessOtherLoans) + ")) order by Purpose";
        }

        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    public string GetValueTextbox(string sDataType) 
    {
        if (sDataType == "2")   // Numeric
        {
            return "<input id=\"txtValue\" type=\"text\" value=\"\" style=\"width:370px;\" />";
        }
        else if (sDataType == "3")   //  Yes/No
        {
            return "<input id=\"txtValue\" type=\"text\" value=\"\" style=\"width:370px;\" />";
        }
        else if (sDataType == "4")   //  Date
        {
            return "<input id=\"txtValue\" type=\"text\" value=\"\" style=\"width:370px;\" />";
        }
        else // String
        {
            return "<input id=\"txtValue\" type=\"text\" value=\"\" style=\"width:370px;\" />";
        }
    }
}
