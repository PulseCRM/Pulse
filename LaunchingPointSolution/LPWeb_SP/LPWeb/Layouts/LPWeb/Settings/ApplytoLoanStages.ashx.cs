using System;
using System.Web;
using LPWeb.Common;

namespace LPWeb.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplytoLoanStages : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            // json示例
            // {"ExecResult":"Success"}
            // {"ExecResult":"Failed","ErrorMsg":"error message."}

            context.Response.ContentType = "text/plain";
            
            try
            {
                LPWeb.DAL.DbHelperSQL.ExecuteNonQuery("exec MassUpdateStageAlias");
            }
            catch (Exception)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Exception happened when apply to loan stages.\"}");
            }

            context.Response.Write("{\"ExecResult\":\"Success\"}");
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