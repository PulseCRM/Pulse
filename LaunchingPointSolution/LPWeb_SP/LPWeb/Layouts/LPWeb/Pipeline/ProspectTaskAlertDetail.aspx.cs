using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using LPWeb.BLL;
using LPWeb.Common;
using Utilities;
using System.Collections.Generic;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    public partial class ProspectTaskAlertDetail : LayoutsPageBase
    {
        public DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BLL.Prospect bProspect = new BLL.Prospect();
                string sContactID = Request.QueryString["contactID"];
                int iContactID = 0;
                bool loantaskID_null = false;

                if (string.IsNullOrEmpty(sContactID))
                {
                    return;
                }


                if (int.TryParse(sContactID, out iContactID))
                {
                    try
                    {
                        hfdFileId.Value = iContactID.ToString();

                        dt = bProspect.GetTaskAlertDetail(iContactID);
                       
                        if (dt == null || dt.Rows.Count < 1)
                        {
                            PageCommon.WriteJs(this, "", "window.parent.closeDialog();");
                            return;
                        }

                        string taskIds = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            taskIds += row["ProspectTaskId"] + ",";
                        }
                        hfdAllTaskIds.Value = taskIds.TrimEnd(',');
                        //Get task id based on task table(when loan task id is null and file id not null)
                        if (dt.Rows.Count > 0)
                        {
                            hfdTaskId.Value = dt.Rows[0]["ProspectTaskId"].ToString();
                        }

                        LoginUser CurrentUser = new LoginUser();

                        if (!CurrentUser.userRole.SendEmail)
                        {
                            btnSendEmail.Enabled = false;
                        }

                        #region alex

                        //Get Task Owner and Current User's Make Others' Task Complate Power  by Alex 2011-01-22
                        string sTaskOwner = dt.Rows[0]["Owner"].ToString();
                        hdnTaskOwner.Text = sTaskOwner;
                        hdnLoginUserID.Value = CurrentUser.sFullName;
                        hdnMakeOtherTaskComp.Text = CurrentUser.userRole.MarkOtherTaskCompl == true ? "1" : "0";

                        #endregion

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
            //Complate 操作在前台调用的页面中完成
        }
    }
}
