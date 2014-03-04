using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;

public partial class Settings_RuleTemplateSelection : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 初始化Rule列表

        string sSql = "select * from Template_Rules where Enabled=1";
        DataTable RuleList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        this.gridRuleList.DataSource = RuleList;
        this.gridRuleList.DataBind();

        #endregion
    }
}
