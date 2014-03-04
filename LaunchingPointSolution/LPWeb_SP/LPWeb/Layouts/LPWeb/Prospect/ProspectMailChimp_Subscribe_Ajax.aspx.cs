using System;
using LPWeb.Common;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using System.Collections.Generic;

public partial class Prospect_ProspectMailChimp_Subscribe_Ajax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"错误信息"}

        #region 校验页面参数

        #region ContactIDs

        if (this.Request.QueryString["ContactIDs"] == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }

        string sContactIDs = this.Request.QueryString["ContactIDs"];

        //if (Regex.IsMatch(sContactIDs, @"^([1-9]\d*)(,[1-9]\d*)*$") == false)
        //{
        //    this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid query string.\"}");
        //    this.Response.End();
        //}

        string[] ContactIDArray = sContactIDs.Split(',');

        #endregion

        #region LID

        if (this.Request.QueryString["LID"] == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }

        string sLID = this.Request.QueryString["LID"];

        #endregion

        #endregion

        #region 获取LID对应的BranchID

        int iBranchID = 0;
        string sSql2 = "select * from MailChimpLists where LID=@LID";
        SqlCommand SqlCmd2 = new SqlCommand(sSql2);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd2, "@LID", SqlDbType.NVarChar, sLID);

        DataTable MailChimpListInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd2);
        if (MailChimpListInfo.Rows.Count == 0)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid query string.\"}");
            this.Response.End();
        }

        if (MailChimpListInfo.Rows[0]["BranchId"] != DBNull.Value) 
        {
            iBranchID = Convert.ToInt32(MailChimpListInfo.Rows[0]["BranchId"]);
        }

        #endregion

        int iLoginUserID = this.CurrUser.iUserID;

        #region 调用MailChimp_Subscribe API

        int[] ContactIDList = new int[ContactIDArray.Length];
        int j = 0;
        foreach (string sContactID in ContactIDArray)
        {
            int iContactID = Convert.ToInt32(sContactID);

            ContactIDList.SetValue(iContactID, j);

            j++;
        }

        bool bSuccess = true;
        string sError = string.Empty;
        LPWeb.LP_Service.MailChimp_Response mc_resp = null;

        try
        {                      
            //LPWeb.LP_Service.LP2ServiceClient x = new LPWeb.LP_Service.LP2ServiceClient();
            ServiceManager sm = new ServiceManager();
            using (LPWeb.LP_Service.LP2ServiceClient client = sm.StartServiceClient())
            {
                mc_resp = client.MailChimp_SubscribeBatch(ContactIDList, sLID);

                if (mc_resp.hdr.Successful == false)
                {
                    bSuccess = false;
                    sError = mc_resp.hdr.StatusInfo.Replace("\"", "'");
                }
                else
                {
                    bSuccess = true;
                    sError = "Subscribe Successfully";
                }
            }
        }
        catch (Exception ex)
        {
            bSuccess = false;
            sError = "Exception happened when invoke API MailChimp_SubscribeBatch: " + ex.Message;
        }
        finally 
        {
            if (bSuccess == false)
            {
                this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError + "\"}");
                this.Response.End();
            }
        }

        #endregion

        #region Insert ContactMailCampaign

        #region Build SqlCommand

        int i = 0;
        SqlCommand[] SqlCmdList = new SqlCommand[ContactIDArray.Length];
        foreach (string sContactID in ContactIDArray)
        {
            int iContactID = Convert.ToInt32(sContactID);
            string sSql = "insert into dbo.ContactMailCampaign (ContactId,CID,LID,BranchId,Added,AddedBy,Result) values (@ContactId,@CID,@LID,@BranchId,getdate(),@AddedBy,@Result)";
            SqlCommand SqlCmd = new SqlCommand(sSql);

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@ContactId", SqlDbType.Int, iContactID);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@CID", SqlDbType.NVarChar, DBNull.Value);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LID", SqlDbType.NVarChar, sLID);
            
            if(iBranchID == 0)
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@BranchId", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@BranchId", SqlDbType.Int, iBranchID);
            }

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@AddedBy", SqlDbType.Int, iLoginUserID);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Result", SqlDbType.NVarChar, DBNull.Value);

            SqlCmdList.SetValue(SqlCmd, i);
            i++;
        }

        #endregion

        #region 批量执行SQL语句

        SqlConnection SqlConn = null;
        SqlTransaction SqlTrans = null;

        try
        {
            SqlConn = LPWeb.DAL.DbHelperSQL.GetOpenConnection();
            SqlTrans = SqlConn.BeginTransaction();

            foreach (SqlCommand SqlCmd1 in SqlCmdList)
            {
                LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd1, SqlTrans);
            }

            SqlTrans.Commit();
        }
        catch (Exception ex)
        {
            SqlTrans.Rollback();

            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Exception happened when insert ContactMailCampaign.\"}");
            this.Response.End();
        }
        finally
        {
            if (SqlConn != null)
            {
                SqlConn.Close();
            }
        }

        #endregion

        #endregion

        this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
        this.Response.End();
    }
}