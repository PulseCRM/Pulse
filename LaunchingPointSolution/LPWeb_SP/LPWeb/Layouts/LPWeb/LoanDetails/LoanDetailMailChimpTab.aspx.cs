using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;
using System.Data.SqlClient;

public partial class LoanDetails_LoanDetailMailChimpTab : BasePage
{
    int iFileID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "FileID", QueryStringType.ID);

        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", string.Empty);
        }

        string sFileID = this.Request.QueryString["FileID"].ToString();
        this.iFileID = Convert.ToInt32(sFileID);

        #endregion

        #region 获取BorrowerID和CoBorrowerID

        string sSql2 = "select ContactId from LoanContacts where FileId=" + this.iFileID + " and ContactRoleId=dbo.lpfn_GetBorrowerRoleId()";
        DataTable BorrowerInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql2);
        if (BorrowerInfo.Rows.Count > 0) 
        {
            this.hdnBorrowerID.Value = BorrowerInfo.Rows[0]["ContactId"].ToString();
        }

        string sSql3 = "select ContactId from LoanContacts where FileId=" + this.iFileID + " and ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId()";
        DataTable CoBorrowerInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql3);
        if (CoBorrowerInfo.Rows.Count > 0)
        {
            this.hdnCoBorrowerID.Value = CoBorrowerInfo.Rows[0]["ContactId"].ToString();
        }

        #endregion

        #region 加载列表数据

        #region get row count

        int iRowCount = this.GetContactMailCampaignRowCount();
        this.AspNetPager1.RecordCount = iRowCount;

        #endregion

        #region Calc. StartIndex and EndIndex

        int iPageSize = this.AspNetPager1.PageSize;
        int iPageIndex = 1;
        if (this.Request.QueryString["PageIndex"] != null)
        {
            iPageIndex = Convert.ToInt32(this.Request.QueryString["PageIndex"]);
        }
        int iStartIndex = PageCommon.CalcStartIndex(iPageIndex, iPageSize);
        int iEndIndex = PageCommon.CalcEndIndex(iStartIndex, iPageSize, iRowCount);

        #endregion

        #region 加载ContactMailCampaign列表

        DataTable ContactMailCampaignList = this.GetContactMailCampaignList(iStartIndex, iEndIndex);

        this.gridContactMailCampaignList.DataSource = ContactMailCampaignList;
        this.gridContactMailCampaignList.DataBind();

        #endregion

        #endregion
    }

    private int GetContactMailCampaignRowCount() 
    {
        string sSql = "select a.*, b.Name as MailChimpCampaignName, c.Name as MailChimpListName, d.LastName +', '+ d.FirstName as AddedByName "
                    + "from ContactMailCampaign as a "
                    + "left outer join MailChimpCampaigns as b on a.CID=b.CID "
                    + "left outer join MailChimpLists as c on a.LID=c.LID "
                    + "left outer join Users as d on a.AddedBy=d.UserId "
                    + "where ContactId in (select ContactId from LoanContacts where FileId=" + this.iFileID + " and (ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId()))";
        
        // row count
        int iRowCount = LPWeb.DAL.DbHelperSQL.Count("(" + sSql + ") as t", "");

        return iRowCount;
    }

    private DataTable GetContactMailCampaignList(int iStartIndex, int iEndIndex)
    {
        string sSql = "select a.*, b.Name as MailChimpCampaignName, c.Name as MailChimpListName, d.LastName +', '+ d.FirstName as AddedByName "
                    + "from ContactMailCampaign as a "
                    + "left outer join MailChimpCampaigns as b on a.CID=b.CID "
                    + "left outer join MailChimpLists as c on a.LID=c.LID "
                    + "left outer join Users as d on a.AddedBy=d.UserId "
                    + "where ContactId in (select ContactId from LoanContacts where FileId=" + this.iFileID + " and (ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId()))";

        SqlCommand SqlCmd = new SqlCommand("lpsp_ExecSqlByPager");
        SqlCmd.CommandType = CommandType.StoredProcedure;

        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@OrderByField", SqlDbType.NVarChar, "ContactMailCampaignId");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@AscOrDesc", SqlDbType.NVarChar, "asc");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Fields", SqlDbType.NVarChar, "*");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DbTable", SqlDbType.NVarChar, "(" + sSql + ") as t");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Where", SqlDbType.NVarChar, "");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@StartIndex", SqlDbType.Int, iStartIndex);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@EndIndex", SqlDbType.Int, iEndIndex);

        DataTable x = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd);

        return x;
    }
}
