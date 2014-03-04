using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.LoanDetails
{
    public partial class CloseLoanPopupWindowOpen : LayoutsPageBase
    {
        LoginUser CurrentUser = new LoginUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strFileId = Request.QueryString["fid"];  // file id
                if (string.IsNullOrEmpty(strFileId))
                {
                    PageCommon.WriteJsEnd(this, "Invalid FileId=" + strFileId, "window.parent.close();");
                    return;
                }
                int FileId = Convert.ToInt32(strFileId);
                if (FileId <= 0)
                {
                    PageCommon.WriteJsEnd(this, "Invalid FileId.", "window.parent.close();");
                    return;
                }

                string strTargetStatus = "3";
                //string strTargetStatus = Request.QueryString["tls"];    // target folder status 
                int iBranchId = 0;
                string strStatus7 = " OR LoanStatus='7'";

                BLL.PointFiles pFile = new BLL.PointFiles();
                iBranchId = pFile.GetPointFileBrancId(strFileId, "");
                BLL.PointFolders PF = new BLL.PointFolders();
                string sqlCondStr = string.Format(" BranchId='{0}' AND (LoanStatus='{1}' {2})", iBranchId, strTargetStatus, strStatus7);

                //DataSet dsPF = PF.GetList(20, 1,sqlCondStr);
                DataSet dsPF = PF.GetList(sqlCondStr);
                if (dsPF == null || dsPF.Tables[0].Rows.Count <= 0)
                {
                    PageCommon.WriteJsEnd(this, "No Closed or Archived Loan folder found for the branch.", "window.parent.close();");
                    return;
                }
                this.gvFolder.DataSource = dsPF;
                this.gvFolder.DataBind();
                hdnUserId.Value = CurrentUser.iUserID.ToString();
                hdnFileId.Value = strFileId;
            }
        }

        private void CloseLoan(int FolderId)
        {
            int FileId = Convert.ToInt32(hdnFileId.Value);
            int UserId = Convert.ToInt32(hdnUserId.Value);
            string ErrMsg = LoanTaskCommon.CloseLoan(FileId, UserId, FolderId);
            if (ErrMsg != null && ErrMsg.Length > 0)
            {
                PageCommon.WriteJsEnd(this, ErrMsg, "window.parent.close();");
            }
        }

        protected void lbtnSelect_Click(object sender, EventArgs e)
        {
            // return selected record as XML
            int count = 0;
            int FolderId = 0;
            foreach (GridViewRow row in gvFolder.Rows)
            {
                CheckBox ckbSelected = row.FindControl("ckbSelected") as CheckBox;
                if (ckbSelected.Checked)
                {
                    count++;
                    FolderId = Convert.ToInt32(gvFolder.DataKeys[row.RowIndex].Value.ToString());
                }
            }
            if (count <= 0)
            {
                PageCommon.WriteJsEnd(this, "No Point folder has been selected.", PageCommon.Js_RefreshSelf);
                return;
            }
            if (count > 1)
            {
                PageCommon.WriteJsEnd(this, "Only one Point folder can be selected.", PageCommon.Js_RefreshSelf);
                return;
            }
            CloseLoan(FolderId);
        }

    }
}

