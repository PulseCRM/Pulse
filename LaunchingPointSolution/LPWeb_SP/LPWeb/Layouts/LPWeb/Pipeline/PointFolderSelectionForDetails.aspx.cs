using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using LPWeb.LP_Service;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    public partial class PointFolderSelectionForDetails : BasePage
    {
        private string _strPstatus = string.Empty;
        private string strFileId = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            _strPstatus = Request.QueryString["tls"];
            strFileId = Request.QueryString["fid"];  // file id

            if (!IsPostBack)
            {

                string strTargetStatus = Request.QueryString["tls"];    // target folder status 
                string strBranchId = Request.QueryString["bid"];
                string err;
                if (string.IsNullOrEmpty(strFileId) || string.IsNullOrEmpty(strTargetStatus))
                {
                    err = string.Format("Invalid file Id: {0} or LoanStatus={1}.", strFileId, strTargetStatus);
                    LPLog.LogMessage(LogType.Logerror, err);
                    //PageCommon.WriteJsEnd(this, "The operation failed, "+err, PageCommon.Js_RefreshParent);
                    PageCommon.WriteJsEnd(this, "The operation failed, " + err, "window.parent.CloseGlobalPopup();");
                    return;
                }
                int iLoanId = Convert.ToInt32(strFileId);
                BLL.PointFolders PF = new BLL.PointFolders();
                string sqlCondStr = " 1>0 ";
                string orderby = "  Order By Name ";

                //CR60 
                BLL.Company_Point CPMgr = new BLL.Company_Point();
                Model.Company_Point CPModel = CPMgr.GetModel();
                //check if Company_Point.Enable_MultiBranchFolders=true
                //select FolderId, [Name] from PointFolders where (LoanStatus=<selected loan/lead status>) order by [Name] asc
                bool bMultBranchFolder = false;
                if (CPModel.Enable_MultiBranchFolders == true)
                {
                    bMultBranchFolder = true;
                }

                #region 需求变化，暂去掉Status控制，bug 1186  by Alex 20110904
                //if ("1" == Request.QueryString["forProspect"])
                //{
                //    switch (strTargetStatus.ToLower())
                //    {
                //        //change Request 011 : if the Loans.ProspectLoanStatus<>’Active’, display a list of prospect archive folders (PointFolders.LoanStatus=8) within the branch of the loan and invoke Point Manager’s MoveFile method as it’s currently doing.
                //        //case "converted":
                //        //    sqlCondStr = " LoanStatus=1 AND Enabled=1 ";         // processing
                //        //    break;
                //        case "active":
                //            sqlCondStr = " LoanStatus=6 AND Enabled=1 ";         // active prospect
                //            break;
                //        default:
                //            sqlCondStr = " LoanStatus=8 AND Enabled=1 ";  // " LoanStatus<>1 AND LoanStatus<>6 ";
                //            break;
                //    }
                //}
                //else
                //{
                //    if (strTargetStatus == "Processing")
                //    {
                //        strTargetStatus = "1";
                //    }
                //    else if (strTargetStatus == "Prospect")
                //    {
                //        strTargetStatus = "6";
                //    }
                //    else if (strTargetStatus == "Canceled")
                //    {
                //        strTargetStatus = "2";
                //    }
                //    else if (strTargetStatus == "Closed")
                //    {
                //        strTargetStatus = "3";
                //    }
                //    else if (strTargetStatus == "Denied")
                //    {
                //        strTargetStatus = "4";
                //    }
                //    else if (strTargetStatus == "Suspended")
                //    {
                //        strTargetStatus = "5";
                //    }
                //    else if (strTargetStatus == "Archive")
                //    {
                //        strTargetStatus = "7";
                //    }


                //    sqlCondStr = " (LoanStatus=" + strTargetStatus;

                //    if (strTargetStatus == "1" || strTargetStatus == "6")
                //        sqlCondStr += " AND Enabled=1) ";            // processing
                //    else
                //        sqlCondStr += " OR LoanStatus=7) ";
                //} 
                #endregion
                DataSet dsPF = null;
                string sProspect = "";
                if (strBranchId == "-1" || string.IsNullOrEmpty(strBranchId) || strBranchId == "0")
                {
                    sProspect = Request.QueryString["forProspect"] == null ? "" : Request.QueryString["forProspect"].ToString(); //是否是Lead调用
                    strBranchId = PF.GetLoanOfficerBranchID(iLoanId, (sProspect == "1" ? "lead" : ""));
                    if (strBranchId == "0") //查不到Branch信息
                    {
                        if (bMultBranchFolder == false)
                        {
                            dsPF = PF.GetListByLoanId(iLoanId, sqlCondStr + orderby);
                        }
                        else
                        {
                            dsPF = PF.GetList(sqlCondStr + orderby);
                        }
                    }
                    else
                    {
                        if (bMultBranchFolder == false)
                        {
                            sqlCondStr += " AND BranchId=" + strBranchId;
                        }
                        dsPF = PF.GetList(sqlCondStr + orderby);
                    }
                }
                else
                {
                    if (bMultBranchFolder == false)
                    {
                        sqlCondStr += " AND BranchId=" + strBranchId;
                    }
                    dsPF = PF.GetList(sqlCondStr + orderby);
                }
                this.gvFolder.DataSource = dsPF;
                this.gvFolder.DataBind();
            }
        }

        protected void btnSel_Click(object sender, EventArgs e)
        {
            string sFolderID = "0";
            // return selected record as XML
            foreach (GridViewRow row in gvFolder.Rows)
            {
                CheckBox ckbSelected = row.FindControl("ckbSelected") as CheckBox;
                if (ckbSelected.Checked)
                {
                    if (!string.IsNullOrEmpty(_strPstatus) && _strPstatus == "1")
                    {
                        //ClientFun("callback", string.Format("callBack('{0}','{1}');", gvFolder.DataKeys[row.RowIndex].Value, _strPstatus));
                        sFolderID = gvFolder.DataKeys[row.RowIndex].Value.ToString();
                        //return;
                    }
                    //ClientFun("callback", string.Format("callBack('{0}');", gvFolder.DataKeys[row.RowIndex].Value.ToString()));
                    sFolderID = gvFolder.DataKeys[row.RowIndex].Value.ToString();
                    //return;
                }
            }

            BLL.Loans LoansManager = new BLL.Loans();
            
            string sProspectStatus="";
            try
            {
                sProspectStatus = LoansManager.GetProspectStatusInfo(Convert.ToInt32(this.strFileId));
                string sFileName = LoansManager.GetProspectFileNameInfo(Convert.ToInt32(this.strFileId));  //bug 878
                if (sFileName == "")
                {
                    //PageCommon.AlertMsg(this, string.Format("The selected loan does not have a Point file. Please export the loan to a Point file using the Lead Detail page and try again."));
                    PageCommon.WriteJsEnd(this, "The selected loan does not have a Point file. Please export the loan to a Point file using the Lead Detail page and try again.", "window.parent.CloseGlobalPopup();");
                    return;
                }
               
            }
            catch
            {
                LPLog.LogMessage(LogType.Logerror, "Invalid loan status: " + sProspectStatus);
                return;
            }
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    MoveFileRequest req = new MoveFileRequest();
                    req.FileId = Convert.ToInt32(this.strFileId);
         //           req.LoanStatus = lse;
                    req.NewFolderId = Convert.ToInt32(sFolderID);
                    req.hdr = new ReqHdr();
                    req.hdr.UserId = CurrUser.iUserID;
         //           req.StatusDate = DateTime.Now;

                    MoveFileResponse response = client.MoveFile(req);
                    if (response.hdr.Successful)
                    {
                        PageCommon.WriteJsEnd(this, "", "window.parent.CloseGlobalPopup();");
                    }
                    else
                    {
                        LPLog.LogMessage(LogType.Logerror, string.Format("Failed to move file:{0}", response.hdr.StatusInfo));
                        //PageCommon.AlertMsg(this, response.hdr.StatusInfo);
                        //ClientFun("callback", "");
                        PageCommon.WriteJsEnd(this, response.hdr.StatusInfo, "window.parent.CloseGlobalPopup();");
                    }
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ee)
            {
                LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ee.Message));
                //PageCommon.AlertMsg(this, "Failed to move the Point file, reason: Point Manager is not running.");
                //ClientFun("callback", "");
                PageCommon.WriteJsEnd(this, "Failed to move the Point file, reason: Point Manager is not running.", "window.parent.CloseGlobalPopup();");
            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ex.Message));
                PageCommon.WriteJsEnd(this, ex.Message, "window.parent.CloseGlobalPopup();");
            }

        }

        /// <summary>
        /// Call client function
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

