using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;

public partial class Prospect_ProspectDetailIncomeTab : BasePage
{
    int iProspectID = 0;
    int iLoanID = 0;
    DataTable ProspectIncomeInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        if (this.Request.QueryString["LoanID"] != null)
        {
            bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);

            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, "Missing required query string.", string.Empty);
            }

            string sLoanID = this.Request.QueryString["LoanID"].ToString();
            this.iLoanID = Convert.ToInt32(sLoanID);

            if (this.Request.QueryString["ProspectID"] != null)
            {
                bIsValid = PageCommon.ValidateQueryString(this, "ProspectID", QueryStringType.ID);

                if (bIsValid == false)
                {
                    PageCommon.WriteJsEnd(this, "Missing required query string.", string.Empty);
                }

                string sProspectID = this.Request.QueryString["ProspectID"].ToString();
                this.iProspectID = Convert.ToInt32(sProspectID);
            }
        }
        else // only ProspectID
        {
            bool bIsValid = PageCommon.ValidateQueryString(this, "ProspectID", QueryStringType.ID);

            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, "Missing required query string.", string.Empty);
            }

            string sProspectID = this.Request.QueryString["ProspectID"].ToString();
            this.iProspectID = Convert.ToInt32(sProspectID);
        }

        #endregion

        #region 加载Borrower列表

        DataTable BorrowerListData = null;
        if (this.iLoanID == 0)
        {
            string sSql0 = "select LastName +', '+ FirstName + case when MiddleName is null then '' when MiddleName='' then '' else ' '+ MiddleName end as ContactName,* from Contacts where ContactId=" + this.iProspectID;
            BorrowerListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);

            this.ddlBorrowerList.Enabled = false;
        }
        else
        {
            string sSql0 = "select c.LastName +', '+ c.FirstName + case when c.MiddleName is null then '' when c.MiddleName='' then '' else ' '+ c.MiddleName end as ContactName, "
                        + "b.ContactId "
                        + "from Loans as a inner join LoanContacts as b on a.FileId=b.FileId "
                        + "inner join Contacts as c on b.ContactId=c.ContactId "
                        + "where a.FileId=" + this.iLoanID + " and (b.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() or b.ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId()) "
                        + "order by b.ContactRoleId ";

            BorrowerListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);

            // default Borrower ID
            if (this.iProspectID == 0)
            {
                this.iProspectID = Convert.ToInt32(BorrowerListData.Rows[0]["ContactId"]);
            }
        }

        this.ddlBorrowerList.DataSource = BorrowerListData;
        this.ddlBorrowerList.DataBind();

        if (this.IsPostBack == false)
        {
            this.ddlBorrowerList.SelectedValue = this.iProspectID.ToString();
        }

        #endregion

        #region 加载Prospect Income信息

        string sSql2 = "select *, isnull(Salary,0.00)+isnull(Overtime,0.00)+isnull(Bonuses,0.00)+isnull(Commission,0.00)+isnull(Div_Int,0.00)+isnull(NetRent,0.00)+isnull(Other,0.00) as SubTotal from ProspectIncome where ContactId=" + this.iProspectID;
        this.ProspectIncomeInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql2);
        if (this.ProspectIncomeInfo.Rows.Count > 0)
        {
            if (this.IsPostBack == false)
            {
                if (this.ProspectIncomeInfo.Rows[0]["Salary"] != DBNull.Value)
                {
                    this.txtBase.Text = this.ProspectIncomeInfo.Rows[0]["Salary"].ToString();
                }

                if (this.ProspectIncomeInfo.Rows[0]["Overtime"] != DBNull.Value)
                {
                    this.txtOvertime.Text = this.ProspectIncomeInfo.Rows[0]["Overtime"].ToString();
                }

                if (this.ProspectIncomeInfo.Rows[0]["Bonuses"] != DBNull.Value)
                {
                    this.txtBonuses.Text = this.ProspectIncomeInfo.Rows[0]["Bonuses"].ToString();
                }

                if (this.ProspectIncomeInfo.Rows[0]["Commission"] != DBNull.Value)
                {
                    this.txtCommission.Text = this.ProspectIncomeInfo.Rows[0]["Commission"].ToString();
                }

                if (this.ProspectIncomeInfo.Rows[0]["Div_Int"] != DBNull.Value)
                {
                    this.txtDivInt.Text = this.ProspectIncomeInfo.Rows[0]["Div_Int"].ToString();
                }

                if (this.ProspectIncomeInfo.Rows[0]["NetRent"] != DBNull.Value)
                {
                    this.txtNetRent.Text = this.ProspectIncomeInfo.Rows[0]["NetRent"].ToString();
                }

                if (this.ProspectIncomeInfo.Rows[0]["Other"] != DBNull.Value)
                {
                    this.txtOther.Text = this.ProspectIncomeInfo.Rows[0]["Other"].ToString();
                }

                if (this.ProspectIncomeInfo.Rows[0]["SubTotal"] != DBNull.Value)
                {
                    this.lbSubtotal.Text = "$" + Convert.ToDecimal(this.ProspectIncomeInfo.Rows[0]["SubTotal"]).ToString("n2");
                }
            }
        }

        #endregion

        #region 加载Other Income列表

        string sSql = "select * from ProspectOtherIncome where ContactId=" + this.iProspectID;

        DataTable OtherIncomeList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        object oSum = OtherIncomeList.Compute("Sum(MonthlyIncome)", string.Empty);
        decimal dSum = decimal.Zero;
        if (oSum != DBNull.Value)
        {
            dSum = Convert.ToDecimal(oSum);
        }
        this.lbOtherIncomeTotal.Text = "$" + dSum.ToString("n2");

        this.gridOtherIncomeList.DataSource = OtherIncomeList;
        this.gridOtherIncomeList.DataBind();


        #endregion
    }

    protected void btnSaveIncome_Click(object sender, EventArgs e)
    {
        #region 获取用户输入

        string sBase = this.txtBase.Text.Trim();
        string sOvertime = this.txtOvertime.Text.Trim();
        string sBonuses = this.txtBonuses.Text.Trim();
        string sCommission = this.txtCommission.Text.Trim();
        string sDivInt = this.txtDivInt.Text.Trim();
        string sNetRent = this.txtNetRent.Text.Trim();
        string sOther = this.txtOther.Text.Trim();

        string sOtherIncomeTypes = this.hdnOtherIncomeTypes.Value;
        string sMonthlyIncomes = this.hdnMonthlyIncomes.Value;

        #endregion

        Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

        #region 保存ProspectIncome

        string sSql = string.Empty;
        if (this.ProspectIncomeInfo.Rows.Count == 0)
        {
            sSql = "insert into ProspectIncome (ContactId,Salary,Overtime,Bonuses,Commission,Div_Int,NetRent,Other) values (@ContactId,@Salary,@Overtime,@Bonuses,@Commission,@Div_Int,@NetRent,@Other)";

        }
        else
        {
            sSql = "update ProspectIncome set Salary=@Salary,Overtime=@Overtime,Bonuses=@Bonuses,Commission=@Commission,Div_Int=@Div_Int,NetRent=@NetRent,Other=@Other where ContactId=@ContactId";

        }

        SqlCommand SqlCmd = new SqlCommand(sSql);

        #region Add Parameters

        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@ContactId", SqlDbType.Int, this.iProspectID);

        if (sBase == string.Empty)
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Salary", SqlDbType.Decimal, DBNull.Value);
        }
        else
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Salary", SqlDbType.Decimal, Convert.ToDecimal(sBase));
        }

        if (sOvertime == string.Empty)
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Overtime", SqlDbType.Decimal, DBNull.Value);
        }
        else
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Overtime", SqlDbType.Decimal, Convert.ToDecimal(sOvertime));
        }

        if (sBonuses == string.Empty)
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Bonuses", SqlDbType.Decimal, DBNull.Value);
        }
        else
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Bonuses", SqlDbType.Decimal, Convert.ToDecimal(sBonuses));
        }

        if (sCommission == string.Empty)
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Commission", SqlDbType.Decimal, DBNull.Value);
        }
        else
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Commission", SqlDbType.Decimal, Convert.ToDecimal(sCommission));
        }

        if (sDivInt == string.Empty)
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Div_Int", SqlDbType.Decimal, DBNull.Value);
        }
        else
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Div_Int", SqlDbType.Decimal, Convert.ToDecimal(sDivInt));
        }

        if (sNetRent == string.Empty)
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@NetRent", SqlDbType.Decimal, DBNull.Value);
        }
        else
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@NetRent", SqlDbType.Decimal, Convert.ToDecimal(sNetRent));
        }

        if (sOther == string.Empty)
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Other", SqlDbType.Decimal, DBNull.Value);
        }
        else
        {
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Other", SqlDbType.Decimal, Convert.ToDecimal(sOther));
        }

        #endregion

        SqlCmdList.Add(SqlCmd);

        #endregion

        #region 保存ProspectOtherIncome

        string sSql2 = "delete from ProspectOtherIncome where ContactId=" + this.iProspectID;
        SqlCommand SqlCmd2 = new SqlCommand(sSql2);
        SqlCmdList.Add(SqlCmd2);

        if (sOtherIncomeTypes != string.Empty)
        {
            string[] OtherIncomeTypeArray = sOtherIncomeTypes.Split(',');
            string[] MonthlyIncomeArray = sMonthlyIncomes.Split(',');

            for (int i = 0; i < OtherIncomeTypeArray.Length; i++)
            {
                string sOtherIncomeType = OtherIncomeTypeArray[i];
                string sMonthlyIncome = MonthlyIncomeArray[i];

                string sSql3 = "insert into ProspectOtherIncome (ContactId, Type, MonthlyIncome) values (" + this.iProspectID + ", @Type, @MonthlyIncome)";
                SqlCommand SqlCmd3 = new SqlCommand(sSql3);
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd3, "@Type", SqlDbType.NVarChar, sOtherIncomeType);
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd3, "@MonthlyIncome", SqlDbType.Decimal, Convert.ToDecimal(sMonthlyIncome));
                SqlCmdList.Add(SqlCmd3);
            }
        }

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
        this.ClientScript.RegisterStartupScript(this.GetType(), "_JsSuccess", "alert('Save prospect income successfully.');SendAjax('" + this.iProspectID + "');", true);
    }
}
