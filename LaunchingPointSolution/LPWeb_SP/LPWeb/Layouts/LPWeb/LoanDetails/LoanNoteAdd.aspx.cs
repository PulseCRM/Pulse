using System;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.LP_Service;
using Microsoft.SharePoint.WebControls;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;

namespace LPWeb.Layouts.LPWeb.LoanDetails
{
    public partial class LoanNoteAdd : BasePage
    {
        private string imgSrc = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int fileId = 0;
                int noteId = 0;
                if (Request.QueryString["fileId"] != null)
                    int.TryParse(Request.QueryString["fileId"], out fileId);

                if (Request.QueryString["noteId"] != null)
                    int.TryParse(Request.QueryString["noteId"], out noteId);

                if (fileId < 1 && noteId<1)
                {
                    btnSave.Enabled = false;
                    tbxNote.Enabled = false;
                    PageCommon.AlertMsg(this, "Invalid parameter");
                    return;
                }

                if(noteId>0)
                {
                    btnSave.Enabled = false;
                    tbxNote.Enabled = false;
                }

                hfdFileId.Value = fileId.ToString();
                BindPage(fileId,noteId);

            }
        }

        private void BindPage(int fileId,int noteId)
        {
            var bllLoans = new Loans();
            var bllLoanNotes = new LoanNotes();
            var bllContact = new Contacts();

            Model.Loans modelLoan = bllLoans.GetModel(fileId);
            if (modelLoan != null)
                lblProperty.Text = modelLoan.PropertyAddr + " " + modelLoan.PropertyCity + " " + modelLoan.PropertyState +
                                   " " + modelLoan.PropertyZip;

            var curUser = this.CurrUser;
            lblSender.Text = curUser.sFirstName + " " + curUser.sLastName;
            lblBorrower.Text = bllContact.GetBorrower(fileId);

            if(noteId>0)
            {
                Model.LoanNotes modelNotes = bllLoanNotes.GetModel(noteId);
                tbxNote.Text = modelNotes.Note;
                lblSender.Text = modelNotes.Sender;
                cbExternalViewing.Checked = modelNotes.ExternalViewing;

                #region 加载Condition信息

                DataTable LoanNotesInfo = bllLoanNotes.GetLoanNotesInfo(noteId);
                if (LoanNotesInfo.Rows.Count > 0)
                {
                    if (LoanNotesInfo.Rows[0]["LoanConditionId"] != DBNull.Value)
                    {
                        int iConditionID = Convert.ToInt32(LoanNotesInfo.Rows[0]["LoanConditionId"]);
                        BLL.LoanConditions LoanConditionsMgr = new BLL.LoanConditions();
                        DataTable ConditionInfo = LoanConditionsMgr.GetLoanConditionsInfo(iConditionID);
                        if (ConditionInfo.Rows.Count > 0)
                        {
                            this.lbCondition.Text = ConditionInfo.Rows[0]["CondName"].ToString();
                        }
                    }
                }

                #endregion
            }
        }

        private bool UpdatePointNote(int fileId, DateTime dtNow, string senderName, out string err)
        {
            bool exported = false;
            err = string.Empty;
            try 
            {
                BLL.PointFiles pfMgr = new PointFiles();
                Model.PointFiles pfModel = pfMgr.GetModel(fileId);
                if (pfModel == null || pfModel.FolderId <= 0 || string.IsNullOrEmpty(pfModel.Name) || string.IsNullOrEmpty(pfModel.CurrentImage)) 
                {
                    exported = true;
                    return exported;
                } 
                var req = new AddNoteRequest
                   {
                        FileId = fileId,
                        Created = dtNow,//DateTime.Now,
                        NoteTime=dtNow,
                        Note = tbxNote.Text.Trim(),
                        Sender = senderName,
                        hdr = new ReqHdr
                              {
                                 UserId = this.CurrUser.iUserID
                              }
                    };
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    AddNoteResponse res = client.AddNote(req);
                    exported = !res.hdr.Successful ? false : true;
                    err = res.hdr.StatusInfo;
                }
            } catch (Exception ex)
            {
                return exported;
            }
            return exported;
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
                //if(tbxNote.Text.Trim().Length>500)
                //{
                //    PageCommon.AlertMsg(this, "The note length must be less than 500 characters!");
                //    return;
                //}
                int fileId = int.Parse(hfdFileId.Value);
                var curUser = new LoginUser();
                string senderName = curUser.sFirstName + " " + curUser.sLastName;
                string sLocalTime = hfLocalTime.Value;
                
                DateTime dtNow = Convert.ToDateTime(sLocalTime);
                bool exported = UpdatePointNote(fileId, dtNow, senderName, out err);
                var model = new Model.LoanNotes
                                {
                                    Note = tbxNote.Text,
                                    FileId = fileId,
                                    Created = dtNow,//DateTime.Now,
                                    Sender = senderName,
                                    Exported = exported,
                                    ExternalViewing = cbExternalViewing.Checked
                                };
 
                var bllLoanNotes = new LoanNotes();
                bllLoanNotes.Add(model);

                if (!exported)
                {
                    PageCommon.WriteJs(this, err, "parent.ClosePopupWindow();");
                }
                else
                {
                    PageCommon.WriteJs(this, "Added note successfully.", "parent.ClosePopupWindow();");
                    //PageCommon.WriteJsEnd(this, "Add note Failed.", PageCommon.Js_RefreshSelf);
                }
            }
            catch (Exception exception)
            {
                err = "Failed to add note, reason:" + exception.Message + " LocalTime:" + hfLocalTime.Value;
                LPLog.LogMessage(err);
                PageCommon.WriteJsEnd(this, err, PageCommon.Js_RefreshSelf);
            }
        }
    }
}