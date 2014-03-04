using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using LPModel = LPWeb.Model;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;
using System.Text;

public partial class Prospect_EmploymentDetailPopup : BasePage
{
    private LoginUser _curLoginUser = new LoginUser();
    string sCloseDialogCodes = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 检查页面参数
        // CloseDialogCodes
        bool bIsValid = PageCommon.ValidateQueryString(this, "CloseDialogCodes", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.parent.location.href=window.parent.location.href");
        }
        this.sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";

        // ContactID
        bIsValid = PageCommon.ValidateQueryString(this, "ContactID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", this.sCloseDialogCodes);
        }

        // when update, verify the EmployID
        if (!string.Equals(this.Request.QueryString["Action"], "Add", StringComparison.InvariantCultureIgnoreCase))
        {
            // EmployID
            bIsValid = PageCommon.ValidateQueryString(this, "EmployID", QueryStringType.ID);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, "Missing required query string.", this.sCloseDialogCodes);
            }
        }
        #endregion

        var strContractId = this.Request.QueryString["ContactID"];
        this.hfdContractId.Value = strContractId;

        if (this.IsPostBack == false)
        {
            int employmentId = -1;
            var strEmploymentId = this.Request.QueryString["EmployID"];
            employmentId = string.IsNullOrEmpty(strEmploymentId) ? -1 : Convert.ToInt32(strEmploymentId);
            this.hfdEmploymentId.Value = employmentId.ToString();
            // initialize the dropdownlist
            USStates.Init(ddlState);

            if (employmentId == -1)
                return;

            InitPage(employmentId);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        LPModel.ProspectEmployment model = CreateModelFromPage();
        if (hfdEmploymentId.Value == "-1")
            new ProspectEmployment().Add(model);
        else
        {
            new ProspectEmployment().Update(model);
            LPWeb.BLL.ProspectIncome bll = new ProspectIncome();
            bll.UpdateIncome(model.EmplId, string.IsNullOrEmpty(txtSalary.Text) ? "NULL" : txtSalary.Text);
        }

        StringBuilder sbJavaScript = new StringBuilder();
        sbJavaScript.AppendLine("$('#divContainer').hide();");
        sbJavaScript.AppendLine("alert('The Point file(s) has been updated successfully.'); CancelOnClick();");
        // success
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", sbJavaScript.ToString(), true);
    }

    private void InitPage(int employmentId)
    {
        // bind the controls in the page and datasource
        LPWeb.BLL.ProspectEmployment bll = new ProspectEmployment();

        DataTable employmentInfo = bll.GetEmployment(employmentId);

        if (employmentInfo == null || employmentInfo.Rows.Count == 0) return;

        txtEmployer.Text = employmentInfo.Rows[0]["EmploymentName"] == null ? string.Empty : employmentInfo.Rows[0]["EmploymentName"].ToString();
        txtPisition.Text = employmentInfo.Rows[0]["Position"] == null ? string.Empty : employmentInfo.Rows[0]["Position"].ToString();
        checkSelf.Checked = employmentInfo.Rows[0]["SelfEmployed"] != null && string.Equals(employmentInfo.Rows[0]["SelfEmployed"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase);
        txtStart.Text = (employmentInfo.Rows[0].IsNull("StartMonth") || employmentInfo.Rows[0].IsNull("StartYear")) ? string.Empty : employmentInfo.Rows[0]["StartMonth"].ToString() + "/" + employmentInfo.Rows[0]["StartYear"].ToString();
        txtEnd.Text = (employmentInfo.Rows[0].IsNull("EndMonth") || employmentInfo.Rows[0].IsNull("EndYear")) ? string.Empty : employmentInfo.Rows[0]["EndMonth"].ToString() + "/" + employmentInfo.Rows[0]["EndYear"].ToString();
        txtSalary.Text = employmentInfo.Rows[0]["Salary"] == null ? string.Empty : employmentInfo.Rows[0]["Salary"].ToString();
        txtPhone.Text = employmentInfo.Rows[0]["Phone"] == null ? string.Empty : employmentInfo.Rows[0]["Phone"].ToString();
        txtYearsExp.Text = employmentInfo.Rows[0]["YearsOnWork"] == null ? string.Empty : employmentInfo.Rows[0]["YearsOnWork"].ToString();
        txtAddress.Text = employmentInfo.Rows[0]["Address"] == null ? string.Empty : employmentInfo.Rows[0]["Address"].ToString();
        txtCity.Text = employmentInfo.Rows[0]["City"] == null ? string.Empty : employmentInfo.Rows[0]["City"].ToString();

        if (employmentInfo.Rows[0]["State"] == null)
            ddlState.SelectedIndex = -1;
        else
            ddlState.SelectedValue = employmentInfo.Rows[0]["State"].ToString();

        txtZip.Text = employmentInfo.Rows[0]["Zip"] == null ? string.Empty : employmentInfo.Rows[0]["Zip"].ToString();

        txtBusinessType.Text = employmentInfo.Rows[0]["BusinessType"] == null ? string.Empty : employmentInfo.Rows[0]["BusinessType"].ToString();

        checkVerify.Checked = employmentInfo.Rows[0]["VerifyYourTaxes"] != null && string.Equals(employmentInfo.Rows[0]["VerifyYourTaxes"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase);

        hfdBranchContractId.Value = employmentInfo.Rows[0]["ContactBranchId"] == null ? string.Empty : employmentInfo.Rows[0]["ContactBranchId"].ToString();
    }

    private LPModel.ProspectEmployment CreateModelFromPage()
    {
        LPModel.ProspectEmployment model = new LPModel.ProspectEmployment();

        model.CompanyName = this.txtEmployer.Text;
        if (hfdEmploymentId.Value != "-1")
            model.EmplId = Convert.ToInt32(hfdEmploymentId.Value);

        model.ContactId = Convert.ToInt32(hfdContractId.Value);
        if (!string.IsNullOrEmpty(hfdBranchContractId.Value))
            model.ContactBranchId = Convert.ToInt32(hfdBranchContractId.Value.ToString());

        model.SelfEmployed = checkSelf.Checked;
        model.Position = txtPisition.Text;
        if (string.IsNullOrEmpty(txtStart.Text))
        {
            model.StartMonth = null;
            model.StartYear = null;
        }
        else
        {
            model.StartMonth = Convert.ToDecimal(txtStart.Text.Split('/')[0]);
            model.StartYear = Convert.ToDecimal(txtStart.Text.Split('/')[1]);
        }

        if (string.IsNullOrEmpty(txtEnd.Text))
        {
            model.EndMonth = null;
            model.EndYear = null;
        }
        else
        {
            model.EndMonth = Convert.ToDecimal(txtEnd.Text.Split('/')[0]);
            model.EndYear = Convert.ToDecimal(txtEnd.Text.Split('/')[1]);
        }

        model.Phone = txtPhone.Text;
        if (!string.IsNullOrEmpty(txtYearsExp.Text))
        {
            model.YearsOnWork = Convert.ToDecimal(txtYearsExp.Text);
        }
        model.Address = txtAddress.Text;
        model.City = txtCity.Text;
        model.State = ddlState.SelectedValue;
        model.Zip = txtZip.Text;

        model.BusinessType = txtBusinessType.Text;
        model.VerifyYourTaxes = checkVerify.Checked;

        return model;
    }

    private void GetBranchInfo(string branchIDStr)
    {
        LPWeb.BLL.ContactCompanies bllContactCom = new ContactCompanies();

        DataTable dt = bllContactCom.SearchSingleContact(" ContactBranchId=" + branchIDStr);

        if (dt != null && dt.Rows.Count > 0)
        {
            txtEmployer.Text = dt.Rows[0]["Name"] == null ? string.Empty : dt.Rows[0]["Name"].ToString();
            txtPhone.Text = dt.Rows[0]["Phone"] == null ? string.Empty : dt.Rows[0]["Phone"].ToString();
            if (dt.Rows[0]["State"] == null)
                ddlState.SelectedIndex = -1;
            else
                ddlState.SelectedValue = dt.Rows[0]["State"].ToString();
            txtAddress.Text = dt.Rows[0]["Address"] == null ? string.Empty : dt.Rows[0]["Address"].ToString();
            txtCity.Text = dt.Rows[0]["City"] == null ? string.Empty : dt.Rows[0]["City"].ToString();
            txtZip.Text = dt.Rows[0]["Zip"] == null ? string.Empty : dt.Rows[0]["Zip"].ToString();
        }
    }
}