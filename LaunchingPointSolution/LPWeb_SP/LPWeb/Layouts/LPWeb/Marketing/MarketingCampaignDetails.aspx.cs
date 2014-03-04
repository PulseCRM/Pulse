using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;
using LPWeb.BLL;

namespace LPWeb.Layouts.LPWeb.Marketing
{
    public partial class MarketingCampaignDetails : BasePage
    {
        private int iCampaignId = 0;
        private BLL.MarketingCampaignEvents _bMarketingCampaignEvents = new BLL.MarketingCampaignEvents();
        private BLL.MarketingCampaigns _bMarketingCampaigns = new BLL.MarketingCampaigns();

        string sReturnUrl = "MarketingCampaign.aspx";
        string sErrorMsg = "Failed to load this page: missing required query string.";

        protected void Page_Load(object sender, EventArgs e)
        {
            bool bIsValid = PageCommon.ValidateQueryString(this, "campaignId", QueryStringType.ID);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.parent.location.href='" + sReturnUrl + "'");
            }
            iCampaignId = Convert.ToInt32(Request.QueryString["campaignId"]);

            if (!IsPostBack)
            {
                BindCampaignInfo();
            }
        }

        private void BindCampaignInfo()
        {
            Model.MarketingCampaigns _mMarketingCampaigns = _bMarketingCampaigns.GetModel(iCampaignId);

            this.lbName.Text = _mMarketingCampaigns.CampaignName;
            this.lbPrice.Text =_mMarketingCampaigns.Price.ToString()==""? "0": Convert.ToDecimal(_mMarketingCampaigns.Price).ToString("n2");
            this.lbDesc.Text = _mMarketingCampaigns.Description;
        }

        /// <summary>
        ///  Get Campaign Detail Icons
        /// Alex 2011-06-22
        /// </summary>
        /// <param name="ltreportTaskInfo"></param>
        /// <returns></returns>
        protected string GetCampaignDetailIconsInfo()
        {
            DataSet ds = _bMarketingCampaignEvents.GetList("CampaignId=" + this.iCampaignId);
            MarketingSettings _bMarketingSettings=new MarketingSettings();
            DataSet dsSetting = _bMarketingSettings.GetAllList();
            string sCampaignDetailURL = "";
            if (dsSetting.Tables[0].Rows.Count > 0 && dsSetting.Tables[0].Rows[0]["CampaignDetailURL"]!=DBNull.Value)
            {
                sCampaignDetailURL = dsSetting.Tables[0].Rows[0]["CampaignDetailURL"].ToString();
            }
            string sRst = "";
            // <img alt='' src='<@ContactPicturePath@>' style='width:80px; height: 108px;' />
            string sTemp = @"<td style='height: 180px; width:280px' align='center'>
                        <div style='padding: 8px 0px 8px 10px; color: #1f477d; font-size: 14px;'><@EventInfo@></div>
                        <table cellpadding='0' cellspacing='0' border='0' style='width: 100%; padding-left: 10px; font-size: 11px; color: #365074;'>
                            <tr>
                                <td style='width: 250px;'align='center'>
                                   <@Link@><img id='img<@i@>' src=<@ImageURL@>  style='width:160px; height:160px'/><@EndLink@>
                                </td>
                            </tr>
                        </table>
                    </td>";

            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string sNewPart = sTemp;
                sNewPart = sNewPart.Replace("<@EventInfo@>", "Week " + dr["WeekNo"].ToString() + " " + dr["Action"].ToString());
                sNewPart = sNewPart.Replace("<@i@>", i.ToString());
                sNewPart = sNewPart.Replace("<@ImageURL@>", sCampaignDetailURL + "\\" + dr["EventURL"].ToString());
                if (dr["Action"].ToString() == "Call" || dr["Action"].ToString() == "Email" || dr["Action"].ToString().EndsWith("Mailing")==true )
                {
                    sNewPart = sNewPart.Replace("<@Link@>", "<a href='MarketingCampaignEventContent.aspx?campaignEventId=" + dr["CampaignEventId"].ToString() + "'  target='_blank' >");
                    //sNewPart = sNewPart.Replace("<@Link@>", "<a href='' onclick='javascript:ShowDetails(" + dr["CampaignEventId"].ToString() + ")'  >");
                    sNewPart = sNewPart.Replace("<@EndLink@>", "</a>");
                }
                else
                {
                    sNewPart = sNewPart.Replace("<@Link@>", "");
                    sNewPart = sNewPart.Replace("<@EndLink@>", "");
                }


                if (i == 0)
                {
                    sNewPart = "<tr>" + sNewPart;
                }

                sRst += sNewPart;

                i++;

                if (i == 2)
                {
                    sRst += "</tr>";
                    i = 0;
                }

            }
            if (ds.Tables[0].Rows.Count % 2 != 0)  //奇数个Contact记录，则增加结束</tr>
            {
                sRst += "</tr>";
            }

            return sRst;
        }
    }
}
