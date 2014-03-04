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
    public partial class WorkflowTemplateAdd : BasePage
    {
        public string FromURL = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 检查页面参数

            // CloseDialogCodes
            bool bIsValid = PageCommon.ValidateQueryString(this, "CloseDialogCodes", QueryStringType.String);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, "Missing required query string.", "window.parent.location.href=window.parent.location.href");
            }

            #endregion
            
            // __doPostBack
            this.GetPostBackEventReference(this.btnSave);

            if (Request.Url != null)
            {
                FromURL = Request.Url.ToString();
            }

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
                DataTable StageTemplateList1 = this.AddSelectStageRow(StageTemplateListView1.ToTable());
                this.ddlStageProcessing.DataSource = StageTemplateList1;
                this.ddlStageProcessing.DataBind();

                // Prospect
                DataView StageTemplateListView2 = new DataView(StageTemplateList);
                StageTemplateListView2.RowFilter = "WorkflowType='Prospect'";
                DataTable StageTemplateList2 = this.AddSelectStageRow(StageTemplateListView2.ToTable());
                this.ddlStageProspect.DataSource = StageTemplateList2;
                this.ddlStageProspect.DataBind();

                #endregion
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
            string sDesc = this.txtDesc.Text.Trim();
            string sCalcDueDateMethod = this.ddlCalcDueDateMethod.SelectedValue;
            bool bDefault = this.chkDefault.Checked;

            #region get stage data

            string sSequences = this.hdnSequences.Text;
            string sStageTempIDs = this.hdnStageIDs.Text;
            string sStageNames = this.hdnStageNames.Text;   // Open$Submit$Approve
            string sEnableds = this.hdnEnableds.Text;
            string sDaysFromEstCloseDates = this.hdnDaysFromEstCloseDates.Text;
            string sDaysAfterCreationDates = this.hdnDaysAfterCreationDates.Text;

            #endregion

            Template_Workflow WorkflowTemplateManager = new Template_Workflow();

            #region build stage list

            DataTable StageList = WorkflowTemplateManager.GetWflStageList(" and (1=0)");

            if (sSequences != string.Empty)
            {
                string[] SequenceArray = sSequences.Split(',');
                string[] StageTempIDArray = sStageTempIDs.Split(',');
                string[] StageNameArray = sStageNames.Split(',');
                string[] EnabledArray = sEnableds.Split(',');
                string[] DaysFromEstCloseDateArray = sDaysFromEstCloseDates.Split(',');
                string[] DaysAfterCreationDateArray = sDaysAfterCreationDates.Split(',');

                for (int i = 0; i < SequenceArray.Length; i++)
                {
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

                    #region add rows

                    DataRow StageRow = StageList.NewRow();
                    StageRow["WflStageId"] = 0;
                    StageRow["WflTemplId"] = 0;
                    StageRow["Name"] = sStageName;
                    StageRow["SequenceNumber"] = Convert.ToInt16(sSequence);
                    StageRow["Enabled"] = Convert.ToBoolean(sEnabled);

                    if (sCalcDueDateMethod == "Est Close Date")
                    {
                        if (sDaysFromEstCloseDate == "null")
                        {
                            StageRow["DaysFromEstClose"] = DBNull.Value;
                        }
                        else
                        {
                            StageRow["DaysFromEstClose"] = Convert.ToInt16(sDaysFromEstCloseDate);
                        }

                        StageRow["DaysFromCreation"] = DBNull.Value;
                        StageRow["CalculationMethod"] = 1;
                    }
                    else
                    {
                        StageRow["DaysFromEstClose"] = DBNull.Value;

                        if (sDaysAfterCreationDate == "null")
                        {
                            StageRow["DaysFromCreation"] = DBNull.Value;
                        }
                        else 
                        {
                            StageRow["DaysFromCreation"] = Convert.ToInt16(sDaysAfterCreationDate);
                        }
                        StageRow["CalculationMethod"] = 2;
                    }

                    StageRow["TemplStageId"] = sStageTempID;

                    StageList.Rows.Add(StageRow);

                    #endregion
                }
            }

            #endregion

            // insert
            WorkflowTemplateManager.InsertWorkflowTemplate(sWorkflowTemplagteName, true, sDesc, sWorkflowType, bDefault, sCalcDueDateMethod, StageList);

            // success
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Created workflow template successfully.');window.parent.location.href=window.parent.location.href;", true);
        }
    }
}
