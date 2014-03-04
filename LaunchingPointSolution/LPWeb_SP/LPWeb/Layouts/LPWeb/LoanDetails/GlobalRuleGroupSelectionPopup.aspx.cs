using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using System.Data;

public partial class LoanDetails_GlobalRuleGroupSelectionPopup : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 初始化Rule列表

        Template_RuleGroups RuleGroupManager = new Template_RuleGroups();
        DataTable RuleGroupList = RuleGroupManager.GetNonGlobalRuleGroupList();
        this.gridRuleGroupList.DataSource = RuleGroupList;
        this.gridRuleGroupList.DataBind();

        #endregion
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string sSelectedRuleGroupIDs = this.hdnSelectedRuleGroupIDs.Value;
        
        // update
        Template_RuleGroups RuleGroupManager = new Template_RuleGroups();
        RuleGroupManager.AddGlobalRuleGroups(sSelectedRuleGroupIDs);

        PageCommon.WriteJsEnd(this, "Added global rule group successfully.", "window.parent.location.href=window.parent.location.href;");
    }
}
