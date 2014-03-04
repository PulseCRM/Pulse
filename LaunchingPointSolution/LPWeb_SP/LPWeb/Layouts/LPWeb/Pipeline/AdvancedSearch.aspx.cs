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
using System.Data.SqlClient;

public partial class Pipeline_AdvancedSearch : BasePage
{
    LoginUser CurrentUser;
    bool bSearchLoans = true;
    bool bSearchOpportunities = true;
    bool bSearchArchivedLoans = true;
    bool bSearchClients = true;
    bool bSearchPartners = true;
    string sName = string.Empty;
    string sCompany = string.Empty;
    string sAddress = string.Empty;
    string sCity = string.Empty;
    string sState = string.Empty;
    string sEmail = string.Empty;
    string sPhone = string.Empty;
    string sLoanNumber = string.Empty;
    string sFilename = string.Empty;
    string sLoanOfficer = string.Empty;
    string sProcessor = string.Empty;
    string sRegion = string.Empty;
    string sDivision = string.Empty;
    string sBranch = string.Empty;
    string sServiceType = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 接收页面参数

        #region checkbox

        bSearchLoans = true;
        if (this.Request.QueryString["SearchLoans"] != null)
        {
            string sSearchLoans = this.Request.QueryString["SearchLoans"].ToString();
            if (sSearchLoans != "true" && sSearchLoans != "false")
            {
                PageCommon.WriteJsEnd(this, "Invalid SearchLoans option.", "window.location.href = window.location.pathname;");
            }

            bSearchLoans = Boolean.Parse(sSearchLoans);
        }

        bSearchOpportunities = true;
        if (this.Request.QueryString["SearchOpportunities"] != null)
        {
            string sSearchOpportunities = this.Request.QueryString["SearchOpportunities"].ToString();
            if (sSearchOpportunities != "true" && sSearchOpportunities != "false")
            {
                PageCommon.WriteJsEnd(this, "Invalid SearchOpportunities option.", "window.location.href = window.location.pathname;");
            }

            bSearchOpportunities = Boolean.Parse(sSearchOpportunities);
        }

        bSearchArchivedLoans = true;
        if (this.Request.QueryString["SearchArchivedLoans"] != null)
        {
            string sSearchArchivedLoans = this.Request.QueryString["SearchArchivedLoans"].ToString();
            if (sSearchArchivedLoans != "true" && sSearchArchivedLoans != "false")
            {
                PageCommon.WriteJsEnd(this, "Invalid SearchArchivedLoans option.", "window.location.href = window.location.pathname;");
            }

            bSearchArchivedLoans = Boolean.Parse(sSearchArchivedLoans);
        }

        bSearchClients = true;
        if (this.Request.QueryString["SearchClients"] != null)
        {
            string sSearchClients = this.Request.QueryString["SearchClients"].ToString();
            if (sSearchClients != "true" && sSearchClients != "false")
            {
                PageCommon.WriteJsEnd(this, "Invalid SearchClients option.", "window.location.href = window.location.pathname;");
            }

            bSearchClients = Boolean.Parse(sSearchClients);
        }

        bSearchPartners = true;
        if (this.Request.QueryString["SearchPartners"] != null)
        {
            string sSearchPartners = this.Request.QueryString["SearchPartners"].ToString();
            if (sSearchPartners != "true" && sSearchPartners != "false")
            {
                PageCommon.WriteJsEnd(this, "Invalid SearchPartners option.", "window.location.href = window.location.pathname;");
            }

            bSearchPartners = Boolean.Parse(sSearchPartners);
        }


        #endregion

        #region textbox

        // Name
        sName = string.Empty;
        if (this.Request.QueryString["Name"] != null)
        {
            sName = this.Request.QueryString["Name"].ToString();
        }

        // Company
        sCompany = string.Empty;
        if (this.Request.QueryString["Company"] != null)
        {
            sCompany = this.Request.QueryString["Company"].ToString();
        }

        // Address
        sAddress = string.Empty;
        if (this.Request.QueryString["Address"] != null)
        {
            sAddress = this.Request.QueryString["Address"].ToString();
        }

        // City
        sCity = string.Empty;
        if (this.Request.QueryString["City"] != null)
        {
            sCity = this.Request.QueryString["City"].ToString();
        }

        // State
        sState = string.Empty;
        if (this.Request.QueryString["State"] != null)
        {
            sState = this.Request.QueryString["State"].ToString();
        }

        // Email
        sEmail = string.Empty;
        if (this.Request.QueryString["Email"] != null)
        {
            sEmail = this.Request.QueryString["Email"].ToString();
        }

        // Phone
        sPhone = string.Empty;
        if (this.Request.QueryString["Phone"] != null)
        {
            sPhone = this.Request.QueryString["Phone"].ToString();
        }

        // Loan Number
        sLoanNumber = string.Empty;
        if (this.Request.QueryString["LoanNumber"] != null)
        {
            sLoanNumber = this.Request.QueryString["LoanNumber"].ToString();
        }

        // Filename
        sFilename = string.Empty;
        if (this.Request.QueryString["Filename"] != null)
        {
            sFilename = this.Request.QueryString["Filename"].ToString();
        }

        #endregion

        #region dropdown list

        // Loan Officer
        sLoanOfficer = string.Empty;
        if (this.Request.QueryString["LoanOfficer"] != null)
        {
            string sLoanOfficerTemp = this.Request.QueryString["LoanOfficer"].ToString();
            if (PageCommon.IsID(sLoanOfficerTemp) == false)
            {
                PageCommon.WriteJsEnd(this, "Invalid LoanOfficer id.", "window.location.href = window.location.pathname;");
            }

            sLoanOfficer = sLoanOfficerTemp;
        }

        // Processor
        sProcessor = string.Empty;
        if (this.Request.QueryString["Processor"] != null)
        {
            string sProcessorTemp = this.Request.QueryString["Processor"].ToString();
            if (PageCommon.IsID(sProcessorTemp) == false)
            {
                PageCommon.WriteJsEnd(this, "Invalid Processor id.", "window.location.href = window.location.pathname;");
            }

            sProcessor = sProcessorTemp;
        }

        // Region
        sRegion = string.Empty;
        if (this.Request.QueryString["Region"] != null)
        {
            string sRegionTemp = this.Request.QueryString["Region"].ToString();
            if (PageCommon.IsID(sRegionTemp) == false)
            {
                PageCommon.WriteJsEnd(this, "Invalid region id.", "window.location.href = window.location.pathname;");
            }

            sRegion = sRegionTemp;
        }

        // Division
        sDivision = string.Empty;
        if (this.Request.QueryString["Division"] != null)
        {
            string sDivisionTemp = this.Request.QueryString["Division"].ToString();
            if (PageCommon.IsID(sDivisionTemp) == false)
            {
                PageCommon.WriteJsEnd(this, "Invalid division id.", "window.location.href = window.location.pathname;");
            }

            sDivision = sDivisionTemp;
        }

        // Branch
        sBranch = string.Empty;
        if (this.Request.QueryString["Branch"] != null)
        {
            string sBranchTemp = this.Request.QueryString["Branch"].ToString();
            if (PageCommon.IsID(sBranchTemp) == false)
            {
                PageCommon.WriteJsEnd(this, "Invalid Branch id.", "window.location.href = window.location.pathname;");
            }

            sBranch = sBranchTemp;
        }

        // Service Type
        sServiceType = string.Empty;
        if (this.Request.QueryString["ServiceType"] != null)
        {
            string sServiceTypeTemp = this.Request.QueryString["ServiceType"].ToString();
            if (PageCommon.IsID(sServiceTypeTemp) == false)
            {
                PageCommon.WriteJsEnd(this, "Invalid ServiceType id.", "window.location.href = window.location.pathname;");
            }

            sServiceType = sServiceTypeTemp;
        }

        #endregion

        #endregion
        CurrentUser = this.CurrUser;
        #region Dropdown Lists

        #region 加载Loan Officer List

        if (CurrentUser.iRoleID > 2 && CurrentUser.userRole.OtherLoanAccess == false)
        {
            this.ddlLoanOfficer.Enabled = false;
        }
        else
        {
            DataTable LoanOfficerListData = this.GetLoanOfficerList(CurrentUser.iUserID);

            DataRow NewLoanOfficerRow = LoanOfficerListData.NewRow();
            NewLoanOfficerRow["UserId"] = DBNull.Value;
            NewLoanOfficerRow["FullName"] = "All";

            LoanOfficerListData.Rows.InsertAt(NewLoanOfficerRow, 0);

            this.ddlLoanOfficer.DataSource = LoanOfficerListData;
            this.ddlLoanOfficer.DataBind();
        }

        #endregion

        #region 加载Processor List

        if (CurrentUser.iRoleID > 2 && CurrentUser.userRole.OtherLoanAccess == false)
        {
            this.ddlProcessor.Enabled = false;
        }
        else
        {
            DataTable ProcessorListData = this.GetProcessorList(CurrentUser.iUserID);

            DataRow NewProcessorRow = ProcessorListData.NewRow();
            NewProcessorRow["UserId"] = DBNull.Value;
            NewProcessorRow["FullName"] = "All";

            ProcessorListData.Rows.InsertAt(NewProcessorRow, 0);

            this.ddlProcessor.DataSource = ProcessorListData;
            this.ddlProcessor.DataBind();
        }

        #endregion

        #region 加载Region List

        if (CurrentUser.bIsCompanyExecutive == true && CurrentUser.userRole.OtherLoanAccess == true)
        {
            #region 加载Region List

            DataTable RegionListData = this.GetRegionList();

            DataRow NewRegionRow = RegionListData.NewRow();
            NewRegionRow["RegionId"] = DBNull.Value;
            NewRegionRow["Name"] = "All";

            RegionListData.Rows.InsertAt(NewRegionRow, 0);

            this.ddlRegion.DataSource = RegionListData;
            this.ddlRegion.DataBind();

            #endregion
        }
        else
        {
            this.ddlRegion.Enabled = false;
        }

        #endregion

        #region 加载Division List

        if ((CurrentUser.bIsCompanyExecutive == true && CurrentUser.userRole.OtherLoanAccess == true)
            || (CurrentUser.bIsRegionExecutive == true && CurrentUser.userRole.OtherLoanAccess == true))
        {
            #region 加载Division List

            DataTable DivisionListData = this.GetDivisionList(sRegion);

            DataRow NewDivisionRow = DivisionListData.NewRow();
            NewDivisionRow["DivisionId"] = DBNull.Value;
            NewDivisionRow["Name"] = "All";

            DivisionListData.Rows.InsertAt(NewDivisionRow, 0);

            this.ddlDivision.DataSource = DivisionListData;
            this.ddlDivision.DataBind();

            #endregion
        }
        else
        {
            this.ddlDivision.Enabled = false;
        }

        #endregion

        #region 加载Branch List

        if ((CurrentUser.bIsCompanyExecutive == true && CurrentUser.userRole.OtherLoanAccess == true)
            || (CurrentUser.bIsRegionExecutive == true && CurrentUser.userRole.OtherLoanAccess == true)
            || (CurrentUser.bIsDivisionExecutive == true && CurrentUser.userRole.OtherLoanAccess == true))
        {
            #region 加载Branch List

            DataTable BranchListData = this.GetBranchList(sDivision);

            DataRow NewBranchRow = BranchListData.NewRow();
            NewBranchRow["BranchId"] = DBNull.Value;
            NewBranchRow["Name"] = "All";

            BranchListData.Rows.InsertAt(NewBranchRow, 0);

            this.ddlBranch.DataSource = BranchListData;
            this.ddlBranch.DataBind();

            #endregion
        }
        else
        {
            this.ddlBranch.Enabled = false;
        }

        #endregion

        #region 加载ServiceType List

        DataTable ServiceTypeListData = this.GetServiceTypeList();

        DataRow NewServiceTypeRow = ServiceTypeListData.NewRow();
        NewServiceTypeRow["ServiceTypeId"] = DBNull.Value;
        NewServiceTypeRow["Name"] = "All";

        ServiceTypeListData.Rows.InsertAt(NewServiceTypeRow, 0);

        this.ddlServiceType.DataSource = ServiceTypeListData;
        this.ddlServiceType.DataBind();

        #endregion

        #endregion

        if (this.Request.QueryString.Count == 0)
        {
            this.divSearchResultTile.Visible = false;
            this.divSearchResultContainer.Visible = false;
        }
        else
        {
            this.divActiveLoansContainer.Visible = bSearchLoans;
            this.divOpportunitiesContainer.Visible = bSearchOpportunities;
            this.divArchivedLoansContainer.Visible = bSearchArchivedLoans;
            this.divClientsContainer.Visible = bSearchClients;
            this.divPartnersContainer.Visible = bSearchPartners;

            if (bSearchLoans == true || bSearchOpportunities == true || bSearchArchivedLoans == true)
            {
                #region sWhere

                DataTable IDDataTable = new DataTable();
                IDDataTable.Columns.Add("FileId", typeof(int));

                // Name
                //if (sName != string.Empty)
                //{
                    //string sSql0 = "select distinct a.FileId from LoanContacts as a inner join Contacts as b on a.ContactId = b.ContactId "
                    //             + "where lower(b.LastName) like lower('" + SqlTextBuilder.ConvertQueryValue(sName) + "%')"
                    //             + " or lower(b.FirstName) like lower('" + SqlTextBuilder.ConvertQueryValue(sName) + "%') ";
                    //DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    ////Fixed bug 779(20110501 Rocky)
                    //if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    //{
                    //    DataRow drNew = IDs.NewRow();
                    //    drNew["FileId"] = 0;
                    //    IDs.Rows.Add(drNew);
                    //}
                    //IDDataTable.Merge(IDs);
                    
                //}

                // Company
                if (sCompany != string.Empty)
                {
                    string sSql0 = "select distinct c.FileId from Contacts as a inner join ContactCompanies as b on a.ContactCompanyId = b.ContactCompanyId "
                                 + "inner join LoanContacts as c on a.ContactId = c.ContactId "
                                 + "where lower(b.Name) like lower('" + SqlTextBuilder.ConvertQueryValue(sCompany) + "%')";
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                // Address
                if (sAddress != string.Empty)
                {
                    string sSql0 = "select FileId from Loans where lower(PropertyAddr)=lower('" + SqlTextBuilder.ConvertQueryValue(sAddress) + "')";
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                // City
                if (sCity != string.Empty)
                {
                    string sSql0 = "select FileId from Loans where lower(PropertyCity)=lower('" + SqlTextBuilder.ConvertQueryValue(sCity) + "')";
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                // State
                if (sState != string.Empty)
                {
                    string sSql0 = "select FileId from Loans where PropertyState='" + SqlTextBuilder.ConvertQueryValue(sState) + "'";
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                // Email
                if (sEmail != string.Empty)
                {
                    string sSql0 = "select distinct a.FileId from LoanContacts as a inner join Contacts as b on a.ContactId = b.ContactId "
                                 + "where lower(b.Email)=lower('" + SqlTextBuilder.ConvertQueryValue(sEmail) + "')";
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                // Phone
                if (sPhone != string.Empty)
                {
                    string sSql0 = "select distinct a.FileId from LoanContacts as a inner join Contacts as b on a.ContactId = b.ContactId "
                                 + "where b.BusinessPhone='" + SqlTextBuilder.ConvertQueryValue(sPhone) + "' or b.CellPhone='" + SqlTextBuilder.ConvertQueryValue(sPhone) + "' or b.HomePhone='" + SqlTextBuilder.ConvertQueryValue(sPhone) + "'";
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                // Loan Number
                if (sLoanNumber != string.Empty)
                {
                    string sSql0 = "select FileId from Loans where LoanNumber='" + SqlTextBuilder.ConvertQueryValue(sLoanNumber) + "'";
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                // Filename
                if (sFilename != string.Empty)
                {
                    string sSql0 = "select FileId from PointFiles where Name='" + SqlTextBuilder.ConvertQueryValue(sFilename) + "'";
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                // Loan Officer
                if (sLoanOfficer != string.Empty)
                {
                    string sSql0 = "select distinct FileId from LoanTeam where RoleId=3 and UserId=" + sLoanOfficer;
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                // Processor
                if (sProcessor != string.Empty)
                {
                    string sSql0 = "select distinct FileId from LoanTeam where RoleId=5 and UserId=" + sProcessor;
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                // Region
                if (sRegion != string.Empty)
                {
                    string sSql0 = "select a.FileId from PointFiles as a inner join GroupFolder as b on a.FolderId = b.FolderId "
                                 + "inner join Branches as c on b.GroupID = c.GroupId "
                                 + "where c.RegionID=" + sRegion;
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                // Division
                if (sDivision != string.Empty)
                {
                    string sSql0 = "select a.FileId from PointFiles as a inner join GroupFolder as b on a.FolderId = b.FolderId "
                                 + "inner join Branches as c on b.GroupID = c.GroupId "
                                 + "where c.DivisionID=" + sDivision;
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                // Branch
                if (sBranch != string.Empty)
                {
                    string sSql0 = "select a.FileId from PointFiles as a inner join GroupFolder as b on a.FolderId = b.FolderId "
                                 + "inner join Branches as c on b.GroupID = c.GroupId "
                                 + "where c.BranchID=" + sBranch;
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                // Service Type
                if (sServiceType != string.Empty)
                {
                    string sSql0 = "select a.FileId from LoanContacts as a inner join Contacts as b on a.ContactId = b.ContactId "
                                 + "inner join ContactCompanies as c on b.ContactCompanyId = c.ContactCompanyId where c.ServiceTypeId=" + sServiceType;
                    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    //Fixed bug 779(20110501 Rocky)
                    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                    {
                        DataRow drNew = IDs.NewRow();
                        drNew["FileId"] = 0;
                        IDs.Rows.Add(drNew);
                    }
                    IDDataTable.Merge(IDs);
                }

                //用户权限 gdc Bug #1669
                //if (1 == 1)
                //{
                //    string sSql0 = string.Format("  SELECT LoanID as FileId FROM dbo.[lpfn_GetUserLoans2] ('{0}', '{1}')", CurrUser.iUserID, CurrUser.bAccessOtherLoans);
                //    DataTable IDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                //    //Fixed bug 779(20110501 Rocky)
                //    if (IDs.Rows.Count == 0 && IDDataTable.Select("FileId=0").Length < 1)
                //    {
                //        DataRow drNew = IDs.NewRow();
                //        drNew["FileId"] = 0;
                //        IDs.Rows.Add(drNew);
                //    }
                //    IDDataTable.Merge(IDs);
                //}

                // distinct

                
                DataView IDDataView = new DataView(IDDataTable);
                DataTable DistinctIDList = IDDataView.ToTable(true, "FileId");

                string sWhere = string.Empty;
                if (!string.IsNullOrEmpty(sName))
                {
                    sWhere += string.Format(" and FullName like '{0}%' ", SqlTextBuilder.ConvertQueryValue(sName));
                }

                if (DistinctIDList.Rows.Count > 0)
                {
                    StringBuilder sbFileIDs = new StringBuilder();
                    for (int i = 0; i < DistinctIDList.Rows.Count; i++)
                    {
                        string sFileID = DistinctIDList.Rows[i]["FileId"].ToString();

                        //Fixed bug 779(20110501 Rocky)
                        if (sFileID == "0")
                        {
                            sbFileIDs.Append("0");
                            sWhere += " AND FileId = 0";
                            break;
                        }
                        if (i == 0)
                        {
                            sbFileIDs.Append(sFileID);
                        }
                        else
                        {
                            sbFileIDs.Append("," + sFileID);
                        }
                    }
                    //Fixed bug#114 by Rocky
                    sWhere += " AND FileId IN(" + sbFileIDs.ToString() + ")";
                }

                //用户权限 gdc Bug #1669 20120523
                if (CurrUser.bIsCompanyExecutive || CurrUser.bIsRegionExecutive || CurrUser.bIsDivisionExecutive)
                    sWhere += string.Format(" and FileId in (Select LoanId as FileId from [dbo].[lpfn_GetUserLoans_Executive] ({0})) ", CurrUser.iUserID);
                if (CurrUser.bIsBranchManager)
                    sWhere += string.Format(" and FileId in (Select LoanId as FileId from [dbo].[lpfn_GetUserLoans_Branch_Manager] ({0})) ", CurrUser.iUserID);
                else
                    sWhere += string.Format(" and FileId in (Select LoanId as FileId from [dbo].[lpfn_GetUserLoans2]({0},{1})) ", CurrUser.iUserID, (CurrUser.bAccessOtherLoans) ? 1 : 0);             
                #endregion

                #region 加载Active Loans

                if (bSearchLoans == true)
                {
                    this.ActiveLoanSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

                    int iRowCount = LPWeb.DAL.DbHelperSQL.Count(this.ActiveLoanSqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);

                    this.AspNetPager1.RecordCount = iRowCount;

                    #region Pipeline Link

                    if (iRowCount > 0)
                    {
                        string sFileIDs = this.GetFileIDs(this.ActiveLoanSqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);
                        string sFileIDs_Encode = Encrypter.Base64Encode(sFileIDs);
                        this.aPipeline_ActiveLoans.HRef = "ProcessingPipelineSummary.aspx?Ads=" + sFileIDs_Encode;
                    }
                    else
                    {
                        this.aPipeline_ActiveLoans.HRef = "javascript:alert('There is no active loan meeting the criteria.')";
                    }

                    #endregion

                    this.ActiveLoanSqlDataSource.SelectParameters["Where"].DefaultValue = sWhere;
                    this.gridActiveLoanList.DataBind();
                }

                #endregion

                #region 加载Opportunities

                if (bSearchOpportunities == true)
                {
                    this.OpportunitySqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

                    int iRowCount = LPWeb.DAL.DbHelperSQL.Count(this.OpportunitySqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);

                    this.AspNetPager2.RecordCount = iRowCount;

                    #region Pipeline Link

                    if (iRowCount > 0)
                    {
                        string sFileIDs = this.GetFileIDs(this.OpportunitySqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);
                        string sFileIDs_Encode = Encrypter.Base64Encode(sFileIDs);
                        this.aPipeline_Opportunities.HRef = "ProspectPipelineSummaryLoan.aspx?Ads=" + sFileIDs_Encode;
                    }
                    else
                    {
                        this.aPipeline_Opportunities.HRef = "javascript:alert('There is no prospect loan by criteria.')";
                    }

                    #endregion

                    this.OpportunitySqlDataSource.SelectParameters["Where"].DefaultValue = sWhere;
                    this.gridOpportunityList.DataBind();
                }

                #endregion

                #region 加载Archived Loans

                if (bSearchArchivedLoans == true)
                {
                    this.ArchivedLoanSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

                    int iRowCount = LPWeb.DAL.DbHelperSQL.Count(this.ArchivedLoanSqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);

                    this.AspNetPager3.RecordCount = iRowCount;

                    #region Pipeline Link

                    if (iRowCount > 0)
                    {
                        string sFileIDs = this.GetFileIDs(this.ArchivedLoanSqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);
                        string sFileIDs_Encode = Encrypter.Base64Encode(sFileIDs);
                        this.aPipeline_ArchivedLoans.HRef = "ProcessingPipelineSummary.aspx?Ads=" + sFileIDs_Encode + "&LoanStatus=Archived";
                    }
                    else
                    {
                        this.aPipeline_ArchivedLoans.HRef = "javascript:alert('There is no archived loan by criteria.')";
                    }

                    #endregion

                    this.ArchivedLoanSqlDataSource.SelectParameters["Where"].DefaultValue = sWhere;
                    this.gridArchivedLoanList.DataBind();
                }

                #endregion
            }

            if (bSearchClients == true)
            {

                LoadClientData();
            }
            
            if (bSearchPartners == true)
            {
                    LoadPartnerData();
            }
        }

    }
    private void LoadClientData()
    {
        string sDbTable = " (select ContactId,  LastName+', '+FirstName+ISNULL(MiddleName, '') as FullName, DOB, Email, HomePhone, CellPhone from Contacts) as t";
        //                    string sWhere0 = string.Format(@" AND (ContactRoleId=dbo.lpfn_GetBorrowerRoleId() OR ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId() )
        //						                        AND FileId in (SELECT LoanID FROM dbo.[lpfn_GetUserLoans2] ('{0}', '{1}'))", CurrUser.iUserID, CurrUser.bAccessOtherLoans);

        //string sWhere = sWhere0 + sWhere1 + sWhere2;
        string sJointTable = " select DISTINCT a.ContactId from Contacts a inner join Prospect p on p.ContactId=a.ContactId left join LoanContacts b on a.ContactId=b.ContactId left join loans l on b.FileId=l.FileId inner join PointFiles pf on b.FileId=pf.FileId inner join ContactCompanies cc on cc.ContactCompanyId=dbo.lpfn_GetContactCompanyId(a.ContactId) WHERE 1=1 ";
        string sWhere1 = string.Empty;
        string sWhere2 = string.Empty;

        if (!string.IsNullOrEmpty(sName))
            sWhere1 += string.Format(" AND (a.LastName like '{0}%' OR a.FirstName like '{0}%')", SqlTextBuilder.ConvertQueryValue(sName));
        if (!string.IsNullOrEmpty(sAddress))
            sWhere1 += string.Format(" AND (a.MailingAddr like '%{0}%' OR cc.[Address] like '%{0}%')", SqlTextBuilder.ConvertQueryValue(sAddress));
        if (!string.IsNullOrEmpty(sCity))
            sWhere1 += string.Format(" AND (a.MailingCity like '%{0}%' OR cc.[City] like '%{0}%')", SqlTextBuilder.ConvertQueryValue(sCity));
        if (!string.IsNullOrEmpty(sState))
            sWhere1 += string.Format(" AND (a.MailingState='{0}' OR cc.[State]='{0}')", SqlTextBuilder.ConvertQueryValue(sState));
        if (!string.IsNullOrEmpty(sEmail))
            sWhere1 += string.Format(" AND a.Email like'%{0}%' ", SqlTextBuilder.ConvertQueryValue(sEmail));
        if (!string.IsNullOrEmpty(sPhone))
            sWhere1 += string.Format(" AND (a.BusinessPhone like'%{0}%' OR a.HomePhone like'%{0}%' OR a.CellPhone like'%{0}%') ", SqlTextBuilder.ConvertQueryValue(sPhone));
        if (!string.IsNullOrEmpty(sLoanNumber))
            sWhere1 += string.Format(" AND l.LoanNumber like '{0}%' ", SqlTextBuilder.ConvertQueryValue(sLoanNumber));
        if (!string.IsNullOrEmpty(sRegion))
            sWhere1 += string.Format(" AND l.RegionId={0} ", sRegion);
        if (!string.IsNullOrEmpty(sDivision))
            sWhere1 += string.Format(" AND l.DivisionId={0} ", sDivision);
        if (!string.IsNullOrEmpty(sBranch))
            sWhere1 += string.Format(" AND l.BranchId={0} ", sBranch);
        if (!string.IsNullOrEmpty(sLoanOfficer))
            sWhere1 += string.Format(" AND dbo.lpfn_GetLoanOfficerId(l.FileId)={0} ", sLoanOfficer);
        if (!string.IsNullOrEmpty(sProcessor))
            sWhere1 += string.Format(" AND dbo.lpfn_GetProcessorId(l.FileId)={0} ", sProcessor);
        if (!string.IsNullOrEmpty(sFilename))
            sWhere1 += string.Format(" AND pf.[Name] like '%{0}%' ", sFilename);

        string sqlWhere1 = " AND (l.FileId in (select LoanId from {0})  ";
        string sqlWhere2 = string.Empty;
        if (CurrentUser.bIsCompanyExecutive || CurrentUser.bIsRegionExecutive || CurrentUser.bIsDivisionExecutive)
        {
            sqlWhere2 = string.Format(" dbo.[lpfn_GetUserLoans_Executive]({0})", CurrentUser.iUserID);
        }
        else if (CurrentUser.bIsBranchManager)
        {
            sqlWhere2 = string.Format(" dbo.[lpfn_GetUserLoans_Branch_Manager]({0})", CurrentUser.iUserID);
        }
        else
        {
            sqlWhere2 = string.Format(" dbo.[lpfn_GetUserLoans2]({0}, {1})", CurrentUser.iUserID, CurrentUser.bAccessOtherLoans ? 1 : 0);
        }
        string sqlWhere = string.Format(sqlWhere1, sqlWhere2);
        if (CurrUser.bAccessOtherLoans || CurrUser.userRole.AccessUnassignedLeads)
            sqlWhere += " OR l.BranchId IS NULL OR dbo.lpfn_GetLoanOfficerId(l.FileId) IS NULL ";
        sqlWhere += ")";
        string sWhere3 = sWhere1 + sWhere2 + sqlWhere;
        string sWhere = " AND ContactId in (" + sJointTable + sWhere3 + ")";

        #region get row count

        int iRowCount = this.GetClientsCount(sDbTable, sWhere);
        this.AspNetPager4.RecordCount = iRowCount;

        #endregion

        #region Calc. StartIndex and EndIndex

        int iPageSize = this.AspNetPager4.PageSize;
        int iPageIndex = 1;
        if (this.Request.QueryString["PageIndex4"] != null)
        {
            iPageIndex = Convert.ToInt32(this.Request.QueryString["PageIndex4"]);
        }
        int iStartIndex = PageCommon.CalcStartIndex(iPageIndex, iPageSize);
        int iEndIndex = PageCommon.CalcEndIndex(iStartIndex, iPageSize, iRowCount);

        #endregion

        #region 绑定Clients列表

        DataTable ClientList = this.GetClientList(sDbTable, sWhere, iStartIndex, iEndIndex);

        DataView ClientView = new DataView(ClientList);
        DataTable ClientList_Distinct = ClientView.ToTable(true, "ContactId", "FullName", "DOB", "Email", "HomePhone", "CellPhone");

        this.gridClientList.DataSource = ClientList_Distinct;
        this.gridClientList.DataBind();
        #endregion
    }

    private void LoadPartnerData()
    {
            string sDbTable = " (select ContactId, ContactCompanyId as ContactCompanyId2, LastName+', '+FirstName as FullName, CompanyName, ServiceType, BusinessPhone, Email from [dbo].[lpvw_PartnerContactsSearch] where ServiceType IS NOT NULL) as t";
            //                    string sWhere0 = string.Format(@" AND (ContactRoleId=dbo.lpfn_GetBorrowerRoleId() OR ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId() )
            //						                        AND FileId in (SELECT LoanID FROM dbo.[lpfn_GetUserLoans2] ('{0}', '{1}'))", CurrUser.iUserID, CurrUser.bAccessOtherLoans);

            //string sWhere = sWhere0 + sWhere1 + sWhere2;
            string sJointTable = " select DISTINCT a.ContactId from [dbo].[lpvw_PartnerContactsSearch] a inner join dbo.lpvw_GetLoanContactInfowRoles b on a.ContactId=b.ContactId inner join loans l on b.FileId=l.FileId inner join PointFiles pf on b.FileId=pf.FileId WHERE 1=1 ";
            string sWhere1 = string.Empty;
            string sWhere2 = string.Empty;
            
            if (!string.IsNullOrEmpty(sCompany))
                sWhere1 += string.Format(" AND a.CompanyName like '{0}%' ", sCompany);
            if (!string.IsNullOrEmpty(sServiceType))
                sWhere1 += string.Format(" AND a.ServiceTypeId='{0}' ", sServiceType);
            if (!string.IsNullOrEmpty(sName))
                sWhere1 += string.Format(" AND (a.LastName like '{0}%' OR a.FirstName like '{0}%')", SqlTextBuilder.ConvertQueryValue(sName));
            if (!string.IsNullOrEmpty(sAddress))
                sWhere1 += string.Format(" AND (a.MailingAddr like '%{0}%' OR a.CompanyAddress like '%{0}%')", SqlTextBuilder.ConvertQueryValue(sAddress));
            if (!string.IsNullOrEmpty(sCity))
                sWhere1 += string.Format(" AND (a.MailingCity like '%{0}%' OR a.CompanyCity like '%{0}%')", SqlTextBuilder.ConvertQueryValue(sCity));
            if (!string.IsNullOrEmpty(sState))
                sWhere1 += string.Format(" AND (a.MailingState='{0}' OR a.CompanyState='{0}')", SqlTextBuilder.ConvertQueryValue(sState));
            if (!string.IsNullOrEmpty(sEmail))
                sWhere1 += string.Format(" AND a.Email like'%{0}%' ", SqlTextBuilder.ConvertQueryValue(sEmail));
            if (!string.IsNullOrEmpty(sPhone))
                sWhere1 += string.Format(" AND a.BusinessPhone like'%{0}%' ", SqlTextBuilder.ConvertQueryValue(sPhone));
            if (!string.IsNullOrEmpty(sLoanNumber))
                sWhere1 += string.Format(" AND l.LoanNumber like '{0}%' ", SqlTextBuilder.ConvertQueryValue(sLoanNumber));
            if (!string.IsNullOrEmpty(sRegion))
                sWhere1 += string.Format(" AND l.RegionId={0} ", sRegion);
            if (!string.IsNullOrEmpty(sDivision))
                sWhere1 += string.Format(" AND l.DivisionId={0} ", sDivision);
            if (!string.IsNullOrEmpty(sBranch))
                sWhere1 += string.Format(" AND l.BranchId={0} ", sBranch);
            if (!string.IsNullOrEmpty(sLoanOfficer))
                sWhere1 += string.Format(" AND dbo.lpfn_GetLoanOfficerId(l.FileId)={0} ", sLoanOfficer);
            if (!string.IsNullOrEmpty(sProcessor))
                sWhere1 += string.Format(" AND dbo.lpfn_GetProcessorId(l.FileId)={0} ", sProcessor);
            if (!string.IsNullOrEmpty(sFilename))
                sWhere1 += string.Format(" AND pf.[Name] like '%{0}%' ", sFilename);

            sWhere2 += BuildUserLoanQuery();
            string sWhere3 = sWhere1 + sWhere2;
            string sWhere = " AND ContactId in (" + sJointTable + sWhere3 + ")";

            this.PartnerSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;
            this.PartnerSqlDataSource.SelectParameters["DbTable"].DefaultValue = sDbTable;


            int iRowCount = LPWeb.DAL.DbHelperSQL.Count(this.PartnerSqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);

            this.AspNetPager5.RecordCount = iRowCount;

            this.PartnerSqlDataSource.SelectParameters["Where"].DefaultValue = sWhere;
            this.gridPartnerList.DataBind();
        
    }

    private int GetClientsCount(string sDbTable, string sWhere)
    {
        // row count
        int iRowCount = LPWeb.DAL.DbHelperSQL.Count(sDbTable, sWhere);

        return iRowCount;
    }

    private DataTable GetClientList(string sDbTable, string sWhere, int iStartIndex, int iEndIndex) 
    {
        SqlCommand SqlCmd = new SqlCommand("lpsp_ExecSqlByPager");
        SqlCmd.CommandType = CommandType.StoredProcedure;

        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@OrderByField", SqlDbType.NVarChar, "FullName");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@AscOrDesc", SqlDbType.NVarChar, "asc");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Fields", SqlDbType.NVarChar, "*");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DbTable", SqlDbType.NVarChar, sDbTable);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Where", SqlDbType.NVarChar, sWhere);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@StartIndex", SqlDbType.Int, iStartIndex);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@EndIndex", SqlDbType.Int, iEndIndex);

        DataTable x = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd);

        return x;
    }

    /// <summary>
    /// get loan officer list
    /// neo 2011-04-26
    /// </summary>
    /// <param name="iLoginUserID"></param>
    /// <returns></returns>
    private DataTable GetLoanOfficerList(int iLoginUserID)
    {
        string sSql = "select distinct LastName, FirstName, LastName +', '+FirstName as FullName,UserId from dbo.lpfn_GetAllLoanOfficer(" + iLoginUserID + ") order by  LastName, FirstName";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    /// <summary>
    /// get process list
    /// neo 2011-04-26
    /// </summary>
    /// <param name="iLoginUserID"></param>
    /// <returns></returns>
    private DataTable GetProcessorList(int iLoginUserID)
    {
        string sSql = "select distinct LastName, FirstName, LastName +', '+FirstName as FullName,UserId from dbo.lpfn_GetAllProcessor(" + iLoginUserID + ") order by  LastName, FirstName";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    /// <summary>
    /// get region list
    /// neo 2011-04-26
    /// </summary>
    /// <returns></returns>
    private DataTable GetRegionList()
    {
        string sSql = "select * from Regions where Enabled=1 order by Name";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    /// <summary>
    /// get division list
    /// neo 2011-04-26
    /// </summary>
    /// <returns></returns>
    private DataTable GetDivisionList(string sRegionID)
    {
        string sWhere = string.Empty;
        if (sRegionID != string.Empty)
        {
            sWhere = " and RegionID=" + sRegionID;
        }

        string sSql = "select * from Divisions where Enabled=1 " + sWhere + " order by Name";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    /// <summary>
    /// get branch list
    /// neo 2011-04-26
    /// </summary>
    /// <returns></returns>
    private DataTable GetBranchList(string sDivisionID)
    {
        string sWhere = string.Empty;
        if (sDivisionID != string.Empty)
        {
            sWhere = " and DivisionID=" + sDivisionID;
        }

        string sSql = "select * from Branches where Enabled=1 " + sWhere + " order by Name";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    /// <summary>
    /// get service type list
    /// neo 2011-04-26
    /// </summary>
    /// <returns></returns>
    private DataTable GetServiceTypeList()
    {
        string sSql = "select * from ServiceTypes where Enabled=1 order by Name";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }
    private string BuildUserLoanQuery()
    {
        string sqlWhere = " AND l.FileId in (select LoanId from {0}) ";
        string sqlWhere2 = string.Empty;
        if (CurrentUser.bIsCompanyExecutive || CurrentUser.bIsRegionExecutive || CurrentUser.bIsDivisionExecutive)
        {
            sqlWhere2 = string.Format(" dbo.[lpfn_GetUserLoans_Executive]({0})", CurrentUser.iUserID);
        }
        else if (CurrentUser.bIsBranchManager)
        {
            sqlWhere2 = string.Format(" dbo.[lpfn_GetUserLoans_Branch_Manager]({0})", CurrentUser.iUserID);
        }
        else
        {
            sqlWhere2 = string.Format(" dbo.[lpfn_GetUserLoans2]({0}, {1})", CurrentUser.iUserID, CurrentUser.bAccessOtherLoans ? 1 : 0);
        }
        sqlWhere = string.Format(sqlWhere, sqlWhere2);
        return sqlWhere;
    }
    /// <summary>
    /// get file ids
    /// neo 2011-04-27
    /// </summary>
    /// <param name="sDbTable"></param>
    /// <param name="sWhere"></param>
    /// <returns></returns>
    private string GetFileIDs(string sDbTable, string sWhere)
    {
        string sSql = "select FileId from " + sDbTable + " where 1=1 " + sWhere;
        DataTable FileIDList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        StringBuilder sbFileIDs = new StringBuilder();
        for (int i = 0; i < FileIDList.Rows.Count; i++)
        {
            string sFileID = FileIDList.Rows[i]["FileId"].ToString();
            if (i == 0)
            {
                sbFileIDs.Append(sFileID);
            }
            else
            {
                sbFileIDs.Append("," + sFileID);
            }
        }

        return sbFileIDs.ToString();
    }
}
