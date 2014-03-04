using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Company_General。
	/// </summary>
	public class Company_GeneralBase
    {
        public Company_GeneralBase()
		{}
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.Company_General model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_General(");
            strSql.Append(" Name, AD_OU_Filter,[Desc],Address, City, State, Zip, ImportUserInterval, ReleaseVersion, Edition, RuleMonitorInterval, GlobalId, EnableMarketing, Phone, Fax, Email, WebURL, IntegrationID, APIKey, LeadStar_ID, LeadStar_username, LeadStar_userid, ActiveLoanWorkflow, StartMarketingSync, MyEmailInboxURL, MyCalendarURL, RatesURL)");
            strSql.Append(" values (");
            strSql.Append("@Name,@AD_OU_Filter,@Desc,@Address,@City,@State,@Zip,@ImportUserInterval,@ReleaseVersion,@Edition,@RuleMonitorInterval,@GlobalId,@EnableMarketing,@Phone,@Fax,@Email,@WebURL,@IntegrationID,@APIKey,@LeadStar_ID,@LeadStar_username,@LeadStar_userid,@ActiveLoanWorkflow,@StartMarketingSync, @MyEmailInboxURL, @MyCalendarURL, @RatesURL)");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@AD_OU_Filter", SqlDbType.NVarChar,50),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@Address", SqlDbType.NVarChar,255),
					new SqlParameter("@City", SqlDbType.NVarChar,100),
					new SqlParameter("@State", SqlDbType.NVarChar,2),
					new SqlParameter("@Zip", SqlDbType.NVarChar,5),
					new SqlParameter("@ImportUserInterval", SqlDbType.SmallInt,2),
					new SqlParameter("@ReleaseVersion", SqlDbType.NVarChar,255),
					new SqlParameter("@Edition", SqlDbType.Int),
					new SqlParameter("@RuleMonitorInterval", SqlDbType.Int),
					new SqlParameter("@GlobalId", SqlDbType.NVarChar,255),
					new SqlParameter("@EnableMarketing", SqlDbType.Bit),
                    new SqlParameter("@Phone", SqlDbType.NVarChar,20),
                    new SqlParameter("@Fax", SqlDbType.NVarChar,20),
                    new SqlParameter("@Email", SqlDbType.NVarChar,255),
                    new SqlParameter("@WebURL", SqlDbType.NVarChar,255),
                    new SqlParameter("@IntegrationID", SqlDbType.NVarChar,255),
                    new SqlParameter("@APIKey", SqlDbType.NVarChar,255),
                    new SqlParameter("@LeadStar_ID", SqlDbType.NVarChar,255),
                    new SqlParameter("@LeadStar_username", SqlDbType.NVarChar,255),
                    new SqlParameter("@LeadStar_userid", SqlDbType.NVarChar,255),
                    new SqlParameter("@ActiveLoanWorkflow", SqlDbType.Bit),
                    new SqlParameter("@StartMarketingSync",SqlDbType.Bit),
                    new SqlParameter("@MyEmailInboxURL",SqlDbType.NVarChar),
                    new SqlParameter("@MyCalendarURL",SqlDbType.NVarChar),
                    new SqlParameter("@RatesURL",SqlDbType.NVarChar)
        };
            parameters[0].Value = model.Name;
            parameters[1].Value = model.AD_OU_Filter;
            parameters[2].Value = model.Desc;
            parameters[3].Value = model.Address;
            parameters[4].Value = model.City;
            parameters[5].Value = model.State;
            parameters[6].Value = model.Zip;
            parameters[7].Value = model.ImportUserInterval;
            parameters[8].Value = model.ReleaseVersion;
            parameters[9].Value = model.Edition;
            parameters[10].Value = model.RuleMonitorInterval;
            parameters[11].Value = model.GlobalId;
            parameters[12].Value = model.EnableMarketing;
            parameters[13].Value = model.Phone;
            parameters[14].Value = model.Fax;
            parameters[15].Value = model.Email;
            parameters[16].Value = model.WebURL;
            parameters[17].Value = model.IntegrationID;
            parameters[18].Value = model.APIKey;
            parameters[19].Value = model.LeadStar_ID;
            parameters[20].Value = model.LeadStar_username;
            parameters[21].Value = model.LeadStar_userid;
            parameters[22].Value = model.ActiveLoanWorkflow;
            parameters[23].Value = model.StartMarketingSync;
            parameters[24].Value = model.MyEmailInboxURL;
            parameters[25].Value = model.MyCalendarURL;
            parameters[26].Value = model.RatesURL;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Company_General model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Company_General set ");
            strSql.Append("[Name]=@Name,");
            strSql.Append("AD_OU_Filter=@AD_OU_Filter,");
            strSql.Append("[Desc]=@Desc,");
            strSql.Append("[Address]=@Address,");
            strSql.Append("[City]=@City,");
            strSql.Append("[State]=@State,");
            strSql.Append("[Zip]=@Zip,");
            strSql.Append("[ImportUserInterval]=@ImportUserInterval,");
            strSql.Append("[ReleaseVersion]=@ReleaseVersion,");
            strSql.Append("[Edition]=@Edition,");
            strSql.Append("[RuleMonitorInterval]=@RuleMonitorInterval,");
            strSql.Append("[GlobalId]=@GlobalId,");
            strSql.Append("[EnableMarketing]=@EnableMarketing,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[Fax]=@Fax,");
            strSql.Append("[Email]=@Email,");
            strSql.Append("[WebURL]=@WebURL,");
            strSql.Append("[IntegrationID]=@IntegrationID,");
            strSql.Append("[APIKey]=@APIKey,");
            strSql.Append("[LeadStar_ID]=@LeadStar_ID,");
            strSql.Append("[LeadStar_username]=@LeadStar_username,");
            strSql.Append("[LeadStar_userid]=@LeadStar_userid,");
            strSql.Append("[ActiveLoanWorkflow] = @ActiveLoanWorkflow, ");
            strSql.Append("[StartMarketingSync] =@StartMarketingSync, ");
            strSql.Append("[MyEmailInboxURL] =@MyEmailInboxURL, ");
            strSql.Append("[MyCalendarURL] =@MyCalendarURL, ");
            strSql.Append("[RatesURL] =@RatesURL ");

            //strSql.Append("Prefix=@Prefix");
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@AD_OU_Filter", SqlDbType.NVarChar,50),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@Address", SqlDbType.NVarChar,255),
					new SqlParameter("@City", SqlDbType.NVarChar,100),
					new SqlParameter("@State", SqlDbType.NVarChar,2),
					new SqlParameter("@Zip", SqlDbType.NVarChar,5),
					new SqlParameter("@ImportUserInterval", SqlDbType.SmallInt,2),
					new SqlParameter("@ReleaseVersion", SqlDbType.NVarChar,255),
					new SqlParameter("@Edition", SqlDbType.Int),
					new SqlParameter("@RuleMonitorInterval", SqlDbType.Int),
					new SqlParameter("@GlobalId", SqlDbType.NVarChar,255),
					new SqlParameter("@EnableMarketing", SqlDbType.Bit),
                    new SqlParameter("@Phone", SqlDbType.NVarChar,20),
                    new SqlParameter("@Fax", SqlDbType.NVarChar,20),
                    new SqlParameter("@Email", SqlDbType.NVarChar,255),
                    new SqlParameter("@WebURL", SqlDbType.NVarChar,255),
                    new SqlParameter("@IntegrationID", SqlDbType.NVarChar,255),
                    new SqlParameter("@APIKey", SqlDbType.NVarChar,255),
                    new SqlParameter("@LeadStar_ID", SqlDbType.NVarChar,255),
                    new SqlParameter("@LeadStar_username", SqlDbType.NVarChar,255),
                    new SqlParameter("@LeadStar_userid", SqlDbType.NVarChar,255),
                    new SqlParameter("@ActiveLoanWorkflow",SqlDbType.Bit),
                    new SqlParameter("@StartMarketingSync",SqlDbType.Bit),
                    new SqlParameter("@MyEmailInboxURL",SqlDbType.NVarChar),
                    new SqlParameter("@MyCalendarURL",SqlDbType.NVarChar),
                    new SqlParameter("@RatesURL",SqlDbType.NVarChar)
                                        };
            parameters[0].Value = model.Name;
            parameters[1].Value = model.AD_OU_Filter;
            parameters[2].Value = model.Desc;
            parameters[3].Value = model.Address;
            parameters[4].Value = model.City;
            parameters[5].Value = model.State;
            parameters[6].Value = model.Zip;
            parameters[7].Value = model.ImportUserInterval;
            parameters[8].Value = model.ReleaseVersion;
            parameters[9].Value = model.Edition;
            parameters[10].Value = model.RuleMonitorInterval;
            parameters[11].Value = model.GlobalId;
            parameters[12].Value = model.EnableMarketing;
            parameters[13].Value = model.Phone;
            parameters[14].Value = model.Fax;
            parameters[15].Value = model.Email;
            parameters[16].Value = model.WebURL;
            parameters[17].Value = model.IntegrationID;
            parameters[18].Value = model.APIKey;
            parameters[19].Value = model.LeadStar_ID;
            parameters[20].Value = model.LeadStar_username;
            parameters[21].Value = model.LeadStar_userid;
            parameters[22].Value = model.ActiveLoanWorkflow;
            parameters[23].Value = model.StartMarketingSync;
            parameters[24].Value = model.MyEmailInboxURL;
            parameters[25].Value = model.MyCalendarURL;
            parameters[26].Value = model.RatesURL;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Company_General ");
            strSql.Append(" where ");
            SqlParameter[] parameters = {
};

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_General GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Name,AD_OU_Filter,[Desc],Address,City,State,Zip,ImportUserInterval,ReleaseVersion,Edition,RuleMonitorInterval,GlobalId,EnableMarketing,Phone,Fax,Email,WebURL,IntegrationID,APIKey,LeadStar_ID,LeadStar_username,LeadStar_userid,ActiveLoanWorkflow,StartMarketingSync, MyEmailInboxURL, MyCalendarURL, RatesURL from Company_General ");
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
};

            LPWeb.Model.Company_General model = new LPWeb.Model.Company_General();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                model.AD_OU_Filter = ds.Tables[0].Rows[0]["AD_OU_Filter"].ToString();
                model.Desc = ds.Tables[0].Rows[0]["Desc"].ToString();
                model.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                model.City = ds.Tables[0].Rows[0]["City"].ToString();
                model.State = ds.Tables[0].Rows[0]["State"].ToString();
                model.Zip = ds.Tables[0].Rows[0]["Zip"].ToString();
                if (ds.Tables[0].Rows[0]["ImportUserInterval"].ToString() != "")
                {
                    model.ImportUserInterval = int.Parse(ds.Tables[0].Rows[0]["ImportUserInterval"].ToString());
                }
                model.ReleaseVersion = ds.Tables[0].Rows[0]["ReleaseVersion"].ToString();
                if (ds.Tables[0].Rows[0]["Edition"].ToString() != "")
                {
                    model.Edition = int.Parse(ds.Tables[0].Rows[0]["Edition"].ToString());
                }
                model.GlobalId = ds.Tables[0].Rows[0]["GlobalId"].ToString();
                if (ds.Tables[0].Rows[0]["RuleMonitorInterval"].ToString() != "")
                {
                    model.RuleMonitorInterval = int.Parse(ds.Tables[0].Rows[0]["RuleMonitorInterval"].ToString());
                }
                model.EnableMarketing = ds.Tables[0].Rows[0]["EnableMarketing"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["EnableMarketing"]) : true;
                model.Phone = ds.Tables[0].Rows[0]["Phone"].ToString();
                model.Fax = ds.Tables[0].Rows[0]["Fax"].ToString();
                model.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                model.WebURL = ds.Tables[0].Rows[0]["WebURL"].ToString();
                model.IntegrationID = ds.Tables[0].Rows[0]["IntegrationID"].ToString();
                model.APIKey = ds.Tables[0].Rows[0]["APIKey"].ToString();
                model.LeadStar_ID = ds.Tables[0].Rows[0]["LeadStar_ID"].ToString();
                model.LeadStar_username = ds.Tables[0].Rows[0]["LeadStar_username"].ToString();
                model.LeadStar_userid = ds.Tables[0].Rows[0]["LeadStar_userid"].ToString();
                model.ActiveLoanWorkflow = ds.Tables[0].Rows[0]["ActiveLoanWorkflow"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["ActiveLoanWorkflow"]) : true;
                model.StartMarketingSync = ds.Tables[0].Rows[0]["StartMarketingSync"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["StartMarketingSync"]) : true;

                model.MyEmailInboxURL = ds.Tables[0].Rows[0]["MyEmailInboxURL"].ToString();
                model.MyCalendarURL = ds.Tables[0].Rows[0]["MyCalendarURL"].ToString();
                model.RatesURL = ds.Tables[0].Rows[0]["RatesURL"].ToString();
                
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
            strSql.Append("select Name,AD_OU_Filter,[Desc],Address,City,State,Zip,ImportUserInterval,Prefix ");
            strSql.Append(" FROM Company_General ");
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
            strSql.Append(" SELECT [Name],[AD_OU_Filter],[Desc],[Address],[City],[State],[Zip],[ImportUserInterval],[ReleaseVersion],[Edition],[RuleMonitorInterval],[GlobalId],[EnableMarketing],[Phone],[Fax],[WebURL],[IntegrationID],[APIKey],[Email],[ActiveLoanWorkflow],[LeadStar_ID],[LeadStar_username],[LeadStar_userid],[StartMarketingSync], MyEmailInboxURL, MyCalendarURL, RatesURL ");
            strSql.Append(" FROM Company_General ");
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
            parameters[0].Value = "Company_General";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        #endregion  成员方法
	}
}

