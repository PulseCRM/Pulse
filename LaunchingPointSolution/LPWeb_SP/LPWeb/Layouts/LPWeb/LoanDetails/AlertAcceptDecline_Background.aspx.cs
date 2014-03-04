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

public partial class LoanDetails_AlertAcceptDecline_Background : BasePage
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
        if (sAction != "Accept" && sAction != "Decline")
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid action command.\"}");
            this.Response.End();
        }


        bIsValid = PageCommon.ValidateQueryString(this, "AlertId", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string sAlertID = this.Request.QueryString["AlertId"].ToString();
        int iAlertID = Convert.ToInt32(sAlertID);

        string sLoginUserID = string.Empty;
        int iLoginUserID = 0;
        bIsValid = PageCommon.ValidateQueryString(this, "LoginUserID", QueryStringType.ID);
        if (bIsValid == false)
        {
            //this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            //this.Response.End();
        }
        else
        {
            sLoginUserID = this.Request.QueryString["LoginUserID"].ToString();
            iLoginUserID = Convert.ToInt32(sLoginUserID);
        }



        #endregion

        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}

        string sExecResult = string.Empty;
        string sErrorMsg = string.Empty;

        try
        {
            #region 获取Alert信息

            string sSql = "select * from LoanAlerts where LoanAlertId=" + sAlertID;
            DataTable AlertInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
            if (AlertInfo.Rows.Count == 0)
            {
                this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid alert id.\"}");
                this.Response.End();
            }

            string sAlertDesc = AlertInfo.Rows[0]["Desc"].ToString();
            string sAlertEmailContent = AlertInfo.Rows[0]["AlertEmail"].ToString();
            string sRecomEmailContent = AlertInfo.Rows[0]["RecomEmail"].ToString();

            #endregion

            #region 调用RuleManager API

            bool bIsSuccess = false;
            if (sAction == "Accept")
            {
                bIsSuccess = RuleManager.AcceptAlert(iAlertID, iLoginUserID, string.Empty);
            }
            else if (sAction == "Decline")
            {
                bIsSuccess = RuleManager.DeclineAlert(iAlertID, iLoginUserID, string.Empty);
            }

            if (bIsSuccess == true)
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
        catch (Exception ex)
        {
            sExecResult = "Failed";
            sErrorMsg = "Failed to " + sAction + " the selected alert.";
        }

        System.Threading.Thread.Sleep(1000);

        if (sExecResult == "Success")
        {
            this.Response.Write("Thank you very much for your response.");
        }
        else
        {
            this.Response.Write("{\"ExecResult\":\"" + sExecResult + "\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
        }

        this.Response.End();
    }
}
