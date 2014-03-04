using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using System.Data.SqlClient;
using LPWeb.Common;
using LPWeb.BLL;

public partial class Settings_LoanProgramSetupEdit : BasePage
{
    int iInvestorID = 0;
    int iLoanProgramID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        // InvestorID
        bool bValid = PageCommon.ValidateQueryString(this, "InvestorID", QueryStringType.ID);
        if (bValid == false)
        {
            PageCommon.WriteJsEnd(this, "Invalid investor id.", PageCommon.Js_RefreshParent);
        }
        this.iInvestorID = Convert.ToInt32(this.Request.QueryString["InvestorID"]);

        // LoanProgramID
        bValid = PageCommon.ValidateQueryString(this, "LoanProgramID", QueryStringType.ID);
        if (bValid == false)
        {
            PageCommon.WriteJsEnd(this, "Invalid loan program id.", PageCommon.Js_RefreshParent);
        }
        this.iLoanProgramID = Convert.ToInt32(this.Request.QueryString["LoanProgramID"]);

        #endregion

        #region 加载Company_Loan_Programs信息

        string sSql = "select * from dbo.Company_Loan_Programs where LoanProgramID=" + this.iLoanProgramID;
        DataTable LoanProgramInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        if (LoanProgramInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid loan program id.", PageCommon.Js_RefreshParent);
        }

        #endregion

        #region 加载Company_LoanProgramDetails信息

        string sSql3 = "select * from dbo.Company_LoanProgramDetails where InvestorID=" + this.iInvestorID + " and LoanProgramID=" + this.iLoanProgramID;
        DataTable LoanProgramDetailsInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql3);
        if (LoanProgramDetailsInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid id.", PageCommon.Js_RefreshParent);
        }
        
        #endregion

        if (this.IsPostBack == false)
        {
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

            #region 加载Investor

            string sSql4 = "select * from dbo.ContactCompanies as a inner join ServiceTypes as b on a.ServiceTypeId=b.ServiceTypeId where 1=1 and b.Name='Investor' and (a.Enabled=1";
            if (iInvestorID != null && iInvestorID != 0 && Int32.TryParse(iInvestorID .ToString(),out iInvestorID)==true)
            {
                sSql4 += " OR ContactCompanyId=" + iInvestorID.ToString();
            }
            sSql4 += ") order by a.Name";
            DataTable InvestorList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql4);

            DataRow InvestorRow = InvestorList.NewRow();
            InvestorRow["ContactCompanyId"] = 0;
            InvestorRow["Name"] = "-- select --";

            InvestorList.Rows.InsertAt(InvestorRow, 0);

            this.ddlInvestor.DataSource = InvestorList;
            this.ddlInvestor.DataBind();

            #endregion

            #region 绑定数据

            this.ddlInvestor.SelectedValue = this.iInvestorID.ToString();

            #region ARM

            if (LoanProgramInfo.Rows[0]["IsARM"] == DBNull.Value)
            {
                this.chkARM.Checked = false;
            }
            else
            {
                this.chkARM.Checked = Convert.ToBoolean(LoanProgramInfo.Rows[0]["IsARM"].ToString());
            }

            #endregion

            #region Loan Program

            int iLoanProgramID = Convert.ToInt32(LoanProgramDetailsInfo.Rows[0]["LoanProgramID"]);
            this.ddlLoanProgram.SelectedValue = iLoanProgramID.ToString();

            #endregion

            this.txtIndex.Text = LoanProgramDetailsInfo.Rows[0]["IndexType"].ToString();

            this.txtMargin.Text = LoanProgramDetailsInfo.Rows[0]["Margin"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanProgramDetailsInfo.Rows[0]["Margin"]).ToString("00.000");
            this.txtFirstAdj.Text = LoanProgramDetailsInfo.Rows[0]["FirstAdj"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanProgramDetailsInfo.Rows[0]["FirstAdj"]).ToString("00.000");
            this.txtSubAdj.Text = LoanProgramDetailsInfo.Rows[0]["SubAdj"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanProgramDetailsInfo.Rows[0]["SubAdj"]).ToString("00.000");
            this.txtLifetimeCap.Text = LoanProgramDetailsInfo.Rows[0]["LifetimeCap"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanProgramDetailsInfo.Rows[0]["LifetimeCap"]).ToString("00.000");

            if (LoanProgramDetailsInfo.Rows[0]["Enabled"] == DBNull.Value)
            {
                this.chkEnabled.Checked = false;
            }
            else
            {
                this.chkEnabled.Checked = Convert.ToBoolean(LoanProgramDetailsInfo.Rows[0]["Enabled"].ToString());
            }
            this.txtTerm.Text = LoanProgramDetailsInfo.Rows[0]["Term"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanProgramDetailsInfo.Rows[0]["Term"]).ToString("000");
            this.txtDue.Text = LoanProgramDetailsInfo.Rows[0]["Due"] == DBNull.Value ? string.Empty : Convert.ToDecimal(LoanProgramDetailsInfo.Rows[0]["Due"]).ToString("000");
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

        if (this.iInvestorID != iInvestorID || this.iLoanProgramID != iLoanProgramID)
        {
            string sSql4 = "select * from dbo.Company_LoanProgramDetails where InvestorID=" + sInvestorID + " and LoanProgramID=" + sLoanProgramID;
            DataTable LoanProgramDetailsInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql4);
            if (LoanProgramDetailsInfo.Rows.Count > 0)
            {
                sError = "Duplicated Investor and Loan Program.";
                return false;
            }
        }

        #endregion

        #region Update Company_LoanProgramDetails

        string sSql = @"update dbo.Company_LoanProgramDetails
                       set LoanProgramID=@LoanProgramID
                       ,InvestorID=@InvestorID
                       ,IndexType=@IndexType
                       ,Margin=@Margin
                       ,FirstAdj=@FirstAdj
                       ,SubAdj=@SubAdj
                       ,LifetimeCap=@LifetimeCap
                       ,Enabled=@Enabled
                       ,Term=@Term
                       ,Due=@Due
                        where InvestorID=" + this.iInvestorID + " and LoanProgramID=" + this.iLoanProgramID;
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

        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);
        
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
