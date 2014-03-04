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
using System.IO;

namespace LPWeb.Layouts.LPWeb.LoanDetails
{
    public partial class SendNowWithAttachments : BasePage
    {
        int iLoanID = 0;
        LoginUser CurrentUser;
        public string CloseDialogCodes = "window.parent.parent.parent.CloseGlobalPopup()";
        private string sErrorMsg = "Failed to load current page: invalid FileID / ContactIDs.";
        string ContactIDs = string.Empty;
        bool NoteSaved = false;
        string sLocalTime = string.Empty;

        Dictionary<string, byte[]> Attachments = new Dictionary<string, byte[]>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["CloseDialogCodes"] != null) // 如果有LoanID
            {
                CloseDialogCodes = Request.QueryString["CloseDialogCodes"].ToString();
            }

            if (Request.QueryString["LoanID"] != null) // 如果有LoanID
            {
                string sFileID = Request.QueryString["LoanID"];

                if (PageCommon.IsID(sFileID) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, CloseDialogCodes);
                }

                iLoanID = Convert.ToInt32(sFileID);
            }

            if (Request.QueryString["ContactIDs"] != null) // 如果有LoanID
            {
                ContactIDs = Request.QueryString["ContactIDs"];
            }
            else
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, CloseDialogCodes);
            }


            ///判定是不是 在执行发送
            if (Request.Form["hidIsSend"] != null)
            {
                string Note = Request.Form["tbxNote"].ToString().Trim();
                sLocalTime = Request.Form["hfLocalTime"].ToString().Trim();
                //Attachment 

                #region Attachments
                if (Request.Files.Count > 0)
                {

                    foreach (var key in Request.Files.AllKeys)
                    {
                        var file = Request.Files[key];

                        if (file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
                        {
                            byte[] bytes = new byte[file.InputStream.Length];
                            file.InputStream.Read(bytes, 0, bytes.Length);
                            file.InputStream.Seek(0, SeekOrigin.Begin);

                            FileInfo info = new FileInfo(file.FileName);

                            Attachments.Add(info.Name, bytes);
                        }

                    }
                }
                #endregion

                if (string.IsNullOrEmpty(Note) && Attachments.Count == 0)
                {
                    ErrorMsg("Attachments / Note is empty.");
                    Response.End();
                }

                /*  todo:Dechao please double check
                var isOK = SaveLoanNotes();
                if (isOK)
                {
                    SendMailWithAttachments();
                }
                 * */
                SaveLoanNotes();
                SendMailWithAttachments();//new

                Response.End();
            }

        }


        protected void SendMailWithAttachments()
        {
            string ReturnMessage = string.Empty;

            try
            {
                //string ContactIDs = this.hfContactIDs.Value;
                string[] Ids = ContactIDs.Split(",".ToCharArray());

                foreach (string cid in Ids)
                {
                    int ContactID = 0;
                    bool External = true;
                    if (cid.Contains("User"))
                    {
                        ContactID = int.Parse(cid.Replace("User", ""));
                        BLL.Users blluser = new Users();
                        var userMod = blluser.GetModel(ContactID);

                        if (string.IsNullOrEmpty(userMod.EmailAddress))
                        {
                            //PageCommon.AlertMsg(this, "The selected recipient " + userMod.LastName + "," + userMod.FirstName + " does not have an email address.");
                            ErrorMsg("The selected recipient " + userMod.LastName + "," + userMod.FirstName + " does not have an email address.");
                            return;
                        }

                        External = true;
                        ReturnMessage += SendEmail(0, "", "", ContactID, userMod.EmailAddress, userMod.LastName + "," + userMod.FirstName, External, Attachments);
                    }
                    else if (cid.Contains("Contract"))
                    {
                        ContactID = int.Parse(cid.Replace("Contract", ""));

                        BLL.Contacts bllcontacts = new Contacts();
                        var contactMod = bllcontacts.GetModel(ContactID);
                        if (string.IsNullOrEmpty(contactMod.Email))
                        {
                            //PageCommon.AlertMsg(this, "The selected recipient " + contactMod.LastName + "," + contactMod.FirstName + " does not have an email address.");
                            ErrorMsg("The selected recipient " + contactMod.LastName + "," + contactMod.FirstName + " does not have an email address.");
                            return;
                        }


                        External = false;
                        ReturnMessage += SendEmail(ContactID, contactMod.Email, contactMod.LastName + "," + contactMod.FirstName, 0, "", "", External, Attachments);
                    }


                }
            }
            catch (Exception ex)
            {
                //PageCommon.AlertMsg(this, "Failed to disable the selected contact role(s).");
                LPLog.LogMessage(LogType.Logerror, "Failed to disable the selected contact role(s), exception: " + ex.Message);
                // PageCommon.AlertMsg(this, "Failed to send the report, error:" + ex.Message);
                ErrorMsg("Failed to send the report, error:" + ex.Message);
            }


            if (string.IsNullOrEmpty(ReturnMessage))
            {
                //PageCommon.AlertMsg(this, "The report has been sent successfully!");
                ErrorMsg(true, "The report has been sent successfully!");
            }
            else
            {
                //PageCommon.AlertMsg(this, "Failed to send the report, error:" + ReturnMessage);
                ErrorMsg("Failed to send the report, error:" + ReturnMessage);
                return;
            }

        }

        private string SendEmail(int ContactID, string ContactEmail, string ContactName, int UserID, string UserEmail, string UserName, bool External, Dictionary<string, byte[]> Attachments)
        {
            string ReturnMessage = string.Empty;
            ServiceManager sm = new ServiceManager();
            Loans _bLoan = new Loans();
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
                //req.LoanAutoEmailid


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
                //Commented out because this causes the Notes to be saved multiple times
                //SaveLoanNotes();//todo:save notes after loan auto email id is created. 

                SendLSRResponse respone = null;
                try
                {
                    //respone = service.SendEmail(req);
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

        private int GetTemplReportId()
        {
            int TemplReportId = 0;
            try
            {
                BLL.Template_Reports bll = new BLL.Template_Reports();
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

        private bool SaveLoanNotes()
        {
            if (NoteSaved)
                return true;
            string err = "";
            string Note = Request.Form["tbxNote"].ToString().Trim();
            bool cbExternalViewing = true;
            if (Request.Form["cbExternalViewing"] == null || string.IsNullOrEmpty(Request.Form["cbExternalViewing"].ToString()))
                cbExternalViewing = false;
            else
                cbExternalViewing = Request.Form["cbExternalViewing"].ToString().ToUpper() == "ON" ? true : false;
            try
            {

                if (Note == "")
                {
                    //PageCommon.AlertMsg(this, "Please input the note!");
                    //ErrorMsg("Please input the note!");
                    return true; //gdc CR43  为空时不添加Note ，需要在调用前综合判定  Note 和 Attachment
                }
                //if (Note.Length > 500)
                //{
                //    //PageCommon.AlertMsg(this, "The note length must be less than 500 characters!");
                //    ErrorMsg("The note length must be less than 500 characters!");
                //    return false;
                //}
                int fileId = iLoanID;
                var curUser = new LoginUser();
                string senderName = curUser.sFirstName + " " + curUser.sLastName;

                DateTime dtNow = Convert.ToDateTime(sLocalTime);

                dtNow.AddMinutes(2);
                dtNow.AddSeconds(5);
                var req = new AddNoteRequest
                {
                    FileId = fileId,
                    Created = dtNow,//DateTime.Now,
                    NoteTime = dtNow,
                    Note = Note,
                    Sender = senderName,
                    hdr = new ReqHdr
                    {
                        UserId = curUser.iUserID
                    }
                };
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    AddNoteResponse res = client.AddNote(req);
                    bool exported = res.hdr.Successful;
                    var model = new Model.LoanNotes
                    {
                        Note = Note,
                        FileId = fileId,
                        Created = dtNow,//DateTime.Now,
                        Sender = senderName,
                        Exported = exported,
                        ExternalViewing = cbExternalViewing
                    };

                    var bllLoanNotes = new LoanNotes();
                    bllLoanNotes.Add(model);
                    NoteSaved = true;
                    if (!exported)
                    {
                        //PageCommon.WriteJs(this, res.hdr.StatusInfo, "parent.ClosePopupWindow();");
                        ErrorMsg(res.hdr.StatusInfo);
                        return false;
                    }
                    else
                    {
                        //PageCommon.WriteJs(this, "Add note successfully.", "parent.ClosePopupWindow();");
                        //PageCommon.WriteJsEnd(this, "Add note Failed.", PageCommon.Js_RefreshSelf);
                        return true;
                    }
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ee)
            {
                LPLog.LogMessage(ee.Message);
                //PageCommon.AlertMsg(this, "Failed to add note, reason: Point Manager is not running.");
                ErrorMsg("Failed to add note, reason: Point Manager is not running.");
                return false;
            }
            catch (Exception exception)
            {
                err = "Failed to add note, reason:" + exception.Message + " LocalTime:" + sLocalTime;
                LPLog.LogMessage(err);
                // PageCommon.WriteJsEnd(this, err, PageCommon.Js_RefreshSelf);
                ErrorMsg(err);
                return false;
            }
        }


        private void ErrorMsg(bool IsSusccess, string msg)
        {
            Response.Write("<script>window.parent.SendOK(" + (IsSusccess ? "1" : "0") + " , '" + msg + "');</script>");
            //Response.End();
        }

        private void ErrorMsg(string msg)
        {
            ErrorMsg(false, msg);
        }

        private int SaveLoanAutoEmailid(int FileId, int TemplReportId, int ToContactId, int ToUserId, bool External)
        {
            BLL.LoanAutoEmails bll = new LoanAutoEmails();

            int LoanAutoEmailId = bll.GetLoanAutoEmailIdByContactUserId(FileId, ToUserId, ToContactId);

            Model.LoanAutoEmails model = new Model.LoanAutoEmails();
            DateTime dtNow = Convert.ToDateTime(sLocalTime);
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
                //Model.LoanAutoEmails oldModel = bll.GetModel(LoanAutoEmailId);
                //if (oldModel == null)
                //    return LoanAutoEmailId;
                //oldModel.Applied = DateTime.Now;
                //oldModel.LastRun = DateTime.Now;
                //oldModel.LoanAutoEmailid = LoanAutoEmailId;
                //bll.Update(oldModel);
                model.LoanAutoEmailid = LoanAutoEmailId;
            }
            return model.LoanAutoEmailid;
        }
    }
}
