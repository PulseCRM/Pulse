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
using System.Text.RegularExpressions;

public partial class Prospect_ProspectDetailAssetTab : BasePage
{
    int iProspectID = 0;
    int iLoanID = 0;

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
            string sSql0 = "select LastName +', '+ FirstName + case when MiddleName is null then '' when MiddleName='' then '' else ' '+ MiddleName end as ContactName,* from Contacts as a "
                         + "inner join Prospect as b on a.ContactId=b.ContactId "
                         + "where a.ContactId=" + this.iProspectID;
            BorrowerListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);

            if (BorrowerListData.Rows.Count == 0)
            {
                PageCommon.WriteJsEnd(this, "Unable to find the borrower and co-borrower records in the Prospects table. Please contact your system administrator.", string.Empty);
            }
        }
        else
        {
            string sSql0 = "select c.LastName +', '+ c.FirstName + case when c.MiddleName is null then '' when c.MiddleName='' then '' else ' '+ c.MiddleName end as ContactName, "
                          + "b.ContactId, b.ContactRoleId, 'Borrower' as ContactRoleName "
                          + "from Loans as a inner join LoanContacts as b on a.FileId=b.FileId "
                          + "inner join Contacts as c on b.ContactId=c.ContactId "
                          + "inner join Prospect as d on c.ContactId=d.ContactId "
                          + "where a.FileId=" + this.iLoanID + "  and b.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() "
                          + "union "
                          + "select c.LastName +', '+ c.FirstName + case when c.MiddleName is null then '' when c.MiddleName='' then '' else ' '+ c.MiddleName end as ContactName, "
                          + "b.ContactId, b.ContactRoleId, 'CoBorrower' as ContactRoleName "
                          + "from Loans as a inner join LoanContacts as b on a.FileId=b.FileId "
                          + "inner join Contacts as c on b.ContactId=c.ContactId "
                          + "inner join Prospect as d on c.ContactId=d.ContactId "
                          + "where a.FileId=" + this.iLoanID + "  and b.ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId() "
                          + "order by ContactRoleName";

            BorrowerListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);

            if (BorrowerListData.Rows.Count == 0)
            {
                PageCommon.WriteJsEnd(this, "Unable to find the borrower and co-borrower records in the Prospects table. Please contact your system administrator.", string.Empty);
            }

            // default Borrower ID
            if (this.iProspectID == 0)
            {
                this.iProspectID = Convert.ToInt32(BorrowerListData.Rows[0]["ContactId"]);
            }
            else
            {
                DataRow[] ContactRows = BorrowerListData.Select("ContactId=" + this.iProspectID);
                if (ContactRows.Length == 0)
                {
                    PageCommon.WriteJsEnd(this, "Invalid prospect id.", string.Empty);
                }
            }
        }

        this.ddlBorrowerList.DataSource = BorrowerListData;
        this.ddlBorrowerList.DataBind();

        if (this.IsPostBack == false)
        {
            this.ddlBorrowerList.SelectedValue = this.iProspectID.ToString();
        }

        #endregion

        #region 控制Joint

        if (this.iLoanID == 0)
        {
            this.chkJoint.Disabled = true;
            this.ddlBorrowerList.Enabled = false;
        }
        else
        {
            DataRow[] ContactRows = BorrowerListData.Select("ContactId=" + this.iProspectID);
            if (ContactRows[0]["ContactRoleName"].ToString() == "Borrower")
            {
                this.chkJoint.Disabled = false;

                #region 加载Loans.Joint

                if (this.IsPostBack == false)
                {
                    string sSql4 = "select * from Loans where FileId=" + this.iLoanID;
                    DataTable LoanInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql4);
                    bool bJoint = LoanInfo.Rows[0]["Joint"] == DBNull.Value ? false : Convert.ToBoolean(LoanInfo.Rows[0]["Joint"]);

                    this.chkJoint.Checked = bJoint;
                    this.ddlBorrowerList.Enabled = !bJoint;
                }

                #endregion
            }
            else
            {
                this.chkJoint.Disabled = true;
            }
        }

        #endregion

        #region 加载Assets列表

        string sSql = "select * from ProspectAssets where ContactId=" + this.iProspectID;

        DataTable AssetsList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        this.gridAssetsList.DataSource = AssetsList;
        this.gridAssetsList.DataBind();


        #endregion
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region 获取用户输入

        string sAssetsTypes = this.hdnAssetsTypes.Value;
        string sAmounts = this.hdnAmounts.Value;
        string sNames = this.hdnNames.Value;
        string sAccountNums = this.hdnAccountNums.Value;

        #endregion

        Collection<SqlCommand> SqlCmdList = new Collection<SqlCommand>();

        #region Loans.Joint

        if (this.chkJoint.Disabled == false)
        {
            bool bIsJoint = this.chkJoint.Checked;
            string sSql5 = "update Loans set Joint=@Joint where FileId=" + this.iLoanID;
            SqlCommand SqlCmd5 = new SqlCommand(sSql5);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd5, "@Joint", bIsJoint);
            SqlCmdList.Add(SqlCmd5);
        }

        #endregion

        #region 保存ProspectAssets

        string sSql2 = "delete from ProspectAssets where ContactId=" + this.iProspectID;
        SqlCommand SqlCmd2 = new SqlCommand(sSql2);
        SqlCmdList.Add(SqlCmd2);

        if (sAssetsTypes != string.Empty)
        {
            string[] AssetsTypeArray = sAssetsTypes.Split(',');
            string[] AmountArray = sAmounts.Split(',');
            string[] NameArray = sNames.Split('|');
            string[] AccountNumArray = sAccountNums.Split('|');

            for (int i = 0; i < AssetsTypeArray.Length; i++)
            {
                string sAssetsType = AssetsTypeArray[i];
                string sAmount = AmountArray[i];
                string sName = NameArray[i];
                string sAccountNum = AccountNumArray[i];

                string sSql3 = "insert into ProspectAssets (ContactId, Name, Account, Amount, Type) values (" + this.iProspectID + ", @Name, @Account, @Amount, @Type)";
                SqlCommand SqlCmd3 = new SqlCommand(sSql3);
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd3, "@Name", SqlDbType.NVarChar, sName);
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd3, "@Account", SqlDbType.NVarChar, sAccountNum);
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd3, "@Amount", SqlDbType.Decimal, Convert.ToDecimal(sAmount));
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd3, "@Type", SqlDbType.NVarChar, sAssetsType);

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
        this.ClientScript.RegisterStartupScript(this.GetType(), "_JsSuccess", "alert('Save prospect assets successfully.');SendAjax('" + this.iProspectID + "');", true);
    }
}
