using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Settings
{
    public partial class WorkflowTemplateEdit : BasePage
    {
        int iWorkflowTemplateID = 0;
        DataTable StageTemplateList1;
        DataTable StageTemplateList2;
        public string FromURL = string.Empty;
        public string gWorkflowType = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 校验必要参数

            // WorkflowTemplateID
            bool bIsValid = PageCommon.ValidateQueryString(this, "WorkflowTemplateID", QueryStringType.ID);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, "Missing required query string.", "window.parent.location.href=window.parent.location.href");
            }
            this.iWorkflowTemplateID = Convert.ToInt32(this.Request.QueryString["WorkflowTemplateID"]);

            #endregion

            if (Request.Url != null)
            {
                FromURL = Request.Url.ToString();
            }

            Template_Workflow WorkflowTemplateManager = new Template_Workflow();

            #region 加载Workflow Template信息

            DataTable WorkflowTemplateInfo = WorkflowTemplateManager.GetWorkflowTemplateInfo(this.iWorkflowTemplateID);
            if (WorkflowTemplateInfo.Rows.Count == 0)
            {
                PageCommon.WriteJsEnd(this, "Invalid required query string.", "window.parent.location.href=window.parent.location.href");
            }

            #endregion

            // is custom
            this.hdnIsCustom.Text = WorkflowTemplateInfo.Rows[0]["Custom"].ToString();
            this.gWorkflowType = WorkflowTemplateInfo.Rows[0]["WorkflowType"].ToString();
            
            // __doPostBack
            this.GetPostBackEventReference(this.btnSave);

            if (this.IsPostBack == false)
            {
                #region build ddlSeq options

                StringBuilder sbSeqOptions = new StringBuilder();
                for (int i = 0; i < 999; i++)
                {
                    int iSeq = i + 1;
                    sbSeqOptions.AppendLine("<option value=\"" + iSeq + "\">" + iSeq + "</option>");
                }
                this.ltrSeqOptions.Text = sbSeqOptions.ToString();

                #endregion

                #region 加载Stage Template List

                Template_Stages StageTemplateManager = new Template_Stages();

                // Processing and Prospect
                DataTable StageTemplateList = StageTemplateManager.GetStageTemplateList(" and [Enabled]=1");
                this.ddlStageTemplateList.DataSource = StageTemplateList;
                this.ddlStageTemplateList.DataBind();

                // Processing
                DataView StageTemplateListView1 = new DataView(StageTemplateList);
                StageTemplateListView1.RowFilter = "WorkflowType='Processing'";
                this.StageTemplateList1 = this.AddSelectStageRow(StageTemplateListView1.ToTable());
                this.ddlStageProcessing.DataSource = StageTemplateList1;
                this.ddlStageProcessing.DataBind();

                // Prospect
                DataView StageTemplateListView2 = new DataView(StageTemplateList);
                StageTemplateListView2.RowFilter = "WorkflowType='Prospect'";
                this.StageTemplateList2 = this.AddSelectStageRow(StageTemplateListView2.ToTable());
                this.ddlStageProspect.DataSource = StageTemplateList2;
                this.ddlStageProspect.DataBind();

                #endregion

                #region 绑定数据

                if (WorkflowTemplateInfo.Rows[0]["Name"] == DBNull.Value)
                    this.txtWorkflowTemplate.Text = "";
                else
                    this.txtWorkflowTemplate.Text = WorkflowTemplateInfo.Rows[0]["Name"].ToString();
                if ((WorkflowTemplateInfo.Rows[0]["WorkflowType"] == DBNull.Value) ||
                    (WorkflowTemplateInfo.Rows[0]["WorkflowType"].ToString() == ""))
                {
                    this.ddlWorkflowType.SelectedValue = "Processing";
                }
                else
                {
                    //this.ddlWorkflowType.SelectedValue = WorkflowTemplateInfo.Rows[0]["WorkflowType"].ToString();
                    if (WorkflowTemplateInfo.Rows[0]["WorkflowType"].ToString().ToUpper() == "PROCESSING")
                        this.ddlWorkflowType.SelectedValue = "Processing";
                    if (WorkflowTemplateInfo.Rows[0]["WorkflowType"].ToString().ToUpper() == "PROSPECT")
                        this.ddlWorkflowType.SelectedValue = "Prospect";
                }

                if (WorkflowTemplateInfo.Rows[0]["Desc"] != DBNull.Value)
                    this.txtDesc.Text = WorkflowTemplateInfo.Rows[0]["Desc"].ToString();

                if (WorkflowTemplateInfo.Rows[0]["Enabled"] == DBNull.Value)
                {
                    this.chkEnabled.Checked = false;
                }
                else
                {
                    this.chkEnabled.Checked = Convert.ToBoolean(WorkflowTemplateInfo.Rows[0]["Enabled"]);
                }

                // cr47
                if (this.chkEnabled.Checked == true)
                {
                    this.btnRegen.Disabled = false;
                }
                else
                {
                    this.btnRegen.Disabled = true;
                }

                if (WorkflowTemplateInfo.Rows[0]["CalculationMethod"] == DBNull.Value)
                {
                    this.ddlCalcDueDateMethod.SelectedValue = "Est Close Date";
                }
                else if (WorkflowTemplateInfo.Rows[0]["CalculationMethod"].ToString() == "1")
                {
                    this.ddlCalcDueDateMethod.SelectedValue = "Est Close Date";
                }
                else if (WorkflowTemplateInfo.Rows[0]["CalculationMethod"].ToString() == "2")
                {
                    this.ddlCalcDueDateMethod.SelectedValue = "Creation Date";
                }
                else
                {
                    this.ddlCalcDueDateMethod.SelectedValue = "Completion Date of the previous Stage";
                }

 //this.ddlCalcDueDateMethod.SelectedValue = WorkflowTemplateInfo.Rows[0]["CalculationMethod"].ToString() == "1" ? "Est Close Date" : "Creation Date";

                if (WorkflowTemplateInfo.Rows[0]["Default"] == DBNull.Value)
                    this.chkDefault.Checked = false;
                else
                    this.chkDefault.Checked = Convert.ToBoolean(WorkflowTemplateInfo.Rows[0]["Default"]);

                #endregion

                #region 加载Workflow Stage List

                DataTable WflStageListData = WorkflowTemplateManager.GetWflStageList(this.iWorkflowTemplateID);
                this.gridStageList.DataSource = WflStageListData;
                this.gridStageList.DataBind();

                #endregion

                // set counter
                this.hdnCounter.Value = WflStageListData.Rows.Count.ToString();

            }
        }

        private DataTable AddSelectStageRow(DataTable StageTemplateList)
        {
            DataRow NewStageRow = StageTemplateList.NewRow();
            NewStageRow["TemplStageId"] = DBNull.Value;
            NewStageRow["Name"] = "-- select --";
            StageTemplateList.Rows.InsertAt(NewStageRow, 0);

            return StageTemplateList;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string sWorkflowTemplagteName = this.txtWorkflowTemplate.Text.Trim();
            string sWorkflowType = this.ddlWorkflowType.SelectedValue;
            bool bEnabled = this.chkEnabled.Checked;
            string sDesc = this.txtDesc.Text.Trim();
            string sCalcDueDateMethod = this.ddlCalcDueDateMethod.SelectedValue;
            bool bDefault = this.chkDefault.Checked;

            #region get stage data

            string sWflStageIDs = this.hdnWflStageIDs.Text;
            string sSequences = this.hdnSequences.Text;
            string sStageTempIDs = this.hdnStageTemplateIDs.Text;
            string sStageNames = this.hdnStageNames.Text;   // Open$Submit$Approve
            string sEnableds = this.hdnEnableds.Text;
            string sDaysFromEstCloseDates = this.hdnDaysFromEstCloseDates.Text;
            string sDaysAfterCreationDates = this.hdnDaysAfterCreationDates.Text;
            string sRemovedStageIDs = this.hdnRemovedStageIDs.Text;

            #endregion

            Template_Workflow WorkflowTemplateManager = new Template_Workflow();

            #region build stage list

            DataTable StageList = WorkflowTemplateManager.GetWflStageList(" and (1=0)");

            if (sWflStageIDs != string.Empty)
            {
                string[] WflStageIDArray = sWflStageIDs.Split(',');
                string[] SequenceArray = sSequences.Split(',');
                string[] StageTempIDArray = sStageTempIDs.Split(',');
                string[] StageNameArray = sStageNames.Split(',');
                string[] EnabledArray = sEnableds.Split(',');
                string[] DaysFromEstCloseDateArray = sDaysFromEstCloseDates.Split(',');
                string[] DaysAfterCreationDateArray = sDaysAfterCreationDates.Split(',');

                for (int i = 0; i < WflStageIDArray.Length; i++)
                {
                    string sWflStageID = WflStageIDArray[i];
                    int iWflStageID = Convert.ToInt32(sWflStageID);

                    string sSequence = SequenceArray[i];
                    string sStageTempID = StageTempIDArray[i];
                    string sStageNameBlock = StageNameArray[i];
                    string sEnabled = EnabledArray[i];
                    string sDaysFromEstCloseDate = DaysFromEstCloseDateArray[i];
                    string sDaysAfterCreationDate = DaysAfterCreationDateArray[i];

                    #region format StageName

                    string sStageName = sStageNameBlock.Replace("[$", string.Empty);
                    sStageName = sStageName.Replace("$]", string.Empty);

                    #endregion

                    #region Get stage calcDueDateMethod
                    BLL.Template_Wfl_Stages twsMgr = new Template_Wfl_Stages();
                    Model.Template_Wfl_Stages twsModel = twsMgr.GetModel(iWflStageID);
                    string sCalcMethod = "";
                    if (twsModel != null)
                    {
                        sCalcMethod = twsModel.CalculationMethod.Value.ToString();
                    }
                    int iCalcMethod = 1;
                    //if (sCalcMethod == "" || sCalcMethod == "0")
                    //{
                    //    if (sCalcDueDateMethod == "Creation Date")
                    //    {
                    //        sCalcMethod = "2";
                    //    }
                    //    else
                    //    {
                    //        sCalcMethod = "1";
                    //    }
                    //}

                    if (sCalcDueDateMethod == "Est Close Date")
                    {
                        sCalcMethod ="1";
                    }
                    else if (sCalcDueDateMethod == "Creation Date")
                    {
                        sCalcMethod ="2";
                    }
                    else
                    {

                        sCalcMethod ="3";
                    }
                    iCalcMethod = int.Parse(sCalcMethod);
                    #endregion

                    #region add rows

                    DataRow StageRow = StageList.NewRow();
                    StageRow["WflStageId"] = iWflStageID;
                    StageRow["WflTemplId"] = this.iWorkflowTemplateID;
                    StageRow["Name"] = sStageName;
                    StageRow["SequenceNumber"] = Convert.ToInt16(sSequence);
                    StageRow["Enabled"] = Convert.ToBoolean(sEnabled);

                    //if (iCalcMethod == 1)
                    //{
                        if (sDaysFromEstCloseDate == "null")
                        {
                            StageRow["DaysFromEstClose"] = DBNull.Value;
                        }
                        else
                        {
                            StageRow["DaysFromEstClose"] = Convert.ToInt16(sDaysFromEstCloseDate);
                        }

                        //StageRow["DaysFromCreation"] = DBNull.Value;
                        StageRow["CalculationMethod"] = iCalcMethod;
                    //}
                    //else
                    //{
                        //StageRow["DaysFromEstClose"] = DBNull.Value;

                        if (sDaysAfterCreationDate == "null")
                        {
                            StageRow["DaysFromCreation"] = DBNull.Value;
                        }
                        else
                        {
                            StageRow["DaysFromCreation"] = Convert.ToInt16(sDaysAfterCreationDate);
                        }
                        //StageRow["CalculationMethod"] = iCalcMethod;
                   // }

                    StageRow["TemplStageId"] = sStageTempID;

                    StageList.Rows.Add(StageRow);

                    if (iWflStageID > 0)
                    {
                        StageRow.AcceptChanges();
                        StageRow.SetModified();
                    }

                    #endregion
                }
            }

            #endregion

            sWorkflowType = this.gWorkflowType;

            // update
            WorkflowTemplateManager.UpdateWorkflowTemplate(this.iWorkflowTemplateID, sWorkflowTemplagteName, bEnabled, sDesc, sWorkflowType, bDefault, sCalcDueDateMethod, StageList, sRemovedStageIDs);

            // success
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Update workflow template successfully.');window.parent.location.href=window.parent.location.href;", true);
        }

        public string GetOptions_ddlSeq(string sSelectedSeq)
        {
            int iSelectedSeq = Convert.ToInt32(sSelectedSeq);
            StringBuilder sbOptions = new StringBuilder();
            for (int i = 0; i < 999; i++)
            {
                int iSeq = i + 1;

                if (iSeq == iSelectedSeq)
                {
                    sbOptions.AppendLine("<option value=\"" + iSeq + "\" selected>" + iSeq + "</option>");
                }
                else
                {
                    sbOptions.AppendLine("<option value=\"" + iSeq + "\">" + iSeq + "</option>");
                }
            }

            return sbOptions.ToString();
        }

        public string GetOptions_ddlStage(string sStageTemplateID)
        {
            StringBuilder sbOptions = new StringBuilder();

            string sWorkflowType = this.ddlWorkflowType.SelectedValue;
            if (sWorkflowType == "Processing")
            {
                foreach (DataRow StageRow in StageTemplateList1.Rows)
                {
                    string sStageID = StageRow["TemplStageId"].ToString();
                    string sStageName = StageRow["Name"].ToString();
                    if (sStageID == sStageTemplateID)
                    {
                        sbOptions.AppendLine("<option value='" + sStageID + "' selected>" + sStageName + "</option>");
                    }
                    else
                    {
                        sbOptions.AppendLine("<option value='" + sStageID + "'>" + sStageName + "</option>");
                    }
                }
            }
            else // Prospect
            {
                foreach (DataRow StageRow in StageTemplateList2.Rows)
                {
                    string sStageID = StageRow["TemplStageId"].ToString();
                    string sStageName = StageRow["Name"].ToString();
                    if (sStageID == sStageTemplateID)
                    {
                        sbOptions.AppendLine("<option value='" + sStageID + "' selected>" + sStageName + "</option>");
                    }
                    else
                    {
                        sbOptions.AppendLine("<option value='" + sStageID + "'>" + sStageName + "</option>");
                    }
                }
            }

            return sbOptions.ToString();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Template_Workflow WorkflowTemplateManager = new Template_Workflow();
            WorkflowTemplateManager.DeleteWorkflowTemplate(this.iWorkflowTemplateID);
            // success
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Delete workflow template successfully.');window.parent.location.href='WorkflowTemplateList.aspx';", true);
        }
    }
}
