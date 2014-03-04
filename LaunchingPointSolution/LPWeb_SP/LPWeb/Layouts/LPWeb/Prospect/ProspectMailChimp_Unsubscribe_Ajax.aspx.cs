using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;

public partial class Prospect_ProspectMailChimp_Unsubscribe_Ajax : BasePage
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

        #region LIDs

        if (this.Request.QueryString["LIDs"] == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }

        string sLIDs = this.Request.QueryString["LIDs"];

        if (Regex.IsMatch(sLIDs, @"^(\w+)(,\w+)*$") == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid query string.\"}");
            this.Response.End();
        }

        string[] LIDsArray = sLIDs.Split(',');

        #endregion

        #endregion

        #region 调用MailChimp_Unsubscribe API

        LPWeb.LP_Service.MailChimp_Response mc_resp = null;

        foreach (string sContactID in ContactIDArray)
        {
            int iContactID = Convert.ToInt32(sContactID);

            foreach (string sLID in LIDsArray)
            {
                bool bSuccess = false;
                string sError = string.Empty;
                try
                {
                    ServiceManager sm = new ServiceManager();
                    using (LPWeb.LP_Service.LP2ServiceClient client = sm.StartServiceClient())
                    {
                        mc_resp = client.MailChimp_Unsubscribe(iContactID, sLID);
                        
                        if (mc_resp.hdr.Successful == false)
                        {
                            bSuccess = false;
                            sError = mc_resp.hdr.StatusInfo.Replace("\"","'");
                        }
                        else 
                        {
                            bSuccess = true;
                            sError = "Unsubscribe Successfully";
                        }
                    }
                }
                catch (Exception ex)
                {
                    bSuccess = false;
                    sError = "Exception happened when invoke API MailChimp_Unsubscribe.";
                }
                finally
                {
                    if (bSuccess == false)
                    {
                        this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError + "\"}");
                        this.Response.End();
                    }
                }
            }
        }

        #endregion

        #region delete ContactMailCampaign

        #region Build SqlCommand

        int i = 0;
        SqlCommand[] SqlCmdList = new SqlCommand[ContactIDArray.Length];
        foreach (string sContactID in ContactIDArray)
        {
            int iContactID = Convert.ToInt32(sContactID);
            string sSql = "delete from dbo.ContactMailCampaign where ContactId=@ContactId and LID='" + sLIDs + "'";
            SqlCommand SqlCmd = new SqlCommand(sSql);

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@ContactId", SqlDbType.Int, iContactID);
            
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

            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Exception happened when delete ContactMailCampaign.\"}");
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
