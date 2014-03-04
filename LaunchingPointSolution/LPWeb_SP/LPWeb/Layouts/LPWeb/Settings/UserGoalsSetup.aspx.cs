using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using LPWeb.Common;
using Utilities;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Settings
{
    /// <summary>
    /// User Goals Setup page
    /// Author: Peter
    /// Date: 2010-09-20
    /// </summary>
    public partial class UserGoalsSetup : BasePage
    {
        BLL.UserGoals UserGoalsManager = new BLL.UserGoals();

        string strUserIDs = "";
        protected string str1stMonth = "January";
        protected string str2ndMonth = "February";
        protected string str3rdMonth = "March";
        protected void Page_Load(object sender, EventArgs e)
        {
            // Response.Write(Request.QueryString["ids"]);
            // get all three months user goals data of all users
            // use repeater
            strUserIDs = Request.QueryString["ids"];
            if (!IsPostBack)
            {
                BindRepeater();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //dr["LowRange"], dr["MediumRange"], dr["HighRange"], dr["UserId"], dr["Month"]
            DataSet dsUserGoals = new DataSet();
            DataTable dtUserGoals = new DataTable();
            dsUserGoals.Tables.Add(dtUserGoals);

            DataColumn col = new DataColumn("UserId");
            dtUserGoals.Columns.Add(col);
            col = new DataColumn("LowRange");
            dtUserGoals.Columns.Add(col);
            col = new DataColumn("MediumRange");
            dtUserGoals.Columns.Add(col);
            col = new DataColumn("HighRange");
            dtUserGoals.Columns.Add(col);
            col = new DataColumn("Month");
            dtUserGoals.Columns.Add(col);

            string[] arrMonths = this.ddlMonths.SelectedValue.Split(',');
            if (arrMonths.Length != 3)
                return;
            foreach (RepeaterItem item in rpUg.Items)
            {
                if (ListItemType.AlternatingItem != item.ItemType && ListItemType.Item != item.ItemType)
                    continue;

                HiddenField hiUId = item.FindControl("hiUId") as HiddenField;
                TextBox tbFL_Input = item.FindControl("tbFL_Input") as TextBox;
                TextBox tbFM_Input = item.FindControl("tbFM_Input") as TextBox;
                TextBox tbFH_Input = item.FindControl("tbFH_Input") as TextBox;
                TextBox tbSL_Input = item.FindControl("tbSL_Input") as TextBox;
                TextBox tbSM_Input = item.FindControl("tbSM_Input") as TextBox;
                TextBox tbSH_Input = item.FindControl("tbSH_Input") as TextBox;
                TextBox tbTL_Input = item.FindControl("tbTL_Input") as TextBox;
                TextBox tbTM_Input = item.FindControl("tbTM_Input") as TextBox;
                TextBox tbTH_Input = item.FindControl("tbTH_Input") as TextBox;
                if (null == hiUId || string.IsNullOrEmpty(hiUId.Value))
                    continue;

                DataRow dr = dtUserGoals.NewRow();
                dr["UserId"] = hiUId.Value;
                dr["Month"] = arrMonths[0];
                if (null != tbFL_Input)
                {
                    decimal dFL = 0.0m;
                    if (!decimal.TryParse(tbFL_Input.Text, out dFL))
                        dFL = 0.0m;
                    dr["LowRange"] = dFL;
                }
                if (null != tbFM_Input)
                {
                    decimal dFM = 0.0m;
                    if (!decimal.TryParse(tbFM_Input.Text, out dFM))
                        dFM = 0.0m;
                    dr["MediumRange"] = dFM;
                }
                if (null != tbFH_Input)
                {
                    decimal dFH = 0.0m;
                    if (!decimal.TryParse(tbFH_Input.Text, out dFH))
                        dFH = 0.0m;
                    dr["HighRange"] = dFH;
                }
                dtUserGoals.Rows.Add(dr);

                dr = dtUserGoals.NewRow();
                dr["UserId"] = hiUId.Value;
                dr["Month"] = arrMonths[1];
                if (null != tbSL_Input)
                {
                    decimal dSL = 0.0m;
                    if (!decimal.TryParse(tbSL_Input.Text, out dSL))
                        dSL = 0.0m;
                    dr["LowRange"] = dSL;
                }
                if (null != tbSM_Input)
                {
                    decimal dSM = 0.0m;
                    if (!decimal.TryParse(tbSM_Input.Text, out dSM))
                        dSM = 0.0m;
                    dr["MediumRange"] = dSM;
                }
                if (null != tbSH_Input)
                {
                    decimal dSH = 0.0m;
                    if (!decimal.TryParse(tbSH_Input.Text, out dSH))
                        dSH = 0.0m;
                    dr["HighRange"] = dSH;
                }
                dtUserGoals.Rows.Add(dr);

                dr = dtUserGoals.NewRow();
                dr["UserId"] = hiUId.Value;
                dr["Month"] = arrMonths[2];
                if (null != tbTL_Input)
                {
                    decimal dTL = 0.0m;
                    if (!decimal.TryParse(tbTL_Input.Text, out dTL))
                        dTL = 0.0m;
                    dr["LowRange"] = dTL;
                }
                if (null != tbTM_Input)
                {
                    decimal dTM = 0.0m;
                    if (!decimal.TryParse(tbTM_Input.Text, out dTM))
                        dTM = 0.0m;
                    dr["MediumRange"] = dTM;
                }
                if (null != tbTH_Input)
                {
                    decimal dTH = 0.0m;
                    if (!decimal.TryParse(tbTH_Input.Text, out dTH))
                        dTH = 0.0m;
                    dr["HighRange"] = dTH;
                }
                dtUserGoals.Rows.Add(dr);
            }

            try
            {
                UserGoalsManager.SaveUserGoals(strUserIDs, this.ddlMonths.SelectedValue, dsUserGoals);
                CallClientFn("alert('Successfully saved!');");
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to save user goals info, please try it again.");
                LPLog.LogMessage(LogType.Logerror, "Failed to save User Goals info: " + ex.Message);
                return;
            }
            BindRepeater();
        }

        DataSet dsUserGoals = null;     // curr quarter User Goals info
        DataSet dsPrevUserGoals = null; // Previous quarter User Goals info
        private void BindRepeater()
        {
            dsUserGoals = UserGoalsManager.GetUserGoals(strUserIDs, this.ddlMonths.SelectedValue);
            string strPrevMonths = "";
            if (this.ddlMonths.SelectedIndex > 0)
                strPrevMonths = this.ddlMonths.Items[this.ddlMonths.SelectedIndex - 1].Value;
            else
                strPrevMonths = this.ddlMonths.Items[this.ddlMonths.Items.Count - 1].Value;
            dsPrevUserGoals = UserGoalsManager.GetUserGoals(strUserIDs, strPrevMonths);

            if (null != dsPrevUserGoals && dsPrevUserGoals.Tables.Count > 0)
            {
                string[] arrPrevMonths = strPrevMonths.Split(',');
                if (arrPrevMonths.Length == 3)
                {
                    string[] arrUserIds = strUserIDs.Split(',');
                    StringBuilder sbPrevUserGoals = new StringBuilder();
                    foreach (string str in arrUserIds)
                    {
                        string strObjId = string.Format("user{0}Goals", str);
                        sbPrevUserGoals.AppendFormat("var {0} = new PrevQuarter();", strObjId);
                        sbPrevUserGoals.AppendFormat("{0}.UId = '{1}';", strObjId, str);
                        DataRow[] drsPrevUserMonthGoals = dsPrevUserGoals.Tables[0].Select(string.Format("UserId='{0}' AND Month='{1}'", str, arrPrevMonths[0]));
                        if (drsPrevUserMonthGoals.Length == 1)
                        {
                            sbPrevUserGoals.AppendFormat("{0}.FstM.L_D = '{1:C}';", strObjId, drsPrevUserMonthGoals[0]["LowRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.FstM.L_I = '{1}';", strObjId, drsPrevUserMonthGoals[0]["LowRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.FstM.M_D = '{1:C}';", strObjId, drsPrevUserMonthGoals[0]["MediumRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.FstM.M_I = '{1}';", strObjId, drsPrevUserMonthGoals[0]["MediumRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.FstM.H_D = '{1:C}';", strObjId, drsPrevUserMonthGoals[0]["HighRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.FstM.H_I = '{1}';", strObjId, drsPrevUserMonthGoals[0]["HighRange"]);
                        }
                        drsPrevUserMonthGoals = dsPrevUserGoals.Tables[0].Select(string.Format("UserId='{0}' AND Month='{1}'", str, arrPrevMonths[1]));
                        if (drsPrevUserMonthGoals.Length == 1)
                        {
                            sbPrevUserGoals.AppendFormat("{0}.SndM.L_D = '{1:C}';", strObjId, drsPrevUserMonthGoals[0]["LowRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.SndM.L_I = '{1}';", strObjId, drsPrevUserMonthGoals[0]["LowRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.SndM.M_D = '{1:C}';", strObjId, drsPrevUserMonthGoals[0]["MediumRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.SndM.M_I = '{1}';", strObjId, drsPrevUserMonthGoals[0]["MediumRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.SndM.H_D = '{1:C}';", strObjId, drsPrevUserMonthGoals[0]["HighRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.SndM.H_I = '{1}';", strObjId, drsPrevUserMonthGoals[0]["HighRange"]);
                        }
                        drsPrevUserMonthGoals = dsPrevUserGoals.Tables[0].Select(string.Format("UserId='{0}' AND Month='{1}'", str, arrPrevMonths[2]));
                        if (drsPrevUserMonthGoals.Length == 1)
                        {
                            sbPrevUserGoals.AppendFormat("{0}.TrdM.L_D = '{1:C}';", strObjId, drsPrevUserMonthGoals[0]["LowRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.TrdM.L_I = '{1}';", strObjId, drsPrevUserMonthGoals[0]["LowRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.TrdM.M_D = '{1:C}';", strObjId, drsPrevUserMonthGoals[0]["MediumRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.TrdM.M_I = '{1}';", strObjId, drsPrevUserMonthGoals[0]["MediumRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.TrdM.H_D = '{1:C}';", strObjId, drsPrevUserMonthGoals[0]["HighRange"]);
                            sbPrevUserGoals.AppendFormat("{0}.TrdM.H_I = '{1}';", strObjId, drsPrevUserMonthGoals[0]["HighRange"]);
                        }
                        sbPrevUserGoals.AppendFormat("userPrevQuarterData.push({0});", strObjId);
                    }

                    ClientFun("regprevusergoals", sbPrevUserGoals.ToString());
                }
            }

            this.rpUg.DataSource = UserGoalsManager.GetUserForGoalsGrid(strUserIDs);
            this.rpUg.DataBind();
        }

        StringBuilder sbInputIds = new StringBuilder();
        protected void rpUg_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (ListItemType.Item == e.Item.ItemType || ListItemType.AlternatingItem == e.Item.ItemType)
            {
                HiddenField hiUId = e.Item.FindControl("hiUId") as HiddenField;
                if (null == hiUId
                    || string.IsNullOrEmpty(hiUId.Value)
                    || string.IsNullOrEmpty(this.ddlMonths.SelectedValue)
                    || null == dsUserGoals
                    || dsUserGoals.Tables.Count < 1)
                    return;
                DataRow[] drsUserGoals = dsUserGoals.Tables[0].Select(string.Format("Month IN ({0})", this.ddlMonths.SelectedValue));

                string[] arrMonths = this.ddlMonths.SelectedValue.Split(',');
                if (arrMonths.Length != 3)
                    return;
                string strObjId = string.Format("user{0}id", hiUId.Value);
                sbInputIds.AppendFormat("var {0} = new UserCtlId();", strObjId);
                sbInputIds.AppendFormat("{0}.UId = '{1}';", strObjId, hiUId.Value);
                // get first month goals info of current user
                DataRow[] drsUserMonthGoals = dsUserGoals.Tables[0].Select(string.Format("UserId='{0}' AND Month='{1}'", hiUId.Value, arrMonths[0]));
                Label lblFL_Dis = e.Item.FindControl("lblFL_Dis") as Label;
                TextBox tbFL_Input = e.Item.FindControl("tbFL_Input") as TextBox;
                if (null != lblFL_Dis && null != tbFL_Input)
                {
                    lblFL_Dis.Attributes.Add("onclick", string.Format("onInput('{0}', '{1}');", lblFL_Dis.ClientID, tbFL_Input.ClientID));
                    //ClientFun("text", string.Format("regDisAresClickEvent('{0}', '{1}')", lblFL_Dis.ClientID, tbFL_Input.ClientID));
                    //tbFL_Input.Attributes.Add("onblur", string.Format("onIBlur('{0}', '{1}');", lblFL_Dis.ClientID, tbFL_Input.ClientID));
                    tbFL_Input.Attributes.Add("onblur", string.Format("onIBlur('{0}', '1', 'L');", hiUId.Value));
                    if (1 == drsUserMonthGoals.Length)
                    {
                        lblFL_Dis.Text = string.Format("{0:C}", drsUserMonthGoals[0]["LowRange"]);
                        tbFL_Input.Text = string.Format("{0}", drsUserMonthGoals[0]["LowRange"]);
                    }
                    sbInputIds.AppendFormat("{0}.FstM.L_D='{1}';", strObjId, lblFL_Dis.ClientID);
                    sbInputIds.AppendFormat("{0}.FstM.L_I='{1}';", strObjId, tbFL_Input.ClientID);
                }
                Label lblFM_Dis = e.Item.FindControl("lblFM_Dis") as Label;
                TextBox tbFM_Input = e.Item.FindControl("tbFM_Input") as TextBox;
                if (null != lblFM_Dis && null != tbFM_Input)
                {
                    lblFM_Dis.Attributes.Add("onclick", string.Format("onInput('{0}', '{1}');", lblFM_Dis.ClientID, tbFM_Input.ClientID));
                    tbFM_Input.Attributes.Add("onblur", string.Format("onIBlur('{0}', '1', 'M');", hiUId.Value));
                    if (1 == drsUserMonthGoals.Length)
                    {
                        lblFM_Dis.Text = string.Format("{0:C}", drsUserMonthGoals[0]["MediumRange"]);
                        tbFM_Input.Text = string.Format("{0}", drsUserMonthGoals[0]["MediumRange"]);
                    }
                    sbInputIds.AppendFormat("{0}.FstM.M_D='{1}';", strObjId, lblFM_Dis.ClientID);
                    sbInputIds.AppendFormat("{0}.FstM.M_I='{1}';", strObjId, tbFM_Input.ClientID);
                }
                Label lblFH_Dis = e.Item.FindControl("lblFH_Dis") as Label;
                TextBox tbFH_Input = e.Item.FindControl("tbFH_Input") as TextBox;
                if (null != lblFH_Dis && null != tbFH_Input)
                {
                    lblFH_Dis.Attributes.Add("onclick", string.Format("onInput('{0}', '{1}');", lblFH_Dis.ClientID, tbFH_Input.ClientID));
                    tbFH_Input.Attributes.Add("onblur", string.Format("onIBlur('{0}', '1', 'H');", hiUId.Value));
                    if (1 == drsUserMonthGoals.Length)
                    {
                        lblFH_Dis.Text = string.Format("{0:C}", drsUserMonthGoals[0]["HighRange"]);
                        tbFH_Input.Text = string.Format("{0}", drsUserMonthGoals[0]["HighRange"]);
                    }
                    sbInputIds.AppendFormat("{0}.FstM.H_D='{1}';", strObjId, lblFH_Dis.ClientID);
                    sbInputIds.AppendFormat("{0}.FstM.H_I='{1}';", strObjId, tbFH_Input.ClientID);
                }

                // get first month goals info of current user
                drsUserMonthGoals = dsUserGoals.Tables[0].Select(string.Format("UserId='{0}' AND Month='{1}'", hiUId.Value, arrMonths[1]));
                Label lblSL_Dis = e.Item.FindControl("lblSL_Dis") as Label;
                TextBox tbSL_Input = e.Item.FindControl("tbSL_Input") as TextBox;
                if (null != lblSL_Dis && null != tbSL_Input)
                {
                    lblSL_Dis.Attributes.Add("onclick", string.Format("onInput('{0}', '{1}');", lblSL_Dis.ClientID, tbSL_Input.ClientID));
                    tbSL_Input.Attributes.Add("onblur", string.Format("onIBlur('{0}', '2', 'L');", hiUId.Value));
                    if (1 == drsUserMonthGoals.Length)
                    {
                        lblSL_Dis.Text = string.Format("{0:C}", drsUserMonthGoals[0]["LowRange"]);
                        tbSL_Input.Text = string.Format("{0}", drsUserMonthGoals[0]["LowRange"]);
                    }
                    sbInputIds.AppendFormat("{0}.SndM.L_D='{1}';", strObjId, lblSL_Dis.ClientID);
                    sbInputIds.AppendFormat("{0}.SndM.L_I='{1}';", strObjId, tbSL_Input.ClientID);
                }
                Label lblSM_Dis = e.Item.FindControl("lblSM_Dis") as Label;
                TextBox tbSM_Input = e.Item.FindControl("tbSM_Input") as TextBox;
                if (null != lblSM_Dis && null != tbSM_Input)
                {
                    lblSM_Dis.Attributes.Add("onclick", string.Format("onInput('{0}', '{1}');", lblSM_Dis.ClientID, tbSM_Input.ClientID));
                    tbSM_Input.Attributes.Add("onblur", string.Format("onIBlur('{0}', '2', 'M');", hiUId.Value));
                    if (1 == drsUserMonthGoals.Length)
                    {
                        lblSM_Dis.Text = string.Format("{0:C}", drsUserMonthGoals[0]["MediumRange"]);
                        tbSM_Input.Text = string.Format("{0}", drsUserMonthGoals[0]["MediumRange"]);
                    }
                    sbInputIds.AppendFormat("{0}.SndM.M_D='{1}';", strObjId, lblSM_Dis.ClientID);
                    sbInputIds.AppendFormat("{0}.SndM.M_I='{1}';", strObjId, tbSM_Input.ClientID);
                }
                Label lblSH_Dis = e.Item.FindControl("lblSH_Dis") as Label;
                TextBox tbSH_Input = e.Item.FindControl("tbSH_Input") as TextBox;
                if (null != lblSH_Dis && null != tbSH_Input)
                {
                    lblSH_Dis.Attributes.Add("onclick", string.Format("onInput('{0}', '{1}');", lblSH_Dis.ClientID, tbSH_Input.ClientID));
                    tbSH_Input.Attributes.Add("onblur", string.Format("onIBlur('{0}', '2', 'H');", hiUId.Value));
                    if (1 == drsUserMonthGoals.Length)
                    {
                        lblSH_Dis.Text = string.Format("{0:C}", drsUserMonthGoals[0]["HighRange"]);
                        tbSH_Input.Text = string.Format("{0}", drsUserMonthGoals[0]["HighRange"]);
                    }
                    sbInputIds.AppendFormat("{0}.SndM.H_D='{1}';", strObjId, lblSH_Dis.ClientID);
                    sbInputIds.AppendFormat("{0}.SndM.H_I='{1}';", strObjId, tbSH_Input.ClientID);
                }

                // get first month goals info of current user
                drsUserMonthGoals = dsUserGoals.Tables[0].Select(string.Format("UserId='{0}' AND Month='{1}'", hiUId.Value, arrMonths[2]));
                Label lblTL_Dis = e.Item.FindControl("lblTL_Dis") as Label;
                TextBox tbTL_Input = e.Item.FindControl("tbTL_Input") as TextBox;
                if (null != lblTL_Dis && null != tbTL_Input)
                {
                    lblTL_Dis.Attributes.Add("onclick", string.Format("onInput('{0}', '{1}');", lblTL_Dis.ClientID, tbTL_Input.ClientID));
                    tbTL_Input.Attributes.Add("onblur", string.Format("onIBlur('{0}', '3', 'L');", hiUId.Value));
                    if (1 == drsUserMonthGoals.Length)
                    {
                        lblTL_Dis.Text = string.Format("{0:C}", drsUserMonthGoals[0]["LowRange"]);
                        tbTL_Input.Text = string.Format("{0}", drsUserMonthGoals[0]["LowRange"]);
                    }
                    sbInputIds.AppendFormat("{0}.TrdM.L_D='{1}';", strObjId, lblTL_Dis.ClientID);
                    sbInputIds.AppendFormat("{0}.TrdM.L_I='{1}';", strObjId, tbTL_Input.ClientID);
                }
                Label lblTM_Dis = e.Item.FindControl("lblTM_Dis") as Label;
                TextBox tbTM_Input = e.Item.FindControl("tbTM_Input") as TextBox;
                if (null != lblTM_Dis && null != tbTM_Input)
                {
                    lblTM_Dis.Attributes.Add("onclick", string.Format("onInput('{0}', '{1}');", lblTM_Dis.ClientID, tbTM_Input.ClientID));
                    tbTM_Input.Attributes.Add("onblur", string.Format("onIBlur('{0}', '3', 'M');", hiUId.Value));
                    if (1 == drsUserMonthGoals.Length)
                    {
                        lblTM_Dis.Text = string.Format("{0:C}", drsUserMonthGoals[0]["MediumRange"]);
                        tbTM_Input.Text = string.Format("{0}", drsUserMonthGoals[0]["MediumRange"]);
                    }
                    sbInputIds.AppendFormat("{0}.TrdM.M_D='{1}';", strObjId, lblTM_Dis.ClientID);
                    sbInputIds.AppendFormat("{0}.TrdM.M_I='{1}';", strObjId, tbTM_Input.ClientID);
                }
                Label lblTH_Dis = e.Item.FindControl("lblTH_Dis") as Label;
                TextBox tbTH_Input = e.Item.FindControl("tbTH_Input") as TextBox;
                if (null != lblTH_Dis && null != tbTH_Input)
                {
                    lblTH_Dis.Attributes.Add("onclick", string.Format("onInput('{0}', '{1}');", lblTH_Dis.ClientID, tbTH_Input.ClientID));
                    tbTH_Input.Attributes.Add("onblur", string.Format("onIBlur('{0}', '3', 'H');", hiUId.Value));
                    if (1 == drsUserMonthGoals.Length)
                    {
                        lblTH_Dis.Text = string.Format("{0:C}", drsUserMonthGoals[0]["HighRange"]);
                        tbTH_Input.Text = string.Format("{0}", drsUserMonthGoals[0]["HighRange"]);
                    }
                    sbInputIds.AppendFormat("{0}.TrdM.H_D='{1}';", strObjId, lblTH_Dis.ClientID);
                    sbInputIds.AppendFormat("{0}.TrdM.H_I='{1}';", strObjId, tbTH_Input.ClientID);
                }
                sbInputIds.Append(string.Format("currUserCtlId.push({0});", strObjId));
            }
        }

        /// <summary>
        /// call client function
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strScript"></param>
        private void ClientFun(string strKey, string strScript)
        {
            ClientScriptManager csm = this.Page.ClientScript;
            if (!csm.IsStartupScriptRegistered(this.GetType(), strKey))
            {
                csm.RegisterStartupScript(this.GetType(), strKey, strScript, true);
            }
        }

        /// <summary>
        /// Register client javascript function
        /// </summary>
        /// <param name="strScript"></param>
        private void CallClientFn(string strScript)
        {
            ClientScriptManager csm = this.Page.ClientScript;
            string strKey = "clientcallback";
            if (!csm.IsStartupScriptRegistered(this.GetType(), strKey))
            {
                csm.RegisterStartupScript(this.GetType(), strKey, strScript, true);
            }
        }

        protected void rpUg_PreRender(object sender, EventArgs e)
        {
            ClientFun("renderclientctlid", sbInputIds.ToString());
        }

        protected void ddlMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRepeater();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        { 
            switch (this.ddlMonths.SelectedIndex)
            {
                case 0:
                    str1stMonth = "January";
                    str2ndMonth = "February";
                    str3rdMonth = "March";
                    break;
                case 1:
                    str1stMonth = "April";
                    str2ndMonth = "May";
                    str3rdMonth = "June";
                    break;
                case 2:
                    str1stMonth = "July";
                    str2ndMonth = "August";
                    str3rdMonth = "September";
                    break;
                case 3:
                    str1stMonth = "October";
                    str2ndMonth = "November";
                    str3rdMonth = "December";
                    break;
            }
        }
    }
}