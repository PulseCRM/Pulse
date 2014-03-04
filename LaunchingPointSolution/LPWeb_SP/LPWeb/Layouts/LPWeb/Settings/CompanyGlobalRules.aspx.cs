using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
 
public partial class CompanyGlobalRules : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 加载Rule List and RuleGroup List

        DataTable RuleListData = this.GetGlabalRuleList();

        this.gridRuleList.DataSource = RuleListData;
        this.gridRuleList.DataBind();

        #endregion

        // Add thead and tbody
        PageCommon.MakeGridViewAccessible(this.gridRuleList);
    }

    protected void lnkDisable_Click(object sender, EventArgs e)
    {
        // build ids
        string sRuleGroupIDs;
        string sRuleIDs;
        this.BuildRuleIDs(out sRuleGroupIDs, out sRuleIDs);

        // disable
        try
        {
            this.DisableRuleAndRuleGroup(sRuleGroupIDs, sRuleIDs);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Exception happened when disable rule(s) and rule group(s): {0}", ex.Message);
            //LPLog.LogMessage(LogType.Logerror, ex.Message);

            PageCommon.WriteJsEnd(this, "Exception happened when disable rule(s) and rule group(s).", PageCommon.Js_RefreshSelf);
        }


        // success
        PageCommon.WriteJsEnd(this, "Disable selected rule(s) and rule group(s) successfully.", PageCommon.Js_RefreshSelf);
    }

    protected void lnkRemove_Click(object sender, EventArgs e)
    {
        // build ids
        string sRuleGroupIDs;
        string sRuleIDs;
        this.BuildRuleIDs(out sRuleGroupIDs, out sRuleIDs);

        // remove
        try
        {
            this.RemoveRuleAndRuleGroup(sRuleGroupIDs, sRuleIDs);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Exception happened when remove rule(s) and rule group(s): {0}", ex.Message);
            //LPLog.LogMessage(LogType.Logerror, ex.Message);

            PageCommon.WriteJsEnd(this, "Exception happened when remove rule(s) and rule group(s).", PageCommon.Js_RefreshSelf);
        }


        // success
        PageCommon.WriteJsEnd(this, "Remove selected rule(s) and rule group(s) successfully.", PageCommon.Js_RefreshSelf);
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        // build ids
        string sRuleGroupIDs;
        string sRuleIDs;
        this.BuildRuleIDs(out sRuleGroupIDs, out sRuleIDs);

        // remove
        try
        {
            this.DeleteRuleAndRuleGroup(sRuleGroupIDs, sRuleIDs);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Exception happened when delete rule(s) and rule group(s): {0}", ex.Message);
            //LPLog.LogMessage(LogType.Logerror, ex.Message);

            PageCommon.WriteJsEnd(this, "Exception happened when delete rule(s) and rule group(s).", PageCommon.Js_RefreshSelf);
        }


        // success
        PageCommon.WriteJsEnd(this, "Delete selected rule(s) and rule group(s) successfully.", PageCommon.Js_RefreshSelf);
    }

    /// <summary>
    /// get global rule and rule group list
    /// neo 2011-03-18
    /// </summary>
    /// <returns></returns>
    private DataTable GetGlabalRuleList() 
    {
        string sSql = "select 'Rule Group' as RuleType, RuleGroupId as RuleID, Name as RuleName, '' as AlertEmail, case when Enabled=1 then 'Yes' else 'No' end as [Enabled], 0 as AlertEmailTemplID "
                    + "from Template_RuleGroups where RuleScope=1 "
                    + "union "
                    + "select 'Rule' as RuleType, RuleID, a.Name as RuleName, b.Name as AlertEmail, case when a.Enabled=1 then 'Yes' else 'No' end as [Enabled], a.AlertEmailTemplID from Template_Rules as a "
                    + "left outer join Template_Email as b on a.AlertEmailTemplId = b.TemplEmailId "
                    + "where a.RuleScope=1";

        DataTable RuleListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        return RuleListData;
    }

    /// <summary>
    /// build rule and rule group id
    /// neo 2011-03-18
    /// </summary>
    /// <param name="sRuleGroupIDs"></param>
    /// <param name="sRuleIDs"></param>
    private void BuildRuleIDs(out string sRuleGroupIDs, out string sRuleIDs)
    {
        sRuleGroupIDs = string.Empty;
        sRuleIDs = string.Empty;

        string sIDs = this.hdnSelectedRuleIDs.Value;
        string sTypes = this.hdnSelectedRuleTypes.Value;

        string[] IDArray = sIDs.Split(',');
        string[] TypeArray = sTypes.Split(',');

        StringBuilder sbRuleIDs = new StringBuilder();
        StringBuilder sbRuleGroupIDs = new StringBuilder();

        for (int i = 0; i < IDArray.Length; i++)
        {
            string sID = IDArray[i];
            string sType = TypeArray[i];

            if (sType == "Rule")
            {
                if (sbRuleIDs.Length == 0)
                {
                    sbRuleIDs.Append(sID);
                }
                else
                {
                    sbRuleIDs.Append("," + sID);
                }
            }
            else
            {
                if (sbRuleGroupIDs.Length == 0)
                {
                    sbRuleGroupIDs.Append(sID);
                }
                else
                {
                    sbRuleGroupIDs.Append("," + sID);
                }
            }
        }

        sRuleGroupIDs = sbRuleGroupIDs.ToString();
        sRuleIDs = sbRuleIDs.ToString();
    }

    /// <summary>
    /// disable rule and rule group
    /// neo 2011-03-18
    /// </summary>
    /// <param name="sRuleGroupIDs"></param>
    /// <param name="sRuleIDs"></param>
    private void DisableRuleAndRuleGroup(string sRuleGroupIDs, string sRuleIDs)
    {
        string sSql = string.Empty;
        if (sRuleGroupIDs != string.Empty)
        {
            sSql += "update Template_RuleGroups set [Enabled]=0 where RuleGroupId in (" + sRuleGroupIDs + ");";
        }

        if (sRuleIDs != string.Empty)
        {
            sSql += "update Template_Rules set [Enabled]=0 where RuleId in (" + sRuleIDs + ")";
        }

        LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
    }

    /// <summary>
    /// remove rule and rule group
    /// neo 2011-03-18
    /// </summary>
    /// <param name="sRuleGroupIDs"></param>
    /// <param name="sRuleIDs"></param>
    private void RemoveRuleAndRuleGroup(string sRuleGroupIDs, string sRuleIDs)
    {
        string sSql = string.Empty;
        if (sRuleGroupIDs != string.Empty)
        {
            sSql += "update Template_RuleGroups set RuleScope=0 where RuleGroupId in (" + sRuleGroupIDs + ");";
        }

        if (sRuleIDs != string.Empty)
        {
            sSql += "update Template_Rules set RuleScope=0 where RuleId in (" + sRuleIDs + ")";
        }

        LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
    }

    /// <summary>
    /// delete rule and rule group
    /// neo 2011-03-18
    /// </summary>
    /// <param name="sRuleGroupIDs"></param>
    /// <param name="sRuleIDs"></param>
    private void DeleteRuleAndRuleGroup(string sRuleGroupIDs, string sRuleIDs)
    {
        string sSql = string.Empty;
        if (sRuleGroupIDs != string.Empty)
        {
            sSql += "delete from Template_RuleGroups where RuleGroupId in (" + sRuleGroupIDs + ");";
        }

        if (sRuleIDs != string.Empty)
        {
            sSql += "delete from Template_Rules where RuleId in (" + sRuleIDs + ")";
        }

        LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
    }

    protected void lbtnEmpty_Click(object sender, EventArgs e)
    {

    }
}
