using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using System.Collections.Generic;
using LPWeb.Common;
using Utilities;

public partial class LoanDetails_AnssignLoanTeam_Ajax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"错误信息"}

        int iFileID = 0;
        int iUserID = 0;
        int iLoanRoleID = 0;

        int iCurrrentUserID = this.CurrUser.iUserID;

        #region 校验页面参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "FileID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string sFileID = this.Request.QueryString["FileID"];
        iFileID = Convert.ToInt32(sFileID);

        bIsValid = PageCommon.ValidateQueryString(this, "UserID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string sUserID = this.Request.QueryString["UserID"];
        iUserID = Convert.ToInt32(sUserID);

        bIsValid = PageCommon.ValidateQueryString(this, "LoanRoleID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string sLoanRoleID = this.Request.QueryString["LoanRoleID"];
        iLoanRoleID = Convert.ToInt32(sLoanRoleID);

        #endregion

        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            #region 调用ReassignProspect

            #region Build ReassignProspectRequest

            ReassignProspectRequest rpq = new ReassignProspectRequest();
            rpq.hdr = new ReqHdr();
            rpq.FileId = new int[1] { iFileID };
            rpq.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            rpq.hdr.UserId = iCurrrentUserID;
            rpq.FromUser = 0;
            rpq.ToUser = iUserID;
            rpq.ContactId = null;
            rpq.UserId = null;

            #endregion

            #region invoke ReassignProspect

            bool bSuccess = false;
            string sError = string.Empty;
            try
            {
                ReassignProspectResponse rpp = service.ReassignProspect(rpq);
                bSuccess = rpp.hdr.Successful;

                if (bSuccess == false)
                {
                    sError = "Failed to invoke API ReassignProspect.";
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ex)
            {
                bSuccess = false;
                sError = "Exception happened: Point Manager is not running.";

                LPLog.LogMessage(ex.Message);
            }
            catch (Exception exception)
            {
                bSuccess = false;
                sError = "Exception happened when invoke API ReassignProspect.";

                LPLog.LogMessage(exception.Message);
            }
            finally
            {
                if (bSuccess == false)
                {
                    this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError + "\"}");
                    this.Response.End();
                }
            }

            #endregion

            #endregion

            #region 调用ReassignLoan

            #region Build ReassignLoanRequest

            ReassignLoanRequest req = new ReassignLoanRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = iCurrrentUserID;

            List<ReassignUserInfo> UserList = new List<ReassignUserInfo>();

            ReassignUserInfo UserInfo = new ReassignUserInfo();
            UserInfo.FileId = iFileID;
            UserInfo.RoleId = iLoanRoleID;
            UserInfo.NewUserId = iUserID;
            UserList.Add(UserInfo);

            req.reassignUsers = UserList.ToArray();

            #endregion

            #region invoke api
            bSuccess = false;
            sError = string.Empty;
            try
            {
                ReassignLoanResponse respone = service.ReassignLoan(req);
                bSuccess = respone.hdr.Successful;

                if (bSuccess == false)
                {
                    sError = "Failed to invoke API ReassignLoan.";
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ex)
            {
                LPLog.LogMessage(ex.Message);

                bSuccess = false;
                sError = "Exception happened: Point Manager is not running.";
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);

                bSuccess = false;
                sError = "Exception happened when invoke API ReassignLoan.";
            }
            finally
            {
                if (bSuccess == false)
                {
                    this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError + "\"}");
                    this.Response.End();
                }
            }
            #endregion

            #endregion
        }

        #region Reassign Loan Team

        LPWeb.Model.LoanTeam lcModel = new LPWeb.Model.LoanTeam();
        lcModel.FileId = iFileID;
        lcModel.RoleId = iLoanRoleID;
        lcModel.UserId = iUserID;

        LPWeb.Model.LoanTeam oldlcModel = new LPWeb.Model.LoanTeam();
        oldlcModel.FileId = iFileID;
        oldlcModel.RoleId = 0;
        oldlcModel.UserId = 0;

        LPWeb.BLL.LoanTeam LoanTeam1 = new LPWeb.BLL.LoanTeam();
        LoanTeam1.Reassign(oldlcModel, lcModel, iCurrrentUserID);

        #endregion

        this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
        this.Response.End();
    }
}
