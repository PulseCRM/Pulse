using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.BLL;
using Utilities;


public partial class Contact_PartnerContactDetailNoteDetail : LayoutsPageBase
{
    private string imgSrc = string.Empty;

    #region Event
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int contactId = 0;
            int noteId = 0;
            if (Request.QueryString["contactId"] != null)
                int.TryParse(Request.QueryString["contactId"], out contactId);

            if (Request.QueryString["noteId"] != null)
                int.TryParse(Request.QueryString["noteId"], out noteId);

            if (contactId < 1 && noteId < 1)
            {
                btnSave.Enabled = false;
                tbxNote.Enabled = false;
                PageCommon.AlertMsg(this, "Invalid parameter");
                return;
            }

            if (noteId > 0)
            {
                btnSave.Enabled = false;
                tbxNote.Enabled = false;
            }

            hfdContactId.Value = contactId.ToString();
            BindPage(contactId, noteId);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string err = "";
        try
        {
            if (tbxNote.Text.Trim() == "")
            {
                PageCommon.AlertMsg(this, "Please input the note!");
                return;
            }
            //if (tbxNote.Text.Trim().Length > 500)
            //{
            //    PageCommon.AlertMsg(this, "The note length must be less than 500 characters!");
            //    return;
            //}
            int contactId = int.Parse(hfdContactId.Value);
            var curUser = new LoginUser();
            string senderName = curUser.sFirstName + " " + curUser.sLastName;

            var req = new AddNoteRequest
            {
                FileId = contactId,
                Created = DateTime.Now,
                Note = tbxNote.Text.Trim(),
                Sender = senderName,
                hdr = new ReqHdr
                {
                    UserId = curUser.iUserID
                }
            };
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient client = sm.StartServiceClient())
            {

                //AddNoteResponse res = client.AddNote(req);
                //bool exported = !res.hdr.Successful ? false : true;
                bool exported = true;
                LPWeb.Model.ContactNotes model = new LPWeb.Model.ContactNotes
                {
                    Note = tbxNote.Text,
                    ContactId = contactId,
                    Created = DateTime.Now,
                    CreatedBy = curUser.iUserID
                };

                ContactNotes bllContactNotes = new ContactNotes();
                bllContactNotes.Add(model);

                //if (!exported)
                //{
                //    PageCommon.WriteJs(this, res.hdr.StatusInfo, "parent.ClosePopupWindow();");
                //}
                //else
                //{
                PageCommon.WriteJs(this, "Added note successfully.", "parent.ClosePopupWindow();");
                //PageCommon.WriteJsEnd(this, "Add note Failed.", PageCommon.Js_RefreshSelf);
                //}
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            LPLog.LogMessage(ee.Message);
            PageCommon.AlertMsg(this, "Failed to add note, reason: Point Manager is not running.");
        }
        catch (Exception exception)
        {
            err = "Failed to add note, reason:" + exception.Message;
            LPLog.LogMessage(err);
            PageCommon.WriteJsEnd(this, err, PageCommon.Js_RefreshSelf);
        }
    }
    #endregion

    #region Function
    /// <summary>
    /// load data
    /// </summary>
    /// <param name="contactId"></param>
    /// <param name="noteId"></param>
    private void BindPage(int contactId, int noteId)
    {
        ContactNotes bllContactNotes = new ContactNotes();
        Contacts bllContact = new Contacts();

        LPWeb.Model.Contacts modelContact = bllContact.GetModel(contactId);
        if (modelContact != null)
            lblProperty.Text = modelContact.MailingAddr + " " + modelContact.MailingCity + " " + modelContact.MailingState +
                               " " + modelContact.MailingZip;

        var curUser = new LoginUser();
        lblSender.Text = curUser.sFirstName + " " + curUser.sLastName;
        lblBorrower.Text = modelContact.LastName + ", " + modelContact.FirstName + " " + modelContact.MiddleName;

        if (noteId > 0)
        {
            tbxNote.Text = bllContactNotes.GetModel(noteId).Note;
        }
    }
    #endregion
}
