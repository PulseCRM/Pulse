using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Text;

public partial class Settings_WorkflowStageSetup : BasePage
{
    int iWflStageID = 0;
    int iWorkflowTemplateID = 0;
    int PageIndex = 1;
    string sFrom = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验必要参数

        // from
        if (this.Request.QueryString["from"] == null)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing3", "$('#divContainer').hide();alert('Missing required query string.');window.parent.location.href='WorkflowTemplateList.aspx');", true);
            return;
        }
        else
        {
            this.sFrom = this.Request.QueryString["from"];
        }

        bool bIsValid = PageCommon.ValidateQueryString(this, "WflStageID", QueryStringType.ID);
        if (bIsValid == false)
        {
            string sWflStageID = this.Request.QueryString["WflStageID"].ToString();
            if (sWflStageID != "0")
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing1", "$('#divContainer').hide();alert('Missing required query string.');window.parent.location.href='" + this.sFrom + "';", true);
                return;
            }
        }
        this.iWflStageID = Convert.ToInt32(this.Request.QueryString["WflStageID"]);

        bIsValid = PageCommon.ValidateQueryString(this, "WorkflowTemplateID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing2", "$('#divContainer').hide();alert('Missing required query string.');window.parent.location.href='" + this.sFrom + "';", true);
            return;
        }
        this.iWorkflowTemplateID = Convert.ToInt32(this.Request.QueryString["WorkflowTemplateID"]);
        
        if (this.Request.QueryString["PageIndex"] != null) // PageIndex
        {
            try
            {
                PageIndex = int.Parse(this.Request.QueryString["PageIndex"].ToString());
            }
            catch
            {
                PageIndex = 1;
            }
        }
        else
        {
            PageIndex = AspNetPager1.CurrentPageIndex;
        }

        

        #endregion

        Template_Workflow WorkflowTemplateManager = new Template_Workflow();

        #region 加载Workflow Stage信息

        DataTable WflStageInfo = null;
        if (this.iWflStageID > 0)
        {
            WflStageInfo = WorkflowTemplateManager.GetWflStageInfo(this.iWflStageID);
            if (WflStageInfo.Rows.Count == 0)
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Invalid1", "$('#divContainer').hide();alert('Invalid required query string.');window.parent..location.href='" + this.sFrom + "';", true);
                return;
            }
        }

        #endregion

        #region 加载Workflow Template信息

        DataTable WorkflowTemplateInfo = WorkflowTemplateManager.GetWorkflowTemplateInfo(this.iWorkflowTemplateID);
        if (WorkflowTemplateInfo.Rows.Count == 0)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Invalid2", "$('#divContainer').hide();alert('Invalid required query string.');window.parent..location.href='" + this.sFrom + "';", true);
            return;
        }

        string sWorkflowTemplate = WorkflowTemplateInfo.Rows[0]["Name"].ToString();
        string sWorkflowType = WorkflowTemplateInfo.Rows[0]["WorkflowType"].ToString();
        string sCalcMethod = WorkflowTemplateInfo.Rows[0]["CalculationMethod"].ToString();

        this.lbWorkflowTemplate.Text = sWorkflowTemplate;
        this.lbWorkflowType.Text = sWorkflowType;

        #endregion

        #region 加载Workflow Task List

        Template_Stages StageTemplateManager = new Template_Stages();

        string sSql = string.Empty;
        string sOrderName = string.Empty;
        if (this.iWflStageID == 0)   // // All Stages
        {
            //sSql = "select top(10) a.*, b.Name as PrerequisiteTask, c.Name as StageName, case when a.Enabled=1 then 'Yes' else 'No' end as TaskEnabled "
            //            + "from Template_Wfl_Tasks as a left join Template_Wfl_Tasks as b on a.PrerequisiteTaskId = b.TemplTaskId "
            //            + "inner join Template_Wfl_Stages as c on a.WflStageId = c.WflStageId "
            //            + "where a.WflStageId in (select WflStageId from Template_Wfl_Stages where WflTemplId=" + this.iWorkflowTemplateID + ")";
            sSql = " AND WflTemplId='" + this.iWorkflowTemplateID + "'";
            sOrderName = "StageName";
        }
        else
        {
            //sSql = "select top(10) a.*, b.Name as PrerequisiteTask, c.Name as StageName, case when a.Enabled=1 then 'Yes' else 'No' end as TaskEnabled "
            //            + "from Template_Wfl_Tasks as a left join Template_Wfl_Tasks as b on a.PrerequisiteTaskId = b.TemplTaskId "
            //            + "inner join Template_Wfl_Stages as c on a.WflStageId = c.WflStageId "
            //            + "where a.WflStageId=" + this.iWflStageID;
            sSql = " AND WflStageId='" + this.iWflStageID + "'";
            sOrderName = "SequenceNumber";
        }
        int pageSize = AspNetPager1.PageSize;
        int pageIndex = PageIndex;
        int recordCount = 0;

        //DataTable TaskListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        LPWeb.BLL.Template_Wfl_Tasks tBLL = new Template_Wfl_Tasks();
        DataSet TaskListData = tBLL.GetWorkflowStageTasks(pageSize, pageIndex, sOrderName, sSql, out recordCount);

        AspNetPager1.PageSize = pageSize;
        AspNetPager1.RecordCount = recordCount;

        this.gridWorkflowTaskList.DataSource = TaskListData;
        this.gridWorkflowTaskList.DataBind();

        #endregion

        if (this.IsPostBack == false)
        {
            #region 加载Workflow Stage List

            DataTable WflStageListData = WorkflowTemplateManager.GetWflStageList(this.iWorkflowTemplateID);

            DataRow NewStageRow = WflStageListData.NewRow();
            NewStageRow["TemplStageId"] = DBNull.Value;
            NewStageRow["Name"] = "All Stages";
            WflStageListData.Rows.InsertAt(NewStageRow, 0);

            this.ddlStage.DataSource = WflStageListData;
            this.ddlStage.DataBind();

            #endregion

            #region 绑定数据

            if (this.iWflStageID == 0)  // All Stages
            {

            }
            else
            {
                this.ddlStage.SelectedValue = this.iWflStageID.ToString();
                this.chkEnabled.Checked = Convert.ToBoolean(WflStageInfo.Rows[0]["Enabled"]);
                this.txtSeq.Text = WflStageInfo.Rows[0]["SequenceNumber"].ToString();
                if (WflStageInfo.Rows[0]["CalculationMethod"].ToString() != ""
                    && WflStageInfo.Rows[0]["CalculationMethod"].ToString() != "0")
                {
                    sCalcMethod = WflStageInfo.Rows[0]["CalculationMethod"].ToString();
                }

                if (sCalcMethod != "3")
                {
                    if (sCalcMethod != "" && sCalcMethod != "0")
                    {
                        this.hdnCalcDueDateMethod.Value = sCalcMethod == "1" ? "Est Close Date" : "Creation Date";
                        //this.ddlCalcDueDateMethod.SelectedValue = sCalcMethod == "1" ? "Est Close Date" : "Creation Date";
                    }
                    if (sCalcMethod == "1")
                    {
                        this.txtDaysFromEstClose.Text = WflStageInfo.Rows[0]["DaysFromEstClose"].ToString();
                        this.txtDaysFromEstClose.Enabled = true;
                        this.txtDaysAfterCreation.Enabled = false;
                    }
                    else
                    {
                        this.txtDaysFromEstClose.Enabled = false;
                        this.txtDaysAfterCreation.Text = WflStageInfo.Rows[0]["DaysFromCreation"].ToString();
                        this.txtDaysAfterCreation.Enabled = true;
                    }
                }
                else
                {
                    this.hdnCalcDueDateMethod.Value = "Completion Date of the previous Stage";
                }
            }
            #endregion
        }
        Template_Wfl_Stages tempWflStages = new Template_Wfl_Stages();
        this.hdnMinStagesSeq.Value = tempWflStages.GetMinStageSeqNumByWflTempID(this.iWorkflowTemplateID).ToString();
        this.hdnSecStagesSeq.Value = tempWflStages.GetSecStageSeqNumByWflTempID(this.iWorkflowTemplateID).ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        short iSeq = Convert.ToInt16(this.txtSeq.Text);
        bool bEnabled = this.chkEnabled.Checked;
        short iDaysFromEstClose = this.txtDaysFromEstClose.Text == string.Empty ? Int16.Parse("0") : Convert.ToInt16(this.txtDaysFromEstClose.Text);
        short iDaysAfterCreation = this.txtDaysAfterCreation.Text == string.Empty ? Int16.Parse("0") : Convert.ToInt16(this.txtDaysAfterCreation.Text);
        short iCalcMethod = 0;
        if (this.hdnCalcDueDateMethod.Value == "Est Close Date")
        {
            iCalcMethod = 1;
        }
        else if (this.hdnCalcDueDateMethod.Value == "Creation Date")
        {
            iCalcMethod = 2;
        }
        else if (this.hdnCalcDueDateMethod.Value == "Completion Date of the previous Stage")
        {
            iCalcMethod = 3;
        }

        Template_Workflow WorkflowTemplateManager = new Template_Workflow();

        #region 检查Rule Name是否重复

        bool bIsExist = WorkflowTemplateManager.IsWflStageSeqExist(this.iWorkflowTemplateID, this.iWflStageID, iSeq);
        if (bIsExist == true)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Duplicate", "$('#divContainer').hide();alert('The Sequence has been taken. Please use a different number.');$('#divContainer').show();", true);
            return;
        }

        #endregion


        WorkflowTemplateManager.UpdateWflStage(this.iWflStageID, iSeq, bEnabled, iDaysFromEstClose, iDaysAfterCreation,iCalcMethod);

        #region update seq no of workflow task

        string sSeqNos = this.hdnSeqNos.Value;
        if (sSeqNos != string.Empty)
        {
            string[] IDSeqs = sSeqNos.Split(',');

            foreach (string IDSeq in IDSeqs)
            {
                string[] temp = IDSeq.Split(':');
                
                string sTaskID = temp[0];
                string sSeqNo = temp[1];

                int iTaskID = Convert.ToInt32(sTaskID);
                int iSeqNo = Convert.ToInt32(sSeqNo);

                this.UpdateWflTaskSeqNo(iTaskID, iSeqNo);
            }
        }

        #endregion

        // success
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Update workflow stage successfully.');window.parent.location.href='" + this.sFrom + "';", true);
    }

    public string GetSeqNoOptions(string sSeqNo) 
    {
        StringBuilder sbOptions = new StringBuilder();

        for (int i = 0; i < 100; i++)
        {
            int j = i + 1;

            if (sSeqNo == j.ToString())
            {
                sbOptions.AppendLine("<option selected value=\"" + j + "\">" + j + "</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value=\"" + j + "\">" + j + "</option>");
            }
        }

        return sbOptions.ToString();
    }

    public string GetPrerequisiteTaskName(string id)
    {
         string Name = "";
         if (!string.IsNullOrEmpty(id))
         {
             string sSql = "select Name from Template_Wfl_Tasks where TemplTaskId=" + id;
             DataTable dt = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

             foreach (DataRow dr in dt.Rows)
             {
                 if (dr["Name"] == DBNull.Value)
                     Name = "";
                 else
                     Name = dr["Name"].ToString().Trim();
             }
         }

         return Name;
    }

    private void UpdateWflTaskSeqNo(int iTaskID, int iSeqNo) 
    {
        string sSql = "update Template_Wfl_Tasks set SequenceNumber=" + iSeqNo + " where TemplTaskId=" + iTaskID;
        LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
    }
}
