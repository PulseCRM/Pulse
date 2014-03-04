using System;
using LPWeb.BLL;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;
using LPWeb.LP_Service;
using Utilities;

public partial class LoanDetails_LoanConditionAddNote : BasePage
{
    int iFileId = 0;
    int iConditionID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        // FileId
        bool bValid = PageCommon.ValidateQueryString(this, "FileId", QueryStringType.ID);
        if (bValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.top.CloseGlobalPopup();");
            return;
        }
        string sFileId = this.Request.QueryString["FileId"];
        this.iFileId = Convert.ToInt32(sFileId);

        // ConditionID
        bValid = PageCommon.ValidateQueryString(this, "ConditionID", QueryStringType.ID);
        if (bValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.top.CloseGlobalPopup();");
            return;
        }
        string sConditionID = this.Request.QueryString["ConditionID"];
        this.iConditionID = Convert.ToInt32(sConditionID);

        #endregion

        Loans LoansMgr = new Loans();

        #region 加载Loans信息

        DataTable LoansInfo = LoansMgr.GetLoanInfo(this.iFileId);
        if (LoansInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid File Id.", "window.top.CloseGlobalPopup();");
            return;
        }

        DataTable PipelineInfo = LoansMgr.GetLoanPipelineInfo(this.iFileId);
        if (PipelineInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid File Id.", "window.top.CloseGlobalPopup();");
            return;
        }
        
        #endregion

        #region 加载LoanConditions信息

        LPWeb.BLL.LoanConditions LoanConditionsMgr = new LPWeb.BLL.LoanConditions();
        DataTable ConditionInfo = LoanConditionsMgr.GetLoanConditionsInfo(this.iConditionID);
        if (ConditionInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid Condition ID.", "window.top.CloseGlobalPopup();");
            return;
        }

        #endregion

        if (this.IsPostBack == false)
        {
            #region Borrower

            string sBorrowerName = LoansMgr.GetLoanBorrowerName(this.iFileId);
            this.lbBorrower.Text = sBorrowerName;

            #endregion

            #region Property Address

            string PropertyAddr = LoansInfo.Rows[0]["PropertyAddr"].ToString();
            string PropertyCity = LoansInfo.Rows[0]["PropertyCity"].ToString();
            string PropertyState = LoansInfo.Rows[0]["PropertyState"].ToString();
            string PropertyZip = LoansInfo.Rows[0]["PropertyZip"].ToString();
            string sPropertyAddress = PropertyAddr + ", " + PropertyCity + ", " + PropertyState + " " + PropertyZip;
            this.lbPropertyAddress.Text = sPropertyAddress;

            #endregion

            #region Point Folder and Point filename

            string PointFolder = PipelineInfo.Rows[0]["Point Folder"].ToString();
            this.lbPointFolder.Text = PointFolder;

            string Filename = PipelineInfo.Rows[0]["Filename"].ToString();
            this.lbPointfilename.Text = Filename;

            #endregion

            this.lbConditionName.Text = ConditionInfo.Rows[0]["CondName"].ToString();
        }
    }

    private bool UpdatePointNote(int fileId, DateTime dtNow, string senderName, out string err)
    {
        bool exported = false;
        err = string.Empty;
        try
        {
            LPWeb.BLL.PointFiles pfMgr = new PointFiles();
            LPWeb.Model.PointFiles pfModel = pfMgr.GetModel(fileId);
            if (pfModel == null || pfModel.FolderId <= 0 || string.IsNullOrEmpty(pfModel.Name) || string.IsNullOrEmpty(pfModel.CurrentImage))
            {
                exported = true;
                return exported;
            }
            var req = new AddNoteRequest
            {
                FileId = fileId,
                Created = dtNow,//DateTime.Now,
                NoteTime = dtNow,
                Note = this.txtNote.Text.Trim(),
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
        }
        catch (Exception ex)
        {
            return exported;
        }
        return exported;
    }

    protected void btnSave_Click(object sender, EventArgs e) 
    {
        string err = "";
        string sNote = this.txtNote.Text.Trim();

        try
        {
            if (this.txtNote.Text.Trim() == "")
            {
                PageCommon.AlertMsg(this, "Please input the note!");
                return;
            }

            DateTime dtNow = DateTime.Now;             

            string senderName = this.CurrUser.sFirstName + " " + this.CurrUser.sLastName;

            bool exported = UpdatePointNote(this.iFileId, dtNow, senderName, out err);

            LoanNotes LoanNotesMgr = new LoanNotes();
            LoanNotesMgr.InsertConditionNote(this.iFileId, this.iConditionID, sNote, this.chkExternalViewingEnabled.Checked, this.CurrUser.sFirstName + " " + this.CurrUser.sLastName);

            // success
            PageCommon.WriteJsEnd(this, "Add Note successfully.", "window.top.RefreshConditionsTab();window.top.CloseGlobalPopup();");
        }
        catch (Exception exception)
        {
            err = "Failed to add note, reason:" + exception.Message;         
            PageCommon.WriteJsEnd(this, err, PageCommon.Js_RefreshSelf);
        }


    }
}
