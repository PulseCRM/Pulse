using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using LPWeb.Common;

public partial class Settings_EmailTemplateAttachmentList : BasePage
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

        #region 排序

        string sOrderByField = "Name";
        if (this.Request.QueryString["OrderByField"] != null)
        {
            sOrderByField = this.Request.QueryString["OrderByField"];
        }

        string sOrderByType = string.Empty;
        if (this.Request.QueryString["OrderByType"] != null)
        {
            string OrderByTypeNum = this.Request.QueryString["OrderByType"];
            if (OrderByTypeNum != "0" && OrderByTypeNum != "1")
            {
                OrderByTypeNum = "0";
            }

            if (OrderByTypeNum == "0")
            {
                sOrderByType = " asc ";
            }
            else if (OrderByTypeNum == "1")
            {
                sOrderByType = " desc ";
            }
        }

        #endregion

        #region 加载Email Attachment List

        string sSql = "select * from Template_Email_Attachments where TemplEmailId=" + this.iEmailTemplateID + " order by " + sOrderByField + sOrderByType;
        DataTable AttachmentList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        this.gridAttachmentList.DataSource = AttachmentList;
        this.gridAttachmentList.DataBind();

        #endregion

    }
}
