using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.Model;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;


public partial class Settings_CompanyReport : BasePage
    {
        /// <summary>
        /// 
        /// </summary>
        private Company_Report modCompanyReport = new Company_Report();
        LPWeb.BLL.Company_Report bllCompanyReport = new LPWeb.BLL.Company_Report();
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
            try
            {
                if (!Page.IsPostBack)
                {
                    if (Request.UrlReferrer != null)
                    {
                        this.btnCancel.PostBackUrl = Request.UrlReferrer.ToString();
                    }

                    GetInitData();
                    if (modCompanyReport != null)
                    {
                        SetDataToUI();
                    }
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

        }
        /// <summary>
        /// Gets the data from UI.
        /// </summary>
        private void GetDataFromUI()
        {
            //get existing record
            GetInitData();
            if (modCompanyReport == null)
            {
                modCompanyReport = new Company_Report();
                isNew = true;
            }

            modCompanyReport.DOW = Convert.ToInt32(ddlDOW.SelectedValue);
            modCompanyReport.TOD = Convert.ToInt32(ddlTOD.SelectedValue);
            modCompanyReport.SenderRoleId = Convert.ToInt32(ddlRole.SelectedValue);
            modCompanyReport.SenderEmail = tbSenderEmail.Text;
            modCompanyReport.SenderName = tbSenderName.Text;
        }

        /// <summary>
        /// Gets the init data.
        /// </summary>
        private void GetInitData()
        {
            try
            {
                modCompanyReport = bllCompanyReport.GetModel();

                LPWeb.BLL.Roles broles=new LPWeb.BLL.Roles();
                DataSet dsRole = broles.GetList("1>0 order by Name");
                if (dsRole.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsRole.Tables[0].Rows)
                    {
                        ddlRole.Items.Add(new ListItem(dr["Name"].ToString(), dr["RoleId"].ToString()));
                    }
                }
            }
            catch (Exception exception)
            {
                modCompanyReport = null;
                LPLog.LogMessage(exception.Message);
            }
        }

        /// <summary>
        /// Sets the data to UI.
        /// </summary>
        private void SetDataToUI()
        {
            if (modCompanyReport == null)
            {
                return;
            }
            ddlDOW.SelectedValue = modCompanyReport.DOW.ToString();
            ddlTOD.SelectedValue = modCompanyReport.TOD.ToString();
            ddlRole.SelectedValue = modCompanyReport.SenderRoleId.ToString();
            tbSenderEmail.Text = modCompanyReport.SenderEmail;
            tbSenderName.Text = modCompanyReport.SenderName;

            if (ddlRole.SelectedValue != "0")
            {
                tbSenderEmail.Enabled = false;
                tbSenderName.Enabled = false;
            }
            else
            {
                tbSenderEmail.Enabled = true;
                tbSenderName.Enabled = true;
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
                    bllCompanyReport.Add(modCompanyReport);
                }
                else
                {
                    bllCompanyReport.Update(modCompanyReport);
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
                //display successful message
                PageCommon.WriteJsEnd(this, "Saved successfully.", PageCommon.Js_RefreshSelf);
            }
            else
            {
                //display faild message
                PageCommon.WriteJsEnd(this, "Failed to save the record.", PageCommon.Js_RefreshSelf);
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

