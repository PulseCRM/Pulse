using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.BLL;
using System.Data;
using LPWeb.LP_Service;
using LPWeb.Layouts.LPWeb;
using System.Data.SqlClient;

public partial class LoanProgramCreateAjax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 接收参数

        // LoanProgram
        if (this.Request.QueryString["LoanProgram"] == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            return;
        }
        string LoanProgram = this.Request.QueryString["LoanProgram"].ToString().Trim();

        // IsARM
        if (this.Request.QueryString["IsARM"] == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            return;
        }
        string IsARM = this.Request.QueryString["IsARM"].ToString().Trim();
        if (IsARM != "true")
        {
            IsARM = "false";
        }
        
        #endregion

        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}

        #region 检查是否重复

        string sSql = "select * from  Company_Loan_Programs where LoanProgram=@LoanProgram";
        SqlCommand SqlCmd = new SqlCommand(sSql);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LoanProgram", SqlDbType.NVarChar, LoanProgram);
        DataTable LoanProgramInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd);
        if (LoanProgramInfo.Rows.Count > 0)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Loan Program has exist.\"}");
            return;
        }

        #endregion

        #region Insert Company_Loan_Programs

        string sSql2 = "insert Company_Loan_Programs (LoanProgram, IsARM) values (@LoanProgram, @IsARM)";
        SqlCommand SqlCmd2 = new SqlCommand(sSql2);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd2, "@LoanProgram", SqlDbType.NVarChar, LoanProgram);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd2, "@IsARM", SqlDbType.NVarChar, Convert.ToBoolean(IsARM));

        try
        {
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd2);
        }
        catch (Exception ex)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Failed to create loan program.\"}");
            return;
        }

        #endregion

        this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
    }
}

