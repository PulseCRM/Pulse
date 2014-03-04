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

public partial class LoanDetails_RuleGroupSelectionPopup : BasePage
{
    int iLoanID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 检查必要参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing", "$('#divContainer').hide();alert('Missing required query string.');window.parent.CloseDialog_AddRule();", true);
            return;
        }

        this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

        //Get rule num by loan id
        LPWeb.BLL.LoanRules loanRulesMgr = new LPWeb.BLL.LoanRules();
        System.Data.DataSet ds = loanRulesMgr.GetList(" Fileid=" + this.iLoanID.ToString());
        int iRuleCount = ds.Tables[0].Rows.Count;
        this.hdnAddRuleNum.Value = (20 - iRuleCount).ToString();
        #endregion

        #region 初始化Rule列表

        this.RuleGroupSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

        string sSql = "select * from Template_RuleGroups where Enabled=1 and RuleGroupId not in (select RuleGroupId from LoanRules where Fileid=" + this.iLoanID.ToString() + " and RuleGroupId is not null)"
                    + " AND RuleScope = 0 AND (LoanTarget = 0 OR LoanTarget = 2)";
        //
        //Check prospect loan
        if (iLoanID != 0)
        {
            LPWeb.BLL.Loans loanMgr = new LPWeb.BLL.Loans();
            LPWeb.Model.Loans loanModel = loanMgr.GetModel(iLoanID);
            if (loanModel.Status.Trim().ToLower() == "prospect")
            {
                this.hdnProspectLoan.Value = "true";
                sSql = "SELECT * FROM Template_RuleGroups WHERE Enabled=1 and RuleGroupId NOT IN (SELECT RuleGroupId FROM LoanRules WHERE Fileid=" + this.iLoanID.ToString() + " AND RuleGroupId IS NOT NULL)"
                    + " AND RuleScope = 0 AND (LoanTarget = 1 OR LoanTarget = 2)";
            }
        }

        this.RuleGroupSqlDataSource.SelectCommand = sSql;
        this.gridRuleGroupList.DataBind();

        #endregion
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string sSelectedRuleGroupIDs = this.hdnSelectedRuleGroupIDs.Value;
        string[] RuleGroupIDArray = sSelectedRuleGroupIDs.Split(',');

        LoginUser CurrentUser = new LoginUser();

        // insert
        LoanRules LoanRules1 = new LoanRules();
        LoanRules1.AddRuleGroupToLoan(RuleGroupIDArray, this.iLoanID, CurrentUser.iUserID);

        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Add rule group to this loan successfully.');window.parent.CloseDialog_AddRule();window.parent.location.href=window.parent.location.href;", true);
    }
}
