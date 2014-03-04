<%@ WebHandler Language="C#" Class="Contact_CheckContactRef" %>

using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;

public class Contact_CheckContactRef : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {

        // json示例
        // {"ExecResult":"Success","IsRef":"true|false"}
        // {"ExecResult":"Failed","ErrorMsg":"Failed to check contact reference."}

        context.Response.ContentType = "text/plain";

        #region 接收页面参数

        // ContactID
        if (context.Request.QueryString["ContactID"] == null)
        {
            context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            return;
        }
        string sContactID = context.Request.QueryString["ContactID"].ToString().Trim();
        if (this.IsID(sContactID) == false)
        {
            context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid contact id.\"}");
            return;
        }
        
        #endregion

        bool bIsRef = false;
        string sSql = "select count(1) from LoanContacts where ContactID=" + sContactID;
        int iRefLoanCount = Convert.ToInt32(LPWeb.DAL.DbHelperSQL.ExecuteScalar(sSql));
        if (iRefLoanCount > 0)   // 如果Contact被Loan所引用，删除前，需要重新选择Contact
        {
            bIsRef = true;
        }
        
        context.Response.Write("{\"ExecResult\":\"Success\",\"IsRef\":\"" + bIsRef.ToString().ToLower() + "\"}");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private bool IsID(string sID)
    {
        int iInt;
        bool bIsInt = int.TryParse(sID, out iInt);
        if (bIsInt == false)
        {
            return false;
        }

        if (iInt >= -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}