using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using LPWeb.Common;
using System.Data;
using System.Linq;

namespace LPWeb.Layouts.LPWeb.Settings
{
    public partial class PersonalizationLoansViewtTab : BasePage
    {
        BLL.UserPipelineViews userPipelineViewsBll = new BLL.UserPipelineViews();
        BLL.UserHomePref UserHomePrefManager = new BLL.UserHomePref();
        BLL.UserPipelineColumns UserPipelineColsManager = new BLL.UserPipelineColumns();
        Model.UserHomePref userHomePref = null;

        BLL.UserLoansViewPointFields userLoansViewPointFieldsBLL = new BLL.UserLoansViewPointFields();
        protected void Page_Load(object sender, EventArgs e)
        {

            #region 获取Select Pipeline Point Fields

            // 未分配的和已分配到当前Group中的User
            BLL.CompanyLoanPointFields CompanyLoanPointFields = new BLL.CompanyLoanPointFields();
            DataTable dtCompanyLoanPointFieldsInfo = CompanyLoanPointFields.GetCompanyLoanPointFieldsInfo();
            this.gridPipelinePointFieldsList.DataSource = dtCompanyLoanPointFieldsInfo;
            this.gridPipelinePointFieldsList.DataBind();


            #endregion

            
            if (!IsPostBack)
            {
                userHomePref = UserHomePrefManager.GetModel(CurrUser.iUserID);
                if (userHomePref == null)
                {
                    userHomePref = new Model.UserHomePref();
                }
                Model.UserPipelineColumns userPipelineCols = UserPipelineColsManager.GetModel(CurrUser.iUserID);
                BindView();
                if (null != userPipelineCols)
                    SetUserPipelineColumnsInfo(userPipelineCols);



                BindList();
            }

            

           

        }


        public void BindList()
        {
            var dt = userLoansViewPointFieldsBLL.GetUserLoansViewPointFieldsLabelHeadingInfo(CurrUser.iUserID);

            gridList.DataSource = dt;
            gridList.DataBind();

            var oldData = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(oldData))
                    {
                        oldData = oldData + ",";
                    }
                    oldData = oldData + dr["PointFieldId"].ToString();
                }
            }

            hidOldPointFieldID.Value = oldData;
        }


        public void BindView()
        {
            ddlDefaultLoansPV.DataSource = userPipelineViewsBll.GetList_ViewName("PipelineType = 'Loans' AND Enabled = 1  AND UserID = " + CurrUser.iUserID.ToString(), "ViewName asc");
            ddlDefaultLoansPV.DataBind();
            ddlDefaultLoansPV.Items.Insert(0, new ListItem("-- select --", "0"));

            if (userHomePref != null && userHomePref.DefaultLoansPipelineViewId != null)
            {
                ddlDefaultLoansPV.SelectedValue = userHomePref.DefaultLoansPipelineViewId.ToString();
            }
        }


        /// <summary>
        /// bind UserPipelineColumns Info
        /// </summary>
        /// <param name="pipelineCols"></param>
        private bool SetUserPipelineColumnsInfo(Model.UserPipelineColumns pipelineCols)
        {
            this.ckbPointFolder.Checked = pipelineCols.PointFolder;
            this.ckbStage.Checked = pipelineCols.Stage;
            this.ckbBranch.Checked = pipelineCols.Branch;
            this.ckbEstimatedClose.Checked = pipelineCols.EstimatedClose;
            this.ckbAlerts.Checked = pipelineCols.Alerts;
            this.ckbLoanOfficer.Checked = pipelineCols.LoanOfficer;
            this.ckbAmount.Checked = pipelineCols.Amount;
            this.ckbLien.Checked = pipelineCols.Lien;
            this.ckbRate.Checked = pipelineCols.Rate;
            this.ckbLender.Checked = pipelineCols.Lender;
            this.ckbLockExpirDate.Checked = pipelineCols.LockExp;
            this.ckbPercentComplete.Checked = pipelineCols.PercentCompl;
            this.ckbProcessor.Checked = pipelineCols.Processor;
            this.ckbTaskCount.Checked = pipelineCols.TaskCount;
            this.ckbFilename.Checked = pipelineCols.PointFileName;

            this.chkLastComplStage.Checked = pipelineCols.LastCompletedStage;
            this.chkLastStageComlDate.Checked = pipelineCols.LastStageComplDate;

            //gdc CR40
            this.ckbCloser.Checked = pipelineCols.Closer;
            this.ckbShipper.Checked = pipelineCols.Shipper;
            this.ckbDocPrep.Checked = pipelineCols.DocPrep;
            this.ckbAssistant.Checked = pipelineCols.Assistant;

            //gdc CR47

            this.cbxLoanProgram.Checked = pipelineCols.LoanProgram;

            //CR49
            this.cbxPurpose.Checked = pipelineCols.Purpose;

            //gdc CR51
            this.cbxJrProcessor.Checked = pipelineCols.JrProcessor;


            this.ckbLastLoanNote.Checked = pipelineCols.LastLoanNote;

            return true;
        }

        /// <summary>
        /// load UserPipelineColumns Personalization Info
        /// </summary>
        /// <param name="pipelineCols"></param>
        private bool GetUserPipelineColumnsInfo(ref Model.UserPipelineColumns pipelineCols)
        {
            pipelineCols.PointFolder = this.ckbPointFolder.Checked;
            pipelineCols.Stage = this.ckbStage.Checked;
            pipelineCols.Branch = this.ckbBranch.Checked;
            pipelineCols.EstimatedClose = this.ckbEstimatedClose.Checked;
            pipelineCols.Alerts = this.ckbAlerts.Checked;
            pipelineCols.LoanOfficer = this.ckbLoanOfficer.Checked;
            pipelineCols.Amount = this.ckbAmount.Checked;
            pipelineCols.Lien = this.ckbLien.Checked;
            pipelineCols.Rate = this.ckbRate.Checked;
            pipelineCols.Lender = this.ckbLender.Checked;
            pipelineCols.LockExp = this.ckbLockExpirDate.Checked;
            pipelineCols.PercentCompl = this.ckbPercentComplete.Checked;
            pipelineCols.Processor = this.ckbProcessor.Checked;
            pipelineCols.TaskCount = this.ckbTaskCount.Checked;
            pipelineCols.PointFileName = this.ckbFilename.Checked;
            pipelineCols.LastCompletedStage = this.chkLastComplStage.Checked;
            pipelineCols.LastStageComplDate = this.chkLastStageComlDate.Checked;

            // CR49
            pipelineCols.Purpose = this.cbxPurpose.Checked;


            //gdc CR40
            pipelineCols.Assistant = this.ckbAssistant.Checked;
            pipelineCols.DocPrep = this.ckbDocPrep.Checked;
            pipelineCols.Shipper = this.ckbShipper.Checked;
            pipelineCols.Closer = this.ckbCloser.Checked;

            //gdc CR47
            pipelineCols.LoanProgram = this.cbxLoanProgram.Checked;

            //gdc CR48
            //pipelineCols.QuickLeadForm = this.ckbQuickleadform.Checked;

            pipelineCols.JrProcessor = this.cbxJrProcessor.Checked;

            pipelineCols.LastLoanNote = this.ckbLastLoanNote.Checked;

            return true;
        }

        /// <summary>
        /// call client function
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strScript"></param>
        private void ClientFun(string strKey, string strScript)
        {
            ClientScriptManager csm = this.Page.ClientScript;
            if (!csm.IsStartupScriptRegistered(this.GetType(), strKey))
            {
                csm.RegisterStartupScript(this.GetType(), strKey, strScript, true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Model.UserPipelineColumns userPipelineCols = UserPipelineColsManager.GetModel(CurrUser.iUserID);
            Model.UserHomePref userHomePref = UserHomePrefManager.GetModel(CurrUser.iUserID);


            try
            {
                if (null == userPipelineCols)
                {
                    userPipelineCols = new Model.UserPipelineColumns();
                    userPipelineCols.UserId = CurrUser.iUserID;
                    GetUserPipelineColumnsInfo(ref userPipelineCols);
                    UserPipelineColsManager.Add(userPipelineCols);
                }
                else
                {
                    GetUserPipelineColumnsInfo(ref userPipelineCols);
                    UserPipelineColsManager.Update(userPipelineCols);
                }


                if (null == userHomePref)
                {
                    userHomePref = new Model.UserHomePref();
                    userHomePref.UserId = CurrUser.iUserID;
                    //GetUserHomePrefInfo(ref userHomePref);
                    userHomePref.DefaultLoansPipelineViewId = Convert.ToInt32(this.ddlDefaultLoansPV.SelectedValue);
                    UserHomePrefManager.Add(userHomePref);
                }
                else
                {
                    //GetUserHomePrefInfo(ref userHomePref);
                    userHomePref.DefaultLoansPipelineViewId = Convert.ToInt32(this.ddlDefaultLoansPV.SelectedValue);
                    UserHomePrefManager.Update(userHomePref);
                }

                ClientFun("sucsmsg", "alert('Saved!');");
            }
            catch { }


            #region Point Fields


            var oldData = hidOldPointFieldID.Value.Trim(); // 旧的fieldid
            var data = hidData.Value.Trim();//new list  may be Contains old fieldID

            try
            {
                userLoansViewPointFieldsBLL.DeleteAllByUser(CurrUser.iUserID);

                if (string.IsNullOrEmpty(data))
                {
                    BindList();
                    return;
                }

                var list = data.Replace("pid=", "").Replace("heading=", "").Split(';').ToList();   //"pid=" + pid + ",heading=" + heading;

                foreach (string item in list)
                {
                    var kv = item.Split(',').ToList();

                    if (kv.Count > 0)
                    {
                        Model.UserLoansViewPointFields model = new Model.UserLoansViewPointFields();

                        model.PointFieldId = Convert.ToInt32(kv.FirstOrDefault());
                        model.UserId = CurrUser.iUserID;

                        userLoansViewPointFieldsBLL.Add(model);
                    }
                }
                string msg = "Saved successfully.";
                PageCommon.WriteJsEnd(this, msg, PageCommon.Js_RefreshSelf);
            }
            catch (Exception ex)
            {
                string msg = "Error:" + ex.Message;
                PageCommon.WriteJsEnd(this, msg, PageCommon.Js_RefreshSelf);
            }

            #endregion



        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            int nPCSCheckedCount = 0;   // Pipeline Column Selections count
            if (this.ckbPointFolder.Checked) nPCSCheckedCount++;
            if (this.ckbAmount.Checked) nPCSCheckedCount++;
            if (this.ckbPercentComplete.Checked) nPCSCheckedCount++;
            if (this.ckbStage.Checked) nPCSCheckedCount++;
            if (this.ckbLien.Checked) nPCSCheckedCount++;
            if (this.ckbProcessor.Checked) nPCSCheckedCount++;
            if (this.ckbBranch.Checked) nPCSCheckedCount++;
            if (this.ckbRate.Checked) nPCSCheckedCount++;
            if (this.ckbTaskCount.Checked) nPCSCheckedCount++;
            if (this.ckbAlerts.Checked) nPCSCheckedCount++;
            if (this.ckbLender.Checked) nPCSCheckedCount++;
            if (this.ckbFilename.Checked) nPCSCheckedCount++;
            if (this.ckbLoanOfficer.Checked) nPCSCheckedCount++;
            if (this.ckbLockExpirDate.Checked) nPCSCheckedCount++;
            if (this.ckbEstimatedClose.Checked) nPCSCheckedCount++;
            if (this.chkLastComplStage.Checked) nPCSCheckedCount++;
            if (this.chkLastStageComlDate.Checked) nPCSCheckedCount++;

            if (this.cbxPurpose.Checked) nPCSCheckedCount++;
            if (this.ckbCloser.Checked) nPCSCheckedCount++;
            if (this.ckbShipper.Checked) nPCSCheckedCount++;
            if (this.ckbDocPrep.Checked) nPCSCheckedCount++;
            if (this.ckbAssistant.Checked) nPCSCheckedCount++;
            if (this.cbxLoanProgram.Checked) nPCSCheckedCount++;
            if (this.cbxJrProcessor.Checked) nPCSCheckedCount++;
            if (this.ckbLastLoanNote.Checked) nPCSCheckedCount++;


            ClientFun("pcscheckedcount", string.Format("nPCSChecked={0};", nPCSCheckedCount));

     
        }

    }
}
