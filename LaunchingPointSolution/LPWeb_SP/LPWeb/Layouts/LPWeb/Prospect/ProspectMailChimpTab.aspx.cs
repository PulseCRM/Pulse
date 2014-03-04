using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;

public partial class Prospect_ProspectMailChimpTab : BasePage
{
    int iProspectID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string sProspectID =string.Empty;
        #region 校验页面参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "FileID", QueryStringType.ID);
        bool bIsValid1 = PageCommon.ValidateQueryString(this, "ContactID", QueryStringType.ID);

        if (bIsValid == false && bIsValid1 == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", string.Empty);
        }
        #region CR48

        if (Request.QueryString["FileID"] == null && Request.QueryString["ContactID"] == null)
        {
            sProspectID = "0";
            iProspectID = 0;
        }
        else if (Request.QueryString["FileID"] == null && Request.QueryString["ContactID"] != null)
        {
            sProspectID = Request.QueryString["ContactID"].ToString();
            if (Int32.TryParse(Request.QueryString["ContactID"].ToString(), out iProspectID) == false)
            {
                iProspectID = 0;
                sProspectID = "0";
            }
        }
        else
        {

            var sFileId = Request.QueryString["FileID"].ToString();
            LPWeb.BLL.Loans bllLoans = new LPWeb.BLL.Loans();
            this.iProspectID = bllLoans.GetBorrowerID(Convert.ToInt32(sFileId)).Value;
            sProspectID = this.iProspectID.ToString();
            this.hdnBorrowerID.Value = sProspectID;
        }
        #endregion

        #endregion

        #region 加载ContactMailCampaign列表

        string sSql = "select a.*, b.Name as MailChimpCampaignName, c.Name as MailChimpListName, d.LastName +', '+ d.FirstName as AddedByName "
                         + "from ContactMailCampaign as a "
                         + "left outer join MailChimpCampaigns as b on a.CID=b.CID "
                         + "left outer join MailChimpLists as c on a.LID=c.LID "
                         + "left outer join Users as d on a.AddedBy=d.UserId "
                         + "where ContactId=" + this.iProspectID;
        DataTable ContactMailCampaignList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        this.gridContactMailCampaignList.DataSource = ContactMailCampaignList;
        this.gridContactMailCampaignList.DataBind();

        #endregion
    }
}
