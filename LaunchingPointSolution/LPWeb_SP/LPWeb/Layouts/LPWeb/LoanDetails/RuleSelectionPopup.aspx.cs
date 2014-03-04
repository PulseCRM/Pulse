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

public partial class LoanDetails_RuleSelectionPopup : BasePage
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
        this.RuleSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

        string sSql = "select * from Template_Rules where Enabled=1 and RuleId not in (select RuleId from LoanRules where Fileid=" + this.iLoanID.ToString() + " and RuleId is not null)"
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
                sSql = "SELECT * FROM Template_Rules WHERE Enabled=1 AND RuleId NOT IN (SELECT RuleId FROM LoanRules WHERE Fileid=" + this.iLoanID.ToString() + " AND RuleId IS NOT NULL)"
                    + " AND RuleScope = 0 AND (LoanTarget = 1 OR LoanTarget = 2)";
            }
        }

        this.RuleSqlDataSource.SelectCommand = sSql;
        this.gridRuleList.DataBind();

        #endregion
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string sSelectedRuleIDs = this.hdnSelectedRuleIDs.Value;
        string[] RuleIDArray = sSelectedRuleIDs.Split(',');

        LoginUser CurrentUser = new LoginUser();

        // insert
        LoanRules LoanRules1 = new LoanRules();
        LoanRules1.AddRuleToLoan(RuleIDArray, this.iLoanID, CurrentUser.iUserID);

        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Add rule to this loan successfully.');window.parent.location.href=window.parent.location.href;", true);
    }
}
