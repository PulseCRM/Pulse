using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;
using LPWeb.BLL;
using System.Text;

public partial class LoanDetails_GetUserDivisionAjax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // [{"DivisionID":"1","Name":"DivisionA"},{"DivisionID":"2","Name":"DivisionB"}]

        #region 校验页面参数

        // RegionID
        string sRegionIDs = this.Request.QueryString["RegionIDs"].ToString();
        
        #endregion

        #region Division

        Divisions DivisionMgr = new Divisions();
        DataTable DivisionList = null;

        if (this.CurrUser.sRoleName == "Executive")
        {
            DivisionList = DivisionMgr.GetDivisionFilter_Executive(this.CurrUser.iUserID, sRegionIDs);
        }
        else if (this.CurrUser.sRoleName == "Branch Manager")
        {
            DivisionList = DivisionMgr.GetDivisionFilter_Branch_Manager(this.CurrUser.iUserID, sRegionIDs);
        }
        else
        {
            DivisionList = DivisionMgr.GetDivisionFilter(this.CurrUser.iUserID, sRegionIDs);
        }

        #endregion

        StringBuilder sbJson = new StringBuilder();

        int i = 0;
        foreach (DataRow DivisionRow in DivisionList.Rows)
        {
            string sDivisionID = DivisionRow["DivisionID"].ToString();
            string sName = DivisionRow["Name"].ToString();

            if (i == 0)
            {
                sbJson.Append("{\"DivisionID\":\"" + sDivisionID + "\",\"Name\":\"" + sName + "\"}");
            }
            else
            {
                sbJson.Append(",{\"DivisionID\":\"" + sDivisionID + "\",\"Name\":\"" + sName + "\"}");
            }

            i++;
        }

        this.Response.Write("[" + sbJson.ToString() + "]");
    }
}
