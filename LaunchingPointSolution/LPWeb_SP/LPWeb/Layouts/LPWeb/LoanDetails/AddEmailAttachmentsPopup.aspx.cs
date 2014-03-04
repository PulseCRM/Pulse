using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using System.Text;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using System.IO;


public partial class AddEmailAttachmentsPopup : BasePage
{
    private string sErrorMsg = "Failed to load current page: invalid TemplEmailId.";
    public string CloseDialogCodes = "window.parent.CloseGlobalPopup()";

    Dictionary<string, byte[]> Attachments = new Dictionary<string, byte[]>();
    int TemplEmailId = 0;

    string sLocalTime = string.Empty;

    string Token = "";
    bool isListTemplEmailAttach = false;

    public string Default_lbxAttachments = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["CloseDialogCodes"] != null) // 如果有LoanID
        {
            CloseDialogCodes = Request.QueryString["CloseDialogCodes"].ToString();
        }

        if (Request.QueryString["TemplEmailId"] != null) // 如果有LoanID
        {
            string sTemplEmailId = Request.QueryString["TemplEmailId"];

            if (PageCommon.IsID(sTemplEmailId) == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, CloseDialogCodes);
            }

            TemplEmailId = Convert.ToInt32(sTemplEmailId);
        }

        Token = Request.QueryString["token"] != null ? Request.QueryString["token"].ToString() : "";
        isListTemplEmailAttach = Request.QueryString["isListTemplEmailAttach"] != null ? Convert.ToBoolean(Request.QueryString["isListTemplEmailAttach"]) : false;

        if (!IsPostBack)
        {
            if (isListTemplEmailAttach)
            {
                LPWeb.BLL.Template_Email_Attachments bllTempEmailAttach = new Template_Email_Attachments();

                var ds = bllTempEmailAttach.GetListWithOutFileImage(" Enabled =1 AND TemplEmailId = " + TemplEmailId);
                StringBuilder sb = new StringBuilder();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sb.Append("<option value='s" + dr["TemplAttachId"].ToString() + "' >" + dr["Name"].ToString() + "." + dr["FileType"].ToString() + "</option>");
                    }
                }

                Default_lbxAttachments = sb.ToString();
            }
        }



        ///判定是不是 在执行发送
        if (Request.Form["hidIsSend"] != null)
        {
            sLocalTime = Request.Form["hfLocalTime"].ToString().Trim();
            //Attachment 

            #region Attachments
            if (Request.Files.Count > 0)
            {

                foreach (var key in Request.Files.AllKeys)
                {
                    var file = Request.Files[key];

                    if (file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
                    {

                        if (file.ContentLength > 1024 * 1024 * 20)
                        {
                            ErrorMsg("The file you tried to upload has exceeded the allowed limit, 20MB.");
                            Response.End();
                        }

                        byte[] bytes = new byte[file.InputStream.Length];
                        file.InputStream.Read(bytes, 0, bytes.Length);
                        file.InputStream.Seek(0, SeekOrigin.Begin);

                        FileInfo info = new FileInfo(file.FileName);

                        Attachments.Add(info.Name, bytes);
                    }

                }
            }
            #endregion

            if (Attachments.Count == 0)
            {
                ErrorMsg("Attachments / Note is empty.");
                Response.End();
            }


            #region Save file

            try
            {
                LPWeb.BLL.Email_AttachmentsTemp bllEmailAttachTemp = new Email_AttachmentsTemp();
                foreach (var item in Attachments)
                {
                    LPWeb.Model.Email_AttachmentsTemp model = new LPWeb.Model.Email_AttachmentsTemp();

                    model.Token = Token;
                    model.Name = item.Key.ToString().Substring(0, item.Key.ToString().LastIndexOf('.'));
                    model.FileType = item.Key.ToString().Substring(item.Key.ToString().LastIndexOf('.') + 1);
                    model.FileImage = item.Value;

                   bllEmailAttachTemp.Add(model);
                }

                ErrorMsg(true, "");
            }
            catch (Exception ex)
            {
                ErrorMsg(ex.Message.Replace("\"", "").Replace("\\n", ""));  
            }

            #endregion


            Response.End();
        }

    }


    private void ErrorMsg(bool IsSusccess, string msg)
    {
        Response.Write("<script>window.parent.SendOK(" + (IsSusccess ? "1" : "0") + " , '" + msg + "');</script>");
        //Response.End();
    }

    private void ErrorMsg(string msg)
    {
        ErrorMsg(false, msg);
    }
}
