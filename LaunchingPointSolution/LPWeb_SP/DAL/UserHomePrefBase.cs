using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类UserHomePref。
    /// </summary>
    public class UserHomePrefBase
    {
        public UserHomePrefBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.UserHomePref model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UserHomePref(");
            strSql.Append("UserId,CompanyCalendar,PipelineChart,SalesBreakdownChart,OrgProductionChart,Org_N_Sales_Charts,RateSummary,GoalsChart,OverDueTaskAlert,Announcements,ExchangeInbox,ExchangeCalendar,AlertFilter,DefaultClientsPipelineViewId,DefaultLoansPipelineViewId,DefaultLeadsPipelineViewId,DashboardLastCompletedStages,QuickLeadForm)");
            strSql.Append(" values (");
            strSql.Append("@UserId,@CompanyCalendar,@PipelineChart,@SalesBreakdownChart,@OrgProductionChart,@Org_N_Sales_Charts,@RateSummary,@GoalsChart,@OverDueTaskAlert,@Announcements,@ExchangeInbox,@ExchangeCalendar,@AlertFilter,@DefaultClientsPipelineViewId,@DefaultLoansPipelineViewId,@DefaultLeadsPipelineViewId,@DashboardLastCompletedStages,@QuickLeadForm)");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@CompanyCalendar", SqlDbType.Bit,1),
					new SqlParameter("@PipelineChart", SqlDbType.Bit,1),
					new SqlParameter("@SalesBreakdownChart", SqlDbType.Bit,1),
					new SqlParameter("@OrgProductionChart", SqlDbType.Bit,1),
					new SqlParameter("@Org_N_Sales_Charts", SqlDbType.Bit,1),
					new SqlParameter("@RateSummary", SqlDbType.Bit,1),
					new SqlParameter("@GoalsChart", SqlDbType.Bit,1),
					new SqlParameter("@OverDueTaskAlert", SqlDbType.Bit,1),
					new SqlParameter("@Announcements", SqlDbType.Bit,1),
					new SqlParameter("@ExchangeInbox", SqlDbType.Bit,1),
					new SqlParameter("@ExchangeCalendar", SqlDbType.Bit,1),
                    new SqlParameter("@AlertFilter",SqlDbType.Int),
                    new SqlParameter("@DefaultClientsPipelineViewId",SqlDbType.Int),
                    new SqlParameter("@DefaultLoansPipelineViewId",SqlDbType.Int),
                    new SqlParameter("@DefaultLeadsPipelineViewId",SqlDbType.Int),
                    new SqlParameter("@DashboardLastCompletedStages",SqlDbType.Int),
                    new SqlParameter("@QuickLeadForm",SqlDbType.Bit,1)
                                        };
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.CompanyCalendar;
            parameters[2].Value = model.PipelineChart;
            parameters[3].Value = model.SalesBreakdownChart;
            parameters[4].Value = model.OrgProductionChart;
            parameters[5].Value = model.Org_N_Sales_Charts;
            parameters[6].Value = model.RateSummary;
            parameters[7].Value = model.GoalsChart;
            parameters[8].Value = model.OverDueTaskAlert;
            parameters[9].Value = model.Announcements;
            parameters[10].Value = model.ExchangeInbox;
            parameters[11].Value = model.ExchangeCalendar;
            parameters[12].Value = model.AlertFilter;
            parameters[13].Value = model.DefaultClientsPipelineViewId;
            parameters[14].Value = model.DefaultLoansPipelineViewId;
            parameters[15].Value = model.DefaultLeadsPipelineViewId;
            parameters[16].Value = model.DashboardLastCompletedStages;
            parameters[17].Value = model.QuickLeadForm;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.UserHomePref model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserHomePref set ");
            strSql.Append("UserId=@UserId,");
            strSql.Append("CompanyCalendar=@CompanyCalendar,");
            strSql.Append("PipelineChart=@PipelineChart,");
            strSql.Append("SalesBreakdownChart=@SalesBreakdownChart,");
            strSql.Append("OrgProductionChart=@OrgProductionChart,");
            strSql.Append("Org_N_Sales_Charts=@Org_N_Sales_Charts,");
            strSql.Append("RateSummary=@RateSummary,");
            strSql.Append("GoalsChart=@GoalsChart,");
            strSql.Append("OverDueTaskAlert=@OverDueTaskAlert,");
            strSql.Append("Announcements=@Announcements,");
            strSql.Append("ExchangeInbox=@ExchangeInbox,");
            strSql.Append("ExchangeCalendar=@ExchangeCalendar,");
            strSql.Append("AlertFilter = @AlertFilter,");
            strSql.Append("DefaultClientsPipelineViewId = @DefaultClientsPipelineViewId,");
            strSql.Append("DefaultLoansPipelineViewId = @DefaultLoansPipelineViewId,");
            strSql.Append("DefaultLeadsPipelineViewId = @DefaultLeadsPipelineViewId,");
            strSql.Append("DashboardLastCompletedStages = @DashboardLastCompletedStages,");
            strSql.Append("QuickLeadForm = @QuickLeadForm");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@CompanyCalendar", SqlDbType.Bit,1),
					new SqlParameter("@PipelineChart", SqlDbType.Bit,1),
					new SqlParameter("@SalesBreakdownChart", SqlDbType.Bit,1),
					new SqlParameter("@OrgProductionChart", SqlDbType.Bit,1),
					new SqlParameter("@Org_N_Sales_Charts", SqlDbType.Bit,1),
					new SqlParameter("@RateSummary", SqlDbType.Bit,1),
					new SqlParameter("@GoalsChart", SqlDbType.Bit,1),
					new SqlParameter("@OverDueTaskAlert", SqlDbType.Bit,1),
					new SqlParameter("@Announcements", SqlDbType.Bit,1),
					new SqlParameter("@ExchangeInbox", SqlDbType.Bit,1),
					new SqlParameter("@ExchangeCalendar", SqlDbType.Bit,1),
                    new SqlParameter("@AlertFilter",SqlDbType.Int),
                    new SqlParameter("@DefaultClientsPipelineViewId",SqlDbType.Int),
                    new SqlParameter("@DefaultLoansPipelineViewId",SqlDbType.Int),
                    new SqlParameter("@DefaultLeadsPipelineViewId",SqlDbType.Int),
                    new SqlParameter("@DashboardLastCompletedStages",SqlDbType.Int),
                    new SqlParameter("@QuickLeadForm",SqlDbType.Bit,1)
                                        };
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.CompanyCalendar;
            parameters[2].Value = model.PipelineChart;
            parameters[3].Value = model.SalesBreakdownChart;
            parameters[4].Value = model.OrgProductionChart;
            parameters[5].Value = model.Org_N_Sales_Charts;
            parameters[6].Value = model.RateSummary;
            parameters[7].Value = model.GoalsChart;
            parameters[8].Value = model.OverDueTaskAlert;
            parameters[9].Value = model.Announcements;
            parameters[10].Value = model.ExchangeInbox;
            parameters[11].Value = model.ExchangeCalendar;
            parameters[12].Value = model.AlertFilter;
            parameters[13].Value = model.DefaultClientsPipelineViewId;
            parameters[14].Value = model.DefaultLoansPipelineViewId;
            parameters[15].Value = model.DefaultLeadsPipelineViewId;
            parameters[16].Value = model.DashboardLastCompletedStages;
            parameters[17].Value = model.QuickLeadForm;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UserHomePref ");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.UserHomePref GetModel(int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserId,CompanyCalendar,PipelineChart,SalesBreakdownChart,OrgProductionChart,Org_N_Sales_Charts,RateSummary,GoalsChart,OverDueTaskAlert,Announcements,ExchangeInbox,ExchangeCalendar,AlertFilter,DefaultClientsPipelineViewId,DefaultLoansPipelineViewId,DefaultLeadsPipelineViewId,DashboardLastCompletedStages,QuickLeadForm from UserHomePref ");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;

            LPWeb.Model.UserHomePref model = new LPWeb.Model.UserHomePref();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CompanyCalendar"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["CompanyCalendar"].ToString() == "1") || (ds.Tables[0].Rows[0]["CompanyCalendar"].ToString().ToLower() == "true"))
                    {
                        model.CompanyCalendar = true;
                    }
                    else
                    {
                        model.CompanyCalendar = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["PipelineChart"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["PipelineChart"].ToString() == "1") || (ds.Tables[0].Rows[0]["PipelineChart"].ToString().ToLower() == "true"))
                    {
                        model.PipelineChart = true;
                    }
                    else
                    {
                        model.PipelineChart = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["SalesBreakdownChart"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["SalesBreakdownChart"].ToString() == "1") || (ds.Tables[0].Rows[0]["SalesBreakdownChart"].ToString().ToLower() == "true"))
                    {
                        model.SalesBreakdownChart = true;
                    }
                    else
                    {
                        model.SalesBreakdownChart = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["OrgProductionChart"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["OrgProductionChart"].ToString() == "1") || (ds.Tables[0].Rows[0]["OrgProductionChart"].ToString().ToLower() == "true"))
                    {
                        model.OrgProductionChart = true;
                    }
                    else
                    {
                        model.OrgProductionChart = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Org_N_Sales_Charts"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Org_N_Sales_Charts"].ToString() == "1") || (ds.Tables[0].Rows[0]["Org_N_Sales_Charts"].ToString().ToLower() == "true"))
                    {
                        model.Org_N_Sales_Charts = true;
                    }
                    else
                    {
                        model.Org_N_Sales_Charts = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["RateSummary"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["RateSummary"].ToString() == "1") || (ds.Tables[0].Rows[0]["RateSummary"].ToString().ToLower() == "true"))
                    {
                        model.RateSummary = true;
                    }
                    else
                    {
                        model.RateSummary = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["GoalsChart"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["GoalsChart"].ToString() == "1") || (ds.Tables[0].Rows[0]["GoalsChart"].ToString().ToLower() == "true"))
                    {
                        model.GoalsChart = true;
                    }
                    else
                    {
                        model.GoalsChart = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["OverDueTaskAlert"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["OverDueTaskAlert"].ToString() == "1") || (ds.Tables[0].Rows[0]["OverDueTaskAlert"].ToString().ToLower() == "true"))
                    {
                        model.OverDueTaskAlert = true;
                    }
                    else
                    {
                        model.OverDueTaskAlert = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Announcements"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Announcements"].ToString() == "1") || (ds.Tables[0].Rows[0]["Announcements"].ToString().ToLower() == "true"))
                    {
                        model.Announcements = true;
                    }
                    else
                    {
                        model.Announcements = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ExchangeInbox"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ExchangeInbox"].ToString() == "1") || (ds.Tables[0].Rows[0]["ExchangeInbox"].ToString().ToLower() == "true"))
                    {
                        model.ExchangeInbox = true;
                    }
                    else
                    {
                        model.ExchangeInbox = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ExchangeCalendar"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ExchangeCalendar"].ToString() == "1") || (ds.Tables[0].Rows[0]["ExchangeCalendar"].ToString().ToLower() == "true"))
                    {
                        model.ExchangeCalendar = true;
                    }
                    else
                    {
                        model.ExchangeCalendar = false;
                    }
                }

                model.AlertFilter = ds.Tables[0].Rows[0]["AlertFilter"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["AlertFilter"]) : 0;

                //gdc CR45
                model.DashboardLastCompletedStages = ds.Tables[0].Rows[0]["DashboardLastCompletedStages"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["DashboardLastCompletedStages"]) : 0;
                model.DefaultClientsPipelineViewId = ds.Tables[0].Rows[0]["DefaultClientsPipelineViewId"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["DefaultClientsPipelineViewId"]) : 0;
                model.DefaultLeadsPipelineViewId = ds.Tables[0].Rows[0]["DefaultLeadsPipelineViewId"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["DefaultLeadsPipelineViewId"]) : 0;
                model.DefaultLoansPipelineViewId = ds.Tables[0].Rows[0]["DefaultLoansPipelineViewId"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["DefaultLoansPipelineViewId"]) : 0;


                if (ds.Tables[0].Rows[0]["QuickLeadForm"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["QuickLeadForm"].ToString() == "1") || (ds.Tables[0].Rows[0]["QuickLeadForm"].ToString().ToLower() == "true"))
                    {
                        model.QuickLeadForm = true;
                    }
                    else
                    {
                        model.QuickLeadForm = false;
                    }
                }


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
            strSql.Append("select UserId,CompanyCalendar,PipelineChart,SalesBreakdownChart,OrgProductionChart,Org_N_Sales_Charts,RateSummary,GoalsChart,OverDueTaskAlert,Announcements,ExchangeInbox,ExchangeCalendar,QuickLeadForm ");
            strSql.Append(" FROM UserHomePref ");
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
            strSql.Append(" UserId,CompanyCalendar,PipelineChart,SalesBreakdownChart,OrgProductionChart,Org_N_Sales_Charts,RateSummary,GoalsChart,OverDueTaskAlert,Announcements,ExchangeInbox,ExchangeCalendar,QuickLeadForm ");
            strSql.Append(" FROM UserHomePref ");
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
            parameters[0].Value = "UserHomePref";
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

