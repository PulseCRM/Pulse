using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.BLL;
using Utilities;
using System.Linq;


public partial class CheckPointFileStatus_BG_JSON : BasePage
{

    int iFileID = 0;
    LoginUser loginUser = new LoginUser();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["FileID"] == null && Request.QueryString["ContactID"] != null)
        {
            var ContactID = string.IsNullOrEmpty(Request.QueryString["ContactID"]) ? 0 : Convert.ToInt32(Request.QueryString["ContactID"]);
            LPWeb.BLL.LoanContacts loancontacts = new LoanContacts();
            var ds = loancontacts.GetContactLoans(ContactID);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                iFileID = (ds.Tables[0].Rows[0]["FileId"] == DBNull.Value || ds.Tables[0].Rows[0]["FileId"].ToString() == "") ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["FileId"]);
            }
            else
            {
                iFileID = 0;
            }

        }
        else
        {
            iFileID = Request.QueryString["FileID"] == null ? 0 : Convert.ToInt32(Request.QueryString["FileID"]);
        }
        CheckPointFileStatusReq req = new CheckPointFileStatusReq();
        CheckPointFileStatusResp resp = new CheckPointFileStatusResp();

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {

                req.FileId = iFileID;
                req.hdr = new ReqHdr() { UserId = loginUser.iUserID };

                resp = service.CheckPointFileStatus(req);

                if (resp == null || resp.hdr == null)
                {
                    Write(false, "Err : CheckPointFileStatusResp is NULL");
                    return ;
                }
                if (resp.FileLocked)
                {
                    Write(false, resp.hdr.StatusInfo);
                }
                else
                {
                    Write(true, "");
                }
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("Failed to sync data from Point. Reason: Point Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            Write(false, sExMsg);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("error: {0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);

            Write(false, sExMsg);
        }

    }


    string OutJsonTmp = "{\"ExecResult\":\"{0}\",\"ErrorMsg\":\"{1}\"}";
    private string GetOutJson(string State, string msg)
    {
        return OutJsonTmp.Replace("{0}", State).Replace("{1}", msg);

    }

    private void Write(bool isSuccess, string msg)
    {
        if (isSuccess)
        {
            Response.Write(GetOutJson("Success", msg));
            return;
        }
        else
        {
            Response.Write(GetOutJson("Failed", msg));
            return;
        }
    }
}
