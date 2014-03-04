using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Common;
using System.Text.RegularExpressions;
using System.Data;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Reports
{
    public partial class ReportParent : BasePage
    {
        LoginUser CurrentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 获取当前用户信息
            this.CurrentUser = new LoginUser();
            if(this.CurrentUser.userRole.Reports == false)
            {
                    Response.Redirect("../Unauthorize.aspx");
                    return;
            }

            #region add items to ReportType

            if (this.CurrentUser.sRoleName == "Executive")
            {
                if(this.CurrentUser.bIsCompanyExecutive == true)
                {
                    this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Region Production Goals Report", "1"));
                    this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Division Production Goals Report", "2"));
                    this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Branch Production Goals Report", "3"));
                    this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("User Production Goals Report", "4"));
                }
                else if (this.CurrentUser.bIsRegionExecutive == true)
                {
                    //this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Region Production Goals Report", "1"));
                    this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Division Production Goals Report", "2"));
                    this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Branch Production Goals Report", "3"));
                    this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("User Production Goals Report", "4"));
                }
                else if (this.CurrentUser.bIsDivisionExecutive == true)
                {
                    //this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Region Production Goals Report", "1"));
                    //this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Division Production Goals Report", "2"));
                    this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Branch Production Goals Report", "3"));
                    this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("User Production Goals Report", "4"));
                }
            }
            else if (this.CurrentUser.sRoleName == "Branch Manager")
            {
                //this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Region Production Goals Report", "1"));
                //this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Division Production Goals Report", "2"));
                //this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Branch Production Goals Report", "3"));
                this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("User Production Goals Report", "4"));
            }
            else // Regular Users
            {
                //this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Region Production Goals Report", "1"));
                //this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Division Production Goals Report", "2"));
                //this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Branch Production Goals Report", "3"));
                this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("User Production Goals Report", "4"));
            }

            //gdc CR45
            this.ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Point Pipeline Report", "5"));
            

            #endregion

            #region 参数 ReportTypeID

            string sErrorMsg = "Invalid query string, be ignored.";

            string sReportTypeID = string.Empty;
            if (this.Request.QueryString["ReportTypeID"] == null)  // 如果没有ReportType参数
            {
                this.ddlRegions.Enabled = false;
                this.ddlDivisions.Enabled = false;
                this.ddlBranches.Enabled = false;
                this.btnFilter.Disabled = true;
                //this.btnExport.Disabled = true; // commented by Peter 2010-11-30 23:37

                return;
            }

            sReportTypeID = this.Request.QueryString["ReportTypeID"].ToString();
            if (sReportTypeID != "1"
                && sReportTypeID != "2"
                && sReportTypeID != "3"
                && sReportTypeID != "4"
                && sReportTypeID != "5")//gdc CR45
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
            }

            #endregion

            int iRegionID = 0;
            int iDivisionID = 0;
            int iBranchID = 0;

            #region 参数 RegionID

            if (this.Request.QueryString["Region"] != null)
            {
                #region get region id

                string sRegionID = this.Request.QueryString["Region"].ToString();
                bool IsValid = Regex.IsMatch(sRegionID, @"^(-)?\d+$");
                if (IsValid == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
                }

                iRegionID = Convert.ToInt32(sRegionID);

                #endregion
            }

            #endregion

            #region 参数 DivisionID

            if (this.Request.QueryString["Division"] != null)
            {
                #region get division id

                string sDivisionID = this.Request.QueryString["Division"].ToString();
                bool IsValid = Regex.IsMatch(sDivisionID, @"^(-)?\d+$");
                if (IsValid == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
                }

                iDivisionID = Convert.ToInt32(sDivisionID);

                #endregion
            }

            #endregion

            #region 参数 BranchID

            if (this.Request.QueryString["Branch"] != null)
            {
                #region get branch id

                string sBranchID = this.Request.QueryString["Branch"].ToString();
                bool IsValid = Regex.IsMatch(sBranchID, @"^(-)?\d+$");
                if (IsValid == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href = window.location.pathname");
                }

                iBranchID = Convert.ToInt32(sBranchID);

                #endregion
            }

            #endregion

            #region Load Data For Filters

            if (sReportTypeID == "5")
            {
                #region ReportType = 5  Bindfilter And Select

                var ds = PageCommon.GetOrganFilter(iRegionID, iDivisionID);

                BLL.Users bllUser = new Users();

                
                
                //BindRegions
                //Regions RegionManager = new Regions();
                //DataTable RegionFilterData = RegionManager.GetUserRegions(CurrUser.iUserID);
                //DataRow NewRegionRow = RegionFilterData.NewRow();
                //NewRegionRow["RegionID"] = "-1";
                //NewRegionRow["Name"] = "All Regions";
                //RegionFilterData.Rows.InsertAt(NewRegionRow, 0);

                this.ddlRegions.DataSource = ds.Tables["Regions"]; //RegionFilterData;
                this.ddlRegions.DataBind();

                if (this.ddlRegions.Items.Count > 1 && !bllUser.IsCompanyUser(CurrUser.iUserID))
                {
                    this.ddlRegions.SelectedIndex = 1;
                }
                else
                {
                    this.ddlRegions.SelectedIndex = 0;
                }


                //BindDivision

                //Divisions DivisionManager = new Divisions();
                //DataTable DivisionFilterData = DivisionManager.GetUserDivisions(CurrUser.iUserID);
                //DataRow NewDivisionRow = DivisionFilterData.NewRow();
                //NewDivisionRow["DivisionID"] = "-1";
                //NewDivisionRow["Name"] = "All Divisions";
                //DivisionFilterData.Rows.InsertAt(NewDivisionRow, 0);

                this.ddlDivisions.DataSource = ds.Tables["Divisions"]; //DivisionFilterData;
                this.ddlDivisions.DataBind();

                if (this.ddlDivisions.Items.Count > 1 && !bllUser.IsRegionUser(CurrUser.iUserID))
                {
                    this.ddlDivisions.SelectedIndex = 1;
                }
                else
                {
                    this.ddlDivisions.SelectedIndex = 0;
                }


                //Bind

                //Branches BranchManager = new Branches();

                //DataTable BranchFilterData = BranchManager.GetUserBranches(CurrUser.iUserID);
                //DataRow NewBranchRow = BranchFilterData.NewRow();
                //NewBranchRow["BranchID"] = "-1";
                //NewBranchRow["Name"] = "All Branches";
                //BranchFilterData.Rows.InsertAt(NewBranchRow, 0);

                this.ddlBranches.DataSource = ds.Tables["Branches"]; //BranchFilterData;
                this.ddlBranches.DataBind();

                if (this.ddlBranches.Items.Count > 1 && !bllUser.IsDivisionUser(CurrUser.iUserID))
                {
                    this.ddlBranches.SelectedIndex = 1;
                }
                else
                {
                    this.ddlBranches.SelectedIndex = 0;
                }

                #endregion
            }
            else
            {
                #region ReportType = 1-4
                if (this.CurrentUser.sRoleName == "Executive")
                {
                    if (this.CurrentUser.bIsCompanyExecutive == true)
                    {
                        if (sReportTypeID == "1")    // Region Production Goals Report
                        {
                            this.ddlRegions.Enabled = true;
                            this.ddlDivisions.Enabled = false;
                            this.ddlBranches.Enabled = false;

                            this.BindData_RegionFilter();
                        }
                        else if (sReportTypeID == "2")    // Division Production Goals Report
                        {
                            this.ddlRegions.Enabled = true;
                            this.ddlDivisions.Enabled = true;
                            this.ddlBranches.Enabled = false;

                            this.BindData_RegionFilter();
                            this.BindData_DivisionFilter(this.CurrentUser.iUserID, iRegionID);
                        }
                        else if (sReportTypeID == "3"   // Branch Production Goals Report
                            || sReportTypeID == "4")    // User Production Goals Report 
                        {
                            this.ddlRegions.Enabled = true;
                            this.ddlDivisions.Enabled = true;
                            this.ddlBranches.Enabled = true;

                            this.BindData_RegionFilter();
                            this.BindData_DivisionFilter(this.CurrentUser.iUserID, iRegionID);
                            this.BindData_BranchFilter(this.CurrentUser.iUserID, iRegionID, iDivisionID);
                        }

                    }
                    else if (this.CurrentUser.bIsRegionExecutive == true)
                    {
                        if (sReportTypeID == "2")    // Division Production Goals Report
                        {
                            this.ddlRegions.Enabled = false;
                            this.ddlDivisions.Enabled = true;
                            this.ddlBranches.Enabled = false;

                            this.BindData_DivisionFilter(this.CurrentUser.iUserID, iRegionID);
                        }
                        else if (sReportTypeID == "3"   // Branch Production Goals Report
                            || sReportTypeID == "4" || sReportTypeID == "5")    // User Production Goals Report 
                        {
                            this.ddlRegions.Enabled = false;
                            this.ddlDivisions.Enabled = true;
                            this.ddlBranches.Enabled = true;

                            this.BindData_DivisionFilter(this.CurrentUser.iUserID, iRegionID);
                            this.BindData_BranchFilter(this.CurrentUser.iUserID, iRegionID, iDivisionID);
                        }
                    }
                    else if (this.CurrentUser.bIsDivisionExecutive == true)
                    {
                        if (sReportTypeID == "3"   // Branch Production Goals Report
                            || sReportTypeID == "4" || sReportTypeID == "5")    // User Production Goals Report
                        {
                            this.ddlRegions.Enabled = false;
                            this.ddlDivisions.Enabled = false;
                            this.ddlBranches.Enabled = true;

                            this.BindData_BranchFilter(this.CurrentUser.iUserID, iRegionID, iDivisionID);
                        }
                    }
                }
                else if (this.CurrentUser.sRoleName == "Branch Manager")
                {
                    this.ddlRegions.Enabled = false;
                    this.ddlDivisions.Enabled = false;
                    this.ddlBranches.Enabled = false;
                }
                else // Regular Users
                {
                    this.ddlRegions.Enabled = false;
                    this.ddlDivisions.Enabled = false;
                    this.ddlBranches.Enabled = false;
                }
                #endregion
            }
            #endregion
        }

        /// <summary>
        /// build data for region filter
        /// neo 2010-11-14
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        private DataTable BuildRegionFilterData(int iUserID) 
        {
            Regions RegionManager = new Regions();

            // load data for region filter
            DataTable RegionFilterData = RegionManager.GetRegionFilter_UserList(iUserID);
            DataRow NewRegionRow = RegionFilterData.NewRow();
            NewRegionRow["RegionID"] = "-1";
            NewRegionRow["Name"] = "All Regions";
            RegionFilterData.Rows.InsertAt(NewRegionRow, 0);

            return RegionFilterData;
        }

        /// <summary>
        /// build data for division filter
        /// neo 2010-11-14
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="iRegionID"></param>
        /// <returns></returns>
        private DataTable BuildDivisionFilterData(int iUserID, int iRegionID) 
        {
            Divisions DivisionManager = new Divisions();

            // load data for division filter
            DataTable DivisionFilterData = DivisionManager.GetDivisionFilter_UserList(iUserID, iRegionID);
            DataRow NewDivisionRow = DivisionFilterData.NewRow();
            NewDivisionRow["DivisionID"] = "-1";
            NewDivisionRow["Name"] = "All Divisions";
            DivisionFilterData.Rows.InsertAt(NewDivisionRow, 0);

            return DivisionFilterData;
        }

        /// <summary>
        /// build data for branch filter
        /// neo 2010-11-14
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="iRegionID"></param>
        /// <param name="iDivisionID"></param>
        /// <returns></returns>
        private DataTable BuildBranchFilterData(int iUserID, int iRegionID, int iDivisionID)
        {
            Branches BranchManager = new Branches();

            // load data for division filter
            DataTable BranchFilterData = BranchManager.GetBranchFilter_UserList(iUserID, iRegionID, iDivisionID);
            DataRow NewBranchRow = BranchFilterData.NewRow();
            NewBranchRow["BranchID"] = "-1";
            NewBranchRow["Name"] = "All Branches";
            BranchFilterData.Rows.InsertAt(NewBranchRow, 0);

            return BranchFilterData;
        }

        /// <summary>
        /// bind data for region filter
        /// neo 2010-11-14
        /// </summary>
        private void BindData_RegionFilter()
        {
            // load data for region filter
            DataTable RegionFilterData = this.BuildRegionFilterData(this.CurrentUser.iUserID);
            this.ddlRegions.DataSource = RegionFilterData;
            this.ddlRegions.DataBind();
        }

        /// <summary>
        /// bind data for region filter
        /// neo 2010-11-14
        /// </summary>
        private void BindData_DivisionFilter(int iUserID, int iRegionID)
        {
            // load data for division filter
            DataTable DivisionFilterData = this.BuildDivisionFilterData(iUserID, iRegionID);
            this.ddlDivisions.DataSource = DivisionFilterData;
            this.ddlDivisions.DataBind();
        }

        /// <summary>
        /// bind data for region filter
        /// neo 2010-11-14
        /// </summary>
        private void BindData_BranchFilter(int iUserID, int iRegionID, int iDivisionID)
        {
            // load data for branch filter
            DataTable BranchFilterData = this.BuildBranchFilterData(this.CurrentUser.iUserID, iRegionID, iDivisionID);
            this.ddlBranches.DataSource = BranchFilterData;
            this.ddlBranches.DataBind();
        }
    }
}
