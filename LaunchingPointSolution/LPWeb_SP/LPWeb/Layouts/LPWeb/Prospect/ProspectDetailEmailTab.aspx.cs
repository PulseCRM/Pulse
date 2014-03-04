using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Web.UI.WebControls;
using System.Text;
using Utilities;
using System.Data;
using System.Text.RegularExpressions;
using LPWeb.Common;
using LPWeb.BLL;

public partial class Prospect_ProspectDetailEmailTab : BasePage
{

    protected string sHasSendRight = "0";
    private LoginUser _curLoginUser = new LoginUser();

    EmailLog emailLogManager = new EmailLog();
    private bool isReset = false;

    private string sProspectID = "";
    private string sFileId = "";
    private string str_Where = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        bool bIsValid = PageCommon.ValidateQueryString(this, "ProspectID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Invalid prospect id.", "");
        }
        this.sProspectID = Request.QueryString["ProspectID"].ToString();

        //是否有Send权限
        sHasSendRight = _curLoginUser.userRole.Prospect.ToString().IndexOf('O') > 0 ? "1" : "0";  //Send  

        string sSql = "select distinct a.FileId as EmailTypeID, case when a.Status='Prospect' then 'Lead' when a.Status='Processing' then 'Active Loan' else a.Status+' Loan' end+'-'+a.LienPosition+'-'+a.PropertyAddr+' Emails' as EmailType "
                    + "from Loans as a inner join LoanContacts as b on a.FileId= b.FileId "
                    + "where b.ContactId=" + this.sProspectID + " and (b.ContactRoleId=1 or b.ContactRoleId=2)";

        DataTable EmailTypeList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        #region Build Context Menu Items
        
        StringBuilder sbMenuItems = new StringBuilder();

        str_Where = string.Format(" AND ( ProspectId={0} ",  this.sProspectID);

       
        string strw = "";

        foreach (DataRow EmailTypeRow in EmailTypeList.Rows)
	    {
            sFileId = EmailTypeRow["EmailTypeID"].ToString();
            strw = string.Format(" OR FileId='{0}' ", sFileId);
            str_Where = str_Where + strw;

            string sLoanID = EmailTypeRow["EmailTypeID"].ToString();
            string sLoanName = EmailTypeRow["EmailType"].ToString();

            sbMenuItems.AppendLine("<li><a href=\"#" + sLoanID + "\">" + sLoanName + "</a></li>");
	    }

        str_Where = str_Where + " ) ";       

        this.ltrContextMenuItems.Text = sbMenuItems.ToString();

        #endregion

        if (!IsPostBack)
        {
            #region 加载Email Type Filter

            DataRow NewEmailTypeRow = EmailTypeList.NewRow();
            NewEmailTypeRow["EmailTypeID"] = DBNull.Value;
            NewEmailTypeRow["EmailType"] = "All Emails";
            EmailTypeList.Rows.InsertAt(NewEmailTypeRow, 0);

            NewEmailTypeRow = EmailTypeList.NewRow();
            NewEmailTypeRow["EmailTypeID"] = 0;
            NewEmailTypeRow["EmailType"] = "Prospect Emails";
            EmailTypeList.Rows.InsertAt(NewEmailTypeRow, 1);

            this.ddlEmailTypeFilter.DataSource = EmailTypeList;
            this.ddlEmailTypeFilter.DataBind();


            #endregion

            BindGrid();
        }
    }

    /// <summary>
    /// Bind prospect loan gridview
    /// </summary>
    private void BindGrid()
    {
        int pageSize = AspNetPager1.PageSize;
        int pageIndex = 1;

        if (isReset == true)
            pageIndex = AspNetPager1.CurrentPageIndex = 1;
        else
            pageIndex = AspNetPager1.CurrentPageIndex;

        string sDbTable = "(select EmailLogId, LastSent, DateTimeReceived, 'Prospect Email' as EmailType, '' as LoanFile, "
                        + "dbo.lpfn_GetEmailLogFromUser(EmailLogId) as FromUserName, "
                        + "dbo.lpfn_GetEmailLogFirstToUser(EmailLogId) as ToUserName, "
                        + "Subject, EmailBody, null as LoanID,dbo.lpfn_GetEmailLog_AttachmentsByEmailLogId(EmailLogId) as Attachments from EmailLog where ProspectId=" + this.sProspectID + " "
                        + "union "
                        + "select a.EmailLogId, a.LastSent, a.DateTimeReceived, 'Prospect Email' as EmailType, '' as LoanFile, "
                        + "dbo.lpfn_GetEmailLogFromUser(a.EmailLogId) as FromUserName, "
                        + "dbo.lpfn_GetEmailLogFirstToUser(a.EmailLogId) as ToUserName, "
                        + "a.Subject, a.EmailBody, null as LoanID,dbo.lpfn_GetEmailLog_AttachmentsByEmailLogId(EmailLogId) as Attachments from EmailLog as a "
                        + "inner join ProspectAlerts as b on a.ProspectAlertId=b.ProspectAlertId "
                        + "inner join Prospect as c on b.ContactId=c.Contactid "
                        + "where c.Contactid=" + this.sProspectID + " "
                        + "union "
                        + "select a.EmailLogId, a.LastSent, a.DateTimeReceived, "
                        + "case when b.Status='Prospect' then 'Lead Email' when b.Status='Processing' then 'Active Loan Email' else 'Archived Loan Email' end as EmailType, "
                        + "d.Name as LoanFile, "
                        + "dbo.lpfn_GetEmailLogFromUser(a.EmailLogId) as FromUserName, "
                        + "dbo.lpfn_GetEmailLogFirstToUser(a.EmailLogId) as ToUserName, "
                        + "a.Subject, a.EmailBody, b.FileId as LoanID,dbo.lpfn_GetEmailLog_AttachmentsByEmailLogId(EmailLogId) as Attachments "
                        + " from EmailLog as a "
                        + "inner join Loans as b on a.FileId=b.FileId "
                        + "inner join LoanContacts as c on b.FileId= c.FileId "
                        + "inner join PointFiles as d on c.FileId=d.FileId "
                        + "where c.ContactId=" + this.sProspectID + " and (c.ContactRoleId=1 or c.ContactRoleId=2) "
                        + "union "
                        + "select a.EmailLogId, a.LastSent, a.DateTimeReceived, "
                        + "case when d.Status='Prospect' then 'Lead Email' when d.Status='Processing' then 'Active Loan Email' else 'Archived Loan Email' end as EmailType, "
                        + "e.Name as LoanFile, "
                        + "dbo.lpfn_GetEmailLogFromUser(a.EmailLogId) as FromUserName, "
                        + "dbo.lpfn_GetEmailLogFirstToUser(a.EmailLogId) as ToUserName, "
                        + "a.Subject, a.EmailBody, b.FileId as LoanID ,dbo.lpfn_GetEmailLog_AttachmentsByEmailLogId(EmailLogId) as Attachments "
                        + " from EmailLog as a "
                        + "inner join LoanAlerts as b on a.LoanAlertId=b.LoanAlertId "
                        + "inner join LoanContacts as c on b.FileId=c.FileId "
                        + "inner join Loans as d on c.FileId=d.FileId "
                        + "inner join PointFiles as e on d.FileId=e.FileId "
                        + "where c.ContactId=" + this.sProspectID + " and (c.ContactRoleId=1 or c.ContactRoleId=2)) as t";

        string sWhere = GetSqlWhereClause();
        //str_Where = string.Format(" AND FileId='{0}' ", sFileId);
        int recordCount = 0;

        int iRowCount = LPWeb.DAL.DbHelperSQL.Count(sDbTable, sWhere);

        int iStartIndex = PageCommon.CalcStartIndex(pageIndex, pageSize);
        int iEndIndex = PageCommon.CalcEndIndex(iStartIndex, pageSize, iRowCount);
        DataSet loansList = null;

        //DataTable loansList = emailLogManager.GetProspectEmailLogList(sDbTable, iStartIndex, iEndIndex, sWhere, OrderName, OrderType);

        loansList = emailLogManager.GetListForGridView_Client(pageSize, pageIndex, str_Where, out recordCount, OrderName, OrderType);

        AspNetPager1.PageSize = pageSize;
        //AspNetPager1.RecordCount = iRowCount;
        AspNetPager1.RecordCount = recordCount;

        gridList.DataSource = loansList;
        gridList.DataBind();
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
        BindGrid();
    }

    /// <summary>
    /// Get filter
    /// </summary>
    /// <returns></returns>
    private string GetSqlWhereClause()
    {
        string strWhere = "";

        if (!string.IsNullOrEmpty(this.tbSentStart.Text))
        {
            DateTime dt = new DateTime();
            DateTime dtNew = new DateTime();
            try
            {
                if (DateTime.TryParse(this.tbSentStart.Text, out dt))
                {
                    dtNew = new DateTime(dt.Year, dt.Month, dt.Day);
                    strWhere += string.Format(" AND [LastSent] >= '{0}' ", dtNew.ToString());
                }
            }
            catch
            { }
        }

        if (!string.IsNullOrEmpty(this.tbSentEnd.Text))
        {
            DateTime dt = new DateTime();
            DateTime dtNew = new DateTime();
            try
            {
                if (DateTime.TryParse(this.tbSentEnd.Text, out dt))
                {
                    dtNew = dt.AddDays(1).AddMilliseconds(-1);
                    strWhere += string.Format(" AND [LastSent] <= '{0}' ", dtNew.ToString());
                }
            }
            catch
            { }
        }

        string sEmailTypeFilter = this.ddlEmailTypeFilter.SelectedValue;
        if (sEmailTypeFilter == string.Empty)    // All Emails
        {

        }
        else if (sEmailTypeFilter == "0")    // Prospect Emails
        {
            strWhere += " and EmailType='Prospect Email'";
        }
        else
        {
            strWhere += " and LoanID=" + sEmailTypeFilter;
        }

        return strWhere;
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
                ViewState["orderName"] = "LastSent";
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
                ViewState["orderType"] = 1;
            return Convert.ToInt32(ViewState["orderType"]);
        }
        set
        {
            ViewState["orderType"] = value;
        }
    }

    StringBuilder sbAllIds = new StringBuilder();
    string strCkAllID = "";
    /// <summary>
    /// Set selected row when click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView gv = sender as GridView;
        if (null == gv)
            return;

        if (DataControlRowType.Header == e.Row.RowType)
        {
            CheckBox ckbAll = e.Row.FindControl("ckbAll") as CheckBox;
            if (null != ckbAll)
            {
                ckbAll.Attributes.Add("onclick", string.Format("CheckAllClicked(this, '{0}', '{1}', '{2}');",
                    gv.ClientID, this.hiAllIds.ClientID, this.hiCheckedIds.ClientID));
                strCkAllID = ckbAll.ClientID;
            }
        }
        else if (DataControlRowType.DataRow == e.Row.RowType)
        {
            string strID = gv.DataKeys[e.Row.RowIndex].Value.ToString();

            if (sbAllIds.Length > 0)
                sbAllIds.Append(",");
            sbAllIds.AppendFormat("{0}", strID);

            CheckBox ckb = e.Row.FindControl("ckbSelect") as CheckBox;
            if (null != ckb)
            {
                ckb.Attributes.Add("onclick", string.Format("CheckBoxClicked(this, '{0}', '{1}', '{2}', '{3}');",
                    strCkAllID, this.hiAllIds.ClientID, this.hiCheckedIds.ClientID, strID));
            }
        }

        // set ranking icon
        Literal litBody = e.Row.FindControl("litBody") as Literal;
        if (null != litBody)
        {
            if (gv.DataKeys[e.Row.RowIndex]["EmailBody"].ToString() == string.Empty) 
            {
                litBody.Text = string.Empty;
            }
            else
            {
                byte[] bytes = null;
                try
                {
                    bytes = (byte[])gv.DataKeys[e.Row.RowIndex]["EmailBody"];
                }
                catch
                { }
                string strTemp = new String(Encoding.UTF8.GetChars(bytes));
                var reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
                strTemp = reg.Replace(strTemp, "");
                if (strTemp.Length > 50)
                    litBody.Text = strTemp.Substring(0, 50) + "...";
                else
                    litBody.Text = strTemp;
            }
        }
    }

    protected void gridList_PreRender(object sender, EventArgs e)
    {
        this.hiAllIds.Value = sbAllIds.ToString();
        this.hiCheckedIds.Value = "";
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

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        isReset = true;
        BindGrid();
    }
}
