using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;

public partial class LoanDetails_ChangeEstClose_Background : BasePage
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

        bIsValid = PageCommon.ValidateQueryString(this, "NewDate", QueryStringType.Date);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }

        string sNewDate = this.Request.QueryString["NewDate"].ToString();

        bIsValid = PageCommon.ValidateQueryString(this, "LoginUserID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }

        string sLoginUserID = this.Request.QueryString["LoginUserID"].ToString();

        #endregion

        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}
        string sExecResult = string.Empty;
        string sErrorMsg = string.Empty;


        #region 调用LP2Service

        try
        {
            // workflow api
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                UpdateEstCloseDateRequest req = new UpdateEstCloseDateRequest();
                req.FileId = Convert.ToInt32(sLoanID);
                req.NewDate = Convert.ToDateTime(sNewDate);
                req.hdr = new ReqHdr();
                req.hdr.UserId = Convert.ToInt32(sLoginUserID);
                UpdateEstCloseDateResponse respone = service.UpdateEstCloseDate(req);

                if (respone.hdr.Successful == true)
                {
                    sExecResult = "Success";
                    sErrorMsg = "";
                }
                else
                {
                    sExecResult = "Failed";
                    sErrorMsg = "Failed to change est. close date: " + respone.hdr.StatusInfo + ".";
                }
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            sExecResult = "Failed";
            sErrorMsg = "Failed to change est. close date.";
            PageCommon.AlertMsg(this, "Failed to change est. close date, reason: Point Manager is not running.");
        }
        catch (Exception ex)
        {
            sExecResult = "Failed";
            sErrorMsg = "Failed to change est. close date.";
        }

        #endregion

        System.Threading.Thread.Sleep(1000);

        sErrorMsg = sErrorMsg.Replace(@"\", @"\\");

        this.Response.Write("{\"ExecResult\":\"" + sExecResult + "\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
        this.Response.End();
    }
}

