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
using System.Web.UI;


public partial class LoanMarketingTab : BasePage
{
    #region Fields
    string sFileID = "0";
    private bool isReset = false;
    protected string sLoanType = "";
    public string FromURL = string.Empty;
    private LoanMarketing _bLoanMarketing = new LoanMarketing();

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
        set
        {
            ViewState["sortDirection"] = value;
        }

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
                ViewState["orderName"] = "CampaignName";// from "CampaignName" to "CategoryName"
            return Convert.ToString(ViewState["orderName"]);
        }
        set
        {
            ViewState["orderName"] = value;
        }
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
        set
        {
            ViewState["orderType"] = value;
        }
    }
    #endregion

    #region Event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Url != null)
        {
            FromURL = Request.Url.ToString();
        }
        sFileID = Request.QueryString["FileID"] == null ? "0" : Request.QueryString["FileID"].ToString();
        //User Right
        this.btnNew.Disabled = CurrUser.userRole.Marketing.ToString().IndexOf("1") > -1 ? false : true;
        this.btnRemove.Enabled = CurrUser.userRole.Marketing.ToString().IndexOf("2") > -1 ? true : false;
        if (!IsPostBack)
        {
            BindFilterData();
            //BindContextMenu();
            BindGrid();
            this.hfFileID.Value = sFileID;

            LPWeb.BLL.Company_General bllcompanyGeneral = new Company_General();

            var modelCompanyGen = bllcompanyGeneral.GetModel();
            if (modelCompanyGen != null && (modelCompanyGen.EnableMarketing == false || modelCompanyGen.StartMarketingSync == false))
            {
                btnNew.Disabled = true;
                //btnNew.HRef = "javascript:;";
                btnNew.Attributes["onclick"] = "";
                btnNew.Attributes["style"] = "";
            }
            else
            {
                btnNew.Disabled = false;
            }
        }
    }

    /// <summary>
    /// Set selected row when click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chk = (CheckBox)e.Row.FindControl("chkAuto");
            TextBox tb = (TextBox)e.Row.FindControl("tbAuto");
            if (tb.Text == "Auto")
            {
                chk.Checked = true;
            }
            else
            {
                chk.Checked = false;
            }


            CheckBox chkCompleted = (CheckBox)e.Row.FindControl("chkCompleted");
            TextBox tbCompleted = (TextBox)e.Row.FindControl("tbCompleted");

            chkCompleted.Enabled = false;
            if (tbCompleted.Text == "" || tbCompleted.Text == "0" || tbCompleted.Text.ToLower() == "false")
            {
                chkCompleted.Checked = false;
                if (tbCompleted.ToolTip.ToLower() == "call")
                {
                    chkCompleted.Enabled = true;
                }
            }
            else
            {
                chkCompleted.Checked = true;
              
            }

            
        }
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    /// <summary>
    /// Handles the Click event of the btnRemove control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        var selctedStr = this.hfDeleteItems.Value;
        if (!string.IsNullOrEmpty(selctedStr))
        {
            string[] selectedItems = selctedStr.Split(',');
            //delete the selected items
            DeleteMarketings(selectedItems);
            //reload the grid data
            PageCommon.WriteJsEnd(this, "Campaign has been removed successfully.", PageCommon.Js_RefreshSelf);
        }
        this.hfDeleteItems.Value = "";
    }

    protected void btnSaveSel_Click(object sender, EventArgs e)
    {
        var selctedStr = this.hfSelCampaigns.Value;
        var sStartDate = this.hfStartDate.Value;
        if (!string.IsNullOrEmpty(selctedStr))
        {
            string[] selectedItems = selctedStr.Split(',');
            //delete the selected items

            string strErrorCode = "";
            string sRst = string.Empty;
            try
            {
                // 请大家不要使用 System.Transactions.TransactionScope, 它导致Service崩溃.
                // using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                // {
                    SaveMarketings(selectedItems, sStartDate);
                    sRst = StartCampaign(Convert.ToInt32(selctedStr), Convert.ToInt32(this.hfFileID.Value), ref strErrorCode);
                    if (string.IsNullOrEmpty(sRst))
                    {
                        //ts.Complete();
                    }
                    else
                    {
                        throw new Exception(sRst);
                    }                    
                //}
            }
            catch { }

            if (sRst != "")
            {
                if ("1219" == strErrorCode)
                {
                    CallClientFn("showUpdateCardWin();");
                    BindGrid();
                }
                else
                {
                    //reload the grid data
                    PageCommon.WriteJsEnd(this, sRst, PageCommon.Js_RefreshSelf);
                }
            }
            else
            {
                //reload the grid data
                //PageCommon.WriteJsEnd(this, "Campaign has been added successfully.", PageCommon.Js_RefreshSelf);

                Response.Write("<script type='text/javascript' language='javascript'>" + PageCommon.Js_RefreshSelf + "</script>");
            }
        }
        this.hfSelCampaigns.Value = "";
    }

    protected void chkCompleted_Click(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        if (!string.IsNullOrEmpty(chk.ToolTip))
        {
            int eventId = Convert.ToInt32(chk.ToolTip);

            LPWeb.BLL.LoanMarketingEvents bll = new LoanMarketingEvents();

            LPWeb.Model.LoanMarketingEvents modalLME = bll.GetModel(eventId);

            modalLME.Completed = chk.Checked;

            bll.Update(modalLME);

            //later  : send a CompleteCampaignEventRequest to Marketing Manager
        }

        //PageCommon.WriteJsEnd(this, chk.ToolTip, PageCommon.Js_RefreshSelf);
        BindGrid();
    }

    /// <summary>
    /// Handles the Sorting event of the gridList control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
    protected void gridList_Sorting(object sender, GridViewSortEventArgs e)
    {
        OrderName = e.SortExpression;
        string sortExpression = e.SortExpression;
        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            OrderType = 0;
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            OrderType = 1;
        }
        BindGrid();
    }
    #endregion

    #region Function
    private void BindContextMenu()
    {
        //DataTable dtAddSource = _bLoanMarketing.GetLoaTypeInfoByFileID(Convert.ToInt32(sFileID));
        //foreach (DataRow drAdd in dtAddSource.Rows)
        //{
        //    sLoanType += "<li id='" + drAdd["FileId"].ToString() + "'><a href=javascript:PopupAddMarketingWindow('" + drAdd["StatusName"].ToString().Replace(" ", "") + "'," + drAdd["FileId"].ToString() + ");><span>" + drAdd["NoteTypeName"].ToString() + "</span></a></li>";
        //}
    }

    private void SaveMarketings(string[] selectedItems, string sStartDate)
    {
        int iItem = 0;
        string sFileID = "0";
        sFileID = this.hfFileID.Value;
        DateTime dtStart = DateTime.Now;
        if(!DateTime.TryParse(sStartDate,out dtStart))
        {
            dtStart = DateTime.Now;
        }

        foreach (var item in selectedItems)
        {
            if (int.TryParse(item, out iItem))
            {
                try
                {
                    LPWeb.Model.LoanMarketing _mLoanMarketing = new LPWeb.Model.LoanMarketing();
                    _mLoanMarketing.CampaignId = iItem;
                    _mLoanMarketing.Selected = DateTime.Now;
                    _mLoanMarketing.SelectedBy = CurrUser.iUserID;
                    _mLoanMarketing.FileId = Convert.ToInt32(sFileID);
                    _mLoanMarketing.Started = dtStart;
                    _mLoanMarketing.StartedBy = CurrUser.iUserID;
                    _mLoanMarketing.Status = "Active";
                    _mLoanMarketing.Type = "Manual";
                    _bLoanMarketing.Add(_mLoanMarketing);
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }
            }
        }
    }

    private void BindFilterData()
    {
        MarketingCampaigns _bMarketingCampaigns = new MarketingCampaigns();
        DataSet dsCampaign = _bMarketingCampaigns.GetList("1>0");
        DataTable dtCampaign = new DataTable();
        dtCampaign.Columns.Add("CampaignName");
        DataRow dr;
        foreach (DataRow drCp in dsCampaign.Tables[0].Rows)
        {
            if (dtCampaign.Select("CampaignName='" + drCp["CampaignName"].ToString() + "'").Length == 0)
            {
                dr = dtCampaign.NewRow();
                dr["CampaignName"] = drCp["CampaignName"].ToString();
                dtCampaign.Rows.Add(dr);
            }
        }
        DataView dv = dtCampaign.DefaultView;
        dv.Sort = "CampaignName";
        DataTable dtAllCampaigns = dv.Table;
        dr = dtAllCampaigns.NewRow();
        dr["CampaignName"] = "All Campaigns";
        dtAllCampaigns.Rows.InsertAt(dr, 0);

        ddlCampaigns.DataSource = dtAllCampaigns;
        ddlCampaigns.DataBind();

        DataTable dtStartedBy = _bLoanMarketing.GetDisStartByInfo();
        DataRow drStartedBy = dtStartedBy.NewRow();
        drStartedBy["UserId"] = 0;
        drStartedBy["StartedByUser"] = "Started By";
        dtStartedBy.Rows.InsertAt(drStartedBy, 0);
        ddlStartedBy.DataSource = dtStartedBy;
        ddlStartedBy.DataBind();
    }

    /// <summary>
    /// Bind Grid
    /// </summary>
    private void BindGrid()
    {

        int pageIndex = 1;
        int pageSize = AspNetPager1.PageSize;
        if (AspNetPager1.CurrentPageIndex > 0 && isReset == false)
            pageIndex = AspNetPager1.CurrentPageIndex;

        string queryCondition = GenOrgQueryCondition();

        int recordCount = 0;

        DataSet LoanMarketingLists = null;
        try
        {
            LoanMarketingLists = _bLoanMarketing.GetLoanMarketingByFileID(pageSize, pageIndex, queryCondition, out recordCount, OrderName, OrderType, sFileID);
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

        AspNetPager1.PageSize = pageSize;
        AspNetPager1.RecordCount = recordCount;

        gridList.DataSource = LoanMarketingLists;
        gridList.DataBind();


    }

    /// <summary>
    /// Query Condition
    /// </summary>
    /// <returns></returns>
    private string GenOrgQueryCondition()
    {

        string strWhere = " 1>0";
        if (this.ddlCampaigns.SelectedIndex > 0)
        {
            strWhere += " AND CampaignName like '%" + this.ddlCampaigns.SelectedValue + "%'";
        }


        if (this.ddlStatuses.SelectedValue != "-1" && this.ddlStatuses.SelectedValue != "0")
        {
            strWhere += " AND Status like '%" + this.ddlStatuses.SelectedValue + "%'";
        }

        if (this.ddlStartedBy.SelectedValue != "-1" && this.ddlStartedBy.SelectedValue != "0")
        {
            strWhere += " AND StartedBy like '%" + this.ddlStartedBy.SelectedValue + "%'";
        }
        if (tbSentStart.Text.Trim().Length > 0)
        {
            try
            {
                DateTime.Parse(tbSentStart.Text.Trim()).ToShortDateString();
                strWhere += " AND Started >= '" + DateTime.Parse(tbSentStart.Text.Trim()).ToShortDateString() + "'";
            }
            catch
            { }
        }

        if (tbSentEnd.Text.Trim().Length > 0)
        {
            try
            {
                DateTime.Parse(tbSentEnd.Text.Trim()).ToShortDateString();
                strWhere += " AND Started < '" + DateTime.Parse(tbSentEnd.Text.Trim()).AddDays(1).ToShortDateString() + "'";
            }
            catch
            { }
        }

        if (ddlEvents.SelectedValue.Trim() != "-1")
        {
            strWhere += " AND Completed = " + this.ddlEvents.SelectedValue.Trim();
        }
        return strWhere;
    }

    /// <summary>
    /// Deletes the LoanMarketings.
    /// </summary>
    /// <param name="items">The items.</param>
    private void DeleteMarketings(string[] items)
    {
        int iItem = 0;
        foreach (var item in items)
        {
            if (int.TryParse(item, out iItem))
            {
                try
                {
                    _bLoanMarketing.Delete(iItem);
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }
            }
        }
    
    
    }

    private string StartCampaign(int iItem, int iFileID, ref string strErrorCode)
    {
        string ReturnMessage = string.Empty;
        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            StartCampaignRequest req = new StartCampaignRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = this.CurrUser.iUserID;
            req.CampaignId = iItem;
            req.FileId = new int[1] { iFileID };

            StartCampaignResponse respone = null;
            try
            {
                respone = service.StartCampaign(req);
                if (respone.hdr.Successful)
                {
                    ReturnMessage = "";
                }
                else
                {
                    strErrorCode = respone.ErrorCode;
                    ReturnMessage = respone.hdr.StatusInfo;
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                string sExMsg = string.Format("Exception happened when StartCampaign (FileID={0}): {1}", iFileID, "Marketing Manager is not running.");
                LPLog.LogMessage(LogType.Logerror, sExMsg);
                ReturnMessage = sExMsg;
            }
            catch (Exception ex)
            {
                string sExMsg = string.Format("Exception happened when StartCampaign (FileID={0}): {1}", iFileID, ex.Message);
                LPLog.LogMessage(LogType.Logerror, sExMsg);
                ReturnMessage = sExMsg;
            }

            return ReturnMessage;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Action"></param>
    /// <returns></returns>
    public string GetEventImg(object Action)
    {
        string img = string.Empty;
        string strAction = string.Format("{0}", Action);

        if ("CALL" == strAction.ToUpper())
        {
            img = "<img src='../images/marketing/Action_Call.jpg' alt='icon' height='14' />";
        }
        else if ("EMAIL" == strAction.ToUpper())
        {
            img = "<img src='../images/marketing/Action_Email.jpg' alt='icon' height='14' />";
        }
        else if (strAction.ToUpper().Contains("MAIL"))
        {
            img = "<img src='../images/marketing/Action_Mail.jpg' alt='icon' height='14' />";
        }
        return img;
    }

    public string IsShow(object obj)
    {
        if (obj != null && obj.ToString() != "")
        {
            return "";
        }
        return "none";
    }

    /// <summary>
    /// Register client javascript function
    /// </summary>
    /// <param name="strScript"></param>
    private void CallClientFn(string strScript)
    {
        ClientScriptManager csm = this.Page.ClientScript;
        string strKey = "clientcallback";
        if (!csm.IsStartupScriptRegistered(this.GetType(), strKey))
        {
            csm.RegisterStartupScript(this.GetType(), strKey, strScript, true);
        }
    }
    #endregion
}

