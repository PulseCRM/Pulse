using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using Utilities;
using System.Data;
using LPWeb.LP_Service;
using System.IO;

public partial class ProspectDetailInfo : BasePage
{
    int iContactID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 检查权限

        LoginUser _curLoginUser = this.CurrUser;

        //权限验证
        if (_curLoginUser.userRole.Prospect.ToString().IndexOf('D') > -1) // View
        {
            if (_curLoginUser.userRole.Prospect.ToString().IndexOf('B') == -1)
            {
                btnModify.Disabled = true;
            }
            if (_curLoginUser.userRole.Prospect.ToString().IndexOf('O') == -1)
            {
                btnSendEmail.Disabled = true;
            }
            if (_curLoginUser.userRole.Prospect.ToString().IndexOf('K') == -1)
            {
                btnLinkLoan.Disabled = true;
            }
            if (_curLoginUser.userRole.Prospect.ToString().IndexOf('G') == -1)
            {
                btnSyncNow.Enabled = false;
            }
            if (_curLoginUser.userRole.Prospect.ToString().IndexOf('C') == -1)
            {
                btnDelete.Enabled = false;
            }

            if (this.CurrUser.userRole.ExportClients == true)
            {
                this.btnSaveAsVCard.Enabled = true;
            }
            else
            {
                this.btnSaveAsVCard.Enabled = false;
            }
        }
        else
        {
            Response.Redirect("../Unauthorize1.aspx");  // have not View Power
            return;
        }

        #endregion

        #region 检查必要参数

        string sReturnUrl = "../Pipeline/ProspectPipelineSummaryLoan.aspx";
        string sErrorMsg = "Failed to load this page: missing required query string.";

        bool bIsValid = PageCommon.ValidateQueryString(this, "ContactID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sErrorMsg, "window.parent.parent.location.href='" + sReturnUrl + "'");
        }
        this.iContactID = Convert.ToInt32(this.Request.QueryString["ContactID"]);
        
        #endregion

        FillLabels();
    }
    private void FillLabels()
    {
        if (iContactID == 0)
        {
            return;
        }
        try
        {
            LPWeb.BLL.Contacts contact = new LPWeb.BLL.Contacts();
            LPWeb.Model.Contacts cModel = contact.GetModel(iContactID);
            lbClient.Text = cModel.LastName + ", " + cModel.FirstName + " " + cModel.MiddleName;
            lbNikeName.Text = cModel.NickName;
            if (cModel.DOB.HasValue)
            {
                lbDOB.Text = cModel.DOB.Value.ToShortDateString();
            }
            else
            {
                lbDOB.Text = string.Empty;
            }
            lbGenCode.Text = cModel.GenerationCode;
            lbSSN.Text = cModel.SSN;
            lbTitle.Text = cModel.Title;
            if (cModel.Experian.HasValue)
                lbExperScore.Text = cModel.Experian.Value.ToString();
            if (cModel.TransUnion.HasValue)
                lbTranScore.Text = cModel.TransUnion.Value.ToString();
            if (cModel.Equifax.HasValue)
                lbEquifax.Text = cModel.Equifax.Value.ToString();
            lbHomePhone.Text = cModel.HomePhone;
            lbCellPhone.Text = cModel.CellPhone;
            lbBusinessPhone.Text = cModel.BusinessPhone;
            lbFax.Text = cModel.Fax;
            lbEmail.Text = cModel.Email;
            lbAddress.Text = cModel.MailingAddr;
            lbAddress1.Text = cModel.MailingCity + ", " + cModel.MailingState + " " + cModel.MailingZip;
        }
        catch
        { }
        try
        {
            LPWeb.BLL.Prospect prospect = new LPWeb.BLL.Prospect();
            LPWeb.Model.Prospect pModel = prospect.GetModel(iContactID);
            lbStatus.Text = pModel.Status;
            lbLeadSource.Text = pModel.LeadSource;//Get referral contact name
            if (pModel.Loanofficer.HasValue)
                lbLoanOfficer.Text = GetUserName(pModel.Loanofficer.Value);
            if (pModel.Created.HasValue)
                lbCreatedOn.Text = pModel.Created.Value.ToShortDateString();
            if (pModel.CreatedBy.HasValue)
                lbCreatedBy.Text = GetUserName(pModel.CreatedBy.Value);
            if (pModel.Modifed.HasValue)
                lbLastModified.Text = pModel.Modifed.Value.ToShortDateString();
            if (pModel.ModifiedBy.HasValue)
                lbModifiedBy.Text = GetUserName(pModel.ModifiedBy.Value);

            int iReferralId = 0;

            if (pModel.Referral != null)
            {
                iReferralId = (int)pModel.Referral;

                LPWeb.BLL.Contacts contact = new LPWeb.BLL.Contacts();
                lbReferral.Text = contact.GetContactName(iReferralId);
            }

            this.lbPreferredContact.Text = pModel.PreferredContact;
            this.lbCreditRanking.Text = pModel.CreditRanking;

        }
        catch
        { }
    }

    private string GetUserName(int UserID)
    {
        LPWeb.BLL.Users user = new LPWeb.BLL.Users();
        try
        {
            LPWeb.Model.Users model = user.GetModel(UserID);
            return model.LastName + ", " + model.FirstName;
        }
        catch
        {
            return string.Empty;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LPWeb.BLL.Prospect prospect = new LPWeb.BLL.Prospect();
            prospect.Delete(iContactID);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Failed to delete the prospect, error:{0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
        }

        PageCommon.WriteJsEnd(this, "Delete prospect successfully.", "window.parent.parent.location.href='../Pipeline/ProspectPipelineSummary.aspx'");
    }

    protected void btnSyncNow_Click(object sender, EventArgs e)
    {
        ImportLoansResponse respone = null;

        try
        {
            LPWeb.BLL.Loans loanMgr = new LPWeb.BLL.Loans();
            string sWhere = " FileId IN(SELECT FileId FROM LoanContacts WHERE (ContactRoleId = 1 OR ContactRoleId = 2) AND ContactId = " + this.iContactID + ")";
            DataSet ds = loanMgr.GetList(sWhere);
            string sFileIds = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sFileIds += (sFileIds == "") ? dr["FileId"].ToString() : ("," + dr["FileId"].ToString());
            }
            if (sFileIds == "")
            {
                //No loans
                PageCommon.WriteJsEnd(this, "Invalid FileId.", PageCommon.Js_RefreshSelf);
                return;
            }
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                ImportLoansRequest req = new ImportLoansRequest();
                string[] selectedItems = sFileIds.Split(',');
                req.FileIds = Array.ConvertAll(selectedItems, item => int.Parse(item));
                req.hdr = new ReqHdr();
                req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                req.hdr.UserId = 5;

                respone = service.ImportLoans(req);
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("Failed to sync with Point, reason: Point Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Failed to sync with Point, error:{0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
        }

        if (respone.hdr.Successful)
        {
            PageCommon.WriteJsEnd(this, "Synched successfully", PageCommon.Js_RefreshSelf);
        }
        else
        {
            PageCommon.WriteJsEnd(this, "Failed to sync with Point.", PageCommon.Js_RefreshSelf);
        }
    }

    protected void btnSaveAsVCard_Click(object sender, EventArgs e) 
    {
        string sCurrentPagePath = this.Server.MapPath("~/");
        string sFileName = Guid.NewGuid().ToString();
        string sFilePath = Path.Combine(Path.GetDirectoryName(sCurrentPagePath), sFileName);

        #region #region Call vCardToString(ContactId, true) API

        LPWeb.BLL.Contacts x = new LPWeb.BLL.Contacts();
        string sContactStr = x.vCardToString(this.iContactID, true);

        #endregion

        // save file
        //if (File.Exists(sFilePath) == false)
        //{
        //    // Create a file to write to.
        //    using (StreamWriter sw = File.CreateText(sFilePath))
        //    {
        //        sw.Write(sContactStr);
        //    }
        //}

        //FileInfo FileInfo1 = new FileInfo(sFilePath);

        this.Response.Clear();
        this.Response.ClearHeaders();
        this.Response.Buffer = false;
        this.Response.ContentType = "application/octet-stream";
        this.Response.AppendHeader("Content-Disposition", "attachment;filename=vcard.vcf");
        //this.Response.AppendHeader("Content-Length", FileInfo1.Length.ToString());
        //this.Response.WriteFile(sFilePath);

        this.Response.AppendHeader("Content-Length", sContactStr.Length.ToString());
        this.Response.Write(sContactStr);
        this.Response.Flush();

        // 删除临时文件
        //File.Delete(sFilePath);

        this.Response.End();
    }

}
