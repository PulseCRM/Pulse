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


public partial class Prospect_EmailLogAttachmentsPopup : BasePage
{

    int iEmailLogID = 0;
    string EmailLog_FileId = string.Empty;
    public string Sentlast = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        string sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";

        bool bIsValid = PageCommon.ValidateQueryString(this, "EmailLogID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", sCloseDialogCodes);
        }
        this.iEmailLogID = Convert.ToInt32(this.Request.QueryString["EmailLogID"]);

        #endregion

        #region 加载EmailLog

        DataTable EmailLogList = this.GetEmailLogInfo(this.iEmailLogID);
        if (EmailLogList.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid Email Log ID.", sCloseDialogCodes);
        }

        string sChainId = EmailLogList.Rows[0]["ChainId"].ToString();
        EmailLog_FileId = EmailLogList.Rows[0]["FileId"].ToString();
        Sentlast = EmailLogList.Rows[0]["LastSent"].ToString();


        LPWeb.BLL.EmailLog bllEmailLog = new EmailLog();

        var dt = bllEmailLog.GetEmailLogAttachments(iEmailLogID);

        gridList.DataSource = dt;
        gridList.DataBind();


        #endregion

        #region Out put file
        bIsValid = PageCommon.ValidateQueryString(this, "fileName", QueryStringType.String);
        if (bIsValid)
        {
            string fileName = this.Request.QueryString["fileName"].ToString();
            
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Name"].ToString().Trim() + "." + dr["FileType"].ToString().Trim() == fileName.Trim())
                {

                    byte[] file = (byte[])dr["FileImage"];

                    Response.Clear();
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = Encoding.Default;

                    string FileType = dr["FileType"].ToString().Trim();
                    string Name = dr["Name"].ToString().Trim();
                    // 文件扩展名
                    string Ext = "";
                    if (FileType == "Word" || FileType.ToLower() == "docx")
                    {

                        Ext = ".docx";
                    }
                    else if (FileType.ToLower() == "doc")
                    {

                        Ext = ".doc";
                    }
                    else if (FileType.ToUpper() == "XLS" || FileType.ToUpper() == "XLSX")
                    {

                        Ext = ".xlsx";
                    }
                    else if (FileType == "JPGE")
                    {

                        Ext = ".jpg";
                    }
                    else
                    {

                        Ext = "." + FileType.ToLower();
                    }

                    // 文件名
                    string sFileName = Name + Ext;

                    this.Response.Clear();
                    this.Response.ClearHeaders();
                    this.Response.Buffer = false;
                    if (FileType == "PDF")
                    {
                        this.Response.ContentType = "application/pdf";
                    }
                    else if (FileType == "Word")
                    {
                        this.Response.ContentType = "application/msword";
                    }
                    else if (FileType == "XLS")
                    {
                        this.Response.ContentType = "application/vnd.ms-excel";
                    }
                    else if (FileType == "ZIP")
                    {
                        this.Response.ContentType = "application/zip";
                    }
                    else
                    {
                        this.Response.ContentType = "application/octet-stream";
                    }

                    if (FileType == "ZIP" || FileType == "Word" || FileType == "XLS")
                    {
                        this.Response.AppendHeader("Content-Disposition", "attachment;filename=" + sFileName);
                    }
                    else
                    {
                        this.Response.AppendHeader("Content-Disposition", "inline;filename=" + sFileName);
                    }
                    this.Response.BinaryWrite(file);
                    this.Response.Flush();
                    Response.End();
                }
            }

        } 
        #endregion

    }

    public string GetBorrowerName()
    {
        LPWeb.BLL.Loans bll = new Loans();
        if (!string.IsNullOrEmpty(EmailLog_FileId))
        {
            return bll.GetLoanBorrowerName(Convert.ToInt32(EmailLog_FileId));
        }
        return string.Empty;
    }



    private DataTable GetEmailLogInfo(int iEmailLogID)
    {
        string sSql = "select * from EmailLog where EmailLogId=" + iEmailLogID;
        DataTable EmailLogInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        return EmailLogInfo;
    }

    public string GetSizeStr(object sizeObj)
    {
        if (sizeObj != null)
        {
            var size = Convert.ToDouble(sizeObj);

            if (size < 1024 * 10240.00)
            {
                return (size / 1024.00).ToString("0.00") + "KB";
            }
            else
            {
                return (size / 1024/1024.00).ToString("0.00") + "MB";
            }
        }

        return "0KB";
    }
}

