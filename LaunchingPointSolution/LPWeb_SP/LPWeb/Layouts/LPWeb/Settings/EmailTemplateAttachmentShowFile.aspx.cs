using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.BLL;
using System.Data;

public partial class Settings_EmailTemplateAttachmentShowFile : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        bool bValid = PageCommon.ValidateQueryString(this, "TemplAttachId", QueryStringType.ID);
        if (bValid == false)
        {
            this.Response.End();
        }
        int TemplAttachId = Convert.ToInt32(this.Request.QueryString["TemplAttachId"]);

        #endregion

        #region 加载Email Template信息

        Template_Email_Attachments Template_Email_AttachmentsMgr = new Template_Email_Attachments();
        LPWeb.Model.Template_Email_Attachments AttchModel = Template_Email_AttachmentsMgr.GetModel(TemplAttachId);
        if (AttchModel == null)
        {
            PageCommon.WriteJsEnd(this, "Invalid email attachment id.", "window.close();");
        }

        

        #endregion

        // 文件扩展名
        string Ext = "";
        if (AttchModel.FileType == "Word") {

            Ext = ".docx";
        }
        else if (AttchModel.FileType == "XLS") {

            Ext = ".xlsx";
        }
        else if (AttchModel.FileType == "JPGE") {

            Ext = ".jpg";
        }
        else {

            Ext = "." + AttchModel.FileType.ToLower();
        }

        // 文件名
        string sFileName = AttchModel.Name + Ext;

        this.Response.Clear();
        this.Response.ClearHeaders();
        this.Response.Buffer = false;
        if (AttchModel.FileType == "PDF")
        {
            this.Response.ContentType = "application/pdf";
        }
        else if (AttchModel.FileType == "Word")
        {
            this.Response.ContentType = "application/msword";
        }
        else if (AttchModel.FileType == "XLS")
        {
            this.Response.ContentType = "application/vnd.ms-excel";
        }
        else if (AttchModel.FileType == "ZIP")
        {
            this.Response.ContentType = "application/zip";
        }
        else
        {
            this.Response.ContentType = "application/octet-stream";
        }

        if (AttchModel.FileType == "ZIP" || AttchModel.FileType == "Word" || AttchModel.FileType == "XLS")
        {
            this.Response.AppendHeader("Content-Disposition", "attachment;filename=" + sFileName);
        }
        else
        {
            this.Response.AppendHeader("Content-Disposition", "inline;filename=" + sFileName);
        }
        this.Response.BinaryWrite(AttchModel.FileImage);
        this.Response.Flush();

        //if (AttchModel.FileType == "Word" || AttchModel.FileType == "XLS" || AttchModel.FileType == "ZIP")
        //{
        //    PageCommon.WriteJsEnd(this, "", PageCommon.Js_CloseWindow);
        //}
    }
}