using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.BLL;
using System.Data;

public partial class LoanDetails_LoanDetailsRuleSetupTab : BasePage
{
    int iLoanID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        var loginUser = new LoginUser();
        //权限验证
        if (!loginUser.userRole.ApplyAlertRule)
        {
            Response.Redirect("../Unauthorize1.aspx");
            return;
        }
        #region 检查必要参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "FileID", QueryStringType.ID);
        if (bIsValid == false)
        {
            try
            {
                this.Response.End();
            }
            catch
            {

            }
        }

        this.iLoanID = Convert.ToInt32(this.Request.QueryString["FileID"]);

        #endregion

        #region 获取Loan Status

        Loans LoanManager = new Loans();
        DataTable LoanInfo = LoanManager.GetLoanInfo(this.iLoanID);
        string sLoanStatus = LoanInfo.Rows[0]["Status"].ToString();
        this.hdnLoanStatus.Value = sLoanStatus;
        if (sLoanStatus.Trim().ToLower() == "prospect")
        {
            this.hdnProspectLoanStatus.Value = LoanInfo.Rows[0]["ProspectLoanStatus"].ToString();
        }

        #endregion

        #region 初始化Rule列表

        this.RuleSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

        string sSql = "select a.*, case when a.RuleGroupId is null then 'Rule' else 'Rule Group' end as RuleType, case when a.RuleGroupId is null then b.Name else c.Name end as 'RuleName' from LoanRules as a left outer join Template_Rules as b on a.RuleId = b.RuleId "
                    + "left outer join Template_RuleGroups as c on a.RuleGroupId = c.RuleGroupId "
                    + "where a.Fileid = " + this.iLoanID;

        this.RuleSqlDataSource.SelectCommand = sSql;
        this.gridRuleList.DataBind();
        if (this.gridRuleList.Rows.Count >= 20)
        {
            this.hdnRuleNum.Value = "true";
        }
        else
        { 
        }

        #endregion
    }
}
