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
using System.Text;
using LPWeb.Common;

public partial class Settings_RuleEdit : BasePage
{
    int iRuleID = 0;

    protected string strNeedReturn = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["needret"]))
            strNeedReturn = "0";
        else
            strNeedReturn = "1";

        #region 校验必要参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "RuleID", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing", "$('#divContainer').hide();alert('Missing required query string.');window.parent.CloseDialog_EditRule();", true);
            return;
        }

        this.iRuleID = Convert.ToInt32(this.Request.QueryString["RuleID"]);

        #endregion

        #region 加载Rule信息
        Template_Rules RuleManager = new Template_Rules();
        DataTable RuleInfo = RuleManager.GetRuleInfo(this.iRuleID);
        if (RuleInfo.Rows.Count == 0)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Invalid", "$('#divContainer').hide();alert('Invalid required query string.');window.parent.CloseDialog_EditRule();", true);
            return;
        }

        #endregion

        // is referenced by rule group
        this.hdnIsRef.Text = RuleManager.bIsRef(this.iRuleID).ToString();

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

            #region 加载Conditions

            DataTable ConditionListData = RuleManager.GetConditionList(this.iRuleID);
            this.gridConditionList.DataSource = ConditionListData;
            this.gridConditionList.DataBind();

            #endregion

            #region 绑定数据

            this.txtRuleName.Text = RuleInfo.Rows[0]["Name"].ToString();
            this.txtDesc.Text = RuleInfo.Rows[0]["Desc"].ToString();
            this.chkEnabled.Checked = Convert.ToBoolean(RuleInfo.Rows[0]["Enabled"]);
            this.ddlRecomActionTemplate.SelectedValue = RuleInfo.Rows[0]["RecomEmailTemplid"].ToString();
            this.ddlAlertEmailTemplate.SelectedValue = RuleInfo.Rows[0]["AlertEmailTemplId"].ToString();
            this.chkReqAck.Checked = Convert.ToBoolean(RuleInfo.Rows[0]["AckReq"]);
            this.txtFormula.Text = RuleInfo.Rows[0]["AdvFormula"].ToString();
            this.ddlScope.SelectedValue = RuleInfo.Rows[0]["RuleScope"].ToString();
            this.ddlTarget.SelectedValue = RuleInfo.Rows[0]["LoanTarget"].ToString();
           

            #endregion
            if (RuleInfo.Rows[0]["AutoCampaignId"].ToString() != "")
            {
                this.hfSelCampaigns.Value = RuleInfo.Rows[0]["AutoCampaignId"].ToString();
                MarketingCampaigns _bMarketingCampaigns = new MarketingCampaigns();
                LPWeb.Model.MarketingCampaigns _mMarketingCampaigns = _bMarketingCampaigns.GetModel(Convert.ToInt32(RuleInfo.Rows[0]["AutoCampaignId"]));
                //txtMarketingCampaign.Text = _mMarketingCampaigns.CampaignName;
            }
            // set counter
            this.hdnCounter.Value = ConditionListData.Rows.Count.ToString();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region get rule data

        string sRuleName = this.txtRuleName.Text.Trim();
        bool bEnabled = this.chkEnabled.Checked;
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

        bool bIsExist = RuleManager.IsExist_Edit(this.iRuleID, sRuleName);
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
            ConditionRow["RuleId"] = this.iRuleID;
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
        RuleManager.UpdateRule(this.iRuleID, sRuleName, sDesc, bEnabled, iAlertEmailTemplateID, bReqAck, isRecomEmailTemplateID, sAdvFormula, iScope, iTarget, ConditionList);

        DataTable dtNewRuleWithEmailTpltInfo = RuleManager.GetRuleWithAlertEmailTpltInfo(this.iRuleID);
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
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Success", string.Format("updateSuccessfully('{0}');", strReturn), true);
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Template_Rules RuleManager = new Template_Rules();
        RuleManager.DeleteRule(this.iRuleID);

        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_DeleteSuccess", string.Format("deleteSuccessfully('{0}');", this.iRuleID), true);
    }

    public string GetOptions_ddlCondition(string sPointFieldDataType, string sSelectedID)
    {
        StringBuilder sbOptions = new StringBuilder();

        #region default: equals or not equal to

        if (sSelectedID == "1")
        {
            sbOptions.AppendLine("<option value='1' selected>equals</option>");
        }
        else if (sPointFieldDataType != "4")
        {
            sbOptions.AppendLine("<option value='1'>equals</option>");
        }

        if (sSelectedID == "2")
        {
            sbOptions.AppendLine("<option value='2' selected>not equal to</option>");
        }
        else if (sPointFieldDataType != "4")
        {
            sbOptions.AppendLine("<option value='2'>not equal to</option>");
        }

        #endregion

        if (sPointFieldDataType == "1")   // string
        {
            #region string

            if (sSelectedID == "10")
            {
                sbOptions.AppendLine("<option value='10' selected>contains</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='10'>contains</option>");
            }

            if (sSelectedID == "11")
            {
                sbOptions.AppendLine("<option value='11' selected>does not contain</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='11'>does not contain</option>");
            }

            if (sSelectedID == "12")
            {
                sbOptions.AppendLine("<option value='12' selected>starts with</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='12'>starts with</option>");
            }

            #endregion
        }
        else if (sPointFieldDataType == "2") // numeric
        {
            #region numeric

            if (sSelectedID == "3")
            {
                sbOptions.AppendLine("<option value='3' selected>less than</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='3'>less than</option>");
            }

            if (sSelectedID == "4")
            {
                sbOptions.AppendLine("<option value='4' selected>greater than</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='4'>greater than</option>");
            }

            if (sSelectedID == "5")
            {
                sbOptions.AppendLine("<option value='5' selected>less or equal</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='5'>less or equal</option>");
            }

            if (sSelectedID == "6")
            {
                sbOptions.AppendLine("<option value='6' selected>greater or equal</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='6'>greater or equal</option>");
            }

            if (sSelectedID == "7")
            {
                sbOptions.AppendLine("<option value='7' selected>increases by more than</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='7'>increases by more than</option>");
            }

            if (sSelectedID == "8")
            {
                sbOptions.AppendLine("<option value='8' selected>decreases by more than</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='8'>decreases by more than</option>");
            }

            if (sSelectedID == "9")
            {
                sbOptions.AppendLine("<option value='9' selected>changes by more than</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='9'>changes by more than</option>");
            }

            #endregion
        }
        else if (sPointFieldDataType == "3") // boolean
        {

        }
        else if (sPointFieldDataType == "4") // date
        {
            #region date
            if (sSelectedID == "13")
            {
                sbOptions.AppendLine("<option value='13' selected>Today</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='13'>Today</option>");
            }

            if (sSelectedID == "14")
            {
                sbOptions.AppendLine("<option value='14' selected>Yesterday</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='14'>Yesterday</option>");
            }

            if (sSelectedID == "15")
            {
                sbOptions.AppendLine("<option value='15' selected>Within last x days</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='15'>Within last x days</option>");
            }

            if (sSelectedID == "16")
            {
                sbOptions.AppendLine("<option value='16' selected>Within next x days</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='16'>Within next x days</option>");
            }

            if (sSelectedID == "17")
            {
                sbOptions.AppendLine("<option value='17' selected>Older than x days</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='17'>Older than x days</option>");
            }

            if (sSelectedID == "18")
            {
                sbOptions.AppendLine("<option value='18' selected>After x days from now</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='18'>After x days from now</option>");
            } 
            #endregion
        }

        return sbOptions.ToString();
    }

    public string GetToleranceInput(string sSeq, string sPointFieldDataType, string ToleranceValue)
    {
        string sInputCode = string.Empty;
        if (sPointFieldDataType == "3")  // boolean
        {
            string ddlTolerance_NewID = "ddlTolerance" + sSeq;
            sInputCode = "<select id='" + ddlTolerance_NewID + "' class='txtTolerance' style='width: 128px;'>";

            if (ToleranceValue == "Yes")
            {
                sInputCode += "<option value='Yes' selected>Yes</option>"
                            + "<option value='No'>No</option>";
            }
            else if (ToleranceValue == "No")
            {
                sInputCode += "<option value='Yes'>Yes</option>"
                            + "<option value='No' selected>No</option>";
            }
            sInputCode += "</select>";
        }
        else
        {
            string txtTolerance_NewID = "txtTolerance" + sSeq;
            if (sPointFieldDataType == "1")  // string
            {
                sInputCode = "<input id='" + txtTolerance_NewID + "' name='" + txtTolerance_NewID + "' type='text' value='" + ToleranceValue + "' maxlength='64' class='txtTolerance' />";
            }
            else if (sPointFieldDataType == "2")  // numeric
            {
                sInputCode = "<input id='" + txtTolerance_NewID + "' name='" + txtTolerance_NewID + "' type='text' value='" + ToleranceValue + "' maxlength='8' class='txtTolerance' />";
            }
            else if (sPointFieldDataType == "4")  // date
            {
                sInputCode = "<input id='" + txtTolerance_NewID + "' name='" + txtTolerance_NewID + "' type='text' value='" + ToleranceValue + "' maxlength='8' class='txtTolerance' />";
            }
        }

        return sInputCode;
    }

    public string GetToleranceTypeDropdownList(string sPointFieldDataType, string ToleranceType)
    {
        string sDropdownList = "<select id='ddlType'";

        if (sPointFieldDataType == "2")   // numeric
        {
            sDropdownList += " >";

            if (ToleranceType == string.Empty)
            {
                sDropdownList += "<option value='' selected></option>";
            }
            else
            {
                sDropdownList += "<option value=''></option>";
            }

            if (ToleranceType == "$")
            {
                sDropdownList += "<option value='$' selected>$</option>";
            }
            else
            {
                sDropdownList += "<option value='$'>$</option>";
            }

            if (ToleranceType == "%")
            {
                sDropdownList += "<option value='%' selected>%</option>";
            }
            else
            {
                sDropdownList += "<option value='%'>%</option>";
            }
        }
        else if (sPointFieldDataType == "4") //date
        {
            sDropdownList += " disabled>";
            if (ToleranceType == "Days")
            {
                sDropdownList += "<option value='Days' selected>Days</option>";
            }
            else
            {
                sDropdownList += "<option value=''></option>";
            }
            sDropdownList += "</select>";
        }
        else
        {
            sDropdownList += " disabled >"
                           + "<option value=''></option>"
                           + "<option value='$'>$</option>"
                           + "<option value='%'>%</option>";
        }

        sDropdownList += "</select>";
        return sDropdownList;
    }
}
