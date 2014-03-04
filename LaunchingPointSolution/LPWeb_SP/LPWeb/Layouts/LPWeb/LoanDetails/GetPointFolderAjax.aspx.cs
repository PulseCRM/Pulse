using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;
using LPWeb.BLL;
using System.Text;

public partial class LoanDetails_GetPointFolderAjax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // [{"FolderID":"1","Name":"FolderA"},{"FolderID":"2","Name":"FolderB"}]

        #region 校验页面参数

        // RegionID
        string sRegionIDs = this.Request.QueryString["RegionIDs"].ToString();

        // DivisionID
        string sDivisionIDs = this.Request.QueryString["DivisionIDs"].ToString();
        
        // BranchIDs
        string sBranchIDs = this.Request.QueryString["BranchIDs"].ToString();
        
        #endregion

        #region Point Folder

        PointFolders PointFolderMgr = new PointFolders();
        DataTable PointFolderList = null;

        if (this.CurrUser.sRoleName == "Executive")
        {
            PointFolderList = PointFolderMgr.GetPointFolder_Executive(this.CurrUser.iUserID, sRegionIDs, sDivisionIDs, sBranchIDs);
        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            PointFolderList = PointFolderMgr.GetPointFolder_BranchManager(this.CurrUser.iUserID, sRegionIDs, sDivisionIDs, sBranchIDs);
        }
        else
        {
            PointFolderList = PointFolderMgr.GetPointFolder_User(this.CurrUser.iUserID, sRegionIDs, sDivisionIDs, sBranchIDs);
        }

        #endregion

        StringBuilder sbJson = new StringBuilder();

        int i = 0;
        foreach (DataRow PointFolderRow in PointFolderList.Rows)
        {
            string sFolderID = PointFolderRow["FolderID"].ToString();
            string sName = PointFolderRow["Name"].ToString();

            if (i == 0)
            {
                sbJson.Append("{\"FolderID\":\"" + sFolderID + "\",\"Name\":\"" + sName + "\"}");
            }
            else
            {
                sbJson.Append(",{\"FolderID\":\"" + sFolderID + "\",\"Name\":\"" + sName + "\"}");
            }

            i++;
        }

        this.Response.Write("[" + sbJson.ToString() + "]");
    }
}
