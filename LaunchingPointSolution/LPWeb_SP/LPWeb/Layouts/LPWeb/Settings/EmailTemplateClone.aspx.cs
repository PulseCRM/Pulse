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
using System.Text;
using LPWeb.Common;
using Telerik.Web.UI;
using System.IO;

public partial class Settings_EmailTemplateClone : BasePage
{
    int iEmailTemplateID = 0;

    Roles RoleManager = new Roles();
    Template_Email EmailTemplateManager = new Template_Email();

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验必要参数

        // EmailTemplateID
        bool bIsValid = PageCommon.ValidateQueryString(this, "EmailTemplateID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.parent.location.href=window.parent.location.href");
        }
        this.iEmailTemplateID = Convert.ToInt32(this.Request.QueryString["EmailTemplateID"]);

        #endregion

        #region 加载Email Template信息

        DataTable EmailTemplateInfo = this.EmailTemplateManager.GetEmailTemplateInfo(this.iEmailTemplateID);
        if (EmailTemplateInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid required query string.", "window.parent.location.href=window.parent.location.href");
        }

        #endregion

        // is post back
        this.hdnIsPostBack.Text = this.IsPostBack.ToString();

        if (this.IsPostBack == false)
        {
            
            this.InitRecipientList();

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

            #region 加载Email Skins

            DataTable EmailSkinList = this.GetEmailSkinList(" and Enabled=1", "Name");

            DataRow EmptyEmailSkinRow = EmailSkinList.NewRow();
            EmptyEmailSkinRow["EmailSkinId"] = 0;
            EmptyEmailSkinRow["Name"] = "-- select an Email Skin --";
            EmailSkinList.Rows.InsertAt(EmptyEmailSkinRow, 0);

            this.ddlEmailSkin.DataSource = EmailSkinList;
            this.ddlEmailSkin.DataBind();

            #endregion

            #region bind data

            this.txtEmailTemplateName.Text = "Copy of " + EmailTemplateInfo.Rows[0]["Name"].ToString();
            string sEnabled = EmailTemplateInfo.Rows[0]["Enabled"].ToString();
            if (sEnabled == "True")
            {
                this.chkEnabled.Checked = true;
            }
            else
            {
                this.chkEnabled.Checked = false;
            }
            this.txtDesc.Text = EmailTemplateInfo.Rows[0]["Desc"].ToString() == string.Empty ? string.Empty : "Copy of " + EmailTemplateInfo.Rows[0]["Desc"].ToString();
            
            string sEmailSkinID = EmailTemplateInfo.Rows[0]["EmailSkinId"].ToString();
            if (sEmailSkinID != string.Empty)
            {
                this.ddlEmailSkin.SelectedValue = sEmailSkinID;
            }
            
            this.txtSubject.Text = EmailTemplateInfo.Rows[0]["Subject"].ToString();
            string sFromUserRoleID = EmailTemplateInfo.Rows[0]["FromUserRoles"].ToString();
            if (sFromUserRoleID != string.Empty)
            {
                this.ddlFromUserRoles.SelectedValue = sFromUserRoleID;
                //this.txtFromEmail.ReadOnly = true;
            }
            string sFromEmail = EmailTemplateInfo.Rows[0]["FromEmailAddress"].ToString();
            if (sFromEmail != string.Empty)
            {
                this.txtFromEmail.Text = sFromEmail;
                this.ddlFromUserRoles.Enabled = false;
            }
            this.RadEditor1.Content = EmailTemplateInfo.Rows[0]["Content"].ToString();

            bool isLeadCread = false;
            if (!EmailTemplateInfo.Rows[0].IsNull("SendTrigger"))
            {
                string leadCread = EmailTemplateInfo.Rows[0]["SendTrigger"].ToString();
                if (leadCread.Equals("LeadCreated", StringComparison.CurrentCultureIgnoreCase))
                {
                    isLeadCread = true;
                }
            }

            chkLeadCreated.Checked = isLeadCread;

            this.txtSenderName.Text = EmailTemplateInfo.Rows[0]["SenderName"].ToString();

            #endregion

            #region 加载Recipient Roles for selection

            DataTable RecipeintRoleList = this.RoleManager.GetRecipientRoleList();
            DataView RecipeintRoleView = new DataView(RecipeintRoleList);
            RecipeintRoleView.Sort = "RecipientRole";
            this.gridRecipientRoleList.DataSource = RecipeintRoleView;
            this.gridRecipientRoleList.DataBind();

            #endregion

        }
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

        this.EmailTemplateManager.InsertEmailTemplate(sEmailTemplateName, sDesc, iFromUserRoleID, sUserDefinedEmail, sBody, sSubject, sEmailList_To, sUserRoleIDs_To, sContactRoleIDs_To, sEmailList_CC, sUserRoleIDs_CC, sContactRoleIDs_CC, sTaskOwnerChecked_To, sTaskOwnerChecked_CC, chkLeadCreated.Checked, sSenderName, iEmailSkinID, Enabled);

        PageCommon.WriteJsEnd(this, "Clone email template successfully.", "window.parent.location.href = 'EmailTemplateList.aspx';");
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

        // 加载recipient信息
        DataTable RecipientList = this.EmailTemplateManager.GetRecipientList(this.iEmailTemplateID);

        // 加载UserRoles
        DataTable UserRoles = this.RoleManager.GetRoleList(string.Empty);

        // 加载ContactRoles
        ContactRoles ContactRoleManager = new ContactRoles();
        DataTable ContactRoles = ContactRoleManager.GetContactRoleList(string.Empty);

        #region To

        this.hdnToTaskOwnerChecked.Text = "False";

        DataRow[] ToRecipient = RecipientList.Select("RecipientType='To'");
        if (ToRecipient.Length > 0)
        {
            foreach (DataRow ToRecipientRow in ToRecipient)
            {
                string sEmailList_To = ToRecipientRow["EmailAddr"].ToString();
                string sContactList_To = ToRecipientRow["ContactRoles"].ToString();
                string sUserRoleList_To = ToRecipientRow["UserRoles"].ToString();
                string sTaskOwnerChecked_To = ToRecipientRow["TaskOwner"].ToString();

                // TaskOwner=True
                if (sEmailList_To == string.Empty
                    && sContactList_To == string.Empty
                    && sUserRoleList_To == string.Empty
                    && sTaskOwnerChecked_To == "True")
                {
                    DataRow NewToListRow = ToList.NewRow();
                    NewToListRow["Type"] = "User";
                    NewToListRow["Email"] = DBNull.Value;
                    NewToListRow["RoleID"] = 0;
                    NewToListRow["RoleName"] = "Task Owner";
                    ToList.Rows.Add(NewToListRow);

                    // Task Owner Checked
                    this.hdnToTaskOwnerChecked.Text = "True";
                }
                else
                {
                    #region Email

                    if (sEmailList_To != string.Empty)
                    {
                        // set hidden
                        this.hdnToEmailList.Text = sEmailList_To;

                        string[] EmailArray_To = sEmailList_To.Split(';');
                        foreach (string sEmailTo in EmailArray_To)
                        {
                            DataRow NewEmailRowTo = ToList.NewRow();
                            NewEmailRowTo["Type"] = "Email";
                            NewEmailRowTo["Email"] = sEmailTo;
                            NewEmailRowTo["RoleID"] = DBNull.Value;
                            NewEmailRowTo["RoleName"] = DBNull.Value;
                            ToList.Rows.Add(NewEmailRowTo);
                        }
                    }

                    #endregion

                    #region Contact Role

                    if (sContactList_To != string.Empty)
                    {
                        StringBuilder sbContactListTo = new StringBuilder();
                        string[] ContactArray_To = sContactList_To.Split(';');
                        foreach (string sContactIDTo in ContactArray_To)
                        {
                            DataRow NewEmailRowTo = ToList.NewRow();
                            NewEmailRowTo["Type"] = "Contact";
                            NewEmailRowTo["Email"] = DBNull.Value;
                            NewEmailRowTo["RoleID"] = Convert.ToInt32(sContactIDTo);

                            DataRow[] ContactRoleRows = ContactRoles.Select("ContactRoleId=" + sContactIDTo);
                            NewEmailRowTo["RoleName"] = ContactRoleRows[0]["Name"];

                            ToList.Rows.Add(NewEmailRowTo);

                            #region build string for hidden

                            if (sbContactListTo.Length == 0)
                            {
                                sbContactListTo.Append(sContactIDTo + ":" + ContactRoleRows[0]["Name"].ToString());
                            }
                            else
                            {
                                sbContactListTo.Append(";" + sContactIDTo + ":" + ContactRoleRows[0]["Name"].ToString());
                            }

                            #endregion
                        }

                        // set hidden
                        this.hdnToContactList.Text = sbContactListTo.ToString();
                    }

                    #endregion

                    #region User Role

                    if (sUserRoleList_To != string.Empty)
                    {
                        StringBuilder sbUserRoleListTo = new StringBuilder();
                        string[] UserRoleArray_To = sUserRoleList_To.Split(';');
                        foreach (string sUserRoleIDTo in UserRoleArray_To)
                        {
                            DataRow NewEmailRowTo = ToList.NewRow();
                            NewEmailRowTo["Type"] = "User";
                            NewEmailRowTo["Email"] = DBNull.Value;
                            NewEmailRowTo["RoleID"] = Convert.ToInt32(sUserRoleIDTo);

                            DataRow[] UserRoleRoleRows = UserRoles.Select("RoleId=" + sUserRoleIDTo);
                            NewEmailRowTo["RoleName"] = UserRoleRoleRows[0]["Name"];

                            ToList.Rows.Add(NewEmailRowTo);

                            #region build string for hidden

                            if (sbUserRoleListTo.Length == 0)
                            {
                                sbUserRoleListTo.Append(sUserRoleIDTo + ":" + UserRoleRoleRows[0]["Name"].ToString());
                            }
                            else
                            {
                                sbUserRoleListTo.Append(";" + sUserRoleIDTo + ":" + UserRoleRoleRows[0]["Name"].ToString());
                            }

                            #endregion
                        }

                        // set hidden
                        this.hdnToUserRoleList.Text = sbUserRoleListTo.ToString();
                    }

                    #endregion
                }
            }
        }

        #endregion

        #region CC

        this.hdnCCTaskOwnerChecked.Text = "False";

        DataRow[] CCRecipient = RecipientList.Select("RecipientType='CC'");
        if (CCRecipient.Length > 0)
        {
            foreach (DataRow CCRecipientRow in CCRecipient)
            {
                string sEmailList_CC = CCRecipientRow["EmailAddr"].ToString();
                string sContactList_CC = CCRecipientRow["ContactRoles"].ToString();
                string sUserRoleList_CC = CCRecipientRow["UserRoles"].ToString();
                string sTaskOwnerChecked_CC = CCRecipientRow["TaskOwner"].ToString();

                // TaskOwner=True
                if (sEmailList_CC == string.Empty
                    && sContactList_CC == string.Empty
                    && sUserRoleList_CC == string.Empty
                    && sTaskOwnerChecked_CC == "True")
                {
                    DataRow NewCCListRow = CCList.NewRow();
                    NewCCListRow["Type"] = "User";
                    NewCCListRow["Email"] = DBNull.Value;
                    NewCCListRow["RoleID"] = 0;
                    NewCCListRow["RoleName"] = "Task Owner";
                    CCList.Rows.Add(NewCCListRow);

                    this.hdnCCTaskOwnerChecked.Text = "True";
                }
                else
                {
                    #region Email

                    if (sEmailList_CC != string.Empty)
                    {
                        // set hidden
                        this.hdnCCEmailList.Text = sEmailList_CC;

                        string[] EmailArray_CC = sEmailList_CC.Split(';');
                        foreach (string sEmailCC in EmailArray_CC)
                        {
                            DataRow NewEmailRowCC = CCList.NewRow();
                            NewEmailRowCC["Type"] = "Email";
                            NewEmailRowCC["Email"] = sEmailCC;
                            NewEmailRowCC["RoleID"] = DBNull.Value;
                            NewEmailRowCC["RoleName"] = DBNull.Value;
                            CCList.Rows.Add(NewEmailRowCC);
                        }
                    }

                    #endregion

                    #region Contact Role

                    if (sContactList_CC != string.Empty)
                    {
                        StringBuilder sbContactListCC = new StringBuilder();
                        string[] ContactArray_CC = sContactList_CC.Split(';');
                        foreach (string sContactIDCC in ContactArray_CC)
                        {
                            DataRow NewEmailRowCC = CCList.NewRow();
                            NewEmailRowCC["Type"] = "Contact";
                            NewEmailRowCC["Email"] = DBNull.Value;
                            NewEmailRowCC["RoleID"] = Convert.ToInt32(sContactIDCC);

                            DataRow[] ContactRoleRows = ContactRoles.Select("ContactRoleId=" + sContactIDCC);
                            NewEmailRowCC["RoleName"] = ContactRoleRows[0]["Name"];

                            CCList.Rows.Add(NewEmailRowCC);

                            #region build string for hidden

                            if (sbContactListCC.Length == 0)
                            {
                                sbContactListCC.Append(sContactIDCC + ":" + ContactRoleRows[0]["Name"].ToString());
                            }
                            else
                            {
                                sbContactListCC.Append(";" + sContactIDCC + ":" + ContactRoleRows[0]["Name"].ToString());
                            }

                            #endregion
                        }

                        // set hidden
                        this.hdnCCContactList.Text = sbContactListCC.ToString();
                    }

                    #endregion

                    #region User Role

                    if (sUserRoleList_CC != string.Empty)
                    {

                        StringBuilder sbUserRoleListCC = new StringBuilder();
                        string[] UserRoleArray_CC = sUserRoleList_CC.Split(';');
                        foreach (string sUserRoleIDCC in UserRoleArray_CC)
                        {
                            DataRow NewEmailRowCC = CCList.NewRow();
                            NewEmailRowCC["Type"] = "User";
                            NewEmailRowCC["Email"] = DBNull.Value;
                            NewEmailRowCC["RoleID"] = Convert.ToInt32(sUserRoleIDCC);

                            DataRow[] UserRoleRoleRows = UserRoles.Select("RoleId=" + sUserRoleIDCC);
                            NewEmailRowCC["RoleName"] = UserRoleRoleRows[0]["Name"];

                            CCList.Rows.Add(NewEmailRowCC);

                            #region build string for hidden

                            if (sbUserRoleListCC.Length == 0)
                            {
                                sbUserRoleListCC.Append(sUserRoleIDCC + ":" + UserRoleRoleRows[0]["Name"].ToString());
                            }
                            else
                            {
                                sbUserRoleListCC.Append(";" + sUserRoleIDCC + ":" + UserRoleRoleRows[0]["Name"].ToString());
                            }

                            #endregion
                        }

                        // set hidden
                        this.hdnCCUserRoleList.Text = sbUserRoleListCC.ToString();
                    }

                    #endregion
                }
            }
        }

        #endregion

        #region bind data

        this.gridToList.DataSource = ToList;
        this.gridToList.DataBind();

        this.gridCCList.DataSource = CCList;
        this.gridCCList.DataBind();

        #endregion
    }

    public string GetEmailOrRole(string sType, string sEmail, string sRoleName)
    {
        if (sType == "Email")
        {
            return sEmail;
        }
        else
        {
            return sRoleName;
        }
    }

    private DataTable GetEmailSkinList(string sWhere, string sOrderby)
    {
        Template_EmailSkins x = new Template_EmailSkins();
        return x.GetEmailSkinList(sWhere, sOrderby);
    }
}
