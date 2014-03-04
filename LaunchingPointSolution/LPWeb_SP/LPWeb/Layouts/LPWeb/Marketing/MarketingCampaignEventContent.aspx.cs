using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Common;
using System.Data;
using LPWeb.BLL;
using System.IO;
using System.Drawing;
using System.Net;

namespace LPWeb.Layouts.LPWeb.Marketing
{
    public partial class MarketingCampaignEventContent : LayoutsPageBase
    {
        private int iEventID = 0;
        string sErrorMsg = "Failed to load this page: missing required query string.";
        MarketingCampaignEvents lmeMngr = new MarketingCampaignEvents();
        MarketingSettings msMngr = new MarketingSettings();

        protected void Page_Load(object sender, EventArgs e)
        {
            bool bIsValid = PageCommon.ValidateQueryString(this, "campaignEventId", QueryStringType.ID);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.opener=null; window.close();");
            }
            int iEventID = 0;
            if (!int.TryParse(Request.QueryString["campaignEventId"], out iEventID))
                iEventID = 0;
            Model.MarketingCampaignEvents theEvent = lmeMngr.GetModel(iEventID);

            if (theEvent != null && theEvent.Action != null)
            {
                Model.MarketingSettings msObj = msMngr.GetModel();
                string sCampaignDetailURL = string.Format("{0}", msObj.CampaignDetailURL).TrimEnd(new char[] { '/' });
                string strEventUrl = string.Format("{0}", theEvent.EventURL).Replace("-s", "-p");

                lbTitle.Text = string.Format("Week {0} {1}", theEvent.WeekNo, theEvent.Action);
                if (theEvent.Action.ToUpper() == "CALL")
                {
                    lbContent.Text = string.Format("<table style='Width:100%; vertical-align: top;' ><tr><td align='left' valign='top'>{0}</td></tr></table>", theEvent.EventContent);
                    ifmEmail.Visible = false;
                    lbContent.Visible = true;
                }
                else
                {
                    string strResUrl = "";
                    if (theEvent.Action.ToUpper() == "EMAIL")
                    {
                        if (!string.IsNullOrEmpty(theEvent.EventContent))
                            strResUrl = string.Format("{0}/{1}", sCampaignDetailURL, theEvent.EventContent);
                    }
                    else if (theEvent.Action.ToUpper().Contains("MAIL"))
                    {
                        if (!string.IsNullOrEmpty(strEventUrl))
                            strResUrl = string.Format("{0}/{1}", sCampaignDetailURL, strEventUrl);
                    }
                    //ifmEmail.Attributes.Add("src", string.Format("DownloadResource.aspx?url={0}", strResUrl));
                    ifmEmail.Attributes.Add("src", string.Format("{0}", strResUrl));
                    ifmEmail.Visible = true;
                    lbContent.Visible = false;
                }
            }            
        }

        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        } 
    }
}
