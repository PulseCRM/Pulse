using System;
using System.Collections.Generic;
using System.Data;
using LPWeb.BLL;
using LPWeb.Common;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Utilities;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    public partial class TaskAlertDetail : LayoutsPageBase
    {
        public DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Loans bllLoans = new Loans();
                string strFileID = Request.QueryString["fileID"];
                string strloantaskID = Request.QueryString["LoanTaskId"];
                int iFileID = 0;
                int iloantaskID = 0;
                bool status = true;
                bool loantaskID_null = false;

                this.hdnCloseDialogCodes.Value = "";
                this.hdnCloseDialogCodes.Value = this.Request.QueryString["CloseDialogCodes"];
                if (string.IsNullOrEmpty(strFileID))
                {
                    return;
                }

                if (string.IsNullOrEmpty(strloantaskID))
                {
                    loantaskID_null = true;
                }
                else
                {
                    status = int.TryParse(strloantaskID, out iloantaskID);
                    hfdTaskId.Value = iloantaskID.ToString();
                }

                if (int.TryParse(strFileID, out iFileID))
                {
                    try
                    {
                        hfdFileId.Value = iFileID.ToString();
                        if (loantaskID_null == true)
                            dt = bllLoans.GetTaskAlertDetail(iFileID);
                        else
                            dt = bllLoans.GetTaskAlertDetail_loantaskID(iFileID, iloantaskID);

                        if (dt == null || dt.Rows.Count < 1)
                        {
                            tblNoDataMessage.Visible = true;
                            //PageCommon.WriteJs(this, "", "window.parent.closeDialog();");
                            return;
                        }

                        string taskIds = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            taskIds += row["LoanTaskId"] + ",";
                        }
                        hfdAllTaskIds.Value = taskIds.TrimEnd(',');
                        //Get task id based on task table(when loan task id is null and file id not null)
                        if (loantaskID_null && dt.Rows.Count > 0)
                        {
                            hfdTaskId.Value = dt.Rows[0]["LoanTaskId"].ToString();
                        }
                        hdnLoanStatus.Value = dt.Rows[0]["LoanStatus"] == DBNull.Value ? "" : dt.Rows[0]["LoanStatus"].ToString();
                        #region neo 2011-01-07 add send completion email

                        string sPrerequisiteID = dt.Rows[0]["PrerequisiteTaskId"].ToString();
                        if (sPrerequisiteID != string.Empty) // has prerequisite task
                        {
                            // get prerequisite task info
                            LoanTasks LoanTaskManager = new LoanTasks();
                            DataTable PrerequisiteInfo = LoanTaskManager.GetLoanTaskList(" and  FileId = " + iFileID + " and LoanTaskId = " + sPrerequisiteID);

                            if (PrerequisiteInfo.Rows.Count == 0)   // not exist
                            {
                                this.hdnHasUncompletePrerequisite.Text = "False";
                            }
                            else
                            {
                                string sCompleteDate = PrerequisiteInfo.Rows[0]["Completed"].ToString();
                                if (sCompleteDate == string.Empty)   // uncomplete
                                {
                                    this.hdnHasUncompletePrerequisite.Text = "True";
                                }
                                else
                                {
                                    this.hdnHasUncompletePrerequisite.Text = "False";
                                }
                            }
                        }
                        else
                        {
                            this.hdnHasUncompletePrerequisite.Text = "False";
                        }

                        LoginUser CurrentUser = new LoginUser();

                        if (!CurrentUser.userRole.SendEmail)
                        {
                            btnSendEmail.Enabled = false;
                        }

                        #endregion

                        #region alex

                        //Get Task Owner and Current User's Make Others' Task Complate Power  by Alex 2011-01-22
                        string sTaskOwner = dt.Rows[0]["OwnerId"] == DBNull.Value ? "0" : dt.Rows[0]["OwnerId"].ToString();
                        hdnTaskOwner.Text = sTaskOwner;
                        hdnMakeOtherTaskComp.Text = CurrentUser.userRole.MarkOtherTaskCompl == true ? "1" : "0";
                        hdnUserId.Value = CurrentUser.iUserID.ToString();
                        if (sTaskOwner.Trim() == hdnUserId.Value.Trim() || sTaskOwner.Trim() == "0")       // if the user id and task owner are the same, allow
                            hdnMakeOtherTaskComp.Text = "1";
                        #endregion
                        if (dt == null || dt.Rows.Count < 1)
                        {
                            tblNoDataMessage.Visible = true;
                        }

                        fvTaskAlertDetail.DataSource = dt;
                        fvTaskAlertDetail.DataBind();
                    }
                    catch (Exception exception)
                    {
                        LPLog.LogMessage(exception.Message);
                        return;
                    }
                }
            }
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            if (hfdFileId.Value.Trim() == "")
            {
                PageCommon.AlertMsg(this, "Invalid parameter:'File ID'.");
                return;
            }

            LoanTasks bllLoanTask = new LoanTasks();

            var dtTasks = new List<Model.LoanTasks>();
            if (hfdTaskId.Value.Trim() != "")
            {
                dtTasks = bllLoanTask.GetModelList("LoanTaskId=" + hfdTaskId.Value);
            }
            else
            {
                dtTasks = bllLoanTask.GetModelList("FileId=" + hfdFileId.Value);
            }

            if (dtTasks == null || dtTasks.Count == 0)
            {
                PageCommon.AlertMsg(this, "There is no tasks in database.");
                return;
            }

            Dictionary<int, int> successTasks = new Dictionary<int, int>();
            int userId = new LoginUser().iUserID;
            int emailTmpId = 0;
            foreach (var task in dtTasks)
            {
                bool bIsSuccess = DAL.WorkflowManager.CompleteTask(task.LoanTaskId, userId, ref emailTmpId);
                if (bIsSuccess == true)
                {
                    successTasks.Add(task.LoanTaskId, emailTmpId);
                }
            }

            if (successTasks.Count == dtTasks.Count)
            {
                PageCommon.RegisterJsMsg(this, "Completed task(s) Successfuly!", "parent.closeDialog();");
                //PageCommon.AlertMsg(this,"Complete task(s) Successfuly!");
            }
            else
            {
                PageCommon.RegisterJsMsg(this, "There are " + successTasks.Count + " success and " + (dtTasks.Count - successTasks.Count) + " failture of " + dtTasks.Count + " Tasks.", "parent.closeDialog();");
                //PageCommon.AlertMsg(this, "There are " + successTasks.Count + " success and " + (dtTasks.Count - successTasks.Count) + " failture of " + dtTasks.Count + " Tasks.");
            }


        }
    }
}
