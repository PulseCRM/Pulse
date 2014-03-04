using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Utilities;
using LPWeb.BLL;
using LPWeb.LP_Service;
using LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Marketing
{
    /// <summary>
    /// Loan Marketing Activities Events List
    /// Author: Peter
    /// Date: 2011-07-10
    /// </summary>
    public partial class MarketingActivitiesEvents : BasePage
    {
        LoanMarketingEvents lmeMngr = new LoanMarketingEvents();
        private bool isReset = false;
        protected string FromURL = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if ("1" == Request.QueryString["unsync"])
            {
                int nId = 0;
                if (!int.TryParse(Request.QueryString["eventId"], out nId))
                    nId = 0;
                if (0 != nId)
                {
                    // Send WCF Request: CompleteCampaignEventRequest 
                    try
                    {
                        ServiceManager sm = new ServiceManager();
                        using (LP2ServiceClient service = sm.StartServiceClient())
                        {
                            CompleteCampaignEventRequest req = new CompleteCampaignEventRequest();
                            ReqHdr hdr = new ReqHdr();
                            hdr.UserId = CurrUser.iUserID;
                            req.hdr = hdr;
                            req.FileId = 0;
                            req.CampaignId = 0;
                            req.EventId = 0;

                            CompleteCampaignEventResponse uResponse;
                            uResponse = service.CompleteCampaignEvent(req);
                            if (uResponse.hdr.Successful)
                            {
                                if (lmeMngr.CompleteLoanMarketingEvent(nId))
                                    Response.Write("");
                                else
                                    Response.Write("Failed to complete campaign event.");
                            }
                            else
                            {
                                string strMsg = "Failed to complete campaign event, reason: " + uResponse.hdr.StatusInfo;
                                LPLog.LogMessage(LogType.Logerror, strMsg);
                                Response.Write(strMsg);
                            }
                        }
                    }
                    catch (System.ServiceModel.EndpointNotFoundException ee)
                    {
                        LPLog.LogMessage(LogType.Logerror, ee.Message);
                        Response.Write("Failed to complete campaign event, reason: Marketing Manager is not running.");
                    }
                    catch (Exception ex)
                    {
                        string strMsg = "Failed to complete campaign event, reason: " + ex.Message;
                        LPLog.LogMessage(LogType.Logerror, strMsg);
                        Response.Write(strMsg);
                    }
                }
                Response.End();
            }

            if (Request.Url != null)
            {
                FromURL = Request.Url.ToString();
            }

            if (!IsPostBack)
            {
                // binding categories drop down list
                BLL.MarketingCategory marketingCategory = new BLL.MarketingCategory();
                DataSet dsCategories = marketingCategory.GetListInAlphOrder(" CategoryId IN (SELECT b.CategoryId FROM LoanMarketing a INNER JOIN MarketingCampaigns b ON a.CampaignId=b.CampaignId)");
                this.ddlCategories.DataSource = dsCategories;
                this.ddlCategories.DataBind();
                this.ddlCategories.Items.Insert(0, new ListItem("All Categories", "0"));

                // binding campaigns drop down list
                BLL.MarketingCampaigns marketingCampaignsMngr = new BLL.MarketingCampaigns();
                DataSet dsCampaigns = marketingCampaignsMngr.GetListInAlphOrder(" CampaignId IN (SELECT DISTINCT CampaignId FROM LoanMarketing)");
                this.ddlCampaigns.DataSource = dsCampaigns;
                this.ddlCampaigns.DataBind();
                this.ddlCampaigns.Items.Insert(0, new ListItem("All Campaigns", "0"));
                if (null != Request.QueryString["CampaignId"])
                {
                    ListItem item = this.ddlCampaigns.Items.FindByValue(Request.QueryString["CampaignId"]);
                    if (null != item)
                        item.Selected = true;
                }

                // binding started by user drop down list
                DataSet dsStartedBy = lmeMngr.GetStartedByUserOfLoanMarketing("");
                this.ddlStartBy.DataSource = dsStartedBy;
                this.ddlStartBy.DataBind();
                this.ddlStartBy.Items.Insert(0, new ListItem("Started By", "0"));

                BindGrid();
            }
        }

        /// <summary>
        /// Bind contact role gridview
        /// </summary>
        private void BindGrid()
        {
            int pageSize = AspNetPager1.PageSize;
            int pageIndex = 1;

            if (isReset == true)
                pageIndex = AspNetPager1.CurrentPageIndex = 1;
            else
                pageIndex = AspNetPager1.CurrentPageIndex;

            string strWhare = GetSqlWhereClause();
            int recordCount = 0;

            DataSet lmEvents = null;
            try
            {
                lmEvents = lmeMngr.GetListForMarketingActivitiesEvents(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gridList.DataSource = lmEvents;
            gridList.DataBind();
        }

        /// <summary>
        /// Get filter
        /// </summary>
        /// <returns></returns>
        private string GetSqlWhereClause()
        {
            StringBuilder sbWhere = new StringBuilder();

            if (this.ddlCategories.SelectedIndex > 0)
            {
                sbWhere.AppendFormat(" AND CategoryId='{0}'", this.ddlCategories.SelectedValue);
            }

            if (this.ddlCampaigns.SelectedIndex > 0)
            {
                sbWhere.AppendFormat(" AND CampaignId='{0}'", this.ddlCampaigns.SelectedValue);
            }

            if (this.ddlStatuses.SelectedIndex > 0)
            {
                sbWhere.AppendFormat(" AND [Status]='{0}'", this.ddlStatuses.SelectedValue);
            }

            if (this.ddlType.SelectedIndex > 0)
            {
                sbWhere.AppendFormat(" AND [Type]='{0}'", this.ddlType.SelectedValue);
            }

            if (this.ddlStartBy.SelectedIndex > 0)
            {
                sbWhere.AppendFormat(" AND StartedBy='{0}'", this.ddlStartBy.SelectedValue);
            }

            if (!string.IsNullOrEmpty(this.tbStartDate.Text))
            {
                sbWhere.AppendFormat(" AND ExecutionDate>='{0}'", this.tbStartDate.Text);
            }

            if (!string.IsNullOrEmpty(this.tbEndDate.Text))
            {
                sbWhere.AppendFormat(" AND ExecutionDate<='{0}'", this.tbEndDate.Text);
            }

            if (this.ddlAlphabet.SelectedIndex > 0)
            {
                sbWhere.AppendFormat(" AND BorrowerName LIKE '{0}%'", ddlAlphabet.SelectedValue);
            }

            return sbWhere.ToString();
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
            BindGrid();
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
                    ViewState["orderName"] = "CampaignName";
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

        protected void gridList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (DataControlRowType.DataRow == e.Row.RowType)
            {
                CheckBox ckbCompleted = e.Row.FindControl("ckbCompleted") as CheckBox;
                if (null != ckbCompleted)
                {
                    ckbCompleted.Attributes.Add("onclick",
                        string.Format("completedStateChanged(this, '{0}');", gridList.DataKeys[e.Row.RowIndex]["LoanMarketingEventId"]));

                    bool completed = false;
                    if (null != gridList.DataKeys[e.Row.RowIndex]["Completed"])
                    {
                        try
                        {
                            completed = Convert.ToBoolean(gridList.DataKeys[e.Row.RowIndex]["Completed"]);
                        }
                        catch
                        {
                            completed = false;
                        }
                    }
                    ckbCompleted.Checked = completed;
                    string strAction = string.Format("{0}", gridList.DataKeys[e.Row.RowIndex]["Action"]);
                    if (completed)
                        ckbCompleted.Enabled = false;
                    else if (strAction.ToLower() == "call")
                    {
                        ckbCompleted.Enabled = true;
                    }
                    else 
                    {
                        ckbCompleted.Enabled = false;
                    }
                }

                LinkButton lbtnClient = e.Row.FindControl("lbtnClient") as LinkButton;
                if (null != lbtnClient)
                {
                    string strBorrowerName = string.Format("{0}", gridList.DataKeys[e.Row.RowIndex]["BorrowerName"]);
                    string strLoanStatus = string.Format("{0}", gridList.DataKeys[e.Row.RowIndex]["LoanStatus"]);
                    string strPointFileName = string.Format("{0}", gridList.DataKeys[e.Row.RowIndex]["PointFileName"]);

                    lbtnClient.Text = string.Format("{0} - {1} Loan - {2}", strBorrowerName, strLoanStatus, 
                        strPointFileName.Substring(strPointFileName.LastIndexOf('\\') + 1));
                    lbtnClient.OnClientClick = string.Format("showLoanDetail('{0}'); return false;", gridList.DataKeys[e.Row.RowIndex]["FileId"]);
                }

                // set ranking icon
                Literal litAction = e.Row.FindControl("litAction") as Literal;
                if (null != litAction)
                {
                    string strEventId = string.Format("{0}", gridList.DataKeys[e.Row.RowIndex]["LoanMarketingEventId"]);
                    string strAction = string.Format("{0}", gridList.DataKeys[e.Row.RowIndex]["Action"]);
                    string strActionFileName = "";
                    
                    if ("CALL" == strAction.ToUpper())
                    {
                        strActionFileName = "Action_Call.jpg";
                    }
                    else if ("EMAIL" == strAction.ToUpper())
                    {
                        strActionFileName = "Action_Email.jpg";
                    }
                    else if (strAction.ToUpper().Contains("MAIL"))
                    {
                        strActionFileName = "Action_Mail.jpg";
                    }

                    if (!string.IsNullOrEmpty(strActionFileName))
                    {
                        litAction.Text = string.Format("<img alt='{0}' src='../images/marketing/{0}' style='cursor: pointer;' onclick='ShowEventContent(\"{1}\")' />",
                            strActionFileName, strEventId);
                    }
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }

        protected void ddlAlphabet_SelectedIndexChanged(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }
    }
}
