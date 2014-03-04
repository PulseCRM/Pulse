using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;

public partial class Prospect_UpdateBorrower_Background : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 接受参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "ContactID", QueryStringType.ID);
        if (bIsValid == false)
        {
            bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
            if (!bIsValid)
            {
                this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
                this.Response.End();
            }
        }

        int iContactID = Convert.ToInt32(this.Request.QueryString["ContactID"]);
        int iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);
        if (iContactID <= 0 && iLoanID <= 0)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing ContactId OR LoanId.\"}");
            this.Response.End();
        }
        
        if (iContactID <= 0 && iLoanID > 0)
        {
            object ob = LPWeb.DAL.DbHelperSQL.GetSingle(string.Format("Select dbo.lpfn_GetBorrowerContactId({0})", iLoanID));
            if (ob == null || ob == DBNull.Value)
            {
                this.Response.Write(string.Format("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Cannot get BorrowerContactId for LoanId {0}.\"}", iLoanID));
                this.Response.End();
            }
            iContactID = (int)ob;
        }
        #endregion

        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"error message"}

        #region UpdateBorrowerRequest

        UpdateBorrowerResponse response = null;

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                UpdateBorrowerRequest req = new UpdateBorrowerRequest();
                ReqHdr hdr = new ReqHdr();
                hdr.UserId = this.CurrUser.iUserID;
                req.hdr = hdr;
                req.ContactId = iContactID;

                response = service.UpdateBorrower(req);
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("Exception happened when send UpdateBorrowerRequest to Point Manager (ContactID={0}): {1}", iContactID, "Point Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);

            // update Contacts.UpdatePoint=1
            this.UpdatePoint(iContactID, true);

            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sExMsg + "\"}");
            this.Response.End();
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Exception happened when send UpdateBorrowerRequest to Point Manager (ContactID={0}): {1}", iContactID, ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);

            // update Contacts.UpdatePoint=1
            this.UpdatePoint(iContactID, true);

            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sExMsg + "\"}");
            this.Response.End();
        }

        if (response.hdr.Successful == false)
        {
            string sFailedMsg = "Failed to send UpdateBorrowerRequest to Point Manager: " + response.hdr.StatusInfo;
            LPLog.LogMessage(LogType.Logerror, sFailedMsg);

            // update Contacts.UpdatePoint=1
            this.UpdatePoint(iContactID, true);

            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sFailedMsg + "\"}");
            this.Response.End();
        }
        else
        {
            // update Contacts.UpdatePoint=0
            this.UpdatePoint(iContactID, false);
        }

        #endregion

        System.Threading.Thread.Sleep(1000);

        this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
        this.Response.End();
    }

    /// <summary>
    /// update Contacts.UpdatePoint= 0 or 1
    /// neo 2011-03-13
    /// </summary>
    /// <param name="iContactID"></param>
    /// <param name="bUpdatePoint"></param>
    private void UpdatePoint(int iContactID, bool bUpdatePoint)
    {
        LPWeb.BLL.Contacts ContactManager = new LPWeb.BLL.Contacts();

        try
        {
            ContactManager.UpdatePoint(iContactID, false);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Failed to update the client record (ContactID={0}): {1}", iContactID, ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
        }
    }
}
