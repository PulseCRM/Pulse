using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using System.Data.SqlClient;
using LPWeb.Common;

public partial class LoanProgramSetupAdd : BasePage
{
    int iInvestorID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.QueryString["InvestorID"] != null)
        {
            this.iInvestorID = Convert.ToInt32(this.Request.QueryString["InvestorID"]);
        }

        if (this.IsPostBack == false)
        {
            #region 加载Investor

            string sSql4 = "select * from dbo.ContactCompanies as a inner join ServiceTypes as b on a.ServiceTypeId=b.ServiceTypeId where 1=1 and b.Name='Investor' and a.Enabled=1 order by a.Name";
            DataTable InvestorList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql4);

            DataRow InvestorRow = InvestorList.NewRow();
            InvestorRow["ContactCompanyId"] = 0;
            InvestorRow["Name"] = "-- select --";

            InvestorList.Rows.InsertAt(InvestorRow, 0);

            this.ddlInvestor.DataSource = InvestorList;
            this.ddlInvestor.DataBind();

            #endregion

            if (this.iInvestorID != 0)
            {
                this.ddlInvestor.SelectedValue = this.iInvestorID.ToString();
            }

            #region 加载Loan Program

            string sSql2 = "select * from  Company_Loan_Programs order by LoanProgram";
            DataTable LoanProgramList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql2);

            DataRow NewLoanProgramRow = LoanProgramList.NewRow();
            NewLoanProgramRow["LoanProgramID"] = 0;
            NewLoanProgramRow["LoanProgram"] = "-- select --";
            LoanProgramList.Rows.InsertAt(NewLoanProgramRow, 0);

            this.ddlLoanProgram.DataSource = LoanProgramList;
            this.ddlLoanProgram.DataBind();

            #endregion
        }
    }

    protected void btnSaveAndClose_Click(object sender, EventArgs e) 
    {
        string sError;
        bool bSuccess = SaveLoanProgram(out sError);
        if (bSuccess == false)
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "_dup", "alert('" + sError + "');", true);
            return;
        }

        // success
        PageCommon.WriteJsEnd(this, "Save successfully.", PageCommon.Js_RefreshParent);
    }

    protected void btnSaveAndCreate_Click(object sender, EventArgs e)
    {
        string sError;
        bool bSuccess = SaveLoanProgram(out sError);
        if (bSuccess == false)
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "_dup", "alert('" + sError + "');", true);
            return;
        }

        // success
        string sInvestorID = this.ddlInvestor.SelectedValue;
        PageCommon.WriteJsEnd(this, "Save successfully.", "window.location.href='LoanProgramSetupAdd.aspx?InvestorID=" + sInvestorID + "'");
    }

    private bool SaveLoanProgram(out string sError)
    {
        sError = string.Empty;

        #region 校验用户输入

        string sInvestorID = this.ddlInvestor.SelectedValue;
        int iInvestorID = Convert.ToInt32(sInvestorID);

        bool bEnabled = this.chkEnabled.Checked;
        bool bARM = this.chkARM.Checked;

        string sLoanProgramID = this.ddlLoanProgram.SelectedValue.ToString();
        int iLoanProgramID = Convert.ToInt32(sLoanProgramID);
        
        string sIndex = this.txtIndex.Text.Trim();

        string sMargin = this.txtMargin.Text.Trim();
        string sFirstAdj = this.txtFirstAdj.Text.Trim();
        string sSubAdj = this.txtSubAdj.Text.Trim();
        string sLifetimeCap = this.txtLifetimeCap.Text.Trim();
        string sTerm = this.txtTerm.Text.Trim();
        string sDue = this.txtDue.Text.Trim();

        #endregion

        #region 检查InvestorID和LoanProgramID是否重复

        string sSql4 = "select * from dbo.Company_LoanProgramDetails where InvestorID=" + sInvestorID + " and LoanProgramID=" + sLoanProgramID;
        DataTable LoanProgramDetailsInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql4);
        if (LoanProgramDetailsInfo.Rows.Count > 0)
        {
            sError = "Duplicated Investor and Loan Program.";
            return false;
        }

        #endregion

        #region Insert Company_LoanProgramDetails

        string sSql = @"INSERT INTO dbo.Company_LoanProgramDetails
                       (LoanProgramID
                       ,InvestorID
                       ,IndexType
                       ,Margin
                       ,FirstAdj
                       ,SubAdj
                       ,LifetimeCap
                       ,Enabled
                       ,Term
                       ,Due)
                 VALUES
                       (@LoanProgramID
                       ,@InvestorID
                       ,@IndexType
                       ,@Margin
                       ,@FirstAdj
                       ,@SubAdj
                       ,@LifetimeCap
                       ,@Enabled
                       ,@Term
                       ,@Due)";
        SqlCommand SqlCmd = new SqlCommand(sSql);

        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LoanProgramID", SqlDbType.Int, iLoanProgramID);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@InvestorID", SqlDbType.Int, iInvestorID);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@IndexType", SqlDbType.NVarChar, sIndex);
        if (sMargin == string.Empty)
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Margin", SqlDbType.Money, DBNull.Value);
        }
        else
        {
            decimal dMargin = Convert.ToDecimal(sMargin);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Margin", SqlDbType.Money, dMargin);
        }
        if (sFirstAdj == string.Empty)
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@FirstAdj", SqlDbType.Money, DBNull.Value);
        }
        else
        {
            decimal dFirstAdj = Convert.ToDecimal(sFirstAdj);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@FirstAdj", SqlDbType.Money, dFirstAdj);
        }
        if (sSubAdj == string.Empty)
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@SubAdj", SqlDbType.Money, DBNull.Value);
        }
        else
        {
            decimal dSubAdj = Convert.ToDecimal(sSubAdj);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@SubAdj", SqlDbType.Money, dSubAdj);
        }
        if (sLifetimeCap == string.Empty)
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LifetimeCap", SqlDbType.Money, DBNull.Value);
        }
        else
        {
            decimal dLifetimeCap = Convert.ToDecimal(sLifetimeCap);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@LifetimeCap", SqlDbType.Money, dLifetimeCap);
        }
        if (sTerm == string.Empty)
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Term", SqlDbType.Int, DBNull.Value);
        }
        else
        {
            int iTerm = 0;
            if (int.TryParse(sTerm, out iTerm))
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Term", SqlDbType.Int, iTerm);
            }
            else
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Term", SqlDbType.Int, DBNull.Value);
        }
        if (sDue == string.Empty)
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Due", SqlDbType.Int, DBNull.Value);
        }
        else
        {
            int iDue = 0;
            if (int.TryParse(sDue, out iDue))
            {
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Due", SqlDbType.Int, iDue);
            }
            else
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Due", SqlDbType.Int, DBNull.Value);
        }

        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);

        LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd);

        #endregion

        #region Update Company_Loan_Programs.IsARM

        string sSql5 = "update Company_Loan_Programs set IsARM=@IsARM where LoanProgramID=" + sLoanProgramID;
        SqlCommand SqlCmd5 = new SqlCommand(sSql5);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd5, "@IsARM", SqlDbType.Bit, bARM);
        LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd5);

        #endregion

        return true;
    }
}
