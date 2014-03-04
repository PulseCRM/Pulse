using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;

public partial class LoanDetails_CheckPointFileStatusAjax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"Failed to do sth."}

        #region 校验页面参数

        
        string sError_Lost = "Lost required query string.";
        string sError_Invalid = "Invalid query string.";

        if (this.Request.QueryString["FileId"] == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError_Lost + "\"}");
            this.Response.End();
        }
        string sFileId = this.Request.QueryString["FileId"];

        if (PageCommon.IsID(sFileId) == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError_Invalid + "\"}");
            this.Response.End();
        }
        int iFileId = Convert.ToInt32(sFileId);
        
        #endregion

        #region Invoke WCF

        string sErrorMsg = string.Empty;

        CheckPointFileStatusReq req = new CheckPointFileStatusReq();
        CheckPointFileStatusResp resp = new CheckPointFileStatusResp();

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                req.FileId = iFileId;
                req.hdr = new ReqHdr() { UserId = this.CurrUser.iUserID };

                resp = service.CheckPointFileStatus(req);

                if (resp == null || resp.hdr == null)
                {
                    this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"CheckPointFileStatusResp is NULL\"}");
                    return;
                }

                sErrorMsg = resp.hdr.StatusInfo;                

                if (resp.hdr.Successful == true)
                {
                    if (resp.FileLocked == true)    // locked
                    {
                        this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
                        return;
                    }
                    else
                    {
                        this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
                        return;
                    }
                }
                else
                {
                    sErrorMsg = "Point Manager Exception: Database login failed";
                    this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
                    return;
                }
                
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            sErrorMsg = string.Format("Exception: Point Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sErrorMsg);            

            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
            return;
        }
        catch (Exception ex)
        {
            sErrorMsg = string.Format("error: {0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sErrorMsg);
            sErrorMsg = "Point Manager Exception: Database timeout expired";
            
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
            return;
        }

        #endregion
    }
}
