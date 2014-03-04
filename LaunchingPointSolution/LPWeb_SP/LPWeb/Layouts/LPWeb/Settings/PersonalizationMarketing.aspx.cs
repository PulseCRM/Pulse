using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Web.UI.WebControls;
using Utilities;
using System.Data;
using System.Text;
using LPWeb.Common;
using System.Web.UI;
using System.Collections.Generic;
using LPWeb.LP_Service;

namespace LPWeb.Layouts.LPWeb.Settings
{
    /// <summary>
    /// Personalization - Marketing
    /// Author: Peter
    /// Date: 2011-05-25
    /// </summary>
    public partial class PersonalizationMarketing : BasePage
    {
        BLL.Company_General cG = new BLL.Company_General();
        BLL.Users UsersManager = new BLL.Users();
        BLL.MarketingCampaigns MarketingCampaignsManager = new BLL.MarketingCampaigns();
        BLL.Template_Stages TemplateStagesManager = new BLL.Template_Stages();
        DataSet dsTemplateStage = new DataSet();
        private bool isReset = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                BindTemplStageDDL(ddlTemplStageSel1, ddlTemplStageSel2);
                ddlLoanTypeSel.Attributes.Add("onchange", string.Format("loanTypeChanged(this, '{0}', '{1}', '{2}')",
                    ddlTemplStageSel1.ClientID, ddlTemplStageSel2.ClientID, hiTemplStageSel.ClientID));
                ddlTemplStageSel1.Attributes.Add("onchange", string.Format("ddlTemplStageChanged(this, '{0}')",
                    hiTemplStageSel.ClientID));
                ddlTemplStageSel2.Attributes.Add("onchange", string.Format("ddlTemplStageChanged(this, '{0}')",
                    hiTemplStageSel.ClientID));
                Model.Company_General compayGen = cG.GetModel();
                if (!compayGen.EnableMarketing)
                {
                    this.ckbEnableMarketing.Enabled = false;
                    this.lbtnAdd.Enabled = false;
                }

                Model.Users userInfo = UsersManager.GetModel(CurrUser.iUserID);
                if (null == userInfo)
                {
                    LPLog.LogMessage(LogType.Logerror, string.Format("User Personalization: User with id {0} does not exist.", CurrUser.iUserID));
                    ClientFun("unknowerrormsg", "alert('User does not exists, unknow error.');");
                }
                this.ckbEnableMarketing.Checked = userInfo.MarketingAcctEnabled;
            }
        }

        /// <summary>
        /// Bind gridview
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

            DataSet dsList = null;
            try
            {
                dsTemplateStage = TemplateStagesManager.GetList("Enabled=1");

                dsList = MarketingCampaignsManager.GetListForPersonlization(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gridList.DataSource = dsList;
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
            string strWhere = string.Format(" AND PaidBy=2 and SelectedBy='{0}'", CurrUser.iUserID);

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

                // bind template stage dropdownlist
                DropDownList ddlLoanType = e.Row.FindControl("ddlLoanType") as DropDownList;
                DropDownList ddlTemplStage1 = e.Row.FindControl("ddlTemplStage1") as DropDownList;
                DropDownList ddlTemplStage2 = e.Row.FindControl("ddlTemplStage2") as DropDownList;
                TextBox tbTeplStageId = e.Row.FindControl("tbTeplStageId") as TextBox;
                if (ddlLoanType != null && ddlTemplStage1 != null && ddlTemplStage2 != null && tbTeplStageId != null)
                {
                    // bind dropdownlist
                    BindTemplStageDDL(ddlTemplStage1, ddlTemplStage2);

                    // show template stage list
                    string strLoanType = string.Format("{0}", gv.DataKeys[e.Row.RowIndex]["LoanType"]);
                    ListItem item = ddlLoanType.Items.FindByValue(strLoanType);
                    if (null != item)
                        item.Selected = true;
                    switch (strLoanType.ToLower())
                    {
                        case "active":
                            goto case "archived";
                        case "archived":
                            ddlTemplStage1.Attributes.CssStyle.Add("display", "");
                            ddlTemplStage2.Attributes.CssStyle.Add("display", "none");
                            break;
                        case "opportunities":
                            ddlTemplStage1.Attributes.CssStyle.Add("display", "none");
                            ddlTemplStage2.Attributes.CssStyle.Add("display", "");
                            break;
                        default:
                            ddlTemplStage1.Attributes.CssStyle.Add("display", "none");
                            ddlTemplStage2.Attributes.CssStyle.Add("display", "none");
                            break;
                    }

                    string strTemplStageId = string.Format("{0}", gv.DataKeys[e.Row.RowIndex]["TemplStageId"]);
                    item = ddlTemplStage1.Items.FindByValue(strTemplStageId);
                    if (null != item)
                        item.Selected = true;
                    item = ddlTemplStage2.Items.FindByValue(strTemplStageId);
                    if (null != item)
                        item.Selected = true;

                    ddlLoanType.Attributes.Add("onchange", string.Format("loanTypeChanged(this, '{0}', '{1}', '{2}')",
                        ddlTemplStage1.ClientID, ddlTemplStage2.ClientID, tbTeplStageId.ClientID));

                    ddlTemplStage1.Attributes.Add("onchange", string.Format("ddlTemplStageChanged(this, '{0}')",
                        tbTeplStageId.ClientID));
                    ddlTemplStage2.Attributes.Add("onchange", string.Format("ddlTemplStageChanged(this, '{0}')",
                        tbTeplStageId.ClientID));
                }
            }
        }

        private void BindTemplStageDDL(DropDownList ddl1, DropDownList ddl2)
        {
            if (null != dsTemplateStage && dsTemplateStage.Tables.Count > 0)
            {
                DataView dvStage1 = new DataView();
                DataView dvStage2 = new DataView();
                dvStage1.Table = dsTemplateStage.Tables[0];
                dvStage2.Table = dsTemplateStage.Tables[0];
                dvStage1.RowFilter = "WorkflowType='Processing'";
                dvStage2.RowFilter = "WorkflowType='Prospect'";
                dvStage1.Sort = "Name";
                dvStage2.Sort = "Name";
                ddl1.DataSource = dvStage1;
                ddl1.DataBind();
                ddl1.Items.Insert(0, new ListItem("-- select --", ""));
                ddl2.DataSource = dvStage2;
                ddl2.DataBind();
                ddl2.Items.Insert(0, new ListItem("-- select --", ""));
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Model.Users userInfo = UsersManager.GetModel(CurrUser.iUserID);

            try
            {
                if (null == userInfo)
                {
                    LPLog.LogMessage(LogType.Logerror, string.Format("User Personalization - Marketing: User with id {0} does not exist.", CurrUser.iUserID));
                    ClientFun("unknowerrmsg2", "alert('User does not exists, unknow error.');");
                }
                if (!GetUserInfo(ref userInfo))
                {
                    ClientFun("invalidinputmsg", "alert('Invalid input!');");
                    return;
                }
                UsersManager.Update(userInfo);

                DataTable dtAc = new DataTable();
                DataColumn col = new DataColumn("ID");
                dtAc.Columns.Add(col);
                col = new DataColumn("LoanType");
                dtAc.Columns.Add(col);
                col = new DataColumn("TemplStageId");
                dtAc.Columns.Add(col);
                col = new DataColumn("Enabled");
                dtAc.Columns.Add(col);
                DataRow drNew = null;
                foreach (GridViewRow gvr in gridList.Rows)
                {
                    DropDownList ddlLoanType = gvr.FindControl("ddlLoanType") as DropDownList;
                    TextBox tbTeplStageId = gvr.FindControl("tbTeplStageId") as TextBox;
                    CheckBox ckbEnabled = gvr.FindControl("ckbEnabled") as CheckBox;
                    if (ddlLoanType != null && tbTeplStageId != null && null != ckbEnabled)
                    {
                        int nId = 0;
                        if (!int.TryParse(string.Format("{0}", gridList.DataKeys[gvr.RowIndex].Value), out nId))
                            nId = 0;
                        if (nId > 0)
                        {
                            string strLoanType = ddlLoanType.SelectedValue;
                            if ("Active" == strLoanType || "Archived" == strLoanType || "Opportunities" == strLoanType)
                            { }
                            else
                                strLoanType = null;

                            int nTemplId = 0;
                            if (!int.TryParse(tbTeplStageId.Text, out nTemplId))
                                nTemplId = 0;

                            drNew = dtAc.NewRow();
                            dtAc.Rows.Add(drNew);
                            drNew["ID"] = nId;
                            drNew["LoanType"] = strLoanType;
                            if (0 == nTemplId)
                                drNew["TemplStageId"] = null;
                            else
                                drNew["TemplStageId"] = nTemplId;
                            drNew["Enabled"] = ckbEnabled.Checked ? 1 : 0;
                        }
                    }
                }
                if (dtAc.Rows.Count > 0)
                {
                    MarketingCampaignsManager.SaveAutoCampaigns(dtAc);
                    BindGrid();
                }

                ClientFun("sucsmsg", "alert('Saved successfully.');");
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to save user personalization preferences info, reason:" + ex.Message);
                LPLog.LogMessage(LogType.Logerror, "Failed to save user personalization preferences info: " + ex.Message);
                return;
            }
        }

        /// <summary>
        /// load Users Personalization Info
        /// </summary>
        /// <param name="user"></param>
        private bool GetUserInfo(ref Model.Users user)
        {
            user.MarketingAcctEnabled = this.ckbEnableMarketing.Checked;

            return true;
        }

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            string[] arrIds = this.hiReturnedIds.Value.Split(new char[] { ',' });
            List<int> listIds = new List<int>();
            foreach (string str in arrIds)
            {
                int nTemp = 0;
                if (!int.TryParse(str, out nTemp))
                    nTemp = 0;
                if (nTemp > 0)
                    listIds.Add(nTemp);
            }
            if (listIds.Count > 0)
            {
                int nStage = 0;
                if (!int.TryParse(this.hiTemplStageSel.Value, out nStage))
                    nStage = 0;
                MarketingCampaignsManager.AddAutoCampaigns(listIds.ToArray(), this.hiLoanTypeSel.Value, nStage, CurrUser.iUserID);
                BindGrid();
            }
        }

        protected void lbtnRemove_Click(object sender, EventArgs e)
        {
            string[] arrIds = this.hiCheckedIds.Value.Split(new char[] { ',' });
            List<int> listIds = new List<int>();
            foreach (string str in arrIds)
            {
                int nTemp = 0;
                if (!int.TryParse(str, out nTemp))
                    nTemp = 0;
                if (nTemp > 0)
                    listIds.Add(nTemp);
            }

            if (listIds.Count > 0)
            {
                try
                {
                    ServiceManager sm = new ServiceManager();
                    using (LP2ServiceClient service = sm.StartServiceClient())
                    {
                        ReqHdr hdr;
                        RemoveCampaignRequest uReq = new RemoveCampaignRequest();
                        hdr = new ReqHdr();
                        hdr.UserId = CurrUser.iUserID;

                        uReq.hdr = hdr;
                        uReq.CampaignId = listIds[0];

                        RemoveCampaignResponse uResponse;
                        uResponse = service.RemoveCampaign(uReq);
                        if (uResponse.hdr.Successful)
                        {
                            MarketingCampaignsManager.RemoveAutoCampaigns(listIds.ToArray());
                            BindGrid();
                        }
                        else
                        {
                            PageCommon.AlertMsg(this, "Failed to remove campaign, error info: " + uResponse.hdr.StatusInfo);
                            LPLog.LogMessage(LogType.Logerror, "Failed to remove campaign, error info:" + uResponse.hdr.StatusInfo);
                            return;
                        }
                    }
                }
                catch (System.ServiceModel.EndpointNotFoundException ee)
                {
                    LPLog.LogMessage(ee.Message);
                    PageCommon.AlertMsg(this, "Failed to remove campaign, reason: Marketing Manager is not running.");
                    return;
                }
                catch (Exception ex)
                {
                    PageCommon.AlertMsg(this, "Failed to remove campaign, exception info: " + ex.Message);
                    LPLog.LogMessage(LogType.Logerror, "Failed to remove campaign, exception info: " + ex.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// call client function
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strScript"></param>
        private void ClientFun(string strKey, string strScript)
        {
            ClientScriptManager csm = this.Page.ClientScript;
            if (!csm.IsStartupScriptRegistered(this.GetType(), strKey))
            {
                csm.RegisterStartupScript(this.GetType(), strKey, strScript, true);
            }
        }
    }
}
