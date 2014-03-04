using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;
using System.Text;

public partial class Prospect_ProspectDetailCreate : BasePage
{
    int iLoanOfficerRoleID = 3;
    int iBranchID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (this.IsPostBack == false)
        //{
        //    if (ddlState.Items.Count <= 0)
        //        USStates.Init(ddlState);
        //    if (ddlPropState.Items.Count <= 0)
        //        USStates.Init(ddlPropState);
        //    //#region 加载Loan Officer列表

        //    //DataTable LoanOfficerList = this.GetLoanOfficerList(this.CurrUser.iUserID);
        //    //this.ddlLoanOfficer.DataSource = LoanOfficerList;
        //    //this.ddlLoanOfficer.DataBind();

        //    //#endregion

        //    #region 加载Lead Source列表

        //    Company_Lead_Sources LeadSourceManager = new Company_Lead_Sources();
        //    DataTable LeadSourceList = LeadSourceManager.GetList(string.Empty).Tables[0];
        //    this.ddlLeadSource.DataSource = LeadSourceList;
        //    this.ddlLeadSource.DataBind();
        //    this.ddlLeadSource.Items.Insert(0, new ListItem("-- select --", ""));

        //    #endregion

        //    LPWeb.BLL.Roles bRoles = new LPWeb.BLL.Roles();
        //    DataSet ds = bRoles.GetList(" [Name]='Loan Officer' ");

        //    if (ds != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        iLoanOfficerRoleID = Convert.ToInt32(ds.Tables[0].Rows[0]["RoleId"]);
        //    }
        //    LoadBranches();
        //    if (CurrUser.iRoleID == iLoanOfficerRoleID)
        //    {
        //        ddlBranch.Enabled = false;
        //        ddlLoanOfficer.Items.Add(new ListItem(CurrUser.iUserID.ToString(), CurrUser.sFullName));
        //        ddlLoanOfficer.SelectedValue = CurrUser.iUserID.ToString();
        //        ddlLoanOfficer.SelectedItem.Text = CurrUser.sFullName;
        //        ddlLoanOfficer.Enabled = false;
        //    }
        //}
    }

    private void LoadBranches()
    {
        //if (ddlBranch.Enabled == false)
        //    return;
        //Branches bBranches = new Branches();
        //string sCondition = " BranchId in (select BranchId from ";

        //if (CurrUser.bIsCompanyExecutive || CurrUser.bIsRegionExecutive || CurrUser.bIsDivisionExecutive)
        //    sCondition += string.Format("dbo.lpfn_GetUserBranches_Executive({0}))", CurrUser.iUserID);
        //else if (CurrUser.bIsBranchManager)
        //    sCondition += string.Format("dbo.lpfn_GetUserBranches_Branch_Manager({0})) ", CurrUser.iUserID);
        //else
        //    sCondition += string.Format(" dbo.lpfn_GetUserBranches({0}))",CurrUser.iUserID);
        //DataSet dsBranches = bBranches.GetList(sCondition);
        //DataTable dtBH = new DataTable();
        //if (dsBranches != null && dsBranches.Tables[0].Rows.Count > 0)
        //{
        //    dtBH = dsBranches.Tables[0];
        //}
        //if (!dtBH.Columns.Contains("BranchId"))
        //{
        //    dtBH.Columns.Add("BranchId");
        //}
        //if (!dtBH.Columns.Contains("Name"))
        //{
        //    dtBH.Columns.Add("Name");
        //}
        //DataRow drbh = dtBH.NewRow();
        //drbh["BranchId"] = "-1";
        //drbh["Name"] = "-- select --";
        //dtBH.Rows.InsertAt(drbh, 0);
        //ddlBranch.DataSource = dtBH;
        //ddlBranch.DataBind();
        //Users bUsers = new Users();
        //DataTable dt = null;
        //if (CurrUser.bIsBranchUser)
        //{
        //    dt = bUsers.GetUserBranchInfo(CurrUser.iUserID.ToString());
        //    if (dt.Rows.Count > 0)
        //    {
        //        ddlBranch.SelectedValue = dt.Rows[0]["BranchID"].ToString();
        //        iBranchID = Convert.ToInt32(ddlBranch.SelectedValue);
        //        BindLoanOfficer(iBranchID.ToString());
        //        //ddlBranch.Enabled = false;
        //        dt.Clear();
        //        dt.Dispose();
        //    }
        //}
    }


    //private bool CheckInput()
    //{
    //    if (txbAmount.Text.Trim().Length > 0)
    //    {
    //        decimal dAmount;
    //        bool bDecimalAmount = decimal.TryParse(txbAmount.Text.Trim(), out dAmount);
    //        if (bDecimalAmount == false)
    //        {
    //            PageCommon.AlertMsg(this, "Amount must be numeric.");
    //            return false;
    //        }
    //    }

    //    if (txbInterestRate.Text.Trim().Length > 0)
    //    {
    //        decimal dRate;
    //        bool bDecimalRate = decimal.TryParse(txbInterestRate.Text.Trim(), out dRate);
    //        if (bDecimalRate == false)
    //        {
    //            PageCommon.AlertMsg(this, "Rate must be decimal.");
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    //protected void ddlBranch_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    iBranchID = Convert.ToInt32(ddlBranch.SelectedValue);
    //    if (iBranchID > 0)
    //         BindLoanOfficer(iBranchID.ToString());
    //}

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    #region 获取用户输入
    //    if (!CheckInput())
    //    {
    //        return;
    //    }
    //    #endregion
    //    int iLoanOfficerID = 0;
    //    if (CurrUser.iRoleID == iLoanOfficerRoleID)
    //    {
    //        iLoanOfficerID = CurrUser.iUserID;
    //    }
    //    else
    //    {
    //        string sLoanOfficerID = this.ddlLoanOfficer.SelectedValue;

    //        if (!int.TryParse(sLoanOfficerID, out iLoanOfficerID))
    //            iLoanOfficerID = 0;
    //    }
    //    LPWeb.Model.Contacts contactRec = new LPWeb.Model.Contacts();
    //    contactRec.ContactId = 0;
    //    contactRec.FirstName = txtFirstName.Text.Trim();
    //    contactRec.MiddleName = txtMiddleName.Text.Trim();
    //    contactRec.LastName = txtLastName.Text.Trim();
    //    contactRec.NickName = txtFirstName.Text.Trim();
    //    contactRec.Title = ddlTitle.SelectedValue.Trim();
    //    contactRec.GenerationCode = txtGenerationCode.Text.Trim();
    //    contactRec.HomePhone = txtHomePhone.Text.Trim();
    //    contactRec.CellPhone = txtCellPhone.Text.Trim();
    //    contactRec.BusinessPhone = txtBizPhone.Text.Trim();
    //    contactRec.Fax = txtFax.Text.Trim();
    //    contactRec.Email = txtEmail.Text.Trim();
    //    contactRec.DOB = null;
    //    contactRec.MailingAddr = txtAddress.Text.Trim();
    //    contactRec.MailingCity = txtCity.Text.Trim();
    //    contactRec.MailingState = ddlState.SelectedValue.Trim();
    //    contactRec.MailingZip = txtZip.Text.Trim();

    //    #region gdc crm33

    //    LPWeb.Model.Contacts contactRecCoBo = new LPWeb.Model.Contacts();

    //    contactRecCoBo.ContactId = 0;
    //    contactRecCoBo.FirstName = txtCBFirstname.Text.Trim();
    //    contactRecCoBo.MiddleName = txtCBMiddlename.Text.Trim();
    //    contactRecCoBo.LastName = txtCBLastname.Text.Trim(); 
    //    #endregion


    //    LPWeb.Model.Prospect prospectRec = new LPWeb.Model.Prospect();
    //    prospectRec.Created = DateTime.Now;
    //    prospectRec.CreatedBy = this.CurrUser.iUserID;
    //    prospectRec.LeadSource = ddlLeadSource.SelectedValue.Trim();
    //    prospectRec.Loanofficer = iLoanOfficerID;
    //    prospectRec.ReferenceCode = txtRefCode.Text.Trim();
    //    prospectRec.Status = "Active";
    //    prospectRec.Contactid = 0;
    //    prospectRec.CreditRanking = this.ddlCreditRanking.SelectedValue;
    //    prospectRec.PreferredContact = this.ddlPreferredContact.SelectedValue;

    //    prospectRec.Referral = hdnReferralID.Value.Trim();//gdc crm33

    //    #region Update prospect detail

    //    Prospect ProspectManager = new Prospect();
    //    Contacts contactsbll = new Contacts();//gdc crm33
    //    try
    //    {
    //        int iContactId = ProspectManager.CreateContactAndProspect(contactRec, prospectRec);
    //        int iContactCoBoId = contactsbll.AddClient(contactRecCoBo);//gdc crm33
    //        if (iContactId > 0)
    //        {
    //            LPWeb.Model.LoanDetails loan = new LPWeb.Model.LoanDetails();
    //            loan.BoID = iContactId;
    //            loan.CoBoID = iContactCoBoId > 0 ? iContactCoBoId : 0; ; //gdc crm33
    //            loan.Created = DateTime.Now;
    //            loan.CreatedBy = this.CurrUser.iUserID;
    //            loan.Lien = ddlLienPosition.SelectedValue;
    //            decimal dTemp = 0;
    //            if (!decimal.TryParse(txbAmount.Text.Trim(), out dTemp))
    //            {
    //                dTemp = 0;
    //            }
    //            loan.LoanAmount = dTemp;
    //            iBranchID = Convert.ToInt32(ddlBranch.SelectedValue);
    //            if (iBranchID > 0)
    //                loan.BranchId = iBranchID;
    //            else
    //                loan.BranchId = 0;
    //            loan.FolderId = 0;
    //            loan.FileName = null;
    //            loan.LoanOfficerId = iLoanOfficerID;
    //            loan.Purpose = ddlPurpose.SelectedValue;
    //            loan.Ranking = ddlRanking.SelectedValue;
    //            loan.Status = "Prospect";
    //            loan.ProspectLoanStatus = "Active";
    //            if (!decimal.TryParse(txbInterestRate.Text.Trim(), out dTemp))
    //            {
    //                dTemp = 0;
    //            }
    //            loan.Rate = dTemp;
    //            loan.PropertyAddr = txbPropertyAddress.Text.Trim();
    //            loan.PropertyCity = txbPropertyCity.Text.Trim();
    //            loan.PropertyState = ddlPropState.SelectedValue;
    //            loan.PropertyZip = txbPropertyZip.Text.Trim();
    //            loan.UserId = this.CurrUser.iUserID;
    //            loan.Program = string.Empty;
    //            loan.EstCloseDate = null;
    //            LPWeb.BLL.Loans loanMgr = new Loans();
    //            loanMgr.LoanDetailSave(loan);

    //            #region Referral to loanContacts  gdc crm33

    //            int referralId = 0;
    //            try
    //            {
    //                if (!string.IsNullOrEmpty(prospectRec.Referral) && loan.FileId > 0)
    //                {
    //                    referralId = Convert.ToInt32(prospectRec.Referral);

    //                    if (referralId > 0)
    //                    {

    //                        LPWeb.BLL.ContactRoles contactRolesbll = new ContactRoles();
    //                        int refrralRoleID = 0;
    //                        var referralRoleList = contactRolesbll.GetModelList(" Name = 'Referral' ");
    //                        if (referralRoleList != null && referralRoleList.Count > 0 && referralRoleList.FirstOrDefault() != null)
    //                        {
    //                            refrralRoleID = referralRoleList.FirstOrDefault().ContactRoleId;
    //                        }

    //                        if (refrralRoleID != 0)
    //                        {
    //                            LPWeb.BLL.LoanContacts loanContactsBll = new LoanContacts();
    //                            LPWeb.Model.LoanContacts loanContactModel = new LPWeb.Model.LoanContacts();
    //                            loanContactModel.FileId = loan.FileId;
    //                            loanContactModel.ContactRoleId = refrralRoleID;
    //                            loanContactModel.ContactId = referralId;

    //                            loanContactsBll.Add(loanContactModel);
    //                        }
    //                    }
    //                }
    //            }
    //            catch { }
    //            #endregion
                
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string sExMsg = string.Format("Failed to create the prospect, Exception: {0}", ex.Message);
    //        LPLog.LogMessage(LogType.Logerror, ex.Message);
    //        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_ex", "$('#divContainer').hide();alert('Failed to create the prospect.');", true);
    //        return;
    //    }

    //    #endregion

    //    // build js
    //    StringBuilder sbJavaScript = new StringBuilder();
    //    sbJavaScript.AppendLine("$('#divContainer').hide();");
    //    sbJavaScript.AppendLine("alert('Create prospect successfully.');");
    //    sbJavaScript.AppendLine("window.parent.location.href=window.parent.location.href;");
        
    //    // success
    //    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", sbJavaScript.ToString(), true);
    //}

    //private void BindLoanOfficer(string sBranchID)
    //{
    //    if (ddlLoanOfficer.Enabled == false)
    //    {
    //        return;
    //    }
    //    LPWeb.BLL.Users bllUser = new LPWeb.BLL.Users();

    //    //string strWhere = "AND RoleID=" + iLoanOfficerRoleID;
    //    //strWhere += " AND (UserId IN (SELECT UserId FROM GroupUsers WHERE GroupID IN(select GroupID from Groups where  BranchID =" + sBranchID + ")))";
    //    string strWhere = string.Format(" AND ((BranchId={0} AND (RoleName='Loan Officer' OR RoleName='Loan Officer Assistant' or RoleName='Branch Manager')) ", sBranchID);
    //    //if (CurrUser.bIsCompanyExecutive)
    //    strWhere += " OR (RoleName='Executive'))";
     
    //    DataTable dtLoadOfficer = bllUser.GetUserList(strWhere);
    //    if (!dtLoadOfficer.Columns.Contains("LoanOfficer"))
    //    {
    //        dtLoadOfficer.Columns.Add("LoanOfficer");
    //    }
    //    foreach (DataRow dr in dtLoadOfficer.Rows)
    //    {
    //        dr["LoanOfficer"] = dr["UserName"].ToString();
    //    }
    //    DataRow drNew = dtLoadOfficer.NewRow();
    //    drNew["UserID"] = -1;
    //    drNew["LoanOfficer"] = "-- select --";
    //    dtLoadOfficer.Rows.InsertAt(drNew, 0);

    //    ddlLoanOfficer.ClearSelection();
    //    ddlLoanOfficer.DataSource = dtLoadOfficer;
    //    ddlLoanOfficer.DataTextField = "LoanOfficer";
    //    ddlLoanOfficer.DataValueField = "UserID";
    //    ddlLoanOfficer.SelectedValue = "-1";
    //    ddlLoanOfficer.DataBind();
    //}

    //protected void btnCopyAddress_Click(object sender, EventArgs e)
    //{
    //    txbPropertyAddress.Text = txtAddress.Text;
    //    txbPropertyCity.Text = txtCity.Text;
    //    txbPropertyZip.Text = txtZip.Text;
    //    ddlPropState.SelectedValue = ddlState.SelectedValue;
    //}
}