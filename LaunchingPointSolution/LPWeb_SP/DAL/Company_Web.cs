using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Company_Web。
    /// </summary>
    public class Company_Web : Company_WebBase
    {
        public Company_Web()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.Company_Web model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_Web(");
            strSql.Append("EmailAlertsEnabled,EmailRelayServer,DefaultAlertEmail,EmailInterval,LPCompanyURL,BorrowerURL,BorrowerGreeting,HomePageLogo,LogoForSubPages,HomePageLogoData,SubPageLogoData,EnableEmailAuditTrail,SendEmailViaEWS,EwsUrl,SMTP_Port,AuthReq,AuthEmailAccount,AuthPassword,SMTP_EncryptMethod,EWS_Version,EWS_Domain)");
            strSql.Append(" values (");
            strSql.Append("@EmailAlertsEnabled,@EmailRelayServer,@DefaultAlertEmail,@EmailInterval,@LPCompanyURL,@BorrowerURL,@BorrowerGreeting,@HomePageLogo,@LogoForSubPages,@HomePageLogoData,@SubPageLogoData,@EnableEmailAuditTrail,@SendEmailViaEWS,@EwsUrl,@SMTP_Port,@AuthReq,@AuthEmailAccount,@AuthPassword,@SMTP_EncryptMethod,@EWS_Version,@EWS_Domain)");
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
					new SqlParameter("@SubPageLogoData", SqlDbType.Image),
                    new SqlParameter("@EnableEmailAuditTrail", SqlDbType.Bit),
					new SqlParameter("@SendEmailViaEWS", SqlDbType.Bit,1),
					new SqlParameter("@EwsUrl", SqlDbType.NVarChar,255),
                    new SqlParameter("@SMTP_Port", SqlDbType.Int),
                    new SqlParameter("@AuthReq", SqlDbType.Bit,1),
                    new SqlParameter("@AuthEmailAccount", SqlDbType.NVarChar,255),
                    new SqlParameter("@AuthPassword", SqlDbType.NVarChar,255),
                    new SqlParameter("@SMTP_EncryptMethod", SqlDbType.NVarChar,255),
                    new SqlParameter("@EWS_Version", SqlDbType.NVarChar,255),
                    new SqlParameter("@EWS_Domain", SqlDbType.NVarChar,255)};
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
            parameters[11].Value = model.EnableEmailAuditTrail;
            parameters[12].Value = model.SendEmailViaEWS;
            parameters[13].Value = model.EwsUrl;
            parameters[14].Value = model.SMTP_Port;
            parameters[15].Value = model.AuthReq;
            parameters[16].Value = model.AuthEmailAccount;
            parameters[17].Value = model.AuthPassword;
            parameters[18].Value = model.SMTP_EncryptMethod;
            parameters[19].Value = model.EWS_Version;
            parameters[20].Value = model.EWS_Domain;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Company_Web model)
        {
            StringBuilder strSql = new StringBuilder();
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
            strSql.Append("SubPageLogoData=@SubPageLogoData,");
            strSql.Append("EnableEmailAuditTrail=@EnableEmailAuditTrail,");
            strSql.Append("SendEmailViaEWS=@SendEmailViaEWS,");
            strSql.Append("EwsUrl=@EwsUrl,");
            strSql.Append("SMTP_Port=@SMTP_Port,");
            strSql.Append("AuthReq =@AuthReq,");
            strSql.Append("AuthEmailAccount =@AuthEmailAccount,");
            strSql.Append("AuthPassword =@AuthPassword,");
            strSql.Append("SMTP_EncryptMethod=@SMTP_EncryptMethod,");
            strSql.Append("EWS_Version=@EWS_Version,");
            strSql.Append("EWS_Domain=@EWS_Domain");
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
					new SqlParameter("@SubPageLogoData", SqlDbType.Image),
                    new SqlParameter("@EnableEmailAuditTrail", SqlDbType.Bit),
					new SqlParameter("@SendEmailViaEWS", SqlDbType.Bit,1),
					new SqlParameter("@EwsUrl", SqlDbType.NVarChar,255),
                    new SqlParameter("@SMTP_Port", SqlDbType.Int),
                    new SqlParameter("@AuthReq", SqlDbType.Bit,1),
                    new SqlParameter("@AuthEmailAccount", SqlDbType.NVarChar,255),
                    new SqlParameter("@AuthPassword", SqlDbType.NVarChar,255),
                    new SqlParameter("@SMTP_EncryptMethod", SqlDbType.NVarChar,255),
                    new SqlParameter("@EWS_Version", SqlDbType.NVarChar,255),
                    new SqlParameter("@EWS_Domain", SqlDbType.NVarChar,255)
                    
                                        };
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
            parameters[11].Value = model.EnableEmailAuditTrail;
            parameters[12].Value = model.SendEmailViaEWS;
            parameters[13].Value = model.EwsUrl;
            parameters[14].Value = model.SMTP_Port;
            parameters[15].Value = model.AuthReq;
            parameters[16].Value = model.AuthEmailAccount;
            parameters[17].Value = model.AuthPassword;
            parameters[18].Value = model.SMTP_EncryptMethod;
            parameters[19].Value = model.EWS_Version;
            parameters[20].Value = model.EWS_Domain;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Company_Web ");
            strSql.Append(" where ");
            SqlParameter[] parameters = {
};

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_Web GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from Company_Web ");
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
};

            LPWeb.Model.Company_Web model = new LPWeb.Model.Company_Web();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["EmailAlertsEnabled"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["EmailAlertsEnabled"].ToString() == "1") || (ds.Tables[0].Rows[0]["EmailAlertsEnabled"].ToString().ToLower() == "true"))
                    {
                        model.EmailAlertsEnabled = true;
                    }
                    else
                    {
                        model.EmailAlertsEnabled = false;
                    }
                }
                model.EmailRelayServer = ds.Tables[0].Rows[0]["EmailRelayServer"].ToString();
                model.DefaultAlertEmail = ds.Tables[0].Rows[0]["DefaultAlertEmail"].ToString();
                if (ds.Tables[0].Rows[0]["EmailInterval"].ToString() != "")
                {
                    model.EmailInterval = int.Parse(ds.Tables[0].Rows[0]["EmailInterval"].ToString());
                }
                model.LPCompanyURL = ds.Tables[0].Rows[0]["LPCompanyURL"].ToString();
                model.BorrowerURL = ds.Tables[0].Rows[0]["BorrowerURL"].ToString();
                model.BorrowerGreeting = ds.Tables[0].Rows[0]["BorrowerGreeting"].ToString();
                model.HomePageLogo = ds.Tables[0].Rows[0]["HomePageLogo"].ToString();
                model.LogoForSubPages = ds.Tables[0].Rows[0]["LogoForSubPages"].ToString();
                if (ds.Tables[0].Rows[0]["HomePageLogoData"].ToString() != "")
                {
                    model.HomePageLogoData = (byte[])ds.Tables[0].Rows[0]["HomePageLogoData"];
                }
                if (ds.Tables[0].Rows[0]["SubPageLogoData"].ToString() != "")
                {
                    model.SubPageLogoData = (byte[])ds.Tables[0].Rows[0]["SubPageLogoData"];
                }
                model.BackgroundLoanAlertPage = ds.Tables[0].Rows[0]["BackgroundLoanAlertPage"].ToString();
                if (ds.Tables[0].Rows[0]["EnableEmailAuditTrail"] == DBNull.Value)
                {
                    model.EnableEmailAuditTrail = false;
                }
                else
                {
                    if (ds.Tables[0].Rows[0]["EnableEmailAuditTrail"].ToString() == "True")
                    {
                        model.EnableEmailAuditTrail = true;
                    }
                    else
                    {
                        model.EnableEmailAuditTrail = false;
                    }
                }

                model.BackgroundWCFURL = ds.Tables[0].Rows[0]["BackgroundWCFURL"].ToString();

                if (ds.Tables[0].Rows[0]["SendEmailViaEWS"] == DBNull.Value)
                {
                    model.SendEmailViaEWS = false;
                }
                else
                {
                    if (ds.Tables[0].Rows[0]["SendEmailViaEWS"].ToString() == "True")
                    {
                        model.SendEmailViaEWS = true;
                    }
                    else
                    {
                        model.SendEmailViaEWS = false;
                    }
                }

                model.EwsUrl = ds.Tables[0].Rows[0]["EwsUrl"].ToString();


                model.SMTP_Port = ds.Tables[0].Rows[0]["SMTP_Port"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["SMTP_Port"]) : 25;

                model.AuthReq = ds.Tables[0].Rows[0]["AuthReq"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["AuthReq"]) : false;

                model.AuthEmailAccount = ds.Tables[0].Rows[0]["AuthEmailAccount"] != DBNull.Value ? ds.Tables[0].Rows[0]["AuthEmailAccount"].ToString() : "";

                model.AuthPassword = ds.Tables[0].Rows[0]["AuthPassword"] != DBNull.Value ? ds.Tables[0].Rows[0]["AuthPassword"].ToString() : "";

                model.SMTP_EncryptMethod = ds.Tables[0].Rows[0]["SMTP_EncryptMethod"] != DBNull.Value ? ds.Tables[0].Rows[0]["SMTP_EncryptMethod"].ToString() : "";

                model.EWS_Version = ds.Tables[0].Rows[0]["EWS_Version"] != DBNull.Value ? ds.Tables[0].Rows[0]["EWS_Version"].ToString() : "";
                model.EWS_Domain = ds.Tables[0].Rows[0]["EWS_Domain"] != DBNull.Value ? ds.Tables[0].Rows[0]["EWS_Domain"].ToString() : "";
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EmailAlertsEnabled,EmailRelayServer,DefaultAlertEmail,EmailInterval,LPCompanyURL,BorrowerURL,BorrowerGreeting,HomePageLogo,LogoForSubPages,HomePageLogoData,SubPageLogoData,BackgroundLoanAlertPage,EnableEmailAuditTrail,EWS_Version,EWS_Domain ");
            strSql.Append(" FROM Company_Web ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" EmailAlertsEnabled,EmailRelayServer,DefaultAlertEmail,EmailInterval,LPCompanyURL,BorrowerURL,BorrowerGreeting,HomePageLogo,LogoForSubPages,HomePageLogoData,SubPageLogoData,BackgroundLoanAlertPage,EnableEmailAuditTrail,EWS_Version,EWS_Domain ");
            strSql.Append(" FROM Company_Web ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
            parameters[0].Value = "Company_Web";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        #endregion  成员方法

        public string GetWcfUrl()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT TOP 1 BackgroundWCFURL FROM dbo.Company_Web");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0].IsNull(0) == false)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
            }
            return string.Empty;
        }
    }
}

