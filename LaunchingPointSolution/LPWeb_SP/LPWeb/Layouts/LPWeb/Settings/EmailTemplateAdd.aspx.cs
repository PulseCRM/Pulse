using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using System.Text.RegularExpressions;
using LPWeb.Common;
using Telerik.Web.UI;
using System.IO;

public partial class Settings_EmailTemplateAdd : BasePage
{
    Roles RoleManager = new Roles();
    Template_Email EmailTemplateManager = new Template_Email();

    protected void Page_Load(object sender, EventArgs e)
    {
        

        // is post back
        this.hdnIsPostBack.Text = this.IsPostBack.ToString();

        if (this.IsPostBack == false)
        {
            

            #region 加载User Roles

            string sWhere = " and RoleID not in (1,2)";     // 不包括Executive和Branch Manager
            DataTable RoleList = this.RoleManager.GetRoleList(sWhere);

            DataRow EmptyRoleRow = RoleList.NewRow();
            EmptyRoleRow["RoleID"] = 0;
            EmptyRoleRow["Name"] = "-- select a role --";
            RoleList.Rows.InsertAt(EmptyRoleRow, 0);

            DataView RoleView = new DataView(RoleList);
            RoleView.Sort = "Name";
            this.ddlFromUserRoles.DataSource = RoleView;
            this.ddlFromUserRoles.DataBind();

            #endregion

            #region 加载Recipient Roles

            DataTable RecipeintRoleList = this.RoleManager.GetRecipientRoleList();
            DataView RecipeintRoleView = new DataView(RecipeintRoleList);
            RecipeintRoleView.Sort = "RecipientRole";
            this.gridRecipientRoleList.DataSource = RecipeintRoleView;
            this.gridRecipientRoleList.DataBind();

            #endregion

            #region 加载Email Skins

            DataTable EmailSkinList = this.GetEmailSkinList(" and Enabled=1", "Name");

            DataRow EmptyEmailSkinRow = EmailSkinList.NewRow();
            EmptyEmailSkinRow["EmailSkinId"] = 0;
            EmptyEmailSkinRow["Name"] = "-- select an Email Skin --";
            EmailSkinList.Rows.InsertAt(EmptyEmailSkinRow, 0);

            this.ddlEmailSkin.DataSource = EmailSkinList;
            this.ddlEmailSkin.DataBind();

            #endregion
        }

        #region 初始化ToList/CCList

        this.InitRecipientList();

        #endregion
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sEmailTemplateName = this.txtEmailTemplateName.Text.Trim();
        string sDesc = this.txtDesc.Text.Trim();
        bool Enabled = this.chkEnabled.Checked;
        string sEmailSkinID = this.ddlEmailSkin.SelectedItem.Value;
        int iEmailSkinID = Convert.ToInt32(sEmailSkinID);
        string sSubject = this.txtSubject.Text.Trim();
        string sFromUserRole = this.ddlFromUserRoles.SelectedItem.Value;
        int iFromUserRoleID = Convert.ToInt32(sFromUserRole);
        string sUserDefinedEmail = this.txtFromEmail.Text.Trim();
        string sBody = this.RadEditor1.GetHtml(EditorStripHtmlOptions.None).Trim();
        if (sBody == "<BR>")
        {
            sBody = string.Empty;
        }

        string sSenderName = this.txtSenderName.Text.Trim();

        string sEmailList_To = this.hdnToEmailList.Text;
        string sContactList_To = this.hdnToContactList.Text;
        string sUserRoleList_To = this.hdnToUserRoleList.Text;

        string sEmailList_CC = this.hdnCCEmailList.Text;
        string sContactList_CC = this.hdnCCContactList.Text;
        string sUserRoleList_CC = this.hdnCCUserRoleList.Text;

        string sTaskOwnerChecked_To = this.hdnToTaskOwnerChecked.Text;
        string sTaskOwnerChecked_CC = this.hdnCCTaskOwnerChecked.Text;

        #region 检查是否重复

        bool bIsExist = this.EmailTemplateManager.IsExist_Create(sEmailTemplateName);
        if (bIsExist == true)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Duplicate", "$('#divContainer').hide();alert('The email template name has been taken. Please use a different name.');$('#divContainer').show();", true);
            return;
        }

        #endregion

        #region build string - Contact Role IDs (To)

        string sContactRoleIDs_To = string.Empty;

        if (sContactList_To != string.Empty)
        {
            string[] ContactRoleArray = Regex.Split(sContactList_To, ";");
            foreach (string sContactRoleIDName in ContactRoleArray)
            {
                string[] IDNamArray = sContactRoleIDName.Split(':');
                string sContactRoleID = IDNamArray[0];
                string sContactRoleName = IDNamArray[1];

                if (sContactRoleIDs_To == string.Empty)
                {
                    sContactRoleIDs_To = sContactRoleID;
                }
                else
                {
                    sContactRoleIDs_To += ";" + sContactRoleID;
                }
            }
        }

        #endregion

        #region build string - User Role IDs (To)

        string sUserRoleIDs_To = string.Empty;

        if (sUserRoleList_To != string.Empty)
        {
            string[] UserRoleArray = Regex.Split(sUserRoleList_To, ";");
            foreach (string sUserRoleIDName in UserRoleArray)
            {
                string[] IDNamArray = sUserRoleIDName.Split(':');
                string sUserRoleID = IDNamArray[0];
                string sUserRoleName = IDNamArray[1];

                if (sUserRoleIDs_To == string.Empty)
                {
                    sUserRoleIDs_To = sUserRoleID;
                }
                else
                {
                    sUserRoleIDs_To += ";" + sUserRoleID;
                }
            }
        }

        #endregion

        #region build string - Contact Role IDs (CC)

        string sContactRoleIDs_CC = string.Empty;

        if (sContactList_CC != string.Empty)
        {
            string[] ContactRoleArray_CC = Regex.Split(sContactList_CC, ";");
            foreach (string sContactRoleIDName in ContactRoleArray_CC)
            {
                string[] IDNamArray = sContactRoleIDName.Split(':');
                string sContactRoleID = IDNamArray[0];
                string sContactRoleName = IDNamArray[1];

                if (sContactRoleIDs_CC == string.Empty)
                {
                    sContactRoleIDs_CC = sContactRoleID;
                }
                else
                {
                    sContactRoleIDs_CC += ";" + sContactRoleID;
                }
            }
        }

        #endregion

        #region build string - User Role IDs (CC)

        string sUserRoleIDs_CC = string.Empty;

        if (sUserRoleList_CC != string.Empty)
        {
            string[] UserRoleArray_CC = Regex.Split(sUserRoleList_CC, ";");
            foreach (string sUserRoleIDName in UserRoleArray_CC)
            {
                string[] IDNamArray = sUserRoleIDName.Split(':');
                string sUserRoleID = IDNamArray[0];
                string sUserRoleName = IDNamArray[1];

                if (sUserRoleIDs_CC == string.Empty)
                {
                    sUserRoleIDs_CC = sUserRoleID;
                }
                else
                {
                    sUserRoleIDs_CC += ";" + sUserRoleID;
                }
            }
        }

        #endregion

        // insert email template
        this.EmailTemplateManager.InsertEmailTemplate(sEmailTemplateName, sDesc, iFromUserRoleID, sUserDefinedEmail, sBody, sSubject, sEmailList_To, sUserRoleIDs_To, sContactRoleIDs_To, sEmailList_CC, sUserRoleIDs_CC, sContactRoleIDs_CC, sTaskOwnerChecked_To, sTaskOwnerChecked_CC, this.chkLeadCreated.Checked, sSenderName, iEmailSkinID, Enabled);

        // success
        PageCommon.WriteJsEnd(this, "Create email template successfully.", "window.parent.location.href = 'EmailTemplateList.aspx';");
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        this.Response.Clear();
        this.Response.ClearHeaders();
        this.Response.Buffer = false;
        this.Response.ContentType = "application/octet-stream";
        this.Response.AppendHeader("Content-Disposition", "attachment;filename=email-template.html");
        this.Response.AppendHeader("Content-Length", this.RadEditor1.Content.Length.ToString());
        this.Response.ContentEncoding = System.Text.Encoding.UTF8;
        this.Response.Write(this.RadEditor1.Content);
        this.Response.Flush();
        this.Response.End();

    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        if (this.FileUpload1.HasFile == false)
        {
            PageCommon.WriteJsEnd(this, "Please select a html file.", "window.parent.location.href = window.parent.location.href;");
        }

        string sMsg = string.Empty;
        bool bValid = PageCommon.ValidateUpload(this, this.FileUpload1, 1024 * 1024 * 15, out sMsg, ".html", ".htm");
        if (bValid == false)
        {
            PageCommon.WriteJsEnd(this, sMsg, "window.parent.location.href = window.parent.location.href;");
        }

        using (StreamReader reader = new StreamReader(this.FileUpload1.FileContent))
        {
            string sHtml = reader.ReadToEnd();

            this.RadEditor1.Content = sHtml;
        }

        //this.Response.Flush();
    }

    private void InitRecipientList()
    {
        DataTable ToList = new DataTable();
        DataTable CCList = new DataTable();

        #region Add Columns

        ToList.Columns.Add("Type", typeof(string));
        ToList.Columns.Add("Email", typeof(string));
        ToList.Columns.Add("RoleID", typeof(int));
        ToList.Columns.Add("RoleName", typeof(string));

        CCList.Columns.Add("Type", typeof(string));
        CCList.Columns.Add("Email", typeof(string));
        CCList.Columns.Add("RoleID", typeof(int));
        CCList.Columns.Add("RoleName", typeof(string));

        #endregion

        #region bind data

        this.gridToList.DataSource = ToList;
        this.gridToList.DataBind();

        this.gridCCList.DataSource = CCList;
        this.gridCCList.DataBind();

        #endregion
    }

    private DataTable GetEmailSkinList(string sWhere, string sOrderby) 
    {
        Template_EmailSkins x = new Template_EmailSkins();
        return x.GetEmailSkinList(sWhere, sOrderby);
    }
}
