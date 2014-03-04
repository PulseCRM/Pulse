using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.BLL;
using System.Data;

namespace LPWeb.Layouts.LPWeb.Marketing
{
    public partial class MarketingCampaignDetail : BasePage
    {
        protected int iCampaignId = 0;
        private int ipageSize = 10;
        private int ipageIndex = 10;
        private int recordCount = 0;
        private string sOrderName = "CategoryName";

        private BLL.MarketingCampaignEvents _bMarketingCampaignEvents = new BLL.MarketingCampaignEvents();
        private BLL.MarketingCampaigns _bMarketingCampaigns = new BLL.MarketingCampaigns();

        string sReturnUrl = "MarketingCampaign.aspx";
        string sErrorMsg = "Failed to load this page: missing required query string.";
        string sPageFrom = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool bIsValid = PageCommon.ValidateQueryString(this, "campaignId", QueryStringType.ID);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.parent.location.href='" + sReturnUrl + "'");
            }
            iCampaignId = Convert.ToInt32(Request.QueryString["campaignId"]);
            ipageIndex = Convert.ToInt32(Request.QueryString["pageindex"]);
            MarketingCampaigns _bMarketingCampaigns = new MarketingCampaigns();

            DataSet dsCampaigns = _bMarketingCampaigns.GetCampaignsList(ipageSize, ipageIndex, "1>0", out recordCount, sOrderName, 0);
            //DataSet dsCampaigns = _bMarketingCampaigns.GetAllList();
            string sIDs = "";
            foreach (DataRow dr in dsCampaigns.Tables[0].Rows)
            {
                sIDs += dr["CampaignId"].ToString() + ",";
            }
            sIDs = sIDs.TrimEnd(',');
            this.hfID.Value = iCampaignId.ToString();
            this.hfIDs.Value = sIDs;
        }
    }
}
