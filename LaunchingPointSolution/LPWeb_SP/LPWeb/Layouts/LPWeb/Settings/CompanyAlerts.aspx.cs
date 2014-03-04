using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.Model;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;

public partial class Settings_CompanyAlerts : BasePage
{
    /// <summary>
    /// 
    /// </summary>
    private Company_Alerts modCompanyAlert = new Company_Alerts();
    LPWeb.BLL.Company_Alerts bllCompanyAlert = new LPWeb.BLL.Company_Alerts();
    private bool isNew = false;
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //权限验证
            var loginUser = new LoginUser();
            if (!loginUser.userRole.CompanySetup)
            {
                Response.Redirect("../Unauthorize.aspx");
                return;
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
        if (!Page.IsPostBack)
        {
            if (Request.UrlReferrer != null)
            {
                this.btnCancel.PostBackUrl = Request.UrlReferrer.ToString();
            }

            GetInitData();
            
            LPWeb.BLL.Template_Email tmpEmailBLL = new LPWeb.BLL.Template_Email();

            ddlTaskEmail.DataSource = tmpEmailBLL.GetEmailTemplate("  and Enabled = 1");
            ddlTaskEmail.DataBind();
            ddlTaskEmail.Items.Insert(0, new ListItem() { Text = "--select--", Value = "0" });

            if (modCompanyAlert != null)
            {
                SetDataToUI();
            }
        }

    }
    /// <summary>
    /// Gets the data from UI.
    /// </summary>
    private void GetDataFromUI()
    {
        GetInitData();
        if (modCompanyAlert == null)
        {
            modCompanyAlert = new Company_Alerts();
            isNew = true;
        }

        string alertRedDays = txtAlertRed.Text;
        if (!string.IsNullOrEmpty(alertRedDays))
        {
            modCompanyAlert.AlertRedDays = alertRedDays.Parse<int>();
        }

        string alertYellowDays = txtAlertYellow.Text;
        if (!string.IsNullOrEmpty(alertYellowDays))
        {
            modCompanyAlert.AlertYellowDays = alertYellowDays.Parse<int>();
        }

        string taskYellowDays = txtTaskYellow.Text;
        if (!string.IsNullOrEmpty(taskYellowDays))
        {
            modCompanyAlert.TaskYellowDays = taskYellowDays.Parse<int>();
        }

        string taskRedDays = txtTaskRed.Text;
        if (!string.IsNullOrEmpty(taskRedDays))
        {
            modCompanyAlert.TaskRedDays = taskRedDays.Parse<int>();
        }

        string rateLockYellowDays = txtRateLockYellowDays.Text;
        if (!string.IsNullOrEmpty(rateLockYellowDays))
        {
            modCompanyAlert.RateLockYellowDays = rateLockYellowDays.Parse<int>();
        }
        else
        {
            //fix default value
            modCompanyAlert.RateLockYellowDays = 7;
        }
        string rateLockRedDays = txtRateLockRedDays.Text;
        if (!string.IsNullOrEmpty(rateLockRedDays))
        {
            modCompanyAlert.RateLockRedDays = rateLockRedDays.Parse<int>();
        }
        else
        {
            //fix default value
            modCompanyAlert.RateLockRedDays = 5;
        }

        modCompanyAlert.SendEmailCustomTasks = cbSendMail.Checked;

        if (!string.IsNullOrEmpty(ddlTaskEmail.SelectedValue) && ddlTaskEmail.SelectedValue != "0")
        {
            try
            {
                modCompanyAlert.CustomTaskEmailTemplId = Convert.ToInt32(ddlTaskEmail.SelectedValue);
            }
            catch { modCompanyAlert.CustomTaskEmailTemplId = 0; }
        }

    }

    /// <summary>
    /// Gets the init data.
    /// </summary>
    private void GetInitData()
    {
        try
        {
            modCompanyAlert = bllCompanyAlert.GetModel();

        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
    }

    /// <summary>
    /// Sets the data to UI.
    /// </summary>
    private void SetDataToUI()
    {
        if (modCompanyAlert != null)
        {
            txtAlertRed.Text = modCompanyAlert.AlertRedDays.ToString();
            txtAlertYellow.Text = modCompanyAlert.AlertYellowDays.ToString();
            txtTaskYellow.Text = modCompanyAlert.TaskYellowDays.ToString();
            txtTaskRed.Text = modCompanyAlert.TaskRedDays.ToString();
            txtRateLockYellowDays.Text = modCompanyAlert.RateLockYellowDays.ToString();
            txtRateLockRedDays.Text = modCompanyAlert.RateLockRedDays.ToString();
            cbSendMail.Checked = modCompanyAlert.SendEmailCustomTasks;

            ddlTaskEmail.SelectedValue = modCompanyAlert.CustomTaskEmailTemplId.ToString();

        }
    }
    /// <summary>
    /// Saves this instance.
    /// </summary>
    /// <returns></returns>
    private bool Save()
    {
        bool status = false;
        try
        {
            if (isNew)
            {
                bllCompanyAlert.Add(modCompanyAlert);
            }
            else
            {
                bllCompanyAlert.Update(modCompanyAlert);
            }
            status = true;
        }
        catch (Exception exception)
        {
            status = false;
            LPLog.LogMessage(exception.Message);
        }
        return status;
    }

    /// <summary>
    /// Handles the Click event of the btnSave control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            return;
        }
        GetDataFromUI();
        if (Save() == true)
        {
            //todo:display successful message
            PageCommon.WriteJsEnd(this, "Saved successfully.", PageCommon.Js_RefreshSelf);
        }
        else
        {
            //todo:display faild message
            PageCommon.WriteJsEnd(this, "Failed to save.", PageCommon.Js_RefreshSelf);
        }
        SetDataToUI();
    }

    /// <summary>
    /// Handles the Click event of the btnCancel control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
}