using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.Common;

public partial class LoanDetails_Regenerate_Background : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 接收参数

        // LoanID
        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string sLoanID = this.Request.QueryString["LoanID"].ToString();

        // WorkflowTemplateID
        bIsValid = PageCommon.ValidateQueryString(this, "WorkflowTemplateID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string sWorkflowTemplateID = this.Request.QueryString["WorkflowTemplateID"].ToString();
        
        #endregion

        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}

        string sExecResult = string.Empty;
        string sErrorMsg = string.Empty;

        try
        {
            // workflow api
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                GenerateWorkflowRequest req = new GenerateWorkflowRequest();
                req.FileId = int.Parse(sLoanID);
                req.WorkflowTemplId = Convert.ToInt32(sWorkflowTemplateID);
                req.hdr = new ReqHdr();
                req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                req.hdr.UserId = this.CurrUser.iUserID;

                GenerateWorkflowResponse respone = service.GenerateWorkflow(req);

                if (respone.hdr.Successful)
                {
                    sExecResult = "Success";
                    sErrorMsg = "Success to completed selected task(s)";
                }
                else
                {
                    sExecResult = "Failed";
                    sErrorMsg = "Failed to completed selected task(s)";
                }
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            sExecResult = "Failed";
            sErrorMsg = "Failed to regenerate task(s), reason: Point Manager is not running.";          
        }
        catch (Exception)
        {
            sExecResult = "Failed";
            sErrorMsg = "Failed to regenerate task(s)";
        }

        System.Threading.Thread.Sleep(1000);

        this.Response.Write("{\"ExecResult\":\"" + sExecResult + "\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
        this.Response.End();
    }
}

