using System;
using System.Data;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.LP_Service;
using Microsoft.SharePoint.WebControls;
using Utilities;
using LoanActivities = LPWeb.Model.LoanActivities;
using LPWeb.Layouts.LPWeb.Common;

public partial class LoanDetails_WorkflowSetupTab : BasePage
{
    private readonly LoanWflTempl bllWflTemp = new LoanWflTempl();
    private string sErrorMsg = "Failed to load current page: invalid FileID.";
    private string sReturnPage = "WorkflowSetupTab.aspx";
    private string sLoanStatus = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            var loginUser = new LoginUser();
            this.hdnLoginUserID.Value = loginUser.iUserID.ToString();
            //loginUser.ValidatePageVisitPermission("LoanSetup");
            //权限验证
            if (!loginUser.userRole.ApplyWorkflow)
            {
                Response.Redirect("../Unauthorize1.aspx");
                return;
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }


        if (Request.QueryString["FileID"] != null) // 如果有GroupID
        {
            string sFileID = Request.QueryString["FileID"];

            if (PageCommon.IsID(sFileID) == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
            }

            CurrentFileId = Convert.ToInt32(sFileID);
        }
        if (CurrentFileId == 0)
        {
            return;
        }

        if (!IsPostBack)
        {
            BindPage(1);
        }
    }

    private void BindPage(int pageIndex)
    {
        if (CurrentFileId < 1)
        {
            PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
            return;
        }

        BindDropdownList();

        int pageSize = 20;
        int recordCount = 0;
        DataSet ds = bllWflTemp.GetWorkflowSetupList(pageSize, pageIndex,
                                                     " TemplTaskId IS NULL AND FileId=" + CurrentFileId,
                                                     out recordCount, OrderName, OrderType);

        AspNetPager1.RecordCount = recordCount;
        AspNetPager1.CurrentPageIndex = pageIndex;
        AspNetPager1.PageSize = pageSize;

        gvWfGrid.DataSource = ds;
        gvWfGrid.DataBind();

    }

    private void BindDropdownList()
    {
        object obj = LPWeb.DAL.DbHelperSQL.GetSingle("Select [Status] from Loans where FileId=" + CurrentFileId);
        sLoanStatus = obj == null ? string.Empty : obj.ToString().ToUpper().Trim();

        //add by shawn ,for prospect
        string strWhere = " Enabled=1 ";
        if (sLoanStatus == "PROCESSING")
        {
            strWhere += " AND WorkflowType = 'Processing' ";
        }
        else if (sLoanStatus == "PROSPECT")
        {
            strWhere += " AND WorkflowType = 'Prospect' ";
        }

        DataSet dsWflTemps = new Template_Workflow().GetList(strWhere);

        ddlWfTemps.DataTextField = "Name";
        ddlWfTemps.DataValueField = "WflTemplId";
        ddlWfTemps.DataSource = dsWflTemps;
        ddlWfTemps.DataBind();

        ddlWfTemps.Items.Insert(0, new ListItem("— select a workflow template –", "-1"));

        var models = bllWflTemp.GetModelList(string.Format("FileId={0}", CurrentFileId));
        lblWflTemplApplied.Text = string.Empty;
        lblWflAppliedDate.Text = string.Empty;
        lblWflAppliedBy.Text = string.Empty;
        if (models == null || models.Count == 0)
        {
            ddlWfTemps.Items[0].Selected = true;
            return;
        }
        if (models[0].WflTemplId > 0)
        {
            object ob1 = LPWeb.DAL.DbHelperSQL.GetSingle(string.Format("Select [Name] from Template_Workflow where WflTemplId={0} ", models[0].WflTemplId));
            lblWflTemplApplied.Text = ob1 == null ? string.Empty : ob1.ToString();
        }
        lblWflAppliedDate.Text = models[0].ApplyDate == DateTime.MinValue ? string.Empty : models[0].ApplyDate.ToString();
        if (models[0].ApplyBy > 0)
        {
            object ob = LPWeb.DAL.DbHelperSQL.GetSingle(string.Format("Select dbo.lpfn_GetUserName({0}) ", models[0].ApplyBy));
            lblWflAppliedBy.Text = ob == null ? string.Empty : ob.ToString();
        }
        int idx = 0;

        foreach (ListItem item in ddlWfTemps.Items)
        {
            if (item.Value == models[0].WflTemplId.ToString())
            {
                item.Selected = true;
                ddlWfTemps.Items[idx].Selected = true;
                return;
            }
            idx = idx + 1;
        }
    }

    private void ValidateButtonPermission()
    {
        var loginUser = new LoginUser();
        //var loan = new Loans().GetModel(CurrentFileId);

        if (!loginUser.userRole.ApplyWorkflow || (sLoanStatus != "Processing" && sLoanStatus != "Prospect"))
        {
            btnApplyWfl.Enabled = false;
        }

        if (!loginUser.userRole.CustomTask.HasValue || (sLoanStatus != "Processing" && sLoanStatus != "Prospect"))
        {
            btnNew.Disabled = true;
            btnUpdate.Disabled = true;
            btnRemoveWfl.Disabled = true;
            btnNew.HRef = "";
            btnUpdate.HRef = "";
            btnRemoveWfl.HRef = "";
        }
        else
        {
            if (loginUser.userRole.CustomTask.Value.ToString().Contains("1"))
            {
                btnNew.Disabled = false;
            }
            else
            {
                btnNew.Disabled = true;
                btnNew.HRef = "";
            }

            if (loginUser.userRole.CustomTask.Value.ToString().Contains("2"))
            {
                btnUpdate.Disabled = false;
            }
            else
            {
                btnUpdate.Disabled = true;
                btnUpdate.HRef = "";
            }
            if (loginUser.userRole.CustomTask.Value.ToString().Contains("3"))
            {
                btnRemoveWfl.Disabled = false;
            }
            else
            {
                btnRemoveWfl.Disabled = true;
                btnRemoveWfl.HRef = "";
            }
        }

        if (sLoanStatus != "Processing" && sLoanStatus.ToUpper().Trim() != "PROSPECT")
        {
            btnApplyWfl.Enabled = false;
            btnNew.Disabled = true;
            btnRemoveWfl.Disabled = true;
            btnUpdate.Disabled = true;
        }
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        BindPage(AspNetPager1.CurrentPageIndex);
    }

    protected void gvWfGrid_Sorting(object sender, GridViewSortEventArgs e)
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
        BindPage(AspNetPager1.CurrentPageIndex);
    }


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
                ViewState["orderName"] = "Name";
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

    protected void btnApplyWfl_Click(object sender, EventArgs e)
    {
        int iWorkflowTemplID = Convert.ToInt32(this.ddlWfTemps.SelectedValue);
        //string sJsCodes = "window.parent.document.getElementById('ifrLoanInfo').contentWindow.location.reload();window.location.href=window.location.href;";
        string sJsCodes = "window.location.href=window.location.href;";

        var request = new GenerateWorkflowRequest
        {
            FileId = CurrentFileId,
            WorkflowTemplId = iWorkflowTemplID,
            hdr = new ReqHdr { UserId = new LoginUser().iUserID }
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
            string sExMsg = string.Format("Failed to apply workflow template: {0}", "Workflow Manager is not running.", iWorkflowTemplID);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, sJsCodes);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Failed to apply workflow template: {0}, error detail:", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            PageCommon.WriteJsEnd(this, sExMsg, sJsCodes);
        }

        if (response.hdr.Successful == false)
        {
            PageCommon.WriteJsEnd(this, "Failed to apply workflow template: " + response.hdr.StatusInfo, sJsCodes);
        }
        System.Threading.Thread.Sleep(2000);
        var CurrentUserId = this.CurrUser.iUserID;
        LPWeb.Model.LoanWflTempl lWflTempModel = new LPWeb.Model.LoanWflTempl();
        lWflTempModel.FileId = CurrentFileId;
        lWflTempModel.ApplyBy = CurrentUserId;
        lWflTempModel.ApplyDate = DateTime.Now;
        lWflTempModel.WflTemplId = iWorkflowTemplID;
        LPWeb.BLL.LoanWflTempl bLoanWflTempl = new LoanWflTempl();
        bLoanWflTempl.Apply(lWflTempModel);

        // success
        PageCommon.WriteJsEnd(this, "Applied workflow template successfully.", sJsCodes);
    }
}