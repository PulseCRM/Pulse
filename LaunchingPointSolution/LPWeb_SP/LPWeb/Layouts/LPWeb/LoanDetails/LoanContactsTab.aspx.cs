using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using Utilities;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.Model;

public partial class LoanContactsTab : BasePage
{
    int iLoanID = 0;
    LoginUser CurrentUser;
    private readonly LPWeb.BLL.LoanContacts contactMgr = new LPWeb.BLL.LoanContacts();
    private string sErrorMsg = "Failed to load current page: invalid FileID.";
    private string sReturnPage = "LoanContactsTab.aspx";

    #region Properties

    /// <summary>
    /// Gets or sets the current file id.
    /// </summary>
    /// <value>The current file id.</value>
    protected int CurrentFileId
    {
        set
        {
            hfdFileId.Value = value.ToString();
            ViewState["fileId"] = value;
        }
        get
        {
            if (ViewState["fileId"] == null)
                return 0;
            int fileId = 0;
            int.TryParse(ViewState["fileId"].ToString(), out fileId);

            return fileId;
        }
    }

    /// <summary>
    /// Gets or sets the grid view sort direction.
    /// </summary>
    /// <value>The grid view sort direction.</value>
    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;
            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    /// <summary>
    /// Gets or sets the name of the order.
    /// </summary>
    /// <value>The name of the order.</value>
    public string OrderName
    {
        get
        {
            if (ViewState["orderName"] == null)
                ViewState["orderName"] = "ContactRole";
            return Convert.ToString(ViewState["orderName"]);
        }
        set { ViewState["orderName"] = value; }
    }

    /// <summary>
    /// Gets or sets the type of the order.
    /// </summary>
    /// <value>The type of the order.</value>
    public int OrderType
    {
        get
        {
            if (ViewState["orderType"] == null)
                ViewState["orderType"] = 0;
            return Convert.ToInt32(ViewState["orderType"]);
        }
        set { ViewState["orderType"] = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        this.GetPostBackEventReference(this.btnRemove);
        
        try
        {
            CurrentUser = new LoginUser();
            //权限验证
            if (CurrentUser.userRole.ContactMgmt.ToString().IndexOf('5') > -1)
            {
                if (CurrentUser.userRole.ContactMgmt.ToString().IndexOf('1') == -1)
                {
                    aAdd.Enabled = false;
                }
                if (CurrentUser.userRole.ContactMgmt.ToString().IndexOf('2') == -1)
                {
                    btnUpdate.Enabled = false;
                }
                if (CurrentUser.userRole.ContactMgmt.ToString().IndexOf('3') == -1)
                {
                    this.aRemove.Disabled = true;
                }
                if (CurrentUser.userRole.ContactMgmt.ToString().IndexOf('4') == -1)
                {
                    btnReassign.Disabled = true;
                }
                if (!CurrentUser.userRole.SendEmail)
                {
                    btnSendEmail.Enabled = false;
                }

                if (!CurrentUser.userRole.SendLSR)
                {
                    this.btnSendReport.Enabled = false;
                }
            }
            else
            {
                Response.Redirect("../Unauthorize1.aspx");  // have not View Power
                return;
            }

        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }


        if (Request.QueryString["FileID"] != null) // 如果有FileID
        {
            string sFileID = Request.QueryString["FileID"];

            if (PageCommon.IsID(sFileID) == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
            }

            CurrentFileId = Convert.ToInt32(sFileID);
        }

        iLoanID = CurrentFileId;
        if (CurrentFileId == 0)
        {
            return;
        }

        hfdFileId.Value = CurrentFileId.ToString();

        if (!IsPostBack)
        {
            iLoanID = CurrentFileId;
            CheckLoanStatus();

            BindGrid(1);
            BtnPrivilege();
        }
    }
    private void CheckLoanStatus()
    {
        LPWeb.BLL.Loans loanMgr = new LPWeb.BLL.Loans();

        this.hdnActiveLoan.Value = loanMgr.IsActiveLoan(iLoanID) == true ? "True" : "False";
    }

    private void BtnPrivilege()
    {
        try
        {
            if (this.hdnActiveLoan.Value.ToUpper() == "FALSE")
            {
                this.btnRemove.Attributes["OnClientClick"] = "return false;";
                this.btnSendEmail.Attributes["OnClick"] = "return false;";
                this.btnReassign.Attributes["OnClick"] = "return false;";
            }
            else
            {
                this.btnRemove.Attributes["OnClientClick"] = "return BeforeDelete();";
            }
        }
        catch
        {

        }
        #region
        //LPWeb.Model.Roles role = CurrentUser.userRole;
        //if (role == null)
        //{
        //    return;
        //}
        //if (role.ContactMgmt.ToString().Contains("1"))
        //{
        //    btnNew.Enabled = true;
        //}
        //else
        //{
        //    btnNew.Enabled = false;
        //}

        //if (role.ContactMgmt.ToString().Contains("2"))
        //{
        //    btnUpdate.Enabled = true;
        //}
        //else
        //{
        //    btnUpdate.Enabled = false;
        //}


        //if (role.AssignLoan)
        //{
        //    btnReassign.Enabled = true;
        //    btnRemove.Enabled = true;
        //}
        //else
        //{
        //    btnReassign.Enabled = false;
        //    btnRemove.Enabled = false;
        //}

        //if (role.SendEmail)
        //{
        //    btnSendEmail.Enabled = true;
        //}
        //else
        //{
        //    btnSendEmail.Enabled = false;
        //}
        #endregion
    }

    private void BindGrid(int pageIndex)
    {
        //if (CurrentFileId < 1)
        //{
        //    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
        //    return;
        //}
        try
        {
            int pageSize = AspNetPager1.PageSize;
            int recordCount = 0;
            DataSet ds = contactMgr.GetLoanContacts(pageSize, pageIndex, " FileId=" + CurrentFileId.ToString(), out recordCount, OrderName, OrderType);
            //DataSet ds = contacts.GetLoanContacts(pageSize, pageIndex, "", out recordCount, OrderName, OrderType);

            AspNetPager1.RecordCount = recordCount;
            AspNetPager1.CurrentPageIndex = pageIndex;
            AspNetPager1.PageSize = pageSize;

            gvContacts.DataSource = ds;
            gvContacts.DataBind();
        }
        catch
        { }
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        BindGrid(AspNetPager1.CurrentPageIndex);
    }

    protected void gvContacts_Sorting(object sender, GridViewSortEventArgs e)
    {
        OrderName = e.SortExpression;
        string sortExpression = e.SortExpression;
        if (GridViewSortDirection == SortDirection.Ascending) //设置排序方向
        {
            GridViewSortDirection = SortDirection.Descending;
            OrderType = 0;
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            OrderType = 1;
        }
        BindGrid(AspNetPager1.CurrentPageIndex);
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            string ContactIDs = this.hfContactIDs.Value;
            
            contactMgr.DeleteLoanContacts(CurrentFileId, ContactIDs);
        }
        catch
        {
            PageCommon.WriteJsEnd(this, "Failed to delete contact.", PageCommon.Js_RefreshSelf);
        }

        PageCommon.WriteJsEnd(this, "Delete contact successfully.", PageCommon.Js_RefreshSelf);
    }

    protected void lbtnSendNow_Click(object sender, EventArgs e)
    {
        try
        {
            string ContactIDs = this.hfContactIDs.Value;
            string[] Ids = ContactIDs.Split(",".ToCharArray());
            string ReturnMessage = string.Empty;
            foreach (string cid in Ids)
            {
                int ContactID = 0;
                bool External = true;
                if (cid.Contains("User"))
                {
                    ContactID = int.Parse(cid.Replace("User", ""));
                    LPWeb.BLL.Users blluser = new LPWeb.BLL.Users();
                    var userMod = blluser.GetModel(ContactID);

                    if (string.IsNullOrEmpty(userMod.EmailAddress))
                    {
                        PageCommon.AlertMsg(this, "The selected recipient " + userMod.LastName + "," + userMod.FirstName + " does not have an email address.");
                        return;
                    }

                    External = true;
                    ReturnMessage += SendLSREmail(0, string.Empty, string.Empty, ContactID, userMod.EmailAddress, userMod.FirstName + " " + userMod.LastName, External, null);
                    //ReturnMessage = SendExternalReport(0,ContactID, External);
                }
                else if (cid.Contains("Contract"))
                {
                    ContactID = int.Parse(cid.Replace("Contract", ""));

                    LPWeb.BLL.Contacts bllcontacts = new LPWeb.BLL.Contacts();
                    var contactMod = bllcontacts.GetModel(ContactID);
                    if (string.IsNullOrEmpty(contactMod.Email))
                    {
                        PageCommon.AlertMsg(this, "The selected recipient " + contactMod.LastName + "," + contactMod.FirstName + " does not have an email address.");
                        return;
                    }


                    External = true;
                    ReturnMessage += SendLSREmail(ContactID, contactMod.Email, contactMod.FirstName + ' ' + contactMod.LastName, 0, string.Empty, string.Empty, External, null);
                    //ReturnMessage = SendExternalReport(ContactID,0, External);
                }

                if (string.IsNullOrEmpty(ReturnMessage))
                {
                    PageCommon.AlertMsg(this, "The report has been sent successfully!");
                }
                else
                {
                    PageCommon.AlertMsg(this, "Failed to send the report, error:" + ReturnMessage);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            PageCommon.AlertMsg(this, "Failed to disable the selected contact role(s).");
            LPLog.LogMessage(LogType.Logerror, "Failed to disable the selected contact role(s), exception: " + ex.Message);
            PageCommon.AlertMsg(this, "Failed to send the report, error:" + ex.Message);
        }
    }

    private int GetTemplReportId()
    {
        int TemplReportId = 0;
        try
        {
            LPWeb.BLL.Template_Reports bll = new LPWeb.BLL.Template_Reports();
            DataSet ds = bll.GetList(1, "Name='LSR'", "TemplReportId");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TemplReportId = int.Parse(ds.Tables[0].Rows[0]["TemplReportId"].ToString());
            }
        }
        catch
        { }

        return TemplReportId;
    }
    private string SendLSREmail(int ContactID, string ContactEmail, string ContactName, int UserID, string UserEmail, string UserName, bool External, Dictionary<string, byte[]> Attachments)
    {
        string ReturnMessage = string.Empty;
        ServiceManager sm = new ServiceManager();
        LPWeb.BLL.Loans _bLoan = new LPWeb.BLL.Loans();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            SendLSRRequest req = new SendLSRRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = this.CurrUser.iUserID;
            req.FileId = iLoanID;
            req.TemplReportId = GetTemplReportId();
            req.External = External;
            req.Attachments = Attachments;
 
            if (ContactID > 0)
            {
                req.ToContactId = ContactID;
                req.ToContactEmail = ContactEmail;
                req.ToContactUserName = ContactName;
            }
            if (UserID > 0)
            {
                req.ToUserId = UserID;
                req.ToUserEmail = UserEmail;
                req.ToUserUserName = UserName;
            }

            req.LoanAutoEmailid = SaveLoanAutoEmailid(req.FileId, req.TemplReportId, req.ToContactId, req.ToUserId, req.External);

            SendLSRResponse respone = null;
            try
            {
                respone = service.SendLSR(req);

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
                string sExMsg = string.Format("Failed to send email, reason: Email Manager is not running.");
                LPLog.LogMessage(LogType.Logerror, sExMsg);
                ReturnMessage = sExMsg;
            }
            catch (Exception ex)
            {
                string sExMsg = string.Format("Failed to send email, error: {0}", ex.Message);
                LPLog.LogMessage(LogType.Logerror, sExMsg);
                ReturnMessage = sExMsg;
            }

            return ReturnMessage;
        }
    }
    private int SaveLoanAutoEmailid(int FileId, int TemplReportId, int ToContactId, int ToUserId, bool External)
    {
        LPWeb.BLL.LoanAutoEmails bll = new LPWeb.BLL.LoanAutoEmails();

        int LoanAutoEmailId = bll.GetLoanAutoEmailIdByContactUserId(FileId, ToUserId, ToContactId);

        LPWeb.Model.LoanAutoEmails model = new LPWeb.Model.LoanAutoEmails();
        // model.LoanAutoEmailid
        model.Enabled = true;
        model.External = External;
        model.FileId = FileId;
        model.ScheduleType = 0;
        model.TemplReportId = TemplReportId;
        model.ToContactId = ToContactId;
        model.ToUserId = ToUserId;
        model.Applied = DateTime.Now;
        model.LastRun = null;
        if (LoanAutoEmailId <= 0)
            model.LoanAutoEmailid = bll.Add(model);
        else
        {
            model.LoanAutoEmailid = LoanAutoEmailId;
        }
        return model.LoanAutoEmailid;
    }
    private string SendExternalReport(int ContactID, int UserID, bool External)
    {
        string ReturnMessage = string.Empty;
        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            GenerateReportRequest req = new GenerateReportRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = this.CurrUser.iUserID;
            req.External = External;
            req.FileId = this.iLoanID;
            req.TemplReportId = GetTemplReportId();

            GenerateReportResponse respone = null;
            try
            {
                respone = service.GenerateReport(req);
                string SendEmailReturnMessage = string.Empty;
                if (respone.hdr.Successful)
                {
                    ReturnMessage = SendEmail( ContactID,UserID, respone.ReportContent);
                }
                else
                {
                    ReturnMessage = respone.hdr.StatusInfo;
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                string sExMsg = string.Format("Exception happened when Send Report (FileID={0}): {1}", this.iLoanID, "Point Manager is not running.");
                LPLog.LogMessage(LogType.Logerror, sExMsg);
                ReturnMessage = sExMsg;
            }
            catch (Exception ex)
            {
                string sExMsg = string.Format("Exception happened when Send Report (FileID={0}): {1}", this.iLoanID, ex.Message);
                LPLog.LogMessage(LogType.Logerror, sExMsg);
                ReturnMessage = sExMsg;
            }

            return ReturnMessage;
        }
    }

    private string SendEmail(int ContactID,int UserID, byte[] EmailBody)
    {
        string ReturnMessage = string.Empty;
        ServiceManager sm = new ServiceManager();
        LPWeb.BLL.Loans _bLoan=new LPWeb.BLL.Loans();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            SendEmailRequest req = new SendEmailRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = this.CurrUser.iUserID;
            req.EmailBody = EmailBody;
            req.EmailSubject = string.Format("Loan Status Report for {0}'s loan", _bLoan.GetLoanBorrowerName(iLoanID));
            req.UserId = this.CurrUser.iUserID;
            if (ContactID > 0)
            {
                req.ToContactIds = new int[1] { ContactID };
            }
            if (UserID > 0)
            {
                req.ToUserIds = new int[1] { UserID };
            }
            SendEmailResponse respone = null;
            try
            {
                respone = service.SendEmail(req);

                if (respone.resp.Successful)
                {
                    ReturnMessage = string.Empty;
                }
                else
                {
                    ReturnMessage = respone.resp.StatusInfo;
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                string sExMsg = string.Format("Failed to send email, reason: Email Manager is not running.");
                LPLog.LogMessage(LogType.Logerror, sExMsg);
            }
            catch (Exception ex)
            {
                string sExMsg = string.Format("Failed to send email, error: {0}", ex.Message);
                LPLog.LogMessage(LogType.Logerror, sExMsg);
            }

            return ReturnMessage;
        }
    }

    private void UpdateEmailSettings(int ContactID, int UserID, int ScheduleType, bool Enabled, int iTempReportID)
    {
        LPWeb.Model.LoanAutoEmails model = new LPWeb.Model.LoanAutoEmails();
        model.FileId = this.iLoanID;
        model.Enabled = Enabled;
        model.ScheduleType = ScheduleType;
        model.ToContactId = ContactID;
        model.ToUserId = UserID;
        model.Applied = DateTime.Now;
        model.AppliedBy = this.CurrentUser.iUserID;
        if (ContactID != 0 && UserID == 0)
        {
            model.External = true;
        }
        else
        {
            model.External = false;
        }
        if(iTempReportID != 0)
        {
            model.TemplReportId = 1;
        }

        try
        {
            LPWeb.BLL.LoanAutoEmails bll = new LPWeb.BLL.LoanAutoEmails();
            bll.UpdateEmailSettings(model);
        }
        catch
        { }

    }

    /// <summary>
    /// For each selected contact/user, it will write a LoanAutoEmails record with LoanAutoEmails.ScheduleType=1 and Enabled=1
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnSendDaily_Click(object sender, EventArgs e)
    {
        try
        {
            string ContactIDs = this.hfContactIDs.Value;
            string[] Ids = ContactIDs.Split(",".ToCharArray());
            string ReturnMessage = string.Empty;
            foreach (string cid in Ids)
            {
                string ContactID = string.Empty;
                if (cid.Contains("User"))
                {
                    ContactID = cid.Replace("User", "");
                    LPWeb.BLL.Users blluser = new LPWeb.BLL.Users();
                    var userMod = blluser.GetModel(int.Parse(ContactID));

                    if (string.IsNullOrEmpty(userMod.EmailAddress))
                    {
                        PageCommon.AlertMsg(this, "The selected recipient " + userMod.LastName + "," + userMod.FirstName + " does not have an email address.");
                        return;
                    }
                    UpdateEmailSettings(0, int.Parse(ContactID), 1, true, 1);
                }
                else if (cid.Contains("Contract"))
                {

                    ContactID = cid.Replace("Contract", "");

                    LPWeb.BLL.Contacts bllcontacts = new LPWeb.BLL.Contacts();
                    var contactMod = bllcontacts.GetModel(int.Parse(ContactID));
                    if (string.IsNullOrEmpty(contactMod.Email))
                    {
                        PageCommon.AlertMsg(this, "The selected recipient " + contactMod.LastName + "," + contactMod.FirstName + " does not have an email address.");
                        return;
                    }
                    UpdateEmailSettings(int.Parse(ContactID), 0, 1, true, 1);
                }
            }

            PageCommon.WriteJsEnd(this, "The report has been scheduled successfully!", PageCommon.Js_RefreshSelf);
        }
        catch (Exception ex)
        {
            PageCommon.AlertMsg(this, "The operation is Failed .");
            LPLog.LogMessage(LogType.Logerror, "Send Daily is Failed , exception: " + ex.Message);

            PageCommon.WriteJsEnd(this, "Failed to schedule the report, error:" + ex.Message, PageCommon.Js_RefreshSelf);
        }
    }

    protected void lbtnSendWeekly_Click(object sender, EventArgs e)
    {
        try
        {
            string ContactIDs = this.hfContactIDs.Value;
            string[] Ids = ContactIDs.Split(",".ToCharArray());
            string ReturnMessage = string.Empty;
            foreach (string cid in Ids)
            {
                string ContactID = string.Empty;
                if (cid.Contains("User"))
                {
                    ContactID = cid.Replace("User", "");

                    LPWeb.BLL.Users blluser = new LPWeb.BLL.Users();
                    var userMod = blluser.GetModel(int.Parse(ContactID));

                    if (string.IsNullOrEmpty(userMod.EmailAddress))
                    {
                        PageCommon.AlertMsg(this, "The selected recipient " + userMod.LastName + "," + userMod.FirstName + " does not have an email address.");
                        return;
                    }

                    UpdateEmailSettings(0, int.Parse(ContactID), 2, true, 1);
                }
                else if (cid.Contains("Contract"))
                {
                    ContactID = cid.Replace("Contract", "");

                    LPWeb.BLL.Contacts bllcontacts = new LPWeb.BLL.Contacts();
                    var contactMod = bllcontacts.GetModel(int.Parse(ContactID));
                    if (string.IsNullOrEmpty(contactMod.Email))
                    {
                        PageCommon.AlertMsg(this, "The selected recipient " + contactMod.LastName + "," + contactMod.FirstName + " does not have an email address.");
                        return;
                    }

                    UpdateEmailSettings(int.Parse(ContactID), 0, 2, true, 1);
                }

            }

            PageCommon.WriteJsEnd(this, "The report has been scheduled successfully!", PageCommon.Js_RefreshSelf);
        }
        catch (Exception ex)
        {
            PageCommon.AlertMsg(this, "The operation is Failed .");
            LPLog.LogMessage(LogType.Logerror, "Failed to schedule the report , exception: " + ex.Message);

            PageCommon.WriteJsEnd(this, "Failed to schedule the report, error:" + ex.Message, PageCommon.Js_RefreshSelf);
        }
    }

    protected void lbtnDisable_Click(object sender, EventArgs e)
    {
        try
        {
            string ContactIDs = this.hfContactIDs.Value;
            string[] Ids = ContactIDs.Split(",".ToCharArray());
            string ReturnMessage = string.Empty;
            foreach (string cid in Ids)
            {
                string ContactID = string.Empty;
                if (cid.Contains("User"))
                {
                    ContactID = cid.Replace("User", "");

                    LPWeb.BLL.Users blluser = new LPWeb.BLL.Users();
                    var userMod = blluser.GetModel(int.Parse(ContactID));

                    if (string.IsNullOrEmpty(userMod.EmailAddress))
                    {
                        PageCommon.AlertMsg(this, "The selected recipient " + userMod.LastName + "," + userMod.FirstName + " does not have an email address.");
                        return;
                    }

                    UpdateEmailSettings(0, int.Parse(ContactID), 1, false, 0);
                }
                else if (cid.Contains("Contract"))
                {
                    ContactID = cid.Replace("Contract", "");

                    LPWeb.BLL.Contacts bllcontacts = new LPWeb.BLL.Contacts();
                    var contactMod = bllcontacts.GetModel(int.Parse(ContactID));
                    if (string.IsNullOrEmpty(contactMod.Email))
                    {
                        PageCommon.AlertMsg(this, "The selected recipient " + contactMod.LastName + "," + contactMod.FirstName + " does not have an email address.");
                        return;
                    }

                    UpdateEmailSettings(int.Parse(ContactID), 0, 1, false, 0);
                }
            }

            PageCommon.WriteJsEnd(this, "", PageCommon.Js_RefreshSelf);
        }
        catch (Exception ex)
        {
            PageCommon.AlertMsg(this, "The operation is Failed .");
            LPLog.LogMessage(LogType.Logerror, "Disable is Failed , exception: " + ex.Message);
        }
    }
}