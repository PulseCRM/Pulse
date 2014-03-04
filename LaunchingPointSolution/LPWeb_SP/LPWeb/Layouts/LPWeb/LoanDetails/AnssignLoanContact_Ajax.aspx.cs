using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using System.Collections.Generic;
using LPWeb.Common;
using Utilities;

public partial class LoanDetails_AnssignLoanContact_Ajax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"错误信息"}

        int iFileID = 0;
        int iContactID = 0;
        int iContactRoleID = 0;

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

        bIsValid = PageCommon.ValidateQueryString(this, "ContactID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string sContactID = this.Request.QueryString["ContactID"];
        iContactID = Convert.ToInt32(sContactID);

        bIsValid = PageCommon.ValidateQueryString(this, "ContactRoleID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }
        string sContactRoleID = this.Request.QueryString["ContactRoleID"];
        iContactRoleID = Convert.ToInt32(sContactRoleID);

        #endregion

        #region 调用ReassignContact

        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            #region Build ReassignContactRequest

            ReassignContactRequest req = new ReassignContactRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = this.CurrUser.iUserID;

            List<ReassignContactInfo> ContactList = new List<ReassignContactInfo>();

            ReassignContactInfo ContactInfo = new ReassignContactInfo();
            ContactInfo.FileId = iFileID;
            ContactInfo.ContactRoleId = iContactRoleID;
            ContactInfo.NewContactId = iContactID;
            ContactList.Add(ContactInfo);

            req.reassignContacts = ContactList.ToArray();

            #endregion

            #region invoke ReassignContact

            bool bSuccess = false;
            string sError = string.Empty;
            try
            {
                ReassignContactResponse respone = service.ReassignContact(req);
                bSuccess = respone.hdr.Successful;

                if (bSuccess == false)
                {
                    sError = "Failed to assign contact, reason:"+respone.hdr.StatusInfo;
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ex)
            {
                bSuccess = false;
                sError = "Failed to assign contact: Point Manager is not running.";

                LPLog.LogMessage(ex.Message);
            }
            catch (Exception exception)
            {
                bSuccess = false;
                sError = "Failed to assign contact. Exception: "+exception.Message;

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
        }

        #endregion

        #region Reassign Loan Contact

        LPWeb.Model.LoanContacts lcModel = new LPWeb.Model.LoanContacts();
        lcModel.FileId = iFileID;
        lcModel.ContactRoleId = iContactRoleID;
        lcModel.ContactId = iContactID;

        LPWeb.Model.LoanContacts oldlcModel = new LPWeb.Model.LoanContacts();
        oldlcModel.FileId = iFileID;
        oldlcModel.ContactRoleId = 0;
        oldlcModel.ContactId = 0;

        LPWeb.BLL.LoanContacts LoanContacts1 = new LPWeb.BLL.LoanContacts();
        LoanContacts1.Reassign(oldlcModel, lcModel, iCurrrentUserID);

        #endregion

        this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
        this.Response.End();
    }
}
