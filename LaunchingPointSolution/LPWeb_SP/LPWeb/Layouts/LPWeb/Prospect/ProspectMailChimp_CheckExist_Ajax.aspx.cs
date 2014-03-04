using System;
using LPWeb.Common;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using System.Text;

public partial class Prospect_ProspectMailChimp_CheckExist_Ajax : BasePage
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

        string sSql2 = "select * from MailChimpLists where LID=@LID";
        SqlCommand SqlCmd2 = new SqlCommand(sSql2);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd2, "@LID", SqlDbType.NVarChar, sLID);

        DataTable MailChimpListInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd2);
        if (MailChimpListInfo.Rows.Count == 0)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Invalid query string.\"}");
            this.Response.End();
        }

        string sListName = MailChimpListInfo.Rows[0]["Name"].ToString();

        #endregion

        #region 检查ContactMailCampaign记录是否存在

        StringBuilder sbErrorMsg = new StringBuilder();
        foreach (string sContactID in ContactIDArray)
        {
            int iContactID = Convert.ToInt32(sContactID);
            string sSql = "select count(1) from ContactMailCampaign where ContactId=@ContactId and LID=@LID";
            SqlCommand SqlCmd = new SqlCommand(sSql);

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@ContactId", SqlDbType.Int, iContactID);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LID", SqlDbType.NVarChar, sLID);

            int iCount = Convert.ToInt32(LPWeb.DAL.DbHelperSQL.ExecuteScalar(SqlCmd));

            if(iCount > 0)
            {
                #region Build Error Msg

                string sContactName = this.GetContactName(iContactID);
                string sErrorMsg = "The client [" + sContactName + "] has already been added to the list [" + sListName + "].";

                #endregion

                if(sbErrorMsg.Length == 0)
                {
                    sbErrorMsg.Append(sErrorMsg);
                }
                else
                {
                    sbErrorMsg.Append("\\r\\n" + sErrorMsg);
                }
            }
        }

        #endregion

        if (sbErrorMsg.Length == 0)     // not exist
        {
            this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
            this.Response.End();
        }
        else
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sbErrorMsg.ToString() + "\"}");
            this.Response.End();
        }
    }

    private string GetContactName(int iContactID) 
    {
        string sSql = "select dbo.lpfn_GetContactName(" + iContactID + ") as ContactName";

        string sContactName = LPWeb.DAL.DbHelperSQL.ExecuteScalar(sSql).ToString();

        return sContactName;
    }
}
