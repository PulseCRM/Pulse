using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;

public partial class LoanDetails_GlobalRuleSelectionPopup : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 初始化Rule列表

        Template_Rules RuleManager = new Template_Rules();
        DataTable RuleList = RuleManager.GetNonGlobalRuleList();
        this.gridRuleList.DataSource = RuleList;
        this.gridRuleList.DataBind();

        #endregion
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string sSelectedRuleIDs = this.hdnSelectedRuleIDs.Value;

        // update
        Template_Rules RuleManager = new Template_Rules();
        RuleManager.AddGlobalRules(sSelectedRuleIDs);

        PageCommon.WriteJsEnd(this, "Added global rule successfully.", "window.parent.location.href=window.parent.location.href;");
    }
}
