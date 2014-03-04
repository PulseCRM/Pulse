using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using System.Data;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;

public partial class LoanDetailEdit : BasePage
{

    int iFileID = 0;
    int iContactID = 0;
    Loans loan = new Loans();
    LoginUser loginUser = new LoginUser();
    int iBorrowerContactId = -1;
    int iCoborrowerContactId = -1;
    DataTable relatedContactsTable = null;
    string sCloseDialogCodes = string.Empty;
    string sRefreshCodes = string.Empty;
    string sProspectLoanOfficer = string.Empty;
    Contacts contact = new Contacts();
    ListItemCollection lc = new ListItemCollection();

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        // CloseDialogCodes
        bool bIsValid = PageCommon.ValidateQueryString(this, "CloseDialogCodes", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.parent.location.href=window.parent.location.href");
        }
        this.sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";

        // RefreshCodes
        bIsValid = PageCommon.ValidateQueryString(this, "RefreshCodes", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.parent.location.href=window.parent.location.href");
        }
        this.sRefreshCodes = this.Request.QueryString["RefreshCodes"].ToString() + ";";

        #endregion

        string sFileID = this.Request.QueryString["FileID"];
        string sContactID = this.Request.QueryString["ContactID"];
        if (PageCommon.IsID(sFileID) == false)
        {
            iFileID = 0;
        }
        else
        {
            iFileID = int.Parse(sFileID);
        }

        if (PageCommon.IsID(sContactID) == false)
        {
            iContactID = 0;
            //return;
        }
        else
        {
            iContactID = int.Parse(sContactID);
        }
        hfFileID.Value = iFileID.ToString();
        if (!IsPostBack)
        {
            USStates.Init(ddlState);
            BindDLL();
            FillLabels();
        }
    }
    private void GetBorrowerListFromSctach()
    {
        string sqlCmd = string.Empty;
        DataSet ds = null;
        if (lc != null)
            lc.Clear();

        if (CurrUser.bIsBranchUser)
        {
            if (CurrUser.userRole.Name.ToUpper() == "LOAN OFFICER")
                sqlCmd = "select v.* from dbo.lpvw_GetuserGroups_ByRoleName v where v.UserId=" + CurrUser.iUserID;
            else
                sqlCmd = "select v.* from dbo.lpvw_GetUserGroups_ByRoleName v inner join users u on v.GroupId=u.GroupId where u.UserId=" + CurrUser.iUserID;
        }
        else
        {

        }
        if (sqlCmd != string.Empty)
        {
            ds = LPWeb.DAL.DbHelperSQL.Query(sqlCmd);
        }
        if (ds == null)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Failed", "$('#divContainer').hide();alert('Unable to find Loan Officer users.');window.parent.RefreshPage();", true);
            return;
        }
    }

    private void BindBorrower()
    {
        DataTable dt = null;
        string borrowerName = string.Empty;
        int iBorrowerContactId = iContactID;
        if (iFileID <= 0 && iContactID <= 0)
        {
            GetBorrowerListFromSctach();
            return;
        }
        try
        {
            if (iFileID > 0)
            {
                // loan officer name
                LoanTeam lt = new LoanTeam();
                sProspectLoanOfficer = lt.GetLoanOfficer(iFileID);

                #region Get Loan Officer ID

                string sSql = "select isnull(dbo.lpfn_GetLoanOfficerID(" + iFileID + "),0)";
                string sLoanOfficerIDx = LPWeb.DAL.DbHelperSQL.ExecuteScalar(sSql).ToString();
                
                #endregion


                if (!string.IsNullOrEmpty(sProspectLoanOfficer))
                {
                    if (ddlLoanOfficer.Items != null && ddlLoanOfficer.Items.Count > 0)
                    {
                        //ddlLoanOfficer.SelectedItem.Text = sProspectLoanOfficer;

                        ddlLoanOfficer.SelectedValue = sLoanOfficerIDx;

                        if (ddlLoanOfficer.SelectedValue == "-1")
                        {
                            //var s = sLoanOfficerIDx;
                            ddlLoanOfficer.Items.Add(new ListItem() { Text = sProspectLoanOfficer, Value = sLoanOfficerIDx });
                            ddlLoanOfficer.SelectedValue = sLoanOfficerIDx;
                        }

                        hfLoanOfficer.Value = ddlLoanOfficer.SelectedItem.Text;
                    }

                }
                dt = loan.GetBorrowerInfo(iFileID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["LastName"] == DBNull.Value || dt.Rows[0]["ContactId"] == DBNull.Value || dt.Rows[0]["FirstName"] == DBNull.Value)
                        iBorrowerContactId = iContactID;
                    else
                    {
                        iBorrowerContactId = (int)dt.Rows[0]["ContactId"];
                        if (iContactID <= 0)
                            iContactID = iBorrowerContactId;
                    }
                }

                borrowerName = contact.GetContactName(iBorrowerContactId);
                if (dt != null)
                    dt.Clear();
            }
            if (borrowerName == string.Empty)
            {
                borrowerName = contact.GetContactName(iContactID);
                iBorrowerContactId = iContactID;
            }
            if (iBorrowerContactId > 0)
            {
                ddlBorrower.Items.Add(new ListItem(borrowerName, iBorrowerContactId.ToString()));
                ddlBorrower.SelectedValue = iBorrowerContactId.ToString();
            }
            else
            {
                ddlBorrower.Items.Add(new ListItem("-- select --", "0"));
                ddlBorrower.SelectedValue = "0";
            }

            if (relatedContactsTable == null || relatedContactsTable.Rows.Count <= 0)
                relatedContactsTable = contact.GetRelatedContacts(iContactID);

            if (relatedContactsTable != null && relatedContactsTable.Rows.Count > 0)
            {
                lc.Clear();
                foreach (DataRow dr in relatedContactsTable.Rows)
                {
                    if (dr["ContactName"] == DBNull.Value || dr["RelContactID"] == null)
                        continue;
                    lc.Add(new ListItem(dr["ContactName"].ToString(), dr["RelContactID"].ToString()));
                }
            }
            if (lc.Count > 0)
            {
                foreach (ListItem item in lc)
                    ddlBorrower.Items.Add(item);
            }
        }
        catch
        { }
    }

    private void BindCoborrower()
    {
        //try
        //{
        //    DataTable dt = null;
        //    string coborrowerName = string.Empty;

        //    if (iFileID > 0)
        //    {
        //        dt = loan.GetCoBorrowerInfo(iFileID);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            if (dt.Rows[0]["LastName"] != DBNull.Value && dt.Rows[0]["ContactId"] != DBNull.Value && dt.Rows[0]["FirstName"] != DBNull.Value)
        //            {
        //                iCoborrowerContactId = (int)dt.Rows[0]["ContactId"];
        //            }
        //        }

        //        if (dt != null)
        //            dt.Clear();
        //    }
        //    if (iCoborrowerContactId > 0)
        //    {
        //        coborrowerName = contact.GetContactName(iCoborrowerContactId);
        //        ListItem item = new ListItem(coborrowerName, iCoborrowerContactId.ToString());
        //        ddlCoBorrower.Items.Add(item);
        //        ddlCoBorrower.SelectedValue = iCoborrowerContactId.ToString();
        //    }
        //    else
        //    {
        //        ddlCoBorrower.Items.Add(new ListItem("-- select --", "0"));
        //        ddlBorrower.SelectedValue = "-1";
        //        if (iBorrowerContactId != iContactID)
        //        {
        //            coborrowerName = contact.GetContactName(iContactID);
        //            ddlCoBorrower.Items.Add(new ListItem(coborrowerName, iContactID.ToString()));
        //        }
        //    }

        //    if (lc.Count > 0)
        //    {
        //        foreach (ListItem item in lc)
        //            ddlCoBorrower.Items.Add(item);
        //    }
        //}
        //catch
        //{ }

    }

    private void BindDLL()
    {
        try
        {
            Company_Loan_Programs programs = new Company_Loan_Programs();
            DataSet ds = programs.GetAllList();
            if (ds == null || ds.Tables.Count < 1)
            { }
            else
            {
                ddlLoanProgram.DataTextField = "LoanProgram";
                ddlLoanProgram.DataValueField = "LoanProgram";

                ddlLoanProgram.DataSource = ds;
                ddlLoanProgram.DataBind();
            }
            ListItem item = new ListItem("—select—", "");
            ddlLoanProgram.Items.Insert(0, item);
        }
        catch
        { }
        if (iContactID == 0)
        {
            return;
        }
        try
        {
            DataSet ds = loan.GetProspectCopyFromLoans(iContactID);
            if (ds == null || ds.Tables.Count < 1)
            { }
            else
            {
                ddlCopyFrom.DataTextField = "Loan";
                ddlCopyFrom.DataValueField = "FileId";

                ddlCopyFrom.DataSource = ds;
                ddlCopyFrom.DataBind();
            }
        }
        catch
        { }
        try
        {
            Users user = new Users();
            DataSet ds = new DataSet();
            string strWhere = "";
            if (CurrUser.bIsCompanyExecutive)
            {
                ds = user.GetConditionLoanOfficers("");
            }
            else if (CurrUser.bIsRegionExecutive)
            {
                
                strWhere += " AND (t.UserId IN(SELECT UserId FROM GroupUsers WHERE GroupID IN(SELECT GroupId FROM Groups WHERE RegionID IN(SELECT RegionId FROM RegionExecutives WHERE ExecutiveId = " + CurrUser.iUserID.ToString() + ")))";
                strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM RegionExecutives WHERE RegionId IN (SELECT RegionId FROM RegionExecutives WHERE ExecutiveId = {0}))", CurrUser.iUserID.ToString());
                strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM DivisionExecutives WHERE DivisionId IN(SELECT DivisionId FROM Divisions WHERE RegionID IN (SELECT RegionId FROM RegionExecutives WHERE ExecutiveId = {0})))", CurrUser.iUserID.ToString());
                strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId IN(SELECT BranchId FROM Branches WHERE RegionID IN (SELECT RegionId FROM RegionExecutives WHERE ExecutiveId = {0})))", CurrUser.iUserID.ToString());
                strWhere += ")";

                ds = user.GetConditionLoanOfficers(strWhere);
            }
            else if (CurrUser.bIsDivisionExecutive)
            {
                strWhere += " AND (t.UserId IN(SELECT UserId FROM GroupUsers WHERE GroupID IN(SELECT GroupId FROM Groups WHERE DivisionID IN(SELECT DivisionId FROM DivisionExecutives WHERE ExecutiveId = " + CurrUser.iUserID.ToString() + ")))";
                strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM DivisionExecutives WHERE DivisionId IN (SELECT DivisionId FROM DivisionExecutives WHERE ExecutiveId ={0}))", CurrUser.iUserID.ToString());
                strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId IN(SELECT BranchId FROM Branches WHERE DivisionID IN(SELECT DivisionId FROM DivisionExecutives WHERE ExecutiveId ={0})))", CurrUser.iUserID.ToString());

                strWhere += ")";

                ds = user.GetConditionLoanOfficers(strWhere);
            }
            else if (CurrUser.bIsBranchManager)
            {
                strWhere += " AND (t.UserId IN(SELECT UserId FROM GroupUsers WHERE GroupID IN(SELECT GroupId FROM Groups WHERE BranchID IN(SELECT BranchId FROM BranchManagers WHERE BranchMgrId = " + CurrUser.iUserID.ToString() + ")))";
                strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId IN (SELECT BranchId FROM BranchManagers WHERE BranchMgrId ={0}))", CurrUser.iUserID.ToString());
                strWhere += ")";

                ds = user.GetConditionLoanOfficers(strWhere);
            }
            else
            {
                ds = user.GetProspectLoanOfficers(iContactID);
            }

            if (ds == null || ds.Tables.Count < 1)
            { }
            else
            {
                ddlLoanOfficer.DataTextField = "FullName";
                ddlLoanOfficer.DataValueField = "UserId";

                ddlLoanOfficer.DataSource = ds;
                ddlLoanOfficer.DataBind();

                ddlLoanOfficer.Items.Insert(0, new ListItem() { Text = "-- select --", Value = "-1" });
                ddlLoanOfficer.SelectedValue = "-1";
            }
        }
        catch
        { }

        BindBorrower();

        if (ddlLoanOfficer.Items.Count > 0)
        {
            ddlLoanOfficer_SelectedIndexChanged(null, null);
        }

        BindCoborrower();

        #region bind Lead Source

        string sqlLeadSource = "SELECT [LeadSourceID],[LeadSource] FROM [dbo].[Company_Lead_Sources] order by LeadSource ";

        var dtLeadSource = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sqlLeadSource);

        ddlLeadSource.DataSource = dtLeadSource;
        ddlLeadSource.DataBind();
        ddlLeadSource.Items.Insert(0, new ListItem() { Text = "--Select--", Value = "" });

        #endregion

    }

    private void FillLabels()
    {
        if (iFileID == 0)
        {
            return;
        }
        LPWeb.Model.Loans model = new LPWeb.Model.Loans();
        model = loan.GetModel(iFileID);
        if (model == null)
        {
            return;
        }
        if (model.LoanAmount.HasValue && model.LoanAmount.Value > 0)
        {
            txbAmount.Text = string.Format("{0:C}", model.LoanAmount.Value.ToString());
        }
        else
        {
            txbAmount.Text = string.Empty;
        }

        if (model.Rate.HasValue && model.Rate.Value > 0)
        {
            txbInterestRate.Text = model.Rate.Value.ToString();
        }
        else
        {
            txbInterestRate.Text = string.Empty;
        }

        if (model.EstCloseDate.HasValue && model.EstCloseDate.Value > DateTime.Parse("1900-1-1"))
        {
            txbEstimatedDate.Text = model.EstCloseDate.Value.ToShortDateString();
        }
        else
        {
            txbEstimatedDate.Text = string.Empty;
        }
        if (model.Program.Length == 0)
        {
            ddlLoanProgram.SelectedIndex = 0;
        }
        else
        {
            if(!ddlLoanProgram.Items.Contains(new ListItem(model.Program, model.Program)))
            {
                ddlLoanProgram.Items.Add(new ListItem(model.Program, model.Program));
            }
            ddlLoanProgram.SelectedValue = model.Program;
        }
        txbPropertyAddress.Text = model.PropertyAddr;
        txbCity.Text = model.PropertyCity;
        txbZip.Text = model.PropertyZip;
        if (model.PropertyState.Length > 0)
        {
            ddlState.SelectedValue = model.PropertyState.Trim();
        }
        else
        {
            ddlState.SelectedIndex = 0;
        }
        if (model.Ranking.Length > 0)
        {
            ddlRanking.SelectedValue = model.Ranking.ToLower();
        }
        else
        {
            ddlRanking.SelectedIndex = -1;
        }
        if (model.LienPosition.Length > 0)
        {
            ddlLienPosition.SelectedValue = model.LienPosition.Trim();
        }
        if (model.Purpose.Length > 0)
        {
            ddlPurpose.SelectedValue = model.Purpose.Trim();
        }
        Contacts contact = new Contacts();
        try
        {
            string sBoID = contact.GetBorrowerDetails(iFileID, "Borrower")["ContactID"].ToString();
            ddlBorrower.SelectedValue = sBoID;
        }
        catch
        { }

        try
        {
            var dr= contact.GetBorrowerDetails(iFileID, "CoBorrower");
            string sCBoID =  dr["ContactID"].ToString();
            //ddlCoBorrower.SelectedValue = sCBoID;
            hdnCoBorrowerID.Value = sCBoID;
            txtCBFirstname.Text = dr["FirstName"] != DBNull.Value ? dr["FirstName"].ToString() : "";
            txtCBMiddlename.Text = dr["Middlename"] != DBNull.Value ? dr["Middlename"].ToString() : "";
            txtCBLastname.Text = dr["Lastname"] != DBNull.Value ? dr["Lastname"].ToString() : "";

            hdnCoBorrowerName.Value = txtCBFirstname.Text + txtCBMiddlename.Text + txtCBLastname.Text;

        }
        catch
        { }

        BindPoint(iFileID);

        if (model.Created.HasValue)
            lbCreatedOn.Text = model.Created.Value.ToShortDateString();
        if (model.CreatedBy.HasValue)
            lbCreatedBy.Text = GetUserName(model.CreatedBy.Value);

        if (model.Modifed.HasValue)
            lbCreatedOn.Text = model.Modifed.Value.ToShortDateString();
        if (model.ModifiedBy.HasValue)
            lbCreatedBy.Text = GetUserName(model.ModifiedBy.Value);

        //add by gdc 20110828

        txbPropetyType.Text = model.PropertyType;
        txbHousingStatus.Text = model.HousingStatus;
        cbInterestOnly.Checked = model.InterestOnly;
        cbIncludeEscrows.Checked = model.IncludeEscrows;

        txbCoborrowerType.Text = model.CoBrwType;
        txbRentAmount.Text = model.RentAmount == null ? "" : Convert.ToDecimal(model.RentAmount).ToString("n0");


        //gdc crm33
        LPWeb.Model.Prospect modelProspect = new LPWeb.Model.Prospect();

        LPWeb.BLL.Prospect bllProspect = new Prospect();
        modelProspect = bllProspect.GetModel(iContactID);

        if (modelProspect != null & modelProspect.Referral != null)
        {
            hdnReferralID.Value = modelProspect.Referral.ToString();

            string sqlReferralName = string.Format("select dbo.lpfn_GetReferralName({0})", iFileID);

            var obj = LPWeb.DAL.DbHelperSQL.GetSingle(sqlReferralName);
            if (obj != null)
            {
                txtReferral.Text = obj.ToString();
            }
            

        }

        ddlLeadSource.SelectedValue = modelProspect.LeadSource;

    }

    private void BindPoint(int FileID)
    {
        try
        {
            //btnExport.Enabled = false;
            PointFiles file = new PointFiles();
            LPWeb.Model.PointFiles fileModel = new LPWeb.Model.PointFiles();
            fileModel = file.GetModel(iFileID);

            if (fileModel == null)
            {
                return;
            }
            txbPointFileName.Text = fileModel.Name;
            PointFolders folder = new PointFolders();
            if (fileModel.FolderId > 0)
            {

                LPWeb.Model.PointFolders folderModel = new LPWeb.Model.PointFolders();
                folderModel = folder.GetModel(fileModel.FolderId);
                if (folderModel != null && folderModel.Name.Length > 0)
                {
                    ListItem item = new ListItem(folderModel.Name, folderModel.FolderId.ToString());
                    ddlPointFolder.Items.Insert(0, item);
                }
                ddlPointFolder.Enabled = false;
            }
            else
                if (ddlPointFolder.Items.Count > 1)
                    ddlPointFolder.Enabled = true;

            if (txbPointFileName.Text.Length > 0 && ddlPointFolder.Text.Length > 0)
            {
                if (!txbPointFileName.Text.ToUpper().EndsWith(".PRS") &&
                    !txbPointFileName.Text.ToUpper().EndsWith(".BRW"))
                {
                    //btnExport.Enabled = false;
                    return;
                }
                string filename = System.IO.Path.GetFileName(txbPointFileName.Text);
                if (txbPointFileName.Text.ToUpper().EndsWith(".PRS"))
                    txbPointFileName.Text = @"\PROSPECT\" + filename;
                else
                    txbPointFileName.Text = @"\BORROWER\" + filename;
                ///btnExport.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            LPLog.LogMessage(ex.Message);
        }
    }

    private string GetUserName(int UserID)
    {
        Users user = new Users();
        try
        {
            LPWeb.Model.Users model = user.GetModel(UserID);
            return model.LastName + ", " + model.FirstName;
        }
        catch
        {
            return string.Empty;
        }
    }

    private bool CheckInput()
    {
        if (ddlBorrower.SelectedIndex < 0)
        {
            PageCommon.AlertMsg(this, "Please select a borrower.");
            return false;
        }

        //if (ddlCoBorrower.SelectedIndex > 0)
        //{
        //    if (ddlCoBorrower.SelectedValue == ddlBorrower.SelectedValue)
        //    {
        //        PageCommon.AlertMsg(this, "The coborrower same to borrower.");
        //        return false;
        //    }
        //}

        if (txbAmount.Text.Trim().Length > 0)
        {
            decimal dAmount;
            bool bDecimalAmount = decimal.TryParse(txbAmount.Text.Trim(), out dAmount);
            if (bDecimalAmount == false)
            {
                PageCommon.AlertMsg(this, "Amount should be the decimal type.");
                return false;
            }
        }

        if (txbInterestRate.Text.Trim().Length > 0)
        {
            decimal dRate;
            bool bDecimalRate = decimal.TryParse(txbInterestRate.Text.Trim(), out dRate);
            if (bDecimalRate == false)
            {
                PageCommon.AlertMsg(this, "Rate should be the decimal type.");
                return false;
            }
        }

        if (txbEstimatedDate.Text.Trim().Length > 0)
        {
            DateTime TempDate;
            bool bDate = DateTime.TryParse(txbEstimatedDate.Text.Trim(), out TempDate);
            if (bDate == false)
            {
                PageCommon.AlertMsg(this, "Estimated Date should be the datetime type.");
                return false;
            }
        }
        if (txbPointFileName.Text.Trim().Length <= 0)
            return true;
        string filename = System.IO.Path.GetFileName(txbPointFileName.Text).Trim();
        if (filename.Length < 6 || (!filename.ToUpper().EndsWith(".PRS") &&
            (!filename.ToUpper().EndsWith(".BRW"))))
        {
            PageCommon.AlertMsg(this, "Point filename must be valid and must end with .PRS or .BRW.");
            //btnExport.Enabled = false;
            return false;
        }

        if (filename.ToUpper().EndsWith(".PRS"))
            txbPointFileName.Text = @"\PROSPECT\" + filename;
        else if (filename.ToUpper().EndsWith(".BRW"))
            txbPointFileName.Text = @"\BORROWER\" + filename;

         //btnExport.Enabled = true;
        return true;
    }

    private bool AllowReassignProspect()
    {
        bool EnableMarketing = false;

        if (hfLoanOfficer.Value.Length == 0 || ddlLoanOfficer.SelectedItem.Text == hfLoanOfficer.Value)
        {
            return EnableMarketing;
        }

        try
        {
            LPWeb.BLL.Company_General cg = new LPWeb.BLL.Company_General();
            EnableMarketing = cg.CheckMarketingEnabled();
        }
        catch
        { }

        return EnableMarketing;
    }

    private void ReassignProspect()
    {
        if (!AllowReassignProspect())
        {
            return;
        }
        try
        {
            ServiceManager sm = new ServiceManager();

            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                ReassignProspectRequest rpq = new ReassignProspectRequest();
                rpq.hdr = new ReqHdr();
                rpq.FileId = new int[1] { this.iFileID };
                rpq.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                rpq.hdr.UserId = CurrUser.iUserID;
                rpq.FromUser = int.Parse(hfLoanOfficer.Value);
                rpq.ToUser = int.Parse(ddlLoanOfficer.SelectedValue);
                rpq.ContactId = null;
                rpq.UserId = null;
                ReassignProspectResponse rpp = null;
                rpp = service.ReassignProspect(rpq);

                if (!rpp.hdr.Successful)
                {
                    PageCommon.AlertMsg(this, rpp.hdr.StatusInfo);
                }
            }
        
        }
        catch(Exception ex)
        {
            LPLog.LogMessage(LogType.Logerror, ex.Message);
        }

    }

    protected void ddlLoanOfficer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int iLOID = 0;
        try
        {
            iLOID = int.Parse(ddlLoanOfficer.SelectedItem.Value);
        }
        catch
        {
            return;
        }

        try
        {
            if (ddlPointFolder.Enabled == false)
                return;
            DataSet ds = loan.GetProspectPointFolders(iLOID);
            if (ds == null || ds.Tables.Count < 1)
            { }
            else
            {
                ddlPointFolder.DataTextField = "Name";
                ddlPointFolder.DataValueField = "FolderId";

                ddlPointFolder.DataSource = ds;
                ddlPointFolder.DataBind();
            }
        }
        catch (Exception ex)
        {
            LPLog.LogMessage(ex.Message);
        }
    }

    private int Save_LoanDetail()
    {
        ReassignProspect();

        #region gdc crm33
        int iContactCoBoId = string.IsNullOrEmpty(hdnCoBorrowerID.Value) ? 0 : Convert.ToInt32(hdnCoBorrowerID.Value);
        string CBname = txtCBFirstname.Text + txtCBMiddlename.Text + txtCBLastname.Text;

        if ((CBname != hdnCoBorrowerName.Value.Trim() || iContactCoBoId == 0) && !string.IsNullOrEmpty(CBname))
        {
            LPWeb.Model.Contacts contactRecCoBo = new LPWeb.Model.Contacts();
            Contacts contactsbll = new Contacts();//gdc crm33
            contactRecCoBo.ContactId = 0;
            contactRecCoBo.FirstName = txtCBFirstname.Text.Trim();
            contactRecCoBo.MiddleName = txtCBMiddlename.Text.Trim();
            contactRecCoBo.LastName = txtCBLastname.Text.Trim();

            iContactCoBoId = contactsbll.AddClient(contactRecCoBo);//gdc crm33

            #region CoBo to loanContacts  gdc crm33
            LPWeb.BLL.LoanContacts loanContactsBll = new LoanContacts();


            LPWeb.BLL.ContactRoles contactRolesbll = new ContactRoles();
            int contactRoleID = 0;
            var contactRoleList = contactRolesbll.GetModelList(" Name = 'CoBorrower' ");
            if (contactRoleList != null && contactRoleList.Count > 0 && contactRoleList.FirstOrDefault() != null)
            {
                contactRoleID = contactRoleList.FirstOrDefault().ContactRoleId;
            }

            if (contactRoleID != 0)
            {

                LPWeb.Model.LoanContacts loanContactModel = new LPWeb.Model.LoanContacts();
                loanContactModel.FileId = iFileID;
                loanContactModel.ContactRoleId = contactRoleID;
                loanContactModel.ContactId = iContactCoBoId;

                loanContactsBll.Add(loanContactModel);
            }

            #endregion


        }
        #endregion

        LPWeb.Model.LoanDetails model = new LPWeb.Model.LoanDetails();
        model.FileId = iFileID;
        if (ddlBorrower.Items.Count < 1)
        {
            model.BoID = 0;
        }
        else
        {
            model.BoID = int.Parse(ddlBorrower.SelectedValue);
        }
        //if (ddlCoBorrower.Items.Count < 1)
        //{
        //    model.CoBoID = 0;
        //}
        //else
        //{
        //    model.CoBoID = int.Parse(ddlCoBorrower.SelectedValue);
        //}

        if (iContactCoBoId != 0)
        {
            model.CoBoID = iContactCoBoId;
        }
        
        
        model.Created = DateTime.Now;
        model.CreatedBy = loginUser.iUserID;
        model.Modifed = DateTime.Now;
        model.ModifiedBy = loginUser.iUserID;
        if (txbAmount.Text.Trim().Length < 1)
        {
            model.LoanAmount = 0;
        }
        else
        {
            model.LoanAmount = decimal.Parse(txbAmount.Text.Trim());
        }

        if (txbEstimatedDate.Text.Trim().Length > 5)
        {
            model.EstCloseDate = DateTime.Parse(txbEstimatedDate.Text.Trim());
        }
        else
        {
            model.EstCloseDate = DateTime.Parse("1900-1-1");
        }
        if (ddlRanking.SelectedIndex >= 0)
        {
            model.Ranking = ddlRanking.SelectedValue;
        }
        model.UserId = CurrUser.iUserID;
        if (ddlLoanOfficer.Items.Count < 1)
        {
            model.LoanOfficerId = 0;
        }
        else
        {
            model.LoanOfficerId = int.Parse(ddlLoanOfficer.SelectedValue);
        }
        if (txbInterestRate.Text.Trim().Length < 1)
        {
            model.Rate = 0;
        }
        else
        {
            model.Rate = decimal.Parse(txbInterestRate.Text.Trim());
        }

        if (ddlLoanProgram.SelectedIndex == 0)
        {
            model.Program = "";
        }
        else
        {
            model.Program = ddlLoanProgram.SelectedItem.Text;
        }
        if (ddlPurpose.SelectedIndex == 0)
        {
            model.Purpose = "";
        }
        else
        {
            model.Purpose = ddlPurpose.SelectedItem.Text.Trim();
        }

        model.Lien = ddlLienPosition.SelectedItem.Value.Trim();
        model.PropertyAddr = txbPropertyAddress.Text.Trim();
        model.PropertyCity = txbCity.Text.Trim();
        model.PropertyState = ddlState.SelectedValue;
        model.PropertyZip = txbZip.Text.Trim();
        if (ddlPointFolder.Items.Count < 1)
        {
            model.FolderId = 0;
        }
        else
        {
            model.FolderId = int.Parse(ddlPointFolder.SelectedValue);
        }
        model.FileName = txbPointFileName.Text.Trim();
        model.Status = "Prospect";
        model.ProspectLoanStatus = "Active";

        model.PropertyType = txbPropetyType.Text.Trim();
        model.HousingStatus = txbHousingStatus.Text.Trim();
        model.IncludeEscrows = cbIncludeEscrows.Checked;
        model.InterestOnly = cbInterestOnly.Checked;
        model.RentAmount = string.IsNullOrEmpty(txbRentAmount.Text.Trim()) ? 0 : Convert.ToDecimal(txbRentAmount.Text.Trim());
        model.CoborrowerType = txbCoborrowerType.Text.Trim();
        iFileID = loan.LoanDetailSaveFileId(model);


        #region Referral
        LPWeb.Model.Prospect modelProspect = new LPWeb.Model.Prospect();

        LPWeb.BLL.Prospect bllProspect = new Prospect();
        modelProspect = bllProspect.GetModel(iContactID);

        modelProspect.LeadSource = ddlLeadSource.SelectedValue; //gdc CR40

        int referralIDNew = string.IsNullOrEmpty(hdnReferralID.Value.Trim()) ? 0 : Convert.ToInt32(hdnReferralID.Value.Trim());

        if (referralIDNew > 0 && referralIDNew.ToString() != modelProspect.Referral.ToString())
        {
            int referralIDOld = modelProspect.Referral == null ? 0 : Convert.ToInt32(modelProspect.Referral);

            modelProspect.Referral = referralIDNew;
            
            #region Referral to loanContacts  gdc crm33
            LPWeb.BLL.LoanContacts loanContactsBll = new LoanContacts();

            LPWeb.BLL.ContactRoles contactRolesbll = new ContactRoles();
            int refrralRoleID = 0;
            var referralRoleList = contactRolesbll.GetModelList(" Name = 'Referral' ");
            if (referralRoleList != null && referralRoleList.Count > 0 && referralRoleList.FirstOrDefault() != null)
            {
                refrralRoleID = referralRoleList.FirstOrDefault().ContactRoleId;
            }

            if (refrralRoleID != 0)
            {

                LPWeb.Model.LoanContacts loanContactModel = new LPWeb.Model.LoanContacts();
                loanContactModel.FileId = iFileID;
                loanContactModel.ContactRoleId = refrralRoleID;
                loanContactModel.ContactId = referralIDNew;

                loanContactsBll.Add(loanContactModel);
            }

            #endregion

            
            #region Del Old loanContacts

            try
            {
                if (referralIDOld > 0)
                {
                    loanContactsBll.Delete(iFileID, refrralRoleID, referralIDOld);
                }
            }
            catch { }

            #endregion
        }

        bllProspect.Update(modelProspect); //gdc CR40 

        #endregion

        return iFileID;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!CheckInput())
        {
            return;
        }
        string err = string.Empty;
        try
        {
            iFileID = Save_LoanDetail();
            if (string.IsNullOrEmpty(this.txbPointFileName.Text) || string.IsNullOrEmpty(ddlPointFolder.SelectedValue))
            {
                // success
                PageCommon.WriteJs(this, "Save loan detail successfully.", this.sRefreshCodes + this.sCloseDialogCodes);
                return;
            }

            if (UpdatePointFile(iFileID, true, ref err) == false)
            {
                err = string.Format("Failed to update the Point file, FileId={0}, Error:{1}", iFileID, err);
                PageCommon.WriteJs(this, err, this.sRefreshCodes + this.sCloseDialogCodes);
                return;
            }
        }
        catch (Exception ex)
        {
            LPLog.LogMessage(ex.Message);
            err = string.Format("Failed to save the loan, FileId={0}, Error:{1}", iFileID, ex.Message);
            PageCommon.WriteJsEnd(this, err, this.sRefreshCodes + this.sCloseDialogCodes);
        }

        // success
        PageCommon.WriteJsEnd(this, "Save loan detail successfully.", this.sRefreshCodes + this.sCloseDialogCodes);
    }

    private bool UpdatePointFile(int iFileID, bool CreateFile, ref string err)
    {
        ServiceManager sm = new ServiceManager();
        UpdateLoanInfoResponse response = null;
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            #region UpdateLoanInfoRequest

            UpdateLoanInfoRequest req = new UpdateLoanInfoRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = this.CurrUser.iUserID;
            req.FileId = iFileID;
            req.CreateFile = CreateFile;
            #endregion

            response = service.UpdateLoanInfo(req);
            err = response.hdr.StatusInfo;
        }
        return response.hdr.Successful;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        //if (string.IsNullOrEmpty(this.txbPointFileName.Text) || string.IsNullOrEmpty(ddlPointFolder.SelectedValue))
        if (string.IsNullOrEmpty(ddlPointFolder.SelectedValue))
        {
            PageCommon.WriteJs(this, "You must specify the Point Folder.", this.sRefreshCodes + this.sCloseDialogCodes);
            return;
        }
        iFileID = Save_LoanDetail();
        string sExMsg = string.Empty;
        try
        {
            if (UpdatePointFile(iFileID, true, ref sExMsg) == false)
            {
                PageCommon.WriteJs(this, sExMsg, this.sRefreshCodes + this.sCloseDialogCodes);
                return;
            }
            PageCommon.WriteJsEnd(this, "Exported to Point file successfully.", this.sRefreshCodes + this.sCloseDialogCodes);
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            sExMsg = string.Format("Failed to export to the Point File, FileId={0}, {1}", this.iFileID, "Point Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, this.sRefreshCodes + this.sCloseDialogCodes);
        }
        catch (Exception ex)
        {
            sExMsg = string.Format("Failed to export to the Point File, FileID={0}, Error:{1}", this.iFileID, ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, this.sRefreshCodes + this.sCloseDialogCodes);
        }


    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        if (ddlCopyFrom.SelectedIndex < 0)
        {
            return;
        }
        try
        {
            int FileID = int.Parse(ddlCopyFrom.SelectedItem.Value);
            LPWeb.Model.Loans model = new LPWeb.Model.Loans();
            model = loan.GetModel(FileID);
            // Loan.Status+”Loan “+Loans.LienPosition+” – “+Loans.PropertyAddr

            if (model == null)
            {
                return;
            }
            if (model.LoanAmount.HasValue && model.LoanAmount.Value > 0)
            {
                txbAmount.Text = model.LoanAmount.Value.ToString("n0");
            }
            else
            {
                txbAmount.Text = string.Empty;
            }

            if (model.Rate.HasValue && model.Rate.Value > 0)
            {
                txbInterestRate.Text = model.Rate.Value.ToString();
            }
            else
            {
                txbInterestRate.Text = string.Empty;
            }

            //if (model.EstCloseDate.HasValue && model.EstCloseDate.Value > DateTime.Parse("1900-1-1"))
            //{
            //    txbEstimatedDate.Text = model.EstCloseDate.Value.ToShortDateString();
            //}
            //else
            //{
            //    txbEstimatedDate.Text = string.Empty;
            //}
            if (model.Program.Length == 0)
            {
                ddlLoanProgram.SelectedIndex = 0;
            }
            else
            {
                ddlLoanProgram.SelectedValue = model.Program;
            }
            if (model.LienPosition.Length == 0)
            {
                ddlLienPosition.SelectedIndex = 0;
            }
            else
                ddlLienPosition.SelectedValue = model.LienPosition;

            if (model.Purpose.Length == 0)
            {
                ddlPurpose.SelectedIndex = 0;
            }
            else
                ddlPurpose.SelectedValue = model.Purpose;
            txbPropertyAddress.Text = model.PropertyAddr;
            txbCity.Text = model.PropertyCity;
            txbZip.Text = model.PropertyZip;
            if (model.PropertyState.Length > 0)
            {
                ddlState.SelectedValue = model.PropertyState.Trim();
            }
            else
            {
                ddlState.SelectedIndex = 0;
            }
            if (model.Ranking.Length > 0)
            {
                ddlRanking.SelectedValue = model.Ranking.ToLower();
            }
            else
            {
                ddlRanking.SelectedIndex = -1;
            }

        }
        catch (Exception ex)
        {
            LPLog.LogMessage(ex.Message);
        }
    }

    protected void btnCopyAddress_Click(object sender, EventArgs e)
    {
        if (ddlBorrower.SelectedIndex < 0)
        {
            return;
        }
        try
        {
            int ContactID = int.Parse(ddlBorrower.SelectedItem.Value);
            LPWeb.Model.Contacts model = new LPWeb.Model.Contacts();
            Contacts ct = new Contacts();
            model = ct.GetModel(ContactID);
            // Loan.Status+”Loan “+Loans.LienPosition+” – “+Loans.PropertyAddr
            txbPropertyAddress.Text = model.MailingAddr;
            txbCity.Text = model.MailingCity;
            txbZip.Text = model.MailingZip;
            ddlState.SelectedValue = model.MailingState;
        }
        catch (Exception ex)
        {
            LPLog.LogMessage(ex.Message);
        }
    }

}
