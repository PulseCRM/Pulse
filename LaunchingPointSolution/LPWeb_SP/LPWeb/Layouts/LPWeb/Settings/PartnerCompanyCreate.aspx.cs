using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using System.Data.SqlClient;
using LPWeb.Common;

public partial class Settings_PartnerCompanyCreate : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsPostBack == false)
        {
            #region 加载ServiceType

            string sSql = "select * from ServiceTypes where Enabled=1 order by Name";
            DataTable ServiceTypeList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
            DataRow[] TargetRows = ServiceTypeList.Select("Name='Investor'");
            string ServiceTypeId = TargetRows[0]["ServiceTypeId"].ToString();

            this.ddlServiceType.DataSource = ServiceTypeList;
            this.ddlServiceType.DataBind();

            this.ddlServiceType.SelectedValue = ServiceTypeId;

            #endregion
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
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



    private bool SaveLoanProgram(out string sError)
    {
        sError = string.Empty;

        #region 校验用户输入

        string CompanyName = this.txtCompanyName.Text.Trim();
        string ServiceTypeID = this.ddlServiceType.SelectedValue.ToString();
        string ServiceType = this.ddlServiceType.SelectedItem.Text;
        
        bool bEnabled = this.chkEnabled.Checked;
        string Address = this.txtAddress.Text.Trim();
        string City = this.txtCity.Text.Trim();
        string State = this.ddlState.SelectedValue.ToString();
        string Zip = this.txtZip.Text.Trim();
        string Website = this.txtWebsite.Text.Trim();
        
        #endregion

        #region 检查ContactCompanies是否重复

        string sSql4 = @"select * from dbo.ContactCompanies as a 
                        inner join ServiceTypes as b on a.ServiceTypeId=b.ServiceTypeId 
                        where 1=1 and b.ServiceTypeId=@ServiceTypeId 
                        and a.Name=@Name";
        SqlCommand SqlCmd4 = new SqlCommand(sSql4);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd4, "@ServiceTypeId", SqlDbType.Int, Convert.ToInt32(ServiceTypeID));
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd4, "@Name", SqlDbType.NVarChar, CompanyName);
        DataTable ContactCompanyInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd4);
        if (ContactCompanyInfo.Rows.Count > 0)
        {
            sError = "The partner company already exists.";
            return false;
        }

        #endregion

        #region Insert Company_LoanProgramDetails

        string sSql = @"INSERT INTO dbo.ContactCompanies
                       (Name
                       ,Address
                       ,City
                       ,State
                       ,Zip
                       ,ServiceTypes
                       ,ServiceTypeId
                       ,Website
                       ,Enabled)
                 VALUES
                       (@Name
                       ,@Address
                       ,@City
                       ,@State
                       ,@Zip
                       ,@ServiceTypes
                       ,@ServiceTypeId
                       ,@Website
                       ,@Enabled)";
        SqlCommand SqlCmd = new SqlCommand(sSql);

        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, CompanyName);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Address", SqlDbType.NVarChar, Address);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@City", SqlDbType.NVarChar, City);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@State", SqlDbType.NVarChar, State);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Zip", SqlDbType.NVarChar, Zip);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@ServiceTypes", SqlDbType.NVarChar, ServiceType);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@ServiceTypeId", SqlDbType.Int, ServiceTypeID);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Website", SqlDbType.NVarChar, Website);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);

        LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd);

        #endregion

        

        return true;
    }
}
