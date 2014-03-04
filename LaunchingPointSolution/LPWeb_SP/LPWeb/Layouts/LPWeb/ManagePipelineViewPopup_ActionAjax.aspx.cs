using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data.SqlClient;
using System.Data;

public partial class ManagePipelineViewPopup_ActionAjax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"错误信息"}


        #region 校验页面参数

        #region Action

        string sError_Lost = "Lost required query string.";
        string sError_Invalid = "Invalid query string.";

        if (this.Request.QueryString["Action"] == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError_Lost + "\"}");
            this.Response.End();
        }

        string sAction = this.Request.QueryString["Action"];
        if (sAction != "Enable" && sAction != "Disable" && sAction != "MakeDefault" && sAction != "Delete")
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError_Invalid + "\"}");
            this.Response.End();
        }

        #endregion

        #region PipelineViewIDs

        if (this.Request.QueryString["PipelineViewIDs"] == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError_Lost + "\"}");
            this.Response.End();
        }

        string sPipelineViewIDs = this.Request.QueryString["PipelineViewIDs"];
        if (sPipelineViewIDs == string.Empty)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError_Invalid + "\"}");
            this.Response.End();
        }

        string[] PipelineViewIDArray = sPipelineViewIDs.Split(',');

        #endregion

        #endregion

        #region Do Actions

        if (sAction == "Enable")
        {
            #region Enable

            try
            {
                foreach (string sID in PipelineViewIDArray)
                {
                    string sSql = "update UserPipelineViews set Enabled=1 where UserPipelineViewID=" + sID;
                    LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
                }
            }
            catch (Exception ex)
            {
                this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"exception happened when enable pipeline view.\"}");
                this.Response.End();
            }

            #endregion
        }
        else if (sAction == "Disable")
        {
            #region Disable

            try
            {
                foreach (string sID in PipelineViewIDArray)
                {
                    string sSql = "update UserPipelineViews set Enabled=0 where UserPipelineViewID=" + sID;
                    LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
                }
            }
            catch (Exception ex)
            {
                this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"exception happened when disable pipeline view.\"}");
                this.Response.End();
            }

            #endregion
        }
        else if (sAction == "MakeDefault")
        {
            #region MakeDefault

            try
            {
                foreach (string sID in PipelineViewIDArray)
                {
                    string sSql0 = "select * from UserPipelineViews where UserPipelineViewID=" + sID;
                    DataTable PipelineViewInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
                    if (PipelineViewInfo.Rows.Count == 0)
                    {
                        continue;
                    }

                    string sPipelineType = PipelineViewInfo.Rows[0]["PipelineType"].ToString();

                    if (sPipelineType == "Loans")
                    {
                        string sSql = "update UserHomePref set DefaultLoansPipelineViewId=" + sID + " where UserId=" + this.CurrUser.iUserID;
                        LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
                    }
                    else if (sPipelineType == "Leads")
                    {
                        string sSql = "update UserHomePref set DefaultLeadsPipelineViewId=" + sID + " where UserId=" + this.CurrUser.iUserID;
                        LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
                    }
                    else if (sPipelineType == "Clients")
                    {
                        string sSql = "update UserHomePref set DefaultClientsPipelineViewId=" + sID + " where UserId=" + this.CurrUser.iUserID;
                        LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"exception happened when make pipeline view default.\"}");
                this.Response.End();
            }


            #endregion
        }
        else if (sAction == "Delete")
        {
            #region Delete

            try
            {
                foreach (string sID in PipelineViewIDArray)
                {
                    string sSql = "delete from UserPipelineViews where UserPipelineViewID=" + sID;
                    LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
                }
            }
            catch (Exception ex)
            {
                this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"exception happened when delete pipeline view.\"}");
                this.Response.End();
            }

            #endregion
        }

        #endregion

        this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
        this.Response.End();
    }
}
