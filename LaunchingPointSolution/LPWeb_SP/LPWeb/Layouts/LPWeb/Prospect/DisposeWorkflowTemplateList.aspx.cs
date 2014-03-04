using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;

public partial class Prospect_DisposeWorkflowTemplateList : BasePage
{
    string sAction = string.Empty;
    int iLoanID = 0;
    int iBranchID = 0;
    string sCloseDialogCodes = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        this.sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";

        string sMissing = "Missing required qurey string.";

        // LoanID
        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sMissing, sCloseDialogCodes);
        }
        this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

        #endregion

        #region 加载Workflow Template List

        string sSql = "select * from Template_Workflow where WorkflowType='Processing' and Enabled=1";
        DataTable WorkflowTemplateListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        this.gridWorkflowTemplateList.DataSource = WorkflowTemplateListData;
        this.gridWorkflowTemplateList.DataBind();

        #endregion
    }

    protected void btnApply_Click(object sender, EventArgs e)
    {
        string sTargetWorkflowTemplateID = this.hdnTargetWorkflowTemplateID.Value;
        int iTargetWorkflowTemplateID = Convert.ToInt32(sTargetWorkflowTemplateID);

        #region Generate Workflow

        try
        {
            // workflow api
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                GenerateWorkflowRequest req = new GenerateWorkflowRequest();
                req.FileId = this.iLoanID;
                req.WorkflowTemplId = iTargetWorkflowTemplateID;
                req.hdr = new ReqHdr();
                req.hdr.UserId = CurrUser.iUserID;

                GenerateWorkflowResponse response = service.GenerateWorkflow(req);

                if (response.hdr.Successful)
                {
                    PageCommon.WriteJsEnd(this, "Generate workflow successfully.", "window.parent.location.href=window.parent.location.href;");
                }
                else
                {
                    string sFailedMsg = "Failed to generate workflow: " + response.hdr.StatusInfo;

                    LPLog.LogMessage(LogType.Logerror, sFailedMsg);

                    PageCommon.WriteJsEnd(this, sFailedMsg, PageCommon.Js_RefreshSelf);
                }
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("Exception happened when send GenerateWorkflowRequest to Point Manager (FileID={0}): {1}", this.iLoanID, "Point Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Exception happened when send GenerateWorkflowRequest to Point Manager (FileID={0}): {1}", this.iLoanID, ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
        }

        #endregion
    }
}
