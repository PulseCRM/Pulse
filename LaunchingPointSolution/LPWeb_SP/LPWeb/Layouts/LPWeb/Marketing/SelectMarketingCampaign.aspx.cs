﻿using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using Utilities;
using LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.BLL;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;


public partial class SelectMarketingCampaign : LayoutsPageBase
{

    private LPWeb.BLL.MarketingCampaigns marketingCampaigns = new LPWeb.BLL.MarketingCampaigns();
    private LoginUser _curLoginUser = new LoginUser();
    private string sType = "";
    private string sFileId = "";
    private string sCampIds = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        sType = Request.QueryString["type"] == null ? "0" : Request.QueryString["type"].ToString();
        sFileId = Request.QueryString["f"] == null ? "0" : Request.QueryString["f"].ToString();
        sCampIds = Request.QueryString["camps"] ?? string.Empty;
        if (!IsPostBack)
        {

            BindContactsGrid();
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
                ViewState["orderName"] = "CategoryName"; // from "CampaignName" to "CategoryName"
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
    /// Bind Grid
    /// </summary>
    private void BindContactsGrid()
    {
        int pageSize = 15;
        int pageIndex = 1;

        if (AspNetPager1.CurrentPageIndex > 0)
            pageIndex = AspNetPager1.CurrentPageIndex;

        string queryCondition = GenerateQueryCondition();
        int recordCount = 0;

        DataTable dt = null;
        try
        {
            DataSet ds = null;
            if ("2" == sType)
                ds = this.marketingCampaigns.GetCampaignsListForPersonlizationAdd(pageSize, pageIndex, queryCondition, out recordCount, OrderName, OrderType);
            else
                ds = this.marketingCampaigns.GetCampaignsList(pageSize, pageIndex, queryCondition, out recordCount, OrderName, OrderType);
            dt = ds.Tables[0];
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

        AspNetPager1.PageSize = pageSize;
        AspNetPager1.RecordCount = recordCount;

        this.gridMarketingCampaignList.DataSource = dt;
        this.gridMarketingCampaignList.DataBind();
    }

    /// <summary>
    /// 根据用户界面选择生成过滤条件
    /// </summary>
    /// <returns></returns>
    private string GenerateQueryCondition()
    {
        string queryCon = " 1=1 ";
        if ("2" == sType)
            queryCon = string.Format(" AND CampaignId NOT IN (SELECT CampaignId FROM AutoCampaigns WHERE PaidBy=0 OR SelectedBy={0})", _curLoginUser.iUserID);
        else if(!string.IsNullOrEmpty(sCampIds))
        {
            queryCon += string.Format(" AND CampaignId NOT IN ({0})", sCampIds.TrimEnd(new char[]{','}));
        }
        else //Marketing Tab 调用条件
        {
            if (sFileId != "" && sFileId != "0")
            {
                queryCon += " AND CampaignId not in (select CampaignId from AutoCampaigns ) and CampaignId not in (select CampaignId from LoanMarketing WHERE FileId=" + sFileId + ")";
            }
            else
            {
                queryCon += " AND CampaignId not in (select CampaignId from AutoCampaigns) and CampaignId not in (select CampaignId from LoanMarketing)";
            }
        }

        return queryCon;
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
        BindContactsGrid();
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        string sCheckValues = hdnSelectedCampaignIds.Value;
        string sCampaignName = string.Empty;
        string sCategoryName = string.Empty;
        sCheckValues = sCheckValues.TrimEnd(',');

        int cId = 0;
        int.TryParse(sCheckValues, out cId);
        try
        {
            var model = marketingCampaigns.GetModel(cId);
            if (model != null)
            {
                sCampaignName = model.CampaignName;
          //    sCategoryName = model.CategoryName;
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);    
        }


        ClientFun("callback", string.Format("callBack('{0}','{1}','{2}');", sCheckValues, sCampaignName,sCategoryName));
    }

    /// <summary>
    /// Call client function
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

