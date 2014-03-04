using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;

public partial class Contact_PartnerCompanyAdd : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsPostBack == false)
        {
            #region 加载Service Type Filter

            ServiceTypes ServiceTypeManager = new ServiceTypes();
            DataTable ServiceTypeList = ServiceTypeManager.GetServiceTypeList(" and (Enabled=1)");
            DataRow NewServiceTypeRow = ServiceTypeList.NewRow();
            NewServiceTypeRow["ServiceTypeId"] = DBNull.Value;
            NewServiceTypeRow["Name"] = "-- select --";
            ServiceTypeList.Rows.InsertAt(NewServiceTypeRow, 0);
            ServiceTypeList.AcceptChanges();

            this.ddlServiceType.DataSource = ServiceTypeList;
            this.ddlServiceType.DataBind();

            #endregion
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sCompanyName = this.txtCompanyName.Text.Trim();
        string sServiceTypeID = this.ddlServiceType.SelectedValue;
        int iServiceTypeID = Convert.ToInt32(sServiceTypeID);
        string sServiceType = this.ddlServiceType.SelectedItem.Text;
        string sAddress = this.txtAddress.Text.Trim();
        string sCity = this.txtCity.Text.Trim();
        string sState = this.ddlState.SelectedValue;
        string sZip = this.txtZip.Text.Trim();
        string strWebsite = this.tbWebsite.Text.Trim();

        ContactCompanies ContactCompanyManager = new ContactCompanies();
        ContactCompanyManager.InsertContactCompany(sCompanyName, iServiceTypeID, sServiceType, sAddress, sCity, sState, sZip, strWebsite);

        PageCommon.WriteJsEnd(this, "Created partner company successfully.", "window.parent.location.href='PartnerCompanyList.aspx';");
    }
}