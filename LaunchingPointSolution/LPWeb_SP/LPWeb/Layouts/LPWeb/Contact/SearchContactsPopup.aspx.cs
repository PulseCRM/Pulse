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

public partial class SearchContactsPopup : BasePage
{
    Contacts bllContact = new Contacts();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (ddlState.Items.Count <= 0)
                USStates.Init(ddlState);
            BindServiceTypes();
        }
    }

    private void BindServiceTypes()
    {
        try
        {
            LPWeb.BLL.ServiceTypes st = new ServiceTypes();
            ddlServiceType.DataSource = st.GetList(" Enabled=1 ");
            ddlServiceType.DataBind();

            var item = new ListItem("All ", "0") { Selected = true };
            ddlServiceType.Items.Insert(0, item);
        }
        catch
        { }
    }

    private string GetCondition()
    {
        string strCondition = string.Empty;

        if (ddlContactType.SelectedIndex == 1)
        {
            strCondition += " AND (ContactId IN (SELECT Referral From Prospect) OR 	 ContactId IN (SELECT LoanContacts.ContactId FROM LoanContacts INNER JOIN ContactRoles ";
            strCondition += " ON LoanContacts.ContactRoleId = ContactRoles.ContactRoleId AND (ContactRoles.Name = 'Borrower' OR ContactRoles.Name = 'CoBorrower'))) ";
        }
        else if (ddlContactType.SelectedIndex == 2)
        {
            strCondition += " AND ContactId IN (SELECT ContactId From lpvw_PartnerContacts) ";
        }

        if (ddlServiceType.SelectedIndex > 0)
        {
            strCondition += " AND ServiceTypeId = '" + ddlServiceType.SelectedValue + "'";
        }

        if (txbCompany.Text.Trim().Length > 0)
        {
            strCondition += " AND CompanyName like '%" + txbCompany.Text.Trim() + "%'";
            strCondition += " AND ISNULL([Enabled],1)=1 ";
        }

        if (txbBranch.Text.Trim().Length > 0)
        {
            strCondition += " AND BranchName like '%" + txbBranch.Text.Trim() + "%'";
            strCondition += " AND ISNULL([Enabled],1)=1 ";
        }

        if (txbLastName.Text.Trim().Length > 0)
        {
            strCondition += " AND LastName like '%" + txbLastName.Text.Trim() + "%'";
            strCondition += " AND ISNULL([Enabled],1)=1 ";
        }

        if (txbAddress.Text.Trim().Length > 0)
        {
            strCondition += " AND MailingAddr like '%" + txbAddress.Text.Trim() + "%'";
            strCondition += " AND ISNULL([Enabled],1)=1 ";
        }

        if (txbCity.Text.Trim().Length > 0)
        {
            strCondition += " AND MailingCity like '%" + txbCity.Text.Trim() + "%'";
            strCondition += " AND ISNULL([Enabled],1)=1 ";
        }
        if (ddlState.SelectedIndex > 0)
        {
            strCondition += " AND MailingState = '" + ddlState.SelectedItem.Text.Trim() + "'";
            strCondition += " AND ISNULL([Enabled],1)=1 ";
        }
        return strCondition;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strWhere = GetCondition();
        //string strCondition = string.Empty;
        //try
        //{
        //    DataSet ds = bllContact.SearchContacts(strWhere);
        //    if (ds == null || ds.Tables.Count < 1)
        //    {
        //        strCondition = string.Empty;
        //    }

        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        strCondition += row["ContactID"].ToString() + ",";
        //    }
        //    if (strCondition.Length > 1)
        //    {
        //        strCondition = strCondition.TrimEnd(",".ToCharArray());
        //    }
        //}
        //catch
        //{ } AND ServiceTypeId

        if (this.Request.QueryString["from"] != null)
        {
            if (this.Request.QueryString["from"] == "TabPage")
            {
                PageCommon.WriteJsEnd(this, string.Empty, "  window.parent.InvokeFn('GetSearchCondition_TabPage', '" + LPWeb.Common.Encrypter.Base64Encode(strWhere) + "');");
            }
            else
            {
                PageCommon.WriteJsEnd(this, string.Empty, "  window.parent.InvokeFn('GetSearchCondition', '" + LPWeb.Common.Encrypter.Base64Encode(strWhere) + "');");
            }
        }
        else
        {
            PageCommon.WriteJsEnd(this, string.Empty, "window.parent.GetSearchCondition('" + LPWeb.Common.Encrypter.Base64Encode(strWhere) + "');");
            
        }
        

    }
}
