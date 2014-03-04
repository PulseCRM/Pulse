using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;

public partial class Settings_CompanyMarketing : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 加载Company_General信息

        string sSql0 = "select top(1) * from Company_General";
        DataTable CompanyGeneralInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
        if (this.IsPostBack == false)
        {
            //this.chkEnableMarketing.Checked = CompanyGeneralInfo.Rows[0]["EnableMarketing"] == DBNull.Value ? false : Convert.ToBoolean(CompanyGeneralInfo.Rows[0]["EnableMarketing"]);
            this.chkEnableMarketing.Checked = false;
            this.chkEnableMarketing.Enabled = false;
            this.btnSync.Enabled = false;
        }

        #endregion

        #region 加载Campaign列表

        string sSql = "select a.Enabled as AutoCampaignEnabled, * from AutoCampaigns as a inner join MarketingCampaigns as b on a.CampaignId=b.CampaignId "
                    + "inner join MarketingCategory as c on b.CategoryId=c.CategoryId "
                    + "inner join Template_Rules as d on a.CampaignId=d.AutoCampaignId ";

        DataTable CampaignList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        this.gridCampaignList.DataSource = CampaignList;
        this.gridCampaignList.DataBind();

        #endregion
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        LoginUser CurrentUser = new LoginUser();

        bool bEnableMarketing = false;//this.chkEnableMarketing.Checked;
        string sCategoryIDs = this.hdnCategoryIDs.Value;
        string sCampaignIDs = this.hdnCampaignIDs.Value;
        string sRuleIDs = this.hdnRuleIDs.Value;
        string sEnableStatuses = this.hdnEnableStatuses.Value;

        #region Build SqlCommand List

        string[] CampaignIDArray = sCampaignIDs.Split(',');
        string[] RuleIDArray = sRuleIDs.Split(',');
        string[] EnableStatusArray = sEnableStatuses.Split(',');

        Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

        string sSql0 = string.Empty;
        if (sCampaignIDs == string.Empty)
        {
            sSql0 = "update Template_Rules set AutoCampaignId=null";
        }
        else
        {
            sSql0 = "update Template_Rules set AutoCampaignId=null where AutoCampaignId in (" + sCampaignIDs + ")";
        }
        SqlCommand SqlCmd0 = new SqlCommand(sSql0);
        SqlCmdList.Add(SqlCmd0);

        string sSql2 = "delete from AutoCampaigns where PaidBy=0";
        SqlCommand SqlCmd2 = new SqlCommand(sSql2);
        SqlCmdList.Add(SqlCmd2);

        if (sCampaignIDs != string.Empty)
        {
            for (int i = 0; i < CampaignIDArray.Length; i++)
            {
                string sCampaignID = CampaignIDArray[i];
                string sRuleID = RuleIDArray[i];
                string sEnabled = EnableStatusArray[i];

                string sSql3 = "insert into AutoCampaigns (CampaignId, PaidBy, Enabled, SelectedBy, Started) values (" + sCampaignID + ", 0, @Enabled, " + CurrentUser.iUserID + ", getdate())";
                SqlCommand SqlCmd3 = new SqlCommand(sSql3);
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd3, "@Enabled", SqlDbType.Bit, bool.Parse(sEnabled));
                SqlCmdList.Add(SqlCmd3);

                string sSql5 = "update Template_Rules set AutoCampaignId=" + sCampaignID + " where RuleId=" + sRuleID;
                SqlCommand SqlCmd5 = new SqlCommand(sSql5);
                SqlCmdList.Add(SqlCmd5);

            }
        }

        string sSql4 = "update Company_General set EnableMarketing=@EnableMarketing";
        SqlCommand SqlCmd4 = new SqlCommand(sSql4);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd4, "@EnableMarketing", SqlDbType.Bit, bEnableMarketing);
        SqlCmdList.Add(SqlCmd4);

        #endregion

        #region 批量执行SQL语句

        SqlConnection SqlConn = null;
        SqlTransaction SqlTrans = null;

        try
        {
            SqlConn = LPWeb.DAL.DbHelperSQL.GetOpenConnection();
            SqlTrans = SqlConn.BeginTransaction();

            foreach (SqlCommand SqlCmdItem in SqlCmdList)
            {
                LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmdItem, SqlTrans);
            }

            SqlTrans.Commit();
        }
        catch (Exception ex)
        {
            SqlTrans.Rollback();
            throw ex;
        }
        finally
        {
            if (SqlConn != null)
            {
                SqlConn.Close();
            }
        }

        #endregion

        // success
        PageCommon.WriteJsEnd(this, "Auto campaign saved successfully.", PageCommon.Js_RefreshSelf);
    }


    /// <summary>
    /// 同步按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSync_Click(object sender, EventArgs e)
    {
        try
        {
            // 请大家不要使用 System.Transactions.TransactionScope, 它导致Service崩溃.
            //using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            //{
                LPWeb.BLL.Company_General bllCG = new LPWeb.BLL.Company_General();
                LPWeb.Model.Company_General modGeneral = new LPWeb.Model.Company_General();

                modGeneral = bllCG.GetModel();

                modGeneral.StartMarketingSync = true;

                bllCG.Update(modGeneral);

                ServiceManager sm = new ServiceManager();

                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    string err = "";
                    if (!client.SyncMarketingData(ref err))
                    {
                        PageCommon.AlertMsg(this, err);
                        return;
                    }

                    //ts.Complete();
                }
            //}

            PageCommon.WriteJsEnd(this, "Sync Marketing Info successfully.", PageCommon.Js_RefreshSelf);
        }
        catch (Exception ex)
        {
            PageCommon.AlertMsg(this, ex.Message);
            return;
        }
    }
}