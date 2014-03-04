using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
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
    public partial class StageTemplateSetup : BasePage
    {

        private LPWeb.BLL.Template_Stages stageMgr = new LPWeb.BLL.Template_Stages();

        private string FromPage = string.Empty;
        private int iStageID = 0;
        private bool bCustom = true;

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //权限验证
                var loginUser = new LoginUser();
                if (loginUser.userRole.WorkflowTempl.ToString() == "")
                {
                    Response.Redirect("../Unauthorize.aspx");
                    return;
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            string sErrorMsg = "Failed to load current page: invalid stage template ID.";
            string sReturnPage = "StageTemplateSetup.aspx";

            if (this.Request.QueryString["FromPage"] != null) // no task id
            {
                FromPage = this.Request.QueryString["FromPage"];

            }
            if (this.Request.QueryString["stageid"] != null) // no task id
            {
                string sStageID = this.Request.QueryString["stageid"].ToString();
                if (PageCommon.IsID(sStageID) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }

                this.iStageID = Convert.ToInt32(sStageID);

            }

            if (!IsPostBack)
            {
                try
                {
                    DoInitData();
                    LoadStageData();

                    SetControlsReadyonly(this.bCustom);

                    if (this.Request.QueryString["WorkflowType"] != null)
                    {
                        string sWorkflowType = this.Request.QueryString["WorkflowType"].ToString();
                        this.ddlType.SelectedValue = sWorkflowType;
                        this.ddlType.SelectedItem.Text = sWorkflowType;
                    }
                }
                catch (Exception ex)
                {
                    LPLog.LogMessage(LogType.Logdebug, ex.ToString());
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //CheckInput
                /*同一Workflow type下的stage Name不可重复
                /Name必输
                */
                if (!CheckInput())
                {
                    return;
                }
                this.SaveStage(this.SetStageModel());
                Response.Write("<script>window.parent.ClosePopupStage(); </script>");
                if (FromPage.Length > 0)
                    Response.Write("<script>parent.window.location.href=parent.window.location.href </script>");
            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
            }
        }
        #endregion

        #region function
        /// <summary>
        /// Init data
        /// </summary>
        private void DoInitData()
        {

            //Binding workflow type
            this.ddlType.Items.Add(new ListItem("-- select --", ""));
            this.ddlType.Items.Add(new ListItem("Processing", "Processing"));
            this.ddlType.Items.Add(new ListItem("Prospect", "Prospect"));

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bIsCustom"></param>
        private void SetControlsReadyonly(bool bIsCustom)
        {
            if (!bIsCustom)
            {
                this.tbxStageName.Enabled  = false;
                this.ddlType.Enabled = false;
                this.tbxStageName.Enabled = false;
                this.chkEnabled.Enabled = false;
            }
            else
            {
                this.tbxStageName.Enabled = true;
                this.ddlType.Enabled = true;
                this.tbxStageName.Enabled = true;
                this.chkEnabled.Enabled = true;
            }

            this.tbxAlias.Enabled = true;
            this.tbxType.Enabled = false;
            this.tbxDaysAfterCreationDate.Enabled = true;
            this.tbxDaysFromEstClose.Enabled = true;
            this.tbxPointStageDateField.Enabled = true;
            this.tbxPointStageNameField.Enabled = true;

        }

        /// <summary>
        /// load stage data base on stage id
        /// </summary>
        private void LoadStageData()
        {
            this.tbxStageName.Text = "";
            this.tbxAlias.Text = "";
            this.chkEnabled.Checked = false;
            this.ddlType.SelectedIndex = 0;
            this.tbxSequence.Text = "";
            this.tbxType.Text = "";
            this.tbxDaysAfterCreationDate.Text = "";
            this.tbxDaysFromEstClose.Text = "";
            this.tbxPointStageDateField.Text = "";
            this.tbxPointStageNameField.Text = "";

            
            LPWeb.Model.Template_Stages model = null;
            try
            {
                model = this.stageMgr.GetModel(this.iStageID);
                if (this.iStageID == 0 || model == null)
                {
                    this.tbxType.Text = "Custom";
                    this.bCustom = true;
                    this.chkEnabled.Checked = true;
                    this.tbxSequence.Text = "0";

                    this.hdnMaxIDType1.Value = Convert.ToString(this.stageMgr.GetMaxSequence("processing") + 1);
                    this.hdnMaxIDType2.Value = Convert.ToString(this.stageMgr.GetMaxSequence("prospect") + 1);
                    return;
                }
                this.tbxStageName.Text = model.Name;
                this.tbxAlias.Text = model.Alias;
                this.chkEnabled.Checked = model.Enabled;
                if (model.WorkflowType.ToLower().Trim() == "processing")
                {
                    this.ddlType.SelectedIndex = 1;
                }
                else if (model.WorkflowType.ToLower().Trim() == "prospect")
                {
                    this.ddlType.SelectedIndex = 2;
                }
                else
                {
                    this.ddlType.SelectedIndex = 1;
                }
                this.tbxSequence.Text = model.SequenceNumber.ToString();
                this.bCustom = model.Custom;
                if(model.Custom)
                {
                    this.tbxType.Text = "Custom";
                }
                else
                {
                    this.tbxType.Text = "Standard";
                }
                this.tbxDaysAfterCreationDate.Text = "";
                this.tbxDaysFromEstClose.Text = "";
                this.tbxPointStageDateField.Text = model.PointStageDateField.ToString();
                this.tbxPointStageNameField.Text = model.PointStageNameField.ToString();
                this.tbxDaysFromEstClose.Text = model.DaysFromEstClose.ToString();
                this.tbxDaysAfterCreationDate.Text = model.DaysFromCreation.ToString();
                this.hdnStageID.Value = this.iStageID.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private LPWeb.Model.Template_Stages SetStageModel()
        {
            LPWeb.Model.Template_Stages reModel = new Model.Template_Stages();
            reModel.TemplStageId = 0;

            if (this.iStageID != 0)
            {
                reModel = this.stageMgr.GetModel(this.iStageID);
            }

            reModel.Name = this.tbxStageName.Text.Trim();
            if (this.tbxAlias.Text.Trim() == "")
            {
                reModel.Alias = reModel.Name;
            }
            else
            {
                reModel.Alias = this.tbxAlias.Text.Trim() ;
            }
            reModel.WorkflowType = this.ddlType.SelectedValue;
            reModel.Enabled = this.chkEnabled.Checked;

            reModel.Custom = this.bCustom;
            if (this.tbxSequence.Text.Trim() != "")
            {
                reModel.SequenceNumber = Convert.ToInt32(this.tbxSequence.Text);
            }
            else
            {
                reModel.SequenceNumber = null;
            }
            if (this.tbxPointStageNameField.Text.Trim() != "")
            {
                reModel.PointStageNameField = Convert.ToInt32(this.tbxPointStageNameField.Text);
            }
            else
            {
                reModel.PointStageNameField = null;
            }
            if (this.tbxPointStageDateField.Text.Trim() != "")
            {
                reModel.PointStageDateField = Convert.ToInt32(this.tbxPointStageDateField.Text);
            }
            else
            {
                reModel.PointStageDateField = null;
            }
            if (this.tbxDaysFromEstClose.Text.Trim() != "")
            {
                reModel.DaysFromEstClose = Convert.ToInt32(this.tbxDaysFromEstClose.Text);
            }
            else
            {
                reModel.DaysFromEstClose = null;
            }
            if (this.tbxDaysAfterCreationDate.Text.Trim() != "")
            {
                reModel.DaysFromCreation = Convert.ToInt32(this.tbxDaysAfterCreationDate.Text);
            }
            else
            {
                reModel.DaysFromCreation = null;
            }

            return reModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stageModel"></param>
        private void SaveStage(LPWeb.Model.Template_Stages stageModel)
        {
            try
            {
                if (stageModel.TemplStageId == 0)
                {
                    this.iStageID = this.stageMgr.Add(stageModel);
                }
                else
                {
                    this.stageMgr.Update(stageModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private bool CheckInput()
        {
            if (this.hdnStageID.Value == "")
            {
                this.hdnStageID.Value = "0";
            }
            if (this.tbxStageName.Text.Trim().Length < 1)
            {
                PageCommon.AlertMsg(this, "Please enter the stage name.");
                return false;
            }
            else
            {
                try
                {
                    DataSet ds = this.stageMgr.GetList(" [Name] = '" + this.tbxStageName.Text.Trim().Replace("'", "''") + "' AND [TemplStageId]<>" + this.hdnStageID.Value + " AND [WorkflowType]='" + ddlType.SelectedItem.Text + "'");
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    { }
                    else
                    {
                        PageCommon.AlertMsg(this, "The stage name already exists.");
                        return false;
                    }
                    //Bug #1637

                    //ds = this.stageMgr.GetList(" [SequenceNumber] = '" + this.tbxSequence.Text.Trim().Replace("'", "''") + "' AND [TemplStageId]<>" + this.hdnStageID.Value + " AND [WorkflowType]='" + ddlType.SelectedItem.Text + "'");
                    //if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    //{ }
                    //else
                    //{
                    //    PageCommon.AlertMsg(this, "The sequence number has been used by another stage. ");
                    //    return false;
                    //}

                }
                catch { }
            }

            return true;
        }
        #endregion
    }
}
