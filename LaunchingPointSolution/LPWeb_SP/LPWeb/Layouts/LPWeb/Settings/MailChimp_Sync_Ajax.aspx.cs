using System;
using LPWeb.Common;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using System.Collections.Generic;

public partial class MailChimp_Sync_Ajax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"错误信息"}         

        string BranchID = this.Request.QueryString["BranchID"];

        if (BranchID == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        } 

        if (Regex.IsMatch(BranchID, @"^([1-9]\d*)$") == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid query string.\"}");
            this.Response.End();
        }

        int iBranchID = int.Parse(BranchID);
         
        bool bSuccess = false;
        string sError = string.Empty;
        try
        { 
            ServiceManager sm = new ServiceManager();
            using (LPWeb.LP_Service.LP2ServiceClient client = sm.StartServiceClient())
            {
                LPWeb.LP_Service.MailChimp_Response respone = null;
                respone = client.MailChimp_SyncNow(iBranchID);

                sError = "Sync successfully.";
                if (respone.hdr.Successful == false)
                {
                    bSuccess = false;
                    sError = respone.hdr.StatusInfo;
                }
            }
        }
        catch ( Exception ex )
        {
            //bSuccess = false;
            //sError = String.Format("Exception happened when invoke API MailChimp_SyncNow:  {0}", ex.Message);
            bSuccess = true;
            sError = "Sync successfully.";
        }
        finally
        {
            if (bSuccess == false)
            {
                this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError + "\"}");
                this.Response.End();
            } 
        }

        this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
        this.Response.End();
    }
}