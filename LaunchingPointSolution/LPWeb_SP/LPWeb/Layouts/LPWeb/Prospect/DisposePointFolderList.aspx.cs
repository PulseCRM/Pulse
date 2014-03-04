using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;

public partial class Prospect_DisposePointFolderList : BasePage
{
    public string sAction = string.Empty;
    protected int iLoanID = 0;
    int iBranchID = 0;
    string sCloseDialogCodes = string.Empty;

    public int iContactID = 0;
    DataTable LoanList = null;
    string sBranchIDs = string.Empty;

    private Loans LoansManager = new Loans();
    public string sFileName = "";

    protected string sShowSubmitLoanPopup = "0";

    protected string sShowTemplate = "0";

    private string sParaForDetail = "0";

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        this.sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";

        string sMissing = "Missing required qurey string.";

        #region Action
        if (this.Request.QueryString["Action"] == null)
        {
            PageCommon.WriteJsEnd(this, sMissing, sCloseDialogCodes);
        }
        this.sAction = this.Request.QueryString["Action"].ToString();

        if (this.Request.QueryString["detail"] != null)
        {
            sParaForDetail = this.Request.QueryString["detail"].ToString();
        }


        //CR60 
        Company_Point CPMgr = new Company_Point();
        LPWeb.Model.Company_Point CPModel = CPMgr.GetModel();
        //check if Company_Point.Enable_MultiBranchFolders=true
        //select FolderId, [Name] from PointFolders where (LoanStatus=<selected loan/lead status>) order by [Name] asc
        bool bMultBranchFolder = false;
        if (CPModel.Enable_MultiBranchFolders == true)
        {
            bMultBranchFolder = true;
        }

        if (this.Request.QueryString["ContactID"] == null)
        {
            //if (this.sAction != "Cancel" && this.sAction != "Convert"
            //&& this.sAction != "Suspend" && this.sAction != "Resume" && this.sAction != "Bad" && this.sAction != "Lost")
            //{
            //    PageCommon.WriteJsEnd(this, "Invalid query string.", sCloseDialogCodes);
            //}
        }
        else 
        {
            // 如果传入ContactID，只允许Suspend/Bad/Lost操作
            if (this.sAction != "Suspend" && this.sAction != "Bad" && this.sAction != "Lost")
            {
                PageCommon.WriteJsEnd(this, "Invalid query string.", sCloseDialogCodes);
            }
        }

        #endregion

        if (this.Request.QueryString["ContactID"] == null)  // from Prospect Loan Detail→Dispose
        {
            // LoanID
            bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, sMissing, sCloseDialogCodes);
            }
            this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

            // BranchID
            bIsValid = PageCommon.ValidateQueryString(this, "BranchID", QueryStringType.ID);
            iBranchID = 0;
            if (bIsValid)
            {
                //PageCommon.WriteJsEnd(this, sMissing, sCloseDialogCodes);
                this.iBranchID = Convert.ToInt32(this.Request.QueryString["BranchID"]);
            }           
        }
        else  // From Prospect Detail Popup→Update Point
        {
            // ContactID
            bool bIsValid = PageCommon.ValidateQueryString(this, "ContactID", QueryStringType.ID);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, sMissing, sCloseDialogCodes);
            }
            this.iContactID = Convert.ToInt32(this.Request.QueryString["ContactID"]);

            #region Get FileIDs and BranchIDs associated to ContactID

            string sSql0 = "select a.FileId, c.BranchId from LoanContacts as a inner join PointFiles as b on a.FileId=b.FileId "
                         + "inner join PointFolders as c on b.FolderId = c.FolderId "
                         + "where ContactId=" + this.iContactID + " and (ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId())";

            this.LoanList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);

            #endregion

            #region Build BranchIDs

            foreach (DataRow LoanRow in this.LoanList.Rows)
            {
                string sBranchID = LoanRow["BranchID"].ToString();
                if(this.sBranchIDs == string.Empty)
                {
                    this.sBranchIDs = sBranchID;
                }
                else
                {
                    this.sBranchIDs += "," + sBranchID;
                }
            }

            #endregion
        }

        #region Bug 882 若FIleName为空，不显示Point文件夹列表，更新状态，直接关闭
        sFileName = LoansManager.GetProspectFileNameInfo(this.iLoanID);
       
        if (sFileName == "" && this.sAction != "Convert")
        {
            #region call Workflow API: bool UpdateProspectLoanStatus(int LoanId, string newLoanStatus)
            this.form1.Visible = false;
            this.Visible = false;
            try
            {
                string sError_UpdateProspectLoanStatus = string.Empty;
                if (this.sAction == "Resume***Active")
                {
                    this.sAction = "Active";
                }
                sError_UpdateProspectLoanStatus = WorkflowManager.UpdateProspectLoanStatus(this.iLoanID, this.sAction, this.CurrUser.iUserID);

                if (sError_UpdateProspectLoanStatus != string.Empty)
                {
                    string sFailedMsg = string.Format("Failed to update lead status (FileID={0}): {1}", this.iLoanID, sError_UpdateProspectLoanStatus);
                    // LPLog.LogMessage(LogType.Logerror, sFailedMsg);
                    PageCommon.WriteJsEnd(this, sFailedMsg, sCloseDialogCodes);
                    //PageCommon.WriteJsEnd(this, sFailedMsg, PageCommon.Js_RefreshSelf);
                }
            }
            catch (Exception ex)
            {
                string sExMsg = string.Format("Failed to dispose of the lead, status (FileID={0}): {1}", this.iLoanID, ex.Message);
                // LPLog.LogMessage(LogType.Logerror, sExMsg);

                throw;
            }

            #endregion

            PageCommon.WriteJsEnd(this, "Disposed of the lead successfully.", "window.parent.location.href=window.parent.location.href;");
        } 
        #endregion

        #endregion

        #region 加载Point Folder List

        string sWhere = "1=1";
        if (iBranchID <= 0 && iLoanID > 0)
        {
            string SqlCmd = string.Format("Select BranchId from Loans where FileId={0}", iLoanID);
            object obj = LPWeb.DAL.DbHelperSQL.GetSingle(SqlCmd);
            iBranchID = obj == null ? 0 : (int)obj;
            if (iBranchID <= 0)
            {
                string SqlCmd_01 = string.Format("Select dbo.[lpfn_GetLoanOfficerID]({0})", iLoanID);
                object obj_01 = LPWeb.DAL.DbHelperSQL.GetSingle(SqlCmd_01);
                int LoanOfficerId = obj_01 == null ? 0 : (int)obj_01;
                if (LoanOfficerId > 0)
                {
                    string SqlCmd_02 = string.Format("Select b.BranchId from GroupUsers bu inner join Branches b on bu.GroupId=b.GroupId where bu.UserId={0} ", LoanOfficerId);
                    object obj_02 = LPWeb.DAL.DbHelperSQL.GetSingle(SqlCmd_02);
                    iBranchID = obj_02 == null ? 0 : (int)obj_02;
                }
            }
        }
        //if (iBranchID <= 0)
        //{
        //    PageCommon.WriteJsEnd(this, "Cannot find the branch for the selected lead; no Point folders found.", sCloseDialogCodes);
        //}
        if(this.Request.QueryString["ContactID"] == null)
        {
            #region 传入LoanID和BranchID
            if (bMultBranchFolder == false)
            {
                if (iBranchID > 0)
                    sWhere += " and BranchId=" + this.iBranchID;
            }
            if (sAction == "Convert")  // 特殊
            {
                sWhere += " and LoanStatus=1 and Enabled=1";
            }
            else if (sAction == "Resume***Active")
            {
                sWhere += " and LoanStatus=6 and Enabled=1 "; // sWhere1; bug 970 应弹出“Prospect“的文件夹列表(PointFolders.LoanStatus=6)
            }
            else
            {
                sWhere += " and LoanStatus=8 "; // sWhere1;
            }
          
            #endregion
        }
        else // 传入ContactID
        {
            // Suspend/Bad/Lost 筛选条件相同
            if (bMultBranchFolder == false)
            {
                if (!string.IsNullOrEmpty(this.sBranchIDs))
                    sWhere += " and BranchId in (" + this.sBranchIDs + ") ";
            }

            sWhere += " and LoanStatus<>1 and LoanStatus<>6 ";

        }
        PointFolders PointFolderManager = new PointFolders();
        string orderby = "  Order By Name ";
        DataTable PointFolderListData = PointFolderManager.GetList(sWhere+orderby).Tables[0];
        if (PointFolderListData.Rows.Count == 0)
        {
            this.form1.Visible = false;
            this.Visible = false;
            string folderType = string.IsNullOrEmpty(this.sAction) || this.sAction.Trim().ToUpper() != "CONVERT" ? "Archived Leads " : "Active Loans";
            string err = string.Format("No {0} folder available.", folderType);
            PageCommon.WriteJsEnd(this, err, sCloseDialogCodes);
        }
        this.gridPointFolderList.DataSource = PointFolderListData;
        this.gridPointFolderList.DataBind();

        #endregion

        #region If Company_Point.MasterSource=’DataTrac’
        Company_Point _bCompany_Point = new Company_Point();
        DataTable dt = _bCompany_Point.GetCompany_PointInfo();
        if (dt.Rows.Count > 0)
        {
            sShowSubmitLoanPopup = dt.Rows[0]["MasterSource"].ToString() == "DataTrac" ? "1" : "0";
        } 
        #endregion

        #region sFileName

        sFileName = LoansManager.GetProspectFileNameInfo(this.iLoanID);

        #endregion
    }
    private bool MoveFile(int nFileId, int nFolderId, out string err)
    {
        string strResultInfo = string.Empty;
        err = string.Empty;

        string sProspectStatus = LoansManager.GetProspectStatusInfo(nFileId);
        string sFileName = LoansManager.GetProspectFileNameInfo(nFileId);  //bug 878
        if (sFileName == "")
        {
            err = "The selected lead does not have a Point file.";
            return false;
        }

        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient client = sm.StartServiceClient())
        {
            MoveFileRequest req = new MoveFileRequest();
            req.FileId = nFileId;
            req.NewFolderId = nFolderId;
            req.hdr = new ReqHdr();
            req.hdr.UserId = CurrUser.iUserID;

            MoveFileResponse response = client.MoveFile(req);
            if (response.hdr.Successful)
                return true;

            // LPLog.LogMessage(LogType.Logerror, string.Format("Failed to move file:{0}", response.hdr.StatusInfo));
            //PageCommon.WriteJsEnd(this, response.hdr.StatusInfo);
            err = response.hdr.StatusInfo;
            return false;
        }

    }
    private bool DisposeLead(int nFileId, int nFolderId, string newLeadStatus, out string err)
    {
        string strResultInfo = string.Empty;
        err = string.Empty;
        int oFolderId = LoansManager.CheckProspectFileFolderId(nFileId);
        string oName = LoansManager.CheckProspectFileName(nFileId);

        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient client = sm.StartServiceClient())
        {
            DisposeLeadRequest req = new DisposeLeadRequest();
            req.FileId = nFileId;
            req.NewFolderId = nFolderId;
            req.hdr = new ReqHdr();
            req.hdr.UserId = CurrUser.iUserID;
            req.LoanStatus = newLeadStatus;
            req.StatusDate = DateTime.Now;

            DisposeLeadResponse response = client.DisposeLead(req);
            if (response.hdr.Successful)
                return true;

            // LPLog.LogMessage(LogType.Logerror, string.Format("Failed to dispose file:{0}", response.hdr.StatusInfo));
            //PageCommon.WriteJsEnd(this, response.hdr.StatusInfo);
            err = response.hdr.StatusInfo;
            return false;
        }

    }
    private void SendRequestToService(string strStatus, int nFileId, int nFolderId)
    { 
           // Invoke the PointManager method DisposeLoan, move loan to folder with id nFolderId
            string strTemp = "";
            bool status = false; 
            try
            {
                string strResultInfo = "";

                switch (strStatus.ToLower())
                {
                    case "convert":
                        strTemp = "convert the lead ";
                        status = DisposeLead(nFileId, nFolderId, "Processing", out strResultInfo);
                        if (sShowSubmitLoanPopup == "1")
                        {
                            //Response.Write("<script language='javascript'>ShowSubmitLoanPopup();</script>");
                            this.ClientScript.RegisterStartupScript(this.GetType(), "Submit Loan", "ShowSubmitLoanPopup();", true);
                            return;
                        }
                        //else
                        //{
                        //    this.ClientScript.RegisterStartupScript(this.GetType(), "_$Success2", "alert('" + this.sAction + " lead successfully.');" + sCloseDialogCodes, true);
                        //}
                        break;
                    case "move":
                        strTemp = "move the Point file ";
                        status = MoveFile(nFileId, nFolderId, out strResultInfo);
                        break;
                    case "resume":
                        strTemp = "resume the lead ";
                        status = DisposeLead(nFileId, nFolderId, "Active", out strResultInfo);
                        break;
                    default:
                        strTemp = "archive the lead ";
                        status = DisposeLead(nFileId, nFolderId, strStatus, out strResultInfo);
                        break;
                }
                if (status)
                {
                    PageCommon.WriteJsEnd(this, strTemp+" successfully.", "window.parent.location.href=window.parent.location.href;");                   
                }
                else
                    PageCommon.WriteJsEnd(this, "Failed to " + strTemp + strResultInfo, "window.parent.location.href=window.parent.location.href;"); 
            }
            catch (System.ServiceModel.EndpointNotFoundException ee)
            {
                // LPLog.LogMessage(ee.Message);
                PageCommon.WriteJsEnd(this, string.Format("Failed to {0}, reason: Point Manager is not running.", strTemp),"");
            }
            catch (Exception ex)
            {
                PageCommon.WriteJsEnd(this, string.Format("Failed to {0}, error message: {1} ", strTemp, ex.Message), "");
                // LPLog.LogMessage(LogType.Logerror, string.Format("Failed to dispose/move the lead with id({0}), error message: {1}",
                // nFileId, ex.Message));
            }
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        string sTargetPointFolderID = this.hdnTargetPointFolderID.Value;
        int iTargetPointFolderID = Convert.ToInt32(sTargetPointFolderID);
        string err = string.Empty;
        //sFileName = string.IsNullOrEmpty(sFileName) ? LoansManager.GetProspectFileNameInfo(this.iLoanID) : sFileName;

        if (this.Request.QueryString["ContactID"] == null)
        {
            if (sAction.ToUpper().Contains("RESUME"))
                sAction = "Resume";
            SendRequestToService(this.sAction, this.iLoanID, iTargetPointFolderID);
        }
    }

    //protected void btnYesWithFileName_Click(object sender, EventArgs e)
    //{
    //    if (string.IsNullOrEmpty(hdnFileName.Value.Trim()))
    //    {
    //        PageCommon.WriteJsEnd(this, "Input FileName Text!", "");
    //        return ;
    //    }

    //    LPWeb.Model.PointFiles modelPF = new LPWeb.Model.PointFiles();
    //    LPWeb.BLL.PointFiles bllPF = new PointFiles();
    //    modelPF = bllPF.GetModel(iLoanID);

    //    if (modelPF != null)
    //    {
    //        modelPF.Name = @"BORROWER\" + hdnFileName.Value.Trim();
    //        bllPF.UpdateBase(modelPF);
    //    }
    //    else
    //    {
    //        modelPF = new LPWeb.Model.PointFiles();
    //        modelPF.FileId = iLoanID;
    //        modelPF.Name = @"BORROWER\" + hdnFileName.Value.Trim();

    //        bllPF.Add(modelPF);
    //    }
  
    //    btnYes_Click(sender, e);
    //}

 }
