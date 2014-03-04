using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Model;

namespace LPWeb.Layouts.LPWeb.LoanDetails
{
    public partial class LoanMarketingEventContent : LayoutsPageBase
    {
        private int LoanMarkettingEventId
        {
            get
            {
                if (Request.QueryString["eventid"] != null)
                {
                    return Convert.ToInt32(Request.QueryString["eventid"]);
                }
                else
                {
                    return 0;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDate();
            }
        }

        public void BindDate()
        {
            BLL.LoanMarketingEvents bllLoanME = new BLL.LoanMarketingEvents();

            LoanMarketingEvents modelLoanME = bllLoanME.GetModel(LoanMarkettingEventId);
            
            //lbTitle.Text =modelLoanME.
            lbContent.Text = modelLoanME.EventContent;
        }

    }
}
