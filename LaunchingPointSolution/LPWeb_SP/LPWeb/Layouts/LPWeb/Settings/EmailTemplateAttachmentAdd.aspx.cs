using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.BLL;
using System.Data;
using System.IO;

public partial class Settings_EmailTemplateAttachmentAdd : BasePage
{
    public int iEmailTemplateID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        bool bValid = PageCommon.ValidateQueryString(this, "EmailTemplateID", QueryStringType.ID);
        if (bValid == false)
        {
            this.Response.End();
        }
        this.iEmailTemplateID = Convert.ToInt32(this.Request.QueryString["EmailTemplateID"]);

        #endregion

        if (this.IsPostBack == false)
        {
            #region 加载Email Template List

            Template_Email Template_EmailMgr = new Template_Email();
            DataTable EmailTemplateList = Template_EmailMgr.GetEmailTemplate(" and Enabled=1");
            this.ddlEmailTemplate.DataSource = EmailTemplateList;
            this.ddlEmailTemplate.DataBind();

            // default selected
            this.ddlEmailTemplate.SelectedValue = this.iEmailTemplateID.ToString();

            #endregion
        }
    }

    protected void btnSave_Click(object sender, EventArgs e) 
    {
        SaveAttachment();

        // success
        PageCommon.WriteJsEnd(this, "Add attachment successfully.", "window.top.RefreshEmailTemplateAttachmentList();window.top.CloseDialog();");
    }

    protected void btnSaveAnother_Click(object sender, EventArgs e)
    {
        SaveAttachment();

        // success and another
        PageCommon.WriteJsEnd(this, "Add attachment successfully.", "window.top.RefreshEmailTemplateAttachmentList();window.location.href=window.location.href");
    }

    private void SaveAttachment() 
    {
        if (this.FileUpload1.HasFile == false)
        {
            PageCommon.WriteJsEnd(this, "Please select a valid file.", PageCommon.Js_RefreshSelf);
        }

        string sMsg = string.Empty;
        bool bValid = PageCommon.ValidateUpload(this, this.FileUpload1, 1024 * 1024 * 20, out sMsg, ".gif", ".jpg", ".jpeg", ".pdf", ".png", ".doc", ".docx", ".xls", ".xlsx", ".zip");
        if (bValid == false)
        {
            PageCommon.WriteJsEnd(this, sMsg, PageCommon.Js_RefreshSelf);
        }

        string sSelEmailTemplateID = this.ddlEmailTemplate.SelectedValue;
        string sAttachName = this.txtAttchName.Text.Trim();
        string sFileType = this.ddlFileType.SelectedValue;

        LPWeb.Model.Template_Email_Attachments AttachModel = new LPWeb.Model.Template_Email_Attachments();
        AttachModel.TemplEmailId = Convert.ToInt32(sSelEmailTemplateID);
        AttachModel.Enabled = true;
        AttachModel.Name = sAttachName;
        AttachModel.FileType = sFileType;
        AttachModel.FileImage = this.FileUpload1.FileBytes;

        Template_Email_Attachments Template_Email_AttachmentsMgr = new Template_Email_Attachments();
        Template_Email_AttachmentsMgr.Add(AttachModel);
    }
}
