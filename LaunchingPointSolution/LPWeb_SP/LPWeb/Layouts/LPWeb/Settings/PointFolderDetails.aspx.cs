using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using LPWeb.Common;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Settings
{
    public partial class Settings_PointFolderDetails : BasePage
    {
        int iFolderID = 0;
        PointFolders folderMgr = new PointFolders();

        #region events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //权限验证
            var loginUser = new LoginUser();
            if (!loginUser.userRole.CompanySetup)
            {
                Response.Redirect("../Unauthorize.aspx");
                return;
            }
            cbdigitsName.Attributes.Add("onclick", "javascript:CheckChkBox(this,document.getElementById('" + cbdigitsName.ClientID + "'));");

            //cbEnableAutoFileNaming.Attributes.Add("onclick", "javascript:CheckChkBox1(this,document.getElementById('" + cbEnableAutoFileNaming.ClientID + "'));");

         

            //btnSave.Attributes.Add("onclick", "javascript:BeforeSave();");



            string sErrorMsg = "Failed to load current page: invalid point folder ID.";
            string sReturnPage = "PointFolderList.aspx";

            try
            {
                if (this.Request.QueryString["PointFolderID"] != null) // no folder id
                {
                    string sFolderID = this.Request.QueryString["PointFolderID"].ToString();
                    if (PageCommon.IsID(sFolderID) == false)
                    {
                        PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                    }

                    this.iFolderID = Convert.ToInt32(sFolderID);

                }

                if (!IsPostBack)
                {
                    // From Page
                    string sFromPage = this.Request.QueryString["FromPage"].ToString();

                    hfPageFrom.Value = sFromPage;
                   

                    LoadControls();

                }

            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (cbEnableAutoFileNaming.Checked && string.IsNullOrEmpty(txbPrefix.Text.Trim()))
            {
                PageCommon.AlertMsg(this.Page, "The Prefix field cannot be empty.");
                return;
            }

            string digits = "";
            if (cbdigitsName.Checked)
            {
               digits=txtdigits.Text.Trim();
            }

          


            BLL.PointFolders bll = new PointFolders();


            bll.UpdatePointFolderAutoNaming(this.iFolderID, cbEnableAutoFileNaming.Checked, txbPrefix.Text.Trim(), cbdigitsName.Checked,digits);

            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContent').hide();alert('Save successfully.');window.parent.location.href=window.parent.location.href;", true);
        

            //Page.ClientScript.RegisterClientScriptBlock(GetType(), "_Success", "<script>alert('Save successfully.');document.location='/_layouts/LPWeb/Settings/PointFolderDetails.aspx?PointFolderID='" + this.iFolderID + "'</script>",true);

            //Response.Write(" < script language=javascript>alert('Save successfully.');window.location.reload(true ); < /script>");


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadControls();
            Response.Redirect("/_Layouts/LPWeb/Settings/PointFolderList.aspx");
            
        }

        #endregion

        #region functions


        private void LoadControls()
        {
            this.tbxFolderName.Text = "";
            this.tbxBranch.Text = "";
            this.tbxImportCount.Text = "";
            this.tbxLastImportDate.Text = "";
            this.tbxPath.Text = "";
            this.tbxEnabled.Text = "";
            this.gridSyncLogList.DataSource = null;

            LPWeb.Model.PointFolders model = null;
            try
            {
                model = this.folderMgr.GePonitFolderModel(this.iFolderID);
                if (model == null)
                {
                    return;
                }
                this.tbxFolderName.Text = model.Name;
                this.tbxBranch.Text = model.BranchName;
                this.tbxImportCount.Text = model.ImportCount.ToString();
                if (model.LastImport != null)
                {
                    this.tbxLastImportDate.Text = Convert.ToDateTime(model.LastImport).ToString("MM/dd/yyyy HH:mm:ss");
                }
                this.tbxPath.Text = model.Path;
                this.tbxEnabled.Text = model.Enabled.ToString();

                this.cbEnableAutoFileNaming.Checked = model.AutoNaming;

             


                if (model.PreFix != null)
                {
                    this.txbPrefix.Text = model.PreFix.ToString();
                }

                this.cbdigitsName.Checked = model.RandomFileNaming;

             

                if (model.FilenameLength != null)
                {
                    this.txtdigits.Text = model.FilenameLength.ToString();
                }

                #region Get sync log
                PointImportHistory importHistory = new PointImportHistory();
                DataSet dsSyncLog = importHistory.GetList(" FileId IN(SELECT FileId FROM PointFiles WHERE FolderId =" + this.iFolderID.ToString() + ")");

                this.gridSyncLogList.DataSource = dsSyncLog.Tables[0];
                this.gridSyncLogList.DataBind();

                

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        #endregion
    }
}