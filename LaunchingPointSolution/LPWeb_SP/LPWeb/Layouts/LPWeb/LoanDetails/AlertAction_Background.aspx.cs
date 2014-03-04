using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.DAL;
using LPWeb.LP_Service;

public partial class LoanDetails_AlertAction_Background : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 接收参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "Action", QueryStringType.String);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string sAction = this.Request.QueryString["Action"].ToString();
        if (sAction != "Acknowledge" && sAction != "Dismiss" && sAction != "Accept" && sAction != "Decline")
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid action command.\"}");
            this.Response.End();
        }

        bIsValid = PageCommon.ValidateQueryString(this, "AlertID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string sAlertID = this.Request.QueryString["AlertID"].ToString();
        int iAlertID = Convert.ToInt32(sAlertID);

        int iLoginUserID = this.CurrUser.iUserID;

        #endregion

        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}

        string sExecResult = string.Empty;
        string sErrorMsg = string.Empty;

        try
        {
            #region 调用RuleManager API

            bool bIsSuccess = false;
            if (sAction == "Acknowledge")
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient service = sm.StartServiceClient())
                {
                    bIsSuccess = service.AcknowledgeAlert(iAlertID, iLoginUserID);
                }
            }
            else if (sAction == "Dismiss")
            {
                bIsSuccess = RuleManager.DismissAlert(iAlertID, iLoginUserID);
            }
            else if (sAction == "Accept")
            {
                bIsSuccess = RuleManager.AcceptAlert(iAlertID, iLoginUserID, string.Empty);
            }
            else if (sAction == "Decline")
            {
                bIsSuccess = RuleManager.DeclineAlert(iAlertID, iLoginUserID, string.Empty);
            }

            if(bIsSuccess == true)
            {
                sExecResult = "Success";
                sErrorMsg = "";
            }
            else
            {
                sExecResult = "Failed";
                sErrorMsg = "Failed to " + sAction + " the selected alert.";
            }

            #endregion
        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            sExecResult = "Failed";
            sErrorMsg = "Failed to " + sAction + " the selected alert: " + ee.Message.Replace("\"", "\\\"");
            PageCommon.AlertMsg(this, "Failed reason: Point Manager is not running.");
        }
        catch (Exception ex)
        {
            sExecResult = "Failed";
            sErrorMsg = "Failed to " + sAction + " the selected alert: "+ex.Message.Replace("\"", "\\\"");
        }

        System.Threading.Thread.Sleep(1000);

        this.Response.Write("{\"ExecResult\":\"" + sExecResult + "\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
        this.Response.End();
    }
}
