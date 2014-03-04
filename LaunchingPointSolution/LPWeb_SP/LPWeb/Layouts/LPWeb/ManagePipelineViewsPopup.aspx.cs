using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using LPWeb.Common;
using System.Web.UI.WebControls;

public partial class ManagePipelineViewsPopup : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 查询条件

        string sWhere = string.Empty;

        string sFilter_PipelineType = string.Empty;
        string sFilter_Enabled = string.Empty;

        if (this.Request.QueryString["PipelineType"] != null)
        {
            sFilter_PipelineType = this.Request.QueryString["PipelineType"];
            if (sFilter_PipelineType != "Loans" && sFilter_PipelineType != "Leads" && sFilter_PipelineType != "Clients")
            {
                this.Response.Redirect("ManagePipelineViewsPopup.aspx");
            }
            else
            {
                sWhere += " and PipelineType='" + sFilter_PipelineType + "'";
            }
        }

        if (this.Request.QueryString["Enable"] != null)
        {
            sFilter_Enabled = this.Request.QueryString["Enable"];
            if (sFilter_Enabled != "Yes" && sFilter_Enabled != "No")
            {
                this.Response.Redirect("ManagePipelineViewsPopup.aspx");
            }
            else
            {
                if (sFilter_Enabled == "Yes")
                {
                    sWhere += " and Enabled=1";
                }
                else
                {
                    sWhere += " and Enabled=0";
                }
            }
        }

        #endregion

        #region 排序

        string sOrderByField = "PipelineType,ViewName";
        if (this.Request.QueryString["OrderByField"] != null)
        {
            sOrderByField = this.Request.QueryString["OrderByField"];

            if (sOrderByField == "DefaultText")
            {
                sOrderByField = "PipelineType,ViewName";
            }
        }

        int iOrderByType = 0;  // default asc
        if (this.Request.QueryString["OrderByType"] != null)
        {
            string sOrderByType = this.Request.QueryString["OrderByType"];
            if (sOrderByType != "0")
            {
                sOrderByType = "1";
            }

            iOrderByType = Convert.ToInt32(sOrderByType);
        }

        string sAsc = " asc";
        if (iOrderByType == 0)
        {
            sAsc = " asc";
        }
        else
        {
            sAsc = " desc";
        }

        string sOrderBy = " order by " + sOrderByField + sAsc;

        #endregion

        #region 加载UserHomePref

        string sDefaultViewID_Loans = "";
        string sDefaultViewID_Leads = "";
        string sDefaultViewID_Clients = "";

        DataTable UserHomePrefInfo = this.GetUserHomePrefInfo(this.CurrUser.iUserID);
        //if (UserHomePrefInfo.Rows.Count == 0)
        //{
        //    PageCommon.WriteJsEnd(this, "There is no user profile info for current user.", string.Empty);
        //}
        if (UserHomePrefInfo.Rows.Count > 0)
        {
            sDefaultViewID_Loans = UserHomePrefInfo.Rows[0]["DefaultLoansPipelineViewId"].ToString();
            sDefaultViewID_Leads = UserHomePrefInfo.Rows[0]["DefaultLeadsPipelineViewId"].ToString();
            sDefaultViewID_Clients = UserHomePrefInfo.Rows[0]["DefaultClientsPipelineViewId"].ToString();
        }
        #endregion

        #region 加载ContactMailCampaign列表

        DataTable UserPipelineViewList = this.GetUserPipelineViewList(this.CurrUser.iUserID, sWhere, sOrderBy);

        #region add columns

        UserPipelineViewList.Columns.Add("EnabledText", typeof(string));
        UserPipelineViewList.Columns.Add("DefaultText", typeof(string));

        foreach (DataRow RowItem in UserPipelineViewList.Rows)
        {
            #region EnabledText

            if (RowItem["Enabled"].ToString() == "True")
            {
                RowItem["EnabledText"] = "Yes";
            }
            else
            {
                RowItem["EnabledText"] = "No";
            }

            #endregion

            #region DefaultText

            string sViewID = RowItem["UserPipelineViewID"].ToString();
            if (sViewID == sDefaultViewID_Loans || sViewID == sDefaultViewID_Leads || sViewID == sDefaultViewID_Clients)
            {
                RowItem["DefaultText"] = "Yes";
            }
            else
            {
                RowItem["DefaultText"] = "No";
            }

            #endregion
        }

        UserPipelineViewList.AcceptChanges();

        #endregion

        DataView xView = new DataView(UserPipelineViewList);

        if (this.Request.QueryString["OrderByField"] == "DefaultText")
        {
            xView.Sort = "DefaultText" + sAsc;
        }

        this.gridUserPipelineViewList.DataSource = xView;
        this.gridUserPipelineViewList.DataBind();

        #endregion
    }

    private DataTable GetUserPipelineViewList(int iUserID, string sWhere, string sOrderBy) 
    {
        string sSql = "select * from dbo.UserPipelineViews where UserId=" + iUserID + " " + sWhere + " " + sOrderBy;

        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private DataTable GetUserHomePrefInfo(int iUserID) 
    {
        string sSql = "select * from UserHomePref where UserId=" + iUserID;

        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql); 
    }
}