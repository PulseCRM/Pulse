using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using System.Data;
using System.Text;
using LPWeb.Layouts.LPWeb.Common;
using Utilities;
using LPWeb.LP_Service;

namespace LPWeb.Settings
{
    public partial class Settings_BranchSetup : BasePage
    {
        #region
        int iBranchID = 0;
        Branches branchManager = new Branches();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //LoginUser loginUser = new LoginUser();
                //loginUser.ValidatePageVisitPermission("BranchSetup");
                //权限验证
                var loginUser = new LoginUser();
                if (!loginUser.userRole.CompanySetup)
                {
                    Response.Redirect("../Unauthorize.aspx");
                    return;
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
            if (!IsPostBack)
            {
                if (ddlState.Items.Count <= 0)
                    USStates.Init(ddlState);
                BindBranchNames();
                LPWeb.BLL.Company_General cG = new Company_General();
                this.hdnMarketingEnabled.Value = "0";
                if (cG.CheckMarketingEnabled())
                    this.hdnMarketingEnabled.Value = "1";
                string sErrorMsg = "Failed to load current page: invalid GroupID.";
                string sReturnPage = "BranchSetup.aspx";

                if (this.Request.QueryString["BranchID"] != null) // 如果有GroupID
                {
                    string sBranchID = this.Request.QueryString["BranchID"].ToString();
                    if (PageCommon.IsID(sBranchID) == false)
                    {
                        PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                    }

                    this.iBranchID = Convert.ToInt32(sBranchID);
                }
                else // 如果没有BranchID，初始化时
                {
                    // 取第一个BranchID的ID
                    if (ddlBranchName.Items.Count > 0)
                    {
                        this.iBranchID = Convert.ToInt32(ddlBranchName.Items[0].Value);
                    }
                }
                if (!branchManager.ExistBranch(iBranchID))
                {
                    return;
                }
                ViewState["branchid"] = iBranchID.ToString();
                this.ddlBranchName.SelectedValue = iBranchID.ToString();
                LoadControls();

                
            }

            iBranchID = int.Parse(ViewState["branchid"].ToString());
        }
        #endregion

        #region Function
        private void BindBranchNames()
        {
            try
            {

                ddlBranchName.DataValueField = "BranchId";
                ddlBranchName.DataTextField = "Name";
                ddlBranchName.DataSource = branchManager.GetAllList();
                ddlBranchName.DataBind();
            }
            catch
            {
            }
        }

        private void LoadControls()
        {
            BindGroups();
            FillBranch();
            BindPointFolders();
            BindPointFoldersSelection();
            BindBranchManagers();
            BindBranchManagersSelection();

            //MailChimp
            //FillMailChimp();
        }

        private void BindGroups()
        {
            try
            {
                Groups group = new Groups();
                ddlGroupAccess.DataValueField = "GroupId";
                ddlGroupAccess.DataTextField = "GroupName";
                ddlGroupAccess.DataSource = group.GetGroupsByBranchID(iBranchID);
                ddlGroupAccess.DataBind();


                var item = new System.Web.UI.WebControls.ListItem();
                item.Text = "----Select a Group ----";
                item.Value = "0";
                ddlGroupAccess.Items.Insert(0, item);
            }
            catch
            {
            }
        }

        private void FillBranch()
        {
            try
            {
                LPWeb.Model.Branches model = branchManager.GetModel(iBranchID);
                ddlBranchName.SelectedValue = iBranchID.ToString();
                ckbEnabled.Checked = model.Enabled;

                if (model.GroupID.HasValue && ddlGroupAccess.Items.FindByValue(model.GroupID.Value.ToString()) != null)
                {
                    ddlGroupAccess.SelectedValue = model.GroupID.Value.ToString();//设置选中项
                }
                else
                {
                    ddlGroupAccess.SelectedValue = "0";
                }
                txbDescription.Text = model.Desc;
                txbAddress.Text = model.BranchAddress;
                txbCity.Text = model.City;
                txbZip.Text = model.Zip;
                ddlState.Text = model.BranchState;
                txbPhone.Text = model.Phone;
                txbFax.Text = model.Fax;
                txbEmail.Text = model.Email;
                txbWebURL.Text = model.WebURL;
                chkHomeBranch.Checked = model.HomeBranch;
                chkHomeBranch.Attributes.Add("cid", "chkHomeBranch");
                chkHomeBranch.Attributes.Add("BranchID", model.BranchId.ToString());

                #region find HomeBranch = true  BranchId

                var homebranchId = 0;
                if (model.HomeBranch != true)
                {
                    var ds = branchManager.GetList(1, " HomeBranch = 1 ", "BranchId");

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        homebranchId = Convert.ToInt32(ds.Tables[0].Rows[0]["BranchId"]);
                    }
                }
                else
                {
                    homebranchId = model.BranchId;
                }

                chkHomeBranch.Attributes.Add("HomeBranchID", homebranchId.ToString());

                #endregion

                if (model.WebsiteLogo == null)
                {
                    Image1.ImageUrl = "../images/YourLogo.jpg";
                }
                else if (model.WebsiteLogo.Length < 1)
                {
                    Image1.ImageUrl = "../images/YourLogo.jpg";
                }
                else
                {
                    Image1.ImageUrl = "BranchLogo.aspx?BranchID=" + iBranchID.ToString() + "&ss=" + DateTime.Now.Millisecond.ToString();
                }

                txbLicense1.Text = model.License1;
                txbLicense2.Text = model.License2;
                txbLicense3.Text = model.License3;
                txbLicense4.Text = model.License4;
                txbLicense5.Text = model.License5;
                txbDisclaimer.Text = model.Disclaimer;

            }
            catch
            {

            }
        }

        private void BindBranchManager()
        {
            try
            {
                Users UserManager = new Users();
                DataTable GroupMembers = UserManager.GetBranchManager(iBranchID.ToString());
                this.gridUserList.DataSource = GroupMembers;
                this.gridUserList.DataBind();
            }
            catch
            { }
        }

        private void BindPointFolders()
        {
            PointFolders folder = new PointFolders();
            try
            {
                string strWhere = string.Format(" PointFolders.BranchID = {0} order by [Name] asc ", iBranchID);
                DataSet ds = folder.GetList(strWhere);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    //ds=null;
                }

                gridPointFolderList.AutoGenerateColumns = false;
                gridPointFolderList.DataSource = ds.Tables[0];
                gridPointFolderList.DataBind();
            }
            catch
            { }
        }

        private void BindPointFoldersSelection()
        {
            PointFolders folder = new PointFolders();
            try
            {
                string strWhere = string.Format(" BranchID is null or BranchID = {0} order by [Name] asc ", iBranchID);
                DataSet ds = folder.GetList(strWhere);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    //ds=null;
                }

                gridPointFolderSelectionList.AutoGenerateColumns = false;
                gridPointFolderSelectionList.DataSource = ds.Tables[0];
                gridPointFolderSelectionList.DataBind();
            }
            catch
            { }
        }

        private void BindBranchManagers()
        {
            try
            {
                Users user = new Users();
                DataTable dt = user.GetBranchManager(iBranchID.ToString());

                gridUserList.AutoGenerateColumns = false;
                gridUserList.DataSource = dt;
                gridUserList.DataBind();
            }
            catch
            {
            }
        }

        private void BindBranchManagersSelection()
        {
            try
            {
                DataTable dt = branchManager.GetBranchManagerSeletion();

                gridUserSelectionList.AutoGenerateColumns = false;
                gridUserSelectionList.DataSource = dt;
                gridUserSelectionList.DataBind();
            }
            catch
            { }
        }
        #endregion

        #region Event
        protected void btnSave_Click(object sender, EventArgs e)
        {

            #region set Default 
            if (!string.IsNullOrEmpty(hifId.Value))
            {
                bool IsCancel = false;

                var val = hifId.Value.Split(':');

                if (val.Count() != 2)
                {
                    return;
                }
                IsCancel = Convert.ToBoolean(val[1]);

                PointFolders folder = new PointFolders();
                folder.SetDefaultPoint(val[0], this.iBranchID, IsCancel); 
            }
            
            #endregion

            bool bEnabled = this.ckbEnabled.Checked;
            string sDesc = this.txbDescription.Text.Trim();
            string sFolderIDs = this.hdnFolderIDs.Value;
            string sManagers = this.hdnManagers.Value;


            LPWeb.Model.Branches model = new LPWeb.Model.Branches();
            model = branchManager.GetModel(iBranchID);
            int iOldGroupID = Convert.ToInt32(model.GroupID);
            model.BranchId = iBranchID;
            model.Enabled = ckbEnabled.Checked;
            model.Desc = txbDescription.Text.Trim();
            model.Name = ddlBranchName.SelectedItem.Text;
            if (ddlGroupAccess.Items.Count > 0)
            {
                model.GroupID = int.Parse(ddlGroupAccess.SelectedValue);
            }
            else
            {
                model.GroupID = 0;
            }
            model.BranchAddress = txbAddress.Text.Trim();
            model.City = txbCity.Text.Trim();
            model.Zip = txbZip.Text.Trim();
            model.BranchState = ddlState.SelectedValue.Trim();

            if (this.fuldWebLogo.HasFile && this.fuldWebLogo.FileName.ToUpper().IndexOf(".TIF") < 0)
            {
                model.WebsiteLogo = fuldWebLogo.FileBytes;
            }
            else
            {
                model.WebsiteLogo = null;
            }
            model.License1 = txbLicense1.Text.Trim();
            model.License2 = txbLicense2.Text.Trim();
            model.License3 = txbLicense3.Text.Trim();
            model.License4 = txbLicense4.Text.Trim();
            model.License5 = txbLicense5.Text.Trim();

            model.Disclaimer = txbDisclaimer.Text.Trim();

            model.Phone = txbPhone.Text.Trim();
            model.Fax = txbFax.Text.Trim();
            model.Email = txbEmail.Text.Trim();
            model.WebURL = txbWebURL.Text.Trim();

            var oldHomeBranch = model.HomeBranch;
            model.HomeBranch = chkHomeBranch.Checked;
           

            try
            {
                if (oldHomeBranch != model.HomeBranch)
                {
                   branchManager.SetOtherHomeBranchFalse(model.BranchId);
                }
                branchManager.SaveBranchAndMembersBase(model, sFolderIDs, sManagers);
                //Save point folder enable status
                PointFolders pointFolderMgr = new PointFolders();
                if (this.hdnDisableFolderIDs.Value.Trim() != "")
                {
                    pointFolderMgr.UpdatePointFolderEnabled(this.hdnDisableFolderIDs.Value, false);
                }
                if (this.hdnEnableFolderIDs.Value.Trim() != "")
                {
                    pointFolderMgr.UpdatePointFolderEnabled(this.hdnEnableFolderIDs.Value, true);
                }
                //this.divManager.SaveDivisionAndMembersBase(this.iDivisionID, bEnabled, sDesc, sBranchMemberIDs, sExectives);
                //Save group folder info
                model = branchManager.GetModel(model.BranchId);
                GroupFolder groupFolder = new GroupFolder();
                if (model.GroupID != 0)
                {
                    groupFolder.DoSaveGroupFolder(Convert.ToInt32(model.GroupID), model.BranchId, "branch", iOldGroupID);
                }

                if (model.RegionID != 0 && model.RegionID != null)
                {
                    Regions regMgr = new Regions();
                    LPWeb.Model.Regions regionModel = regMgr.GetModel(Convert.ToInt32(model.RegionID));
                    if (regionModel.GroupID != null && regionModel.GroupID != 0)
                    {
                        groupFolder.DoSaveGroupFolder(Convert.ToInt32(regionModel.GroupID), Convert.ToInt32(model.RegionID), "region", Convert.ToInt32(regionModel.GroupID));
                    }
                }
                if (model.DivisionID != 0 && model.DivisionID != null)
                {
                    Divisions divMgr = new Divisions();
                    LPWeb.Model.Divisions divModel = divMgr.GetModel(Convert.ToInt32(model.DivisionID));
                    if (divModel.GroupID != null && divModel.GroupID != 0)
                    {
                        groupFolder.DoSaveGroupFolder(Convert.ToInt32(divModel.GroupID), Convert.ToInt32(model.DivisionID), "division", Convert.ToInt32(divModel.GroupID));
                    }
                }
                PageCommon.WriteJsEnd(this, "Branch saved successfully.", PageCommon.Js_RefreshSelf);
            }
            catch
            {
                PageCommon.WriteJsEnd(this, "Failed to save the record.", PageCommon.Js_RefreshSelf);
            }

        }

        protected void gridPointFolderList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("DropDownList1");
                if (ddl != null)
                {
                    ddl.SelectedValue = e.Row.Cells[0].Text;
                    ddl.Attributes.Add("cid", "DropDownList1");
                }

                CheckBox cb = (CheckBox)e.Row.FindControl("cbDefault");
                if (ddl.SelectedValue.Trim() == "1")
                {
                    if (e.Row.Cells[1].Text.Trim().ToLower() == "true" )
                    {
                        cb.Checked = true;
                    }
                    else
                    {
                        cb.Checked = false;
                    }
                    
                    cb.Enabled = true;
                }
                else
                {
                    cb.Checked = false;
                    cb.Enabled = false;
                }
                cb.ToolTip = e.Row.Cells[2].Text;
                cb.Attributes.Add("cid", "default");

            }
        }

        protected void gridPointFolderSelectionList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("DropDownList1");
                if (ddl != null)
                {
                    ddl.SelectedValue = e.Row.Cells[0].Text;
                }
            }
        }

        protected void btnSetDeault_Click(object sender, EventArgs e)
        {
            //Response.Write(hifId.Value);
            if (string.IsNullOrEmpty(hifId.Value))
            {
                return;
            }
            bool IsCancel = false;

            var val = hifId.Value.Split(':');

            if (val.Count() != 2)
            {
                return;
            }
            IsCancel = Convert.ToBoolean(val[1]);

            PointFolders folder = new PointFolders();
            folder.SetDefaultPoint(val[0], this.iBranchID, IsCancel);

            BindPointFolders();
        }
        #endregion


        //private void FillMailChimp()
        //{
        //    LPWeb.Model.Branches model = new LPWeb.Model.Branches();
        //    model = branchManager.GetModel(iBranchID);

        //    cbEnableMailChimp.Checked = model.EnableMailChimp;

        //    txbMCKey.Text = model.MailChimpAPIKey;

        //    if (cbEnableMailChimp.Checked)
        //    {
        //        txbMCKey.Enabled = true;

        //        if (model.MailChimpAPIKey != null )
        //        {
        //            btnSync.Disabled = false;
        //        }
        //        else
        //        {
        //            btnSync.Disabled = true;
        //        }
        //    }
        //    else
        //    {
        //        txbMCKey.Enabled = false;
        //        btnSync.Disabled = true;
        //    }
        //}

        //protected void btnSaveMailChimp_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        LPWeb.Model.Branches model = new LPWeb.Model.Branches();
        //        model.BranchId = iBranchID;
        //        model.EnableMailChimp = cbEnableMailChimp.Checked;
        //        model.MailChimpAPIKey = txbMCKey.Text.Trim();

        //        branchManager.UpdateChimpAPIKey(model);
        //    }
        //    catch
        //    {

        //    }
        //}

    }
}