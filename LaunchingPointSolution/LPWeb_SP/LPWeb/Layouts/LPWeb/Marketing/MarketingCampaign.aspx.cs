using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.BLL;
using System.Data;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Marketing
{
    public partial class MarketingCampaign : BasePage
    {
        private int _i = 0;
        protected string sDefaultURL = "";
        protected int iPageIndex = 1;
        protected int iCampaignId = 0;
        protected int _Queryi = 0;
        private bool bSel = false;
        private string sListIDs = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            iCampaignId = Request.QueryString["CampaignId"] == null ? 0 : Convert.ToInt32(Request.QueryString["CampaignId"]);

            sListIDs = ",";
            if (!IsPostBack)
            {
                BindFilter();
                BindGridInfo(1);
                if (bSel)
                {
                    iFrmURL.Attributes.Add("src", "MarketingCampaignDetail.aspx?campaignId=" + iCampaignId + "&pageindex=" + iPageIndex);
                }
            }
        }

        private void BindFilter()
        {
            MarketingCampaigns _bMarketingCampaigns = new MarketingCampaigns();
            DataTable dtSource = _bMarketingCampaigns.GetMarketingCategoryInfo();


            // add "All Lead Sources" row
            DataRow NewSourceRow = dtSource.NewRow();
            NewSourceRow["CategoryId"] = "-1";
            NewSourceRow["CategoryName"] = "All Categories";
            dtSource.Rows.InsertAt(NewSourceRow, 0);

            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "CategoryId";

            ddlCategory.DataSource = dtSource;
            ddlCategory.DataBind();
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
                    ViewState["orderName"] = "CategoryName";//CampaignName
                return Convert.ToString(ViewState["orderName"]);
            }
            set { ViewState["orderName"] = value; }
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
            set { ViewState["orderType"] = value; }
        }

        private void BindGridInfo(int pageIndex)
        {
            int pageSize = AspNetPager1.PageSize;
            int recordCount = 0;
            MarketingCampaigns _bMarketingCampaigns = new MarketingCampaigns();
            string sWhere = "1>0";
            if (ddlCategory.SelectedValue != "-1")
            {
                sWhere = "CategoryName='" + ddlCategory.SelectedItem.Text + "'";
            }
            DataSet ds = _bMarketingCampaigns.GetCampaignsList(pageSize, pageIndex, sWhere, out recordCount, OrderName, OrderType);

            AspNetPager1.RecordCount = recordCount;
            AspNetPager1.CurrentPageIndex = pageIndex;
            AspNetPager1.PageSize = pageSize;

            gridMarketingCampaign.DataSource = ds;
            gridMarketingCampaign.DataBind();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && iCampaignId==0)
            {
                //iFrmURL.Attributes.Add("src", GetCampaignUrl(ds.Tables[0].Rows[0]["CampaignDetailURL"]));
                //若该页面未传递ID，则默认显示第一条ID的Detail信息
                iCampaignId = Convert.ToInt32(ds.Tables[0].Rows[0]["CampaignId"]);
            }
        }

        protected string GetCampaignUrl(object objUrl)
        {
            string strUrl = string.Format("{0}", objUrl);
            int nIndexOfHttp = strUrl.IndexOf("http://", StringComparison.CurrentCultureIgnoreCase);
            if (nIndexOfHttp == 0)
                strUrl = strUrl.Substring(7); // 7 = length of "http://"
            if (strUrl != "")
                strUrl = string.Format("http://{0}", strUrl);
            else
                strUrl = "about:blank";
            return strUrl;
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            iPageIndex = AspNetPager1.CurrentPageIndex;
            BindGridInfo(AspNetPager1.CurrentPageIndex);
           
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindGridInfo(1);
        }

        protected void gridMarketingCampaign_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox tbID = (TextBox)e.Row.FindControl("tbID");
                if (tbID != null)
                {
                    e.Row.Attributes.Add("id", tbID.Text.ToString());
                    e.Row.Attributes.Add("onKeyDown", "SelectRow();");
                    e.Row.Attributes.Add("onClick", "MarkRow(" + tbID.Text.ToString() + ");");

                    int iID = Convert.ToInt32(tbID.Text);
                    if (iID == iCampaignId)
                    {
                        //Response.Write("<script type='text/javascript' language='javascript'>MarkRow(" + _i.ToString() + ");</script>");
                        _Queryi = _i;
                        bSel = true;
                    }
                    if (sListIDs.IndexOf("," + iID + ",") == -1)
                    {
                        sListIDs += iID + ",";

                    }
                }
                _i++;

            }


        }
    }
}
