using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.BLL;
using LPWeb.Common;
using System.Data;

namespace LPWeb.Layouts.LPWeb.Contact
{
    public partial class SelectContactsCompanyPopup : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBind();
            }
        }

        public void DataBind()
        {

            BLL.ServiceTypes bllServiceType = new BLL.ServiceTypes();

            ddlServiceType.DataSource = bllServiceType.GetList(" [Enabled] = 1 ");
            ddlServiceType.DataTextField = "Name";
            ddlServiceType.DataValueField = "ServiceTypeId";
            ddlServiceType.DataBind();

            var item = new ListItem("All ", "0") { Selected = true };
            ddlServiceType.Items.Insert(0, item);

            if (ddlState.Items.Count <= 0)
                USStates.Init(ddlState);
        }


        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btSearch_Click(object sender, EventArgs e)
        {
            string strWhere = " 1=1 ";

            if (!string.IsNullOrEmpty(ddlServiceType.SelectedValue) && ddlServiceType.SelectedValue.Trim() != "0")
            {
                strWhere += " AND ServiceTypeid = " + ddlServiceType.SelectedValue.Trim();
            }

            if (!string.IsNullOrEmpty(txbCompany.Text.Trim()))
            {
                strWhere += " AND CompanyName like '%" + txbCompany.Text.Trim() + "%'";
            }

            if (!string.IsNullOrEmpty(txbBranch.Text.Trim()))
            {
                strWhere += " AND BranchName like '%" + txbBranch.Text.Trim() + "%'";
            }


            if (!string.IsNullOrEmpty(txbCity.Text.Trim()))
            {
                strWhere += " AND City like '%" + txbCity.Text.Trim() + "%'";
            }


            if (!string.IsNullOrEmpty(ddlState.SelectedValue.Trim()))
            {
                strWhere += " AND State = '" + ddlState.SelectedValue.Trim() + "'";
            }


            BLL.ContactCompanies bllContactCom = new ContactCompanies();

            var dt = bllContactCom.Search(strWhere);

            gvCompanyList.DataSource = dt;
            gvCompanyList.DataBind();
        }
    }
}
