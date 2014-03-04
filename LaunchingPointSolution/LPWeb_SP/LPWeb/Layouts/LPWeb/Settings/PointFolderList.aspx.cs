using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.DAL;
using System.Data;
using Utilities;
using LPWeb.LP_Service;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Settings
{
    public partial class Settings_PointFolderList : BasePage
    {
        #region Event

        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (this.IsPostBack == false)
                {
                    this.FolderSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;
                    //权限验证
                    var loginUser = new LoginUser();
                    if (!loginUser.userRole.CompanySetup)
                    {
                        Response.Redirect("../Unauthorize.aspx");
                        return;
                    }
                    else
                    {
                        if (!loginUser.userRole.ImportLoan)
                        {
                            lbtnSync.Enabled = false;
                            lbtnSuspend.Enabled = false;
                        }
                    }

                    this.DoInitData();

                    this.BindingGrid();
                }
            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRegion.SelectedIndex < 0)
            {
                return;
            }
            try
            {
                this.ddlDivision.AutoPostBack = false;
                if (ddlRegion.SelectedValue == "0")
                {
                    string sDivisionID = this.ddlDivision.SelectedValue;
                    string sBranchID = this.ddlBranch.SelectedValue;
                    this.ddlDivision.DataSource = this.GetDivisionData("0");
                    this.ddlBranch.DataSource = this.GetBranchData("0", "0");
                    this.ddlDivision.DataBind();
                    this.ddlBranch.DataBind();

                    this.ddlDivision.SelectedValue = sDivisionID;
                    this.ddlBranch.SelectedValue = sBranchID;
                }
                else
                {
                    this.ddlDivision.DataSource = this.GetDivisionData(this.ddlRegion.SelectedValue);
                    this.ddlBranch.DataSource = this.GetBranchData(this.ddlRegion.SelectedValue, "0");
                    this.ddlDivision.DataBind();
                    this.ddlBranch.DataBind();
                }
                this.ddlDivision.AutoPostBack = true;
            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDivision.SelectedIndex < 0)
            {
                return;
            }
            try
            {
                if (ddlDivision.SelectedValue == "0")
                {
                    string sBranchID = this.ddlBranch.SelectedValue;
                    this.ddlBranch.DataSource = this.GetBranchData(this.ddlRegion.SelectedValue, "0");
                    this.ddlBranch.DataBind();
                    this.ddlBranch.SelectedValue = sBranchID;
                }
                else
                {
                    this.ddlBranch.DataSource = this.GetBranchData(this.ddlRegion.SelectedValue, this.ddlDivision.SelectedValue);
                    this.ddlBranch.DataBind();
                }
            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
            }

        }

        protected void lbtnSync_Click(object sender, EventArgs e)
        {
            string FolderPaths = hdnFolderPaths.Value;
            string[] paths = FolderPaths.Split(",".ToArray());
            string err = "";
            if (paths.Length < 1)
            {
                return;
            }
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient service = sm.StartServiceClient())
                {
                   ImportAllLoansRequest req = new ImportAllLoansRequest();
                    req.hdr = new ReqHdr();
                    req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                    req.hdr.UserId = 5;//todo:check dummy data 
                    req.PointFolders = paths;
                    ImportAllLoansResponse respone = null;

                    try
                    {
                        respone = service.ImportAllLoans(req);

                        if (respone.hdr.Successful)
                        {
                            PageCommon.WriteJsEnd(this, "Sync Point Folder(s) successfully.", PageCommon.Js_RefreshSelf);
                        }
                        else
                        {
                            PageCommon.WriteJsEnd(this, "Failed to sync Point Folder(s), reason: "+respone.hdr.StatusInfo, PageCommon.Js_RefreshSelf);
                        }
                    }
                    catch (System.ServiceModel.EndpointNotFoundException ee)
                    {
                        LPLog.LogMessage(ee.Message);
                        PageCommon.AlertMsg(this, "Failed to Import Point Folders, reason: Point Manager is not running.");
                    }
                    catch (Exception exception)
                    {
                        err = "Failed to sync Point folder(s), reason: " + exception.Message;
                        LPLog.LogMessage(err);
                        PageCommon.WriteJsEnd(this, err, PageCommon.Js_RefreshSelf);
                    }
                }
            }
            catch (Exception ex)
            {
                err = "Failed to sync Point Folder(s), reason: " + ex.Message;
                LPLog.LogMessage(ex.Message);
                PageCommon.WriteJsEnd(this, err, PageCommon.Js_RefreshSelf);
            }
        }
 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridFolderList_Sorting(object sender, GridViewSortEventArgs e)
        { 
            this.BindingGrid();            
        }

        protected void lbtnSuspend_Click(object sender, EventArgs e)
        {
            string err = "Failed to ";
            
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient service = sm.StartServiceClient())
                {
                    if (this.lbtnSuspend.Text.ToLower().IndexOf("suspend") >= 0)
                    {
                        err += "suspend ";
                        StopPointImportRequest req = new StopPointImportRequest();
                        req.hdr = new ReqHdr();
                        req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                        req.hdr.UserId = 5;//todo:check dummy data 
                        StopPointImportResponse respone = null;
                        try
                        {
                            respone = service.StopPointImportService(req);                      
                            if (respone.hdr.Successful)
                            {
                                this.lbtnSuspend.Text = "Resume Sync";
                                PageCommon.WriteJsEnd(this, "Suspend sync successfully.", PageCommon.Js_RefreshSelf);
                            }
                            else
                            {
                                PageCommon.WriteJsEnd(this, "Suspend sync failed, reason: "+ respone.hdr.StatusInfo, PageCommon.Js_RefreshSelf);
                            }
                        }
                        catch (System.ServiceModel.EndpointNotFoundException ee)
                        {
                            LPLog.LogMessage(ee.Message);
                            PageCommon.AlertMsg(this, "Failed to Suspend sync, reason: Point Manager is not running.");
                        }
                        catch (Exception exception)
                        {
                            err = "Failed to suspend sync, reason: " + exception.Message;
                            LPLog.LogMessage(err);
                            PageCommon.WriteJsEnd(this, err, PageCommon.Js_RefreshSelf);
                        }
                    }
                    else if (this.lbtnSuspend.Text.ToLower().IndexOf("resume") >= 0)
                    {
                        err += "resume ";
                        StartPointImportRequest req = new StartPointImportRequest();
                        req.hdr = new ReqHdr();
                        req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                        req.hdr.UserId = 5;//todo:check dummy data 
                        StartPointImportResponse respone = null;
                                                
                        try
                        {
                            respone = service.StartPointImportService(req);

                            if (respone.hdr.Successful)
                            {
                                this.lbtnSuspend.Text = "Suspend Sync";
                                PageCommon.WriteJsEnd(this, "Resume sync successfully.", PageCommon.Js_RefreshSelf);
                            }
                            else
                            {
                                PageCommon.WriteJsEnd(this, "Resume sync failed.", PageCommon.Js_RefreshSelf);
                            }
                        }
                        catch (System.ServiceModel.EndpointNotFoundException ee)
                        {
                            err = "Failed to suspend sync, reason: " + ee.Message;
                            LPLog.LogMessage(err);
                            PageCommon.AlertMsg(this, "Failed to Resume sync, reason: Point Manager is not running.");
                        }
                        catch (Exception exception)
                        {
                            err = "Failed to suspend sync, reason: " + exception.Message; 
                            LPLog.LogMessage(err);
                            PageCommon.WriteJsEnd(this, err, PageCommon.Js_RefreshSelf);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                err += "sync, reason: " + ex.Message;
                LPLog.LogMessage(LogType.Logerror, err);
                PageCommon.WriteJsEnd(this, err, PageCommon.Js_RefreshSelf);
            }
        }
        #endregion

        #region function
        /// <summary>
        /// load dropdown list data
        /// </summary>
        private void DoInitData()
        {

            try
            {
                int? intRegionID = null;
                int? intDivisionID = null;
                string sRegionID = "0";
                if (this.Request.QueryString["Region"] != null)
                {
                    sRegionID = this.Request.QueryString["Region"].ToString();
                    intRegionID = Convert.ToInt32(sRegionID);
                }
                string sDivisionID = "0";
                if (this.Request.QueryString["Division"] != null)
                {
                    sDivisionID = this.Request.QueryString["Division"].ToString();
                    intDivisionID = Convert.ToInt32(sDivisionID);
                }
                DataSet dsFilter = PageCommon.GetOrgStructureDataSourceByLoginUser(intRegionID, intDivisionID,true);
                this.ddlRegion.DataSource = dsFilter.Tables[0];
                this.ddlRegion.DataValueField = "RegionId";
                this.ddlRegion.DataTextField = "Name";
                this.ddlRegion.DataBind();
                this.ddlRegion.Attributes.Add("onchange", "send_request('Region')");
                //this.ddlDivision.DataSource = this.GetDivisionData(sRegionID);
                this.ddlDivision.DataSource = dsFilter.Tables[1];
                this.ddlDivision.DataValueField = "DivisionId";
                this.ddlDivision.DataTextField = "Name";
                this.ddlDivision.DataBind();
                this.ddlDivision.Attributes.Add("onchange", "send_request('Division')");

                //this.ddlBranch.DataSource = this.GetBranchData(sRegionID, sDivisionID);
                this.ddlBranch.DataSource = dsFilter.Tables[2];
                this.ddlBranch.DataValueField = "BranchId";
                this.ddlBranch.DataTextField = "Name";
                this.ddlBranch.DataBind();

                this.ddlRegion.SelectedIndex = 0;
                this.ddlDivision.SelectedIndex = 0;
                this.ddlBranch.SelectedIndex = 0;

                #region set suspend text

                try
                {
                    ServiceManager sm = new ServiceManager();
                    using (LP2ServiceClient service = sm.StartServiceClient())
                    {
                        GetPointMgrStatusRequest req = new GetPointMgrStatusRequest();

                        req.hdr = new ReqHdr();
                        req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                        req.hdr.UserId = 5;//todo:check dummy data 
                        GetPointMgrStatusResponse respone = null;

                        respone = service.GetPointManagerStatus(req);

                        if (respone.Running)
                        {
                            this.lbtnSuspend.Text = "Suspend Sync";
                        }
                        else
                        {
                            this.lbtnSuspend.Text = "Resume Sync";
                        }
                    }
                }
                catch (System.ServiceModel.EndpointNotFoundException ee)
                {
                    LPLog.LogMessage(ee.Message);
                    PageCommon.AlertMsg(this, "Failed reason: Point Manager is not running.");
                    return;
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                    return;
                    //PageCommon.WriteJsEnd(this, "Import Point Folders Failed.", PageCommon.Js_RefreshSelf);
                }

                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Region data
        /// </summary>
        /// <returns></returns>
        private DataTable GetRegionData()
        {
            DataTable dtRegion;
            try
            {
                Regions regionManager = new Regions();
                //Binding Region
                dtRegion = regionManager.GetList(" Enabled='true'").Tables[0];

                DataRow drNewRegion = dtRegion.NewRow();
                drNewRegion["RegionId"] = 0;
                drNewRegion["Name"] = "All Regions";
                dtRegion.Rows.InsertAt(drNewRegion, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtRegion;
        }

        /// <summary>
        /// Get division data
        /// </summary>
        /// <returns></returns>
        private DataTable GetDivisionData(string sRegionID)
        {
            DataTable dtDivision;
            try
            {
                Divisions divisionManager = new Divisions();
                //Binding Division
                string sCondition = " Enabled='true'";
                if (sRegionID != "" && sRegionID != "0")
                {
                    sCondition += " AND RegionID = " + sRegionID;
                }
                dtDivision = divisionManager.GetList(sCondition).Tables[0];

                DataRow drNewDivision = dtDivision.NewRow();
                drNewDivision["DivisionId"] = 0;
                drNewDivision["Name"] = "All Divisions";
                dtDivision.Rows.InsertAt(drNewDivision, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtDivision;
        }

        /// <summary>
        /// Get Branch data
        /// </summary>
        /// <returns></returns>
        private DataTable GetBranchData(string sRegionID, string sDivisionID)
        {
            DataTable dtBranches;
            try
            {
                Branches branchManager = new Branches(); ;
                //Binding Branch
                string sCondition = " Enabled='true'";
                if (sRegionID != "" && sRegionID != "0")
                {
                    sCondition += " AND RegionID = " + sRegionID;
                }
                if (sDivisionID != "" && sDivisionID != "0")
                {
                    sCondition += " AND DivisionID = " + sDivisionID;
                }
                dtBranches = branchManager.GetList(sCondition).Tables[0];

                DataRow drNewBranches = dtBranches.NewRow();
                drNewBranches["BranchId"] = 0;
                drNewBranches["Name"] = "All Branches";
                dtBranches.Rows.InsertAt(drNewBranches, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtBranches;
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindingGrid()
        {
            try
            {
                this.FolderSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;
                string sWhere = "";
                #region Default condition(based on user role)
                if (CurrUser.bIsCompanyExecutive)
                {
                    sWhere = "";
                }
                else if (CurrUser.bIsRegionExecutive)
                {
                    sWhere = " AND (RegionID IN(SELECT RegionId FROM RegionExecutives WHERE ExecutiveId =" + CurrUser.iUserID.ToString() + "))";
                }
                else if (CurrUser.bIsDivisionExecutive)
                {
                    sWhere = " AND (DivisionID IN(SELECT DivisionID FROM DivisionExecutives WHERE ExecutiveId =" + CurrUser.iUserID.ToString() + "))";
                }
                else if (CurrUser.bIsBranchManager)
                {
                    //sWhere = " AND (BranchId IN(SELECT DivisionID FROM BranchManagers WHERE BranchMgrId =" + CurrUser.iUserID.ToString() + "))";

                    sWhere = " AND (BranchId IN(SELECT BranchId FROM BranchManagers WHERE BranchMgrId =" + CurrUser.iUserID.ToString() + "))";
                }
                else
                {
                    sWhere = " AND (FolderId IN(SELECT FolderId FROM GroupFolder WHERE GroupId IN(SELECT GroupID FROM GroupUsers WHERE UserID = " + CurrUser.iUserID.ToString() + ")))";
                }

                #endregion
                bool bSetWhere = false;

                // Region
                if (this.Request.QueryString["Region"] != null && this.Request.QueryString["Region"].ToString() != "0" && this.Request.QueryString["Region"].ToString() != "-1")
                {
                    string sRegion = this.Request.QueryString["Region"].ToString();
                    sWhere += " AND (RegionID = " + sRegion + ")";
                    bSetWhere = true;
                }
                // Division
                if (this.Request.QueryString["Division"] != null && this.Request.QueryString["Division"].ToString() != "0" && this.Request.QueryString["Division"].ToString() != "-1")
                {
                    string sDivision = this.Request.QueryString["Division"].ToString();
                    sWhere += " AND (DivisionID = " + sDivision + ")";
                    bSetWhere = true;
                }
                // Branch
                if (this.Request.QueryString["Branch"] != null && this.Request.QueryString["Branch"].ToString() != "0" && this.Request.QueryString["Branch"].ToString() != "-1")
                {
                    string sBranch = this.Request.QueryString["Branch"].ToString();
                    sWhere += " AND (BranchId = " + sBranch + ")";
                    bSetWhere = true;
                }
                //Alphabet
                if (this.Request.QueryString["Alphabet"] != null && this.Request.QueryString["Alphabet"].ToString() != "")
                {
                    sWhere += " AND (Name LIKE '" + this.Request.QueryString["Alphabet"].ToString() + "%')";
                    bSetWhere = true;
                }

                // Get data number
                int iCount = DbHelperSQL.Count(this.FolderSqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);
                this.AspNetPager1.RecordCount = iCount;

                // No data
                if (bSetWhere == true && iCount == 0)
                {
                    //set message
                    this.gridFolderList.EmptyDataText = "There is no point folders by your conditions.";
                }
                else
                {
                    //data bing
                    this.FolderSqlDataSource.SelectParameters["Where"].DefaultValue = sWhere;
                    this.gridFolderList.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetDefSearchWhere()
        {
            string sWhere = "";

            try
            {
                var iCurUserId = new LoginUser().iUserID;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sWhere;
        }
        #endregion

    }
}