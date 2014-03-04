using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.BLL;
using System.Data;

public partial class LoanDetails_LoanConditionNoteDetail : BasePage
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

        #region 加载LoanNotes信息

        LoanNotes LoanNotesMgr = new LoanNotes();
        DataTable NoteList = LoanNotesMgr.GetConditionNoteList(this.iFileId, this.iConditionID);
        this.rptNoteList.DataSource = NoteList;
        this.rptNoteList.DataBind();

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

    protected void lnkEnable_Click(object sender, EventArgs e) 
    {
        string NoteIDs = this.hdnNoteIDs.Value;
        if (NoteIDs == string.Empty)
        {
            PageCommon.WriteJsEnd(this, "Please select a note first.", PageCommon.Js_RefreshSelf);
            return;
        }

        LoanNotes LoanNotesMgr = new LoanNotes();
        LoanNotesMgr.EnableExternalViewing(NoteIDs, true);

        // Success
        PageCommon.WriteJsEnd(this, "Enable External Viewing successfully.", PageCommon.Js_RefreshSelf);
    }

    protected void lnkDisable_Click(object sender, EventArgs e)
    {
        string NoteIDs = this.hdnNoteIDs.Value;
        if (NoteIDs == string.Empty)
        {
            PageCommon.WriteJsEnd(this, "Please select a note first.", PageCommon.Js_RefreshSelf);
            return;
        }

        LoanNotes LoanNotesMgr = new LoanNotes();
        LoanNotesMgr.EnableExternalViewing(NoteIDs, false);

        // Success
        PageCommon.WriteJsEnd(this, "Disable External Viewing successfully.", PageCommon.Js_RefreshSelf);
    }
}
