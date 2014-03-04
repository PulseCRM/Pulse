using System;
using System.Data;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.LP_Service;
using Microsoft.SharePoint.WebControls;
using Utilities;
using LoanActivities = LPWeb.Model.LoanActivities;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.LoanDetails
{
    public partial class WorkflowSetupTabConfirm : LayoutsPageBase
    {
        private string sErrorMsg = "Failed to load current page: invalid {0}.";
        private string sReturnPage = "WorkflowSetupTabConfirm.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["FileID"] != null) // 如果有GroupID
                {
                    string sFileID = Request.QueryString["FileID"];

                    if (PageCommon.IsID(sFileID) == false)
                    {
                        PageCommon.WriteJsEnd(this, string.Format(sErrorMsg, "FileID"), "window.location.href='" + sReturnPage + "'");
                    }

                    string workflowTemplId = Request.QueryString["wftId"];

                    if (PageCommon.IsID(workflowTemplId) == false)
                    {
                        PageCommon.WriteJsEnd(this, string.Format(sErrorMsg, "WorkflowTemplId"), "window.location.href='" + sReturnPage + "'");
                    }

                    CurrentFileId = Convert.ToInt32(sFileID);
                    WorkflowTemplId = Convert.ToInt32(workflowTemplId);
                }

                if (CurrentFileId < 1 || WorkflowTemplId < 1)
                {
                    return;
                }
            }
        }

        #region Properties

        protected int WorkflowTemplId
        {
            set { ViewState["wftId"] = value; }
            get
            {
                if (ViewState["wftId"] == null)
                    return 0;
                int wftId = 0;
                int.TryParse(ViewState["wftId"].ToString(), out wftId);

                return wftId;
            }
        }

        ///// <summary>
        ///// Gets or sets the current file id.
        ///// </summary>
        ///// <value>The current file id.</value>
        protected int CurrentFileId
        {
            set { ViewState["fileId"] = value; }
            get
            {
                if (ViewState["fileId"] == null)
                    return 0;
                int fileId = 0;
                int.TryParse(ViewState["fileId"].ToString(), out fileId);

                return fileId;
            }
        }

        #endregion

        protected void btnApply_Click(object sender, EventArgs e)
        {

            var request = new GenerateWorkflowRequest
                              {
                                  FileId = CurrentFileId,
                                  WorkflowTemplId = WorkflowTemplId,
                                  hdr = new ReqHdr {UserId = new LoginUser().iUserID}
                              };
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    var response = client.GenerateWorkflow(request);
                    if (!response.hdr.Successful)
                    {
                        PageCommon.AlertMsg(this, response.hdr.StatusInfo);
                        return;
                    }
                    else
                    {
                        var CurrentUserId = new LoginUser().iUserID;
                        Model.LoanWflTempl lWflTempModel = new Model.LoanWflTempl();
                        lWflTempModel.FileId = CurrentFileId;
                        lWflTempModel.ApplyBy = CurrentUserId;
                        lWflTempModel.ApplyDate = DateTime.Now;
                        lWflTempModel.WflTemplId = WorkflowTemplId;
                        BLL.LoanWflTempl bLoanWflTempl = new LoanWflTempl();
                        bLoanWflTempl.Apply(lWflTempModel);

                        PageCommon.WriteJs(this, "Applied workflow template successfully.", "parent.DialogClose();");
                    }
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ex)
            {
                PageCommon.WriteJs(this, "Failed to apply workflow template, reason: Workflow Manager service is not running.", "parent.DialogClose();");
            }
            catch (Exception ee)
            {
                PageCommon.WriteJs(this, String.Format("Failed to apply workflow template, reason: {0}.",ee.Message), "parent.DialogClose();");
            }

        }

        protected void btnApplyCancel_Click(object sender, EventArgs e)
        {
            //var wfTempName = new Template_Workflow().GetModel(WorkflowTemplId).Name;
            //Model.LoanActivities model = new LoanActivities
            //                                 {
            //                                     FileId = CurrentFileId,
            //                                     ActivityTime = DateTime.Now,
            //                                     UserId = new LoginUser().iUserID,
            //                                     ActivityName = string.Format("Select Workflow Template<{0}>", wfTempName)
            //                                 };

            //var id = new BLL.LoanActivities().Add(model);
            PageCommon.WriteJs(this, "", "parent.DialogClose();");
            //PageCommon.WriteJs(this, id <= 0 ? "Apply failed." : "Apply successful.", "parent.DialogClose();");
        }
    }
}