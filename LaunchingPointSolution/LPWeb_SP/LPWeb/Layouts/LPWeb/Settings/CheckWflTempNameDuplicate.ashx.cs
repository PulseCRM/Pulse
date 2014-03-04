using System;
using System.Web;
using LPWeb.Common;

namespace LPWeb.Settings
{
    /// <summary>
    /// Summary description for Image
    /// </summary>
    public class CheckWorkflowTemplateNameDuplication : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            // json示例
            // {"ExecResult":"Success","IsDuplicated":"true|false"}
            // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}

            context.Response.ContentType = "text/plain";
            //context.Response.ContentType = "application/xhtml+xml";

            // ----------------------- 接收参数 --------------------------

            // WflTempName
            if (context.Request.QueryString["WflTempName"] == null)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
                return;
            }

            string sWorkflowTemplateName = context.Request.QueryString["WflTempName"].ToString().Trim();
            if (sWorkflowTemplateName == string.Empty)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid workflow template name.\"}");
                return;
            }

            // WflTempID
            if (context.Request.QueryString["WflTempID"] == null)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
                return;
            }
            string sWorkflowTemplateID = context.Request.QueryString["WflTempID"].ToString().Trim();
            if (sWorkflowTemplateID != "0" && PageCommon.IsID(sWorkflowTemplateID) == false)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid workflow template id.\"}");
                return;
            }
            int iWorkflowTemplateID = Convert.ToInt32(sWorkflowTemplateID);

            // 检查是否重复
            bool bIsDuplicated = true;
            LPWeb.BLL.Template_Workflow WorkflowTemplateManager = new LPWeb.BLL.Template_Workflow();

            try
            {
                if (iWorkflowTemplateID == 0)    // add
                {
                    bIsDuplicated = WorkflowTemplateManager.IsExist_Create(sWorkflowTemplateName);
                }
                else // edit
                {
                    bIsDuplicated = WorkflowTemplateManager.IsExist_Edit(iWorkflowTemplateID, sWorkflowTemplateName);
                }

                context.Response.Write("{\"ExecResult\":\"Success\",\"IsDuplicated\":\"" + bIsDuplicated.ToString().ToLower() + "\"}");
            }
            catch (Exception)
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