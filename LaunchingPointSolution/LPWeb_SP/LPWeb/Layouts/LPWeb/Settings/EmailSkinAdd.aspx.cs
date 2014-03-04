using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data.SqlClient;
using System.Data;
using LPWeb.Common;

public partial class Settings_EmailSkinAdd : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

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

        LPWeb.BLL.Template_EmailSkins EmailSkinMrg = new LPWeb.BLL.Template_EmailSkins();

        #region 检查Email Skin Name重复

        bool bIsDup = EmailSkinMrg.IsDulplicated_Add(sEmailSkinName);
        if (bIsDup == true)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "__Dup", "alert('The Email Skin Name is duplicated, please enter another one.');", true);
            return;
        }

        #endregion

        // insert
        EmailSkinMrg.InsertEmailSkin(sEmailSkinName, sDesc, sHtmlBody, bEnabled, bDefault);

        // success
        PageCommon.WriteJsEnd(this, "Save email skin successfully.", "window.location.href='EmailSkinList.aspx'");
    }

}
