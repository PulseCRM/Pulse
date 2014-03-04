using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.BLL;
using Utilities;
using LPWeb.LP_Service;
using System.Data.SqlClient;

public partial class Prospect_ProspectLoanDetailsInfo : BasePage
{
    protected int iLoanID = 0;
    
    private DataTable GetBorrowerInfo(int iFileId) 
    {
        string sSql5 = "select top 1 b2.* from LoanContacts as b1 inner join Contacts as b2 on b1.ContactId = b2.ContactId where b1.FileId=" + iFileId + "  and b1.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() ";
        DataTable BorrowerInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql5);

        return BorrowerInfo;
    }

    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 检查必要参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "");
        }
        this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

        #endregion

        #region 加载Loan Info

        string sSql3 = "select l.*, pf.FolderId, pf.Name as FileName from Loans l inner join PointFiles pf on l.FileId=pf.FileId where l.FileId=" + this.iLoanID;
        DataTable LoanInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql3);
        if ((LoanInfo == null ) || (LoanInfo.Rows.Count == 0))
        {
            PageCommon.WriteJsEnd(this, string.Format("No Loan Info found for LoanID {0}.", iLoanID), "");
        }

        int FolderId = 0;
        string FileName = string.Empty;
        FolderId = LoanInfo.Rows[0]["FolderId"] == DBNull.Value ? 0 : (int)LoanInfo.Rows[0]["FolderId"];
        FileName = LoanInfo.Rows[0]["FileName"] == DBNull.Value ? string.Empty : (string)LoanInfo.Rows[0]["FileName"];
        if (FolderId < 1 || string.IsNullOrEmpty(FileName))
        {
            lnkImport.Visible = false;
        }

        #endregion

        #region 权限验证
        //try
        //{
        //    if (this.CurrUser.userRole.Prospect.ToString().IndexOf('B') == -1)
        //    {
        //        this.btnModify.Disabled = true;
        //    }
        //    if (this.CurrUser.userRole.Prospect.ToString().IndexOf('K') == -1)
        //    {
        //        this.btnSendEmail.Disabled = true;
        //    }
        //    if (this.CurrUser.userRole.Prospect.ToString().IndexOf('F') == -1)
        //    {
        //        this.btnDispose.Disabled = true;
        //    }
        //    if (this.CurrUser.userRole.Prospect.ToString().IndexOf('C') == -1)
        //    {
        //        btnDelete.Enabled = false;
        //    }
        //    if (this.CurrUser.userRole.Prospect.ToString().IndexOf('G') == -1)
        //    {
        //        btnSyncNow.Enabled = false;
        //    }
        //}
        //catch (Exception exception)
        //{
        //    LPLog.LogMessage(exception.Message);
        //} 


        if (this.CurrUser.userRole.ExportClients == true)
        {
            this.btnvCardExport.Enabled = true;
        }
        else
        {
            this.btnvCardExport.Enabled = false;
        }

        #endregion

        this.GetPostBackEventReference(this.lnkExport);

        if (this.IsPostBack == false)
        {
            #region 加载Lead Source列表

            Company_Lead_Sources LeadSourceManager = new Company_Lead_Sources();
            DataTable LeadSourceList = LeadSourceManager.GetList("1=1 order by LeadSource").Tables[0];

            DataRow NewLeadSourceRow = LeadSourceList.NewRow();
            NewLeadSourceRow["LeadSourceID"] = DBNull.Value;
            NewLeadSourceRow["LeadSource"] = "-- select --";

            LeadSourceList.Rows.InsertAt(NewLeadSourceRow, 0);

            this.ddlLeadSource.DataSource = LeadSourceList;
            this.ddlLeadSource.DataBind();

            #endregion

            #region 加载Program列表

            Company_Loan_Programs ProgramMgr = new Company_Loan_Programs();
            DataTable ProgramList = ProgramMgr.GetList("1=1 order by LoanProgram").Tables[0];

            DataRow NewProgramRow = ProgramList.NewRow();
            NewProgramRow["LoanProgramID"] = DBNull.Value;
            NewProgramRow["LoanProgram"] = "-- select --";

            ProgramList.Rows.InsertAt(NewProgramRow, 0);

            this.ddlProgram.DataSource = ProgramList;
            this.ddlProgram.DataBind();

            #endregion

            #region 加载Borrower Info

            DataTable BorrowerInfo = this.GetBorrowerInfo(this.iLoanID);

            #region 绑定Borrower信息
            if (BorrowerInfo.Rows.Count > 0)
            {
                string sFirstName = BorrowerInfo.Rows[0]["FirstName"].ToString();
                string sLastName = BorrowerInfo.Rows[0]["LastName"].ToString();
                string sBorrowerName = sLastName + ", " + sFirstName;
                this.hProspectName.InnerText = sBorrowerName;
                this.txtFirstName.Text = sFirstName;
                this.txtLastName.Text = sLastName;

                this.txtEmail.Text = BorrowerInfo.Rows[0]["Email"].ToString();
                this.txtCellPhone.Text = BorrowerInfo.Rows[0]["CellPhone"].ToString();
                this.txtHomePhone.Text = BorrowerInfo.Rows[0]["HomePhone"].ToString();
                this.txtWorkPhone.Text = BorrowerInfo.Rows[0]["BusinessPhone"].ToString();
                this.txtBirthday.Text = BorrowerInfo.Rows[0]["DOB"].ToString() == "" ? "" : Convert.ToDateTime(BorrowerInfo.Rows[0]["DOB"]).ToString("MM/dd/yyyy");

                this.lbFirstName.Text = BorrowerInfo.Rows[0]["FirstName"].ToString();
                this.lbLastName.Text = BorrowerInfo.Rows[0]["LastName"].ToString();
                this.lbEmail.Text = BorrowerInfo.Rows[0]["Email"].ToString();
                this.lbCellPhone.Text = BorrowerInfo.Rows[0]["CellPhone"].ToString();
                this.lbHomePhone.Text = BorrowerInfo.Rows[0]["HomePhone"].ToString();
                this.lbWorkPhone.Text = BorrowerInfo.Rows[0]["BusinessPhone"].ToString();
                this.lbBirthday.Text = BorrowerInfo.Rows[0]["DOB"].ToString() == "" ? "" : Convert.ToDateTime(BorrowerInfo.Rows[0]["DOB"]).ToString("MM/dd/yyyy");
            
            }
            #endregion

            if (BorrowerInfo.Rows.Count > 0)
            {
                string sContactID = BorrowerInfo.Rows[0]["ContactId"].ToString();
                
                
                #region 加载Prospect Info

                string sSqlx0 = "select * from Prospect where ContactId=" + sContactID;
                DataTable ProspectInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx0);

                #region 绑定Prospect信息

                if (ProspectInfo.Rows.Count > 0)
                {
                    // Lead Source
                    if (ProspectInfo.Rows[0]["LeadSource"] != DBNull.Value)
                    {
                        this.ddlLeadSource.SelectedValue = ProspectInfo.Rows[0]["LeadSource"].ToString();
                        
                        this.lbLeadSource.Text = ProspectInfo.Rows[0]["LeadSource"].ToString();
                    }

                    // Referral Source (Prospect.Referral)
                    if (ProspectInfo.Rows[0]["Referral"] != DBNull.Value)
                    {
                        int iReferralID = Convert.ToInt32(ProspectInfo.Rows[0]["Referral"]);
                        Contacts ContactManager = new Contacts();
                        this.txtReferralSource.Text = ContactManager.GetContactName(iReferralID);
                        this.hdnReferralID.Value = iReferralID.ToString();

                        this.lbReferralSource.Text = ContactManager.GetContactName(iReferralID);
                    }
                }

                #endregion

                #endregion
            }
            #endregion

            #region 绑定Lead信息

            if (LoanInfo.Rows[0]["Purpose"] != DBNull.Value)
            {
                this.ddlPurpose.SelectedValue = LoanInfo.Rows[0]["Purpose"].ToString();

                this.lbPurpose.Text = LoanInfo.Rows[0]["Purpose"].ToString();
            }

            if (LoanInfo.Rows[0]["LoanType"] != DBNull.Value)
            {
                this.ddlType.SelectedValue = LoanInfo.Rows[0]["LoanType"].ToString();
                
                this.lbType.Text = LoanInfo.Rows[0]["LoanType"].ToString();
            }

            if (LoanInfo.Rows[0]["Program"] != DBNull.Value)
            {
                this.ddlProgram.SelectedValue = LoanInfo.Rows[0]["Program"].ToString();

                this.lbProgram.Text = LoanInfo.Rows[0]["Program"].ToString();
            }

            this.txtAmount.Text = LoanInfo.Rows[0]["LoanAmount"].ToString() == "" ? "" : Convert.ToDecimal(LoanInfo.Rows[0]["LoanAmount"]).ToString("n0");
            this.txtRate.Text = LoanInfo.Rows[0]["Rate"].ToString() == "" ? "" : Convert.ToDecimal(LoanInfo.Rows[0]["Rate"]).ToString("n3");

            this.lbAmount.Text = LoanInfo.Rows[0]["LoanAmount"].ToString() == "" ? "" : Convert.ToDecimal(LoanInfo.Rows[0]["LoanAmount"]).ToString("n0");
            this.lbRate.Text = LoanInfo.Rows[0]["Rate"].ToString() == "" ? "" : Convert.ToDecimal(LoanInfo.Rows[0]["Rate"]).ToString("n3") + "%";

            #endregion
        }

        #region Codes for Stage Progress Bar

        DataTable StageProgressData = this.GetStageProgressData(this.iLoanID);
        this.rptStageProgressItems.DataSource = StageProgressData;
        this.rptStageProgressItems.DataBind();

        #endregion

        #region 获取当前Point Folder的BranchID

        string sSqlx = "select b.BranchId,b.name from PointFiles as a inner join PointFolders as b on a.FolderId = b.FolderId where a.FileId=" + this.iLoanID;
        DataTable BranchIDInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx);
        if (BranchIDInfo.Rows.Count > 0 && BranchIDInfo.Rows[0]["BranchId"].ToString() != "" && BranchIDInfo.Rows[0]["BranchId"].ToString() != "0")
        {
            this.hdnBranchID.Value = BranchIDInfo.Rows[0]["BranchId"].ToString();
        }
        else
        {
            sSqlx = "select BranchID from Loans where FileId=" + this.iLoanID;
            DataTable dtLoan = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx);
            if (dtLoan.Rows.Count > 0 && dtLoan.Rows[0]["BranchID"].ToString() != "" && dtLoan.Rows[0]["BranchID"].ToString() != "0")
            {
                this.hdnBranchID.Value = dtLoan.Rows[0]["BranchID"].ToString();
            }
            else
            {
                sSqlx = "SELECT TOP 1 BranchID FROM Groups WHERE GroupId IN(SELECT GroupID FROM GroupUsers WHERE UserID IN(SELECT UserId FROM LoanTeam WHERE FileId = " + this.iLoanID + " AND RoleId =(SELECT RoleId FROM Roles WHERE [Name]='Loan Officer')))";
                DataTable dtGroup = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx);
                if (dtGroup.Rows.Count > 0 && dtGroup.Rows[0]["BranchID"].ToString() != "" && dtGroup.Rows[0]["BranchID"].ToString() != "0")
                {
                    this.hdnBranchID.Value = dtGroup.Rows[0]["BranchID"].ToString();
                }
            }
        }

        #endregion

        #region 加载Point File Info

        string sSql7 = "Select top 1 * from dbo.lpvw_GetPointFileInfo where FileId=" + this.iLoanID;
        DataTable PointFileInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql7);

        string sHasPointFile = "False";
        if (PointFileInfo.Rows.Count > 0)
        {
            string sPointFilePath = PointFileInfo.Rows[0]["Path"].ToString();
            string sFolderID = PointFileInfo.Rows[0]["FolderId"].ToString();

            if (sPointFilePath != string.Empty || sFolderID != string.Empty)
            {
                sHasPointFile = "True";
            }
        }

        this.hdnHasPointFile.Value = sHasPointFile;

        #endregion

        #region Context Menu

        if (LoanInfo.Rows.Count > 0)
        {
            string sProspectLoanStatus = LoanInfo.Rows[0]["ProspectLoanStatus"].ToString();

            //加载 ArchiveLeadStatus
            
            ArchiveLeadStatus ArchiveLeadStatusMgr = new ArchiveLeadStatus();
            DataSet ArchiveLeadStatusList = ArchiveLeadStatusMgr.GetList("isnull([Enabled],0)=1 order by LeadStatusName");

            #region Build Sub Menu Item

            StringBuilder sbSubMenuItems = new StringBuilder();
            if (ArchiveLeadStatusList.Tables[0].Rows.Count > 0)
            {
                sbSubMenuItems.AppendLine("<ul>");
                foreach (DataRow dr in ArchiveLeadStatusList.Tables[0].Rows)
                {
                    string sLeadStatusName1 = dr["LeadStatusName"].ToString();
                    string sLeadStatusName2 = dr["LeadStatusName"].ToString().Replace("'", "\\'");
                    sbSubMenuItems.AppendLine("<li><a href=\"\" onclick=\"ChangeStatus('" + sLeadStatusName2 + "'); return false;\">" + sLeadStatusName1 + "</a></li>");
                }
                sbSubMenuItems.AppendLine("</ul>");
            }

            #endregion

            StringBuilder sbMenuItems = new StringBuilder();
            if (sProspectLoanStatus.ToLower() == "active")
            {
                sbMenuItems.AppendLine("<li><a href=\"\" onclick=\"return false;\">Archive</a>" + sbSubMenuItems + "</li>");
                sbMenuItems.AppendLine("<li><a href=\"\" onclick=\"ChangeStatus('Convert'); return false;\">Convert to Loan</a></li>");
                
                
            }

            this.ltrChangeStatusMenuItems.Text = sbMenuItems.ToString();
        }

        #endregion

        #region Actions

        LoanTasks LoanTasksMgr = new LoanTasks();

        DataTable LastTaskInfo = LoanTasksMgr.GetLoanTaskList(1, " and FileId=" + this.iLoanID + " and Completed is not null", " order by Completed desc");
        DataTable NextTaskInfo = LoanTasksMgr.GetLoanTaskList(1, " and FileId=" + this.iLoanID + " and Completed is null", " order by LoanTaskId asc");

        if (LastTaskInfo.Rows.Count == 0 && NextTaskInfo.Rows.Count == 0)
        {
            this.divActions.Visible = false;
        }
        else
        {
            if (LastTaskInfo.Rows.Count == 0)
            {
                this.trLastAction.Visible = false;
            }
            else
            {
                this.ltrLastTaskName.Text = LastTaskInfo.Rows[0]["Name"].ToString();
                this.ltrLastDueDate.Text = LastTaskInfo.Rows[0]["Due"] == DBNull.Value ? string.Empty : Convert.ToDateTime(LastTaskInfo.Rows[0]["Due"].ToString()).ToLongDateString();
                this.ltrLastDueTime.Text = LastTaskInfo.Rows[0]["DueTime"] == DBNull.Value ? string.Empty : TimeSpan.Parse(LastTaskInfo.Rows[0]["DueTime"].ToString()).ToString();
            }

        
            if (NextTaskInfo.Rows.Count == 0)
            {
                this.trNextAction.Visible = false;
            }
            else
            {
                this.ltrNextTaskName.Text = NextTaskInfo.Rows[0]["Name"].ToString();
                this.ltrNextDueDate.Text = NextTaskInfo.Rows[0]["Due"] == DBNull.Value ? string.Empty : Convert.ToDateTime(NextTaskInfo.Rows[0]["Due"].ToString()).ToLongDateString();
                this.ltrNextDueTime.Text = NextTaskInfo.Rows[0]["DueTime"] == DBNull.Value ? string.Empty : TimeSpan.Parse(NextTaskInfo.Rows[0]["DueTime"].ToString()).ToString();
            
                // hidden fields
                this.hdnNextTaskID.Value = NextTaskInfo.Rows[0]["LoanTaskId"].ToString();
                this.hdnNextTaskOwnerID.Value = NextTaskInfo.Rows[0]["Owner"].ToString();
                this.hdnLoginUserID.Value = this.CurrUser.iUserID.ToString();
                this.hdnCompleteOtherTask.Value = this.CurrUser.userRole.MarkOtherTaskCompl == true ? "True" : "False";
                this.hdnLoanStatus.Value = LoanInfo.Rows[0]["Status"].ToString();
            }
        }

        #endregion
    }

    protected bool UploadLoanInfo(int iFileID, bool bCreateFile, int iUserID, out string sError)
    {
        #region API UploadLoanInfo

        sError = string.Empty;

        LPWeb.LP_Service.UpdateLoanInfoResponse response = null;
        try
        {
            ServiceManager sm = new ServiceManager();
            using (LPWeb.LP_Service.LP2ServiceClient client = sm.StartServiceClient())
            {
                #region UpdateLoanInfoRequest

                UpdateLoanInfoRequest req = new UpdateLoanInfoRequest();
                req.hdr = new ReqHdr();
                req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                req.hdr.UserId = iUserID;
                req.FileId = iFileID;
                req.CreateFile = bCreateFile;
                #endregion

                response = client.UpdateLoanInfo(req);
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            sError = "Failed to invoke API UploadLoanInfo: Workflow Manager is not running.";
            return false;
        }
        catch (Exception ex)
        {
            sError = "Exception happened when invoke API UploadLoanInfo: " + ex.Message;
            return false;
        }

        if (response.hdr.Successful == false)
        {
            sError = response.hdr.StatusInfo.Replace("\"", "'");
            return false;
        }

        #endregion

        return true;
    }

    private LPWeb.Model.PointFiles GetPointFileInfo(int iFileId) 
    {
        PointFiles PointFilesMgr = new PointFiles();
        LPWeb.Model.PointFiles PointFileInfo = PointFilesMgr.GetModel(this.iLoanID);

        return PointFileInfo;
    }

    protected void btnSave_Click(object sender, EventArgs e) 
    {
        #region get user input

        string sFirstName = this.txtFirstName.Text.Trim();
        string sLastName = this.txtLastName.Text.Trim();
        string sEmail = this.txtEmail.Text.Trim();
        string sCellPhone = this.txtCellPhone.Text.Trim();
        string sHomePhone = this.txtHomePhone.Text.Trim();
        string sWorkPhone = this.txtWorkPhone.Text.Trim();
        string sDOB = this.txtBirthday.Text.Trim();
        DateTime DOB;
        DateTime DT = DateTime.Today;
        DT = DateTime.Today.AddYears(-18);

        if (sDOB != string.Empty)
        {
            if (DateTime.TryParse(sDOB, out DOB) == false)
            {
                PageCommon.AlertMsg(this, "Date of Birth is not a valid date format.");
                return;
            }
            if (DOB > DT)
            {
                PageCommon.AlertMsg(this, "Date of Birth should be 18 years old.");
                return;
            }
        }

        string sLeadSource = string.Empty;
        if (this.ddlLeadSource.SelectedIndex != 0)
        {
            sLeadSource = this.ddlLeadSource.SelectedValue;
        }

        string sReferralID = this.hdnReferralID.Value;
        string sPurpose = this.ddlPurpose.SelectedValue;
        string sLoanType = this.ddlType.SelectedValue;
        string sProgram = string.Empty;
        if (this.ddlProgram.SelectedIndex != 0)
        {
            sProgram = this.ddlProgram.SelectedValue;
        }

        string sAmount = this.txtAmount.Text.Trim();
        decimal dAmount = 0;
        if (sAmount != string.Empty)
        {         
            Decimal da = 0;
            if (Decimal.TryParse(sAmount, out da) == false)
            {
                PageCommon.AlertMsg(this, "Amount is not a valid value.");
                return;
            }

            if ( da > 9999999999999 )
            {
                PageCommon.AlertMsg(this, "Amount is not a valid value.");
                return;
            }

            dAmount = da;            
        }
        string sRate = this.txtRate.Text.Trim();
        decimal dRate = 0;
        if (sRate != string.Empty)
        {
            dRate = Convert.ToDecimal(sRate);
        }
        
        #endregion

        #region update Loans

        Loans LoansMgr = new Loans();
        LoansMgr.UpldateLoanInfo(this.iLoanID, sPurpose, sLoanType, sProgram, sAmount, sRate);

        #endregion

        #region update Contacts and Prospect

        // get borrower info
        DataTable BorrowerInfo = this.GetBorrowerInfo(this.iLoanID);
        if (BorrowerInfo.Rows.Count > 0)
        {
            int iBorrowerID = Convert.ToInt32(BorrowerInfo.Rows[0]["ContactId"]);

            Contacts ContactsMgr = new Contacts();
            ContactsMgr.UpdateContactProspectInfo(iBorrowerID, sFirstName, sLastName, sEmail, sCellPhone, sHomePhone, sWorkPhone, sDOB, sLeadSource, sReferralID);
        }

        #endregion

        #region WCF API UploadLoanInfo

        // 加载PointFiles信息
        LPWeb.Model.PointFiles PointFileInfo = this.GetPointFileInfo(this.iLoanID);
        if (PointFileInfo != null && !string.IsNullOrEmpty(PointFileInfo.Name))
        {
            string sName = PointFileInfo.Name;
            string sFolderId = PointFileInfo.FolderId.ToString();

            if (sName != string.Empty && sFolderId != string.Empty)
            {
                string sError = string.Empty;
                bool bResult = this.UploadLoanInfo(this.iLoanID, true, this.CurrUser.iUserID, out sError);

                if (bResult == false)
                {
                    PageCommon.WriteJsEnd(this, "Saved successfully, but failed to invoke API UploadLoanInfo: " + sError, PageCommon.Js_RefreshSelf);
                    return;
                }
            }
        }

        #endregion

        // success
        PageCommon.WriteJsEnd(this, "Saved successfully.", PageCommon.Js_RefreshSelf);
    }

    protected void lnkImport_Click(object sender, EventArgs e)
    {
        // 加载PointFiles信息
        LPWeb.Model.PointFiles PointFileInfo = this.GetPointFileInfo(this.iLoanID);
        if (PointFileInfo== null || string.IsNullOrEmpty(PointFileInfo.Name) || PointFileInfo.FolderId <= 0)
        {
            PageCommon.WriteJsEnd(this, "Unable to sync with Point, missing Point filename or Point Folder.", PageCommon.Js_RefreshSelf);
            return;
        }

        string sName = PointFileInfo.Name;
        string sFolderId = PointFileInfo.FolderId.ToString();

        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            ImportLoansRequest req = new ImportLoansRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = this.CurrUser.iUserID;
            req.FileIds = new int[1] { this.iLoanID };
            ImportLoansResponse respone = null;
            try
            {
                respone = service.ImportLoans(req);

                if (respone.hdr.Successful)
                {
                    PageCommon.WriteJsEnd(this, "Synched with Point successfully.", PageCommon.Js_RefreshSelf);
                }
                else
                {
                    PageCommon.WriteJsEnd(this, "Failed to sync with Point, reason:" + respone.hdr.StatusInfo, PageCommon.Js_RefreshSelf);
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
                string sExMsg = string.Format("Failed to sync with Point, error: {0}", ex.Message);
                LPLog.LogMessage(LogType.Logerror, sExMsg);
                PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
            }
        }
    }

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string sError = string.Empty;
        bool bResult = this.UploadLoanInfo(this.iLoanID, true, this.CurrUser.iUserID, out sError);

        if (bResult == false)
        {
            PageCommon.WriteJsEnd(this, "Failed to invoke API UploadLoanInfo: " + sError, PageCommon.Js_RefreshSelf);
            return;
        }

        PageCommon.WriteJsEnd(this, "Exported successfully." + sError, PageCommon.Js_RefreshSelf);
    }

    protected void btnvCardExport_Click(object sender, EventArgs e)
    {
        // 加载BorrowerInfo
        DataTable BorrowerInfo = this.GetBorrowerInfo(this.iLoanID);
        if (BorrowerInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "There is no borrower for this lead.", PageCommon.Js_RefreshSelf);
        }
        int iContactID = Convert.ToInt32(BorrowerInfo.Rows[0]["ContactId"]);
             
        string sCurrentPagePath = this.Server.MapPath("~/");
        string sFileName = Guid.NewGuid().ToString();
        string sFilePath = Path.Combine(Path.GetDirectoryName(sCurrentPagePath), sFileName);

        #region #region Call vCardToString(ContactId, true) API

        LPWeb.BLL.Contacts x = new LPWeb.BLL.Contacts();
        string sContactStr = x.vCardToString(iContactID, true);

        #endregion

        // save file
        if (File.Exists(sFilePath) == false)
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(sFilePath))
            {
                sw.Write(sContactStr);
            }
        }

        FileInfo FileInfo1 = new FileInfo(sFilePath);
        this.Response.Clear();
        this.Response.ClearHeaders();
        this.Response.Buffer = false;
        this.Response.ContentType = "application/octet-stream";
        this.Response.AppendHeader("Content-Disposition", "attachment;filename=vcard.vcf");
        this.Response.AppendHeader("Content-Length", FileInfo1.Length.ToString());
        this.Response.WriteFile(sFilePath);
        this.Response.Flush();

        // 删除临时文件
        File.Delete(sFilePath);

        this.Response.End();
    }

    protected void btniCalendarExport_Click(object sender, EventArgs e)
    {
        string sNextTaskID = this.hdnNextTaskID.Value;
        if (sNextTaskID == string.Empty)
        {
            PageCommon.WriteJsEnd(this, "This is no next action to be exported.", PageCommon.Js_RefreshSelf);
            return;
        }

        int iNextTaskID = Convert.ToInt32(sNextTaskID);

        string sCurrentPagePath = this.Server.MapPath("~/");
        string sFileName = Guid.NewGuid().ToString() + ".ics";       
        string sFilePath = Path.Combine(Path.GetDirectoryName(sCurrentPagePath), sFileName);

        #region #region Call iCalendarToString() API

        LPWeb.BLL.LoanTasks x = new LPWeb.BLL.LoanTasks();
        string s = x.iCalendarToString(this.iLoanID, iNextTaskID, this.CurrUser.iUserID);

        #endregion

        try
        {
            using (StreamWriter sw = File.CreateText(sFilePath))
            {
                sw.Write(s);
            }

            FileInfo FileInfo1 = new FileInfo(sFilePath);
            this.Response.Clear();
            this.Response.ClearHeaders();
            this.Response.Buffer = false;
            this.Response.ContentType = "application/octet-stream";
            this.Response.AppendHeader("Content-Disposition", "attachment;filename=Lock.ics");
            this.Response.AppendHeader("Content-Length", FileInfo1.Length.ToString());
            this.Response.WriteFile(sFilePath);
            this.Response.Flush();     
            this.Response.End();
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("iCalendarExport, error: {0}", ex.Message);                
            PageCommon.WriteJsEnd(this, sExMsg, PageCommon.Js_RefreshSelf);
        }
    }

    #region Methods for Stage Progress Bar

    /// <summary>
    /// get stage progress data
    /// neo 2011-02-26
    /// </summary>
    /// <param name="iLoanID"></param>
    /// <returns></returns>
    private DataTable GetStageProgressData(int iLoanID)
    {
        string sSql = "select dbo.lpfn_GetStageProgressImageFileName(LoanStageId) as StageImage, dbo.lpfn_GetStageAlias(LoanStageId) as StageAlias, * from LoanStages where FileId=" + iLoanID + " order by SequenceNumber";
        DataTable StageProgressData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        return StageProgressData;
    }

    /// <summary>
    /// get stage tool tip
    /// neo 2011-02-26
    /// </summary>
    /// <param name="sStageAlias"></param>
    /// <param name="sCompleteDate"></param>
    /// <returns></returns>
    public string GetStageToolTip(string sStageAlias, string sCompleteDate)
    {
        if (sCompleteDate == string.Empty)
        {
            return sStageAlias;
        }
        else
        {
            return sStageAlias + " " + sCompleteDate;
        }
    }

    /// <summary>
    /// get span html for stage alias and complete date
    /// neo 2011-02-26
    /// </summary>
    /// <param name="sStageAlias"></param>
    /// <param name="sStageImage"></param>
    /// <param name="sCompleteDate"></param>
    /// <returns></returns>
    public string GetSpan_StageAliasCompleteDate(string sStageAlias, string sStageImage, string sCompleteDate)
    {
        string sSpanText = string.Empty;

        if (sStageImage.Contains("Grass") || sStageImage.Contains("Pink") || sStageImage.Contains("Silver") || sStageImage.Contains("Yellow"))
        {
            sSpanText += "<span style=\"color: #000;\">" + sStageAlias + "</span>";
            if (sCompleteDate != string.Empty)
            {
                sSpanText += "<span style=\"color: #000; padding-top: 2px;\">" + Convert.ToDateTime(sCompleteDate).ToShortDateString() + "</span>";
            }
        }
        else
        {
            sSpanText += "<span>" + sStageAlias + "</span>";
            if (sCompleteDate != string.Empty)
            {
                sSpanText += "<span style=\"padding-top: 2px;\">" + Convert.ToDateTime(sCompleteDate).ToShortDateString() + "</span>";
            }
        }

        return sSpanText;
    }

    #endregion

}
