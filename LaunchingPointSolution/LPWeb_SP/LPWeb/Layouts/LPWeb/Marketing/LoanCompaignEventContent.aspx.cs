using System;
using System.IO;
using System.Drawing;
using System.Net;
using System.Data;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Common;
using LPWeb.BLL;

namespace LPWeb.Layouts.LPWeb.Marketing
{
    /// <summary>
    /// Show LoanMarketingEvent detail
    /// Author: Peter
    /// Date: 2011-08-10
    /// </summary>
    public partial class LoanCompaignEventContent : LayoutsPageBase
    {
        LoanMarketingEvents lmeMngr = new LoanMarketingEvents();
        MarketingSettings msMngr = new MarketingSettings();

        protected void Page_Load(object sender, EventArgs e)
        {
            bool bIsValid = PageCommon.ValidateQueryString(this, "eventId", QueryStringType.ID);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, "Failed to load this page: missing required query string.", "window.opener=null; window.close();");
            }

            int iEventID = 0;
            if (!int.TryParse(Request.QueryString["eventId"], out iEventID))
                iEventID = 0;
            Model.LoanMarketingEvents theEvent = lmeMngr.GetModel(iEventID);

            if (theEvent != null && theEvent.Action != null)
            {
                Model.MarketingSettings msObj = msMngr.GetModel();
                string strCampaignDetailUrl = string.Format("{0}", msObj.CampaignDetailURL).TrimEnd(new char[] { '/' });
                string strEventUrl = string.Format("{0}", theEvent.EventURL).Replace("-s", "-p");

                lbTitle.Text = string.Format("Week {0} {1}", theEvent.WeekNo, theEvent.Action);
                if ("CALL" == theEvent.Action.ToUpper())
                {
                    lbContent.Text = string.Format("<table style='Width:100%; vertical-align: top;' ><tr><td align='left' valign='top'>{0}</td></tr></table>", theEvent.EventContent);
                    lbContent.Visible = true;
                    ifmRes.Visible = false;
                }
                else
                {
                    string strResUrl = "";
                    if ("EMAIL" == theEvent.Action.ToUpper())
                    {
                        if (!string.IsNullOrEmpty(theEvent.EventContent))
                            strResUrl = string.Format("{0}/{1}", strCampaignDetailUrl, theEvent.EventContent);
                    }
                    else if (theEvent.Action.ToUpper().Contains("MAIL"))
                    {
                        if (!string.IsNullOrEmpty(strEventUrl))
                            strResUrl = string.Format("{0}/{1}", strCampaignDetailUrl, strEventUrl);
                    }
                    ifmRes.Attributes.Add("src", string.Format("DownloadResource.aspx?url={0}", strResUrl));
                    ifmRes.Visible = true;
                    lbContent.Visible = false;
                }
            }
        }
    }
}
