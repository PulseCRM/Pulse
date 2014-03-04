using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;
using LPWeb.BLL;
using System.Text;

public partial class LoanDetails_GetUserBranchAjax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // [{"BranchID":"1","Name":"BranchA"},{"BranchID":"2","Name":"BranchB"}]

        #region 校验页面参数

        // RegionID
        string sRegionID = this.Request.QueryString["RegionIDs"].ToString();

        // DivisionID
        string sDivisionIDs = this.Request.QueryString["DivisionIDs"].ToString();

        #endregion

        #region Branch

        Branches BranchMgr = new Branches();
        DataTable BranchList = null;

        if (this.CurrUser.sRoleName == "Executive")
        {
            BranchList = BranchMgr.GetBranchFilter_Executive(this.CurrUser.iUserID, sRegionID, sDivisionIDs);
        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            BranchList = BranchMgr.GetBranchFilter_Branch_Manager(this.CurrUser.iUserID, sRegionID, sDivisionIDs);
        }
        else
        {
            BranchList = BranchMgr.GetBranchFilter(this.CurrUser.iUserID, sRegionID, sDivisionIDs);
        }

        #endregion

        StringBuilder sbJson = new StringBuilder();

        int i = 0;
        foreach (DataRow BranchRow in BranchList.Rows)
        {
            string sBranchID = BranchRow["BranchID"].ToString();
            string sName = BranchRow["Name"].ToString();

            if (i == 0)
            {
                sbJson.Append("{\"BranchID\":\"" + sBranchID + "\",\"Name\":\"" + sName + "\"}");
            }
            else
            {
                sbJson.Append(",{\"BranchID\":\"" + sBranchID + "\",\"Name\":\"" + sName + "\"}");
            }

            i++;
        }

        this.Response.Write("[" + sbJson.ToString() + "]");
    }
}
