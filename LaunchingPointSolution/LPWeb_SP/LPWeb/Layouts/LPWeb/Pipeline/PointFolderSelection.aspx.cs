using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using LPWeb.BLL;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    /// <summary>
    /// Point Folder selection page
    /// Author：Peter
    /// Date：2010-09-04
    /// </summary>
    public partial class PointFolderSelection : BasePage
    {
        private string _strPstatus = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            _strPstatus = Request.QueryString["tls"];

            if (!IsPostBack)
            {
                string strFileId = Request.QueryString["fid"];  // file id

                string strTargetStatus = Request.QueryString["tls"];    // target folder status 
                string strBranchId = Request.QueryString["bid"];
                string sType = Request.QueryString["type"] == null ? "" : Request.QueryString["type"]; //Loan List 页面Dispose传递专用参数
                string err;
                if (string.IsNullOrEmpty(strFileId) || string.IsNullOrEmpty(strTargetStatus))
                {
                    err = string.Format("Invalid file Id: {0} or LoanStatus={1}.", strFileId, strTargetStatus);
                    LPLog.LogMessage(LogType.Logerror, err);
                    PageCommon.WriteJsEnd(this, "The operation failed, "+err, PageCommon.Js_RefreshParent);
                    ClientFun("callback", string.Format("callBack('{0}');", -1));
                    return;
                }

                Loans LoansManager = new Loans();

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
                if (sType == "dispose")
                {
                    if ("1" == Request.QueryString["forProspect"])
                    {
                        switch (strTargetStatus.ToLower())
                        {
                            case "converted":
                                sqlCondStr = " LoanStatus=1 AND Enabled=1 ";         // processing
                                break;
                            case "active":
                                sqlCondStr = " LoanStatus=6 AND Enabled=1 ";         // active prospect
                                break;
                            default:
                                sqlCondStr = " LoanStatus=8 AND Enabled=1 ";  // " LoanStatus<>1 AND LoanStatus<>6 ";
                                break;
                        }
                    }
                    else if ("2" == Request.QueryString["forProspect"])
                    {
                        sqlCondStr = " LoanStatus=8 AND Enabled=1 ";  // " LoanStatus<>1 AND LoanStatus<>6 ";
                    }
                    else
                    {
                        if (strTargetStatus == "Processing")
                        {
                            strTargetStatus = "1";
                        }
                        else if (strTargetStatus == "Prospect")
                        {
                            strTargetStatus = "6";
                        }
                        else if (strTargetStatus == "Canceled")
                        {
                            strTargetStatus = "2";
                        }
                        else if (strTargetStatus == "Closed")
                        {
                            strTargetStatus = "3";
                        }
                        else if (strTargetStatus == "Denied")
                        {
                            strTargetStatus = "4";
                        }
                        else if (strTargetStatus == "Suspended")
                        {
                            strTargetStatus = "5";
                        }
                        else if (strTargetStatus == "Archive")
                        {
                            strTargetStatus = "7";
                        }


                        sqlCondStr = " (LoanStatus=" + strTargetStatus;

                        if (strTargetStatus == "1" || strTargetStatus == "6")
                            sqlCondStr += " AND Enabled=1) ";            // processing
                        else
                            sqlCondStr += " OR LoanStatus=7) ";
                    } 
                }
                #endregion
                string sFileName = LoansManager.GetProspectFileNameInfo(Convert.ToInt32(strFileId));
                 if (sFileName == "" && strTargetStatus.ToLower() != "converted") //bug 877
                 {
                     ClientFun("callback", string.Format("callBack('{0}');", "0"));
                     return;
                 }



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

        protected void lbtnSelect_Click(object sender, EventArgs e)
        {
            // return selected record as XML
            foreach (GridViewRow row in gvFolder.Rows)
            {
                CheckBox ckbSelected = row.FindControl("ckbSelected") as CheckBox;
                if (ckbSelected.Checked)
                {
                    if (!string.IsNullOrEmpty(_strPstatus) && _strPstatus == "1")
                    {
                        ClientFun("callback", string.Format("callBack('{0}','{1}');", gvFolder.DataKeys[row.RowIndex].Value, _strPstatus));
                        return;
                    }
                    ClientFun("callback", string.Format("callBack('{0}');", gvFolder.DataKeys[row.RowIndex].Value.ToString()));
                    return;
                }
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
