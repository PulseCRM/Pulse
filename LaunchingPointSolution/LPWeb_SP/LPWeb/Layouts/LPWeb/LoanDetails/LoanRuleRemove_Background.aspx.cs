using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using LPWeb.Common;

public partial class LoanDetails_LoanRuleRemove_Background : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 接收参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string sLoanID = this.Request.QueryString["LoanID"].ToString();

        if (this.Request.QueryString["LoanRuleIDs"] == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }

        string sLoanRuleIDs = this.Request.QueryString["LoanRuleIDs"].ToString();

        if (Regex.IsMatch(sLoanRuleIDs, @"^([1-9]\d*)(,[1-9]\d*)*$") == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid query string.\"}");
            this.Response.End();
        }

        #endregion

        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}

        string sExecResult = string.Empty;
        string sErrorMsg = string.Empty;

        try
        {
            LPWeb.BLL.LoanRules LoanRules1 = new LPWeb.BLL.LoanRules();
            LoanRules1.RemoveRuleFromLoan(Convert.ToInt32(sLoanID), sLoanRuleIDs);

            sExecResult = "Success";
            sErrorMsg = "";
        }
        catch (Exception ex)
        {
            sExecResult = "Failed";
            sErrorMsg = "Failed to remove selected rule(s).";
        }

        System.Threading.Thread.Sleep(1000);

        this.Response.Write("{\"ExecResult\":\"" + sExecResult + "\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
        this.Response.End();
    }
}
