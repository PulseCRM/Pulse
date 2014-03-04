using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using Utilities;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Linq;

public partial class PointInfoTab : LayoutsPageBase
{
    public int iFileID = 0;
    int iPointFolderId = 0;
    int iBranchId = 0;
    LoginUser loginUser = new LoginUser();
    PointFiles bllPointFile = new PointFiles();
    PointFolders bllPointFolders = new PointFolders();
    Loans bllLoans = new Loans();
    LPWeb.Model.Loans loanInfo = new LPWeb.Model.Loans();
    LPWeb.Model.PointFiles pointFileInfo = new LPWeb.Model.PointFiles();
    LPWeb.Model.PointFolders pointFolderInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        #region 校验页面参数

        string sReturnUrl = "../Pipeline/ProspectPipelineSummaryLoan.aspx";
        string sErrorMsg = "Failed to load this page: missing required query string.";

        // FileID
        bool bIsValid = PageCommon.ValidateQueryString(this, "FileID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, sErrorMsg, "window.parent.location.href='" + sReturnUrl + "'");
        }
        string sFileID = this.Request.QueryString["FileID"];
        this.hfFileID.Value = sFileID;
        try
        {
            this.iFileID = Convert.ToInt32(sFileID);
        }
        catch { }
        #endregion

        #region LoadbaseInfo
        loanInfo = bllLoans.GetModel(iFileID);

        if (loanInfo != null)
        {
            iBranchId = (loanInfo.BranchID == null) ? 0 : loanInfo.BranchID.Value;

        }

        pointFileInfo = bllPointFile.GetModel(iFileID);

        iPointFolderId = (pointFileInfo == null || pointFileInfo.FolderId <= 0) ? 0 : pointFileInfo.FolderId;
        
        if (iPointFolderId > 0)
        {
            pointFolderInfo = bllPointFolders.GetModel(pointFileInfo.FolderId);
        } 
        #endregion

        if (!IsPostBack)
        {
            BindData(); 
        }
    }

    public void BindData()
    {
        #region Local PointFile  ----pointFileInfo and  pointFolderInfo
        

        

        if (pointFileInfo != null)
        {
            txbLastSync.Text = pointFileInfo.LastImported == null ? "" : pointFileInfo.LastImported.Value.ToString("MM/dd/yyyy hh:mm tt");
            txbFilename.Text = pointFileInfo.Name;
        }

        if (pointFolderInfo==null || string.IsNullOrEmpty(pointFolderInfo.Path))
            txbPath.Text = string.Empty;
        else
            txbPath.Text = pointFolderInfo.Path;
        txbPath.Text += (pointFileInfo == null || string.IsNullOrEmpty(pointFileInfo.Name))?string.Empty :  pointFileInfo.Name;
        // pointFolderInfo.Name;

        txbPath.Enabled = false;
        #endregion

        #region Branch ----loanInfo.BranchID

        Branches bllBranches = new Branches();

        if (iBranchId > 0)
        {
            ddlBranch.DataSource = bllBranches.GetList(" BranchId = " + loanInfo.BranchID);
            ddlBranch.DataBind();
            if (ddlBranch.Items.Count > 0)
            {
                ddlBranch.SelectedIndex = 0;
            }
            ddlBranch.Enabled = false;
        }
        else
        {
            DataTable dt = new DataTable();
            if (loginUser.sRoleName == "Executive")
            {
                dt = bllBranches.GetBranchFilter_Executive(loginUser.iUserID, 0, 0);
            }
            else if (loginUser.sRoleName == "Branch Manager")
            {
                dt = bllBranches.GetBranchFilter_Branch_Manager(loginUser.iUserID, 0, 0);
            }
            else
            {
                dt = bllBranches.GetBranchFilter(loginUser.iUserID, 0, 0);
            }

            ddlBranch.DataSource = dt;
            ddlBranch.DataBind();

            ddlBranch.Enabled = true;
        }


        #endregion

        
        BindOther();

        var folderId = pointFolderInfo != null ? pointFolderInfo.FolderId : 0;
        var fileName = pointFileInfo != null ? pointFileInfo.Name : "";

        #region btnDisabled
//1. Create Point File可用
//a. folder name或filename为空时
//b. Folder name和filename不为空时, 但Point文件不存在
//2. Update Point File可用
//a. Folder name和filename不为空时, 以及Point文件存在
//3. Sync Now 可用
//a. Folder name和filename不为空时, 以及Point文件存在

        var pointFileWcf = GetPointFileInfo();
        btnCreatePointFile.Disabled = true;
        btnSyncNow.Disabled = true;
        btnUpdatePoint.Disabled = true;
        
        //Create Point File
        if (folderId == 0 || string.IsNullOrEmpty(pointFileInfo.Name)) 
        {
            btnCreatePointFile.Disabled = false;
        }
        else if (!pointFileWcf.FileExists) //not exists
        {
            btnCreatePointFile.Disabled = false;
        }
        else
        {
            btnSyncNow.Disabled = false;
            btnUpdatePoint.Disabled = false;
        }


        #endregion

        if (folderId != 0)
        {
            ddlFolderName.Enabled = false;
        }
        if (!string.IsNullOrEmpty(fileName))
        {
            txbFilename.Enabled = false;
        }

    }

    private GetPointFileInfoResp GetPointFileInfo()
    {
        #region GetPointFileInfo

        GetPointFileInfoReq req = new GetPointFileInfoReq();
        GetPointFileInfoResp resp = new GetPointFileInfoResp();

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {


                req.FileId = iFileID;
                req.hdr = new ReqHdr() { UserId = loginUser.iUserID };

                resp = service.GetPointFileInfo(req);

                if (resp == null)
                {
                    return new GetPointFileInfoResp();
                }

                return resp;
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("reason: Point Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);

            return new GetPointFileInfoResp();
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("error: {0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            return new GetPointFileInfoResp();
        }

        #endregion
    }

    protected void btnCreatePointFile_Click(object sender, EventArgs e)
    {

    }

    protected void btnUpdatePoint_Click(object sender, EventArgs e)
    {

    }

    protected void btnSyncNow_Click(object sender, EventArgs e)
    {

    }

    protected void ddlBranch_OnChanged(object sender, EventArgs e)
    {
        BindOther();
    }

    private void BindOther()
    {
        if (iBranchId == 0 && !string.IsNullOrEmpty(ddlBranch.SelectedValue))
        {
            iBranchId = Convert.ToInt32(ddlBranch.SelectedValue);
        }

        #region Loan Officer

        LoanTeam bllLoanTeam = new LoanTeam();
        Users bllUsers = new Users();
        var loanOfficer = bllLoanTeam.GetLoanOfficer(iFileID);
        var loanOfficerId = bllLoanTeam.GetLoanOfficerID(iFileID);

        if (!string.IsNullOrEmpty(loanOfficer))
        {
            ddlLoanOfficer.DataTextField = "text";
            ddlLoanOfficer.DataValueField = "value";
            ddlLoanOfficer.DataSource = new ListItemCollection() { new ListItem() { Selected = true, Text = loanOfficer, Value = loanOfficerId.ToString() } };
            ddlLoanOfficer.DataBind();
        }
        else
        {
            if (iBranchId > 0)
            {
                ddlLoanOfficer.DataTextField = "UserName";
                ddlLoanOfficer.DataValueField = "UserId";
                //ddlLoanOfficer.DataSource = bllUsers.GetAllUsers(loanInfo.BranchID.Value);
                ddlLoanOfficer.DataSource = bllUsers.GetAllUsers(iBranchId);
                ddlLoanOfficer.DataBind();
            }
            else
            {
                if (loginUser.sRoleName == "Executive")
                {

                    ddlLoanOfficer.DataTextField = "UserName";
                    ddlLoanOfficer.DataValueField = "UserId";
                    ddlLoanOfficer.DataSource = bllUsers.GetUserListByBranches_Executive(loginUser.iUserID);
                    ddlLoanOfficer.DataBind();

                }
                else if (loginUser.bIsBranchManager || loginUser.bIsBranchUser)
                {

                    ddlLoanOfficer.DataTextField = "UserName";
                    ddlLoanOfficer.DataValueField = "UserId";
                    ddlLoanOfficer.DataSource = bllUsers.GetUserListByUserBranches(loginUser.iUserID);
                    ddlLoanOfficer.DataBind();
                }
            }

            if (ddlLoanOfficer.Items.Count > 0 && loanOfficerId != 0)
            {
                ddlLoanOfficer.SelectedValue = loanOfficerId.ToString();
                //ddlLoanOfficer.SelectedIndex = 0;
            }

        }

        #endregion

        #region FolderName
        ddlFolderName.Enabled = true;
        //if (pointFileInfo.FolderId != 0)
        //{
        //    ddlFolderName.DataTextField = "text";
        //    ddlFolderName.DataValueField = "value";
        //    ddlFolderName.DataSource = new ListItemCollection() { new ListItem() { Text = pointFolderInfo.Name, Value = pointFolderInfo.FolderId.ToString(), Selected = true } };
        //    ddlFolderName.DataBind();
        //    //ddlFolderName.Enabled = false;
        //}
        //else 

        //CR60 ADD
        Company_Point bllcomPoint = new Company_Point();
        //LPWeb.BLL.ArchiveLeadStatus bllLeadStatus = new ArchiveLeadStatus();
        LPWeb.Model.Company_Point comPointInfo = bllcomPoint.GetModel();

        //int leadStatus = 0;
        //LPWeb.Model.ArchiveLeadStatus statusInfo = bllLeadStatus.GetModelList(" LeadStatusName ='" + loanInfo.ProspectLoanStatus + "'").FirstOrDefault();
        //if (statusInfo != null && statusInfo.LeadStatusName == loanInfo.ProspectLoanStatus)
        //{
        //    leadStatus = statusInfo.LeadStatusId;
        //}

        if (comPointInfo != null && comPointInfo.Enable_MultiBranchFolders)
        {
            ddlFolderName.DataTextField = "Name";
            ddlFolderName.DataValueField = "FolderId";
            ddlFolderName.DataSource = bllPointFolders.GetList(" [Enabled] = 1 AND  (BranchID IS NOT NULL)  AND (LoanStatus= 8 ) order by [Name] asc");
            ddlFolderName.DataBind();
        }
        else
        {
            #region CR60前绑定逻辑
            if (iBranchId > 0 || Convert.ToInt32(ddlBranch.SelectedValue) > 0)
            {
                if (loanInfo.ProspectLoanStatus == "Active")
                {
                    ddlFolderName.DataTextField = "Name";
                    ddlFolderName.DataValueField = "FolderId";
                    ddlFolderName.DataSource = bllPointFolders.GetList(" [Enabled] = 1 AND BranchId = " + iBranchId + " AND LoanStatus  = 6  order by Name");
                    ddlFolderName.DataBind();

                }
                else
                {
                    ddlFolderName.DataTextField = "Name";
                    ddlFolderName.DataValueField = "FolderId";
                    ddlFolderName.DataSource = bllPointFolders.GetList(" [Enabled] = 1 AND BranchId = " + iBranchId + " AND LoanStatus  = 8  order by Name");
                    ddlFolderName.DataBind();
                }
            }
            else
            {
                string whereStr = " [Enabled] = 1 ";
                if (loanInfo.ProspectLoanStatus == "Active")
                {
                    whereStr += " AND LoanStatus  = 6 ";
                }
                else
                {
                    whereStr += " AND LoanStatus  = 8 ";
                }

                if (loginUser.bIsCompanyExecutive)
                {
                    whereStr += " AND BranchId IN (select BranchId from dbo.lpfn_GetUserBranches_Executive(" + loginUser.iUserID + "))";
                }
                else if (loginUser.bIsBranchManager)
                {
                    whereStr += " AND BranchId IN (select BranchId from dbo.lpfn_GetUserBranches_Branch_Manager(" + loginUser.iUserID + "))";
                }
                else
                {
                    whereStr += " AND BranchId IN (select BranchId from dbo.lpfn_GetUserBranches(" + loginUser.iUserID + "))";
                }

                whereStr += " order by Name ";

                ddlFolderName.DataTextField = "Name";
                ddlFolderName.DataValueField = "FolderId";
                ddlFolderName.DataSource = bllPointFolders.GetList(whereStr);
                ddlFolderName.DataBind();
            } 
            #endregion

        }


        if (ddlFolderName.Items.Count > 0 && iPointFolderId > 0)
        {
            ddlFolderName.SelectedValue = iPointFolderId.ToString();
            //ddlFolderName.SelectedIndex = 0;
        }

        #endregion
    }
}
