using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;
using LPWeb.BLL;

namespace LPWeb.Layouts.LPWeb.Prospect
{
    public partial class SubmitLoanPopup_Background : BasePage
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {
            string sExecResult = string.Empty;
            string sErrorMsg = string.Empty;

            try
            {
                int iLoanID = Convert.ToInt32(this.Request.QueryString["loanID"]);
                string sProgram = this.Request.QueryString["program"].ToString();
                string sType=this.Request.QueryString["type"].ToString();

                ImportLoansAndSubmitLoan(iLoanID, sProgram, sType);

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

        private string ImportLoansAndSubmitLoan(int iLoanID, string sProgram, string sType)
        {
            string ReturnMessage = string.Empty;
            ServiceManager sm = new ServiceManager();
            int[] FileIDs=new int[1];
            FileIDs.SetValue(iLoanID,0);
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                ImportLoansRequest req = new ImportLoansRequest();
                req.hdr = new ReqHdr();
                req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                req.hdr.UserId = this.CurrUser.iUserID;
                req.FileIds = FileIDs;

                ImportLoansResponse respone = null;
                try
                {
                    respone = service.ImportLoans(req);

                    if (respone.hdr.Successful)
                    {
                        ReturnMessage = string.Empty;
                    }
                    else
                    {
                        ReturnMessage = respone.hdr.StatusInfo;
                    }
                }
                catch (System.ServiceModel.EndpointNotFoundException)
                {
                    string sExMsg = string.Format("Failed to ImportLoans, reason: ImportLoans Manager is not running.");
                    LPLog.LogMessage(LogType.Logerror, sExMsg);

                    return ReturnMessage;
                }
                catch (Exception ex)
                {
                    string sExMsg = string.Format("Failed toImportLoans, error: {0}", ex.Message);
                    LPLog.LogMessage(LogType.Logerror, sExMsg);

                    return ReturnMessage;
                }


                DT_SubmitLoanRequest reqDT = new DT_SubmitLoanRequest();
                reqDT.hdr = new ReqHdr();
                reqDT.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                reqDT.hdr.UserId = this.CurrUser.iUserID;
                reqDT.FileId = iLoanID;
                reqDT.Loan_Program = sProgram;
                reqDT.Originator_Type = sType;

                DT_SubmitLoanResponse responeDT = null;
                try
                {
                    responeDT = service.DTSubmitLoan(reqDT);

                    if (responeDT.hdr.Successful)
                    {
                        ReturnMessage = string.Empty;
                    }
                    else
                    {
                        ReturnMessage = responeDT.hdr.StatusInfo;
                    }
                }
                catch (System.ServiceModel.EndpointNotFoundException)
                {
                    string sExMsg = string.Format("Failed to ImportLoans, reason: ImportLoans Manager is not running.");
                    LPLog.LogMessage(LogType.Logerror, sExMsg);

                    return ReturnMessage;
                }
                catch (Exception ex)
                {
                    string sExMsg = string.Format("Failed to ImportLoans, error: {0}", ex.Message);
                    LPLog.LogMessage(LogType.Logerror, sExMsg);

                    return ReturnMessage;
                }

                try
                {
                    LoanActivities _bLoanActivities = new LoanActivities();
                    Model.LoanActivities _mLoanActivities = new Model.LoanActivities();
                    _mLoanActivities.FileId = iLoanID;
                    _mLoanActivities.UserId = this.CurrUser.iUserID;
                    _mLoanActivities.ActivityName = "The loan has been submitted to DataTrac by " + this.CurrUser.sFullName;
                    _mLoanActivities.ActivityTime = DateTime.Now;
                    int iNew = _bLoanActivities.Add(_mLoanActivities);
                }
                catch (Exception ex )
                {
                    string sExMsg = string.Format("Failed to Update LoanActivities, error: {0}", ex.Message);
                    LPLog.LogMessage(LogType.Logerror, sExMsg);

                    return ReturnMessage;
                }

                return ReturnMessage;
            }
        }
    }
}
