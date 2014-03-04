using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.BLL;
using Utilities;

namespace LPWeb.Layouts.LPWeb.Prospect
{
    public partial class ProspectLoanDetailsInfo_BG_ExportToPoint : BasePage
    {

        string sFileID = "";
        int iFileID = 0;
        private Loans LoansManager = new Loans();

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 接受参数

            bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
            if (!bIsValid)
            {
                this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
                this.Response.End();
            }

            string sFileID = this.Request.QueryString["LoanID"];
            

            if (PageCommon.IsID(sFileID) == false)
            {
                iFileID = 0;
            }
            else
            {
                iFileID = int.Parse(sFileID);
            }

            #endregion

            // json示例
            // {"ExecResult":"Success","ErrorMsg":""}
            // {"ExecResult":"Failed","ErrorMsg":"error message"}

           
            string sExMsg = string.Empty;
            string OutJson = string.Empty;
            try
            {
                if (UpdatePointFile(iFileID, true, ref sExMsg) == false)
                {
                    OutJson = GetOutJson("Failed", sExMsg);
                }
                else
                {
                    // PageCommon.WriteJsEnd(this, "Exported to Point file successfully.", this.sRefreshCodes + this.sCloseDialogCodes);

                    OutJson = GetOutJson("Success", sExMsg);
                    
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                sExMsg = string.Format("Failed to export to the Point File, FileId={0}, {1}", this.iFileID, "Point Manager is not running.");
                LPLog.LogMessage(LogType.Logerror, sExMsg);
                //PageCommon.WriteJsEnd(this, sExMsg, this.sRefreshCodes + this.sCloseDialogCodes);

                OutJson = GetOutJson("Failed", sExMsg);

            }
            catch (Exception ex)
            {
                sExMsg = string.Format("Failed to export to the Point File, FileID={0}, Error:{1}", this.iFileID, ex.Message);
                LPLog.LogMessage(LogType.Logerror, sExMsg);
                //PageCommon.WriteJsEnd(this, sExMsg, this.sRefreshCodes + this.sCloseDialogCodes);

                OutJson = GetOutJson("Failed", sExMsg);

            }

            this.Response.Write(OutJson);
            this.Response.End();
        }
        string OutJsonTmp = "{\"ExecResult\":\"{0}\",\"ErrorMsg\":\"{1}\"}";
        private string GetOutJson(string State, string msg)
        {
            return OutJsonTmp.Replace("{0}", State).Replace("{1}", msg);

        }


        private bool UpdatePointFile(int iFileID, bool CreateFile, ref string err)
        {
            int oFolderId = LoansManager.CheckProspectFileFolderId(iFileID);
            string oName = LoansManager.CheckProspectFileName(iFileID);

            ServiceManager sm = new ServiceManager();
            UpdateLoanInfoResponse response = null;
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                #region UpdateLoanInfoRequest

                UpdateLoanInfoRequest req = new UpdateLoanInfoRequest();
                req.hdr = new ReqHdr();
                req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                req.hdr.UserId = this.CurrUser.iUserID;
                req.FileId = iFileID;
                req.CreateFile = CreateFile;
                #endregion

                response = service.UpdateLoanInfo(req);
                err = response.hdr.StatusInfo;
            }
            return response.hdr.Successful;
        }
    }
}
