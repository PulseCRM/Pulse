using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.Common;
using System.Data;

public partial class Settings_RegenerateWorkflowAjax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // {"ExecResult":"Success"}
        // {"ExecResult":"Failed","ErrorMsg":"unknown errors."}

        #region 接收参数

        // WorkflowTemplateID
        bool bIsValid = PageCommon.ValidateQueryString(this, "WorkflowTemplateID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Lose required query string.\"}");
            this.Response.End();
        }
        string sWorkflowTemplateID = this.Request.QueryString["WorkflowTemplateID"];
        int iWorkflowTemplateID = Convert.ToInt32(sWorkflowTemplateID);
        
        #endregion

        #region 加载Workflow Template信息

        LPWeb.BLL.Template_Workflow WorkflowTemplateManager = new LPWeb.BLL.Template_Workflow();
        DataTable WorkflowTemplateInfo = WorkflowTemplateManager.GetWorkflowTemplateInfo(iWorkflowTemplateID);
        if (WorkflowTemplateInfo.Rows.Count == 0)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid workflow template id.\"}");
            this.Response.End();
        }

        string sWorkflowType = WorkflowTemplateInfo.Rows[0]["WorkflowType"].ToString();

        #endregion

        #region 加载Loan List

        string sSql = "select * from LoanWflTempl a inner join Loans b on a.FileId=b.FileId WHERE a.WflTemplId=" + sWorkflowTemplateID + " and b.Status='" + sWorkflowType + "'";
        DataTable LoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        #endregion
        bool bAllSuccess = true;
        try
         {
            foreach (DataRow LoanRow in LoanList.Rows)
            {
                int iFileID = Convert.ToInt32(LoanRow["FileId"]);
                // workflow api
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient service = sm.StartServiceClient())
                {
                    GenerateWorkflowRequest req = new GenerateWorkflowRequest();
                    req.FileId = iFileID;
                    req.WorkflowTemplId = iWorkflowTemplateID;
                    req.hdr = new ReqHdr();
                    req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                    req.hdr.UserId = this.CurrUser.iUserID;

                    GenerateWorkflowResponse respone = service.GenerateWorkflow(req);

                    if (respone.hdr.Successful == false)
                    {
                        bAllSuccess = false;
                        break;
                    }
                }
            }
         }
         catch (System.ServiceModel.EndpointNotFoundException)
         {
             this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Exception happened when regenerate workflows: Point Manager is not running.\"}");
             this.Response.End();
         }
         catch (Exception)
         {
             this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Unknown exception happened when regenerate workflows.\"}");
             this.Response.End();
         }

        if (bAllSuccess == true)
        {
            this.Response.Write("{\"ExecResult\":\"Success\"}");
        }
        else
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Failed to regenerate workflows.\"}");
        }
    }
}

