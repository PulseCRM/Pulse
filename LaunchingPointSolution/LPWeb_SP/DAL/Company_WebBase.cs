using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Company_Web。
	/// </summary>
	public class Company_WebBase
    {
		#region  成员方法

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.Company_Web model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Company_Web(");
			strSql.Append("EmailAlertsEnabled,EmailRelayServer,DefaultAlertEmail,EmailInterval,LPCompanyURL,BorrowerURL,BorrowerGreeting,HomePageLogo,LogoForSubPages,HomePageLogoData,SubPageLogoData)");
			strSql.Append(" values (");
			strSql.Append("@EmailAlertsEnabled,@EmailRelayServer,@DefaultAlertEmail,@EmailInterval,@LPCompanyURL,@BorrowerURL,@BorrowerGreeting,@HomePageLogo,@LogoForSubPages,@HomePageLogoData,@SubPageLogoData)");
			SqlParameter[] parameters = {
					new SqlParameter("@EmailAlertsEnabled", SqlDbType.Bit,1),
					new SqlParameter("@EmailRelayServer", SqlDbType.NVarChar,255),
					new SqlParameter("@DefaultAlertEmail", SqlDbType.NVarChar,255),
					new SqlParameter("@EmailInterval", SqlDbType.SmallInt,2),
					new SqlParameter("@LPCompanyURL", SqlDbType.NVarChar,255),
					new SqlParameter("@BorrowerURL", SqlDbType.NVarChar,255),
					new SqlParameter("@BorrowerGreeting", SqlDbType.NVarChar,255),
					new SqlParameter("@HomePageLogo", SqlDbType.NVarChar),
					new SqlParameter("@LogoForSubPages", SqlDbType.NVarChar),
					new SqlParameter("@HomePageLogoData", SqlDbType.Image),
					new SqlParameter("@SubPageLogoData", SqlDbType.Image)};
			parameters[0].Value = model.EmailAlertsEnabled;
			parameters[1].Value = model.EmailRelayServer;
			parameters[2].Value = model.DefaultAlertEmail;
			parameters[3].Value = model.EmailInterval;
			parameters[4].Value = model.LPCompanyURL;
			parameters[5].Value = model.BorrowerURL;
			parameters[6].Value = model.BorrowerGreeting;
			parameters[7].Value = model.HomePageLogo;
			parameters[8].Value = model.LogoForSubPages;
			parameters[9].Value = model.HomePageLogoData;
			parameters[10].Value = model.SubPageLogoData;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.Company_Web model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Company_Web set ");
			strSql.Append("EmailAlertsEnabled=@EmailAlertsEnabled,");
			strSql.Append("EmailRelayServer=@EmailRelayServer,");
			strSql.Append("DefaultAlertEmail=@DefaultAlertEmail,");
			strSql.Append("EmailInterval=@EmailInterval,");
			strSql.Append("LPCompanyURL=@LPCompanyURL,");
			strSql.Append("BorrowerURL=@BorrowerURL,");
			strSql.Append("BorrowerGreeting=@BorrowerGreeting,");
			strSql.Append("HomePageLogo=@HomePageLogo,");
			strSql.Append("LogoForSubPages=@LogoForSubPages,");
			strSql.Append("HomePageLogoData=@HomePageLogoData,");
			strSql.Append("SubPageLogoData=@SubPageLogoData");
            //strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@EmailAlertsEnabled", SqlDbType.Bit,1),
					new SqlParameter("@EmailRelayServer", SqlDbType.NVarChar,255),
					new SqlParameter("@DefaultAlertEmail", SqlDbType.NVarChar,255),
					new SqlParameter("@EmailInterval", SqlDbType.SmallInt,2),
					new SqlParameter("@LPCompanyURL", SqlDbType.NVarChar,255),
					new SqlParameter("@BorrowerURL", SqlDbType.NVarChar,255),
					new SqlParameter("@BorrowerGreeting", SqlDbType.NVarChar,255),
					new SqlParameter("@HomePageLogo", SqlDbType.NVarChar),
					new SqlParameter("@LogoForSubPages", SqlDbType.NVarChar),
					new SqlParameter("@HomePageLogoData", SqlDbType.Image),
					new SqlParameter("@SubPageLogoData", SqlDbType.Image)};
			parameters[0].Value = model.EmailAlertsEnabled;
			parameters[1].Value = model.EmailRelayServer;
			parameters[2].Value = model.DefaultAlertEmail;
			parameters[3].Value = model.EmailInterval;
			parameters[4].Value = model.LPCompanyURL;
			parameters[5].Value = model.BorrowerURL;
			parameters[6].Value = model.BorrowerGreeting;
			parameters[7].Value = model.HomePageLogo;
			parameters[8].Value = model.LogoForSubPages;
			parameters[9].Value = model.HomePageLogoData;
			parameters[10].Value = model.SubPageLogoData;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.Company_Web GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 EmailAlertsEnabled,EmailRelayServer,DefaultAlertEmail,EmailInterval,LPCompanyURL,BorrowerURL,BorrowerGreeting,HomePageLogo,LogoForSubPages,HomePageLogoData,SubPageLogoData from Company_Web ");
            //strSql.Append(" where ");
			SqlParameter[] parameters = {
};

			LPWeb.Model.Company_Web model=new LPWeb.Model.Company_Web();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["EmailAlertsEnabled"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["EmailAlertsEnabled"].ToString()=="1")||(ds.Tables[0].Rows[0]["EmailAlertsEnabled"].ToString().ToLower()=="true"))
					{
						model.EmailAlertsEnabled=true;
					}
					else
					{
						model.EmailAlertsEnabled=false;
					}
				}
				model.EmailRelayServer=ds.Tables[0].Rows[0]["EmailRelayServer"].ToString();
				model.DefaultAlertEmail=ds.Tables[0].Rows[0]["DefaultAlertEmail"].ToString();
				if(ds.Tables[0].Rows[0]["EmailInterval"].ToString()!="")
				{
					model.EmailInterval=int.Parse(ds.Tables[0].Rows[0]["EmailInterval"].ToString());
				}
				model.LPCompanyURL=ds.Tables[0].Rows[0]["LPCompanyURL"].ToString();
				model.BorrowerURL=ds.Tables[0].Rows[0]["BorrowerURL"].ToString();
				model.BorrowerGreeting=ds.Tables[0].Rows[0]["BorrowerGreeting"].ToString();
				model.HomePageLogo=ds.Tables[0].Rows[0]["HomePageLogo"].ToString();
				model.LogoForSubPages=ds.Tables[0].Rows[0]["LogoForSubPages"].ToString();
				if(ds.Tables[0].Rows[0]["HomePageLogoData"].ToString()!="")
				{
					model.HomePageLogoData=(byte[])ds.Tables[0].Rows[0]["HomePageLogoData"];
				}
				if(ds.Tables[0].Rows[0]["SubPageLogoData"].ToString()!="")
				{
					model.SubPageLogoData=(byte[])ds.Tables[0].Rows[0]["SubPageLogoData"];
				}
				return model;
			}
			else
			{
				return null;
			}
		}
		#endregion  成员方法
	}
}

