using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using Utilities;
using LPWeb.Common;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using LPWeb.DAL;

namespace LPWeb.Layouts.LPWeb.Prospect
{
    /// <summary>
    /// Prospect Task Setup
    /// Author: Peter
    /// Date: 2011-02-24
    /// </summary>
    public partial class ProspectTaskSetup : BasePage
    {
        BLL.ProspectTasks ptManager = new BLL.ProspectTasks();
        BLL.Contacts contactsManager = new BLL.Contacts();

        /// <summary>
        /// Prospect task id
        /// </summary>
        protected int? ProspectTaskId
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
        /// Prospect task id
        /// </summary>
        protected int? ContactId
        {
            get
            {
                string strTemp = "";
                if (null == ViewState["ContactId"])
                {
                    strTemp = Request.QueryString["ContactId"];
                }
                else
                {
                    strTemp = string.Format("{0}", ViewState["ContactId"]);
                }
                int nId = -1;
                if (!int.TryParse(strTemp, out nId))
                    nId = -1;

                if (-1 == nId)
                    return null;
                else
                    return nId;
            }
            set
            {
                ViewState["ContactId"] = value;
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
                this.btnPreWarn.OnClientClick = string.Format("return previewEmailTemplate('{0}');", this.ddlWarningEmail.ClientID);
                this.btnPreOverdue.OnClientClick = string.Format("return previewEmailTemplate('{0}');", this.ddlOverdueEmail.ClientID);
                this.btnPreComple.OnClientClick = string.Format("return previewEmailTemplate('{0}');", this.ddlComleEmail.ClientID);

                // bind email template
                BLL.Template_Email emailTpltManager = new BLL.Template_Email();

                DataTable dtEmailTplts = emailTpltManager.GetEmailTemplate("");
                this.ddlWarningEmail.DataValueField = "TemplEmailId";
                this.ddlWarningEmail.DataTextField = "Name";
                this.ddlWarningEmail.DataSource = dtEmailTplts;
                this.ddlWarningEmail.DataBind();
                this.ddlWarningEmail.Items.Insert(0, new ListItem("--select an email template--", "0"));

                this.ddlOverdueEmail.DataValueField = "TemplEmailId";
                this.ddlOverdueEmail.DataTextField = "Name";
                this.ddlOverdueEmail.DataSource = dtEmailTplts;
                this.ddlOverdueEmail.DataBind();
                this.ddlOverdueEmail.Items.Insert(0, new ListItem("--select an email template--", "0"));

                this.ddlComleEmail.DataValueField = "TemplEmailId";
                this.ddlComleEmail.DataTextField = "Name";
                this.ddlComleEmail.DataSource = dtEmailTplts;
                this.ddlComleEmail.DataBind();
                this.ddlComleEmail.Items.Insert(0, new ListItem("--select an email template--", "0"));

                BindTaskOwner();

                if ("0" == Mode)
                {
                    BLL.Prospect prospectManager = new BLL.Prospect();
                    Model.Prospect currProspect = prospectManager.GetModel(ContactId.GetValueOrDefault(0));
                    if (null == currProspect)
                    {
                        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Invalid3", "alert('Invalid query string.');window.close();", true);
                        return;
                    }
                    // set prospect's officer as task owner
                    ListItem listItem = this.ddlOwner.Items.FindByValue(currProspect.Loanofficer.GetValueOrDefault(0).ToString());
                    if (null != listItem)
                        listItem.Selected = true;
                    
                    this.btnDelete.Enabled = false;
                    this.btnClone.Enabled = false;

                    int nContactId = -1;
                    if (!int.TryParse(Request.QueryString["ContactId"], out nContactId))
                        nContactId = -1;
                    Model.Contacts contact = contactsManager.GetModel(nContactId);
                    if (null != contact)
                        this.lblClient.Text = string.Format("{0}, {1} {2}", contact.LastName, contact.FirstName, contact.MiddleName);
                    else
                    {
                        PageCommon.AlertMsg(this, "Invalid contact id!");
                        ClientFun("closewindow", "closeBox(false, false);");
                    }
                }
                else if ("1" == Mode)
                {
                    if (!ProspectTaskId.HasValue)
                    {
                        // if no ProspectTaskId，thorw exception
                        LPLog.LogMessage(LogType.Logerror, "Invalid client task id");
                        throw new Exception("Invalid client task id");
                    }
                    else
                    {
                        Model.ProspectTasks pt = ptManager.GetModel(ProspectTaskId.Value);
                        if (null == pt)
                        {
                            LPLog.LogMessage(LogType.Logerror, string.Format("Cannot find the task with ID:{0}", ProspectTaskId.Value));
                        }
                        else
                        {
                            if (pt.Completed.HasValue)
                            {
                                this.btnSave.Enabled = false;
                                this.btnDelete.Enabled = false;
                            }
                            Model.Contacts contact = contactsManager.GetModel(pt.ContactId);
                            if (null != contact)
                                this.lblClient.Text = string.Format("{0}, {1} {2}", contact.LastName, contact.FirstName, contact.MiddleName);
                            this.tbTaskName.Text = pt.TaskName;
                            this.ckbEnabled.Checked = pt.Enabled;
                            this.tbDesc.Text = pt.Desc;
                            ListItem listTaskItem = this.ddlOwner.Items.FindByValue(pt.OwnerId.GetValueOrDefault(0).ToString());
                            if (null != listTaskItem)
                                listTaskItem.Selected = true;
                            this.ddlOwner.SelectedIndex = pt.OwnerId.GetValueOrDefault();
                            if (pt.Due.HasValue)
                                this.tbDue.Text = pt.Due.Value.ToString("MM/dd/yyyy");

                            ListItem listItem = this.ddlWarningEmail.Items.FindByValue(pt.WarningEmailTemplId.GetValueOrDefault().ToString());
                            if (null != listItem)
                                listItem.Selected = true;

                            listItem = this.ddlOverdueEmail.Items.FindByValue(pt.OverdueEmailTemplId.GetValueOrDefault().ToString());
                            if (null != listItem)
                                listItem.Selected = true;

                            listItem = this.ddlComleEmail.Items.FindByValue(pt.CompletionEmailTemplid.GetValueOrDefault().ToString());
                            if (null != listItem)
                                listItem.Selected = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Bind DDL Data Source
        /// </summary>
        private void BindTaskOwner()
        {
            DataTable dtLoadOfficer = GetLoanOfficerList(CurrUser.iUserID);

            ddlOwner.DataSource = dtLoadOfficer;
            ddlOwner.DataTextField = "FullName";
            ddlOwner.DataValueField = "UserID";
            ddlOwner.DataBind();
        }

        /// <summary>
        /// get loan officer list
        /// neo 2011-03-13
        /// ProspectDetailPopup.aspx, modified by peter on 2011-03-20
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        private DataTable GetLoanOfficerList(int iUserID)
        {
            // get branch count
            //string sSql0 = "select count(1) from lpfn_GetUserBranches_UserList(" + iUserID + ")";
            //int iCount = Convert.ToInt32(DbHelperSQL.ExecuteScalar(sSql0));

            // all loan officer, modified by peter
            string sSql = "select a.UserID, a.FirstName +', '+ a.LastName as FullName "
                        + "from Users as a "
                        + "inner join GroupUsers as b on a.UserId = b.UserID "
                        + "inner join Groups as c on b.GroupID = c.GroupId "
                        + "where c.Enabled = 1 and a.UserEnabled=1 and a.RoleId = 3"
                        + " and (c.BranchId in (select * from lpfn_GetUserBranches_UserList(" + iUserID + ")))";
            sSql += "GROUP BY a.UserID, a.FirstName, a.LastName";

            // if belong to branch, get loan officer under same branch
            //if (iCount > 0)
            //{
            //    sSql += " and (c.BranchId in (select * from lpfn_GetUserBranches_UserList(" + iUserID + ")))";
            //}

            DataTable LoanOfficerList = DbHelperSQL.ExecuteDataTable(sSql);

            DataRow NewLORow = LoanOfficerList.NewRow();
            NewLORow["UserID"] = 0;
            NewLORow["FullName"] = "-- select --";
            LoanOfficerList.Rows.InsertAt(NewLORow, 0);

            return LoanOfficerList;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Model.ProspectTasks prospectTasks = null;
            if ("0" == Mode)
            {
                prospectTasks = new Model.ProspectTasks();
                GetValue(ref prospectTasks);

                // check duplicate
                if (ptManager.IsProspectTaskNameExists(-1, prospectTasks.TaskName))
                {
                    PageCommon.AlertMsg(this, "Duplicate task name.");
                    return;
                }

                try
                {
                    // save the new added prospect task info
                    
                        int iNewProspectID= ptManager.Add(prospectTasks);
                        ProspectTaskId = iNewProspectID;
                        bool bAlert = WorkflowManager.CreateProspectTaskAlerts(iNewProspectID);
                    Mode = "1";
                    this.btnDelete.Enabled = true;
                    this.btnClone.Enabled = true;
                    //ClientFun("savesuccess", "alert('Saved successfully!');");
                    PageCommon.RegisterJsMsg(this, "Saved successfully!", "parent.closeProspectTaskSetupWin();");
                }
                catch (Exception ex)
                {
                    ClientFun("failedToSave", "alert('Failed to save task info, please check your data and try again.');");
                    LPLog.LogMessage(LogType.Logerror, "Error occured when save the task: " + ex.Message);
                    return;
                }
            }
            else if ("1" == Mode)
            {
                prospectTasks = ptManager.GetModel(ProspectTaskId.Value);
                GetValue(ref prospectTasks);

                // check name duplicate
                if (ptManager.IsProspectTaskNameExists(ProspectTaskId.Value, prospectTasks.TaskName))
                {
                    ClientFun("duplicateName", "alert('Duplicate task name.');");
                    return;
                }

                try
                {
                    // update current prospect task info
                    ptManager.Update(prospectTasks);
                    ptManager.CheckProspectTaskAlert(Convert.ToInt32(ProspectTaskId));
                   //ClientFun("savesuccess", "alert('Saved successfully!');");
                   PageCommon.RegisterJsMsg(this, "Saved successfully!", "parent.closeProspectTaskSetupWin();");
                }
                catch (Exception ex)
                {
                    ClientFun("failedToSave", "alert('Failed to save the task, please check your data and try again.');");
                    LPLog.LogMessage(LogType.Logerror, "Error occured when save the task: " + ex.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// Load prospect task info from the form
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private void GetValue(ref Model.ProspectTasks item)
        {
            int nTemp = 0;

            item.TaskName = this.tbTaskName.Text;
            item.Enabled = this.ckbEnabled.Checked;
            item.Desc = this.tbDesc.Text;
            if (ContactId.HasValue)
                item.ContactId = ContactId.Value;

            // set prospect 
            if (!int.TryParse(this.ddlOwner.SelectedValue, out nTemp))
                nTemp = 0;
            if (0 != nTemp)
                item.OwnerId = nTemp;

            DateTime dtTemp = DateTime.MinValue;
            if (!DateTime.TryParse(this.tbDue.Text, out dtTemp))
                dtTemp = DateTime.MinValue;
            if (DateTime.MinValue != dtTemp)
                item.Due = dtTemp;

            if (!int.TryParse(this.ddlWarningEmail.SelectedValue, out nTemp))
                nTemp = 0;
            if (0 != nTemp)
                item.WarningEmailTemplId = nTemp;

            if (!int.TryParse(this.ddlOverdueEmail.SelectedValue, out nTemp))
                nTemp = 0;
            if (0 != nTemp)
                item.OverdueEmailTemplId = nTemp;

            if (!int.TryParse(this.ddlComleEmail.SelectedValue, out nTemp))
                nTemp = 0;
            if (0 != nTemp)
                item.CompletionEmailTemplid = nTemp;
        }

        protected void btnClone_Click(object sender, EventArgs e)
        {
            Mode = "0";

            // get contactId for clone
            Model.ProspectTasks pt = ptManager.GetModel(ProspectTaskId.Value);
            ContactId = pt.ContactId;

            this.btnDelete.Enabled = false;
            this.btnClone.Enabled = false;
            this.btnSave.Enabled = true;
            this.tbTaskName.Text = string.Format("Copy of {0}", this.tbTaskName.Text);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ptManager.Delete(ProspectTaskId.Value);
                LPLog.LogMessage(string.Format("The task {0} have been deleted", this.tbTaskName.Text));
                ClientFun("closesetupwin", "closeBox(true, true);");
            }
            catch (Exception ex)
            {
                ClientFun("failedToDelete", "alert('Failed to delete the task, please check your data and try again.');");
                LPLog.LogMessage(string.Format("Failed to delete the task {0}: {1}", this.tbTaskName.Text, ex.Message));
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
    }
}
