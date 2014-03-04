using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.BLL;
using Utilities;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;


public partial class LoanReassignContactTab : BasePage
{
    int iLoanID = 0;
    LoginUser CurrentUser;
    private readonly LPWeb.BLL.LoanContacts contacts = new LoanContacts();
    private string sErrorMsg = "Failed to load current page: invalid FileID.";
    private string sReturnPage = "LoanReassignContactTab.aspx";
    int oldContactID = 0;
    int oldRoleID = 0;
    #region Properties

    /// <summary>
    /// Gets or sets the current file id.
    /// </summary>
    /// <value>The current file id.</value>
    protected int CurrentFileId
    {
        set
        {
            hfdFileId.Value = value.ToString();
            ViewState["fileId"] = value;
        }
        get
        {
            if (ViewState["fileId"] == null)
                return 0;
            int fileId = 0;
            int.TryParse(ViewState["fileId"].ToString(), out fileId);

            return fileId;
        }
    }

    /// <summary>
    /// Gets or sets the grid view sort direction.
    /// </summary>
    /// <value>The grid view sort direction.</value>
    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;
            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    /// <summary>
    /// Gets or sets the name of the order.
    /// </summary>
    /// <value>The name of the order.</value>
    public string OrderName
    {
        get
        {
            if (ViewState["orderName"] == null)
                ViewState["orderName"] = "ContactsName";
            return Convert.ToString(ViewState["orderName"]);
        }
        set { ViewState["orderName"] = value; }
    }

    /// <summary>
    /// Gets or sets the type of the order.
    /// </summary>
    /// <value>The type of the order.</value>
    public int OrderType
    {
        get
        {
            if (ViewState["orderType"] == null)
                ViewState["orderType"] = 0;
            return Convert.ToInt32(ViewState["orderType"]);
        }
        set { ViewState["orderType"] = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CurrentUser = new LoginUser();
            //权限验证
            if (CurrentUser.userRole.ContactMgmt.ToString().IndexOf('4') == -1)
            {
                Response.Redirect("../Unauthorize1.aspx");
                return;
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }


        if (Request.QueryString["FileID"] != null) // 如果有FileID
        {
            string sFileID = Request.QueryString["FileID"];

            if (PageCommon.IsID(sFileID) == false)
            {
                //PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
            }

            CurrentFileId = Convert.ToInt32(sFileID);
        }

        if (Request.QueryString["uandr"] != null) // 如果有userid and roleid
        {
            string sUandRID = Request.QueryString["uandr"].Replace("Contract", "");
            try
            {
                oldContactID = int.Parse(sUandRID.Split("_".ToCharArray())[0]);
                oldRoleID = int.Parse(sUandRID.Split("_".ToCharArray())[1]);
            }
            catch
            { }
        }
        if (!IsPostBack)
        {
            BindDDL();
            Contacts contact = new Contacts();
            lbBorrower.Text = contact.GetBorrower(CurrentFileId);
            BindGrid(1);

            this.hdnCloseDialogCodes.Value = "";
            this.hdnCloseDialogCodes.Value = this.Request.QueryString["CloseDialogCodes"];
        }
    }

    private void BindDDL()
    {
        try
        {
            ServiceTypes type = new ServiceTypes();
            DataSet ds = type.GetList(" Enabled=1 ORDER BY ServiceType ASC");
            ddlServiceTypes.DataTextField = "ServiceType";
            ddlServiceTypes.DataValueField = "ServiceType";
            ddlServiceTypes.DataSource = ds;
            ddlServiceTypes.DataBind();

            ddlServiceTypes.Items.Insert(0, new ListItem("All", "0"));
        }
        catch
        { }

        try
        {
            ContactRoles roles = new ContactRoles();
            DataSet ds = roles.GetList(" Name <> 'Borrower' AND Name <> 'CoBorrower' ORDER BY [Name] ASC");
            ddlContactRole.DataValueField = "ContactRoleID";
            ddlContactRole.DataTextField = "Name";
            ddlContactRole.DataSource = ds;
            ddlContactRole.DataBind();
        }
        catch
        { }

        try
        {
            LPWeb.Model.Loans model = new LPWeb.Model.Loans();
            Loans loans = new Loans();
            model = loans.GetModel(CurrentFileId);
            if (model == null)
            { return; }

            //lbPointFile.Text
            lbProperty.Text = model.PropertyAddr + " " + model.PropertyCity + " " + model.PropertyState + " " + model.PropertyZip;
        }
        catch
        {

        }
    }

    private void BindGrid(int pageIndex)
    {
        //if (CurrentFileId < 1)
        //{
        //    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
        //    return;
        //}

        int pageSize = AspNetPager1.PageSize;
        int recordCount = 0;
        string strWhere = string.Empty;
        //string strWhere = " (FileId <> '" + CurrentFileId.ToString() + "' OR FileId IS NULL )";
        strWhere = " (1=1) ";
        if (ddlServiceTypes.SelectedIndex > 0)
        {
            strWhere += " AND ServiceTypes = '" + ddlServiceTypes.SelectedItem.Text + "'";
        }
        DataSet ds = contacts.GetDistinctLoanContactsForReassign(pageSize, pageIndex, strWhere, out recordCount, OrderName, OrderType);
        //DataSet ds = contacts.GetLoanContactsReassign(pageSize, pageIndex, "", out recordCount, OrderName, OrderType);

        AspNetPager1.RecordCount = recordCount;
        AspNetPager1.CurrentPageIndex = pageIndex;
        AspNetPager1.PageSize = pageSize;

        gvContacts.DataSource = ds;
        gvContacts.DataBind();
    }

    private string GetCompanyName(int CompanyID)
    {
        string CompanyName = string.Empty;

        try
        {
            ContactCompanies cc = new ContactCompanies();
            DataSet ds = cc.GetList(" ContactCompanyId = '" + CompanyID.ToString() + "' ");
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                return string.Empty;
            }

            CompanyName = ds.Tables[0].Rows[0]["Name"].ToString();
        }
        catch
        { }

        return CompanyName;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.CurrentFileId == 0)
        {
            return;
        }
        int ContactID = 0;
        LPWeb.Model.Contacts model = new LPWeb.Model.Contacts();
        try
        {
            ContactID = int.Parse(hfdContactID.Value);
            Contacts contact = new Contacts();
            model = contact.GetModel(ContactID);
        }
        catch
        { }

        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            ReassignContactRequest req = new ReassignContactRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = CurrentUser.iUserID;
            ReassignContactInfo cInfo = new ReassignContactInfo();
            List<ReassignContactInfo> cList = new List<ReassignContactInfo>();
            cInfo.FileId = this.CurrentFileId;//todo:check dummy data
            cInfo.ContactRoleId = int.Parse(ddlContactRole.SelectedValue);
            cInfo.NewContactId = ContactID;
            cList.Add(cInfo);
            req.reassignContacts = cList.ToArray();
            ReassignContactResponse respone = null;
            try
            {
                respone = service.ReassignContact(req);
                if (respone.hdr.Successful)
                {
                    LPWeb.Model.LoanContacts lcModel = new LPWeb.Model.LoanContacts();
                    lcModel.FileId = CurrentFileId;
                    lcModel.ContactRoleId = cInfo.ContactRoleId;
                    lcModel.ContactId = ContactID;

                    LPWeb.Model.LoanContacts oldlcModel = new LPWeb.Model.LoanContacts();
                    oldlcModel.FileId = CurrentFileId;
                    oldlcModel.ContactRoleId = oldRoleID;
                    oldlcModel.ContactId = oldContactID;

                    LPWeb.BLL.LoanContacts lc = new LoanContacts();

                    lc.Reassign(oldlcModel, lcModel, req.hdr.UserId);
                    try
                    {
                        PageCommon.WriteJsEnd(this, "Reassigned contact successfully", PageCommon.Js_RefreshParent);
                    }
                    catch
                    { }
                }
                else
                {
                    try
                    {
                        PageCommon.WriteJsEnd(this, String.Format("Failed to reassign contact: reason: {0}.", respone.hdr.StatusInfo), PageCommon.Js_RefreshSelf);
                    }
                    catch
                    { }
                }

            }
            catch (System.ServiceModel.EndpointNotFoundException ex)
            {
                LPLog.LogMessage(ex.Message);
                PageCommon.WriteJsEnd(this, "Failed to reassign contact: reason, Point Manager is not running. ", PageCommon.Js_RefreshSelf);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
                PageCommon.WriteJsEnd(this, String.Format("Failed to reassign contact, reason: {0}", exception.Message), PageCommon.Js_RefreshSelf);
            }
        }
    }

    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        BindGrid(1);
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        BindGrid(AspNetPager1.CurrentPageIndex);
    }

    protected void gvContacts_Sorting(object sender, GridViewSortEventArgs e)
    {

        OrderName = e.SortExpression;
        string sortExpression = e.SortExpression;
        if (GridViewSortDirection == SortDirection.Ascending) //设置排序方向
        {
            GridViewSortDirection = SortDirection.Descending;
            OrderType = 0;
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            OrderType = 1;
        }
        BindGrid(AspNetPager1.CurrentPageIndex);
    }
}