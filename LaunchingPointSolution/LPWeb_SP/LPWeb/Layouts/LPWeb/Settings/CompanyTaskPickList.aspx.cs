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
using System.Data.SqlClient;
using System.Data;



/// <summary>
/// Email template list
/// author: peter pan
/// date: 2010-12-10
/// </summary>
public partial class Settings_CompanyTaskPickList : BasePage
{
    CompanyTaskPickList bllTaskPickList = new CompanyTaskPickList();

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

            txtSequenceNumber.Text = (bllTaskPickList.GetMaxSequenceNumber() + 10).ToString();

            FillDataGrid(string.Empty);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            return;
        }

        var companyTaskPick = new LPWeb.Model.CompanyTaskPick();


        string TaskName = this.txtTaskName.Text.Trim();

        if (bllTaskPickList.Exists(TaskName))
        {
            PageCommon.WriteJsEnd(this, "The task name is already taken.", PageCommon.Js_RefreshSelf);

            return;
        }

        int SequenceNumber = int.Parse(this.txtSequenceNumber.Text.Trim());

        if (bllTaskPickList.ExistsSequenceNumber(SequenceNumber))
        {
            PageCommon.WriteJsEnd(this, "The task name is already taken.", PageCommon.Js_RefreshSelf);

            return;
        }




        var status = false;


        companyTaskPick.TaskName = TaskName;
        companyTaskPick.Enabled = true;
        companyTaskPick.SequenceNumber = SequenceNumber;
        try
        {
            bllTaskPickList.Add(companyTaskPick);
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
            PageCommon.WriteJsEnd(this, "Added successfully.", PageCommon.Js_RefreshSelf);
        }
        else
        {
            //todo:display faild message
            PageCommon.WriteJsEnd(this, "Failed to add the task.", PageCommon.Js_RefreshSelf);
        }


    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        var selctedStr = this.hfDeleteItems.Value;
        if (!string.IsNullOrEmpty(selctedStr))
        {
            string[] selectedItems = selctedStr.Split(',');
            //delete the selected items
            DeleteCompanyTaskPick(selectedItems);
            //reload the grid data
            PageCommon.WriteJsEnd(this, "Removed successfully.", PageCommon.Js_RefreshSelf);
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
        //List<LPWeb.Model.CompanyTaskPick> companyTaskPick = null;
        //try
        //{
        //    companyTaskPick = bllTaskPickList.GetModelList(condition);
        //}
        //catch (Exception exception)
        //{
        //    LPLog.LogMessage(exception.Message);
        //}

        //if (companyTaskPick != null && companyTaskPick.Count > 0)
        //{
        //    gvTaskName.DataSource = companyTaskPick;
        //    gvTaskName.DataBind();
        //}

        DataSet ds = bllTaskPickList.GetAllList();

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];


            gvTaskName.DataSource = dt;
            gvTaskName.DataBind();
        }



        PageCommon.MakeGridViewAccessible(this.gvTaskName);
    }

    /// <summary>
    /// Deletes the loan programs.
    /// </summary>
    /// <param name="items">The items.</param>
    private void DeleteCompanyTaskPick(string[] items)
    {

        foreach (string item in items)
        {

            try
            {
                bllTaskPickList.Delete(item);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
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

        foreach (string item in selectedItems)
        {

            try
            {
                var mod = bllTaskPickList.GetModel(item);
                if (mod != null)
                {
                    mod.Enabled = status;
                    bllTaskPickList.Update(mod);
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
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

