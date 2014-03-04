using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShortLeadForm
{
    public partial class _Default : System.Web.UI.Page
    {
        public string LoanOfficerLastName = string.Empty;
        public string LoanOfficerFirstName = string.Empty;
        public string LeadSource = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Request.QueryString["LoanOfficerLastName"] != null)
                {
                    LoanOfficerLastName = Request.QueryString["LoanOfficerLastName"].ToString().Trim();
                }

                if (Request.QueryString["LoanOfficerFirstName"] != null)
                {
                    LoanOfficerFirstName = Request.QueryString["LoanOfficerFirstName"].ToString().Trim();
                }
                if (Request.QueryString["LeadSource"] != null)
                {
                    LeadSource = Request.QueryString["LeadSource"].ToString().Trim();
                }
            }
            catch { }
        }
    }
}
