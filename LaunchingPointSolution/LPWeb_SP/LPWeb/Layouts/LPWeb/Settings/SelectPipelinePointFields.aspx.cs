using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using System.Data;

namespace LPWeb.Layouts.LPWeb.Settings
{
    public partial class SelectPipelinePointFields : LayoutsPageBase
    {
        string strCurrViewType = "";

        int nUserID = 0;
        string lastNamefilter = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // get current selected rows
            strCurrViewType = Request.QueryString["ViewType"];
           
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            if (strCurrViewType == "Loans")
            {
                BLL.CompanyLoanPointFields CompanyLoanPointFields = new BLL.CompanyLoanPointFields();
                DataTable dtCompanyLoanPointFieldsInfo = CompanyLoanPointFields.GetCompanyLoanPointFieldsInfo();
                this.gridPointFields.DataSource = dtCompanyLoanPointFieldsInfo;
                this.gridPointFields.DataBind();
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
           
            BindData();
        }


        protected void gridPointFields_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (DataControlRowType.DataRow == e.Row.RowType)
            {
                // set current selected row selected and disabled
                CheckBox ckbSelected = e.Row.FindControl("ckbSelected") as CheckBox;
                if (null != ckbSelected)
                {
                    //if (listCurrRow.Contains(gridPointFields.DataKeys[e.Row.RowIndex].Value.ToString()))
                    //{
                    //    ckbSelected.Checked = true;
                    //    //ckbSelected.Enabled = false;
                    //}
                }
            }
        }

       
    }
}
