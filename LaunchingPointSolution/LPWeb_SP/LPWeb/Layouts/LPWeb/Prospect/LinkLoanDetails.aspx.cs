using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using LPWeb.Common;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;

namespace LPWeb.Prospect
{
    public partial class LinkLoanDetails : BasePage
    {
        private LPWeb.BLL.Prospect prospectMgr = new BLL.Prospect();
        private LPWeb.BLL.Loans loanMgr = new BLL.Loans();
        private int iProspectID = 0;
        private string FromPage = string.Empty;
        int PageIndex = 1;

        string sCloseDialogCodes = string.Empty;
        string sRefreshCodes = string.Empty;

        #region Event
        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ////权限验证
                //var loginUser = new LoginUser();
                //if (loginUser.userRole.LoanSetup.ToString() == "")
                //{
                //    Response.Redirect("../Unauthorize.aspx");
                //    return;
                //}
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

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

            string sErrorMsg = "Failed to load current page: invalid prospect ID.";
            string sReturnPage = "ProspectDetailView.aspx";


            if (this.Request.QueryString["ProspectID"] != null) // no task id
            {
                string sProspectID = this.Request.QueryString["ProspectID"].ToString();
                if (PageCommon.IsID(sProspectID) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }

                this.iProspectID = Convert.ToInt32(sProspectID);
                this.hdProspectID.Value = this.iProspectID.ToString();
            }
            if (this.Request.QueryString["PageIndex"] != null) // PageIndex
            {
                try
                {
                    PageIndex = int.Parse(this.Request.QueryString["PageIndex"].ToString());
                }
                catch
                {
                    PageIndex = 1;
                }
            }
            else
            {
                PageIndex = AspNetPager1.CurrentPageIndex;
            }

            if (!IsPostBack)
            {
                try
                {
                    DoInitData();
                    LoadProspectData();
                    if (this.Request.QueryString["Type"] != null)
                    {
                        LoadLoanInfo();
                    }

                }
                catch (Exception ex)
                {
                    LPLog.LogMessage(LogType.Logdebug, ex.ToString());
                }
            }
        }

        /// <summary>
        /// Link button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLink_Click(object sender, EventArgs e)
        {

            string sLoanIds = this.hdLoanIds.Value;
            string sLinkType = this.hdLinkType.Value;
            int iLinkType = 1;
            if (sLoanIds.Length < 1)
            {
                return;
            }

            try
            {
                LPWeb.BLL.ContactRoles contactRoles = new LPWeb.BLL.ContactRoles();
                DataSet dsRole = contactRoles.GetList("Name='" + sLinkType + "'");
                if (dsRole.Tables.Count > 0 && dsRole.Tables[0].Rows.Count > 0)
                {
                    iLinkType = Convert.ToInt32(dsRole.Tables[0].Rows[0]["ContactRoleId"].ToString());
                }
                LPWeb.BLL.LoanContacts loanContact = new LPWeb.BLL.LoanContacts();
                foreach (string sLoanID in sLoanIds.Split(','))
                {
                    if (sLoanID.Trim() == "")
                    {
                        continue;
                    }
                    int iLoanID = 0;
                    int iOldContactID = 0;
                    if (sLoanID == "" || Int32.TryParse(sLoanID, out iLoanID) == false)
                    {
                        continue;
                    }
                    DataSet dsloancontact = loanContact.GetList("FileId=" + sLoanID + " AND ContactRoleId=" + iLinkType.ToString());
                    if(dsloancontact.Tables.Count > 0 && dsloancontact.Tables[0].Rows.Count > 0)
                    {
                        iOldContactID = Convert.ToInt32("0"+dsloancontact.Tables[0].Rows[0]["ContactId"].ToString());
                    }
                    LPWeb.Model.LoanContacts modelOld = loanContact.GetModel(iLoanID,iLinkType,iOldContactID);
                    if (modelOld == null)
                    {
                        modelOld = new Model.LoanContacts();
                        modelOld.ContactId = 0;
                    }
                    LPWeb.Model.LoanContacts model = new LPWeb.Model.LoanContacts();
                    model.FileId = iLoanID;
                    model.ContactRoleId = iLinkType;
                    model.ContactId = this.iProspectID;
                    loanContact.Reassign(modelOld, model, 0);
                    
                    ServiceManager sm = new ServiceManager();
                    using (LP2ServiceClient service = sm.StartServiceClient())
                    {
                        UpdateBorrowerRequest req = new UpdateBorrowerRequest();
                        req.hdr = new ReqHdr();
                        req.hdr.SecurityToken = "SecurityToken"; 
                        req.hdr.UserId = this.CurrUser.iUserID;
                        req.ContactId = model.ContactId;
    
                        UpdateBorrowerResponse respone = null;

                        try
                        {
                            respone = service.UpdateBorrower(req);
                        }
                        catch (System.ServiceModel.EndpointNotFoundException)
                        {
                            string sExMsg = string.Format("Exception happened when  Update loan (ContactId={0}): {1}", model.ContactId, "Point Manager is not running.");
                            LPLog.LogMessage(LogType.Logerror, sExMsg);                            
                        }
                        catch (Exception ex)
                        {
                            string sExMsg = string.Format("Exception happened when  Update loan (ContactId={0}): {1}", model.ContactId, ex.Message);
                            LPLog.LogMessage(LogType.Logerror, sExMsg);                            
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
                PageCommon.WriteJsEnd(this, "Failed to link the selected loans.", this.sCloseDialogCodes + this.sRefreshCodes);
            }

            this.hdLoanIds.Value = string.Empty;
            this.hdLinkType.Value = string.Empty;

            // success
            PageCommon.WriteJsEnd(this, "Link the selected loans successfully.", this.sCloseDialogCodes + this.sRefreshCodes);
        }
        #endregion

        #region function
        /// <summary>
        /// Init data
        /// </summary>
        private void DoInitData()
        {

            //Binding loan status
            this.ddlLoanStatus.Items.Add(new ListItem("All", ""));
            this.ddlLoanStatus.Items.Add(new ListItem("Processing", "Processing"));
            this.ddlLoanStatus.Items.Add(new ListItem("Prospect", "Prospect"));
            this.ddlLoanStatus.Items.Add(new ListItem("Canceled", "Canceled"));
            this.ddlLoanStatus.Items.Add(new ListItem("Closed", "Closed"));
            this.ddlLoanStatus.Items.Add(new ListItem("Denied", "Denied"));
            this.ddlLoanStatus.Items.Add(new ListItem("Suspended", "Suspended"));

            this.ddlLoanStatus.SelectedIndex = 0;
        }

        /// <summary>
        /// Load prospect detail info
        /// </summary>
        private void LoadProspectData()
        {
            LPWeb.Model.Prospect prospectModel = new Model.Prospect();
            try
            {
                //prospect detail info
                prospectModel = this.prospectMgr.GetModel(iProspectID);
                BLL.Contacts contactMgr = new BLL.Contacts();
                LPWeb.Model.Contacts contactModel = contactMgr.GetModel(prospectModel.Contactid);
                this.lbProspect.Text = contactModel.LastName + ", " + contactModel.FirstName + " " + contactModel.MiddleName;
                this.lbSSN.Text = (contactModel.SSN.Length > 7 ? "xxx-xx-"+contactModel.SSN.Substring(7) : contactModel.SSN);
                if (contactModel.DOB != null && Convert.ToDateTime(contactModel.DOB).Year != 1900)
                {
                    this.lbDOB.Text = Convert.ToDateTime(contactModel.DOB).ToString("MM/dd/yyyy");
                }
                this.lbAddress.Text = contactModel.MailingAddr + ", " + contactModel.MailingCity + ", " + contactModel.MailingState + ", " + contactModel.MailingZip;
                BLL.Users userMgr = new BLL.Users();
                if (prospectModel.Loanofficer != null)
                {
                    Model.Users userModel = userMgr.GetModel(Convert.ToInt32(prospectModel.Loanofficer));
                    if (userModel != null)
                    {
                        this.lbLoanOfficer.Text = userModel.LastName + ", " + userModel.FirstName;
                    }
                }
                //search info
                if (this.Request.QueryString["Type"] == null)
                {
                    if (contactModel.LastName.Trim() != "")
                    {
                        this.tbxBrwLastName.Text = contactModel.LastName;
                    }
                    if (contactModel.FirstName.Trim() != "")
                    {
                        this.tbxBrwFirstName.Text = contactModel.FirstName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 根据用户界面选择生成过滤条件
        /// </summary>
        /// <returns></returns>
        private string GenerateQueryCondition()
        {
            string strQueryCon = " 1=1 AND l.FileId NOT IN(SELECT Fileid FROM LoanContacts WHERE (ContactRoleId = 1 OR ContactRoleId = 2) AND ContactId = " + iProspectID.ToString() + ")";

            if (this.Request.QueryString["FirstName"] != null && this.Request.QueryString["FirstName"].ToString().Trim() != "")
            {
                this.tbxBrwFirstName.Text = this.Request.QueryString["FirstName"].Trim();
                strQueryCon += " AND l.FileId IN(SELECT Fileid FROM LoanContacts WHERE ContactId IN(SELECT ContactId FROM Contacts WHERE FirstName LIKE '%" + this.tbxBrwFirstName.Text.Trim().Replace("'","''") + "%'))";
            }
            if (this.Request.QueryString["LastName"] != null && this.Request.QueryString["LastName"].ToString().Trim() != "")
            {
                this.tbxBrwLastName.Text = this.Request.QueryString["LastName"].Trim();
                strQueryCon += " AND l.FileId IN(SELECT Fileid FROM LoanContacts WHERE ContactId IN(SELECT ContactId FROM Contacts WHERE LastName LIKE '%" + this.tbxBrwLastName.Text.Trim().Replace("'", "''") + "%'))";
            }
            if (this.Request.QueryString["LoanStatus"] != null && this.Request.QueryString["LoanStatus"].ToString().Trim() != "")
            {
                this.ddlLoanStatus.SelectedValue = this.Request.QueryString["LoanStatus"].Trim();
                strQueryCon += " AND l.Status = '" + this.ddlLoanStatus.SelectedValue + "'";
            }
            if (this.Request.QueryString["PropertyAddr"] != null && this.Request.QueryString["PropertyAddr"].ToString().Trim() != "")
            {
                this.tbxAddress.Text = this.Request.QueryString["PropertyAddr"].Trim();
                strQueryCon += " AND l.PropertyAddr LIKE '%" + this.tbxAddress.Text.Trim().Replace("'", "''") + "%'";
            }
            if (this.Request.QueryString["PropertyCity"] != null && this.Request.QueryString["PropertyCity"].ToString().Trim() != "")
            {
                this.tbxCity.Text = this.Request.QueryString["PropertyCity"].Trim();
                strQueryCon += " AND l.PropertyCity LIKE '%" + this.tbxCity.Text.Trim().Replace("'", "''") + "%'";
            }
            if (this.Request.QueryString["PropertyZip"] != null && this.Request.QueryString["PropertyZip"].ToString().Trim() != "")
            {
                this.tbxZip.Text = this.Request.QueryString["PropertyZip"].Trim();
                strQueryCon += " AND l.PropertyZip LIKE '%" + this.tbxZip.Text.Trim().Replace("'", "''") + "%'";
            }
            if (this.Request.QueryString["PropertyState"] != null && this.Request.QueryString["PropertyState"].ToString().Trim() != "")
            {
                this.ddlState.SelectedValue = this.Request.QueryString["PropertyState"].Trim();
                strQueryCon += " AND l.PropertyState = '" + this.ddlState.SelectedValue + "'";
            }

            return strQueryCon;
        }

        /// <summary>
        /// Bind Grid
        /// </summary>
        private void LoadLoanInfo()
        {
            int pageSize = AspNetPager1.PageSize;
            int pageIndex = PageIndex;
            string queryCondition = GenerateQueryCondition();

            int recordCount = 0;

            DataSet dsLists = null;
            DataTable dtList = null;
            try
            {
                dsLists = this.loanMgr.GetLoanDetailByLinkLoans(pageSize, pageIndex, queryCondition, out recordCount, "fileName", 0);
                dtList = dsLists.Tables[0];
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            this.gvLinkLoanList.DataSource = dtList;
            gvLinkLoanList.DataBind();

        }
        #endregion
    }
}
