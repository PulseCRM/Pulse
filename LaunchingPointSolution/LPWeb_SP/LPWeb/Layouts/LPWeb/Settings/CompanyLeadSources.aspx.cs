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

public partial class Settings_CompanyLeadSources : BasePage
    {
        Company_Lead_Sources bllLeadSource = new Company_Lead_Sources();
        Users bllUsers = new Users();
        User2LeadSource bllUser2LeadSource = new User2LeadSource();
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
                   // this.btnCancel.PostBackUrl = Request.UrlReferrer.ToString();
                }

                FillDataGrid(string.Empty);
            }
        }

        /// <summary>
        /// Fills the data grid.
        /// </summary>
        /// <param name="condition">The condition.</param>
        private void FillDataGrid(string condition)
        {
            List<LPWeb.Model.Company_Lead_Sources> LeadSourceses = null;
            try
            {
                LeadSourceses = bllLeadSource.GetModelList(condition);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            if (LeadSourceses != null && LeadSourceses.Count == 0)
            {
                if (gvLeadSourceses.ShowFooter)
                {
                    LeadSourceses.Add(new LPWeb.Model.Company_Lead_Sources());
                }
            }
            gvLeadSourceses.DataSource = LeadSourceses;
            gvLeadSourceses.DataBind();
            if (LeadSourceses != null && LeadSourceses.Count > 0)
            {
                if (LeadSourceses[0].LeadSource == null)
                {
                    gvLeadSourceses.Rows[0].Visible = false;
                }
            }
        }
    /// <summary>
    /// 在Grid中添加一行
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            if (gvLeadSourceses.ShowFooter)
            {
                lbtnAdd.Text = "Add";
                lbtnAdd.ValidationGroup = "AddLeadSource";
                gvLeadSourceses.ShowFooter = false;
            }
            else
            {
                lbtnAdd.Text = "Cancel";
                lbtnAdd.ValidationGroup = "none";
                lbtnAdd.CausesValidation = true;
                gvLeadSourceses.ShowFooter = true;
            }

            FillDataGrid(string.Empty);
        }

    /// <summary>
    /// 删除选中行的数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        protected void lbtnDel_Click(object sender, EventArgs e)
        {
            string leadSouceIds = hfdChkIds.Value;
            try
            {
                if (leadSouceIds.Length > 0)
                {
                    bllLeadSource.Delete(leadSouceIds);
                    bllUser2LeadSource.Delete(leadSouceIds);
                }
               
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
            FillDataGrid(string.Empty);
        }

    /// <summary>
    /// 添加一条新的数据到数据库
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        protected void btnAddSave_Click(object sender, EventArgs e)
        {
            var tbxLeadSource = gvLeadSourceses.FooterRow.FindControl("tbxLeadSource") as TextBox;
            var ddlDefaultUser = gvLeadSourceses.FooterRow.FindControl("ddlUsers") as DropDownList;
            var cbxDefault = gvLeadSourceses.FooterRow.FindControl("cbxDefaultStr") as CheckBox;

            var record = new LPWeb.Model.Company_Lead_Sources
            {
                LeadSource = tbxLeadSource.Text.Trim(),
                Default = cbxDefault.Checked,
                DefaultUserId = int.Parse(ddlDefaultUser.SelectedValue),
            };

            try
            {
            //更新Default
            bllLeadSource.UpdateDefault(cbxDefault.Checked);

            bllLeadSource.Add(record);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
            gvLeadSourceses.ShowFooter = false;
            lbtnAdd.Text = "Add";
            FillDataGrid(string.Empty);

        }

    /// <summary>
    /// 把这一行变为编辑模式
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        protected void gvLeadSourceses_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //Set the edit index.
            gvLeadSourceses.EditIndex = e.NewEditIndex;
            //Bind data to the GridView control.
            FillDataGrid(string.Empty);
        }
    /// <summary>
    /// 绑定每一行的数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        protected void gvLeadSourceses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (((DropDownList)e.Row.FindControl("ddlDefaultUser")) != null)
            {
                DropDownList ddlDefaultUser = (DropDownList)e.Row.FindControl("ddlDefaultUser");

                BindDefaultUser(ddlDefaultUser);

                //  选中 DropDownList
                if (((HiddenField) e.Row.FindControl("hfDefaultUser")).Value.Length > 0)
                {
                    ddlDefaultUser.SelectedValue = ((HiddenField)e.Row.FindControl("hfDefaultUser")).Value;
                }
            }

            if (((DropDownList)e.Row.FindControl("ddlUsers")) != null)
            {
                DropDownList ddlDefaultUser = (DropDownList)e.Row.FindControl("ddlUsers");
               
                BindDefaultUser(ddlDefaultUser);
            }
        }

        private void BindDefaultUser(DropDownList ddlDefaultUser)
        {

            System.Data.DataTable tableUser = null;

            //  生成 DropDownList 的值，绑定数据
            try
            {
                tableUser = bllUsers.GetUserListBuRoles("'Loan Officer', 'Branch Manager','Executive'");
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
            ddlDefaultUser.DataSource = tableUser;
            ddlDefaultUser.DataTextField = "UserName";
            ddlDefaultUser.DataValueField = "UserId";
            ddlDefaultUser.DataBind();
        }

    /// <summary>
    /// 修改数据到数据库
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        protected void gvLeadSourceses_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int leadSourceID = 0;
            if (gvLeadSourceses.DataKeys[e.RowIndex] != null)
            {
                leadSourceID = int.Parse(gvLeadSourceses.DataKeys[e.RowIndex].Values[0].ToString());
            }

            string leadSource = ((TextBox)gvLeadSourceses.Rows[e.RowIndex].FindControl("txtLeadSource")).Text;
            int defaultUserId = int.Parse(((DropDownList)gvLeadSourceses.Rows[e.RowIndex].FindControl("ddlDefaultUser")).SelectedValue);
            bool isDefault = ((CheckBox)gvLeadSourceses.Rows[e.RowIndex].FindControl("cbxDefaultStr")).Checked;

            bllLeadSource.UpdateDefault(isDefault);

            LPWeb.Model.Company_Lead_Sources leadSourceModel = new LPWeb.Model.Company_Lead_Sources();
            leadSourceModel.LeadSourceID = leadSourceID;
            leadSourceModel.LeadSource = leadSource;
            leadSourceModel.DefaultUserId = defaultUserId;
            leadSourceModel.Default = isDefault;

             try
             {
                bllLeadSource.Update(leadSourceModel);
             }
             catch (Exception exception)
             {
                 LPLog.LogMessage(exception.Message);
             }

            gvLeadSourceses.EditIndex = -1;
            FillDataGrid(string.Empty);
        }
    }

