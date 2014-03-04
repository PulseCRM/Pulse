using System;
using System.Web;

namespace LPWeb.Settings
{
    /// <summary>
    /// Summary description for Image
    /// </summary>
    public class DisabledWorkflowTask : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {

            // json示例
            // {"ExecResult":"Success"}
            // {"ExecResult":"Failed","ErrorMsg":"Failed to do something."}

            context.Response.ContentType = "text/plain";

            // WflTaskIDs
            if (context.Request.QueryString["WflTaskIDs"] == null)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
                return;
            }

            string sWflTaskIDs = context.Request.QueryString["WflTaskIDs"].ToString();

            if (System.Text.RegularExpressions.Regex.IsMatch(sWflTaskIDs, @"^([1-9]\d*)(,[1-9]\d*)*$") == false)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid query string.\"}");
                return;
            }

            string sSql = "update Template_Wfl_Tasks set Enabled=0 where TemplTaskId in (" + sWflTaskIDs + ")";

            try
            {
                LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);

                context.Response.Write("{\"ExecResult\":\"Success\"}");
            }
            catch (Exception ex)
            {
                string sErrorMsg = "Failed to disable selected task.";
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