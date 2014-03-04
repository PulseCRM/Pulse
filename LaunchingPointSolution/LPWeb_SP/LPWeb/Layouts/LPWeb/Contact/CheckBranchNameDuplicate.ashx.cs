using System;
using System.Web;
using LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Contact
{
    public partial class CheckBranchNameDuplicate : IHttpHandler
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
            if (context.Request.QueryString["name"] == null)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
                return;
            }
            string sBranchName = context.Request.QueryString["name"].ToString();
            if (sBranchName == string.Empty)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid branch name.\"}");
                return;
            }

            // WflTempID
            if (context.Request.QueryString["branchID"] == null)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
                return;
            }
            string sBranchID = context.Request.QueryString["branchID"].ToString().Trim();
            if (sBranchID != "0" && PageCommon.IsID(sBranchID) == false)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid branch id.\"}");
                return;
            }
            int iBranchID = Convert.ToInt32(sBranchID);

            // 检查是否重复
            bool bIsDuplicated = true;
            BLL.ContactBranches bContactBranches = new BLL.ContactBranches();

            try
            {
                if (iBranchID == 0)    // add
                {
                    bIsDuplicated = bContactBranches.IsExist_CreateBase(sBranchName);
                }
                else // edit
                {
                    bIsDuplicated = bContactBranches.IsExist_EditBase(iBranchID, sBranchName);
                }

                context.Response.Write("{\"ExecResult\":\"Success\",\"IsDuplicated\":\"" + bIsDuplicated.ToString().ToLower() + "\"}");
            }
            catch (Exception)
            {
                string sErrorMsg = "Failed to check duplication of this branch name.";
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
