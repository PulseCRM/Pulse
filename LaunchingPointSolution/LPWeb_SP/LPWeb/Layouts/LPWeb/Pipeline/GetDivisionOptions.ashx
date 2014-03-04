<%@ WebHandler Language="C#" Class="Pipeline_GetDivisionOptions" %>

using System;
using System.Web;

public class Pipeline_GetDivisionOptions : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {

        // json示例
        // {"ExecResult":"Success","Options":"<option value=''>All</option><option value='1'>Division 1A</option>"}
        // {"ExecResult":"Failed","ErrorMsg":"Failed to get division options."}

        context.Response.ContentType = "text/plain";

        #region 接收页面参数

        // RegionID
        if (context.Request.QueryString["RegionID"] == null)
        {
            context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            return;
        }

        string sRegionID = context.Request.QueryString["RegionID"].ToString().Trim();
        if (sRegionID != string.Empty && this.IsID(sRegionID) == false) {

            context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid region id.\"}");
            return;
        }

        #endregion

        #region 加载Division List
        
        System.Data.DataTable DivisionListData = null;

        try
        {
            DivisionListData = this.GetDivisionList(sRegionID);

            System.Data.DataRow NewDivisionRow = DivisionListData.NewRow();
            NewDivisionRow["DivisionId"] = DBNull.Value;
            NewDivisionRow["Name"] = "All";

            DivisionListData.Rows.InsertAt(NewDivisionRow, 0);
        }
        catch (Exception)
        {
            string sErrorMsg = "Failed to get division options.";
            context.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
            return;
        }

        #endregion

        #region Build Division Options

        System.Text.StringBuilder sbOptions = new System.Text.StringBuilder();
        foreach (System.Data.DataRow DivisionRow in DivisionListData.Rows)
        {
            string sDivisionID = DivisionRow["DivisionId"].ToString();
            string sDivision = DivisionRow["Name"].ToString();

            sbOptions.Append("<option value='" + sDivisionID + "'>" + sDivision + "</option>");
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
    /// get division list
    /// neo 2011-04-26
    /// </summary>
    /// <returns></returns>
    private System.Data.DataTable GetDivisionList(string sRegionID)
    {
        string sWhere = string.Empty;
        if (sRegionID != string.Empty)
        {
            sWhere = " and RegionID=" + sRegionID;
        }

        string sSql = "select * from Divisions where Enabled=1 " + sWhere;
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