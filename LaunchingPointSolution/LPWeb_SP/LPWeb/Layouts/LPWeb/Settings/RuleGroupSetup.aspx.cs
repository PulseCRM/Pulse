using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using LPWeb.Layouts.LPWeb.Common;
using Utilities;
using LPWeb.Common;
using System.Xml;

namespace LPWeb.Layouts.LPWeb.Settings
{
    /// <summary>
    /// Rule Group Setup page
    /// Author: Peter
    /// Date: 2011-01-08
    /// </summary>
    public partial class RuleGroupSetup : BasePage
    {
        BLL.Template_RuleGroups ruleGroupManager = new BLL.Template_RuleGroups();
        BLL.Template_Rules rulesManager = new BLL.Template_Rules();

        /// <summary>
        /// Rule group Id
        /// </summary>
        protected int? RuleGroupID
        {
            get
            {
                int nId = -1;
                if (!int.TryParse(Request.QueryString["id"], out nId))
                    nId = -1;

                if (-1 == nId)
                    return null;
                else
                    return nId;
            }
            set
            {
                ViewState["id"] = value;
            }
        }

        /// <summary>
        /// setup page mode：
        /// 0：new
        /// 1：edit
        /// </summary>
        private string Mode
        {
            get
            {
                if (null == ViewState["mode"])
                    return Request.QueryString["mode"];
                else
                    return ViewState["mode"].ToString();
            }
            set
            {
                ViewState["mode"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DoInitData();
                BindGrid();

                if ("0" == Mode)
                {

                }
                if ("1" == Mode)
                {
                    if (!RuleGroupID.HasValue)
                    {
                        // if no UserId，thorw exception
                        LPLog.LogMessage(LogType.Logerror, "Invalid RuleGroupID");
                        throw new Exception("Invalid RuleGroupID");
                    }
                    else
                    {
                        Model.Template_RuleGroups rg = ruleGroupManager.GetModel(RuleGroupID.Value);
                        bool bIsReferenced = ruleGroupManager.IsReferencedByLoan(RuleGroupID.Value);
                        if (null == rg)
                        {
                            LPLog.LogMessage(LogType.Logerror, string.Format("Can not find rule group with ID:{0}", RuleGroupID.Value));
                        }
                        else
                        {
                            this.tbGroupName.Text = rg.Name;
                            this.tbDescription.Text = rg.Desc;
                            this.ddlScope.SelectedValue = (rg.RuleScope == null) ? "0" : rg.RuleScope.ToString();
                            this.ddlTarget.SelectedValue = (rg.LoanTarget == null) ? "0" : rg.LoanTarget.ToString();
                            this.ckbEnabled.Checked = rg.Enabled;
                            this.hiReferenced.Value = bIsReferenced ? "1" : "0";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Save user info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Model.Template_RuleGroups ruleGroup = null;
            if ("0" == Mode)
            {
                ruleGroup = new Model.Template_RuleGroups();
                GetValue(ref ruleGroup);

                // check duplicate
                if (ruleGroupManager.IsRuleGroupNameExists(-1, ruleGroup.Name))
                {
                    PageCommon.AlertMsg(this, "Duplicate rule group name!");
                    return;
                }

                try
                {
                    // save the new added rule group info, nSourceUID is null or not indicated to clone or not
                    RuleGroupID = ruleGroupManager.AddRuleGroupInfo(ruleGroup, this.hiAllIds.Value, null);
                    Mode = "1";

                    ClientFun(this.updatePanel, "saveSuccessfully", "saveSuccess();");
                }
                catch (Exception ex)
                {
                    ClientFun(this.updatePanel, "failedToSave", "alert('Failed to save rule group info, please try it again.');");
                    LPLog.LogMessage(LogType.Logerror, "Error occured when save rule group info: " + ex.Message);
                    return;
                }
            }
            else if ("1" == Mode)
            {
                ruleGroup = ruleGroupManager.GetModel(RuleGroupID.Value);
                GetValue(ref ruleGroup);

                // check name duplicate
                if (ruleGroupManager.IsRuleGroupNameExists(RuleGroupID.Value, ruleGroup.Name))
                {
                    ClientFun(this.updatePanel, "duplicateName", "alert('Duplicate rule group name!');");
                    return;
                }

                try
                {
                    // update current user info, without personalization info
                    ruleGroupManager.UpdateRuleGroupInfo(ruleGroup, this.hiAllIds.Value);
                    ClientFun(this.updatePanel, "saveSuccessfully", "saveSuccess();");
                }
                catch (Exception ex)
                {
                    ClientFun(this.updatePanel, "failedToSave", "alert('Failed to save rule group info, please try it again.');");
                    LPLog.LogMessage(LogType.Logerror, "Failed to save rule group info: " + ex.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// Load user info from the form
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private void GetValue(ref Model.Template_RuleGroups rg)
        {
            rg.Name = this.tbGroupName.Text.Trim();
            rg.Desc = this.tbDescription.Text.Trim();
            rg.Enabled = this.ckbEnabled.Checked;
            if (this.ddlScope.SelectedIndex > 0)
            {
                rg.RuleScope = Convert.ToInt32(this.ddlScope.SelectedValue);
            }
            else
            {
                rg.RuleScope = 0;
            }
            if (this.ddlTarget.SelectedIndex > 0)
            {
                rg.LoanTarget = Convert.ToInt32(this.ddlTarget.SelectedValue);
            }
            else
            {
                rg.LoanTarget = 0;
            }
        }

        protected void btnClone_Click(object sender, EventArgs e)
        {
            Mode = "0";            
            this.tbGroupName.Text = string.Format("Copy of {0}", this.tbGroupName.Text);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ruleGroupManager.DeleteRuleGroupInfo(RuleGroupID.Value);
                LPLog.LogMessage(string.Format("Rule Group {0} have been deleted", this.tbGroupName.Text));
                ClientFun(this.updatePanel, "closesetupwin", "closeBox(true, true);");
            }
            catch (Exception ex)
            {
                ClientFun(this.updatePanel, "failedToDelete", "alert('Failed to delete Rule Group info, please try it again.');");
                LPLog.LogMessage(string.Format("Failed to delete Rule Group info: ", this.tbGroupName.Text));
            }
        }

        /// <summary>
        /// Bind rule gridview
        /// </summary>
        private void BindGrid()
        {
            int recordCount = 0;

            DataSet listData = null;
            try
            {
                listData = ruleGroupManager.GetRuleListOfRuleGroup(1000, 0, string.Format(" AND RuleGroupId='{0}'", RuleGroupID),
                    out recordCount, "Name", 0);
                this.hiCurrentData.Value = GetEncodedXmlOfRuleItems(listData);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
        }

        /// <summary>
        /// get selected data as encoded xml
        /// </summary>
        /// <param name="dsCurr"></param>
        /// <returns></returns>
        private string GetEncodedXmlOfRuleItems(DataSet dsCurr)
        {
            if (null == dsCurr || dsCurr.Tables.Count != 1)
                return "";

            // return selected record as XML
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement element = xmlDoc.CreateElement("table");
            xmlDoc.AppendChild(element);
            XmlElement childElement = null;
            XmlAttribute attri = null;

            foreach (DataRow row in dsCurr.Tables[0].Rows)
            {
                childElement = xmlDoc.CreateElement("tr");
                element.AppendChild(childElement);

                attri = xmlDoc.CreateAttribute("RuleId");
                attri.Value = row["RuleId"].ToString();
                childElement.Attributes.Append(attri);

                attri = xmlDoc.CreateAttribute("Name");
                attri.Value = row["Name"].ToString().Replace("<", "&lt;");
                childElement.Attributes.Append(attri);

                attri = xmlDoc.CreateAttribute("AlertEmailTemplId");
                attri.Value = row["AlertEmailTemplId"].ToString();
                childElement.Attributes.Append(attri);

                attri = xmlDoc.CreateAttribute("AlertEmailTpltName");
                attri.Value = row["AlertEmailTpltName"].ToString().Replace("<", "&lt;");
                childElement.Attributes.Append(attri);
            }
            return xmlDoc.OuterXml.Replace('<', '\u0001').Replace("'", "&#39;");
        }

        /// <summary>
        /// disable the selected rules
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDisable_Click(object sender, EventArgs e)
        {
            if (this.hiCheckedIds.Value.Length > 0)
            {
                try
                {
                    ruleGroupManager.DisableRuleOfRuleGroup(RuleGroupID.Value, this.hiCheckedIds.Value);
                    ClientFun(this.updatePanel, "disablesuccessfully", "alert('Disabled successfully!');");
                }
                catch (Exception ex)
                {
                    ClientFun(this.updatePanel, "failedtodisable", "alert('Failed to disable the selected rule(s), please try it again.');");
                    LPLog.LogMessage(LogType.Logerror, string.Format("Failed to disable the selected rule(s) of rule group \"{0}\": {1}",
                        this.tbGroupName.Text, ex.Message));
                    return;
                }
            }
        }

        /// <summary>
        /// romove rule from rule gorup, then remove the selected rules
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            if (this.hiCheckedIds.Value.Length > 0)
            {
                try
                {
                    ruleGroupManager.DeleteRuleOfRuleGroup(RuleGroupID.Value, this.hiCheckedIds.Value);
                    ClientFun(this.updatePanel, "deleteselectedRuleSuccessfully", "afterSelectedRuleDeleted();");
                }
                catch (Exception ex)
                {
                    ClientFun(this.updatePanel, "failedtodeleterecord", "alert('Failed to delete the selected rule(s), please try it again.');");
                    LPLog.LogMessage(LogType.Logerror, string.Format("Failed to delete the selected rule(s) of rule group \"{0}\": {1}",
                        this.tbGroupName.Text, ex.Message));
                    return;
                }
            }
        }

        /// <summary>
        /// call client function
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="strKey"></param>
        /// <param name="strScript"></param>
        private void ClientFun(Control ctl, string strKey, string strScript)
        {
            ScriptManager.RegisterStartupScript(ctl, this.GetType(), strKey, strScript, true);
        }

        /// <summary>
        /// Init data
        /// </summary>
        private void DoInitData()
        {

            //Binding Scope
            this.ddlScope.Items.Add(new ListItem("Loan", "0"));
            this.ddlScope.Items.Add(new ListItem("Company", "1"));


            //Binding Target
            this.ddlTarget.Items.Add(new ListItem("Processing", "0"));
            this.ddlTarget.Items.Add(new ListItem("Prospect", "1"));
            this.ddlTarget.Items.Add(new ListItem("Processing and Prospect", "2"));
        }
    }
}
