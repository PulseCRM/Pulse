using System;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;


public partial class QuickLeadForm : BasePage
{
    private bool _isAccessAllMailChimpList = false;
    private int iLoanOfficerID;

    protected void Page_Load(object sender, EventArgs e)
    {


        if (this.IsPostBack == false)
        {
            #region  加载 ddlLeadSource
            Company_Lead_Sources LeadSourceManager = new Company_Lead_Sources();
            DataTable LeadSourceList = LeadSourceManager.GetList("1=1 order by LeadSource").Tables[0];

            DataRow NewLeadSourceRow = LeadSourceList.NewRow();
            NewLeadSourceRow["LeadSourceID"] = 0;
            NewLeadSourceRow["LeadSource"] = "- select -";
            NewLeadSourceRow["Default"] = DBNull.Value;

            LeadSourceList.Rows.InsertAt(NewLeadSourceRow, 0);

            this.ddlLeadSource.DataSource = LeadSourceList;
            this.ddlLeadSource.DataBind();
            // set default selected
            DataRow[] DefaultRowArray = LeadSourceList.Select("Default=1");
            if (DefaultRowArray.Length > 0)
            {
                string sLeadSource = DefaultRowArray[0]["LeadSourceID"].ToString();
                this.ddlLeadSource.SelectedValue = sLeadSource;
            }
            #endregion

            #region 加载 ddlLoanOfficer

            DataTable dtLoadOfficer = this.GetLoanOfficerList(CurrUser.iUserID);

            DataRow drNew = dtLoadOfficer.NewRow();
            //2014/1/16 CR072 Add the current user in the Loan Officer dropdown list
            if (dtLoadOfficer.Select("ID=" + CurrUser.iUserID.ToString()).Length < 1)
            {
                drNew["ID"] = CurrUser.iUserID;
                drNew["Name"] = CurrUser.sFullName;
                drNew["LastName"] = CurrUser.sLastName;
                drNew["FirstName"] = CurrUser.sFirstName;
                dtLoadOfficer.Rows.InsertAt(drNew, 0);
            }
            drNew = dtLoadOfficer.NewRow();
            drNew["ID"] = 0;
            drNew["Name"] = "Lead Routing Engine";
            dtLoadOfficer.Rows.InsertAt(drNew, 0);

            drNew = dtLoadOfficer.NewRow();
            drNew["ID"] = -1;
            drNew["Name"] = "Unassigned";
            dtLoadOfficer.Rows.InsertAt(drNew, 0);

            drNew = dtLoadOfficer.NewRow();
            drNew["ID"] = -2;
            drNew["Name"] = "- select -";
            dtLoadOfficer.Rows.InsertAt(drNew, 0); 

            ddlLoanOfficer.DataSource = dtLoadOfficer;
            ddlLoanOfficer.DataTextField = "Name";
            ddlLoanOfficer.DataValueField = "ID";
            //if (dtLoadOfficer.Select("ID=" + CurrUser.iUserID.ToString()).Length > 0)
            //{
            //    ddlLoanOfficer.SelectedValue = CurrUser.iUserID.ToString();
            //}
            //else
            //{
                ddlLoanOfficer.SelectedValue = "0";
            //}
            ddlLoanOfficer.DataBind(); 
            #endregion

            #region 加载ddlWorkflow

            Template_Workflow WflTempMgr = new Template_Workflow();
            DataTable WorkflowList = WflTempMgr.GetWorkflowTemplateList(" and WorkflowType='Prospect' and Enabled=1", "Name");
            this.ddlWorkflow.DataSource = WorkflowList;
            this.ddlWorkflow.DataBind();

            // set default selected
            DefaultRowArray = WorkflowList.Select("Default=1");
            if (DefaultRowArray.Length > 0)
            {
                string sWflTemplId = DefaultRowArray[0]["WflTemplId"].ToString();

                this.ddlWorkflow.SelectedValue = sWflTemplId;
            }

            #endregion

            #region 加载Marketing enrollment

            MailChimpLists MailChimpListsMgr = new MailChimpLists();

            DataTable MailChimpList = null;

            CheckRolePermistion(this.CurrUser.iUserID);

            if (_isAccessAllMailChimpList == true )
            {
                if (this.CurrUser.bIsCompanyExecutive == true)
                {
                    MailChimpList = MailChimpListsMgr.GetMailChimpList(" and 1=1 ", "Name");
                }
                else
                {
                    if (this.CurrUser.bIsBranchManager == true)
                    {
                        MailChimpList = MailChimpListsMgr.GetMailChimpList_BranchManager(this.CurrUser.iUserID);
                    }
                    else
                    {
                        MailChimpList = MailChimpListsMgr.GetMailChimpList(" and UserId=" + this.CurrUser.iUserID, "Name");
                    }
                }
            }
            else
            {
                MailChimpList = MailChimpListsMgr.GetMailChimpList(" and UserId=" + this.CurrUser.iUserID, "Name");
            }

            DataRow NewMarkingRow = MailChimpList.NewRow();
            NewMarkingRow["LID"] = "";
            NewMarkingRow["Name"] = "-- select --";
            MailChimpList.Rows.InsertAt(NewMarkingRow, 0);

            this.ddlMarketing.DataSource = MailChimpList;
            this.ddlMarketing.DataBind();

            #endregion

            #region 加载ddlTaskList

            LeadTaskList LeadTaskListMgr = new LeadTaskList();

            string sOrderBy = string.Empty;
            if (this.CurrUser.SortTaskPickList == "S")
            {
                sOrderBy = "SequenceNumber";
            }
            else
            {
                sOrderBy = "TaskName";
            }

            DataTable LeadTaskList1 = LeadTaskListMgr.GetLeadTaskList(" and Enabled=1", sOrderBy);

            DataRow NewTaskRow = LeadTaskList1.NewRow();
            NewTaskRow["TaskName"] = "-- select --";
            NewTaskRow["SequenceNumber"] = 0;
            NewTaskRow["Enabled"] = "True";
            LeadTaskList1.Rows.InsertAt(NewTaskRow, 0);

            this.ddlTaskList.DataSource = LeadTaskList1;
            this.ddlTaskList.DataBind();

            #endregion

            // set default value
            this.txtDueDate.Text = DateTime.Now.ToString("MM/dd/yyyy");


            this.txtDueTime.Text = System.DateTime.Now.AddHours(2).ToString("HH:mm");


            this.txtReminderUser.Text = this.CurrUser.sLastName + ", " + this.CurrUser.sFirstName;
            this.hdnReminderUserID.Value = this.CurrUser.iUserID.ToString();
        }
    }

    public DataTable GetLoanOfficerList()
    {
        string sSql0 = string.Empty;
        if (this.CurrUser.sRoleName == "Executive")
        {
            sSql0 = @"select distinct LoanOfficerId as ID, [Loan Officer] as Name from V_ProcessingPipelineInfo 
where isnull(LoanOfficerId,'')<>'' 
and BranchId in (select BranchId from dbo.lpfn_GetUserBranches_Executive(" + this.CurrUser.iUserID + @")) 
order by [Loan Officer]";
        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            sSql0 = @"select distinct LoanOfficerId as ID, [Loan Officer] as Name from V_ProcessingPipelineInfo 
where isnull(LoanOfficerId,'')<>'' 
and BranchId in (select BranchId from dbo.lpfn_GetUserBranches_Branch_Manager(" + this.CurrUser.iUserID + @")) 
order by [Loan Officer]";
        }
        else
        {
            sSql0 = @"select distinct LoanOfficerId as ID, [Loan Officer] as Name from V_ProcessingPipelineInfo 
where isnull(LoanOfficerId,'')<>'' 
and BranchId in (select BranchId from dbo.lpfn_GetUserBranches(" + this.CurrUser.iUserID + @")) 
order by [Loan Officer]";
        }

        DataTable LoanOfficerList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);

        return LoanOfficerList;
    }

    /// <summary>
    /// get loan officer list
    /// neo 2011-04-26
    /// </summary>
    /// <param name="iLoginUserID"></param>
    /// <returns></returns>
    private DataTable GetLoanOfficerList(int iLoginUserID)
    {
        string sSql = "select distinct LastName, FirstName, LastName +', '+FirstName as Name,UserId as ID from dbo.lpfn_GetAllLoanOfficer(" + iLoginUserID + ") order by  LastName, FirstName";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private void CheckRolePermistion(int iUserId)
    {
        try
        {
            var bllRole = new Roles();
            var role = bllRole.GetRoleByUserID(iUserId);
            if (role != null && role.Rows.Count > 0)
            {
                if (role.Rows[0]["AccessAllMailChimpList"] != DBNull.Value)
                {
                    if ((role.Rows[0]["AccessAllMailChimpList"].ToString() == "1") || (role.Rows[0]["AccessAllMailChimpList"].ToString().ToLower() == "true"))
                    {
                        _isAccessAllMailChimpList = true;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
    }

    public string day()
    {
        return System.DateTime.Now.AddHours(2).ToShortTimeString();
    }


    protected int GetFolderID()
    {
        string sSql = "select top 1 * from PointFolders where Enabled=1 and LoanStatus=6 ";
        DataTable FolderInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        if (FolderInfo.Rows.Count == 0)
        {
            return 0;
        }
        else
        {
            return Convert.ToInt32(FolderInfo.Rows[0]["FolderId"]);
        }
    }

    protected int GetTaskStageID(int iFileId)
    {
        //LPWeb.BLL.WorkflowManager wm = new LPWeb.BLL.WorkflowManager();
        int iCurrentLoanStageId = LPWeb.BLL.WorkflowManager.GetCurrentLoanStageId(iFileId);
        if (iCurrentLoanStageId > 0)
            return iCurrentLoanStageId;
        int iDefaultWorkflowTempl = LPWeb.BLL.WorkflowManager.GetDefaultWorkflowTemplate("Prospect");
        if (iDefaultWorkflowTempl > 0)
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                GenerateWorkflowRequest req = new GenerateWorkflowRequest();
                req.hdr = new ReqHdr();
                req.hdr.SecurityToken = string.Empty;
                req.hdr.UserId = CurrUser.iUserID;
                req.FileId = iFileId;
                req.WorkflowTemplId = iDefaultWorkflowTempl;
                GenerateWorkflowResponse resp = service.GenerateWorkflow(req);
                if ((resp == null) || (!resp.hdr.Successful))
                {
                    return iCurrentLoanStageId;
                }
                iCurrentLoanStageId = LPWeb.BLL.WorkflowManager.GetCurrentLoanStageId(iFileId);
                return iCurrentLoanStageId;
            }
            return iCurrentLoanStageId;
        }
        iCurrentLoanStageId = LPWeb.BLL.WorkflowManager.GenerateDefaultLoanStages(iFileId, "Prospect");
        return iCurrentLoanStageId;
    }

    protected int CreateLoanTask(int iFileID, string sTaskName, int TaskOwnerId, string sDueDate, string sDueTime, int iLoanStageID)
    {
        #region create new task

        LoanTasks LoanTaskManager = new LoanTasks();

        // add task
        LPWeb.Model.LoanTasks taskModel = new LPWeb.Model.LoanTasks();
        taskModel.LoanTaskId = 0;
        taskModel.FileId = iFileID;
        taskModel.Name = sTaskName;
        if (sDueDate == string.Empty)
        {
            taskModel.Due = null;
        }
        else
        {
            taskModel.Due = DateTime.Parse(sDueDate);
        }

        DateTime DTN = DateTime.Now;
        string sDueTime_Span = null;
        TimeSpan DueTime = new TimeSpan();

        if (sDueTime == string.Empty)
        {
            taskModel.DueTime = null;
        }
        else
        {
            taskModel.DueTime = null;
            if (DateTime.TryParse(sDueTime, out DTN) == true)
            {
                sDueTime_Span = DTN.ToString("HH:mm");
                if (TimeSpan.TryParse(sDueTime_Span, out DueTime) == true)
                {
                    taskModel.DueTime = DueTime;
                }
            }
        }

        taskModel.LoanStageId = iLoanStageID;

        taskModel.OldLoanStageId = 0;
        taskModel.Owner = TaskOwnerId;
        taskModel.ModifiedBy = CurrUser.iUserID;
        taskModel.LastModified = DateTime.Now;
        taskModel.DaysDueFromEstClose = 0;
        taskModel.DaysFromCreation = 0;
        taskModel.PrerequisiteTaskId = 0;
        taskModel.DaysDueAfterPrerequisite = 0;
        taskModel.CompletionEmailId = 0;
        taskModel.WarningEmailId = 0;
        taskModel.OverdueEmailId = 0;
        taskModel.SequenceNumber = 1;
        taskModel.ExternalViewing = false;

        int iLoanTaskId = LPWeb.BLL.WorkflowManager.AddTask_Lead(taskModel);


        #endregion

        return iLoanTaskId;
    }

    protected int CreateLoan(int iContactId, decimal LoanAmount, string Purpose)
    {
        int iFolderId = 0;

        LPWeb.Model.LoanDetails LoanDetailsModel = new LPWeb.Model.LoanDetails();

        LoanDetailsModel.FileId = 0;
        LoanDetailsModel.FolderId = iFolderId;
        LoanDetailsModel.Status = "Prospect";
        LoanDetailsModel.ProspectLoanStatus = "Active";

        LoanDetailsModel.BoID = iContactId;
        LoanDetailsModel.CoBoID = 0;

        LoanDetailsModel.Created = DateTime.Now;
        LoanDetailsModel.CreatedBy = CurrUser.iUserID;
        LoanDetailsModel.Modifed = DateTime.Now;
        LoanDetailsModel.ModifiedBy = CurrUser.iUserID;

        LoanDetailsModel.LoanAmount = LoanAmount;
        LoanDetailsModel.EstCloseDate = null;
        LoanDetailsModel.Ranking = string.Empty;
        LoanDetailsModel.UserId = CurrUser.iUserID;

        string sLoanOfficerID = ddlLoanOfficer.SelectedValue;
        iLoanOfficerID = 0;
        if (sLoanOfficerID == "-1" || sLoanOfficerID == "-2")
        {
            //Nobody
        }
        else if (sLoanOfficerID == "0")
        {
            //Lead Routing Engine
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    //invoke the WCF API GetNextLoanOfficer
                    LR_GetNextLoanOfficerReq req = new LR_GetNextLoanOfficerReq();
                    req.LeadSource = ddlLeadSource.SelectedItem.Text;
                    req.Purpose = Purpose;
                    req.ReqHdr = new ReqHdr();
                    req.ReqHdr.UserId = CurrUser.iUserID;
                    req.ReqHdr.SecurityToken = "SecurityToken";
                    LR_GetNextLoanOfficerResp response = client.LeadRouting_GetNextLoanofficer(req);
                    if (response.RespHdr.Successful)
                    {
                        iLoanOfficerID = response.NextLoanOfficerID;
                    }
                    else
                    {
                        Company_Lead_Sources LeadSourceMgr = new Company_Lead_Sources();
                        DataTable SelLeadSourceList = LeadSourceMgr.GetList("LeadSourceID='" + ddlLeadSource.SelectedValue + "'").Tables[0];
                        if (SelLeadSourceList != null && SelLeadSourceList.Rows.Count > 0)
                        {
                            iLoanOfficerID = SelLeadSourceList.Rows[0]["DefaultUserId"] == DBNull.Value ? 0 : Convert.ToInt32(SelLeadSourceList.Rows[0]["DefaultUserId"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                 Company_Lead_Sources LeadSourceMgr = new Company_Lead_Sources();
                 DataTable SelLeadSourceList = LeadSourceMgr.GetList("LeadSourceID='" + ddlLeadSource.SelectedValue + "'").Tables[0];
                 if (SelLeadSourceList!=null && SelLeadSourceList.Rows.Count > 0)
                 {
                     iLoanOfficerID = SelLeadSourceList.Rows[0]["DefaultUserId"] == DBNull.Value ? 0 : Convert.ToInt32(SelLeadSourceList.Rows[0]["DefaultUserId"]);
                 }
            }
        }
        else
        {
            iLoanOfficerID = Convert.ToInt32(sLoanOfficerID);
        }

        LoanDetailsModel.LoanOfficerId = iLoanOfficerID;
        LoanDetailsModel.Rate = null;
        LoanDetailsModel.Program = string.Empty;
        LoanDetailsModel.Purpose = Purpose;
        LoanDetailsModel.Lien = string.Empty;

        LoanDetailsModel.PropertyAddr = string.Empty;
        LoanDetailsModel.PropertyCity = string.Empty;
        LoanDetailsModel.PropertyState = string.Empty;
        LoanDetailsModel.PropertyZip = string.Empty;

        LoanDetailsModel.FileName = string.Empty;
        LoanDetailsModel.PropertyType = string.Empty;
        LoanDetailsModel.HousingStatus = string.Empty;
        LoanDetailsModel.IncludeEscrows = false;
        LoanDetailsModel.InterestOnly = false;
        LoanDetailsModel.RentAmount = 0;
        LoanDetailsModel.CoborrowerType = string.Empty;

        Loans LoansMgr = new Loans();

        int iFileID = LoansMgr.LoanDetailSaveFileId(LoanDetailsModel);

        return iFileID;

    }

    protected int CreateContactAndProspect(string sFirstName, string sLastName, string sEmail, string sType, string sPhone, string sDOB, string sSSN, int iUserID, string Purpose)
    {
        #region create new contact

        LPWeb.Model.Contacts ContactsModel = new LPWeb.Model.Contacts();
        ContactsModel.ContactId = 0;
        ContactsModel.FirstName = sFirstName;
        ContactsModel.LastName = sLastName;
        ContactsModel.Email = sEmail;

        if (sType == "Cell Phone")
        {
            ContactsModel.CellPhone = sPhone;
            ContactsModel.HomePhone = string.Empty;
            ContactsModel.BusinessPhone = string.Empty;
        }
        else if (sType == "Home Phone")
        {
            ContactsModel.CellPhone = string.Empty;
            ContactsModel.HomePhone = sPhone;
            ContactsModel.BusinessPhone = string.Empty;
        }
        else if (sType == "Work Phone")
        {
            ContactsModel.CellPhone = string.Empty;
            ContactsModel.HomePhone = string.Empty;
            ContactsModel.BusinessPhone = sPhone;
        }

        if (sDOB == string.Empty)
        {
            ContactsModel.DOB = null;
        }
        else
        {
            ContactsModel.DOB = Convert.ToDateTime(sDOB);
        }

        ContactsModel.SSN = sSSN;

        ContactsModel.MiddleName = string.Empty;
        ContactsModel.NickName = txtFirstName.Text.Trim();
        ContactsModel.Title = string.Empty;
        ContactsModel.GenerationCode = string.Empty;
        ContactsModel.Fax = string.Empty;
        ContactsModel.MailingAddr = string.Empty;
        ContactsModel.MailingCity = string.Empty;
        ContactsModel.MailingState = string.Empty;
        ContactsModel.MailingZip = string.Empty;

        #endregion

        #region create new prospect

        LPWeb.Model.Prospect ProspectModel = new LPWeb.Model.Prospect();

        ProspectModel.PreferredContact = sType;

        ProspectModel.Contactid = 0;
        ProspectModel.Created = DateTime.Now;
        ProspectModel.CreatedBy = iUserID;
        ProspectModel.LeadSource = ddlLeadSource.SelectedValue.ToString() == "0" ? "" : this.ddlLeadSource.SelectedItem.Text;

        string sLoanOfficerID = ddlLoanOfficer.SelectedValue;
        iLoanOfficerID = 0;
        if (sLoanOfficerID == "-1" || sLoanOfficerID == "-2")
        {
            //Nobody
        }
        else if (sLoanOfficerID == "0")
        {
            //Lead Routing Engine
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    //invoke the WCF API GetNextLoanOfficer
                    LR_GetNextLoanOfficerReq req = new LR_GetNextLoanOfficerReq();
                    req.LeadSource = ddlLeadSource.SelectedItem.Text;
                    req.Purpose = Purpose;
                    req.ReqHdr = new ReqHdr();
                    req.ReqHdr.UserId = CurrUser.iUserID;
                    req.ReqHdr.SecurityToken = "SecurityToken";
                    LR_GetNextLoanOfficerResp response = client.LeadRouting_GetNextLoanofficer(req);
                    if (response.RespHdr.Successful)
                    {
                        iLoanOfficerID = response.NextLoanOfficerID;
                    }
                    else
                    {
                        Company_Lead_Sources LeadSourceMgr = new Company_Lead_Sources();
                        DataTable SelLeadSourceList = LeadSourceMgr.GetList("LeadSourceID='" + ddlLeadSource.SelectedValue + "'").Tables[0];
                        if (SelLeadSourceList != null && SelLeadSourceList.Rows.Count > 0)
                        {
                            iLoanOfficerID = SelLeadSourceList.Rows[0]["DefaultUserId"] == DBNull.Value ? 0 : Convert.ToInt32(SelLeadSourceList.Rows[0]["DefaultUserId"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Company_Lead_Sources LeadSourceMgr = new Company_Lead_Sources();
                DataTable SelLeadSourceList = LeadSourceMgr.GetList("LeadSourceID='" + ddlLeadSource.SelectedValue + "'").Tables[0];
                if (SelLeadSourceList != null && SelLeadSourceList.Rows.Count > 0)
                {
                    iLoanOfficerID = SelLeadSourceList.Rows[0]["DefaultUserId"] == DBNull.Value ? 0 : Convert.ToInt32(SelLeadSourceList.Rows[0]["DefaultUserId"]);
                }
            }
        }
        else
        {
            iLoanOfficerID = Convert.ToInt32(sLoanOfficerID);
        }


        ProspectModel.Loanofficer = iLoanOfficerID;
        ProspectModel.ReferenceCode = string.Empty;
        ProspectModel.Status = "Active";
        ProspectModel.CreditRanking = string.Empty;
        ProspectModel.Referral = null;

        Prospect ProspectManager = new Prospect();

        int iContactId = ProspectManager.CreateContactAndProspect(ContactsModel, ProspectModel);


        #endregion

        return iContactId;
    }

    protected bool GenerateWorkflow(int iFileID, int iWflTempID, int iUserID, out string sError)
    {
        #region API GenerateWorkflow

        sError = string.Empty;

        var request = new GenerateWorkflowRequest
        {
            FileId = iFileID,
            WorkflowTemplId = iWflTempID,
            hdr = new ReqHdr { UserId = iUserID }
        };

        GenerateWorkflowResponse response = null;

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient client = sm.StartServiceClient())
            {
                response = client.GenerateWorkflow(request);
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("Failed to apply workflow template: {0}", "Workflow Manager is not running.", iWflTempID);
            LPLog.LogMessage(LogType.Logerror, sExMsg);

            sError = sExMsg;
            return false;
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Failed to apply workflow template: {0}, error detail:", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);

            sError = sExMsg;
            return false;
        }

        if (response.hdr.Successful == false)
        {
            sError = "Failed to apply workflow template: " + response.hdr.StatusInfo;
            return false;
        }

        #endregion

        return true;
    }

    protected void ApplyLoanWflTempl(int iFileID, int iWflTempID, int iUserID)
    {
        #region LoanWflTempl

        LPWeb.Model.LoanWflTempl WflTempModel = new LPWeb.Model.LoanWflTempl();
        WflTempModel.FileId = iFileID;
        WflTempModel.WflTemplId = iWflTempID;
        WflTempModel.ApplyBy = iUserID;
        WflTempModel.ApplyDate = DateTime.Now;

        LPWeb.BLL.LoanWflTempl LoanWflTemplMgr = new LoanWflTempl();

        LoanWflTemplMgr.Apply(WflTempModel);

        #endregion
    }

    protected bool MailChimp_Subscribe(int iContactId, string sLID, out string sError)
    {
        #region API MailChimp_Subscribe

        sError = string.Empty;

        LPWeb.LP_Service.MailChimp_Response response = null;
        try
        {
            ServiceManager sm = new ServiceManager();
            using (LPWeb.LP_Service.LP2ServiceClient client = sm.StartServiceClient())
            {
                int[] ContactIds = new int[1];
                ContactIds.SetValue(iContactId, 0);

                response = client.MailChimp_SubscribeBatch(ContactIds, sLID);
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            sError = "Failed to invoke API MailChimp_SubscribeBatch: Workflow Manager is not running.";
            return false;
        }
        catch (Exception ex)
        {
            sError = "Exception happened when invoke API MailChimp_SubscribeBatch: " + ex.Message;
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Save())
        {
            // success
            PageCommon.WriteJsEnd(this, "Saved successfully.", PageCommon.Js_RefreshSelf);
        }
    }

    protected void btnSaveAndGoToLeadDetail_Click(object sender, EventArgs e)
    {
        if (Save())
        {
            // success
            //PageCommon.WriteJsEnd(this, "Saved successfully.", PageCommon.Js_RefreshSelf);
            //Response.Redirect("Prospect/ProspectLoanDetails.aspx?FileID=" + iFileID.ToString());
            PageCommon.WriteJsEnd(this, "", string.Format("window.parent.window.location.href='Prospect/ProspectLoanDetails.aspx?FromPage=http%3A%2F%2Flocalhost%2F_layouts%2FLPWeb%2FPipeline%2FProspectPipelineSummaryLoan.aspx&FileID={0}&FileIDs={0}';", iFileID));
            
        }
    }

    int iFileID = 0;
    private bool Save()
    {
        #region get user input

        string sFirstName = this.txtFirstName.Text.Trim();
        string sLastName = this.txtLastName.Text.Trim();
        string sEmail = this.txtEmail.Text.Trim();
        string sPhone = this.txtPhone.Text.Trim();
        string sType = this.ddlType.SelectedValue;

        string sDOB = this.txtBirthday.Text.Trim();
        if (!string.IsNullOrEmpty(sDOB))
        {
            DateTime DOB;
            if (DateTime.TryParse(sDOB, out DOB) == false)
            {
                //PageCommon.AlertMsg(this, "Date of Birth is not a invalid date format.");
                PageCommon.WriteJsEnd(this, "Date of Birth is not a invalid date format.", PageCommon.Js_RefreshSelf);
                return false;
            }

        }
        string sSSN = this.txtSSN.Text.Trim();
        string sWorkflow = this.ddlWorkflow.SelectedValue;
        int iWflTempID = 0;
        int.TryParse(sWorkflow, out iWflTempID);
        string sMarketing = this.ddlMarketing.SelectedValue;    // MailChimpList ID

        // Task Name
        string sTaskName = string.Empty;
        if (this.radTaskList.Checked == true)
        {
            if (this.ddlTaskList.SelectedValue == "-- select --")
            {
                sTaskName = string.Empty;
            }
            else
            {
                sTaskName = this.ddlTaskList.SelectedValue;
            }
        }
        else
        {
            sTaskName = this.txtTaskName.Text.Trim();
        }

        // Due Date
        string sDueDate = this.txtDueDate.Text.Trim();
        DateTime DueDate;
        if (sDueDate != string.Empty)
        {
            if (DateTime.TryParse(sDueDate, out DueDate) == false)
            {
                //PageCommon.AlertMsg(this, "Due Date is not a invalid date format.");
                PageCommon.WriteJsEnd(this, "Due Date is not a invalid date format.", PageCommon.Js_RefreshSelf);
                return false;
            }

            DueDate = DateTime.Parse(sDueDate);
        }

        // Due Time
        string sDueTime = ddlDueTime_hour.Text + ":" + ddlDueTime_min.Text; //this.txtDueTime.Text.Trim();
        DateTime DTN = DateTime.Now;
        string sDueTime_Span = null;
        TimeSpan DueTime = new TimeSpan();

        if (sDueTime != string.Empty)
        {
            if (DateTime.TryParse(sDueTime, out DTN) == true)
            {
                sDueTime_Span = DTN.ToString("HH:mm");
                if (TimeSpan.TryParse(sDueTime_Span, out DueTime) == false)
                {
                    //PageCommon.AlertMsg(this, "Invalid Due Time format.");
                    PageCommon.WriteJsEnd(this, "Invalid Due Time format.", PageCommon.Js_RefreshSelf);
                    return false;
                }
            }
            else
            {
                //PageCommon.AlertMsg(this, "Invalid Due Time format.");
                PageCommon.WriteJsEnd(this, "Invalid Due Time format.", PageCommon.Js_RefreshSelf);
                return false;
            }
        }

        string sReminderUser = string.Empty;
        string sReminderUserID = string.Empty;
        int iReminderUserID = 0;
        string sReminderInterval = string.Empty;
        if (this.chkReminder.Checked == true)
        {
            sReminderUser = this.txtReminderUser.Text.Trim();
            sReminderUserID = this.hdnReminderUserID.Value;
            iReminderUserID = Convert.ToInt32(sReminderUserID);
            sReminderInterval = this.ddlReminderInterval.SelectedValue;
        }

        //CR53
        decimal loanAmount = string.IsNullOrEmpty(txtLoanAmount.Text) ? 0M : Convert.ToDecimal(txtLoanAmount.Text.Trim());

        string purpose = ddlPurpose.Text.Trim() == "- select -" ? "" : ddlPurpose.Text;

        #endregion

        #region create contact/prospect and loan

        int iContactId = 0;

        try
        {
            iContactId = this.CreateContactAndProspect(sFirstName, sLastName, sEmail, sType, sPhone, sDOB, sSSN, this.CurrUser.iUserID, purpose);
        }
        catch (Exception ex)
        {
            //PageCommon.AlertMsg(this, "Failed to save contact and prospect.");
            PageCommon.WriteJsEnd(this, "Failed to save contact and prospect." + ex.Message, PageCommon.Js_RefreshSelf);
            return false;
        }

        #endregion

        #region create loan

        //int iFileID = 0;

        try
        {
            iFileID = this.CreateLoan(iContactId, loanAmount, purpose);
        }
        catch (Exception ex)
        {
            //PageCommon.AlertMsg(this, "Failed to save loan info.");
            PageCommon.WriteJsEnd(this, "Failed to save loan info.", PageCommon.Js_RefreshSelf);
            return false;
        }

        #endregion

        #region Apply LoanWflTempl

        if (iWflTempID > 0)
        {
            try
            {
                this.ApplyLoanWflTempl(iFileID, iWflTempID, this.CurrUser.iUserID);
            }
            catch (Exception ex)
            {
                //PageCommon.AlertMsg(this, "Failed to apply workflow template to loan.");
                PageCommon.WriteJsEnd(this, "Failed to apply workflow template to loan.", PageCommon.Js_RefreshSelf);
                return false;
            }
        }

        #endregion

        #region GenerateWorkflow and get iCurrentLoanStageId

        int iCurrentLoanStageId = 0;
        if (iWflTempID > 0)
        {
            string sError;
            bool bResult = this.GenerateWorkflow(iFileID, iWflTempID, this.CurrUser.iUserID, out sError);

            if (bResult == false)
            {
                //PageCommon.AlertMsg(this, "Failed to generate workflow template: " + sError);
                PageCommon.WriteJsEnd(this, "Failed to generate workflow template: " + sError, PageCommon.Js_RefreshSelf);
                return false;
            }
            System.Threading.Thread.Sleep(2000);
            // get current loan stage id
            iCurrentLoanStageId = LPWeb.BLL.WorkflowManager.GetCurrentLoanStageId(iFileID);
        }
        else if (!string.IsNullOrEmpty(sTaskName))
        {
            iWflTempID = LPWeb.BLL.WorkflowManager.GetDefaultWorkflowTemplate("Prospect");
            if (iWflTempID < 1)
            {
                iCurrentLoanStageId = LPWeb.BLL.WorkflowManager.GenerateDefaultLoanStages(iFileID, "Prospect");
            }
        }



        #endregion

        #region Create Task and Reminder

        if (!string.IsNullOrEmpty(sTaskName))
        {
            #region create task
            if (iCurrentLoanStageId < 0)
            {
                //PageCommon.AlertMsg(this, "There is no loan stage available.");
                PageCommon.WriteJsEnd(this, "There is no loan stage available.", PageCommon.Js_RefreshSelf);
                return false;
            }
            int iLoanTaskId = 0;
            try
            {
                iLoanTaskId = this.CreateLoanTask(iFileID, sTaskName, this.CurrUser.iUserID, sDueDate, sDueTime, iCurrentLoanStageId);
            }
            catch (Exception ex)
            {

            }



            #endregion

            #region reminder

            if (this.chkReminder.Checked == true)
            {
                string sReminderDueTime = string.Empty;
                if (sDueTime != string.Empty)
                {
                    TimeSpan Interval = new TimeSpan();
                    if (sReminderInterval.Contains("minutes") == true)
                    {
                        string sMinutes = sReminderInterval.Replace(" minutes", "");
                        int iMinutes = Convert.ToInt32(sMinutes);
                        Interval = new TimeSpan(0, iMinutes, 0);
                    }
                    else if (sReminderInterval.Contains("hours") == true)
                    {
                        string sHours = sReminderInterval.Replace(" hours", "");
                        int iHours = Convert.ToInt32(sHours);
                        Interval = new TimeSpan(iHours, 0, 0);
                    }

                    TimeSpan ReminderDueTime = DueTime.Add(Interval);

                    sReminderDueTime = ReminderDueTime.ToString();
                }

                string sReminderTaskName = sTaskName + " - Quick Lead Form - Reminder";

                if (iReminderUserID == this.CurrUser.iUserID && sDueDate == DateTime.Now.ToString("MM/dd/yyyy"))
                {
                    this.CurrUser.InitTaskListDueToday();
                }
            }
            #endregion
        }

        #endregion

        #region API MailChimp_Subscribe

        if (sMarketing != string.Empty)
        {
            string sError2 = string.Empty;
            bool bResult2 = this.MailChimp_Subscribe(iContactId, sMarketing, out sError2);

            if (bResult2 == false)
            {
                //PageCommon.AlertMsg(this, "Failed to invoke API MailChimp_Subscribe: " + sError2);
                PageCommon.WriteJsEnd(this, "Failed to invoke API MailChimp_Subscribe: " + sError2, PageCommon.Js_RefreshSelf);
                return false;
            }
        }

        #endregion

        if (ddlLoanOfficer.SelectedValue == "0")  //Lead Routing Engine
        {
            #region  invoke the WCF API  LeadRouting_AssignLoanOfficer
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    LR_AssignLoanOfficerReq req = new LR_AssignLoanOfficerReq();
                    req.LoanId = iFileID;
                    req.LoanOfficerId = iLoanOfficerID;
                    req.BorrowerContactId = iContactId;
                    req.CoBorrowerContactId = 0;
                    req.ReqHdr = new ReqHdr();
                    req.ReqHdr.UserId = CurrUser.iUserID;
                    req.ReqHdr.SecurityToken = "SecurityToken";
                    RespHdr resp = client.LeadRouting_AssignLoanOfficer(req);
                }
            }
            catch (Exception ex)
            {

                PageCommon.WriteJsEnd(this, "Pulse Lead Manager is not running. Please select a Loan Officer from the list and save the lead.", PageCommon.Js_RefreshSelf);
                return false;
            }
            #endregion
        }

        return true;
    }
}
