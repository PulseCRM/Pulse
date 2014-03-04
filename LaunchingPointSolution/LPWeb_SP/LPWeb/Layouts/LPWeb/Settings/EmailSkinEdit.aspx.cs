using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data.SqlClient;
using System.Data;
using LPWeb.Common;

public partial class Settings_EmailSkinEdit : BasePage
{
    int iEmailSkinID = 0;
    DataTable EmailSkinInfo;
    LPWeb.BLL.Template_EmailSkins EmailSkinMrg = new LPWeb.BLL.Template_EmailSkins();

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验必要参数

        // EmailSkinID
        bool bIsValid = PageCommon.ValidateQueryString(this, "EmailSkinID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.parent.location.href='EmailSkinList.aspx'");
        }
        this.iEmailSkinID = Convert.ToInt32(this.Request.QueryString["EmailSkinID"]);

        #endregion

        #region 加载Email Skin Info

        this.EmailSkinInfo = EmailSkinMrg.GetEmailSkinInfo(this.iEmailSkinID);
        if (this.EmailSkinInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid email skin id.", "window.parent.location.href='EmailSkinList.aspx'");
        }

        #endregion

        if (this.IsPostBack == false)
        {
            #region bing data

            this.txtEmailSkinName.Text = this.EmailSkinInfo.Rows[0]["Name"].ToString();

            if (this.EmailSkinInfo.Rows[0]["Enabled"].ToString() == string.Empty)
            {
                this.chkEnabled.Checked = false;
            }
            else
            {
                this.chkEnabled.Checked = Convert.ToBoolean(this.EmailSkinInfo.Rows[0]["Enabled"]);
            }
            if (this.EmailSkinInfo.Rows[0]["Default"].ToString() == string.Empty)
            {
                this.chkDefault.Checked = false;
            }
            else
            {
                this.chkDefault.Checked = Convert.ToBoolean(this.EmailSkinInfo.Rows[0]["Default"]);
            }

            this.txtDesc.Text = this.EmailSkinInfo.Rows[0]["Desc"].ToString();
            this.txtHtmlBody.Text = this.EmailSkinInfo.Rows[0]["HTMLBody"].ToString();

            #endregion
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region 获取用户输入

        string sEmailSkinName = this.txtEmailSkinName.Text.Trim();
        string sDesc = this.txtDesc.Text.Trim();
        string sHtmlBody = this.txtHtmlBody.Text.Trim();
        bool bEnabled = this.chkEnabled.Checked;
        bool bDefault = this.chkDefault.Checked;

        #endregion

        #region 检查Email Skin Name重复

        bool bIsDup = EmailSkinMrg.IsDulplicated_Edit(this.iEmailSkinID, sEmailSkinName);
        if (bIsDup == true)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "__Dup", "alert('The Email Skin Name is duplicated, please enter another one.');", true);
            return;
        }

        #endregion

        // insert
        EmailSkinMrg.UpdateEmailSkin(this.iEmailSkinID, sEmailSkinName, sDesc, sHtmlBody, bEnabled, bDefault);

        // success
        PageCommon.WriteJsEnd(this, "Save email skin successfully.", "window.location.href='EmailSkinList.aspx'");
    }

    protected void btnClone_Click(object sender, EventArgs e) 
    {
        EmailSkinMrg.CloneEmailSkin(this.iEmailSkinID);

        PageCommon.WriteJsEnd(this, "Clone email skin successfully.", "window.location.href='EmailSkinList.aspx'");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {

        EmailSkinMrg.DeleteEmailSkin(this.iEmailSkinID);

        PageCommon.WriteJsEnd(this, "Delete email skin successfully.", "window.location.href='EmailSkinList.aspx'");
    }

}
