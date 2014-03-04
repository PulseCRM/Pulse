using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;

public partial class Settings_CompanyLeadStatus : BasePage
{
    ArchiveLeadStatus bllArchiveLeadStatus = new ArchiveLeadStatus();

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

            FillDataGrid(string.Empty);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            return;
        }

        var archiveLeadStatus = new LPWeb.Model.ArchiveLeadStatus();
        string leadStatusName = this.txtLeadSource.Text.Trim();
        var status = false;

        if (!string.IsNullOrEmpty(leadStatusName))
        {
            archiveLeadStatus.LeadStatusName = leadStatusName;
            archiveLeadStatus.Enabled = true;
            try
            {
                bllArchiveLeadStatus.Add(archiveLeadStatus);
                status = true;
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            if (status == true)
            {
                //reload the grid data
                FillDataGrid(string.Empty);
                //todo:display successfuly message
                PageCommon.WriteJsEnd(this, "Lead Status added successfully.", PageCommon.Js_RefreshSelf);
            }
            else
            {
                //todo:display faild message
                PageCommon.WriteJsEnd(this, "Failed to add the Lead Status.", PageCommon.Js_RefreshSelf);
            }
        }
        else
        {
            //todo:display the field can not be empty
            PageCommon.WriteJsEnd(this, "Lead Status cannot be empty.", PageCommon.Js_RefreshSelf);
        }

    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        var selctedStr = this.hfDeleteItems.Value;
        if (!string.IsNullOrEmpty(selctedStr))
        {
            string[] selectedItems = selctedStr.Split(',');
            //delete the selected items
            DeleteLeadStatus(selectedItems);
            //reload the grid data
            PageCommon.WriteJsEnd(this, "Record removed successfully.", PageCommon.Js_RefreshSelf);
        }
        FillDataGrid(string.Empty);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }


    /// <summary>
    /// Fills the data grid.
    /// </summary>
    /// <param name="condition">The condition.</param>
    private void FillDataGrid(string condition)
    {
        List<LPWeb.Model.ArchiveLeadStatus> archiveLeadStatuses = null;
        try
        {
            archiveLeadStatuses = bllArchiveLeadStatus.GetModelList(condition);
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

        if (archiveLeadStatuses != null && archiveLeadStatuses.Count > 0)
        {
            gvLeadStatus.DataSource = archiveLeadStatuses;
            gvLeadStatus.DataBind();
        }
        PageCommon.MakeGridViewAccessible(this.gvLeadStatus);
    }

    /// <summary>
    /// Deletes the loan programs.
    /// </summary>
    /// <param name="items">The items.</param>
    private void DeleteLeadStatus(string[] items)
    {
        int iItem = 0;
        foreach (var item in items)
        {
            if (int.TryParse(item, out iItem))
            {
                try
                {
                    bllArchiveLeadStatus.Delete(iItem);
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }
            }
        }
    }

    protected void btnDisable_Click(object sender, EventArgs e)
    {
        bool status = false;
        var selctedStr = this.hfDeleteItems.Value;
        if (!string.IsNullOrEmpty(selctedStr))
        {
            string[] selectedItems = selctedStr.Split(',');
            //delete the selected items
            UpdateLeadStatus(selectedItems, status);
            //reload the grid data
            PageCommon.WriteJsEnd(this, "Disabled successfully.", PageCommon.Js_RefreshSelf);
        }
        FillDataGrid(string.Empty);
    }

    private void UpdateLeadStatus(string[] selectedItems, bool status)
    {
        int iItem = 0;
        foreach (var item in selectedItems)
        {
            if (int.TryParse(item, out iItem))
            {
                try
                {
                    var mod = bllArchiveLeadStatus.GetModel(iItem);
                    if (mod != null)
                    {
                        mod.Enabled = status;
                        bllArchiveLeadStatus.Update(mod);
                    }
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }
            }
        }
    }

    protected void btnEnable_Click(object sender, EventArgs e)
    {
        bool status = true;
        var selctedStr = this.hfDeleteItems.Value;
        if (!string.IsNullOrEmpty(selctedStr))
        {
            string[] selectedItems = selctedStr.Split(',');
            //delete the selected items
            UpdateLeadStatus(selectedItems, status);
            //reload the grid data
            PageCommon.WriteJsEnd(this, "Enabled successfully.", PageCommon.Js_RefreshSelf);
        }
        FillDataGrid(string.Empty);
    }

}

