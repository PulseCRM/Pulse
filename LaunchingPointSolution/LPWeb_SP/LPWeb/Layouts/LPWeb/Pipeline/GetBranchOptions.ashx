<%@ WebHandler Language="C#" Class="Pipeline_GetBranchOptions" %>

using System;
using System.Web;

public class Pipeline_GetBranchOptions : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {

        // json示例
        // {"ExecResult":"Success","Options":"<option value=''>All</option><option value='1'>Branch 1A</option>"}
        // {"ExecResult":"Failed","ErrorMsg":"Failed to get Branch options."}

        context.Response.ContentType = "text/plain";

        #region 接收页面参数

        // RegionID and DivisionID
        if (context.Request.QueryString["RegionID"] == null && context.Request.QueryString["DivisionID"] == null)
        {
            context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            return;
        }
        
        if (context.Request.QueryString["RegionID"] != null && context.Request.QueryString["DivisionID"] != null)
        {
            context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            return;
        }

        string sRegionID = string.Empty;
        if (context.Request.QueryString["RegionID"] != null)
        {
            sRegionID = context.Request.QueryString["RegionID"].ToString().Trim();
            if (sRegionID != string.Empty && this.IsID(sRegionID) == false)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid region id.\"}");
                return;
            }
        }

        string sDivisionID = string.Empty;
        if (context.Request.QueryString["DivisionID"] != null)
        {
            sDivisionID = context.Request.QueryString["DivisionID"].ToString().Trim();
            if (sDivisionID != string.Empty && this.IsID(sDivisionID) == false)
            {
                context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid Division id.\"}");
                return;
            }
        }

        #endregion

        #region 加载Branch List
        
        System.Data.DataTable BranchListData = null;

        try
        {
            if (context.Request.QueryString["RegionID"] != null)
            {
                BranchListData = this.GetBranchListByRegion(sRegionID);
            }
            else
            {
                BranchListData = this.GetBranchList(sDivisionID);
            }

            System.Data.DataRow NewBranchRow = BranchListData.NewRow();
            NewBranchRow["BranchId"] = DBNull.Value;
            NewBranchRow["Name"] = "All";

            BranchListData.Rows.InsertAt(NewBranchRow, 0);
        }
        catch (Exception)
        {
            string sErrorMsg = "Failed to get Branch options.";
            context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
            return;
        }

        #endregion

        #region Build Branch Options

        System.Text.StringBuilder sbOptions = new System.Text.StringBuilder();
        foreach (System.Data.DataRow BranchRow in BranchListData.Rows)
        {
            string sBranchID = BranchRow["BranchId"].ToString();
            string sBranch = BranchRow["Name"].ToString();

            sbOptions.Append("<option value='" + sBranchID + "'>" + sBranch + "</option>");
        }

        #endregion

        context.Response.Write("{\"ExecResult\":\"Success\",\"Options\":\"" + sbOptions.ToString() + "\"}");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    /// <summary>
    /// get branch list
    /// neo 2011-04-26
    /// </summary>
    /// <returns></returns>
    private System.Data.DataTable GetBranchListByRegion(string sRegionID)
    {
        string sWhere = string.Empty;
        if (sRegionID != string.Empty)
        {
            sWhere = " and RegionID=" + sRegionID;
        }

        string sSql = "select * from Branches where Enabled=1 " + sWhere;
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    /// <summary>
    /// get branch list
    /// neo 2011-04-26
    /// </summary>
    /// <returns></returns>
    private System.Data.DataTable GetBranchList(string sDivisionID)
    {
        string sWhere = string.Empty;
        if (sDivisionID != string.Empty)
        {
            sWhere = " and DivisionID=" + sDivisionID;
        }

        string sSql = "select * from Branches where Enabled=1 " + sWhere;
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private bool IsID(string sID)
    {
        int iInt;
        bool bIsInt = int.TryParse(sID, out iInt);
        if (bIsInt == false)
        {
            return false;
        }

        if (iInt >= -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}