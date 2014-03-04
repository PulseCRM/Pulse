using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;

public partial class Settings_MarketingCampaignSelection : BasePage
{
    string sCategoryID = "0";
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 获取页面参数


        if (this.Request.QueryString["CategoryID"] != null)
        {
            this.sCategoryID = this.Request.QueryString["CategoryID"].ToString();
        }

        #endregion

        #region 加载Category列表

        string sSql0 = "select * from MarketingCategory order by CategoryName";
        DataTable CategoryList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
        this.ddlMarketingCategory.DataSource = CategoryList;
        this.ddlMarketingCategory.DataBind();

        #endregion

        if (this.Request.QueryString["CategoryID"] == null)
        {
            if (CategoryList.Rows.Count > 0)
            {
                this.sCategoryID = CategoryList.Rows[0]["CategoryID"].ToString();
            }
        }

        #region 加载Campaign列表

        string sWhere = " and a.CategoryId=" + this.sCategoryID;

        string sSql3 = "select count(1) from MarketingCampaigns as a inner join MarketingCategory as b on a.CategoryId=b.CategoryId where 1=1 " + sWhere;
        int iRowCount = Convert.ToInt32(LPWeb.DAL.DbHelperSQL.ExecuteScalar(sSql3));
        this.AspNetPager1.RecordCount = iRowCount;

        #endregion
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        int iStartIndex = this.AspNetPager1.StartRecordIndex;
        int iEndIndex = this.AspNetPager1.EndRecordIndex;

        string sWhere1 = " and CategoryId=" + this.sCategoryID;
        string sDbTable = "(select a.*,b.CategoryName from MarketingCampaigns as a inner join MarketingCategory as b on a.CategoryId=b.CategoryId) as t";
        DataTable CampaignListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sDbTable, iStartIndex, iEndIndex, sWhere1, "CampaignName", 0);

        this.gridCampaignList.DataSource = CampaignListData;
        this.gridCampaignList.DataBind();
    }
}
