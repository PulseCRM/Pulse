using System;
using System.Web;

namespace LPWeb.Settings
{
    /// <summary>
    /// Summary description for Image
    /// </summary>
    public class GetDefaultWflTempCount : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            // json示例
            // {"ExecResult":"Success","DefaultCount":"1"}
            // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}

            context.Response.ContentType = "text/plain";
            //context.Response.ContentType = "application/xhtml+xml";

            // ----------------------- 接收参数 --------------------------

            // WorkflowType
            if (context.Request.QueryString["WorkflowType"] == null)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
                return;
            }

            string sWorkflowType = context.Request.QueryString["WorkflowType"].ToString().Trim();
            if (sWorkflowType != "Processing" && sWorkflowType != "Prospect")
            {
                context.Response.Write(string.Format("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Unknown workflow type {0}.\"}", sWorkflowType));
                return;
            }

            // get default workflow template count
            int iCount = 0;
            LPWeb.BLL.Template_Workflow WorkflowTemplateManager = new LPWeb.BLL.Template_Workflow();

            try
            {
                iCount = WorkflowTemplateManager.GetDefaultWflTemplateCount(sWorkflowType);

                context.Response.Write("{\"ExecResult\":\"Success\",\"DefaultCount\":\"" + iCount + "\"}");
            }
            catch (Exception ex)
            {
                string sErrorMsg = "Failed to check duplication of this workflow template name.";
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}