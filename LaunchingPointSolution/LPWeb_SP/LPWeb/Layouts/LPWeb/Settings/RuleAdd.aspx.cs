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
using System.Data;
using System.Text.RegularExpressions;

public partial class Settings_RuleAdd : BasePage
{
    protected string strNeedReturn = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["needret"]))
            strNeedReturn = "0";
        else
            strNeedReturn = "1";

        if (this.IsPostBack == false)
        {
            #region 加载email template
            LoanTasks LoanTaskManager = new LoanTasks();
            Template_Email EmailTempManager = new Template_Email();
            DataTable EmailTemplates = EmailTempManager.GetEmailTemplate(" and Enabled = 1");

            DataRow NoneEmailTemplateRow = EmailTemplates.NewRow();
            NoneEmailTemplateRow["TemplEmailId"] = 0;
            NoneEmailTemplateRow["Name"] = "-- select an email template --";
            EmailTemplates.Rows.InsertAt(NoneEmailTemplateRow, 0);

            this.ddlRecomActionTemplate.DataSource = EmailTemplates;
            this.ddlRecomActionTemplate.DataBind();

            this.ddlAlertEmailTemplate.DataSource = EmailTemplates;
            this.ddlAlertEmailTemplate.DataBind();

            #endregion
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region get rule data

        string sRuleName = this.txtRuleName.Text.Trim();
        string sDesc = this.txtDesc.Text.Trim();
        if (string.IsNullOrEmpty(sDesc))
        {
            sDesc = txtRuleName.Text.Trim();
            this.txtDesc.Text = sDesc;
        }

        string sRecomEmailTemplateID = this.ddlRecomActionTemplate.SelectedValue;
        int isRecomEmailTemplateID = Convert.ToInt32(sRecomEmailTemplateID);

        string sAlertEmailTemplateID = this.ddlAlertEmailTemplate.SelectedValue;
        int iAlertEmailTemplateID = Convert.ToInt32(sAlertEmailTemplateID);

        bool bReqAck = this.chkReqAck.Checked;
        string sAdvFormula = this.txtFormula.Text.Trim();

        string sScope = this.ddlScope.SelectedValue;
        Int16 iScope = Convert.ToInt16(sScope);
        string sTarget = this.ddlTarget.SelectedValue;
        Int16 iTarget = Convert.ToInt16(sTarget);

        #endregion

        Template_Rules RuleManager = new Template_Rules();

        #region 检查Rule Name是否重复

        bool bIsExist = RuleManager.IsExist_Create(sRuleName);
        if (bIsExist == true)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Duplicate", "$('#divContainer').hide();alert('The rule name has been taken. Please use a different name.');$('#divContainer').show();", true);
            return;
        }

        #endregion

        #region get condition data

        string sSequences = this.hdnSequences.Text;
        string sPointFieldIDs = this.hdnPointFieldIDs.Text;
        string sConditions = this.hdnConditions.Text;
        string sTolerances = this.hdnTolerances.Text;   // [$xxx$],[$yyy$],[$zzz$]
        string sToleranceTypes = this.hdnToleranceTypes.Text;   // $,%,#null#

        #endregion

        #region build conditiion list


        DataTable ConditionList = RuleManager.GetConditionList(" and 1=0");

        string[] SequenceArray = sSequences.Split(',');
        string[] PointFieldIDArray = sPointFieldIDs.Split(',');
        string[] ConditionArray = sConditions.Split(',');
        string[] ToleranceArray = sTolerances.Split(',');
        string[] ToleranceTypeArray = sToleranceTypes.Split(',');

        for (int i = 0; i < PointFieldIDArray.Length; i++)
        {
            string sSequence = SequenceArray[i];
            string sPointFieldID = PointFieldIDArray[i];
            string sConditionID = ConditionArray[i];
            string sToleranceBlock = ToleranceArray[i];
            string sToleranceType = ToleranceTypeArray[i];

            #region format Tolerance

            string sTelerance = sToleranceBlock.Replace("[$", string.Empty);
            sTelerance = sTelerance.Replace("$]", string.Empty);

            #endregion

            #region add rows

            DataRow ConditionRow = ConditionList.NewRow();
            ConditionRow["RuleCondId"] = 0;
            ConditionRow["RuleId"] = 0;
            ConditionRow["PointFieldId"] = Convert.ToInt32(sPointFieldID);
            ConditionRow["Condition"] = Convert.ToInt32(sConditionID);
            ConditionRow["Tolerance"] = sTelerance;
            if (sToleranceType == "#null#")
            {
                ConditionRow["ToleranceType"] = DBNull.Value;
            }
            else if (sToleranceType == "#empty#")
            {
                ConditionRow["ToleranceType"] = string.Empty;
            }
            else
            {
                ConditionRow["ToleranceType"] = sToleranceType;
            }
            ConditionRow["Sequence"] = Convert.ToInt32(sSequence);
            ConditionList.Rows.Add(ConditionRow);

            #endregion
        }



        #endregion

        // insert
        if (sAdvFormula.Trim() == string.Empty)
        {
            sAdvFormula = sSequences.Replace(",", " AND ");
        }

        int iSelCampaignID = 0;
        if (hfSelCampaigns.Value != "")
        {
            iSelCampaignID=Convert.ToInt32(hfSelCampaigns.Value);
            AutoCampaigns _bAutoCampaigns = new AutoCampaigns();
            int iCount = _bAutoCampaigns.GetMarketingCount(" AND CampaignId=" + iSelCampaignID);
            if (iCount == 0)
            {
                LPWeb.Model.AutoCampaigns _mAutoCampaigns = new LPWeb.Model.AutoCampaigns();
                _mAutoCampaigns.CampaignId = iSelCampaignID;
                _mAutoCampaigns.PaidBy = 0;
                _mAutoCampaigns.Enabled = true;
                _mAutoCampaigns.SelectedBy = CurrUser.iUserID;
                _bAutoCampaigns.Add(_mAutoCampaigns);
            }
        }

        int nNewId = RuleManager.InsertRule(sRuleName, sDesc, iAlertEmailTemplateID, bReqAck, isRecomEmailTemplateID, sAdvFormula, iScope, iTarget, ConditionList, iSelCampaignID);

        DataTable dtNewRuleWithEmailTpltInfo = RuleManager.GetRuleWithAlertEmailTpltInfo(nNewId);
        string strReturn = "<table></table>";
        if (dtNewRuleWithEmailTpltInfo.Rows.Count > 0)
        {
            strReturn = "<table><Rule RuleId=\"{0}\" Name=\"{1}\" AlertEmailTemplId=\"{2}\" AlertEmailTpltName=\"{3}\"></Rule></table>";
            strReturn = string.Format(strReturn, dtNewRuleWithEmailTpltInfo.Rows[0]["RuleId"],
                string.Format("{0}", dtNewRuleWithEmailTpltInfo.Rows[0]["Name"]).Replace("<", "&amp;lt;"), dtNewRuleWithEmailTpltInfo.Rows[0]["AlertEmailTemplId"],
                string.Format("{0}", dtNewRuleWithEmailTpltInfo.Rows[0]["AlertEmailTpltName"]).Replace("<", "&amp;lt;"));
        }
        strReturn = strReturn.Replace('<', '\u0001').Replace("'", "&#39;");
        // success
        //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", "$('#divContainer').hide();alert('Create rule successfully.');window.parent.location.href=window.parent.location.href;", true);
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", string.Format("addSuccessfully('{0}');", strReturn), true); // changed by peter 20110116
    }
}
