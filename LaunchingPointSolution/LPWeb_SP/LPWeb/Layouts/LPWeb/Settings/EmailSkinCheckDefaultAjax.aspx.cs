using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.Common;
using System.Data;

public partial class Settings_EmailSkinCheckDefaultAjax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // {"ExecResult":"Success","hasDefault":"true|false"}
        // {"ExecResult":"Failed","ErrorMsg":"error msg."}

        // 检查是否重复
        bool hasDefault = true;
        
        try
        {
            string sSql = "select count(1) from dbo.Template_EmailSkins where [Default]=1";
            int iCount = Convert.ToInt32(LPWeb.DAL.DbHelperSQL.ExecuteScalar(sSql));
            if (iCount == 0)
            {
                hasDefault = false;
            }
            else
            {
                hasDefault = true;
            }

            this.Response.Write("{\"ExecResult\":\"Success\",\"hasDefault\":\"" + hasDefault.ToString().ToLower() + "\"}");
        }
        catch (Exception)
        {
            string sErrorMsg = "Failed to check default email skin.";
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
        }
    }
}

